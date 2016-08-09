using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Caching;
using DocGen.DAL;
using DocGen.Utility;
using System.Linq;
public partial class Login : System.Web.UI.Page
{
    int _iSearchCriteriaID = -1;
    int _iAccountType = 2; //free

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.QueryString["Account"] != null)
        {
            Session["client"] = Request.QueryString["Account"].ToString();
            Page.MasterPageFile = SystemData.SystemOption_ValueByKey_Account(Request.QueryString["Account"].ToString().ToUpper() + "_LoginMasterPage",null,null).ToString();
        }
        else
        {

            if (SystemData.SystemOption_ValueByKey_Account("LoginMasterPage", null, null) != "")
            {
                if (SystemData.SystemOption_ValueByKey_Account("LoginMasterPage", null, null).ToUpper() != "NA")
                {
                    Page.MasterPageFile = SystemData.SystemOption_ValueByKey_Account("LoginMasterPage", null, null).ToString();
                    //if (SystemData.SystemOption_ValueByKey("AccountKey").ToLower() == "rrp")
                    //{
                    //    bHideRegister = true;
                    //}
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Sign Up";


        try
        {


            if (Request.QueryString["AccountType"] != null)
            {
                _iAccountType =int.Parse( Request.QueryString["AccountType"].ToString());
            }
           
          
            if (!Page.IsPostBack)
            {
                ViewState["Account"] = null;
                Session["User"]= null;                 
            }

            
        }
        catch (Exception ex)
        {
            //
        }
    

    }

        

    protected void lnkCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "parent.$.fancybox.close();", true);
        Response.Redirect("~/AccountCreated.aspx?Contentkey=" + Cryptography.EncryptStatic("AccountCreated") + "&Email=" + Cryptography.Encrypt(Email.Text.Trim()), false);

    }

    protected void lnkNext1_Click(object sender, EventArgs e)
    {

        LblMessage.Text = "";
        LblMessage.Attributes.Add("style", "color:red");
       

        if (Email.Text == "")
        {
            lblEmailErrorMsg.Text = "Email Address is required";
            Email.Focus();
            return;
        }

        if (txtAccount.Text == "")
        {
            lblAccountErrorMsg.Text = "Account Name is required";
            txtAccount.Focus();
            return;
        }

        if (ViewState["Account"] == null)
        {          
                     
                Random Rnd = new Random();
                //string strAutoPassword = txtAccount.Text.Trim() + DateTime.Now.Second.ToString();
                string strAutoPassword = txtRegPassword.Text;
                int AccountId = 0;
                int iTN = 0;
                int iUserID = -1;
                bool bCopyAccount=false;
                string strTemplateAccountID = "";

                if (Request.QueryString["Account"] != null)
                {
                    if (Request.QueryString["Account"].ToString().ToUpper() == "RRP"
                        && SystemData.SystemOption_ValueByKey_Account(Request.QueryString["Account"].ToString().ToUpper() + "_TemplateAccountID",null,null) != "")
                    {
                        bCopyAccount = true;
                        strTemplateAccountID = SystemData.SystemOption_ValueByKey_Account(Request.QueryString["Account"].ToString().ToUpper() + "_TemplateAccountID", null, null);
                    }
                }
                else
                {
                    if (SystemData.SystemOption_ValueByKey_Account("AccountKey", null, null).ToLower() == "rrp"
                        && SystemData.SystemOption_ValueByKey_Account("RRP_TemplateAccountID", null, null) != "")
                    {
                        bCopyAccount = true;
                        strTemplateAccountID = SystemData.SystemOption_ValueByKey_Account("RRP_TemplateAccountID", null, null).ToLower();
                    }
                }
                
            

                try
                {
                    try
                    {


                        //string strAccountName = Email.Text.Substring(0, Email.Text.IndexOf("@")) + "1";
                        string strAccountName = txtAccount.Text;
                        strAccountName = Common.GetValueFromSQL("select dbo.fnNextAvailableAccountName('" + strAccountName + "')");


                        Account newAccount = new Account(null, strAccountName, null, null, 
                            _iAccountType,
                                      (DateTime?)DateTime.Now.AddDays(30));
                        newAccount.IsActive = true;
                        newAccount.PhoneNumber = "na";
                        newAccount.OrganisationName = "na";
                        newAccount.CountryID =null;
                        newAccount.HomeMenuCaption = "Dashboard";
                        AccountId = SecurityManager.Account_Insert(newAccount);
                        newAccount.AccountID = AccountId;
                        ViewState["Account"] = newAccount;
                        Session["AccountID"] = AccountId.ToString();
                      
                        //Now create the user
                        User newUser = new User(null, null, null, "",
                             Email.Text.Trim(), txtRegPassword.Text, true, null, null);//,  "", true, false
                        //newUser.UserName = newUser.Email;
                        iUserID = SecurityManager.User_Insert(newUser);
                        newUser.UserID = iUserID;

                        Session["User"] = newUser;

                        //Now assign the user as a role type 2(admin)
                        //List<Role> lstRoles = SecurityManager.Role_Select(null, "", "2", "", null, null,
                        //    "RoleID", "ASC", null, null, ref iTN, int.Parse(Session["AccountID"].ToString()), null, null);

                        DataTable dtRoles = Common.DataTableFromText("SELECT RoleID FROM [Role]  WHERE RoleType='2' AND AccountID=" + Session["AccountID"].ToString());

                        if (dtRoles.Rows.Count > 0)
                        {
                            UserRole newUserRole = new UserRole(null, iUserID, int.Parse(dtRoles.Rows[0][0].ToString()), null, null);
                            newUserRole.AccountID = AccountId;
                            newUserRole.IsPrimaryAccount = true;
                            newUserRole.IsAccountHolder = true;
                            int iUserRoleID = SecurityManager.UserRole_Insert(newUserRole);

                        }

                        Common.ExecuteText("UPDATE Account SET IsActive=1, ConfirmationCode=NULL WHERE AccountID=" + AccountId.ToString());

                        newAccount.AccountID = AccountId;

                        //string strURL = "~/Default.aspx?FromSignUp=yes";

                        string strURL = "~/SignUpDone.aspx";

                        if (bCopyAccount)
                        {
                            try
                            {
                                RecordManager.dbg_Copy_Account_SignUp(int.Parse(strTemplateAccountID), AccountId, iUserID);
                                strURL = "~/Default.aspx";

                                if (Request.QueryString["Account"] != null)
                                {
                                    if (Request.QueryString["Account"].ToString().ToUpper() == "RRP")
                                    {
                                        newAccount.MasterPage = "~/Home/RRP.master";
                                        SecurityManager.Account_Update(newAccount);
                                    }
                                
                                }
                                else
                                {
                                    if (SystemData.SystemOption_ValueByKey_Account("AccountKey",null,null).ToLower() == "rrp")
                                    {
                                        newAccount.MasterPage = "~/Home/RRP.master";
                                        SecurityManager.Account_Update(newAccount);


                                    }
                                }
                            }
                            catch
                            {
                                //
                            }
                        }

                      

                        if (newAccount.MasterPage == "~/Home/RRP.master")
                        {
                            string strDocumentID = Common.GetValueFromSQL(" SELECT TOP 1 DocumentID FROM Document WHERE AccountID=" + newAccount.AccountID.ToString());
                            if (strDocumentID != "")
                            {
                                string strDocumentSectionID = Common.GetValueFromSQL("  SELECT DocumentSectionID FROM DocumentSection WHERE DocumentID="+strDocumentID+" AND Position=1");
                                if (strDocumentSectionID != "")
                                {
                                    using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
                                    {
                                        DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == int.Parse(strDocumentSectionID));
                                        if (section != null)
                                        {
                                            string strContent = section.Content;

                                            if (strContent != "")
                                            {

                                                  //Contractor
                                                string strContractorTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                                                        WHERE TableName='Company' AND AccountID=" + AccountId.ToString());

                                                string strTContractorTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                                                        WHERE TableName='Company' AND AccountID=" + strTemplateAccountID);

                                                if (strContractorTableID != "" && strTContractorTableID != "")
                                                {
                                                    strContent = strContent.Replace(Cryptography.Encrypt(strTContractorTableID), Cryptography.Encrypt(strContractorTableID));

                                                    strContent = strContent.Replace("ReportView.aspx?TableID=" + Cryptography.Encrypt(strContractorTableID), "ReportView.aspx?TableID=" + Cryptography.Encrypt(strTContractorTableID));
                                                }

                                                //Risk
                                                string strRiskTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                                                WHERE TableName='Hazard' AND AccountID=" + AccountId.ToString());

                                                string strTRiskTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                                                WHERE TableName='Hazard' AND AccountID=" + strTemplateAccountID);

                                                if (strRiskTableID != "" && strTRiskTableID!="")
                                                {
                                                    strContent = strContent.Replace(Cryptography.Encrypt(strTRiskTableID), Cryptography.Encrypt(strRiskTableID));
                                                    strContent = strContent.Replace("ReportView.aspx?TableID=" + Cryptography.Encrypt(strRiskTableID), "ReportView.aspx?TableID=" + Cryptography.Encrypt(strTRiskTableID));
                                                }


                                                //Incident
                                                string strIncidentTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                                                    WHERE TableName='Incidents' AND AccountID=" + AccountId.ToString());

                                                string strTIncidentTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                                                    WHERE TableName='Incidents' AND AccountID=" + strTemplateAccountID);

                                                if (strIncidentTableID != "" && strTIncidentTableID!="")
                                                {
                                                    strContent = strContent.Replace(Cryptography.Encrypt(strTIncidentTableID), Cryptography.Encrypt(strIncidentTableID));
                                                    strContent = strContent.Replace("ReportView.aspx?TableID=" + Cryptography.Encrypt(strIncidentTableID), "ReportView.aspx?TableID=" + Cryptography.Encrypt(strTIncidentTableID));
                                                }

                                                //Injury
                                                string strInjuryTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                    WHERE TableName='Injury' AND AccountID=" + AccountId.ToString());

                                                string strTInjuryTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                    WHERE TableName='Injury' AND AccountID=" + strTemplateAccountID);

                                                if (strInjuryTableID != "")
                                                {
                                                    strContent = strContent.Replace(Cryptography.Encrypt(strTInjuryTableID), Cryptography.Encrypt(strInjuryTableID));
                                                    strContent = strContent.Replace("ReportView.aspx?TableID=" + Cryptography.Encrypt(strInjuryTableID), "ReportView.aspx?TableID=" + Cryptography.Encrypt(strTInjuryTableID));
                                                }

                                                section.Content = strContent;

                                                ctx.SubmitChanges();
                                            }
                                        }
                                    }

                                }

                            }

                            //lets call the spCreateAllViews
                            spCreateAllViews(AccountId, "rrponline");
                        }


                        SignInUser();

                        Response.Redirect(strURL, false);
                        return;

                    }
                    catch (SqlException ex)
                    {
                        ViewState["Account"] = null;
                        Session["AccountID"] = null;
                      
                        Session["User"] = null;
                       
                       
                        if (ex.Message.IndexOf("UQ_User_Email") > -1)
                        {
                            //LblMessage.Text = "Sorry this email address("+Email.Text+") is already used by another user. Please use another email address!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry this email address(" + Email.Text + ") is already used by another user. Please use another email address." + "');", true);
                            Email.Focus();

                        }
                        else
                        {
                            //LblMessage.Text = ex.Message;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + ex.Message + "');", true);
                        }

                        return;
                    }


                    string strEmail = SystemData.SystemOption_ValueByKey_Account("EmailFrom",null,null);
                    string strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
                    string strEmailUserName = SystemData.SystemOption_ValueByKey_Account("EmailUserName", null, null);
                    string strEmailPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);

                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(strEmail);

                    Content theContent = SystemData.Content_Details_ByKey("RegistrationEmail_New", null);
                    if (theContent == null)
                    {
                        theContent = SystemData.Content_Details_ByKey("RegistrationEmail2", null);
                    }

                    msg.Subject =theContent.Heading;
                    msg.IsBodyHtml = true;


                    //theContent.ContentP = theContent.ContentP.Replace("[First Name]", FirstName.Text);
                    theContent.ContentP = theContent.ContentP.Replace("[Email]", Email.Text.Trim());
                    theContent.ContentP = theContent.ContentP.Replace("[Password]", strAutoPassword);


                    Account theAccount = SecurityManager.Account_Details(AccountId);
                    theContent.ContentP = theContent.ContentP.Replace("[URL]", "http://" + Request.Url.Authority + Request.ApplicationPath + "/SignUp.aspx?AccountID=" + AccountId.ToString() + "&ValidationCode=" + theAccount.ConfirmationCode.ToString());



                    msg.Body = theContent.ContentP;// Sb.ToString();
                    
                    SmtpClient smtpClient = new SmtpClient(strEmailServer);
                    smtpClient.Timeout = 99999;
                    smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
                    smtpClient.Port = DBGurus.StringToInt(SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null));
                    smtpClient.EnableSsl = Convert.ToBoolean(SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null));
                    //Send BCC to Global user              

                    List<User> lstGlobalUser = SecurityManager.User_ByRoleType("1");

                    foreach (User oUser in lstGlobalUser)
                    {
                        //MailAddress bcc = new MailAddress(oUser.Email);
                        //msg.Bcc.Add(bcc);

                        msg.To.Add(oUser.Email);
                        
                    }
                    try
                    {
#if (!DEBUG)
                        smtpClient.Send(msg);
#endif
                    }
                    catch (Exception ex)
                    {

                        //LblMessage.Text = "Sorry we have had a problem sending the email. Please contact us to resolve this issue.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SendEmail", "alert('" + "Sorry we have had a problem sending the email. Please contact us to resolve this issue." + "');", true);
                        return;
                    }

                    //Response.Redirect("~/AccountCreated.aspx?Contentkey=" + Cryptography.EncryptStatic("AccountCreated") + "&Email=" + Cryptography.Encrypt(Email.Text.Trim()), false);

                }
                catch (Exception ex)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry we have failed to create the account, please try again." + "');", true);
                    return;
                }

            
          

        }
    

       

    }

    protected void lnkResendWelEmail_Click(object sender, EventArgs e)
    {

        string strEmail = SystemData.SystemOption_ValueByKey_Account("EmailFrom",null,null);
        string strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
        string strEmailUserName = SystemData.SystemOption_ValueByKey_Account("EmailUserName", null, null);
        string strEmailPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress(strEmail);

        Content theContent = SystemData.Content_Details_ByKey("RegistrationEmail2", null);


        msg.Subject = theContent.Heading;
        msg.IsBodyHtml = true;

        User etUser = SecurityManager.User_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["UserID"].ToString())));

        int? iAccountID = SecurityManager.GetPrimaryAccountID((int)etUser.UserID);

        theContent.ContentP = theContent.ContentP.Replace("[First Name]",etUser.FirstName);
        theContent.ContentP = theContent.ContentP.Replace("[Email]", etUser.Email);
        theContent.ContentP = theContent.ContentP.Replace("[Password]", etUser.Password);


        Account theAccount = SecurityManager.Account_Details((int)iAccountID);
        theContent.ContentP = theContent.ContentP.Replace("[URL]", "http://" + Request.Url.Authority + Request.ApplicationPath + "/SignUp.aspx?AccountID=" + iAccountID.ToString() + "&ValidationCode=" + theAccount.ConfirmationCode.ToString());

        
        

        msg.Body = theContent.ContentP;// Sb.ToString();
        msg.To.Add(etUser.Email);
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
        }
        catch (Exception ex)
        {

            //LblMessage.Text = "Sorry we have had a problem sending the email. Please contact us to resolve this issue.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry we have had a problem sending the email. Please contact us to resolve this issue." + "');", true);
            return;
        }


        //lnkFinish_Click(null, null);
    }

    protected void lnkSkipConfirmEmail_Click(object sender, EventArgs e)
    {
        //lnkFinish_Click(null, null);
    }

  


    protected void SignInUser()
    {
        User etUser = (User)Session["User"];
        Session["GridPageSize"] = SystemData.SystemOption_ValueByKey_Account("GridPageSize",null,null);


        int? iAccountID = SecurityManager.GetPrimaryAccountID((int)etUser.UserID);
        Session["User"] = etUser;
        Session["AccountID"] = iAccountID;
        //update session id

        SecurityManager.User_SessionID_Update((int)etUser.UserID, Session.SessionID.ToString(),(int)iAccountID);

        ///

        string strUserInfor = string.Format("{0};{1};{2};{3};{4}", etUser.Email, etUser.Password, etUser.FirstName, etUser.UserID, iAccountID);

        FormsAuthentication.SetAuthCookie(strUserInfor, false);

        HttpCookie oUseInfor = new HttpCookie("UserInformation", "");
        oUseInfor.Expires = DateTime.Now.AddDays(-3d);
        Response.Cookies.Add(oUseInfor);

        int iTN = 0;
        //List<UserRole> uroles = SecurityManager.UserRole_Select(null, etUser.UserID, null, null, null, "", "", null, null, ref iTN);

        string roletype = "";
        //foreach (UserRole item in uroles)
        //{
        //    roletype += item.RoleType.ToString() + ",";
        //}
        //if (roletype.Length > 1)
        //    roletype = roletype.Substring(0, roletype.Length - 1);

        roletype = SecurityManager.GetUserRoleTypeID((int)etUser.UserID, (int)iAccountID);

        Session["roletype"] = roletype;

        //Insert Usage
        try
        {
            Usage theUsage = new Usage(null, iAccountID, DateTime.Now, 1, 0);

            SecurityManager.Usage_Insert(theUsage);
        }
        catch
        {
        }


    }

    protected void lnkNext2_Click(object sender, EventArgs e)
    {
        
      

        User _ObjUser = (User)Session["User"];
        SignInUser();
      



    }

   


    


    protected void Email_TextChanged(object sender, EventArgs e)
    {
        lblEmailErrorMsg.Text="";
        lblEmailErrorMsg.Font.Bold = false;
        lblEmailErrorMsg.ForeColor = System.Drawing.Color.Red;
        if (Email.Text == "")
        {
            lblEmailErrorMsg.Text = "Email Address is required";
            Email.Focus();
            return;
        }        
        else
        {
            if (Common.IsEmailFormatOK(Email.Text) == false)
            {

                lblEmailErrorMsg.Text = "Invalid Email";
                return;
            }
        }

        DataTable dtTemp = Common.DataTableFromText("SELECT UserID FROM [User] WHERE Email='"+Email.Text.Replace("'","''")+"'");

        if (dtTemp.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry this email address(" + Email.Text + ") is already used by another user. Please use another email address." + "');", true);
            lblEmailErrorMsg.Text = "Not Available";
            lblEmailErrorMsg.Font.Bold = true;
            Email.Focus();
            return;
        }

        lblEmailErrorMsg.Font.Bold = true;
        lblEmailErrorMsg.ForeColor = System.Drawing.Color.Green;
        lblEmailErrorMsg.Text = "Available";
        txtRegPassword.Focus();
    }


    protected void Account_TextChanged(object sender, EventArgs e)
    {
        lblAccountErrorMsg.Text = "";
        lblAccountErrorMsg.Font.Bold = false;
        lblAccountErrorMsg.ForeColor = System.Drawing.Color.Red;
        if (txtAccount.Text == "")
        {
            lblAccountErrorMsg.Text = "Account Name is required";
            txtAccount.Focus();
            return;
        }
       

        DataTable dtTemp = Common.DataTableFromText("SELECT AccountID FROM [Account] WHERE AccountName='" + txtAccount.Text.Replace("'", "''") + "'");

        if (dtTemp.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry this Account Name(" + txtAccount.Text.Replace("'", "") + ") is already used by another user. Please use another Account Name." + "');", true);
            lblAccountErrorMsg.Text = "Not Available";
            lblAccountErrorMsg.Font.Bold = true;
            txtAccount.Focus();
            return;
        }

        lblAccountErrorMsg.Font.Bold = true;
        lblAccountErrorMsg.ForeColor = System.Drawing.Color.Green;
        lblAccountErrorMsg.Text = "Available";
        lnkNext1.Focus();
    }
    protected int spCreateAllViews(int AccountID, string DatabaseUser)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spCreateAllViews", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("AccountID", AccountID));
                command.Parameters.Add(new SqlParameter("DatabaseUser", DatabaseUser));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }
   
   
}

