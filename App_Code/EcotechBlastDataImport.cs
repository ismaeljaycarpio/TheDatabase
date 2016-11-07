using System;
using System.Data;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Linq;

using System.Collections.Generic;
using System.Data.SqlClient;

public class EcotechBlastDataImport
{
    //TODO logging
    private List<LogMessage> _logs;
    private string dateTimeFormat = "dd/MM/yyyy HH:mm";

    public EcotechBlastDataImport()
    {
        this._logs = new List<LogMessage>();
    }
    public string ImportBlastEvents(DynamasterConfig ecotechSiteRec)
    {
        //Table theTable = RecordManager.ets_Table_Details((int)ecotechSiteRec.TableID);
        //string strAccountID = Common.GetValueFromSQL("SELECT AccountID FROM Record Where AccountName='" + ecotechSiteRec.EMD.Replace("'", "''") + "'");

        string strAccountID = Common.GetValueFromSQL("SELECT V001 FROM Record Where RecordID=" + ecotechSiteRec.EMD);
        if (strAccountID == "")
            return "";

        bool fetchValidShotsOnly = (ecotechSiteRec.OnlyImportValidShots.ToLower() == "yes") ? true : false;
        int accountID = int.Parse(strAccountID);

        //if (accountID != 24928)
        //    return "NA";

        //Get system options from DB
        string dynamasterConfigErrors = "";
        if (string.IsNullOrEmpty(ecotechSiteRec.Username))
            dynamasterConfigErrors += "Dynamaster login is not specified; ";

        if (string.IsNullOrEmpty(ecotechSiteRec.Password))
            dynamasterConfigErrors += "Dynamaster password is not specified; ";

        if (string.IsNullOrEmpty(ecotechSiteRec.EcotechSiteID))
            dynamasterConfigErrors += "Dynamaster site id is not specified; ";

        if (string.IsNullOrEmpty(ecotechSiteRec.NumberEventsToFetch))
            dynamasterConfigErrors += "Dynamaster fetch count is not specified; ";

        string dynamasterURL = SystemData.SystemOption_ValueByKey_Account("EcotechDynamasterLink", null, null);
        if (dynamasterURL.Length == 0)
            dynamasterConfigErrors += "Ecotech Dynamaster URL is not specified (Please, check system options); ";

        if (!string.IsNullOrEmpty(dynamasterConfigErrors))
        {
            throw new Exception(dynamasterConfigErrors);
        }

        //Get authorization form
        BrowserSession browserSession = new BrowserSession();
        browserSession.Get(dynamasterURL + @"/Dynamaster/Common/Login.aspx");

        //Fill Username and Password, set Remember Me checkbox to checked
        browserSession.FormElements["txtUserName"] = ecotechSiteRec.Username;
        browserSession.FormElements["txtPassword"] = ecotechSiteRec.Password;
        browserSession.FormElements["CbRememberMe"] = "on";

        //Post filled authorization form to server
        string blastJSON = browserSession.Post(dynamasterURL + @"/Dynamaster/Common/Login.aspx");
        if (blastJSON.Contains("Invalid Username or Password"))
        {
            string error = string.Format("Invalid Username or Password for the {0} site on the Ecotech portal", ecotechSiteRec.EcotechSiteID);
            throw (new Exception(error));
        }
        //Create request to web server which returns JSON with required data
        

        string dataURL = string.Format(@"{0}/Dynamaster/Events/Events.ashx?desc=&site={1}&h=false&cat=0,2&sort=code&dir=desc&results={2}", dynamasterURL, ecotechSiteRec.EcotechSiteID.ToString(), ecotechSiteRec.NumberEventsToFetch);
        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(dataURL);

        //Create a collection to store parsed data
        List<EcotechBlastData> ecotechBlastDataRecords = new List<EcotechBlastData>();

        //Copy cookies recieved in response after authorization to this request
        if (browserSession.UpdateRequest(myHttpWebRequest))
        {
            //Execute request and get JSON string
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream());
            string JSON = myStreamReader.ReadToEnd();

            //Parse recieved JSON
            EcotechJSON ecotechJSON = (EcotechJSON)JsonConvert.DeserializeObject(JSON, typeof(EcotechJSON));


            _logs.Add(new LogMessage("  Result:", LoggingOption.Full, LogMessageCategory.Message));

            //For each record get record details
            foreach (EcotechJSONRecord record in ecotechJSON.Records)
            {
                bool isValid = true;
                if (fetchValidShotsOnly && record.Category != "Valid Shot") isValid = false;
                if (record.IsRejected || string.IsNullOrEmpty(record.Code)) isValid = false;

                if (isValid)
                {

                    //Convert from milliseconds to site's local date/time
                    DateTime posixDateTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
                    DateTime eventDateTime = posixDateTime.AddMilliseconds(record.Date);
                    eventDateTime = eventDateTime.AddMinutes(record.UtcOffset);

                    _logs.Add(new LogMessage(string.Format("Event code: {0}   Event date: {1}. ", record.Code, eventDateTime.ToString("G")), LoggingOption.Full, LogMessageCategory.Message));


                    string detailsURL = string.Format(@"{0}/Dynamaster/Events/EventStations.ashx?event={1}", dynamasterURL, record.Id.ToString());
                    HttpWebRequest detailsHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(detailsURL);

                    //Copy cookies recieved in response after authorization to this request
                    if (browserSession.UpdateRequest(detailsHttpWebRequest))
                    {
                        //Execute request and get JSON string
                        HttpWebResponse detailsHttpWebResponse = (HttpWebResponse)detailsHttpWebRequest.GetResponse();
                        StreamReader detailsStreamReader = new StreamReader(detailsHttpWebResponse.GetResponseStream());
                        string detailsJSON = detailsStreamReader.ReadToEnd();

                        //Parse recieved JSON
                        EcotechDetailJSON ecotechDetailJSON = (EcotechDetailJSON)JsonConvert.DeserializeObject(detailsJSON, typeof(EcotechDetailJSON));

                        foreach (EcotechDetailJSONRecord detailRecord in ecotechDetailJSON.Records)
                        {
                            //_logs.Add(new LogMessage(string.Format("<br/>Sample site: <strong>{0}</strong>   Overpressure: <strong>{1}</strong>.   Vibration: <strong>{2}</strong>. ", detailRecord.Name, detailRecord.Peakabdbl, detailRecord.Peakr), LoggingOption.Full, LogMessageCategory.Message));

                            //Add parsed blast data row to collection
                            EcotechBlastData ecotechBlastData = new EcotechBlastData(Convert.ToInt32(accountID), record.Code, detailRecord.Name, eventDateTime.ToString("G"), detailRecord.Peakabdbl, detailRecord.Peakr);
                            ecotechBlastDataRecords.Add(ecotechBlastData);
                        }

                    }

                }
            }
        }

