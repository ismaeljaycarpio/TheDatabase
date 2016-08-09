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

public partial class SystemSignUp : SecurePage
{
    int _iSearchCriteriaID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "System Sign Up";


        try
        {


            if (!Page.IsPostBack)
            {

                if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
                {
                    User objUser = (User)Session["User"];

                    if (objUser.Email.ToLower() == SystemData.SystemOption_ValueByKey_Account("DemoAccountCreator",null,null))
                    {

                    }
                    else
                    {
                        if (Session["LoginAccount"] == null)
                        {
                            Response.Redirect("~/Login.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("~/Login.aspx?" + Session["LoginAccount"].ToString(), false);
                        }
                    }
                
                }

                PopulateAccountType();
                PopulateCountry();              


               
                if (Request.Cookies["UserInformation"] != null )
                {
                    //DateTime dtExpires

                    if (Request.QueryString["Logout"] != null)
                    {
                       
                        HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
                        oUseInfor.Expires = DateTime.Now.AddDays(-3d);
                        Response.Cookies.Add(oUseInfor);
                    }
                    else
                    {

                        string oUseInfor = Request.Cookies["UserInformation"].Value;
                        string[] aUserInfor = oUseInfor.Split('/');
                        
                    }
                    
                }
                else
                {
                   
                    HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
                    oUseInfor.Expires = DateTime.Now.AddDays(-3d);
                    Response.Cookies.Add(oUseInfor);
                }

                
                if (Request.QueryString["Password"] != null)
                {
                    if (Request.QueryString["Email"] != null)
                    {                       

                    }
                   
                }
                else
                {

                    if (Request.QueryString["Email"] != null)
                    {
                        try
                        {
                           
                        }
                        catch
                        {
                           
                        }
                       
                    }
                }
               


            }          


        }
        catch (Exception ex)
        {
            //
        }


    }



    protected void PopulateAccountType()
    {

        ddlAccountType.DataSource = Common.DataTableFromText("SELECT * FROM AccountType WHERE AccountTypeID<>4");

        ddlAccountType.DataBind();
        ddlAccountType.SelectedIndex = 1;

        //ListItem liAll = new ListItem("-Please Select-", "-1");
        //ddlAccountType.Items.Insert(0, liAll);

    }


  



    //protected void SaveButton_Click(object sender, ImageClickEventArgs e)
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        lblError.Visible = false;
        LblMessage.Text = "";
        LblMessage.Attributes.Add("style", "color:red");

        if (ddlAccountType.SelectedValue == "-1")
        {
            //LblMessage.Text = "Please select an Account Type.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Please select an Account Type." + "');", true);
            ddlAccountType.Focus();
            return;
        }


       
        if (txtRegPassword.Text.ToLower().IndexOf(txtAccount.Text.ToLower()) > -1)
        {
            //LblMessage.Text = "Password should not have account name.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "The password cannot be the same as the account name. Please select another password." + "');", true);
            txtRegPassword.Focus();
            return;
        }
        if (txtRegPassword.Text.ToLower().IndexOf(FirstName.Text.ToLower()) > -1)
        {
            //LblMessage.Text = "Password should not have first name.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Password should not have first name." + "');", true);
            txtRegPassword.Focus();
            return;
        }
        if (txtRegPassword.Text.ToLower().IndexOf(LastName.Text.ToLower()) > -1)
        {
            //LblMessage.Text = "Password should not have last name.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Password should not have last name." + "');", true);
            txtRegPassword.Focus();
            return;
        }
        if (txtRegPassword.Text.ToLower().IndexOf(Email.Text.ToLower().Substring(0, Email.Text.IndexOf("@"))) > -1)
        {
            //LblMessage.Text = "Password should not have email address.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Password should not have email address." + "');", true);
            txtRegPassword.Focus();
            return;
        }


       
            Random Rnd = new Random();
            //string strAutoPassword = txtAccount.Text.Trim() + DateTime.Now.Second.ToString();
            string strAutoPassword = txtRegPassword.Text;
            int iAccountId = 0;
            int iTN = 0;
            int iUserID = -1;

           

