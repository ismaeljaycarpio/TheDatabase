using System;
//using System.Collections;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Data.SqlClient;
using DBG.Common;
public partial class Pages_Record_RecordUpload : SecurePage
{

    string _strFilesPhisicalPath = "";
    string _strFilesLocation="";
    Table _theTable;
    ImportTemplate _theImportTemplate;
    Menu _qsMenu;
    User _ObjUser;
    string _qsTableID = "";
    int _iTotalDynamicColumns = 0;
    int _iValidRecordIDIndex = 0;
    string _strDateRecordedColumnName = "Date Recorded";
    string _strTimeSamledColumnName = "Time Recorded";
    string _strRecordRightID = Common.UserRoleType.None;
    protected void Page_Load(object sender, EventArgs e)
    {

        

        try
        {
            _ObjUser = (User)Session["User"];


            _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();
            _strFilesLocation = Session["FilesLocation"].ToString();
            _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"]);
            UserRole theUserRole = (UserRole)Session["UserRole"];
            if ((bool)theUserRole.IsAdvancedSecurity)
            {
                //DataTable dtUserTable = SecurityManager.ets_UserTable_Select(null,
                //    int.Parse(_qsTableID), _ObjUser.UserID, null);

                DataTable dtUserTable = null;

                //if (_ObjUser.RoleGroupID == null)
                //{
                    dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
                   int.Parse(_qsTableID), theUserRole.RoleID, null);
                //}
                //else
                //{

                //    dtUserTable = SecurityManager.dbg_RoleGroupTable_Select((int)_ObjUser.RoleGroupID, null,
                //  int.Parse(_qsTableID), null);
                //}


                if (dtUserTable.Rows.Count > 0)
                {
                    _strRecordRightID = dtUserTable.Rows[0]["RoleType"].ToString();
                }               
            }
            else
            {
                if (Session["roletype"] != null)
                    _strRecordRightID = Session["roletype"].ToString();
            }
            if (_strRecordRightID == Common.UserRoleType.None) //none role
            {
                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Default.aspx", false);
                return;
            }


            if (!Common.HaveAccess(_strRecordRightID, "1,2,3,4,5,7,8,9"))
            { Response.Redirect("~/Default.aspx", false); }

            if (Common.HaveAccess(_strRecordRightID, Common.UserRoleType.ReadOnly))
            {
                lnkNext.Enabled = false;
                lblMsg.Text = "Read Only Account. To upload please ask your Account Administrator for appropriate rights.";
            }

            if(!IsPostBack)
                PopulateImportTemplate(int.Parse(_qsTableID));

            if (Request.QueryString["TableID"] != null)
            {
               
                _theTable = RecordManager.ets_Table_Details(int.Parse(_qsTableID));
                //_qsMenu = RecordManager.ets_Menu_Details((int)_theTable.MenuID);


                DataTable dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      ImportHeaderName FROM  [Column] C JOIN ImportTemplateItem ITI 
                        ON C.ColumnID=ITI.ColumnID
                        WHERE SystemName='DateTimeRecorded' AND TableID=" + _theTable.TableID.ToString());
                if (dtDateTimeColumnName.Rows.Count > 0 && dtDateTimeColumnName.Rows[0][0] != DBNull.Value)
                {
                    string strDT = dtDateTimeColumnName.Rows[0][0].ToString();
                    if (strDT.IndexOf(",") > 0)
                    {
                        _strDateRecordedColumnName = strDT.Substring(0, strDT.IndexOf(","));
                        _strTimeSamledColumnName = strDT.Substring(strDT.IndexOf(",") + 1);
                    }
                    else
                    {
                        _strDateRecordedColumnName = strDT;
                        _strTimeSamledColumnName = "";
                    }
                }

            }
            else
            {
                Response.Redirect("~/Default.aspx", false);
            }

            


            

            

            if (!IsPostBack)
            {
                BindGrids();
                //if(_theTable.IsDataUpdateAllowed!=null && (bool)(_theTable.IsDataUpdateAllowed))
                //{
                //    trDataUpdateUniqueColumnID.Visible = true;
                //    chkDataUpdateUniqueColumnID.Checked = true;
                //    string strMatchColumn = "";
                //    if(_theTable.UniqueColumnID!=null)
                //    {
                //        Column theUniqueColumn = RecordManager.ets_Column_Details((int)_theTable.UniqueColumnID);
                //        strMatchColumn = theUniqueColumn.DisplayName;
                //    }
                //    if (_theTable.UniqueColumnID2 != null)
                //    {
                //        Column theUniqueColumn2 = RecordManager.ets_Column_Details((int)_theTable.UniqueColumnID2);
                //        strMatchColumn = strMatchColumn == "" ? theUniqueColumn2.DisplayName : strMatchColumn + " and " + theUniqueColumn2.DisplayName;
                //    }

                //    chkDataUpdateUniqueColumnID.Text = "Update existing data, matching on [" + strMatchColumn + "]";
                //}


               

                //CheckLocationColumn();
                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();
            }

            if (_theTable.CustomUploadSheet != "")
            {
               
                //hlCustomUploadSheet.NavigateUrl = "~//UserFiles//Template//" + _theTable.CustomUploadSheet;


                if (_theTable.CustomUploadSheet != "")
                {
                    try
                    {
                        //string strFilePath = Server.MapPath("~/UserFiles/Template/" + _theTable.CustomUploadSheet);

                        string strFilePath = _strFilesPhisicalPath + "\\UserFiles\\Template\\" + _theTable.CustomUploadSheet;

                        if (File.Exists(strFilePath))
                        {
                            lblImortType.Visible = false;
                            divColumnName.Visible = false;
                            divColumnPosition.Visible = false;
                            divCustomUploadFiles.Visible = true;
                            divUploadFiles.Visible = false;
                          
                            hlCustomUploadSheet.Text = (_theTable.CustomUploadSheet.Substring(_theTable.CustomUploadSheet.IndexOf("_") + 1)).ToString();
                        }
                    }
                    catch
                    {
                        //
                    }
                }
            }
            



            Title = "Upload Records - " + _theTable.TableName;
            lblTitle.Text = "Upload Records - " + _theTable.TableName;

            if (!IsPostBack)
            {
                if (Session["CurrentBatch"] != null)
                {
                    txtBatchDescription.Text = Session["CurrentBatch"].ToString();
                }
                //PopulateLocationDDL();
            }


            if (!IsPostBack && _theTable != null && _theTable.DefaultImportTemplateID != null)
            {
                int? iIT_ID = _theTable.DefaultImportTemplateID; //Common.GetDefaultImportTemplate(_theTable.TableID);

                if (iIT_ID != null && ddlTemplate.Items.FindByValue(iIT_ID.ToString()) != null)
                {
                    ddlTemplate.SelectedValue = iIT_ID.ToString();
                    ddlTemplate_SelectedIndexChanged(null, null);
                }
            }



        }
        catch (Exception ex)
        {
            
            ErrorLog theErrorLog = new ErrorLog(null, "Record Upload", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

        }

    }

//    protected void CheckLocationColumn()
//    {
//        if ((bool)_theTable.IsImportPositional)
//        {
//            DataTable dtTemp = Common.DataTableFromText(@"Select Columnid from [Column] WHERE  
//                                TableID=" + _theTable.TableID.ToString() + " AND SystemName='LocationID' AND PositionOnImport is not null");

//            if (dtTemp.Rows.Count > 0)
//            {
//                trLocation.Visible = false;
//            }
//            else
//            {
//                trLocation.Visible = true;
//            }
//        }
//        else
//        {
//            DataTable dtTemp = Common.DataTableFromText(@"Select Columnid from [Column] WHERE   
//                                TableID=" + _theTable.TableID.ToString() + " AND SystemName='LocationID' AND Name_OnImport is not null");

//            if (dtTemp.Rows.Count > 0)
//            {
//                trLocation.Visible = false;
//            }
//            else
//            {
//                trLocation.Visible = true;
//            }
//        }
//    }


    protected List<string> GetListColumnsByName()
    {
        if (_theImportTemplate == null && ddlTemplate.SelectedValue != "")
        {
            _theImportTemplate = ImportManager.dbg_ImportTemplate_Detail(int.Parse(ddlTemplate.SelectedValue));
        }
        DataTable dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import(int.Parse(_qsTableID), _theImportTemplate.ImportTemplateID);
        List<string> lstColumns = new List<string>();
        for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
        {
            string strBeforeComma = dtRecordTypleColumns.Rows[j]["ImportHeaderName"].ToString();
            string strAfterComma = "";
            if (strBeforeComma.ToString().Trim().IndexOf(",") == -1)
            {
                //
            }
            else
            {
                strAfterComma = Common.AferComma(strBeforeComma);
                strBeforeComma = Common.BeforeComma(strBeforeComma);
            }

            if (strBeforeComma != "" && lstColumns.Contains(strBeforeComma) == false)
            {
                lstColumns.Add(strBeforeComma);
            }

            if (strAfterComma != "" && lstColumns.Contains(strAfterComma) == false)
            {
                lstColumns.Add(strAfterComma);
            }
        }
        return lstColumns;
    }
    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.Cells[_iTotalDynamicColumns].Visible = false;
        //    e.Row.Cells[_iValidRecordIDIndex + 1].Visible = false;

