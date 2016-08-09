using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



/// <summary>
/// Summary description for GraphManager
/// </summary>
public class GraphManager
{
	public GraphManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static int ets_Record_RecordCount(string sTableIDs, DateTime dDateFrom, DateTime dDateTo)
    {
        return ets_Record_RecordCount(sTableIDs, dDateFrom, dDateTo, null);
    }

    public static int ets_Record_RecordCount(string sTableIDs, DateTime dDateFrom, DateTime dDateTo, string sColumnID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_RecordCount", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@sTableIDs", sTableIDs));
                command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));
                command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));
                if (!String.IsNullOrEmpty(sColumnID))
                    command.Parameters.Add(new SqlParameter("@sColumnID", sColumnID));


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

    public static int ets_GraphOption_Insert(GraphOption p_GraphOption)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOption_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

               

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nAccountID", p_GraphOption.AccountID));
                command.Parameters.Add(new SqlParameter("@nGraphPanel", p_GraphOption.GraphPanel));

                if (p_GraphOption.Heading != "")
                    command.Parameters.Add(new SqlParameter("@sHeading", p_GraphOption.Heading));
                if (p_GraphOption.TimePeriod != "")
                    command.Parameters.Add(new SqlParameter("@sTimePeriod", p_GraphOption.TimePeriod));
                if (p_GraphOption.ShowLimits != null)
                    command.Parameters.Add(new SqlParameter("@bShowLimits", p_GraphOption.ShowLimits));
                if (p_GraphOption.ShowMissing != null)
                    command.Parameters.Add(new SqlParameter("@bShowMissing", p_GraphOption.ShowMissing));
                if (p_GraphOption.FromDate != null)
                    command.Parameters.Add(new SqlParameter("@dFromDate", p_GraphOption.FromDate));
                if (p_GraphOption.ToDate != null)
                    command.Parameters.Add(new SqlParameter("@dToDate", p_GraphOption.ToDate));
                if (p_GraphOption.Legend != "")
                    command.Parameters.Add(new SqlParameter("@sLegend", p_GraphOption.Legend));
                if (p_GraphOption.Width != null)
                    command.Parameters.Add(new SqlParameter("@nWidth", p_GraphOption.Width));
                if (p_GraphOption.Height != null)
                    command.Parameters.Add(new SqlParameter("@nHeight", p_GraphOption.Height));
                if (p_GraphOption.UserReportDate != null)
                    command.Parameters.Add(new SqlParameter("@bUserReportDate", p_GraphOption.UserReportDate));

                if (p_GraphOption.CustomTimePeriod != "")
                    command.Parameters.Add(new SqlParameter("@sCustomTimePeriod", p_GraphOption.CustomTimePeriod));
                if (p_GraphOption.Display3D != null)
                    command.Parameters.Add(new SqlParameter("@bDisplay3D", p_GraphOption.Display3D));

                if (p_GraphOption.ReportChart != null)
                    command.Parameters.Add(new SqlParameter("@bReportChart", p_GraphOption.ReportChart));

                if (p_GraphOption.WarningCaption != "")
                    command.Parameters.Add(new SqlParameter("@sWarningCaption", p_GraphOption.WarningCaption));
                if (p_GraphOption.WarningValue != null)
                    command.Parameters.Add(new SqlParameter("@nWarningValue", p_GraphOption.WarningValue));
                if (p_GraphOption.WarningColor != "")
                    command.Parameters.Add(new SqlParameter("@sWarningColor", p_GraphOption.WarningColor));
                if (p_GraphOption.ExceedanceCaption != "")
                    command.Parameters.Add(new SqlParameter("@sExceedanceCaption", p_GraphOption.ExceedanceCaption));
                if (p_GraphOption.ExceedanceValue != null)
                    command.Parameters.Add(new SqlParameter("@nExceedanceValue", p_GraphOption.ExceedanceValue));
                if (p_GraphOption.ExceedanceColor != "")
                    command.Parameters.Add(new SqlParameter("@sExceedanceColor", p_GraphOption.ExceedanceColor));
                if (p_GraphOption.DateFormat != "")
                    command.Parameters.Add(new SqlParameter("@sDateFormat", p_GraphOption.DateFormat));
                if (!String.IsNullOrEmpty(p_GraphOption.SubHeading))
                    command.Parameters.Add(new SqlParameter("@sSubHeading", p_GraphOption.SubHeading));
                if (p_GraphOption.GraphDefinitionID != null)
                    command.Parameters.Add(new SqlParameter("@nGraphDefinitionID", p_GraphOption.GraphDefinitionID));
                if (p_GraphOption.YAxisHighestValue != null)
                    command.Parameters.Add(new SqlParameter("@nYAxisHighestValue", p_GraphOption.YAxisHighestValue));
                if (p_GraphOption.YAxisLowestValue != null)
                    command.Parameters.Add(new SqlParameter("@nYAxisLowestValue", p_GraphOption.YAxisLowestValue));
                if (p_GraphOption.YAxisInterval != null)
                    command.Parameters.Add(new SqlParameter("@nYAxisInterval", p_GraphOption.YAxisInterval));

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



    public static int ets_GraphOption_Update(GraphOption p_GraphOption)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOption_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

              

                command.Parameters.Add(new SqlParameter("@nGraphOptionID", p_GraphOption.GraphOptionID));

                command.Parameters.Add(new SqlParameter("@nAccountID", p_GraphOption.AccountID));
                command.Parameters.Add(new SqlParameter("@nGraphPanel", p_GraphOption.GraphPanel));

                if (p_GraphOption.Heading != "")
                    command.Parameters.Add(new SqlParameter("@sHeading", p_GraphOption.Heading));
                if (p_GraphOption.TimePeriod != "")
                    command.Parameters.Add(new SqlParameter("@sTimePeriod", p_GraphOption.TimePeriod));
                if (p_GraphOption.ShowLimits != null)
                    command.Parameters.Add(new SqlParameter("@bShowLimits", p_GraphOption.ShowLimits));
                if (p_GraphOption.ShowMissing != null)
                    command.Parameters.Add(new SqlParameter("@bShowMissing", p_GraphOption.ShowMissing));
                if (p_GraphOption.FromDate != null)
                    command.Parameters.Add(new SqlParameter("@dFromDate", p_GraphOption.FromDate));
                if (p_GraphOption.ToDate != null)
                    command.Parameters.Add(new SqlParameter("@dToDate", p_GraphOption.ToDate));
                if (p_GraphOption.Legend != "")
                    command.Parameters.Add(new SqlParameter("@sLegend", p_GraphOption.Legend));
                if (p_GraphOption.Width != null)
                    command.Parameters.Add(new SqlParameter("@nWidth", p_GraphOption.Width));
                if (p_GraphOption.Height != null)
                    command.Parameters.Add(new SqlParameter("@nHeight", p_GraphOption.Height));
                if (p_GraphOption.UserReportDate != null)
                    command.Parameters.Add(new SqlParameter("@bUserReportDate", p_GraphOption.UserReportDate));

                if (p_GraphOption.CustomTimePeriod != "")
                    command.Parameters.Add(new SqlParameter("@sCustomTimePeriod", p_GraphOption.CustomTimePeriod));
                if (p_GraphOption.Display3D != null)
                    command.Parameters.Add(new SqlParameter("@bDisplay3D", p_GraphOption.Display3D));

                if (p_GraphOption.ReportChart != null)
                    command.Parameters.Add(new SqlParameter("@bReportChart", p_GraphOption.ReportChart));

                if (p_GraphOption.WarningCaption != "")
                    command.Parameters.Add(new SqlParameter("@sWarningCaption", p_GraphOption.WarningCaption));
                if (p_GraphOption.WarningValue != null)
                    command.Parameters.Add(new SqlParameter("@nWarningValue", p_GraphOption.WarningValue));
                if (p_GraphOption.WarningColor != "")
                    command.Parameters.Add(new SqlParameter("@sWarningColor", p_GraphOption.WarningColor));
                if (p_GraphOption.ExceedanceCaption != "")
                    command.Parameters.Add(new SqlParameter("@sExceedanceCaption", p_GraphOption.ExceedanceCaption));
                if (p_GraphOption.ExceedanceValue != null)
                    command.Parameters.Add(new SqlParameter("@nExceedanceValue", p_GraphOption.ExceedanceValue));
                if (p_GraphOption.ExceedanceColor != "")
                    command.Parameters.Add(new SqlParameter("@sExceedanceColor", p_GraphOption.ExceedanceColor));
                if (p_GraphOption.DateFormat != "")
                    command.Parameters.Add(new SqlParameter("@sDateFormat", p_GraphOption.DateFormat));
                if (!String.IsNullOrEmpty(p_GraphOption.SubHeading))
                    command.Parameters.Add(new SqlParameter("@sSubHeading", p_GraphOption.SubHeading));
                if (p_GraphOption.GraphDefinitionID != null)
                    command.Parameters.Add(new SqlParameter("@nGraphDefinitionID", p_GraphOption.GraphDefinitionID));
                if (p_GraphOption.YAxisHighestValue != null)
                    command.Parameters.Add(new SqlParameter("@nYAxisHighestValue", p_GraphOption.YAxisHighestValue));
                if (p_GraphOption.YAxisLowestValue != null)
                    command.Parameters.Add(new SqlParameter("@nYAxisLowestValue", p_GraphOption.YAxisLowestValue));
                if (p_GraphOption.YAxisInterval != null)
                    command.Parameters.Add(new SqlParameter("@nYAxisInterval", p_GraphOption.YAxisInterval));

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



    public static int ets_GraphOption_Delete(int nGraphOptionID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOption_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nGraphOptionID ", nGraphOptionID));

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




    //public static int ets_GraphOption_Restore(int nGraphOptionID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_GraphOption_Restore", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nGraphOptionID ", nGraphOptionID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}





    public static GraphOption ets_GraphOption_Detail(int nGraphOptionID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOption_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
              
                command.Parameters.Add(new SqlParameter("@nGraphOptionID ", nGraphOptionID));

                connection.Open();



                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GraphOption temp = new GraphOption(
                                (int)reader["GraphOptionID"],
                                (int)reader["AccountID"],
                                (int)reader["GraphPanel"],
                                  reader["Heading"] == DBNull.Value ? "" : (string)reader["Heading"],
                                reader["TimePeriod"] == DBNull.Value ? "" : (string)reader["TimePeriod"]
                                );

                            temp.ShowLimits = reader["ShowLimits"] == DBNull.Value ? null : (bool?)bool.Parse(reader["ShowLimits"].ToString());
                            temp.ShowMissing = reader["ShowMissing"] == DBNull.Value ? null : (bool?)bool.Parse(reader["ShowMissing"].ToString());
                            temp.FromDate = reader["FromDate"] == DBNull.Value ? null : (DateTime?)DateTime.Parse(reader["FromDate"].ToString());
                            temp.ToDate = reader["ToDate"] == DBNull.Value ? null : (DateTime?)DateTime.Parse(reader["ToDate"].ToString());
                            temp.Legend = reader["Legend"] == DBNull.Value ? "" : (string)reader["Legend"];
                            temp.Width = reader["Width"] == DBNull.Value ? null : (double?)double.Parse(reader["Width"].ToString());
                            temp.Height = reader["Height"] == DBNull.Value ? null : (double?)double.Parse(reader["Height"].ToString());
                            temp.UserReportDate = reader["UserReportDate"] == DBNull.Value ? null : (bool?)bool.Parse(reader["UserReportDate"].ToString());
                            temp.CustomTimePeriod = reader["CustomTimePeriod"] == DBNull.Value ? "" : (string)reader["CustomTimePeriod"];
                            temp.Display3D = reader["Display3D"] == DBNull.Value ? null : (bool?)bool.Parse(reader["Display3D"].ToString());
                            temp.WarningCaption = reader["WarningCaption"] == DBNull.Value ? "" : (string)(reader["WarningCaption"].ToString());
                            temp.WarningValue = reader["WarningValue"] == DBNull.Value ? null : (double?)double.Parse(reader["WarningValue"].ToString());
                            temp.WarningColor = reader["WarningColor"] == DBNull.Value ? "" : (string)(reader["WarningColor"].ToString());
                            temp.ExceedanceCaption = reader["ExceedanceCaption"] == DBNull.Value ? "" : (string)(reader["ExceedanceCaption"].ToString());
                            temp.ExceedanceValue = reader["ExceedanceValue"] == DBNull.Value ? null : (double?)double.Parse(reader["ExceedanceValue"].ToString());
                            temp.ExceedanceColor = reader["ExceedanceColor"] == DBNull.Value ? null : (string)(reader["ExceedanceColor"].ToString());
                            temp.DateFormat = reader["Height"] == DBNull.Value ? "" : (string)(reader["DateFormat"].ToString());
                            temp.SubHeading = reader["SubHeading"] == DBNull.Value ? "" : (string)reader["SubHeading"];
                            temp.GraphDefinitionID = reader["GraphDefinitionID"] == DBNull.Value ? null : (int?)reader["GraphDefinitionID"];
                            temp.YAxisHighestValue = reader["YAxisHighestValue"] == DBNull.Value ? null : (double?)double.Parse(reader["YAxisHighestValue"].ToString());
                            temp.YAxisLowestValue = reader["YAxisLowestValue"] == DBNull.Value ? null : (double?)double.Parse(reader["YAxisLowestValue"].ToString());
                            temp.YAxisInterval = reader["YAxisInterval"] == DBNull.Value ? null : (double?)double.Parse(reader["YAxisInterval"].ToString());

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





    public static DataTable ets_GraphOption_Select(int? nAccountID, int? nGraphPanel, bool? bReportChart,
     bool? bIsActive, string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOption_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nAccountID != null)
                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nGraphPanel != null)
                    command.Parameters.Add(new SqlParameter("@nGraphPanel", nGraphPanel));

                if (bReportChart != null)
                    command.Parameters.Add(new SqlParameter("@bReportChart", bReportChart));

                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));
              

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "GraphOptionID"; sOrderDirection = "DESC"; }

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



    ////////



    public static int ets_GraphOptionDetail_Insert(GraphOptionDetail p_GraphOptionDetail)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOptionDetail_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nGraphOptionID", p_GraphOptionDetail.GraphOptionID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_GraphOptionDetail.TableID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_GraphOptionDetail.ColumnID));

                if (p_GraphOptionDetail.Label != null)
                    command.Parameters.Add(new SqlParameter("@sLabel", p_GraphOptionDetail.Label));
                if (p_GraphOptionDetail.Axis != "")
                    command.Parameters.Add(new SqlParameter("@sAxis", p_GraphOptionDetail.Axis));
                if (p_GraphOptionDetail.GraphOrder != null)
                    command.Parameters.Add(new SqlParameter("@nGraphOrder", p_GraphOptionDetail.GraphOrder));
                if (p_GraphOptionDetail.Scale != null)
                    command.Parameters.Add(new SqlParameter("@nScale", p_GraphOptionDetail.Scale));
                if (p_GraphOptionDetail.GraphType != "")
                    command.Parameters.Add(new SqlParameter("@sGraphType", p_GraphOptionDetail.GraphType));
                if (p_GraphOptionDetail.Colour != "")
                    command.Parameters.Add(new SqlParameter("@sColour", p_GraphOptionDetail.Colour));
                if (p_GraphOptionDetail.High != null)
                    command.Parameters.Add(new SqlParameter("@dHigh", p_GraphOptionDetail.High));
                if (p_GraphOptionDetail.Low != null)
                    command.Parameters.Add(new SqlParameter("@dLow", p_GraphOptionDetail.Low));
                if (p_GraphOptionDetail.GraphSeriesColumnID != null)
                    command.Parameters.Add(new SqlParameter("@sGraphSeriesColumnID", p_GraphOptionDetail.GraphSeriesColumnID));
                if (p_GraphOptionDetail.GraphSeriesID != null)
                    command.Parameters.Add(new SqlParameter("@sGraphSeriesID", p_GraphOptionDetail.GraphSeriesID));

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



    public static int ets_GraphOptionDetail_Update(GraphOptionDetail p_GraphOptionDetail)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOptionDetail_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nGraphOptionDetailID", p_GraphOptionDetail.GraphOptionDetailID));
                command.Parameters.Add(new SqlParameter("@nGraphOptionID", p_GraphOptionDetail.GraphOptionID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_GraphOptionDetail.TableID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_GraphOptionDetail.ColumnID));

                if (p_GraphOptionDetail.Label != null)
                    command.Parameters.Add(new SqlParameter("@sLabel", p_GraphOptionDetail.Label));
                if (p_GraphOptionDetail.Axis != "")
                    command.Parameters.Add(new SqlParameter("@sAxis", p_GraphOptionDetail.Axis));
                if (p_GraphOptionDetail.GraphOrder != null)
                    command.Parameters.Add(new SqlParameter("@nGraphOrder", p_GraphOptionDetail.GraphOrder));
                if (p_GraphOptionDetail.Scale != null)
                    command.Parameters.Add(new SqlParameter("@nScale", p_GraphOptionDetail.Scale));
                if (p_GraphOptionDetail.GraphType != "")
                    command.Parameters.Add(new SqlParameter("@sGraphType", p_GraphOptionDetail.GraphType));
                if (p_GraphOptionDetail.Colour != "")
                    command.Parameters.Add(new SqlParameter("@sColour", p_GraphOptionDetail.Colour));
                if (p_GraphOptionDetail.High != null)
                    command.Parameters.Add(new SqlParameter("@dHigh", p_GraphOptionDetail.High));
                if (p_GraphOptionDetail.Low != null)
                    command.Parameters.Add(new SqlParameter("@dLow", p_GraphOptionDetail.Low));
                if (p_GraphOptionDetail.GraphSeriesColumnID != null)
                    command.Parameters.Add(new SqlParameter("@sGraphSeriesColumnID", p_GraphOptionDetail.GraphSeriesColumnID));
                if (p_GraphOptionDetail.GraphSeriesID != null)
                    command.Parameters.Add(new SqlParameter("@sGraphSeriesID", p_GraphOptionDetail.GraphSeriesID));

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


    public static int ets_GraphOptionDetail_Delete(int nGraphOptionDetailID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOptionDetail_Delete", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nGraphOptionDetailID ", nGraphOptionDetailID));

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

    
    public static GraphOptionDetail ets_GraphOptionDetail_Detail(int nGraphOptionDetailID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_GraphOptionDetail_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nGraphOptionDetailID ", nGraphOptionDetailID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GraphOptionDetail temp = new GraphOptionDetail((int)reader["GraphOptionDetailID"],
                                (int)reader["GraphOptionID"],
                                (int)reader["TableID"], (int)reader["ColumnID"]
                                );

                            temp.Label = reader["Label"] == DBNull.Value ? null : (string)reader["Label"];
                            temp.Axis = reader["Axis"] == DBNull.Value ? "" : (string)reader["Axis"];
                            temp.GraphOrder = reader["GraphOrder"] == DBNull.Value ? null : (int?)int.Parse(reader["GraphOrder"].ToString());
                            temp.Scale = reader["Scale"] == DBNull.Value ? null : (double?)double.Parse(reader["Scale"].ToString());
                            temp.GraphType = reader["GraphType"] == DBNull.Value ? "" : (string)reader["GraphType"];
                            temp.Colour = reader["Colour"] == DBNull.Value ? "" : (string)reader["Colour"];
                            //temp.LocationID = reader["LocationID"] == DBNull.Value ? null : (int?)int.Parse(reader["LocationID"].ToString());
                            temp.High = reader["High"] == DBNull.Value ? null : (double?)double.Parse(reader["High"].ToString());
                            temp.Low = reader["Low"] == DBNull.Value ? null : (double?)double.Parse(reader["Low"].ToString());
                            temp.GraphSeriesColumnID = reader["GraphSeriesColumnID"] == DBNull.Value ? "" : (string)reader["GraphSeriesColumnID"];
                            temp.GraphSeriesID = reader["GraphSeriesID"] == DBNull.Value ? "" : (string)reader["GraphSeriesID"];

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


    public static DataTable ets_GraphOptionDetail_Select(int? nGraphOptionID, int? nTableID, 
     string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphOptionDetail_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nGraphOptionID != null)
                    command.Parameters.Add(new SqlParameter("@nGraphOptionID", nGraphOptionID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

               


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "GraphOptionDetailID"; sOrderDirection = "ASC"; }

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

    ////////

    public static int ets_GraphDefinition_Insert(GraphDefinition p_GraphDefinition)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphDefinition_Insert", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                if (p_GraphDefinition.AccountID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nAccountID", p_GraphDefinition.AccountID.Value));
                if (!String.IsNullOrEmpty(p_GraphDefinition.DefinitionName))
                    command.Parameters.Add(new SqlParameter("@sDefinitionName", p_GraphDefinition.DefinitionName));
                if (!String.IsNullOrEmpty(p_GraphDefinition.Definition))
                    command.Parameters.Add(new SqlParameter("@sDefinition", p_GraphDefinition.Definition));
                if (p_GraphDefinition.IsSystem.HasValue)
                    command.Parameters.Add(new SqlParameter("@bIsSystem", p_GraphDefinition.IsSystem.Value));
                if (p_GraphDefinition.IsHidden.HasValue)
                    command.Parameters.Add(new SqlParameter("@bIsHidden", p_GraphDefinition.IsHidden.Value));
                if (p_GraphDefinition.TableID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_GraphDefinition.TableID.Value));
                if (p_GraphDefinition.ColumnID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nColumnID", p_GraphDefinition.ColumnID.Value));
                if (!String.IsNullOrEmpty(p_GraphDefinition.DefinitionKey))
                    command.Parameters.Add(new SqlParameter("@sDefinitionKey", p_GraphDefinition.DefinitionKey));
                if (p_GraphDefinition.DataColumn1ID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nDataColumn1ID", p_GraphDefinition.DataColumn1ID.Value));
                if (p_GraphDefinition.DataColumn2ID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nDataColumn2ID", p_GraphDefinition.DataColumn2ID.Value));
                if (p_GraphDefinition.DataColumn3ID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nDataColumn3ID", p_GraphDefinition.DataColumn3ID.Value));
                if (p_GraphDefinition.DataColumn4ID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nDataColumn4ID", p_GraphDefinition.DataColumn4ID.Value));

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


    public static int ets_GraphDefinition_Update(GraphDefinition p_GraphDefinition)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphDefinition_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                
                command.Parameters.Add(new SqlParameter("@nGraphDefinitionID", p_GraphDefinition.GraphDefinitionID));
                if (p_GraphDefinition.AccountID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nAccountID", p_GraphDefinition.AccountID.Value));
                if (!String.IsNullOrEmpty(p_GraphDefinition.DefinitionName))
                    command.Parameters.Add(new SqlParameter("@sDefinitionName", p_GraphDefinition.DefinitionName));
                if (!String.IsNullOrEmpty(p_GraphDefinition.Definition))
                    command.Parameters.Add(new SqlParameter("@sDefinition", p_GraphDefinition.Definition));
                if (p_GraphDefinition.IsSystem.HasValue)
                    command.Parameters.Add(new SqlParameter("@bIsSystem", p_GraphDefinition.IsSystem.Value));
                if (p_GraphDefinition.IsHidden.HasValue)
                    command.Parameters.Add(new SqlParameter("@bIsHidden", p_GraphDefinition.IsHidden.Value));
                command.Parameters.Add(new SqlParameter("@nTableID", p_GraphDefinition.TableID));
                command.Parameters.Add(new SqlParameter("@nColumnID", p_GraphDefinition.ColumnID));
                command.Parameters.Add(new SqlParameter("@sDefinitionKey", p_GraphDefinition.DefinitionKey));
                command.Parameters.Add(new SqlParameter("@nDataColumn1ID", p_GraphDefinition.DataColumn1ID));
                command.Parameters.Add(new SqlParameter("@nDataColumn2ID", p_GraphDefinition.DataColumn2ID));
                command.Parameters.Add(new SqlParameter("@nDataColumn3ID", p_GraphDefinition.DataColumn3ID));
                command.Parameters.Add(new SqlParameter("@nDataColumn4ID", p_GraphDefinition.DataColumn4ID));

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


    public static int ets_GraphDefinition_Delete(int nGraphDefinitionID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphDefinition_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nGraphDefinitionID", nGraphDefinitionID));

                connection.Open();

                try
                {

                    SqlParameter retval = command.Parameters.Add("x", SqlDbType.Int);
                    retval.Direction = ParameterDirection.ReturnValue;
                    command.ExecuteNonQuery();
                    int returnValue = (int)command.Parameters["x"].Value;

                    connection.Close();
                    connection.Dispose();

                    return returnValue;
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                    return -1;
                }

            }
        }
    }


    public static int ets_GraphDefinition_Restore(int nGraphDefinitionID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphDefinition_Restore", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nGraphDefinitionID", nGraphDefinitionID));

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


    public static GraphDefinition ets_GraphDefinition_Detail(int nGraphDefinitionID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphDefinition_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nGraphDefinitionID ", nGraphDefinitionID));

                connection.Open();


                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GraphDefinition temp = new GraphDefinition(
                                (int)reader["GraphDefinitionID"],
                                reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
                                reader["DefinitionName"] == DBNull.Value ? "" : (string)reader["DefinitionName"],
                                reader["Definition"] == DBNull.Value ? "" : (string)reader["Definition"],
                                reader["IsSystem"] == DBNull.Value ? null : (bool?)reader["IsSystem"],
                                reader["IsHidden"] == DBNull.Value ? null : (bool?)reader["IsHidden"],
                                reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
                                reader["ColumnID"] == DBNull.Value ? null : (int?)reader["ColumnID"],
                                reader["DefinitionKey"] == DBNull.Value ? "" : (string)reader["DefinitionKey"],
                                reader["IsActive"] == DBNull.Value ? null : (bool?)reader["IsActive"],
                                reader["DataColumn1ID"] == DBNull.Value ? null : (int?)reader["DataColumn1ID"],
                                reader["DataColumn2ID"] == DBNull.Value ? null : (int?)reader["DataColumn2ID"],
                                reader["DataColumn3ID"] == DBNull.Value ? null : (int?)reader["DataColumn3ID"],
                                reader["DataColumn4ID"] == DBNull.Value ? null : (int?)reader["DataColumn4ID"]
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


    public static DataTable ets_GraphDefinition_Select(int? nAccountID,
        string sDefinitionName, string sDefinition,
        bool? bIsSystem, bool? bIsHidden,
        int? nTableID, int? nColumnID,
        string sDefinitionKey,
        bool? bIsActive,
        int ? nDataColumn1ID, int? nDataColumn2ID, int? nDataColumn3ID, int? nDataColumn4ID,
        string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_GraphDefinition_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nAccountID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID.Value));
                if (!String.IsNullOrEmpty(sDefinitionName))
                    command.Parameters.Add(new SqlParameter("@sDefinitionName", sDefinitionName));
                if (!String.IsNullOrEmpty(sDefinition))
                    command.Parameters.Add(new SqlParameter("@sDefinition", sDefinition));
                if (bIsSystem.HasValue)
                    command.Parameters.Add(new SqlParameter("@bIsSystem", bIsSystem.Value));
                if (bIsHidden.HasValue)
                    command.Parameters.Add(new SqlParameter("@bIsHidden", bIsHidden.Value));
                if (nTableID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID.Value));
                if (nColumnID.HasValue)
                    command.Parameters.Add(new SqlParameter("@nColumnID", nColumnID.Value));
                if (!String.IsNullOrEmpty(sDefinitionKey))
                    command.Parameters.Add(new SqlParameter("@sDefinitionKey", sDefinitionKey));
                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));
                if (nDataColumn1ID != null)
                    command.Parameters.Add(new SqlParameter("@nDataColumn1ID", nDataColumn1ID));
                if (nDataColumn2ID != null)
                    command.Parameters.Add(new SqlParameter("@nDataColumn2ID", nDataColumn2ID));
                if (nDataColumn3ID != null)
                    command.Parameters.Add(new SqlParameter("@nDataColumn3ID", nDataColumn3ID));
                if (nDataColumn4ID != null)
                    command.Parameters.Add(new SqlParameter("@nDataColumn4ID", nDataColumn4ID));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                {
                    sOrder = "GraphDefinitionID";
                    sOrderDirection = "DESC";
                }
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

                return null;
            }
        }
    }
}



