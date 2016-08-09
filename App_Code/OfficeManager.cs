using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

//using Microsoft.Office.Interop.Excel;
using System.Text;
using System.Reflection;
using System.Data.OleDb;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

/// <summary>
/// Summary description for OfficeManager
/// </summary>
public class OfficeManager
{
	public OfficeManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private static void GetFileNameAndPath(string completePath, ref string fileName, ref string folderPath)
    {
        string[] fileSep = completePath.Split('\\');
        for (int iCount = 0; iCount < fileSep.Length; iCount++)
        {
            if (iCount == fileSep.Length - 2)
            {
                if (fileSep.Length == 2)
                {
                    folderPath += fileSep[iCount] + "\\";
                }
                else
                {
                    folderPath += fileSep[iCount];
                }
            }
            else
            {
                if (fileSep[iCount].IndexOf(".") > 0)
                {
                    fileName = fileSep[iCount];
                    fileName = fileName.Substring(0, fileName.IndexOf("."));
                }
                else
                {
                    folderPath += fileSep[iCount] + "\\";
                }
            }
        }
    }

    public static DataTable ImportDBF_Odbc(string filePath)
    {
        string ImportDirPath = string.Empty;
        string tableName = string.Empty;

        // This function give the Folder name and table name to use in
        // the connection string and create table statement.
        GetFileNameAndPath(filePath, ref tableName, ref ImportDirPath);

        System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection();
        //conn.ConnectionString = "DRIVER={Microsoft dBase Driver (*.dbf)};Deleted=1";


//        using (OdbcConnection conn = new OdbcConnection("Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;Exclusive=No;Collate=Machine;NULL=NO;DELETED=YES;BACKGROUNDFETCH=NO;SourceDB=yourFilePath.db")
//{
//  conn.Open();
//}
        conn.ConnectionString = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;Exclusive=No;Collate=Machine;NULL=NO;DELETED=YES;BACKGROUNDFETCH=NO;SourceDB=" + filePath + ";";
        DataTable dt = new DataTable();
        dt = null;

        dt = GetODBCDatatable(filePath, ref conn);

        if(dt==null)
        {
            conn.ConnectionString = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;Exclusive=No;Collate=Machine;NULL=NO;DELETED=YES;BACKGROUNDFETCH=NO;SourceDB=" + ImportDirPath + ";";
            dt = GetODBCDatatable(filePath, ref conn);
        }

        if (dt == null)
        {
            conn.ConnectionString = "DRIVER={Microsoft dBase Driver (*.dbf)};Deleted=1";
            dt = GetODBCDatatable(filePath, ref conn);
        }

        return dt;
    }