        //}

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[_iTotalDynamicColumns].Visible = false;
        //    e.Row.Cells[_iValidRecordIDIndex + 1].Visible = false;
        //}
    }
    protected DataTable GetPositionDataTable()
    {
        return Common.DataTableFromText(@"SELECT SystemName, DisplayName , PositionOnImport 
             FROM [Column] C JOIN ImportTemplateItem ITI ON C.ColumnID=ITI.ColumnID WHERE ITI.ImportTemplateID="
             + ddlTemplate.SelectedValue + @" AND PositionOnImport IS NOT NULL
                ORDER BY CASE 
				                WHEN Charindex(',', PositionOnImport)=0 THEN CAST(PositionOnImport AS INT)
				                WHEN Charindex(',', PositionOnImport)>1 THEN CAST(Substring(PositionOnImport, 1,Charindex(',', PositionOnImport)-1) AS INT)
		                  END");
    }

    protected void lnkDownloadXLS_Click(object sender, EventArgs e)
    {
        //XLS

        //Response.Clear();
        //Response.Buffer = true;
        //Response.AddHeader("content-disposition",
        //"attachment;filename=\"" + _theTable.TableName + ".xls\"");
        //Response.Charset = "";
        //Response.ContentType = "application/vnd.ms-excel";

        //StringWriter sw = new StringWriter();

        DataTable dtColumns = new DataTable();

        GetImportTemplate();

      
        if (_theImportTemplate.IsImportPositional == true)
        {
           
          
            //bool bFoundDateTime = false;
            dtColumns = GetPositionalColumns();
        }
        else
        {
            //int iTemp = 0;


            //int? iImportTemplateID = null;

            //if (ddlTemplate.SelectedValue != "")
            //    iImportTemplateID = int.Parse(ddlTemplate.SelectedValue);

            //dt = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
            //     -1, false, null, null,
            //    null, null,
            //  "", "", null, null, ref iTemp, ref _iTotalDynamicColumns, "", iImportTemplateID, "");
            //DataSet ds = new DataSet();

            ////OfficeManager.CreateWorkbook(dt.DataSet, Server.MapPath(""));

            //DataTable dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import(int.Parse(_qsTableID), iImportTemplateID);

            //string strDateTimeCaption = "";
            //for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
            //{
            //    if (dtRecordTypleColumns.Rows[j]["SystemName"].ToString().Trim().ToLower() == "datetimerecorded")
            //    {
            //        strDateTimeCaption = dtRecordTypleColumns.Rows[j]["ImportHeaderName"].ToString().Trim();
            //    }
            //}

            //int n = 0;

            //foreach (DataColumn dc in dt.Columns)
            //{
            //    if (dc.ColumnName.ToLower() != "dbgsystemrecordid" && dc.ColumnName.ToLower() != "rownum")
            //    {
            //        if (dc.ColumnName.ToLower() == strDateTimeCaption.ToLower())
            //        {

            //            dtColumns.Columns.Add(_strDateRecordedColumnName, typeof(DateTime));
            //            if (_strTimeSamledColumnName != "")
            //            {
            //                dtColumns.Columns.Add(_strTimeSamledColumnName);
            //            }
            //            //if (n == 0)
            //            //{
            //            //    sw.Write("Date Recorded" + "\t" + "Time Recorded");
            //            //}
            //            //else
            //            //{
            //            //    sw.Write("\t" + "Date Recorded" + "\t" + "Time Recorded");
            //            //}

            //        }
            //        else
            //        {

            //            dtColumns.Columns.Add(dc.ColumnName);
            //            //if (n == 0)
            //            //{

            //            //    sw.Write(dc.ColumnName);
            //            //}
            //            //else
            //            //{

            //            //    sw.Write("\t" + dc.ColumnName);
            //            //}
            //        }
            //        n = n + 1;
            //    }

            //}           

            //Response.Write(sw.ToString());
            //Response.Flush();
            //Response.End();

            List<string> lstColumns = new List<string>();

            lstColumns = GetListColumnsByName();
            foreach (string strC in lstColumns)
            {
                dtColumns.Columns.Add(strC);
            }
            dtColumns.AcceptChanges();
            dtColumns.Rows.Add(dtColumns.NewRow());
            dtColumns.AcceptChanges();
        }


        

        ExportUtil.ExportToExcel(dtColumns, _theTable.TableName.Replace(' ','-') + ".xls");

    }



    protected void hlCustomUploadSheet_Click(object sender, EventArgs e)
    {
        if (_theTable.CustomUploadSheet != "")
        {
            string strFilePath = _strFilesPhisicalPath + "\\UserFiles\\Template\\" + _theTable.CustomUploadSheet;
            if (File.Exists(strFilePath))
            {
                Response.ContentType = "application/octet-stream";

                //Response.AppendHeader("Content-Disposition", "attachment; filename=" +  theDocument.FileUniqename.Substring(37));
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + (_theTable.CustomUploadSheet.Substring(_theTable.CustomUploadSheet.IndexOf("_") + 1)).ToString());
                Response.WriteFile(strFilePath);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This file is missing!');", true);

            }
        }
    }

    //protected void lnkDownloadXLS_Click(object sender, EventArgs e)
    //{
    //    //XLS

    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition",
    //    "attachment;filename=\"" + _theTable.TableName + ".xls\"");
    //    Response.Charset = "";
    //    Response.ContentType = "application/vnd.ms-excel";

    //    StringWriter sw = new StringWriter();
    //    //HtmlTextWriter hw = new HtmlTextWriter(sw);



    //    //BindGridAgainToExport();

    //    DataTable dt;
    //    if (_theTable.IsImportPositional == true)
    //    {
    //        dt = Common.DataTableFromText("SELECT  SystemName,DisplayName , PositionOnImport " +
    //               " FROM [Column] WHERE  TableID = " + _theTable.TableID.ToString() +
    //                                                              " AND PositionOnImport IS NOT NULL" +
    //                                                              " ORDER BY PositionOnImport");
    //        //DataTable dtCustom=new DataTable();
    //        bool bFoundDateTime = false;
    //        if (dt.Rows.Count > 0)
    //        {
    //            int iMaxColumn = int.Parse(dt.Rows[dt.Rows.Count - 1]["PositionOnImport"].ToString());
    //            int m = 0;

    //            for (int i = 1; i <= iMaxColumn; i++)
    //            {

    //                string strColumnName;
    //                if (bFoundDateTime)
    //                {
    //                    strColumnName = "C" + (i + 1).ToString();
    //                }
    //                else
    //                {
    //                    strColumnName = "C" + i.ToString();
    //                }
    //                string strSystemName = "";
    //                foreach (DataRow dr in dt.Rows)
    //                {
    //                    if (dr["PositionOnImport"].ToString() == i.ToString())
    //                    {

    //                        strColumnName = dr["DisplayName"].ToString();
    //                        strSystemName = dr["SystemName"].ToString();

    //                    }
    //                }
    //                if (strSystemName.ToLower() == "datetimerecorded")
    //                {
    //                    bFoundDateTime = true;
    //                    i++;

    //                    if (m == 0)
    //                    {
    //                        sw.Write("Date Recorded" + "\t" + "Time Recorded");
    //                    }
    //                    else
    //                    {
    //                        sw.Write("\t" + "Date Recorded" + "\t" + "Time Recorded");
    //                    }

    //                }
    //                else
    //                {
    //                    if (m == 0)
    //                    {
    //                        sw.Write(strColumnName);
    //                    }
    //                    else
    //                    {
    //                        sw.Write("\t" + strColumnName);
    //                    }
    //                }


    //                m = m + 1;



    //            }



    //            Response.Write(sw.ToString());
    //            Response.Flush();
    //            Response.End();


    //        }
    //    }
    //    else
    //    {
    //        int iTemp = 0;

    //        dt = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
    //             -1, false, null, null,
    //             "", null, null,
    //          "", "", null, null, ref iTemp, ref _iTotalDynamicColumns);
    //        DataSet ds = new DataSet();

    //        //OfficeManager.CreateWorkbook(dt.DataSet, Server.MapPath(""));

    //        DataTable dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import(int.Parse(_qsTableID));

    //        string strDateTimeCaption = "";
    //        for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
    //        {
    //            if (dtRecordTypleColumns.Rows[j]["SystemName"].ToString().Trim().ToLower() == "datetimerecorded")
    //            {
    //                strDateTimeCaption = dtRecordTypleColumns.Rows[j]["Name_OnImport"].ToString().Trim();
    //            }
    //        }

    //        int n = 0;

    //        foreach (DataColumn dc in dt.Columns)
    //        {
    //            if (dc.ColumnName.ToLower() != "DBGSystemRecordID" && dc.ColumnName.ToLower() != "rownum")
    //            {
    //                if (dc.ColumnName.ToLower() == strDateTimeCaption.ToLower())
    //                {
    //                    if (n == 0)
    //                    {
    //                        sw.Write("Date Recorded" + "\t" + "Time Recorded");
    //                    }
    //                    else
    //                    {
    //                        sw.Write("\t" + "Date Recorded" + "\t" + "Time Recorded");
    //                    }

    //                }
    //                else
    //                {

    //                    if (n == 0)
    //                    {

    //                        sw.Write(dc.ColumnName);
    //                    }
    //                    else
    //                    {

    //                        sw.Write("\t" + dc.ColumnName);
    //                    }
    //                }
    //                n = n + 1;
    //            }

    //        }

    //        Response.Write(sw.ToString());
    //        Response.Flush();
    //        Response.End();





    //    }




    //}

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void lnkDownloadXLSX_Click(object sender, EventArgs e)
    {
        //XLSX

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=\"" + _theTable.TableName + ".xlsx\"");
        //Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //Response.ContentType = "OCTET-STREAM";

        StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);



        //BindGridAgainToExport();

        DataTable dt;
        if (_theImportTemplate.IsImportPositional == true)
        {
            dt = GetPositionDataTable();

            if (dt.Rows.Count > 0)
            {
                string strTempPoI = dt.Rows[dt.Rows.Count - 1]["PositionOnImport"].ToString();

                int iMaxColumn = int.Parse(Common.BeforeComma(strTempPoI));
                int m = 0;
                for (int i = 1; i <= iMaxColumn; i++)
                {

                    string strColumnName = "C" + i.ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Common.BeforeComma(dr["PositionOnImport"].ToString()) == i.ToString())
                        {

                            strColumnName = dr["DisplayName"].ToString();

                        }
                    }
                    if (m == 0)
                    {
                        sw.Write(strColumnName);
                    }
                    else
                    {
                        sw.Write("\t" + strColumnName);
                    }
                    m = m + 1;


                }


                Response.Write(sw.ToString());
                //Response.BinaryWrite(EncodeString(sw.ToString(), "windows-1250"));
                Response.Flush();
                Response.End();


            }
        }
        else
        {
            int iTemp = 0;
            int? iImportTemplateID = null;

            if (ddlTemplate.SelectedValue != "")
                iImportTemplateID = int.Parse(ddlTemplate.SelectedValue);

            dt = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
                 -1, false, null, null,
                 null, null,
              "", "", null, null, ref iTemp, ref _iTotalDynamicColumns, "", iImportTemplateID, "");
            DataSet ds = new DataSet();

            //OfficeManager.CreateWorkbook(dt.DataSet, Server.MapPath(""));


            int i = 0;

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.ToLower() != "dbgsystemrecordid" && dc.ColumnName.ToLower() != "rownum")
                {
                    if (i == 0)
                    {

                        sw.Write(dc.ColumnName);
                    }
                    else
                    {

                        sw.Write("\t" + dc.ColumnName);
                    }
                    i = i + 1;
                }

            }

            Response.Write(sw.ToString());
            Response.Flush();
            Response.End();





        }



    }


    public static byte[] EncodeString(String sourceData, String charSet)
    {
        byte[] bSourceData = System.Text.Encoding.Unicode.GetBytes(sourceData);
        System.Text.Encoding outEncoding = System.Text.Encoding.GetEncoding(charSet);
        return System.Text.Encoding.Convert(System.Text.Encoding.Unicode, outEncoding, bSourceData);
    }

    protected void lnkDownloadTemplate_Click(object sender, EventArgs e)
    {
        //CSV
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=\"" + _theTable.TableName.Replace(' ','-') + ".csv\"");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        DataTable dt;

        if (_theImportTemplate==null && ddlTemplate.SelectedValue != "")
        {
            _theImportTemplate = ImportManager.dbg_ImportTemplate_Detail(int.Parse(ddlTemplate.SelectedValue));
        }

        if (_theImportTemplate.IsImportPositional == true)
        {
            //dt = GetPositionDataTable();

            //if (dt.Rows.Count > 0)
            //{
            //    //int iMaxColumn = int.Parse(dt.Rows[dt.Rows.Count - 1]["PositionOnImport"].ToString());
            //    string strTempPoI = dt.Rows[dt.Rows.Count - 1]["PositionOnImport"].ToString();

            //    int iMaxColumn = int.Parse(Common.BeforeComma(strTempPoI));
            //    for (int i = 1; i <= iMaxColumn; i++)
            //    {

            //        string strColumnName;
            //        if (strTempPoI.IndexOf(",") > 0)
            //        {
            //            strColumnName = "C" + (i + 1).ToString();
            //        }
            //        else
            //        {
            //            strColumnName = "C" + i.ToString();
            //        }

            //        string strSystemName = "";
            //        //string strIsDateSingleColumn = "false";
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            if (Common.BeforeComma(dr["PositionOnImport"].ToString()) == i.ToString())
            //            {

            //                strColumnName = dr["DisplayName"].ToString();
            //                strSystemName = dr["SystemName"].ToString();
            //                //strIsDateSingleColumn = dr["IsDateSingleColumn"].ToString().ToLower();
            //            }
            //        }

            //        if (strSystemName.ToLower() == "datetimerecorded")
            //        {

            //            if (strTempPoI.IndexOf(",") == 0)
            //            {
            //                sw.Write("Date Recorded");
            //            }
            //            else
            //            {
            //                //bFoundDateTime = true;
            //                i++;
            //                sw.Write("Date Recorded,Time Recorded");
            //            }
            //            sw.Write(",");
            //        }
            //        else
            //        {
            //            sw.Write(strColumnName);

            //            sw.Write(",");
            //        }
            //    }
            //    sw.Write(sw.NewLine);
            //}
            DataTable dtColumns = new DataTable();
            dtColumns = GetPositionalColumns();
            foreach(DataColumn drC in dtColumns.Columns)
            {
                sw.Write(drC.ColumnName);

                sw.Write(",");
            }
            sw.Write(sw.NewLine);
        }
        else
        {
            //int iTemp = 0;
            //int? iImportTemplateID = null;
            //if (ddlTemplate.SelectedValue != "")
            //    iImportTemplateID = int.Parse(ddlTemplate.SelectedValue);

            //dt = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
            //     -1, false, null, null,
            //      null, null,
            //  "", "", null, null, ref iTemp, ref _iTotalDynamicColumns, "", iImportTemplateID, "");


         

            //string strDateTimeCaption = "";
            List<string> lstColumns = new List<string>();

            lstColumns = GetListColumnsByName();
            foreach(string strC in lstColumns)
            {
                sw.Write(strC);

                sw.Write(",");
            }
            sw.Write(sw.NewLine);
            //for (int i = 0; i < dt.Columns.Count - 1; i++)
            //{

            //    if (dt.Columns[i].ColumnName != "DBGSystemRecordID")
            //    {

            //        if (dt.Columns[i].ColumnName.ToLower() == strDateTimeCaption.ToLower())
            //        {
            //            if (_strTimeSamledColumnName == "")
            //            {
            //                sw.Write(_strDateRecordedColumnName );
            //            }
            //            else
            //            {
            //                sw.Write(_strDateRecordedColumnName + "," + _strTimeSamledColumnName);
            //            }
                       

            //            sw.Write(",");
            //        }
            //        else
            //        {
            //            sw.Write(dt.Columns[i].ColumnName);

            //            sw.Write(",");
            //        }
            //    }
            //}
            //sw.Write(sw.NewLine);
        }

        

        sw.Close();

        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();


    }

    protected DataTable GetPositionalColumns()
    {
        DataTable dtColumns = new DataTable();
        DataTable dt;
        dt = GetPositionDataTable();
        if (dt.Rows.Count > 0)
        {
            string strTempPoI = dt.Rows[dt.Rows.Count - 1]["PositionOnImport"].ToString();
            int iMaxPosition = 0;
            int iMaxBeforeComma = int.Parse(Common.BeforeComma(strTempPoI.Trim()));
            int iMaxAfterComma = 0;
            foreach (DataRow dr in dt.Rows)
            {
                int iTempMax = 0;
                if (dr["PositionOnImport"].ToString().IndexOf(",") > -1)
                {
                    iTempMax = int.Parse(Common.AferComma(dr["PositionOnImport"].ToString()).Trim());
                    if (iTempMax > iMaxAfterComma)
                    {
                        iMaxAfterComma = iTempMax;
                    }
                }
            }

            iMaxPosition = iMaxBeforeComma;
            if (iMaxAfterComma > iMaxBeforeComma)
                iMaxPosition = iMaxAfterComma;

            for (int i = 1; i <= iMaxPosition; i++)
            {
                dtColumns.Columns.Add("NoImport" + i.ToString());
            }


            foreach (DataRow dr in dt.Rows)
            {
                if (dr["PositionOnImport"].ToString().IndexOf(",") > -1)
                {
                    dtColumns.Columns[int.Parse(Common.AferComma(dr["PositionOnImport"].ToString()))-1].ColumnName = dr["DisplayName"].ToString() + "_2ndPart";
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    dtColumns.Columns[int.Parse(Common.BeforeComma(dr["PositionOnImport"].ToString())) - 1].ColumnName = dr["DisplayName"].ToString();
                }
                catch
                {
                    //
                }
               
            }

            dtColumns.AcceptChanges();
            dtColumns.Rows.Add(dtColumns.NewRow());
            dtColumns.AcceptChanges();
        }
        return dtColumns;
    }


    protected void GetImportTemplate()
    {
        int? iImportTemplateID = null;
        if (ddlTemplate.SelectedValue != "")
        {
            iImportTemplateID = int.Parse(ddlTemplate.SelectedValue);
            _theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)iImportTemplateID);
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {


        lblMsg.Text = "";

      
        Guid guidNew = Guid.NewGuid();

        string strFileExtension = "";

        switch (fuRecordFile.FileName.Substring(fuRecordFile.FileName.LastIndexOf('.') + 1).ToLower())
        {

            case "txt":
                strFileExtension = ".txt";
                break;
            case "dbf":
                strFileExtension = ".dbf";
                break;
            case "csv":
                strFileExtension = ".csv";
                break;
            case "xls":
                strFileExtension = ".xls";
                break;
            case "xlsx":
                strFileExtension = ".xlsx";
                break;
            case "xml":
                strFileExtension = ".xml";
                break;           
        }







        int? iImportTemplateID = null;
        ImportTemplate theImportTemplate = null;
        if (ddlTemplate.SelectedValue != "")
        {
            iImportTemplateID = int.Parse(ddlTemplate.SelectedValue);
            theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)iImportTemplateID);
        }
           


        string strFileUniqueName;
        strFileUniqueName = guidNew.ToString() + strFileExtension;
        if (fuRecordFile.HasFile)
        {
            try
            {

                if (strFileExtension == "")
                {
                    lblMsg.Text = "Please select a .csv/.xls/.xlsx filem or .txt file or .dbf file";
                    return;
                }


                //fuRecordFile.SaveAs(Server.MapPath("../../UserFiles/AppFiles") + "\\" + strFileUniqueName);

                fuRecordFile.SaveAs( _strFilesPhisicalPath + "\\UserFiles\\AppFiles" + "\\" + strFileUniqueName);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
                return;
            }
        }
        else
        {
            lblMsg.Text = "You have not specified a file.";
            return;
        }

        //check if it has multiple sheets
        string strSelectedSheet = "";

        if (strFileExtension == ".xls" || strFileExtension == ".xlsx")
        {

            List<string> lstSheets = OfficeManager.GetExcelSheetNames(_strFilesPhisicalPath + "\\UserFiles\\AppFiles", strFileUniqueName);

            if (lstSheets.Count > 1)
            {
                int iSCID = GetFileInforSC(guidNew, strFileExtension);
                if (Request.QueryString["SearchCriteriaID"] != null)
                {
                    Response.Redirect("~/Pages/Record/MultipleSheetsUpload.aspx?TableID=" + Request.QueryString["TableID"] + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&FileInfo=" + Cryptography.Encrypt(iSCID.ToString()), false);
                }
                else
                {
                    Response.Redirect("~/Pages/Record/MultipleSheetsUpload.aspx?TableID=" + Request.QueryString["TableID"] + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&FileInfo=" + Cryptography.Encrypt(iSCID.ToString()), false);
                }
                return;
            }
            else
            {
                //Single

            }
        }

        /////

        string strMsg = "";
        int iBatchID = -1;
        //SqlTransaction tn;
        //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

        //connection.Open();
        //tn = connection.BeginTransaction();

        try
        {



            UploadManager.UploadCSV(_ObjUser.UserID, _theTable, txtBatchDescription.Text,
                    fuRecordFile.FileName, guidNew, _strFilesPhisicalPath + "\\UserFiles\\AppFiles",
                   out strMsg, out iBatchID,  strFileExtension, strSelectedSheet,
                   int.Parse(Session["AccountID"].ToString()), null, iImportTemplateID, null);
          

            if (strMsg == "")
            {
               
            }
            else
            {              
                lblMsg.Text = strMsg;
                //connection.Close();
                //connection.Dispose();
                return;

            }

            string strVirtualUsersTableID = SystemData.SystemOption_ValueByKey_Account("VirtualUsersTableID", int.Parse(Session["AccountID"].ToString()), _theTable.TableID);

            if (iBatchID != -1 && strVirtualUsersTableID != "" && strVirtualUsersTableID == _theTable.TableID.ToString())
            {
                SecurityManager.dbg_User_Validation(iBatchID);
            }

            string strExtra = "";
            //if (iImportTemplateID != null)
            //    strExtra = "&ImportTemplateID=" + Cryptography.Encrypt(iImportTemplateID.ToString());


            //connection.Close();
            //connection.Dispose();

            if (Request.QueryString["SearchCriteriaID"]!=null)
            {
                Response.Redirect("~/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=" + Cryptography.Encrypt(iBatchID.ToString()) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + strExtra, false);
            }
            else
            {
                Response.Redirect("~/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=" + Cryptography.Encrypt(iBatchID.ToString()) + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + strExtra, false);
            }
        }
        catch (Exception ex)
        {

            //tn.Rollback();
            //if (connection.State==ConnectionState.Open)
            //{
            //    connection.Close();
            //    connection.Dispose();
            //}
            

            lblMsg.Text = strMsg;

            ErrorLog theErrorLog = new ErrorLog(null, "Record Upload", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            //throw;
        }


    }

    protected void ddlTemplate_PreRender(object sender, EventArgs e)
    {
        DataTable dtTemp = Common.DataTableFromText("SELECT ImportTemplateID,ImportTemplateName,HelpText FROM ImportTemplate WHERE TableID=" + _qsTableID + " ORDER BY ImportTemplateName");

        foreach (DataRow dr in dtTemp.Rows)
        {
            if (dr["HelpText"] != DBNull.Value && dr["HelpText"].ToString() != "")
            {
                foreach (ListItem liItem in ddlTemplate.Items)
                {
                    if(liItem.Value==dr["ImportTemplateID"].ToString())
                    {
                        liItem.Attributes.Add("title", dr["HelpText"].ToString());
                        break;
                    }
                }
            }
           

        }

    }

    protected void PopulateImportTemplate(int iTableID)
    {
        ddlTemplate.Items.Clear();
        //ddlTemplate.DataSource = Common.DataTableFromText("SELECT * FROM ImportTemplate WHERE TableID=" + iTableID.ToString() + " ORDER BY ImportTemplateName");
        //ddlTemplate.DataBind();

        DataTable dtTemp = Common.DataTableFromText("SELECT  ImportTemplateID,ImportTemplateName,HelpText  FROM ImportTemplate WHERE TableID=" 
            + iTableID.ToString() + " ORDER BY ImportTemplateName");

        foreach (DataRow dr in dtTemp.Rows)
        {
            ListItem liTemp = new ListItem(dr["ImportTemplateName"].ToString(), dr["ImportTemplateID"].ToString());
            //if (dr["HelpText"] != DBNull.Value && dr["HelpText"].ToString() != "")
            //    liTemp.Attributes.Add("title", dr["HelpText"].ToString());
            ddlTemplate.Items.Add(liTemp);
        }

        if(!IsPostBack)
        {
            if (ddlTemplate.Items.Count == 0)
            {
                //no template, so lets create one

                ImportManager.CreateDefaultImportTemplate(iTableID,"");

                PopulateImportTemplate(iTableID);

              


            }
        }

        ListItem liSelect = new ListItem("--Please select--", "");
        //liSelect.Attributes.Add("title", "This will be using column name on import.");
        ddlTemplate.Items.Insert(0, liSelect);

        if(!IsPostBack)
        {
            int? iDT = ImportManager.GetDefaultImportTemplate((int)iTableID);
            if(iDT!=null)
            {
                if(ddlTemplate.Items.FindByValue(iDT.ToString())!=null)
                {
                    ddlTemplate.SelectedValue = iDT.ToString();
                }
            }
        }
       // ddlTemplate_SelectedIndexChanged(null, null);
    }

    protected void BindGrids()
    {
        int? iImportTemplateID = null;
        if (ddlTemplate.SelectedValue != "")
            iImportTemplateID = int.Parse(ddlTemplate.SelectedValue);

        ImportTemplate theImprtTemplate = ImportManager.dbg_ImportTemplate_Detail((int)iImportTemplateID);

        int iColumnCount = 0;
        int iTemp = 0;
        if (theImprtTemplate.IsImportPositional != null && (bool)theImprtTemplate.IsImportPositional == true)
        {
            //divColumnName.Visible = false;

            //lblImortType.Text = "Following columns will be imported from the file. Column positions must match.";

            //DataTable dt = Common.DataTableFromText("SELECT SystemName, DisplayName , PositionOnImport,IsDateSingleColumn  " +
            //     " FROM [Column] WHERE  TableID = " + _theTable.TableID.ToString() +
            //                                                    " AND PositionOnImport IS NOT NULL" +
            //                                                    " ORDER BY PositionOnImport");
            //iColumnCount = dt.Rows.Count;
            //DataTable dtNew = new DataTable();
            //dtNew.Columns.Add("DisplayName");
            //dtNew.Columns.Add("PositionOnImport");

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
            //    {
            //        int iPosiion = int.Parse(dt.Rows[i]["PositionOnImport"].ToString());
            //        dtNew.Rows.Add("Date Recorded", dt.Rows[i]["PositionOnImport"].ToString());

            //        if (dt.Rows[i]["IsDateSingleColumn"].ToString().ToLower() == "true")
            //        {
            //        }
            //        else
            //        {
            //            dtNew.Rows.Add("Time Recorded", (iPosiion + 1).ToString());
            //        }
            //    }
            //    else
            //    {
            //        dtNew.Rows.Add(dt.Rows[i]["DisplayName"].ToString(), dt.Rows[i]["PositionOnImport"].ToString());
            //    }
            //}

            //gvPosition.DataSource = dtNew;
            //gvPosition.VirtualItemCount = dtNew.Rows.Count;
            //gvPosition.DataBind();

            //if (iColumnCount == 0)
            //{
            //    lnkNext.Visible = false;
            //    lblMsg.Text = "Data can not be imported because no fields have been selected for import";
            //}
            BindPositionGrid();
        }
        else
        {
            BindTheColumnGrid();
        }
    }
    protected void BindTheColumnGrid()
    {
        divColumnPosition.Visible = false;
        divColumnName.Visible = true;
        lblImortType.Text = "Following columns will be imported from the file. Column names must match.";

        int iColumnCount = 0;
        int iTemp = 0;

        int? iImportTemplateID = null;

        if (ddlTemplate.SelectedValue != "")
            iImportTemplateID = int.Parse(ddlTemplate.SelectedValue);
        lblImportTemplateFile.Text = "";
        if(iImportTemplateID!=null)
        {
            ImportTemplate theImprtTemplate = ImportManager.dbg_ImportTemplate_Detail((int)iImportTemplateID);
            if(theImprtTemplate!=null && theImprtTemplate.TemplateUniqueFileName!="")
            {
                string strFilePath = Cryptography.Encrypt(_strFilesLocation + "/UserFiles/AppFiles/" + theImprtTemplate.TemplateUniqueFileName);
                string strFileName = theImprtTemplate.TemplateUniqueFileName.Substring(37);

                lblImportTemplateFile.Text = "<a target='_blank' href='" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Filedownload.aspx?FilePath="
                       + strFilePath + "&FileName=" + Cryptography.Encrypt(strFileName) + "'>" +
                         strFileName + "</a>";
            }
        }

       // DataTable dt = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
       //   -1, false, null, null,
       //    null, null,
       //"", "", null, null, ref iTemp, ref _iTotalDynamicColumns, "", iImportTemplateID, "");
        //DataRow dr;
        //dr=dt.NewRow();

        //DataTable dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import(int.Parse(_qsTableID), iImportTemplateID);

        //iColumnCount = dtRecordTypleColumns.Rows.Count;

        //string strDateTimeCaption = "";
        //for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
        //{
        //    if (dtRecordTypleColumns.Rows[j]["SystemName"].ToString().Trim().ToLower() == "datetimerecorded")
        //    {
        //        strDateTimeCaption = dtRecordTypleColumns.Rows[j]["ImportHeaderName"].ToString().Trim();
        //    }
        //}
        gvTheGrid.Attributes.Add("bordercolor", "D1E9FF");
        //gvTheGrid.Attributes.Add("borderwidth", "10");



        //int iDateIndex = -1;
        //for (int i = 0; i < dt.Columns.Count; i++)
        //{

        //    if (dt.Columns[i].ColumnName.ToLower() == strDateTimeCaption.ToLower())
        //    {

        //        iDateIndex = i;
        //    }
        //}



        DataTable dtNew = new DataTable();


        //for (int i = 0; i < dt.Columns.Count; i++)
        //{

        //    if (i == iDateIndex)
        //    {
        //        dtNew.Columns.Add(_strDateRecordedColumnName);


        //        if (_strTimeSamledColumnName == "")
        //        {

        //        }
        //        else
        //        {
        //            dtNew.Columns.Add(_strTimeSamledColumnName);
        //            _iTotalDynamicColumns = _iTotalDynamicColumns + 1;
        //        }


        //    }
        //    else
        //    {
        //        dtNew.Columns.Add(dt.Columns[i].ColumnName);
        //    }




        //}

        //for (int i = 0; i < dtNew.Columns.Count; i++)
        //{
        //    if (dtNew.Columns[i].ColumnName == "DBGSystemRecordID")
        //    {
        //        _iValidRecordIDIndex = i;
        //        break;
        //    }

        //}



        //dtNew.Rows.Add(dtNew.NewRow());
        List<string> lstColumns = new List<string>();

        //for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
        //{
        //    string strBeforeComma = dtRecordTypleColumns.Rows[j]["ImportHeaderName"].ToString();
        //    string strAfterComma = "";
        //    if (strBeforeComma.ToString().Trim().IndexOf(",") == -1)
        //    {
        //        //
        //    }
        //    else
        //    {
        //        strAfterComma = Common.AferComma(strBeforeComma);
        //        strBeforeComma = Common.BeforeComma(strBeforeComma);
        //    }

        //    if (strBeforeComma != "" && lstColumns.Contains(strBeforeComma) == false)
        //    {
        //        lstColumns.Add(strBeforeComma);
        //    }

        //    if (strAfterComma != "" && lstColumns.Contains(strAfterComma) == false)
        //    {
        //        lstColumns.Add(strAfterComma);
        //    }
        //}
        lstColumns = GetListColumnsByName();
        foreach (string strC in lstColumns)
        {
            dtNew.Columns.Add(strC);
        }
        dtNew.AcceptChanges();
        dtNew.Rows.Add(dtNew.NewRow());
        iColumnCount = dtNew.Columns.Count;
        dtNew.AcceptChanges();
        gvTheGrid.DataSource = dtNew;
        gvTheGrid.VirtualItemCount = 1;
        gvTheGrid.DataBind();

        if (iColumnCount == 0)
        {
            lnkNext.Visible = false;
            lblMsg.Text = "Data can not be imported because no fields have been selected for import";
        }
        else
        {
            lnkNext.Visible = true;
        }
    }
    protected void BindPositionGrid()
    {
        int iColumnCount = 0;
        int iTemp = 0;
        divColumnName.Visible = false;
        divColumnPosition.Visible = true;
        lblImortType.Text = "Following columns will be imported from the file. Column positions must match.";

        DataTable dt = GetPositionDataTable(); 

        iColumnCount = dt.Rows.Count;
        //DataTable dtNew = new DataTable();
        //dtNew.Columns.Add("DisplayName");
        //dtNew.Columns.Add("PositionOnImport");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strTempPoI = dt.Rows[i]["PositionOnImport"].ToString();
            //if (dt.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
            //{

            //    if (strTempPoI.IndexOf(",") > 0)
            //    {
            //        dtNew.Rows.Add("Date Recorded",Common.BeforeComma( strTempPoI));
            //        dtNew.Rows.Add("Time Recorded", Common.AferComma(strTempPoI));
            //    }
            //    else
            //    {
            //        dtNew.Rows.Add("Date Recorded", strTempPoI);
            //    }
                              
            //}
            //else
            //{
            //dt.Rows.Add(dt.Rows[i]["DisplayName"].ToString(), strTempPoI);
            //}
        }

        gvPosition.DataSource = dt;//dtNew
        gvPosition.VirtualItemCount = dt.Rows.Count;
        gvPosition.DataBind();

        if (iColumnCount == 0)
        {
            lnkNext.Visible = false;
            lblMsg.Text = "Data can not be imported because no fields have been selected for import";
        }
    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {

        //BindTheColumnGrid();
        BindGrids();
    }

    protected int GetFileInforSC(Guid guidNew, string strFileExtension)
    {

        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <txtBatchDescription>" + HttpUtility.HtmlEncode(txtBatchDescription.Text) + "</txtBatchDescription>" +
                   " <FileName>" + HttpUtility.HtmlEncode(fuRecordFile.FileName) + "</FileName>" +
                   " <guidNew>" + HttpUtility.HtmlEncode(guidNew.ToString()) + "</guidNew>" +
                    " <ddlTemplate>" + HttpUtility.HtmlEncode(ddlTemplate.SelectedValue) + "</ddlTemplate>" +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            return SystemData.SearchCriteria_Insert(theSearchCriteria);
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
        return -1;
        //End Searchcriteria

    }



    //protected void PopulateLocationDDL()
    //{
    //    int iTN = 0;
    //    ddlLocation.DataSource = SiteManager.ets_Location_Select(null, int.Parse(_qsTableID), null,
    //            string.Empty, string.Empty,  true, null, null, null, null,
    //            int.Parse(Session["AccountID"].ToString()),
    //            "LocationName", "ASC",
    //            null, null, ref  iTN, "");

    //    ddlLocation.DataBind();
    //    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "-1");
    //    ddlLocation.Items.Insert(0, liSelect);


    //}





//    public static void UploadCSV(User objUser, Table theTable, string strBatchDescription,
//  string strOriginalFileName, Guid guidNew, string strImportFolder, string strLocationID,
//  string strLocation, out string strMsg, out int iBatchID, ref SqlConnection connection, ref SqlTransaction tn, string strFileExtension, string strSelectedSheet)
//    {
//        string strTemp = "";
//        string strFileUniqueName = guidNew.ToString() + strFileExtension;
//        strMsg = "";
//        int z = 0;
//        iBatchID = -1;

//        string strDateRecordedColumnName = "Date Recorded";
//        string strTimeSamledColumnName = "Time Recorded";

//        if (strLocationID == "-1")
//        {
//            DataTable dtSS = Common.DataTableFromText(@"SELECT     Location.LocationID
//                FROM         Location INNER JOIN
//                                      LocationTable ON Location.LocationID = LocationTable.LocationID
//                WHERE Location.IsActive=1 AND  LocationTable.TableID=" + theTable.TableID.ToString(), ref connection, ref tn);
//            if (dtSS.Rows.Count > 0)
//            {


//            }
//            else
//            {
//                //no Location, so create one

//                try
//                {

//                    DataTable dtSS2 = Common.DataTableFromText(@"SELECT     Location.LocationID
//                                FROM         Location  
//                                WHERE Location.IsActive=1 AND LocationName='Location 1'
//                                AND AccountID=" + theTable.AccountID.ToString(), ref connection, ref tn);

//                    int iLocationID = -1;
//                    if (dtSS2.Rows.Count > 0)
//                    {
//                        iLocationID = int.Parse(dtSS2.Rows[0][0].ToString());
//                    }
//                    else
//                    {

//                        Location newLocation = new Location(null, "Location 1", null, "", "Created at the time of import", true, null, null, null, null, (int)theTable.AccountID, "", "");
//                        iLocationID = SiteManager.ets_Location_Insert(newLocation, tn, null);
//                    }



//                    LocationTable newLocationTable = new LocationTable(null, iLocationID, (int)theTable.TableID, "", "");
//                    SiteManager.ets_LocationTable_Insert(newLocationTable, tn, null);

//                    strLocationID = iLocationID.ToString();
//                }
//                catch
//                {
//                    //
//                }

//            }

//        }



//        if (theTable.IsImportPositional == false)
//        {

//            try
//            {

//                strMsg = "";


//                //lets get date and time column name

//                DataTable dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      Name_OnImport FROM  Column
//                        WHERE SystemName='DateTimeRecorded' AND TableID=" + theTable.TableID.ToString(), ref connection, ref tn);

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

//                string strImportHeaderName = "Name_OnImport";

//                dtImportFileTable = null;

//                switch (strFileExtension.ToLower())
//                {

//                    case ".csv":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName, ref strMsg);
//                        z = 1;
//                        break;
//                    case ".xls":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet);
//                        break;
//                    case ".xlsx":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet);
//                        break;

//                    case ".xml":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromXML(strImportFolder, strFileUniqueName);
//                        strImportHeaderName = "DisplayName";

//                        break;

//                }

//                if (strMsg != "")
//                {
                    

//                }
//                //PERFORM CLIENT Specific treatment

//                if (theTable.ImportDataStartRow != null)
//                {
//                    for (int i = 1; i <= (int)theTable.ImportDataStartRow; i++)
//                    {
//                        dtImportFileTable.Rows.RemoveAt(0);

//                    }
//                    dtImportFileTable.AcceptChanges();

//                }





//                DataTable dtRecordTypleColumns;
//                string strListOfNoNeedColumns = "";


//                //dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();

//                dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare(field.ToString().Trim(), string.Empty) == 0)).CopyToDataTable();



//                if (strImportHeaderName == "DisplayName")
//                {
//                    dtRecordTypleColumns = RecordManager.ets_Table_Columns_DisplayName((int)theTable.TableID, ref connection, ref tn);
//                }
//                else
//                {
//                    dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, ref connection, ref tn);
//                }



//                for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
//                {
//                    bool bIsFound = false;
//                    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                    {
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                            Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strImportHeaderName].ToString().Trim().ToLower()))
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

