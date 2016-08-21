using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;

[Serializable]
public class EmailCallBack
{
    public EmailCallBack()
    {
    }
    public MailMessage TheMail { get; set; }
    public Message TheMessage { get; set; }
}

    public class DBGurus
    {

        public DBGurus()
        {
        }
        public static string strGlobalConnectionString = ConfigurationManager.ConnectionStrings["CnString"].ConnectionString;
        //public static string strGlobalTestConnectionString = ConfigurationManager.ConnectionStrings["TestArea"].ConnectionString;

        public static string strGodEmail = "info@dbgurus.com.au";
        /// <summary>
        /// Formats a date for presentation e.g. UK (DD/MM/YYYY), Us (MM/DD/YYYY), ODBC (YYYY-MM-DD) or you specify e.g. {0:MM-dd-yyyy}
        /// </summary>
        /// <param name="dDate"></param>
        /// <param name="sOutFormat"></param>
        /// <returns></returns>
        public static string DateFormat(object oDate, string sOutFormat)
        {
            DateTime dDate;
            if (DateTime.TryParse(oDate.ToString(), out dDate))
            {
                if (sOutFormat.StartsWith("UK"))
                    sOutFormat = "dd/MM/yyyy";
                else if (sOutFormat.StartsWith("US"))
                    sOutFormat = "MM/dd/yyyy";
                else if (sOutFormat.StartsWith("ODBC"))
                    sOutFormat = "yyyy-MM-dd";
                else
                {
                    sOutFormat = sOutFormat.Replace("D", "d");
                    sOutFormat = sOutFormat.Replace("m", "M");
                    sOutFormat = sOutFormat.Replace("Y", "y");
                }
                return dDate.ToString(sOutFormat);
            }
            return "";
        }

        public static void WriteLogFile(string message)
        {
            StreamWriter log;
            string fileName = "./ErrorLog/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (!System.IO.File.Exists(fileName))
            {
                log = new StreamWriter(fileName);
            }
            else
            {
                log = System.IO.File.AppendText(fileName);
            }

            // Write to the file:
            log.WriteLine(DateTime.Now);
            log.WriteLine(message);
            log.WriteLine("---------------------------------------------------------------------------------------");

            // Close the stream:
            log.Close();
        }

        /// <summary>
        /// Formats a date for presentation e.g. UK (DD/MM/YYYY), Us (MM/DD/YYYY), ODBC (YYYY-MM-DD) or you specify e.g. {0:MM-dd-yyyy}
        /// </summary>
        /// <param name="sDate">Date to be processed as a string</param>
        /// <param name="sInFormat"></param>
        /// <param name="sOutFormat"></param>
        /// <returns></returns>
        /// <remarks>This instance processes a string into a date and then calls the previous instance</remarks>
        public static string DateFormat(string sDate, string sInFormat, string sOutFormat)
        {
            string returnValue = "";
            if (sInFormat == "UK")
                sInFormat = "DD/MM/YYYY";
            if (sInFormat == "US")
                sInFormat = "MM/DD/YYYY";
            if (sOutFormat == "UK")
                sOutFormat = "DD/MM/YYYY";
            if (sOutFormat == "US")
                sOutFormat = "MM/DD/YYYY";

            if ((sDate.Length == sInFormat.Length) && (sInFormat.ToUpper().Contains("D")) && (sInFormat.ToUpper().Contains("M")) && (sInFormat.ToUpper().Contains("Y")))
            {
                string sDays = sDate.Substring(sInFormat.ToUpper().IndexOf("D"), 2);
                string sMonths = sDate.Substring(sInFormat.ToUpper().IndexOf("M"), 2);
                string sYears = sDate.Substring(sInFormat.ToUpper().IndexOf("Y"), 4);

                if ((IsNumeric(sDays)) && (IsNumeric(sMonths)) && (IsNumeric(sMonths)))
                {
                    DateTime dt = new DateTime(Int32.Parse(sYears), Int32.Parse(sMonths), Int32.Parse(sDays));
                    returnValue = DateFormat(dt, sOutFormat);
                }
            }
            return returnValue;
        }

        public static bool ValidateDateString(string dateString)
        {
            bool ret = false;
            DateTime tempDateTime;

            if (DateTime.TryParse(dateString, out tempDateTime))
                ret = true;

            return ret;
        }
        public static string GetContent(string sContentKey)
        {
            int nResult = 0;
            string sErrorInfo = "";

            DataSet dataSet = new DataSet();

            nResult = DBGurus.ExecuteSQLDataSet(dataSet, String.Concat("SELECT Content from Content WHERE ContentKey = '", sContentKey, "'"), out sErrorInfo);
            if (nResult == 0)
            {
                if (dataSet.Tables[0].Rows.Count == 1)
                {
                    return dataSet.Tables[0].Rows[0]["Content"].ToString();
                }
            }
            return sErrorInfo;
        }

        /// <summary>Override with simpler option (no default)</summary>
        //public static string GetSystemOption(string sKey)
        //{
        //    return GetSystemOption(sKey, "");
        //}

        /// <summary>
        /// Get the system option. Pass a default value if it is not found. Overide without error message.
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sDefault"></param>
        /// <returns></returns>
        //public static string GetSystemOption(string sKey, string sDefault)
        //{
        //    string sErrorMessage;
        //    return GetSystemOption(sKey, sDefault, out sErrorMessage);
        //}

        /// <summary>
        /// Get the system option. Pass a default value if it is not found. 
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sDefault"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        //public static string GetSystemOption(string sKey, string sDefault, out string sErrorMessage)
        //{
        //    string returnValue = sDefault;
        //    DataSet dataSet = new DataSet();
        //    if (DBGurus.ExecuteSQLDataSet(dataSet, String.Concat("SELECT * FROM SystemOption WHERE OptionKey = '", sKey, "'"), out sErrorMessage) == 0)
        //    {
        //        if (dataSet.Tables[0].Rows.Count == 1)
        //            returnValue = dataSet.Tables[0].Rows[0]["OptionValue"].ToString();
        //    }
        //    dataSet.Dispose();
        //    return returnValue;
        //}

        public static string GetUserAttribute(string userID, string sAttribute, out string sError)
        {
            string returnValue = "";
            DataSet ds = new DataSet();
            string sSQL = string.Concat("SELECT ", sAttribute, " FROM User WHERE UserID = ", userID);
            if (DBGurus.ExecuteSQLDataSet(ds, sSQL, out sError) == 0)
                returnValue = ds.Tables[0].Rows[0][sAttribute].ToString();
            ds.Dispose();
            return returnValue;
        }

        /// <summary>
        /// Run a stored procedure that does not return a recordset.
        /// </summary>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="command"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        public static int ExecuteSP(string sStoredProcedureName, SqlCommand command, out string sErrorMessage)
        {
            int returnValue = 0;
            sErrorMessage = "";
            SqlParameter pRV = new SqlParameter("@RETURNVALUE", SqlDbType.Int);
            pRV.Direction = ParameterDirection.ReturnValue;

            command.Parameters.Add(pRV);

            command.CommandText = sStoredProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = new SqlConnection(Settings.GetConnectionString());
            command.CommandTimeout = 300;

            command.Connection.Open();
            int nReturnedRows = 0;
            try
            {
                nReturnedRows = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("REFERENCE constraint"))
                    sErrorMessage = GetReferenceConstraintMessage(e.Message);
                else
                    sErrorMessage = e.Message;
            }
            command.Connection.Close();
            command.Connection.Dispose();
            if (pRV.Value == null)
                returnValue = -99999;
            else
                returnValue = DBGurus.StringToInt(pRV.Value.ToString());

            return returnValue;
        }

        /// <summary>
        /// Run a stored procedure that does not return a recordset.
        /// </summary>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="sParameterName"></param>
        /// <param name="oParameterValue"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        public static int ExecuteSP(string sStoredProcedureName, string sParameterName, object oParameterValue, out string sErrorMessage)
        {
            int returnValue = -1;
            SqlCommand executeCommand = new SqlCommand();
            executeCommand.Parameters.AddWithValue(sParameterName, oParameterValue);
            returnValue = ExecuteSP(sStoredProcedureName, executeCommand, out sErrorMessage);
            executeCommand.Dispose();
            return returnValue;
        }


        /// <summary>
        /// Run a stored procedure and bring back a dataset. Returns success indicator. No error message. 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="command"></param>
        /// <returns>0 for success</returns>
        public static int ExecuteSPDataset(DataSet dataSet, string sStoredProcedureName, SqlCommand command)
        {
            string sErrorMessage = ""; // Not used
            return ExecuteSPDataset(dataSet, sStoredProcedureName, command, null, out sErrorMessage);
        }

        /// <summary>
        /// Run a stored procedure and bring back a dataset. Returns success indicator. 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="command"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        public static int ExecuteSPDataset(DataSet dataSet, string sStoredProcedureName, SqlCommand command, out string sErrorMessage)
        {
            return ExecuteSPDataset(dataSet, sStoredProcedureName, command, null, out sErrorMessage);
        }

        /// <summary>
        /// Run a stored procedure and bring back a dataset. Returns success indicator. Allows one parameter.
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="sParameter"></param>
        /// <param name="oValue"></param>
        /// <param name="sError"></param>
        /// <returns></returns>
        public static int ExecuteSPDataset(DataSet dataSet, string sStoredProcedureName, string sParameter, object oValue, out string sError)
        {
            int returnValue = -1;
            SqlCommand executeCommand = new SqlCommand();
            executeCommand.Parameters.AddWithValue(sParameter, oValue);
            returnValue = ExecuteSPDataset(dataSet, sStoredProcedureName, executeCommand, null, out sError);
            //executeCommand.Dispose();
            return returnValue;
        }

        /// <summary>
        /// Run a stored procedure and bring back a dataset. Returns success indicator. Allows table mappings.
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="command"></param>
        /// <param name="tableMappings"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        public static int ExecuteSPDataset(DataSet dataSet, string sStoredProcedureName, SqlCommand command, DataTableMapping[] tableMappings, out string sErrorMessage)
        {
            sErrorMessage = "";
            int returnValue = 0;
            command.CommandText = sStoredProcedureName;
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter pRV = new SqlParameter("@RETURNVALUE", SqlDbType.Int);
            pRV.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(pRV);
            command.Connection = new SqlConnection(Settings.GetConnectionString());
            command.CommandTimeout = 300;
            System.Data.SqlClient.SqlDataAdapter dataAdapter = new SqlDataAdapter(command);


           
            if (tableMappings != null)
            {
                foreach (DataTableMapping map in tableMappings)
                    dataAdapter.TableMappings.Add((object)map);
            }

            try
            {
                dataAdapter.Fill(dataSet);
            }
            catch (Exception e)
            {
                returnValue = -1;
                sErrorMessage = e.Message;
            }

            return returnValue;

        }

        /// <summary>
        /// Runs an SQL statement and provides a record set. Returns 0 if successful
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="sSQL"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        public static int ExecuteSQL(string sSQL, out string sErrorMessage)
        {
            int returnValue = 0;
            sErrorMessage = "";

            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            command.CommandText = sSQL;
            command.CommandType = CommandType.Text;
            command.Connection = new SqlConnection(Settings.GetConnectionString());
            command.CommandTimeout = 300;
            command.Connection.Open();

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                returnValue = -1;
                sErrorMessage = e.Message;
            }
            command.Connection.Close();
            command.Connection.Dispose();
            return returnValue;
        }

        /// <summary>
        /// Runs an SQL statement and provides a record set. Returns 0 if successful
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="command"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        public static int ExecuteSQL(string sSQL, SqlCommand command, out string sErrorMessage)
        {
            int returnValue = 0;
            sErrorMessage = "";

            command.CommandText = sSQL;
            command.CommandType = CommandType.Text;
            command.Connection = new SqlConnection(Settings.GetConnectionString());
            command.CommandTimeout = 300;
            command.Connection.Open();

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                returnValue = -1;
                sErrorMessage = e.Message;
            }
            command.Connection.Close();
            command.Connection.Dispose();
            return returnValue;
        }

        /// <summary>
        /// Runs an SQL statement and provides a record set. Returns 0 if successful
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="sSQL"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        public static int ExecuteSQLDataSet(DataSet dataSet, string sSQL, out string sErrorMessage)
        {
            int returnValue = 0;
            sErrorMessage = "";

            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            command.CommandText = sSQL;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.Connection = new SqlConnection(Settings.GetConnectionString());

            System.Data.SqlClient.SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

            try
            {
                dataAdapter.Fill(dataSet);
            }
            catch (Exception e)
            {
                returnValue = -1;
                sErrorMessage = e.Message;
            }
            command.Connection.Close();
            return returnValue;
        }

        /// <summary>
        /// Fills the passed dropdown with values from the database
        /// </summary>
        /// <param name="dropDown">Name of dropdown e.g. oStates</param>
        /// <param name="sTable">Name of table to get data from</param>
        /// <param name="sDisplayColumn">Column to display</param>
        /// <param name="sValueColumn">Value column (hidden)</param>
        /// <param name="sWhereCondition">Optional where clause</param>
        /// <param name="sError">Output</param>
        /// <returns>0 for success</returns>
        public static int FillDropDown(DropDownList dropDown, string sTable, string sDisplayColumn, string sValueColumn, string sWhereCondition, out string sError)
        {
            int returnValue = -1;
            sError = "";
            if ((sWhereCondition == null) || (sWhereCondition.Length == 0))
                sWhereCondition = "1=1";
            string sSQL = String.Format("SELECT {0} AS DisplayColumn, {1} AS ValueColumn FROM {2} WHERE {3}",
                sDisplayColumn, sValueColumn, sTable, sWhereCondition);

            // Now we add the first item
            sSQL = String.Concat("SELECT '** Please Select **' AS DisplayColumn, NULL AS ValueColumn UNION ", sSQL);

            DataSet ds = new DataSet();

            if (DBGurus.ExecuteSQLDataSet(ds, sSQL, out sError) == 0)
            {
                dropDown.DataSource = ds;
                dropDown.DataTextField = "DisplayColumn";
                dropDown.DataValueField = "ValueColumn";
                dropDown.DataBind();
                returnValue = 0;
            }
            ds.Dispose();
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSQL">Must be in the format Select X as DisplayColumn, Y as ValueColumn FROM... E.g. SELECT DISTINCT CAST(YearRecorded AS varchar(50)) AS DisplayColumn, YearRecorded as ValueColumn FROM emdGasFugitive ORDER BY DisplayColumn</param>
        /// <param name="sTopItem">The text to appear in the dropdown. Pass null to use default ** Please Select **</param>
        /// <param name="sError"></param>
        /// <returns></returns>
        public static int FillDropDown(DropDownList dropDown, string sSQL, string sTopItem, out string sError)
        {
            int returnValue = -1;
            sError = "";

            if (sTopItem == null)
                sTopItem = "** Please Select **";

            sSQL = String.Concat("SELECT '" + sTopItem + "' AS DisplayColumn, NULL AS ValueColumn UNION ", sSQL);

            DataSet ds = new DataSet();

            if (DBGurus.ExecuteSQLDataSet(ds, sSQL, out sError) == 0)
            {
                dropDown.DataSource = ds;
                dropDown.DataTextField = "DisplayColumn";
                dropDown.DataValueField = "ValueColumn";
                dropDown.DataBind();
                returnValue = 0;
            }
            ds.Dispose();
            return returnValue;
        }

        /// <summary>
        /// Returns the passed filename as a string
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string getFileAsString(string fileName)
        {
            StreamReader sReader = null;
            string contents = null;
            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                sReader = new StreamReader(fileStream);
                contents = sReader.ReadToEnd();
            }
            finally
            {
                if (sReader != null)
                {
                    sReader.Close();
                }
            }
            return contents;
        }

        /// <summary>
        /// Get content using the ContentKey
        /// </summary>
        public static string GetContentWithValues(string sContentID, string sKeyForSP, out string nContentKey, out string sError)
        {
            string sContent = "";
            nContentKey = "";
            DataSet dsContent = new DataSet();
            if (DBGurus.ExecuteSQLDataSet(dsContent, String.Concat("SELECT * FROM Content WHERE ContentID = '", sContentID, "'"), out sError) == 0)
            {
                if (dsContent.Tables[0].Rows.Count > 0)
                {
                    nContentKey = dsContent.Tables[0].Rows[0]["ContentKey"].ToString();
                    sContent = ReplaceContentValues(dsContent.Tables[0].Rows[0]["Content"].ToString(),
                        dsContent.Tables[0].Rows[0]["StoredProcedure"].ToString(),
                        sKeyForSP, out sError);
                }
                else
                    sError = "That ContentID could not be located";
            }
            return sContent;
        }

        /// <summary>
        /// Get content using the ContentKey
        /// </summary>
        public static string GetContentHeadingWithValues(string sContentID, string sKeyForSP, out string nContentKey, out string sError)
        {
            string sContent = "";
            nContentKey = "";
            DataSet dsContent = new DataSet();
            if (DBGurus.ExecuteSQLDataSet(dsContent, String.Concat("SELECT * FROM Content WHERE ContentID = '", sContentID, "'"), out sError) == 0)
            {
                if (dsContent.Tables[0].Rows.Count > 0)
                {
                    nContentKey = dsContent.Tables[0].Rows[0]["ContentKey"].ToString();
                    sContent = ReplaceContentValues(dsContent.Tables[0].Rows[0]["Heading"].ToString(),
                        dsContent.Tables[0].Rows[0]["StoredProcedure"].ToString(),
                        sKeyForSP, out sError);
                }
                else
                    sError = "That ContentID could not be located";
            }
            return sContent;
        }

        public static string ReplaceContentValues(string sContent, string sStoredProcedure, string sKeyForSP, out string sError)
        {
            sError = "";
            if ((sStoredProcedure != null) && (sStoredProcedure.Length > 0))
            {
                DataSet dsCustom = new DataSet();
                if (DBGurus.ExecuteSPDataset(dsCustom, sStoredProcedure, "@sKey", sKeyForSP, out sError) == 0)
                {
                    if ((dsCustom.Tables.Count > 0) && (dsCustom.Tables[0].Rows.Count > 0))
                    {
                        for (int i = 0; i < dsCustom.Tables[0].Columns.Count; i++)
                        {
                            string sColumnName = dsCustom.Tables[0].Columns[i].ColumnName;
                            sContent = sContent.Replace(String.Concat("[", sColumnName, "]"), dsCustom.Tables[0].Rows[0][sColumnName].ToString());
                            sContent = sContent.Replace(String.Concat("%5B", sColumnName, "%5D"), dsCustom.Tables[0].Rows[0][sColumnName].ToString());
                        }
                    }
                }
                else sContent = string.Concat("Stored procedure ", sStoredProcedure, " returned the following error message", sError);
            }

            bool bWebRootURL = sContent.Contains("[WebRootURL]");
            bool bWebRootURLFF = sContent.Contains("%5BWebRootURL%5D");
            if (bWebRootURL
                || bWebRootURLFF)
            {
                string host = HttpContext.Current.Request.Url.Host;
                if (HttpContext.Current.Request.Url.Port != 80)
                    host = String.Concat(host, ":", HttpContext.Current.Request.Url.Port.ToString());
                ListItem item = new ListItem();
                string url = string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, host, HttpContext.Current.Request.ApplicationPath);

                if (bWebRootURL)
                    sContent = sContent.Replace(String.Concat("[WebRootURL]"), url);
                else if (bWebRootURLFF)
                    sContent = sContent.Replace(String.Concat("%5BWebRootURL%5D"), url);
            }

            return sContent;
        }

        public static int GetContentTypeID(string sContentTypeKey)
        {
            int nContentTypeID = -1;
            string sError = "";

            if (GetContentTypeID(sContentTypeKey, out nContentTypeID, out sError) == 0)
                return nContentTypeID;
            else
                return -1;
        }

        public static int GetContentTypeID(string sContentTypeKey, out int nContentTypeID, out string sError)
        {
            nContentTypeID = 0;
            // Need to get the content type ID first (from the key)
            DataSet dsContentType = new DataSet();
            if (DBGurus.ExecuteSQLDataSet(dsContentType, String.Concat("SELECT * FROM Content WHERE ContentTypeKey = '", sContentTypeKey, "'"), out sError) == 0)
            {
                if ((dsContentType.Tables.Count > 0) && (dsContentType.Tables[0].Rows.Count > 0))
                {
                    nContentTypeID = Int32.Parse(dsContentType.Tables[0].Rows[0]["ContentTypeID"].ToString());
                    dsContentType.Dispose();
                }
            }
            else return -1;
            return 0;  // Normal
        }

        public static int AddErrorLog(string error)
        {
            return AddErrorLog(error, null, null);
        }

        public static int AddErrorLog(string error, string url, string userHostAddress)
        {
            int ret = -1;

            try
            {

                //ErrorLog rec = new ErrorLog();
                //if (url == null && userHostAddress == null)
                //{
                //    rec.Error = error;
                //}
                //else
                //{
                //    rec.Error = string.Format("Error:{0}", error);
                //}

                //if (url != null)
                //{
                //    rec.Error = string.Concat(rec.Error, string.Format(";URL:{0}", url));
                //}

                //if (userHostAddress != null)
                //{
                //    rec.Error = string.Concat(rec.Error, string.Format(";IP:{0}", userHostAddress));
                //}
                //rec.DateTimeStamp = DateTime.Now;
                //srcLF.ErrorLogs.InsertOnSubmit(rec);

                //ret = rec.ErrorLogID;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return ret;
        }

        public static string StandardString(string sContent)
        {
            sContent = sContent.Trim();
            sContent = sContent.Replace("<td>", "");
            sContent = sContent.Replace("</td>", "");
            sContent = sContent.Replace("<tr>", "");
            sContent = sContent.Replace("</tr>", "");
            sContent = sContent.Replace("<table>", "");
            sContent = sContent.Replace("</table>", "");
            sContent = sContent.Replace("<script", "");
            sContent = sContent.Replace("</script>", "");
            sContent = sContent.Replace("'", "");
            sContent = sContent.Replace("OR", "");
            sContent = sContent.Replace("ALTER", "");
            sContent = sContent.Replace("DROP", "");
            return sContent;
        }

        /// <summary>
        /// Returns the position of an item in a list. E.g DropDown.SelectedIndex = GetListIndex(DropDown.Items, "ValueToFind")
        /// You can pass the text to find, the value to find or both
        /// </summary>
        /// <returns></returns>
        public static int GetListIndex(ListItemCollection list, string sItemValue)
        {
            int returnValue = -1;
            int nCounter = 0;
            foreach (ListItem item in list)
            {
                if ((sItemValue != null) && (item.Value == sItemValue))
                    returnValue = nCounter;

                nCounter++;
            }
            return returnValue;
        }

        /// <summary>
        /// Returns the position of an item in a list. E.g DropDown.SelectedIndex = GetListIndex(DropDown.Items, "ValueToFind")
        /// You can pass the text to find, the value to find or both
        /// </summary>
        /// <returns></returns>
        public static int GetListIndex(ListItemCollection list, string sText, Boolean bUseDropdownText)
        {
            int returnValue = -1;
            int nCounter = 0;
            foreach (ListItem item in list)
            {
                if ((bUseDropdownText) && (item.Text == sText))
                    returnValue = nCounter;

                if ((!bUseDropdownText) && (item.Value == sText))
                    returnValue = nCounter;

                nCounter++;
            }
            return returnValue;
        }


        private static string GetReferenceConstraintMessage(string sExceptionMessage)
        {
            int nStartTableName = sExceptionMessage.IndexOf("dbo.") + 4;
            string sReferenceConstraintMessage = String.Concat(
                "It is not possible to delete this record because it is used on records in another table (foreign key constraint). Please remove all rows in ",
                sExceptionMessage.Substring(nStartTableName, sExceptionMessage.IndexOf("column") - 3 - nStartTableName),
                " that reference this record and then try again");
            return sReferenceConstraintMessage;
        }

        /// <summary>
        /// Tests if the string is numeric. Returns true if string is a number  
        /// </summary>
        /// <returns></returns>
        public static bool IsNumeric(string sValue)
        {
            double nResult;

            //check whether string argument is numeric
            bool bIsNumeric = Double.TryParse(sValue,
                System.Globalization.NumberStyles.Any,
                System.Globalization.NumberFormatInfo.InvariantInfo,
                out nResult);
            return bIsNumeric;
        }

        public static DateTime LocalTime(DateTime dtServerTime)
        {
            // Removed when we moved to a local server
            return dtServerTime;
            //DateTime dtLocal = dtServerTime.ToUniversalTime().AddHours(10);

            //if (
            //    (DateBetween(dtLocal, DateTime.Parse("2016-10-30"), DateTime.Parse("2017-03-26")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2015-10-25"), DateTime.Parse("2016-03-27")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2014-10-26"), DateTime.Parse("2015-03-29")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2013-10-27"), DateTime.Parse("2014-03-30")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2012-10-28"), DateTime.Parse("2013-03-31")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2011-10-30"), DateTime.Parse("2012-03-25")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2010-10-31"), DateTime.Parse("2011-03-27")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2009-10-25"), DateTime.Parse("2010-03-28")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2008-10-26"), DateTime.Parse("2009-03-29")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2007-10-28"), DateTime.Parse("2008-03-30")))
            //    || (DateBetween(dtLocal, DateTime.Parse("2006-10-29"), DateTime.Parse("2007-03-25")))
            //)
            //{
            //    dtLocal = dtLocal.AddHours(1);
            //}
            //return dtLocal;
        }

        public static Boolean DateBetween(DateTime dtNeedle, DateTime dtFrom, DateTime dtTo)
        {
            if ((dtNeedle >= dtFrom) && (dtNeedle <= dtTo))
                return true;
            else
                return false;
        }


        ////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the last time entry for a specific date
        /// </summary>
        public static string NumbersOnly(string mixedString, bool bAllowPoint)
        {
            string cCurrChar = "";
            string numberString = "";
            int nStringPos;


            for (nStringPos = 0; nStringPos < mixedString.Length; nStringPos++)
            {
                cCurrChar = mixedString.Substring(nStringPos, 1);
                if ((IsNumeric(cCurrChar) == true) || ((cCurrChar == ".") && (bAllowPoint == true)))
                {
                    numberString = numberString + cCurrChar;
                }
            }
            return numberString;
        }

        /// <summary>
        ///  Sends an HTML Email
        /// </summary>
        /// <param name="sHeading">Email heading or subject</param>
        /// <param name="sMessage">Email body - HTML format</param>
        /// <param name="sTo">Email TO address</param>
        /// <param name="sCC">Email copy to address. Null if not used</param>
        /// <param name="sBCC">Email blind (hidden) copy to. Null if not used</param>
        /// <param name="sFrom">Email from address</param>
        /// <param name="sErrorMessage">Returned error message</param>
        /// <returns></returns>
        //public static int SendEmail
        //(
        //    string sHeading,
        //    string sMessage,
        //    string sFrom,
        //    string sTo,
        //    out string sErrorMessage
        //)
        //{
        //    sErrorMessage = "";
        //    return SendEmail(sHeading, sMessage, sFrom, sTo, "", "", out sErrorMessage);
        //}

//        public static int SendEmail
//        (
//            string strEmailType,bool? bEmailCount, bool? bSMSCount,
//            string sHeading,
//            string sMessage,
//            string sFrom,
//            string sTo,
//            string sCC,
//            string sBCC, AttachmentCollection attFiles, Message theMessage,
//            out string sErrorMessage
//        )
//        {
            
//            int returnValue = 0;
//            string strPath="Send Email";
//            try
//            {

//                bool bSMS = false;

//                 if(bSMSCount!=null && (bool)bSMSCount==true)
//                 {
//                     bSMS = true;
//                 }

//                string strSMTPReplyToEmail = "";

//                if (System.Web.HttpContext.Current!=null && System.Web.HttpContext.Current.Session!=null && System.Web.HttpContext.Current.Session["ReplyTo"] != null)
//                {
//                    strSMTPReplyToEmail = System.Web.HttpContext.Current.Session["ReplyTo"].ToString();
//                }

//                int? iAccountID = null;

//                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null  && System.Web.HttpContext.Current.Session["AccountID"] != null)
//                {

//                    iAccountID = int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString());

//                    strPath=System.Web.HttpContext.Current.Request.Path;
//                }

                
//                if (theMessage != null && theMessage.AccountID!=null)
//                {
//                    iAccountID = theMessage.AccountID;
//                }
                       
                


//                string strMessageIDMail = "mail.thedatabase.net";

//                string strEmailServer = "";
//                string smtpUsername = "";
//                string smtpPassword = "";
//                string strSmtpPort = "";
//                string strEnableSSL = "";

//                //string strSMSEmailFrom = SystemData.SystemOption_ValueByKey_Account("EmailFrom", null, null);
//                //string strSMSEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
//                //string smtpSMSUsername = SystemData.SystemOption_ValueByKey_Account("EmailUsername", null, null);
//                //string smtpSMSPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);
//                //string strSMSSmtpPort = SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null);
//                //string strSMSEnableSSL = SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null);

//                Account theAccount = null;
//                if(iAccountID!=null)
//                {
//                    theAccount=SecurityManager.Account_Details((int)iAccountID);
//                }

//                if (theAccount != null && theAccount.SMTPEmail != "" && theAccount.SMTPUserName != "" && theAccount.SMTPPassword != ""
//                    && theAccount.SMTPServer != "" && theAccount.SMTPPort != "" && theAccount.SMTPSSL != "" && bSMS==false)
//                {
//                    if (sFrom == "")
//                        sFrom = theAccount.SMTPEmail;

//                    strMessageIDMail = "mail." + theAccount.SMTPEmail.Substring(theAccount.SMTPEmail.IndexOf("@")+1);
//                     smtpUsername = theAccount.SMTPUserName;
//                    smtpPassword = theAccount.SMTPPassword;
//                    strEmailServer=theAccount.SMTPServer;
//                    strSmtpPort=theAccount.SMTPPort;
//                    strEnableSSL = theAccount.SMTPSSL;

//                    if (strSMTPReplyToEmail=="")
//                     strSMTPReplyToEmail=theAccount.SMTPReplyToEmail;

                   
//                }
//                else
//                {
//                    if (sFrom == "" || bSMS)
//                        sFrom = SystemData.SystemOption_ValueByKey_Account("EmailFrom", null, null);

//                     smtpUsername = SystemData.SystemOption_ValueByKey_Account("EmailUsername", null, null);
//                    smtpPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);
//                    strEmailServer=SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
//                    strSmtpPort=SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null);
//                    strEnableSSL=SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null);

//                    if (strSMTPReplyToEmail == "")
//                        strSMTPReplyToEmail=sFrom;
//                }
//                if(strEnableSSL=="1" || strEnableSSL.ToLower()=="true")
//                {
//                    strEnableSSL = "True";
//                }

//                if (strEnableSSL == "" || strEnableSSL == "0" || strEnableSSL.ToLower() == "false")
//                {
//                    strEnableSSL = "False";
//                }



//                MailMessage oMailMessage = new MailMessage();
//                oMailMessage.From = new MailAddress(sFrom,smtpUsername);


//                string strGUID = Guid.NewGuid().ToString();

//                if (theMessage != null && theMessage.ExternalMessageKey != "")
//                    strGUID = theMessage.ExternalMessageKey;


//                oMailMessage.Headers.Add("Message-Id", String.Format("<{0}@{1}>", strGUID, strMessageIDMail));

//                string[] sToCollection = sTo.Split(';');
//                foreach (string str in sToCollection)
//                {
//                    if (!string.IsNullOrEmpty(str))
//                        oMailMessage.To.Add(str);
//                }

//                if ((sCC != null) && (sCC.Length > 0))
//                {
//                    string[] sCCCollection = sCC.Split(';');
//                    foreach (string str in sCCCollection)
//                    {
//                        if (!string.IsNullOrEmpty(str))
//                            oMailMessage.CC.Add(str);
//                    }
//                }

//                if ((sBCC != null) && (sBCC.Length > 0))
//                {
//                    string[] sBCCCollection = sBCC.Split(';');
//                    foreach (string str in sBCCCollection)
//                    {
//                        if (!string.IsNullOrEmpty(str))
//                            oMailMessage.Bcc.Add(str);
//                    }
//                }

//                oMailMessage.Subject = sHeading;

//                if (attFiles != null)
//                {
//                    foreach (Attachment item in attFiles)
//                    {
//                        oMailMessage.Attachments.Add(item);
//                    }                   

//                }
               


                
//                if(bSMSCount==null || (bool)bSMSCount==false)
//                {
//                    Content theFooterContent = SystemData.Content_Details_ByKey("AllEmailFooter", iAccountID);
//                    string strContentFooter = "";
//                    if (theFooterContent != null && iAccountID != null)
//                    {
//                        strContentFooter = theFooterContent.ContentP;
//                        //Account theAccount = SecurityManager.Account_Details((int)iAccountID);
//                        if(theAccount!=null)
//                        {
//                            strContentFooter = strContentFooter.Replace("[Account]", theAccount.AccountName);
//                            User theAccountHolder = SecurityManager.User_AccountHolder((int)theAccount.AccountID);
                            
//                            if(theAccountHolder!=null)
//                            {
//                                strContentFooter = strContentFooter.Replace("[AccountEmail]", theAccountHolder.Email);
//                            }

//                        }
//                    }
//                    sMessage = sMessage + strContentFooter;
//                }
//                oMailMessage.Body = sMessage;

//                if (theAccount!=null && theAccount.POP3Email != "")
//                    strSMTPReplyToEmail = theAccount.POP3Email;

//                if (strSMTPReplyToEmail!="")
//                    oMailMessage.ReplyToList.Add(new MailAddress(strSMTPReplyToEmail));

//                oMailMessage.IsBodyHtml = true;
//                SmtpClient smtp = new SmtpClient(strEmailServer);

//                if (((sFrom != null) && (sFrom.Length > 0)) && ((smtpPassword != null) && (smtpPassword.Length > 0)))
//                {
//                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(sFrom, smtpPassword);
//                    smtp.Credentials = credentials;
//                    smtp.Port = DBGurus.StringToInt(strSmtpPort);
//                    smtp.EnableSsl = Convert.ToBoolean(strEnableSSL);
//                }

//#if (!DEBUG)
//                smtp.Send(oMailMessage);


//                //smtp.SendCompleted += (s, e) =>
//                //{
//                //    SmtpClient callbackClient = s as SmtpClient;
//                //    callbackClient.Dispose();
//                //    //smtp.Dispose();
//                //    //oMailMessage.Dispose();
//                //};
//                //string userState = "DBG message";
//                //smtp.SendAsync(oMailMessage, userState);
//#endif


//                if (iAccountID != null)
//                {

//                    SecurityManager.Account_SMS_Email_Count((int)iAccountID,bEmailCount,bSMSCount);
//                }


//                if (iAccountID != null)
//                {
//                    if (oMailMessage.To.Count > 0)
//                    {
//                        if (theMessage == null)
//                        {
//                            if (strEmailType == "E" || strEmailType=="S")
//                            {
//                                //do nothing
//                            }
//                            else
//                            {
//                                strEmailType = "E";
//                            }
                            
//                            theMessage = new Message(null, null, null, iAccountID, DateTime.Now, strEmailType, "E",
//                           null, oMailMessage.To[0].ToString(), oMailMessage.Subject, oMailMessage.Body, null, "");
                            
//                        }
                                               

//                       if(bSMSCount!=null && (bool)bSMSCount==true)
//                       {
//                           theMessage.DeliveryMethod = "S";
//                       }
//                       theMessage.ExternalMessageKey = oMailMessage.Headers["Message-Id"].ToString();
//                       theMessage.ExternalMessageKey = theMessage.ExternalMessageKey.Replace("<", "");
//                       theMessage.ExternalMessageKey = theMessage.ExternalMessageKey.Replace(">", "");
//                        EmailManager.Message_Insert(theMessage);
//                        //EmailManager.dbg_EmailLog_Insert(theEmailLog, null, null);


//                    }
//                }

//                sErrorMessage = "";
//            }
//            catch (Exception e)
//            {
//                sErrorMessage = e.Message;
//                ErrorLog theErrorLog = new ErrorLog(null, "Email Send" + theMessage==null?"":" - " + theMessage.Subject, 
//                    e.Message, e.StackTrace, DateTime.Now, strPath);
//                SystemData.ErrorLog_Insert(theErrorLog);


//                returnValue = -1;
//            }

//            return returnValue;
//        }


       // static async System.Threading.Tasks.Task SendEmail_A
       //(
       //    MailMessage oMailMessage, SmtpClient smtp
       //)
       // {
       //     //await smtp.SendMailAsync(oMailMessage);
       //     //await smtp.Send(oMailMessage);
       //     System.Threading.Tasks.Task t = System.Threading.Tasks.Task.Run(async () =>
       //         {
       //             await smtp.SendMailAsync(oMailMessage);
       //         });
       //     t.Wait();
            
       // }

        public static int SendEmail
       (
           string strEmailType, bool? bEmailCount, bool? bSMSCount,
           string sHeading,
           string sMessage,
           string sFrom,
           string sTo,
           string sCC,
           string sBCC, AttachmentCollection attFiles, Message theMessage,
           out string sErrorMessage
       )
        {

            int returnValue = 0;
            string strPath = "Send Email";
            try
            {

                bool bSMS = false;

                if (bSMSCount != null && (bool)bSMSCount == true)
                {
                    bSMS = true;
                }

                string strSMTPReplyToEmail = "";

                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["ReplyTo"] != null)
                {
                    strSMTPReplyToEmail = System.Web.HttpContext.Current.Session["ReplyTo"].ToString();
                }

                int? iAccountID = null;

                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["AccountID"] != null)
                {

                    iAccountID = int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString());

                    strPath = System.Web.HttpContext.Current.Request.Path;
                }


                if (theMessage != null && theMessage.AccountID != null)
                {
                    iAccountID = theMessage.AccountID;
                }




                string strMessageIDMail = "mail.thedatabase.net";

                string strEmailServer = "";
                string smtpUsername = "";
                string smtpPassword = "";
                string strSmtpPort = "";
                string strEnableSSL = "";

                //string strSMSEmailFrom = SystemData.SystemOption_ValueByKey_Account("EmailFrom", null, null);
                //string strSMSEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
                //string smtpSMSUsername = SystemData.SystemOption_ValueByKey_Account("EmailUsername", null, null);
                //string smtpSMSPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);
                //string strSMSSmtpPort = SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null);
                //string strSMSEnableSSL = SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null);

                Account theAccount = null;
                if (iAccountID != null)
                {
                    theAccount = SecurityManager.Account_Details((int)iAccountID);
                }

                if (theAccount != null && theAccount.SMTPEmail != "" && theAccount.SMTPUserName != "" && theAccount.SMTPPassword != ""
                    && theAccount.SMTPServer != "" && theAccount.SMTPPort != "" && theAccount.SMTPSSL != "" && bSMS == false)
                {
                    if (sFrom == "")
                        sFrom = theAccount.SMTPEmail;

                    strMessageIDMail = "mail." + theAccount.SMTPEmail.Substring(theAccount.SMTPEmail.IndexOf("@") + 1);
                    smtpUsername = theAccount.SMTPUserName;
                    smtpPassword = theAccount.SMTPPassword;
                    strEmailServer = theAccount.SMTPServer;
                    strSmtpPort = theAccount.SMTPPort;
                    strEnableSSL = theAccount.SMTPSSL;

                    if (strSMTPReplyToEmail == "")
                        strSMTPReplyToEmail = theAccount.SMTPReplyToEmail;


                }
                else
                {
                    if (sFrom == "" || bSMS)
                        sFrom = SystemData.SystemOption_ValueByKey_Account("EmailFrom", null, null);

                    smtpUsername = SystemData.SystemOption_ValueByKey_Account("EmailUsername", null, null);
                    smtpPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);
                    strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
                    strSmtpPort = SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null);
                    strEnableSSL = SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null);

                    if (strSMTPReplyToEmail == "")
                        strSMTPReplyToEmail = sFrom;
                }
                if (strEnableSSL == "1" || strEnableSSL.ToLower() == "true")
                {
                    strEnableSSL = "True";
                }

                if (strEnableSSL == "" || strEnableSSL == "0" || strEnableSSL.ToLower() == "false")
                {
                    strEnableSSL = "False";
                }



                MailMessage oMailMessage = new MailMessage();
                oMailMessage.From = new MailAddress(sFrom, smtpUsername);


                string strGUID = Guid.NewGuid().ToString();

                if (theMessage != null && theMessage.ExternalMessageKey != "")
                    strGUID = theMessage.ExternalMessageKey;


                oMailMessage.Headers.Add("Message-Id", String.Format("<{0}@{1}>", strGUID, strMessageIDMail));

                string[] sToCollection = sTo.Split(';');
                foreach (string str in sToCollection)
                {
                    if (!string.IsNullOrEmpty(str))
                        oMailMessage.To.Add(str);
                }

                if ((sCC != null) && (sCC.Length > 0))
                {
                    string[] sCCCollection = sCC.Split(';');
                    foreach (string str in sCCCollection)
                    {
                        if (!string.IsNullOrEmpty(str))
                            oMailMessage.CC.Add(str);
                    }
                }

                if ((sBCC != null) && (sBCC.Length > 0))
                {
                    string[] sBCCCollection = sBCC.Split(';');
                    foreach (string str in sBCCCollection)
                    {
                        if (!string.IsNullOrEmpty(str))
                            oMailMessage.Bcc.Add(str);
                    }
                }

                oMailMessage.Subject = sHeading;

                if (attFiles != null)
                {
                    foreach (Attachment item in attFiles)
                    {
                        oMailMessage.Attachments.Add(item);
                    }

                }




                if (bSMSCount == null || (bool)bSMSCount == false)
                {
                    Content theFooterContent = SystemData.Content_Details_ByKey("AllEmailFooter", iAccountID);
                    string strContentFooter = "";
                    if (theFooterContent != null && iAccountID != null)
                    {
                        strContentFooter = theFooterContent.ContentP;
                        //Account theAccount = SecurityManager.Account_Details((int)iAccountID);
                        if (theAccount != null)
                        {
                            strContentFooter = strContentFooter.Replace("[Account]", theAccount.AccountName);
                            User theAccountHolder = SecurityManager.User_AccountHolder((int)theAccount.AccountID);

                            if (theAccountHolder != null)
                            {
                                strContentFooter = strContentFooter.Replace("[AccountEmail]", theAccountHolder.Email);
                            }

                        }
                    }
                    sMessage = sMessage + strContentFooter;
                }
                oMailMessage.Body = sMessage;

                if (theAccount != null && theAccount.POP3Email != "")
                    strSMTPReplyToEmail = theAccount.POP3Email;

                if (strSMTPReplyToEmail != "")
                    oMailMessage.ReplyToList.Add(new MailAddress(strSMTPReplyToEmail));

                oMailMessage.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(strEmailServer);

                if (((sFrom != null) && (sFrom.Length > 0)) && ((smtpPassword != null) && (smtpPassword.Length > 0)))
                {
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(sFrom, smtpPassword);
                    smtp.Credentials = credentials;
                    smtp.Port = DBGurus.StringToInt(strSmtpPort);
                    smtp.EnableSsl = Convert.ToBoolean(strEnableSSL);
                }


                if (iAccountID != null)
                {

                    SecurityManager.Account_SMS_Email_Count((int)iAccountID, bEmailCount, bSMSCount);
                }


                if (iAccountID != null)
                {
                    if (oMailMessage.To.Count > 0)
                    {
                        if (theMessage == null)
                        {
                            if (strEmailType == "E" || strEmailType == "S")
                            {
                                //do nothing
                            }
                            else
                            {
                                strEmailType = "E";
                            }

                            theMessage = new Message(null, null, null, iAccountID, DateTime.Now, strEmailType, "E",
                           null, oMailMessage.To[0].ToString(), oMailMessage.Subject, oMailMessage.Body, null, "");

                        }


                        if (bSMSCount != null && (bool)bSMSCount == true)
                        {
                            theMessage.DeliveryMethod = "S";
                        }


                        theMessage.ExternalMessageKey = oMailMessage.Headers["Message-Id"].ToString();
                        theMessage.ExternalMessageKey = theMessage.ExternalMessageKey.Replace("<", "");
                        theMessage.ExternalMessageKey = theMessage.ExternalMessageKey.Replace(">", "");
                        EmailManager.Message_Insert(theMessage);

                    }
                }
              
                //EmailCallBack theEmailCallBack = new EmailCallBack();
                //theEmailCallBack.TheMail = oMailMessage;
                //theEmailCallBack.TheMessage = theMessage;
                //Object state = oMailMessage;
                //event handler for asynchronous call
                //smtp.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);