        Batch newSourceBatch=null;
        User theUser = null;
        if (ecotechBlastDataRecords.Count>0)
        {
            int? iTableID=null;

            iTableID = ecotechSiteRec.SampleTableID;

            //string strDBName = Common.GetDatabaseName();

            //if (accountID == 24916)//Bulga
            //    iTableID = 2955;
            //if (accountID == 24931)//Clermont
            //    iTableID = 2956;
            //if (accountID == 24934)//Collinsville
            //    iTableID = 2957;
            //if (accountID == 24918)//Liddell
            //    iTableID = 2958;
            //if (accountID == 24919)//Mangoola
            //    iTableID = 2959;
            //if (accountID == 24920)//Mt.Owen
            //    iTableID = 2960;
            //if (accountID == 24933)//Newlands
            //    iTableID = 2961;
            //if (accountID == 24938)//OCAL
            //    iTableID = 2962;
            //if (accountID == 24928)//Rolleston
            //    iTableID = 2551;
            //if (accountID == 24936)//Ulan
            //    iTableID = 2964;

            ////LOCAL
            //if (strDBName == "thedatabase_dev_27-Jan-2016")
            //{
            //    if (accountID == 25981) //Liddell Local - Site 2970
            //        iTableID = 2969;
            //}
            ////dev
            //if (strDBName == "thedatabase_dev")
            //{
            //    if (accountID == 25049) //Liddell - Site 3411
            //        iTableID = 3410;
            //}

            if(iTableID==null)
            {
                _logs.Add(new LogMessage("Table not found for accountid"+ accountID.ToString(), LoggingOption.Minimal, LogMessageCategory.Message));
                return WriteLog();
            }

            theUser=SecurityManager.User_AccountHolder(accountID);
            newSourceBatch = new Batch(null, (int)iTableID,
              "NA",
              "NA", null, Guid.NewGuid(), theUser.UserID, accountID);
            int iBatchID = UploadManager.ets_Batch_Insert(newSourceBatch);
            newSourceBatch.BatchID = iBatchID;
            if (iBatchID<0)
            {
                return "";
            }
            foreach (EcotechBlastData rec in ecotechBlastDataRecords)
            {
                using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("ImportDynamasterBlastData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@AccountID", accountID));
                        command.Parameters.Add(new SqlParameter("@SampleSiteName", rec.EventSampleSite));
                        command.Parameters.Add(new SqlParameter("@Event", rec.EventId));
                        command.Parameters.Add(new SqlParameter("@EventDateTime", rec.EventDateTime));
                        command.Parameters.Add(new SqlParameter("@Overpressure", rec.Overpressure));
                        command.Parameters.Add(new SqlParameter("@Vibration", rec.Vibration));
                        //command.Parameters.Add(new SqlParameter("@UserID", theUser.UserID));
                        command.Parameters.Add(new SqlParameter("@BatchID", iBatchID));
                        command.Parameters.Add(new SqlParameter("@TableID", iTableID));

                        SqlParameter pRV = new SqlParameter("@Status", SqlDbType.Int);
                        pRV.Direction = ParameterDirection.Output;

                        command.Parameters.Add(pRV);

                        connection.Open();
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            //
                        }