//                List<string> strRemoveIndexes = strListOfNoNeedColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();


//                foreach (string item in strRemoveIndexes)
//                {
//                    dtImportFileTable.Columns.Remove(item);
//                }


//                for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
//                {
//                    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                    {
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                            Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strImportHeaderName].ToString().Trim().ToLower()))
//                        {
//                            dtImportFileTable.Columns[r].ColumnName = dtRecordTypleColumns.Rows[i]["SystemName"].ToString();
//                            break;
//                        }
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                          strDateRecordedColumnName.ToLower())
//                        {
//                            dtImportFileTable.Columns[r].ColumnName = "DateTimeRecorded";
//                            break;
//                        }
//                    }

//                }







//                //now dtCSV is ready to be imported into Batch & TempRecord
//                Batch newBatch = new Batch(null, (int)theTable.TableID,
//                    strBatchDescription.Trim() == "" ? strOriginalFileName : strBatchDescription.Trim(),
//                    strOriginalFileName, null, guidNew, objUser.UserID, objUser.AccountID, theTable.IsImportPositional);

//                //need a single transaction

//                iBatchID = UploadManager.ets_Batch_Insert(newBatch, ref connection, ref tn);


//                //AddMissingLocation
//                if (theTable.AddMissingLocation != null && (bool)theTable.AddMissingLocation)
//                {
//                    //lets find Location column
//                    bool bFoundSS = false;
//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        if (dc.ColumnName.ToUpper() == "LocationID")
//                        {
//                            bFoundSS = true;
//                        }

