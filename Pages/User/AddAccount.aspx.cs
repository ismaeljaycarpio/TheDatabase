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


public partial class Pages_User_AddAccount : SecurePage
{


    User _ObjUser;
    int? _iAccountID = null;
    Account _theAccount;
    bool _bUserDetailPage = true;
    UserRole _CurrentUserRole = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        //_ObjUser=SecurityManager.User_Details(int.Parse(Request.QueryString["userid"].ToString()));

        _iAccountID = SecurityManager.GetPrimaryAccountID((int)_ObjUser.UserID);

        _CurrentUserRole = (UserRole)Session["UserRole"];
        
        if (!IsPostBack)
        {
            ViewState["attempcount"] = 0;

            if (Request.QueryString["menu"] != null)
            {
                hlCancel.Visible = true;
                hlBack.Visible = false;
                hlCancel.NavigateUrl = "~/Default.aspx";
            }
            
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }
                          
         
            //hfAccountID.Value = Cryptography.Decrypt( Request.QueryString["AccountID"].ToString().Trim());
          
        }
        else
        {
            
            //if (ViewState["password"] != null)
            //{
            //    txtPassword.Text = ViewState["password"].ToString();
            //}
            
        }

//        string strHelpJS = @" $(function () {
//            $('#hlHelpCommon').fancybox({
//                scrolling: 'auto',
//                type: 'iframe',
//                'transitionIn': 'elastic',
//                'transitionOut': 'none',               
//                titleShow: false
//            });
//        });";


