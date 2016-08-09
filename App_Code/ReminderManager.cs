using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ReminderManager
/// </summary>
public class ReminderManager
{
	public ReminderManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}




    public static int ets_DataReminder_Insert(DataReminder p_DataReminder)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DataReminder_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nColumnID", p_DataReminder.ColumnID));
                command.Parameters.Add(new SqlParameter("@nNumberOfDays", p_DataReminder.NumberOfDays));
                command.Parameters.Add(new SqlParameter("@sReminderContent", p_DataReminder.ReminderContent));
                command.Parameters.Add(new SqlParameter("@sReminderHeader", p_DataReminder.ReminderHeader));


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


    public static int ets_DataReminder_Update(DataReminder p_DataReminder)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DataReminder_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               
                command.Parameters.Add(new SqlParameter("@nDataReminderID", p_DataReminder.DataReminderID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_DataReminder.ColumnID));
                command.Parameters.Add(new SqlParameter("@nNumberOfDays", p_DataReminder.NumberOfDays));
                command.Parameters.Add(new SqlParameter("@sReminderContent", p_DataReminder.ReminderContent));
                command.Parameters.Add(new SqlParameter("@sReminderHeader", p_DataReminder.ReminderHeader));


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



    public static DataTable ets_DataReminder_Select(int? nColumnID, int? nNumberOfDays, string sReminderHeader,
       string sReminderContent, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DataReminder_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

                if (nNumberOfDays != null)
                    command.Parameters.Add(new SqlParameter("@nNumberOfDays", nNumberOfDays));

                if (sReminderHeader != "")
                    command.Parameters.Add(new SqlParameter("@sReminderHeader", sReminderHeader));

                if (sReminderContent != "")
                    command.Parameters.Add(new SqlParameter("@sReminderContent", sReminderContent));
               


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "DataReminderID"; sOrderDirection = "DESC"; }

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




    public static int ets_DataReminder_Delete(int nDataReminderID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DataReminder_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nDataReminderID ", nDataReminderID));

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




    public static DataReminder ets_DataReminder_Detail(int nDataReminderID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DataReminder_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nDataReminderID", nDataReminderID));

                connection.Open();


                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataReminder temp = new DataReminder(
                                (int)reader["DataReminderID"], (int)reader["ColumnID"], (int)reader["NumberOfDays"],
                               reader["ReminderHeader"] == DBNull.Value ? "" : (string)reader["ReminderHeader"],
                               reader["ReminderContent"] == DBNull.Value ? "" : (string)reader["ReminderContent"]
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









    public static int ets_DataReminderUser_Insert(DataReminderUser p_DataReminderUser)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("ets_DataReminderUser_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                                

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nDataReminderID", p_DataReminderUser.DataReminderID));
                command.Parameters.Add(new SqlParameter("@nUserID", p_DataReminderUser.UserID));
                command.Parameters.Add(new SqlParameter("@nReminderColumnID", p_DataReminderUser.ReminderColumnID));

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


    public static int ets_DataReminderUser_Update(DataReminderUser p_DataReminderUser)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_DataReminderUser_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               
                command.Parameters.Add(new SqlParameter("@nDataReminderUserID", p_DataReminderUser.DataReminderUserID));
                command.Parameters.Add(new SqlParameter("@nDataReminderID", p_DataReminderUser.DataReminderID));
                command.Parameters.Add(new SqlParameter("@nUserID", p_DataReminderUser.UserID));
                command.Parameters.Add(new SqlParameter("@nReminderColumnID", p_DataReminderUser.ReminderColumnID));


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



    public static DataTable dbg_DataReminderUser_Select(int? nDataReminderID, int? nUserID,
       string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_DataReminderUser_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

             

                if (nDataReminderID != null)
                    command.Parameters.Add(new SqlParameter("@nDataReminderID", nDataReminderID));

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

                


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "DataReminderUserID"; sOrderDirection = "DESC"; }

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




    public static int ets_DataReminderUser_Delete(int nDataReminderUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DataReminderUser_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nDataReminderUserID ", nDataReminderUserID));

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



    public static DataTable ets_DataReminderUser_Select(int? nDataReminderID, int? nUserID, string sOrder,
    string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DataReminderUser_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

                if (nDataReminderID != null)
                    command.Parameters.Add(new SqlParameter("@nDataReminderID", nDataReminderID));

                

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "DataReminderUserID"; sOrderDirection = "DESC"; }

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


    public static DataReminderUser ets_DataReminderUser_Detail(int nDataReminderUserID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_DataReminderUser_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nDataReminderUserID", nDataReminderUserID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataReminderUser temp = new DataReminderUser(
                                (int)reader["DataReminderUserID"], (int)reader["DataReminderID"], (int)reader["UserID"]
                                );
                            temp.ReminderColumnID = reader["ReminderColumnID"] == DBNull.Value ? null : (int?)reader["ReminderColumnID"];

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













}