//                    }

//                    if (bFoundSS)
//                    {
//                        for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//                        {
//                            string strSSName = dtImportFileTable.Rows[r]["LocationID"].ToString();
//                            Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, strSSName, ref connection, ref tn);

//                            if (tempLocation == null)
//                            {
//                                Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, strSSName, ref connection, ref tn);
//                                {
//                                    if (tempLocationImp == null)
//                                    {
//                                        //ops we have not found it so lets insert it

//                                        try
//                                        {
//                                            Location newLocation = new Location(null, strSSName, null, "", "", true, null, null, null, null,
//                                                (int)theTable.AccountID, "", "");

//                                            int iNewLocationID = SiteManager.ets_Location_Insert(newLocation, tn, null);

//                                            LocationTable newLocationTable=new LocationTable(null,iNewLocationID,(int)theTable.TableID,"","");
//                                            SiteManager.ets_LocationTable_Insert(newLocationTable,tn,null);
//                                        }
//                                        catch
//                                        {
//                                            //do nothing
//                                        }
//                                    }                                   
//                                }
//                            }

//                        }

//                    }
                   

//                }//end AddMissingLocation





//                for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//                {
//                    TempRecord newTempRecord = new TempRecord();
//                    newTempRecord.AccountID = objUser.AccountID;
//                    newTempRecord.BatchID = iBatchID;
//                    //bool bIsBlank = false;
//                    string strRejectReason = "";
//                    string strWarningReason = "";