[Serializable]
public class GraphOptionDetail
{
    private int? _iGraphOptionDetailID;
    private int? _iGraphOptionID;
    private int? _iTableID;
    private int? _iColumnID;
    private string _strLabel;
    private string _strAxis;
    private int? _iGraphOrder;
    private double? _dScale;
    private string _strGraphType;
    private string _strColour;
    private double? _dHigh;
    private double? _dLow;

    public GraphOptionDetail(int? p_iGraphOptionDetailID, int? p_iGraphOptionID, int? p_iTableID
        , int? p_iColumnID)
    {

        _iGraphOptionDetailID = p_iGraphOptionDetailID;
        _iGraphOptionID = p_iGraphOptionID;
        _iTableID = p_iTableID;
        _iColumnID = p_iColumnID;

    }

    public int? GraphOptionDetailID
    {
        get { return _iGraphOptionDetailID; }
        set { _iGraphOptionDetailID = value; }
    }
    public int? GraphOptionID
    {
        get { return _iGraphOptionID; }
        set { _iGraphOptionID = value; }
    }
    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }
    public int? ColumnID
    {
        get { return _iColumnID; }
        set { _iColumnID = value; }
    }
    public string Label
    {
        get { return _strLabel; }
        set { _strLabel = value; }
    }

    public string Axis
    {
        get { return _strAxis; }
        set { _strAxis = value; }
    }



    public int? GraphOrder
    {
        get { return _iGraphOrder; }
        set { _iGraphOrder = value; }
    }
    public double? Scale
    {
        get { return _dScale; }
        set { _dScale = value; }
    }

    public string GraphType
    {
        get { return _strGraphType; }
        set { _strGraphType = value; }
    }

    public string Colour
    {
        get { return _strColour; }
        set { _strColour = value; }
    }

    public double? High
    {
        get { return _dHigh; }
        set { _dHigh = value; }
    }
    public double? Low
    {
        get { return _dLow; }
        set { _dLow = value; }
    }

    public string GraphSeriesColumnID { get; set; }
    public string GraphSeriesID { get; set; }
}


