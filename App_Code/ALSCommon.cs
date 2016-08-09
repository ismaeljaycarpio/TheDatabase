using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ALSCommon
/// </summary>
public class ALSCommon
{
    public static void ALSNotificationMessage(Batch theBatch, List<int> children)
    {
        ALSNotificationMessage(theBatch, children,
                null, null, null, null, null, null);
    }

    public static void ALSNotificationMessage(Batch theBatch, List<int> children,
        string uploadType, string site, string sampleType, string samplePoints, DateTime? startDate, DateTime? endDate)
    {
        Content theContentEmail = null;
        Content theContentSMS = null;

        if (String.IsNullOrEmpty(uploadType))
        {
            theContentEmail = SystemData.Content_Details_ByKey("ALSDataUploadEmailHeader", (int)theBatch.AccountID);
            theContentSMS = SystemData.Content_Details_ByKey("ALSDataUploadSMSHeader", (int)theBatch.AccountID);
        }
        else
        {
            theContentEmail = SystemData.Content_Details_ByKey("ALSDataUploadEmailHeaderEx", (int)theBatch.AccountID);
            theContentSMS = SystemData.Content_Details_ByKey("ALSDataUploadSMSHeaderEx", (int)theBatch.AccountID);
        }

        string strBody = "";
        string strBodySMS = "";

        strBody = theContentEmail.ContentP;
        strBodySMS = theContentSMS.ContentP;

        if (String.IsNullOrEmpty(uploadType))
        {
            strBody = strBody.Replace("[FileName]", theBatch.UploadedFileName);
            strBodySMS = strBodySMS.Replace("[FileName]", theBatch.UploadedFileName);
        }
        else
        {
            strBody = strBody.Replace("[Data_Type]", uploadType);
            strBody = strBody.Replace("[Site]", site);
            strBody = strBody.Replace("[Sample_Type]", sampleType);
            strBody = strBody.Replace("[Sample_Points]", samplePoints);
            strBody = strBody.Replace("[From_Date]", startDate.Value.ToString("dd/MM/yyyy HH:mm"));
            strBody = strBody.Replace("[To_Date]", endDate.Value.ToString("dd/MM/yyyy HH:mm"));
            strBodySMS = strBodySMS.Replace("[Date_Type]", uploadType);
            strBodySMS = strBodySMS.Replace("[Site]", site);
            strBodySMS = strBodySMS.Replace("[Sample_Type]", sampleType);
            strBodySMS = strBodySMS.Replace("[Sample_Points]", samplePoints);
            strBodySMS = strBodySMS.Replace("[From_Date]", startDate.Value.ToString("dd/MM/yyyy HH:mm"));
            strBodySMS = strBodySMS.Replace("[To_Date]", endDate.Value.ToString("dd/MM/yyyy HH:mm"));
        }

        bool isAnyALSError = false;

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.AddWithValue("@TableID", theBatch.TableID);
        cmd.Parameters.AddWithValue("@BatchID", theBatch.BatchID);
        SqlParameter parameter1 = new SqlParameter();
        parameter1.ParameterName = "@TotalRecordsCount";
        parameter1.SqlDbType = SqlDbType.Int;
        parameter1.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(parameter1);
        SqlParameter parameter2 = new SqlParameter();
        parameter2.ParameterName = "@ErrorsCount";
        parameter2.SqlDbType = SqlDbType.Int;
        parameter2.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(parameter2);

        string error = "";
        if (DBGurus.ExecuteSPDataset(ds, "dbg_Import_Mapping_Results", cmd, out error) == 0)
        {
            strBody = strBody.Replace("[Total_Records]", cmd.Parameters["@TotalRecordsCount"].Value.ToString());
            strBodySMS = strBodySMS.Replace("[Total_Records]", cmd.Parameters["@TotalRecordsCount"].Value.ToString());

            strBody = strBody.Replace("[Mapping_Errors]", cmd.Parameters["@ErrorsCount"].Value.ToString());
            strBodySMS = strBodySMS.Replace("[Mapping_Errors]", cmd.Parameters["@ErrorsCount"].Value.ToString());

            if ((ds.Tables.Count > 1) && (ds.Tables[1].Rows.Count > 0))
            {
                string s = "<h3>Mapping Errors</h3>" + Environment.NewLine;
                s = s + "<table border=\"1\">" + Environment.NewLine;

                s = s + "<tr>" + Environment.NewLine;
                foreach (DataColumn column in ds.Tables[1].Columns)
                    s = s + String.Format("<th>{0}</th>", column.ColumnName) + Environment.NewLine;
                s = s + "</tr>" + Environment.NewLine;

                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    s = s + "<tr>" + Environment.NewLine;
                    foreach (DataColumn column in ds.Tables[1].Columns)
                        if (row.IsNull(column))
                            s = s + "<td>&nbsp;</td>" + Environment.NewLine;
                        else
                            s = s + String.Format("<td>{0}</td>", row[column]) + Environment.NewLine;
                    s = s + "</tr>" + Environment.NewLine;
                }

                s = s + "</table>" + Environment.NewLine;
                strBody = strBody.Replace("[Mapping_Errors_Table]", s);
                strBodySMS = strBodySMS.Replace("[Mapping_Errors_Table]", s);

                isAnyALSError = true;
            }
            else
            {
                strBody = strBody.Replace("[Mapping_Errors_Table]", "");
                strBodySMS = strBodySMS.Replace("[Mapping_Errors_Table]", "");
            }
        }

