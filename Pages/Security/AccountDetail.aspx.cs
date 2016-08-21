using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

public partial class Pages_Security_AccountDetail : SecurePage
{

    protected string returnValue = String.Empty;
    string _strActionMode = "view";
    int? _iAccountID=-1 ;
    string _qsMode = "";
    string _qsAccountID = "";
    Common_Pager _gvPager;
    bool _bIsAdmin = false;
    bool _bGod = false;
    protected void Page_Load(object sender, EventArgs e)
    {

        // checking action mode


        if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        {
            _bIsAdmin = true;
            divAccountDetail.Visible = true;
        }
        else
        {
            TabContainer1.Tabs[1].Visible = false;
            TabContainer1.Tabs[2].Visible = false;
            TabContainer1.Tabs[3].Visible = false;
            divAccountDetail.Visible = false;
        }

        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            _bGod = true;
            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountList.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {
                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountList.aspx";
            }
            //divCheckPayment.Visible = false;
            divDemoEmail.Visible = true;
            //trLogo.Visible = true;
        }
        else
        {
                //trLogo.Visible = false;
              hlBack.NavigateUrl= "~/Default.aspx";

              User objUser = (User)Session["User"];

              if (objUser.Email.ToLower() == SystemData.SystemOption_ValueByKey_Account("DemoAccountCreator",_iAccountID,null))
              {
                  divDemoEmail.Visible = true;
              }


        }



        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            trPassword.Visible = true;
            trMasterPage.Visible = true;
            //txtAccountType.Visible = false;
        }
        else
        {
            trMasterPage.Visible = false;
            txtAccountName.Visible = false;
            txtExpiryDate.Visible = false;
            trPassword.Visible = false;
            ddlAccountType.Visible = false;
            //trTrial.Visible = false;
            //chkIsActive.Enabled = false;

            lblAccountName.Visible = true;
            lblAccountType.Visible = true;
            lblExpiryDate.Visible = true;
            //hlChangeAccountType.Visible = true;
            //hlRenew.Visible = true;
            
            lblExtension.Visible = true;
            lblAlerts.Visible = true;
            lblReportGen.Visible = true;

            txtExtension.Visible = false;
            chkAlerts.Visible = false;
            chkReportGen.Visible = false;

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

                if (Request.QueryString["accountid"] != null)
                {
                    _qsAccountID = Cryptography.Decrypt(Request.QueryString["accountid"]);
                    _iAccountID = int.Parse(_qsAccountID);
                }

                if (_strActionMode == "add")
                {
                    //trActive.Visible = false;
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }  
        }

        if (!IsPostBack)
        {
            ShowHideBySysOpt();
            //TabContainer1.ActiveTab = TabContainer1.Tabs[0];
            PopulateTerminology();
            PopulateCountry();
            PopulateGraphOption();
            if (Request.QueryString["wizard"] != null)
            {
                divSave.Visible = false;
                divBack.Visible = false;
                //divDelete.Visible = false;
                trIsActive.Visible = false;
                divUnDelete.Visible = false;
                divEdit.Visible = false;
                //divCheckPayment.Visible = false;
            }
            else
            {
                divNext.Visible = false;
            }


            hlMakePayment.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/MakePayment.aspx?PaymentInfo="
                    + Cryptography.Encrypt("Payment_MakePayment");

            //Common.PopulateAdminDropDown(ref ddlAdminArea);
            //ddlAdminArea.Text = "Account";
            PopulateAccountType();
            PopulateTableDDL();
            PopulateDisplayTableDDL();
            //PopulateYAxis();

            BindTheInvoiceGrid(0, gvInvoice.PageSize);
        }

        if (fuPhoto.HasFile)
        {
            try
            {
                PopulateImageControl();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Please upload an image file!";
                return;
            }

        }

        string strTitle = "Account Detail";

        // checking permission       

        switch (_strActionMode.ToLower())
        {
            case "add":
                //divDelete.Visible = false;
                trIsActive.Visible = false;
                divUnDelete.Visible = false;
                strTitle = "Add Account";
                break;

            case "view":


                PopulateTheRecord();
                EnableTheRecordControls(false);
                divSave.Visible = false;
                //hlChangeAccountType.Visible = true;
                //hlRenew.Visible = true;

                hlChangeAccountType.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountTypeChange.aspx?AccountID=" + Cryptography.Encrypt(_iAccountID.ToString());
                hlRenew.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountTypeChange.aspx?type=pay&AccountID=" + Cryptography.Encrypt(_iAccountID.ToString());

                strTitle = "My Account";
                break;

            case "edit":
                //_lstUserRole = SecurityManager.UserRole_SelectByUserID(_iAccountID);

                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }

                hlChangeAccountType.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountTypeChange.aspx?AccountID=" + Cryptography.Encrypt(_iAccountID.ToString());
                hlRenew.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountTypeChange.aspx?type=pay&AccountID=" + Cryptography.Encrypt(_iAccountID.ToString());

                strTitle = "My Account";
                break;


            default:
                //?

                break;
        }

        Title = strTitle;
        lblTitle.Text = strTitle;


        //if (Request.UserAgent.Contains("Android"))
        //{
        //    lblAdminArea.Visible = true;
        //    ddlAdminArea.Visible = true;
        //}
        //else
        //{            
        //    lblAdminArea.Visible = false;
        //    ddlAdminArea.Visible = false;
        //}

        hfRootURL.Value = Request.Url.Authority + Request.ApplicationPath;

