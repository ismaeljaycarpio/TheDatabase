using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Data.SqlClient;
public partial class Renewal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string strURL = "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
            string strDeveloperEmail = System.Configuration.ConfigurationManager.AppSettings["Coder"];
            bool bSentEmailToClient = true;

            if (strURL.IndexOf("test.") > -1 || strURL.IndexOf("prototype.") > -1)
            {
                bSentEmailToClient = false;
            }


            string strEmail = SystemData.SystemOption_ValueByKey_Account("EmailFrom",null,null);
            string strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
            string strEmailUserName = SystemData.SystemOption_ValueByKey_Account("EmailUserName", null, null);
            string strEmailPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);
            string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail", null, null);

            string strSmtpPort = SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null);
            string strEnableSSL = SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null);          

            List<User> lstGlobalUser = SecurityManager.User_ByRoleType("1");



            //1 week before renewal
            DataTable dtAcconts7 = Common.DataTableFromText(@"SELECT     Account.AccountID,[User].Email, [User].FirstName, [User].LastName, Account.AccountName,Account.ExpiryDate
                     FROM [User] INNER JOIN UserRole ON [User].UserID=UserRole.UserID    INNER JOIN
                  Account ON Account.AccountID = UserRole.AccountID
                      AND Account.IsActive=1 AND CAST(Account.ExpiryDate AS Date)=CAST( DATEADD(d,7, GETDATE()) AS Date)
                      AND [User].IsAccountHolder=1");


            Content theEmail = SystemData.Content_Details_ByKey("WeekBeforeRenewal", null);


            foreach (DataRow drEachAccount in dtAcconts7.Rows)
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(strEmail);

                msg.Subject = theEmail.Heading;
                msg.IsBodyHtml = true;

                msg.Body = theEmail.ContentP;// Sb.ToString();
              

                if (bSentEmailToClient)
                {
                    msg.To.Add(drEachAccount["Email"].ToString());
                }
                else
                {
                    msg.To.Add(strDeveloperEmail);
                }


                SmtpClient smtpClient = new SmtpClient(strEmailServer);
                smtpClient.Timeout = 99999;
                smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

                smtpClient.Port = DBGurus.StringToInt(strSmtpPort);
                smtpClient.EnableSsl = Convert.ToBoolean(strEnableSSL);

                //Send BCC to Global user   
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
                catch (Exception)
                {

                    //
                }
            }



            //Day of renewal
            DataTable dtAcconts1 = Common.DataTableFromText(@"SELECT     Account.AccountID,[User].Email, [User].FirstName, [User].LastName, Account.AccountName,Account.ExpiryDate
                        FROM [User] INNER JOIN UserRole ON [User].UserID=UserRole.UserID    INNER JOIN
                  Account ON Account.AccountID = UserRole.AccountID
                      AND Account.IsActive=1 AND CAST(Account.ExpiryDate AS Date)=CAST(  GETDATE() AS Date)
                      AND [User].IsAccountHolder=1");


            Content theEmail2 = SystemData.Content_Details_ByKey("DayOfRenewal ", null);


            foreach (DataRow drEachAccount in dtAcconts1.Rows)
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(strEmail);

                msg.Subject = theEmail2.Heading;
                msg.IsBodyHtml = true;

                msg.Body = theEmail2.ContentP;// Sb.ToString();

                if (bSentEmailToClient)
                {
                    msg.To.Add(drEachAccount["Email"].ToString());
                }
                else
                {
                    msg.To.Add(strDeveloperEmail);
                }


                SmtpClient smtpClient = new SmtpClient(strEmailServer);
                smtpClient.Timeout = 99999;
                smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

                smtpClient.Port = DBGurus.StringToInt(strSmtpPort);
                smtpClient.EnableSsl = Convert.ToBoolean(strEnableSSL);

                //Send BCC to Global user   
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
                catch (Exception)
                {

                    //
                }
            }







        }

    }
}