                        connection.Close();
                        connection.Dispose();

                        //returned status: -1 Error; 0 - nothing inserted; >0 - #records inserted
                        //int.Parse(pRV.Value.ToString());
                    }
                }
            }
        }

       
        if(newSourceBatch!=null)
        {
            string strOutMsg="";
            int iFinalBatchID=0;
            Table theTable = RecordManager.ets_Table_Details((int)newSourceBatch.TableID);
            UploadManager.UploadCSV(theUser.UserID, theTable, newSourceBatch.BatchDescription, newSourceBatch.UploadedFileName,
                null, "", out strOutMsg, out iFinalBatchID, "", "", 
                newSourceBatch.AccountID, null, null, newSourceBatch.BatchID);
            Batch theBatch = UploadManager.ets_Batch_Details(iFinalBatchID);

            string strImportMsg = UploadManager.ImportClickFucntions(theBatch);
        }


        //set ecotechSiteRec.LastDataImportOn = DateTime.Now;
        string sSQL = string.Format("UPDATE Record SET V007 = '{0}' FROM Record INNER JOIN [Table] ON Record.TableID = [Table].TableID WHERE([Table].TableName = N'_DynamasterConfig') AND(Record.V001 = '{1}')", DateTime.Now.ToString(dateTimeFormat), ecotechSiteRec.EMD);

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand(sSQL, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //
                }
                connection.Close();
                connection.Dispose();
            }
        }
        
        _logs.Add(new LogMessage("Done.", LoggingOption.Minimal, LogMessageCategory.Message));
        BaseClasses.Utils.DbUtils.CommitTransaction();




        return WriteLog();
    }
    public string ImportBlastEvents()
    {


        List<DynamasterConfig> listDynamasterConfig = GetDynamastersConfig();
        // создание обьекта класса EcotechDataClassesDataContext  
 
            foreach (var rec in listDynamasterConfig)
            {

                DateTime? dtLastDataImportedOn = Common.GetDateTimeFromString(rec.LastDataImportedOn,"");

                if (dtLastDataImportedOn!=null && rec.ImportData == "Yes" &&
                    Convert.ToInt32(Common.IgnoreSymbols(rec.NumberEventsToFetch)) > 0 && Common.DaysBetween((DateTime)dtLastDataImportedOn, DateTime.Now) > 0)
                {
                    try
                    {
                        ImportBlastEvents(rec);
                    }
                    catch(Exception ex)
                    {
                        ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -ImportBlastEvents - Account= "+ rec.EMD, ex.Message, ex.StackTrace, DateTime.Now, "DBEmail");
                        SystemData.ErrorLog_Insert(theErrorLog);
                    }
                   
                }
            }
        

        return WriteLog();
    }


    #region "WriteLog"
    private string WriteLog()
    {
        // Clear log message
        string ecotechProcessingLogMessage = "";

        // Writing log message
        foreach (LogMessage log in _logs)
        {
            if (ecotechProcessingLogMessage.Length > 0)
                ecotechProcessingLogMessage += Environment.NewLine;

            if (log.Category == LogMessageCategory.Error)
            {
                ecotechProcessingLogMessage += "ERROR: " + log.Message;
            }
            else
            {
                ecotechProcessingLogMessage += log.Message;
            }
        }

        return ecotechProcessingLogMessage;
    }
    #endregion


    public List<DynamasterConfig> GetDynamastersConfig()
    {
        List<DynamasterConfig> ldc = new List<DynamasterConfig>();

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand(@"SELECT * FROM Account24929.v_DynamasterConfig", connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DynamasterConfig tempDC = new DynamasterConfig(
                                (int?)reader["TableID"],
                                (int?)reader["Record ID"],
                                reader["EMD"] == DBNull.Value ? "" : (string)reader["EMD"],
                                reader["EcotechSiteID"] == DBNull.Value ? "" : (string)reader["EcotechSiteID"],
                                reader["NumberEventsToFetch"] == DBNull.Value ? "" : (string)reader["NumberEventsToFetch"],
                                reader["ImportData"] == DBNull.Value ? "" : (string)reader["ImportData"],
                                reader["OnlyImportValidShots"] == DBNull.Value ? "" : (string)reader["OnlyImportValidShots"],
                                reader["LastCheckedOn"] == DBNull.Value ? "" : (string)reader["LastCheckedOn"],
                                reader["LastDataImportedOn"] == DBNull.Value ? "" : (string)reader["LastDataImportedOn"],
                                reader["Password"] == DBNull.Value ? "" : (string)reader["Password"],
                                reader["Username"] == DBNull.Value ? "" : (string)reader["Username"],
                                (DateTime?)reader["DateAdded"]);
                            tempDC.SampleTableID = reader["SampleTableID"] == DBNull.Value ? null : (int?)int.Parse(reader["SampleTableID"].ToString());
                            ldc.Add(tempDC);
                        }
                    }
                }
                catch
                {
                        //
                }


                connection.Close();
                connection.Dispose();
               
            }
        }   
        return ldc;


    }
}


