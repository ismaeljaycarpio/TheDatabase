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
/// Summary description for UploadRecordManager
/// </summary>
public class UploadManager
{
    public UploadManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void UploadCSV(int? iUserID, Table theTable, string strBatchDescription,
     string strOriginalFileName, Guid? guidNew, string strImportFolder, out string strMsg, out int iBatchID,
        string strFileExtension, string strSelectedSheet, int? iAccountID,
        bool? bAllowDataUpload, int? iImportTemplateID, int? iSourceBatchID)
    {
        //try
        //{
        //    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-GB");
           
        //}
        //catch
        //{

        //}
       


        string strDateRecordedColumnName = "Date Recorded";
        string strTimeSamledColumnName = "Time Recorded";
        string strDBName = Common.GetDatabaseName();
        iBatchID = -1;
        strMsg = "";


        try
        {
            if (theTable != null && iAccountID != null && theTable.AccountID != iAccountID)
                iAccountID = theTable.AccountID;

            string strNameOnImport = "NameOnImport";
            Batch theSourceBatch = null;
            if (iSourceBatchID != null)
            {
                theSourceBatch = UploadManager.ets_Batch_Details((int)iSourceBatchID);
                if (theSourceBatch == null)
                {
                    return;
                }

                //strNameOnImport = "DisplayName";

                if (theTable == null)
                    theTable = RecordManager.ets_Table_Details((int)theSourceBatch.TableID);

              
                if (iUserID == null)
                    iUserID = theSourceBatch.UserIDUploaded;

                if (iAccountID == null && theTable != null)
                    iAccountID = theTable.AccountID;

                if (iAccountID == null)
                    iAccountID = theSourceBatch.AccountID;
            }

            int? iAutoUserID = int.Parse(SystemData.SystemOption_ValueByKey_Account("AutoUploadUserID", null, null));
            if (strFileExtension.ToLower()!="virtual" && iImportTemplateID == null && theTable.DefaultImportTemplateID != null
                && (iSourceBatchID != null || iUserID == iAutoUserID)) 
            {
                iImportTemplateID = theTable.DefaultImportTemplateID;

            }

            //if (strFileExtension.ToLower()=="virtual" && iImportTemplateID == null
            //    && theTable.DefaultImportTemplateID != null && strImportFolder!="") //ALS Labdata only
            //{
            //    iImportTemplateID = theTable.DefaultImportTemplateID;
            //}



            bool bCheckIgnoreMidnight = false;
            string strIgnoreMidnight = SystemData.SystemOption_ValueByKey_Account("Time Calculation Ignore Midnight", (int)theTable.AccountID, theTable.TableID);

            if (strIgnoreMidnight != "" && strIgnoreMidnight.ToString().ToLower() == "yes")
            {
                bCheckIgnoreMidnight = true;
            }


            ImportTemplate theImportTemplate = null;
            string strUniqueColumnIDSys = "";
            string strUniqueColumnID2Sys = "";

            if (theTable.UniqueColumnID != null)
                strUniqueColumnIDSys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + theTable.UniqueColumnID.ToString());

            if (theTable.UniqueColumnID2 != null)
                strUniqueColumnID2Sys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + theTable.UniqueColumnID2.ToString());


            string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", theTable.AccountID, theTable.TableID);
            bool bShowExceedances = false;

            if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
            {
                bShowExceedances = true;
            }

            if (iImportTemplateID != null)
                theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)iImportTemplateID);

            if (theImportTemplate != null)
            {
                theTable.ImportColumnHeaderRow = theImportTemplate.ImportColumnHeaderRow;
                theTable.ImportDataStartRow = theImportTemplate.ImportDataStartRow;
            }

            //lets get date and time column name
            DataTable dtDateTimeColumnName = null;

           if(iImportTemplateID!=null)
           {
               dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      ITI.ImportHeaderName FROM ImportTemplateItem ITI INNER JOIN [Column] C
                            ON ITI.ColumnID=C.ColumnID WHERE ITI.ImportTemplateID="+iImportTemplateID.ToString()+@"                   
                    AND C.SystemName='DateTimeRecorded' AND C.TableID=" + theTable.TableID.ToString());               

           }

           if (dtDateTimeColumnName==null)
           {
               dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      NameOnImport 
                    FROM  [Column]
                    WHERE SystemName='DateTimeRecorded' AND TableID=" + theTable.TableID.ToString());

           }          
          



            if (dtDateTimeColumnName.Rows.Count > 0 && dtDateTimeColumnName.Rows[0][0] != DBNull.Value)
            {
                string strDT = dtDateTimeColumnName.Rows[0][0].ToString();
                if (strDT.IndexOf(",") > 0)
                {
                    strDateRecordedColumnName = strDT.Substring(0, strDT.IndexOf(","));
                    strTimeSamledColumnName = strDT.Substring(strDT.IndexOf(",") + 1);
                }
                else
                {
                    strDateRecordedColumnName = strDT;
                    strTimeSamledColumnName = "";
                }
            }

            DataTable dtImportFileTable;



            dtImportFileTable = null;




            string strTemp = "";

            int z = 0;


            if (iSourceBatchID == null)
            {

                string strFileUniqueName = guidNew.ToString() + strFileExtension;

                if (theTable.TempImportColumnHeaderRow != null)
                    theTable.ImportColumnHeaderRow = theTable.TempImportColumnHeaderRow;

                if (theTable.TempImportDataStartRow != null)
                    theTable.ImportDataStartRow = theTable.TempImportDataStartRow;

                switch (strFileExtension.ToLower())
                {
                    case ".dbf":
                        dtImportFileTable = UploadManager.GetImportFileTableFromDBF(strImportFolder, strFileUniqueName, ref strMsg);
                        z = 0;
                        break;

                    case ".txt":
                        dtImportFileTable = UploadManager.GetImportFileTableFromText(strImportFolder, strFileUniqueName, ref strMsg);
                        z = 0;
                        break;
                    case ".csv":
                        dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName, ref strMsg);
                        z = 0;
                        break;
                    case ".xls":
                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
                        break;
                    case ".xlsx":
                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
                        break;

                    case ".xml":
                        dtImportFileTable = UploadManager.GetImportFileTableFromXML(strImportFolder, strFileUniqueName);
                        strNameOnImport = "DisplayName";

                        break;

                    case "virtual":
                        dtImportFileTable = UploadManager.GetVirtualImportFileTable(BitConverter.ToInt32(((Guid)guidNew).ToByteArray(), 8),
                            theTable.TableID.Value,
                            BitConverter.ToInt32(((Guid)guidNew).ToByteArray(), 0));
                        theTable.ImportColumnHeaderRow = 1;
                        theTable.ImportDataStartRow = 2;

                        if (theImportTemplate != null)
                        {

                            theImportTemplate.ImportColumnHeaderRow = null;
                            theImportTemplate.ImportDataStartRow = null;
                        }
                          
                        
                        


                        break;
                }

                if (strMsg != "")
                {

                    return;

                }

                //PERFORM CLIENT Specific treatment




                if (strFileExtension.ToLower() != ".dbf")
                {
                    if (theTable.ImportColumnHeaderRow == null)
                        theTable.ImportColumnHeaderRow = 1;
                    if (theTable.ImportDataStartRow == null)
                        theTable.ImportDataStartRow = 2;

                    if (theTable.ImportColumnHeaderRow != null)
                    {
                        //if ((int)theTable.ImportColumnHeaderRow > 1)
                        //{
                        if (dtImportFileTable.Rows.Count >= (int)theTable.ImportColumnHeaderRow)
                        {
                            for (int i = 0; i <= dtImportFileTable.Columns.Count - 1; i++)
                            {
                                if (dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() == "")
                                {
                                    //do nothing for it
                                    if (strFileExtension.ToLower() == ".csv")
                                    {
                                        try
                                        {
                                            dtImportFileTable.Columns[i].ColumnName = "Column" + (i + 1).ToString();
                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.Message.IndexOf("already belongs to this DataTable") > -1)
                                        {
                                            for (int j = 1; j < 20; j++)
                                            {
                                                bool bOK = true;
                                                foreach (DataColumn dc in dtImportFileTable.Columns)
                                                {
                                                    if (dc.ColumnName == dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() + j.ToString())
                                                    {
                                                        bOK = false;
                                                    }
                                                }

                                                if (bOK)
                                                {
                                                    dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() + j.ToString();
                                                    dtImportFileTable.AcceptChanges();
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                }

                            }
                            dtImportFileTable.AcceptChanges();
                        }
                        //}

                    }



                    if (theTable.ImportDataStartRow != null)
                    {
                        for (int i = 1; i <= (int)theTable.ImportDataStartRow - 1; i++)
                        {
                            dtImportFileTable.Rows.RemoveAt(0);

                        }
                        dtImportFileTable.AcceptChanges();
                    }

                    int xy = 0;
                    foreach (DataColumn dc in dtImportFileTable.Columns)
                    {
                        dtImportFileTable.Columns[xy].ColumnName = Common.RemoveSpecialCharacters(dc.ColumnName);
                        xy = xy + 1;
                    }
                    dtImportFileTable.AcceptChanges();
                }
            }


           


            if(strFileExtension.ToLower()=="virtual" && theImportTemplate!=null)
            {
                DataTable dtIT_Columns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, iImportTemplateID);
                DataTable dtNoI_Columns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, null);


                for (int i = 0; i <= dtImportFileTable.Columns.Count - 1; i++)
                {
                    foreach (DataRow drNoI in dtNoI_Columns.Rows)
                    {

                        if (dtImportFileTable.Columns[i].ColumnName.ToLower() ==
                           drNoI["NameOnImport"].ToString().ToLower())
                        {
                            foreach (DataRow drIT in dtIT_Columns.Rows)
                            {
                                if (int.Parse(drNoI["ColumnID"].ToString()) == int.Parse(drIT["ColumnID"].ToString()))
                                {
                                    dtImportFileTable.Columns[i].ColumnName = drIT["NameOnImport"].ToString();
                                    dtImportFileTable.AcceptChanges();
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                           
            }




            DataTable dtRecordTypleColumns;
            DataTable dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theTable.TableID);



            string strListOfNoNeedColumns = "";
            bool bHasValidationOnEntry = false;
            bool bHasValidationOnWarning = false;
            bool bHasCheckUnlikelyValue = false;

            bool bNeedAcceptChange = false;
            foreach (DataRow drC in dtColumnsAll.Rows)
            {
                if (drC["TextType"] != DBNull.Value &&
                    (drC["TextType"].ToString() == "d" || drC["TextType"].ToString() == "t"))
                {

                }
                else
                {
                    bNeedAcceptChange = true;
                    drC["Calculation"] = Common.GetCalculationSystemNameOnly(drC["Calculation"].ToString(), (int)theTable.TableID);
                }

                if ((drC["ValidationOnEntry"] != DBNull.Value && drC["ValidationOnEntry"].ToString().Length > 0)
                    || drC["ConV"] != DBNull.Value)
                {
                    bHasValidationOnEntry = true;
                }
                if ((drC["ValidationOnWarning"] != DBNull.Value && drC["ValidationOnWarning"].ToString().Length > 0)
                    || drC["ConW"] != DBNull.Value)
                {
                    bHasValidationOnWarning = true;
                }
                if (drC["CheckUnlikelyValue"] != DBNull.Value && bool.Parse(drC["CheckUnlikelyValue"].ToString()))
                {
                    bHasCheckUnlikelyValue = true;
                }


            }


            if (bNeedAcceptChange)
                dtColumnsAll.AcceptChanges();



            if (iImportTemplateID != null)
            {
                strNameOnImport = "NameOnImport";
                dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, iImportTemplateID);
            }
            else if (strNameOnImport == "DisplayName")
            {
                dtRecordTypleColumns = RecordManager.ets_Table_Columns_DisplayName((int)theTable.TableID);
            }
            else
            {
                dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, null);
            }

          


            if (iSourceBatchID == null)
            {
                if (dtImportFileTable.Rows.Count > 0)
                {
                    try
                    {
                        dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare(field.ToString().Trim(), string.Empty) == 0)).CopyToDataTable();
                    }
                    catch
                    {
                        //
                    }

                }

                for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
                {
                    bool bIsFound = false;
                    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
                    {
                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
                            Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
                        {
                            bIsFound = true;
                            break;
                        }
                    }
                    if (bIsFound == false)
                    {
                        if (dtImportFileTable.Columns[r].ColumnName.ToLower() != strTimeSamledColumnName.ToLower() && dtImportFileTable.Columns[r].ColumnName.ToLower() != strDateRecordedColumnName.ToLower())
                        {
                            strListOfNoNeedColumns += dtImportFileTable.Columns[r].ColumnName + ",";
                        }
                    }
                }

                if (strFileExtension == ".txt")
                {
                    strListOfNoNeedColumns = "";
                }

                List<string> strRemoveIndexes = strListOfNoNeedColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();


                foreach (string item in strRemoveIndexes)
                {
                    try
                    {
                        dtImportFileTable.Columns.Remove(item);
                    }
                    catch
                    {
                        //
                    }
                }


                string strListOfMissingColumns = "";
                for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
                {


                    bool bMissingColumnFound = false;

                    for (int ic = 0; ic < dtImportFileTable.Columns.Count; ic++)
                    {
                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[ic].ColumnName.Trim().ToLower()) ==
                    Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
                        {
                            bMissingColumnFound = true;
                            break;
                        }

                    }

                    if (bMissingColumnFound == false)
                    {
                        strListOfMissingColumns += dtRecordTypleColumns.Rows[i][strNameOnImport].ToString() + ",";
                    }


                }

                if (strFileExtension == ".txt")
                {
                    strListOfMissingColumns = "";
                }
                if (strListOfMissingColumns.Length > 0)
                {
                    List<string> strMissingColumns = strListOfMissingColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();
                    foreach (string item in strMissingColumns)
                    {
                        try
                        {
                            dtImportFileTable.Columns.Add(item);
                        }
                        catch
                        {
                            //
                        }
                    }

                }

                dtImportFileTable.AcceptChanges();



                for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
                {
                    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
                    {
                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
                            Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
                        {
                            try
                            {
                                dtImportFileTable.Columns[r].ColumnName = dtRecordTypleColumns.Rows[i]["SystemName"].ToString();
                                break;
                            }
                            catch
                            {
                                //
                            }
                        }
                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
                          strDateRecordedColumnName.ToLower())
                        {
                            dtImportFileTable.Columns[r].ColumnName = "DateTimeRecorded";
                            break;
                        }
                    }
                }

            }


            if (theSourceBatch != null)
            {

                int iTN_Temp = 0;

                dtImportFileTable = ets_TempRecord_List((int)theSourceBatch.TableID, (int)theSourceBatch.BatchID, null, null, null, null, null, "", "", null, null,
                    ref iTN_Temp, ref iTN_Temp, "", null, "system");

                if (dtImportFileTable == null)
                    return;

                if (dtImportFileTable.Rows.Count == 0)
                    return;

                strBatchDescription = theSourceBatch.BatchDescription;
                strOriginalFileName = "NA";
                if (guidNew == null)
                    guidNew = Guid.NewGuid(); //may be we can remove this
            }


            //now dtCSV is ready to be imported into Batch & TempRecord

            Batch newBatch = new Batch(null, (int)theTable.TableID,
                strBatchDescription.Trim() == "" ? strOriginalFileName : strBatchDescription.Trim(),
                strOriginalFileName, null, guidNew, iUserID, theTable.AccountID, theTable.IsImportPositional);


            if (bAllowDataUpload.HasValue)
            {
                newBatch.AllowDataUpdate = bAllowDataUpload;
            }
            //else
            //{
            //    if (Common.GetDatabaseName() == "thedatabase_emd")
            //    {
            //        newBatch.AllowDataUpdate = theTable.IsDataUpdateAllowed;
            //    }

            //}
                

            newBatch.ImportTemplateID = iImportTemplateID;
            iBatchID = UploadManager.ets_Batch_Insert(newBatch);

            //bool bHasParentMatching = false;
            bool bHasParent = false;
            string strHasParent = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE TableID=" + theTable.TableID.ToString() + " AND TableTableID  is not NULL AND DisplayColumn is not NULL AND ColumnType='dropdown' ");

            bNeedAcceptChange = false;
            if (strHasParent != "")
            {
                bHasParent = true;
                dtRecordTypleColumns.Columns.Add("ParentColumnSystemName", typeof(String));
                dtRecordTypleColumns.AcceptChanges();
                foreach (DataRow drC in dtRecordTypleColumns.Rows)
                {
                    if ((drC["ColumnType"].ToString() == "dropdown") &&
                            (drC["DropDownType"].ToString() == "tabledd" || drC["DropDownType"].ToString() == "table") &&
                            (drC["TableTableID"] != DBNull.Value) && (drC["DisplayColumn"] != DBNull.Value))
                    {
                        bool bParentImportColumnID = false;
                        if (iImportTemplateID != null)
                        {
                            string strParentImportColumnID = Common.GetValueFromSQL("SELECT ParentImportColumnID FROM ImportTemplateItem WHERE ImportTemplateID="
                                + iImportTemplateID.ToString() + " AND ParentImportColumnID IS NOT NULL AND ColumnID=" + drC["ColumnID"].ToString());
                            if (strParentImportColumnID != "" && strParentImportColumnID.Length > 0)
                            {
                                bParentImportColumnID = true;
                            }
                        }

                        // || theTable.TableName.IndexOf("EMD_Test") > -1
                        if (bParentImportColumnID == false)// when bParentImportColumnID==true we use  [dbo].[spAdjustTempRecordLinkedValueOnImport]
                        {
                            string lookupColumnName = drC["DisplayColumn"].ToString();

                            string lookupSystemName = "";                           
                            if (strFileExtension.ToLower() == "virtual" && strImportFolder!="")
                            {
                                if (lookupColumnName == "[Site Name]")
                                {
                                    lookupSystemName = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE TableID=" + drC["TableTableID"].ToString()
                                        + " AND (DisplayName='Site Name On Import File' OR NameOnImport='Site Name On Import File')");
                                }                                   
                            }

                           if(lookupSystemName.Trim()=="")
                           {
                               lookupSystemName = Common.GetValueFromSQL("SELECT dbo.fnReplaceDisplayColumns_NoAlias(" + drC["ColumnID"].ToString() + ")");
                           }

                            if (lookupSystemName != "")
                            {
                                drC["ParentColumnSystemName"] = lookupSystemName;
                                bNeedAcceptChange = true;
                            }
                        }

                    }
                }

            }







            if (bNeedAcceptChange)
                dtRecordTypleColumns.AcceptChanges();


            for (int r = z; r < dtImportFileTable.Rows.Count; r++)
            {
                TempRecord newTempRecord = new TempRecord();
                newTempRecord.AccountID = theTable.AccountID;
                newTempRecord.BatchID = iBatchID;
                newTempRecord.TableID = (int)theTable.TableID;
                //bool bIsBlank = false;
                string strRejectReason = "";
                string strWarningReason = "";
                string strExceedanceReason = "";


                foreach (DataColumn dc in dtImportFileTable.Columns)
                {
                    string strColumnName = "";
                    strColumnName = dc.ColumnName;
                    if (strColumnName.ToLower() != strTimeSamledColumnName.ToLower())
                    {


                        if (dc.ColumnName.ToUpper() == "DATETIMERECORDED")
                        {
                            newTempRecord.DateFormat = theTable.DateFormat;
                            try
                            {
                                if (strFileExtension == ".csv")
                                {
                                    if (strTimeSamledColumnName == "")
                                    {
                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
                                    }
                                    else
                                    {

                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() + " " + dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString());
                                    }

                                }
                                else if (strFileExtension == ".xml" || iSourceBatchID != null)
                                {
                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
                                }
                                else
                                {
                                    string strDateTimeTemp = "";
                                    if (strTimeSamledColumnName == "")
                                    {
                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0));
                                    }
                                    else
                                    {
                                        strDateTimeTemp = dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString();

                                        if (dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString().Length > 10)
                                        {
                                            strDateTimeTemp = strDateTimeTemp.Substring(11);
                                        }
                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
                                        {
                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ") != -1)
                                            {
                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ")) + " " + strDateTimeTemp);
                                            }
                                            else
                                            {
                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0) + " " + strDateTimeTemp);
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                //if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
                                //    strRejectReason = strRejectReason + " Invalid Date Recorded.";
                                newTempRecord.DateTimeRecorded = DateTime.Now;
                            }

                        }
                        else
                        {
                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
                        }


                        for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
                        {
                            if (dc.ColumnName.ToLower() ==
                                dtRecordTypleColumns.Rows[i]["SystemName"].ToString().Trim().ToLower())
                            {

                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length == 0)
                                {
                                    if (dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString() != "")
                                    {
                                        if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "datetime"
                                            || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "date"
                                            || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "time")
                                        {
                                            dtImportFileTable.Rows[r][dc.ColumnName] = DateTime.Now;

                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, DateTime.Now);
                                        }
                                        else
                                        {

                                            dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());
                                        }
                                        dtImportFileTable.AcceptChanges();
                                    }

                                }



                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 0)
                                {



                                    if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "datetime"
                                        || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "date")
                                    {
                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
                                        {
                                            try
                                            {
                                                DateTime dtt = Convert.ToDateTime(dtImportFileTable.Rows[r][dc.ColumnName].ToString());
                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtt.ToString("dd/MM/yyyy HH:mm:ss"));
                                            }
                                            catch
                                            {
                                                strRejectReason = strRejectReason +  TheDatabase.GetInvalid_msg(dtRecordTypleColumns.Rows[i]["DisplayName"].ToString());
                                            }
                                        }
                                    }

                                    if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "time")
                                    {
                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
                                        {
                                            try
                                            {
                                                Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + dtImportFileTable.Rows[r][dc.ColumnName].ToString());
                                            }
                                            catch
                                            {
                                                strRejectReason = strRejectReason +TheDatabase.GetInvalid_msg(dtRecordTypleColumns.Rows[i]["DisplayName"].ToString());
                                            }
                                        }
                                    }

                                    if (bHasParent == true && (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "dropdown") &&
                                        (dtRecordTypleColumns.Rows[i]["DropDownType"].ToString() == "tabledd"
                                        || dtRecordTypleColumns.Rows[i]["DropDownType"].ToString() == "table") &&
                                        (dtRecordTypleColumns.Rows[i]["TableTableID"] != DBNull.Value) &&
                                        (dtRecordTypleColumns.Rows[i]["DisplayColumn"] != DBNull.Value) && (dtRecordTypleColumns.Columns.Contains("ParentColumnSystemName")
                                        && dtRecordTypleColumns.Rows[i]["ParentColumnSystemName"] != DBNull.Value && dtRecordTypleColumns.Rows[i]["ParentColumnSystemName"].ToString().Length > 0))
                                    {
                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
                                        {

                                            string strRecordIDSQL = "SELECT RecordID FROM [Record] WHERE IsActive=1 AND TableID=" + dtRecordTypleColumns.Rows[i]["TableTableID"]
                                                 + " AND CHARINDEX (';' +'" + dtImportFileTable.Rows[r][dc.ColumnName].ToString().Replace("'", "''").Trim() 
                                                                     + "'+ ';',';' + " 
                                                                     + dtRecordTypleColumns.Rows[i]["ParentColumnSystemName"] + " + ';')>0";
                                                
                                            
                                            //+ " AND " + dtRecordTypleColumns.Rows[i]["ParentColumnSystemName"] + "='" + dtImportFileTable.Rows[r][dc.ColumnName].ToString() + "'";
                                                                                       

                                            string strParentRecordID = Common.GetValueFromSQL(strRecordIDSQL);
                                            if (strParentRecordID != "")
                                            {
                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, strParentRecordID);
                                            }
                                            else
                                            {
                                                strRejectReason = strRejectReason + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + "(" + dtImportFileTable.Rows[r][dc.ColumnName].ToString() + ") not found.";
                                            }
                                        }
                                    }                               
                                }

                                if (dtRecordTypleColumns.Rows[i]["Importance"].ToString().ToLower() == "m")
                                {
                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
                                    {
                                        strRejectReason = strRejectReason + "MANDATORY:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + ".";

                                    }

                                }

                                break;
                            }// END of dc.ColumnName.ToLower() ===dtRecordTypleColumns.Rows[i]["SystemName"].ToString().Trim().ToLower()
                        }

                    }
                }


                if (newTempRecord.DateTimeRecorded == null)
                {
                    newTempRecord.DateTimeRecorded = DateTime.Now;

                }

                bool bIsAnyCalculationField = false;

                for (int i = 0; i < dtColumnsAll.Rows.Count; i++)
                {

                    if (dtColumnsAll.Rows[i]["ColumnType"].ToString() == "calculation"
                                               && dtColumnsAll.Rows[i]["Calculation"] != DBNull.Value
                                               && dtColumnsAll.Rows[i]["Calculation"].ToString() != "")
                    {
                        bIsAnyCalculationField = true;

                    }

                    if (dtColumnsAll.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "number")
                    {
                        if (dtColumnsAll.Rows[i]["NumberType"] != null &&
                            dtColumnsAll.Rows[i]["NumberType"].ToString() == "8")
                        {
                            string strValue = "1";
                            try
                            {
                                string strMax = "";

                                if (r == z)
                                {
                                    strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")) FROM Record WHERE IsNumeric(" + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")=1 and TableID=" + theTable.TableID.ToString());
                                }
                                else
                                {
                                    strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")) FROM TempRecord WHERE IsNumeric(" + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")=1 and  TableID=" + theTable.TableID.ToString());
                                }
                                if (strMax == "")
                                {
                                    strValue = "1";
                                }
                                else
                                {
                                    strValue = (int.Parse(strMax) + 1).ToString();
                                }
                            }
                            catch
                            {
                                strValue = "1";
                            }
                            UploadManager.MakeTheTempRecord(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString(), strValue);

                        }
                        //update constant field 
                        if (dtColumnsAll.Rows[i]["Constant"] != DBNull.Value && dtColumnsAll.Rows[i]["Constant"].ToString() != ""
                        && dtColumnsAll.Rows[i]["NumberType"] != null && dtColumnsAll.Rows[i]["NumberType"].ToString() == "2")
                        {

                            UploadManager.MakeTheTempRecord(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString(), dtColumnsAll.Rows[i]["Constant"].ToString());

                        }
                    }
                }



                if (bIsAnyCalculationField)
                {
                    for (int i = 0; i < dtColumnsAll.Rows.Count; i++)
                    {
                        //perform calculation for this column (if found)
                        if (dtColumnsAll.Rows[i]["ColumnType"].ToString() == "calculation"
                                                    && dtColumnsAll.Rows[i]["Calculation"] != DBNull.Value
                                                    && dtColumnsAll.Rows[i]["Calculation"].ToString() != "")
                        {
                            string strValue = "";
                            if (dtColumnsAll.Rows[i]["TextType"] != DBNull.Value
                            && dtColumnsAll.Rows[i]["TextType"].ToString().ToLower() == "d")
                            {
                                //datetime calculation
                                string strCalculation = dtColumnsAll.Rows[i]["Calculation"].ToString();

                                try
                                {
                                    strValue = TheDatabaseS.GetDateCalculationResult(ref dtColumnsAll, strCalculation, null, null, null,
                                   dtColumnsAll.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : dtColumnsAll.Rows[i]["DateCalculationType"].ToString(),
                                   newTempRecord, theTable, bCheckIgnoreMidnight);
                                }
                                catch
                                {
                                    //
                                }
                            }
                            else if (dtColumnsAll.Rows[i]["TextType"] != DBNull.Value
                           && dtColumnsAll.Rows[i]["TextType"].ToString().ToLower() == "t")
                            {
                                try
                                {
                                    string strFormula = Common.GetCalculationSystemNameOnly(dtColumnsAll.Rows[i]["Calculation"].ToString(), (int)theTable.TableID);
                                    //string strFormula = Common.GetCalculationSystemNameOnly(_dtColumnsDetail.Rows[i]["Calculation"].ToString(), (int)_theTable.TableID);

                                    Column theColumn = RecordManager.ets_Column_Details(int.Parse(dtColumnsAll.Rows[i]["ColumnID"].ToString()));
                                    strValue = TheDatabaseS.GetTextCalculationResult(ref dtColumnsAll, strFormula, null, null, null,
                                        newTempRecord, theTable, theColumn);
                                }
                                catch
                                {
                                    //
                                }
                            }
                            else
                            {
                                //number calculation
                                try
                                {
                                    //string strFormula = Common.GetCalculationSystemNameOnly(dtColumnsAll.Rows[i]["Calculation"].ToString(), (int)theTable.TableID);
                                    Column theColumn = RecordManager.ets_Column_Details(int.Parse(dtColumnsAll.Rows[i]["ColumnID"].ToString()));
                                    strValue = TheDatabaseS.GetCalculationResult(ref dtColumnsAll, dtColumnsAll.Rows[i]["Calculation"].ToString(), null, null, null,
                                        newTempRecord, theTable, theColumn);
                                }
                                catch
                                {
                                    //
                                }
                            }

                            if (strValue != "")
                            {
                                UploadManager.MakeTheTempRecord(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString(), strValue);
                            }

                        }

                    }

                }

                for (int i = 0; i < dtColumnsAll.Rows.Count; i++)
                {
                    bool bEachColumnExceedance = false;
                    if (bHasValidationOnEntry)
                    {
                        string strValue = UploadManager.GetTempRecordValue(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString());
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            string strFormulaV = "";

                            if (dtColumnsAll.Rows[i]["ConV"] != DBNull.Value)
                            {
                                Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(dtColumnsAll.Rows[i]["ConV"].ToString()));
                                if (theCheckColumn != null)
                                {
                                    string strCheckValue = UploadManager.GetTempRecordValue(ref newTempRecord, theCheckColumn.SystemName);
                                    strFormulaV = UploadWorld.Condition_GetFormula(int.Parse(dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                        "V", strCheckValue);
                                }
                            }
                            else
                            {
                                if (dtColumnsAll.Rows[i]["ValidationOnEntry"] != DBNull.Value && dtColumnsAll.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                                {
                                    strFormulaV = dtColumnsAll.Rows[i]["ValidationOnEntry"].ToString();
                                }
                            }

                            if (strFormulaV != "" && !UploadManager.IsDataValid(strValue, strFormulaV, ref strTemp))
                            {
                                strRejectReason = strRejectReason + TheDatabase.GetInvalid_msg( dtColumnsAll.Rows[i]["DisplayName"].ToString() );
                            }
                        }
                    }

                    if (bShowExceedances)
                    {
                        if (dtColumnsAll.Rows[i]["ValidationOnExceedance"] != DBNull.Value || dtColumnsAll.Rows[i]["ConE"] != DBNull.Value)
                        {
                            string strValue = UploadManager.GetTempRecordValue(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString());
                            if (!string.IsNullOrEmpty(strValue))
                            {
                                string strFormulaE = "";

                                if (dtColumnsAll.Rows[i]["ConE"] != DBNull.Value)
                                {
                                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(dtColumnsAll.Rows[i]["ConE"].ToString()));
                                    if (theCheckColumn != null)
                                    {
                                        string strCheckValue = UploadManager.GetTempRecordValue(ref newTempRecord, theCheckColumn.SystemName);
                                        strFormulaE = UploadWorld.Condition_GetFormula(int.Parse(dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                            "E", strCheckValue);
                                    }
                                }
                                else
                                {
                                    if (dtColumnsAll.Rows[i]["ValidationOnExceedance"] != DBNull.Value && dtColumnsAll.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                                    {
                                        strFormulaE = dtColumnsAll.Rows[i]["ValidationOnExceedance"].ToString();
                                    }
                                }

                                if (strFormulaE != "" && !UploadManager.IsDataValid(strValue, strFormulaE, ref strTemp))
                                {
                                    strExceedanceReason = strExceedanceReason + TheDatabase.GetExceedance_msg(dtColumnsAll.Rows[i]["DisplayName"].ToString());
                                    bEachColumnExceedance = true;
                                }
                            }
                        }
                    }


                    if (bHasValidationOnWarning && bEachColumnExceedance == false)
                    {
                        if (dtColumnsAll.Rows[i]["ValidationOnWarning"] != DBNull.Value || dtColumnsAll.Rows[i]["ConW"] != DBNull.Value)
                        {
                            string strValue = UploadManager.GetTempRecordValue(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString());
                            if (!string.IsNullOrEmpty(strValue))
                            {
                                string strFormulaW = "";

                                if (dtColumnsAll.Rows[i]["ConW"] != DBNull.Value)
                                {
                                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(dtColumnsAll.Rows[i]["ConW"].ToString()));
                                    if (theCheckColumn != null)
                                    {
                                        string strCheckValue = UploadManager.GetTempRecordValue(ref newTempRecord, theCheckColumn.SystemName);
                                        strFormulaW = UploadWorld.Condition_GetFormula(int.Parse(dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                            "W", strCheckValue);
                                    }
                                }
                                else
                                {
                                    if (dtColumnsAll.Rows[i]["ValidationOnWarning"] != DBNull.Value && dtColumnsAll.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                                    {
                                        strFormulaW = dtColumnsAll.Rows[i]["ValidationOnWarning"].ToString();
                                    }
                                }

                                if (strFormulaW != "" && !UploadManager.IsDataValid(strValue, strFormulaW, ref strTemp))
                                {
                                    strWarningReason = strWarningReason + TheDatabase.GetWarning_msg(dtColumnsAll.Rows[i]["DisplayName"].ToString());
                                }
                            }
                        }

                    }

                    //check SD

                    if (bHasCheckUnlikelyValue)
                    {
                        string strData = UploadManager.GetTempRecordValue(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString());
                        if (strData != "" && bool.Parse(dtColumnsAll.Rows[i]["CheckUnlikelyValue"].ToString()))
                        {
                            int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtColumnsAll.Rows[i]["SystemName"].ToString(), -1);

                            if (iCount >= Common.MinSTDEVRecords)
                            {
                                string strRecordedate;
                                if (dtColumnsAll.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
                                {
                                    strRecordedate = Common.IgnoreSymbols(strData);
                                }
                                else
                                {
                                    strRecordedate = strData;
                                }

                                double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtColumnsAll.Rows[i]["SystemName"].ToString(), -1);

                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtColumnsAll.Rows[i]["SystemName"].ToString(), -1);

                                double dRecordedate = double.Parse(strRecordedate);
                                if (dAVG != null && dSTDEV != null)
                                {
                                    dSTDEV = dSTDEV * 3;
                                    if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
                                    {
                                        //deviation happaned
                                        strWarningReason = strWarningReason + TheDatabase.GetWarningUnlikely_msg(dtColumnsAll.Rows[i]["DisplayName"].ToString());
                                    }

                                }
                            }

                        }
                    }




                }


                strRejectReason = strRejectReason.Trim();
                strWarningReason = strWarningReason.Trim();
                strExceedanceReason = strExceedanceReason.Trim();


                if (strRejectReason.Length > 0)
                {
                    newTempRecord.RejectReason = strRejectReason.Trim();
                }

                if (strWarningReason.Length > 0)
                {
                    newTempRecord.WarningReason = strWarningReason.Trim();
                }

                if (strExceedanceReason.Length > 0)
                {
                    newTempRecord.WarningReason = newTempRecord.WarningReason == "" ? strExceedanceReason.Trim() : newTempRecord.WarningReason + " " + strExceedanceReason.Trim();

                }


                int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord);


            }

            if (theImportTemplate != null)
            {
                DataTable dtMatchingField = Common.DataTableFromText("SELECT ColumnID,ParentImportColumnID FROM ImportTemplateItem WHERE ImportTemplateID=" + theImportTemplate.ImportTemplateID.ToString() + " AND ParentImportColumnID IS NOT NULL ORDER BY ColumnIndex");

                if (dtMatchingField != null && dtMatchingField.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMatchingField.Rows)
                    {
                        Column theColumn = RecordManager.ets_Column_Details(int.Parse(dr["ColumnID"].ToString()));
                        if (theColumn != null && theColumn.TableTableID != null)
                        {
                            Column theParentImportColumn = RecordManager.ets_Column_Details(int.Parse(dr["ParentImportColumnID"].ToString()));
                            if (theParentImportColumn != null)
                            {
                                spAdjustTempRecordLinkedValueOnImport(iBatchID, theColumn.SystemName, (int)theColumn.TableTableID, theParentImportColumn.SystemName);
                            }
                        }
                    }
                }
            }


            // if ((strUniqueColumnIDSys != "") && (!theTable.IsDataUpdateAllowed.HasValue || !theTable.IsDataUpdateAllowed.Value))

            if ((strUniqueColumnIDSys != "") && 
                (newBatch.AllowDataUpdate == null || 
                            (newBatch.AllowDataUpdate != null && (bool)newBatch.AllowDataUpdate==false)))
            {
                RecordManager.ets_Batch_Duplicate(iBatchID, strUniqueColumnIDSys, strUniqueColumnID2Sys);
            }



            if (theTable.MaxTimeBetweenRecords != null && theTable.MaxTimeBetweenRecordsUnit != null)
            {
                //NEED JB's HELP
            }

            if (theTable.SPAfterImport != "" && theTable.SPAfterImport.Length > 0)
            {
                SPAfterImport(iBatchID, theTable.SPAfterImport);
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Upload CSV", ex.Message, ex.StackTrace, DateTime.Now, strOriginalFileName);
            SystemData.ErrorLog_Insert(theErrorLog);

            if (ex.Message.IndexOf("DateTime") > -1)
            {
                strMsg = "Date Recorded data are not valid, please review the file data.";
            }
            else if (ex.Message.IndexOf("recognized") > -1)
            {
                strMsg = "Unknown error occurred please review your import data.";
            }
            else if (ex.Message.IndexOf(strTimeSamledColumnName) > -1)
            {
                strMsg = "The file must have a Time Recorded column just after Date Recorded column.";
            }
            else
            {
                strMsg = "UNKNOWN:" + ex.Message + ex.StackTrace;
                //SystemData.ErrorLog_Insert(theErrorLog);
            }

        }


    }




    public static int ets_Batch_Insert(Batch p_Batch)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_Batch_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                if (p_Batch.ImportTemplateID != null)
                    command.Parameters.Add(new SqlParameter("@ImportTemplateID", p_Batch.ImportTemplateID));

                if (p_Batch.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_Batch.TableID));

                command.Parameters.Add(new SqlParameter("@sBatchDescription", p_Batch.BatchDescription));
                if (p_Batch.UploadedFileName != "")
                    command.Parameters.Add(new SqlParameter("@sUploadedFileName", p_Batch.UploadedFileName));

                //command.Parameters.Add(new SqlParameter("@dDateAdded", p_Batch.DateAdded));
                command.Parameters.Add(new SqlParameter("@nUniqueName", p_Batch.UniqueName));


                command.Parameters.Add(new SqlParameter("@nUserIDUploaded", p_Batch.UserIDUploaded));

                command.Parameters.Add(new SqlParameter("@nAccountID ", p_Batch.AccountID));

                if (p_Batch.IsImportPositional != null)
                    command.Parameters.Add(new SqlParameter("@bIsImportPositional", p_Batch.IsImportPositional));

                if (p_Batch.AllowDataUpdate != null)
                    command.Parameters.Add(new SqlParameter("@bAllowDataUpdate", p_Batch.AllowDataUpdate));




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




    //public static int ets_Batch_Insert(Batch p_Batch, ref SqlConnection connection, ref SqlTransaction tn)
    //{

    //    using (SqlCommand command = new SqlCommand("ets_Batch_Insert", connection, tn))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;
    //        SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //        pRV.Direction = ParameterDirection.Output;

    //        command.Parameters.Add(pRV);

    //        if (p_Batch.TableID != null)
    //            command.Parameters.Add(new SqlParameter("@nTableID", p_Batch.TableID));

    //        command.Parameters.Add(new SqlParameter("@sBatchDescription", p_Batch.BatchDescription));
    //        if (p_Batch.UploadedFileName != "")
    //            command.Parameters.Add(new SqlParameter("@sUploadedFileName", p_Batch.UploadedFileName));

    //        //command.Parameters.Add(new SqlParameter("@dDateAdded", p_Batch.DateAdded));
    //        command.Parameters.Add(new SqlParameter("@nUniqueName", p_Batch.UniqueName));


    //        command.Parameters.Add(new SqlParameter("@nUserIDUploaded", p_Batch.UserIDUploaded));

    //        command.Parameters.Add(new SqlParameter("@nAccountID ", p_Batch.AccountID));

    //        if (p_Batch.IsImportPositional != null)
    //            command.Parameters.Add(new SqlParameter("@bIsImportPositional", p_Batch.IsImportPositional));


    //        if (p_Batch.AllowDataUpdate != null)
    //            command.Parameters.Add(new SqlParameter("@bAllowDataUpdate", p_Batch.AllowDataUpdate));


    //        //connection.Open();
    //        command.ExecuteNonQuery();

    //        return int.Parse(pRV.Value.ToString());
    //    }

    //}

    public static bool IsUnlikelyValueOK(string sData, int iTableID)
    {
        try
        {

        }
        catch
        {
            return true;
        }

        return true;
    }

    public static bool IsDataValid(string sData, string sValidation, ref string sError)
    {

       


        try
        {

            if (sData != "" && sData.IndexOf("year") > -1 && sData.IndexOf(" ") > -1)
            {
                sData = sData.Substring(0, sData.IndexOf(" "));
            }
           
            DataTable dtTemp = new DataTable();
            DataColumn colDecimal = new DataColumn("Value");
            colDecimal.DataType = System.Type.GetType("System.Decimal");
            dtTemp.Columns.Add(colDecimal);
            DataRow dr = dtTemp.NewRow();

            sData = Common.IgnoreSymbols(sData);

            dr[0] = double.Parse(sData);
            dtTemp.Rows.Add(dr);


            DataView dv = new DataView(dtTemp);
            dv.RowFilter = sValidation; 

            if (dv.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch
        {
            return false;
        }
    }

    //public static bool IsDataValid(string sData, string sValidation, ref string sError, bool bIgnoreSymbols)
    //{



    //    if (bIgnoreSymbols)
    //    {
    //        sData = Common.IgnoreSymbols(sData);
    //    }


    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalTestConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("Data_Validation", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.CommandTimeout = 0;
    //            command.Parameters.Add(new SqlParameter("@sData", sData));
    //            command.Parameters.Add(new SqlParameter("@sValidation", sValidation));

    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            DataSet ds = new DataSet();
    //            connection.Open();
    //            try
    //            {
    //                da.Fill(ds);
    //            }
    //            catch
    //            {
    //                //

    //            }
    //            connection.Close();
    //            connection.Dispose();

    //            if (ds == null)
    //                return false;


    //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //            {
    //                if (ds.Tables[0].Rows[0][0].ToString().ToLower() == "valid")
    //                {
    //                    sError = "";
    //                    return true;
    //                }
    //                else
    //                {
    //                    sError = ds.Tables[0].Rows[0][1].ToString();
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                sError = "";
    //                return false;
    //            }
    //        }
    //    }


        
    //}


//    public static void UploadText(int? iUserID, Table theTable, string strBatchDescription,
//    string strOriginalFileName, Guid guidNew, string strImportFolder,
//     out string strMsg, out int iBatchID, ref SqlConnection connection,
//       ref SqlTransaction tn, string strFileExtension, string strSelectedSheet, int iAccountID, bool? bAllowDataUpload, int? iImportTemplateID)
//    {
//        ImportTemplate theImportTemplate = null;


//        string strUniqueColumnIDSys = "";
//        string strUniqueColumnID2Sys = "";

//        if (theTable.UniqueColumnID != null)
//            strUniqueColumnIDSys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + theTable.UniqueColumnID.ToString(), tn, connection);

//        if (theTable.UniqueColumnID2 != null)
//            strUniqueColumnID2Sys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + theTable.UniqueColumnID2.ToString(), tn, connection);


//        string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", theTable.AccountID, theTable.TableID, tn, connection);
//        bool bShowExceedances = false;

//        if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
//        {
//            bShowExceedances = true;
//        }

//        if (iImportTemplateID != null)
//            theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)iImportTemplateID, tn, connection);

//        if (theImportTemplate != null)
//        {
//            theTable.ImportColumnHeaderRow = theImportTemplate.ImportColumnHeaderRow;
//            theTable.ImportDataStartRow = theImportTemplate.ImportDataStartRow;
//        }

//        string strTemp = "";
//        string strFileUniqueName = guidNew.ToString() + strFileExtension;
//        strMsg = "";
//        int z = 0;
//        iBatchID = -1;
//        string strDateRecordedColumnName = "Date Recorded";
//        string strTimeSamledColumnName = "Time Recorded";



//        if (theTable.TempImportColumnHeaderRow != null)
//            theTable.ImportColumnHeaderRow = theTable.TempImportColumnHeaderRow;

//        if (theTable.TempImportDataStartRow != null)
//            theTable.ImportDataStartRow = theTable.TempImportDataStartRow;





//        try
//        {

//            strMsg = "";


//            //lets get date and time column name

//            DataTable dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      NameOnImport 
//                        FROM  [Column]
//                        WHERE SystemName='DateTimeRecorded' AND TableID=" + theTable.TableID.ToString(), ref connection, ref tn);

//            if (dtDateTimeColumnName.Rows.Count > 0 && dtDateTimeColumnName.Rows[0][0] != DBNull.Value)
//            {
//                string strDT = dtDateTimeColumnName.Rows[0][0].ToString();
//                if (strDT.IndexOf(",") > 0)
//                {
//                    strDateRecordedColumnName = strDT.Substring(0, strDT.IndexOf(","));
//                    strTimeSamledColumnName = strDT.Substring(strDT.IndexOf(",") + 1);
//                }
//                else
//                {
//                    strDateRecordedColumnName = strDT;
//                    strTimeSamledColumnName = "";
//                }
//            }




//            DataTable dtImportFileTable;

//            string strNameOnImport = "NameOnImport";

//            dtImportFileTable = null;

            
//            dtImportFileTable = UploadManager.GetImportFileTableFromText(strImportFolder, strFileUniqueName, ref strMsg);
//            z = 0;
                 

//            if (strMsg != "")
//            {

//                return;

//            }

//            //PERFORM CLIENT Specific treatment

//            if (theTable.ImportColumnHeaderRow == null)
//                theTable.ImportColumnHeaderRow = 1;
//            if (theTable.ImportDataStartRow == null)
//                theTable.ImportDataStartRow = 2;



//            if (theTable.ImportColumnHeaderRow != null)
//            {
//                //if ((int)theTable.ImportColumnHeaderRow > 1)
//                //{
//                if (dtImportFileTable.Rows.Count >= (int)theTable.ImportColumnHeaderRow)
//                {
//                    for (int i = 0; i <= dtImportFileTable.Columns.Count - 1; i++)
//                    {
//                        if (dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() == "")
//                        {
//                            //do nothing for it
//                            if (strFileExtension.ToLower() == ".csv")
//                            {
//                                try
//                                {
//                                    dtImportFileTable.Columns[i].ColumnName = "Column" + (i + 1).ToString();
//                                }
//                                catch
//                                {
//                                    //
//                                }
//                            }
//                        }
//                        else
//                        {
//                            try
//                            {
//                                dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString();
//                            }
//                            catch (Exception ex)
//                            {
//                                if (ex.Message.IndexOf("already belongs to this DataTable") > -1)
//                                {
//                                    for (int j = 1; j < 20; j++)
//                                    {
//                                        bool bOK = true;
//                                        foreach (DataColumn dc in dtImportFileTable.Columns)
//                                        {
//                                            if (dc.ColumnName == dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() + j.ToString())
//                                            {
//                                                bOK = false;
//                                            }
//                                        }

//                                        if (bOK)
//                                        {
//                                            dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() + j.ToString();
//                                            dtImportFileTable.AcceptChanges();
//                                            break;
//                                        }

//                                    }
//                                }
//                            }
//                        }

//                    }
//                    dtImportFileTable.AcceptChanges();
//                }
//                //}

//            }



//            if (theTable.ImportDataStartRow != null)
//            {
//                for (int i = 1; i <= (int)theTable.ImportDataStartRow - 1; i++)
//                {
//                    dtImportFileTable.Rows.RemoveAt(0);

//                }
//                dtImportFileTable.AcceptChanges();
//            }





//            DataTable dtRecordTypleColumns;
//            DataTable dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theTable.TableID, tn, connection);
//            string strListOfNoNeedColumns = "";


//            //dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
//            if (dtImportFileTable.Rows.Count > 0)
//            {
//                try
//                {
//                    dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare(field.ToString().Trim(), string.Empty) == 0)).CopyToDataTable();
//                }
//                catch
//                {
//                    //
//                }

//            }

//            if (iImportTemplateID != null)
//            {
//                strNameOnImport = "NameOnImport";
//                dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, iImportTemplateID, ref connection, ref tn);
//            }
//            else if (strNameOnImport == "DisplayName")
//            {
//                dtRecordTypleColumns = RecordManager.ets_Table_Columns_DisplayName((int)theTable.TableID, ref connection, ref tn);
//            }
//            else
//            {
//                dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, null, ref connection, ref tn);
//            }



//            for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
//            {
//                bool bIsFound = false;
//                for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                {
//                    if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                        Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
//                    {
//                        bIsFound = true;
//                        break;
//                    }
//                }
//                if (bIsFound == false)
//                {
//                    if (dtImportFileTable.Columns[r].ColumnName.ToLower() != strTimeSamledColumnName.ToLower() && dtImportFileTable.Columns[r].ColumnName.ToLower() != strDateRecordedColumnName.ToLower())
//                    {
//                        strListOfNoNeedColumns += dtImportFileTable.Columns[r].ColumnName + ",";
//                    }
//                }
//            }

//            List<string> strRemoveIndexes = strListOfNoNeedColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();


//            foreach (string item in strRemoveIndexes)
//            {
//                try
//                {
//                    dtImportFileTable.Columns.Remove(item);
//                }
//                catch
//                {
//                    //
//                }
//            }


//            string strListOfMissingColumns = "";
//            for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//            {


//                bool bMissingColumnFound = false;

//                for (int ic = 0; ic < dtImportFileTable.Columns.Count; ic++)
//                {
//                    if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[ic].ColumnName.Trim().ToLower()) ==
//                Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
//                    {
//                        bMissingColumnFound = true;
//                        break;
//                    }

//                }

//                if (bMissingColumnFound == false)
//                {
//                    strListOfMissingColumns += dtRecordTypleColumns.Rows[i][strNameOnImport].ToString() + ",";
//                }


//            }

//            if (strListOfMissingColumns.Length > 0)
//            {
//                List<string> strMissingColumns = strListOfMissingColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();
//                foreach (string item in strMissingColumns)
//                {
//                    try
//                    {
//                        dtImportFileTable.Columns.Add(item);
//                    }
//                    catch
//                    {
//                        //
//                    }
//                }

//            }

//            dtImportFileTable.AcceptChanges();

//            for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
//            {
//                for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                {
//                    if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                        Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
//                    {
//                        try
//                        {
//                            dtImportFileTable.Columns[r].ColumnName = dtRecordTypleColumns.Rows[i]["SystemName"].ToString();
//                            break;
//                        }
//                        catch
//                        {
//                            //
//                        }
//                    }
//                    if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                      strDateRecordedColumnName.ToLower())
//                    {
//                        dtImportFileTable.Columns[r].ColumnName = "DateTimeRecorded";
//                        break;
//                    }
//                }

//            }







//            //now dtCSV is ready to be imported into Batch & TempRecord

//            Batch newBatch = new Batch(null, (int)theTable.TableID,
//                strBatchDescription.Trim() == "" ? strOriginalFileName : strBatchDescription.Trim(),
//                strOriginalFileName, null, guidNew, iUserID, iAccountID, theTable.IsImportPositional);

//            //need a single transaction

//            newBatch.AllowDataUpdate = bAllowDataUpload;

//            iBatchID = UploadManager.ets_Batch_Insert(newBatch, ref connection, ref tn);




//            for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//            {
//                TempRecord newTempRecord = new TempRecord();
//                newTempRecord.AccountID = iAccountID;
//                newTempRecord.BatchID = iBatchID;
//                //bool bIsBlank = false;
//                string strRejectReason = "";
//                string strWarningReason = "";
//                string strExceedanceReason = "";


//                foreach (DataColumn dc in dtImportFileTable.Columns)
//                {
//                    string strColumnName = "";
//                    strColumnName = dc.ColumnName;
//                    bool bEachColumnExceedance = false;

//                    if (strColumnName.ToLower() != strTimeSamledColumnName.ToLower())
//                    {

//                        //if (dc.ColumnName.ToUpper() == "LOCATIONID")
//                        //{
//                        //    strColumnName = "LocationName";
//                        //}

//                        if (dc.ColumnName.ToUpper() == "DATETIMERECORDED")
//                        {
//                            //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() == "")
//                            //{
//                            //    if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
//                            //    {
//                            //        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                            //    }
//                            //    //bIsBlank = true;
//                            //}
//                        }

//                        if (dc.ColumnName.ToUpper() == "DATETIMERECORDED")
//                        {
//                            newTempRecord.DateFormat = theTable.DateFormat;
//                            //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                            //{


//                            try
//                            {
//                                if (strFileExtension == ".csv")
//                                {
//                                    if (strTimeSamledColumnName == "")
//                                    {
//                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                    }
//                                    else
//                                    {

//                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() + " " + dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString());
//                                    }

//                                }
//                                else if (strFileExtension == ".xml")
//                                {
//                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                }
//                                else
//                                {
//                                    string strDateTimeTemp = "";
//                                    if (strTimeSamledColumnName == "")
//                                    {
//                                        //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                        //{
//                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0));
//                                        //}
//                                    }
//                                    else
//                                    {
//                                        strDateTimeTemp = dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString();

//                                        if (dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString().Length > 10)
//                                        {
//                                            strDateTimeTemp = strDateTimeTemp.Substring(11);
//                                        }
//                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                        {
//                                            //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 9)
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ") != -1)
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ")) + " " + strDateTimeTemp);
//                                            }
//                                            else
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0) + " " + strDateTimeTemp);
//                                            }
//                                            //
//                                        }
//                                    }
//                                }
//                            }
//                            catch
//                            {
//                                if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
//                                    strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                //bIsBlank = true;
//                            }

//                        }
//                        else
//                        {
//                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                        }



//                        for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                        {
//                            if (dc.ColumnName.ToLower() ==
//                                dtRecordTypleColumns.Rows[i]["SystemName"].ToString().Trim().ToLower())
//                            {

//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length == 0)
//                                {
//                                    if (dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString() != "")
//                                    {
//                                        if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "datetime"
//                                            || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "date"
//                                            || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "time")
//                                        {
//                                            dtImportFileTable.Rows[r][dc.ColumnName] = DateTime.Now;

//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, DateTime.Now);
//                                        }
//                                        else
//                                        {

//                                            dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());
//                                        }
//                                        dtImportFileTable.AcceptChanges();
//                                    }

//                                }



//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 0)
//                                {



//                                    if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "datetime"
//                                        || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "date")
//                                    {
//                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                        {
//                                            try
//                                            {
//                                                Convert.ToDateTime(dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                            }
//                                            catch
//                                            {
//                                                strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                            }

//                                        }

//                                    }

//                                    if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "time")
//                                    {
//                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                        {
//                                            try
//                                            {
//                                                Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                            }
//                                            catch
//                                            {
//                                                strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                            }

//                                        }

//                                    }


//                                    //if (dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString() != "")
//                                    //{
//                                    //    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                    //    {
//                                    //        if (!UploadManager.IsDataIntoDropDown(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString()))
//                                    //        {
//                                    //            strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                    //        }

//                                    //    }

//                                    //}


//                                    if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"] != DBNull.Value)
//                                    {
//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
//                                        {
//                                            if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString()), null))
//                                            {

//                                            }
//                                            else
//                                            {
//                                                strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                            }
//                                        }

//                                    }

//                                    if (bShowExceedances)
//                                    {
//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
//                                            {
//                                                if (!UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString()), null))
//                                                {
//                                                    strExceedanceReason = strExceedanceReason + " EXCEEDANCE: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
//                                                    bEachColumnExceedance = true;
//                                                }
//                                                else
//                                                {

//                                                }
//                                            }
//                                        }

//                                    }

//                                    if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"] != DBNull.Value && bEachColumnExceedance == false)
//                                    {
//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
//                                        {
//                                            if (!UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
//                                            {
//                                                strWarningReason = strWarningReason + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
//                                            }
//                                            else
//                                            {

//                                            }
//                                        }
//                                    }





//                                    //check SD
//                                    string strData = dtImportFileTable.Rows[r][dc.ColumnName].ToString();
//                                    if (bool.Parse(dtRecordTypleColumns.Rows[i]["CheckUnlikelyValue"].ToString()))
//                                    {
//                                        int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

//                                        if (iCount >= Common.MinSTDEVRecords)
//                                        {
//                                            string strRecordedate;
//                                            if (dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
//                                            {
//                                                strRecordedate = Common.IgnoreSymbols(strData);
//                                            }
//                                            else
//                                            {
//                                                strRecordedate = strData;
//                                            }

//                                            double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

//                                            double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

//                                            double dRecordedate = double.Parse(strRecordedate);
//                                            if (dAVG != null && dSTDEV != null)
//                                            {
//                                                dSTDEV = dSTDEV * 3;
//                                                if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
//                                                {
//                                                    //deviation happaned
//                                                    strWarningReason = strWarningReason + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
//                                                }

//                                            }
//                                        }

//                                    }

//                                    //End SD

//                                }
//                                //else
//                                //{

    //                                if (dtRecordTypleColumns.Rows[i]["Importance"].ToString().ToLower() == "m")
//                                {
//                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
//                                    {
//                                        strRejectReason = strRejectReason + " MANDATORY:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                    }

//                                }

//                                //}
//                                break;
//                            }
//                        }
//                    }
//                }

//                for (int i = 0; i < dtColumnsAll.Rows.Count; i++)
//                {
//                    if (dtColumnsAll.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "number")
//                    {
//                        if (dtColumnsAll.Rows[i]["NumberType"] != null &&
//                            dtColumnsAll.Rows[i]["NumberType"].ToString() == "8")
//                        {
//                            string strValue = "1";
//                            try
//                            {
//                                string strMax = "";

//                                if (r == z)
//                                {
//                                    strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")) FROM Record WHERE IsNumeric(" + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")=1 and TableID=" + theTable.TableID.ToString(), tn, connection);
//                                }
//                                else
//                                {
//                                    strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")) FROM TempRecord WHERE IsNumeric(" + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")=1 and  TableID=" + theTable.TableID.ToString(), tn, connection);
//                                }
//                                if (strMax == "")
//                                {
//                                    strValue = "1";
//                                }
//                                else
//                                {
//                                    strValue = (int.Parse(strMax) + 1).ToString();
//                                }
//                            }
//                            catch
//                            {
//                                strValue = "1";
//                            }
//                            UploadManager.MakeTheTempRecord(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString(), strValue);

//                        }
//                    }
//                }

//                newTempRecord.TableID = (int)theTable.TableID;

//                //if (strLocationID == "-1")
//                //{
//                //    if (newTempRecord.LocationID == null)
//                //    {
//                //        if (newTempRecord.LocationName == null)
//                //        {
//                //            //ops we have not found it
//                //            // strRejectReason = strRejectReason + " Location not found.";
//                //        }
//                //        else
//                //        {

//                //            if (newTempRecord.LocationName.Length > 0)
//                //            {

//                //                Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);

//                //                if (tempLocation == null)
//                //                {
//                //                    Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);
//                //                    {
//                //                        if (tempLocationImp == null)
//                //                        {
//                //                            //ops we have not found it
//                //                            strRejectReason = strRejectReason + " Location -" + newTempRecord.LocationName + " - is not found!";
//                //                        }
//                //                        else
//                //                        {
//                //                            newTempRecord.LocationID = tempLocationImp.LocationID;
//                //                            newTempRecord.LocationName = tempLocationImp.LocationName;
//                //                        }
//                //                    }
//                //                }
//                //                else
//                //                {
//                //                    newTempRecord.LocationID = tempLocation.LocationID;
//                //                    newTempRecord.LocationName = tempLocation.LocationName;
//                //                }

//                //            }
//                //            else
//                //            {
//                //                //strRejectReason = strRejectReason + " Location not found.";
//                //            }
//                //        }
//                //    }
//                //    else
//                //    {
//                //        Location tempLocation = SiteManager.ets_Location_Details((int)newTempRecord.LocationID, ref connection, ref tn);

//                //        if (tempLocation == null)
//                //        {
//                //            // strRejectReason = strRejectReason + " Location not found.";
//                //        }
//                //        else
//                //        {
//                //            newTempRecord.LocationID = tempLocation.LocationID;
//                //            newTempRecord.LocationName = tempLocation.LocationName;
//                //        }
//                //    }

//                //}
//                //else
//                //{
//                //    newTempRecord.LocationID = int.Parse(strLocationID);
//                //    newTempRecord.LocationName = strLocation;
//                //}

//                if (newTempRecord.DateTimeRecorded == null)
//                {
//                    newTempRecord.DateTimeRecorded = DateTime.Now;

//                }


//                strRejectReason = strRejectReason.Trim();
//                strWarningReason = strWarningReason.Trim();
//                strExceedanceReason = strExceedanceReason.Trim();


//                if (strRejectReason.Length > 0)
//                {
//                    newTempRecord.RejectReason = strRejectReason.Trim();
//                }

//                if (strWarningReason.Length > 0)
//                {
//                    newTempRecord.WarningReason = strWarningReason.Trim();
//                }

//                if (strExceedanceReason.Length > 0)
//                {
//                    newTempRecord.WarningReason = newTempRecord.WarningReason == "" ? strExceedanceReason.Trim() : newTempRecord.WarningReason + " " + strExceedanceReason.Trim();

//                }


//                int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord, ref connection, ref tn);


//            }

//            if (strUniqueColumnIDSys != "" || strUniqueColumnID2Sys != "")
//            {
//                RecordManager.ets_Batch_Duplicate(iBatchID, strUniqueColumnIDSys, strUniqueColumnID2Sys, tn);
//            }

//            try
//            {
//                //if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
//                //    File.Delete(strImportFolder + "\\" + strFileUniqueName);
//            }
//            catch
//            {

//            }
//            //Response.Redirect("~/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=" + Cryptography.Encrypt(iBatchID.ToString()), false);
//        }
//        catch (Exception ex)
//        {
//            //ErrorLog theErrorLog = new ErrorLog(null, "Record Upload", ex.Message, ex.StackTrace, DateTime.Now, strTemp);


//            if (ex.Message.IndexOf("DateTime") > -1)
//            {
//                strMsg = "Date Recorded data are not valid, please review the file data.";
//            }
//            else if (ex.Message.IndexOf("recognized") > -1)
//            {
//                strMsg = "Unknown error occurred please review your import data.";
//            }
//            else if (ex.Message.IndexOf(strTimeSamledColumnName) > -1)
//            {
//                strMsg = "The file must have a Time Recorded column just after Date Recorded column.";
//            }
//            else
//            {
//                strMsg = "UNKNOWN:" + ex.Message + ex.StackTrace;
//                //SystemData.ErrorLog_Insert(theErrorLog);
//            }

//        }




//    }


//    public static void UploadCSV(  SqlConnection cnOne, int? iUserID, Table theTable, string strBatchDescription,
//     string strOriginalFileName, Guid guidNew, string strImportFolder,
//      out string strMsg, out int iBatchID,  string strFileExtension, string strSelectedSheet, int iAccountID, bool? bAllowDataUpload, int? iImportTemplateID)
//   {

//       SqlConnection cn1;
//       SqlConnection cn2; //Teast Area for IsDataValid - for safe use

//        if(cnOne==null)
//        {

//        }




//        ImportTemplate theImportTemplate = null;
//        string strUniqueColumnIDSys = "";
//        string strUniqueColumnID2Sys = "";

//        if (theTable.UniqueColumnID != null)
//            strUniqueColumnIDSys=Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + theTable.UniqueColumnID.ToString(),null,null);

//        if (theTable.UniqueColumnID2 != null)
//            strUniqueColumnID2Sys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + theTable.UniqueColumnID2.ToString(), null, null);


//        string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", theTable.AccountID, theTable.TableID, null, null);
//        bool bShowExceedances = false;

//        if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
//        {
//            bShowExceedances = true;
//        }

//        if (iImportTemplateID != null)
//            theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)iImportTemplateID, null, null);

//        if (theImportTemplate != null)
//        {
//            theTable.ImportColumnHeaderRow = theImportTemplate.ImportColumnHeaderRow;
//            theTable.ImportDataStartRow = theImportTemplate.ImportDataStartRow;
//        }

//        string strTemp = "";
//        string strFileUniqueName = guidNew.ToString() + strFileExtension;
//        strMsg = "";
//        int z = 0;
//        iBatchID = -1;
//        string strDateRecordedColumnName = "Date Recorded";
//        string strTimeSamledColumnName = "Time Recorded";

//        //if (strLocationID == "-1")
//        //{
//        //            DataTable dtSS = Common.DataTableFromText(@"SELECT     Location.LocationID
//        //                FROM         Location INNER JOIN
//        //                                      LocationTable ON Location.LocationID = LocationTable.LocationID
//        //                WHERE Location.IsActive=1 AND  LocationTable.TableID=" + theTable.TableID.ToString(), ref connection, ref tn);
//        //            if (dtSS.Rows.Count > 0)
//        //            {


//        //            }
//        //            else
//        //            {
//        //                //no Location, so create one

//        //                try
//        //                {

//        //                    DataTable dtSS2 = Common.DataTableFromText(@"SELECT     Location.LocationID
//        //                                FROM         Location  
//        //                                WHERE Location.IsActive=1 AND LocationName='Location 1'
//        //                                AND AccountID=" + theTable.AccountID.ToString(), ref connection, ref tn);

//        //                    int iLocationID = -1;
//        //                    if (dtSS2.Rows.Count > 0)
//        //                    {
//        //                        iLocationID = int.Parse(dtSS2.Rows[0][0].ToString());
//        //                    }
//        //                    else
//        //                    {

//        //                        Location newLocation = new Location(null, "Location 1",
//        //                            "", "Created at the time of import", true, null, null, null, null, (int)theTable.AccountID, "");
//        //                        iLocationID = SiteManager.ets_Location_Insert(newLocation, tn, null);
//        //                    }



//        //                    LocationTable newLocationTable = new LocationTable(null, iLocationID, (int)theTable.TableID, "", "");
//        //                    SiteManager.ets_LocationTable_Insert(newLocationTable, tn, null);

//        //                    strLocationID = iLocationID.ToString();
//        //                }
//        //                catch
//        //                {
//        //                    //
//        //                }

//        //            }

//        //}

//        if (theTable.TempImportColumnHeaderRow != null)
//            theTable.ImportColumnHeaderRow = theTable.TempImportColumnHeaderRow;

//        if (theTable.TempImportDataStartRow != null)
//            theTable.ImportDataStartRow = theTable.TempImportDataStartRow;

//        //if (strFileExtension.ToLower() == ".csv")
//        //{
//        //    if (theTable.ImportColumnHeaderRow != null)
//        //    {
//        //        theTable.ImportColumnHeaderRow =(int)theTable.ImportColumnHeaderRow + 1;

//        //    }
//        //    if (theTable.ImportDataStartRow != null)
//        //    {
//        //        theTable.ImportDataStartRow = (int)theTable.ImportDataStartRow + 1;

//        //    }
//        //}

//        if (theTable.IsImportPositional == false)
//        {

//            try
//            {

//                strMsg = "";


//                //lets get date and time column name

//                DataTable dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      NameOnImport 
//                        FROM  [Column]
//                        WHERE SystemName='DateTimeRecorded' AND TableID=" + theTable.TableID.ToString(), null, null);


//                //////////////Testing tn
//                //int iUserIDTEST = int.Parse(SystemData.SystemOption_ValueByKey_Account("AutoUploadUserID", null, null));

//                //Common.ExecuteText("UPDATE Record SET V020='NO tn 2 ' WHERE RecordID=1242456");

//                //Common.ExecuteText("UPDATE Record SET V021='WITH tn 2' WHERE RecordID=1242456", tn);

               
//                //Record aaTestRecord = RecordManager.ets_Record_Detail_Full(1242456);

//                ///////////////


//                if (dtDateTimeColumnName.Rows.Count > 0 && dtDateTimeColumnName.Rows[0][0] != DBNull.Value)
//                {
//                    string strDT = dtDateTimeColumnName.Rows[0][0].ToString();
//                    if (strDT.IndexOf(",") > 0)
//                    {
//                        strDateRecordedColumnName = strDT.Substring(0, strDT.IndexOf(","));
//                        strTimeSamledColumnName = strDT.Substring(strDT.IndexOf(",") + 1);
//                    }
//                    else
//                    {
//                        strDateRecordedColumnName = strDT;
//                        strTimeSamledColumnName = "";
//                    }
//                }




//                DataTable dtImportFileTable;

//                string strNameOnImport = "NameOnImport";

//                dtImportFileTable = null;

//                switch (strFileExtension.ToLower())
//                {
//                    case ".dbf":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromDBF(strImportFolder, strFileUniqueName, ref strMsg);
//                        z = 0;
//                        break;

//                    case ".txt":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromText(strImportFolder, strFileUniqueName, ref strMsg);
//                        z = 0;
//                        break;
//                    case ".csv":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName, ref strMsg);
//                        z = 0;
//                        break;
//                    case ".xls":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
//                        break;
//                    case ".xlsx":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
//                        break;

//                    case ".xml":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromXML(strImportFolder, strFileUniqueName);
//                        strNameOnImport = "DisplayName";

//                        break;

//                    case "virtual":
//                        dtImportFileTable = UploadManager.GetVirtualImportFileTable(BitConverter.ToInt32(guidNew.ToByteArray(), 8),
//                            theTable.TableID.Value,
//                            BitConverter.ToInt32(guidNew.ToByteArray(), 0),
//                            null, null);
//                        theTable.ImportColumnHeaderRow = 1;
//                        theTable.ImportDataStartRow = 2;
//                        break;
//                }

//                if (strMsg != "")
//                {

//                    return;

//                }

//                //PERFORM CLIENT Specific treatment


//                if (strFileExtension.ToLower()!=".dbf")
//                {
//                    if (theTable.ImportColumnHeaderRow == null)
//                        theTable.ImportColumnHeaderRow = 1;
//                    if (theTable.ImportDataStartRow == null)
//                        theTable.ImportDataStartRow = 2;



//                    if (theTable.ImportColumnHeaderRow != null)
//                    {
//                        //if ((int)theTable.ImportColumnHeaderRow > 1)
//                        //{
//                        if (dtImportFileTable.Rows.Count >= (int)theTable.ImportColumnHeaderRow)
//                        {
//                            for (int i = 0; i <= dtImportFileTable.Columns.Count - 1; i++)
//                            {
//                                if (dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() == "")
//                                {
//                                    //do nothing for it
//                                    if (strFileExtension.ToLower() == ".csv")
//                                    {
//                                        try
//                                        {
//                                            dtImportFileTable.Columns[i].ColumnName = "Column" + (i + 1).ToString();
//                                        }
//                                        catch
//                                        {
//                                            //
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    try
//                                    {
//                                        dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString();
//                                    }
//                                    catch (Exception ex)
//                                    {
//                                        if (ex.Message.IndexOf("already belongs to this DataTable") > -1)
//                                        {
//                                            for (int j = 1; j < 20; j++)
//                                            {
//                                                bool bOK = true;
//                                                foreach (DataColumn dc in dtImportFileTable.Columns)
//                                                {
//                                                    if (dc.ColumnName == dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() + j.ToString())
//                                                    {
//                                                        bOK = false;
//                                                    }
//                                                }

//                                                if (bOK)
//                                                {
//                                                    dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[(int)theTable.ImportColumnHeaderRow - 1][i].ToString() + j.ToString();
//                                                    dtImportFileTable.AcceptChanges();
//                                                    break;
//                                                }

//                                            }
//                                        }
//                                    }
//                                }

//                            }
//                            dtImportFileTable.AcceptChanges();
//                        }
//                        //}

//                    }



//                    if (theTable.ImportDataStartRow != null)
//                    {
//                        for (int i = 1; i <= (int)theTable.ImportDataStartRow - 1; i++)
//                        {
//                            dtImportFileTable.Rows.RemoveAt(0);

//                        }
//                        dtImportFileTable.AcceptChanges();
//                    }

//                    int xy = 0;
//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        dtImportFileTable.Columns[xy].ColumnName = Common.RemoveSpecialCharacters(dc.ColumnName);
//                        xy = xy + 1;
//                    }
//                    dtImportFileTable.AcceptChanges();



//                }
                

//                DataTable dtRecordTypleColumns;
//                DataTable dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theTable.TableID, null, null);
//                string strListOfNoNeedColumns = "";


//                //dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
//                if (dtImportFileTable.Rows.Count > 0)
//                {
//                    try
//                    {
//                        dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare(field.ToString().Trim(), string.Empty) == 0)).CopyToDataTable();
//                    }
//                    catch
//                    {
//                        //
//                    }

//                }

//                if (iImportTemplateID != null)
//                {
//                    strNameOnImport = "NameOnImport";
//                    dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, iImportTemplateID);
//                }
//                else if (strNameOnImport == "DisplayName")
//                {
//                    dtRecordTypleColumns = RecordManager.ets_Table_Columns_DisplayName((int)theTable.TableID,null,null);
//                }
//                else
//                {
//                    dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, null);
//                }



//                for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
//                {
//                    bool bIsFound = false;
//                    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                    {
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                            Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
//                        {
//                            bIsFound = true;
//                            break;
//                        }
//                    }
//                    if (bIsFound == false)
//                    {
//                        if (dtImportFileTable.Columns[r].ColumnName.ToLower() != strTimeSamledColumnName.ToLower() && dtImportFileTable.Columns[r].ColumnName.ToLower() != strDateRecordedColumnName.ToLower())
//                        {
//                            strListOfNoNeedColumns += dtImportFileTable.Columns[r].ColumnName + ",";
//                        }
//                    }
//                }

//                if (strFileExtension == ".txt")
//                {
//                    strListOfNoNeedColumns = "";
//                }

//                List<string> strRemoveIndexes = strListOfNoNeedColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();


//                foreach (string item in strRemoveIndexes)
//                {
//                    try
//                    {
//                        dtImportFileTable.Columns.Remove(item);
//                    }
//                    catch
//                    {
//                        //
//                    }
//                }


//                string strListOfMissingColumns = "";
//                for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                {


//                    bool bMissingColumnFound = false;

//                    for (int ic = 0; ic < dtImportFileTable.Columns.Count; ic++)
//                    {
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[ic].ColumnName.Trim().ToLower()) ==
//                    Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
//                        {
//                            bMissingColumnFound = true;
//                            break;
//                        }

//                    }

//                    if (bMissingColumnFound == false)
//                    {
//                        strListOfMissingColumns += dtRecordTypleColumns.Rows[i][strNameOnImport].ToString() + ",";
//                    }


//                }

//                if (strFileExtension == ".txt")
//                {
//                    strListOfMissingColumns = "";
//                }
//                if (strListOfMissingColumns.Length > 0)
//                {
//                    List<string> strMissingColumns = strListOfMissingColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();
//                    foreach (string item in strMissingColumns)
//                    {
//                        try
//                        {
//                            dtImportFileTable.Columns.Add(item);
//                        }
//                        catch
//                        {
//                            //
//                        }
//                    }

//                }

//                dtImportFileTable.AcceptChanges();

              

//                for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
//                {
//                    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                    {
//                        //if (strFileExtension == ".txt")
//                        //{
//                        //    dtImportFileTable.Columns[r].ColumnName = dtRecordTypleColumns.Rows[i]["SystemName"].ToString();
//                        //    break;                           
//                        //}

//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                            Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strNameOnImport].ToString().Trim().ToLower()))
//                        {
//                            try
//                            {
//                                dtImportFileTable.Columns[r].ColumnName = dtRecordTypleColumns.Rows[i]["SystemName"].ToString();
//                                break;
//                            }
//                            catch
//                            {
//                                //
//                            }
//                        }
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                          strDateRecordedColumnName.ToLower())
//                        {
//                            dtImportFileTable.Columns[r].ColumnName = "DateTimeRecorded";
//                            break;
//                        }
//                    }
//                    //if (strFileExtension == ".txt")
//                    //{
//                    //    break;
//                    //}
//                }







//                //now dtCSV is ready to be imported into Batch & TempRecord

//                Batch newBatch = new Batch(null, (int)theTable.TableID,
//                    strBatchDescription.Trim() == "" ? strOriginalFileName : strBatchDescription.Trim(),
//                    strOriginalFileName, null, guidNew, iUserID, iAccountID, theTable.IsImportPositional);

//                //need a single transaction

//                newBatch.AllowDataUpdate = bAllowDataUpload;

//                iBatchID = UploadManager.ets_Batch_Insert(newBatch,null,null);


//                //AddMissingLocation
//                //if (theTable.AddMissingLocation != null && (bool)theTable.AddMissingLocation)
//                //{
//                //    //lets find Location column
//                //    bool bFoundSS = false;
//                //    foreach (DataColumn dc in dtImportFileTable.Columns)
//                //    {
//                //        if (dc.ColumnName.ToUpper() == "LOCATIONID")
//                //        {
//                //            bFoundSS = true;
//                //        }

//                //    }

//                //    if (bFoundSS)
//                //    {
//                //        for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//                //        {
//                //            string strSSName = dtImportFileTable.Rows[r]["LocationID"].ToString();
//                //            Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, strSSName, ref connection, ref tn);

//                //            if (tempLocation == null)
//                //            {
//                //                Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, strSSName, ref connection, ref tn);
//                //                {
//                //                    if (tempLocationImp == null)
//                //                    {
//                //                        //ops we have not found it so lets insert it

//                //                        try
//                //                        {
//                //                            Location newLocation = new Location(null, strSSName, "", "", true, null, null, null, null,
//                //                                (int)theTable.AccountID, "");

//                //                            int iNewLocationID = SiteManager.ets_Location_Insert(newLocation, tn, null);

//                //                            LocationTable newLocationTable = new LocationTable(null, iNewLocationID, (int)theTable.TableID, "", "");
//                //                            SiteManager.ets_LocationTable_Insert(newLocationTable, tn, null);
//                //                        }
//                //                        catch
//                //                        {
//                //                            //do nothing
//                //                        }
//                //                    }
//                //                }
//                //            }

//                //        }

//                //    }


//                //}//end AddMissingLocation





//                for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//                {
//                    TempRecord newTempRecord = new TempRecord();
//                    newTempRecord.AccountID = iAccountID;
//                    newTempRecord.BatchID = iBatchID;
//                    newTempRecord.TableID = (int)theTable.TableID;
//                    //bool bIsBlank = false;
//                    string strRejectReason = "";
//                    string strWarningReason = "";
//                    string strExceedanceReason = "";


//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        string strColumnName = "";
//                        strColumnName = dc.ColumnName;
//                        bool bEachColumnExceedance = false;

//                        if (strColumnName.ToLower() != strTimeSamledColumnName.ToLower())
//                        {

//                            //if (dc.ColumnName.ToUpper() == "LOCATIONID")
//                            //{
//                            //    strColumnName = "LocationName";
//                            //}

//                            if (dc.ColumnName.ToUpper() == "DATETIMERECORDED")
//                            {
//                                //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() == "")
//                                //{
//                                //    if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
//                                //    {
//                                //        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                //    }
//                                //    //bIsBlank = true;
//                                //}
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERECORDED")
//                            {
//                                newTempRecord.DateFormat = theTable.DateFormat;
//                                //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                //{


//                                try
//                                {
//                                    if (strFileExtension == ".csv")
//                                    {
//                                        if (strTimeSamledColumnName == "")
//                                        {
//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                        }
//                                        else
//                                        {

//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() + " " + dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString());
//                                        }

//                                    }
//                                    else if (strFileExtension == ".xml")
//                                    {
//                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                    }
//                                    else
//                                    {
//                                        string strDateTimeTemp = "";
//                                        if (strTimeSamledColumnName == "")
//                                        {
//                                            //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                            //{
//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0));
//                                            //}
//                                        }
//                                        else
//                                        {
//                                            strDateTimeTemp = dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString();

//                                            if (dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString().Length > 10)
//                                            {
//                                                strDateTimeTemp = strDateTimeTemp.Substring(11);
//                                            }
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                            {
//                                                //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 9)
//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ") != -1)
//                                                {
//                                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ")) + " " + strDateTimeTemp);
//                                                }
//                                                else
//                                                {
//                                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0) + " " + strDateTimeTemp);
//                                                }
//                                                //
//                                            }
//                                        }
//                                    }
//                                }
//                                catch
//                                {
//                                    if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    //bIsBlank = true;
//                                }

//                            }
//                            else
//                            {
//                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                            }



//                            for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                            {
//                                if (dc.ColumnName.ToLower() ==
//                                    dtRecordTypleColumns.Rows[i]["SystemName"].ToString().Trim().ToLower())
//                                {

//                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length == 0)
//                                    {
//                                        if (dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString() != "")
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "datetime"
//                                                || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "date"
//                                                || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "time")
//                                            {
//                                                dtImportFileTable.Rows[r][dc.ColumnName] = DateTime.Now;

//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, DateTime.Now);
//                                            }
//                                            else
//                                            {

//                                                dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());
//                                            }
//                                            dtImportFileTable.AcceptChanges();
//                                        }

//                                    }



//                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 0)
//                                    {



//                                        if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "datetime"
//                                            || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "date")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                            {
//                                                try
//                                                {
//                                                    DateTime dtt = Convert.ToDateTime(dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtt.ToString("dd/MM/yyyy HH:mm:ss"));                                                    
//                                                }
//                                                catch
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                                }
//                                            }
//                                        }

//                                        if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "time")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                            {
//                                                try
//                                                {
//                                                    Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                                }
//                                                catch
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                                }
//                                            }
//                                        }

//                                        if ((dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "dropdown") &&
//                                            (dtRecordTypleColumns.Rows[i]["DropDownType"].ToString() == "tabledd") &&
//                                            (dtRecordTypleColumns.Rows[i]["TableTableID"] != DBNull.Value) &&
//                                            (dtRecordTypleColumns.Rows[i]["DisplayColumn"] != DBNull.Value))
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                            {
//                                                int iTN = 0;
//                                                int iTDN = 0;

//                                                string lookupColumnName = dtRecordTypleColumns.Rows[i]["DisplayColumn"].ToString();
//                                                if (lookupColumnName == "[Site Name]")
//                                                    lookupColumnName = "[Site Name On Import File]";
//                                                string lookupSystemName = String.Empty;
//                                                DataTable columns = RecordManager.ets_Table_Columns_All(int.Parse(dtRecordTypleColumns.Rows[i]["TableTableID"].ToString()),
//                                                    null, null);
//                                                if (columns != null)
//                                                {
//                                                    foreach (DataRow dr in columns.Rows)
//                                                    {
//                                                        if (("[" + dr["DisplayName"].ToString() + "]") == lookupColumnName)
//                                                        {
//                                                            lookupSystemName = dr["SystemName"].ToString();
//                                                            break;
//                                                        }
//                                                    }
//                                                }

//                                                if (!String.IsNullOrEmpty(lookupSystemName))
//                                                {
//                                                    DataTable dtLookup = RecordManager.ets_Record_List(
//                                                        int.Parse(dtRecordTypleColumns.Rows[i]["TableTableID"].ToString()),
//                                                        null, true, null, null, null,
//                                                        "DBGSystemRecordID", "ASC", null, null, ref iTN, ref iTDN,
//                                                        "", "",
//                                                        String.Format("{0} = '{1}'", lookupSystemName, dtImportFileTable.Rows[r][dc.ColumnName].ToString()),
//                                                        null, null, "", "", "", null);
//                                                    if (dtLookup.Rows.Count > 0)
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtLookup.Rows[0]["DBGSystemRecordID"].ToString());
//                                                    }
//                                                }
//                                            }
//                                        }


//                                        //if (dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString() != "")
//                                        //{
//                                        //    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                        //    {
//                                        //        if (!UploadManager.IsDataIntoDropDown(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString()))
//                                        //        {
//                                        //            strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                        //        }

//                                        //    }

//                                        //}


//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString()), null))
//                                                {

//                                                }
//                                                else
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                                }
//                                            }

//                                        }

//                                        if (bShowExceedances)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"] != DBNull.Value)
//                                            {
//                                                if (dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
//                                                {
//                                                    if (!UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString()), null))
//                                                    {
//                                                        strExceedanceReason = strExceedanceReason + " EXCEEDANCE: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
//                                                        bEachColumnExceedance = true;
//                                                    }
//                                                    else
//                                                    {

//                                                    }
//                                                }
//                                            }

//                                        }

//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"] != DBNull.Value && bEachColumnExceedance == false)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
//                                            {
//                                                if (!UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString()), null))
//                                                {
//                                                    strWarningReason = strWarningReason + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
//                                                }
//                                                else
//                                                {

//                                                }
//                                            }
//                                        }





//                                        //check SD
//                                        string strData = dtImportFileTable.Rows[r][dc.ColumnName].ToString();
//                                        if (bool.Parse(dtRecordTypleColumns.Rows[i]["CheckUnlikelyValue"].ToString()))
//                                        {
//                                            int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),  -1);

//                                            if (iCount >= Common.MinSTDEVRecords)
//                                            {
//                                                string strRecordedate;
//                                                if (dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
//                                                {
//                                                    strRecordedate = Common.IgnoreSymbols(strData);
//                                                }
//                                                else
//                                                {
//                                                    strRecordedate = strData;
//                                                }

//                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),  -1);

//                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),  -1);

//                                                double dRecordedate = double.Parse(strRecordedate);
//                                                if (dAVG != null && dSTDEV != null)
//                                                {
//                                                    dSTDEV = dSTDEV * 3;
//                                                    if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
//                                                    {
//                                                        //deviation happaned
//                                                        strWarningReason = strWarningReason + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
//                                                    }

//                                                }
//                                            }

//                                        }

//                                        //End SD

//                                    }
//                                    //else
//                                    //{

    //                                    if (dtRecordTypleColumns.Rows[i]["Importance"].ToString().ToLower() == "m")
//                                    {
//                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
//                                        {
//                                            strRejectReason = strRejectReason + " MANDATORY:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                        }

//                                    }

//                                    //}
//                                    break;
//                                }
//                            }
//                        }
//                    }

//                    for (int i = 0; i < dtColumnsAll.Rows.Count; i++)
//                    {
//                        if (dtColumnsAll.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "number")
//                        {
//                            if (dtColumnsAll.Rows[i]["NumberType"] != null &&
//                                dtColumnsAll.Rows[i]["NumberType"].ToString() == "8")
//                            {
//                                string strValue = "1";
//                                try
//                                {
//                                    string strMax = "";

//                                    if (r == z)
//                                    {
//                                        strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")) FROM Record WHERE IsNumeric(" + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")=1 and TableID=" + theTable.TableID.ToString(), null, null);
//                                    }
//                                    else
//                                    {
//                                        strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")) FROM TempRecord WHERE IsNumeric(" + dtColumnsAll.Rows[i]["SystemName"].ToString() + ")=1 and  TableID=" + theTable.TableID.ToString(), null, null);
//                                    }
//                                    if (strMax == "")
//                                    {
//                                        strValue = "1";
//                                    }
//                                    else
//                                    {
//                                        strValue = (int.Parse(strMax) + 1).ToString();
//                                    }
//                                }
//                                catch
//                                {
//                                    strValue = "1";
//                                }
//                                UploadManager.MakeTheTempRecord(ref newTempRecord, dtColumnsAll.Rows[i]["SystemName"].ToString(), strValue);

//                            }
//                        }
//                    }

//                    //newTempRecord.TableID = (int)theTable.TableID;

//                    //if (strLocationID == "-1")
//                    //{
//                    //    if (newTempRecord.LocationID == null)
//                    //    {
//                    //        if (newTempRecord.LocationName == null)
//                    //        {
//                    //            //ops we have not found it
//                    //            // strRejectReason = strRejectReason + " Location not found.";
//                    //        }
//                    //        else
//                    //        {

//                    //            if (newTempRecord.LocationName.Length > 0)
//                    //            {

//                    //                Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);

//                    //                if (tempLocation == null)
//                    //                {
//                    //                    Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);
//                    //                    {
//                    //                        if (tempLocationImp == null)
//                    //                        {
//                    //                            //ops we have not found it
//                    //                            strRejectReason = strRejectReason + " Location -" + newTempRecord.LocationName + " - is not found!";
//                    //                        }
//                    //                        else
//                    //                        {
//                    //                            newTempRecord.LocationID = tempLocationImp.LocationID;
//                    //                            newTempRecord.LocationName = tempLocationImp.LocationName;
//                    //                        }
//                    //                    }
//                    //                }
//                    //                else
//                    //                {
//                    //                    newTempRecord.LocationID = tempLocation.LocationID;
//                    //                    newTempRecord.LocationName = tempLocation.LocationName;
//                    //                }

//                    //            }
//                    //            else
//                    //            {
//                    //                //strRejectReason = strRejectReason + " Location not found.";
//                    //            }
//                    //        }
//                    //    }
//                    //    else
//                    //    {
//                    //        Location tempLocation = SiteManager.ets_Location_Details((int)newTempRecord.LocationID, ref connection, ref tn);

//                    //        if (tempLocation == null)
//                    //        {
//                    //            // strRejectReason = strRejectReason + " Location not found.";
//                    //        }
//                    //        else
//                    //        {
//                    //            newTempRecord.LocationID = tempLocation.LocationID;
//                    //            newTempRecord.LocationName = tempLocation.LocationName;
//                    //        }
//                    //    }

//                    //}
//                    //else
//                    //{
//                    //    newTempRecord.LocationID = int.Parse(strLocationID);
//                    //    newTempRecord.LocationName = strLocation;
//                    //}

//                    if (newTempRecord.DateTimeRecorded == null)
//                    {
//                        newTempRecord.DateTimeRecorded = DateTime.Now;

//                    }


//                    strRejectReason = strRejectReason.Trim();
//                    strWarningReason = strWarningReason.Trim();
//                    strExceedanceReason = strExceedanceReason.Trim();


//                    if (strRejectReason.Length > 0)
//                    {
//                        newTempRecord.RejectReason = strRejectReason.Trim();
//                    }

//                    if (strWarningReason.Length > 0)
//                    {
//                        newTempRecord.WarningReason = strWarningReason.Trim();
//                    }

//                    if (strExceedanceReason.Length > 0)
//                    {
//                        newTempRecord.WarningReason = newTempRecord.WarningReason == "" ? strExceedanceReason.Trim() : newTempRecord.WarningReason + " " + strExceedanceReason.Trim();

//                    }


//                    int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord,null,null);


//                }
//                if (strUniqueColumnIDSys!="" || strUniqueColumnID2Sys!="")
//                {
//                    RecordManager.ets_Batch_Duplicate(iBatchID,strUniqueColumnIDSys,strUniqueColumnID2Sys, tn);
//                }

//                try
//                {
//                    //if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
//                    //    File.Delete(strImportFolder + "\\" + strFileUniqueName);
//                }
//                catch
//                {

//                }
//                //Response.Redirect("~/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=" + Cryptography.Encrypt(iBatchID.ToString()), false);
//            }
//            catch (Exception ex)
//            {
//                //ErrorLog theErrorLog = new ErrorLog(null, "Record Upload", ex.Message, ex.StackTrace, DateTime.Now, strTemp);


//                if (ex.Message.IndexOf("DateTime") > -1)
//                {
//                    strMsg = "Date Recorded data are not valid, please review the file data.";
//                }
//                else if (ex.Message.IndexOf("recognized") > -1)
//                {
//                    strMsg = "Unknown error occurred please review your import data.";
//                }
//                else if (ex.Message.IndexOf(strTimeSamledColumnName) > -1)
//                {
//                    strMsg = "The file must have a Time Recorded column just after Date Recorded column.";
//                }
//                else
//                {
//                    strMsg = "UNKNOWN:" + ex.Message + ex.StackTrace;
//                    //SystemData.ErrorLog_Insert(theErrorLog);
//                }

//                //if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
//                //    File.Delete(strImportFolder + "\\" + strFileUniqueName);
//                //throw;
//            }
//        }
//        else
//        {

//            //now lets play with column position
//            //try
////            {

////                strMsg = "";

////                bool bIsDateSingleColumn = false;
////                int iDatePosition = 1;
////                DataTable dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      IsDateSingleColumn,
////                        PositionOnImport FROM  [Column]
////                        WHERE SystemName='DateTimeRecorded' AND TableID=" + theTable.TableID.ToString());
////                if (dtDateTimeColumnName.Rows.Count > 0 && dtDateTimeColumnName.Rows[0][0] != DBNull.Value)
////                {

////                    if (dtDateTimeColumnName.Rows[0]["IsDateSingleColumn"].ToString().ToLower() == "true")
////                    {
////                        bIsDateSingleColumn = true;
////                    }
////                    iDatePosition = int.Parse(dtDateTimeColumnName.Rows[0]["PositionOnImport"].ToString());
////                }


////                DataTable dtImportFileTable;
////                dtImportFileTable = null;

////                switch (strFileExtension.ToLower())
////                {

////                    case ".csv":
////                        dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName, ref strMsg);
////                        z = 1;
////                        break;
////                    case ".xls":
////                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, true);
////                        break;
////                    case ".xlsx":
////                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, true);
////                        break;

////                }
////                if (strMsg != "")
////                {
////                    return;
////                }

////                //Remove rows

////                if (theTable.ImportDataStartRow != null)
////                {
////                    for (int i = 1; i <= (int)theTable.ImportDataStartRow - 2; i++)
////                    {
////                        dtImportFileTable.Rows.RemoveAt(0);

////                    }
////                    dtImportFileTable.AcceptChanges();

////                }






////                DataTable dtRecordTypleColumns = RecordManager.ets_Table_Import_Position((int)theTable.TableID);


////                string strListOfNoNeedColumns = "";
////                for (int i = 0; i < dtImportFileTable.Columns.Count; i++)
////                {
////                    bool bIsFound = false;
////                    for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
////                    {
////                        if (i == (int.Parse(dtRecordTypleColumns.Rows[j]["PositionOnImport"].ToString()) - 1))
////                        {
////                            bIsFound = true;
////                            dtImportFileTable.Columns[i].ColumnName = dtRecordTypleColumns.Rows[j]["SystemName"].ToString();
////                            break;
////                        }
////                    }
////                    if (bIsFound == false)
////                    {

////                        for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
////                        {
////                            if (i == (iDatePosition + 1 - 1))
////                            {
////                                bIsFound = true;
////                                dtImportFileTable.Columns[i].ColumnName = "Time Recorded";
////                                break;
////                            }
////                        }

////                        if (dtImportFileTable.Columns[i].ColumnName.ToLower() != "time recorded")
////                        {
////                            strListOfNoNeedColumns += dtImportFileTable.Columns[i].ColumnName + ",";
////                        }
////                    }
////                }

////                List<string> strRemoveIndexes = strListOfNoNeedColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();


////                foreach (string item in strRemoveIndexes)
////                {
////                    dtImportFileTable.Columns.Remove(item);
////                }







////                //now dtCSV is ready to be imported into Batch & TempRecord
////                Batch newBatch = new Batch(null, (int)theTable.TableID,
////                    strBatchDescription.Trim() == "" ? strOriginalFileName : strBatchDescription.Trim(),
////                    strOriginalFileName, null, guidNew, iUserID, iAccountID, theTable.IsImportPositional);



////                newBatch.AllowDataUpdate = bAllowDataUpload;

////                iBatchID = UploadManager.ets_Batch_Insert(newBatch, null, null);





////                //AddMissingLocation
////                //if (theTable.AddMissingLocation != null && (bool)theTable.AddMissingLocation)
////                //{
////                //    //lets find Location column
////                //    bool bFoundSS = false;
////                //    foreach (DataColumn dc in dtImportFileTable.Columns)
////                //    {
////                //        if (dc.ColumnName.ToUpper() == "LOCATIONID")
////                //        {
////                //            bFoundSS = true;
////                //        }

////                //    }

////                //    if (bFoundSS)
////                //    {
////                //        for (int r = z; r < dtImportFileTable.Rows.Count; r++)
////                //        {
////                //            string strSSName = dtImportFileTable.Rows[r]["LocationID"].ToString();
////                //            Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, strSSName, ref connection, ref tn);

////                //            if (tempLocation == null)
////                //            {
////                //                Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, strSSName, ref connection, ref tn);
////                //                {
////                //                    if (tempLocationImp == null)
////                //                    {
////                //                        //ops we have not found it so lets insert it

////                //                        try
////                //                        {
////                //                            Location newLocation = new Location(null, strSSName, "", "", true, null, null, null, null,
////                //                                (int)theTable.AccountID, "");

////                //                            int iNewLocationID = SiteManager.ets_Location_Insert(newLocation, tn, null);

////                //                            LocationTable newLocationTable = new LocationTable(null, iNewLocationID, (int)theTable.TableID, "", "");
////                //                            SiteManager.ets_LocationTable_Insert(newLocationTable, tn, null);
////                //                        }
////                //                        catch
////                //                        {
////                //                            //do nothing
////                //                        }
////                //                    }
////                //                }
////                //            }

////                //        }

////                //    }


////                //}//end AddMissingLocation







////                for (int r = z; r < dtImportFileTable.Rows.Count; r++)
////                {
////                    TempRecord newTempRecord = new TempRecord();
////                    newTempRecord.AccountID = iAccountID;
////                    newTempRecord.BatchID = iBatchID;
////                    //bool bIsBlank = false;
////                    string strRejectReason = "";
////                    string strWarningReason = "";
////                    string strExceedanceReason = "";
////                    foreach (DataColumn dc in dtImportFileTable.Columns)
////                    {
////                        string strColumnName = "";
////                        strColumnName = dc.ColumnName;
////                        if (strColumnName.ToLower() != "time recorded")
////                        {
////                            //if (dc.ColumnName.ToUpper() == "LOCATIONID")
////                            //{
////                            //    strColumnName = "LocationName";
////                            //}

////                            if (dc.ColumnName.ToUpper() == "DATETIMERECORDED")
////                            {
////                                //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
////                                //{
////                                //    if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
////                                //    strRejectReason = strRejectReason + " Invalid Date Recorded.";
////                                //    //bIsBlank = true;
////                                //}
////                            }

////                            if (dc.ColumnName.ToUpper() == "DATETIMERECORDED")
////                            {
////                                newTempRecord.DateFormat = theTable.DateFormat;
////                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
////                                {
////                                    try
////                                    {
////                                        if (strFileExtension == ".csv")
////                                        {
////                                            if (bIsDateSingleColumn)
////                                            {

////                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
////                                            }
////                                            else
////                                            {
////                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() + " " + dtImportFileTable.Rows[r]["Time Recorded"].ToString());
////                                            }
////                                        }
////                                        else
////                                        {
////                                            if (bIsDateSingleColumn)
////                                            {

////                                                //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
////                                                //{
////                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
////                                                //}

////                                            }
////                                            else
////                                            {
////                                                string strDateTimeTemp = dtImportFileTable.Rows[r]["Time Recorded"].ToString();

////                                                if (dtImportFileTable.Rows[r]["Time Recorded"].ToString().Length > 10)
////                                                {
////                                                    strDateTimeTemp = strDateTimeTemp.Substring(11);
////                                                }
////                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
////                                                {

////                                                    //UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, 10) + " " + strDateTimeTemp);

////                                                    //if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 9)
////                                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ") != -1)
////                                                    {
////                                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ") > 1)
////                                                        {
////                                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, dtImportFileTable.Rows[r][dc.ColumnName].ToString().IndexOf(" ")) + " " + strDateTimeTemp);
////                                                        }

////                                                    }
////                                                    else
////                                                    {
////                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0) + " " + strDateTimeTemp);
////                                                    }
////                                                }
////                                            }

////                                        }
////                                    }
////                                    catch
////                                    {
////                                        if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
////                                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
////                                        //bIsBlank = true;
////                                    }
////                                }
////                                else
////                                {
////                                    if (strRejectReason == "")
////                                    {
////                                        if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
////                                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
////                                    }
////                                    else
////                                    {
////                                        if (strRejectReason.IndexOf("Date Recorded") > -1)
////                                        {
////                                        }
////                                        else
////                                        {
////                                            if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
////                                                strRejectReason = strRejectReason + " Invalid Date Recorded.";
////                                        }
////                                    }

////                                }
////                            }
////                            else
////                            {

////                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
////                            }


////                            for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
////                            {
////                                if (dc.ColumnName.ToLower() ==
////                                    dtRecordTypleColumns.Rows[i]["SystemName"].ToString().Trim().ToLower())
////                                {


////                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length == 0)
////                                    {
////                                        if (dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString() != "")
////                                        {
////                                            //dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

////                                            //UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());

////                                            if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "datetime"
////                                               || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "date"
////                                               || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "time")
////                                            {
////                                                dtImportFileTable.Rows[r][dc.ColumnName] = DateTime.Now;

////                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, DateTime.Now);
////                                            }
////                                            else
////                                            {

////                                                dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

////                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());
////                                            }

////                                            dtImportFileTable.AcceptChanges();
////                                        }

////                                    }




////                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 0)
////                                    {

////                                        //Not Empty



////                                        if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "datetime"
////                                            || dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "date")
////                                        {
////                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
////                                            {
////                                                try
////                                                {
////                                                    Convert.ToDateTime(dtImportFileTable.Rows[r][dc.ColumnName].ToString());
////                                                }
////                                                catch
////                                                {
////                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
////                                                }

////                                            }

////                                        }

////                                        if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "time")
////                                        {
////                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
////                                            {
////                                                try
////                                                {
////                                                    Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + dtImportFileTable.Rows[r][dc.ColumnName].ToString());
////                                                }
////                                                catch
////                                                {
////                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
////                                                }

////                                            }

////                                        }



////                                        //if (dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString() != "")
////                                        //{
////                                        //    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
////                                        //    {
////                                        //        if (!UploadManager.IsDataIntoDropDown(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString()))
////                                        //        {
////                                        //            strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

////                                        //        }

////                                        //    }

////                                        //}



////                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"] != DBNull.Value)
////                                        {
////                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
////                                            {
////                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
////                                                {

////                                                }
////                                                else
////                                                {
////                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
////                                                }
////                                            }

////                                        }

////                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"] != DBNull.Value)
////                                        {
////                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
////                                            {
////                                                if (!UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
////                                                {
////                                                    strWarningReason = strWarningReason + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
////                                                }
////                                                else
////                                                {

////                                                }
////                                            }

////                                        }

////                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"] != DBNull.Value)
////                                        {
////                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
////                                            {
////                                                if (!UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnExceedance"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
////                                                {
////                                                    strExceedanceReason = strExceedanceReason + " EXCEEDANCE: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
////                                                }
////                                                else
////                                                {

////                                                }
////                                            }
////                                        }



////                                        //check SD
////                                        string strData = dtImportFileTable.Rows[r][dc.ColumnName].ToString();
////                                        if (bool.Parse(dtRecordTypleColumns.Rows[i]["CheckUnlikelyValue"].ToString()))
////                                        {
////                                            int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

////                                            if (iCount >= Common.MinSTDEVRecords)
////                                            {
////                                                string strRecordedate;
////                                                if (dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
////                                                {
////                                                    strRecordedate = Common.IgnoreSymbols(strData);
////                                                }
////                                                else
////                                                {
////                                                    strRecordedate = strData;
////                                                }

////                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

////                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

////                                                double dRecordedate = double.Parse(strRecordedate);
////                                                if (dAVG != null && dSTDEV != null)
////                                                {
////                                                    dSTDEV = dSTDEV * 3;
////                                                    if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
////                                                    {
////                                                        //deviation happaned
////                                                        strWarningReason = strWarningReason + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
////                                                    }

////                                                }
////                                            }

////                                        }

////                                        //End SD


////                                    }
////                                    //else
////                                    //{
    ////                                    if (dtRecordTypleColumns.Rows[i]["Importance"].ToString().ToLower() == "m")
////                                    {
////                                        if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
////                                        {
////                                            strRejectReason = strRejectReason + " MANDATORY:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

////                                        }

////                                    }


////                                    //}
////                                    break;
////                                }
////                            }
////                        }
////                    }

////                    newTempRecord.TableID = (int)theTable.TableID;

////                    //if (strLocationID == "-1")
////                    //{
////                    //    if (newTempRecord.LocationID == null)
////                    //    {
////                    //        if (newTempRecord.LocationName == null)
////                    //        {
////                    //            //ops we have not found it
////                    //            //strRejectReason = strRejectReason + " Location not found.";
////                    //        }
////                    //        else
////                    //        {

////                    //            if (newTempRecord.LocationName.Length > 0)
////                    //            {

////                    //                Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);

////                    //                if (tempLocation == null)
////                    //                {
////                    //                    Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);
////                    //                    {
////                    //                        if (tempLocationImp == null)
////                    //                        {
////                    //                            //ops we have not found it
////                    //                            strRejectReason = strRejectReason + " Location -" + newTempRecord.LocationName + " - is not found!";
////                    //                        }
////                    //                        else
////                    //                        {
////                    //                            newTempRecord.LocationID = tempLocationImp.LocationID;
////                    //                            newTempRecord.LocationName = tempLocationImp.LocationName;
////                    //                        }
////                    //                    }
////                    //                }
////                    //                else
////                    //                {
////                    //                    newTempRecord.LocationID = tempLocation.LocationID;
////                    //                    newTempRecord.LocationName = tempLocation.LocationName;
////                    //                }
////                    //            }
////                    //            else
////                    //            {
////                    //                // strRejectReason = strRejectReason + " Location not found.";
////                    //            }
////                    //        }
////                    //    }
////                    //    else
////                    //    {
////                    //        Location tempLocation = SiteManager.ets_Location_Details((int)newTempRecord.LocationID, ref connection, ref tn);

////                    //        if (tempLocation == null)
////                    //        {
////                    //            //strRejectReason = strRejectReason + " Location not found.";
////                    //        }
////                    //        else
////                    //        {
////                    //            newTempRecord.LocationID = tempLocation.LocationID;
////                    //            newTempRecord.LocationName = tempLocation.LocationName;
////                    //        }
////                    //    }

////                    //}
////                    //else
////                    //{
////                    //    newTempRecord.LocationID = int.Parse(strLocationID);
////                    //    newTempRecord.LocationName = strLocation;
////                    //}
////                    if (newTempRecord.DateTimeRecorded == null)
////                    {
////                        newTempRecord.DateTimeRecorded = DateTime.Now;
////                        //if (strRejectReason == "")
////                        //{
////                        //    if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
////                        //        strRejectReason = strRejectReason + " Invalid Date Recorded.";
////                        //}
////                        //else
////                        //{
////                        //    if (strRejectReason.IndexOf("Date Recorded") > -1)
////                        //    {
////                        //    }
////                        //    else
////                        //    {
////                        //        if (strRejectReason.IndexOf("Invalid Date Recorded") == -1)
////                        //            strRejectReason = strRejectReason + " Invalid Date Recorded.";
////                        //    }
////                        //}
////                    }

////                    //if (strRejectReason == "")
////                    //{
////                    //    if (newTempRecord.LocationID != null)
////                    //    {
////                    //        if (RecordManager.ets_Record_IsDuplicate(newTempRecord, ref connection, ref tn))
////                    //        {
////                    //            strRejectReason = strRejectReason + " DUPLICATE Record!";
////                    //        }
////                    //    }
////                    //}


////                    if (strRejectReason.Length > 0)
////                    {
////                        newTempRecord.RejectReason = strRejectReason.Trim();
////                    }

////                    if (strWarningReason.Length > 0)
////                    {
////                        newTempRecord.WarningReason = strWarningReason.Trim();
////                    }

////                    if (strExceedanceReason.Length > 0)
////                    {
////                        newTempRecord.WarningReason = newTempRecord.WarningReason == "" ? strExceedanceReason.Trim() : newTempRecord.WarningReason + " " + strExceedanceReason.Trim();
////                    }
////                    //check blank line

////                    int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord, ref connection, ref tn);

////                }

////                if (strUniqueColumnIDSys != "" || strUniqueColumnID2Sys != "")
////                {
////                    RecordManager.ets_Batch_Duplicate(iBatchID, strUniqueColumnIDSys, strUniqueColumnID2Sys, tn);
////                }

////                try
////                {
////                    //if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
////                    //    File.Delete(strImportFolder + "\\" + strFileUniqueName);
////                }
////                catch
////                {

////                }
////                //Response.Redirect("~/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=" + Cryptography.Encrypt(iBatchID.ToString()), false);
////            }
//            //catch (Exception ex)
//            //{
//            //    //ErrorLog theErrorLog = new ErrorLog(null, "Record Upload", ex.Message, ex.StackTrace, DateTime.Now, strTemp);


//            //    if (ex.Message.IndexOf("DateTime") > -1)
//            //    {
//            //        strMsg = "Date Recorded data are not valid, please review the file data.";
//            //    }
//            //    else if (ex.Message.IndexOf("string was not recognized") > -1)
//            //    {
//            //        strMsg = "Unknown error occurred please review your import data.";
//            //    }
//            //    else if (ex.Message.IndexOf("Time Recorded") > -1)
//            //    {
//            //        strMsg = "The file must have a 'Time Recorded' column just after 'Date Recorded' column.";
//            //    }
//            //    else if (ex.Message.IndexOf("must refer to a location within the string") > -1)
//            //    {
//            //        strMsg = "The file columns are not well positioned.";
//            //    }
//            //    else
//            //    {
//            //        strMsg = "UNKNOWN:" + ex.Message + ex.StackTrace;
//            //        //SystemData.ErrorLog_Insert(theErrorLog);
//            //    }

//            //    //if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
//            //    //    File.Delete(strImportFolder + "\\" + strFileUniqueName);
//            //    //throw;
//            //}

//        }


//    }





    public static int ets_Batch_Update(Batch p_Batch)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Batch_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nBatchID", p_Batch.BatchID));


                if (p_Batch.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_Batch.TableID));

                command.Parameters.Add(new SqlParameter("@sBatchDescription", p_Batch.BatchDescription));

                if (p_Batch.UploadedFileName != "")
                    command.Parameters.Add(new SqlParameter("@sUploadedFileName", p_Batch.UploadedFileName));

                command.Parameters.Add(new SqlParameter("@dDateAdded", p_Batch.DateAdded));
                command.Parameters.Add(new SqlParameter("@nUniqueName", p_Batch.UniqueName));


                command.Parameters.Add(new SqlParameter("@nUserIDUploaded", p_Batch.UserIDUploaded));


                command.Parameters.Add(new SqlParameter("@nAccountID ", p_Batch.AccountID));

                if (p_Batch.IsImportPositional != null)
                    command.Parameters.Add(new SqlParameter("@bIsImportPositional", p_Batch.IsImportPositional));



                if (p_Batch.IsImported != null)
                    command.Parameters.Add(new SqlParameter("@bIsImported", p_Batch.IsImported));

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

    public static int ets_Batch_Delete(int nBatchID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Batch_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));

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



    public static int SPAfterImport(int nBatchID, string strSPAfterImport)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand(strSPAfterImport, connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));
                

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
                return -1;
            }
        }
    }


    public static int spUpdateExistingData(int nTableID, string sUniqueField, int nCurrentBatchID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spUpdateExistingData", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                command.Parameters.Add(new SqlParameter("@sUniqueField", sUniqueField));
                command.Parameters.Add(new SqlParameter("@nCurrentBatchID", nCurrentBatchID));

                SqlParameter pRV = new SqlParameter("@nUpdatedRowCount", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
                command.Parameters.Add(pRV);

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

    public static int spCheckLinkedValueExist(int nBatchID, string sSystemName, int nTableTableID, string sParentSystemName)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spCheckLinkedValueExist", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));
                command.Parameters.Add(new SqlParameter("@sSystemName", sSystemName));
                command.Parameters.Add(new SqlParameter("@nTableTableID", nTableTableID));
                command.Parameters.Add(new SqlParameter("@sParentSystemName", sParentSystemName));


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



    public static int spAdjustTempRecordLinkedValueOnImport(int nBatchID, string sSystemName, int nTableTableID, string sParentSystemName)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spAdjustTempRecordLinkedValueOnImport", connection))
            {
                command.CommandTimeout = 3600;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));
                command.Parameters.Add(new SqlParameter("@sSystemName", sSystemName));
                command.Parameters.Add(new SqlParameter("@nTableTableID", nTableTableID));
                command.Parameters.Add(new SqlParameter("@sParentSystemName", sParentSystemName));


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
    public static int ets_Record_ImportByBatch(int nBatchID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_Record_ImportByBatch", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));

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


    public static int ets_Record_UpdateByBatch(int nBatchID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_UpdateByBatch", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));
                SqlParameter pRV = new SqlParameter("@nUpdatedRowCount", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;
                command.Parameters.Add(pRV);

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
                if (i != -1 && pRV!=null)
                {
                    i = int.Parse(pRV.Value.ToString());
                }
                return i;
            }
        }
    }



    public static DataTable ets_Records_By_Batch(int nBatchID, int nTableID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("ets_Records_By_Batch", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));
                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));


                // connection.Open();
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



 



    public static DataTable ets_Records_By_BatchForEmail(int nBatchID, int nTableID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Records_By_BatchForEmail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //command.Transaction = tn;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));
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


    public static string ImportTemplate_SPName(string spName, int? BatchID, int? UserID)
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
                if (BatchID != null)
                    command.Parameters.Add(new SqlParameter("@BatchID", BatchID));



                if (UserID != null)
                    command.Parameters.Add(new SqlParameter("@UserID", UserID));



                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    strValue=pRV.Value.ToString();
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return strValue;
            }

        }



        
    }


    public static string ImportClickFucntions(Batch oBatch)
    {


        int iImportOk = 0;


        try
        {
            //insert Records from tempRecord to Record 
            iImportOk = UploadManager.ets_Record_ImportByBatch((int)oBatch.BatchID);

            //return "ok"; //for now


            bool bHasAvg = false;


            //string strExecuteLot = "";


            if (iImportOk == 1)
            {
                Table theTable = RecordManager.ets_Table_Details((int)oBatch.TableID);

                if (oBatch.AllowDataUpdate.HasValue && oBatch.AllowDataUpdate.Value &&
                    theTable.IsDataUpdateAllowed.HasValue && theTable.IsDataUpdateAllowed.Value)
                {
                    UploadManager.ets_Record_UpdateByBatch((int)oBatch.BatchID);


                }
                    

                DataTable dtRecordTypleColumns = RecordManager.ets_Table_Columns_All((int)oBatch.TableID);

                // UPDATE AVG   - Need to review it with JB
                for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
                {

                    if (dtRecordTypleColumns.Rows[i]["NumberType"] != null)
                    {
                        if (dtRecordTypleColumns.Rows[i]["NumberType"].ToString() == "4")
                        {
                            bHasAvg = true;
                        }
                    }
                }
                bHasAvg = false;//TEMP
                if (bHasAvg)
                {
                    DataTable dtTempAvg = Common.DataTableFromText(@"SELECT  RecordID From [Record] BatchID="
                                 + oBatch.BatchID.ToString());

                    foreach (DataRow dr in dtTempAvg.Rows)
                    {
                        RecordManager.ets_Record_Avg_ForARecordID(int.Parse(dr["RecordID"].ToString()));
                    }

                }

            }
            else
            {
                //NOT OK, NEED TO find out why
                return "Please try again!";
            }

            //Everything ok
            try
            {
                Usage theUsage = new Usage(null, oBatch.AccountID, DateTime.Now, 0, 1);
                SecurityManager.Usage_Insert(theUsage);
            }
            catch
            {
                //
            }
            return "ok";
        }
        catch (Exception ex)
        {

            return "System failed to import this batch!" + ex.Message + ex.StackTrace;
            //throw;
        }
    }




    //  END AVG


    // DataTable dtDatas;


    //UPLOAD CSV
    //for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
    //{
    //    //update constant field 
    //    if (  dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "number"
    //        && dtRecordTypleColumns.Rows[i]["NumberType"] != null && dtRecordTypleColumns.Rows[i]["NumberType"].ToString() == "2"
    //        && dtRecordTypleColumns.Rows[i]["Constant"]!=DBNull.Value && dtRecordTypleColumns.Rows[i]["Constant"].ToString() != "")
    //    {                       
    //        strExecuteLot = strExecuteLot + "UPDATE Record SET " + dtRecordTypleColumns.Rows[i]["SystemName"].ToString()
    //            + "='" + dtRecordTypleColumns.Rows[i]["Constant"].ToString().Replace("'", "''")
    //            + "' WHERE BatchID="
    //            + oBatch.BatchID.ToString() + ";";
    //    }                                  

    //}

    //if (strExecuteLot != "")
    //    Common.ExecuteText(strExecuteLot);

    //load Records data


    //bool bIsAnyCalculationField = false;

    //dtDatas = UploadManager.ets_Records_By_Batch((int)oBatch.BatchID, (int)oBatch.TableID);


    //for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
    //{                  
    //    if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "calculation"
    //                                && dtRecordTypleColumns.Rows[i]["Calculation"] != DBNull.Value 
    //                                && dtRecordTypleColumns.Rows[i]["Calculation"].ToString()!="")
    //    {
    //        bIsAnyCalculationField = true;

    //    }
    //}

    // if (bIsAnyCalculationField)
    //     dtDatas = UploadManager.ets_Records_By_Batch((int)oBatch.BatchID, (int)oBatch.TableID);
    // //loop around each column
    //if(bIsAnyCalculationField && dtDatas!=null && dtDatas.Rows.Count>0)
    //{
    //    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
    //    {
    //bool bCalculation = false;
    ////perform calculation for this column (if found)
    //if (dtRecordTypleColumns.Rows[i]["ColumnType"].ToString() == "calculation"
    //                            && dtRecordTypleColumns.Rows[i]["Calculation"] != DBNull.Value
    //                            && dtRecordTypleColumns.Rows[i]["Calculation"].ToString() != "")
    //{
    //    strExecuteLot = "";
    //    if (dtDatas.Rows.Count > 0)
    //    {
    //        string strTempFormula = "";
    //        strTempFormula = dtRecordTypleColumns.Rows[i]["Calculation"].ToString().ToLower();

    //        bool bDateCalculation = false;
    //        if (dtRecordTypleColumns.Rows[i]["TextType"] != DBNull.Value
    //                && dtRecordTypleColumns.Rows[i]["TextType"].ToString().ToLower() == "d")
    //        {
    //            bDateCalculation = true;
    //        }
    //        else
    //        {
    //            strTempFormula = TheDatabaseS.GetCalculationFormula((int)oBatch.TableID, strTempFormula,null,null);
    //        }



    //        string strResult = "";
    //        foreach (DataRow dr in dtDatas.Rows)
    //        {
    //            //process a record
    //            Record theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(dr["DBGSystemRecordID"].ToString()));
    //            bool bNeedUpdate = false;
    //            try
    //            {

    //                if (bDateCalculation)
    //                {
    //                    strResult = TheDatabaseS.GetDateCalculationResult(dtRecordTypleColumns, strTempFormula, null, theRecord, null,
    //                      dtRecordTypleColumns.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : dtRecordTypleColumns.Rows[i]["DateCalculationType"].ToString(),
    //                      null,null);
    //                }
    //                else
    //                {
    //                    strResult = TheDatabaseS.GetCalculationResult(dtRecordTypleColumns, strTempFormula, null, theRecord, null,null,null);
    //                }
    //            }
    //            catch
    //            {
    //                //
    //            }

    //            if (strResult == "" || strResult.IndexOf("Infinity") > -1)
    //            {

    //                // strExecuteLot = strExecuteLot + "UPDATE Record SET " + dtRecordTypleColumns.Rows[i]["SystemName"].ToString() + "=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
    //            }
    //            else
    //            {
    //                strExecuteLot = strExecuteLot + "UPDATE Record SET " + dtRecordTypleColumns.Rows[i]["SystemName"].ToString() + "='" + strResult.Replace("'", "''") + "'  WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
    //            }
    //        }

    //        if (strExecuteLot != "")
    //        {
    //            bCalculation = true;
    //            try
    //            {
    //                Common.ExecuteText(strExecuteLot);
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }
    //    }
    //}

    //calculation done



    //string strTemp = "";
    //string strTempWarning = "";
    //double dWarningRecords = 0;

    //reload data as calculation field updated
    //if (bCalculation)
    //    dtDatas = UploadManager.ets_Records_By_Batch((int)oBatch.BatchID, (int)oBatch.TableID);



    ////check warning for calculation field

    //if (bCalculation && dtRecordTypleColumns.Rows[i]["ValidationOnWarning"] != DBNull.Value && dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString() != "")
    //{
    //    if (dtDatas.Rows.Count > 0)
    //    {
    //        strExecuteLot = "";
    //        foreach (DataRow dr in dtDatas.Rows)
    //        {
    //            strTempWarning = dr["WarningResults"].ToString();

    //            strTempWarning = strTempWarning.Replace("WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.", "");

    //            //check warning
    //            bool bWarningFound = false;
    //            if (UploadManager.IsDataValid(dr[dtRecordTypleColumns.Rows[i]["DisplayName"].ToString()].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString()), null))
    //            {
    //                //Warning

    //                strTempWarning = strTempWarning + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
    //                bWarningFound = true;
    //            }
    //            else
    //            {
    //                //
    //            }


    //            //check SD
    //            string strData = dr[dtRecordTypleColumns.Rows[i]["DisplayName"].ToString()].ToString();
    //            if (strData != "" && bool.Parse(dtRecordTypleColumns.Rows[i]["CheckUnlikelyValue"].ToString()))
    //            {
    //                int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), int.Parse(dr["DBGSystemRecordID"].ToString()), null, null);

    //                if (iCount >= Common.MinSTDEVRecords)
    //                {
    //                    string strRecordedate;
    //                    if (dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
    //                    {
    //                        strRecordedate = Common.IgnoreSymbols(strData);
    //                    }
    //                    else
    //                    {
    //                        strRecordedate = strData;
    //                    }

    //                    double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),
    //                        int.Parse(dr["DBGSystemRecordID"].ToString()), null, null);

    //                    double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),
    //                        int.Parse(dr["DBGSystemRecordID"].ToString()), null, null);

    //                    double dRecordedate = double.Parse(strRecordedate);
    //                    if (dAVG != null && dSTDEV != null)
    //                    {
    //                        dSTDEV = dSTDEV * 3;
    //                        if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
    //                        {
    //                            //deviation happaned
    //                            strTempWarning = strTempWarning + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
    //                        }

    //                    }
    //                }

    //            }

    //            //End SD


    //            if (strTempWarning.Trim().Length > 0)
    //            {
    //                //Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''").Trim() + "' WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
    //                strExecuteLot = strExecuteLot + "UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''").Trim() + "' WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
    //            }
    //            else
    //            {
    //                //Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
    //                strExecuteLot = strExecuteLot + "UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
    //            }

    //            if (bWarningFound)
    //            {
    //                dWarningRecords = dWarningRecords + 1;
    //            }
    //        }

    //        if (strExecuteLot != "")
    //        {
    //            Common.ExecuteText(strExecuteLot);
    //        }

    //    }

    //    if (dWarningRecords > 0)
    //    {
    //        //warning happend for this column, so lets email it

    //    }


    //}


    //if (bCalculation && dtRecordTypleColumns.Rows[i]["CheckUnlikelyValue"] != DBNull.Value && bool.Parse(dtRecordTypleColumns.Rows[i]["CheckUnlikelyValue"].ToString()))
    //{
    //    //reload
    //    dtDatas = UploadManager.ets_Records_By_Batch((int)oBatch.BatchID, (int)oBatch.TableID);


    //}




    //calculated column's warning DONE




    //check warning for calculation field

    //if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString() != "" && dtRecordTypleColumns.Rows[i]["Calculation"].ToString() != "")
    //{
    //    if (dtDatas.Rows.Count > 0)
    //    {
    //        strExecuteLot = "";
    //        foreach (DataRow dr in dtDatas.Rows)
    //        {
    //            strTempWarning = dr["WarningResults"].ToString();

    //            strTempWarning = strTempWarning.Replace("WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.", "");

    //            //check warning
    //            bool bWarningFound = false;
    //            if (UploadManager.IsDataValid(dr[dtRecordTypleColumns.Rows[i]["DisplayName"].ToString()].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
    //            {
    //                //Warning

    //                strTempWarning = strTempWarning + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
    //                bWarningFound = true;
    //            }
    //            else
    //            {
    //                //
    //            }


    //            //check SD
    //            string strData = dr[dtRecordTypleColumns.Rows[i]["DisplayName"].ToString()].ToString();
    //            if (strData != "" && bool.Parse(dtRecordTypleColumns.Rows[i]["CheckUnlikelyValue"].ToString()))
    //            {
    //                int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), int.Parse(dr["DBGSystemRecordID"].ToString()));

    //                if (iCount >= Common.MinSTDEVRecords)
    //                {
    //                    string strRecordedate;
    //                    if (dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
    //                    {
    //                        strRecordedate = Common.IgnoreSymbols(strData);
    //                    }
    //                    else
    //                    {
    //                        strRecordedate = strData;
    //                    }

    //                    double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),
    //                        int.Parse(dr["DBGSystemRecordID"].ToString()));

    //                    double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),
    //                        int.Parse(dr["DBGSystemRecordID"].ToString()));

    //                    double dRecordedate = double.Parse(strRecordedate);
    //                    if (dAVG != null && dSTDEV != null)
    //                    {
    //                        dSTDEV = dSTDEV * 3;
    //                        if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
    //                        {
    //                            //deviation happaned
    //                            strTempWarning = strTempWarning + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
    //                        }

    //                    }
    //                }

    //            }

    //            //End SD


    //            if (strTempWarning.Trim().Length > 0)
    //            {
    //                //Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''").Trim() + "' WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
    //                strExecuteLot = strExecuteLot + "UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''").Trim() + "' WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
    //            }
    //            else
    //            {
    //                //Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
    //                strExecuteLot = strExecuteLot + "UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
    //            }

    //            if (bWarningFound)
    //            {
    //                dWarningRecords = dWarningRecords + 1;
    //            }


    //        }

    //        if (strExecuteLot != "")
    //        {
    //            Common.ExecuteText(strExecuteLot);
    //        }

    //    }

    //    if (dWarningRecords > 0)
    //    {
    //        //warning happend for this column, so lets email it

    //    }


    //}


    //calculated column's warning DONE


    //Check max time between Records
    //if (i == (dtDatas.Rows.Count - 1))//0 //do it only first/Last time 
    //{
    //    //reload data as warning field updated
    //    if (theTable.MaxTimeBetweenRecords != null && theTable.MaxTimeBetweenRecordsUnit != null)
    //    {
    //        dtDatas = UploadManager.ets_Records_By_Batch((int)oBatch.BatchID, (int)oBatch.TableID);

    //        if (dtDatas.Rows.Count > 0)
    //        {

    //            foreach (DataRow dr in dtDatas.Rows)
    //            {
    //                strTempWarning = dr["WarningResults"].ToString();

    //                if (!RecordManager.IsTimeBetweenRecordOK((int)oBatch.TableID, (double)theTable.MaxTimeBetweenRecords, theTable.MaxTimeBetweenRecordsUnit, int.Parse(dr["DBGSystemRecordID"].ToString()), DateTime.Parse(dr["DateTimeRecorded"].ToString())))
    //                {
    //                    strTempWarning = strTempWarning + " WARNING: " + WarningMsg.MaxtimebetweenRecords + "!";

    //                }

    //                if (strTempWarning.Trim().Length > 0)
    //                {
    //                    Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''").Trim() + "' WHERE RecordID=" + dr["DBGSystemRecordID"].ToString());
    //                }
    //                else
    //                {
    //                    Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString());
    //                }

    //            }

    //        }
    //    }
    //}



    //check linked columns

    //                       DataTable dtLinkedColumns = Common.DataTableFromText(@"SELECT ColumnID,SystemName,ColumnType,NumberType FROM [Column]
    //                            WHERE TableID=" + theTable.TableID.ToString() + @" AND ColumnID IN (SELECT DISTINCT LinkedParentColumnID FROM [Column] INNER JOIN [Table] ON
    //                            [Table].TableID=[Column].TableID WHERE [Table].AccountID=" + theTable.AccountID.ToString() + ")");

    //                       if (dtLinkedColumns.Rows.Count > 0)
    //                       {

    //                           strExecuteLot = "";
    //                           foreach (DataRow drLC in dtLinkedColumns.Rows)
    //                           {
    //                               DataTable dtRecords = Common.DataTableFromText("SELECT RecordID FROM Record WHERE " + drLC["SystemName"].ToString() + " IS NULL AND TableID=" + theTable.TableID.ToString());

    //                               if (drLC["ColumnType"].ToString().ToLower() == "number")
    //                               {
    //                                   if (drLC["NumberType"].ToString().ToLower() == "8")
    //                                   {

    //                                       string strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + drLC["SystemName"].ToString() + ")) FROM Record WHERE IsNumeric(" + drLC["SystemName"].ToString() + ")=1 and  TableID=" + theTable.TableID.ToString());
    //                                       string strValue = "";
    //                                       if (strMax == "")
    //                                       {
    //                                           strValue = "1";
    //                                       }
    //                                       else
    //                                       {
    //                                           strValue = (int.Parse(strMax) + 1).ToString();
    //                                       }

    //                                       foreach (DataRow drR in dtRecords.Rows)
    //                                       {

    //                                           strExecuteLot = strExecuteLot + "UPDATE Record SET " + drLC["SystemName"].ToString() + "='" + strValue + "' WHERE RecordID=" + drR["RecordID"].ToString() + ";";

    //                                           strValue = (int.Parse(strValue) + 1).ToString();
    //                                       }

    //                                   }
    //                               }
    //                               else
    //                               {
    //                                   foreach (DataRow drR in dtRecords.Rows)
    //                                   {
    //                                       Guid newGUID = Guid.NewGuid();
    //                                       strExecuteLot = strExecuteLot + "UPDATE Record SET " + drLC["SystemName"].ToString() + "='" + newGUID.ToString() + "' WHERE RecordID=" + drR["RecordID"].ToString() + ";";
    //                                   }
    //                               }

    //                           }
    //                           if (strExecuteLot != "")
    //                           {
    //                               Common.ExecuteText(strExecuteLot);
    //                           }

    //                       }

    //    }
    //}






//    public static string ImportClickFucntions(Batch oBatch, ref SqlConnection connection, ref SqlTransaction tn)
//    {


//        int iImportOk = 0;


//        try
//        {
//            //insert Records from tempRecord to Record 
//            iImportOk = UploadManager.ets_Record_ImportByBatch((int)oBatch.BatchID, ref connection, ref tn);

//            bool bHasAvg = false;


//            string strExecuteLot = "";

//            Table theTable = RecordManager.ets_Table_Details((int)oBatch.TableID, ref connection, ref tn);
//            if (iImportOk == 1)
//            {

//                //DataTable dtRecordTypleColumns = RecordManager.ets_Table_Columns_Detail((int)oBatch.TableID);
//                DataTable dtRecordTypleColumns = RecordManager.ets_Table_Columns_All((int)oBatch.TableID, tn, null);
//                //update constant values

//                for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                {
//                    //update constant field 
//                    if (dtRecordTypleColumns.Rows[i]["Constant"].ToString() != "")
//                    {
//                        //Common.ExecuteText("UPDATE Record SET Record." + dtRecordTypleColumns.Rows[i]["SystemName"].ToString()
//                        //    + "='" + dtRecordTypleColumns.Rows[i]["Constant"].ToString().Replace("'", "''")
//                        //    + "' From [Record]  INNER JOIN TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN Batch ON TempRecord.BatchID = Batch.BatchID   WHERE Batch.BatchID="
//                        //    + oBatch.BatchID.ToString(), ref connection, ref tn);

//                        strExecuteLot = strExecuteLot + "UPDATE Record SET Record." + dtRecordTypleColumns.Rows[i]["SystemName"].ToString()
//                            + "='" + dtRecordTypleColumns.Rows[i]["Constant"].ToString().Replace("'", "''")
//                            + "' From [Record]  INNER JOIN TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN Batch ON TempRecord.BatchID = Batch.BatchID   WHERE Batch.BatchID="
//                            + oBatch.BatchID.ToString() + ";";
//                    }

//                    // UPDATE AVG                  

//                    if (dtRecordTypleColumns.Rows[i]["NumberType"] != null)
//                    {
//                        if (dtRecordTypleColumns.Rows[i]["NumberType"].ToString() == "4")
//                        {

//                            bHasAvg = true;

//                        }
//                    }


//                }

//                if (strExecuteLot != "")
//                    Common.ExecuteText(strExecuteLot, ref connection, ref tn);

//                //load Records data

//                DataTable dtDatas = UploadManager.ets_Records_By_Batch((int)oBatch.BatchID, (int)oBatch.TableID, ref connection, ref tn);


//                if (bHasAvg)
//                {
//                    DataTable dtTempAvg = Common.DataTableFromText(@"SELECT  [Record].RecordID From [Record]  INNER JOIN TempRecord 
//                            ON Record.TempRecordID = TempRecord.RecordID INNER JOIN Batch ON TempRecord.BatchID = Batch.BatchID   WHERE Batch.BatchID="
//                                 + oBatch.BatchID.ToString(), tn, null);

//                    foreach (DataRow dr in dtTempAvg.Rows)
//                    {
//                        RecordManager.ets_Record_Avg_ForARecordID(int.Parse(dr["RecordID"].ToString()), tn);
//                    }

//                }


//                //loop around each column
//                for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                {
//                    bool bCalculation = false;

//                    //perform calculation for this column (if found)
//                    if (dtRecordTypleColumns.Rows[i]["Calculation"].ToString() != "")
//                    {
//                        strExecuteLot = "";
//                        if (dtDatas.Rows.Count > 0)
//                        {
//                            string strTempFormula = "";
//                            string strResult = "";
//                            foreach (DataRow dr in dtDatas.Rows)
//                            {
//                                //process a record
//                                strTempFormula = dtRecordTypleColumns.Rows[i]["Calculation"].ToString().ToLower();
//                                for (int j = 0; j < dtDatas.Columns.Count; j++)
//                                {
//                                    strTempFormula = strTempFormula.Replace("[" + dtDatas.Columns[j].ColumnName.ToLower() + "]", Common.MakeDecimal(Common.IgnoreSymbols(dr[j].ToString())));
//                                }
//                                strResult = RecordManager.CalculationResult(strTempFormula);

//                                if (strResult == "" || strResult.IndexOf("Infinity") > -1)
//                                {
//                                    //Common.ExecuteText("UPDATE Record SET " + dtRecordTypleColumns.Rows[i]["SystemName"].ToString() + "=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
//                                    strExecuteLot = strExecuteLot + "UPDATE Record SET " + dtRecordTypleColumns.Rows[i]["SystemName"].ToString() + "=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
//                                }
//                                else
//                                {
//                                    //Common.ExecuteText("UPDATE Record SET " + dtRecordTypleColumns.Rows[i]["SystemName"].ToString() + "=" + strResult + "  WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
//                                    strExecuteLot = strExecuteLot + "UPDATE Record SET " + dtRecordTypleColumns.Rows[i]["SystemName"].ToString() + "=" + strResult + "  WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
//                                }
//                            }

//                            if (strExecuteLot != "")
//                            {
//                                bCalculation = true;
//                                Common.ExecuteText(strExecuteLot, ref connection, ref tn);
//                            }

//                        }
//                    }

//                    //calculation done



//                    string strTemp = "";
//                    string strTempWarning = "";
//                    double dWarningRecords = 0;

//                    //reload data as calculation field updated
//                    if (bCalculation)
//                        dtDatas = UploadManager.ets_Records_By_Batch((int)oBatch.BatchID, (int)oBatch.TableID, ref connection, ref tn);


//                    //check warning for calculation field

//                    if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString() != "" && dtRecordTypleColumns.Rows[i]["Calculation"].ToString() != "")
//                    {
//                        if (dtDatas.Rows.Count > 0)
//                        {
//                            strExecuteLot = "";
//                            foreach (DataRow dr in dtDatas.Rows)
//                            {
//                                strTempWarning = dr["WarningResults"].ToString();

//                                strTempWarning = strTempWarning.Replace("WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.", "");

//                                //check warning
//                                bool bWarningFound = false;
//                                if (UploadManager.IsDataValid(dr[dtRecordTypleColumns.Rows[i]["DisplayName"].ToString()].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
//                                {
//                                    //Warning

//                                    strTempWarning = strTempWarning + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
//                                    bWarningFound = true;
//                                }
//                                else
//                                {
//                                    //
//                                }


//                                //check SD
//                                string strData = dr[dtRecordTypleColumns.Rows[i]["DisplayName"].ToString()].ToString();
//                                if (strData != "" && bool.Parse(dtRecordTypleColumns.Rows[i]["CheckUnlikelyValue"].ToString()))
//                                {
//                                    int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, int.Parse(dr["DBGSystemRecordID"].ToString()));

//                                    if (iCount >= Common.MinSTDEVRecords)
//                                    {
//                                        string strRecordedate;
//                                        if (dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
//                                        {
//                                            strRecordedate = Common.IgnoreSymbols(strData);
//                                        }
//                                        else
//                                        {
//                                            strRecordedate = strData;
//                                        }

//                                        double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),
//                                            ref connection, ref tn, int.Parse(dr["DBGSystemRecordID"].ToString()));

//                                        double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(),
//                                            ref connection, ref tn, int.Parse(dr["DBGSystemRecordID"].ToString()));

//                                        double dRecordedate = double.Parse(strRecordedate);
//                                        if (dAVG != null && dSTDEV != null)
//                                        {
//                                            dSTDEV = dSTDEV * 3;
//                                            if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
//                                            {
//                                                //deviation happaned
//                                                strTempWarning = strTempWarning + " WARNING: " + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
//                                            }

//                                        }
//                                    }

//                                }

//                                //End SD


//                                if (strTempWarning.Trim().Length > 0)
//                                {
//                                    //Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''").Trim() + "' WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
//                                    strExecuteLot = strExecuteLot + "UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''").Trim() + "' WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
//                                }
//                                else
//                                {
//                                    //Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
//                                    strExecuteLot = strExecuteLot + "UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString() + ";";
//                                }

//                                if (bWarningFound)
//                                {
//                                    dWarningRecords = dWarningRecords + 1;
//                                }


//                            }

//                            if (strExecuteLot != "")
//                            {
//                                Common.ExecuteText(strExecuteLot, ref connection, ref tn);
//                            }

//                        }

//                        if (dWarningRecords > 0)
//                        {
//                            //warning happend for this column, so lets email it

//                        }


//                    }


//                    //calculated column's warning DONE


//                    //Check max time between Records
//                    if (i == (dtDatas.Rows.Count - 1))//0 //do it only first/Last time 
//                    {
//                        //reload data as warning field updated
//                        if (theTable.MaxTimeBetweenRecords != null && theTable.MaxTimeBetweenRecordsUnit != null)
//                        {
//                            dtDatas = UploadManager.ets_Records_By_Batch((int)oBatch.BatchID, (int)oBatch.TableID, ref connection, ref tn);

//                            if (dtDatas.Rows.Count > 0)
//                            {

//                                foreach (DataRow dr in dtDatas.Rows)
//                                {
//                                    strTempWarning = dr["WarningResults"].ToString();

//                                    if (!RecordManager.IsTimeBetweenRecordOK((int)oBatch.TableID, (double)theTable.MaxTimeBetweenRecords, theTable.MaxTimeBetweenRecordsUnit, int.Parse(dr["DBGSystemRecordID"].ToString()), DateTime.Parse(dr["DateTimeRecorded"].ToString()), ref connection, ref tn))
//                                    {
    //                                        strTempWarning = strTempWarning + " WARNING: " + WarningMsg.MaxtimebetweenRecords + "!";

//                                    }

//                                    if (strTempWarning.Trim().Length > 0)
//                                    {
//                                        Common.ExecuteText("UPDATE Record SET WarningResults='" + strTempWarning.Replace("'", "''").Trim() + "' WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
//                                    }
//                                    else
//                                    {
//                                        Common.ExecuteText("UPDATE Record SET WarningResults=null WHERE RecordID=" + dr["DBGSystemRecordID"].ToString(), ref connection, ref tn);
//                                    }

//                                }

//                            }
//                        }
//                    }

//                    //check validation for calculation fields only

//                    if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString() != "" && dtRecordTypleColumns.Rows[i]["Calculation"].ToString() != "")
//                    {
//                        if (dtDatas.Rows.Count > 0)
//                        {

//                            string strRecordIDs = "";
//                            string strValidationError = "";
//                            double dInvalidCounter = 0;
//                            foreach (DataRow dr in dtDatas.Rows)
//                            {
//                                if (dr[dtRecordTypleColumns.Rows[i]["DisplayName"].ToString()].ToString() != "")
//                                {
//                                    if (!UploadManager.IsDataValid(dr[dtRecordTypleColumns.Rows[i]["DisplayName"].ToString()].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString(), ref strValidationError, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
//                                    {
//                                        strRecordIDs = strRecordIDs + dr["DBGSystemRecordID"].ToString() + ",";
//                                        dInvalidCounter = dInvalidCounter + 1;
//                                    }
//                                }
//                            }

//                            if (dInvalidCounter > 0)
//                            {
//                                string strValidResults = " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                strRecordIDs = strRecordIDs.Substring(0, strRecordIDs.Length - 1);
//                                Common.ExecuteText("Update Record SET IsActive=0, validationresults=( Case WHEN (ValidationResults IS null) then '" + strValidResults.Replace("'", "''") + "' else (ValidationResults+ '" + strValidResults.Replace("'", "''") + "') END) WHERE RecordID in (" + strRecordIDs + ")", ref connection, ref tn);
//                            }

//                            if (dInvalidCounter > 0)
//                            {
//                                //invalid data found
//                            }

//                        }

//                    }

//                    //check linked columns

//                    DataTable dtLinkedColumns = Common.DataTableFromText(@"SELECT ColumnID,SystemName FROM [Column]
//                            WHERE TableID=" + theTable.TableID.ToString() + @" AND ColumnID IN (SELECT DISTINCT LinkedParentColumnID FROM [Column] INNER JOIN [Table] ON
//                            [Table].TableID=[Column].TableID WHERE [Table].AccountID=" + theTable.AccountID.ToString() + ")", tn, connection);

//                    if (dtLinkedColumns.Rows.Count > 0)
//                    {

//                        strExecuteLot = "";
//                        foreach (DataRow drLC in dtLinkedColumns.Rows)
//                        {
//                            DataTable dtRecords = Common.DataTableFromText("SELECT RecordID FROM Record WHERE " + drLC["SystemName"].ToString() + " IS NULL AND TableID=" + theTable.TableID.ToString(), tn, connection);
//                            foreach (DataRow drR in dtRecords.Rows)
//                            {
//                                Guid newGUID = Guid.NewGuid();
//                                //Common.ExecuteText("UPDATE Record SET " + drLC["SystemName"].ToString() + "='" + newGUID.ToString() + "' WHERE RecordID=" + drR["RecordID"].ToString(), tn);
//                                strExecuteLot = strExecuteLot + "UPDATE Record SET " + drLC["SystemName"].ToString() + "='" + newGUID.ToString() + "' WHERE RecordID=" + drR["RecordID"].ToString() + ";";
//                            }

//                        }
//                        if (strExecuteLot != "")
//                        {
//                            Common.ExecuteText(strExecuteLot, ref connection, ref tn);
//                        }

//                    }

//                }

//            }
//            else
//            {

//            }


//            //Everything ok
//            try
//            {
//                Usage theUsage = new Usage(null, oBatch.AccountID, DateTime.Now, 0, 1);
//                SecurityManager.Usage_Insert(theUsage, null);
//            }
//            catch
//            {
//            }


//            return "ok";
//        }
//        catch (Exception ex)
//        {

//            return "System failed to import this batch!" + ex.Message + ex.StackTrace;
//            //throw;
//        }


//    }

    public static void RecordsImportEmail(Batch theBatch, ref string strImportedtRecords, string strBaseURL)
    {



        Content theContentEmail = SystemData.Content_Details_ByKey("DataUploadEmail", (int)theBatch.AccountID);
        Content theContentSMS = SystemData.Content_Details_ByKey("DataUploadSMS", (int)theBatch.AccountID);
        
        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail",null,theBatch.TableID);
       
        Table theTable = RecordManager.ets_Table_Details((int)theBatch.TableID);

        bool bShowExceedances = false;

        string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", theTable.AccountID, theTable.TableID);

        if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
        {
            bShowExceedances = true;
           
        }

        User theUser = SecurityManager.User_Details((int)theBatch.UserIDUploaded);

        string strBody = "";
        string strBodySMS = "";

        strBody = theContentEmail.ContentP;

        strBodySMS = theContentSMS.ContentP;

        strBody = strBody.Replace("[FileName]", theBatch.UploadedFileName);

        strBodySMS = strBodySMS.Replace("[FileName]", theBatch.UploadedFileName);


        DataTable dtCSVRecords = Common.DataTableFromText(@"Select COUNT(RecordID)
                    FROM TempRecord WHERE BatchID =" + theBatch.BatchID.ToString());

        strBody = strBody.Replace("[Total_Records]", dtCSVRecords.Rows[0][0].ToString());
        strBodySMS = strBodySMS.Replace("[Total_Records]", dtCSVRecords.Rows[0][0].ToString());



        strBody = strBody.Replace("[Table]", theTable.TableName);
        strBodySMS = strBodySMS.Replace("[Table]", theTable.TableName);


        DataTable dtImportedNowarningRecords = Common.DataTableFromText(@"Select COUNT(RecordID)
                        FROM [Record] WHERE WarningResults is null AND BatchID=" + theBatch.BatchID.ToString());


        strImportedtRecords = dtImportedNowarningRecords.Rows[0][0].ToString();
        strBody = strBody.Replace("[Valid_no_warnings]", dtImportedNowarningRecords.Rows[0][0].ToString());

        //strBody = strBody.Replace("[RejectedRecords]", (int.Parse(dtCSVRecords.Rows[0][0].ToString()) -int.Parse(dtImportedRecords.Rows[0][0].ToString())).ToString());

        strBodySMS = strBodySMS.Replace("[Valid_no_warnings]", dtImportedNowarningRecords.Rows[0][0].ToString());

        //strBodySMS = strBodySMS.Replace("[RejectedRecords]", (int.Parse(dtCSVRecords.Rows[0][0].ToString()) - int.Parse(dtImportedRecords.Rows[0][0].ToString())).ToString());

        //try
        //{
            //if(HttpContext.Current!=null)
            //{
                //string strURL = HttpContext.Current.Request.Url.Scheme +"://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/Pages/Record/UploadValidation.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt(theTable.TableID.ToString()) + "&BatchID=" + Cryptography.Encrypt(theBatch.BatchID.ToString());
            string strURL = strBaseURL+ "/Pages/Record/UploadValidation.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt(theTable.TableID.ToString()) + "&BatchID=" + Cryptography.Encrypt(theBatch.BatchID.ToString());
                strBody = strBody.Replace("[URL]", strURL);
                strBodySMS = strBodySMS.Replace("[URL]", strURL);
            //}
            
        //}
        //catch
        //{

        //}



        DataTable dtWarningRecords = Common.DataTableFromText(@"Select COUNT(RecordID)
                        FROM [Record] WHERE WarningResults is not null AND ValidationResults is null 
                                AND WarningResults LIKE '%WARNING:%'   AND WarningResults NOT LIKE '%EXCEEDANCE:%'
                                AND BatchID =" + theBatch.BatchID.ToString());


        strImportedtRecords = (int.Parse(strImportedtRecords) + int.Parse(dtWarningRecords.Rows[0][0].ToString())).ToString();

        strBody = strBody.Replace("[Valid_with_warnings]", dtWarningRecords.Rows[0][0].ToString());

        strBodySMS = strBodySMS.Replace("[Valid_with_warnings]", dtWarningRecords.Rows[0][0].ToString());
        if (bShowExceedances)
         {
             DataTable dtExceedanceRecords = Common.DataTableFromText(@"Select COUNT(RecordID)
                        FROM [Record] WHERE WarningResults is not null AND ValidationResults is null 
                                AND WarningResults LIKE '%EXCEEDANCE:%'
                                AND BatchID =" + theBatch.BatchID.ToString());


             strImportedtRecords = (int.Parse(strImportedtRecords) + int.Parse(dtExceedanceRecords.Rows[0][0].ToString())).ToString();

             strBody = strBody.Replace("[Valid_with_Exceedances]", dtExceedanceRecords.Rows[0][0].ToString());

             strBodySMS = strBodySMS.Replace("[Valid_with_Exceedances]", dtExceedanceRecords.Rows[0][0].ToString());

         }
        



        DataTable dtInValidRecords = Common.DataTableFromText(@"Select COUNT(RecordID)
                            FROM [TempRecord]                   
                              WHERE RejectReason IS NOT NULL 
                            AND BatchID =" + theBatch.BatchID.ToString());



        DataTable dtDuplicateRecords = Common.DataTableFromText(@" Select COUNT(RecordID)
                            FROM [TempRecord]                   
                              WHERE RejectReason LIKE '%DUPLICATE%'
                            AND BatchID =" + theBatch.BatchID.ToString());


        int iTotalInvalid = int.Parse(dtInValidRecords.Rows[0][0].ToString());
        int iTotalDuplicate = int.Parse(dtDuplicateRecords.Rows[0][0].ToString());
        int iTotalOtherInvalid = iTotalInvalid - iTotalDuplicate;


        strBody = strBody.Replace("[Invalid_duplicates]", iTotalDuplicate.ToString());
        strBodySMS = strBodySMS.Replace("[Invalid_duplicates]", iTotalDuplicate.ToString());

        strBody = strBody.Replace("[Invalid_others]", iTotalOtherInvalid.ToString());
        strBodySMS = strBodySMS.Replace("[Invalid_others]", iTotalOtherInvalid.ToString());



        DataTable dtRecordTypleColumlns = RecordManager.ets_Table_Columns_ForImportEmail((int)theBatch.TableID);


        string strSubject = theContentEmail.Heading.Replace("[Table]", theTable.TableName);

        
        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
         (int)theTable.TableID, null, null, null, true, null, null, null, null, null, null, null);

        string strTempBody = strBody;

        foreach (DataRow dr in dtUsersEmail.Rows)
        {
           
            strTempBody = strBody;
            strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
            try
            {

              
                string sSendEmailError = "";

                Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
                  DateTime.Now, "E", "E",
                      null, dr["Email"].ToString(), strSubject, strTempBody, null, "");


                DBGurus.SendEmail("Batch", true, null, strSubject, strTempBody, "", dr["Email"].ToString(), "", "", null, theMessage, out sSendEmailError);


            }
            catch (Exception ex)
            {

                //strErrorMsg = "Server could not send warning Email & SMS";
            }


        }

       strSubject = theContentSMS.Heading.Replace("[Table]", theTable.TableName);

        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
     (int)theTable.TableID, null, null, null, null, true, null, null, null, null, null, null);

        foreach (DataRow dr in dtUsersSMS.Rows)
        {
            //msg2.To.Clear();
            if (dr["PhoneNumber"] != DBNull.Value)
            {
                if (dr["PhoneNumber"].ToString() != "")
                {
                    strTempBody = strBodySMS;
                    strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
                    try
                    {                      

                        string sSendEmailError = "";

                        Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
                 DateTime.Now, "E", "S",
                     null, dr["PhoneNumber"].ToString() + strWarningSMSEMail, strSubject, strTempBody, null, ""); 


                        DBGurus.SendEmail("Batch SMS", null, true, strSubject, strTempBody, "",
                            dr["PhoneNumber"].ToString() + strWarningSMSEMail, "", "", null, theMessage, out sSendEmailError);



                    }
                    catch (Exception ex)
                    {

                        ErrorLog theErrorLog = new ErrorLog(null, "SMS Email", ex.Message, ex.StackTrace, DateTime.Now, "");
                        SystemData.ErrorLog_Insert(theErrorLog);
                    }
                }
            }
        }


        //now lets do the warning email

        Content theContentWarningEmail = SystemData.Content_Details_ByKey("DataUploadWarningEmail", (int)theBatch.AccountID);
        Content theContentWarningSMS = SystemData.Content_Details_ByKey("DataUploadWarningSMS", (int)theBatch.AccountID);
        

        string strWarningBody = theContentWarningEmail.ContentP;

        string strWarningBodySMS = theContentWarningSMS.ContentP;


        strWarningBody = strWarningBody.Replace("[UploadedBy]", theUser.FirstName + " " + theUser.LastName);
        strWarningBody = strWarningBody.Replace("[FileName]", theBatch.UploadedFileName);

        strWarningBodySMS = strWarningBodySMS.Replace("[UploadedBy]", theUser.FirstName + " " + theUser.LastName);
        strWarningBodySMS = strWarningBodySMS.Replace("[FileName]", theBatch.UploadedFileName);


        //make the Record table 

        string strRecordTable = "<table border='1' cellpadding='5'> <thead style='background-color:Silver;font-weight:bold'>" +
        "<tr><td> Date/Time </td><td>Record </td><td>Field</td><td>Value</td><td>Warning</td></tr></thead>";

        DataTable dtBatchDatas = UploadManager.ets_Records_By_BatchForEmail((int)theBatch.BatchID, (int)theBatch.TableID);

        bool bHasWaring = false;

        int i = 0;
        foreach (DataRow drBatch in dtBatchDatas.Rows)
        {
            if (drBatch["WarningResults"] != DBNull.Value && drBatch["WarningResults"].ToString() != "")
            {
                string strWarning = drBatch["WarningResults"].ToString();
                string strRecordIDURL = strBaseURL + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteriaID="
                    + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt(theBatch.TableID.ToString())
                    + "&Recordid=" + Cryptography.Encrypt(drBatch["DBGSystemRecordID"].ToString());

                if (strWarning.IndexOf("WARNING: Max time between") > -1)
                {
                    strRecordTable = strRecordTable + "<tr><td> " + drBatch["DateTimeRecorded"].ToString() + " </td><td><a href='" + strRecordIDURL + "'>" + drBatch["DBGSystemRecordID"].ToString() + "</a>"
                        + "</td><td>" + "Date Time Recorded" + "</td><td>" + drBatch["DateTimeRecorded"].ToString() + "</td><td>"
                        + "" + WarningMsg.MaxtimebetweenRecords + "." + "</td></tr>";
                    i = i + 1;
                    bHasWaring = true;
                }

                foreach (DataRow drColumns in dtRecordTypleColumlns.Rows)
                {

                    if (strWarning.IndexOf("WARNING: " + drColumns["DisplayName"].ToString().Trim() + " – Value outside accepted") > -1)
                    {
                        strRecordTable = strRecordTable + "<tr><td> " + drBatch["DateTimeRecorded"].ToString() + " </td><td><a href='" + strRecordIDURL + "'>" + drBatch["DBGSystemRecordID"].ToString() + "</a>"
                        + "</td><td>" + drColumns["DisplayName"].ToString() + "</td><td>" + drBatch[drColumns["DisplayName"].ToString().Trim()].ToString() + "</td><td>"
                        + drColumns["DisplayName"].ToString() + " - Value outside accepted range." + "</td></tr>";
                        i = i + 1;
                        bHasWaring = true;
                    }

                    if (strWarning.IndexOf("WARNING: " + drColumns["DisplayName"].ToString().Trim() + " – Unlikely data – outside") > -1)
                    {
                        strRecordTable = strRecordTable + "<tr><td> " + drBatch["DateTimeRecorded"].ToString() + " </td><td><a href='" + strRecordIDURL + "'>" + drBatch["DBGSystemRecordID"].ToString() + "</a>"
                        + "</td><td>" + drColumns["DisplayName"].ToString() + "</td><td>" + drBatch[drColumns["DisplayName"].ToString().Trim()].ToString() + "</td><td>"
                        + drColumns["DisplayName"].ToString() + " - Unlikely data – outside 3 standard deviations." + "</td></tr>";
                        i = i + 1;
                        bHasWaring = true;
                    }


                    if (strWarning.IndexOf("WARNING: " + drColumns["DisplayName"].ToString().Trim() + " - Value below minimum detectable") > -1)
                    {
                        strRecordTable = strRecordTable + "<tr><td> " + drBatch["DateTimeRecorded"].ToString() + " </td><td><a href='" + strRecordIDURL + "'>" + drBatch["DBGSystemRecordID"].ToString() + "</a>"
                        + "</td><td>" + drColumns["DisplayName"].ToString() + "</td><td>" + drBatch[drColumns["DisplayName"].ToString().Trim()].ToString() + "</td><td>"
                        + drColumns["DisplayName"].ToString() + " - Value below minimum detectable limit." + "</td></tr>";
                        i = i + 1;
                        bHasWaring = true;
                    }


                }
                if (i > 99)
                {
                    break;
                }

            }

        }

        strRecordTable = strRecordTable + "</table>";


       
        if (bHasWaring)
        {
            strWarningBody = strWarningBody.Replace("[RecordTable]", strRecordTable);
            strWarningBodySMS = strWarningBodySMS.Replace("[RecordTable]", strRecordTable);

            strSubject = theContentWarningEmail.Heading.Replace("[Table]", theTable.TableName);

            dtUsersEmail = RecordManager.ets_TableUser_Select(null,
             (int)theTable.TableID, null, null, null, null, null, true, null, null, null, null, null);

            foreach (DataRow dr in dtUsersEmail.Rows)
            {
                strTempBody = strWarningBody;
                strTempBody = strTempBody.Replace("[FullName]", dr["UserName"].ToString());
                strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
                try
                {
                                     
                    
                    string sSendEmailError = "";

                    Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
                 DateTime.Now, "W", "E",
                     null, dr["Email"].ToString(), strSubject, strTempBody, null, "");

                    DBGurus.SendEmail("Batch Warning", true, null, strSubject, strTempBody, "", dr["Email"].ToString(), "", "", null, theMessage, out sSendEmailError);


                }
                catch (Exception ex)
                {

                    //strErrorMsg = "Server could not send warning Email & SMS";
                }

            }

            strSubject = theContentWarningSMS.Heading.Replace("[Table]", theTable.TableName);

            dtUsersSMS = RecordManager.ets_TableUser_Select(null,
         (int)theTable.TableID, null, null, null, null, null, null, true, null, null, null, null);

            foreach (DataRow dr in dtUsersSMS.Rows)
            {
                //msg4.To.Clear();
                if (dr["PhoneNumber"] != DBNull.Value)
                {
                    if (dr["PhoneNumber"].ToString() != "")
                    {
                        strTempBody = strWarningBodySMS;
                        strTempBody = strTempBody.Replace("[FullName]", dr["UserName"].ToString());
                        strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
                        try
                        {
                         
                            string sSendEmailError = "";

                            Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
                 DateTime.Now, "W", "S",
                     null, dr["PhoneNumber"].ToString() + strWarningSMSEMail, strSubject, strTempBody, null, ""); 

                            DBGurus.SendEmail("Batch SMS Warning", null, true, strSubject, strTempBody, "",
                                dr["PhoneNumber"].ToString() + strWarningSMSEMail, "", "", null, theMessage, out sSendEmailError);


                        }
                        catch (Exception ex)
                        {

                            ErrorLog theErrorLog = new ErrorLog(null, "SMS Email", ex.Message, ex.StackTrace, DateTime.Now, "");
                            SystemData.ErrorLog_Insert(theErrorLog);
                        }
                    }
                }
            }

        }


        



        //find each Record column flat line
        //bool bFlatline = false;//test

//        DataTable dtSTColumns = Common.DataTableFromText(@"SELECT * FROM [Column] WHERE   FlatLineNumber IS NOT NULL
//                    AND TableID=" + theTable.TableID.ToString());

//        string strColumns = "";
//        if (dtSTColumns.Rows.Count > 0)
//        {
//            foreach (DataRow eachColumn in dtSTColumns.Rows)
//            {

//                DataTable dtValues = Common.DataTableFromText(@"Select [Record].DateTimeRecorded  ,[Record]." + eachColumn["SystemName"].ToString() +
//                                @" FROM [Record] INNER JOIN
//                                TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN
//                                Batch ON TempRecord.BatchID = Batch.BatchID 
//                                WHERE Batch.BatchID=" + theBatch.BatchID.ToString() +
//                                @" ORDER BY [Record].DateTimeRecorded ");

//                string strPreValue = "";
//                int iDuplicate = 0;
//                int iFlatLineNumber = int.Parse(eachColumn["FlatLineNumber"].ToString());
//                foreach (DataRow eachValue in dtValues.Rows)
//                {

//                    if (eachValue[1] != DBNull.Value && strPreValue == eachValue[1].ToString())
//                    {
//                        iDuplicate = iDuplicate + 1;
//                    }
//                    if (iDuplicate >= iFlatLineNumber)
//                    {
//                        bFlatline = true;
//                        strColumns = strColumns + " " + eachColumn["DisplayName"].ToString() + " ";
//                        break;
//                    }

//                    if (eachValue[1] != DBNull.Value && strPreValue != eachValue[1].ToString())
//                    {
//                        strPreValue = eachValue[1].ToString();
//                        iDuplicate = 1;
//                    }

//                }

//            }

//        }



        //if (bFlatline)
        //{



        //    //we need to send email


        //    Content theFlatLineEmail = SystemData.Content_Details_ByKey("FlatLineEmail", (int)theBatch.AccountID);
        //    Content theFlatLineSMS = SystemData.Content_Details_ByKey("FlatLineSMS", (int)theBatch.AccountID);

        //    if (theFlatLineEmail == null && theFlatLineSMS == null)
        //    {
        //        return;
        //    }
        //    strBody = theFlatLineEmail.ContentP;
        //    strBodySMS = theFlatLineSMS.ContentP;

        //    strBody = strBody.Replace("[UploadedBy]", theUser.FirstName + " " + theUser.LastName);
        //    strBody = strBody.Replace("[FileName]", theBatch.UploadedFileName);

        //    strBodySMS = strBodySMS.Replace("[UploadedBy]", theUser.FirstName + " " + theUser.LastName);
        //    strBodySMS = strBodySMS.Replace("[FileName]", theBatch.UploadedFileName);

        //    strBody = strBody.Replace("[Table]", theTable.TableName);
        //    strBodySMS = strBodySMS.Replace("[Table]", theTable.TableName);

        //    strBody = strBody.Replace("[Columns]", strColumns);
        //    strBodySMS = strBodySMS.Replace("[Columns]", strColumns);

            
        //    strSubject = theFlatLineEmail.Heading.Replace("[Table]", theTable.TableName);


        //    dtUsersEmail = RecordManager.ets_TableUser_Select(null,
        //     (int)theTable.TableID, null, true, null, null, null, null, null, null, null, null, null);

        //    strTempBody = strBody;

        //    foreach (DataRow dr in dtUsersEmail.Rows)
        //    {
        //        strTempBody = strBody;
        //        strTempBody = strTempBody.Replace("[FullName]", dr["UserName"].ToString());
        //        strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
        //        try
        //        {
                   
        //            //Guid guidNew = Guid.NewGuid();
        //            //string strEmailUID = guidNew.ToString();

        //            //EmailLog theEmailLog = new EmailLog(null, theBatch.AccountID, strSubject,
        //            //  dr["Email"].ToString(), DateTime.Now, theBatch.TableID,
        //            //  theBatch.BatchID,
        //            //  "Batch Flatline", strTempBody);
        //            //theEmailLog.EmailUID = strEmailUID;
                   
        //            string sSendEmailError = "";

        //            Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
        //         DateTime.Now, "W", "E",
        //             null, dr["Email"].ToString(), strSubject, strTempBody, null, "");

        //            DBGurus.SendEmail("Batch Flatline", true, null, strSubject, strTempBody, "", dr["Email"].ToString(), "", "", null, theMessage, out sSendEmailError);


        //        }
        //        catch (Exception ex)
        //        {

        //            //strErrorMsg = "Server could not send warning Email & SMS";
        //        }


        //    }

            
        //    strSubject = theFlatLineSMS.Heading.Replace("[Table]", theTable.TableName);

        //    dtUsersSMS = RecordManager.ets_TableUser_Select(null,
        // (int)theTable.TableID, null, null, true, null, null, null, null, null, null, null, null);

        //    foreach (DataRow dr in dtUsersSMS.Rows)
        //    {
        //        //msg6.To.Clear();
        //        if (dr["PhoneNumber"] != DBNull.Value)
        //        {
        //            if (dr["PhoneNumber"].ToString() != "")
        //            {
        //                strTempBody = strBodySMS;
        //                strTempBody = strTempBody.Replace("[FullName]", dr["UserName"].ToString());
        //                strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
        //                try
        //                {

        //                    // Guid guidNew = Guid.NewGuid();
        //                    //string strEmailUID = guidNew.ToString();

        //                    //EmailLog theEmailLog = new EmailLog(null, theBatch.AccountID, strSubject,
        //                    //  dr["PhoneNumber"].ToString() + strWarningSMSEMail, DateTime.Now, theBatch.TableID,
        //                    //  theBatch.BatchID,
        //                    //  "Batch SMS Flatline", strTempBody);
        //                    //theEmailLog.EmailUID = strEmailUID;
                            

        //                    string sSendEmailError = "";

        //                    Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
        //         DateTime.Now, "W", "S",
        //             null, dr["PhoneNumber"].ToString() + strWarningSMSEMail, strSubject, strTempBody, null, ""); 

        //                    DBGurus.SendEmail("Batch SMS Flatline", null, true, strSubject, strTempBody, "",
        //                        dr["PhoneNumber"].ToString() + strWarningSMSEMail, "", "", null, theMessage, out sSendEmailError);


        //                }
        //                catch (Exception ex)
        //                {

        //                    ErrorLog theErrorLog = new ErrorLog(null, "SMS Email", ex.Message, ex.StackTrace, DateTime.Now, "");
        //                    SystemData.ErrorLog_Insert(theErrorLog);
        //                }
        //            }
        //        }
        //    }




        //}


        //Exceedance

        if (bShowExceedances)
        {
            //now lets do the Exceedance email

            Content theContentExceedanceEmail = SystemData.Content_Details_ByKey("DataUploadExceedanceEmail", (int)theBatch.AccountID);
            Content theContentExceedanceSMS = SystemData.Content_Details_ByKey("DataUploadExceedanceSMS", (int)theBatch.AccountID);


            string strExceedanceBody = theContentExceedanceEmail.ContentP;

            string strExceedanceBodySMS = theContentExceedanceSMS.ContentP;


            strExceedanceBody = strExceedanceBody.Replace("[UploadedBy]", theUser.FirstName + " " + theUser.LastName);
            strExceedanceBody = strExceedanceBody.Replace("[FileName]", theBatch.UploadedFileName);

            strExceedanceBodySMS = strExceedanceBodySMS.Replace("[UploadedBy]", theUser.FirstName + " " + theUser.LastName);
            strExceedanceBodySMS = strExceedanceBodySMS.Replace("[FileName]", theBatch.UploadedFileName);


            //make the Record table 

            strRecordTable = "<table border='1' cellpadding='5'> <thead style='background-color:Silver;font-weight:bold'>" +
            "<tr><td> Date/Time </td><td>Record </td><td>Field</td><td>Value</td><td>Exceedance</td></tr></thead>";

           // dtBatchDatas = UploadManager.ets_Records_By_BatchForEmail((int)theBatch.BatchID, (int)theBatch.TableID);

            bool bHasExceedance = false;

            i = 0;
            foreach (DataRow drBatch in dtBatchDatas.Rows)
            {
                if (drBatch["WarningResults"] != DBNull.Value && drBatch["WarningResults"].ToString() != "")
                {
                    string strExceedance = drBatch["WarningResults"].ToString();
                    string strRecordIDURL = strBaseURL + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteriaID="
                        + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt(theBatch.TableID.ToString())
                        + "&Recordid=" + Cryptography.Encrypt(drBatch["DBGSystemRecordID"].ToString());



                    foreach (DataRow drColumns in dtRecordTypleColumlns.Rows)
                    {

                        if (strExceedance.IndexOf("EXCEEDANCE: " + drColumns["DisplayName"].ToString().Trim() + " – Value outside accepted") > -1)
                        {
                            strRecordTable = strRecordTable + "<tr><td> " + drBatch["DateTimeRecorded"].ToString() + " </td><td><a href='" + strRecordIDURL + "'>" + drBatch["DBGSystemRecordID"].ToString() + "</a>"
                            + "</td><td>" + drColumns["DisplayName"].ToString() + "</td><td>" + drBatch[drColumns["DisplayName"].ToString().Trim()].ToString() + "</td><td>"
                            + drColumns["DisplayName"].ToString() + " - Value outside accepted range." + "</td></tr>";
                            i = i + 1;
                            bHasExceedance = true;
                        }

                    }
                    if (i > 99)
                    {
                        break;
                    }

                }

            }

            strRecordTable = strRecordTable + "</table>";



            if (bShowExceedances && bHasExceedance)
            {
                strExceedanceBody = strExceedanceBody.Replace("[RecordTable]", strRecordTable);
                strExceedanceBodySMS = strExceedanceBodySMS.Replace("[RecordTable]", strRecordTable);

                strSubject = theContentExceedanceEmail.Heading.Replace("[Table]", theTable.TableName);

                dtUsersEmail = RecordManager.ets_TableUser_Select(null,
                 (int)theTable.TableID, null, null, null, null, null, null, null, null, null, true, null);

                foreach (DataRow dr in dtUsersEmail.Rows)
                {
                    strTempBody = strExceedanceBody;
                    strTempBody = strTempBody.Replace("[FullName]", dr["UserName"].ToString());
                    strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
                    try
                    {

                       

                        string sSendEmailError = "";



                        Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
             DateTime.Now, "W", "E",
                 null, dr["Email"].ToString(), strSubject, strTempBody, null, "");


                        DBGurus.SendEmail("Batch Exceedance", true, null, strSubject, strTempBody, "", dr["Email"].ToString(), "", "", null, theMessage, out sSendEmailError);


                    }
                    catch (Exception ex)
                    {

                        //strErrorMsg = "Server could not send Exceedance Email & SMS";
                    }

                }

                strSubject = theContentExceedanceSMS.Heading.Replace("[Table]", theTable.TableName);

                dtUsersSMS = RecordManager.ets_TableUser_Select(null,
             (int)theTable.TableID, null, null, null, null, null, null, null, null, null, null, true);

                foreach (DataRow dr in dtUsersSMS.Rows)
                {
                    //msg4.To.Clear();
                    if (dr["PhoneNumber"] != DBNull.Value)
                    {
                        if (dr["PhoneNumber"].ToString() != "")
                        {
                            strTempBody = strExceedanceBodySMS;
                            strTempBody = strTempBody.Replace("[FullName]", dr["UserName"].ToString());
                            strTempBody = strTempBody.Replace("[FirstName]", dr["FirstName"].ToString());
                            try
                            {

                              
                                string sSendEmailError = "";


                                Message theMessage = new Message(null, null, theBatch.TableID, theBatch.AccountID,
           DateTime.Now, "W", "S",
               null, dr["PhoneNumber"].ToString() + strWarningSMSEMail, strSubject, strTempBody, null, ""); 


                                DBGurus.SendEmail("Batch SMS Exceedance", null, true, strSubject, strTempBody, "",
                                    dr["PhoneNumber"].ToString() + strWarningSMSEMail, "", "", null, theMessage, out sSendEmailError);


                            }
                            catch (Exception ex)
                            {

                                ErrorLog theErrorLog = new ErrorLog(null, "SMS Email", ex.Message, ex.StackTrace, DateTime.Now, "");
                                SystemData.ErrorLog_Insert(theErrorLog);
                            }
                        }
                    }
                }

            }
        }



    }



//    public static void MobileSynceEmail(Batch theBatch, ref string strImportedtRecords)
//    {
//        //Content theContent = SystemData.Content_Details_ByKey("RecordsImportEmail");


//        Content theContentEmail = SystemData.Content_Details_ByKey("DataUploadEmail", (int)theBatch.AccountID);
//        Content theContentSMS = SystemData.Content_Details_ByKey("DataUploadSMS", (int)theBatch.AccountID);
//        if (theContentEmail == null && theContentSMS == null)
//        {
//            return;
//        }


//        string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
//        string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
//        string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
//        string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
//        string strWarningSMSEMail = SystemData.SystemOption_ValueByKey("WarningSMSEmail");
//        string strEnableSSL = SystemData.SystemOption_ValueByKey("EnableSSL");
//        string strSmtpPort = SystemData.SystemOption_ValueByKey("SmtpPort");

//        Table theTable = RecordManager.ets_Table_Details((int)theBatch.TableID);

//        User theUser = SecurityManager.User_Details((int)theBatch.UserIDUploaded);

//        string strBody = theContentEmail.ContentP;

//        string strBodySMS = theContentSMS.ContentP;

//        strBody = strBody.Replace("[FullName]", theUser.FirstName + " " + theUser.LastName);
//        strBody = strBody.Replace("[FileName]", theBatch.UploadedFileName);

//        strBodySMS = strBodySMS.Replace("[FullName]", theUser.FirstName + " " + theUser.LastName);
//        strBodySMS = strBodySMS.Replace("[FileName]", theBatch.UploadedFileName);


//        DataTable dtCSVRecords = Common.DataTableFromText(@"Select COUNT(*)
//                    FROM TempRecord WHERE BatchID =" + theBatch.BatchID.ToString());


//        strBody = strBody.Replace("[CSVRecords]", dtCSVRecords.Rows[0][0].ToString());
//        strBodySMS = strBodySMS.Replace("[CSVRecords]", dtCSVRecords.Rows[0][0].ToString());


//        DataTable dtImportedRecords = Common.DataTableFromText(@"Select COUNT(*)
//                        FROM [Record] INNER JOIN
//                        TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN
//                        Batch ON TempRecord.BatchID = Batch.BatchID                      
//                          WHERE Batch.BatchID=" + theBatch.BatchID.ToString());


//        strImportedtRecords = dtImportedRecords.Rows[0][0].ToString();
//        strBody = strBody.Replace("[ImportedRecords]", dtImportedRecords.Rows[0][0].ToString());

//        strBody = strBody.Replace("[RejectedRecords]", (int.Parse(dtCSVRecords.Rows[0][0].ToString()) - int.Parse(dtImportedRecords.Rows[0][0].ToString())).ToString());

//        strBodySMS = strBodySMS.Replace("[ImportedRecords]", dtImportedRecords.Rows[0][0].ToString());

//        strBodySMS = strBodySMS.Replace("[RejectedRecords]", (int.Parse(dtCSVRecords.Rows[0][0].ToString()) - int.Parse(dtImportedRecords.Rows[0][0].ToString())).ToString());



//        DataTable dtWarningRecords = Common.DataTableFromText(@"Select COUNT(*)
//                        FROM [Record] INNER JOIN
//                        TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN
//                        Batch ON TempRecord.BatchID = Batch.BatchID                      
//                          WHERE Record.WarningResults is not null AND Record.ValidationResults is null 
//                            AND Batch.BatchID =" + theBatch.BatchID.ToString());




//        strBody = strBody.Replace("[WarningRecords]", dtWarningRecords.Rows[0][0].ToString());

//        strBodySMS = strBodySMS.Replace("[WarningRecords]", dtWarningRecords.Rows[0][0].ToString());

//        DataTable dtInValidRecords = Common.DataTableFromText(@"Select COUNT(*)
//                            FROM [Record] INNER JOIN
//                            TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN
//                            Batch ON TempRecord.BatchID = Batch.BatchID                      
//                              WHERE Record.ValidationResults is not null 
//                            AND Batch.BatchID =" + theBatch.BatchID.ToString());




//        strBody = strBody.Replace("[InValidRecords]", dtInValidRecords.Rows[0][0].ToString());
//        strBodySMS = strBodySMS.Replace("[InValidRecords]", dtInValidRecords.Rows[0][0].ToString());


//        DataTable dtMaxTimeRecords = Common.DataTableFromText(@"Select COUNT(*)
//                            FROM [Record] INNER JOIN
//                            TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN
//                            Batch ON TempRecord.BatchID = Batch.BatchID                      
    //                              WHERE Record.WarningResults LIKE'%" + WarningMsg.MaxtimebetweenRecords + "%'
//                            AND Batch.BatchID =" + theBatch.BatchID.ToString());



//        strBody = strBody.Replace("[MaxTimeRecords]", dtMaxTimeRecords.Rows[0][0].ToString());
//        strBodySMS = strBodySMS.Replace("[MaxTimeRecords]", dtMaxTimeRecords.Rows[0][0].ToString());







//        //        DataTable dtRecordTypleColumlns = RecordManager.ets_Table_Columns_ForImportEmail((int)theBatch.TableID);
//        //        DataTable dtDatas = UploadManager.ets_Records_By_Batch((int)theBatch.BatchID, (int)theBatch.TableID);

//        //        string strTable = "<table><tr><td> Column </td><td> Warning</td><td>Sensor Warning</td><td>Invalid</td></tr>";

//        //        for (int i = 0; i < dtRecordTypleColumlns.Rows.Count; i++)
//        //        {


//        //            DataTable dtWarning = Common.DataTableFromText(@"Select COUNT(*)
//        //                        FROM [Record] INNER JOIN
//        //                        TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN
//        //                        Batch ON TempRecord.BatchID = Batch.BatchID                      
//        //                          WHERE Batch.BatchID  =" + theBatch.BatchID.ToString() + " AND Record.WarningResults " +
//        //                  " LIKE '%WARNING: " + dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString().Trim() + " – Value outside accepted range%'");

//        //            DataTable dtSensorWarning = Common.DataTableFromText(@"Select COUNT(*)
//        //                        FROM [Record] INNER JOIN
//        //                        TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN
//        //                        Batch ON TempRecord.BatchID = Batch.BatchID                      
//        //                          WHERE Batch.BatchID  =" + theBatch.BatchID.ToString() + " AND (Record.WarningResults " +
//        //                  "LIKE '%SENSOR WARNING: " + dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString() + " - Sensor out of Calibration%' OR Record.WarningResults LIKE '%SENSOR WARNING: " + dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString() + " - Value below minimum detectable limit%')");


//        //            DataTable dtInvalid = Common.DataTableFromText(@"Select COUNT(*)
//        //                        FROM [Record] INNER JOIN
//        //                        TempRecord ON Record.TempRecordID = TempRecord.RecordID INNER JOIN
//        //                        Batch ON TempRecord.BatchID = Batch.BatchID                      
//        //                          WHERE Batch.BatchID  =" + theBatch.BatchID.ToString() + " AND Record.ValidationResults " +
//        //                  "LIKE'%INVALID:" + dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString() + "%'");

//        //            if (dtWarning.Rows[0][0].ToString() == "0" && dtSensorWarning.Rows[0][0].ToString() == "0" && dtInvalid.Rows[0][0].ToString() == "0")
//        //            {

//        //            }
//        //            else
//        //            {
//        //                strTable = strTable + "<tr><td> " + dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString() + " </td><td> " + dtWarning.Rows[0][0].ToString() + "</td><td>" + dtSensorWarning.Rows[0][0].ToString() + "</td><td>" + dtInvalid.Rows[0][0].ToString() + "</td></tr>";
//        //            }


//        //        }

//        //        strTable = strTable + "</table>";

//        //        //we need to make the dynamic body 


//        //        strBody = strBody.Replace("[ColumnTable]", strTable);
//        //        strBodySMS = strBodySMS.Replace("[ColumnTable]", strTable);




//        MailMessage msg = new MailMessage();
//        msg.From = new MailAddress(strEmail);


//        msg.Subject = theContentEmail.Heading.Replace("[Table]", "Mobile Sync - " + theTable.TableName);

//        msg.IsBodyHtml = true;

//        msg.Body = strBody;// Sb.ToString();
//        //msg.To.Add(Email.Text);
//        SmtpClient smtpClient = new SmtpClient(strEmailServer);
//        smtpClient.Timeout = 99999;
//        smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

//        smtpClient.EnableSsl = bool.Parse(strEnableSSL);
//        smtpClient.Port = int.Parse(strSmtpPort);

//        DataTable dtUsersEmail = RecordManager.ets_TableUser_Select(null,
//         (int)theTable.TableID, null, null, null, true, null, null, null, null, null);

//        foreach (DataRow dr in dtUsersEmail.Rows)
//        {
//            msg.To.Clear();
//            msg.To.Add(dr["Email"].ToString());
//            try
//            {


//#if (!DEBUG)
//                smtpClient.Send(msg);
//#endif

//                if (System.Web.HttpContext.Current.Session["AccountID"] != null)
//                {

//                    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
//                }

//            }
//            catch (Exception ex)
//            {

//                //strErrorMsg = "Server could not send warning Email & SMS";
//            }


//        }


//        msg = new MailMessage();
//        msg.From = new MailAddress(strEmail);


//        msg.Subject = theContentSMS.Heading.Replace("[Table]", "Mobile Sync - " + theTable.TableName);
//        msg.IsBodyHtml = true;

//        msg.Body = strBodySMS;


//        DataTable dtUsersSMS = RecordManager.ets_TableUser_Select(null,
//     (int)theTable.TableID, null, null, null, null, true, null, null, null, null);

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

//                        if (System.Web.HttpContext.Current.Session["AccountID"] != null)
//                        {

//                            SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), null, true, null, null);
//                        }

//                    }
//                    catch (Exception)
//                    {

//                        //strErrorMsg = "Server could not send warning Email & SMS";
//                    }
//                }
//            }
//        }

//    }

  

    public static Batch ets_Batch_Details(int nBatchID)
    {



        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("ets_Batch_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;



                //command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Batch temp = new Batch(
                                (int)reader["BatchID"],
                                reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
                                 (string)reader["BatchDescription"],
                                 reader["UploadedFileName"] == DBNull.Value ? "" : (string)reader["UploadedFileName"],
                                 (DateTime)reader["DateAdded"],
                                 (Guid)reader["UniqueName"],
                                 reader["UserIDUploaded"] == DBNull.Value ? null : (int?)reader["UserIDUploaded"],
                                 (int)reader["AccountID"], (bool)reader["IsImportPositional"]
                                 );
                            temp.IsImported = reader["IsImported"] == DBNull.Value ? null : (bool?)reader["IsImported"];
                            temp.AllowDataUpdate = reader["AllowDataUpdate"] == DBNull.Value ? null : (bool?)reader["AllowDataUpdate"];
                            temp.ImportTemplateID = reader["ImportTemplateID"] == DBNull.Value ? null : (int?)reader["ImportTemplateID"];

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

    public static DataTable ets_Batch_Select(int? nBatchID, int? nTableID,
  string sBatchDescription, string sUploadedFileName, string sSearch, DateTime? dDateFrom, DateTime? dDateTo, int? nUserIDUploaded,
   int? nAccountID, string sOrder,
string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, string sTableIn)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Batch_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.CommandTimeout = 0;

                if (nBatchID != null)
                    command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (sBatchDescription != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sBatchDescription", sBatchDescription));

                if (sUploadedFileName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sUploadedFileName", sUploadedFileName));

                if (sSearch != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sSearch", sSearch));


                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));

                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo ", dDateTo));


                if (nUserIDUploaded != null)
                    command.Parameters.Add(new SqlParameter("@nUserIDUploaded", nUserIDUploaded));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));


                if (sOrder == string.Empty || sOrderDirection == string.Empty)
                {
                    sOrder = "BatchID";
                    sOrderDirection = "DESC";

                }
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
                DataSet ds = new DataSet();

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
                    //iTotalDynamicColumns = ds.Tables[0].Columns.Count;
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





    //public static int ets_TempRecord_Insert(TempRecord p_TempRecord)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_TempRecord_Insert", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //            pRV.Direction = ParameterDirection.Output;

    //            command.Parameters.Add(pRV);
    //            command.Parameters.Add(new SqlParameter("@nAccountID", p_TempRecord.AccountID));
    //            command.Parameters.Add(new SqlParameter("@nBatchID", p_TempRecord.BatchID));

    //            //if (p_TempRecord.LocationID !=null)
    //            //command.Parameters.Add(new SqlParameter("@nLocationID", p_TempRecord.LocationID));

    //            //if (p_TempRecord.LocationName != string.Empty)
    //            //command.Parameters.Add(new SqlParameter("@sLocationName", p_TempRecord.LocationName));

    //            if (p_TempRecord.TableID != null)
    //                command.Parameters.Add(new SqlParameter("@nTableID", p_TempRecord.TableID));

    //            if (p_TempRecord.DateTimeRecorded != null)
    //                command.Parameters.Add(new SqlParameter("@dDateTimeRecorded", p_TempRecord.DateTimeRecorded));

    //            if (p_TempRecord.Notes != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sNotes", p_TempRecord.Notes));


    //            if (p_TempRecord.IsActive == null)
    //                p_TempRecord.IsActive = true;
    //            command.Parameters.Add(new SqlParameter("@bIsActive", p_TempRecord.IsActive));

    //            if (p_TempRecord.IsValidated != null)
    //                command.Parameters.Add(new SqlParameter("@bIsValidated", p_TempRecord.IsValidated));

    //            if (p_TempRecord.RejectReason != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sRejectReason", p_TempRecord.RejectReason));

    //            if (p_TempRecord.WarningReason != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sWarningReason", p_TempRecord.WarningReason));


    //            if (p_TempRecord.ValidationResults != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sValidationResults", p_TempRecord.ValidationResults));


    //            if (p_TempRecord.V001 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV001", p_TempRecord.V001));
    //            if (p_TempRecord.V002 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV002", p_TempRecord.V002));
    //            if (p_TempRecord.V003 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV003", p_TempRecord.V003));
    //            if (p_TempRecord.V004 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV004", p_TempRecord.V004));
    //            if (p_TempRecord.V005 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV005", p_TempRecord.V005));
    //            if (p_TempRecord.V006 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV006", p_TempRecord.V006));
    //            if (p_TempRecord.V007 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV007", p_TempRecord.V007));
    //            if (p_TempRecord.V008 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV008", p_TempRecord.V008));
    //            if (p_TempRecord.V009 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV009", p_TempRecord.V009));
    //            if (p_TempRecord.V010 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV010", p_TempRecord.V010));
    //            if (p_TempRecord.V011 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV011", p_TempRecord.V011));
    //            if (p_TempRecord.V012 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV012", p_TempRecord.V012));
    //            if (p_TempRecord.V013 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV013", p_TempRecord.V013));
    //            if (p_TempRecord.V014 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV014", p_TempRecord.V014));
    //            if (p_TempRecord.V015 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV015", p_TempRecord.V015));
    //            if (p_TempRecord.V016 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV016", p_TempRecord.V016));
    //            if (p_TempRecord.V017 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV017", p_TempRecord.V017));
    //            if (p_TempRecord.V018 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV018", p_TempRecord.V018));
    //            if (p_TempRecord.V019 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV019", p_TempRecord.V019));
    //            if (p_TempRecord.V020 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV020", p_TempRecord.V020));
    //            if (p_TempRecord.V021 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV021", p_TempRecord.V021));
    //            if (p_TempRecord.V022 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV022", p_TempRecord.V022));
    //            if (p_TempRecord.V023 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV023", p_TempRecord.V023));
    //            if (p_TempRecord.V024 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV024", p_TempRecord.V024));
    //            if (p_TempRecord.V025 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV025", p_TempRecord.V025));
    //            if (p_TempRecord.V026 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV026", p_TempRecord.V026));
    //            if (p_TempRecord.V027 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV027", p_TempRecord.V027));
    //            if (p_TempRecord.V028 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV028", p_TempRecord.V028));
    //            if (p_TempRecord.V029 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV029", p_TempRecord.V029));
    //            if (p_TempRecord.V030 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV030", p_TempRecord.V030));
    //            if (p_TempRecord.V031 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV031", p_TempRecord.V031));
    //            if (p_TempRecord.V032 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV032", p_TempRecord.V032));
    //            if (p_TempRecord.V033 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV033", p_TempRecord.V033));
    //            if (p_TempRecord.V034 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV034", p_TempRecord.V034));
    //            if (p_TempRecord.V035 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV035", p_TempRecord.V035));
    //            if (p_TempRecord.V036 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV036", p_TempRecord.V036));
    //            if (p_TempRecord.V037 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV037", p_TempRecord.V037));
    //            if (p_TempRecord.V038 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV038", p_TempRecord.V038));
    //            if (p_TempRecord.V039 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV039", p_TempRecord.V039));
    //            if (p_TempRecord.V040 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV040", p_TempRecord.V040));
    //            if (p_TempRecord.V041 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV041", p_TempRecord.V041));
    //            if (p_TempRecord.V042 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV042", p_TempRecord.V042));
    //            if (p_TempRecord.V043 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV043", p_TempRecord.V043));
    //            if (p_TempRecord.V044 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV044", p_TempRecord.V044));
    //            if (p_TempRecord.V045 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV045", p_TempRecord.V045));
    //            if (p_TempRecord.V046 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV046", p_TempRecord.V046));
    //            if (p_TempRecord.V047 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV047", p_TempRecord.V047));
    //            if (p_TempRecord.V048 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV048", p_TempRecord.V048));
    //            if (p_TempRecord.V049 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV049", p_TempRecord.V049));
    //            if (p_TempRecord.V050 != string.Empty)
    //                command.Parameters.Add(new SqlParameter("@sV050", p_TempRecord.V050));


    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();


    //            return int.Parse(pRV.Value.ToString());
    //        }
    //    }
    //}




    public static int ets_TempRecord_Insert(TempRecord p_TempRecord)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TempRecord_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nAccountID", p_TempRecord.AccountID));
                command.Parameters.Add(new SqlParameter("@nBatchID", p_TempRecord.BatchID));

                //if (p_TempRecord.LocationID != null)
                //    command.Parameters.Add(new SqlParameter("@nLocationID", p_TempRecord.LocationID));

                //if (p_TempRecord.LocationName != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sLocationName", p_TempRecord.LocationName));

                if (p_TempRecord.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_TempRecord.TableID));

                if (p_TempRecord.DateTimeRecorded != null)
                    command.Parameters.Add(new SqlParameter("@dDateTimeRecorded", p_TempRecord.DateTimeRecorded));

                if (p_TempRecord.Notes != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sNotes", p_TempRecord.Notes));


                if (p_TempRecord.IsActive == null)
                    p_TempRecord.IsActive = true;
                command.Parameters.Add(new SqlParameter("@bIsActive", p_TempRecord.IsActive));

                if (p_TempRecord.IsValidated != null)
                    command.Parameters.Add(new SqlParameter("@bIsValidated", p_TempRecord.IsValidated));

                if (p_TempRecord.RejectReason != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sRejectReason", p_TempRecord.RejectReason));

                if (p_TempRecord.WarningReason != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sWarningReason", p_TempRecord.WarningReason));

                //if (p_TempRecord.ExceedanceReason != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sExceedanceReason", p_TempRecord.ExceedanceReason));


                if (p_TempRecord.ValidationResults != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValidationResults", p_TempRecord.ValidationResults));


                if (p_TempRecord.V001 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V001", p_TempRecord.V001));
                if (p_TempRecord.V002 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V002", p_TempRecord.V002));
                if (p_TempRecord.V003 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V003", p_TempRecord.V003));
                if (p_TempRecord.V004 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V004", p_TempRecord.V004));
                if (p_TempRecord.V005 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V005", p_TempRecord.V005));
                if (p_TempRecord.V006 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V006", p_TempRecord.V006));
                if (p_TempRecord.V007 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V007", p_TempRecord.V007));
                if (p_TempRecord.V008 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V008", p_TempRecord.V008));
                if (p_TempRecord.V009 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V009", p_TempRecord.V009));
                if (p_TempRecord.V010 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V010", p_TempRecord.V010));
                if (p_TempRecord.V011 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V011", p_TempRecord.V011));
                if (p_TempRecord.V012 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V012", p_TempRecord.V012));
                if (p_TempRecord.V013 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V013", p_TempRecord.V013));
                if (p_TempRecord.V014 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V014", p_TempRecord.V014));
                if (p_TempRecord.V015 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V015", p_TempRecord.V015));
                if (p_TempRecord.V016 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V016", p_TempRecord.V016));
                if (p_TempRecord.V017 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V017", p_TempRecord.V017));
                if (p_TempRecord.V018 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V018", p_TempRecord.V018));
                if (p_TempRecord.V019 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V019", p_TempRecord.V019));
                if (p_TempRecord.V020 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V020", p_TempRecord.V020));
                if (p_TempRecord.V021 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V021", p_TempRecord.V021));
                if (p_TempRecord.V022 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V022", p_TempRecord.V022));
                if (p_TempRecord.V023 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V023", p_TempRecord.V023));
                if (p_TempRecord.V024 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V024", p_TempRecord.V024));
                if (p_TempRecord.V025 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V025", p_TempRecord.V025));
                if (p_TempRecord.V026 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V026", p_TempRecord.V026));
                if (p_TempRecord.V027 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V027", p_TempRecord.V027));
                if (p_TempRecord.V028 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V028", p_TempRecord.V028));
                if (p_TempRecord.V029 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V029", p_TempRecord.V029));
                if (p_TempRecord.V030 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V030", p_TempRecord.V030));
                if (p_TempRecord.V031 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V031", p_TempRecord.V031));
                if (p_TempRecord.V032 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V032", p_TempRecord.V032));
                if (p_TempRecord.V033 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V033", p_TempRecord.V033));
                if (p_TempRecord.V034 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V034", p_TempRecord.V034));
                if (p_TempRecord.V035 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V035", p_TempRecord.V035));
                if (p_TempRecord.V036 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V036", p_TempRecord.V036));
                if (p_TempRecord.V037 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V037", p_TempRecord.V037));
                if (p_TempRecord.V038 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V038", p_TempRecord.V038));
                if (p_TempRecord.V039 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V039", p_TempRecord.V039));
                if (p_TempRecord.V040 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V040", p_TempRecord.V040));
                if (p_TempRecord.V041 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V041", p_TempRecord.V041));
                if (p_TempRecord.V042 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V042", p_TempRecord.V042));
                if (p_TempRecord.V043 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V043", p_TempRecord.V043));
                if (p_TempRecord.V044 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V044", p_TempRecord.V044));
                if (p_TempRecord.V045 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V045", p_TempRecord.V045));
                if (p_TempRecord.V046 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V046", p_TempRecord.V046));
                if (p_TempRecord.V047 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V047", p_TempRecord.V047));
                if (p_TempRecord.V048 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V048", p_TempRecord.V048));
                if (p_TempRecord.V049 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V049", p_TempRecord.V049));
                if (p_TempRecord.V050 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V050", p_TempRecord.V050));

                if (p_TempRecord.V051 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V051", p_TempRecord.V051));
                if (p_TempRecord.V052 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V052", p_TempRecord.V052));
                if (p_TempRecord.V053 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V053", p_TempRecord.V053));
                if (p_TempRecord.V054 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V054", p_TempRecord.V054));
                if (p_TempRecord.V055 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V055", p_TempRecord.V055));
                if (p_TempRecord.V056 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V056", p_TempRecord.V056));
                if (p_TempRecord.V057 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V057", p_TempRecord.V057));
                if (p_TempRecord.V058 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V058", p_TempRecord.V058));
                if (p_TempRecord.V059 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V059", p_TempRecord.V059));
                if (p_TempRecord.V060 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V060", p_TempRecord.V060));
                if (p_TempRecord.V061 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V061", p_TempRecord.V061));
                if (p_TempRecord.V062 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V062", p_TempRecord.V062));
                if (p_TempRecord.V063 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V063", p_TempRecord.V063));
                if (p_TempRecord.V064 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V064", p_TempRecord.V064));
                if (p_TempRecord.V065 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V065", p_TempRecord.V065));
                if (p_TempRecord.V066 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V066", p_TempRecord.V066));
                if (p_TempRecord.V067 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V067", p_TempRecord.V067));
                if (p_TempRecord.V068 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V068", p_TempRecord.V068));
                if (p_TempRecord.V069 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V069", p_TempRecord.V069));
                if (p_TempRecord.V070 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V070", p_TempRecord.V070));
                if (p_TempRecord.V071 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V071", p_TempRecord.V071));
                if (p_TempRecord.V072 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V072", p_TempRecord.V072));
                if (p_TempRecord.V073 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V073", p_TempRecord.V073));
                if (p_TempRecord.V074 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V074", p_TempRecord.V074));
                if (p_TempRecord.V075 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V075", p_TempRecord.V075));
                if (p_TempRecord.V076 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V076", p_TempRecord.V076));
                if (p_TempRecord.V077 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V077", p_TempRecord.V077));
                if (p_TempRecord.V078 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V078", p_TempRecord.V078));
                if (p_TempRecord.V079 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V079", p_TempRecord.V079));
                if (p_TempRecord.V080 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V080", p_TempRecord.V080));
                if (p_TempRecord.V081 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V081", p_TempRecord.V081));
                if (p_TempRecord.V082 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V082", p_TempRecord.V082));
                if (p_TempRecord.V083 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V083", p_TempRecord.V083));
                if (p_TempRecord.V084 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V084", p_TempRecord.V084));
                if (p_TempRecord.V085 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V085", p_TempRecord.V085));
                if (p_TempRecord.V086 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V086", p_TempRecord.V086));
                if (p_TempRecord.V087 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V087", p_TempRecord.V087));
                if (p_TempRecord.V088 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V088", p_TempRecord.V088));
                if (p_TempRecord.V089 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V089", p_TempRecord.V089));
                if (p_TempRecord.V090 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V090", p_TempRecord.V090));
                if (p_TempRecord.V091 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V091", p_TempRecord.V091));
                if (p_TempRecord.V092 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V092", p_TempRecord.V092));
                if (p_TempRecord.V093 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V093", p_TempRecord.V093));
                if (p_TempRecord.V094 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V094", p_TempRecord.V094));
                if (p_TempRecord.V095 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V095", p_TempRecord.V095));
                if (p_TempRecord.V096 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V096", p_TempRecord.V096));
                if (p_TempRecord.V097 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V097", p_TempRecord.V097));
                if (p_TempRecord.V098 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V098", p_TempRecord.V098));
                if (p_TempRecord.V099 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V099", p_TempRecord.V099));
                if (p_TempRecord.V100 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V100", p_TempRecord.V100));


                if (p_TempRecord.V101 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V101", p_TempRecord.V101));
                if (p_TempRecord.V102 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V102", p_TempRecord.V102));
                if (p_TempRecord.V103 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V103", p_TempRecord.V103));
                if (p_TempRecord.V104 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V104", p_TempRecord.V104));
                if (p_TempRecord.V105 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V105", p_TempRecord.V105));
                if (p_TempRecord.V106 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V106", p_TempRecord.V106));
                if (p_TempRecord.V107 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V107", p_TempRecord.V107));
                if (p_TempRecord.V108 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V108", p_TempRecord.V108));
                if (p_TempRecord.V109 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V109", p_TempRecord.V109));
                if (p_TempRecord.V110 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V110", p_TempRecord.V110));
                if (p_TempRecord.V111 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V111", p_TempRecord.V111));
                if (p_TempRecord.V112 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V112", p_TempRecord.V112));
                if (p_TempRecord.V113 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V113", p_TempRecord.V113));
                if (p_TempRecord.V114 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V114", p_TempRecord.V114));
                if (p_TempRecord.V115 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V115", p_TempRecord.V115));
                if (p_TempRecord.V116 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V116", p_TempRecord.V116));
                if (p_TempRecord.V117 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V117", p_TempRecord.V117));
                if (p_TempRecord.V118 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V118", p_TempRecord.V118));
                if (p_TempRecord.V119 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V119", p_TempRecord.V119));
                if (p_TempRecord.V120 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V120", p_TempRecord.V120));
                if (p_TempRecord.V121 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V121", p_TempRecord.V121));
                if (p_TempRecord.V122 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V122", p_TempRecord.V122));
                if (p_TempRecord.V123 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V123", p_TempRecord.V123));
                if (p_TempRecord.V124 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V124", p_TempRecord.V124));
                if (p_TempRecord.V125 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V125", p_TempRecord.V125));
                if (p_TempRecord.V126 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V126", p_TempRecord.V126));
                if (p_TempRecord.V127 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V127", p_TempRecord.V127));
                if (p_TempRecord.V128 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V128", p_TempRecord.V128));
                if (p_TempRecord.V129 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V129", p_TempRecord.V129));
                if (p_TempRecord.V130 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V130", p_TempRecord.V130));
                if (p_TempRecord.V131 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V131", p_TempRecord.V131));
                if (p_TempRecord.V132 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V132", p_TempRecord.V132));
                if (p_TempRecord.V133 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V133", p_TempRecord.V133));
                if (p_TempRecord.V134 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V134", p_TempRecord.V134));
                if (p_TempRecord.V135 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V135", p_TempRecord.V135));
                if (p_TempRecord.V136 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V136", p_TempRecord.V136));
                if (p_TempRecord.V137 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V137", p_TempRecord.V137));
                if (p_TempRecord.V138 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V138", p_TempRecord.V138));
                if (p_TempRecord.V139 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V139", p_TempRecord.V139));
                if (p_TempRecord.V140 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V140", p_TempRecord.V140));
                if (p_TempRecord.V141 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V141", p_TempRecord.V141));
                if (p_TempRecord.V142 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V142", p_TempRecord.V142));
                if (p_TempRecord.V143 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V143", p_TempRecord.V143));
                if (p_TempRecord.V144 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V144", p_TempRecord.V144));
                if (p_TempRecord.V145 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V145", p_TempRecord.V145));
                if (p_TempRecord.V146 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V146", p_TempRecord.V146));
                if (p_TempRecord.V147 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V147", p_TempRecord.V147));
                if (p_TempRecord.V148 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V148", p_TempRecord.V148));
                if (p_TempRecord.V149 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V149", p_TempRecord.V149));
                if (p_TempRecord.V150 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V150", p_TempRecord.V150));



                if (p_TempRecord.V151 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V151", p_TempRecord.V151));
                if (p_TempRecord.V152 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V152", p_TempRecord.V152));
                if (p_TempRecord.V153 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V153", p_TempRecord.V153));
                if (p_TempRecord.V154 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V154", p_TempRecord.V154));
                if (p_TempRecord.V155 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V155", p_TempRecord.V155));
                if (p_TempRecord.V156 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V156", p_TempRecord.V156));
                if (p_TempRecord.V157 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V157", p_TempRecord.V157));
                if (p_TempRecord.V158 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V158", p_TempRecord.V158));
                if (p_TempRecord.V159 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V159", p_TempRecord.V159));
                if (p_TempRecord.V160 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V160", p_TempRecord.V160));
                if (p_TempRecord.V161 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V161", p_TempRecord.V161));
                if (p_TempRecord.V162 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V162", p_TempRecord.V162));
                if (p_TempRecord.V163 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V163", p_TempRecord.V163));
                if (p_TempRecord.V164 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V164", p_TempRecord.V164));
                if (p_TempRecord.V165 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V165", p_TempRecord.V165));
                if (p_TempRecord.V166 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V166", p_TempRecord.V166));
                if (p_TempRecord.V167 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V167", p_TempRecord.V167));
                if (p_TempRecord.V168 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V168", p_TempRecord.V168));
                if (p_TempRecord.V169 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V169", p_TempRecord.V169));
                if (p_TempRecord.V170 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V170", p_TempRecord.V170));
                if (p_TempRecord.V171 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V171", p_TempRecord.V171));
                if (p_TempRecord.V172 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V172", p_TempRecord.V172));
                if (p_TempRecord.V173 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V173", p_TempRecord.V173));
                if (p_TempRecord.V174 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V174", p_TempRecord.V174));
                if (p_TempRecord.V175 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V175", p_TempRecord.V175));
                if (p_TempRecord.V176 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V176", p_TempRecord.V176));
                if (p_TempRecord.V177 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V177", p_TempRecord.V177));
                if (p_TempRecord.V178 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V178", p_TempRecord.V178));
                if (p_TempRecord.V179 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V179", p_TempRecord.V179));
                if (p_TempRecord.V180 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V180", p_TempRecord.V180));
                if (p_TempRecord.V181 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V181", p_TempRecord.V181));
                if (p_TempRecord.V182 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V182", p_TempRecord.V182));
                if (p_TempRecord.V183 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V183", p_TempRecord.V183));
                if (p_TempRecord.V184 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V184", p_TempRecord.V184));
                if (p_TempRecord.V185 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V185", p_TempRecord.V185));
                if (p_TempRecord.V186 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V186", p_TempRecord.V186));
                if (p_TempRecord.V187 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V187", p_TempRecord.V187));
                if (p_TempRecord.V188 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V188", p_TempRecord.V188));
                if (p_TempRecord.V189 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V189", p_TempRecord.V189));
                if (p_TempRecord.V190 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V190", p_TempRecord.V190));
                if (p_TempRecord.V191 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V191", p_TempRecord.V191));
                if (p_TempRecord.V192 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V192", p_TempRecord.V192));
                if (p_TempRecord.V193 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V193", p_TempRecord.V193));
                if (p_TempRecord.V194 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V194", p_TempRecord.V194));
                if (p_TempRecord.V195 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V195", p_TempRecord.V195));
                if (p_TempRecord.V196 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V196", p_TempRecord.V196));
                if (p_TempRecord.V197 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V197", p_TempRecord.V197));
                if (p_TempRecord.V198 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V198", p_TempRecord.V198));
                if (p_TempRecord.V199 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V199", p_TempRecord.V199));
                if (p_TempRecord.V200 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V200", p_TempRecord.V200));


                if (p_TempRecord.V201 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V201", p_TempRecord.V201));
                if (p_TempRecord.V202 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V202", p_TempRecord.V202));
                if (p_TempRecord.V203 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V203", p_TempRecord.V203));
                if (p_TempRecord.V204 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V204", p_TempRecord.V204));
                if (p_TempRecord.V205 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V205", p_TempRecord.V205));
                if (p_TempRecord.V206 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V206", p_TempRecord.V206));
                if (p_TempRecord.V207 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V207", p_TempRecord.V207));
                if (p_TempRecord.V208 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V208", p_TempRecord.V208));
                if (p_TempRecord.V209 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V209", p_TempRecord.V209));
                if (p_TempRecord.V210 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V210", p_TempRecord.V210));
                if (p_TempRecord.V211 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V211", p_TempRecord.V211));
                if (p_TempRecord.V212 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V212", p_TempRecord.V212));
                if (p_TempRecord.V213 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V213", p_TempRecord.V213));
                if (p_TempRecord.V214 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V214", p_TempRecord.V214));
                if (p_TempRecord.V215 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V215", p_TempRecord.V215));
                if (p_TempRecord.V216 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V216", p_TempRecord.V216));
                if (p_TempRecord.V217 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V217", p_TempRecord.V217));
                if (p_TempRecord.V218 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V218", p_TempRecord.V218));
                if (p_TempRecord.V219 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V219", p_TempRecord.V219));
                if (p_TempRecord.V220 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V220", p_TempRecord.V220));
                if (p_TempRecord.V221 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V221", p_TempRecord.V221));
                if (p_TempRecord.V222 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V222", p_TempRecord.V222));
                if (p_TempRecord.V223 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V223", p_TempRecord.V223));
                if (p_TempRecord.V224 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V224", p_TempRecord.V224));
                if (p_TempRecord.V225 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V225", p_TempRecord.V225));
                if (p_TempRecord.V226 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V226", p_TempRecord.V226));
                if (p_TempRecord.V227 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V227", p_TempRecord.V227));
                if (p_TempRecord.V228 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V228", p_TempRecord.V228));
                if (p_TempRecord.V229 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V229", p_TempRecord.V229));
                if (p_TempRecord.V230 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V230", p_TempRecord.V230));
                if (p_TempRecord.V231 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V231", p_TempRecord.V231));
                if (p_TempRecord.V232 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V232", p_TempRecord.V232));
                if (p_TempRecord.V233 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V233", p_TempRecord.V233));
                if (p_TempRecord.V234 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V234", p_TempRecord.V234));
                if (p_TempRecord.V235 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V235", p_TempRecord.V235));
                if (p_TempRecord.V236 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V236", p_TempRecord.V236));
                if (p_TempRecord.V237 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V237", p_TempRecord.V237));
                if (p_TempRecord.V238 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V238", p_TempRecord.V238));
                if (p_TempRecord.V239 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V239", p_TempRecord.V239));
                if (p_TempRecord.V240 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V240", p_TempRecord.V240));
                if (p_TempRecord.V241 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V241", p_TempRecord.V241));
                if (p_TempRecord.V242 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V242", p_TempRecord.V242));
                if (p_TempRecord.V243 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V243", p_TempRecord.V243));
                if (p_TempRecord.V244 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V244", p_TempRecord.V244));
                if (p_TempRecord.V245 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V245", p_TempRecord.V245));
                if (p_TempRecord.V246 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V246", p_TempRecord.V246));
                if (p_TempRecord.V247 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V247", p_TempRecord.V247));
                if (p_TempRecord.V248 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V248", p_TempRecord.V248));
                if (p_TempRecord.V249 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V249", p_TempRecord.V249));
                if (p_TempRecord.V250 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V250", p_TempRecord.V250));



                if (p_TempRecord.V251 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V251", p_TempRecord.V251));
                if (p_TempRecord.V252 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V252", p_TempRecord.V252));
                if (p_TempRecord.V253 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V253", p_TempRecord.V253));
                if (p_TempRecord.V254 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V254", p_TempRecord.V254));
                if (p_TempRecord.V255 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V255", p_TempRecord.V255));
                if (p_TempRecord.V256 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V256", p_TempRecord.V256));
                if (p_TempRecord.V257 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V257", p_TempRecord.V257));
                if (p_TempRecord.V258 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V258", p_TempRecord.V258));
                if (p_TempRecord.V259 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V259", p_TempRecord.V259));
                if (p_TempRecord.V260 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V260", p_TempRecord.V260));
                if (p_TempRecord.V261 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V261", p_TempRecord.V261));
                if (p_TempRecord.V262 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V262", p_TempRecord.V262));
                if (p_TempRecord.V263 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V263", p_TempRecord.V263));
                if (p_TempRecord.V264 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V264", p_TempRecord.V264));
                if (p_TempRecord.V265 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V265", p_TempRecord.V265));
                if (p_TempRecord.V266 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V266", p_TempRecord.V266));
                if (p_TempRecord.V267 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V267", p_TempRecord.V267));
                if (p_TempRecord.V268 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V268", p_TempRecord.V268));
                if (p_TempRecord.V269 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V269", p_TempRecord.V269));
                if (p_TempRecord.V270 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V270", p_TempRecord.V270));
                if (p_TempRecord.V271 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V271", p_TempRecord.V271));
                if (p_TempRecord.V272 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V272", p_TempRecord.V272));
                if (p_TempRecord.V273 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V273", p_TempRecord.V273));
                if (p_TempRecord.V274 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V274", p_TempRecord.V274));
                if (p_TempRecord.V275 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V275", p_TempRecord.V275));
                if (p_TempRecord.V276 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V276", p_TempRecord.V276));
                if (p_TempRecord.V277 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V277", p_TempRecord.V277));
                if (p_TempRecord.V278 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V278", p_TempRecord.V278));
                if (p_TempRecord.V279 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V279", p_TempRecord.V279));
                if (p_TempRecord.V280 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V280", p_TempRecord.V280));
                if (p_TempRecord.V281 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V281", p_TempRecord.V281));
                if (p_TempRecord.V282 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V282", p_TempRecord.V282));
                if (p_TempRecord.V283 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V283", p_TempRecord.V283));
                if (p_TempRecord.V284 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V284", p_TempRecord.V284));
                if (p_TempRecord.V285 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V285", p_TempRecord.V285));
                if (p_TempRecord.V286 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V286", p_TempRecord.V286));
                if (p_TempRecord.V287 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V287", p_TempRecord.V287));
                if (p_TempRecord.V288 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V288", p_TempRecord.V288));
                if (p_TempRecord.V289 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V289", p_TempRecord.V289));
                if (p_TempRecord.V290 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V290", p_TempRecord.V290));
                if (p_TempRecord.V291 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V291", p_TempRecord.V291));
                if (p_TempRecord.V292 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V292", p_TempRecord.V292));
                if (p_TempRecord.V293 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V293", p_TempRecord.V293));
                if (p_TempRecord.V294 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V294", p_TempRecord.V294));
                if (p_TempRecord.V295 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V295", p_TempRecord.V295));
                if (p_TempRecord.V296 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V296", p_TempRecord.V296));
                if (p_TempRecord.V297 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V297", p_TempRecord.V297));
                if (p_TempRecord.V298 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V298", p_TempRecord.V298));
                if (p_TempRecord.V299 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V299", p_TempRecord.V299));
                if (p_TempRecord.V300 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V300", p_TempRecord.V300));


                if (p_TempRecord.V301 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V301", p_TempRecord.V301));
                if (p_TempRecord.V302 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V302", p_TempRecord.V302));
                if (p_TempRecord.V303 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V303", p_TempRecord.V303));
                if (p_TempRecord.V304 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V304", p_TempRecord.V304));
                if (p_TempRecord.V305 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V305", p_TempRecord.V305));
                if (p_TempRecord.V306 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V306", p_TempRecord.V306));
                if (p_TempRecord.V307 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V307", p_TempRecord.V307));
                if (p_TempRecord.V308 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V308", p_TempRecord.V308));
                if (p_TempRecord.V309 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V309", p_TempRecord.V309));
                if (p_TempRecord.V310 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V310", p_TempRecord.V310));
                if (p_TempRecord.V311 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V311", p_TempRecord.V311));
                if (p_TempRecord.V312 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V312", p_TempRecord.V312));
                if (p_TempRecord.V313 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V313", p_TempRecord.V313));
                if (p_TempRecord.V314 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V314", p_TempRecord.V314));
                if (p_TempRecord.V315 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V315", p_TempRecord.V315));
                if (p_TempRecord.V316 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V316", p_TempRecord.V316));
                if (p_TempRecord.V317 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V317", p_TempRecord.V317));
                if (p_TempRecord.V318 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V318", p_TempRecord.V318));
                if (p_TempRecord.V319 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V319", p_TempRecord.V319));
                if (p_TempRecord.V320 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V320", p_TempRecord.V320));
                if (p_TempRecord.V321 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V321", p_TempRecord.V321));
                if (p_TempRecord.V322 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V322", p_TempRecord.V322));
                if (p_TempRecord.V323 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V323", p_TempRecord.V323));
                if (p_TempRecord.V324 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V324", p_TempRecord.V324));
                if (p_TempRecord.V325 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V325", p_TempRecord.V325));
                if (p_TempRecord.V326 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V326", p_TempRecord.V326));
                if (p_TempRecord.V327 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V327", p_TempRecord.V327));
                if (p_TempRecord.V328 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V328", p_TempRecord.V328));
                if (p_TempRecord.V329 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V329", p_TempRecord.V329));
                if (p_TempRecord.V330 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V330", p_TempRecord.V330));
                if (p_TempRecord.V331 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V331", p_TempRecord.V331));
                if (p_TempRecord.V332 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V332", p_TempRecord.V332));
                if (p_TempRecord.V333 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V333", p_TempRecord.V333));
                if (p_TempRecord.V334 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V334", p_TempRecord.V334));
                if (p_TempRecord.V335 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V335", p_TempRecord.V335));
                if (p_TempRecord.V336 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V336", p_TempRecord.V336));
                if (p_TempRecord.V337 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V337", p_TempRecord.V337));
                if (p_TempRecord.V338 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V338", p_TempRecord.V338));
                if (p_TempRecord.V339 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V339", p_TempRecord.V339));
                if (p_TempRecord.V340 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V340", p_TempRecord.V340));
                if (p_TempRecord.V341 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V341", p_TempRecord.V341));
                if (p_TempRecord.V342 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V342", p_TempRecord.V342));
                if (p_TempRecord.V343 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V343", p_TempRecord.V343));
                if (p_TempRecord.V344 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V344", p_TempRecord.V344));
                if (p_TempRecord.V345 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V345", p_TempRecord.V345));
                if (p_TempRecord.V346 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V346", p_TempRecord.V346));
                if (p_TempRecord.V347 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V347", p_TempRecord.V347));
                if (p_TempRecord.V348 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V348", p_TempRecord.V348));
                if (p_TempRecord.V349 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V349", p_TempRecord.V349));
                if (p_TempRecord.V350 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V350", p_TempRecord.V350));



                if (p_TempRecord.V351 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V351", p_TempRecord.V351));
                if (p_TempRecord.V352 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V352", p_TempRecord.V352));
                if (p_TempRecord.V353 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V353", p_TempRecord.V353));
                if (p_TempRecord.V354 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V354", p_TempRecord.V354));
                if (p_TempRecord.V355 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V355", p_TempRecord.V355));
                if (p_TempRecord.V356 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V356", p_TempRecord.V356));
                if (p_TempRecord.V357 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V357", p_TempRecord.V357));
                if (p_TempRecord.V358 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V358", p_TempRecord.V358));
                if (p_TempRecord.V359 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V359", p_TempRecord.V359));
                if (p_TempRecord.V360 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V360", p_TempRecord.V360));
                if (p_TempRecord.V361 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V361", p_TempRecord.V361));
                if (p_TempRecord.V362 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V362", p_TempRecord.V362));
                if (p_TempRecord.V363 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V363", p_TempRecord.V363));
                if (p_TempRecord.V364 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V364", p_TempRecord.V364));
                if (p_TempRecord.V365 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V365", p_TempRecord.V365));
                if (p_TempRecord.V366 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V366", p_TempRecord.V366));
                if (p_TempRecord.V367 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V367", p_TempRecord.V367));
                if (p_TempRecord.V368 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V368", p_TempRecord.V368));
                if (p_TempRecord.V369 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V369", p_TempRecord.V369));
                if (p_TempRecord.V370 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V370", p_TempRecord.V370));
                if (p_TempRecord.V371 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V371", p_TempRecord.V371));
                if (p_TempRecord.V372 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V372", p_TempRecord.V372));
                if (p_TempRecord.V373 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V373", p_TempRecord.V373));
                if (p_TempRecord.V374 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V374", p_TempRecord.V374));
                if (p_TempRecord.V375 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V375", p_TempRecord.V375));
                if (p_TempRecord.V376 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V376", p_TempRecord.V376));
                if (p_TempRecord.V377 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V377", p_TempRecord.V377));
                if (p_TempRecord.V378 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V378", p_TempRecord.V378));
                if (p_TempRecord.V379 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V379", p_TempRecord.V379));
                if (p_TempRecord.V380 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V380", p_TempRecord.V380));
                if (p_TempRecord.V381 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V381", p_TempRecord.V381));
                if (p_TempRecord.V382 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V382", p_TempRecord.V382));
                if (p_TempRecord.V383 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V383", p_TempRecord.V383));
                if (p_TempRecord.V384 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V384", p_TempRecord.V384));
                if (p_TempRecord.V385 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V385", p_TempRecord.V385));
                if (p_TempRecord.V386 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V386", p_TempRecord.V386));
                if (p_TempRecord.V387 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V387", p_TempRecord.V387));
                if (p_TempRecord.V388 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V388", p_TempRecord.V388));
                if (p_TempRecord.V389 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V389", p_TempRecord.V389));
                if (p_TempRecord.V390 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V390", p_TempRecord.V390));
                if (p_TempRecord.V391 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V391", p_TempRecord.V391));
                if (p_TempRecord.V392 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V392", p_TempRecord.V392));
                if (p_TempRecord.V393 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V393", p_TempRecord.V393));
                if (p_TempRecord.V394 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V394", p_TempRecord.V394));
                if (p_TempRecord.V395 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V395", p_TempRecord.V395));
                if (p_TempRecord.V396 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V396", p_TempRecord.V396));
                if (p_TempRecord.V397 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V397", p_TempRecord.V397));
                if (p_TempRecord.V398 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V398", p_TempRecord.V398));
                if (p_TempRecord.V399 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V399", p_TempRecord.V399));
                if (p_TempRecord.V400 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V400", p_TempRecord.V400));


                if (p_TempRecord.V401 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V401", p_TempRecord.V401));
                if (p_TempRecord.V402 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V402", p_TempRecord.V402));
                if (p_TempRecord.V403 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V403", p_TempRecord.V403));
                if (p_TempRecord.V404 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V404", p_TempRecord.V404));
                if (p_TempRecord.V405 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V405", p_TempRecord.V405));
                if (p_TempRecord.V406 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V406", p_TempRecord.V406));
                if (p_TempRecord.V407 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V407", p_TempRecord.V407));
                if (p_TempRecord.V408 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V408", p_TempRecord.V408));
                if (p_TempRecord.V409 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V409", p_TempRecord.V409));
                if (p_TempRecord.V410 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V410", p_TempRecord.V410));
                if (p_TempRecord.V411 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V411", p_TempRecord.V411));
                if (p_TempRecord.V412 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V412", p_TempRecord.V412));
                if (p_TempRecord.V413 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V413", p_TempRecord.V413));
                if (p_TempRecord.V414 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V414", p_TempRecord.V414));
                if (p_TempRecord.V415 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V415", p_TempRecord.V415));
                if (p_TempRecord.V416 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V416", p_TempRecord.V416));
                if (p_TempRecord.V417 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V417", p_TempRecord.V417));
                if (p_TempRecord.V418 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V418", p_TempRecord.V418));
                if (p_TempRecord.V419 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V419", p_TempRecord.V419));
                if (p_TempRecord.V420 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V420", p_TempRecord.V420));
                if (p_TempRecord.V421 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V421", p_TempRecord.V421));
                if (p_TempRecord.V422 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V422", p_TempRecord.V422));
                if (p_TempRecord.V423 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V423", p_TempRecord.V423));
                if (p_TempRecord.V424 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V424", p_TempRecord.V424));
                if (p_TempRecord.V425 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V425", p_TempRecord.V425));
                if (p_TempRecord.V426 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V426", p_TempRecord.V426));
                if (p_TempRecord.V427 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V427", p_TempRecord.V427));
                if (p_TempRecord.V428 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V428", p_TempRecord.V428));
                if (p_TempRecord.V429 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V429", p_TempRecord.V429));
                if (p_TempRecord.V430 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V430", p_TempRecord.V430));
                if (p_TempRecord.V431 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V431", p_TempRecord.V431));
                if (p_TempRecord.V432 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V432", p_TempRecord.V432));
                if (p_TempRecord.V433 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V433", p_TempRecord.V433));
                if (p_TempRecord.V434 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V434", p_TempRecord.V434));
                if (p_TempRecord.V435 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V435", p_TempRecord.V435));
                if (p_TempRecord.V436 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V436", p_TempRecord.V436));
                if (p_TempRecord.V437 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V437", p_TempRecord.V437));
                if (p_TempRecord.V438 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V438", p_TempRecord.V438));
                if (p_TempRecord.V439 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V439", p_TempRecord.V439));
                if (p_TempRecord.V440 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V440", p_TempRecord.V440));
                if (p_TempRecord.V441 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V441", p_TempRecord.V441));
                if (p_TempRecord.V442 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V442", p_TempRecord.V442));
                if (p_TempRecord.V443 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V443", p_TempRecord.V443));
                if (p_TempRecord.V444 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V444", p_TempRecord.V444));
                if (p_TempRecord.V445 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V445", p_TempRecord.V445));
                if (p_TempRecord.V446 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V446", p_TempRecord.V446));
                if (p_TempRecord.V447 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V447", p_TempRecord.V447));
                if (p_TempRecord.V448 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V448", p_TempRecord.V448));
                if (p_TempRecord.V449 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V449", p_TempRecord.V449));
                if (p_TempRecord.V450 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V450", p_TempRecord.V450));



                if (p_TempRecord.V451 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V451", p_TempRecord.V451));
                if (p_TempRecord.V452 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V452", p_TempRecord.V452));
                if (p_TempRecord.V453 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V453", p_TempRecord.V453));
                if (p_TempRecord.V454 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V454", p_TempRecord.V454));
                if (p_TempRecord.V455 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V455", p_TempRecord.V455));
                if (p_TempRecord.V456 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V456", p_TempRecord.V456));
                if (p_TempRecord.V457 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V457", p_TempRecord.V457));
                if (p_TempRecord.V458 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V458", p_TempRecord.V458));
                if (p_TempRecord.V459 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V459", p_TempRecord.V459));
                if (p_TempRecord.V460 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V460", p_TempRecord.V460));
                if (p_TempRecord.V461 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V461", p_TempRecord.V461));
                if (p_TempRecord.V462 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V462", p_TempRecord.V462));
                if (p_TempRecord.V463 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V463", p_TempRecord.V463));
                if (p_TempRecord.V464 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V464", p_TempRecord.V464));
                if (p_TempRecord.V465 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V465", p_TempRecord.V465));
                if (p_TempRecord.V466 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V466", p_TempRecord.V466));
                if (p_TempRecord.V467 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V467", p_TempRecord.V467));
                if (p_TempRecord.V468 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V468", p_TempRecord.V468));
                if (p_TempRecord.V469 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V469", p_TempRecord.V469));
                if (p_TempRecord.V470 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V470", p_TempRecord.V470));
                if (p_TempRecord.V471 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V471", p_TempRecord.V471));
                if (p_TempRecord.V472 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V472", p_TempRecord.V472));
                if (p_TempRecord.V473 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V473", p_TempRecord.V473));
                if (p_TempRecord.V474 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V474", p_TempRecord.V474));
                if (p_TempRecord.V475 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V475", p_TempRecord.V475));
                if (p_TempRecord.V476 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V476", p_TempRecord.V476));
                if (p_TempRecord.V477 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V477", p_TempRecord.V477));
                if (p_TempRecord.V478 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V478", p_TempRecord.V478));
                if (p_TempRecord.V479 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V479", p_TempRecord.V479));
                if (p_TempRecord.V480 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V480", p_TempRecord.V480));
                if (p_TempRecord.V481 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V481", p_TempRecord.V481));
                if (p_TempRecord.V482 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V482", p_TempRecord.V482));
                if (p_TempRecord.V483 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V483", p_TempRecord.V483));
                if (p_TempRecord.V484 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V484", p_TempRecord.V484));
                if (p_TempRecord.V485 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V485", p_TempRecord.V485));
                if (p_TempRecord.V486 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V486", p_TempRecord.V486));
                if (p_TempRecord.V487 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V487", p_TempRecord.V487));
                if (p_TempRecord.V488 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V488", p_TempRecord.V488));
                if (p_TempRecord.V489 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V489", p_TempRecord.V489));
                if (p_TempRecord.V490 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V490", p_TempRecord.V490));
                if (p_TempRecord.V491 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V491", p_TempRecord.V491));
                if (p_TempRecord.V492 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V492", p_TempRecord.V492));
                if (p_TempRecord.V493 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V493", p_TempRecord.V493));
                if (p_TempRecord.V494 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V494", p_TempRecord.V494));
                if (p_TempRecord.V495 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V495", p_TempRecord.V495));
                if (p_TempRecord.V496 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V496", p_TempRecord.V496));
                if (p_TempRecord.V497 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V497", p_TempRecord.V497));
                if (p_TempRecord.V498 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V498", p_TempRecord.V498));
                if (p_TempRecord.V499 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V499", p_TempRecord.V499));
                if (p_TempRecord.V500 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V500", p_TempRecord.V500));


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



    public static int ets_TempRecord_Update(TempRecord p_TempRecord)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TempRecord_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTempRecordID", p_TempRecord.TempRecordID));

                command.Parameters.Add(new SqlParameter("@nAccountID", p_TempRecord.AccountID));
                command.Parameters.Add(new SqlParameter("@nBatchID", p_TempRecord.BatchID));

                //if (p_TempRecord.LocationID != null)
                //    command.Parameters.Add(new SqlParameter("@nLocationID", p_TempRecord.LocationID));

                //if (p_TempRecord.LocationName != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sLocationName", p_TempRecord.LocationName));

                if (p_TempRecord.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_TempRecord.TableID));

                if (p_TempRecord.DateTimeRecorded != null)
                    command.Parameters.Add(new SqlParameter("@dDateTimeRecorded", p_TempRecord.DateTimeRecorded));

                if (p_TempRecord.Notes != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sNotes", p_TempRecord.Notes));


                if (p_TempRecord.IsActive == null)
                    p_TempRecord.IsActive = true;
                command.Parameters.Add(new SqlParameter("@bIsActive", p_TempRecord.IsActive));

                if (p_TempRecord.IsValidated != null)
                    command.Parameters.Add(new SqlParameter("@bIsValidated", p_TempRecord.IsValidated));

                if (p_TempRecord.RejectReason != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sRejectReason", p_TempRecord.RejectReason));

                if (p_TempRecord.WarningReason != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sWarningReason", p_TempRecord.WarningReason));

                //if (p_TempRecord.ExceedanceReason != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sExceedanceReason", p_TempRecord.ExceedanceReason));

                if (p_TempRecord.ValidationResults != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sValidationResults", p_TempRecord.ValidationResults));





                if (p_TempRecord.V001 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V001", p_TempRecord.V001));
                if (p_TempRecord.V002 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V002", p_TempRecord.V002));
                if (p_TempRecord.V003 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V003", p_TempRecord.V003));
                if (p_TempRecord.V004 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V004", p_TempRecord.V004));
                if (p_TempRecord.V005 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V005", p_TempRecord.V005));
                if (p_TempRecord.V006 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V006", p_TempRecord.V006));
                if (p_TempRecord.V007 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V007", p_TempRecord.V007));
                if (p_TempRecord.V008 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V008", p_TempRecord.V008));
                if (p_TempRecord.V009 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V009", p_TempRecord.V009));
                if (p_TempRecord.V010 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V010", p_TempRecord.V010));
                if (p_TempRecord.V011 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V011", p_TempRecord.V011));
                if (p_TempRecord.V012 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V012", p_TempRecord.V012));
                if (p_TempRecord.V013 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V013", p_TempRecord.V013));
                if (p_TempRecord.V014 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V014", p_TempRecord.V014));
                if (p_TempRecord.V015 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V015", p_TempRecord.V015));
                if (p_TempRecord.V016 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V016", p_TempRecord.V016));
                if (p_TempRecord.V017 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V017", p_TempRecord.V017));
                if (p_TempRecord.V018 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V018", p_TempRecord.V018));
                if (p_TempRecord.V019 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V019", p_TempRecord.V019));
                if (p_TempRecord.V020 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V020", p_TempRecord.V020));
                if (p_TempRecord.V021 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V021", p_TempRecord.V021));
                if (p_TempRecord.V022 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V022", p_TempRecord.V022));
                if (p_TempRecord.V023 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V023", p_TempRecord.V023));
                if (p_TempRecord.V024 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V024", p_TempRecord.V024));
                if (p_TempRecord.V025 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V025", p_TempRecord.V025));
                if (p_TempRecord.V026 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V026", p_TempRecord.V026));
                if (p_TempRecord.V027 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V027", p_TempRecord.V027));
                if (p_TempRecord.V028 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V028", p_TempRecord.V028));
                if (p_TempRecord.V029 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V029", p_TempRecord.V029));
                if (p_TempRecord.V030 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V030", p_TempRecord.V030));
                if (p_TempRecord.V031 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V031", p_TempRecord.V031));
                if (p_TempRecord.V032 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V032", p_TempRecord.V032));
                if (p_TempRecord.V033 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V033", p_TempRecord.V033));
                if (p_TempRecord.V034 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V034", p_TempRecord.V034));
                if (p_TempRecord.V035 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V035", p_TempRecord.V035));
                if (p_TempRecord.V036 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V036", p_TempRecord.V036));
                if (p_TempRecord.V037 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V037", p_TempRecord.V037));
                if (p_TempRecord.V038 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V038", p_TempRecord.V038));
                if (p_TempRecord.V039 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V039", p_TempRecord.V039));
                if (p_TempRecord.V040 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V040", p_TempRecord.V040));
                if (p_TempRecord.V041 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V041", p_TempRecord.V041));
                if (p_TempRecord.V042 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V042", p_TempRecord.V042));
                if (p_TempRecord.V043 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V043", p_TempRecord.V043));
                if (p_TempRecord.V044 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V044", p_TempRecord.V044));
                if (p_TempRecord.V045 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V045", p_TempRecord.V045));
                if (p_TempRecord.V046 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V046", p_TempRecord.V046));
                if (p_TempRecord.V047 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V047", p_TempRecord.V047));
                if (p_TempRecord.V048 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V048", p_TempRecord.V048));
                if (p_TempRecord.V049 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V049", p_TempRecord.V049));
                if (p_TempRecord.V050 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V050", p_TempRecord.V050));

                if (p_TempRecord.V051 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V051", p_TempRecord.V051));
                if (p_TempRecord.V052 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V052", p_TempRecord.V052));
                if (p_TempRecord.V053 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V053", p_TempRecord.V053));
                if (p_TempRecord.V054 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V054", p_TempRecord.V054));
                if (p_TempRecord.V055 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V055", p_TempRecord.V055));
                if (p_TempRecord.V056 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V056", p_TempRecord.V056));
                if (p_TempRecord.V057 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V057", p_TempRecord.V057));
                if (p_TempRecord.V058 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V058", p_TempRecord.V058));
                if (p_TempRecord.V059 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V059", p_TempRecord.V059));
                if (p_TempRecord.V060 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V060", p_TempRecord.V060));
                if (p_TempRecord.V061 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V061", p_TempRecord.V061));
                if (p_TempRecord.V062 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V062", p_TempRecord.V062));
                if (p_TempRecord.V063 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V063", p_TempRecord.V063));
                if (p_TempRecord.V064 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V064", p_TempRecord.V064));
                if (p_TempRecord.V065 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V065", p_TempRecord.V065));
                if (p_TempRecord.V066 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V066", p_TempRecord.V066));
                if (p_TempRecord.V067 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V067", p_TempRecord.V067));
                if (p_TempRecord.V068 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V068", p_TempRecord.V068));
                if (p_TempRecord.V069 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V069", p_TempRecord.V069));
                if (p_TempRecord.V070 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V070", p_TempRecord.V070));
                if (p_TempRecord.V071 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V071", p_TempRecord.V071));
                if (p_TempRecord.V072 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V072", p_TempRecord.V072));
                if (p_TempRecord.V073 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V073", p_TempRecord.V073));
                if (p_TempRecord.V074 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V074", p_TempRecord.V074));
                if (p_TempRecord.V075 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V075", p_TempRecord.V075));
                if (p_TempRecord.V076 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V076", p_TempRecord.V076));
                if (p_TempRecord.V077 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V077", p_TempRecord.V077));
                if (p_TempRecord.V078 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V078", p_TempRecord.V078));
                if (p_TempRecord.V079 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V079", p_TempRecord.V079));
                if (p_TempRecord.V080 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V080", p_TempRecord.V080));
                if (p_TempRecord.V081 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V081", p_TempRecord.V081));
                if (p_TempRecord.V082 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V082", p_TempRecord.V082));
                if (p_TempRecord.V083 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V083", p_TempRecord.V083));
                if (p_TempRecord.V084 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V084", p_TempRecord.V084));
                if (p_TempRecord.V085 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V085", p_TempRecord.V085));
                if (p_TempRecord.V086 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V086", p_TempRecord.V086));
                if (p_TempRecord.V087 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V087", p_TempRecord.V087));
                if (p_TempRecord.V088 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V088", p_TempRecord.V088));
                if (p_TempRecord.V089 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V089", p_TempRecord.V089));
                if (p_TempRecord.V090 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V090", p_TempRecord.V090));
                if (p_TempRecord.V091 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V091", p_TempRecord.V091));
                if (p_TempRecord.V092 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V092", p_TempRecord.V092));
                if (p_TempRecord.V093 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V093", p_TempRecord.V093));
                if (p_TempRecord.V094 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V094", p_TempRecord.V094));
                if (p_TempRecord.V095 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V095", p_TempRecord.V095));
                if (p_TempRecord.V096 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V096", p_TempRecord.V096));
                if (p_TempRecord.V097 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V097", p_TempRecord.V097));
                if (p_TempRecord.V098 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V098", p_TempRecord.V098));
                if (p_TempRecord.V099 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V099", p_TempRecord.V099));
                if (p_TempRecord.V100 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V100", p_TempRecord.V100));


                if (p_TempRecord.V101 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V101", p_TempRecord.V101));
                if (p_TempRecord.V102 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V102", p_TempRecord.V102));
                if (p_TempRecord.V103 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V103", p_TempRecord.V103));
                if (p_TempRecord.V104 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V104", p_TempRecord.V104));
                if (p_TempRecord.V105 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V105", p_TempRecord.V105));
                if (p_TempRecord.V106 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V106", p_TempRecord.V106));
                if (p_TempRecord.V107 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V107", p_TempRecord.V107));
                if (p_TempRecord.V108 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V108", p_TempRecord.V108));
                if (p_TempRecord.V109 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V109", p_TempRecord.V109));
                if (p_TempRecord.V110 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V110", p_TempRecord.V110));
                if (p_TempRecord.V111 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V111", p_TempRecord.V111));
                if (p_TempRecord.V112 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V112", p_TempRecord.V112));
                if (p_TempRecord.V113 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V113", p_TempRecord.V113));
                if (p_TempRecord.V114 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V114", p_TempRecord.V114));
                if (p_TempRecord.V115 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V115", p_TempRecord.V115));
                if (p_TempRecord.V116 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V116", p_TempRecord.V116));
                if (p_TempRecord.V117 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V117", p_TempRecord.V117));
                if (p_TempRecord.V118 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V118", p_TempRecord.V118));
                if (p_TempRecord.V119 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V119", p_TempRecord.V119));
                if (p_TempRecord.V120 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V120", p_TempRecord.V120));
                if (p_TempRecord.V121 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V121", p_TempRecord.V121));
                if (p_TempRecord.V122 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V122", p_TempRecord.V122));
                if (p_TempRecord.V123 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V123", p_TempRecord.V123));
                if (p_TempRecord.V124 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V124", p_TempRecord.V124));
                if (p_TempRecord.V125 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V125", p_TempRecord.V125));
                if (p_TempRecord.V126 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V126", p_TempRecord.V126));
                if (p_TempRecord.V127 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V127", p_TempRecord.V127));
                if (p_TempRecord.V128 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V128", p_TempRecord.V128));
                if (p_TempRecord.V129 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V129", p_TempRecord.V129));
                if (p_TempRecord.V130 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V130", p_TempRecord.V130));
                if (p_TempRecord.V131 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V131", p_TempRecord.V131));
                if (p_TempRecord.V132 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V132", p_TempRecord.V132));
                if (p_TempRecord.V133 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V133", p_TempRecord.V133));
                if (p_TempRecord.V134 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V134", p_TempRecord.V134));
                if (p_TempRecord.V135 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V135", p_TempRecord.V135));
                if (p_TempRecord.V136 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V136", p_TempRecord.V136));
                if (p_TempRecord.V137 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V137", p_TempRecord.V137));
                if (p_TempRecord.V138 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V138", p_TempRecord.V138));
                if (p_TempRecord.V139 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V139", p_TempRecord.V139));
                if (p_TempRecord.V140 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V140", p_TempRecord.V140));
                if (p_TempRecord.V141 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V141", p_TempRecord.V141));
                if (p_TempRecord.V142 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V142", p_TempRecord.V142));
                if (p_TempRecord.V143 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V143", p_TempRecord.V143));
                if (p_TempRecord.V144 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V144", p_TempRecord.V144));
                if (p_TempRecord.V145 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V145", p_TempRecord.V145));
                if (p_TempRecord.V146 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V146", p_TempRecord.V146));
                if (p_TempRecord.V147 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V147", p_TempRecord.V147));
                if (p_TempRecord.V148 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V148", p_TempRecord.V148));
                if (p_TempRecord.V149 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V149", p_TempRecord.V149));
                if (p_TempRecord.V150 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V150", p_TempRecord.V150));



                if (p_TempRecord.V151 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V151", p_TempRecord.V151));
                if (p_TempRecord.V152 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V152", p_TempRecord.V152));
                if (p_TempRecord.V153 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V153", p_TempRecord.V153));
                if (p_TempRecord.V154 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V154", p_TempRecord.V154));
                if (p_TempRecord.V155 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V155", p_TempRecord.V155));
                if (p_TempRecord.V156 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V156", p_TempRecord.V156));
                if (p_TempRecord.V157 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V157", p_TempRecord.V157));
                if (p_TempRecord.V158 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V158", p_TempRecord.V158));
                if (p_TempRecord.V159 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V159", p_TempRecord.V159));
                if (p_TempRecord.V160 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V160", p_TempRecord.V160));
                if (p_TempRecord.V161 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V161", p_TempRecord.V161));
                if (p_TempRecord.V162 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V162", p_TempRecord.V162));
                if (p_TempRecord.V163 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V163", p_TempRecord.V163));
                if (p_TempRecord.V164 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V164", p_TempRecord.V164));
                if (p_TempRecord.V165 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V165", p_TempRecord.V165));
                if (p_TempRecord.V166 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V166", p_TempRecord.V166));
                if (p_TempRecord.V167 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V167", p_TempRecord.V167));
                if (p_TempRecord.V168 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V168", p_TempRecord.V168));
                if (p_TempRecord.V169 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V169", p_TempRecord.V169));
                if (p_TempRecord.V170 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V170", p_TempRecord.V170));
                if (p_TempRecord.V171 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V171", p_TempRecord.V171));
                if (p_TempRecord.V172 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V172", p_TempRecord.V172));
                if (p_TempRecord.V173 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V173", p_TempRecord.V173));
                if (p_TempRecord.V174 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V174", p_TempRecord.V174));
                if (p_TempRecord.V175 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V175", p_TempRecord.V175));
                if (p_TempRecord.V176 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V176", p_TempRecord.V176));
                if (p_TempRecord.V177 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V177", p_TempRecord.V177));
                if (p_TempRecord.V178 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V178", p_TempRecord.V178));
                if (p_TempRecord.V179 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V179", p_TempRecord.V179));
                if (p_TempRecord.V180 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V180", p_TempRecord.V180));
                if (p_TempRecord.V181 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V181", p_TempRecord.V181));
                if (p_TempRecord.V182 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V182", p_TempRecord.V182));
                if (p_TempRecord.V183 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V183", p_TempRecord.V183));
                if (p_TempRecord.V184 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V184", p_TempRecord.V184));
                if (p_TempRecord.V185 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V185", p_TempRecord.V185));
                if (p_TempRecord.V186 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V186", p_TempRecord.V186));
                if (p_TempRecord.V187 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V187", p_TempRecord.V187));
                if (p_TempRecord.V188 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V188", p_TempRecord.V188));
                if (p_TempRecord.V189 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V189", p_TempRecord.V189));
                if (p_TempRecord.V190 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V190", p_TempRecord.V190));
                if (p_TempRecord.V191 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V191", p_TempRecord.V191));
                if (p_TempRecord.V192 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V192", p_TempRecord.V192));
                if (p_TempRecord.V193 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V193", p_TempRecord.V193));
                if (p_TempRecord.V194 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V194", p_TempRecord.V194));
                if (p_TempRecord.V195 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V195", p_TempRecord.V195));
                if (p_TempRecord.V196 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V196", p_TempRecord.V196));
                if (p_TempRecord.V197 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V197", p_TempRecord.V197));
                if (p_TempRecord.V198 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V198", p_TempRecord.V198));
                if (p_TempRecord.V199 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V199", p_TempRecord.V199));
                if (p_TempRecord.V200 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V200", p_TempRecord.V200));


                if (p_TempRecord.V201 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V201", p_TempRecord.V201));
                if (p_TempRecord.V202 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V202", p_TempRecord.V202));
                if (p_TempRecord.V203 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V203", p_TempRecord.V203));
                if (p_TempRecord.V204 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V204", p_TempRecord.V204));
                if (p_TempRecord.V205 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V205", p_TempRecord.V205));
                if (p_TempRecord.V206 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V206", p_TempRecord.V206));
                if (p_TempRecord.V207 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V207", p_TempRecord.V207));
                if (p_TempRecord.V208 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V208", p_TempRecord.V208));
                if (p_TempRecord.V209 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V209", p_TempRecord.V209));
                if (p_TempRecord.V210 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V210", p_TempRecord.V210));
                if (p_TempRecord.V211 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V211", p_TempRecord.V211));
                if (p_TempRecord.V212 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V212", p_TempRecord.V212));
                if (p_TempRecord.V213 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V213", p_TempRecord.V213));
                if (p_TempRecord.V214 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V214", p_TempRecord.V214));
                if (p_TempRecord.V215 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V215", p_TempRecord.V215));
                if (p_TempRecord.V216 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V216", p_TempRecord.V216));
                if (p_TempRecord.V217 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V217", p_TempRecord.V217));
                if (p_TempRecord.V218 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V218", p_TempRecord.V218));
                if (p_TempRecord.V219 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V219", p_TempRecord.V219));
                if (p_TempRecord.V220 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V220", p_TempRecord.V220));
                if (p_TempRecord.V221 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V221", p_TempRecord.V221));
                if (p_TempRecord.V222 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V222", p_TempRecord.V222));
                if (p_TempRecord.V223 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V223", p_TempRecord.V223));
                if (p_TempRecord.V224 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V224", p_TempRecord.V224));
                if (p_TempRecord.V225 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V225", p_TempRecord.V225));
                if (p_TempRecord.V226 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V226", p_TempRecord.V226));
                if (p_TempRecord.V227 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V227", p_TempRecord.V227));
                if (p_TempRecord.V228 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V228", p_TempRecord.V228));
                if (p_TempRecord.V229 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V229", p_TempRecord.V229));
                if (p_TempRecord.V230 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V230", p_TempRecord.V230));
                if (p_TempRecord.V231 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V231", p_TempRecord.V231));
                if (p_TempRecord.V232 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V232", p_TempRecord.V232));
                if (p_TempRecord.V233 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V233", p_TempRecord.V233));
                if (p_TempRecord.V234 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V234", p_TempRecord.V234));
                if (p_TempRecord.V235 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V235", p_TempRecord.V235));
                if (p_TempRecord.V236 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V236", p_TempRecord.V236));
                if (p_TempRecord.V237 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V237", p_TempRecord.V237));
                if (p_TempRecord.V238 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V238", p_TempRecord.V238));
                if (p_TempRecord.V239 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V239", p_TempRecord.V239));
                if (p_TempRecord.V240 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V240", p_TempRecord.V240));
                if (p_TempRecord.V241 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V241", p_TempRecord.V241));
                if (p_TempRecord.V242 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V242", p_TempRecord.V242));
                if (p_TempRecord.V243 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V243", p_TempRecord.V243));
                if (p_TempRecord.V244 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V244", p_TempRecord.V244));
                if (p_TempRecord.V245 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V245", p_TempRecord.V245));
                if (p_TempRecord.V246 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V246", p_TempRecord.V246));
                if (p_TempRecord.V247 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V247", p_TempRecord.V247));
                if (p_TempRecord.V248 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V248", p_TempRecord.V248));
                if (p_TempRecord.V249 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V249", p_TempRecord.V249));
                if (p_TempRecord.V250 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V250", p_TempRecord.V250));



                if (p_TempRecord.V251 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V251", p_TempRecord.V251));
                if (p_TempRecord.V252 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V252", p_TempRecord.V252));
                if (p_TempRecord.V253 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V253", p_TempRecord.V253));
                if (p_TempRecord.V254 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V254", p_TempRecord.V254));
                if (p_TempRecord.V255 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V255", p_TempRecord.V255));
                if (p_TempRecord.V256 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V256", p_TempRecord.V256));
                if (p_TempRecord.V257 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V257", p_TempRecord.V257));
                if (p_TempRecord.V258 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V258", p_TempRecord.V258));
                if (p_TempRecord.V259 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V259", p_TempRecord.V259));
                if (p_TempRecord.V260 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V260", p_TempRecord.V260));
                if (p_TempRecord.V261 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V261", p_TempRecord.V261));
                if (p_TempRecord.V262 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V262", p_TempRecord.V262));
                if (p_TempRecord.V263 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V263", p_TempRecord.V263));
                if (p_TempRecord.V264 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V264", p_TempRecord.V264));
                if (p_TempRecord.V265 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V265", p_TempRecord.V265));
                if (p_TempRecord.V266 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V266", p_TempRecord.V266));
                if (p_TempRecord.V267 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V267", p_TempRecord.V267));
                if (p_TempRecord.V268 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V268", p_TempRecord.V268));
                if (p_TempRecord.V269 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V269", p_TempRecord.V269));
                if (p_TempRecord.V270 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V270", p_TempRecord.V270));
                if (p_TempRecord.V271 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V271", p_TempRecord.V271));
                if (p_TempRecord.V272 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V272", p_TempRecord.V272));
                if (p_TempRecord.V273 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V273", p_TempRecord.V273));
                if (p_TempRecord.V274 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V274", p_TempRecord.V274));
                if (p_TempRecord.V275 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V275", p_TempRecord.V275));
                if (p_TempRecord.V276 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V276", p_TempRecord.V276));
                if (p_TempRecord.V277 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V277", p_TempRecord.V277));
                if (p_TempRecord.V278 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V278", p_TempRecord.V278));
                if (p_TempRecord.V279 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V279", p_TempRecord.V279));
                if (p_TempRecord.V280 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V280", p_TempRecord.V280));
                if (p_TempRecord.V281 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V281", p_TempRecord.V281));
                if (p_TempRecord.V282 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V282", p_TempRecord.V282));
                if (p_TempRecord.V283 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V283", p_TempRecord.V283));
                if (p_TempRecord.V284 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V284", p_TempRecord.V284));
                if (p_TempRecord.V285 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V285", p_TempRecord.V285));
                if (p_TempRecord.V286 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V286", p_TempRecord.V286));
                if (p_TempRecord.V287 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V287", p_TempRecord.V287));
                if (p_TempRecord.V288 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V288", p_TempRecord.V288));
                if (p_TempRecord.V289 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V289", p_TempRecord.V289));
                if (p_TempRecord.V290 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V290", p_TempRecord.V290));
                if (p_TempRecord.V291 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V291", p_TempRecord.V291));
                if (p_TempRecord.V292 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V292", p_TempRecord.V292));
                if (p_TempRecord.V293 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V293", p_TempRecord.V293));
                if (p_TempRecord.V294 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V294", p_TempRecord.V294));
                if (p_TempRecord.V295 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V295", p_TempRecord.V295));
                if (p_TempRecord.V296 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V296", p_TempRecord.V296));
                if (p_TempRecord.V297 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V297", p_TempRecord.V297));
                if (p_TempRecord.V298 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V298", p_TempRecord.V298));
                if (p_TempRecord.V299 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V299", p_TempRecord.V299));
                if (p_TempRecord.V300 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V300", p_TempRecord.V300));


                if (p_TempRecord.V301 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V301", p_TempRecord.V301));
                if (p_TempRecord.V302 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V302", p_TempRecord.V302));
                if (p_TempRecord.V303 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V303", p_TempRecord.V303));
                if (p_TempRecord.V304 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V304", p_TempRecord.V304));
                if (p_TempRecord.V305 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V305", p_TempRecord.V305));
                if (p_TempRecord.V306 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V306", p_TempRecord.V306));
                if (p_TempRecord.V307 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V307", p_TempRecord.V307));
                if (p_TempRecord.V308 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V308", p_TempRecord.V308));
                if (p_TempRecord.V309 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V309", p_TempRecord.V309));
                if (p_TempRecord.V310 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V310", p_TempRecord.V310));
                if (p_TempRecord.V311 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V311", p_TempRecord.V311));
                if (p_TempRecord.V312 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V312", p_TempRecord.V312));
                if (p_TempRecord.V313 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V313", p_TempRecord.V313));
                if (p_TempRecord.V314 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V314", p_TempRecord.V314));
                if (p_TempRecord.V315 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V315", p_TempRecord.V315));
                if (p_TempRecord.V316 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V316", p_TempRecord.V316));
                if (p_TempRecord.V317 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V317", p_TempRecord.V317));
                if (p_TempRecord.V318 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V318", p_TempRecord.V318));
                if (p_TempRecord.V319 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V319", p_TempRecord.V319));
                if (p_TempRecord.V320 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V320", p_TempRecord.V320));
                if (p_TempRecord.V321 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V321", p_TempRecord.V321));
                if (p_TempRecord.V322 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V322", p_TempRecord.V322));
                if (p_TempRecord.V323 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V323", p_TempRecord.V323));
                if (p_TempRecord.V324 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V324", p_TempRecord.V324));
                if (p_TempRecord.V325 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V325", p_TempRecord.V325));
                if (p_TempRecord.V326 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V326", p_TempRecord.V326));
                if (p_TempRecord.V327 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V327", p_TempRecord.V327));
                if (p_TempRecord.V328 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V328", p_TempRecord.V328));
                if (p_TempRecord.V329 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V329", p_TempRecord.V329));
                if (p_TempRecord.V330 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V330", p_TempRecord.V330));
                if (p_TempRecord.V331 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V331", p_TempRecord.V331));
                if (p_TempRecord.V332 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V332", p_TempRecord.V332));
                if (p_TempRecord.V333 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V333", p_TempRecord.V333));
                if (p_TempRecord.V334 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V334", p_TempRecord.V334));
                if (p_TempRecord.V335 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V335", p_TempRecord.V335));
                if (p_TempRecord.V336 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V336", p_TempRecord.V336));
                if (p_TempRecord.V337 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V337", p_TempRecord.V337));
                if (p_TempRecord.V338 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V338", p_TempRecord.V338));
                if (p_TempRecord.V339 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V339", p_TempRecord.V339));
                if (p_TempRecord.V340 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V340", p_TempRecord.V340));
                if (p_TempRecord.V341 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V341", p_TempRecord.V341));
                if (p_TempRecord.V342 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V342", p_TempRecord.V342));
                if (p_TempRecord.V343 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V343", p_TempRecord.V343));
                if (p_TempRecord.V344 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V344", p_TempRecord.V344));
                if (p_TempRecord.V345 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V345", p_TempRecord.V345));
                if (p_TempRecord.V346 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V346", p_TempRecord.V346));
                if (p_TempRecord.V347 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V347", p_TempRecord.V347));
                if (p_TempRecord.V348 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V348", p_TempRecord.V348));
                if (p_TempRecord.V349 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V349", p_TempRecord.V349));
                if (p_TempRecord.V350 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V350", p_TempRecord.V350));



                if (p_TempRecord.V351 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V351", p_TempRecord.V351));
                if (p_TempRecord.V352 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V352", p_TempRecord.V352));
                if (p_TempRecord.V353 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V353", p_TempRecord.V353));
                if (p_TempRecord.V354 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V354", p_TempRecord.V354));
                if (p_TempRecord.V355 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V355", p_TempRecord.V355));
                if (p_TempRecord.V356 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V356", p_TempRecord.V356));
                if (p_TempRecord.V357 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V357", p_TempRecord.V357));
                if (p_TempRecord.V358 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V358", p_TempRecord.V358));
                if (p_TempRecord.V359 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V359", p_TempRecord.V359));
                if (p_TempRecord.V360 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V360", p_TempRecord.V360));
                if (p_TempRecord.V361 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V361", p_TempRecord.V361));
                if (p_TempRecord.V362 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V362", p_TempRecord.V362));
                if (p_TempRecord.V363 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V363", p_TempRecord.V363));
                if (p_TempRecord.V364 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V364", p_TempRecord.V364));
                if (p_TempRecord.V365 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V365", p_TempRecord.V365));
                if (p_TempRecord.V366 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V366", p_TempRecord.V366));
                if (p_TempRecord.V367 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V367", p_TempRecord.V367));
                if (p_TempRecord.V368 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V368", p_TempRecord.V368));
                if (p_TempRecord.V369 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V369", p_TempRecord.V369));
                if (p_TempRecord.V370 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V370", p_TempRecord.V370));
                if (p_TempRecord.V371 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V371", p_TempRecord.V371));
                if (p_TempRecord.V372 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V372", p_TempRecord.V372));
                if (p_TempRecord.V373 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V373", p_TempRecord.V373));
                if (p_TempRecord.V374 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V374", p_TempRecord.V374));
                if (p_TempRecord.V375 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V375", p_TempRecord.V375));
                if (p_TempRecord.V376 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V376", p_TempRecord.V376));
                if (p_TempRecord.V377 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V377", p_TempRecord.V377));
                if (p_TempRecord.V378 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V378", p_TempRecord.V378));
                if (p_TempRecord.V379 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V379", p_TempRecord.V379));
                if (p_TempRecord.V380 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V380", p_TempRecord.V380));
                if (p_TempRecord.V381 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V381", p_TempRecord.V381));
                if (p_TempRecord.V382 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V382", p_TempRecord.V382));
                if (p_TempRecord.V383 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V383", p_TempRecord.V383));
                if (p_TempRecord.V384 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V384", p_TempRecord.V384));
                if (p_TempRecord.V385 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V385", p_TempRecord.V385));
                if (p_TempRecord.V386 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V386", p_TempRecord.V386));
                if (p_TempRecord.V387 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V387", p_TempRecord.V387));
                if (p_TempRecord.V388 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V388", p_TempRecord.V388));
                if (p_TempRecord.V389 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V389", p_TempRecord.V389));
                if (p_TempRecord.V390 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V390", p_TempRecord.V390));
                if (p_TempRecord.V391 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V391", p_TempRecord.V391));
                if (p_TempRecord.V392 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V392", p_TempRecord.V392));
                if (p_TempRecord.V393 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V393", p_TempRecord.V393));
                if (p_TempRecord.V394 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V394", p_TempRecord.V394));
                if (p_TempRecord.V395 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V395", p_TempRecord.V395));
                if (p_TempRecord.V396 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V396", p_TempRecord.V396));
                if (p_TempRecord.V397 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V397", p_TempRecord.V397));
                if (p_TempRecord.V398 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V398", p_TempRecord.V398));
                if (p_TempRecord.V399 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V399", p_TempRecord.V399));
                if (p_TempRecord.V400 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V400", p_TempRecord.V400));


                if (p_TempRecord.V401 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V401", p_TempRecord.V401));
                if (p_TempRecord.V402 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V402", p_TempRecord.V402));
                if (p_TempRecord.V403 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V403", p_TempRecord.V403));
                if (p_TempRecord.V404 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V404", p_TempRecord.V404));
                if (p_TempRecord.V405 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V405", p_TempRecord.V405));
                if (p_TempRecord.V406 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V406", p_TempRecord.V406));
                if (p_TempRecord.V407 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V407", p_TempRecord.V407));
                if (p_TempRecord.V408 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V408", p_TempRecord.V408));
                if (p_TempRecord.V409 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V409", p_TempRecord.V409));
                if (p_TempRecord.V410 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V410", p_TempRecord.V410));
                if (p_TempRecord.V411 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V411", p_TempRecord.V411));
                if (p_TempRecord.V412 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V412", p_TempRecord.V412));
                if (p_TempRecord.V413 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V413", p_TempRecord.V413));
                if (p_TempRecord.V414 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V414", p_TempRecord.V414));
                if (p_TempRecord.V415 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V415", p_TempRecord.V415));
                if (p_TempRecord.V416 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V416", p_TempRecord.V416));
                if (p_TempRecord.V417 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V417", p_TempRecord.V417));
                if (p_TempRecord.V418 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V418", p_TempRecord.V418));
                if (p_TempRecord.V419 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V419", p_TempRecord.V419));
                if (p_TempRecord.V420 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V420", p_TempRecord.V420));
                if (p_TempRecord.V421 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V421", p_TempRecord.V421));
                if (p_TempRecord.V422 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V422", p_TempRecord.V422));
                if (p_TempRecord.V423 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V423", p_TempRecord.V423));
                if (p_TempRecord.V424 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V424", p_TempRecord.V424));
                if (p_TempRecord.V425 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V425", p_TempRecord.V425));
                if (p_TempRecord.V426 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V426", p_TempRecord.V426));
                if (p_TempRecord.V427 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V427", p_TempRecord.V427));
                if (p_TempRecord.V428 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V428", p_TempRecord.V428));
                if (p_TempRecord.V429 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V429", p_TempRecord.V429));
                if (p_TempRecord.V430 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V430", p_TempRecord.V430));
                if (p_TempRecord.V431 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V431", p_TempRecord.V431));
                if (p_TempRecord.V432 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V432", p_TempRecord.V432));
                if (p_TempRecord.V433 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V433", p_TempRecord.V433));
                if (p_TempRecord.V434 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V434", p_TempRecord.V434));
                if (p_TempRecord.V435 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V435", p_TempRecord.V435));
                if (p_TempRecord.V436 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V436", p_TempRecord.V436));
                if (p_TempRecord.V437 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V437", p_TempRecord.V437));
                if (p_TempRecord.V438 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V438", p_TempRecord.V438));
                if (p_TempRecord.V439 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V439", p_TempRecord.V439));
                if (p_TempRecord.V440 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V440", p_TempRecord.V440));
                if (p_TempRecord.V441 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V441", p_TempRecord.V441));
                if (p_TempRecord.V442 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V442", p_TempRecord.V442));
                if (p_TempRecord.V443 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V443", p_TempRecord.V443));
                if (p_TempRecord.V444 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V444", p_TempRecord.V444));
                if (p_TempRecord.V445 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V445", p_TempRecord.V445));
                if (p_TempRecord.V446 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V446", p_TempRecord.V446));
                if (p_TempRecord.V447 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V447", p_TempRecord.V447));
                if (p_TempRecord.V448 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V448", p_TempRecord.V448));
                if (p_TempRecord.V449 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V449", p_TempRecord.V449));
                if (p_TempRecord.V450 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V450", p_TempRecord.V450));



                if (p_TempRecord.V451 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V451", p_TempRecord.V451));
                if (p_TempRecord.V452 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V452", p_TempRecord.V452));
                if (p_TempRecord.V453 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V453", p_TempRecord.V453));
                if (p_TempRecord.V454 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V454", p_TempRecord.V454));
                if (p_TempRecord.V455 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V455", p_TempRecord.V455));
                if (p_TempRecord.V456 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V456", p_TempRecord.V456));
                if (p_TempRecord.V457 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V457", p_TempRecord.V457));
                if (p_TempRecord.V458 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V458", p_TempRecord.V458));
                if (p_TempRecord.V459 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V459", p_TempRecord.V459));
                if (p_TempRecord.V460 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V460", p_TempRecord.V460));
                if (p_TempRecord.V461 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V461", p_TempRecord.V461));
                if (p_TempRecord.V462 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V462", p_TempRecord.V462));
                if (p_TempRecord.V463 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V463", p_TempRecord.V463));
                if (p_TempRecord.V464 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V464", p_TempRecord.V464));
                if (p_TempRecord.V465 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V465", p_TempRecord.V465));
                if (p_TempRecord.V466 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V466", p_TempRecord.V466));
                if (p_TempRecord.V467 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V467", p_TempRecord.V467));
                if (p_TempRecord.V468 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V468", p_TempRecord.V468));
                if (p_TempRecord.V469 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V469", p_TempRecord.V469));
                if (p_TempRecord.V470 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V470", p_TempRecord.V470));
                if (p_TempRecord.V471 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V471", p_TempRecord.V471));
                if (p_TempRecord.V472 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V472", p_TempRecord.V472));
                if (p_TempRecord.V473 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V473", p_TempRecord.V473));
                if (p_TempRecord.V474 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V474", p_TempRecord.V474));
                if (p_TempRecord.V475 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V475", p_TempRecord.V475));
                if (p_TempRecord.V476 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V476", p_TempRecord.V476));
                if (p_TempRecord.V477 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V477", p_TempRecord.V477));
                if (p_TempRecord.V478 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V478", p_TempRecord.V478));
                if (p_TempRecord.V479 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V479", p_TempRecord.V479));
                if (p_TempRecord.V480 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V480", p_TempRecord.V480));
                if (p_TempRecord.V481 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V481", p_TempRecord.V481));
                if (p_TempRecord.V482 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V482", p_TempRecord.V482));
                if (p_TempRecord.V483 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V483", p_TempRecord.V483));
                if (p_TempRecord.V484 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V484", p_TempRecord.V484));
                if (p_TempRecord.V485 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V485", p_TempRecord.V485));
                if (p_TempRecord.V486 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V486", p_TempRecord.V486));
                if (p_TempRecord.V487 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V487", p_TempRecord.V487));
                if (p_TempRecord.V488 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V488", p_TempRecord.V488));
                if (p_TempRecord.V489 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V489", p_TempRecord.V489));
                if (p_TempRecord.V490 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V490", p_TempRecord.V490));
                if (p_TempRecord.V491 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V491", p_TempRecord.V491));
                if (p_TempRecord.V492 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V492", p_TempRecord.V492));
                if (p_TempRecord.V493 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V493", p_TempRecord.V493));
                if (p_TempRecord.V494 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V494", p_TempRecord.V494));
                if (p_TempRecord.V495 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V495", p_TempRecord.V495));
                if (p_TempRecord.V496 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V496", p_TempRecord.V496));
                if (p_TempRecord.V497 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V497", p_TempRecord.V497));
                if (p_TempRecord.V498 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V498", p_TempRecord.V498));
                if (p_TempRecord.V499 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V499", p_TempRecord.V499));
                if (p_TempRecord.V500 != string.Empty)
                    command.Parameters.Add(new SqlParameter("@V500", p_TempRecord.V500));





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

    public static int ets_TempRecord_Delete(int nTempRecordID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TempRecord_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nTempRecordID", nTempRecordID));

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

    public static string GetTempRecordValue(ref TempRecord objRecord, string strSystemName)
    {


        switch (strSystemName.ToUpper())
        {
            //case "LOCATIONID":
            //    return objRecord.LocationID.ToString();

            case "RECORDID":
                return objRecord.TempRecordID.ToString();

            case "TABLEID":
                return objRecord.TableID.ToString();


            case "DATETIMERECORDED":
                return objRecord.DateTimeRecorded.ToString();


            case "NOTES":
                return objRecord.Notes.ToString();

            //case "ENTEREDBY":
            //    return objRecord.EnteredBy.ToString();


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


    public static void MakeTheTempRecord(ref TempRecord objTempRecord, string strSystemName, object objValue)
    {


        switch (strSystemName.ToUpper())
        {
            //case "LOCATIONID":
            //    objTempRecord.LocationID = int.Parse(objValue.ToString());
            //    break;
            //case "LOCATIONNAME":
            //    objTempRecord.LocationName = objValue.ToString();
            //    break;

            case "TABLEID":
                //do nothing here
                //objTempRecord.TableID = int.Parse(objValue.ToString());
                break;

            case "DATETIMERECORDED":
                if (objValue.ToString().Trim() == "")
                {
                    objTempRecord.DateTimeRecorded = DateTime.Now;
                }
                else
                {

                    switch (objTempRecord.DateFormat)
                    {
                        case "MM/DD/YYYY":

                            System.Globalization.CultureInfo culture2 = new System.Globalization.CultureInfo("en-US");
                            if (objValue.ToString().IndexOf(" ") > 0)
                            {
                                if (objValue.ToString().Substring(0, objValue.ToString().IndexOf(" ")).Length < 7)
                                {
                                    string strTempDateTime = objValue.ToString().Substring(0, objValue.ToString().IndexOf(" ")) + "-" + DateTime.Now.Year.ToString() + " " + objValue.ToString().Substring(objValue.ToString().IndexOf(" ") + 1);
                                    objTempRecord.DateTimeRecorded = Convert.ToDateTime(strTempDateTime, culture2);
                                }
                                else
                                {
                                    if (objValue.ToString().Length == 16)
                                    {
                                        //objTempRecord.DateTimeRecorded = DateTime.ParseExact(objValue.ToString(), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                                        objTempRecord.DateTimeRecorded = DateTime.ParseExact(objValue.ToString(), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        objTempRecord.DateTimeRecorded = Convert.ToDateTime(objValue.ToString(), culture2);
                                    }
                                }
                            }
                            else
                            {
                                objTempRecord.DateTimeRecorded = Convert.ToDateTime(objValue.ToString(), culture2);
                            }

                            break;
                        case "YYYY-MM-DD":


                            if (objValue.ToString().IndexOf(" ") > 0)
                            {
                                if (objValue.ToString().Substring(0, objValue.ToString().IndexOf(" ")).Length < 7)
                                {
                                    string strTempDateTime = objValue.ToString().Substring(0, objValue.ToString().IndexOf(" ")) + "-" + DateTime.Now.Year.ToString() + " " + objValue.ToString().Substring(objValue.ToString().IndexOf(" ") + 1);
                                    objTempRecord.DateTimeRecorded = Convert.ToDateTime(strTempDateTime, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    if (objValue.ToString().Length == 16)
                                    {
                                        //objTempRecord.DateTimeRecorded = DateTime.ParseExact(objValue.ToString(), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                                        objTempRecord.DateTimeRecorded = DateTime.ParseExact(objValue.ToString(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        objTempRecord.DateTimeRecorded = Convert.ToDateTime(objValue.ToString(), CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            else
                            {
                                objTempRecord.DateTimeRecorded = Convert.ToDateTime(objValue.ToString(), CultureInfo.InvariantCulture);
                            }

                            break;

                        default:

                            objTempRecord.DateTimeRecorded = Common.GetDateTimeFromString(objValue.ToString(), "");
                            if (objTempRecord.DateTimeRecorded == null)
                                objTempRecord.DateTimeRecorded = DateTime.Now;

                            break;
                    }




                }
                break;

            case "NOTES":
                objTempRecord.Notes = objValue.ToString();
                break;
            //case "ENTEREDBY":
            //    objTempRecord.EnteredBy = int.Parse(objValue.ToString());
            //    break;

            case "ISACTIVE":
                objTempRecord.IsActive = (bool)objValue;
                break;

            case "V001":
                objTempRecord.V001 = objValue.ToString();
                break;

            case "V002":
                objTempRecord.V002 = objValue.ToString();
                break;

            case "V003":
                objTempRecord.V003 = objValue.ToString();
                break;

            case "V004":
                objTempRecord.V004 = objValue.ToString();
                break;

            case "V005":
                objTempRecord.V005 = objValue.ToString();
                break;

            case "V006":
                objTempRecord.V006 = objValue.ToString();
                break;

            case "V007":
                objTempRecord.V007 = objValue.ToString();
                break;

            case "V008":
                objTempRecord.V008 = objValue.ToString();
                break;

            case "V009":
                objTempRecord.V009 = objValue.ToString();
                break;

            case "V010":
                objTempRecord.V010 = objValue.ToString();
                break;

            case "V011":
                objTempRecord.V011 = objValue.ToString();
                break;

            case "V012":
                objTempRecord.V012 = objValue.ToString();
                break;

            case "V013":
                objTempRecord.V013 = objValue.ToString();
                break;

            case "V014":
                objTempRecord.V014 = objValue.ToString();
                break;

            case "V015":
                objTempRecord.V015 = objValue.ToString();
                break;

            case "V016":
                objTempRecord.V016 = objValue.ToString();
                break;

            case "V017":
                objTempRecord.V017 = objValue.ToString();
                break;

            case "V018":
                objTempRecord.V018 = objValue.ToString();
                break;

            case "V019":
                objTempRecord.V019 = objValue.ToString();
                break;

            case "V020":
                objTempRecord.V020 = objValue.ToString();
                break;
            case "V021":
                objTempRecord.V021 = objValue.ToString();
                break;

            case "V022":
                objTempRecord.V022 = objValue.ToString();
                break;

            case "V023":
                objTempRecord.V023 = objValue.ToString();
                break;

            case "V024":
                objTempRecord.V024 = objValue.ToString();
                break;

            case "V025":
                objTempRecord.V025 = objValue.ToString();
                break;

            case "V026":
                objTempRecord.V026 = objValue.ToString();
                break;

            case "V027":
                objTempRecord.V027 = objValue.ToString();
                break;

            case "V028":
                objTempRecord.V028 = objValue.ToString();
                break;

            case "V029":
                objTempRecord.V029 = objValue.ToString();
                break;

            case "V030":
                objTempRecord.V030 = objValue.ToString();
                break;
            case "V031":
                objTempRecord.V031 = objValue.ToString();
                break;

            case "V032":
                objTempRecord.V032 = objValue.ToString();
                break;

            case "V033":
                objTempRecord.V033 = objValue.ToString();
                break;

            case "V034":
                objTempRecord.V034 = objValue.ToString();
                break;

            case "V035":
                objTempRecord.V035 = objValue.ToString();
                break;

            case "V036":
                objTempRecord.V036 = objValue.ToString();
                break;

            case "V037":
                objTempRecord.V037 = objValue.ToString();
                break;

            case "V038":
                objTempRecord.V038 = objValue.ToString();
                break;

            case "V039":
                objTempRecord.V039 = objValue.ToString();
                break;

            case "V040":
                objTempRecord.V040 = objValue.ToString();
                break;
            case "V041":
                objTempRecord.V041 = objValue.ToString();
                break;

            case "V042":
                objTempRecord.V042 = objValue.ToString();
                break;

            case "V043":
                objTempRecord.V043 = objValue.ToString();
                break;

            case "V044":
                objTempRecord.V044 = objValue.ToString();
                break;

            case "V045":
                objTempRecord.V045 = objValue.ToString();
                break;

            case "V046":
                objTempRecord.V046 = objValue.ToString();
                break;

            case "V047":
                objTempRecord.V047 = objValue.ToString();
                break;

            case "V048":
                objTempRecord.V048 = objValue.ToString();
                break;

            case "V049":
                objTempRecord.V049 = objValue.ToString();
                break;

            case "V050":
                objTempRecord.V050 = objValue.ToString();
                break;
            case "V051":
                objTempRecord.V051 = objValue.ToString();
                break;

            case "V052":
                objTempRecord.V052 = objValue.ToString();
                break;

            case "V053":
                objTempRecord.V053 = objValue.ToString();
                break;

            case "V054":
                objTempRecord.V054 = objValue.ToString();
                break;

            case "V055":
                objTempRecord.V055 = objValue.ToString();
                break;

            case "V056":
                objTempRecord.V056 = objValue.ToString();
                break;

            case "V057":
                objTempRecord.V057 = objValue.ToString();
                break;

            case "V058":
                objTempRecord.V058 = objValue.ToString();
                break;

            case "V059":
                objTempRecord.V059 = objValue.ToString();
                break;

            case "V060":
                objTempRecord.V060 = objValue.ToString();
                break;
            case "V061":
                objTempRecord.V061 = objValue.ToString();
                break;

            case "V062":
                objTempRecord.V062 = objValue.ToString();
                break;

            case "V063":
                objTempRecord.V063 = objValue.ToString();
                break;

            case "V064":
                objTempRecord.V064 = objValue.ToString();
                break;

            case "V065":
                objTempRecord.V065 = objValue.ToString();
                break;

            case "V066":
                objTempRecord.V066 = objValue.ToString();
                break;

            case "V067":
                objTempRecord.V067 = objValue.ToString();
                break;

            case "V068":
                objTempRecord.V068 = objValue.ToString();
                break;

            case "V069":
                objTempRecord.V069 = objValue.ToString();
                break;

            case "V070":
                objTempRecord.V070 = objValue.ToString();
                break;
            case "V071":
                objTempRecord.V071 = objValue.ToString();
                break;

            case "V072":
                objTempRecord.V072 = objValue.ToString();
                break;

            case "V073":
                objTempRecord.V073 = objValue.ToString();
                break;

            case "V074":
                objTempRecord.V074 = objValue.ToString();
                break;

            case "V075":
                objTempRecord.V075 = objValue.ToString();
                break;

            case "V076":
                objTempRecord.V076 = objValue.ToString();
                break;

            case "V077":
                objTempRecord.V077 = objValue.ToString();
                break;

            case "V078":
                objTempRecord.V078 = objValue.ToString();
                break;

            case "V079":
                objTempRecord.V079 = objValue.ToString();
                break;

            case "V080":
                objTempRecord.V080 = objValue.ToString();
                break;
            case "V081":
                objTempRecord.V081 = objValue.ToString();
                break;

            case "V082":
                objTempRecord.V082 = objValue.ToString();
                break;

            case "V083":
                objTempRecord.V083 = objValue.ToString();
                break;

            case "V084":
                objTempRecord.V084 = objValue.ToString();
                break;

            case "V085":
                objTempRecord.V085 = objValue.ToString();
                break;

            case "V086":
                objTempRecord.V086 = objValue.ToString();
                break;

            case "V087":
                objTempRecord.V087 = objValue.ToString();
                break;

            case "V088":
                objTempRecord.V088 = objValue.ToString();
                break;

            case "V089":
                objTempRecord.V089 = objValue.ToString();
                break;

            case "V090":
                objTempRecord.V090 = objValue.ToString();
                break;
            case "V091":
                objTempRecord.V091 = objValue.ToString();
                break;

            case "V092":
                objTempRecord.V092 = objValue.ToString();
                break;

            case "V093":
                objTempRecord.V093 = objValue.ToString();
                break;

            case "V094":
                objTempRecord.V094 = objValue.ToString();
                break;

            case "V095":
                objTempRecord.V095 = objValue.ToString();
                break;

            case "V096":
                objTempRecord.V096 = objValue.ToString();
                break;

            case "V097":
                objTempRecord.V097 = objValue.ToString();
                break;

            case "V098":
                objTempRecord.V098 = objValue.ToString();
                break;

            case "V099":
                objTempRecord.V099 = objValue.ToString();
                break;

            case "V100":
                objTempRecord.V100 = objValue.ToString();
                break;

            case "V101":
                objTempRecord.V101 = objValue.ToString();
                break;

            case "V102":
                objTempRecord.V102 = objValue.ToString();
                break;

            case "V103":
                objTempRecord.V103 = objValue.ToString();
                break;

            case "V104":
                objTempRecord.V104 = objValue.ToString();
                break;

            case "V105":
                objTempRecord.V105 = objValue.ToString();
                break;

            case "V106":
                objTempRecord.V106 = objValue.ToString();
                break;

            case "V107":
                objTempRecord.V107 = objValue.ToString();
                break;

            case "V108":
                objTempRecord.V108 = objValue.ToString();
                break;

            case "V109":
                objTempRecord.V109 = objValue.ToString();
                break;

            case "V110":
                objTempRecord.V110 = objValue.ToString();
                break;
            case "V111":
                objTempRecord.V111 = objValue.ToString();
                break;

            case "V112":
                objTempRecord.V112 = objValue.ToString();
                break;

            case "V113":
                objTempRecord.V113 = objValue.ToString();
                break;

            case "V114":
                objTempRecord.V114 = objValue.ToString();
                break;

            case "V115":
                objTempRecord.V115 = objValue.ToString();
                break;

            case "V116":
                objTempRecord.V116 = objValue.ToString();
                break;

            case "V117":
                objTempRecord.V117 = objValue.ToString();
                break;

            case "V118":
                objTempRecord.V118 = objValue.ToString();
                break;

            case "V119":
                objTempRecord.V119 = objValue.ToString();
                break;

            case "V120":
                objTempRecord.V120 = objValue.ToString();
                break;
            case "V121":
                objTempRecord.V121 = objValue.ToString();
                break;

            case "V122":
                objTempRecord.V122 = objValue.ToString();
                break;

            case "V123":
                objTempRecord.V123 = objValue.ToString();
                break;

            case "V124":
                objTempRecord.V124 = objValue.ToString();
                break;

            case "V125":
                objTempRecord.V125 = objValue.ToString();
                break;

            case "V126":
                objTempRecord.V126 = objValue.ToString();
                break;

            case "V127":
                objTempRecord.V127 = objValue.ToString();
                break;

            case "V128":
                objTempRecord.V128 = objValue.ToString();
                break;

            case "V129":
                objTempRecord.V129 = objValue.ToString();
                break;

            case "V130":
                objTempRecord.V130 = objValue.ToString();
                break;
            case "V131":
                objTempRecord.V131 = objValue.ToString();
                break;

            case "V132":
                objTempRecord.V132 = objValue.ToString();
                break;

            case "V133":
                objTempRecord.V133 = objValue.ToString();
                break;

            case "V134":
                objTempRecord.V134 = objValue.ToString();
                break;

            case "V135":
                objTempRecord.V135 = objValue.ToString();
                break;

            case "V136":
                objTempRecord.V136 = objValue.ToString();
                break;

            case "V137":
                objTempRecord.V137 = objValue.ToString();
                break;

            case "V138":
                objTempRecord.V138 = objValue.ToString();
                break;

            case "V139":
                objTempRecord.V139 = objValue.ToString();
                break;

            case "V140":
                objTempRecord.V140 = objValue.ToString();
                break;
            case "V141":
                objTempRecord.V141 = objValue.ToString();
                break;

            case "V142":
                objTempRecord.V142 = objValue.ToString();
                break;

            case "V143":
                objTempRecord.V143 = objValue.ToString();
                break;

            case "V144":
                objTempRecord.V144 = objValue.ToString();
                break;

            case "V145":
                objTempRecord.V145 = objValue.ToString();
                break;

            case "V146":
                objTempRecord.V146 = objValue.ToString();
                break;

            case "V147":
                objTempRecord.V147 = objValue.ToString();
                break;

            case "V148":
                objTempRecord.V148 = objValue.ToString();
                break;

            case "V149":
                objTempRecord.V149 = objValue.ToString();
                break;

            case "V150":
                objTempRecord.V150 = objValue.ToString();
                break;

            case "V151":
                objTempRecord.V151 = objValue.ToString();
                break;

            case "V152":
                objTempRecord.V152 = objValue.ToString();
                break;

            case "V153":
                objTempRecord.V153 = objValue.ToString();
                break;

            case "V154":
                objTempRecord.V154 = objValue.ToString();
                break;

            case "V155":
                objTempRecord.V155 = objValue.ToString();
                break;

            case "V156":
                objTempRecord.V156 = objValue.ToString();
                break;

            case "V157":
                objTempRecord.V157 = objValue.ToString();
                break;

            case "V158":
                objTempRecord.V158 = objValue.ToString();
                break;

            case "V159":
                objTempRecord.V159 = objValue.ToString();
                break;

            case "V160":
                objTempRecord.V160 = objValue.ToString();
                break;
            case "V161":
                objTempRecord.V161 = objValue.ToString();
                break;

            case "V162":
                objTempRecord.V162 = objValue.ToString();
                break;

            case "V163":
                objTempRecord.V163 = objValue.ToString();
                break;

            case "V164":
                objTempRecord.V164 = objValue.ToString();
                break;

            case "V165":
                objTempRecord.V165 = objValue.ToString();
                break;

            case "V166":
                objTempRecord.V166 = objValue.ToString();
                break;

            case "V167":
                objTempRecord.V167 = objValue.ToString();
                break;

            case "V168":
                objTempRecord.V168 = objValue.ToString();
                break;

            case "V169":
                objTempRecord.V169 = objValue.ToString();
                break;

            case "V170":
                objTempRecord.V170 = objValue.ToString();
                break;
            case "V171":
                objTempRecord.V171 = objValue.ToString();
                break;

            case "V172":
                objTempRecord.V172 = objValue.ToString();
                break;

            case "V173":
                objTempRecord.V173 = objValue.ToString();
                break;

            case "V174":
                objTempRecord.V174 = objValue.ToString();
                break;

            case "V175":
                objTempRecord.V175 = objValue.ToString();
                break;

            case "V176":
                objTempRecord.V176 = objValue.ToString();
                break;

            case "V177":
                objTempRecord.V177 = objValue.ToString();
                break;

            case "V178":
                objTempRecord.V178 = objValue.ToString();
                break;

            case "V179":
                objTempRecord.V179 = objValue.ToString();
                break;

            case "V180":
                objTempRecord.V180 = objValue.ToString();
                break;
            case "V181":
                objTempRecord.V181 = objValue.ToString();
                break;

            case "V182":
                objTempRecord.V182 = objValue.ToString();
                break;

            case "V183":
                objTempRecord.V183 = objValue.ToString();
                break;

            case "V184":
                objTempRecord.V184 = objValue.ToString();
                break;

            case "V185":
                objTempRecord.V185 = objValue.ToString();
                break;

            case "V186":
                objTempRecord.V186 = objValue.ToString();
                break;

            case "V187":
                objTempRecord.V187 = objValue.ToString();
                break;

            case "V188":
                objTempRecord.V188 = objValue.ToString();
                break;

            case "V189":
                objTempRecord.V189 = objValue.ToString();
                break;

            case "V190":
                objTempRecord.V190 = objValue.ToString();
                break;
            case "V191":
                objTempRecord.V191 = objValue.ToString();
                break;

            case "V192":
                objTempRecord.V192 = objValue.ToString();
                break;

            case "V193":
                objTempRecord.V193 = objValue.ToString();
                break;

            case "V194":
                objTempRecord.V194 = objValue.ToString();
                break;

            case "V195":
                objTempRecord.V195 = objValue.ToString();
                break;

            case "V196":
                objTempRecord.V196 = objValue.ToString();
                break;

            case "V197":
                objTempRecord.V197 = objValue.ToString();
                break;

            case "V198":
                objTempRecord.V198 = objValue.ToString();
                break;

            case "V199":
                objTempRecord.V199 = objValue.ToString();
                break;

            case "V200":
                objTempRecord.V200 = objValue.ToString();
                break;

            case "V201":
                objTempRecord.V201 = objValue.ToString();
                break;

            case "V202":
                objTempRecord.V202 = objValue.ToString();
                break;

            case "V203":
                objTempRecord.V203 = objValue.ToString();
                break;

            case "V204":
                objTempRecord.V204 = objValue.ToString();
                break;

            case "V205":
                objTempRecord.V205 = objValue.ToString();
                break;

            case "V206":
                objTempRecord.V206 = objValue.ToString();
                break;

            case "V207":
                objTempRecord.V207 = objValue.ToString();
                break;

            case "V208":
                objTempRecord.V208 = objValue.ToString();
                break;

            case "V209":
                objTempRecord.V209 = objValue.ToString();
                break;

            case "V210":
                objTempRecord.V210 = objValue.ToString();
                break;
            case "V211":
                objTempRecord.V211 = objValue.ToString();
                break;

            case "V212":
                objTempRecord.V212 = objValue.ToString();
                break;

            case "V213":
                objTempRecord.V213 = objValue.ToString();
                break;

            case "V214":
                objTempRecord.V214 = objValue.ToString();
                break;

            case "V215":
                objTempRecord.V215 = objValue.ToString();
                break;

            case "V216":
                objTempRecord.V216 = objValue.ToString();
                break;

            case "V217":
                objTempRecord.V217 = objValue.ToString();
                break;

            case "V218":
                objTempRecord.V218 = objValue.ToString();
                break;

            case "V219":
                objTempRecord.V219 = objValue.ToString();
                break;

            case "V220":
                objTempRecord.V220 = objValue.ToString();
                break;
            case "V221":
                objTempRecord.V221 = objValue.ToString();
                break;

            case "V222":
                objTempRecord.V222 = objValue.ToString();
                break;

            case "V223":
                objTempRecord.V223 = objValue.ToString();
                break;

            case "V224":
                objTempRecord.V224 = objValue.ToString();
                break;

            case "V225":
                objTempRecord.V225 = objValue.ToString();
                break;

            case "V226":
                objTempRecord.V226 = objValue.ToString();
                break;

            case "V227":
                objTempRecord.V227 = objValue.ToString();
                break;

            case "V228":
                objTempRecord.V228 = objValue.ToString();
                break;

            case "V229":
                objTempRecord.V229 = objValue.ToString();
                break;

            case "V230":
                objTempRecord.V230 = objValue.ToString();
                break;
            case "V231":
                objTempRecord.V231 = objValue.ToString();
                break;

            case "V232":
                objTempRecord.V232 = objValue.ToString();
                break;

            case "V233":
                objTempRecord.V233 = objValue.ToString();
                break;

            case "V234":
                objTempRecord.V234 = objValue.ToString();
                break;

            case "V235":
                objTempRecord.V235 = objValue.ToString();
                break;

            case "V236":
                objTempRecord.V236 = objValue.ToString();
                break;

            case "V237":
                objTempRecord.V237 = objValue.ToString();
                break;

            case "V238":
                objTempRecord.V238 = objValue.ToString();
                break;

            case "V239":
                objTempRecord.V239 = objValue.ToString();
                break;

            case "V240":
                objTempRecord.V240 = objValue.ToString();
                break;
            case "V241":
                objTempRecord.V241 = objValue.ToString();
                break;

            case "V242":
                objTempRecord.V242 = objValue.ToString();
                break;

            case "V243":
                objTempRecord.V243 = objValue.ToString();
                break;

            case "V244":
                objTempRecord.V244 = objValue.ToString();
                break;

            case "V245":
                objTempRecord.V245 = objValue.ToString();
                break;

            case "V246":
                objTempRecord.V246 = objValue.ToString();
                break;

            case "V247":
                objTempRecord.V247 = objValue.ToString();
                break;

            case "V248":
                objTempRecord.V248 = objValue.ToString();
                break;

            case "V249":
                objTempRecord.V249 = objValue.ToString();
                break;

            case "V250":
                objTempRecord.V250 = objValue.ToString();
                break;

            case "V251":
                objTempRecord.V251 = objValue.ToString();
                break;

            case "V252":
                objTempRecord.V252 = objValue.ToString();
                break;

            case "V253":
                objTempRecord.V253 = objValue.ToString();
                break;

            case "V254":
                objTempRecord.V254 = objValue.ToString();
                break;

            case "V255":
                objTempRecord.V255 = objValue.ToString();
                break;

            case "V256":
                objTempRecord.V256 = objValue.ToString();
                break;

            case "V257":
                objTempRecord.V257 = objValue.ToString();
                break;

            case "V258":
                objTempRecord.V258 = objValue.ToString();
                break;

            case "V259":
                objTempRecord.V259 = objValue.ToString();
                break;

            case "V260":
                objTempRecord.V260 = objValue.ToString();
                break;
            case "V261":
                objTempRecord.V261 = objValue.ToString();
                break;

            case "V262":
                objTempRecord.V262 = objValue.ToString();
                break;

            case "V263":
                objTempRecord.V263 = objValue.ToString();
                break;

            case "V264":
                objTempRecord.V264 = objValue.ToString();
                break;

            case "V265":
                objTempRecord.V265 = objValue.ToString();
                break;

            case "V266":
                objTempRecord.V266 = objValue.ToString();
                break;

            case "V267":
                objTempRecord.V267 = objValue.ToString();
                break;

            case "V268":
                objTempRecord.V268 = objValue.ToString();
                break;

            case "V269":
                objTempRecord.V269 = objValue.ToString();
                break;

            case "V270":
                objTempRecord.V270 = objValue.ToString();
                break;
            case "V271":
                objTempRecord.V271 = objValue.ToString();
                break;

            case "V272":
                objTempRecord.V272 = objValue.ToString();
                break;

            case "V273":
                objTempRecord.V273 = objValue.ToString();
                break;

            case "V274":
                objTempRecord.V274 = objValue.ToString();
                break;

            case "V275":
                objTempRecord.V275 = objValue.ToString();
                break;

            case "V276":
                objTempRecord.V276 = objValue.ToString();
                break;

            case "V277":
                objTempRecord.V277 = objValue.ToString();
                break;

            case "V278":
                objTempRecord.V278 = objValue.ToString();
                break;

            case "V279":
                objTempRecord.V279 = objValue.ToString();
                break;

            case "V280":
                objTempRecord.V280 = objValue.ToString();
                break;
            case "V281":
                objTempRecord.V281 = objValue.ToString();
                break;

            case "V282":
                objTempRecord.V282 = objValue.ToString();
                break;

            case "V283":
                objTempRecord.V283 = objValue.ToString();
                break;

            case "V284":
                objTempRecord.V284 = objValue.ToString();
                break;

            case "V285":
                objTempRecord.V285 = objValue.ToString();
                break;

            case "V286":
                objTempRecord.V286 = objValue.ToString();
                break;

            case "V287":
                objTempRecord.V287 = objValue.ToString();
                break;

            case "V288":
                objTempRecord.V288 = objValue.ToString();
                break;

            case "V289":
                objTempRecord.V289 = objValue.ToString();
                break;

            case "V290":
                objTempRecord.V290 = objValue.ToString();
                break;
            case "V291":
                objTempRecord.V291 = objValue.ToString();
                break;

            case "V292":
                objTempRecord.V292 = objValue.ToString();
                break;

            case "V293":
                objTempRecord.V293 = objValue.ToString();
                break;

            case "V294":
                objTempRecord.V294 = objValue.ToString();
                break;

            case "V295":
                objTempRecord.V295 = objValue.ToString();
                break;

            case "V296":
                objTempRecord.V296 = objValue.ToString();
                break;

            case "V297":
                objTempRecord.V297 = objValue.ToString();
                break;

            case "V298":
                objTempRecord.V298 = objValue.ToString();
                break;

            case "V299":
                objTempRecord.V299 = objValue.ToString();
                break;

            case "V300":
                objTempRecord.V300 = objValue.ToString();
                break;

            case "V301":
                objTempRecord.V301 = objValue.ToString();
                break;

            case "V302":
                objTempRecord.V302 = objValue.ToString();
                break;

            case "V303":
                objTempRecord.V303 = objValue.ToString();
                break;

            case "V304":
                objTempRecord.V304 = objValue.ToString();
                break;

            case "V305":
                objTempRecord.V305 = objValue.ToString();
                break;

            case "V306":
                objTempRecord.V306 = objValue.ToString();
                break;

            case "V307":
                objTempRecord.V307 = objValue.ToString();
                break;

            case "V308":
                objTempRecord.V308 = objValue.ToString();
                break;

            case "V309":
                objTempRecord.V309 = objValue.ToString();
                break;

            case "V310":
                objTempRecord.V310 = objValue.ToString();
                break;
            case "V311":
                objTempRecord.V311 = objValue.ToString();
                break;

            case "V312":
                objTempRecord.V312 = objValue.ToString();
                break;

            case "V313":
                objTempRecord.V313 = objValue.ToString();
                break;

            case "V314":
                objTempRecord.V314 = objValue.ToString();
                break;

            case "V315":
                objTempRecord.V315 = objValue.ToString();
                break;

            case "V316":
                objTempRecord.V316 = objValue.ToString();
                break;

            case "V317":
                objTempRecord.V317 = objValue.ToString();
                break;

            case "V318":
                objTempRecord.V318 = objValue.ToString();
                break;

            case "V319":
                objTempRecord.V319 = objValue.ToString();
                break;

            case "V320":
                objTempRecord.V320 = objValue.ToString();
                break;
            case "V321":
                objTempRecord.V321 = objValue.ToString();
                break;

            case "V322":
                objTempRecord.V322 = objValue.ToString();
                break;

            case "V323":
                objTempRecord.V323 = objValue.ToString();
                break;

            case "V324":
                objTempRecord.V324 = objValue.ToString();
                break;

            case "V325":
                objTempRecord.V325 = objValue.ToString();
                break;

            case "V326":
                objTempRecord.V326 = objValue.ToString();
                break;

            case "V327":
                objTempRecord.V327 = objValue.ToString();
                break;

            case "V328":
                objTempRecord.V328 = objValue.ToString();
                break;

            case "V329":
                objTempRecord.V329 = objValue.ToString();
                break;

            case "V330":
                objTempRecord.V330 = objValue.ToString();
                break;
            case "V331":
                objTempRecord.V331 = objValue.ToString();
                break;

            case "V332":
                objTempRecord.V332 = objValue.ToString();
                break;

            case "V333":
                objTempRecord.V333 = objValue.ToString();
                break;

            case "V334":
                objTempRecord.V334 = objValue.ToString();
                break;

            case "V335":
                objTempRecord.V335 = objValue.ToString();
                break;

            case "V336":
                objTempRecord.V336 = objValue.ToString();
                break;

            case "V337":
                objTempRecord.V337 = objValue.ToString();
                break;

            case "V338":
                objTempRecord.V338 = objValue.ToString();
                break;

            case "V339":
                objTempRecord.V339 = objValue.ToString();
                break;

            case "V340":
                objTempRecord.V340 = objValue.ToString();
                break;
            case "V341":
                objTempRecord.V341 = objValue.ToString();
                break;

            case "V342":
                objTempRecord.V342 = objValue.ToString();
                break;

            case "V343":
                objTempRecord.V343 = objValue.ToString();
                break;

            case "V344":
                objTempRecord.V344 = objValue.ToString();
                break;

            case "V345":
                objTempRecord.V345 = objValue.ToString();
                break;

            case "V346":
                objTempRecord.V346 = objValue.ToString();
                break;

            case "V347":
                objTempRecord.V347 = objValue.ToString();
                break;

            case "V348":
                objTempRecord.V348 = objValue.ToString();
                break;

            case "V349":
                objTempRecord.V349 = objValue.ToString();
                break;

            case "V350":
                objTempRecord.V350 = objValue.ToString();
                break;

            case "V351":
                objTempRecord.V351 = objValue.ToString();
                break;

            case "V352":
                objTempRecord.V352 = objValue.ToString();
                break;

            case "V353":
                objTempRecord.V353 = objValue.ToString();
                break;

            case "V354":
                objTempRecord.V354 = objValue.ToString();
                break;

            case "V355":
                objTempRecord.V355 = objValue.ToString();
                break;

            case "V356":
                objTempRecord.V356 = objValue.ToString();
                break;

            case "V357":
                objTempRecord.V357 = objValue.ToString();
                break;

            case "V358":
                objTempRecord.V358 = objValue.ToString();
                break;

            case "V359":
                objTempRecord.V359 = objValue.ToString();
                break;

            case "V360":
                objTempRecord.V360 = objValue.ToString();
                break;
            case "V361":
                objTempRecord.V361 = objValue.ToString();
                break;

            case "V362":
                objTempRecord.V362 = objValue.ToString();
                break;

            case "V363":
                objTempRecord.V363 = objValue.ToString();
                break;

            case "V364":
                objTempRecord.V364 = objValue.ToString();
                break;

            case "V365":
                objTempRecord.V365 = objValue.ToString();
                break;

            case "V366":
                objTempRecord.V366 = objValue.ToString();
                break;

            case "V367":
                objTempRecord.V367 = objValue.ToString();
                break;

            case "V368":
                objTempRecord.V368 = objValue.ToString();
                break;

            case "V369":
                objTempRecord.V369 = objValue.ToString();
                break;

            case "V370":
                objTempRecord.V370 = objValue.ToString();
                break;
            case "V371":
                objTempRecord.V371 = objValue.ToString();
                break;

            case "V372":
                objTempRecord.V372 = objValue.ToString();
                break;

            case "V373":
                objTempRecord.V373 = objValue.ToString();
                break;

            case "V374":
                objTempRecord.V374 = objValue.ToString();
                break;

            case "V375":
                objTempRecord.V375 = objValue.ToString();
                break;

            case "V376":
                objTempRecord.V376 = objValue.ToString();
                break;

            case "V377":
                objTempRecord.V377 = objValue.ToString();
                break;

            case "V378":
                objTempRecord.V378 = objValue.ToString();
                break;

            case "V379":
                objTempRecord.V379 = objValue.ToString();
                break;

            case "V380":
                objTempRecord.V380 = objValue.ToString();
                break;
            case "V381":
                objTempRecord.V381 = objValue.ToString();
                break;

            case "V382":
                objTempRecord.V382 = objValue.ToString();
                break;

            case "V383":
                objTempRecord.V383 = objValue.ToString();
                break;

            case "V384":
                objTempRecord.V384 = objValue.ToString();
                break;

            case "V385":
                objTempRecord.V385 = objValue.ToString();
                break;

            case "V386":
                objTempRecord.V386 = objValue.ToString();
                break;

            case "V387":
                objTempRecord.V387 = objValue.ToString();
                break;

            case "V388":
                objTempRecord.V388 = objValue.ToString();
                break;

            case "V389":
                objTempRecord.V389 = objValue.ToString();
                break;

            case "V390":
                objTempRecord.V390 = objValue.ToString();
                break;
            case "V391":
                objTempRecord.V391 = objValue.ToString();
                break;

            case "V392":
                objTempRecord.V392 = objValue.ToString();
                break;

            case "V393":
                objTempRecord.V393 = objValue.ToString();
                break;

            case "V394":
                objTempRecord.V394 = objValue.ToString();
                break;

            case "V395":
                objTempRecord.V395 = objValue.ToString();
                break;

            case "V396":
                objTempRecord.V396 = objValue.ToString();
                break;

            case "V397":
                objTempRecord.V397 = objValue.ToString();
                break;

            case "V398":
                objTempRecord.V398 = objValue.ToString();
                break;

            case "V399":
                objTempRecord.V399 = objValue.ToString();
                break;

            case "V400":
                objTempRecord.V400 = objValue.ToString();
                break;

            case "V401":
                objTempRecord.V401 = objValue.ToString();
                break;

            case "V402":
                objTempRecord.V402 = objValue.ToString();
                break;

            case "V403":
                objTempRecord.V403 = objValue.ToString();
                break;

            case "V404":
                objTempRecord.V404 = objValue.ToString();
                break;

            case "V405":
                objTempRecord.V405 = objValue.ToString();
                break;

            case "V406":
                objTempRecord.V406 = objValue.ToString();
                break;

            case "V407":
                objTempRecord.V407 = objValue.ToString();
                break;

            case "V408":
                objTempRecord.V408 = objValue.ToString();
                break;

            case "V409":
                objTempRecord.V409 = objValue.ToString();
                break;

            case "V410":
                objTempRecord.V410 = objValue.ToString();
                break;
            case "V411":
                objTempRecord.V411 = objValue.ToString();
                break;

            case "V412":
                objTempRecord.V412 = objValue.ToString();
                break;

            case "V413":
                objTempRecord.V413 = objValue.ToString();
                break;

            case "V414":
                objTempRecord.V414 = objValue.ToString();
                break;

            case "V415":
                objTempRecord.V415 = objValue.ToString();
                break;

            case "V416":
                objTempRecord.V416 = objValue.ToString();
                break;

            case "V417":
                objTempRecord.V417 = objValue.ToString();
                break;

            case "V418":
                objTempRecord.V418 = objValue.ToString();
                break;

            case "V419":
                objTempRecord.V419 = objValue.ToString();
                break;

            case "V420":
                objTempRecord.V420 = objValue.ToString();
                break;
            case "V421":
                objTempRecord.V421 = objValue.ToString();
                break;

            case "V422":
                objTempRecord.V422 = objValue.ToString();
                break;

            case "V423":
                objTempRecord.V423 = objValue.ToString();
                break;

            case "V424":
                objTempRecord.V424 = objValue.ToString();
                break;

            case "V425":
                objTempRecord.V425 = objValue.ToString();
                break;

            case "V426":
                objTempRecord.V426 = objValue.ToString();
                break;

            case "V427":
                objTempRecord.V427 = objValue.ToString();
                break;

            case "V428":
                objTempRecord.V428 = objValue.ToString();
                break;

            case "V429":
                objTempRecord.V429 = objValue.ToString();
                break;

            case "V430":
                objTempRecord.V430 = objValue.ToString();
                break;
            case "V431":
                objTempRecord.V431 = objValue.ToString();
                break;

            case "V432":
                objTempRecord.V432 = objValue.ToString();
                break;

            case "V433":
                objTempRecord.V433 = objValue.ToString();
                break;

            case "V434":
                objTempRecord.V434 = objValue.ToString();
                break;

            case "V435":
                objTempRecord.V435 = objValue.ToString();
                break;

            case "V436":
                objTempRecord.V436 = objValue.ToString();
                break;

            case "V437":
                objTempRecord.V437 = objValue.ToString();
                break;

            case "V438":
                objTempRecord.V438 = objValue.ToString();
                break;

            case "V439":
                objTempRecord.V439 = objValue.ToString();
                break;

            case "V440":
                objTempRecord.V440 = objValue.ToString();
                break;
            case "V441":
                objTempRecord.V441 = objValue.ToString();
                break;

            case "V442":
                objTempRecord.V442 = objValue.ToString();
                break;

            case "V443":
                objTempRecord.V443 = objValue.ToString();
                break;

            case "V444":
                objTempRecord.V444 = objValue.ToString();
                break;

            case "V445":
                objTempRecord.V445 = objValue.ToString();
                break;

            case "V446":
                objTempRecord.V446 = objValue.ToString();
                break;

            case "V447":
                objTempRecord.V447 = objValue.ToString();
                break;

            case "V448":
                objTempRecord.V448 = objValue.ToString();
                break;

            case "V449":
                objTempRecord.V449 = objValue.ToString();
                break;

            case "V450":
                objTempRecord.V450 = objValue.ToString();
                break;

            case "V451":
                objTempRecord.V451 = objValue.ToString();
                break;

            case "V452":
                objTempRecord.V452 = objValue.ToString();
                break;

            case "V453":
                objTempRecord.V453 = objValue.ToString();
                break;

            case "V454":
                objTempRecord.V454 = objValue.ToString();
                break;

            case "V455":
                objTempRecord.V455 = objValue.ToString();
                break;

            case "V456":
                objTempRecord.V456 = objValue.ToString();
                break;

            case "V457":
                objTempRecord.V457 = objValue.ToString();
                break;

            case "V458":
                objTempRecord.V458 = objValue.ToString();
                break;

            case "V459":
                objTempRecord.V459 = objValue.ToString();
                break;

            case "V460":
                objTempRecord.V460 = objValue.ToString();
                break;
            case "V461":
                objTempRecord.V461 = objValue.ToString();
                break;

            case "V462":
                objTempRecord.V462 = objValue.ToString();
                break;

            case "V463":
                objTempRecord.V463 = objValue.ToString();
                break;

            case "V464":
                objTempRecord.V464 = objValue.ToString();
                break;

            case "V465":
                objTempRecord.V465 = objValue.ToString();
                break;

            case "V466":
                objTempRecord.V466 = objValue.ToString();
                break;

            case "V467":
                objTempRecord.V467 = objValue.ToString();
                break;

            case "V468":
                objTempRecord.V468 = objValue.ToString();
                break;

            case "V469":
                objTempRecord.V469 = objValue.ToString();
                break;

            case "V470":
                objTempRecord.V470 = objValue.ToString();
                break;
            case "V471":
                objTempRecord.V471 = objValue.ToString();
                break;

            case "V472":
                objTempRecord.V472 = objValue.ToString();
                break;

            case "V473":
                objTempRecord.V473 = objValue.ToString();
                break;

            case "V474":
                objTempRecord.V474 = objValue.ToString();
                break;

            case "V475":
                objTempRecord.V475 = objValue.ToString();
                break;

            case "V476":
                objTempRecord.V476 = objValue.ToString();
                break;

            case "V477":
                objTempRecord.V477 = objValue.ToString();
                break;

            case "V478":
                objTempRecord.V478 = objValue.ToString();
                break;

            case "V479":
                objTempRecord.V479 = objValue.ToString();
                break;

            case "V480":
                objTempRecord.V480 = objValue.ToString();
                break;
            case "V481":
                objTempRecord.V481 = objValue.ToString();
                break;

            case "V482":
                objTempRecord.V482 = objValue.ToString();
                break;

            case "V483":
                objTempRecord.V483 = objValue.ToString();
                break;

            case "V484":
                objTempRecord.V484 = objValue.ToString();
                break;

            case "V485":
                objTempRecord.V485 = objValue.ToString();
                break;

            case "V486":
                objTempRecord.V486 = objValue.ToString();
                break;

            case "V487":
                objTempRecord.V487 = objValue.ToString();
                break;

            case "V488":
                objTempRecord.V488 = objValue.ToString();
                break;

            case "V489":
                objTempRecord.V489 = objValue.ToString();
                break;

            case "V490":
                objTempRecord.V490 = objValue.ToString();
                break;
            case "V491":
                objTempRecord.V491 = objValue.ToString();
                break;

            case "V492":
                objTempRecord.V492 = objValue.ToString();
                break;

            case "V493":
                objTempRecord.V493 = objValue.ToString();
                break;

            case "V494":
                objTempRecord.V494 = objValue.ToString();
                break;

            case "V495":
                objTempRecord.V495 = objValue.ToString();
                break;

            case "V496":
                objTempRecord.V496 = objValue.ToString();
                break;

            case "V497":
                objTempRecord.V497 = objValue.ToString();
                break;

            case "V498":
                objTempRecord.V498 = objValue.ToString();
                break;

            case "V499":
                objTempRecord.V499 = objValue.ToString();
                break;

            case "V500":
                objTempRecord.V500 = objValue.ToString();
                break;


        }


    }




    public static DataTable ets_TempRecord_List(int nTableID, int nBatchID, bool? bHasRejectReason, bool? bHasWarningReason,
      bool? bIsActive, DateTime? dDateFrom, DateTime? dDateTo,
         string sOrder,
   string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum,
        ref int iTotalDynamicColumns, string sTextSearch, int? nImportTemplateID, string sType)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TempRecord_List", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                if (sType != "")
                    command.Parameters.Add(new SqlParameter("@sType", sType));

                if (nImportTemplateID!=null)
                    command.Parameters.Add(new SqlParameter("@nImportTemplateID", nImportTemplateID));

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));

                if (bHasRejectReason!=null)
                    command.Parameters.Add(new SqlParameter("@bHasRejectReason", bHasRejectReason));

                if (sTextSearch != "")
                    command.Parameters.Add(new SqlParameter("@sTextSearch", sTextSearch));


                if (bHasWarningReason != null)
                    command.Parameters.Add(new SqlParameter("@bHasWarningReason", bHasWarningReason));

                //if (nEnteredBy != null)
                //    command.Parameters.Add(new SqlParameter("@nEnteredBy", nEnteredBy));

                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));

                //if (sLocations != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sLocations", sLocations));

                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));
                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));


                if (sOrder == string.Empty || sOrderDirection == string.Empty)
                {
                    sOrder = "DBGSystemRecordID";
                    sOrderDirection = "ASC";

                }
                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


               
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

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
                    iTotalDynamicColumns = ds.Tables[0].Columns.Count;
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



    public static DataTable ets_TempRecord_Position_List(int nTableID, int nBatchID, bool bHasRejectReason,
    bool? bHasWaringReason, bool? bIsActive, DateTime? dDateFrom, DateTime? dDateTo,
       string sOrder,
 string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, ref int iTotalDynamicColumns)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TempRecord_Position_List", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));
                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));
                command.Parameters.Add(new SqlParameter("@bHasRejectReason", bHasRejectReason));
                if (bHasWaringReason != null)
                    command.Parameters.Add(new SqlParameter("@bHasWarningReason", bHasWaringReason));

                //if (nEnteredBy != null)
                //    command.Parameters.Add(new SqlParameter("@nEnteredBy", nEnteredBy));

                if (bIsActive != null)
                    command.Parameters.Add(new SqlParameter("@bIsActive", bIsActive));

                //if (sLocations != string.Empty)
                //    command.Parameters.Add(new SqlParameter("@sLocations", sLocations));

                if (dDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dDateFrom", dDateFrom));
                if (dDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dDateTo", dDateTo));


                if (sOrder == string.Empty || sOrderDirection == string.Empty)
                {
                    sOrder = "DBGSystemRecordID";
                    sOrderDirection = "ASC";

                }
                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


               
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

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
                iTotalDynamicColumns = 0;
                iTotalRowsNum = 0;
                if (ds == null) return null;

                if (ds.Tables.Count > 1)
                {
                    iTotalDynamicColumns = ds.Tables[0].Columns.Count;
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


    public static DataTable GetImportFileTableFromText(string strImportFolder, string strFileUniqueName, ref string strError)
    {
        DataTable dtImportFileTable=null;

        using (GenericParserAdapter parser = new GenericParserAdapter(strImportFolder + "\\" + strFileUniqueName))
        {
            dtImportFileTable = parser.GetDataTable();
        }

        //var filestream = new System.IO.FileStream(textFilePath,
        //                                  System.IO.FileMode.Open,
        //                                  System.IO.FileAccess.Read,
        //                                  System.IO.FileShare.ReadWrite);
        //var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);

        //while ((lineOfText = file.ReadLine()) != null)
        //{
        //    //Do something with the lineOfText
        //}

        //FileStream textFile = new FileStream(strImportFolder + "\\" + strFileUniqueName, FileMode.Open, FileAccess.ReadWrite);
        //StreamReader srFile = new StreamReader(textFile);
        //string textToWrite = "";
        //int flag = 0;
        //string line = "";
        //while ((line = srFile.ReadLine()) != null)
        //{
        //    if (flag == 0)
        //    {
        //        //line = line.Replace(" ", "");
        //        textToWrite += line;
        //        flag = 1;
        //        break;
        //    }
        //    else
        //    {
        //        textToWrite += " " + line;
        //    }

        //}
        //textFile.Close();


       


        return dtImportFileTable;
    }
    public static DataTable GetImportFileTableFromDBFInValid(string strImportFolder, string strFileUniqueName, ref string strError)
    {
        string strFileFullPath = strImportFolder + "\\" + strFileUniqueName;
        try
        {
            DataTable dtImportFileTable = null;



            try
            {
                dtImportFileTable = OfficeManager.ImportDBF_OLEDB(strFileFullPath);
            }
            catch
            {
                //
            }

            if (dtImportFileTable != null)
                return dtImportFileTable;

            
            try
            {
                dtImportFileTable = OfficeManager.ImportDBF_Odbc(strFileFullPath);
            }
            catch
            {
                //
            }

            if (dtImportFileTable != null)
                return dtImportFileTable;


            try
            {
                dtImportFileTable = OfficeManager.ImportDBF_VFP(strFileFullPath);
            }
            catch
            {
                //
            }

            if (dtImportFileTable != null)
                return dtImportFileTable;

            //try
            //{
            //    dtImportFileTable=DBFReader.ReadDBF(strFileFullPath);
            //}
            //catch
            //{
            //    //
            //}


            //if (dtImportFileTable != null)
            //    return dtImportFileTable;


            using (GenericParserAdapter parser = new GenericParserAdapter(strFileFullPath))
            {
                parser.MaxBufferSize = 4096 * 255;
                dtImportFileTable = parser.GetDataTable();
            }

            if (dtImportFileTable != null)
                return dtImportFileTable;

        }
        catch
        {
            //
        }
        return null;
    }


    public static DataTable GetImportFileTableFromDBF(string strImportFolder, string strFileUniqueName, ref string strError)
    {
         DataTable dtImportFileTable = null;
        try
        {
          dtImportFileTable=  GetImportFileTableFromDBFInValid(strImportFolder, strFileUniqueName, ref strError);

            if(dtImportFileTable!=null)
            {
                int i = 0;
                foreach(DataColumn dc in dtImportFileTable.Columns)
                {
                    dtImportFileTable.Columns[i].ColumnName = Common.RemoveSpecialCharacters( dc.ColumnName);
                    i = i + 1;
                }
            }
        }
        catch
        {
            //
        }
        return dtImportFileTable;
    }


    public static DataTable GetImportFileTableFromCSV(string strImportFolder, string strFileUniqueName, ref string strError)
    {
        DataTable dtImportFileTable;
        using (GenericParserAdapter parser = new GenericParserAdapter(strImportFolder + "\\" + strFileUniqueName,
            System.Text.Encoding.GetEncoding("iso-8859-1")))
        {
            dtImportFileTable = parser.GetDataTable();
        }

        FileStream textFile = new FileStream(strImportFolder + "\\" + strFileUniqueName, FileMode.Open, FileAccess.ReadWrite);
        StreamReader srFile = new StreamReader(textFile);
        string textToWrite = "";
        int flag = 0;
        string line = "";
        while ((line = srFile.ReadLine()) != null)
        {
            if (flag == 0)
            {
                //line = line.Replace(" ", "");
                textToWrite += line;
                flag = 1;
                break;
            }
            else
            {
                textToWrite += " " + line;
            }

        }
        textFile.Close();


        List<string> strColumnNames = textToWrite.Split(',', '"').Where(s => (!String.IsNullOrEmpty(s))).ToList();

        if (dtImportFileTable.Columns[dtImportFileTable.Columns.Count - 1].ColumnName == "NoName")
        {
            dtImportFileTable.Columns.RemoveAt(dtImportFileTable.Columns.Count - 1);//last column not needed. why?
        }
        for (int i = 0; i < dtImportFileTable.Columns.Count; i++)
        {
            try
            {
                if (strColumnNames.Count > i)
                {
                    dtImportFileTable.Columns[i].ColumnName = strColumnNames[i];
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("already belongs to this DataTable") > -1)
                {
                    //string strSameNameColumn =ex.Message.Substring(0, ex.Message.LastIndexOf("'"));
                    //strSameNameColumn = strSameNameColumn.Substring(16);
                    //strError = "This spreadsheet has multiple columns with the same name(" + strSameNameColumn + "). Each Import Column Name must be unique. Please adjust and try again.";
                    //dtImportFileTable = null;
                    //return dtImportFileTable;
                    for (int j = 1; j < 20; j++)
                    {
                        bool bOK = true;
                        foreach (DataColumn dc in dtImportFileTable.Columns)
                        {
                            if (dc.ColumnName == strColumnNames[i] + j.ToString())
                            {
                                bOK = false;
                            }
                        }

                        if (bOK)
                        {
                            dtImportFileTable.Columns[i].ColumnName = strColumnNames[i] + j.ToString();
                            dtImportFileTable.AcceptChanges();
                            break;
                        }

                    }

                }

            }
        }


        return dtImportFileTable;
    }




    public static DataTable GetImportFileTableFromXML(string strImportFolder, string strFileUniqueName)
    {


        String st = strImportFolder + "\\" + strFileUniqueName;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(st);

        XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

        DataSet ds = new DataSet();
        ds.ReadXml(r);


        return ds.Tables[0];

    }

    public static DataTable GetVirtualImportFileTable(int tableID, int targetTableID, int batchID)
    {

       

        int iTN = 0;

        DataTable dt = new DataTable();

        Column targetTableColumn = RecordManager.ets_Table_Columns(tableID, null, null, ref iTN)
            .Where(x => x.DisplayName == "Target Table").FirstOrDefault();
        if (targetTableColumn != null)
        {
            string targetTableColumnName = targetTableColumn.SystemName;
            string sSearch = String.Format(" AND {1}={2}", batchID, targetTableColumnName, targetTableID);


            SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

            SqlCommand command = new SqlCommand("ets_TempRecord_List", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@nTableID", tableID));
            command.Parameters.Add(new SqlParameter("@nBatchID", batchID));
            command.Parameters.Add(new SqlParameter("@bHasRejectReason", false));
            command.Parameters.Add(new SqlParameter("@sTextSearch", sSearch));
            command.Parameters.Add(new SqlParameter("@sOrder", "[DBGSystemRecordID] ASC"));

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = command;
            DataSet ds = new DataSet();

            connection.Open();
            try
            {
                dataAdapter.Fill(ds);
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Dispose();
                ErrorLog theErrorLog = new ErrorLog(null, "GetVirtualImportFileTable", ex.Message, ex.StackTrace, DateTime.Now, String.Empty);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                string targetDateTimeColumnName = "Date Time Sampled";
                string targetLocationColumnName = "Site Name";

                string sourceSampleTypeColumnName = String.Empty;
                string sourceSiteColumnName = String.Empty;

                string sourceLocationColumnName = String.Empty;
                string sourceDateTimeColumnName = String.Empty;
                string sourceSampleValueColumnName = String.Empty;
                string sourceQualityScoreColumnName = String.Empty;

                DataTable sourceData = ds.Tables[0];

                if (sourceData.Rows.Count > 0)
                {
                    DataTable columns = RecordManager.ets_Table_Columns_All(tableID);
                    if (columns != null)
                    {
                        foreach (DataRow dr in columns.Rows)
                        {
                            if (dr["DisplayName"].ToString() == "Incoming Sample Type")
                                sourceSampleTypeColumnName = dr["NameOnImport"].ToString();
                            else if (dr["DisplayName"].ToString() == "Incoming Site")
                                sourceSiteColumnName = dr["NameOnImport"].ToString();

                            else if (dr["DisplayName"].ToString() == "Sample Point Name")
                                sourceLocationColumnName = dr["NameOnImport"].ToString();
                            else if (dr["DisplayName"].ToString() == "Sample Event Date")
                                sourceDateTimeColumnName = dr["NameOnImport"].ToString();
                            else if (dr["DisplayName"].ToString() == "Sample Value")
                                sourceSampleValueColumnName = dr["NameOnImport"].ToString();
                            else if (dr["DisplayName"].ToString() == "Quality Score")
                                sourceQualityScoreColumnName = dr["NameOnImport"].ToString();
                        }
                    }

                    if (!String.IsNullOrEmpty(sourceSampleTypeColumnName) && !String.IsNullOrEmpty(sourceSiteColumnName))
                    {
                        string incomingSampleType = sourceData.Rows[0][sourceSampleTypeColumnName].ToString();
                        string incomingSite = sourceData.Rows[0][sourceSiteColumnName].ToString();
                        if (!String.IsNullOrEmpty(incomingSampleType) && !String.IsNullOrEmpty(incomingSite))
                        {
                            DataTable dtDateTimeColumnName = Common.DataTableFromText(
                                String.Format("SELECT [NameOnImport (auto populated)] FROM [ALS_Mapping] INNER JOIN [Account24929].[vSystem Column Lookup]" +
                                " ON [ALS_Mapping].[Target Column ID] = [Account24929].[vSystem Column Lookup].[Record ID]" +
                                " WHERE [Incoming Site]='{0}'" +
                                " AND [Incoming Sample Type]='{1}' AND [Incoming Analyte Name]='Sample Event Date'",
                                incomingSite, incomingSampleType));
                            if ((dtDateTimeColumnName != null) && (dtDateTimeColumnName.Rows.Count > 0) &&
                                !String.IsNullOrEmpty(dtDateTimeColumnName.Rows[0][0].ToString()))
                                targetDateTimeColumnName = dtDateTimeColumnName.Rows[0][0].ToString();

                            DataTable dtLocationColumnName = Common.DataTableFromText(
                                String.Format("SELECT [NameOnImport (auto populated)] FROM [ALS_Mapping] INNER JOIN [Account24929].[vSystem Column Lookup]" +
                                " ON [ALS_Mapping].[Target Column ID] = [Account24929].[vSystem Column Lookup].[Record ID]" +
                                " WHERE [Incoming Site]='{0}'" +
                                " AND [Incoming Sample Type]='{1}' AND [Incoming Analyte Name]='Sample Point Name'",
                                incomingSite, incomingSampleType));
                            if ((dtLocationColumnName != null) && (dtLocationColumnName.Rows.Count > 0) &&
                                !String.IsNullOrEmpty(dtLocationColumnName.Rows[0][0].ToString()))
                                targetLocationColumnName = dtLocationColumnName.Rows[0][0].ToString();
                        }
                    }
                }

                DataRow targetRecord = dt.NewRow();
                DataColumn dc = null;

                HashSet<String> targetColumns = new HashSet<String>();

                dc = dt.Columns.Add(targetDateTimeColumnName, typeof(String));
                targetRecord[dc] = targetDateTimeColumnName;
                targetColumns.Add(targetDateTimeColumnName);
                dc = dt.Columns.Add(targetLocationColumnName, typeof(String));
                targetRecord[dc] = targetLocationColumnName;
                targetColumns.Add(targetLocationColumnName);

                foreach (DataRow sourceRecord in sourceData.Rows)
                {
                    string valueColumnName = sourceRecord["Target Column"].ToString();
                    if (!targetColumns.Contains(valueColumnName))
                    {
                        dc = dt.Columns.Add(valueColumnName, typeof(String));
                        targetRecord[dc] = valueColumnName;
                        targetColumns.Add(valueColumnName);
                    }

                    if (!sourceRecord.IsNull(sourceQualityScoreColumnName))
                    {
                        valueColumnName += " QC";
                        if (!targetColumns.Contains(valueColumnName))
                        {
                            dc = dt.Columns.Add(valueColumnName, typeof(String));
                            targetRecord[dc] = valueColumnName;
                            targetColumns.Add(valueColumnName);
                        }
                    }
                }

                dt.Rows.Add(targetRecord);

                //Dictionary<String, Dictionary<String, List<int>>> targetDict =
                //    new Dictionary<String, Dictionary<String, List<int>>>();

                if (!String.IsNullOrEmpty(sourceDateTimeColumnName) && !String.IsNullOrEmpty(sourceLocationColumnName) &&
                    !String.IsNullOrEmpty(sourceSampleValueColumnName))
                {
                    foreach (DataRow sourceRecord in sourceData.Rows)
                    {
                        //if (!targetDict.ContainsKey(record["Sample Point Name"].ToString()))
                        //{
                        //    List<int> values = new List<int>();
                        //    values.Add((int)record["Record ID"]);
                        //    Dictionary<String, List<int>> sample = new Dictionary<String, List<int>>();
                        //    sample.Add(record["Sample Event Date"].ToString(), values);
                        //    targetDict.Add(record["Sample Point Name"].ToString(), sample);
                        //}
                        //else
                        //{
                        //    if (!targetDict[record["Sample Point Name"].ToString()].ContainsKey(record["Sample Event Date"].ToString()))
                        //    {
                        //        List<int> values = new List<int>();
                        //        values.Add((int)record["Record ID"]);
                        //        targetDict[record["Sample Point Name"].ToString()]
                        //            .Add(record["Sample Event Date"].ToString(), values);
                        //    }
                        //    else
                        //    {
                        //        targetDict[record["Sample Point Name"].ToString()][record["Sample Event Date"].ToString()]
                        //            .Add((int)record["Record ID"]);
                        //    }
                        //}

                        targetRecord = null;
                        foreach (DataRow rec in dt.Rows)
                        {
                            if ((rec[targetDateTimeColumnName].ToString() == sourceRecord[sourceDateTimeColumnName].ToString()) &&
                                (rec[targetLocationColumnName].ToString() == sourceRecord[sourceLocationColumnName].ToString()))
                            {
                                targetRecord = rec;
                                break;
                            }
                        }
                        if (targetRecord == null)
                        {
                            targetRecord = dt.NewRow();
                            targetRecord[targetDateTimeColumnName] = sourceRecord[sourceDateTimeColumnName].ToString();
                            targetRecord[targetLocationColumnName] = sourceRecord[sourceLocationColumnName].ToString();
                            dt.Rows.Add(targetRecord);
                        }
                        targetRecord[sourceRecord["Target Column"].ToString()] = sourceRecord[sourceSampleValueColumnName];
                        if (!sourceRecord.IsNull(sourceQualityScoreColumnName))
                        {
                            targetRecord[sourceRecord["Target Column"].ToString() + " QC"] = sourceRecord[sourceQualityScoreColumnName];
                        }
                    }
                }
            }
        }

        dt.AcceptChanges();


        return dt;
    }

    //public static DataTable RemoveEmptyRows(DataTable dtIN)
    //{

    //    foreach (DataRow dr in dtIN.Rows)
    //    {


    //    }




    //    //for (int i = 0; i < dtIN.Rows.Count; i++)
    //    //{
    //    //    bool bEmpty = true;
    //    //    for (int j = 0; j < dtIN.Columns.Count; j++)
    //    //    {
    //    //        if (dtIN.Rows[i][j].ToString() != "")
    //    //            bEmpty = false;

    //    //    }

    //    //    if (bEmpty == true)
    //    //    {

    //    //    }
    //    //}


    //    return dtIN;
    //}



    //public static DataTable ReShapDatatable(DataTable dtOriginalTable, string strAccountName)
    //{
    //    switch (strAccountName.ToLower())
    //    {
    //        case "__asas":

    //            if (dtOriginalTable.Rows.Count > 2)
    //            {

    //                for (int i = 0; i < dtOriginalTable.Columns.Count; i++)
    //                {
    //                    if (dtOriginalTable.Columns[i].ColumnName != "Sitename")
    //                    {
    //                        dtOriginalTable.Columns[i].ColumnName = dtOriginalTable.Rows[1][i].ToString();
    //                    }
    //                }

    //                dtOriginalTable.Rows.RemoveAt(0);
    //                dtOriginalTable.Rows.RemoveAt(0);
    //                dtOriginalTable.Rows.RemoveAt(0);
    //                dtOriginalTable.AcceptChanges();

    //            }

    //            return dtOriginalTable;

    //        default:
    //            return dtOriginalTable;

    //    }



    //}


    public static bool IsDataIntoDropDown(string strData, string strDDLvalues)
    {
        string[] result = strDDLvalues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in result)
        {
            if (strData.ToLower() == s.ToLower())
            {
                return true;
            }
        }

        return false;
    }

    protected bool IsBlanKTempRecord(TempRecord theTempRecord)
    {





        return false;
    }


    public static TempRecord ets_TempRecord_Detail_Full(int nTempRecordID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TempRecord_Detail_Full", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTempRecordID", nTempRecordID));

                connection.Open();


                try
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TempRecord temp = new TempRecord();
                            temp.TempRecordID = nTempRecordID;
                            //temp.LocationID = reader["LocationID"] == DBNull.Value ? null : (int?)reader["LocationID"];
                            temp.TableID = (int)reader["TableID"];
                            temp.DateTimeRecorded = reader["DATETIMERecorded"] == DBNull.Value ? null : (DateTime?)reader["DATETIMERecorded"];
                            temp.Notes = reader["NOTES"] == DBNull.Value ? string.Empty : (string)reader["NOTES"];
                            temp.BatchID = (int)reader["BatchID"];
                            temp.IsActive = (bool)reader["ISACTIVE"];

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
                            temp.RejectReason = reader["RejectReason"] == DBNull.Value ? string.Empty : (string)reader["RejectReason"];
                            temp.WarningReason = reader["WarningReason"] == DBNull.Value ? string.Empty : (string)reader["WarningReason"];
                            //temp.ExceedanceReason = reader["ExceedanceReason"] == DBNull.Value ? string.Empty : (string)reader["ExceedanceReason"];

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



    public static DataTable ets_Record_Webservice(int nTableID,
    int nBatchID, string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, ref int iTotalDynamicColumns)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Record_Webservice", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));


                if (sOrder != string.Empty && sOrderDirection != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));


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
                iTotalDynamicColumns = 0;
                iTotalRowsNum = 0;
                if (ds == null) return null;

                if (ds.Tables.Count > 1)
                {
                    iTotalDynamicColumns = ds.Tables[0].Columns.Count;
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





    public static DataTable ets_TempRecord_WebService(int nTableID,
   int nBatchID, string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, ref int iTotalDynamicColumns)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_TempRecord_WebService", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                command.Parameters.Add(new SqlParameter("@nBatchID", nBatchID));


                if (sOrder != string.Empty && sOrderDirection != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));


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
                iTotalDynamicColumns = 0;
                iTotalRowsNum = 0;
                if (ds == null) return null;


                if (ds.Tables.Count > 1)
                {
                    iTotalDynamicColumns = ds.Tables[0].Columns.Count;
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







    public static int ets_Upload_Insert(Upload p_Upload)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Upload_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sEmailFrom", p_Upload.EmailFrom));
                command.Parameters.Add(new SqlParameter("@sFilename", p_Upload.Filename));
                //command.Parameters.Add(new SqlParameter("@nLocationID", p_Upload.LocationID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_Upload.TableID));
                command.Parameters.Add(new SqlParameter("@sUploadName", p_Upload.UploadName));
                command.Parameters.Add(new SqlParameter("@bUseMapping", p_Upload.UseMapping));



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


    public static int ets_Upload_Update(Upload p_Upload)
    {




        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Upload_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@nUploadID", p_Upload.UploadID));
                command.Parameters.Add(new SqlParameter("@sEmailFrom", p_Upload.EmailFrom));
                command.Parameters.Add(new SqlParameter("@sFilename", p_Upload.Filename));
                //command.Parameters.Add(new SqlParameter("@nLocationID", p_Upload.LocationID));
                command.Parameters.Add(new SqlParameter("@nTableID", p_Upload.TableID));
                command.Parameters.Add(new SqlParameter("@sUploadName", p_Upload.UploadName));
                command.Parameters.Add(new SqlParameter("@bUseMapping", p_Upload.UseMapping));

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



    public static DataTable ets_Upload_Select(int? nTableID, string sUploadName, string sEmailFrom,
       string sFilename, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Upload_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (HttpContext.Current.Session["AccountID"] != null)
                {
                    command.Parameters.Add(new SqlParameter("@nAccountID", int.Parse(HttpContext.Current.Session["AccountID"].ToString())));
                }

                if (sUploadName != "")
                    command.Parameters.Add(new SqlParameter("@sUploadName", sUploadName));

                if (sEmailFrom != "")
                    command.Parameters.Add(new SqlParameter("@sEmailFrom", sEmailFrom));

                if (sFilename != "")
                    command.Parameters.Add(new SqlParameter("@sFilename", sFilename));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "UploadID"; sOrderDirection = "DESC"; }

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




    public static int ets_Upload_Delete(int nUploadID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Upload_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nUploadID ", nUploadID));

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




    public static Upload ets_Upload_Detail(int nUploadID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {


            using (SqlCommand command = new SqlCommand("ets_Upload_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUploadID", nUploadID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Upload temp = new Upload(
                                (int)reader["UploadID"], (int)reader["TableID"],
                                //reader["LocationID"] == DBNull.Value ? null : (int?)reader["LocationID"],
                                (string)reader["UploadName"], (string)reader["EmailFrom"], (string)reader["Filename"],
                                (bool)reader["UseMapping"]
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








}







