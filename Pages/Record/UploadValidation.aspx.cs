using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using System.Net.Mail;


public partial class Pages_Record_UploadValidation : SecurePage 
{
    int _iTotalDynamicColumns = 0;
    Common_Pager _gvPager;
    Common_Pager _gvPagerInvalid;
    Common_Pager _gvPagerWarning;
    Table _qsTable;
    Batch _qsBatch;
    //string _qsMenu = "";
    string _qsBatchID = "";
    string _qsTableID = "";
    //DataTable _dtValid;    
    int _iValidRecordIDIndex = 0;
    int _iInValidValidRecordIDIndex = 0;
    int _iWarningRecordIDIndex = 0;
    DataTable _dtRecordColums;
     DataTable _dtDataSourceWarning;
     DataTable _dtDataSourceInvalid;
     string _strRecordRightID = Common.UserRoleType.None;
     User _ObjUser;
     UserRole _theUserRole;
     Account _theAccount;

     int _iNumberOfExceedance = 0;
    
     string _strFilesPhisicalPath = "";

     int? _iImportTemplateID = null;
     ImportTemplate _theImportTemplate = null;
     bool _bIsRevalidate = false;
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }


    protected void PopulateTerminology()
    {
        stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));

    }

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Valid - "  +_qsTable.TableName;
        gvTheGrid.PageIndex = 0;// why???
        BindTheGrid(0, _gvPager.TotalRows);
    }



    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {

        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }


    protected void gvTheGrid_PreRender(object sender, EventArgs e)
    {
        GridView grid = (GridView)sender;
        if (grid != null)
        {
            GridViewRow pagerRow = (GridViewRow)grid.TopPagerRow;
            if (pagerRow != null)
            {
                pagerRow.Visible = true;
            }
        }
        //gvTheGrid.Columns[10].Visible = false;

    }

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[_iTotalDynamicColumns ].Visible = false;
            e.Row.Cells[_iValidRecordIDIndex+1].Visible = false; 

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

            e.Row.Cells[_iTotalDynamicColumns ].Visible = false;
            e.Row.Cells[_iValidRecordIDIndex+1].Visible = false;
        }
    }

    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {
        gvTheGrid.PageIndex = 0;// why???
        BindTheGrid(0, gvTheGrid.PageSize);

    }


    protected void gvInValid_PreRender(object sender, EventArgs e)
    {
        GridView grid = (GridView)sender;
        if (grid != null)
        {
            GridViewRow pagerRow = (GridViewRow)grid.TopPagerRow;
            if (pagerRow != null)
            {
                pagerRow.Visible = true;
            }
        }
        

    }

    protected void gvWarning_PreRender(object sender, EventArgs e)
    {
        GridView grid = (GridView)sender;
        if (grid != null)
        {
            GridViewRow pagerRow = (GridViewRow)grid.TopPagerRow;
            if (pagerRow != null)
            {
                pagerRow.Visible = true;
            }
        }


    }

    protected void gvInValid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

       
        
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
            e.Row.Cells[_iTotalDynamicColumns].Visible = false;
            e.Row.Cells[_iInValidValidRecordIDIndex + 1].Visible = false; 
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

            e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
            e.Row.Cells[_iTotalDynamicColumns].Visible = false;
            e.Row.Cells[_iInValidValidRecordIDIndex + 1].Visible = false;


            string strRecordID = "-1";

            if (DataBinder.Eval(e.Row.DataItem, "DBGSystemRecordID") != DBNull.Value)
            {
                strRecordID = DataBinder.Eval(e.Row.DataItem, "DBGSystemRecordID").ToString();
            }

            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {
                for (int j = 0; j < _dtDataSourceInvalid.Columns.Count; j++)
                {
                    if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                    {
                        if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceInvalid.Columns[j].ColumnName)
                        {
                            e.Row.Cells[j + 1].HorizontalAlign = HorizontalAlign.Right;

                        }
                    }
                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "RecordID")
                    {
                        if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceInvalid.Columns[j].ColumnName)
                        {
                            e.Row.Cells[j + 1].HorizontalAlign = HorizontalAlign.Right;
                            //strRecordID = e.Row.Cells[j + 1].Text;
                        }
                    }

                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "IsActive"
                        || _dtRecordColums.Rows[i]["SystemName"].ToString() == "DateTimeRecorded"
                        )
                    {
                        if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceInvalid.Columns[j].ColumnName)
                        {
                            e.Row.Cells[j + 1].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }

                }
            }

            //Red & Blue Color

            if (strRecordID != "-1")
            {
                TempRecord aRecord = UploadManager.ets_TempRecord_Detail_Full(int.Parse(strRecordID));

                if (aRecord != null)
                {
                    if (aRecord.RejectReason != "")
                    {
                        //for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                        //{
                        //    aRecord.ValidationResults = aRecord.ValidationResults.Replace(_dtRecordColums.Rows[i]["DisplayName"].ToString(), _dtRecordColums.Rows[i]["NameOnImport"].ToString());
                        //}

                        //now we have warning with NameOnImport

                        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                        {
                            for (int j = 0; j < _dtDataSourceInvalid.Columns.Count; j++)
                            {
                                if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                                {
                                    if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceInvalid.Columns[j].ColumnName)
                                    {
                                        if (aRecord.RejectReason.IndexOf(": " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0
                                            || aRecord.RejectReason.IndexOf(":" + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0)
                                        {
                                            e.Row.Cells[j + 1].ForeColor = System.Drawing.Color.Red;
                                        }
                                        //if (aRecord.RejectReason.IndexOf(":" + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0)
                                        //{
                                        //    e.Row.Cells[j + 1].ForeColor = System.Drawing.Color.Red;
                                        //}
                                    }
                                }
                                else
                                {
                                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "DateTimeRecorded")
                                    {
                                        if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceInvalid.Columns[j].ColumnName)
                                        {
                                            if (aRecord.RejectReason.IndexOf(WarningMsg.MaxtimebetweenRecords) >= 0)
                                            {
                                                e.Row.Cells[j + 1].ForeColor = System.Drawing.Color.Red;
                                            }

                                        }
                                    }
                                }
                            }
                        }

                    }




                }
            }

        }
        }
        catch (Exception ex)
        {

            //throw;
        }
    }

    protected void gvInValid_Sorting(object sender, GridViewSortEventArgs e)
    {
        gvInValid.PageIndex = 0;// why???
        BindTheInvalidGrid(0, gvInValid.PageSize);

    }

    protected void gvWarning_Sorting(object sender, GridViewSortEventArgs e)
    {
        gvWarning.PageIndex = 0;// why???
        BindWarningGrid(0, gvWarning.PageSize);

    }

    protected void gvWarning_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].ForeColor = System.Drawing.Color.Blue;
            e.Row.Cells[_iTotalDynamicColumns].Visible = false;
            e.Row.Cells[_iWarningRecordIDIndex + 1].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

            e.Row.Cells[1].ForeColor = System.Drawing.Color.Blue;
            e.Row.Cells[_iTotalDynamicColumns].Visible = false;
            e.Row.Cells[_iWarningRecordIDIndex + 1].Visible = false;

            string strWarning = "";
            if (DataBinder.Eval(e.Row.DataItem, "Warning Reason") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "Warning Reason").ToString()!="")
            {

                strWarning = DataBinder.Eval(e.Row.DataItem, "Warning Reason").ToString();

                if (strWarning.IndexOf("EXCEEDANCE:") > -1 && strWarning.IndexOf("WARNING:") > -1)
                {
                    for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                    {
                        if (strWarning.IndexOf("EXCEEDANCE: " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) > -1)
                        {
                            strWarning = strWarning.Replace("EXCEEDANCE: " + _dtRecordColums.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.",
                               "<span style='color:#ffa500;'>" + "EXCEEDANCE: " + _dtRecordColums.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range." + "</span>");

                            e.Row.Cells[1].Text = strWarning;
                        }
                    }

                }
                else if (strWarning.IndexOf("EXCEEDANCE:") > -1)
               {
                   e.Row.Cells[1].ForeColor = System.Drawing.Color.Orange;
                   
               }
            }

            string strRecordID = "-1";
            if (DataBinder.Eval(e.Row.DataItem, "DBGSystemRecordID") != DBNull.Value)
            {
                strRecordID = DataBinder.Eval(e.Row.DataItem, "DBGSystemRecordID").ToString();
            }
            
            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {
                for (int j = 0; j < _dtDataSourceWarning.Columns.Count; j++)
                {
                    if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                    {
                        if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceWarning.Columns[j].ColumnName)
                        {
                            e.Row.Cells[j + 1].HorizontalAlign = HorizontalAlign.Right;

                        }
                    }
                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "RecordID")
                    {
                        if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceWarning.Columns[j].ColumnName)
                        {
                            e.Row.Cells[j + 1].HorizontalAlign = HorizontalAlign.Right;
                            //strRecordID = e.Row.Cells[j + 1].Text;
                        }
                    }

                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "IsActive"
                        || _dtRecordColums.Rows[i]["SystemName"].ToString() == "DateTimeRecorded"
                        )
                    {
                        if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceWarning.Columns[j].ColumnName)
                        {
                            e.Row.Cells[j + 1].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }

                }
            }

            //Red & Blue Color

            if (strRecordID != "-1")
            {
                TempRecord aRecord = UploadManager.ets_TempRecord_Detail_Full(int.Parse(strRecordID));

                if (aRecord != null)
                {
                    if (aRecord.WarningReason != "")
                    {
                        //for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                        //{
                        //    aRecord.WarningReason = aRecord.WarningReason.Replace(_dtRecordColums.Rows[i]["DisplayName"].ToString(), _dtRecordColums.Rows[i]["NameOnImport"].ToString());
                        //}

                        //now we have warning with NameOnImport

                        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                        {
                            for (int j = 0; j < _dtDataSourceWarning.Columns.Count; j++)
                            {
                                if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                                {
                                    if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceWarning.Columns[j].ColumnName)
                                    {
                                        if (aRecord.WarningReason.IndexOf("WARNING: " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0)
                                        {
                                            e.Row.Cells[j + 1].ForeColor = System.Drawing.Color.Blue;
                                        }
                                        if (aRecord.WarningReason.IndexOf("EXCEEDANCE: " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0)
                                        {
                                            e.Row.Cells[j + 1].ForeColor = System.Drawing.Color.Orange;
                                        }
                                    }
                                }
                                else
                                {
                                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "DateTimeRecorded")
                                    {
                                        if (_dtRecordColums.Rows[i]["NameOnImport"].ToString() == _dtDataSourceWarning.Columns[j].ColumnName)
                                        {
                                            if (aRecord.WarningReason.IndexOf( WarningMsg.MaxtimebetweenRecords ) >= 0)
                                            {
                                                e.Row.Cells[j + 1].ForeColor = System.Drawing.Color.Blue;
                                            }

                                        }
                                    }
                                }
                            }
                        }

                    }




                }
            }
        }
        }
        catch (Exception ex)
        {

            //throw;
        }
    }


    protected void gvInValidPager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPagerInvalid.ExportFileName = "Invalid - " + _qsTable.TableName;
        gvInValid.PageIndex = 0;// why???
        BindTheInvalidGrid(0, _gvPagerInvalid.TotalRows);
    }

    protected void gvWarningPager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPagerWarning.ExportFileName = "Invalid - " + _qsTable.TableName;
        gvWarning.PageIndex = 0;// why???
        BindWarningGrid(0, _gvPagerWarning.TotalRows);
    }



    protected void gvInValidPager_BindTheGridAgain(object sender, EventArgs e)
    {

        BindTheInvalidGrid(_gvPagerInvalid.StartIndex, _gvPagerInvalid._gridView.PageSize);
    }

    protected void gvWarningPager_BindTheGridAgain(object sender, EventArgs e)
    {

        BindWarningGrid(_gvPagerWarning.StartIndex, _gvPagerWarning._gridView.PageSize);
    }


    //public string GetViewURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=view&TableID=" + ddlTable.SelectedValue + "&Recordid=";

    //}


    protected void ddlInValidReason_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindTheInvalidGrid(0, gvInValid.PageSize);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Title = "Records";
       
      


        try
        {

           
            _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();

            if (Request.QueryString["ImportTemplateID"] != null)//for compability
            {
                _iImportTemplateID = int.Parse(Cryptography.Decrypt(Request.QueryString["ImportTemplateID"].ToString()));
                _theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)_iImportTemplateID);
            }
            if (!IsPostBack)
            {

              
                    


                if (Request.QueryString["auto"] != null)
                {
                    hlBack.Visible = false;

                }
                if (Request.QueryString["FileInfo"] == null)
                {
                    trColumnHeader.Visible = false;
                }
            }

            _ObjUser = (User)Session["User"];
            _theUserRole = (UserRole)Session["UserRole"];

            _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
            _qsBatchID = Cryptography.Decrypt(Request.QueryString["BatchID"]);
            _qsBatch = UploadManager.ets_Batch_Details(int.Parse(_qsBatchID));

            _qsTableID = _qsBatch.TableID.ToString();

           if(_qsBatch.ImportTemplateID!=null)
           {
               _iImportTemplateID = (int)_qsBatch.ImportTemplateID;
               _theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)_iImportTemplateID);
           }

           _qsTable = RecordManager.ets_Table_Details(int.Parse(_qsTableID));
            //Menu qsMenu = RecordManager.ets_Menu_Details((int)_qsTable.MenuID);
            if (_theImportTemplate != null)
            {
                _qsTable.ImportColumnHeaderRow = _theImportTemplate.ImportColumnHeaderRow;
                _qsTable.ImportDataStartRow = _theImportTemplate.ImportDataStartRow;
            }


            //_dtRecordColums = RecordManager.ets_Table_Columns_Summary(
            // int.Parse(_qsTableID), null);

            _dtRecordColums = RecordManager.ets_Table_Columns_Import(
            int.Parse(_qsTableID), _iImportTemplateID);

            if (!IsPostBack)
            {
                if (_theImportTemplate != null)
                {
                    trImportTemplate.Visible = true;
                    lblImportTemplate.Text = _theImportTemplate.ImportTemplateName;
                }

                //if(_qsTable.DataUpdateUniqueColumnID!=null)
                //{
                   
                    if (_qsBatch.AllowDataUpdate != null && (bool)_qsBatch.AllowDataUpdate)
                    {
                        tblAllowDataUpdate.Visible = true;
                        lblDataUpdate.Text = "Yes";
                    }
                //}
               

            }

            if ((bool)_theUserRole.IsAdvancedSecurity)
            {
                //DataTable dtUserTable = SecurityManager.ets_UserTable_Select(null,
                //    int.Parse(_qsTableID), _ObjUser.UserID, null);

                DataTable dtUserTable = null;

                //if (_ObjUser.RoleGroupID == null)
                //{
                    dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
                   int.Parse(_qsTableID), _theUserRole.RoleID, null);
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
                lnkImport.Enabled = false;
                //lnkImportNow2.Enabled = false;
            }



            
            lblTable.Text = _qsTable.TableName;
            lblBatch.Text = _qsBatch.BatchDescription;
            lblFile.Text = _qsBatch.UploadedFileName;
            //lblIsImported.Text = _qsBatch.IsImported.ToString();

            if (_qsBatch.IsImported == true)
            {
                divImport.Visible = false;
                //lnkImportNow2.Visible = false;
                lblIsImported.Text = "Yes";
            }
            else
            {
                if (_theAccount.UploadAfterVerificaition != null)
                {
                    if ((bool)_theAccount.UploadAfterVerificaition)
                    {
                        //
                        trRejectSubmit.Visible = true;
                        if (Request.QueryString["Review"] != null)
                        {
                            if (Cryptography.Decrypt(Request.QueryString["Review"].ToString()).ToString()
                                == _qsBatch.BatchID.ToString() + _qsBatch.TableID.ToString())
                            {
                                divSubmit.Visible = false;
                                divImport.Visible = true;
                                divReject.Visible = true;
                            }
                        }
                        else
                        {
                            if (Common.HaveAccess(_strRecordRightID, "1,2,8,9"))
                            {
                                divSubmit.Visible = false;
                                divImport.Visible = true;
                                divReject.Visible = true;
                            }
                            else
                            {

                                divSubmit.Visible = true;
                                divImport.Visible = false;
                            }
                        }

                    }
                    else
                    {
                        divImport.Visible = true;
                    }
                }
                else
                {
                    
                    divImport.Visible = true;
                }
                //lnkImportNow2.Visible = true;
                lblIsImported.Text = "No";

                

            }

            if (_qsBatch.IsImportPositional == true)
            {
               
                lblPositional.Text = "Yes";
            }
            else
            {

                lblPositional.Text = "No";
            }

            //if (_qsTable.AddMissingLocation != null)
            //{
            //    if ((bool)_qsTable.AddMissingLocation)
            //    {
            //        lblMissingSS.Text = "Yes";
            //    }
            //    else
            //    {
            //        lblMissingSS.Text = "No";
            //    }
            //}
            //else
            //{
            //    lblMissingSS.Text = "No";

            //}

            Title = "Data Validation - " + _qsTable.TableName;
            lblTitle.Text = "Data Validation - " + _qsTable.TableName;
          
            if (!IsPostBack)
            {
                PopulateInvalidReject(_qsTableID,_qsBatchID);
                if (Request.QueryString["ColumnHeaderRow"] != null
                    && Request.QueryString["DataStartRow"] != null)
                {
                    txtImportColumnHeaderRow.Text = Request.QueryString["ColumnHeaderRow"].ToString();
                    txtImportDataStartRow.Text = Request.QueryString["DataStartRow"].ToString();
                    hfColumnRowChanges.Value = "yes";
                    if(Request.QueryString["revalidate"]!=null)
                    {
                        hfColumnRowChanges.Value = "no";
                    }
                }
                else
                {
                    if(_qsTable.ImportColumnHeaderRow!=null)
                        txtImportColumnHeaderRow.Text = _qsTable.ImportColumnHeaderRow.ToString();

                    if (_qsTable.ImportDataStartRow != null)
                        txtImportDataStartRow.Text = _qsTable.ImportDataStartRow.ToString();
                }

                //if (Common.HaveAccess(_strRecordRightID, "1,2"))
                //{

                //    hlChangeAccountType.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountTypeChange.aspx?AccountID=" + Cryptography.Encrypt(_qsTable.AccountID.ToString());

                //}

                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { 
                    gvTheGrid.PageSize = int.Parse(Session["GridPageSize"].ToString());
                    gvInValid.PageSize = int.Parse(Session["GridPageSize"].ToString());
                
                }

                gvTheGrid.PageIndex = 0;// why???
                BindTheGrid(0, gvTheGrid.PageSize);

                gvInValid.PageIndex = 0;// why???
                BindTheInvalidGrid(0, gvInValid.PageSize);

                gvWarning.PageIndex = 0;// why???
                BindWarningGrid(0, gvWarning.PageSize);

                if (Request.QueryString["menu"] != null && Cryptography.Decrypt(Request.QueryString["menu"]) == "yes")
                {
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Batches.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString());
                    
                }
                else
                {
                    if (Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri != null && Request.UrlReferrer.AbsoluteUri.IndexOf("Record/UploadValidation.aspx") == -1)
                    {
                        hlBack.NavigateUrl = Request.UrlReferrer.AbsoluteUri;
                    }
                    else
                    {
                        hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordUpload.aspx?TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString()) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();
                    }
                    
                }

                if (Request.QueryString["FileInfo"] != null)
                {
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordUpload.aspx?TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString()) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"];
                }

            }
            else
            {

            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

            GridViewRow gvrInValid = gvInValid.TopPagerRow;
            if (gvrInValid != null)
                _gvPagerInvalid = (Common_Pager)gvrInValid.FindControl("Pager");

            GridViewRow gvrWarning = gvWarning.TopPagerRow;
            if (gvrWarning != null)
                _gvPagerWarning = (Common_Pager)gvrWarning.FindControl("Pager");

        }
        catch (Exception ex)
        {
        }



        string strFancy = @"
       $(function () {
            $("".popuplink"").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 900,
                height: 350,
                titleShow: false
            });
        });
      ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);

        if (!IsPostBack)
        {
            PopulateTerminology();
        }

    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        lblMsg.Text = "";
        try
        {
            int iTN = 0;

            string strOrderDirection = "DESC";
            string sOrder = "";

            if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
            {
                strOrderDirection = "ASC";
            }
            if (gvTheGrid.GridViewSortColumn != "")
            {
                sOrder = gvTheGrid.GridViewSortColumn;
            }


            if (_qsBatch.IsImportPositional==true)
            {
                gvTheGrid.DataSource = UploadManager.ets_TempRecord_Position_List(int.Parse(_qsTableID),
                    int.Parse(_qsBatchID), false,false, null,
                     null, null,
                 sOrder, strOrderDirection, iStartIndex, iMaxRows, ref iTN, ref _iTotalDynamicColumns);
            }
            else
            {

                gvTheGrid.DataSource = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
                    int.Parse(_qsBatchID), false,false, null,
                   null, null,
                 sOrder, strOrderDirection, iStartIndex, iMaxRows, ref iTN, ref _iTotalDynamicColumns, "", _iImportTemplateID, "");
            }

            DataTable dtTemp = (DataTable)gvTheGrid.DataSource;
            for (int i = 0; i < dtTemp.Columns.Count; i++)
            {
                if (dtTemp.Columns[i].ColumnName == "DBGSystemRecordID")
                {
                    _iValidRecordIDIndex = i;
                    break;
                }
            }

            //gvTheGrid.DataSource = RecordManager.ets_Record_List(int.Parse(ddlTable.SelectedValue),
            //    ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
            //    ddlActiveFilter.SelectedValue == "-1" ? null : (bool?)bool.Parse(ddlActiveFilter.SelectedValue),
            //    GetLocationIDs(),
            //    txtDateFrom.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateFrom.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
            //    txtDateTo.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
            //    sOrder, strOrderDirection, iStartIndex, iStartIndex, ref iTN, ref _iTotalDynamicColumns);



            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            lblValidData.Text = "Valid Data: " + iTN.ToString() ;
            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                //_gvPager.AddURL = GetAddURL();
            }
            if (iTN == 0)
            {
                divEmptyData.Visible = true;
                //hplNewData.NavigateUrl = GetAddURL();
            }
            else
            {
                divEmptyData.Visible = false;
            }
            //lblSelectedTable.Text = ddlTable.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Upload Validation", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }










    protected void PopulateInvalidReject(string sTableID,string sBatchID)
    {

        ddlInValidReason.Items.Clear();
        DataTable dtTemp = Common.DataTableFromText(@"SELECT RejectReason,COUNT(*) FROM TempRecord 
            WHERE RejectReason IS NOT NULL AND TableID=" + sTableID + @" AND TempRecord.BatchID="+sBatchID+@" GROUP BY RejectReason");

        if (dtTemp.Rows.Count > 0)
        {
            foreach (DataRow dr in dtTemp.Rows)
            {
                ListItem liTemp = new ListItem(dr[0].ToString() + "(" + dr[1].ToString() + " records)", dr[0].ToString());
                ddlInValidReason.Items.Insert(0, liTemp);
            }

            ListItem liAll = new ListItem("All", "");
            ddlInValidReason.Items.Insert(0, liAll);
        }
        else
        {
            ddlInValidReason.Visible = false;

        }


    }

    protected void BindWarningGrid(int iStartIndex, int iMaxRows)
    {
        lblMsg.Text = "";
        try
        {
            int iTN = 0;

            string strOrderDirection = "DESC";
            string sOrder = "";

            if (gvWarning.GridViewSortDirection == SortDirection.Ascending)
            {
                strOrderDirection = "ASC";
            }
            if (gvWarning.GridViewSortColumn != "")
            {
                sOrder = gvInValid.GridViewSortColumn;
            }


            if (_qsBatch.IsImportPositional == true)
            {
                gvWarning.DataSource = UploadManager.ets_TempRecord_Position_List(int.Parse(_qsTableID),
                   int.Parse(_qsBatchID), false, true, null,
                    null, null,
                sOrder, strOrderDirection, iStartIndex, iMaxRows, ref iTN, ref _iTotalDynamicColumns);
            }
            else
            {

                gvWarning.DataSource = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
                    int.Parse(_qsBatchID), false, true, null,
                     null, null,
                 sOrder, strOrderDirection, iStartIndex, iMaxRows, ref iTN, ref _iTotalDynamicColumns, "", _iImportTemplateID, "");

            }
            DataTable dtTemp = (DataTable)gvWarning.DataSource;
            _dtDataSourceWarning=dtTemp;
            for (int i = 0; i < dtTemp.Columns.Count; i++)
            {
                if (dtTemp.Columns[i].ColumnName == "DBGSystemRecordID")
                {
                    _iWarningRecordIDIndex = i;
                    break;
                }
            }

            //gvInValid.DataSource = RecordManager.ets_Record_List(int.Parse(ddlTable.SelectedValue),
            //    ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
            //    ddlActiveFilter.SelectedValue == "-1" ? null : (bool?)bool.Parse(ddlActiveFilter.SelectedValue),
            //    GetLocationIDs(),
            //    txtDateFrom.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateFrom.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
            //    txtDateTo.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
            //    sOrder, strOrderDirection, iStartIndex, iStartIndex, ref iTN, ref _iTotalDynamicColumns);
            gvWarning.VirtualItemCount = iTN;
            gvWarning.DataBind();
            if (gvWarning.TopPagerRow != null)
                gvWarning.TopPagerRow.Visible = true;


            lblWarning.Text = "Warnings: " + iTN.ToString() + "&nbsp;<a href='#stWarnings'> View</a>";

            if(Common.SO_ShowExceedances(_qsTable.AccountID,_qsTable.TableID))
            {
                tdExceedance.Visible = true;
                string strWC = Common.GetValueFromSQL(@"Select COUNT(RecordID)
                        FROM [TempRecord] WHERE WarningReason is not null AND RejectReason is null 
                                AND WarningReason LIKE '%WARNING:%'   AND WarningReason NOT LIKE '%EXCEEDANCE:%'
                                AND BatchID =" + _qsBatchID.ToString());
                string strEC = Common.GetValueFromSQL(@"Select COUNT(RecordID)
                        FROM [TempRecord] WHERE WarningReason is not null AND RejectReason is null 
                                AND WarningReason LIKE '%EXCEEDANCE:%'
                                AND BatchID =" + _qsBatchID.ToString());
                if (strWC == "")
                    strWC = "0";
                if (strEC == "")
                    strEC = "0";
                lblWarning.Text = "Warnings: " + strWC + "&nbsp;<a href='#stWarnings'> View</a>";
                lblExceedance.Text = "Exceedances: " + strEC.ToString() + "&nbsp;<a href='#stWarnings'> View</a>";
            }
            else
            {
                tdExceedance.Visible = false;
            }

            GridViewRow gvr = gvWarning.TopPagerRow;
            if (gvr != null)
            {
                _gvPagerWarning = (Common_Pager)gvr.FindControl("Pager");
                
            }
            if (iTN == 0)
            {
                divEmptyWarning.Visible = true;
                //hplNewData.NavigateUrl = GetAddURL();
            }
            else
            {
                divEmptyWarning.Visible = false;
            }
            //lblSelectedTable.Text = ddlTable.SelectedItem.Text;

            
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Upload Validation", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }

     protected bool IsStandard(string strColumnName)
    {
        if (strColumnName.Substring(0, 1).ToUpper() == "V")
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    protected void BindTheInvalidGrid(int iStartIndex, int iMaxRows)
    {
        lblMsg.Text = "";
        try
        {
            int iTN = 0;

            string strOrderDirection = "DESC";
            string sOrder = "";

            if (gvInValid.GridViewSortDirection == SortDirection.Ascending)
            {
                strOrderDirection = "ASC";
            }
            if (gvInValid.GridViewSortColumn != "")
            {
                sOrder = gvInValid.GridViewSortColumn;
            }

            string strRF = "";

            if (ddlInValidReason.SelectedItem != null)
            {
                if (ddlInValidReason.SelectedValue != "")
                    strRF = " AND RejectReason='" + ddlInValidReason.SelectedValue.Replace("'", "''") + "'";
            }

            if (_qsBatch.IsImportPositional == true)
            {
                gvInValid.DataSource = UploadManager.ets_TempRecord_Position_List(int.Parse(_qsTableID),
                   int.Parse(_qsBatchID), true,null, null,
                    null, null,
                sOrder, strOrderDirection, iStartIndex, iMaxRows, ref iTN, ref _iTotalDynamicColumns);
            }
            else
            {

                gvInValid.DataSource = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
                    int.Parse(_qsBatchID), true,null, null,
                     null, null,
                 sOrder, strOrderDirection, iStartIndex, iMaxRows, ref iTN, ref _iTotalDynamicColumns, strRF, _iImportTemplateID, "");

            }
            DataTable dtTemp = (DataTable)gvInValid.DataSource;
            _dtDataSourceInvalid = dtTemp;
            for (int i = 0; i < dtTemp.Columns.Count; i++)
            {
                if (dtTemp.Columns[i].ColumnName == "DBGSystemRecordID")
                {
                    _iInValidValidRecordIDIndex = i;
                    break;
                }
            }

            //gvInValid.DataSource = RecordManager.ets_Record_List(int.Parse(ddlTable.SelectedValue),
            //    ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
            //    ddlActiveFilter.SelectedValue == "-1" ? null : (bool?)bool.Parse(ddlActiveFilter.SelectedValue),
            //    GetLocationIDs(),
            //    txtDateFrom.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateFrom.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
            //    txtDateTo.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
            //    sOrder, strOrderDirection, iStartIndex, iStartIndex, ref iTN, ref _iTotalDynamicColumns);
            gvInValid.VirtualItemCount = iTN;
            gvInValid.DataBind();
            if (gvInValid.TopPagerRow != null)
                gvInValid.TopPagerRow.Visible = true;


            lblInvalidData.Text = "Invalid Data: " + iTN.ToString() + "&nbsp;<a href='#stInvalid'>View </a>";

            GridViewRow gvr = gvInValid.TopPagerRow;
            if (gvr != null)
            {
                _gvPagerInvalid = (Common_Pager)gvr.FindControl("Pager");
                //_gvPager.AddURL = GetAddURL();
            }
            if (iTN == 0)
            {
                divEmptyDataInValid.Visible = true;
                //hplNewData.NavigateUrl = GetAddURL();
            }
            else
            {
                divEmptyDataInValid.Visible = false;
            }
            //lblSelectedTable.Text = ddlTable.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Upload Validation", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }


    public string GetViewURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString()) + "&Recordid=";

    }

    //protected void cmdBack_Click(object sender, ImageClickEventArgs e)
    //protected void lnkBack_Click(object sender, EventArgs e)
    //{
    //    if (Request.QueryString["menu"] != null && Cryptography.Decrypt(Request.QueryString["menu"]) == "yes")
    //    {
    //        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Batches.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString()));
    //    }
    //    else
    //    {
    //        Response.Redirect("~/Pages/Record/RecordUpload.aspx?TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString()), false);
    //    }

    //}
    //protected void cmdImport_Click(object sender, ImageClickEventArgs e)


    protected void lnkReject_Click(object sender, EventArgs e)
    {
        //delete a batch

        UploadManager.ets_Batch_Delete(int.Parse(_qsBatchID));

        Response.Redirect("~/Default.aspx", false);
    }


    protected void lnkSubmit_Click(object sender, EventArgs e)
    {

        //lets get 

        //string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
        //string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
        //string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
        //string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
        //string strWarningSMSEMail = SystemData.SystemOption_ValueByKey("WarningSMSEmail");
        //string strEnableSSL = SystemData.SystemOption_ValueByKey("EnableSSL");
        //string strSmtpPort = SystemData.SystemOption_ValueByKey("SmtpPort");

        Content theContentEmail = SystemData.Content_Details_ByKey("HoldingPenReviewEmail", (int)_theAccount.AccountID);
        //Content theContentSMS = SystemData.Content_Details_ByKey("HoldingPenReviewSMS", (int)_theAccount.AccountID);
        string strBody = "";
        //string strBodySMS = "";


        strBody = theContentEmail.ContentP;

        //strBodySMS = theContentSMS.ContentP;

        strBody = strBody.Replace("[FullName]", _ObjUser.FirstName + " " + _ObjUser.LastName);
        //strBodySMS = strBodySMS.Replace("[FullName]", _ObjUser.FirstName + " " + _ObjUser.LastName);

        strBody = strBody.Replace("[Tablename]", _qsTable.TableName);
        //strBodySMS = strBodySMS.Replace("[Tablename]", _qsTable.TableName);


        strBody = strBody.Replace("[ReviewBatch]", Request.Url.AbsoluteUri + "&Review=" + Cryptography.Encrypt(_qsBatch.BatchID.ToString() + _qsBatch.TableID.ToString()) + "&SearchCriteriaID=" + Cryptography.Encrypt("-1"));
        //strBodySMS = strBodySMS.Replace("[ReviewRecord]", Request.RawUrl + "&Review=" + Cryptography.Encrypt(_qsBatch.BatchID.ToString() + _qsBatch.TableID.ToString()));




        //MailMessage msg = new MailMessage();
        //msg.From = new MailAddress(strEmail);


        string strSubject = theContentEmail.Heading.Replace("[Table]", _qsTable.TableName);

        //msg.IsBodyHtml = true;

        //msg.Body = strBody;
        //SmtpClient smtpClient = new SmtpClient(strEmailServer);
        //smtpClient.Timeout = 99999;
        //smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

        //smtpClient.EnableSsl = bool.Parse(strEnableSSL);
        //smtpClient.Port = int.Parse(strSmtpPort);


        //msg.To.Clear();
        //msg.To.Add(drAD["Email"].ToString());

        User oAdmin = SecurityManager.User_AccountHolder((int)_qsBatch.AccountID);


        //msg.To.Add(oAdmin.Email);

        string strTo = oAdmin.Email;
        //msg.Body = strBody;

        try
        {


#if (!DEBUG)
            //smtpClient.Send(msg);
#endif
            string strError = "";

            DBGurus.SendEmail("Upload validation", true, null, strSubject, strBody, "", strTo, "", "", null, null, out strError);

            //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
            //{

            //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
            //}

        }
        catch (Exception ex)
        {

            //strErrorMsg = "Server could not send warning Email & SMS";
        }

        Session["tdbmsg"] = "Your data has been submitted for review and you will be notified as soon it has been imported.";
        Response.Redirect("~/Default.aspx", false);





    }


    protected void lnkImport_Click(object sender, EventArgs e)
    {
        if (hfColumnRowChanges.Value == "yes")
        {
            mpeColumnRowStart.Show();
           
            return;
        }

        ImportThisBatch();
    }

    protected void ImportThisBatch()
    {
        lblMsg.Text="";

        //if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        //{
        //    lblMsg.Text = "Global user can not import!";
        //    return;

        //}

        if (_qsBatch.IsImported == true)
        {
            lblMsg.Text = "This batch is already imported! You can now only see this batch, after few days this batch will be removed from the database!";
            return;
        }

        string strImportMsg="";

        string strRollBackSQL = @"DELETE Record WHERE BatchID=" + _qsBatch.BatchID.ToString() + "; Update Batch set IsImported=0 WHERE BatchID=" + _qsBatch.BatchID.ToString() + ";";

        bool bUpdatedBatch=false;

        try
        {
            if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())) == false)
            {
                strImportMsg = UploadManager.ImportClickFucntions(_qsBatch);


                if (strImportMsg!="ok")
                {
                    lblMsg.Text = strImportMsg;
                    return;
                }


                if(_theImportTemplate!=null && _theImportTemplate.SPName!="")
                {
                    try
                    {
                      string strReturn=  UploadManager.ImportTemplate_SPName(_theImportTemplate.SPName, (int)_qsBatch.BatchID, (int)_ObjUser.UserID);
                    }
                    catch
                    {
                        //
                    }
                    
                }
                Table theTable = RecordManager.ets_Table_Details((int)_qsBatch.TableID);

                if(_qsBatch.AllowDataUpdate!=null && (bool)_qsBatch.AllowDataUpdate )
                {
                    bUpdatedBatch = true;

                    //if(theTable.DataUpdateUniqueColumnID!=null)
                    //{
                    //    Column theColumn = RecordManager.ets_Column_Details((int)theTable.DataUpdateUniqueColumnID);

                    //    if(theColumn!=null)
                    //    {
                    //      nUpdatedRowCount=  UploadManager.spUpdateExistingData((int)_qsBatch.TableID, theColumn.SystemName, (int)_qsBatch.BatchID);
                    //    }                           

                    //}                    
                }


                //check if this is a users upload

                string strVirtualUsersTableID = SystemData.SystemOption_ValueByKey_Account("VirtualUsersTableID", int.Parse(Session["AccountID"].ToString()), null);
                if(strVirtualUsersTableID!="" && strVirtualUsersTableID==theTable.TableID.ToString())
                {
                    //lets add users

                    int iTotalUserImported = SecurityManager.dbg_Users_Import((int)_qsBatch.BatchID);
                    string strBatchRecordAmount = Common.GetValueFromSQL("SELECT COUNT(*) FROM [TempRecord] WHERE BatchID=" + _qsBatch.BatchID);
                    if (strBatchRecordAmount == "")
                        strBatchRecordAmount = "0";

                    string strRejectedAmount = "0";
                    if (strBatchRecordAmount != iTotalUserImported.ToString())
                    {
                        strRejectedAmount = (int.Parse(strBatchRecordAmount) - iTotalUserImported).ToString();
                    }
                    string strNotifications = "";

                    if (iTotalUserImported>0)
                    {
                        strNotifications = iTotalUserImported.ToString() + " user(s) have been successfully imported. ";
                    }

                    if(strRejectedAmount.ToString()!="0")
                    {                   
                        strNotifications=strNotifications + strRejectedAmount + " user(s) have been rejected.";
                    }

                    Session["tdbmsg"] = strNotifications;
                    Response.Redirect("~/Pages/User/List.aspx", false);
                    return;
                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "DoNotAllow", "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);
                return;
            }
        }
        catch
        {
            lblMsg.Text = strImportMsg;
            Common.ExecuteText(strRollBackSQL);
        }


        if (strImportMsg != "ok")
        {
            lblMsg.Text = strImportMsg;
            Common.ExecuteText(strRollBackSQL);
           
        }
        else
        {
           
            if (Request.QueryString["Review"] != null)
            {
            
                Content theContentEmail = SystemData.Content_Details_ByKey("HoldingPenViewEmail", (int)_theAccount.AccountID);
                string strBody = theContentEmail.ContentP;

                string strExtra = "";
                if (_iImportTemplateID != null)
                    strExtra = "&ImportTemplateID=" + Cryptography.Encrypt(_iImportTemplateID.ToString());

                string strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&BatchID=" + Request.QueryString["BatchID"].ToString() + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                strURL = strURL + strExtra;
                
                strBody = strBody.Replace("[ViewBatch]", strURL);

                User oUser=SecurityManager.User_Details((int)_qsBatch.UserIDUploaded);

                string strSubject = theContentEmail.Heading.Replace("[Table]", _qsTable.TableName);

                string strTo = oUser.Email;         

                try
                {
                    string strError = "";
                    DBGurus.SendEmail("Validation Review", true, null, strSubject, strBody, "", strTo, "", "", null, null, out strError);                   

                }
                catch (Exception ex)
                {
                    //strErrorMsg = "Server could not send warning Email & SMS";
                }
            }

            string strImportedRecords = "0";
            try
            {               

                    UploadManager.RecordsImportEmail(_qsBatch, ref strImportedRecords,
                        Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath);              

                //SecurityManager.CheckRecordsExceeded(int.Parse(Session["AccountID"].ToString()));
            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Import Email error", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
               
            }
            string strNotifications="";

            
            string strImp_Upd="imported";
            if (bUpdatedBatch == false)
            {

            }
            else
            {
                strImp_Upd = "updated";
                //strImportedRecords = strImportedRecords.ToString();
            }
            if(strImportedRecords=="0")
            {
                strNotifications="No record has been "+strImp_Upd+".";
            }
            else if (strImportedRecords=="1")
            {
                strNotifications="1 record has been successfully "+strImp_Upd+".";
            }
            else
            {
                strNotifications=strImportedRecords + " records have been successfully "+strImp_Upd+".";
            }


            Session["tdbmsg"] = strNotifications;
            Response.Redirect("~/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString()) , false);
        }
        
    }
    //protected void cmdRevalidate_Click(object sender, ImageClickEventArgs e)
    //{

    //}
    //protected void cmdCancel_Click(object sender, ImageClickEventArgs e)
    protected void lnkCancel_Click(object sender, EventArgs e)
    {
        //delete this batch and go back
        UploadManager.ets_Batch_Delete(int.Parse(_qsBatchID));

        if (Request.QueryString["menu"] != null && Cryptography.Decrypt(Request.QueryString["menu"]) == "yes")
        {
            Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Batches.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString()));
        }
        else
        {
            Response.Redirect("~/Pages/Record/RecordUpload.aspx?TableID=" + Cryptography.Encrypt(_qsBatch.TableID.ToString()), false);
        }



    }



    protected void btnTrigger_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "ShowModalErrors();", true);
        //ModalPopupExtender1.Show();
    }







    //protected void lnkImportNow2_Click(object sender, EventArgs e)
    //{
    //    lnkImport_Click(null, null);
    //}





    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

            
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=\"Valid-" + _qsTable.TableName.Replace(' ', '-') + ".csv\"");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);



        int iTN = 0;
        gvTheGrid.PageIndex = 0;

        string strOrderDirection = "DESC";
        string sOrder = "";

        if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
        {
            strOrderDirection = "ASC";
        }
        if (gvTheGrid.GridViewSortColumn != "")
        {
            sOrder = gvTheGrid.GridViewSortColumn;
        }

       
        sOrder = gvTheGrid.GridViewSortColumn + " ";
        

        if (sOrder.Trim() == "")
        {
            sOrder = "DBGSystemRecordID";
        }





        DataTable dt=new DataTable();
        if (_qsBatch.IsImportPositional == true)
        {
            dt = UploadManager.ets_TempRecord_Position_List(int.Parse(_qsTableID),
                int.Parse(_qsBatchID), false, false, null,
                 null, null,
             sOrder, strOrderDirection, null, null, ref iTN, ref _iTotalDynamicColumns);
        }
        else
        {

            dt = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
                int.Parse(_qsBatchID), false, false, null,
                 null, null,
             sOrder, strOrderDirection, null, null, ref iTN, ref _iTotalDynamicColumns, "", _iImportTemplateID, "");
        }


        DataRow drFooter = dt.NewRow();

        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                {
                    if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
                    {
                        drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(dt, dt.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));

                    }


                }

            }

        }

        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
        {
            for (int j = dt.Columns.Count - 1; j >= 0; j--)
            {
                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                {
                    if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "true")
                    {
                        if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                        {
                            dt.Columns.RemoveAt(j);
                        }
                    }


                }

            }

        }


        dt.Rows.Add(drFooter);




        //Round export

        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //DisplayTextSummary
                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                    {
                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                        {

                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file"
                                || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "image")
                            {
                                if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
                                {
                                    try
                                    {
                                        if (dr[j].ToString().Length > 37)
                                        {
                                            dr[j] = dr[j].ToString().Substring(37);

                                        }
                                    }
                                    catch
                                    {
                                        //
                                    }
                                }

                            }



                            if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                                 && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                            {
                                if (_dtRecordColums.Rows[i]["DisplayTextSummary"].ToString() == dt.Columns[j].ColumnName)
                                {

                                    if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
                                    {
                                        try
                                        {
                                            int iTableRecordID = int.Parse(dr[j].ToString());
                                            DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
                                             + _dtRecordColums.Rows[i]["TableTableID"].ToString());

                                            string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

                                            foreach (DataRow dr2 in dtTableTableSC.Rows)
                                            {
                                                strDisplayColumn = strDisplayColumn.Replace("[" + dr2["DisplayName"].ToString() + "]", "[" + dr2["SystemName"].ToString() + "]");

                                            }

                                            DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());
                                            if (dtTheRecord.Rows.Count > 0)
                                            {
                                                foreach (DataColumn dc in dtTheRecord.Columns)
                                                {
                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                                }
                                            }

                                            dr[j] = strDisplayColumn;
                                        }
                                        catch
                                        {
                                            //
                                        }


                                    }
                                }

                            }



                            if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
                            {
                                if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
                                {
                                    if (dr[j].ToString() != "")
                                    {
                                        dr[j] = Math.Round(double.Parse(dr[j].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
                                    }
                                }

                            }
                        }

                    }

                    //mm:hh
                    if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
                    {

                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "datetime")
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                            {
                                if (dr[j].ToString().Length > 15)
                                {
                                    dr[j] = dr[j].ToString().Substring(0, 16);
                                }
                            }
                        }

                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "date")
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                            {
                                if (dr[j].ToString().Length > 9)
                                {
                                    dr[j] = dr[j].ToString().Substring(0, 10);
                                }
                            }
                        }


                    }

                }
            }
        }

        // First we will write the headers.

        int iColCount = dt.Columns.Count;



        for (int i = 0; i < iColCount - 2; i++)
        {
            sw.Write(dt.Columns[i].ToString().Replace(',','_'));
            if (i < iColCount - 3)
            {
                sw.Write(",");
            }

        }

        sw.Write(sw.NewLine);



        // Now write all the rows.


        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < iColCount - 2; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    sw.Write("\"" + dr[i].ToString().Replace("\"", "'") + "\"");
                }

                if (i < iColCount - 3)
                {
                    sw.Write(",");
                }
            }

            sw.Write(sw.NewLine);

        }

        sw.Close();

        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }


    protected void gvWarningPager_OnExportForCSV(object sender, EventArgs e)
    {


        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=\"Warning-" + _qsTable.TableName.Replace(' ', '-') + ".csv\"");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);



        int iTN = 0;
        gvTheGrid.PageIndex = 0;

        string strOrderDirection = "DESC";
        string sOrder = "";

        if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
        {
            strOrderDirection = "ASC";
        }
        if (gvTheGrid.GridViewSortColumn != "")
        {
            sOrder = gvTheGrid.GridViewSortColumn;
        }


        sOrder = gvTheGrid.GridViewSortColumn + " ";


        if (sOrder.Trim() == "")
        {
            sOrder = "DBGSystemRecordID";
        }





        DataTable dt = new DataTable();
        if (_qsBatch.IsImportPositional == true)
        {
            dt = UploadManager.ets_TempRecord_Position_List(int.Parse(_qsTableID),
                int.Parse(_qsBatchID), false, true, null,
                 null, null,
             sOrder, strOrderDirection, null, null, ref iTN, ref _iTotalDynamicColumns);
        }
        else
        {

            dt = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
                int.Parse(_qsBatchID), false, true, null,
                 null, null,
             sOrder, strOrderDirection, null, null, ref iTN, ref _iTotalDynamicColumns, "", _iImportTemplateID, "");
        }


        DataRow drFooter = dt.NewRow();

        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                {
                    if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
                    {
                        drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(dt, dt.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));

                    }


                }

            }

        }

        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
        {
            for (int j = dt.Columns.Count - 1; j >= 0; j--)
            {
                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                {
                    if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "true")
                    {
                        if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                        {
                            dt.Columns.RemoveAt(j);
                        }
                    }


                }

            }

        }


        dt.Rows.Add(drFooter);




        //Round export

        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //DisplayTextSummary
                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                    {
                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                        {

                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file"
                                || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "image")
                            {
                                if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
                                {
                                    try
                                    {
                                        if (dr[j].ToString().Length > 37)
                                        {
                                            dr[j] = dr[j].ToString().Substring(37);

                                        }
                                    }
                                    catch
                                    {
                                        //
                                    }
                                }

                            }



                            if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                                 && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                            {
                                if (_dtRecordColums.Rows[i]["DisplayTextSummary"].ToString() == dt.Columns[j].ColumnName)
                                {

                                    if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
                                    {
                                        try
                                        {
                                            int iTableRecordID = int.Parse(dr[j].ToString());
                                            DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
                                             + _dtRecordColums.Rows[i]["TableTableID"].ToString());

                                            string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

                                            foreach (DataRow dr2 in dtTableTableSC.Rows)
                                            {
                                                strDisplayColumn = strDisplayColumn.Replace("[" + dr2["DisplayName"].ToString() + "]", "[" + dr2["SystemName"].ToString() + "]");

                                            }

                                            DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());
                                            if (dtTheRecord.Rows.Count > 0)
                                            {
                                                foreach (DataColumn dc in dtTheRecord.Columns)
                                                {
                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                                }
                                            }

                                            dr[j] = strDisplayColumn;
                                        }
                                        catch
                                        {
                                            //
                                        }


                                    }
                                }

                            }



                            if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
                            {
                                if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
                                {
                                    if (dr[j].ToString() != "")
                                    {
                                        dr[j] = Math.Round(double.Parse(dr[j].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
                                    }
                                }

                            }
                        }

                    }

                    //mm:hh
                    if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
                    {

                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "datetime")
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                            {
                                if (dr[j].ToString().Length > 15)
                                {
                                    dr[j] = dr[j].ToString().Substring(0, 16);
                                }
                            }
                        }

                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "date")
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                            {
                                if (dr[j].ToString().Length > 9)
                                {
                                    dr[j] = dr[j].ToString().Substring(0, 10);
                                }
                            }
                        }


                    }

                }
            }
        }

        // First we will write the headers.

        int iColCount = dt.Columns.Count;



        for (int i = 0; i < iColCount - 2; i++)
        {
            sw.Write(dt.Columns[i].ToString().Replace(',', '_'));
            if (i < iColCount - 3)
            {
                sw.Write(",");
            }

        }

        sw.Write(sw.NewLine);



        // Now write all the rows.


        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < iColCount - 2; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    sw.Write("\"" + dr[i].ToString().Replace("\"", "'") + "\"");
                }

                if (i < iColCount - 3)
                {
                    sw.Write(",");
                }
            }

            sw.Write(sw.NewLine);

        }

        sw.Close();

        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }



    protected void gvInValidPager_OnExportForCSV(object sender, EventArgs e)
    {


        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
         "attachment;filename=\"Invalid-" + _qsTable.TableName.Replace(' ', '-') + ".csv\"");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);



        int iTN = 0;
        gvTheGrid.PageIndex = 0;

        string strOrderDirection = "DESC";
        string sOrder = "";

        if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
        {
            strOrderDirection = "ASC";
        }
        if (gvTheGrid.GridViewSortColumn != "")
        {
            sOrder = gvTheGrid.GridViewSortColumn;
        }


        sOrder = gvTheGrid.GridViewSortColumn + " ";


        if (sOrder.Trim() == "")
        {
            sOrder = "DBGSystemRecordID";
        }





        DataTable dt = new DataTable();
        if (_qsBatch.IsImportPositional == true)
        {
            dt = UploadManager.ets_TempRecord_Position_List(int.Parse(_qsTableID),
                int.Parse(_qsBatchID), true, null, null,
                 null, null,
             sOrder, strOrderDirection, null, null, ref iTN, ref _iTotalDynamicColumns);
        }
        else
        {

            dt = UploadManager.ets_TempRecord_List(int.Parse(_qsTableID),
                int.Parse(_qsBatchID), true, null, null,
                 null, null,
             sOrder, strOrderDirection, null, null, ref iTN, ref _iTotalDynamicColumns, "", _iImportTemplateID, "");
        }


        DataRow drFooter = dt.NewRow();

        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                {
                    if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
                    {
                        drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(dt, dt.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));

                    }


                }

            }

        }

        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
        {
            for (int j = dt.Columns.Count - 1; j >= 0; j--)
            {
                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                {
                    if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "true")
                    {
                        if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                        {
                            dt.Columns.RemoveAt(j);
                        }
                    }


                }

            }

        }


        dt.Rows.Add(drFooter);




        //Round export

        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //DisplayTextSummary
                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                    {
                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                        {

                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file"
                                || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "image")
                            {
                                if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
                                {
                                    try
                                    {
                                        if (dr[j].ToString().Length > 37)
                                        {
                                            dr[j] = dr[j].ToString().Substring(37);

                                        }
                                    }
                                    catch
                                    {
                                        //
                                    }
                                }

                            }



                            if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                                 && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                            {
                                if (_dtRecordColums.Rows[i]["DisplayTextSummary"].ToString() == dt.Columns[j].ColumnName)
                                {

                                    if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
                                    {
                                        try
                                        {
                                            int iTableRecordID = int.Parse(dr[j].ToString());
                                            DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
                                             + _dtRecordColums.Rows[i]["TableTableID"].ToString());

                                            string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

                                            foreach (DataRow dr2 in dtTableTableSC.Rows)
                                            {
                                                strDisplayColumn = strDisplayColumn.Replace("[" + dr2["DisplayName"].ToString() + "]", "[" + dr2["SystemName"].ToString() + "]");

                                            }

                                            DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());
                                            if (dtTheRecord.Rows.Count > 0)
                                            {
                                                foreach (DataColumn dc in dtTheRecord.Columns)
                                                {
                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                                }
                                            }

                                            dr[j] = strDisplayColumn;
                                        }
                                        catch
                                        {
                                            //
                                        }


                                    }
                                }

                            }



                            if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
                            {
                                if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
                                {
                                    if (dr[j].ToString() != "")
                                    {
                                        dr[j] = Math.Round(double.Parse(dr[j].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
                                    }
                                }

                            }
                        }

                    }

                    //mm:hh
                    if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
                    {

                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "datetime")
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                            {
                                if (dr[j].ToString().Length > 15)
                                {
                                    dr[j] = dr[j].ToString().Substring(0, 16);
                                }
                            }
                        }

                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "date")
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                            {
                                if (dr[j].ToString().Length > 9)
                                {
                                    dr[j] = dr[j].ToString().Substring(0, 10);
                                }
                            }
                        }


                    }

                }
            }
        }

        // First we will write the headers.

        int iColCount = dt.Columns.Count;



        for (int i = 0; i < iColCount - 2; i++)
        {
            sw.Write(dt.Columns[i].ToString().Replace(',', '_'));
            if (i < iColCount - 3)
            {
                sw.Write(",");
            }

        }

        sw.Write(sw.NewLine);



        // Now write all the rows.


        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < iColCount - 2; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    sw.Write("\"" + dr[i].ToString().Replace("\"", "'") + "\"");
                }

                if (i < iColCount - 3)
                {
                    sw.Write(",");
                }
            }

            sw.Write(sw.NewLine);

        }

        sw.Close();

        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }


    protected double CalculateTotalForAColumn(DataTable dt, string strColumn, bool bIgnoreSymbols)
    {
        double amount = 0;

        foreach (DataRow row in dt.Rows)
        {
            if (row[strColumn].ToString() != "")
            {
                try
                {
                    if (bIgnoreSymbols)
                    {
                        amount += double.Parse(Common.IgnoreSymbols(row[strColumn].ToString()), System.Globalization.NumberStyles.Any);
                    }
                    else
                    {
                        amount += double.Parse(row[strColumn].ToString(), System.Globalization.NumberStyles.Any);
                    }
                }
                catch (Exception ex)
                {
                    //
                }
            }
        }

        return amount;
    }

    protected void lnkChangeColumnStartYes_Click(object sender, EventArgs e)
    {
        if (txtImportColumnHeaderRow.Text.Trim() != ""
            && txtImportDataStartRow.Text.Trim() != "")
        {
           
            try
            {
                _qsTable.ImportColumnHeaderRow = int.Parse(txtImportColumnHeaderRow.Text.Trim());
                _qsTable.ImportDataStartRow = int.Parse(txtImportDataStartRow.Text.Trim());
                if (_theImportTemplate!=null)
                {
                    _theImportTemplate.ImportColumnHeaderRow = _qsTable.ImportColumnHeaderRow;
                    _theImportTemplate.ImportDataStartRow = _qsTable.ImportDataStartRow;
                    ImportManager.dbg_ImportTemplate_Update(_theImportTemplate);
                }
                else
                {
                    RecordManager.ets_Table_Update(_qsTable);
                }
                
               
            }
            catch
            {
               //
            }

        }
        ImportThisBatch();
        mpeColumnRowStart.Hide();
    }

    protected void lnkChangeColumnStartNo_Click(object sender, EventArgs e)
    {
        ImportThisBatch();
        mpeColumnRowStart.Hide();
    }


    protected void btnHiddenRefreshHeader_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["FileInfo"] != null)
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["FileInfo"].ToString())));


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                string strExcelFileName = xmlDoc.FirstChild["FileName"].InnerText;
               

                //hfRecordsData.Value = xmlDoc.FirstChild["chkRecordsData"].InnerText;
                //if (hfRecordsData.Value.ToLower() == "true")
                //{
                //    lblRecordsData.Text = "Yes";
                //}
                //else
                //{
                //    lblRecordsData.Text = "No";
                //}

                //lblImportColumnHeaderRow.Text = xmlDoc.FirstChild["txtImportColumnHeaderRow"].InnerText;
                //lblImportDataStartRow.Text = xmlDoc.FirstChild["txtImportDataStartRow"].InnerText;
                //lblShowunderMenu.Text = xmlDoc.FirstChild["ddlMenuText"].InnerText;
                string strShowMenu = xmlDoc.FirstChild["ddlMenuValue"].InnerText;
                //lblNewMenuName.Text = xmlDoc.FirstChild["txtNewMenuName"].InnerText;

                //if (lblNewMenuName.Text == "")
                //    trNewMenuName.Visible = false;
                //if (lblShowunderMenu.Text == "--None--")
                //    trNewMenuName.Visible = false;

                //lblImportColumnHeaderRow.Text = xmlDoc.FirstChild["txtImportColumnHeaderRow"].InnerText;
                //lblImportDataStartRow.Text = xmlDoc.FirstChild["txtImportDataStartRow"].InnerText;


               string strguidNew = xmlDoc.FirstChild["guidNew"].InnerText;
              
                string strFileExtension = "";
              strFileExtension = "." + strExcelFileName.Substring(strExcelFileName.LastIndexOf('.') + 1).ToLower();



                Guid guidNew = Guid.Empty;




                guidNew = Guid.Parse(strguidNew);

                string strFileUniqueName;
                strFileUniqueName = guidNew.ToString() + strFileExtension;

                string strSelectedSheet = "";
                if (strFileExtension == ".xls" || strFileExtension == ".xlsx")
                {

                    //List<string> lstSheets = OfficeManager.GetExcelSheetNames(Server.MapPath("../../UserFiles/AppFiles"), strFileUniqueName);

                    List<string> lstSheets = OfficeManager.GetExcelSheetNames( _strFilesPhisicalPath +"\\UserFiles\\AppFiles", strFileUniqueName);

                }

                //string strImportFolder = Server.MapPath("../../UserFiles/AppFiles");

                string strImportFolder = _strFilesPhisicalPath + "\\UserFiles\\AppFiles";

                DataTable dtImportFileTable;
                dtImportFileTable = null;

                int iRowIndex = 1;
                string strMsg = "";
                switch (strFileExtension.ToLower())
                {
                    case ".dbf":
                        dtImportFileTable = UploadManager.GetImportFileTableFromDBF(strImportFolder, strFileUniqueName, ref strMsg);
                        break;
                    case ".txt":
                        dtImportFileTable = UploadManager.GetImportFileTableFromText(strImportFolder, strFileUniqueName, ref strMsg);
                        break; 
                    case ".xls":
                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
                        break;
                    case ".xlsx":
                        dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
                        break;
                    case ".csv":
                        dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName, ref strMsg);
                        iRowIndex = 1;
                        break;

                }

                if (strMsg != "")
                {
                    lblMsg.Text = strMsg;
                    return;
                }


                //int iMenuID =(int) _qsTable.MenuID;
                int iBatchID = -1;

                //string strImportMsg;
                Batch theBatch = null;
                //SqlTransaction tn;
                //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

                //connection.Open();
                //tn = connection.BeginTransaction();

                try
                {

                                      


                    int iColulmnCount = 0;

                    Table newTable = new Table(null,
                                 _qsTable.TableName, 
                                 null, null, false, true);
                    newTable.AccountID = int.Parse(Session["AccountID"].ToString());




                    int iTableID = RecordManager.ets_Table_Insert(newTable,null); //need parent menu id

                    Table theTable = RecordManager.ets_Table_Details(iTableID);

                    if (txtImportColumnHeaderRow.Text.Trim() != ""
                    && txtImportDataStartRow.Text.Trim() != "")
                    {

                        theTable.ImportColumnHeaderRow = int.Parse(txtImportColumnHeaderRow.Text.Trim());
                        theTable.ImportDataStartRow = int.Parse(txtImportDataStartRow.Text.Trim());
                        if (_theImportTemplate != null)
                        {
                            _theImportTemplate.ImportColumnHeaderRow = theTable.ImportColumnHeaderRow;
                            _theImportTemplate.ImportDataStartRow = theTable.ImportDataStartRow;
                            ImportManager.dbg_ImportTemplate_Update(_theImportTemplate);
                        }
                        else
                        {
                            RecordManager.ets_Table_Update(theTable);
                        }

                       
                    }



                    //lets check column header

                    if (_theImportTemplate != null)
                    {
                        theTable.ImportColumnHeaderRow = _theImportTemplate.ImportColumnHeaderRow;
                        theTable.ImportDataStartRow = _theImportTemplate.ImportDataStartRow;
                    }

                    if (strFileExtension.ToLower() != ".dbf")
                    {
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

                        if (dtImportFileTable.Columns.Count > 0)
                        {
                            if (strFileExtension.ToLower() == ".csv")
                            {
                                if (dtImportFileTable.Columns[0].ColumnName.ToLower() == "column1")
                                {
                                    dtImportFileTable.Columns.RemoveAt(0);
                                    dtImportFileTable.AcceptChanges();
                                }

                            }
                            else
                            {
                                if (dtImportFileTable.Columns[0].ColumnName.ToLower() == "f1")
                                {
                                    dtImportFileTable.Columns.RemoveAt(0);
                                    dtImportFileTable.AcceptChanges();
                                }
                            }

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

                   
                    iColulmnCount = 5;
                    
                    int iColCount = 0;
                    foreach (DataColumn dc in dtImportFileTable.Columns)
                    {
                        bool bUsedColumn = false;
                        iColCount = iColCount + 1;
                        if (iColCount > 495)
                        {

                            break;
                        }

                       
                        if (bUsedColumn == false && dc.ColumnName.ToLower().IndexOf("dbgsystemrecordid") == -1)
                        {
                            //non standard column

                            string strAutoSystemName = "";

                            strAutoSystemName = RecordManager.ets_Column_NextSystemName(iTableID);
                            if (strAutoSystemName == "NO")
                            {
                                lblMsg.Text = "You can add 500 columns, please remove other columns!";
                                //tn.Rollback();
                                //connection.Close();

                                return;
                            }

                            int? iDisplayOrder = RecordManager.ets_Table_MaxOrder(iTableID);

                            if (iDisplayOrder == null)
                                iDisplayOrder = -1;

                            iColulmnCount = iColulmnCount + 1;



                            string strSummaryName = "";
                            string strColumnName = dc.ColumnName.Replace("'", "").Replace(",", "");

                            strSummaryName = strColumnName;
                            if (iColulmnCount > 10)
                            {
                                strSummaryName = "";

                            }

                            Column newColumn = new Column(null, iTableID,
                          strAutoSystemName, iDisplayOrder + 1, strSummaryName, strColumnName, strColumnName, strColumnName,
                           null, "", "", null, null, "", "", false, strColumnName, null, "", null,
                           null, null, strColumnName, "", null);

                            newColumn.ColumnType = "text";
                            //newColumn.NumberType = 1; //normal
                            if (dtImportFileTable.Rows.Count > iRowIndex)
                            {
                                if (dtImportFileTable.Rows[iRowIndex][dc.ColumnName] != null)
                                {
                                    try
                                    {
                                        if (dtImportFileTable.Rows[iRowIndex][dc.ColumnName].ToString() != "")
                                        {
                                            double dTest = double.Parse(dtImportFileTable.Rows[iRowIndex][dc.ColumnName].ToString());
                                            newColumn.ColumnType = "number";
                                            newColumn.GraphLabel = strColumnName;
                                            newColumn.NumberType = 1; //normal
                                        }
                                    }
                                    catch
                                    {
                                                                           
                                        try
                                        {
                                        
                                            DateTime dateValue;

                                            if (DateTime.TryParseExact(dtImportFileTable.Rows[iRowIndex][dc.ColumnName].ToString(), Common.Dateformats,
                                          new CultureInfo("en-GB"),
                                          DateTimeStyles.None,
                                          out dateValue))
                                            {
                                                newColumn.ColumnType = "date";
                                                newColumn.NumberType = null;
                                            }
                                            else
                                            {
                                                newColumn.ColumnType = "text";
                                                newColumn.NumberType = null;
                                            }
                                        }
                                        catch
                                        {
                                            newColumn.ColumnType = "text";
                                            newColumn.NumberType = null;
                                        }





                                    }

                                }
                            }



                            try
                            {
                                DataTable dtColumnID = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE TableID=" + newColumn.TableID + " AND IsStandard=1 AND DisplayName='" + newColumn.DisplayName.Replace("'", "''") + "'");

                                if (dtColumnID.Rows.Count == 0)
                                {
                                    RecordManager.ets_Column_Insert(newColumn);
                                }
                                else
                                {
                                    Column theColumn = RecordManager.ets_Column_Details(int.Parse(dtColumnID.Rows[0][0].ToString()));
                                    if (theColumn != null)
                                    {
                                        theColumn.DisplayTextSummary = newColumn.DisplayTextSummary;
                                        theColumn.DisplayTextDetail = newColumn.DisplayTextDetail;
                                        theColumn.NameOnImport = newColumn.NameOnImport;
                                        RecordManager.ets_Column_Update(theColumn);

                                    }
                                }
                            }
                            catch
                            {
                                //
                            }

                        }

                    }

                    

                    
                        //lets import the file

                       Table _theTable = RecordManager.ets_Table_Details(iTableID);


                       UploadManager.UploadCSV(_ObjUser.UserID, _theTable, strExcelFileName,
            strExcelFileName, guidNew, _strFilesPhisicalPath + "\\UserFiles\\AppFiles",
            out strMsg, out iBatchID,  strFileExtension,
            strSelectedSheet, int.Parse(Session["AccountID"].ToString()), _qsBatch.AllowDataUpdate, _iImportTemplateID, null);

                     theBatch = UploadManager.ets_Batch_Details(iBatchID);
                                      


                    //tn.Commit();
                    //connection.Close();
                    //connection.Dispose();

                    RecordManager.dbg_Table_Delete_Permanent((int)_qsTable.TableID);


                    string strExtra = "";
                    if (_iImportTemplateID != null)
                        strExtra = "&ImportTemplateID=" + Cryptography.Encrypt(_iImportTemplateID.ToString());

                    Response.Redirect("~/Pages/Record/UploadValidation.aspx?auto=yes&TableID=" + Cryptography.Encrypt(theBatch.TableID.ToString()) + "&BatchID=" + Cryptography.Encrypt(theBatch.BatchID.ToString()) + "&SearchCriteriaID="
                        + Cryptography.Encrypt("-1") + "&FileInfo=" + Request.QueryString["FileInfo"].ToString() + strExtra, false);
                    
                  

                }
                catch (Exception ex)
                {

                    //if (connection.State!=ConnectionState.Closed)
                    //{
                    //    tn.Rollback();
                    //    connection.Close();
                    //    connection.Dispose();
                    //}
                   

                    //if (ex.Message.IndexOf("Location") > -1)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('The file should have a " +
                    //        SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Location", "Location") + " column.');", true);
                    //}
                    //else 

                    if (ex.Message.IndexOf("Transaction count") > -1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This menu has a same " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " name, please try another name.');", true);
                    }
                    else if (ex.Message.IndexOf("UQ_SampleTypeDisplayName") > -1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('The file has duplicate column name.');", true);
                    }
                    else if (ex.Message.IndexOf("UQ_STG_StgAndAccountID") > -1)
                    {
                        lblMsg.Text = "This menu already exist, please try anohter menu name.";
                       
                        return;
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Error: " + ex.Message.Replace("'", "") + "');", true);

                        ErrorLog theErrorLog = new ErrorLog(null, "Table Sheet", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                        SystemData.ErrorLog_Insert(theErrorLog);
                    }
                    //throw; 


                } 






            }

        }

    }

    protected void lnkRevalidate_Click(object sender, EventArgs e)
    {
        _bIsRevalidate = true;
        btnHiddenRefresh_Click(null, null);
    }
    protected void btnHiddenRefresh_Click(object sender, EventArgs e)
    {

        //if (txtImportColumnHeaderRow.Text.Trim() != ""
        //    && txtImportDataStartRow.Text.Trim() != "")
        //{

        if (txtImportColumnHeaderRow.Text == "")
            txtImportColumnHeaderRow.Text = "1";

        if (txtImportDataStartRow.Text == "")
            txtImportDataStartRow.Text = "2";

            _qsTable.TempImportColumnHeaderRow = int.Parse(txtImportColumnHeaderRow.Text.Trim());
            _qsTable.TempImportDataStartRow = int.Parse(txtImportDataStartRow.Text.Trim());




            string strMsg = "";

            //SqlTransaction tn;
            //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

            //connection.Open();
            //tn = connection.BeginTransaction();

            try
            {
                int iBatchID = -1;

                Guid guidNew = (Guid)_qsBatch.UniqueName;

                string strFileExtension = "";

                switch (_qsBatch.UploadedFileName.Substring(_qsBatch.UploadedFileName.LastIndexOf('.') + 1).ToLower())
                {

                    case "dbf":
                        strFileExtension = ".dbf";
                        break;
                    case "txt":
                        strFileExtension = ".txt";
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
                    default:
                        strFileExtension = "";
                        break;
                }

                UploadManager.UploadCSV(_ObjUser.UserID, _qsTable, _qsBatch.BatchDescription,
              _qsBatch.UploadedFileName, guidNew, _strFilesPhisicalPath + "\\UserFiles\\AppFiles",
             out strMsg, out iBatchID,  strFileExtension, "",
             int.Parse(Session["AccountID"].ToString()), _qsBatch.AllowDataUpdate, _iImportTemplateID, null);

                
                if (strMsg == "")
                {
                    //tn.Commit();
                    //connection.Close();
                    //connection.Dispose();
                }
                else
                {
                    //tn.Rollback();
                    //connection.Close();
                    //connection.Dispose();

                    lblMsg.Text = strMsg;
                    return;

                }
                string strURL = "";

                string strExtra = "";
                if (_iImportTemplateID != null)
                    strExtra = "&ImportTemplateID=" + Cryptography.Encrypt(_iImportTemplateID.ToString());

                if (Request.QueryString["SearchCriteriaID"] != null)
                {
                    strURL = "~/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=" + Cryptography.Encrypt(iBatchID.ToString()) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&ColumnHeaderRow=" + txtImportColumnHeaderRow.Text + "&DataStartRow=" + txtImportDataStartRow.Text;
                }
                else
                {
                    strURL = "~/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=" + Cryptography.Encrypt(iBatchID.ToString()) + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&ColumnHeaderRow=" + txtImportColumnHeaderRow.Text + "&DataStartRow=" + txtImportDataStartRow.Text;
                }

                strURL = strURL + strExtra;


                if (_bIsRevalidate)
                {
                    strURL = strURL + "&revalidate=yes";
                }
                if (Request.QueryString["FileInfo"] != null)
                {
                    strURL = strURL + "&FileInfo=" + Request.QueryString["FileInfo"].ToString();
                }

                Response.Redirect(strURL, false);


            }
            catch (Exception ex)
            {

                //tn.Rollback();
                //connection.Close();
                //connection.Dispose();

                lblMsg.Text = strMsg;

                ErrorLog theErrorLog = new ErrorLog(null, "Record Upload", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                //throw;
            }



        //}

    }


}