//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        string strColumnName = "";
//                        strColumnName = dc.ColumnName;

//                        if (strColumnName.ToLower() != strTimeSamledColumnName.ToLower())
//                        {

//                            if (dc.ColumnName.ToUpper() == "LocationID")
//                            {
//                                strColumnName = "LocationName";
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERecorded")
//                            {
//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() == "")
//                                {
//                                    strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    //bIsBlank = true;
//                                }
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERecorded")
//                            {
//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                {

//                                    try
//                                    {
//                                        if (strFileExtension == ".csv")
//                                        {
//                                            if (strTimeSamledColumnName == "")
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                            }
//                                            else
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() + " " + dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString());
//                                            }

//                                        }
//                                        else if (strFileExtension == ".xml")
//                                        {
//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                        }
//                                        else
//                                        {
//                                            string strDateTimeTemp = "";
//                                            if (strTimeSamledColumnName == "")
//                                            {
//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                                {
//                                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0));
//                                                }
//                                            }
//                                            else
//                                            {
//                                                strDateTimeTemp = dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString();

//                                                if (dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString().Length > 10)
//                                                {
//                                                    strDateTimeTemp = strDateTimeTemp.Substring(11);
//                                                }
//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                                {
//                                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 9)
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, 10) + " " + strDateTimeTemp);
//                                                    }
//                                                    else
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0) + " " + strDateTimeTemp);
//                                                    }
//                                                    //
//                                                }
//                                            }
//                                        }
//                                    }
//                                    catch
//                                    {

//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                        //bIsBlank = true;
//                                    }
//                                }
//                                else
//                                {
//                                    if (strRejectReason == "")
//                                    {
//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    }
//                                    else
//                                    {
//                                        if (strRejectReason.IndexOf(strDateRecordedColumnName) > -1)
//                                        {
//                                        }
//                                        else
//                                        {
//                                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                        }
//                                    }
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
//                                            dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());

//                                            dtImportFileTable.AcceptChanges();
//                                        }

//                                    }



//                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 0)
//                                    {





//                                        if (dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString() != "")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                            {
//                                                if (!UploadManager.IsDataIntoDropDown(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString()))
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                                }

//                                            }

//                                        }


//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
//                                                {

//                                                }
//                                                else
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                                }
//                                            }

//                                        }

//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
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
//                                            int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

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

//                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

//                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

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
//                                    else
//                                    {

//                                        if (dtRecordTypleColumns.Rows[i]["IsMandatory"].ToString().ToLower() == "true")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
//                                            {
//                                                strRejectReason = strRejectReason + " REQUIRED:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                            }

//                                        }

//                                    }
//                                    break;
//                                }
//                            }
//                        }
//                    }



//                    newTempRecord.TableID = (int)theTable.TableID;

//                    if (strLocationID == "-1")
//                    {
//                        if (newTempRecord.LocationID == null)
//                        {
//                            if (newTempRecord.LocationName == null)
//                            {
//                                //ops we have not found it
//                                strRejectReason = strRejectReason + " Location not found.";
//                            }
//                            else
//                            {

//                                if (newTempRecord.LocationName.Length > 0)
//                                {

//                                    Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);

//                                    if (tempLocation == null)
//                                    {
//                                        Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);
//                                        {
//                                            if (tempLocationImp == null)
//                                            {
//                                                //ops we have not found it
//                                                strRejectReason = strRejectReason + " Location -" + newTempRecord.LocationName + " - is not found!";
//                                            }
//                                            else
//                                            {
//                                                newTempRecord.LocationID = tempLocationImp.LocationID;
//                                                newTempRecord.LocationName = tempLocationImp.LocationName;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        newTempRecord.LocationID = tempLocation.LocationID;
//                                        newTempRecord.LocationName = tempLocation.LocationName;
//                                    }

//                                }
//                                else
//                                {
//                                    strRejectReason = strRejectReason + " Location not found.";
//                                }
//                            }
//                        }
//                        else
//                        {
//                            Location tempLocation = SiteManager.ets_Location_Details((int)newTempRecord.LocationID, ref connection, ref tn);

//                            if (tempLocation == null)
//                            {
//                                strRejectReason = strRejectReason + " Location not found.";
//                            }
//                            else
//                            {
//                                newTempRecord.LocationID = tempLocation.LocationID;
//                                newTempRecord.LocationName = tempLocation.LocationName;
//                            }
//                        }

//                    }
//                    else
//                    {
//                        newTempRecord.LocationID = int.Parse(strLocationID);
//                        newTempRecord.LocationName = strLocation;
//                    }

//                    if (newTempRecord.DateTimeRecorded == null)
//                    {
//                        if (strRejectReason == "")
//                        {
//                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                        }
//                        else
//                        {
//                            if (strRejectReason.IndexOf(strDateRecordedColumnName) > -1)
//                            {
//                            }
//                            else
//                            {
//                                strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                            }
//                        }
//                    }


//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
//                        {

//                            if (dc.ColumnName.ToLower() ==
//                                   dtRecordTypleColumns.Rows[j]["SystemName"].ToString().Trim().ToLower())
//                            {
//                                //check sensor information
//                                if (newTempRecord.LocationID != null)
//                                {
//                                    if (!bool.Parse(dtRecordTypleColumns.Rows[j]["IsStandard"].ToString()))
//                                    {
//                                        if (dtRecordTypleColumns.Rows[j]["DropdownValues"].ToString() == "")
//                                        {
//                                            string strLinkMsg = "";
//                                            int iSensonIDTemp = -1;
//                                            bool bIsSensorOK = InstrumentManager.IsSensorOk((int)newTempRecord.LocationID, int.Parse(dtRecordTypleColumns.Rows[j]["ColumnID"].ToString()), newTempRecord.DateTimeRecorded,
//                                                dtImportFileTable.Rows[r][dc.ColumnName].ToString(), ref strLinkMsg, ref iSensonIDTemp, ref connection, ref tn);
//                                            if (bIsSensorOK == false)
//                                            {
//                                                strWarningReason = strWarningReason + " SENSOR WARNING: " + dtRecordTypleColumns.Rows[j]["DisplayName"].ToString() + " - " + strLinkMsg;

//                                            }
//                                        }
//                                    }
//                                }

//                            }
//                        }

//                    }

//                    strRejectReason = strRejectReason.Trim();
//                    strWarningReason = strWarningReason.Trim();


//                    if (strRejectReason == "")
//                    {
//                        if (newTempRecord.LocationID != null)
//                        {
//                            if (RecordManager.ets_Record_IsDuplicate(newTempRecord, ref connection, ref tn))
//                            {
//                                strRejectReason = strRejectReason + " DUPLICATE Record!";
//                            }
//                        }
//                    }


//                    //                    

//                    if (strRejectReason.Length > 0)
//                    {
//                        newTempRecord.RejectReason = strRejectReason.Trim();
//                    }
//                    if (strWarningReason.Length > 0)
//                    {
//                        newTempRecord.WarningReason = strWarningReason.Trim();
//                    }



//                    int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord, ref connection, ref tn);


//                }

//                try
//                {
//                    if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
//                        File.Delete(strImportFolder + "\\" + strFileUniqueName);
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
//            try
//            {

//                strMsg = "";

//                bool bIsDateSingleColumn = false;
//                int iDatePosition = 1;
//                DataTable dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      IsDateSingleColumn,PositionOnImport FROM  Column
//                        WHERE SystemName='DateTimeRecorded' AND TableID=" + theTable.TableID.ToString(), ref connection, ref tn);
//                if (dtDateTimeColumnName.Rows.Count > 0 && dtDateTimeColumnName.Rows[0][0] != DBNull.Value)
//                {

//                    if (dtDateTimeColumnName.Rows[0]["IsDateSingleColumn"].ToString().ToLower() == "true")
//                    {
//                        bIsDateSingleColumn = true;
//                    }
//                    iDatePosition = int.Parse(dtDateTimeColumnName.Rows[0]["PositionOnImport"].ToString());
//                }