        string strBodySections = "";
        string strBodySMSSections = "";
        foreach (int batchID in children)
        {
            Content theContentEmailSection = SystemData.Content_Details_ByKey("ALSDataUploadEmailSection", (int)theBatch.AccountID);
            Content theContentSMSSection = SystemData.Content_Details_ByKey("ALSDataUploadSMSSection", (int)theBatch.AccountID);

            string strBodySection = "";
            string strBodySMSSection = "";

            strBodySection = theContentEmailSection.ContentP;
            strBodySMSSection = theContentSMSSection.ContentP;

            Batch oMapBatch = UploadManager.ets_Batch_Details(batchID);
            Table theTable = RecordManager.ets_Table_Details(oMapBatch.TableID.Value);
            Account theAccount = SecurityManager.Account_Details(theTable.AccountID.Value);

            strBodySection = strBodySection.Replace("[Account]", theAccount.AccountName);
            strBodySMSSection = strBodySMSSection.Replace("[Account]", theAccount.AccountName);
            strBodySection = strBodySection.Replace("[Table]", theTable.TableName);
            strBodySMSSection = strBodySMS.Replace("[Table]", theTable.TableName);

            DataTable dtCSVRecordsSection = Common.DataTableFromText(@"Select COUNT(*)
                        FROM TempRecord WHERE BatchID =" + batchID.ToString());
            strBodySection = strBodySection.Replace("[Total_Records]", dtCSVRecordsSection.Rows[0][0].ToString());
            strBodySMSSection = strBodySMSSection.Replace("[Total_Records]", dtCSVRecordsSection.Rows[0][0].ToString());

            DataTable dtValidRecordsSection = Common.DataTableFromText(@"Select COUNT(*)
                        FROM TempRecord WHERE BatchID =" + batchID.ToString() + " AND RejectReason IS NULL");
            strBodySection = strBodySection.Replace("[Valid_Records]", dtValidRecordsSection.Rows[0][0].ToString());
            strBodySMSSection = strBodySMSSection.Replace("[Valid_Records]", dtValidRecordsSection.Rows[0][0].ToString());

            DataTable dtInvalidRecordsSection = Common.DataTableFromText(@"Select COUNT(*)
                        FROM TempRecord WHERE BatchID =" + batchID.ToString() + " AND RejectReason IS NOT NULL");
            strBodySection = strBodySection.Replace("[Invalid_Records]", dtInvalidRecordsSection.Rows[0][0].ToString());
            strBodySMSSection = strBodySMSSection.Replace("[Invalid_Records]", dtInvalidRecordsSection.Rows[0][0].ToString());

            DataTable dtImportedRecordsSection = Common.DataTableFromText(@"Select COUNT(*)
                        FROM Record WHERE BatchID =" + batchID.ToString());
            strBodySection = strBodySection.Replace("[Records_Imported]", dtImportedRecordsSection.Rows[0][0].ToString());
            strBodySMSSection = strBodySMSSection.Replace("[Records_Imported]", dtImportedRecordsSection.Rows[0][0].ToString());

            if ((dtInvalidRecordsSection.Rows.Count > 0) &&
                (!dtInvalidRecordsSection.Rows[0].IsNull(0)) &&
                ((int)dtInvalidRecordsSection.Rows[0][0] > 0))
            {
                DataTable dtRejectReasonSection = Common.DataTableFromText(@"SELECT RejectReason, COUNT(RecordID)
                            FROM TempRecord WHERE BatchID =" + batchID.ToString() + @" AND RejectReason IS NOT NULL
                            GROUP BY RejectReason");
                string s = "<h3>Validation Errors</h3>" + Environment.NewLine;
                s = s + "<table border=\"1\">" + Environment.NewLine;
                foreach (DataRow row in dtRejectReasonSection.Rows)
                    s = s + String.Format("<tr><td>{0}</td><td>{1}</td></tr>", row[0], row[1]) + Environment.NewLine;
                s = s + "</table>" + Environment.NewLine;
                strBodySection = strBodySection.Replace("[Validation_Errors_Table]", s);
                strBodySMSSection = strBodySMSSection.Replace("[Validation_Errors_Table]", s);
            }
            else
            {
                strBodySection = strBodySection.Replace("[Validation_Errors_Table]", "");
                strBodySMSSection = strBodySMSSection.Replace("[Validation_Errors_Table]", "");
            }

            strBodySections = strBodySections + strBodySection;
            strBodySMSSections = strBodySMSSections + strBodySMSSection;
        }

        strBody = strBody.Replace("[Message_Sections]", strBodySections);
        strBodySMS = strBodySMS.Replace("[Message_Sections]", strBodySMSSections);


        Table theALSTable = RecordManager.ets_Table_Details((int)theBatch.TableID);
        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
            (int)theALSTable.TableID, null, null, null, true, null, null, null, null, null, null, null);

        string strSubject = theContentEmail.Heading;
        string strTempBody = strBody;

        foreach (DataRow dr in dtUsersEmail.Rows)
        {
            strTempBody = strBody;
            strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
            try
            {
                string sSendEmailError = "";
                Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
                    DateTime.Now, "E", "E",
                    null, dr["Email"].ToString(), strSubject, strTempBody, null, "");
                DBGurus.SendEmail("Batch", true, null, strSubject, strTempBody, "", dr["Email"].ToString(),
                    "", "", null, theMessage, out sSendEmailError);
            }
            catch (Exception ex)
            {
                //strErrorMsg = "Server could not send warning Email & SMS";
            }
        }

        strSubject = theContentSMS.Heading;

        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail", null, theBatch.TableID);
        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
            (int)theALSTable.TableID, null, null, null, null, true, null, null, null, null, null, null);

        foreach (DataRow dr in dtUsersSMS.Rows)
        {
            if (dr["PhoneNumber"] != DBNull.Value)
            {
                if (dr["PhoneNumber"].ToString() != "")
                {
                    strTempBody = strBodySMS;
                    strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
                    try
                    {
                        string sSendEmailError = "";

                        Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
                            DateTime.Now, "E", "S",
                            null, dr["PhoneNumber"].ToString() + strWarningSMSEMail, strSubject, strTempBody, null, "");

                        DBGurus.SendEmail("Batch SMS", null, true, strSubject, strTempBody, "",
                            dr["PhoneNumber"].ToString() + strWarningSMSEMail, "", "", null, theMessage, out sSendEmailError);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog theErrorLog = new ErrorLog(null, "SMS Email", ex.Message, ex.StackTrace, DateTime.Now, "");
                        SystemData.ErrorLog_Insert(theErrorLog);
                    }
                }
            }
        }

        if (isAnyALSError)
        {
            Content theALSContentEmail = null;
            if (String.IsNullOrEmpty(uploadType))
            {
                theALSContentEmail = SystemData.Content_Details_ByKey("ALSDataErrorEmail", (int)theBatch.AccountID);
            }
            else
            {
                theALSContentEmail = SystemData.Content_Details_ByKey("ALSDataErrorEmailEx", (int)theBatch.AccountID);
            }

            string strALSBody = theALSContentEmail.ContentP;
            string strALSSubject = theALSContentEmail.Heading;
            if (String.IsNullOrEmpty(uploadType))
            {
                strALSBody = strALSBody.Replace("[FileName]", theBatch.UploadedFileName);
            }
            else
            {
                strALSBody = strALSBody.Replace("[Data_Type]", uploadType);
                strALSBody = strALSBody.Replace("[Site]", site);
                strALSBody = strALSBody.Replace("[Sample_Type]", sampleType);
                strALSBody = strALSBody.Replace("[Sample_Points]", samplePoints);
                strALSBody = strALSBody.Replace("[From_Date]", startDate.Value.ToString("dd/MM/yyyy HH:mm"));
                strALSBody = strALSBody.Replace("[To_Date]", endDate.Value.ToString("dd/MM/yyyy HH:mm"));
            }

            DataSet ds2 = new DataSet();
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Parameters.AddWithValue("@nBatchID", theBatch.BatchID);
            if (DBGurus.ExecuteSPDataset(ds2, "dbg_Get_ALS_Mapped_Accounts", cmd2, out error) == 0)
            {
                if ((ds2.Tables.Count > 0) && (ds2.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow accountRow in ds2.Tables[0].Rows)
                    {
                        string sourceSite = accountRow[0].ToString();
                        string tempBody = strALSBody;

                        DataSet ds4 = new DataSet();
                        SqlCommand cmd4 = new SqlCommand();
                        cmd4.Parameters.AddWithValue("@TableID", theBatch.TableID);
                        cmd4.Parameters.AddWithValue("@BatchID", theBatch.BatchID);
                        cmd4.Parameters.AddWithValue("@Site", sourceSite);
                        SqlParameter parameter41 = new SqlParameter();
                        parameter41.ParameterName = "@TotalRecordsCount";
                        parameter41.SqlDbType = SqlDbType.Int;
                        parameter41.Direction = ParameterDirection.Output;
                        cmd4.Parameters.Add(parameter41);
                        SqlParameter parameter42 = new SqlParameter();
                        parameter42.ParameterName = "@ErrorsCount";
                        parameter42.SqlDbType = SqlDbType.Int;
                        parameter42.Direction = ParameterDirection.Output;
                        cmd4.Parameters.Add(parameter42);

                        if (DBGurus.ExecuteSPDataset(ds4, "dbg_Import_Mapping_Results", cmd4, out error) == 0)
                        {
                            tempBody = tempBody.Replace("[Total_Records]", cmd4.Parameters["@TotalRecordsCount"].Value.ToString());
                            tempBody = tempBody.Replace("[Mapping_Errors]", cmd4.Parameters["@ErrorsCount"].Value.ToString());

                            if ((ds4.Tables.Count > 1) && (ds4.Tables[1].Rows.Count > 0))
                            {
                                string s = "<h3>Mapping Errors</h3>" + Environment.NewLine;
                                s = s + "<table border=\"1\">" + Environment.NewLine;

                                s = s + "<tr>" + Environment.NewLine;
                                foreach (DataColumn column in ds4.Tables[1].Columns)
                                    s = s + String.Format("<th>{0}</th>", column.ColumnName) + Environment.NewLine;
                                s = s + "</tr>" + Environment.NewLine;

                                foreach (DataRow row in ds4.Tables[1].Rows)
                                {
                                    s = s + "<tr>" + Environment.NewLine;
                                    foreach (DataColumn column in ds4.Tables[1].Columns)
                                        if (row.IsNull(column))
                                            s = s + "<td>&nbsp;</td>" + Environment.NewLine;
                                        else
                                            s = s + String.Format("<td>{0}</td>", row[column]) + Environment.NewLine;
                                    s = s + "</tr>" + Environment.NewLine;
                                }

                                s = s + "</table>" + Environment.NewLine;
                                tempBody = tempBody.Replace("[Mapping_Errors_Table]", s);
                            }
                            else
                            {
                                tempBody = tempBody.Replace("[Mapping_Errors_Table]", "");
                            }
                        }

                        DataSet ds3 = new DataSet();
                        SqlCommand cmd3 = new SqlCommand();
                        cmd3.Parameters.AddWithValue("@Site", sourceSite);
                        if (DBGurus.ExecuteSPDataset(ds3, "dbg_Get_EMD_Site_Record", cmd3, out error) == 0)
                        {
                            if ((ds3.Tables.Count > 0) && (ds3.Tables[0].Rows.Count > 0))
                            {
                                string val = ds3.Tables[0].Rows[0]["Send ALS Issues To"].ToString();
                                string[] addresses = val.Split(new char[] {',', ';'});
                                foreach (string address in addresses)
                                {
                                    try
                                    {
                                        string sSendEmailError = "";
                                        Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
                                            DateTime.Now, "E", "E",
                                            null, address, strALSSubject, tempBody, null, "");
                                        DBGurus.SendEmail("Batch", true, null, strALSSubject, tempBody, "", address,
                                            "", "", null, theMessage, out sSendEmailError);
                                    }
                                    catch (Exception ex)
                                    {
                                        //strErrorMsg = "Server could not send warning Email & SMS";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}