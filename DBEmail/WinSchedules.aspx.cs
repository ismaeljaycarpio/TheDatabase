//using Pop3;
//using OpenPOP.POP3;
//using OpenPOP;
//using OpenPOP.MIMEParser;
//using Independentsoft;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using OpenPop.Common;
using OpenPop.Mime;
using OpenPop.Pop3;

using DocGen.DAL;
using System.IO;
using System.IO.Packaging;
using System.Globalization;

public partial class DBEmail_WinSchedules : System.Web.UI.Page
{
    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";
    string _strAuthority = "xdev.thedatabase.net";
    bool _bTestTime = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        _strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath",null,null);
        _strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation",null,null);

#if (DEBUG)
        _strAuthority="localhost:8081_no"; //MR Local
#endif

        if (!IsPostBack)
        {

            Page.Server.ScriptTimeout = 1600;


            //Dynamasters Blast data
            if (Request.Url.Authority == "emd.thedatabase.net" ||Request.QueryString["blast"]!=null)
            {
                try
                {
                    RegisterAsyncTask(new PageAsyncTask(EcotecImportBlastEvents_A));
                    if(Request.QueryString["blast"]!=null)
                    {
                        return;//only run this method
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -EcotecImportBlastEvents_A ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                    SystemData.ErrorLog_Insert(theErrorLog);
                }

            }



            try
            {
                if (Request.QueryString["AutoImportRecords"] != null)
                {
                    RegisterAsyncTask(new PageAsyncTask(AutoImportRecords_A));
                    return;//only run this method
                }
                else
                {
                    if (Request.Url.Authority != "emd.thedatabase.net")
                    {
                        RegisterAsyncTask(new PageAsyncTask(AutoImportRecords_A));
                    }                    
                }
               

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -AutoImportRecords ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }






            try
            {

                RegisterAsyncTask(new PageAsyncTask(DataReminderEmail_A));

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -DataReminderEmail ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

            try
            {

                RegisterAsyncTask(new PageAsyncTask(ReadIncomingEmails_A));

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -ReadIncomingEmails ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }


            try
            {

                RegisterAsyncTask(new PageAsyncTask(BatchAutoProcessImport_A));

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -BatchAutoProcessImport ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }


           

            try
            {
                if (Request.Url.Authority == _strAuthority)
                {
                    RegisterAsyncTask(new PageAsyncTask(emd_WinSchedules_A));                   
                }

            }
            catch(Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, " DBEmail - EMD_WinSchedules_A ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);               
            }

            try
            {
                if (Request.Url.Authority == _strAuthority)
                {

                    RegisterAsyncTask(new PageAsyncTask(rrp_WinSchedules_A));   
                }

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -rrp ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

            try
            {
                if (Request.Url.Authority == _strAuthority)
                {
                    RegisterAsyncTask(new PageAsyncTask(syduni_WinSchedules_A));   
                }

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -syduni_WinSchedules_A ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }


           

        }

    }





    ////Import Dynamasters Blast data

    //if (Request.Url.Authority == "emd.thedatabase.net")
    //{
    //    try
    //    {
    //        EcotechBlastDataImport ecotechBlastDataImport = new EcotechBlastDataImport();
    //        ecotechBlastDataImport.ImportBlastEvents();
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -EcotechBlastDataImport ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //    }
    //}
            



    protected void AutoImportRecords()
    {

        //SqlTransaction tn;
        //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
        Pop3Client popClient = new Pop3Client();
        try
        {

            //eventLog1.WriteEntry("Read Email from POP3 function has started.");

            string strPopEmailFrom = SystemData.SystemOption_ValueByKey_Account("PopEmailFrom",null,null);
            string strPopServer = SystemData.SystemOption_ValueByKey_Account("PopServer", null, null);
            string strPopUserName = SystemData.SystemOption_ValueByKey_Account("PopUserName", null, null);
            string strPopPassword = SystemData.SystemOption_ValueByKey_Account("PopPassword", null, null);
            string strPopPort = SystemData.SystemOption_ValueByKey_Account("PopPort", null, null);
            string strPopEnableSSL = SystemData.SystemOption_ValueByKey_Account("PopEnableSSL", null, null);


            //string strSamplesFolder = Server.MapPath("~/UserFiles/AppFiles");

            string strSamplesFolder = _strFilesPhisicalPath + "/UserFiles/AppFiles";


            //connect with pop3

          
            //OpenPOP.POP3.Utility.Log = true;
            if(popClient.Connected)
                popClient.Disconnect();

            popClient.Connect(strPopServer, int.Parse(strPopPort),bool.Parse(strPopEnableSSL));
            popClient.Authenticate(strPopUserName, strPopPassword, OpenPop.Pop3.AuthenticationMethod.UsernameAndPassword);

            //work with all messages

            int Count = popClient.GetMessageCount();

            //TheDBDataContext TheDB = new TheDBDataContext();
            List<InComingEmail> listInComingEmail = new List<InComingEmail>();
            //List<Email> listEmail = new List<Email>();

            // loop start from latest message
            //connection.Open();
            for (int i = Count; i >= 1; i -= 1)
            {

                //tn = connection.BeginTransaction();
                try
                {

                    //string strMessageUID = popClient.GetMessageUID(i);
                    OpenPop.Mime.Header.MessageHeader messageH = popClient.GetMessageHeaders(i);

                    if (messageH == null)
                    {
                        //tn.Rollback();
                        goto EndMsg;
                    }




                    InComingEmail theInComingEmail = EmailManager.ets_InComingEmail_Detail_By_MessageID(messageH.MessageId);

                    if (theInComingEmail != null)
                    {
                        //found, so we are done
                        //tn.Rollback();
                        goto L_1;
                    }

                    //a new single message, we need to play with it.

                    string strInComingAttachFileIDs = "";
                    string strBatchIDs = "";
                    List<Batch> listBatch = new List<Batch>();
                    Dictionary<int, List<int>> dictMappedBatch = new Dictionary<int, List<int>>();
                   OpenPop.Mime.Message message = popClient.GetMessage(i);


                    //message is a single pop3 message                   
                    string strDBRefEmailID = "none";
                    if (message != null)
                    {

                        // Check TheDatabase Ref

                        //get the user


                        //User theUser = SecurityManager.User_ByEmail(message.FromEmail, ref connection, ref tn);

                        int iUserID;
                        int iAccountID = -1;

                        int iTableID = -1;
                        bool bUseMapping = false;
                        string strFileName = "";

                        DataTable dtUploadEmail = Common.DataTableFromText(@"SELECT     [Table].AccountID, Upload.UploadID, Upload.TableID,  Upload.UploadName, Upload.EmailFrom, Upload.Filename
                                FROM         Upload INNER JOIN
                                   [Table] ON Upload.TableID = [Table].TableID WHERE EmailFrom LIKE '%" + messageH.From.Address + "%'");


                        if (dtUploadEmail.Rows.Count > 0)
                        {

                        }
                        else
                        {

                            //tn.Rollback();
                            goto EndMsg;
                        }


                        //check if there is any attachements

                        iUserID = int.Parse(SystemData.SystemOption_ValueByKey_Account("AutoUploadUserID",null,null));

                        string strFileUniqueName;
                                             

                        foreach (OpenPop.Mime.MessagePart item in message.FindAllAttachments())
                        {
                            if (item.FileName != "" && item.IsAttachment)
                            {

                                DataTable dtUpload = Common.DataTableFromText(@"SELECT     [Table].AccountID, Upload.UploadID,
                                Upload.TableID, Upload.UseMapping, Upload.UploadName, Upload.EmailFrom, Upload.Filename
                                FROM         Upload INNER JOIN
                                [Table] ON Upload.TableID = [Table].TableID 
                                WHERE EmailFrom LIKE '%" + messageH.From.Address + "%'");

                                bool bThisFileIsOK = false;

                                if (dtUpload.Rows.Count > 0)
                                {
                                    //
                                    foreach (DataRow dr in dtUpload.Rows)
                                    {
                                        strFileName = dr["Filename"].ToString();

                                        if (item.FileName.ToLower().IndexOf(strFileName.ToLower().Replace("%", "")) > -1)
                                        {
                                            iAccountID = int.Parse(dr["AccountID"].ToString());

                                            iTableID = int.Parse(dr["TableID"].ToString());
                                            bUseMapping = (bool)dr["UseMapping"];
                                            bThisFileIsOK = true;
                                            break;
                                        }
                                    }

                                }
                                else
                                {

                                    continue;
                                }

                                bool bRecordxceeded = false;
                               if(iAccountID!=-1)
                               {
                                   bRecordxceeded=SecurityManager.IsRecordsExceeded(iAccountID);

                                   //if bRecordxceeded true then may be we need to send an email as we can not display message here


                                   if(bRecordxceeded)
                                   {
                                       Content theContent = SystemData.Content_Details_ByKey("RecordsExceededAuto", null);

                                       if (theContent != null)
                                       {


                                           User theAccountHolder = SecurityManager.User_AccountHolder(iAccountID);
                                           if (theAccountHolder != null)
                                           {
                                               string sEmailError = "";

                                               //Guid guidNewE = Guid.NewGuid();
                                               //string strEmailUID = guidNewE.ToString();

                                               //EmailLog theEmailLog = new EmailLog(null, (int)iAccountID, theContent.Heading,
                                               //  theAccountHolder.Email, DateTime.Now, null,
                                               //  null,
                                               // theContent.ContentKey, theContent.ContentP);

                                               //theEmailLog.EmailUID = strEmailUID;


                                               Message theMessage = new Message(null, null, null, iAccountID,
                                                DateTime.Now, "E", "E",
                                                    null, theAccountHolder.Email, theContent.Heading, theContent.ContentP, null, ""); 


                                               DBGurus.SendEmail(theContent.ContentKey, true, null, theContent.Heading, theContent.ContentP, "",
                                              theAccountHolder.Email, "", "", null, theMessage, out sEmailError);
                                           }


                                       }

                                       //tn.Rollback();

                                       InComingEmail newInComingEmailE = new InComingEmail(null, messageH.Subject, messageH.From.Address,
                              messageH.To[0].ToString(), "", "", "",
                              DateTime.Now, messageH.MessageId, "RecordsExceededAuto", "RecordsExceededAuto", "RecordsExceededAuto", "", DateTime.Now,
                              null, strBatchIDs);
                                       EmailManager.ets_InComingEmail_Insert(newInComingEmailE);


                                       goto EndMsg;

                                   }
                                   
                               }


                               if (iTableID == -1 || iAccountID == -1 || bThisFileIsOK == false )
                                {
                                    continue;
                                }
                                Table theTable = RecordManager.ets_Table_Details(iTableID);


                                string strFileExtension = "";

                                switch (item.FileName.Substring(item.FileName.LastIndexOf('.') + 1).ToLower())
                                {
                                    case "dbf":
                                        strFileExtension = ".dbf";
                                        break;
                                    case "txt":
                                        strFileExtension = ".txt";
                                        break;
                                    case "csv":
                                        strFileExtension = ".csv";
                                        break;
                                    case "xls":
                                        strFileExtension = ".xls";
                                        break;
                                    case "xlsx":
                                        strFileExtension = ".xlsx";
                                        break;
                                    default:
                                        strFileExtension = "";
                                        break;
                                }


                                //Now insert a row in File table & then in get string of FileIDs for IncomingEmail.Attachements
                                Guid guidNew = Guid.NewGuid();
                                //strFileUniqueName = guidNew.ToString() + "." + item.ContentFileName.Substring(item.ContentFileName.LastIndexOf(".") + 1);
                                strFileUniqueName = guidNew.ToString() + strFileExtension;

                                dbgFile newFile = new dbgFile
                                (
                                  null, item.ContentDescription, item.FileName.Substring(item.FileName.LastIndexOf(".") + 1),
                                  item.FileName, strFileUniqueName,
                                  null, null, iAccountID, false, true);

                                //save it and get FileID

                                int iFileID = EmailManager.ets_File_Insert(newFile);

                                //prepare sting for Email.Attachments or InComingEmail.Attachments
                                strInComingAttachFileIDs = strInComingAttachFileIDs + iFileID.ToString() + ",";
                                //got a attachment here, so lets save it.
                                                               
                                //message.SaveAttachment(item, strSamplesFolder + "\\" + strFileUniqueName);

                                FileStream Stream = new FileStream(strSamplesFolder + "\\" + strFileUniqueName, FileMode.Create);
                                BinaryWriter BinaryStream = new BinaryWriter(Stream);
                                BinaryStream.Write(item.Body);
                                BinaryStream.Close();
                                
                                //here i need to create a batch!
                                
                                int iBatchID;
                                string strMsg;

                                //string strLocationIDs = "-1";

                                //if (iLocationID != null)
                                //    strLocationIDs = iLocationID.ToString();

                                UploadManager.UploadCSV(iUserID, theTable, messageH.Subject,
                                    item.FileName, guidNew, strSamplesFolder,
                                    out strMsg, out iBatchID, strFileExtension, "", iAccountID, null, null, null);

                                //eventLog1.WriteEntry(strMsg);

                                strBatchIDs = strBatchIDs + iBatchID.ToString() + ",";
                                Batch oBatch = UploadManager.ets_Batch_Details(iBatchID);
                                listBatch.Add(oBatch);

                                //string strImportMsg = UploadManager.ImportClickFucntions(oBatch, ref connection, ref tn);                                

                                if (bUseMapping)
                                {
                                    List<int> children = new List<int>();
                                    dictMappedBatch.Add(iBatchID, children);

                                    DataSet ds = new DataSet();
                                    string error = String.Empty;
                                    SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
                                    connection.Open();
                                    SqlCommand executeCommand = new SqlCommand("dbg_Import_Mapping", connection);
                                    executeCommand.CommandType = CommandType.StoredProcedure;
                                    executeCommand.CommandTimeout = 3600;
                                    executeCommand.Parameters.AddWithValue("@TableID", theTable.TableID);
                                    executeCommand.Parameters.AddWithValue("@BatchID", iBatchID);
                                    
                                    System.Data.SqlClient.SqlDataAdapter dataAdapter = new SqlDataAdapter(executeCommand);
                                    try
                                    {
                                        dataAdapter.Fill(ds);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorLog theErrorLog = new ErrorLog(null, "Auto Import records - mapping", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                                        SystemData.ErrorLog_Insert(theErrorLog);
                                    }
                                    connection.Close();
                                    connection.Dispose();

                                        
                                    if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                                    {
                                        int parentBatchID = iBatchID;
                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {
                                            int targetTableID = 0;
                                            if (int.TryParse(dr[0].ToString(), out targetTableID))
                                            {
                                                Table targetTable = RecordManager.ets_Table_Details(targetTableID);
                                                Byte[] parameters = new Byte[16];
                                                Byte[] batchIDPart = BitConverter.GetBytes((long)parentBatchID);
                                                Byte[] tableIDPart = BitConverter.GetBytes((long)theTable.TableID.Value);
                                                for (int j = 0; j < 8; j++)
                                                {
                                                    parameters[j] = batchIDPart[j];
                                                    parameters[j + 8] = tableIDPart[j];
                                                }


                                                //Table tempTableMapped = RecordManager.ets_Table_Details(targetTableID);

                                                //UploadManager.UploadCSV(iUserID, tempTableMapped, messageH.Subject,
                                                //    "", new Guid(parameters),
                                                //    "", out strMsg, out iBatchID, "als", "",
                                                //        tempTableMapped.AccountID, null, null, parentBatchID);



                                                UploadManager.UploadCSV(iUserID, targetTable, messageH.Subject,
                                                    null, new Guid(parameters), strSamplesFolder,
                                                    out strMsg, out iBatchID, "virtual", "", iAccountID, null, null, null);

                                                strBatchIDs = strBatchIDs + iBatchID.ToString() + ",";
                                                Batch oMapBatch = UploadManager.ets_Batch_Details(iBatchID);
                                                listBatch.Add(oMapBatch);
                                                dictMappedBatch[parentBatchID].Add(iBatchID);
                                            }
                                        }
                                    }
                                }
                            }

                        }//end attachment foreach

                        if (iTableID == -1 || iAccountID == -1)
                        {
                            //tn.Rollback();
                            //connection.Close();
                            //connection.Dispose();

                            goto EndMsg;
                        }

                        //tn.Commit();
                        //connection.Close();
                        //connection.Dispose();   

                        //Lets import and  send emails.

                        try
                        {
                            foreach (Batch item in listBatch)
                            {
                                if (item.BatchID.HasValue && !dictMappedBatch.ContainsKey(item.BatchID.Value))
                                {

                                    string strRollBackSQL = @"DELETE Record WHERE BatchID=" + item.BatchID.ToString() + "; Update Batch set IsImported=0 WHERE BatchID=" + item.BatchID.ToString() + ";";

                                    string strImportMsg = UploadManager.ImportClickFucntions(item);
                                    if (strImportMsg != "ok")
                                    {
                                        Common.ExecuteText(strRollBackSQL);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //eventLog1.WriteEntry("Sending email failed." + " - " + ex.Message + ex.StackTrace);

                        }


                        try
                        {
                            foreach (Batch item in listBatch)
                            {
                                //UploadManager.SamplesImportEmail(item);
                                string strImportedSamples = "0";
                                string strRoot = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath;


                                if (item.BatchID.HasValue && dictMappedBatch.ContainsKey(item.BatchID.Value))
                                    ALSCommon.ALSNotificationMessage(item, dictMappedBatch[item.BatchID.Value]);
                                else
                                    UploadManager.RecordsImportEmail(item, ref strImportedSamples, strRoot);                             

                                //UploadManager.RecordsImportEmail(item, ref strImportedSamples, strRoot);  
   
                                //SecurityManager.CheckSamplesExceeded((int)item.AccountID);
                            }
                        }
                        catch (Exception ex)
                        {
                            //eventLog1.WriteEntry("Sending email failed." + " - " + ex.Message + ex.StackTrace);

                        }
                        string strBodyMessage = "";
                        string strTextMessage = "";
                        OpenPop.Mime.MessagePart html = message.FindFirstHtmlVersion();
                        OpenPop.Mime.MessagePart plainText = message.FindFirstPlainTextVersion();

                        if (plainText != null)
                        {
                            strTextMessage = plainText.GetBodyAsText();
                        }

                        if (html != null)
                        {
                            if (html.Body != null)
                            {
                                strBodyMessage=System.Text.Encoding.UTF8.GetString(html.Body);
                                
                            }
                        }

                        InComingEmail newInComingEmail = new InComingEmail(null, messageH.Subject, messageH.From.Address,
                              messageH.To[0].ToString(), "", "", strInComingAttachFileIDs,
                              DateTime.Now, messageH.MessageId, strBodyMessage, strTextMessage, strBodyMessage, "", DateTime.Now,
                              null, strBatchIDs);


                        EmailManager.ets_InComingEmail_Insert(newInComingEmail);


                        //Message theMessage2 = new Message(null, null, iTableID, iAccountID, DateTime.Now, "E", "E",
                        //    true, messageH.From.Address, messageH.Subject, strTextMessage, null, "");
                                              
                        
                        //EmailManager.Message_Insert(theMessage2, null, null);

                    }
                    else
                    {
                        // message is empty, so do nothing  
                        //tn.Rollback();
                        //connection.Close();
                        //connection.Dispose();
                    }

                EndMsg:
                    //do nothing    
                    int iWhy = 0;
                    //eventLog1.WriteEntry("ReadUploadEmail OK");
                    //tn.Rollback();
                }
                catch (Exception ex)
                {
                    //tn.Rollback();
                    //connection.Close();
                    //connection.Dispose();
                    if (popClient.Connected)
                        popClient.Disconnect();

                    ErrorLog theErrorLog = new ErrorLog(null, "Auto Import records", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                    SystemData.ErrorLog_Insert(theErrorLog);
                }
            }

            //end of message reading
        L_1:

            //connection.Close();
            //connection.Dispose();
            if (popClient.Connected)
                popClient.Disconnect();
            //eventLog1.WriteEntry("ReadUploadEmail OK");

        }
        catch (Exception ex)
        {

            //tn.Rollback();
            //connection.Close();
            //connection.Dispose();
            if (popClient.Connected)
                popClient.Disconnect();
            ErrorLog theErrorLog = new ErrorLog(null, "Auto Import records", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

            //eventLog1.WriteEntry("ReadUploadEmail Error: " + ex.Message.ToString() + "--Detail-->" + ex.StackTrace);
        }
    }

    private string CreateUriFromFilename(string pFileName)
    {
        pFileName = pFileName.Replace(" ", "_");
        pFileName = "/" + System.IO.Path.GetFileName(pFileName);
        //pFileName = pFileName.Replace("./", "");
        return pFileName;
    }

    protected void BatchAutoProcessImport()
    {
        try
        {
            DataTable dtBatchList = Common.DataTableFromText("SELECT BatchID FROM Batch WHERE AutoProcess=1");

            foreach (DataRow dr in dtBatchList.Rows)
            {

                try
                {
                    Batch newSourceBatch = UploadManager.ets_Batch_Details(int.Parse(dr[0].ToString()));

                    string strOutMsg = "";
                    int iFinalBatchID = 0;

                    //Table theTable = RecordManager.ets_Table_Details((int)newSourceBatch.TableID);
                    UploadManager.UploadCSV(null, null, "", "", null, "", out strOutMsg, out iFinalBatchID, "", "",
                        newSourceBatch.AccountID, null, null, newSourceBatch.BatchID);
                    Batch theBatch = UploadManager.ets_Batch_Details(iFinalBatchID);

                    string strImportMsg = UploadManager.ImportClickFucntions(theBatch);

                    if (strImportMsg == "ok")
                    {
                        Common.ExecuteText("UPDATE Batch SET AutoProcess=0 WHERE BatchID=" + newSourceBatch.BatchID.ToString());
                        //send email
                        string strImportedSamples = "0";
                        string strRoot = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath;
                        UploadManager.RecordsImportEmail(theBatch, ref strImportedSamples, strRoot);
                    }

                }
                catch(Exception ex)
                {
                    ErrorLog theErrorLog = new ErrorLog(null, "batch Auto Process", ex.Message + "-- BatchID=" + dr[0].ToString(), ex.StackTrace, DateTime.Now, Request.Path);
                    SystemData.ErrorLog_Insert(theErrorLog);
                }
                
            }
        }
        catch
        {
            //unknown
        }
    }

    protected void ReadIncomingEmails()
    {
        try
        {

            DataTable dtInComingAccounts = Common.DataTableFromText(@"SELECT AccountID FROM Account WHERE POP3Email IS NOT NULL AND POP3UserName  IS NOT NULL AND POP3Password IS NOT NULL 
                                AND POP3Port IS NOT NULL  AND POP3Server IS NOT NULL  AND POP3SSL IS NOT NULL ");
            
            string strRecordID = "";

            foreach (DataRow dr in dtInComingAccounts.Rows)
            {
                try
                {
                    strRecordID = "";
                    Account theAccount = SecurityManager.Account_Details(int.Parse(dr["AccountID"].ToString()));
                    if (theAccount != null)
                    {
                        if (theAccount.POP3Email != "" && theAccount.POP3UserName != "" && theAccount.POP3Password != "" && theAccount.POP3Server != ""
                                            && theAccount.POP3Port != null && theAccount.POP3SSL != null)
                        {

                            string strPopEmailFrom = theAccount.POP3Email;
                            string strPopServer = theAccount.POP3Server;
                            string strPopUserName = theAccount.POP3UserName;
                            string strPopPassword = theAccount.POP3Password;
                            string strPopPort = theAccount.POP3Port.ToString();
                            string strPopEnableSSL = theAccount.POP3SSL.ToString();
                            if (strPopEnableSSL == "1")
                                strPopEnableSSL = "True";

                            if (strPopEnableSSL == "0")
                                strPopEnableSSL = "False";

                            Pop3Client popClient = new Pop3Client();

                            try
                            {


                                if (popClient.Connected)
                                    popClient.Disconnect();

                                popClient.Connect(strPopServer, int.Parse(strPopPort), bool.Parse(strPopEnableSSL));
                                popClient.Authenticate(strPopUserName, strPopPassword, OpenPop.Pop3.AuthenticationMethod.UsernameAndPassword);

                                int Count = popClient.GetMessageCount();

                                List<InComingEmail> listInComingEmail = new List<InComingEmail>();
                                int iReadMsgCount = 0;

                                for (int i = Count; i >= 1; i -= 1)
                                {
                                    strRecordID = "";
                                    OpenPop.Mime.Header.MessageHeader messageH = popClient.GetMessageHeaders(i);

                                    if (messageH == null)
                                    {
                                        goto EndMsg;
                                    }

                                    //InComingEmail theServerInComingEmail = new InComingEmail();

                                    //theServerInComingEmail = EmailManager.ets_InComingEmail_Detail_By_SERVER(strPopServer);

                                    //InComingEmail theInComingEmail = EmailManager.ets_InComingEmail_Detail_By_MessageID(messageH.MessageId);


                                    Message theInComingMessage = EmailManager.Message_Detail_BY_ExternalMessageKey(messageH.MessageId);

                                    iReadMsgCount = iReadMsgCount + 1;

                                    if (theInComingMessage != null || iReadMsgCount > 1000)
                                    {
                                        goto EndMailbox;
                                    }


                                    string strSubject = messageH.Subject.Trim();



                                    if (messageH.Subject != "" && messageH.Subject.Trim().Length > 2)
                                    {
                                        if (strSubject.IndexOf("#") > -1)
                                        {
                                            if (strSubject.LastIndexOf("#") > -1)
                                            {
                                                if (strSubject.IndexOf("#") != strSubject.LastIndexOf("#"))
                                                {
                                                    strRecordID = strSubject.Substring(strSubject.IndexOf("#") + 1, strSubject.LastIndexOf("#") - strSubject.IndexOf("#") - 1);

                                                    try
                                                    {
                                                        int iTest = int.Parse(strRecordID);
                                                    }
                                                    catch
                                                    {
                                                        strRecordID = "";
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    if (strRecordID == "")
                                    {
                                        goto EndMsg;
                                    }

                                    int iRecordID = int.Parse(strRecordID);
                                    Record theRecord = RecordManager.ets_Record_Detail_Full(iRecordID);


                                    if (theRecord == null)
                                    {
                                        goto EndMsg;
                                    }


                                    //start a message
                                    OpenPop.Mime.Message message = popClient.GetMessage(i);

                                    if (message != null)
                                    {
                                        
                                        try
                                        {

                                            if (theRecord != null)
                                            {

                                                OpenPop.Mime.MessagePart html = message.FindFirstHtmlVersion();

                                                string strMsgBody = "";
                                                if (html != null)
                                                {
                                                    if (html.Body != null)
                                                        strMsgBody = System.Text.Encoding.UTF8.GetString(html.Body);
                                                }


                                                Message theMessage = new Message(null, iRecordID, theRecord.TableID, theAccount.AccountID, messageH.DateSent, "E", "E",
                                                    true, messageH.From.Address, messageH.Subject, strMsgBody, null, messageH.MessageId);


                                                EmailManager.Message_Insert(theMessage);


                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                            ErrorLog theErrorLog = new ErrorLog(null, "Incoming Message" + theAccount == null ? "" : "-" + theAccount.AccountName + "--" + strRecordID,
                                                ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                                            SystemData.ErrorLog_Insert(theErrorLog);

                                            goto EndMsg;
                                        }
                                    }

                                EndMsg:
                                    //do nothing    
                                    int iWhy = 0;

                                }
                            }
                            catch (Exception ex)
                            {

                                ErrorLog theErrorLog = new ErrorLog(null, "Incoming Message" + theAccount == null ? "" : "-" + theAccount.AccountName + "--" + strRecordID,
                                                          ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                                SystemData.ErrorLog_Insert(theErrorLog);
                            }
                            finally
                            {
                                if (popClient.Connected)
                                    popClient.Disconnect();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    //
                    ErrorLog theErrorLog = new ErrorLog(null, "IncomingEmail attachments", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                    SystemData.ErrorLog_Insert(theErrorLog);
                }

            EndMailbox:
                int iWhy2 = 0;
            }



        }
        catch (Exception ex)
        {
            //
            ErrorLog theErrorLog = new ErrorLog(null, "IncomingEmail attachments", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
        }
    }


    //protected async System.Threading.Tasks.Task ALSLive_A()
    //{
    //    await System.Threading.Tasks.Task.Run(() => ALSLive());
    //}

    protected async System.Threading.Tasks.Task EcotecImportBlastEvents_A()
    {
        await System.Threading.Tasks.Task.Run(() => EcotecImportBlastEvents());
    }



    protected void EcotecImportBlastEvents()
    {
        try
        {
            EcotechBlastDataImport ecotechBlastDataImport = new EcotechBlastDataImport();
            ecotechBlastDataImport.ImportBlastEvents();
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -EcotecImportBlastEvents ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
        }
    }



    //protected void ALSLive()
    //{
    //    try
    //    {
    //        ALSLiveImport als = new ALSLiveImport();
    //        als.ImportALSLiveData();
    //        AutoImportRecords();
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -ALSLiveImport ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //    }
    //}


    protected async System.Threading.Tasks.Task ReadIncomingEmails_A()
    {
        await System.Threading.Tasks.Task.Run(() => ReadIncomingEmails());
    }
    protected async System.Threading.Tasks.Task BatchAutoProcessImport_A()
    {
        await System.Threading.Tasks.Task.Run(() => BatchAutoProcessImport());
    }

    protected async System.Threading.Tasks.Task DataReminderEmail_A()
    {
        await System.Threading.Tasks.Task.Run(() => DataReminderEmail());
    }

    protected async System.Threading.Tasks.Task AutoImportRecords_A()
    {
        await System.Threading.Tasks.Task.Run(() => AutoImportRecords());
    }

    protected async System.Threading.Tasks.Task syduni_WinSchedules_A()
    {
        await System.Threading.Tasks.Task.Run(() => syduni_WinSchedules());
    }

    protected async System.Threading.Tasks.Task rrp_WinSchedules_A()
    {
        await System.Threading.Tasks.Task.Run(() => rrp_WinSchedules());
    }
    protected async System.Threading.Tasks.Task emd_WinSchedules_A()
    {
        await System.Threading.Tasks.Task.Run(() => emd_WinSchedules()); 
    }

    protected void emd_WinSchedules()
    {
        System.Net.WebRequest webRequestEMD = System.Net.WebRequest.Create("http://emd.thedatabase.net/DBEmail/WinSchedules.aspx");
        System.Net.WebResponse webRespEMD = webRequestEMD.GetResponse();
    }
     protected void rrp_WinSchedules()
    {
        System.Net.WebRequest webRequestRRP = System.Net.WebRequest.Create("http://rrp.thedatabase.net/DBEmail/WinSchedules.aspx");
        System.Net.WebResponse webRespRRP = webRequestRRP.GetResponse();

    }
     protected void syduni_WinSchedules()
     {
         System.Net.WebRequest webRequestSydUni = System.Net.WebRequest.Create("http://syduni.thedatabase.net/DBEmail/WinSchedules.aspx");
         System.Net.WebResponse webRespSydUni = webRequestSydUni.GetResponse();
     }


     //protected async System.Threading.Tasks.Task all_WinSchedules_A(string strWinSchedules)
     //{
     //    await System.Threading.Tasks.Task.Run(() => all_WinSchedules(strWinSchedules));
     //}

     //protected void all_WinSchedules(string strWinSchedules)
     //{
     //    System.Net.WebRequest webRequestAll = System.Net.WebRequest.Create(strWinSchedules);
     //    System.Net.WebResponse webRespAll = webRequestAll.GetResponse();
     //}

    //protected void AttachIncomingEmails()
    //{
    //    try
    //    {
    //        if (SystemData.SystemOption_ValueByKey_Account("EmailAttachments",null,null).ToLower() == "yes")
    //        {

    //            DataTable dtIncoomingTables = Common.DataTableFromText("SELECT TableID FROM [Table] WHERE IsActive=1 AND JSONAttachmentPOP3 IS NOT NULL AND JSONAttachmentInfo IS NOT NULL");


    //            foreach (DataRow dr in dtIncoomingTables.Rows)
    //            {
    //                try
    //                {

    //                    Table theTable = RecordManager.ets_Table_Details(int.Parse(dr["TableID"].ToString()));
    //                    if (theTable != null)
    //                    {
    //                        if (theTable.JSONAttachmentInfo != "" && theTable.JSONAttachmentPOP3!="")
    //                        {
    //                            AttachmentSetting theAttachmentSetting = JSONField.GetTypedObject<AttachmentSetting>(theTable.JSONAttachmentInfo);

    //                            if (theAttachmentSetting.AttachIncomingEmails != null)
    //                            {
    //                                if ((bool)theAttachmentSetting.AttachIncomingEmails )
    //                                {
    //                                    AttachmentPOP3 theAttachmentPOP3 = JSONField.GetTypedObject<AttachmentPOP3>(theTable.JSONAttachmentPOP3);

    //                                    if (theAttachmentPOP3 != null)
    //                                    {
    //                                        if (theAttachmentPOP3.Email != "" && theAttachmentPOP3.Password != "" && theAttachmentPOP3.POP3Server != ""
    //                                            && theAttachmentPOP3.Port != null && theAttachmentPOP3.SSL != null)
    //                                        {

    //                                            string strPopEmailFrom = theAttachmentPOP3.Email;
    //                                            string strPopServer = theAttachmentPOP3.POP3Server;
    //                                            string strPopUserName = theAttachmentPOP3.Username;
    //                                            string strPopPassword = theAttachmentPOP3.Password;
    //                                            string strPopPort = theAttachmentPOP3.Port.ToString();
    //                                            string strPopEnableSSL = theAttachmentPOP3.SSL.ToString();

    //                                            //string strSamplesFolder = Server.MapPath("~/UserFiles/AppFiles");

    //                                            string strSamplesFolder = _strFilesPhisicalPath + "/UserFiles/AppFiles";

    //                                            //SqlTransaction tn;
    //                                            //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //                                            Pop3Client popClient = new Pop3Client();
    //                                            //connection.Open();
    //                                            //tn = connection.BeginTransaction();


    //                                            try
    //                                            {

    //                                                //OpenPOP.POP3.Utility.Log = true;
    //                                                if(popClient.Connected)
    //                                                    popClient.Disconnect();
    
    //                                                popClient.Connect(strPopServer, int.Parse(strPopPort),bool.Parse(strPopEnableSSL));
    //                                                popClient.Authenticate(strPopUserName, strPopPassword, OpenPop.Pop3.AuthenticationMethod.UsernameAndPassword);

    //                                                int Count = popClient.GetMessageCount();

    //                                                List<InComingEmail> listInComingEmail = new List<InComingEmail>();

                                                  
    //                                                for (int i = Count; i >= 1; i -= 1)
    //                                                {

    //                                                   OpenPop.Mime.Header.MessageHeader messageH = popClient.GetMessageHeaders(i);

    //                                                    if (messageH == null)
    //                                                    {
    //                                                        goto EndMsg;
    //                                                    }

    //                                                    InComingEmail theServerInComingEmail = new InComingEmail();

    //                                                    theServerInComingEmail = EmailManager.ets_InComingEmail_Detail_By_SERVER(strPopServer);

    //                                                    InComingEmail theInComingEmail = EmailManager.ets_InComingEmail_Detail_By_MessageID(messageH.MessageId);

    //                                                    if (theInComingEmail != null)
    //                                                    {
    //                                                        goto EndMailbox;
    //                                                    }


    //                                                    if (theServerInComingEmail != null)
    //                                                    {
    //                                                        if (theServerInComingEmail.MessageID.ToLower() ==
    //                                                            messageH.MessageId.ToLower())
    //                                                        {
    //                                                            goto EndMailbox;
    //                                                        }
    //                                                    }
    //                                                    else
    //                                                    {


    //                                                        ////not found any email
    //                                                        //InComingEmail newInComingEmail = new InComingEmail(null, "IncomingEmail test", "dbg",
    //                                                        //     "IncomingEmail test", "", "", "",
    //                                                        //     DateTime.Now, messageH.MessageId, messageH.Subject, "", "", "", DateTime.Now,
    //                                                        //     null, "");
    //                                                        //newInComingEmail.POPServer = strPopServer;
    //                                                        //EmailAndIncoming.ets_InComingEmail_Insert(newInComingEmail);

    //                                                        ////tn.Commit();
    //                                                        //goto EndMailbox;
                                                            

    //                                                    }

    //                                                    //start a message
    //                                                    OpenPop.Mime.Message message = popClient.GetMessage(i);

    //                                                    if (message != null)
    //                                                    {
    //                                                        if (messageH.Subject != "" && messageH.Subject.Trim().Length > 2)
    //                                                        {
    //                                                            string strSubject = messageH.Subject.Trim();

    //                                                            string strRecordID = "";

    //                                                            if (strSubject.IndexOf("#") > -1)
    //                                                            {
    //                                                                if (strSubject.LastIndexOf("#") > -1)
    //                                                                {
    //                                                                    if (strSubject.IndexOf("#") != strSubject.LastIndexOf("#"))
    //                                                                    {
    //                                                                        strRecordID = strSubject.Substring(strSubject.IndexOf("#") + 1, strSubject.LastIndexOf("#") - strSubject.IndexOf("#") - 1);

    //                                                                        try
    //                                                                        {
    //                                                                            int iTest = int.Parse(strRecordID);
    //                                                                        }
    //                                                                        catch
    //                                                                        {
    //                                                                            strRecordID = "";
    //                                                                        }
    //                                                                    }
    //                                                                }
    //                                                            }

    //                                                            //if (messageH.Subject.Trim().Substring(0, 1) == "#" &&
    //                                                            //    messageH.Subject.Trim().Substring(messageH.Subject.Trim().Length - 1,1)=="#") 
    //                                                            if(strRecordID!="")
    //                                                            {
    //                                                                //string strRecordID = messageH.Subject.Trim().Replace("#", "");
    //                                                                try
    //                                                                {
    //                                                                    int iRecordID = int.Parse(strRecordID);

    //                                                                    if (theAttachmentSetting.InIdentifierColumnID != null)
    //                                                                    {
    //                                                                        Column theIdentifierColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.InIdentifierColumnID);

    //                                                                        if (theIdentifierColumn != null)
    //                                                                        {

    //                                                                            if (theIdentifierColumn.SystemName.ToLower() != "recordid")
    //                                                                            {
    //                                                                                string strDBRecordID = Common.GetValueFromSQL(" SELECT RecordID FROM [Record] WHERE IsActive=1 AND TableID="+theIdentifierColumn.TableID.ToString()
    //                                                                                    +" AND "+theIdentifierColumn.SystemName+"='"+strRecordID+"'");
    //                                                                                if (strDBRecordID != "")
    //                                                                                {
    //                                                                                    iRecordID = int.Parse(strDBRecordID);
    //                                                                                }
    //                                                                            }
    //                                                                        }

    //                                                                    }
    //                                                                    Record theRecord = RecordManager.ets_Record_Detail_Full(iRecordID);
    //                                                                    if (theRecord != null)
    //                                                                    {
    //                                                                        if (theRecord.TableID == theTable.TableID)
    //                                                                        {

    //                                                                            if (theAttachmentSetting.InSavetoTableID != null)
    //                                                                            {
    //                                                                                Record theChildRecord = new Record();
    //                                                                                theChildRecord.TableID = (int)theAttachmentSetting.InSavetoTableID;
    //                                                                                theChildRecord.EnteredBy = theRecord.EnteredBy;

    //                                                                                DataTable dtChildColumn = Common.DataTableFromText("SELECT * FROM [Column] WHERE TableID=" + theChildRecord.TableID.ToString() + " AND TableTableID=" + theTable.TableID.ToString());

    //                                                                                if (dtChildColumn.Rows.Count > 0)
    //                                                                                {
    //                                                                                    Column lnkColumn = RecordManager.ets_Column_Details(int.Parse(dtChildColumn.Rows[0]["LinkedParentColumnID"].ToString()));
    //                                                                                    RecordManager.MakeTheRecord(ref theChildRecord, dtChildColumn.Rows[0]["SystemName"].ToString(), RecordManager.GetRecordValue(ref theRecord, lnkColumn.SystemName));

    //                                                                                }

    //                                                                                if (theAttachmentSetting.InSaveSubjectColumnID != null)
    //                                                                                {
    //                                                                                    Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.InSaveSubjectColumnID);

    //                                                                                    RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, messageH.Subject);
    //                                                                                }

    //                                                                                if (theAttachmentSetting.InSaveSenderColumnID != null)
    //                                                                                {
    //                                                                                    Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.InSaveSenderColumnID);

    //                                                                                    RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, messageH.From.Address);
    //                                                                                }


    //                                                                                if (theAttachmentSetting.InSaveEmailColumnID != null)
    //                                                                                {
    //                                                                                    Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.InSaveEmailColumnID);


    //                                                                                    //make .msg file
    //                                                                                    //OpenPOP.MIMEParser.Message
    //                                                                                    //MemoryStream stream = new MemoryStream();
    //                                                                                    //message.Save(stream);

    //                                                                                    //byte[] data = stream.ToArray();
    //                                                                                    //stream.Close();
    //                                                                                    //stream.Dispose();
    //                                                                                    //stream = null;
    //                                                                                    //string strFolder = "../Pages/Record/RecordFile_dev/";
    //                                                                                    //string strFileName = "Message.msg";
    //                                                                                    //string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;// +"." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
    //                                                                                    //string strPath = Server.MapPath(strFolder + "\\" + strUniqueName);

    //                                                                                    //Independentsoft.Email.Mime.Message mine = new Independentsoft.Email.Mime.Message(data);
    //                                                                                    //Independentsoft.Msg.Message msg = new Independentsoft.Msg.Message(mine);
    //                                                                                    //msg.Save(strPath, true);

    //                                                                                    theServerInComingEmail.MessageID = messageH.MessageId;
    //                                                                                    //string strMessage = message.RawMessage.ToString();
    //                                                                                    OpenPop.Mime.MessagePart html = message.FindFirstHtmlVersion();
    //                                                                                    if (html != null)
    //                                                                                    {
    //                                                                                        if(html.Body!=null)
    //                                                                                            RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, System.Text.Encoding.UTF8.GetString(html.Body));
    //                                                                                    }
    //                                                                                }
    //                                                                                if (theAttachmentSetting.InSaveAttachmentColumnID != null)
    //                                                                                {

    //                                                                                   List<OpenPop.Mime.MessagePart> Items= message.FindAllAttachments();

                                                                                     
    //                                                                                   int iAttachCount = 0;
    //                                                                                   foreach (OpenPop.Mime.MessagePart item in Items)
    //                                                                                   {
    //                                                                                       if (item.FileName != "" && item.IsAttachment)
    //                                                                                       {
    //                                                                                           iAttachCount = iAttachCount + 1;
    //                                                                                       }
    //                                                                                   }

    //                                                                                   if (iAttachCount > 1)
    //                                                                                   {
    //                                                                                       List<string> lstFiles = new List<string>();

    //                                                                                       foreach (OpenPop.Mime.MessagePart item in Items)
    //                                                                                       {
    //                                                                                           if (item.FileName != "" && item.IsAttachment)
    //                                                                                           {
    //                                                                                               string strFileUniqueName = Guid.NewGuid().ToString() + "_" + item.FileName;
    //                                                                                               string strPath = strSamplesFolder + "\\" + strFileUniqueName;
    //                                                                                               FileStream Stream = new FileStream(strPath, FileMode.Create);

    //                                                                                               BinaryWriter BinaryStream = new BinaryWriter(Stream);
    //                                                                                               BinaryStream.Write(item.Body);
    //                                                                                               BinaryStream.Close();

    //                                                                                               lstFiles.Add(strPath);
    //                                                                                           }
    //                                                                                       }

    //                                                                                       string strOnlyZipName=Guid.NewGuid().ToString() + "_AttachmentFiles.zip";
    //                                                                                       string strZipFilename = strSamplesFolder + "\\" + strOnlyZipName;
    //                                                                                      //System.IO.Packaging 
    //                                                                                       Package pZip = ZipPackage.Open(strZipFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);

    //                                                                                       foreach (string aFilePath in lstFiles)
    //                                                                                       {
    //                                                                                           if(File.Exists(aFilePath))
    //                                                                                           {
    //                                                                                               Uri partUri = new Uri(CreateUriFromFilename(aFilePath), UriKind.Relative);
    //                                                                                               PackagePart pkgPart = pZip.CreatePart(partUri, System.Net.Mime.MediaTypeNames.Application.Zip, CompressionOption.Normal);
    //                                                                                               byte[] arrBuffer = File.ReadAllBytes(aFilePath);
    //                                                                                               pkgPart.GetStream().Write(arrBuffer, 0, arrBuffer.Length);

    //                                                                                           }

    //                                                                                       }
    //                                                                                       pZip.Close();
    //                                                                                       pZip = null;

    //                                                                                       Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.InSaveAttachmentColumnID);

    //                                                                                       RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strOnlyZipName);
    //                                                                                   }
    //                                                                                   else
    //                                                                                   {

    //                                                                                       foreach (OpenPop.Mime.MessagePart item in Items)
    //                                                                                       {
    //                                                                                           if (item.FileName != "" && item.IsAttachment)
    //                                                                                           {
    //                                                                                               string strFileUniqueName = Guid.NewGuid().ToString() + "_" + item.FileName;
    //                                                                                               string strPath = strSamplesFolder + "\\" + strFileUniqueName;
    //                                                                                               FileStream Stream = new FileStream(strPath, FileMode.Create);

    //                                                                                               BinaryWriter BinaryStream = new BinaryWriter(Stream);
    //                                                                                               BinaryStream.Write(item.Body);
    //                                                                                               BinaryStream.Close();

    //                                                                                               Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.InSaveAttachmentColumnID);

    //                                                                                               RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strFileUniqueName);
    //                                                                                               break;
    //                                                                                           }

    //                                                                                       }
    //                                                                                   }

    //                                                                                }


    //                                                                                try
    //                                                                                {
    //                                                                                    RecordManager.ets_Record_Insert(theChildRecord);
    //                                                                                    EmailManager.ets_InComingEmail_Update(theServerInComingEmail, null);

    //                                                                                    InComingEmail newInComingEmail = new InComingEmail(null, "IncomingEmail test", "dbg",
    //                                                                                       "IncomingEmail test", "", "", "",
    //                                                                                       DateTime.Now, messageH.MessageId, messageH.Subject, "", "", "", DateTime.Now,
    //                                                                                       null, "");
    //                                                                                    newInComingEmail.POPServer = strPopServer;
    //                                                                                    EmailManager.ets_InComingEmail_Insert(newInComingEmail);

    //                                                                                }
    //                                                                                catch (Exception ex)
    //                                                                                {
    //                                                                                    //
    //                                                                                    ErrorLog theErrorLog = new ErrorLog(null, "IncomingEmail attachments", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //                                                                                    SystemData.ErrorLog_Insert(theErrorLog);

    //                                                                                }

    //                                                                            }
    //                                                                        }

    //                                                                    }


    //                                                                }
    //                                                                catch(Exception ex)
    //                                                                {

    //                                                                    ErrorLog theErrorLog = new ErrorLog(null, "IncomingEmail attachments", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //                                                                    SystemData.ErrorLog_Insert(theErrorLog);

    //                                                                    goto EndMsg;
    //                                                                }


    //                                                            }

    //                                                        }

    //                                                    }

    //                                                EndMsg:
    //                                                    //do nothing    
    //                                                    int iWhy = 0;

    //                                                }
    //                                                //tn.Commit();
    //                                            }
    //                                            catch(Exception ex)
    //                                            {

    //                                                ErrorLog theErrorLog = new ErrorLog(null, "IncomingEmail attachments", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //                                                SystemData.ErrorLog_Insert(theErrorLog);
    //                                                //tn.Rollback();
    //                                                //
    //                                            }
    //                                            finally
    //                                            {
    //                                                //connection.Close();
    //                                                //connection.Dispose();
    //                                                if (popClient.Connected)
    //                                                    popClient.Disconnect();
    //                                            }
    //                                        }
    //                                    }
    //                                }                               

    //                            }                                
    //                        }
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    //
    //                    ErrorLog theErrorLog = new ErrorLog(null, "IncomingEmail attachments", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //                    SystemData.ErrorLog_Insert(theErrorLog);
    //                }

    //            EndMailbox:
    //                int iWhy2 = 0;
    //            }

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        //
    //        ErrorLog theErrorLog = new ErrorLog(null, "IncomingEmail attachments", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //    }
    //}

    protected void DataReminderEmail()
    {

        if (_bTestTime)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Not Error-Testing", "DataReminderEmail", "Start", DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
        }

        string strUpdatedRecordIDs = "-1";
        try
        {
            //int iUserID = int.Parse(SystemData.SystemOption_ValueByKey("AutoUploadUserID"));
            DataTable dtDRUsers = Common.DataTableFromText(@"SELECT     DataReminder.*, DataReminderUser.*,  [Table].TableName,[Table].AccountID,
                 [Column].TableID, [User].FirstName, [User].LastName, [User].Email, 
                 [Column].SystemName, [Column].DisplayName, [Column].ColumnType
                FROM         DataReminder INNER JOIN
                  DataReminderUser ON DataReminder.DataReminderID = DataReminderUser.DataReminderID 
                  INNER JOIN
                  [Column] ON DataReminder.ColumnID = [Column].ColumnID INNER JOIN
                  [Table] ON [Column].TableID = [Table].TableID   
	               LEFT JOIN
                  [User] ON DataReminderUser.UserID = [User].UserID
                  INNER JOIN Account ON Account.AccountID=[Table].AccountID
                  WHERE [Table].IsActive=1 AND Account.IsActive=1 
                  AND ([Column].ColumnType='datetime' OR [Column].ColumnType='date')");

                        
            string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail",null,null);
          

            foreach (DataRow dr in dtDRUsers.Rows)
            {
                //lets send emails ReminderContent
                DataTable _dtRecordColums = RecordManager.ets_Table_Columns_Summary(int.Parse(dr["TableID"].ToString()), null);


                Table theTable = RecordManager.ets_Table_Details(int.Parse(dr["TableID"].ToString()));


                string strBody = dr["ReminderContent"].ToString();
                Column theColumn = RecordManager.ets_Column_Details(int.Parse(dr["ColumnID"].ToString()));

                //lets find records

                DataTable dtRecords = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=1 AND TableID=" + dr["TableID"].ToString()
                    + " AND " + theColumn.SystemName + " IS NOT NULL AND ReminderSentDate IS NULL AND ISDATE(" + theColumn.SystemName + ")=1 "
                    + "  AND " + " CONVERT(Datetime," + theColumn.SystemName + ",103) >= CONVERT(Datetime,'" + DateTime.Today.Date.ToShortDateString() + "',103)");


                if (dtRecords.Rows.Count > 0)
                {

                    foreach (DataRow drR in dtRecords.Rows)
                    {
                         Record thisRecord = RecordManager.ets_Record_Detail_Full(int.Parse(drR["RecordID"].ToString()));
                        
                        DateTime dateReminder;
                        if (!DateTime.TryParseExact(RecordManager.GetRecordValue(ref thisRecord,theColumn.SystemName)  , Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dateReminder))
                        {

                            if (!DateTime.TryParseExact(RecordManager.GetRecordValue(ref thisRecord,theColumn.SystemName) , Common.DateTimeformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dateReminder))
                            {
                                //not a date or valid date format, so continue to next record
                                continue;
                            }

                        }

                        dateReminder = dateReminder.AddDays(-int.Parse(dr["NumberOfDays"].ToString()));
                       
                        //if (dr["ReminderColumnID"] != DBNull.Value)
                        //{
                        //    int iTest = 0;
                        //}

                        if (dateReminder.ToShortDateString() == DateTime.Today.Date.ToShortDateString())
                        {
                            string strEachRecordBody = strBody;
                            DataTable dtColumns = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE IsStandard=0 AND   TableID="
                                + dr["TableID"].ToString() + "  ORDER BY DisplayName");
                            foreach (DataRow drC in dtColumns.Rows)
                            {
                                strEachRecordBody = strEachRecordBody.Replace("[" + drC["DisplayName"].ToString() + "]", RecordManager.GetRecordValue(ref thisRecord,drC["SystemName"].ToString()));

                            }



                            //Work with 1 top level Parent tables.
                          
                            DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + dr["TableID"].ToString()); //AND DetailPageType<>'not'

                            if (dtPT.Rows.Count > 0)
                            {

                                foreach (DataRow drPT in dtPT.Rows)
                                {

                                    for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                                    {
                                        if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                   && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                   || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                                    && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                   && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                                        {
                                            if (_dtRecordColums.Rows[i]["TableTableID"].ToString() == drPT["ParentTableID"].ToString())
                                            {
                                                if (RecordManager.GetRecordValue(ref thisRecord, _dtRecordColums.Rows[i]["SystemName"].ToString()) != "")
                                                {

                                                    //DataTable dtParentRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + drR[_dtRecordColums.Rows[i]["SystemName"].ToString()].ToString());


                                                    Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));
                                                    DataTable dtParentRecord = null;
                                                    if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                                    {
                                                        dtParentRecord = Common.DataTableFromText("SELECT RecordID FROM Record WHERE RecordID=" + RecordManager.GetRecordValue(ref thisRecord, _dtRecordColums.Rows[i]["SystemName"].ToString()));
                                                    }
                                                    else
                                                    {
                                                        dtParentRecord = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "='" +
                                                            RecordManager.GetRecordValue(ref thisRecord, _dtRecordColums.Rows[i]["SystemName"].ToString()).ToString().Replace("'", "''") + "'");
                                                    }

                                                    if (dtParentRecord.Rows.Count > 0)
                                                    {
                                                        Record theParentRecord=RecordManager.ets_Record_Detail_Full(int.Parse(dtParentRecord.Rows[0]["RecordID"].ToString()));
                                                        DataTable dtColumnsPT = Common.DataTableFromText(@"SELECT distinct SystemName, TableName + ':' + DisplayName AS DP FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE  [Column].IsStandard=0 AND  [Column].TableID=" + drPT["ParentTableID"].ToString());
                                                        foreach (DataRow drC in dtColumnsPT.Rows)
                                                        {
                                                            strEachRecordBody = strEachRecordBody.Replace("[" + drC["DP"].ToString() + "]",  RecordManager.GetRecordValue(ref theParentRecord,drC["SystemName"].ToString()));

                                                        }
                                                    }



                                                }



                                            }

                                        }

                                    }




                                }

                               

                            }

                            //
                                                        //lets send an email
                         
                            string strSubject = dr["ReminderHeader"].ToString();
                                                       
                            string strTo = "";
                            if (dr["Email"] != DBNull.Value)
                            {
                                //msg.To.Add(dr["Email"].ToString());
                                strTo=dr["Email"].ToString();
                                strEachRecordBody = strEachRecordBody.Replace("[LastName]", dr["LastName"].ToString());
                                strEachRecordBody = strEachRecordBody.Replace("[FirstName]", dr["FirstName"].ToString());
                            }
                            else
                            {
                                if (dr["ReminderColumnID"] != DBNull.Value)
                                {
                                    Column theReminderColumn = RecordManager.ets_Column_Details(int.Parse(dr["ReminderColumnID"].ToString()));
                                    if (theReminderColumn != null)
                                    {
                                        string strEmailRC = Common.GetValueFromSQL("  SELECT " + theReminderColumn.SystemName + " FROM Record WHERE RecordID=" + drR["RecordID"].ToString());
                                        //msg.To.Add(strEmailRC);
                                        strTo=strEmailRC;
                                    }
                                }
                            }
                            //msg.Body = strEachRecordBody;

                            try
                            {


                                if (strTo!="")
                                {
                                   
                                 string sSendEmailError = "";

                                 Message theMessage = new Message(null, null, int.Parse(dr["TableID"].ToString()), int.Parse(dr["AccountID"].ToString()),
                DateTime.Now, "W", "E",
                    null, strTo, strSubject, strEachRecordBody, null, ""); 


                                DBGurus.SendEmail("Reminder", true, null, strSubject, strEachRecordBody, "",
                                    strTo, "", "", null, theMessage, out sSendEmailError);

                                strUpdatedRecordIDs = strUpdatedRecordIDs + "," + drR["RecordID"].ToString();
                                //Common.ExecuteText("UPDATE Record SET ReminderSentDate=GETDATE() WHERE RecordID=" + drR["RecordID"].ToString());

                                //if (SystemData.SystemOption_ValueByKey_Account("EmailAttachments",null,null) == "Yes")
                                //    {
                                //        try
                                //        {
                                //            if (theTable.JSONAttachmentInfo != "")
                                //            {
                                //                AttachmentSetting theAttachmentSetting = JSONField.GetTypedObject<AttachmentSetting>(theTable.JSONAttachmentInfo);
                                //                if (theAttachmentSetting.AttachOutgoingEmails != null)
                                //                {
                                //                    if ((bool)theAttachmentSetting.AttachOutgoingEmails)
                                //                    {
                                //                        if (theAttachmentSetting.OutSavetoTableID != null)
                                //                        {
                                //                            Record theChildRecord = new Record();
                                //                            theChildRecord.TableID = (int)theAttachmentSetting.OutSavetoTableID;
                                //                            theChildRecord.EnteredBy = int.Parse(dr["UserID"].ToString());

                                //                            //link the record with the parent table

                                //                            DataTable dtChildColumn = Common.DataTableFromText("SELECT * FROM [Column] WHERE TableID=" + theChildRecord.TableID.ToString() + " AND TableTableID=" + theTable.TableID.ToString());

                                //                            if (dtChildColumn.Rows.Count > 0)
                                //                            {
                                //                                Column lnkColumn = RecordManager.ets_Column_Details(int.Parse(dtChildColumn.Rows[0]["LinkedParentColumnID"].ToString()));
                                //                                RecordManager.MakeTheRecord(ref theChildRecord, dtChildColumn.Rows[0]["SystemName"].ToString(), RecordManager.GetRecordValue(ref thisRecord, lnkColumn.SystemName));

                                //                            }

                                //                            if (theAttachmentSetting.OutSaveRecipientColumnID != null)
                                //                            {
                                //                                Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveRecipientColumnID);

                                //                                RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strTo);
                                //                            }

                                //                            if (theAttachmentSetting.OutSaveSubjectColumnID != null)
                                //                            {
                                //                                Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveSubjectColumnID);

                                //                                RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strSubject);
                                //                            }

                                //                            if (theAttachmentSetting.OutSaveBodyColumnID != null)
                                //                            {
                                //                                Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveBodyColumnID);

                                //                                RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strEachRecordBody);
                                //                            }


                                //                            try
                                //                            {
                                //                                RecordManager.ets_Record_Insert(theChildRecord);
                                //                            }
                                //                            catch
                                //                            {
                                //                                //

                                //                            }

                                //                        }

                                //                    }

                                //                }

                                //            }
                                //        }
                                //        catch
                                //        {
                                //            //
                                //        }


                                //    }


                                }


                            }
                            catch (Exception ex)
                            {
                                ErrorLog theErrorLog = new ErrorLog(null, "Send Reminder Emails Error", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                                SystemData.ErrorLog_Insert(theErrorLog);
                                //strErrorMsg = "Server could not send warning Email & SMS";
                            }
                        }


                    }

                }





                //msg.Body = strBody;





            }

            if(strUpdatedRecordIDs!="-1")
            {
                Common.ExecuteText("UPDATE Record SET ReminderSentDate=GETDATE() WHERE RecordID IN ( " + strUpdatedRecordIDs + ")");
            }

        }
        catch (Exception ex)
        {

            if (strUpdatedRecordIDs != "-1")
            {
                Common.ExecuteText("UPDATE Record SET ReminderSentDate=GETDATE() WHERE RecordID IN ( " + strUpdatedRecordIDs + ")");
            }

            ErrorLog theErrorLog = new ErrorLog(null, "Reminder Error", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            //eventLog1.WriteEntry("Send Alerts Emails Error: " + ex.Message.ToString() + "--Detail-->" + ex.StackTrace);
        }


        if (_bTestTime)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Not Error-Testing", "DataReminderEmail-" + strUpdatedRecordIDs, "End", DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
        }


    }

//    protected void SendSydnUniShowWhenIssue()
//    {
//        try
//        {
//            string strRC=Common.GetValueFromSQL("SELECT Count(*) FROM ShowWhen");
//            if (strRC == "")
//                strRC = "0";

//            if (int.Parse(strRC) < 50)
//            {
//                string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
//                string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
//                string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
//                string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
//                string strWarningSMSEMail = SystemData.SystemOption_ValueByKey("WarningSMSEmail");
//                string strEnableSSL = SystemData.SystemOption_ValueByKey("EnableSSL");
//                string strSmtpPort = SystemData.SystemOption_ValueByKey("SmtpPort");

//                SmtpClient smtpClient = new SmtpClient(strEmailServer);
//                smtpClient.Timeout = 99999;
//                smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

//                smtpClient.EnableSsl = bool.Parse(strEnableSSL);
//                smtpClient.Port = int.Parse(strSmtpPort);

//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress(strEmail);


//                msg.Subject = "ShowWhen DELETED -- " + Request.Url.Authority;

//                msg.IsBodyHtml = false;

//                msg.Body = "ShowWhen records has been deleted, please check " + Request.Url.Authority + " DB " + ", It has only " + strRC + " record(s) ";// Sb.ToString();

//                msg.To.Clear();
//                msg.To.Add("info@dbgurus.com.au");
//                msg.To.Add("r_mohsin@yahoo.com");
//                msg.To.Add("61403959116" + strWarningSMSEMail);
//                //msg.To.Add("8801944613448" + strWarningSMSEMail);

//#if (!DEBUG)
//                smtpClient.Send(msg);
//#endif
//            }
//        }
//        catch (Exception ex)
//        {
//            ErrorLog theErrorLog = new ErrorLog(null, "ShowWhen - temp ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
//            SystemData.ErrorLog_Insert(theErrorLog);     
//        }

//    }

//    protected void SendAlarmEmail()
//    {



//        try
//        {

//            DataTable dtMSUsers = Common.DataTableFromText(@"SELECT     MonitorSchedule.MonitorScheduleID,[User].FirstName, [User].UserID,
//                        [User].LastName, [User].Email,[Table].TableID,  [Table].TableName, MonitorSchedule.AccountID,MonitorSchedule.HasAlarm, 
//                            MonitorSchedule.AlarmDateTime, MonitorSchedule.IsAlarmSent, MonitorSchedule.MonitorScheduleID, MonitorSchedule.Description
//                            FROM         MonitorSchedule INNER JOIN
//                            MonitorScheduleUser ON MonitorSchedule.MonitorScheduleID = MonitorScheduleUser.MonitorScheduleID INNER JOIN
//                            [User] ON 
//                            MonitorScheduleUser.UserID = [User].UserID  LEFT JOIN
//                            [Table] ON MonitorSchedule.TableID = [Table].TableID
//                            WHERE MonitorSchedule.IsAlarmSent=0 AND CONVERT(date,MonitorSchedule.AlarmDateTime)<=CONVERT(date, getdate())");



//            //string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
//            //string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
//            //string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
//            //string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
//            string strWarningSMSEMail = SystemData.SystemOption_ValueByKey("WarningSMSEmail");
//            //string strEnableSSL = SystemData.SystemOption_ValueByKey("EnableSSL");
//            //string strSmtpPort = SystemData.SystemOption_ValueByKey("SmtpPort");


//            Content theContentEmail = null;
//            theContentEmail = SystemData.Content_Details_ByKey("ScheduleAlarm", null);
//            if (theContentEmail != null)
//            {
//                foreach (DataRow dr in dtMSUsers.Rows)
//                {
//                    //lets send emails

//                    string strBody = theContentEmail.ContentP;


//                    strBody = strBody.Replace("[TableName]", dr["TableName"].ToString() == "" ? "n/a" : dr["TableName"].ToString());
//                    strBody = strBody.Replace("[AlarmDateTime]", dr["AlarmDateTime"].ToString());
//                    strBody = strBody.Replace("[Description]", dr["Description"].ToString());

//                    //MailMessage msg = new MailMessage();
//                    //msg.From = new MailAddress(strEmail);

//                    string strSubject = theContentEmail.Heading;

//                    //msg.IsBodyHtml = true;

//                    ////msg.Body = strBody;
//                    //SmtpClient smtpClient = new SmtpClient(strEmailServer);
//                    //smtpClient.Timeout = 99999;
//                    //smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

//                    //smtpClient.EnableSsl = bool.Parse(strEnableSSL);
//                    //smtpClient.Port = int.Parse(strSmtpPort);


//                    //msg.To.Clear();
//                    string strTo=dr["Email"].ToString();


//                    strBody = strBody.Replace("[LastName]", dr["LastName"].ToString());
//                    strBody = strBody.Replace("[FirstName]", dr["FirstName"].ToString());
//                    //msg.Body = strBody;
//                    try
//                    {

//#if (!DEBUG)
//                        //smtpClient.Send(msg);
//#endif


//                        //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
//                        //{

//                        //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
//                        //}

                       
                        

//                        //if (msg.To.Count > 0)
//                        //{
//                            Guid guidNew = Guid.NewGuid();
//                            string strEmailUID = guidNew.ToString();

//                            EmailLog theEmailLog = new EmailLog(null, int.Parse(dr["AccountID"].ToString()), strSubject,
//                              strTo, DateTime.Now, dr["TableID"] == DBNull.Value ? null : (int?)int.Parse(dr["TableID"].ToString()),
//                              null,
//                              "Alert", strBody);
//                            theEmailLog.EmailUID = strEmailUID;
//                            //EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);

//                            string sSendEmailError = "";
//                            DBGurus.SendEmail("Alert", true, null, strSubject, strBody, "",
//                                strTo, "", "", null, theEmailLog, out sSendEmailError);


//                            Common.ExecuteText("UPDATE MonitorSchedule SET IsAlarmSent=1 WHERE MonitorScheduleID=" + dr["MonitorScheduleID"].ToString(), null);
//                            try
//                            {

//                                if (SystemData.SystemOption_ValueByKey("EmailAttachments") == "Yes"
//                                    && dr["TableID"]!=DBNull.Value)
//                                {
//                                    Table theTable = RecordManager.ets_Table_Details(int.Parse(dr["TableID"].ToString()));
//                                    if (theTable.JSONAttachmentInfo != "")
//                                    {
//                                        AttachmentSetting theAttachmentSetting = JSONField.GetTypedObject<AttachmentSetting>(theTable.JSONAttachmentInfo);
//                                        if (theAttachmentSetting.AttachOutgoingEmails != null)
//                                        {
//                                            if ((bool)theAttachmentSetting.AttachOutgoingEmails)
//                                            {
//                                                if (theAttachmentSetting.OutSavetoTableID != null)
//                                                {
//                                                    Record theChildRecord = new Record();
//                                                    theChildRecord.TableID = (int)theAttachmentSetting.OutSavetoTableID;
//                                                    theChildRecord.EnteredBy = int.Parse(dr["UserID"].ToString());

//                                                    //link the record with the parent table

//                                                    //DataTable dtChildColumn = Common.DataTableFromText("SELECT * FROM [Column] WHERE TableID=" + theChildRecord.TableID.ToString() + " AND TableTableID=" + theTable.TableID.ToString());

//                                                    //if (dtChildColumn.Rows.Count > 0)
//                                                    //{
//                                                    //    Column lnkColumn = RecordManager.ets_Column_Details(int.Parse(dtChildColumn.Rows[0]["LinkedParentColumnID"].ToString()));
//                                                    //    RecordManager.MakeTheRecord(ref theChildRecord, dtChildColumn.Rows[0]["SystemName"].ToString(), RecordManager.GetRecordValue(ref this, lnkColumn.SystemName));

//                                                    //}

//                                                    if (theAttachmentSetting.OutSaveRecipientColumnID != null)
//                                                    {
//                                                        Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveRecipientColumnID);

//                                                        RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strTo);
//                                                    }

//                                                    if (theAttachmentSetting.OutSaveSubjectColumnID != null)
//                                                    {
//                                                        Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveSubjectColumnID);

//                                                        RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strSubject);
//                                                    }

//                                                    if (theAttachmentSetting.OutSaveBodyColumnID != null)
//                                                    {
//                                                        Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveBodyColumnID);

//                                                        RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strBody);
//                                                    }
//                                                    try
//                                                    {
//                                                        RecordManager.ets_Record_Insert(theChildRecord);
//                                                    }
//                                                    catch
//                                                    {
//                                                        //

//                                                    }

//                                                }

//                                            }

//                                        }

//                                    }


//                                }

//                            }
//                            catch
//                            {
//                                //
//                            }



//                        //}


//                    }
//                    catch (Exception ex)
//                    {
//                        ErrorLog theErrorLog = new ErrorLog(null, "Send Alerts Emails Error", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
//                        SystemData.ErrorLog_Insert(theErrorLog);
//                        //strErrorMsg = "Server could not send warning Email & SMS";
//                    }

//                }

//            }

//        }
//        catch (Exception ex)
//        {
//            ErrorLog theErrorLog = new ErrorLog(null, "Send Alerts Emails Error", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
//            SystemData.ErrorLog_Insert(theErrorLog);
//            //eventLog1.WriteEntry("Send Alerts Emails Error: " + ex.Message.ToString() + "--Detail-->" + ex.StackTrace);
//        }





//    }
}





//public partial class Weather : System.Web.UI.Page
//{
//    private string weatherProviderUrl = "http://ourweatherprovider.com/api/get";

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        RegisterAsyncTask(new PageAsyncTask(GetWeatherData));
//    }

//    public async Task GetWeatherData()
//    {
//        using (HttpClient httpClient = new HttpClient())
//        {
//            var response = await httpClient.GetAsync(this.weatherProviderUrl);
//            string jsonResp = await response.Content.ReadAsStringAsync();
//            ForecastVM forecastModel = await JsonConvert.DeserializeObjectAsync<ForecastVM>(jsonResp);
//            this.ourControl.DataSource = forecastModel;
//            this.ourControl.DataBind();
//        }
//    }
//}