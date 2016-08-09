using System;
using System.Collections.Generic;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
/// <summary>
/// Summary description for EmailAndIncoming
/// </summary>
public class EmailManager
{
	public EmailManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

   

    public static int ets_InComingEmail_Insert(InComingEmail p_InComingEmail)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_InComingEmail_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@sEmailSubject", p_InComingEmail.EmailSubject));
                command.Parameters.Add(new SqlParameter("@sEmailFrom", p_InComingEmail.EmailFrom));
                command.Parameters.Add(new SqlParameter("@sEmailTo", p_InComingEmail.EmailTo));
                command.Parameters.Add(new SqlParameter("@sCC", p_InComingEmail.InComingEmailID));
                command.Parameters.Add(new SqlParameter("@sBCC", p_InComingEmail.BCC));
                command.Parameters.Add(new SqlParameter("@sAttachments", p_InComingEmail.Attachments));
                command.Parameters.Add(new SqlParameter("@dEmailDate", p_InComingEmail.EmailDate));

                command.Parameters.Add(new SqlParameter("@sMessageID", p_InComingEmail.MessageID));
                command.Parameters.Add(new SqlParameter("@sRawMessage", p_InComingEmail.RawMessage));
                command.Parameters.Add(new SqlParameter("@sTextMessage", p_InComingEmail.TextMessage));
                command.Parameters.Add(new SqlParameter("@sHTMLTextMessage", p_InComingEmail.HTMLTextMessage));
                command.Parameters.Add(new SqlParameter("@sMIMEVersion", p_InComingEmail.MIMEVersion));
                command.Parameters.Add(new SqlParameter("@dDateCreated", p_InComingEmail.DateCreated));
                command.Parameters.Add(new SqlParameter("@nParentEmailID", p_InComingEmail.ParentEmailID));
                command.Parameters.Add(new SqlParameter("@sBatchIDs", p_InComingEmail.BatchIDs));

                command.Parameters.Add(new SqlParameter("@sPOPServer", p_InComingEmail.POPServer));

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