            try
            {
                try
                {

                    Account newAccount = new Account(null, txtAccount.Text.Trim(), null, null, ddlAccountType.SelectedValue == "-1" ? null : (int?)int.Parse(ddlAccountType.SelectedValue),
                                  (DateTime?)DateTime.Now.AddDays(14));
                    newAccount.IsActive = true;

                    newAccount.PhoneNumber = txtContactNumber.Text.Trim();
                    newAccount.CountryID = int.Parse(ddlCountry.SelectedValue);
                    newAccount.CreatedByWizard = true;

                    iAccountId = SecurityManager.Account_Insert(newAccount);

                    newAccount.AccountID = iAccountId;
                    //Now create the user
                    User newUser = new User(null, FirstName.Text.Trim(), LastName.Text.Trim(), "",
                         Email.Text.Trim(), strAutoPassword, true, null, null);
                    //newUser.UserName = newUser.Email;
                    iUserID = SecurityManager.User_Insert(newUser);
                    newUser.UserID = iUserID;
                    //Now assign the user as a role type 2(admin)
                    List<Role> lstRoles = SecurityManager.Role_Select(null, "", "2", "", null, null,
                        "RoleID", "ASC", null, null, ref iTN, int.Parse(Session["AccountID"].ToString()), null, null);

                    if (lstRoles.Count > 0)
                    {
                        UserRole newUserRole = new UserRole(null, iUserID, lstRoles[0].RoleID, null, null);
                        newUserRole.AccountID = iAccountId;
                        newUserRole.IsAccountHolder = true;
                        newUserRole.IsPrimaryAccount = true;
                        int iUserRoleID = SecurityManager.UserRole_Insert(newUserRole);

                    }

                   

                    Session["GridPageSize"] = SystemData.SystemOption_ValueByKey_Account("GridPageSize",null,null);

                    Session["User"] = newUser;
                    Session["AccountID"] = iAccountId;


                    string strUserInfor = string.Format("{0};{1};{2};{3};{4}", newUser.Email, newUser.Password, newUser.FirstName, newUser.UserID, iAccountId);

                    FormsAuthentication.SetAuthCookie(strUserInfor, false);

                    
                    //List<UserRole> uroles = SecurityManager.UserRole_Select(null, newUser.UserID, null, null, null, "", "", null, null, ref iTN);

                    //string roletype = "";
                    //foreach (UserRole item in uroles)
                    //{
                    //    roletype += item.RoleType.ToString() + ",";
                    //}

                    string roletype = "";
                    roletype = SecurityManager.GetUserRoleTypeID((int)newUser.UserID, (int)iAccountId);

                    Session["roletype"] = roletype;




                }
                catch (SqlException ex)
                {
                   
                    if (ex.Message.IndexOf("UQ_AccountName") > -1 || ex.Message.IndexOf(" BEGIN and COMMIT") > -1)
                    {
                        //lets get available Account Name
                        DataTable dtNextAccount = Common.DataTableFromText("select dbo.fnNextAvailableAccountName('" + txtAccount.Text.Replace("'", "''") + "')");
                        //LblMessage.Text = "Sorry that Account Name is not available.  Please choose another.  You could try - " + dtNextAccount.Rows[0][0].ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry that Account Name is not available.  Please choose another.  You could try - " + dtNextAccount.Rows[0][0].ToString() + "');", true);
                        txtAccount.Focus();

                    }
                    else if (ex.Message.IndexOf("UQ_User_Email") > -1)
                    {
                        //LblMessage.Text = "Sorry this email address("+Email.Text+") is already used by another user. Please use another email address!";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry this email address(" + Email.Text + ") is already used by another user as a Username. Please use another email address or change previous user Username." + "');", true);
                        Email.Focus();

                    }
                    else
                    {
                        LblMessage.Text = ex.Message;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + ex.Message + "');", true);
                    }

                    return;
                }


                //evrything is ok so lets make ConfirmationCode=null

                Common.ExecuteText("UPDATE Account SET IsActive=1, ConfirmationCode=NULL WHERE AccountID=" + Session["AccountID"].ToString());


                string strEmail = SystemData.SystemOption_ValueByKey_Account("EmailFrom",null,null);
                string strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
                string strEmailUserName = SystemData.SystemOption_ValueByKey_Account("EmailUserName", null, null);
                string strEmailPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(strEmail);

                Content theContent = SystemData.Content_Details_ByKey("RegistrationEmailWizard",null);


                msg.Subject =theContent.Heading;
                msg.IsBodyHtml = true;
                                              

                Account theAccount = SecurityManager.Account_Details(iAccountId);
                if (Session["LoginAccount"] == null)
                {
                    theContent.ContentP = theContent.ContentP.Replace("[URL]", "http://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx?email=" + Email.Text);
                }
                else
                {
                    theContent.ContentP = theContent.ContentP.Replace("[URL]", "http://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx?email=" + Email.Text + "&" + Session["LoginAccount"].ToString());
                }

                DataTable theSPTable = SystemData.Run_ContentSP("ets_RegistrationEmailWizard", iUserID.ToString());
                theContent.ContentP = Common.ReplaceDataFiledByValue(theSPTable, theContent.ContentP);
                

                msg.Body = theContent.ContentP;// Sb.ToString();
                msg.To.Add("info@dbgurus.com.au");
                SmtpClient smtpClient = new SmtpClient(strEmailServer);
                smtpClient.Timeout = 99999;
                smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

                smtpClient.Port = DBGurus.StringToInt(SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null));
                smtpClient.EnableSsl = Convert.ToBoolean(SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null));

                
                try
                {
#if (!DEBUG)
                    smtpClient.Send(msg);
#endif
                }
                catch (Exception ex)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry we have had a problem sending the email. Please contact us to resolve this issue." + "');", true);
                    return;
                }


                Response.Redirect("~/Pages/Security/AccountDetail.aspx?wizard=yes&mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(Session["AccountID"].ToString()) , false);
                //Response.Redirect("~/DisplayContent.aspx?Contentkey=" + Cryptography.EncryptStatic("AccountCreated") + "&Email=" + Cryptography.Encrypt(Email.Text.Trim()), false);

            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + "Sorry we have failed to create the account, please try again." + "');", true);
                return;
            }

     
    }


    protected void PopulateCountry()
    {

        int iTN = 0;
        ddlCountry.DataSource = SystemData.LookUpData_Select(null, -1, "", "", null, null,
             "DisplayText", "ASC", null, null, ref iTN,  "");
        ddlCountry.DataBind();

        ddlCountry.SelectedIndex = 12;

    }
    


}

