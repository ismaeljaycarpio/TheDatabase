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
using System.Web.UI.HtmlControls;

public partial class Login2 : System.Web.UI.Page
{

    bool bHideRegister = false;
    protected void Page_PreInit(object sender, EventArgs e)
    {

        if (Request.QueryString["Account"] != null)
        {
            Session["client"] = Request.QueryString["Account"].ToString();
            Page.MasterPageFile = SystemData.SystemOption_ValueByKey_Account(Request.QueryString["Account"].ToString().ToUpper() + "_LoginMasterPage",null,null);

            if (Request.QueryString["Account"].ToString().ToUpper() == "RRP")
            {
                bHideRegister = true;
            }
        }
        else
        {
            if (SystemData.SystemOption_ValueByKey_Account("LoginMasterPage",null,null) != "")
            {
                if (SystemData.SystemOption_ValueByKey_Account("LoginMasterPage", null, null).ToUpper() != "NA")
                {
                    Page.MasterPageFile = SystemData.SystemOption_ValueByKey_Account("LoginMasterPage", null, null).ToString();
                    if (SystemData.SystemOption_ValueByKey_Account("AccountKey", null, null).ToLower() == "rrp")
                    {
                        bHideRegister = true;
                    }
                }
            }
        }
    }
    protected void LogOutTDB()
    {
        try
        {
            txtLogInEmail.Text = "";
            txtLogInPassword.Text = "";
            chkRememberMe.Checked = false;
            txtLogInEmail.Focus();
            Session["User"] = null;
            Session.Abandon();

            FormsAuthentication.SignOut();
            HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
            oUseInfor.Expires = DateTime.Now.AddDays(-3d);
            Response.Cookies.Add(oUseInfor);
            Session.Clear();
            Response.Redirect("~/Login.aspx", true);
        }
        catch
        {
            //
        }
        
    }
    protected void Page_Load(object sender, EventArgs e)
    {




        string strNotLogedInUser = "  <p style='font-size:11pt;'> Sorry but the page you are trying to access is restricted to signed in users. Please sign in to continue. </p>";
        string strErrorMessage = "  <p style='font-size:11pt;'> Oops… a system error has occurred and our technicians have been notified. Please sign in again. </p>";

        try
        {


            if (!Page.IsPostBack)
            {


                string strRunSpeedLog = SystemData.SystemOption_ValueByKey_Account("RunSpeedLog",null,null);

                string strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation", null, null);
                string strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath", null, null);

                if (strFilesLocation != "")
                {
                    Session["FilesLocation"] = strFilesLocation;
                }
                else
                {
                    Session["FilesLocation"] = "http://" + Request.Url.Authority + Request.ApplicationPath;
                }


                if (strFilesPhisicalPath != "")
                {
                    Session["FilesPhisicalPath"] = strFilesPhisicalPath;
                }
                else
                {
                    Session["FilesPhisicalPath"] = Server.MapPath("~");
                }


                if (strRunSpeedLog != "" && strRunSpeedLog.ToLower() == "yes")
                {
                    Session["RunSpeedLog"] = strRunSpeedLog;

                }
                else
                {
                    Session["RunSpeedLog"] = null;
                }


                if (Request.Browser.IsMobileDevice || Request.UserAgent.Contains("Android"))
                {
                    //Response.Redirect("~/Pages/Mobile/Login.aspx");
                    Session["IsMobile"] = "yes";
                }
                else
                {

                    if (Request.Cookies["IsMobile"] == null)
                    {
                        string u = Request.ServerVariables["HTTP_USER_AGENT"];
                        Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                        {
                            //Response.Redirect("~/Pages/Mobile/Login.aspx");
                            Session["IsMobile"] = "yes";
                        }
                    }

                }






                Title = "Login";


                if (Request.QueryString["frompublic"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Information has been saved successfully.');", true);

                }

                if (Request.QueryString["AccountID"] != null)
                {
                    hlForgotPassword.NavigateUrl = hlForgotPassword.NavigateUrl + "?AccountID=" + Request.QueryString["AccountID"].ToString();
                    Account theAccount = SecurityManager.Account_Details(int.Parse(Request.QueryString["AccountID"].ToString()));

                    if (theAccount != null)
                    {

                        divSignUp.Visible = false;
                        if (theAccount.LoginContentID != null)
                        {
                            Content theContent = SystemData.Content_Details((int)theAccount.LoginContentID);
                            if (theContent != null)
                            {
                                //h1Heading.InnerText = theContent.Heading;
                                lblContentCommon.Text = theContent.ContentP;
                            }

                        }
                        Title = theAccount.AccountName + " Login";
                    }

                }
                else
                {
                    //h1Heading.InnerText = "";

                    //divContentCommon.Visible = false;
                }

                if (Request.QueryString["AccountID"] == null)
                {
                    Content theContent = SystemData.Content_Details_ByKey("LoginScreenContent", null);
                    if (theContent != null)
                    {
                        lblContentCommon.Text = theContent.ContentP;
                    }
                }



                string strLoginPageContentKey = SystemData.SystemOption_ValueByKey_Account("LoginPageContentKey", null, null);
                if (strLoginPageContentKey.Trim()!="")
                {
                    Content theContent = SystemData.Content_Details_ByKey(strLoginPageContentKey, null);
                    if (theContent != null)
                    {
                        hlContentCommonEdit.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&allowadmin=yes&fixedurl=~/Login.aspx?Logout=yes&ContentID=" + Cryptography.Encrypt(theContent.ContentID.ToString());
                        hlContentCommonEdit.Visible = true;
                        lblContentCommon.Text = theContent.ContentP;
                    }
                }


                if (Request.QueryString["ReturnURL"] != null)
                {
                    if (Request.QueryString["ReturnURL"].ToString().Trim() != "/")
                    {
                        lblContentCommon.Text = strNotLogedInUser;
                    }
                }

                lblContentCommon2.Text = @"<strong style='font-size: 20px;'>New to the TheDatabase?</strong><br />  <br />
                                                                                            <p>Get started now. It quick, easy and free!</p>
                                                                                            <br />";

                if (bHideRegister)
                {
                    Content theContent = SystemData.Content_Details_ByKey("LoginScreenContent_RRP", null);
                    if (theContent != null)
                    {
                        lblContentCommon2.Text = theContent.ContentP;
                        trCommonContentHeight.Visible = false;
                        trSignUp.Visible = false;
                        tdContentCommon2.Align = "left";

                    }

                }
                else
                {
                    tdContentCommon2.Style.Add("align", "center");
                }

                if (Request.QueryString["ErrorLogID"] != null)
                {
                    lblContentCommon.Text = strErrorMessage;
                    lblContentCommon.ForeColor = System.Drawing.Color.Red;
                }
                

                if (Request.Cookies["UserInformation"] != null)
                {
                    //DateTime dtExpires
                    if (Request.QueryString["Logout"] != null)
                    {
                        LogOutTDB();
                        return;
                    }
                    else
                    {

                        string oUseInfor = Request.Cookies["UserInformation"].Value;
                        string[] aUserInfor = oUseInfor.Split('/');
                        txtLogInEmail.Text = aUserInfor.GetValue(0).ToString();
                        txtLogInPassword.Text = aUserInfor.GetValue(1).ToString();
                        chkRememberMe.Checked = true;
                        lnkLogIn_Click(null, null);
                    }
                }
                else
                {                    
                    if (Request.QueryString["Logout"] != null)
                    {
                        LogOutTDB();                      
                        return;
                    }
                    else
                    {
                        txtLogInEmail.Text = "";
                        txtLogInPassword.Text = "";
                        chkRememberMe.Checked = false;
                        txtLogInEmail.Focus();
                        HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
                        oUseInfor.Expires = DateTime.Now.AddDays(-3d);
                        Response.Cookies.Add(oUseInfor);
                    }
                }


                if (Request.QueryString["Password"] != null)
                {
                    if (Request.QueryString["Email"] != null)
                    {

                        txtLogInEmail.Text = Request.QueryString["Email"].ToString();

                        if (Session["DemoEmail"] != null)
                        {
                            if (txtLogInEmail.Text.ToLower() == Session["DemoEmail"].ToString().ToLower())
                            {
                                txtLogInPassword.Text = Request.QueryString["Password"].ToString();
                            }
                        }

                    }


                    //txtLogInPassword.Text = Request.QueryString["Password"].ToString();
                }
                else
                {

                    if (Request.QueryString["Email"] != null)
                    {
                        try
                        {

                            txtLogInEmail.Text = Cryptography.Decrypt(Request.QueryString["Email"].ToString());
                            if (txtLogInEmail.Text.Trim() == "")
                            {
                                txtLogInEmail.Text = Request.QueryString["Email"].ToString();
                            }

                        }
                        catch
                        {
                            //txtLogInEmail.Text = Request.QueryString["Email"].ToString();
                        }

                    }
                }
                if (Request.QueryString["RememberMe"] != null)
                {

                    if (Request.QueryString["RememberMe"].ToString().ToLower() == "yes")
                    {
                        chkRememberMe.Checked = true;
                    }
                }

                if (Request.QueryString["Email"] != null && Request.QueryString["Password"] != null)
                {
                    lnkLogIn_Click(null, null);

                }


                //if (Request.QueryString["Logout"]!=null && Request.QueryString["Logout"].ToString() == "concurrent")
                //{
                //    if (!IsPostBack)
                //    {
                //        lblError.Visible = true;
                //        lblError.Text = "You can only sign in from one computer at a time. Please sign in using a different email.";

                //    }

                //}




            }
            txtLogInPassword.Attributes.Add("Value", txtLogInPassword.Text);

            //hlTerms.NavigateUrl = "~/DisplayContent.aspx?Contentkey=" + Cryptography.EncryptStatic("DBGTermsConditions");

            if (!IsPostBack)
            {
                if (Request.Url.AbsoluteUri.IndexOf("http://oilpaw.thedatabase.net") > -1)
                {
                    //hlSignUp.Visible = false;
                    //divSignUp.Visible = false;
                    //tblRightPanel.Visible = false;
                }


            }


        }
        catch (Exception ex)
        {
            //
        }


    }





    protected void lnkLogIn_Click(object sender, EventArgs e)
    {
        //lblError.Visible = false;

        try
        {
            //if(IsPostBack)
            //{
            //    Session["IsFlashSupported"] = hfFlashSupport.Value;   

            //}

            if (hfScreenWidth.Value != "")
            {
                Session["ScreenWidth"] = hfScreenWidth.Value;
            }
            else
            {
                
                   
                    hfScreenWidth.Value = Request.Browser.ScreenPixelsWidth.ToString();
                    Session["ScreenWidth"] = hfScreenWidth.Value;
                

            }

                 

            //if (Session["IsMobile"] == null)
            //{
            //    if (Session["ScreenWidth"] != null)
            //    {
            //        if (int.Parse(Session["ScreenWidth"].ToString()) < 1000)
            //        {
            //            Session["IsMobile"] = "yes";
            //        }
            //    }
            //}
        }
        catch
        {
            //
        }


        StringBuilder sb = new StringBuilder();
        string chkValid = "";
        //Username
        if (string.IsNullOrEmpty(txtLogInEmail.Text.Trim()))
        {
            chkValid += "1";
        }
        if (string.IsNullOrEmpty(txtLogInPassword.Text.Trim()))
        {
            chkValid += "2";
        }
        if (!string.IsNullOrEmpty(chkValid))
        {
            sb.AppendLine("Please correct the following errors:");
            if (chkValid.Equals("1"))
            {
                txtLogInEmail.Focus();
                sb.AppendLine(Resources.Login.UserReq);
            }
            else
                if (chkValid.Equals("2"))
                {
                    txtLogInPassword.Focus();
                    sb.AppendLine(Resources.Login.PassReq);
                }
                else
                {
                    txtLogInEmail.Focus();
                    sb.AppendLine(Resources.Login.UserReq);
                    sb.AppendLine(Resources.Login.PassReq);
                }
            string messerr = sb.ToString().Replace("\r\n", "");
            //Alert.Show(messerr);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginErr", "alert('" + messerr + "');", true);
        }
        else
        {
            //Check Username and password
            //string strLogInEmail = DBGurus.StandardString(txtLogInEmail.Text);
            //string pass = DBGurus.StandardString(txtLogInPassword.Text);

            string strLogInEmail = txtLogInEmail.Text;
            string pass = txtLogInPassword.Text;

            //int iTemp = 0;
            //List<User> listUser = SecurityManager.User_Select(-1, user, "", "", "", pass , null, null, null, -1, "FullName", "ASC", -1, -1, ref iTemp);

            User etUser = SecurityManager.User_LoginByEmail(strLogInEmail, pass);


            if (etUser != null)
            {
                //check if the user has used code or not

                SecurityManager.User_LoginCount_Increment((int)etUser.UserID);
                int? iAccountID = SecurityManager.GetPrimaryAccountID((int)etUser.UserID);

                Account theAccount = SecurityManager.Account_Details((int)iAccountID);

                if (theAccount != null)
                {
                    //if (theAccount.ConfirmationCode != "" )
                    //{
                    //    if ((bool)etUser.IsAccountHolder == false)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ProblemAlert", "alert('Please contact with Account holder to activate the account by the Confirmation code.');", true);
                    //        return;
                    //    }



                    //    Response.Redirect("~/SignUp.aspx?AccountID=" +  theAccount.AccountID.ToString() + "&UserID=" + Cryptography.Encrypt(etUser.UserID.ToString()));
                    //    return;
                    //}

                }

                //User Actived
                if ((bool)etUser.IsActive)
                {
                    //check if the account is active



                    if (theAccount.IsActive == false)
                    {
                        string strContactUs = SystemData.SystemOption_ValueByKey_Account("ContactUsPage",null,null);
                        if (strContactUs == "")
                        {
                            strContactUs = "www.dbgurus.com.au";
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "InActiveAccount", "alert('Your account (" + theAccount.AccountName + ") is not active – please contact us via our Contact Us page at " + strContactUs + "');", true);
                        return;
                    }

                    if (Request.QueryString["AccountID"] != null && theAccount.HideMyAccount != null
                        && (bool)theAccount.HideMyAccount)
                    {
                        Session["LoginAccount"] = "AccountID=" + theAccount.AccountID.ToString();
                    }
                    else
                    {
                        Session["LoginAccount"] = null;

                        if (theAccount.HideMyAccount != null && (bool)theAccount.HideMyAccount)
                        {
                            Session["LoginAccount"] = "AccountID=" + theAccount.AccountID.ToString();

                        }

                    }




                    Session["GridPageSize"] = SystemData.SystemOption_ValueByKey_Account("GridPageSize", iAccountID, null);

                    Session["User"] = etUser;
                    Session["AccountID"] = iAccountID;


                    //update session id

                    SecurityManager.User_SessionID_Update((int)etUser.UserID, Session.SessionID.ToString(),(int) iAccountID);

                    ///

                    string strUserInfor = string.Format("{0};{1};{2};{3};{4}", etUser.Email, etUser.Password, etUser.FirstName, etUser.UserID, iAccountID);

                    FormsAuthentication.SetAuthCookie(strUserInfor, false);
                    if (chkRememberMe.Checked)
                    {
                        HttpCookie oUseInfor = new HttpCookie("UserInformation", etUser.Email + "/" + etUser.Password + "/" + etUser.FirstName);
                        oUseInfor.Expires = DateTime.Now.AddDays(300d);
                        Response.Cookies.Add(oUseInfor);
                    }
                    else
                    {
                        HttpCookie oUseInfor = new HttpCookie("UserInformation", "");
                        oUseInfor.Expires = DateTime.Now.AddDays(-3d);
                        Response.Cookies.Add(oUseInfor);
                    }
                    //List<UserRole> uroles= etUser.UserRoles.ToList();

                    int iTN = 0;
                    string roletype = "";
                    roletype = SecurityManager.GetUserRoleTypeID((int)etUser.UserID, (int)iAccountID);

                    

                    Session["roletype"] = roletype;

                    UserRole theUserRole = SecurityManager.GetUserRole((int)etUser.UserID, (int)iAccountID);

                    Session["UserRole"] = theUserRole;

                    if ((bool)theUserRole.IsAdvancedSecurity)
                    {
                        if (Session["roletype"].ToString() != Common.UserRoleType.OwnData)
                        {
                            Session["roletype"] = Common.UserRoleType.ReadOnly;
                        }
                    }

                    //Insert Usage
                    try
                    {

                        if (theAccount.SPAfterLogin != "" && theAccount.SPAfterLogin.Length>0)
                       {
                           string strReturn = "";
                           strReturn=SecurityManager.Account_SPAfterLogin(theAccount.SPAfterLogin, theAccount.AccountID, etUser.UserID);
                       }


                        Usage theUsage = new Usage(null, iAccountID, DateTime.Now, 1, 0);

                        SecurityManager.Usage_Insert(theUsage);
                    }
                    catch
                    {
                    }

                    //get advanced security 

                    //DataTable dtUserTable = SecurityManager.ets_UserTable_Select(null, null, etUser.UserID, null);

                    //string RecordRightIDs = "";
                    //foreach (DataRow item in dtUserTable.Rows)
                    //{
                    //    RecordRightIDs += item["RoleType"].ToString() + ",";
                    //}

                    //Session["RecordRightIDs"] = RecordRightIDs;

                    if (roletype.Length > 0)
                    {
                        if (roletype.Contains("1"))
                        {
                            //global admin
                            FormsAuthentication.RedirectFromLoginPage(strLogInEmail, false);

                            if (Request.QueryString["ReturnURL"] != null)
                            {
                                if (Request.QueryString["ReturnURL"] != "")
                                {
                                    Response.Redirect(Request.QueryString["ReturnURL"].ToString(),false);
                                }
                                else
                                {
                                    Response.Redirect("~/Pages/Security/AccountList.aspx",false);
                                }
                            }
                            else
                            {
                                Response.Redirect("~/Pages/Security/AccountList.aspx",false);
                            }
                            return;

                        }
                        else
                        {


                            try
                            {







                                //if (theAccount.ExpiryDate != null)
                                //{
                                //    Session["ExpireLeftDay"] = Common.DaysBetween((DateTime)DateTime.Today, (DateTime)theAccount.ExpiryDate);

                                //    if (theAccount.ExpiryDate.Value.AddDays(0) < DateTime.Today)
                                //    {
                                //        Session["DoNotAllow"] = "true";

                                //    }

                                //}

                                if (theAccount.ExpiryDate != null)
                                {
                                    if (theAccount.ExpiryDate.Value.AddDays(0) < DateTime.Today)
                                    {
                                        //send email
                                        try
                                        {
                                            string sError = "";
                                            string sFrom = SystemData.SystemOption_ValueByKey_Account_Default("EmailFrom",null,null, "no-reply@dbgurus.com.au");


                                            Content theContent = SystemData.Content_Details_ByKey("ExpiredAccount", null);

                                            //get account holder
                                            User theAccountHolder = SecurityManager.User_AccountHolder((int)theAccount.AccountID);

                                            string sHeading = theContent.Heading;
                                            theContent.ContentP = theContent.ContentP.Replace("[Accountholder]", theAccountHolder.Email);
                                            theContent.ContentP = theContent.ContentP.Replace("[AccountID]", theAccount.AccountID.ToString());
                                            theContent.ContentP = theContent.ContentP.Replace("[AccountName]", theAccount.AccountName);
                                            theContent.ContentP = theContent.ContentP.Replace("[ExpiryDate]", theAccount.ExpiryDate.Value.ToLongDateString());

                                            string strBasePage = "http://" + Request.Url.Authority + Request.ApplicationPath + "/ExpiredAccountUpdate.aspx?id=" + Cryptography.Encrypt(theAccount.AccountID.ToString())
                                                + "&ed=" + Cryptography.Encrypt(theAccount.ExpiryDate.Value.ToShortDateString()) + "&cd=" + Cryptography.Encrypt(DateTime.Now.ToString());


                                            theContent.ContentP = theContent.ContentP.Replace("[URL1M]", strBasePage+ "&p=" + Cryptography.Encrypt("1M"));
                                            theContent.ContentP = theContent.ContentP.Replace("[URL3M]", strBasePage + "&p=" + Cryptography.Encrypt("3M"));
                                            theContent.ContentP = theContent.ContentP.Replace("[URL1Y]", strBasePage + "&p=" + Cryptography.Encrypt("1Y"));

                                            string strGodUserEmail = "info@dbgurus.com.au"; //"r_mohsin@yahoo.com"; 

                                            if (DBGurus.SendEmail("ExpiredAccount", null, null, sHeading, theContent.ContentP, sFrom, strGodUserEmail, "", "", null, null, out sError) != 0)
                                                {
                                                    ErrorLog theErrorLog = new ErrorLog(null, "Expiry Email", sError, sError, DateTime.Now, Request.Path);
                                                    SystemData.ErrorLog_Insert(theErrorLog);

                                                }

                                        }
                                        catch (Exception ex)
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "EmailPasswordErr", "alert('" + ex.Message + "');", true);
                                            //DBGurus.AddErrorLog(ex.Message);
                                            ErrorLog theErrorLog = new ErrorLog(null, "Password Reminder", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                                            SystemData.ErrorLog_Insert(theErrorLog);

                                        }


                                    }

                                }

                                if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())))
                                {
                                    Session["DoNotAllow"] = "true";
                                    //Response.Redirect("~/Pages/Security/AccountTypeChange.aspx?type=renew", false);

                                    if (Session["IsMobile"] != null)
                                    {
                                        if (Session["ScreenWidth"] != null)
                                        {
                                            if (int.Parse(Session["ScreenWidth"].ToString()) > 1000)
                                            {
                                                Response.Redirect("~/Default.aspx", false);
                                                return;
                                            }
                                        }
                                        Response.Redirect("~/Pages/Mobile/Default.aspx", false);


                                    }
                                    else
                                    {
                                        Response.Redirect("~/Default.aspx", false);
                                    }
                                    return;
                                }


                            }
                            catch
                            {
                                //
                            }


                            ///

                            FormsAuthentication.RedirectFromLoginPage(strLogInEmail, false);

                            string strDefaultURL = "~/Default.aspx";


                            if (Session["IsMobile"] != null)
                            {
                               
                                strDefaultURL="~/Pages/Mobile/Default.aspx";
                                if (Session["ScreenWidth"] != null)
                                {
                                    if (int.Parse(Session["ScreenWidth"].ToString()) > 1000)
                                    {
                                        strDefaultURL = "~/Default.aspx";
                                    }
                                }
                            }

                            if (theAccount.DisplayTableID != null)
                            {
                                if (strDefaultURL.IndexOf("/Mobile/") > -1)
                                {
                                    strDefaultURL = "~/Pages/Mobile/RecordList.aspx?TableID=" +
                                Cryptography.Encrypt(theAccount.DisplayTableID.ToString());
                                }
                                else
                                {

                                    strDefaultURL = "~/Pages/Record/RecordList.aspx?TableID=" +
                                 Cryptography.Encrypt(theAccount.DisplayTableID.ToString());
                                }
                            }

                            if (roletype.Contains("2"))
                            {
                                //admin user
                                List<Menu> listSTG = RecordManager.ets_Menu_List(int.Parse(Session["AccountID"].ToString()));
                                bool bTableFound = false;
                                foreach (Menu item in listSTG)
                                {

                                    List<Table> lstTable = RecordManager.ets_Table_Select(null, "", (int)item.MenuID, null, null, null, true,
                                        "TableName", "ASC", null, null, ref iTN, "");



                                    foreach (Table tempST in lstTable)
                                    {

                                        bTableFound = true;
                                        break;
                                    }
                                    if (bTableFound)
                                    {
                                        break;
                                    }

                                }

                                if (bTableFound)
                                {
                                    //Response.Redirect("~/Default.aspx", false);

                                    if (Request.QueryString["ReturnURL"] != null)
                                    {
                                        if (Request.QueryString["ReturnURL"] != "")
                                        {
                                            Response.Redirect(Request.QueryString["ReturnURL"].ToString(),false);
                                        }
                                        else
                                        {
                                            Response.Redirect(strDefaultURL, false);
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect(strDefaultURL, false);
                                    }
                                    return;
                                }
                                else
                                {
                                    Response.Redirect(strDefaultURL, false);
                                    return;
                                    //Response.Redirect("~/Pages/Record/TableOption.aspx?FirstTime=yes&MenuID=" + Cryptography.Encrypt("-1") + "&SearchCriteria=" + Cryptography.Encrypt("-1"), false);
                                }

                            }
                            else
                            {
                                //other user
                                if (Request.QueryString["ReturnURL"] != null)
                                {
                                    if (Request.QueryString["ReturnURL"] != "")
                                    {
                                        Response.Redirect(Request.QueryString["ReturnURL"].ToString(), false);
                                    }
                                    else
                                    {
                                        Response.Redirect(strDefaultURL, false);
                                    }
                                }
                                else
                                {
                                    Response.Redirect(strDefaultURL, false);
                                   
                                }
                                return;
                            }
                        }

                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "ExpiryInfo", "alert('Testing');", true);
                    }
                    else
                    {
                        //user has no role
                        if (Session["LoginAccount"] == null)
                        {
                            Session.Clear();
                            FormsAuthentication.SignOut();
                            Response.Redirect("~/Login.aspx", false);
                        }
                        else
                        {
                            string strLoginAccount = Session["LoginAccount"].ToString();
                            Session.Clear();
                            FormsAuthentication.SignOut();
                            Response.Redirect("~/Login.aspx?" + strLoginAccount, false);
                        }
                        return;
                    }
                }
                else
                {
                    txtLogInEmail.Focus();
                    //System.Exception ex = new System.Exception(Resources.Login.LoginActive);
                    //throw ex;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InActiveUser", "alert('" + Resources.Login.LoginActive + "');", true);
                }
            }
            else
            {
                txtLogInEmail.Focus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + Resources.Login.LoginErr + "');", true);
            }
        }

    }





}