public class EcotechJSON
{
    private int _recordReturned;
    private int _totalRecords;
    private string _sort;
    private string _dir;
    private int _pageSize;
    private EcotechJSONRecord[] _records;

    public int RecordReturned
    {
        get { return _recordReturned; }
        set { _recordReturned = value; }
    }

    public int TotalRecords
    {
        get { return _totalRecords; }
        set { _totalRecords = value; }
    }

    public string Sort
    {
        get { return _sort; }
        set { _sort = value; }
    }

    public string Dir
    {
        get { return _dir; }
        set { _dir = value; }
    }

    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }

    public EcotechJSONRecord[] Records
    {
        get { return _records; }
        set { _records = value; }
    }
}
public class EcotechJSONRecord
{
    private int _id;
    private string _code;
    private string _description;
    private long _date;
    private string _triggerSource;
    private int _triggerSourceId;
    private string _category;
    private bool _isRejected;
    private string _isHidden;
    private int _utcOffset;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Code
    {
        get { return _code; }
        set { _code = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public long Date
    {
        get { return _date; }
        set { _date = value; }
    }

    public string TriggerSource
    {
        get { return _triggerSource; }
        set { _triggerSource = value; }
    }

    public int TriggerSourceId
    {
        get { return _triggerSourceId; }
        set { _triggerSourceId = value; }
    }

    public string Category
    {
        get { return _category; }
        set { _category = value; }
    }

    public bool IsRejected
    {
        get { return _isRejected; }
        set { _isRejected = value; }
    }

    public string IsHidden
    {
        get { return _isHidden; }
        set { _isHidden = value; }
    }

    public int UtcOffset
    {
        get { return _utcOffset; }
        set { _utcOffset = value; }
    }
}
public class EcotechDetailJSON
{
    private string _dir;
    private int _pageSize;
    private int _recordReturned;
    private EcotechDetailJSONRecord[] _records;
    private string _sort;
    private int _totalRecords;
    public string Dir
    {
        get { return _dir; }
        set { _dir = value; }
    }
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }
    public int RecordReturned
    {
        get { return _recordReturned; }
        set { _recordReturned = value; }
    }
    public EcotechDetailJSONRecord[] Records
    {
        get { return _records; }
        set { _records = value; }
    }
    public string Sort
    {
        get { return _sort; }
        set { _sort = value; }
    }
    public int TotalRecords
    {
        get { return _totalRecords; }
        set { _totalRecords = value; }
    }
}
public class EcotechDetailJSONRecord
{
    private int _id;
    private string _name;
    private string _status;
    private string _auditstatus;
    private string _audittext;
    private string _peakl;
    private string _peakv;
    private string _peakt;
    private string _peakab;
    private string _peakabdbl;
    private string _peakr;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public string Status
    {
        get { return _status; }
        set { _status = value; }
    }
    public string Auditstatus
    {
        get { return _auditstatus; }
        set { _auditstatus = value; }
    }
    public string Audittext
    {
        get { return _audittext; }
        set { _audittext = value; }
    }
    public string Peakl
    {
        get { return _peakl; }
        set { _peakl = value; }
    }
    public string Peakv
    {
        get { return _peakv; }
        set { _peakv = value; }
    }
    public string Peakt
    {
        get { return _peakt; }
        set { _peakt = value; }
    }
    public string Peakab
    {
        get { return _peakab; }
        set { _peakab = value; }
    }
    public string Peakabdbl
    {
        get { return _peakabdbl; }
        set { _peakabdbl = value; }
    }
    public string Peakr
    {
        get { return _peakr; }
        set { _peakr = value; }
    }
}

public class EcotechBlastData
{
    private int _siteId;
    private string _eventId;
    private string _eventSampleSite;
    private string _eventDateTime;
    private string _overpressure;
    private string _vibration;