[Serializable]
public class GraphOption
{
    private int? _iGraphOptionID;
    private int? _iAccountID;
    private int? _iGraphPanel;
    private string _strHeading;
    private bool? _bShowLimits;
    private bool? _bShowMissing;
    private string _strTimePeriod;
    private DateTime? _FromDate;
    private DateTime? _ToDate;
    private string _strLegend;
    private double? _dWidth;
    private double? _dHeight;
    private bool? _bUserReportDate;
    private string _strCustomTimePeriod;
    private bool? _bDisplay3D;
    private bool? _bReportChart;
    private bool? _bIsActive;

    public GraphOption(int? p_iGraphOptionID, int? p_iAccountID, int? p_iGraphPanel,
        string p_strHeading, string p_strTimePeriod)
    {
        _iGraphOptionID = p_iGraphOptionID;
        _iAccountID = p_iAccountID;
        _iGraphPanel = p_iGraphPanel;
        _strHeading = p_strHeading;
        _strTimePeriod = p_strTimePeriod;
    }
    public bool? IsActive
    {
        get { return _bIsActive; }
        set { _bIsActive = value; }
    }


    public bool? ReportChart
    {
        get { return _bReportChart; }
        set { _bReportChart = value; }
    }

    public int? GraphOptionID
    {
        get { return _iGraphOptionID; }
        set { _iGraphOptionID = value; }
    }
    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public int? GraphPanel
    {
        get { return _iGraphPanel; }
        set { _iGraphPanel = value; }
    }

