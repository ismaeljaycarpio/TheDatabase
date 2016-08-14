using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.IO;
using GenericParsing;
using System.Net.Mail;
using System.Xml;


/// <summary>
/// Summary description for UploadWorld
/// </summary>
public class UploadWorld
{
    public int TableID;
	public UploadWorld(int iTableID,int? iImportTemplateID)
	{
        TableID = iTableID;
	}


    #region Condition

    public static int dbg_Condition_Insert(Condition p_Condition)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_Condition_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@nColumnID", p_Condition.ColumnID));
                command.Parameters.Add(new SqlParameter("@sConditionType", p_Condition.ConditionType));
                command.Parameters.Add(new SqlParameter("@nCheckColumnID", p_Condition.CheckColumnID));
                command.Parameters.Add(new SqlParameter("@sCheckFormula", p_Condition.CheckFormula));

                if (p_Condition.CheckValue!="")
                    command.Parameters.Add(new SqlParameter("@sCheckValue", p_Condition.CheckValue));

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

    public static int dbg_Condition_Update(Condition p_Condition)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Condition_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nConditionID", p_Condition.ConditionID));


                command.Parameters.Add(new SqlParameter("@nColumnID", p_Condition.ColumnID));
                command.Parameters.Add(new SqlParameter("@sConditionType", p_Condition.ConditionType));
                command.Parameters.Add(new SqlParameter("@nCheckColumnID", p_Condition.CheckColumnID));
                command.Parameters.Add(new SqlParameter("@sCheckFormula", p_Condition.CheckFormula));
                if (p_Condition.CheckValue != "")
                    command.Parameters.Add(new SqlParameter("@sCheckValue", p_Condition.CheckValue));



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

    public static int dbg_Condition_Delete(int nConditionID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Condition_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nConditionID ", nConditionID));

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


    public static Condition dbg_Condition_Detail(int nConditionID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_Condition_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nConditionID", nConditionID));

                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Condition temp = new Condition(
                                (int)reader["ConditionID"], (int)reader["ColumnID"], (string)reader["ConditionType"],
                                (int)reader["CheckColumnID"],
                              (string)reader["CheckFormula"], (string)reader["CheckValue"]
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


    public static string Condition_GetFormula(int? ColumnID, int? CheckColumnID, string ConditionType, string CheckValue)
    {
        string strFormula = "";

        DataTable dtCondition= dbg_Condition_Select(ColumnID, CheckColumnID, ConditionType, CheckValue);
        if(dtCondition!=null && dtCondition.Rows.Count>0)
        {
            if(dtCondition.Rows[0]["CheckFormula"]!=DBNull.Value)
            {
                strFormula = dtCondition.Rows[0]["CheckFormula"].ToString();
            }
        }
        return strFormula;
    }

    public static string Condition_GetFormulaHTMLTable(Column theColumn,  string ConditionType, string CheckValue)
    {
        string strFormula = "";
        string strCheckColumnID = Common.GetValueFromSQL("SELECT TOP 1 CheckColumnID FROM [Condition] Con WHERE Con.ConditionType='" + ConditionType + "'AND Con.ColumnID=" + theColumn.ColumnID.ToString());
        Column theCheckColumn=null;
        if (strCheckColumnID != "")
        {
            theCheckColumn = RecordManager.ets_Column_Details(int.Parse(strCheckColumnID));           
        }

        if (theCheckColumn == null)
            return strFormula;

        DataTable dtCondition = dbg_Condition_Select(theColumn.ColumnID, theCheckColumn.ColumnID, ConditionType, CheckValue);
        if (dtCondition != null && dtCondition.Rows.Count > 0)
        {
            strFormula = @"<table  style=""border-color: #600;border-width: 0 0 1px 1px;border-style: solid;border-collapse:collapse;"">
                               <tr >
                                   <td style=""border-color: #600;border-width: 1px 1px 0 0; border-style: solid;margin: 0;"">
                                       <strong>When</strong>
                                   </td>
                                   <td style=""border-color: #600;border-width: 1px 1px 0 0; border-style: solid;margin: 0;"">
                                       <strong>Is</strong>
                                   </td>
                                   <td style=""border-color: #600;border-width: 1px 1px 0 0; border-style: solid;margin: 0;"">
                                       <strong></strong>
                                   </td>
                               </tr>";
            foreach(DataRow dr in dtCondition.Rows)
            {

                string strDisplayName = dr["DisplayName"].ToString();
                string strCheckValue = Common.GetDisplayTextFromColumnAndValue(theCheckColumn, dr["CheckValue"].ToString());

                string strCheckFormula = dr["CheckFormula"].ToString();
                strCheckFormula = Common.GetFromulaMsg("", theColumn.DisplayName, strCheckFormula);
                strCheckFormula = strCheckFormula.Replace("<br/>", " and ");
                try
                {
                    strDisplayName = System.Net.WebUtility.HtmlEncode(strDisplayName);
                    strCheckValue = System.Net.WebUtility.HtmlEncode(strCheckValue);
                    strCheckFormula = System.Net.WebUtility.HtmlEncode(strCheckFormula);
                }
                catch
                {
                    //
                }
                

                strFormula = strFormula + @"<tr >
                                    <td  style=""border-color: #600;border-width: 1px 1px 0 0; border-style: solid;margin: 0;"">" +  dr["DisplayName"].ToString() + @"</td>
                                    <td  style=""border-color: #600;border-width: 1px 1px 0 0; border-style: solid;margin: 0;"">" + strCheckValue + @"</td>
                                    <td  style=""border-color: #600;border-width: 1px 1px 0 0; border-style: solid;margin: 0;"">" + strCheckFormula + @"</td>
                                </tr>";
            }

            strFormula = strFormula + "</table>";
        }
        return strFormula;
    }

    public static string Condition_GetFormula_Full(int? ColumnID,  string ConditionType)
    {
        string strFormula = "";

        DataTable dtCondition = dbg_Condition_Select(ColumnID,null,  ConditionType, "");
        if (dtCondition != null && dtCondition.Rows.Count > 0)
        {
            foreach(DataRow dr in dtCondition.Rows)
            {
                strFormula =strFormula+ dr["CheckColumnID"].ToString() + dr["CheckFormula"].ToString() + (dr["CheckValue"]==DBNull.Value?"": dr["CheckValue"].ToString());
            }
            if (strFormula.Length > 0)
                strFormula = strFormula.ToUpper();
        }
        return strFormula;
    }

    public static DataTable dbg_Condition_Select(int? ColumnID, int? CheckColumnID, string ConditionType, string CheckValue)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Condition_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;



                 command.Parameters.Add(new SqlParameter("@ColumnID", ColumnID));

                 if (CheckColumnID != null)
                     command.Parameters.Add(new SqlParameter("@CheckColumnID", CheckColumnID));


                 if (ConditionType != "")
                     command.Parameters.Add(new SqlParameter("@ConditionType", ConditionType));

                 if (CheckValue != "")
                     command.Parameters.Add(new SqlParameter("@CheckValue", CheckValue));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                //System.Data.DataSet ds = new System.Data.DataSet();
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


    #endregion



}