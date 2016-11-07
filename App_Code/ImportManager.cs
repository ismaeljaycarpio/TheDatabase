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
/// Summary description for ImportManager
/// </summary>
public class ImportManager
{
	public ImportManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}




    #region ImportTemplate
    public static int? GetDefaultImportTemplate(int? iTableID)
    {
        if (iTableID == null)
            return null;
        try
        {
            string strDID = Common.GetValueFromSQL("SELECT DefaultImportTemplateID FROM [Table] WHERE TableID=" + iTableID.ToString());

            if (strDID != "")
            {
                return int.Parse(strDID);
            }
            string strID = Common.GetValueFromSQL("SELECT  TOP 1 ImportTemplateID  FROM ImportTemplate WHERE TableID="
                + iTableID.ToString() + "  ORDER BY ImportTemplateID DESC");// the latest one

            if (strID != "")
            {
                return int.Parse(strID);
            }
            else
            {
                //create one
                return ImportManager.CreateDefaultImportTemplate((int)iTableID, "");
            }
        }
        catch
        {
            //
        }

        return null;
    }
    public static int? CreateDefaultImportTemplate(int TableID, string strDisplayTextSummary)
    {
        if (strDisplayTextSummary == "")
            strDisplayTextSummary = "DisplayTextSummary";

        ImportTemplate newImportTemplate = new ImportTemplate(null, (int)TableID,
                              "Automatically Created Template", "This is the default import template which is auto created by the system.", "", "", "", "");
        int? iNewImportTemplateID = ImportManager.dbg_ImportTemplate_Insert(newImportTemplate);


        DataTable dtImportColumns = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE 
            Systemname not in('IsActive','TableID') AND "+strDisplayTextSummary
            +@" IS NOT NULL AND LEN("+strDisplayTextSummary+@") > 0
            AND ColumnType NOT IN ('staticcontent') AND TableID=" + TableID.ToString()
            + @"  ORDER BY DisplayRight,DisplayOrder");

        int i = 0;

        foreach (DataRow dr in dtImportColumns.Rows)
        {
            ImportTemplateItem newImportTemplateItem = new ImportTemplateItem(null, iNewImportTemplateID, int.Parse(dr[0].ToString()),
          dr[1].ToString(), i);
            ImportManager.dbg_ImportTemplateItem_Insert(newImportTemplateItem);
            i = i + 1;
        }
        return iNewImportTemplateID;
    }

    public static int dbg_ImportTemplate_Insert(ImportTemplate p_ImportTemplate)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplate_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                
                if (p_ImportTemplate.IsImportPositional != null)
                    command.Parameters.Add(new SqlParameter("@bIsImportPositional", p_ImportTemplate.IsImportPositional));


                if (p_ImportTemplate.ImportDataStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nImportDataStartRow", p_ImportTemplate.ImportDataStartRow));
                if (p_ImportTemplate.ImportColumnHeaderRow != null)
                    command.Parameters.Add(new SqlParameter("@nImportColumnHeaderRow", p_ImportTemplate.ImportColumnHeaderRow));


                command.Parameters.Add(new SqlParameter("@nTableID", p_ImportTemplate.TableID));
                command.Parameters.Add(new SqlParameter("@sImportTemplateName", p_ImportTemplate.ImportTemplateName));

                if (p_ImportTemplate.HelpText != "")
                    command.Parameters.Add(new SqlParameter("@sHelpText", p_ImportTemplate.HelpText));
                if (p_ImportTemplate.TemplateUniqueFileName != "")
                    command.Parameters.Add(new SqlParameter("@sTemplateUniqueFileName", p_ImportTemplate.TemplateUniqueFileName));
                if (p_ImportTemplate.FileFormat != "")
                    command.Parameters.Add(new SqlParameter("@sFileFormat", p_ImportTemplate.FileFormat));
                if (p_ImportTemplate.SPName != "")
                    command.Parameters.Add(new SqlParameter("@sSPName", p_ImportTemplate.SPName));
                if (p_ImportTemplate.Notes != "")
                    command.Parameters.Add(new SqlParameter("@sNotes", p_ImportTemplate.Notes));



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


    public static int dbg_ImportTemplate_Update(ImportTemplate p_ImportTemplate)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplate_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               command.Parameters.Add(new SqlParameter("@nImportTemplateID", p_ImportTemplate.ImportTemplateID));

               if (p_ImportTemplate.IsImportPositional != null)
                   command.Parameters.Add(new SqlParameter("@bIsImportPositional", p_ImportTemplate.IsImportPositional));

                if (p_ImportTemplate.ImportDataStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nImportDataStartRow", p_ImportTemplate.ImportDataStartRow));
                if (p_ImportTemplate.ImportColumnHeaderRow != null)
                    command.Parameters.Add(new SqlParameter("@nImportColumnHeaderRow", p_ImportTemplate.ImportColumnHeaderRow));


                command.Parameters.Add(new SqlParameter("@nTableID", p_ImportTemplate.TableID));
                command.Parameters.Add(new SqlParameter("@sImportTemplateName", p_ImportTemplate.ImportTemplateName));


                if (p_ImportTemplate.HelpText != "")
                    command.Parameters.Add(new SqlParameter("@sHelpText", p_ImportTemplate.HelpText));
                if (p_ImportTemplate.TemplateUniqueFileName != "")
                    command.Parameters.Add(new SqlParameter("@sTemplateUniqueFileName", p_ImportTemplate.TemplateUniqueFileName));
                if (p_ImportTemplate.FileFormat != "")
                    command.Parameters.Add(new SqlParameter("@sFileFormat", p_ImportTemplate.FileFormat));
                if (p_ImportTemplate.SPName != "")
                    command.Parameters.Add(new SqlParameter("@sSPName", p_ImportTemplate.SPName));
                if (p_ImportTemplate.Notes != "")
                    command.Parameters.Add(new SqlParameter("@sNotes", p_ImportTemplate.Notes));


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
    

    public static DataTable dbg_ImportTemplate_Select( int nAccountID, int? nTableID, string sImportTemplateName,
         string sHelpText, string sTemplateUniqueFileName, string sFileFormat, string sSPName,
         string sNotes,
         string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplate_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if(nTableID!=null)
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));



                if (sImportTemplateName != "")
                    command.Parameters.Add(new SqlParameter("@sImportTemplateName", sImportTemplateName));

                if (sHelpText != "")
                    command.Parameters.Add(new SqlParameter("@sHelpText", sHelpText));

                if (sTemplateUniqueFileName != "")
                    command.Parameters.Add(new SqlParameter("@sTemplateUniqueFileName", sTemplateUniqueFileName));

                if (sFileFormat != "")
                    command.Parameters.Add(new SqlParameter("@sFileFormat", sFileFormat));

                if (sSPName != "")
                    command.Parameters.Add(new SqlParameter("@sSPName", sSPName));

                if (sNotes != "")
                    command.Parameters.Add(new SqlParameter("@sNotes", sNotes));



                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "ImportTemplateID"; sOrderDirection = "DESC"; }

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

    

    public static int dbg_ImportTemplate_Delete(int nImportTemplateID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplate_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nImportTemplateID ", nImportTemplateID));

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



    //public static int spRefreshImportTemplateFields(int nImportTemplateID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("spRefreshImportTemplateFields", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nImportTemplateID ", nImportTemplateID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}


  
    
    public static ImportTemplate dbg_ImportTemplate_Detail(int nImportTemplateID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplate_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nImportTemplateID", nImportTemplateID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ImportTemplate temp = new ImportTemplate(
                                (int)reader["ImportTemplateID"], (int)reader["TableID"],
                              (string)reader["ImportTemplateName"],
                               reader["HelpText"] == DBNull.Value ? "" : (string)reader["HelpText"],
                                reader["TemplateUniqueFileName"] == DBNull.Value ? "" : (string)reader["TemplateUniqueFileName"],
                                 reader["FileFormat"] == DBNull.Value ? "" : (string)reader["FileFormat"],
                                  reader["SPName"] == DBNull.Value ? "" : (string)reader["SPName"],
                                    reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"]
                                );
                            temp.ImportDataStartRow = reader["ImportDataStartRow"] == DBNull.Value ? null : (int?)reader["ImportDataStartRow"];
                            temp.ImportColumnHeaderRow = reader["ImportColumnHeaderRow"] == DBNull.Value ? null : (int?)reader["ImportColumnHeaderRow"];
                            temp.IsImportPositional = reader["IsImportPositional"] == DBNull.Value ? null : (bool?)reader["IsImportPositional"];


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



    #region ImportTemplateItem



    public static int dbg_ImportTemplateItem_Insert(ImportTemplateItem p_ImportTemplateItem)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplateItem_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nImportTemplateID", p_ImportTemplateItem.ImportTemplateID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_ImportTemplateItem.ColumnID));
                command.Parameters.Add(new SqlParameter("@sImportHeaderName", p_ImportTemplateItem.ImportHeaderName));
                command.Parameters.Add(new SqlParameter("@nColumnIndex", p_ImportTemplateItem.ColumnIndex));

                if (p_ImportTemplateItem.PositionOnImport != "")
                    command.Parameters.Add(new SqlParameter("@sPositionOnImport", p_ImportTemplateItem.PositionOnImport));
                //if (p_ImportTemplateItem.IsDateSingleColumn != null)
                //    command.Parameters.Add(new SqlParameter("@bIsDateSingleColumn", p_ImportTemplateItem.IsDateSingleColumn));


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


    public static int dbg_ImportTemplateItem_Update(ImportTemplateItem p_ImportTemplateItem)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplateItem_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               

                command.Parameters.Add(new SqlParameter("@nImportTemplateItemID", p_ImportTemplateItem.ImportTemplateItemID));

                command.Parameters.Add(new SqlParameter("@nImportTemplateID", p_ImportTemplateItem.ImportTemplateID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_ImportTemplateItem.ColumnID));
                command.Parameters.Add(new SqlParameter("@sImportHeaderName", p_ImportTemplateItem.ImportHeaderName));
                command.Parameters.Add(new SqlParameter("@nColumnIndex", p_ImportTemplateItem.ColumnIndex));
                command.Parameters.Add(new SqlParameter("@nParentImportColumnID", p_ImportTemplateItem.ParentImportColumnID));

                if (p_ImportTemplateItem.PositionOnImport != "")
                    command.Parameters.Add(new SqlParameter("@sPositionOnImport", p_ImportTemplateItem.PositionOnImport));
                //if (p_ImportTemplateItem.IsDateSingleColumn != null)
                //    command.Parameters.Add(new SqlParameter("@bIsDateSingleColumn", p_ImportTemplateItem.IsDateSingleColumn));


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


    public static DataTable dbg_ImportTemplateItem_Select(int? nImportTemplateID, int? nColumnID, string sImportHeaderName,
              string  sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum )
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplateItem_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                if(nImportTemplateID!=null)
                    command.Parameters.Add(new SqlParameter("@nImportTemplateID", nImportTemplateID));

                if (nColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

                if (sImportHeaderName != "")
                    command.Parameters.Add(new SqlParameter("@sImportHeaderName", sImportHeaderName));
                


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



    public static int dbg_ImportTemplateItem_Delete(int nImportTemplateItemID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ImportTemplateItem_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nImportTemplateItemID ", nImportTemplateItemID));

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




    public static ImportTemplateItem dbg_ImportTemplateItem_Detail(int nImportTemplateItemID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_ImportTemplateItem_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nImportTemplateItemID", nImportTemplateItemID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ImportTemplateItem temp = new ImportTemplateItem(
                                (int)reader["ImportTemplateItemID"], (int)reader["ImportTemplateID"], (int)reader["ColumnID"],
                              (string)reader["ImportHeaderName"], (int)reader["ColumnIndex"]

                                );

                            temp.ParentImportColumnID = reader["ParentImportColumnID"] == DBNull.Value ? null : (int?)reader["ParentImportColumnID"];
                            temp.PositionOnImport = reader["PositionOnImport"] == DBNull.Value ? "" : (string)reader["PositionOnImport"];
                            //temp.IsDateSingleColumn = reader["IsDateSingleColumn"] == DBNull.Value ? null : (bool?)reader["IsDateSingleColumn"];



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

    public static int spRefreshImportTemplateFields(int nImportTemplateID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spRefreshImportTemplateFields", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nImportTemplateID ", nImportTemplateID));

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



}