    //public static InComingEmail ets_InComingEmail_Detail_By_MessageID(string strMessageID, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_InComingEmail_Detail_By_MessageID", connection, tn))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        command.Parameters.Add(new SqlParameter("@sMessageID", strMessageID));
    //        //connection.Open();
    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                InComingEmail temp = new InComingEmail(
    //                    (int)reader["InComingEmailID"],
    //                    reader["EmailSubject"] == DBNull.Value ? "" : (string)reader["EmailSubject"],
    //                    (string)reader["EmailFrom"],
    //                    (string)reader["EmailTo"],
    //                    reader["CC"] == DBNull.Value ? "" : (string)reader["CC"],
    //                    reader["BCC"] == DBNull.Value ? "" : (string)reader["BCC"],
    //                    reader["Attachments"] == DBNull.Value ? "" : (string)reader["Attachments"],
    //                reader["EmailDate"] == DBNull.Value ? null : (DateTime?)reader["EmailDate"],
    //                reader["MessageID"] == DBNull.Value ? "" : (string)reader["MessageID"],
    //                reader["RawMessage"] == DBNull.Value ? "" : (string)reader["RawMessage"],
    //                reader["TextMessage"] == DBNull.Value ? "" : (string)reader["TextMessage"],
    //                reader["HTMLTextMessage"] == DBNull.Value ? "" : (string)reader["HTMLTextMessage"],
    //                reader["MIMEVersion"] == DBNull.Value ? "" : (string)reader["MIMEVersion"],
    //                reader["DateCreated"] == DBNull.Value ? null : (DateTime?)reader["DateCreated"],
    //                reader["ParentEmailID"] == DBNull.Value ? null : (int?)reader["ParentEmailID"],
    //                 reader["BatchIDs"] == DBNull.Value ? "" : (string)reader["BatchIDs"]

    //                );

    //                temp.POPServer = reader["POPServer"] == DBNull.Value ? "" : (string)reader["POPServer"];
    //                return temp;
    //            }
    //        }
    //        return null;
    //    }

    //}

    //public static InComingEmail ets_InComingEmail_Detail_By_SERVER(string sPOPServer, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_InComingEmail_Detail_By_SERVER", connection, tn))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        command.Parameters.Add(new SqlParameter("@sPOPServer", sPOPServer));
    //        //connection.Open();
    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                InComingEmail temp = new InComingEmail(
    //                    (int)reader["InComingEmailID"],
    //                    reader["EmailSubject"] == DBNull.Value ? "" : (string)reader["EmailSubject"],
    //                    (string)reader["EmailFrom"],
    //                    (string)reader["EmailTo"],
    //                    reader["CC"] == DBNull.Value ? "" : (string)reader["CC"],
    //                    reader["BCC"] == DBNull.Value ? "" : (string)reader["BCC"],
    //                    reader["Attachments"] == DBNull.Value ? "" : (string)reader["Attachments"],
    //                reader["EmailDate"] == DBNull.Value ? null : (DateTime?)reader["EmailDate"],
    //                reader["MessageID"] == DBNull.Value ? "" : (string)reader["MessageID"],
    //                reader["RawMessage"] == DBNull.Value ? "" : (string)reader["RawMessage"],
    //                reader["TextMessage"] == DBNull.Value ? "" : (string)reader["TextMessage"],
    //                reader["HTMLTextMessage"] == DBNull.Value ? "" : (string)reader["HTMLTextMessage"],
    //                reader["MIMEVersion"] == DBNull.Value ? "" : (string)reader["MIMEVersion"],
    //                reader["DateCreated"] == DBNull.Value ? null : (DateTime?)reader["DateCreated"],
    //                reader["ParentEmailID"] == DBNull.Value ? null : (int?)reader["ParentEmailID"],
    //                 reader["BatchIDs"] == DBNull.Value ? "" : (string)reader["BatchIDs"]

    //                );

    //                temp.POPServer = reader["POPServer"] == DBNull.Value ? "" : (string)reader["POPServer"];


    //                return temp;


    //            }
    //        }
    //        return null;
    //    }

    //}



    //public static int ets_InComingEmail_Update(InComingEmail p_InComingEmail, SqlTransaction tn)
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

    //    using (SqlCommand command = new SqlCommand("ets_InComingEmail_Update", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        command.Parameters.Add(new SqlParameter("@nInComingEmailID ", p_InComingEmail.InComingEmailID));

    //        command.Parameters.Add(new SqlParameter("@sEmailSubject", p_InComingEmail.EmailSubject));
    //        command.Parameters.Add(new SqlParameter("@sEmailFrom", p_InComingEmail.EmailFrom));
    //        command.Parameters.Add(new SqlParameter("@sEmailTo", p_InComingEmail.EmailTo));
    //        command.Parameters.Add(new SqlParameter("@sCC", p_InComingEmail.InComingEmailID));
    //        command.Parameters.Add(new SqlParameter("@sBCC", p_InComingEmail.BCC));
    //        command.Parameters.Add(new SqlParameter("@sAttachments", p_InComingEmail.Attachments));
    //        command.Parameters.Add(new SqlParameter("@dEmailDate", p_InComingEmail.EmailDate));

    //        command.Parameters.Add(new SqlParameter("@sMessageID", p_InComingEmail.MessageID));
    //        command.Parameters.Add(new SqlParameter("@sRawMessage", p_InComingEmail.RawMessage));
    //        command.Parameters.Add(new SqlParameter("@sTextMessage", p_InComingEmail.TextMessage));
    //        command.Parameters.Add(new SqlParameter("@sHTMLTextMessage", p_InComingEmail.HTMLTextMessage));
    //        command.Parameters.Add(new SqlParameter("@sMIMEVersion", p_InComingEmail.MIMEVersion));
    //        command.Parameters.Add(new SqlParameter("@dDateCreated", p_InComingEmail.DateCreated));
    //        command.Parameters.Add(new SqlParameter("@nParentEmailID", p_InComingEmail.ParentEmailID));
    //        command.Parameters.Add(new SqlParameter("@sBatchIDs", p_InComingEmail.BatchIDs));
    //        command.Parameters.Add(new SqlParameter("@sPOPServer", p_InComingEmail.POPServer));


    //        command.ExecuteNonQuery();

    //        if (tn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        return 1;
    //    }

    //}


    public static InComingEmail ets_InComingEmail_Detail_By_MessageID(string strMessageID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_InComingEmail_Detail_By_MessageID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@sMessageID", strMessageID));
                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InComingEmail temp = new InComingEmail(
                                (int)reader["InComingEmailID"],
                                reader["EmailSubject"] == DBNull.Value ? "" : (string)reader["EmailSubject"],
                                (string)reader["EmailFrom"],
                                (string)reader["EmailTo"],
                                reader["CC"] == DBNull.Value ? "" : (string)reader["CC"],
                                reader["BCC"] == DBNull.Value ? "" : (string)reader["BCC"],
                                reader["Attachments"] == DBNull.Value ? "" : (string)reader["Attachments"],
                            reader["EmailDate"] == DBNull.Value ? null : (DateTime?)reader["EmailDate"],
                            reader["MessageID"] == DBNull.Value ? "" : (string)reader["MessageID"],
                            reader["RawMessage"] == DBNull.Value ? "" : (string)reader["RawMessage"],
                            reader["TextMessage"] == DBNull.Value ? "" : (string)reader["TextMessage"],
                            reader["HTMLTextMessage"] == DBNull.Value ? "" : (string)reader["HTMLTextMessage"],
                            reader["MIMEVersion"] == DBNull.Value ? "" : (string)reader["MIMEVersion"],
                            reader["DateCreated"] == DBNull.Value ? null : (DateTime?)reader["DateCreated"],
                            reader["ParentEmailID"] == DBNull.Value ? null : (int?)reader["ParentEmailID"],
                             reader["BatchIDs"] == DBNull.Value ? "" : (string)reader["BatchIDs"]

                            );

                            temp.POPServer = reader["POPServer"] == DBNull.Value ? "" : (string)reader["POPServer"];
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


    //public static InComingEmail ets_InComingEmail_Detail_By_SERVER(string sPOPServer)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_InComingEmail_Detail_By_SERVER", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@sPOPServer", sPOPServer));
    //            connection.Open();
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    InComingEmail temp = new InComingEmail(
    //                        (int)reader["InComingEmailID"],
    //                        reader["EmailSubject"] == DBNull.Value ? "" : (string)reader["EmailSubject"],
    //                        (string)reader["EmailFrom"],
    //                        (string)reader["EmailTo"],
    //                        reader["CC"] == DBNull.Value ? "" : (string)reader["CC"],
    //                        reader["BCC"] == DBNull.Value ? "" : (string)reader["BCC"],
    //                        reader["Attachments"] == DBNull.Value ? "" : (string)reader["Attachments"],
    //                    reader["EmailDate"] == DBNull.Value ? null : (DateTime?)reader["EmailDate"],
    //                    reader["MessageID"] == DBNull.Value ? "" : (string)reader["MessageID"],
    //                    reader["RawMessage"] == DBNull.Value ? "" : (string)reader["RawMessage"],
    //                    reader["TextMessage"] == DBNull.Value ? "" : (string)reader["TextMessage"],
    //                    reader["HTMLTextMessage"] == DBNull.Value ? "" : (string)reader["HTMLTextMessage"],
    //                    reader["MIMEVersion"] == DBNull.Value ? "" : (string)reader["MIMEVersion"],
    //                    reader["DateCreated"] == DBNull.Value ? null : (DateTime?)reader["DateCreated"],
    //                    reader["ParentEmailID"] == DBNull.Value ? null : (int?)reader["ParentEmailID"],
    //                     reader["BatchIDs"] == DBNull.Value ? "" : (string)reader["BatchIDs"]

    //                    );

    //                    temp.POPServer = reader["POPServer"] == DBNull.Value ? "" : (string)reader["POPServer"];
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

    public static int ets_File_Insert(dbgFile p_dbgFile)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_File_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@sFileTitle", p_dbgFile.FileTitle));
                command.Parameters.Add(new SqlParameter("@sFileType", p_dbgFile.FileType));
                command.Parameters.Add(new SqlParameter("@sFileName", p_dbgFile.FileName));
                command.Parameters.Add(new SqlParameter("@sUniqueName", p_dbgFile.UniqueName));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_dbgFile.AccountID));

                command.Parameters.Add(new SqlParameter("@bIsTemp", p_dbgFile.IsTemp));
                command.Parameters.Add(new SqlParameter("@bIsInComingEmail", p_dbgFile.IsIncomingEmail));

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




    //public static int ets_File_Insert(dbgFile p_dbgFile, ref SqlConnection connection, ref SqlTransaction tn)
    //{
       
    //        using (SqlCommand command = new SqlCommand("ets_File_Insert", connection,tn))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //            pRV.Direction = ParameterDirection.Output;

    //            command.Parameters.Add(pRV);

    //            command.Parameters.Add(new SqlParameter("@sFileTitle", p_dbgFile.FileTitle));
    //            command.Parameters.Add(new SqlParameter("@sFileType", p_dbgFile.FileType));
    //            command.Parameters.Add(new SqlParameter("@sFileName", p_dbgFile.FileName));
    //            command.Parameters.Add(new SqlParameter("@sUniqueName", p_dbgFile.UniqueName));
    //            command.Parameters.Add(new SqlParameter("@nAccountID", p_dbgFile.AccountID));

    //            command.Parameters.Add(new SqlParameter("@bIsTemp", p_dbgFile.IsTemp));
    //            command.Parameters.Add(new SqlParameter("@bIsInComingEmail", p_dbgFile.IsIncomingEmail));

    //            //connection.Open();
    //            command.ExecuteNonQuery();

    //            return int.Parse(pRV.Value.ToString());
    //        }
        
    //}





    //#region EmailLog



    //public static int dbg_EmailLog_Insert(EmailLog p_EmailLog, SqlTransaction tn, SqlConnection cn)
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

    //    using (SqlCommand command = new SqlCommand("dbg_EmailLog_Insert", connection))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //        pRV.Direction = ParameterDirection.Output;

    //        command.Parameters.Add(pRV);
    //        command.Parameters.Add(new SqlParameter("@nAccountID", p_EmailLog.AccountID));
    //        command.Parameters.Add(new SqlParameter("@dEmailDate", p_EmailLog.EmailDate));
    //        //command.Parameters.Add(new SqlParameter("@nEmailLogID", p_EmailLog.EmailLogID));
    //        command.Parameters.Add(new SqlParameter("@sEmailSubject", p_EmailLog.EmailSubject));
    //        command.Parameters.Add(new SqlParameter("@sEmailTo", p_EmailLog.EmailTo));
    //        command.Parameters.Add(new SqlParameter("@sEmailType", p_EmailLog.EmailType));
    //        command.Parameters.Add(new SqlParameter("@sRawMessage", p_EmailLog.RawMessage));
    //        command.Parameters.Add(new SqlParameter("@nRecordID", p_EmailLog.RecordID));
    //        command.Parameters.Add(new SqlParameter("@nTableID", p_EmailLog.TableID));
    //        command.Parameters.Add(new SqlParameter("@sEmailUID", p_EmailLog.EmailUID));
            


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


    //public static int dbg_EmailLog_Update(EmailLog p_EmailLog, SqlTransaction tn)
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

    //    using (SqlCommand command = new SqlCommand("dbg_EmailLog_Update", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        command.Parameters.Add(new SqlParameter("@nEmailLogID", p_EmailLog.EmailLogID));
    //        command.Parameters.Add(new SqlParameter("@nAccountID", p_EmailLog.AccountID));
    //        command.Parameters.Add(new SqlParameter("@dEmailDate", p_EmailLog.EmailDate));
    //        command.Parameters.Add(new SqlParameter("@nEmailLogID", p_EmailLog.EmailLogID));
    //        command.Parameters.Add(new SqlParameter("@sEmailSubject", p_EmailLog.EmailSubject));
    //        command.Parameters.Add(new SqlParameter("@sEmailTo", p_EmailLog.EmailTo));
    //        command.Parameters.Add(new SqlParameter("@sEmailType", p_EmailLog.EmailType));
    //        command.Parameters.Add(new SqlParameter("@sRawMessage", p_EmailLog.RawMessage));
    //        command.Parameters.Add(new SqlParameter("@nRecordID", p_EmailLog.RecordID));
    //        command.Parameters.Add(new SqlParameter("@nTableID", p_EmailLog.TableID));
            


    //        command.ExecuteNonQuery();

    //        if (tn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        return 1;
    //    }

    //}



    //public static DataTable dbg_EmailLog_Select(int? nAccountID, string sEmailSubject,string sEmailTo,
    //    DateTime? dEmailDate,int? nTableID,int? nRecordID, string sEmailType,string sRawMessage,
    //    string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    //{

    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_EmailLog_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));




