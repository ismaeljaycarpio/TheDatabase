using System;
using System.Collections;
using System.Collections.Generic;

//using System.Linq;

using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;



using System.Net.Mail;
using System.CodeDom.Compiler;

//using DocGen.DAL;

using System.IO;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for ExportManager
/// </summary>
public class ExportManager
{
	public ExportManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}




    #region ExportTemplate

    public static void CreateDefaultExportTemplate(int TableID)
    {
        ExportTemplate newExportTemplate = new ExportTemplate(null, TableID,
                          "Default");

        int iNewExportTemplateID = ExportManager.dbg_ExportTemplate_Insert(newExportTemplate);

        //            DataTable dtExportColumns = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE 
        //            Systemname not in('IsActive','TableID') 
        //            AND ColumnType NOT IN ('staticcontent') AND TableID=" + TableID.ToString()
        //           + @"  ORDER BY DisplayOrder ASC");

        DataTable dtExportColumns = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE 
            Systemname not in('IsActive','TableID') AND DisplayName IS NOT NULL AND LEN(DisplayName) > 0
            AND ColumnType NOT IN ('staticcontent') AND TableID=" + TableID.ToString()
     + @"  ORDER BY DisplayRight,DisplayOrder");

        int i = 0;

        foreach (DataRow dr in dtExportColumns.Rows)
        {
            ExportTemplateItem newExportTemplateItem = new ExportTemplateItem(null, iNewExportTemplateID, int.Parse(dr[0].ToString()),
          dr[1].ToString(), i);
            ExportManager.dbg_ExportTemplateItem_Insert(newExportTemplateItem);
            i = i + 1;
        }

    }

    public static int dbg_ExportTemplate_Insert(ExportTemplate p_ExportTemplate)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_ExportTemplate_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@nTableID", p_ExportTemplate.TableID));
                command.Parameters.Add(new SqlParameter("@sExportTemplateName", p_ExportTemplate.ExportTemplateName));


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


    public static int dbg_ExportTemplate_Update(ExportTemplate p_ExportTemplate)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ExportTemplate_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               
                command.Parameters.Add(new SqlParameter("@nExportTemplateID", p_ExportTemplate.ExportTemplateID));



                command.Parameters.Add(new SqlParameter("@nTableID", p_ExportTemplate.TableID));
                command.Parameters.Add(new SqlParameter("@sExportTemplateName", p_ExportTemplate.ExportTemplateName));

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


    public static DataTable dbg_ExportTemplate_Select(int nAccountID, int? nTableID, string sExportTemplateName,
         string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ExportTemplate_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));



                if (sExportTemplateName != "")
                    command.Parameters.Add(new SqlParameter("@sExportTemplateName", sExportTemplateName));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "ExportTemplateID"; sOrderDirection = "DESC"; }

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



    public static int dbg_ExportTemplate_Delete(int nExportTemplateID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ExportTemplate_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nExportTemplateID ", nExportTemplateID));

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



    public static int spRefreshExportTemplateFields(int nExportTemplateID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spRefreshExportTemplateFields", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nExportTemplateID ", nExportTemplateID));

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




    public static ExportTemplate dbg_ExportTemplate_Detail(int nExportTemplateID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_ExportTemplate_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nExportTemplateID", nExportTemplateID));

                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ExportTemplate temp = new ExportTemplate(
                                (int)reader["ExportTemplateID"], (int)reader["TableID"],
                              (string)reader["ExportTemplateName"]
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



    #region ExportTemplateItem



    public static int dbg_ExportTemplateItem_Insert(ExportTemplateItem p_ExportTemplateItem)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ExportTemplateItem_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;



                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nExportTemplateID", p_ExportTemplateItem.ExportTemplateID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_ExportTemplateItem.ColumnID));
                command.Parameters.Add(new SqlParameter("@sExportHeaderName", p_ExportTemplateItem.ExportHeaderName));
                command.Parameters.Add(new SqlParameter("@nColumnIndex", p_ExportTemplateItem.ColumnIndex));



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


    public static int dbg_ExportTemplateItem_Update(ExportTemplateItem p_ExportTemplateItem)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ExportTemplateItem_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nExportTemplateItemID", p_ExportTemplateItem.ExportTemplateItemID));

                command.Parameters.Add(new SqlParameter("@nExportTemplateID", p_ExportTemplateItem.ExportTemplateID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_ExportTemplateItem.ColumnID));
                command.Parameters.Add(new SqlParameter("@sExportHeaderName", p_ExportTemplateItem.ExportHeaderName));
                command.Parameters.Add(new SqlParameter("@nColumnIndex", p_ExportTemplateItem.ColumnIndex));

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


    public static DataTable dbg_ExportTemplateItem_Select(int? nExportTemplateID, int? nColumnID, string sExportHeaderName,
              string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ExportTemplateItem_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                if (nExportTemplateID != null)
                    command.Parameters.Add(new SqlParameter("@nExportTemplateID", nExportTemplateID));

                if (nColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));
                if (sExportHeaderName != "")
                    command.Parameters.Add(new SqlParameter("@sExportHeaderName", sExportHeaderName));



                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "ColumnIndex"; sOrderDirection = "ASC"; }

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



    public static int dbg_ExportTemplateItem_Delete(int nExportTemplateItemID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ExportTemplateItem_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nExportTemplateItemID ", nExportTemplateItemID));

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




    public static ExportTemplateItem dbg_ExportTemplateItem_Detail(int nExportTemplateItemID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ExportTemplateItem_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nExportTemplateItemID", nExportTemplateItemID));

                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ExportTemplateItem temp = new ExportTemplateItem(
                                (int)reader["ExportTemplateItemID"], (int)reader["ExportTemplateID"], (int)reader["ColumnID"],
                              (string)reader["ExportHeaderName"], (int)reader["ColumnIndex"]

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