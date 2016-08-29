using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Web.UI;
/// <summary>
/// Summary description for TheDatabase
/// </summary>
/// 


public static class EMD_Standardised_Field_Table
{
    public static string Sample_Type = "Sample Type";
    public static string Analyte_Name = "Analyte Name";
    public static string Decimals = "Decimals";
    public static string Ignore_Symbols = "Ignore Symbols";
    public static string Show_Graph = "Show Graph";

}


public static class TheDatabaseS
{
    public static string SystemNameFromColumnID(int iColumnID)
    {
        string strSystemName = "";
        strSystemName = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + iColumnID.ToString());
        return strSystemName;
    }

    public static void PopulateMenuDDL(ref DropDownList ddlShowUnder)
    {

        

        ddlShowUnder.Items.Clear();

        DataTable dtTopLevel = Common.DataTableFromText(@"SELECT MenuID,Menu FROM Menu WHERE IsActive=1 AND 
                AccountID=" + System.Web.HttpContext.Current.Session["AccountID"].ToString() +
                            @" AND ParentMenuID IS  NULL   AND(TableID IS NULL AND DocumentID IS NULL AND ExternalPageLink IS NULL AND Menu<>'---') ORDER BY DisplayOrder");

        foreach (DataRow drTop in dtTopLevel.Rows)
        {
            ListItem liItem = new ListItem(drTop["Menu"].ToString(), drTop["MenuID"].ToString());
            ddlShowUnder.Items.Add(liItem);

            PopulateSubMenu(ref ddlShowUnder, int.Parse(drTop["MenuID"].ToString()), 0);

        }
        //add top level in UI please
        //ListItem liTop = new ListItem("-- Top Level --", "");
        //ddlShowUnder.Items.Insert(0, liTop);
    }



    static void PopulateSubMenu(ref DropDownList ddlShowUnder, int iParentMenuID, int iLD)
    {
        DataTable dtSubMenu = Common.DataTableFromText(@"SELECT MenuID,Menu FROM Menu WHERE IsActive=1 AND 
                AccountID=" + System.Web.HttpContext.Current.Session["AccountID"].ToString() + @" AND ParentMenuID=" + iParentMenuID.ToString()
                            + @"  AND (TableID IS NULL AND DocumentID IS NULL AND ExternalPageLink IS NULL AND Menu<>'---')  ORDER BY DisplayOrder");

        iLD = iLD + 1;
        string strLD = "";
        for (int i = 1; i <= iLD; i++)
        {
            strLD = strLD + "-";
        }

        foreach (DataRow drSubMenu in dtSubMenu.Rows)
        {
            ListItem liItem = new ListItem(strLD + drSubMenu["Menu"].ToString(), drSubMenu["MenuID"].ToString());
            ddlShowUnder.Items.Add(liItem);
            PopulateSubMenu(ref ddlShowUnder, int.Parse(drSubMenu["MenuID"].ToString()), iLD);
        }
    }


    public static string spGetParentRecordID(int nChildRecordID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spGetParentRecordID", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nChildRecordID", nChildRecordID));

                
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


                if (ds.Tables[0].Rows[0][0] == DBNull.Value)
                {
                    return "";
                }
                else
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }



            }
        }
    }



    public static string spGetValueFromRelatedTable(int nParentRecordID, int nRequiredTableID, string sRequiredColumnName)
    {

        try
        {

            using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
            {
                using (SqlCommand command = new SqlCommand("spGetValueFromRelatedTable", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@nParentRecordID", nParentRecordID));
                    command.Parameters.Add(new SqlParameter("@nRequiredTableID", nRequiredTableID));
                    command.Parameters.Add(new SqlParameter("@sRequiredColumnName", sRequiredColumnName));

                  
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

                    if (ds.Tables.Count>0)
                    {
                        if (ds.Tables[0].Rows[0][0] == DBNull.Value)
                        {
                            return "";
                        }
                        else
                        {
                            return ds.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    

                    return "";

                }
            }
        }
        catch
        {
            return "";
        }
    }


    public static int spUpdateRelatedTable(int nParentRecordID, int nRequiredTableID, string sRequiredColumnName, string sValue)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spUpdateRelatedTable", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nParentRecordID", nParentRecordID));
                command.Parameters.Add(new SqlParameter("@nRequiredTableID", nRequiredTableID));
                command.Parameters.Add(new SqlParameter("@sRequiredColumnName", sRequiredColumnName));
                command.Parameters.Add(new SqlParameter("@sValue", sValue));

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


    public static int Column_ReplaceDisplayColumn(int TableID, string OldColumnName, string NewColumnName)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Column_ReplaceDisplayColumn", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@TableID", TableID));
                command.Parameters.Add(new SqlParameter("@OldColumnName", OldColumnName));
                command.Parameters.Add(new SqlParameter("@NewColumnName", NewColumnName));


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



    public static int Table_TableNameRename(int TableID, string OldName, string NewName)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Table_TableNameRename", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@TableID", TableID));
                command.Parameters.Add(new SqlParameter("@OldName", OldName));
                command.Parameters.Add(new SqlParameter("@NewName", NewName));


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



    public static DataTable spExportAllTables(int? nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_ExportRelatedData", connection)) //spExportAllTables
            {
                command.CommandType = CommandType.StoredProcedure;

                command.CommandTimeout = 0;

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@ParentTableTableID", nTableID));
                              

               
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


    // This presumes that weeks start with Monday.
    // Week 1 is the 1st week of the year with a Thursday in it.
    public static int GetIso8601WeekOfYear(DateTime time)
    {
        // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
        // be the same week# as whatever Thursday, Friday or Saturday are,
        // and we always get those right
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }

        // Return the week of our adjusted day
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    } 

    public static int spAuditRawToAudit(int ProcessRecordID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spAuditRawToAudit", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProcessRecordID ", ProcessRecordID));

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

    public static string EscapeStringValue(string s)
    {
        if (s == null || s.Length == 0)
        {
            return "";
        }

        char c = '\0';
        int i;
        int len = s.Length;
        System.Text.StringBuilder sb = new System.Text.StringBuilder(len + 4);
        String t;

        for (i = 0; i < len; i += 1)
        {
            c = s[i];
            switch (c)
            {
                case '\\':
                case '"':
                    sb.Append('\\');
                    sb.Append(c);
                    break;
                case '/':
                    sb.Append('\\');
                    sb.Append(c);
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                default:
                    if (c < ' ')
                    {
                        t = "000" + String.Format("X", c);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    break;
            }
        }
        return sb.ToString();
    }

    public static string GetCalculationFormula(int iTableID,string strFormula,DataTable dtSys)
    {
        strFormula = strFormula.ToLower();

        if (dtSys==null)
            dtSys=  Common.DataTableFromText("SELECT SystemName FROM [Column] WHERE TableID=" + iTableID.ToString());

          foreach (DataRow dr in dtSys.Rows)
          {
              if (strFormula.IndexOf("removenonnumericchar") > -1)
              {
                  strFormula = strFormula.Replace("dbo.removenonnumericchar([" + dr["SystemName"].ToString().ToLower() + "])", "ISNULL( NULLIF(dbo.RemoveNonNumericChar([" + dr["SystemName"].ToString().ToLower() + "]),''),0)");
              }
              else
              {

                  strFormula = strFormula.Replace("[" + dr["SystemName"].ToString().ToLower() + "]", "ISNULL([" + dr["SystemName"].ToString().ToLower() + "],0)");
              }
          }


        return strFormula;

    }




    public static double ToJulianDate(this DateTime date)
    {
        //return date.ToOADate() + 2415018.5;
        //return date.ToJulianDate() ;
        return date.ToOADate();
    }

    public static List<int> ListIDsByCOALESCE(int  iTableID)
    {

        try
        {
           

            using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Get_RecordIDS_BY_TableID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@nTableID", iTableID));

                  

                    List<int> lstIDs = new List<int>();

                    string strAllIDs = "";

                    connection.Open();

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                strAllIDs = (string)reader[0];
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        //
                    }
                   
                    connection.Close();
                    connection.Dispose();
                    var elements = strAllIDs.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                    // To Loop through
                    foreach (string items in elements)
                    {
                        lstIDs.Add(int.Parse(items));
                    }
                   

                    return lstIDs;

                }
            }

        }
        catch
        {
            return null;
        }
    }
    
    public static List<int> ListOfIDs(string strSQL)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
            {
                using (SqlCommand command = new SqlCommand(strSQL, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    List<int> lstIDs = new List<int>();

                    try
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstIDs.Add((int)reader[0]);
                            }
                        }

                    }
                    catch
                    {
                        lstIDs = null;
                    }

                    connection.Close();
                    connection.Dispose();

                    return lstIDs;

                }
            }
        }
        catch
        {
            return null;
        }

        
        
    }
//    public static string GetCalculationResult(DataTable _dtColumnsAll, string strCalculation, int? _iRecordID,Record theRecord, int? iParentRecordID,
//        TempRecord theTempRecord)
//    {
//        if (theRecord == null && _iRecordID == null && theTempRecord==null)
//            return "";


//        string strResult = "";
//        string strCalculationOrg = strCalculation;
//        try
//        {


//            if (_iRecordID != null)
//                theRecord = RecordManager.ets_Record_Detail_Full((int)_iRecordID);
//            //DataTable _dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theRecord.TableID, null, null);

//            for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
//            {
//                if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
//                {
//                    string strValue = "";

//                    if (theRecord!=null)
//                    {
//                        strValue = RecordManager.GetRecordValue(ref theRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
//                    }
//                    else
//                    {
//                        strValue = UploadManager.GetTempRecordValue(ref theTempRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
//                    }


//                    if (!string.IsNullOrEmpty(strValue))
//                    {

