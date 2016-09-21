using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for ALSLiveImport
/// </summary>
public class ALSLiveImport
{
    public enum ALSLiveDataType
    {
        Raw,
        Checked
    }

    private DataTable _dtStatus;
    private List<string> _errors;

    public ALSLiveImport()
    {
        InitVariables();
    }

    private void InitVariables()
    {
        _errors = new List<string>();

        _dtStatus = new DataTable();
        _dtStatus.Columns.Add("EMD");
        _dtStatus.Columns.Add("SampleType");
        _dtStatus.Columns.Add("ServiceUrl");
        _dtStatus.Columns.Add("RecordsInserted", typeof(Int32));
        _dtStatus.Columns.Add("RecordsImported", typeof(Int32));
        _dtStatus.Columns.Add("Errors");
    }

    private string GetALSSites(int nTableID, Dictionary<string, string> sampleSiteMap)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Get_ALS_Sites", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();
                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }
                connection.Close();
                connection.Dispose();

                if (ds == null) return null;

                if (ds.Tables.Count > 0)
                {
                    string sites = String.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sites = sites + row["ALS Live Feed"] + ",";
                        sampleSiteMap.Add(row["ALS Live Feed"].ToString(), 
                            row["Site Name"].ToString());
                    }
                    sites = sites.Substring(0, sites.Length - 1);
                    return sites;
                }

                return null;
            }
        }
    }

    public void ImportALSLiveData()
    {
        int iTN = 0;
        int iTDN = 0;
        int nTableID = -1;

        List<Table> tables = RecordManager.ets_Table_Select(null, "ALS Live Web Services", null, null,
            null, null, null, "st.TableName", "ASC", null, null, ref iTN, "");
        if ((tables.Count > 0) && tables[0].TableID.HasValue)
        {
            nTableID = tables[0].TableID.Value;

            int userID = int.Parse(SystemData.SystemOption_ValueByKey_Account("AutoUploadUserID", null, null));

            Column lastRawDataUploadColumn = RecordManager.ets_Table_Columns(nTableID, null, null, ref iTN)
                .Where(x => x.DisplayName == "Last Raw Data Upload").FirstOrDefault();
            Column lastQalityCheckedDataUploadColumn = RecordManager.ets_Table_Columns(nTableID, null, null, ref iTN)
                .Where(x => x.DisplayName == "Last Quality Checked Data Upload").FirstOrDefault();
            Column lastQualityCheckedDataDateColumn = RecordManager.ets_Table_Columns(nTableID, null, null, ref iTN)
                .Where(x => x.DisplayName == "Last Quality Checked Data Date").FirstOrDefault();

            DataTable dtWebServices = Common.DataTableFromText("SELECT * FROM [ALS_Live_Web_Services]");
            foreach (DataRow serviceRow in dtWebServices.Rows)
            {
                bool uploadRaw = false;
                if (serviceRow.IsNull("Last Raw Data Upload"))
                    uploadRaw = true;
                else
                {
                    DateTime lastRawUpload = DateTime.Parse(serviceRow["Last Raw Data Upload"].ToString());
                    int rawUploadInterval = int.Parse(serviceRow["Raw Data Upload Interval"].ToString());
                    if ((DateTime.Now - lastRawUpload).TotalMinutes > rawUploadInterval)
                        uploadRaw = true;
                }

                if (uploadRaw)
                {
                    int siteTableID = -1;
                    int.TryParse(serviceRow["Sample Site Table ID"].ToString(), out siteTableID);
                    int tempTableID = -1;
                    int.TryParse(serviceRow["Temp Table ID"].ToString(), out tempTableID);
                    if (GetData(serviceRow["URI"].ToString(), ALSLiveDataType.Raw,
                        serviceRow["Site"].ToString(), serviceRow["Sample Type"].ToString(), null,
                        siteTableID, tempTableID, userID))
                    {
                        Record serviceRecord = RecordManager.ets_Record_Detail_Full((int)serviceRow["Record ID"]);
                        RecordManager.MakeTheRecord(ref serviceRecord, lastRawDataUploadColumn.SystemName, DateTime.Now);
                        RecordManager.ets_Record_Update(serviceRecord,null);
                    }
                }

                bool uploadChecked = false;
                if (serviceRow.IsNull("Last Quality Checked Data Upload"))
                    uploadChecked = true;
                else
                {
                    DateTime lastCheckedUpload = DateTime.Parse(serviceRow["Last Quality Checked Data Upload"].ToString());
                    int checkedUploadInterval = int.Parse(serviceRow["Quality Checked Data Upload Interval"].ToString());
                    if ((DateTime.Now - lastCheckedUpload).TotalMinutes > checkedUploadInterval)
                        uploadChecked = true;
                }

                if (uploadChecked)
                {
                    DateTime? startDate = null;
                    if (!serviceRow.IsNull("Last Quality Checked Data Date"))
                    {
                        startDate = DateTime.Parse(serviceRow["Last Quality Checked Data Date"].ToString());
                        if (startDate.HasValue)
                            startDate = startDate.Value.AddDays(1);
                    }
                    if (startDate == null)
                    {
                        startDate = DateTime.Today.AddMonths(-1);
                    }

                    int siteTableID = -1;
                    int.TryParse(serviceRow["Sample Site Table ID"].ToString(), out siteTableID);
                    int tempTableID = -1;
                    int.TryParse(serviceRow["Temp Table ID"].ToString(), out tempTableID);
                    if (GetData(serviceRow["URI"].ToString(), ALSLiveDataType.Checked,
                        serviceRow["Site"].ToString(), serviceRow["Sample Type"].ToString(), startDate,
                        siteTableID, tempTableID, userID))
                    {
                        Record serviceRecord = RecordManager.ets_Record_Detail_Full((int)serviceRow["Record ID"]);
                        DateTime dt = DateTime.Now;
                        dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
                        RecordManager.MakeTheRecord(ref serviceRecord, lastQalityCheckedDataUploadColumn.SystemName, dt);
                        RecordManager.MakeTheRecord(ref serviceRecord, lastQualityCheckedDataDateColumn.SystemName, startDate.Value.Date);
                        RecordManager.ets_Record_Update(serviceRecord, null);
                    }
                }
            }
        }
    }

    #region Helper Methods

    private bool GetData(string URI, ALSLiveDataType dataType,
        string incomingSite, string incomingSampleType, DateTime? startDate,
        int siteTableID, int tempTableID, int userID)
    {
        //Clear out errors
        _errors.Clear();
        long rowsCopied = 0;

        DataRow statusRow = _dtStatus.NewRow();

        statusRow["EMD"] = incomingSite;
        statusRow["SampleType"] = incomingSampleType;

        Dictionary<string, string> sampleSiteMap = new Dictionary<string, string>();
        string sites = GetALSSites(siteTableID, sampleSiteMap);

        if (!String.IsNullOrEmpty(sites))
        {
            int accountID = -1;
            int parentBatchID = -1;

            Table tempTable = RecordManager.ets_Table_Details(tempTableID);
            if ((tempTable != null) && tempTable.AccountID.HasValue)
                accountID = tempTable.AccountID.Value;

            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            if (startDate.HasValue)
            {
                startTime = startDate.Value;
                endTime = startTime.AddDays(1).AddSeconds(-1);
            }
            else
            {
                endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day,
                    endTime.Hour, (int)Math.Floor((decimal)endTime.Minute / 10m) * 10, 0);
                startTime = endTime.AddDays(-1);
            }
            //endTime = startTime.AddHours(23).AddMinutes(59).AddSeconds(59);

            Uri updatedUri = UpdateParamsInServiceUrl(URI, dataType,
                sites, startTime, endTime);
            if (updatedUri != null)
            {
                statusRow["ServiceUrl"] = updatedUri.OriginalString;

                string jsonData = DownloadJSONResponse(updatedUri);

                List<Batch> listBatch = new List<Batch>();
                int ALSFeedBatchID = -1;
                            
                try
                {
                    rowsCopied = ParseJSONResponse(jsonData, incomingSite, incomingSampleType, tempTableID,
                        out ALSFeedBatchID, userID, accountID, sampleSiteMap);

                    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
                    {
                        DataSet ds = new DataSet();
                        string error = String.Empty;
                        SqlCommand executeCommand = new SqlCommand("dbg_Import_Mapping", connection);
                        executeCommand.CommandType = CommandType.StoredProcedure;
                        executeCommand.CommandTimeout = 3600;
                        executeCommand.Parameters.AddWithValue("@TableID", tempTableID);
                        executeCommand.Parameters.AddWithValue("@BatchID", ALSFeedBatchID);
                        connection.Open();
                        System.Data.SqlClient.SqlDataAdapter dataAdapter = new SqlDataAdapter(executeCommand);
                        try
                        {
                            dataAdapter.Fill(ds);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog theErrorLog = new ErrorLog(null, "ALS Live Feed - mapping", ex.Message, ex.StackTrace, DateTime.Now, "");
                            SystemData.ErrorLog_Insert(theErrorLog);
                        }
                        connection.Close();
                        connection.Dispose();

                        if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                        {
                            parentBatchID = ALSFeedBatchID;
                            int tempBatchID = -1;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                int targetTableID = 0;
                                if (int.TryParse(dr[0].ToString(), out targetTableID))
                                {
                                                   
                                    Table targetTable = RecordManager.ets_Table_Details(targetTableID);
                                    Byte[] parameters = new Byte[16];
                                    Byte[] batchIDPart = BitConverter.GetBytes((long)parentBatchID);
                                    Byte[] tableIDPart = BitConverter.GetBytes((long)tempTableID);

                                    for (int j = 0; j < 8; j++)
                                    {
                                        parameters[j] = batchIDPart[j];
                                        parameters[j + 8] = tableIDPart[j];
                                    }

                                    string strMsg = string.Empty;

                                                
                                    //UploadManager.UploadCSV(userID, targetTable, "ALS Live Feed " + DateTime.Now.ToString(Common.LongDateWithTimeStringFormat), 
                                    //    "", new Guid(parameters),
                                    //    "", out strMsg, out tempBatchID, "als", "",
                                    //        targetTable.AccountID, null, null, parentBatchID);


                                    UploadManager.UploadCSV(userID, targetTable, "ALS Live Feed",
                                        null, new Guid(parameters), "",
                                        out strMsg, out tempBatchID, "virtual", "", accountID, targetTable.IsDataUpdateAllowed, null, null);

                                    Batch oMapBatch = UploadManager.ets_Batch_Details(tempBatchID);

                                    listBatch.Add(oMapBatch);
                                }
                            }
                        }
                    }
                    //tn.Commit();
                }
                catch (Exception ex)
                {
                    ErrorLog theErrorLog = new ErrorLog(null, "ALS Live Import - ImportALSLiveData", ex.Message, ex.StackTrace, DateTime.Now, "");
                    SystemData.ErrorLog_Insert(theErrorLog);
                    _errors.Add(ex.ToString());
                }

                try
                {
                    foreach (Batch item in listBatch)
                    {
                        if (item.BatchID.HasValue)
                        {
                            string strRollBackSQL = "DELETE Record WHERE BatchID=" + item.BatchID.ToString() + "; Update Batch set IsImported=0 WHERE BatchID=" + item.BatchID.ToString() + ";";
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
                    //
                }

                if (ALSFeedBatchID != -1)
                {
                    Batch ALSFeedBatch = UploadManager.ets_Batch_Details(ALSFeedBatchID);
                    if (ALSFeedBatch != null)
                    {
                        try
                        {
                            List<int> listID = listBatch.Select(x => x.BatchID.Value).Where(x => x != null).ToList<int>();
                            ALSCommon.ALSNotificationMessage(ALSFeedBatch, listID,
                                dataType == ALSLiveDataType.Raw ? "Raw" : "Quality assured",
                                incomingSite, incomingSampleType, sites, startTime, endTime);
                        }
                        catch (Exception ex)
                        {
                            //eventLog1.WriteEntry("Sending email failed." + " - " + ex.Message + ex.StackTrace);
                        }
                    }
                }

                try
                {
                    foreach (Batch item in listBatch)
                    {
                        //UploadManager.SamplesImportEmail(item);
                        string strImportedSamples = "0";
                        HttpRequest request = HttpContext.Current.Request;
                        string strRoot = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath;

                        UploadManager.RecordsImportEmail(item, ref strImportedSamples, strRoot);

                        //UploadManager.RecordsImportEmail(item, ref strImportedSamples, strRoot);  

                        //SecurityManager.CheckSamplesExceeded((int)item.AccountID);
                    }
                }
                catch (Exception ex)
                {
                    //eventLog1.WriteEntry("Sending email failed." + " - " + ex.Message + ex.StackTrace);
                }

                return true;
            }

            statusRow["RecordsInserted"] = rowsCopied;
            statusRow["Errors"] = _errors.Count > 0 ? string.Join("<br/><br/>", _errors.ToArray()) : "No Errors";
        }

        return false;
    }


    private Uri UpdateParamsInServiceUrl(string serviceUrl, ALSLiveDataType dataType,
        string sites, DateTime startDateTime, DateTime endDateTime)
    {
        Uri updateServiceUri = null;

        try
        {
            Uri inputServiceUrl = new Uri(serviceUrl);

            NameValueCollection queryParams = HttpUtility.ParseQueryString(inputServiceUrl.Query);

            if (queryParams.Count > 0)
            {
                JObject containerObject = JObject.Parse(queryParams[0]);

                if (containerObject != null)
                {
                    JProperty paramsProperty = containerObject.Property("params");
                    if (paramsProperty != null)
                    {
                        JObject paramsObject = (JObject)paramsProperty.Value;

                        paramsObject.Property("start_time").Value = JValue.CreateString(startDateTime.ToString("yyyyMMddHHmmss"));
                        paramsObject.Property("end_time").Value = JValue.CreateString(endDateTime.ToString("yyyyMMddHHmmss"));

                        // Replace sample sites list
                        paramsObject.Property("site_list").Value = JValue.CreateString(sites);

                        //Update DataSource param
                        //paramsObject.Property("datasource").Value = JValue.CreateString(_dataType == ALSLiveDataType.Raw ? "AJ" : "A");
                        paramsObject.Property("datasource").Value = JValue.CreateString("AJ");

                        //Update DataType param
                        paramsObject.Property("data_type").Value = JValue.CreateString(dataType == ALSLiveDataType.Raw ? "point" : "start");

                        queryParams.Set(null, JsonConvert.SerializeObject(containerObject));
                        //queryParams.Set(null, JavaScriptConvert.SerializeObject(containerObject));

                        Uri.TryCreate(string.Format("{0}?{1}",
                            inputServiceUrl.GetComponents(UriComponents.Scheme | UriComponents.HostAndPort | UriComponents.Path, UriFormat.UriEscaped),
                            queryParams.ToString()), UriKind.Absolute, out updateServiceUri);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog2 = new ErrorLog(null, "ALS Live Import - UpdateParamsInServiceUrl", ex.Message,
                String.Concat("URI: ", serviceUrl, Environment.NewLine, ex.StackTrace), DateTime.Now, "");
            SystemData.ErrorLog_Insert(theErrorLog2);
            _errors.Add(ex.ToString());
        }

        return updateServiceUri;
    }

    private string DownloadJSONResponse(Uri serviceAddress)
    {
        string JSONResponse = null;

        try
        {
            WebClient serviceClient = new WebClient();
            serviceClient.Headers["User-Agent"] = "ALS Data Import";

            JSONResponse = serviceClient.DownloadString(serviceAddress);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog2 = new ErrorLog(null, "ALS Live Import - DownloadJSONResponse", ex.Message,
                String.Concat("URI: ", serviceAddress, Environment.NewLine, ex.StackTrace), DateTime.Now, "");
            SystemData.ErrorLog_Insert(theErrorLog2);
            _errors.Add(ex.ToString());
        }

        return JSONResponse;
    }

    private int ParseJSONResponse(string jsonData, string incomingSite, string sampleType, int tempTableID,
        out int tempBatchID, int userID, int accountID, Dictionary<string, string> sampleSiteMap)
    {
        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

        int iTN = 0;
        Column incomingSiteColumn = RecordManager.ets_Table_Columns(tempTableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Incoming Site").FirstOrDefault();
        Column incomingSampleTypeColumn = RecordManager.ets_Table_Columns(tempTableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Incoming Sample Type").FirstOrDefault();
        Column sampleSiteNameColum = RecordManager.ets_Table_Columns(tempTableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Sample Point Name").FirstOrDefault();
        Column incomingAnalyteNameColumn = RecordManager.ets_Table_Columns(tempTableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Incoming Analyte Name").FirstOrDefault();
        Column dateTimeSampleColumn = RecordManager.ets_Table_Columns(tempTableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Sample Event Date").FirstOrDefault();
        Column sampleValueColumn = RecordManager.ets_Table_Columns(tempTableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Sample Value").FirstOrDefault();
        Column qualityScoreColumn = RecordManager.ets_Table_Columns(tempTableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Quality Score").FirstOrDefault();
        Column typeOfDataColumn = RecordManager.ets_Table_Columns(tempTableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Type Of Data").FirstOrDefault();

        string strIncomingSiteColumnName = incomingSiteColumn == null ? String.Empty : incomingSiteColumn.SystemName;
        string strIncomingSampleTypeColumnName = incomingSampleTypeColumn == null ? String.Empty : incomingSampleTypeColumn.SystemName;
        string strSampleSiteNameColumnName = sampleSiteNameColum == null ? String.Empty : sampleSiteNameColum.SystemName;
        string strIncomingAnalyteNameColumnName = incomingAnalyteNameColumn == null ? String.Empty : incomingAnalyteNameColumn.SystemName;
        string strDateTimeSampleColumnName = dateTimeSampleColumn == null ? String.Empty : dateTimeSampleColumn.SystemName;
        string strSampleValueColumnName = sampleValueColumn == null ? String.Empty : sampleValueColumn.SystemName;
        string strQualityScoreColumnName = qualityScoreColumn == null ? String.Empty : qualityScoreColumn.SystemName;
        string strTypeOfDataColumnName = typeOfDataColumn == null ? String.Empty : typeOfDataColumn.SystemName;

        Batch newBatch = new Batch(null, tempTableID,
            "ALS Live Feed", "ALS Live Data", null, new Guid(), userID, accountID, false);

        //need a single transaction
        newBatch.AllowDataUpdate = true;
        tempBatchID = UploadManager.ets_Batch_Insert(newBatch);

        int count = 0;
        try
        {
            JObject container = JObject.Parse(jsonData);

            //Check for any error occured
            int errorNum;
            int.TryParse(GetJTokenValue(container.Property("error_num").Value), out errorNum);

            if (errorNum > 0)
            {
                try
                {
                    _errors.Add(string.Format("error num: {0}, error_msg: {1}", errorNum, GetJTokenValue(container.Property("error_msg").Value)));
                }
                catch
                {

                }
            }

            JProperty returnProperty = container.Property("_return");

            if (returnProperty != null)
            {
                JObject tracesObject = (JObject)returnProperty.Value;

                if (tracesObject != null)
                {
                    JArray traces = (JArray)tracesObject.Property("traces").Value;

                    foreach (JObject item in traces.Children<JObject>()) //traces loop
                    {
                        JProperty traceProperty = item.Property("trace");
                        if (traceProperty != null)
                        {
                            JArray traceItems = (JArray)traceProperty.Value;

                            foreach (JObject traceItem in traceItems.Children<JObject>())
                            {
                                TempRecord newTempRecord = new TempRecord();
                                count++;
                                newTempRecord.TableID = tempTableID;
                                newTempRecord.DateTimeRecorded = DateTime.Now;
                                newTempRecord.AccountID = accountID;
                                newTempRecord.BatchID = tempBatchID;

                                UploadManager.MakeTheTempRecord(ref newTempRecord, strIncomingSiteColumnName, incomingSite);
                                UploadManager.MakeTheTempRecord(ref newTempRecord, strIncomingSampleTypeColumnName, sampleType);

                                //UploadManager.MakeTheTempRecord(ref newTempRecord, strIsValidColumnName, false);
                                //UploadManager.MakeTheTempRecord(ref newTempRecord, strIsProcessedColumnName, false);

                                string site = GetJTokenValue(item.Property("site").Value);
                                if (sampleSiteMap.ContainsKey(site))
                                    site = sampleSiteMap[site];
                                else
                                    site = String.Format("Unknown site: {0}", site);
                                UploadManager.MakeTheTempRecord(ref newTempRecord, strSampleSiteNameColumnName, site);

                                //IncomingAnalyteName = varto_details -> short_name                               
                                JProperty vartoDetailsProperty = item.Property("varto_details");
                                if (vartoDetailsProperty != null)
                                {
                                    string s = GetJTokenValue(((JObject)vartoDetailsProperty.Value).Property("short_name").Value);
                                    s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ");
                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strIncomingAnalyteNameColumnName, s);
                                }

                                //Trace data
                                if (traceItem.Property("t") != null)
                                {
                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strDateTimeSampleColumnName,
                                        DateTime.ParseExact(GetJTokenValue(traceItem.Property("t").Value),
                                            "yyyyMMddHHmmss", provider).ToString("d/M/yyyy HH:mm:ss"));
                                }
                                if (traceItem.Property("v") != null)
                                {
                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strSampleValueColumnName, traceItem.Property("v").Value);
                                }
                                if ((traceItem.Property("q") != null) && !String.IsNullOrEmpty(strQualityScoreColumnName))
                                {
                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strQualityScoreColumnName, GetJTokenValue(traceItem.Property("q").Value));
                                }
                                if ((traceItem.Property("d") != null) && !String.IsNullOrEmpty(strTypeOfDataColumnName))
                                {
                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strTypeOfDataColumnName, GetJTokenValue(traceItem.Property("d").Value));
                                }

                                int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "ALS Live Import - ParseJSONResponse", ex.Message, ex.StackTrace, DateTime.Now, "");
            SystemData.ErrorLog_Insert(theErrorLog);
            _errors.Add(ex.ToString());
        }

        return count;
    }

    private string GetJTokenValue(JToken input)
    {
        string output = string.Empty;

        if (input != null)
        {
            output = Convert.ToString(input).Trim('\"');
        }

        return output;
    }
    /*
        private int GetEMDUserID(string userName)
        {
            WhereClause wc = new WhereClause(EmdUserTable.UserName0, BaseFilter.ComparisonOperator.EqualsTo, userName);
            OrderBy ob = null;
            EmdUserRecord[] userRecord = EmdUserTable.GetRecords(wc, ob, 0, 2);
            if (userRecord.Length > 0)
                return userRecord[0].UserId0;
            else
                return 0; // System
        }

        private int CreateTempBatchRecord()
        {
            int ret = -1;

            EmdTempBatchRecord tempBatchRecord = EmdTempBatchTable.CreateNewRecord() as EmdTempBatchRecord;
            tempBatchRecord.BatchDescription = "ALS Live Data " + DateTime.Now.ToShortDateString();
            tempBatchRecord.UploadedFileName = "N/A";
            tempBatchRecord.UserIDUploaded = _autoUploadEMDUserID;
            tempBatchRecord.SampleTypeID = (int)SampleTypeEnum.ALSData;
            try
            {
                DbUtils.StartTransaction();
                tempBatchRecord.Save();
                DbUtils.CommitTransaction();
                ret = tempBatchRecord.TempBatchID;
            }
            catch (Exception ex)
            {
                string s = String.Concat("Cannot create temporary batch record.", Environment.NewLine,
                    "Error: ", ex.Message);
                DBGurus.AddErrorLog(s);
                DbUtils.RollBackTransaction();
            }
            finally
            {
                DbUtils.EndTransaction();
            }

            return ret;
        }

        private void AddProcessingLog(string message)
        {
            string error = String.Empty;
            DBGurus.ExecuteSQL(String.Format("INSERT INTO emdALSLiveFeedProcessingLog ([Message]) VALUES ('{0}')", message), out error);
        }
    */

    #endregion
}