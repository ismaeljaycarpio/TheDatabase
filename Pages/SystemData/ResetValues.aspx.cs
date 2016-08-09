using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_SystemData_ResetValues : SecurePage
{

    User _ObjUser;
    UserRole _theUserRole;
    protected void Page_Load(object sender, EventArgs e)
    {
        _ObjUser = (User)Session["User"];
        _theUserRole = (UserRole)Session["UserRole"];


        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        { 
            Response.Redirect("~/Default.aspx", false);
            return;
        }
        if(!IsPostBack)
        {
            PopulateTable();
        }



    }


    protected void lnkShowAllTables_Click(object sender, EventArgs e)
    {
       // lblMessage.Text = "";
        lblErrorMsg.Text = "";
        try
        {
            Common.ExecuteText("DELETE  FROM ResetValuesDone WHERE ID1 IN(SELECT TableID FROM [Table] WHERE [Table].AccountID=" + Session["AccountID"].ToString() + ")");
//            Common.ExecuteText(@"DELETE  FROM ResetValuesDone WHERE ID2 IN(SELECT ColumnID FROM [Column] C
//	                    INNER JOIN [Table] T ON C.TableID=T.TableID WHERE T.AccountID=" + Session["AccountID"].ToString() + @")");
            PopulateTable();
        }
        catch(Exception ex)
        {
            lblErrorMsg.Text = ex.Message + ex.StackTrace;
            //
        }

    }

    protected void lnkClearMessage_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        lnkClearMessage.Visible = false;
    }
    protected void lnkReset_Click(object sender, EventArgs e)
    {

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        //long before = System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64;



        lblErrorMsg.Text = "";
        lnkClearMessage.Visible = true;
        string strMessage = "";

        bool bSelected = false;
        try
        {
            foreach (ListItem item in lstTable.Items)
            {
                if (item.Selected == true)
                {
                    bSelected = true;
                    break;
                }
            }
            if (bSelected == false)
            {
                lblErrorMsg.Text = "Please select table(s).";
                return;
            }

            //lblMessage.Text = "";
            foreach (ListItem item in lstTable.Items)
            {
                if (item.Selected == true)
                {


                    Table theTable = RecordManager.ets_Table_Details(int.Parse(item.Value));


                   

                    bool bErrorInAnyColumn = false;
                    if (theTable != null)
                    {
                        strMessage = strMessage + "</br></br>" + "<strong>" + item.Text + " Started...</strong>";
                        DateTime dtStart = DateTime.Now;

                        DataTable dtColumnsAll = RecordManager.ets_Table_Columns_All((int)theTable.TableID);
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


                        List<int> lstRecordIDs = TheDatabaseS.ListIDsByCOALESCE((int)theTable.TableID);


                        DataTable dtColumns = Common.DataTableFromText(@" SELECT ColumnID,SystemName,ColumnType,DisplayName,TextType,DateCalculationType,
Calculation,ValidationOnEntry,ValidationOnExceedance,ValidationOnWarning,IgnoreSymbols FROM [Column] C WHERE C.ColumnType='calculation' AND LEN( C.Calculation)>0
                       AND C.TableID=" + theTable.TableID.ToString() + @"
                        ORDER BY DisplayOrder");

                        bool bHasValidationOnEntry = false;
                        bool bHasValidationOnWarning = false;
                        foreach (DataRow drC in dtColumns.Rows)
                        {
                            if (drC["TextType"] != DBNull.Value && drC["TextType"].ToString() == "d")
                            {
                            }
                            else
                            {
                                drC["Calculation"] = Common.GetCalculationSystemNameOnly(drC["Calculation"].ToString(), (int)theTable.TableID);
                            }

                            if (drC["ValidationOnEntry"] != DBNull.Value && drC["ValidationOnEntry"].ToString().Length>0)
                            {
                                bHasValidationOnEntry = true;
                            }

                            if (drC["ValidationOnWarning"] != DBNull.Value && drC["ValidationOnWarning"].ToString().Length > 0)
                            {
                                bHasValidationOnWarning = true;
                            }
                        }
                        dtColumns.AcceptChanges();


                        int iInvalid = 0;
                        int iExceedance = 0;
                        int iWarning = 0;
                        foreach (int iRecordID in lstRecordIDs)
                        {
                            Record aRecord = RecordManager.ets_Record_Detail_Full(iRecordID);

                            if (aRecord != null)
                            {
                                
                                try
                                {                                  
                                    
                                    foreach (DataRow drC in dtColumns.Rows)
                                    {
                                        string strTemp = "";

                                        if(drC["TextType"]!=DBNull.Value && drC["TextType"].ToString()=="d")
                                        {
                                            //date calculation

                                            try
                                            {
                                                string strNewValue = TheDatabaseS.GetDateCalculationResult(ref dtColumnsAll, drC["Calculation"].ToString(), null, aRecord, null,
                                                drC["DateCalculationType"] == DBNull.Value ? "" : drC["DateCalculationType"].ToString(),
                                                                           null, theTable, bCheckIgnoreMidnight);

                                                RecordManager.MakeTheRecord(ref aRecord, drC["SystemName"].ToString(), strNewValue);
                                            }
                                            catch
                                            {
                                                //

                                                
                                            }
                                            
                                        }
                                        else
                                        {
                                            //Number
                                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(drC["ColumnID"].ToString()));
                                            string strNewValue = TheDatabaseS.GetCalculationResult(ref dtColumnsAll, drC["Calculation"].ToString(), null, aRecord, null, null, theTable, theColumn);

                                            RecordManager.MakeTheRecord(ref aRecord, drC["SystemName"].ToString(), strNewValue);

                                            if(bHasValidationOnEntry)
                                            {
                                                if (drC["ValidationOnEntry"] != DBNull.Value && drC["ValidationOnEntry"].ToString().Length > 0)
                                                {
                                                    if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, drC["ValidationOnEntry"].ToString(), ref strTemp))
                                                    {
                                                        aRecord.IsActive = false;                                                       
                                                        iInvalid = iInvalid + 1;
                                                        if (aRecord.ValidationResults.IndexOf("INVALID:" + drC["DisplayName"].ToString()) > -1)
                                                        {
                                                            //
                                                        }
                                                        else
                                                        {
                                                            aRecord.ValidationResults = aRecord.ValidationResults + " INVALID:" + drC["DisplayName"].ToString();
                                                        }

                                                    }
                                                    else
                                                    {
                                                        aRecord.ValidationResults = aRecord.ValidationResults.Replace("INVALID:" + drC["DisplayName"].ToString(), "");
                                                    }
                                                }
                                            }

                                            bool bEachColumnExceedance = false;

                                            if(bShowExceedances)
                                            {
                                                if (drC["ValidationOnExceedance"] != DBNull.Value && drC["ValidationOnExceedance"].ToString().Length > 0)
                                                 {
                                                     if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, drC["ValidationOnExceedance"].ToString(), ref strTemp))
                                                     {
                                                         iExceedance = iExceedance + 1;
                                                         if (aRecord.WarningResults.IndexOf("EXCEEDANCE: " + drC["DisplayName"].ToString()) > -1)
                                                         {
                                                             //old
                                                         }
                                                         else
                                                         {
                                                             aRecord.WarningResults = aRecord.WarningResults + " EXCEEDANCE: " + drC["DisplayName"].ToString() + " – Value outside accepted range.";
                                                         }


                                                         bEachColumnExceedance = true;

                                                         aRecord.WarningResults = aRecord.WarningResults.Replace("WARNING: " + drC["DisplayName"].ToString() + " – Value outside accepted range.", "");
                                                     }
                                                     else
                                                     {
                                                         aRecord.WarningResults = aRecord.WarningResults.Replace("EXCEEDANCE: " + drC["DisplayName"].ToString() + " – Value outside accepted range.", "");

                                                     }
                                                 }
                                            }

                                            if (bEachColumnExceedance == false && bHasValidationOnWarning && drC["DisplayName"] != DBNull.Value && drC["ValidationOnWarning"].ToString() != "")
                                            {
                                                aRecord.WarningResults = aRecord.WarningResults.Replace("EXCEEDANCE: " + drC["DisplayName"].ToString() + " – Value outside accepted range.", "");

                                                //if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, theColumn.ValidationOnWarning, ref strTemp, theColumn.IgnoreSymbols == null ? false : (bool)theColumn.IgnoreSymbols, null))
                                                if (strNewValue != "" && !UploadManager.IsDataValid(strNewValue, drC["ValidationOnWarning"].ToString(), ref strTemp))
                                                {
                                                    iWarning = iWarning + 1;
                                                    if (aRecord.WarningResults.IndexOf("WARNING: " + drC["DisplayName"].ToString() + " – Value outside accepted range.") > -1)
                                                    {
                                                        //old
                                                    }
                                                    else
                                                    {
                                                        aRecord.WarningResults = aRecord.WarningResults + " WARNING: " + drC["DisplayName"].ToString() + " – Value outside accepted range.";
                                                    }

                                                }
                                                else
                                                {

                                                    aRecord.WarningResults = aRecord.WarningResults.Replace("WARNING: " + drC["DisplayName"].ToString() + " – Value outside accepted range.", "");


                                                }
                                            }                                           

                                        }
                                    }

                                    RecordManager.ets_Record_Update(aRecord, null);

                                }
                                catch
                                {

                                }
                            }
                        }

                        strMessage = strMessage + "</br>" + dtColumns.Rows.Count.ToString() + " calculated columns value are updated.</br>";
                        
                        if (iInvalid > 0)
                        {
                            strMessage = " InValid(" + iInvalid.ToString() + " cells) ";
                        }
                        if (iExceedance > 0)
                        {
                            strMessage = strMessage + " Exceedance(" + iExceedance.ToString() + " cells) ";
                        }
                        if (iWarning > 0)
                        {
                            strMessage = strMessage + " Warning(" + iWarning.ToString() + " cells)";
                        }


                        foreach (DataRow drC in dtColumns.Rows)
                        {
                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(drC[0].ToString()));

                            if (theColumn != null)
                            {
                                strMessage = strMessage + "</br>" + "<span style='color:green;padding-left:10px;'> Calculated column:" + theColumn.DisplayName  + "</span>";
                            }
                        }

                        DateTime dtEnd = DateTime.Now;
                        double diffInSeconds = (dtEnd - dtStart).TotalSeconds;
                        
                        if (bErrorInAnyColumn == false)
                        {
                            Common.ExecuteText(" INSERT INTO ResetValuesDone (ID1) VALUES(" + theTable.TableID.ToString() + ")");
                            strMessage = strMessage + "</br>" + "<strong style='color:blue;'>DONE:" + theTable.TableName + " Table: Total " + diffInSeconds.ToString("N1") + "</span> Seconds </strong>";

                        }
                        else
                        {
                            strMessage = strMessage + "</br>" + "<strong style='color:red'>" + theTable.TableName + " -- end(" + diffInSeconds.ToString("N1") + " Seconds) with error, please try again.</strong>";
                        }

                    }




                }
            }
            
        }
        catch(Exception ex)
        {
            lblErrorMsg.Text = ex.Message + "--" + ex.StackTrace;
            //

        }

        //long after = System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64;

        //strMessage = strMessage + " </br> memory used " + before.ToString("N2") + " to " + after.ToString("N2")
        //    + "</br>" + (after-before).ToString("N2");

        if (strMessage != "")
            lblMessage.Text = strMessage + "<hr>" + lblMessage.Text;

        PopulateTable();



        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

    }






