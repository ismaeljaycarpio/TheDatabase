using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class Subscribe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        string strEmailToAdmin = "info@EnvironmentTrackingSystem.com.au";//ets@EnvironmentTrackingSystem.com.au

        Content theContent = SystemData.Content_Details_ByKey("DemoSubscribeEmailContent", null);
        string strBody="";
        if (theContent != null)
        {
            strBody = theContent.ContentP;
            string strFirstName=txtName.Text;

            if (txtName.Text.IndexOf(" ") > -1)
            {
               strFirstName= txtName.Text.Substring(0, txtName.Text.IndexOf(" "));
            }

            strBody = strBody.Replace("[FirstName]", strFirstName);
            strBody = strBody.Replace("[Name]", txtName.Text);
            strBody = strBody.Replace("[Email]", txtEmail.Text);
            strBody = strBody.Replace("[Phone]", txtPhone.Text);

            string strEmail = SystemData.SystemOption_ValueByKey_Account("EmailFrom",null,null);
            string strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
            string strEmailUserName = SystemData.SystemOption_ValueByKey_Account("EmailUserName", null, null);
            string strEmailPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);
            string strEnableSSL = SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null);
            string strSmtpPort = SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null);


            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(strEmail);

            msg.Subject = theContent.Heading;

            msg.IsBodyHtml = true;

            //msg.Body = strBody;
            SmtpClient smtpClient = new SmtpClient(strEmailServer);
            smtpClient.Timeout = 99999;
            smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

            smtpClient.EnableSsl = bool.Parse(strEnableSSL);
            smtpClient.Port = int.Parse(strSmtpPort);

            msg.To.Clear();
            msg.To.Add(txtEmail.Text);
            msg.Bcc.Add(strEmailToAdmin);//
            msg.Body = strBody;
            try
            {


#if (!DEBUG)
                smtpClient.Send(msg);
#endif



            }
            catch (Exception ex)
            {

                ErrorLog theErrorLog = new ErrorLog(null, "Demo user email", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Saved", "parent.$.fancybox.close();", true);
            
        }

    }

}