    public EcotechBlastData(int _siteId, string _eventId, string _eventSampleSite, string _eventDateTime, string _overpressure, string _vibration)
    {
        this._siteId = _siteId;
        this._eventId = _eventId;
        this._eventSampleSite = _eventSampleSite;
        this._eventDateTime = _eventDateTime;
        this._overpressure = _overpressure;
        this._vibration = _vibration;
    }

    public int SiteId
    {
        get
        {
            return _siteId;
        }

        set
        {
            _siteId = value;
        }
    }

    public string EventId
    {
        get
        {
            return _eventId;
        }

        set
        {
            _eventId = value;
        }
    }

    public string EventSampleSite
    {
        get
        {
            return _eventSampleSite;
        }

        set
        {
            _eventSampleSite = value;
        }
    }

    public string EventDateTime
    {
        get
        {
            return _eventDateTime;
        }

        set
        {
            _eventDateTime = value;
        }
    }

    public string Overpressure
    {
        get
        {
            return _overpressure;
        }

        set
        {
            _overpressure = value;
        }
    }

    public string Vibration
    {
        get
        {
            return _vibration;
        }

        set
        {
            _vibration = value;
        }
    }
}


public class LogMessage
{
    private string _message;
    private LoggingOption _logLevel = LoggingOption.None;
    private LogMessageCategory _category = LogMessageCategory.Message;

    public LogMessage(string message, LoggingOption logLevel, LogMessageCategory category)
    {
        this._message = message;
        this._logLevel = logLevel;
        this._category = category;
    }

    public string Message
    {
        get { return _message; }
    }

    public LoggingOption LogLevel
    {
        get { return _logLevel; }
    }

    public LogMessageCategory Category
    {
        get { return _category; }
    }
}

public enum LoggingOption
{
    None = 0,
    Minimal = 1,
    Full = 2
}

public enum LogMessageCategory
{
    Message = 1,
    Error = 2
}