    private static DataTable GetODBCDatatable(string filePath, ref System.Data.Odbc.OdbcConnection conn)
    {
        string ImportDirPath = string.Empty;
        string tableName = string.Empty;
        DataTable dt = new DataTable();

        GetFileNameAndPath(filePath, ref tableName, ref ImportDirPath);

        try
        {


            conn.Open();
            System.Data.Odbc.OdbcCommand comm = new System.Data.Odbc.OdbcCommand();
            comm.CommandText = @"SELECT * FROM " + tableName; //tableName

            comm.Connection = conn;

            dt.Load(comm.ExecuteReader());
        }
        catch
        {
            try
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                conn.Open();
                System.Data.Odbc.OdbcCommand comm = new System.Data.Odbc.OdbcCommand();
                comm.CommandText = @"SELECT * FROM " + tableName + ".dbf"; //tableName

                comm.Connection = conn;

                dt.Load(comm.ExecuteReader());
            }
            catch
            {
                try
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();

                    conn.Open();
                    System.Data.Odbc.OdbcCommand comm = new System.Data.Odbc.OdbcCommand();
                    comm.CommandText = @"SELECT * FROM " + tableName.Substring(37); //tableName

                    comm.Connection = conn;

                    dt.Load(comm.ExecuteReader());
                }
                catch
                {
                    try
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();

                        conn.Open();
                        System.Data.Odbc.OdbcCommand comm = new System.Data.Odbc.OdbcCommand();
                        comm.CommandText = @"SELECT * FROM " + tableName.Substring(37) + ".dbf"; //tableName

                        comm.Connection = conn;

                        dt.Load(comm.ExecuteReader());
                    }
                    catch
                    {
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();

                        conn.Open();
                        System.Data.Odbc.OdbcCommand comm = new System.Data.Odbc.OdbcCommand();
                        comm.CommandText = @"SELECT * FROM " + filePath; //tableName

                        comm.Connection = conn;

                        dt.Load(comm.ExecuteReader());
                    }

                }

            }
        }
        finally
        {
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }

        return dt;
    }

    private static OleDbDataAdapter GetOleDbDataAdapter(string filePath, ref OleDbConnection conn)
    {

        string ImportDirPath = string.Empty;
        string tableName = string.Empty;
        // This function give the Folder name and table name to use in
        // the connection string and create table statement.
        GetFileNameAndPath(filePath, ref tableName, ref ImportDirPath);

        OleDbDataAdapter daGetTableData = null;
        try
        {
            daGetTableData = new OleDbDataAdapter("Select *  FROM " + tableName, conn);
        }
        catch
        {
            try
            {
                daGetTableData = new OleDbDataAdapter("Select *  FROM " + tableName + ".dbf", conn);
            }
            catch
            {
                try
                {
                    daGetTableData = new OleDbDataAdapter("Select *  FROM " + tableName.Substring(37), conn);
                }
                catch
                {
                    try
                    {
                        daGetTableData = new OleDbDataAdapter("Select *  FROM " + tableName.Substring(37) + ".dbf", conn);
                    }
                    catch
                    {
                        daGetTableData = new OleDbDataAdapter("Select *  FROM " + filePath, conn);
                    }
                    
                }
            }

        }

        return null;
    }
    public static DataTable ImportDBF_VFP(string filePath)
    {
        try
        {
            string ImportDirPath = string.Empty;
            string tableName = string.Empty;
            // This function give the Folder name and table name to use in
            // the connection string and create table statement.
            GetFileNameAndPath(filePath, ref tableName, ref ImportDirPath);
            //tableName = "[" + tableName + "]";
            DataSet dsImport = new DataSet();
            //string thousandSep = thousandSeparator;
            //string connString = "Provider=Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + ImportDirPath + "; Extended Properties=dBASE IV;";

            //string connString = "Provider=vfpoledb;Data Source=" + ImportDirPath + ";Collating Sequence=machine;"; Collating Sequence=machine;

            string connString = @"Provider=vfpoledb;Data Source=" + ImportDirPath;

            OleDbConnection conn = new OleDbConnection(connString);
            DataSet dsGetData = new DataSet();
            OleDbDataAdapter daGetTableData;

            daGetTableData = GetOleDbDataAdapter(filePath, ref conn);


            

            if (daGetTableData == null)
            {
                connString = @"Provider=vfpoledb;Data Source=" + filePath + ";Collating Sequence=machine;";
                conn = new OleDbConnection(connString);
                daGetTableData = GetOleDbDataAdapter(filePath, ref conn);
            }

            if (daGetTableData == null)
            {
                connString = @"Provider=vfpoledb;Data Source=" + filePath + ";Collating Sequence=general;";
                conn = new OleDbConnection(connString);
                daGetTableData = GetOleDbDataAdapter(filePath, ref conn);
            }

            if (daGetTableData == null)
            {
                connString = @"Provider=vfpoledb;Data Source=" + ImportDirPath + ";Collating Sequence=general;";
                conn = new OleDbConnection(connString);
                daGetTableData = GetOleDbDataAdapter(filePath, ref conn);
            }
            if (daGetTableData == null)
            {
                connString = @"Provider=vfpoledb;Data Source=" + ImportDirPath + ";Collating Sequence=machine;";
                conn = new OleDbConnection(connString);
                daGetTableData = GetOleDbDataAdapter(filePath, ref conn);
            }

            // fill all the data in to dataset
            daGetTableData.Fill(dsGetData);
            DataTable dt = new DataTable(dsGetData.Tables[0].TableName.ToString());
            dsImport.Tables.Add(dt);
            // here I am copying get Dataset into another dataset because //before return the dataset I want to format the data like change //"datesymbol","thousand symbol" and date format as did while
            // exporting. If you do not want to format the data then you can // directly return the dsGetData
            for (int row = 0; row < dsGetData.Tables[0].Rows.Count; row++)
            {
                DataRow dr = dsImport.Tables[0].NewRow();
                dsImport.Tables[0].Rows.Add(dr);
                for (int col = 0; col < dsGetData.Tables[0].Columns.Count; col++)
                {
                    if (row == 0)
                    {
                        DataColumn dc = new DataColumn(dsGetData.Tables[0].Columns[col].ColumnName.ToString());
                        dsImport.Tables[0].Columns.Add(dc);
                    }
                    if (!String.IsNullOrEmpty(dsGetData.Tables[0].Rows[row][col].
                    ToString()))
                    {
                        dsImport.Tables[0].Rows[row][col] = Convert.ToString(dsGetData.Tables[0].Rows[row][col].ToString().Trim());
                    }
                } // close inner for loop
            }// close ouer for loop

            return dsImport.Tables[0];

        }
        catch(Exception ex)
        {
            return null;
        }
        
    } // close function


    public static DataTable ImportDBF_OLEDB(string filePath)
    {
        try
        {
            string ImportDirPath = string.Empty;
            string tableName = string.Empty;
            // This function give the Folder name and table name to use in
            // the connection string and create table statement.
            GetFileNameAndPath(filePath, ref tableName, ref ImportDirPath);
            //tableName = "[" + tableName + "]";
            DataSet dsImport = new DataSet();
            //string thousandSep = thousandSeparator;
            string connString = "Provider=Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + ImportDirPath + "; Extended Properties=dBASE IV;";

            OleDbConnection conn;

           
            DataSet dsGetData = new DataSet();
            OleDbDataAdapter daGetTableData=null;

            conn = new OleDbConnection(connString);

            daGetTableData = GetOleDbDataAdapter(filePath, ref conn);

            if (daGetTableData==null)
            {
                connString = "Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277;Dbq="+ImportDirPath+";";
                conn = new OleDbConnection(connString);
                daGetTableData = GetOleDbDataAdapter(filePath, ref conn);
            }

            if (daGetTableData == null)
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ImportDirPath + ";Extended Properties=dBASE IV;";
                conn = new OleDbConnection(connString);
                daGetTableData = GetOleDbDataAdapter(filePath, ref conn);
            }

            if (daGetTableData == null)
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ImportDirPath + ";Extended Properties=dBASE IV;User ID=Admin;";
                conn = new OleDbConnection(connString);
                daGetTableData = GetOleDbDataAdapter(filePath, ref conn);
            }

            if (daGetTableData == null)
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ImportDirPath + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
                conn = new OleDbConnection(connString);
                daGetTableData = GetOleDbDataAdapter(filePath, ref conn);
            }

            // fill all the data in to dataset
            daGetTableData.Fill(dsGetData);
            DataTable dt = new DataTable(dsGetData.Tables[0].TableName.ToString());
            dsImport.Tables.Add(dt);
            // here I am copying get Dataset into another dataset because //before return the dataset I want to format the data like change //"datesymbol","thousand symbol" and date format as did while
            // exporting. If you do not want to format the data then you can // directly return the dsGetData
            for (int row = 0; row < dsGetData.Tables[0].Rows.Count; row++)
            {
                DataRow dr = dsImport.Tables[0].NewRow();
                dsImport.Tables[0].Rows.Add(dr);
                for (int col = 0; col < dsGetData.Tables[0].Columns.Count; col++)
                {
                    if (row == 0)
                    {
                        DataColumn dc = new DataColumn(dsGetData.Tables[0].Columns[col].ColumnName.ToString());
                        dsImport.Tables[0].Columns.Add(dc);
                    }
                    if (!String.IsNullOrEmpty(dsGetData.Tables[0].Rows[row][col].
                    ToString()))
                    {
                        dsImport.Tables[0].Rows[row][col] = Convert.ToString(dsGetData.Tables[0].Rows[row][col].ToString().Trim());
                    }
                } // close inner for loop
            }// close ouer for loop

            return dsImport.Tables[0];

        }
        catch (Exception ex)
        {
            return null;
        }

    } 

    public static List<string> GetExcelSheetNames(string strImportFolder, string strFileUniqueName)
    {

        try
        {

            string fileName = strImportFolder + "\\" + strFileUniqueName;

            //open the excel file using OLEDB  
            OleDbConnection con;
            string connectionString;
            try
            {
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                fileName + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                con = new OleDbConnection(connectionString);
                con.Open();
            }
            catch (Exception ex)
            {
                try
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                fileName + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                    con = new OleDbConnection(connectionString);
                    con.Open();
                }
                catch (Exception ex2)
                {

                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileName + ";Extended Properties=\"HTML Import;HDR=YES;\"";
                        con = new OleDbConnection(connectionString);
                        con.Open();
                    }
                    catch (Exception ex3)
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", fileName);
                        con = new OleDbConnection(connectionString);
                        con.Open();
                    }

                }
            }


            //get all the available sheets  
            System.Data.DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //get the number of sheets in the file  
            //string[] workSheetNames = new String[dt.Rows.Count];
            List<string> workSheetNames=new List<string>();
            int i = 0;
            DataTable dtResult = null;
            foreach (DataRow row in dt.Rows)
            {
                //insert the sheet's name in the current element of the array  
                //and remove the $ sign at the end  
                //workSheetNames[i] = row["TABLE_NAME"].ToString().Trim(new[] { '$' });
                string strTempWSName = row["TABLE_NAME"].ToString();

                if (row["TABLE_NAME"].ToString().Substring(0, 1) == "'")
                {
                    strTempWSName = strTempWSName.Substring(1, strTempWSName.Length - 1);
                }

                if (row["TABLE_NAME"].ToString().Substring(row["TABLE_NAME"].ToString().Length - 1, 1) == "'")
                {
                    strTempWSName = strTempWSName.Substring(0, strTempWSName.Length - 1);
                }

                //workSheetNames[i] = strTempWSName.Replace("$", "").Trim();
                try
                {
                    //DataTable dtTempSheet = GetWorksheet(strTempWSName.Replace("$", "").Trim(), connectionString);
                    DataTable dtTempSheet = GetWorksheet2(strTempWSName, connectionString,false);
                    if (dtTempSheet != null)
                    {
                        if (dtTempSheet.Columns.Count > 1)
                        {
                            workSheetNames.Add(strTempWSName.Replace("$", "").Trim());
                        }
                    }
                  
                }
                    catch 
                {
                }
                
            }

            return workSheetNames;
           
        }
        catch (Exception ex)
        {
            return null;

        }

    }


    public static List<string> GetExcelFirstSheetNames(string strImportFolder, string strFileUniqueName)
    {

        try
        {

            string fileName = strImportFolder + "\\" + strFileUniqueName;

            //open the excel file using OLEDB  
            OleDbConnection con;
            string connectionString;
            try
            {
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                fileName + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                con = new OleDbConnection(connectionString);
                con.Open();
            }
            catch (Exception ex)
            {
                try
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                fileName + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                    con = new OleDbConnection(connectionString);
                    con.Open();
                }
                catch (Exception ex2)
                {

                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileName + ";Extended Properties=\"HTML Import;HDR=YES;\"";
                        con = new OleDbConnection(connectionString);
                        con.Open();
                    }
                    catch (Exception ex3)
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", fileName);
                        con = new OleDbConnection(connectionString);
                        con.Open();
                    }

                }
            }


            //get all the available sheets  
            System.Data.DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //get the number of sheets in the file  
            //string[] workSheetNames = new String[dt.Rows.Count];
            List<string> workSheetNames = new List<string>();
            int i = 0;
            DataTable dtResult = null;
            foreach (DataRow row in dt.Rows)
            {
                //insert the sheet's name in the current element of the array  
                //and remove the $ sign at the end  
                //workSheetNames[i] = row["TABLE_NAME"].ToString().Trim(new[] { '$' });
                string strTempWSName = row["TABLE_NAME"].ToString();

                if (row["TABLE_NAME"].ToString().Substring(0, 1) == "'")
                {
                    strTempWSName = strTempWSName.Substring(1, strTempWSName.Length - 1);
                }

                if (row["TABLE_NAME"].ToString().Substring(row["TABLE_NAME"].ToString().Length - 1, 1) == "'")
                {
                    strTempWSName = strTempWSName.Substring(0, strTempWSName.Length - 1);
                }

                //workSheetNames[i] = strTempWSName.Replace("$", "").Trim();
                try
                {
                    DataTable dtTempSheet = GetWorksheet(strTempWSName.Replace("$", "").Trim(), connectionString,false);
                    if (dtTempSheet != null)
                    {
                        if (dtTempSheet.Columns.Count > 1)
                        {
                            workSheetNames.Add(strTempWSName.Replace("$", "").Trim());
                            return workSheetNames;
                        }
                    }

                }
                catch
                {
                }

            }

            return workSheetNames;

        }
        catch (Exception ex)
        {
            return null;

        }

    }




    public static System.Data.DataTable GetImportFileTableFromXLSX(string strImportFolder, string strFileUniqueName, string strSelectedSheet, bool RemoveF)
    {
       
        try
        {

            string fileName = strImportFolder + "\\" + strFileUniqueName;
           
             //open the excel file using OLEDB  
                OleDbConnection con;
                string connectionString ;
               

                //string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", fileName);

                try
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileName + ";Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text;\"";
                    con = new OleDbConnection(connectionString);
                    con.Open();
                }
                catch (Exception ex)
                {
                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileName + ";Extended Properties=\"Excel 12.0;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text;\"";
                        con = new OleDbConnection(connectionString);
                        con.Open();
                    }
                    catch (Exception ex2)
                    {

                        try
                        {
                            //connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", fileName);
                            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileName + ";Extended Properties=\"HTML Import;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text;\"";
                            con = new OleDbConnection(connectionString);
                            con.Open();
                        }
                        catch (Exception ex3)
                        {
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", fileName);
                            con = new OleDbConnection(connectionString);
                            con.Open();
                        }
                       
                    }
                }
            
           

                //get all the available sheets  
                System.Data.DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //get the number of sheets in the file  
                string[] workSheetNames = new String[dt.Rows.Count];
                int i = 0;
                DataTable dtResult = null;
                foreach (DataRow row in dt.Rows)
                {
                    //insert the sheet's name in the current element of the array  
                    //and remove the $ sign at the end  
                    //workSheetNames[i] = row["TABLE_NAME"].ToString().Trim(new[] { '$' });
                    string strTempWSName = row["TABLE_NAME"].ToString();

                    if (row["TABLE_NAME"].ToString().Substring(0, 1) == "'")
                    {
                        strTempWSName = strTempWSName.Substring(1, strTempWSName.Length - 1);
                    }

                    if (row["TABLE_NAME"].ToString().Substring(row["TABLE_NAME"].ToString().Length - 1, 1) == "'")
                    {
                        strTempWSName = strTempWSName.Substring(0, strTempWSName.Length - 1);
                    }

                    //workSheetNames[i] = strTempWSName.Replace("$", "").Trim();
                    workSheetNames[i] = strTempWSName;
                    if (strSelectedSheet == "")
                    {
                        try
                        {
                            dtResult = GetWorksheet2(workSheetNames[i], connectionString, RemoveF);
                        }
                        catch
                        {
                            dtResult = GetWorksheet(workSheetNames[i], connectionString, RemoveF);
                        }
                    }
                    else
                    {
                        try
                        {
                            dtResult = GetWorksheet2(strSelectedSheet, connectionString, RemoveF);
                        }
                        catch
                        {

                            dtResult = GetWorksheet(strSelectedSheet, connectionString, RemoveF);
                        }
                    }
                    break;
                }

                return dtResult;
            

        }
        catch (Exception ex)
        {
            return null;
           
        }
       
    }

    public static System.Data.DataTable GetWorksheet(string worksheetName, string connectionString, bool RemoveF)
    {
         
        OleDbConnection con = new System.Data.OleDb.OleDbConnection(connectionString);
        OleDbDataAdapter cmd = new System.Data.OleDb.OleDbDataAdapter(
            "select * from [" + worksheetName + "$]", con);

        con.Open();
        System.Data.DataSet excelDataSet = new DataSet();
        cmd.Fill(excelDataSet);
        con.Close();

        DataTable dtTable = excelDataSet.Tables[0];

        if (RemoveF)
        {
            int iCount = dtTable.Columns.Count - 1;

            for (int i = iCount; i >= 0; i--)
            {
                if (dtTable.Columns[i].ColumnName.Substring(0, 1) == "F")
                {
                    try
                    {
                        //int j = int.Parse(dtTable.Columns[i].ColumnName.Replace("F", "").ToString());
                        //dtTable.Columns.RemoveAt(i);

                    }
                    catch
                    {
                        //
                    }
                }
            }
        }
        

        //foreach (DataColumn dc in dtTable.Columns)
        //{
        //    if (dc.ColumnName.Substring(0, 1) == "F")
        //    {
        //        try
        //        {
        //           int i=int.Parse( dc.ColumnName.Replace("F","").ToString());
        //           dtTable.Columns.Remove(dc);
        //        }
        //        catch 
        //        {
        //            //
        //        }
        //    }
        //}
        dtTable.AcceptChanges();

        return dtTable;
        //return excelDataSet.Tables[0];
    }


    public static System.Data.DataTable GetWorksheet2(string worksheetName, string connectionString, bool RemoveF)
    {
        OleDbConnection con = new System.Data.OleDb.OleDbConnection(connectionString);
        OleDbDataAdapter cmd = new System.Data.OleDb.OleDbDataAdapter(
            "select * from [" + worksheetName + "]", con);

        con.Open();
        System.Data.DataSet excelDataSet = new DataSet();
        cmd.Fill(excelDataSet);
        con.Close();

        DataTable dtTable = excelDataSet.Tables[0];

        if (RemoveF)
        {
            int iCount = dtTable.Columns.Count - 1;

            for (int i = iCount; i >= 0; i--)
            {

                if (dtTable.Columns[i].ColumnName.Substring(0, 1) == "F")
                {
                    try
                    {
                        int j = int.Parse(dtTable.Columns[i].ColumnName.Replace("F", "").ToString());
                        dtTable.Columns.RemoveAt(i);
                    }
                    catch
                    {
                        //
                    }
                }

            }
        }
        


        //foreach (DataColumn dc in dtTable.Columns)
        //{
        //    if (dc.ColumnName.Substring(0, 1) == "F")
        //    {
        //        try
        //        {
        //            int i = int.Parse(dc.ColumnName.Replace("F", "").ToString());
        //            dtTable.Columns.Remove(dc);
        //        }
        //        catch
        //        {
        //            //
        //        }
        //    }
        //}
        dtTable.AcceptChanges();

        return dtTable;

        //return excelDataSet.Tables[0];
    }  


    public static void CreateWorkbook(DataSet ds, String path)
{
   XmlDataDocument xmlDataDoc = new XmlDataDocument(ds);
   XslTransform xt = new XslTransform();
   StreamReader reader = new StreamReader(typeof(WorkbookEngine).Assembly.GetManifestResourceStream(typeof(WorkbookEngine), "Excel.xsl"));
   //StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("[YourNameSpace].Resources.Excel.xls"));
   //StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Resources.Excel.xsl"));

   XmlTextReader xRdr = new XmlTextReader(reader);
   xt.Load(xRdr, null, null);

   StringWriter sw = new StringWriter();
   xt.Transform(xmlDataDoc, null, sw, null);

   StreamWriter myWriter = new StreamWriter (path + "\\Report.xls");
   myWriter.Write (sw.ToString());
   myWriter.Close ();
}

   
    //public static System.Data.DataTable GetImportFileTableFromXLSX(string strImportFolder, string strFileUniqueName)
    //{
    //    Application oXL;
    //    Workbook oWB;
    //    Worksheet oSheet;
    //    Range oRng;
    //    try
    //    {
    //        //  creat a Application object
    //        oXL = new ApplicationClass();
    //        //   get   WorkBook  object

    //        string fileName = strImportFolder + "\\" + strFileUniqueName;
    //        oWB = oXL.Workbooks.Open(fileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
    //                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
    //                Missing.Value, Missing.Value);

    //        //   get   WorkSheet object 
    //        oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Sheets[1];
    //        System.Data.DataTable dt = new System.Data.DataTable("dtExcel");
    //        DataSet ds = new DataSet();
    //        ds.Tables.Add(dt);
    //        DataRow dr;

    //        StringBuilder sb = new StringBuilder();
    //        int jValue = oSheet.UsedRange.Cells.Columns.Count;
    //        int iValue = oSheet.UsedRange.Cells.Rows.Count;
    //        //  get data columns
    //        //for (int j = 1; j <= jValue; j++)
    //        //{
    //        //    dt.Columns.Add("column" + j, System.Type.GetType("System.String"));
    //        //}

    //        for (int j = 1; j <= jValue; j++)
    //        {
    //            oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[1, j];
    //            string strValue = oRng.Text.ToString();              
    //            dt.Columns.Add(strValue, System.Type.GetType("System.String"));
    //        }


    //        //string colString = sb.ToString().Trim();
    //        //string[] colArray = colString.Split(':');

    //        //  get data in cell
    //        for (int i = 2; i <= iValue; i++)
    //        {
    //            dr = ds.Tables["dtExcel"].NewRow();
    //            for (int j = 1; j <= jValue; j++)
    //            {
    //                oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[i, j];
    //                string strValue = oRng.Text.ToString();
    //                dr[j-1] = strValue;
    //            }
    //            ds.Tables["dtExcel"].Rows.Add(dr);
    //        }

    //        oXL = null;
    //        oWB = null;
    //        oSheet = null;

    //        return ds.Tables[0];
    //    }
    //    catch (Exception ex)
    //    {
    //        oXL = null;
    //        oWB = null;
    //        oSheet = null;
    //        return null;
    //    }
    //    finally
    //    {
           
    //    }
    //}

}
public class WorkbookEngine
{

