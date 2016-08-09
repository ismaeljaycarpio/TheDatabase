using System;
using System.Collections.Generic;
////using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Data;
using DocGen.DAL;

public partial class Pages_Record_TableDetail : SecurePage
{
    bool _bShowExceedances = false;
    string _strFilesPhisicalPath = "";
    private DataTable _dtDBTableTab;
    bool _bTableTabYes = false;

    Common_Pager _gvPager;
    string _strActionMode = "view";
    string _qsMode = "";
    string _qsTableID = "";
    User _ObjUser;
    bool _bFirstChangePosition = false;
    Common_Pager _gvCL_Pager;
 
    int _iCLColumnCount = 0;
    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    //string _strGridViewSortColumn = "OptionKey";
    //string _strGridViewSortDirection = "ASC";
    //Table _theTable;
    int _iCLStartIndex = 0;
    int _iCLMaxRows = 0;
    int _iCLTN = 0;
    Table _theTable;
    string _strTableCaption = "Table";
    string _strSFTID = "";
    bool _bIsAccountHolder = false;
    UserRole _CurrentUserRole = null;
    Role _CurrentRole = null;
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        //txtAttacmentPassword.Attributes["type"] = "password";
    }

    protected void lnkShowHistory_Click(object sender, EventArgs e)
    {
        lnkHideHistory.Visible = true;
        divHistory.Visible = true;
        lnkShowHistory.Visible = false;

        if (_qsTableID != "")
        {          
            BindTheChangedLogGrid(0, gvChangedLog.PageSize);
        }
    }

    protected void lnkHideHistory_Click(object sender, EventArgs e)
    {
        lnkShowHistory.Visible = true;
        lnkHideHistory.Visible = false;
        divHistory.Visible = false;
    }


    protected void chkAnonymous_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAnonymous.Checked)
        {
            divValidateUser.Visible = true;
        }
        else
        {
            divValidateUser.Visible = false;
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            Response.Redirect("~/Login.aspx", false);
            return;
        }       


        Session["Mappopup"] = null;
        Session["OptionImage"] = null;
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();
        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        {
            Response.Redirect("~/Default.aspx", false);

        }


        if (this.Master.FindControl("hfIsAccountHolder") != null)
        {
            HiddenField hfIsAccountHolder = (HiddenField)this.Master.FindControl("hfIsAccountHolder");
            if (hfIsAccountHolder != null && hfIsAccountHolder.Value != "")
            {
                _bIsAccountHolder = true;
            }
        }

        string strHelpJS = @" $(function () {
            $('#hlHelpCommon').fancybox({
                scrolling: 'auto',
                type: 'iframe',
                'transitionIn': 'elastic',
                'transitionOut': 'none',
                width: 600,
                height: 350,
                titleShow: false
            });
        });";


        ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);

        string strChildTables = @"
                    $(function () {
                            $('.popuplink').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 800,
                                height: 450,
                                titleShow: false
                            });
                        }); 
                    $(function () {
                           $('.popuprenametable').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 500,
                                height: 250,
                                titleShow: false
                            });
                        });               

                ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ChildTables", strChildTables, true);


        _ObjUser = (User)Session["User"];

        _CurrentUserRole = (UserRole)Session["UserRole"];
        _CurrentRole = SecurityManager.Role_Details((int)_CurrentUserRole.RoleID);

        string strFancy = @"
                    $(function () {
                        $("".popuplink"").fancybox({
                            scrolling: 'auto',
                            type: 'iframe',
                            width: 700,
                            height: 550,
                            titleShow: false
                        });
                    });

                  $(function () {
                            $('#hlDDEdit').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 600,
                                height: 300,
                                titleShow: false
                            });
                        });

                  $(function () {
                            $('#hlSummaryPageContent').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 900,
                                height: 600,
                                titleShow: false
                            });
                        });


                    ";


        ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);

        if(!IsPostBack)
        {
            hlTableRename.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                   Cryptography.Encrypt("Would you like to rename the menu to match the new table name?")
                   + "&okbutton=" + Cryptography.Encrypt(btnTableRenameOK.ClientID) + "&nobutton=" + Cryptography.Encrypt(btnTableRenameNo.ClientID);
        }
        