//                DataTable dtImportFileTable;
//                dtImportFileTable = null;

//                switch (strFileExtension.ToLower())
//                {

//                    case ".csv":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName);
//                        z = 1;
//                        break;
//                    case ".xls":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet);
//                        break;
//                    case ".xlsx":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet);
//                        break;

//                }

//                //Remove rows

//                if (theTable.ImportDataStartRow != null)
//                {
//                    for (int i = 1; i <= (int)theTable.ImportDataStartRow; i++)
//                    {
//                        dtImportFileTable.Rows.RemoveAt(0);

//                    }
//                    dtImportFileTable.AcceptChanges();

//                }






//                DataTable dtRecordTypleColumns = RecordManager.ets_Table_Import_Position((int)theTable.TableID, ref connection, ref tn);


//                string strListOfNoNeedColumns = "";
//                for (int i = 0; i < dtImportFileTable.Columns.Count; i++)
//                {
//                    bool bIsFound = false;
//                    for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
//                    {
//                        if (i == (int.Parse(dtRecordTypleColumns.Rows[j]["PositionOnImport"].ToString()) - 1))
//                        {
//                            bIsFound = true;
//                            dtImportFileTable.Columns[i].ColumnName = dtRecordTypleColumns.Rows[j]["SystemName"].ToString();
//                            break;
//                        }
//                    }
//                    if (bIsFound == false)
//                    {

//                        for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
//                        {
//                            if (i == (iDatePosition + 1 - 1))
//                            {
//                                bIsFound = true;
//                                dtImportFileTable.Columns[i].ColumnName = "time Recorded";
//                                break;
//                            }
//                        }

//                        if (dtImportFileTable.Columns[i].ColumnName.ToLower() != "time Recorded")
//                        {
//                            strListOfNoNeedColumns += dtImportFileTable.Columns[i].ColumnName + ",";
//                        }
//                    }
//                }

//                List<string> strRemoveIndexes = strListOfNoNeedColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();


//                foreach (string item in strRemoveIndexes)
//                {
//                    dtImportFileTable.Columns.Remove(item);
//                }







//                //now dtCSV is ready to be imported into Batch & TempRecord
//                Batch newBatch = new Batch(null, (int)theTable.TableID,
//                    strBatchDescription.Trim() == "" ? strOriginalFileName : strBatchDescription.Trim(),
//                    strOriginalFileName, null, guidNew, objUser.UserID, objUser.AccountID, theTable.IsImportPositional);





//                iBatchID = UploadManager.ets_Batch_Insert(newBatch, ref connection, ref tn);





//                //AddMissingLocation
//                if (theTable.AddMissingLocation != null && (bool)theTable.AddMissingLocation)
//                {
//                    //lets find Location column
//                    bool bFoundSS = false;
//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        if (dc.ColumnName.ToUpper() == "LocationID")
//                        {
//                            bFoundSS = true;
//                        }

//                    }

//                    if (bFoundSS)
//                    {
//                        for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//                        {
//                            string strSSName = dtImportFileTable.Rows[r]["LocationID"].ToString();
//                            Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, strSSName, ref connection, ref tn);

//                            if (tempLocation == null)
//                            {
//                                Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, strSSName, ref connection, ref tn);
//                                {
//                                    if (tempLocationImp == null)
//                                    {
//                                        //ops we have not found it so lets insert it

//                                        try
//                                        {
//                                            Location newLocation = new Location(null, strSSName, null, "", "", true, null, null, null, null,
//                                                (int)theTable.AccountID, "", "");

//                                            int iNewLocationID = SiteManager.ets_Location_Insert(newLocation, tn, null);

//                                            LocationTable newLocationTable = new LocationTable(null, iNewLocationID, (int)theTable.TableID, "", "");
//                                            SiteManager.ets_LocationTable_Insert(newLocationTable, tn, null);
//                                        }
//                                        catch
//                                        {
//                                            //do nothing
//                                        }
//                                    }
//                                }
//                            }

//                        }

//                    }


//                }//end AddMissingLocation







//                for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//                {
//                    TempRecord newTempRecord = new TempRecord();
//                    newTempRecord.AccountID = objUser.AccountID;
//                    newTempRecord.BatchID = iBatchID;
//                    //bool bIsBlank = false;
//                    string strRejectReason = "";
//                    string strWarningReason = "";

//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        string strColumnName = "";
//                        strColumnName = dc.ColumnName;
//                        if (strColumnName.ToLower() != "time Recorded")
//                        {
//                            if (dc.ColumnName.ToUpper() == "LocationID")
//                            {
//                                strColumnName = "LocationName";
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERecorded")
//                            {
//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
//                                {
//                                    strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    //bIsBlank = true;
//                                }
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERecorded")
//                            {
//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                {
//                                    try
//                                    {
//                                        if (strFileExtension == ".csv")
//                                        {
//                                            if (bIsDateSingleColumn)
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                            }
//                                            else
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() + " " + dtImportFileTable.Rows[r]["Time Recorded"].ToString());
//                                            }
//                                        }
//                                        else
//                                        {
//                                            if (bIsDateSingleColumn)
//                                            {

//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                                {
//                                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                                }

//                                            }
//                                            else
//                                            {
//                                                string strDateTimeTemp = dtImportFileTable.Rows[r]["Time Recorded"].ToString();

//                                                if (dtImportFileTable.Rows[r]["Time Recorded"].ToString().Length > 10)
//                                                {
//                                                    strDateTimeTemp = strDateTimeTemp.Substring(11);
//                                                }
//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                                {

//                                                    //UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, 10) + " " + strDateTimeTemp);

//                                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 9)
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, 10) + " " + strDateTimeTemp);
//                                                    }
//                                                    else
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0) + " " + strDateTimeTemp);
//                                                    }
//                                                }
//                                            }

//                                        }
//                                    }
//                                    catch
//                                    {
//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                        //bIsBlank = true;
//                                    }
//                                }
//                                else
//                                {
//                                    if (strRejectReason == "")
//                                    {
//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    }
//                                    else
//                                    {
//                                        if (strRejectReason.IndexOf("Date Recorded") > -1)
//                                        {
//                                        }
//                                        else
//                                        {
//                                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                        }
//                                    }

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
//                                            dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());

//                                            dtImportFileTable.AcceptChanges();
//                                        }

//                                    }




//                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 0)
//                                    {

//                                        //Not Empty

//                                        if (dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString() != "")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                            {
//                                                if (!UploadManager.IsDataIntoDropDown(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString()))
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                                }

//                                            }

//                                        }



//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
//                                                {

//                                                }
//                                                else
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                                }
//                                            }

//                                        }

//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
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
//                                            int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

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

//                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

//                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

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
//                                    else
//                                    {
//                                        if (dtRecordTypleColumns.Rows[i]["IsMandatory"].ToString().ToLower() == "true")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
//                                            {
//                                                strRejectReason = strRejectReason + " REQUIRED:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                            }

//                                        }


//                                    }
//                                    break;
//                                }
//                            }
//                        }
//                    }

//                    newTempRecord.TableID = (int)theTable.TableID;

//                    if (strLocationID == "-1")
//                    {
//                        if (newTempRecord.LocationID == null)
//                        {
//                            if (newTempRecord.LocationName == null)
//                            {
//                                //ops we have not found it
//                                strRejectReason = strRejectReason + " Location not found.";
//                            }
//                            else
//                            {

//                                if (newTempRecord.LocationName.Length > 0)
//                                {

//                                    Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);

//                                    if (tempLocation == null)
//                                    {
//                                        Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);
//                                        {
//                                            if (tempLocationImp == null)
//                                            {
//                                                //ops we have not found it
//                                                strRejectReason = strRejectReason + " Location -" + newTempRecord.LocationName + " - is not found!";
//                                            }
//                                            else
//                                            {
//                                                newTempRecord.LocationID = tempLocationImp.LocationID;
//                                                newTempRecord.LocationName = tempLocationImp.LocationName;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        newTempRecord.LocationID = tempLocation.LocationID;
//                                        newTempRecord.LocationName = tempLocation.LocationName;
//                                    }
//                                }
//                                else
//                                {
//                                    strRejectReason = strRejectReason + " Location not found.";
//                                }
//                            }
//                        }
//                        else
//                        {
//                            Location tempLocation = SiteManager.ets_Location_Details((int)newTempRecord.LocationID, ref connection, ref tn);

//                            if (tempLocation == null)
//                            {
//                                strRejectReason = strRejectReason + " Location not found.";
//                            }
//                            else
//                            {
//                                newTempRecord.LocationID = tempLocation.LocationID;
//                                newTempRecord.LocationName = tempLocation.LocationName;
//                            }
//                        }

//                    }
//                    else
//                    {
//                        newTempRecord.LocationID = int.Parse(strLocationID);
//                        newTempRecord.LocationName = strLocation;
//                    }
//                    if (newTempRecord.DateTimeRecorded == null)
//                    {
//                        if (strRejectReason == "")
//                        {
//                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                        }
//                        else
//                        {
//                            if (strRejectReason.IndexOf("Date Recorded") > -1)
//                            {
//                            }
//                            else
//                            {
//                                strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                            }
//                        }
//                    }

//                    if (strRejectReason == "")
//                    {
//                        if (newTempRecord.LocationID != null)
//                        {
//                            if (RecordManager.ets_Record_IsDuplicate(newTempRecord, ref connection, ref tn))
//                            {
//                                strRejectReason = strRejectReason + " DUPLICATE Record!";
//                            }
//                        }
//                    }


//                    if (strRejectReason.Length > 0)
//                    {
//                        newTempRecord.RejectReason = strRejectReason.Trim();
//                    }

//                    if (strWarningReason.Length > 0)
//                    {
//                        newTempRecord.WarningReason = strWarningReason.Trim();
//                    }
//                    //check blank line

//                    int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord, ref connection, ref tn);

//                }

//                try
//                {
//                    if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
//                        File.Delete(strImportFolder + "\\" + strFileUniqueName);
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
//                else if (ex.Message.IndexOf("string was not recognized") > -1)
//                {
//                    strMsg = "Unknown error occurred please review your import data.";
//                }
//                else if (ex.Message.IndexOf("Time Recorded") > -1)
//                {
//                    strMsg = "The file must have a 'Time Recorded' column just after 'Date Recorded' column.";
//                }
//                else if (ex.Message.IndexOf("must refer to a location within the string") > -1)
//                {
//                    strMsg = "The file columns are not well positioned.";
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


//    }





//    public static void UploadCSV(User objUser, Table theTable, string strBatchDescription,
//    string strOriginalFileName, Guid guidNew, string strImportFolder, string strLocationID,
//    string strLocation, out string strMsg, out int iBatchID, ref SqlConnection connection, ref SqlTransaction tn, string strFileExtension, string strSelectedSheet)
//    {
//        string strTemp = "";
//        string strFileUniqueName = guidNew.ToString() + strFileExtension;
//        strMsg = "";
//        int z = 0;
//        iBatchID = -1;

//        string strDateRecordedColumnName = "Date Recorded";
//        string strTimeSamledColumnName = "Time Recorded";

//        if (strLocationID == "-1")
//        {
//            DataTable dtSS = Common.DataTableFromText(@"SELECT     Location.LocationID
//                FROM         Location INNER JOIN
//                                      LocationTable ON Location.LocationID = LocationTable.LocationID
//                WHERE Location.IsActive=1 AND  LocationTable.TableID=" + theTable.TableID.ToString(), ref connection, ref tn);
//            if (dtSS.Rows.Count > 0)
//            {


//            }
//            else
//            {
//                //no Location, so create one

//                try
//                {

//                    DataTable dtSS2 = Common.DataTableFromText(@"SELECT     Location.LocationID
//                                FROM         Location  
//                                WHERE Location.IsActive=1 AND LocationName='Location 1'
//                                AND AccountID=" + theTable.AccountID.ToString(), ref connection, ref tn);