    public static string WriteToExcel(DataSet ds)
    {
        XmlDataDocument xmlDataDoc = new XmlDataDocument(ds);
        XslTransform xt = new XslTransform();
        StreamReader reader = new StreamReader(typeof(WorkbookEngine).Assembly.GetManifestResourceStream(typeof(WorkbookEngine), "Excel.xsl"));
        XmlTextReader xRdr = new XmlTextReader(reader);

        xt.Load(xRdr, null, null);
        StringWriter sw = new StringWriter();
        xt.Transform(xmlDataDoc, null, sw, null);

        return sw.ToString();
    }
}
//namespace ExcelUtil
//{
//    public class WorkbookEngine
//    {

//        public static string WriteToExcel(DataSet ds)
//        {
//            XmlDataDocument xmlDataDoc = new XmlDataDocument(ds);
//            XslTransform xt = new XslTransform();
//            StreamReader reader = new StreamReader(typeof(WorkbookEngine).Assembly.GetManifestResourceStream(typeof(WorkbookEngine), "Excel.xsl"));
//            XmlTextReader xRdr = new XmlTextReader(reader);

//            xt.Load(xRdr, null, null);
//            StringWriter sw = new StringWriter();
//            xt.Transform(xmlDataDoc, null, sw, null);

//            return sw.ToString();
//        }
//    }
//}