//        string strHistoryJS = @"  $(document).ready(function () {
//                                                     $('#lnkHideHistory').fadeOut(); $('#lnkShowHistory').fadeIn();
//                                                    $('#divHistory').fadeOut();
//                                                 });";
//        ScriptManager.RegisterStartupScript(this, this.GetType(), "strHistoryJS", strHistoryJS, true);


        _strTableCaption = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1),
            "Table", "Table");
        Title = _strTableCaption + " Detail";
        lblMsg.Text = "";

        if (Request.QueryString["mode"] == null)
        {          
                //Server.Transfer("~/Default.aspx");     
            Response.Redirect("~/Default.aspx", false);
            return;
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;

                if (Request.QueryString["TableID"] != null)
                {
                    _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"]);

                    if(!IsPostBack)
                        PopulateHeaderDisplay();

                    hfTableID.Value = _qsTableID;
                    hlRecords.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"];

                }

                _theTable = RecordManager.ets_Table_Details(int.Parse(_qsTableID));
                cbcvSumFilter.TableID = (int)_theTable.TableID;
            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }

        }

        _strSFTID = SystemData.SystemOption_ValueByKey_Account("Standardised_Field_Table", _theTable.AccountID, _theTable.TableID);

        string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", _theTable.AccountID, _theTable.TableID);

        if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
        {
            _bShowExceedances = true;
        }

        if (_strSFTID != "")
        {
            string strEMD = @"
                    $(function () {
                            $('.popuplinkEMD').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 1150,
                                height: 550,
                                titleShow: false
                            });
                        });                

                ";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "strEMD", strEMD, true);
        }

        if (!IsPostBack)
        {
            if(_theTable!=null)
            {
                hlSummaryPageContent.NavigateUrl = "~/Pages/Help/FancyContent.aspx?title=" +
                   Cryptography.Encrypt("Content Below Summary("+_theTable.TableName+")")
                   + "&select=" + Cryptography.Encrypt("SELECT SummaryPageContent FROM [Table] WHERE TableID="+_theTable.TableID.ToString())
                   + "&update=" + Cryptography.Encrypt("UPDATE [Table] SET SummaryPageContent='{0}' WHERE TableID="+_theTable.TableID.ToString())
                   + "&okjs=" + Cryptography.Encrypt("var chkSummaryPageContent = window.parent.document.getElementById('chkSummaryPageContent'); chkSummaryPageContent.checked = true;");
            }

            if (Request.QueryString["SearchCriteriaET"] != null)
            {
                TabContainer2.ActiveTabIndex = 3;
                hfTemplateCategory.Value = "export";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "jsRearrangeProperttyTab", "setTimeout(function () { ShowHideTemplateCategory('export',document.getElementById('" + lnkTemplateImport.ClientID + "')); }, 2000);", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideTemplateCategoryJS1", "ShowHideTemplateCategory('export',document.getElementById('" + lnkTemplateExport.ClientID + "'));", true);

            }

            if (Request.QueryString["SearchCriteriaIT"] != null)
            {
                TabContainer2.ActiveTabIndex = 3;
                hfTemplateCategory.Value = "import";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "jsRearrangeProperttyTab", "setTimeout(function () { ShowHideTemplateCategory('export',document.getElementById('" + lnkTemplateImport.ClientID + "')); }, 2000);", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideTemplateCategoryJS1", "ShowHideTemplateCategory('import',document.getElementById('" + lnkTemplateImport.ClientID + "'));", true);

            }

            if (_strSFTID != "" && !Common.HaveAccess(Session["roletype"].ToString(), "1") && _theTable!=null)
            {

                if (_strSFTID == _theTable.TableID.ToString())
                    {
                        Response.Redirect("~/Default.aspx", true);
                        return;
                    }
                
            }

        }


        _dtDBTableTab = Common.DataTableFromText("SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + " AND DisplayOrder<>0 ORDER BY DisplayOrder");

        if (_dtDBTableTab != null)
        {
            if (_dtDBTableTab.Rows.Count > 0)
            {

                _bTableTabYes = true;
                gvTheGrid.Columns[8].Visible = true;
            }
        }


        if (_strActionMode.ToLower() == "add")
        {

            //string strTable = "Table";
            //strTable = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", strTable);


            lblTitle.Text = "Add " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " Options";
            Title = lblTitle.Text;

            divRecords.Visible = false;
        }
        else
        {
            //trFilter.Visible = true;
            trReasonChange.Visible = true;
            //trChangeHistory.Visible = true;
            trAnonymousUser.Visible = true;        
           
            hplAddChildTable.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/ChildTableDetail.aspx?ParentTableID=" + _qsTableID;
            hlAddTemplates.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/DocTemplateDetail.aspx?TableID=" + _qsTableID;

        }

        try
        {
            if(IsPostBack)
            {
                 if (_strActionMode.ToLower() != "add")
                 {
                     ScriptManager.RegisterStartupScript(this, this.GetType(), "jsRearrangeProperttyTab", "ShowHideCategory('display',document.getElementById('" + lnkPropertyDisplay.ClientID + "'));", true);
                     //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideTemplateCategoryJS2", "ShowHideTemplateCategory('import',document.getElementById('" + lnkTemplateImport.ClientID + "'));", true);

                 }
            }

            if (!IsPostBack)
            {

                //ViewState["prepage"] = Request.UrlReferrer.ToString();


                //txtAttacmentPassword.Attributes["type"] = "password";





                PopulateTerminology();

                if (_strActionMode.ToLower() != "add")
                {

                    lnkPropertyDisplay.Attributes.Add("onclick", "ShowHideCategory('display',this); return false");
                    lnkPropertyDisplay.CssClass = "TablLinkClass";

                    lnkPropertyImportData.Attributes.Add("onclick", "ShowHideCategory('importdata',this); return false");
                    lnkPropertyImportData.CssClass = "TablLinkClass";

                    lnkPropertyAddRecords.Attributes.Add("onclick", "ShowHideCategory('addrecords',this); return false");
                    lnkPropertyAddRecords.CssClass = "TablLinkClass";

                    lnkColours.Attributes.Add("onclick", "ShowHideCategory('colours',this); return false");
                    lnkColours.CssClass = "TablLinkClass";

                    lnkGraphs.Attributes.Add("onclick", "ShowHideCategory('graphs',this); return false");
                    lnkGraphs.CssClass = "TablLinkClass";

                    lnkEmails.Attributes.Add("onclick", "ShowHideCategory('emails',this); return false");
                    lnkEmails.CssClass = "TablLinkClass";
                    //template tab
                    lnkTemplateImport.Attributes.Add("onclick", "ShowHideTemplateCategory('import',this); return false");
                    lnkTemplateImport.CssClass = "TemplateTablLinkClass";

                    lnkTemplateExport.Attributes.Add("onclick", "ShowHideTemplateCategory('export',this); return false");
                    lnkTemplateExport.CssClass = "TemplateTablLinkClass";

                    lnkTemplateWordMerge.Attributes.Add("onclick", "ShowHideTemplateCategory('word',this); return false");
                    lnkTemplateWordMerge.CssClass = "TemplateTablLinkClass";


                    //if (SystemData.SystemOption_ValueByKey_Account("EmailAttachments", null, _theTable == null ? null : _theTable.TableID) == "Yes")
                    //{
                    //    TabContainer2.Tabs[5].Visible = true;
                    //    PopulateOutSavetoTable();
                    //    PopulateInSavetoTable();
                    //    PopulateIncomingIdentifier();
                    //}
                    //else
                    //{
                    //    TabContainer2.Tabs[5].Visible = false;
                    //}


                    PopulateSortColumnDDL();
                    PopulateGraphXAxisColumnDDL();
                    PopulateGraphYAxisColumnDDL();
                    PopulateGraphSeriesColumnDDL();

                    PopulateAddUserUserColumnID();
                    PopulateAddUserPasswordColumnID();
                    PopulateUniqueColumns();
                   
                    PopulateTabs();
                    
                    //PopulateYAxis();
                    //PopulateValidateColumn();
                    System.Web.UI.WebControls.ListItem iNone = new System.Web.UI.WebControls.ListItem("--None--", "");
                    ddlValidateColumnID1.Items.Insert(0, iNone);

                    System.Web.UI.WebControls.ListItem iNone2 = new System.Web.UI.WebControls.ListItem("--None--", "");
                    ddlValidateColumnID2.Items.Insert(0, iNone2);
                   

                    //hlMenuEdit.Attributes.Add("Target", "_blank");

                }

                //chkAddMissingLocation.Text = "Add Missing " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Locations", "Locations");

                hfWebroot.Value = "http://" + Request.Url.Authority + Request.ApplicationPath;


                //PopulateRecordGroupDDL(int.Parse(Session["AccountID"].ToString()));

                hlAddNewField.NavigateUrl = GetAddURL();

                if (_strSFTID != "")
                {
                    hlAddNewField.CssClass = "popuplinkEMD";

                }

                if (Request.QueryString["fromsheet"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Congratulations! " + _theTable.TableName+ " has been created. The field attributes have been defaulted but you can adjust them if you like.');", true);
                }


                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableList.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }
                else
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableList.aspx?MenuID=" + Request.QueryString["MenuID"];

                    if (Request.UrlReferrer != null)
                    {
                        if (Request.UrlReferrer.ToString().IndexOf("RecordList.aspx") > -1)
                        {                           
                            hlBack.NavigateUrl = Request.UrlReferrer.ToString();
                        }
                        else if (Request.UrlReferrer.ToString().IndexOf("TableSheet.aspx") > -1)
                        {
                            //importinfo
                            if (Request.QueryString["fromsheet"] != null)
                            {
                                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableList.aspx?MenuID=" + Request.QueryString["MenuID"];
                            }
                            else
                            {
                                hlBack.NavigateUrl = Request.UrlReferrer.ToString();
                            }
                        }
                        else
                        {

                            hlBack.NavigateUrl = Request.UrlReferrer.ToString();
                        }

                    }

                }

                if (Request.QueryString["template"] != null)
                {
                    if (Request.UrlReferrer != null)
                    {
                        hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Template/TableList.aspx";
                    }

                }

                if (hlBack.NavigateUrl.IndexOf("TableList.aspx") > -1)
                {
                    hlBack.Text = "<strong>< " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Tables", "Tables") + "</strong>";
                }

              
                if (Request.QueryString["MenuID"] != null)
                {

                    if (Request.QueryString["SearchCriteria2"] != null)
                    {
                        PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria2"].ToString())));
                    }

                    if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                    {

                        gvChangedLog.PageSize = int.Parse(Session["GridPageSize"].ToString());

                    }



                    if (Request.QueryString["SearchCriteria2"] != null)
                    {
                        //gvTheGrid.PageSize = _iMaxRows;

                        BindTheGrid(_iStartIndex, _iMaxRows);
                    }
                    else
                    {
                        if (_strActionMode.ToLower() != "add")
                        {
                            BindTheGrid(0, gvTheGrid.PageSize);
                        }
                    }



                    if (_strActionMode.ToLower() != "add")
                    {
                        //BindTheChangedLogGrid(0, gvChangedLog.PageSize);
                    }


                }

                if (_strActionMode.ToLower() != "add")
                {
                    PopulateUserDropDown();
                }



            }
            else
            {
                if (_strActionMode.ToLower() != "add")
                {
                    GridViewRow gvr = gvTheGrid.TopPagerRow;
                    if (gvr != null)
                        _gvPager = (Common_Pager)gvr.FindControl("Pager");


                    GridViewRow gvrCL = gvChangedLog.TopPagerRow;
                    if (gvrCL != null)
                        _gvCL_Pager = (Common_Pager)gvrCL.FindControl("CL_Pager");

                    //_ViewItemPager
                }
            }


            switch (_strActionMode.ToLower())
            {
                case "view":

                    if (!IsPostBack)
                    {
                        PopulateTheRecord();
                        EnableTheRecordControls(false);

                        //hlProperties.NavigateUrl = GetViewTableURL().Replace("TableDetail.aspx", "TableProperty.aspx");

                    }

                    break;

                case "edit":

                    if (!IsPostBack)
                    {

                        if (Request.QueryString["signup"] != null)
                        {
                            hlFinished.NavigateUrl = "~/Default.aspx";
                            tblButtons.Visible = false;
                            divFinished.Visible = true;
                            pInstruction.Visible = true;
                        }

                        PopulateTheRecord();
                        //hlProperties.NavigateUrl = GetEditTableURL().Replace("TableDetail.aspx", "TableProperty.aspx");
                    }
                    break;

            }


         

            if (!IsPostBack)
            {

                lnkSave.Focus();
                //if (Request.QueryString["MenuID"] != null)
                //{
                //    if (Cryptography.Decrypt(Request.QueryString["MenuID"]) != "-1")
                //    {
                //        ddlMenu.Text = Cryptography.Decrypt(Request.QueryString["MenuID"]);
                //    }

                //}

                hlAddFormSet.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&TableID=" + Request.QueryString["TableID"].ToString();


            }

           
           

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void gvChangedLog_PreRender(object sender, EventArgs e)
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

    protected void gvChangedLog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton lb = (ImageButton)e.Row.FindControl("btnView");

                HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");
                DateTime dtUpdateDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "DateAdded"));
                //lb.Attributes.Add("onclick", "javascript:OpenAuditDetail('" + dtUpdateDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "','" + _iRecordID.ToString() + "');return false;");
                hlView.NavigateUrl = "TableAudit.aspx?UpdatedDate=" + Server.UrlEncode(dtUpdateDate.ToString("yyyy-MM-dd HH:mm:ss.fff")) + "&TableID=" + _qsTableID;
            }

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }




    protected void BindTheChangedLogGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
            int iTN = 0;

            //gvChangedLog.DataSource = RecordManager.ets_Record_Changes_Select(
            //        (int)_iRecordID,int.Parse(_qsTableID), iStartIndex, iMaxRows, ref  iTN, ref _iCLColumnCount);


            gvChangedLog.DataSource = RecordManager.Table_Audit_Summary(
                   int.Parse(_qsTableID), iStartIndex, iMaxRows, ref  iTN);

            gvChangedLog.VirtualItemCount = iTN;

            //_iCLColumnCount = 4;

            _iCLStartIndex = iStartIndex;
            _iCLMaxRows = iMaxRows;
            _iCLTN = iTN;


            gvChangedLog.DataBind();
            if (gvChangedLog.TopPagerRow != null)
                gvChangedLog.TopPagerRow.Visible = true;
            GridViewRow gvr = gvChangedLog.TopPagerRow;
            if (gvr != null)
            {
                _gvCL_Pager = (Common_Pager)gvr.FindControl("CL_Pager");

            }


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table Change Log", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }




    protected void CL_Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheChangedLogGrid(_gvCL_Pager.StartIndex, _gvCL_Pager._gridView.PageSize);
    }

   

    protected void CL_Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvCL_Pager.ExportFileName = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " Change Log ";
        BindTheChangedLogGrid(0, _gvCL_Pager.TotalRows);
    }
    protected void CL_Pager_OnApplyFilter(object sender, EventArgs e)
    {
        //_gvCL_Pager.ExportFileName = "Sensor Change Log";
        BindTheChangedLogGrid(0, gvChangedLog.PageSize);
    }


    protected void CL_Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvChangedLog.AllowPaging = false;
        BindTheChangedLogGrid(0, _gvCL_Pager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=SensorChangedLog.csv");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        DataTable dtTemp = (DataTable)gvChangedLog.DataSource;

        int iColCount = dtTemp.Columns.Count;
        for (int i = 0; i < iColCount - 1; i++)
        {
            if (string.IsNullOrEmpty(dtTemp.Columns[i].ColumnName))
            {
            }
            else
            {
                sw.Write(dtTemp.Columns[i].ColumnName);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
        }

        sw.Write(sw.NewLine);

        // Now write all the rows.
        foreach (DataRow dr in dtTemp.Rows)
        {

            for (int i = 0; i < iColCount - 1; i++)
            {
                if (string.IsNullOrEmpty(dtTemp.Columns[i].ColumnName))
                {
                }
                else
                {


                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write("\"" + dr[i].ToString() + "\"");
                    }




                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();


        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }

    protected void EnableTheRecordControls(bool p_bEnable)
    {

        gvTheGrid.Columns[1].Visible = p_bEnable;
        gvTheGrid.Columns[2].Visible = p_bEnable;
        gvTheGrid.Columns[12].Visible = p_bEnable;


        ddlHeaderText.Enabled = p_bEnable;
        ddlDataUpdateUniqueColumnID.Enabled = false;
        txtHeaderColor.Enabled = p_bEnable;

        divUserAdd.Visible = p_bEnable;

        txtTable.Enabled = p_bEnable;

        chkIsPosition.Enabled = p_bEnable;
        ddlPinImages.Visible = p_bEnable;
        txtMaxTimeBetweenRecords.Enabled = p_bEnable;
        txtLateDataDays.Enabled = p_bEnable;

        txtImportDataStartRow.Enabled = p_bEnable;
        txtImportColumnHeaderRow.Enabled = p_bEnable;
        //chkAddMissingLocation.Enabled = p_bEnable;
        ddlMaxTimeBetweenRecordsUnit.Enabled = p_bEnable;

        //hlMenuEdit.Visible = false;

        //ddlMenu.Enabled = p_bEnable;
        chkUniqueRecordedate.Enabled = p_bEnable;

        ddlUniqueColumnID.Enabled = p_bEnable;
        ddlUniqueColumnID2.Enabled = p_bEnable;

        chkFlashAlerts.Enabled = p_bEnable;
        chkNavigationArrows.Enabled = p_bEnable;
        chkSaveAndAdd.Enabled = p_bEnable;
        ddlReasonChange.Enabled = p_bEnable;
        ddlChangeHistory.Enabled = p_bEnable;
        chkAnonymous.Enabled = p_bEnable;
        ddlParentTable.Enabled = p_bEnable;


        chkAddUserRecord.Enabled = p_bEnable;
        chkDataUpdateUniqueColumnID.Enabled = false;
        ddlAddUserUserColumnID.Enabled = p_bEnable;
        ddlSortColumn.Enabled = p_bEnable;
        ddlAddUserPasswordColumnID.Enabled = p_bEnable;
        chkAddUserNotification.Enabled = p_bEnable;

        ddlGraphXAxisColumnID.Enabled = p_bEnable;
        ddlGraphSeriesColumnID.Enabled = p_bEnable;
        ddlDefaultGraphPeriod.Enabled = p_bEnable;
    }
    protected void PopulateAddUserUserColumnID()
    {
        ddlAddUserUserColumnID.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE 
            ColumnType='text' AND TextType='email' AND IsStandard=0 AND TableID=" + _qsTableID);

        ddlAddUserUserColumnID.DataBind();
    }


    protected void PopulateUniqueColumns()
    {
        DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE 
            ColumnType<>'staticcontent' AND SystemName not in ('RecordID','TableID','EnteredBy','IsActive')  AND TableID=" + _qsTableID);


        ddlUniqueColumnID.DataSource = dtTemp;
        ddlUniqueColumnID.DataBind();

        ListItem liSelect = new ListItem("--Please Select--", "");
        ddlUniqueColumnID.Items.Insert(0, liSelect);

        ddlUniqueColumnID2.DataSource = dtTemp;
        ddlUniqueColumnID2.DataBind();

        ListItem liSelect2 = new ListItem("--Please Select--", "");
        ddlUniqueColumnID2.Items.Insert(0, liSelect2);

    }

    protected void PopulateSortColumnDDL()
    {
        ddlSortColumn.Items.Clear();
        ddlSortColumn.DataSource = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
            IsStandard=0 AND TableID=" + _qsTableID);

        ddlSortColumn.DataBind();
        ListItem liSeletec = new ListItem("-- Please Select --", "");

        ddlSortColumn.Items.Insert(0,liSeletec);
    }

    protected void PopulateGraphXAxisColumnDDL()
    {
        ddlGraphXAxisColumnID.Items.Clear();
        ddlGraphXAxisColumnID.DataSource = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
            IsStandard = 0 AND TableID = " + _qsTableID);

        ddlGraphXAxisColumnID.DataBind();
        ListItem liSeletec = new ListItem("-- Please Select --", "");

        ddlGraphXAxisColumnID.Items.Insert(0, liSeletec);
    }

    protected void PopulateGraphYAxisColumnDDL()
    {
        ddlGraphDefaultYAxisColumnID.Items.Clear();
        ddlGraphDefaultYAxisColumnID.DataSource = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
            IsStandard = 0 AND TableID = " + _qsTableID + " AND ColumnType IN ('number','calculation')");

        ddlGraphDefaultYAxisColumnID.DataBind();
        ListItem liSeletec = new ListItem("-- Please Select --", "");

        ddlGraphDefaultYAxisColumnID.Items.Insert(0, liSeletec);
    }

    protected void PopulateGraphSeriesColumnDDL()
    {
        ddlGraphSeriesColumnID.Items.Clear();
        ddlGraphSeriesColumnID.DataSource = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
            IsStandard = 0 AND TableID = " + _qsTableID);

        ddlGraphSeriesColumnID.DataBind();
        ListItem liSeletec = new ListItem("-- Please Select --", "");

        ddlGraphSeriesColumnID.Items.Insert(0, liSeletec);
    }


    protected void PopulateAddUserPasswordColumnID()
    {
        ddlAddUserPasswordColumnID.DataSource = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
            ColumnType='text' AND IsStandard=0 AND TableID=" + _qsTableID);

        ddlAddUserPasswordColumnID.DataBind();
    }

    //protected void PopulateMenuDDL(int iAccountID)
    //{
    //    ddlMenu.Items.Clear();
    //    int iTemp = 0;
    //    List<Menu> lstMenuSelect = RecordManager.ets_Menu_Select(null, string.Empty, null,
    //        iAccountID, true,
    //        "Menu", "ASC", null, null, ref iTemp,null,null);

    //    string strNone = "";
    //    foreach (Menu aMenu in lstMenuSelect)
    //    {
    //        if (aMenu.MenuP == "--None--")
    //        {
    //            strNone = aMenu.MenuID.ToString();
    //            lstMenuSelect.Remove(aMenu);
    //            break;
    //        }
    //    }


    //    ddlMenu.DataSource = lstMenuSelect;
    //    ddlMenu.DataBind();

    //    System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("--None--", strNone);
    //    ddlMenu.Items.Insert(0, liNone);
    //    System.Web.UI.WebControls.ListItem liNew = new System.Web.UI.WebControls.ListItem("--New--", "new");
    //    ddlMenu.Items.Insert(1, liNew);


    //}

    //protected void PopulateMenuDDL(int iAccountID)
    //{
    //    //ddlMenu.Items.Clear();
    //    int iTemp = 0;
    //    List<Menu> lstMenuSelect = RecordManager.ets_Menu_Select(null, string.Empty, null,
    //        iAccountID, true,
    //        "Menu", "ASC", null, null, ref iTemp, null, null);

    //    string strNone = "";
    //    foreach (Menu aMenu in lstMenuSelect)
    //    {
    //        if (aMenu.MenuP == "--None--" && aMenu.ParentMenuID==null)
    //        {
    //            strNone = aMenu.MenuID.ToString();
    //            //lstMenuSelect.Remove(aMenu);
    //            break;
    //        }
    //    }


    //    //ddlMenu.DataSource = lstMenuSelect;
    //    //ddlMenu.DataBind();

    //    TheDatabaseS.PopulateMenuDDL(ref ddlMenu);

    //    bool bNoneFound = false;
    //    if(strNone!="" && ddlMenu.Items.FindByValue(strNone)!=null)
    //    {
    //        ddlMenu.Items.Remove(ddlMenu.Items.FindByValue(strNone));

    //        System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("--None--", strNone);
    //        ddlMenu.Items.Insert(0, liNone);
    //        bNoneFound = true;

    //    }



    //    int i = 0;
    //    if (bNoneFound)
    //        i = 1;

    //    System.Web.UI.WebControls.ListItem liNew = new System.Web.UI.WebControls.ListItem("--New--", "new");
    //    ddlMenu.Items.Insert(i, liNew);


    //}
  
    public string GetAddURL()
    {
        string strURL = "";

        if (_strSFTID == "")
        {
            if (Request.QueryString["SearchCriteria"] != null)
            {
                strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordColumnDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&typemode=" + Cryptography.Encrypt(_strActionMode) +  "&TableID=" + Cryptography.Encrypt(hfTableID.Value) + "&SearchCriteria2=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {
                strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordColumnDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&typemode=" + Cryptography.Encrypt(_strActionMode) + "&TableID=" + Cryptography.Encrypt(hfTableID.Value) + "&SearchCriteria2=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());
            }

            if (Request.QueryString["signup"] != null)
                strURL = strURL + "&signup=yes";
        }
        else
        {
            strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/EMD/TemplateColumnDetail.aspx?TableID=" + Cryptography.Encrypt(hfTableID.Value) ;

        }

        return strURL;
    }

    public string GetEditURL()
    {
        string strURL = "";
        if (Request.QueryString["SearchCriteria"] != null)
        {
            strURL= "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordColumnDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&typemode=" + Cryptography.Encrypt(_strActionMode) +  "&SearchCriteria2=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&ColumnID=";
        }
        else
        {
            strURL= "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordColumnDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&typemode=" + Cryptography.Encrypt(_strActionMode) +  "&SearchCriteria2=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&ColumnID=";
        }

        if (Request.QueryString["signup"] != null)
            strURL = strURL.Replace("RecordColumnDetail.aspx?", "RecordColumnDetail.aspx?signup=yes&");

        return strURL;

    }


    public string GetEditTableURL()
    {
        if (Request.QueryString["SearchCriteria"] != null)
        {
            return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") +"&TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "#topline";
        }
        else
        {
            return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "#topline";
        }



    }

    public string GetViewTableURL()
    {
        if (Request.QueryString["SearchCriteria"] != null)
        {
            return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "#topline";
        }
        else
        {
            return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("view") +  "&TableID=" + Request.QueryString["TableID"].ToString() + "#topline";
        }



    }


    public string GetViewURL()
    {
        string strURL = "";
        if (Request.QueryString["template"] != null)
        {
            strURL= "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordColumnDetail.aspx?template=trure&mode=" + Cryptography.Encrypt("view") + "&typemode=" + Cryptography.Encrypt(_strActionMode) +  "&SearchCriteria2=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&ColumnID=";
            
        }
        else
        {

            if (Request.QueryString["SearchCriteria"] != null)
            {
                strURL= "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordColumnDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&typemode=" + Cryptography.Encrypt(_strActionMode)   + "&SearchCriteria2=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&ColumnID=";
            }
            else
            {
                strURL= "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordColumnDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&typemode=" + Cryptography.Encrypt(_strActionMode)  + "&SearchCriteria2=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&ColumnID=";
            }
        }

        if (Request.QueryString["signup"] != null)
            strURL = strURL.Replace("RecordColumnDetail.aspx?", "RecordColumnDetail.aspx?signup=yes&");

        return strURL;
    }



    protected void PopulateTheRecord()
    {
        try
        {
            Table theTable = RecordManager.ets_Table_Details(int.Parse(hfTableID.Value));


            if (!IsPostBack)
            {
                PopulateImportTemplate((int)theTable.TableID);
                if (theTable!=null)
                    lblTableID.Text = "Table ID: " + theTable.TableID.ToString();
                hlTableTabs.NavigateUrl = "~/Pages/Record/TableTabList.aspx?TableID=" + Cryptography.Encrypt( theTable.TableID.ToString());
            }
            //string strTable = "Table";
            //strTable = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", strTable);
            lblTitle.Text = "Configure " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + ": " + theTable.TableName;
            Title = lblTitle.Text;

            //if (theTable.MenuID != null)
            //    hfMenuID.Value = theTable.MenuID.ToString();

            //Menu theRecordGroup = RecordManager.ets_Menu_Details((int)theTable.MenuID);

            //PopulateFormSet();
            if (!IsPostBack)
            {
                if (Request.QueryString["SearchCriteria2"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria2"].ToString())));
                }
                else
                {
                    if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                    { gvTheGrid.PageSize = 550; }
                }
            }


            BindTheGrid(_iStartIndex, gvTheGrid.PageSize);
            

            if(theTable.AllowCopyRecords!=null && (bool)theTable.AllowCopyRecords)
            {
                gvTheGrid.Columns[14].Visible = true;
            }
            else
            {
                gvTheGrid.Columns[14].Visible = false;
            }

            if (_strActionMode == "edit")
            {
                ViewState["theTable"] = theTable;
                if (theTable.IsActive == true)
                {
                    divUnDelete.Visible = false;
                }
                else
                {
                    divDelete.Visible = false;
                }

                //Title = "Edit " + _strTableCaption + " - " + theTable.TableName;
                //lblTitle.Text = "Edit " + _strTableCaption + " - " + theTable.TableName;
            }
            else if (_strActionMode == "view")
            {
                divDelete.Visible = false;
                divUnDelete.Visible = false;
                //Title = _strTableCaption + " - " + theTable.TableName;
                //lblTitle.Text = _strTableCaption + " - " + theTable.TableName;
            }
            PopulateSTUserGrid();



            if (Request.QueryString["template"] != null)
            {
                divEdit.Visible = false;
                //TabContainer1.Visible = false;
                divRecords.Visible = false;
                //divSave.Visible = false;
                divUnDelete.Visible = false;
                divDelete.Visible = false;

            }


            PopulateParentTable((int)theTable.TableID);

            //txtHeaderName.Text = theTable.HeaderName;
            if(theTable.SummaryPageContent!="")
            {
                chkSummaryPageContent.Checked = true;
            }
            else
            {
                chkSummaryPageContent.Checked = false;
            }
            hfDisplayColumnsFormula.Value = theTable.HeaderName;
            txtHeaderColor.Text = theTable.HeaderColor;
            txtTabColour.Text = theTable.TabColour;
            txtTabTextColour.Text = theTable.TabTextColour;
            txtFilterTopColour.Text = theTable.FilterTopColour;
            txtFilterBottomColour.Text = theTable.FilterBottomColour;
            if (theTable.BoxAroundField != null)
            {
                chkBoxAroundField.Checked = (bool)theTable.BoxAroundField;
            }
            if (theTable.ShowEditAfterAdd != null)
            {
                chkShowEditAfterAdd.Checked = (bool)theTable.ShowEditAfterAdd;
            }
            if (theTable.AllowCopyRecords != null)
            {
                chkAllowCopyRecords.Checked = (bool)theTable.AllowCopyRecords;
            }

            if (theTable.ShowSentEmails != null)
                chkShowSentEmails.Checked = (bool)theTable.ShowSentEmails;


            if (theTable.ShowReceivedEmails != null)
                chkShowReceivedEmails.Checked = (bool)theTable.ShowReceivedEmails;


            if (theTable.SaveAndAdd != null)
            {
                chkSaveAndAdd.Checked = (bool)theTable.SaveAndAdd;
            }

            if (theTable.ShowTabVertically != null)
                chkShowTabVertically.Checked = (bool)theTable.ShowTabVertically;


            if (theTable.CopyToChildrenAfterImport != null)
                chkCopyToChildTables.Checked = (bool)theTable.CopyToChildrenAfterImport;

            if(theTable.CustomUploadSheet!="")
            {
                try
                {
                    string strFilePath = _strFilesPhisicalPath + "\\UserFiles\\Template\\" + _theTable.CustomUploadSheet;
                    if (File.Exists(strFilePath))
                    {

                        hlCustomUploadSheet.Visible = true;
                        hlCustomUploadSheet.Text = (_theTable.CustomUploadSheet.Substring(_theTable.CustomUploadSheet.IndexOf("_") + 1)).ToString();
                    }
                }
                catch
                {
                    //
                }
            }

            if (theTable.FilterType != "")
                ddlFilterType.SelectedValue = theTable.FilterType;

            if (theTable.PinDisplayOrder != null)
                txtPinDisplayOrder.Text = theTable.PinDisplayOrder.ToString();

            if(theTable.DataUpdateUniqueColumnID!=null)
            {
                chkDataUpdateUniqueColumnID.Checked = true;
                ddlDataUpdateUniqueColumnID.SelectedValue = theTable.DataUpdateUniqueColumnID.ToString();
            }

            if (theTable.HeaderName != "")
            {
                string strDisplayName = theTable.HeaderName.Replace("[", "");

                if (strDisplayName.IndexOf("[", 0) == -1)
                {
                    //single Field
                    strDisplayName = strDisplayName.Replace("]", "");
                    strDisplayName = strDisplayName.Replace("'", "''");
                    DataTable dtTempSC = Common.DataTableFromText("SELECT * FROM [Column] WHERE   TableID=" + _qsTableID + " AND DisplayName='" + strDisplayName + "'");

                    if (dtTempSC.Rows.Count > 0)
                    {
                        ddlHeaderText.SelectedValue = dtTempSC.Rows[0]["ColumnID"].ToString();
                    }

                }
                else
                {
                    //advanced
                    //do nothing
                }

            }

            if (theTable.ParentTableID != null)
            {
                ddlParentTable.Text = theTable.ParentTableID.ToString();
                PopulateValidateColumn();
            }
            txtTable.Text = theTable.TableName;

            if (theTable.ReasonChangeType != "")
                ddlReasonChange.SelectedValue = theTable.ReasonChangeType;

            if (theTable.ChangeHistoryType != "")
                ddlChangeHistory.SelectedValue = theTable.ChangeHistoryType;

            if (theTable.AddWithoutLogin != null)
            {
                chkAnonymous.Checked = (bool)theTable.AddWithoutLogin;

                if (chkAnonymous.Checked)
                {
                    divValidateUser.Visible = true;
                }
                else
                {
                    divValidateUser.Visible = false;
                }
               
                if (theTable.ValidateColumnID1 != null)
                {
                    if (ddlValidateColumnID1.Items.FindByValue(theTable.ValidateColumnID1.ToString()) != null)
                        ddlValidateColumnID1.SelectedValue = theTable.ValidateColumnID1.ToString();
                }
                if (theTable.ValidateColumnID2 != null)
                {
                    if (ddlValidateColumnID2.Items.FindByValue(theTable.ValidateColumnID2.ToString()) != null)
                        ddlValidateColumnID2.SelectedValue = theTable.ValidateColumnID2.ToString();
                }
            }

            hlPublicFormURL.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Public.aspx?TableID=" + theTable.TableID.ToString();
            hlPublicFormURL.Text = hlPublicFormURL.NavigateUrl;
            if (theTable.IsImportPositional != null)
                chkIsPosition.Checked = (bool)theTable.IsImportPositional;

            //if (theTable.MenuID!=null)
            //    hfMenuID.Value = theTable.MenuID.ToString();

            //Menu theRecordGroup = RecordManager.ets_Menu_Details((int)theTable.MenuID);

            //ddlAccount.Text=theRecordGroup.AccountID.ToString();
            //PopulateMenuDDL((int)theRecordGroup.AccountID);
            //PopulateMenuDDL((int)theTable.AccountID);

            //if (theTable.MenuID != null)
            //ddlMenu.Text = theTable.MenuID.ToString();

            if (theTable.MaxTimeBetweenRecordsUnit != null)
            {
                txtMaxTimeBetweenRecords.Text = theTable.MaxTimeBetweenRecords.ToString();
                ddlMaxTimeBetweenRecordsUnit.Text = theTable.MaxTimeBetweenRecordsUnit;
            }
            if (theTable.LateDataDays != null)
            {
                txtLateDataDays.Text = theTable.LateDataDays.ToString();
            }

            //if (theTable.IsRecordDateUnique != null)
            //    chkUniqueRecordedate.Checked = (bool)theTable.IsRecordDateUnique;


            if (theTable.UniqueColumnID != null || theTable.UniqueColumnID2 != null)
                chkUniqueRecordedate.Checked = true;

            if(theTable.UniqueColumnID!=null)
            {
                if (ddlUniqueColumnID.Items.FindByValue(theTable.UniqueColumnID.ToString()) != null)
                    ddlUniqueColumnID.SelectedValue = theTable.UniqueColumnID.ToString();
            }

            if (theTable.UniqueColumnID2 != null)
            {
                if (ddlUniqueColumnID2.Items.FindByValue(theTable.UniqueColumnID2.ToString()) != null)
                    ddlUniqueColumnID2.SelectedValue = theTable.UniqueColumnID2.ToString();
            }

            if (theTable.IsDataUpdateAllowed.HasValue && theTable.IsDataUpdateAllowed.Value)
                chkDataUpdateUniqueColumnID.Checked = true;

            if (theTable.ImportDataStartRow != null)
            {
                txtImportDataStartRow.Text = theTable.ImportDataStartRow.ToString();
            }
            if(theTable.DefaultImportTemplateID!=null)
            {
                if (ddlTemplate.Items.FindByValue(theTable.DefaultImportTemplateID.ToString()) != null)
                    ddlTemplate.SelectedValue = theTable.DefaultImportTemplateID.ToString();
            }


            if (theTable.ImportColumnHeaderRow != null)
            {
                txtImportColumnHeaderRow.Text = theTable.ImportColumnHeaderRow.ToString();
            }
            //if (theTable.AddMissingLocation != null)
            //{
            //    chkAddMissingLocation.Checked = (bool)theTable.AddMissingLocation;
            //}

            if (theTable.FlashAlerts != null)
            {
                chkFlashAlerts.Checked = (bool)theTable.FlashAlerts;
            }
            if (theTable.NavigationArrows != null)
            {
                chkNavigationArrows.Checked = (bool)theTable.NavigationArrows;
            }

            //if (theTable.SaveAndAdd != null)
            //{
            //    chkSaveAndAdd.Checked = (bool)theTable.SaveAndAdd;
            //}
            //txtPinImage.Text = theTable.PinImage;

            if (theTable.PinImage != null)
            {
                if (theTable.PinImage != "")
                {
                    imgPIN.ImageUrl = "~/" + theTable.PinImage;
                    try
                    {
                        ddlPinImages.Text = theTable.PinImage;
                    }
                    catch (Exception ex)
                    {
                        ddlPinImages.SelectedIndex = 0;
                        //
                    }
                }
            }

            if (theTable.FilterColumnID != null)
            {
                //ddlYAxis.SelectedValue = theTable.FilterColumnID.ToString();
                cbcvSumFilter.ddlYAxisV = theTable.FilterColumnID.ToString();
                //ddlYAxis_SelectedIndexChanged(null, null);

                if (theTable.FilterDefaultValue != null && theTable.FilterDefaultValue != "")
                {
                    //ddlFilterValue.SelectedValue = theTable.FilterDefaultValue;
                    cbcvSumFilter.SetValue = theTable.FilterDefaultValue;
                }
            }
            if (theTable.HideAdvancedOption != null)
            {
                chkShowAdvancedOptions.Checked = !(bool)theTable.HideAdvancedOption;
            }

            if (theTable.HideFilter != null)
            {
                chkHideFilter.Checked = (bool)theTable.HideFilter;
            }

            if (theTable.AddUserRecord != null)
            {
                chkAddUserRecord.Checked = (bool)theTable.AddUserRecord;
                if(theTable.AddUserUserColumnID!=null)
                    ddlAddUserUserColumnID.Text = theTable.AddUserUserColumnID.ToString();
                if (theTable.AddUserPasswordColumnID != null)
                    ddlAddUserPasswordColumnID.Text = theTable.AddUserPasswordColumnID.ToString();

                if (theTable.AddUserNotification != null)
                    chkAddUserNotification.Checked = (bool)theTable.AddUserNotification;
                
            }
            if (theTable.SortColumnID != null)
            {
                if (ddlSortColumn.Items.FindByValue(theTable.SortColumnID.ToString()) != null)
                    ddlSortColumn.SelectedValue = theTable.SortColumnID.ToString();

            }

            if (theTable.GraphXAxisColumnID != null)
            {
                if (ddlGraphXAxisColumnID.Items.FindByValue(theTable.GraphXAxisColumnID.ToString()) != null)
                    ddlGraphXAxisColumnID.SelectedValue = theTable.GraphXAxisColumnID.ToString();

            }
            if (theTable.GraphSeriesColumnID != null)
            {
                if (ddlGraphSeriesColumnID.Items.FindByValue(theTable.GraphSeriesColumnID.ToString()) != null)
                    ddlGraphSeriesColumnID.SelectedValue = theTable.GraphSeriesColumnID.ToString();

            }
            if (theTable.GraphDefaultPeriod != null)
            {
                if (ddlDefaultGraphPeriod.Items.FindByValue(theTable.GraphDefaultPeriod.ToString()) != null)
                    ddlDefaultGraphPeriod.SelectedValue = theTable.GraphDefaultPeriod.ToString();

            }

            if (theTable.GraphDefaultYAxisColumnID != null)
            {
                if (ddlGraphDefaultYAxisColumnID.Items.FindByValue(theTable.GraphDefaultYAxisColumnID.ToString()) != null)
                    ddlGraphDefaultYAxisColumnID.SelectedValue = theTable.GraphDefaultYAxisColumnID.ToString();
            }

            if (theTable.GraphOnStart != "")
            {
                if (ddlGraphOnStart.Items.FindByValue(theTable.GraphOnStart) != null)
                    ddlGraphOnStart.SelectedValue = theTable.GraphOnStart;

            }
           
            if(_bIsAccountHolder==false)
            {
                 divDelete.Visible=false;
                if(_CurrentRole!=null && _CurrentRole.RoleType=="2" )
                {
                    if(_CurrentUserRole!=null && _CurrentUserRole.AllowDeleteTable!=null && (bool)_CurrentUserRole.AllowDeleteTable)
                    {
                        divDelete.Visible = true;
                    }
                }

            }
            //if (SystemData.SystemOption_ValueByKey_Account("EmailAttachments",null,_theTable==null?null:_theTable.TableID) == "Yes")
            //{
            //    if (theTable.JSONAttachmentInfo != "")
            //    {
            //        AttachmentSetting theAttachmentSetting = JSONField.GetTypedObject<AttachmentSetting>(theTable.JSONAttachmentInfo);
            //        if (theAttachmentSetting != null)
            //        {
            //            if( theAttachmentSetting.AttachIncomingEmails!=null)
            //            chkAttachIncomingEmails.Checked=(bool) theAttachmentSetting.AttachIncomingEmails;

            //            if (theAttachmentSetting.AttachOutgoingEmails != null)
            //                chkAttachOutgoingEmails.Checked = (bool)theAttachmentSetting.AttachOutgoingEmails;

            //            if (theAttachmentSetting.InIdentifierColumnID != null)
            //            {
            //                if (ddlInIdentifier.Items.FindByValue(theAttachmentSetting.InIdentifierColumnID.ToString()) != null)
            //                    ddlInIdentifier.SelectedValue = theAttachmentSetting.InIdentifierColumnID.ToString();
            //            }


            //            if (theAttachmentSetting.InSavetoTableID != null)
            //            {
            //                if (ddlInSaveToTable.Items.FindByValue(theAttachmentSetting.InSavetoTableID.ToString()) != null)
            //                    ddlInSaveToTable.SelectedValue = theAttachmentSetting.InSavetoTableID.ToString();
            //            }

            //            ddlInSaveToTable_SelectedIndexChanged(null, null);

            //            if (theAttachmentSetting.InSaveEmailColumnID != null)
            //            {
            //                if (ddlInSaveBodyTo.Items.FindByValue(theAttachmentSetting.InSaveEmailColumnID.ToString()) != null)
            //                    ddlInSaveBodyTo.SelectedValue = theAttachmentSetting.InSaveEmailColumnID.ToString();
            //            }

            //            if (theAttachmentSetting.InSaveAttachmentColumnID!= null)
            //            {
            //                if (ddlInSaveAttachmentTo.Items.FindByValue(theAttachmentSetting.InSaveAttachmentColumnID.ToString()) != null)
            //                    ddlInSaveAttachmentTo.SelectedValue = theAttachmentSetting.InSaveAttachmentColumnID.ToString();
            //            }

            //            if (theAttachmentSetting.InSaveSubjectColumnID != null)
            //            {
            //                if (ddlInSaveSubJectTo.Items.FindByValue(theAttachmentSetting.InSaveSubjectColumnID.ToString()) != null)
            //                    ddlInSaveSubJectTo.SelectedValue = theAttachmentSetting.InSaveSubjectColumnID.ToString();
            //            }

            //            if (theAttachmentSetting.InSaveSenderColumnID != null)
            //            {
            //                if (ddlInSaveToSender.Items.FindByValue(theAttachmentSetting.InSaveSenderColumnID.ToString()) != null)
            //                    ddlInSaveToSender.SelectedValue = theAttachmentSetting.InSaveSenderColumnID.ToString();
            //            }

            //            if (theAttachmentSetting.OutSavetoTableID != null)
            //            {
            //                if (ddlOutSaveToTable.Items.FindByValue(theAttachmentSetting.OutSavetoTableID.ToString()) != null)
            //                    ddlOutSaveToTable.SelectedValue = theAttachmentSetting.OutSavetoTableID.ToString();
            //            }

            //            ddlOutSaveToTable_SelectedIndexChanged(null, null);

            //            if (theAttachmentSetting.OutSaveBodyColumnID != null)
            //            {
            //                if (ddlOutSaveBodyTo.Items.FindByValue(theAttachmentSetting.OutSaveBodyColumnID.ToString()) != null)
            //                    ddlOutSaveBodyTo.SelectedValue = theAttachmentSetting.OutSaveBodyColumnID.ToString();
            //            }


            //            if (theAttachmentSetting.OutSaveRecipientColumnID != null)
            //            {
            //                if (ddlOutSaveRecipient.Items.FindByValue(theAttachmentSetting.OutSaveRecipientColumnID.ToString()) != null)
            //                    ddlOutSaveRecipient.SelectedValue = theAttachmentSetting.OutSaveRecipientColumnID.ToString();
            //            }


            //            if (theAttachmentSetting.OutSaveSubjectColumnID != null)
            //            {
            //                if (ddlOutSaveSubjectto.Items.FindByValue(theAttachmentSetting.OutSaveSubjectColumnID.ToString()) != null)
            //                    ddlOutSaveSubjectto.SelectedValue = theAttachmentSetting.OutSaveSubjectColumnID.ToString();
            //            }


            //        }

            //    }

            //    if (theTable.JSONAttachmentPOP3 != "")
            //    {
            //        AttachmentPOP3 theAttachmentPOP3 = JSONField.GetTypedObject<AttachmentPOP3>(theTable.JSONAttachmentPOP3);
            //        if (theAttachmentPOP3 != null)
            //        {
            //            txtAttacmentEmail.Text = theAttachmentPOP3.Email;
            //            txtAttacmentPassword.Text = theAttachmentPOP3.Password;
            //            txtAttacmentServer.Text = theAttachmentPOP3.POP3Server;
            //            txtAttacmentUserName.Text = theAttachmentPOP3.Username;

            //            if(theAttachmentPOP3.Port!=null)
            //            txtAttacmentPort.Text = theAttachmentPOP3.Port.ToString();

            //            if (theAttachmentPOP3.SSL != null)
            //            {
            //                if ((bool)theAttachmentPOP3.SSL)
            //                {
            //                    optAttachmentSSL.SelectedValue = "1";
            //                }
            //                else
            //                {
            //                    optAttachmentSSL.SelectedValue = "0";
            //                }
            //            }
            //        }

            //    }

            //}

            if (_strActionMode == "edit")
            {
                ViewState["theTable"] = theTable;

                //Title = theTable.TableName + ": Properties";
                //lblTitle.Text = theTable.TableName + ": Properties";
            }
            else if (_strActionMode == "view")
            {
                //Title = theTable.TableName + ": Properties";
                //lblTitle.Text = theTable.TableName + ": Properties";
            }


            PopulateChildTable();
            PopulateTemplates();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideAutoUser1", "ShowHideAutoUser();", true);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }




    }




  

    protected void PopulateUserDropDown()
    {

        ddlUser.Items.Clear();

        //ddlUser.DataSource = SecurityManager.ets_User_ByAccount(int.Parse(Session["AccountID"].ToString()));

        ddlUser.DataSource = Common.DataTableFromText(@"SELECT    ([User].FirstName + ' ' + [User].LastName ) As UserName, [User].Userid
	                    FROM [User] INNER JOIN UserRole ON [User].UserID=UserRole.UserID WHERE [User].IsActive=1 and
	                    UserRole.AccountID=" + Session["AccountID"].ToString() + @" AND [User].UserID NOT IN (SELECT     DISTINCT TableUser.UserID
                    FROM         [User] INNER JOIN
                      TableUser ON [User].UserID = TableUser.UserID
                      INNER JOIN UserRole ON [User].UserID=UserRole.UserID
                      WHERE [UserRole].AccountID=" + Session["AccountID"].ToString() + @" AND TableID=" + _qsTableID + @" UNION SELECT -1)
	                    ORDER BY [User].FirstName");


        ddlUser.DataBind();

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Select User-", "-1");
        ddlUser.Items.Insert(0, liSelect);
    }

    protected void PopulateSearchCriteria(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));


                chkShowSystemFields.Checked = bool.Parse(xmlDoc.FirstChild[chkShowSystemFields.ID].InnerText);
                ddlTableTabFilter.SelectedValue = xmlDoc.FirstChild[ddlTableTabFilter.ID].InnerText;
                _iStartIndex = int.Parse(xmlDoc.FirstChild["iStartIndex"].InnerText);
                _iMaxRows = int.Parse(xmlDoc.FirstChild["iMaxRows"].InnerText);

            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }

    protected void lnkDeleteAllOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkDeleteParmanent.Checked && chkUndo.Checked)
            {             
                    RecordManager.dbg_Table_Delete_Permanent(int.Parse(_qsTableID));
                    mpeDeleteAll.Hide();

            }
            else
            {
                RecordManager.ets_Table_Delete(int.Parse(_qsTableID));
            }
            Response.Redirect("~/Pages/Record/TableList.aspx", false);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ProblemDelete", "alert('Delete failed! Please try again.');", true);
        }

    }


    
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            //check if it has child records
            DataTable dtCT = Common.DataTableFromText("SELECT * FROM TableChild WHERE ParentTableID=" + _qsTableID + " AND DetailPageType<>'not'  ORDER BY TableChildID"); //AND DetailPageType<>'not'

            if (dtCT.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ProblemDelete", "alert('This table has associated child table(s), please delete those associated child table(s) first and then try again.');", true);
                return;
            }
            //check if it has reminders

            DataTable dtDR = Common.DataTableFromText(@"SELECT DataReminderID FROM DataReminder INNER JOIN [Column] 
                ON DataReminder.ColumnID=[Column].ColumnID
                WHERE [Column].TableID=" + _qsTableID );

            if (dtDR.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ProblemDelete", "alert('This table has associated reminder field(s), please delete those associated reminder(s) first and then try again.');", true);
                return;
            }

            mpeDeleteAll.Show();
           
        }
        catch (Exception ex)
        {
            if (ex is Exception)
            {
                lblMsg.Text = "Delete failed! Please try again.";
            }
            else
            {

                lblMsg.Text = ex.Message;
            }
        }


    }

    protected void lnkUnDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            RecordManager.ets_Table_UnDelete(int.Parse(_qsTableID));
            Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            if (ex is Exception)
            {
                if (ex.Message.IndexOf("UQ_RecordGroup_TypeName") > -1) //rework
                {                   

                    lblMsg.Text = " A " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " (" + _theTable.TableName + ") is already exists!";
                }
                else
                {
                    lblMsg.Text = "Restore failed! Please try again.";
                }
            }
            else
            {

                lblMsg.Text = ex.Message;
            }
        }


    }

    protected void MakeSearchCriteria(int iStartIndex, int iMaxRows)
    {
        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                   " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                     " <" + chkShowSystemFields.ID + ">" + HttpUtility.HtmlEncode(chkShowSystemFields.Checked.ToString()) + "</" + chkShowSystemFields.ID + ">" +
                      " <" + ddlTableTabFilter.ID + ">" + HttpUtility.HtmlEncode(ddlTableTabFilter.SelectedValue.ToString()) + "</" + ddlTableTabFilter.ID + ">" +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }

        //End Searchcriteria
    }




    protected void PopulateTabs()
    {
        ddlTableTabFilter.Items.Clear();

        DataTable dtTemp = Common.DataTableFromText(@"SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + " AND DisplayOrder<>0 ORDER BY DisplayOrder");

        if (dtTemp.Rows.Count == 0)
        {
            ddlTableTabFilter.Visible = false;
        }


        DataTable dtTempp = Common.DataTableFromText(@"SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + " AND DisplayOrder=0 ORDER BY DisplayOrder");

        
        ddlTableTabFilter.DataSource = dtTemp;
        ddlTableTabFilter.DataBind();

        if (dtTempp.Rows.Count > 0)
        {
            ListItem liSelect = new ListItem(dtTempp.Rows[0]["TabName"].ToString(), "");
            liSelect.Selected = true;
            ddlTableTabFilter.Items.Insert(0, liSelect);
        }
        
        ListItem liaLL = new ListItem("--All Tabs--", "-1");
        ddlTableTabFilter.Items.Insert(0, liaLL);


    }
    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {

        lblMsg.Text = "";

        MakeSearchCriteria(iStartIndex, iMaxRows);


        try
        {

            //int iTN = 0;

            //bool? bIsStandard = false;

            //if (chkShowSystemFields.Checked)
            //    bIsStandard = null;

            //gvTheGrid.DataSource = RecordManager.ets_Table_Columns_Display(
            //   int.Parse(hfTableID.Value),
            //    iStartIndex, iMaxRows, ref iTN, bIsStandard);
            //gvTheGrid.VirtualItemCount = iTN;



            //if (bIsStandard == false && iTN == 0)
            //{
            //    chkShowSystemFields.Checked = true;
            //    gvTheGrid.DataSource = RecordManager.ets_Table_Columns_Display(
            // int.Parse(hfTableID.Value),
            //  iStartIndex, iMaxRows, ref iTN, null);
            //    gvTheGrid.VirtualItemCount = iTN;
            //}

          
            string strFilter = "";
            if (ddlTableTabFilter.SelectedValue == "")
            {
                strFilter = " AND TableTabID IS NULL ";
            }
            else if (ddlTableTabFilter.SelectedValue == "-1")
            {
                strFilter = "";
            }
            else
            {
                strFilter = " AND TableTabID= " + ddlTableTabFilter.SelectedValue;
            }
            if (chkShowSystemFields.Checked == false)
                strFilter = strFilter + " AND IsStandard=0 ";

            DataTable dtColumns = Common.DataTableFromText(@"SELECT * FROM [Column] WHERE SystemName<>'TableID' AND SystemName<>'IsActive' AND TableID=" + hfTableID.Value.ToString() + strFilter + " ORDER BY DisplayOrder");

            //if (chkShowSystemFields.Checked == false)
            //{              
            //    for (int i = dtColumns.Rows.Count-1; i >=0; i--)
            //    {

            //        if (dtColumns.Rows[i]["DisplayTextSummary"] == DBNull.Value && dtColumns.Rows[i]["DisplayTextDetail"] == DBNull.Value
            //           && dtColumns.Rows[i]["NameOnImport"] == DBNull.Value && dtColumns.Rows[i]["NameOnExport"] == DBNull.Value
            //           && dtColumns.Rows[i]["MobileName"] == DBNull.Value)
            //        {
            //            dtColumns.Rows.RemoveAt(i);
            //        }
            //    }
            //    dtColumns.AcceptChanges();
            //}



            gvTheGrid.DataSource = dtColumns;
            gvTheGrid.VirtualItemCount = dtColumns.Rows.Count;
            gvTheGrid.DataBind();

            if (gvTheGrid.VirtualItemCount == 0)
            {
                divEmptyFields.Visible = true;
            }
            else
            {
                divEmptyFields.Visible = false;
            }

            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;

            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

                _gvPager.AddURL = GetAddURL();

                if (_strSFTID != "")
                {
                    _gvPager.HyperAdd_CSS = "popuplinkEMD";

                }
                //_gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);

                hlEdit.NavigateUrl = GetEditTableURL();
                if (_strActionMode == "view")
                {
                    _gvPager.HideAdd = true;
                    divEdit.Visible = true;
                }
            }
            if (_strActionMode == "view")
            {
                gvTheGrid.Columns[3].Visible = false;
            }
            if (_strActionMode != "edit")
            {
                gvTheGrid.Columns[0].Visible = false;
                gvTheGrid.Columns[1].Visible = false;
            }

            Table theTable = RecordManager.ets_Table_Details(int.Parse(hfTableID.Value));

            if (theTable.IsImportPositional == true)
            {
                gvTheGrid.Columns[11].Visible = false;
                gvTheGrid.Columns[12].Visible = true;

            }
            else
            {
                gvTheGrid.Columns[11].Visible = true;
                gvTheGrid.Columns[12].Visible = false;

            }

            if(_bIsAccountHolder==false)
            {
                TabContainer2.Tabs[1].Visible = false;
                TabContainer2.Tabs[2].Visible = false;
                //if (_gvPager!=null)
                //{
                //    _gvPager.HideAdd = true;
                //}
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void chkShowSystemFields_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (_gvPager != null)
        {
            BindTheGrid(0, _gvPager.TotalRows);
            Pager_OnApplyFilter(null, null);
        }
        else
        {
            BindTheGrid(0, 1000);
        }
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
    }




    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action

        if (chkAddUserRecord.Checked)
        {
            if (ddlAddUserUserColumnID.SelectedItem == null)
            {
                lblMsg.Text = "Please select  Automatically Create UserName.";
                ddlAddUserUserColumnID.Focus();                
                return false;
            }
            if (ddlAddUserPasswordColumnID.SelectedItem == null)
            {
                ddlAddUserPasswordColumnID.Focus();
                lblMsg.Text = "Please select  Automatically Create UserName's Password.";
                return false;
            }

        }
        if(chkDataUpdateUniqueColumnID.Checked)
        {
            //if(ddlDataUpdateUniqueColumnID.SelectedValue=="")
            //{
            //    lblMsg.Text = "Please select Unique identifier for Allow data to be up updated.";
            //    ddlDataUpdateUniqueColumnID.Focus();
            //    return false;
            //}
        }

        //if (chkAnonymous.Checked)
        //{
        //    if (ddlValidateColumnID1.SelectedValue == "" || ddlValidateColumnID2.SelectedValue == "")
        //    {
        //        lblMsg.Text = "Please select both columns to validate user.";
        //        return false;
        //    }
        //}


        return true;
    }
   




    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        //string strTable = "Table";
        //strTable = Common.T_Table;

        _gvPager.ExportFileName = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " - " + _theTable.TableName;
        BindTheGrid(0, _gvPager.TotalRows);
    }

   

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {

        if (_strActionMode.ToLower() == "view")
        {
            if (_gvPager != null)
                _gvPager.HideAdd = true;
        }


        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        BindTheGrid(0, _gvPager.TotalRows);
    }



    protected void cbcvSumFilter_OnddlYAxis_Changed(object sender, EventArgs e)
    {
        //do nothing here
    }


    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

            bool bIsStandard = bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsStandard").ToString());
            ImageButton ib = (ImageButton)e.Row.FindControl("imgbtnDelete");

            if (_bTableTabYes)
            {
                DropDownList ddlTableTab = (DropDownList)e.Row.FindControl("ddlTableTab");
                if (ddlTableTab != null)
                {
                    ddlTableTab.Items.Clear();

                    DataTable dtTemp = Common.DataTableFromText(@"SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + " AND DisplayOrder<>0  ORDER BY DisplayOrder");

                    ddlTableTab.DataSource = dtTemp;
                    ddlTableTab.DataBind();

                    DataTable dtTempp = Common.DataTableFromText(@"SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + " AND DisplayOrder=0 ORDER BY DisplayOrder");

                    if (dtTempp.Rows.Count > 0)
                    {
                        ListItem liSelect = new ListItem(dtTempp.Rows[0]["TabName"].ToString(), "");
                        ddlTableTab.Items.Insert(0, liSelect);
                    }

                    if (DataBinder.Eval(e.Row.DataItem, "TableTabID") != null)
                    {
                        ddlTableTab.SelectedValue = DataBinder.Eval(e.Row.DataItem, "TableTabID").ToString();
                    }

                }

            }

            //bool bDisplayRight = bool.Parse(DataBinder.Eval(e.Row.DataItem, "DisplayRight").ToString());

            //if (DataBinder.Eval(e.Row.DataItem, "IsSystemColumn").ToString().ToLower() == "true")
            //{
            //    CheckBox chkDisplayTextSummary = (CheckBox)e.Row.FindControl("chkDisplayTextSummary");
            //    chkDisplayTextSummary.Enabled = false;
            //    CheckBox chkDisplayTextDetail = (CheckBox)e.Row.FindControl("chkDisplayTextDetail");
            //    chkDisplayTextDetail.Enabled = false;
            //    CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
            //    chkNameOnImport.Enabled = false;
            //    CheckBox chkDisplayRight = (CheckBox)e.Row.FindControl("chkDisplayRight");
            //    chkDisplayRight.Enabled = false;
            //    CheckBox chkNameOnExport = (CheckBox)e.Row.FindControl("chkNameOnExport");
            //    chkNameOnExport.Enabled = false;

            //}


            if (bIsStandard)
            {
                ib.Visible = false;

                if (DataBinder.Eval(e.Row.DataItem, "SystemName").ToString() == "RecordID")
                {
                    //HyperLink hlEdit = (HyperLink)e.Row.FindControl("EditHyperLink");
                    //hlEdit.Visible = false;

                    //CheckBox chkDisplayTextSummary = (CheckBox)e.Row.FindControl("chkDisplayTextSummary");
                    //chkDisplayTextSummary.Enabled = false;

                    //CheckBox chkDisplayTextDetail = (CheckBox)e.Row.FindControl("chkDisplayTextDetail");
                    //chkDisplayTextDetail.Enabled = false;


                    CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                    chkIsMandatory.Enabled = false;

                    CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                    chkNameOnImport.Enabled = false;

                    //CheckBox chkNameOnExport = (CheckBox)e.Row.FindControl("chkNameOnExport");
                    //chkNameOnExport.Enabled = false;


                }
                //if (DataBinder.Eval(e.Row.DataItem, "SystemName").ToString() == "LocationID")
                //{
                //    //CheckBox chkDisplayTextDetail = (CheckBox)e.Row.FindControl("chkDisplayTextDetail");
                //    //chkDisplayTextDetail.Enabled = false;


                //    //CheckBox chkGraph = (CheckBox)e.Row.FindControl("chkGraph");
                //    //chkGraph.Enabled = false;

                //}

                if (DataBinder.Eval(e.Row.DataItem, "SystemName").ToString() == "EnteredBy")
                {
                    //CheckBox chkDisplayTextDetail = (CheckBox)e.Row.FindControl("chkDisplayTextDetail");
                    //chkDisplayTextDetail.Enabled = false;
                    CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                    chkNameOnImport.Enabled = false;
                    CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                    chkIsMandatory.Enabled = false;
                }

                if (DataBinder.Eval(e.Row.DataItem, "SystemName").ToString() == "DateTimeRecorded")
                {
                    //CheckBox chkDisplayTextDetail = (CheckBox)e.Row.FindControl("chkDisplayTextDetail");
                    //chkDisplayTextDetail.Enabled = false;

                    CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                    //chkNameOnImport.Enabled = false;

                    //CheckBox chkGraph = (CheckBox)e.Row.FindControl("chkGraph");
                    //chkGraph.Enabled = false;
                    if ((bool)_theTable.IsImportPositional)
                    {
                        if (DataBinder.Eval(e.Row.DataItem, "IsDateSingleColumn").ToString() == "False"
                            && DataBinder.Eval(e.Row.DataItem, "PositionOnImport")!=null)
                        {
                            Label lblPositionOnImport = (Label)e.Row.FindControl("lblPositionOnImport");
                            if (lblPositionOnImport != null)
                            {
                                lblPositionOnImport.Text = DataBinder.Eval(e.Row.DataItem, "PositionOnImport").ToString()
                                    + "," + (int.Parse(DataBinder.Eval(e.Row.DataItem, "PositionOnImport").ToString()) + 1).ToString();
                            }

                        }

                    }

                }

                if (DataBinder.Eval(e.Row.DataItem, "SystemName").ToString() == "IsActive")
                {
                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("EditHyperLink");
                    hlEdit.Visible = false;
                }
                if (DataBinder.Eval(e.Row.DataItem, "SystemName").ToString() == "TableID")
                {
                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("EditHyperLink");
                    hlEdit.Visible = false;
                }
            }
            else
            {
                ib.Visible = true;
                ib.Attributes.Add("onclick", "javascript:return " +
                "confirm('Are you sure you want to delete -" +
                DataBinder.Eval(e.Row.DataItem, "DisplayName") + "- Field Name?')");
            }


            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() != "number")
            {
                //CheckBox chkGraph = (CheckBox)e.Row.FindControl("chkGraph");
                //chkGraph.Checked = false;
                //chkGraph.Enabled = false;

            }

            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "file"
                || DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "image")
            {
                CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                chkNameOnImport.Checked = false;
                chkNameOnImport.Enabled = false;

            }
            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "data_retriever")
            {
                CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                chkNameOnImport.Enabled = false;
                CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                chkIsMandatory.Enabled = false;
            }

            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "slider")
            {
              
                CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                chkIsMandatory.Enabled = false;
            }

            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "content")
            {
                CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                chkNameOnImport.Checked = false;
                chkNameOnImport.Enabled = false;

                CheckBox chkDisplayTextSummary = (CheckBox)e.Row.FindControl("chkDisplayTextSummary");
                chkDisplayTextSummary.Checked = false;
                chkDisplayTextSummary.Enabled = false;

                CheckBox chkNameOnExport = (CheckBox)e.Row.FindControl("chkNameOnExport");
                chkNameOnExport.Checked = false;
                chkNameOnExport.Enabled = false;

                CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                chkIsMandatory.Enabled = false;

            }

            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "calculation")
            {
                CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                chkNameOnImport.Checked = false;
                chkNameOnImport.Enabled = false;

                CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                chkIsMandatory.Enabled = false;

            }

            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "location")
            {
                CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                chkNameOnImport.Checked = false;
                chkNameOnImport.Enabled = false;

                //CheckBox chkDisplayTextSummary = (CheckBox)e.Row.FindControl("chkDisplayTextSummary");
                //chkDisplayTextSummary.Checked = false;
                //chkDisplayTextSummary.Enabled = false;

                CheckBox chkNameOnExport = (CheckBox)e.Row.FindControl("chkNameOnExport");
                chkNameOnExport.Checked = false;
                chkNameOnExport.Enabled = false;

                CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                chkIsMandatory.Enabled = false;

            }

            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "staticcontent")
            {
                CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                chkNameOnImport.Checked = false;
                chkNameOnImport.Enabled = false;

                CheckBox chkDisplayTextSummary = (CheckBox)e.Row.FindControl("chkDisplayTextSummary");
                chkDisplayTextSummary.Checked = false;
                chkDisplayTextSummary.Enabled = false;

                CheckBox chkNameOnExport = (CheckBox)e.Row.FindControl("chkNameOnExport");
                chkNameOnExport.Checked = false;
                chkNameOnExport.Enabled = false;

                CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                chkIsMandatory.Enabled = false;

            }
            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "button")
            {
                CheckBox chkNameOnImport = (CheckBox)e.Row.FindControl("chkNameOnImport");
                chkNameOnImport.Checked = false;
                chkNameOnImport.Enabled = false;

                CheckBox chkDisplayTextSummary = (CheckBox)e.Row.FindControl("chkDisplayTextSummary");
                chkDisplayTextSummary.Checked = false;
                chkDisplayTextSummary.Enabled = false;

                CheckBox chkNameOnExport = (CheckBox)e.Row.FindControl("chkNameOnExport");
                chkNameOnExport.Checked = false;
                chkNameOnExport.Enabled = false;

                CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                chkIsMandatory.Enabled = false;

            }
            //Testing

            Label lblIsNumeric = (Label)e.Row.FindControl("lblIsNumeric");

            if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "number")
            {
                lblIsNumeric.Text = "Number";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "button")
            {
                lblIsNumeric.Text = "Button";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "text")
            {
                lblIsNumeric.Text = "Text";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "data_retriever")
            {
                lblIsNumeric.Text = "Data Retriever";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "dropdown")
            {
                lblIsNumeric.Text = "DropDown";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "date")
            {
                lblIsNumeric.Text = "Date";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "time")
            {
                lblIsNumeric.Text = "Time";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "datetime")
            {
                lblIsNumeric.Text = "DateTime";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "radiobutton")
            {
                lblIsNumeric.Text = "RadioButton";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "file")
            {
                lblIsNumeric.Text = "File";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "image")
            {
                lblIsNumeric.Text = "Image";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "listbox")
            {
                lblIsNumeric.Text = "ListBox";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "checkbox")
            {
                lblIsNumeric.Text = "Checkbox";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "content")
            {
                lblIsNumeric.Text = "Content Editor";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "location")
            {
                lblIsNumeric.Text = "Location";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "staticcontent")
            {
                lblIsNumeric.Text = "Content";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "trafficlight")
            {
                lblIsNumeric.Text = "Traffic Light";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "slider")
            {
                lblIsNumeric.Text = "Slider";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString() == "calculation")
            {
                lblIsNumeric.Text = "Calculation";
            }




            //Label lblCalculation = (Label)e.Row.FindControl("lblCalculation");

            //if (DataBinder.Eval(e.Row.DataItem, "Calculation").ToString() == "")
            //{
            //    lblCalculation.Text = "";
            //}
            //else 
            //{
            //    lblCalculation.Text = "Yes";
            //}


            //Label lblDefaultValue = (Label)e.Row.FindControl("lblDefaultValue");

            //if (DataBinder.Eval(e.Row.DataItem, "Constant").ToString() == "")
            //{
            //    lblDefaultValue.Text = "";
            //}
            //else
            //{
            //    lblDefaultValue.Text = "Yes";
            //}


            //Label lblDisplayTextSummary = (Label)e.Row.FindControl("lblDisplayTextSummary");

            //if (DataBinder.Eval(e.Row.DataItem, "DisplayTextSummary").ToString() == "")
            //{
            //    lblDisplayTextSummary.Text = "";
            //}
            //else
            //{
            //    lblDisplayTextSummary.Text = "Yes";
            //}

            //Label lblDisplayTextDetail = (Label)e.Row.FindControl("lblDisplayTextDetail");

            //if (DataBinder.Eval(e.Row.DataItem, "DisplayTextDetail").ToString() == "")
            //{
            //    lblDisplayTextDetail.Text = "";
            //}
            //else
            //{
            //    lblDisplayTextDetail.Text = "Yes";
            //}


            //Label lblNameOnImport = (Label)e.Row.FindControl("lblNameOnImport");

            //if (DataBinder.Eval(e.Row.DataItem, "NameOnImport").ToString() == "")
            //{
            //    lblNameOnImport.Text = "";
            //}
            //else
            //{
            //    lblNameOnImport.Text = "Yes";
            //}

            //Position should be as it is

            //Label lblNameOnExport = (Label)e.Row.FindControl("lblNameOnExport");

            //if (DataBinder.Eval(e.Row.DataItem, "NameOnExport").ToString() == "")
            //{
            //    lblNameOnExport.Text = "";
            //}
            //else
            //{
            //    lblNameOnExport.Text = "Yes";
            //}

        }


    }


    protected void gvTheGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "delete")
        {
            try
            {
                //RecordManager.ets_Column_Delete(Convert.ToInt32(e.CommandArgument));

                BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);

                _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
                if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
                {
                    BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
                }

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;

                //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete failed!');", true);
            }
        }
        if (e.CommandName == "uporder")
        {
            try
            {
                RecordManager.ets_Column_OrderChange(Convert.ToInt32(e.CommandArgument), true);

                BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;

                //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Order failed!');", true);
            }
        }
        if (e.CommandName == "downorder")
        {
            try
            {
                RecordManager.ets_Column_OrderChange(Convert.ToInt32(e.CommandArgument), false);

                BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);


            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;

                //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Order failed!');", true);
            }
        }
    }
    protected void gvTheGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //do nothing here.
    }


    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        //gvTheGrid.AllowPaging = false;
        //BindTheGrid(0, _gvPager.TotalRows);



        //Response.Clear();
        //Response.Buffer = true;
        //Response.AddHeader("content-disposition",
        //"attachment;filename=\"" + "Table - " + txtTable.Text + ".csv\"");
        //Response.Charset = "";
        //Response.ContentType = "text/csv";

        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);

        //Table theTable = RecordManager.ets_Table_Details(int.Parse(hfTableID.Value));

        ////gvTheGrid.Columns[7].Visible = true;
        ////gvTheGrid.Columns[6].Visible = true;

        //int iColCount = gvTheGrid.Columns.Count;
        //for (int i = 0; i < iColCount; i++)
        //{
        //    if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
        //    {
        //    }
        //    else
        //    {

        //            if (theTable.IsImportPositional == true)
        //            {
        //                if (i != 9)
        //                {
        //                    sw.Write(gvTheGrid.Columns[i].HeaderText);
        //                    if (i < iColCount - 1)
        //                    {
        //                        sw.Write(",");
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                if (i != 10)
        //                {
        //                    sw.Write(gvTheGrid.Columns[i].HeaderText);
        //                    if (i < iColCount - 1)
        //                    {
        //                        sw.Write(",");
        //                    }

        //                }

        //            }


        //    }
        //}

        //sw.Write(sw.NewLine);

        //// Now write all the rows.
        //foreach (GridViewRow dr in gvTheGrid.Rows)
        //{

        //    for (int i = 0; i < iColCount; i++)
        //    {
        //        if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
        //        {
        //        }
        //        else
        //        {
        //            switch (i)
        //            {
        //                case 3:
        //                    HyperLink hlView = (HyperLink)dr.FindControl("hlView");
        //                    sw.Write("\"" + hlView.Text + "\"");
        //                    break;


        //                case 4:
        //                    CheckBox chkIsNumeric = (CheckBox)dr.FindControl("chkIsNumeric");
        //                    sw.Write("\"" + chkIsNumeric.Checked.ToString()=="True"?"Yes":"" + "\"");
        //                    break;

        //                case 12:

        //                    break;
        //                case 9:

        //                    if (theTable.IsImportPositional == false)
        //                    {
        //                        if (!Convert.IsDBNull(dr.Cells[i]))
        //                        {
        //                            sw.Write("\"" + (dr.Cells[i].Text.Trim() == "&nbsp;" ? "" : dr.Cells[i].Text) + "\"");
        //                        }

        //                    }
        //                    break;
        //                case 10:
        //                    if (theTable.IsImportPositional == true)
        //                    {
        //                        if (!Convert.IsDBNull(dr.Cells[i]))
        //                        {
        //                            sw.Write("\"" + (dr.Cells[i].Text.Trim() == "&nbsp;" ? "" : dr.Cells[i].Text) + "\"");
        //                        }

        //                    }
        //                    break;

        //                //default:
        //                //    if (!Convert.IsDBNull(dr.Cells[i]))
        //                //    {
        //                //        sw.Write("\"" + (dr.Cells[i].Text.Trim() == "&nbsp;" ? "" : dr.Cells[i].Text) + "\"");
        //                //    }

        //                //    break;
        //            }


        //            if (theTable.IsImportPositional == true)
        //            {
        //                if (i != 9)
        //                {

        //                    if (i < iColCount - 1)
        //                    {
        //                        sw.Write(",");
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                if (i != 10)
        //                {

        //                    if (i < iColCount - 1)
        //                    {
        //                        sw.Write(",");
        //                    }

        //                }

        //            }
        //        }
        //        sw.Write(sw.NewLine);
        //    }

        //}
        //sw.Close();


        //Response.Output.Write(sw.ToString());
        //Response.Flush();
        //Response.End();
    }

   
    protected void PopulateSTUserGrid()
    {
        //int iTemp = 0;

        switch (_strActionMode.ToLower())
        {
            case "add":


                //no need to show

                break;

            case "edit":

                if (_qsTableID != "")
                {
                    grdTableUser.DataSource = RecordManager.ets_TableUser_Select(null, int.Parse(_qsTableID), null, null, null, null, null, null, null, null, null, null, null);
                    grdTableUser.DataBind();
                }
                break;

            case "view":
                if (_qsTableID != "")
                {
                    grdTableUser.DataSource = RecordManager.ets_TableUser_Select(null, int.Parse(_qsTableID), null, null, null, null, null, null, null, null, null, null, null);
                    grdTableUser.DataBind();

                    grdTableUser.Columns[1].Visible = false;
                    grdTableUser.Columns[2].Visible = false;
                    grdTableUser.Enabled = false;
                    //if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2,4"))
                    //{

                    //    divDelete.Visible = false;
                    //    divUnDelete.Visible = false;
                    //}
                }
                break;
        }



        //AddHeaderForTableUserGridView();
        if(_bShowExceedances==false && grdTableUser.Columns.Count>6)
        {
            grdTableUser.Columns[6].Visible = false;
        }

    }

    protected void UpdateSupplyLed(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;

        if (ddl != null)
        {
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            Label lblID = row.FindControl("lblID") as Label;

            TableUser theTableUser = RecordManager.ets_TableUser_Detail(int.Parse(lblID.Text));
            switch (ddl.ID)
            {


                case "ddlAddDataOption":

                    if (ddl.SelectedValue == "none")
                    {
                        theTableUser.AddDataEmail = false;
                        theTableUser.AddDataSMS = false;
                    }
                    if (ddl.SelectedValue == "email")
                    {
                        theTableUser.AddDataEmail = true;
                        theTableUser.AddDataSMS = false;
                    }
                    if (ddl.SelectedValue == "sms")
                    {
                        theTableUser.AddDataEmail = false;
                        theTableUser.AddDataSMS = true;
                    }
                    if (ddl.SelectedValue == "both")
                    {
                        theTableUser.AddDataEmail = true;
                        theTableUser.AddDataSMS = true;
                    }


                    break;
                case "ddlUploadOption":

                    if (ddl.SelectedValue == "none")
                    {
                        theTableUser.UploadEmail = false;
                        theTableUser.UploadSMS = false;
                    }
                    if (ddl.SelectedValue == "email")
                    {
                        theTableUser.UploadEmail = true;
                        theTableUser.UploadSMS = false;
                    }
                    if (ddl.SelectedValue == "sms")
                    {
                        theTableUser.UploadEmail = false;
                        theTableUser.UploadSMS = true;
                    }
                    if (ddl.SelectedValue == "both")
                    {
                        theTableUser.UploadEmail = true;
                        theTableUser.UploadSMS = true;
                    }


                    break;

                case "ddlUploadWarningOption":
                    if (ddl.SelectedValue == "none")
                    {
                        theTableUser.UploadWarningEmail = false;
                        theTableUser.UploadWarningSMS = false;
                    }
                    if (ddl.SelectedValue == "email")
                    {
                        theTableUser.UploadWarningEmail = true;
                        theTableUser.UploadWarningSMS = false;
                    }
                    if (ddl.SelectedValue == "sms")
                    {
                        theTableUser.UploadWarningEmail = false;
                        theTableUser.UploadWarningSMS = true;
                    }
                    if (ddl.SelectedValue == "both")
                    {
                        theTableUser.UploadWarningEmail = true;
                        theTableUser.UploadWarningSMS = true;
                    }


                    break;
                case "ddlUploadExceedanceOption":
                    if (ddl.SelectedValue == "none")
                    {
                        theTableUser.ExceedanceEmail = false;
                        theTableUser.ExceedanceSMS = false;
                    }
                    if (ddl.SelectedValue == "email")
                    {
                        theTableUser.ExceedanceEmail = true;
                        theTableUser.ExceedanceSMS = false;
                    }
                    if (ddl.SelectedValue == "sms")
                    {
                        theTableUser.ExceedanceEmail = false;
                        theTableUser.ExceedanceSMS = true;
                    }
                    if (ddl.SelectedValue == "both")
                    {
                        theTableUser.ExceedanceEmail = true;
                        theTableUser.ExceedanceSMS = true;
                    }


                    break;

                case "ddlLateWarningOption":
                    if (ddl.SelectedValue == "none")
                    {
                        theTableUser.LateWarningEmail = false;
                        theTableUser.LateWarningSMS = false;
                    }
                    if (ddl.SelectedValue == "email")
                    {
                        theTableUser.LateWarningEmail = true;
                        theTableUser.LateWarningSMS = false;
                    }
                    if (ddl.SelectedValue == "sms")
                    {
                        theTableUser.LateWarningEmail = false;
                        theTableUser.LateWarningSMS = true;
                    }
                    if (ddl.SelectedValue == "both")
                    {
                        theTableUser.LateWarningEmail = true;
                        theTableUser.LateWarningSMS = true;
                    }


                    break;


            }

            RecordManager.ets_TableUser_Update(theTableUser);

            //update database logic here.
        }


    }



    protected void grdTableUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetype")
        {
            try
            {
                lblMsgTab.Text = "";
                //SiteManager.ets_LocationTable_Delete(Convert.ToInt32(e.CommandArgument));

                RecordManager.ets_TableUser_Delete(Convert.ToInt32(e.CommandArgument));
                PopulateUserDropDown();
                PopulateSTUserGrid();

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Table User", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsgTab.Text = ex.Message;

                //ScriptManager.RegisterClientScriptBlock(grdTable, typeof(Page), "msg_delete", "alert('Delete failed!');", true);
            }
        }
    }


    protected void AddHeaderForTableUserGridView()
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        TableCell cell1 = new TableCell();
        row.Cells.Add(cell1);

        TableCell cell2 = new TableCell();
        cell2.HorizontalAlign = HorizontalAlign.Center;
        cell2.Text = "User";
        cell2.Font.Bold = true;
        row.Cells.Add(cell2);

        TableCell cell6 = new TableCell();
        cell6.ColumnSpan = 2;
        cell6.HorizontalAlign = HorizontalAlign.Center;
        cell6.Text = "Data Upload";
        cell6.Font.Bold = true;
        row.Cells.Add(cell6);

        TableCell cell4 = new TableCell();
        cell4.ColumnSpan = 2;
        cell4.HorizontalAlign = HorizontalAlign.Center;
        cell4.Text = "Data Warning";
        cell4.Font.Bold = true;
        row.Cells.Add(cell4);


        TableCell cell5 = new TableCell();
        cell5.ColumnSpan = 2;
        cell5.HorizontalAlign = HorizontalAlign.Center;
        cell5.Text = "Late Data";
        cell5.Font.Bold = true;
        row.Cells.Add(cell5);
        row.EnableViewState = false;
        grdTableUser.Controls[0].EnableViewState = false;
        grdTableUser.Controls[0].Controls.AddAt(0, row);
    }

    protected void grdTableUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ib = (ImageButton)e.Row.FindControl("imgbtnDelete");
                ib.Attributes.Add("onclick", "javascript:return " +
                "confirm('Are you sure you want to remove User --" +
                DataBinder.Eval(e.Row.DataItem, "UserName") + "?');");


                DropDownList ddlUploadOption = (DropDownList)e.Row.FindControl("ddlUploadOption");
                DropDownList ddlUploadWarningOption = (DropDownList)e.Row.FindControl("ddlUploadWarningOption");
                DropDownList ddlUploadExceedanceOption = (DropDownList)e.Row.FindControl("ddlUploadExceedanceOption");
                DropDownList ddlLateWarningOption = (DropDownList)e.Row.FindControl("ddlLateWarningOption");
                DropDownList ddlAddDataOption = (DropDownList)e.Row.FindControl("ddlAddDataOption");

                TableUser theTableUser = RecordManager.ets_TableUser_Detail(int.Parse(DataBinder.Eval(e.Row.DataItem, "TableUserID").ToString()));
                if (theTableUser != null)
                {
                    //if (theTableUser.AddDataEmail != null && theTableUser.AddDataSMS != null)
                    //{
                       

                    //    if (theTableUser.AddDataEmail != null)
                    //    {
                    //        if ((bool)theTableUser.AddDataEmail)
                    //        {
                    //            ddlAddDataOption.Text = "email";
                    //        }
                    //    }

                    //    if (theTableUser.AddDataSMS != null)
                    //    {
                    //        if ((bool)theTableUser.AddDataSMS)
                    //        {
                    //            ddlAddDataOption.Text = "sms";
                    //        }
                    //    }

                    //    if ((bool)theTableUser.AddDataEmail && (bool)theTableUser.AddDataSMS)
                    //    {
                    //        ddlAddDataOption.Text = "both";
                    //    }
                    //    if ((bool)theTableUser.AddDataEmail == false && (bool)theTableUser.AddDataSMS == false)
                    //    {
                    //        ddlAddDataOption.Text = "none";
                    //    }
                    //}
                    //else
                    //{
                    //    ddlAddDataOption.Text = "none";

                    //     if (theTableUser.AddDataEmail != null)
                    //     {
                    //         if ((bool)theTableUser.AddDataEmail)
                    //         {
                    //             ddlAddDataOption.Text = "email";
                    //         }
                    //     }

                    //     if (theTableUser.AddDataSMS != null)
                    //     {
                    //         if ((bool)theTableUser.AddDataSMS)
                    //         {
                    //             ddlAddDataOption.Text = "sms";
                    //         }
                    //     }


                    //}

                    if (theTableUser.AddDataEmail != null && theTableUser.AddDataSMS != null)
                    {
                        if ((bool)theTableUser.AddDataEmail && (bool)theTableUser.AddDataSMS)
                        {
                            ddlAddDataOption.Text = "both";
                        }
                        else
                        {
                            if ((bool)theTableUser.AddDataEmail == false && (bool)theTableUser.AddDataSMS == false)
                            {
                                ddlAddDataOption.Text = "none";
                            }
                            else
                            {
                                if ((bool)theTableUser.AddDataEmail)
                                {
                                    ddlAddDataOption.Text = "email";
                                }
                                else
                                {
                                    ddlAddDataOption.Text = "sms";
                                }

                            }

                        }
                    }
                   



                    //
                    if ((bool)theTableUser.UploadEmail && (bool)theTableUser.UploadSMS)
                    {
                        ddlUploadOption.Text = "both";
                    }
                    else
                    {
                        if ((bool)theTableUser.UploadEmail == false && (bool)theTableUser.UploadSMS == false)
                        {
                            ddlUploadOption.Text = "none";
                        }
                        else
                        {
                            if ((bool)theTableUser.UploadEmail)
                            {
                                ddlUploadOption.Text = "email";
                            }
                            else
                            {
                                ddlUploadOption.Text = "sms";
                            }

                        }

                    }
                    //
                    if (theTableUser.ExceedanceEmail != null && theTableUser.ExceedanceSMS != null)
                    {
                        if ((bool)theTableUser.ExceedanceEmail && (bool)theTableUser.ExceedanceSMS)
                        {
                            ddlUploadExceedanceOption.Text = "both";
                        }
                        else
                        {
                            if ((bool)theTableUser.ExceedanceEmail == false && (bool)theTableUser.ExceedanceSMS == false)
                            {
                                ddlUploadExceedanceOption.Text = "none";
                            }
                            else
                            {
                                if ((bool)theTableUser.ExceedanceEmail)
                                {
                                    ddlUploadExceedanceOption.Text = "email";
                                }
                                else
                                {
                                    ddlUploadExceedanceOption.Text = "sms";
                                }

                            }

                        }
                    }

                   

                    ///

                    if ((bool)theTableUser.UploadWarningEmail && (bool)theTableUser.UploadWarningSMS)
                    {
                        ddlUploadWarningOption.Text = "both";
                    }
                    else
                    {
                        if ((bool)theTableUser.UploadWarningEmail == false && (bool)theTableUser.UploadWarningSMS == false)
                        {
                            ddlUploadWarningOption.Text = "none";
                        }
                        else
                        {
                            if ((bool)theTableUser.UploadWarningEmail)
                            {
                                ddlUploadWarningOption.Text = "email";
                            }
                            else
                            {
                                ddlUploadWarningOption.Text = "sms";
                            }

                        }

                    }
                    //

                    if ((bool)theTableUser.LateWarningEmail && (bool)theTableUser.LateWarningSMS)
                    {
                        ddlLateWarningOption.Text = "both";
                    }
                    else
                    {
                        if ((bool)theTableUser.LateWarningEmail == false && (bool)theTableUser.LateWarningSMS == false)
                        {
                            ddlLateWarningOption.Text = "none";
                        }
                        else
                        {
                            if ((bool)theTableUser.LateWarningEmail)
                            {
                                ddlLateWarningOption.Text = "email";
                            }
                            else
                            {
                                ddlLateWarningOption.Text = "sms";
                            }

                        }

                    }





                }

                //
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {





                HyperLink hlUploadEmail = (HyperLink)e.Row.FindControl("hlUploadEmail");
                Content xContent = SystemData.Content_Details_ByKey("DataUploadEmail",int.Parse(Session["AccountID"].ToString()));
                if (xContent != null)
                    hlUploadEmail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(xContent.ContentID.ToString());

                HyperLink hlUploadSMS = (HyperLink)e.Row.FindControl("hlUploadSMS");
                Content yContent = SystemData.Content_Details_ByKey("DataUploadSMS",int.Parse(Session["AccountID"].ToString()));

                if (yContent != null)
                    hlUploadSMS.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(yContent.ContentID.ToString());



                HyperLink hlAddDataEmail = (HyperLink)e.Row.FindControl("hlAddDataEmail");
                Content adContent = SystemData.Content_Details_ByKey("AddDataEmail", int.Parse(Session["AccountID"].ToString()));
                if (adContent != null)
                    hlAddDataEmail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(adContent.ContentID.ToString());

                HyperLink hlAddDataSMS = (HyperLink)e.Row.FindControl("hlAddDataSMS");
                Content adSContent = SystemData.Content_Details_ByKey("AddDataSMS", int.Parse(Session["AccountID"].ToString()));

                if (adSContent != null)
                    hlAddDataSMS.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(adSContent.ContentID.ToString());




                //HyperLink hlWarningEmail = (HyperLink)e.Row.FindControl("hlWarningEmail");
                //Content aContent = SystemData.Content_Details_ByKey("DataWarningEmail_" + Session["AccountID"].ToString());
                //if (aContent!=null)
                //hlWarningEmail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(aContent.ContentID.ToString());

                //HyperLink hlWarningSMS = (HyperLink)e.Row.FindControl("hlWarningSMS");
                //Content bContent = SystemData.Content_Details_ByKey("DataWarningSMS_" + Session["AccountID"].ToString());

                //if (bContent != null)
                //hlWarningSMS.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(bContent.ContentID.ToString());





                HyperLink hlLateWarningEmail = (HyperLink)e.Row.FindControl("hlLateWarningEmail");
                Content cContent = SystemData.Content_Details_ByKey("LateWarningEmail",int.Parse(Session["AccountID"].ToString()));

                if (cContent != null)
                    hlLateWarningEmail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(cContent.ContentID.ToString());


                HyperLink hlLateWarningSMS = (HyperLink)e.Row.FindControl("hlLateWarningSMS");
                Content dContent = SystemData.Content_Details_ByKey("LateWarningSMS",int.Parse(Session["AccountID"].ToString()));

                if (dContent != null)
                    hlLateWarningSMS.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(dContent.ContentID.ToString());



                HyperLink hlUploadWarningEmail = (HyperLink)e.Row.FindControl("hlUploadWarningEmail");
                Content pContent = SystemData.Content_Details_ByKey("DataUploadWarningEmail",int.Parse(Session["AccountID"].ToString()));
                if (pContent != null)
                    hlUploadWarningEmail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(pContent.ContentID.ToString());

                HyperLink hlUploadWarningSMS = (HyperLink)e.Row.FindControl("hlUploadWarningSMS");
                Content qContent = SystemData.Content_Details_ByKey("DataUploadWarningSMS",int.Parse(Session["AccountID"].ToString()));

                if (qContent != null)
                    hlUploadWarningSMS.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(qContent.ContentID.ToString());


                HyperLink hlUploadExceedanceEmail = (HyperLink)e.Row.FindControl("hlUploadExceedanceEmail");
                Content eContent = SystemData.Content_Details_ByKey("DataUploadExceedanceEmail", int.Parse(Session["AccountID"].ToString()));
                if (eContent != null)
                    hlUploadExceedanceEmail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(eContent.ContentID.ToString());

                HyperLink hlUploadExceedanceSMS = (HyperLink)e.Row.FindControl("hlUploadExceedanceSMS");
                Content esContent = SystemData.Content_Details_ByKey("DataUploadExceedanceSMS", int.Parse(Session["AccountID"].ToString()));

                if (esContent != null)
                    hlUploadExceedanceSMS.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(esContent.ContentID.ToString());




            }
        }
        catch (Exception ex)
        {
            //
        }


        //Label lblEmail = (Label)e.Row.FindControl("lblEmail");
        //Label lblSMS = (Label)e.Row.FindControl("lblSMS");

        //if (DataBinder.Eval(e.Row.DataItem, "WarningEmail").ToString() == "True")
        //{
        //    lblEmail.Text = "Yes";
        //}
        //else 
        //{
        //    lblEmail.Text = "No";
        //}

        //if (DataBinder.Eval(e.Row.DataItem, "WarningSMS").ToString() == "True")
        //{
        //    lblSMS.Text = "Yes";
        //}
        //else
        //{
        //    lblSMS.Text = "No";
        //}





    }
    public string GetUserViewURL()
    {
        return "#";
        //return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&MenuID=" + Cryptography.Encrypt(_qsMenuID) + "&TableID=";

    }

 

  



    protected void lnkSmallSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsgTab.Text = "";

            if (ddlUser.SelectedValue == "-1")
            {
                lblMsgTab.Text = "Please select a User!";
                return;
            }


            TableUser newTableUser;
            switch (_strActionMode.ToLower())
            {
                case "add":

                    //newLocationTable = new LocationTable(null,
                    //    int.Parse(ViewState["iNewLocationID"].ToString()),
                    //    int.Parse(ddlTable.SelectedValue), "", "");
                    //SiteManager.ets_LocationTable_Insert(newLocationTable);


                    break;

                case "edit":

                    newTableUser = new TableUser(null, int.Parse(_qsTableID),
                        int.Parse(ddlUser.SelectedValue), true, false, true, false, true, false, null, null);
                    newTableUser.ExceedanceEmail = true;
                    RecordManager.ets_TableUser_Insert(newTableUser);

                    PopulateUserDropDown();
                    //        newLocationTable = new LocationTable(null,
                    //_iLocationID, int.Parse(ddlTable.SelectedValue),
                    //"", "");
                    //        SiteManager.ets_LocationTable_Insert(newLocationTable);
                    break;
            }




            PopulateSTUserGrid();

        }
        catch (Exception ex)
        {

            if (ex.Message.IndexOf("UQ_SampleTypeUser") > -1)
            {
                lblMsgTab.Text = "This user is already added! ";
            }
            else
            {
                lblMsgTab.Text = ex.Message;
            }
        }
    }

    protected void cmdSmallCancel_Click(object sender, ImageClickEventArgs e)
    {

    }


    protected void UpdateColumn(object sender, EventArgs e)
    {

      
        
        try
        {
            CheckBox chkBx = sender as CheckBox;

            if (chkBx != null)
            {
                GridViewRow row = chkBx.NamingContainer as GridViewRow;
                Label lblID = row.FindControl("LblID") as Label;

                Column theColumn = RecordManager.ets_Column_Details(int.Parse(lblID.Text));
                switch (chkBx.ID)
                {
                    case "chkDisplayTextSummary":
                        if (chkBx.Checked)
                        {
                            theColumn.DisplayTextSummary = theColumn.DisplayName;
                        }
                        else
                        {
                            theColumn.DisplayTextSummary = "";
                        }


                        break;

                    case "chkDisplayTextDetail":
                        if (chkBx.Checked)
                        {
                            theColumn.DisplayTextDetail = theColumn.DisplayName;
                        }
                        else
                        {
                            theColumn.DisplayTextDetail = "";
                        }
                        break;

                    case "chkIsMandatory":

                        theColumn.IsMandatory = chkBx.Checked;
                        
                        
                        break;

                    case "chkDisplayRight":
                        if (chkBx.Checked)
                        {
                            theColumn.DisplayRight = true;
                        }
                        else
                        {
                            theColumn.DisplayRight = false;
                        }
                        break;

                    case "chkNameOnImport":
                        if (chkBx.Checked)
                        {
                            theColumn.NameOnImport = theColumn.DisplayName;
                        }
                        else
                        {
                            theColumn.NameOnImport = "";
                        }

                        break;

                    case "chkNameOnExport":
                        if (chkBx.Checked)
                        {
                            theColumn.NameOnExport = theColumn.DisplayName;
                        }
                        else
                        {
                            theColumn.NameOnExport = "";
                        }

                        break;



                }

                RecordManager.ets_Column_Update(theColumn);


            }

           
        }
        catch (Exception ex)
        {

           //

        }
    }

    protected void PopulateImportTemplate(int iTableID)
    {
        ddlTemplate.Items.Clear();

        DataTable dtTemp = Common.DataTableFromText("SELECT  ImportTemplateID,ImportTemplateName,HelpText  FROM ImportTemplate WHERE TableID="
            + iTableID.ToString() + " ORDER BY ImportTemplateName");

        foreach (DataRow dr in dtTemp.Rows)
        {
            ListItem liTemp = new ListItem(dr["ImportTemplateName"].ToString(), dr["ImportTemplateID"].ToString());
         
            ddlTemplate.Items.Add(liTemp);
        }        

        ListItem liSelect = new ListItem("--Please select--", "");
        ddlTemplate.Items.Insert(0, liSelect);

    }
    protected void PopulateTemplates()
    {
         DataTable dtTemp = Common.DataTableFromText(@"
            SELECT DocTemplateID,FileUniqueName,FileName,SPName,DocTemplate.DataRetrieverID FROM DocTemplate INNER JOIN DataRetriever
            ON DocTemplate.DataRetrieverID=DataRetriever.DataRetrieverID
            WHERE TableID=" + _theTable.TableID.ToString());


         gvTemplates.DataSource = dtTemp;
        gvTemplates.DataBind();

        if (dtTemp.Rows.Count > 0)
        {
            divEmptyDataTemplates.Visible = false;
        }
        else
        {
            divEmptyDataTemplates.Visible = true;
        }



    }

//    protected void PopulateOutSavetoTable()
//    {
//        DataTable dtTemp = Common.DataTableFromText(@"SELECT   TableChild.ChildTableID, [Table].TableName 
//                        FROM         [Table] INNER JOIN
//                        TableChild ON [Table].TableID = TableChild.ChildTableID
//                        WHERE [Table].IsActive=1 AND TableChild.ParentTableID=" + _qsTableID);
//        ddlOutSaveToTable.Items.Clear();
//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["TableName"].ToString(), dr["ChildTableID"].ToString());
//            ddlOutSaveToTable.Items.Add(liItem);

//        }

//        ListItem liSelect= new ListItem("--Select Table--","");
//        ddlOutSaveToTable.Items.Insert(0,liSelect);

//    }



//    protected void PopulateInSavetoTable()
//    {
//        DataTable dtTemp = Common.DataTableFromText(@"SELECT   TableChild.ChildTableID, [Table].TableName 
//                        FROM         [Table] INNER JOIN
//                        TableChild ON [Table].TableID = TableChild.ChildTableID
//                        WHERE [Table].IsActive=1 AND TableChild.ParentTableID=" + _qsTableID);

//        ddlInSaveToTable.Items.Clear();
//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["TableName"].ToString(), dr["ChildTableID"].ToString());
//            ddlInSaveToTable.Items.Add(liItem);

//        }

//        ListItem liSelect = new ListItem("--Select Table--", "");
//        ddlInSaveToTable.Items.Insert(0, liSelect);

//    }


//    protected void PopulateIncomingIdentifier()
//    {
//        ddlInIdentifier.Items.Clear();
//        DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
//            (SystemName='recordid' OR(ColumnType='number' AND NumberType=8)) AND TableID=" + _qsTableID);
//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
//            ddlInIdentifier.Items.Add(liItem);
//        }
//        ListItem liSelect = new ListItem("--Select Field--", "");
//        ddlInIdentifier.Items.Insert(0, liSelect);

//    }


//    protected void ddlOutSaveToTable_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        if (ddlOutSaveToTable.SelectedValue == "")
//        {
//            ddlOutSaveBodyTo.Items.Clear();
//            ddlOutSaveRecipient.Items.Clear();
//            ddlOutSaveSubjectto.Items.Clear();

//            ListItem liSelect = new ListItem("--None--", "");
//            ddlOutSaveBodyTo.Items.Insert(0, liSelect);

//            ListItem liSelect2 = new ListItem("--None--", "");
//            ddlOutSaveRecipient.Items.Insert(0, liSelect2);

//            ListItem liSelect3 = new ListItem("--None--", "");
//            ddlOutSaveSubjectto.Items.Insert(0, liSelect3);
//        }
//        else
//        {
//            PopulateOutgoingColumns();
//        }
//    }


    protected void ddlTableTabs_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindTheGrid(0, 1000);

    }
    //protected void ddlInSaveToTable_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    if (ddlInSaveToTable.SelectedValue == "")
    //    {
           
    //        ddlInSaveSubJectTo.Items.Clear();
    //        ddlInSaveBodyTo.Items.Clear();
    //        ddlInSaveToSender.Items.Clear();
    //        ddlInSaveAttachmentTo.Items.Clear();

    //        ListItem liSelect = new ListItem("--None--", "");
    //        ddlInSaveSubJectTo.Items.Insert(0, liSelect);

    //        ListItem liSelect2 = new ListItem("--None--", "");
    //        ddlInSaveBodyTo.Items.Insert(0, liSelect2);

    //        ListItem liSelect3 = new ListItem("--None--", "");
    //        ddlInSaveToSender.Items.Insert(0, liSelect3);

    //        ListItem liSelect4 = new ListItem("--None--", "");
    //        ddlInSaveAttachmentTo.Items.Insert(0, liSelect4);

    //    }
    //    else
    //    {
    //        PopulateIngoingColumns();
    //    }
    //}

   

   
    

    protected void PopulateChildTable()
    {
        DataTable dtTemp = RecordManager.ets_TableChild_Select(int.Parse(_qsTableID));
        grdTable.DataSource = dtTemp;
        grdTable.DataBind();

        if (dtTemp.Rows.Count > 0)
        {
            divEmptyData.Visible = false;
        }
        else
        {
            //divEmptyData.Visible = true;
        }

    }


    protected void PopulateFormSet()
    {
        int iTotalRecords = 0;
        DataTable dtFormGroups = FormSetManager.dbg_FormSetGroup_Select(int.Parse( _qsTableID), "",
            null, null,"", "", null, null, ref iTotalRecords);

        if (dtFormGroups.Rows.Count == 0)
        {
            //create one FormSetGroup

            //FormSetGroup firstFormSetGroup = new FormSetGroup(null, "Forms", 1, _iParentTableID, false);
            //int iFormSetGroupID = FormSetManager.dbg_FormSetGroup_Insert(firstFormSetGroup, null, null);
            //hfFormSetGroupID.Value = iFormSetGroupID.ToString();
            divEmptyDataFormSet.Visible = true;

        }
        else
        {
            int iTR = 0;
            DataTable dtTemp = FormSetManager.dbg_FormSet_Select(int.Parse(dtFormGroups.Rows[0]["FormSetGroupID"].ToString()),
                null,"","RowPosition","ASC",null,null,ref iTR);
            grdFormSet.DataSource = dtTemp;
            grdFormSet.DataBind();

            if (dtTemp.Rows.Count > 0)
            {
                divEmptyDataFormSet.Visible = false;
            }
            else
            {
                divEmptyDataFormSet.Visible = true;
            }

        }

       

    }
    protected void btnRefreshTemplates_Click(object sender, EventArgs e)
    {
        //PopulateChildTable();
        PopulateTemplates();
    }

    protected void btnRefreshGrid_Click(object sender, EventArgs e)
    {
        PopulateChildTable();
    }

    protected void btnRefreshColumns_Click(object sender, EventArgs e)
    {
        BindTheGrid(0,1000);
    }

    protected void btnTableRenameOK_Click(object sender, EventArgs e)
    {
        ViewState["RenameOK"] = "yes";
        lnkSave_Click(null, null);
        ViewState["RenameNo"] = null;
        ViewState["RenameOK"] = null;
    }
    protected void btnTableRenameNo_Click(object sender, EventArgs e)
    {
        ViewState["RenameNo"] = "yes";
        lnkSave_Click(null, null);
        ViewState["RenameNo"] = null;
        ViewState["RenameOK"] = null;
    }
    protected void btnRefreshForms_Click(object sender, EventArgs e)
    {
        PopulateFormSet();
    }

    protected void btnOrderSC_Click(object sender, EventArgs e)
    {
        //
        if (hfOrderSC.Value != "")
        {
            //SqlTransaction tn;
            //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
            //connection.Open();
            //tn = connection.BeginTransaction();

            try
            {
                string strNewSC = hfOrderSC.Value.Substring(0, hfOrderSC.Value.Length - 1);
                string[] newSC = strNewSC.Split(',');

                string strFilter = "";

                //if (chkShowSystemFields.Checked == false)
                //    strFilter = " IsStandard=0 AND ";

                if (ddlTableTabFilter.SelectedValue == "")
                {
                    strFilter = "  TableTabID IS NULL AND ";
                }
                else if (ddlTableTabFilter.SelectedValue == "-1")
                {
                    strFilter = "";
                }
                else
                {
                    strFilter = "  TableTabID= " + ddlTableTabFilter.SelectedValue + " AND ";
                }


                DataTable dtDO = Common.DataTableFromText("SELECT DisplayOrder FROM [Column] WHERE "+strFilter+"  ColumnID IN (" + strNewSC + ") ORDER BY DisplayOrder");

                string strSQL = "";
                if (newSC.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newSC.Length; i++)
                    {
                        strSQL = strSQL + "UPDATE [Column] SET DisplayOrder=" + dtDO.Rows[i][0].ToString() + " WHERE ColumnID=" + newSC[i] + ";";

                        //Column newColumn = RecordManager.ets_Column_Details(int.Parse(newSC[i]));

                        //if (newColumn != null)
                        //{
                        //    newColumn.DisplayOrder = int.Parse(dtDO.Rows[i][0].ToString());
                        //    RecordManager.ets_Column_Update(newColumn);

                        //}
                    }
                }
                if (strSQL != "")
                {
                    Common.ExecuteText(strSQL);
                }

                //tn.Commit();
                //connection.Close();
                //connection.Dispose();
            }
            catch (Exception ex)
            {

                //tn.Rollback();
                //connection.Close();
                //connection.Dispose();

            }
            BindTheGrid(0, _gvPager.TotalRows); 
        }
    }


    protected void btnOrderTC_Click(object sender, EventArgs e)
    {
        //
        if (hfOrderTC.Value != "")
        {
         

            try
            {
                string strNewTC = hfOrderTC.Value.Substring(0, hfOrderTC.Value.Length - 1);
                string[] newTC = strNewTC.Split(',');

                //string strFilter = "";

               

                DataTable dtDO = Common.DataTableFromText("SELECT DisplayOrder,TableChildID FROM [TableChild] WHERE TableChildID IN (" + strNewTC + ") ORDER BY DisplayOrder");
                if (newTC.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newTC.Length; i++)
                    {
                        //Common.ExecuteText("UPDATE TableChild SET DisplayOrder =" + dtDO.Rows[i][0].ToString() + " WHERE TableChildID=" + newTC[i], tn);

                        Common.ExecuteText("UPDATE TableChild SET DisplayOrder ="+i.ToString()+"  WHERE TableChildID=" + newTC[i]);
                       
                    }
                }


            }
            catch (Exception ex)
            {


            }
            PopulateChildTable();
        }
    }


    protected void btnOrderFS_Click(object sender, EventArgs e)
    {
        //
        if (hfOrderFS.Value != "")
        {
           
            try
            {
                string strNewFS = hfOrderFS.Value.Substring(0, hfOrderFS.Value.Length - 1);
                string[] newFS = strNewFS.Split(',');

                //string strFilter = "";

               

                DataTable dtDO = Common.DataTableFromText("SELECT RowPosition,FormSetID FROM [FormSet] WHERE FormSetID IN (" + strNewFS + ") ORDER BY RowPosition");
                if (newFS.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newFS.Length; i++)
                    {
                        Common.ExecuteText("UPDATE FormSet SET RowPosition =" + dtDO.Rows[i][0].ToString() + " WHERE FormSetID=" + newFS[i]);

                    }
                }

            }
            catch (Exception ex)
            {
                //

            }
            PopulateFormSet();
        }
    }


    protected void UpdateTableChild(object sender, EventArgs e)
    {
        CheckBox chkBx = sender as CheckBox;

        if (chkBx != null)
        {
            GridViewRow row = chkBx.NamingContainer as GridViewRow;
            Label lblID = row.FindControl("LblID") as Label;

            TableChild theTableChild = RecordManager.ets_TableChild_Detail(int.Parse(lblID.Text));
            switch (chkBx.ID)
            {
                case "chkShowAddButton":
                    if (chkBx.Checked)
                    {
                        theTableChild.ShowAddButton = true;
                    }
                    else
                    {
                        theTableChild.ShowAddButton = false;
                    }


                    break;

                case "chkShowEditButton":
                    if (chkBx.Checked)
                    {
                        theTableChild.ShowEditButton = true;
                    }
                    else
                    {
                        theTableChild.ShowEditButton = false;
                    }
                    break;





            }

            RecordManager.ets_TableChild_Update(theTableChild);

        }


    }


    protected void grdFormSet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            HyperLink hlAddDetail = e.Row.FindControl("hlAddDetail") as HyperLink;
            if (hlAddDetail != null)
            {
                hlAddDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetDetail.aspx?mode="+Cryptography.Encrypt("add")+"&TableID=" + Request.QueryString["TableID"].ToString();
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");



            HyperLink hlEditDetail = e.Row.FindControl("hlEditDetail") as HyperLink;

            if (hlEditDetail != null)
            {
                hlEditDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&FormSetID=" + Cryptography.Encrypt(DataBinder.Eval(e.Row.DataItem, "FormSetID").ToString());
            }


        }
    }

    protected void grdTable_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            HyperLink hlAddDetail = e.Row.FindControl("hlAddDetail") as HyperLink;
            if (hlAddDetail != null)
            {
                hlAddDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/ChildTableDetail.aspx?ParentTableID=" + _qsTableID;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");



            HyperLink hlEditDetail = e.Row.FindControl("hlEditDetail") as HyperLink;

            if (hlEditDetail != null)
            {
                hlEditDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/ChildTableDetail.aspx?ParentTableID=" + _qsTableID + "&TableChildID=" + DataBinder.Eval(e.Row.DataItem, "TableChildID").ToString();
            }

            Label lblDetailPageType = e.Row.FindControl("lblDetailPageType") as Label;

            if (lblDetailPageType != null)
            {
                switch (DataBinder.Eval(e.Row.DataItem, "DetailPageType").ToString())
                {
                    case "not":
                        lblDetailPageType.Text = "Not displayed";
                        break;
                    case "list":
                        lblDetailPageType.Text = "As a list";
                        break;
                    case "one":
                        lblDetailPageType.Text = "One at a time";
                        break;
                    case "alone":
                        lblDetailPageType.Text = "One Record Only";
                        break;
                }
            }

            CheckBox chkShowWhen = e.Row.FindControl("chkShowWhen") as CheckBox;

            if (chkShowWhen != null)
            {
                if (DataBinder.Eval(e.Row.DataItem, "HideColumnID") == DBNull.Value)
                {
                    chkShowWhen.Checked = false;
                }
                else
                {
                    chkShowWhen.Checked = true;
                }
                chkShowWhen.Enabled = false;
            }
        }
    }



    protected void gvTemplates_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            HyperLink hlAddDetail = e.Row.FindControl("hlAddDetail") as HyperLink;
            if (hlAddDetail != null)
            {
                hlAddDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/DocTemplateDetail.aspx?TableID=" + _qsTableID;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hlEditDetail = e.Row.FindControl("hlEditDetail") as HyperLink;

            if (hlEditDetail != null)
            {
                hlEditDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/DocTemplateDetail.aspx?TableID=" + _qsTableID + "&DocTemplateID=" + DataBinder.Eval(e.Row.DataItem, "DocTemplateID").ToString();
            }          


        }
    }

    protected void grdFormSet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetype")
        {
            Common.ExecuteText("DELETE FormSetForm WHERE FormSetID=" + e.CommandArgument);
            FormSetManager.dbg_FormSet_Delete(int.Parse(e.CommandArgument.ToString()));
            PopulateFormSet();
        }
    }

  

    protected void grdTable_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetype")
        {

            DataTable dtTableChildA = Common.DataTableFromText("SELECT ParentTableID,ChildTableID FROM TableChild where TableChildID=" + e.CommandArgument);

            string strParentTableID = "-1";
            string strChildTableID = "-1";

            if (dtTableChildA.Rows.Count > 0)
            {
                strParentTableID = dtTableChildA.Rows[0][0].ToString();
                strChildTableID = dtTableChildA.Rows[0][1].ToString();
            }

            //Table theChildTable = RecordManager.ets_Table_Details(int.Parse(strChildTableID));
            //if (theChildTable != null)
            //{

            //}           

            //lets check if there is still other records

            DataTable dtTableChild = Common.DataTableFromText("SELECT * FROM TableChild where ParentTableID=" + strParentTableID + " AND ChildTableID=" + strChildTableID);

            if (dtTableChild.Rows.Count > 1)
            {
                RecordManager.ets_TableChild_Delete(Convert.ToInt32(e.CommandArgument));
            }
            else
            {
                //only single record

                DataTable dtColumns = Common.DataTableFromText("SELECT DisplayName FROM [Column] WHERE TableID=" + strChildTableID + " AND TableTableID=" + strParentTableID);
                if (dtColumns.Rows.Count > 0)
                {
                    hfTableChildDeleteID.Value = e.CommandArgument.ToString();
                    mpeModalDeleteTableChild.Show();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please remove the link of " + dtColumns.Rows[0]["DisplayName"].ToString() + " Field of the child table.');", true);
                }
                else
                {
                    //no link
                    RecordManager.ets_TableChild_Delete(Convert.ToInt32(e.CommandArgument));
                }


            }


            PopulateChildTable();

        }

    }



    protected void gvTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetype")
        {
            Common.ExecuteText("DELETE DocTemplate WHERE DocTemplateID=" + e.CommandArgument);
            PopulateTemplates();
        }

    }

    public string GetTableViewURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=";

    }


    protected void PopulateValidateColumn()
    {
            ddlValidateColumnID1.Items.Clear();
            ddlValidateColumnID2.Items.Clear();

            string strTableID = _qsTableID;

            if (ddlParentTable.SelectedValue != "")
            {
                strTableID = ddlParentTable.SelectedValue;
            }
            else
            {
                System.Web.UI.WebControls.ListItem iNone = new System.Web.UI.WebControls.ListItem("--None--", "");
                ddlValidateColumnID1.Items.Insert(0, iNone);

                System.Web.UI.WebControls.ListItem iNone2 = new System.Web.UI.WebControls.ListItem("--None--", "");
                ddlValidateColumnID2.Items.Insert(0, iNone2);
                return;
            }

            DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE IsStandard=0 
                                AND TableID=" + strTableID);

            ddlValidateColumnID1.DataSource = dtTemp;
            ddlValidateColumnID1.DataBind();

            ddlValidateColumnID2.DataSource = dtTemp;
            ddlValidateColumnID2.DataBind();

            System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
            ddlValidateColumnID1.Items.Insert(0, fItem);


            System.Web.UI.WebControls.ListItem fItem2 = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
            ddlValidateColumnID2.Items.Insert(0, fItem2);
        

    }
