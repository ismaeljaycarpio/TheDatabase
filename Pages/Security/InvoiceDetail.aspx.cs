using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Mail;

public partial class Pages_Security_InvoiceDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iInvoiceID;
    string _qsMode = "";
    string _qsInvoiceID = "";
    User _objUser;

  

    protected void Page_Load(object sender, EventArgs e)
    {
        //int iTemp = 0;
        _objUser = (User)Session["User"];
        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
            { Response.Redirect("~/Default.aspx", false); }
            PopulateAccountTypeDDL();
            PopulateAccountDDL();
            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Invoice.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Invoice.aspx";
            }
        }
        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;


                if (Request.QueryString["InvoiceID"] != null)
                {

                    _qsInvoiceID = Cryptography.Decrypt(Request.QueryString["InvoiceID"]);

                    _iInvoiceID = int.Parse(_qsInvoiceID);


                  


                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
      


        // checking permission

        string strTitle = "Invoice Detail";
        switch (_strActionMode.ToLower())
        {
            case "add":
                divSendInvoice.Visible = false;
                strTitle = "Add Invoice";
                break;

            case "view":


                strTitle = "View Invoice";
                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Invoice";

                if (!IsPostBack)
                {
                    PopulateTheRecord();
                    divDelete.Visible = true;
                }
                break;


            default:
                //?

                break;
        }

        Title = strTitle;
        lblTitle.Text = strTitle;

//        string strFancy = @"  $(function () {
//            $("".popuplink"").fancybox({
//                scrolling: 'auto',
//                type: 'iframe',
//                width: 600,
//                height: 650,
//                titleShow: false
//            });
//        });   ";


//        ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);

    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {

            Invoice theInvoice = InvoiceManager.ets_Invoice_Detail((int)_iInvoiceID);

            if (theInvoice.PaidDate!=null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This is a paid Invoice, we can not delete a paid Invoice. To delete this Invoice please update it as non paid and then try to delete again.');", true);
                return;
            }
            else
            {

                InvoiceManager.ets_Invoice_Delete((int)_iInvoiceID);
                Response.Redirect(hlBack.NavigateUrl, false);
            }
        }
        catch (Exception ex)
        {
            if (ex is SqlException)
            {
                lblMsg.Text = "Delete failed! Please try again.";
            }
            else
            {

                lblMsg.Text = ex.Message;
            }
        }


    }



  

    protected void PopulateTheRecord()
    {
        try
        {
            ddlAccount.Visible = false;

            Invoice theInvoice = InvoiceManager.ets_Invoice_Detail((int)_iInvoiceID);

            lblInvoiceID.Text = theInvoice.InvoiceID.ToString();
            if( theInvoice.AccountID!=null)
            lblAccountID.Text = theInvoice.AccountID.ToString();
            lblAccountName.Text = theInvoice.AccountName;

            if (theInvoice.AccountTypeID != null)
                ddlAccountType.Text = theInvoice.AccountTypeID.ToString();

            if (theInvoice.NetAmountAUD != null)
                txtNetAmountAUD.Text = theInvoice.NetAmountAUD.ToString();

            if (theInvoice.GSTAmountAUD != null)
                txtGSTAmountAUD.Text = theInvoice.GSTAmountAUD.ToString();

            if (theInvoice.GrossAmountAUD != null)
                txtGrossAmountAUD.Text = theInvoice.GrossAmountAUD.ToString();

            if (theInvoice.InvoiceDate != null)
            {
                txtInvoiceDate.Text = theInvoice.InvoiceDate.Value.Day.ToString() + "/" + theInvoice.InvoiceDate.Value.Month.ToString() + "/" + theInvoice.InvoiceDate.Value.Year.ToString();

            }

            if (theInvoice.StartDate != null)
            {
                txtStartDate.Text = theInvoice.StartDate.Value.Day.ToString() + "/" + theInvoice.StartDate.Value.Month.ToString() + "/" + theInvoice.StartDate.Value.Year.ToString();

            }

            if (theInvoice.EndDate != null)
            {
                txtEndDate.Text = theInvoice.EndDate.Value.Day.ToString() + "/" + theInvoice.EndDate.Value.Month.ToString() + "/" + theInvoice.EndDate.Value.Year.ToString();

            }

            ddlPaymentMethod.Text = theInvoice.PaymentMethod.Trim();

            if (theInvoice.PaidDate != null)
            {
                txtPaidDate.Text = theInvoice.PaidDate.Value.Day.ToString() + "/" + theInvoice.PaidDate.Value.Month.ToString() + "/" + theInvoice.PaidDate.Value.Year.ToString();

            }


            txtNotes.Text = theInvoice.Notes;
            txtOrganisationName.Text = theInvoice.OrganisationName;
            txtBillingEmail.Text = theInvoice.BillingEmail;
            txtBillingAddress.Text = theInvoice.BillingAddress;
            txtCountry.Text = theInvoice.Country;
            txtClientRef.Text = theInvoice.ClientRef;
          
            

            if (theInvoice.PaypalID != null && theInvoice.PaypalID!=-1)
            {
                //lets find the paypal detail
                lblPaypalID.Text = theInvoice.PaypalID.ToString();
                Paypal thePayPal = SecurityManager.PayPal_Details((int)theInvoice.PaypalID);
                if (thePayPal != null)
                {
                    trPayPalDetail.Visible = true;

                    string strPayPalDetail = "";
                    strPayPalDetail = strPayPalDetail + "<b>txn_id: </b>" + thePayPal.txn_id + "</br>";
                    strPayPalDetail = strPayPalDetail + "<b>Invoice_status: </b>" + thePayPal.payment_status + "</br>";
                    //strPayPalDetail = strPayPalDetail + "<b>pending_reason: </b>" + thePayPal.pending_reason + "</br>";
                    strPayPalDetail = strPayPalDetail + "<b>payer_email: </b>" + thePayPal.payer_email + "</br>";
                    strPayPalDetail = strPayPalDetail + "<b>receiver_email: </b>" + thePayPal.receiver_email + "</br>";
                    strPayPalDetail = strPayPalDetail + "<b>mc_gross: </b>" + thePayPal.mc_gross.ToString()+ "</br>";
                    strPayPalDetail = strPayPalDetail + "<b>DateAdded: </b>" + thePayPal.DateAdded.Value.ToString() + "</br>";
                    strPayPalDetail = strPayPalDetail + "<b>txn_type: </b>" + thePayPal.txn_type + "</br>";

                    lblPayPalDetail.Text = Server.HtmlDecode(strPayPalDetail);
                }
            }


            if (_strActionMode == "edit")
            {
                ViewState["theInvoice"] = theInvoice;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/InvoiceDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&InvoiceID=" + Cryptography.Encrypt(theInvoice.InvoiceID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }
                else
                {
                    hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/InvoiceDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&InvoiceID=" + Cryptography.Encrypt(theInvoice.InvoiceID.ToString());
                }
            }
        }
        catch (Exception ex)
        {
           
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {
        ddlAccountType.Enabled = p_bEnable;

        txtNetAmountAUD.Enabled = p_bEnable;
        txtGSTAmountAUD.Enabled = p_bEnable;
        txtGrossAmountAUD.Enabled = p_bEnable;
        txtInvoiceDate.Enabled = p_bEnable;
        txtStartDate.Enabled = p_bEnable;
        txtEndDate.Enabled = p_bEnable;
        ddlPaymentMethod.Enabled = p_bEnable;
        txtPaidDate.Enabled = p_bEnable;
        txtNotes.Enabled = p_bEnable;
        txtOrganisationName.Enabled = p_bEnable;
        txtBillingEmail.Enabled = p_bEnable;
        txtBillingAddress.Enabled = p_bEnable;
        txtCountry.Enabled = p_bEnable;
        txtClientRef.Enabled = p_bEnable;       
       
            

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action
    

        return true;
    }




    protected void lnkSendInvoice_Click(object sender, EventArgs e)
    {

        Content theContent = SystemData.Content_Details_ByKey("Invoice_Content",null);
        Invoice theInvoice = InvoiceManager.ets_Invoice_Detail((int)_iInvoiceID);

        if (theInvoice != null)
        {
            if (theInvoice.BillingEmail.Trim() == "")
            {
                txtBillingEmail.Focus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Please enter billing email.');", true);
                return;
            }
        }

        if (theContent != null && theInvoice!=null)
        {

            string strEmail = SystemData.SystemOption_ValueByKey_Account("EmailFrom",null,null);
            string strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
            string strEmailUserName = SystemData.SystemOption_ValueByKey_Account("EmailUserName", null, null);
            string strEmailPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(strEmail);

            msg.Subject = theContent.Heading;
            msg.IsBodyHtml = true;

            theContent.ContentP = theContent.ContentP.Replace("[InvoiceID]", theInvoice.InvoiceID.ToString());
            theContent.ContentP = theContent.ContentP.Replace("[InvoiceDate]", theInvoice.InvoiceDate.Value.ToShortDateString());

            if (theInvoice.AccountID != null)
            {
                Account theAccount = SecurityManager.Account_Details((int)theInvoice.AccountID);
                if (theAccount != null)
                {
                    theContent.ContentP = theContent.ContentP.Replace("[ClientName]", theAccount.BillingFirstName + " " + theAccount.BillingLastName);
                }
            }
            theContent.ContentP = theContent.ContentP.Replace("[ContactAddress]", theInvoice.BillingAddress.ToString());
            theContent.ContentP = theContent.ContentP.Replace("[ClientRef]", theInvoice.ClientRef.ToString());
            theContent.ContentP = theContent.ContentP.Replace("[AccountName]", theInvoice.AccountName.ToString());
            if(theInvoice.StartDate!=null)
                theContent.ContentP = theContent.ContentP.Replace("[StartDate]", theInvoice.StartDate.Value.ToShortDateString());
            if (theInvoice.EndDate != null)
                theContent.ContentP = theContent.ContentP.Replace("[EndDate]", theInvoice.EndDate.Value.ToShortDateString());
            if(theInvoice.NetAmountAUD!=null)
            theContent.ContentP = theContent.ContentP.Replace("[NetAmountAUD]", theInvoice.NetAmountAUD.ToString());
            if (theInvoice.GSTAmountAUD != null)
                theContent.ContentP = theContent.ContentP.Replace("[GSTAmountAUD]", theInvoice.GSTAmountAUD.ToString());
            if (theInvoice.GrossAmountAUD != null)
                theContent.ContentP = theContent.ContentP.Replace("[GrossAmountAUD]", theInvoice.GrossAmountAUD.ToString());

           
            msg.Body = theContent.ContentP;// Sb.ToString();
            msg.To.Add(theInvoice.BillingEmail);
            SmtpClient smtpClient = new SmtpClient(strEmailServer);
            smtpClient.Timeout = 99999;
            smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
            smtpClient.Port = DBGurus.StringToInt(SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null));
            smtpClient.EnableSsl = Convert.ToBoolean(SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null));
            //Send BCC to Global user              

            List<User> lstGlobalUser = SecurityManager.User_ByRoleType("1");

            foreach (User oUser in lstGlobalUser)
            {
                MailAddress bcc = new MailAddress(oUser.Email);
                msg.Bcc.Add(bcc);
            }
            try
            {
#if (!DEBUG)
                smtpClient.Send(msg);
#endif

                if (msg.To.Count > 0)
                {
                    //Guid guidNew = Guid.NewGuid();
                    //string strEmailUID = guidNew.ToString();

                    //EmailLog theEmailLog = new EmailLog(null, int.Parse(Session["AccountID"].ToString()), msg.Subject,
                    //  msg.To[0].ToString(), DateTime.Now, null,
                    //  theInvoice.InvoiceID,
                    //  "Send Invoice", msg.Body);
                    //theEmailLog.EmailUID = strEmailUID;
                    //EmailManager.dbg_EmailLog_Insert(theEmailLog, null, null);


                }



                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmailSent", "alert('" + "Email sent." + "');", true);

            }
            catch (Exception ex)
            {

                //LblMessage.Text = "Sorry we have had a problem sending the email. Please contact us to resolve this issue.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmailSent", "alert('" + "Sorry we have had a problem sending the email. Please contact us to resolve this issue." + "');", true);
                return;
            }
                       
           



        }

    }


    protected void lnkSave_Click(object sender, EventArgs e)
    {

      


        try
        {
            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        Invoice newInvoice = new Invoice(null, int.Parse(ddlAccount.SelectedValue), ddlAccount.SelectedItem.Text,
                            int.Parse(ddlAccountType.SelectedValue), txtNetAmountAUD.Text == "" ? null : (Double?)Double.Parse(txtNetAmountAUD.Text),
                            txtGSTAmountAUD.Text == "" ? null : (Double?)Double.Parse(txtGSTAmountAUD.Text),
                            txtGrossAmountAUD.Text == "" ? null : (Double?)Double.Parse(txtGrossAmountAUD.Text),
                            txtInvoiceDate.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtInvoiceDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                            txtStartDate.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtStartDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                            txtEndDate.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtEndDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                            ddlPaymentMethod.SelectedValue, null,
                            txtPaidDate.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtPaidDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                            txtNotes.Text, txtOrganisationName.Text, txtBillingEmail.Text, txtBillingAddress.Text,
                            txtCountry.Text, txtClientRef.Text, null, null);

                        InvoiceManager.ets_Invoice_Insert(newInvoice);



                       
                        


                        break;

                    case "view":


                        break;

                    case "edit":
                        Invoice editInvoice = (Invoice)ViewState["theInvoice"];


                        //update expiry date
                        //Account editAccount = SecurityManager.Account_Details(int.Parse(ddlAccountName.SelectedValue), tn, null);

                        //int iMonthDifferent = int.Parse(txtMonthsPaid.Text) - (int)editInvoice.MonthsPaid;
                        //if (editAccount != null)
                        //{
                        //    if ((bool)editInvoice.IsPaid == true)
                        //    {
                        //        if (chkPaid.Checked)
                        //        {
                        //            editAccount.ExpiryDate = editAccount.ExpiryDate.Value.AddMonths(iMonthDifferent);
                        //        }
                        //        else
                        //        {
                        //            editAccount.ExpiryDate = editAccount.ExpiryDate.Value.AddMonths(-int.Parse(txtMonthsPaid.Text));
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (chkPaid.Checked)
                        //        {
                        //            if (editAccount.ExpiryDate != null)
                        //            {
                        //                editAccount.ExpiryDate = editAccount.ExpiryDate.Value.AddMonths(int.Parse(txtMonthsPaid.Text));
                        //            }
                        //            else
                        //            {
                        //                editAccount.ExpiryDate = DateTime.Today.AddMonths(int.Parse(txtMonthsPaid.Text));
                        //            }
                        //        }
                        //        else
                        //        {
                        //            //do nothing
                        //        }

                        //    }

                        //    SecurityManager.Account_Update(editAccount, tn);

                        //}

                        //now lets update the Invoice info
                        editInvoice.AccountTypeID = int.Parse(ddlAccountType.SelectedValue);

                        editInvoice.NetAmountAUD = txtNetAmountAUD.Text == "" ? null : (Double?)Double.Parse(txtNetAmountAUD.Text);
                        editInvoice.GSTAmountAUD = txtGSTAmountAUD.Text == "" ? null : (Double?)Double.Parse(txtGSTAmountAUD.Text);
                        editInvoice.GrossAmountAUD = txtGrossAmountAUD.Text == "" ? null : (Double?)Double.Parse(txtGrossAmountAUD.Text);

                        editInvoice.InvoiceDate = txtInvoiceDate.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtInvoiceDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);
                        editInvoice.StartDate = txtStartDate.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtStartDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);
                        editInvoice.EndDate = txtEndDate.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtEndDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);

                        editInvoice.PaymentMethod= ddlPaymentMethod.SelectedValue;

                        editInvoice.PaidDate = txtPaidDate.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtPaidDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);

                        editInvoice.Notes = txtNotes.Text;
                        editInvoice.OrganisationName = txtOrganisationName.Text;
                        editInvoice.BillingEmail = txtBillingEmail.Text;
                        editInvoice.BillingAddress = txtBillingAddress.Text;
                        editInvoice.Country = txtCountry.Text;
                        editInvoice.ClientRef = txtClientRef.Text;
                       
                       
                        InvoiceManager.ets_Invoice_Update(editInvoice);


                        break;

                    default:
                        //?
                        break;
                }
            }
            else
            {
                //user input is not ok

            }

           
            Response.Redirect(hlBack.NavigateUrl, false);

        }
        catch (Exception ex)
        {
         
            lblMsg.Text = ex.Message;
        }

     

    }

  
    //protected void cmdBack_Click(object sender, ImageClickEventArgs e)
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Invoice.aspx", false);
    }


    //protected void PopulateAccountDDL()
    //{
    //    int iTN = 0;
    //    ddlAccountName.DataSource =Common.DataTableFromText("SELECT AccountID,AccountName FROM Account") ;

    //    ddlAccountName.DataBind();
    //    ListItem liNone = new ListItem("None", "-1");
    //    ddlAccountName.Items.Insert(0, liNone);

    //}

    protected void PopulateAccountTypeDDL()
    {
     
        ddlAccountType.DataSource = Common.DataTableFromText("SELECT * FROM AccountType");

        ddlAccountType.DataBind();
       

    }

    protected void PopulateAccountDDL()
    {

        ddlAccount.DataSource = Common.DataTableFromText("SELECT * FROM Account ORDER BY AccountName");

        ddlAccount.DataBind();

        ListItem liPleaseSelect = new ListItem("--Please Select--", "");
        ddlAccount.Items.Insert(0, liPleaseSelect);

    }


    protected void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAccount.SelectedValue != "")
        {
            Account theAccount = SecurityManager.Account_Details(int.Parse(ddlAccount.SelectedValue));
            if (theAccount != null)
            {
                lblAccountID.Text = theAccount.AccountID.ToString();
                txtBillingAddress.Text = theAccount.BillingAddress;
                txtBillingEmail.Text = theAccount.BillingEmail;
                txtClientRef.Text = theAccount.ClientRef;
                if (theAccount.CountryID != null)
                {
                    LookUpData theCountry = SystemData.LookUpData_Detail((int)theAccount.CountryID);
                    if (theCountry != null)
                    {
                        txtCountry.Text = theCountry.Value;
                    }
                }
                else
                {
                    txtCountry.Text = "";
                }

                txtOrganisationName.Text = theAccount.OrganisationName;

                txtInvoiceDate.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();

            }

        }
    }
}
