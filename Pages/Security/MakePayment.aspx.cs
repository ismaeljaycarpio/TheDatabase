using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;

public partial class Pages_Security_MakePayment : SecurePage
{
    User _objUser;
    int _iInvoiceID = -1;
   
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["InvoiceID"] == null)
        {
            _iInvoiceID = -1;
        }
        else
        {
            _iInvoiceID = int.Parse(Request.QueryString["InvoiceID"].ToString());
        }

        Title = "ETS - Payment";
        _objUser = (User)Session["User"];
       

        if (!IsPostBack)
        {


          

           Content theContent;
       
            theContent = SystemData.Content_Details_ByKey(Cryptography.Decrypt(Request.QueryString["PaymentInfo"].ToString()),null);
            if (theContent != null)
            {
                if (theContent.ContentKey == "Payment_MakePayment")
                {

                    tdPayLater.Visible = false;
                    divCancle.Visible = true;
                }
                else if (theContent.ContentKey == "Payment_TrialExpired")
                {

                    tdPayLater.Visible = true;
                    divCancle.Visible = false;

                    if (Session["DoNotAllow"] != null)
                    {
                        if (Session["DoNotAllow"].ToString() == "true")
                        {
                            tdPayLater.Visible = false;

                        }

                    }

                }
                else
                {
                    tdPayLater.Visible = true;
                    divCancle.Visible = false;
                }

                Account theAccount=SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

                if (theAccount.ExpiryDate != null)
                {
                    theContent.ContentP = theContent.ContentP.Replace("[ExpiryDate]", theAccount.ExpiryDate.Value.ToShortDateString());
                }

                if (theAccount.CountryID != null)
                {
                    DataTable dtTemp = Common.DataTableFromText("SELECT LookUpDataID FROM LookUpData WHERE Value='Australia' AND LookupDataID=" + theAccount.CountryID.ToString());
                    if (dtTemp.Rows.Count > 0)
                    {
                        hfAustralian.Value = "y";
                    }
                    else
                    {
                        hfAustralian.Value = "n";
                      
                    }

                }

                lblContent.Text = theContent.ContentP;

                if (theAccount.AccountTypeID != null)
                {
                    AccountType theAccountType = SecurityManager.AccountType_Details((int)theAccount.AccountTypeID);

                    lblAccountInfo.Text = theAccountType.AccountTypeName;
                }
                if ((bool)theAccount.Alerts)
                {
                    lblAccountInfo.Text = lblAccountInfo.Text + " " + "+ Alerts";
                }
                if ((bool)theAccount.ReportGen)
                {
                    lblAccountInfo.Text = lblAccountInfo.Text + " " + "+ Report Gen";
                }


            }
             
            

            lblEFTDetail.Text = @"The details for <b>Electronic Fund Transfer:</b></br>
                   <b>Account Name:</b>  Carbon Monitoring Pty Ltd </br>
                    <b>BSB:</b> 802124</br>
                   <b> Account Number:</b> 96031</br>
                   <b> Reference:</b> " + Session["AccountID"].ToString() + @"</br>" +
                    @"<b>Bank Name:</b> Horizon Credit Union </br>
                    <b>Bank Address:</b> PO Box 280, Wollongong, NSW, 2520
                    ";

            lblSwiftCode.Text = "<b>Swift Payment Code:</b> CUSCAU2S";

            lblCheque.Text=@"<b>Payable To:</b>  Carbon Monitoring Pty Ltd </br>
                            <b>Address:</b> 3/61 Moore Street, Austinmer, NSW 2515, Australia
                            ";

        }



       


    }

    

   


    protected void lnkPayNow_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (UserInputOk() == false)
            return;
        InvoiceUpdate();

        if (optPayPal.Checked)
        {
            if (optMonthly.Checked)
            {

                Response.Redirect("~/Pages/Security/PayPal/sendpayment_recurring.aspx", false);
            }
            else if (optOnce.Checked)
            {
                Session["item_name"] = "ETS 5 Years Service";
                Response.Redirect("~/Pages/Security/PayPal/sendpayment.aspx", false);
            }
            else if (optYearly.Checked)
            {
                Session["item_name"] = "ETS Yearly Service";
                Response.Redirect("~/Pages/Security/PayPal/sendpayment.aspx", false);
            }

        }
    }

    protected void lnkInvoiceMade_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (UserInputOk() == false)
            return;

        InvoiceUpdate();
       
        Response.Redirect("~/Default.aspx", false);
               
    }

    protected double GetPaymentAmount()
    {
        if (hfAustralian.Value == "y")
        {

            if (optMonthly.Checked)
            {
                return 544.5;
            }
            else if (optYearly.Checked)
            {
                return 5445;
            }
            else
            {
                return 10890;
            }
        }
        else
        {

            if (optMonthly.Checked)
            {
                return 495;
            }
            else if (optYearly.Checked)
            {
                return 4950;
            }
            else
            {
                return 9900;
            }


        }
     

    }

    protected bool UserInputOk()
    {
        if (optEFT.Checked)
        {
            if (txtTransactionRef.Text.Trim() == "")
            {
                txtTransactionRef.Focus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please enter Transaction Ref.');", true);
                return false;
            }
        }
        return true;
    }

    protected void InvoiceUpdate()
    {

        lblMsg.Text = "";
        try
        {
            string strPaymentMethod = "P";
            if (optEFT.Checked)
                strPaymentMethod = "E";

            if (optCheque.Checked)
                strPaymentMethod = "C";

            int iMonthsPaid=1;
            if (optYearly.Checked)
                    iMonthsPaid=12;

            if (optOnce.Checked)
                iMonthsPaid = 60;

            if (_iInvoiceID == -1)
            {

                double dCost = GetPaymentAmount();

              
                
                Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

                LookUpData theCountry = SystemData.LookUpData_Detail((int)theAccount.CountryID);

                DateTime? datePaidDate = null;
                if (strPaymentMethod == "P")
                {


                }
                else
                {
                    datePaidDate = DateTime.Now;
                }


                Invoice theInvoice = new Invoice(null, theAccount.AccountID, theAccount.AccountName,
                      theAccount.AccountTypeID, dCost, 0, dCost , DateTime.Now, DateTime.Now,
                      DateTime.Now.AddMonths(iMonthsPaid)
                      , strPaymentMethod, null, datePaidDate, txtTransactionRef.Text, theAccount.OrganisationName, theAccount.BillingEmail,
                      theAccount.BillingEmail, theCountry.Value, "", DateTime.Now, DateTime.Now);

                int iInvoiceID = InvoiceManager.ets_Invoice_Insert(theInvoice);
                Session["InvoiceID"] = iInvoiceID.ToString();
                Session["PaymentAmount"] = dCost.ToString();
            }
            else
            {
                Invoice theInvoice = InvoiceManager.ets_Invoice_Detail(_iInvoiceID);

                if (theInvoice != null)
                {
                    theInvoice.StartDate = DateTime.Now;
                    theInvoice.EndDate = DateTime.Now.AddMonths(iMonthsPaid);
                    theInvoice.PaidAmount = GetPaymentAmount();
                    theInvoice.PaymentMethod = strPaymentMethod;
                    if (optPayPal.Checked)
                    {
                        Session["InvoiceID"] = _iInvoiceID.ToString();
                        Session["PaymentAmount"] = theInvoice.PaidAmount.ToString();
                    }

                    if (strPaymentMethod == "P")
                    {


                    }
                    else
                    {
                        theInvoice.PaidDate = DateTime.Now;
                    }

                    InvoiceManager.ets_Invoice_Update(theInvoice);

                }
            }

            Session["DoNotAllow"] = null;
            Session["ExpireLeftDay"] = null;
                
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message + " " + ex.StackTrace;
        }
    }

}