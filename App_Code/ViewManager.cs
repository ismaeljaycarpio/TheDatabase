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
/// Summary description for ViewManager
/// </summary>
public class ViewManager
{
	public ViewManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}




    #region View

    public static string strViewFilterNone = "  AND ((( 1=1   AND   1=1 )  AND   1=1 )  AND   1=1 )";
    

    //public static int CreateDashView(int iTableID)
    //{
    //    User objUser = (User)System.Web.HttpContext.Current.Session["User"];
    //    Table theTable = RecordManager.ets_Table_Details(iTableID);
    //    View newView = new View(null, iTableID, theTable.TableName, objUser.UserID, 10, "", "",
    //               null, null, null, null, null, null);

    //    newView.ViewPageType = "dash";

    //    int iNewViewID = ViewManager.dbg_View_Insert(newView);
    //    ViewManager.dbg_CreateDefaultViewItem(iNewViewID);

    //    return iNewViewID;
    //}
    

    public static int dbg_View_Insert(View p_View)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_View_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

               

                if (p_View.ShowFixedHeader != null)
                    command.Parameters.Add(new SqlParameter("@bShowFixedHeader", p_View.ShowFixedHeader));

                if (p_View.ViewPageType != "")
                    command.Parameters.Add(new SqlParameter("@sViewPageType", p_View.ViewPageType));
                
                if (p_View.FilterControlsInfo != "")
                    command.Parameters.Add(new SqlParameter("@sFilterControlsInfo", p_View.FilterControlsInfo));

                command.Parameters.Add(new SqlParameter("@nTableID", p_View.TableID));
                command.Parameters.Add(new SqlParameter("@sViewName", p_View.ViewName));

                command.Parameters.Add(new SqlParameter("@nUserID", p_View.UserID));


                if (p_View.RowsPerPage != null)
                    command.Parameters.Add(new SqlParameter("@nRowsPerPage", p_View.RowsPerPage));

                if (p_View.SortOrder != "")
                    command.Parameters.Add(new SqlParameter("@sSortOrder", p_View.SortOrder));

                if (p_View.Filter != "")
                    command.Parameters.Add(new SqlParameter("@sFilter", p_View.Filter));


                command.Parameters.Add(new SqlParameter("@bShowSearchFields", p_View.ShowSearchFields));


                command.Parameters.Add(new SqlParameter("@bShowAddIcon", p_View.ShowAddIcon));
                command.Parameters.Add(new SqlParameter("@bShowEditIcon", p_View.ShowEditIcon));
                command.Parameters.Add(new SqlParameter("@bShowDeleteIcon", p_View.ShowDeleteIcon));
                command.Parameters.Add(new SqlParameter("@bShowViewIcon", p_View.ShowViewIcon));
                command.Parameters.Add(new SqlParameter("@bShowBulkUpdateIcon", p_View.ShowBulkUpdateIcon));

                if (p_View.ViewPageType.ToLower()!= "child")
                {
                    p_View.ParentTableID = null;
                }
                if (p_View.ParentTableID != null)
                    command.Parameters.Add(new SqlParameter("@ParentTableID", p_View.ParentTableID));


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


    public static int dbg_View_Update(View p_View)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_View_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nViewID", p_View.ViewID));

                if (p_View.ShowFixedHeader != null)
                    command.Parameters.Add(new SqlParameter("@bShowFixedHeader", p_View.ShowFixedHeader));

                if (p_View.ViewPageType != "")
                    command.Parameters.Add(new SqlParameter("@sViewPageType", p_View.ViewPageType));

              

                command.Parameters.Add(new SqlParameter("@nTableID", p_View.TableID));
                command.Parameters.Add(new SqlParameter("@sViewName", p_View.ViewName));

                command.Parameters.Add(new SqlParameter("@nUserID", p_View.UserID));


                if (p_View.RowsPerPage != null)
                    command.Parameters.Add(new SqlParameter("@nRowsPerPage", p_View.RowsPerPage));

                if (p_View.SortOrder != "")
                    command.Parameters.Add(new SqlParameter("@sSortOrder", p_View.SortOrder));

                if (p_View.Filter != "")
                {
                    if(p_View.Filter.Trim().ToLower()==" AND ((( 1=1   AND   1=1 )  AND   1=1 )  AND   1=1 )".Trim().ToLower())
                    {
                        p_View.Filter = "";                        
                    }

                    if (p_View.Filter==""   && p_View.SortOrder == "")
                    {
                       
                        p_View.FilterControlsInfo = "";
                    }


                    if (p_View.Filter != "")
                        command.Parameters.Add(new SqlParameter("@sFilter", p_View.Filter));
                }

                if (p_View.FilterControlsInfo != "")
                    command.Parameters.Add(new SqlParameter("@sFilterControlsInfo", p_View.FilterControlsInfo));

                command.Parameters.Add(new SqlParameter("@bShowSearchFields", p_View.ShowSearchFields));


                command.Parameters.Add(new SqlParameter("@bShowAddIcon", p_View.ShowAddIcon));
                command.Parameters.Add(new SqlParameter("@bShowEditIcon", p_View.ShowEditIcon));
                command.Parameters.Add(new SqlParameter("@bShowDeleteIcon", p_View.ShowDeleteIcon));
                command.Parameters.Add(new SqlParameter("@bShowViewIcon", p_View.ShowViewIcon));
                command.Parameters.Add(new SqlParameter("@bShowBulkUpdateIcon", p_View.ShowBulkUpdateIcon));

                if (p_View.ViewPageType.ToLower() != "child")
                {
                    p_View.ParentTableID = null;
                }
                if (p_View.ParentTableID != null)
                    command.Parameters.Add(new SqlParameter("@ParentTableID", p_View.ParentTableID));


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
    

    public static DataTable dbg_View_Select(int? nTableID, string sViewName,
       int? nRowsPerPage,int? nUserID, string sSortOrder, string sFilter, bool? bShowSearchFields, 
        bool? bShowAddIcon, bool? bShowEditIcon, bool? bShowDeleteIcon, bool? bShowViewIcon, bool? bShowBulkUpdateIcon,
         string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));



                if (sViewName != "")
                    command.Parameters.Add(new SqlParameter("@sViewName", sViewName));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "ViewID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                if (nRowsPerPage != null)
                    command.Parameters.Add(new SqlParameter("@nRowsPerPage", nRowsPerPage));
                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));
                if (sSortOrder != "")
                    command.Parameters.Add(new SqlParameter("@sSortOrder", sSortOrder));
                if (sFilter != "")
                    command.Parameters.Add(new SqlParameter("@sFilter", sFilter));
                if (bShowSearchFields != null)
                    command.Parameters.Add(new SqlParameter("@bShowSearchFields", bShowSearchFields));
                if (bShowAddIcon != null)
                    command.Parameters.Add(new SqlParameter("@bShowAddIcon", bShowAddIcon));
                if (bShowEditIcon != null)
                    command.Parameters.Add(new SqlParameter("@bShowEditIcon", bShowEditIcon));
                if (bShowDeleteIcon != null)
                    command.Parameters.Add(new SqlParameter("@bShowDeleteIcon", bShowDeleteIcon));
                if (bShowViewIcon != null)
                    command.Parameters.Add(new SqlParameter("@bShowViewIcon", bShowViewIcon));
                if (bShowBulkUpdateIcon != null)
                    command.Parameters.Add(new SqlParameter("@bShowBulkUpdateIcon", bShowBulkUpdateIcon));
               


                

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

    

    public static int dbg_View_Delete(int nViewID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nViewID ", nViewID));

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



    //public static int dbg_View_ResetViewItems(int nTableID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_View_ResetViewItems", connection))
    //        {
    //            command.CommandTimeout = 0;
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nTableID ", nTableID));

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
    public static int dbg_View_TableUpdate(int TableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_TableUpdate", connection))
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@TableID ", TableID));

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

    //public static int dbg_View_ResetOneItem(int nColumnID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_View_ResetOneItem", connection))
    //        {
    //            command.CommandTimeout = 0;
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nColumnID ", nColumnID));

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

    public static int dbg_View_ColumnUpdate(int ColumnID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_ColumnUpdate", connection))
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ColumnID ", ColumnID));

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
    //public static int dbg_View_Default_ResetColumns(int nViewID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_View_Default_ResetColumns", connection))
    //        {
    //            command.CommandTimeout = 0;
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nViewID ", nViewID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}

    public static int? dbg_View_Copy(int ViewID, int UserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_Copy", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@NewViewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@ViewID ", ViewID));
                command.Parameters.Add(new SqlParameter("@UserID ", UserID));

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
                return null;

            }
        }
    }
    
    public static View dbg_View_Detail(int nViewID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nViewID", nViewID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            View temp = new View(
                                (int)reader["ViewID"], (int)reader["TableID"],
                              (string)reader["ViewName"],
                              reader["UserID"] == DBNull.Value ? null : (int?)reader["UserID"],
                               reader["RowsPerPage"] == DBNull.Value ? null : (int?)reader["RowsPerPage"],
                               reader["SortOrder"] == DBNull.Value ? string.Empty : (string)reader["SortOrder"],
                               reader["Filter"] == DBNull.Value ? string.Empty : (string)reader["Filter"],
                               reader["ShowSearchFields"] == DBNull.Value ? null : (bool?)reader["ShowSearchFields"],
                               reader["ShowAddIcon"] == DBNull.Value ? null : (bool?)reader["ShowAddIcon"],
                               reader["ShowEditIcon"] == DBNull.Value ? null : (bool?)reader["ShowEditIcon"],
                               reader["ShowDeleteIcon"] == DBNull.Value ? null : (bool?)reader["ShowDeleteIcon"],
                               reader["ShowViewIcon"] == DBNull.Value ? null : (bool?)reader["ShowViewIcon"],
                               reader["ShowBulkUpdateIcon"] == DBNull.Value ? null : (bool?)reader["ShowBulkUpdateIcon"]
                                );

                            temp.FilterControlsInfo = reader["FilterControlsInfo"] == DBNull.Value ? string.Empty : (string)reader["FilterControlsInfo"];
                            temp.ViewPageType = reader["ViewPageType"] == DBNull.Value ? string.Empty : (string)reader["ViewPageType"];
                            temp.FixedFilter = reader["FixedFilter"] == DBNull.Value ? string.Empty : (string)reader["FixedFilter"];
                            temp.ShowFixedHeader = reader["ShowFixedHeader"] == DBNull.Value ? null : (bool?)reader["ShowFixedHeader"];

                            temp.ParentTableID = reader["ParentTableID"] == DBNull.Value ? null : (int?)reader["ParentTableID"];

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


    public static int? dbg_View_BestFittingNew(int UserID, string ViewPageType, int TableID, int? ParentTableID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_BestFittingNew", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@UserID", UserID));
                command.Parameters.Add(new SqlParameter("@ViewPageType", ViewPageType));
                command.Parameters.Add(new SqlParameter("@TableID", TableID));

                command.Parameters.Add(new SqlParameter("@ParentTableID", ParentTableID));

                SqlParameter opViewID = new SqlParameter("@ViewID", SqlDbType.Int);
                opViewID.Direction = ParameterDirection.Output;

                command.Parameters.Add(opViewID);


                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(opViewID.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();
                }               

                return null;

            }
        }      


    }


    public static int? dbg_View_Reset(int rsViewID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_Reset", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@rsViewID", rsViewID));
                
                SqlParameter opViewID = new SqlParameter("@ViewID", SqlDbType.Int);
                opViewID.Direction = ParameterDirection.Output;

                command.Parameters.Add(opViewID);


                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(opViewID.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();
                }

                return null;

            }
        }


    }


    public static int? dbg_View_CreateDash(int? UserID,  int TableID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_View_CreateDash", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@UserID", UserID));
                command.Parameters.Add(new SqlParameter("@TableID", TableID));

                SqlParameter opViewID = new SqlParameter("@ViewID", SqlDbType.Int);
                opViewID.Direction = ParameterDirection.Output;

                command.Parameters.Add(opViewID);


                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(opViewID.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();
                }

                return null;

            }
        }


    }


    //public static View dbg_View_BestFitting(int? UserID,string ViewPageType,int TableID,string ViewName)
    //{


    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_View_BestFitting", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@UserID", UserID));
    //            command.Parameters.Add(new SqlParameter("@ViewPageType", ViewPageType));
    //            command.Parameters.Add(new SqlParameter("@TableID", TableID));

    //            if (ViewName != "")
    //                command.Parameters.Add(new SqlParameter("@ViewName", ViewName));


    //            connection.Open();

    //            try
    //            {
    //                using (SqlDataReader reader = command.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        View temp = new View(
    //                            (int)reader["ViewID"], (int)reader["TableID"],
    //                          (string)reader["ViewName"],
    //                          reader["UserID"] == DBNull.Value ? null : (int?)reader["UserID"],
    //                           reader["RowsPerPage"] == DBNull.Value ? null : (int?)reader["RowsPerPage"],
    //                           reader["SortOrder"] == DBNull.Value ? string.Empty : (string)reader["SortOrder"],
    //                           reader["Filter"] == DBNull.Value ? string.Empty : (string)reader["Filter"],
    //                           reader["ShowSearchFields"] == DBNull.Value ? null : (bool?)reader["ShowSearchFields"],
    //                           reader["ShowAddIcon"] == DBNull.Value ? null : (bool?)reader["ShowAddIcon"],
    //                           reader["ShowEditIcon"] == DBNull.Value ? null : (bool?)reader["ShowEditIcon"],
    //                           reader["ShowDeleteIcon"] == DBNull.Value ? null : (bool?)reader["ShowDeleteIcon"],
    //                           reader["ShowViewIcon"] == DBNull.Value ? null : (bool?)reader["ShowViewIcon"],
    //                           reader["ShowBulkUpdateIcon"] == DBNull.Value ? null : (bool?)reader["ShowBulkUpdateIcon"]
    //                            );

    //                        temp.FilterControlsInfo = reader["FilterControlsInfo"] == DBNull.Value ? string.Empty : (string)reader["FilterControlsInfo"];
    //                        temp.ViewPageType = reader["ViewPageType"] == DBNull.Value ? string.Empty : (string)reader["ViewPageType"];

    //                        connection.Close();
    //                        connection.Dispose();
    //                        return temp;
    //                    }

    //                }
    //            }
    //            catch
    //            {
                  
    //            }

    //            connection.Close();
    //            connection.Dispose();

    //            return null;

    //        }
    //    }


        

    //}

    #endregion



    #region ViewItem



    public static int dbg_ViewItem_Insert(ViewItem p_ViewItem)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ViewItem_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nViewID", p_ViewItem.ViewID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_ViewItem.ColumnID));


                if (p_ViewItem.ColumnIndex != null)
                    command.Parameters.Add(new SqlParameter("@nColumnIndex", p_ViewItem.ColumnIndex));

                //if (p_ViewItem.Heading != "")
                //    command.Parameters.Add(new SqlParameter("@sHeading", p_ViewItem.Heading));

                if (p_ViewItem.SearchField != null)
                    command.Parameters.Add(new SqlParameter("@bSearchField", p_ViewItem.SearchField));

                if (p_ViewItem.FilterField != null)
                    command.Parameters.Add(new SqlParameter("@bFilterField", p_ViewItem.FilterField));

                if (!string.IsNullOrEmpty(p_ViewItem.Alignment))
                    command.Parameters.Add(new SqlParameter("@sAlignment", p_ViewItem.Alignment));


                if (p_ViewItem.Width != null)
                    command.Parameters.Add(new SqlParameter("@nWidth", p_ViewItem.Width));
                if (p_ViewItem.ShowTotal != null)
                    command.Parameters.Add(new SqlParameter("@bShowTotal", p_ViewItem.ShowTotal));
                if (!string.IsNullOrEmpty(p_ViewItem.SummaryCellBackColor ))
                    command.Parameters.Add(new SqlParameter("@sSummaryCellBackColor", p_ViewItem.SummaryCellBackColor));


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


    public static int dbg_ViewItem_Update(ViewItem p_ViewItem)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ViewItem_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nViewItemID", p_ViewItem.ViewItemID));

                command.Parameters.Add(new SqlParameter("@nViewID", p_ViewItem.ViewID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_ViewItem.ColumnID));


                if (p_ViewItem.ColumnIndex != null)
                    command.Parameters.Add(new SqlParameter("@nColumnIndex", p_ViewItem.ColumnIndex));

                //if (p_ViewItem.Heading != "")
                //    command.Parameters.Add(new SqlParameter("@sHeading", p_ViewItem.Heading));

                if (p_ViewItem.SearchField != null)
                    command.Parameters.Add(new SqlParameter("@bSearchField", p_ViewItem.SearchField));

                if (p_ViewItem.FilterField != null)
                    command.Parameters.Add(new SqlParameter("@bFilterField", p_ViewItem.FilterField));

                if ( !string.IsNullOrEmpty( p_ViewItem.Alignment ))
                    command.Parameters.Add(new SqlParameter("@sAlignment", p_ViewItem.Alignment));


                if (p_ViewItem.Width != null)
                    command.Parameters.Add(new SqlParameter("@nWidth", p_ViewItem.Width));
                if (p_ViewItem.ShowTotal != null)
                    command.Parameters.Add(new SqlParameter("@bShowTotal", p_ViewItem.ShowTotal));
                if (!string.IsNullOrEmpty(p_ViewItem.SummaryCellBackColor))
                    command.Parameters.Add(new SqlParameter("@sSummaryCellBackColor", p_ViewItem.SummaryCellBackColor));

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


    public static DataTable dbg_ViewItem_Select(int? nViewID, int? nColumnID, string sHeading,
        bool? bSearchField,bool? bFilterField,string sAlignment,int? nWidth,bool? bShowTotal, 
        string sSummaryCellBackColor, 
        string  sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum )
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ViewItem_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                if(nViewID!=null)
                    command.Parameters.Add(new SqlParameter("@nViewID", nViewID));

                if (nColumnID != null)
                    command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID));
                if (sHeading != "")
                    command.Parameters.Add(new SqlParameter("@sHeading", sHeading));
                if (bSearchField != null)
                    command.Parameters.Add(new SqlParameter("@bSearchField", bSearchField));
                if (bFilterField != null)
                    command.Parameters.Add(new SqlParameter("@bFilterField", bFilterField));
                if (sAlignment != "")
                    command.Parameters.Add(new SqlParameter("@sAlignment", sAlignment));
                if (nWidth != null)
                    command.Parameters.Add(new SqlParameter("@nWidth", nWidth));
                if (bShowTotal != null)
                    command.Parameters.Add(new SqlParameter("@bShowTotal", bShowTotal));
                if (sSummaryCellBackColor != "")
                    command.Parameters.Add(new SqlParameter("@sSummaryCellBackColor", sSummaryCellBackColor));
               


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



    public static int dbg_ViewItem_Delete(int nViewItemID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ViewItem_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nViewItemID ", nViewItemID));

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


    //public static int dbg_CreateDefaultViewItem(int nViewID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_CreateDefaultViewItem", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nViewID ", nViewID));

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




    //public static int dbg_ResetViews(int nViewID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbg_ResetViews", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nViewID ", nViewID));

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


    public static ViewItem dbg_ViewItem_Detail(int nViewItemID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_ViewItem_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nViewItemID", nViewItemID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewItem temp = new ViewItem(
                                (int)reader["ViewItemID"], (int)reader["ViewID"], (int)reader["ColumnID"],
                               reader["SearchField"] == DBNull.Value ? null : (bool?)reader["SearchField"],
                                reader["FilterField"] == DBNull.Value ? null : (bool?)reader["FilterField"],
                                 reader["Alignment"] == DBNull.Value ? string.Empty : (string)reader["Alignment"],
                              reader["Width"] == DBNull.Value ? null : (int?)reader["Width"],
                               reader["ShowTotal"] == DBNull.Value ? null : (bool?)reader["ShowTotal"],
                              reader["SummaryCellBackColor"] == DBNull.Value ? string.Empty : (string)reader["SummaryCellBackColor"]

                                );

                            temp.ColumnIndex = reader["ColumnIndex"] == DBNull.Value ? null : (int?)reader["ColumnIndex"];
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