//                        if (strValue.IndexOf(" ") > -1)
//                        {
//                            strValue = strValue.Substring(0, strValue.IndexOf(" "));
//                            strValue = strValue.Trim();
                           
//                        }
//                        strValue = Common.IgnoreSymbols(strValue);
//                        strCalculation = strCalculation.ToUpper().Replace("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
//                    }
//                }
//            }

//            if (iParentRecordID == null && theTempRecord==null && strCalculation.IndexOf("[") > -1 && strCalculation.IndexOf(":") > -1)
//            {
//                string strParentTableName = strCalculation.Substring(strCalculation.IndexOf("[") + 1, strCalculation.IndexOf(":") - 1);

//                if (strParentTableName != "")
//                {
                    
//                    Table theTable = RecordManager.ets_Table_Details((int)theRecord.TableID);
//                    string strParentSys = Common.GetValueFromSQL(@" SELECT SystemName FROM [Column] WHERE TableID=" + theTable.TableID.ToString() + @" AND TableTableID=(SELECT Top 1 TableID FROM [Table] WHERE IsActive=1
//                                AND AccountID=" + theTable.AccountID.ToString() + @"  and LinkedParentColumnID IS NOT NULL 
//                                    AND (columntype='dropdown' or columntype='listbox')
//                                    AND TableName='" + strParentTableName.Replace("'", "''") + "')");


//                    if (strParentSys != "")
//                    {
//                        string strParentID = RecordManager.GetRecordValue(ref theRecord, strParentSys);
//                        if (strParentID != "")
//                        {
//                            int iTemp = 0;
//                            int.TryParse(strParentID, out iTemp);
//                            if (iTemp > 0)
//                            {
//                                iParentRecordID = iTemp;
//                            }

//                        }
//                    }
//                }

//            }

//            if (iParentRecordID != null)
//            {
//                //string strParentTableID = Common.GetValueFromSQL("SELECT TableID FROM [Record] WHERE RecordID=" + iParentRecordID.ToString());
//                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)iParentRecordID);

//                Table theParnetTable = RecordManager.ets_Table_Details((int)theParentRecord.TableID);
//                DataTable dtRecordTypleColumlnsP = RecordManager.ets_Table_Columns_All((int)theParnetTable.TableID);

//                for (int j = 0; j < dtRecordTypleColumlnsP.Rows.Count; j++)
//                {
//                    if (strCalculation.ToUpper().IndexOf("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
//                    {
//                        string strValue = RecordManager.GetRecordValue(ref theParentRecord, dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString());
//                        if (!string.IsNullOrEmpty(strValue))
//                        {
//                            //lets convert it to datetime

//                            if (strValue.IndexOf(" ") > -1)
//                            {
//                                strValue = strValue.Substring(0, strValue.IndexOf(" "));
//                                strValue = strValue.Trim();
                               
//                            }
//                            strValue = Common.IgnoreSymbols(strValue);
//                            strCalculation = strCalculation.ToUpper().Replace("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
//                        }
//                    }

//                }
//            }

//            strCalculation = strCalculation.Replace("[", "");
//            strCalculation = strCalculation.Replace("]", "");


//            if (strCalculation.ToLower().IndexOf("RemoveNonNumericChar(v".ToLower()) > -1
//                || strCalculation.ToLower() == strCalculationOrg.ToLower())
//            {

//                return "";
//            }
//            else
//            {
//                strResult = Common.GetValueFromSQL("SELECT " + strCalculation);
//            }

//        }
//        catch
//        {
//            //
//        }

