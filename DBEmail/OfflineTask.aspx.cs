using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DocGen.DAL;
using System.IO;
using System.Web.Security;
public partial class DBEmail_OfflineTask : System.Web.UI.Page
{

    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";
    string _strPhyFolder = "";
    string _strHTTPFolder = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        _strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath", null, null);
        _strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation", null, null);

        _strPhyFolder=_strFilesPhisicalPath+ "\\UserFiles\\Export\\";

        _strHTTPFolder = _strFilesLocation + "/UserFiles/Export/";

        if(!IsPostBack)
        {
           
            try
            {
                LogMeIn();
                ProcessOfflineTaskList();

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "OfflineTask - ProcessOfflineTaskList", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);

            }
        }
    }

    protected void LogMeIn()
    {
        //int iUserID = int.Parse(SystemData.SystemOption_ValueByKey_Account("AutoUploadUserID", null, null));
        //User etUser = SecurityManager.User_Details(iUserID);
        //string strUserInfor = string.Format("{0};{1};{2};{3};{4}", etUser.Email, etUser.Password, etUser.FirstName, etUser.UserID, iAccountID);
        
        try
        {
            string strUserInfor = string.Format("{0};{1};{2};{3};{4}", "auto@dbgurus.com.au", "11legend", "Auto", 1, 1);
            FormsAuthentication.SetAuthCookie(strUserInfor, false);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "OfflineTask - LogMeIn", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

        }
        
    }

    protected void ProcessOfflineTaskList()
    {
        DataTable dtOfflineTaskList = OfflineTaskManager.dbg_OfflineTask_ListToProcess();

        if (dtOfflineTaskList != null && dtOfflineTaskList.Rows.Count > 0)
        {
            foreach (DataRow drOT in dtOfflineTaskList.Rows)
            {
                if (drOT["Processtorun"].ToString() == "ExportRecords" && drOT["Parameters"] != DBNull.Value)
                {
                    ProcessExportRecords(drOT);
                }

                if (drOT["Processtorun"].ToString() == "DeleteFile" && drOT["Parameters"] != DBNull.Value)
                {
                    ProcessDeleteFile(drOT);
                }
            }
        }
    }
    
    
    
    
    
    protected void ProcessExportRecords(DataRow drOT)
    {
        try
        {
            OfflineTaskParameters aOTParam = JSONField.GetTypedObject<OfflineTaskParameters>(drOT["Parameters"].ToString());

            if(aOTParam!=null)
            {
                int iResult=OfflineTaskManager.ets_Record_List_BCP(aOTParam.ReturnSQL, aOTParam.ReturnHeaderSQL, _strPhyFolder+aOTParam.UniqueFileName);

                if (iResult == -1)
                    return;
                //Send Email
                Content theContentEmail = SystemData.Content_Details_ByKey("BulkRecordsExportEmailTemplate", int.Parse(drOT["AccountID"].ToString()));
                if (theContentEmail == null)
                    return;
                OfflineTask aOfflineTask = OfflineTaskManager.dbg_OfflineTask_Detail(int.Parse(drOT["OfflineTaskID"].ToString()));

                string strClickHere = "<a href='" + _strHTTPFolder + aOTParam.UniqueFileName+".csv" + "'>"+aOTParam.FileFriendlyName+"</a>";
                string strBody = theContentEmail.ContentP;
                string strSubject = theContentEmail.Heading.Replace("[FileName]", aOTParam.FileFriendlyName);
                strBody = strBody.Replace("[FirstName]", drOT["FirstName"].ToString());
                strBody = strBody.Replace("[TableName]", aOTParam.TableName);
                strBody = strBody.Replace("[filelink]", strClickHere);

                string sSendEmailError = "";


                Message theMessage = new Message(null, null, aOTParam.TableID, int.Parse(drOT["AccountID"].ToString()),
                   DateTime.Now, "E", "E",
                       null, drOT["Email"].ToString(), strSubject, strBody, null, "");


                DBGurus.SendEmail("OfflineTask " +aOfflineTask.Processtorun, true, null, strSubject, strBody, "", drOT["Email"].ToString(), "", "", null, theMessage, out sSendEmailError);
                
                //Done
                DoneProcess(null,aOfflineTask);                

                //Log it
                OfflineTaskLog aOfflineTaskLog = new OfflineTaskLog(null, int.Parse(drOT["OfflineTaskID"].ToString()), "OK, " + aOTParam.TotalNumberOfRecords + " records exported.", null);
                OfflineTaskManager.dbg_OfflineTaskLog_Insert(aOfflineTaskLog);

            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, " OfflineTask - ProcessExportRecords", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

            OfflineTaskLog aOfflineTaskLog = new OfflineTaskLog(null, int.Parse(drOT["OfflineTaskID"].ToString()), "error, ExportRecords, " + ex.Message, null);
            OfflineTaskManager.dbg_OfflineTaskLog_Insert(aOfflineTaskLog);
        }
    }

    protected void DoneProcess(int? iOfflineTaskID, OfflineTask aOfflineTask)
    {

        if (aOfflineTask==null)
            aOfflineTask = OfflineTaskManager.dbg_OfflineTask_Detail((int)iOfflineTaskID);


        aOfflineTask.ActuallyRun = DateTime.Now;
        OfflineTaskManager.dbg_OfflineTask_Update(aOfflineTask);
    }

    protected void ProcessDeleteFile(DataRow drOT)
    {
        try
        {
            string strFile = _strPhyFolder + drOT["Parameters"].ToString();
            if (File.Exists(strFile))
            {
                File.Delete(strFile);

                //
                DoneProcess(int.Parse(drOT["OfflineTaskID"].ToString()),null); 
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "OfflineTask - ProcessDeleteFile", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

            OfflineTaskLog aOfflineTaskLog = new OfflineTaskLog(null, int.Parse(drOT["OfflineTaskID"].ToString()), "error, DeleteFile, " + ex.Message, null);
            OfflineTaskManager.dbg_OfflineTaskLog_Insert(aOfflineTaskLog);
        }
    }

}