//        string strFancy = @" 
//            if (document.getElementById('hlChoose') != null) {
//            document.getElementById('hlChoose').href = " + @"'http://" + Request.Url.Authority + Request.ApplicationPath + @"/Pages/Site/" +@"GoogleMap.aspx?type=account&lat=' + document.getElementById(""ctl00_HomeContentPlaceHolder_txtLatitude"").value + ""&lng="" + document.getElementById(""ctl00_HomeContentPlaceHolder_txtLongitude"").value;
//        }; ";

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);



        bool bHasValue = false;

        hfFlag.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Images/Flag.png";


        Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

        if (theAccount != null)
        {
            
           

            txtSMTPEmail.Attributes["onBlur"] = "EmailValidaotrEnableDisable()";
            txtPOP3Email.Attributes["onBlur"] = "EmailValidaotrEnableDisable()";

            //if (theAccount.MapCentreLat != null && theAccount.MapCentreLong != null)
            //{
            //    hfCentreLat.Value = theAccount.MapCentreLat.ToString();
            //    hfCentreLong.Value = theAccount.MapCentreLong.ToString();

            //    hfLat.Value = theAccount.MapCentreLat.ToString();
            //    hfLng.Value = theAccount.MapCentreLong.ToString();
            //    bHasValue = true;
            //}
            if (theAccount.MapZoomLevel != null)
            {
                hfMapZoomLevel.Value = theAccount.MapZoomLevel.ToString();
            }
        }


    }


   


    protected void PopulateImageControl()
    {
        BinaryReader br = new BinaryReader(fuPhoto.PostedFile.InputStream);
        byte[] data = null;
        data = br.ReadBytes((int)fuPhoto.PostedFile.ContentLength);

        System.Drawing.Image theImage = System.Drawing.Image.FromStream(new MemoryStream(data));

        
        Guid newGuid = new Guid();
        newGuid = Guid.NewGuid();      
        
        string strFileNameTemp = newGuid.ToString();

        string strFilyType =fuPhoto.FileName.Substring(fuPhoto.FileName.LastIndexOf("."));
       
        strFileNameTemp = strFileNameTemp + strFilyType;   

      
        ViewState["data"] = data;
        
        if (theImage.Width > 400 || theImage.Height > 400)
        {
           data=Common.ResizeImageFile(data, 400);
        }
        else
        {
           //
        }
        theImage = System.Drawing.Image.FromStream(new MemoryStream(data));
        Bitmap bmp = new Bitmap(theImage);
        bmp.Save(Server.MapPath("Images/" + strFileNameTemp));
        imgPhoto.ImageUrl = "Images/" + strFileNameTemp;
        //trLogo.Attributes.Add ("style","display:block;");
        //lets delete old files.
        try
        {
            DeleteOldFiles("Images");
        }
        catch
        {

        }
    }





   


    protected void DeleteOldFiles(string strFolder)
    {
        DirectoryInfo di = new DirectoryInfo(Server.MapPath(strFolder));
        FileInfo[] rgFiles = di.GetFiles();
        foreach (FileInfo fi in rgFiles)
        {
            if (fi.CreationTime.AddHours(1) < DateTime.Now)
            {
                fi.Delete();
            }
        } 
    }

    protected void PopulateTerminology()
    {
        stgShowLocation.InnerText = SecurityManager.etsTerminology("AccountDetail.aspx", "Show Location", "Show Location");
    }

    protected void PopulateAccountType()
    {

        //ddlAccountType.DataSource = Common.DataTableFromText("SELECT * FROM AccountType WHERE AccountTypeID<>4");


        ddlAccountType.DataSource = Common.DataTableFromText("SELECT * FROM AccountType");

        ddlAccountType.DataBind();

        ListItem liAll = new ListItem("-Please Select-", "-1");
        ddlAccountType.Items.Insert(0, liAll);

    }


    protected void PopulateGraphOption()
    {
        ddlDefaultGraph.Items.Clear();
        DataTable dtRecord = Common.DataTableFromText(@"SELECT * FROM GraphOption WHERE 
            AccountID=" + Session["AccountID"].ToString() + " AND GraphPanel=2 AND IsActive=1 AND ReportChart=0 ORDER BY Heading DESC");
        foreach (DataRow drEachST in dtRecord.Rows)
        {
            if (drEachST["Heading"] != DBNull.Value)
            {
                System.Web.UI.WebControls.ListItem liSTs = new System.Web.UI.WebControls.ListItem(drEachST["Heading"].ToString(),
                    drEachST["GraphOptionID"].ToString());
                ddlDefaultGraph.Items.Insert(0, liSTs);
            }
        }        

        ListItem liNone = new ListItem("--None--", "-1");
        ddlDefaultGraph.Items.Insert(0, liNone);

    }



    protected void chkIsActiveAccount_CheckedChanged(Object sender, EventArgs args)
    {
        if (chkIsActiveAccount.Checked)
        {
            lnkUnDelete_Click(null, null);
            
        }
        else
        {
            lnkDelete_Click(null, null);
        }
    }

    protected void PopulateTheRecord()
    {
        try
        {

            hlDemoEmail.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/DemoEmail.aspx?AccountID=" + Cryptography.Encrypt(_iAccountID.ToString());

            if (_bGod)
                trIsActive.Visible = true;

            Account theAccount = SecurityManager.Account_Details((int)_iAccountID);
            //User theUser = SecurityManager.User_AccountHolder((int)theAccount.AccountID);
            //now get the account holder
            chkSameAsSMTP.Checked = false;
            if (theAccount.SMTPEmail != null && theAccount.SMTPEmail!="")
            {
                txtSMTPEmail.Text = theAccount.SMTPEmail;
                txtSMTPPassword.Text = theAccount.SMTPPassword;
                txtSMTPPort.Text = theAccount.SMTPPort;
                txtSMTPReplyToEmail.Text = theAccount.SMTPReplyToEmail;
                txtSMTPServer.Text = theAccount.SMTPServer;
                radioSMTPSSL.SelectedValue = theAccount.SMTPSSL;
                txtSMTPUserName.Text = theAccount.SMTPUserName;
            }
            if (theAccount.POP3Email!=null && theAccount.POP3Email != "")
            {
                txtPOP3Email.Text = theAccount.POP3Email;
                txtPOP3Password.Text = theAccount.POP3Password;
                txtPOP3Port.Text = theAccount.POP3Port;
                txtPOP3Server.Text = theAccount.POP3Server;
                radioPOP3SSL.SelectedValue = theAccount.POP3SSL;
                txtPOP3UserName.Text = theAccount.POP3UserName;

                if (theAccount.POP3Email == theAccount.SMTPEmail && theAccount.POP3SSL == theAccount.SMTPSSL
                    && theAccount.POP3UserName == theAccount.SMTPUserName && theAccount.POP3Password == theAccount.SMTPPassword)
                {
                    chkSameAsSMTP.Checked = true;
                }

            }



            User theUser = (User)Session["User"];
            txtAccountName.Text = theAccount.AccountName;
            lblAccountName.Text = theAccount.AccountName;

            txtFirstName.Text = theUser.FirstName;
            txtLastName.Text = theUser.LastName;
            txtContactNumber.Text = theUser.PhoneNumber;

            chkUseDefaultLogo.Checked = (bool)theAccount.UseDefaultLogo;
            //txtContactNumber.Text = theAccount.PhoneNumber;
            txtBillingFirstName.Text = theAccount.BillingFirstName;
            txtBillingLastName.Text = theAccount.BillingLastName;
            txtBillingPhoneNumber.Text = theAccount.BillingPhoneNumber;
            txtBillingEmail.Text = theAccount.BillingEmail;
            txtBillingAddress.Text = theAccount.BillingAddress;
            ddlMasterPage.SelectedValue = theAccount.MasterPage;

            txtReportUser.Text = theAccount.ReportUser;
            txtReportServerUrl.Text = theAccount.ReportServerUrl;
            txtReportServer.Text = theAccount.ReportServer;
            txtReportPW.Text = theAccount.ReportPW;


            if (txtReportServerUrl.Text.Trim() == "")
                txtReportServerUrl.Text = "http://localhost/reportserver";
            if (txtReportServer.Text.Trim() == "")
                txtReportServer.Text = "localhost";

            if (theAccount.IsReportTopMenu != null)
            {
                chkIsReportTopMenu.Checked = (bool)theAccount.IsReportTopMenu;
            }
            if (theAccount.LabelOnTop != null)
            {
                chkLabelOnTop.Checked = (bool)theAccount.LabelOnTop;
            }
            //if (theAccount.UploadAfterVerificaition != null)
            //{
            //    chkUploadAfterVerificaition.Checked = (bool)theAccount.UploadAfterVerificaition;
            //}



            if (theAccount.ShowOpenMenu != null)
            {
                chkShowOpenMenu.Checked = (bool)theAccount.ShowOpenMenu;
            }

            if (theAccount.HomeMenuCaption != "")
            {
                txtHomeMenuCaption.Text = theAccount.HomeMenuCaption;
            }

            if (theAccount.UseDataScope != null)
            {
                chkUseDataScope.Checked = (bool)theAccount.UseDataScope;
            }

            if (theAccount.CountryID != null)
            {
                try
                {
                    ddlCountry.Text = theAccount.CountryID.ToString();
                }
                catch
                {
                }
            }

            txtEmail.Text = theUser.Email;

            if (theAccount.ExpiryDate != null)
            {
                txtExpiryDate.Text = theAccount.ExpiryDate.Value.Day.ToString() + "/" + theAccount.ExpiryDate.Value.Month.ToString() + "/" + theAccount.ExpiryDate.Value.Year.ToString();
                
            }
            lblExpiryDate.Text = txtExpiryDate.Text;

            //chkTrial.Checked = !(bool)theAccount.IsClient;
            
            
            if (theAccount.AccountTypeID != null)
            {
                ddlAccountType.Text = theAccount.AccountTypeID.ToString();

                AccountType theAccountType = SecurityManager.AccountType_Details((int)theAccount.AccountTypeID);

                string  strEmailCount = "0";
                
                if (theAccountType.MaxEmailsPerMonth != null)
                {
                    if (theAccount.EmailCount != null)
                    {
                        strEmailCount = theAccount.EmailCount.ToString();
                    }
                    lblEmailsSent.Text = strEmailCount + " of " + theAccountType.MaxEmailsPerMonth.ToString();
                }

                string strSMSCount = "0";

                if (theAccountType.MaxSMSPerMonth != null)
                {
                    if (theAccount.SMSCount != null)
                    {
                        strSMSCount = theAccount.SMSCount.ToString();
                    }
                    lblSMSSent.Text = strSMSCount + " of " + theAccountType.MaxSMSPerMonth.ToString();
                }


                int iTotalRecords = 0;
                iTotalRecords = SecurityManager.ets_TotalRecords((int)theAccount.AccountID);
                if(theAccountType.MaxTotalRecords!=null)
                {
                    lblTotalRecords.Text = lblTotalRecords.Text.Replace("[MaxTotalRecords]", theAccountType.MaxTotalRecords.ToString());
                }
                else
                {
                    lblTotalRecords.Text = lblTotalRecords.Text.Replace("[MaxTotalRecords]", "Unlimited");
                }

                lblTotalRecords.Text = lblTotalRecords.Text.Replace("[RecordCount]", iTotalRecords.ToString());

            }

            if (theAccount.DefaultGraphOptionID != null)
            {
                ddlDefaultGraph.Text = theAccount.DefaultGraphOptionID.ToString();
            }

            //if (chkTrial.Checked)
            //{
            //    lblAccountType.Text = ddlAccountType.SelectedItem.Text + " - " + "Trial";
            //}
            //else
            //{
            //    lblAccountType.Text = ddlAccountType.SelectedItem.Text + " - " + "Live";
            //}

            lblAccountType.Text = ddlAccountType.SelectedItem.Text;

            if (theAccount.ExtensionPacks != null)
            {
                lblExtension.Text = theAccount.ExtensionPacks.ToString();
                txtExtension.Text = theAccount.ExtensionPacks.ToString(); 
            }

            if (theAccount.Alerts != null)
            {
                if (theAccount.Alerts == true)
                {
                    lblAlerts.Text = "Yes";
                    chkAlerts.Checked = true;
                }              

            }

            if (theAccount.ReportGen != null)
            {
                if (theAccount.ReportGen == true)
                {
                    lblReportGen.Text = "Yes";
                    chkReportGen.Checked = true;
                }

            }



         
            if (theAccount.MapZoomLevel != null)
                hfMapScale.Value = theAccount.MapZoomLevel.ToString();

            if (theAccount.OtherMapZoomLevel != null)
                ddlOtherMapScale.Text = theAccount.OtherMapZoomLevel.ToString();


           

            if (theAccount.MapDefaultTableID != null)
            {
                try
                {
                    ddlTableMap.Text = theAccount.MapDefaultTableID.ToString();                 

                }
                catch
                {

                }

            }
            if (theAccount.DisplayTableID != null)
            {
                try
                {
                    optDisplayTable.Checked = true;
                    ddlDisplayTable.Text = theAccount.DisplayTableID.ToString();

                }
                catch
                {

                }

            }
            else
            {
                if (theAccount.HomePageLink == "")
                {
                    optDashboard.Checked = true;
                }
                else
                {
                    optHomePageLink.Checked = true;
                    txtHomePageLink.Text = theAccount.HomePageLink;
                }

            }



            ddlLayout.Text = theAccount.Layout.ToString();

           


            if (theAccount.Logo != null)
            {
                imgPhoto.ImageUrl = "~/SSPhoto.ashx?AccountID=" + _iAccountID;

            }
            
            if (_strActionMode == "edit")
            {
                ViewState["theAccount"] = theAccount;
                ViewState["theUser"] = theUser;
                lblPassword.Text = "New Password";
                rfvPassword.Enabled = false;


                if (theAccount.IsActive == true)
                {
                    divUnDelete.Visible = false;
                    chkIsActiveAccount.Checked = true;
                }
                else
                {
                    //divDelete.Visible = false;
                    chkIsActiveAccount.Checked = false;
                    //trIsActive.Visible = false;
                }
            }           
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                //divDelete.Visible = false;
                trIsActive.Visible = false;
                divUnDelete.Visible = false;

                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(theAccount.AccountID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }
                else
                {
                    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(theAccount.AccountID.ToString());
                }
            }

            if (Common.HaveAccess(Session["roletype"].ToString(), "2"))
            {
                //divDelete.Visible = false;
                trIsActive.Visible = false;
                divUnDelete.Visible = false;
                txtExpiryDate.Visible = false;
                //chkTrial.Enabled = false;

            }


            txtSMTPPassword.Attributes.Add("value", txtSMTPPassword.Text);
            txtPOP3Password.Attributes.Add("value", txtPOP3Password.Text);
            txtReportPW.Attributes.Add("value", txtReportPW.Text);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Account Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }     

}
   

    protected void EnableTheRecordControls(bool p_bEnable)
    {
       
        txtAccountName.Visible = p_bEnable;
        ddlAccountType.Visible = p_bEnable;
        txtExpiryDate.Visible = p_bEnable;
        lblAccountName.Visible = !p_bEnable;
        lblAccountType.Visible = !p_bEnable;
        lblExpiryDate.Visible = !p_bEnable;
      
        lblExtension.Visible = !p_bEnable;
        lblAlerts.Visible = !p_bEnable;
        lblReportGen.Visible = !p_bEnable;

        txtExtension.Visible = p_bEnable;
        chkAlerts.Visible = p_bEnable;
        chkReportGen.Visible = p_bEnable;



        txtHomeMenuCaption.Enabled = p_bEnable;
        txtFirstName.Enabled = p_bEnable;
        txtLastName.Enabled = p_bEnable;
        //chkIsActive.Enabled = p_bEnable;
        fuPhoto.Enabled = p_bEnable;
        txtPhoneNumber.Enabled = p_bEnable;
        txtEmail.Enabled = p_bEnable;
        txtPassword.Enabled = p_bEnable;
      
       
        chkUseDefaultLogo.Enabled = p_bEnable;
        //chkTrial.Enabled = p_bEnable;

       
        //ddlScale.Enabled = p_bEnable;
        ddlOtherMapScale.Enabled = p_bEnable;
       
        //hlChoose.Enabled = p_bEnable;
        ddlTableMap.Enabled = p_bEnable;
        
        ddlDisplayTable.Enabled = p_bEnable;
        ddlCountry.Enabled = p_bEnable;
        txtContactNumber.Enabled = p_bEnable;

      
        ddlLayout.Enabled = p_bEnable;

        chkUseDataScope.Enabled = p_bEnable;

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action
       
        if (txtSMTPEmail.Text.Trim() != "")
        {
            //if(txtSMTPPassword.Text.Trim()=="")
            //{
            //    lblMsg.Text = "SMTP Password is required";
            //    txtSMTPPassword.Focus();
            //    return false;
            //}
            if (txtSMTPPort.Text.Trim() == "")
            {
                lblMsg.Text = "SMTP Port is required";
                txtSMTPPort.Focus();
                return false;
            }
            if (txtSMTPServer.Text.Trim() == "")
            {
                lblMsg.Text = "SMTP Server is required";
                txtSMTPServer.Focus();
                return false;
            }
            if (txtSMTPUserName.Text.Trim() == "")
            {
                lblMsg.Text = "SMTP User name is required";
                txtSMTPUserName.Focus();
                return false;
            }
        }

        if (txtPOP3Email.Text.Trim() != "")
        {
            //if (txtPOP3Password.Text.Trim() == "")
            //{
            //    lblMsg.Text = "POP3 Password is required";
            //    txtPOP3Password.Focus();
            //    return false;
            //}
            if (txtPOP3Port.Text.Trim() == "")
            {
                lblMsg.Text = "POP3 Port is required";
                txtPOP3Port.Focus();
                return false;
            }
            if (txtPOP3Server.Text.Trim() == "")
            {
                lblMsg.Text = "POP3 Server is required";
                txtPOP3Server.Focus();
                return false;
            }
            if (txtPOP3UserName.Text.Trim() == "")
            {
                lblMsg.Text = "POP3 User name is required";
                txtPOP3UserName.Focus();
                return false;
            }
        }

        if (txtPassword.Text.ToLower().IndexOf(txtAccountName.Text.ToLower()) > -1)
        {
            lblMsg.Text = "Password should not have account name!";
            txtPassword.Focus();
            return false;
        }

        if (txtPassword.Text.ToLower().IndexOf(txtFirstName.Text.ToLower()) > -1)
        {
            lblMsg.Text = "Password should not have first name!";
            txtPassword.Focus();
            return false;
        }

        if (txtPassword.Text.ToLower().IndexOf(txtLastName.Text.ToLower()) > -1)
        {
            lblMsg.Text = "Password should not have last name!";
            txtPassword.Focus();
            return false;
        }

        if (txtPassword.Text.ToLower().IndexOf(txtEmail.Text.ToLower().Substring(0, txtEmail.Text.IndexOf("@"))) > -1)
        {
            lblMsg.Text = "Password should not have email address!";
            txtPassword.Focus();
            return false;
        }

        if (ddlAccountType.SelectedValue == "-1")
        {
            lblMsg.Text = "Please select an Account Type.";
            ddlAccountType.Focus();
            return false;

        }
        //if (chkTrial.Checked)
        //{
        //    if (txtExpiryDate.Text == "")
        //    {
        //        lblMsg.Text = "Please select an expiry date.";
        //        txtExpiryDate.Focus();
        //        return false;
        //    }
        //}

        return true;
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {

        lnkSave_Click(null, null);
       

    }

   
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtSMTPPassword.Attributes.Add("value", txtSMTPPassword.Text);
            txtPOP3Password.Attributes.Add("value", txtPOP3Password.Text);
            txtReportPW.Attributes.Add("value", txtReportPW.Text);
            if (IsUserInputOK())
            {

                

              
                switch (_strActionMode.ToLower())
                {
                    case "add":

                        //SqlTransaction tn;
                        //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
                        //connection.Open();
                        //tn = connection.BeginTransaction();

                        //try
                        //{
                        //    int AccountId;


                        //    DateTime? dtExpiryDate = null;
                           
                        //    if (txtExpiryDate.Text.Trim() == "")
                        //    {
                        //        dtExpiryDate = DateTime.Now.AddDays(14);
                        //    }
                        //    else
                        //    {
                        //        dtExpiryDate = (DateTime?)DateTime.ParseExact(txtExpiryDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);
                        //    }
                          
                            

                        //    Account newAccount = new Account(null, txtAccountName.Text.Trim(),  null, null,
                        //        ddlAccountType.SelectedValue == "-1" ? null : (int?) int.Parse(ddlAccountType.SelectedValue),
                        //         dtExpiryDate);
                        //       newAccount.IsActive = true;

                        //       //newAccount.IsClient = !chkTrial.Checked;

                        //       newAccount.UseDefaultLogo = chkUseDefaultLogo.Checked;

                        //       //newAccount.MapCentreLat = txtLatitude.Text == "" ? null : (double?)double.Parse(txtLatitude.Text.Trim());
                        //       //newAccount.MapCentreLong = txtLongitude.Text == "" ? null : (double?)double.Parse(txtLongitude.Text.Trim());
                        //       newAccount.MapZoomLevel = hfMapScale.Value == "-1" ? null : (int?)int.Parse(hfMapScale.Value);
                        //       newAccount.OtherMapZoomLevel = ddlOtherMapScale.SelectedValue == "-1" ? null : (int?)int.Parse(ddlOtherMapScale.SelectedValue);


                        //       newAccount.PhoneNumber = txtContactNumber.Text.Trim();
                        //       newAccount.CountryID = int.Parse(ddlCountry.SelectedValue);

                        //   //if (ddlYAxis.SelectedValue != "-1")
                        //   //{
                        //   //    newAccount.GraphDefaultTableID = int.Parse(ddlTable.SelectedValue);
                        //   //    newAccount.GraphDefaultColumnID = int.Parse(ddlYAxis.SelectedValue);
                        //   //}
                           

                        //    //}



                        //    if (ViewState["data"]!=null)
                        //    {                              
                        //        byte[] data = null;
                        //        data = (byte[])ViewState["data"];
                        //        newAccount.Logo = (object)data;
                        //    }

                        //    AccountId = SecurityManager.Account_Insert(newAccount, ref connection, ref tn);

                        //    //Now create the user
                        //    User newUser = new User(null, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtContactNumber.Text,
                        //         txtEmail.Text.Trim(), txtPassword.Text, true, null, null);//,  "", true,false
                        //    //newUser.UserName = newUser.Email;
                        //    int iUserID = SecurityManager.User_Insert(newUser, ref connection, ref tn);

                        //    //Now assign the user as a role type 2(admin)
                        //    int iTN=0;
                        //    List<Role> lstRoles = SecurityManager.Role_Select(null, "", "2", "", null, null,
                        //        "RoleID", "ASC", null, null, ref iTN, int.Parse(Session["AccountID"].ToString()), null, null);

                        //    if (lstRoles.Count > 0)
                        //    {
                        //        UserRole newUserRole = new UserRole(null, iUserID, lstRoles[0].RoleID, null, null);
                        //        newUserRole.AccountID = AccountId;
                        //        newUserRole.IsPrimaryAccount = true;

                        //        newUserRole.IsAccountHolder = true;
                        //        newUserRole.IsAdvancedSecurity = false;
                              
                        //        int iUserRoleID = SecurityManager.UserRole_Insert(newUserRole, ref connection, ref tn);

                        //    }

                        //    tn.Commit();
                        //    connection.Close();
                        //    connection.Dispose();

                        //}
                        //catch ( Exception ex)
                        //{
                            
                        //    tn.Rollback();
                        //    connection.Close();
                        //    connection.Dispose();

                        //    if (ex.Message.IndexOf("UQ_AccountName") > -1 || ex.Message.IndexOf("Transaction count") > -1)
                        //    {

                        //        DataTable dtNextAccount = Common.DataTableFromText("select dbo.fnNextAvailableAccountName('" + txtAccountName.Text.Replace("'", "''") + "')");
                        //        lblMsg.Text = "Sorry that Account Name is not available.  Please choose another.  You could try - " + dtNextAccount.Rows[0][0].ToString();
                        //        txtAccountName.Focus();
                        //    }
                        //    else if (ex.Message.IndexOf("UQ_UserEmail") > -1)
                        //    {
                        //        lblMsg.Text = "Sorry this email address("+txtEmail.Text+") is already used by another user. Please use another email address!";
                        //        txtEmail.Focus();
                        //    }
                        //    else
                        //    {
                        //        lblMsg.Text = ex.Message;
                        //    }
                        //    return;
                        //}
                       


                        break;

                    case "view":


                        break;

                    case "edit":
                        Account editAccount = (Account)ViewState["theAccount"];
                        User editUser = (User)ViewState["theUser"];

                        editAccount.ShowOpenMenu = chkShowOpenMenu.Checked;
                        editAccount.AccountName = txtAccountName.Text;
                        editUser.FirstName = txtFirstName.Text;
                        editUser.LastName = txtLastName.Text;
                        editUser.PhoneNumber = txtContactNumber.Text;
                        editUser.Email = txtEmail.Text;
                        editAccount.UseDefaultLogo = chkUseDefaultLogo.Checked;
                        editAccount.MasterPage = ddlMasterPage.SelectedValue;
                        editAccount.UseDataScope = chkUseDataScope.Checked;
                        editAccount.ReportPW = txtReportPW.Text;
                        editAccount.ReportServer = txtReportServer.Text;
                        editAccount.ReportServerUrl = txtReportServerUrl.Text;
                        editAccount.ReportUser = txtReportUser.Text;
                        //editAccount.MapCentreLat = txtLatitude.Text == "" ? null : (double?)double.Parse(txtLatitude.Text.Trim());
                        //editAccount.MapCentreLong = txtLongitude.Text == "" ? null : (double?)double.Parse(txtLongitude.Text.Trim());

                        editAccount.MapZoomLevel = hfMapScale.Value == "-1" ? null : (int?)int.Parse(hfMapScale.Value);
                        editAccount.OtherMapZoomLevel = ddlOtherMapScale.SelectedValue == "-1" ? null : (int?)int.Parse(ddlOtherMapScale.SelectedValue);

                        if (txtExtension.Text != "")
                        {
                            editAccount.ExtensionPacks = int.Parse(txtExtension.Text);
                        }
                        editAccount.Alerts = chkAlerts.Checked;
                        editAccount.ReportGen = chkReportGen.Checked;

                        //editAccount.PhoneNumber = txtContactNumber.Text.Trim();
                        editAccount.CountryID = int.Parse(ddlCountry.SelectedValue);

                       if(  txtSMTPEmail.Text.Trim()=="")
                       {
                           editAccount.SMTPEmail = "";
                           editAccount.SMTPUserName = "";
                           editAccount.SMTPPassword = "";
                           editAccount.SMTPPort = "";
                           editAccount.SMTPReplyToEmail = "";
                           editAccount.SMTPServer = "";
                           editAccount.SMTPSSL = "";
                          
                       }
                       else
                       {
                           editAccount.SMTPEmail = txtSMTPEmail.Text;
                           editAccount.SMTPUserName = txtSMTPUserName.Text;

                           if (txtSMTPPassword.Text == "" && editAccount.SMTPPassword != "")
                               txtSMTPPassword.Text = editAccount.SMTPPassword;

                           editAccount.SMTPPassword = txtSMTPPassword.Text;
                           editAccount.SMTPPort = txtSMTPPort.Text;
                           editAccount.SMTPReplyToEmail = txtSMTPReplyToEmail.Text;
                           editAccount.SMTPServer = txtSMTPServer.Text;
                           editAccount.SMTPSSL = radioSMTPSSL.SelectedValue;
                       }

                       if (txtPOP3Email.Text.Trim() == "")
                       {
                           editAccount.POP3Email = "";
                           editAccount.POP3UserName = "";
                           editAccount.POP3Password = "";
                           editAccount.POP3Port = "";                        
                           editAccount.POP3Server = "";
                           editAccount.POP3SSL = "";

                       }
                       else
                       {
                           editAccount.POP3Email = txtPOP3Email.Text;
                           editAccount.POP3UserName = txtPOP3UserName.Text;

                           if (txtPOP3Password.Text == "" && editAccount.POP3Password != "")
                               txtPOP3Password.Text = editAccount.POP3Password;

                           editAccount.POP3Password = txtPOP3Password.Text;
                           editAccount.POP3Port = txtPOP3Port.Text;                          
                           editAccount.POP3Server = txtPOP3Server.Text;
                           editAccount.POP3SSL = radioPOP3SSL.SelectedValue;
                       }
                        if(chkSameAsSMTP.Checked)
                        {
                            editAccount.POP3Email = editAccount.SMTPEmail;
                            editAccount.POP3UserName = editAccount.SMTPUserName;
                           // editAccount.POP3Server = editAccount.SMTPServer;
                            editAccount.POP3Password = editAccount.SMTPPassword;
                            editAccount.POP3SSL = editAccount.SMTPSSL;
                            editAccount.POP3Port = txtPOP3Port.Text;
                            editAccount.POP3Server = txtPOP3Server.Text;
                        }

                        if (txtHomeMenuCaption.Text.Trim() == "")
                        {
                            //txtHomeMenuCaption.Text = "Dashboard";
                        }
                        editAccount.HomeMenuCaption = txtHomeMenuCaption.Text;

                        
                        if (ddlTableMap.SelectedValue != "-1")
                        {
                            editAccount.MapDefaultTableID = int.Parse(ddlTableMap.SelectedValue);
                        }
                        else
                        {
                            editAccount.MapDefaultTableID = null;
                        }

                        if (ddlDisplayTable.SelectedValue != "" && optDisplayTable.Checked==true)
                        {
                            editAccount.DisplayTableID = int.Parse(ddlDisplayTable.SelectedValue);
                            editAccount.HomePageLink = "";
                        }
                        else
                        {
                            editAccount.DisplayTableID = null;
                            editAccount.HomePageLink = "";
                            if (optHomePageLink.Checked == true)
                            {
                                if (txtHomePageLink.Text.Trim() != "")
                                {
                                    editAccount.HomePageLink = txtHomePageLink.Text.Trim();
                                }
                            }

                        }

                      

                        if (txtPassword.Text.Trim() != "")
                        {
                            editUser.Password = txtPassword.Text;
                        }

                        editAccount.DateUpdated = DateTime.Now;

                        editAccount.Layout = int.Parse(ddlLayout.SelectedValue);

                        editAccount.AccountTypeID = ddlAccountType.SelectedValue == "-1" ? null : (int?)int.Parse(ddlAccountType.SelectedValue);

                        editAccount.DefaultGraphOptionID = ddlDefaultGraph.SelectedValue == "-1" ? null : (int?)int.Parse(ddlDefaultGraph.SelectedValue);
                       
                        if (txtExpiryDate.Text.Trim() == "")
                        {
                            editAccount.ExpiryDate = null;
                        }
                        else
                        {
                            editAccount.ExpiryDate = (DateTime?)DateTime.ParseExact(txtExpiryDate.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);
                        }

                        //editAccount.IsClient = !chkTrial.Checked;
                        
                        if (ViewState["data"] != null)
                        {
                            byte[] data = null;
                            data = (byte[])ViewState["data"];                           
                            editAccount.Logo = (object)data;
                        }
                        

                        editAccount.BillingFirstName = txtBillingFirstName.Text;
                        editAccount.BillingLastName = txtBillingLastName.Text;
                        editAccount.BillingPhoneNumber = txtBillingPhoneNumber.Text;
                        editAccount.BillingEmail = txtBillingEmail.Text;
                        editAccount.BillingAddress = txtBillingAddress.Text;
                        editAccount.IsActive = chkIsActiveAccount.Checked;
                        editAccount.IsReportTopMenu = chkIsReportTopMenu.Checked;
                        editAccount.LabelOnTop = chkLabelOnTop.Checked;
                        //editAccount.UploadAfterVerificaition = chkUploadAfterVerificaition.Checked;


                        
                        if(_bIsAdmin)
                            SecurityManager.Account_Update(editAccount);

                       
                        SecurityManager.User_Update(editUser);

                        Session["User"] = editUser;

                        break;

                }
            }
            else
            {
                //user input is not ok
                return;
            }


            if (Request.QueryString["wizard"] != null)
            {
                Response.Redirect("~/Pages/Template/TableList.aspx?wizard=yes", false);
            }
            else
            {
                Response.Redirect(hlBack.NavigateUrl, false);
            }
            

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Account Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            //lblMsg.Text = ex.Message; 

            if (ex.Message.IndexOf("UQ_AccountName") > -1 || ex.Message.IndexOf("correct format") > -1)
            {
                
                DataTable dtNextAccount = Common.DataTableFromText("select dbo.fnNextAvailableAccountName('" + txtAccountName.Text.Replace("'", "''") + "')");
                lblMsg.Text = "Sorry that Account Name is not available.  Please choose another.  You could try - " + dtNextAccount.Rows[0][0].ToString();
            }
            else
            {
                lblMsg.Text = ex.Message;
            }
        }

    }

    protected void gvInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblInvoiceCovers = (Label)e.Row.FindControl("lblInvoiceCovers");
                string strInvoiceCovers = "";
                DateTime dateStartDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "StartDate"));
                DateTime dateEndDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "EndDate"));

                if ((dateStartDate.Month == dateEndDate.Month  || dateStartDate.Month+1 == dateEndDate.Month) && dateStartDate.Year == dateEndDate.Year)
                {
                    strInvoiceCovers = dateStartDate.ToString("MMM yyyy");
                }
                else
                {
                    strInvoiceCovers = dateStartDate.ToString("MMM yyyy") + " - " + dateEndDate.ToString("MMM yyyy");
                }
                lblInvoiceCovers.Text = strInvoiceCovers;

                Label lblAmountAUD = (Label)e.Row.FindControl("lblAmountAUD");

                lblAmountAUD.Text = DataBinder.Eval(e.Row.DataItem, "GrossAmountAUD").ToString();

                Label lblDetails = (Label)e.Row.FindControl("lblDetails");
                
                if (DataBinder.Eval(e.Row.DataItem, "PaidDate") == DBNull.Value)
                {
                    lblDetails.Text = "Not Paid";
                    //lblDetails.Text = "<a href='MakePayment.aspx?PaymentInfo=" + Cryptography.Encrypt("Payment_MakePayment") + "&InvoiceID=" + DataBinder.Eval(e.Row.DataItem, "InvoiceID").ToString() + "'>Pay Now</a>";
                }
                else
                {
                    string strPaymentMethod = "EFT";
                    if (DataBinder.Eval(e.Row.DataItem, "PaymentMethod").ToString().Trim() == "E")
                    {
                        strPaymentMethod = "EFT";
                    }
                    else
                    {
                        strPaymentMethod = "Paypal";
                    }
                    lblDetails.Text = strPaymentMethod + " " + Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "PaidDate")).ToShortDateString();

                }


               
            }

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }

    protected void BindTheInvoiceGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
            int iTN = 0;

            //gvInvoice.DataSource = RecordManager.ets_Record_Changes_Select(
            //        (int)_iRecordID,int.Parse(_qsTableID), iStartIndex, iMaxRows, ref  iTN, ref _iCLColumnCount);


            gvInvoice.DataSource = InvoiceManager.ets_Invoice_Select(_iAccountID, "", null, null, null, "", null,
                "",  "","","","",null,null,"DateAdded","DESC", iStartIndex, iMaxRows, ref iTN);                
            gvInvoice.VirtualItemCount = iTN;         


            gvInvoice.DataBind();
            if (gvInvoice.TopPagerRow != null)
                gvInvoice.TopPagerRow.Visible = true;
            GridViewRow gvr = gvInvoice.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

            }




        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Invoice Grid", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheInvoiceGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }
    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        BindTheInvoiceGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }


    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Invoices";
        BindTheInvoiceGrid(0, _gvPager.TotalRows);
    }
    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        //
    }

    protected void gvInvoice_PreRender(object sender, EventArgs e)
    {
        GridView grid = (GridView)sender;
        if (grid != null)
        {
            GridViewRow pagerRow = (GridViewRow)grid.TopPagerRow;
            if (pagerRow != null)
            {
                pagerRow.Visible = true;
            }
        }
    }


    protected void lnkBack_Click(object sender, EventArgs e)
    {
        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountList.aspx", false);
        }
        else
        {
            Response.Redirect("~/Default.aspx", false);
        }
        
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            SecurityManager.Account_Delete((int)_iAccountID);
            Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountList.aspx", false);

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

    protected void lnkUnDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            SecurityManager.Account_UnDelete((int)_iAccountID);
            Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountList.aspx", false);
        }
        catch (Exception ex)
        {          

           lblMsg.Text = ex.Message;
            
        }


    }

    //protected void ddlAdminArea_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Common.GetNavigateURL(ddlAdminArea.SelectedValue,int.Parse(Session["AccountID"].ToString())) , false);
    //}







    //protected void lnkCheckPayment_Click(object sender, EventArgs e)
    //{
    //    User objUser=(User)Session["User"];
    //    string strCheckIsClient = SecurityManager.CheckIsClient(objUser.UserID);

    //    switch (strCheckIsClient.ToUpper())
    //    {
    //        case "GRACE":
    //            //Payment_Grace
    //            Response.Redirect("~/Pages/Security/PaymentInfo.aspx?PaymentInfo="
    //                + Cryptography.Encrypt("Payment_Grace"), false);
    //            break;

    //        case "LIVE EXPIRED":
    //            //Payment_LiveExpired
    //            Response.Redirect("~/Pages/Security/PaymentInfo.aspx?PaymentInfo="
    //                + Cryptography.Encrypt("Payment_LiveExpired"), false);
    //            break;

    //        case "TRIAL EXPIRED":
    //            //Payment_TrialExpired
    //            Response.Redirect("~/Pages/Security/PaymentInfo.aspx?PaymentInfo="
    //               + Cryptography.Encrypt("Payment_TrialExpired"), false);

    //            //Session["CheckIsClient"] = "NO";

    //            break;

    //        case "TRIAL LAST WEEK":
    //            //Payment_TrialLastWeek
    //            Response.Redirect("~/Pages/Security/PaymentInfo.aspx?PaymentInfo="
    //              + Cryptography.Encrypt("Payment_TrialLastWeek"), false);
    //            break;

    //        case "OK":
    //           //
    //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('You are ok to continue.');", true);
    //            Response.Redirect("~/Pages/Security/PaymentInfo.aspx?PaymentInfo="
    //              + Cryptography.Encrypt("Payment_Ok"), false);
    //            break;
    //    }

    //}

   

   

 




   

   

    protected void PopulateTableDDL()
    {
        int iTN = 0;

        ddlTableMap.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                _iAccountID,
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, "");
      

        ddlTableMap.DataBind();           
               
      
        System.Web.UI.WebControls.ListItem liSelect2 = new System.Web.UI.WebControls.ListItem("All", "-1");
        ddlTableMap.Items.Insert(0, liSelect2);       
      
    }

    protected void PopulateDisplayTableDDL()
    {
        int iTN = 0;

        ddlDisplayTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                _iAccountID,
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, "");


        ddlDisplayTable.DataBind();


        System.Web.UI.WebControls.ListItem liSelect2 = new System.Web.UI.WebControls.ListItem("--None--", "");
        ddlDisplayTable.Items.Insert(0, liSelect2);

    }

    
    protected void ShowHideBySysOpt()
    {
        if (SystemData.SystemOption_ValueByKey_Account("HideAccountContactDetails",_iAccountID,null).ToLower() == "yes")
        {
            TabContainer1.Tabs[0].Visible = false;//1
        }

        if (SystemData.SystemOption_ValueByKey_Account("HideAccountBilling",_iAccountID,null).ToLower() == "yes")
        {
            TabContainer1.Tabs[2].Visible = false;
            TabContainer1.Tabs[3].Visible = false;
        }

        if (SystemData.SystemOption_ValueByKey_Account("HideAccountDataScope",_iAccountID,null).ToLower() == "yes")
        {
           //trDataScopHeadline.Visible = false;
           //trDataScopeCheckBox.Visible = false;
            chkUseDataScope.Visible = false;
        }

        if (SystemData.SystemOption_ValueByKey_Account("HideAccountHomePageOptions",_iAccountID,null).ToLower() == "yes")
        {
            //trHomePageCaption.Visible = false;
            trHomePageControls.Visible = false;
        }

    }



    protected void PopulateCountry()
    {

        int iTN = 0;
        ddlCountry.DataSource = SystemData.LookUpData_Select(null, -1, "", "", null, null,
             "DisplayText", "ASC", null, null, ref iTN, "");
        ddlCountry.DataBind();

        ddlCountry.SelectedIndex = 12;

    }


}