//                    int iLocationID = -1;
//                    if (dtSS2.Rows.Count > 0)
//                    {
//                        iLocationID = int.Parse(dtSS2.Rows[0][0].ToString());
//                    }
//                    else
//                    {

//                        Location newLocation = new Location(null, "Location 1", null, "", "Created at the time of import", true, null, null, null, null, (int)theTable.AccountID, "", "");
//                        iLocationID = SiteManager.ets_Location_Insert(newLocation, tn, null);
//                    }



//                    LocationTable newLocationTable = new LocationTable(null, iLocationID, (int)theTable.TableID, "", "");
//                    SiteManager.ets_LocationTable_Insert(newLocationTable, tn, null);

//                    strLocationID = iLocationID.ToString();
//                }
//                catch
//                {
//                    //
//                }

//            }

//        }



//        if (theTable.IsImportPositional == false)
//        {

//            try
//            {

//                strMsg = "";


//                //lets get date and time column name

//                DataTable dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      Name_OnImport FROM  Column
//                        WHERE SystemName='DateTimeRecorded' AND TableID=" + theTable.TableID.ToString(), ref connection, ref tn);

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

//                string strImportHeaderName = "Name_OnImport";

//                dtImportFileTable = null;

//                switch (strFileExtension.ToLower())
//                {

//                    case ".csv":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName);
//                        z = 1;
//                        break;
//                    case ".xls":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet);
//                        break;
//                    case ".xlsx":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet);
//                        break;

//                    case ".xml":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromXML(strImportFolder, strFileUniqueName);
//                        strImportHeaderName = "DisplayName";

//                        break;

//                }

//                //PERFORM CLIENT Specific treatment

//                if (theTable.ImportDataStartRow != null)
//                {
//                    for (int i = 1; i <= (int)theTable.ImportDataStartRow; i++)
//                    {
//                        dtImportFileTable.Rows.RemoveAt(0);

//                    }
//                    dtImportFileTable.AcceptChanges();

//                }





//                DataTable dtRecordTypleColumns;
//                string strListOfNoNeedColumns = "";


//                //dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();

//                dtImportFileTable = dtImportFileTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare(field.ToString().Trim(), string.Empty) == 0)).CopyToDataTable();



//                if (strImportHeaderName == "DisplayName")
//                {
//                    dtRecordTypleColumns = RecordManager.ets_Table_Columns_DisplayName((int)theTable.TableID, ref connection, ref tn);
//                }
//                else
//                {
//                    dtRecordTypleColumns = RecordManager.ets_Table_Columns_Import((int)theTable.TableID, ref connection, ref tn);
//                }



//                for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
//                {
//                    bool bIsFound = false;
//                    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                    {
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                            Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strImportHeaderName].ToString().Trim().ToLower()))
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

//                List<string> strRemoveIndexes = strListOfNoNeedColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();


//                foreach (string item in strRemoveIndexes)
//                {
//                    dtImportFileTable.Columns.Remove(item);
//                }


//                for (int r = 0; r < dtImportFileTable.Columns.Count; r++)
//                {
//                    for (int i = 0; i < dtRecordTypleColumns.Rows.Count; i++)
//                    {
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                            Common.RemoveSpecialCharacters(dtRecordTypleColumns.Rows[i][strImportHeaderName].ToString().Trim().ToLower()))
//                        {
//                            dtImportFileTable.Columns[r].ColumnName = dtRecordTypleColumns.Rows[i]["SystemName"].ToString();
//                            break;
//                        }
//                        if (Common.RemoveSpecialCharacters(dtImportFileTable.Columns[r].ColumnName.Trim().ToLower()) ==
//                          strDateRecordedColumnName.ToLower())
//                        {
//                            dtImportFileTable.Columns[r].ColumnName = "DateTimeRecorded";
//                            break;
//                        }
//                    }

//                }







//                //now dtCSV is ready to be imported into Batch & TempRecord
//                Batch newBatch = new Batch(null, (int)theTable.TableID,
//                    strBatchDescription.Trim() == "" ? strOriginalFileName : strBatchDescription.Trim(),
//                    strOriginalFileName, null, guidNew, objUser.UserID, objUser.AccountID, theTable.IsImportPositional);

//                //need a single transaction

//                iBatchID = UploadManager.ets_Batch_Insert(newBatch, ref connection, ref tn);



//                for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//                {
//                    TempRecord newTempRecord = new TempRecord();
//                    newTempRecord.AccountID = objUser.AccountID;
//                    newTempRecord.BatchID = iBatchID;
//                    //bool bIsBlank = false;
//                    string strRejectReason = "";
//                    string strWarningReason = "";

//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        string strColumnName = "";
//                        strColumnName = dc.ColumnName;

//                        if (strColumnName.ToLower() != strTimeSamledColumnName.ToLower())
//                        {

//                            if (dc.ColumnName.ToUpper() == "LocationID")
//                            {
//                                strColumnName = "LocationName";
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERecorded")
//                            {
//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() == "")
//                                {
//                                    strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    //bIsBlank = true;
//                                }
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERecorded")
//                            {
//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                {

//                                    try
//                                    {
//                                        if (strFileExtension == ".csv")
//                                        {
//                                            if (strTimeSamledColumnName == "")
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                            }
//                                            else
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() + " " + dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString());
//                                            }

//                                        }
//                                        else if (strFileExtension == ".xml")
//                                        {
//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                        }
//                                        else
//                                        {
//                                            string strDateTimeTemp = "";
//                                            if (strTimeSamledColumnName == "")
//                                            {
//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                                {
//                                                    UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0));
//                                                }
//                                            }
//                                            else
//                                            {
//                                                strDateTimeTemp = dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString();

//                                                if (dtImportFileTable.Rows[r][strTimeSamledColumnName].ToString().Length > 10)
//                                                {
//                                                    strDateTimeTemp = strDateTimeTemp.Substring(11);
//                                                }
//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                                {
//                                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 9)
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, 10) + " " + strDateTimeTemp);
//                                                    }
//                                                    else
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0) + " " + strDateTimeTemp);
//                                                    }
//                                                    //
//                                                }
//                                            }
//                                        }
//                                    }
//                                    catch
//                                    {

//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                        //bIsBlank = true;
//                                    }
//                                }
//                                else
//                                {
//                                    if (strRejectReason == "")
//                                    {
//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    }
//                                    else
//                                    {
//                                        if (strRejectReason.IndexOf(strDateRecordedColumnName) > -1)
//                                        {
//                                        }
//                                        else
//                                        {
//                                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                        }
//                                    }
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
//                                            dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());

//                                            dtImportFileTable.AcceptChanges();
//                                        }

//                                    }



//                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 0)
//                                    {





//                                        if (dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString() != "")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                            {
//                                                if (!UploadManager.IsDataIntoDropDown(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString()))
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                                }

//                                            }

//                                        }


//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
//                                                {

//                                                }
//                                                else
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                                }
//                                            }

//                                        }

//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
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
//                                            int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

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

//                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

//                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

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
//                                    else
//                                    {

//                                        if (dtRecordTypleColumns.Rows[i]["IsMandatory"].ToString().ToLower() == "true")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
//                                            {
//                                                strRejectReason = strRejectReason + " REQUIRED:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                            }

//                                        }

//                                    }
//                                    break;
//                                }
//                            }
//                        }
//                    }



//                    newTempRecord.TableID = (int)theTable.TableID;

//                    if (strLocationID == "-1")
//                    {
//                        if (newTempRecord.LocationID == null)
//                        {
//                            if (newTempRecord.LocationName == null)
//                            {
//                                //ops we have not found it
//                                strRejectReason = strRejectReason + " Location not found.";
//                            }
//                            else
//                            {

//                                if (newTempRecord.LocationName.Length > 0)
//                                {

//                                    Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);

//                                    if (tempLocation == null)
//                                    {
//                                        Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);
//                                        {
//                                            if (tempLocationImp == null)
//                                            {
//                                                //ops we have not found it
//                                                strRejectReason = strRejectReason + " Location -" + newTempRecord.LocationName + " - is not found!";
//                                            }
//                                            else
//                                            {
//                                                newTempRecord.LocationID = tempLocationImp.LocationID;
//                                                newTempRecord.LocationName = tempLocationImp.LocationName;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        newTempRecord.LocationID = tempLocation.LocationID;
//                                        newTempRecord.LocationName = tempLocation.LocationName;
//                                    }

//                                }
//                                else
//                                {
//                                    strRejectReason = strRejectReason + " Location not found.";
//                                }
//                            }
//                        }
//                        else
//                        {
//                            Location tempLocation = SiteManager.ets_Location_Details((int)newTempRecord.LocationID, ref connection, ref tn);

//                            if (tempLocation == null)
//                            {
//                                strRejectReason = strRejectReason + " Location not found.";
//                            }
//                            else
//                            {
//                                newTempRecord.LocationID = tempLocation.LocationID;
//                                newTempRecord.LocationName = tempLocation.LocationName;
//                            }
//                        }

//                    }
//                    else
//                    {
//                        newTempRecord.LocationID = int.Parse(strLocationID);
//                        newTempRecord.LocationName = strLocation;
//                    }

//                    if (newTempRecord.DateTimeRecorded == null)
//                    {
//                        if (strRejectReason == "")
//                        {
//                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                        }
//                        else
//                        {
//                            if (strRejectReason.IndexOf(strDateRecordedColumnName) > -1)
//                            {
//                            }
//                            else
//                            {
//                                strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                            }
//                        }
//                    }


//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
//                        {

//                            if (dc.ColumnName.ToLower() ==
//                                   dtRecordTypleColumns.Rows[j]["SystemName"].ToString().Trim().ToLower())
//                            {
//                                //check sensor information
//                                if (newTempRecord.LocationID != null)
//                                {
//                                    if (!bool.Parse(dtRecordTypleColumns.Rows[j]["IsStandard"].ToString()))
//                                    {
//                                        if (dtRecordTypleColumns.Rows[j]["DropdownValues"].ToString() == "")
//                                        {
//                                            string strLinkMsg = "";
//                                            int iSensonIDTemp = -1;
//                                            bool bIsSensorOK = InstrumentManager.IsSensorOk((int)newTempRecord.LocationID, int.Parse(dtRecordTypleColumns.Rows[j]["ColumnID"].ToString()), newTempRecord.DateTimeRecorded,
//                                                dtImportFileTable.Rows[r][dc.ColumnName].ToString(), ref strLinkMsg, ref iSensonIDTemp, ref connection, ref tn);
//                                            if (bIsSensorOK == false)
//                                            {
//                                                strWarningReason = strWarningReason + " SENSOR WARNING: " + dtRecordTypleColumns.Rows[j]["DisplayName"].ToString() + " - " + strLinkMsg;

//                                            }
//                                        }
//                                    }
//                                }

//                            }
//                        }

//                    }

//                    strRejectReason = strRejectReason.Trim();
//                    strWarningReason = strWarningReason.Trim();


//                    if (strRejectReason == "")
//                    {
//                        if (newTempRecord.LocationID != null)
//                        {
//                            if (RecordManager.ets_Record_IsDuplicate(newTempRecord, ref connection, ref tn))
//                            {
//                                strRejectReason = strRejectReason + " DUPLICATE Record!";
//                            }
//                        }
//                    }


//                    //                    

//                    if (strRejectReason.Length > 0)
//                    {
//                        newTempRecord.RejectReason = strRejectReason.Trim();
//                    }
//                    if (strWarningReason.Length > 0)
//                    {
//                        newTempRecord.WarningReason = strWarningReason.Trim();
//                    }



//                    int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord, ref connection, ref tn);


//                }

//                try
//                {
//                    if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
//                        File.Delete(strImportFolder + "\\" + strFileUniqueName);
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
//            try
//            {

//                strMsg = "";