//        return strResult;
//    }

    public static string GetCalculationResult(ref DataTable _dtColumnsAll, string strCalculation, int? _iRecordID, Record theRecord, int? iParentRecordID,
      TempRecord theTempRecord, Table theTable,Column theColumn)
    {
        if (theRecord == null && _iRecordID == null && theTempRecord == null)
            return "";


        string strResult = "";
        string strCalculationOrg = strCalculation;
        try
        {


            if (_iRecordID != null)
                theRecord = RecordManager.ets_Record_Detail_Full((int)_iRecordID);
            //DataTable _dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theRecord.TableID, null, null);

            for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
            {
                if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
                {
                    string strValue = "";

                    if (theRecord != null)
                    {
                        strValue = RecordManager.GetRecordValue(ref theRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
                    }
                    else
                    {
                        strValue = UploadManager.GetTempRecordValue(ref theTempRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
                    }


                    if (string.IsNullOrEmpty(strValue))
                    {
                        strValue = "0";
                    }

                    if (strValue.IndexOf(" ") > -1)
                    {
                        strValue = strValue.Substring(0, strValue.IndexOf(" "));
                        strValue = strValue.Trim();

                    }
                    strValue = Common.IgnoreSymbols(strValue);
                    strCalculation = strCalculation.ToUpper().Replace("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
                    
                }
            }

            if (iParentRecordID == null && theTempRecord == null && strCalculation.IndexOf("[") > -1 && strCalculation.IndexOf(":") > -1)
            {
                string strParentTableName = strCalculation.Substring(strCalculation.IndexOf("[") + 1, strCalculation.IndexOf(":") - 1);

                if (strParentTableName != "")
                {

                    if (theTable==null)
                        theTable = RecordManager.ets_Table_Details((int)theRecord.TableID);

                    string strParentSys = Common.GetValueFromSQL(@" SELECT SystemName FROM [Column] WHERE TableID=" + theTable.TableID.ToString() + @" AND TableTableID=(SELECT Top 1 TableID FROM [Table] WHERE IsActive=1
                                AND AccountID=" + theTable.AccountID.ToString() + @"  and LinkedParentColumnID IS NOT NULL 
                                    AND (columntype='dropdown' or columntype='listbox')
                                    AND TableName='" + strParentTableName.Replace("'", "''") + "')");


                    if (strParentSys != "")
                    {
                        string strParentID = RecordManager.GetRecordValue(ref theRecord, strParentSys);
                        if (strParentID != "")
                        {
                            int iTemp = 0;
                            int.TryParse(strParentID, out iTemp);
                            if (iTemp > 0)
                            {
                                iParentRecordID = iTemp;
                            }

                        }
                    }
                }

            }

            if (iParentRecordID != null)
            {
                //string strParentTableID = Common.GetValueFromSQL("SELECT TableID FROM [Record] WHERE RecordID=" + iParentRecordID.ToString());
                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)iParentRecordID);

                Table theParnetTable = RecordManager.ets_Table_Details((int)theParentRecord.TableID);
                DataTable dtRecordTypleColumlnsP = RecordManager.ets_Table_Columns_All((int)theParnetTable.TableID);

                for (int j = 0; j < dtRecordTypleColumlnsP.Rows.Count; j++)
                {
                    if (strCalculation.ToUpper().IndexOf("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
                    {
                        string strValue = RecordManager.GetRecordValue(ref theParentRecord, dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString());
                        if (string.IsNullOrEmpty(strValue))
                        {
                            strValue = "0";
                        }
                        //lets convert it to datetime

                        if (strValue.IndexOf(" ") > -1)
                        {
                            strValue = strValue.Substring(0, strValue.IndexOf(" "));
                            strValue = strValue.Trim();

                        }
                        strValue = Common.IgnoreSymbols(strValue);
                        strCalculation = strCalculation.ToUpper().Replace("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
                        
                    }

                }
            }

            strCalculation = strCalculation.Replace("[", "");
            strCalculation = strCalculation.Replace("]", "");


            if (strCalculation.ToLower().IndexOf("RemoveNonNumericChar(v".ToLower()) > -1
                || strCalculation.ToLower() == strCalculationOrg.ToLower())
            {

                return "";
            }
            else  if (strCalculation.ToLower().IndexOf("v".ToLower()) > -1)
            {
                return "";
            }
            else
            {
                //strResult = Common.GetValueFromSQL("SELECT " + strCalculation);
                strResult = Common.EvaluateCalculationFormula(strCalculation);

                if (theColumn.RoundNumber!=null)
                {
                    strResult = Math.Round(double.Parse(Common.IgnoreSymbols(strResult)), (int)theColumn.RoundNumber).ToString("N" + theColumn.RoundNumber.ToString());
                }
            }

        }
        catch
        {
            //
        }

        return strResult;
    }



    public static string GetTextCalculationResult(ref DataTable _dtColumnsAll, string strCalculation, int? _iRecordID, Record theRecord, int? iParentRecordID,
     TempRecord theTempRecord, Table theTable, Column theColumn)
    {
        if (theRecord == null && _iRecordID == null && theTempRecord == null)
            return "";


        string strResult = "";
        string strCalculationOrg = strCalculation;
        try
        {


            if (_iRecordID != null)
                theRecord = RecordManager.ets_Record_Detail_Full((int)_iRecordID);
            //DataTable _dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theRecord.TableID, null, null);

            for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
            {
                if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
                {
                    string strValue = "";

                    if (theRecord != null)
                    {
                        strValue = RecordManager.GetRecordValue(ref theRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
                    }
                    else
                    {
                        strValue = UploadManager.GetTempRecordValue(ref theTempRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
                    }


                    //if (string.IsNullOrEmpty(strValue))
                    //{
                    //    strValue = "0";
                    //}

                    //if (strValue.IndexOf(" ") > -1)
                    //{
                    //    strValue = strValue.Substring(0, strValue.IndexOf(" "));
                    //    strValue = strValue.Trim();

                    //}
                    //strValue = Common.IgnoreSymbols(strValue);
                    strCalculation = strCalculation.Replace("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);

                }
            }

            if (iParentRecordID == null && theTempRecord == null && strCalculation.IndexOf("[") > -1 && strCalculation.IndexOf(":") > -1)
            {
                string strParentTableName = strCalculation.Substring(strCalculation.IndexOf("[") + 1, strCalculation.IndexOf(":") - 1);

                if (strParentTableName != "")
                {

                    if (theTable == null)
                        theTable = RecordManager.ets_Table_Details((int)theRecord.TableID);

                    string strParentSys = Common.GetValueFromSQL(@" SELECT SystemName FROM [Column] WHERE TableID=" + theTable.TableID.ToString() + @" AND TableTableID=(SELECT Top 1 TableID FROM [Table] WHERE IsActive=1
                                AND AccountID=" + theTable.AccountID.ToString() + @"  and LinkedParentColumnID IS NOT NULL 
                                    AND (columntype='dropdown' or columntype='listbox')
                                    AND TableName='" + strParentTableName.Replace("'", "''") + "')");


                    if (strParentSys != "")
                    {
                        string strParentID = RecordManager.GetRecordValue(ref theRecord, strParentSys);
                        if (strParentID != "")
                        {
                            int iTemp = 0;
                            int.TryParse(strParentID, out iTemp);
                            if (iTemp > 0)
                            {
                                iParentRecordID = iTemp;
                            }

                        }
                    }
                }

            }

            if (iParentRecordID != null)
            {
                //string strParentTableID = Common.GetValueFromSQL("SELECT TableID FROM [Record] WHERE RecordID=" + iParentRecordID.ToString());
                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)iParentRecordID);

                Table theParnetTable = RecordManager.ets_Table_Details((int)theParentRecord.TableID);
                DataTable dtRecordTypleColumlnsP = RecordManager.ets_Table_Columns_All((int)theParnetTable.TableID);

                for (int j = 0; j < dtRecordTypleColumlnsP.Rows.Count; j++)
                {
                    if (strCalculation.ToUpper().IndexOf("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
                    {
                        string strValue = RecordManager.GetRecordValue(ref theParentRecord, dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString());
                        //if (string.IsNullOrEmpty(strValue))
                        //{
                        //    strValue = "0";
                        //}
                        //lets convert it to datetime

                        //if (strValue.IndexOf(" ") > -1)
                        //{
                        //    strValue = strValue.Substring(0, strValue.IndexOf(" "));
                        //    strValue = strValue.Trim();

                        //}
                        //strValue = Common.IgnoreSymbols(strValue);
                        strCalculation = strCalculation.Replace("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);

                    }

                }
            }

            strCalculation = strCalculation.Replace("[", "");
            strCalculation = strCalculation.Replace("]", "");
            strResult = strCalculation;
            return strResult;

            //if (strCalculation.ToLower().IndexOf("RemoveNonNumericChar(v".ToLower()) > -1
            //    || strCalculation.ToLower() == strCalculationOrg.ToLower())
            //{

            //    return "";
            //}
            //else if (strCalculation.ToLower().IndexOf("v".ToLower()) > -1)
            //{
            //    return "";
            //}
            //else
            //{
            //    //strResult = Common.GetValueFromSQL("SELECT " + strCalculation);
            //    strResult = Common.EvaluateCalculationFormula(strCalculation);

            //    if (theColumn.RoundNumber != null)
            //    {
            //        strResult = Math.Round(double.Parse(Common.IgnoreSymbols(strResult)), (int)theColumn.RoundNumber).ToString("N" + theColumn.RoundNumber.ToString());
            //    }
            //}

        }
        catch
        {
            //
        }

        return strResult;
    }



//    public static string GetDateCalculationResult(DataTable _dtColumnsAll, string strCalculation, int? _iRecordID,Record theRecord, int? iParentRecordID, string strDateCalculationType,
//        TempRecord theTempRecord)
//    {
//        if (theRecord == null && _iRecordID == null && theTempRecord==null)
//            return "";


//        string strResult = "";
//        bool bCheckIgnoreMidnight = false;
      
//        try
//        {

//            if (_iRecordID !=null)
//                theRecord=RecordManager.ets_Record_Detail_Full((int)_iRecordID);

//            Table theTable=RecordManager.ets_Table_Details(theRecord!=null?(int)theRecord.TableID:(int)theTempRecord.TableID);


//            string strIgnoreMidnight = SystemData.SystemOption_ValueByKey_Account("Time Calculation Ignore Midnight", (int)theTable.AccountID, theTable.TableID);

//            if(strIgnoreMidnight!="" && strIgnoreMidnight.ToString().ToLower()=="yes")
//            {
//                bCheckIgnoreMidnight = true;
//            }

            
//            //DataTable _dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theRecord.TableID, null, null);

//            string strResultFormat = "day";
//            if (strDateCalculationType != "")
//                strResultFormat = strDateCalculationType;
//            //if (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
//            //    strResultFormat = _dtRecordTypleColumlns.Rows[i]["DateCalculationType"].ToString().ToLower();

//            //check if it has "time"

//            bool bHasTime = false;

//            for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
//            {
//                if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
//                {
//                    if (_dtColumnsAll.Rows[j]["ColumnType"].ToString() == "time")
//                    {
//                        bHasTime = true;
//                    }
//                }
//            }


//            for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
//            {
//                if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
//                {
//                    string strValue = "";

//                    if(theRecord!=null)
//                    {
//                        strValue = RecordManager.GetRecordValue(ref theRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
//                    }
//                    else
//                    {
//                        strValue = UploadManager.GetTempRecordValue(ref theTempRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
//                    }
//                    if (!string.IsNullOrEmpty(strValue))
//                    {
//                        //lets convert it to datetime

//                        if (_dtColumnsAll.Rows[j]["ColumnType"].ToString() == "number")
//                        {
//                            strCalculation = strCalculation.ToUpper().Replace("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
//                            strCalculation = strCalculation.ToUpper().Replace("CAST(DBO.REMOVENONNUMERICCHAR(", ""); 
//                            strCalculation = strCalculation.ToUpper().Replace(") AS DECIMAL(20,10))", "");
//                            continue;
//                        }

//                        if (bHasTime)
//                        {
//                            if (strValue.IndexOf(" ") > -1)
//                            {
//                                strValue = strValue.Substring(strValue.IndexOf(" "));
//                                strValue = strValue.Trim();
//                            }
//                        }

//                        DateTime dtTempDateTime = DateTime.Today;
//                        try
//                        {
//                            dtTempDateTime = DateTime.Parse(strValue);

//                            if (bCheckIgnoreMidnight && dtTempDateTime.TimeOfDay.Ticks == 0)
//                            {
//                                if (_dtColumnsAll.Rows[j]["ColumnType"].ToString() != "date")
//                                    return "";
//                            }

//                        }
//                        catch
//                        {
//                            //
//                        }
//                        strValue = TheDatabaseS.ToJulianDate(dtTempDateTime).ToString();
//                        strCalculation = strCalculation.ToUpper().Replace("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
//                    }
//                }
//            }

//            if (iParentRecordID == null && strCalculation.IndexOf("[") > -1 && strCalculation.IndexOf(":")>-1)
//            {
//                string strParentTableName = strCalculation.Substring(strCalculation.IndexOf("[") + 1, strCalculation.IndexOf(":") - 1);

//                if(strParentTableName!="")
//                {
//                    string strParentSys = Common.GetValueFromSQL(@" SELECT SystemName FROM [Column] WHERE TableID="+theTable.TableID.ToString()+@" AND TableTableID=(SELECT Top 1 TableID FROM [Table] WHERE IsActive=1
//                                AND AccountID=" + theTable.AccountID.ToString() + @"  and LinkedParentColumnID IS NOT NULL 
//                                    AND (columntype='dropdown' or columntype='listbox')
//                                    AND TableName='" + strParentTableName.Replace("'", "''") + "')");


//                    if(strParentSys!="")
//                    {
//                        string strParentID = RecordManager.GetRecordValue(ref theRecord, strParentSys);
//                        if (!string.IsNullOrEmpty(strParentID))
//                        {
//                            int iTemp = 0;
//                            int.TryParse(strParentID, out iTemp);
//                            if(iTemp>0)
//                            {
//                                iParentRecordID = iTemp;
//                            }

//                        }
//                    }
//                }

//            }

//            if (iParentRecordID != null)
//            {
//                //string strParentTableID = Common.GetValueFromSQL("SELECT TableID FROM [Record] WHERE RecordID=" + iParentRecordID.ToString());
//                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)iParentRecordID);
//                Table theParnetTable = RecordManager.ets_Table_Details((int)theParentRecord.TableID);
//                DataTable dtRecordTypleColumlnsP = RecordManager.ets_Table_Columns_All((int)theParentRecord.TableID);

//                for (int j = 0; j < dtRecordTypleColumlnsP.Rows.Count; j++)
//                {
//                    if (strCalculation.ToUpper().IndexOf("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
//                    {
//                        //string strValue = Common.GetValueFromSQL("SELECT " + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString() + " FROM Record WHERE RecordID=" + iParentRecordID.ToString());
//                        string strValue = RecordManager.GetRecordValue(ref theParentRecord, dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString());

//                        if (!string.IsNullOrEmpty(strValue))
//                        {
//                            //lets convert it to datetime
//                            DateTime dtTempDateTime = DateTime.Today;
//                            try
//                            {
//                                dtTempDateTime = DateTime.Parse(strValue);

//                                if (bCheckIgnoreMidnight && dtTempDateTime.TimeOfDay.Ticks == 0)
//                                {
//                                    if (dtRecordTypleColumlnsP.Rows[j]["ColumnType"].ToString() != "date")
//                                        return "";
//                                }
//                            }
//                            catch
//                            {

//                            }
//                            strValue = TheDatabaseS.ToJulianDate(dtTempDateTime).ToString();
//                            strCalculation = strCalculation.ToUpper().Replace("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
//                        }
//                    }

//                }
//            }



//            if (strCalculation.IndexOf("[") > -1)
//            {
//                //we have number here
//                string regex = Common.NumberRegExDC; //@"\[(.*?)\]";  // Common.NumberRegExDC; //@"^.*?\([^\d]*(\d+)[^\d]*\).*$"
//                string text = strCalculation;
//                double num;
//                foreach (Match match in Regex.Matches(text, regex))
//                {
//                    string strEachNumber = match.Groups[1].Value;
//                    string strEachJulianNumber = strEachNumber;


//                    if (double.TryParse(strEachJulianNumber, out num))
//                    {
//                        // It's a number!
//                    }
//                    else
//                    {
//                        continue;
//                    }

//                    switch (strResultFormat)
//                    {
//                        case "datetime":
//                            strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute

//                            break;
//                        case "date":
//                            strEachJulianNumber = strEachNumber; //day
//                            break;
//                        case "time":
//                            strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
//                            break;
//                        case "minute":
//                            strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
//                            break;
//                        case "hour":
//                            strEachJulianNumber = (double.Parse(strEachNumber) / (24)).ToString(); //hour
//                            break;
//                        case "day":
//                            strEachJulianNumber = strEachNumber; //day
//                            break;

//                        case "ymd":
//                            strEachJulianNumber = strEachNumber; //day



//                            break;

//                        case "ym":
//                            strEachJulianNumber = strEachNumber; //day
//                            break;

//                        default:
//                            strEachJulianNumber = strEachNumber; //day
//                            break;
//                    }

//                    strCalculation = strCalculation.Replace("[" + strEachNumber + "]", strEachJulianNumber);
//                }




//            }


//            if (strCalculation.ToLower().IndexOf("[v") > -1)
//            {
//                return "";
//            }

//            strCalculation = strCalculation.Replace("[", "");
//            strCalculation = strCalculation.Replace("]", "");

//            string strJulianValue = Common.GetValueFromSQL("SELECT " + strCalculation);


//            //implement Result Format

//            if (strResultFormat != "")
//            {

//                strResult = strJulianValue;
//                switch (strResultFormat)
//                {
//                    case "datetime":

//                        DateTime dtResult = DateTime.FromOADate(double.Parse(strResult));
//                        //if (dtResult > DateTime.Today.AddYears(1000))
//                        //    dtResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

//                        strResult = dtResult.ToString();
//                        break;
//                    case "date":
//                        DateTime dResult = DateTime.FromOADate(double.Parse(strResult));
//                        //if (dResult > DateTime.Today.AddYears(1000))
//                        //    dResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

//                        strResult = dResult.ToShortDateString();
//                        break;
//                    case "time":
//                        string strOriginal = strResult;
//                        DateTime tResult = DateTime.FromOADate(double.Parse(strResult));
//                        //if (tResult > DateTime.Today.AddYears(1000))
//                        //    tResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

//                        string strDay = "";
//                        if (strResult.IndexOf(".") > -1)
//                        {
//                            strDay = strResult.Substring(0, strResult.IndexOf("."));
//                        }
//                        else
//                        {
//                            strDay = strResult;
//                        }
//                        if (strDay == "0")
//                        {
//                            strDay = "";
//                        }
//                        else if (strDay == "-0")
//                        {
//                            strDay = "-";
//                        }
//                        else if (strDay == "1")
//                        {
//                            strDay = "1 day ";
//                        }
//                        else
//                        {

//                            strDay = strDay + " days ";
//                        }
                        

//                        string strTimePart = tResult.ToLongTimeString();

//                        strResult = strDay + strTimePart;
//                        break;
//                    case "minute":
//                        double dMinutes = double.Parse(strJulianValue) * 24 * 60;
//                        strResult = (Math.Round(dMinutes,2)).ToString("N6");
//                        break;
//                    case "hour":
//                        double dHours = double.Parse(strJulianValue) * 24;
//                        strResult = (Math.Round(dHours,2)).ToString("N6");
//                        break;
//                    case "day":
//                        double dDays = double.Parse(strJulianValue);
//                        strResult = (Math.Round(dDays,2)).ToString("N6");
//                        break;
//                    case "ymd":

//                        double totalDays = double.Parse(strJulianValue);
//                        string totalYears = Math.Truncate(totalDays / 365).ToString();
//                        string totalMonths = Math.Truncate((totalDays % 365) / 30).ToString();
//                        string remainingDays = Math.Truncate((totalDays % 365) % 30).ToString();
//                        strResult = totalYears + " years " + totalMonths + " months " + remainingDays + " days";
//                        break;

//                    case "ym":

//                        double totalDays2 = double.Parse(strJulianValue);
//                        string totalYears2 = Math.Truncate(totalDays2 / 365).ToString();
//                        string totalMonths2 = Math.Truncate((totalDays2 % 365) / 30).ToString();

//                        strResult = totalYears2 + " years " + totalMonths2 + " months";
//                        break;

//                    default:
//                        break;
//                }

//            }
//        }
//        catch
//        {
//            //
//        }


//        return strResult;

//    }


    public static string GetDateCalculationResult(ref DataTable _dtColumnsAll, string strCalculation, int? _iRecordID, Record theRecord, int? iParentRecordID, string strDateCalculationType,
      TempRecord theTempRecord, Table theTable, bool? bCheckIgnoreMidnight)
    {
        if (theRecord == null && _iRecordID == null && theTempRecord == null)
            return "";


        string strResult = "";
       // bool bCheckIgnoreMidnight = false;

        try
        {

            if (_iRecordID != null)
                theRecord = RecordManager.ets_Record_Detail_Full((int)_iRecordID);

            if (theTable==null)
             theTable = RecordManager.ets_Table_Details(theRecord != null ? (int)theRecord.TableID : (int)theTempRecord.TableID);


            if(bCheckIgnoreMidnight==null)
            {
                bCheckIgnoreMidnight = false;
                string strIgnoreMidnight = SystemData.SystemOption_ValueByKey_Account("Time Calculation Ignore Midnight", (int)theTable.AccountID, theTable.TableID);

                if (strIgnoreMidnight != "" && strIgnoreMidnight.ToString().ToLower() == "yes")
                {
                    bCheckIgnoreMidnight = true;
                }
            }
            


            //DataTable _dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theRecord.TableID, null, null);

            string strResultFormat = "day";
            if (strDateCalculationType != "")
                strResultFormat = strDateCalculationType;
            //if (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
            //    strResultFormat = _dtRecordTypleColumlns.Rows[i]["DateCalculationType"].ToString().ToLower();

            //check if it has "time"

            bool bHasTime = false;

            for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
            {
                if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
                {
                    if (_dtColumnsAll.Rows[j]["ColumnType"].ToString() == "time")
                    {
                        bHasTime = true;
                    }
                }
            }


            for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
            {
                if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
                {
                    string strValue = "";

                    if (theRecord != null)
                    {
                        strValue = RecordManager.GetRecordValue(ref theRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
                    }
                    else
                    {
                        strValue = UploadManager.GetTempRecordValue(ref theTempRecord, _dtColumnsAll.Rows[j]["SystemName"].ToString());
                    }
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        //lets convert it to datetime

                        if (_dtColumnsAll.Rows[j]["ColumnType"].ToString() == "number")
                        {
                            strCalculation = strCalculation.ToUpper().Replace("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
                            strCalculation = strCalculation.ToUpper().Replace("CAST(DBO.REMOVENONNUMERICCHAR(", "");
                            strCalculation = strCalculation.ToUpper().Replace(") AS DECIMAL(20,10))", "");
                            continue;
                        }

                        if (bHasTime)
                        {
                            if (strValue.IndexOf(" ") > -1)
                            {
                                strValue = strValue.Substring(strValue.IndexOf(" "));
                                strValue = strValue.Trim();
                            }
                        }

                        DateTime dtTempDateTime = DateTime.Today;
                        try
                        {
                            dtTempDateTime = DateTime.Parse(strValue);

                            if ((bool)bCheckIgnoreMidnight && dtTempDateTime.TimeOfDay.Ticks == 0)
                            {
                                if (_dtColumnsAll.Rows[j]["ColumnType"].ToString() != "date")
                                    return "";
                            }

                        }
                        catch
                        {
                            //
                        }
                        strValue = TheDatabaseS.ToJulianDate(dtTempDateTime).ToString();
                        strCalculation = strCalculation.ToUpper().Replace("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
                    }
                }
            }

            if (iParentRecordID == null && strCalculation.IndexOf("[") > -1 && strCalculation.IndexOf(":") > -1)
            {
                string strParentTableName = strCalculation.Substring(strCalculation.IndexOf("[") + 1, strCalculation.IndexOf(":") - 1);

                if (strParentTableName != "")
                {
                    string strParentSys = Common.GetValueFromSQL(@" SELECT SystemName FROM [Column] WHERE TableID=" + theTable.TableID.ToString() + @" AND TableTableID=(SELECT Top 1 TableID FROM [Table] WHERE IsActive=1
                                AND AccountID=" + theTable.AccountID.ToString() + @"  and LinkedParentColumnID IS NOT NULL 
                                    AND (columntype='dropdown' or columntype='listbox')
                                    AND TableName='" + strParentTableName.Replace("'", "''") + "')");


                    if (strParentSys != "")
                    {
                        string strParentID = RecordManager.GetRecordValue(ref theRecord, strParentSys);
                        if (!string.IsNullOrEmpty(strParentID))
                        {
                            int iTemp = 0;
                            int.TryParse(strParentID, out iTemp);
                            if (iTemp > 0)
                            {
                                iParentRecordID = iTemp;
                            }

                        }
                    }
                }

            }

            if (iParentRecordID != null)
            {
                //string strParentTableID = Common.GetValueFromSQL("SELECT TableID FROM [Record] WHERE RecordID=" + iParentRecordID.ToString());
                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)iParentRecordID);
                Table theParnetTable = RecordManager.ets_Table_Details((int)theParentRecord.TableID);
                DataTable dtRecordTypleColumlnsP = RecordManager.ets_Table_Columns_All((int)theParentRecord.TableID);

                for (int j = 0; j < dtRecordTypleColumlnsP.Rows.Count; j++)
                {
                    if (strCalculation.ToUpper().IndexOf("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
                    {
                        //string strValue = Common.GetValueFromSQL("SELECT " + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString() + " FROM Record WHERE RecordID=" + iParentRecordID.ToString());
                        string strValue = RecordManager.GetRecordValue(ref theParentRecord, dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString());

                        if (!string.IsNullOrEmpty(strValue))
                        {
                            //lets convert it to datetime
                            DateTime dtTempDateTime = DateTime.Today;
                            try
                            {
                                dtTempDateTime = DateTime.Parse(strValue);

                                if ((bool)bCheckIgnoreMidnight && dtTempDateTime.TimeOfDay.Ticks == 0)
                                {
                                    if (dtRecordTypleColumlnsP.Rows[j]["ColumnType"].ToString() != "date")
                                        return "";
                                }
                            }
                            catch
                            {

                            }
                            strValue = TheDatabaseS.ToJulianDate(dtTempDateTime).ToString();
                            strCalculation = strCalculation.ToUpper().Replace("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
                        }
                    }

                }
            }



            if (strCalculation.IndexOf("[") > -1)
            {
                //we have number here
                string regex = Common.NumberRegExDC; //@"\[(.*?)\]";  // Common.NumberRegExDC; //@"^.*?\([^\d]*(\d+)[^\d]*\).*$"
                string text = strCalculation;
                double num;
                foreach (Match match in Regex.Matches(text, regex))
                {
                    string strEachNumber = match.Groups[1].Value;
                    string strEachJulianNumber = strEachNumber;


                    if (double.TryParse(strEachJulianNumber, out num))
                    {
                        // It's a number!
                    }
                    else
                    {
                        continue;
                    }

                    switch (strResultFormat)
                    {
                        case "datetime":
                            strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute

                            break;
                        case "date":
                            strEachJulianNumber = strEachNumber; //day
                            break;
                        case "time":
                            strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
                            break;
                        case "minute":
                            strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
                            break;
                        case "hour":
                            strEachJulianNumber = (double.Parse(strEachNumber) / (24)).ToString(); //hour
                            break;
                        case "day":
                            strEachJulianNumber = strEachNumber; //day
                            break;

                        case "ymd":
                            strEachJulianNumber = strEachNumber; //day



                            break;

                        case "ym":
                            strEachJulianNumber = strEachNumber; //day
                            break;

                        default:
                            strEachJulianNumber = strEachNumber; //day
                            break;
                    }

                    strCalculation = strCalculation.Replace("[" + strEachNumber + "]", strEachJulianNumber);
                }




            }


            if (strCalculation.ToLower().IndexOf("[v") > -1)
            {
                return "";
            }

            strCalculation = strCalculation.Replace("[", "");
            strCalculation = strCalculation.Replace("]", "");

            //string strJulianValue = Common.GetValueFromSQL("SELECT " + strCalculation);
            string strJulianValue = Common.EvaluateCalculationFormula(strCalculation);

            //implement Result Format

            if (strResultFormat != "")
            {

                strResult = strJulianValue;
                switch (strResultFormat)
                {
                    case "datetime":

                        DateTime dtResult = DateTime.FromOADate(double.Parse(strResult));
                        //if (dtResult > DateTime.Today.AddYears(1000))
                        //    dtResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

                        strResult = dtResult.ToString();
                        break;
                    case "date":
                        DateTime dResult = DateTime.FromOADate(double.Parse(strResult));
                        //if (dResult > DateTime.Today.AddYears(1000))
                        //    dResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

                        strResult = dResult.ToShortDateString();
                        break;
                    case "time":
                        string strOriginal = strResult;
                        DateTime tResult = DateTime.FromOADate(double.Parse(strResult));
                        //if (tResult > DateTime.Today.AddYears(1000))
                        //    tResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

                        string strDay = "";
                        if (strResult.IndexOf(".") > -1)
                        {
                            strDay = strResult.Substring(0, strResult.IndexOf("."));
                        }
                        else
                        {
                            strDay = strResult;
                        }
                        if (strDay == "0")
                        {
                            strDay = "";
                        }
                        else if (strDay == "-0")
                        {
                            strDay = "-";
                        }
                        else if (strDay == "1")
                        {
                            strDay = "1 day ";
                        }
                        else
                        {

                            strDay = strDay + " days ";
                        }


                        string strTimePart = tResult.ToLongTimeString();

                        strResult = strDay + strTimePart;
                        break;
                    case "minute":
                        double dMinutes = double.Parse(strJulianValue) * 24 * 60;
                        strResult = (Math.Round(dMinutes, 2)).ToString("N6");
                        break;
                    case "hour":
                        double dHours = double.Parse(strJulianValue) * 24;
                        strResult = (Math.Round(dHours, 2)).ToString("N6");
                        break;
                    case "day":
                        double dDays = double.Parse(strJulianValue);
                        strResult = (Math.Round(dDays, 2)).ToString("N6");
                        break;
                    case "ymd":

                        double totalDays = double.Parse(strJulianValue);
                        string totalYears = Math.Truncate(totalDays / 365).ToString();
                        string totalMonths = Math.Truncate((totalDays % 365) / 30).ToString();
                        string remainingDays = Math.Truncate((totalDays % 365) % 30).ToString();
                        strResult = totalYears + " years " + totalMonths + " months " + remainingDays + " days";
                        break;

                    case "ym":

                        double totalDays2 = double.Parse(strJulianValue);
                        string totalYears2 = Math.Truncate(totalDays2 / 365).ToString();
                        string totalMonths2 = Math.Truncate((totalDays2 % 365) / 30).ToString();

                        strResult = totalYears2 + " years " + totalMonths2 + " months";
                        break;

                    default:
                        break;
                }

            }
        }
        catch
        {
            //
        }


        return strResult;

    }
    //public static string GetDateCalculationResult(DataTable _dtColumnsAll, string strCalculation, int _iRecordID, int? iParentRecordID, string strDateCalculationType)
    //{
    //    string strResult = "";

    //    try
    //    {
    //        //Record theRecord=RecordManager.ets_Record_Detail_Full(_iRecordID);
    //        //DataTable _dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theRecord.TableID, null, null);
           
    //        string strResultFormat = "day";
    //        if (strDateCalculationType != "")
    //            strResultFormat = strDateCalculationType;
    //        //if (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
    //        //    strResultFormat = _dtRecordTypleColumlns.Rows[i]["DateCalculationType"].ToString().ToLower();

    //        //check if it has "time"

    //        bool bHasTime = false;

    //        for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
    //        {
    //            if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
    //            {
    //                if(_dtColumnsAll.Rows[j]["ColumnType"].ToString()=="time")
    //                {
    //                    bHasTime=true;
    //                }
    //            }
    //        }


    //        for (int j = 0; j < _dtColumnsAll.Rows.Count; j++)
    //        {
    //            if (strCalculation.ToUpper().IndexOf("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
    //            {
    //                string strValue = Common.GetValueFromSQL("SELECT " + _dtColumnsAll.Rows[j]["SystemName"].ToString() + " FROM Record WHERE RecordID=" + _iRecordID.ToString());
    //                if (strValue != "")
    //                {
    //                    //lets convert it to datetime

    //                    if(bHasTime)
    //                    {
    //                        if(strValue.IndexOf(" ")>-1)
    //                        {
    //                            strValue = strValue.Substring(strValue.IndexOf(" "));
    //                            strValue = strValue.Trim();
    //                        }
    //                    }


    //                    DateTime dtTempDateTime = DateTime.Today;
    //                    try
    //                    {
    //                        dtTempDateTime = DateTime.Parse(strValue);

    //                    }
    //                    catch
    //                    {

    //                    }

    //                    strValue = TheDatabaseS.ToJulianDate(dtTempDateTime).ToString();
    //                    strCalculation = strCalculation.ToUpper().Replace("[" + _dtColumnsAll.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
    //                }


    //            }

    //        }

    //        if (iParentRecordID != null)
    //        {
    //            string strParentTableID = Common.GetValueFromSQL("SELECT TableID FROM [Record] WHERE RecordID=" + iParentRecordID.ToString());
    //            Table theParnetTable = RecordManager.ets_Table_Details(int.Parse(strParentTableID));
    //            DataTable dtRecordTypleColumlnsP = RecordManager.ets_Table_Columns_All(int.Parse(strParentTableID),null,null);

    //            for (int j = 0; j < dtRecordTypleColumlnsP.Rows.Count; j++)
    //            {
    //                if (strCalculation.ToUpper().IndexOf("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]") > -1)
    //                {
    //                    string strValue = Common.GetValueFromSQL("SELECT " + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString() + " FROM Record WHERE RecordID=" + iParentRecordID.ToString());
    //                    if (strValue != "")
    //                    {
    //                        //lets convert it to datetime
    //                        DateTime dtTempDateTime = DateTime.Today;
    //                        try
    //                        {
    //                            dtTempDateTime = DateTime.Parse(strValue);
    //                        }
    //                        catch
    //                        {

    //                        }
    //                        strValue = TheDatabaseS.ToJulianDate(dtTempDateTime).ToString();
    //                        strCalculation = strCalculation.ToUpper().Replace("[" + theParnetTable.TableName.ToUpper() + ":" + dtRecordTypleColumlnsP.Rows[j]["SystemName"].ToString().ToUpper() + "]", strValue);
    //                    }
    //                }

    //            }
    //        }



    //        if (strCalculation.IndexOf("[") > -1)
    //        {
    //            //we have number here
    //            string regex = Common.NumberRegExDC; //@"\[(.*?)\]";  // Common.NumberRegExDC; //@"^.*?\([^\d]*(\d+)[^\d]*\).*$"
    //            string text = strCalculation;

    //            foreach (Match match in Regex.Matches(text, regex))
    //            {
    //                string strEachNumber = match.Groups[1].Value;
    //                string strEachJulianNumber = strEachNumber;

    //                switch (strResultFormat)
    //                {
    //                    case "datetime":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute

    //                        break;
    //                    case "date":
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;
    //                    case "time":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
    //                        break;
    //                    case "minute":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
    //                        break;
    //                    case "hour":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24)).ToString(); //hour
    //                        break;
    //                    case "day":
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;

    //                    case "ymd":
    //                        strEachJulianNumber = strEachNumber; //day



    //                        break;

    //                    case "ym":
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;

    //                    default:
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;
    //                }

    //                strCalculation = strCalculation.Replace("[" + strEachNumber + "]", strEachJulianNumber);
    //            }




    //        }

    //        strCalculation = strCalculation.Replace("[", "");
    //        strCalculation = strCalculation.Replace("]", "");

    //        string strJulianValue = Common.GetValueFromSQL("SELECT " + strCalculation);


    //        //implement Result Format

    //        if (strResultFormat != "")
    //        {

    //            strResult = strJulianValue;
    //            switch (strResultFormat)
    //            {
    //                case "datetime":

    //                    DateTime dtResult = DateTime.FromOADate(double.Parse(strResult));
    //                    //if (dtResult > DateTime.Today.AddYears(1000))
    //                    //    dtResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                    strResult = dtResult.ToString();
    //                    break;
    //                case "date":
    //                    DateTime dResult = DateTime.FromOADate(double.Parse(strResult));
    //                    //if (dResult > DateTime.Today.AddYears(1000))
    //                    //    dResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                    strResult = dResult.ToShortDateString();
    //                    break;
    //                case "time":
    //                    string strOriginal = strResult;
    //                    DateTime tResult = DateTime.FromOADate(double.Parse(strResult));
    //                    //if (tResult > DateTime.Today.AddYears(1000))
    //                    //    tResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                    string strDay="";
    //                    if (strResult.IndexOf(".") > -1)
    //                    {
    //                        strDay = strResult.Substring(0, strResult.IndexOf("."));
    //                        if(strDay=="0")
    //                        {
    //                            strDay = "";
    //                        }
    //                        else if (strDay=="1")
    //                        {
    //                            strDay = "1 day ";
    //                        }
    //                        else
    //                        {

    //                            strDay = strDay + " days ";
    //                        }
    //                    }

    //                    string strTimePart = tResult.ToLongTimeString();

    //                    strResult = strDay + strTimePart;
    //                    break;
    //                case "minute":
    //                    double dMinutes = double.Parse(strJulianValue) * 24 * 60;
    //                    strResult = ((int)Math.Round(dMinutes)).ToString();
    //                    break;
    //                case "hour":
    //                    double dHours = double.Parse(strJulianValue) * 24;
    //                    strResult = ((int)Math.Round(dHours)).ToString();
    //                    break;
    //                case "day":
    //                    double dDays = double.Parse(strJulianValue);
    //                    strResult = ((int)Math.Round(dDays)).ToString();
    //                    break;
    //                case "ymd":

    //                    double totalDays = double.Parse(strJulianValue);
    //                    string totalYears = Math.Truncate(totalDays / 365).ToString();
    //                    string totalMonths = Math.Truncate((totalDays % 365) / 30).ToString();
    //                    string remainingDays = Math.Truncate((totalDays % 365) % 30).ToString();
    //                    strResult = totalYears + " years " + totalMonths + " months " + remainingDays + " days";
    //                    break;

    //                case "ym":

    //                    double totalDays2 = double.Parse(strJulianValue);
    //                    string totalYears2 = Math.Truncate(totalDays2 / 365).ToString();
    //                    string totalMonths2 = Math.Truncate((totalDays2 % 365) / 30).ToString();

    //                    strResult = totalYears2 + " years " + totalMonths2 + " months";
    //                    break;

    //                default:
    //                    break;
    //            }

    //        }
    //    }
    //    catch
    //    {
    //        //
    //    }


    //    return strResult;

    //}


    //public static string GetDateCalculationResult(DataTable _dtRecordTypleColumlns, string strCalculation, int _iRecordID, int i)
    //{
    //    string strResult = "";

    //    try
    //    {
    //        string strResultFormat = "day";

    //        if (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
    //            strResultFormat = _dtRecordTypleColumlns.Rows[i]["DateCalculationType"].ToString().ToLower();



    //        for (int j = 0; j < _dtRecordTypleColumlns.Rows.Count; j++)
    //        {
    //            if (strCalculation.IndexOf(_dtRecordTypleColumlns.Rows[j]["SystemName"].ToString()) > -1)
    //            {
    //                string strValue = Common.GetValueFromSQL("SELECT " + _dtRecordTypleColumlns.Rows[j]["SystemName"].ToString() + " FROM Record WHERE RecordID=" + _iRecordID.ToString());
    //                if (strValue != "")
    //                {
    //                    //lets convert it to datetime
    //                    DateTime dtTempDateTime = DateTime.Today;
    //                    try
    //                    {
    //                        dtTempDateTime = DateTime.Parse(strValue);

    //                    }
    //                    catch
    //                    {

    //                    }

    //                    strValue = TheDatabaseS.ToJulianDate(dtTempDateTime).ToString();
    //                    strCalculation = strCalculation.Replace("[" + _dtRecordTypleColumlns.Rows[j]["SystemName"].ToString() + "]", strValue);
    //                }


    //            }

    //        }

    //        if (strCalculation.IndexOf("[") > -1)
    //        {
    //            //we have number here
    //            string regex = Common.NumberRegExDC; //@"\[(.*?)\]";  // Common.NumberRegExDC; //@"^.*?\([^\d]*(\d+)[^\d]*\).*$"
    //            string text = strCalculation;

    //            foreach (Match match in Regex.Matches(text, regex))
    //            {
    //                string strEachNumber = match.Groups[1].Value;
    //                string strEachJulianNumber = strEachNumber;

    //                switch (strResultFormat)
    //                {
    //                    case "datetime":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute

    //                        break;
    //                    case "date":
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;
    //                    case "time":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
    //                        break;
    //                    case "minute":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
    //                        break;
    //                    case "hour":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24)).ToString(); //hour
    //                        break;
    //                    case "day":
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;

    //                    case "ymd":
    //                        strEachJulianNumber = strEachNumber; //day



    //                        break;

    //                    case "ym":
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;

    //                    default:
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;
    //                }

    //                strCalculation = strCalculation.Replace("[" + strEachNumber + "]", strEachJulianNumber);
    //            }




    //        }

    //        strCalculation = strCalculation.Replace("[", "");
    //        strCalculation = strCalculation.Replace("]", "");

    //        string strJulianValue = Common.GetValueFromSQL("SELECT " + strCalculation);


    //        //implement Result Format

    //        if (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
    //        {

    //            strResult = strJulianValue;
    //            switch (strResultFormat)
    //            {
    //                case "datetime":

    //                    DateTime dtResult = DateTime.FromOADate(double.Parse(strResult));
    //                    //if (dtResult > DateTime.Today.AddYears(1000))
    //                    //    dtResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                    strResult = dtResult.ToString();
    //                    break;
    //                case "date":
    //                    DateTime dResult = DateTime.FromOADate(double.Parse(strResult));
    //                    //if (dResult > DateTime.Today.AddYears(1000))
    //                    //    dResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                    strResult = dResult.ToShortDateString();
    //                    break;
    //                case "time":
    //                    DateTime tResult = DateTime.FromOADate(double.Parse(strResult));
    //                    //if (tResult > DateTime.Today.AddYears(1000))
    //                    //    tResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                    strResult = tResult.ToLongTimeString();
    //                    break;
    //                case "minute":
    //                    double dMinutes = double.Parse(strJulianValue) * 24 * 60;
    //                    strResult = ((int)Math.Round(dMinutes)).ToString();
    //                    break;
    //                case "hour":
    //                    double dHours = double.Parse(strJulianValue) * 24;
    //                    strResult = ((int)Math.Round(dHours)).ToString();
    //                    break;
    //                case "day":
    //                    double dDays = double.Parse(strJulianValue);
    //                    strResult = ((int)Math.Round(dDays)).ToString();
    //                    break;
    //                case "ymd":

    //                    double totalDays = double.Parse(strJulianValue);
    //                    string totalYears = Math.Truncate(totalDays / 365).ToString();
    //                    string totalMonths = Math.Truncate((totalDays % 365) / 30).ToString();
    //                    string remainingDays = Math.Truncate((totalDays % 365) % 30).ToString();
    //                    strResult = totalYears + " years " + totalMonths + " months " + remainingDays + " days";
    //                    break;

    //                case "ym":

    //                    double totalDays2 = double.Parse(strJulianValue);
    //                    string totalYears2 = Math.Truncate(totalDays2 / 365).ToString();
    //                    string totalMonths2 = Math.Truncate((totalDays2 % 365) / 30).ToString();

    //                    strResult = totalYears2 + " years " + totalMonths2 + " months";
    //                    break;

    //                default:
    //                    break;
    //            }

    //        }
    //    }
    //    catch
    //    {
    //        //
    //    }
        

    //    return strResult;

    //}

    

}
public class TheDatabase
{
	public TheDatabase()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public static string GetCode(Control _control, string strValidationGroup)
    {
        // building helper
        StringBuilder _output = new StringBuilder();

        // the logic of scanning
        if (_control.GetType().GetProperty("ValidationGroup") != null && !string.IsNullOrEmpty(_control.ID))
        {
            // the desired code
            _output.AppendFormat("{0}.{1} = {2};", _control.ID, "ValidationGroup", strValidationGroup);
            _output.AppendLine();
        }

        // recursive search within children
        _output.Append(GetCode(_control.Controls, strValidationGroup));

        // outputting
        return _output.ToString();
    }

    public static string GetCode(ControlCollection _collection, string strValidationGroup)
    {
        // building helper
        StringBuilder _output = new StringBuilder();
        foreach (Control _control in _collection)
        {
            // get code for each child
            _output.Append(GetCode(_control, strValidationGroup));
        }
        // outputting
        return _output.ToString();
    }

    public static void SetValidationGroup(ControlCollection _collection, string strValidationGroup)
    {      
        foreach (Control _control in _collection)
        {
            // set each child
            SetValidationGroup(_control, strValidationGroup);
        }
      
    }
    public static void SetValidationGroup(Control _control, string strValidationGroup)
    {
    
        // the logic of scanning
        if (_control.GetType().GetProperty("ValidationGroup") != null && !string.IsNullOrEmpty(_control.ID))
        {
            _control.GetType().GetProperty("ValidationGroup").SetValue(_control, strValidationGroup);
          
        }
        // recursive search within children
        SetValidationGroup(_control.Controls, strValidationGroup);
    
    }
    public static bool IsRecordDuplicate(Record theRecord, string strUniqueColumnIDSys, string strUniqueColumnID2Sys,int iRecordID)
    {
        if (strUniqueColumnIDSys != "" || strUniqueColumnID2Sys != "")
        {
            string strUniqueColumnIDValue = "";
            string strUniqueColumnID2Value = "";
            if (strUniqueColumnIDSys != "")
                strUniqueColumnIDValue = RecordManager.GetRecordValue(ref theRecord, strUniqueColumnIDSys);

            if (strUniqueColumnID2Sys != "")
                strUniqueColumnID2Value = RecordManager.GetRecordValue(ref theRecord, strUniqueColumnID2Sys);

            if (RecordManager.ets_Record_IsDuplicate_Entry((int)theRecord.TableID, iRecordID, strUniqueColumnIDSys, strUniqueColumnIDValue,
                strUniqueColumnID2Sys, strUniqueColumnID2Value))
            {
               
                return true;
            }

        }
        return false;
    }
    public static void PerformAllValidation(ref Record theRecord, ref DataTable dtValidWarning,
       bool bAddToGrid, bool bSendEmail, DataTable _dtColumnsAll, ref string _strValidationError, ref string _strInValidResults,
        bool _bShowExceedances, ref string _strExceedanceResults, int _iSessionAccountID, string _strURL,ref string _strExceedanceEmailFullBody,
        ref string _strExceedanceSMSFullBody, ref int _iExceedanceColumnCount, ref string _strWarningResults,ref string _strWarningEmailFullBody,
        ref string _strWarningSMSFullBody,ref int _iWarningColumnCount)
    {
        bool bEachColumnExceedance = false;
        bool bEachColumnInValid = false;
        string strTemp = "";
        if (bSendEmail)
            bAddToGrid = false;//in case

        for (int i = 0; i < _dtColumnsAll.Rows.Count; i++)
        {
            //ALL Validation
            bEachColumnExceedance = false;
            bEachColumnInValid = false;
            string strValue = RecordManager.GetRecordValue(ref theRecord, _dtColumnsAll.Rows[i]["SystemName"].ToString());
            if (strValue != "")
            {
                bool bValidationCanIgnore = false;
                if (_dtColumnsAll.Rows[i]["ValidationCanIgnore"] != DBNull.Value && (bool)_dtColumnsAll.Rows[i]["ValidationCanIgnore"])
                {
                    bValidationCanIgnore = true;
                }


                string strFormulaV = "";

                if (_dtColumnsAll.Rows[i]["ConV"] != DBNull.Value)
                {
                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsAll.Rows[i]["ConV"].ToString()));
                    if (theCheckColumn != null)
                    {
                        string strCheckValue = RecordManager.GetRecordValue(ref theRecord, theCheckColumn.SystemName);
                        strFormulaV = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                            "V", strCheckValue);
                    }
                }
                else
                {
                    if (_dtColumnsAll.Rows[i]["ValidationOnEntry"] != DBNull.Value && _dtColumnsAll.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                    {
                        strFormulaV = _dtColumnsAll.Rows[i]["ValidationOnEntry"].ToString();
                    }
                }

                if (strFormulaV != "" && !UploadManager.IsDataValid(strValue, strFormulaV, ref _strValidationError))
                {
                    if (bValidationCanIgnore)
                    {
                        _strWarningResults = _strWarningResults + TheDatabase.GetInvalidIgnored_msg(_dtColumnsAll.Rows[i]["DisplayName"].ToString());

                    }
                    else
                    {
                        _strInValidResults = _strInValidResults + TheDatabase.GetInvalid_msg(_dtColumnsAll.Rows[i]["DisplayName"].ToString());

                    }

                    bEachColumnInValid = true;

                    if (bAddToGrid)
                    {
                        dtValidWarning.Rows.Add(_dtColumnsAll.Rows[i]["ColumnID"].ToString(), "i", "no",
                        Common.GetFromulaMsg("i", _dtColumnsAll.Rows[i]["DisplayName"].ToString(), strFormulaV)//    "Invalid data - " + _dtColumnsAll.Rows[i]["DisplayName"].ToString()
                            , strFormulaV, strValue);

                    }
                }
                //}

                //bEachColumnExceedance = false;
                if (_bShowExceedances && bEachColumnInValid == false)
                {

                    string strFormulaE = "";

                    if (_dtColumnsAll.Rows[i]["ConE"] != DBNull.Value)
                    {
                        Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsAll.Rows[i]["ConE"].ToString()));
                        if (theCheckColumn != null)
                        {
                            string strCheckValue = RecordManager.GetRecordValue(ref theRecord, theCheckColumn.SystemName);
                            strFormulaE = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                "E", strCheckValue);
                        }
                    }
                    else
                    {
                        if (_dtColumnsAll.Rows[i]["ValidationOnExceedance"] != DBNull.Value && _dtColumnsAll.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                        {
                            strFormulaE = _dtColumnsAll.Rows[i]["ValidationOnExceedance"].ToString();
                        }
                    }


                    if (strFormulaE != "")
                    {
                        if (!UploadManager.IsDataValid(strValue, strFormulaE, ref _strValidationError))
                        {
                            _strExceedanceResults = _strExceedanceResults + TheDatabase.GetExceedance_msg(_dtColumnsAll.Rows[i]["DisplayName"].ToString());
                            bEachColumnExceedance = true;
                            //_bDataExceedance = true;
                            if (bAddToGrid)
                            {
                                dtValidWarning.Rows.Add(_dtColumnsAll.Rows[i]["ColumnID"].ToString(), "e", "yes",
                                  Common.GetFromulaMsg("e", _dtColumnsAll.Rows[i]["DisplayName"].ToString(), strFormulaE)//  "EXCEEDANCE: " + _dtColumnsAll.Rows[i]["DisplayName"].ToString() + " –  Value outside accepted range."
                                    , strFormulaE, strValue);
                            }

                            if (bSendEmail)
                            {
                                RecordManager.BuildDataExceedanceSMSandEmail(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), strValue, theRecord.DateTimeRecorded.ToString(),
                                    ref strTemp, _iSessionAccountID, _strURL, ref _strExceedanceEmailFullBody, ref _strExceedanceSMSFullBody, ref _iExceedanceColumnCount);

                            }
                        }

                    }
                }

                if (bEachColumnExceedance == false && bEachColumnInValid == false)
                {

                    string strFormulaW = "";

                    if (_dtColumnsAll.Rows[i]["ConW"] != DBNull.Value)
                    {
                        Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsAll.Rows[i]["ConW"].ToString()));
                        if (theCheckColumn != null)
                        {
                            string strCheckValue = RecordManager.GetRecordValue(ref theRecord, theCheckColumn.SystemName);
                            strFormulaW = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                "W", strCheckValue);
                        }
                    }
                    else
                    {
                        if (_dtColumnsAll.Rows[i]["ValidationOnWarning"] != DBNull.Value && _dtColumnsAll.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                        {
                            strFormulaW = _dtColumnsAll.Rows[i]["ValidationOnWarning"].ToString();
                        }
                    }


                    if (strFormulaW != "" && !UploadManager.IsDataValid(strValue, strFormulaW, ref _strValidationError))
                    {
                        _strWarningResults = _strWarningResults + TheDatabase.GetWarning_msg(_dtColumnsAll.Rows[i]["DisplayName"].ToString());
                        //_bDataWarning = true;
                        if (bAddToGrid)
                        {
                            dtValidWarning.Rows.Add(_dtColumnsAll.Rows[i]["ColumnID"].ToString(), "w", "no",
                               Common.GetFromulaMsg("w", _dtColumnsAll.Rows[i]["DisplayName"].ToString(), strFormulaW)// "WARNING: " + _dtColumnsAll.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range."
                                , strFormulaW, strValue);
                        }

                        if (bSendEmail)
                        {
                            RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), strValue, theRecord.DateTimeRecorded.ToString(),
                                ref strTemp, _iSessionAccountID, _strURL, ref _strWarningEmailFullBody, ref _strWarningSMSFullBody, ref _iWarningColumnCount);
                        }
                    }

                }
            }
        }
    }
    public static string GetAdjustValidationNotification(string strNotifications, string strInvalidRecordIDs, string strValidRecordIDs, Column editColumn)
    {
        if (strInvalidRecordIDs != "" )
        {
            strNotifications = strNotifications + "Validation Adjusted:" + editColumn.DisplayColumn + " " + (strInvalidRecordIDs.Split(',').Length - 1).ToString() + " invalid records";
            //strNotifications = strNotifications + " and " + (strValidRecordIDs.Split(',').Length - 1).ToString() + " records became valid from invalid.";
        }
        //else if (strInvalidRecordIDs != "")
        //{
        //    strNotifications = strNotifications + "Validation Adjusted:" + editColumn.DisplayColumn + " " + (strInvalidRecordIDs.Split(',').Length - 1).ToString() + " invalid records.";
        //}
        //else if (strInvalidRecordIDs != "" && strValidRecordIDs != "")
        //{
        //    strNotifications = strNotifications + "Validation Adjusted:" + editColumn.DisplayColumn + " " + (strValidRecordIDs.Split(',').Length - 1).ToString() + " records became valid from invalid";
        //}

        return strNotifications;
    }
   
    public static string GetInvalid_msg(string strDisplayName)
    {
        return "INVALID: " + strDisplayName + ".";
    }
    public static string GetInvalidIgnored_msg(string strDisplayName)
    {
        return "INVALID (and ignored): " + strDisplayName + ".";
    }

    public static string GetWarning_msg(string strDisplayName)
    {
        return "WARNING: " + strDisplayName + " – Value outside accepted range.";
    }
    public static string GetWarningUnlikely_msg(string strDisplayName)
    {
        return "WARNING: " + strDisplayName + " – Unlikely data – outside 3 standard deviations.";
    }
    public static string GetExceedance_msg(string strDisplayName)
    {
        return "EXCEEDANCE: " + strDisplayName + " – Value outside accepted range.";
    }

    public static bool HasInvalidIgnored_msg(string strWarningResults, string strDisplayName, string strType)
    {
        string strFullMsg = GetInvalidIgnored_msg(strDisplayName);
        return strWarningResults.IndexOf(strFullMsg.Substring(0, (strFullMsg.Length - 1))) > -1;
    }

    public static bool HasInvalid_msg(string strWarningResults, string strDisplayName, string strType)
    {
        string strFullMsg = GetInvalid_msg(strDisplayName);
        return strWarningResults.IndexOf(strFullMsg.Substring(0, (strFullMsg.Length - 1))) > -1;
    }

    public static bool HasExceedance_msg(string strWarningResults, string strDisplayName, string strType)
    {
        string strFullMsg = GetExceedance_msg(strDisplayName);

        if (strType == "")
        {
            return strWarningResults.IndexOf("EXCEEDANCE: " + strDisplayName) > -1;
        }
        if (strType == "l")
        {
            return strWarningResults.IndexOf(strFullMsg.Substring(0, (strFullMsg.Length - 5))) > -1;
        }

        return false;
    }

    public static bool HasWarning_msg(string strWarningResults, string strDisplayName,string strType)
    {
        string strFullMsg = GetWarning_msg(strDisplayName);

        if (strType=="")
        {
            return strWarningResults.IndexOf("WARNING: " + strDisplayName) > -1;
        }
        if (strType == "l")
        {
            return strWarningResults.IndexOf(strFullMsg.Substring(0,(strFullMsg.Length-5))) > -1;
        }

        return false;
    }

    public static bool HasWarningUnlikely_msg(string strWarningResults, string strDisplayName, string strType)
    {
        string strFullMsg = GetWarningUnlikely_msg(strDisplayName);

        if (strType == "")
        {
            return strWarningResults.IndexOf("WARNING: " + strDisplayName) > -1;
        }
        if (strType == "l")
        {
            return strWarningResults.IndexOf(strFullMsg.Substring(0, (strFullMsg.Length - 5))) > -1;
        }

        return false;
    }

    public static void PutCheckBoxListValues(string strDropdownValues, ref  CheckBoxList lb)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in result)
        {
            ListItem liTemp = new ListItem(s, s);
            lb.Items.Add(liTemp);
        }

    }


    public static void SetCheckBoxListValues(string strDBValues, ref  CheckBoxList lb, string strDropdownValues)
    {


        if (strDBValues != "")
        {

            lb.Items.Clear();


            string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);


            string[] strSS = strDBValues.Split(',');

            foreach (string s in result)
            {

                foreach (string SS in strSS)
                {
                    if (SS == s)
                    {
                        ListItem liTemp = new ListItem(s, s);
                        lb.Items.Add(liTemp);
                    }
                }
            }

            foreach (string s in result)
            {

                if (lb.Items.FindByValue(s) == null)
                {
                    ListItem liTemp = new ListItem(s, s);
                    lb.Items.Add(liTemp);
                }

            }

            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }

            foreach (ListItem li in lb.Items)
            {
                li.Attributes.Add("DataValue", li.Value);
            }
        }

    }


    public static void SetCheckBoxListValues_Text(string strDBValues, ref  CheckBoxList lb, string strDropdownValues)
    {


        if (strDBValues != "")
        {

            lb.Items.Clear();


            string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string strValue = "";
            string strText = "";

            string[] strSS = strDBValues.Split(',');

            foreach (string s in result)
            {
                strValue = "";
                strText = "";

                if (s.IndexOf(",") > -1)
                {
                    strValue = s.Substring(0, s.IndexOf(","));
                    strText = s.Substring(strValue.Length + 1);
                }

                foreach (string SS in strSS)
                {
                    if (SS == strValue)
                    {
                        ListItem liTemp = new ListItem(strText, strValue);
                        lb.Items.Add(liTemp);
                    }
                }
            }

            foreach (string s in result)
            {


                strValue = "";
                strText = "";

                if (s.IndexOf(",") > -1)
                {
                    strValue = s.Substring(0, s.IndexOf(","));
                    strText = s.Substring(strValue.Length + 1);
                }

                if (lb.Items.FindByValue(strValue) == null)
                {
                    ListItem liTemp = new ListItem(strText, strValue);
                    lb.Items.Add(liTemp);
                }

            }

            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }

            foreach (ListItem li in lb.Items)
            {
                li.Attributes.Add("DataValue", li.Value);
            }
        }

    }

    public static void PutListValues_Text(string strDropdownValues, ref  ListBox lb)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    ListItem liTemp = new ListItem(strText, strValue);
                    lb.Items.Add(liTemp);
                }
            }
        }


    }

    public static void SetListValues(string strDBValues, ref  ListBox lb)
    {
        if (strDBValues != "")
        {
            string[] strSS = strDBValues.Split(',');
            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }
        }

    }

    public static void PutListValues(string strDropdownValues, ref  ListBox lb)
    {
        lb.Items.Clear();
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in result)
        {
            ListItem liTemp = new ListItem(s, s);
            lb.Items.Add(liTemp);
        }


    }

    public static string GetListValues(ListBox lb)
    {
        string strSelectedValues = "";

        foreach (ListItem item in lb.Items)
        {
            if (item.Selected)
            {
                strSelectedValues = strSelectedValues + item.Value + ",";
            }
        }

        if (strSelectedValues != "")
            strSelectedValues = strSelectedValues.Substring(0, strSelectedValues.Length - 1);
        return strSelectedValues;
    }

    public static string GetCheckBoxValue(string strDropdownValues, ref  CheckBox chk)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in result)
        {
            if (i == 0)
            {
                if (chk.Checked)
                {
                    return s;
                }
            }
            if (i == 1)
            {
                if (chk.Checked == false)
                {
                    return s;
                }
            }
            i = i + 1;
        }
        return "";
    }

    public static void SetCheckBoxValue(string strDropdownValues, string strValue, ref  CheckBox chk)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in result)
        {
            if (i == 0)
            {
                if (s.ToLower() == strValue.ToLower())
                {
                    chk.Checked = true;
                }
            }
            if (i == 1)
            {
                if (s.ToLower() == strValue.ToLower())
                {
                    chk.Checked = false;
                }
            }
            i = i + 1;
        }


    }
    public static void PutCheckBoxDefault(string strDropdownValues, ref  CheckBox chk)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in result)
        {
            if (i == 2)
            {
                if (s.ToLower() == "yes")
                {
                    chk.Checked = true;
                }
            }
            i = i + 1;
        }


    }
   
    public static void PutRadioList(string strDropdownValues, ref  RadioButtonList rl)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        rl.Items.Clear();
        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            ListItem liTemp = new ListItem(s + "&nbsp;&nbsp;", s);
            rl.Items.Add(liTemp);
        }

    }
    
    public static void PutRadioListValue_Text(string strDropdownValues, ref  RadioButtonList rl)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        rl.Items.Clear();
        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    ListItem liTemp = new ListItem(strText + "&nbsp;&nbsp;", strValue);
                    rl.Items.Add(liTemp);
                }
            }
        }


    }

     public static void PutCheckboxIntoDDL(string strDropdownValues, ref  DropDownList ddl)
    {
         PutDDLValues( strDropdownValues, ref  ddl);
         if(ddl.Items.Count>1)
         {
             ddl.Items.RemoveAt(ddl.Items.Count - 1);// remove the defaule part
         }
    }
    public static void PutDDLValues(string strDropdownValues, ref  DropDownList ddl)
    {
        ddl.Items.Clear();

        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            ListItem liTemp = new ListItem(s, s);
            ddl.Items.Add(liTemp);
        }

        ListItem liSelect = new ListItem("--Please Select--", "");
        ddl.Items.Insert(0, liSelect);

    }


    public static void PutDDLValue_Text(string strDropdownValues, ref  DropDownList ddl)
    {
        ddl.Items.Clear();
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    ListItem liTemp = new ListItem(strText, strValue);
                    ddl.Items.Add(liTemp);
                }
            }
        }

        ListItem liSelect = new ListItem("--Please Select--", "");
        ddl.Items.Insert(0, liSelect);

    }

    public static int Account24769_spAddNewMedication(int PatientID, int NewMedicationID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("Account24769.spAddNewMedication", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@PatientID", PatientID));
                command.Parameters.Add(new SqlParameter("@NewMedicationID", NewMedicationID));

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


}