//#if (!DEBUG)

                //if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
                //{
                System.Threading.Tasks.Task t = System.Threading.Tasks.Task.Run(async () =>
                {
                    await smtp.SendMailAsync(oMailMessage);
                 
                });
               // t.Wait();
                ////}
                //else
                //{

                //smtp.Send(oMailMessage);
                //}

               

                 //SendEmail_A(oMailMessage, smtp);
                //smtp.Send(oMailMessage);
                //smtp.SendAsync(oMailMessage, null);
                                
//#endif


                

                sErrorMessage = "";
            }
            catch (Exception e)
            {
                sErrorMessage = e.Message;
                ErrorLog theErrorLog = new ErrorLog(null, "Email Send" + theMessage == null ? "" : " - " + theMessage.Subject,
                    e.Message, e.StackTrace, DateTime.Now, strPath);
                SystemData.ErrorLog_Insert(theErrorLog);


                returnValue = -1;
            }

            return returnValue;
        }
       //static void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
       // {
       //     //MailMessage oMailMessage = e.UserState as MailMessage;
       //     //oMailMessage.Dispose();
       //     if (!e.Cancelled && e.Error != null)
       //     {
       //        // message.Text = "Mail sent successfully";
               
       //     }
           
       // }
        public static int SendEmail_AttachFie
       (
           string sHeading,
           string sMessage,
           string sFrom,
           string sTo,
           string sCC,
           string sBCC,
           AttachmentCollection attFiles,
           out string sErrorMessage
       )
        {
            sErrorMessage = "";
            int returnValue = 0;
            try
            {
                MailMessage oMailMessage = new MailMessage();
                oMailMessage.From = new MailAddress(sFrom);

                string[] sToCollection = sTo.Split(';');
                foreach (string str in sToCollection)
                {
                    oMailMessage.To.Add(str);
                }

                if ((sCC != null) && (sCC.Length > 0))
                {
                    string[] sCCCollection = sCC.Split(';');
                    foreach (string str in sCCCollection)
                    {
                        oMailMessage.CC.Add(str);
                    }
                }

                if ((sBCC != null) && (sBCC.Length > 0))
                {
                    string[] sBCCCollection = sBCC.Split(';');
                    foreach (string str in sBCCCollection)
                    {
                        oMailMessage.Bcc.Add(str);
                    }
                }

                oMailMessage.Subject = sHeading;
                foreach (Attachment item in attFiles)
                {
                    oMailMessage.Attachments.Add(item);
                }
                oMailMessage.Body = sMessage;
                oMailMessage.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null));
                string smtpUsername = SystemData.SystemOption_ValueByKey_Account("EmailUsername", null, null);
                string smtpPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);
                if (((smtpUsername != null) && (smtpUsername.Length > 0)) && ((smtpPassword != null) && (smtpPassword.Length > 0)))
                {
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                    smtp.Credentials = credentials;
                    smtp.Port = DBGurus.StringToInt(SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null));
                    smtp.EnableSsl = Convert.ToBoolean(SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null));
                }
                smtp.Send(oMailMessage);
            }
            catch (Exception e)
            {
                sErrorMessage = e.Message;
                returnValue = -1;
            }

            return returnValue;
        }


        public static decimal StringToDecimal(string sIn)
        {
            if (IsNumeric(sIn))
                return decimal.Parse(sIn);
            else
                return 0;
        }

        public static double StringToDouble(string sIn)
        {
            if (IsNumeric(sIn))
                return double.Parse(sIn);
            else
                return 0;
        }

        public static int StringToInt(string sIn)
        {
            if ((sIn.Length > 0) && (IsNumeric(sIn)))
                return int.Parse(sIn);
            else
                return 0;
        }


        /// <summary>
        /// Convers a string into a date. sFormat should be in the format 'MDY' or 'DMY' 
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="sFormat"></param>
        /// <returns></returns>
        public static DateTime StringToDate(string sDate, string sFormat)
        {
            DateTime returnValue = DateTime.MinValue;
            if ((sDate == null) || (sDate.Length < 6))
                return returnValue;

            sDate = sDate.Replace("/", " ");
            sDate = sDate.Replace("-", " ");
            sDate = sDate.Replace(".", " ");
            sDate = sDate.Replace("\\", " ");
            int nYears;

            DateTime dt = DateTime.MinValue;

            if (sFormat.StartsWith("D"))
            {
                if ((IsNumeric(sDate.Substring(0, 2).Trim()))
                    && (IsNumeric(sDate.Substring(sDate.IndexOf(" "), 2).Trim()))
                        && (IsNumeric(sDate.Substring(sDate.LastIndexOf(" "), 2).Trim())))
                {
                    int nDays = Int32.Parse(sDate.Substring(0, 2).Trim());
                    int nMonths = Int32.Parse(sDate.Substring(sDate.IndexOf(" ") + 1, 2).Trim());
                    if (sDate.Length > 8)
                        nYears = Int32.Parse(sDate.Substring(sDate.LastIndexOf(" ") + 1, 4).Trim());
                    else
                        nYears = 2000 + Int32.Parse(sDate.Substring(sDate.LastIndexOf(" ") + 1, 2).Trim());
                    returnValue = new DateTime(nYears, nMonths, nDays);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// URLBuild adds a new parameter on to an existing URL
        /// </summary>
        /// <param name="sStartingURL"></param>
        /// <param name="sNewBit"></param>
        /// <param name="bEncodeNewBit"></param>
        /// <returns></returns>
        public static string URLBuild(string sStartingURL, string sNewBit)
        {
            string returnValue = sStartingURL;
            if (sStartingURL.Contains("?"))
                returnValue = String.Concat(returnValue, "&", sNewBit);
            else
                returnValue = String.Concat(returnValue, "?", sNewBit);
            return returnValue;
        }

        /// <summary>
        /// Helper method encodes a string correctly for an HTTP POST
        /// </summary>
        /// <param name="oldValue"></param>
        /// <returns></returns>
        public static string UrlEncode(string oldValue)
        {
            string newValue = oldValue.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue);
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        /// <summary>
        /// Override to make the url unrecognisable 
        /// </summary>
        /// <param name="oldValue"></param>
        /// /// <param name="bRemoveEqualsSigns">Pass true to make the URL parameters unitelligable to the receiving page</param>
        /// <returns></returns>
        /// 
        public static string UrlEncode(string oldValue, Boolean bReplaceEqualsSigns)
        {
            string newValue = oldValue.Replace("\"", "'");
            if (bReplaceEqualsSigns)
                newValue = oldValue.Replace("=", "~equals~");
            newValue = System.Web.HttpUtility.UrlEncode(newValue);
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        public static string UrlDecode(string oldValue, Boolean bReplaceEqualsSigns)
        {
            string newValue = oldValue;
            if (bReplaceEqualsSigns)
                newValue = oldValue.Replace("~equals~", "=");
            return System.Web.HttpUtility.HtmlDecode(newValue);
        }


        public static Boolean UserHasRole(string sUserName, int nRoleID)
        {
            string sResult = "";
            return UserHasRole(sUserName, nRoleID, out sResult);
        }
        /// <summary>
        /// Checks to see the specified user has the requested Role. Returns true if no error and they do.
        /// Returns false if not, or an error occurs (then the error message will be in sError)
        /// </summary>
        /// <param name="sUserName"></param>
        /// <param name="sRole"></param>
        /// <param name="sError"></param>
        /// <returns></returns>
        public static Boolean UserHasRole(string sUserName, int nRoleID, out string sResult)
        {
            Boolean returnValue = false;
            sResult = "";

            //if ((sUserName.ToUpper() == "ADMINISTRATOR") || (sUserName.ToUpper() == "ADMIN"))
            //    returnValue = true;
            //else
            //{
            DataSet ds = new DataSet();
            string sSQL = String.Concat("SELECT * FROM ", "[User] U",
                " INNER JOIN ", "[UserRole] UR", " ON UR.UserID = U.UserID ",
                " WHERE U.UserName = '", sUserName, "'",
                " AND UR.RoleID = ", nRoleID.ToString());

            if (ExecuteSQLDataSet(ds, sSQL, out sResult) == 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    returnValue = true;
                else
                    sResult = "Sorry, but this user account does not have the required security Role for this action";
            }
            //        }
            return returnValue;
        }

        public static string ExtractKey(string sKey)
        {
            if (sKey != null)
            {
                if (sKey.Contains("<v>"))
                {
                    int nStartPos = sKey.IndexOf("<v>") + 3;
                    int nEndPos = sKey.IndexOf("</v>");
                    int nLen = nEndPos - nStartPos;
                    return sKey.Substring(nStartPos, nLen);
                }
            }
            return sKey;
        }

        public static string GetContentWithValues(string sContentKey, string sKeyForSP, out int nContentID, out string sError)
        {
            string sContent = "";
            nContentID = -1;
            DataSet dsContent = new DataSet();
            if (DBGurus.ExecuteSQLDataSet(dsContent, String.Concat("SELECT * FROM Content WHERE ContentKey = '", sContentKey, "'"), out sError) == 0)
            {
                if (dsContent.Tables[0].Rows.Count > 0)
                {
                    nContentID = (int)dsContent.Tables[0].Rows[0]["ContentID"];
                    sContent = ReplaceContentValues(dsContent.Tables[0].Rows[0]["Content"].ToString(),
                        dsContent.Tables[0].Rows[0]["StoredProcedure"].ToString(),
                        sKeyForSP, out sError);
                }
                else
                    sError = "That ContentKey could not be located";
            }
            return sContent;
        }

        public static string GetContentHeadingWithValues(string sContentKey, string sKeyForSP, out int nContentID, out string sError)
        {
            string sContent = "";
            nContentID = -1;
            DataSet dsContent = new DataSet();
            if (DBGurus.ExecuteSQLDataSet(dsContent, String.Concat("SELECT * FROM Content WHERE ContentKey = '", sContentKey, "'"), out sError) == 0)
            {
                if (dsContent.Tables[0].Rows.Count > 0)
                {
                    nContentID = (int)dsContent.Tables[0].Rows[0]["ContentID"];
                    sContent = ReplaceContentValues(dsContent.Tables[0].Rows[0]["Heading"].ToString(),
                        dsContent.Tables[0].Rows[0]["StoredProcedure"].ToString(),
                        sKeyForSP, out sError);
                }
                else
                    sError = "That ContentKey could not be located";
            }
            return sContent;
        }

    }