    //            command.Parameters.Add(new SqlParameter("@sEmailSubject", sEmailSubject));
    //            command.Parameters.Add(new SqlParameter("@sEmailTo", sEmailTo));
    //            command.Parameters.Add(new SqlParameter("@dEmailDate", dEmailDate));
    //            command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
    //            command.Parameters.Add(new SqlParameter("@nRecordID", nRecordID));
    //            command.Parameters.Add(new SqlParameter("@sEmailType", sEmailType));
    //            command.Parameters.Add(new SqlParameter("@sRawMessage", sRawMessage));                


    //            if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
    //            { sOrder = "EmailLogID"; sOrderDirection = "DESC"; }

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




    //public static int dbg_EmailLog_Delete(int nEmailLogID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_EmailLog_Delete", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nEmailLogID ", nEmailLogID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}




    //public static EmailLog dbg_EmailLog_Detail(int nEmailLogID, SqlTransaction tn, SqlConnection cn)
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
    //    using (SqlCommand command = new SqlCommand("dbg_EmailLog_Detail", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }
    //        command.Parameters.Add(new SqlParameter("@nEmailLogID", nEmailLogID));

    //        //connection.Open();

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                EmailLog temp = new EmailLog(
    //                    (int)reader["EmailLogID"], (int)reader["AccountID"],
    //                   reader["EmailSubject"] == DBNull.Value ? string.Empty : (string)reader["EmailSubject"],
    //                   reader["EmailTo"] == DBNull.Value ? string.Empty : (string)reader["EmailTo"],
    //                   reader["EmailDate"] == DBNull.Value ? null : (DateTime?)reader["EmailDate"],
    //                   reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
    //                   reader["RecordID"] == DBNull.Value ? null : (int?)reader["RecordID"],
    //                   reader["EmailType"] == DBNull.Value ? string.Empty  : (string)reader["EmailType"],
    //                   reader["RawMessage"] == DBNull.Value ? string.Empty : (string)reader["RawMessage"]
    //                    );

    //                temp.EmailUID = reader["EmailUID"] == DBNull.Value ? string.Empty : (string)reader["EmailUID"];

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



    //public static EmailLog dbg_EmailLog_Detail_BY_UID(string sEmailUID, SqlTransaction tn, SqlConnection cn)
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
    //    using (SqlCommand command = new SqlCommand("dbg_EmailLog_Detail_BY_UID", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }
    //        command.Parameters.Add(new SqlParameter("@sEmailUID", sEmailUID));

    //        //connection.Open();

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                EmailLog temp = new EmailLog(
    //                    (int)reader["EmailLogID"], (int)reader["AccountID"],
    //                   reader["EmailSubject"] == DBNull.Value ? string.Empty : (string)reader["EmailSubject"],
    //                   reader["EmailTo"] == DBNull.Value ? string.Empty : (string)reader["EmailTo"],
    //                   reader["EmailDate"] == DBNull.Value ? null : (DateTime?)reader["EmailDate"],
    //                   reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
    //                   reader["RecordID"] == DBNull.Value ? null : (int?)reader["RecordID"],
    //                   reader["EmailType"] == DBNull.Value ? string.Empty : (string)reader["EmailType"],
    //                   reader["RawMessage"] == DBNull.Value ? string.Empty : (string)reader["RawMessage"]
    //                    );

    //                temp.EmailUID = reader["EmailUID"] == DBNull.Value ? string.Empty : (string)reader["EmailUID"];

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

    #region Message



    public static int Message_Insert(Message p_Message)
    {


        string strBody = Common.StripTagsCharArray(p_Message.Body);
        if(p_Message.DeliveryMethod=="S")
        {
            //no change in body
        }
        else
        {
            string strMaxBody = SystemData.SystemOption_ValueByKey_Account("Number of Characters from Email", p_Message.TableID, p_Message.AccountID);

            if (strMaxBody != "")
            {
                int iLen = int.Parse(strMaxBody);
                if (iLen < strBody.Length)
                {
                    try
                    {

                        strBody = strBody.Substring(0, iLen - 1) + "...";
                    }
                    catch
                    {
                        //
                    }

                }

            }
        }
        
        p_Message.Body = strBody;

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("Message_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nRecordID", p_Message.RecordID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_Message.TableID));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Message.AccountID));

