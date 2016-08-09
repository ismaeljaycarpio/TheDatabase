using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;


/// <summary> MR: Dec-2010
/// Summary description for SecurityManager
/// </summary>
public class SecurityManager
{
   // No constractor



    public static string Account_SPAfterLogin(string spName, int? AccountID, int? UserID)
    {
        string strValue = "";


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand(spName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@Return", SqlDbType.VarChar);
                pRV.Size = 4000;
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                if (AccountID != null)
                    command.Parameters.Add(new SqlParameter("@AccountID", AccountID));
              
                if (UserID != null)
                    command.Parameters.Add(new SqlParameter("@UserID", UserID));

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    strValue = pRV.Value.ToString();
                }
                catch
                {

                }

                connection.Close();
                connection.Dispose();



                return strValue;
            }

        }



    }

    public static void ProcessLoginUserDefault(string strTableTableID, string strDisplayColumn, string strLinkedParentColumnID, string strUserID, ref string strValue, ref string strText)
    {

        string strSystemName = Common.GetValueFromSQL(@"SELECT TOP 1 SystemName FROM [Column] WHERE TableID=" + strTableTableID + @" AND ColumnType='dropdown' AND DisplayColumn IS NOT NULL
                        AND TableTableID=-1");
        string strColumnID = Common.GetValueFromSQL(@"SELECT TOP 1 ColumnID FROM [Column] WHERE TableID=" + strTableTableID + @" AND ColumnType='dropdown' AND DisplayColumn IS NOT NULL
                        AND TableTableID=-1");
        if (strSystemName != "")
        {

            Column theLinkedParentColumn = RecordManager.ets_Column_Details(int.Parse(strLinkedParentColumnID));

            strValue = Common.GetValueFromSQL("SELECT TOP 1 " + theLinkedParentColumn.SystemName + " FROM [Record] WHERE TableID=" + strTableTableID + " AND IsActive=1 AND "
                + strSystemName + "='" + strUserID + "'");
            if (strValue != "")
            {
                if (strDisplayColumn != "")
                {
                    string strFieldsToShow = RecordManager.fnReplaceDisplayColumns_NoAlias(int.Parse(strColumnID));
                    if (strFieldsToShow != "")
                    {
                        string strFilterValueSQL = "";
                        if (theLinkedParentColumn.SystemName.ToLower() == "recordid")
                        {
                            strFilterValueSQL = strValue;
                        }
                        else
                        {
                            strFilterValueSQL = "'" + strValue.Replace("'", "''") + "'";
                        }

                        //strFieldsToShow = strFieldsToShow.Replace("'", "''");

                        string strDisplayColumnText = Common.GetValueFromSQL("SELECT (" + strFieldsToShow + ") as DisplayColumnText FROM Record WHERE TableID=" + strTableTableID + " AND " + theLinkedParentColumn.SystemName + "=" + strFilterValueSQL);

                        if (strDisplayColumnText != "")
                        {
                            strText = strDisplayColumnText;
                        }
                    }

                }
            }

        }

    }




    public static void AddSpeedLog(SpeedLog theSpeedLog)
    {   


        if (HttpContext.Current.Session["RunSpeedLog"] != null)
        {
            if (HttpContext.Current.Request != null)
            {
                if (HttpContext.Current.Request.RawUrl != "")
                {
                    string strSessionID = "";

                    if (HttpContext.Current.Session != null)
                    {
                        strSessionID = HttpContext.Current.Session.SessionID;
                    }
                    //SpeedLog theSpeedLog = new SpeedLog(null, null, HttpContext.Current.Request.RawUrl, "", null, DateTime.Now, strSessionID);

                    if (theSpeedLog.PageUrl != "")
                        theSpeedLog.PageUrl = HttpContext.Current.Request.RawUrl;


                    theSpeedLog.Runtime = DateTime.Now;
                    theSpeedLog.SessionID = strSessionID;
                    SecurityManager.dbg_SpeedLog_Insert(theSpeedLog);
                }
            }
        }



    }



    public static int dbg_SpeedLog_Insert(SpeedLog p_SpeedLog)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_SpeedLog_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                if (p_SpeedLog.UserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", p_SpeedLog.UserID));

                if (p_SpeedLog.PageUrl != "")
                    command.Parameters.Add(new SqlParameter("@sPageUrl", p_SpeedLog.PageUrl));
                if (p_SpeedLog.FunctionName != "")
                    command.Parameters.Add(new SqlParameter("@sFunctionName", p_SpeedLog.FunctionName));
                if (p_SpeedLog.FunctionLineNumber != null)
                    command.Parameters.Add(new SqlParameter("@nFunctionLineNumber", p_SpeedLog.FunctionLineNumber));
                if (p_SpeedLog.Runtime != null)
                    command.Parameters.Add(new SqlParameter("@dRuntime", p_SpeedLog.Runtime));
                if (p_SpeedLog.SessionID != null)
                    command.Parameters.Add(new SqlParameter("@sSessionID", p_SpeedLog.SessionID));


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



   
    #region User


    public static int dbg_Users_Import(int nBatchID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Users_Import", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));

                SqlParameter pRV = new SqlParameter("@nNumberImported", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
                command.Parameters.Add(pRV);

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


    public static int dbg_User_Validation(int nBatchID)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_User_Validation", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));



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
  


    public static int User_Insert(User p_User)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("User_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                //command.Parameters.Add(new SqlParameter("@sFullName", p_User.FullName));
                //command.Parameters.Add(new SqlParameter("@sSalutation", p_User.Salutation));
                command.Parameters.Add(new SqlParameter("@sFirstName", p_User.FirstName));
                command.Parameters.Add(new SqlParameter("@sLastName", p_User.LastName));
                command.Parameters.Add(new SqlParameter("@sPhonenumber", p_User.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@sEmail", p_User.Email));
                command.Parameters.Add(new SqlParameter("@sPassword", p_User.Password));
                command.Parameters.Add(new SqlParameter("@bIsActive", p_User.IsActive));

                //command.Parameters.Add(new SqlParameter("@bIsAccountHolder", p_User.IsAccountHolder));
                //command.Parameters.Add(new SqlParameter("@bIsAdvancedSecurity", p_User.IsAdvancedSecurity));

                //command.Parameters.Add(new SqlParameter("@bIsDocSecurityAdvanced", p_User.IsDocSecurityAdvanced));
                //command.Parameters.Add(new SqlParameter("@sDocSecurityType", p_User.DocSecurityType));

                //command.Parameters.Add(new SqlParameter("@nDashBoardDocumentID", p_User.DashBoardDocumentID));
                //command.Parameters.Add(new SqlParameter("@nDataScopeColumnID", p_User.DataScopeColumnID));
                //command.Parameters.Add(new SqlParameter("@sDataScopeValue", p_User.DataScopeValue));

                //if (p_User.RoleGroupID!=null)
                //    command.Parameters.Add(new SqlParameter("@nRoleGroupID", p_User.RoleGroupID));

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(pRV.Value.ToString());
                }
                catch(Exception ex)
                {
                    connection.Close();
                    connection.Dispose();
                    if (ex.Message.IndexOf("UQ_User_Email") > -1 && HttpContext.Current != null & HttpContext.Current.Session!=null)
                    {
                        HttpContext.Current.Session["tdbmsgpb"] = "This email address is already used by someone.Please enter another email address";
                      
                    }
                    else
                    {
                        HttpContext.Current.Session["tdbmsg"] = ex.Message;
                    }

                }
                return -1;
            }
        
        }

    }




    public static int User_Update(User p_User)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserID", p_User.UserID));
                //command.Parameters.Add(new SqlParameter("@sFullName", p_User.FullName));              
                //command.Parameters.Add(new SqlParameter("@sSalutation", p_User.Salutation));
                command.Parameters.Add(new SqlParameter("@sFirstName", p_User.FirstName));
                command.Parameters.Add(new SqlParameter("@sLastName", p_User.LastName));
                command.Parameters.Add(new SqlParameter("@sPhonenumber", p_User.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@sEmail", p_User.Email));
                command.Parameters.Add(new SqlParameter("@sPassword", p_User.Password));
                command.Parameters.Add(new SqlParameter("@bIsActive", p_User.IsActive));
              
                //command.Parameters.Add(new SqlParameter("@bIsAdvancedSecurity", p_User.IsAdvancedSecurity));

                //command.Parameters.Add(new SqlParameter("@bIsDocSecurityAdvanced", p_User.IsDocSecurityAdvanced));
                //command.Parameters.Add(new SqlParameter("@sDocSecurityType", p_User.DocSecurityType));

                //command.Parameters.Add(new SqlParameter("@nDashBoardDocumentID", p_User.DashBoardDocumentID));


                //if (p_User.AlertSeen!=null) 
                //    command.Parameters.Add(new SqlParameter("@dateAlertSeen", p_User.AlertSeen));

                //command.Parameters.Add(new SqlParameter("@nDataScopeColumnID", p_User.DataScopeColumnID));
                //command.Parameters.Add(new SqlParameter("@sDataScopeValue", p_User.DataScopeValue));

                //if (p_User.RoleGroupID != null)
                //    command.Parameters.Add(new SqlParameter("@nRoleGroupID", p_User.RoleGroupID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    i = -1;
                    if (ex.Message.IndexOf("UQ_User_Email") > -1 && HttpContext.Current != null & HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session["tdbmsg"] = "This email address is already used by someone.Please enter another email address";

                    }
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }

    public static int User_Delete(int p_UserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserID", p_UserID));

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

    public static int User_UnDelete(int p_UserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_UnDelete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserID", p_UserID));

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


    //public static string User_SessionID_Get(int nUserID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("User_SessionID_Get", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

    //            connection.Open();
    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            System.Data.DataSet ds = new System.Data.DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();


    //            if (ds.Tables[0].Rows[0][0] == DBNull.Value)
    //            {
    //                return "";
    //            }
    //            else
    //            {
    //                return ds.Tables[0].Rows[0][0].ToString();
    //            }


               
    //        }
    //    }
    //}

    public static int User_SessionID_Update(int nUserID, string sSessionID, int nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_SessionID_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserID", nUserID));
                command.Parameters.Add(new SqlParameter("@sSessionID", sSessionID));
                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

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

    public static int User_LoginCount_Increment(int nUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_LoginCount_Increment", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

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



    //public static int User_LoginCount_Increment(int nUserID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("User_LoginCount_Increment", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;
    //        }
    //    }
    //}


    //public static User User_SelectLogin(string @sUserName, string @sPassword)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("User_SelectLogin", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@sUserName", sUserName));
    //            command.Parameters.Add(new SqlParameter("@sPassword", sPassword));

    //            connection.Open();
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    User temp = new User(
    //                        (int)reader["UserID"],
    //                        (string)reader["FullName"],                           
    //                        (string)reader["Salutation"],
    //                        (string)reader["Email"],
    //                        (string)reader["Password"],
    //                        (bool)reader["IsActive"],
    //                        (DateTime)reader["DateAdded"],
    //                        (DateTime)reader["DateUpdated"],
    //                        (int)reader["AccountID"], "", (bool)reader["IsAccountHolder"]);
    //                    return temp;
    //                }
    //            }
    //            return null;
    //        }
    //    }
    //}


    public static User User_LoginByEmail(string sEmail, string sPassword)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_LoginByEmail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@sEmail", sEmail));
                command.Parameters.Add(new SqlParameter("@sPassword", sPassword));

                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User temp = new User(
                                (int)reader["UserID"],
                                reader["FirstName"] == DBNull.Value ? string.Empty : (string)reader["FirstName"],
                                reader["LastName"] == DBNull.Value ? string.Empty : (string)reader["LastName"],
                                reader["PhoneNumber"] == DBNull.Value ? string.Empty : (string)reader["PhoneNumber"],
                                (string)reader["Email"],
                                (string)reader["Password"],
                                (bool)reader["IsActive"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            //temp.AlertSeen = reader["AlertSeen"] == DBNull.Value ? null : (DateTime?)reader["AlertSeen"];


                            //temp.IsDocSecurityAdvanced = reader["IsDocSecurityAdvanced"] == DBNull.Value ? null : (bool?)reader["IsDocSecurityAdvanced"];
                            //temp.DocSecurityType = reader["DocSecurityType"] == DBNull.Value ? "" : (string)reader["DocSecurityType"];

                            //temp.DashBoardDocumentID = reader["DashBoardDocumentID"] == DBNull.Value ? null : (int?)reader["DashBoardDocumentID"];

                            //temp.DataScopeColumnID = reader["DataScopeColumnID"] == DBNull.Value ? null : (int?)reader["DataScopeColumnID"];
                            //temp.DataScopeValue = reader["DataScopeValue"] == DBNull.Value ? "" : (string)reader["DataScopeValue"];

                            //temp.RoleGroupID = reader["RoleGroupID"] == DBNull.Value ? null : (int?)reader["RoleGroupID"];

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

   



    public static User User_ByEmail(string sEmail)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("User_ByEmail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@sEmail", sEmail));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User temp = new User(
                                (int)reader["UserID"],
                               reader["FirstName"] == DBNull.Value ? string.Empty : (string)reader["FirstName"],
                                reader["LastName"] == DBNull.Value ? string.Empty : (string)reader["LastName"],
                                reader["PhoneNumber"] == DBNull.Value ? string.Empty : (string)reader["PhoneNumber"],
                                (string)reader["Email"],
                                (string)reader["Password"],
                                (bool)reader["IsActive"],
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



    public static User User_By_Email(string strEmail)
    {

        string strUserID = Common.GetValueFromSQL("SELECT UserID FROM [User] WHERE Email='" + strEmail.Replace("'","''")+ "'");
        if (strUserID!="")
        {

            using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
            {
                using (SqlCommand command = new SqlCommand("User_Details", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@nUserID", int.Parse(strUserID)));
                    connection.Open();

                    try
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User temp = new User(
                                    (int)reader["UserID"],
                                    reader["FirstName"] == DBNull.Value ? string.Empty : (string)reader["FirstName"],
                                    reader["LastName"] == DBNull.Value ? string.Empty : (string)reader["LastName"],
                                    reader["PhoneNumber"] == DBNull.Value ? string.Empty : (string)reader["PhoneNumber"],
                                    (string)reader["Email"],
                                    (string)reader["Password"],
                                    (bool)reader["IsActive"],
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

        return null;
    }

    public static User User_Details(int nUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nUserID", nUserID));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User temp = new User(
                                (int)reader["UserID"],
                                reader["FirstName"] == DBNull.Value ? string.Empty : (string)reader["FirstName"],
                                reader["LastName"] == DBNull.Value ? string.Empty : (string)reader["LastName"],
                                reader["PhoneNumber"] == DBNull.Value ? string.Empty : (string)reader["PhoneNumber"],
                                (string)reader["Email"],
                                (string)reader["Password"],
                                (bool)reader["IsActive"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            //temp.AlertSeen = reader["AlertSeen"] == DBNull.Value ? null : (DateTime?)reader["AlertSeen"];

                            //temp.IsDocSecurityAdvanced = reader["IsDocSecurityAdvanced"] == DBNull.Value ? null : (bool?)reader["IsDocSecurityAdvanced"];
                            //temp.DocSecurityType = reader["DocSecurityType"] == DBNull.Value ? "" : (string)reader["DocSecurityType"];

                            //temp.DashBoardDocumentID = reader["DashBoardDocumentID"] == DBNull.Value ? null : (int?)reader["DashBoardDocumentID"];

                            //temp.DataScopeColumnID = reader["DataScopeColumnID"] == DBNull.Value ? null : (int?)reader["DataScopeColumnID"];
                            //temp.DataScopeValue = reader["DataScopeValue"] == DBNull.Value ? "" : (string)reader["DataScopeValue"];

                            //temp.RoleGroupID = reader["RoleGroupID"] == DBNull.Value ? null : (int?)reader["RoleGroupID"];


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

    public static User User_AccountHolder(int nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_AccountHolder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nAccountID ", nAccountID));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User temp = new User(
                                (int)reader["UserID"],
                                reader["FirstName"] == DBNull.Value ? string.Empty : (string)reader["FirstName"],
                                reader["LastName"] == DBNull.Value ? string.Empty : (string)reader["LastName"],
                                reader["PhoneNumber"] == DBNull.Value ? string.Empty : (string)reader["PhoneNumber"],
                                (string)reader["Email"],
                                (string)reader["Password"],
                                (bool)reader["IsActive"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            //temp.AlertSeen = reader["AlertSeen"] == DBNull.Value ? null : (DateTime?)reader["AlertSeen"];


                            //temp.IsDocSecurityAdvanced = reader["IsDocSecurityAdvanced"] == DBNull.Value ? null : (bool?)reader["IsDocSecurityAdvanced"];
                            //temp.DocSecurityType = reader["DocSecurityType"] == DBNull.Value ? "" : (string)reader["DocSecurityType"];

                            //temp.DashBoardDocumentID = reader["DashBoardDocumentID"] == DBNull.Value ? null : (int?)reader["DashBoardDocumentID"];

                            //temp.DataScopeColumnID = reader["DataScopeColumnID"] == DBNull.Value ? null : (int?)reader["DataScopeColumnID"];
                            //temp.DataScopeValue = reader["DataScopeValue"] == DBNull.Value ? "" : (string)reader["DataScopeValue"];
                            //temp.RoleGroupID = reader["RoleGroupID"] == DBNull.Value ? null : (int?)reader["RoleGroupID"];

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


    public static DataTable User_Select(int? nUserID, string sFirstName,
       string sLastName, string sPhoneNumber, string sEmail, string sPassword, bool? bIsActive,
       DateTime? dDateAdded, DateTime? dDateUpdated, int? nAccountID, string sOrder,
       string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nUserID !=null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

                if (sFirstName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sFirstName", sFirstName));

                if (sLastName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sLastName", sLastName));


                if (sPhoneNumber != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sPhoneNumber", sPhoneNumber));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (sEmail != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sEmail", sEmail));

                if (sPassword != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sPassword", sPassword));

                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "UserID"; sOrderDirection = "DESC"; }

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




    public static DataTable ets_User_ByAccount(int nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_User_ByAccount", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                
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

    public static List<User> User_ByRoleType(string sRoleType)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_ByRoleType", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@sRoleType", sRoleType));


                connection.Open();

                List<User> list = new List<User>();



                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User temp = new User(
                                (int)reader["UserID"],
                               reader["FirstName"] == DBNull.Value ? string.Empty : (string)reader["FirstName"],
                                reader["LastName"] == DBNull.Value ? string.Empty : (string)reader["LastName"],
                                reader["PhoneNumber"] == DBNull.Value ? string.Empty : (string)reader["PhoneNumber"],
                                (string)reader["Email"],
                                (string)reader["Password"],
                                (bool)reader["IsActive"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            //temp.AlertSeen = reader["AlertSeen"] == DBNull.Value ? null : (DateTime?)reader["AlertSeen"];

                            //temp.IsDocSecurityAdvanced = reader["IsDocSecurityAdvanced"] == DBNull.Value ? null : (bool?)reader["IsDocSecurityAdvanced"];
                            //temp.DocSecurityType = reader["DocSecurityType"] == DBNull.Value ? "" : (string)reader["DocSecurityType"];

                            //temp.DashBoardDocumentID = reader["DashBoardDocumentID"] == DBNull.Value ? null : (int?)reader["DashBoardDocumentID"];

                            //temp.DataScopeColumnID = reader["DataScopeColumnID"] == DBNull.Value ? null : (int?)reader["DataScopeColumnID"];
                            //temp.DataScopeValue = reader["DataScopeValue"] == DBNull.Value ? "" : (string)reader["DataScopeValue"];

                            //temp.RoleGroupID = reader["RoleGroupID"] == DBNull.Value ? null : (int?)reader["RoleGroupID"];

                            list.Add(temp);
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

     public static int ChangePassword(string strNewPassword, int iUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ChangePassword", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@Result", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
                
                command.Parameters.Add(pRV);
                //command.Parameters.Add(new SqlParameter("@OldPassword", strOldPassword));
                command.Parameters.Add(new SqlParameter("@Password", strNewPassword));
                command.Parameters.Add(new SqlParameter("@userId", iUserID));


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







    #endregion


    #region Account
    //public static int Account_Insert(Account p_Account)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("Account_Insert", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //            pRV.Direction = ParameterDirection.Output;
    //            command.Parameters.Add(pRV);
    //            command.Parameters.Add(new SqlParameter("@sAccountName", p_Account.AccountName));
               


    //            if (p_Account.Logo != null)
    //                command.Parameters.Add(new SqlParameter("@Logo", p_Account.Logo));

    //            if (p_Account.IsActive != null)
    //                command.Parameters.Add(new SqlParameter("@IsActive", p_Account.IsActive));

              
    //            if (p_Account.AccountTypeID != null)
    //                command.Parameters.Add(new SqlParameter("@iAccountTypeID", p_Account.AccountTypeID));

    //            if (p_Account.ExpiryDate != null)
    //                command.Parameters.Add(new SqlParameter("@dateExpiryDate", p_Account.ExpiryDate));


    //            if (p_Account.UseDefaultLogo != null)
    //                command.Parameters.Add(new SqlParameter("@bUseDefaultLogo", p_Account.UseDefaultLogo));

              

    //            if (p_Account.MapCentreLat != null)
    //                command.Parameters.Add(new SqlParameter("@dMapCentreLat", p_Account.MapCentreLat));
    //            if (p_Account.MapCentreLong != null)
    //                command.Parameters.Add(new SqlParameter("@dMapCentreLong", p_Account.MapCentreLong));
    //            if (p_Account.MapZoomLevel != null)
    //                command.Parameters.Add(new SqlParameter("@nMapZoomLevel", p_Account.MapZoomLevel));
               

    //            if (p_Account.MapDefaultTableID != null)
    //                command.Parameters.Add(new SqlParameter("@nMapDefaultTableID", p_Account.MapDefaultTableID));

    //            if (p_Account.OtherMapZoomLevel != null)
    //                command.Parameters.Add(new SqlParameter("@nOtherMapZoomLevel", p_Account.OtherMapZoomLevel));


    //            if (p_Account.CountryID != null)
    //                command.Parameters.Add(new SqlParameter("@nCountryID", p_Account.CountryID));

    //            if (p_Account.PhoneNumber != null)
    //                command.Parameters.Add(new SqlParameter("@sPhoneNumber", p_Account.PhoneNumber));

    //            if (p_Account.CreatedByWizard != null)
    //                command.Parameters.Add(new SqlParameter("@bCreatedByWizard", p_Account.CreatedByWizard));


    //            if (p_Account.OrganisationName != "")
    //                command.Parameters.Add(new SqlParameter("@sOrganisationName", p_Account.OrganisationName));





    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return int.Parse(pRV.Value.ToString());
    //        }
    //    }
    //}



    public static int Account_Insert(Account p_Account)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Account_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sAccountName", p_Account.AccountName));

                if (p_Account.Logo != null)
                    command.Parameters.Add(new SqlParameter("@Logo", p_Account.Logo));


                if (p_Account.IsActive != null)
                    command.Parameters.Add(new SqlParameter("@IsActive", p_Account.IsActive));

                if (p_Account.AccountTypeID != null)
                    command.Parameters.Add(new SqlParameter("@iAccountTypeID", p_Account.AccountTypeID));

                if (p_Account.ExpiryDate != null)
                    command.Parameters.Add(new SqlParameter("@dateExpiryDate", p_Account.ExpiryDate));

                if (p_Account.UseDefaultLogo != null)
                    command.Parameters.Add(new SqlParameter("@bUseDefaultLogo", p_Account.UseDefaultLogo));



                if (p_Account.MapCentreLat != null)
                    command.Parameters.Add(new SqlParameter("@dMapCentreLat", p_Account.MapCentreLat));
                if (p_Account.MapCentreLong != null)
                    command.Parameters.Add(new SqlParameter("@dMapCentreLong", p_Account.MapCentreLong));
                if (p_Account.MapZoomLevel != null)
                    command.Parameters.Add(new SqlParameter("@nMapZoomLevel", p_Account.MapZoomLevel));

                if (p_Account.MapDefaultTableID != null)
                    command.Parameters.Add(new SqlParameter("@nMapDefaultTableID", p_Account.MapDefaultTableID));

                if (p_Account.OtherMapZoomLevel != null)
                    command.Parameters.Add(new SqlParameter("@nOtherMapZoomLevel", p_Account.OtherMapZoomLevel));


                if (p_Account.CountryID != null)
                    command.Parameters.Add(new SqlParameter("@nCountryID", p_Account.CountryID));

                if (p_Account.PhoneNumber != null)
                    command.Parameters.Add(new SqlParameter("@sPhoneNumber", p_Account.PhoneNumber));

                if (p_Account.CreatedByWizard != null)
                    command.Parameters.Add(new SqlParameter("@bCreatedByWizard", p_Account.CreatedByWizard));

                if (p_Account.OrganisationName != "")
                    command.Parameters.Add(new SqlParameter("@sOrganisationName", p_Account.OrganisationName));

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



    public static int Account_Update(Account p_Account)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("Account_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

              
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Account.AccountID));
                command.Parameters.Add(new SqlParameter("@sAccountName", p_Account.AccountName));



                if (p_Account.Logo != null)
                    command.Parameters.Add(new SqlParameter("@Logo", p_Account.Logo));

                if (p_Account.IsActive != null)
                    command.Parameters.Add(new SqlParameter("@IsActive", p_Account.IsActive));


                if (p_Account.AccountTypeID != null)
                    command.Parameters.Add(new SqlParameter("@iAccountTypeID", p_Account.AccountTypeID));

                if (p_Account.ExpiryDate != null)
                    command.Parameters.Add(new SqlParameter("@dateExpiryDate", p_Account.ExpiryDate));

                if (p_Account.UseDefaultLogo != null)
                    command.Parameters.Add(new SqlParameter("@bUseDefaultLogo", p_Account.UseDefaultLogo));



                if (p_Account.MapCentreLat != null)
                    command.Parameters.Add(new SqlParameter("@dMapCentreLat", p_Account.MapCentreLat));
                if (p_Account.MapCentreLong != null)
                    command.Parameters.Add(new SqlParameter("@dMapCentreLong", p_Account.MapCentreLong));
                if (p_Account.MapZoomLevel != null)
                    command.Parameters.Add(new SqlParameter("@nMapZoomLevel", p_Account.MapZoomLevel));



                if (p_Account.MapDefaultTableID != null)
                    command.Parameters.Add(new SqlParameter("@nMapDefaultTableID", p_Account.MapDefaultTableID));

                if (p_Account.ExceedLastEmail != null)
                    command.Parameters.Add(new SqlParameter("@dateExceedLastEmail", p_Account.ExceedLastEmail));

                if (p_Account.OtherMapZoomLevel != null)
                    command.Parameters.Add(new SqlParameter("@nOtherMapZoomLevel", p_Account.OtherMapZoomLevel));


                if (p_Account.CountryID != null)
                    command.Parameters.Add(new SqlParameter("@nCountryID", p_Account.CountryID));

                if (p_Account.PhoneNumber != null)
                    command.Parameters.Add(new SqlParameter("@sPhoneNumber", p_Account.PhoneNumber));

                if (p_Account.CreatedByWizard != null)
                    command.Parameters.Add(new SqlParameter("@bCreatedByWizard", p_Account.CreatedByWizard));

                if (p_Account.ExtensionPacks != null)
                    command.Parameters.Add(new SqlParameter("@nExtensionPacks", p_Account.ExtensionPacks));
                if (p_Account.Alerts != null)
                    command.Parameters.Add(new SqlParameter("@bAlerts", p_Account.Alerts));
                if (p_Account.ReportGen != null)
                    command.Parameters.Add(new SqlParameter("@bReportGen", p_Account.ReportGen));



                if (p_Account.Layout != null)
                    command.Parameters.Add(new SqlParameter("@nLayout", p_Account.Layout));


                command.Parameters.Add(new SqlParameter("@bSystemAccount", p_Account.SystemAccount));
                //command.Parameters.Add(new SqlParameter("@sConfirmationCode", p_Account.ConfirmationCode));
                command.Parameters.Add(new SqlParameter("@nNextBilledAccountTypeID", p_Account.NextBilledAccountTypeID));
                command.Parameters.Add(new SqlParameter("@sClientRef", p_Account.ClientRef));
                command.Parameters.Add(new SqlParameter("@bGSTApplicable", p_Account.GSTApplicable));
                command.Parameters.Add(new SqlParameter("@sOrganisationName", p_Account.OrganisationName));
                command.Parameters.Add(new SqlParameter("@sBillingPhoneNumber", p_Account.BillingPhoneNumber));
                command.Parameters.Add(new SqlParameter("@sBillingAddress", p_Account.BillingAddress));
                command.Parameters.Add(new SqlParameter("@sBillingEmail", p_Account.BillingEmail));
                command.Parameters.Add(new SqlParameter("@nBillEveryXMonths", p_Account.BillEveryXMonths));
                command.Parameters.Add(new SqlParameter("@sPaymentMethod", p_Account.PaymentMethod));
                command.Parameters.Add(new SqlParameter("@sBillingFirstName", p_Account.BillingFirstName));
                command.Parameters.Add(new SqlParameter("@sBillingLastName", p_Account.BillingLastName));
                command.Parameters.Add(new SqlParameter("@sComment", p_Account.Comment));

                command.Parameters.Add(new SqlParameter("@nDefaultGraphOptionID", p_Account.DefaultGraphOptionID));

                command.Parameters.Add(new SqlParameter("@sMasterPage", p_Account.MasterPage));

                command.Parameters.Add(new SqlParameter("@bUseDataScope", p_Account.UseDataScope));
                command.Parameters.Add(new SqlParameter("@nDisplayTableID", p_Account.DisplayTableID));

                if (p_Account.HomeMenuCaption != "")
                    command.Parameters.Add(new SqlParameter("@sHomeMenuCaption", p_Account.HomeMenuCaption));

                if (p_Account.ShowOpenMenu != null)
                    command.Parameters.Add(new SqlParameter("@bShowOpenMenu", p_Account.ShowOpenMenu));

                if (p_Account.IsReportTopMenu != null)
                    command.Parameters.Add(new SqlParameter("@bIsReportTopMenu", p_Account.IsReportTopMenu));

                if (p_Account.UploadAfterVerificaition != null)
                    command.Parameters.Add(new SqlParameter("@bUploadAfterVerificaition", p_Account.UploadAfterVerificaition));



                if (p_Account.EmailCount != null)
                    command.Parameters.Add(new SqlParameter("@nEmailCount", p_Account.EmailCount));

                if (p_Account.SMSCount != null)
                    command.Parameters.Add(new SqlParameter("@nSMSCount", p_Account.SMSCount));

                if (p_Account.LabelOnTop != null)
                    command.Parameters.Add(new SqlParameter("@bLabelOnTop", p_Account.LabelOnTop));

                if (p_Account.HomePageLink != "")
                    command.Parameters.Add(new SqlParameter("@sHomePageLink", p_Account.HomePageLink));

                if (p_Account.ReportServer != "")
                    command.Parameters.Add(new SqlParameter("@sReportServer", p_Account.ReportServer));
                if (p_Account.ReportUser != "")
                    command.Parameters.Add(new SqlParameter("@sReportUser", p_Account.ReportUser));
                if (p_Account.ReportPW != "")
                    command.Parameters.Add(new SqlParameter("@sReportPW", p_Account.ReportPW));
                if (p_Account.ReportServerUrl != "")
                    command.Parameters.Add(new SqlParameter("@sReportServerUrl", p_Account.ReportServerUrl));


                if (p_Account.SMTPEmail != "")
                    command.Parameters.Add(new SqlParameter("@sSMTPEmail", p_Account.SMTPEmail));
                if (p_Account.SMTPUserName != "")
                    command.Parameters.Add(new SqlParameter("@sSMTPUserName", p_Account.SMTPUserName));
                if (p_Account.SMTPPassword != "")
                    command.Parameters.Add(new SqlParameter("@sSMTPPassword", p_Account.SMTPPassword));
                if (p_Account.SMTPPort != "")
                    command.Parameters.Add(new SqlParameter("@sSMTPPort", p_Account.SMTPPort));
                if (p_Account.SMTPServer != "")
                    command.Parameters.Add(new SqlParameter("@sSMTPServer", p_Account.SMTPServer));
                if (p_Account.SMTPSSL != "")
                    command.Parameters.Add(new SqlParameter("@sSMTPSSL", p_Account.SMTPSSL));
                if (p_Account.SMTPReplyToEmail != "")
                    command.Parameters.Add(new SqlParameter("@sSMTPReplyToEmail", p_Account.SMTPReplyToEmail));
                if (p_Account.POP3Email != "")
                    command.Parameters.Add(new SqlParameter("@sPOP3Email", p_Account.POP3Email));
                if (p_Account.POP3UserName != "")
                    command.Parameters.Add(new SqlParameter("@sPOP3UserName", p_Account.POP3UserName));
                if (p_Account.POP3Password != "")
                    command.Parameters.Add(new SqlParameter("@sPOP3Password", p_Account.POP3Password));
                if (p_Account.POP3Port != "")
                    command.Parameters.Add(new SqlParameter("@sPOP3Port", p_Account.POP3Port));
                if (p_Account.POP3Server != "")
                    command.Parameters.Add(new SqlParameter("@sPOP3Server", p_Account.POP3Server));
                if (p_Account.POP3SSL != "")
                    command.Parameters.Add(new SqlParameter("@sPOP3SSL", p_Account.POP3SSL));


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

    public static List<Account> Account_Select(int? nAccountID, string sAccountName, 
       DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
       string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Account_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sAccountName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sAccountName", sAccountName));

               

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "AccountID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                connection.Open();

                List<Account> list = new List<Account>();


                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account temp = new Account(
                                (int)reader["AccountID"],
                                (string)reader["AccountName"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"],
                                  reader["AccountTypeID"] == DBNull.Value ? null : (int?)reader["AccountTypeID"],
                            reader["ExpiryDate"] == DBNull.Value ? null : (DateTime?)reader["ExpiryDate"]
                                );

                            //temp.Logo = reader["Logo"] == DBNull.Value ? null : (object)reader["Logo"];
                            temp.IsActive = reader["IsActive"] == DBNull.Value ? null : (bool?)reader["IsActive"];


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



    public static DataTable Account_Summary(int? nAccountID, string sAccountName, string sName,
      string sEmail, string sPhoneNumber, bool? bIsActive,
      DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Account_Summary", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sAccountName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sAccountName", sAccountName));

                if (sName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sName", sName));

                if (sEmail != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sEmail", sEmail));


                if (sPhoneNumber != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sPhoneNumber", sPhoneNumber));
                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "AccountID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                

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
                if (ds == null) return null;

                if (ds.Tables.Count > 1)                {
                   
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


    public static int ets_TotalRecords(int? nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TotalRecords", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

               

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

                
                if (ds == null) return 0;

                return int.Parse(ds.Tables[0].Rows[0][0].ToString());



            }
        }
    }

    //public static int ets_TotalRecordsThisMonth(int? nAccountID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_TotalRecordsThisMonth", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;


    //           command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));               

    //            connection.Open();


    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            DataSet ds = new DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();

    //            return   int.Parse(ds.Tables[0].Rows[0][0].ToString());
               


    //        }
    //    }
    //}

    public static int Account_Delete(int nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Account_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

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



    public static int Account_SMS_Email_Count(int nAccountID,bool? bEmailCount,bool? bSMSCount)
    {
        if (bEmailCount == null && bSMSCount == null)
            return 0;


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Account_SMS_Email_Count", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (bEmailCount != null)
                    command.Parameters.Add(new SqlParameter("@bEmailCount", bEmailCount));

                if (bSMSCount != null)
                    command.Parameters.Add(new SqlParameter("@bSMSCount", bSMSCount));

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

       try
       {
           //send email

           Account theAccount = SecurityManager.Account_Details(nAccountID);
           if (theAccount.AccountTypeID != null)
           {
               AccountType theAccountType = SecurityManager.AccountType_Details((int)theAccount.AccountTypeID);
               bool bSendOverUsage = true;
               if (theAccount != null && theAccount.ExceedLastEmail != null && theAccount.ExceedLastEmail.Value.AddDays(1) > DateTime.Now)
               {
                   bSendOverUsage = false;
               }
               bool bAccountUpdatedExceedLast = false;
               if (bSendOverUsage == true && theAccountType.MaxEmailsPerMonth < theAccount.EmailCount && bEmailCount != null)
               {
                   Content theContent = SystemData.Content_Details_ByKey("Over_Usage_Alert_Email", null);

                   if (theContent != null)
                   {
                       theContent.ContentP = theContent.ContentP.Replace("[AccountID]", theAccount.AccountID.ToString());
                       theContent.ContentP = theContent.ContentP.Replace("[EmailCount]", theAccount.EmailCount.ToString());
                       theContent.ContentP = theContent.ContentP.Replace("[MaxEmailsPerMonth]", theAccountType.MaxEmailsPerMonth.ToString());
                       string sFrom = SystemData.SystemOption_ValueByKey_Account_Default("EmailFrom", nAccountID, null, "no-reply@dbgurus.com.au");
                       string strOut = "";
                       DBGurus.SendEmail(theContent.ContentKey, null, null, theContent.Heading, theContent.ContentP,
                           sFrom, DBGurus.strGodEmail, "", "", null, null, out strOut); ;
                       theAccount.ExceedLastEmail = DateTime.Now;
                       SecurityManager.Account_Update(theAccount);
                       bAccountUpdatedExceedLast = true;
                   }
               }

               if (bSendOverUsage == true && theAccountType.MaxSMSPerMonth < theAccount.SMSCount && bSMSCount != null)
               {
                   Content theContent = SystemData.Content_Details_ByKey("Over_Usage_Alert_SMS", null);

                   if (theContent != null)
                   {
                       theContent.ContentP = theContent.ContentP.Replace("[AccountID]", theAccount.AccountID.ToString());
                       theContent.ContentP = theContent.ContentP.Replace("[SMSCount]", theAccount.SMSCount.ToString());
                       theContent.ContentP = theContent.ContentP.Replace("[MaxSMSPerMonth]", theAccountType.MaxSMSPerMonth.ToString());
                       string sFrom = SystemData.SystemOption_ValueByKey_Account_Default("EmailFrom", nAccountID, null, "no-reply@dbgurus.com.au");
                       string strOut = "";
                       DBGurus.SendEmail(theContent.ContentKey, null, null, theContent.Heading, theContent.ContentP, sFrom,
                           DBGurus.strGodEmail, "", "", null, null, out strOut);

                       if (bAccountUpdatedExceedLast==false)
                       {
                           theAccount.ExceedLastEmail = DateTime.Now;
                           SecurityManager.Account_Update(theAccount);
                       }
                      
                      
                   }
               }

           }

       }
       catch
       {
           //
       }

       return 1;
    }


    //public static string CheckIsClient(int? nAccountID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("CheckIsClient", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;


    //            command.Parameters.Add(new SqlParameter("@nAccountID ", nAccountID));

    //            connection.Open();


    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            DataSet ds = new DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();

    //            return ds.Tables[0].Rows[0][0].ToString();



    //        }
    //    }
    //}

    public static int Account_UnDelete(int nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Account_UnDelete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

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


    public static int ets_Generate_Invoices()
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Generate_Invoices", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

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


    public static string GetUserRoleTypeID(int iUserID, int iAccountID)
    {

        string strGOD = SystemData.SystemOption_ValueByKey_Account("GlobalAdminUserEmail", null, null);

          if(strGOD!="")
          {
              User theUser = SecurityManager.User_Details(iUserID);
              if(theUser!=null)
              {

                  if(theUser.Email.ToLower()==strGOD.ToLower())
                  {
                      return "1";
                  }
              }

          }

        DataTable dtTemp = Common.DataTableFromText(" SELECT RoleID FROM UserRole WHERE UserID=" + iUserID.ToString() + " AND AccountID=" + iAccountID.ToString());
        if (dtTemp.Rows.Count > 0)
        {
            if (dtTemp.Rows[0][0] != DBNull.Value)
            {

                DataTable dtRole = Common.DataTableFromText(" SELECT RoleType FROM [Role] WHERE RoleID=" + dtTemp.Rows[0][0].ToString());

                if (dtRole.Rows.Count > 0)
                {
                    if (dtRole.Rows[0][0] != DBNull.Value)
                    {
                        return dtRole.Rows[0][0].ToString();
                    }

                }

            }
        }
        return "";

    }



    public static UserRole GetUserRole(int iUserID, int iAccountID)
    {
        
        DataTable dtTemp = Common.DataTableFromText(" SELECT UserRoleID FROM UserRole WHERE UserID=" + iUserID.ToString() + " AND AccountID=" + iAccountID.ToString());
        if (dtTemp.Rows.Count > 0)
        {
            if (dtTemp.Rows[0][0] != DBNull.Value)
            {
                UserRole theUserRole=SecurityManager.UserRole_Details(int.Parse( dtTemp.Rows[0][0].ToString()));
                
               if(theUserRole!=null)
               {
                   return theUserRole;
               }

            }
        }
        return null;

    }

    public static UserRole GetGlobalUserRole(int iUserID)
    {

        DataTable dtTemp = Common.DataTableFromText(" SELECT UserRoleID FROM UserRole WHERE UserID=" + iUserID.ToString());
        if (dtTemp.Rows.Count > 0)
        {
            if (dtTemp.Rows[0][0] != DBNull.Value)
            {
                UserRole theUserRole = SecurityManager.UserRole_Details(int.Parse(dtTemp.Rows[0][0].ToString()));

                if (theUserRole != null)
                {
                    return theUserRole;
                }

            }
        }
        return null;

    }


    public static int? GetPrimaryAccountID(int iUserID)
    {

        DataTable dtTemp = Common.DataTableFromText(" SELECT AccountID FROM UserRole WHERE UserID=" + iUserID.ToString() + " AND IsPrimaryAccount=1");
        if (dtTemp.Rows.Count > 0)
        {
            if (dtTemp.Rows[0][0] != DBNull.Value)
            {
                return int.Parse(dtTemp.Rows[0][0].ToString());
            }

        }
        else
        {
            DataTable dtTemp2 = Common.DataTableFromText(" SELECT AccountID FROM UserRole WHERE UserID=" + iUserID.ToString() );
            if (dtTemp2.Rows.Count > 0)
            {
                if (dtTemp2.Rows[0][0] != DBNull.Value)
                {
                    return int.Parse(dtTemp2.Rows[0][0].ToString());
                }

            }
        }
        return null;

    }

  

    public static Account Account_Details(int nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Account_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account temp = new Account(
                                (int)reader["AccountID"],
                                (string)reader["AccountName"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"],
                                reader["AccountTypeID"] == DBNull.Value ? null : (int?)reader["AccountTypeID"],
                            reader["ExpiryDate"] == DBNull.Value ? null : (DateTime?)reader["ExpiryDate"]
                                );
                            temp.Logo = reader["Logo"] == DBNull.Value ? null : (object)reader["Logo"];
                            temp.IsActive = reader["IsActive"] == DBNull.Value ? null : (bool?)reader["IsActive"];
                            temp.UseDefaultLogo = (bool)reader["UseDefaultLogo"];

                            temp.MapCentreLat = reader["MapCentreLat"] == DBNull.Value ? null : (double?)double.Parse(reader["MapCentreLat"].ToString());
                            temp.MapCentreLong = reader["MapCentreLong"] == DBNull.Value ? null : (double?)double.Parse(reader["MapCentreLong"].ToString());
                            temp.MapZoomLevel = reader["MapZoomLevel"] == DBNull.Value ? null : (int?)reader["MapZoomLevel"];
                            temp.MapDefaultTableID = reader["MapDefaultTableID"] == DBNull.Value ? null : (int?)reader["MapDefaultTableID"];
                            temp.ExceedLastEmail = reader["ExceedLastEmail"] == DBNull.Value ? null : (DateTime?)reader["ExceedLastEmail"];

                            temp.OtherMapZoomLevel = reader["OtherMapZoomLevel"] == DBNull.Value ? null : (int?)reader["OtherMapZoomLevel"];

                            temp.CountryID = reader["CountryID"] == DBNull.Value ? null : (int?)reader["CountryID"];
                            temp.PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? "" : (string)reader["PhoneNumber"];

                            temp.CreatedByWizard = reader["CreatedByWizard"] == DBNull.Value ? null : (bool?)reader["CreatedByWizard"];


                            temp.ExtensionPacks = reader["ExtensionPacks"] == DBNull.Value ? null : (int?)reader["ExtensionPacks"];
                            temp.Alerts = reader["Alerts"] == DBNull.Value ? null : (bool?)reader["Alerts"];
                            temp.ReportGen = reader["ReportGen"] == DBNull.Value ? null : (bool?)reader["ReportGen"];


                            temp.Layout = reader["Layout"] == DBNull.Value ? null : (int?)reader["Layout"];

                            temp.SystemAccount = reader["SystemAccount"] == DBNull.Value ? null : (bool?)reader["SystemAccount"];
                            temp.ConfirmationCode = reader["ConfirmationCode"] == DBNull.Value ? "" : (string)reader["ConfirmationCode"];
                            temp.NextBilledAccountTypeID = reader["NextBilledAccountTypeID"] == DBNull.Value ? null : (int?)reader["NextBilledAccountTypeID"];
                            temp.ClientRef = reader["ClientRef"] == DBNull.Value ? "" : (string)reader["ClientRef"];
                            temp.GSTApplicable = reader["GSTApplicable"] == DBNull.Value ? null : (bool?)reader["GSTApplicable"];
                            temp.OrganisationName = reader["OrganisationName"] == DBNull.Value ? "" : (string)reader["OrganisationName"];
                            temp.BillingPhoneNumber = reader["BillingPhoneNumber"] == DBNull.Value ? "" : (string)reader["BillingPhoneNumber"];
                            temp.BillingAddress = reader["BillingAddress"] == DBNull.Value ? "" : (string)reader["BillingAddress"];
                            temp.BillingEmail = reader["BillingEmail"] == DBNull.Value ? "" : (string)reader["BillingEmail"];
                            temp.BillEveryXMonths = reader["BillEveryXMonths"] == DBNull.Value ? null : (int?)(byte)reader["BillEveryXMonths"];
                            temp.PaymentMethod = reader["PaymentMethod"] == DBNull.Value ? "" : (string)reader["PaymentMethod"];
                            temp.BillingFirstName = reader["BillingFirstName"] == DBNull.Value ? "" : (string)reader["BillingFirstName"];
                            temp.BillingLastName = reader["BillingLastName"] == DBNull.Value ? "" : (string)reader["BillingLastName"];
                            temp.Comment = reader["Comment"] == DBNull.Value ? "" : (string)reader["Comment"];
                            temp.DefaultGraphOptionID = reader["DefaultGraphOptionID"] == DBNull.Value ? null : (int?)reader["DefaultGraphOptionID"];

                            temp.CopyRightInfo = reader["CopyRightInfo"] == DBNull.Value ? "" : (string)reader["CopyRightInfo"];

                            temp.LoginContentID = reader["LoginContentID"] == DBNull.Value ? null : (int?)reader["LoginContentID"];
                            temp.HideMyAccount = reader["HideMyAccount"] == DBNull.Value ? null : (bool?)reader["HideMyAccount"];
                            temp.HideDashBoard = reader["HideDashBoard"] == DBNull.Value ? null : (bool?)reader["HideDashBoard"];
                            temp.MasterPage = reader["MasterPage"] == DBNull.Value ? "" : (string)reader["MasterPage"];
                            temp.UseDataScope = reader["UseDataScope"] == DBNull.Value ? null : (bool?)reader["UseDataScope"];
                            temp.DisplayTableID = reader["DisplayTableID"] == DBNull.Value ? null : (int?)reader["DisplayTableID"];

                            temp.HomeMenuCaption = reader["HomeMenuCaption"] == DBNull.Value ? "" : (string)reader["HomeMenuCaption"];
                            temp.ShowOpenMenu = reader["ShowOpenMenu"] == DBNull.Value ? null : (bool?)reader["ShowOpenMenu"];

                            temp.IsReportTopMenu = reader["IsReportTopMenu"] == DBNull.Value ? null : (bool?)reader["IsReportTopMenu"];
                            temp.UploadAfterVerificaition = reader["UploadAfterVerificaition"] == DBNull.Value ? null : (bool?)reader["UploadAfterVerificaition"];

                            temp.SMSCount = reader["SMSCount"] == DBNull.Value ? null : (int?)reader["SMSCount"];
                            temp.EmailCount = reader["EmailCount"] == DBNull.Value ? null : (int?)reader["EmailCount"];

                            temp.LabelOnTop = reader["LabelOnTop"] == DBNull.Value ? null : (bool?)reader["LabelOnTop"];
                            temp.HomePageLink = reader["HomePageLink"] == DBNull.Value ? "" : (string)reader["HomePageLink"];

                            temp.ReportServer = reader["ReportServer"] == DBNull.Value ? "" : (string)reader["ReportServer"];
                            temp.ReportUser = reader["ReportUser"] == DBNull.Value ? "" : (string)reader["ReportUser"];
                            temp.ReportPW = reader["ReportPW"] == DBNull.Value ? "" : (string)reader["ReportPW"];
                            temp.ReportServerUrl = reader["ReportServerUrl"] == DBNull.Value ? "" : (string)reader["ReportServerUrl"];
                            temp.SMTPEmail = reader["SMTPEmail"] == DBNull.Value ? "" : (string)reader["SMTPEmail"];
                            temp.SMTPUserName = reader["SMTPUserName"] == DBNull.Value ? "" : (string)reader["SMTPUserName"];
                            temp.SMTPPassword = reader["SMTPPassword"] == DBNull.Value ? "" : (string)reader["SMTPPassword"];
                            temp.SMTPPort = reader["SMTPPort"] == DBNull.Value ? "" : (string)reader["SMTPPort"];
                            temp.SMTPServer = reader["SMTPServer"] == DBNull.Value ? "" : (string)reader["SMTPServer"];
                            temp.SMTPSSL = reader["SMTPSSL"] == DBNull.Value ? "" : (string)reader["SMTPSSL"];
                            temp.SMTPReplyToEmail = reader["SMTPReplyToEmail"] == DBNull.Value ? "" : (string)reader["SMTPReplyToEmail"];
                            temp.POP3Email = reader["POP3Email"] == DBNull.Value ? "" : (string)reader["POP3Email"];
                            temp.POP3UserName = reader["POP3UserName"] == DBNull.Value ? "" : (string)reader["POP3UserName"];
                            temp.POP3Password = reader["POP3Password"] == DBNull.Value ? "" : (string)reader["POP3Password"];
                            temp.POP3Port = reader["POP3Port"] == DBNull.Value ? "" : (string)reader["POP3Port"];
                            temp.POP3Server = reader["POP3Server"] == DBNull.Value ? "" : (string)reader["POP3Server"];
                            temp.POP3SSL = reader["POP3SSL"] == DBNull.Value ? "" : (string)reader["POP3SSL"];

                            temp.SPAfterLogin = reader["SPAfterLogin"] == DBNull.Value ? "" : (string)reader["SPAfterLogin"];

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


    public static bool IsRecordsExceeded(int iAccountID)
    {
        Account theAccount = SecurityManager.Account_Details(iAccountID);
                
           if (theAccount.AccountTypeID != null)
                {

                    AccountType theAccountType = SecurityManager.AccountType_Details((int)theAccount.AccountTypeID);


                    //for stantard and large
                    if (theAccountType != null &&  theAccountType.MaxTotalRecords != null)
                    {

                        int iTotalRecords = 0;
                        iTotalRecords = SecurityManager.ets_TotalRecords(iAccountID);

                        if (iTotalRecords > 0)
                        {
                            //we have Records here

                            if (theAccountType.MaxTotalRecords != null)
                            {

                                if (iTotalRecords > (int)theAccountType.MaxTotalRecords)
                                {
                                    return true;
                                    //

                                }

                            }
                        }
                    }




                    //for free
                    //if (theAccountType != null && theAccountType.MaxRecordsPerMonth != null)
                    //{


                    //    int iTotalRecordsThisMonth = 0;
                    //    iTotalRecordsThisMonth = SecurityManager.ets_TotalRecordsThisMonth(iAccountID);

                    //    if (iTotalRecordsThisMonth > 0)
                    //    {
                    //        //we have Records here


                    //        if (theAccountType.MaxRecordsPerMonth != null)
                    //        {

                    //            if (iTotalRecordsThisMonth > (int)theAccountType.MaxRecordsPerMonth)
                    //            {

                    //                return true;
                    //            }

                    //        }
                    //    }
                    //}

                    

                }

           return false;
    }


//    public static void CheckRecordsExceededBU(int iAccountID)
//    {
//        Account theAccount = SecurityManager.Account_Details(iAccountID);

//        string strURL = "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath; 
//        string strDeveloperEmail = System.Configuration.ConfigurationManager.AppSettings["Coder"];
//        bool bSentEmailToClient = true;

//        if (strURL.IndexOf("test.") > -1 || strURL.IndexOf("prototype.") > -1)
//        {
//            bSentEmailToClient = false;
//        }

//        //check last time email

//        if (theAccount != null && theAccount.ExceedLastEmail != null)
//        {
//            if (theAccount.ExceedLastEmail.Value.AddDays(1) > DateTime.Now)
//            {
//                return;
//                //no need to send email, so return
//            }
//        }


//        if (theAccount.AccountTypeID != null)
//        {

//            AccountType theAccountType = SecurityManager.AccountType_Details((int)theAccount.AccountTypeID);


//            //for stantard and large
//            if (theAccountType != null && theAccountType.MaxRecordsPerMonth == null && theAccountType.MaxTotalRecords != null)
//            {


//                int iTotalRecords = 0;
//                iTotalRecords = SecurityManager.ets_TotalRecords(iAccountID);

//                if (iTotalRecords > 0)
//                {
//                    //we have Records here


//                    if (theAccountType.MaxTotalRecords != null)
//                    {

//                        if (iTotalRecords > (int)theAccountType.MaxTotalRecords)
//                        {
//                            //need to send email


//                            //lets send email

//                            DataTable dtUser = Common.DataTableFromText("SELECT UserID FROM [User] Where IsAccountHolder=1 AND AccountID=" + iAccountID.ToString());
//                            if (dtUser.Rows.Count > 0)
//                            {
//                                User theUser = SecurityManager.User_Details(int.Parse(dtUser.Rows[0][0].ToString()));

//                                if (theUser != null)
//                                {

//                                    string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
//                                    string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
//                                    string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
//                                    string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");

//                                    MailMessage msg = new MailMessage();
//                                    msg.From = new MailAddress(strEmail);

//                                    Content theContent = SystemData.Content_Details_ByKey("RecordsExceeded", null);


//                                    msg.Subject = theContent.Heading;
//                                    msg.IsBodyHtml = true;


//                                    theContent.ContentP = theContent.ContentP.Replace("[MaxRecords]", theAccountType.MaxTotalRecords.ToString());

//                                    theContent.ContentP = theContent.ContentP.Replace("[TotalRecords]", iTotalRecords.ToString());
//                                    theContent.ContentP = theContent.ContentP.Replace("[FullName]", theUser.FirstName + " " + theUser.LastName);

//                                    //lets get the account holder



//                                    msg.Body = theContent.ContentP;// Sb.ToString();


//                                    if (bSentEmailToClient)
//                                    {
//                                        msg.To.Add(theUser.Email);
//                                    }
//                                    else
//                                    {
//                                        msg.To.Add(strDeveloperEmail);
//                                    }
//                                    SmtpClient smtpClient = new SmtpClient(strEmailServer);
//                                    smtpClient.Timeout = 99999;
//                                    smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

//                                    smtpClient.Port = DBGurus.StringToInt(DBGurus.GetSystemOption("SmtpPort"));
//                                    smtpClient.EnableSsl = Convert.ToBoolean(DBGurus.GetSystemOption("EnableSSL"));

//                                    //Send BCC to Global user              

//                                    List<User> lstGlobalUser = SecurityManager.User_ByRoleType("1");

//                                    foreach (User oUser in lstGlobalUser)
//                                    {
//                                        MailAddress bcc = new MailAddress(oUser.Email);
//                                        msg.Bcc.Add(bcc);
//                                    }

//#if (!DEBUG)
//                                    smtpClient.Send(msg);
//#endif

//                                    //lets update the account

//                                    theAccount.ExceedLastEmail = DateTime.Now;

//                                    SecurityManager.Account_Update(theAccount, null);

//                                }

//                            }

//                        }

//                    }
//                }
//            }




//            //for free
//            if (theAccountType != null && theAccountType.MaxTotalRecords == null && theAccountType.MaxRecordsPerMonth != null)
//            {


//                int iTotalRecordsThisMonth = 0;
//                iTotalRecordsThisMonth = SecurityManager.ets_TotalRecordsThisMonth(iAccountID);

//                if (iTotalRecordsThisMonth > 0)
//                {
//                    //we have Records here


//                    if (theAccountType.MaxRecordsPerMonth != null)
//                    {

//                        if (iTotalRecordsThisMonth > (int)theAccountType.MaxRecordsPerMonth)
//                        {
//                            //need to send email


//                            //lets send email

//                            DataTable dtUser = Common.DataTableFromText("SELECT UserID FROM [User] Where IsAccountHolder=1 AND AccountID=" + iAccountID.ToString());
//                            if (dtUser.Rows.Count > 0)
//                            {
//                                User theUser = SecurityManager.User_Details(int.Parse(dtUser.Rows[0][0].ToString()));

//                                if (theUser != null)
//                                {

//                                    string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
//                                    string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
//                                    string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
//                                    string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");

//                                    MailMessage msg = new MailMessage();
//                                    msg.From = new MailAddress(strEmail);

//                                    Content theContent = SystemData.Content_Details_ByKey("RecordsPerMonthExceeded", null);


//                                    msg.Subject = theContent.Heading;
//                                    msg.IsBodyHtml = true;


//                                    theContent.ContentP = theContent.ContentP.Replace("[MaxRecordsPerMonth]", theAccountType.MaxRecordsPerMonth.ToString());

//                                    theContent.ContentP = theContent.ContentP.Replace("[ThisMonthRecords]", iTotalRecordsThisMonth.ToString());
//                                    theContent.ContentP = theContent.ContentP.Replace("[FullName]", theUser.FirstName + " " + theUser.LastName);

//                                    //lets get the account holder



//                                    msg.Body = theContent.ContentP;// Sb.ToString();
//                                    if (bSentEmailToClient)
//                                    {
//                                        msg.To.Add(theUser.Email);
//                                    }
//                                    else
//                                    {
//                                        msg.To.Add(strDeveloperEmail);
//                                    }
//                                    SmtpClient smtpClient = new SmtpClient(strEmailServer);
//                                    smtpClient.Timeout = 99999;
//                                    smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

//                                    smtpClient.Port = DBGurus.StringToInt(DBGurus.GetSystemOption("SmtpPort"));
//                                    smtpClient.EnableSsl = Convert.ToBoolean(DBGurus.GetSystemOption("EnableSSL"));

//                                    //Send BCC to Global user              

//                                    List<User> lstGlobalUser = SecurityManager.User_ByRoleType("1");

//                                    foreach (User oUser in lstGlobalUser)
//                                    {
//                                        MailAddress bcc = new MailAddress(oUser.Email);
//                                        msg.Bcc.Add(bcc);
//                                    }

//#if (!DEBUG)
//                                    smtpClient.Send(msg);
//#endif

//                                    //lets update the account

//                                    theAccount.ExceedLastEmail = DateTime.Now;

//                                    SecurityManager.Account_Update(theAccount, null);

//                                }

//                            }

//                        }

//                    }
//                }
//            }



//        }


//    }



    //public static bool IsAccountLimitExceeded(int iAccountID)
    //{
    //    Account theAccount = SecurityManager.Account_Details(iAccountID);

    //    if (theAccount.AccountTypeID != null)
    //    {

    //        AccountType theAccountType = SecurityManager.AccountType_Details((int)theAccount.AccountTypeID);


    //        //for stantard and large
    //        if (theAccountType != null && theAccountType.MaxRecordsPerMonth == null && theAccountType.MaxTotalRecords != null)
    //        {

    //            int iTotalRecords = 0;
    //            iTotalRecords = SecurityManager.ets_TotalRecords(iAccountID);

    //            if (iTotalRecords > 0)
    //            {
    //                //we have Records here
                    
    //                if (theAccountType.MaxTotalRecords != null)
    //                {

    //                    if (iTotalRecords > (int)theAccountType.MaxTotalRecords)
    //                    {
    //                        //need to send email
    //                        return true;

    //                    }
    //                }
    //            }
    //        }


    //        //for free
    //        if (theAccountType != null && theAccountType.MaxTotalRecords == null && theAccountType.MaxRecordsPerMonth != null)
    //        {


    //            int iTotalRecordsThisMonth = 0;
    //            iTotalRecordsThisMonth = SecurityManager.ets_TotalRecordsThisMonth(iAccountID);

    //            if (iTotalRecordsThisMonth > 0)
    //            {
    //                //we have Records here
    //                if (theAccountType.MaxRecordsPerMonth != null)
    //                {
    //                    if (iTotalRecordsThisMonth > (int)theAccountType.MaxRecordsPerMonth)
    //                    {
    //                        //need to send email

    //                        return true;
                           
    //                    }//

    //                }
    //            }
    //        }

    //    }

    //    return false;
    //}



  


    public static AccountType AccountType_Details(int nAccountTypeID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("AccountType_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nAccountTypeID", nAccountTypeID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AccountType temp = new AccountType(
                                (int)reader["AccountTypeID"],
                                (string)reader["AccountTypeName"],
                            reader["MaxTotalRecords"] == DBNull.Value ? null : (int?)reader["MaxTotalRecords"],
                             reader["MaxUsers"] == DBNull.Value ? null : (int?)reader["MaxUsers"]
                                );

                            temp.DiskSpaceMB = reader["DiskSpaceMB"] == DBNull.Value ? null : (int?)reader["DiskSpaceMB"];
                            temp.CostPerMonth = reader["CostPerMonth"] == DBNull.Value ? null : (double?)double.Parse(reader["CostPerMonth"].ToString());

                            temp.MaxEmailsPerMonth = reader["MaxEmailsPerMonth"] == DBNull.Value ? null : (int?)reader["MaxEmailsPerMonth"];
                            temp.MaxSMSPerMonth = reader["MaxSMSPerMonth"] == DBNull.Value ? null : (int?)reader["MaxSMSPerMonth"];

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

    public static string etsTerminology(string sPageName, string sInputText, string sOutputText)
    {
       
            using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
            {
                using (SqlCommand command = new SqlCommand("etsTerminology", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    int? nAccountID = null;
                    if (HttpContext.Current.Session["AccountID"] != null)
                    {
                        nAccountID = int.Parse(HttpContext.Current.Session["AccountID"].ToString());
                    }


                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                    if (sPageName != "")
                        command.Parameters.Add(new SqlParameter("@sPageName", sPageName));

                    command.Parameters.Add(new SqlParameter("@sInputText", sInputText));

                    connection.Open();


                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader[0] != DBNull.Value)
                                {
                                    string strOutputText = (string)reader[0];
                                    connection.Close();
                                    connection.Dispose();
                                    return strOutputText == "" ? sOutputText : strOutputText;
                                }
                            }

                        }
                    }
                    catch
                    {
                        //
                    }

                    
                    connection.Close();
                    connection.Dispose();
                    return sOutputText;

                }
            }
       
    }


    public static bool CanThisAccountAddUser(int iAccountID)
    {

        DataTable dtTemp = Common.DataTableFromText(@"SELECT * FROM (SELECT    AccountType.MaxUsers,(SELECT COUNT(*) FROM [User] INNER JOIN UserRole ON [User].UserID=UserRole.UserID
                                            WHERE [UserRole].AccountID=Account.AccountID and [User].IsActive=1 ) as CurrentUser
                                            FROM  Account INNER JOIN
                                            AccountType ON Account.AccountTypeID = AccountType.AccountTypeID 
                                            WHERE  Account.AccountID=" + iAccountID.ToString() + @" AND AccountType.MaxUsers is not null ) as AccountUserInFo
                                            WHERE CurrentUser>=MaxUsers");

        if (dtTemp.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion
    

    #region UserRole

    public static DataTable ets_UserRoleAccount_Select(int? nUserID,
   string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserRoleAccount_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "UserRoleID"; sOrderDirection = "ASC"; }

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


    public static UserRole UserRole_Details(int nUserRoleID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("UserRole_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nUserRoleID", nUserRoleID));
                connection.Open();

                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserRole temp = new UserRole(
                                (int)reader["UserRoleID"],
                                (int)reader["UserID"],
                                (int)reader["RoleID"],
                                 (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            temp.AccountID = (int)reader["AccountID"];
                            temp.IsPrimaryAccount = reader["IsPrimaryAccount"] == DBNull.Value ? null : (bool?)reader["IsPrimaryAccount"];

                            temp.IsAccountHolder = reader["IsAccountHolder"] == DBNull.Value ? null : (bool?)reader["IsAccountHolder"];
                            temp.IsAdvancedSecurity = reader["IsAdvancedSecurity"] == DBNull.Value ? null : (bool?)reader["IsAdvancedSecurity"];


                            temp.AlertSeen = reader["AlertSeen"] == DBNull.Value ? null : (DateTime?)reader["AlertSeen"];

                            temp.IsDocSecurityAdvanced = reader["IsDocSecurityAdvanced"] == DBNull.Value ? null : (bool?)reader["IsDocSecurityAdvanced"];
                            temp.DocSecurityType = reader["DocSecurityType"] == DBNull.Value ? "" : (string)reader["DocSecurityType"];

                            temp.DashBoardDocumentID = reader["DashBoardDocumentID"] == DBNull.Value ? null : (int?)reader["DashBoardDocumentID"];

                            temp.DataScopeColumnID = reader["DataScopeColumnID"] == DBNull.Value ? null : (int?)reader["DataScopeColumnID"];
                            temp.DataScopeValue = reader["DataScopeValue"] == DBNull.Value ? "" : (string)reader["DataScopeValue"];
                            //temp.AllowEditDashboard = reader["AllowEditDashboard"] == DBNull.Value ? null : (bool?)reader["AllowEditDashboard"];

                            temp.AllowDeleteTable = reader["AllowDeleteTable"] == DBNull.Value ? null : (bool?)reader["AllowDeleteTable"];
                            temp.AllowDeleteColumn = reader["AllowDeleteColumn"] == DBNull.Value ? null : (bool?)reader["AllowDeleteColumn"];
                            temp.AllowDeleteRecord = reader["AllowDeleteRecord"] == DBNull.Value ? null : (bool?)reader["AllowDeleteRecord"];






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

   



    public static int UserRole_Insert(UserRole p_UserRole)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("UserRole_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nUserID", p_UserRole.UserID));
                command.Parameters.Add(new SqlParameter("@nRoleID", p_UserRole.RoleID));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_UserRole.AccountID));
                command.Parameters.Add(new SqlParameter("@bIsPrimaryAccount", p_UserRole.IsPrimaryAccount));

                command.Parameters.Add(new SqlParameter("@bIsAccountHolder", p_UserRole.IsAccountHolder));
                command.Parameters.Add(new SqlParameter("@bIsAdvancedSecurity", p_UserRole.IsAdvancedSecurity));

                command.Parameters.Add(new SqlParameter("@bIsDocSecurityAdvanced", p_UserRole.IsDocSecurityAdvanced));
                command.Parameters.Add(new SqlParameter("@sDocSecurityType", p_UserRole.DocSecurityType));

                command.Parameters.Add(new SqlParameter("@nDashBoardDocumentID", p_UserRole.DashBoardDocumentID));

                command.Parameters.Add(new SqlParameter("@nDataScopeColumnID", p_UserRole.DataScopeColumnID));
                command.Parameters.Add(new SqlParameter("@sDataScopeValue", p_UserRole.DataScopeValue));

                if (p_UserRole.AllowDeleteTable!=null)
                    command.Parameters.Add(new SqlParameter("@AllowDeleteTable", p_UserRole.AllowDeleteTable));
                if (p_UserRole.AllowDeleteColumn != null)
                    command.Parameters.Add(new SqlParameter("@AllowDeleteColumn", p_UserRole.AllowDeleteColumn));
                if (p_UserRole.AllowDeleteRecord != null)
                    command.Parameters.Add(new SqlParameter("@AllowDeleteRecord", p_UserRole.AllowDeleteRecord));
                
                //if (p_UserRole.AllowEditDashboard != null)
                //    command.Parameters.Add(new SqlParameter("@bAllowEditDashboard", p_UserRole.AllowEditDashboard));

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


    public static int UserRole_Update(UserRole p_UserRole)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("UserRole_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserRoleID", p_UserRole.UserRoleID));
                command.Parameters.Add(new SqlParameter("@nUserID", p_UserRole.UserID));
                command.Parameters.Add(new SqlParameter("@nRoleID", p_UserRole.RoleID));

                command.Parameters.Add(new SqlParameter("@nAccountID", p_UserRole.AccountID));


                command.Parameters.Add(new SqlParameter("@bIsAdvancedSecurity", p_UserRole.IsAdvancedSecurity));

                command.Parameters.Add(new SqlParameter("@bIsDocSecurityAdvanced", p_UserRole.IsDocSecurityAdvanced));
                command.Parameters.Add(new SqlParameter("@sDocSecurityType", p_UserRole.DocSecurityType));

                command.Parameters.Add(new SqlParameter("@nDashBoardDocumentID", p_UserRole.DashBoardDocumentID));

                command.Parameters.Add(new SqlParameter("@nDataScopeColumnID", p_UserRole.DataScopeColumnID));
                command.Parameters.Add(new SqlParameter("@sDataScopeValue", p_UserRole.DataScopeValue));
                command.Parameters.Add(new SqlParameter("@dateAlertSeen", p_UserRole.AlertSeen));


                if (p_UserRole.AllowDeleteTable != null)
                    command.Parameters.Add(new SqlParameter("@AllowDeleteTable", p_UserRole.AllowDeleteTable));
                if (p_UserRole.AllowDeleteColumn != null)
                    command.Parameters.Add(new SqlParameter("@AllowDeleteColumn", p_UserRole.AllowDeleteColumn));
                if (p_UserRole.AllowDeleteRecord != null)
                    command.Parameters.Add(new SqlParameter("@AllowDeleteRecord", p_UserRole.AllowDeleteRecord));
                

                //if (p_UserRole.AllowEditDashboard != null)
                //    command.Parameters.Add(new SqlParameter("@bAllowEditDashboard", p_UserRole.AllowEditDashboard));

                //command.Parameters.Add(new SqlParameter("@dDateUpdated", p_UserRole.DateUpdated));

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

    public static int UserRole_Delete(int  p_UserRoleID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("UserRole_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserRoleID", p_UserRoleID));

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

  


    public static List<UserRole> UserRole_Select(int? nUserRoleID, int? nUserID,
     int? nRoleID, DateTime? dDateAdded,DateTime? dDateUpdated,  string sOrder,
   string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, int? nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("UserRole_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

                if (nUserRoleID != null)
                    command.Parameters.Add(new SqlParameter("@nUserRoleID", nUserRoleID));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

                if (nRoleID != null)
                    command.Parameters.Add(new SqlParameter("@nRoleID", nRoleID));


                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "UserRoleID"; sOrderDirection = "DESC"; }
                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                connection.Open();

                List<UserRole> list = new List<UserRole>();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserRole temp = new UserRole(
                                (int)reader["UserRoleID"],
                                (int)reader["UserID"],
                                (int)reader["RoleID"],
                                 (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]
                                 );


                            temp.AccountID = (int)reader["AccountID"];
                            temp.IsPrimaryAccount = reader["IsPrimaryAccount"] == DBNull.Value ? null : (bool?)reader["IsPrimaryAccount"];

                            temp.IsAccountHolder = reader["IsAccountHolder"] == DBNull.Value ? null : (bool?)reader["IsAccountHolder"];
                            temp.IsAdvancedSecurity = reader["IsAdvancedSecurity"] == DBNull.Value ? null : (bool?)reader["IsAdvancedSecurity"];
                            
                            temp.AlertSeen = reader["AlertSeen"] == DBNull.Value ? null : (DateTime?)reader["AlertSeen"];

                            temp.IsDocSecurityAdvanced = reader["IsDocSecurityAdvanced"] == DBNull.Value ? null : (bool?)reader["IsDocSecurityAdvanced"];
                            temp.DocSecurityType = reader["DocSecurityType"] == DBNull.Value ? "" : (string)reader["DocSecurityType"];

                            temp.DashBoardDocumentID = reader["DashBoardDocumentID"] == DBNull.Value ? null : (int?)reader["DashBoardDocumentID"];

                            temp.DataScopeColumnID = reader["DataScopeColumnID"] == DBNull.Value ? null : (int?)reader["DataScopeColumnID"];
                            temp.DataScopeValue = reader["DataScopeValue"] == DBNull.Value ? "" : (string)reader["DataScopeValue"];
                            //temp.AllowEditDashboard = reader["AllowEditDashboard"] == DBNull.Value ? null : (bool?)reader["AllowEditDashboard"];
                            temp.AllowDeleteTable = reader["AllowDeleteTable"] == DBNull.Value ? null : (bool?)reader["AllowDeleteTable"];
                            temp.AllowDeleteColumn = reader["AllowDeleteColumn"] == DBNull.Value ? null : (bool?)reader["AllowDeleteColumn"];
                            temp.AllowDeleteRecord = reader["AllowDeleteRecord"] == DBNull.Value ? null : (bool?)reader["AllowDeleteRecord"];


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



    #endregion


    #region Role

    public static Role Role_Details(int nRoleID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Role_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nRoleID", nRoleID));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Role temp = new Role(
                               (int)reader["RoleID"],
                               (string)reader["Role"],
                               (reader["RoleType"] == DBNull.Value) ? string.Empty : (string)reader["RoleType"],
                                (reader["RoleNotes"] == DBNull.Value) ? string.Empty : (string)reader["RoleNotes"],
                               (DateTime)reader["DateAdded"],
                               (DateTime)reader["DateUpdated"]);



                            temp.AccountID = (reader["AccountID"] == DBNull.Value) ? null : (int?)reader["AccountID"];
                            temp.IsSystemRole = (reader["IsSystemRole"] == DBNull.Value) ? null : (bool?)reader["IsSystemRole"];
                            temp.IsActive = (reader["IsActive"] == DBNull.Value) ? null : (bool?)reader["IsActive"];
                            temp.OwnerUserID = (reader["OwnerUserID"] == DBNull.Value) ? null : (int?)reader["OwnerUserID"];
                            temp.AllowEditDashboard = (reader["AllowEditDashboard"] == DBNull.Value) ? null : (bool?)reader["AllowEditDashboard"];
                            temp.DashboardDefaultFromUserID = (reader["DashboardDefaultFromUserID"] == DBNull.Value) ? null : (int?)reader["DashboardDefaultFromUserID"];
                            temp.AllowEditView = reader["AllowEditView"] == DBNull.Value ? null : (bool?)reader["AllowEditView"];
                            temp.ViewsDefaultFromUserID = reader["ViewsDefaultFromUserID"] == DBNull.Value ? null : (int?)reader["ViewsDefaultFromUserID"];


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



    public static List<Role> Role_Select(int? nRoleID, string sRole, string sRoleType,
        string sRoleNotes, DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum,
        int? nAccountID, bool? bIsSystemRole, bool? bIsActive)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Role_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (nRoleID != null)
                command.Parameters.Add(new SqlParameter("@nRoleID", nRoleID));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                if (bIsSystemRole != null)
                    command.Parameters.Add(new SqlParameter("@bIsSystemRole", bIsSystemRole));
                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));

                if (sRole != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sRole", sRole));
                if (sRoleType != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sRoleTye", sRoleType));
                if (sRoleNotes != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sRoleNotes", sRoleNotes));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "RoleID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                List<Role> list = new List<Role>();
                connection.Open();


                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Role temp = new Role(
                                (int)reader["RoleID"],
                                (string)reader["Role"],
                                (reader["RoleType"] == DBNull.Value) ? string.Empty : (string)reader["RoleType"],
                                 (reader["RoleNotes"] == DBNull.Value) ? string.Empty : (string)reader["RoleNotes"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            temp.AccountID = (reader["AccountID"] == DBNull.Value) ? null : (int?)reader["AccountID"];
                            temp.IsSystemRole = (reader["IsSystemRole"] == DBNull.Value) ? null : (bool?)reader["IsSystemRole"];
                            temp.IsActive = (reader["IsActive"] == DBNull.Value) ? null : (bool?)reader["IsActive"];
                            temp.OwnerUserID = (reader["OwnerUserID"] == DBNull.Value) ? null : (int?)reader["OwnerUserID"];
                            temp.AllowEditDashboard = (reader["AllowEditDashboard"] == DBNull.Value) ? null : (bool?)reader["AllowEditDashboard"];
                            temp.DashboardDefaultFromUserID = (reader["DashboardDefaultFromUserID"] == DBNull.Value) ? null : (int?)reader["DashboardDefaultFromUserID"];
                            temp.AllowEditView = reader["AllowEditView"] == DBNull.Value ? null : (bool?)reader["AllowEditView"];
                            temp.ViewsDefaultFromUserID = reader["ViewsDefaultFromUserID"] == DBNull.Value ? null : (int?)reader["ViewsDefaultFromUserID"];

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



    public static int Role_Insert(Role p_Role)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Role_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
               
                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sRole", p_Role.RoleName));
                command.Parameters.Add(new SqlParameter("@sRoleTye", p_Role.RoleType));
                command.Parameters.Add(new SqlParameter("@sRoleNotes", p_Role.RoleNotes));

                if (p_Role.AccountID!=null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", p_Role.AccountID));
                if (p_Role.IsSystemRole != null)
                    command.Parameters.Add(new SqlParameter("@bIsSystemRole", p_Role.IsSystemRole));
                if (p_Role.IsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", p_Role.IsActive));
                if (p_Role.OwnerUserID != null)
                    command.Parameters.Add(new SqlParameter("@nOwnerUserID", p_Role.OwnerUserID));

                if (p_Role.AllowEditDashboard != null)
                    command.Parameters.Add(new SqlParameter("@bAllowEditDashboard", p_Role.AllowEditDashboard));

                if (p_Role.DashboardDefaultFromUserID != null)
                    command.Parameters.Add(new SqlParameter("@nDashboardDefaultFromUserID", p_Role.DashboardDefaultFromUserID));

                if (p_Role.ViewsDefaultFromUserID != null)
                    command.Parameters.Add(new SqlParameter("@ViewsDefaultFromUserID", p_Role.ViewsDefaultFromUserID));
                if (p_Role.AllowEditView != null)
                    command.Parameters.Add(new SqlParameter("@AllowEditView", p_Role.AllowEditView));

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

    public static int Role_Update(Role p_Role)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Role_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nRoleID", p_Role.RoleID));
                command.Parameters.Add(new SqlParameter("@sRole", p_Role.RoleName));
                command.Parameters.Add(new SqlParameter("@sRoleTye", p_Role.RoleType));
                command.Parameters.Add(new SqlParameter("@sRoleNotes", p_Role.RoleNotes));

                if (p_Role.AccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", p_Role.AccountID));
                if (p_Role.IsSystemRole != null)
                    command.Parameters.Add(new SqlParameter("@bIsSystemRole", p_Role.IsSystemRole));
                if (p_Role.IsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", p_Role.IsActive));
                if (p_Role.OwnerUserID != null)
                    command.Parameters.Add(new SqlParameter("@nOwnerUserID", p_Role.OwnerUserID));

                if (p_Role.AllowEditDashboard != null)
                    command.Parameters.Add(new SqlParameter("@bAllowEditDashboard", p_Role.AllowEditDashboard));

                if (p_Role.DashboardDefaultFromUserID != null)
                    command.Parameters.Add(new SqlParameter("@nDashboardDefaultFromUserID", p_Role.DashboardDefaultFromUserID));

                if (p_Role.ViewsDefaultFromUserID != null)
                    command.Parameters.Add(new SqlParameter("@ViewsDefaultFromUserID", p_Role.ViewsDefaultFromUserID));
                if (p_Role.AllowEditView != null)
                    command.Parameters.Add(new SqlParameter("@AllowEditView", p_Role.AllowEditView));


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


    public static int Role_Delete(int nRoleID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Role_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nRoleID", nRoleID));

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


    #region Test

    public static int test_TestTable_InsertMillion(Menu p_Menu, string strTestText)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
            {


                connection.Open();
                try
                {

                    for (int i = 0; i < 250000; i++)
                    {

                        p_Menu.MenuP = strTestText + " " + i.ToString();
                        using (SqlCommand command = new SqlCommand("test_TestTable_Insert", connection))
                        {

                            command.CommandType = CommandType.StoredProcedure;
                            SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                            pRV.Direction = ParameterDirection.Output;
                            //pRV.Value = -1;
                            command.Parameters.Add(pRV);
                            command.Parameters.Add(new SqlParameter("@sMenu", p_Menu.MenuP));
                            command.Parameters.Add(new SqlParameter("@nAccountID", p_Menu.AccountID));
                            command.Parameters.Add(new SqlParameter("@bShowOnMenu", p_Menu.ShowOnMenu));

                            command.ExecuteNonQuery();



                        }

                    }

                    
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();


              

                return 1;//great!
            }
        }
        catch (Exception ex)
        {
        }

        return -1; //failed

    }

    public static int test_TestTable_Insert(Menu p_Menu)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("test_TestTable_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
                //pRV.Value = -1;
                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sMenu", p_Menu.MenuP));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Menu.AccountID));
                command.Parameters.Add(new SqlParameter("@bShowOnMenu", p_Menu.ShowOnMenu));

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

    public static int test_TestTable_Update(Menu p_Menu)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("test_TestTable_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nMenuID", p_Menu.MenuID));
                command.Parameters.Add(new SqlParameter("@sMenu", p_Menu.MenuP));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Menu.AccountID));
                command.Parameters.Add(new SqlParameter("@bShowOnMenu", p_Menu.ShowOnMenu));
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


    public static int test_TestTable_Delete(int iMenuID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("test_TestTable_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nMenuID", iMenuID));

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

    public static List<Menu> test_TestTable_Select(int? nMenuID, string sMenu,
     bool? bShowOnMenu, int? nAccountID, string sOrder,
   string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("test_TestTable_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

                if (nMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nMenuID", nMenuID));

                if ( !string.IsNullOrEmpty(sMenu))
                    command.Parameters.Add(new SqlParameter("@sMenu", sMenu));

                if (bShowOnMenu != null)
                    command.Parameters.Add(new SqlParameter("@bShowOnMenu", bShowOnMenu));


                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "MenuID"; sOrderDirection = "DESC"; }
                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                connection.Open();

                List<Menu> list = new List<Menu>();

                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Menu temp = new Menu(
                                (int)reader["MenuID"],
                                (string)reader["Menu"],
                                 reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
                                reader["ShowOnMenu"] == DBNull.Value ? null : (bool?)reader["ShowOnMenu"],
                                 (bool)reader["IsActive"]
                                 );
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



    #endregion 

   

    public static int dbg_RoleTable_Insert(RoleTable p_RoleTable)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_RoleTable_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nTableID", p_RoleTable.TableID));
                //command.Parameters.Add(new SqlParameter("@nUserID", p_RoleTable.UserID));
                command.Parameters.Add(new SqlParameter("@nRoleType", p_RoleTable.RoleType));

                if (p_RoleTable.CanExport!=null)
                command.Parameters.Add(new SqlParameter("@bCanExport", p_RoleTable.CanExport));
                if (p_RoleTable.RoleID != null)
                    command.Parameters.Add(new SqlParameter("@nRoleID", p_RoleTable.RoleID));

                if (p_RoleTable.ShowMenu != null)
                    command.Parameters.Add(new SqlParameter("@bShowMenu", p_RoleTable.ShowMenu));

                if (p_RoleTable.ViewsDefaultFromUserID != null)
                    command.Parameters.Add(new SqlParameter("@nViewsDefaultFromUserID", p_RoleTable.ViewsDefaultFromUserID));
                if (p_RoleTable.AllowEditView != null)
                    command.Parameters.Add(new SqlParameter("@bAllowEditView", p_RoleTable.AllowEditView));

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



    public static int dbg_RoleTable_Update(RoleTable p_RoleTable)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_RoleTable_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nRoleTableID", p_RoleTable.RoleTableID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_RoleTable.TableID));
                //command.Parameters.Add(new SqlParameter("@nUserID", p_RoleTable.UserID));
                command.Parameters.Add(new SqlParameter("@nRoleType", p_RoleTable.RoleType));

                if (p_RoleTable.CanExport != null)
                    command.Parameters.Add(new SqlParameter("@bCanExport", p_RoleTable.CanExport));

                if (p_RoleTable.RoleID != null)
                    command.Parameters.Add(new SqlParameter("@nRoleID", p_RoleTable.RoleID));

                if (p_RoleTable.ShowMenu != null)
                    command.Parameters.Add(new SqlParameter("@bShowMenu", p_RoleTable.ShowMenu));

                if (p_RoleTable.ViewsDefaultFromUserID != null)
                    command.Parameters.Add(new SqlParameter("@nViewsDefaultFromUserID", p_RoleTable.ViewsDefaultFromUserID));
                if (p_RoleTable.AllowEditView != null)
                    command.Parameters.Add(new SqlParameter("@bAllowEditView", p_RoleTable.AllowEditView));

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


    public static DataTable dbg_RoleTable_Select(int? nRoleTableID, int? nTableID,
    int? nRoleID,  int? nRoleType)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_RoleTable_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                int? nAccountID = null;
                if (HttpContext.Current.Session["AccountID"] != null)
                {
                    nAccountID = int.Parse(HttpContext.Current.Session["AccountID"].ToString());
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                }
                else
                {
                     if (nTableID != null)
                     {
                         Table theTable = RecordManager.ets_Table_Details((int)nTableID);
                         if(theTable!=null)
                             command.Parameters.Add(new SqlParameter("@nAccountID", theTable.AccountID));

                     }

                }
                if (nRoleTableID != null)
                    command.Parameters.Add(new SqlParameter("@nRoleTableID", nRoleTableID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nRoleID != null)
                    command.Parameters.Add(new SqlParameter("@nRoleID", nRoleID));

               
                if (nRoleType != null)
                    command.Parameters.Add(new SqlParameter("@nRoleType", nRoleType));
               

              
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



    public static int dbg_RoleTable_Delete(int nRoleTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_RoleTable_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nRoleTableID", nRoleTableID));

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

    public static RoleTable dbg_RoleTable_Detail(int nRoleTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_RoleTable_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nRoleTableID", nRoleTableID));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RoleTable temp = new RoleTable(
                                (int)reader["RoleTableID"],
                                (int)reader["TableID"],
                               reader["RoleType"] == DBNull.Value ? null : (int?)reader["RoleType"],
                            (DateTime)reader["DateAdded"],
                            (DateTime)reader["DateUpdated"]);

                            temp.CanExport = reader["CanExport"] == DBNull.Value ? null : (bool?)reader["CanExport"];
                            temp.RoleID = reader["RoleID"] == DBNull.Value ? null : (int?)reader["RoleID"];

                            temp.AllowEditView = reader["AllowEditView"] == DBNull.Value ? null : (bool?)reader["AllowEditView"];
                            temp.ViewsDefaultFromUserID = reader["ViewsDefaultFromUserID"] == DBNull.Value ? null : (int?)reader["ViewsDefaultFromUserID"];

                            temp.ShowMenu = reader["ShowMenu"] == DBNull.Value ? null : (bool?)reader["ShowMenu"];

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


    #region Contact



    public static int Contact_Insert(Contact p_Contact)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Contact_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@sEmail", p_Contact.Email));

                if (p_Contact.Name != "")
                    command.Parameters.Add(new SqlParameter("@sName", p_Contact.Name));

                if (p_Contact.Phone != "")
                    command.Parameters.Add(new SqlParameter("@sPhone", p_Contact.Phone));


                if (p_Contact.Message != "")
                    command.Parameters.Add(new SqlParameter("@sMessage", p_Contact.Message));

                command.Parameters.Add(new SqlParameter("@nContactTypeID", p_Contact.ContactTypeID));



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




    public static DataTable Contact_Select(int? nContactID, string sEmail,
       DateTime? dSubscriptionDate, string sOrder,
     string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Contact_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (nContactID != null)
                    command.Parameters.Add(new SqlParameter("@nContactID", nContactID));

                if (sEmail != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sEmail", sEmail));


                if (dSubscriptionDate != null)
                    command.Parameters.Add(new SqlParameter("@dSubscriptionDate", dSubscriptionDate));



                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "ContactID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


               
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


    public static int Contact_Delete(int nContactID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Contact_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nContactID", nContactID));

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



    public static DataTable User_PopUp_Select(string sSeacrh, int? nAccountID, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("User_PopUp_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;



                if (sSeacrh != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sSeacrh", sSeacrh));

               

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "UserID"; sOrderDirection = "DESC"; }

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





//    public static int Payment_Insert(Payment p_Payment, SqlTransaction tn)
//    {
//        SqlConnection connection;
//        if (tn == null)
//        {
//            connection = new SqlConnection(DBGurus.strGlobalConnectionString);
//            connection.Open();
//        }
//        else
//        {
//            connection = tn.Connection;
//        }




//        using (SqlCommand command = new SqlCommand("Payment_Insert", connection))
//        {

//            command.CommandType = CommandType.StoredProcedure;

//            if (tn != null)
//            {
//                command.Transaction = tn;
//            }

//            SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
//            pRV.Direction = ParameterDirection.Output;




//            command.Parameters.Add(pRV);
//            command.Parameters.Add(new SqlParameter("@nMonthsPaid", p_Payment.MonthsPaid));


//            command.Parameters.Add(new SqlParameter("@sPaymentType", p_Payment.PaymentType));

//            if (p_Payment.PaypalID != null)
//                command.Parameters.Add(new SqlParameter("@nPaypalID", p_Payment.PaypalID));

//            if (p_Payment.UserID != null)
//                command.Parameters.Add(new SqlParameter("@nUserID", p_Payment.UserID));

//            if (p_Payment.AccountID != null)
//                command.Parameters.Add(new SqlParameter("@nAccountID", p_Payment.AccountID));

//            if (p_Payment.IsManual != null)
//                command.Parameters.Add(new SqlParameter("@bIsManual", p_Payment.IsManual));

//            if (p_Payment.PaymentAmount != null)
//                command.Parameters.Add(new SqlParameter("@nPaymentAmount", p_Payment.PaymentAmount));

//            if (p_Payment.IsPaid != null)
//                command.Parameters.Add(new SqlParameter("@bIsPaid", p_Payment.IsPaid));
            
//            if (p_Payment.PaymentReceiveDate != null)
//                command.Parameters.Add(new SqlParameter("@dPaymentReceiveDate", p_Payment.PaymentReceiveDate));
            
//            if (p_Payment.CustomerInfo != null)
//                command.Parameters.Add(new SqlParameter("@sCustomerInfo", p_Payment.CustomerInfo));

//            if (p_Payment.PrimaryPhone != "")
//                command.Parameters.Add(new SqlParameter("@sPrimaryPhone", p_Payment.PrimaryPhone));
//            if (p_Payment.BackUpEmail != "")
//                command.Parameters.Add(new SqlParameter("@sBackUpEmail", p_Payment.BackUpEmail));
//            if (p_Payment.BillingAddress != "")
//                command.Parameters.Add(new SqlParameter("@sBillingAddress", p_Payment.BillingAddress));
            


//            //connection.Open();
//            command.ExecuteNonQuery();

//            if (tn == null)
//            {
//                connection.Close();
//                connection.Dispose();
//            }


//            return int.Parse(pRV.Value.ToString());
//        }

//    }





//    public static int Payment_Update(Payment p_Payment, SqlTransaction tn)
//    {

//        SqlConnection connection;
//        if (tn == null)
//        {
//            connection = new SqlConnection(DBGurus.strGlobalConnectionString);
//            connection.Open();
//        }
//        else
//        {
//            connection = tn.Connection;
//        }

//        using (SqlCommand command = new SqlCommand("Payment_Update", connection))
//        {

//            command.CommandType = CommandType.StoredProcedure;

//            if (tn != null)
//            {
//                command.Transaction = tn;
//            }

//            command.Parameters.Add(new SqlParameter("@nPaymentID", p_Payment.PaymentID));


//            command.Parameters.Add(new SqlParameter("@nMonthsPaid", p_Payment.MonthsPaid));


//            command.Parameters.Add(new SqlParameter("@sPaymentType", p_Payment.PaymentType));

//            if (p_Payment.PaypalID != null)
//                command.Parameters.Add(new SqlParameter("@nPaypalID", p_Payment.PaypalID));

//            if (p_Payment.UserID != null)
//                command.Parameters.Add(new SqlParameter("@nUserID", p_Payment.UserID));

//            if (p_Payment.AccountID != null)
//                command.Parameters.Add(new SqlParameter("@nAccountID", p_Payment.AccountID));

//            if (p_Payment.IsManual != null)
//                command.Parameters.Add(new SqlParameter("@bIsManual", p_Payment.IsManual));

//            if (p_Payment.PaymentAmount != null)
//                command.Parameters.Add(new SqlParameter("@nPaymentAmount", p_Payment.PaymentAmount));

//            if (p_Payment.IsPaid != null)
//                command.Parameters.Add(new SqlParameter("@bIsPaid", p_Payment.IsPaid));

//            if (p_Payment.PaymentReceiveDate != null)
//                command.Parameters.Add(new SqlParameter("@dPaymentReceiveDate", p_Payment.PaymentReceiveDate));

//            if (p_Payment.CustomerInfo != null)
//                command.Parameters.Add(new SqlParameter("@sCustomerInfo", p_Payment.CustomerInfo));

//            if (p_Payment.LastUpdatedUserID != null)
//                command.Parameters.Add(new SqlParameter("@nLastUpdatedUserID", p_Payment.LastUpdatedUserID));


//            if (p_Payment.PrimaryPhone != "")
//                command.Parameters.Add(new SqlParameter("@sPrimaryPhone", p_Payment.PrimaryPhone));
//            if (p_Payment.BackUpEmail != "")
//                command.Parameters.Add(new SqlParameter("@sBackUpEmail", p_Payment.BackUpEmail));
//            if (p_Payment.BillingAddress != "")
//                command.Parameters.Add(new SqlParameter("@sBillingAddress", p_Payment.BillingAddress));


//            command.ExecuteNonQuery();

//            if (tn == null)
//            {
//                connection.Close();
//                connection.Dispose();
//            }


//            return 1;

//        }

//    }





//    public static Payment Payment_Details(int nPaymentID, SqlTransaction tn, SqlConnection cn)
//    {
//        SqlConnection connection;
//        if (tn == null)
//        {
//            if (cn != null)
//            {
//                connection = cn;
//            }
//            else
//            {
//                connection = new SqlConnection(DBGurus.strGlobalConnectionString);
//                connection.Open();
//            }
//        }
//        else
//        {
//            if (cn != null)
//            {
//                connection = cn;
//            }
//            else
//            {
//                connection = tn.Connection;
//            }
//        }

//        using (SqlCommand command = new SqlCommand("Payment_Details", connection))
//        {
//            command.CommandType = CommandType.StoredProcedure;
//            if (tn != null)
//            {
//                command.Transaction = tn;
//            }

//            command.Parameters.Add(new SqlParameter("@nPaymentID", nPaymentID));

//            using (SqlDataReader reader = command.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    Payment temp = new Payment(
//                        (int)reader["PaymentID"],
//                        (int)reader["MonthsPaid"],
//                        (string)reader["PaymentType"],
//                        reader["PaypalID"] == DBNull.Value ? null : (int?)reader["PaypalID"],
//                        (int)reader["UserID"],
//                        (int)reader["AccountID"],
//                        (bool)reader["IsManual"],
//                        double.Parse(reader["PaymentAmount"].ToString()),
//                        (bool)reader["IsPaid"],
//                        reader["PaymentReceiveDate"] == DBNull.Value ? null : (DateTime?)reader["PaymentReceiveDate"],
//                         (string)reader["CustomerInfo"],
//                        (DateTime)reader["DateAdded"],
//                        (DateTime)reader["DateUpdated"]);
//                    temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
//                    temp.PrimaryPhone = reader["PrimaryPhone"] == DBNull.Value ? "" : (string)reader["PrimaryPhone"];
//                    temp.BackUpEmail = reader["BackUpEmail"] == DBNull.Value ? "" : (string)reader["BackUpEmail"];
//                    temp.BillingAddress = reader["BillingAddress"] == DBNull.Value ? "" : (string)reader["BillingAddress"];

//                    if (tn == null && cn == null)
//                    {
//                        connection.Close();
//                        connection.Dispose();
//                    }
//                    return temp;
//                }
//            }

//            if (tn == null && cn == null)
//            {
//                connection.Close();
//                connection.Dispose();
//            }
//            return null;
//        }

//    }



//    public static int Payment_Delete(int nPaymentID)
//    {
//        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
//        {
//            using (SqlCommand command = new SqlCommand("Payment_Delete", connection))
//            {

//                command.CommandType = CommandType.StoredProcedure;
//                command.Parameters.Add(new SqlParameter("@nPaymentID", nPaymentID));

//                connection.Open();
//                command.ExecuteNonQuery();

//                return 1;

//            }
//        }
//    }



//    public static DataTable Payment_Select(int? nPaymentID,
//int? nMonthsPaid, string sPaymentType, int? nPaypalID, int? nUserID,
//     int? nAccountID, bool? bIsManual, double? nPaymentAmount,bool? bIsPaid,
//        DateTime? dPaymentReceiveDateFrom,DateTime? dPaymentReceiveDateTo,
//         DateTime? dAddedDateFrom, DateTime? dAddedDateTo,string sAccountName,
//        string sEmail, DateTime? dExpiryFrom, DateTime? dExpiryDateTo, 
//    string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
//    {
//        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
//        {
//            using (SqlCommand command = new SqlCommand("Payment_Select", connection))
//            {
//                command.CommandType = CommandType.StoredProcedure;
//                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


//                if (nPaymentID != null)
//                    command.Parameters.Add(new SqlParameter("@nPaymentID", nPaymentID));

//                if (nMonthsPaid != null)
//                    command.Parameters.Add(new SqlParameter("@nMonthsPaid", nMonthsPaid));

//                if (sPaymentType != "")
//                    command.Parameters.Add(new SqlParameter("@sPaymentType", sPaymentType));

//                if (nPaypalID != null)
//                    command.Parameters.Add(new SqlParameter("@nPaypalID", nPaypalID));

//                if (nUserID != null)
//                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));


//                if (nAccountID != null)
//                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));


//                if (bIsManual != null)
//                    command.Parameters.Add(new SqlParameter("@bIsManual", bIsManual));

//                if (nPaymentAmount != null)
//                    command.Parameters.Add(new SqlParameter("@nPaymentAmount", nPaymentAmount));

//                if (bIsPaid != null)
//                    command.Parameters.Add(new SqlParameter("@bIsPaid", bIsPaid));

//                if (dPaymentReceiveDateFrom != null)
//                    command.Parameters.Add(new SqlParameter("@dPaymentReceiveDateFrom", dPaymentReceiveDateFrom));

//                if (dPaymentReceiveDateTo != null)
//                    command.Parameters.Add(new SqlParameter("@dPaymentReceiveDateTo", dPaymentReceiveDateTo));

//                if (dAddedDateFrom != null)
//                    command.Parameters.Add(new SqlParameter("@dAddedDateFrom", dAddedDateFrom));

//                if (dAddedDateTo != null)
//                    command.Parameters.Add(new SqlParameter("@dAddedDateTo", dAddedDateTo));


//                if (sAccountName != "")
//                    command.Parameters.Add(new SqlParameter("@sAccountName", sAccountName));

//                if (sEmail != "")
//                    command.Parameters.Add(new SqlParameter("@sEmail", sEmail));


//                if (dExpiryFrom != null)
//                    command.Parameters.Add(new SqlParameter("@dExpiryFrom", dExpiryFrom));

//                if (dExpiryDateTo != null)
//                    command.Parameters.Add(new SqlParameter("@dExpiryDateTo", dExpiryDateTo));




//                if (sOrder == "")
//                    sOrder = "PaymentID";

//                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

//                if (nStartRow != null)
//                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

//                if (nMaxRows != null)
//                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));



//                connection.Open();
//                SqlDataAdapter da = new SqlDataAdapter();
//                da.SelectCommand = command;
//                DataTable dt = new DataTable();
//                System.Data.DataSet ds = new System.Data.DataSet();
//                da.Fill(ds);
//                iTotalRowsNum = 0;
//                if (ds.Tables.Count > 1)
//                {
//                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
//                }
//                if (ds.Tables.Count > 0)
//                {
//                    return ds.Tables[0];
//                }
//                {
//                    return null;
//                }
//            }
//        }
//    }

    public static string GetPaypalReceiverEmail()
    {
        string strPayPalLive = SystemData.SystemOption_ValueByKey_Account("PayPalLive",null,null);

        if (bool.Parse(strPayPalLive))
        {
            return SystemData.SystemOption_ValueByKey_Account("PayPalLiveReceiverEmail",null,null); 
        }
        else
        {
            return SystemData.SystemOption_ValueByKey_Account("PayPalSandBoxReceiverEmail",null,null); 
        }

    }

    public static string GetPaypalActionURL()
    {
        string strPayPalLive = SystemData.SystemOption_ValueByKey_Account("PayPalLive",null,null);

        if (bool.Parse(strPayPalLive))
        {
            return "https://www.paypal.com/cgi-bin/webscr";
        }
        else
        {
            return "https://www.sandbox.paypal.com/cgi-bin/webscr";
        }

    }




    public static int PayPal_Insert(Paypal p_Paypal)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("PayPal_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@stxn_id", p_Paypal.txn_id));


                command.Parameters.Add(new SqlParameter("@spayment_status", p_Paypal.payment_status));

                if (p_Paypal.pending_reason != "")
                    command.Parameters.Add(new SqlParameter("@spending_reason", p_Paypal.pending_reason));


                command.Parameters.Add(new SqlParameter("@spayer_email", p_Paypal.payer_email));


                command.Parameters.Add(new SqlParameter("@sreceiver_email", p_Paypal.receiver_email));

                if (p_Paypal.mc_gross != null)
                    command.Parameters.Add(new SqlParameter("@nmc_gross", p_Paypal.mc_gross));

                if (p_Paypal.txn_type != null)
                    command.Parameters.Add(new SqlParameter("@stxn_type", p_Paypal.txn_type));


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



    public static Paypal PayPal_Details(int nPayPalID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("PayPal_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nPayPalID", nPayPalID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Paypal temp = new Paypal(
                                (int)reader["PaypalID"],
                                 (string)reader["txn_id"],
                                (string)reader["payment_status"],
                                reader["pending_reason"] == DBNull.Value ? null : (string)reader["pending_reason"],
                                 (string)reader["payer_email"],
                                   (string)reader["receiver_email"],
                                     double.Parse(reader["mc_gross"].ToString()),
                                      (string)reader["txn_type"],
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





    public static void Usage_Insert(Usage p_Usage)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("Usage_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;


                //SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                //pRV.Direction = ParameterDirection.Output;




                //command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@nAccountID", p_Usage.AccountID));

                command.Parameters.Add(new SqlParameter("@dDate", p_Usage.Date));

                command.Parameters.Add(new SqlParameter("@nSignedInCount", p_Usage.SignedInCount));

                command.Parameters.Add(new SqlParameter("@nUploadedCount", p_Usage.UploadedCount));

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

       

    }


    public static DataTable Usage_Select(int? nUsageID,
int? nAccountID, DateTime? dDate, bool? bSignedInCount, bool? bUploadedCount, 
        DateTime? dDateFrom,DateTime? dDateTo,
   string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Usage_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


                if (nUsageID != null)
                    command.Parameters.Add(new SqlParameter("@nUsageID", nUsageID));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (dDate != null)
                    command.Parameters.Add(new SqlParameter("@dDate", dDate));

                if (bSignedInCount != null)
                    command.Parameters.Add(new SqlParameter("@bSignedInCount", bSignedInCount));


                if (bUploadedCount != null)
                    command.Parameters.Add(new SqlParameter("@bUploadedCount", bUploadedCount));


                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));

                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));

               

                if (sOrder == "")
                    sOrder = "UsageID";

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





   
    

    public static int ets_Terminology_Insert(Terminology p_Terminology)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("ets_Terminology_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Terminology.AccountID));

                if (p_Terminology.PageName != "")
                    command.Parameters.Add(new SqlParameter("@sPageName", p_Terminology.PageName));

                command.Parameters.Add(new SqlParameter("@sInputText", p_Terminology.InputText));
                command.Parameters.Add(new SqlParameter("@sOutputText", p_Terminology.OutputText));



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


    public static int ets_Terminology_Update(Terminology p_Terminology)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_Terminology_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                                
                command.Parameters.Add(new SqlParameter("@nTerminologyID", p_Terminology.TerminologyID));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Terminology.AccountID));
                if (p_Terminology.PageName != "")
                    command.Parameters.Add(new SqlParameter("@sPageName", p_Terminology.PageName));

                command.Parameters.Add(new SqlParameter("@sInputText", p_Terminology.InputText));
                command.Parameters.Add(new SqlParameter("@sOutputText", p_Terminology.OutputText));


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



    public static DataTable ets_Terminology_Select(int? nAccountID, string sPageName, string sInputText,
       string sOutputText,  string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Terminology_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sPageName != "")
                    command.Parameters.Add(new SqlParameter("@sPageName", sPageName));

                if (sInputText != "")
                    command.Parameters.Add(new SqlParameter("@sInputText", sInputText));

                if (sOutputText != "")
                    command.Parameters.Add(new SqlParameter("@sOutputText", sOutputText));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "TerminologyID"; sOrderDirection = "DESC"; }

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




    public static int ets_Terminology_Delete(int nTerminologyID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Terminology_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTerminologyID ", nTerminologyID));

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




    public static Terminology ets_Terminology_Detail(int nTerminologyID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Terminology_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
              
                command.Parameters.Add(new SqlParameter("@nTerminologyID", nTerminologyID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Terminology temp = new Terminology(
                                (int)reader["TerminologyID"], reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
                               reader["PageName"] == DBNull.Value ? "" : (string)reader["PageName"],
                                reader["InputText"] == DBNull.Value ? "" : (string)reader["InputText"],
                                 reader["OutputText"] == DBNull.Value ? "" : (string)reader["OutputText"]
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





    public static SubDomainInfo SubDomainInfo_Details(string sRootURL)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("SubDomainInfo_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@sRootURL", sRootURL));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SubDomainInfo temp = new SubDomainInfo(
                                (int)reader["SubDomainInfoID"], (string)reader["RootURL"], (string)reader["LogoFileName"],
                               reader["Footer"] == DBNull.Value ? "" : (string)reader["Footer"],
                               reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"]
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





    //#region RoleGroup



    //public static int dbg_RoleGroup_Insert(RoleGroup p_RoleGroup, SqlTransaction tn, SqlConnection cn)
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

    //    using (SqlCommand command = new SqlCommand("dbg_RoleGroup_Insert", connection))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //        pRV.Direction = ParameterDirection.Output;

    //        command.Parameters.Add(pRV);
    //        command.Parameters.Add(new SqlParameter("@sRoleGroupName", p_RoleGroup.RoleGroupName));
    //        command.Parameters.Add(new SqlParameter("@nAccountID", p_RoleGroup.AccountID));
    //        command.Parameters.Add(new SqlParameter("@nOwnerUserID", p_RoleGroup.OwnerUserID));

            

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


    //public static int dbg_RoleGroup_Update(RoleGroup p_RoleGroup, SqlTransaction tn)
    //{

    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //        connection.Open();
    //    }
    //    else
    //    {
    //        connection = tn.Connection;
    //    }

    //    using (SqlCommand command = new SqlCommand("dbg_RoleGroup_Update", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        command.Parameters.Add(new SqlParameter("@nRoleGroupID", p_RoleGroup.RoleGroupID));

    //        command.Parameters.Add(new SqlParameter("@sRoleGroupName", p_RoleGroup.RoleGroupName));
    //        command.Parameters.Add(new SqlParameter("@nAccountID", p_RoleGroup.AccountID));
    //        command.Parameters.Add(new SqlParameter("@nOwnerUserID", p_RoleGroup.OwnerUserID));


    //        command.ExecuteNonQuery();

    //        if (tn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        return 1;
    //    }

    //}


    //public static DataTable dbg_RoleGroup_Select(string sRoleGroupName, int? nAccountID, int? nOwnerUserID,
    //    string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    //{

    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_RoleGroup_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;


    //            if (sRoleGroupName != "")
    //                command.Parameters.Add(new SqlParameter("@sRoleGroupName", sRoleGroupName));

    //            if (nAccountID != null)
    //                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
    //            if (nOwnerUserID != null)
    //                command.Parameters.Add(new SqlParameter("@nOwnerUserID", nOwnerUserID));
              


    //            if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
    //            { sOrder = "RoleGroupID"; sOrderDirection = "DESC"; }

    //            command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

    //            if (nStartRow != null)
    //                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

    //            if (nMaxRows != null)
    //                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));





    //            connection.Open();

    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            System.Data.DataSet ds = new System.Data.DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();

    //            iTotalRowsNum = 0;
    //            if (ds.Tables.Count > 1)
    //            {
    //                iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
    //            }
    //            if (ds.Tables.Count > 0)
    //            {
    //                return ds.Tables[0];
    //            }
    //            {
    //                return null;
    //            }


    //        }
    //    }
    //}



    //public static int dbg_RoleGroup_Delete(int nRoleGroupID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_RoleGroup_Delete", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nRoleGroupID ", nRoleGroupID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}




    //public static RoleGroup dbg_RoleGroup_Detail(int nRoleGroupID, SqlTransaction tn, SqlConnection cn)
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
    //    using (SqlCommand command = new SqlCommand("dbg_RoleGroup_Detail", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }
    //        command.Parameters.Add(new SqlParameter("@nRoleGroupID", nRoleGroupID));

    //        //connection.Open();

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                RoleGroup temp = new RoleGroup(
    //                    (int)reader["RoleGroupID"], (string)reader["RoleGroupName"], (int)reader["AccountID"],
    //                  (int)reader["OwnerUserID"]);

    //                if (tn == null && cn == null)
    //                {
    //                    connection.Close();
    //                    connection.Dispose();
    //                }

    //                return temp;
    //            }

    //        }

    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }

    //        return null;

    //    }

    //}




    //#endregion






    //#region RoleGroupTable



    //public static int dbg_RoleGroupTable_Insert(RoleGroupTable p_RoleGroupTable, SqlTransaction tn, SqlConnection cn)
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

    //    using (SqlCommand command = new SqlCommand("dbg_RoleGroupTable_Insert", connection))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //        pRV.Direction = ParameterDirection.Output;

    //        command.Parameters.Add(pRV);
    //        command.Parameters.Add(new SqlParameter("@nRoleGroupID", p_RoleGroupTable.RoleGroupID));
    //        command.Parameters.Add(new SqlParameter("@nRecordRightID", p_RoleGroupTable.RoleType));
    //        command.Parameters.Add(new SqlParameter("@nTableID", p_RoleGroupTable.TableID));
    //        command.Parameters.Add(new SqlParameter("@bCanExport", p_RoleGroupTable.CanExport));



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


    //public static int dbg_RoleGroupTable_Update(RoleGroupTable p_RoleGroupTable, SqlTransaction tn)
    //{

    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //        connection.Open();
    //    }
    //    else
    //    {
    //        connection = tn.Connection;
    //    }

    //    using (SqlCommand command = new SqlCommand("dbg_RoleGroupTable_Update", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        command.Parameters.Add(new SqlParameter("@nRoleGroupTableID", p_RoleGroupTable.RoleGroupTableID));


    //        command.Parameters.Add(new SqlParameter("@nRoleGroupID", p_RoleGroupTable.RoleGroupID));
    //        command.Parameters.Add(new SqlParameter("@nRecordRightID", p_RoleGroupTable.RoleType));
    //        command.Parameters.Add(new SqlParameter("@nTableID", p_RoleGroupTable.TableID));
    //        command.Parameters.Add(new SqlParameter("@bCanExport", p_RoleGroupTable.CanExport));

    //        command.ExecuteNonQuery();

    //        if (tn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        return 1;
    //    }

    //}


    //public static DataTable dbg_RoleGroupTable_Select(int? nRoleGroupID, int? nRecordRightID, int? nTableID,
    //    bool? bCanExport)
    //{

    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_RoleGroupTable_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;


    //            if (nRoleGroupID != null)
    //                command.Parameters.Add(new SqlParameter("@nRoleGroupID", nRoleGroupID));

    //            if (nRecordRightID != null)
    //                command.Parameters.Add(new SqlParameter("@nRecordRightID", nRecordRightID));

    //            if (nTableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

    //            if (bCanExport != null)
    //                command.Parameters.Add(new SqlParameter("@bCanExport", bCanExport));
                






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



    //public static int dbg_RoleGroupTable_Delete(int nRoleGroupTableID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_RoleGroupTable_Delete", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nRoleGroupTableID ", nRoleGroupTableID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}

      


    //public static RoleGroupTable dbg_RoleGroupTable_Detail(int nRoleGroupTableID, SqlTransaction tn, SqlConnection cn)
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
    //    using (SqlCommand command = new SqlCommand("dbg_RoleGroupTable_Detail", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }
    //        command.Parameters.Add(new SqlParameter("@nRoleGroupTableID", nRoleGroupTableID));

    //        //connection.Open();

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                RoleGroupTable temp = new RoleGroupTable(
    //                    (int)reader["RoleGroupTableID"], (int)reader["RoleGroupID"], (int)reader["RoleType"],
    //                  (int)reader["TableID"],
    //                   (bool)reader["CanExport"]);
                  

    //                if (tn == null && cn == null)
    //                {
    //                    connection.Close();
    //                    connection.Dispose();
    //                }

    //                return temp;
    //            }

    //        }

    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }

    //        return null;

    //    }

    //}




    //#endregion







}