    public string Heading
    {
        get { return _strHeading; }
        set { _strHeading = value; }
    }

    public string TimePeriod
    {
        get { return _strTimePeriod; }
        set { _strTimePeriod = value; }
    }


    public bool? ShowLimits
    {
        get { return _bShowLimits; }
        set { _bShowLimits = value; }
    }
    public bool? ShowMissing
    {
        get { return _bShowMissing; }
        set { _bShowMissing = value; }
    }
    public DateTime? FromDate
    {
        get { return _FromDate; }
        set { _FromDate = value; }
    }
    public DateTime? ToDate
    {
        get { return _ToDate; }
        set { _ToDate = value; }
    }
    public string Legend
    {
        get { return _strLegend; }
        set { _strLegend = value; }
    }
    public double? Width
    {
        get { return _dWidth; }
        set { _dWidth = value; }
    }
    public double? Height
    {
        get { return _dHeight; }
        set { _dHeight = value; }
    }
    public bool? UserReportDate
    {
        get { return _bUserReportDate; }
        set { _bUserReportDate = value; }
    }

    public string CustomTimePeriod
    {
        get { return _strCustomTimePeriod; }
        set { _strCustomTimePeriod = value; }
    }
    public bool? Display3D
    {
        get { return _bDisplay3D; }
        set { _bDisplay3D = value; }
    }
    public string WarningCaption { get; set; }
    public double? WarningValue { get; set; }

