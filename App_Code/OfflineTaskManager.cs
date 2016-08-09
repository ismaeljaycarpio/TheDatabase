using System;
using System.Collections;
using System.Collections.Generic;

//using System.Linq;

using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

/// <summary>
/// Summary description for OfflineTaskManager
/// </summary>
public class OfflineTaskManager
{
	public OfflineTaskManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}




    #region OfflineTask

    public static int dbg_OfflineTask_Insert(OfflineTask p_OfflineTask)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_OfflineTask_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@nAccountID", p_OfflineTask.AccountID));
                command.Parameters.Add(new SqlParameter("@nAddedByUserID", p_OfflineTask.AddedByUserID));
                command.Parameters.Add(new SqlParameter("@nPriority", p_OfflineTask.Priority));
                command.Parameters.Add(new SqlParameter("@sProcesstorun", p_OfflineTask.Processtorun));

                if (p_OfflineTask.Parameters!="")
                    command.Parameters.Add(new SqlParameter("@sParameters", p_OfflineTask.Parameters));
                if (p_OfflineTask.RepeatMins!=null)
                    command.Parameters.Add(new SqlParameter("@nRepeatMins", p_OfflineTask.RepeatMins));
                if (p_OfflineTask.ScheduledToRun != null)
                    command.Parameters.Add(new SqlParameter("@dScheduledToRun", p_OfflineTask.ScheduledToRun));
                if (p_OfflineTask.ActuallyRun != null)
                    command.Parameters.Add(new SqlParameter("@dActuallyRun", p_OfflineTask.ActuallyRun));


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
    
    public static int dbg_OfflineTask_Update(OfflineTask p_OfflineTask)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_OfflineTask_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               
                command.Parameters.Add(new SqlParameter("@nOfflineTaskID", p_OfflineTask.OfflineTaskID));


                command.Parameters.Add(new SqlParameter("@nPriority", p_OfflineTask.Priority));
                command.Parameters.Add(new SqlParameter("@sProcesstorun", p_OfflineTask.Processtorun));

                if (p_OfflineTask.Parameters != "")
                    command.Parameters.Add(new SqlParameter("@sParameters", p_OfflineTask.Parameters));
                if (p_OfflineTask.RepeatMins != null)
                    command.Parameters.Add(new SqlParameter("@nRepeatMins", p_OfflineTask.RepeatMins));
                if (p_OfflineTask.ScheduledToRun != null)
                    command.Parameters.Add(new SqlParameter("@dScheduledToRun", p_OfflineTask.ScheduledToRun));
                if (p_OfflineTask.ActuallyRun != null)
                    command.Parameters.Add(new SqlParameter("@dActuallyRun", p_OfflineTask.ActuallyRun));


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

    public static int dbg_OfflineTask_Delete(int nOfflineTaskID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_OfflineTask_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nOfflineTaskID ", nOfflineTaskID));

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

    
    public static OfflineTask dbg_OfflineTask_Detail(int nOfflineTaskID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_OfflineTask_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nOfflineTaskID", nOfflineTaskID));

                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OfflineTask temp = new OfflineTask(
                                (int)reader["OfflineTaskID"], (int)reader["AccountID"], (int)reader["AddedByUserID"], (byte)reader["Priority"],
                              (string)reader["Processtorun"], reader["Parameters"] == DBNull.Value ? string.Empty : (string)reader["Parameters"],
                               reader["RepeatMins"] == DBNull.Value ? null : (int?)reader["RepeatMins"],
                                reader["ScheduledToRun"] == DBNull.Value ? null : (DateTime?)reader["ScheduledToRun"],
                                 reader["ActuallyRun"] == DBNull.Value ? null : (DateTime?)reader["ActuallyRun"],
                                 (DateTime)reader["DateAdded"]
                                );

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



    #region OfflineTaskLog



    public static int dbg_OfflineTaskLog_Insert(OfflineTaskLog p_OfflineTaskLog)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_OfflineTaskLog_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;



                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nOfflineTaskID", p_OfflineTaskLog.OfflineTaskID));
                command.Parameters.Add(new SqlParameter("@sResult", p_OfflineTaskLog.Result));


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

    public static int dbg_OfflineTaskLog_Update(OfflineTaskLog p_OfflineTaskLog)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_OfflineTaskLog_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nOfflineTaskLogID", p_OfflineTaskLog.OfflineTaskLogID));

                command.Parameters.Add(new SqlParameter("@nOfflineTaskID", p_OfflineTaskLog.OfflineTaskID));
                command.Parameters.Add(new SqlParameter("@sResult", p_OfflineTaskLog.Result));


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
    
    public static int dbg_OfflineTaskLog_Delete(int nOfflineTaskLogID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_OfflineTaskLog_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nOfflineTaskLogID ", nOfflineTaskLogID));

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
    
    public static OfflineTaskLog dbg_OfflineTaskLog_Detail(int nOfflineTaskLogID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_OfflineTaskLog_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nOfflineTaskLogID", nOfflineTaskLogID));

                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OfflineTaskLog temp = new OfflineTaskLog(
                                (int)reader["OfflineTaskLogID"], (int)reader["OfflineTaskID"], (string)reader["Result"],
                              (DateTime)reader["DateAdded"]);

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


    public static DataTable dbg_OfflineTask_ListToProcess()
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_OfflineTask_ListToProcess", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();


                connection.Open();
                try
                {
                    da.Fill(dt);
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();

                return dt;

            }
        }
    }


    public static int ets_Record_List_BCP(string sSQL, string sHeaderSQL, string sFileName)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_List_BCP", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@sSQL ", sSQL));
                command.Parameters.Add(new SqlParameter("@sHeaderSQL ", sHeaderSQL));
                command.Parameters.Add(new SqlParameter("@sFileName ", sFileName));

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


}