using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using ChartDirector;
using System.Net.Mail;
using System.CodeDom.Compiler;

/// <summary>
/// Summary description for ScheduleManager
/// </summary>
public class ScheduleManager
{
	public ScheduleManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //public static int ets_MonitorSchedule_Insert(MonitorSchedule p_MonitorSchedule)
    //{
        
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {

    //        using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_Insert", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //            pRV.Direction = ParameterDirection.Output;

    //            command.Parameters.Add(pRV);
    //            command.Parameters.Add(new SqlParameter("@nAccountID", p_MonitorSchedule.AccountID));

    //            if (p_MonitorSchedule.TableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableID", p_MonitorSchedule.TableID));

    //            //if (p_MonitorSchedule.LocationID != null)
    //            //command.Parameters.Add(new SqlParameter("@nLocationID", p_MonitorSchedule.LocationID));

    //            command.Parameters.Add(new SqlParameter("@dScheduleDateTime", p_MonitorSchedule.ScheduleDateTime));

    //            command.Parameters.Add(new SqlParameter("@sDescription", p_MonitorSchedule.Description));
    //            command.Parameters.Add(new SqlParameter("@bHasAlarm", p_MonitorSchedule.HasAlarm));
    //            command.Parameters.Add(new SqlParameter("@dAlarmDateTime", p_MonitorSchedule.AlarmDateTime));
    //            command.Parameters.Add(new SqlParameter("@nUserAdded", p_MonitorSchedule.UserAdded));
    //            command.Parameters.Add(new SqlParameter("@nInitialScheduleID", p_MonitorSchedule.InitialScheduleID));


    //            connection.Open();
    //            try
    //            {
    //                command.ExecuteNonQuery();
    //                connection.Close();
    //                connection.Dispose();
    //                return int.Parse(pRV.Value.ToString());
    //            }
    //            catch
    //            {
    //                connection.Close();
    //                connection.Dispose();

    //            }
    //            return -1;
    //        }
      
    //    }



    //}


    //public static int ets_MonitorSchedule_Update(MonitorSchedule p_MonitorSchedule)
    //{


    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_Update", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nMonitorScheduleID", p_MonitorSchedule.MonitorScheduleID));


    //            command.Parameters.Add(new SqlParameter("@nAccountID", p_MonitorSchedule.AccountID));

    //            if (p_MonitorSchedule.TableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableID", p_MonitorSchedule.TableID));

    //            //if (p_MonitorSchedule.LocationID != null)
    //            //    command.Parameters.Add(new SqlParameter("@nLocationID", p_MonitorSchedule.LocationID));

    //            command.Parameters.Add(new SqlParameter("@dScheduleDateTime", p_MonitorSchedule.ScheduleDateTime));

    //            command.Parameters.Add(new SqlParameter("@sDescription", p_MonitorSchedule.Description));
    //            command.Parameters.Add(new SqlParameter("@bHasAlarm", p_MonitorSchedule.HasAlarm));
    //            command.Parameters.Add(new SqlParameter("@dAlarmDateTime", p_MonitorSchedule.AlarmDateTime));
    //            command.Parameters.Add(new SqlParameter("@nUserUpdated", p_MonitorSchedule.UserUpdated));
    //            command.Parameters.Add(new SqlParameter("@nInitialScheduleID", p_MonitorSchedule.InitialScheduleID));
    //            int i = 1;
    //            connection.Open();
    //            try
    //            {
    //                command.ExecuteNonQuery();
    //            }
    //            catch
    //            {
    //                i = -1;
    //            }

    //            connection.Close();
    //            connection.Dispose();

    //            return i;


    //        }
    //    }



        
           
        
    //}


    //public static MonitorSchedule ets_MonitorSchedule_Delail(int nMonitorScheduleID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_Delail", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

                
    //            command.Parameters.Add(new SqlParameter("@nMonitorScheduleID", nMonitorScheduleID));

    //            connection.Open();

    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    MonitorSchedule temp = new MonitorSchedule(
    //                       (int)reader["MonitorScheduleID"],
    //                       (int)reader["AccountID"],
    //                       reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
    //                        //reader["LocationID"] == DBNull.Value ? null : (int?)reader["LocationID"],
    //                       (DateTime)reader["ScheduleDateTime"],
    //                       (string)reader["Description"], (bool)reader["HasAlarm"],
    //                        reader["AlarmDateTime"] == DBNull.Value ? null : (DateTime?)reader["AlarmDateTime"],
    //                         reader["DateAdded"] == DBNull.Value ? null : (DateTime?)reader["DateAdded"],
    //                          reader["DateUpdated"] == DBNull.Value ? null : (DateTime?)reader["DateUpdated"],
    //                           reader["UserAdded"] == DBNull.Value ? null : (int?)reader["UserAdded"],
    //                            reader["UserUpdated"] == DBNull.Value ? null : (int?)reader["UserUpdated"],
    //                             reader["InitialScheduleID"] == DBNull.Value ? null : (int?)reader["InitialScheduleID"]
    //                        );

    //                    connection.Close();
    //                    connection.Dispose();
    //                    return temp;
    //                }

    //            }
    //            connection.Close();
    //            connection.Dispose();
    //            return null;

    //        }
    //    }
    //}

    //public static MonitorSchedule ets_MonitorSchedule_Delail(int nMonitorScheduleID)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_Delail", connection, tn))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;


    //        command.Parameters.Add(new SqlParameter("@nMonitorScheduleID", nMonitorScheduleID));

    //        //connection.Open();

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                MonitorSchedule temp = new MonitorSchedule(
    //                   (int)reader["MonitorScheduleID"],
    //                   (int)reader["AccountID"],
    //                   reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
    //                    //reader["LocationID"] == DBNull.Value ? null : (int?)reader["LocationID"],
    //                   (DateTime)reader["ScheduleDateTime"],
    //                   (string)reader["Description"], (bool)reader["HasAlarm"],
    //                    reader["AlarmDateTime"] == DBNull.Value ? null : (DateTime?)reader["AlarmDateTime"],
    //                     reader["DateAdded"] == DBNull.Value ? null : (DateTime?)reader["DateAdded"],
    //                      reader["DateUpdated"] == DBNull.Value ? null : (DateTime?)reader["DateUpdated"],
    //                       reader["UserAdded"] == DBNull.Value ? null : (int?)reader["UserAdded"],
    //                        reader["UserUpdated"] == DBNull.Value ? null : (int?)reader["UserUpdated"],
    //                         reader["InitialScheduleID"] == DBNull.Value ? null : (int?)reader["InitialScheduleID"]
    //                    );

                    
    //                return temp;
    //            }

    //        }
    //        return null;

    //    }

    //}

    //public static int ets_MonitorSchedule_Delete(int nMonitorScheduleID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_Delete", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nMonitorScheduleID", nMonitorScheduleID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
       
    //}

    //public static int ets_MonitorSchedule_Delete(int nMonitorScheduleID, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_Delete", connection, tn))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;
    //        command.Parameters.Add(new SqlParameter("@nMonitorScheduleID", nMonitorScheduleID));

    //        //connection.Open();
    //        command.ExecuteNonQuery();

    //        return 1;

    //    }

    //}


    //public static DataTable ets_MonitorSchedule_Select(int? nMonitorScheduleID, int? nAccountID,
    //   int? nTableID, DateTime? dScheduleDateTimeStart, DateTime? dScheduleDateTimeEnd,
    //    bool? bHasAlarm, bool? dAlarmDateTime, int? nInitialScheduleID, DateTime? dateOneDay, string sSearchText)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

    //            if (dateOneDay != null)
    //                command.Parameters.Add(new SqlParameter("@dateOneDay", dateOneDay));

    //            if (nMonitorScheduleID != null)
    //                command.Parameters.Add(new SqlParameter("@nMonitorScheduleID", nMonitorScheduleID));

    //            if (nAccountID != null)
    //                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

    //            if (nTableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

    //            //if (nLocationID != null)
    //            //    command.Parameters.Add(new SqlParameter("@nLocationID", nLocationID));

    //            if (dScheduleDateTimeStart != null)
    //                command.Parameters.Add(new SqlParameter("@dScheduleDateTimeStart", dScheduleDateTimeStart));

    //            if (dScheduleDateTimeEnd != null)
    //                command.Parameters.Add(new SqlParameter("@dScheduleDateTimeEnd", dScheduleDateTimeEnd));

    //            if (bHasAlarm != null)
    //                command.Parameters.Add(new SqlParameter("@bHasAlarm", bHasAlarm));

    //            if (dAlarmDateTime != null)
    //                command.Parameters.Add(new SqlParameter("@dAlarmDateTime", dAlarmDateTime));

    //            if(nInitialScheduleID!=null)
    //                command.Parameters.Add(new SqlParameter("@nInitialScheduleID", nInitialScheduleID));

    //            if (sSearchText != "")
    //                command.Parameters.Add(new SqlParameter("@sSearchText", sSearchText));


    //            connection.Open();
    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            da.Fill(dt);

    //            connection.Close();
    //            connection.Dispose();

    //            return dt;

    //        }
    //    }
    //}




 




    //public static DataTable ets_MonitorSchedule_BySTandSS( 
    //    int nTableID, DateTime dScheduleDateTime)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_BySTandSS", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;    

    //                //command.Parameters.Add(new SqlParameter("@nLocationID", nLocationID));

    //                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
    //                command.Parameters.Add(new SqlParameter("@dScheduleDateTime", dScheduleDateTime));
                
    //            connection.Open();
    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            da.Fill(dt);

    //            connection.Close();
    //            connection.Dispose();

    //            return dt;

    //        }
    //    }
    //}



   

  

    //public static DataTable ets_MonitorSchedule_SelectByDateRange(
    // int? nTableID, DateTime dStartDateTime, DateTime dEndDateTime, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_MonitorSchedule_SelectByDateRange", connection, tn))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        //if (nLocationID!=null)
    //        //command.Parameters.Add(new SqlParameter("@nLocationID", nLocationID));

    //        if (nTableID != null)
    //        command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
            
    //        command.Parameters.Add(new SqlParameter("@dStartDateTime", dStartDateTime));
    //        command.Parameters.Add(new SqlParameter("@dEndDateTime", dEndDateTime));

    //        //connection.Open();


    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = command;
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
    //        return dt;

    //    }

    //}



    //public static int ets_MonitorScheduleUser_Insert(MonitorScheduleUser P_MonitorScheduleUser, SqlTransaction tn, SqlConnection cn)
    //{

    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //            connection.Open();
    //        }
    //    }
    //    else
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = tn.Connection;
    //        }
    //    }

    //    using (SqlCommand command = new SqlCommand("ets_MonitorScheduleUser_Insert", connection))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //        pRV.Direction = ParameterDirection.Output;

    //        command.Parameters.Add(pRV);
    //        command.Parameters.Add(new SqlParameter("@nMonitorScheduleID", P_MonitorScheduleUser.MonitorScheduleID));
    //        command.Parameters.Add(new SqlParameter("@nUserID", P_MonitorScheduleUser.UserID));
    //        command.Parameters.Add(new SqlParameter("@sEmailSMS", P_MonitorScheduleUser.EmailSMS));
                       

    //        //connection.Open();
    //        command.ExecuteNonQuery();


    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }

    //        return int.Parse(pRV.Value.ToString());
    //    }

    //}


    //public static int ets_MonitorScheduleUser_Delete(int nMonitorScheduleUserID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_MonitorScheduleUser_Delete", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nMonitorScheduleUserID", nMonitorScheduleUserID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}



    //public static DataTable ets_MonitorScheduleUser_Select(int? nMonitorScheduleUserID,
    //   int? nMonitorScheduleID, int? nUserID, SqlTransaction tn, SqlConnection cn)
    //{

    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //            connection.Open();
    //        }
    //    }
    //    else
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = tn.Connection;
    //        }
    //    }

    //    using (SqlCommand command = new SqlCommand("ets_MonitorScheduleUser_Select", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }
    //        //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

    //        if (nMonitorScheduleUserID != null)
    //            command.Parameters.Add(new SqlParameter("@nMonitorScheduleUserID", nMonitorScheduleUserID));

    //        if (nMonitorScheduleID != null)
    //            command.Parameters.Add(new SqlParameter("@nMonitorScheduleID", nMonitorScheduleID));

    //        if (nUserID !=null)
    //            command.Parameters.Add(new SqlParameter("@nUserID", nUserID));


    //        //connection.Open();

    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = command;
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
            

    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }

    //        return dt;
    //    }

    //}





    public static int ets_ScheduleReport_Insert(ScheduleReport p_ScheduleReport)
    {




        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_ScheduleReport_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nMainDocumentID", p_ScheduleReport.MainDocumentID));
                command.Parameters.Add(new SqlParameter("@sFrequency", p_ScheduleReport.Frequency));
                command.Parameters.Add(new SqlParameter("@sFrequencyWhen", p_ScheduleReport.FrequencyWhen));

                command.Parameters.Add(new SqlParameter("@nReportPeriod", p_ScheduleReport.ReportPeriod));
                command.Parameters.Add(new SqlParameter("@sReportPeriodUnit", p_ScheduleReport.ReportPeriodUnit));

                if (p_ScheduleReport.Emails != null)
                    command.Parameters.Add(new SqlParameter("@sEmails", p_ScheduleReport.Emails));

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return -1;

            }

        }


    }


    public static int ets_ScheduleReport_Update(ScheduleReport p_ScheduleReport)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_ScheduleReport_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               
                command.Parameters.Add(new SqlParameter("@nScheduleReportID", p_ScheduleReport.ScheduleReportID));
                command.Parameters.Add(new SqlParameter("@nMainDocumentID", p_ScheduleReport.MainDocumentID));
                command.Parameters.Add(new SqlParameter("@sFrequency", p_ScheduleReport.Frequency));
                command.Parameters.Add(new SqlParameter("@sFrequencyWhen", p_ScheduleReport.FrequencyWhen));

                command.Parameters.Add(new SqlParameter("@nReportPeriod", p_ScheduleReport.ReportPeriod));
                command.Parameters.Add(new SqlParameter("@sReportPeriodUnit", p_ScheduleReport.ReportPeriodUnit));

                if (p_ScheduleReport.Emails != null)
                    command.Parameters.Add(new SqlParameter("@sEmails", p_ScheduleReport.Emails));


                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;
            }
        }



       

    }



    public static DataTable ets_ScheduleReport_Select( int nAccountID, int? nMainDocumentID, string sFrequency,
      string sFrequencyWhen, DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_ScheduleReport_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nMainDocumentID != null)
                    command.Parameters.Add(new SqlParameter("@nMainDocumentID", nMainDocumentID));

                if (sFrequency != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sFrequency", sFrequency));

                if (sFrequencyWhen != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sFrequencyWhen", sFrequencyWhen));

            

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));
                               

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "MainDocumentID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


               

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

                iTotalRowsNum = 0;
                if (ds == null) return null;


                if (ds.Tables.Count > 1)
                {
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }


            }
        }
    }




    public static int ets_ScheduleReport_Delete(int nScheduleReportID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_ScheduleReport_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nScheduleReportID ", nScheduleReportID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }




    public static ScheduleReport ets_ScheduleReport_Detail(int nScheduleReportID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_ScheduleReport_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nScheduleReportID", nScheduleReportID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScheduleReport temp = new ScheduleReport(
                                (int)reader["ScheduleReportID"],
                                (int)reader["MainDocumentID"], (string)reader["Frequency"], (string)reader["FrequencyWhen"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]
                                );

                            temp.ReportPeriod = (int)reader["ReportPeriod"];
                            temp.ReportPeriodUnit = (string)reader["ReportPeriodUnit"];
                            temp.Emails = reader["Emails"] == DBNull.Value ? "" : (string)reader["Emails"];

                            connection.Close();
                            connection.Dispose();

                            return temp;
                        }

                    }

                }
                catch
                {
                   
                }

                connection.Close();
                connection.Dispose();

                return null;

            }
        }



        

    }






    #region ScheduledTask



    public static int dbg_ScheduledTask_Insert(ScheduledTask p_ScheduledTask)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ScheduledTask_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nAccountID", p_ScheduledTask.AccountID));
                command.Parameters.Add(new SqlParameter("@nMessageID", p_ScheduledTask.MessageID));
                command.Parameters.Add(new SqlParameter("@sFrequency", p_ScheduledTask.Frequency));
                command.Parameters.Add(new SqlParameter("@sFrequencyWhen", p_ScheduledTask.FrequencyWhen));
                //command.Parameters.Add(new SqlParameter("@nScheduledTaskID", p_ScheduledTask.ScheduledTaskID));
                command.Parameters.Add(new SqlParameter("@sScheduleType", p_ScheduledTask.ScheduleType));
                command.Parameters.Add(new SqlParameter("@nTableID", p_ScheduledTask.TableID));
                command.Parameters.Add(new SqlParameter("@dRecordDateAdded", p_ScheduledTask.RecordDateAdded));
                command.Parameters.Add(new SqlParameter("@dLastEmailSentDate", p_ScheduledTask.LastEmailSentDate));


                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return -1;
            }
        }




        

    }


    public static int dbg_ScheduledTask_Update(ScheduledTask p_ScheduledTask)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ScheduledTask_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                

                command.Parameters.Add(new SqlParameter("@nScheduledTaskID", p_ScheduledTask.ScheduledTaskID));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_ScheduledTask.AccountID));
                command.Parameters.Add(new SqlParameter("@nEmailLogID", p_ScheduledTask.MessageID));
                command.Parameters.Add(new SqlParameter("@sFrequency", p_ScheduledTask.Frequency));
                command.Parameters.Add(new SqlParameter("@sFrequencyWhen", p_ScheduledTask.FrequencyWhen));
                //command.Parameters.Add(new SqlParameter("@nScheduledTaskID", p_ScheduledTask.ScheduledTaskID));
                command.Parameters.Add(new SqlParameter("@sScheduleType", p_ScheduledTask.ScheduleType));
                command.Parameters.Add(new SqlParameter("@nTableID", p_ScheduledTask.TableID));
                command.Parameters.Add(new SqlParameter("@dRecordDateAdded", p_ScheduledTask.RecordDateAdded));
                command.Parameters.Add(new SqlParameter("@dLastEmailSentDate", p_ScheduledTask.LastEmailSentDate));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }


        

    }



    public static DataTable dbg_ScheduledTask_Select(int? nAccountID, int? nTableID,string sFrequency,
        string sFrequencyWhen, string sScheduleType, int? nMessageID,
        string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, DateTime? dRecordDateAdded)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ScheduledTask_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if(sFrequency!="")
                command.Parameters.Add(new SqlParameter("@sFrequency", sFrequency));

                if (sFrequencyWhen != "")
                command.Parameters.Add(new SqlParameter("@sFrequencyWhen", sFrequencyWhen));

                if (sScheduleType != "")
                command.Parameters.Add(new SqlParameter("@sScheduleType", sScheduleType));

                if (nMessageID != null)
                    command.Parameters.Add(new SqlParameter("@nMessageID", nMessageID));

                if (dRecordDateAdded != null)
                command.Parameters.Add(new SqlParameter("@dRecordDateAdded", dRecordDateAdded)); 

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "ScheduledTaskID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                

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

                iTotalRowsNum = 0;
                if (ds == null) return null;


                if (ds.Tables.Count > 1)
                {
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }


            }
        }
    }




    public static int dbg_ScheduledTask_Delete(int nScheduledTaskID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ScheduledTask_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nScheduledTaskID ", nScheduledTaskID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }




    public static ScheduledTask dbg_ScheduledTask_Detail(int nScheduledTaskID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_ScheduledTask_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nScheduledTaskID", nScheduledTaskID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScheduledTask temp = new ScheduledTask(
                                (int)reader["ScheduledTaskID"], (int)reader["AccountID"],
                              reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
                              reader["Frequency"] == DBNull.Value ? string.Empty : (string)reader["Frequency"],
                              reader["FrequencyWhen"] == DBNull.Value ? string.Empty : (string)reader["FrequencyWhen"],
                              reader["ScheduleType"] == DBNull.Value ? string.Empty : (string)reader["ScheduleType"],
                              reader["MessageID"] == DBNull.Value ? null : (int?)reader["MessageID"]);
                            temp.RecordDateAdded = reader["RecordDateAdded"] == DBNull.Value ? null : (DateTime?)reader["RecordDateAdded"];
                            temp.LastEmailSentDate = reader["LastEmailSentDate"] == DBNull.Value ? null : (DateTime?)reader["LastEmailSentDate"];

                            connection.Close();
                            connection.Dispose();

                            return temp;
                        }

                    }
                }
                catch
                {
                   
                }

                connection.Close();
                connection.Dispose();
                return null;

            }

        }


    }




    public static ScheduledTask dbg_ScheduledTask_Detail_By_MessageID(int nMessageID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_ScheduledTask_Detail_By_MessageID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nMessageID", nMessageID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScheduledTask temp = new ScheduledTask(
                                (int)reader["ScheduledTaskID"], (int)reader["AccountID"],
                              reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
                              reader["Frequency"] == DBNull.Value ? string.Empty : (string)reader["Frequency"],
                              reader["FrequencyWhen"] == DBNull.Value ? string.Empty : (string)reader["FrequencyWhen"],
                              reader["ScheduleType"] == DBNull.Value ? string.Empty : (string)reader["ScheduleType"],
                              reader["MessageID"] == DBNull.Value ? null : (int?)reader["MessageID"]);
                            temp.RecordDateAdded = reader["RecordDateAdded"] == DBNull.Value ? null : (DateTime?)reader["RecordDateAdded"];
                            temp.LastEmailSentDate = reader["LastEmailSentDate"] == DBNull.Value ? null : (DateTime?)reader["LastEmailSentDate"];

                            connection.Close();
                            connection.Dispose();

                            return temp;
                        }

                    }

                }
                catch
                {
                   
                }

                connection.Close();
                connection.Dispose();
                return null;

            }
        }



    }








    #endregion



}