//                bool bIsDateSingleColumn = false;
//                int iDatePosition = 1;
//                DataTable dtDateTimeColumnName = Common.DataTableFromText(@"SELECT      IsDateSingleColumn,PositionOnImport FROM  Column
//                        WHERE SystemName='DateTimeRecorded' AND TableID=" + theTable.TableID.ToString(), ref connection, ref tn);
//                if (dtDateTimeColumnName.Rows.Count > 0 && dtDateTimeColumnName.Rows[0][0] != DBNull.Value)
//                {

//                    if (dtDateTimeColumnName.Rows[0]["IsDateSingleColumn"].ToString().ToLower() == "true")
//                    {
//                        bIsDateSingleColumn = true;
//                    }
//                    iDatePosition = int.Parse(dtDateTimeColumnName.Rows[0]["PositionOnImport"].ToString());
//                }


//                DataTable dtImportFileTable;
//                dtImportFileTable = null;

//                switch (strFileExtension.ToLower())
//                {

//                    case ".csv":
//                        dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName);
//                        z = 1;
//                        break;
//                    case ".xls":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet);
//                        break;
//                    case ".xlsx":
//                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet);
//                        break;

//                }

//                //Remove rows

//                if (theTable.ImportDataStartRow != null)
//                {
//                    for (int i = 1; i <= (int)theTable.ImportDataStartRow; i++)
//                    {
//                       dtImportFileTable.Rows.RemoveAt(0);
 
//                    }
//                    dtImportFileTable.AcceptChanges();

//                }






//                DataTable dtRecordTypleColumns = RecordManager.ets_Table_Import_Position((int)theTable.TableID, ref connection, ref tn);


//                string strListOfNoNeedColumns = "";
//                for (int i = 0; i < dtImportFileTable.Columns.Count; i++)
//                {
//                    bool bIsFound = false;
//                    for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
//                    {
//                        if (i == (int.Parse(dtRecordTypleColumns.Rows[j]["PositionOnImport"].ToString()) - 1))
//                        {
//                            bIsFound = true;
//                            dtImportFileTable.Columns[i].ColumnName = dtRecordTypleColumns.Rows[j]["SystemName"].ToString();
//                            break;
//                        }
//                    }
//                    if (bIsFound == false)
//                    {                     

//                        for (int j = 0; j < dtRecordTypleColumns.Rows.Count; j++)
//                        {
//                            if (i == (iDatePosition+1 - 1))
//                            {
//                                bIsFound = true;
//                                dtImportFileTable.Columns[i].ColumnName = "time Recorded";
//                                break;
//                            }
//                        }

//                        if (dtImportFileTable.Columns[i].ColumnName.ToLower() != "time Recorded")
//                        {
//                            strListOfNoNeedColumns += dtImportFileTable.Columns[i].ColumnName + ",";
//                        }
//                    }
//                }

//                List<string> strRemoveIndexes = strListOfNoNeedColumns.Split(',').Where(s => (!String.IsNullOrEmpty(s))).ToList();


//                foreach (string item in strRemoveIndexes)
//                {
//                    dtImportFileTable.Columns.Remove(item);
//                }







//                //now dtCSV is ready to be imported into Batch & TempRecord
//                Batch newBatch = new Batch(null, (int)theTable.TableID,
//                    strBatchDescription.Trim() == "" ? strOriginalFileName : strBatchDescription.Trim(),
//                    strOriginalFileName, null, guidNew, objUser.UserID, objUser.AccountID, theTable.IsImportPositional);





//                iBatchID = UploadManager.ets_Batch_Insert(newBatch, ref connection, ref tn);



//                for (int r = z; r < dtImportFileTable.Rows.Count; r++)
//                {
//                    TempRecord newTempRecord = new TempRecord();
//                    newTempRecord.AccountID = objUser.AccountID;
//                    newTempRecord.BatchID = iBatchID;
//                    //bool bIsBlank = false;
//                    string strRejectReason = "";
//                    string strWarningReason = "";

//                    foreach (DataColumn dc in dtImportFileTable.Columns)
//                    {
//                        string strColumnName = "";
//                        strColumnName = dc.ColumnName;
//                        if (strColumnName.ToLower() != "time Recorded")
//                        {
//                            if (dc.ColumnName.ToUpper() == "LocationID")
//                            {
//                                strColumnName = "LocationName";
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERecorded")
//                            {
//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
//                                {
//                                    strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    //bIsBlank = true;
//                                }
//                            }

//                            if (dc.ColumnName.ToUpper() == "DATETIMERecorded")
//                            {
//                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                {
//                                    try
//                                    {
//                                        if (strFileExtension == ".csv")
//                                        {
//                                            if (bIsDateSingleColumn)
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() );
//                                            }
//                                            else
//                                            {
//                                                UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString() + " " + dtImportFileTable.Rows[r]["Time Recorded"].ToString());
//                                            }
//                                        }
//                                        else
//                                        {
//                                            if (bIsDateSingleColumn)
//                                            {

//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                                {                   
//                                                     UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString());
//                                                }

//                                            }
//                                            else
//                                            {
//                                                string strDateTimeTemp = dtImportFileTable.Rows[r]["Time Recorded"].ToString();

//                                                if (dtImportFileTable.Rows[r]["Time Recorded"].ToString().Length > 10)
//                                                {
//                                                    strDateTimeTemp = strDateTimeTemp.Substring(11);
//                                                }
//                                                if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Trim() != "")
//                                                {

//                                                    //UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, 10) + " " + strDateTimeTemp);

//                                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 9)
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0, 10) + " " + strDateTimeTemp);
//                                                    }
//                                                    else
//                                                    {
//                                                        UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtImportFileTable.Rows[r][dc.ColumnName].ToString().Substring(0) + " " + strDateTimeTemp);
//                                                    }
//                                                }
//                                            }

//                                        }
//                                    }
//                                    catch
//                                    {
//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                        //bIsBlank = true;
//                                    }
//                                }
//                                else
//                                {
//                                    if (strRejectReason == "")
//                                    {
//                                        strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                    }
//                                    else
//                                    {
//                                        if (strRejectReason.IndexOf("Date Recorded") > -1)
//                                        {
//                                        }
//                                        else
//                                        {
//                                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                                        }
//                                    }

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
//                                            dtImportFileTable.Rows[r][dc.ColumnName] = dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString();

//                                            UploadManager.MakeTheTempRecord(ref newTempRecord, strColumnName, dtRecordTypleColumns.Rows[i]["DefaultValue"].ToString());

//                                            dtImportFileTable.AcceptChanges();
//                                        }

//                                    }




//                                    if (dtImportFileTable.Rows[r][dc.ColumnName].ToString().Length > 0)
//                                    {

//                                        //Not Empty

//                                        if (dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString() != "")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() != "")
//                                            {
//                                                if (!UploadManager.IsDataIntoDropDown(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["DropdownValues"].ToString()))
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                                }

//                                            }

//                                        }



//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnEntry"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
//                                                {

//                                                }
//                                                else
//                                                {
//                                                    strRejectReason = strRejectReason + " INVALID:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();
//                                                }
//                                            }

//                                        }

//                                        if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"] != DBNull.Value)
//                                        {
//                                            if (dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
//                                            {
//                                                if (UploadManager.IsDataValid(dtImportFileTable.Rows[r][dc.ColumnName].ToString(), dtRecordTypleColumns.Rows[i]["ValidationOnWarning"].ToString(), ref strTemp, bool.Parse(dtRecordTypleColumns.Rows[i]["IgnoreSymbols"].ToString())))
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
//                                            int? iCount = RecordManager.ets_Table_GetCount((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

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

//                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

//                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)theTable.TableID, dtRecordTypleColumns.Rows[i]["SystemName"].ToString(), ref connection, ref tn, -1);

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
//                                    else
//                                    {
//                                        if (dtRecordTypleColumns.Rows[i]["IsMandatory"].ToString().ToLower() == "true")
//                                        {
//                                            if (dtImportFileTable.Rows[r][dc.ColumnName].ToString() == "")
//                                            {
//                                                strRejectReason = strRejectReason + " REQUIRED:" + dtRecordTypleColumns.Rows[i]["DisplayName"].ToString();

//                                            }

//                                        }


//                                    }
//                                    break;
//                                }
//                            }
//                        }
//                    }

//                    newTempRecord.TableID = (int)theTable.TableID;

//                    if (strLocationID == "-1")
//                    {
//                        if (newTempRecord.LocationID == null)
//                        {
//                            if (newTempRecord.LocationName == null)
//                            {
//                                //ops we have not found it
//                                strRejectReason = strRejectReason + " Location not found.";
//                            }
//                            else
//                            {

//                                if (newTempRecord.LocationName.Length > 0)
//                                {

//                                    Location tempLocation = SiteManager.ets_Location_ByName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);

//                                    if (tempLocation == null)
//                                    {
//                                        Location tempLocationImp = SiteManager.ets_Location_ByImportName((int)theTable.TableID, newTempRecord.LocationName, ref connection, ref tn);
//                                        {
//                                            if (tempLocationImp == null)
//                                            {
//                                                //ops we have not found it
//                                                strRejectReason = strRejectReason + " Location -" + newTempRecord.LocationName + " - is not found!";
//                                            }
//                                            else
//                                            {
//                                                newTempRecord.LocationID = tempLocationImp.LocationID;
//                                                newTempRecord.LocationName = tempLocationImp.LocationName;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        newTempRecord.LocationID = tempLocation.LocationID;
//                                        newTempRecord.LocationName = tempLocation.LocationName;
//                                    }
//                                }
//                                else
//                                {
//                                    strRejectReason = strRejectReason + " Location not found.";
//                                }
//                            }
//                        }
//                        else
//                        {
//                            Location tempLocation = SiteManager.ets_Location_Details((int)newTempRecord.LocationID, ref connection, ref tn);

//                            if (tempLocation == null)
//                            {
//                                strRejectReason = strRejectReason + " Location not found.";
//                            }
//                            else
//                            {
//                                newTempRecord.LocationID = tempLocation.LocationID;
//                                newTempRecord.LocationName = tempLocation.LocationName;
//                            }
//                        }

//                    }
//                    else
//                    {
//                        newTempRecord.LocationID = int.Parse(strLocationID);
//                        newTempRecord.LocationName = strLocation;
//                    }
//                    if (newTempRecord.DateTimeRecorded == null)
//                    {
//                        if (strRejectReason == "")
//                        {
//                            strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                        }
//                        else
//                        {
//                            if (strRejectReason.IndexOf("Date Recorded") > -1)
//                            {
//                            }
//                            else
//                            {
//                                strRejectReason = strRejectReason + " Invalid Date Recorded.";
//                            }
//                        }
//                    }

//                    if (strRejectReason == "")
//                    {
//                        if (newTempRecord.LocationID != null)
//                        {
//                            if (RecordManager.ets_Record_IsDuplicate(newTempRecord, ref connection, ref tn))
//                            {
//                                strRejectReason = strRejectReason + " DUPLICATE Record!";
//                            }
//                        }
//                    }


//                    if (strRejectReason.Length > 0)
//                    {
//                        newTempRecord.RejectReason = strRejectReason.Trim();
//                    }

//                    if (strWarningReason.Length > 0)
//                    {
//                        newTempRecord.WarningReason = strWarningReason.Trim();
//                    }
//                    //check blank line

//                    int iTempRecordID = UploadManager.ets_TempRecord_Insert(newTempRecord, ref connection, ref tn);

//                }

//                try
//                {
//                    if (File.Exists(strImportFolder + "\\" + strFileUniqueName))
//                        File.Delete(strImportFolder + "\\" + strFileUniqueName);
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
//                else if (ex.Message.IndexOf("string was not recognized") > -1)
//                {
//                    strMsg = "Unknown error occurred please review your import data.";
//                }
//                else if (ex.Message.IndexOf("Time Recorded") > -1)
//                {
//                    strMsg = "The file must have a 'Time Recorded' column just after 'Date Recorded' column.";
//                }
//                else if (ex.Message.IndexOf("must refer to a location within the string") > -1)
//                {
//                    strMsg = "The file columns are not well positioned.";
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


//    }




}