    public string WarningColor { get; set; }
    public string ExceedanceCaption { get; set; }
    public double? ExceedanceValue { get; set; }
    public string ExceedanceColor { get; set; }
    public string DateFormat { get; set; }
    public string SubHeading { get; set; }
    public int? GraphDefinitionID { get; set; }
    public double? YAxisHighestValue { get; set; }
    public double? YAxisLowestValue { get; set; }
    public double? YAxisInterval { get; set; }
}

[Serializable]
public class GraphDefinition
{
    private int? _iGraphDefinitionID;
    private int? _iAccountID;
    private string _strDefinitionName;
    private string _strDefinition;
    private bool? _bIsSystem;
    private bool? _bIsHidden;
    private int? _iTableID;
    private int? _iColumnID;
    private string _strDefinitionKey;
    private bool? _bIsActive;
    private int? _iDataColumn1ID;
    private int? _iDataColumn2ID;
    private int? _iDataColumn3ID;
    private int? _iDataColumn4ID;
    private string _strTableName;
    private string _strColumnName;

    public GraphDefinition(int? p_iGraphDefinitionID, int? p_iAccountID,
        string p_strDefinitionName, string p_strDefinition,
        bool? p_bIsSystem, bool? p_bIsHidden,
        int? p_iTableID, int? p_iColumnID,
        string p_strDefinitionKey,
        bool? p_bIsActive,
        int? p_iDataColumn1ID, int? p_iDataColumn2ID, int? p_iDataColumn3ID, int? p_iDataColumn4ID)
    {
        _iGraphDefinitionID = p_iGraphDefinitionID;
        _iAccountID = p_iAccountID;
        _strDefinitionName = p_strDefinitionName;
        _strDefinition = p_strDefinition;
        _bIsSystem = p_bIsSystem;
        _bIsHidden = p_bIsHidden;
        _iTableID = p_iTableID;
        _iColumnID = p_iColumnID;
        _strDefinitionKey = p_strDefinitionKey;
        _bIsActive = p_bIsActive;
        _iDataColumn1ID = p_iDataColumn1ID;
        _iDataColumn2ID = p_iDataColumn2ID;
        _iDataColumn3ID = p_iDataColumn3ID;
        _iDataColumn4ID = p_iDataColumn4ID;
        _strTableName = string.Empty;
        _strColumnName = string.Empty;
    }