//        ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);


                
    }




    protected void PopulateRole()
    {
        ddlBasicRoles.Items.Clear();

        ddlBasicRoles.DataSource = Common.DataTableFromText("SELECT * FROM [Role] WHERE (IsActive IS NULL OR IsActive=1) AND AccountID="
            + hfSelectedAccount.Value + " ORDER BY [Role]");
        ddlBasicRoles.DataBind();

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddlBasicRoles.Items.Insert(0, liSelect);
          if (_CurrentUserRole.IsAccountHolder == null || (bool)_CurrentUserRole.IsAccountHolder == true)
          {

          }
          else
          {
              if (ddlBasicRoles.Items.FindByText("Administrator") != null)
                  ddlBasicRoles.Items.Remove(ddlBasicRoles.Items.FindByText("Administrator"));
          }




    }

    protected void lnkVerify_Click(object sender, EventArgs e)
    {
        //lblAccountName.Text = "";
        //trSelectAccount.Visible = false;
        //trRoles.Visible = false;

        ddlBasicRoles.Items.Clear();

        hfSelectedAccount.Value = "";
        User etUser = SecurityManager.User_LoginByEmail(txtEmail.Text, txtPassword.Text);
       
        if (etUser != null)
        {
            //int? iAccountID = SecurityManager.GetPrimaryAccountID((int)etUser.UserID, null, null);
            if ((bool)etUser.IsActive)
            {
                //Account theAccount = SecurityManager.Account_Details((int)iAccountID);


                        DataTable dtAdminAccount = Common.DataTableFromText(@"SELECT UserRole.AccountID,AccountName 
            FROM [Role] INNER JOIN UserRole ON
            [Role].RoleID=UserRole.RoleID INNER JOIN Account ON
            UserRole.AccountID=Account.AccountID
            WHERE Account.IsActive=1 AND [UserRole].IsPrimaryAccount=1 AND UserID=" + etUser.UserID.ToString() + @" AND RoleType='2'");


                        if (dtAdminAccount.Rows.Count > 0)
                        {
                            // wow found admin user accounts

                            hfSelectedAccount.Value = dtAdminAccount.Rows[0]["AccountID"].ToString();
                            PopulateRole();
                           //trSelectAccount.Visible = true;
                            trRoles.Visible = true;
                            divSave.Visible = true;
                            divVerify.Visible = false;
                            //trPassword.Visible = false;
                            //ViewState["password"] = txtPassword.Text;
                            txtPassword.Attributes.Add("value", txtPassword.Text);

                            //txtEmail.Enabled = false;
                            //txtEmail.Style.Add("", "");

                            //ddlSelectAccount.DataSource = dtAdminAccount;
                            //ddlSelectAccount.DataBind();
                        }
                        else
                        {
                            trRoles.Visible = false;
                            divSave.Visible = false;
                            divVerify.Visible = true;
                            txtPassword.Attributes.Add("value", "");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Sorry but that is not an admin user or the account is not active.');", true);
                            return;
                        }

                    
                
               
            }
            else
            {
                txtPassword.Attributes.Add("value", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('User is not active.');", true);

            }

        }
        else
        {
            ViewState["attempcount"] = int.Parse(ViewState["attempcount"].ToString()) + 1;
            txtPassword.Attributes.Add("value", "");

            if (int.Parse(ViewState["attempcount"].ToString()) == 3 ||
                int.Parse(ViewState["attempcount"].ToString()) > 3)
            {
                SendEmail();
                string strContuctUsURL = SystemData.SystemOption_ValueByKey_Account("ContactUsPage",null,null);
                txtEmail.Enabled = false;
                txtPassword.Enabled = false;
                ddlBasicRoles.Enabled = false;
                divSave.Visible = false;
                lblMsg.Text = @"You have entered the incorrect login details 3 times. If you are having problems 
                please use the password reminder functionality or contact " + "<a href='"+strContuctUsURL+@"' target='_blank'>technical support</a>";

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem2", "parent.$.fancybox.close(); ", true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem2", "setTimeout(function () { parent.$.fancybox.close(); }, 30000);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Sorry the Email/Password is incorrect.');", true);
            }


            return;

            

        }


    }



    protected override void OnPreInit(EventArgs e)
    {
        _ObjUser = SecurityManager.User_Details(int.Parse(Request.QueryString["UserID"].ToString()));
        //_iAccountID = SecurityManager.GetPrimaryAccountID((int)_ObjUser.UserID, null, null);

        if (Request.QueryString["menu"] != null)
        {
            _bUserDetailPage = false;
            _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

            if (_theAccount.MasterPage != "")
            {
                Page.MasterPageFile = _theAccount.MasterPage;
            }
            else
            {
                Page.MasterPageFile = "~/Home/Home.master";
            }
        }

       

    }




    protected void SendEmail()
    {
        //
        string strEmail = SystemData.SystemOption_ValueByKey_Account("EmailFrom",null,null);
        string strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer",null,null);
        string strEmailUserName = SystemData.SystemOption_ValueByKey_Account("EmailUserName", null, null);
        string strEmailPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress(strEmail);


        msg.Subject = "A user failed attempts to link another Account.";
        msg.IsBodyHtml = true;


        msg.Body = @"Following user had tried 3 times to link another Account.
                    UserID:" + _ObjUser.UserID.ToString() + @" , Email:" + _ObjUser.Email.ToString() +
                            " & Password:" + _ObjUser.Password;

        SmtpClient smtpClient = new SmtpClient(strEmailServer);
        smtpClient.Timeout = 99999;
        smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
        smtpClient.Port = DBGurus.StringToInt(SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null));
        smtpClient.EnableSsl = Convert.ToBoolean(SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null));

        //msg.To.Add("r_mohsin@yahoo.com");//
        List<User> lstGlobalUser = SecurityManager.User_ByRoleType("1");

        foreach (User oUser in lstGlobalUser)
        {
            MailAddress bcc = new MailAddress(oUser.Email);
            msg.To.Add(bcc);
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
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry we have had a problem sending the email. Please contact us to resolve this issue." + "');", true);
            //return;
        }

    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        //lnkVerify_Click(null, null);

        if (hfSelectedAccount.Value!="")
        {

        }
        else
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('There are no Accounts for it.');", true);
            return;
        }
    

        try
        {

           
            //lblAccountName.Text = "";


            User etUser = SecurityManager.User_LoginByEmail(txtEmail.Text, txtPassword.Text);
            //int? iAccountID = SecurityManager.GetPrimaryAccountID((int)etUser.UserID, null, null);
            if (etUser != null)
            {
                if ((bool)etUser.IsActive)
                {
                    Account theAccount = SecurityManager.Account_Details(int.Parse(hfSelectedAccount.Value));

                    if ((bool)theAccount.IsActive)
                    {
                        if (theAccount.AccountID == _iAccountID)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('You can not add your base account.');", true);
                        }
                        else
                        {
                           

                            //int iTN = 0;
                            //List<Role> lstRole = SecurityManager.Role_Select(null, "", ddlBasicRoles.SelectedValue, "", null,
                            //    null, "", "", null, null, ref iTN, int.Parse(Session["AccountID"].ToString()), null, null);
                            //Role theRole = new Role(null, "", "", "", null, null);
                            //foreach (Role tempRole in lstRole)
                            //{
                            //    theRole = tempRole;
                            //}

                            UserRole newUserRole = new UserRole(null, _ObjUser.UserID,int.Parse(ddlBasicRoles.SelectedValue) , DateTime.Now, DateTime.Now);
                            newUserRole.AccountID = (int)theAccount.AccountID;
                            newUserRole.IsPrimaryAccount = false;

                            Role theRole = SecurityManager.Role_Details(int.Parse(ddlBasicRoles.SelectedValue));
                            newUserRole.IsAdvancedSecurity = !theRole.IsSystemRole;

                            SecurityManager.UserRole_Insert(newUserRole);

                            //if (Request.QueryString["menu"] != null)
                            //{
                                Session["LoginAccount"] = null;

                                if (theAccount.HideMyAccount != null && (bool)theAccount.HideMyAccount)
                                {
                                    Session["LoginAccount"] = "AccountID=" + theAccount.AccountID.ToString();

                                }

                                Session["GridPageSize"] = SystemData.SystemOption_ValueByKey_Account("GridPageSize", theAccount.AccountID, null);
                                etUser = _ObjUser;
                                Session["User"] = etUser;
                                Session["AccountID"] = theAccount.AccountID;


                                //update session id

                                SecurityManager.User_SessionID_Update((int)etUser.UserID, Session.SessionID.ToString(), (int)theAccount.AccountID);

                                ///

                                string strUserInfor = string.Format("{0};{1};{2};{3};{4}", etUser.Email, etUser.Password, etUser.FirstName, etUser.UserID, theAccount.AccountID);

                                FormsAuthentication.SetAuthCookie(strUserInfor, false);

                                HttpCookie oUseInfor = new HttpCookie("UserInformation", "");
                                oUseInfor.Expires = DateTime.Now.AddDays(-3d);
                                Response.Cookies.Add(oUseInfor);


                                string roletype = "";
                                roletype = SecurityManager.GetUserRoleTypeID((int)etUser.UserID, (int)theAccount.AccountID);



                                Session["roletype"] = roletype;

                                UserRole theUserRole = SecurityManager.GetUserRole((int)etUser.UserID, (int)theAccount.AccountID);

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
                                    Usage theUsage = new Usage(null, theAccount.AccountID, DateTime.Now, 1, 0);

                                    SecurityManager.Usage_Insert(theUsage);
                                }
                                catch
                                {
                                    //
                                }

                                if (roletype.Length > 0)
                                {
                                    if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())))
                                    {
                                        Session["DoNotAllow"] = "true";
                                    }

                                }

                                if (Request.QueryString["menu"] != null)
                                {
                                    Response.Redirect(hlCancel.NavigateUrl, true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AddAccount", "GetBackValue();", true);
                                }

                               

                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AddAccount", "GetBackValue();", true);
                            //}
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Account is not active.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('User is not active.');", true);

                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Login failed');", true);

            }


          
            


           

        }
        catch (Exception ex)
        {
            if (ex.Message.IndexOf("IX_UserIDAccountID") > -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('You have already added this account.');", true);
            }
            else
            {

                ErrorLog theErrorLog = new ErrorLog(null, "Add Account", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please try again.');", true);

            }
        }


    }

    

  


   


}