//    protected void PopulateOutgoingColumns()
//    {
//        ddlOutSaveBodyTo.Items.Clear();
//        ddlOutSaveRecipient.Items.Clear();
//        ddlOutSaveSubjectto.Items.Clear();

//        DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
//            IsStandard=0 AND  (ColumnType='text' OR ColumnType='content' OR ColumnType='staticcontent')  AND TableID=" + ddlOutSaveToTable.SelectedValue);

//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
//            ddlOutSaveBodyTo.Items.Add(liItem);
//        }
//        ListItem liSelect = new ListItem("--Select Field--", "");
//        ddlOutSaveBodyTo.Items.Insert(0, liSelect);

//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
//            ddlOutSaveRecipient.Items.Add(liItem);
//        }
//        ListItem liSelect2 = new ListItem("--Select Field--", "");
//        ddlOutSaveRecipient.Items.Insert(0, liSelect2);

//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
//            ddlOutSaveSubjectto.Items.Add(liItem);
//        }
//        ListItem liSelect3 = new ListItem("--Select Field--", "");
//        ddlOutSaveSubjectto.Items.Insert(0, liSelect3);
//    }


//    protected void PopulateIngoingColumns()
//    {
//        ddlInSaveSubJectTo.Items.Clear();
//        ddlInSaveBodyTo.Items.Clear();
//        ddlInSaveToSender.Items.Clear();
//        ddlInSaveAttachmentTo.Items.Clear();
//        DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
//            IsStandard=0 AND ColumnType='text' AND TableID=" + ddlInSaveToTable.SelectedValue);