    public GraphDefinition(int? p_iGraphDefinitionID, int? p_iAccountID,
        string p_strDefinitionName, string p_strDefinition,
        bool? p_bIsSystem, bool? p_bIsHidden,
        int? p_iTableID, int? p_iColumnID,
        string p_strDefinitionKey,
        bool? p_bIsActive,
        int? p_iDataColumn1ID, int? p_iDataColumn2ID, int? p_iDataColumn3ID, int? p_iDataColumn4ID,
        string p_strTableName, string p_strColunnName)
    {
        _iGraphDefinitionID = p_iGraphDefinitionID;
        _iAccountID = p_iAccountID;
        _strDefinitionName = p_strDefinitionName;
        _strDefinition = p_strDefinition;
        _bIsSystem = p_bIsSystem;
        _bIsHidden = p_bIsHidden;
        _iTableID = p_iTableID;
        _iColumnID = p_iColumnID;
        _strDefinitionKey = p_strDefinitionKey;
        _bIsActive = p_bIsActive;
        _iDataColumn1ID = p_iDataColumn1ID;
        _iDataColumn2ID = p_iDataColumn2ID;
        _iDataColumn3ID = p_iDataColumn3ID;
        _iDataColumn4ID = p_iDataColumn4ID;
        _strTableName = p_strTableName;
        _strColumnName = p_strColunnName;
    }

