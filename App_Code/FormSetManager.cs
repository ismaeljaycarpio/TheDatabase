using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


/// <summary>
/// Summary description for FormSetManager
/// </summary>
public class FormSetManager
{
	public FormSetManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}





    public static int dbg_FormSetGroup_Insert(FormSetGroup p_FormSetGroup)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetGroup_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@sFormSetGroupName", p_FormSetGroup.FormSetGroupName));

                if (p_FormSetGroup.ColumnPosition != null)
                    command.Parameters.Add(new SqlParameter("@nColumnPosition", p_FormSetGroup.ColumnPosition));

                if (p_FormSetGroup.ParentTableID != null)
                    command.Parameters.Add(new SqlParameter("@nParentTableID", p_FormSetGroup.ParentTableID));

                if (p_FormSetGroup.Sequential != null)
                    command.Parameters.Add(new SqlParameter("@bSequential", p_FormSetGroup.Sequential));

                if (p_FormSetGroup.HideColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nHideColumnID", p_FormSetGroup.HideColumnID));

                if (p_FormSetGroup.HideColumnValue != "")
                    command.Parameters.Add(new SqlParameter("@sHideColumnValue", p_FormSetGroup.HideColumnValue));

                if (p_FormSetGroup.HideOperator != "")
                    command.Parameters.Add(new SqlParameter("@sHideOperator", p_FormSetGroup.HideOperator));





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


    public static int dbg_FormSetGroup_Update(FormSetGroup p_FormSetGroup)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetGroup_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               
                command.Parameters.Add(new SqlParameter("@nFormSetGroupID", p_FormSetGroup.FormSetGroupID));

                command.Parameters.Add(new SqlParameter("@sFormSetGroupName", p_FormSetGroup.FormSetGroupName));

                if (p_FormSetGroup.ColumnPosition != null)
                    command.Parameters.Add(new SqlParameter("@nColumnPosition", p_FormSetGroup.ColumnPosition));

                if (p_FormSetGroup.ParentTableID != null)
                    command.Parameters.Add(new SqlParameter("@nParentTableID", p_FormSetGroup.ParentTableID));

                if (p_FormSetGroup.Sequential != null)
                    command.Parameters.Add(new SqlParameter("@bSequential", p_FormSetGroup.Sequential));

                if (p_FormSetGroup.HideColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nHideColumnID", p_FormSetGroup.HideColumnID));

                if (p_FormSetGroup.HideColumnValue != "")
                    command.Parameters.Add(new SqlParameter("@sHideColumnValue", p_FormSetGroup.HideColumnValue));

                if (p_FormSetGroup.HideOperator != "")
                    command.Parameters.Add(new SqlParameter("@sHideOperator", p_FormSetGroup.HideOperator));


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



    public static DataTable dbg_FormSetGroup_Select(int? nParentTableID, string sFormSetGroupName,
        int? nColumnPosition,bool? bSequential,
        string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetGroup_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nParentTableID", nParentTableID));

                if (sFormSetGroupName != "")
                    command.Parameters.Add(new SqlParameter("@sFormSetGroupName", sFormSetGroupName));

                if (nColumnPosition != null)
                    command.Parameters.Add(new SqlParameter("@nColumnPosition", nColumnPosition));

                if (bSequential != null)
                    command.Parameters.Add(new SqlParameter("@bSequential", bSequential));



                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "FormSetGroupID"; sOrderDirection = "DESC"; }

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





    public static int dbg_FormSetGroup_Delete(int nFormSetGroupID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetGroup_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nFormSetGroupID ", nFormSetGroupID));

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






    public static int dbg_FormSet_Insert(FormSet p_FormSet)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_FormSet_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@nFormSetGroupID", p_FormSet.FormSetGroupID));

                if (p_FormSet.RowPosition != null)
                    command.Parameters.Add(new SqlParameter("@nRowPosition", p_FormSet.RowPosition));

                if (p_FormSet.FormSetName != "")
                    command.Parameters.Add(new SqlParameter("@sFormSetName", p_FormSet.FormSetName));





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


    public static int dbg_FormSet_Update(FormSet p_FormSet)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSet_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nFormSetID", p_FormSet.FormSetID));

                command.Parameters.Add(new SqlParameter("@nFormSetGroupID", p_FormSet.FormSetGroupID));

                if (p_FormSet.RowPosition != null)
                    command.Parameters.Add(new SqlParameter("@nRowPosition", p_FormSet.RowPosition));

                if (p_FormSet.FormSetName != "")
                    command.Parameters.Add(new SqlParameter("@sFormSetName", p_FormSet.FormSetName));


                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return 1;
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



    public static DataTable dbg_FormSet_Select(int? nFormSetGroupID,int? nRowPosition, string sFormSetName,       
        string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSet_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if(nFormSetGroupID!=null)
                command.Parameters.Add(new SqlParameter("@nFormSetGroupID", nFormSetGroupID));

                if (nRowPosition != null)
                    command.Parameters.Add(new SqlParameter("@nRowPosition", nRowPosition));

                if (sFormSetName != "")
                    command.Parameters.Add(new SqlParameter("@sFormSetName", sFormSetName));

                


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "FormSetID"; sOrderDirection = "DESC"; }

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





    public static int dbg_FormSet_Delete(int nFormSetID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSet_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nFormSetID ", nFormSetID));

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






    public static int dbg_FormSetForm_Insert(FormSetForm p_FormSetForm)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetForm_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                if (p_FormSetForm.FormSetFormID != null)
                    command.Parameters.Add(new SqlParameter("@nFormSetFormID", p_FormSetForm.FormSetFormID));
                command.Parameters.Add(new SqlParameter("@nFormSetID", p_FormSetForm.FormSetID));

                if (p_FormSetForm.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_FormSetForm.TableID));

                if (p_FormSetForm.UpdateColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nUpdateColumnID", p_FormSetForm.UpdateColumnID));

                if (p_FormSetForm.UpdateColumnValue != "")
                    command.Parameters.Add(new SqlParameter("@sUpdateColumnValue", p_FormSetForm.UpdateColumnValue));

                if (p_FormSetForm.DisplayOrder != null)
                    command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_FormSetForm.DisplayOrder));

                if (p_FormSetForm.IncompleteImage != "")
                    command.Parameters.Add(new SqlParameter("@sIncompleteImage", p_FormSetForm.IncompleteImage));

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


    public static int dbg_FormSetForm_Update(FormSetForm p_FormSetForm)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetForm_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nFormSetFormID", p_FormSetForm.FormSetFormID));
                command.Parameters.Add(new SqlParameter("@nFormSetID", p_FormSetForm.FormSetID));


                if (p_FormSetForm.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_FormSetForm.TableID));

                if (p_FormSetForm.UpdateColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nUpdateColumnID", p_FormSetForm.UpdateColumnID));

                if (p_FormSetForm.UpdateColumnValue != "")
                    command.Parameters.Add(new SqlParameter("@sUpdateColumnValue", p_FormSetForm.UpdateColumnValue));

                if (p_FormSetForm.DisplayOrder != null)
                    command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_FormSetForm.DisplayOrder));

                if (p_FormSetForm.IncompleteImage != "")
                    command.Parameters.Add(new SqlParameter("@sIncompleteImage", p_FormSetForm.IncompleteImage));



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



    public static DataTable dbg_FormSetForm_Select(int? nFormSetID, int? nTableID, int? nDisplayOrder,
        string sIncompleteImage,
        string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetForm_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                if (nFormSetID != null)
                    command.Parameters.Add(new SqlParameter("@nFormSetID", nFormSetID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nDisplayOrder != null)
                    command.Parameters.Add(new SqlParameter("@nDisplayOrder", nDisplayOrder));

                if (sIncompleteImage != "")
                    command.Parameters.Add(new SqlParameter("@sIncompleteImage", sIncompleteImage));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "FormSetFormID"; sOrderDirection = "DESC"; }

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





    public static int dbg_FormSetForm_Delete(int nFormSetFormID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetForm_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nFormSetFormID ", nFormSetFormID));

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


    public static FormSet dbg_FormSet_Detail(int nFormSetID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSet_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nFormSetID", nFormSetID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormSet temp = new FormSet(
                                (int)reader["FormSetID"], (int)reader["FormSetGroupID"],
                              (int)reader["RowPosition"],
                              (string)reader["FormSetName"]
                             );
                            temp.ShowOnAdd = reader["ShowOnAdd"] == DBNull.Value ? null : (bool?)reader["ShowOnAdd"];
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

    
    public static FormSetGroup dbg_FormSetGroup_Detail(int nFormSetGroupID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetGroup_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nFormSetGroupID", nFormSetGroupID));

                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormSetGroup temp = new FormSetGroup(
                                (int)reader["FormSetGroupID"], (string)reader["FormSetGroupName"],
                              (int)reader["ColumnPosition"],
                              (int)reader["ParentTableID"], (bool)reader["Sequential"]
                             );
                            temp.HideColumnID = reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"];
                            temp.HideColumnValue = reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"];
                            temp.HideOperator = reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"];
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
        

    public static FormSetForm dbg_FormSetForm_Detail(int nFormSetFormID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetForm_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
              
                command.Parameters.Add(new SqlParameter("@nFormSetFormID", nFormSetFormID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormSetForm temp = new FormSetForm(
                                (int)reader["FormSetFormID"], (int)reader["FormSetID"],
                              (int)reader["TableID"],
                              (int)reader["DisplayOrder"],
                             reader["UpdateColumnID"] == DBNull.Value ? null : (int?)reader["UpdateColumnID"],
                             reader["UpdateColumnValue"] == DBNull.Value ? "" : (string)reader["UpdateColumnValue"]
                             );
                            temp.IncompleteImage = reader["IncompleteImage"] == DBNull.Value ? "" : (string)reader["IncompleteImage"];

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




    public static int dbg_FormSetProgress_Insert(FormSetProgress p_FormSetProgress)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_FormSetProgress_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@nRecordID", p_FormSetProgress.RecordID));
                command.Parameters.Add(new SqlParameter("@nFormSetFormID", p_FormSetProgress.FormSetFormID));
                command.Parameters.Add(new SqlParameter("@nFormSetID", p_FormSetProgress.FormSetID));
                command.Parameters.Add(new SqlParameter("@bCompleted", p_FormSetProgress.Completed));

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


    //public static int dbg_FormSetProgress_Update(FormSetProgress p_FormSetProgress, SqlTransaction tn)
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

    //    using (SqlCommand command = new SqlCommand("dbg_FormSetProgress_Update", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        command.Parameters.Add(new SqlParameter("@nFormSetProgressID", p_FormSetProgress.FormSetProgressID));

    //        command.Parameters.Add(new SqlParameter("@nRecordID", p_FormSetProgress.RecordID));
    //        command.Parameters.Add(new SqlParameter("@nFormSetFormID", p_FormSetProgress.FormSetFormID));
    //        command.Parameters.Add(new SqlParameter("@nFormSetID", p_FormSetProgress.FormSetID));
    //        command.Parameters.Add(new SqlParameter("@bCompleted", p_FormSetProgress.Completed));


    //        command.ExecuteNonQuery();

    //        if (tn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        return 1;
    //    }

    //}


    //public static int dbg_FormSetProgress_Delete(int nFormSetProgressID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_FormSetProgress_Delete", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nFormSetProgressID ", nFormSetProgressID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}

    
    public static FormSetProgress dbg_FormSetProgress_Detail(int nFormSetProgressID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_FormSetProgress_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nFormSetProgressID", nFormSetProgressID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormSetProgress temp = new FormSetProgress(
                                (int)reader["FormSetProgressID"], (int)reader["RecordID"], (int)reader["FormSetID"],
                               (int)reader["FormSetFormID"],
                               reader["Completed"] == DBNull.Value ? null : (bool?)reader["Completed"]
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



    public static void StartProgressHistory(int iFormSetID, int iRecordID)
    {
        // clean if any record found
        Common.ExecuteText("DELETE FormSetProgress WHERE RecordID=" + iRecordID.ToString() + " AND FormSetID=" + iFormSetID.ToString());

        DataTable dtFromSetForm = Common.DataTableFromText("SELECT * FROM FormSetForm WHERE FormSetID=" + iFormSetID + " ORDER BY DisplayOrder");

        foreach (DataRow dr in dtFromSetForm.Rows)
        {
            FormSetProgress theFormSetProgress = new FormSetProgress(null, iRecordID, iFormSetID, int.Parse(dr["FormSetFormID"].ToString()), null);
            dbg_FormSetProgress_Insert(theFormSetProgress);
        }

    }

}