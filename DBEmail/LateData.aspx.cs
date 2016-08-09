using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
public partial class DBEmail_LateData : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LateDataMethod();


            //Import Dynamasters Blast data

            if (Request.Url.Authority == "emd.thedatabase.net")
            {
                try
                {
                    EcotechBlastDataImport ecotechBlastDataImport = new EcotechBlastDataImport();
                    ecotechBlastDataImport.ImportBlastEvents();
                }
                catch (Exception ex)
                {
                    ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -EcotechBlastDataImport ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                    SystemData.ErrorLog_Insert(theErrorLog);
                }
            }



            try
            {
                if (Request.Url.Authority == "dev.thedatabase.net")
                {
                    System.Net.WebRequest webRequest = System.Net.WebRequest.Create("http://rrp.thedatabase.net/DBEmail/LateData.aspx");
                    System.Net.WebResponse webResp = webRequest.GetResponse();

                }

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "RRP - DBEmail -LateData ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

            try
            {
                if (Request.Url.Authority == "dev.thedatabase.net")
                {
                    System.Net.WebRequest webRequest = System.Net.WebRequest.Create("http://emd.thedatabase.net/DBEmail/LateData.aspx");
                    System.Net.WebResponse webResp = webRequest.GetResponse();

                }

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "RRP - DBEmail -LateData ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

        }
    }

    protected void LateDataMethod()
    {
        try
        {




            DataTable dtTables = Common.DataTableFromText(@"SELECT * FROM (SELECT      [Table].LateDataDays, Account.AccountID, [Table].TableName,[Table].TableID,
                        (SELECT MAX(DateAdded)FROM Record WHERE Record.IsActive=1 AND Record.TableID=[Table].TableID) AS LastRecordDate,Account.SMTPEmail
                        FROM Account INNER JOIN
                          [Table] ON Account.AccountID = [Table].AccountID
                          WHERE [Table].IsActive=1 AND Account.IsActive=1
                          AND [Table].LateDataDays is not null) AS T
                          WHERE LastRecordDate is not null
                          AND cast (DATEADD(DAY,LateDataDays, LastRecordDate) as date) <CAST (Getdate() as date) ORDER BY AccountID");



            //string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
            //string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
            //string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
            //string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
            string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail",null,null);
            //string strEnableSSL = SystemData.SystemOption_ValueByKey("EnableSSL");
            //string strSmtpPort = SystemData.SystemOption_ValueByKey("SmtpPort");


            string strAccountID = "-1";
            Content theContentEmail = null;
            Content theContentSMS = null;

            foreach (DataRow dr in dtTables.Rows)
            {


                //check if it is in scheduledtask


                //DataTable dtScheduledTask = Common.DataTableFromText("SELECT * FROM ScheduledTask WHERE ScheduleType='Late Data'" +
                //"AND AccountID=" + dr["AccountID"].ToString() + " AND TableID=" + dr["TableID"].ToString()
                //+ " AND RecordDateAdded=CONVERT(varchar(30), '" + dr["LastRecordDate"].ToString() + "', 120) ");
                int iTN = 0;
                DataTable dtScheduledTask = ScheduleManager.dbg_ScheduledTask_Select(int.Parse(dr["AccountID"].ToString()), int.Parse(dr["TableID"].ToString()), "", "", "Late Data",
                    null, "", "", null, null, ref iTN, DateTime.Parse(dr["LastRecordDate"].ToString()));
                ScheduledTask theScheduleTask = null;
                if (dtScheduledTask.Rows.Count > 0)
                {
                    //email was sent before

                    theScheduleTask = ScheduleManager.dbg_ScheduledTask_Detail(int.Parse(dtScheduledTask.Rows[0]["ScheduledTaskID"].ToString()));

                    //DateTime dtLastEmailSentDate = DateTime.Parse(DateTime.Parse(dr["LastRecordDate"].ToString()).ToShortDateString());
                    double dDayDifference = (DateTime.Today - (DateTime)theScheduleTask.LastEmailSentDate).TotalDays;
                    int iDayDiff = (int)dDayDifference;

                    if (iDayDiff <= 0)
                        iDayDiff = 1;

                    if (dtScheduledTask.Rows[0]["Frequency"].ToString().ToLower() == "t")
                    {
                        continue;//no need to send email for if
                    }

                    if (dtScheduledTask.Rows[0]["Frequency"].ToString().ToLower() == "w")
                    {
                        //week

                        if (iDayDiff % 7 == 0)
                        {

                        }
                        else
                        {
                            continue;//no need to send email for if
                        }

                    }

                    if (dtScheduledTask.Rows[0]["Frequency"].ToString().ToLower() == "m")
                    {
                        //week

                        if (iDayDiff % 30 == 0)
                        {

                        }
                        else
                        {
                            continue;//no need to send email for if
                        }

                    }


                }

                //lets send emails
                if (strAccountID != dr["AccountID"].ToString())
                {
                    strAccountID = dr["AccountID"].ToString();
                    theContentEmail = SystemData.Content_Details_ByKey("LateWarningEmail", int.Parse(dr["AccountID"].ToString()));
                    theContentSMS = SystemData.Content_Details_ByKey("LateWarningSMS", int.Parse(dr["AccountID"].ToString()));
                }


                Guid guidNew = Guid.NewGuid();
                string strEmailUID = guidNew.ToString();


                if (theContentEmail != null && theContentSMS != null)
                {
                    string strBody = theContentEmail.ContentP;

                    string strBodySMS = theContentSMS.ContentP;

                    strBody = strBody.Replace("[DateTime]", dr["LastRecordDate"].ToString());
                    strBody = strBody.Replace("[Table]", dr["TableName"].ToString());
                    strBody = strBody.Replace("[LateDataDays]", dr["LateDataDays"].ToString());
                    strBody = strBody.Replace("[week]", "http://" + Request.Url.Authority + Request.ApplicationPath +
                        "/LateDataSnooze.aspx?AccountID=" + Cryptography.Encrypt(dr["AccountID"].ToString())
                        + "&TableID=" + Cryptography.Encrypt(dr["TableID"].ToString()) + "&Period=week&emailuid="
                        + Cryptography.Encrypt(strEmailUID) + "&datetime=" + Cryptography.Encrypt(dr["LastRecordDate"].ToString()));
                    strBody = strBody.Replace("[month]", "http://" + Request.Url.Authority + Request.ApplicationPath +
                        "/LateDataSnooze.aspx?AccountID=" + Cryptography.Encrypt(dr["AccountID"].ToString())
                        + "&TableID=" + Cryptography.Encrypt(dr["TableID"].ToString()) + "&Period=month&emailuid="
                        + Cryptography.Encrypt(strEmailUID) + "&datetime=" + Cryptography.Encrypt(dr["LastRecordDate"].ToString()));
                    strBody = strBody.Replace("[turnoff]", "http://" + Request.Url.Authority + Request.ApplicationPath +
                        "/LateDataSnooze.aspx?AccountID=" + Cryptography.Encrypt(dr["AccountID"].ToString())
                        + "&TableID=" + Cryptography.Encrypt(dr["TableID"].ToString()) + "&Period=turnoff&emailuid="
                        + Cryptography.Encrypt(strEmailUID) + "&datetime=" + Cryptography.Encrypt(dr["LastRecordDate"].ToString()));


                    strBodySMS = strBodySMS.Replace("[DateTime]", dr["LastRecordDate"].ToString());
                    strBodySMS = strBodySMS.Replace("[Table]", dr["TableName"].ToString());
                    strBodySMS = strBodySMS.Replace("[LateDataDays]", dr["LateDataDays"].ToString());

                    strBodySMS = strBodySMS.Replace("[week]", "http://" + Request.Url.Authority + Request.ApplicationPath +
                       "/LateDataSnooze.aspx?AccountID=" + Cryptography.Encrypt(dr["AccountID"].ToString())
                       + "&TableID=" + Cryptography.Encrypt(dr["TableID"].ToString()) + "&Period=week&emailuid="
                       + Cryptography.Encrypt(strEmailUID) + "&datetime=" + Cryptography.Encrypt(dr["LastRecordDate"].ToString()));
                    strBodySMS = strBodySMS.Replace("[month]", "http://" + Request.Url.Authority + Request.ApplicationPath +
                      "/LateDataSnooze.aspx?AccountID=" + Cryptography.Encrypt(dr["AccountID"].ToString())
                      + "&TableID=" + Cryptography.Encrypt(dr["TableID"].ToString()) + "&Period=month&emailuid="
                      + Cryptography.Encrypt(strEmailUID) + "&datetime=" + Cryptography.Encrypt(dr["LastRecordDate"].ToString()));
                    strBodySMS = strBodySMS.Replace("[turnoff]", "http://" + Request.Url.Authority + Request.ApplicationPath +
                      "/LateDataSnooze.aspx?AccountID=" + Cryptography.Encrypt(dr["AccountID"].ToString())
                      + "&TableID=" + Cryptography.Encrypt(dr["TableID"].ToString()) + "&Period=turnoff&emailuid="
                      + Cryptography.Encrypt(strEmailUID) + "&datetime=" + Cryptography.Encrypt(dr["LastRecordDate"].ToString()));


                    //MailMessage msg = new MailMessage();
                    //msg.From = new MailAddress(strEmail);


                    string strSubject = theContentEmail.Heading.Replace("[Table]", dr["TableName"].ToString());

                    //msg.IsBodyHtml = true;

                    //msg.Body = strBody;
                    //SmtpClient smtpClient = new SmtpClient(strEmailServer);
                    //smtpClient.Timeout = 99999;
                    //smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

                    //smtpClient.EnableSsl = bool.Parse(strEnableSSL);
                    //smtpClient.Port = int.Parse(strSmtpPort);

                    DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
                     int.Parse(dr["TableID"].ToString()), null, true, null, null, null, null, null, null, null, null, null);

                    string strTempBody = strBody;

                    foreach (DataRow drU in dtUsersEmail.Rows)
                    {
                        //msg.To.Clear();
                        //msg.To.Add(drU["Email"].ToString());
                        string strTo = drU["Email"].ToString();
                        strTempBody = strBody;
                        strTempBody = strTempBody.Replace("[FullName]", drU["UserName"].ToString());
                        strTempBody = strTempBody.Replace("[FirstName]", drU["FirstName"].ToString());
                        //msg.Body = strTempBody;
                        try
                        {


#if (!DEBUG)
                            //smtpClient.Send(msg);
#endif
                            if (theScheduleTask != null)
                            {
                                theScheduleTask.LastEmailSentDate = DateTime.Now;
                                ScheduleManager.dbg_ScheduledTask_Update(theScheduleTask);
                            }


                            //if (msg.To.Count > 0)
                            //{
                                //EmailLog theEmailLog = new EmailLog(null, int.Parse(dr["AccountID"].ToString()), strSubject,
                                // strTo, DateTime.Now, int.Parse(dr["TableID"].ToString()), null, "Late Data", strTempBody);
                                //theEmailLog.EmailUID = strEmailUID;
                                //EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);

                                string sSendEmailError = "";

                                Message theMessage = new Message(null, null, null, null,
                                    DateTime.Now, "E", "E",
                                            null, strTo, strSubject, strTempBody, null, strEmailUID); 


                                DBGurus.SendEmail("Late Data", true, null, strSubject, strTempBody, "",
                                    strTo, "", "", null, theMessage, out sSendEmailError);
                            //}

                            //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                            //{

                            //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
                            //}

                        }
                        catch (Exception ex)
                        {

                            //strErrorMsg = "Server could not send warning Email & SMS";
                        }


                    }



                   


                    //msg = new MailMessage();
                    //msg.From = new MailAddress(strEmail);


                    strSubject = theContentSMS.Heading.Replace("[Table]", dr["TableName"].ToString());
                    //msg.IsBodyHtml = true;

                    //msg.Body = strBodySMS;


                    DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
                  int.Parse(dr["TableID"].ToString()), null, null, true, null, null, false, false, null, null, null, null);

                    foreach (DataRow drs in dtUsersSMS.Rows)
                    {
                        //msg.To.Clear();
                        if (drs["PhoneNumber"] != DBNull.Value)
                        {
                            if (drs["PhoneNumber"].ToString() != "")
                            {
                                //msg.To.Add(drs["PhoneNumber"].ToString() + strWarningSMSEMail);
                                string strTo = drs["PhoneNumber"].ToString() + strWarningSMSEMail;

                                strTempBody = strBodySMS;
                                strTempBody = strTempBody.Replace("[FullName]", drs["UserName"].ToString());
                                strTempBody = strTempBody.Replace("[FirstName]", drs["FirstName"].ToString());
                                //msg.Body = strTempBody;
                                try
                                {


#if (!DEBUG)
                                    //smtpClient.Send(msg);
#endif

                                    //if (msg.To.Count > 0)
                                    //{
                                        //EmailLog theEmailLog = new EmailLog(null, int.Parse(dr["AccountID"].ToString()), strSubject,
                                        //  strTo, DateTime.Now, int.Parse(dr["TableID"].ToString()), null, "Late Data SMS", strTempBody);
                                        //theEmailLog.EmailUID = strEmailUID;

                                    //Message theMessage=new Message(null,null,int.Parse(dr["TableID"].ToString()),int.Parse(dr["AccountID"].ToString()),)

                                        //EmailManager.dbg_EmailLog_Insert(theEmailLog, null, null);


                                        string sSendEmailError = "";

                                        Message theMessage = new Message(null, null, int.Parse(dr["TableID"].ToString()), int.Parse(dr["AccountID"].ToString()),
                   DateTime.Now, "W", "S",
                       null, strTo, strSubject, strTempBody, null, ""); 


                                        DBGurus.SendEmail("Late Data SMS", null, true, strSubject, strTempBody, "",
                                            strTo, "", "", null, theMessage, out sSendEmailError);

                                    //}

                                    //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                                    //{

                                    //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), null, true, null, null);
                                    //}

                                }
                                catch (Exception)
                                {

                                    //strErrorMsg = "Server could not send warning Email & SMS";
                                }
                            }
                        }
                    }




                }





            }




        }
        catch (Exception ex)
        {
            //
            ErrorLog theErrorLog = new ErrorLog(null, "Late Data", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);


        }

    }
   
}