//    protected void lnkResetNew_Click(object sender, EventArgs e)
//    {
//        lblErrorMsg.Text = "";
//        lnkClearMessage.Visible = true;
//        string strMessage = "";

//        bool bSelected = false;
//        try
//        {
//            foreach (ListItem item in lstTable.Items)
//            {
//                if (item.Selected == true)
//                {
//                    bSelected = true;
//                    break;
//                }
//            }
//            if (bSelected == false)
//            {
//                lblErrorMsg.Text = "Please select table(s).";
//                return;
//            }

//            //lblMessage.Text = "";
//            foreach (ListItem item in lstTable.Items)
//            {
//                if (item.Selected == true)
//                {
//                    Table theTable = RecordManager.ets_Table_Details(int.Parse(item.Value));

//                    bool bErrorInAnyColumn = false;
//                    if (theTable != null)
//                    {

//                        strMessage = strMessage + "</br></br>" + "<strong>" + item.Text + " Started...</strong>";
//                        DateTime dtStart = DateTime.Now;
//                        DataTable dtColumns = Common.DataTableFromText(@" SELECT ColumnID FROM [Column] C WHERE C.ColumnType='calculation' AND LEN( C.Calculation)>0
//                       AND C.TableID=" + theTable.TableID.ToString() + @"
//                       AND C.ColumnID NOT IN(SELECT DISTINCT ID2 FROM ResetValuesDone  WHERE ID2 IS NOT NULL) ORDER BY DisplayOrder");