//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
//            ddlInSaveSubJectTo.Items.Add(liItem);
//        }
//        ListItem liSelect = new ListItem("--Select Field--", "");
//        ddlInSaveSubJectTo.Items.Insert(0, liSelect);


//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
//            ddlInSaveToSender.Items.Add(liItem);
//        }
//        ListItem liSelectS = new ListItem("--Select Field--", "");
//        ddlInSaveToSender.Items.Insert(0, liSelectS);



//        dtTemp = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
//            IsStandard=0 AND (ColumnType='text' OR ColumnType='content' OR ColumnType='staticcontent') AND TableID=" + ddlInSaveToTable.SelectedValue);

//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
//            ddlInSaveBodyTo.Items.Add(liItem);
//        }
//        ListItem liSelect2 = new ListItem("--Select Field--", "");
//        ddlInSaveBodyTo.Items.Insert(0, liSelect2);


//        dtTemp = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
//            IsStandard=0 AND (ColumnType='file' OR ColumnType='image') AND TableID=" + ddlInSaveToTable.SelectedValue);

//        foreach (DataRow dr in dtTemp.Rows)
//        {
//            ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
//            ddlInSaveAttachmentTo.Items.Add(liItem);
//        }
//        ListItem liSelect3 = new ListItem("--Select Field--", "");
//        ddlInSaveAttachmentTo.Items.Insert(0, liSelect3);