    public int? GraphDefinitionID
    {
        get { return _iGraphDefinitionID; }
        set { _iGraphDefinitionID = value; }
    }

    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }

    public string DefinitionName
    {
        get { return _strDefinitionName; }
        set { _strDefinitionName = value; }
    }

    public string Definition
    {
        get { return _strDefinition; }
        set { _strDefinition = value; }
    }

    public bool? IsSystem
    {
        get { return _bIsSystem; }
        set { _bIsSystem = value; }
    }

    public bool? IsHidden
    {
        get { return _bIsHidden; }
        set { _bIsHidden = value; }
    }

    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }

    public int? ColumnID
    {
        get { return _iColumnID; }
        set { _iColumnID = value; }
    }

    public string DefinitionKey
    {
        get { return _strDefinitionKey; }
        set { _strDefinitionKey = value; }
    }

    public bool? IsActive
    {
        get { return _bIsActive; }
        set { _bIsActive = value; }
    }

    public int? DataColumn1ID
    {
        get { return _iDataColumn1ID; }
        set { _iDataColumn1ID = value; }
    }

    public int? DataColumn2ID
    {
        get { return _iDataColumn2ID; }
        set { _iDataColumn2ID = value; }
    }

    public int? DataColumn3ID
    {
        get { return _iDataColumn3ID; }
        set { _iDataColumn3ID = value; }
    }

    public int? DataColumn4ID
    {
        get { return _iDataColumn4ID; }
        set { _iDataColumn4ID = value; }
    }

    public string TableName
    {
        get { return _strTableName; }
    }

    public string ColumnName
    {
        get { return _strColumnName; }
    }
}