//                        foreach (DataRow drC in dtColumns.Rows)
//                        {
//                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(drC[0].ToString()));

//                            if (theColumn != null)
//                            {
//                                string strColumnError = "";
//                                try
//                                {
//                                    string strInfo = RecordManager.ets_AdjustCalculationFormulaChanges(theColumn, ref strColumnError);

//                                    if (strInfo.Trim() != "")
//                                        strInfo = "--" + strInfo;

//                                    Common.ExecuteText(" INSERT INTO ResetValuesDone (ID2) VALUES(" + theColumn.ColumnID.ToString() + ")");
//                                    strMessage = strMessage + "</br>" + "<span style='color:green;padding-left:10px;'> DONE:" + theColumn.DisplayName + strInfo + "</span>";

//                                }
//                                catch
//                                {
//                                    bErrorInAnyColumn = true;
//                                    strMessage = strMessage + "</br>" + "<span style='color:red;padding-left:10px;'>Error:" + theColumn.DisplayName + "--" + strColumnError + "</span>";
//                                }
//                            }
//                        }
//                        DateTime dtEnd = DateTime.Now;
//                        double diffInSeconds = (dtEnd - dtStart).TotalSeconds;

//                        if (bErrorInAnyColumn == false)
//                        {
//                            Common.ExecuteText(" INSERT INTO ResetValuesDone (ID1) VALUES(" + theTable.TableID.ToString() + ")");
//                            strMessage = strMessage + "</br>" + "<strong style='color:blue;'>DONE:" + theTable.TableName + " Table: Total " + diffInSeconds.ToString("N1") + "</span> Seconds </strong>";