//    }

    //protected void PopulateYAxis()
    //{

    //    DataTable dtSCs = RecordManager.ets_Table_Columns_Summary(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"])));

    //    foreach (DataRow dr in dtSCs.Rows)
    //    {
    //        if (bool.Parse(dr["IsStandard"].ToString()) == false
    //            && dr["ColumnType"].ToString() == "dropdown" && dr["DropDownType"] != DBNull.Value)
    //        {
    //            if (dr["DropDownType"].ToString() == "values")
    //            {
    //                System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(dr["DisplayTextSummary"].ToString(), dr["ColumnID"].ToString());

    //                ddlYAxis.Items.Insert(ddlYAxis.Items.Count, aItem);
    //            }
    //        }

    //    }

    //    System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("--None--", "-1");

    //    ddlYAxis.Items.Insert(0, fItem);

    //}



    protected void PopulateParentTable(int iTableID)
    {
        ddlParentTable.DataSource = Common.DataTableFromText(@"SELECT  DISTINCT ParentTableID,(SELECT TableName FROM [Table] WHERE TableID=[TableChild].ParentTableID) AS TableName 
                    FROM TableChild WHERE ChildTableID=" + iTableID.ToString() + @"  ORDER BY TableName");
        ddlParentTable.DataBind();

        System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("--None--", "");

        ddlParentTable.Items.Insert(0, fItem);
    }

    protected void chkIsPosition_CheckedChanged(object sender, EventArgs e)
    {
        //_bFirstChangePosition = true;
        ViewState["Reload"] = true;
        lnkSave_Click(null, null);
        BindTheGrid(0, _gvPager.TotalRows);
        ViewState["Reload"] = null;
        
    }

    protected void PopulateTerminology()
    {
        //lblLocationCap.Text = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), lblLocationCap.Text, lblLocationCap.Text);

        chkShowSystemFields.Text = chkShowSystemFields.Text.Replace("Fields", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Fields", "Fields"));
        gvTheGrid.Columns[4].HeaderText = gvTheGrid.Columns[4].HeaderText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        stgImportDataColumnHeader.InnerText = stgImportDataColumnHeader.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        pInstruction.InnerText = pInstruction.InnerText.Replace("fields", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Fields", "Fields").ToLower());
        stgFieldsCap.InnerText = stgFieldsCap.InnerText.Replace("Fields", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Fields", "Fields"));
        stgDisplayHeader.InnerText = stgDisplayHeader.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));

       
    }





    protected void lnkSaveFields_Click(object sender, EventArgs e)
    {
        

        try
        {

            foreach (GridViewRow row in gvTheGrid.Rows)
            {
                Label lblID = row.FindControl("LblID") as Label;
                Column theColumn = RecordManager.ets_Column_Details(int.Parse(lblID.Text));

                CheckBox chkDisplayTextSummary = row.FindControl("chkDisplayTextSummary") as CheckBox;
                CheckBox chkDisplayTextDetail = row.FindControl("chkDisplayTextDetail") as CheckBox;
                CheckBox chkIsMandatory = row.FindControl("chkIsMandatory") as CheckBox;
                CheckBox chkDisplayRight = row.FindControl("chkDisplayRight") as CheckBox;
                CheckBox chkNameOnImport = row.FindControl("chkNameOnImport") as CheckBox;
                CheckBox chkNameOnExport = row.FindControl("chkNameOnExport") as CheckBox;
                CheckBox chkAllowCopy = row.FindControl("chkAllowCopy") as CheckBox;

                if (_bTableTabYes)
                {
                    DropDownList ddlTableTab = (DropDownList)row.FindControl("ddlTableTab");
                    if (ddlTableTab.Text == "")
                    {
                        theColumn.TableTabID = null;
                    }
                    else
                    {
                        theColumn.TableTabID = int.Parse(ddlTableTab.SelectedValue);
                    }
                }


                if(_theTable.AllowCopyRecords!=null && (bool)_theTable.AllowCopyRecords)
                {
                    theColumn.AllowCopy = chkAllowCopy.Checked;
                }

                if (chkDisplayTextSummary.Checked)
                {
                    if(theColumn.DisplayTextSummary=="")
                        theColumn.DisplayTextSummary = theColumn.DisplayName;
                }
                else
                {
                    theColumn.DisplayTextSummary = "";
                }

                if (chkDisplayTextDetail.Checked)
                {
                    if (theColumn.DisplayTextDetail == "")
                        theColumn.DisplayTextDetail = theColumn.DisplayName;
                }
                else
                {
                    theColumn.DisplayTextDetail = "";
                }


                theColumn.IsMandatory = chkIsMandatory.Checked;


                if (chkDisplayRight.Checked)
                {
                    theColumn.DisplayRight = true;
                }
                else
                {
                    theColumn.DisplayRight = false;
                }

                if (chkNameOnImport.Checked)
                {
                    if (theColumn.NameOnImport == "")
                        theColumn.NameOnImport = theColumn.DisplayName;
                }
                else
                {
                    theColumn.NameOnImport = "";
                }


                if (chkNameOnExport.Checked)
                {
                    if (theColumn.NameOnExport == "")
                        theColumn.NameOnExport = theColumn.DisplayName;
                }
                else
                {
                    theColumn.NameOnExport = "";
                }


                RecordManager.ets_Column_Update(theColumn);
            }

            

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Field changes are Saved.');", true);


        }
        catch (Exception ex)
        {
            //

        }


        //ViewManager.dbg_View_ResetViewItems(int.Parse(_qsTableID));

        //ViewManager.dbg_View_TableUpdate(int.Parse(_qsTableID));

        BindTheGrid(0, 1000);


    }

    protected void lnkDeleteTableChildOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (hfTableChildDeleteID.Value != "")
            {
                DataTable dtTableChildA = Common.DataTableFromText("SELECT ParentTableID,ChildTableID FROM TableChild where TableChildID=" + hfTableChildDeleteID.Value);

                string strParentTableID = "-1";
                string strChildTableID = "-1";

                if (dtTableChildA.Rows.Count > 0)
                {
                    strParentTableID = dtTableChildA.Rows[0][0].ToString();
                    strChildTableID = dtTableChildA.Rows[0][1].ToString();
                }



                DataTable dtColumns = Common.DataTableFromText("SELECT ColumnID,LinkedParentColumnID,DisplayName FROM [Column] WHERE LinkedParentColumnID IS NOT NULL AND TableID=" + strChildTableID + " AND TableTableID=" + strParentTableID);
                if (dtColumns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtColumns.Rows)
                    {
                        Column theColumn = RecordManager.ets_Column_Details(int.Parse(dr["ColumnID"].ToString()));
                        if (theColumn != null)
                        {
                            theColumn.LinkedParentColumnID = null;
                            theColumn.TableTableID = null;
                            theColumn.DropDownType = "";
                            theColumn.ColumnType = "text";
                            theColumn.DisplayColumn = "";
                            RecordManager.ets_Column_Update(theColumn);
                        }
                    }
                }
                RecordManager.ets_TableChild_Delete(Convert.ToInt32(hfTableChildDeleteID.Value));
                PopulateChildTable();
            }
        }
        catch
        {
            //

        }
        finally
        {
            mpeModalDeleteTableChild.Hide(); 
        }
       
       
    }


    protected void PopulateHeaderDisplay()
    {
        int iTN = 0;
        List<Column> lstColumns = RecordManager.ets_Table_Columns( int.Parse( _qsTableID),
             null, null, ref iTN);
        Column dtColumn = new Column();

        ListItem liAdvanced = new ListItem("--Advanced--", "");
        ddlHeaderText.Items.Add(liAdvanced);

        foreach (Column eachColumn in lstColumns)
        {

            if (eachColumn.IsStandard == false)
            {
                 ListItem liTemp= new ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
                 ddlHeaderText.Items.Add(liTemp);
            }
        }

        ddlDataUpdateUniqueColumnID.Items.Clear();
        ListItem liSelect = new ListItem("--Please select--", "");
        ddlDataUpdateUniqueColumnID.Items.Add(liSelect);


        foreach (Column eachColumn in lstColumns)
        {

            if (eachColumn.IsStandard == false || eachColumn.SystemName.ToLower()=="recordid")
            {
                ListItem liTemp = new ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
                ddlDataUpdateUniqueColumnID.Items.Add(liTemp);
            }
        }

    }

    //protected void lnkAttachementSave_Click(object sender, EventArgs e)
    //{
    //    if (chkAttachOutgoingEmails.Checked)
    //    {
    //        if (ddlOutSaveToTable.SelectedValue == "")
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "AttachmentConfig", "alert('Please select a table');", true);
    //            ddlOutSaveToTable.Focus();
    //            return;
    //        }

            

    //    }

    //    if (chkAttachIncomingEmails.Checked)
    //    {
    //        if (ddlInSaveToTable.SelectedValue == "")
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "AttachmentConfig", "alert('Please select a table');", true);
    //            ddlInSaveToTable.Focus();
    //            return;
    //        }

    //        if (ddlInIdentifier.SelectedValue == "")
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "AttachmentConfig", "alert('Please select an Identifier');", true);
    //            ddlInIdentifier.Focus();
    //            return;
    //        }
    //    }
        
    //    AttachmentPOP3 oAttachePOP3 = new AttachmentPOP3();
    //    oAttachePOP3.Email = txtAttacmentEmail.Text;
    //    oAttachePOP3.Password = txtAttacmentPassword.Text;
    //    oAttachePOP3.POP3Server = txtAttacmentServer.Text;
    //    if(txtAttacmentPort.Text!="")
    //    {
    //        try
    //        {
    //            oAttachePOP3.Port = int.Parse(txtAttacmentPort.Text);
    //        }
    //        catch
    //        {
    //            //
    //        }
    //    }
    //    if (optAttachmentSSL.SelectedValue == "1")
    //    {
    //        oAttachePOP3.SSL = true;
    //    }
    //    else
    //    {
    //        oAttachePOP3.SSL = false;
    //    }
    //    //oAttachePOP3.SSL = bool.Parse( optAttachmentSSL.SelectedValue);

    //    oAttachePOP3.Username = txtAttacmentUserName.Text;

    //    _theTable.JSONAttachmentPOP3 = oAttachePOP3.GetJSONString();

    //    AttachmentSetting oAttachSetting = new AttachmentSetting();

    //    if (chkAttachOutgoingEmails.Checked)
    //    {
    //        oAttachSetting.AttachOutgoingEmails = true;
            
    //        if(ddlOutSaveRecipient.SelectedValue!="")
    //            oAttachSetting.OutSaveRecipientColumnID = int.Parse(ddlOutSaveRecipient.SelectedValue);

    //        oAttachSetting.OutSavetoTableID = int.Parse(ddlOutSaveToTable.SelectedValue);

    //        if(ddlOutSaveSubjectto.SelectedValue!="")
    //            oAttachSetting.OutSaveSubjectColumnID = int.Parse(ddlOutSaveSubjectto.SelectedValue);

    //        if(ddlOutSaveBodyTo.SelectedValue!="")
    //            oAttachSetting.OutSaveBodyColumnID = int.Parse(ddlOutSaveBodyTo.SelectedValue);
            
    //    }


    //    if (chkAttachIncomingEmails.Checked)
    //    {
    //        oAttachSetting.AttachIncomingEmails = true;
    //        oAttachSetting.InIdentifierColumnID = int.Parse(ddlInIdentifier.SelectedValue);
    //        oAttachSetting.InSavetoTableID = int.Parse(ddlInSaveToTable.SelectedValue);

    //        if (ddlInSaveSubJectTo.SelectedValue != "")
    //            oAttachSetting.InSaveSubjectColumnID = int.Parse(ddlInSaveSubJectTo.SelectedValue);

    //        if (ddlInSaveToSender.SelectedValue != "")
    //            oAttachSetting.InSaveSenderColumnID = int.Parse(ddlInSaveToSender.SelectedValue);

    //        if (ddlInSaveBodyTo.SelectedValue != "")
    //            oAttachSetting.InSaveEmailColumnID = int.Parse(ddlInSaveBodyTo.SelectedValue);


    //        if (ddlInSaveAttachmentTo.SelectedValue != "")
    //            oAttachSetting.InSaveAttachmentColumnID = int.Parse(ddlInSaveAttachmentTo.SelectedValue);

    //    }


    //    _theTable.JSONAttachmentInfo = oAttachSetting.GetJSONString();

    //    SqlTransaction tn;
    //    SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //    connection.Open();

    //    tn = connection.BeginTransaction();

    //    RecordManager.ets_Table_Update(_theTable, ref connection, ref tn);

    //    tn.Commit();
    //    connection.Close();
    //    connection.Dispose();

    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AttachmentConfig", "alert('Attachment settings have been saved.');", true);


    //}
    protected void hlCustomUploadSheet_Click(object sender, EventArgs e)
    {
        if (_theTable.CustomUploadSheet != "")
        {

            //string strFilePath = Server.MapPath("~/UserFiles/Template/" + _theTable.CustomUploadSheet);

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
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (IsUserInputOK())
            {

                //int iMenuID = -1;
                
                //if (ddlMenu.SelectedValue == "")
                //{
                //    Menu newMenu = new Menu(null, "--None--",
                //int.Parse(Session["AccountID"].ToString()), false,  true);
                //    iMenuID = RecordManager.ets_Menu_Insert(newMenu);
                //}
                //else if (ddlMenu.SelectedValue == "new")
                //{
                //    if (txtNewMenuName.Text == "")
                //    {
                //        lblMsg.Text = "New Menu Name - Required.";
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "trNewMenuName2", "$('#trNewMenuName').fadeIn();", true);
                //        return;
                //    }

                //    Menu newMenu = new Menu(null, txtNewMenuName.Text,
                //int.Parse(Session["AccountID"].ToString()), true,  true);
                //    iMenuID = RecordManager.ets_Menu_Insert(newMenu);
                //}
                //else
                //{

                //    iMenuID = int.Parse(ddlMenu.SelectedValue);
                //}


                switch (_strActionMode.ToLower())
                {
                    case "add":
                        

                        return;





                    case "edit":
                        //Table editTable = (Table)ViewState["theTable"];

                        Table editTable=RecordManager.ets_Table_Details(int.Parse(_qsTableID));
                        bool? bImportOld = editTable.IsImportPositional;
                        string strOldTableName = editTable.TableName;
                        //editTable.MenuID = iMenuID;
                        editTable.TableName = txtTable.Text.Trim();
                        
                       

                        //editTable.HeaderName = txtHeaderName.Text.Trim();
                        editTable.HeaderName = hfDisplayColumnsFormula.Value;
                        editTable.PinDisplayOrder = null;
                        if(txtPinDisplayOrder.Text!="")
                        {
                            int iTemp = 0;
                            if(int.TryParse(txtPinDisplayOrder.Text.Trim(),out iTemp))
                            {
                                editTable.PinDisplayOrder = iTemp;
                            }
                        }

                        if (editTable.FilterType == "" && ddlFilterType.SelectedValue == "ddl")
                        {
                            editTable.FilterType = "";
                        }
                        else
                        {
                            editTable.FilterType = ddlFilterType.SelectedValue;
                        }

                        editTable.IsImportPositional = chkIsPosition.Checked;
                        //editTable.IsRecordDateUnique = chkUniqueRecordedate.Checked;
                        editTable.UniqueColumnID = null;
                        editTable.UniqueColumnID2 = null;
                        bool isUpdateAllowed = false;
                        if(chkUniqueRecordedate.Checked)
                        {
                            if (ddlUniqueColumnID.SelectedValue != "")
                            {
                                editTable.UniqueColumnID = int.Parse(ddlUniqueColumnID.SelectedValue);
                                isUpdateAllowed = true;
                            }

                            if (ddlUniqueColumnID2.SelectedValue != "")
                                editTable.UniqueColumnID2 = int.Parse(ddlUniqueColumnID2.SelectedValue);
                        }
                        if (isUpdateAllowed)
                            editTable.IsDataUpdateAllowed = chkDataUpdateUniqueColumnID.Checked;
                        else
                            editTable.IsDataUpdateAllowed = false;

                        editTable.NavigationArrows = chkNavigationArrows.Checked;
                        editTable.SaveAndAdd = chkSaveAndAdd.Checked;

                        if (editTable.FlashAlerts == null && chkFlashAlerts.Checked == false)
                        {
                            editTable.FlashAlerts = null;
                        }
                        else
                        {
                            editTable.FlashAlerts = chkFlashAlerts.Checked;
                        }
                        bool bIsMaxTimeChanged = false;

                        

                        if (editTable.ReasonChangeType == "" && ddlReasonChange.SelectedValue == "none")
                        {
                            editTable.ReasonChangeType = "";
                        }
                        else
                        {
                            editTable.ReasonChangeType = ddlReasonChange.SelectedValue;
                        }


                        if (editTable.ChangeHistoryType == "" && ddlChangeHistory.SelectedValue == "always")
                        {
                            editTable.ChangeHistoryType = "";
                        }
                        else
                        {
                            editTable.ChangeHistoryType = ddlChangeHistory.SelectedValue;
                        }


                        editTable.HeaderColor = txtHeaderColor.Text.Trim();
                        editTable.TabColour = txtTabColour.Text.Trim();
                        editTable.TabTextColour= txtTabTextColour.Text.Trim();

                        if (editTable.BoxAroundField == null && chkBoxAroundField.Checked == false)
                        {
                            editTable.BoxAroundField = null;
                        }
                        else
                        {
                            editTable.BoxAroundField = chkBoxAroundField.Checked;
                        }

                        if (editTable.ShowEditAfterAdd == null && chkShowEditAfterAdd.Checked == false)
                        {
                            editTable.ShowEditAfterAdd = null;
                        }
                        else
                        {
                            editTable.ShowEditAfterAdd = chkShowEditAfterAdd.Checked;
                        }

                        editTable.AllowCopyRecords = chkAllowCopyRecords.Checked;
                        editTable.ShowSentEmails = chkShowSentEmails.Checked;
                        editTable.ShowReceivedEmails = chkShowReceivedEmails.Checked;
                        //if (editTable.SaveAndAdd == null && chkSaveAndAdd.Checked == false)
                        //{
                        //    editTable.SaveAndAdd = null;
                        //}
                        //else
                        //{
                        //    editTable.SaveAndAdd = chkSaveAndAdd.Checked;
                        //}



                        editTable.FilterTopColour = txtFilterTopColour.Text.Trim();
                        editTable.FilterBottomColour = txtFilterBottomColour.Text.Trim();

                        if (editTable.ShowTabVertically == null && chkShowTabVertically.Checked == false)
                        {
                            editTable.ShowTabVertically = null;
                        }
                        else
                        {
                            editTable.ShowTabVertically = chkShowTabVertically.Checked;
                        }

                        if (editTable.CopyToChildrenAfterImport == null && chkCopyToChildTables.Checked == false)
                        {
                            editTable.CopyToChildrenAfterImport = null;
                        }
                        else
                        {
                            editTable.CopyToChildrenAfterImport = chkCopyToChildTables.Checked;
                        }



                        editTable.PinImage = ddlPinImages.SelectedValue;

                        if (fuRecordFile.HasFile)
                        {
                            lblMsg.Text = "";
                            Guid guidNew = Guid.NewGuid();
                            string strFileExtension = "";

                            switch (fuRecordFile.FileName.Substring(fuRecordFile.FileName.LastIndexOf('.') + 1).ToLower())
                            {

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

                            if (strFileExtension == "")
                            {
                                lblMsg.Text = "Please select a .csv/.xls/.xlsx file.";
                                return;
                            }


                            string strFileUniqueName;
                            strFileUniqueName = guidNew.ToString() + "_" + fuRecordFile.FileName;

                            fuRecordFile.SaveAs(_strFilesPhisicalPath + "\\UserFiles\\Template\\" + strFileUniqueName);

                            editTable.CustomUploadSheet = strFileUniqueName;
                        }

                        if (editTable.AddWithoutLogin == null && chkAnonymous.Checked == false)
                        {
                            editTable.AddWithoutLogin = null;
                        }
                        else
                        {
                            editTable.AddWithoutLogin = chkAnonymous.Checked;
                        }

                        if (ddlSortColumn.SelectedValue == "")
                        {
                            editTable.SortColumnID = null;
                        }
                        else
                        {
                            editTable.SortColumnID = int.Parse(ddlSortColumn.SelectedValue);
                        }

                        if (chkAddUserRecord.Checked)
                        {
                            editTable.AddUserRecord = true;
                            editTable.AddUserUserColumnID = int.Parse(ddlAddUserUserColumnID.SelectedValue);
                            editTable.AddUserPasswordColumnID = int.Parse(ddlAddUserPasswordColumnID.SelectedValue);
                            editTable.AddUserNotification = chkAddUserNotification.Checked;
                        }
                        else
                        {
                            editTable.AddUserRecord = null;
                            editTable.AddUserUserColumnID = null;
                            editTable.AddUserPasswordColumnID = null;
                            editTable.AddUserNotification = null;
                        }

                        editTable.DataUpdateUniqueColumnID = null;
                        if(chkDataUpdateUniqueColumnID.Checked)
                        {
                            if(ddlDataUpdateUniqueColumnID.SelectedValue!="")
                            {
                                editTable.DataUpdateUniqueColumnID = int.Parse(ddlDataUpdateUniqueColumnID.SelectedValue);
                            }
                        }
                       

                        if (ddlParentTable.SelectedValue == "")
                        {
                            editTable.ParentTableID = null;
                        }
                        else
                        {
                            editTable.ParentTableID = int.Parse(ddlParentTable.SelectedValue);
                        }


                        if (chkAnonymous.Checked)
                        {
                            

                            if(ddlValidateColumnID1.SelectedValue!="")
                             editTable.ValidateColumnID1 = int.Parse(ddlValidateColumnID1.SelectedValue);

                            if(ddlValidateColumnID2.SelectedValue!="")
                                editTable.ValidateColumnID2 = int.Parse(ddlValidateColumnID2.SelectedValue);

                        }
                        else
                        {
                          
                            editTable.ValidateColumnID1 = null;
                            editTable.ValidateColumnID2 = null;
                        }

                        if (txtMaxTimeBetweenRecords.Text.Trim() != "")
                        {
                            if (editTable.MaxTimeBetweenRecords != double.Parse(txtMaxTimeBetweenRecords.Text.Trim()))
                            {
                                bIsMaxTimeChanged = true;
                                editTable.MaxTimeBetweenRecords = double.Parse(txtMaxTimeBetweenRecords.Text.Trim());

                                editTable.MaxTimeBetweenRecordsUnit = ddlMaxTimeBetweenRecordsUnit.Text;
                            }

                            if (editTable.MaxTimeBetweenRecordsUnit != ddlMaxTimeBetweenRecordsUnit.Text.Trim())
                            {
                                bIsMaxTimeChanged = true;
                                editTable.MaxTimeBetweenRecords = double.Parse(txtMaxTimeBetweenRecords.Text.Trim());

                                editTable.MaxTimeBetweenRecordsUnit = ddlMaxTimeBetweenRecordsUnit.Text;
                            }
                        }
                        else
                        {
                            if (editTable.MaxTimeBetweenRecords != null)
                            {
                                bIsMaxTimeChanged = true;
                            }
                            editTable.MaxTimeBetweenRecords = null;
                            editTable.MaxTimeBetweenRecordsUnit = "";
                        }


                        if (txtLateDataDays.Text.Trim() != "")
                        {
                            editTable.LateDataDays = int.Parse(txtLateDataDays.Text.Trim());
                        }
                        else
                        {
                            editTable.LateDataDays = null;
                        }


                        if (ddlGraphXAxisColumnID.SelectedValue == "")
                        {
                            editTable.GraphXAxisColumnID = null;
                        }
                        else
                        {
                            editTable.GraphXAxisColumnID = int.Parse(ddlGraphXAxisColumnID.SelectedValue);
                        }

                        if (ddlGraphSeriesColumnID.SelectedValue == "")
                        {
                            editTable.GraphSeriesColumnID = null;
                        }
                        else
                        {
                            editTable.GraphSeriesColumnID = int.Parse(ddlGraphSeriesColumnID.SelectedValue);
                        }

                        if (ddlDefaultGraphPeriod.SelectedValue == "")
                        {
                            editTable.GraphDefaultPeriod = null;
                        }
                        else
                        {
                            editTable.GraphDefaultPeriod = int.Parse(ddlDefaultGraphPeriod.SelectedValue);
                        }

                        if (ddlGraphDefaultYAxisColumnID.SelectedValue == "")
                        {
                            editTable.GraphDefaultYAxisColumnID = null;
                        }
                        else
                        {
                            editTable.GraphDefaultYAxisColumnID = int.Parse(ddlGraphDefaultYAxisColumnID.SelectedValue);
                        }

                        if (ddlGraphOnStart.SelectedValue == "")
                        {
                            editTable.GraphOnStart = "";
                        }
                        else
                        {
                            editTable.GraphOnStart = ddlGraphOnStart.SelectedValue;
                        }

                        if (txtImportDataStartRow.Text.Trim() != "")
                        {
                            editTable.ImportDataStartRow = int.Parse(txtImportDataStartRow.Text.Trim());
                        }
                        else
                        {
                            editTable.ImportDataStartRow = null;
                        }
                        if(ddlTemplate.SelectedItem!=null && ddlTemplate.SelectedValue!="")
                        {
                            editTable.DefaultImportTemplateID = int.Parse(ddlTemplate.SelectedValue);
                        }
                        else
                        {
                            editTable.DefaultImportTemplateID = null;
                        }


                        if (txtImportColumnHeaderRow.Text.Trim() != "")
                        {
                            editTable.ImportColumnHeaderRow = int.Parse(txtImportColumnHeaderRow.Text.Trim());
                        }
                        else
                        {
                            editTable.ImportColumnHeaderRow = null;
                        }


                        editTable.LastUpdatedUserID = (int)_ObjUser.UserID;

                        //editTable.AddMissingLocation = chkAddMissingLocation.Checked;

                        editTable.FilterColumnID = null;
                        editTable.FilterDefaultValue = "";

                        if (editTable.HideAdvancedOption == null && chkShowAdvancedOptions.Checked)
                        {
                            editTable.HideAdvancedOption = null;
                        }
                        else
                        {

                            editTable.HideAdvancedOption = !chkShowAdvancedOptions.Checked;
                        }

                        editTable.HideFilter = chkHideFilter.Checked;

                        //if (ddlYAxis.SelectedItem != null)
                        //{
                        //    if (ddlYAxis.SelectedIndex != 0)
                        //    {
                        //        if (ddlFilterValue.SelectedItem != null)
                        //        {
                        //            if (ddlFilterValue.SelectedIndex != 0)
                        //            {
                        //                editTable.FilterColumnID = int.Parse(ddlYAxis.SelectedValue);
                        //                editTable.FilterDefaultValue = ddlFilterValue.SelectedValue;
                        //            }
                        //        }
                        //    }
                        //}

                        if (cbcvSumFilter.ddlYAxisV != "" && cbcvSumFilter.GetValue != "")
                        {
                            editTable.FilterColumnID = int.Parse(cbcvSumFilter.ddlYAxisV);
                            editTable.FilterDefaultValue = cbcvSumFilter.GetValue;
                        }
                        else
                        {
                            editTable.FilterColumnID = null;
                            editTable.FilterDefaultValue = "";
                        }

                        //need a single transaction


                        if (strOldTableName != editTable.TableName)
                        {
                            if (ViewState["RenameOK"] == null && ViewState["RenameNo"] == null)
                            {
                                //trResetViews.Visible = true;OpenTableRenameConfirm,   setTimeout(function () { OpenAlertBox(); }, currentOpts.speedOut);

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "jsTableRenameConfirm", "setTimeout(function () { OpenTableRenameConfirm(); }, 1000);", true);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "jsTableRenameConfirm", "OpenTableRenameConfirm();", true);

                                return;

                            }

                        }

                        if(chkSummaryPageContent.Checked==false)
                        {
                            editTable.SummaryPageContent = "";
                        }

                     
                        int iTotalNewRecordsEffected = 0;

                        try
                        {
                            int iIsUpdated = RecordManager.ets_Table_Update(editTable);

                            if (bIsMaxTimeChanged)
                            {

                                string strMsg = RecordManager.AdjusTMaxTimeBetweenRecords((int)editTable.TableID,  ref iTotalNewRecordsEffected);

                            }




                            if(strOldTableName!=editTable.TableName)
                            {
                                TheDatabaseS.Table_TableNameRename((int)editTable.TableID, strOldTableName, editTable.TableName);

                                if(ViewState["RenameOK"]!=null)
                                {
                                    string strMenuID = Common.GetValueFromSQL("SELECT MenuID FROM [Menu] WHERE IsActive=1 AND TableID=" + editTable.TableID.ToString());

                                    if(strMenuID!="")
                                    {
                                        Menu theMenu = RecordManager.ets_Menu_Details(int.Parse(strMenuID));

                                        if (theMenu != null)
                                        {
                                            theMenu.MenuP = editTable.TableName;
                                            RecordManager.ets_Menu_Update(theMenu);
                                            ViewState["RenameOK"] = null;
                                        }
                                    }
                                   
                                }
                            }


                            if (ViewState["Reload"] == null)
                            {
                                Response.Redirect(hlBack.NavigateUrl, false);
                            }


                            //if (bImportOld == editTable.IsImportPositional && strOldTableName == editTable.TableName)
                            //{
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "parent.$.fancybox.close();", true);
                            //}
                            //else
                            //{
                            //    //window.opener.location.reload(false);
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "window.parent.location.reload(); parent.$.fancybox.close();", true);

                            //}

                        }
                        catch (Exception ex)
                        {
                       
                            lblMsg.Text = ex.Message;
                            ViewState["Reload"] = null;
                            //throw;
                        }




                        break;
                }
            }
            else
            {
                //user input is not ok

            }

            //temp
            //if (_bFirstChangePosition == false)
            //{
            //    Response.Redirect(hlBack.NavigateUrl, false);
            //}
            //else
            //{
            //    _bFirstChangePosition = false;
            //}
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

            string strTable = "Table";
            strTable = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table");

            lblMsg.Text = "Can not add this " + strTable + ", you may have this " + strTable + "!";
        }

        ViewState["Reload"] = null;
    }


    protected void ddlParentTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateValidateColumn();
    }

   
  

}
