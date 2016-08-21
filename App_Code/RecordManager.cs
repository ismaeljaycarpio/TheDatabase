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
//using DocGen.DAL;
using System.IO;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using DocGen.DAL;
/// <summary>
/// Summary description for RecordManager
/// </summary>
public class RecordManager
{
    public RecordManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region ETS Record Group

    public static int ets_Menu_Insert(Menu p_Menu)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Menu_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                if (p_Menu.MenuType != "")
                    command.Parameters.Add(new SqlParameter("@sMenuType", p_Menu.MenuType));

                if (p_Menu.DocumentTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nDocumentTypeID", p_Menu.DocumentTypeID));

                command.Parameters.Add(new SqlParameter("@sMenu", p_Menu.MenuP));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Menu.AccountID));
                command.Parameters.Add(new SqlParameter("@bShowOnMenu", p_Menu.ShowOnMenu));

                if (p_Menu.IsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", p_Menu.IsActive));
                if (p_Menu.ParentMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nParentMenuID", p_Menu.ParentMenuID));


                if (p_Menu.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_Menu.TableID));

                if (p_Menu.DocumentID != null)
                    command.Parameters.Add(new SqlParameter("@nDocumentID", p_Menu.DocumentID));

                //if (p_Menu.CustomPageLink != "")
                //    command.Parameters.Add(new SqlParameter("@sCustomPageLink", p_Menu.CustomPageLink));

                if (p_Menu.ExternalPageLink != "")
                    command.Parameters.Add(new SqlParameter("@sExternalPageLink", p_Menu.ExternalPageLink));

                if (p_Menu.OpenInNewWindow != null)
                    command.Parameters.Add(new SqlParameter("@bOpenInNewWindow", p_Menu.OpenInNewWindow));

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


    //public static int ets_Menu_Insert(Menu p_Menu)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Menu_Insert", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //            pRV.Direction = ParameterDirection.Output;

    //            command.Parameters.Add(pRV);

    //            if (p_Menu.MenuType != "")
    //                command.Parameters.Add(new SqlParameter("@sMenuType", p_Menu.MenuType));

    //            command.Parameters.Add(new SqlParameter("@sMenu", p_Menu.MenuP));
    //            command.Parameters.Add(new SqlParameter("@nAccountID", p_Menu.AccountID));
    //            command.Parameters.Add(new SqlParameter("@bShowOnMenu", p_Menu.ShowOnMenu));

    //            if (p_Menu.DocumentTypeID != null)
    //                command.Parameters.Add(new SqlParameter("@nDocumentTypeID", p_Menu.DocumentTypeID));


    //            if (p_Menu.IsActive != null)
    //                command.Parameters.Add(new SqlParameter("@bIsActive", p_Menu.IsActive));

    //            if (p_Menu.ParentMenuID != null)
    //                command.Parameters.Add(new SqlParameter("@nParentMenuID", p_Menu.ParentMenuID));

    //            if (p_Menu.TableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableID", p_Menu.TableID));

    //            if (p_Menu.DocumentID != null)
    //                command.Parameters.Add(new SqlParameter("@nDocumentID", p_Menu.DocumentID));

    //            //if (p_Menu.CustomPageLink != "")
    //            //    command.Parameters.Add(new SqlParameter("@sCustomPageLink", p_Menu.CustomPageLink));

    //            if (p_Menu.ExternalPageLink != "")
    //                command.Parameters.Add(new SqlParameter("@sExternalPageLink", p_Menu.ExternalPageLink));

    //            if (p_Menu.OpenInNewWindow != null)
    //                command.Parameters.Add(new SqlParameter("@bOpenInNewWindow", p_Menu.OpenInNewWindow));


    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return int.Parse(pRV.Value.ToString());
    //        }
    //    }
    //}

    public static int ets_Menu_Update(Menu p_Menu)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Menu_Update", connection))
            {


                command.CommandType = CommandType.StoredProcedure;


                if (p_Menu.MenuType != "")
                    command.Parameters.Add(new SqlParameter("@sMenuType", p_Menu.MenuType));

                if (p_Menu.DocumentTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nDocumentTypeID", p_Menu.DocumentTypeID));

                command.Parameters.Add(new SqlParameter("@nMenuID", p_Menu.MenuID));
                command.Parameters.Add(new SqlParameter("@sMenu", p_Menu.MenuP));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Menu.AccountID));
                command.Parameters.Add(new SqlParameter("@bShowOnMenu", p_Menu.ShowOnMenu));
                if (p_Menu.IsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", p_Menu.IsActive));

                if (p_Menu.ParentMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nParentMenuID", p_Menu.ParentMenuID));

                if (p_Menu.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_Menu.TableID));

                if (p_Menu.DocumentID != null)
                    command.Parameters.Add(new SqlParameter("@nDocumentID", p_Menu.DocumentID));

                //if (p_Menu.CustomPageLink != "")
                //    command.Parameters.Add(new SqlParameter("@sCustomPageLink", p_Menu.CustomPageLink));

                if (p_Menu.ExternalPageLink != "")
                    command.Parameters.Add(new SqlParameter("@sExternalPageLink", p_Menu.ExternalPageLink));

                if (p_Menu.OpenInNewWindow != null)
                    command.Parameters.Add(new SqlParameter("@bOpenInNewWindow", p_Menu.OpenInNewWindow));


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


    public static int ets_Menu_Delete(int iMenuID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Menu_Delete", connection))
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


    public static int ets_Menu_UnDelete(int iMenuID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Menu_UnDelete", connection))
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



    //public static List<Menu>      
                
   //     ets_Menu_Select(int? nMenuID, string sMenu,
   //  bool? bShowOnMenu, int? nAccountID, bool? bIsActive, string sOrder,
   //string sOrderDirection, int? nStartRow, int? nMaxRows,
   //     ref int iTotalRowsNum, int? nParentMenuID, bool? bWithTable)
   // {
   //     using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
   //     {
   //         using (SqlCommand command = new SqlCommand("ets_Menu_Select", connection))
   //         {
   //             command.CommandType = CommandType.StoredProcedure;
   //             //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

   //             if (bWithTable != null)
   //                 command.Parameters.Add(new SqlParameter("@bWithTable", bWithTable));

   //             if (nMenuID != null)
   //                 command.Parameters.Add(new SqlParameter("@nMenuID", nMenuID));

   //             if (nParentMenuID != null)
   //                 command.Parameters.Add(new SqlParameter("@nParentMenuID", nParentMenuID));

   //             if (sMenu != string.Empty)
   //                 command.Parameters.Add(new SqlParameter("@sMenu", sMenu));

   //             if (bShowOnMenu != null)
   //                 command.Parameters.Add(new SqlParameter("@bShowOnMenu", bShowOnMenu));


   //             if (nAccountID != null)
   //                 command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

   //             if (bIsActive != null)
   //                 command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));


   //             if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
   //             { sOrder = "MenuID"; sOrderDirection = "DESC"; }

   //             command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

   //             if (nStartRow != null)
   //                 command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

   //             if (nMaxRows != null)
   //                 command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


   //             connection.Open();

   //             List<Menu> list = new List<Menu>();
   //             using (SqlDataReader reader = command.ExecuteReader())
   //             {
   //                 while (reader.Read())
   //                 {
   //                     Menu temp = new Menu(
   //                         (int)reader["MenuID"],
   //                         (string)reader["Menu"],
   //                           (int)reader["AccountID"],
   //                          (bool)reader["ShowOnMenu"],
   //                           (bool)reader["IsActive"]
   //                          );

   //                     temp.ParentMenuID = reader["ParentMenuID"] == DBNull.Value ? null : (int?)reader["ParentMenuID"];
   //                     temp.TableID = reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"];
   //                     temp.DocumentID = reader["DocumentID"] == DBNull.Value ? null : (int?)reader["DocumentID"];
   //                     //temp.CustomPageLink = reader["CustomPageLink"] == DBNull.Value ? null : (string)reader["CustomPageLink"];
   //                     temp.OpenInNewWindow = reader["OpenInNewWindow"] == DBNull.Value ? null : (bool?)reader["OpenInNewWindow"];
   //                     temp.ExternalPageLink = reader["ExternalPageLink"] == DBNull.Value ? null : (string)reader["ExternalPageLink"];
   //                     //temp.TableName = reader["TableName"] == DBNull.Value ? null : (string)reader["TableName"];

   //                     list.Add(temp);
   //                 }
                    

   //                 reader.NextResult();
   //                 while (reader.Read())
   //                 {
   //                     iTotalRowsNum = (int)reader["TotalRows"];
   //                 }


   //             }

   //             connection.Close();
   //             connection.Dispose();

   //             return list;
   //         }
   //     }
   // }



    public static DataTable  ets_Menu_Select(int? nMenuID, string sMenu,
     bool? bShowOnMenu, int? nAccountID, bool? bIsActive, string sOrder,
   string sOrderDirection, int? nStartRow, int? nMaxRows,
        ref int iTotalRowsNum, int? nParentMenuID, bool? bWithTable)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Menu_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

                if (bWithTable != null)
                    command.Parameters.Add(new SqlParameter("@bWithTable", bWithTable));

                if (nMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nMenuID", nMenuID));

                if (nParentMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nParentMenuID", nParentMenuID));

                if (sMenu != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sMenu", sMenu));

                if (bShowOnMenu != null)
                    command.Parameters.Add(new SqlParameter("@bShowOnMenu", bShowOnMenu));


                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "MenuID"; sOrderDirection = "DESC"; }

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
    public static Menu ets_Menu_Details(int nMenuID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Menu_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nMenuID", nMenuID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Menu temp = new Menu(
                                (int)reader["MenuID"],
                                (string)reader["Menu"],
                                  (int?)reader["AccountID"],
                                 (bool)reader["ShowOnMenu"],
                                 (bool)reader["IsActive"]
                                 );
                            temp.ParentMenuID = reader["ParentMenuID"] == DBNull.Value ? null : (int?)reader["ParentMenuID"];
                            temp.TableID = reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"];

                            temp.DocumentID = reader["DocumentID"] == DBNull.Value ? null : (int?)reader["DocumentID"];
                            //temp.CustomPageLink = reader["CustomPageLink"] == DBNull.Value ? null : (string)reader["CustomPageLink"];

                            temp.OpenInNewWindow = reader["OpenInNewWindow"] == DBNull.Value ? null : (bool?)reader["OpenInNewWindow"];
                            temp.ExternalPageLink = reader["ExternalPageLink"] == DBNull.Value ? null : (string)reader["ExternalPageLink"];
                            temp.DocumentTypeID = reader["DocumentTypeID"] == DBNull.Value ? null : (int?)reader["DocumentTypeID"];
                            temp.MenuType = reader["MenuType"] == DBNull.Value ? "" : (string)reader["MenuType"];

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



    public static Menu ets_Menu_By_TableID(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Menu_By_TableID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                connection.Open();


                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Menu temp = new Menu(
                                (int)reader["MenuID"],
                                (string)reader["Menu"],
                                  (int?)reader["AccountID"],
                                 (bool)reader["ShowOnMenu"],
                                 (bool)reader["IsActive"]
                                 );
                            temp.ParentMenuID = reader["ParentMenuID"] == DBNull.Value ? null : (int?)reader["ParentMenuID"];
                            temp.TableID = reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"];

                            temp.DocumentID = reader["DocumentID"] == DBNull.Value ? null : (int?)reader["DocumentID"];
                            //temp.CustomPageLink = reader["CustomPageLink"] == DBNull.Value ? null : (string)reader["CustomPageLink"];

                            temp.OpenInNewWindow = reader["OpenInNewWindow"] == DBNull.Value ? null : (bool?)reader["OpenInNewWindow"];
                            temp.ExternalPageLink = reader["ExternalPageLink"] == DBNull.Value ? null : (string)reader["ExternalPageLink"];

                            temp.DocumentTypeID = reader["DocumentTypeID"] == DBNull.Value ? null : (int?)reader["DocumentTypeID"];
                            temp.MenuType = reader["MenuType"] == DBNull.Value ? "" : (string)reader["MenuType"];

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

    #region Table

    public static void PopulateTableCheckBoxList(int iColumnID, ref  CheckBoxList cb)
    {

        Column theColumn = RecordManager.ets_Column_Details(iColumnID);

        if(theColumn.TableTableID!=null && theColumn.DisplayName!="")
        {
            DropDownList ddl = new DropDownList();
            PopulateTableDropDown(iColumnID, ref  ddl);
            cb.Items.Clear();
            if (ddl.Items.Count > 0)
            {
                foreach (ListItem ddlLI in ddl.Items)
                {
                    if (ddlLI.Value != "")
                        cb.Items.Add(ddlLI);
                }
            }
        }
        else
        {
            DataTable dtTemp = Common.DataTableFromText(
            String.Format("SELECT DISTINCT [{0}] FROM Record WHERE TableID={1} AND IsActive=1 AND [{0}] IS NOT NULL ORDER BY [{0}]",
                theColumn.SystemName, theColumn.TableID.ToString()));

            if (dtTemp != null)
            {
                if (dtTemp.Columns.Count > 1)
                {
                    foreach (DataRow dr in dtTemp.Rows)
                    {

                        cb.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                    }
                }
                else
                {
                    foreach (DataRow dr in dtTemp.Rows)
                    {

                        cb.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
                    }
                }


            }
        }
       

    }
    
    public static void PopulateTableDropDown(int iColumnID, ref  DropDownList ddl)
    {
        try
        {
            ddl.Items.Clear();
            string regex = @"\[(.*?)\]";

            Column theColumn = RecordManager.ets_Column_Details(iColumnID);

            if (theColumn != null)
            {
                if (theColumn.TableTableID != null && (int)theColumn.TableTableID>-1)
                {
                    //string strDisplayColumn = theColumn.DisplayColumn;
                    //string text = theColumn.DisplayColumn;

                    //string strDCForSQL = strDisplayColumn.Replace("'", "''");
                    //int i = 1;
                    //string strFirstSystemName = "";

                    //foreach (Match match in Regex.Matches(text, regex))
                    //{
                    //    string strEachDisplayName = match.Groups[1].Value;

                    //    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,ColumnType FROM [Column] WHERE   TableID ="
                    //        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

                    //    string strEachSystemName = "";
                    //    if (dtTableTableSC.Rows.Count > 0)
                    //    {
                    //        if (dtTableTableSC.Rows[0]["ColumnType"].ToString() == "date")
                    //        {
                    //            strEachSystemName = "CONVERT(VARCHAR, CONVERT (DATE," + dtTableTableSC.Rows[0]["SystemName"].ToString() + ",103),103)";
                    //        }
                    //        else
                    //        {
                    //            strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
                    //        }
                    //    }


                    //    if (i == 1)
                    //    {
                    //        strFirstSystemName = strEachSystemName;
                    //        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                    //            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
                    //    }
                    //    else
                    //    {
                    //        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                    //            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
                    //    }
                    //    i = i + 1;
                    //}

                    //strDCForSQL = strDCForSQL.Trim() + "'";

                    //Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

                    //string strWhere = "";

                    //if (theColumn.FilterParentColumnID != null && theColumn.FilterValue != "")
                    //{
                    //    Column theFilterParentColumnID = RecordManager.ets_Column_Details((int)theColumn.FilterParentColumnID);
                    //    strWhere = " AND " + theFilterParentColumnID.SystemName + " IN ('" + theColumn.FilterValue.Replace("'", "''").Replace(",","','") + "')";
                    //}

                    ListItem li1 = new ListItem("--Please Select--", "");
                    ddl.Items.Add(li1);

                    


                    try
                    {
//                        DataTable dtData = Common.DataTableFromText(@"SELECT " + theLinkedColumn.SystemName.ToString() + "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " " + strWhere + " ORDER BY " + strFirstSystemName);

                        string strWhere = "";

                        if (theColumn.FilterParentColumnID != null && theColumn.FilterValue != "")
                        {
                            Column theFilterParentColumnID = RecordManager.ets_Column_Details((int)theColumn.FilterParentColumnID);

                            if (theColumn.FilterOperator == "contains")
                            {
                                strWhere = " AND Record." + theFilterParentColumnID.SystemName + " LIKE '%" + theColumn.FilterValue.Replace("'", "''") + "%'";
                            }
                            else if (theColumn.FilterOperator == "greaterthan")
                            {
                                strWhere = " AND ( IsNumeric(Record." + theFilterParentColumnID.SystemName + ")=1 AND  CONVERT(decimal(20,10),Record." + theFilterParentColumnID.SystemName + ") > " + Common.IgnoreSymbols(theColumn.FilterValue) + ")";
                            }
                            else if (theColumn.FilterOperator == "lessthan")
                            {
                                strWhere = " AND ( IsNumeric(Record." + theFilterParentColumnID.SystemName + ")=1 AND  CONVERT(decimal(20,10),Record." + theFilterParentColumnID.SystemName + ") < " + Common.IgnoreSymbols(theColumn.FilterValue) + ")";
                            }
                            else if (theColumn.FilterOperator == "empty")
                            {
                                strWhere = " AND (Record." + theFilterParentColumnID.SystemName + " IS NULL OR LEN(Record." + theFilterParentColumnID.SystemName + ")=0) ";
                            }
                            else if (theColumn.FilterOperator == "notempty")
                            {
                                strWhere = " AND (Record." + theFilterParentColumnID.SystemName + " IS NOT NULL AND LEN(Record." + theFilterParentColumnID.SystemName + ")>0) ";
                            }
                            else
                            {
                                strWhere = " AND Record." + theFilterParentColumnID.SystemName + " IN ('" + theColumn.FilterValue.Replace("'", "''").Replace(",", "','") + "')";

                            }
                        }

                        DataTable dtData = Common.spGetLinkedRecordIDnDisplayText(theColumn.DisplayColumn, (int)theColumn.TableTableID, null, strWhere, "");

                        foreach (DataRow dr in dtData.Rows)
                        {

                            ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                            ddl.Items.Add(li);
                        }
                    }
                    catch
                    {
                        //

                    }

                }
                else if (theColumn.TableTableID != null && theColumn.TableTableID==-1)
                {
                    try
                    {
                        if (theColumn.DisplayColumn != "")
                        {
                            Table thisTable = RecordManager.ets_Table_Details((int)theColumn.TableID);
                            DataTable dtData;
                            if (theColumn.DisplayColumn == "[Name]")
                            {
                                dtData = Common.DataTableFromText(@"SELECT U.UserID,ISNULL(U.FirstName,'') +' ' + ISNULL(U.LastName,'') AS FullName 
                                                    FROM [User] U INNER JOIN [UserRole] UR ON U.UserID=UR.UserID 
                                                    WHERE U.IsActive=1 AND UR.AccountID=" + thisTable.AccountID.ToString() + @" ORDER BY FullName");
                            }
                            else
                            {
                                dtData = Common.DataTableFromText(@"SELECT U.UserID,U.Email 
                                                    FROM [User] U INNER JOIN [UserRole] UR
                                                    ON U.UserID=UR.UserID WHERE  U.IsActive=1 AND UR.AccountID=" + thisTable.AccountID.ToString() + @" ORDER BY U.Email ");
                            }
                            ListItem li1 = new ListItem("--Please Select--", "");
                            ddl.Items.Add(li1);

                            foreach (DataRow dr in dtData.Rows)
                            {

                                ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
                                ddl.Items.Add(li);
                            }

                        }
                    }
                    catch
                    {
                        //
                    }

                    
                    

                }

            }
        }
        catch
        {
            //
        }
       
        //return lstIDnText;


    }

    public static string GetUserDisplayName(string strDisplayColumn,string strUserID)
    {
        if (string.IsNullOrEmpty(strUserID))
        {
            return "";
        }
        if (strDisplayColumn == "[Name]")
        {
            return Common.GetValueFromSQL(@"SELECT ISNULL(FirstName,'') +' ' + ISNULL(LastName,'') AS FullName FROM [User] WHERE UserID=" + strUserID);
        }
        else
        {
            return Common.GetValueFromSQL(@"SELECT Email FROM [User] WHERE UserID=" + strUserID);
        }
    }

//    public static void PopulateTableDropDownWithFilter(int iColumnID, ref  DropDownList ddl,string strFilterValue)
//    {


//        //search = search.Replace("'", "''");

//        string regex = @"\[(.*?)\]";

//        Column theColumn = RecordManager.ets_Column_Details(iColumnID);
//        //List<IDnText> lstIDnText = new List<IDnText>();

//        if (theColumn != null)
//        {
//            if (theColumn.TableTableID != null)
//            {
//                //DataTable dtTableTableSC = Common.DataTableFromText("SELECT * FROM [Column] WHERE  TableID =" + theColumn.TableTableID.ToString());
//                string strDisplayColumn = theColumn.DisplayColumn;
//                string text = theColumn.DisplayColumn;

//                string strDCForSQL = strDisplayColumn.Replace("'", "''");
//                int i = 1;
//                //List<string> lstDisplayName = new List<string>();
//                //List<string> lstSystemName = new List<string>();
//                string strFirstSystemName = "";

//                foreach (Match match in Regex.Matches(text, regex))
//                {
//                    string strEachDisplayName = match.Groups[1].Value;

//                    //lstDisplayName.Add(strEachDisplayName);

//                    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,ColumnType FROM [Column] WHERE   TableID ="
//                        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

//                    string strEachSystemName = "";
//                    if (dtTableTableSC.Rows.Count > 0)
//                    {
//                        if (dtTableTableSC.Rows[0]["ColumnType"].ToString() == "date")
//                        {
//                            strEachSystemName = "CONVERT(VARCHAR, CONVERT (DATE," + dtTableTableSC.Rows[0]["SystemName"].ToString() + ",103),103)";
//                        }
//                        else
//                        {
//                            strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
//                        }
//                        //lstSystemName.Add(strEachSystemName);
//                    }


//                    if (i == 1)
//                    {
//                        strFirstSystemName = strEachSystemName;
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    else
//                    {
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    i = i + 1;
//                }

//                strDCForSQL = strDCForSQL.Trim() + "'";

//                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

//                string strWhere = "";

//                if (theColumn.FilterParentColumnID != null && strFilterValue != "")
//                {
//                    Column theFilterParentColumnID = RecordManager.ets_Column_Details((int)theColumn.FilterParentColumnID);
//                    strWhere = " AND " + theFilterParentColumnID.SystemName + "='" + strFilterValue.Replace("'", "''") + "'";
//                }

//                DataTable dtData = Common.DataTableFromText(@"SELECT " + theLinkedColumn.SystemName.ToString() + "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " " + strWhere + " ORDER BY " + strFirstSystemName);

//                ListItem li1 = new ListItem("--Please Select--", "");
//                ddl.Items.Add(li1);
//                foreach (DataRow dr in dtData.Rows)
//                {

//                    ListItem li = new ListItem(dr[1].ToString(), dr[0].ToString());
//                    ddl.Items.Add(li);
//                    //IDnText theIDnText = new IDnText();
//                    //theIDnText.ID = dr[0].ToString();
//                    //theIDnText.Text = dr[1].ToString();
//                    //lstIDnText.Add(theIDnText);
//                }

//            }

//        }


//        //return lstIDnText;


//    }
    public static void ets_Record_Avg_ForARecordID(int nRecordID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_Avg_ForARecordID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;

                command.Parameters.Add(new SqlParameter("@nRecordID", nRecordID));
                
                //int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //i = -1;
                }

                connection.Close();
                connection.Dispose();

                //return i;

            }
        }      

    }


    public static int ets_Table_Insert(Table p_Table,int? nParentMenuID)
    {
        
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sTableName", p_Table.TableName));

                //if (p_Table.MenuID != null)
                //    command.Parameters.Add(new SqlParameter("@nMenuID", p_Table.MenuID));

                if (nParentMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nParentMenuID", nParentMenuID));

                if (p_Table.IsImportPositional != null)
                    command.Parameters.Add(new SqlParameter("@bIsImportPositional", p_Table.IsImportPositional));

                if (p_Table.IsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", p_Table.IsActive));

                command.Parameters.Add(new SqlParameter("@iAccountID", p_Table.AccountID));


                if (p_Table.PinImage != "")
                    command.Parameters.Add(new SqlParameter("@sPinImage", p_Table.PinImage));


                if (p_Table.MaxTimeBetweenRecords != null)
                    command.Parameters.Add(new SqlParameter("@dMaxTimeBetweenRecords", p_Table.MaxTimeBetweenRecords));

                if (p_Table.MaxTimeBetweenRecordsUnit != "")
                    command.Parameters.Add(new SqlParameter("@sMaxTimeBetweenRecordsUnit", p_Table.MaxTimeBetweenRecordsUnit));


                if (p_Table.LateDataDays != null)
                    command.Parameters.Add(new SqlParameter("@nLateDataDays", p_Table.LateDataDays));
                if (p_Table.ImportDataStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nImportDataStartRow", p_Table.ImportDataStartRow));

                //if (p_Table.AddMissingLocation != null)
                //    command.Parameters.Add(new SqlParameter("@bAddMissingLocation", p_Table.AddMissingLocation));

                //if (p_Table.DisplayOrder != null)
                //    command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_Table.DisplayOrder));



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

    public static int ets_Table_Update(Table p_Table)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_Table_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                if (p_Table.DefaultImportTemplateID != null)
                    command.Parameters.Add(new SqlParameter("@DefaultImportTemplateID", p_Table.DefaultImportTemplateID));


                if (p_Table.ShowSentEmails != null)
                    command.Parameters.Add(new SqlParameter("@bShowSentEmails", p_Table.ShowSentEmails));
                if (p_Table.ShowReceivedEmails != null)
                    command.Parameters.Add(new SqlParameter("@bShowReceivedEmails", p_Table.ShowReceivedEmails));

                if (p_Table.AllowCopyRecords != null)
                    command.Parameters.Add(new SqlParameter("@bAllowCopyRecords", p_Table.AllowCopyRecords));

                if (p_Table.DataUpdateUniqueColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nDataUpdateUniqueColumnID", p_Table.DataUpdateUniqueColumnID));


                if (p_Table.NavigationArrows != null)
                    command.Parameters.Add(new SqlParameter("@bNavigationArrows", p_Table.NavigationArrows));

                if (p_Table.SaveAndAdd != null)
                    command.Parameters.Add(new SqlParameter("@bSaveAndAdd", p_Table.SaveAndAdd));

                if (p_Table.HideFilter != null)
                    command.Parameters.Add(new SqlParameter("@bHideFilter", p_Table.HideFilter));
                if (p_Table.ShowEditAfterAdd != null)
                    command.Parameters.Add(new SqlParameter("@bShowEditAfterAdd", p_Table.ShowEditAfterAdd));

                if (p_Table.FilterTopColour != "")
                    command.Parameters.Add(new SqlParameter("@sFilterTopColour", p_Table.FilterTopColour));
                if (p_Table.FilterBottomColour != "")
                    command.Parameters.Add(new SqlParameter("@sFilterBottomColour", p_Table.FilterBottomColour));


                if (p_Table.TabColour != "")
                    command.Parameters.Add(new SqlParameter("@sTabColour", p_Table.TabColour));
                if (p_Table.TabTextColour != "")
                    command.Parameters.Add(new SqlParameter("@sTabTextColour", p_Table.TabTextColour));

                if (p_Table.BoxAroundField != null)
                    command.Parameters.Add(new SqlParameter("@bBoxAroundField", p_Table.BoxAroundField));

                if (p_Table.FilterType != "")
                    command.Parameters.Add(new SqlParameter("@sFilterType", p_Table.FilterType));

                if (p_Table.CustomUploadSheet != "")
                    command.Parameters.Add(new SqlParameter("@sCustomUploadSheet", p_Table.CustomUploadSheet));

                if (p_Table.ParentTableID != null)
                    command.Parameters.Add(new SqlParameter("@nParentTableID", p_Table.ParentTableID));

                if (p_Table.ChangeHistoryType != "")
                    command.Parameters.Add(new SqlParameter("@sChangeHistoryType", p_Table.ChangeHistoryType));

                if (p_Table.ReasonChangeType != "")
                    command.Parameters.Add(new SqlParameter("@sReasonChangeType", p_Table.ReasonChangeType));

                command.Parameters.Add(new SqlParameter("@TableID", p_Table.TableID));

                command.Parameters.Add(new SqlParameter("@TableName", p_Table.TableName));

                //command.Parameters.Add(new SqlParameter("@MenuID", p_Table.MenuID));



                command.Parameters.Add(new SqlParameter("@bIsImportPositional", p_Table.IsImportPositional));
                if (p_Table.IsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", p_Table.IsActive));


                command.Parameters.Add(new SqlParameter("@iAccountID", p_Table.AccountID));



                if (p_Table.PinImage != "")
                    command.Parameters.Add(new SqlParameter("@sPinImage", p_Table.PinImage));

                if (p_Table.MaxTimeBetweenRecords != null)
                    command.Parameters.Add(new SqlParameter("@dMaxTimeBetweenRecords", p_Table.MaxTimeBetweenRecords));

                if (p_Table.MaxTimeBetweenRecordsUnit != "")
                    command.Parameters.Add(new SqlParameter("@sMaxTimeBetweenRecordsUnit", p_Table.MaxTimeBetweenRecordsUnit));
                if (p_Table.LastUpdatedUserID != null)
                    command.Parameters.Add(new SqlParameter("@nLastUpdatedUserID ", p_Table.LastUpdatedUserID));


                if (p_Table.LateDataDays != null)
                    command.Parameters.Add(new SqlParameter("@nLateDataDays", p_Table.LateDataDays));

                if (p_Table.ImportDataStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nImportDataStartRow", p_Table.ImportDataStartRow));

                //if (p_Table.AddMissingLocation != null)
                //    command.Parameters.Add(new SqlParameter("@bAddMissingLocation", p_Table.AddMissingLocation));

                if (p_Table.DateFormat != "")
                    command.Parameters.Add(new SqlParameter("@sDateFormat", p_Table.DateFormat));

                if (p_Table.IsRecordDateUnique != null)
                    command.Parameters.Add(new SqlParameter("@bIsRecordDateUnique", p_Table.IsRecordDateUnique));

                if (p_Table.UniqueColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nUniqueColumnID", p_Table.UniqueColumnID));
                if (p_Table.UniqueColumnID2 != null)
                    command.Parameters.Add(new SqlParameter("@nUniqueColumnID2", p_Table.UniqueColumnID2));

                if (p_Table.IsDataUpdateAllowed != null)
                    command.Parameters.Add(new SqlParameter("@bIsDataUpdateAllowed", p_Table.IsDataUpdateAllowed));




                if (p_Table.FlashAlerts != null)
                    command.Parameters.Add(new SqlParameter("@bFlashAlerts", p_Table.FlashAlerts));

                if (p_Table.FilterColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nFilterColumnID", p_Table.FilterColumnID));

                if (p_Table.FilterDefaultValue != null && p_Table.FilterDefaultValue != "")
                    command.Parameters.Add(new SqlParameter("@sFilterDefaultValue", p_Table.FilterDefaultValue));


                if (p_Table.AddWithoutLogin != null)
                    command.Parameters.Add(new SqlParameter("@bAddWithoutLogin", p_Table.AddWithoutLogin));

                if (p_Table.AddUserRecord != null)
                    command.Parameters.Add(new SqlParameter("@bAddUserRecord", p_Table.AddUserRecord));
                if (p_Table.AddUserUserColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nAddUserUserColumnID", p_Table.AddUserUserColumnID));
                if (p_Table.AddUserPasswordColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nAddUserPasswordColumnID", p_Table.AddUserPasswordColumnID));
                if (p_Table.AddUserNotification != null)
                    command.Parameters.Add(new SqlParameter("@bAddUserNotification", p_Table.AddUserNotification));

                if (p_Table.ImportColumnHeaderRow != null)
                    command.Parameters.Add(new SqlParameter("@nImportColumnHeaderRow", p_Table.ImportColumnHeaderRow));

                if (p_Table.SortColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nSortColumnID", p_Table.SortColumnID));

                if (p_Table.HeaderName != "")
                    command.Parameters.Add(new SqlParameter("@sHeaderName", p_Table.HeaderName));

                if (p_Table.HideAdvancedOption != null)
                    command.Parameters.Add(new SqlParameter("@bHideAdvancedOption", p_Table.HideAdvancedOption));

                if (p_Table.ValidateColumnID1 != null)
                    command.Parameters.Add(new SqlParameter("@nValidateColumnID1", p_Table.ValidateColumnID1));
                if (p_Table.ValidateColumnID2 != null)
                    command.Parameters.Add(new SqlParameter("@nValidateColumnID2", p_Table.ValidateColumnID2));


                if (p_Table.HeaderColor != "")
                    command.Parameters.Add(new SqlParameter("@sHeaderColor", p_Table.HeaderColor));

                if (p_Table.JSONAttachmentInfo != "")
                    command.Parameters.Add(new SqlParameter("@sJSONAttachmentInfo", p_Table.JSONAttachmentInfo));
                if (p_Table.JSONAttachmentPOP3 != "")
                    command.Parameters.Add(new SqlParameter("@sJSONAttachmentPOP3", p_Table.JSONAttachmentPOP3));

                if (p_Table.ShowTabVertically != null)
                    command.Parameters.Add(new SqlParameter("@bShowTabVertically", p_Table.ShowTabVertically));

                if (p_Table.CopyToChildrenAfterImport != null)
                    command.Parameters.Add(new SqlParameter("@bCopyToChildrenAfterImport", p_Table.CopyToChildrenAfterImport));


                if (p_Table.GraphXAxisColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nGraphXAxisColumnID", p_Table.GraphXAxisColumnID));

                if (p_Table.GraphSeriesColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nGraphSeriesColumnID", p_Table.GraphSeriesColumnID));

                if (p_Table.GraphDefaultPeriod != null)
                    command.Parameters.Add(new SqlParameter("@nGraphDefaultPeriod", p_Table.GraphDefaultPeriod));

                if (p_Table.PinDisplayOrder != null)
                    command.Parameters.Add(new SqlParameter("@nPinDisplayOrder", p_Table.PinDisplayOrder));


                if (p_Table.GraphOnStart != "")
                    command.Parameters.Add(new SqlParameter("@GraphOnStart", p_Table.GraphOnStart));
                if (p_Table.GraphDefaultYAxisColumnID != null)
                    command.Parameters.Add(new SqlParameter("@GraphDefaultYAxisColumnID", p_Table.GraphDefaultYAxisColumnID));


                if (p_Table.SummaryPageContent != "")
                    command.Parameters.Add(new SqlParameter("@SummaryPageContent", p_Table.SummaryPageContent));

                //if (p_Table.BoxAroundField != null)
                //command.Parameters.Add(new SqlParameter("@bBoxAroundField", p_Table.BoxAroundField));


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



   
    public static int ets_Table_Delete(int iTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@TableID", iTableID));


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



    public static int dbg_Table_Delete_Permanent(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Table_Delete_Permanent", connection))
            {


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

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


    public static int ets_Table_UnDelete(int iTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_UnDelete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@TableID", iTableID));



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
    //public static int ets_CopyTables(string sTables, int nAccountID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_CopyTables", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@sTables", sTables));
    //            command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            return 1;

    //        }
    //    }
    //}


    public static int ets_CopyTables(int nSourceTableID, int nTargetAccountID,
        int nUserID, bool bWithData)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_CopyTables", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nSourceTableID", nSourceTableID));
                command.Parameters.Add(new SqlParameter("@nTargetAccountID", nTargetAccountID));
                command.Parameters.Add(new SqlParameter("@nUserID", nUserID));
                command.Parameters.Add(new SqlParameter("@bWithData", bWithData));


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


    //public static Table ets_Table_Details(int nTableID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Table_Details", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

    //            //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
    //            command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
    //            command.CommandTimeout = 0;
    //            connection.Open();

    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    Table temp = new Table(
    //                       (int)reader["TableID"],
    //                       (string)reader["TableName"],
    //                       (DateTime)reader["DateAdded"],
    //                       (DateTime)reader["DateUpdated"],
    //                        (bool)reader["IsImportPositional"], (bool)reader["IsActive"]
    //                        );

    //                    temp.AccountID = reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"];

    //                    temp.PinImage = reader["PinImage"] == DBNull.Value ? "" : (string)reader["PinImage"];
    //                    temp.MaxTimeBetweenRecords = reader["MaxTimeBetweenRecords"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxTimeBetweenRecords"].ToString());
    //                    temp.MaxTimeBetweenRecordsUnit = reader["MaxTimeBetweenRecordsUnit"] == DBNull.Value ? "" : (string)reader["MaxTimeBetweenRecordsUnit"];
    //                    temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
    //                    temp.LateDataDays = reader["LateDataDays"] == DBNull.Value ? null : (int?)reader["LateDataDays"];
    //                    temp.ImportDataStartRow = reader["ImportDataStartRow"] == DBNull.Value ? null : (int?)reader["ImportDataStartRow"];
    //                    //temp.AddMissingLocation = reader["AddMissingLocation"] == DBNull.Value ? null : (bool?)reader["AddMissingLocation"];
    //                    temp.IsRecordDateUnique = reader["IsRecordDateUnique"] == DBNull.Value ? null : (bool?)reader["IsRecordDateUnique"];
    //                    temp.UniqueColumnID = reader["UniqueColumnID"] == DBNull.Value ? null : (int?)reader["UniqueColumnID"];
    //                    temp.UniqueColumnID2 = reader["UniqueColumnID2"] == DBNull.Value ? null : (int?)reader["UniqueColumnID2"];


    //                    temp.FlashAlerts = reader["FlashAlerts"] == DBNull.Value ? null : (bool?)reader["FlashAlerts"];

    //                    temp.DateFormat = (string)reader["DateFormat"];

    //                    temp.FilterColumnID = reader["FilterColumnID"] == DBNull.Value ? null : (int?)reader["FilterColumnID"];
    //                    temp.FilterDefaultValue = reader["FilterDefaultValue"] == DBNull.Value ? "" : (string)reader["FilterDefaultValue"];

    //                    temp.ReasonChangeType = reader["ReasonChangeType"] == DBNull.Value ? "" : (string)reader["ReasonChangeType"];
    //                    temp.ChangeHistoryType = reader["ChangeHistoryType"] == DBNull.Value ? "" : (string)reader["ChangeHistoryType"];

    //                    temp.AddWithoutLogin = reader["AddWithoutLogin"] == DBNull.Value ? null : (bool?)reader["AddWithoutLogin"];
    //                    temp.ParentTableID = reader["ParentTableID"] == DBNull.Value ? null : (int?)reader["ParentTableID"];


    //                    temp.AddUserRecord = reader["AddUserRecord"] == DBNull.Value ? null : (bool?)reader["AddUserRecord"];
    //                    temp.AddUserUserColumnID = reader["AddUserUserColumnID"] == DBNull.Value ? null : (int?)reader["AddUserUserColumnID"];
    //                    temp.AddUserPasswordColumnID = reader["AddUserPasswordColumnID"] == DBNull.Value ? null : (int?)reader["AddUserPasswordColumnID"];
    //                    temp.AddUserNotification = reader["AddUserNotification"] == DBNull.Value ? null : (bool?)reader["AddUserNotification"];

    //                    temp.ImportColumnHeaderRow = reader["ImportColumnHeaderRow"] == DBNull.Value ? null : (int?)reader["ImportColumnHeaderRow"];
    //                    temp.SortColumnID = reader["SortColumnID"] == DBNull.Value ? null : (int?)reader["SortColumnID"];

    //                    temp.HeaderName = reader["HeaderName"] == DBNull.Value ? "" : (string)reader["HeaderName"];

    //                    temp.HideAdvancedOption = reader["HideAdvancedOption"] == DBNull.Value ? null : (bool?)reader["HideAdvancedOption"];


    //                    temp.ValidateColumnID1 = reader["ValidateColumnID1"] == DBNull.Value ? null : (int?)reader["ValidateColumnID1"];
    //                    temp.ValidateColumnID2 = reader["ValidateColumnID2"] == DBNull.Value ? null : (int?)reader["ValidateColumnID2"];

    //                    temp.HeaderColor = reader["HeaderColor"] == DBNull.Value ? "" : (string)reader["HeaderColor"];

    //                    temp.JSONAttachmentPOP3 = reader["JSONAttachmentPOP3"] == DBNull.Value ? "" : (string)reader["JSONAttachmentPOP3"];
    //                    temp.JSONAttachmentInfo = reader["JSONAttachmentInfo"] == DBNull.Value ? "" : (string)reader["JSONAttachmentInfo"];

    //                    temp.ShowTabVertically = reader["ShowTabVertically"] == DBNull.Value ? null : (bool?)reader["ShowTabVertically"];

    //                    temp.CopyToChildrenAfterImport = reader["CopyToChildrenAfterImport"] == DBNull.Value ? null : (bool?)reader["CopyToChildrenAfterImport"];

    //                    temp.CustomUploadSheet = reader["CustomUploadSheet"] == DBNull.Value ? "" : (string)reader["CustomUploadSheet"];
    //                    temp.FilterType = reader["FilterType"] == DBNull.Value ? "" : (string)reader["FilterType"];
    //                    temp.TabTextColour = reader["TabTextColour"] == DBNull.Value ? "" : (string)reader["TabTextColour"];
    //                    temp.TabColour = reader["TabColour"] == DBNull.Value ? "" : (string)reader["TabColour"];
    //                    temp.BoxAroundField = reader["BoxAroundField"] == DBNull.Value ? null : (bool?)reader["BoxAroundField"];
    //                    temp.FilterTopColour = reader["FilterTopColour"] == DBNull.Value ? "" : (string)reader["FilterTopColour"];
    //                    temp.FilterBottomColour = reader["FilterBottomColour"] == DBNull.Value ? "" : (string)reader["FilterBottomColour"];

    //                    temp.ShowEditAfterAdd = reader["ShowEditAfterAdd"] == DBNull.Value ? null : (bool?)reader["ShowEditAfterAdd"];
    //                    temp.AddOpensForm = reader["AddOpensForm"] == DBNull.Value ? null : (bool?)reader["AddOpensForm"];
    //                    temp.AddRecordSP = reader["AddRecordSP"] == DBNull.Value ? "" : (string)reader["AddRecordSP"];
    //                    temp.SPSaveRecord = reader["SPSaveRecord"] == DBNull.Value ? "" : (string)reader["SPSaveRecord"];

    //                    temp.HideFilter = reader["HideFilter"] == DBNull.Value ? null : (bool?)reader["HideFilter"];

    //                    temp.SaveAndAdd = reader["SaveAndAdd"] == DBNull.Value ? null : (bool?)reader["SaveAndAdd"];
    //                    temp.NavigationArrows = reader["NavigationArrows"] == DBNull.Value ? null : (bool?)reader["NavigationArrows"];

    //                    temp.GraphXAxisColumnID = reader["GraphXAxisColumnID"] == DBNull.Value ? null : (int?)reader["GraphXAxisColumnID"];
    //                    temp.GraphSeriesColumnID = reader["GraphSeriesColumnID"] == DBNull.Value ? null : (int?)reader["GraphSeriesColumnID"];
    //                    temp.GraphDefaultPeriod = reader["GraphDefaultPeriod"] == DBNull.Value ? null : (int?)reader["GraphDefaultPeriod"];
    //                    temp.DataUpdateUniqueColumnID = reader["DataUpdateUniqueColumnID"] == DBNull.Value ? null : (int?)reader["DataUpdateUniqueColumnID"];
    //                    temp.AllowCopyRecords = reader["AllowCopyRecords"] == DBNull.Value ? null : (bool?)reader["AllowCopyRecords"];

    //                    temp.SPSendEmail = reader["SPSendEmail"] == DBNull.Value ? "" : (string)reader["SPSendEmail"];
    //                    temp.SPUpdateConfirm = reader["SPUpdateConfirm"] == DBNull.Value ? "" : (string)reader["SPUpdateConfirm"];

    //                    temp.ShowSentEmails = reader["ShowSentEmails"] == DBNull.Value ? null : (bool?)reader["ShowSentEmails"];
    //                    temp.ShowReceivedEmails = reader["ShowReceivedEmails"] == DBNull.Value ? null : (bool?)reader["ShowReceivedEmails"];

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



   

    public static TableTab ets_TableTab_Detail(int nTableTabID)
    {
        
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TableTab_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
              
                command.Parameters.Add(new SqlParameter("@nTableTabID", nTableTabID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TableTab temp = new TableTab(
                                (int)reader["TableTabID"], (int)reader["TableID"], (string)reader["TabName"],
                               (int)reader["DisplayOrder"]
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






    public static int dbg_TableTab_Insert(TableTab p_TableTab)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_TableTab_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);


                command.Parameters.Add(new SqlParameter("@nTableID", p_TableTab.TableID));

                command.Parameters.Add(new SqlParameter("@sTabName", p_TableTab.TabName));

                command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_TableTab.DisplayOrder));



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


    public static int dbg_TableTab_Update(TableTab p_TableTab)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_TableTab_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableTabID", p_TableTab.TableTabID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_TableTab.TableID));



                command.Parameters.Add(new SqlParameter("@sTabName", p_TableTab.TabName));


                command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_TableTab.DisplayOrder));



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



    public static DataTable dbg_TableTab_Select(int? nTableID, string sTabName, int? nDisplayOrder,
        string sIncompleteImage,
        string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_TableTab_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                if (sTabName != "")
                    command.Parameters.Add(new SqlParameter("@sTabName", sTabName));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nDisplayOrder != null)
                    command.Parameters.Add(new SqlParameter("@nDisplayOrder", nDisplayOrder));

              

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "TableTabID"; sOrderDirection = "DESC"; }

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





    public static int dbg_TableTab_Delete(int nTableTabID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_TableTab_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableTabID ", nTableTabID));

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


    public static Table ets_Table_Details(int nTableID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Table temp = new Table(
                               (int)reader["TableID"],
                               (string)reader["TableName"],
                               (DateTime)reader["DateAdded"],
                               (DateTime)reader["DateUpdated"],
                                 (bool)reader["IsImportPositional"], (bool)reader["IsActive"]
                                );

                            temp.AccountID = reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"];

                            temp.PinImage = reader["PinImage"] == DBNull.Value ? "" : (string)reader["PinImage"];
                            temp.MaxTimeBetweenRecords = reader["MaxTimeBetweenRecords"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxTimeBetweenRecords"].ToString());
                            temp.MaxTimeBetweenRecordsUnit = reader["MaxTimeBetweenRecordsUnit"] == DBNull.Value ? "" : (string)reader["MaxTimeBetweenRecordsUnit"];
                            temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
                            temp.LateDataDays = reader["LateDataDays"] == DBNull.Value ? null : (int?)reader["LateDataDays"];
                            temp.ImportDataStartRow = reader["ImportDataStartRow"] == DBNull.Value ? null : (int?)reader["ImportDataStartRow"];
                            //temp.AddMissingLocation = reader["AddMissingLocation"] == DBNull.Value ? null : (bool?)reader["AddMissingLocation"];
                            temp.IsRecordDateUnique = reader["IsRecordDateUnique"] == DBNull.Value ? null : (bool?)reader["IsRecordDateUnique"];
                            temp.UniqueColumnID = reader["UniqueColumnID"] == DBNull.Value ? null : (int?)reader["UniqueColumnID"];
                            temp.UniqueColumnID2 = reader["UniqueColumnID2"] == DBNull.Value ? null : (int?)reader["UniqueColumnID2"];
                            temp.IsDataUpdateAllowed = reader["IsDataUpdateAllowed"] == DBNull.Value ? null : (bool?)reader["IsDataUpdateAllowed"];
                            temp.FlashAlerts = reader["FlashAlerts"] == DBNull.Value ? null : (bool?)reader["FlashAlerts"];

                            temp.DateFormat = (string)reader["DateFormat"];

                            temp.FilterColumnID = reader["FilterColumnID"] == DBNull.Value ? null : (int?)reader["FilterColumnID"];
                            temp.FilterDefaultValue = reader["FilterDefaultValue"] == DBNull.Value ? "" : (string)reader["FilterDefaultValue"];

                            temp.ReasonChangeType = reader["ReasonChangeType"] == DBNull.Value ? "" : (string)reader["ReasonChangeType"];
                            temp.ChangeHistoryType = reader["ChangeHistoryType"] == DBNull.Value ? "" : (string)reader["ChangeHistoryType"];
                            temp.AddWithoutLogin = reader["AddWithoutLogin"] == DBNull.Value ? null : (bool?)reader["AddWithoutLogin"];

                            temp.ParentTableID = reader["ParentTableID"] == DBNull.Value ? null : (int?)reader["ParentTableID"];


                            temp.AddUserRecord = reader["AddUserRecord"] == DBNull.Value ? null : (bool?)reader["AddUserRecord"];
                            temp.AddUserUserColumnID = reader["AddUserUserColumnID"] == DBNull.Value ? null : (int?)reader["AddUserUserColumnID"];
                            temp.AddUserPasswordColumnID = reader["AddUserPasswordColumnID"] == DBNull.Value ? null : (int?)reader["AddUserPasswordColumnID"];
                            temp.AddUserNotification = reader["AddUserNotification"] == DBNull.Value ? null : (bool?)reader["AddUserNotification"];
                            temp.ImportColumnHeaderRow = reader["ImportColumnHeaderRow"] == DBNull.Value ? null : (int?)reader["ImportColumnHeaderRow"];
                            temp.SortColumnID = reader["SortColumnID"] == DBNull.Value ? null : (int?)reader["SortColumnID"];
                            temp.HeaderName = reader["HeaderName"] == DBNull.Value ? "" : (string)reader["HeaderName"];
                            temp.HideAdvancedOption = reader["HideAdvancedOption"] == DBNull.Value ? null : (bool?)reader["HideAdvancedOption"];


                            temp.ValidateColumnID1 = reader["ValidateColumnID1"] == DBNull.Value ? null : (int?)reader["ValidateColumnID1"];
                            temp.ValidateColumnID2 = reader["ValidateColumnID2"] == DBNull.Value ? null : (int?)reader["ValidateColumnID2"];

                            temp.HeaderColor = reader["HeaderColor"] == DBNull.Value ? "" : (string)reader["HeaderColor"];

                            temp.JSONAttachmentPOP3 = reader["JSONAttachmentPOP3"] == DBNull.Value ? "" : (string)reader["JSONAttachmentPOP3"];
                            temp.JSONAttachmentInfo = reader["JSONAttachmentInfo"] == DBNull.Value ? "" : (string)reader["JSONAttachmentInfo"];
                            temp.ShowTabVertically = reader["ShowTabVertically"] == DBNull.Value ? null : (bool?)reader["ShowTabVertically"];
                            temp.CopyToChildrenAfterImport = reader["CopyToChildrenAfterImport"] == DBNull.Value ? null : (bool?)reader["CopyToChildrenAfterImport"];
                            temp.CustomUploadSheet = reader["CustomUploadSheet"] == DBNull.Value ? "" : (string)reader["CustomUploadSheet"];
                            temp.FilterType = reader["FilterType"] == DBNull.Value ? "" : (string)reader["FilterType"];
                            temp.TabTextColour = reader["TabTextColour"] == DBNull.Value ? "" : (string)reader["TabTextColour"];
                            temp.TabColour = reader["TabColour"] == DBNull.Value ? "" : (string)reader["TabColour"];
                            temp.BoxAroundField = reader["BoxAroundField"] == DBNull.Value ? null : (bool?)reader["BoxAroundField"];
                            temp.FilterTopColour = reader["FilterTopColour"] == DBNull.Value ? "" : (string)reader["FilterTopColour"];
                            temp.FilterBottomColour = reader["FilterBottomColour"] == DBNull.Value ? "" : (string)reader["FilterBottomColour"];
                            temp.ShowEditAfterAdd = reader["ShowEditAfterAdd"] == DBNull.Value ? null : (bool?)reader["ShowEditAfterAdd"];
                            temp.AddOpensForm = reader["AddOpensForm"] == DBNull.Value ? null : (bool?)reader["AddOpensForm"];
                            temp.AddRecordSP = reader["AddRecordSP"] == DBNull.Value ? "" : (string)reader["AddRecordSP"];
                            temp.SPSaveRecord = reader["SPSaveRecord"] == DBNull.Value ? "" : (string)reader["SPSaveRecord"];
                            temp.HideFilter = reader["HideFilter"] == DBNull.Value ? null : (bool?)reader["HideFilter"];
                            temp.SaveAndAdd = reader["SaveAndAdd"] == DBNull.Value ? null : (bool?)reader["SaveAndAdd"];
                            temp.NavigationArrows = reader["NavigationArrows"] == DBNull.Value ? null : (bool?)reader["NavigationArrows"];

                            temp.GraphXAxisColumnID = reader["GraphXAxisColumnID"] == DBNull.Value ? null : (int?)reader["GraphXAxisColumnID"];
                            temp.GraphSeriesColumnID = reader["GraphSeriesColumnID"] == DBNull.Value ? null : (int?)reader["GraphSeriesColumnID"];
                            temp.GraphDefaultPeriod = reader["GraphDefaultPeriod"] == DBNull.Value ? null : (int?)reader["GraphDefaultPeriod"];
                            temp.DataUpdateUniqueColumnID = reader["DataUpdateUniqueColumnID"] == DBNull.Value ? null : (int?)reader["DataUpdateUniqueColumnID"];
                            temp.AllowCopyRecords = reader["AllowCopyRecords"] == DBNull.Value ? null : (bool?)reader["AllowCopyRecords"];

                            temp.SPSendEmail = reader["SPSendEmail"] == DBNull.Value ? "" : (string)reader["SPSendEmail"];
                            temp.SPUpdateConfirm = reader["SPUpdateConfirm"] == DBNull.Value ? "" : (string)reader["SPUpdateConfirm"];

                            temp.ShowSentEmails = reader["ShowSentEmails"] == DBNull.Value ? null : (bool?)reader["ShowSentEmails"];
                            temp.ShowReceivedEmails = reader["ShowReceivedEmails"] == DBNull.Value ? null : (bool?)reader["ShowReceivedEmails"];

                            temp.SPAfterImport = reader["SPAfterImport"] == DBNull.Value ? "" : (string)reader["SPAfterImport"];
                            temp.PinDisplayOrder = reader["PinDisplayOrder"] == DBNull.Value ? null : (int?)reader["PinDisplayOrder"];

                            temp.GraphOnStart = reader["GraphOnStart"] == DBNull.Value ? "" : (string)reader["GraphOnStart"];
                            temp.GraphDefaultYAxisColumnID = reader["GraphDefaultYAxisColumnID"] == DBNull.Value ? null : (int?)reader["GraphDefaultYAxisColumnID"];

                            temp.SummaryPageContent = reader["SummaryPageContent"] == DBNull.Value ? "" : (string)reader["SummaryPageContent"];
                            temp.DefaultImportTemplateID = reader["DefaultImportTemplateID"] == DBNull.Value ? null : (int?)reader["DefaultImportTemplateID"];
                            

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

   

    public static int? ets_Table_MaxOrder(int nTableID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_MaxOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                connection.Open();


                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? iMax = (int?)reader[0];
                            connection.Close();
                            connection.Dispose();                          

                            return iMax;
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


    public static int ets_Table_MaxPosition(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_MaxPosition", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            int iValue = reader[0] == DBNull.Value ? 0 : (int)reader[0];
                            connection.Close();
                            connection.Dispose();

                            return iValue;
                        }

                    }
                }
                catch
                {
                  
                }
                connection.Close();
                connection.Dispose();
                
                return 0;

            }
        }
    }


    public static string ets_Table_ByUser_AdvancedSecurity(int nRoleID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_ByUser_AdvancedSecurity", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nRoleID", nRoleID));
             
                

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

              
                if (ds == null) return "";

                string strTableIDs = "-1,";

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strTableIDs = strTableIDs + dr[0].ToString() + ",";
                    }
                }

                strTableIDs = strTableIDs.Substring(0, strTableIDs.Length - 1);

                return strTableIDs;

            }
        }
    }


    public static string ets_Table_Hide_ByShowMenu(int nRoleID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Hide_ByShowMenu", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nRoleID", nRoleID));



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


                if (ds == null) return "";

                string strTableIDs = "-1,";

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strTableIDs = strTableIDs + dr[0].ToString() + ",";
                    }
                }

                strTableIDs = strTableIDs.Substring(0, strTableIDs.Length - 1);

                return strTableIDs;

            }
        }
    }
    //public static string ets_Table_ByUser_AdvancedSecurity(int nUserID, string sRecordRightIDs)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Table_ByUser_AdvancedSecurity", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nUserID", nUserID));
    //            command.Parameters.Add(new SqlParameter("@sRecordRightIDs", sRecordRightIDs));


    //            connection.Open();

    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            System.Data.DataSet ds = new System.Data.DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();

    //            string strTableIDs = "-1,";

    //            if (ds.Tables.Count > 0)
    //            {
    //                foreach (DataRow dr in ds.Tables[0].Rows)
    //                {
    //                    strTableIDs = strTableIDs + dr[0].ToString() + ",";
    //                }
    //            }

    //            strTableIDs = strTableIDs.Substring(0, strTableIDs.Length - 1);

    //            return strTableIDs;

    //        }
    //    }
    //}

    public static List<Table> ets_Table_Select(int? nTableID, string sTableName,
        int? nMenuID, int? nAccountID, DateTime? dDateAdded, DateTime? dDateUpdated, bool? bIsActive, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, string sTableIn)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (sTableName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sTableName", sTableName));

                if (nMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nMenuID", nMenuID));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));


                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                //if (nAccountID != null)
                //    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));




                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "TableID"; sOrderDirection = "DESC"; }


                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                if (sTableIn != "")
                    command.Parameters.Add(new SqlParameter("@sTableIn", sTableIn));



                connection.Open();

                List<Table> list = new List<Table>();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Table temp = new Table(
                                (int)reader["TableID"],
                                (string)reader["TableName"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"],
                                  (bool)reader["IsImportPositional"], (bool)reader["IsActive"]
                                 );
                            temp.PinImage = reader["PinImage"] == DBNull.Value ? "" : (string)reader["PinImage"];

                            temp.MaxTimeBetweenRecords = reader["MaxTimeBetweenRecords"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxTimeBetweenRecords"].ToString());
                            temp.MaxTimeBetweenRecordsUnit = reader["MaxTimeBetweenRecordsUnit"] == DBNull.Value ? "" : (string)reader["MaxTimeBetweenRecordsUnit"];
                            temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
                            temp.LateDataDays = reader["LateDataDays"] == DBNull.Value ? null : (int?)reader["LateDataDays"];
                            temp.ImportDataStartRow = reader["ImportDataStartRow"] == DBNull.Value ? null : (int?)reader["ImportDataStartRow"];
                            //temp.AddMissingLocation = reader["AddMissingLocation"] == DBNull.Value ? null : (bool?)reader["AddMissingLocation"];
                            //temp.IsRecordDateUnique = reader["IsRecordDateUnique"] == DBNull.Value ? null : (bool?)reader["IsRecordDateUnique"];



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
                    //
                }

                

                connection.Close();
                connection.Dispose();


                return list;
            }
        }
    }



    public static DataTable  ets_Table_Select_dt(int? nTableID, string sTableName,
      int? nMenuID, int? nAccountID, DateTime? dDateAdded, DateTime? dDateUpdated, bool? bIsActive, string sOrder,
    string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, string sTableIn)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (sTableName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sTableName", sTableName));

                if (nMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nMenuID", nMenuID));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));


                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                //if (nAccountID != null)
                //    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));




                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "TableID"; sOrderDirection = "DESC"; }


                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                if (sTableIn != "")
                    command.Parameters.Add(new SqlParameter("@sTableIn", sTableIn));




               

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
    //public static List<Column> ets_Table_Columns(int nTableID,
    //    int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Table_Columns", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

    //            if (nStartRow != null)
    //                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

    //            if (nMaxRows != null)
    //                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

    //            connection.Open();

    //            List<Column> list = new List<Column>();
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    Column temp = new Column(
    //                        (int)reader["ColumnID"], nTableID,
    //                        (string)reader["SystemName"],
    //                        (int)reader["DisplayOrder"],
    //                        reader["DisplayTextSummary"] == DBNull.Value ? "" : (string)reader["DisplayTextSummary"],
    //                        reader["DisplayTextDetail"] == DBNull.Value ? "" : (string)reader["DisplayTextDetail"],
    //                         reader["NameOnImport"] == DBNull.Value ? "" : (string)reader["NameOnImport"],
    //                        reader["NameOnExport"] == DBNull.Value ? "" : (string)reader["NameOnExport"],
    //                        null, "", "",
    //                        null, null, "", "", (bool)reader["IsStandard"], (string)reader["DisplayName"],
    //                        reader["PositionOnImport"] == DBNull.Value ? null : (int?)reader["PositionOnImport"],
    //                         reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
    //                        reader["IsRound"] == DBNull.Value ? null : (bool?)reader["IsRound"],
    //                         reader["RoundNumber"] == DBNull.Value ? null : (int?)reader["RoundNumber"],
    //                         reader["CheckUnlikelyValue"] == DBNull.Value ? null : (bool?)reader["CheckUnlikelyValue"],
    //                          reader["GraphLabel"] == DBNull.Value ? "" : (string)reader["GraphLabel"],
    //                          reader["DropdownValues"] == DBNull.Value ? string.Empty : (string)reader["DropdownValues"],
    //                          reader["Importance"] == DBNull.Value ? string.Empty : (string)reader["Importance"]
    //                        );

    //                    temp.Constant = reader["Constant"] == DBNull.Value ? string.Empty : (string)reader["Constant"];
    //                    temp.Calculation = reader["Calculation"] == DBNull.Value ? string.Empty : (string)reader["Calculation"];
    //                    temp.ShowTotal = reader["ShowTotal"] == DBNull.Value ? null : (bool?)reader["ShowTotal"];
    //                    temp.IgnoreSymbols = reader["IgnoreSymbols"] == DBNull.Value ? null : (bool?)reader["IgnoreSymbols"];
    //                    temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
    //                    temp.Alignment = reader["Alignment"] == DBNull.Value ? string.Empty : (string)reader["Alignment"];

                       
    //                    temp.NumberType = reader["NumberType"] == DBNull.Value ? null : (int?)reader["NumberType"];
    //                    temp.DefaultValue = reader["DefaultValue"] == DBNull.Value ? string.Empty : (string)reader["DefaultValue"];

    //                    temp.AvgColumnID = reader["AvgColumnID"] == DBNull.Value ? null : (int?)reader["AvgColumnID"];
    //                    temp.AvgNumberOfRecords = reader["AvgNumberOfRecords"] == DBNull.Value ? null : (int?)reader["AvgNumberOfRecords"];
    //                    temp.MobileName = reader["MobileName"] == DBNull.Value ? string.Empty : (string)reader["MobileName"];
    //                    temp.IsDateSingleColumn = reader["IsDateSingleColumn"] == DBNull.Value ? null : (bool?)reader["IsDateSingleColumn"];
    //                    temp.ShowGraphExceedance = reader["ShowGraphExceedance"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphExceedance"].ToString());
    //                    temp.ShowGraphWarning = reader["ShowGraphWarning"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphWarning"].ToString());

    //                    temp.FlatLineNumber = reader["FlatLineNumber"] == DBNull.Value ? null : (int?)reader["FlatLineNumber"];

    //                    temp.MaxValueAt = reader["MaxValueAt"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxValueAt"].ToString());
    //                    temp.DefaultGraphDefinitionID = reader["DefaultGraphDefinitionID"] == DBNull.Value ? null : (int?)int.Parse(reader["DefaultGraphDefinitionID"].ToString());

    //                    temp.TextWidth = reader["TextWidth"] == DBNull.Value ? null : (int?)reader["TextWidth"];
    //                    temp.TextHeight = reader["TextHeight"] == DBNull.Value ? null : (int?)reader["TextHeight"];
    //                    temp.ColumnType = reader["ColumnType"] == DBNull.Value ? "" : (string)reader["ColumnType"];
    //                    temp.DropDownType = reader["DropDownType"] == DBNull.Value ? null : (string)reader["DropDownType"];
    //                    temp.TableTableID = reader["TableTableID"] == DBNull.Value ? null : (int?)reader["TableTableID"];
    //                    temp.DisplayColumn = reader["DisplayColumn"] == DBNull.Value ? "" : (string)reader["DisplayColumn"];
    //                    temp.DisplayRight = reader["DisplayRight"] == DBNull.Value ? null : (bool?)reader["DisplayRight"];

    //                    temp.SummaryCellBackColor = reader["SummaryCellBackColor"] == DBNull.Value ? "" : (string)reader["SummaryCellBackColor"];

    //                    temp.ParentColumnID = reader["ParentColumnID"] == DBNull.Value ? null : (int?)reader["ParentColumnID"];
    //                    temp.TextType = reader["TextType"] == DBNull.Value ? "" : (string)reader["TextType"];
    //                    temp.RegEx = reader["RegEx"] == DBNull.Value ? "" : (string)reader["RegEx"];

    //                    //temp.HideColumnID = reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"];
    //                    //temp.HideColumnValue = reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"];
    //                    temp.DateCalculationType = reader["DateCalculationType"] == DBNull.Value ? "" : (string)reader["DateCalculationType"];

    //                    temp.OnlyForAdmin = reader["OnlyForAdmin"] == DBNull.Value ? null : (int?)reader["OnlyForAdmin"];
    //                    temp.IsSystemColumn = (bool)reader["IsSystemColumn"];
    //                    temp.LinkedParentColumnID = reader["LinkedParentColumnID"] == DBNull.Value ? null : (int?)reader["LinkedParentColumnID"];
    //                    //temp.DataRetrieverID = reader["DataRetrieverID"] == DBNull.Value ? null : (int?)reader["DataRetrieverID"];
    //                    temp.VerticalList = reader["VerticalList"] == DBNull.Value ? null : (bool?)reader["VerticalList"];
    //                    temp.SummarySearch = reader["SummarySearch"] == DBNull.Value ? null : (bool?)reader["SummarySearch"];
    //                    temp.QuickAddLink = reader["QuickAddLink"] == DBNull.Value ? null : (bool?)reader["QuickAddLink"];
    //                    //temp.HideOperator = reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"];
    //                    temp.CalculationIsActive = reader["CalculationIsActive"] == DBNull.Value ? null : (bool?)reader["CalculationIsActive"];
    //                    temp.TableTabID = reader["TableTabID"] == DBNull.Value ? null : (int?)reader["TableTabID"];
    //                    temp.ViewName = reader["ViewName"] == DBNull.Value ? string.Empty : (string)reader["ViewName"];
    //                    temp.SPDefaultValue = reader["SPDefaultValue"] == DBNull.Value ? string.Empty : (string)reader["SPDefaultValue"];

    //                    temp.DefaultColumnID = reader["DefaultColumnID"] == DBNull.Value ? null : (int?)reader["DefaultColumnID"];
    //                    temp.DefaultType = reader["DefaultType"] == DBNull.Value ? string.Empty : (string)reader["DefaultType"];
    //                    temp.ShowViewLink = reader["ShowViewLink"] == DBNull.Value ? string.Empty : (string)reader["ShowViewLink"];

    //                    temp.FilterParentColumnID = reader["FilterParentColumnID"] == DBNull.Value ? null : (int?)reader["FilterParentColumnID"];
    //                    temp.FilterOtherColumnID = reader["FilterOtherColumnID"] == DBNull.Value ? null : (int?)reader["FilterOtherColumnID"];
    //                    temp.FilterValue = reader["FilterValue"] == DBNull.Value ? string.Empty : (string)reader["FilterValue"];
    //                    temp.MapPinHoverColumnID = reader["MapPinHoverColumnID"] == DBNull.Value ? null : (int?)reader["MapPinHoverColumnID"];

    //                    temp.CompareColumnID = reader["CompareColumnID"] == DBNull.Value ? null : (int?)reader["CompareColumnID"];
    //                    temp.CompareOperator = reader["CompareOperator"] == DBNull.Value ? string.Empty : (string)reader["CompareOperator"];
    //                    temp.MapPopup = reader["MapPopup"] == DBNull.Value ? string.Empty : (string)reader["MapPopup"];
    //                    temp.TrafficLightColumnID = reader["TrafficLightColumnID"] == DBNull.Value ? null : (int?)reader["TrafficLightColumnID"];
    //                    temp.TrafficLightValues = reader["TrafficLightValues"] == DBNull.Value ? string.Empty : (string)reader["TrafficLightValues"];
    //                    temp.DefaultRelatedTableID = reader["DefaultRelatedTableID"] == DBNull.Value ? null : (int?)reader["DefaultRelatedTableID"];
    //                    temp.DefaultUpdateValues = reader["DefaultUpdateValues"] == DBNull.Value ? null : (bool?)reader["DefaultUpdateValues"];

    //                    temp.ValidationCanIgnore = reader["ValidationCanIgnore"] == DBNull.Value ? null : (bool?)reader["ValidationCanIgnore"];
    //                    temp.ImageOnSummary = reader["ImageOnSummary"] == DBNull.Value ? null : (bool?)reader["ImageOnSummary"];
    //                    temp.AllowCopy = reader["AllowCopy"] == DBNull.Value ? null : (bool?)reader["AllowCopy"];
    //                    temp.ValidationOnExceedance = reader["ValidationOnExceedance"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnExceedance"];
    //                    temp.ColourCells = reader["ColourCells"] == DBNull.Value ? null : (bool?)reader["ColourCells"];
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





    public static List<Column> ets_Table_Columns(int nTableID,
      int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
              
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                connection.Open();

                List<Column> list = new List<Column>();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Column temp = new Column(
                                (int)reader["ColumnID"], nTableID,
                                (string)reader["SystemName"],
                                (int)reader["DisplayOrder"],
                                reader["DisplayTextSummary"] == DBNull.Value ? "" : (string)reader["DisplayTextSummary"],
                                reader["DisplayTextDetail"] == DBNull.Value ? "" : (string)reader["DisplayTextDetail"],
                                 reader["NameOnImport"] == DBNull.Value ? "" : (string)reader["NameOnImport"],
                                reader["NameOnExport"] == DBNull.Value ? "" : (string)reader["NameOnExport"],
                                null, "", "",
                                null, null, "", "", (bool)reader["IsStandard"], (string)reader["DisplayName"],
                                reader["PositionOnImport"] == DBNull.Value ? null : (int?)reader["PositionOnImport"],
                                 reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
                                reader["IsRound"] == DBNull.Value ? null : (bool?)reader["IsRound"],
                                 reader["RoundNumber"] == DBNull.Value ? null : (int?)reader["RoundNumber"],
                                 reader["CheckUnlikelyValue"] == DBNull.Value ? null : (bool?)reader["CheckUnlikelyValue"],
                                  reader["GraphLabel"] == DBNull.Value ? "" : (string)reader["GraphLabel"],
                                  reader["DropdownValues"] == DBNull.Value ? string.Empty : (string)reader["DropdownValues"],
                                   reader["Importance"] == DBNull.Value ? string.Empty : (string)reader["Importance"]
                                );

                            temp.Constant = reader["Constant"] == DBNull.Value ? string.Empty : (string)reader["Constant"];
                            temp.Calculation = reader["Calculation"] == DBNull.Value ? string.Empty : (string)reader["Calculation"];
                            temp.ShowTotal = reader["ShowTotal"] == DBNull.Value ? null : (bool?)reader["ShowTotal"];
                            temp.IgnoreSymbols = reader["IgnoreSymbols"] == DBNull.Value ? null : (bool?)reader["IgnoreSymbols"];
                            temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
                            temp.Alignment = reader["Alignment"] == DBNull.Value ? string.Empty : (string)reader["Alignment"];


                            temp.NumberType = reader["NumberType"] == DBNull.Value ? null : (int?)reader["NumberType"];
                            temp.DefaultValue = reader["DefaultValue"] == DBNull.Value ? string.Empty : (string)reader["DefaultValue"];

                            temp.AvgColumnID = reader["AvgColumnID"] == DBNull.Value ? null : (int?)reader["AvgColumnID"];
                            temp.AvgNumberOfRecords = reader["AvgNumberOfRecords"] == DBNull.Value ? null : (int?)reader["AvgNumberOfRecords"];
                            temp.MobileName = reader["MobileName"] == DBNull.Value ? string.Empty : (string)reader["MobileName"];
                            temp.IsDateSingleColumn = reader["IsDateSingleColumn"] == DBNull.Value ? null : (bool?)reader["IsDateSingleColumn"];
                            temp.ShowGraphExceedance = reader["ShowGraphExceedance"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphExceedance"].ToString());
                            temp.ShowGraphWarning = reader["ShowGraphWarning"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphWarning"].ToString());

                            temp.FlatLineNumber = reader["FlatLineNumber"] == DBNull.Value ? null : (int?)reader["FlatLineNumber"];

                            temp.MaxValueAt = reader["MaxValueAt"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxValueAt"].ToString());
                            temp.DefaultGraphDefinitionID = reader["DefaultGraphDefinitionID"] == DBNull.Value ? null : (int?)int.Parse(reader["DefaultGraphDefinitionID"].ToString());

                            temp.TextWidth = reader["TextWidth"] == DBNull.Value ? null : (int?)reader["TextWidth"];
                            temp.TextHeight = reader["TextHeight"] == DBNull.Value ? null : (int?)reader["TextHeight"];
                            temp.ColumnType = reader["ColumnType"] == DBNull.Value ? "" : (string)reader["ColumnType"];
                            temp.DropDownType = reader["DropDownType"] == DBNull.Value ? null : (string)reader["DropDownType"];
                            temp.TableTableID = reader["TableTableID"] == DBNull.Value ? null : (int?)reader["TableTableID"];
                            temp.DisplayColumn = reader["DisplayColumn"] == DBNull.Value ? "" : (string)reader["DisplayColumn"];
                            temp.DisplayRight = reader["DisplayRight"] == DBNull.Value ? null : (bool?)reader["DisplayRight"];

                            temp.SummaryCellBackColor = reader["SummaryCellBackColor"] == DBNull.Value ? "" : (string)reader["SummaryCellBackColor"];

                            temp.ParentColumnID = reader["ParentColumnID"] == DBNull.Value ? null : (int?)reader["ParentColumnID"];
                            temp.TextType = reader["TextType"] == DBNull.Value ? "" : (string)reader["TextType"];
                            temp.RegEx = reader["RegEx"] == DBNull.Value ? "" : (string)reader["RegEx"];

                            //temp.HideColumnID = reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"];
                            //temp.HideColumnValue = reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"];
                            temp.DateCalculationType = reader["DateCalculationType"] == DBNull.Value ? "" : (string)reader["DateCalculationType"];

                            temp.OnlyForAdmin = reader["OnlyForAdmin"] == DBNull.Value ? null : (int?)reader["OnlyForAdmin"];
                            temp.IsSystemColumn = (bool)reader["IsSystemColumn"];
                            temp.LinkedParentColumnID = reader["LinkedParentColumnID"] == DBNull.Value ? null : (int?)reader["LinkedParentColumnID"];
                            //temp.DataRetrieverID = reader["DataRetrieverID"] == DBNull.Value ? null : (int?)reader["DataRetrieverID"];
                            temp.VerticalList = reader["VerticalList"] == DBNull.Value ? null : (bool?)reader["VerticalList"];
                            temp.SummarySearch = reader["SummarySearch"] == DBNull.Value ? null : (bool?)reader["SummarySearch"];
                            temp.QuickAddLink = reader["QuickAddLink"] == DBNull.Value ? null : (bool?)reader["QuickAddLink"];
                            //temp.HideOperator = reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"];
                            temp.CalculationIsActive = reader["CalculationIsActive"] == DBNull.Value ? null : (bool?)reader["CalculationIsActive"];
                            temp.TableTabID = reader["TableTabID"] == DBNull.Value ? null : (int?)reader["TableTabID"];
                            temp.ViewName = reader["ViewName"] == DBNull.Value ? string.Empty : (string)reader["ViewName"];
                            temp.SPDefaultValue = reader["SPDefaultValue"] == DBNull.Value ? string.Empty : (string)reader["SPDefaultValue"];

                            temp.DefaultColumnID = reader["DefaultColumnID"] == DBNull.Value ? null : (int?)reader["DefaultColumnID"];
                            temp.DefaultType = reader["DefaultType"] == DBNull.Value ? string.Empty : (string)reader["DefaultType"];
                            temp.ShowViewLink = reader["ShowViewLink"] == DBNull.Value ? string.Empty : (string)reader["ShowViewLink"];

                            temp.FilterParentColumnID = reader["FilterParentColumnID"] == DBNull.Value ? null : (int?)reader["FilterParentColumnID"];
                            temp.FilterOtherColumnID = reader["FilterOtherColumnID"] == DBNull.Value ? null : (int?)reader["FilterOtherColumnID"];
                            temp.FilterValue = reader["FilterValue"] == DBNull.Value ? string.Empty : (string)reader["FilterValue"];
                            temp.MapPinHoverColumnID = reader["MapPinHoverColumnID"] == DBNull.Value ? null : (int?)reader["MapPinHoverColumnID"];

                            temp.CompareColumnID = reader["CompareColumnID"] == DBNull.Value ? null : (int?)reader["CompareColumnID"];
                            temp.CompareOperator = reader["CompareOperator"] == DBNull.Value ? string.Empty : (string)reader["CompareOperator"];
                            temp.MapPopup = reader["MapPopup"] == DBNull.Value ? string.Empty : (string)reader["MapPopup"];
                            temp.TrafficLightColumnID = reader["TrafficLightColumnID"] == DBNull.Value ? null : (int?)reader["TrafficLightColumnID"];
                            temp.TrafficLightValues = reader["TrafficLightValues"] == DBNull.Value ? string.Empty : (string)reader["TrafficLightValues"];
                            temp.DefaultRelatedTableID = reader["DefaultRelatedTableID"] == DBNull.Value ? null : (int?)reader["DefaultRelatedTableID"];
                            temp.DefaultUpdateValues = reader["DefaultUpdateValues"] == DBNull.Value ? null : (bool?)reader["DefaultUpdateValues"];

                            temp.ValidationCanIgnore = reader["ValidationCanIgnore"] == DBNull.Value ? null : (bool?)reader["ValidationCanIgnore"];
                            temp.ImageOnSummary = reader["ImageOnSummary"] == DBNull.Value ? null : (bool?)reader["ImageOnSummary"];
                            temp.AllowCopy = reader["AllowCopy"] == DBNull.Value ? null : (bool?)reader["AllowCopy"];
                            temp.ValidationOnExceedance = reader["ValidationOnExceedance"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnExceedance"];
                            temp.ColourCells = reader["ColourCells"] == DBNull.Value ? null : (bool?)reader["ColourCells"];
                            temp.ButtonInfo = reader["ButtonInfo"] == DBNull.Value ? string.Empty : (string)reader["ButtonInfo"];
                            temp.FilterOperator = reader["FilterOperator"] == DBNull.Value ? string.Empty : (string)reader["FilterOperator"];
                            list.Add(temp);
                        }

                        reader.NextResult();
                        while (reader.Read())
                        {
                            iTotalRowsNum = (int)reader["TotalRows"];
                        }
                    }
                    connection.Close();
                    connection.Dispose();
                }
                catch
                {
                    
                }
                connection.Close();
                connection.Dispose();
                return list;
            }
        }


        


    }

    public static List<Column> ets_Table_Columns_Display(int nTableID,
       int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, bool? bIsStandard)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_Display", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if(bIsStandard!=null)
                    command.Parameters.Add(new SqlParameter("@bIsStandard", bIsStandard));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                connection.Open();

                List<Column> list = new List<Column>();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Column temp = new Column(
                                (int)reader["ColumnID"], nTableID,
                                (string)reader["SystemName"],
                                (int)reader["DisplayOrder"],
                                reader["DisplayTextSummary"] == DBNull.Value ? "" : (string)reader["DisplayTextSummary"],
                                reader["DisplayTextDetail"] == DBNull.Value ? "" : (string)reader["DisplayTextDetail"],
                                 reader["NameOnImport"] == DBNull.Value ? "" : (string)reader["NameOnImport"],
                                reader["NameOnExport"] == DBNull.Value ? "" : (string)reader["NameOnExport"],
                                null, "", "",
                                null, null, "", "", (bool)reader["IsStandard"], (string)reader["DisplayName"],
                                reader["PositionOnImport"] == DBNull.Value ? null : (int?)reader["PositionOnImport"],
                                 reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
                                reader["IsRound"] == DBNull.Value ? null : (bool?)reader["IsRound"],
                                 reader["RoundNumber"] == DBNull.Value ? null : (int?)reader["RoundNumber"],
                                 reader["CheckUnlikelyValue"] == DBNull.Value ? null : (bool?)reader["CheckUnlikelyValue"],
                                  reader["GraphLabel"] == DBNull.Value ? "" : (string)reader["GraphLabel"],
                                  reader["DropdownValues"] == DBNull.Value ? string.Empty : (string)reader["DropdownValues"],
                                  reader["Importance"] == DBNull.Value ? string.Empty : (string)reader["Importance"]
                                );

                            temp.Constant = reader["Constant"] == DBNull.Value ? string.Empty : (string)reader["Constant"];
                            temp.Calculation = reader["Calculation"] == DBNull.Value ? string.Empty : (string)reader["Calculation"];
                            temp.ShowTotal = reader["ShowTotal"] == DBNull.Value ? null : (bool?)reader["ShowTotal"];
                            temp.IgnoreSymbols = reader["IgnoreSymbols"] == DBNull.Value ? null : (bool?)reader["IgnoreSymbols"];
                            temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
                            temp.Alignment = reader["Alignment"] == DBNull.Value ? string.Empty : (string)reader["Alignment"];


                            temp.NumberType = reader["NumberType"] == DBNull.Value ? null : (int?)reader["NumberType"];
                            temp.DefaultValue = reader["DefaultValue"] == DBNull.Value ? string.Empty : (string)reader["DefaultValue"];

                            temp.AvgColumnID = reader["AvgColumnID"] == DBNull.Value ? null : (int?)reader["AvgColumnID"];
                            temp.AvgNumberOfRecords = reader["AvgNumberOfRecords"] == DBNull.Value ? null : (int?)reader["AvgNumberOfRecords"];
                            temp.MobileName = reader["MobileName"] == DBNull.Value ? string.Empty : (string)reader["MobileName"];
                            temp.IsDateSingleColumn = reader["IsDateSingleColumn"] == DBNull.Value ? null : (bool?)reader["IsDateSingleColumn"];
                            temp.ShowGraphExceedance = reader["ShowGraphExceedance"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphExceedance"].ToString());
                            temp.ShowGraphWarning = reader["ShowGraphWarning"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphWarning"].ToString());

                            temp.FlatLineNumber = reader["FlatLineNumber"] == DBNull.Value ? null : (int?)reader["FlatLineNumber"];
                            temp.MaxValueAt = reader["MaxValueAt"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxValueAt"].ToString());


                            temp.TextWidth = reader["TextWidth"] == DBNull.Value ? null : (int?)reader["TextWidth"];
                            temp.TextHeight = reader["TextHeight"] == DBNull.Value ? null : (int?)reader["TextHeight"];
                            temp.ColumnType = reader["ColumnType"] == DBNull.Value ? "" : (string)reader["ColumnType"];
                            temp.DropDownType = reader["DropDownType"] == DBNull.Value ? null : (string)reader["DropDownType"];
                            temp.TableTableID = reader["TableTableID"] == DBNull.Value ? null : (int?)reader["TableTableID"];
                            temp.DisplayColumn = reader["DisplayColumn"] == DBNull.Value ? "" : (string)reader["DisplayColumn"];
                            temp.DisplayRight = reader["DisplayRight"] == DBNull.Value ? null : (bool?)reader["DisplayRight"];

                            temp.SummaryCellBackColor = reader["SummaryCellBackColor"] == DBNull.Value ? "" : (string)reader["SummaryCellBackColor"];

                            temp.ParentColumnID = reader["ParentColumnID"] == DBNull.Value ? null : (int?)reader["ParentColumnID"];

                            temp.TextType = reader["TextType"] == DBNull.Value ? "" : (string)reader["TextType"];
                            temp.RegEx = reader["RegEx"] == DBNull.Value ? "" : (string)reader["RegEx"];

                            //temp.HideColumnID = reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"];
                            //temp.HideColumnValue = reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"];
                            temp.DateCalculationType = reader["DateCalculationType"] == DBNull.Value ? "" : (string)reader["DateCalculationType"];
                            temp.OnlyForAdmin = reader["OnlyForAdmin"] == DBNull.Value ? null : (int?)reader["OnlyForAdmin"];
                            temp.IsSystemColumn = (bool)reader["IsSystemColumn"];
                            temp.LinkedParentColumnID = reader["LinkedParentColumnID"] == DBNull.Value ? null : (int?)reader["LinkedParentColumnID"];
                            //temp.DataRetrieverID = reader["DataRetrieverID"] == DBNull.Value ? null : (int?)reader["DataRetrieverID"];
                            temp.VerticalList = reader["VerticalList"] == DBNull.Value ? null : (bool?)reader["VerticalList"];
                            temp.SummarySearch = reader["SummarySearch"] == DBNull.Value ? null : (bool?)reader["SummarySearch"];
                            temp.QuickAddLink = reader["QuickAddLink"] == DBNull.Value ? null : (bool?)reader["QuickAddLink"];
                            //temp.HideOperator = reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"];
                            temp.CalculationIsActive = reader["CalculationIsActive"] == DBNull.Value ? null : (bool?)reader["CalculationIsActive"];
                            temp.TableTabID = reader["TableTabID"] == DBNull.Value ? null : (int?)reader["TableTabID"];
                            temp.ViewName = reader["ViewName"] == DBNull.Value ? string.Empty : (string)reader["ViewName"];

                            temp.SPDefaultValue = reader["SPDefaultValue"] == DBNull.Value ? string.Empty : (string)reader["SPDefaultValue"];

                            temp.DefaultColumnID = reader["DefaultColumnID"] == DBNull.Value ? null : (int?)reader["DefaultColumnID"];
                            temp.DefaultType = reader["DefaultType"] == DBNull.Value ? string.Empty : (string)reader["DefaultType"];
                            temp.ShowViewLink = reader["ShowViewLink"] == DBNull.Value ? string.Empty : (string)reader["ShowViewLink"];

                            temp.FilterParentColumnID = reader["FilterParentColumnID"] == DBNull.Value ? null : (int?)reader["FilterParentColumnID"];
                            temp.FilterOtherColumnID = reader["FilterOtherColumnID"] == DBNull.Value ? null : (int?)reader["FilterOtherColumnID"];
                            temp.FilterValue = reader["FilterValue"] == DBNull.Value ? string.Empty : (string)reader["FilterValue"];
                            temp.MapPinHoverColumnID = reader["MapPinHoverColumnID"] == DBNull.Value ? null : (int?)reader["MapPinHoverColumnID"];

                            temp.CompareColumnID = reader["CompareColumnID"] == DBNull.Value ? null : (int?)reader["CompareColumnID"];
                            temp.CompareOperator = reader["CompareOperator"] == DBNull.Value ? string.Empty : (string)reader["CompareOperator"];

                            temp.MapPopup = reader["MapPopup"] == DBNull.Value ? string.Empty : (string)reader["MapPopup"];
                            temp.TrafficLightColumnID = reader["TrafficLightColumnID"] == DBNull.Value ? null : (int?)reader["TrafficLightColumnID"];
                            temp.TrafficLightValues = reader["TrafficLightValues"] == DBNull.Value ? string.Empty : (string)reader["TrafficLightValues"];
                            temp.DefaultRelatedTableID = reader["DefaultRelatedTableID"] == DBNull.Value ? null : (int?)reader["DefaultRelatedTableID"];
                            temp.DefaultUpdateValues = reader["DefaultUpdateValues"] == DBNull.Value ? null : (bool?)reader["DefaultUpdateValues"];

                            temp.ValidationCanIgnore = reader["ValidationCanIgnore"] == DBNull.Value ? null : (bool?)reader["ValidationCanIgnore"];
                            temp.ImageOnSummary = reader["ImageOnSummary"] == DBNull.Value ? null : (bool?)reader["ImageOnSummary"];
                            temp.AllowCopy = reader["AllowCopy"] == DBNull.Value ? null : (bool?)reader["AllowCopy"];
                            temp.ValidationOnExceedance = reader["ValidationOnExceedance"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnExceedance"];
                            temp.ColourCells = reader["ColourCells"] == DBNull.Value ? null : (bool?)reader["ColourCells"];
                            temp.ButtonInfo = reader["ButtonInfo"] == DBNull.Value ? string.Empty : (string)reader["ButtonInfo"];
                            temp.FilterOperator = reader["FilterOperator"] == DBNull.Value ? string.Empty : (string)reader["FilterOperator"];

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
                    //
                }



                connection.Close();
                connection.Dispose();

                return list;
            }
        }

    }


    #endregion


    #region Record Column

    //public static int ets_Column_Insert(Column p_Column)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Column_Insert", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //            pRV.Direction = ParameterDirection.Output;

    //            command.Parameters.Add(pRV);

    //            UpdateColumnForSysTableLink(ref p_Column);


    //            if (p_Column.ColourCells != null)
    //                command.Parameters.Add(new SqlParameter("@bColourCells", p_Column.ColourCells));

    //            if (p_Column.ValidationOnExceedance != "")
    //                command.Parameters.Add(new SqlParameter("@sValidationOnExceedance", p_Column.ValidationOnExceedance));

    //            if (p_Column.ImageOnSummary != null)
    //                command.Parameters.Add(new SqlParameter("@bImageOnSummary", p_Column.ImageOnSummary));

    //            if (p_Column.ValidationCanIgnore != null)
    //                command.Parameters.Add(new SqlParameter("@bValidationCanIgnore", p_Column.ValidationCanIgnore));

    //            if (p_Column.DefaultRelatedTableID != null)
    //                command.Parameters.Add(new SqlParameter("@nDefaultRelatedTableID", p_Column.DefaultRelatedTableID));
    //            if (p_Column.DefaultUpdateValues != null)
    //                command.Parameters.Add(new SqlParameter("@bDefaultUpdateValues", p_Column.DefaultUpdateValues));

    //            if (p_Column.TrafficLightColumnID != null)
    //                command.Parameters.Add(new SqlParameter("@nTrafficLightColumnID", p_Column.TrafficLightColumnID));
    //            if (p_Column.TrafficLightValues != "")
    //                command.Parameters.Add(new SqlParameter("@sTrafficLightValues", p_Column.TrafficLightValues));


    //            if (p_Column.MapPopup != "")
    //                command.Parameters.Add(new SqlParameter("@sMapPopup", p_Column.MapPopup));

    //            if (p_Column.CompareColumnID != null)
    //                command.Parameters.Add(new SqlParameter("@nCompareColumnID", p_Column.CompareColumnID));
    //            if (p_Column.CompareOperator != "")
    //                command.Parameters.Add(new SqlParameter("@sCompareOperator", p_Column.CompareOperator));


    //            if (p_Column.MapPinHoverColumnID != null)
    //                command.Parameters.Add(new SqlParameter("@nMapPinHoverColumnID", p_Column.MapPinHoverColumnID));

    //            if (p_Column.FilterParentColumnID != null)
    //                command.Parameters.Add(new SqlParameter("@nFilterParentColumnID", p_Column.FilterParentColumnID));
    //            if (p_Column.FilterOtherColumnID != null)
    //                command.Parameters.Add(new SqlParameter("@nFilterOtherColumnID", p_Column.FilterOtherColumnID));
    //            if (p_Column.FilterValue != "")
    //                command.Parameters.Add(new SqlParameter("@sFilterValue", p_Column.FilterValue));



    //            if (p_Column.OnlyForAdmin != null)
    //                command.Parameters.Add(new SqlParameter("@bOnlyForAdmin", p_Column.OnlyForAdmin));

    //            if (p_Column.ShowViewLink != "")
    //                command.Parameters.Add(new SqlParameter("@sShowViewLink", p_Column.ShowViewLink));

    //            command.Parameters.Add(new SqlParameter("@nTableID", p_Column.TableID));
    //            command.Parameters.Add(new SqlParameter("@sSystemName", p_Column.SystemName));
    //            command.Parameters.Add(new SqlParameter("@sDisplayName", p_Column.DisplayName));
    //            //if (p_Column.DisplayOrder!=null)
    //            command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_Column.DisplayOrder));


    //            if (p_Column.ViewName != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sViewName", p_Column.ViewName));

    //            if (p_Column.DisplayTextSummary != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sDisplayTextSummary", p_Column.DisplayTextSummary));
    //            if (p_Column.DisplayTextDetail != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sDisplayTextDetail", p_Column.DisplayTextDetail));
    //            if (p_Column.NameOnImport != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sNameOnImport", p_Column.NameOnImport));

    //            if (p_Column.NameOnExport != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sNameOnExport", p_Column.NameOnExport));

    //            if (p_Column.GraphLabel != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sGraphLabel", p_Column.GraphLabel));


    //            if (p_Column.GraphTypeID != null)
    //                command.Parameters.Add(new SqlParameter("@nGraphTypeID", p_Column.GraphTypeID));

    //            if (p_Column.ValidationOnWarning != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sValidationOnWarning", p_Column.ValidationOnWarning));

    //            if (p_Column.ValidationOnEntry != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sValidationOnEntry", p_Column.ValidationOnEntry));

    //            if (p_Column.IsStandard != null)
    //                command.Parameters.Add(new SqlParameter("@bIsStandard", p_Column.IsStandard));

    //            if (p_Column.PositionOnImport != null)
    //                command.Parameters.Add(new SqlParameter("@nPositionOnImport", p_Column.PositionOnImport));


    //            if (p_Column.Constant != "")
    //                command.Parameters.Add(new SqlParameter("@sConstant", p_Column.Constant));

    //            if (p_Column.Calculation != "")
    //                command.Parameters.Add(new SqlParameter("@sCalculation", p_Column.Calculation));

    //            if (p_Column.ShowTotal != null)
    //                command.Parameters.Add(new SqlParameter("@bShowTotal ", p_Column.ShowTotal));

    //            if (p_Column.Notes != "")
    //                command.Parameters.Add(new SqlParameter("@sNotes ", p_Column.Notes));

    //            if (p_Column.DropdownValues != "")
    //                command.Parameters.Add(new SqlParameter("@sDropdownValues ", p_Column.DropdownValues));

    //            if (p_Column.IsMandatory != null)
    //                command.Parameters.Add(new SqlParameter("@bIsMandatory ", p_Column.IsMandatory));

    //            if (p_Column.Alignment != "")
    //                command.Parameters.Add(new SqlParameter("@sAlignment ", p_Column.Alignment));

    //            if (p_Column.DefaultValue != "")
    //                command.Parameters.Add(new SqlParameter("@sDefaultValue ", p_Column.DefaultValue));

    //            if (p_Column.TextWidth != null)
    //                command.Parameters.Add(new SqlParameter("@nTextWidth ", p_Column.TextWidth));
    //            if (p_Column.TextHeight != null)
    //                command.Parameters.Add(new SqlParameter("@nTextHeight ", p_Column.TextHeight));

    //            if (p_Column.TableTableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableTableID ", p_Column.TableTableID));

    //            if (p_Column.DisplayRight != null)
    //                command.Parameters.Add(new SqlParameter("@bDisplayRight ", p_Column.DisplayRight));

    //            if (p_Column.DropDownType != "")
    //                command.Parameters.Add(new SqlParameter("@sDropDownType ", p_Column.DropDownType));
    //            if (p_Column.DisplayColumn != "")
    //                command.Parameters.Add(new SqlParameter("@sDisplayColumn ", p_Column.DisplayColumn));

    //            command.Parameters.Add(new SqlParameter("@sColumnType ", p_Column.ColumnType));

    //            if (p_Column.ColumnType == "number"
    //               || p_Column.ColumnType == "calculation")
    //            {
                   
    //                    if (p_Column.IgnoreSymbols != null)
    //                        command.Parameters.Add(new SqlParameter("@bIgnoreSymbols ", p_Column.IgnoreSymbols));

                       

    //                    if (p_Column.RoundNumber != null)
    //                        command.Parameters.Add(new SqlParameter("@nRoundNumber ", p_Column.RoundNumber));
    //                    if (p_Column.CheckUnlikelyValue != null)
    //                        command.Parameters.Add(new SqlParameter("@bCheckUnlikelyValue ", p_Column.CheckUnlikelyValue));

                   
    //            }

    //            if (p_Column.IsRound != null)
    //                command.Parameters.Add(new SqlParameter("@bIsRound ", p_Column.IsRound));
    //            if (p_Column.ParentColumnID != null)
    //                command.Parameters.Add(new SqlParameter("@nParentColumnID", p_Column.ParentColumnID));

    //            if (p_Column.NumberType != null)
    //                command.Parameters.Add(new SqlParameter("@nNumberType ", p_Column.NumberType));

    //            if (p_Column.AvgColumnID != null)
    //                command.Parameters.Add(new SqlParameter("@nAvgColumnID ", p_Column.AvgColumnID));
    //            if (p_Column.AvgNumberOfRecords != null)
    //                command.Parameters.Add(new SqlParameter("@nAvgNumberOfRecords ", p_Column.AvgNumberOfRecords));

    //            if (p_Column.MobileName != "")
    //                command.Parameters.Add(new SqlParameter("@sMobileName ", p_Column.MobileName));

    //            if (p_Column.IsDateSingleColumn != null)
    //                command.Parameters.Add(new SqlParameter("@bIsDateSingleColumn ", p_Column.IsDateSingleColumn));

    //            if (p_Column.ShowGraphExceedance != null)
    //                command.Parameters.Add(new SqlParameter("@dShowGraphExceedance ", p_Column.ShowGraphExceedance));
    //            if (p_Column.ShowGraphWarning != null)
    //                command.Parameters.Add(new SqlParameter("@dShowGraphWarning ", p_Column.ShowGraphWarning));

    //            if (p_Column.FlatLineNumber != null)
    //                command.Parameters.Add(new SqlParameter("@nFlatLineNumber ", p_Column.FlatLineNumber));


    //            if (p_Column.MaxValueAt != null)
    //                command.Parameters.Add(new SqlParameter("@dMaxValueAt ", p_Column.MaxValueAt));

    //            if (p_Column.DefaultGraphDefinitionID != null)
    //                command.Parameters.Add(new SqlParameter("@nDefaultGraphDefinitionID ", p_Column.DefaultGraphDefinitionID));

    //            if (p_Column.SummaryCellBackColor != "" && p_Column.SummaryCellBackColor != null)
    //                command.Parameters.Add(new SqlParameter("@sSummaryCellBackColor ", p_Column.SummaryCellBackColor));



    //            if (p_Column.TextType != "")
    //                command.Parameters.Add(new SqlParameter("@sTextType ", p_Column.TextType));

    //            if (p_Column.RegEx != "")
    //                command.Parameters.Add(new SqlParameter("@sRegEx ", p_Column.RegEx));

    //            //if (p_Column.HideColumnID != null)
    //            //    command.Parameters.Add(new SqlParameter("@nHideColumnID ", p_Column.HideColumnID));
    //            //if (p_Column.HideColumnValue != "")
    //            //    command.Parameters.Add(new SqlParameter("@sHideColumnValue ", p_Column.HideColumnValue));

    //            if (p_Column.DateCalculationType != "")
    //                command.Parameters.Add(new SqlParameter("@sDateCalculationType ", p_Column.DateCalculationType));

    //            command.Parameters.Add(new SqlParameter("@nLinkedParentColumnID ", p_Column.LinkedParentColumnID));


    //            //if (p_Column.DataRetrieverID != null)
    //            //    command.Parameters.Add(new SqlParameter("@NDataRetrieverID ", p_Column.DataRetrieverID));

    //            if (p_Column.VerticalList != null)
    //                command.Parameters.Add(new SqlParameter("@bVerticalList ", p_Column.VerticalList));

    //            if (p_Column.SummarySearch != null)
    //                command.Parameters.Add(new SqlParameter("@bSummarySearch ", p_Column.SummarySearch));

    //            if (p_Column.QuickAddLink != null)
    //                command.Parameters.Add(new SqlParameter("@bQuickAddLink ", p_Column.QuickAddLink));


    //            if (p_Column.TableTabID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableTabID ", p_Column.TableTabID));

    //            if (p_Column.DefaultColumnID != null)
    //                command.Parameters.Add(new SqlParameter("@nDefaultColumnID ", p_Column.DefaultColumnID));


    //            if (p_Column.DefaultType != "")
    //                command.Parameters.Add(new SqlParameter("@sDefaultType ", p_Column.DefaultType));

    //            //if (p_Column.AllowCopy != null)
    //            //    command.Parameters.Add(new SqlParameter("@bAllowCopy ", p_Column.AllowCopy));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return int.Parse(pRV.Value.ToString());
    //        }
    //    }
    //}

    public static int ets_Column_Insert(Column p_Column)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("ets_Column_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                            
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);


                UpdateColumnForSysTableLink(ref p_Column);

                if (p_Column.ButtonInfo != null)
                    command.Parameters.Add(new SqlParameter("@sButtonInfo", p_Column.ButtonInfo));


                if (p_Column.ColourCells != null)
                    command.Parameters.Add(new SqlParameter("@bColourCells", p_Column.ColourCells));


                if (p_Column.ValidationOnExceedance != "")
                    command.Parameters.Add(new SqlParameter("@sValidationOnExceedance", p_Column.ValidationOnExceedance));

                if (p_Column.ImageOnSummary != null)
                    command.Parameters.Add(new SqlParameter("@bImageOnSummary", p_Column.ImageOnSummary));

                if (p_Column.ValidationCanIgnore != null)
                    command.Parameters.Add(new SqlParameter("@bValidationCanIgnore", p_Column.ValidationCanIgnore));

                if (p_Column.DefaultRelatedTableID != null)
                    command.Parameters.Add(new SqlParameter("@nDefaultRelatedTableID", p_Column.DefaultRelatedTableID));
                if (p_Column.DefaultUpdateValues != null)
                    command.Parameters.Add(new SqlParameter("@bDefaultUpdateValues", p_Column.DefaultUpdateValues));

                if (p_Column.TrafficLightColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nTrafficLightColumnID", p_Column.TrafficLightColumnID));
                if (p_Column.TrafficLightValues != "")
                    command.Parameters.Add(new SqlParameter("@sTrafficLightValues", p_Column.TrafficLightValues));


                if (p_Column.MapPopup != "")
                    command.Parameters.Add(new SqlParameter("@sMapPopup", p_Column.MapPopup));

                if (p_Column.CompareColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nCompareColumnID", p_Column.CompareColumnID));
                if (p_Column.CompareOperator != "")
                    command.Parameters.Add(new SqlParameter("@sCompareOperator", p_Column.CompareOperator));


                if (p_Column.MapPinHoverColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nMapPinHoverColumnID", p_Column.MapPinHoverColumnID));


                if (p_Column.FilterParentColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nFilterParentColumnID", p_Column.FilterParentColumnID));
                if (p_Column.FilterOtherColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nFilterOtherColumnID", p_Column.FilterOtherColumnID));
                if (p_Column.FilterValue != "")
                    command.Parameters.Add(new SqlParameter("@sFilterValue", p_Column.FilterValue));

                if (p_Column.FilterOperator != "")
                    command.Parameters.Add(new SqlParameter("@sFilterOperator", p_Column.FilterOperator));

                if (p_Column.DefaultColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nDefaultColumnID ", p_Column.DefaultColumnID));

                if (p_Column.ShowViewLink != "")
                    command.Parameters.Add(new SqlParameter("@sShowViewLink", p_Column.ShowViewLink));

                if (p_Column.DefaultType != "")
                    command.Parameters.Add(new SqlParameter("@sDefaultType ", p_Column.DefaultType));

                if (p_Column.OnlyForAdmin != null)
                    command.Parameters.Add(new SqlParameter("@bOnlyForAdmin", p_Column.OnlyForAdmin));

                //if (p_Column.HideColumnID != null)
                //    command.Parameters.Add(new SqlParameter("@nHideColumnID ", p_Column.HideColumnID));
                //if (p_Column.HideColumnValue != "")
                //    command.Parameters.Add(new SqlParameter("@sHideColumnValue ", p_Column.HideColumnValue));

                if (p_Column.ParentColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nParentColumnID", p_Column.ParentColumnID));

                command.Parameters.Add(new SqlParameter("@nTableID", p_Column.TableID));
                command.Parameters.Add(new SqlParameter("@sSystemName", p_Column.SystemName));
                command.Parameters.Add(new SqlParameter("@sDisplayName", p_Column.DisplayName));
                //if (p_Column.DisplayOrder!=null)
                command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_Column.DisplayOrder));

                if (p_Column.ViewName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sViewName", p_Column.ViewName));

                if (p_Column.DisplayTextSummary != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sDisplayTextSummary", p_Column.DisplayTextSummary));
                if (p_Column.DisplayTextDetail != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sDisplayTextDetail", p_Column.DisplayTextDetail));
                if (p_Column.NameOnImport != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sNameOnImport", p_Column.NameOnImport));

                if (p_Column.NameOnExport != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sNameOnExport", p_Column.NameOnExport));

                if (p_Column.GraphLabel != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sGraphLabel", p_Column.GraphLabel));


                if (p_Column.GraphTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nGraphTypeID", p_Column.GraphTypeID));

                if (p_Column.ValidationOnWarning != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValidationOnWarning", p_Column.ValidationOnWarning));

                if (p_Column.ValidationOnEntry != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValidationOnEntry", p_Column.ValidationOnEntry));

                if (p_Column.IsStandard != null)
                    command.Parameters.Add(new SqlParameter("@bIsStandard", p_Column.IsStandard));

                if (p_Column.PositionOnImport != null)
                    command.Parameters.Add(new SqlParameter("@nPositionOnImport", p_Column.PositionOnImport));


                if (p_Column.Constant != "")
                    command.Parameters.Add(new SqlParameter("@sConstant", p_Column.Constant));

                if (p_Column.Calculation != "")
                    command.Parameters.Add(new SqlParameter("@sCalculation", p_Column.Calculation));

                if (p_Column.ShowTotal != null)
                    command.Parameters.Add(new SqlParameter("@bShowTotal ", p_Column.ShowTotal));

                if (p_Column.Notes != "")
                    command.Parameters.Add(new SqlParameter("@sNotes ", p_Column.Notes));

                if (p_Column.DropdownValues != "")
                    command.Parameters.Add(new SqlParameter("@sDropdownValues ", p_Column.DropdownValues));

                if (p_Column.Importance != "")
                    command.Parameters.Add(new SqlParameter("@sImportance ", p_Column.Importance));

                if (p_Column.Alignment != "")
                    command.Parameters.Add(new SqlParameter("@sAlignment ", p_Column.Alignment));

                if (p_Column.DefaultValue != "")
                    command.Parameters.Add(new SqlParameter("@sDefaultValue ", p_Column.DefaultValue));


                if (p_Column.TextWidth != null)
                    command.Parameters.Add(new SqlParameter("@nTextWidth ", p_Column.TextWidth));
                if (p_Column.TextHeight != null)
                    command.Parameters.Add(new SqlParameter("@nTextHeight ", p_Column.TextHeight));

                if (p_Column.TableTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableTableID ", p_Column.TableTableID));

                if (p_Column.DisplayRight != null)
                    command.Parameters.Add(new SqlParameter("@bDisplayRight ", p_Column.DisplayRight));

                if (p_Column.DropDownType != "")
                    command.Parameters.Add(new SqlParameter("@sDropDownType ", p_Column.DropDownType));
                if (p_Column.DisplayColumn != "")
                    command.Parameters.Add(new SqlParameter("@sDisplayColumn ", p_Column.DisplayColumn));

                command.Parameters.Add(new SqlParameter("@sColumnType ", p_Column.ColumnType));

                if (p_Column.ColumnType == "number"
                      || p_Column.ColumnType == "calculation")
                {

                    if (p_Column.IgnoreSymbols != null)
                        command.Parameters.Add(new SqlParameter("@bIgnoreSymbols ", p_Column.IgnoreSymbols));


                    if (p_Column.RoundNumber != null)
                        command.Parameters.Add(new SqlParameter("@nRoundNumber ", p_Column.RoundNumber));
                    if (p_Column.CheckUnlikelyValue != null)
                        command.Parameters.Add(new SqlParameter("@bCheckUnlikelyValue ", p_Column.CheckUnlikelyValue));


                }
                if (p_Column.IsRound != null)
                    command.Parameters.Add(new SqlParameter("@bIsRound ", p_Column.IsRound));



                if (p_Column.NumberType != null)
                    command.Parameters.Add(new SqlParameter("@nNumberType ", p_Column.NumberType));

                if (p_Column.AvgColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nAvgColumnID ", p_Column.AvgColumnID));

                if (p_Column.AvgNumberOfRecords != null)
                    command.Parameters.Add(new SqlParameter("@nAvgNumberOfRecords ", p_Column.AvgNumberOfRecords));


                if (p_Column.MobileName != "")
                    command.Parameters.Add(new SqlParameter("@sMobileName ", p_Column.MobileName));

                if (p_Column.IsDateSingleColumn != null)
                    command.Parameters.Add(new SqlParameter("@bIsDateSingleColumn ", p_Column.IsDateSingleColumn));

                if (p_Column.ShowGraphExceedance != null)
                    command.Parameters.Add(new SqlParameter("@dShowGraphExceedance ", p_Column.ShowGraphExceedance));
                if (p_Column.ShowGraphWarning != null)
                    command.Parameters.Add(new SqlParameter("@dShowGraphWarning ", p_Column.ShowGraphWarning));

                if (p_Column.FlatLineNumber != null)
                    command.Parameters.Add(new SqlParameter("@nFlatLineNumber ", p_Column.FlatLineNumber));

                if (p_Column.MaxValueAt != null)
                    command.Parameters.Add(new SqlParameter("@dMaxValueAt ", p_Column.MaxValueAt));

                if (p_Column.DefaultGraphDefinitionID != null)
                    command.Parameters.Add(new SqlParameter("@nDefaultGraphDefinitionID ", p_Column.DefaultGraphDefinitionID));

                if (p_Column.SummaryCellBackColor != "" && p_Column.SummaryCellBackColor != null)
                    command.Parameters.Add(new SqlParameter("@sSummaryCellBackColor ", p_Column.SummaryCellBackColor));


                if (p_Column.TextType != "")
                    command.Parameters.Add(new SqlParameter("@sTextType ", p_Column.TextType));

                if (p_Column.RegEx != "")
                    command.Parameters.Add(new SqlParameter("@sRegEx ", p_Column.RegEx));



                if (p_Column.DateCalculationType != "")
                    command.Parameters.Add(new SqlParameter("@sDateCalculationType ", p_Column.DateCalculationType));

                command.Parameters.Add(new SqlParameter("@nLinkedParentColumnID ", p_Column.LinkedParentColumnID));

                //if (p_Column.DataRetrieverID != null)
                //    command.Parameters.Add(new SqlParameter("@NDataRetrieverID ", p_Column.DataRetrieverID));

                if (p_Column.VerticalList != null)
                    command.Parameters.Add(new SqlParameter("@bVerticalList ", p_Column.VerticalList));

                if (p_Column.SummarySearch != null)
                    command.Parameters.Add(new SqlParameter("@bSummarySearch ", p_Column.SummarySearch));

                if (p_Column.QuickAddLink != null)
                    command.Parameters.Add(new SqlParameter("@bQuickAddLink ", p_Column.QuickAddLink));



                if (p_Column.TableTabID != null)
                    command.Parameters.Add(new SqlParameter("@nTableTabID ", p_Column.TableTabID));

                //if (p_Column.AllowCopy != null)
                //    command.Parameters.Add(new SqlParameter("@bAllowCopy ", p_Column.AllowCopy));

                //if (p_Column.HideOperator != "")
                //    command.Parameters.Add(new SqlParameter("@sHideOperator ", p_Column.HideOperator));

                //connection.Open();
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
    public static int ets_Column_Update(Column p_Column)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Column_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                UpdateColumnForSysTableLink(ref p_Column);

                if (p_Column.ButtonInfo != null)
                    command.Parameters.Add(new SqlParameter("@sButtonInfo", p_Column.ButtonInfo));

                if (p_Column.ColourCells != null)
                    command.Parameters.Add(new SqlParameter("@bColourCells", p_Column.ColourCells));


                if (p_Column.ValidationOnExceedance != "")
                    command.Parameters.Add(new SqlParameter("@sValidationOnExceedance", p_Column.ValidationOnExceedance));

                if (p_Column.AllowCopy != null)
                    command.Parameters.Add(new SqlParameter("@bAllowCopy ", p_Column.AllowCopy));

                if (p_Column.ImageOnSummary != null)
                    command.Parameters.Add(new SqlParameter("@bImageOnSummary", p_Column.ImageOnSummary));

                if (p_Column.ValidationCanIgnore != null)
                    command.Parameters.Add(new SqlParameter("@bValidationCanIgnore", p_Column.ValidationCanIgnore));


                if (p_Column.DefaultRelatedTableID != null)
                    command.Parameters.Add(new SqlParameter("@nDefaultRelatedTableID", p_Column.DefaultRelatedTableID));
                if (p_Column.DefaultUpdateValues != null)
                    command.Parameters.Add(new SqlParameter("@bDefaultUpdateValues", p_Column.DefaultUpdateValues));

                if (p_Column.TrafficLightColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nTrafficLightColumnID", p_Column.TrafficLightColumnID));
                if (p_Column.TrafficLightValues != "")
                    command.Parameters.Add(new SqlParameter("@sTrafficLightValues", p_Column.TrafficLightValues));

                if (p_Column.MapPopup != "")
                    command.Parameters.Add(new SqlParameter("@sMapPopup", p_Column.MapPopup));

                if (p_Column.CompareColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nCompareColumnID", p_Column.CompareColumnID));
                if (p_Column.CompareOperator != "")
                    command.Parameters.Add(new SqlParameter("@sCompareOperator", p_Column.CompareOperator));


                if (p_Column.MapPinHoverColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nMapPinHoverColumnID", p_Column.MapPinHoverColumnID));


                if (p_Column.FilterParentColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nFilterParentColumnID", p_Column.FilterParentColumnID));
                if (p_Column.FilterOtherColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nFilterOtherColumnID", p_Column.FilterOtherColumnID));
                if (p_Column.FilterValue != "")
                    command.Parameters.Add(new SqlParameter("@sFilterValue", p_Column.FilterValue));

                if (p_Column.FilterOperator != "")
                    command.Parameters.Add(new SqlParameter("@sFilterOperator", p_Column.FilterOperator));

                if (p_Column.DefaultColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nDefaultColumnID ", p_Column.DefaultColumnID));

                if (p_Column.ShowViewLink != "")
                    command.Parameters.Add(new SqlParameter("@sShowViewLink", p_Column.ShowViewLink));

                if (p_Column.DefaultType != "")
                    command.Parameters.Add(new SqlParameter("@sDefaultType ", p_Column.DefaultType));

                if (p_Column.OnlyForAdmin != null)
                    command.Parameters.Add(new SqlParameter("@bOnlyForAdmin", p_Column.OnlyForAdmin));

                if (p_Column.DateCalculationType != "")
                    command.Parameters.Add(new SqlParameter("@sDateCalculationType ", p_Column.DateCalculationType));

                //if (p_Column.HideColumnID != null)
                //    command.Parameters.Add(new SqlParameter("@nHideColumnID ", p_Column.HideColumnID));
                //if (p_Column.HideColumnValue != "")
                //    command.Parameters.Add(new SqlParameter("@sHideColumnValue ", p_Column.HideColumnValue));


                if (p_Column.ViewName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sViewName", p_Column.ViewName));

                if (p_Column.TextType != "")
                    command.Parameters.Add(new SqlParameter("@sTextType ", p_Column.TextType));

                if (p_Column.RegEx != "")
                    command.Parameters.Add(new SqlParameter("@sRegEx ", p_Column.RegEx));

                //command.Parameters.Add(new SqlParameter("@bWarningChanged", bWarningChanged));
                if (p_Column.ParentColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nParentColumnID", p_Column.ParentColumnID));

                command.Parameters.Add(new SqlParameter("@nColumnID", p_Column.ColumnID));

                command.Parameters.Add(new SqlParameter("@nTableID", p_Column.TableID));
                command.Parameters.Add(new SqlParameter("@sSystemName", p_Column.SystemName));
                command.Parameters.Add(new SqlParameter("@sDisplayName", p_Column.DisplayName));

                command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_Column.DisplayOrder));


                //if (p_Column.SystemName == "LocationID")
                //{
                //    if (p_Column.DisplayTextDetail == string.Empty)
                //    {
                //        p_Column.DisplayTextDetail = "Location";
                //    }

                //}

                if (p_Column.DisplayTextSummary != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sDisplayTextSummary", p_Column.DisplayTextSummary));
                if (p_Column.DisplayTextDetail != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sDisplayTextDetail", p_Column.DisplayTextDetail));
                if (p_Column.NameOnImport != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sNameOnImport", p_Column.NameOnImport));

                if (p_Column.NameOnExport != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sNameOnExport", p_Column.NameOnExport));

                if (p_Column.GraphLabel != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sGraphLabel", p_Column.GraphLabel));

                if (p_Column.GraphTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nGraphTypeID", p_Column.GraphTypeID));

                if (p_Column.ValidationOnWarning != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValidationOnWarning", p_Column.ValidationOnWarning));

                if (p_Column.ValidationOnEntry != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValidationOnEntry", p_Column.ValidationOnEntry));


                if (p_Column.IsStandard != null)
                    command.Parameters.Add(new SqlParameter("@bIsStandard", p_Column.IsStandard));

                if (p_Column.PositionOnImport != null)
                    command.Parameters.Add(new SqlParameter("@nPositionOnImport", p_Column.PositionOnImport));



                if (p_Column.Constant != "")
                    command.Parameters.Add(new SqlParameter("@sConstant", p_Column.Constant));

                if (p_Column.Calculation != "")
                    command.Parameters.Add(new SqlParameter("@sCalculation", p_Column.Calculation));

                if (p_Column.ShowTotal != null)
                    command.Parameters.Add(new SqlParameter("@bShowTotal ", p_Column.ShowTotal));


                if (p_Column.LastUpdatedUserID != null)
                    command.Parameters.Add(new SqlParameter("@nLastUpdatedUserID ", p_Column.LastUpdatedUserID));



                if (p_Column.Notes != "")
                    command.Parameters.Add(new SqlParameter("@sNotes ", p_Column.Notes));


                if (p_Column.DropdownValues != "")
                    command.Parameters.Add(new SqlParameter("@sDropdownValues ", p_Column.DropdownValues));

                if (p_Column.Importance != "")
                    command.Parameters.Add(new SqlParameter("@sImportance ", p_Column.Importance));

                if (p_Column.Alignment != "")
                    command.Parameters.Add(new SqlParameter("@sAlignment ", p_Column.Alignment));

                if (p_Column.DefaultValue != "")
                    command.Parameters.Add(new SqlParameter("@sDefaultValue ", p_Column.DefaultValue));

                if (p_Column.TextWidth != null)
                    command.Parameters.Add(new SqlParameter("@nTextWidth ", p_Column.TextWidth));
                if (p_Column.TextHeight != null)
                    command.Parameters.Add(new SqlParameter("@nTextHeight ", p_Column.TextHeight));

                if (p_Column.TableTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableTableID ", p_Column.TableTableID));

                if (p_Column.DisplayRight != null)
                    command.Parameters.Add(new SqlParameter("@bDisplayRight ", p_Column.DisplayRight));

                if (p_Column.DropDownType != "")
                    command.Parameters.Add(new SqlParameter("@sDropDownType ", p_Column.DropDownType));
                if (p_Column.DisplayColumn != "")
                    command.Parameters.Add(new SqlParameter("@sDisplayColumn ", p_Column.DisplayColumn));

                command.Parameters.Add(new SqlParameter("@sColumnType ", p_Column.ColumnType));

                if (p_Column.ColumnType == "number"
                    || p_Column.ColumnType == "calculation")
                {

                    if (p_Column.IgnoreSymbols != null)
                        command.Parameters.Add(new SqlParameter("@bIgnoreSymbols ", p_Column.IgnoreSymbols));

                

                    if (p_Column.RoundNumber != null)
                        command.Parameters.Add(new SqlParameter("@nRoundNumber ", p_Column.RoundNumber));
                    if (p_Column.CheckUnlikelyValue != null)
                        command.Parameters.Add(new SqlParameter("@bCheckUnlikelyValue ", p_Column.CheckUnlikelyValue));


                }

                if (p_Column.IsRound != null)
                    command.Parameters.Add(new SqlParameter("@bIsRound ", p_Column.IsRound));

                if (p_Column.NumberType != null)
                    command.Parameters.Add(new SqlParameter("@nNumberType ", p_Column.NumberType));

                if (p_Column.AvgColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nAvgColumnID ", p_Column.AvgColumnID));

                if (p_Column.AvgNumberOfRecords != null)
                    command.Parameters.Add(new SqlParameter("@nAvgNumberOfRecords ", p_Column.AvgNumberOfRecords));



                if (p_Column.MobileName != "")
                    command.Parameters.Add(new SqlParameter("@sMobileName ", p_Column.MobileName));

                if (p_Column.IsDateSingleColumn != null)
                    command.Parameters.Add(new SqlParameter("@bIsDateSingleColumn ", p_Column.IsDateSingleColumn));

                if (p_Column.ShowGraphExceedance != null)
                    command.Parameters.Add(new SqlParameter("@dShowGraphExceedance ", p_Column.ShowGraphExceedance));
                if (p_Column.ShowGraphWarning != null)
                    command.Parameters.Add(new SqlParameter("@dShowGraphWarning ", p_Column.ShowGraphWarning));

                if (p_Column.FlatLineNumber != null)
                    command.Parameters.Add(new SqlParameter("@nFlatLineNumber ", p_Column.FlatLineNumber));

                if (p_Column.DefaultGraphDefinitionID != null)
                    command.Parameters.Add(new SqlParameter("@nDefaultGraphDefinitionID ", p_Column.DefaultGraphDefinitionID));

                if (p_Column.MaxValueAt != null)
                    command.Parameters.Add(new SqlParameter("@dMaxValueAt ", p_Column.MaxValueAt));

                if (p_Column.SummaryCellBackColor != "" && p_Column.SummaryCellBackColor != null)
                    command.Parameters.Add(new SqlParameter("@sSummaryCellBackColor ", p_Column.SummaryCellBackColor));


                if (p_Column.FormVerticalPosition != null)
                    command.Parameters.Add(new SqlParameter("@nFormVerticalPosition ", p_Column.FormVerticalPosition));

                if (p_Column.FormHorizontalPosition != null)
                    command.Parameters.Add(new SqlParameter("@nFormHorizontalPosition ", p_Column.FormHorizontalPosition));



                command.Parameters.Add(new SqlParameter("@nLinkedParentColumnID ", p_Column.LinkedParentColumnID));

                //if (p_Column.DataRetrieverID != null)
                //    command.Parameters.Add(new SqlParameter("@NDataRetrieverID ", p_Column.DataRetrieverID));

                if (p_Column.VerticalList != null)
                    command.Parameters.Add(new SqlParameter("@bVerticalList ", p_Column.VerticalList));

                if (p_Column.SummarySearch != null)
                    command.Parameters.Add(new SqlParameter("@bSummarySearch ", p_Column.SummarySearch));

                if (p_Column.QuickAddLink != null)
                    command.Parameters.Add(new SqlParameter("@bQuickAddLink ", p_Column.QuickAddLink));


                //if (p_Column.HideOperator != "")
                //    command.Parameters.Add(new SqlParameter("@sHideOperator ", p_Column.HideOperator));

                if (p_Column.CalculationIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bCalculationIsActive ", p_Column.CalculationIsActive));


                if (p_Column.TableTabID != null)
                    command.Parameters.Add(new SqlParameter("@nTableTabID ", p_Column.TableTabID));


               
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
    private static void UpdateColumnForSysTableLink(ref Column p_Column)
    {
        if (p_Column.TableTableID != null & p_Column.TableTableID == -1)
        {
            p_Column.LinkedParentColumnID = null;
            p_Column.ParentColumnID = null;
            p_Column.FilterParentColumnID = null;
            p_Column.FilterOtherColumnID = null;
            p_Column.FilterValue = null;
            p_Column.ShowViewLink = "";
            p_Column.QuickAddLink = null;
            p_Column.FilterOperator = "";
        }
    }

    //public static int ets_Column_Update(Column p_Column, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_Column_Update", connection, tn))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;
    //        command.Transaction = tn;
    //        //command.Parameters.Add(new SqlParameter("@bWarningChanged", bWarningChanged));



    //        UpdateColumnForSysTableLink(ref p_Column);


    //        if (p_Column.ColourCells != null)
    //            command.Parameters.Add(new SqlParameter("@bColourCells", p_Column.ColourCells));

    //        if (p_Column.ValidationOnExceedance != "")
    //            command.Parameters.Add(new SqlParameter("@sValidationOnExceedance", p_Column.ValidationOnExceedance));

    //        if (p_Column.AllowCopy != null)
    //            command.Parameters.Add(new SqlParameter("@bAllowCopy ", p_Column.AllowCopy));

    //        if (p_Column.ImageOnSummary != null)
    //            command.Parameters.Add(new SqlParameter("@bImageOnSummary", p_Column.ImageOnSummary));

    //        if (p_Column.ValidationCanIgnore != null)
    //            command.Parameters.Add(new SqlParameter("@bValidationCanIgnore", p_Column.ValidationCanIgnore));

    //        if (p_Column.DefaultRelatedTableID != null)
    //            command.Parameters.Add(new SqlParameter("@nDefaultRelatedTableID", p_Column.DefaultRelatedTableID));
    //        if (p_Column.DefaultUpdateValues != null)
    //            command.Parameters.Add(new SqlParameter("@bDefaultUpdateValues", p_Column.DefaultUpdateValues));


    //        if (p_Column.TrafficLightColumnID != null)
    //            command.Parameters.Add(new SqlParameter("@nTrafficLightColumnID", p_Column.TrafficLightColumnID));
    //        if (p_Column.TrafficLightValues != "")
    //            command.Parameters.Add(new SqlParameter("@sTrafficLightValues", p_Column.TrafficLightValues));

    //        if (p_Column.MapPopup != "")
    //            command.Parameters.Add(new SqlParameter("@sMapPopup", p_Column.MapPopup));


    //        if (p_Column.CompareColumnID != null)
    //            command.Parameters.Add(new SqlParameter("@nCompareColumnID", p_Column.CompareColumnID));
    //        if (p_Column.CompareOperator != "")
    //            command.Parameters.Add(new SqlParameter("@sCompareOperator", p_Column.CompareOperator));


    //        if (p_Column.MapPinHoverColumnID != null)
    //            command.Parameters.Add(new SqlParameter("@nMapPinHoverColumnID", p_Column.MapPinHoverColumnID));

    //        if (p_Column.FilterParentColumnID != null)
    //            command.Parameters.Add(new SqlParameter("@nFilterParentColumnID", p_Column.FilterParentColumnID));
    //        if (p_Column.FilterOtherColumnID != null)
    //            command.Parameters.Add(new SqlParameter("@nFilterOtherColumnID", p_Column.FilterOtherColumnID));
    //        if (p_Column.FilterValue != "")
    //            command.Parameters.Add(new SqlParameter("@sFilterValue", p_Column.FilterValue));


    //        if (p_Column.DefaultColumnID != null)
    //            command.Parameters.Add(new SqlParameter("@nDefaultColumnID ", p_Column.DefaultColumnID));

    //        if (p_Column.ShowViewLink != "")
    //            command.Parameters.Add(new SqlParameter("@sShowViewLink", p_Column.ShowViewLink));

    //        if (p_Column.DefaultType != "")
    //            command.Parameters.Add(new SqlParameter("@sDefaultType ", p_Column.DefaultType));


    //        if (p_Column.OnlyForAdmin != null)
    //            command.Parameters.Add(new SqlParameter("@bOnlyForAdmin", p_Column.OnlyForAdmin));

    //        if (p_Column.DateCalculationType != "")
    //            command.Parameters.Add(new SqlParameter("@sDateCalculationType ", p_Column.DateCalculationType));


    //        //if (p_Column.HideColumnID != null)
    //        //    command.Parameters.Add(new SqlParameter("@nHideColumnID ", p_Column.HideColumnID));
    //        //if (p_Column.HideColumnValue != "")
    //        //    command.Parameters.Add(new SqlParameter("@sHideColumnValue ", p_Column.HideColumnValue));


    //        if (p_Column.ViewName != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@sViewName", p_Column.ViewName));

    //        if (p_Column.TextType != "")
    //            command.Parameters.Add(new SqlParameter("@sTextType ", p_Column.TextType));

    //        if (p_Column.RegEx != "")
    //            command.Parameters.Add(new SqlParameter("@sRegEx ", p_Column.RegEx));

    //        if (p_Column.ParentColumnID != null)
    //            command.Parameters.Add(new SqlParameter("@nParentColumnID", p_Column.ParentColumnID));

    //        command.Parameters.Add(new SqlParameter("@nColumnID", p_Column.ColumnID));

    //        command.Parameters.Add(new SqlParameter("@nTableID", p_Column.TableID));
    //        command.Parameters.Add(new SqlParameter("@sSystemName", p_Column.SystemName));
    //        command.Parameters.Add(new SqlParameter("@sDisplayName", p_Column.DisplayName));

    //        command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_Column.DisplayOrder));


    //        //if (p_Column.SystemName == "LocationID")
    //        //{
    //        //    if (p_Column.DisplayTextDetail == string.Empty)
    //        //    {
    //        //        p_Column.DisplayTextDetail = "Location";
    //        //    }

    //        //}

    //        if (p_Column.DisplayTextSummary != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@sDisplayTextSummary", p_Column.DisplayTextSummary));
    //        if (p_Column.DisplayTextDetail != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@sDisplayTextDetail", p_Column.DisplayTextDetail));
    //        if (p_Column.NameOnImport != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@sNameOnImport", p_Column.NameOnImport));

    //        if (p_Column.NameOnExport != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@sNameOnExport", p_Column.NameOnExport));

    //        if (p_Column.GraphLabel != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@sGraphLabel", p_Column.GraphLabel));

    //        if (p_Column.GraphTypeID != null)
    //            command.Parameters.Add(new SqlParameter("@nGraphTypeID", p_Column.GraphTypeID));

    //        if (p_Column.ValidationOnWarning != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@sValidationOnWarning", p_Column.ValidationOnWarning));

    //        if (p_Column.ValidationOnEntry != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@sValidationOnEntry", p_Column.ValidationOnEntry));


    //        if (p_Column.IsStandard != null)
    //            command.Parameters.Add(new SqlParameter("@bIsStandard", p_Column.IsStandard));

    //        if (p_Column.PositionOnImport != null)
    //            command.Parameters.Add(new SqlParameter("@nPositionOnImport", p_Column.PositionOnImport));



    //        if (p_Column.Constant != "")
    //            command.Parameters.Add(new SqlParameter("@sConstant", p_Column.Constant));

    //        if (p_Column.Calculation != "")
    //            command.Parameters.Add(new SqlParameter("@sCalculation", p_Column.Calculation));

    //        if (p_Column.ShowTotal != null)
    //            command.Parameters.Add(new SqlParameter("@bShowTotal ", p_Column.ShowTotal));

    //        if (p_Column.LastUpdatedUserID != null)
    //            command.Parameters.Add(new SqlParameter("@nLastUpdatedUserID ", p_Column.LastUpdatedUserID));

    //        if (p_Column.DropdownValues != "")
    //            command.Parameters.Add(new SqlParameter("@sDropdownValues ", p_Column.DropdownValues));

    //        if (p_Column.IsMandatory != null)
    //            command.Parameters.Add(new SqlParameter("@bIsMandatory ", p_Column.IsMandatory));

    //        if (p_Column.Alignment != "")
    //            command.Parameters.Add(new SqlParameter("@sAlignment ", p_Column.Alignment));

    //        if (p_Column.DefaultValue != "")
    //            command.Parameters.Add(new SqlParameter("@sDefaultValue ", p_Column.DefaultValue));

    //        if (p_Column.Notes != "")
    //            command.Parameters.Add(new SqlParameter("@sNotes ", p_Column.Notes));


    //        if (p_Column.TextWidth != null)
    //            command.Parameters.Add(new SqlParameter("@nTextWidth ", p_Column.TextWidth));
    //        if (p_Column.TextHeight != null)
    //            command.Parameters.Add(new SqlParameter("@nTextHeight ", p_Column.TextHeight));

    //        if (p_Column.TableTableID != null)
    //            command.Parameters.Add(new SqlParameter("@nTableTableID ", p_Column.TableTableID));

    //        if (p_Column.DisplayRight != null)
    //            command.Parameters.Add(new SqlParameter("@bDisplayRight ", p_Column.DisplayRight));

    //        if (p_Column.DropDownType != "")
    //            command.Parameters.Add(new SqlParameter("@sDropDownType ", p_Column.DropDownType));
    //        if (p_Column.DisplayColumn != "")
    //            command.Parameters.Add(new SqlParameter("@sDisplayColumn ", p_Column.DisplayColumn));

    //        command.Parameters.Add(new SqlParameter("@sColumnType ", p_Column.ColumnType));

    //        if (p_Column.ColumnType == "number"
    //              || p_Column.ColumnType == "calculation")
    //        {

    //            if (p_Column.IgnoreSymbols != null)
    //                command.Parameters.Add(new SqlParameter("@bIgnoreSymbols ", p_Column.IgnoreSymbols));

                

    //            if (p_Column.RoundNumber != null)
    //                command.Parameters.Add(new SqlParameter("@nRoundNumber ", p_Column.RoundNumber));
    //            if (p_Column.CheckUnlikelyValue != null)
    //                command.Parameters.Add(new SqlParameter("@bCheckUnlikelyValue ", p_Column.CheckUnlikelyValue));


    //        }

    //        if (p_Column.IsRound != null)
    //            command.Parameters.Add(new SqlParameter("@bIsRound ", p_Column.IsRound));

    //        if (p_Column.NumberType != null)
    //            command.Parameters.Add(new SqlParameter("@nNumberType ", p_Column.NumberType));

    //        if (p_Column.AvgColumnID != null)
    //            command.Parameters.Add(new SqlParameter("@nAvgColumnID ", p_Column.AvgColumnID));

    //        if (p_Column.AvgNumberOfRecords != null)
    //            command.Parameters.Add(new SqlParameter("@nAvgNumberOfRecords ", p_Column.AvgNumberOfRecords));



    //        if (p_Column.MobileName != "")
    //            command.Parameters.Add(new SqlParameter("@sMobileName ", p_Column.MobileName));

    //        if (p_Column.IsDateSingleColumn != null)
    //            command.Parameters.Add(new SqlParameter("@bIsDateSingleColumn ", p_Column.IsDateSingleColumn));

    //        if (p_Column.ShowGraphExceedance != null)
    //            command.Parameters.Add(new SqlParameter("@dShowGraphExceedance ", p_Column.ShowGraphExceedance));
    //        if (p_Column.ShowGraphWarning != null)
    //            command.Parameters.Add(new SqlParameter("@dShowGraphWarning ", p_Column.ShowGraphWarning));

    //        if (p_Column.FlatLineNumber != null)
    //            command.Parameters.Add(new SqlParameter("@nFlatLineNumber ", p_Column.FlatLineNumber));

    //        if (p_Column.MaxValueAt != null)
    //            command.Parameters.Add(new SqlParameter("@dMaxValueAt ", p_Column.MaxValueAt));

    //        if (p_Column.DefaultGraphDefinitionID != null)
    //            command.Parameters.Add(new SqlParameter("@nDefaultGraphDefinitionID ", p_Column.DefaultGraphDefinitionID));

    //        if (p_Column.SummaryCellBackColor != "" && p_Column.SummaryCellBackColor != null)
    //            command.Parameters.Add(new SqlParameter("@sSummaryCellBackColor ", p_Column.SummaryCellBackColor));



    //        if (p_Column.FormVerticalPosition != null)
    //            command.Parameters.Add(new SqlParameter("@nFormVerticalPosition ", p_Column.FormVerticalPosition));

    //        if (p_Column.FormHorizontalPosition != null)
    //            command.Parameters.Add(new SqlParameter("@nFormHorizontalPosition ", p_Column.FormHorizontalPosition));


    //        command.Parameters.Add(new SqlParameter("@nLinkedParentColumnID ", p_Column.LinkedParentColumnID));

    //        //if (p_Column.DataRetrieverID != null)
    //        //    command.Parameters.Add(new SqlParameter("@NDataRetrieverID ", p_Column.DataRetrieverID));

    //        if (p_Column.VerticalList != null)
    //            command.Parameters.Add(new SqlParameter("@bVerticalList ", p_Column.VerticalList));

    //        if (p_Column.SummarySearch != null)
    //            command.Parameters.Add(new SqlParameter("@bSummarySearch ", p_Column.SummarySearch));

    //        if (p_Column.QuickAddLink != null)
    //            command.Parameters.Add(new SqlParameter("@bQuickAddLink ", p_Column.QuickAddLink));

    //        //if (p_Column.HideOperator != "")
    //        //    command.Parameters.Add(new SqlParameter("@sHideOperator ", p_Column.HideOperator));

    //        if (p_Column.CalculationIsActive != null)
    //            command.Parameters.Add(new SqlParameter("@bCalculationIsActive ", p_Column.CalculationIsActive));



    //        if (p_Column.TableTabID != null)
    //            command.Parameters.Add(new SqlParameter("@nTableTabID ", p_Column.TableTabID));

    //        //connection.Open();
    //        command.ExecuteNonQuery();

    //        return 1;

    //    }

    //}



    public static int ets_Column_Delete(int nColumnID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Column_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

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

    public static int ets_Column_OrderChange(int nColumnID, bool bMoveUp)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Column_OrderChange", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));
                command.Parameters.Add(new SqlParameter("@bMoveUp", bMoveUp));

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





    //public static int ets_Menu_OrderChange(int nMenuID, bool bMoveUp)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Menu_OrderChange", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nMenuID", nMenuID));
    //            command.Parameters.Add(new SqlParameter("@bMoveUp", bMoveUp));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}



    //public static Column ets_Column_Details(int nColumnID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Column_Details", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

    //            //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
    //            command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

    //            connection.Open();

    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    Column temp = new Column(
    //                        (int)reader["ColumnID"],
    //                        (int)reader["TableID"],
    //                        (string)reader["SystemName"],
    //                         (int)reader["DisplayOrder"],
    //                        reader["DisplayTextSummary"] == DBNull.Value ? string.Empty : (string)reader["DisplayTextSummary"],
    //                        reader["DisplayTextDetail"] == DBNull.Value ? string.Empty : (string)reader["DisplayTextDetail"],
    //                        reader["NameOnImport"] == DBNull.Value ? string.Empty : (string)reader["NameOnImport"],
    //                        reader["NameOnExport"] == DBNull.Value ? string.Empty : (string)reader["NameOnExport"],
    //                         reader["GraphTypeID"] == DBNull.Value ? null : (int?)reader["GraphTypeID"],
    //                         reader["ValidationOnWarning"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnWarning"],
    //                         reader["ValidationOnEntry"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnEntry"],
    //                        (DateTime)reader["DateAdded"],
    //                        (DateTime)reader["DateUpdated"], string.Empty,
    //                        reader["TableName"] == DBNull.Value ? string.Empty : (string)reader["TableName"],
    //                        (bool)reader["IsStandard"], (string)reader["DisplayName"],
    //                        reader["PositionOnImport"] == DBNull.Value ? null : (int?)reader["PositionOnImport"],
    //                        reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
    //                        reader["IsRound"] == DBNull.Value ? null : (bool?)reader["IsRound"],
    //                         reader["RoundNumber"] == DBNull.Value ? null : (int?)reader["RoundNumber"],
    //                          reader["CheckUnlikelyValue"] == DBNull.Value ? null : (bool?)reader["CheckUnlikelyValue"],
    //                          reader["GraphLabel"] == DBNull.Value ? string.Empty : (string)reader["GraphLabel"],
    //                          reader["DropdownValues"] == DBNull.Value ? string.Empty : (string)reader["DropdownValues"],
    //                           (bool)reader["IsMandatory"]
    //                         );


    //                    temp.Constant = reader["Constant"] == DBNull.Value ? string.Empty : (string)reader["Constant"];
    //                    temp.Calculation = reader["Calculation"] == DBNull.Value ? string.Empty : (string)reader["Calculation"];
    //                    temp.ShowTotal = reader["ShowTotal"] == DBNull.Value ? null : (bool?)reader["ShowTotal"];
    //                    temp.IgnoreSymbols = reader["IgnoreSymbols"] == DBNull.Value ? null : (bool?)reader["IgnoreSymbols"];

    //                    temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
    //                    temp.Alignment = reader["Alignment"] == DBNull.Value ? string.Empty : (string)reader["Alignment"];

                    
    //                    temp.NumberType = reader["NumberType"] == DBNull.Value ? null : (int?)reader["NumberType"];
    //                    temp.DefaultValue = reader["DefaultValue"] == DBNull.Value ? string.Empty : (string)reader["DefaultValue"];
    //                    temp.AvgColumnID = reader["AvgColumnID"] == DBNull.Value ? null : (int?)reader["AvgColumnID"];
    //                    temp.AvgNumberOfRecords = reader["AvgNumberOfRecords"] == DBNull.Value ? null : (int?)reader["AvgNumberOfRecords"];
    //                    temp.MobileName = reader["MobileName"] == DBNull.Value ? string.Empty : (string)reader["MobileName"];
    //                    temp.IsDateSingleColumn = reader["IsDateSingleColumn"] == DBNull.Value ? null : (bool?)reader["IsDateSingleColumn"];

    //                    temp.ShowGraphExceedance = reader["ShowGraphExceedance"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphExceedance"].ToString());
    //                    temp.ShowGraphWarning = reader["ShowGraphWarning"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphWarning"].ToString());

    //                    temp.FlatLineNumber = reader["FlatLineNumber"] == DBNull.Value ? null : (int?)reader["FlatLineNumber"];

    //                    temp.MaxValueAt = reader["MaxValueAt"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxValueAt"].ToString());

    //                    temp.DefaultGraphDefinitionID = reader["DefaultGraphDefinitionID"] == DBNull.Value ? null : (int?)int.Parse(reader["DefaultGraphDefinitionID"].ToString());

    //                    temp.TextWidth = reader["TextWidth"] == DBNull.Value ? null : (int?)reader["TextWidth"];
    //                    temp.TextHeight = reader["TextHeight"] == DBNull.Value ? null : (int?)reader["TextHeight"];
    //                    temp.ColumnType = reader["ColumnType"] == DBNull.Value ? "" : (string)reader["ColumnType"];
    //                    temp.DropDownType = reader["DropDownType"] == DBNull.Value ? null : (string)reader["DropDownType"];
    //                    temp.TableTableID = reader["TableTableID"] == DBNull.Value ? null : (int?)reader["TableTableID"];
    //                    temp.DisplayColumn = reader["DisplayColumn"] == DBNull.Value ? "" : (string)reader["DisplayColumn"];
    //                    temp.DisplayRight = reader["DisplayRight"] == DBNull.Value ? null : (bool?)reader["DisplayRight"];

    //                    temp.SummaryCellBackColor = reader["SummaryCellBackColor"] == DBNull.Value ? "" : (string)reader["SummaryCellBackColor"];

    //                    temp.ParentColumnID = reader["ParentColumnID"] == DBNull.Value ? null : (int?)reader["ParentColumnID"];

    //                    temp.TextType = reader["TextType"] == DBNull.Value ? "" : (string)reader["TextType"];
    //                    temp.RegEx = reader["RegEx"] == DBNull.Value ? "" : (string)reader["RegEx"];

    //                    //temp.HideColumnID = reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"];
    //                    //temp.HideColumnValue = reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"];
    //                    temp.DateCalculationType = reader["DateCalculationType"] == DBNull.Value ? "" : (string)reader["DateCalculationType"];
    //                    temp.OnlyForAdmin = reader["OnlyForAdmin"] == DBNull.Value ? null : (int?)reader["OnlyForAdmin"];
    //                    temp.IsSystemColumn = (bool)reader["IsSystemColumn"];

    //                    temp.LinkedParentColumnID = reader["LinkedParentColumnID"] == DBNull.Value ? null : (int?)reader["LinkedParentColumnID"];

    //                    //temp.DataRetrieverID = reader["DataRetrieverID"] == DBNull.Value ? null : (int?)reader["DataRetrieverID"];
    //                    temp.VerticalList = reader["VerticalList"] == DBNull.Value ? null : (bool?)reader["VerticalList"];

    //                    temp.SummarySearch = reader["SummarySearch"] == DBNull.Value ? null : (bool?)reader["SummarySearch"];
    //                    temp.QuickAddLink = reader["QuickAddLink"] == DBNull.Value ? null : (bool?)reader["QuickAddLink"];

    //                    //temp.HideOperator = reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"];

    //                    temp.CalculationIsActive = reader["CalculationIsActive"] == DBNull.Value ? null : (bool?)reader["CalculationIsActive"];
    //                    temp.TableTabID = reader["TableTabID"] == DBNull.Value ? null : (int?)reader["TableTabID"];

    //                    temp.ViewName = reader["ViewName"] == DBNull.Value ? string.Empty : (string)reader["ViewName"];
    //                    temp.SPDefaultValue = reader["SPDefaultValue"] == DBNull.Value ? string.Empty : (string)reader["SPDefaultValue"];

    //                    temp.DefaultColumnID = reader["DefaultColumnID"] == DBNull.Value ? null : (int?)reader["DefaultColumnID"];
    //                    temp.DefaultType = reader["DefaultType"] == DBNull.Value ? string.Empty : (string)reader["DefaultType"];

    //                    temp.ShowViewLink = reader["ShowViewLink"] == DBNull.Value ? "" : (string)reader["ShowViewLink"];

    //                    temp.FilterParentColumnID = reader["FilterParentColumnID"] == DBNull.Value ? null : (int?)reader["FilterParentColumnID"];
    //                    temp.FilterOtherColumnID = reader["FilterOtherColumnID"] == DBNull.Value ? null : (int?)reader["FilterOtherColumnID"];
    //                    temp.FilterValue = reader["FilterValue"] == DBNull.Value ? string.Empty : (string)reader["FilterValue"];

    //                    temp.MapPinHoverColumnID = reader["MapPinHoverColumnID"] == DBNull.Value ? null : (int?)reader["MapPinHoverColumnID"];

    //                    temp.CompareColumnID = reader["CompareColumnID"] == DBNull.Value ? null : (int?)reader["CompareColumnID"];
    //                    temp.CompareOperator = reader["CompareOperator"] == DBNull.Value ? string.Empty : (string)reader["CompareOperator"];


    //                    temp.MapPopup = reader["MapPopup"] == DBNull.Value ? string.Empty : (string)reader["MapPopup"];


    //                    temp.TrafficLightColumnID = reader["TrafficLightColumnID"] == DBNull.Value ? null : (int?)reader["TrafficLightColumnID"];
    //                    temp.TrafficLightValues = reader["TrafficLightValues"] == DBNull.Value ? string.Empty : (string)reader["TrafficLightValues"];

    //                    temp.DefaultRelatedTableID = reader["DefaultRelatedTableID"] == DBNull.Value ? null : (int?)reader["DefaultRelatedTableID"];
    //                    temp.DefaultUpdateValues = reader["DefaultUpdateValues"] == DBNull.Value ? null : (bool?)reader["DefaultUpdateValues"];

    //                    temp.ValidationCanIgnore = reader["ValidationCanIgnore"] == DBNull.Value ? null : (bool?)reader["ValidationCanIgnore"];
    //                    temp.ImageOnSummary = reader["ImageOnSummary"] == DBNull.Value ? null : (bool?)reader["ImageOnSummary"];
    //                    temp.AllowCopy = reader["AllowCopy"] == DBNull.Value ? null : (bool?)reader["AllowCopy"];
    //                    temp.ValidationOnExceedance = reader["ValidationOnExceedance"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnExceedance"];
    //                    temp.ColourCells = reader["ColourCells"] == DBNull.Value ? null : (bool?)reader["ColourCells"];

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



    public static Column ets_Column_Details_By_Sys(int iTableID,string  sSystemName)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Column_Details_By_Sys", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@iTableID", iTableID));
                command.Parameters.Add(new SqlParameter("@sSystemName", sSystemName));

                connection.Open();

                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Column temp = new Column(
                                (int)reader["ColumnID"],
                                (int)reader["TableID"],
                                (string)reader["SystemName"],
                                 (int)reader["DisplayOrder"],
                                reader["DisplayTextSummary"] == DBNull.Value ? string.Empty : (string)reader["DisplayTextSummary"],
                                reader["DisplayTextDetail"] == DBNull.Value ? string.Empty : (string)reader["DisplayTextDetail"],
                                reader["NameOnImport"] == DBNull.Value ? string.Empty : (string)reader["NameOnImport"],
                                reader["NameOnExport"] == DBNull.Value ? string.Empty : (string)reader["NameOnExport"],
                                 reader["GraphTypeID"] == DBNull.Value ? null : (int?)reader["GraphTypeID"],
                                 reader["ValidationOnWarning"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnWarning"],
                                 reader["ValidationOnEntry"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnEntry"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"], string.Empty,
                                reader["TableName"] == DBNull.Value ? string.Empty : (string)reader["TableName"],
                                (bool)reader["IsStandard"], (string)reader["DisplayName"],
                                reader["PositionOnImport"] == DBNull.Value ? null : (int?)reader["PositionOnImport"],
                                reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
                                reader["IsRound"] == DBNull.Value ? null : (bool?)reader["IsRound"],
                                 reader["RoundNumber"] == DBNull.Value ? null : (int?)reader["RoundNumber"],
                                  reader["CheckUnlikelyValue"] == DBNull.Value ? null : (bool?)reader["CheckUnlikelyValue"],
                                  reader["GraphLabel"] == DBNull.Value ? string.Empty : (string)reader["GraphLabel"],
                                  reader["DropdownValues"] == DBNull.Value ? string.Empty : (string)reader["DropdownValues"],
                                   reader["Importance"] == DBNull.Value ? string.Empty : (string)reader["Importance"]
                                 );


                            temp.Constant = reader["Constant"] == DBNull.Value ? string.Empty : (string)reader["Constant"];
                            temp.Calculation = reader["Calculation"] == DBNull.Value ? string.Empty : (string)reader["Calculation"];
                            temp.ShowTotal = reader["ShowTotal"] == DBNull.Value ? null : (bool?)reader["ShowTotal"];
                            temp.IgnoreSymbols = reader["IgnoreSymbols"] == DBNull.Value ? null : (bool?)reader["IgnoreSymbols"];

                            temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
                            temp.Alignment = reader["Alignment"] == DBNull.Value ? string.Empty : (string)reader["Alignment"];


                            temp.NumberType = reader["NumberType"] == DBNull.Value ? null : (int?)reader["NumberType"];
                            temp.DefaultValue = reader["DefaultValue"] == DBNull.Value ? string.Empty : (string)reader["DefaultValue"];
                            temp.AvgColumnID = reader["AvgColumnID"] == DBNull.Value ? null : (int?)reader["AvgColumnID"];
                            temp.AvgNumberOfRecords = reader["AvgNumberOfRecords"] == DBNull.Value ? null : (int?)reader["AvgNumberOfRecords"];
                            temp.MobileName = reader["MobileName"] == DBNull.Value ? string.Empty : (string)reader["MobileName"];
                            temp.IsDateSingleColumn = reader["IsDateSingleColumn"] == DBNull.Value ? null : (bool?)reader["IsDateSingleColumn"];

                            temp.ShowGraphExceedance = reader["ShowGraphExceedance"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphExceedance"].ToString());
                            temp.ShowGraphWarning = reader["ShowGraphWarning"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphWarning"].ToString());

                            temp.FlatLineNumber = reader["FlatLineNumber"] == DBNull.Value ? null : (int?)reader["FlatLineNumber"];

                            temp.MaxValueAt = reader["MaxValueAt"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxValueAt"].ToString());

                            temp.DefaultGraphDefinitionID = reader["DefaultGraphDefinitionID"] == DBNull.Value ? null : (int?)int.Parse(reader["DefaultGraphDefinitionID"].ToString());

                            temp.TextWidth = reader["TextWidth"] == DBNull.Value ? null : (int?)reader["TextWidth"];
                            temp.TextHeight = reader["TextHeight"] == DBNull.Value ? null : (int?)reader["TextHeight"];
                            temp.ColumnType = reader["ColumnType"] == DBNull.Value ? "" : (string)reader["ColumnType"];
                            temp.DropDownType = reader["DropDownType"] == DBNull.Value ? null : (string)reader["DropDownType"];
                            temp.TableTableID = reader["TableTableID"] == DBNull.Value ? null : (int?)reader["TableTableID"];
                            temp.DisplayColumn = reader["DisplayColumn"] == DBNull.Value ? "" : (string)reader["DisplayColumn"];
                            temp.DisplayRight = reader["DisplayRight"] == DBNull.Value ? null : (bool?)reader["DisplayRight"];

                            temp.SummaryCellBackColor = reader["SummaryCellBackColor"] == DBNull.Value ? "" : (string)reader["SummaryCellBackColor"];

                            temp.ParentColumnID = reader["ParentColumnID"] == DBNull.Value ? null : (int?)reader["ParentColumnID"];

                            temp.TextType = reader["TextType"] == DBNull.Value ? "" : (string)reader["TextType"];
                            temp.RegEx = reader["RegEx"] == DBNull.Value ? "" : (string)reader["RegEx"];

                            //temp.HideColumnID = reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"];
                            //temp.HideColumnValue = reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"];
                            temp.DateCalculationType = reader["DateCalculationType"] == DBNull.Value ? "" : (string)reader["DateCalculationType"];
                            temp.OnlyForAdmin = reader["OnlyForAdmin"] == DBNull.Value ? null : (int?)reader["OnlyForAdmin"];
                            temp.IsSystemColumn = (bool)reader["IsSystemColumn"];

                            temp.LinkedParentColumnID = reader["LinkedParentColumnID"] == DBNull.Value ? null : (int?)reader["LinkedParentColumnID"];
                            //temp.DataRetrieverID = reader["DataRetrieverID"] == DBNull.Value ? null : (int?)reader["DataRetrieverID"];
                            temp.VerticalList = reader["VerticalList"] == DBNull.Value ? null : (bool?)reader["VerticalList"];
                            temp.SummarySearch = reader["SummarySearch"] == DBNull.Value ? null : (bool?)reader["SummarySearch"];
                            temp.QuickAddLink = reader["QuickAddLink"] == DBNull.Value ? null : (bool?)reader["QuickAddLink"];
                            //temp.HideOperator = reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"];
                            temp.CalculationIsActive = reader["CalculationIsActive"] == DBNull.Value ? null : (bool?)reader["CalculationIsActive"];
                            temp.TableTabID = reader["TableTabID"] == DBNull.Value ? null : (int?)reader["TableTabID"];
                            temp.ViewName = reader["ViewName"] == DBNull.Value ? string.Empty : (string)reader["ViewName"];

                            temp.SPDefaultValue = reader["SPDefaultValue"] == DBNull.Value ? string.Empty : (string)reader["SPDefaultValue"];

                            temp.DefaultColumnID = reader["DefaultColumnID"] == DBNull.Value ? null : (int?)reader["DefaultColumnID"];
                            temp.DefaultType = reader["DefaultType"] == DBNull.Value ? string.Empty : (string)reader["DefaultType"];
                            temp.ShowViewLink = reader["ShowViewLink"] == DBNull.Value ? "" : (string)reader["ShowViewLink"];
                            temp.FilterParentColumnID = reader["FilterParentColumnID"] == DBNull.Value ? null : (int?)reader["FilterParentColumnID"];
                            temp.FilterOtherColumnID = reader["FilterOtherColumnID"] == DBNull.Value ? null : (int?)reader["FilterOtherColumnID"];
                            temp.FilterValue = reader["FilterValue"] == DBNull.Value ? string.Empty : (string)reader["FilterValue"];
                            temp.MapPinHoverColumnID = reader["MapPinHoverColumnID"] == DBNull.Value ? null : (int?)reader["MapPinHoverColumnID"];

                            temp.CompareColumnID = reader["CompareColumnID"] == DBNull.Value ? null : (int?)reader["CompareColumnID"];
                            temp.CompareOperator = reader["CompareOperator"] == DBNull.Value ? string.Empty : (string)reader["CompareOperator"];
                            temp.MapPopup = reader["MapPopup"] == DBNull.Value ? string.Empty : (string)reader["MapPopup"];
                            temp.TrafficLightColumnID = reader["TrafficLightColumnID"] == DBNull.Value ? null : (int?)reader["TrafficLightColumnID"];
                            temp.TrafficLightValues = reader["TrafficLightValues"] == DBNull.Value ? string.Empty : (string)reader["TrafficLightValues"];
                            temp.DefaultRelatedTableID = reader["DefaultRelatedTableID"] == DBNull.Value ? null : (int?)reader["DefaultRelatedTableID"];
                            temp.DefaultUpdateValues = reader["DefaultUpdateValues"] == DBNull.Value ? null : (bool?)reader["DefaultUpdateValues"];
                            temp.ValidationCanIgnore = reader["ValidationCanIgnore"] == DBNull.Value ? null : (bool?)reader["ValidationCanIgnore"];
                            temp.ImageOnSummary = reader["ImageOnSummary"] == DBNull.Value ? null : (bool?)reader["ImageOnSummary"];
                            temp.AllowCopy = reader["AllowCopy"] == DBNull.Value ? null : (bool?)reader["AllowCopy"];
                            temp.ValidationOnExceedance = reader["ValidationOnExceedance"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnExceedance"];
                            temp.ColourCells = reader["ColourCells"] == DBNull.Value ? null : (bool?)reader["ColourCells"];
                            temp.ButtonInfo = reader["ButtonInfo"] == DBNull.Value ? string.Empty : (string)reader["ButtonInfo"];
                            temp.FilterOperator = reader["FilterOperator"] == DBNull.Value ? string.Empty : (string)reader["FilterOperator"];

                            connection.Close();
                            connection.Dispose();

                            return temp;
                        }

                    }
                }
                catch
                {
                    //
                }



                connection.Close();
                connection.Dispose();
                return null;

            }
        }
    }

    public static Column ets_Column_Details(int nColumnID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_Column_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Column temp = new Column(
                                (int)reader["ColumnID"],
                                (int)reader["TableID"],
                                (string)reader["SystemName"],
                                 (int)reader["DisplayOrder"],
                                reader["DisplayTextSummary"] == DBNull.Value ? string.Empty : (string)reader["DisplayTextSummary"],
                                reader["DisplayTextDetail"] == DBNull.Value ? string.Empty : (string)reader["DisplayTextDetail"],
                                reader["NameOnImport"] == DBNull.Value ? string.Empty : (string)reader["NameOnImport"],
                                reader["NameOnExport"] == DBNull.Value ? string.Empty : (string)reader["NameOnExport"],
                                 reader["GraphTypeID"] == DBNull.Value ? null : (int?)reader["GraphTypeID"],
                                 reader["ValidationOnWarning"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnWarning"],
                                 reader["ValidationOnEntry"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnEntry"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"], string.Empty,
                                reader["TableName"] == DBNull.Value ? string.Empty : (string)reader["TableName"],
                                (bool)reader["IsStandard"], (string)reader["DisplayName"],
                                reader["PositionOnImport"] == DBNull.Value ? null : (int?)reader["PositionOnImport"],
                                 reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
                                reader["IsRound"] == DBNull.Value ? null : (bool?)reader["IsRound"],
                                 reader["RoundNumber"] == DBNull.Value ? null : (int?)reader["RoundNumber"],
                                  reader["CheckUnlikelyValue"] == DBNull.Value ? null : (bool?)reader["CheckUnlikelyValue"],
                                   reader["GraphLabel"] == DBNull.Value ? string.Empty : (string)reader["GraphLabel"],
                                   reader["DropdownValues"] == DBNull.Value ? string.Empty : (string)reader["DropdownValues"],
                                   reader["Importance"] == DBNull.Value ? string.Empty : (string)reader["Importance"]
                                 );


                            temp.Constant = reader["Constant"] == DBNull.Value ? string.Empty : (string)reader["Constant"];
                            temp.Calculation = reader["Calculation"] == DBNull.Value ? string.Empty : (string)reader["Calculation"];
                            temp.ShowTotal = reader["ShowTotal"] == DBNull.Value ? null : (bool?)reader["ShowTotal"];
                            temp.IgnoreSymbols = reader["IgnoreSymbols"] == DBNull.Value ? null : (bool?)reader["IgnoreSymbols"];

                            temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
                            temp.Alignment = reader["Alignment"] == DBNull.Value ? string.Empty : (string)reader["Alignment"];


                            temp.NumberType = reader["NumberType"] == DBNull.Value ? null : (int?)reader["NumberType"];
                            temp.DefaultValue = reader["DefaultValue"] == DBNull.Value ? string.Empty : (string)reader["DefaultValue"];

                            temp.AvgColumnID = reader["AvgColumnID"] == DBNull.Value ? null : (int?)reader["AvgColumnID"];
                            temp.AvgNumberOfRecords = reader["AvgNumberOfRecords"] == DBNull.Value ? null : (int?)reader["AvgNumberOfRecords"];

                            temp.MobileName = reader["MobileName"] == DBNull.Value ? string.Empty : (string)reader["MobileName"];
                            temp.IsDateSingleColumn = reader["IsDateSingleColumn"] == DBNull.Value ? null : (bool?)reader["IsDateSingleColumn"];
                            temp.ShowGraphExceedance = reader["ShowGraphExceedance"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphExceedance"].ToString());
                            temp.ShowGraphWarning = reader["ShowGraphWarning"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphWarning"].ToString());

                            temp.FlatLineNumber = reader["FlatLineNumber"] == DBNull.Value ? null : (int?)reader["FlatLineNumber"];

                            temp.MaxValueAt = reader["MaxValueAt"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxValueAt"].ToString());

                            temp.DefaultGraphDefinitionID = reader["DefaultGraphDefinitionID"] == DBNull.Value ? null : (int?)int.Parse(reader["DefaultGraphDefinitionID"].ToString());

                            temp.TextWidth = reader["TextWidth"] == DBNull.Value ? null : (int?)reader["TextWidth"];
                            temp.TextHeight = reader["TextHeight"] == DBNull.Value ? null : (int?)reader["TextHeight"];
                            temp.ColumnType = reader["ColumnType"] == DBNull.Value ? "" : (string)reader["ColumnType"];
                            temp.DropDownType = reader["DropDownType"] == DBNull.Value ? null : (string)reader["DropDownType"];
                            temp.TableTableID = reader["TableTableID"] == DBNull.Value ? null : (int?)reader["TableTableID"];
                            temp.DisplayColumn = reader["DisplayColumn"] == DBNull.Value ? "" : (string)reader["DisplayColumn"];
                            temp.DisplayRight = reader["DisplayRight"] == DBNull.Value ? null : (bool?)reader["DisplayRight"];

                            temp.SummaryCellBackColor = reader["SummaryCellBackColor"] == DBNull.Value ? "" : (string)reader["SummaryCellBackColor"];
                            temp.ParentColumnID = reader["ParentColumnID"] == DBNull.Value ? null : (int?)reader["ParentColumnID"];

                            temp.TextType = reader["TextType"] == DBNull.Value ? "" : (string)reader["TextType"];
                            temp.RegEx = reader["RegEx"] == DBNull.Value ? "" : (string)reader["RegEx"];

                            //temp.HideColumnID = reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"];
                            //temp.HideColumnValue = reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"];
                            temp.DateCalculationType = reader["DateCalculationType"] == DBNull.Value ? "" : (string)reader["DateCalculationType"];
                            temp.OnlyForAdmin = reader["OnlyForAdmin"] == DBNull.Value ? null : (int?)reader["OnlyForAdmin"];
                            temp.IsSystemColumn = (bool)reader["IsSystemColumn"];
                            temp.LinkedParentColumnID = reader["LinkedParentColumnID"] == DBNull.Value ? null : (int?)reader["LinkedParentColumnID"];
                            //temp.DataRetrieverID = reader["DataRetrieverID"] == DBNull.Value ? null : (int?)reader["DataRetrieverID"];
                            temp.VerticalList = reader["VerticalList"] == DBNull.Value ? null : (bool?)reader["VerticalList"];
                            temp.SummarySearch = reader["SummarySearch"] == DBNull.Value ? null : (bool?)reader["SummarySearch"];
                            temp.QuickAddLink = reader["QuickAddLink"] == DBNull.Value ? null : (bool?)reader["QuickAddLink"];
                            //temp.HideOperator = reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"];
                            temp.CalculationIsActive = reader["CalculationIsActive"] == DBNull.Value ? null : (bool?)reader["CalculationIsActive"];
                            temp.TableTabID = reader["TableTabID"] == DBNull.Value ? null : (int?)reader["TableTabID"];
                            temp.ViewName = reader["ViewName"] == DBNull.Value ? string.Empty : (string)reader["ViewName"];
                            temp.SPDefaultValue = reader["SPDefaultValue"] == DBNull.Value ? string.Empty : (string)reader["SPDefaultValue"];

                            temp.DefaultColumnID = reader["DefaultColumnID"] == DBNull.Value ? null : (int?)reader["DefaultColumnID"];
                            temp.DefaultType = reader["DefaultType"] == DBNull.Value ? string.Empty : (string)reader["DefaultType"];
                            temp.ShowViewLink = reader["ShowViewLink"] == DBNull.Value ? "" : (string)reader["ShowViewLink"];
                            temp.FilterParentColumnID = reader["FilterParentColumnID"] == DBNull.Value ? null : (int?)reader["FilterParentColumnID"];
                            temp.FilterOtherColumnID = reader["FilterOtherColumnID"] == DBNull.Value ? null : (int?)reader["FilterOtherColumnID"];
                            temp.FilterValue = reader["FilterValue"] == DBNull.Value ? string.Empty : (string)reader["FilterValue"];
                            temp.MapPinHoverColumnID = reader["MapPinHoverColumnID"] == DBNull.Value ? null : (int?)reader["MapPinHoverColumnID"];

                            temp.CompareColumnID = reader["CompareColumnID"] == DBNull.Value ? null : (int?)reader["CompareColumnID"];
                            temp.CompareOperator = reader["CompareOperator"] == DBNull.Value ? string.Empty : (string)reader["CompareOperator"];

                            temp.MapPopup = reader["MapPopup"] == DBNull.Value ? string.Empty : (string)reader["MapPopup"];
                            temp.TrafficLightColumnID = reader["TrafficLightColumnID"] == DBNull.Value ? null : (int?)reader["TrafficLightColumnID"];
                            temp.TrafficLightValues = reader["TrafficLightValues"] == DBNull.Value ? string.Empty : (string)reader["TrafficLightValues"];
                            temp.DefaultRelatedTableID = reader["DefaultRelatedTableID"] == DBNull.Value ? null : (int?)reader["DefaultRelatedTableID"];
                            temp.DefaultUpdateValues = reader["DefaultUpdateValues"] == DBNull.Value ? null : (bool?)reader["DefaultUpdateValues"];
                            temp.ValidationCanIgnore = reader["ValidationCanIgnore"] == DBNull.Value ? null : (bool?)reader["ValidationCanIgnore"];
                            temp.ImageOnSummary = reader["ImageOnSummary"] == DBNull.Value ? null : (bool?)reader["ImageOnSummary"];
                            temp.AllowCopy = reader["AllowCopy"] == DBNull.Value ? null : (bool?)reader["AllowCopy"];
                            temp.ValidationOnExceedance = reader["ValidationOnExceedance"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnExceedance"];
                            temp.ColourCells = reader["ColourCells"] == DBNull.Value ? null : (bool?)reader["ColourCells"];
                            temp.ButtonInfo = reader["ButtonInfo"] == DBNull.Value ? string.Empty : (string)reader["ButtonInfo"];
                            temp.FilterOperator = reader["FilterOperator"] == DBNull.Value ? string.Empty : (string)reader["FilterOperator"];
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




    public static Column ets_Column_Details_Position(int nTableID, int nPositionOnImport)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Column_Details_Position", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                command.Parameters.Add(new SqlParameter("@nPositionOnImport", nPositionOnImport));

                connection.Open();


                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Column temp = new Column(
                                (int)reader["ColumnID"],
                                (int)reader["TableID"],
                                (string)reader["SystemName"],
                                 (int)reader["DisplayOrder"],
                                reader["DisplayTextSummary"] == DBNull.Value ? string.Empty : (string)reader["DisplayTextSummary"],
                                reader["DisplayTextDetail"] == DBNull.Value ? string.Empty : (string)reader["DisplayTextDetail"],
                                reader["NameOnImport"] == DBNull.Value ? string.Empty : (string)reader["NameOnImport"],
                                reader["NameOnExport"] == DBNull.Value ? string.Empty : (string)reader["NameOnExport"],
                                 reader["GraphTypeID"] == DBNull.Value ? null : (int?)reader["GraphTypeID"],
                                 reader["ValidationOnEntry"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnEntry"],
                                 reader["ValidationOnEntry"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnEntry"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"], string.Empty,
                                reader["TableName"] == DBNull.Value ? string.Empty : (string)reader["TableName"],
                                (bool)reader["IsStandard"], (string)reader["DisplayName"],
                                reader["PositionOnImport"] == DBNull.Value ? null : (int?)reader["PositionOnImport"],
                                  reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
                                reader["IsRound"] == DBNull.Value ? null : (bool?)reader["IsRound"],
                                 reader["RoundNumber"] == DBNull.Value ? null : (int?)reader["RoundNumber"],
                                  reader["CheckUnlikelyValue"] == DBNull.Value ? null : (bool?)reader["CheckUnlikelyValue"],
                                   reader["GraphLabel"] == DBNull.Value ? string.Empty : (string)reader["GraphLabel"],
                                   reader["DropdownValues"] == DBNull.Value ? string.Empty : (string)reader["DropdownValues"],
                                   reader["Importance"] == DBNull.Value ? string.Empty : (string)reader["Importance"]
                                 );

                            temp.Constant = reader["Constant"] == DBNull.Value ? string.Empty : (string)reader["Constant"];
                            temp.Calculation = reader["Calculation"] == DBNull.Value ? string.Empty : (string)reader["Calculation"];
                            temp.ShowTotal = reader["ShowTotal"] == DBNull.Value ? null : (bool?)reader["ShowTotal"];
                            temp.IgnoreSymbols = reader["IgnoreSymbols"] == DBNull.Value ? null : (bool?)reader["IgnoreSymbols"];
                            temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];
                            temp.Alignment = reader["Alignment"] == DBNull.Value ? string.Empty : (string)reader["Alignment"];


                            temp.NumberType = reader["NumberType"] == DBNull.Value ? null : (int?)reader["NumberType"];
                            temp.DefaultValue = reader["DefaultValue"] == DBNull.Value ? string.Empty : (string)reader["DefaultValue"];
                            temp.AvgColumnID = reader["AvgColumnID"] == DBNull.Value ? null : (int?)reader["AvgColumnID"];
                            temp.AvgNumberOfRecords = reader["AvgNumberOfRecords"] == DBNull.Value ? null : (int?)reader["AvgNumberOfRecords"];
                            temp.MobileName = reader["MobileName"] == DBNull.Value ? string.Empty : (string)reader["MobileName"];
                            temp.IsDateSingleColumn = reader["IsDateSingleColumn"] == DBNull.Value ? null : (bool?)reader["IsDateSingleColumn"];
                            temp.ShowGraphExceedance = reader["ShowGraphExceedance"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphExceedance"].ToString());
                            temp.ShowGraphWarning = reader["ShowGraphWarning"] == DBNull.Value ? null : (double?)double.Parse(reader["ShowGraphWarning"].ToString());

                            temp.FlatLineNumber = reader["FlatLineNumber"] == DBNull.Value ? null : (int?)reader["FlatLineNumber"];

                            temp.MaxValueAt = reader["MaxValueAt"] == DBNull.Value ? null : (double?)double.Parse(reader["MaxValueAt"].ToString());

                            temp.DefaultGraphDefinitionID = reader["DefaultGraphDefinitionID"] == DBNull.Value ? null : (int?)int.Parse(reader["DefaultGraphDefinitionID"].ToString());

                            temp.TextWidth = reader["TextWidth"] == DBNull.Value ? null : (int?)reader["TextWidth"];
                            temp.TextHeight = reader["TextHeight"] == DBNull.Value ? null : (int?)reader["TextHeight"];
                            temp.ColumnType = reader["ColumnType"] == DBNull.Value ? "" : (string)reader["ColumnType"];
                            temp.DropDownType = reader["DropDownType"] == DBNull.Value ? null : (string)reader["DropDownType"];
                            temp.TableTableID = reader["TableTableID"] == DBNull.Value ? null : (int?)reader["TableTableID"];
                            temp.DisplayColumn = reader["DisplayColumn"] == DBNull.Value ? "" : (string)reader["DisplayColumn"];
                            temp.DisplayRight = reader["DisplayRight"] == DBNull.Value ? null : (bool?)reader["DisplayRight"];


                            temp.SummaryCellBackColor = reader["SummaryCellBackColor"] == DBNull.Value ? "" : (string)reader["SummaryCellBackColor"];

                            temp.ParentColumnID = reader["ParentColumnID"] == DBNull.Value ? null : (int?)reader["ParentColumnID"];

                            temp.TextType = reader["TextType"] == DBNull.Value ? "" : (string)reader["TextType"];
                            temp.RegEx = reader["RegEx"] == DBNull.Value ? "" : (string)reader["RegEx"];

                            //temp.HideColumnID = reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"];
                            //temp.HideColumnValue = reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"];
                            temp.DateCalculationType = reader["DateCalculationType"] == DBNull.Value ? "" : (string)reader["DateCalculationType"];
                            temp.OnlyForAdmin = reader["OnlyForAdmin"] == DBNull.Value ? null : (int?)reader["OnlyForAdmin"];
                            temp.IsSystemColumn = (bool)reader["IsSystemColumn"];
                            temp.LinkedParentColumnID = reader["LinkedParentColumnID"] == DBNull.Value ? null : (int?)reader["LinkedParentColumnID"];
                            //temp.DataRetrieverID = reader["DataRetrieverID"] == DBNull.Value ? null : (int?)reader["DataRetrieverID"];
                            temp.VerticalList = reader["VerticalList"] == DBNull.Value ? null : (bool?)reader["VerticalList"];
                            temp.SummarySearch = reader["SummarySearch"] == DBNull.Value ? null : (bool?)reader["SummarySearch"];
                            temp.QuickAddLink = reader["QuickAddLink"] == DBNull.Value ? null : (bool?)reader["QuickAddLink"];
                            //temp.HideOperator = reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"];
                            temp.CalculationIsActive = reader["CalculationIsActive"] == DBNull.Value ? null : (bool?)reader["CalculationIsActive"];
                            temp.TableTabID = reader["TableTabID"] == DBNull.Value ? null : (int?)reader["TableTabID"];
                            temp.ViewName = reader["ViewName"] == DBNull.Value ? string.Empty : (string)reader["ViewName"];
                            temp.SPDefaultValue = reader["SPDefaultValue"] == DBNull.Value ? string.Empty : (string)reader["SPDefaultValue"];

                            temp.DefaultColumnID = reader["DefaultColumnID"] == DBNull.Value ? null : (int?)reader["DefaultColumnID"];
                            temp.DefaultType = reader["DefaultType"] == DBNull.Value ? string.Empty : (string)reader["DefaultType"];
                            temp.ShowViewLink = reader["ShowViewLink"] == DBNull.Value ? "" : (string)reader["ShowViewLink"];
                            temp.FilterParentColumnID = reader["FilterParentColumnID"] == DBNull.Value ? null : (int?)reader["FilterParentColumnID"];
                            temp.FilterOtherColumnID = reader["FilterOtherColumnID"] == DBNull.Value ? null : (int?)reader["FilterOtherColumnID"];
                            temp.FilterValue = reader["FilterValue"] == DBNull.Value ? string.Empty : (string)reader["FilterValue"];
                            temp.MapPinHoverColumnID = reader["MapPinHoverColumnID"] == DBNull.Value ? null : (int?)reader["MapPinHoverColumnID"];

                            temp.CompareColumnID = reader["CompareColumnID"] == DBNull.Value ? null : (int?)reader["CompareColumnID"];
                            temp.CompareOperator = reader["CompareOperator"] == DBNull.Value ? string.Empty : (string)reader["CompareOperator"];
                            temp.MapPopup = reader["MapPopup"] == DBNull.Value ? string.Empty : (string)reader["MapPopup"];

                            temp.TrafficLightColumnID = reader["TrafficLightColumnID"] == DBNull.Value ? null : (int?)reader["TrafficLightColumnID"];
                            temp.TrafficLightValues = reader["TrafficLightValues"] == DBNull.Value ? string.Empty : (string)reader["TrafficLightValues"];
                            temp.DefaultRelatedTableID = reader["DefaultRelatedTableID"] == DBNull.Value ? null : (int?)reader["DefaultRelatedTableID"];
                            temp.DefaultUpdateValues = reader["DefaultUpdateValues"] == DBNull.Value ? null : (bool?)reader["DefaultUpdateValues"];
                            temp.ValidationCanIgnore = reader["ValidationCanIgnore"] == DBNull.Value ? null : (bool?)reader["ValidationCanIgnore"];
                            temp.ImageOnSummary = reader["ImageOnSummary"] == DBNull.Value ? null : (bool?)reader["ImageOnSummary"];
                            temp.AllowCopy = reader["AllowCopy"] == DBNull.Value ? null : (bool?)reader["AllowCopy"];

                            temp.ValidationOnExceedance = reader["ValidationOnExceedance"] == DBNull.Value ? string.Empty : (string)reader["ValidationOnExceedance"];
                            temp.ColourCells = reader["ColourCells"] == DBNull.Value ? null : (bool?)reader["ColourCells"];
                            temp.ButtonInfo = reader["ButtonInfo"] == DBNull.Value ? string.Empty : (string)reader["ButtonInfo"];
                            temp.FilterOperator = reader["FilterOperator"] == DBNull.Value ? string.Empty : (string)reader["FilterOperator"];
                            connection.Close();
                            connection.Dispose();
                            return temp;
                        }

                    }
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();
                return null;

            }
        }
    }


    #endregion

    #region Record


    public static int ets_Record_Count(int nTableID,DateTime dtStart,DateTime dEnd)
    {
        int iCount = 0;


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_Count", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                command.Parameters.Add(new SqlParameter("@dtStart", dtStart));
                command.Parameters.Add(new SqlParameter("@dEnd", dEnd));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            iCount = reader[0] == DBNull.Value ? 0 : (int)reader[0];


                            connection.Close();
                            connection.Dispose();

                            return iCount;
                        }

                    }
                }
                catch
                {
                  
                }
                connection.Close();
                connection.Dispose();
                
                return iCount;

            }
        }



       

    }


   

    public static int ets_Record_Insert(Record p_Record)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                //if(p_Record.LocationID!=null)
                //command.Parameters.Add(new SqlParameter("@LocationID", p_Record.LocationID));

                command.Parameters.Add(new SqlParameter("@TableID", p_Record.TableID));

                if (p_Record.DateTimeRecorded == null)
                    p_Record.DateTimeRecorded = DateTime.Now;

                command.Parameters.Add(new SqlParameter("@DateTimeRecorded", p_Record.DateTimeRecorded));
                if (p_Record.Notes != string.Empty)
                    command.Parameters.Add(new SqlParameter("@Notes", p_Record.Notes));

                if (p_Record.EnteredBy != null)
                    command.Parameters.Add(new SqlParameter("@EnteredBy", p_Record.EnteredBy));

                if (p_Record.IsActive == null)
                    p_Record.IsActive = true;
                command.Parameters.Add(new SqlParameter("@IsActive", p_Record.IsActive));



                if (p_Record.ValidationResults != string.Empty)
                    command.Parameters.Add(new SqlParameter("@ValidationResults", p_Record.ValidationResults));

                if (p_Record.WarningResults != string.Empty)
                    command.Parameters.Add(new SqlParameter("@WarningResults", p_Record.WarningResults));


                if (p_Record.V001 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V001", p_Record.V001));
                if (p_Record.V002 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V002", p_Record.V002));
                if (p_Record.V003 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V003", p_Record.V003));
                if (p_Record.V004 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V004", p_Record.V004));
                if (p_Record.V005 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V005", p_Record.V005));
                if (p_Record.V006 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V006", p_Record.V006));
                if (p_Record.V007 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V007", p_Record.V007));
                if (p_Record.V008 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V008", p_Record.V008));
                if (p_Record.V009 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V009", p_Record.V009));
                if (p_Record.V010 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V010", p_Record.V010));
                if (p_Record.V011 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V011", p_Record.V011));
                if (p_Record.V012 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V012", p_Record.V012));
                if (p_Record.V013 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V013", p_Record.V013));
                if (p_Record.V014 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V014", p_Record.V014));
                if (p_Record.V015 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V015", p_Record.V015));
                if (p_Record.V016 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V016", p_Record.V016));
                if (p_Record.V017 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V017", p_Record.V017));
                if (p_Record.V018 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V018", p_Record.V018));
                if (p_Record.V019 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V019", p_Record.V019));
                if (p_Record.V020 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V020", p_Record.V020));
                if (p_Record.V021 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V021", p_Record.V021));
                if (p_Record.V022 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V022", p_Record.V022));
                if (p_Record.V023 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V023", p_Record.V023));
                if (p_Record.V024 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V024", p_Record.V024));
                if (p_Record.V025 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V025", p_Record.V025));
                if (p_Record.V026 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V026", p_Record.V026));
                if (p_Record.V027 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V027", p_Record.V027));
                if (p_Record.V028 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V028", p_Record.V028));
                if (p_Record.V029 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V029", p_Record.V029));
                if (p_Record.V030 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V030", p_Record.V030));
                if (p_Record.V031 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V031", p_Record.V031));
                if (p_Record.V032 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V032", p_Record.V032));
                if (p_Record.V033 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V033", p_Record.V033));
                if (p_Record.V034 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V034", p_Record.V034));
                if (p_Record.V035 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V035", p_Record.V035));
                if (p_Record.V036 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V036", p_Record.V036));
                if (p_Record.V037 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V037", p_Record.V037));
                if (p_Record.V038 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V038", p_Record.V038));
                if (p_Record.V039 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V039", p_Record.V039));
                if (p_Record.V040 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V040", p_Record.V040));
                if (p_Record.V041 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V041", p_Record.V041));
                if (p_Record.V042 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V042", p_Record.V042));
                if (p_Record.V043 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V043", p_Record.V043));
                if (p_Record.V044 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V044", p_Record.V044));
                if (p_Record.V045 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V045", p_Record.V045));
                if (p_Record.V046 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V046", p_Record.V046));
                if (p_Record.V047 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V047", p_Record.V047));
                if (p_Record.V048 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V048", p_Record.V048));
                if (p_Record.V049 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V049", p_Record.V049));
                if (p_Record.V050 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V050", p_Record.V050));



                if (p_Record.V051 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V051", p_Record.V051));
                if (p_Record.V052 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V052", p_Record.V052));
                if (p_Record.V053 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V053", p_Record.V053));
                if (p_Record.V054 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V054", p_Record.V054));
                if (p_Record.V055 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V055", p_Record.V055));
                if (p_Record.V056 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V056", p_Record.V056));
                if (p_Record.V057 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V057", p_Record.V057));
                if (p_Record.V058 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V058", p_Record.V058));
                if (p_Record.V059 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V059", p_Record.V059));
                if (p_Record.V060 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V060", p_Record.V060));
                if (p_Record.V061 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V061", p_Record.V061));
                if (p_Record.V062 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V062", p_Record.V062));
                if (p_Record.V063 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V063", p_Record.V063));
                if (p_Record.V064 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V064", p_Record.V064));
                if (p_Record.V065 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V065", p_Record.V065));
                if (p_Record.V066 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V066", p_Record.V066));
                if (p_Record.V067 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V067", p_Record.V067));
                if (p_Record.V068 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V068", p_Record.V068));
                if (p_Record.V069 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V069", p_Record.V069));
                if (p_Record.V070 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V070", p_Record.V070));
                if (p_Record.V071 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V071", p_Record.V071));
                if (p_Record.V072 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V072", p_Record.V072));
                if (p_Record.V073 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V073", p_Record.V073));
                if (p_Record.V074 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V074", p_Record.V074));
                if (p_Record.V075 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V075", p_Record.V075));
                if (p_Record.V076 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V076", p_Record.V076));
                if (p_Record.V077 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V077", p_Record.V077));
                if (p_Record.V078 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V078", p_Record.V078));
                if (p_Record.V079 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V079", p_Record.V079));
                if (p_Record.V080 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V080", p_Record.V080));
                if (p_Record.V081 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V081", p_Record.V081));
                if (p_Record.V082 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V082", p_Record.V082));
                if (p_Record.V083 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V083", p_Record.V083));
                if (p_Record.V084 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V084", p_Record.V084));
                if (p_Record.V085 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V085", p_Record.V085));
                if (p_Record.V086 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V086", p_Record.V086));
                if (p_Record.V087 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V087", p_Record.V087));
                if (p_Record.V088 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V088", p_Record.V088));
                if (p_Record.V089 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V089", p_Record.V089));
                if (p_Record.V090 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V090", p_Record.V090));
                if (p_Record.V091 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V091", p_Record.V091));
                if (p_Record.V092 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V092", p_Record.V092));
                if (p_Record.V093 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V093", p_Record.V093));
                if (p_Record.V094 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V094", p_Record.V094));
                if (p_Record.V095 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V095", p_Record.V095));
                if (p_Record.V096 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V096", p_Record.V096));
                if (p_Record.V097 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V097", p_Record.V097));
                if (p_Record.V098 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V098", p_Record.V098));
                if (p_Record.V099 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V099", p_Record.V099));
                if (p_Record.V100 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V100", p_Record.V100));


                if (p_Record.V101 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V101", p_Record.V101));
                if (p_Record.V102 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V102", p_Record.V102));
                if (p_Record.V103 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V103", p_Record.V103));
                if (p_Record.V104 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V104", p_Record.V104));
                if (p_Record.V105 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V105", p_Record.V105));
                if (p_Record.V106 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V106", p_Record.V106));
                if (p_Record.V107 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V107", p_Record.V107));
                if (p_Record.V108 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V108", p_Record.V108));
                if (p_Record.V109 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V109", p_Record.V109));
                if (p_Record.V110 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V110", p_Record.V110));
                if (p_Record.V111 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V111", p_Record.V111));
                if (p_Record.V112 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V112", p_Record.V112));
                if (p_Record.V113 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V113", p_Record.V113));
                if (p_Record.V114 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V114", p_Record.V114));
                if (p_Record.V115 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V115", p_Record.V115));
                if (p_Record.V116 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V116", p_Record.V116));
                if (p_Record.V117 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V117", p_Record.V117));
                if (p_Record.V118 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V118", p_Record.V118));
                if (p_Record.V119 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V119", p_Record.V119));
                if (p_Record.V120 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V120", p_Record.V120));
                if (p_Record.V121 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V121", p_Record.V121));
                if (p_Record.V122 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V122", p_Record.V122));
                if (p_Record.V123 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V123", p_Record.V123));
                if (p_Record.V124 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V124", p_Record.V124));
                if (p_Record.V125 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V125", p_Record.V125));
                if (p_Record.V126 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V126", p_Record.V126));
                if (p_Record.V127 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V127", p_Record.V127));
                if (p_Record.V128 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V128", p_Record.V128));
                if (p_Record.V129 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V129", p_Record.V129));
                if (p_Record.V130 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V130", p_Record.V130));
                if (p_Record.V131 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V131", p_Record.V131));
                if (p_Record.V132 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V132", p_Record.V132));
                if (p_Record.V133 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V133", p_Record.V133));
                if (p_Record.V134 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V134", p_Record.V134));
                if (p_Record.V135 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V135", p_Record.V135));
                if (p_Record.V136 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V136", p_Record.V136));
                if (p_Record.V137 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V137", p_Record.V137));
                if (p_Record.V138 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V138", p_Record.V138));
                if (p_Record.V139 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V139", p_Record.V139));
                if (p_Record.V140 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V140", p_Record.V140));
                if (p_Record.V141 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V141", p_Record.V141));
                if (p_Record.V142 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V142", p_Record.V142));
                if (p_Record.V143 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V143", p_Record.V143));
                if (p_Record.V144 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V144", p_Record.V144));
                if (p_Record.V145 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V145", p_Record.V145));
                if (p_Record.V146 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V146", p_Record.V146));
                if (p_Record.V147 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V147", p_Record.V147));
                if (p_Record.V148 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V148", p_Record.V148));
                if (p_Record.V149 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V149", p_Record.V149));
                if (p_Record.V150 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V150", p_Record.V150));



                if (p_Record.V151 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V151", p_Record.V151));
                if (p_Record.V152 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V152", p_Record.V152));
                if (p_Record.V153 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V153", p_Record.V153));
                if (p_Record.V154 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V154", p_Record.V154));
                if (p_Record.V155 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V155", p_Record.V155));
                if (p_Record.V156 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V156", p_Record.V156));
                if (p_Record.V157 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V157", p_Record.V157));
                if (p_Record.V158 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V158", p_Record.V158));
                if (p_Record.V159 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V159", p_Record.V159));
                if (p_Record.V160 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V160", p_Record.V160));
                if (p_Record.V161 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V161", p_Record.V161));
                if (p_Record.V162 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V162", p_Record.V162));
                if (p_Record.V163 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V163", p_Record.V163));
                if (p_Record.V164 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V164", p_Record.V164));
                if (p_Record.V165 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V165", p_Record.V165));
                if (p_Record.V166 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V166", p_Record.V166));
                if (p_Record.V167 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V167", p_Record.V167));
                if (p_Record.V168 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V168", p_Record.V168));
                if (p_Record.V169 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V169", p_Record.V169));
                if (p_Record.V170 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V170", p_Record.V170));
                if (p_Record.V171 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V171", p_Record.V171));
                if (p_Record.V172 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V172", p_Record.V172));
                if (p_Record.V173 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V173", p_Record.V173));
                if (p_Record.V174 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V174", p_Record.V174));
                if (p_Record.V175 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V175", p_Record.V175));
                if (p_Record.V176 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V176", p_Record.V176));
                if (p_Record.V177 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V177", p_Record.V177));
                if (p_Record.V178 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V178", p_Record.V178));
                if (p_Record.V179 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V179", p_Record.V179));
                if (p_Record.V180 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V180", p_Record.V180));
                if (p_Record.V181 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V181", p_Record.V181));
                if (p_Record.V182 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V182", p_Record.V182));
                if (p_Record.V183 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V183", p_Record.V183));
                if (p_Record.V184 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V184", p_Record.V184));
                if (p_Record.V185 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V185", p_Record.V185));
                if (p_Record.V186 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V186", p_Record.V186));
                if (p_Record.V187 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V187", p_Record.V187));
                if (p_Record.V188 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V188", p_Record.V188));
                if (p_Record.V189 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V189", p_Record.V189));
                if (p_Record.V190 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V190", p_Record.V190));
                if (p_Record.V191 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V191", p_Record.V191));
                if (p_Record.V192 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V192", p_Record.V192));
                if (p_Record.V193 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V193", p_Record.V193));
                if (p_Record.V194 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V194", p_Record.V194));
                if (p_Record.V195 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V195", p_Record.V195));
                if (p_Record.V196 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V196", p_Record.V196));
                if (p_Record.V197 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V197", p_Record.V197));
                if (p_Record.V198 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V198", p_Record.V198));
                if (p_Record.V199 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V199", p_Record.V199));
                if (p_Record.V200 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V200", p_Record.V200));


                if (p_Record.V201 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V201", p_Record.V201));
                if (p_Record.V202 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V202", p_Record.V202));
                if (p_Record.V203 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V203", p_Record.V203));
                if (p_Record.V204 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V204", p_Record.V204));
                if (p_Record.V205 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V205", p_Record.V205));
                if (p_Record.V206 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V206", p_Record.V206));
                if (p_Record.V207 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V207", p_Record.V207));
                if (p_Record.V208 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V208", p_Record.V208));
                if (p_Record.V209 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V209", p_Record.V209));
                if (p_Record.V210 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V210", p_Record.V210));
                if (p_Record.V211 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V211", p_Record.V211));
                if (p_Record.V212 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V212", p_Record.V212));
                if (p_Record.V213 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V213", p_Record.V213));
                if (p_Record.V214 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V214", p_Record.V214));
                if (p_Record.V215 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V215", p_Record.V215));
                if (p_Record.V216 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V216", p_Record.V216));
                if (p_Record.V217 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V217", p_Record.V217));
                if (p_Record.V218 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V218", p_Record.V218));
                if (p_Record.V219 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V219", p_Record.V219));
                if (p_Record.V220 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V220", p_Record.V220));
                if (p_Record.V221 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V221", p_Record.V221));
                if (p_Record.V222 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V222", p_Record.V222));
                if (p_Record.V223 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V223", p_Record.V223));
                if (p_Record.V224 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V224", p_Record.V224));
                if (p_Record.V225 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V225", p_Record.V225));
                if (p_Record.V226 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V226", p_Record.V226));
                if (p_Record.V227 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V227", p_Record.V227));
                if (p_Record.V228 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V228", p_Record.V228));
                if (p_Record.V229 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V229", p_Record.V229));
                if (p_Record.V230 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V230", p_Record.V230));
                if (p_Record.V231 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V231", p_Record.V231));
                if (p_Record.V232 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V232", p_Record.V232));
                if (p_Record.V233 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V233", p_Record.V233));
                if (p_Record.V234 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V234", p_Record.V234));
                if (p_Record.V235 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V235", p_Record.V235));
                if (p_Record.V236 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V236", p_Record.V236));
                if (p_Record.V237 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V237", p_Record.V237));
                if (p_Record.V238 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V238", p_Record.V238));
                if (p_Record.V239 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V239", p_Record.V239));
                if (p_Record.V240 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V240", p_Record.V240));
                if (p_Record.V241 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V241", p_Record.V241));
                if (p_Record.V242 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V242", p_Record.V242));
                if (p_Record.V243 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V243", p_Record.V243));
                if (p_Record.V244 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V244", p_Record.V244));
                if (p_Record.V245 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V245", p_Record.V245));
                if (p_Record.V246 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V246", p_Record.V246));
                if (p_Record.V247 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V247", p_Record.V247));
                if (p_Record.V248 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V248", p_Record.V248));
                if (p_Record.V249 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V249", p_Record.V249));
                if (p_Record.V250 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V250", p_Record.V250));



                if (p_Record.V251 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V251", p_Record.V251));
                if (p_Record.V252 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V252", p_Record.V252));
                if (p_Record.V253 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V253", p_Record.V253));
                if (p_Record.V254 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V254", p_Record.V254));
                if (p_Record.V255 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V255", p_Record.V255));
                if (p_Record.V256 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V256", p_Record.V256));
                if (p_Record.V257 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V257", p_Record.V257));
                if (p_Record.V258 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V258", p_Record.V258));
                if (p_Record.V259 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V259", p_Record.V259));
                if (p_Record.V260 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V260", p_Record.V260));
                if (p_Record.V261 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V261", p_Record.V261));
                if (p_Record.V262 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V262", p_Record.V262));
                if (p_Record.V263 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V263", p_Record.V263));
                if (p_Record.V264 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V264", p_Record.V264));
                if (p_Record.V265 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V265", p_Record.V265));
                if (p_Record.V266 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V266", p_Record.V266));
                if (p_Record.V267 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V267", p_Record.V267));
                if (p_Record.V268 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V268", p_Record.V268));
                if (p_Record.V269 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V269", p_Record.V269));
                if (p_Record.V270 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V270", p_Record.V270));
                if (p_Record.V271 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V271", p_Record.V271));
                if (p_Record.V272 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V272", p_Record.V272));
                if (p_Record.V273 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V273", p_Record.V273));
                if (p_Record.V274 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V274", p_Record.V274));
                if (p_Record.V275 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V275", p_Record.V275));
                if (p_Record.V276 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V276", p_Record.V276));
                if (p_Record.V277 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V277", p_Record.V277));
                if (p_Record.V278 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V278", p_Record.V278));
                if (p_Record.V279 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V279", p_Record.V279));
                if (p_Record.V280 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V280", p_Record.V280));
                if (p_Record.V281 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V281", p_Record.V281));
                if (p_Record.V282 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V282", p_Record.V282));
                if (p_Record.V283 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V283", p_Record.V283));
                if (p_Record.V284 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V284", p_Record.V284));
                if (p_Record.V285 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V285", p_Record.V285));
                if (p_Record.V286 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V286", p_Record.V286));
                if (p_Record.V287 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V287", p_Record.V287));
                if (p_Record.V288 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V288", p_Record.V288));
                if (p_Record.V289 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V289", p_Record.V289));
                if (p_Record.V290 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V290", p_Record.V290));
                if (p_Record.V291 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V291", p_Record.V291));
                if (p_Record.V292 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V292", p_Record.V292));
                if (p_Record.V293 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V293", p_Record.V293));
                if (p_Record.V294 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V294", p_Record.V294));
                if (p_Record.V295 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V295", p_Record.V295));
                if (p_Record.V296 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V296", p_Record.V296));
                if (p_Record.V297 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V297", p_Record.V297));
                if (p_Record.V298 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V298", p_Record.V298));
                if (p_Record.V299 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V299", p_Record.V299));
                if (p_Record.V300 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V300", p_Record.V300));


                if (p_Record.V301 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V301", p_Record.V301));
                if (p_Record.V302 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V302", p_Record.V302));
                if (p_Record.V303 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V303", p_Record.V303));
                if (p_Record.V304 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V304", p_Record.V304));
                if (p_Record.V305 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V305", p_Record.V305));
                if (p_Record.V306 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V306", p_Record.V306));
                if (p_Record.V307 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V307", p_Record.V307));
                if (p_Record.V308 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V308", p_Record.V308));
                if (p_Record.V309 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V309", p_Record.V309));
                if (p_Record.V310 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V310", p_Record.V310));
                if (p_Record.V311 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V311", p_Record.V311));
                if (p_Record.V312 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V312", p_Record.V312));
                if (p_Record.V313 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V313", p_Record.V313));
                if (p_Record.V314 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V314", p_Record.V314));
                if (p_Record.V315 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V315", p_Record.V315));
                if (p_Record.V316 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V316", p_Record.V316));
                if (p_Record.V317 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V317", p_Record.V317));
                if (p_Record.V318 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V318", p_Record.V318));
                if (p_Record.V319 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V319", p_Record.V319));
                if (p_Record.V320 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V320", p_Record.V320));
                if (p_Record.V321 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V321", p_Record.V321));
                if (p_Record.V322 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V322", p_Record.V322));
                if (p_Record.V323 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V323", p_Record.V323));
                if (p_Record.V324 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V324", p_Record.V324));
                if (p_Record.V325 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V325", p_Record.V325));
                if (p_Record.V326 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V326", p_Record.V326));
                if (p_Record.V327 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V327", p_Record.V327));
                if (p_Record.V328 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V328", p_Record.V328));
                if (p_Record.V329 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V329", p_Record.V329));
                if (p_Record.V330 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V330", p_Record.V330));
                if (p_Record.V331 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V331", p_Record.V331));
                if (p_Record.V332 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V332", p_Record.V332));
                if (p_Record.V333 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V333", p_Record.V333));
                if (p_Record.V334 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V334", p_Record.V334));
                if (p_Record.V335 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V335", p_Record.V335));
                if (p_Record.V336 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V336", p_Record.V336));
                if (p_Record.V337 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V337", p_Record.V337));
                if (p_Record.V338 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V338", p_Record.V338));
                if (p_Record.V339 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V339", p_Record.V339));
                if (p_Record.V340 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V340", p_Record.V340));
                if (p_Record.V341 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V341", p_Record.V341));
                if (p_Record.V342 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V342", p_Record.V342));
                if (p_Record.V343 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V343", p_Record.V343));
                if (p_Record.V344 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V344", p_Record.V344));
                if (p_Record.V345 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V345", p_Record.V345));
                if (p_Record.V346 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V346", p_Record.V346));
                if (p_Record.V347 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V347", p_Record.V347));
                if (p_Record.V348 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V348", p_Record.V348));
                if (p_Record.V349 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V349", p_Record.V349));
                if (p_Record.V350 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V350", p_Record.V350));



                if (p_Record.V351 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V351", p_Record.V351));
                if (p_Record.V352 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V352", p_Record.V352));
                if (p_Record.V353 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V353", p_Record.V353));
                if (p_Record.V354 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V354", p_Record.V354));
                if (p_Record.V355 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V355", p_Record.V355));
                if (p_Record.V356 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V356", p_Record.V356));
                if (p_Record.V357 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V357", p_Record.V357));
                if (p_Record.V358 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V358", p_Record.V358));
                if (p_Record.V359 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V359", p_Record.V359));
                if (p_Record.V360 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V360", p_Record.V360));
                if (p_Record.V361 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V361", p_Record.V361));
                if (p_Record.V362 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V362", p_Record.V362));
                if (p_Record.V363 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V363", p_Record.V363));
                if (p_Record.V364 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V364", p_Record.V364));
                if (p_Record.V365 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V365", p_Record.V365));
                if (p_Record.V366 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V366", p_Record.V366));
                if (p_Record.V367 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V367", p_Record.V367));
                if (p_Record.V368 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V368", p_Record.V368));
                if (p_Record.V369 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V369", p_Record.V369));
                if (p_Record.V370 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V370", p_Record.V370));
                if (p_Record.V371 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V371", p_Record.V371));
                if (p_Record.V372 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V372", p_Record.V372));
                if (p_Record.V373 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V373", p_Record.V373));
                if (p_Record.V374 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V374", p_Record.V374));
                if (p_Record.V375 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V375", p_Record.V375));
                if (p_Record.V376 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V376", p_Record.V376));
                if (p_Record.V377 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V377", p_Record.V377));
                if (p_Record.V378 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V378", p_Record.V378));
                if (p_Record.V379 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V379", p_Record.V379));
                if (p_Record.V380 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V380", p_Record.V380));
                if (p_Record.V381 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V381", p_Record.V381));
                if (p_Record.V382 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V382", p_Record.V382));
                if (p_Record.V383 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V383", p_Record.V383));
                if (p_Record.V384 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V384", p_Record.V384));
                if (p_Record.V385 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V385", p_Record.V385));
                if (p_Record.V386 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V386", p_Record.V386));
                if (p_Record.V387 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V387", p_Record.V387));
                if (p_Record.V388 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V388", p_Record.V388));
                if (p_Record.V389 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V389", p_Record.V389));
                if (p_Record.V390 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V390", p_Record.V390));
                if (p_Record.V391 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V391", p_Record.V391));
                if (p_Record.V392 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V392", p_Record.V392));
                if (p_Record.V393 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V393", p_Record.V393));
                if (p_Record.V394 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V394", p_Record.V394));
                if (p_Record.V395 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V395", p_Record.V395));
                if (p_Record.V396 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V396", p_Record.V396));
                if (p_Record.V397 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V397", p_Record.V397));
                if (p_Record.V398 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V398", p_Record.V398));
                if (p_Record.V399 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V399", p_Record.V399));
                if (p_Record.V400 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V400", p_Record.V400));


                if (p_Record.V401 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V401", p_Record.V401));
                if (p_Record.V402 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V402", p_Record.V402));
                if (p_Record.V403 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V403", p_Record.V403));
                if (p_Record.V404 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V404", p_Record.V404));
                if (p_Record.V405 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V405", p_Record.V405));
                if (p_Record.V406 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V406", p_Record.V406));
                if (p_Record.V407 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V407", p_Record.V407));
                if (p_Record.V408 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V408", p_Record.V408));
                if (p_Record.V409 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V409", p_Record.V409));
                if (p_Record.V410 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V410", p_Record.V410));
                if (p_Record.V411 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V411", p_Record.V411));
                if (p_Record.V412 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V412", p_Record.V412));
                if (p_Record.V413 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V413", p_Record.V413));
                if (p_Record.V414 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V414", p_Record.V414));
                if (p_Record.V415 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V415", p_Record.V415));
                if (p_Record.V416 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V416", p_Record.V416));
                if (p_Record.V417 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V417", p_Record.V417));
                if (p_Record.V418 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V418", p_Record.V418));
                if (p_Record.V419 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V419", p_Record.V419));
                if (p_Record.V420 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V420", p_Record.V420));
                if (p_Record.V421 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V421", p_Record.V421));
                if (p_Record.V422 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V422", p_Record.V422));
                if (p_Record.V423 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V423", p_Record.V423));
                if (p_Record.V424 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V424", p_Record.V424));
                if (p_Record.V425 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V425", p_Record.V425));
                if (p_Record.V426 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V426", p_Record.V426));
                if (p_Record.V427 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V427", p_Record.V427));
                if (p_Record.V428 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V428", p_Record.V428));
                if (p_Record.V429 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V429", p_Record.V429));
                if (p_Record.V430 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V430", p_Record.V430));
                if (p_Record.V431 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V431", p_Record.V431));
                if (p_Record.V432 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V432", p_Record.V432));
                if (p_Record.V433 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V433", p_Record.V433));
                if (p_Record.V434 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V434", p_Record.V434));
                if (p_Record.V435 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V435", p_Record.V435));
                if (p_Record.V436 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V436", p_Record.V436));
                if (p_Record.V437 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V437", p_Record.V437));
                if (p_Record.V438 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V438", p_Record.V438));
                if (p_Record.V439 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V439", p_Record.V439));
                if (p_Record.V440 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V440", p_Record.V440));
                if (p_Record.V441 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V441", p_Record.V441));
                if (p_Record.V442 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V442", p_Record.V442));
                if (p_Record.V443 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V443", p_Record.V443));
                if (p_Record.V444 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V444", p_Record.V444));
                if (p_Record.V445 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V445", p_Record.V445));
                if (p_Record.V446 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V446", p_Record.V446));
                if (p_Record.V447 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V447", p_Record.V447));
                if (p_Record.V448 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V448", p_Record.V448));
                if (p_Record.V449 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V449", p_Record.V449));
                if (p_Record.V450 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V450", p_Record.V450));



                if (p_Record.V451 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V451", p_Record.V451));
                if (p_Record.V452 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V452", p_Record.V452));
                if (p_Record.V453 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V453", p_Record.V453));
                if (p_Record.V454 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V454", p_Record.V454));
                if (p_Record.V455 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V455", p_Record.V455));
                if (p_Record.V456 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V456", p_Record.V456));
                if (p_Record.V457 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V457", p_Record.V457));
                if (p_Record.V458 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V458", p_Record.V458));
                if (p_Record.V459 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V459", p_Record.V459));
                if (p_Record.V460 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V460", p_Record.V460));
                if (p_Record.V461 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V461", p_Record.V461));
                if (p_Record.V462 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V462", p_Record.V462));
                if (p_Record.V463 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V463", p_Record.V463));
                if (p_Record.V464 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V464", p_Record.V464));
                if (p_Record.V465 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V465", p_Record.V465));
                if (p_Record.V466 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V466", p_Record.V466));
                if (p_Record.V467 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V467", p_Record.V467));
                if (p_Record.V468 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V468", p_Record.V468));
                if (p_Record.V469 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V469", p_Record.V469));
                if (p_Record.V470 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V470", p_Record.V470));
                if (p_Record.V471 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V471", p_Record.V471));
                if (p_Record.V472 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V472", p_Record.V472));
                if (p_Record.V473 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V473", p_Record.V473));
                if (p_Record.V474 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V474", p_Record.V474));
                if (p_Record.V475 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V475", p_Record.V475));
                if (p_Record.V476 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V476", p_Record.V476));
                if (p_Record.V477 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V477", p_Record.V477));
                if (p_Record.V478 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V478", p_Record.V478));
                if (p_Record.V479 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V479", p_Record.V479));
                if (p_Record.V480 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V480", p_Record.V480));
                if (p_Record.V481 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V481", p_Record.V481));
                if (p_Record.V482 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V482", p_Record.V482));
                if (p_Record.V483 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V483", p_Record.V483));
                if (p_Record.V484 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V484", p_Record.V484));
                if (p_Record.V485 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V485", p_Record.V485));
                if (p_Record.V486 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V486", p_Record.V486));
                if (p_Record.V487 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V487", p_Record.V487));
                if (p_Record.V488 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V488", p_Record.V488));
                if (p_Record.V489 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V489", p_Record.V489));
                if (p_Record.V490 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V490", p_Record.V490));
                if (p_Record.V491 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V491", p_Record.V491));
                if (p_Record.V492 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V492", p_Record.V492));
                if (p_Record.V493 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V493", p_Record.V493));
                if (p_Record.V494 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V494", p_Record.V494));
                if (p_Record.V495 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V495", p_Record.V495));
                if (p_Record.V496 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V496", p_Record.V496));
                if (p_Record.V497 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V497", p_Record.V497));
                if (p_Record.V498 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V498", p_Record.V498));
                if (p_Record.V499 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V499", p_Record.V499));
                if (p_Record.V500 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V500", p_Record.V500));


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




    public static int ets_Record_Update(Record p_Record, bool? NeedAudit)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                if (NeedAudit != null)
                    command.Parameters.Add(new SqlParameter("@NeedAudit", NeedAudit));

                if (p_Record.DeleteReason != string.Empty)
                    command.Parameters.Add(new SqlParameter("@DeleteReason", p_Record.DeleteReason));

                command.Parameters.Add(new SqlParameter("@RecordID", p_Record.RecordID));

                //command.Parameters.Add(new SqlParameter("@LocationID", p_Record.LocationID));
                command.Parameters.Add(new SqlParameter("@TableID", p_Record.TableID));

                if (p_Record.DateTimeRecorded == null)
                    p_Record.DateTimeRecorded = DateTime.Now;
                command.Parameters.Add(new SqlParameter("@DateTimeRecorded", p_Record.DateTimeRecorded));
                if (p_Record.Notes != string.Empty)
                    command.Parameters.Add(new SqlParameter("@Notes", p_Record.Notes));

                if (p_Record.EnteredBy != null)
                    command.Parameters.Add(new SqlParameter("@EnteredBy", p_Record.EnteredBy));

                if (p_Record.IsActive == null)
                    p_Record.IsActive = true;
                command.Parameters.Add(new SqlParameter("@IsActive", p_Record.IsActive));


                if (p_Record.ValidationResults != string.Empty)
                    command.Parameters.Add(new SqlParameter("@ValidationResults", p_Record.ValidationResults));

                if (p_Record.WarningResults != string.Empty)
                    command.Parameters.Add(new SqlParameter("@WarningResults", p_Record.WarningResults));

                if (p_Record.LastUpdatedUserID != null)
                    command.Parameters.Add(new SqlParameter("@nLastUpdatedUserID", p_Record.LastUpdatedUserID));

                if (p_Record.V001 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V001", p_Record.V001));
                if (p_Record.V002 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V002", p_Record.V002));
                if (p_Record.V003 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V003", p_Record.V003));
                if (p_Record.V004 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V004", p_Record.V004));
                if (p_Record.V005 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V005", p_Record.V005));
                if (p_Record.V006 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V006", p_Record.V006));
                if (p_Record.V007 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V007", p_Record.V007));
                if (p_Record.V008 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V008", p_Record.V008));
                if (p_Record.V009 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V009", p_Record.V009));
                if (p_Record.V010 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V010", p_Record.V010));
                if (p_Record.V011 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V011", p_Record.V011));
                if (p_Record.V012 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V012", p_Record.V012));
                if (p_Record.V013 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V013", p_Record.V013));
                if (p_Record.V014 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V014", p_Record.V014));
                if (p_Record.V015 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V015", p_Record.V015));
                if (p_Record.V016 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V016", p_Record.V016));
                if (p_Record.V017 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V017", p_Record.V017));
                if (p_Record.V018 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V018", p_Record.V018));
                if (p_Record.V019 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V019", p_Record.V019));
                if (p_Record.V020 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V020", p_Record.V020));
                if (p_Record.V021 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V021", p_Record.V021));
                if (p_Record.V022 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V022", p_Record.V022));
                if (p_Record.V023 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V023", p_Record.V023));
                if (p_Record.V024 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V024", p_Record.V024));
                if (p_Record.V025 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V025", p_Record.V025));
                if (p_Record.V026 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V026", p_Record.V026));
                if (p_Record.V027 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V027", p_Record.V027));
                if (p_Record.V028 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V028", p_Record.V028));
                if (p_Record.V029 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V029", p_Record.V029));
                if (p_Record.V030 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V030", p_Record.V030));
                if (p_Record.V031 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V031", p_Record.V031));
                if (p_Record.V032 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V032", p_Record.V032));
                if (p_Record.V033 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V033", p_Record.V033));
                if (p_Record.V034 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V034", p_Record.V034));
                if (p_Record.V035 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V035", p_Record.V035));
                if (p_Record.V036 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V036", p_Record.V036));
                if (p_Record.V037 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V037", p_Record.V037));
                if (p_Record.V038 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V038", p_Record.V038));
                if (p_Record.V039 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V039", p_Record.V039));
                if (p_Record.V040 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V040", p_Record.V040));
                if (p_Record.V041 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V041", p_Record.V041));
                if (p_Record.V042 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V042", p_Record.V042));
                if (p_Record.V043 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V043", p_Record.V043));
                if (p_Record.V044 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V044", p_Record.V044));
                if (p_Record.V045 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V045", p_Record.V045));
                if (p_Record.V046 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V046", p_Record.V046));
                if (p_Record.V047 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V047", p_Record.V047));
                if (p_Record.V048 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V048", p_Record.V048));
                if (p_Record.V049 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V049", p_Record.V049));
                if (p_Record.V050 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V050", p_Record.V050));

                




                if (p_Record.V051 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V051", p_Record.V051));
                if (p_Record.V052 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V052", p_Record.V052));
                if (p_Record.V053 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V053", p_Record.V053));
                if (p_Record.V054 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V054", p_Record.V054));
                if (p_Record.V055 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V055", p_Record.V055));
                if (p_Record.V056 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V056", p_Record.V056));
                if (p_Record.V057 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V057", p_Record.V057));
                if (p_Record.V058 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V058", p_Record.V058));
                if (p_Record.V059 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V059", p_Record.V059));
                if (p_Record.V060 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V060", p_Record.V060));
                if (p_Record.V061 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V061", p_Record.V061));
                if (p_Record.V062 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V062", p_Record.V062));
                if (p_Record.V063 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V063", p_Record.V063));
                if (p_Record.V064 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V064", p_Record.V064));
                if (p_Record.V065 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V065", p_Record.V065));
                if (p_Record.V066 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V066", p_Record.V066));
                if (p_Record.V067 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V067", p_Record.V067));
                if (p_Record.V068 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V068", p_Record.V068));
                if (p_Record.V069 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V069", p_Record.V069));
                if (p_Record.V070 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V070", p_Record.V070));
                if (p_Record.V071 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V071", p_Record.V071));
                if (p_Record.V072 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V072", p_Record.V072));
                if (p_Record.V073 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V073", p_Record.V073));
                if (p_Record.V074 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V074", p_Record.V074));
                if (p_Record.V075 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V075", p_Record.V075));
                if (p_Record.V076 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V076", p_Record.V076));
                if (p_Record.V077 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V077", p_Record.V077));
                if (p_Record.V078 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V078", p_Record.V078));
                if (p_Record.V079 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V079", p_Record.V079));
                if (p_Record.V080 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V080", p_Record.V080));
                if (p_Record.V081 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V081", p_Record.V081));
                if (p_Record.V082 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V082", p_Record.V082));
                if (p_Record.V083 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V083", p_Record.V083));
                if (p_Record.V084 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V084", p_Record.V084));
                if (p_Record.V085 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V085", p_Record.V085));
                if (p_Record.V086 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V086", p_Record.V086));
                if (p_Record.V087 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V087", p_Record.V087));
                if (p_Record.V088 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V088", p_Record.V088));
                if (p_Record.V089 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V089", p_Record.V089));
                if (p_Record.V090 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V090", p_Record.V090));
                if (p_Record.V091 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V091", p_Record.V091));
                if (p_Record.V092 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V092", p_Record.V092));
                if (p_Record.V093 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V093", p_Record.V093));
                if (p_Record.V094 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V094", p_Record.V094));
                if (p_Record.V095 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V095", p_Record.V095));
                if (p_Record.V096 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V096", p_Record.V096));
                if (p_Record.V097 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V097", p_Record.V097));
                if (p_Record.V098 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V098", p_Record.V098));
                if (p_Record.V099 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V099", p_Record.V099));
                if (p_Record.V100 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V100", p_Record.V100));




                if (p_Record.V101 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V101", p_Record.V101));
                if (p_Record.V102 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V102", p_Record.V102));
                if (p_Record.V103 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V103", p_Record.V103));
                if (p_Record.V104 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V104", p_Record.V104));
                if (p_Record.V105 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V105", p_Record.V105));
                if (p_Record.V106 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V106", p_Record.V106));
                if (p_Record.V107 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V107", p_Record.V107));
                if (p_Record.V108 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V108", p_Record.V108));
                if (p_Record.V109 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V109", p_Record.V109));
                if (p_Record.V110 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V110", p_Record.V110));
                if (p_Record.V111 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V111", p_Record.V111));
                if (p_Record.V112 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V112", p_Record.V112));
                if (p_Record.V113 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V113", p_Record.V113));
                if (p_Record.V114 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V114", p_Record.V114));
                if (p_Record.V115 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V115", p_Record.V115));
                if (p_Record.V116 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V116", p_Record.V116));
                if (p_Record.V117 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V117", p_Record.V117));
                if (p_Record.V118 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V118", p_Record.V118));
                if (p_Record.V119 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V119", p_Record.V119));
                if (p_Record.V120 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V120", p_Record.V120));
                if (p_Record.V121 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V121", p_Record.V121));
                if (p_Record.V122 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V122", p_Record.V122));
                if (p_Record.V123 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V123", p_Record.V123));
                if (p_Record.V124 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V124", p_Record.V124));
                if (p_Record.V125 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V125", p_Record.V125));
                if (p_Record.V126 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V126", p_Record.V126));
                if (p_Record.V127 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V127", p_Record.V127));
                if (p_Record.V128 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V128", p_Record.V128));
                if (p_Record.V129 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V129", p_Record.V129));
                if (p_Record.V130 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V130", p_Record.V130));
                if (p_Record.V131 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V131", p_Record.V131));
                if (p_Record.V132 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V132", p_Record.V132));
                if (p_Record.V133 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V133", p_Record.V133));
                if (p_Record.V134 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V134", p_Record.V134));
                if (p_Record.V135 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V135", p_Record.V135));
                if (p_Record.V136 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V136", p_Record.V136));
                if (p_Record.V137 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V137", p_Record.V137));
                if (p_Record.V138 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V138", p_Record.V138));
                if (p_Record.V139 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V139", p_Record.V139));
                if (p_Record.V140 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V140", p_Record.V140));
                if (p_Record.V141 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V141", p_Record.V141));
                if (p_Record.V142 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V142", p_Record.V142));
                if (p_Record.V143 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V143", p_Record.V143));
                if (p_Record.V144 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V144", p_Record.V144));
                if (p_Record.V145 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V145", p_Record.V145));
                if (p_Record.V146 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V146", p_Record.V146));
                if (p_Record.V147 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V147", p_Record.V147));
                if (p_Record.V148 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V148", p_Record.V148));
                if (p_Record.V149 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V149", p_Record.V149));
                if (p_Record.V150 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V150", p_Record.V150));



                if (p_Record.V151 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V151", p_Record.V151));
                if (p_Record.V152 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V152", p_Record.V152));
                if (p_Record.V153 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V153", p_Record.V153));
                if (p_Record.V154 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V154", p_Record.V154));
                if (p_Record.V155 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V155", p_Record.V155));
                if (p_Record.V156 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V156", p_Record.V156));
                if (p_Record.V157 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V157", p_Record.V157));
                if (p_Record.V158 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V158", p_Record.V158));
                if (p_Record.V159 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V159", p_Record.V159));
                if (p_Record.V160 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V160", p_Record.V160));
                if (p_Record.V161 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V161", p_Record.V161));
                if (p_Record.V162 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V162", p_Record.V162));
                if (p_Record.V163 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V163", p_Record.V163));
                if (p_Record.V164 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V164", p_Record.V164));
                if (p_Record.V165 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V165", p_Record.V165));
                if (p_Record.V166 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V166", p_Record.V166));
                if (p_Record.V167 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V167", p_Record.V167));
                if (p_Record.V168 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V168", p_Record.V168));
                if (p_Record.V169 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V169", p_Record.V169));
                if (p_Record.V170 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V170", p_Record.V170));
                if (p_Record.V171 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V171", p_Record.V171));
                if (p_Record.V172 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V172", p_Record.V172));
                if (p_Record.V173 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V173", p_Record.V173));
                if (p_Record.V174 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V174", p_Record.V174));
                if (p_Record.V175 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V175", p_Record.V175));
                if (p_Record.V176 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V176", p_Record.V176));
                if (p_Record.V177 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V177", p_Record.V177));
                if (p_Record.V178 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V178", p_Record.V178));
                if (p_Record.V179 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V179", p_Record.V179));
                if (p_Record.V180 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V180", p_Record.V180));
                if (p_Record.V181 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V181", p_Record.V181));
                if (p_Record.V182 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V182", p_Record.V182));
                if (p_Record.V183 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V183", p_Record.V183));
                if (p_Record.V184 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V184", p_Record.V184));
                if (p_Record.V185 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V185", p_Record.V185));
                if (p_Record.V186 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V186", p_Record.V186));
                if (p_Record.V187 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V187", p_Record.V187));
                if (p_Record.V188 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V188", p_Record.V188));
                if (p_Record.V189 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V189", p_Record.V189));
                if (p_Record.V190 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V190", p_Record.V190));
                if (p_Record.V191 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V191", p_Record.V191));
                if (p_Record.V192 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V192", p_Record.V192));
                if (p_Record.V193 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V193", p_Record.V193));
                if (p_Record.V194 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V194", p_Record.V194));
                if (p_Record.V195 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V195", p_Record.V195));
                if (p_Record.V196 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V196", p_Record.V196));
                if (p_Record.V197 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V197", p_Record.V197));
                if (p_Record.V198 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V198", p_Record.V198));
                if (p_Record.V199 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V199", p_Record.V199));
                if (p_Record.V200 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V200", p_Record.V200));


                if (p_Record.V201 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V201", p_Record.V201));
                if (p_Record.V202 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V202", p_Record.V202));
                if (p_Record.V203 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V203", p_Record.V203));
                if (p_Record.V204 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V204", p_Record.V204));
                if (p_Record.V205 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V205", p_Record.V205));
                if (p_Record.V206 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V206", p_Record.V206));
                if (p_Record.V207 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V207", p_Record.V207));
                if (p_Record.V208 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V208", p_Record.V208));
                if (p_Record.V209 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V209", p_Record.V209));
                if (p_Record.V210 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V210", p_Record.V210));
                if (p_Record.V211 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V211", p_Record.V211));
                if (p_Record.V212 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V212", p_Record.V212));
                if (p_Record.V213 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V213", p_Record.V213));
                if (p_Record.V214 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V214", p_Record.V214));
                if (p_Record.V215 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V215", p_Record.V215));
                if (p_Record.V216 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V216", p_Record.V216));
                if (p_Record.V217 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V217", p_Record.V217));
                if (p_Record.V218 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V218", p_Record.V218));
                if (p_Record.V219 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V219", p_Record.V219));
                if (p_Record.V220 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V220", p_Record.V220));
                if (p_Record.V221 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V221", p_Record.V221));
                if (p_Record.V222 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V222", p_Record.V222));
                if (p_Record.V223 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V223", p_Record.V223));
                if (p_Record.V224 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V224", p_Record.V224));
                if (p_Record.V225 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V225", p_Record.V225));
                if (p_Record.V226 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V226", p_Record.V226));
                if (p_Record.V227 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V227", p_Record.V227));
                if (p_Record.V228 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V228", p_Record.V228));
                if (p_Record.V229 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V229", p_Record.V229));
                if (p_Record.V230 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V230", p_Record.V230));
                if (p_Record.V231 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V231", p_Record.V231));
                if (p_Record.V232 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V232", p_Record.V232));
                if (p_Record.V233 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V233", p_Record.V233));
                if (p_Record.V234 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V234", p_Record.V234));
                if (p_Record.V235 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V235", p_Record.V235));
                if (p_Record.V236 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V236", p_Record.V236));
                if (p_Record.V237 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V237", p_Record.V237));
                if (p_Record.V238 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V238", p_Record.V238));
                if (p_Record.V239 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V239", p_Record.V239));
                if (p_Record.V240 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V240", p_Record.V240));
                if (p_Record.V241 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V241", p_Record.V241));
                if (p_Record.V242 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V242", p_Record.V242));
                if (p_Record.V243 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V243", p_Record.V243));
                if (p_Record.V244 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V244", p_Record.V244));
                if (p_Record.V245 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V245", p_Record.V245));
                if (p_Record.V246 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V246", p_Record.V246));
                if (p_Record.V247 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V247", p_Record.V247));
                if (p_Record.V248 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V248", p_Record.V248));
                if (p_Record.V249 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V249", p_Record.V249));
                if (p_Record.V250 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V250", p_Record.V250));



                if (p_Record.V251 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V251", p_Record.V251));
                if (p_Record.V252 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V252", p_Record.V252));
                if (p_Record.V253 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V253", p_Record.V253));
                if (p_Record.V254 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V254", p_Record.V254));
                if (p_Record.V255 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V255", p_Record.V255));
                if (p_Record.V256 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V256", p_Record.V256));
                if (p_Record.V257 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V257", p_Record.V257));
                if (p_Record.V258 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V258", p_Record.V258));
                if (p_Record.V259 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V259", p_Record.V259));
                if (p_Record.V260 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V260", p_Record.V260));
                if (p_Record.V261 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V261", p_Record.V261));
                if (p_Record.V262 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V262", p_Record.V262));
                if (p_Record.V263 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V263", p_Record.V263));
                if (p_Record.V264 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V264", p_Record.V264));
                if (p_Record.V265 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V265", p_Record.V265));
                if (p_Record.V266 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V266", p_Record.V266));
                if (p_Record.V267 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V267", p_Record.V267));
                if (p_Record.V268 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V268", p_Record.V268));
                if (p_Record.V269 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V269", p_Record.V269));
                if (p_Record.V270 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V270", p_Record.V270));
                if (p_Record.V271 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V271", p_Record.V271));
                if (p_Record.V272 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V272", p_Record.V272));
                if (p_Record.V273 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V273", p_Record.V273));
                if (p_Record.V274 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V274", p_Record.V274));
                if (p_Record.V275 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V275", p_Record.V275));
                if (p_Record.V276 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V276", p_Record.V276));
                if (p_Record.V277 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V277", p_Record.V277));
                if (p_Record.V278 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V278", p_Record.V278));
                if (p_Record.V279 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V279", p_Record.V279));
                if (p_Record.V280 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V280", p_Record.V280));
                if (p_Record.V281 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V281", p_Record.V281));
                if (p_Record.V282 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V282", p_Record.V282));
                if (p_Record.V283 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V283", p_Record.V283));
                if (p_Record.V284 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V284", p_Record.V284));
                if (p_Record.V285 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V285", p_Record.V285));
                if (p_Record.V286 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V286", p_Record.V286));
                if (p_Record.V287 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V287", p_Record.V287));
                if (p_Record.V288 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V288", p_Record.V288));
                if (p_Record.V289 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V289", p_Record.V289));
                if (p_Record.V290 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V290", p_Record.V290));
                if (p_Record.V291 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V291", p_Record.V291));
                if (p_Record.V292 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V292", p_Record.V292));
                if (p_Record.V293 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V293", p_Record.V293));
                if (p_Record.V294 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V294", p_Record.V294));
                if (p_Record.V295 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V295", p_Record.V295));
                if (p_Record.V296 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V296", p_Record.V296));
                if (p_Record.V297 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V297", p_Record.V297));
                if (p_Record.V298 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V298", p_Record.V298));
                if (p_Record.V299 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V299", p_Record.V299));
                if (p_Record.V300 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V300", p_Record.V300));


                if (p_Record.V301 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V301", p_Record.V301));
                if (p_Record.V302 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V302", p_Record.V302));
                if (p_Record.V303 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V303", p_Record.V303));
                if (p_Record.V304 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V304", p_Record.V304));
                if (p_Record.V305 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V305", p_Record.V305));
                if (p_Record.V306 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V306", p_Record.V306));
                if (p_Record.V307 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V307", p_Record.V307));
                if (p_Record.V308 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V308", p_Record.V308));
                if (p_Record.V309 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V309", p_Record.V309));
                if (p_Record.V310 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V310", p_Record.V310));
                if (p_Record.V311 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V311", p_Record.V311));
                if (p_Record.V312 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V312", p_Record.V312));
                if (p_Record.V313 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V313", p_Record.V313));
                if (p_Record.V314 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V314", p_Record.V314));
                if (p_Record.V315 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V315", p_Record.V315));
                if (p_Record.V316 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V316", p_Record.V316));
                if (p_Record.V317 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V317", p_Record.V317));
                if (p_Record.V318 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V318", p_Record.V318));
                if (p_Record.V319 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V319", p_Record.V319));
                if (p_Record.V320 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V320", p_Record.V320));
                if (p_Record.V321 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V321", p_Record.V321));
                if (p_Record.V322 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V322", p_Record.V322));
                if (p_Record.V323 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V323", p_Record.V323));
                if (p_Record.V324 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V324", p_Record.V324));
                if (p_Record.V325 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V325", p_Record.V325));
                if (p_Record.V326 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V326", p_Record.V326));
                if (p_Record.V327 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V327", p_Record.V327));
                if (p_Record.V328 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V328", p_Record.V328));
                if (p_Record.V329 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V329", p_Record.V329));
                if (p_Record.V330 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V330", p_Record.V330));
                if (p_Record.V331 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V331", p_Record.V331));
                if (p_Record.V332 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V332", p_Record.V332));
                if (p_Record.V333 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V333", p_Record.V333));
                if (p_Record.V334 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V334", p_Record.V334));
                if (p_Record.V335 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V335", p_Record.V335));
                if (p_Record.V336 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V336", p_Record.V336));
                if (p_Record.V337 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V337", p_Record.V337));
                if (p_Record.V338 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V338", p_Record.V338));
                if (p_Record.V339 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V339", p_Record.V339));
                if (p_Record.V340 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V340", p_Record.V340));
                if (p_Record.V341 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V341", p_Record.V341));
                if (p_Record.V342 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V342", p_Record.V342));
                if (p_Record.V343 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V343", p_Record.V343));
                if (p_Record.V344 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V344", p_Record.V344));
                if (p_Record.V345 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V345", p_Record.V345));
                if (p_Record.V346 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V346", p_Record.V346));
                if (p_Record.V347 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V347", p_Record.V347));
                if (p_Record.V348 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V348", p_Record.V348));
                if (p_Record.V349 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V349", p_Record.V349));
                if (p_Record.V350 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V350", p_Record.V350));



                if (p_Record.V351 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V351", p_Record.V351));
                if (p_Record.V352 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V352", p_Record.V352));
                if (p_Record.V353 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V353", p_Record.V353));
                if (p_Record.V354 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V354", p_Record.V354));
                if (p_Record.V355 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V355", p_Record.V355));
                if (p_Record.V356 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V356", p_Record.V356));
                if (p_Record.V357 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V357", p_Record.V357));
                if (p_Record.V358 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V358", p_Record.V358));
                if (p_Record.V359 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V359", p_Record.V359));
                if (p_Record.V360 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V360", p_Record.V360));
                if (p_Record.V361 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V361", p_Record.V361));
                if (p_Record.V362 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V362", p_Record.V362));
                if (p_Record.V363 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V363", p_Record.V363));
                if (p_Record.V364 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V364", p_Record.V364));
                if (p_Record.V365 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V365", p_Record.V365));
                if (p_Record.V366 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V366", p_Record.V366));
                if (p_Record.V367 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V367", p_Record.V367));
                if (p_Record.V368 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V368", p_Record.V368));
                if (p_Record.V369 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V369", p_Record.V369));
                if (p_Record.V370 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V370", p_Record.V370));
                if (p_Record.V371 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V371", p_Record.V371));
                if (p_Record.V372 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V372", p_Record.V372));
                if (p_Record.V373 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V373", p_Record.V373));
                if (p_Record.V374 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V374", p_Record.V374));
                if (p_Record.V375 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V375", p_Record.V375));
                if (p_Record.V376 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V376", p_Record.V376));
                if (p_Record.V377 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V377", p_Record.V377));
                if (p_Record.V378 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V378", p_Record.V378));
                if (p_Record.V379 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V379", p_Record.V379));
                if (p_Record.V380 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V380", p_Record.V380));
                if (p_Record.V381 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V381", p_Record.V381));
                if (p_Record.V382 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V382", p_Record.V382));
                if (p_Record.V383 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V383", p_Record.V383));
                if (p_Record.V384 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V384", p_Record.V384));
                if (p_Record.V385 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V385", p_Record.V385));
                if (p_Record.V386 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V386", p_Record.V386));
                if (p_Record.V387 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V387", p_Record.V387));
                if (p_Record.V388 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V388", p_Record.V388));
                if (p_Record.V389 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V389", p_Record.V389));
                if (p_Record.V390 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V390", p_Record.V390));
                if (p_Record.V391 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V391", p_Record.V391));
                if (p_Record.V392 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V392", p_Record.V392));
                if (p_Record.V393 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V393", p_Record.V393));
                if (p_Record.V394 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V394", p_Record.V394));
                if (p_Record.V395 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V395", p_Record.V395));
                if (p_Record.V396 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V396", p_Record.V396));
                if (p_Record.V397 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V397", p_Record.V397));
                if (p_Record.V398 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V398", p_Record.V398));
                if (p_Record.V399 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V399", p_Record.V399));
                if (p_Record.V400 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V400", p_Record.V400));


                if (p_Record.V401 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V401", p_Record.V401));
                if (p_Record.V402 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V402", p_Record.V402));
                if (p_Record.V403 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V403", p_Record.V403));
                if (p_Record.V404 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V404", p_Record.V404));
                if (p_Record.V405 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V405", p_Record.V405));
                if (p_Record.V406 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V406", p_Record.V406));
                if (p_Record.V407 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V407", p_Record.V407));
                if (p_Record.V408 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V408", p_Record.V408));
                if (p_Record.V409 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V409", p_Record.V409));
                if (p_Record.V410 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V410", p_Record.V410));
                if (p_Record.V411 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V411", p_Record.V411));
                if (p_Record.V412 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V412", p_Record.V412));
                if (p_Record.V413 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V413", p_Record.V413));
                if (p_Record.V414 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V414", p_Record.V414));
                if (p_Record.V415 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V415", p_Record.V415));
                if (p_Record.V416 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V416", p_Record.V416));
                if (p_Record.V417 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V417", p_Record.V417));
                if (p_Record.V418 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V418", p_Record.V418));
                if (p_Record.V419 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V419", p_Record.V419));
                if (p_Record.V420 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V420", p_Record.V420));
                if (p_Record.V421 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V421", p_Record.V421));
                if (p_Record.V422 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V422", p_Record.V422));
                if (p_Record.V423 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V423", p_Record.V423));
                if (p_Record.V424 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V424", p_Record.V424));
                if (p_Record.V425 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V425", p_Record.V425));
                if (p_Record.V426 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V426", p_Record.V426));
                if (p_Record.V427 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V427", p_Record.V427));
                if (p_Record.V428 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V428", p_Record.V428));
                if (p_Record.V429 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V429", p_Record.V429));
                if (p_Record.V430 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V430", p_Record.V430));
                if (p_Record.V431 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V431", p_Record.V431));
                if (p_Record.V432 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V432", p_Record.V432));
                if (p_Record.V433 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V433", p_Record.V433));
                if (p_Record.V434 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V434", p_Record.V434));
                if (p_Record.V435 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V435", p_Record.V435));
                if (p_Record.V436 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V436", p_Record.V436));
                if (p_Record.V437 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V437", p_Record.V437));
                if (p_Record.V438 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V438", p_Record.V438));
                if (p_Record.V439 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V439", p_Record.V439));
                if (p_Record.V440 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V440", p_Record.V440));
                if (p_Record.V441 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V441", p_Record.V441));
                if (p_Record.V442 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V442", p_Record.V442));
                if (p_Record.V443 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V443", p_Record.V443));
                if (p_Record.V444 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V444", p_Record.V444));
                if (p_Record.V445 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V445", p_Record.V445));
                if (p_Record.V446 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V446", p_Record.V446));
                if (p_Record.V447 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V447", p_Record.V447));
                if (p_Record.V448 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V448", p_Record.V448));
                if (p_Record.V449 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V449", p_Record.V449));
                if (p_Record.V450 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V450", p_Record.V450));



                if (p_Record.V451 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V451", p_Record.V451));
                if (p_Record.V452 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V452", p_Record.V452));
                if (p_Record.V453 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V453", p_Record.V453));
                if (p_Record.V454 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V454", p_Record.V454));
                if (p_Record.V455 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V455", p_Record.V455));
                if (p_Record.V456 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V456", p_Record.V456));
                if (p_Record.V457 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V457", p_Record.V457));
                if (p_Record.V458 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V458", p_Record.V458));
                if (p_Record.V459 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V459", p_Record.V459));
                if (p_Record.V460 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V460", p_Record.V460));
                if (p_Record.V461 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V461", p_Record.V461));
                if (p_Record.V462 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V462", p_Record.V462));
                if (p_Record.V463 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V463", p_Record.V463));
                if (p_Record.V464 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V464", p_Record.V464));
                if (p_Record.V465 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V465", p_Record.V465));
                if (p_Record.V466 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V466", p_Record.V466));
                if (p_Record.V467 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V467", p_Record.V467));
                if (p_Record.V468 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V468", p_Record.V468));
                if (p_Record.V469 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V469", p_Record.V469));
                if (p_Record.V470 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V470", p_Record.V470));
                if (p_Record.V471 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V471", p_Record.V471));
                if (p_Record.V472 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V472", p_Record.V472));
                if (p_Record.V473 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V473", p_Record.V473));
                if (p_Record.V474 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V474", p_Record.V474));
                if (p_Record.V475 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V475", p_Record.V475));
                if (p_Record.V476 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V476", p_Record.V476));
                if (p_Record.V477 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V477", p_Record.V477));
                if (p_Record.V478 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V478", p_Record.V478));
                if (p_Record.V479 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V479", p_Record.V479));
                if (p_Record.V480 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V480", p_Record.V480));
                if (p_Record.V481 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V481", p_Record.V481));
                if (p_Record.V482 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V482", p_Record.V482));
                if (p_Record.V483 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V483", p_Record.V483));
                if (p_Record.V484 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V484", p_Record.V484));
                if (p_Record.V485 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V485", p_Record.V485));
                if (p_Record.V486 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V486", p_Record.V486));
                if (p_Record.V487 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V487", p_Record.V487));
                if (p_Record.V488 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V488", p_Record.V488));
                if (p_Record.V489 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V489", p_Record.V489));
                if (p_Record.V490 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V490", p_Record.V490));
                if (p_Record.V491 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V491", p_Record.V491));
                if (p_Record.V492 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V492", p_Record.V492));
                if (p_Record.V493 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V493", p_Record.V493));
                if (p_Record.V494 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V494", p_Record.V494));
                if (p_Record.V495 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V495", p_Record.V495));
                if (p_Record.V496 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V496", p_Record.V496));
                if (p_Record.V497 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V497", p_Record.V497));
                if (p_Record.V498 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V498", p_Record.V498));
                if (p_Record.V499 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V499", p_Record.V499));
                if (p_Record.V500 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V500", p_Record.V500));



                if (p_Record.ChangeReason != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sChangeReason", p_Record.ChangeReason));

                if (p_Record.OwnerUserID != null)
                    command.Parameters.Add(new SqlParameter("@nOwnerUserID", p_Record.OwnerUserID));



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


    public static int ets_Record_Delete(int iRecordID, int nUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RecordID", iRecordID));
                command.Parameters.Add(new SqlParameter("@nUserID", nUserID));
                command.CommandTimeout = 0;

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


    public static int ets_Record_UnDelete(int iRecordID, int nUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_UnDelete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RecordID", iRecordID));
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



    public static string fnReplaceDisplayColumns(string sDisplay, int nTableID, int? nColumnID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbo.fnReplaceDisplayColumns", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@Result", SqlDbType.VarChar);
                pRV.Direction = ParameterDirection.ReturnValue;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                command.Parameters.Add(new SqlParameter("@sDisplay", sDisplay));

                if (nColumnID!=null)
                    command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

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

    public static string fnReplaceDisplayColumns_NoAlias(int ColumnID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbo.fnReplaceDisplayColumns_NoAlias", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@Result", SqlDbType.VarChar);
                pRV.Direction = ParameterDirection.ReturnValue;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@ColumnID", ColumnID));                


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
    public static string fnGetSystemUserDisplayText(string sDisplay, string sUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbo.fnGetSystemUserDisplayText", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@Result", SqlDbType.VarChar);
                pRV.Direction = ParameterDirection.ReturnValue;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sUserID", sUserID));

                command.Parameters.Add(new SqlParameter("@sDisplay", sDisplay));


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

    public static DataTable ets_Record_List(int nTableID,
    int? nEnteredBy, bool? bIsActive, bool? bHasWarningResults,  DateTime? dDateFrom, DateTime? dDateTo,
        string sOrder,
  string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, ref int iTotalDynamicColumns,
        string sType, string sNumericSearch, string sTextSearch, DateTime? dDateAddedFrom, DateTime? dDateAddedTo,
        string sParentColumnSortSQL, string sHeaderSQL, string sViewName, int? nViewID, ref string strReturnSQL, ref string sReturnHeaderSQL)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_List", connection)) //   ets_Record_List_BU_19-Oct-2015
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));
                sTextSearch = sTextSearch.Trim();
                sNumericSearch = sNumericSearch.Trim();

                if (sTextSearch.IndexOf("-user-")>0)
                {
                    try
                    {
                        DataTable dtTemp = Common.DataTableFromText(@"SELECT C.TableTableID,C.LinkedParentColumnID FROM [Column] C INNER JOIN [Column] P 
                    ON C.TableTableID=P.TableID WHERE C.TableID=" + nTableID.ToString() + " AND P.TableTableID=-1  AND C.TableTableID IS NOT NULL AND C.LinkedParentColumnID IS NOT NULL");
                        if(dtTemp.Rows.Count>0)
                        {
                            string strLoginValue = "";
                            string strLoginText = "";
                            User objUser = (User)System.Web.HttpContext.Current.Session["User"];
                            SecurityManager.ProcessLoginUserDefault(dtTemp.Rows[0]["TableTableID"].ToString(), "",
                                   dtTemp.Rows[0]["LinkedParentColumnID"].ToString(), objUser.UserID.ToString(), ref  strLoginValue, ref  strLoginText);
                            if(strLoginValue!="")
                            {
                                sTextSearch = sTextSearch.Replace("-user-", strLoginValue);
                            }
                        }

                    }
                    catch
                    {

                    }
                }


                SqlParameter pReturnSQL = new SqlParameter("@sReturnSQL", SqlDbType.Int);
                pReturnSQL.Direction = ParameterDirection.Output;

                SqlParameter pReturnHeaderSQL = new SqlParameter("@sReturnHeaderSQL", SqlDbType.Int);
                pReturnHeaderSQL.Direction = ParameterDirection.Output;

                if (sHeaderSQL != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sHeaderSQL", sHeaderSQL));

                if (sType != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sType", sType));

                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nViewID != null)
                    command.Parameters.Add(new SqlParameter("@nViewID", nViewID));


                if (nEnteredBy != null)
                    command.Parameters.Add(new SqlParameter("@nEnteredBy", nEnteredBy));

                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));

                if (bHasWarningResults != null)
                    command.Parameters.Add(new SqlParameter("@bHasWarningResults", bHasWarningResults));

                //if (sLocations != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sLocations", sLocations));

                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));
                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));

                if (dDateAddedFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateAddedFrom", dDateAddedFrom));
                if (dDateAddedTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateAddedTo", dDateAddedTo));


                if (sNumericSearch != "" && sTextSearch != "")
                {
                    if (sNumericSearch.Trim().Substring(0, 1) == "O")
                    {
                        sTextSearch = sTextSearch + sNumericSearch;
                    }
                    else
                    {
                        
                            sTextSearch = sNumericSearch + sTextSearch;
                        
                    }
                    sNumericSearch = "";
                }


                if (sNumericSearch != "")
                {
                    if (sNumericSearch.Trim().Substring(0, 2).ToLower() == "or")
                    {
                        sNumericSearch = sNumericSearch.Substring(3);
                    }
                    else if (sNumericSearch.Trim().Substring(0, 3).ToLower() == "and")
                    {
                        sNumericSearch = sNumericSearch.Substring(4);
                    }
                    command.Parameters.Add(new SqlParameter("@sNumericSearch", sNumericSearch));
                }

                if (sTextSearch != "")
                {
                    if (sTextSearch.Trim().Substring(0, 2).ToLower() == "or")
                    {
                        sTextSearch = sTextSearch.Substring(3);
                    }
                    else if (sTextSearch.Trim().Substring(0, 3).ToLower() == "and")
                    {
                        sTextSearch = sTextSearch.Substring(4);
                    }

                    command.Parameters.Add(new SqlParameter("@sTextSearch", sTextSearch));
                }



                //if (sOrder == string.Empty || sOrderDirection==string.Empty)
                //{
                //    sOrder="DBGSystemRecordID"; 
                //    sOrderDirection="DESC";

                //}

                if (sOrder.IndexOf("VERT(decimal", 0) > 0)
                {
                    command.Parameters.Add(new SqlParameter("@sOrder", sOrder + sOrderDirection));
                }
                else
                {
                    if (sOrder != "")
                    {

                        if (sOrder.IndexOf("CONVERT(Datetime") > -1)
                        {
                            command.Parameters.Add(new SqlParameter("@sOrder",  sOrder + sOrderDirection));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));
                        }
                    }
                }

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                if (sParentColumnSortSQL != "")
                    command.Parameters.Add(new SqlParameter("@sParentColumnSortSQL", sParentColumnSortSQL));

                if (sViewName != "")
                    command.Parameters.Add(new SqlParameter("@sViewName", sViewName));


                
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
                    iTotalDynamicColumns = ds.Tables[0].Columns.Count;
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }

                if (pReturnSQL.Value != null)
                    strReturnSQL = pReturnSQL.Value.ToString();

                if (pReturnHeaderSQL.Value != null)
                    sReturnHeaderSQL = pReturnHeaderSQL.Value.ToString();

                if (sType == "SQLOnly" && ds != null && ds.Tables.Count > 2)
                {
                    if (ds.Tables[2] != null && ds.Tables[2].Columns.Count == 2 && ds.Tables[2].Rows.Count == 1)
                    {
                        if (ds.Tables[2].Rows[0][0] != DBNull.Value)
                            strReturnSQL = ds.Tables[2].Rows[0][0].ToString();
                        if (ds.Tables[2].Rows[0][0] != DBNull.Value)
                            sReturnHeaderSQL = ds.Tables[2].Rows[0][1].ToString();
                    }
                }


                if (ds != null && ds.Tables.Count > 1)
                {
                    //foreach (DataColumn dc in ds.Tables[0].Columns)
                    //{
                    //    if(dc.ColumnName.IndexOf("_ID**")>-1)
                    //    {
                    //        ds.Tables[0].Columns[0].SetOrdinal
                    //    }
                    //}

                     for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                     {
                         if(ds.Tables[0].Columns[j].ColumnName.IndexOf("_ID**")>-1)
                         {
                             ds.Tables[0].Columns[j].SetOrdinal(ds.Tables[0].Columns.Count - 3);
                         }
                     }


                    return ds.Tables[0];
                }

                
                
                return null;
                
            }
        }
    }




    public static int ets_Record_List_BulkExport(int nTableID,
    int? nEnteredBy, bool? bIsActive, bool? bHasWarningResult,  DateTime? dDateFrom, DateTime? dDateTo,
        string sFileName)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_List_BulkExport", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@sFileName", sFileName));

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nEnteredBy != null)
                    command.Parameters.Add(new SqlParameter("@nEnteredBy", nEnteredBy));

                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));

                if (bHasWarningResult != null)
                    command.Parameters.Add(new SqlParameter("@bHasWarningResult", bHasWarningResult));

                //if (sLocations != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sLocations", sLocations));

                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));
                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));


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



    public static System.Data.DataSet ets_Record_Details(int nRecordID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nRecordID", nRecordID));

                
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
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

                return ds;

            }
        }
    }



    public static Record ets_Record_Detail_Full(int nRecordID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_Detail_Full", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nRecordID", nRecordID));

                connection.Open();


                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Record temp = new Record();
                            temp.RecordID = nRecordID;
                            //temp.LocationID = reader["LocationID"] == DBNull.Value ? null : (int?)reader["LocationID"];
                            temp.TableID = (int)reader["TableID"];
                            temp.DateTimeRecorded = (DateTime)reader["DATETIMERecorded"];
                            temp.Notes = reader["NOTES"] == DBNull.Value ? string.Empty : (string)reader["NOTES"];
                            temp.EnteredBy = (int)reader["ENTEREDBY"];
                            temp.IsActive = (bool)reader["ISACTIVE"];
                            temp.DateAdded = (DateTime)reader["DateAdded"];
                            temp.DateUpdated = (DateTime)reader["DateUpdated"];
                            temp.TempRecordID = reader["TempRecordID"] == DBNull.Value ? null : (int?)reader["TempRecordID"];
                            temp.BatchID = reader["BatchID"] == DBNull.Value ? null : (int?)reader["BatchID"];
                            temp.DeleteReason = reader["DeleteReason"] == DBNull.Value ? string.Empty : (string)reader["DeleteReason"];

                            temp.V001 = reader["V001"] == DBNull.Value ? string.Empty : (string)reader["V001"];
                            temp.V002 = reader["V002"] == DBNull.Value ? string.Empty : (string)reader["V002"];
                            temp.V003 = reader["V003"] == DBNull.Value ? string.Empty : (string)reader["V003"];
                            temp.V004 = reader["V004"] == DBNull.Value ? string.Empty : (string)reader["V004"];
                            temp.V005 = reader["V005"] == DBNull.Value ? string.Empty : (string)reader["V005"];
                            temp.V006 = reader["V006"] == DBNull.Value ? string.Empty : (string)reader["V006"];
                            temp.V007 = reader["V007"] == DBNull.Value ? string.Empty : (string)reader["V007"];
                            temp.V008 = reader["V008"] == DBNull.Value ? string.Empty : (string)reader["V008"];
                            temp.V009 = reader["V009"] == DBNull.Value ? string.Empty : (string)reader["V009"];
                            temp.V010 = reader["V010"] == DBNull.Value ? string.Empty : (string)reader["V010"];
                            temp.V011 = reader["V011"] == DBNull.Value ? string.Empty : (string)reader["V011"];
                            temp.V012 = reader["V012"] == DBNull.Value ? string.Empty : (string)reader["V012"];
                            temp.V013 = reader["V013"] == DBNull.Value ? string.Empty : (string)reader["V013"];
                            temp.V014 = reader["V014"] == DBNull.Value ? string.Empty : (string)reader["V014"];
                            temp.V015 = reader["V015"] == DBNull.Value ? string.Empty : (string)reader["V015"];
                            temp.V016 = reader["V016"] == DBNull.Value ? string.Empty : (string)reader["V016"];
                            temp.V017 = reader["V017"] == DBNull.Value ? string.Empty : (string)reader["V017"];
                            temp.V018 = reader["V018"] == DBNull.Value ? string.Empty : (string)reader["V018"];
                            temp.V019 = reader["V019"] == DBNull.Value ? string.Empty : (string)reader["V019"];
                            temp.V020 = reader["V020"] == DBNull.Value ? string.Empty : (string)reader["V020"];
                            temp.V021 = reader["V021"] == DBNull.Value ? string.Empty : (string)reader["V021"];
                            temp.V022 = reader["V022"] == DBNull.Value ? string.Empty : (string)reader["V022"];
                            temp.V023 = reader["V023"] == DBNull.Value ? string.Empty : (string)reader["V023"];
                            temp.V024 = reader["V024"] == DBNull.Value ? string.Empty : (string)reader["V024"];
                            temp.V025 = reader["V025"] == DBNull.Value ? string.Empty : (string)reader["V025"];
                            temp.V026 = reader["V026"] == DBNull.Value ? string.Empty : (string)reader["V026"];
                            temp.V027 = reader["V027"] == DBNull.Value ? string.Empty : (string)reader["V027"];
                            temp.V028 = reader["V028"] == DBNull.Value ? string.Empty : (string)reader["V028"];
                            temp.V029 = reader["V029"] == DBNull.Value ? string.Empty : (string)reader["V029"];
                            temp.V030 = reader["V030"] == DBNull.Value ? string.Empty : (string)reader["V030"];
                            temp.V031 = reader["V031"] == DBNull.Value ? string.Empty : (string)reader["V031"];
                            temp.V032 = reader["V032"] == DBNull.Value ? string.Empty : (string)reader["V032"];
                            temp.V033 = reader["V033"] == DBNull.Value ? string.Empty : (string)reader["V033"];
                            temp.V034 = reader["V034"] == DBNull.Value ? string.Empty : (string)reader["V034"];
                            temp.V035 = reader["V035"] == DBNull.Value ? string.Empty : (string)reader["V035"];
                            temp.V036 = reader["V036"] == DBNull.Value ? string.Empty : (string)reader["V036"];
                            temp.V037 = reader["V037"] == DBNull.Value ? string.Empty : (string)reader["V037"];
                            temp.V038 = reader["V038"] == DBNull.Value ? string.Empty : (string)reader["V038"];
                            temp.V039 = reader["V039"] == DBNull.Value ? string.Empty : (string)reader["V039"];
                            temp.V040 = reader["V040"] == DBNull.Value ? string.Empty : (string)reader["V040"];
                            temp.V041 = reader["V041"] == DBNull.Value ? string.Empty : (string)reader["V041"];
                            temp.V042 = reader["V042"] == DBNull.Value ? string.Empty : (string)reader["V042"];
                            temp.V043 = reader["V043"] == DBNull.Value ? string.Empty : (string)reader["V043"];
                            temp.V044 = reader["V044"] == DBNull.Value ? string.Empty : (string)reader["V044"];
                            temp.V045 = reader["V045"] == DBNull.Value ? string.Empty : (string)reader["V045"];
                            temp.V046 = reader["V046"] == DBNull.Value ? string.Empty : (string)reader["V046"];
                            temp.V047 = reader["V047"] == DBNull.Value ? string.Empty : (string)reader["V047"];
                            temp.V048 = reader["V048"] == DBNull.Value ? string.Empty : (string)reader["V048"];
                            temp.V049 = reader["V049"] == DBNull.Value ? string.Empty : (string)reader["V049"];
                            temp.V050 = reader["V050"] == DBNull.Value ? string.Empty : (string)reader["V050"];

                            temp.V051 = reader["V051"] == DBNull.Value ? string.Empty : (string)reader["V051"];
                            temp.V052 = reader["V052"] == DBNull.Value ? string.Empty : (string)reader["V052"];
                            temp.V053 = reader["V053"] == DBNull.Value ? string.Empty : (string)reader["V053"];
                            temp.V054 = reader["V054"] == DBNull.Value ? string.Empty : (string)reader["V054"];
                            temp.V055 = reader["V055"] == DBNull.Value ? string.Empty : (string)reader["V055"];
                            temp.V056 = reader["V056"] == DBNull.Value ? string.Empty : (string)reader["V056"];
                            temp.V057 = reader["V057"] == DBNull.Value ? string.Empty : (string)reader["V057"];
                            temp.V058 = reader["V058"] == DBNull.Value ? string.Empty : (string)reader["V058"];
                            temp.V059 = reader["V059"] == DBNull.Value ? string.Empty : (string)reader["V059"];
                            temp.V060 = reader["V060"] == DBNull.Value ? string.Empty : (string)reader["V060"];
                            temp.V061 = reader["V061"] == DBNull.Value ? string.Empty : (string)reader["V061"];
                            temp.V062 = reader["V062"] == DBNull.Value ? string.Empty : (string)reader["V062"];
                            temp.V063 = reader["V063"] == DBNull.Value ? string.Empty : (string)reader["V063"];
                            temp.V064 = reader["V064"] == DBNull.Value ? string.Empty : (string)reader["V064"];
                            temp.V065 = reader["V065"] == DBNull.Value ? string.Empty : (string)reader["V065"];
                            temp.V066 = reader["V066"] == DBNull.Value ? string.Empty : (string)reader["V066"];
                            temp.V067 = reader["V067"] == DBNull.Value ? string.Empty : (string)reader["V067"];
                            temp.V068 = reader["V068"] == DBNull.Value ? string.Empty : (string)reader["V068"];
                            temp.V069 = reader["V069"] == DBNull.Value ? string.Empty : (string)reader["V069"];
                            temp.V070 = reader["V070"] == DBNull.Value ? string.Empty : (string)reader["V070"];
                            temp.V071 = reader["V071"] == DBNull.Value ? string.Empty : (string)reader["V071"];
                            temp.V072 = reader["V072"] == DBNull.Value ? string.Empty : (string)reader["V072"];
                            temp.V073 = reader["V073"] == DBNull.Value ? string.Empty : (string)reader["V073"];
                            temp.V074 = reader["V074"] == DBNull.Value ? string.Empty : (string)reader["V074"];
                            temp.V075 = reader["V075"] == DBNull.Value ? string.Empty : (string)reader["V075"];
                            temp.V076 = reader["V076"] == DBNull.Value ? string.Empty : (string)reader["V076"];
                            temp.V077 = reader["V077"] == DBNull.Value ? string.Empty : (string)reader["V077"];
                            temp.V078 = reader["V078"] == DBNull.Value ? string.Empty : (string)reader["V078"];
                            temp.V079 = reader["V079"] == DBNull.Value ? string.Empty : (string)reader["V079"];
                            temp.V080 = reader["V080"] == DBNull.Value ? string.Empty : (string)reader["V080"];
                            temp.V081 = reader["V081"] == DBNull.Value ? string.Empty : (string)reader["V081"];
                            temp.V082 = reader["V082"] == DBNull.Value ? string.Empty : (string)reader["V082"];
                            temp.V083 = reader["V083"] == DBNull.Value ? string.Empty : (string)reader["V083"];
                            temp.V084 = reader["V084"] == DBNull.Value ? string.Empty : (string)reader["V084"];
                            temp.V085 = reader["V085"] == DBNull.Value ? string.Empty : (string)reader["V085"];
                            temp.V086 = reader["V086"] == DBNull.Value ? string.Empty : (string)reader["V086"];
                            temp.V087 = reader["V087"] == DBNull.Value ? string.Empty : (string)reader["V087"];
                            temp.V088 = reader["V088"] == DBNull.Value ? string.Empty : (string)reader["V088"];
                            temp.V089 = reader["V089"] == DBNull.Value ? string.Empty : (string)reader["V089"];
                            temp.V090 = reader["V090"] == DBNull.Value ? string.Empty : (string)reader["V090"];
                            temp.V091 = reader["V091"] == DBNull.Value ? string.Empty : (string)reader["V091"];
                            temp.V092 = reader["V092"] == DBNull.Value ? string.Empty : (string)reader["V092"];
                            temp.V093 = reader["V093"] == DBNull.Value ? string.Empty : (string)reader["V093"];
                            temp.V094 = reader["V094"] == DBNull.Value ? string.Empty : (string)reader["V094"];
                            temp.V095 = reader["V095"] == DBNull.Value ? string.Empty : (string)reader["V095"];
                            temp.V096 = reader["V096"] == DBNull.Value ? string.Empty : (string)reader["V096"];
                            temp.V097 = reader["V097"] == DBNull.Value ? string.Empty : (string)reader["V097"];
                            temp.V098 = reader["V098"] == DBNull.Value ? string.Empty : (string)reader["V098"];
                            temp.V099 = reader["V099"] == DBNull.Value ? string.Empty : (string)reader["V099"];
                            temp.V100 = reader["V100"] == DBNull.Value ? string.Empty : (string)reader["V100"];



                            temp.V101 = reader["V101"] == DBNull.Value ? string.Empty : (string)reader["V101"];
                            temp.V102 = reader["V102"] == DBNull.Value ? string.Empty : (string)reader["V102"];
                            temp.V103 = reader["V103"] == DBNull.Value ? string.Empty : (string)reader["V103"];
                            temp.V104 = reader["V104"] == DBNull.Value ? string.Empty : (string)reader["V104"];
                            temp.V105 = reader["V105"] == DBNull.Value ? string.Empty : (string)reader["V105"];
                            temp.V106 = reader["V106"] == DBNull.Value ? string.Empty : (string)reader["V106"];
                            temp.V107 = reader["V107"] == DBNull.Value ? string.Empty : (string)reader["V107"];
                            temp.V108 = reader["V108"] == DBNull.Value ? string.Empty : (string)reader["V108"];
                            temp.V109 = reader["V109"] == DBNull.Value ? string.Empty : (string)reader["V109"];
                            temp.V110 = reader["V110"] == DBNull.Value ? string.Empty : (string)reader["V110"];
                            temp.V111 = reader["V111"] == DBNull.Value ? string.Empty : (string)reader["V111"];
                            temp.V112 = reader["V112"] == DBNull.Value ? string.Empty : (string)reader["V112"];
                            temp.V113 = reader["V113"] == DBNull.Value ? string.Empty : (string)reader["V113"];
                            temp.V114 = reader["V114"] == DBNull.Value ? string.Empty : (string)reader["V114"];
                            temp.V115 = reader["V115"] == DBNull.Value ? string.Empty : (string)reader["V115"];
                            temp.V116 = reader["V116"] == DBNull.Value ? string.Empty : (string)reader["V116"];
                            temp.V117 = reader["V117"] == DBNull.Value ? string.Empty : (string)reader["V117"];
                            temp.V118 = reader["V118"] == DBNull.Value ? string.Empty : (string)reader["V118"];
                            temp.V119 = reader["V119"] == DBNull.Value ? string.Empty : (string)reader["V119"];
                            temp.V120 = reader["V120"] == DBNull.Value ? string.Empty : (string)reader["V120"];
                            temp.V121 = reader["V121"] == DBNull.Value ? string.Empty : (string)reader["V121"];
                            temp.V122 = reader["V122"] == DBNull.Value ? string.Empty : (string)reader["V122"];
                            temp.V123 = reader["V123"] == DBNull.Value ? string.Empty : (string)reader["V123"];
                            temp.V124 = reader["V124"] == DBNull.Value ? string.Empty : (string)reader["V124"];
                            temp.V125 = reader["V125"] == DBNull.Value ? string.Empty : (string)reader["V125"];
                            temp.V126 = reader["V126"] == DBNull.Value ? string.Empty : (string)reader["V126"];
                            temp.V127 = reader["V127"] == DBNull.Value ? string.Empty : (string)reader["V127"];
                            temp.V128 = reader["V128"] == DBNull.Value ? string.Empty : (string)reader["V128"];
                            temp.V129 = reader["V129"] == DBNull.Value ? string.Empty : (string)reader["V129"];
                            temp.V130 = reader["V130"] == DBNull.Value ? string.Empty : (string)reader["V130"];
                            temp.V131 = reader["V131"] == DBNull.Value ? string.Empty : (string)reader["V131"];
                            temp.V132 = reader["V132"] == DBNull.Value ? string.Empty : (string)reader["V132"];
                            temp.V133 = reader["V133"] == DBNull.Value ? string.Empty : (string)reader["V133"];
                            temp.V134 = reader["V134"] == DBNull.Value ? string.Empty : (string)reader["V134"];
                            temp.V135 = reader["V135"] == DBNull.Value ? string.Empty : (string)reader["V135"];
                            temp.V136 = reader["V136"] == DBNull.Value ? string.Empty : (string)reader["V136"];
                            temp.V137 = reader["V137"] == DBNull.Value ? string.Empty : (string)reader["V137"];
                            temp.V138 = reader["V138"] == DBNull.Value ? string.Empty : (string)reader["V138"];
                            temp.V139 = reader["V139"] == DBNull.Value ? string.Empty : (string)reader["V139"];
                            temp.V140 = reader["V140"] == DBNull.Value ? string.Empty : (string)reader["V140"];
                            temp.V141 = reader["V141"] == DBNull.Value ? string.Empty : (string)reader["V141"];
                            temp.V142 = reader["V142"] == DBNull.Value ? string.Empty : (string)reader["V142"];
                            temp.V143 = reader["V143"] == DBNull.Value ? string.Empty : (string)reader["V143"];
                            temp.V144 = reader["V144"] == DBNull.Value ? string.Empty : (string)reader["V144"];
                            temp.V145 = reader["V145"] == DBNull.Value ? string.Empty : (string)reader["V145"];
                            temp.V146 = reader["V146"] == DBNull.Value ? string.Empty : (string)reader["V146"];
                            temp.V147 = reader["V147"] == DBNull.Value ? string.Empty : (string)reader["V147"];
                            temp.V148 = reader["V148"] == DBNull.Value ? string.Empty : (string)reader["V148"];
                            temp.V149 = reader["V149"] == DBNull.Value ? string.Empty : (string)reader["V149"];
                            temp.V150 = reader["V150"] == DBNull.Value ? string.Empty : (string)reader["V150"];

                            temp.V151 = reader["V151"] == DBNull.Value ? string.Empty : (string)reader["V151"];
                            temp.V152 = reader["V152"] == DBNull.Value ? string.Empty : (string)reader["V152"];
                            temp.V153 = reader["V153"] == DBNull.Value ? string.Empty : (string)reader["V153"];
                            temp.V154 = reader["V154"] == DBNull.Value ? string.Empty : (string)reader["V154"];
                            temp.V155 = reader["V155"] == DBNull.Value ? string.Empty : (string)reader["V155"];
                            temp.V156 = reader["V156"] == DBNull.Value ? string.Empty : (string)reader["V156"];
                            temp.V157 = reader["V157"] == DBNull.Value ? string.Empty : (string)reader["V157"];
                            temp.V158 = reader["V158"] == DBNull.Value ? string.Empty : (string)reader["V158"];
                            temp.V159 = reader["V159"] == DBNull.Value ? string.Empty : (string)reader["V159"];
                            temp.V160 = reader["V160"] == DBNull.Value ? string.Empty : (string)reader["V160"];
                            temp.V161 = reader["V161"] == DBNull.Value ? string.Empty : (string)reader["V161"];
                            temp.V162 = reader["V162"] == DBNull.Value ? string.Empty : (string)reader["V162"];
                            temp.V163 = reader["V163"] == DBNull.Value ? string.Empty : (string)reader["V163"];
                            temp.V164 = reader["V164"] == DBNull.Value ? string.Empty : (string)reader["V164"];
                            temp.V165 = reader["V165"] == DBNull.Value ? string.Empty : (string)reader["V165"];
                            temp.V166 = reader["V166"] == DBNull.Value ? string.Empty : (string)reader["V166"];
                            temp.V167 = reader["V167"] == DBNull.Value ? string.Empty : (string)reader["V167"];
                            temp.V168 = reader["V168"] == DBNull.Value ? string.Empty : (string)reader["V168"];
                            temp.V169 = reader["V169"] == DBNull.Value ? string.Empty : (string)reader["V169"];
                            temp.V170 = reader["V170"] == DBNull.Value ? string.Empty : (string)reader["V170"];
                            temp.V171 = reader["V171"] == DBNull.Value ? string.Empty : (string)reader["V171"];
                            temp.V172 = reader["V172"] == DBNull.Value ? string.Empty : (string)reader["V172"];
                            temp.V173 = reader["V173"] == DBNull.Value ? string.Empty : (string)reader["V173"];
                            temp.V174 = reader["V174"] == DBNull.Value ? string.Empty : (string)reader["V174"];
                            temp.V175 = reader["V175"] == DBNull.Value ? string.Empty : (string)reader["V175"];
                            temp.V176 = reader["V176"] == DBNull.Value ? string.Empty : (string)reader["V176"];
                            temp.V177 = reader["V177"] == DBNull.Value ? string.Empty : (string)reader["V177"];
                            temp.V178 = reader["V178"] == DBNull.Value ? string.Empty : (string)reader["V178"];
                            temp.V179 = reader["V179"] == DBNull.Value ? string.Empty : (string)reader["V179"];
                            temp.V180 = reader["V180"] == DBNull.Value ? string.Empty : (string)reader["V180"];
                            temp.V181 = reader["V181"] == DBNull.Value ? string.Empty : (string)reader["V181"];
                            temp.V182 = reader["V182"] == DBNull.Value ? string.Empty : (string)reader["V182"];
                            temp.V183 = reader["V183"] == DBNull.Value ? string.Empty : (string)reader["V183"];
                            temp.V184 = reader["V184"] == DBNull.Value ? string.Empty : (string)reader["V184"];
                            temp.V185 = reader["V185"] == DBNull.Value ? string.Empty : (string)reader["V185"];
                            temp.V186 = reader["V186"] == DBNull.Value ? string.Empty : (string)reader["V186"];
                            temp.V187 = reader["V187"] == DBNull.Value ? string.Empty : (string)reader["V187"];
                            temp.V188 = reader["V188"] == DBNull.Value ? string.Empty : (string)reader["V188"];
                            temp.V189 = reader["V189"] == DBNull.Value ? string.Empty : (string)reader["V189"];
                            temp.V190 = reader["V190"] == DBNull.Value ? string.Empty : (string)reader["V190"];
                            temp.V191 = reader["V191"] == DBNull.Value ? string.Empty : (string)reader["V191"];
                            temp.V192 = reader["V192"] == DBNull.Value ? string.Empty : (string)reader["V192"];
                            temp.V193 = reader["V193"] == DBNull.Value ? string.Empty : (string)reader["V193"];
                            temp.V194 = reader["V194"] == DBNull.Value ? string.Empty : (string)reader["V194"];
                            temp.V195 = reader["V195"] == DBNull.Value ? string.Empty : (string)reader["V195"];
                            temp.V196 = reader["V196"] == DBNull.Value ? string.Empty : (string)reader["V196"];
                            temp.V197 = reader["V197"] == DBNull.Value ? string.Empty : (string)reader["V197"];
                            temp.V198 = reader["V198"] == DBNull.Value ? string.Empty : (string)reader["V198"];
                            temp.V199 = reader["V199"] == DBNull.Value ? string.Empty : (string)reader["V199"];
                            temp.V200 = reader["V200"] == DBNull.Value ? string.Empty : (string)reader["V200"];



                            temp.V201 = reader["V201"] == DBNull.Value ? string.Empty : (string)reader["V201"];
                            temp.V202 = reader["V202"] == DBNull.Value ? string.Empty : (string)reader["V202"];
                            temp.V203 = reader["V203"] == DBNull.Value ? string.Empty : (string)reader["V203"];
                            temp.V204 = reader["V204"] == DBNull.Value ? string.Empty : (string)reader["V204"];
                            temp.V205 = reader["V205"] == DBNull.Value ? string.Empty : (string)reader["V205"];
                            temp.V206 = reader["V206"] == DBNull.Value ? string.Empty : (string)reader["V206"];
                            temp.V207 = reader["V207"] == DBNull.Value ? string.Empty : (string)reader["V207"];
                            temp.V208 = reader["V208"] == DBNull.Value ? string.Empty : (string)reader["V208"];
                            temp.V209 = reader["V209"] == DBNull.Value ? string.Empty : (string)reader["V209"];
                            temp.V210 = reader["V210"] == DBNull.Value ? string.Empty : (string)reader["V210"];
                            temp.V211 = reader["V211"] == DBNull.Value ? string.Empty : (string)reader["V211"];
                            temp.V212 = reader["V212"] == DBNull.Value ? string.Empty : (string)reader["V212"];
                            temp.V213 = reader["V213"] == DBNull.Value ? string.Empty : (string)reader["V213"];
                            temp.V214 = reader["V214"] == DBNull.Value ? string.Empty : (string)reader["V214"];
                            temp.V215 = reader["V215"] == DBNull.Value ? string.Empty : (string)reader["V215"];
                            temp.V216 = reader["V216"] == DBNull.Value ? string.Empty : (string)reader["V216"];
                            temp.V217 = reader["V217"] == DBNull.Value ? string.Empty : (string)reader["V217"];
                            temp.V218 = reader["V218"] == DBNull.Value ? string.Empty : (string)reader["V218"];
                            temp.V219 = reader["V219"] == DBNull.Value ? string.Empty : (string)reader["V219"];
                            temp.V220 = reader["V220"] == DBNull.Value ? string.Empty : (string)reader["V220"];
                            temp.V221 = reader["V221"] == DBNull.Value ? string.Empty : (string)reader["V221"];
                            temp.V222 = reader["V222"] == DBNull.Value ? string.Empty : (string)reader["V222"];
                            temp.V223 = reader["V223"] == DBNull.Value ? string.Empty : (string)reader["V223"];
                            temp.V224 = reader["V224"] == DBNull.Value ? string.Empty : (string)reader["V224"];
                            temp.V225 = reader["V225"] == DBNull.Value ? string.Empty : (string)reader["V225"];
                            temp.V226 = reader["V226"] == DBNull.Value ? string.Empty : (string)reader["V226"];
                            temp.V227 = reader["V227"] == DBNull.Value ? string.Empty : (string)reader["V227"];
                            temp.V228 = reader["V228"] == DBNull.Value ? string.Empty : (string)reader["V228"];
                            temp.V229 = reader["V229"] == DBNull.Value ? string.Empty : (string)reader["V229"];
                            temp.V230 = reader["V230"] == DBNull.Value ? string.Empty : (string)reader["V230"];
                            temp.V231 = reader["V231"] == DBNull.Value ? string.Empty : (string)reader["V231"];
                            temp.V232 = reader["V232"] == DBNull.Value ? string.Empty : (string)reader["V232"];
                            temp.V233 = reader["V233"] == DBNull.Value ? string.Empty : (string)reader["V233"];
                            temp.V234 = reader["V234"] == DBNull.Value ? string.Empty : (string)reader["V234"];
                            temp.V235 = reader["V235"] == DBNull.Value ? string.Empty : (string)reader["V235"];
                            temp.V236 = reader["V236"] == DBNull.Value ? string.Empty : (string)reader["V236"];
                            temp.V237 = reader["V237"] == DBNull.Value ? string.Empty : (string)reader["V237"];
                            temp.V238 = reader["V238"] == DBNull.Value ? string.Empty : (string)reader["V238"];
                            temp.V239 = reader["V239"] == DBNull.Value ? string.Empty : (string)reader["V239"];
                            temp.V240 = reader["V240"] == DBNull.Value ? string.Empty : (string)reader["V240"];
                            temp.V241 = reader["V241"] == DBNull.Value ? string.Empty : (string)reader["V241"];
                            temp.V242 = reader["V242"] == DBNull.Value ? string.Empty : (string)reader["V242"];
                            temp.V243 = reader["V243"] == DBNull.Value ? string.Empty : (string)reader["V243"];
                            temp.V244 = reader["V244"] == DBNull.Value ? string.Empty : (string)reader["V244"];
                            temp.V245 = reader["V245"] == DBNull.Value ? string.Empty : (string)reader["V245"];
                            temp.V246 = reader["V246"] == DBNull.Value ? string.Empty : (string)reader["V246"];
                            temp.V247 = reader["V247"] == DBNull.Value ? string.Empty : (string)reader["V247"];
                            temp.V248 = reader["V248"] == DBNull.Value ? string.Empty : (string)reader["V248"];
                            temp.V249 = reader["V249"] == DBNull.Value ? string.Empty : (string)reader["V249"];
                            temp.V250 = reader["V250"] == DBNull.Value ? string.Empty : (string)reader["V250"];

                            temp.V251 = reader["V251"] == DBNull.Value ? string.Empty : (string)reader["V251"];
                            temp.V252 = reader["V252"] == DBNull.Value ? string.Empty : (string)reader["V252"];
                            temp.V253 = reader["V253"] == DBNull.Value ? string.Empty : (string)reader["V253"];
                            temp.V254 = reader["V254"] == DBNull.Value ? string.Empty : (string)reader["V254"];
                            temp.V255 = reader["V255"] == DBNull.Value ? string.Empty : (string)reader["V255"];
                            temp.V256 = reader["V256"] == DBNull.Value ? string.Empty : (string)reader["V256"];
                            temp.V257 = reader["V257"] == DBNull.Value ? string.Empty : (string)reader["V257"];
                            temp.V258 = reader["V258"] == DBNull.Value ? string.Empty : (string)reader["V258"];
                            temp.V259 = reader["V259"] == DBNull.Value ? string.Empty : (string)reader["V259"];
                            temp.V260 = reader["V260"] == DBNull.Value ? string.Empty : (string)reader["V260"];
                            temp.V261 = reader["V261"] == DBNull.Value ? string.Empty : (string)reader["V261"];
                            temp.V262 = reader["V262"] == DBNull.Value ? string.Empty : (string)reader["V262"];
                            temp.V263 = reader["V263"] == DBNull.Value ? string.Empty : (string)reader["V263"];
                            temp.V264 = reader["V264"] == DBNull.Value ? string.Empty : (string)reader["V264"];
                            temp.V265 = reader["V265"] == DBNull.Value ? string.Empty : (string)reader["V265"];
                            temp.V266 = reader["V266"] == DBNull.Value ? string.Empty : (string)reader["V266"];
                            temp.V267 = reader["V267"] == DBNull.Value ? string.Empty : (string)reader["V267"];
                            temp.V268 = reader["V268"] == DBNull.Value ? string.Empty : (string)reader["V268"];
                            temp.V269 = reader["V269"] == DBNull.Value ? string.Empty : (string)reader["V269"];
                            temp.V270 = reader["V270"] == DBNull.Value ? string.Empty : (string)reader["V270"];
                            temp.V271 = reader["V271"] == DBNull.Value ? string.Empty : (string)reader["V271"];
                            temp.V272 = reader["V272"] == DBNull.Value ? string.Empty : (string)reader["V272"];
                            temp.V273 = reader["V273"] == DBNull.Value ? string.Empty : (string)reader["V273"];
                            temp.V274 = reader["V274"] == DBNull.Value ? string.Empty : (string)reader["V274"];
                            temp.V275 = reader["V275"] == DBNull.Value ? string.Empty : (string)reader["V275"];
                            temp.V276 = reader["V276"] == DBNull.Value ? string.Empty : (string)reader["V276"];
                            temp.V277 = reader["V277"] == DBNull.Value ? string.Empty : (string)reader["V277"];
                            temp.V278 = reader["V278"] == DBNull.Value ? string.Empty : (string)reader["V278"];
                            temp.V279 = reader["V279"] == DBNull.Value ? string.Empty : (string)reader["V279"];
                            temp.V280 = reader["V280"] == DBNull.Value ? string.Empty : (string)reader["V280"];
                            temp.V281 = reader["V281"] == DBNull.Value ? string.Empty : (string)reader["V281"];
                            temp.V282 = reader["V282"] == DBNull.Value ? string.Empty : (string)reader["V282"];
                            temp.V283 = reader["V283"] == DBNull.Value ? string.Empty : (string)reader["V283"];
                            temp.V284 = reader["V284"] == DBNull.Value ? string.Empty : (string)reader["V284"];
                            temp.V285 = reader["V285"] == DBNull.Value ? string.Empty : (string)reader["V285"];
                            temp.V286 = reader["V286"] == DBNull.Value ? string.Empty : (string)reader["V286"];
                            temp.V287 = reader["V287"] == DBNull.Value ? string.Empty : (string)reader["V287"];
                            temp.V288 = reader["V288"] == DBNull.Value ? string.Empty : (string)reader["V288"];
                            temp.V289 = reader["V289"] == DBNull.Value ? string.Empty : (string)reader["V289"];
                            temp.V290 = reader["V290"] == DBNull.Value ? string.Empty : (string)reader["V290"];
                            temp.V291 = reader["V291"] == DBNull.Value ? string.Empty : (string)reader["V291"];
                            temp.V292 = reader["V292"] == DBNull.Value ? string.Empty : (string)reader["V292"];
                            temp.V293 = reader["V293"] == DBNull.Value ? string.Empty : (string)reader["V293"];
                            temp.V294 = reader["V294"] == DBNull.Value ? string.Empty : (string)reader["V294"];
                            temp.V295 = reader["V295"] == DBNull.Value ? string.Empty : (string)reader["V295"];
                            temp.V296 = reader["V296"] == DBNull.Value ? string.Empty : (string)reader["V296"];
                            temp.V297 = reader["V297"] == DBNull.Value ? string.Empty : (string)reader["V297"];
                            temp.V298 = reader["V298"] == DBNull.Value ? string.Empty : (string)reader["V298"];
                            temp.V299 = reader["V299"] == DBNull.Value ? string.Empty : (string)reader["V299"];
                            temp.V300 = reader["V300"] == DBNull.Value ? string.Empty : (string)reader["V300"];




                            temp.V301 = reader["V301"] == DBNull.Value ? string.Empty : (string)reader["V301"];
                            temp.V302 = reader["V302"] == DBNull.Value ? string.Empty : (string)reader["V302"];
                            temp.V303 = reader["V303"] == DBNull.Value ? string.Empty : (string)reader["V303"];
                            temp.V304 = reader["V304"] == DBNull.Value ? string.Empty : (string)reader["V304"];
                            temp.V305 = reader["V305"] == DBNull.Value ? string.Empty : (string)reader["V305"];
                            temp.V306 = reader["V306"] == DBNull.Value ? string.Empty : (string)reader["V306"];
                            temp.V307 = reader["V307"] == DBNull.Value ? string.Empty : (string)reader["V307"];
                            temp.V308 = reader["V308"] == DBNull.Value ? string.Empty : (string)reader["V308"];
                            temp.V309 = reader["V309"] == DBNull.Value ? string.Empty : (string)reader["V309"];
                            temp.V310 = reader["V310"] == DBNull.Value ? string.Empty : (string)reader["V310"];
                            temp.V311 = reader["V311"] == DBNull.Value ? string.Empty : (string)reader["V311"];
                            temp.V312 = reader["V312"] == DBNull.Value ? string.Empty : (string)reader["V312"];
                            temp.V313 = reader["V313"] == DBNull.Value ? string.Empty : (string)reader["V313"];
                            temp.V314 = reader["V314"] == DBNull.Value ? string.Empty : (string)reader["V314"];
                            temp.V315 = reader["V315"] == DBNull.Value ? string.Empty : (string)reader["V315"];
                            temp.V316 = reader["V316"] == DBNull.Value ? string.Empty : (string)reader["V316"];
                            temp.V317 = reader["V317"] == DBNull.Value ? string.Empty : (string)reader["V317"];
                            temp.V318 = reader["V318"] == DBNull.Value ? string.Empty : (string)reader["V318"];
                            temp.V319 = reader["V319"] == DBNull.Value ? string.Empty : (string)reader["V319"];
                            temp.V320 = reader["V320"] == DBNull.Value ? string.Empty : (string)reader["V320"];
                            temp.V321 = reader["V321"] == DBNull.Value ? string.Empty : (string)reader["V321"];
                            temp.V322 = reader["V322"] == DBNull.Value ? string.Empty : (string)reader["V322"];
                            temp.V323 = reader["V323"] == DBNull.Value ? string.Empty : (string)reader["V323"];
                            temp.V324 = reader["V324"] == DBNull.Value ? string.Empty : (string)reader["V324"];
                            temp.V325 = reader["V325"] == DBNull.Value ? string.Empty : (string)reader["V325"];
                            temp.V326 = reader["V326"] == DBNull.Value ? string.Empty : (string)reader["V326"];
                            temp.V327 = reader["V327"] == DBNull.Value ? string.Empty : (string)reader["V327"];
                            temp.V328 = reader["V328"] == DBNull.Value ? string.Empty : (string)reader["V328"];
                            temp.V329 = reader["V329"] == DBNull.Value ? string.Empty : (string)reader["V329"];
                            temp.V330 = reader["V330"] == DBNull.Value ? string.Empty : (string)reader["V330"];
                            temp.V331 = reader["V331"] == DBNull.Value ? string.Empty : (string)reader["V331"];
                            temp.V332 = reader["V332"] == DBNull.Value ? string.Empty : (string)reader["V332"];
                            temp.V333 = reader["V333"] == DBNull.Value ? string.Empty : (string)reader["V333"];
                            temp.V334 = reader["V334"] == DBNull.Value ? string.Empty : (string)reader["V334"];
                            temp.V335 = reader["V335"] == DBNull.Value ? string.Empty : (string)reader["V335"];
                            temp.V336 = reader["V336"] == DBNull.Value ? string.Empty : (string)reader["V336"];
                            temp.V337 = reader["V337"] == DBNull.Value ? string.Empty : (string)reader["V337"];
                            temp.V338 = reader["V338"] == DBNull.Value ? string.Empty : (string)reader["V338"];
                            temp.V339 = reader["V339"] == DBNull.Value ? string.Empty : (string)reader["V339"];
                            temp.V340 = reader["V340"] == DBNull.Value ? string.Empty : (string)reader["V340"];
                            temp.V341 = reader["V341"] == DBNull.Value ? string.Empty : (string)reader["V341"];
                            temp.V342 = reader["V342"] == DBNull.Value ? string.Empty : (string)reader["V342"];
                            temp.V343 = reader["V343"] == DBNull.Value ? string.Empty : (string)reader["V343"];
                            temp.V344 = reader["V344"] == DBNull.Value ? string.Empty : (string)reader["V344"];
                            temp.V345 = reader["V345"] == DBNull.Value ? string.Empty : (string)reader["V345"];
                            temp.V346 = reader["V346"] == DBNull.Value ? string.Empty : (string)reader["V346"];
                            temp.V347 = reader["V347"] == DBNull.Value ? string.Empty : (string)reader["V347"];
                            temp.V348 = reader["V348"] == DBNull.Value ? string.Empty : (string)reader["V348"];
                            temp.V349 = reader["V349"] == DBNull.Value ? string.Empty : (string)reader["V349"];
                            temp.V350 = reader["V350"] == DBNull.Value ? string.Empty : (string)reader["V350"];

                            temp.V351 = reader["V351"] == DBNull.Value ? string.Empty : (string)reader["V351"];
                            temp.V352 = reader["V352"] == DBNull.Value ? string.Empty : (string)reader["V352"];
                            temp.V353 = reader["V353"] == DBNull.Value ? string.Empty : (string)reader["V353"];
                            temp.V354 = reader["V354"] == DBNull.Value ? string.Empty : (string)reader["V354"];
                            temp.V355 = reader["V355"] == DBNull.Value ? string.Empty : (string)reader["V355"];
                            temp.V356 = reader["V356"] == DBNull.Value ? string.Empty : (string)reader["V356"];
                            temp.V357 = reader["V357"] == DBNull.Value ? string.Empty : (string)reader["V357"];
                            temp.V358 = reader["V358"] == DBNull.Value ? string.Empty : (string)reader["V358"];
                            temp.V359 = reader["V359"] == DBNull.Value ? string.Empty : (string)reader["V359"];
                            temp.V360 = reader["V360"] == DBNull.Value ? string.Empty : (string)reader["V360"];
                            temp.V361 = reader["V361"] == DBNull.Value ? string.Empty : (string)reader["V361"];
                            temp.V362 = reader["V362"] == DBNull.Value ? string.Empty : (string)reader["V362"];
                            temp.V363 = reader["V363"] == DBNull.Value ? string.Empty : (string)reader["V363"];
                            temp.V364 = reader["V364"] == DBNull.Value ? string.Empty : (string)reader["V364"];
                            temp.V365 = reader["V365"] == DBNull.Value ? string.Empty : (string)reader["V365"];
                            temp.V366 = reader["V366"] == DBNull.Value ? string.Empty : (string)reader["V366"];
                            temp.V367 = reader["V367"] == DBNull.Value ? string.Empty : (string)reader["V367"];
                            temp.V368 = reader["V368"] == DBNull.Value ? string.Empty : (string)reader["V368"];
                            temp.V369 = reader["V369"] == DBNull.Value ? string.Empty : (string)reader["V369"];
                            temp.V370 = reader["V370"] == DBNull.Value ? string.Empty : (string)reader["V370"];
                            temp.V371 = reader["V371"] == DBNull.Value ? string.Empty : (string)reader["V371"];
                            temp.V372 = reader["V372"] == DBNull.Value ? string.Empty : (string)reader["V372"];
                            temp.V373 = reader["V373"] == DBNull.Value ? string.Empty : (string)reader["V373"];
                            temp.V374 = reader["V374"] == DBNull.Value ? string.Empty : (string)reader["V374"];
                            temp.V375 = reader["V375"] == DBNull.Value ? string.Empty : (string)reader["V375"];
                            temp.V376 = reader["V376"] == DBNull.Value ? string.Empty : (string)reader["V376"];
                            temp.V377 = reader["V377"] == DBNull.Value ? string.Empty : (string)reader["V377"];
                            temp.V378 = reader["V378"] == DBNull.Value ? string.Empty : (string)reader["V378"];
                            temp.V379 = reader["V379"] == DBNull.Value ? string.Empty : (string)reader["V379"];
                            temp.V380 = reader["V380"] == DBNull.Value ? string.Empty : (string)reader["V380"];
                            temp.V381 = reader["V381"] == DBNull.Value ? string.Empty : (string)reader["V381"];
                            temp.V382 = reader["V382"] == DBNull.Value ? string.Empty : (string)reader["V382"];
                            temp.V383 = reader["V383"] == DBNull.Value ? string.Empty : (string)reader["V383"];
                            temp.V384 = reader["V384"] == DBNull.Value ? string.Empty : (string)reader["V384"];
                            temp.V385 = reader["V385"] == DBNull.Value ? string.Empty : (string)reader["V385"];
                            temp.V386 = reader["V386"] == DBNull.Value ? string.Empty : (string)reader["V386"];
                            temp.V387 = reader["V387"] == DBNull.Value ? string.Empty : (string)reader["V387"];
                            temp.V388 = reader["V388"] == DBNull.Value ? string.Empty : (string)reader["V388"];
                            temp.V389 = reader["V389"] == DBNull.Value ? string.Empty : (string)reader["V389"];
                            temp.V390 = reader["V390"] == DBNull.Value ? string.Empty : (string)reader["V390"];
                            temp.V391 = reader["V391"] == DBNull.Value ? string.Empty : (string)reader["V391"];
                            temp.V392 = reader["V392"] == DBNull.Value ? string.Empty : (string)reader["V392"];
                            temp.V393 = reader["V393"] == DBNull.Value ? string.Empty : (string)reader["V393"];
                            temp.V394 = reader["V394"] == DBNull.Value ? string.Empty : (string)reader["V394"];
                            temp.V395 = reader["V395"] == DBNull.Value ? string.Empty : (string)reader["V395"];
                            temp.V396 = reader["V396"] == DBNull.Value ? string.Empty : (string)reader["V396"];
                            temp.V397 = reader["V397"] == DBNull.Value ? string.Empty : (string)reader["V397"];
                            temp.V398 = reader["V398"] == DBNull.Value ? string.Empty : (string)reader["V398"];
                            temp.V399 = reader["V399"] == DBNull.Value ? string.Empty : (string)reader["V399"];
                            temp.V400 = reader["V400"] == DBNull.Value ? string.Empty : (string)reader["V400"];



                            temp.V401 = reader["V401"] == DBNull.Value ? string.Empty : (string)reader["V401"];
                            temp.V402 = reader["V402"] == DBNull.Value ? string.Empty : (string)reader["V402"];
                            temp.V403 = reader["V403"] == DBNull.Value ? string.Empty : (string)reader["V403"];
                            temp.V404 = reader["V404"] == DBNull.Value ? string.Empty : (string)reader["V404"];
                            temp.V405 = reader["V405"] == DBNull.Value ? string.Empty : (string)reader["V405"];
                            temp.V406 = reader["V406"] == DBNull.Value ? string.Empty : (string)reader["V406"];
                            temp.V407 = reader["V407"] == DBNull.Value ? string.Empty : (string)reader["V407"];
                            temp.V408 = reader["V408"] == DBNull.Value ? string.Empty : (string)reader["V408"];
                            temp.V409 = reader["V409"] == DBNull.Value ? string.Empty : (string)reader["V409"];
                            temp.V410 = reader["V410"] == DBNull.Value ? string.Empty : (string)reader["V410"];
                            temp.V411 = reader["V411"] == DBNull.Value ? string.Empty : (string)reader["V411"];
                            temp.V412 = reader["V412"] == DBNull.Value ? string.Empty : (string)reader["V412"];
                            temp.V413 = reader["V413"] == DBNull.Value ? string.Empty : (string)reader["V413"];
                            temp.V414 = reader["V414"] == DBNull.Value ? string.Empty : (string)reader["V414"];
                            temp.V415 = reader["V415"] == DBNull.Value ? string.Empty : (string)reader["V415"];
                            temp.V416 = reader["V416"] == DBNull.Value ? string.Empty : (string)reader["V416"];
                            temp.V417 = reader["V417"] == DBNull.Value ? string.Empty : (string)reader["V417"];
                            temp.V418 = reader["V418"] == DBNull.Value ? string.Empty : (string)reader["V418"];
                            temp.V419 = reader["V419"] == DBNull.Value ? string.Empty : (string)reader["V419"];
                            temp.V420 = reader["V420"] == DBNull.Value ? string.Empty : (string)reader["V420"];
                            temp.V421 = reader["V421"] == DBNull.Value ? string.Empty : (string)reader["V421"];
                            temp.V422 = reader["V422"] == DBNull.Value ? string.Empty : (string)reader["V422"];
                            temp.V423 = reader["V423"] == DBNull.Value ? string.Empty : (string)reader["V423"];
                            temp.V424 = reader["V424"] == DBNull.Value ? string.Empty : (string)reader["V424"];
                            temp.V425 = reader["V425"] == DBNull.Value ? string.Empty : (string)reader["V425"];
                            temp.V426 = reader["V426"] == DBNull.Value ? string.Empty : (string)reader["V426"];
                            temp.V427 = reader["V427"] == DBNull.Value ? string.Empty : (string)reader["V427"];
                            temp.V428 = reader["V428"] == DBNull.Value ? string.Empty : (string)reader["V428"];
                            temp.V429 = reader["V429"] == DBNull.Value ? string.Empty : (string)reader["V429"];
                            temp.V430 = reader["V430"] == DBNull.Value ? string.Empty : (string)reader["V430"];
                            temp.V431 = reader["V431"] == DBNull.Value ? string.Empty : (string)reader["V431"];
                            temp.V432 = reader["V432"] == DBNull.Value ? string.Empty : (string)reader["V432"];
                            temp.V433 = reader["V433"] == DBNull.Value ? string.Empty : (string)reader["V433"];
                            temp.V434 = reader["V434"] == DBNull.Value ? string.Empty : (string)reader["V434"];
                            temp.V435 = reader["V435"] == DBNull.Value ? string.Empty : (string)reader["V435"];
                            temp.V436 = reader["V436"] == DBNull.Value ? string.Empty : (string)reader["V436"];
                            temp.V437 = reader["V437"] == DBNull.Value ? string.Empty : (string)reader["V437"];
                            temp.V438 = reader["V438"] == DBNull.Value ? string.Empty : (string)reader["V438"];
                            temp.V439 = reader["V439"] == DBNull.Value ? string.Empty : (string)reader["V439"];
                            temp.V440 = reader["V440"] == DBNull.Value ? string.Empty : (string)reader["V440"];
                            temp.V441 = reader["V441"] == DBNull.Value ? string.Empty : (string)reader["V441"];
                            temp.V442 = reader["V442"] == DBNull.Value ? string.Empty : (string)reader["V442"];
                            temp.V443 = reader["V443"] == DBNull.Value ? string.Empty : (string)reader["V443"];
                            temp.V444 = reader["V444"] == DBNull.Value ? string.Empty : (string)reader["V444"];
                            temp.V445 = reader["V445"] == DBNull.Value ? string.Empty : (string)reader["V445"];
                            temp.V446 = reader["V446"] == DBNull.Value ? string.Empty : (string)reader["V446"];
                            temp.V447 = reader["V447"] == DBNull.Value ? string.Empty : (string)reader["V447"];
                            temp.V448 = reader["V448"] == DBNull.Value ? string.Empty : (string)reader["V448"];
                            temp.V449 = reader["V449"] == DBNull.Value ? string.Empty : (string)reader["V449"];
                            temp.V450 = reader["V450"] == DBNull.Value ? string.Empty : (string)reader["V450"];

                            temp.V451 = reader["V451"] == DBNull.Value ? string.Empty : (string)reader["V451"];
                            temp.V452 = reader["V452"] == DBNull.Value ? string.Empty : (string)reader["V452"];
                            temp.V453 = reader["V453"] == DBNull.Value ? string.Empty : (string)reader["V453"];
                            temp.V454 = reader["V454"] == DBNull.Value ? string.Empty : (string)reader["V454"];
                            temp.V455 = reader["V455"] == DBNull.Value ? string.Empty : (string)reader["V455"];
                            temp.V456 = reader["V456"] == DBNull.Value ? string.Empty : (string)reader["V456"];
                            temp.V457 = reader["V457"] == DBNull.Value ? string.Empty : (string)reader["V457"];
                            temp.V458 = reader["V458"] == DBNull.Value ? string.Empty : (string)reader["V458"];
                            temp.V459 = reader["V459"] == DBNull.Value ? string.Empty : (string)reader["V459"];
                            temp.V460 = reader["V460"] == DBNull.Value ? string.Empty : (string)reader["V460"];
                            temp.V461 = reader["V461"] == DBNull.Value ? string.Empty : (string)reader["V461"];
                            temp.V462 = reader["V462"] == DBNull.Value ? string.Empty : (string)reader["V462"];
                            temp.V463 = reader["V463"] == DBNull.Value ? string.Empty : (string)reader["V463"];
                            temp.V464 = reader["V464"] == DBNull.Value ? string.Empty : (string)reader["V464"];
                            temp.V465 = reader["V465"] == DBNull.Value ? string.Empty : (string)reader["V465"];
                            temp.V466 = reader["V466"] == DBNull.Value ? string.Empty : (string)reader["V466"];
                            temp.V467 = reader["V467"] == DBNull.Value ? string.Empty : (string)reader["V467"];
                            temp.V468 = reader["V468"] == DBNull.Value ? string.Empty : (string)reader["V468"];
                            temp.V469 = reader["V469"] == DBNull.Value ? string.Empty : (string)reader["V469"];
                            temp.V470 = reader["V470"] == DBNull.Value ? string.Empty : (string)reader["V470"];
                            temp.V471 = reader["V471"] == DBNull.Value ? string.Empty : (string)reader["V471"];
                            temp.V472 = reader["V472"] == DBNull.Value ? string.Empty : (string)reader["V472"];
                            temp.V473 = reader["V473"] == DBNull.Value ? string.Empty : (string)reader["V473"];
                            temp.V474 = reader["V474"] == DBNull.Value ? string.Empty : (string)reader["V474"];
                            temp.V475 = reader["V475"] == DBNull.Value ? string.Empty : (string)reader["V475"];
                            temp.V476 = reader["V476"] == DBNull.Value ? string.Empty : (string)reader["V476"];
                            temp.V477 = reader["V477"] == DBNull.Value ? string.Empty : (string)reader["V477"];
                            temp.V478 = reader["V478"] == DBNull.Value ? string.Empty : (string)reader["V478"];
                            temp.V479 = reader["V479"] == DBNull.Value ? string.Empty : (string)reader["V479"];
                            temp.V480 = reader["V480"] == DBNull.Value ? string.Empty : (string)reader["V480"];
                            temp.V481 = reader["V481"] == DBNull.Value ? string.Empty : (string)reader["V481"];
                            temp.V482 = reader["V482"] == DBNull.Value ? string.Empty : (string)reader["V482"];
                            temp.V483 = reader["V483"] == DBNull.Value ? string.Empty : (string)reader["V483"];
                            temp.V484 = reader["V484"] == DBNull.Value ? string.Empty : (string)reader["V484"];
                            temp.V485 = reader["V485"] == DBNull.Value ? string.Empty : (string)reader["V485"];
                            temp.V486 = reader["V486"] == DBNull.Value ? string.Empty : (string)reader["V486"];
                            temp.V487 = reader["V487"] == DBNull.Value ? string.Empty : (string)reader["V487"];
                            temp.V488 = reader["V488"] == DBNull.Value ? string.Empty : (string)reader["V488"];
                            temp.V489 = reader["V489"] == DBNull.Value ? string.Empty : (string)reader["V489"];
                            temp.V490 = reader["V490"] == DBNull.Value ? string.Empty : (string)reader["V490"];
                            temp.V491 = reader["V491"] == DBNull.Value ? string.Empty : (string)reader["V491"];
                            temp.V492 = reader["V492"] == DBNull.Value ? string.Empty : (string)reader["V492"];
                            temp.V493 = reader["V493"] == DBNull.Value ? string.Empty : (string)reader["V493"];
                            temp.V494 = reader["V494"] == DBNull.Value ? string.Empty : (string)reader["V494"];
                            temp.V495 = reader["V495"] == DBNull.Value ? string.Empty : (string)reader["V495"];
                            temp.V496 = reader["V496"] == DBNull.Value ? string.Empty : (string)reader["V496"];
                            temp.V497 = reader["V497"] == DBNull.Value ? string.Empty : (string)reader["V497"];
                            temp.V498 = reader["V498"] == DBNull.Value ? string.Empty : (string)reader["V498"];
                            temp.V499 = reader["V499"] == DBNull.Value ? string.Empty : (string)reader["V499"];
                            temp.V500 = reader["V500"] == DBNull.Value ? string.Empty : (string)reader["V500"];




                            temp.ValidationResults = reader["ValidationResults"] == DBNull.Value ? string.Empty : (string)reader["ValidationResults"];
                            temp.WarningResults = reader["WarningResults"] == DBNull.Value ? string.Empty : (string)reader["WarningResults"];
                            temp.LastUpdatedUserID = reader["LastUpdatedUserID"] == DBNull.Value ? null : (int?)reader["LastUpdatedUserID"];

                            temp.ChangeReason = reader["ChangeReason"] == DBNull.Value ? "" : (string)reader["ChangeReason"];
                            temp.OwnerUserID = reader["OwnerUserID"] == DBNull.Value ? null : (int?)reader["OwnerUserID"];


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


    public static DataTable ets_Table_Columns_ForImportEmail(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_ForImportEmail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                
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

    public static DataTable ets_Table_Columns_Detail(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

               
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



    public static DataTable ets_Table_Columns_Detail_Default(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_Detail_Default", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                
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

    public static DataTable ets_Table_Columns_NotDetail(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_NotDetail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                
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


    public static DataTable ets_Table_Columns_NotDetail_Default(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_NotDetail_Default", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                
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

    public static DataTable ets_Table_Columns_All(int nTableID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_All", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                
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



    public static DataTable ets_Table_Columns_Export(int nTableID)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_Export", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;               
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

               
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

    public static DataTable ets_Table_Columns_Summary(int nTableID, int? nViewID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_Summary", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if(nViewID!=null)
                    command.Parameters.Add(new SqlParameter("@nViewID", nViewID));

               
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



    public static DataTable ets_Table_Columns_Mobile(int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_Mobile", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                
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


    public static DataTable ets_Table_Columns_Summary_Export(int nTableID, int? nViewID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_Summary_Export", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if(nViewID!=null)
                    command.Parameters.Add(new SqlParameter("@nViewID", nViewID));


                
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


    public static DataTable ets_Table_Columns_Import(int nTableID, int? nImportTemplateID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_Import", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nImportTemplateID != null)
                    command.Parameters.Add(new SqlParameter("@nImportTemplateID", nImportTemplateID));

               
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


  


    //public static DataTable dbg_Table_Columns_ImportTemplate(int nTableID,int nImportTemplateID , ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("dbg_Table_Columns_ImportTemplate", connection, tn))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
    //        command.Parameters.Add(new SqlParameter("@nImportTemplateID", nImportTemplateID));

    //        //connection.Open();
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = command;
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
    //        return dt;

    //    }

    //}
    public static DataTable ets_Table_Columns_DisplayName(int nTableID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Table_Columns_DisplayName", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                //connection.Open();
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

    
    


    public static DataTable ets_Table_Import_Position(int nTableID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_Table_Import_Position", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                //connection.Open();
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

    public static string ets_Column_NextSystemName(int nTableID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Column_NextSystemName", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                //connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();

                connection.Open();
                try
                {
                    da.Fill(dt);
                    connection.Close();
                    connection.Dispose();
                    return dt.Rows[0][0].ToString();
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();
                }


                return "NO";

               

            }
        }

    }





    public static bool ets_Record_IsDuplicate(TempRecord p_Record)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_IsDuplicate", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                //command.Parameters.Add(new SqlParameter("@LocationID", p_Record.LocationID));
                command.Parameters.Add(new SqlParameter("@TableID", p_Record.TableID));
                command.Parameters.Add(new SqlParameter("@DateTimeRecorded", p_Record.DateTimeRecorded));
                //command.Parameters.Add(new SqlParameter("@EnteredBy", p_Record.EnteredBy));


                if (p_Record.V001 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V001", p_Record.V001));
                if (p_Record.V002 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V002", p_Record.V002));
                if (p_Record.V003 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V003", p_Record.V003));
                if (p_Record.V004 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V004", p_Record.V004));
                if (p_Record.V005 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V005", p_Record.V005));
                if (p_Record.V006 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V006", p_Record.V006));
                if (p_Record.V007 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V007", p_Record.V007));
                if (p_Record.V008 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V008", p_Record.V008));
                if (p_Record.V009 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V009", p_Record.V009));
                if (p_Record.V010 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V010", p_Record.V010));
                if (p_Record.V011 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V011", p_Record.V011));
                if (p_Record.V012 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V012", p_Record.V012));
                if (p_Record.V013 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V013", p_Record.V013));
                if (p_Record.V014 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V014", p_Record.V014));
                if (p_Record.V015 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V015", p_Record.V015));
                if (p_Record.V016 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V016", p_Record.V016));
                if (p_Record.V017 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V017", p_Record.V017));
                if (p_Record.V018 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V018", p_Record.V018));
                if (p_Record.V019 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V019", p_Record.V019));
                if (p_Record.V020 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V020", p_Record.V020));
                if (p_Record.V021 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V021", p_Record.V021));
                if (p_Record.V022 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V022", p_Record.V022));
                if (p_Record.V023 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V023", p_Record.V023));
                if (p_Record.V024 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V024", p_Record.V024));
                if (p_Record.V025 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V025", p_Record.V025));
                if (p_Record.V026 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V026", p_Record.V026));
                if (p_Record.V027 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V027", p_Record.V027));
                if (p_Record.V028 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V028", p_Record.V028));
                if (p_Record.V029 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V029", p_Record.V029));
                if (p_Record.V030 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V030", p_Record.V030));
                if (p_Record.V031 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V031", p_Record.V031));
                if (p_Record.V032 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V032", p_Record.V032));
                if (p_Record.V033 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V033", p_Record.V033));
                if (p_Record.V034 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V034", p_Record.V034));
                if (p_Record.V035 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V035", p_Record.V035));
                if (p_Record.V036 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V036", p_Record.V036));
                if (p_Record.V037 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V037", p_Record.V037));
                if (p_Record.V038 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V038", p_Record.V038));
                if (p_Record.V039 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V039", p_Record.V039));
                if (p_Record.V040 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V040", p_Record.V040));
                if (p_Record.V041 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V041", p_Record.V041));
                if (p_Record.V042 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V042", p_Record.V042));
                if (p_Record.V043 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V043", p_Record.V043));
                if (p_Record.V044 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V044", p_Record.V044));
                if (p_Record.V045 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V045", p_Record.V045));
                if (p_Record.V046 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V046", p_Record.V046));
                if (p_Record.V047 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V047", p_Record.V047));
                if (p_Record.V048 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V048", p_Record.V048));
                if (p_Record.V049 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V049", p_Record.V049));
                if (p_Record.V050 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V050", p_Record.V050));


                

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

             
                if (ds == null) return false;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }



            }
        }
    }


    //public static bool ets_Record_IsDuplicate(TempRecord p_Record, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_Record_IsDuplicate", connection, tn))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;


    //        //command.Parameters.Add(new SqlParameter("@LocationID", p_Record.LocationID));
    //        command.Parameters.Add(new SqlParameter("@TableID", p_Record.TableID));
    //        command.Parameters.Add(new SqlParameter("@DateTimeRecorded", p_Record.DateTimeRecorded));
    //        //command.Parameters.Add(new SqlParameter("@EnteredBy", p_Record.EnteredBy));


    //        if (p_Record.V001 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V001", p_Record.V001));
    //        if (p_Record.V002 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V002", p_Record.V002));
    //        if (p_Record.V003 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V003", p_Record.V003));
    //        if (p_Record.V004 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V004", p_Record.V004));
    //        if (p_Record.V005 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V005", p_Record.V005));
    //        if (p_Record.V006 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V006", p_Record.V006));
    //        if (p_Record.V007 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V007", p_Record.V007));
    //        if (p_Record.V008 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V008", p_Record.V008));
    //        if (p_Record.V009 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V009", p_Record.V009));
    //        if (p_Record.V010 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V010", p_Record.V010));
    //        if (p_Record.V011 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V011", p_Record.V011));
    //        if (p_Record.V012 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V012", p_Record.V012));
    //        if (p_Record.V013 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V013", p_Record.V013));
    //        if (p_Record.V014 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V014", p_Record.V014));
    //        if (p_Record.V015 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V015", p_Record.V015));
    //        if (p_Record.V016 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V016", p_Record.V016));
    //        if (p_Record.V017 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V017", p_Record.V017));
    //        if (p_Record.V018 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V018", p_Record.V018));
    //        if (p_Record.V019 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V019", p_Record.V019));
    //        if (p_Record.V020 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V020", p_Record.V020));
    //        if (p_Record.V021 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V021", p_Record.V021));
    //        if (p_Record.V022 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V022", p_Record.V022));
    //        if (p_Record.V023 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V023", p_Record.V023));
    //        if (p_Record.V024 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V024", p_Record.V024));
    //        if (p_Record.V025 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V025", p_Record.V025));
    //        if (p_Record.V026 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V026", p_Record.V026));
    //        if (p_Record.V027 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V027", p_Record.V027));
    //        if (p_Record.V028 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V028", p_Record.V028));
    //        if (p_Record.V029 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V029", p_Record.V029));
    //        if (p_Record.V030 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V030", p_Record.V030));
    //        if (p_Record.V031 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V031", p_Record.V031));
    //        if (p_Record.V032 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V032", p_Record.V032));
    //        if (p_Record.V033 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V033", p_Record.V033));
    //        if (p_Record.V034 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V034", p_Record.V034));
    //        if (p_Record.V035 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V035", p_Record.V035));
    //        if (p_Record.V036 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V036", p_Record.V036));
    //        if (p_Record.V037 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V037", p_Record.V037));
    //        if (p_Record.V038 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V038", p_Record.V038));
    //        if (p_Record.V039 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V039", p_Record.V039));
    //        if (p_Record.V040 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V040", p_Record.V040));
    //        if (p_Record.V041 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V041", p_Record.V041));
    //        if (p_Record.V042 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V042", p_Record.V042));
    //        if (p_Record.V043 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V043", p_Record.V043));
    //        if (p_Record.V044 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V044", p_Record.V044));
    //        if (p_Record.V045 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V045", p_Record.V045));
    //        if (p_Record.V046 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V046", p_Record.V046));
    //        if (p_Record.V047 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V047", p_Record.V047));
    //        if (p_Record.V048 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V048", p_Record.V048));
    //        if (p_Record.V049 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V049", p_Record.V049));
    //        if (p_Record.V050 != string.Empty)
    //            command.Parameters.Add(new SqlParameter("@V050", p_Record.V050));


    //        //connection.Open();

    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = command;
    //        DataTable dt = new DataTable();
    //        System.Data.DataSet ds = new System.Data.DataSet();
    //        da.Fill(ds);

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }



    //    }

    //}

    public static int Record_Audit(int? nRecordID, string RecordIDs, bool bAfterSave, DateTime RightNow)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Record_Audit", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                if (nRecordID!=null)
                    command.Parameters.Add(new SqlParameter("@nRecordID", nRecordID));

                if (RecordIDs!="")
                     command.Parameters.Add(new SqlParameter("@RecordIDs", RecordIDs));

                command.Parameters.Add(new SqlParameter("@RightNow", RightNow));
                command.Parameters.Add(new SqlParameter("@bAfterSave", bAfterSave));

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
    public static bool ets_Record_IsDuplicate_Entry(int iTableID, int iRecordID, string sUniqueColumnIDSys, string sUniqueColumnIDValue,
        string sUniqueColumnID2Sys, string sUniqueColumnID2Value)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_IsDuplicate_Entry", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                //if(iLocationID!=null)
                //command.Parameters.Add(new SqlParameter("@iLocationID", iLocationID));

                command.Parameters.Add(new SqlParameter("@iTableID", iTableID));
                command.Parameters.Add(new SqlParameter("@iRecordID", iRecordID));
                if (sUniqueColumnIDSys!="")
                    command.Parameters.Add(new SqlParameter("@sUniqueColumnIDSys", sUniqueColumnIDSys));
                if (sUniqueColumnIDValue != "")
                    command.Parameters.Add(new SqlParameter("@sUniqueColumnIDValue", sUniqueColumnIDValue));
                if (sUniqueColumnID2Sys != "")
                    command.Parameters.Add(new SqlParameter("@sUniqueColumnID2Sys", sUniqueColumnID2Sys));
                if (sUniqueColumnID2Value != "")
                    command.Parameters.Add(new SqlParameter("@sUniqueColumnID2Value", sUniqueColumnID2Value));
               

                

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

               
                if (ds == null) return false;



                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }



            }
        }
    }


    #endregion

    #region Menu


    public static List<Menu> ets_Menu_List(int nAccountID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Menu_List", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));
                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                List<Menu> list = new List<Menu>();
                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Menu temp = new Menu(
                                (int)reader["MenuID"],
                                (string)reader["Menu"],
                                  (int)reader["AccountID"],
                                 (bool)reader["ShowOnMenu"],
                                 (bool)reader["IsActive"]);

                            temp.ParentMenuID = reader["ParentMenuID"] == DBNull.Value ? null : (int?)reader["ParentMenuID"];
                            temp.TableID = reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"];
                            temp.DocumentID = reader["DocumentID"] == DBNull.Value ? null : (int?)reader["DocumentID"];
                            //temp.CustomPageLink = reader["CustomPageLink"] == DBNull.Value ? null : (string)reader["CustomPageLink"];

                            temp.OpenInNewWindow = reader["OpenInNewWindow"] == DBNull.Value ? null : (bool?)reader["OpenInNewWindow"];
                            temp.ExternalPageLink = reader["ExternalPageLink"] == DBNull.Value ? null : (string)reader["ExternalPageLink"];

                            temp.DocumentTypeID = reader["DocumentTypeID"] == DBNull.Value ? null : (int?)reader["DocumentTypeID"];
                            temp.MenuType = reader["MenuType"] == DBNull.Value ? "" : (string)reader["MenuType"];

                            list.Add(temp);
                        }




                        connection.Close();
                        connection.Dispose();

                        return list;
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


    //public static DataTable ets_Record_list_old()
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Record_List", connection))
    //        {
    //            connection.Open();
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nAccountID", 2));
    //            command.Parameters.Add(new SqlParameter("@nTableID", 1));

    //            SqlDataAdapter da = new SqlDataAdapter();

    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            System.Data.DataSet ds = new System.Data.DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();

    //            return dt;
    //        }

    //    }


    //}

    public static void MakeTheRecord(ref Record objRecord, string strSystemName, object objValue)
    {


        switch (strSystemName.ToUpper())
        {
            //case "LOCATIONID":
            //    if (objValue.ToString() != "")
            //    {
            //        objRecord.LocationID = int.Parse(objValue.ToString());
            //    }
            //    else
            //    {
            //        objRecord.LocationID = null;
            //    }
            //    break;
            case "TABLEID":
                //do nothing here
                //objRecord.TableID = int.Parse(objValue.ToString());
                break;

            case "DATETIMERECORDED":
                if (objValue.ToString() == "")
                {
                    objRecord.DateTimeRecorded = DateTime.Now;
                }
                else
                {

                    //objRecord.DateTimeRecorded = DateTime.ParseExact(objValue.ToString(), "d/M/yyyy", CultureInfo.InvariantCulture);
                    //objRecord.DateTimeRecorded = DateTime.ParseExact(objValue.ToString(), "d/M/yyyy h:m:s tt", CultureInfo.InvariantCulture);

                    objRecord.DateTimeRecorded = Convert.ToDateTime(objValue.ToString());

                }
                break;

            case "NOTES":
                objRecord.Notes = objValue.ToString();
                break;
            case "ENTEREDBY":
                objRecord.EnteredBy = int.Parse(objValue.ToString());
                break;

            case "ISACTIVE":
                objRecord.IsActive = (bool)objValue;
                break;

            case "V001":
                objRecord.V001 = objValue.ToString();
                break;

            case "V002":
                objRecord.V002 = objValue.ToString();
                break;

            case "V003":
                objRecord.V003 = objValue.ToString();
                break;

            case "V004":
                objRecord.V004 = objValue.ToString();
                break;

            case "V005":
                objRecord.V005 = objValue.ToString();
                break;

            case "V006":
                objRecord.V006 = objValue.ToString();
                break;

            case "V007":
                objRecord.V007 = objValue.ToString();
                break;

            case "V008":
                objRecord.V008 = objValue.ToString();
                break;

            case "V009":
                objRecord.V009 = objValue.ToString();
                break;

            case "V010":
                objRecord.V010 = objValue.ToString();
                break;
            case "V011":
                objRecord.V011 = objValue.ToString();
                break;

            case "V012":
                objRecord.V012 = objValue.ToString();
                break;

            case "V013":
                objRecord.V013 = objValue.ToString();
                break;

            case "V014":
                objRecord.V014 = objValue.ToString();
                break;

            case "V015":
                objRecord.V015 = objValue.ToString();
                break;

            case "V016":
                objRecord.V016 = objValue.ToString();
                break;

            case "V017":
                objRecord.V017 = objValue.ToString();
                break;

            case "V018":
                objRecord.V018 = objValue.ToString();
                break;

            case "V019":
                objRecord.V019 = objValue.ToString();
                break;

            case "V020":
                objRecord.V020 = objValue.ToString();
                break;
            case "V021":
                objRecord.V021 = objValue.ToString();
                break;

            case "V022":
                objRecord.V022 = objValue.ToString();
                break;

            case "V023":
                objRecord.V023 = objValue.ToString();
                break;

            case "V024":
                objRecord.V024 = objValue.ToString();
                break;

            case "V025":
                objRecord.V025 = objValue.ToString();
                break;

            case "V026":
                objRecord.V026 = objValue.ToString();
                break;

            case "V027":
                objRecord.V027 = objValue.ToString();
                break;

            case "V028":
                objRecord.V028 = objValue.ToString();
                break;

            case "V029":
                objRecord.V029 = objValue.ToString();
                break;

            case "V030":
                objRecord.V030 = objValue.ToString();
                break;
            case "V031":
                objRecord.V031 = objValue.ToString();
                break;

            case "V032":
                objRecord.V032 = objValue.ToString();
                break;

            case "V033":
                objRecord.V033 = objValue.ToString();
                break;

            case "V034":
                objRecord.V034 = objValue.ToString();
                break;

            case "V035":
                objRecord.V035 = objValue.ToString();
                break;

            case "V036":
                objRecord.V036 = objValue.ToString();
                break;

            case "V037":
                objRecord.V037 = objValue.ToString();
                break;

            case "V038":
                objRecord.V038 = objValue.ToString();
                break;

            case "V039":
                objRecord.V039 = objValue.ToString();
                break;

            case "V040":
                objRecord.V040 = objValue.ToString();
                break;
            case "V041":
                objRecord.V041 = objValue.ToString();
                break;

            case "V042":
                objRecord.V042 = objValue.ToString();
                break;

            case "V043":
                objRecord.V043 = objValue.ToString();
                break;

            case "V044":
                objRecord.V044 = objValue.ToString();
                break;

            case "V045":
                objRecord.V045 = objValue.ToString();
                break;

            case "V046":
                objRecord.V046 = objValue.ToString();
                break;

            case "V047":
                objRecord.V047 = objValue.ToString();
                break;

            case "V048":
                objRecord.V048 = objValue.ToString();
                break;

            case "V049":
                objRecord.V049 = objValue.ToString();
                break;

            case "V050":
                objRecord.V050 = objValue.ToString();
                break;

            case "V051":
                objRecord.V051 = objValue.ToString();
                break;

            case "V052":
                objRecord.V052 = objValue.ToString();
                break;

            case "V053":
                objRecord.V053 = objValue.ToString();
                break;

            case "V054":
                objRecord.V054 = objValue.ToString();
                break;

            case "V055":
                objRecord.V055 = objValue.ToString();
                break;

            case "V056":
                objRecord.V056 = objValue.ToString();
                break;

            case "V057":
                objRecord.V057 = objValue.ToString();
                break;

            case "V058":
                objRecord.V058 = objValue.ToString();
                break;

            case "V059":
                objRecord.V059 = objValue.ToString();
                break;

            case "V060":
                objRecord.V060 = objValue.ToString();
                break;
            case "V061":
                objRecord.V061 = objValue.ToString();
                break;

            case "V062":
                objRecord.V062 = objValue.ToString();
                break;

            case "V063":
                objRecord.V063 = objValue.ToString();
                break;

            case "V064":
                objRecord.V064 = objValue.ToString();
                break;

            case "V065":
                objRecord.V065 = objValue.ToString();
                break;

            case "V066":
                objRecord.V066 = objValue.ToString();
                break;

            case "V067":
                objRecord.V067 = objValue.ToString();
                break;

            case "V068":
                objRecord.V068 = objValue.ToString();
                break;

            case "V069":
                objRecord.V069 = objValue.ToString();
                break;

            case "V070":
                objRecord.V070 = objValue.ToString();
                break;
            case "V071":
                objRecord.V071 = objValue.ToString();
                break;

            case "V072":
                objRecord.V072 = objValue.ToString();
                break;

            case "V073":
                objRecord.V073 = objValue.ToString();
                break;

            case "V074":
                objRecord.V074 = objValue.ToString();
                break;

            case "V075":
                objRecord.V075 = objValue.ToString();
                break;

            case "V076":
                objRecord.V076 = objValue.ToString();
                break;

            case "V077":
                objRecord.V077 = objValue.ToString();
                break;

            case "V078":
                objRecord.V078 = objValue.ToString();
                break;

            case "V079":
                objRecord.V079 = objValue.ToString();
                break;

            case "V080":
                objRecord.V080 = objValue.ToString();
                break;
            case "V081":
                objRecord.V081 = objValue.ToString();
                break;

            case "V082":
                objRecord.V082 = objValue.ToString();
                break;

            case "V083":
                objRecord.V083 = objValue.ToString();
                break;

            case "V084":
                objRecord.V084 = objValue.ToString();
                break;

            case "V085":
                objRecord.V085 = objValue.ToString();
                break;

            case "V086":
                objRecord.V086 = objValue.ToString();
                break;

            case "V087":
                objRecord.V087 = objValue.ToString();
                break;

            case "V088":
                objRecord.V088 = objValue.ToString();
                break;

            case "V089":
                objRecord.V089 = objValue.ToString();
                break;

            case "V090":
                objRecord.V090 = objValue.ToString();
                break;
            case "V091":
                objRecord.V091 = objValue.ToString();
                break;

            case "V092":
                objRecord.V092 = objValue.ToString();
                break;

            case "V093":
                objRecord.V093 = objValue.ToString();
                break;

            case "V094":
                objRecord.V094 = objValue.ToString();
                break;

            case "V095":
                objRecord.V095 = objValue.ToString();
                break;

            case "V096":
                objRecord.V096 = objValue.ToString();
                break;

            case "V097":
                objRecord.V097 = objValue.ToString();
                break;

            case "V098":
                objRecord.V098 = objValue.ToString();
                break;

            case "V099":
                objRecord.V099 = objValue.ToString();
                break;

            case "V100":
                objRecord.V100 = objValue.ToString();
                break;



            case "V101":
                objRecord.V101 = objValue.ToString();
                break;

            case "V102":
                objRecord.V102 = objValue.ToString();
                break;

            case "V103":
                objRecord.V103 = objValue.ToString();
                break;

            case "V104":
                objRecord.V104 = objValue.ToString();
                break;

            case "V105":
                objRecord.V105 = objValue.ToString();
                break;

            case "V106":
                objRecord.V106 = objValue.ToString();
                break;

            case "V107":
                objRecord.V107 = objValue.ToString();
                break;

            case "V108":
                objRecord.V108 = objValue.ToString();
                break;

            case "V109":
                objRecord.V109 = objValue.ToString();
                break;

            case "V110":
                objRecord.V110 = objValue.ToString();
                break;
            case "V111":
                objRecord.V111 = objValue.ToString();
                break;

            case "V112":
                objRecord.V112 = objValue.ToString();
                break;

            case "V113":
                objRecord.V113 = objValue.ToString();
                break;

            case "V114":
                objRecord.V114 = objValue.ToString();
                break;

            case "V115":
                objRecord.V115 = objValue.ToString();
                break;

            case "V116":
                objRecord.V116 = objValue.ToString();
                break;

            case "V117":
                objRecord.V117 = objValue.ToString();
                break;

            case "V118":
                objRecord.V118 = objValue.ToString();
                break;

            case "V119":
                objRecord.V119 = objValue.ToString();
                break;

            case "V120":
                objRecord.V120 = objValue.ToString();
                break;
            case "V121":
                objRecord.V121 = objValue.ToString();
                break;

            case "V122":
                objRecord.V122 = objValue.ToString();
                break;

            case "V123":
                objRecord.V123 = objValue.ToString();
                break;

            case "V124":
                objRecord.V124 = objValue.ToString();
                break;

            case "V125":
                objRecord.V125 = objValue.ToString();
                break;

            case "V126":
                objRecord.V126 = objValue.ToString();
                break;

            case "V127":
                objRecord.V127 = objValue.ToString();
                break;

            case "V128":
                objRecord.V128 = objValue.ToString();
                break;

            case "V129":
                objRecord.V129 = objValue.ToString();
                break;

            case "V130":
                objRecord.V130 = objValue.ToString();
                break;
            case "V131":
                objRecord.V131 = objValue.ToString();
                break;

            case "V132":
                objRecord.V132 = objValue.ToString();
                break;

            case "V133":
                objRecord.V133 = objValue.ToString();
                break;

            case "V134":
                objRecord.V134 = objValue.ToString();
                break;

            case "V135":
                objRecord.V135 = objValue.ToString();
                break;

            case "V136":
                objRecord.V136 = objValue.ToString();
                break;

            case "V137":
                objRecord.V137 = objValue.ToString();
                break;

            case "V138":
                objRecord.V138 = objValue.ToString();
                break;

            case "V139":
                objRecord.V139 = objValue.ToString();
                break;

            case "V140":
                objRecord.V140 = objValue.ToString();
                break;
            case "V141":
                objRecord.V141 = objValue.ToString();
                break;

            case "V142":
                objRecord.V142 = objValue.ToString();
                break;

            case "V143":
                objRecord.V143 = objValue.ToString();
                break;

            case "V144":
                objRecord.V144 = objValue.ToString();
                break;

            case "V145":
                objRecord.V145 = objValue.ToString();
                break;

            case "V146":
                objRecord.V146 = objValue.ToString();
                break;

            case "V147":
                objRecord.V147 = objValue.ToString();
                break;

            case "V148":
                objRecord.V148 = objValue.ToString();
                break;

            case "V149":
                objRecord.V149 = objValue.ToString();
                break;

            case "V150":
                objRecord.V150 = objValue.ToString();
                break;

            case "V151":
                objRecord.V151 = objValue.ToString();
                break;

            case "V152":
                objRecord.V152 = objValue.ToString();
                break;

            case "V153":
                objRecord.V153 = objValue.ToString();
                break;

            case "V154":
                objRecord.V154 = objValue.ToString();
                break;

            case "V155":
                objRecord.V155 = objValue.ToString();
                break;

            case "V156":
                objRecord.V156 = objValue.ToString();
                break;

            case "V157":
                objRecord.V157 = objValue.ToString();
                break;

            case "V158":
                objRecord.V158 = objValue.ToString();
                break;

            case "V159":
                objRecord.V159 = objValue.ToString();
                break;

            case "V160":
                objRecord.V160 = objValue.ToString();
                break;
            case "V161":
                objRecord.V161 = objValue.ToString();
                break;

            case "V162":
                objRecord.V162 = objValue.ToString();
                break;

            case "V163":
                objRecord.V163 = objValue.ToString();
                break;

            case "V164":
                objRecord.V164 = objValue.ToString();
                break;

            case "V165":
                objRecord.V165 = objValue.ToString();
                break;

            case "V166":
                objRecord.V166 = objValue.ToString();
                break;

            case "V167":
                objRecord.V167 = objValue.ToString();
                break;

            case "V168":
                objRecord.V168 = objValue.ToString();
                break;

            case "V169":
                objRecord.V169 = objValue.ToString();
                break;

            case "V170":
                objRecord.V170 = objValue.ToString();
                break;
            case "V171":
                objRecord.V171 = objValue.ToString();
                break;

            case "V172":
                objRecord.V172 = objValue.ToString();
                break;

            case "V173":
                objRecord.V173 = objValue.ToString();
                break;

            case "V174":
                objRecord.V174 = objValue.ToString();
                break;

            case "V175":
                objRecord.V175 = objValue.ToString();
                break;

            case "V176":
                objRecord.V176 = objValue.ToString();
                break;

            case "V177":
                objRecord.V177 = objValue.ToString();
                break;

            case "V178":
                objRecord.V178 = objValue.ToString();
                break;

            case "V179":
                objRecord.V179 = objValue.ToString();
                break;

            case "V180":
                objRecord.V180 = objValue.ToString();
                break;
            case "V181":
                objRecord.V181 = objValue.ToString();
                break;

            case "V182":
                objRecord.V182 = objValue.ToString();
                break;

            case "V183":
                objRecord.V183 = objValue.ToString();
                break;

            case "V184":
                objRecord.V184 = objValue.ToString();
                break;

            case "V185":
                objRecord.V185 = objValue.ToString();
                break;

            case "V186":
                objRecord.V186 = objValue.ToString();
                break;

            case "V187":
                objRecord.V187 = objValue.ToString();
                break;

            case "V188":
                objRecord.V188 = objValue.ToString();
                break;

            case "V189":
                objRecord.V189 = objValue.ToString();
                break;

            case "V190":
                objRecord.V190 = objValue.ToString();
                break;
            case "V191":
                objRecord.V191 = objValue.ToString();
                break;

            case "V192":
                objRecord.V192 = objValue.ToString();
                break;

            case "V193":
                objRecord.V193 = objValue.ToString();
                break;

            case "V194":
                objRecord.V194 = objValue.ToString();
                break;

            case "V195":
                objRecord.V195 = objValue.ToString();
                break;

            case "V196":
                objRecord.V196 = objValue.ToString();
                break;

            case "V197":
                objRecord.V197 = objValue.ToString();
                break;

            case "V198":
                objRecord.V198 = objValue.ToString();
                break;

            case "V199":
                objRecord.V199 = objValue.ToString();
                break;

            case "V200":
                objRecord.V200 = objValue.ToString();
                break;

            case "V201":
                objRecord.V201 = objValue.ToString();
                break;

            case "V202":
                objRecord.V202 = objValue.ToString();
                break;

            case "V203":
                objRecord.V203 = objValue.ToString();
                break;

            case "V204":
                objRecord.V204 = objValue.ToString();
                break;

            case "V205":
                objRecord.V205 = objValue.ToString();
                break;

            case "V206":
                objRecord.V206 = objValue.ToString();
                break;

            case "V207":
                objRecord.V207 = objValue.ToString();
                break;

            case "V208":
                objRecord.V208 = objValue.ToString();
                break;

            case "V209":
                objRecord.V209 = objValue.ToString();
                break;

            case "V210":
                objRecord.V210 = objValue.ToString();
                break;
            case "V211":
                objRecord.V211 = objValue.ToString();
                break;

            case "V212":
                objRecord.V212 = objValue.ToString();
                break;

            case "V213":
                objRecord.V213 = objValue.ToString();
                break;

            case "V214":
                objRecord.V214 = objValue.ToString();
                break;

            case "V215":
                objRecord.V215 = objValue.ToString();
                break;

            case "V216":
                objRecord.V216 = objValue.ToString();
                break;

            case "V217":
                objRecord.V217 = objValue.ToString();
                break;

            case "V218":
                objRecord.V218 = objValue.ToString();
                break;

            case "V219":
                objRecord.V219 = objValue.ToString();
                break;

            case "V220":
                objRecord.V220 = objValue.ToString();
                break;
            case "V221":
                objRecord.V221 = objValue.ToString();
                break;

            case "V222":
                objRecord.V222 = objValue.ToString();
                break;

            case "V223":
                objRecord.V223 = objValue.ToString();
                break;

            case "V224":
                objRecord.V224 = objValue.ToString();
                break;

            case "V225":
                objRecord.V225 = objValue.ToString();
                break;

            case "V226":
                objRecord.V226 = objValue.ToString();
                break;

            case "V227":
                objRecord.V227 = objValue.ToString();
                break;

            case "V228":
                objRecord.V228 = objValue.ToString();
                break;

            case "V229":
                objRecord.V229 = objValue.ToString();
                break;

            case "V230":
                objRecord.V230 = objValue.ToString();
                break;
            case "V231":
                objRecord.V231 = objValue.ToString();
                break;

            case "V232":
                objRecord.V232 = objValue.ToString();
                break;

            case "V233":
                objRecord.V233 = objValue.ToString();
                break;

            case "V234":
                objRecord.V234 = objValue.ToString();
                break;

            case "V235":
                objRecord.V235 = objValue.ToString();
                break;

            case "V236":
                objRecord.V236 = objValue.ToString();
                break;

            case "V237":
                objRecord.V237 = objValue.ToString();
                break;

            case "V238":
                objRecord.V238 = objValue.ToString();
                break;

            case "V239":
                objRecord.V239 = objValue.ToString();
                break;

            case "V240":
                objRecord.V240 = objValue.ToString();
                break;
            case "V241":
                objRecord.V241 = objValue.ToString();
                break;

            case "V242":
                objRecord.V242 = objValue.ToString();
                break;

            case "V243":
                objRecord.V243 = objValue.ToString();
                break;

            case "V244":
                objRecord.V244 = objValue.ToString();
                break;

            case "V245":
                objRecord.V245 = objValue.ToString();
                break;

            case "V246":
                objRecord.V246 = objValue.ToString();
                break;

            case "V247":
                objRecord.V247 = objValue.ToString();
                break;

            case "V248":
                objRecord.V248 = objValue.ToString();
                break;

            case "V249":
                objRecord.V249 = objValue.ToString();
                break;

            case "V250":
                objRecord.V250 = objValue.ToString();
                break;

            case "V251":
                objRecord.V251 = objValue.ToString();
                break;

            case "V252":
                objRecord.V252 = objValue.ToString();
                break;

            case "V253":
                objRecord.V253 = objValue.ToString();
                break;

            case "V254":
                objRecord.V254 = objValue.ToString();
                break;

            case "V255":
                objRecord.V255 = objValue.ToString();
                break;

            case "V256":
                objRecord.V256 = objValue.ToString();
                break;

            case "V257":
                objRecord.V257 = objValue.ToString();
                break;

            case "V258":
                objRecord.V258 = objValue.ToString();
                break;

            case "V259":
                objRecord.V259 = objValue.ToString();
                break;

            case "V260":
                objRecord.V260 = objValue.ToString();
                break;
            case "V261":
                objRecord.V261 = objValue.ToString();
                break;

            case "V262":
                objRecord.V262 = objValue.ToString();
                break;

            case "V263":
                objRecord.V263 = objValue.ToString();
                break;

            case "V264":
                objRecord.V264 = objValue.ToString();
                break;

            case "V265":
                objRecord.V265 = objValue.ToString();
                break;

            case "V266":
                objRecord.V266 = objValue.ToString();
                break;

            case "V267":
                objRecord.V267 = objValue.ToString();
                break;

            case "V268":
                objRecord.V268 = objValue.ToString();
                break;

            case "V269":
                objRecord.V269 = objValue.ToString();
                break;

            case "V270":
                objRecord.V270 = objValue.ToString();
                break;
            case "V271":
                objRecord.V271 = objValue.ToString();
                break;

            case "V272":
                objRecord.V272 = objValue.ToString();
                break;

            case "V273":
                objRecord.V273 = objValue.ToString();
                break;

            case "V274":
                objRecord.V274 = objValue.ToString();
                break;

            case "V275":
                objRecord.V275 = objValue.ToString();
                break;

            case "V276":
                objRecord.V276 = objValue.ToString();
                break;

            case "V277":
                objRecord.V277 = objValue.ToString();
                break;

            case "V278":
                objRecord.V278 = objValue.ToString();
                break;

            case "V279":
                objRecord.V279 = objValue.ToString();
                break;

            case "V280":
                objRecord.V280 = objValue.ToString();
                break;
            case "V281":
                objRecord.V281 = objValue.ToString();
                break;

            case "V282":
                objRecord.V282 = objValue.ToString();
                break;

            case "V283":
                objRecord.V283 = objValue.ToString();
                break;

            case "V284":
                objRecord.V284 = objValue.ToString();
                break;

            case "V285":
                objRecord.V285 = objValue.ToString();
                break;

            case "V286":
                objRecord.V286 = objValue.ToString();
                break;

            case "V287":
                objRecord.V287 = objValue.ToString();
                break;

            case "V288":
                objRecord.V288 = objValue.ToString();
                break;

            case "V289":
                objRecord.V289 = objValue.ToString();
                break;

            case "V290":
                objRecord.V290 = objValue.ToString();
                break;
            case "V291":
                objRecord.V291 = objValue.ToString();
                break;

            case "V292":
                objRecord.V292 = objValue.ToString();
                break;

            case "V293":
                objRecord.V293 = objValue.ToString();
                break;

            case "V294":
                objRecord.V294 = objValue.ToString();
                break;

            case "V295":
                objRecord.V295 = objValue.ToString();
                break;

            case "V296":
                objRecord.V296 = objValue.ToString();
                break;

            case "V297":
                objRecord.V297 = objValue.ToString();
                break;

            case "V298":
                objRecord.V298 = objValue.ToString();
                break;

            case "V299":
                objRecord.V299 = objValue.ToString();
                break;

            case "V300":
                objRecord.V300 = objValue.ToString();
                break;

            case "V301":
                objRecord.V301 = objValue.ToString();
                break;

            case "V302":
                objRecord.V302 = objValue.ToString();
                break;

            case "V303":
                objRecord.V303 = objValue.ToString();
                break;

            case "V304":
                objRecord.V304 = objValue.ToString();
                break;

            case "V305":
                objRecord.V305 = objValue.ToString();
                break;

            case "V306":
                objRecord.V306 = objValue.ToString();
                break;

            case "V307":
                objRecord.V307 = objValue.ToString();
                break;

            case "V308":
                objRecord.V308 = objValue.ToString();
                break;

            case "V309":
                objRecord.V309 = objValue.ToString();
                break;

            case "V310":
                objRecord.V310 = objValue.ToString();
                break;
            case "V311":
                objRecord.V311 = objValue.ToString();
                break;

            case "V312":
                objRecord.V312 = objValue.ToString();
                break;

            case "V313":
                objRecord.V313 = objValue.ToString();
                break;

            case "V314":
                objRecord.V314 = objValue.ToString();
                break;

            case "V315":
                objRecord.V315 = objValue.ToString();
                break;

            case "V316":
                objRecord.V316 = objValue.ToString();
                break;

            case "V317":
                objRecord.V317 = objValue.ToString();
                break;

            case "V318":
                objRecord.V318 = objValue.ToString();
                break;

            case "V319":
                objRecord.V319 = objValue.ToString();
                break;

            case "V320":
                objRecord.V320 = objValue.ToString();
                break;
            case "V321":
                objRecord.V321 = objValue.ToString();
                break;

            case "V322":
                objRecord.V322 = objValue.ToString();
                break;

            case "V323":
                objRecord.V323 = objValue.ToString();
                break;

            case "V324":
                objRecord.V324 = objValue.ToString();
                break;

            case "V325":
                objRecord.V325 = objValue.ToString();
                break;

            case "V326":
                objRecord.V326 = objValue.ToString();
                break;

            case "V327":
                objRecord.V327 = objValue.ToString();
                break;

            case "V328":
                objRecord.V328 = objValue.ToString();
                break;

            case "V329":
                objRecord.V329 = objValue.ToString();
                break;

            case "V330":
                objRecord.V330 = objValue.ToString();
                break;
            case "V331":
                objRecord.V331 = objValue.ToString();
                break;

            case "V332":
                objRecord.V332 = objValue.ToString();
                break;

            case "V333":
                objRecord.V333 = objValue.ToString();
                break;

            case "V334":
                objRecord.V334 = objValue.ToString();
                break;

            case "V335":
                objRecord.V335 = objValue.ToString();
                break;

            case "V336":
                objRecord.V336 = objValue.ToString();
                break;

            case "V337":
                objRecord.V337 = objValue.ToString();
                break;

            case "V338":
                objRecord.V338 = objValue.ToString();
                break;

            case "V339":
                objRecord.V339 = objValue.ToString();
                break;

            case "V340":
                objRecord.V340 = objValue.ToString();
                break;
            case "V341":
                objRecord.V341 = objValue.ToString();
                break;

            case "V342":
                objRecord.V342 = objValue.ToString();
                break;

            case "V343":
                objRecord.V343 = objValue.ToString();
                break;

            case "V344":
                objRecord.V344 = objValue.ToString();
                break;

            case "V345":
                objRecord.V345 = objValue.ToString();
                break;

            case "V346":
                objRecord.V346 = objValue.ToString();
                break;

            case "V347":
                objRecord.V347 = objValue.ToString();
                break;

            case "V348":
                objRecord.V348 = objValue.ToString();
                break;

            case "V349":
                objRecord.V349 = objValue.ToString();
                break;

            case "V350":
                objRecord.V350 = objValue.ToString();
                break;

            case "V351":
                objRecord.V351 = objValue.ToString();
                break;

            case "V352":
                objRecord.V352 = objValue.ToString();
                break;

            case "V353":
                objRecord.V353 = objValue.ToString();
                break;

            case "V354":
                objRecord.V354 = objValue.ToString();
                break;

            case "V355":
                objRecord.V355 = objValue.ToString();
                break;

            case "V356":
                objRecord.V356 = objValue.ToString();
                break;

            case "V357":
                objRecord.V357 = objValue.ToString();
                break;

            case "V358":
                objRecord.V358 = objValue.ToString();
                break;

            case "V359":
                objRecord.V359 = objValue.ToString();
                break;

            case "V360":
                objRecord.V360 = objValue.ToString();
                break;
            case "V361":
                objRecord.V361 = objValue.ToString();
                break;

            case "V362":
                objRecord.V362 = objValue.ToString();
                break;

            case "V363":
                objRecord.V363 = objValue.ToString();
                break;

            case "V364":
                objRecord.V364 = objValue.ToString();
                break;

            case "V365":
                objRecord.V365 = objValue.ToString();
                break;

            case "V366":
                objRecord.V366 = objValue.ToString();
                break;

            case "V367":
                objRecord.V367 = objValue.ToString();
                break;

            case "V368":
                objRecord.V368 = objValue.ToString();
                break;

            case "V369":
                objRecord.V369 = objValue.ToString();
                break;

            case "V370":
                objRecord.V370 = objValue.ToString();
                break;
            case "V371":
                objRecord.V371 = objValue.ToString();
                break;

            case "V372":
                objRecord.V372 = objValue.ToString();
                break;

            case "V373":
                objRecord.V373 = objValue.ToString();
                break;

            case "V374":
                objRecord.V374 = objValue.ToString();
                break;

            case "V375":
                objRecord.V375 = objValue.ToString();
                break;

            case "V376":
                objRecord.V376 = objValue.ToString();
                break;

            case "V377":
                objRecord.V377 = objValue.ToString();
                break;

            case "V378":
                objRecord.V378 = objValue.ToString();
                break;

            case "V379":
                objRecord.V379 = objValue.ToString();
                break;

            case "V380":
                objRecord.V380 = objValue.ToString();
                break;
            case "V381":
                objRecord.V381 = objValue.ToString();
                break;

            case "V382":
                objRecord.V382 = objValue.ToString();
                break;

            case "V383":
                objRecord.V383 = objValue.ToString();
                break;

            case "V384":
                objRecord.V384 = objValue.ToString();
                break;

            case "V385":
                objRecord.V385 = objValue.ToString();
                break;

            case "V386":
                objRecord.V386 = objValue.ToString();
                break;

            case "V387":
                objRecord.V387 = objValue.ToString();
                break;

            case "V388":
                objRecord.V388 = objValue.ToString();
                break;

            case "V389":
                objRecord.V389 = objValue.ToString();
                break;

            case "V390":
                objRecord.V390 = objValue.ToString();
                break;
            case "V391":
                objRecord.V391 = objValue.ToString();
                break;

            case "V392":
                objRecord.V392 = objValue.ToString();
                break;

            case "V393":
                objRecord.V393 = objValue.ToString();
                break;

            case "V394":
                objRecord.V394 = objValue.ToString();
                break;

            case "V395":
                objRecord.V395 = objValue.ToString();
                break;

            case "V396":
                objRecord.V396 = objValue.ToString();
                break;

            case "V397":
                objRecord.V397 = objValue.ToString();
                break;

            case "V398":
                objRecord.V398 = objValue.ToString();
                break;

            case "V399":
                objRecord.V399 = objValue.ToString();
                break;

            case "V400":
                objRecord.V400 = objValue.ToString();
                break;

            case "V401":
                objRecord.V401 = objValue.ToString();
                break;

            case "V402":
                objRecord.V402 = objValue.ToString();
                break;

            case "V403":
                objRecord.V403 = objValue.ToString();
                break;

            case "V404":
                objRecord.V404 = objValue.ToString();
                break;

            case "V405":
                objRecord.V405 = objValue.ToString();
                break;

            case "V406":
                objRecord.V406 = objValue.ToString();
                break;

            case "V407":
                objRecord.V407 = objValue.ToString();
                break;

            case "V408":
                objRecord.V408 = objValue.ToString();
                break;

            case "V409":
                objRecord.V409 = objValue.ToString();
                break;

            case "V410":
                objRecord.V410 = objValue.ToString();
                break;
            case "V411":
                objRecord.V411 = objValue.ToString();
                break;

            case "V412":
                objRecord.V412 = objValue.ToString();
                break;

            case "V413":
                objRecord.V413 = objValue.ToString();
                break;

            case "V414":
                objRecord.V414 = objValue.ToString();
                break;

            case "V415":
                objRecord.V415 = objValue.ToString();
                break;

            case "V416":
                objRecord.V416 = objValue.ToString();
                break;

            case "V417":
                objRecord.V417 = objValue.ToString();
                break;

            case "V418":
                objRecord.V418 = objValue.ToString();
                break;

            case "V419":
                objRecord.V419 = objValue.ToString();
                break;

            case "V420":
                objRecord.V420 = objValue.ToString();
                break;
            case "V421":
                objRecord.V421 = objValue.ToString();
                break;

            case "V422":
                objRecord.V422 = objValue.ToString();
                break;

            case "V423":
                objRecord.V423 = objValue.ToString();
                break;

            case "V424":
                objRecord.V424 = objValue.ToString();
                break;

            case "V425":
                objRecord.V425 = objValue.ToString();
                break;

            case "V426":
                objRecord.V426 = objValue.ToString();
                break;

            case "V427":
                objRecord.V427 = objValue.ToString();
                break;

            case "V428":
                objRecord.V428 = objValue.ToString();
                break;

            case "V429":
                objRecord.V429 = objValue.ToString();
                break;

            case "V430":
                objRecord.V430 = objValue.ToString();
                break;
            case "V431":
                objRecord.V431 = objValue.ToString();
                break;

            case "V432":
                objRecord.V432 = objValue.ToString();
                break;

            case "V433":
                objRecord.V433 = objValue.ToString();
                break;

            case "V434":
                objRecord.V434 = objValue.ToString();
                break;

            case "V435":
                objRecord.V435 = objValue.ToString();
                break;

            case "V436":
                objRecord.V436 = objValue.ToString();
                break;

            case "V437":
                objRecord.V437 = objValue.ToString();
                break;

            case "V438":
                objRecord.V438 = objValue.ToString();
                break;

            case "V439":
                objRecord.V439 = objValue.ToString();
                break;

            case "V440":
                objRecord.V440 = objValue.ToString();
                break;
            case "V441":
                objRecord.V441 = objValue.ToString();
                break;

            case "V442":
                objRecord.V442 = objValue.ToString();
                break;

            case "V443":
                objRecord.V443 = objValue.ToString();
                break;

            case "V444":
                objRecord.V444 = objValue.ToString();
                break;

            case "V445":
                objRecord.V445 = objValue.ToString();
                break;

            case "V446":
                objRecord.V446 = objValue.ToString();
                break;

            case "V447":
                objRecord.V447 = objValue.ToString();
                break;

            case "V448":
                objRecord.V448 = objValue.ToString();
                break;

            case "V449":
                objRecord.V449 = objValue.ToString();
                break;

            case "V450":
                objRecord.V450 = objValue.ToString();
                break;

            case "V451":
                objRecord.V451 = objValue.ToString();
                break;

            case "V452":
                objRecord.V452 = objValue.ToString();
                break;

            case "V453":
                objRecord.V453 = objValue.ToString();
                break;

            case "V454":
                objRecord.V454 = objValue.ToString();
                break;

            case "V455":
                objRecord.V455 = objValue.ToString();
                break;

            case "V456":
                objRecord.V456 = objValue.ToString();
                break;

            case "V457":
                objRecord.V457 = objValue.ToString();
                break;

            case "V458":
                objRecord.V458 = objValue.ToString();
                break;

            case "V459":
                objRecord.V459 = objValue.ToString();
                break;

            case "V460":
                objRecord.V460 = objValue.ToString();
                break;
            case "V461":
                objRecord.V461 = objValue.ToString();
                break;

            case "V462":
                objRecord.V462 = objValue.ToString();
                break;

            case "V463":
                objRecord.V463 = objValue.ToString();
                break;

            case "V464":
                objRecord.V464 = objValue.ToString();
                break;

            case "V465":
                objRecord.V465 = objValue.ToString();
                break;

            case "V466":
                objRecord.V466 = objValue.ToString();
                break;

            case "V467":
                objRecord.V467 = objValue.ToString();
                break;

            case "V468":
                objRecord.V468 = objValue.ToString();
                break;

            case "V469":
                objRecord.V469 = objValue.ToString();
                break;

            case "V470":
                objRecord.V470 = objValue.ToString();
                break;
            case "V471":
                objRecord.V471 = objValue.ToString();
                break;

            case "V472":
                objRecord.V472 = objValue.ToString();
                break;

            case "V473":
                objRecord.V473 = objValue.ToString();
                break;

            case "V474":
                objRecord.V474 = objValue.ToString();
                break;

            case "V475":
                objRecord.V475 = objValue.ToString();
                break;

            case "V476":
                objRecord.V476 = objValue.ToString();
                break;

            case "V477":
                objRecord.V477 = objValue.ToString();
                break;

            case "V478":
                objRecord.V478 = objValue.ToString();
                break;

            case "V479":
                objRecord.V479 = objValue.ToString();
                break;

            case "V480":
                objRecord.V480 = objValue.ToString();
                break;
            case "V481":
                objRecord.V481 = objValue.ToString();
                break;

            case "V482":
                objRecord.V482 = objValue.ToString();
                break;

            case "V483":
                objRecord.V483 = objValue.ToString();
                break;

            case "V484":
                objRecord.V484 = objValue.ToString();
                break;

            case "V485":
                objRecord.V485 = objValue.ToString();
                break;

            case "V486":
                objRecord.V486 = objValue.ToString();
                break;

            case "V487":
                objRecord.V487 = objValue.ToString();
                break;

            case "V488":
                objRecord.V488 = objValue.ToString();
                break;

            case "V489":
                objRecord.V489 = objValue.ToString();
                break;

            case "V490":
                objRecord.V490 = objValue.ToString();
                break;
            case "V491":
                objRecord.V491 = objValue.ToString();
                break;

            case "V492":
                objRecord.V492 = objValue.ToString();
                break;

            case "V493":
                objRecord.V493 = objValue.ToString();
                break;

            case "V494":
                objRecord.V494 = objValue.ToString();
                break;

            case "V495":
                objRecord.V495 = objValue.ToString();
                break;

            case "V496":
                objRecord.V496 = objValue.ToString();
                break;

            case "V497":
                objRecord.V497 = objValue.ToString();
                break;

            case "V498":
                objRecord.V498 = objValue.ToString();
                break;

            case "V499":
                objRecord.V499 = objValue.ToString();
                break;

            case "V500":
                objRecord.V500 = objValue.ToString();
                break;
        }


    }



    public static string GetRecordValue(ref Record objRecord, string strSystemName)
    {


        switch (strSystemName.ToUpper())
        {
            //case "LOCATIONID":
            //    return objRecord.LocationID.ToString();

            case "RECORDID":
                return objRecord.RecordID.ToString();

            case "TABLEID":
                return objRecord.TableID.ToString();


            case "DATETIMERECORDED":
                return objRecord.DateTimeRecorded.ToString();


            case "NOTES":
                return objRecord.Notes.ToString();

            case "ENTEREDBY":
                return objRecord.EnteredBy.ToString();


            case "ISACTIVE":
                return objRecord.IsActive.ToString();


            case "V001":
                return objRecord.V001;


            case "V002":
                return objRecord.V002;


            case "V003":
                return objRecord.V003;


            case "V004":
                return objRecord.V004;


            case "V005":
                return objRecord.V005;


            case "V006":
                return objRecord.V006;


            case "V007":
                return objRecord.V007;


            case "V008":
                return objRecord.V008;


            case "V009":
                return objRecord.V009;


            case "V010":
                return objRecord.V010;

            case "V011":
                return objRecord.V011;


            case "V012":
                return objRecord.V012;


            case "V013":
                return objRecord.V013;


            case "V014":
                return objRecord.V014;


            case "V015":
                return objRecord.V015;


            case "V016":
                return objRecord.V016;


            case "V017":
                return objRecord.V017;


            case "V018":
                return objRecord.V018;


            case "V019":
                return objRecord.V019;


            case "V020":
                return objRecord.V020;

            case "V021":
                return objRecord.V021;


            case "V022":
                return objRecord.V022;


            case "V023":
                return objRecord.V023;


            case "V024":
                return objRecord.V024;


            case "V025":
                return objRecord.V025;


            case "V026":
                return objRecord.V026;


            case "V027":
                return objRecord.V027;


            case "V028":
                return objRecord.V028;


            case "V029":
                return objRecord.V029;


            case "V030":
                return objRecord.V030;

            case "V031":
                return objRecord.V031;


            case "V032":
                return objRecord.V032;


            case "V033":
                return objRecord.V033;


            case "V034":
                return objRecord.V034;


            case "V035":
                return objRecord.V035;


            case "V036":
                return objRecord.V036;


            case "V037":
                return objRecord.V037;


            case "V038":
                return objRecord.V038;


            case "V039":
                return objRecord.V039;


            case "V040":
                return objRecord.V040;

            case "V041":
                return objRecord.V041;


            case "V042":
                return objRecord.V042;


            case "V043":
                return objRecord.V043;


            case "V044":
                return objRecord.V044;


            case "V045":
                return objRecord.V045;


            case "V046":
                return objRecord.V046;


            case "V047":
                return objRecord.V047;


            case "V048":
                return objRecord.V048;


            case "V049":
                return objRecord.V049;


            case "V050":
                return objRecord.V050;




            case "V051":
                return objRecord.V051;


            case "V052":
                return objRecord.V052;


            case "V053":
                return objRecord.V053;


            case "V054":
                return objRecord.V054;


            case "V055":
                return objRecord.V055;


            case "V056":
                return objRecord.V056;


            case "V057":
                return objRecord.V057;


            case "V058":
                return objRecord.V058;


            case "V059":
                return objRecord.V059;


            case "V060":
                return objRecord.V060;




            case "V061":
                return objRecord.V061;


            case "V062":
                return objRecord.V062;


            case "V063":
                return objRecord.V063;


            case "V064":
                return objRecord.V064;


            case "V065":
                return objRecord.V065;


            case "V066":
                return objRecord.V066;


            case "V067":
                return objRecord.V067;


            case "V068":
                return objRecord.V068;


            case "V069":
                return objRecord.V069;


            case "V070":
                return objRecord.V070;




            case "V071":
                return objRecord.V071;


            case "V072":
                return objRecord.V072;


            case "V073":
                return objRecord.V073;


            case "V074":
                return objRecord.V074;


            case "V075":
                return objRecord.V075;


            case "V076":
                return objRecord.V076;


            case "V077":
                return objRecord.V077;


            case "V078":
                return objRecord.V078;


            case "V079":
                return objRecord.V079;


            case "V080":
                return objRecord.V080;




            case "V081":
                return objRecord.V081;


            case "V082":
                return objRecord.V082;


            case "V083":
                return objRecord.V083;


            case "V084":
                return objRecord.V084;


            case "V085":
                return objRecord.V085;


            case "V086":
                return objRecord.V086;


            case "V087":
                return objRecord.V087;


            case "V088":
                return objRecord.V088;


            case "V089":
                return objRecord.V089;


            case "V090":
                return objRecord.V090;




            case "V091":
                return objRecord.V091;


            case "V092":
                return objRecord.V092;


            case "V093":
                return objRecord.V093;


            case "V094":
                return objRecord.V094;


            case "V095":
                return objRecord.V095;


            case "V096":
                return objRecord.V096;


            case "V097":
                return objRecord.V097;


            case "V098":
                return objRecord.V098;


            case "V099":
                return objRecord.V099;


            case "V100":
                return objRecord.V100;


            case "V101":
                return objRecord.V101;


            case "V102":
                return objRecord.V102;


            case "V103":
                return objRecord.V103;


            case "V104":
                return objRecord.V104;


            case "V105":
                return objRecord.V105;


            case "V106":
                return objRecord.V106;


            case "V107":
                return objRecord.V107;


            case "V108":
                return objRecord.V108;


            case "V109":
                return objRecord.V109;


            case "V110":
                return objRecord.V110;

            case "V111":
                return objRecord.V111;


            case "V112":
                return objRecord.V112;


            case "V113":
                return objRecord.V113;


            case "V114":
                return objRecord.V114;


            case "V115":
                return objRecord.V115;


            case "V116":
                return objRecord.V116;


            case "V117":
                return objRecord.V117;


            case "V118":
                return objRecord.V118;


            case "V119":
                return objRecord.V119;


            case "V120":
                return objRecord.V120;

            case "V121":
                return objRecord.V121;


            case "V122":
                return objRecord.V122;


            case "V123":
                return objRecord.V123;


            case "V124":
                return objRecord.V124;


            case "V125":
                return objRecord.V125;


            case "V126":
                return objRecord.V126;


            case "V127":
                return objRecord.V127;


            case "V128":
                return objRecord.V128;


            case "V129":
                return objRecord.V129;


            case "V130":
                return objRecord.V130;

            case "V131":
                return objRecord.V131;


            case "V132":
                return objRecord.V132;


            case "V133":
                return objRecord.V133;


            case "V134":
                return objRecord.V134;


            case "V135":
                return objRecord.V135;


            case "V136":
                return objRecord.V136;


            case "V137":
                return objRecord.V137;


            case "V138":
                return objRecord.V138;


            case "V139":
                return objRecord.V139;


            case "V140":
                return objRecord.V140;

            case "V141":
                return objRecord.V141;


            case "V142":
                return objRecord.V142;


            case "V143":
                return objRecord.V143;


            case "V144":
                return objRecord.V144;


            case "V145":
                return objRecord.V145;


            case "V146":
                return objRecord.V146;


            case "V147":
                return objRecord.V147;


            case "V148":
                return objRecord.V148;


            case "V149":
                return objRecord.V149;


            case "V150":
                return objRecord.V150;




            case "V151":
                return objRecord.V151;


            case "V152":
                return objRecord.V152;


            case "V153":
                return objRecord.V153;


            case "V154":
                return objRecord.V154;


            case "V155":
                return objRecord.V155;


            case "V156":
                return objRecord.V156;


            case "V157":
                return objRecord.V157;


            case "V158":
                return objRecord.V158;


            case "V159":
                return objRecord.V159;


            case "V160":
                return objRecord.V160;




            case "V161":
                return objRecord.V161;


            case "V162":
                return objRecord.V162;


            case "V163":
                return objRecord.V163;


            case "V164":
                return objRecord.V164;


            case "V165":
                return objRecord.V165;


            case "V166":
                return objRecord.V166;


            case "V167":
                return objRecord.V167;


            case "V168":
                return objRecord.V168;


            case "V169":
                return objRecord.V169;


            case "V170":
                return objRecord.V170;




            case "V171":
                return objRecord.V171;


            case "V172":
                return objRecord.V172;


            case "V173":
                return objRecord.V173;


            case "V174":
                return objRecord.V174;


            case "V175":
                return objRecord.V175;


            case "V176":
                return objRecord.V176;


            case "V177":
                return objRecord.V177;


            case "V178":
                return objRecord.V178;


            case "V179":
                return objRecord.V179;


            case "V180":
                return objRecord.V180;




            case "V181":
                return objRecord.V181;


            case "V182":
                return objRecord.V182;


            case "V183":
                return objRecord.V183;


            case "V184":
                return objRecord.V184;


            case "V185":
                return objRecord.V185;


            case "V186":
                return objRecord.V186;


            case "V187":
                return objRecord.V187;


            case "V188":
                return objRecord.V188;


            case "V189":
                return objRecord.V189;


            case "V190":
                return objRecord.V190;




            case "V191":
                return objRecord.V191;


            case "V192":
                return objRecord.V192;


            case "V193":
                return objRecord.V193;


            case "V194":
                return objRecord.V194;


            case "V195":
                return objRecord.V195;


            case "V196":
                return objRecord.V196;


            case "V197":
                return objRecord.V197;


            case "V198":
                return objRecord.V198;


            case "V199":
                return objRecord.V199;


            case "V200":
                return objRecord.V200;


            case "V201":
                return objRecord.V201;


            case "V202":
                return objRecord.V202;


            case "V203":
                return objRecord.V203;


            case "V204":
                return objRecord.V204;


            case "V205":
                return objRecord.V205;


            case "V206":
                return objRecord.V206;


            case "V207":
                return objRecord.V207;


            case "V208":
                return objRecord.V208;


            case "V209":
                return objRecord.V209;


            case "V210":
                return objRecord.V210;

            case "V211":
                return objRecord.V211;


            case "V212":
                return objRecord.V212;


            case "V213":
                return objRecord.V213;


            case "V214":
                return objRecord.V214;


            case "V215":
                return objRecord.V215;


            case "V216":
                return objRecord.V216;


            case "V217":
                return objRecord.V217;


            case "V218":
                return objRecord.V218;


            case "V219":
                return objRecord.V219;


            case "V220":
                return objRecord.V220;

            case "V221":
                return objRecord.V221;


            case "V222":
                return objRecord.V222;


            case "V223":
                return objRecord.V223;


            case "V224":
                return objRecord.V224;


            case "V225":
                return objRecord.V225;


            case "V226":
                return objRecord.V226;


            case "V227":
                return objRecord.V227;


            case "V228":
                return objRecord.V228;


            case "V229":
                return objRecord.V229;


            case "V230":
                return objRecord.V230;

            case "V231":
                return objRecord.V231;


            case "V232":
                return objRecord.V232;


            case "V233":
                return objRecord.V233;


            case "V234":
                return objRecord.V234;


            case "V235":
                return objRecord.V235;


            case "V236":
                return objRecord.V236;


            case "V237":
                return objRecord.V237;


            case "V238":
                return objRecord.V238;


            case "V239":
                return objRecord.V239;


            case "V240":
                return objRecord.V240;

            case "V241":
                return objRecord.V241;


            case "V242":
                return objRecord.V242;


            case "V243":
                return objRecord.V243;


            case "V244":
                return objRecord.V244;


            case "V245":
                return objRecord.V245;


            case "V246":
                return objRecord.V246;


            case "V247":
                return objRecord.V247;


            case "V248":
                return objRecord.V248;


            case "V249":
                return objRecord.V249;


            case "V250":
                return objRecord.V250;




            case "V251":
                return objRecord.V251;


            case "V252":
                return objRecord.V252;


            case "V253":
                return objRecord.V253;


            case "V254":
                return objRecord.V254;


            case "V255":
                return objRecord.V255;


            case "V256":
                return objRecord.V256;


            case "V257":
                return objRecord.V257;


            case "V258":
                return objRecord.V258;


            case "V259":
                return objRecord.V259;


            case "V260":
                return objRecord.V260;




            case "V261":
                return objRecord.V261;


            case "V262":
                return objRecord.V262;


            case "V263":
                return objRecord.V263;


            case "V264":
                return objRecord.V264;


            case "V265":
                return objRecord.V265;


            case "V266":
                return objRecord.V266;


            case "V267":
                return objRecord.V267;


            case "V268":
                return objRecord.V268;


            case "V269":
                return objRecord.V269;


            case "V270":
                return objRecord.V270;




            case "V271":
                return objRecord.V271;


            case "V272":
                return objRecord.V272;


            case "V273":
                return objRecord.V273;


            case "V274":
                return objRecord.V274;


            case "V275":
                return objRecord.V275;


            case "V276":
                return objRecord.V276;


            case "V277":
                return objRecord.V277;


            case "V278":
                return objRecord.V278;


            case "V279":
                return objRecord.V279;


            case "V280":
                return objRecord.V280;




            case "V281":
                return objRecord.V281;


            case "V282":
                return objRecord.V282;


            case "V283":
                return objRecord.V283;


            case "V284":
                return objRecord.V284;


            case "V285":
                return objRecord.V285;


            case "V286":
                return objRecord.V286;


            case "V287":
                return objRecord.V287;


            case "V288":
                return objRecord.V288;


            case "V289":
                return objRecord.V289;


            case "V290":
                return objRecord.V290;




            case "V291":
                return objRecord.V291;


            case "V292":
                return objRecord.V292;


            case "V293":
                return objRecord.V293;


            case "V294":
                return objRecord.V294;


            case "V295":
                return objRecord.V295;


            case "V296":
                return objRecord.V296;


            case "V297":
                return objRecord.V297;


            case "V298":
                return objRecord.V298;


            case "V299":
                return objRecord.V299;


            case "V300":
                return objRecord.V300;


            case "V301":
                return objRecord.V301;


            case "V302":
                return objRecord.V302;


            case "V303":
                return objRecord.V303;


            case "V304":
                return objRecord.V304;


            case "V305":
                return objRecord.V305;


            case "V306":
                return objRecord.V306;


            case "V307":
                return objRecord.V307;


            case "V308":
                return objRecord.V308;


            case "V309":
                return objRecord.V309;


            case "V310":
                return objRecord.V310;

            case "V311":
                return objRecord.V311;


            case "V312":
                return objRecord.V312;


            case "V313":
                return objRecord.V313;


            case "V314":
                return objRecord.V314;


            case "V315":
                return objRecord.V315;


            case "V316":
                return objRecord.V316;


            case "V317":
                return objRecord.V317;


            case "V318":
                return objRecord.V318;


            case "V319":
                return objRecord.V319;


            case "V320":
                return objRecord.V320;

            case "V321":
                return objRecord.V321;


            case "V322":
                return objRecord.V322;


            case "V323":
                return objRecord.V323;


            case "V324":
                return objRecord.V324;


            case "V325":
                return objRecord.V325;


            case "V326":
                return objRecord.V326;


            case "V327":
                return objRecord.V327;


            case "V328":
                return objRecord.V328;


            case "V329":
                return objRecord.V329;


            case "V330":
                return objRecord.V330;

            case "V331":
                return objRecord.V331;


            case "V332":
                return objRecord.V332;


            case "V333":
                return objRecord.V333;


            case "V334":
                return objRecord.V334;


            case "V335":
                return objRecord.V335;


            case "V336":
                return objRecord.V336;


            case "V337":
                return objRecord.V337;


            case "V338":
                return objRecord.V338;


            case "V339":
                return objRecord.V339;


            case "V340":
                return objRecord.V340;

            case "V341":
                return objRecord.V341;


            case "V342":
                return objRecord.V342;


            case "V343":
                return objRecord.V343;


            case "V344":
                return objRecord.V344;


            case "V345":
                return objRecord.V345;


            case "V346":
                return objRecord.V346;


            case "V347":
                return objRecord.V347;


            case "V348":
                return objRecord.V348;


            case "V349":
                return objRecord.V349;


            case "V350":
                return objRecord.V350;




            case "V351":
                return objRecord.V351;


            case "V352":
                return objRecord.V352;


            case "V353":
                return objRecord.V353;


            case "V354":
                return objRecord.V354;


            case "V355":
                return objRecord.V355;


            case "V356":
                return objRecord.V356;


            case "V357":
                return objRecord.V357;


            case "V358":
                return objRecord.V358;


            case "V359":
                return objRecord.V359;


            case "V360":
                return objRecord.V360;




            case "V361":
                return objRecord.V361;


            case "V362":
                return objRecord.V362;


            case "V363":
                return objRecord.V363;


            case "V364":
                return objRecord.V364;


            case "V365":
                return objRecord.V365;


            case "V366":
                return objRecord.V366;


            case "V367":
                return objRecord.V367;


            case "V368":
                return objRecord.V368;


            case "V369":
                return objRecord.V369;


            case "V370":
                return objRecord.V370;




            case "V371":
                return objRecord.V371;


            case "V372":
                return objRecord.V372;


            case "V373":
                return objRecord.V373;


            case "V374":
                return objRecord.V374;


            case "V375":
                return objRecord.V375;


            case "V376":
                return objRecord.V376;


            case "V377":
                return objRecord.V377;


            case "V378":
                return objRecord.V378;


            case "V379":
                return objRecord.V379;


            case "V380":
                return objRecord.V380;




            case "V381":
                return objRecord.V381;


            case "V382":
                return objRecord.V382;


            case "V383":
                return objRecord.V383;


            case "V384":
                return objRecord.V384;


            case "V385":
                return objRecord.V385;


            case "V386":
                return objRecord.V386;


            case "V387":
                return objRecord.V387;


            case "V388":
                return objRecord.V388;


            case "V389":
                return objRecord.V389;


            case "V390":
                return objRecord.V390;




            case "V391":
                return objRecord.V391;


            case "V392":
                return objRecord.V392;


            case "V393":
                return objRecord.V393;


            case "V394":
                return objRecord.V394;


            case "V395":
                return objRecord.V395;


            case "V396":
                return objRecord.V396;


            case "V397":
                return objRecord.V397;


            case "V398":
                return objRecord.V398;


            case "V399":
                return objRecord.V399;


            case "V400":
                return objRecord.V400;

            case "V401":
                return objRecord.V401;


            case "V402":
                return objRecord.V402;


            case "V403":
                return objRecord.V403;


            case "V404":
                return objRecord.V404;


            case "V405":
                return objRecord.V405;


            case "V406":
                return objRecord.V406;


            case "V407":
                return objRecord.V407;


            case "V408":
                return objRecord.V408;


            case "V409":
                return objRecord.V409;


            case "V410":
                return objRecord.V410;

            case "V411":
                return objRecord.V411;


            case "V412":
                return objRecord.V412;


            case "V413":
                return objRecord.V413;


            case "V414":
                return objRecord.V414;


            case "V415":
                return objRecord.V415;


            case "V416":
                return objRecord.V416;


            case "V417":
                return objRecord.V417;


            case "V418":
                return objRecord.V418;


            case "V419":
                return objRecord.V419;


            case "V420":
                return objRecord.V420;

            case "V421":
                return objRecord.V421;


            case "V422":
                return objRecord.V422;


            case "V423":
                return objRecord.V423;


            case "V424":
                return objRecord.V424;


            case "V425":
                return objRecord.V425;


            case "V426":
                return objRecord.V426;


            case "V427":
                return objRecord.V427;


            case "V428":
                return objRecord.V428;


            case "V429":
                return objRecord.V429;


            case "V430":
                return objRecord.V430;

            case "V431":
                return objRecord.V431;


            case "V432":
                return objRecord.V432;


            case "V433":
                return objRecord.V433;


            case "V434":
                return objRecord.V434;


            case "V435":
                return objRecord.V435;


            case "V436":
                return objRecord.V436;


            case "V437":
                return objRecord.V437;


            case "V438":
                return objRecord.V438;


            case "V439":
                return objRecord.V439;


            case "V440":
                return objRecord.V440;

            case "V441":
                return objRecord.V441;


            case "V442":
                return objRecord.V442;


            case "V443":
                return objRecord.V443;


            case "V444":
                return objRecord.V444;


            case "V445":
                return objRecord.V445;


            case "V446":
                return objRecord.V446;


            case "V447":
                return objRecord.V447;


            case "V448":
                return objRecord.V448;


            case "V449":
                return objRecord.V449;


            case "V450":
                return objRecord.V450;




            case "V451":
                return objRecord.V451;


            case "V452":
                return objRecord.V452;


            case "V453":
                return objRecord.V453;


            case "V454":
                return objRecord.V454;


            case "V455":
                return objRecord.V455;


            case "V456":
                return objRecord.V456;


            case "V457":
                return objRecord.V457;


            case "V458":
                return objRecord.V458;


            case "V459":
                return objRecord.V459;


            case "V460":
                return objRecord.V460;




            case "V461":
                return objRecord.V461;


            case "V462":
                return objRecord.V462;


            case "V463":
                return objRecord.V463;


            case "V464":
                return objRecord.V464;


            case "V465":
                return objRecord.V465;


            case "V466":
                return objRecord.V466;


            case "V467":
                return objRecord.V467;


            case "V468":
                return objRecord.V468;


            case "V469":
                return objRecord.V469;


            case "V470":
                return objRecord.V470;




            case "V471":
                return objRecord.V471;


            case "V472":
                return objRecord.V472;


            case "V473":
                return objRecord.V473;


            case "V474":
                return objRecord.V474;


            case "V475":
                return objRecord.V475;


            case "V476":
                return objRecord.V476;


            case "V477":
                return objRecord.V477;


            case "V478":
                return objRecord.V478;


            case "V479":
                return objRecord.V479;


            case "V480":
                return objRecord.V480;




            case "V481":
                return objRecord.V481;


            case "V482":
                return objRecord.V482;


            case "V483":
                return objRecord.V483;


            case "V484":
                return objRecord.V484;


            case "V485":
                return objRecord.V485;


            case "V486":
                return objRecord.V486;


            case "V487":
                return objRecord.V487;


            case "V488":
                return objRecord.V488;


            case "V489":
                return objRecord.V489;


            case "V490":
                return objRecord.V490;




            case "V491":
                return objRecord.V491;


            case "V492":
                return objRecord.V492;


            case "V493":
                return objRecord.V493;


            case "V494":
                return objRecord.V494;


            case "V495":
                return objRecord.V495;


            case "V496":
                return objRecord.V496;


            case "V497":
                return objRecord.V497;


            case "V498":
                return objRecord.V498;


            case "V499":
                return objRecord.V499;


            case "V500":
                return objRecord.V500;



        }
        return "";


    }


   


    public static int ets_TableUser_Insert(TableUser p_TableUser)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TableUser_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                if (p_TableUser.ExceedanceEmail != null)
                    command.Parameters.Add(new SqlParameter("@bExceedanceEmail", p_TableUser.ExceedanceEmail));

                if (p_TableUser.ExceedanceSMS != null)
                    command.Parameters.Add(new SqlParameter("@bExceedanceSMS", p_TableUser.ExceedanceSMS));


                command.Parameters.Add(new SqlParameter("@nTableID", p_TableUser.TableID));
                command.Parameters.Add(new SqlParameter("@nUserID", p_TableUser.UserID));              
                command.Parameters.Add(new SqlParameter("@bLateWarningEmail", p_TableUser.LateWarningEmail));
                command.Parameters.Add(new SqlParameter("@bLateWarningSMS", p_TableUser.LateWarningSMS));

                command.Parameters.Add(new SqlParameter("@bUploadEmail", p_TableUser.UploadEmail));
                command.Parameters.Add(new SqlParameter("@bUploadSMS", p_TableUser.UploadSMS));

                command.Parameters.Add(new SqlParameter("@bUploadWarningEmail", p_TableUser.UploadWarningEmail));
                command.Parameters.Add(new SqlParameter("@bUploadWarningSMS", p_TableUser.UploadWarningSMS));

                if (p_TableUser.AddDataEmail != null)
                    command.Parameters.Add(new SqlParameter("@bAddDataEmail", p_TableUser.AddDataEmail));

                if (p_TableUser.AddDataSMS != null)
                    command.Parameters.Add(new SqlParameter("@bAddDataSMS", p_TableUser.AddDataSMS));

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

    public static int ets_TableUser_Update(TableUser p_TableUser)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TableUser_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                if (p_TableUser.ExceedanceEmail != null)
                    command.Parameters.Add(new SqlParameter("@bExceedanceEmail", p_TableUser.ExceedanceEmail));

                if (p_TableUser.ExceedanceSMS != null)
                    command.Parameters.Add(new SqlParameter("@bExceedanceSMS", p_TableUser.ExceedanceSMS));

                command.Parameters.Add(new SqlParameter("@nTableUserID", p_TableUser.TableUserID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_TableUser.TableID));
                command.Parameters.Add(new SqlParameter("@nUserID", p_TableUser.UserID));
               
                command.Parameters.Add(new SqlParameter("@bLateWarningEmail", p_TableUser.LateWarningEmail));
                command.Parameters.Add(new SqlParameter("@bLateWarningSMS", p_TableUser.LateWarningSMS));

                command.Parameters.Add(new SqlParameter("@bUploadEmail", p_TableUser.UploadEmail));
                command.Parameters.Add(new SqlParameter("@bUploadSMS", p_TableUser.UploadSMS));

                command.Parameters.Add(new SqlParameter("@bUploadWarningEmail", p_TableUser.UploadWarningEmail));
                command.Parameters.Add(new SqlParameter("@bUploadWarningSMS", p_TableUser.UploadWarningSMS));

                if(p_TableUser.AddDataEmail!=null)
                    command.Parameters.Add(new SqlParameter("@bAddDataEmail", p_TableUser.AddDataEmail));

                if( p_TableUser.AddDataSMS!=null)
                    command.Parameters.Add(new SqlParameter("@bAddDataSMS", p_TableUser.AddDataSMS));

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


    public static DataTable ets_TableUser_Select(int? nTableUserID, int? nTableID,
        int? nUserID, 
        bool? bLateWarningEmail, bool? bLateWarningSMS, bool? bUploadEmail,
        bool? bUploadSMS, bool? bUploadWarningEmail, bool? bUploadWarningSMS, bool? bAddDataEmail, bool? bAddDataSMS,
         bool? bExceedanceEmail, bool? bExceedanceSMS)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TableUser_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nTableUserID != null)
                    command.Parameters.Add(new SqlParameter("@nTableUserID", nTableUserID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));


                if (bExceedanceEmail != null)
                    command.Parameters.Add(new SqlParameter("@bExceedanceEmail", bExceedanceEmail));

                if (bExceedanceSMS != null)
                    command.Parameters.Add(new SqlParameter("@bExceedanceSMS", bExceedanceSMS));

                if (bLateWarningEmail != null)
                    command.Parameters.Add(new SqlParameter("@bLateWarningEmail", bLateWarningEmail));

                if (bLateWarningSMS != null)
                    command.Parameters.Add(new SqlParameter("@bLateWarningSMS", bLateWarningSMS));


                if (bUploadEmail != null)
                    command.Parameters.Add(new SqlParameter("@bUploadEmail", bUploadEmail));

                if (bUploadSMS != null)
                    command.Parameters.Add(new SqlParameter("@bUploadSMS", bUploadSMS));

                if (bUploadWarningEmail != null)
                    command.Parameters.Add(new SqlParameter("@bUploadWarningEmail", bUploadWarningEmail));

                if (bUploadWarningSMS != null)
                    command.Parameters.Add(new SqlParameter("@bUploadWarningSMS", bUploadWarningSMS));


                //if (bUploadWarningEmail != null)
                //    command.Parameters.Add(new SqlParameter("@bUploadWarningEmail", bUploadWarningEmail));

                //if (bUploadWarningSMS != null)
                //    command.Parameters.Add(new SqlParameter("@bUploadWarningSMS", bUploadWarningSMS));

                if (bAddDataEmail != null)
                    command.Parameters.Add(new SqlParameter("@bAddDataEmail", bAddDataEmail));

                if (bAddDataSMS != null)
                    command.Parameters.Add(new SqlParameter("@bAddDataSMS", bAddDataSMS));



              
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



    public static int ets_TableUser_Delete(int nTableUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TableUser_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableUserID", nTableUserID));

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


    public static TableUser ets_TableUser_Detail(int nTableUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TableUser_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableUserID", nTableUserID));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TableUser temp = new TableUser(
                                (int)reader["TableUserID"],
                                (int)reader["TableID"],
                                (int)reader["UserID"],
                            (bool)reader["LateWarningEmail"],
                            (bool)reader["LateWarningSMS"],
                            (bool)reader["UploadEmail"],
                            (bool)reader["UploadSMS"], (bool)reader["UploadWarningEmail"], (bool)reader["UploadWarningSMS"],
                            reader["AddDataEmail"] == DBNull.Value ? null : (bool?)reader["AddDataEmail"],
                            reader["AddDataSMS"] == DBNull.Value ? null : (bool?)reader["AddDataSMS"]
                            );

                            temp.ExceedanceEmail = reader["ExceedanceEmail"] == DBNull.Value ? null : (bool?)reader["ExceedanceEmail"];
                            temp.ExceedanceSMS = reader["ExceedanceSMS"] == DBNull.Value ? null : (bool?)reader["ExceedanceSMS"];

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



    



  








    public static DataTable ets_Notification_Select(int? nTableID,
 int? nAccountID, DateTime? dDateFrom, DateTime? dDateTo,
     string sOrder,
string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, string sTableIn)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Notification_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));


                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));
                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));


                if (sOrder == "")
                    sOrder = "ST.TableID";//no use

                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                if (sTableIn != "")
                    command.Parameters.Add(new SqlParameter("@sTableIn", sTableIn));


                
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






    public static DateTime? ets_GetMaxDateTimeRecorded(int nTableID, int nRecordID,  DateTime DateTimeRecorded)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GetMaxDateTimeRecorded", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                command.Parameters.Add(new SqlParameter("@nRecordID", nRecordID));
                //command.Parameters.Add(new SqlParameter("@nLocationID", nLocationID));
                command.Parameters.Add(new SqlParameter("@DateTimeRecorded", DateTimeRecorded));

                //connection.Open();
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

                if (dt!=null)
                {
                    if (dt.Rows[0][0].ToString() != "")
                    {
                        return DateTime.Parse(dt.Rows[0][0].ToString());
                    }
                }
                {
                    return null;
                }
            }

        }



        

    }


    public static bool IsTimeBetweenRecordOK(int nTableID, double? dMaxTimeBetweenRecords, string strMaxTimeUnit, int nRecordID, DateTime dateDateTimeRecorded)
    {
        DateTime? datetimeMAX = ets_GetMaxDateTimeRecorded(nTableID, nRecordID,  dateDateTimeRecorded);

        if (datetimeMAX == null)
        {
            return true;
        }
        else
        {
            // [Table] theTable = ets_Table_Details(nTableID,ref connection,ref tn);

            if (dMaxTimeBetweenRecords == null)
            {
                return true;
            }
            else
            {
                DateTime datetimeDummy = (DateTime)datetimeMAX;
                switch (strMaxTimeUnit.ToLower())
                {
                    case "days":
                        datetimeDummy = datetimeDummy.AddDays((int)dMaxTimeBetweenRecords);
                        break;
                    case "hours":
                        datetimeDummy = datetimeDummy.AddHours((int)dMaxTimeBetweenRecords);
                        break;
                    case "minutes":
                        datetimeDummy = datetimeDummy.AddMinutes((int)dMaxTimeBetweenRecords);
                        break;
                }


                if (datetimeDummy >= dateDateTimeRecorded)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }


        }


    }


    //public static bool IsTimeBetweenRecordOK(int nTableID, double? dMaxTimeBetweenRecords, string strMaxTimeUnit, int nRecordID, DateTime dateDateTimeRecorded)
    //{
    //    DateTime? datetimeMAX = ets_GetMaxDateTimeRecorded(nTableID, nRecordID, dateDateTimeRecorded);

    //    if (datetimeMAX == null)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        // [Table] theTable = ets_Table_Details(nTableID,ref connection,ref tn);

    //        if (dMaxTimeBetweenRecords == null)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            DateTime datetimeDummy = (DateTime)datetimeMAX;
    //            switch (strMaxTimeUnit.ToLower())
    //            {
    //                case "days":
    //                    datetimeDummy = datetimeDummy.AddDays((int)dMaxTimeBetweenRecords);
    //                    break;
    //                case "hours":
    //                    datetimeDummy = datetimeDummy.AddHours((int)dMaxTimeBetweenRecords);
    //                    break;
    //                case "minutes":
    //                    datetimeDummy = datetimeDummy.AddMinutes((int)dMaxTimeBetweenRecords);
    //                    break;
    //            }


    //            if (datetimeDummy >= dateDateTimeRecorded)
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }

    //        }


    //    }


    //}


    public static bool IsTimeBetweenRecordOK(int nTableID, int nRecordID, DateTime dateDateTimeRecorded)
    {
        DateTime? datetimeMAX = ets_GetMaxDateTimeRecorded(nTableID, nRecordID,  dateDateTimeRecorded);

        if (datetimeMAX == null)
        {
            return true;
        }
        else
        {
            Table theTable = ets_Table_Details(nTableID);

            if (theTable.MaxTimeBetweenRecords == null)
            {
                return true;
            }
            else
            {
                DateTime datetimeDummy = (DateTime)datetimeMAX;
                switch (theTable.MaxTimeBetweenRecordsUnit.ToLower())
                {
                    case "days":
                        datetimeDummy = datetimeDummy.AddDays((double)theTable.MaxTimeBetweenRecords);
                        break;
                    case "hours":
                        datetimeDummy = datetimeDummy.AddHours((double)theTable.MaxTimeBetweenRecords);
                        break;
                    case "minutes":
                        datetimeDummy = datetimeDummy.AddMinutes((double)theTable.MaxTimeBetweenRecords);
                        break;
                }


                if (datetimeDummy >= dateDateTimeRecorded)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }


        }


    }

//    public static void SendDataWanrningSMSandEmail(int iColumnID, string strValue, string strDateTimeRecorded, ref string strErrorMsg, int iAccountID, string strURL)
//    {

//        Content theContentEmail = SystemData.Content_Details_ByKey("DataWarningEmail",(int)iAccountID);
//        Content theContentSMS = SystemData.Content_Details_ByKey("DataWarningSMS",(int)iAccountID);
//        if (theContentEmail == null && theContentSMS == null)
//        {
//            return;
//        }
//        Column theColumn = RecordManager.ets_Column_Details(iColumnID);
//        Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);

//        string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
//        string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
//        string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
//        string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
//        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey("WarningSMSEmail");

//        string strClickHere = "<a href='" + strURL + "'>Click Here</a>";


//        string strBody = theContentEmail.ContentP;

//        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
//        strBody = strBody.Replace("[Table]", theTable.TableName);
//        strBody = strBody.Replace("[Record]", strClickHere);
//        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
//        strBody = strBody.Replace("[Value]", strValue);
//        if (theColumn.ValidationOnWarning != "")
//        {
//            strBody = strBody.Replace("[WarningIf]", theColumn.ValidationOnWarning);
//        }



//        MailMessage msg = new MailMessage();
//        msg.From = new MailAddress(strEmail);


//        msg.Subject = theTable.TableName + " -- " + theContentEmail.Heading;
//        msg.IsBodyHtml = true;

//        msg.Body = strBody;// Sb.ToString();
//        //msg.To.Add(Email.Text);
//        SmtpClient smtpClient = new SmtpClient(strEmailServer);
//        smtpClient.Timeout = 99999;
//        smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
//        smtpClient.Port = DBGurus.StringToInt(DBGurus.GetSystemOption("SmtpPort"));
//        smtpClient.EnableSsl = Convert.ToBoolean(DBGurus.GetSystemOption("EnableSSL"));


//        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
//         (int)theTable.TableID, null, null, null, null, null, true, null);

//        foreach (DataRow dr in dtUsersEmail.Rows)
//        {
//            msg.To.Clear();
//            msg.To.Add(dr["Email"].ToString());
//            try
//            {


//#if (!DEBUG)
//                smtpClient.Send(msg);
//#endif


//            }
//            catch (Exception)
//            {

//                strErrorMsg = "Server could not send warning Email & SMS";
//            }


//        }




//        //SMS part
//        strBody = theContentSMS.ContentP;

//        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
//        strBody = strBody.Replace("[Table]", theTable.TableName);
//        strBody = strBody.Replace("[Record]", strClickHere);
//        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
//        strBody = strBody.Replace("[Value]", strValue);
//        if (theColumn.ValidationOnWarning != "")
//        {
//            strBody = strBody.Replace("[WarningIf]", theColumn.ValidationOnWarning);
//        }



//        msg = new MailMessage();
//        msg.From = new MailAddress(strEmail);


//        msg.Subject = theTable.TableName + " -- " + theContentSMS.Heading;
//        msg.IsBodyHtml = true;

//        msg.Body = strBody;// Sb.ToString();


//        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
//     (int)theTable.TableID, null, null, null, null, null, null, true);

//        foreach (DataRow dr in dtUsersSMS.Rows)
//        {
//            msg.To.Clear();
//            if (dr["PhoneNumber"] != DBNull.Value)
//            {
//                if (dr["PhoneNumber"].ToString() != "")
//                {
//                    msg.To.Add(dr["PhoneNumber"].ToString() + strWarningSMSEMail);
//                    try
//                    {


//#if (!DEBUG)
//                        smtpClient.Send(msg);
//#endif



//                    }
//                    catch (Exception)
//                    {

//                        strErrorMsg = "Server could not send warning Email & SMS";
//                    }
//                }
//            }
//        }

//    }


    public static void SendDataWanrningSMSandEmailBatch(int iTableID, ref string strErrorMsg, int iAccountID, 
        string strEmailFullBody, string strSMSFullBody, int iColumnCount,int iRecordID)
    {

        Content theContentEmail = SystemData.Content_Details_ByKey("DataWarningEmail", (int)iAccountID);
        Content theContentSMS = SystemData.Content_Details_ByKey("DataWarningSMS", (int)iAccountID);
        if (theContentEmail == null && theContentSMS == null)
        {
            return;
        }

        Table theTable = RecordManager.ets_Table_Details(iTableID);
        Record theRecord = RecordManager.ets_Record_Detail_Full(iRecordID);
     
        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail",null,iTableID);
               

        string strBody =strEmailFullBody;            

        string strSubject = theTable.TableName + " -- " + iColumnCount.ToString() + " column(s) - " + theContentEmail.Heading;
       
        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
         (int)theTable.TableID, null, null, null, null, null, true, null, null, null, null, null);

        foreach (DataRow dr in dtUsersEmail.Rows)
        {            
            try
            {
                string strTo = dr["Email"].ToString();
                //Guid guidNew = Guid.NewGuid();
                //string strEmailUID = guidNew.ToString();

                //EmailLog theEmailLog = new EmailLog(null, iAccountID, strSubject,
                //  strTo, DateTime.Now, iTableID,
                //  iRecordID,
                //  theContentEmail.ContentKey, strBody);
                //theEmailLog.EmailUID = strEmailUID;
                string strError = "";


                Message theMessage = new Message(null, iRecordID, iTableID, iAccountID, DateTime.Now, "W", "E",
                            null, strTo, strSubject, strBody, null, "");


                DBGurus.SendEmail(theContentEmail.ContentKey, true, null, strSubject, strBody, "", strTo, "", "", null, theMessage, out strError);

                //if (SystemData.SystemOption_ValueByKey_Account("EmailAttachments",null,iTableID) == "Yes")
                //    {
                //        try
                //        {
                //            if (theTable.JSONAttachmentInfo != "")
                //            {
                //                AttachmentSetting theAttachmentSetting = JSONField.GetTypedObject<AttachmentSetting>(theTable.JSONAttachmentInfo);
                //                if (theAttachmentSetting.AttachOutgoingEmails != null)
                //                {
                //                    if ((bool)theAttachmentSetting.AttachOutgoingEmails)
                //                    {
                //                        if (theAttachmentSetting.OutSavetoTableID != null)
                //                        {
                //                            Record theChildRecord = new Record();
                //                            theChildRecord.TableID = (int)theAttachmentSetting.OutSavetoTableID;
                //                            theChildRecord.EnteredBy = theRecord.EnteredBy;

                //                            //link the record with the parent table

                //                            DataTable dtChildColumn = Common.DataTableFromText("SELECT * FROM [Column] WHERE TableID=" + theChildRecord.TableID.ToString() + " AND TableTableID=" + theTable.TableID.ToString());

                //                            if (dtChildColumn.Rows.Count > 0)
                //                            {
                //                                Column lnkColumn = RecordManager.ets_Column_Details(int.Parse(dtChildColumn.Rows[0]["LinkedParentColumnID"].ToString()));
                //                                RecordManager.MakeTheRecord(ref theChildRecord, dtChildColumn.Rows[0]["SystemName"].ToString(), RecordManager.GetRecordValue(ref theRecord, lnkColumn.SystemName));

                //                            }

                //                            if (theAttachmentSetting.OutSaveRecipientColumnID != null)
                //                            {
                //                                Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveRecipientColumnID);

                //                                RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strTo);
                //                            }

                //                            if (theAttachmentSetting.OutSaveSubjectColumnID != null)
                //                            {
                //                                Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveSubjectColumnID);

                //                                RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strSubject);
                //                            }

                //                            if (theAttachmentSetting.OutSaveBodyColumnID != null)
                //                            {
                //                                Column aColumn = RecordManager.ets_Column_Details((int)theAttachmentSetting.OutSaveBodyColumnID);

                //                                RecordManager.MakeTheRecord(ref theChildRecord, aColumn.SystemName, strBody);
                //                            }


                //                            try
                //                            {
                //                                RecordManager.ets_Record_Insert(theChildRecord);
                //                            }
                //                            catch
                //                            {
                //                                //

                //                            }

                //                        }

                //                    }

                //                }

                //            }
                //        }
                //        catch
                //        {
                //            //
                //        }


                //    }







                //}

            }
            catch (Exception)
            {

                strErrorMsg = "Server could not send warning Email & SMS";
            }


        }




        //SMS part
        strBody =strSMSFullBody;         

        
        strSubject = theTable.TableName + " -- " + iColumnCount.ToString() + " column(s) - " + theContentSMS.Heading;
       

        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
     (int)theTable.TableID, null, null, null, null, null, null, true, null, null, null, null);

        foreach (DataRow dr in dtUsersSMS.Rows)
        {
            //msg.To.Clear();
            if (dr["PhoneNumber"] != DBNull.Value)
            {
                if (dr["PhoneNumber"].ToString() != "")
                {
                    try
                    {
                        

                        //Guid guidNew = Guid.NewGuid();
                        //string strEmailUID = guidNew.ToString();
                        string strTo = dr["PhoneNumber"].ToString() + strWarningSMSEMail;
                        //EmailLog theEmailLog = new EmailLog(null, iAccountID, strSubject,
                        //  strTo, DateTime.Now, iTableID,
                        //  iRecordID,
                        //  theContentSMS.ContentKey, strBody);
                        //theEmailLog.EmailUID = strEmailUID;
                       
                        string strError = "";


                        Message theMessage = new Message(null, iRecordID, iTableID, iAccountID, DateTime.Now, "W", "S",
                            null, strTo, strSubject, strBody, null, "");

                        DBGurus.SendEmail(theContentSMS.ContentKey, null, true, strSubject, strBody, "", strTo, "", "", null, theMessage, out strError);
                        

                    }
                    catch (Exception)
                    {

                        strErrorMsg = "Server could not send warning Email & SMS";
                    }
                }
            }
        }

    }



    public static void SendDataExceedanceSMSandEmailBatch(int iTableID, ref string strErrorMsg, int iAccountID,
       string strEmailFullBody, string strSMSFullBody, int iColumnCount, int iRecordID)
    {

        Content theContentEmail = SystemData.Content_Details_ByKey("DataExceedanceEmail", (int)iAccountID);
        Content theContentSMS = SystemData.Content_Details_ByKey("DataExceedanceSMS", (int)iAccountID);
        if (theContentEmail == null && theContentSMS == null)
        {
            return;
        }

        Table theTable = RecordManager.ets_Table_Details(iTableID);
        Record theRecord = RecordManager.ets_Record_Detail_Full(iRecordID);


        string strWarningSMSEmail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail", null, iTableID);



        string strBody = strEmailFullBody;


        
        string strSubject = theTable.TableName + " -- " + iColumnCount.ToString() + " column(s) - " + theContentEmail.Heading;
        

        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
         (int)theTable.TableID, null, null, null, null, null, null, null, null, null, true, null);

        foreach (DataRow dr in dtUsersEmail.Rows)
        {
          
            try
            {
                string strTo = dr["Email"].ToString();
                //Guid guidNew = Guid.NewGuid();
                //string strEmailUID = guidNew.ToString();

                //EmailLog theEmailLog = new EmailLog(null, iAccountID, strSubject,
                //  strTo, DateTime.Now, iTableID,
                //  iRecordID,
                //  theContentEmail.ContentKey, strBody);
                //theEmailLog.EmailUID = strEmailUID;
                //EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);
                string strError = "";


                Message theMessage = new Message(null, iRecordID, iTableID, iAccountID, DateTime.Now, "W", "E",
                           null, strTo, strSubject, strBody, null, "");

                DBGurus.SendEmail(theContentEmail.ContentKey, true, null, strSubject, strBody, "", strTo, "", "", null, theMessage, out strError);

              
                
            }
            catch (Exception)
            {

                strErrorMsg = "Server could not send Exceedance Email & SMS";
            }


        }




        //SMS part
        strBody = strSMSFullBody;

        
        strSubject = theTable.TableName + " -- " + iColumnCount.ToString() + " column(s) - " + theContentSMS.Heading;
       

        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
     (int)theTable.TableID, null, null, null, null, null, null, null, null, null, null, true);

        foreach (DataRow dr in dtUsersSMS.Rows)
        {
            //msg.To.Clear();
            if (dr["PhoneNumber"] != DBNull.Value)
            {
                if (dr["PhoneNumber"].ToString() != "")
                {
                    try
                    {


                        //Guid guidNew = Guid.NewGuid();
                        //string strEmailUID = guidNew.ToString();
                        string strTo = dr["PhoneNumber"].ToString() + strWarningSMSEmail;
                        //EmailLog theEmailLog = new EmailLog(null, iAccountID, strSubject,
                        //  strTo, DateTime.Now, iTableID,
                        //  iRecordID,
                        //  theContentSMS.ContentKey, strBody);
                        //theEmailLog.EmailUID = strEmailUID;
                        
                        string strError = "";

                        Message theMessage = new Message(null, iRecordID, iTableID, iAccountID, DateTime.Now, "W", "S",
                        null, strTo, strSubject, strBody, null, "");

                        DBGurus.SendEmail(theContentSMS.ContentKey, null, true, strSubject, strBody, "", strTo, "", "", null, theMessage, out strError);


                    }
                    catch (Exception)
                    {

                        strErrorMsg = "Server could not send Exceedance Email & SMS";
                    }
                }
            }
        }

    }

    public static void BuildDataWanrningSMSandEmail(int iColumnID, string strValue, string strDateTimeRecorded, ref string strErrorMsg, int iAccountID, 
        string strURL, ref string strEmailFullBody,ref string strSMSFullBody, ref int iColumnCount)
    {

        iColumnCount = iColumnCount + 1;
        Content theContentEmail = SystemData.Content_Details_ByKey("DataWarningEmail", (int)iAccountID);
        Content theContentSMS = SystemData.Content_Details_ByKey("DataWarningSMS", (int)iAccountID);
        if (theContentEmail == null && theContentSMS == null)
        {
            return;
        }
        Column theColumn = RecordManager.ets_Column_Details(iColumnID);
        Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);

    

        string strClickHere = "<a href='" + strURL + "'>Click Here</a>";
        
        string strBody = theContentEmail.ContentP;

        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
        strBody = strBody.Replace("[Table]", theTable.TableName);
        strBody = strBody.Replace("[Record]", strClickHere);
        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
        strBody = strBody.Replace("[Value]", strValue);
        string strConW = "";
        if (theColumn.ValidationOnWarning != "")
        {
            strConW = Common.GetFromulaMsg("", theColumn.DisplayName, theColumn.ValidationOnWarning);
            //strBody = strBody.Replace("[WarningIf]", theColumn.ValidationOnWarning);
        }
        else
        {
            strConW = UploadWorld.Condition_GetFormulaHTMLTable(theColumn, "W", "");

          
        }

        strBody = strBody.Replace("[WarningIf]", strConW);
        strEmailFullBody = strEmailFullBody + "</br>" + strBody;
      


        //SMS part
        strBody = theContentSMS.ContentP;

        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
        strBody = strBody.Replace("[Table]", theTable.TableName);
        strBody = strBody.Replace("[Record]", strClickHere);
        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
        strBody = strBody.Replace("[Value]", strValue);
        strBody = strBody.Replace("[WarningIf]", strConW);
        
        //if (theColumn.ValidationOnWarning != "")
        //{
        //    strBody = strBody.Replace("[WarningIf]", theColumn.ValidationOnWarning);
        //}
        //else
        //{
        //    strBody = strBody.Replace("[WarningIf]", strConW);
        //}


        strSMSFullBody = strSMSFullBody + "</br>" + strBody;
        


        

    }




    public static void BuildDataExceedanceSMSandEmail(int iColumnID, string strValue, string strDateTimeRecorded, ref string strErrorMsg, int iAccountID,
        string strURL, ref string strEmailFullBody, ref string strSMSFullBody, ref int iColumnCount)
    {

        iColumnCount = iColumnCount + 1;
        Content theContentEmail = SystemData.Content_Details_ByKey("DataExceedanceEmail", (int)iAccountID);
        Content theContentSMS = SystemData.Content_Details_ByKey("DataExceedanceSMS", (int)iAccountID);
        if (theContentEmail == null && theContentSMS == null)
        {
            return;
        }
        Column theColumn = RecordManager.ets_Column_Details(iColumnID);
        Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);



        string strClickHere = "<a href='" + strURL + "'>Click Here</a>";

        string strBody = theContentEmail.ContentP;

        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
        strBody = strBody.Replace("[Table]", theTable.TableName);
        strBody = strBody.Replace("[Record]", strClickHere);
        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
        strBody = strBody.Replace("[Value]", strValue);

        string strConE = "";
        if (theColumn.ValidationOnExceedance != "")
        {
            strConE = Common.GetFromulaMsg("", theColumn.DisplayName, theColumn.ValidationOnExceedance);
            //strBody = strBody.Replace("[ExceedanceIf]",Common.GetFromulaMsg("e",theColumn.DisplayName, theColumn.ValidationOnExceedance));
        }
        else
        {
            //it must be conditions
            
             strConE = UploadWorld.Condition_GetFormulaHTMLTable(theColumn,  "E", "");          
            
           
        }
        strBody = strBody.Replace("[ExceedanceIf]", strConE);
        strEmailFullBody = strEmailFullBody + "</br>" + strBody;



        //SMS part
        strBody = theContentSMS.ContentP;

        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
        strBody = strBody.Replace("[Table]", theTable.TableName);
        strBody = strBody.Replace("[Record]", strClickHere);
        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
        strBody = strBody.Replace("[Value]", strValue);
        strBody = strBody.Replace("[ExceedanceIf]", strConE);

        strSMSFullBody = strSMSFullBody + "</br>" + strBody;
        


    }

    public static void SendAdjustWanrningSMSandEmail(Column theColumn, double dNoOfRecords, ref string strErrorMsg, bool IsCalculationChanged)
    {

        Content theContent = SystemData.Content_Details_ByKey("AdjustWarningEmail",null);
        if (theContent == null)
        {
            return;
        }
        Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);


        //string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
        //string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
        //string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
        //string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail",null,theColumn.TableID);




        string strBody = theContent.ContentP;


        strBody = strBody.Replace("[Table]", theTable.TableName);
        //strBody = strBody.Replace("[Record]", "?");
        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
        strBody = strBody.Replace("[NoOfRecords]", dNoOfRecords.ToString());
        strBody = strBody.Replace("[NoOfRows]", dNoOfRecords.ToString());
        string strConW = "";
        if (theColumn.ValidationOnWarning != "")
        {
            //strBody = strBody.Replace("[WarningIf]", theColumn.ValidationOnWarning);
            strConW = Common.GetFromulaMsg("", theColumn.DisplayName, theColumn.ValidationOnWarning);
        }
        else
        {
            //it must be conditions
            strConW = UploadWorld.Condition_GetFormulaHTMLTable(theColumn, "W", "");

           

        }

        strBody = strBody.Replace("[WarningIf]", strConW);
        //MailMessage msg = new MailMessage();
        //msg.From = new MailAddress(strEmail);

        string strSubject = "";
        if (IsCalculationChanged)
        {
            strSubject = theTable.TableName + " -- " + "Calculation formula has been changed and new warning Records has been created!";
        }
        else
        {
            strSubject = theTable.TableName + " -- " + theContent.Heading;
        }
        //msg.IsBodyHtml = true;

        //msg.Body = strBody;// Sb.ToString();
        
        //SmtpClient smtpClient = new SmtpClient(strEmailServer);
        //smtpClient.Timeout = 99999;
        //smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

        //smtpClient.Port = DBGurus.StringToInt(DBGurus.GetSystemOption("SmtpPort"));
        //smtpClient.EnableSsl = Convert.ToBoolean(DBGurus.GetSystemOption("EnableSSL"));


        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
         (int)theTable.TableID, null, null, null, null, null, true, null, null, null, null, null);

        foreach (DataRow dr in dtUsersEmail.Rows)
        {
            //msg.To.Clear();
            //msg.To.Add(dr["Email"].ToString());
            try
            {
               

#if (!DEBUG)
                //smtpClient.Send(msg);
#endif
                //if (msg.To.Count > 0)
                //{
                //    if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                //    {
                string strTo = dr["Email"].ToString();
                //Guid guidNew = Guid.NewGuid();
                //string strEmailUID = guidNew.ToString();

                //EmailLog theEmailLog = new EmailLog(null, int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()),
                //    strSubject,
                //  strTo, DateTime.Now, theColumn.TableID,
                //  theColumn.ColumnID,
                //  "Adjust Warning Email", strBody);
                //theEmailLog.EmailUID = strEmailUID;
                //        EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);

                //    }
                //}


                string sSendEmailError = "";

                Message theMessage = new Message(null, null, theColumn.TableID, (int)theTable.AccountID,
                    DateTime.Now, "W", "E",
                        null, strTo, strSubject, strBody, null, "");

                DBGurus.SendEmail("Adjust Warning Email", true, null, strSubject, strBody, "", strTo, "", "", null, theMessage, out sSendEmailError);

                //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                //{

                //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
                //}

            }
            catch (Exception)
            {

                strErrorMsg = "Server could not send warning Email & SMS";
            }


        }


        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
     (int)theTable.TableID, null, null, null, null, null, null, true, null, null, null, null);

        foreach (DataRow dr in dtUsersSMS.Rows)
        {
            //msg.To.Clear();
            if (dr["PhoneNumber"] != DBNull.Value)
            {
                if (dr["PhoneNumber"].ToString() != "")
                {
                    //msg.To.Add(dr["PhoneNumber"].ToString() + strWarningSMSEMail);
                    try
                    {


#if (!DEBUG)
                        //smtpClient.Send(msg);
#endif
                        



                        //if (msg.To.Count > 0)
                        //{
                        //    if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                        //    {
                        string strTo=dr["PhoneNumber"].ToString() + strWarningSMSEMail;
                        //Guid guidNew = Guid.NewGuid();
                        //string strEmailUID = guidNew.ToString();

                        //EmailLog theEmailLog = new EmailLog(null, int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()),
                        //    strSubject,
                        // strTo, DateTime.Now, theColumn.TableID,
                        //  theColumn.ColumnID,
                        //  "Adjust Warning SMS", strBody);
                        //theEmailLog.EmailUID = strEmailUID;
                      


                        string sSendEmailError = "";

                        Message theMessage = new Message(null, null, theColumn.TableID, (int)theTable.AccountID,
                    DateTime.Now, "W", "S",
                        null, strTo, strSubject, strBody, null, "");  

                        DBGurus.SendEmail("Adjust Warning SMS", null, true, strSubject, strBody, "",
                            strTo, "", "", null, theMessage, out sSendEmailError);

                        //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                        //{

                        //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), null, true, null, null);
                        //}

                    }
                    catch (Exception)
                    {

                        strErrorMsg = "Server could not send warning Email & SMS";
                    }
                }
            }
        }

    }


    public static void SendAdjustExceedanceSMSandEmail(Column theColumn, double dNoOfRecords, ref string strErrorMsg, bool IsCalculationChanged)
    {

        Content theContent = SystemData.Content_Details_ByKey("AdjustExceedanceEmail", null);
        if (theContent == null)
        {
            return;
        }
        Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);

        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail", null, theColumn.TableID);




        string strBody = theContent.ContentP;


        strBody = strBody.Replace("[Table]", theTable.TableName);
        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
        strBody = strBody.Replace("[NoOfRecords]", dNoOfRecords.ToString());
        strBody = strBody.Replace("[NoOfRows]", dNoOfRecords.ToString());
        string strConE = "";
        if (theColumn.ValidationOnExceedance != "")
        {
            //strBody = strBody.Replace("[ExceedanceIf]", theColumn.ValidationOnExceedance);
            strConE = Common.GetFromulaMsg("", theColumn.DisplayName, theColumn.ValidationOnExceedance);
        }
        else
        {
            //it must be conditions
             strConE = UploadWorld.Condition_GetFormulaHTMLTable(theColumn, "E", "");

            //if (strConE=="")
            //{
            //    strBody = strBody.Replace("[ExceedanceIf]", "");
            //}
            //else
            //{
            //    strBody = strBody.Replace("[ExceedanceIf]", strConE);
            //}
            
        }

        strBody = strBody.Replace("[ExceedanceIf]", strConE);

        string strSubject = "";
        if (IsCalculationChanged)
        {
            strSubject = theTable.TableName + " -- " + "Calculation formula has been changed and new exceedance Records has been created!";
        }
        else
        {
            strSubject = theTable.TableName + " -- " + theContent.Heading;
        }
       

        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
         (int)theTable.TableID, null, null, null, null, null, null, null, null, null, true, null);

        foreach (DataRow dr in dtUsersEmail.Rows)
        {
           
            try
            {


                string strTo = dr["Email"].ToString();
                //Guid guidNew = Guid.NewGuid();
                //string strEmailUID = guidNew.ToString();

                //EmailLog theEmailLog = new EmailLog(null, int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()),
                //    strSubject,
                //  strTo, DateTime.Now, theColumn.TableID,
                //  theColumn.ColumnID,
                //  "Adjust Exceedance Email", strBody);
                //theEmailLog.EmailUID = strEmailUID;
               

                string sSendEmailError = "";



                Message theMessage = new Message(null, null, theColumn.TableID, (int)theTable.AccountID,
                    DateTime.Now, "W", "E",
                        null, strTo, strSubject, strBody, null, "");

                DBGurus.SendEmail("Adjust Exceedance Email", true, null, strSubject, strBody, "", strTo, "", "", null, theMessage, out sSendEmailError);

              

            }
            catch (Exception)
            {

                strErrorMsg = "Server could not send Exceedance Email & SMS";
            }


        }


        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
     (int)theTable.TableID, null, null, null, null, null, null, null, null, null, null, true);

        foreach (DataRow dr in dtUsersSMS.Rows)
        {
            //msg.To.Clear();
            if (dr["PhoneNumber"] != DBNull.Value)
            {
                if (dr["PhoneNumber"].ToString() != "")
                {
                    try
                    {
                        
                        string strTo = dr["PhoneNumber"].ToString() + strWarningSMSEMail;
                        //Guid guidNew = Guid.NewGuid();
                        //string strEmailUID = guidNew.ToString();

                        //EmailLog theEmailLog = new EmailLog(null, int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()),
                        //    strSubject,
                        // strTo, DateTime.Now, theColumn.TableID,
                        //  theColumn.ColumnID,
                        //  "Adjust Exceedance SMS", strBody);
                        //theEmailLog.EmailUID = strEmailUID;
                       

                        string sSendEmailError = "";

                        Message theMessage = new Message(null, null, theColumn.TableID, (int)theTable.AccountID,
                    DateTime.Now, "W", "S",
                        null, strTo, strSubject, strBody, null, "");  

                        DBGurus.SendEmail("Adjust Exceedance SMS", null, true, strSubject, strBody, "",
                            strTo, "", "", null, theMessage, out sSendEmailError);

                       
                    }
                    catch (Exception)
                    {

                        strErrorMsg = "Server could not send Exceedance Email & SMS";
                    }
                }
            }
        }

    }


//    public static void SendValidationSMSandEmail(int iColumnID, string strValue, string strDateTimeRecorded, ref string strErrorMsg)
//    {

//        Content theContent = SystemData.Content_Details_ByKey("ValidationEmail",null);
//        if (theContent == null)
//        {
//            return;
//        }
//        Column theColumn = RecordManager.ets_Column_Details(iColumnID);
//        Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);




//        string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
//        string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
//        string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
//        string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
//        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey("WarningSMSEmail");




//        string strBody = theContent.ContentP;

//        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
//        strBody = strBody.Replace("[Table]", theTable.TableName);
//        //strBody = strBody.Replace("[Record]", "?");
//        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
//        strBody = strBody.Replace("[Value]", strValue);
//        if (theColumn.ValidationOnEntry != "")
//        {
//            strBody = strBody.Replace("[ValidIf]", theColumn.ValidationOnEntry);
//        }



//        MailMessage msg = new MailMessage();
//        msg.From = new MailAddress(strEmail);


//        msg.Subject = theTable.TableName + " -- " + theContent.Heading;
//        msg.IsBodyHtml = true;

//        msg.Body = strBody;// Sb.ToString();
//        //msg.To.Add(Email.Text);
//        SmtpClient smtpClient = new SmtpClient(strEmailServer);
//        smtpClient.Timeout = 99999;
//        smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
//        smtpClient.Port = DBGurus.StringToInt(DBGurus.GetSystemOption("SmtpPort"));
//        smtpClient.EnableSsl = Convert.ToBoolean(DBGurus.GetSystemOption("EnableSSL"));


//        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
//         (int)theTable.TableID, null, null, null, null, null, true, null);

//        foreach (DataRow dr in dtUsersEmail.Rows)
//        {
//            msg.To.Clear();
//            msg.To.Add(dr["Email"].ToString());
//            try
//            {



//#if (!DEBUG)
//                smtpClient.Send(msg);
//#endif


//            }
//            catch (Exception)
//            {

//                strErrorMsg = "Server could not send validation Email & SMS";
//            }


//        }


//        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
//     (int)theTable.TableID, null, null, null, null, null, null, true);

//        foreach (DataRow dr in dtUsersSMS.Rows)
//        {
//            msg.To.Clear();
//            if (dr["PhoneNumber"] != DBNull.Value)
//            {
//                if (dr["PhoneNumber"].ToString() != "")
//                {
//                    msg.To.Add(dr["PhoneNumber"].ToString() + strWarningSMSEMail);
//                    try
//                    {


//#if (!DEBUG)
//                        smtpClient.Send(msg);
//#endif


//                    }
//                    catch (Exception)
//                    {

//                        strErrorMsg = "Server could not send validation Email & SMS";
//                    }
//                }
//            }
//        }

//    }





    public static void SendMaxTimeWanrningSMSandEmail(int iTableID, string strDateTimeRecorded, int iAccountID, ref string strErrorMsg, string strURL)
    {

        Content theContentEmail = SystemData.Content_Details_ByKey("GapWarningEmail",(int)iAccountID);
        Content theContentSMS = SystemData.Content_Details_ByKey("GapWarningSMS",(int)iAccountID);
        if (theContentEmail == null && theContentSMS == null)
        {
            return;
        }
        Table theTable = RecordManager.ets_Table_Details(iTableID);

        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail",null,iTableID);

        string strClickHere = "<a href='" + strURL + "'>Click Here</a>";
        string strBody = theContentEmail.ContentP;

        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
        strBody = strBody.Replace("[Table]", theTable.TableName);
        strBody = strBody.Replace("[Record]", strClickHere);

        strBody = strBody.Replace("[MaxTime]", theTable.MaxTimeBetweenRecords.ToString() + " " + theTable.MaxTimeBetweenRecordsUnit);
                
        string strSubject=theContentEmail.Heading.Replace("[Table]", theTable.TableName);
        
        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
         (int)theTable.TableID, null, null, null, null, null, true, null, null, null, null, null);

        foreach (DataRow dr in dtUsersEmail.Rows)
        {
        
            try
            {

               
#if (!DEBUG)
                //smtpClient.Send(msg);
#endif

                //if (msg.To.Count > 0)
                //{
                //Guid guidNew = Guid.NewGuid();
                //string strEmailUID = guidNew.ToString();

                //EmailLog theEmailLog = new EmailLog(null, iAccountID, strSubject,
                //  dr["Email"].ToString(), DateTime.Now, iTableID,
                //  null,
                //  "Max Time Warning Email", strBody);
                //theEmailLog.EmailUID = strEmailUID;
                    //EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);


                //}



                string sSendEmailError = "";


                Message theMessage = new Message(null, null, iTableID, iAccountID,
                   DateTime.Now, "W", "E",
                       null, dr["Email"].ToString(), strSubject, strBody, null, "");


                DBGurus.SendEmail("Max Time Warning Email", true, null, strSubject, strBody, "", dr["Email"].ToString(), "", "", null, theMessage, out sSendEmailError);


                //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                //{

                //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
                //}


            }
            catch (Exception)
            {

                strErrorMsg = "Server could not send max time warning Email & SMS";
            }


        }


        //SMS part


        strBody = theContentSMS.ContentP;

        strBody = strBody.Replace("[DateTime]", strDateTimeRecorded);
        strBody = strBody.Replace("[Table]", theTable.TableName);
        strBody = strBody.Replace("[Record]", strClickHere);

        strBody = strBody.Replace("[MaxTime]", theTable.MaxTimeBetweenRecords.ToString() + " " + theTable.MaxTimeBetweenRecordsUnit);


        //msg = new MailMessage();
        //msg.From = new MailAddress(strEmail);


        strSubject = theContentSMS.Heading.Replace("[Table]", theTable.TableName);
        //msg.IsBodyHtml = true;

        //msg.Body = strBody;// Sb.ToString();

        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
     (int)theTable.TableID, null, null, null, null, null, null, true, null, null, null, null);

        foreach (DataRow dr in dtUsersSMS.Rows)
        {
            //msg.To.Clear();
            if (dr["PhoneNumber"] != DBNull.Value)
            {
                if (dr["PhoneNumber"].ToString() != "")
                {
                    //msg.To.Add(dr["PhoneNumber"].ToString() + strWarningSMSEMail);
                    try
                    {
                        
#if (!DEBUG)
                        //smtpClient.Send(msg);
#endif

                        //if (msg.To.Count > 0)
                        //{
                        //Guid guidNew = Guid.NewGuid();
                        //string strEmailUID = guidNew.ToString();

                        //EmailLog theEmailLog = new EmailLog(null, iAccountID, strSubject,
                        //   dr["PhoneNumber"].ToString() + strWarningSMSEMail, DateTime.Now, iTableID,
                        //  null,
                        //  "Max Time Warning SMS", strBody);
                        //theEmailLog.EmailUID = strEmailUID;
                        //    EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);


                        //}

                        Message theMessage = new Message(null, null, iTableID, iAccountID,
                  DateTime.Now, "W", "S",
                      null, dr["PhoneNumber"].ToString() + strWarningSMSEMail, strSubject, strBody, null, "");  

                        string sSendEmailError = "";

                        DBGurus.SendEmail("Max Time Warning SMS", null, true, strSubject, strBody, "",
                            dr["PhoneNumber"].ToString() + strWarningSMSEMail, "", "", null, theMessage, out sSendEmailError);

                        

                    }
                    catch (Exception)
                    {

                        strErrorMsg = "Server could not send max time warning Email & SMS";
                    }
                }
            }
        }

    }




    public static DataTable ets_GetListOfRecordsByOneSys(string sSystemName, int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GetListOfRecordsByOneSys", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@sSystemName", sSystemName));

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));




                
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



    //public static DataTable ets_GetListOfRecordsByOneSys(string sSystemName, int nTableID, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_GetListOfRecordsByOneSys", connection, tn))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        command.Parameters.Add(new SqlParameter("@sSystemName", sSystemName));

    //        command.Parameters.Add(new SqlParameter("@nTableID", nTableID));




    //        //connection.Open();
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = command;
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
    //        return dt;

    //    }

    //}






    //public static void ets_AdjustAvgNewColumn(Column theColumn, ref SqlConnection connection, ref SqlTransaction tn, ref string strErrorMsg)
    //{

    //    if (theColumn.AvgColumnID != null)
    //    {
    //        //we have a new calculation :)

    //        Column avgColumn = RecordManager.ets_Column_Details((int)theColumn.AvgColumnID, ref connection, ref tn);


    //        DataTable dtDatas = Common.DataTableFromText("SELECT RecordID," + avgColumn.SystemName + 
    //            ",DateTimeRecorded FROM Record WHERE IsActive=1 AND TableID=" + avgColumn.TableID.ToString() +
    //            " ORDER BY DateTimeRecorded", tn, null);
            
    //        if (dtDatas.Rows.Count > 0)
    //        {

    //            foreach (DataRow dr in dtDatas.Rows)
    //            {

                   
    //                    //ok its an Avg
    //                    DateTime dtAvgFrom = (DateTime)dr["DateTimeRecorded"];
    //                    DateTime dtAvgTo = (DateTime)dr["DateTimeRecorded"];

    //                    //switch (theColumn.AvgPeriodUnit.ToString())
    //                    //{
    //                    //    case "D":
    //                    //        dtAvgFrom = dtAvgTo.AddDays(-double.Parse(theColumn.AvgPeriodAmt.ToString()));
    //                    //        break;

    //                    //    case "W":
    //                    //        dtAvgFrom = dtAvgTo.AddDays(-7 * double.Parse(theColumn.AvgPeriodAmt.ToString()));
    //                    //        break;

    //                    //    case "M":
    //                    //        dtAvgFrom = dtAvgTo.AddMonths(-int.Parse(theColumn.AvgPeriodAmt.ToString()));
    //                    //        break;
    //                    //}


    //                    double dAvgValue = 0;

    //                    //dAvgValue = RecordManager.ets_Record_Avg((int)theColumn.TableID, (int)theColumn.AvgColumnID,
    //                    //        dtAvgFrom, dtAvgTo, tn, null);

    //                    if (dAvgValue == 0)
    //                    {
    //                        if (dr[avgColumn.SystemName] != DBNull.Value && dr[avgColumn.SystemName].ToString() != "")
    //                        {
    //                            dAvgValue = double.Parse(dr[avgColumn.SystemName].ToString());

    //                        }

    //                    }

    //                    Common.ExecuteText("UPDATE Record SET " + theColumn.SystemName + "='" + dAvgValue.ToString() + "' WHERE RecordID=" + dr["RecordID"].ToString(), tn);


                    

                  
    //            }

    //        }

    //    }

    //}




    public static string ets_AdjustCalculationFormulaChanges(Column theColumn,  ref string strErrorMsg)
    {
        string strCalculationType = "";
        DateTime dtStartTime= DateTime.Now;
        int iNoOfRecords = 0;
        try
        {

            string strReturn = "";
            int iInvalid = 0;
            int iExceedance = 0;
            int iWarning = 0;

            string strTemp="";
            if (theColumn.ColumnType != "calculation")
            {
                return "";
            }

            if (theColumn.Calculation == "")
            {
                Common.ExecuteText("UPDATE Record SET " + theColumn.SystemName + "=null WHERE TableID=" + theColumn.TableID.ToString());
            }
            else
            {
                //we have a new/updated calculation :)

                string strRC = Common.GetValueFromSQL("SELECT COUNT( RecordID) FROM [Record] WHERE IsActive=1 AND TableID=" + theColumn.TableID.ToString());

                if (string.IsNullOrEmpty(strRC) || strRC == "0")
                {
                    return "";
                }

                //

                Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);

                string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", theTable.AccountID, theTable.TableID);
                bool bShowExceedances = false;

                if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
                {
                    bShowExceedances = true;
                }

                bool bCheckIgnoreMidnight = false;
                string strIgnoreMidnight = SystemData.SystemOption_ValueByKey_Account("Time Calculation Ignore Midnight", (int)theTable.AccountID, theTable.TableID);

                if (strIgnoreMidnight != "" && strIgnoreMidnight.ToString().ToLower() == "yes")
                {
                    bCheckIgnoreMidnight = true;
                }

                DataTable dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theColumn.TableID);
                //List<int> lstRecordIDs = TheDatabaseS.ListOfIDs("SELECT RecordID FROM [Record] WHERE IsActive=1 AND TableID="+theColumn.TableID.ToString()+" ORDER BY RecordID ASC");
                List<int> lstRecordIDs = TheDatabaseS.ListIDsByCOALESCE((int)theColumn.TableID);
                if (theColumn.TextType == "d")
                {
                    //date
                    strCalculationType = "Date calculation-" + strCalculationType;
                    foreach(int iRecordID in lstRecordIDs)
                    {
                        Record aRecord = RecordManager.ets_Record_Detail_Full(iRecordID);
                        
                            if(aRecord!=null)
                            {
                                try
                                {
                                    string strNewValue = TheDatabaseS.GetDateCalculationResult(ref dtColumnsAll, theColumn.Calculation, null, aRecord, null,
                                                                               theColumn.DateCalculationType,
                                                                               null,theTable,bCheckIgnoreMidnight);

                                    RecordManager.MakeTheRecord(ref aRecord, theColumn.SystemName, strNewValue);
                                    RecordManager.ets_Record_Update(aRecord, null);
                                    iNoOfRecords = iNoOfRecords + 1;
                                }
                                catch
                                {
                                    //
                                }

                                
                            }
                        
                    }
                }
                else if (theColumn.TextType == "t")
                {
                    //date
                    strCalculationType = "Text calculation-" + strCalculationType;
                    string strFormula = Common.GetCalculationSystemNameOnly(theColumn.Calculation, (int)theTable.TableID);

                    foreach (int iRecordID in lstRecordIDs)
                    {
                        Record aRecord = RecordManager.ets_Record_Detail_Full(iRecordID);

                        if (aRecord != null)
                        {
                            try
                            {
                                string strNewValue = TheDatabaseS.GetTextCalculationResult(ref dtColumnsAll, strFormula, null, aRecord, null, null, theTable, theColumn);
                                    
                                RecordManager.MakeTheRecord(ref aRecord, theColumn.SystemName, strNewValue);
                                RecordManager.ets_Record_Update(aRecord, null);
                                iNoOfRecords = iNoOfRecords + 1;
                            }
                            catch
                            {
                                //
                            }


                        }

                    }
                }
                else
                {
                    //number
                    strCalculationType = "Number calculation-" + strCalculationType;
                    string strFormula = Common.GetCalculationSystemNameOnly(theColumn.Calculation, (int)theTable.TableID);
                    foreach (int iRecordID in lstRecordIDs)
                    {
                        Record aRecord = RecordManager.ets_Record_Detail_Full(iRecordID);
                        
                            if (aRecord != null)
                            {
                                try
                                {

                                            string strNewValue = TheDatabaseS.GetCalculationResult(ref dtColumnsAll, strFormula, null, aRecord, null, null, theTable, theColumn);
                                    
                                   
                                            //if(theColumn.ValidationOnEntry.Length>0)
                                            //{
                                            //    if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnEntry, ref strTemp))
                                            //    {
                                            //        aRecord.IsActive = false;
                                            //        iInvalid = iInvalid + 1;
                                            //        if (aRecord.ValidationResults.IndexOf("INVALID:" + theColumn.DisplayName) > -1)
                                            //        {
                                            //            //
                                            //        }
                                            //        else
                                            //        {
                                            //            aRecord.ValidationResults = aRecord.ValidationResults + " INVALID:" + theColumn.DisplayName;
                                            //        }
                                                    
                                            //    }
                                            //    else
                                            //    {
                                                  
                                            //            aRecord.ValidationResults = aRecord.ValidationResults.Replace("INVALID:" + theColumn.DisplayName, "");
                                            //    }
                                            //}
                                            //bool bEachColumnExceedance = false;
                                            //if (bShowExceedances && theColumn.ValidationOnExceedance!="")
                                            //{
                                            //    if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnExceedance, ref strTemp))
                                            //    {
                                            //        iExceedance = iExceedance + 1;
                                            //        if (aRecord.WarningResults.IndexOf("EXCEEDANCE: " + theColumn.DisplayName)>-1)
                                            //        {
                                            //            //old
                                            //        }
                                            //        else
                                            //        {
                                            //            aRecord.WarningResults = aRecord.WarningResults + " EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.";
                                            //        }
                                                    

                                            //        bEachColumnExceedance = true;                                                   
                                            //        aRecord.WarningResults = aRecord.WarningResults.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                                            //    }
                                            //    else
                                            //    {
                                                  
                                            //            aRecord.WarningResults = aRecord.WarningResults.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.", "");

                                            //    }
                                            //}

                                            //if (bEachColumnExceedance==false && theColumn.ValidationOnWarning != "")
                                            //{

                                            //    if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnWarning, ref strTemp))
                                            //    {
                                            //        iWarning = iWarning + 1;
                                            //        if (aRecord.WarningResults.IndexOf("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.") > -1)
                                            //        {
                                            //           //old
                                            //        }
                                            //        else
                                            //        {
                                            //            aRecord.WarningResults = aRecord.WarningResults + " WARNING: " + theColumn.DisplayName + " – Value outside accepted range.";
                                            //        }

                                            //    }
                                            //    else
                                            //    {                                                 
                                            //            aRecord.WarningResults = aRecord.WarningResults.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                                            //    }
                                            //}


                                            RecordManager.MakeTheRecord(ref aRecord, theColumn.SystemName, strNewValue);




                                            RecordManager.ets_Record_Update(aRecord, null);
                                            iNoOfRecords = iNoOfRecords + 1;
                                                                             

                                    //}
                                }
                                catch
                                {
                                    //
                                }

                               
                            }
                        
                    }

                    //
                    string strInvalid="";
                    ets_AdjustValidFormulaChanges(theColumn,ref strInvalid,true);
                    string strExceedance="";
                    ets_AdjustExceedanceFormulaChanges(theColumn, ref strExceedance, true);
                    string strWarning = "";
                    ets_AdjustWarningFormulaChanges(theColumn, ref strWarning, true);

                   
                    try
                    {
                        if (strInvalid != "")
                            iInvalid = int.Parse(strInvalid);
                        if (strExceedance != "")
                            iExceedance = int.Parse(strExceedance);
                        if (strWarning != "")
                            iWarning = int.Parse(strWarning);
                    }
                    catch
                    {
                        //
                    }
                }
            }

            strReturn = strReturn + " Total:" + iNoOfRecords.ToString() + " records; ";
            if(iInvalid>0)
            {
                strReturn = strReturn+ " InValid("+iInvalid.ToString()+") ";
            }
            if (iExceedance > 0)
            {
                strReturn =strReturn+ " Exceedance(" + iExceedance.ToString() + ") ";
            }
            if (iWarning > 0)
            {
                strReturn =strReturn+ " Warning(" + iWarning.ToString() + ")";
            }

            strReturn = strReturn + "--- Time:" + (DateTime.Now - dtStartTime).TotalSeconds.ToString("N1") + " seconds.";
            ///////////

            //ErrorLog theTimeLog = new ErrorLog(null, "AdjustCalculation", iNoOfRecords.ToString() + "Records --" + strCalculationType + "--- Time:" + (DateTime.Now-dtStartTime).TotalSeconds.ToString("N1"), strReturn, DateTime.Now, "");
            //SystemData.ErrorLog_Insert(theTimeLog);

            ////////////



            return strReturn;
        }
        catch(Exception ex)
        {

            //ErrorLog theTimeLog = new ErrorLog(null, "AdjustCalculation", strCalculationType + "--- Time:" + (DateTime.Now - dtStartTime).TotalSeconds.ToString("N1"), ex.Message + "---" + ex.StackTrace, 
            //    DateTime.Now, "");
            //SystemData.ErrorLog_Insert(theTimeLog);

            return "error";
        }
        

    }



    //public static string ets_AdjustCalculationFormulaChanges2(Column theColumn, ref string strMessage, ref string strErrorMsg)
    //{
    //    string strCalculationType = "";
    //    DateTime dtStartTime = DateTime.Now;
    //    int iNoOfRecords = 0;
    //    try
    //    {

    //        string strReturn = "";
    //        int iInvalid = 0;
    //        int iExceedance = 0;
    //        int iWarning = 0;

    //        string strTemp = "";
    //        if (theColumn.ColumnType != "calculation")
    //        {
    //            return "";
    //        }

    //        if (theColumn.Calculation == "")
    //        {
    //            Common.ExecuteText("UPDATE Record SET " + theColumn.SystemName + "=null WHERE TableID=" + theColumn.TableID.ToString());
    //        }
    //        else
    //        {
    //            //we have a new/updated calculation :)

    //            string strRC = Common.GetValueFromSQL("SELECT COUNT( RecordID) FROM [Record] WHERE IsActive=1 AND TableID=" + theColumn.TableID.ToString());

    //            if (string.IsNullOrEmpty(strRC) || strRC == "0")
    //            {
    //                return "";
    //            }

    //            //

    //            Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);

    //            string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", theTable.AccountID, theTable.TableID);
    //            bool bShowExceedances = false;

    //            if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
    //            {
    //                bShowExceedances = true;
    //            }

    //           bool bCheckIgnoreMidnight = false;
    //            string strIgnoreMidnight = SystemData.SystemOption_ValueByKey_Account("Time Calculation Ignore Midnight", (int)theTable.AccountID, theTable.TableID);

    //            if (strIgnoreMidnight != "" && strIgnoreMidnight.ToString().ToLower() == "yes")
    //            {
    //                bCheckIgnoreMidnight = true;
    //            }



    //            DataTable dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theColumn.TableID);
    //            //List<int> lstRecordIDs = TheDatabaseS.ListOfIDs("SELECT RecordID FROM [Record] WHERE IsActive=1 AND TableID=" + theColumn.TableID.ToString() + " ORDER BY RecordID ASC");
    //            List<int> lstRecordIDs = TheDatabaseS.ListIDsByCOALESCE((int)theColumn.TableID);
    //            if (theColumn.TextType == "d")
    //            {
    //                //date
    //                strCalculationType = "Date calculation-" + strCalculationType;
    //                foreach (int iRecordID in lstRecordIDs)
    //                {
    //                    Record aRecord = RecordManager.ets_Record_Detail_Full(iRecordID);

    //                    if (aRecord != null)
    //                    {
    //                        try
    //                        {
    //                            string strNewValue = TheDatabaseS.GetDateCalculationResult(ref dtColumnsAll, theColumn.Calculation, null, aRecord, null,
    //                                                                       theColumn.DateCalculationType,
    //                                                                       null, theTable, bCheckIgnoreMidnight);

    //                            //string strOldValue = RecordManager.GetRecordValue(ref aRecord, theColumn.SystemName);

    //                            //if (strNewValue != strOldValue)
    //                            //{
    //                            RecordManager.MakeTheRecord(ref aRecord, theColumn.SystemName, strNewValue);
    //                            RecordManager.ets_Record_Update(aRecord);
    //                            iNoOfRecords = iNoOfRecords + 1;
    //                            //}
    //                        }
    //                        catch
    //                        {
    //                            //
    //                        }


    //                    }

    //                }
    //            }
    //            else
    //            {
    //                //number
    //                strCalculationType = "Number calculation-" + strCalculationType;

    //                string strFormula = Common.GetCalculationSystemNameOnly(theColumn.Calculation, (int)theTable.TableID);

    //                foreach (int iRecordID in lstRecordIDs)
    //                {
    //                    Record aRecord = RecordManager.ets_Record_Detail_Full(iRecordID);

    //                    if (aRecord != null)
    //                    {
    //                        try
    //                        {
    //                            //string strFormula = TheDatabaseS.GetCalculationFormula((int)theColumn.TableID, theColumn.Calculation, null);

    //                            string strNewValue = TheDatabaseS.GetCalculationResult(ref dtColumnsAll, strFormula, null, aRecord, null, null, theTable, theColumn);

    //                            //string strOldValue = RecordManager.GetRecordValue(ref aRecord, theColumn.SystemName);

    //                            //if (strNewValue != strOldValue)
    //                            //{
    //                            //bool bCanUpdate = false;
    //                            //if (strNewValue!="")
    //                            //{
    //                            //    double dNewValue = 0;
    //                            //    double dOldValue = 0;
    //                            //    if(double.TryParse(strNewValue,out dNewValue))
    //                            //    {
    //                            //        bCanUpdate = true;
    //                            //        if (double.TryParse(strOldValue, out dOldValue))
    //                            //        {
    //                            //            if(dNewValue==dOldValue)
    //                            //            {
    //                            //                bCanUpdate = false;
    //                            //            }
    //                            //        }
    //                            //    }
    //                            //}
    //                            //else
    //                            //{
    //                            //    bCanUpdate = true;
    //                            //}

    //                            bool bCanUpdate = true;

    //                            if (bCanUpdate)
    //                            {
    //                                if (theColumn.ValidationOnEntry.Length > 0)
    //                                {
    //                                    //if (strNewValue!="" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnEntry, ref strTemp, theColumn.IgnoreSymbols == null ? false : (bool)theColumn.IgnoreSymbols, null))
    //                                    if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnEntry, ref strTemp))
    //                                    {
    //                                        aRecord.IsActive = false;
    //                                        iInvalid = iInvalid + 1;
    //                                        if (aRecord.ValidationResults.IndexOf("INVALID:" + theColumn.DisplayName) > -1)
    //                                        {
    //                                            //
    //                                        }
    //                                        else
    //                                        {
    //                                            aRecord.ValidationResults = aRecord.ValidationResults + " INVALID:" + theColumn.DisplayName;
    //                                        }

    //                                    }
    //                                    else
    //                                    {
    //                                            aRecord.ValidationResults = aRecord.ValidationResults.Replace("INVALID:" + theColumn.DisplayName, "");
    //                                    }
    //                                }
    //                                bool bEachColumnExceedance = false;
    //                                if (bShowExceedances && theColumn.ValidationOnExceedance != "")
    //                                {
    //                                    //if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnExceedance, ref strTemp, theColumn.IgnoreSymbols == null ? false : (bool)theColumn.IgnoreSymbols, null))
    //                                    if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnExceedance, ref strTemp))
    //                                    {
    //                                        iExceedance = iExceedance + 1;
    //                                        if (aRecord.WarningResults.IndexOf("EXCEEDANCE: " + theColumn.DisplayName) > -1)
    //                                        {
    //                                            //old
    //                                        }
    //                                        else
    //                                        {
    //                                            aRecord.WarningResults = aRecord.WarningResults + " EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.";
    //                                        }


    //                                        bEachColumnExceedance = true;

    //                                         aRecord.WarningResults = aRecord.WarningResults.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.", "");
    //                                    }
    //                                    else
    //                                    {
    //                                            aRecord.WarningResults = aRecord.WarningResults.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.", "");

    //                                    }
    //                                }

    //                                if (bEachColumnExceedance == false && theColumn.ValidationOnWarning != "")
    //                                {
    //                                    aRecord.WarningResults = aRecord.WarningResults.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.", "");

    //                                    //if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnWarning, ref strTemp, theColumn.IgnoreSymbols == null ? false : (bool)theColumn.IgnoreSymbols, null))
    //                                    if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnWarning, ref strTemp))
    //                                    {
    //                                        iWarning = iWarning + 1;
    //                                        if (aRecord.WarningResults.IndexOf("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.") > -1)
    //                                        {
    //                                            //old
    //                                        }
    //                                        else
    //                                        {
    //                                            aRecord.WarningResults = aRecord.WarningResults + " WARNING: " + theColumn.DisplayName + " – Value outside accepted range.";
    //                                        }

    //                                    }
    //                                    else
    //                                    {
                                            
    //                                            aRecord.WarningResults = aRecord.WarningResults.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                                                                                       
                                               
    //                                    }
    //                                }


    //                                RecordManager.MakeTheRecord(ref aRecord, theColumn.SystemName, strNewValue);
    //                                RecordManager.ets_Record_Update(aRecord);
    //                                iNoOfRecords = iNoOfRecords + 1;
    //                            }

    //                            //}
    //                        }
    //                        catch
    //                        {
    //                            //
    //                        }


    //                    }

    //                }

    //            }
    //        }

    //        if (iInvalid > 0)
    //        {
    //            strReturn = " InValid(" + iInvalid.ToString() + ") ";
    //        }
    //        if (iExceedance > 0)
    //        {
    //            strReturn = strReturn + " Exceedance(" + iExceedance.ToString() + ") ";
    //        }
    //        if (iWarning > 0)
    //        {
    //            strReturn = strReturn + " Warning(" + iWarning.ToString() + ")";
    //        }


    //        ///////////

    //        ErrorLog theTimeLog = new ErrorLog(null, "AdjustCalculation", iNoOfRecords.ToString() + "Records --" + strCalculationType + "--- Time:" + (DateTime.Now - dtStartTime).TotalSeconds.ToString("N1"), strReturn, DateTime.Now, "");
    //        SystemData.ErrorLog_Insert(theTimeLog);

    //        ////////////



    //        return strReturn;
    //    }
    //    catch (Exception ex)
    //    {

    //        ErrorLog theTimeLog = new ErrorLog(null, "AdjustCalculation", strCalculationType + "--- Time:" + (DateTime.Now - dtStartTime).TotalSeconds.ToString("N1"), ex.Message + "---" + ex.StackTrace,
    //            DateTime.Now, "");
    //        SystemData.ErrorLog_Insert(theTimeLog);

    //        return "error";
    //    }


    //}
    //DataTable dtDatas = ets_Records_Numeric_Data((int)theColumn.TableID);

    //if (dtDatas.Rows.Count > 0)
    //{

    //    string strTempFormula = "";
    //    string strResult = "";
    //    foreach (DataRow dr in dtDatas.Rows)
    //    {
    //        //process a record
    //        strTempFormula = theColumn.Calculation.ToLower();
    //        for (int i = 0; i < dtDatas.Columns.Count; i++)
    //        {
    //            if (dtDatas.Columns[i].ColumnName.ToLower() != "Record ID".ToLower())
    //            {
    //                strTempFormula = strTempFormula.Replace("[" + dtDatas.Columns[i].ColumnName.ToLower() + "]", Common.MakeDecimal(Common.IgnoreSymbols(dr[i].ToString())));
    //            }

    //        }
    //        strResult = RecordManager.CalculationResult(strTempFormula);

    //        if (strResult == "")
    //        {
    //            Common.ExecuteText("UPDATE Record SET " + theColumn.SystemName + "=null WHERE RecordID=" + dr["Record ID"].ToString());
    //        }
    //        else
    //        {
    //            Common.ExecuteText("UPDATE Record SET " + theColumn.SystemName + "=" + strResult + "  WHERE RecordID=" + dr["Record ID"].ToString());
    //        }
    //    }

    //}


    //public static DataTable ets_Records_Numeric_Data(int nTableID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Records_Numeric_Data", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


    //            command.Parameters.Add(new SqlParameter("@nTableID", nTableID));


    //            connection.Open();
    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            System.Data.DataSet ds = new System.Data.DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();

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




    //public static DataTable ets_Records_Numeric_Data(int nTableID, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_Records_Numeric_Data", connection, tn))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


    //        command.Parameters.Add(new SqlParameter("@nTableID", nTableID));


    //        // connection.Open();
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = command;
    //        DataTable dt = new DataTable();
    //        System.Data.DataSet ds = new System.Data.DataSet();
    //        da.Fill(ds);

    //        if (ds.Tables.Count > 0)
    //        {
    //            return ds.Tables[0];
    //        }
    //        {
    //            return null;
    //        }
    //    }

    //}



    public static string CalculationResult(string strFormula)
    {

        strFormula = ETSMath.RefineEvaluationString(strFormula);
        // build the class using codedom
        ETSMath.BuildClass(strFormula);

        // compile the class into an in-memory assembly.
        // if it doesn't compile, show errors in the window
        CompilerResults results = ETSMath.CompileAssembly();

        //Console.WriteLine("...........................\r\n");
        //Console.WriteLine(_source.ToString());

        // if the code compiled okay,
        // run the code using the new assembly (which is inside the results)
        if (results != null && results.CompiledAssembly != null)
        {
            // run the evaluation function
            return ETSMath.RunCode(results);
        }
        else
        {
            return "";
        }
    }


    public static void ets_AdjustValidFormulaChanges(Column theColumn, ref string strErrorMsg, bool IsCalculationChanged)
    {
        string strTemp = "";
        string strTempValid = "";

        DataTable dtRecords = ets_GetListOfRecordsByOneSys(theColumn.SystemName, (int)theColumn.TableID);
        double dNoOfRecords = 0;
        if (dtRecords.Rows.Count > 0)
        {
            if (theColumn.ValidationOnEntry != "")
            {

                foreach (DataRow dr in dtRecords.Rows)
                {
                    if (dr[theColumn.SystemName].ToString() != "")
                    {

                        strTempValid = dr["ValidationResults"].ToString();

                        strTempValid = strTempValid.Replace("INVALID: " + theColumn.DisplayName , "");
                        strTempValid = strTempValid.Replace("INVALID:" + theColumn.DisplayName, "");

                        if (!UploadManager.IsDataValid(dr[theColumn.SystemName].ToString(), theColumn.ValidationOnEntry, ref strTemp))
                        {
                            //Warning
                            strTempValid = strTempValid + " INVALID: " + theColumn.DisplayName;
                            dNoOfRecords = dNoOfRecords + 1;
                        }

                        if (strTempValid.Trim().Length > 0)
                        {
                            Common.ExecuteText("UPDATE Record SET IsActive=0, ValidationResults='" + strTempValid.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                        }
                        else
                        {
                            Common.ExecuteText("UPDATE Record SET ValidationResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                        }
                    }
                }
            }
            else
            {
                string strCheckColumnID = Common.GetValueFromSQL("SELECT TOP 1 CheckColumnID FROM [Condition] WHERE ConditionType='W' AND ColumnID=" + theColumn.ColumnID.ToString());

                if (strCheckColumnID != "")
                {
                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(strCheckColumnID));
                    if (theCheckColumn != null)
                    {
                        foreach (DataRow dr in dtRecords.Rows)
                        {
                            if (dr[theColumn.SystemName].ToString() != "")
                            {
                                string strEachFormula = "";

                                Record vRecord = RecordManager.ets_Record_Detail_Full(int.Parse(dr["RecordID"].ToString()));
                                string strCheckValue = RecordManager.GetRecordValue(ref vRecord, theCheckColumn.SystemName);
                                strEachFormula = UploadWorld.Condition_GetFormula((int)theColumn.ColumnID, theCheckColumn.ColumnID,
                                      "V", strCheckValue);

                                strTempValid = dr["ValidationResults"].ToString();

                                strTempValid = strTempValid.Replace("INVALID: " + theColumn.DisplayName, "");
                                strTempValid = strTempValid.Replace("INVALID:" + theColumn.DisplayName, "");

                                if (!UploadManager.IsDataValid(dr[theColumn.SystemName].ToString(), strEachFormula, ref strTemp))
                                {
                                    //Warning
                                    strTempValid = strTempValid + " INVALID: " + theColumn.DisplayName;
                                    dNoOfRecords = dNoOfRecords + 1;
                                }

                                if (strTempValid.Trim().Length > 0)
                                {
                                    Common.ExecuteText("UPDATE Record SET IsActive=0, ValidationResults='" + strTempValid.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                                }
                                else
                                {
                                    Common.ExecuteText("UPDATE Record SET ValidationResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                                }

                            }
                        }

                    }
                }
            }



            //lets send email
            if (IsCalculationChanged == true)
                strErrorMsg = dNoOfRecords.ToString();



        }

    }
    public static void ets_AdjustWarningFormulaChanges(Column theColumn, ref string strErrorMsg, bool IsCalculationChanged)
    {
        string strTemp = "";
        string strTempWarning = "";

        DataTable dtRecords = ets_GetListOfRecordsByOneSys(theColumn.SystemName, (int)theColumn.TableID);
        double dNoOfRecords = 0;
        if (dtRecords.Rows.Count > 0)
        {
            if (theColumn.ValidationOnWarning!="")
            {
               
                foreach (DataRow dr in dtRecords.Rows)
                {
                    if (dr[theColumn.SystemName].ToString() != "")
                    {
                       
                        strTempWarning = dr["WarningResults"].ToString();

                        strTempWarning = strTempWarning.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                        strTempWarning = strTempWarning.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range", "");
                        strTempWarning = strTempWarning.Replace("WARNING:" + theColumn.DisplayName + " – Value outside accepted range", "");
                        bool bHasExceedance = false;
                        if (strTempWarning.IndexOf("EXCEEDANCE: " + theColumn.DisplayName) > -1)
                            bHasExceedance = true;

                        if (bHasExceedance==false && !UploadManager.IsDataValid(dr[theColumn.SystemName].ToString(), theColumn.ValidationOnWarning, ref strTemp))
                        {
                            //Warning
                            strTempWarning = strTempWarning + " WARNING: " + theColumn.DisplayName + " – Value outside accepted range.";
                            dNoOfRecords = dNoOfRecords + 1;
                        }

                        if (strTempWarning.Trim().Length > 0)
                        {
                            Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                        }
                        else
                        {
                            Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                        }
                    }
                }
            }
            else
            {
                string strCheckColumnID = Common.GetValueFromSQL("SELECT TOP 1 CheckColumnID FROM [Condition] WHERE ConditionType='W' AND ColumnID=" + theColumn.ColumnID.ToString());

                if (strCheckColumnID != "")
                {
                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(strCheckColumnID));
                    if (theCheckColumn != null)
                    {
                        foreach (DataRow dr in dtRecords.Rows)
                        {
                            if (dr[theColumn.SystemName].ToString() != "")
                            {
                                string strEachFormula = "";

                                Record vRecord = RecordManager.ets_Record_Detail_Full(int.Parse(dr["RecordID"].ToString()));
                                string strCheckValue = RecordManager.GetRecordValue(ref vRecord, theCheckColumn.SystemName);
                                strEachFormula = UploadWorld.Condition_GetFormula((int)theColumn.ColumnID, theCheckColumn.ColumnID,
                                      "W", strCheckValue);

                                strTempWarning = dr["WarningResults"].ToString();

                                strTempWarning = strTempWarning.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                                strTempWarning = strTempWarning.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range", "");
                                strTempWarning = strTempWarning.Replace("WARNING:" + theColumn.DisplayName + " – Value outside accepted range", "");

                                bool bHasExceedance = false;
                                if (strTempWarning.IndexOf("EXCEEDANCE: " + theColumn.DisplayName) > -1)
                                    bHasExceedance = true;

                                if (bHasExceedance==false && !UploadManager.IsDataValid(dr[theColumn.SystemName].ToString(), strEachFormula, ref strTemp))
                                {
                                    //Warning
                                    strTempWarning = strTempWarning + " WARNING: " + theColumn.DisplayName + " – Value outside accepted range.";
                                    dNoOfRecords = dNoOfRecords + 1;
                                }

                                if (strTempWarning.Trim().Length > 0)
                                {
                                    Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                                }
                                else
                                {
                                    Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                                }

                            }
                        }

                    }
                }
                else
                {
                    foreach (DataRow dr in dtRecords.Rows)
                    {
                        if (dr[theColumn.SystemName].ToString() != "")
                        {
                            strTempWarning = dr["WarningResults"].ToString();

                            strTempWarning = strTempWarning.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                            strTempWarning = strTempWarning.Replace("WARNING: " + theColumn.DisplayName + " – Value outside accepted range", "");
                            strTempWarning = strTempWarning.Replace("WARNING:" + theColumn.DisplayName + " – Value outside accepted range", "");
                            if (strTempWarning.Trim().Length > 0)
                            {
                                Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                            }
                            else
                            {
                                Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                            }
                        }
                        

                    }
                    
                }
            }
            


            //lets send email
            if (IsCalculationChanged==false)
             SendAdjustWanrningSMSandEmail(theColumn, dNoOfRecords, ref strErrorMsg, IsCalculationChanged);

            //if (IsCalculationChanged == true)
                strErrorMsg = dNoOfRecords.ToString();

        }

    }


    public static void ets_AdjustExceedanceFormulaChanges(Column theColumn, ref string strErrorMsg, bool IsCalculationChanged)
    {
        string strTemp = "";
        Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);
        DataTable dtRecords = ets_GetListOfRecordsByOneSys(theColumn.SystemName, (int)theColumn.TableID);
        double dNoOfRecords = 0;


        bool bCheckIgnoreMidnight = false;
        string strIgnoreMidnight = SystemData.SystemOption_ValueByKey_Account("Time Calculation Ignore Midnight", (int)theTable.AccountID, theTable.TableID);

        if (strIgnoreMidnight != "" && strIgnoreMidnight.ToString().ToLower() == "yes")
        {
            bCheckIgnoreMidnight = true;
        }

        if (dtRecords.Rows.Count > 0)
        {
           
            string strTempWarning = "";
            if (theColumn.ValidationOnExceedance != "")
            {

                foreach (DataRow dr in dtRecords.Rows)
                {
                    if (dr[theColumn.SystemName].ToString() != "")
                    {

                        strTempWarning = dr["WarningResults"].ToString();

                        strTempWarning = strTempWarning.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                        strTempWarning = strTempWarning.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range", "");
                        strTempWarning = strTempWarning.Replace("EXCEEDANCE:" + theColumn.DisplayName + " – Value outside accepted range", "");

                        if (!UploadManager.IsDataValid(dr[theColumn.SystemName].ToString(), theColumn.ValidationOnExceedance, ref strTemp))
                        {
                            //Warning
                            strTempWarning = strTempWarning + " EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.";
                            dNoOfRecords = dNoOfRecords + 1;
                        }

                        if (strTempWarning.Trim().Length > 0)
                        {
                            Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                        }
                        else
                        {
                            Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                        }
                    }
                }
            }
            else
            {
                string strCheckColumnID = Common.GetValueFromSQL("SELECT TOP 1 CheckColumnID FROM [Condition] WHERE ConditionType='E' AND ColumnID=" + theColumn.ColumnID.ToString());

                if (strCheckColumnID != "")
                {
                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(strCheckColumnID));
                    if (theCheckColumn != null)
                    {
                        foreach (DataRow dr in dtRecords.Rows)
                        {
                            if (dr[theColumn.SystemName].ToString() != "")
                            {
                                string strEachFormula = "";

                                Record vRecord = RecordManager.ets_Record_Detail_Full(int.Parse(dr["RecordID"].ToString()));
                                string strCheckValue = RecordManager.GetRecordValue(ref vRecord, theCheckColumn.SystemName);
                                strEachFormula = UploadWorld.Condition_GetFormula((int)theColumn.ColumnID, theCheckColumn.ColumnID,
                                      "E", strCheckValue);

                                strTempWarning = dr["WarningResults"].ToString();

                                strTempWarning = strTempWarning.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                                strTempWarning = strTempWarning.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range", "");
                                strTempWarning = strTempWarning.Replace("EXCEEDANCE:" + theColumn.DisplayName + " – Value outside accepted range", "");

                                if (!UploadManager.IsDataValid(dr[theColumn.SystemName].ToString(), strEachFormula, ref strTemp))
                                {
                                    //Warning
                                    strTempWarning = strTempWarning + " EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.";
                                    dNoOfRecords = dNoOfRecords + 1;
                                }

                                if (strTempWarning.Trim().Length > 0)
                                {
                                    Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                                }
                                else
                                {
                                    Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                                }

                            }
                        }

                    }
                }
                else
                {
                    foreach (DataRow dr in dtRecords.Rows)
                    {
                        if (dr[theColumn.SystemName].ToString() != "")
                        {
                            strTempWarning = dr["WarningResults"].ToString();

                            strTempWarning = strTempWarning.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range.", "");
                            strTempWarning = strTempWarning.Replace("EXCEEDANCE: " + theColumn.DisplayName + " – Value outside accepted range", "");
                            strTempWarning = strTempWarning.Replace("EXCEEDANCE:" + theColumn.DisplayName + " – Value outside accepted range", "");
                            if (strTempWarning.Trim().Length > 0)
                            {
                                Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                            }
                            else
                            {
                                Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                            }
                        }
                       

                    }

                }
            }


            //lets send email
            if (IsCalculationChanged == false)
                SendAdjustExceedanceSMSandEmail(theColumn, dNoOfRecords, ref strErrorMsg, IsCalculationChanged);


            //if (strErrorMsg == true)
                strErrorMsg = dNoOfRecords.ToString();
        }

    }

    public static void ets_AdjustUnlikelyReadings(Column theColumn,  ref string strErrorMsg)
    {
        string strTemp = "";
        string strTempWarning = "";

        DataTable dtRecords = ets_GetListOfRecordsByOneSys(theColumn.SystemName, (int)theColumn.TableID);
        double dNoOfRecords = 0;
        if (dtRecords.Rows.Count > 0)
        {
            int? iCount = RecordManager.ets_Table_GetCount((int)theColumn.TableID, theColumn.SystemName, -1);
            if (iCount != null)
            {
                if (iCount >= Common.MinSTDEVRecords)
                {
                    double? dAVG = RecordManager.ets_Table_GetAVG((int)theColumn.TableID, theColumn.SystemName,  -1);

                    double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theColumn.TableID, theColumn.SystemName, -1);

                    if (dAVG != null && dSTDEV != null)
                    {
                        dSTDEV = dSTDEV * 3;
                    }

                    foreach (DataRow dr in dtRecords.Rows)
                    {
                        strTempWarning = dr["WarningResults"].ToString();

                        strTempWarning = strTempWarning.Replace("WARNING: " + theColumn.DisplayName + " – Unlikely data – outside 3 standard deviations.", "");

                        string strRecordedate;
                        strRecordedate = dr[theColumn.SystemName].ToString();

                        if (strRecordedate.Trim() != "")
                        {
                            double dRecordedate = double.Parse(strRecordedate);
                            if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
                            {
                                //Warning
                                strTempWarning = strTempWarning + " WARNING: " + theColumn.DisplayName + " – Unlikely data – outside 3 standard deviations.";
                                dNoOfRecords = dNoOfRecords + 1;
                            }
                            else
                            {
                                //
                            }
                        }



                        if (strTempWarning.Trim().Length > 0)
                        {
                            Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''") + "' WHERE RecordID=" + dr["RecordID"].ToString());
                        }
                        else
                        {
                            Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["RecordID"].ToString());
                        }


                    }
                }
            }


            ////lets send email

            //SendAdjustWanrningSMSandEmail(theColumn, dNoOfRecords, ref strErrorMsg, IsCalculationChanged);



        }

    }


    public static void SendAdjustValidationSMSandEmail(Column theColumn, double dNoOfRecords, ref string strErrorMsg, bool IsCalculationChanged)
    {

        Content theContent = SystemData.Content_Details_ByKey("adjustValidationEmail",null);
        if (theContent == null)
        {
            return;
        }

        Table theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);



        //string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
        //string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
        //string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
        //string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail",null,theColumn.TableID);




        string strBody = theContent.ContentP;


        strBody = strBody.Replace("[Table]", theTable.TableName);
        //strBody = strBody.Replace("[Record]", "?");
        strBody = strBody.Replace("[Field]", theColumn.DisplayName);
        strBody = strBody.Replace("[NoOfRecords]", dNoOfRecords.ToString());
        strBody = strBody.Replace("[NoOfRows]", dNoOfRecords.ToString());
        string strConV = "";
        if (theColumn.ValidationOnEntry != "")
        {
            //strBody = strBody.Replace("[ValidIf]", theColumn.ValidationOnEntry);
            strConV = Common.GetFromulaMsg("", theColumn.DisplayName, theColumn.ValidationOnEntry);
        }
        else
        {
            //it must be conditions
            strConV = UploadWorld.Condition_GetFormulaHTMLTable(theColumn, "V", "");

            

        }

        strBody = strBody.Replace("[ValidIf]", strConV);
        //MailMessage msg = new MailMessage();
        //msg.From = new MailAddress(strEmail);
        string strSubject = "";
        if (IsCalculationChanged)
        {
            strSubject = theTable.TableName + " -- " + "Calculation formula has been changed and new invalid Records has been created!";
        }
        else
        {
            strSubject = theTable.TableName + " -- " + theContent.Heading;
        }
        //msg.IsBodyHtml = true;

        //msg.Body = strBody;// Sb.ToString();
        
        //SmtpClient smtpClient = new SmtpClient(strEmailServer);
        //smtpClient.Timeout = 99999;
        //smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

        //smtpClient.Port = DBGurus.StringToInt(DBGurus.GetSystemOption("SmtpPort"));
        //smtpClient.EnableSsl = Convert.ToBoolean(DBGurus.GetSystemOption("EnableSSL"));

        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
         (int)theTable.TableID, null, null, null, null, null, true, null, null, null, null, null);

        foreach (DataRow dr in dtUsersEmail.Rows)
        {
            //msg.To.Clear();
            //msg.To.Add(dr["Email"].ToString());
            try
            {

               

#if (!DEBUG)
                //smtpClient.Send(msg);
#endif


                //if (msg.To.Count > 0)
                //{
                //    if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                //    {
                //Guid guidNew = Guid.NewGuid();
                //string strEmailUID = guidNew.ToString();

                //EmailLog theEmailLog = new EmailLog(null, int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()),
                //    strSubject,
                //  dr["Email"].ToString(), DateTime.Now, theColumn.TableID,
                //  theColumn.ColumnID,
                //  "Adjust Validation Email", strBody);
                //theEmailLog.EmailUID = strEmailUID;
               


                string sSendEmailError = "";


                Message theMessage = new Message(null, null, theColumn.TableID,(int)theTable.AccountID,
                  DateTime.Now, "W", "E",
                      null, dr["Email"].ToString(), strSubject, strBody, null, "");

                DBGurus.SendEmail("Adjust Validation Email", true, null, strSubject, strBody, "", dr["Email"].ToString(), "", "", null, theMessage, out sSendEmailError);


                //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                //{

                //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
                //}


            }
            catch (Exception)
            {

                strErrorMsg = "Server could not send validation Email & SMS";
            }


        }


        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
     (int)theTable.TableID, null, null, null, null, null, null, true, null, null, null, null);

        foreach (DataRow dr in dtUsersSMS.Rows)
        {
            //msg.To.Clear();
            if (dr["PhoneNumber"] != DBNull.Value)
            {
                if (dr["PhoneNumber"].ToString() != "")
                {
                    //msg.To.Add(dr["PhoneNumber"].ToString() + strWarningSMSEMail);
                    try
                    {


#if (!DEBUG)
                        //smtpClient.Send(msg);
#endif

                        


                        //if (msg.To.Count > 0)
                        //{
                        //    if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                        //    {
                        //Guid guidNew = Guid.NewGuid();
                        //string strEmailUID = guidNew.ToString();

                        //EmailLog theEmailLog = new EmailLog(null, int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()),
                        //   strSubject,
                        //  dr["PhoneNumber"].ToString() + strWarningSMSEMail, DateTime.Now, theColumn.TableID,
                        //  theColumn.ColumnID,
                        //  "Adjust Validation SMS",strBody);
                        //theEmailLog.EmailUID = strEmailUID;
                        //        EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);

                        //    }
                        //}


                        string sSendEmailError = "";

                        Message theMessage = new Message(null, null, theColumn.TableID, (int)theTable.AccountID,
                  DateTime.Now, "W", "S",
                      null, dr["PhoneNumber"].ToString() + strWarningSMSEMail, strSubject, strBody, null, ""); 


                        DBGurus.SendEmail("Adjust Validation SMS", null, true, strSubject, strBody, "",
                            dr["PhoneNumber"].ToString() + strWarningSMSEMail, "", "", null, theMessage, out sSendEmailError);


                        //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                        //{

                        //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), null, true, null, null);
                        //}


                    }
                    catch (Exception)
                    {

                        strErrorMsg = "Server could not send validation Email & SMS";
                    }
                }
            }
        }

    }



    public static DataTable ets_Records_By_TableID(int nTableID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("ets_Records_By_TableID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));


                
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

                if (ds != null &&  ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }
            }


        }


    }



    public static DataTable Record_Audit_Detail(int RecordID, DateTime UpdateDate)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Record_Audit_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@RecordID", RecordID));
                command.Parameters.Add(new SqlParameter("@UpdateDate", UpdateDate));

               
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




    


    public static DataTable Record_Audit_Summary(int nRecordID, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Record_Audit_Summary", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


                command.Parameters.Add(new SqlParameter("@RecordID", nRecordID));

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
                    if (ds.Tables[1].Rows[0][0] != DBNull.Value)
                    {
                        iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                    }
                    else
                    {
                        iTotalRowsNum = 0;
                    }
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



    public static DataTable Column_Audit_Summary(int nColumnID, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Column_Audit_Summary", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


                command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

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
                    if (ds.Tables[1].Rows[0][0] != DBNull.Value)
                    {
                        iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                    }
                    else
                    {
                        iTotalRowsNum = 0;
                    }
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



    //public static DataTable Payment_Audit_Summary(int nPaymentID, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("Payment_Audit_Summary", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


    //            command.Parameters.Add(new SqlParameter("@nPaymentID", nPaymentID));

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
    //                if (ds.Tables[1].Rows[0][0] != DBNull.Value)
    //                {
    //                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
    //                }
    //                else
    //                {
    //                    iTotalRowsNum = 0;
    //                }
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


    public static DataTable Column_Audit_Detail(int nColumnID, DateTime UpdateDate)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Column_Audit_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));
                command.Parameters.Add(new SqlParameter("@UpdateDate", UpdateDate));

                
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


    public static DataTable Payment_Audit_Detail(int nPaymentID, DateTime UpdateDate)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Payment_Audit_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nPaymentID", nPaymentID));
                command.Parameters.Add(new SqlParameter("@UpdateDate", UpdateDate));

             
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


    public static DataTable Table_Audit_Summary(int nTableID, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Table_Audit_Summary", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

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
                    if (ds.Tables[1].Rows[0][0] != DBNull.Value)
                    {
                        iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                    }
                    else
                    {
                        iTotalRowsNum = 0;
                    }
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



    public static DataTable Table_Audit_Detail(int nTableID, DateTime UpdateDate)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Table_Audit_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                command.Parameters.Add(new SqlParameter("@UpdateDate", UpdateDate));


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




    public static string AdjusTMaxTimeBetweenRecords(int iTableID, ref int iTotalNewRecordsEffected)
    {

        //get all site all data for this type
        DataTable dtDatas = ets_Records_By_TableID(iTableID);
        string strTempWarning;
        iTotalNewRecordsEffected = 0;
        Table theTable = ets_Table_Details(iTableID);

        if (dtDatas.Rows.Count > 0)
        {

            foreach (DataRow dr in dtDatas.Rows)
            {
                strTempWarning = dr["WarningResults"].ToString();
                if (strTempWarning.Trim().Length > 0)
                {
                    strTempWarning = strTempWarning.Replace("WARNING: " + WarningMsg.MaxtimebetweenRecords + "!", "");
                }

                //if (dr["LocationID"].ToString() != "")
                //{
                    if (!RecordManager.IsTimeBetweenRecordOK(iTableID, theTable.MaxTimeBetweenRecords, theTable.MaxTimeBetweenRecordsUnit, int.Parse(dr["Record ID"].ToString()), DateTime.Parse(dr["DateTimeRecorded"].ToString())))
                    {
                        strTempWarning = strTempWarning + " WARNING: " + WarningMsg.MaxtimebetweenRecords + "!";
                        iTotalNewRecordsEffected = iTotalNewRecordsEffected + 1;
                    }
                //}

                if (strTempWarning.Trim().Length > 0)
                {
                    Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''") + "' WHERE RecordID=" + dr["Record ID"].ToString());
                }
                else
                {
                    Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["Record ID"].ToString());
                }
            }

        }




        return "ok";
    }



    //public static DataTable ets_Table_Changes_Select(int nTableID, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, ref int iCLColumnCount)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Table_Changes_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


    //            command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
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
    //                iCLColumnCount = ds.Tables[0].Columns.Count;
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

    public static double? ets_Table_GetAVG(int iTableID, string strSystemName, int iRecordID)
    {
        DataTable dt = Common.DataTableFromText("SELECT AVG(CONVERT(decimal(20,10),dbo.RemoveSpecialChars(" + strSystemName + "))) FROM Record WHERE TableID=" + iTableID.ToString() + " AND IsActive=1 AND RecordID<>" + iRecordID.ToString());

        if (dt.Rows[0][0] != DBNull.Value)
        {
            return double.Parse(dt.Rows[0][0].ToString());
        }

        return null;
    }

    public static int? ets_Table_GetCount(int iTableID, string strSystemName, int iRecordID)
    {
        DataTable dt = Common.DataTableFromText("SELECT COUNT(*) FROM Record WHERE TableID=" + iTableID.ToString() + " AND IsActive=1 AND " + strSystemName + " is not null  AND RecordID<>" + iRecordID.ToString());

        if (dt!=null && dt.Rows[0][0] != DBNull.Value)
        {
            return int.Parse(dt.Rows[0][0].ToString());
        }

        return null;
    }


    public static double? ets_Table_GetSTDEV(int iTableID, string strSystemName, int iRecordID)
    {
        DataTable dt = Common.DataTableFromText("SELECT STDEV(CONVERT(decimal(20,10),dbo.RemoveSpecialChars(" + strSystemName + "))) FROM Record WHERE TableID=" + iTableID.ToString() + " AND IsActive=1  AND RecordID<>" + iRecordID.ToString());

        if (dt!=null && dt.Rows[0][0] != DBNull.Value)
        {
            return double.Parse(dt.Rows[0][0].ToString());
        }

        return null;
    }




    //public static double? ets_Table_GetAVG(int iTableID, string strSystemName, ref SqlConnection connection, ref SqlTransaction tn, int iRecordID)
    //{
    //    DataTable dt = Common.DataTableFromText("SELECT AVG(CONVERT(decimal(20,10),dbo.RemoveSpecialChars(" + strSystemName + "))) FROM Record WHERE TableID=" + iTableID.ToString() + " AND IsActive=1  AND RecordID<>" + iRecordID.ToString(), ref connection, ref tn);

    //    if (dt.Rows[0][0] != DBNull.Value)
    //    {
    //        return double.Parse(dt.Rows[0][0].ToString());
    //    }

    //    return null;
    //}

    //public static int? ets_Table_GetCount(int iTableID, string strSystemName, ref SqlConnection connection, ref SqlTransaction tn, int iRecordID)
    //{
    //    DataTable dt = Common.DataTableFromText("SELECT COUNT(*) FROM Record WHERE TableID=" + iTableID.ToString() + " AND IsActive=1 AND " + strSystemName + " is not null   AND RecordID<>" + iRecordID.ToString(), ref connection, ref tn);

    //    if (dt.Rows[0][0] != DBNull.Value)
    //    {
    //        return int.Parse(dt.Rows[0][0].ToString());
    //    }

    //    return null;
    //}


    //public static double? ets_Table_GetSTDEV(int iTableID, string strSystemName, ref SqlConnection connection, ref SqlTransaction tn, int iRecordID)
    //{
    //    DataTable dt = Common.DataTableFromText("SELECT STDEV(CONVERT(decimal(20,10),dbo.RemoveSpecialChars(" + strSystemName + "))) FROM Record WHERE TableID=" + iTableID.ToString() + " AND IsActive=1  AND RecordID<>" + iRecordID.ToString(), ref connection, ref tn);

    //    if (dt.Rows[0][0] != DBNull.Value)
    //    {
    //        return double.Parse(dt.Rows[0][0].ToString());
    //    }

    //    return null;
    //}



    //public static DataTable ets_Column_Changes_Select(int nColumnID, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, ref int iCLColumnCount)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Column_Changes_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


    //            command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));
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
    //                iCLColumnCount = ds.Tables[0].Columns.Count;
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


    public static DataTable Audit_Summary(int nAccountID, string sTableName, DateTime? dDateFrom, DateTime? dDateTo,
        int? nTableID, string sTextSearch, int? nStartRow, int? nMaxRows, int? nUserID, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Audit_Summary", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));


                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sTableName != "")
                    command.Parameters.Add(new SqlParameter("@sTableName", sTableName));

                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));
                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (sTextSearch != "")
                    command.Parameters.Add(new SqlParameter("@sTextSearch", sTextSearch));

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));


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
                    if (ds.Tables[1].Rows[0][0] != DBNull.Value)
                    {
                        iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                    }
                    else
                    {
                        iTotalRowsNum = 0;
                    }
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

    public static DataTable Audit_Detail(string sTableName, int nPKID, DateTime UpdateDate)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Audit_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@sTableName", sTableName));
                command.Parameters.Add(new SqlParameter("@nPKID", nPKID));
                command.Parameters.Add(new SqlParameter("@UpdateDate", UpdateDate));

                
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

    //public static double ets_Record_Avg(int nTableID, int nColumnID,int nAvgNumberOfRecords,
    //    DateTime dDateTimeRecorded, SqlTransaction tn, SqlConnection cn)
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

    //    using (SqlCommand command = new SqlCommand("ets_Record_Avg", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }


    //        command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
    //        command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));
    //        command.Parameters.Add(new SqlParameter("@nAvgNumberOfRecords", nAvgNumberOfRecords));
    //        command.Parameters.Add(new SqlParameter("@dDateTimeRecorded", dDateTimeRecorded));

    //        //connection.Open();
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = command;
    //        DataTable dt = new DataTable();
    //        System.Data.DataSet ds = new System.Data.DataSet();
    //        da.Fill(ds);

    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        if (ds.Tables.Count > 0)
    //        {
    //            if (ds.Tables[0].Rows[0][0] == DBNull.Value)
    //            {
    //                return 0;
    //            }
    //            else
    //            {
    //                return double.Parse(ds.Tables[0].Rows[0][0].ToString());
    //            }

    //        }
    //        {
    //            return 0;
    //        }
    //    }


    //}


    //public static double ets_Record_Avg(int nTableID, int nColumnID, DateTime dAvgDateFrom, DateTime dAvgDateTo
    //   , SqlTransaction tn, SqlConnection cn)
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

    //    using (SqlCommand command = new SqlCommand("ets_Record_Avg", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }


    //        command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
    //        command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));
    //        command.Parameters.Add(new SqlParameter("@dAvgDateFrom", dAvgDateFrom));
    //        command.Parameters.Add(new SqlParameter("@dAvgDateTo", dAvgDateTo));

    //        //connection.Open();
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        da.SelectCommand = command;
    //        DataTable dt = new DataTable();
    //        System.Data.DataSet ds = new System.Data.DataSet();
    //        da.Fill(ds);

    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        if (ds.Tables.Count > 0)
    //        {
    //            if (ds.Tables[0].Rows[0][0] == DBNull.Value)
    //            {
    //                return 0;
    //            }
    //            else
    //            {
    //                return double.Parse(ds.Tables[0].Rows[0][0].ToString());
    //            }

    //        }
    //        {
    //            return 0;
    //        }
    //    }


    //}

    //public static DataTable ets_Chart_GetData(int? nTableID, string sSeries,
    //  string sValueFields, DateTime? dStartDate, DateTime? dEndDate)
    //{

    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_Chart_GetData", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

    //            if (nTableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

    //            if (sSeries != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sSeries", sSeries));

    //            if (sValueFields != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sValueFields", sValueFields));



    //            if (dStartDate != null)
    //                command.Parameters.Add(new SqlParameter("@dStartDate", dStartDate));

    //            if (dEndDate != null)
    //                command.Parameters.Add(new SqlParameter("@dEndDate", dEndDate));




    //            connection.Open();

    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            System.Data.DataSet ds = new System.Data.DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();


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



    public static DataTable ets_Chart_GetData_New(int? nTableID,
        string sGraphXAxisColumnID, string sValueFields, DateTime? dStartDate, DateTime? dEndDate,
        string sGraphSeriesColumnID, string sGraphSeriesID,
        string sPeriod, bool bPercent, int? nMaxRows)
    {
        return ets_Chart_GetData_New("ets_Chart_GetData_New",
            nTableID,
            sGraphXAxisColumnID, sValueFields, dStartDate, dEndDate,
            sGraphSeriesColumnID, sGraphSeriesID, false,
            null, null, null, null,
            sPeriod, bPercent, nMaxRows);
    }

    public static DataTable ets_Chart_GetData_New(string sSPName,
        int? nTableID,
        string sGraphXAxisColumnID, string sValueFields, DateTime? dStartDate, DateTime? dEndDate,
        string sGraphSeriesColumnID, string sGraphSeriesID, bool bForceAverage,
        int? nDataColumn1ID, int? nDataColumn2ID, int? nDataColumn3ID, int? nDataColumn4ID,
        string sPeriod, bool bPercent, int? nMaxRows)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand(sSPName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                //if (sSeries != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sSeries", sSeries));

                if (sGraphXAxisColumnID != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sGraphXAxisColumnID", sGraphXAxisColumnID));

                if (sValueFields != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValueFields", sValueFields));

                if (dStartDate != null)
                    command.Parameters.Add(new SqlParameter("@dStartDate", dStartDate));

                if (dEndDate != null)
                    command.Parameters.Add(new SqlParameter("@dEndDate", dEndDate));

                command.Parameters.Add(new SqlParameter("@GraphSeriesColumnID", sGraphSeriesColumnID));
                command.Parameters.Add(new SqlParameter("@GraphSeriesID", sGraphSeriesID));

                command.Parameters.Add(new SqlParameter("@ForceAverage", bForceAverage));

                if (nDataColumn1ID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nDataColumn1ID", nDataColumn1ID.Value));
                if (nDataColumn2ID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nDataColumn2ID", nDataColumn2ID.Value));
                if (nDataColumn3ID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nDataColumn3ID", nDataColumn3ID.Value));
                if (nDataColumn4ID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nDataColumn4ID", nDataColumn4ID.Value));

                if (sPeriod != "")
                    command.Parameters.Add(new SqlParameter("@sPeriod", sPeriod));

                if (bPercent != null)
                    command.Parameters.Add(new SqlParameter("@bPercent", bPercent));

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





    public static DataTable ets_Chart_GetData_MeanMaxMin(int? nTableID,
        string sGraphXAxisColumnID, string sValueFields, DateTime? dStartDate, DateTime? dEndDate,
        string sPeriod, int? nMaxRows)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Chart_GetData_MeanMaxMin", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                //if (sSeries != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sSeries", sSeries));

                if (sGraphXAxisColumnID != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sGraphXAxisColumnID", sGraphXAxisColumnID));

                if (sValueFields != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValueFields", sValueFields));



                if (dStartDate != null)
                    command.Parameters.Add(new SqlParameter("@dStartDate", dStartDate));

                if (dEndDate != null)
                    command.Parameters.Add(new SqlParameter("@dEndDate", dEndDate));

                if (sPeriod != "")
                    command.Parameters.Add(new SqlParameter("@sPeriod", sPeriod));

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



    public static DataTable ets_Chart_GetData_Reduction(int? nTableID, string sSeries,
     string sValueFields, DateTime? dStartDate, DateTime? dEndDate, string sReducedData)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Chart_GetData_Reduction", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (sSeries != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sSeries", sSeries));

                if (sValueFields != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValueFields", sValueFields));



                if (dStartDate != null)
                    command.Parameters.Add(new SqlParameter("@dStartDate", dStartDate));

                if (dEndDate != null)
                    command.Parameters.Add(new SqlParameter("@dEndDate", dEndDate));

                if (sReducedData != "")
                    command.Parameters.Add(new SqlParameter("@sReducedData", sReducedData));




                

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

    

    public static DataTable ets_DashBorad_Alert(int nAccountID,DateTime dLastAlertDate)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DashBorad_Alert", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@dLastAlertDate", dLastAlertDate));

                //connection.Open();
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



    public static void ets_Batch_Duplicate(int nBatchID,string strUniqueColumnIDSys,string strUniqueColumnID2Sys)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Batch_Duplicate", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 900;

                
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));

                if (strUniqueColumnIDSys != "")
                    command.Parameters.Add(new SqlParameter("@sUniqueColumnIDSys", strUniqueColumnIDSys));


                if (strUniqueColumnID2Sys != "")
                    command.Parameters.Add(new SqlParameter("@sUniqueColumnID2Sys", strUniqueColumnID2Sys));

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


    public static int ets_TableChild_Insert(TableChild p_TableChild)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_TableChild_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nParentTableID", p_TableChild.ParentTableID));
                command.Parameters.Add(new SqlParameter("@nChildTableID", p_TableChild.ChildTableID));
                command.Parameters.Add(new SqlParameter("@sDescription", p_TableChild.Description));
                command.Parameters.Add(new SqlParameter("@sDetailPageType", p_TableChild.DetailPageType));

                if (p_TableChild.ShowAddButton != null)
                    command.Parameters.Add(new SqlParameter("@bShowAddButton", p_TableChild.ShowAddButton));

                if (p_TableChild.ShowEditButton != null)
                    command.Parameters.Add(new SqlParameter("@bShowEditButton", p_TableChild.ShowEditButton));

                command.Parameters.Add(new SqlParameter("@nHideColumnID", p_TableChild.HideColumnID));
                command.Parameters.Add(new SqlParameter("@sHideColumnValue", p_TableChild.HideColumnValue));
                command.Parameters.Add(new SqlParameter("@sHideOperator", p_TableChild.HideOperator));

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


    public static int ets_TableChild_Update(TableChild p_TableChild)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_TableChild_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               
                command.Parameters.Add(new SqlParameter("@nTableChildID", p_TableChild.TableChildID));
                command.Parameters.Add(new SqlParameter("@nParentTableID", p_TableChild.ParentTableID));
                command.Parameters.Add(new SqlParameter("@nChildTableID", p_TableChild.ChildTableID));
                command.Parameters.Add(new SqlParameter("@sDescription", p_TableChild.Description));
                command.Parameters.Add(new SqlParameter("@sDetailPageType", p_TableChild.DetailPageType));

                if (p_TableChild.ShowAddButton != null)
                    command.Parameters.Add(new SqlParameter("@bShowAddButton", p_TableChild.ShowAddButton));

                if (p_TableChild.ShowEditButton != null)
                    command.Parameters.Add(new SqlParameter("@bShowEditButton", p_TableChild.ShowEditButton));

                command.Parameters.Add(new SqlParameter("@nHideColumnID", p_TableChild.HideColumnID));
                command.Parameters.Add(new SqlParameter("@sHideColumnValue", p_TableChild.HideColumnValue));
                command.Parameters.Add(new SqlParameter("@sHideOperator", p_TableChild.HideOperator));


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





    public static DataTable ets_TableChild_Select(int? nParentTableID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TableChild_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nParentTableID", nParentTableID));
                         
                

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


    public static int ets_TableChild_Delete(int nTableChildID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TableChild_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableChildID ", nTableChildID));

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




    public static TableChild ets_TableChild_Detail(int nTableChildID)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_TableChild_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nTableChildID", nTableChildID));

                connection.Open();


                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TableChild temp = new TableChild(
                                (int)reader["TableChildID"], (int)reader["ParentTableID"], (int)reader["ChildTableID"],
                               reader["Description"] == DBNull.Value ? "" : (string)reader["Description"],
                              (string)reader["DetailPageType"]
                                );

                            temp.ShowAddButton = (bool)reader["ShowAddButton"];
                            temp.ShowEditButton = (bool)reader["ShowEditButton"];

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



    //public static int dbg_Record_One_Update(string sSystemName,string sValue,string sRecordID, SqlTransaction tn)
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

    //    using (SqlCommand command = new SqlCommand("dbg_Record_One_Update", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        command.Parameters.Add(new SqlParameter("@sSystemName", sSystemName));
    //        command.Parameters.Add(new SqlParameter("@sValue", sValue));
    //        command.Parameters.Add(new SqlParameter("@sRecordID", sRecordID));
            

    //        command.ExecuteNonQuery();

    //        if (tn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        return 1;
    //    }

    //}












    public static int dbg_ShowWhen_Insert(ShowWhen p_ShowWhen)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_ShowWhen_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

               
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                if (p_ShowWhen.TableTabID != null)
                    command.Parameters.Add(new SqlParameter("@TableTabID", p_ShowWhen.TableTabID));

                if (p_ShowWhen.ColumnID!=null)
                command.Parameters.Add(new SqlParameter("@nColumnID", p_ShowWhen.ColumnID));
                if (p_ShowWhen.Context != "")
                    command.Parameters.Add(new SqlParameter("@Context", p_ShowWhen.Context));
                if (p_ShowWhen.DocumentSectionID != null)
                    command.Parameters.Add(new SqlParameter("@DocumentSectionID", p_ShowWhen.DocumentSectionID));

                if (p_ShowWhen.HideColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nHideColumnID", p_ShowWhen.HideColumnID));

                command.Parameters.Add(new SqlParameter("@sHideColumnValue", p_ShowWhen.HideColumnValue));
                command.Parameters.Add(new SqlParameter("@sHideOperator", p_ShowWhen.HideOperator));
                command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_ShowWhen.DisplayOrder));

                if (p_ShowWhen.JoinOperator != "")
                    command.Parameters.Add(new SqlParameter("@sJoinOperator", p_ShowWhen.JoinOperator));

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


    public static int dbg_ShowWhen_Update(ShowWhen p_ShowWhen)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ShowWhen_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                if (p_ShowWhen.TableTabID != null)
                    command.Parameters.Add(new SqlParameter("@TableTabID", p_ShowWhen.TableTabID));

                command.Parameters.Add(new SqlParameter("@nShowWhenID", p_ShowWhen.ShowWhenID));
                if (p_ShowWhen.ColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nColumnID", p_ShowWhen.ColumnID));

                if (p_ShowWhen.Context != "")
                    command.Parameters.Add(new SqlParameter("@Context", p_ShowWhen.Context));

                if (p_ShowWhen.DocumentSectionID != null)
                    command.Parameters.Add(new SqlParameter("@DocumentSectionID", p_ShowWhen.DocumentSectionID));

                if (p_ShowWhen.HideColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nHideColumnID", p_ShowWhen.HideColumnID));


                command.Parameters.Add(new SqlParameter("@sHideColumnValue", p_ShowWhen.HideColumnValue));
                command.Parameters.Add(new SqlParameter("@sHideOperator", p_ShowWhen.HideOperator));
                command.Parameters.Add(new SqlParameter("@nDisplayOrder", p_ShowWhen.DisplayOrder));

                if (p_ShowWhen.JoinOperator != null)
                    command.Parameters.Add(new SqlParameter("@sJoinOperator", p_ShowWhen.JoinOperator));


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



    public static DataTable dbg_ShowWhen_Select(int? nColumnID, int? DocumentSectionID, 
        int? TableTabID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ShowWhen_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (TableTabID != null)
                    command.Parameters.Add(new SqlParameter("@TableTabID", TableTabID));

                if (nColumnID!=null)
                    command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

                if (DocumentSectionID != null)
                    command.Parameters.Add(new SqlParameter("@DocumentSectionID", DocumentSectionID));
                               
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



    public static DataTable dbg_ShowWhen_ForGrid(int? nColumnID, int? DocumentSectionID, int? TableTabID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ShowWhen_ForGrid", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                if (nColumnID!=null)
                    command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));

                if (DocumentSectionID != null)
                    command.Parameters.Add(new SqlParameter("@DocumentSectionID", DocumentSectionID));

                if (TableTabID != null)
                    command.Parameters.Add(new SqlParameter("@TableTabID", TableTabID));

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


    public static int dbg_ShowWhen_Delete(int nShowWhenID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ShowWhen_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nShowWhenID ", nShowWhenID));

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




    public static ShowWhen dbg_ShowWhen_Detail(int nShowWhenID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ShowWhen_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nShowWhenID", nShowWhenID));

                connection.Open();


                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ShowWhen temp = new ShowWhen(
                                (int)reader["ShowWhenID"], 
                                reader["ColumnID"] == DBNull.Value ? null : (int?)reader["ColumnID"],
                                reader["HideColumnID"] == DBNull.Value ? null : (int?)reader["HideColumnID"],
                               reader["HideColumnValue"] == DBNull.Value ? "" : (string)reader["HideColumnValue"],
                               reader["HideOperator"] == DBNull.Value ? "" : (string)reader["HideOperator"],
                               reader["DisplayOrder"] == DBNull.Value ? null : (int?)reader["DisplayOrder"],
                               reader["JoinOperator"] == DBNull.Value ? "" : (string)reader["JoinOperator"]
                                );
                            temp.Context = reader["Context"] == DBNull.Value ? "" : (string)reader["Context"];
                            temp.DocumentSectionID = reader["DocumentSectionID"] == DBNull.Value ? null : (int?)reader["DocumentSectionID"];
                            temp.TableTabID = reader["TableTabID"] == DBNull.Value ? null : (int?)reader["TableTabID"];

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

    public static void dbg_Copy_Account_SignUp(int nTemplateAccountID, int nTargetAccountID, int nSignUpUserID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Copy_Account_SignUp", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                //command.CommandTimeout = 0;

                
                command.Parameters.Add(new SqlParameter("@nTemplateAccountID", nTemplateAccountID));
                command.Parameters.Add(new SqlParameter("@nTargetAccountID", nTargetAccountID));
                command.Parameters.Add(new SqlParameter("@nSignUpUserID", nSignUpUserID));



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

    public static int AddRecordSP(string spName,int? RecordID,int?FormSetID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand(spName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

               
                command.Parameters.Add(new SqlParameter("@RecordID", RecordID));
                if (FormSetID != null)
                    command.Parameters.Add(new SqlParameter("@FormSetID", FormSetID));

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



   

    public static string Column_SPDefaultValue(string spName, int? RecordID, int? UserID,int? ColumnID)
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
                if (RecordID != null)
                    command.Parameters.Add(new SqlParameter("@RecordID", RecordID));

                if (ColumnID != null)
                    command.Parameters.Add(new SqlParameter("@ColumnID", ColumnID));

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
                    //
                }

                connection.Close();
                connection.Dispose();
               
                 return strValue;
            }


        }


    }



    public static string Table_SPSaveRecord(string spName, int? RecordID, int? UserID)
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
                if (RecordID != null)
                    command.Parameters.Add(new SqlParameter("@RecordID", RecordID));



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


    public static string RunSPForRecord(string spName, int? RecordID, int? UserID)
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
                if (RecordID != null)
                    command.Parameters.Add(new SqlParameter("@RecordID", RecordID));



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
    public static string Table_SPUpdateConfirm(string spName, int? RecordID, int? UserID, string UpdateValues)
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
                if (RecordID != null)
                    command.Parameters.Add(new SqlParameter("@RecordID", RecordID));
                if (UpdateValues != "")
                    command.Parameters.Add(new SqlParameter("@UpdateValues", UpdateValues));


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

    public static string Table_SPSendEmail(string spName, int? RecordID, int? UserID)
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
                if (RecordID != null)
                    command.Parameters.Add(new SqlParameter("@RecordID", RecordID));



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
                    //
                }
                connection.Close();
                connection.Dispose();
                

                return strValue;
            }
        }



        

    }
}

