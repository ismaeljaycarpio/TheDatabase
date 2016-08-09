using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
/// <summary>
/// Summary description for SystemData
/// </summary>
public class SystemData
{
    public SystemData()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DataTable Run_ContentSP(string sSPName, string sKey)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Run_ContentSP", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@sSPName", sSPName));

                if (sKey != "")
                    command.Parameters.Add(new SqlParameter("@sKey", sKey));


               
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
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
                    return ds.Tables[0];
                }
                {
                    return null;
                }
            }
        }
    }



    public static DataTable Run_ContentSP(string sSPName, string sKey, string sRoot)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Run_ContentSP", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@sSPName", sSPName));

                if (sKey != "")
                    command.Parameters.Add(new SqlParameter("@sKey", sKey));
                if (sRoot != "")
                    command.Parameters.Add(new SqlParameter("@sRoot", sRoot));


                
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

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
                    return ds.Tables[0];
                }
                {
                    return null;
                }
            }
        }
    }


    #region SystemOption

    //public static List<SystemOption> SystemOption_Select(int? nSystemOptionID, string sOptionKey, string sOptionValue,
    //    string sOptionNotes, DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
    //  string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("SystemOption_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            if (nSystemOptionID != null)
    //                command.Parameters.Add(new SqlParameter("@nSystemOptionID", nSystemOptionID));

    //            if (sOptionKey != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));
    //            if (sOptionValue != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sOptionValue", sOptionValue));
    //            if (sOptionNotes != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sOptionNotes", sOptionNotes));

    //            if (dDateAdded != null)
    //                command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

    //            if (dDateUpdated != null)
    //                command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

    //            if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
    //            { sOrder = "SystemOptionID"; sOrderDirection = "DESC"; }
    //            command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

    //            if (nStartRow != null)
    //                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

    //            if (nMaxRows != null)
    //                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


    //            List<SystemOption> list = new List<SystemOption>();
    //            connection.Open();
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    SystemOption temp = new SystemOption(
    //                        (int)reader["SystemOptionID"],
    //                        (string)reader["OptionKey"],
    //                        (string)reader["OptionValue"],
    //                        (string) reader["OptionNotes"],
    //                        (DateTime)reader["DateAdded"],
    //                        (DateTime)reader["DateUpdated"]);

    //                    list.Add(temp);

    //                }

    //                reader.NextResult();
    //                while (reader.Read())
    //                {
    //                    iTotalRowsNum = (int)reader["TotalRows"];
    //                }
    //            }

    //            connection.Close();
    //            connection.Dispose();

    //            return list;
    //        }
    //    }
    //}




    public static DataTable  SystemOption_Select( string sOptionKey,int? nAccountID,int? nTableID, string sOptionValue,
       string sOptionNotes, DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
     string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SystemOption_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                

                if (sOptionKey != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));
                if (sOptionValue != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sOptionValue", sOptionValue));
                if (sOptionNotes != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sOptionNotes", sOptionNotes));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "SystemOptionID"; sOrderDirection = "DESC"; }
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

                return null;
            }
        }
    }
    public static int SystemOption_Insert(SystemOption p_SystemOption)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SystemOption_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
                
                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sOptionKey", p_SystemOption.OptionKey));
                command.Parameters.Add(new SqlParameter("@sOptionValue", p_SystemOption.OptionValue));
                command.Parameters.Add(new SqlParameter("@sOptionNotes", p_SystemOption.OptionNotes));
                if (p_SystemOption.AccountID!=null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", p_SystemOption.AccountID));
                if (p_SystemOption.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_SystemOption.TableID));

                //command.Parameters.Add(new SqlParameter("@dDateAdded", p_SystemOption.DateAdded));
                //command.Parameters.Add(new SqlParameter("@dDateUpdated", p_SystemOption.DateUpdated));

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

    public static int SystemOption_Update(SystemOption p_SystemOption)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SystemOption_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nSystemOptionID", p_SystemOption.SystemOptionID));
                command.Parameters.Add(new SqlParameter("@sOptionKey", p_SystemOption.OptionKey));
                command.Parameters.Add(new SqlParameter("@sOptionValue", p_SystemOption.OptionValue));
                command.Parameters.Add(new SqlParameter("@sOptionNotes", p_SystemOption.OptionNotes));

                if (p_SystemOption.AccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", p_SystemOption.AccountID));
                if (p_SystemOption.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_SystemOption.TableID));

                //command.Parameters.Add(new SqlParameter("@dDateUpdated", p_SystemOption.DateUpdated));


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


    public static int SystemOption_Delete(int nSystemOptionID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SystemOption_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nSystemOptionID", nSystemOptionID));

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



    //public static string SystemOption_ValueByKey(string sOptionKey,string strDefault)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("SystemOption_ValueByKey", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@sOptionValue", SqlDbType.NVarChar);               
    //            pRV.Direction = ParameterDirection.Output;
    //            pRV.Size = 1000;

    //            command.Parameters.Add(pRV);
    //            command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));               


    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            if (pRV.Value.ToString() == "")
    //            {
    //                return strDefault;
    //            }
    //            else
    //            {
    //                return pRV.Value.ToString();
    //            }
    //        }
    //    }
    //}


    //public static string SystemOption_ValueByKey(string sOptionKey)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("SystemOption_ValueByKey", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@sOptionValue", SqlDbType.NVarChar);
    //            pRV.Direction = ParameterDirection.Output;
    //            pRV.Size = 1000;

    //            command.Parameters.Add(pRV);
    //            command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));


    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();
                
    //            return pRV.Value.ToString();
               
    //        }
    //    }
    //}


    //public static string SystemOption_ValueByKey(string sOptionKey, ref SqlConnection connection,ref SqlTransaction tn)
    //{
        
    //        using (SqlCommand command = new SqlCommand("SystemOption_ValueByKey", connection,tn))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@sOptionValue", SqlDbType.NVarChar);
    //            pRV.Direction = ParameterDirection.Output;
    //            pRV.Size = 1000;

    //            command.Parameters.Add(pRV);
    //            command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));


    //            //connection.Open();
    //            command.ExecuteNonQuery();

    //            return pRV.Value.ToString();

    //        }
        
    //}



    public static string SystemOption_ValueByKey_Account_Default(string sOptionKey, int? nAccountID, int? nTableID,string strDefault)
    {
        string strOptionValue = SystemOption_ValueByKey_Account(sOptionKey, nAccountID, nTableID);
        if(strOptionValue=="")
        {
            strOptionValue = strDefault;
        }
        return strOptionValue;
    }


    //public static string SystemOption_ValueByKey_Account(string sOptionKey, int? nAccountID, int? nTableID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("SystemOption_ValueByKey_Account", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@sOptionValue", SqlDbType.NVarChar);
    //            pRV.Direction = ParameterDirection.Output;
    //            pRV.Size = 1000;

    //            command.Parameters.Add(pRV);
    //            command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));

    //            if (nAccountID == null)
    //            {
    //                try
    //                {
    //                    if (System.Web.HttpContext.Current.Session["AccountID"] != null)
    //                    {
    //                        nAccountID = int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString());
    //                    }
    //                }
    //                catch
    //                {
    //                    //
    //                }
    //            }
                
               

    //            if(nAccountID!=null)
    //                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

    //            if (nTableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                
    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return pRV.Value.ToString();

    //        }
    //    }
    //}



    public static string SystemOption_ValueByKey_Account(string sOptionKey, int? nAccountID, int? nTableID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("SystemOption_ValueByKey_Account", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@sOptionValue", SqlDbType.NVarChar);
                pRV.Direction = ParameterDirection.Output;
                pRV.Size = 1000;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));

                if (nAccountID == null)
                {
                    try
                    {
                        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["AccountID"] != null)
                        {
                            nAccountID = int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString());
                        }
                    }
                    catch
                    {
                        //
                    }
                }



                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));


                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return pRV.Value.ToString();
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return "";

            }
        
        }


    }
    public static string SystemOption_NotesByKey_Account(string sOptionKey, int? nAccountID, int? nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SystemOption_NotesByKey_Account", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@sOptionNotes", SqlDbType.NVarChar);
                pRV.Direction = ParameterDirection.Output;
                pRV.Size = 1000;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));


                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));


                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return pRV.Value.ToString();
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return "";
            }
        }
    }



    public static SystemOption SystemOption_Detail_Key_Account(string sOptionKey, int? nAccountID, int? nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SystemOption_Detail_Key_Account", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@sOptionKey", sOptionKey));

                if (nAccountID == null && System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null  && System.Web.HttpContext.Current.Session["AccountID"] != null)
                {
                    nAccountID = int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString());
                }


                if (nAccountID!=null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nTableID!=null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));


                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SystemOption temp = new SystemOption(
                                (int)reader["SystemOptionID"],
                                (string)reader["OptionKey"],
                                (string)reader["OptionValue"],
                                (string)reader["OptionNotes"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            temp.AccountID = reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"];
                            temp.TableID = reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"];

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
    public static SystemOption SystemOption_Details(int nSystemOptionID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SystemOption_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nSystemOptionID", nSystemOptionID));
                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SystemOption temp = new SystemOption(
                                (int)reader["SystemOptionID"],
                                (string)reader["OptionKey"],
                                (string)reader["OptionValue"],
                                (string)reader["OptionNotes"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            temp.AccountID = reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"];
                            temp.TableID = reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"];
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

    #region ErrorLog

    public static List<ErrorLog> ErrorLog_Select(int? nErrorLogID, string sModule, string sErrorMessage,
        string sErrorTrack, DateTime? dErrorTime, string sPath, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ErrorLog_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (nErrorLogID != null)
                    command.Parameters.Add(new SqlParameter("@nErrorLogID", nErrorLogID));

                if (sModule != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sModule", sModule));
                if (sErrorMessage != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sErrorMessage", sErrorMessage));
                if (sErrorTrack != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sErrorTrack", sErrorTrack));

                if (dErrorTime != null)
                    command.Parameters.Add(new SqlParameter("@dErrorTime", dErrorTime));

                if (sPath != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sPath", sPath));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "ErrorLogID"; sOrderDirection = "DESC"; }
                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                List<ErrorLog> list = new List<ErrorLog>();
                connection.Open();


                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ErrorLog temp = new ErrorLog(
                                (int)reader["ErrorLogID"],
                                (string)reader["Module"],
                                (string)reader["ErrorMessage"],
                                (string)reader["ErrorTrack"],
                                (DateTime)reader["ErrorTime"],
                                (string)reader["Path"]);

                            list.Add(temp);

                        }

                        reader.NextResult();
                        while (reader.Read())
                        {
                            iTotalRowsNum = (int)reader["TotalRows"];
                        }
                    }

                }
                catch
                {
                    list = null;
                }


                connection.Close();
                connection.Dispose();
                return list;
            }
        }
    }

    public static ErrorLog ErrorLog_Details(int nErrorLogID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ErrorLog_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nErrorLogID", nErrorLogID));
                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ErrorLog temp = new ErrorLog(
                                (int)reader["ErrorLogID"],
                                (string)reader["Module"],
                                (string)reader["ErrorMessage"],
                                (string)reader["ErrorTrack"],
                                (DateTime)reader["ErrorTime"],
                                (string)reader["Path"]);

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

    public static int ErrorLog_Insert(ErrorLog p_ErrorLog)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ErrorLog_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
               
                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sModule", p_ErrorLog.Module));
                command.Parameters.Add(new SqlParameter("@sErrorMessage", p_ErrorLog.ErrorMessage));
                command.Parameters.Add(new SqlParameter("@sErrorTrack", p_ErrorLog.ErrorTrack==null?"":p_ErrorLog.ErrorTrack));
                command.Parameters.Add(new SqlParameter("@dErrorTime", p_ErrorLog.ErrorTime));
                command.Parameters.Add(new SqlParameter("@sPath", p_ErrorLog.Path));

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





   
    //public static int ErrorLog_Update(ErrorLog p_ErrorLog)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ErrorLog_Update", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nErrorLogID", p_ErrorLog.ErrorLogID));
    //            command.Parameters.Add(new SqlParameter("@sModule", p_ErrorLog.Module));
    //            command.Parameters.Add(new SqlParameter("@sErrorMessage", p_ErrorLog.ErrorMessage));
    //            command.Parameters.Add(new SqlParameter("@sErrorTrack", p_ErrorLog.ErrorTrack));
    //            command.Parameters.Add(new SqlParameter("@dErrorTime", p_ErrorLog.ErrorTime));
    //            command.Parameters.Add(new SqlParameter("@sPath", p_ErrorLog.Path));


    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}


    public static int ErrorLog_Delete(int nErrorLogID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ErrorLog_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nErrorLogID", nErrorLogID));

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

    #endregion

    #region VisitorLog


    public static int ets_VisitorLog_Insert(VisitorLog p_VisitorLog)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_VisitorLog_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                
                if (p_VisitorLog.UserID!=null)
                command.Parameters.Add(new SqlParameter("@nUserID", p_VisitorLog.UserID));

                if(p_VisitorLog.IPAddress!="")
                command.Parameters.Add(new SqlParameter("@sIPAddress", p_VisitorLog.IPAddress));

                if(p_VisitorLog.Browser!="")
                command.Parameters.Add(new SqlParameter("@sBrowser", p_VisitorLog.Browser));

                if (p_VisitorLog.PageURL!="")
                command.Parameters.Add(new SqlParameter("@sPageURL", p_VisitorLog.PageURL));

                if (p_VisitorLog.RefSite != "")
                    command.Parameters.Add(new SqlParameter("@sRefSite", p_VisitorLog.RefSite));



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





    public static DataTable ets_VisitorLog_Select(int? nVisitorLogID,
  int? nUserID, string sIPAddress, string sBrowser, string sPageURL, 
       string sEmail, DateTime? dDateFrom, DateTime? dDateTo,
      string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_VisitorLog_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


                if (nVisitorLogID != null)
                    command.Parameters.Add(new SqlParameter("@nVisitorLogID", nVisitorLogID));

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

                if (sIPAddress != "")
                    command.Parameters.Add(new SqlParameter("@sIPAddress", sIPAddress));

                if (sBrowser != "")
                    command.Parameters.Add(new SqlParameter("@sBrowser", sBrowser));

                if (sPageURL != "")
                    command.Parameters.Add(new SqlParameter("@sPageURL", sPageURL));

              
                if (sEmail != "")
                    command.Parameters.Add(new SqlParameter("@sEmail", sEmail));


                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));

                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));


                if (sOrder == "")
                    sOrder = "VisitorLogID";

                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

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

    public static void VisitorInsert(User p_User, string p_strIPAddress, string p_strBrowser,
        string p_strPageURL,string strRefSite)
    {
        try
        {
            int? iUserID=null;
            if (p_User!=null)
            {
                iUserID=p_User.UserID;
            }
            VisitorLog theVisitorLog = new VisitorLog(null, iUserID, p_strIPAddress, p_strBrowser,
              p_strPageURL, null, strRefSite);

            SystemData.ets_VisitorLog_Insert(theVisitorLog);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Visitor Log", ex.Message, ex.StackTrace, DateTime.Now, p_strPageURL);
            SystemData.ErrorLog_Insert(theErrorLog);
        }


    }


    public static int spUpdateLinkedTables(int nParentTableID, string sParentTextField, int nChildTableID,
        string sChildTextField, string sChildLinkField, string sParentTextField2, string sChildTextField2)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spUpdateLinkedTables", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nParentTableID", nParentTableID));
                command.Parameters.Add(new SqlParameter("@sParentTextField", sParentTextField));
                command.Parameters.Add(new SqlParameter("@nChildTableID", nChildTableID));
                command.Parameters.Add(new SqlParameter("@sChildTextField", sChildTextField));
                command.Parameters.Add(new SqlParameter("@sChildLinkField", sChildLinkField));


                if (sParentTextField2 != "")
                    command.Parameters.Add(new SqlParameter("@sParentTextField2", sParentTextField2));
                if (sChildTextField2 != "")
                    command.Parameters.Add(new SqlParameter("@sChildTextField2", sChildTextField2));


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


    public static int Record_Copy_Field(int nTableID, string sColumnNameFrom, string sColumnNameTo)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Record_Copy_Field", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                command.Parameters.Add(new SqlParameter("@sColumnNameFrom", sColumnNameFrom));
                command.Parameters.Add(new SqlParameter("@sColumnNameTo", sColumnNameTo));
               

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

    public static int spResetIDs(int TableID, string ColumnName, bool TestRun)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spResetIDs", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                
                command.Parameters.Add(new SqlParameter("@TableID", TableID));
                command.Parameters.Add(new SqlParameter("@ColumnName", ColumnName));
                command.Parameters.Add(new SqlParameter("@TestRun", TestRun));




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

    public static int PageCountPopulate()
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("PageCountPopulate", connection))           
            {

                command.CommandType = CommandType.StoredProcedure;

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

                return 1;

            }
        }
    }



    public static DataTable PageCount_Select( string sPageURL,
      DateTime? dDateFrom, DateTime? dDateTo,
      string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("PageCount_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));
                               

                if (sPageURL != "")
                    command.Parameters.Add(new SqlParameter("@sPageURL", sPageURL));               


                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));

                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));


                if (sOrder == "")
                    sOrder = "visiteddate";

                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

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






    #endregion

    #region Content


    public static DataTable Content_Select(int? nContentID, string sContentKey, string sHeading,
      string sContent, string sContentType, string sStoredProcedure, DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
    string sOrderDirection, int? nStartRow, int? nMaxRows, int? nAccountID, ref int iTotalRowsNum,
       bool bGlobal, bool? bOnlyTemplate, bool? bOnlyGlobal, int? nContentTypeID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Content_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                if (bOnlyTemplate != null)
                    command.Parameters.Add(new SqlParameter("@bOnlyTemplate", bOnlyTemplate));
                if (bOnlyGlobal != null)
                    command.Parameters.Add(new SqlParameter("@bOnlyGlobal", bOnlyGlobal));


                if (nContentID != null)
                    command.Parameters.Add(new SqlParameter("@nContentID", nContentID));

                if (nContentTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nContentTypeID", nContentTypeID));


                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sContentKey != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sContentKey", sContentKey));
                if (sHeading != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sHeading", sHeading));
                if (sContent != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sContent", sContent));

                if (sContentType != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sContentType", sContentType));

                if (sStoredProcedure != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sStoredProcedure", sStoredProcedure));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "ContentID"; sOrderDirection = "DESC"; }
                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                command.Parameters.Add(new SqlParameter("@bGlobal", bGlobal));


               

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

                return null;
            }
        }
    }


    //public static List<Content> Content_Select(int? nContentID, string sContentKey, string sHeading,
    //   string sContent, string sContentType, string sStoredProcedure, DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
    // string sOrderDirection, int? nStartRow, int? nMaxRows, int? nAccountID, ref int iTotalRowsNum,
    //    bool bGlobal, bool? bOnlyTemplate, bool? bOnlyGlobal, int? nContentTypeID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("Content_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;


    //            if (bOnlyTemplate != null)
    //                command.Parameters.Add(new SqlParameter("@bOnlyTemplate", bOnlyTemplate));
    //            if (bOnlyGlobal != null)
    //                command.Parameters.Add(new SqlParameter("@bOnlyGlobal", bOnlyGlobal));


    //            if (nContentID != null)
    //                command.Parameters.Add(new SqlParameter("@nContentID", nContentID));

    //            if (nContentTypeID != null)
    //                command.Parameters.Add(new SqlParameter("@nContentTypeID", nContentTypeID));


    //            if (nAccountID != null)
    //                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

    //            if (sContentKey != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sContentKey", sContentKey));
    //            if (sHeading != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sHeading", sHeading));
    //            if (sContent != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sContent", sContent));

    //            if (sContentType != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sContentType", sContentType));

    //            if (sStoredProcedure != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sStoredProcedure", sStoredProcedure));

    //            if (dDateAdded != null)
    //                command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

    //            if (dDateUpdated != null)
    //                command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

    //            if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
    //            { sOrder = "ContentID"; sOrderDirection = "DESC"; }
    //            command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

    //            if (nStartRow != null)
    //                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

    //            if (nMaxRows != null)
    //                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

    //            command.Parameters.Add(new SqlParameter("@bGlobal", bGlobal));


    //            List<Content> list = new List<Content>();
    //            connection.Open();
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    Content temp = new Content(
    //                       (int)reader["ContentID"],
    //                       (string)reader["ContentKey"],
    //                       reader["Heading"] == DBNull.Value ? string.Empty : (string)reader["Heading"],
    //                       (string)reader["Content"],
    //                       reader["StoredProcedure"] == DBNull.Value ? string.Empty : (string)reader["StoredProcedure"],
    //                       (DateTime)reader["DateAdded"],
    //                       (DateTime)reader["DateUpdated"],
    //                       reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
    //                       (bool)reader["ForAllAccount"]);
    //                    temp.ContentTypeID = reader["ContentTypeID"] == DBNull.Value ? null : (int?)reader["ContentTypeID"];
    //                    list.Add(temp);

    //                }

    //                reader.NextResult();
    //                while (reader.Read())
    //                {
    //                    iTotalRowsNum = (int)reader["TotalRows"];
    //                }
    //            }

    //            connection.Close();
    //            connection.Dispose();

    //            return list;
    //        }
    //    }
    //}



    public static Content Content_Details_ByKey(string sContentKey, int? nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Content_Details_ByKey", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                
                command.Parameters.Add(new SqlParameter("@sContentKey", sContentKey));

                if (nAccountID!=null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                connection.Open();

                try
                {



                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Content temp = new Content(
                              (int)reader["ContentID"],
                              (string)reader["ContentKey"],
                              reader["Heading"] == DBNull.Value ? string.Empty : (string)reader["Heading"],
                              (string)reader["Content"],
                              reader["StoredProcedure"] == DBNull.Value ? string.Empty : (string)reader["StoredProcedure"],
                              (DateTime)reader["DateAdded"],
                              (DateTime)reader["DateUpdated"],
                              reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
                              (bool)reader["ForAllAccount"]);
                            temp.ContentTypeID = reader["ContentTypeID"] == DBNull.Value ? null : (int?)reader["ContentTypeID"];
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


    public static int Content_Insert(Content p_Content)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Content_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);


                if (p_Content.AccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", p_Content.AccountID));

                if (p_Content.ContentTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nContentTypeID", p_Content.ContentTypeID));

                if (p_Content.ForAllAccount != null)
                    command.Parameters.Add(new SqlParameter("@bForAllAccount", p_Content.ForAllAccount));

                command.Parameters.Add(new SqlParameter("@sContentKey", p_Content.ContentKey));
                
                if (p_Content.Heading!=string.Empty)
                command.Parameters.Add(new SqlParameter("@sHeading", p_Content.Heading));

                command.Parameters.Add(new SqlParameter("@sContent", p_Content.ContentP));


                if (p_Content.StoredProcedure != string.Empty)
                command.Parameters.Add(new SqlParameter("@sStoredProcedure", p_Content.StoredProcedure));


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


    public static int Content_Update(Content p_Content)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Content_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nContentID", p_Content.ContentID));

                if (p_Content.ContentTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nContentTypeID", p_Content.ContentTypeID));

                if (p_Content.AccountID !=null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", p_Content.AccountID));

             
                if (p_Content.ForAllAccount != null)
                    command.Parameters.Add(new SqlParameter("@bForAllAccount", p_Content.ForAllAccount));

                command.Parameters.Add(new SqlParameter("@sContentKey", p_Content.ContentKey));

                if (p_Content.Heading != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sHeading", p_Content.Heading));

                    command.Parameters.Add(new SqlParameter("@sContent", p_Content.ContentP));


                if (p_Content.StoredProcedure != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sStoredProcedure", p_Content.StoredProcedure));

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


    //public static int Content_Update_Force(Content p_Content)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("Content_Update_Force", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nContentID", p_Content.ContentID));

    //            if (p_Content.AccountID != null)
    //                command.Parameters.Add(new SqlParameter("@nAccountID", p_Content.AccountID));

             
    //            if (p_Content.ForAllAccount != null)
    //                command.Parameters.Add(new SqlParameter("@bForAllAccount", p_Content.ForAllAccount));

    //            command.Parameters.Add(new SqlParameter("@sContentKey", p_Content.ContentKey));

    //            if (p_Content.Heading != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sHeading", p_Content.Heading));

    //                command.Parameters.Add(new SqlParameter("@sContent", p_Content.ContentP));


    //            if (p_Content.StoredProcedure != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sStoredProcedure", p_Content.StoredProcedure));

    //            connection.Open();
    //            command.ExecuteNonQuery();
    //            connection.Close();
    //            connection.Dispose();

    //            return 1;
    //        }
    //    }
    //}

    public static int Content_Delete(int nContentID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Content_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nContentID", nContentID));

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


    public static Content Content_Details(int nContentID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Content_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nContentID", nContentID));

                connection.Open();


                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Content temp = new Content(
                               (int)reader["ContentID"],
                               (string)reader["ContentKey"],
                               reader["Heading"] == DBNull.Value ? string.Empty : (string)reader["Heading"],
                                (string)reader["Content"],
                               reader["StoredProcedure"] == DBNull.Value ? string.Empty : (string)reader["StoredProcedure"],
                               (DateTime)reader["DateAdded"],
                               (DateTime)reader["DateUpdated"],
                               reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
                               (bool)reader["ForAllAccount"]);
                            temp.ContentTypeID = reader["ContentTypeID"] == DBNull.Value ? null : (int?)reader["ContentTypeID"];
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


    public static ContentType dbg_ContentType_Detail(int nContentTypeID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ContentType_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nContentTypeID", nContentTypeID));

                connection.Open();

                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ContentType temp = new ContentType(
                               (int)reader["ContentTypeID"],
                               (string)reader["ContentTypeKey"],
                               (string)reader["ContentTypeName"]);

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

    #region UserContent
    public static int ets_UserContent_Insert(UserContent p_UserContent)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserContent_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nUserID", p_UserContent.UserID));
                command.Parameters.Add(new SqlParameter("@nContentID", p_UserContent.ContentID));
                command.Parameters.Add(new SqlParameter("@bIsDefaultShow", p_UserContent.IsDefaultShow));

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


    public static int ets_UserContent_Update(UserContent p_UserContent)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserContent_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserContentID", p_UserContent.UserContentID));
                command.Parameters.Add(new SqlParameter("@nUserID", p_UserContent.UserID));
                command.Parameters.Add(new SqlParameter("@nContentID", p_UserContent.ContentID));
                command.Parameters.Add(new SqlParameter("@bIsDefaultShow", p_UserContent.IsDefaultShow));

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

    public static UserContent ets_UserContent_Details(int nUserID, int nContentID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserContent_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nUserID", nUserID));
                command.Parameters.Add(new SqlParameter("@nContentID", nContentID));
                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserContent temp = new UserContent(
                               (int)reader["UserContentID"],
                               (int)reader["UserID"],
                               (int)reader["ContentID"],
                                (bool)reader["IsDefaultShow"]);

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




    #region SearchCriteria

    public static int SearchCriteria_Insert(SearchCriteria p_SearchCriteria)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SearchCriteria_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@sSearchText", p_SearchCriteria.SearchText));


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


    public static SearchCriteria SearchCriteria_Detail(int nSearchCriteriaID)
    {
        SearchCriteria theSearchCriteria = SearchCriteria_Detail_1(nSearchCriteriaID);

        if (theSearchCriteria == null)
        {
            theSearchCriteria = dbg_XMLData_SearchCriteriaID(nSearchCriteriaID);
        }

        return theSearchCriteria;
    }



    public static SearchCriteria SearchCriteria_Detail_1(int nSearchCriteriaID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("SearchCriteria_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nSearchCriteriaID", nSearchCriteriaID));
                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SearchCriteria temp = new SearchCriteria(
                               (int)reader["SearchCriteriaID"],
                               reader["SearchText"] == DBNull.Value ? string.Empty : (string)reader["SearchText"]

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






    #region LookUpData

    public static LookUpData LookUpData_Detail(int nLookupDataID)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("LookUpData_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                
                command.Parameters.Add(new SqlParameter("@nLookupDataID", nLookupDataID));


                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LookUpData temp = new LookUpData(
                               (int)reader["LookupDataID"],
                               (int)reader["LookupTypeID"],
                               (string)reader["DisplayText"], (string)reader["Value"],
                               (DateTime)reader["DateAdded"],
                               (DateTime)reader["DateUpdated"]
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




    public static LookUpData LookUpData_Detail_ByValue(int nLookupTypeID, string sValue)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("LookUpData_Detail_ByValue", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nLookupTypeID", nLookupTypeID));
                command.Parameters.Add(new SqlParameter("@sValue", sValue));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LookUpData temp = new LookUpData(
                               (int)reader["LookupDataID"],
                               (int)reader["LookupTypeID"],
                               (string)reader["DisplayText"], (string)reader["Value"],
                               (DateTime)reader["DateAdded"],
                               (DateTime)reader["DateUpdated"]
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


    public static LookUpData LookUpData_Detail_ByDisplayValue(int nLookupTypeID, string sdisplaytext)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("LookUpData_Detail_ByDisplayValue", connection))
            {
                command.CommandType = CommandType.StoredProcedure;



                command.Parameters.Add(new SqlParameter("@nLookupTypeID", nLookupTypeID));
                command.Parameters.Add(new SqlParameter("@sdisplaytext", sdisplaytext));
                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LookUpData temp = new LookUpData(
                               (int)reader["LookupDataID"],
                               (int)reader["LookupTypeID"],
                               (string)reader["DisplayText"], (string)reader["Value"],
                               (DateTime)reader["DateAdded"],
                               (DateTime)reader["DateUpdated"]
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



    public static LookupType LookUpType_Detail(int nLookupTypeID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("LookUpType_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nLookupTypeID", nLookupTypeID));
                connection.Open();

                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LookupType temp = new LookupType(
                               (int)reader["LookupTypeID"],
                               (string)reader["LookupTypeName"],
                               (bool)reader["LockedValue"],
                               (DateTime)reader["DateAdded"],
                               (DateTime)reader["DateUpdated"]);

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



    public static DataTable LookUpData_Select(int? nLookupDataID, int? nLookupTypeID, string sDisplayText,
      string sValue, DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum,
        string sDisplayTextNotLike)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("LookUpData_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;



                if (nLookupDataID != null)
                    command.Parameters.Add(new SqlParameter("@nLookupDataID", nLookupDataID));

                if (nLookupTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nLookupTypeID", nLookupTypeID));

                if (sDisplayText != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sDisplayText", sDisplayText));

                if (sDisplayTextNotLike != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sDisplayTextNotLike", sDisplayTextNotLike));

                if (sValue != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValue", sValue));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "LookupDataID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                //connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

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
                if (ds != null && ds.Tables.Count > 1)
                {

                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }
            }


        }



    }



    public static int LookUpData_Insert(LookUpData p_LookUpData)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("LookUpData_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

               

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nLookupTypeID", p_LookUpData.LookupTypeID));
                command.Parameters.Add(new SqlParameter("@sDisplayText", p_LookUpData.DisplayText));
                command.Parameters.Add(new SqlParameter("@sValue", p_LookUpData.Value));

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

    public static int LookUpData_Update(LookUpData p_LookUpData)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("LookUpData_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nLookupDataID", p_LookUpData.LookupDataID));
                command.Parameters.Add(new SqlParameter("@nLookupTypeID", p_LookUpData.LookupTypeID));
                command.Parameters.Add(new SqlParameter("@sDisplayText", p_LookUpData.DisplayText));
                command.Parameters.Add(new SqlParameter("@sValue", p_LookUpData.Value));

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


    public static int LookUpData_Delete(int nLookupDataID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("LookUpData_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nLookupDataID", nLookupDataID));

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

    #endregion



    #region XMLData


    public static int dbg_XMLData_Insert(XMLData p_XMLData)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_XMLData_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@sXMLText", p_XMLData.XMLText));

                if (p_XMLData.SearchCriteriaID != null)
                    command.Parameters.Add(new SqlParameter("@nSearchCriteriaID", p_XMLData.SearchCriteriaID));

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


    public static int dbg_XMLData_Update(XMLData p_XMLData)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_XMLData_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

             

                command.Parameters.Add(new SqlParameter("@nXMLDataID", p_XMLData.XMLDataID));


                command.Parameters.Add(new SqlParameter("@sXMLText", p_XMLData.XMLText));

                if (p_XMLData.SearchCriteriaID != null)
                    command.Parameters.Add(new SqlParameter("@nSearchCriteriaID", p_XMLData.SearchCriteriaID));

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

    
    //public static XMLData dbg_XMLData_Detail(int nXMLDataID, SqlTransaction tn, SqlConnection cn)
    //{
    //    XMLData theXMLData = dbg_XMLData_Detail_1(nXMLDataID, tn, cn);

    //    if (theXMLData == null)
    //    {
    //        //try it from search criteria
    //        theXMLData = dbg_SearchCriteria_Detail(nXMLDataID, tn, cn);

    //    }
    //    return theXMLData;

    //}


    public static XMLData dbg_XMLData_Detail(int nXMLDataID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_XMLData_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nXMLDataID", nXMLDataID));

                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            XMLData temp = new XMLData(
                                (int)reader["XMLDataID"],
                              (string)reader["XMLText"],
                              reader["SearchCriteriaID"] == DBNull.Value ? null : (int?)reader["SearchCriteriaID"]);

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



    public static SearchCriteria dbg_XMLData_SearchCriteriaID(int nSearchCriteriaID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_XMLData_SearchCriteriaID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nSearchCriteriaID", nSearchCriteriaID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SearchCriteria temp = new SearchCriteria(
                                (int)reader["SearchCriteriaID"],
                              (string)reader["XMLText"]);

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

    public static XMLData dbg_SearchCriteria_Detail(int nSearchCriteriaID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("SearchCriteria_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nSearchCriteriaID", nSearchCriteriaID));

                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            XMLData temp = new XMLData(
                                null,
                              reader["SearchText"] == DBNull.Value ? string.Empty : (string)reader["SearchText"], (int)reader["SearchCriteriaID"]);

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