//                        }
//                        else
//                        {
//                            strMessage = strMessage + "</br>" + "<strong style='color:red'>" + theTable.TableName + " -- end(" + diffInSeconds.ToString("N1") + " Seconds) with error, please try again.</strong>";
//                        }

//                    }




//                }
//            }

//        }
//        catch (Exception ex)
//        {
//            lblErrorMsg.Text = ex.Message + "--" + ex.StackTrace;
//            //

//        }


//        if (strMessage != "")
//            lblMessage.Text = strMessage + "<hr>" + lblMessage.Text;

//        PopulateTable();





//    }


    
    protected void PopulateTable()
    {
        lstTable.Items.Clear();
        //ID1-TableID,2-ColumnID,3-RecordID  int.Parse(Session["AccountID"].ToString())
        try
        {
            DataTable dtTemp = Common.DataTableFromText(@"SELECT T.TableID,T.TableName,COUNT(*) NC, (SELECT COUNT(*) FROM [Record] R
	                   WHERE  R.IsActive=1 AND R.TableID=T.TableID) AS RC  FROM [Table] T INNER JOIN [Column] C
                       ON T.TableID=C.TableID WHERE T.IsActive=1 AND T.AccountID=" + int.Parse(Session["AccountID"].ToString()) + @"
                       AND C.ColumnType='calculation' AND LEN( C.Calculation)>0
                       AND T.TableID NOT IN(SELECT DISTINCT ID1 FROM ResetValuesDone  WHERE ID1 IS NOT NULL)
                       GROUP BY T.TableID,T.TableName
                       ORDER BY TableName");

            foreach (DataRow dr in dtTemp.Rows)
            {
                ListItem liTemp = new ListItem(dr[1].ToString() + " (" + dr[2].ToString() + " columns & " + dr[3].ToString() + " records)", dr[0].ToString());
                lstTable.Items.Add(liTemp);
            }

        }
        catch (Exception ex)
        {

        }

       
    }


}