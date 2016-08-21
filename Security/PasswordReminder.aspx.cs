using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Data.Linq;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;


public partial class Security_PasswordReminder : System.Web.UI.Page
{
    //LFDataContext srcLF = new LFDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Password Reminder";


        string strHelpJS = @" $(function () {
            $('#hlHelpCommon').fancybox({
                scrolling: 'auto',
                type: 'iframe',
                'transitionIn': 'elastic',
                'transitionOut': 'none',
                width: 600,
                height: 350,
                titleShow: false
            });
        });";


        ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);
    }



    protected void lnkSendPassword_Click(object sender, EventArgs e)
    {
        //Trap empty Email field
        int iTN = 0;
        if (txtEmail.Text.Length == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EmailEmptyErr", "alert('" + "Please enter your email address." + "');", true);
        }
        else
        {
            //Check if email entered is valid
            if (Regex.IsMatch(txtEmail.Text, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
            {
                //Search entered email address
                //List<User> listUser = SqlData_User.UserByEmail(txtEmail.Text);

               User theUser = SecurityManager.User_ByEmail( txtEmail.Text);


               if (theUser != null)
                {

                    if (EmailPassword(theUser))
                    {
                        lblEmailStatus.Text =
                            string.Format("A password reminder has been sent to {0}", this.txtEmail.Text);

                    }
                    else
                    {
                        lblEmailStatus.Text = "Password Reminder email unsuccessful.";
                    }
                    
                }
                else
                {
                    //Pop a message if email not found in database.
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "EmailNotFoundErr", "alert('" + "Email does not exist in database." + "');", true);
                }
            }
            else
            {
                //Pop a message if email is invalid.
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmailInvalidErr", "alert('" + "Email invalid." + "');", true);
            }

        }
    }

    public bool EmailPassword(User userRec)
    {
        bool bReturnValue = false;

        //string usr = userRec.UserName;
        string eml = userRec.Email;
        string pw = userRec.Password;
        string FullName = userRec.FirstName + " " + userRec.LastName;

        String url = Request.Url.OriginalString;
        string rawURL;
        rawURL = url.Substring(0, url.IndexOf("/Security"));

        try
        {
            string sError = "";
            string sFrom = SystemData.SystemOption_ValueByKey_Account_Default("EmailFrom",null,null, "no-reply@dbgurus.com.au");
            string sHeading = SystemData.SystemOption_ValueByKey_Account_Default("PasswordReminderEmailHeading",null,null,
                "ETS - Password Reminder");

            Content theContent = SystemData.Content_Details_ByKey("PasswordReminderEmail",null);
                       

            sHeading = theContent.Heading;
            theContent.ContentP = theContent.ContentP.Replace("[Full Name]", FullName);
            theContent.ContentP = theContent.ContentP.Replace("[Email]", eml);
            theContent.ContentP = theContent.ContentP.Replace("[Password]", pw);

            if (Session["LoginAccount"] == null)
            {
                theContent.ContentP = theContent.ContentP.Replace("[URL]", Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx");
            }
            else
            {
                theContent.ContentP = theContent.ContentP.Replace("[URL]", Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx?" + Session["LoginAccount"].ToString());
            }

            if (DBGurus.SendEmail(theContent.ContentKey, null, null, sHeading, theContent.ContentP, sFrom, eml, "", "", null,null, out sError) != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmailPasswordSendErr", "alert('" + sError + "');", true);
                //DBGurus.AddErrorLog(sError);
                ErrorLog theErrorLog = new ErrorLog(null, "Password Reminder", sError, sError, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);

            }
            else
            {
                bReturnValue = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "EmailPasswordErr", "alert('" + ex.Message + "');", true);
            //DBGurus.AddErrorLog(ex.Message);
            ErrorLog theErrorLog = new ErrorLog(null, "Password Reminder", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

        }
        return bReturnValue;
    }


    protected void lnkReturn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["AccountID"] == null)
        {
            Response.Redirect("~/Login.aspx", false);
        }
        else
        {
            Response.Redirect("~/Login.aspx?AccountID=" + Request.QueryString["AccountID"].ToString(), false);
        }
    }

   


}