                command.Parameters.Add(new SqlParameter("@dDateTime", p_Message.DateTimeP));
                //command.Parameters.Add(new SqlParameter("@nMessageID", p_Message.MessageID));
                command.Parameters.Add(new SqlParameter("@sMessageType", p_Message.MessageType));
                command.Parameters.Add(new SqlParameter("@sDeliveryMethod", p_Message.DeliveryMethod));
                command.Parameters.Add(new SqlParameter("@bIsIncoming", p_Message.IsIncoming));
                command.Parameters.Add(new SqlParameter("@sOtherParty", p_Message.OtherParty));

                command.Parameters.Add(new SqlParameter("@sSubject", p_Message.Subject));
                command.Parameters.Add(new SqlParameter("@sBody", p_Message.Body));
                command.Parameters.Add(new SqlParameter("@sLink", p_Message.Link));
                command.Parameters.Add(new SqlParameter("@sExternalMessageKey", p_Message.ExternalMessageKey));

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


    public static int Message_Update(Message p_Message)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Message_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nMessageID", p_Message.MessageID));

                command.Parameters.Add(new SqlParameter("@nRecordID", p_Message.RecordID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_Message.TableID));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Message.AccountID));

                command.Parameters.Add(new SqlParameter("@dDateTime", p_Message.DateTimeP));
                //command.Parameters.Add(new SqlParameter("@nMessageID", p_Message.MessageID));
                command.Parameters.Add(new SqlParameter("@sMessageType", p_Message.MessageType));
                command.Parameters.Add(new SqlParameter("@sDeliveryMethod", p_Message.DeliveryMethod));
                command.Parameters.Add(new SqlParameter("@bIsIncoming", p_Message.IsIncoming));
                command.Parameters.Add(new SqlParameter("@sOtherParty", p_Message.OtherParty));

                command.Parameters.Add(new SqlParameter("@sSubject", p_Message.Subject));
                command.Parameters.Add(new SqlParameter("@sBody", p_Message.Body));
                command.Parameters.Add(new SqlParameter("@sLink", p_Message.Link));
                command.Parameters.Add(new SqlParameter("@sExternalMessageKey", p_Message.ExternalMessageKey));

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



    public static DataTable Message_Select(Message p_Message,DateTime? dDateTimeFrom, DateTime? dDateTimeTo,
        string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Message_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nRecordID", p_Message.RecordID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_Message.TableID));
               
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Message.AccountID));

                command.Parameters.Add(new SqlParameter("@dDateTimeFrom", dDateTimeFrom));

                command.Parameters.Add(new SqlParameter("@dDateTimeTo", dDateTimeTo));

                if (p_Message.MessageType!="")
                command.Parameters.Add(new SqlParameter("@sMessageType", p_Message.MessageType));

                if (p_Message.DeliveryMethod!="")
                command.Parameters.Add(new SqlParameter("@sDeliveryMethod", p_Message.DeliveryMethod));

                if (p_Message.IsIncoming!=null)
                command.Parameters.Add(new SqlParameter("@bIsIncoming", p_Message.IsIncoming));

                if (p_Message.OtherParty!="")
                command.Parameters.Add(new SqlParameter("@sOtherParty", p_Message.OtherParty));

                if (p_Message.Subject!="")
                command.Parameters.Add(new SqlParameter("@sSubject", p_Message.Subject));

                if (p_Message.Body!="")
                command.Parameters.Add(new SqlParameter("@sBody", p_Message.Body));

                if (p_Message.Link!="")
                command.Parameters.Add(new SqlParameter("@sLink", p_Message.Link));

                if (p_Message.ExternalMessageKey!="")
                command.Parameters.Add(new SqlParameter("@sExternalMessageKey", p_Message.ExternalMessageKey));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "MessageID"; sOrderDirection = "DESC"; }

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




    public static int Message_Delete(int nMessageID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Message_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nMessageID ", nMessageID));

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




    public static Message Message_Detail(int nMessageID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Message_Detail", connection))
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
                            Message temp = new Message(
                                (int)reader["MessageID"],
                                reader["RecordID"] == DBNull.Value ? null : (int?)reader["RecordID"],
                                reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
                                reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
                                 (DateTime)reader["DateTime"],
                                 (string)reader["MessageType"],
                                 (string)reader["DeliveryMethod"],
                                 reader["IsIncoming"] == DBNull.Value ? null : (bool?)reader["IsIncoming"],
                               reader["OtherParty"] == DBNull.Value ? string.Empty : (string)reader["OtherParty"],
                               reader["Subject"] == DBNull.Value ? string.Empty : (string)reader["Subject"],
                               reader["Body"] == DBNull.Value ? string.Empty : (string)reader["Body"],

                               reader["Link"] == DBNull.Value ? string.Empty : (string)reader["Link"],
                               reader["ExternalMessageKey"] == DBNull.Value ? string.Empty : (string)reader["ExternalMessageKey"]
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



    public static Message Message_Detail_BY_ExternalMessageKey(string sExternalMessageKey)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Message_Detail_BY_ExternalMessageKey", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@sExternalMessageKey", sExternalMessageKey));

                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Message temp = new Message(
                                (int)reader["MessageID"],
                                reader["RecordID"] == DBNull.Value ? null : (int?)reader["RecordID"],
                                reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
                                reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
                                 (DateTime)reader["DateTime"],
                                 (string)reader["MessageType"],
                                 (string)reader["DeliveryMethod"],
                                 reader["IsIncoming"] == DBNull.Value ? null : (bool?)reader["IsIncoming"],
                               reader["OtherParty"] == DBNull.Value ? string.Empty : (string)reader["OtherParty"],
                               reader["Subject"] == DBNull.Value ? string.Empty : (string)reader["Subject"],
                               reader["Body"] == DBNull.Value ? string.Empty : (string)reader["Body"],

                               reader["Link"] == DBNull.Value ? string.Empty : (string)reader["Link"],
                               reader["ExternalMessageKey"] == DBNull.Value ? string.Empty : (string)reader["ExternalMessageKey"]
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




}
