using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.DynamicData;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using ChartDirector;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Globalization;
using DocGen.Utility;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using ExcelNS;
using DocGen.DAL;
public partial class DBGGraphControl : UserControl
{

    DataTable dtYdata;
    DataTable[] dtYDT;
    string _strXAxisLabel = "";
    bool _bDotClick = false;
    int _iChartWidth = 700;
    int _iChartHeight = 500;
    int _iNoOfSeries = 0;
    bool _bGotFirstDate = false;
    User _ObjUser;
    Account _theAccount;
    GraphOption _gGraphOption = null;

    bool _bAvoidSize = false;

    //bool _bByPassToDate = false;
    bool? _bToDateUp = null;
    //int _iStartIndex = 0;
    //int _iMaxRows = 500;

    int? _iDocumentID = null;
    int? _iDocumentSectionID = null;
    int? _iPreDocumentSectionID = null;

    bool _bOldDate = false;
    int? _iGraphOptionID = null;
    int? _GraphOptionDetailID = null;

    //Common_Pager _gvPager;


    string _strParentPage = "home";
    string _strBackURL = "/Default.aspx";

    int? _iOneTableID = null;

    string _strSaveFileName = "";


    private static string _tempDirectory = "../TempFiles/Graph/";
    private static int _tempFileLifeTime = 3600; /* time in seconds */

    bool _bWarningExceedShown = false;

    public event EventHandler ZoomGraph;
    public event EventHandler CloseZoomGraph;

    bool? _bOverWriteGraph = null;

    ChartDashBoard _theChartDashBoard = new ChartDashBoard();
    Table _theTable;
    bool _bFromSelectedGraph = false;
    protected void btnUpdateGraphs_Click(object sender, EventArgs e)
    {
        try
        {
            if (hfSelectedGraphs.Value != "")
            {
                _gGraphOption = GraphManager.ets_GraphOption_Detail(int.Parse(hfSelectedGraphs.Value));
                if (_gGraphOption != null)
                {
                    //if (DocumentID != null)
                    //    _gGraphOption.UserReportDate = false;

                    _iGraphOptionID = (int)_gGraphOption.GraphOptionID;
                    int iINTemp = 0;
                    ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)_iGraphOptionID, null, "GraphOrder", "ASC",
                        null, null, ref iINTemp);
                    ViewState["GraphOptionDetailCount"] = iINTemp;
                    DataTable theTableGraphOptionDetail = (DataTable)ViewState["GraphOptionDetail"];
                    int i = 1;
                    foreach (DataRow dr in theTableGraphOptionDetail.Rows)
                    {
                        dr["GraphOptionDetailID"] = i;
                        i = i + 1;
                    }
                    theTableGraphOptionDetail.AcceptChanges();
                    ViewState["GraphOptionDetail"] = theTableGraphOptionDetail;

                    ddlGraphOption.SelectedValue = (-1 * _iGraphOptionID).ToString();

                    //GraphOptionID = null;
                    //ViewState["Mode"] = "add";
                    //Mode = "add";
                    //try
                    //{
                    //    ddlGraphOption.SelectedValue = (-1 * _iGraphOptionID).ToString();
                    //}
                    //catch
                    //{
                    //    System.Web.UI.WebControls.ListItem liNew = new System.Web.UI.WebControls.ListItem("--New--",
                    //        "");
                    //    ddlGraphOption.Items.Insert(0, liNew);
                    //    ddlGraphOption.SelectedValue = "";

                    //}
                    //ViewState["newGraphOption"] = null;
                    _bFromSelectedGraph = true;
                    PopulateRecord();
                    //chkUseReportDates.Checked = false;
                  
                    MakeChart();
                }


            }
        }
        catch(Exception ex)
        {
            //
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (AccountID == null)
        {
            if (Session["LoginAccount"] == null)
            {
                Response.Redirect("http://" + base.Request.Url.Authority + base.Request.ApplicationPath + "/Login.aspx");
            }
            else
            {
                Response.Redirect("http://" + base.Request.Url.Authority + base.Request.ApplicationPath + "/Login.aspx?" + Session["LoginAccount"].ToString());
            }
            return;
        }
        _theAccount = SecurityManager.Account_Details((int)AccountID);

        if (!IsPostBack)
        {
            hlAddNewDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath +
                "/Pages/UserControl/GraphOptionDetail.aspx?GraphOptionDetailID=-1&ModeDetail=add";
            hlFolderOpen.NavigateUrl = "~/Pages/Graph/GraphPopup.aspx?graphbase=control";

            hlBack.NavigateUrl = BackURL;

            PopulateTerminology();
            PopulateTableDDL();

            if (DocumentID != null)
            {
                Mode = ManageDocument(DocumentID);
            }


            if (DocumentSectionID != null && !IsPostBack)
            {
                Mode = "edit";
            }

            //tblNextPrevious.Visible = false;

            switch (ParentPage.ToLower())
            {
                case "home":
                    //SetPlotAreaLayout();
                    lnkZoom.Visible = false;
                    break;
                case "main":
                    tdClickHelp.Visible = true;
                    break;
                case "mobile":
                    //SetPlotAreaLayout();
                    break;
            }

            if (Mode == "")
            {
                Server.Transfer("~/Empty.aspx");
            }

            if (Mode == "add")
            {
                int iINTemp = 0;
                ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select(-1, null, "GraphOrder", "ASC",
                    null, null, ref iINTemp);
                ViewState["GraphOptionDetailCount"] = 0;
                //divEmptyData.Visible = true;

                System.Web.UI.WebControls.ListItem liNewGraph = new System.Web.UI.WebControls.ListItem("--New Graph--", "");
                ddlGraphOption.Items.Insert(0, liNewGraph);
            }

            if (GraphOptionID != null)
            {
                ddlGraphOption.Text = "-" + GraphOptionID.ToString();
                int iINTemp = 0;
                ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID,
                    null, "GraphOrder", "ASC", null, null, ref iINTemp);
                ViewState["GraphOptionDetailCount"] = iINTemp;
            }

            PopulateYAxis();
        }
        else
        {
            //SynchronizeViews();
            FormatDates();
        }

        if (_gGraphOption != null)
        {
            if (!IsPostBack)
            {
                //BindTheGrid(0, _iMaxRows);
            }

            if (_gGraphOption.TimePeriod != "")
            {
                if (!IsPostBack)
                {
                    ddlTimePeriodDisplay_Simple.Text = _gGraphOption.TimePeriod;
                }
            }
        }

        if (GraphOptionID == null && OneTableID == null)
        {
            if (ParentPage == "home" && _theAccount.DefaultGraphOptionID != null)
            {
                if (!IsPostBack)
                {
                    //GraphOptionID = (int)_theAccount.DefaultGraphOptionID;
                    ddlGraphOption.Text = "-" + GraphOptionID.ToString();
                }
            }
            else
            {
                if (ddlGraphOption.SelectedValue.IndexOf("-") > -1)
                {
                    GraphOptionID = int.Parse(ddlGraphOption.SelectedValue.Replace("-", "").ToString());
                }
            }
        }

        if (DocumentSectionID != null)
        {
            lnkZoom.Visible = false;

            using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
            {
                DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);

                if (section.Details != "")
                {
                    GraphOptionID = int.Parse(section.Details);

                    int iINTemp = 0;
                    ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
                  null, null, ref iINTemp);
                    ViewState["GraphOptionDetailCount"] = iINTemp;
                }
            }
        }

        if (GraphOptionID != null)
            _gGraphOption = GraphManager.ets_GraphOption_Detail((int)GraphOptionID);

        if (!IsPostBack)
        {
            if (_gGraphOption != null)
            {
                if (_gGraphOption.TimePeriod.ToString() != "")
                {
                    ddlTimePeriodDisplay_Simple.Text = _gGraphOption.TimePeriod.ToString();//mr
                }
            }

            if (OneTableID != null)
            {
                PopulateOneTableRecord();
            }

            

            InitSimpleView();
            PopulateGraphDefinitionDDL();

            if (Mode.ToLower() != "add")
            {
                PopulateRecord();
            }


            if (SetInitialDate())
            {
                SetPrevNextDateState();
                MakeChart();
            }
        }
        else
        {
            GetDateTimeSelectorsValue();
        }

        if (Mode == "edit")
        {
            hlFolderOpen.Visible = false;
        }

        //GridViewRow gvr = gvTheGrid.TopPagerRow;
        //if (gvr != null)
        //{
        //    _gvPager = (Common_Pager)gvr.FindControl("Pager");
        //    _gvPager.AddURL = "javascript:AddNewGraphOptionDetail();";
        //}

        ShowHide();

        if (OneTableID != null && DocumentSectionID==null)
        {
            Mode = "add";
        }

        SetDateTimeSelectorsState();

       // if ((ddlGraphOption.SelectedValue != "") && (OneTableID != null))
            ManageSeries();

        SetScripts();
    }


    private string ManageDocument(int? documentID)
    {
        string ret = null;

        if (documentID != null)
        {
            lblTimePeriodDisplay_Simple.Text = "Display";
            if (DocumentSectionID == null)
            {
                Mode = "add";
            }
            else
            {
                Mode = "edit";
            }

            lnkCloseZoom.Visible = false;
            lnkZoom.Visible = false;
            divBack.Visible = false;
            lnkExcel.Visible = false;
            //lnkRefresh.Visible = false;
            divMain.ActiveViewIndex = 2;

            //if (Mode == "add")
            //{
            //    txtPlotAreaWidth.Text = ChartWidth.ToString();
            //}
            //Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);

            //if (theDocument.ForDashBoard != null)
            //{
            //    if ((bool)theDocument.ForDashBoard)
            //    {
            //        //lblReportDates.Text = "Use Recent Dates";
            //        //trStartDate.Visible = false;
            //        //trEndDate.Visible = false;
            //        //trRecentDays.Visible = true;
            //        //if (chkUseReportDates.Checked)
            //        //{
            //        //    txtRecentDays.Enabled = true;
            //        //}
            //    }
            //}
        }

        return ret;
    }


    //private void SynchronizeViews()
    //{
    //    if (divMain.ActiveViewIndex == 0)
    //    {
    //        txtGraphTitle.Text = txtGraphTitle_Simple.Text;
    //        //ddlTimePeriodDisplay.SelectedValue = ddlTimePeriodDisplay_Simple.SelectedValue;

            

    //        //txtStartDate.Text = txtStartDate_Simple.Text;
    //        //txtFromTime.Text = txtFromTime_Simple.Text;
    //        //txtEndDate.Text = txtEndDate_Simple.Text;
    //        //txtToTime.Text = txtToTime_Simple.Text;

    //        txtWarningCaption.Text = txtWarningCaption_Simple.Text;
    //        txtWarningValue.Text = txtWarningValue_Simple.Text;
    //        txtExceedanceCaption.Text = txtExceedanceCaption_Simple.Text;
    //        txtExceedanceValue.Text = txtExceedanceValue_Simple.Text;
    //        ddlWarningColor.SelectedIndex = ddlWarningColor_Simple.SelectedIndex;
    //        ddlExceedanceColor.SelectedIndex = ddlExceedanceColor_Simple.SelectedIndex;
    //    }
    //    else
    //    {
    //        txtGraphTitle_Simple.Text = txtGraphTitle.Text;
    //        //ddlTimePeriodDisplay_Simple.SelectedValue = ddlTimePeriodDisplay.SelectedValue;

    //        //txtStartDate_Simple.Text = txtStartDate.Text;
    //        //txtFromTime_Simple.Text = txtFromTime.Text;
    //        //txtEndDate_Simple.Text = txtEndDate.Text;
    //        //txtToTime_Simple.Text = txtToTime.Text;

    //        txtWarningCaption_Simple.Text = txtWarningCaption.Text;
    //        txtWarningValue_Simple.Text = txtWarningValue.Text;
    //        txtExceedanceCaption_Simple.Text = txtExceedanceCaption.Text;
    //        txtExceedanceValue_Simple.Text = txtExceedanceValue.Text;
    //        ddlWarningColor_Simple.SelectedIndex = ddlWarningColor.SelectedIndex;
    //        ddlExceedanceColor_Simple.SelectedIndex = ddlExceedanceColor.SelectedIndex;
    //    }
    //}


    private bool SetInitialDate()
    {
        string dateFormat = "dmy";
        string strTableID = ddlGraphOption.SelectedValue;
        string graphXAxisColumnName = GetGraphXAxisColumnName(ddlGraphOption.SelectedValue == "" ? "-1" : ddlGraphOption.SelectedValue);

        if (String.IsNullOrEmpty(graphXAxisColumnName))
        {
            MakeNoDataChart();
            return false;
        }

        DateTime? dtFromDateTime = DateTime.Now.AddYears(-1);
        DateTime? dtToDateTime = DateTime.Now;

        DataTable dtMaxTime = null;

        if (ddlGraphOption.SelectedValue.IndexOf("-") > -1)
        {
            dtMaxTime = Common.DataTableFromText(
                String.Format(@"SET dateformat {0}; " +
                "SELECT MAX(CAST(CASE WHEN ISDATE(Record.[{1}]) > 0 THEN Record.[{1}] ELSE NULL END AS datetime)), " +
                "MIN(CAST(CASE WHEN ISDATE(Record.[{1}]) > 0 THEN Record.[{1}] ELSE NULL END AS datetime)), " + 
                "GraphOptionDetail.TableID " +
                "FROM Record INNER JOIN GraphOptionDetail " +
                "ON Record.TableID = GraphOptionDetail.TableID " +
                "WHERE Record.IsActive=1 AND GraphOptionDetail.GraphOptionID={2} " +
                "GROUP BY GraphOptionDetail.TableID",
                dateFormat, graphXAxisColumnName, ddlGraphOption.SelectedValue.Replace("-", "")));

            if ((dtMaxTime != null) && (dtMaxTime.Rows.Count > 0) && !dtMaxTime.Rows[0].IsNull(2))
            {
                strTableID = dtMaxTime.Rows[0][2].ToString();
            }
        }
        else
        {
            if (strTableID != "")
            {
                dtMaxTime = Common.DataTableFromText(
                    String.Format(@"SET dateformat {0};SELECT MAX(CAST(CASE WHEN ISDATE(Record.[{1}]) > 0 THEN Record.[{1}] ELSE NULL END AS datetime)), MIN(CAST(CASE WHEN ISDATE(Record.[{1}]) > 0 THEN Record.[{1}] ELSE NULL END AS datetime)) " +
                    "FROM Record WHERE Record.IsActive=1 AND TableID={2}", dateFormat, graphXAxisColumnName, strTableID));
            }
        }

        if ((dtMaxTime != null) && (dtMaxTime.Rows.Count > 0) && !dtMaxTime.Rows[0].IsNull(0))
        {
            dtToDateTime = (DateTime)dtMaxTime.Rows[0][0];

            Table theTable = RecordManager.ets_Table_Details(int.Parse(strTableID));
            if (theTable != null && theTable.GraphDefaultPeriod.HasValue)
            {
                int periodID = theTable.GraphDefaultPeriod.Value;
                //<asp:ListItem Value="0">All</asp:ListItem>
                //<asp:ListItem Value="1">1 year</asp:ListItem>
                //<asp:ListItem Value="2">6 months</asp:ListItem>
                //<asp:ListItem Value="3">3 months</asp:ListItem>
                //<asp:ListItem Value="4">1 month</asp:ListItem>
                //<asp:ListItem Value="5">1 week</asp:ListItem>
                //<asp:ListItem Value="6">1 day</asp:ListItem>
                switch (periodID)
                {
                    case 0:
                        dtFromDateTime = (DateTime)dtMaxTime.Rows[0][1];
                        break;
                    case 1:
                        dtFromDateTime = dtToDateTime.Value.AddYears(-1);
                        break;
                    case 2:
                        dtFromDateTime = dtToDateTime.Value.AddMonths(-6);
                        break;
                    case 3:
                        dtFromDateTime = dtToDateTime.Value.AddMonths(-3);
                        break;
                    case 4:
                        dtFromDateTime = dtToDateTime.Value.AddMonths(-1);
                        break;
                    case 5:
                        dtFromDateTime = dtToDateTime.Value.AddDays(-7);
                        break;
                    case 6:
                        dtFromDateTime = dtToDateTime.Value.AddDays(-1);
                        break;
                    default:
                        dtFromDateTime = (DateTime)dtMaxTime.Rows[0][1];
                        break;
                }
            }
            else
            {
                dtFromDateTime = (DateTime)dtMaxTime.Rows[0][1];
            }

            ViewState["dtMaxDate"] = (DateTime)dtMaxTime.Rows[0][0];
            ViewState["dtMinDate"] = (DateTime)dtMaxTime.Rows[0][1];
        }

        //if (chkUseReportDates.Checked && DocumentID != null)
        //{
        //    Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);
        //    if (theDocument != null)
        //    {
        //        if (theDocument.DocumentDate != null)
        //        {
        //            dtFromDateTime = theDocument.DocumentDate;
        //        }
        //        if (theDocument.DocumentEndDate != null)
        //        {
        //            dtToDateTime = theDocument.DocumentEndDate;
        //        }
        //    }
        //}

        ViewState["dtToDateTime"] = dtToDateTime;
        ViewState["dtFromDateTime"] = dtFromDateTime;
        
        SetDateTimeSelectorsValue();
        return true;
    }


    private void FormatDates()
    {
        if (txtStartDate_Simple.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtStartDate_Simple.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                //txtStartDate_Simple.Text = dtTemp.ToShortDateString();
                txtStartDate_Simple.Text = dtTemp.ToShortDateString();
            }
        }
        else
        {
            //txtFromTime_Simple.Text = String.Empty;
            txtFromTime_Simple.Text = String.Empty;
        }
        if (txtEndDate_Simple.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtEndDate_Simple.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                //txtEndDate_Simple.Text = dtTemp.ToShortDateString();
                txtEndDate_Simple.Text = dtTemp.ToShortDateString();
            }
        }
        else
        {
            //txtToTime_Simple.Text = String.Empty;
            txtToTime_Simple.Text = String.Empty;
        }
    }


    //private void SetPlotAreaLayout()
    //{
    //    txtPlotAreaWidth.Text = ChartWidth.ToString();
    //    txtPlotAreaHeight.Text = ChartHeight.ToString();
    //}


    private void SetScripts()
    {
        string strJS = @"
            $(function () {
                $('.popupdetail').fancybox({
                    scrolling: 'no',
                    type: 'iframe',
                    'transitionIn': 'elastic',
                    'transitionOut': 'none',
                    width: 500,
                    height: 550,
                    titleShow: false
                });
            });

            $(function () {
                $('#hlAddNewDetail').fancybox({
                    scrolling: 'no',
                    type: 'iframe',
                    'transitionIn': 'elastic',
                    'transitionOut': 'none',
                    width: 500,
                    height: 550,
                    titleShow: false
                });
            });

            $(function () {
                $('.popupgraph').fancybox({
                    scrolling: 'auto',
                    type: 'iframe',
                    width: 800,
                    height: 400,
                    titleShow: false
                });
            });

            $(function () {
                $('#lnkHcscript').fancybox({
                    showCloseButton: false,
                    hideOnOverlayClick: false,
                    enableEscapeButton: false,
                    'transitionIn': 'elastic',
                    'transitionOut': 'none',
                    //closeClick: false,
                    //helpers: {
                    //    overlay: {
                    //        closeClick: false
                    //    }
                    //},
                    //keys: {
                    //    close: null
                    //}
                });
            });
        ";
        if(!IsPostBack)
        {
            if (ParentPage == "section" && DocumentSectionID==null)
            {
                strJS = strJS + " setTimeout(function () { FolderOpenClick(); }, 1000);";
            }
        }
       
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JSforAjax", strJS, true);
    }


    private void PopulateGraphDefinitionDDL()
    {
        int tableID = 0;
        int columnID = 0;

        ddlGraphType_Simple.Items.Clear();

        //if (!int.TryParse(ddlGraphOption.SelectedValue, out tableID))
        //{
        //    return;
        //}

        int iTN = 0;
        string strTableID = ddlGraphOption.SelectedValue;

        if (_gGraphOption != null)
        {
            DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
            if (theTable != null && theTable.Rows.Count > 0)
            {
                tableID = (int)theTable.Rows[0]["TableID"];
                strTableID = tableID.ToString();
            }
           
        }
        if (strTableID == "")
            return;


        tableID = int.Parse(strTableID);

        List<Column> lstColumns = RecordManager.ets_Table_Columns(tableID,
                null, null, ref iTN);
        if (lstColumns != null)
        {
            string s="-1";
            Column column;
            //if(ddlYAxis.SelectedValue!="")
            //{
            //    s = ddlYAxis.SelectedValue;               
            //}
            //else
            //{
                s = ddlEachAnalyte_Simple.SelectedValue;
            //}
            column = lstColumns.AsQueryable<Column>().Where(x => x.SystemName == s).FirstOrDefault();

            
            if ((column != null) && column.ColumnID.HasValue)
            {
                columnID = column.ColumnID.Value;

                int defaultDefinitionID = -1;
                if (column.DefaultGraphDefinitionID.HasValue)
                {
                    defaultDefinitionID = column.DefaultGraphDefinitionID.Value;
                }

                DataTable dt = GraphManager.ets_GraphDefinition_Select(null, null, null, null, null,
                    tableID, columnID,
                    null,
                    true,
                    null, null, null, null,
                    null, null, null, null, ref iTN);

                bool bUseDefault = false;
                foreach(DataRow row in dt.Rows)
                {
                    ListItem li = new ListItem(row["DefinitionName"].ToString(), row["GraphDefinitionID"].ToString());
                    if ((int)row["GraphDefinitionID"] == defaultDefinitionID)
                    {
                        li.Selected = true;
                        bUseDefault = true;
                    }
                    ddlGraphType_Simple.Items.Add(li);
                }

                if(!bUseDefault)
                {
                    string s2 = SystemData.SystemOption_ValueByKey_Account("DefaultGraphDefinitionID", null, null);
                    if (!String.IsNullOrEmpty(s2))
                    {
                        ddlGraphType_Simple.SelectedValue = s2;
                    }
                }
            }
        }

    }

    protected void PopulateOneTableRecord()
    {
        //ViewState["ModeDetail"] = "add";
        Mode = "add";
        ddlGraphOption.Text = OneTableID.ToString();
        PopulateYAxis();

        if (ddlEachAnalyte_Simple.SelectedItem != null)
        {
            txtGraphTitle_Simple.Text = ddlGraphOption.SelectedItem.Text;
            txtGraphSubtitle_Simple.Text = ddlEachAnalyte_Simple.SelectedItem.Text;
        }

        int iINTemp = 0;
        ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select(-1, null, "GraphOrder", "ASC",
      null, null, ref iINTemp);
        ViewState["GraphOptionDetailCount"] = 0;


        DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
        DataRow theRecord = theTable.NewRow();
        theRecord["GraphOptionDetailID"] = theTable.Rows.Count + 1;
        theRecord["GraphOptionID"] = -1;
        theRecord["TableID"] = int.Parse(ddlGraphOption.SelectedValue);
        theRecord["SystemName"] = ddlEachAnalyte_Simple.SelectedValue;
        DataTable dtTemp = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE   TableID=" + ddlGraphOption.SelectedValue + " AND SystemName='" + ddlEachAnalyte_Simple.SelectedValue + "'");
        if (dtTemp.Rows.Count > 0)
        {
            if (dtTemp.Rows[0][0] != DBNull.Value)
            {
                theRecord["ColumnID"] = int.Parse(dtTemp.Rows[0][0].ToString());
            }
        }


        theRecord["Axis"] = "Left";
        theRecord["Colour"] = "";
        theRecord["GraphType"] = "line";
        theRecord["TableName"] = ddlGraphOption.SelectedItem.Text;
        if (ddlEachAnalyte_Simple.SelectedItem != null)
            theRecord["GraphLabel"] = ddlEachAnalyte_Simple.SelectedItem.Text;
        //theRecord["LocationName"] = "All";

        theTable.Rows.Add(theRecord);
        theTable.AcceptChanges();
        ViewState["GraphOptionDetail"] = theTable;
        ViewState["GraphOptionDetailCount"] = (int)ViewState["GraphOptionDetailCount"] + 1;

       // BindTheGrid(0, _iMaxRows);

    }

    protected void InitSimpleView()
    {
        int iTN = 0;

        //ddlEachTable_Simple.DataSource = RecordManager.ets_Table_Select(null,
        //    null,
        //    null,
        //    (int)AccountID,
        //    null, null, true,
        //    "st.TableName", "ASC",
        //    null, null, ref iTN, STs);
        //ddlEachTable_Simple.DataBind();

        //System.Web.UI.WebControls.ListItem liPlease = new System.Web.UI.WebControls.ListItem("--Please Select--", "-1");
        //ddlEachTable.Items.Insert(0, liPlease);

        //ddlEachTable.Text = DataBinder.Eval(e.Row.DataItem, "TableID").ToString();

        //ddlEachAnalyte_Simple.Items.Clear();
        string strTableID = ddlGraphOption.SelectedValue;
        if(_gGraphOption!=null)
        {
            DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];

            if(theTable!=null && theTable.Rows.Count>0)
            {
                int tableID = (int)theTable.Rows[0]["TableID"];
                strTableID = tableID.ToString();
            }
           
        }
    


       

        if (strTableID == "")
            strTableID = "-1";

        //List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(strTableID),
        //        null, null, ref iTN);

        //Column dtColumn = new Column();
        //foreach (Column eachColumn in lstColumns)
        //{
        //    if (eachColumn.IsStandard == true)
        //    {
        //        switch (eachColumn.SystemName.ToLower())
        //        {
        //            case "datetimerecorded":
        //                ViewState["DateTimeSummary"] = eachColumn.GraphLabel;
        //                break;

        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        if (eachColumn.GraphLabel != "" && (eachColumn.ColumnType == "number" || eachColumn.ColumnType == "calculation"))
        //        {
        //            System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.SystemName);
        //            ddlEachAnalyte_Simple.Items.Insert(ddlEachAnalyte_Simple.Items.Count, aItem);
        //        }
        //    }
        //}

        //if (ddlEachAnalyte_Simple.SelectedItem != null)
        //{
        //    txtGraphTitle_Simple.Text = ddlGraphOption.SelectedItem.Text;
        //    txtGraphSubtitle_Simple.Text = ddlEachAnalyte_Simple.SelectedItem.Text;
        //}
        //ddlEachTable_Simple.Text = ddlGraphOption.SelectedValue;
        //ddlEachAnalyte_Simple.Text = ddlYAxis.SelectedValue;
        PopulateYAxis();
        PopulateSampleSitesList(strTableID);

    }


    private void PopulateSampleSitesList(string strTableID)
    {
        SampleSitesList.Items.Clear();

        int? graphSeriesColumnID = GetGraphSeriesColumnID(strTableID);
        if (graphSeriesColumnID.HasValue && (graphSeriesColumnID.Value > -1))
        {

            RecordManager.PopulateTableCheckBoxList(graphSeriesColumnID.Value, ref SampleSitesList);
           // Column theColumn = RecordManager.ets_Column_Details(graphSeriesColumnID.Value);
            //string graphSeriesDisplayColumnName = "[" + theColumn.SystemName + "]";
            //string graphSeriesIDColumnName = theColumn.SystemName;

            //if (theColumn.LinkedParentColumnID != null && theColumn.TableTableID != null && theColumn.DisplayColumn != "" &&
            //    (theColumn.ColumnType == "dropdown" || theColumn.ColumnType == "listbox"))
            //{
            //    Column theLinkedParentColumn = RecordManager.ets_Column_Details(((int)theColumn.LinkedParentColumnID));
            //    graphSeriesIDColumnName = theLinkedParentColumn.SystemName;
            //    graphSeriesDisplayColumnName = RecordManager.fnReplaceDisplayColumns(theColumn.DisplayColumn, (int)theColumn.TableTableID);
            //    strTableID = theColumn.TableTableID.ToString();
            //}

            //DataTable dtTemp;

            //if("[" + graphSeriesIDColumnName + "]"==graphSeriesDisplayColumnName)
            //{
            //    dtTemp = Common.DataTableFromText(
            //    String.Format("SELECT DISTINCT [{0}] FROM Record WHERE TableID={1} AND IsActive=1 ORDER BY [{0}]",
            //        graphSeriesIDColumnName, strTableID));
            //}
            //else
            //{
            //    dtTemp = Common.DataTableFromText(
            //    String.Format("SELECT DISTINCT [{0}], {1} FROM Record WHERE TableID={2} AND IsActive=1 ORDER BY {1}",
            //        graphSeriesIDColumnName, graphSeriesDisplayColumnName, strTableID));
            //}

            



            //if (dtTemp != null)
            //{
            //    if (dtTemp.Columns.Count>1)
            //    {
            //        foreach (DataRow dr in dtTemp.Rows)
            //        {

            //            SampleSitesList.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
            //        }
            //    }
            //    else
            //    {
            //        foreach (DataRow dr in dtTemp.Rows)
            //        {

            //            SampleSitesList.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
            //        }
            //    }
                
               
            //}
        }
        else
        {
            trSeries.Visible = false;
        }
    }



    private void SetSampleSitesList(string strTableID, DataTable theTable)
    {
        SampleSitesList.Items.Clear();

        int? graphSeriesColumnID = GetGraphSeriesColumnID(strTableID);
        if (graphSeriesColumnID.HasValue && (graphSeriesColumnID.Value > -1))
        {
            RecordManager.PopulateTableCheckBoxList(graphSeriesColumnID.Value, ref SampleSitesList);
            //Column theColumn = RecordManager.ets_Column_Details(graphSeriesColumnID.Value);
            //string graphSeriesDisplayColumnName = "[" + theColumn.SystemName + "]";
            //string graphSeriesIDColumnName = theColumn.SystemName;

            //if (theColumn.LinkedParentColumnID != null && theColumn.TableTableID != null && theColumn.DisplayColumn != "" &&
            //    (theColumn.ColumnType == "dropdown" || theColumn.ColumnType == "listbox"))
            //{
            //    Column theLinkedParentColumn = RecordManager.ets_Column_Details(((int)theColumn.LinkedParentColumnID));
            //    graphSeriesIDColumnName = theLinkedParentColumn.SystemName;
            //    graphSeriesDisplayColumnName = RecordManager.fnReplaceDisplayColumns(theColumn.DisplayColumn, (int)theColumn.TableTableID);
            //    strTableID = theColumn.TableTableID.ToString();
            //}

            //DataTable dtTemp;

            //if ("[" + graphSeriesIDColumnName + "]" == graphSeriesDisplayColumnName)
            //{
            //    dtTemp = Common.DataTableFromText(
            //    String.Format("SELECT DISTINCT [{0}] FROM Record WHERE TableID={1} AND IsActive=1 ORDER BY [{0}]",
            //        graphSeriesIDColumnName, strTableID));
            //}
            //else
            //{
            //    dtTemp = Common.DataTableFromText(
            //    String.Format("SELECT DISTINCT [{0}], {1} FROM Record WHERE TableID={2} AND IsActive=1 ORDER BY {1}",
            //        graphSeriesIDColumnName, graphSeriesDisplayColumnName, strTableID));
            //}


           


            //if (dtTemp != null)
            //{

                //foreach (DataRow row in theTable.Rows)
                //{
                //    if (!row.IsNull("GraphSeriesID"))
                //    {
                //        foreach (DataRow dr in dtTemp.Rows)
                //        {
                //            if (dr[0].ToString() == row["GraphSeriesID"].ToString())
                //            {
                //                if (dtTemp.Columns.Count > 1)
                //                {
                //                    SampleSitesList.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                //                }
                //                else
                //                {
                //                    SampleSitesList.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
                //                }
                //                break;
                //            }
                //        }
                //    }
                //}


                //if (dtTemp.Columns.Count > 1)
                //{
                //    foreach (DataRow dr in dtTemp.Rows)
                //    {
                //        if (SampleSitesList.Items.FindByValue(dr[0].ToString()) == null)
                //            SampleSitesList.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                //    }
                //}
                //else
                //{
                //    foreach (DataRow dr in dtTemp.Rows)
                //    {
                //        if (SampleSitesList.Items.FindByValue(dr[0].ToString()) == null)
                //            SampleSitesList.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
                //    }
                //}


                foreach (DataRow row in theTable.Rows)
                {
                    if (!row.IsNull("GraphSeriesID"))
                    {
                        foreach (ListItem item in SampleSitesList.Items)
                        {
                            if (item.Value == row["GraphSeriesID"].ToString())
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }
                }
               
            //}

            foreach (System.Web.UI.WebControls.ListItem li in SampleSitesList.Items)
            {
                li.Attributes.Add("DataValue", li.Value);
            }

        }
        else
        {
            trSeries.Visible = false;
        }

      
    }

    private void ManageSeries()
    {
        if (divMain.ActiveViewIndex == 0)
        {
            DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
            if(theTable!=null && theTable.Rows.Count>0)
            {

            }
            else
            {
                return;
            }

            int tableID = (int)theTable.Rows[0]["TableID"];

            int ColumnID = 0;
            DataTable dtTemp = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE TableID=" + tableID +
                " AND SystemName='" + ddlEachAnalyte_Simple.SelectedValue + "'");
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0][0] != DBNull.Value)
                {
                    ColumnID = int.Parse(dtTemp.Rows[0][0].ToString());
                }
            }

            if (ColumnID != 0)
            {
                bool bFirst = true;
                int firstRow = -1;
                List<int> rowstoDelete = new List<int>();

                for (int i = 0; i < theTable.Rows.Count; i++)
                {
                    DataRow dr = theTable.Rows[i];
                    if (
                        //(dr["TableID"].ToString() == ddlGraphOption.SelectedValue) &&
                        (Convert.ToInt32(dr["ColumnID"]) == ColumnID)
                        )
                    {
                        if (bFirst)
                        {
                            firstRow = i;
                            bFirst = false;
                        }
                        else
                            rowstoDelete.Add(i);
                    }
                }
                foreach (int i in rowstoDelete)
                {
                    theTable.Rows[i].Delete();
                }

                if (firstRow != -1)
                {
                    bFirst = true;
                    foreach (ListItem li in SampleSitesList.Items)
                    {
                        if (li.Selected)
                        {
                            if (bFirst)
                            {
                                theTable.Rows[firstRow]["GraphSeriesColumnID"] = GetGraphSeriesColumnName(tableID.ToString());
                                theTable.Rows[firstRow]["GraphSeriesID"] = li.Value;
                                bFirst = false;
                            }
                            else
                            {
                                DataRow theRecord = theTable.NewRow();
                                theRecord["GraphOptionDetailID"] = theTable.Rows.Count + 1;
                                theRecord["GraphOptionID"] = -1;
                                theRecord["TableID"] = theTable.Rows[firstRow]["TableID"];
                                theRecord["SystemName"] = theTable.Rows[firstRow]["SystemName"];
                                theRecord["ColumnID"] = theTable.Rows[firstRow]["ColumnID"];
                                theRecord["Axis"] = theTable.Rows[firstRow]["Axis"]; ;
                                theRecord["Colour"] = "";
                                theRecord["GraphType"] = theTable.Rows[firstRow]["GraphType"]; ;
                                theRecord["TableName"] = theTable.Rows[firstRow]["TableName"]; ;
                                theRecord["GraphLabel"] = theTable.Rows[firstRow]["GraphLabel"];
                                //theRecord["LocationName"] = "All";
                                theRecord["GraphSeriesColumnID"] = theTable.Rows[firstRow]["GraphSeriesColumnID"];
                                theRecord["GraphSeriesID"] = li.Value;

                                theTable.Rows.Add(theRecord);
                            }
                        }
                    }
                }
                theTable.AcceptChanges();
                ViewState["GraphOptionDetail"] = theTable;
                ViewState["GraphOptionDetailCount"] = theTable.Rows.Count;
                //BindTheGrid(0, _iMaxRows);
            }
            else
            {
                MakeNoDataChart();
            }
        }
    }


    private void ClearWarnings()
    {
        chkShowLimits_Simple.Checked = false;

        //trWarning.Visible = false;
        //trExceedance.Visible = false;
        trWarning_Simple.Visible = false;
        trExceedance_Simple.Visible = false;
    }

    private void ClearYAxisLimits()
    {
        txtYHighestValue.Text = String.Empty;
        txtYLowestValue.Text = String.Empty;
        txtYInterval.Text = String.Empty;
    }



    protected void ShowHide()
    {
        if (ParentPage == "home" || ParentPage == "mobile")
        {
            divOptionControls.Visible = false;

        }
        if (ParentPage != "home")
        {
            //hlMoreGraphAlt.Visible = false;
            //hlMoreGraph.Visible = false;
        }
    }

    //protected void BindTheGrid(int iStartIndex, int iMaxRows)
    //{
    //    int iTN = 0;

    //    if (Mode == "edit")
    //    {
    //        if (_gGraphOption == null)
    //            return;

    //        gvTheGrid.DataSource = GraphManager.ets_GraphOptionDetail_Select((int)_gGraphOption.GraphOptionID, null, "GraphOrder", "ASC",
    //            iStartIndex, iMaxRows, ref iTN);
    //        gvTheGrid.VirtualItemCount = iTN;

    //        if (ViewState["GraphOptionDetail"] == null)
    //        {
    //            ViewState["GraphOptionDetail"] = (DataTable)gvTheGrid.DataSource;
    //            ViewState["GraphOptionDetailCount"] = iTN;
    //        }
    //    }
    //    else
    //    {
    //        gvTheGrid.DataSource = (DataTable)ViewState["GraphOptionDetail"];

    //        iTN = (int)ViewState["GraphOptionDetailCount"];
    //        gvTheGrid.VirtualItemCount = iTN;
    //    }

    //    gvTheGrid.DataBind();

    //    GridViewRow gvr = gvTheGrid.TopPagerRow;
    //    if (gvr != null)
    //    {
    //        _gvPager = (Common_Pager)gvr.FindControl("Pager");
    //        _gvPager.AddURL = "javascript:AddNewGraphOptionDetail();";

    //    }

    //    if (iTN == 0)
    //    {
    //        divEmptyData.Visible = true;
    //    }
    //    else
    //    {
    //        divEmptyData.Visible = false;
    //    }

    //    DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
    //    string strTableID = "-1";
    //    string strColumnID = "-1";
    //    string strAxis = "Left";
    //    if (theTable.Rows.Count > 0)
    //    {
    //        int iPreIndex = theTable.Rows.Count - 1;
    //        strTableID = theTable.Rows[iPreIndex]["TableID"].ToString();
    //        strColumnID = theTable.Rows[iPreIndex]["ColumnID"].ToString();

    //        int iR = theTable.Rows.Count % 2;
    //        if (iR != 0)
    //        {
    //            strAxis = "Right";
    //        }
    //        else
    //        {
    //            strAxis = "Left";
    //        }
    //    }
    //    else
    //    {
    //        //first row;           
    //        iTN = 0;
    //        List<Table> lstST = RecordManager.ets_Table_Select(null,
    //        null,
    //        null,
    //        (int)AccountID,
    //        null, null, true,
    //        "st.TableName", "ASC",
    //        null, null, ref  iTN, STs);

    //        if (lstST.Count > 0)
    //        {
    //            strTableID = lstST[0].TableID.ToString();
    //            List<Column> lstColumns = RecordManager.ets_Table_Columns((int)lstST[0].TableID,
    //                       null, null, ref iTN);

    //            Column dtColumn = new Column();
    //            foreach (Column eachColumn in lstColumns)
    //            {
    //                if (eachColumn.IsStandard == false)
    //                {
    //                    if (eachColumn.GraphLabel != "" && eachColumn.ColumnType == "number")
    //                    {
    //                        strColumnID = eachColumn.ColumnID.ToString();
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        strAxis = "Left";
    //    }

    //    hlAddNewDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath +
    //        "/Pages/UserControl/GraphOptionDetail.aspx?GraphOptionDetailID=-1&ModeDetail=add&Axis=" + strAxis +
    //        "&TableID=" + strTableID + "&ColumnID=" + strColumnID;
    //}

    protected void StoreTheGraphOption()
    {
        GraphOption newGraphOption = new GraphOption(null, (int)AccountID,
                    2, txtGraphTitle_Simple.Text, ddlTimePeriodDisplay_Simple.SelectedValue);

        //newGraphOption.Display3D = chk3DEnabled.Checked;
        //newGraphOption.UserReportDate = chkUseReportDates.Checked;
        //if (chkUseReportDates.Checked == false)
        //{
            if (txtStartDate_Simple.Text != "")
            {
                newGraphOption.FromDate = DateTime.ParseExact(txtFromTime_Simple.Text == "" ? txtStartDate_Simple.Text + " 00:00" : txtStartDate_Simple.Text + " " + txtFromTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                newGraphOption.ToDate = DateTime.ParseExact(txtToTime_Simple.Text == "" ? txtEndDate_Simple.Text + " 00:00" : txtEndDate_Simple.Text + " " + txtToTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
            }
        //}

        if (ddlTimePeriodDisplay_Simple.SelectedValue == "C")
        {
            if (txtStartDate_Simple.Text != "")
            {
                newGraphOption.FromDate = DateTime.ParseExact(txtFromTime_Simple.Text == "" ? txtStartDate_Simple.Text + " 00:00" : txtStartDate_Simple.Text + " " + txtFromTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                newGraphOption.ToDate = DateTime.ParseExact(txtToTime_Simple.Text == "" ? txtEndDate_Simple.Text + " 00:00" : txtEndDate_Simple.Text + " " + txtToTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
            }
            newGraphOption.TimePeriod = "C";
            newGraphOption.CustomTimePeriod = ddlTimePeriodDisplay_Simple.SelectedValue;

        }

        //newGraphOption.HideDate = chkHideDate.Checked;


        //if (txtPlotAreaWidth.Text != "")
        //    newGraphOption.Width = double.Parse(txtPlotAreaWidth.Text);
        //if (txtPlotAreaHeight.Text != "")
        //    newGraphOption.Height = double.Parse(txtPlotAreaHeight.Text);

        newGraphOption.IsActive = true;

        //newGraphOption.Legend = rblLegendPosition.SelectedValue;
        newGraphOption.ReportChart = false; //??


        newGraphOption.ShowLimits = chkShowLimits_Simple.Checked;
        //newGraphOption.ShowMissing = chkShowDottedLine.Checked;
        ViewState["newGraphOption"] = newGraphOption;
    }

    protected void BlankField()
    {

        //txtEndDate.Text = "";
        //txtStartDate.Text = "";
        //txtFromTime.Text = "";
        //txtToTime.Text = "";
        if (OneTableID == null)
        {
            txtGraphTitle_Simple.Text = "";
        }
        //chkHideDate.Checked = false;
        //ddlDateFormat.SelectedValue = "A";
        chkShowLimits_Simple.Checked = false;
        //chkShowDottedLine.Checked = false;
        //rblLegendPosition.Items[0].Selected = true;


    }


    protected void PopulateTerminology()
    {
        //gvTheGrid.Columns[2].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), gvTheGrid.Columns[2].HeaderText, gvTheGrid.Columns[2].HeaderText);
        //gvTheGrid.Columns[3].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), gvTheGrid.Columns[3].HeaderText, gvTheGrid.Columns[3].HeaderText);
    }

    protected void PopulateRecord()
    {
        chkDateRange.Checked = false;
        txtRecentNumber.Text = "";
        ddlRecentPeriod.SelectedValue = "Y";
        if (DocumentSectionID != null)
        {
            using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
            {
                DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);

                if (!IsPostBack)
                {
                    if (section.Details != "")
                    {
                        GraphOptionID = int.Parse(section.Details);
                        //ddlGraphOption.Text = "-" + GraphOptionID.ToString();
                        _gGraphOption = GraphManager.ets_GraphOption_Detail((int)GraphOptionID);
                    }

                    Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);

                    if (theDocument.ForDashBoard != null)
                    {
                        if ((bool)theDocument.ForDashBoard)
                        {
                            if (section.ValueFields != "")
                            {
                                //txtRecentDays.Text = section.ValueFields;
                                try
                                {
                                    
                                    _theChartDashBoard = JSONField.GetTypedObject<ChartDashBoard>(section.ValueFields);

                                    if (_theChartDashBoard!=null)
                                    {
                                        if (_theChartDashBoard.RecentNumber != null)
                                            txtRecentNumber.Text = _theChartDashBoard.RecentNumber.ToString();
                                        if (string.IsNullOrEmpty(_theChartDashBoard.RecentPeriod) == false)
                                            ddlRecentPeriod.SelectedValue = _theChartDashBoard.RecentPeriod;
                                        chkDateRange.Checked = true;
                                    }
                                    //else
                                    //{
                                    //    if(_bFromSelectedGraph && _gGraphOption!=null)
                                    //    {

                                    //    }
                                    //}
                                    
                                }
                                catch
                                {
                                    //
                                }
                            }
                            else
                            {
                                //chkUseReportDates.Checked = false;
                                //txtRecentDays.Enabled = false;

                            }
                        }
                    }
                }
            }
        }
      

        if (Mode == "add")
        {
            if (ViewState["newGraphOption"] != null && _gGraphOption == null)
            {
                _gGraphOption = (GraphOption)ViewState["newGraphOption"];

            }
        }

        if (_gGraphOption != null)
        {
            //ddlGraphOption.Text = _gGraphOption.GraphOptionID.ToString();
            //txtGraphTitle.Text = _gGraphOption.Heading;
            txtGraphTitle_Simple.Text = _gGraphOption.Heading;
            txtGraphSubtitle_Simple.Text = _gGraphOption.SubHeading;

            if (ddlGraphType_Simple.Items.FindByValue(_gGraphOption.GraphDefinitionID.ToString()) == null)
            {
                ddlGraphType_Simple.Items.Add(new ListItem("*", _gGraphOption.GraphDefinitionID.ToString()));
            }
            ddlGraphType_Simple.SelectedValue = _gGraphOption.GraphDefinitionID.ToString();

            if (_gGraphOption.ShowLimits != null)
            {
                //chkShowLimits.Checked = (bool)_gGraphOption.ShowLimits;
                chkShowLimits_Simple.Checked = (bool)_gGraphOption.ShowLimits;
            }

            if (chkShowLimits_Simple.Checked)
            {
                //trWarning.Visible = true;
                trWarning_Simple.Visible = true;
                //txtWarningCaption.Text = _gGraphOption.WarningCaption;
                txtWarningCaption_Simple.Text = _gGraphOption.WarningCaption;
                if (_gGraphOption.WarningValue != null)
                {
                    //txtWarningValue.Text = _gGraphOption.WarningValue.ToString();
                    txtWarningValue_Simple.Text = _gGraphOption.WarningValue.ToString();
                }
                if (_gGraphOption.WarningColor != "")
                {
                    //ddlWarningColor.Value = _gGraphOption.WarningColor;
                    ddlWarningColor_Simple.Value = _gGraphOption.WarningColor;
                }

                //trExceedance.Visible = true;
                trExceedance_Simple.Visible = true;
                //txtExceedanceCaption.Text = _gGraphOption.ExceedanceCaption;
                txtExceedanceCaption_Simple.Text = _gGraphOption.ExceedanceCaption;
                if (_gGraphOption.ExceedanceValue != null)
                {
                    //txtExceedanceValue.Text = _gGraphOption.ExceedanceValue.ToString();
                    txtExceedanceValue_Simple.Text = _gGraphOption.ExceedanceValue.ToString();
                }
                if (_gGraphOption.ExceedanceColor != "")
                {
                    //ddlExceedanceColor.Value = _gGraphOption.ExceedanceColor;
                    ddlExceedanceColor_Simple.Value = _gGraphOption.ExceedanceColor;
                }
            }
            else
            {
                //trWarning.Visible = false;
                trWarning_Simple.Visible = false;
                //trExceedance.Visible = false;
                trExceedance_Simple.Visible = false;
            }


            //if (_gGraphOption.ShowMissing != null)
            //    chkShowDottedLine.Checked = (bool)_gGraphOption.ShowMissing;
            //if (_gGraphOption.Display3D != null)
            //    chk3DEnabled.Checked = (bool)_gGraphOption.Display3D;

            if (ParentPage != "home" && ParentPage != "mobile")
            {
                //txtPlotAreaWidth.Text = _gGraphOption.Width.ToString();
                //txtPlotAreaHeight.Text = _gGraphOption.Height.ToString();
            }
            //else
            //{
            //    if (IsDashBorad)
            //    {
            //        txtPlotAreaWidth.Text = _gGraphOption.Width.ToString();
            //        txtPlotAreaHeight.Text = _gGraphOption.Height.ToString();
            //    }
            //}

            //if (_gGraphOption.Legend != "")
            //    rblLegendPosition.SelectedValue = _gGraphOption.Legend;

            //if (_gGraphOption.Display3D != null)
            //    chk3DEnabled.Checked = (bool)_gGraphOption.Display3D;

            //if (_gGraphOption.DateFormat != "")
            //    ddlDateFormat.Text = _gGraphOption.DateFormat.Trim();

            //if (_gGraphOption.UserReportDate != null)
            //{
            //    if ((bool)_gGraphOption.UserReportDate == false)
            //    {
            //        chkUseReportDates.Checked = false;
            //        if (_gGraphOption.FromDate != null && _gGraphOption.ToDate != null)
            //        {
            //            //txtStartDate.Text = ConvertUtil.GetDateString((DateTime)_gGraphOption.FromDate);
            //            //txtEndDate.Text = ConvertUtil.GetDateString((DateTime)_gGraphOption.ToDate);
            //            txtStartDate_Simple.Text = ConvertUtil.GetDateString((DateTime)_gGraphOption.FromDate);
            //            txtEndDate_Simple.Text = ConvertUtil.GetDateString((DateTime)_gGraphOption.ToDate);

            //            //txtFromTime.Text = ((DateTime)_gGraphOption.FromDate).Hour.ToString() + ":" + ((DateTime)_gGraphOption.FromDate).Minute.ToString();
            //            //txtToTime.Text = ((DateTime)_gGraphOption.ToDate).Hour.ToString() + ":" + ((DateTime)_gGraphOption.ToDate).Minute.ToString();
            //            txtFromTime_Simple.Text = ((DateTime)_gGraphOption.FromDate).Hour.ToString() + ":" + ((DateTime)_gGraphOption.FromDate).Minute.ToString();
            //            txtToTime_Simple.Text = ((DateTime)_gGraphOption.ToDate).Hour.ToString() + ":" + ((DateTime)_gGraphOption.ToDate).Minute.ToString();
            //        }
            //        trStartDate.Visible = true;
            //        trEndDate.Visible = true;

            //    }
            //    else
            //    {
            //        trStartDate.Visible = false;
            //        trEndDate.Visible = false;

            //        chkUseReportDates.Checked = true;
            //    }

            //}

            if (_gGraphOption.YAxisHighestValue.HasValue)
                txtYHighestValue.Text = _gGraphOption.YAxisHighestValue.Value.ToString();
            if (_gGraphOption.YAxisLowestValue.HasValue)
                txtYLowestValue.Text = _gGraphOption.YAxisLowestValue.Value.ToString();
            if (_gGraphOption.YAxisInterval.HasValue)
                txtYInterval.Text = _gGraphOption.YAxisInterval.Value.ToString();

            if (DocumentID == null)
            {
                if (_gGraphOption.FromDate != null && _gGraphOption.ToDate != null)
                {
                    //chkCustomDate.Checked = true;

                    //if (ParentPage != "home")
                    //{
                    ddlTimePeriodDisplay_Simple.SelectedValue = "C";
                    //}
                    //trStartDate.Visible = true;
                    //trEndDate.Visible = true;
                    txtStartDate_Simple.Text = ConvertUtil.GetDateString((DateTime)_gGraphOption.FromDate);
                    txtEndDate_Simple.Text = ConvertUtil.GetDateString((DateTime)_gGraphOption.ToDate);

                    txtFromTime_Simple.Text = ((DateTime)_gGraphOption.FromDate).Hour.ToString() + ":" + ((DateTime)_gGraphOption.FromDate).Minute.ToString();
                    txtToTime_Simple.Text = ((DateTime)_gGraphOption.ToDate).Hour.ToString() + ":" + ((DateTime)_gGraphOption.ToDate).Minute.ToString();
                }
            }
            else
            {
                //if(_bFromSelectedGraph && _gGraphOption.TimePeriod!="C")
                //{
                //    Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);
                //    if (theDocument.ForDashBoard != null && (bool)theDocument.ForDashBoard)
                //    {
                //        chkDateRange.Checked = true;
                //        txtRecentNumber.Text = "1";
                //        ddlRecentPeriod.SelectedValue = _gGraphOption.TimePeriod;
                //    }
                //}
             

            }
        }

        DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
        
        if (theTable.Rows.Count > 0)
        {
            //SampleSitesList.ClearSelection();

            Column col = RecordManager.ets_Column_Details((int)theTable.Rows[0]["ColumnID"]);
          
           



            if (col != null)
            {
                ddlEachAnalyte_Simple.SelectedValue = col.SystemName;
                //if (ddlYAxis.Items.FindByValue(col.SystemName) == null)
                //{
                //    ddlYAxis.Items.Add(new ListItem(col.GraphLabel, col.SystemName));
                //}
                //ddlYAxis.SelectedValue = col.SystemName;

                SetSampleSitesList(col.TableID.ToString(), theTable);
            }

            //foreach (DataRow row in theTable.Rows)
            //{
            //    if (!row.IsNull("GraphSeriesID"))
            //    {
            //        foreach (ListItem item in SampleSitesList.Items)
            //        {
            //            if (item.Value == row["GraphSeriesID"].ToString())
            //            {
            //                item.Selected = true;
            //                break;
            //            }
            //        }
            //    }
            //}

            


        }


       // BindTheGrid(0, _iMaxRows);
    }

    //protected void ddlEachTable_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    try
    //    {

    //        int iTN = 0;
    //        DropDownList ddlEachTable = sender as DropDownList;
    //        int iGraphOptionDetailID = 1;
    //        if (ddlEachTable != null)
    //        {
    //            GridViewRow row = ddlEachTable.NamingContainer as GridViewRow;
    //            DropDownList ddlEachAnalyte = row.FindControl("ddlEachAnalyte") as DropDownList;
    //            Label LblID = row.FindControl("LblID") as Label;
    //            iGraphOptionDetailID = int.Parse(LblID.Text);
    //            if (ddlEachAnalyte != null)
    //            {
    //                ddlEachAnalyte.Items.Clear();
    //                string strTableID = ddlEachTable.SelectedValue;

    //                List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(strTableID),
    //                       null, null, ref iTN);

    //                Column dtColumn = new Column();
    //                foreach (Column eachColumn in lstColumns)
    //                {
    //                    if (eachColumn.IsStandard == true)
    //                    {
    //                        switch (eachColumn.SystemName.ToLower())
    //                        {
    //                            case "datetimerecorded":
    //                                ViewState["DateTimeSummary"] = eachColumn.GraphLabel;
    //                                break;

    //                            default:
    //                                break;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (eachColumn.GraphLabel != "" && eachColumn.ColumnType == "number")
    //                        {
    //                            System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.SystemName);

    //                            ddlEachAnalyte.Items.Insert(ddlEachAnalyte.Items.Count, aItem);
    //                        }
    //                    }
    //                }


    //            }

    //            //update virtual table
    //            DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
    //            int i = 0;
    //            foreach (DataRow dr in theTable.Rows)
    //            {
    //                if (dr["GraphOptionDetailID"].ToString() == iGraphOptionDetailID.ToString())
    //                {
    //                    theTable.Rows[i]["TableID"] = int.Parse(ddlEachTable.SelectedValue);
    //                    DataTable dtTemp = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE   TableID=" + ddlEachTable.SelectedValue.ToString() + " AND SystemName='" + ddlEachAnalyte.SelectedValue + "'");

    //                    if (dtTemp.Rows.Count > 0)
    //                    {
    //                        if (dtTemp.Rows[0]["ColumnID"] != null && dtTemp.Rows[0]["ColumnID"].ToString() != "")
    //                        {
    //                            theTable.Rows[i]["ColumnID"] = int.Parse(dtTemp.Rows[0]["ColumnID"].ToString());
    //                        }
    //                    }

    //                    theTable.Rows[i]["SystemName"] = ddlEachAnalyte.SelectedValue;
    //                    theTable.Rows[i]["TableName"] = ddlEachTable.SelectedItem.Text;
    //                    theTable.Rows[i]["GraphLabel"] = ddlEachAnalyte.SelectedItem.Text;
    //                    //theTable.Rows[i]["LocationName"] = "All";
    //                    //theTable.Rows[i]["LocationID"] = DBNull.Value;
    //                    theTable.Rows[i]["Low"] = DBNull.Value;
    //                    theTable.Rows[i]["High"] = DBNull.Value;
    //                    theTable.AcceptChanges();
    //                    ViewState["GraphOptionDetail"] = theTable;

    //                    if (Mode == "edit")
    //                    {

    //                        GraphOptionDetail theGraphOptionDetail = GraphManager.ets_GraphOptionDetail_Detail(iGraphOptionDetailID);

    //                        if (theGraphOptionDetail != null)
    //                        {
    //                            theGraphOptionDetail.TableID = int.Parse(ddlEachTable.SelectedValue); ;
    //                            theGraphOptionDetail.ColumnID = int.Parse(theTable.Rows[i]["ColumnID"].ToString());
    //                            //theGraphOptionDetail.LocationID = null;

    //                            theGraphOptionDetail.Axis = theTable.Rows[i]["Axis"].ToString();
    //                            theGraphOptionDetail.GraphType = theTable.Rows[i]["GraphType"].ToString();
    //                            theGraphOptionDetail.Colour = null;
    //                            theGraphOptionDetail.Low = null;
    //                            theGraphOptionDetail.High = null;
    //                            theGraphOptionDetail.Label = null;
    //                            GraphManager.ets_GraphOptionDetail_Update(theGraphOptionDetail);

    //                            int iINTemp = 0;
    //                            ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
    //                    null, null, ref iINTemp);
    //                            ViewState["GraphOptionDetailCount"] = iINTemp;
    //                        }
    //                    }
    //                }
    //                i = i + 1;
    //            }

    //            //if (gvTheGrid.Rows.Count == 1)
    //            //{
    //                //txtGraphTitle.Text = ddlEachTable.SelectedItem.Text + " - " + ddlEachAnalyte.SelectedItem.Text;
    //                ddlGraphOption.Text = ddlEachTable.SelectedValue;
    //                ddlGrpahOption_SelectedIndexChanged(null, null);
    //            //}
    //        }

    //      //  BindTheGrid(0, _iMaxRows);

    //        //BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);

    //        _bOldDate = true;
    //        MakeChart();
    //    }
    //    catch
    //    {
    //        MakeNoDataChart();
    //    }
    //}


    //protected void ddlEachTableEdit_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int iTN = 0;
    //    DropDownList ddlEachTableEdit = sender as DropDownList;
    //    if (ddlEachTableEdit != null)
    //    {
    //        GridViewRow row = ddlEachTableEdit.NamingContainer as GridViewRow;
    //        DropDownList ddlEachAnalyteEdit = row.FindControl("ddlEachAnalyteEdit") as DropDownList;

    //        if (ddlEachAnalyteEdit != null)
    //        {
    //            ddlEachAnalyteEdit.Items.Clear();
    //            string strTableID = ddlEachTableEdit.SelectedValue;

    //            List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(strTableID),
    //                   null, null, ref iTN);

    //            Column dtColumn = new Column();
    //            foreach (Column eachColumn in lstColumns)
    //            {
    //                if (eachColumn.IsStandard == true)
    //                {
    //                    switch (eachColumn.SystemName.ToLower())
    //                    {
    //                        case "datetimerecorded":
    //                            ViewState["DateTimeSummary"] = eachColumn.GraphLabel;
    //                            break;

    //                        default:
    //                            break;
    //                    }
    //                }
    //                else
    //                {
    //                    if (eachColumn.GraphLabel != "")
    //                    {
    //                        System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString());

    //                        ddlEachAnalyteEdit.Items.Insert(ddlEachAnalyteEdit.Items.Count, aItem);
    //                    }
    //                }

    //            }


    //        }

    //    }

    //    //BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    //    MakeChart();


    //}
    protected void ddlGrpahOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender != null)
        {
            ViewState["dtToDateTime"] = null;
            ViewState["dtFromDateTime"] = null;
            //ddlTimePeriod.SelectedValue = "C";
            ddlTimePeriodDisplay_Simple.SelectedValue = "C";



        }

        try
        {
            BlankField();


            if (Mode == "add" && ddlGraphOption.SelectedValue != "")
            {
                StoreTheGraphOption();
            }

            Session["PlotGraph"] = null;
            _bToDateUp = null;
            //_bByPassToDate = true;
            if (ddlGraphOption.SelectedValue.IndexOf("-") > -1)
            {
                Mode = "edit";

                if (DocumentID == null)
                {
                    GraphOptionID = int.Parse(ddlGraphOption.SelectedValue.Replace("-", ""));
                    _gGraphOption = GraphManager.ets_GraphOption_Detail((int)GraphOptionID);
                    if (sender != null)
                    {
                        if (_gGraphOption != null)
                        {
                            if (_gGraphOption.TimePeriod.ToString() != "")
                            {

                                //ddlTimePeriod.Text = _gGraphOption.TimePeriod.ToString();
                                ddlTimePeriodDisplay_Simple.Text = _gGraphOption.TimePeriod.ToString();


                            }
                        }
                    }

                }
                PopulateRecord();
                //divGrid.Visible = true;
                divSave.Visible = true;
                OneTableID = null;


                int iINTemp = 0;
                ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
              null, null, ref iINTemp);
                ViewState["GraphOptionDetailCount"] = iINTemp;

            }
            else if (ddlGraphOption.SelectedValue != "")
            {
                OneTableID = int.Parse(ddlGraphOption.SelectedValue);

                if (OneTableID == null ||  DocumentSectionID!=null)
                {
                    Mode = "edit";
                }
                else
                {
                    Mode = "add";
                }

                PopulateOneTableRecord();
                GraphOptionID = null;
                //divGrid.Visible = false;
                //divSave.Visible = false;
                //divGrid.Visible = true;
                divSave.Visible = true;
                //PopulateRecord();

            }
            else if (ddlGraphOption.SelectedValue == "")
            {
                Mode = "add";
                //divGrid.Visible = true;
                divSave.Visible = true;
                PopulateRecord();

            }

            //ddlTimePeriod.Text = "Y";
            PopulateYAxis();
            //_bOldDate = true;
            MakeChart();
        }
        catch
        {
            //
        }

    }

    //protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    //{



    //    //PopulateAnalyte();
    //    //PopulateLocation();
    //    MakeChart();

    //}
    //protected void ddlEachAnalyte_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int iGraphOptionDetailID = 1;
    //    DropDownList ddlEachAnalyte = sender as DropDownList;
    //    GridViewRow row = ddlEachAnalyte.NamingContainer as GridViewRow;
    //    DropDownList ddlEachTable = row.FindControl("ddlEachTable") as DropDownList;
    //    Label LblID = row.FindControl("LblID") as Label;
    //    iGraphOptionDetailID = int.Parse(LblID.Text);
    //    if (gvTheGrid.Rows.Count == 1)
    //    {
    //        ddlYAxis.Text = ddlEachAnalyte.SelectedValue;
    //    }

    //    //update virtual table
    //    DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
    //    int i = 0;
    //    foreach (DataRow dr in theTable.Rows)
    //    {
    //        if (dr["GraphOptionDetailID"].ToString() == iGraphOptionDetailID.ToString())
    //        {
    //            theTable.Rows[i]["TableID"] = int.Parse(ddlEachTable.SelectedValue);
    //            DataTable dtTemp = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE   TableID=" + ddlEachTable.SelectedValue.ToString() + " AND SystemName='" + ddlEachAnalyte.SelectedValue + "'");

    //            if (dtTemp.Rows.Count > 0)
    //            {
    //                if (dtTemp.Rows[0]["ColumnID"] != null && dtTemp.Rows[0]["ColumnID"].ToString() != "")
    //                {
    //                    theTable.Rows[i]["ColumnID"] = int.Parse(dtTemp.Rows[0]["ColumnID"].ToString());
    //                }
    //            }
    //            theTable.Rows[i]["SystemName"] = ddlEachAnalyte.SelectedValue;
    //            theTable.Rows[i]["TableName"] = ddlEachTable.SelectedItem.Text;
    //            theTable.Rows[i]["GraphLabel"] = ddlEachAnalyte.SelectedItem.Text;
    //            theTable.Rows[i]["Label"] = ddlEachAnalyte.SelectedItem.Text;
    //            //theTable.Rows[i]["LocationName"] = "All";
    //            //theTable.Rows[i]["LocationID"] = DBNull.Value;
    //            theTable.Rows[i]["Low"] = DBNull.Value;
    //            theTable.Rows[i]["High"] = DBNull.Value;
    //            theTable.AcceptChanges();
    //            ViewState["GraphOptionDetail"] = theTable;

    //            if (Mode == "edit")
    //            {

    //                GraphOptionDetail theGraphOptionDetail = GraphManager.ets_GraphOptionDetail_Detail(iGraphOptionDetailID);

    //                if (theGraphOptionDetail != null)
    //                {
    //                    theGraphOptionDetail.TableID = int.Parse(ddlEachTable.SelectedValue); ;
    //                    theGraphOptionDetail.ColumnID = int.Parse(theTable.Rows[i]["ColumnID"].ToString());
    //                    //theGraphOptionDetail.LocationID = null;

    //                    theGraphOptionDetail.Axis = theTable.Rows[i]["Axis"].ToString();
    //                    theGraphOptionDetail.GraphType = theTable.Rows[i]["GraphType"].ToString();
    //                    theGraphOptionDetail.Colour = null;
    //                    theGraphOptionDetail.Low = null;
    //                    theGraphOptionDetail.High = null;
    //                    theGraphOptionDetail.Label = null;
    //                    GraphManager.ets_GraphOptionDetail_Update(theGraphOptionDetail);

    //                    int iINTemp = 0;
    //                    ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
    //            null, null, ref iINTemp);
    //                    ViewState["GraphOptionDetailCount"] = iINTemp;

    //                }


    //            }



    //        }

    //        i = i + 1;
    //    }

    //    if (gvTheGrid.Rows.Count == 1)
    //    {
    //        txtGraphTitle.Text = ddlEachTable.SelectedItem.Text + " - " + ddlEachAnalyte.SelectedItem.Text;
    //    }

    //    //BindTheGrid(0, _iMaxRows);

    //    _bOldDate = true;
    //    MakeChart();
    //}

    //protected void ddlEachAnalyte_Simple_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //ViewState["ModeDetail"] = "add";
    //    Mode = "add";

    //    txtGraphTitle_Simple.Text = ddlGraphOption.SelectedItem.Text + " - " + ddlEachAnalyte_Simple.SelectedItem.Text;

    //    int iINTemp = 0;
    //    ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select(-1, null, "GraphOrder", "ASC",
    //  null, null, ref iINTemp);
    //    ViewState["GraphOptionDetailCount"] = 0;


    //    DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
    //    DataRow theRecord = theTable.NewRow();
    //    theRecord["GraphOptionDetailID"] = theTable.Rows.Count + 1;
    //    theRecord["GraphOptionID"] = -1;
    //    theRecord["TableID"] = int.Parse(ddlGraphOption.SelectedValue);
    //    theRecord["SystemName"] = ddlEachAnalyte_Simple.SelectedValue;
    //    DataTable dtTemp = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE   TableID=" + ddlGraphOption.SelectedValue + " AND SystemName='" + ddlEachAnalyte_Simple.SelectedValue + "'");
    //    if (dtTemp.Rows.Count > 0)
    //    {
    //        if (dtTemp.Rows[0][0] != DBNull.Value)
    //        {
    //            theRecord["ColumnID"] = int.Parse(dtTemp.Rows[0][0].ToString());
    //        }
    //    }


    //    theRecord["Axis"] = "Left";
    //    theRecord["Colour"] = "";
    //    theRecord["GraphType"] = "line";
    //    theRecord["TableName"] = ddlGraphOption.SelectedItem.Text;
    //    theRecord["GraphLabel"] = ddlEachAnalyte_Simple.SelectedItem.Text;
    //    //theRecord["LocationName"] = "All";

    //    theTable.Rows.Add(theRecord);
    //    theTable.AcceptChanges();
    //    ViewState["GraphOptionDetail"] = theTable;
    //    ViewState["GraphOptionDetailCount"] = (int)ViewState["GraphOptionDetailCount"] + 1;

    //    //BindTheGrid(0, _iMaxRows);



    //    MakeChart();

    //}

    protected void ddlDateFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        _bOldDate = true;
        MakeChart();
    }


    protected void ddlTimePeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlTimePeriodDisplay.Text = ddlTimePeriod.SelectedValue;
        //ddlTimePeriodDisplay_Simple.Text = ddlTimePeriod.SelectedValue;

        if (ddlTimePeriodDisplay_Simple.SelectedValue != "C")
        {
            UpdateStartEndDate();
            SetDateTimeSelectorsValue();
        }
        SetDateTimeSelectorsState();
        SetPrevNextDateState();
        MakeChart();
    }


    //protected void ddlTimePeriodDisplay_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlTimePeriod.Text = ddlTimePeriodDisplay.SelectedValue;
    //    ddlTimePeriodDisplay_Simple.Text = ddlTimePeriodDisplay.SelectedValue;
    //    ddlTimePeriod_SelectedIndexChanged(null, null);

    //    if (DocumentID != null)
    //    {
    //        Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);

    //        if (theDocument != null)
    //        {
    //            if (theDocument.ForDashBoard != null)
    //            {
    //                if ((bool)theDocument.ForDashBoard)
    //                {
    //                    //lblReportDates.Text = "Use Recent Dates";
    //                    //trStartDate.Visible = false;
    //                    //trEndDate.Visible = false;
    //                    //trRecentDays.Visible = true;
    //                }
    //            }
    //        }
    //    }
    //}


    protected void ddlTimePeriodDisplay_Simple_SelectedIndexChanged(object sender, EventArgs e)
    {


        if (ddlTimePeriodDisplay_Simple.SelectedValue != "C")
        {
            UpdateStartEndDate();
            SetDateTimeSelectorsValue();
        }
        SetDateTimeSelectorsState();
        SetPrevNextDateState();
        MakeChart();


        //ddlTimePeriod.Text = ddlTimePeriodDisplay_Simple.SelectedValue;
        //ddlTimePeriodDisplay.Text = ddlTimePeriodDisplay_Simple.SelectedValue;
        //ddlTimePeriod_SelectedIndexChanged(null, null);

        //if (DocumentID != null)
        //{
        //    Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);

        //    if (theDocument != null)
        //    {
        //        if (theDocument.ForDashBoard != null)
        //        {
        //            if ((bool)theDocument.ForDashBoard)
        //            {
        //                //lblReportDates.Text = "Use Recent Dates";
        //                //trStartDate.Visible = false;
        //                //trEndDate.Visible = false;
        //                //trRecentDays.Visible = true;
        //            }
        //        }
        //    }
        //}
    }


    private void UpdateStartEndDate()
    {
        DateTime dt;
        switch (ddlTimePeriodDisplay_Simple.SelectedValue)
        {
            case "Y":
                if (ViewState["dtToDateTime"] != null)
                {
                    dt = (DateTime)ViewState["dtToDateTime"];
                    //dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                    //ViewState["dtToDateTime"] = dt;
                    dt = dt.AddYears(-1);
                    //ViewState["dtFromDateTime"] = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    ViewState["dtFromDateTime"] = dt;
                }
                break;
            case "M":
                if (ViewState["dtToDateTime"] != null)
                {
                    dt = (DateTime)ViewState["dtToDateTime"];
                    //dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                    //ViewState["dtToDateTime"] = dt;
                    dt = dt.AddMonths(-1);
                    //ViewState["dtFromDateTime"] = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    ViewState["dtFromDateTime"] = dt;
                }
                break;
            case "W":
                if (ViewState["dtToDateTime"] != null)
                {
                    dt = (DateTime)ViewState["dtToDateTime"];
                    //dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                    //ViewState["dtToDateTime"] = dt;
                    dt = dt.AddDays(-7);
                    //ViewState["dtFromDateTime"] = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    ViewState["dtFromDateTime"] = dt;
                }
                break;
            case "D":
                if (ViewState["dtToDateTime"] != null)
                {
                    dt = (DateTime)ViewState["dtToDateTime"];
                    //dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                    //ViewState["dtToDateTime"] = dt;
                    dt = dt.AddDays(-1);
                    //ViewState["dtFromDateTime"] = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    ViewState["dtFromDateTime"] = dt;
                }
                break;
            case "H":
                if (ViewState["dtToDateTime"] != null)
                {
                    dt = (DateTime)ViewState["dtToDateTime"];
                    //dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 59, 59);
                    //ViewState["dtToDateTime"] = dt;
                    dt = dt.AddHours(-1);
                    //ViewState["dtFromDateTime"] = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
                    ViewState["dtFromDateTime"] = dt;
                }
                break;
        }
    }


    private void SetPrevNextDateState()
    {
        if ((ddlTimePeriodDisplay_Simple.SelectedValue == "C" || ddlTimePeriodDisplay_Simple.SelectedValue == "H") && ParentPage == "main")
        {
            tdClickHelp.Visible = false;
        }
        else
        {
            if (ParentPage == "main")
            {
                tdClickHelp.Visible = true;
            }
        }

        if (ParentPage == "list")
        {
            //lnkTPPrev2.Visible = true;
            //lnkTPNext2.Visible = true;


            if ((DateTime)ViewState["dtToDateTime"] > (DateTime)ViewState["dtMaxDate"])
            {
                lnkTPNext2_Simple.Visible = false;
                lblTPNext2_Simple.Visible = true;
            }
            else
            {
                lnkTPNext2_Simple.Visible = true;
                lblTPNext2_Simple.Visible = false;
            }

            if ((DateTime)ViewState["dtToDateTime"] <= (DateTime)ViewState["dtMinDate"])
            {
                lnkTPPrev2_Simple.Visible = false;
                lblTPPrev2_Simple.Visible = true;
            }
            else
            {
                lnkTPPrev2_Simple.Visible = true;
                lblTPPrev2_Simple.Visible = false;
            }
        }
        if (ParentPage == "main")
        {
            if (ddlTimePeriodDisplay_Simple.SelectedValue == "C")
            {
                //lnkTPPrev2.Visible = false;
                //lnkTPNext2.Visible = false;
                lnkTPPrev2_Simple.Visible = false;
                lnkTPNext2_Simple.Visible = false;
                lblTPPrev2_Simple.Visible = false;
                lblTPNext2_Simple.Visible = false;
            }
            else
            {
                //lnkTPPrev2.Visible = true;
                //lnkTPNext2.Visible = true;

                if ((DateTime)ViewState["dtToDateTime"] > (DateTime)ViewState["dtMaxDate"])
                {
                    lnkTPNext2_Simple.Visible = false;
                    lblTPNext2_Simple.Visible = true;
                }
                else
                {
                    lnkTPNext2_Simple.Visible = true;
                    lblTPNext2_Simple.Visible = false;
                }

                if ((DateTime)ViewState["dtToDateTime"] <= (DateTime)ViewState["dtMinDate"])
                {
                    lnkTPPrev2_Simple.Visible = false;
                    lblTPPrev2_Simple.Visible = true;
                }
                else
                {
                    lnkTPPrev2_Simple.Visible = true;
                    lblTPPrev2_Simple.Visible = false;
                }
            }
        }
    }


    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        //BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
       // BindTheGrid(0, _iMaxRows);
        MakeChart();
    }


    //protected void Pager_DeleteAction(object sender, EventArgs e)
    //{


    //    string sCheck = "";
    //    for (int i = 0; i < gvTheGrid.Rows.Count; i++)
    //    {
    //        bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
    //        if (ischeck)
    //        {
    //            sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
    //        }
    //    }
    //    if (string.IsNullOrEmpty(sCheck))
    //    {
    //        ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
    //        MakeChart();
    //    }
    //    else
    //    {

    //        if (gvTheGrid.Rows.Count == 1)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please edit this record, there must be one record, you can not delete this record.');", true);
    //            MakeChart();
    //            return;
    //        }


    //        DeleteItem(sCheck);

    //        DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
    //        //for (int i = 0; i < theTable.Rows.Count; i++)
    //        //{
    //        //    theTable.Rows[i]["GraphOptionDetailID"] = i + 1;
    //        //}

    //        //theTable.AcceptChanges();
    //        ViewState["GraphOptionDetail"] = theTable;
    //        ViewState["GraphOptionDetailCount"] = theTable.Rows.Count;
    //        //if (GraphOptionID != null)
    //        //{
    //        //    int iINTemp = 0;
    //        //    ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
    //        //  null, null, ref iINTemp);
    //        //    ViewState["GraphOptionDetailCount"] = iINTemp;
    //        //}
    //        //BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
    //        //_gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
    //        //if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
    //        //{
    //        //    BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
    //        //}

    //       // BindTheGrid(0, _iMaxRows);
    //        MakeChart();
    //    }

    //}



    //private void DeleteItem(string keys)
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(keys))
    //        {

    //            foreach (string sTemp in keys.Split(','))
    //            {
    //                if (!string.IsNullOrEmpty(sTemp))
    //                {

    //                    if (Mode == "edit")
    //                    {
    //                        GraphManager.ets_GraphOptionDetail_Delete(Convert.ToInt32(sTemp));
    //                    }

    //                    DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
    //                    //theTable.Rows.RemoveAt(Convert.ToInt32(sTemp) - 1);


    //                    var rows = theTable.Select("GraphOptionDetailID=" + sTemp);
    //                    foreach (var row in rows)
    //                        row.Delete();

    //                    theTable.AcceptChanges();

    //                    if (Mode.ToLower() == "add")
    //                    {
    //                        for (int i = 0; i < theTable.Rows.Count; i++)
    //                        {
    //                            theTable.Rows[i]["GraphOptionDetailID"] = i + 1;
    //                        }

    //                        theTable.AcceptChanges();
    //                    }

    //                    ViewState["GraphOptionDetail"] = theTable;


    //                }
    //            }


    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLog theErrorLog = new ErrorLog(null, "Graph Option delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //        //lblMsg.Text = ex.Message;

    //        //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
    //    }
    //}



    protected void lnkNewDeatilData_Click(object sender, EventArgs e)
    {
        //ViewState["ModeDetail"] = "add";
        //divDetail.Visible = true;
        AddNewRowToTable();
        //RefereshDetail();
        MakeChart();
        //lblDetailTitle.Text = "Add Series";

    }
    protected void AddNewRowToTable()
    {
        //ViewState["ModeDetail"] = "add";

        if (Mode == "edit")
        {
            DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
            int iPreIndex = theTable.Rows.Count - 1;
            GraphOptionDetail newGraphOptionDetail = new GraphOptionDetail(null, (int)_gGraphOption.GraphOptionID,
           int.Parse(theTable.Rows[iPreIndex]["TableID"].ToString()), int.Parse(theTable.Rows[iPreIndex]["ColumnID"].ToString()));
            int iR = theTable.Rows.Count % 2;
            string strAxis = "Left";
            if (iR != 0)
            {
                strAxis = "Right";
            }
            else
            {
                strAxis = "Left";
            }

            newGraphOptionDetail.Axis = strAxis;
            newGraphOptionDetail.Colour = "";
            newGraphOptionDetail.GraphType = "line";


            GraphManager.ets_GraphOptionDetail_Insert(newGraphOptionDetail);

            int iINTemp = 0;
            ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)_gGraphOption.GraphOptionID, null, "GraphOrder", "ASC",
         null, null, ref iINTemp);
            ViewState["GraphOptionDetailCount"] = iINTemp;


        }
        else
        {

            DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];

            if (theTable.Rows.Count > 0)
            {
                DataRow theRecord = theTable.NewRow();
                int iPreIndex = theTable.Rows.Count - 1;
                theRecord["GraphOptionDetailID"] = theTable.Rows.Count + 1;
                theRecord["GraphOptionID"] = theTable.Rows[iPreIndex]["GraphOptionID"];
                theRecord["TableID"] = theTable.Rows[iPreIndex]["TableID"];//int.Parse(ddlTable.SelectedValue);
                theRecord["ColumnID"] = theTable.Rows[iPreIndex]["ColumnID"];//int.Parse(ddlAnalyte.SelectedValue);

                int iR = theTable.Rows.Count % 2;
                if (iR != 0)
                {
                    theRecord["Axis"] = "Right";
                }
                else
                {
                    theRecord["Axis"] = "Left";
                }
                //Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlAnalyte.SelectedValue));

                //if (theColumn != null)
                //{

                theRecord["SystemName"] = theTable.Rows[iPreIndex]["SystemName"];// theColumn.SystemName;
                //}


                theRecord["Colour"] = "";
                theRecord["GraphType"] = "line";
                theRecord["TableName"] = theTable.Rows[iPreIndex]["TableName"];//  ddlTable.SelectedItem.Text;
                theRecord["GraphLabel"] = theTable.Rows[iPreIndex]["GraphLabel"];//  ddlAnalyte.SelectedItem.Text;

                //if (txtHighestValue.Text.Trim() != "")
                //    theRecord["High"] = double.Parse(txtHighestValue.Text);

                //if (txtLowestValue.Text.Trim() != "")
                //    theRecord["Low"] = double.Parse(txtLowestValue.Text);

                //if (ddlLocation.SelectedValue != "")
                //{
                //    theRecord["LocationID"] = int.Parse(ddlLocation.SelectedValue);
                //    theRecord["LocationName"] = ddlLocation.SelectedItem.Text;
                //}
                //else
                //{
                //theRecord["LocationName"] = "All";
                //}

                theTable.Rows.Add(theRecord);
                theTable.AcceptChanges();
                ViewState["GraphOptionDetail"] = theTable;
                ViewState["GraphOptionDetailCount"] = (int)ViewState["GraphOptionDetailCount"] + 1;
            }
            else
            {
                //first row;

                DataRow theRecord = theTable.NewRow();
                theRecord["GraphOptionDetailID"] = theTable.Rows.Count + 1;
                theRecord["GraphOptionID"] = -1;
                int iTN = 0;
                List<Table> lstST = RecordManager.ets_Table_Select(null,
                null,
                null,
                (int)AccountID,
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, STs);


                theRecord["TableID"] = lstST[0].TableID;//int.Parse(ddlTable.SelectedValue);
                theRecord["TableName"] = lstST[0].TableName;
                List<Column> lstColumns = RecordManager.ets_Table_Columns((int)lstST[0].TableID,
                           null, null, ref iTN);



                Column dtColumn = new Column();
                foreach (Column eachColumn in lstColumns)
                {
                    if (eachColumn.IsStandard == true)
                    {
                        switch (eachColumn.SystemName.ToLower())
                        {
                            case "datetimerecorded":
                                ViewState["DateTimeSummary"] = eachColumn.GraphLabel;
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (eachColumn.GraphLabel != "" && eachColumn.ColumnType == "number")
                        {
                            theRecord["ColumnID"] = eachColumn.ColumnID;
                            theRecord["SystemName"] = eachColumn.SystemName;
                            theRecord["GraphLabel"] = eachColumn.GraphLabel;
                            break;
                        }
                    }

                }

                theRecord["Axis"] = "Left";

                theRecord["Colour"] = "";
                theRecord["GraphType"] = "line";
                //theRecord["LocationName"] = "All";               
                theTable.Rows.Add(theRecord);
                theTable.AcceptChanges();
                ViewState["GraphOptionDetail"] = theTable;
                ViewState["GraphOptionDetailCount"] = 1;

            }
        }

        //BindTheGrid(0, _iMaxRows);



    }

    protected void lnkRefresh_Click(object sender, EventArgs e)
    {
        _bOldDate = true;
        MakeChart();
    }


    protected void btnModalOK_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
        _bOverWriteGraph = true;

        lnkSave_Click(null, null);

    }

    protected void btnModalNo_Click(object sender, EventArgs e)
    {
        _bOverWriteGraph = false;
        ModalPopupExtender1.Hide();
        lnkSave_Click(null, null);
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        //Insert or edit GrpahOption
        //SynchronizeViews();
        try
        {
            _bOldDate = true;

            int ID = 0;
            switch (Mode.ToLower())
            {
                case "add":
                    if (DocumentID == null)
                    {
                        int iNewGraphOptionID = -1;
                        bool bDuplicateTitle = false;

                        if (OneTableID != null || ParentPage == "list")
                        {
                            DataTable dtTemp = Common.DataTableFromText("SELECT GraphOptionID FROM GraphOption WHERE ReportChart=0 AND AccountID=" + AccountID.ToString() + " AND Heading='" + txtGraphTitle_Simple.Text.Replace("'", "''") + "'");
                            if (dtTemp.Rows.Count > 0)
                            {
                                if (_bOverWriteGraph == null)
                                {
                                    MakeChart();
                                    ModalPopupExtender1.Show();
                                    return;
                                }
                                else
                                {
                                    if ((bool)_bOverWriteGraph)
                                    {
                                        iNewGraphOptionID = int.Parse(dtTemp.Rows[0][0].ToString());
                                        bDuplicateTitle = true;
                                    }
                                    else
                                    {
                                        MakeChart();
                                        return;
                                    }
                                }
                            }
                        }

                        DataTable dtGraphOptionDetail = (DataTable)ViewState["GraphOptionDetail"];

                        if (dtGraphOptionDetail.Rows.Count < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "alert('Please add at least one series.');", true);
                            return;
                        }

                        GraphOption newGraphOption = new GraphOption(null, (int)AccountID,
                            2, txtGraphTitle_Simple.Text, ddlTimePeriodDisplay_Simple.SelectedValue);

                        //newGraphOption.Display3D = chk3DEnabled.Checked;
                        //newGraphOption.UserReportDate = chkUseReportDates.Checked;
                        //if (chkUseReportDates.Checked == false)
                        //{
                            if (txtStartDate_Simple.Text.Trim() == "")
                            {
                                newGraphOption.FromDate = (DateTime)ViewState["dtFromDateTime"];
                                newGraphOption.ToDate = (DateTime)ViewState["dtToDateTime"];
                            }
                            else
                            {
                                newGraphOption.FromDate = DateTime.ParseExact(txtFromTime_Simple.Text == "" ? txtStartDate_Simple.Text + " 00:00" : txtStartDate_Simple.Text + " " + txtFromTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                                newGraphOption.ToDate = DateTime.ParseExact(txtToTime_Simple.Text == "" ? txtEndDate_Simple.Text + " 00:00" : txtEndDate_Simple.Text + " " + txtToTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                            }
                        //}

                        if (ddlTimePeriodDisplay_Simple.SelectedValue == "C")
                        {

                            newGraphOption.FromDate = DateTime.ParseExact(txtFromTime_Simple.Text == "" ? txtStartDate_Simple.Text + " 00:00" : txtStartDate_Simple.Text + " " + txtFromTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                            newGraphOption.ToDate = DateTime.ParseExact(txtToTime_Simple.Text == "" ? txtEndDate_Simple.Text + " 00:00" : txtEndDate_Simple.Text + " " + txtToTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);

                            newGraphOption.TimePeriod = "C";
                            newGraphOption.CustomTimePeriod = ddlTimePeriodDisplay_Simple.SelectedValue;

                        }
                        if (ddlTimePeriodDisplay_Simple.SelectedValue != "C" && DocumentID == null)
                        {
                            newGraphOption.FromDate = null;
                            newGraphOption.ToDate = null;
                        }

                        //newGraphOption.DateFormat = ddlDateFormat.SelectedValue;

                        //if (txtPlotAreaWidth.Text != "")
                        //    newGraphOption.Width = double.Parse(txtPlotAreaWidth.Text);
                        //if (txtPlotAreaHeight.Text != "")
                        //    newGraphOption.Height = double.Parse(txtPlotAreaHeight.Text);

                        newGraphOption.IsActive = true;

                        //newGraphOption.Legend = rblLegendPosition.SelectedValue;
                        newGraphOption.ReportChart = false; //??

                        if (DocumentID != null)
                        {
                            if (IsDashBorad == false)
                                newGraphOption.ReportChart = true; //??
                        }

                        newGraphOption.ShowLimits = chkShowLimits_Simple.Checked;
                        //newGraphOption.ShowMissing = chkShowDottedLine.Checked;

                        newGraphOption.WarningCaption = txtWarningCaption_Simple.Text;
                        if (txtWarningValue_Simple.Text != "")
                        {
                            newGraphOption.WarningValue = double.Parse(txtWarningValue_Simple.Text);
                        }
                        newGraphOption.WarningColor = ddlWarningColor_Simple.Value;
                        newGraphOption.ExceedanceCaption = txtExceedanceCaption_Simple.Text;
                        if (txtExceedanceValue_Simple.Text != "")
                        {
                            newGraphOption.ExceedanceValue = double.Parse(txtExceedanceValue_Simple.Text);
                        }
                        newGraphOption.ExceedanceColor = ddlExceedanceColor_Simple.Value;

                        if (!String.IsNullOrEmpty(txtGraphSubtitle_Simple.Text))
                            newGraphOption.SubHeading = txtGraphSubtitle_Simple.Text;
                        int graphDefinitionID = int.Parse(ddlGraphType_Simple.SelectedValue);
                        if (graphDefinitionID < 0)
                            graphDefinitionID = -1 * graphDefinitionID;
                        newGraphOption.GraphDefinitionID = graphDefinitionID;
                        if (!String.IsNullOrEmpty(txtYHighestValue.Text))
                            newGraphOption.YAxisHighestValue = Convert.ToDouble(txtYHighestValue.Text);
                        if (!String.IsNullOrEmpty(txtYLowestValue.Text))
                            newGraphOption.YAxisLowestValue = Convert.ToDouble(txtYLowestValue.Text);
                        if (!String.IsNullOrEmpty(txtYInterval.Text))
                            newGraphOption.YAxisInterval = Convert.ToDouble(txtYInterval.Text);

                        if (bDuplicateTitle == false)
                        {
                            iNewGraphOptionID = GraphManager.ets_GraphOption_Insert(newGraphOption);
                        }
                        else
                        {
                            newGraphOption.GraphOptionID = iNewGraphOptionID;
                            GraphManager.ets_GraphOption_Update(newGraphOption);
                            Common.ExecuteText("DELETE GraphOptionDetail WHERE GraphOptionID=" + iNewGraphOptionID.ToString());
                        }

                        //add GraphOptionDetail

                        foreach (DataRow theRecord in dtGraphOptionDetail.Rows)
                        {
                            GraphOptionDetail newGraphOptionDetail = new GraphOptionDetail(null, iNewGraphOptionID,
                                (int)theRecord["TableID"], (int)theRecord["ColumnID"]);

                            if (theRecord["Axis"] != DBNull.Value)
                                newGraphOptionDetail.Axis = (string)theRecord["Axis"];

                            if (theRecord["Colour"] != DBNull.Value)
                                newGraphOptionDetail.Colour = (string)theRecord["Colour"];

                            if (theRecord["GraphType"] != DBNull.Value)
                                newGraphOptionDetail.GraphType = (string)theRecord["GraphType"];

                            if (theRecord["High"] != DBNull.Value)
                                newGraphOptionDetail.High = double.Parse(theRecord["High"].ToString());

                            if (theRecord["Low"] != DBNull.Value)
                                newGraphOptionDetail.Low = double.Parse(theRecord["Low"].ToString());

                            if (theRecord["Label"] != DBNull.Value)
                                newGraphOptionDetail.Label = theRecord["Label"].ToString();

                            if (theRecord["GraphSeriesColumnID"] != DBNull.Value)
                                newGraphOptionDetail.GraphSeriesColumnID = theRecord["GraphSeriesColumnID"].ToString();

                            if (theRecord["GraphSeriesID"] != DBNull.Value)
                                newGraphOptionDetail.GraphSeriesID = theRecord["GraphSeriesID"].ToString();

                            newGraphOption.GraphOptionID = GraphManager.ets_GraphOptionDetail_Insert(newGraphOptionDetail);

                        }
                    }
                    else
                    {
                        int iPosition = 1;
                        int iNewGraphOptionID = -1 * int.Parse(ddlGraphOption.SelectedValue);
                        using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
                        {

                            if (PreDocumentSectionID != 0)
                            {

                                DocGen.DAL.DocumentSection PreSection = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == PreDocumentSectionID);

                                iPosition = PreSection.Position + 1;
                            }
                            else
                            {
                                iPosition = 1;
                            }

                            DocGen.DAL.DocumentSection newSection = new DocGen.DAL.DocumentSection();

                            ctx.ExecuteCommand("UPDATE DocumentSection SET Position=Position + 1 WHERE DocumentID={0}  AND Position>{1}", DocumentID.ToString(), (iPosition - 1).ToString());

                            newSection.DocumentID = (int)DocumentID;
                            newSection.SectionName = txtGraphTitle_Simple.Text;

                            Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);

                            newSection.DocumentSectionTypeID = 5; //Chart  

                            if (theDocument.ForDashBoard != null)
                            {
                                if ((bool)theDocument.ForDashBoard)
                                {
                                    //newGraphOption.ReportChart = false; //??
                                    //GraphManager.ets_GraphOption_Update(newGraphOption, null);
                                    newSection.DocumentSectionTypeID = 9; //DashChart  
                                    newSection.ValueFields = RecentDays;
                                }

                            }

                            //newSection.Content = txtContent.Text;
                            newSection.Position = iPosition;
                            newSection.DateAdded = DateTime.Now;
                            newSection.DateUpdated = DateTime.Now;

                            newSection.Details = iNewGraphOptionID.ToString();

                            ctx.DocumentSections.InsertOnSubmit(newSection);

                            ctx.SubmitChanges();

                            ID = newSection.DocumentSectionID;
                        }
                    }
                    break;
                case "edit":
                    if (DocumentSectionID != null)
                    {
                        Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);
                        if (theDocument.ForDashBoard != null)
                        {
                            if ((bool)theDocument.ForDashBoard)
                            {
                                using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
                                {
                                    DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);

                                    section.ValueFields =RecentDays;
                                    ctx.SubmitChanges();

                                }
                            }
                        }

                    }


                    if (ParentPage == "list")
                    {
                        DataTable dtTemp = Common.DataTableFromText("SELECT GraphOptionID FROM GraphOption WHERE ReportChart=0 AND GraphOptionID<>" + GraphOptionID.ToString() + "  AND AccountID=" + AccountID.ToString() + " AND Heading='" + txtGraphTitle_Simple.Text.Replace("'", "''") + "'");
                        if (dtTemp.Rows.Count > 0)
                        {

                            if (_bOverWriteGraph == null)
                            {
                                MakeChart();
                                ModalPopupExtender1.Show();
                                return;
                            }
                            else
                            {
                                if ((bool)_bOverWriteGraph)
                                {
                                    Common.ExecuteText("UPDATE Account SET DefaultGraphOptionID=" + GraphOptionID.ToString() + " WHERE DefaultGraphOptionID=" + dtTemp.Rows[0][0].ToString());

                                    Common.ExecuteText("DELETE GraphOptionDetail WHERE GraphOptionID=" + dtTemp.Rows[0][0].ToString());
                                    GraphManager.ets_GraphOption_Delete(int.Parse(dtTemp.Rows[0][0].ToString()));
                                }
                                else
                                {
                                    MakeChart();
                                    return;
                                }
                            }

                        }

                    }

                    GraphOption editGraphOption;

                    if (DocumentID != null)
                    {
                        editGraphOption = GraphManager.ets_GraphOption_Detail((int)GraphOptionID);
                    }
                    else
                    {
                        editGraphOption = GraphManager.ets_GraphOption_Detail(int.Parse(ddlGraphOption.SelectedValue.Replace("-", "")));
                    }
                    //editGraphOption.Display3D = chk3DEnabled.Checked;
                    //editGraphOption.UserReportDate = chkUseReportDates.Checked;
                    //if (chkUseReportDates.Checked == false)
                    //{
                        if (txtStartDate_Simple.Text.Trim() == "")
                        {
                            editGraphOption.FromDate = (DateTime)ViewState["dtFromDateTime"];
                            editGraphOption.ToDate = (DateTime)ViewState["dtToDateTime"];
                        }
                        else
                        {

                            editGraphOption.FromDate = DateTime.ParseExact(txtFromTime_Simple.Text == "" ? txtStartDate_Simple.Text + " 00:00" : txtStartDate_Simple.Text + " " + txtFromTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                            editGraphOption.ToDate = DateTime.ParseExact(txtToTime_Simple.Text == "" ? txtEndDate_Simple.Text + " 00:00" : txtEndDate_Simple.Text + " " + txtToTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                        }
                    //}

                    if (ddlTimePeriodDisplay_Simple.SelectedValue == "C" && _theChartDashBoard==null)
                    {

                        editGraphOption.FromDate = DateTime.ParseExact(txtFromTime_Simple.Text == "" ? txtStartDate_Simple.Text + " 00:00" : txtStartDate_Simple.Text + " " + txtFromTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                        editGraphOption.ToDate = DateTime.ParseExact(txtToTime_Simple.Text == "" ? txtEndDate_Simple.Text + " 00:00" : txtEndDate_Simple.Text + " " + txtToTime_Simple.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);

                        editGraphOption.TimePeriod = "C";
                        editGraphOption.CustomTimePeriod = ddlTimePeriodDisplay_Simple.SelectedValue;

                    }
                    else
                    {
                        editGraphOption.TimePeriod = ddlTimePeriodDisplay_Simple.SelectedValue;
                    }


                    if (ddlTimePeriodDisplay_Simple.SelectedValue != "C" && DocumentID == null)
                    {
                        editGraphOption.FromDate = null;
                        editGraphOption.ToDate = null;
                    }

                    //editGraphOption.HideDate = chkHideDate.Checked;

                    //editGraphOption.DateFormat = ddlDateFormat.SelectedValue;

                    //if (txtPlotAreaWidth.Text != "")
                    //    editGraphOption.Width = double.Parse(txtPlotAreaWidth.Text);
                    //if (txtPlotAreaHeight.Text != "")
                    //    editGraphOption.Height = double.Parse(txtPlotAreaHeight.Text);

                    editGraphOption.IsActive = true;

                    //editGraphOption.Legend = rblLegendPosition.SelectedValue;



                    editGraphOption.ShowLimits = chkShowLimits_Simple.Checked;
                    //editGraphOption.ShowMissing = chkShowDottedLine.Checked;



                    editGraphOption.WarningCaption = txtWarningCaption_Simple.Text;
                    if (txtWarningValue_Simple.Text != "")
                    {
                        editGraphOption.WarningValue = double.Parse(txtWarningValue_Simple.Text);
                    }
                    else
                    {
                        editGraphOption.WarningValue = null;
                    }
                    editGraphOption.WarningColor = ddlWarningColor_Simple.Value;
                    editGraphOption.ExceedanceCaption = txtExceedanceCaption_Simple.Text;
                    if (txtExceedanceValue_Simple.Text != "")
                    {
                        editGraphOption.ExceedanceValue = double.Parse(txtExceedanceValue_Simple.Text);
                    }
                    else
                    {
                        editGraphOption.ExceedanceValue = null;
                    }
                    editGraphOption.ExceedanceColor = ddlExceedanceColor_Simple.Value;



                    editGraphOption.Heading = txtGraphTitle_Simple.Text;

                    int graphDefinitionIDU = int.Parse(ddlGraphType_Simple.SelectedValue);
                    if (graphDefinitionIDU < 0)
                        graphDefinitionIDU = -1 * graphDefinitionIDU;
                    editGraphOption.GraphDefinitionID = graphDefinitionIDU;
                        if (!String.IsNullOrEmpty(txtYHighestValue.Text))
                            editGraphOption.YAxisHighestValue = Convert.ToDouble(txtYHighestValue.Text);
                        if (!String.IsNullOrEmpty(txtYLowestValue.Text))
                            editGraphOption.YAxisLowestValue = Convert.ToDouble(txtYLowestValue.Text);
                        if (!String.IsNullOrEmpty(txtYInterval.Text))
                            editGraphOption.YAxisInterval = Convert.ToDouble(txtYInterval.Text);


                    GraphManager.ets_GraphOption_Update(editGraphOption);
                    Common.ExecuteText("DELETE GraphOptionDetail WHERE GraphOptionID=" + editGraphOption.GraphOptionID.ToString());
                    if (ViewState["GraphOptionDetail"]!=null)
                    {
                        DataTable dtGraphOptionDetail = (DataTable)ViewState["GraphOptionDetail"];
                        //add GraphOptionDetail

                        foreach (DataRow theRecord in dtGraphOptionDetail.Rows)
                        {
                            GraphOptionDetail newGraphOptionDetail = new GraphOptionDetail(null, (int)editGraphOption.GraphOptionID,
                                (int)theRecord["TableID"], (int)theRecord["ColumnID"]);

                            if (theRecord["Axis"] != DBNull.Value)
                                newGraphOptionDetail.Axis = (string)theRecord["Axis"];

                            if (theRecord["Colour"] != DBNull.Value)
                                newGraphOptionDetail.Colour = (string)theRecord["Colour"];

                            if (theRecord["GraphType"] != DBNull.Value)
                                newGraphOptionDetail.GraphType = (string)theRecord["GraphType"];

                            if (theRecord["High"] != DBNull.Value)
                                newGraphOptionDetail.High = double.Parse(theRecord["High"].ToString());

                            if (theRecord["Low"] != DBNull.Value)
                                newGraphOptionDetail.Low = double.Parse(theRecord["Low"].ToString());

                            if (theRecord["Label"] != DBNull.Value)
                                newGraphOptionDetail.Label = theRecord["Label"].ToString();

                            if (theRecord["GraphSeriesColumnID"] != DBNull.Value)
                                newGraphOptionDetail.GraphSeriesColumnID = theRecord["GraphSeriesColumnID"].ToString();

                            if (theRecord["GraphSeriesID"] != DBNull.Value)
                                newGraphOptionDetail.GraphSeriesID = theRecord["GraphSeriesID"].ToString();

                            GraphManager.ets_GraphOptionDetail_Insert(newGraphOptionDetail);

                        }
                    }

                    if (DocumentSectionID != null)
                        ID = (int)DocumentSectionID;


                    break;

            }

            if (ParentPage == "section")
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + ID.ToString() + ");", true);

            }
            else if (ParentPage == "main")
            {
                Session["tdbmsgpb"] = "The graph “" + txtGraphTitle_Simple.Text + "” has been saved.";
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "alert('The graph “" + txtGraphTitle_Simple.Text + "” has been saved.');", true);
                MakeChart();
            }
            else
            {
                Response.Redirect(hlBack.NavigateUrl, false);

            }

        }
        catch (Exception ex)
        {
            MakeNoDataChart();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Problem", "alert('There is no graph to save.');", true);
        }


    }

    //protected void lnkDetailCancel_Click(object sender, EventArgs e)
    //{
    //    divDetail.Visible = false;
    //    MakeChart();

    //}


    //protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    int iTN = 0;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DropDownList ddlEachTable = e.Row.FindControl("ddlEachTable") as DropDownList;

    //        if (ddlEachTable != null)
    //        {
    //            ddlEachTable.Items.Clear();

    //            iTN = 0;
    //            ddlEachTable.DataSource = RecordManager.ets_Table_Select(null,
    //            null,
    //            null,
    //            (int)AccountID,
    //            null, null, true,
    //            "st.TableName", "ASC",
    //            null, null, ref  iTN, STs);
    //            ddlEachTable.DataBind();

    //            //System.Web.UI.WebControls.ListItem liPlease = new System.Web.UI.WebControls.ListItem("--Please Select--", "-1");
    //            //ddlEachTable.Items.Insert(0, liPlease);

    //            ddlEachTable.Text = DataBinder.Eval(e.Row.DataItem, "TableID").ToString();

    //            HyperLink hlEditDetail = e.Row.FindControl("hlEditDetail") as HyperLink;
    //            if (hlEditDetail != null)
    //            {
    //                string strLabel = "-1";
    //                if (DataBinder.Eval(e.Row.DataItem, "Label") != DBNull.Value)
    //                    strLabel = DataBinder.Eval(e.Row.DataItem, "Label").ToString();

    //                hlEditDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath +
    //                    "/Pages/UserControl/GraphOptionDetail.aspx?ModeDetail=edit&TableID=" +
    //                    ddlEachTable.Text + "&GraphOptionDetailID=" + DataBinder.Eval(e.Row.DataItem, "GraphOptionDetailID").ToString() +
    //                    "&ColumnID=" + DataBinder.Eval(e.Row.DataItem, "ColumnID").ToString() +
    //                    //"&LocationID=" + DataBinder.Eval(e.Row.DataItem, "LocationID").ToString() +
    //                     "&GraphType=" + DataBinder.Eval(e.Row.DataItem, "GraphType").ToString() +
    //                      "&Axis=" + DataBinder.Eval(e.Row.DataItem, "Axis").ToString() +
    //                       "&Colour=" + DataBinder.Eval(e.Row.DataItem, "Colour").ToString().Replace("#", "") +
    //                        "&High=" + DataBinder.Eval(e.Row.DataItem, "High").ToString() +
    //                           "&Low=" + DataBinder.Eval(e.Row.DataItem, "Low").ToString() +
    //                           "&Label=" + strLabel +
    //                             "&GraphLabel=" + DataBinder.Eval(e.Row.DataItem, "GraphLabel").ToString();
    //            }

    //            DropDownList ddlEachAnalyte = e.Row.FindControl("ddlEachAnalyte") as DropDownList;
    //            if (ddlEachAnalyte != null)
    //            {
    //                ddlEachAnalyte.Items.Clear();
    //                string strTableID = ddlEachTable.SelectedValue;

    //                List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(strTableID),
    //                       null, null, ref iTN);

    //                Column dtColumn = new Column();
    //                foreach (Column eachColumn in lstColumns)
    //                {
    //                    if (eachColumn.IsStandard == true)
    //                    {
    //                        switch (eachColumn.SystemName.ToLower())
    //                        {
    //                            case "datetimerecorded":
    //                                ViewState["DateTimeSummary"] = eachColumn.GraphLabel;
    //                                break;

    //                            default:
    //                                break;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (eachColumn.GraphLabel != "" && eachColumn.ColumnType == "number")
    //                        {
    //                            System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.SystemName);

    //                            ddlEachAnalyte.Items.Insert(ddlEachAnalyte.Items.Count, aItem);
    //                        }
    //                    }

    //                }


    //                ddlEachAnalyte.Text = DataBinder.Eval(e.Row.DataItem, "SystemName").ToString();
    //            }
    //        }
    //    }

    //}

    //protected void gvTheGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "deletetype")
    //    {
    //        try
    //        {
    //            if (Mode == "edit")
    //            {
    //                GraphManager.ets_GraphOptionDetail_Delete(Convert.ToInt32(e.CommandArgument));
    //                int iINTemp = 0;
    //                ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
    //              null, null, ref iINTemp);
    //                ViewState["GraphOptionDetailCount"] = iINTemp;
    //            }
    //            else
    //            {
    //                DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
    //                theTable.Rows.RemoveAt(Convert.ToInt32(e.CommandArgument) - 1);

    //                for (int i = 0; i < theTable.Rows.Count; i++)
    //                {
    //                    theTable.Rows[i]["GraphOptionDetailID"] = i + 1;
    //                }

    //                theTable.AcceptChanges();
    //                ViewState["GraphOptionDetail"] = theTable;
    //            }
    //            //BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    //            //BindTheGrid(0, _iMaxRows);
    //            MakeChart();

    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog theErrorLog = new ErrorLog(null, "Graph TheGrid", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //            SystemData.ErrorLog_Insert(theErrorLog);
    //        }
    //    }
    //}


    protected void btnAddNewDetail_Click(object sender, EventArgs e)
    {
        lnkNewDeatilData_Click(null, null);
    }

    protected void lnkZoom_Click(object sender, EventArgs e)
    {
        lnkCloseZoom.Visible = true;
        lnkZoom.Visible = false;
        //txtPlotAreaWidth.Text = "930";
        //txtPlotAreaHeight.Text = "650";
        this.ChartWidth = 930;
        this.ChartHeight = 650;
        divOptionControls.Visible = false;

        _bOldDate = true;
        MakeChart();

        if (ZoomGraph != null)
            ZoomGraph(this, EventArgs.Empty);

    }
    protected void lnkCloseZoom_Click(object sender, EventArgs e)
    {
        lnkCloseZoom.Visible = false;
        lnkZoom.Visible = true;
        //txtPlotAreaWidth.Text = ChartWidth.ToString();
        //txtPlotAreaHeight.Text = ChartHeight.ToString();
        if (ParentPage == "main" || ParentPage == "list")
        {
            divOptionControls.Visible = true;
        }

        _bOldDate = true;
        MakeChart();

        if (CloseZoomGraph != null)
            CloseZoomGraph(this, EventArgs.Empty);

    }


    protected void lnkTPNext_Click(object sender, EventArgs e)
    {
        //_bByPassToDate = true;
        _bToDateUp = true;
       // DateUpDown(true);
        MakeChart();
    }


    protected void lnkTPNext2_Click(object sender, EventArgs e)
    {
        _bToDateUp = true;
       // DateUpDown(true);
        MakeChart();
    }


    protected void lnkTPPrev_Click(object sender, EventArgs e)
    {
        _bToDateUp = false;
        //DateUpDown(false);
        MakeChart();
    }


    protected void lnkTPPrev2_Click(object sender, EventArgs e)
    {
        _bToDateUp = false;
        //DateUpDown(false);
        MakeChart();
    }

    protected void DateUpDown(bool isUp)
    {
        if (ViewState["dtFromDateTime"] != null && ViewState["dtToDateTime"] != null)
        {
            DateTime dtFromDateTime = (DateTime)ViewState["dtFromDateTime"];
            DateTime dtToDateTime = (DateTime)ViewState["dtToDateTime"];
            if (isUp)
            {
                switch (ddlTimePeriodDisplay_Simple.Text)
                {
                    case "H":
                        dtFromDateTime = dtFromDateTime.AddHours(1);
                        dtToDateTime = dtToDateTime.AddHours(1);
                        break;
                    case "D":
                        dtFromDateTime = dtFromDateTime.AddDays(1);
                        dtToDateTime = dtToDateTime.AddDays(1);
                        break;
                    case "W":
                        dtFromDateTime = dtFromDateTime.AddDays(7);
                        dtToDateTime = dtToDateTime.AddDays(7);
                        break;
                    case "M":
                        dtFromDateTime = dtFromDateTime.AddMonths(1);
                        dtToDateTime = dtToDateTime.AddMonths(1);
                        break;
                    case "Y":
                        dtFromDateTime = dtFromDateTime.AddYears(1);
                        dtToDateTime = dtToDateTime.AddYears(1);
                        break;
                }
            }
            else
            {
                switch (ddlTimePeriodDisplay_Simple.Text)
                {
                    case "H":
                        dtFromDateTime = dtFromDateTime.AddHours(-1);
                        dtToDateTime = dtToDateTime.AddHours(-1);
                        break;
                    case "D":
                        dtFromDateTime = dtFromDateTime.AddDays(-1);
                        dtToDateTime = dtToDateTime.AddDays(-1);
                        break;
                    case "W":
                        dtFromDateTime = dtFromDateTime.AddDays(-7);
                        dtToDateTime = dtToDateTime.AddDays(-7);
                        break;
                    case "M":
                        dtFromDateTime = dtFromDateTime.AddMonths(-1);
                        dtToDateTime = dtToDateTime.AddMonths(-1);
                        break;
                    case "Y":
                        dtFromDateTime = dtFromDateTime.AddYears(-1);
                        dtToDateTime = dtToDateTime.AddYears(-1);
                        break;
                }
            }

            ViewState["dtToDateTime"] = dtToDateTime;
            ViewState["dtFromDateTime"] = dtFromDateTime;

            SetDateTimeSelectorsValue();
            SetPrevNextDateState();
        }
    }


    protected void MakeDate()
    {

        if (ViewState["dtFromDateTime"] != null && ViewState["dtToDateTime"] != null && _bOldDate == true)
        {
            return;
        }

        if ((!IsPostBack || _bFromSelectedGraph==true) && _gGraphOption != null && _gGraphOption.TimePeriod != "" && _gGraphOption.TimePeriod.ToLower() == "c" && _gGraphOption.FromDate != null && _gGraphOption.ToDate != null)
        {
            txtStartDate_Simple.Text = ConvertUtil.GetDateString((DateTime)_gGraphOption.FromDate);
            txtEndDate_Simple.Text = ConvertUtil.GetDateString((DateTime)_gGraphOption.ToDate);

            txtFromTime_Simple.Text = ((DateTime)_gGraphOption.FromDate).Hour.ToString() + ":" + ((DateTime)_gGraphOption.FromDate).Minute.ToString();
            txtToTime_Simple.Text = ((DateTime)_gGraphOption.ToDate).Hour.ToString() + ":" + ((DateTime)_gGraphOption.ToDate).Minute.ToString();


            ViewState["dtToDateTime"] = _gGraphOption.ToDate;
            ViewState["dtFromDateTime"] = _gGraphOption.FromDate;
            

            if (ddlTimePeriodDisplay_Simple.Text == "C")
            {
                ViewState["C_dtToDateTime"] = ViewState["dtToDateTime"];
                ViewState["C_dtFromDateTime"] = ViewState["dtFromDateTime"];

            }
            return;
        }

        string dateFormat = "dmy";
        //string graphXAxisColumnName = GetGraphXAxisColumnName(ddlGraphOption.SelectedValue == "" ? "-1" : ddlGraphOption.SelectedValue);

        string graphXAxisColumnName = GetGraphXAxisColumnName(ddlGraphOption.SelectedValue == "" ? "-1" : ddlGraphOption.SelectedValue);

        DateTime? dtFromDateTime = DateTime.Now.AddYears(-1);
        DateTime? dtToDateTime = DateTime.Now;

        if (((ViewState["dtToDateTime"] == null) || (ViewState["dtFromDateTime"] == null)) && !_bToDateUp.HasValue)
        {
            string strTableID = ddlGraphOption.SelectedValue;

            if (ddlGraphOption.SelectedValue.IndexOf("-") > -1)
            {
                lnkRefresh.Visible = true;
                DataTable dtGO = Common.DataTableFromText(
                    String.Format(@"SET dateformat {0};SELECT MAX(CAST(CASE WHEN ISDATE(Record.[{1}]) > 0 THEN Record.[{1}] ELSE NULL END AS datetime)), MIN(CAST(CASE WHEN ISDATE(Record.[{1}]) > 0 THEN Record.[{1}] ELSE NULL END AS datetime)) " +
                    "FROM Record INNER JOIN GraphOptionDetail " +
                    "ON Record.TableID = GraphOptionDetail.TableID " +
                    "WHERE Record.IsActive=1 AND GraphOptionDetail.GraphOptionID={2}",dateFormat,
                    graphXAxisColumnName, ddlGraphOption.SelectedValue.Replace("-", "")));
                GraphOption tempGP = GraphManager.ets_GraphOption_Detail(int.Parse(ddlGraphOption.SelectedValue.Replace("-", "")));
                if (dtGO.Rows.Count > 0)
                {
                    if (dtGO.Rows[0][0] != DBNull.Value)
                    {
                        dtToDateTime = (DateTime)dtGO.Rows[0][0];
                        //dtFromDateTime = dtToDateTime.Value.AddYears(-1);
                        dtFromDateTime = (DateTime)dtGO.Rows[0][1];
                        ddlRecentPeriod.SelectedValue = "Y";
                        txtRecentNumber.Text = "";
                        if(tempGP!=null && tempGP.TimePeriod!="")
                        {
                            if(tempGP.TimePeriod.ToLower()=="y")
                            {
                                dtFromDateTime = dtToDateTime.Value.AddYears(-1);
                                txtRecentNumber.Text = "1";
                                ddlRecentPeriod.SelectedValue = "Y";
                                chkDateRange.Checked = true;
                            }
                            if (tempGP.TimePeriod.ToLower() == "m")
                            {
                                dtFromDateTime = dtToDateTime.Value.AddMonths(-1);
                                txtRecentNumber.Text = "1";
                                ddlRecentPeriod.SelectedValue = "M";
                                chkDateRange.Checked = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (strTableID != "")
                {
                    DataTable dtMaxTime = Common.DataTableFromText(
                        String.Format(@"SET dateformat {0};SELECT MAX(CAST(CASE WHEN ISDATE(Record.[{1}]) > 0 THEN Record.[{1}] ELSE NULL END AS datetime)), MIN(CAST(CASE WHEN ISDATE(Record.[{1}]) > 0 THEN Record.[{1}] ELSE NULL END AS datetime)) " +
                        "FROM Record WHERE Record.IsActive=1 AND TableID={2}", dateFormat, graphXAxisColumnName, strTableID));
                    if (dtMaxTime.Rows.Count > 0 && dtMaxTime.Rows[0][0] != DBNull.Value)
                    {
                        dtToDateTime = (DateTime)dtMaxTime.Rows[0][0];
                        if (Page.IsPostBack)
                            dtFromDateTime = (DateTime)dtMaxTime.Rows[0][1];
                        else
                        {
                            Table theTable = RecordManager.ets_Table_Details(int.Parse(strTableID));
                            if (theTable != null && theTable.GraphDefaultPeriod.HasValue)
                            {
                                int periodID = theTable.GraphDefaultPeriod.Value;
                                //<asp:ListItem Value="0">All</asp:ListItem>
                                //<asp:ListItem Value="1">1 year</asp:ListItem>
                                //<asp:ListItem Value="2">6 months</asp:ListItem>
                                //<asp:ListItem Value="3">3 months</asp:ListItem>
                                //<asp:ListItem Value="4">1 month</asp:ListItem>
                                //<asp:ListItem Value="5">1 week</asp:ListItem>
                                //<asp:ListItem Value="6">1 day</asp:ListItem>
                                switch (periodID)
                                {
                                    case 0:
                                        dtFromDateTime = (DateTime)dtMaxTime.Rows[0][1];
                                        break;
                                    case 1:
                                        dtFromDateTime = dtToDateTime.Value.AddYears(-1);
                                        break;
                                    case 2:
                                        dtFromDateTime = dtToDateTime.Value.AddMonths(-6);
                                        break;
                                    case 3:
                                        dtFromDateTime = dtToDateTime.Value.AddMonths(-3);
                                        break;
                                    case 4:
                                        dtFromDateTime = dtToDateTime.Value.AddMonths(-1);
                                        break;
                                    case 5:
                                        dtFromDateTime = dtToDateTime.Value.AddDays(-7);
                                        break;
                                    case 6:
                                        dtFromDateTime = dtToDateTime.Value.AddDays(-1);
                                        break;
                                    default:
                                        dtFromDateTime = (DateTime)dtMaxTime.Rows[0][1];
                                        break;
                                }
                            }
                            else
                            {
                                dtFromDateTime = (DateTime)dtMaxTime.Rows[0][1];
                            }
                            if ((ddlTimePeriodDisplay_Simple.Text != "H") && (ddlTimePeriodDisplay_Simple.Text != "C"))
                            {
                                dtToDateTime = dtToDateTime.Value.AddDays(1);
                                dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, dtToDateTime.Value.Day,
                                    0, 0, 0);
                                dtFromDateTime = new DateTime(dtFromDateTime.Value.Year, dtFromDateTime.Value.Month, dtFromDateTime.Value.Day,
                                    0, 0, 0);
                            }
                        }
                    }
                }
            }

            if (ddlTimePeriodDisplay_Simple.Text != "")
            {
                if (ddlTimePeriodDisplay_Simple.Text != "H")
                {
                    //dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                    dtToDateTime = dtToDateTime.Value.AddDays(1);
                    dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, dtToDateTime.Value.Day,
                        0, 0, 0);
                    switch (ddlTimePeriodDisplay_Simple.Text)
                    {
                        case "D":
                            dtToDateTime = dtToDateTime.Value.AddDays(1);
                           
                            //dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, dtToDateTime.Value.Day, 23, 59, 59);
                            break;
                        case "W":
                            dtToDateTime = Common.PreviousMonday((DateTime)dtToDateTime);
                            
                            dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, dtToDateTime.Value.Day, 0, 0, 0);
                            dtToDateTime = dtToDateTime.Value.AddDays(7);
                            break;
                        case "M":
                            dtToDateTime = dtToDateTime.Value.AddMonths(1);
                            dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, 1, 0, 0, 0);
                            //dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, 1, 23, 59, 59);
                          
                            break;
                        case "Y":
                            dtToDateTime = dtToDateTime.Value.AddYears(1);
                            dtToDateTime = new DateTime(dtToDateTime.Value.Year, 1, 1, 0, 0, 0);
                            break;
                    }
                }
                else
                {
                    dtToDateTime = DateTime.ParseExact(dtToDateTime.Value.Day.ToString() + "/" + dtToDateTime.Value.Month.ToString() + "/" + dtToDateTime.Value.Year.ToString() + " " + dtToDateTime.Value.AddHours(1).Hour.ToString() + ":00", "d/M/yyyy H:mm", CultureInfo.InvariantCulture);
                }

                //if (chkUseReportDates.Checked && DocumentID != null)
                //{
                //    Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);
                //    if (theDocument != null)
                //    {
                //        if (theDocument.DocumentDate != null)
                //        {
                //            dtFromDateTime = theDocument.DocumentDate;
                //        }
                //        if (theDocument.DocumentEndDate != null)
                //        {
                //            dtToDateTime = theDocument.DocumentEndDate;
                //        }
                //    }
                //}
            }
        }
        else
        {

            dtToDateTime = (DateTime)ViewState["dtToDateTime"];
            dtFromDateTime = (DateTime)ViewState["dtFromDateTime"];
            if (_bToDateUp == null && _bDotClick == false)
            {
                switch (ddlTimePeriodDisplay_Simple.Text)
                {
                    case "C":
                        if (txtStartDate_Simple.Text != "")
                        {
                            ViewState["dtFromDateTime"] =
                                txtStartDate_Simple.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(
                                txtFromTime_Simple.Text == "" ? txtStartDate_Simple.Text + " 00:00" : txtStartDate_Simple.Text + " " + txtFromTime_Simple.Text, "d/M/yyyy H:m",
                                CultureInfo.InvariantCulture);
                        }
                        if (txtEndDate_Simple.Text != "")
                        {
                            ViewState["dtToDateTime"] =
                                txtEndDate_Simple.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(
                                txtToTime_Simple.Text == "" ? txtEndDate_Simple.Text + " 00:00" : txtEndDate_Simple.Text + " " + txtToTime_Simple.Text, "d/M/yyyy H:m",
                                CultureInfo.InvariantCulture);
                        }
                        break;
                    case "H":
                      
                        int iHour = dtToDateTime.Value.Hour;
                        dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                        iHour = iHour + 1;
                        dtToDateTime = dtToDateTime.Value.AddHours(iHour);
                        break;
                    case "D":
                        dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                        dtToDateTime = dtToDateTime.Value.AddDays(1);//.AddSeconds(-1);
                        break;
                    case "W":
                         dtToDateTime = Common.PreviousMonday((DateTime)dtToDateTime);
                            
                            dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, dtToDateTime.Value.Day, 0, 0, 0);
                            dtToDateTime = dtToDateTime.Value.AddDays(7);

                        break;
                    case "M":
                        //dtFromDateTime = dtToDateTime.Value.AddMonths(-1);
                        //dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                        //dtToDateTime = dtToDateTime.Value.AddDays(-dtToDateTime.Value.Day + 1);
                        dtToDateTime = dtToDateTime.Value.AddMonths(1);//.AddSeconds(-1);
                        dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, 1, 0, 0, 0);

                        //dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, 1);

                        break;
                    case "Y":
                        // dtFromDateTime = dtToDateTime.Value.AddYears(-1).AddDays(1);
                        //dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                        //dtToDateTime = dtToDateTime.Value.AddDays(-dtToDateTime.Value.Day + 1);
                        //dtToDateTime = dtToDateTime.Value.AddMonths(-dtToDateTime.Value.Month + 1);
                        //dtToDateTime = dtToDateTime.Value.AddYears(1).AddSeconds(-1);
                        dtToDateTime = dtToDateTime.Value.AddYears(1);
                        dtToDateTime = new DateTime(dtToDateTime.Value.Year, 1, 1, 0, 0, 0);
                       // dtToDateTime = new DateTime(dtToDateTime.Value.Year, 12, 31, 23, 59, 59);

                        break;
                    default:
                        break;
                }
            }
        }

        if (_bToDateUp != null)
        {
            if (_bToDateUp.Value)
            {
                if (ddlTimePeriodDisplay_Simple.Text != "C")
                {
                    dtFromDateTime = dtToDateTime.Value;//.AddSeconds(1);
                }
                switch (ddlTimePeriodDisplay_Simple.Text)
                {
                    case "H":
                        dtToDateTime = dtFromDateTime.Value.AddHours(1);
                        break;
                    case "D":
                        dtToDateTime = dtFromDateTime.Value.AddDays(1);
                        break;
                    case "W":
                        dtToDateTime = dtFromDateTime.Value.AddDays(7);
                        break;
                    case "M":
                        dtToDateTime = dtFromDateTime.Value.AddMonths(1);

                        break;
                    case "Y":
                        dtToDateTime = dtFromDateTime.Value.AddYears(1);
                        
                        break;
                    //case "C":
                    //    dtToDateTime = dtToDateTime.Value.AddDays(1);
                    //    break;
                }
                if (ddlTimePeriodDisplay_Simple.Text != "C")
                {
                    dtToDateTime = dtToDateTime.Value;//.AddSeconds(-1);
                }
            }
            else
            {
                DateTime dtTemp = dtFromDateTime.Value;
                switch (ddlTimePeriodDisplay_Simple.Text)
                {
                    case "H":
                        dtFromDateTime = dtFromDateTime.Value.AddHours(-1);
                        break;
                    case "D":
                        dtFromDateTime = dtFromDateTime.Value.AddDays(-1);
                        break;
                    case "W":
                        dtFromDateTime = dtFromDateTime.Value.AddDays(-7);
                        break;
                    case "M":
                        dtFromDateTime = dtFromDateTime.Value.AddMonths(-1);
                        break;
                    case "Y":
                        dtFromDateTime = dtFromDateTime.Value.AddYears(-1);
                        break;
                    //case "C":
                    //    dtToDateTime = dtToDateTime.Value.AddDays(1);
                    //    break;
                }
                if (ddlTimePeriodDisplay_Simple.Text != "C")
                {
                    dtToDateTime = dtTemp;//.AddSeconds(-1);
                }
            }
        }
        else
        {
            //making from date
            switch (ddlTimePeriodDisplay_Simple.Text)
            {
                case "H":
                    dtFromDateTime = dtToDateTime.Value.AddHours(-1);
                    break;
                case "D":
                    dtFromDateTime = dtToDateTime.Value.AddDays(-1);
                    break;
                case "W":
                    dtFromDateTime = dtToDateTime.Value.AddDays(-7);
                    break;
                case "M":
                    dtFromDateTime = dtToDateTime.Value.AddMonths(-1);
                    break;
                case "Y":
                    dtFromDateTime = dtToDateTime.Value.AddYears(-1);
                    break;
                //case "C":
                //    //dtFromDateTime = dtToDateTime.Value.AddYears(-1);
                //    if (ViewState["dtFromDateTime"] != null)
                //    {
                //        dtFromDateTime = (DateTime)ViewState["dtFromDateTime"];
                //        if (_bToDateUp != null)
                //        {
                //            if (_bToDateUp == true)
                //            {
                //                dtFromDateTime = dtFromDateTime.Value.AddDays(1);
                //            }
                //            else
                //            {
                //                dtFromDateTime = dtFromDateTime.Value.AddDays(-1);
                //            }

                //            //update textboxes
                //            txtStartDate.Text = ConvertUtil.GetDateString((DateTime)dtFromDateTime);
                //            txtEndDate.Text = ConvertUtil.GetDateString((DateTime)dtToDateTime);

                //            txtFromTime.Text = ((DateTime)dtFromDateTime).Hour.ToString() + ":" + ((DateTime)dtFromDateTime).Minute.ToString();
                //            txtToTime.Text = ((DateTime)dtToDateTime).Hour.ToString() + ":" + ((DateTime)dtToDateTime).Minute.ToString();
                //        }
                //    }
                //    break;
            }
        }


        //check if there is any data

        bool bChangeDate = true;

        if (ddlTimePeriodDisplay_Simple.Text != "C")
        {
            int iRecordCount = 0;
            string strTableID = ddlGraphOption.SelectedValue;
            string strSTIDs = "";
            if (ddlGraphOption.SelectedValue.IndexOf("-") > -1)
            {
                if (ViewState["GraphOptionDetail"] != null)
                {

                    DataTable dtTempGD = ((DataTable)ViewState["GraphOptionDetail"]);

                    foreach (DataRow drST in dtTempGD.Rows)
                    {
                        strSTIDs = strSTIDs + drST["TableID"].ToString() + ",";
                    }

                }
            }
            else
            {
                strSTIDs = strTableID + ",";
            }

            if (strSTIDs == ",")
                strSTIDs = "";
            strSTIDs = strSTIDs + "-1";

            iRecordCount = GraphManager.ets_Record_RecordCount(strSTIDs, (DateTime)dtFromDateTime, (DateTime)dtToDateTime, graphXAxisColumnName);

            DataTable dtMaxTime = Common.DataTableFromText(String.Format(@"SET dateformat dmy; SELECT MAX(CAST(CASE WHEN ISDATE(Record.[{0}]) > 0 THEN Record.[{0}] ELSE NULL END AS datetime))
                    FROM Record WHERE IsActive=1 AND TableID IN ({1})", graphXAxisColumnName, strSTIDs));

            DataTable dtMinTime = Common.DataTableFromText(String.Format(@"SET dateformat dmy; SELECT MIN(CAST(CASE WHEN ISDATE(Record.[{0}]) > 0 THEN Record.[{0}] ELSE NULL END AS datetime))
                    FROM Record WHERE IsActive=1 AND TableID IN({1})", graphXAxisColumnName, strSTIDs));

            bool bInsideDateRange = false;

            if (dtMinTime.Rows.Count > 0 && dtMinTime.Rows[0][0] != DBNull.Value
                && dtMaxTime.Rows.Count > 0 && dtMaxTime.Rows[0][0] != DBNull.Value)
            {

                if (DateTime.Parse(dtMinTime.Rows[0][0].ToString()) <= (DateTime)dtFromDateTime &&
                    (DateTime)dtFromDateTime <= DateTime.Parse(dtMaxTime.Rows[0][0].ToString()))
                {
                    bInsideDateRange = true;
                }

                if (DateTime.Parse(dtMinTime.Rows[0][0].ToString()) > (DateTime)dtFromDateTime &&
                    (DateTime)dtFromDateTime <= DateTime.Parse(dtMaxTime.Rows[0][0].ToString()))
                {
                    bInsideDateRange = true;
                }

                if (DateTime.Parse(dtMinTime.Rows[0][0].ToString()) > (DateTime)dtToDateTime)
                {
                    bInsideDateRange = false;
                }

            }



            if (/*iRecordCount == 0 && */bInsideDateRange == false)
            {

                if (_bToDateUp != null)
                {
                    bChangeDate = false;
                    if ((bool)_bToDateUp)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LastRecord", "alert('This is the latest data available');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "earliestRecord", "alert('This is the earliest data available');", true);
                    }

                }
                else
                {

                    if (dtMaxTime.Rows.Count > 0 && dtMaxTime.Rows[0][0] != DBNull.Value
                        && dtMinTime.Rows.Count > 0 && dtMinTime.Rows[0][0] != DBNull.Value)
                    {
                        bool bLatest = false;
                        if ((DateTime)dtMaxTime.Rows[0][0] < dtToDateTime)
                        {
                            dtToDateTime = (DateTime)dtMaxTime.Rows[0][0];
                            bLatest = true;
                        }
                        else
                        {
                            dtToDateTime = (DateTime)dtMinTime.Rows[0][0];
                        }

                        switch (ddlTimePeriodDisplay_Simple.Text)
                        {
                            case "H":
                                int iHour = dtToDateTime.Value.Hour;
                                dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                                iHour = iHour + 1;
                                dtToDateTime = dtToDateTime.Value.AddHours(iHour);
                                break;
                            case "D":
                                dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                                dtToDateTime = dtToDateTime.Value.AddDays(1);
                                break;
                            case "W":
                                 dtToDateTime = Common.PreviousMonday((DateTime)dtToDateTime);
                            
                            dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, dtToDateTime.Value.Day, 0, 0, 0);
                            dtToDateTime = dtToDateTime.Value.AddDays(7);
                                break;
                            case "M":
                                //dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                                //dtToDateTime = dtToDateTime.Value.AddDays(-dtToDateTime.Value.Day + 1);
                                dtToDateTime = dtToDateTime.Value.AddMonths(1);
                                dtToDateTime = new DateTime(dtToDateTime.Value.Year, dtToDateTime.Value.Month, 1, 0, 0, 0);
                                break;
                            case "Y":
                                //dtToDateTime = Convert.ToDateTime(dtToDateTime.Value.ToShortDateString());
                                //dtToDateTime = dtToDateTime.Value.AddDays(-dtToDateTime.Value.Day + 1);
                                //dtToDateTime = dtToDateTime.Value.AddMonths(-dtToDateTime.Value.Month + 1);
                                dtToDateTime = dtToDateTime.Value.AddYears(1);
                                dtToDateTime = new DateTime(dtToDateTime.Value.Year, 1, 1, 0, 0, 0);
                                break;
                        }




                        switch (ddlTimePeriodDisplay_Simple.Text)
                        {
                            case "H":
                                dtFromDateTime = dtToDateTime.Value.AddHours(-1);
                                break;
                            case "D":
                                dtFromDateTime = dtToDateTime.Value.AddDays(-1);
                                break;
                            case "W":
                                dtFromDateTime = dtToDateTime.Value.AddDays(-7);
                                break;
                            case "M":
                                dtFromDateTime = dtToDateTime.Value.AddMonths(-1);
                                break;
                            case "Y":
                                dtFromDateTime = dtToDateTime.Value.AddYears(-1);
                                break;
                        }

                        if (bLatest)
                        {
                            Session["tdbmsgpb"] = "This is the latest data available";
                           // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LastRecord", "alert('This is the latest data available');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "earliestRecord", "alert('This is the earliest data available');", true);
                        }


                    }
                }

            }
        }

        //////


        //            //update textboxes
        txtStartDate_Simple.Text = ConvertUtil.GetDateString((DateTime)dtFromDateTime);
        txtEndDate_Simple.Text = ConvertUtil.GetDateString((DateTime)dtToDateTime);

        txtFromTime_Simple.Text = ((DateTime)dtFromDateTime).Hour.ToString() + ":" + ((DateTime)dtFromDateTime).Minute.ToString();
        txtToTime_Simple.Text = ((DateTime)dtToDateTime).Hour.ToString() + ":" + ((DateTime)dtToDateTime).Minute.ToString();

        if (bChangeDate)
        {
            ViewState["dtToDateTime"] = dtToDateTime;
            ViewState["dtFromDateTime"] = dtFromDateTime;
        }

        if (ddlTimePeriodDisplay_Simple.Text == "C")
        {
            ViewState["C_dtToDateTime"] = ViewState["dtToDateTime"];
            ViewState["C_dtFromDateTime"] = ViewState["dtFromDateTime"];

        }


        //SetDateTimeSelectorsValue();
        SetPrevNextDateState();

    }

    protected void btnRefreshChartPop_Click(object sender, EventArgs e)
    {
        try
        {
            int iSearchCriteriaID = int.Parse(hfDetailSearchID.Value);
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);




            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));
            string strGraphOptionDetailID = xmlDoc.FirstChild["GraphOptionDetailID"].InnerText;
            string strModeDetail = xmlDoc.FirstChild["ModeDetail"].InnerText;
            string strTableID = xmlDoc.FirstChild["TableID"].InnerText;
            string strColumnID = xmlDoc.FirstChild["ColumnID"].InnerText;
            //string strLocationID = xmlDoc.FirstChild["LocationID"].InnerText;
            //string strLocationName = xmlDoc.FirstChild["LocationName"].InnerText;
            string strGraphType = xmlDoc.FirstChild["GraphType"].InnerText;
            string strAxis = xmlDoc.FirstChild["Axis"].InnerText;
            string strColour = xmlDoc.FirstChild["Colour"].InnerText;
            string strHigh = xmlDoc.FirstChild["High"].InnerText;
            string strLow = xmlDoc.FirstChild["Low"].InnerText;
            string strLabel = xmlDoc.FirstChild["Label"].InnerText;

            if (Mode == "edit" && strModeDetail == "add")
            {
                GraphOptionDetail theGraphOptionDetail = new GraphOptionDetail(null, null, null, null);
                theGraphOptionDetail.GraphOptionID = (int)GraphOptionID;
                theGraphOptionDetail.TableID = int.Parse(strTableID);
                theGraphOptionDetail.ColumnID = int.Parse(strColumnID);
                //theGraphOptionDetail.LocationID = strLocationID == "" ? null : (int?)int.Parse(strLocationID);
                theGraphOptionDetail.Axis = strAxis;
                theGraphOptionDetail.GraphType = strGraphType;
                theGraphOptionDetail.Colour = strColour == "" ? null : strColour;
                theGraphOptionDetail.Low = strLow == "" ? null : (double?)double.Parse(strLow);
                theGraphOptionDetail.High = strHigh == "" ? null : (double?)double.Parse(strHigh);
                theGraphOptionDetail.Label = strLabel;

                GraphManager.ets_GraphOptionDetail_Insert(theGraphOptionDetail);
                int iINTemp = 0;
                ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
        null, null, ref iINTemp);
                ViewState["GraphOptionDetailCount"] = iINTemp;



            }

            if (Mode == "edit" && strModeDetail == "edit")
            {
                GraphOptionDetail theGraphOptionDetail = GraphManager.ets_GraphOptionDetail_Detail(int.Parse(strGraphOptionDetailID));

                if (theGraphOptionDetail != null)
                {
                    theGraphOptionDetail.TableID = int.Parse(strTableID);
                    theGraphOptionDetail.ColumnID = int.Parse(strColumnID);
                    //theGraphOptionDetail.LocationID = strLocationID == "" ? null : (int?)int.Parse(strLocationID);
                    theGraphOptionDetail.Axis = strAxis;
                    theGraphOptionDetail.GraphType = strGraphType;
                    theGraphOptionDetail.Colour = strColour == "" ? null : strColour;
                    theGraphOptionDetail.Low = strLow == "" ? null : (double?)double.Parse(strLow);
                    theGraphOptionDetail.High = strHigh == "" ? null : (double?)double.Parse(strHigh);
                    theGraphOptionDetail.Label = strLabel;

                    GraphManager.ets_GraphOptionDetail_Update(theGraphOptionDetail);
                    int iINTemp = 0;
                    ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
            null, null, ref iINTemp);
                    ViewState["GraphOptionDetailCount"] = iINTemp;

                }

            }

            if (Mode == "add" && strModeDetail == "edit")
            {
                int iIndex = int.Parse(strGraphOptionDetailID) - 1;
                DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];

                theTable.Rows[iIndex]["GraphOptionID"] = -1;
                theTable.Rows[iIndex]["TableID"] = int.Parse(strTableID);
                theTable.Rows[iIndex]["ColumnID"] = int.Parse(strColumnID);

                Column theColumn = RecordManager.ets_Column_Details(int.Parse(strColumnID));

                if (theColumn != null)
                {

                    theTable.Rows[iIndex]["SystemName"] = theColumn.SystemName;
                }

                theTable.Rows[iIndex]["Axis"] = strAxis;
                if (strColour == "")
                {
                    theTable.Rows[iIndex]["Colour"] = DBNull.Value;
                }
                else
                {
                    theTable.Rows[iIndex]["Colour"] = strColour;
                }

                theTable.Rows[iIndex]["GraphType"] = strGraphType;
                if (strLow == "")
                {
                    theTable.Rows[iIndex]["Low"] = DBNull.Value;
                }
                else
                {
                    theTable.Rows[iIndex]["Low"] = double.Parse(strLow);
                }
                if (strHigh == "")
                {
                    theTable.Rows[iIndex]["High"] = DBNull.Value;
                }
                else
                {
                    theTable.Rows[iIndex]["High"] = double.Parse(strHigh);
                }

                theTable.Rows[iIndex]["Label"] = strLabel;

                //if (strLocationID != "")
                //{
                //    theTable.Rows[iIndex]["LocationID"] = int.Parse(strLocationID);
                //    theTable.Rows[iIndex]["LocationName"] = strLocationName;
                //}
                //else
                //{
                //    theTable.Rows[iIndex]["LocationID"] = DBNull.Value;
                //    theTable.Rows[iIndex]["LocationName"] = "All";
                //}


                theTable.AcceptChanges();
                ViewState["GraphOptionDetail"] = theTable;


            }


            if (Mode == "add" && strModeDetail == "add")
            {
                DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
                DataRow theRecord = theTable.NewRow();
                theRecord["GraphOptionDetailID"] = theTable.Rows.Count + 1;
                theRecord["GraphOptionID"] = -1;
                theRecord["TableID"] = int.Parse(strTableID);
                theRecord["ColumnID"] = int.Parse(strColumnID);


                Column theColumn = RecordManager.ets_Column_Details(int.Parse(strColumnID));

                if (theColumn != null)
                {

                    theRecord["SystemName"] = theColumn.SystemName;
                }

                theRecord["Axis"] = strAxis;
                if (strColour == "")
                {
                    theRecord["Colour"] = DBNull.Value;
                }
                else
                {
                    theRecord["Colour"] = strColour;
                }

                theRecord["GraphType"] = strGraphType;
                if (strLow == "")
                {
                    theRecord["Low"] = DBNull.Value;
                }
                else
                {
                    theRecord["Low"] = double.Parse(strLow);
                }
                if (strHigh == "")
                {
                    theRecord["High"] = DBNull.Value;
                }
                else
                {
                    theRecord["High"] = double.Parse(strHigh);
                }

                theRecord["Label"] = strLabel;

                //if (strLocationID != "")
                //{
                //    theRecord["LocationID"] = int.Parse(strLocationID);
                //    theRecord["LocationName"] = strLocationName;
                //}
                //else
                //{
                //    theRecord["LocationID"] = DBNull.Value;
                //    theRecord["LocationName"] = "All";
                //}

                theTable.Rows.Add(theRecord);
                theTable.AcceptChanges();
                ViewState["GraphOptionDetail"] = theTable;
                ViewState["GraphOptionDetailCount"] = (int)ViewState["GraphOptionDetailCount"] + 1;

            }

            if (DocumentID != null)
            {
                //if (gvTheGrid.Rows.Count == 0)
                //{
                    if (strTableID != "")
                    {
                        Table theTable = RecordManager.ets_Table_Details(int.Parse(strTableID));
                        if (theTable != null)
                        {
                            txtGraphTitle_Simple.Text = theTable.TableName;
                        }
                    }
                //}

            }

        }
        catch
        {
            //
        }



        //BindTheGrid(0, _iMaxRows);

        _bOldDate = true;
        MakeChart();
    }

    protected void btnRefreshChart_Click(object sender, EventArgs e)
    {
        try
        {
            if (hfDateTimeValue.Value == "")
                return;

            //_bByPassToDate = false;
            _bToDateUp = null;
           
            DateTime dtLabel = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            dtLabel = dtLabel.AddMilliseconds(Int64.Parse(hfDateTimeValue.Value));
            
            DateTime dtStart = new DateTime(dtLabel.Year, dtLabel.Month, dtLabel.Day, dtLabel.Hour, 0, 0);;
            DateTime dtEnd = dtStart.AddHours(1);//.AddSeconds(-1);
            switch (ddlTimePeriodDisplay_Simple.Text)
            {
                case "H":
                    //
                    break;
                case "D":
                    ddlTimePeriodDisplay_Simple.Text = "H";
                    dtStart = new DateTime(dtLabel.Year, dtLabel.Month, dtLabel.Day, dtLabel.Hour, 0, 0);
                    dtEnd = dtStart.AddHours(1);//.AddSeconds(-1);
                    break;
                case "W":
                    ddlTimePeriodDisplay_Simple.Text = "D";
                    dtStart = new DateTime(dtLabel.Year, dtLabel.Month, dtLabel.Day, 0, 0, 0);
                    dtEnd = dtStart.AddDays(1);//.AddSeconds(-1);
                    break;
                case "M":
                    ddlTimePeriodDisplay_Simple.Text = "D";
                    dtStart = new DateTime(dtLabel.Year, dtLabel.Month, dtLabel.Day, 0, 0, 0);
                    dtEnd = dtStart.AddDays(1);//.AddSeconds(-1);
                    break;
                case "Y":
                    ddlTimePeriodDisplay_Simple.Text = "M";
                    dtStart = new DateTime(dtLabel.Year, dtLabel.Month, 1, 0, 0, 0);
                    dtEnd = dtStart.AddMonths(1);//.AddSeconds(-1);
                    break;
            }
            //ddlTimePeriodDisplay_Simple.Text = ddlTimePeriod.SelectedValue;
            //ddlTimePeriodDisplay.Text = ddlTimePeriod.SelectedValue;

            if (ddlTimePeriodDisplay_Simple.SelectedValue != "C")
            {
                ViewState["dtFromDateTime"] = dtStart;
                ViewState["dtToDateTime"] = dtEnd;
                SetDateTimeSelectorsValue();
            }

            SetDateTimeSelectorsState();
            SetPrevNextDateState();

            _bDotClick = true;
            MakeChart();
        }
        catch
        {
            //
        }
    }



    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlGraphOption.Items.Clear();
        ddlGraphOption.SelectedValue = null;
        ddlGraphOption.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                (int)AccountID,
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, STs);

        ddlGraphOption.DataBind();

        //ddlTable.DataSource = ddlGraphOption.DataSource;
        //ddlTable.DataBind();


        if (iTN < 1)
        {
            System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("None", "-1");
            ddlGraphOption.Items.Insert(0, liNone);

            //System.Web.UI.WebControls.ListItem liPlease = new System.Web.UI.WebControls.ListItem("--Please Select--", "-1");
            //ddlTable.Items.Insert(0, liPlease);
        }


        DataTable dtRecord = Common.DataTableFromText(@"SELECT * FROM GraphOption WHERE 
            AccountID=" + AccountID.ToString() + " AND GraphPanel=2 AND IsActive=1 AND ReportChart=0 ORDER BY Heading DESC");
        foreach (DataRow drEachST in dtRecord.Rows)
        {

            System.Web.UI.WebControls.ListItem liSTs = new System.Web.UI.WebControls.ListItem(drEachST["Heading"].ToString(),
                "-" + drEachST["GraphOptionID"].ToString());
            ddlGraphOption.Items.Insert(0, liSTs);

        }

    }

    protected void PopulateYAxis()
    {
        ddlEachAnalyte_Simple.Items.Clear();
        int iTN = 0;

        //if (ddlGraphOption.SelectedValue.IndexOf("-") > -1 || ddlGraphOption.SelectedValue == "")
        //{
        //    ddlEachAnalyte_Simple.Visible = false;
        //    return;
        //}
        //else
        //{
        //    ddlEachAnalyte_Simple.Visible = true;
        //}
        //string strTableID = ddlGraphOption.SelectedValue;


        string strTableID = ddlGraphOption.SelectedValue;
        if (_gGraphOption != null)
        {
            DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];

            if (theTable != null && theTable.Rows.Count > 0)
            {
                int tableID = (int)theTable.Rows[0]["TableID"];
                strTableID = tableID.ToString();
            }

        }
        

        if (strTableID == "")
            strTableID = "-1";


        List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(strTableID),
               null, null, ref iTN);

        Column dtColumn = new Column();
        foreach (Column eachColumn in lstColumns)
        {
            if (eachColumn.IsStandard == true)
            {
                switch (eachColumn.SystemName.ToLower())
                {
                    case "datetimerecorded":
                        ViewState["DateTimeSummary"] = eachColumn.GraphLabel;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                if (eachColumn.GraphLabel != "" && (eachColumn.ColumnType == "number" || eachColumn.ColumnType == "calculation"))
                {
                    System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.SystemName);

                    ddlEachAnalyte_Simple.Items.Insert(ddlEachAnalyte_Simple.Items.Count, aItem);
                }
            }
        }

        if ( !IsPostBack && _gGraphOption==null && OneTable != null && OneTable.GraphDefaultYAxisColumnID!=null)
        {
            string strSysName = TheDatabaseS.SystemNameFromColumnID((int)OneTable.GraphDefaultYAxisColumnID);
            if (ddlEachAnalyte_Simple.Items.FindByValue(strSysName) != null)
                ddlEachAnalyte_Simple.SelectedValue = strSysName;
        }
    }

    //protected void gvTheGrid_PreRender(object sender, EventArgs e)
    //{
    //    GridView grid = (GridView)sender;
    //    if (grid != null)
    //    {
    //        GridViewRow pagerRow = (GridViewRow)grid.TopPagerRow;
    //        if (pagerRow != null)
    //        {
    //            pagerRow.Visible = true;
    //        }
    //    }
    //}


    //protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    //{

    //    BindTheGrid(0, gvTheGrid.PageSize);
    //    MakeChart();
    //}

    //protected void PopulateLocation()
    //{
    //    ddlLocation.Items.Clear();
    //    int iTN = 0;

    //    ddlLocation.DataSource = SiteManager.ets_Location_Select(null, int.Parse(ddlTable.SelectedValue), null, null, "",
    //                   null, "", true, null, null, null, null,
    //                   (int)AccountID,
    //                   "LocationName", "ASC", null, null, ref iTN, "");

    //    ddlLocation.DataBind();


    //    System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("--All--", "");
    //    ddlLocation.Items.Insert(0, liAll);

    //}


    //protected void PopulateAnalyte()
    //{
    //    ddlAnalyte.Items.Clear();
    //    int iTN = 0;

    //    string strTableID = ddlTable.SelectedValue;

    //    if (int.Parse(ddlTable.SelectedValue) < 0)
    //    {
    //        //its a graphoption
    //        ddlAxes.Visible = false;
    //    }
    //    else
    //    {
    //        ddlAxes.Visible = true;
    //        List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(strTableID),
    //               null, null, ref iTN);

    //        Column dtColumn = new Column();
    //        foreach (Column eachColumn in lstColumns)
    //        {
    //            if (eachColumn.IsStandard == true)
    //            {
    //                switch (eachColumn.SystemName.ToLower())
    //                {
    //                    case "datetimerecorded":
    //                        ViewState["DateTimeSummary"] = eachColumn.GraphLabel;
    //                        break;

    //                    default:
    //                        break;
    //                }
    //            }
    //            else
    //            {
    //                if (eachColumn.GraphLabel != "")
    //                {
    //                    System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString());

    //                    ddlAnalyte.Items.Insert(ddlAnalyte.Items.Count, aItem);
    //                }



    //            }

    //        }

    //    }



    //}

    protected void ddlEachTable_Simple_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int iTN = 0;
            DropDownList ddlEachTable = sender as DropDownList;
            if (ddlEachTable != null)
            {
                ddlEachAnalyte_Simple.Items.Clear();
                string strTableID = ddlEachTable.SelectedValue;

                List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(strTableID),
                        null, null, ref iTN);

                Column dtColumn = new Column();
                foreach (Column eachColumn in lstColumns)
                {
                    if (eachColumn.IsStandard == true)
                    {
                        switch (eachColumn.SystemName.ToLower())
                        {
                            case "datetimerecorded":
                                ViewState["DateTimeSummary"] = eachColumn.GraphLabel;
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (eachColumn.GraphLabel != "" && eachColumn.ColumnType == "number")
                        {
                            System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.SystemName);

                            ddlEachAnalyte_Simple.Items.Insert(ddlEachAnalyte_Simple.Items.Count, aItem);
                        }
                    }
                }

                txtGraphTitle_Simple.Text = ddlEachTable.SelectedItem.Text;
                txtGraphSubtitle_Simple.Text = ddlEachAnalyte_Simple.SelectedItem.Text;

                //update virtual table
                DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
                if (theTable.Rows.Count==0)
                {
                   DataRow drNew= theTable.NewRow();
                   theTable.Rows.Add(drNew);
                   theTable.AcceptChanges();
                }
                    

                theTable.Rows[0]["TableID"] = int.Parse(ddlEachTable.SelectedValue);
                DataTable dtTemp = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE   TableID=" + ddlEachTable.SelectedValue.ToString() + " AND SystemName='" + ddlEachAnalyte_Simple.SelectedValue + "'");

                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["ColumnID"] != null && dtTemp.Rows[0]["ColumnID"].ToString() != "")
                    {
                        theTable.Rows[0]["ColumnID"] = int.Parse(dtTemp.Rows[0]["ColumnID"].ToString());
                    }
                }

                theTable.Rows[0]["SystemName"] = ddlEachAnalyte_Simple.SelectedValue;
                theTable.Rows[0]["TableName"] = ddlEachTable.SelectedItem.Text;
                theTable.Rows[0]["GraphLabel"] = ddlEachAnalyte_Simple.SelectedItem.Text;
                //theTable.Rows[0]["LocationName"] = "All";
                //theTable.Rows[0]["LocationID"] = DBNull.Value;
                theTable.Rows[0]["Low"] = DBNull.Value;
                theTable.Rows[0]["High"] = DBNull.Value;
                theTable.Rows[0]["GraphSeriesColumnID"] = DBNull.Value;
                theTable.Rows[0]["GraphSeriesID"] = DBNull.Value;
                theTable.AcceptChanges();
                ViewState["GraphOptionDetail"] = theTable;

                //if (gvTheGrid.Rows.Count == 1)
                //{
                    txtGraphTitle_Simple.Text = ddlEachTable.SelectedItem.Text + " - " + ddlEachAnalyte_Simple.SelectedItem.Text;
                    ddlGraphOption.Text = ddlEachTable.SelectedValue;
                    ddlGrpahOption_SelectedIndexChanged(null, null);
                //}
            }

            //BindTheGrid(0, _iMaxRows);

            //BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);

            PopulateSampleSitesList(ddlEachTable.SelectedValue);

            _bOldDate = true;
            MakeChart();
        }
        catch
        {
            MakeNoDataChart();
        }
    }

    protected void ddlEachAnalyte_Simple_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlEachAnalyte = sender as DropDownList;
        if (ddlEachAnalyte != null)
        {

            if (int.Parse(ddlGraphOption.SelectedValue) < 0)
            {
                //ddlGraphOption.SelectedValue = OneTableID.ToString();
                if (ViewState["GraphOptionDetail"] != null)
                {
                    DataTable dtRecord = (DataTable)ViewState["GraphOptionDetail"];
                    if ((dtRecord.Rows.Count > 0) && !dtRecord.Rows[0].IsNull("TableID"))
                        ddlGraphOption.SelectedValue = dtRecord.Rows[0]["TableID"].ToString();
                }

            }


            //if (gvTheGrid.Rows.Count == 1)
            //{
            //if (ddlEachAnalyte_Simple.Items.FindByValue(ddlEachAnalyte.SelectedValue) != null)
            //    ddlEachAnalyte_Simple.Text = ddlEachAnalyte.SelectedValue;
            //}

            txtGraphTitle_Simple.Text = ddlGraphOption.SelectedItem.Text;
            txtGraphSubtitle_Simple.Text = ddlEachAnalyte.SelectedItem.Text;

            //update virtual table
            DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
            theTable.Rows[0]["TableID"] = int.Parse(ddlGraphOption.SelectedValue);
            DataTable dtTemp = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE   TableID=" + ddlGraphOption.SelectedValue.ToString() + " AND SystemName='" + ddlEachAnalyte.SelectedValue + "'");

            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["ColumnID"] != null && dtTemp.Rows[0]["ColumnID"].ToString() != "")
                {
                    theTable.Rows[0]["ColumnID"] = int.Parse(dtTemp.Rows[0]["ColumnID"].ToString());
                }
            }
            theTable.Rows[0]["SystemName"] = ddlEachAnalyte.SelectedValue;
            theTable.Rows[0]["TableName"] = ddlGraphOption.SelectedItem.Text;
            theTable.Rows[0]["GraphLabel"] = ddlEachAnalyte.SelectedItem.Text;
            theTable.Rows[0]["Label"] = ddlEachAnalyte.SelectedItem.Text;
            //theTable.Rows[0]["LocationName"] = "All";
            //theTable.Rows[0]["LocationID"] = DBNull.Value;
            theTable.Rows[0]["Low"] = DBNull.Value;
            theTable.Rows[0]["High"] = DBNull.Value;
            theTable.AcceptChanges();
            ViewState["GraphOptionDetail"] = theTable;

            //if (gvTheGrid.Rows.Count == 1)
            //{
                txtGraphTitle_Simple.Text = ddlGraphOption.SelectedItem.Text;
                txtGraphSubtitle_Simple.Text = ddlEachAnalyte.SelectedItem.Text;
            //}
        }

        ClearWarnings();
        ClearYAxisLimits();

        // BindTheGrid(0, _iMaxRows);

        _bOldDate = true;
        PopulateGraphDefinitionDDL();
        MakeChart();
    }

    protected void ddlGraphType_Simple_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
        theTable.Rows[0]["GraphType"] = (sender as DropDownList).SelectedValue;
        theTable.AcceptChanges();
        ViewState["GraphOptionDetail"] = theTable;

        int iGraphOptionDetailID = (int)theTable.Rows[0]["GraphOptionDetailID"];
        GraphOptionDetail theGraphOptionDetail = GraphManager.ets_GraphOptionDetail_Detail(iGraphOptionDetailID);
        if (theGraphOptionDetail != null)
        {
            theGraphOptionDetail.GraphType = theTable.Rows[0]["GraphType"].ToString();
            int iINTemp = 0;
            ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
                null, null, ref iINTemp);
            ViewState["GraphOptionDetailCount"] = iINTemp;
        }

        //BindTheGrid(0, _iMaxRows);

        _bOldDate = true;
        MakeChart();
    }

    //protected void cpColour_ColorUpdated(object sender, EventArgs e)
    //{
    //    DataTable theTable = (DataTable)ViewState["GraphOptionDetail"];
    //    theTable.Rows[0]["Colour"] = cpColour.ColorHEX;
    //    theTable.AcceptChanges();
    //    ViewState["GraphOptionDetail"] = theTable;

    //    int iGraphOptionDetailID = (int)theTable.Rows[0]["GraphOptionDetailID"];
    //    GraphOptionDetail theGraphOptionDetail = GraphManager.ets_GraphOptionDetail_Detail(iGraphOptionDetailID, null, null);
    //    if (theGraphOptionDetail != null)
    //    {
    //        theGraphOptionDetail.Colour = theTable.Rows[0]["GraphType"].ToString();
    //        int iINTemp = 0;
    //        ViewState["GraphOptionDetail"] = GraphManager.ets_GraphOptionDetail_Select((int)GraphOptionID, null, "GraphOrder", "ASC",
    //            null, null, ref iINTemp);
    //        ViewState["GraphOptionDetailCount"] = iINTemp;
    //    }

    //    BindTheGrid(0, _iMaxRows);
    //    _bOldDate = true;
    //    MakeChart();
    //}


    public int ChartWidth
    {
        get
        {
            return _iChartWidth;
        }
        set
        {
            _iChartWidth = value;
        }
    }


    public int ChartHeight
    {
        get
        {
            return _iChartHeight;
        }
        set
        {
            _iChartHeight = value;
        }

    }
    public int? AccountID { get; set; }
    public string STs { get; set; }

    public bool IsDashBorad { get; set; }

    //public string RecentDays { get; set; }


    public string  RecentDays
    {
        get
        {
            string strValueFields = "";

            if(chkDateRange.Checked)
            {
                if(txtRecentNumber.Text!="")
                {
                    _theChartDashBoard.RecentNumber = int.Parse(txtRecentNumber.Text);
                    _theChartDashBoard.RecentPeriod = ddlRecentPeriod.SelectedValue;
                    strValueFields=_theChartDashBoard.GetJSONString();
                }
            }
            return strValueFields;
        }
        set
        {
            string strValue = value;
            try
            {
                if(strValue!="")
                {
                    _theChartDashBoard = JSONField.GetTypedObject<ChartDashBoard>(strValue);
                }
            }
            catch
            {
                _theChartDashBoard = null;
            }
        }

    }

    //public WebChartViewer ChartControl
    //{
    //    get
    //    {
    //        //Page_Load(null, null);

    //        _theAccount = SecurityManager.Account_Details((int)AccountID);
    //        PopulateTableDDL();
    //        PopulateYAxis();
    //        PopulateRecord();

    //        MakeChart();
    //        return WebChartViewer1;
    //    }
    //}

    public void RefreshGraph()
    {
        _theAccount = SecurityManager.Account_Details((int)AccountID);
        PopulateTableDDL();
        PopulateYAxis();
        PopulateRecord();

        MakeChart();

    }

    public int? DocumentID
    {
        get
        {
            return _iDocumentID;
        }
        set
        {
            _iDocumentID = value;
        }

    }




    public string SaveFileName
    {
        get
        {
            return _strSaveFileName;
        }
        set
        {
            _strSaveFileName = value;
        }

    }


    public int? DocumentSectionID
    {
        get
        {
            return _iDocumentSectionID;
        }
        set
        {
            _iDocumentSectionID = value;
        }

    }


    public int? PreDocumentSectionID
    {
        get
        {
            return _iPreDocumentSectionID;
        }
        set
        {
            _iPreDocumentSectionID = value;
        }

    }



    public string Mode
    {
        get
        {
            if (ViewState["Mode"] == null)
            {
                return "add";
            }
            else
            {
                return ViewState["Mode"].ToString();
            }
        }
        set
        {
            ViewState["Mode"] = value;
        }

    }

    public int? OneTableID
    {
        get
        {
            return _iOneTableID;
        }
        set
        {
            _iOneTableID = value;
            if (_iOneTableID!=null)
                OneTable = RecordManager.ets_Table_Details((int)_iOneTableID);
        }

    }
    public Table OneTable
    {
        get
        {
            if(_theTable==null)
            {
                if (OneTableID != null)
                {
                    _theTable = RecordManager.ets_Table_Details((int)OneTableID);
                }
            }

            return _theTable;
        }
        set
        {
            _theTable = value;
        }

    }

    public bool ShowCustomDate
    {
        set
        {
            //trCustomDate.Visible = value;

            if (value)
            {
                ddlTimePeriodDisplay_Simple.SelectedValue = "C";
            }


        }

    }

    public bool ShowExportToPDF
    {
        set
        {
            //lnkExportToPdf.Visible = value;
        }

    }

    public bool ShowEmail
    {
        set
        {
            divEmail.Visible = value;
        }

    }

    public bool ShowNextPrevious
    {
        set
        {
            //tblNextPrevious.Visible = value;
        }

    }

    public bool ShowDates
    {
        set
        {
            //trStartDate.Visible = value;
            //trEndDate.Visible = value;
        }

    }

    public bool ShowUseReportDates
    {
        set
        {
            //chkUseReportDates.Visible = value;
            //lblReportDates.Visible = value;
        }

    }



    public bool ShowHideDate
    {
        set
        {
            //trHideDate.Visible = value;
        }

    }

    public bool ShowHideWidth
    {
        set
        {
            //trHideWidth.Visible = value;
        }

    }



    public bool ShowLegend
    {
        set
        {
            //trLegend.Visible = value;
        }

    }

    public bool ShowDisplay3d
    {
        set
        {
            //trDisplay3d.Visible = value;
        }

    }




    public string BackURL
    {
        get
        {
            return _strBackURL;
        }
        set
        {
            _strBackURL = value;
        }

    }


    public string ParentPage
    {
        get
        {
            return _strParentPage;
        }
        set
        {
            _strParentPage = value;
        }

    }

    public bool AvoidSize
    {
        get
        {
            return _bAvoidSize;
        }
        set
        {
            _bAvoidSize = value;
        }

    }


    public int? GraphOptionID
    {
        get
        {
            return _iGraphOptionID;
        }
        set
        {
            _iGraphOptionID = value;
        }

    }


    protected void chkShowLimitsOnGraphOne_CheckedChanged(object sender, EventArgs e)
    {
        _bToDateUp = null;
        _bOldDate = true;
        MakeChart();
    }

    protected void chkShowDottedLine_CheckedChanged(object sender, EventArgs e)
    {
        _bOldDate = true;
        MakeChart();
    }

    protected void chkShowLimits_Simple_CheckedChanged(object sender, EventArgs e)
    {
        //chkShowLimits_Simple.Checked = chkShowLimits.Checked;
        if (chkShowLimits_Simple.Checked)
        {
            //trWarning.Visible = true;
            //trExceedance.Visible = true;
            trWarning_Simple.Visible = true;
            trExceedance_Simple.Visible = true;

            //populate default values

//            foreach (GridViewRow row in gvTheGrid.Rows)
//            {
//                DropDownList ddlEachAnalyte = row.FindControl("ddlEachAnalyte") as DropDownList;
//                DropDownList ddlEachTable = row.FindControl("ddlEachTable") as DropDownList;
//                if (ddlEachAnalyte != null && ddlEachTable != null)
//                {
//                    DataTable dtTemp = Common.DataTableFromText(@"SELECT ShowGraphExceedance,ShowGraphWarning 
//                    FROM [Column] WHERE   TableID=" + ddlEachTable.SelectedValue.ToString() + " AND SystemName='" + ddlEachAnalyte.SelectedValue + "'");

//                    if (dtTemp.Rows.Count > 0)
//                    {
//                        bool bFound = false;

//                        if (dtTemp.Rows[0]["ShowGraphExceedance"] != null && dtTemp.Rows[0]["ShowGraphExceedance"].ToString() != "")
//                        {
//                            txtExceedanceValue.Text = double.Parse(dtTemp.Rows[0]["ShowGraphExceedance"].ToString()).ToString();
//                            txtExceedanceValue_Simple.Text = double.Parse(dtTemp.Rows[0]["ShowGraphExceedance"].ToString()).ToString();
//                            bFound = true;
//                        }
//                        if (dtTemp.Rows[0]["ShowGraphWarning"] != null && dtTemp.Rows[0]["ShowGraphWarning"].ToString() != "")
//                        {
//                            txtWarningValue.Text = double.Parse(dtTemp.Rows[0]["ShowGraphWarning"].ToString()).ToString();
//                            txtWarningValue_Simple.Text = double.Parse(dtTemp.Rows[0]["ShowGraphWarning"].ToString()).ToString();
//                            bFound = true;
//                        }
//                        if (bFound)
//                        {
//                            break;
//                        }
//                    }
//                }
//            }

        }
        else
        {
            //trWarning.Visible = false;
            //trExceedance.Visible = false;
            trWarning_Simple.Visible = false;
            trExceedance_Simple.Visible = false;
        }
        _bOldDate = true;
        MakeChart();
    }

    protected void ShowHideDatesReports(bool bDefaultDate)
    {


        Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);

        if (theDocument == null)
        {
            return;
        }

        //if (chkUseReportDates.Checked)
        //{
            //trStartDate.Visible = false;
            //trEndDate.Visible = false;
            if (theDocument.DocumentDate != null && theDocument.DocumentEndDate != null)
            {
                txtFromTime_Simple.Text = "";
                txtStartDate_Simple.Text = "";
                txtEndDate_Simple.Text = "";
                txtToTime_Simple.Text = "";

                txtStartDate_Simple.Text = theDocument.DocumentDate.Value.ToShortDateString();
                txtEndDate_Simple.Text = theDocument.DocumentEndDate.Value.ToShortDateString();
            }

        //}
        //else
        //{

            //trStartDate.Visible = true;
            //trEndDate.Visible = true;

            if (bDefaultDate)
            {
                txtFromTime_Simple.Text = "";
                txtToTime_Simple.Text = "";
                if (theDocument.DocumentDate != null && theDocument.DocumentEndDate != null)
                {
                    txtStartDate_Simple.Text = theDocument.DocumentDate.Value.ToShortDateString();
                    txtEndDate_Simple.Text = theDocument.DocumentEndDate.Value.ToShortDateString();
                }
            }


            if (txtStartDate_Simple.Text.Trim() == "" && ViewState["dtToDateTime"] != null)
            {
                txtStartDate_Simple.Text = ConvertUtil.GetDateString((DateTime)ViewState["dtFromDateTime"]);
                txtEndDate_Simple.Text = ConvertUtil.GetDateString((DateTime)ViewState["dtToDateTime"]);

                txtFromTime_Simple.Text = ((DateTime)ViewState["dtFromDateTime"]).Hour.ToString() + ":" + ((DateTime)ViewState["dtFromDateTime"]).Minute.ToString();
                txtToTime_Simple.Text = ((DateTime)ViewState["dtToDateTime"]).Hour.ToString() + ":" + ((DateTime)ViewState["dtToDateTime"]).Minute.ToString();

            }


        //}


    }

    private void SetDateTimeSelectorsState()
    {
        bool isReadOnly = ddlTimePeriodDisplay_Simple.SelectedValue != "C";
        bool isVisbile = DocumentID == null;

        if (isVisbile)
        {
            //trStartDate.Visible = true;
            //trEndDate.Visible = true;
            trStartDate_Simple.Visible = true;
            trEndDate_Simple.Visible = true;

            txtStartDate_Simple.Enabled = !isReadOnly;
            ibStartDate_Simple.Enabled = !isReadOnly;
            txtFromTime_Simple.Enabled = !isReadOnly;
            txtEndDate_Simple.Enabled = !isReadOnly;
            ibEndDate_Simple.Enabled = !isReadOnly;
            txtToTime_Simple.Enabled = !isReadOnly;
        }
        else
        {
            //trStartDate.Visible = false;
            //trEndDate.Visible = false;
            trStartDate_Simple.Visible = false;
            trEndDate_Simple.Visible = false;

            //txtStartDate.Text = "";
            //txtEndDate.Text = "";
            txtStartDate_Simple.Text = "";
            txtEndDate_Simple.Text = "";

            //txtFromTime.Text = "";
            //txtToTime.Text = "";
            txtFromTime_Simple.Text = "";
            txtToTime_Simple.Text = "";
        }
    }


    private void SetDateTimeSelectorsValue()
    {

        if (ViewState["dtFromDateTime"] != null)
        {
            //txtStartDate.Text = ConvertUtil.GetDateString((DateTime)ViewState["dtFromDateTime"]);
            txtStartDate_Simple.Text = ConvertUtil.GetDateString((DateTime)ViewState["dtFromDateTime"]);
            //txtFromTime.Text = ((DateTime)ViewState["dtFromDateTime"]).Hour.ToString() + ":" + ((DateTime)ViewState["dtFromDateTime"]).Minute.ToString();
            txtFromTime_Simple.Text = ((DateTime)ViewState["dtFromDateTime"]).Hour.ToString() + ":" + ((DateTime)ViewState["dtFromDateTime"]).Minute.ToString();
        }

        if (ViewState["dtToDateTime"] != null)
        { 
            //txtEndDate.Text = ConvertUtil.GetDateString((DateTime)ViewState["dtToDateTime"]);
            //txtToTime.Text = ((DateTime)ViewState["dtToDateTime"]).Hour.ToString() + ":" + ((DateTime)ViewState["dtToDateTime"]).Minute.ToString();
            txtEndDate_Simple.Text = ConvertUtil.GetDateString((DateTime)ViewState["dtToDateTime"]);
            txtToTime_Simple.Text = ((DateTime)ViewState["dtToDateTime"]).Hour.ToString() + ":" + ((DateTime)ViewState["dtToDateTime"]).Minute.ToString();
        }
    }


    private void GetDateTimeSelectorsValue()
    {
        if (txtStartDate_Simple.Text != "")
        {
            ViewState["dtFromDateTime"] =
                txtStartDate_Simple.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(
                txtFromTime_Simple.Text == "" ? txtStartDate_Simple.Text + " 00:00" : txtStartDate_Simple.Text + " " + txtFromTime_Simple.Text, "d/M/yyyy H:m",
                CultureInfo.InvariantCulture);
        }
        if (txtEndDate_Simple.Text != "")
        {
            ViewState["dtToDateTime"] =
                txtEndDate_Simple.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(
                txtToTime_Simple.Text == "" ? txtEndDate_Simple.Text + " 00:00" : txtEndDate_Simple.Text + " " + txtToTime_Simple.Text, "d/M/yyyy H:m",
                CultureInfo.InvariantCulture);
        }
    }


    protected void ibEmail_Click(object sender, ImageClickEventArgs e)
    {
        try
        {



            // create generic name
            string fileName = System.Guid.NewGuid().ToString() + ".pdf";
            string fileFullName = HttpContext.Current.Server.MapPath("~\\ExportedFiles") + "\\" + fileName;

            // create A4 landscape doc
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 200, -1, 100, -1);
            iTextSharp.text.pdf.PdfWriter writer = null;
            try
            {
                // specify the pdf file to create
                writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(fileFullName, FileMode.Create));

                // open pdf writer
                document.Open();

                // add an image
                //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance((byte[])HttpContext.Current.Session[guid]);
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance((byte[])HttpContext.Current.Session[ViewState["_strUniqueName"].ToString()]);

                img.ScalePercent(80);
                document.Add(img);

                int iSearchCriteriaID2 = -1;



                //SearchCriteria 
                try
                {

                    string xml = null;
                    xml = @"<root>" +
                         " <" + ddlEachAnalyte_Simple.ID + ">" + HttpUtility.HtmlEncode(ddlEachAnalyte_Simple.Text) + "</" + ddlEachAnalyte_Simple.ID + ">" +
                        //" <" + ddlGraphType.ID + ">" + HttpUtility.HtmlEncode(ddlGraphType.Text) + "</" + ddlGraphType.ID + ">" +
                       //" <" + chkShowDottedLine.ID + ">" + HttpUtility.HtmlEncode(chkShowDottedLine.Checked.ToString()) + "</" + chkShowDottedLine.ID + ">" +
                             " <" + txtFromTime_Simple.ID + ">" + HttpUtility.HtmlEncode(txtFromTime_Simple.Text) + "</" + txtFromTime_Simple.ID + ">" +
                           " <" + txtToTime_Simple.ID + ">" + HttpUtility.HtmlEncode(txtToTime_Simple.Text) + "</" + txtToTime_Simple.ID + ">" +
                        //" <" + txtLowestValue.ID + ">" + HttpUtility.HtmlEncode(txtLowestValue.Text) + "</" + txtLowestValue.ID + ">" +
                        //" <" + txtHighestValue.ID + ">" + HttpUtility.HtmlEncode(txtHighestValue.Text) + "</" + txtHighestValue.ID + ">" +
                          "</root>";
                    SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
                    iSearchCriteriaID2 = SystemData.SearchCriteria_Insert(theSearchCriteria);

                }
                catch
                {
                    //
                }

                if (ParentPage == "main")
                {
                    Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/SendEmail.aspx?Source=" + Cryptography.Encrypt("graph") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&FileName=" + Cryptography.Encrypt(fileName) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&SearchCriteriaID2=" + Cryptography.Encrypt(iSearchCriteriaID2.ToString()), false);
                }

                if (ParentPage == "list")
                {
                    if (Request.QueryString["GraphOptionID"] != null)
                    {

                        Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/SendEmail.aspx?Source=" + Cryptography.Encrypt("graphlist") +
                            "&FileName=" + Cryptography.Encrypt(fileName) + "&SearchCriteriaID2=" + Cryptography.Encrypt(iSearchCriteriaID2.ToString()) + "&page=list&mode=" + Request.QueryString["mode"].ToString() +
                        "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() +
                        "&GraphOptionID=" + Request.QueryString["GraphOptionID"].ToString(), false);
                    }
                    else
                    {
                        Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/SendEmail.aspx?Source=" + Cryptography.Encrypt("graphlist") +
                          "&FileName=" + Cryptography.Encrypt(fileName) + "&SearchCriteriaID2=" + Cryptography.Encrypt(iSearchCriteriaID2.ToString()) + "&page=list&mode=" + Request.QueryString["mode"].ToString() +
                      "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString(), false);

                    }
                }


            }
            catch (iTextSharp.text.DocumentException de)
            {
                DBGurus.AddErrorLog(de.Message);
            }
            catch (IOException ioe)
            {
                DBGurus.AddErrorLog(ioe.Message);
            }
            catch (Exception ex)
            {
                DBGurus.AddErrorLog(ex.Message);
                MakeNoDataChart();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Problem", "alert('There is no graph to email.');", true);
                return;
            }
            finally
            {
                try
                {
                    // close the document
                    document.Close();
                }
                catch (Exception ex)
                {
                    DBGurus.AddErrorLog(ex.Message);
                }
            }


            //MakeChart();

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Graph was sent to your email address.');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.IndexOf("Object reference not") > -1)
            {

                //lblMsg.Text = "No map image is available to send email.";
            }
            else
            {
                //lblMsg.Text = ex.Message + ex.StackTrace;
            }
            MakeNoDataChart();
        }

    }


    protected void chkCustomDate_CheckedChanged(Object sender, EventArgs args)
    {
        //ShowHideDatesReports(true);
        //ShowHideDatesCustom(true);
        _bOldDate = true;
        MakeChart();
    }

    //protected void chkHideDate_CheckedChanged(Object sender, EventArgs args)
    //{
    //    MakeChart();
    //}

    protected void chkUseReportDates_CheckedChanged(Object sender, EventArgs args)
    {

        ShowHideDatesReports(true);

        //if (chkUseReportDates.Checked)
        //{
        //    txtRecentDays.Enabled = true;
        //}
        //else
        //{
        //    txtRecentDays.Text = "";
        //    txtRecentDays.Enabled = false;

        //}

        MakeChart();

    }

    protected void lnkExportToPdf_Click(object sender, ImageClickEventArgs e)
    {

        //ExportToPDF("chart_ctl00_HomeContentPlaceHolder_WebChartViewer1", _qsTable.TableName + ".pdf");

        string strFileName = "chart";

        if (txtGraphTitle_Simple.Text != "")
            strFileName = txtGraphTitle_Simple.Text;

        ExportToPDF(ViewState["_strUniqueName"].ToString(), strFileName + ".pdf");

        //if (ViewState["GraphImageSessionID"] != null
        //    && ViewState["GraphTitle"] != null)
        //{
        //    ExportToPDF(ViewState["GraphImageSessionID"].ToString(), String.Concat(ViewState["GraphTitle"].ToString().Replace(":", "-"), ".pdf"));
        //}
        //else ViewState["_strUniqueName"]
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CANNOT_EXPORT_GRAPH",
        //        "alert('Cannot export graph');", true);
        //}

    }

    public static void CleanUpTempFiles(string tempPath, string extension, int lifeTime)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(tempPath));
        foreach (FileInfo fileInfo in dirInfo.GetFiles())
        {
            try
            {
                if (fileInfo.Extension.ToLower() == extension.ToLower()
                    && fileInfo.LastWriteTime.AddSeconds(lifeTime) < DateTime.Now)
                {
                    // delete the file
                    File.Delete(fileInfo.FullName);
                }
            }
            catch
            {
                // do nothing
            }
        }
    }

    public void ExportToPDF(string guid, string fileNameToDownload)
    {
        // cleanup created temp pdf

        CleanUpTempFiles(_tempDirectory, ".pdf", _tempFileLifeTime);

        // create generic name
        string fileName = System.Guid.NewGuid().ToString() + ".pdf";
        string fileFullName = HttpContext.Current.Server.MapPath(_tempDirectory) + fileName;

        // create A4 landscape doc
        iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 200, -1, 100, -1);
        iTextSharp.text.pdf.PdfWriter writer = null;
        try
        {
            // specify the pdf file to create
            writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(fileFullName, FileMode.Create));

            // open pdf writer
            document.Open();

            // add an image
            //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance((byte[])HttpContext.Current.Session[guid]);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance((byte[])HttpContext.Current.Session[guid]);

            img.ScalePercent(80);
            document.Add(img);

        }
        catch (iTextSharp.text.DocumentException de)
        {
            DBGurus.AddErrorLog(de.Message);
        }
        catch (IOException ioe)
        {
            DBGurus.AddErrorLog(ioe.Message);
        }
        catch (Exception ex)
        {
            DBGurus.AddErrorLog(ex.Message);
            MakeNoDataChart();

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Problem", "alert('There is no graph to export.');", true);

            return;
        }
        finally
        {
            try
            {
                // close the document
                document.Close();
            }
            catch (Exception ex)
            {
                DBGurus.AddErrorLog(ex.Message);
            }
        }

        // download newly created doc file
        DownloadFile(fileFullName, fileNameToDownload);
    }


    #region "DownloadFile"
    /// <summary>
    /// Download Graph export file
    /// </summary>
    /// <param name="fileSource"></param>
    /// <param name="fileName"></param>
    public static void DownloadFile(string fileSource, string fileName)
    {
        HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM";
        string disHeader;
        disHeader = "Attachment; Filename=\"" + fileName + "\"";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", disHeader);
        System.IO.FileInfo fileToDownload;
        fileToDownload = new System.IO.FileInfo(fileSource);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.WriteFile(fileToDownload.FullName);
        HttpContext.Current.Response.End();
    }
    #endregion


    protected void MakeChart()
    {
        try
        {
            //bool _bForRecentdays = false;

            if (!IsPostBack && _gGraphOption == null && OneTable.GraphOnStart != "" && OneTable.GraphOnStart == "EmptyGraph")
            {
                MakeNoGraphChart();
                return;
            }


            _bGotFirstDate = false;
            _bWarningExceedShown = false;

            bool bReducedRecords = false;
            string strMeanTime = "hourly";
            try
            {
                //Size


                //if (ParentPage != "home")
                //{
                //if (txtPlotAreaWidth.Text.Trim() != "")
                //{
                //    if (AvoidSize == false)
                //        ChartWidth = int.Parse(txtPlotAreaWidth.Text.Trim());
                //}
                //if (txtPlotAreaHeight.Text.Trim() != "")
                //{
                //    if (AvoidSize == false)
                //        ChartHeight = int.Parse(txtPlotAreaHeight.Text.Trim());
                //}
                //}

                if (_gGraphOption == null && GraphOptionID != null)
                {
                    _gGraphOption = GraphManager.ets_GraphOption_Detail((int)GraphOptionID);
                    
                }
                if (_gGraphOption!=null)
                {
                    trSavedGraph.Visible = true;
                    lblSavedGraphName.Text = _gGraphOption.Heading;
                   
                }
                else
                {
                    trSavedGraph.Visible = false;
                    lblSavedGraphName.Text = "";
                }

                //if (ddlTimePeriodDisplay.SelectedValue == "C")
                //{
                //    if (String.IsNullOrEmpty(txtStartDate.Text) && (ViewState["dtFromDateTime"] != null))
                //    {
                //        ViewState["dtFromDateTime"] = null;
                //    }
                //    if (String.IsNullOrEmpty(txtEndDate.Text) && (ViewState["dtToDateTime"] != null))
                //    {
                //        ViewState["dtToDateTime"] = null;
                //    }
                //}

                ////Dates


                MakeDate();

                if (DocumentID != null)
                {


                    //if (chkUseReportDates.Checked == true)
                    //{
                        Document theDocument = DocumentManager.ets_Document_Detail((int)DocumentID);

                        if (theDocument != null && txtRecentNumber.Text!="")
                        {
                            if (theDocument.ForDashBoard != null)
                            {
                                if ((bool)theDocument.ForDashBoard)
                                {

                                    try
                                    {
                                        ddlTimePeriodDisplay_Simple.SelectedValue = "C";
                                        //ddlTimePeriod.Text = "C";
                                        DateTime dtTo = DateTime.Now;
                                        DateTime dtFrom = DateTime.Now;
                                        switch (ddlRecentPeriod.SelectedValue)
                                        {
                                            case "Y":
                                                dtFrom = (DateTime)dtTo.AddYears(-int.Parse(txtRecentNumber.Text));
                                                break;
                                            case "M":
                                                dtFrom = (DateTime)dtTo.AddMonths(-int.Parse(txtRecentNumber.Text));
                                                break;
                                            case "W":
                                                dtFrom = (DateTime)dtTo.AddDays(-7 * int.Parse(txtRecentNumber.Text));
                                                break;
                                            case "D":
                                                dtFrom = (DateTime)dtTo.AddDays(-int.Parse(txtRecentNumber.Text));
                                                break;
                                            case "H":
                                                dtFrom = (DateTime)dtTo.AddHours(-int.Parse(txtRecentNumber.Text));
                                                break;

                                        }
                                        ViewState["dtToDateTime"] = dtTo;
                                        ViewState["dtFromDateTime"] = dtFrom;
                                        ViewState["C_dtToDateTime"] = ViewState["dtToDateTime"];
                                        ViewState["C_dtFromDateTime"] = ViewState["dtFromDateTime"];
                                    }
                                    catch
                                    {
                                        //
                                    }




                                    //if (ddlTimePeriodDisplay.SelectedValue == "C")
                                    //{
                                    //    if (txtRecentDays.Text.Trim() != "" && ViewState["GraphOptionDetail"] != null)
                                    //    {
                                    //        string strSTIDs = "";

                                    //        DataTable dtTempGD = ((DataTable)ViewState["GraphOptionDetail"]);

                                    //        foreach (DataRow drST in dtTempGD.Rows)
                                    //        {
                                    //            strSTIDs = strSTIDs + drST["TableID"].ToString() + ",";
                                    //        }

                                    //        if (strSTIDs != "")
                                    //        {
                                    //            strSTIDs = strSTIDs + "-1";
                                    //            DateTime dtTo = DateTime.Now;
                                    //            DateTime dtFrom = DateTime.Now;
                                    //            DataTable dtTemp = Common.DataTableFromText("SELECT MAX(DateTimeRecorded) FROM Record WHERE IsActive=1 AND TableID IN (" + strSTIDs + ")");

                                    //            if (dtTemp.Rows.Count > 0)
                                    //            {
                                    //                if (dtTemp.Rows[0][0] != DBNull.Value)
                                    //                {
                                    //                    dtTo = (DateTime)dtTemp.Rows[0][0];
                                    //                    dtFrom = ((DateTime)dtTemp.Rows[0][0]).AddDays(-int.Parse(txtRecentDays.Text.Trim()));
                                    //                }

                                    //            }
                                    //            ////_bForRecentdays = true;
                                    //            ViewState["dtToDateTime"] = dtTo;
                                    //            ViewState["dtFromDateTime"] = dtFrom;
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                        }
                    //}

                    //if (chkUseReportDates.Checked == false)
                    //{
                    //    if (txtStartDate.Text != "")
                    //    {
                    //        ViewState["dtFromDateTime"] = txtStartDate.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtFromTime.Text == "" ? txtStartDate.Text + " 00:00" : txtStartDate.Text + " " + txtFromTime.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                    //        ViewState["dtToDateTime"] = txtEndDate.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtToTime.Text == "" ? txtEndDate.Text + " 00:00" : txtEndDate.Text + " " + txtToTime.Text, "d/M/yyyy H:m", CultureInfo.InvariantCulture);
                    //    }
                    //}
                }
                else
                {
                    //if (ddlTimePeriodDisplay.SelectedValue == "C")// && ParentPage != "home"
                    //{
                    //    if (txtStartDate.Text != "")
                    //    {
                    //        ViewState["dtFromDateTime"] =
                    //            txtStartDate.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(
                    //            txtFromTime.Text == "" ? txtStartDate.Text + " 00:00" : txtStartDate.Text + " " + txtFromTime.Text, "d/M/yyyy H:m",
                    //            CultureInfo.InvariantCulture);
                    //    }
                    //    if (txtEndDate.Text != "")
                    //    {
                    //        ViewState["dtToDateTime"] =
                    //            txtEndDate.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(
                    //            txtToTime.Text == "" ? txtEndDate.Text + " 00:00" : txtEndDate.Text + " " + txtToTime.Text, "d/M/yyyy H:m",
                    //            CultureInfo.InvariantCulture);
                    //    }
                    //}
                }

                //

                if (IsDashBorad)
                {
                    if (_theChartDashBoard != null)
                    {
                        if (string.IsNullOrEmpty(_theChartDashBoard.RecentPeriod)==false)
                        {
                            if (!IsPostBack)
                            {
                                try
                                {
                                    ddlTimePeriodDisplay_Simple.SelectedValue = "C";
                                    //ddlTimePeriod.Text = "C";
                                    DateTime dtTo = DateTime.Now;
                                    DateTime dtFrom = DateTime.Now;
                                    switch (_theChartDashBoard.RecentPeriod)
                                      {
                                        case "Y":
                                              dtFrom = (DateTime)dtTo.AddYears(-(int)_theChartDashBoard.RecentNumber);
                                            break;
                                        case "M":
                                              dtFrom = (DateTime)dtTo.AddMonths(-(int)_theChartDashBoard.RecentNumber);
                                            break;
                                        case "W":
                                            dtFrom = (DateTime)dtTo.AddDays(-7*(int)_theChartDashBoard.RecentNumber);
                                            break;
                                        case "D":
                                            dtFrom = (DateTime)dtTo.AddDays(-(int)_theChartDashBoard.RecentNumber);
                                            break;
                                        case "H":
                                            dtFrom = (DateTime)dtTo.AddHours(-(int)_theChartDashBoard.RecentNumber);
                                            break;
                                                         
                                      }
                                    ViewState["dtToDateTime"] = dtTo;
                                    ViewState["dtFromDateTime"] = dtFrom;
                                    ViewState["C_dtToDateTime"] = ViewState["dtToDateTime"];
                                    ViewState["C_dtFromDateTime"] = ViewState["dtFromDateTime"];

                                    //if (ddlTimePeriodDisplay.SelectedValue == "C")
                                    //{
                                    
                                        //if (RecentDays != "" && ViewState["GraphOptionDetail"] != null)
                                        //{
                                        //    string strSTIDs = "";

                                        //    DataTable dtTempGD = ((DataTable)ViewState["GraphOptionDetail"]);

                                        //    foreach (DataRow drST in dtTempGD.Rows)
                                        //    {
                                        //        strSTIDs = strSTIDs + drST["TableID"].ToString() + ",";
                                        //    }

                                        //    if (strSTIDs != "")
                                        //    {
                                        //        strSTIDs = strSTIDs + "-1";
                                        //        DateTime dtTo = DateTime.Now;
                                        //        DateTime dtFrom = DateTime.Now;
                                        //        DataTable dtTemp = Common.DataTableFromText("SELECT MAX(DateTimeRecorded) FROM Record WHERE IsActive=1 AND TableID IN (" + strSTIDs + ")");

                                        //        if (dtTemp.Rows.Count > 0)
                                        //        {
                                        //            if (dtTemp.Rows[0][0] != DBNull.Value)
                                        //            {
                                        //                dtTo = (DateTime)dtTemp.Rows[0][0];
                                        //                dtFrom = ((DateTime)dtTemp.Rows[0][0]).AddDays(-int.Parse(RecentDays));
                                        //            }

                                        //        }
                                        //        //_bForRecentdays = true;
                                        //        ViewState["dtToDateTime"] = dtTo;
                                        //        ViewState["dtFromDateTime"] = dtFrom;
                                        //    }
                                        //}
                                    //}


                                }
                                catch
                                {
                                    //
                                }

                            }
                        }
                    }

                }

                String strError = "";

                ViewState["_strUniqueName"] = Guid.NewGuid().ToString();


                string strTableID = ddlGraphOption.SelectedValue;
                string strColumnSystemName = ddlEachAnalyte_Simple.SelectedValue;
                string strColumnGraphLabel = "";

                if (ddlEachAnalyte_Simple.SelectedItem != null)
                    strColumnGraphLabel = ddlEachAnalyte_Simple.SelectedItem.Text;


                //hlMoreGraphAlt.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/RecordChart.aspx?SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt(ddlGraphOption.SelectedValue) + "&fromhome=yes";
                //hlMoreGraph.NavigateUrl = hlMoreGraphAlt.NavigateUrl;

                string strGraphPage = "home"; //"home"

                if (_theAccount.Layout == 1)
                {
                    strGraphPage = "chartalt"; //"home"
                    if (ParentPage == "home")
                    {
                        //hlMoreGraphAlt.Visible = true;
                        //hlMoreGraph.Visible = false;
                        if (IsDashBorad)
                        {
                            lnkZoom.Visible = false;
                        }
                    }
                }
                else
                {
                    if (ParentPage == "home")
                    {
                        //hlMoreGraphAlt.Visible = false;
                        //hlMoreGraph.Visible = true;
                        if (IsDashBorad)
                        {
                            lnkZoom.Visible = false;
                        }
                    }
                }

                bool bLeft = true;

                bool bPercent = false;

                HashSet<String> hashLeftCaption = new HashSet<string>();
                HashSet<String> hashRightCaption = new HashSet<string>();

                string sPeriod = "Hour";

                switch (ddlTimePeriodDisplay_Simple.Text)
                {
                    case "H":
                        sPeriod = "Hour";
                        break;
                    case "D":
                        sPeriod = "Day";
                        break;
                    case "W":
                        sPeriod = "Week";
                        break;
                    case "M":
                        sPeriod = "Month";
                        break;
                    case "Y":
                        sPeriod = "Year";
                        break;
                }


                if (ddlTimePeriodDisplay_Simple.SelectedValue == "C")
                {
                    sPeriod = "Hour";
                }

                bool bHideDateInX = false;


                bool? bHideDateInXAxis = null;
                if (bHideDateInXAxis != null)
                    bHideDateInX = (bool)bHideDateInXAxis;


                //if (dtFromDateTime == null)
                //    dtFromDateTime = DateTime.Now.AddYears(-1);

                //if (dtToDateTime == null)
                //    dtToDateTime = DateTime.Now;



                int iTN = 0;


                CSVFormatter csf = new CSVFormatter();

                DateTime[] labels;
                Dictionary<DateTime, int> dcDateTimeRecorded = new Dictionary<DateTime, int>();

                bool isCol0DateTime = true;

                if (ddlGraphOption.SelectedValue.IndexOf("-") > -1 || ddlGraphOption.SelectedValue == "" || strTableID != "")
                {

                    //Graph options
                    DataTable dtRecord = null;

                    if (Mode == "edit")
                    {

                        if (_gGraphOption == null)
                        {
                            if (ddlGraphOption.SelectedValue.IndexOf("-") > -1)
                            {
                                GraphOptionID = int.Parse(ddlGraphOption.SelectedValue.Replace("-", ""));
                                _gGraphOption = GraphManager.ets_GraphOption_Detail((int)GraphOptionID);
                            }
                        }
                        if (_gGraphOption != null)
                        {
                            if (ViewState["GraphOptionDetail"] != null)
                            {
                                dtRecord = (DataTable)ViewState["GraphOptionDetail"];
                            }
//                            else
//                            {
//                                //for report
//                                ViewState["GraphOptionDetail"] = Common.DataTableFromText(@"SELECT     GraphOptionDetail.GraphOptionDetailID, GraphOptionDetail.GraphOptionID, GraphOptionDetail.TableID, GraphOptionDetail.ColumnID, 
//                      GraphOptionDetail.Label, GraphOptionDetail.Axis, GraphOptionDetail.GraphOrder, GraphOptionDetail.Scale, GraphOptionDetail.GraphType, GraphOptionDetail.Colour, 
//                       GraphOptionDetail.High, GraphOptionDetail.Low, [Table].TableName, 
//                      [Column].SystemName, [Column].DisplayName, [Column].GraphLabel
//                        FROM         GraphOptionDetail INNER JOIN
//                      [Column] ON GraphOptionDetail.ColumnID = [Column].ColumnID INNER JOIN
//                      [Table] ON GraphOptionDetail.TableID = [Table].TableID AND [Column].TableID = [Table].TableID 
//                      WHERE GraphOptionDetail.GraphOptionID=" + _gGraphOption.GraphOptionID.ToString());
//                                dtRecord = (DataTable)ViewState["GraphOptionDetail"];
//                            }
                        }
                    }
                    else
                    {
                        dtRecord = (DataTable)ViewState["GraphOptionDetail"];
                    }
                    if (dtRecord == null && ViewState["GraphOptionDetail"] != null)
                    {
                        dtRecord = (DataTable)ViewState["GraphOptionDetail"];
                    }

                    int iChartCount = 0;

                    int iWE = 0;

                    //if (showWarnings.Value == "1")
                    //    iWE = iWE + 1;

                    //if (showErrors.Value == "1")
                    //    iWE = iWE + 1;

                    dtYDT=new DataTable[dtRecord.Rows.Count + 2];


                    foreach (DataRow drEachST in dtRecord.Rows)
                    {
                        if (drEachST["ColumnID"] == DBNull.Value)
                        {
                            continue;
                        }

                        // one chart
                        iChartCount = iChartCount + 1;
                        //GraphOptionDetail theGraphOptionDetail = GraphManager.ets_GraphOptionDetail_Detail(int.Parse(drEachST["GraphOptionDetailID"].ToString()), null, null);

                        Column theColumn = RecordManager.ets_Column_Details(int.Parse(drEachST["ColumnID"].ToString()));

                        strColumnSystemName = theColumn.SystemName;
                        strColumnGraphLabel = theColumn.GraphLabel;
                        string strGraphType = "";
                        if (drEachST["GraphType"] != DBNull.Value && drEachST["GraphType"].ToString() != "")
                        {
                            strGraphType = drEachST["GraphType"].ToString();
                        }

                        bLeft = true;
                        bPercent = false;
                        if (drEachST["Axis"] != DBNull.Value && drEachST["Axis"].ToString() != "")
                        {
                            if (drEachST["Axis"].ToString() == "Right")
                                bLeft = false;
                            //if (theGraphOptionDetail.Heading != "")
                            //    strTitle = theGraphOptionDetail.Heading;

                            if (drEachST["Axis"].ToString() == "Percentage")
                                bPercent = true;
                        }


                        string strGraphSeriesColumnName = drEachST["GraphSeriesColumnID"].ToString();
                        string strGraphSeriesValue = drEachST["GraphSeriesID"].ToString();


                        //


                        //Table _qsTable = RecordManager.ets_Table_Details((int)theGraphOptionDetail.TableID);

                        List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(drEachST["TableID"].ToString()),
                                null, null, ref iTN);

                        Column ssColumn = new Column();
                        Column dtColumn = new Column();

                        string graphXAxisColumnName = GetGraphXAxisColumnName(drEachST["TableID"].ToString());

                       
                        foreach (Column eachColumn in lstColumns)
                        {

                            //if (eachColumn.IsStandard == true)
                            //{
                            //    switch (eachColumn.SystemName.ToLower())
                            //    {
                            //        case "datetimerecorded":
                            //            dtColumn = eachColumn;
                            //            break;

                            //        //case "locationid":
                            //        //    ssColumn = eachColumn;
                            //        //    break;
                            //        default:
                            //            break;
                            //    }
                            //}

                            if (eachColumn.SystemName == graphXAxisColumnName)
                            {
                                dtColumn = eachColumn;

                                if (_strXAxisLabel == "")
                                    _strXAxisLabel = eachColumn.GraphLabel == "" ? eachColumn.DisplayName : eachColumn.GraphLabel;
                            }

                        }

                        if (iChartCount == 1)
                        {
                            string strXSummary = dtColumn.GraphLabel;
                            csf.FirstColumnLabel = strXSummary;
                        }


                        //DataTable dtYdata;
                        int definitionID = 0;
                        if (!String.IsNullOrEmpty(ddlGraphType_Simple.SelectedValue))
                            definitionID = int.Parse(ddlGraphType_Simple.SelectedValue);
                        else
                            if ((_gGraphOption != null) && _gGraphOption.GraphDefinitionID.HasValue)
                                definitionID = _gGraphOption.GraphDefinitionID.Value;
                        GraphDefinition graphDef = GraphManager.ets_GraphDefinition_Detail(definitionID);

                        if (strGraphType.ToLower() == "mmm")
                        {
                            dtYdata = RecordManager.ets_Chart_GetData_MeanMaxMin(int.Parse(drEachST["TableID"].ToString()),
                                dtColumn.SystemName, strColumnSystemName, (DateTime)ViewState["dtFromDateTime"],
                                ((DateTime)ViewState["dtToDateTime"]), sPeriod, Common.MaxGraphRecords);
                        }
                        else
                        {
                            string spName = "ets_Chart_GetData_New";
                            if ((graphDef != null) && !String.IsNullOrEmpty(graphDef.DefinitionKey))
                                spName = graphDef.DefinitionKey;

                            //bool forceAverage = (SampleSitesList.Items.Count > 1) && (SampleSitesList.GetSelectedIndices().Count() == 0);


                            bool forceAverage = (SampleSitesList.Items.Count > 1) && (SampleSitesList.SelectedItem == null);

                            if ((DocumentID != null) || (ParentPage.ToLower() == "home"))
                            {
                                if (String.IsNullOrEmpty(strGraphSeriesValue))
                                    forceAverage = true;
                            }
                            dtYdata = RecordManager.ets_Chart_GetData_New(spName,
                                int.Parse(drEachST["TableID"].ToString()),
                                dtColumn.SystemName, strColumnSystemName,
                                (DateTime)ViewState["dtFromDateTime"], ((DateTime)ViewState["dtToDateTime"]) ,
                                strGraphSeriesColumnName, strGraphSeriesValue, forceAverage,
                                graphDef == null ? null : graphDef.DataColumn1ID,
                                graphDef == null ? null : graphDef.DataColumn2ID,
                                graphDef == null ? null : graphDef.DataColumn3ID,
                                graphDef == null ? null : graphDef.DataColumn4ID,
                                sPeriod, bPercent, /*Common.MaxGraphRecords*/ null);
                        }

                        //if(dtYdata!=null && dtYdata.Rows.Count>0)
                        //{

                        //    bool bHasHigestDate = false;

                        //    if (DateTime.Parse(dtYdata.Rows[0][0].ToString()) == (DateTime)ViewState["dtToDateTime"])
                        //    {
                        //        bHasHigestDate = true;
                        //        dtYdata.Rows[0][0] = ((DateTime)ViewState["dtToDateTime"]);
                        //        dtYdata.Rows[0][1] = DBNull.Value;
                        //        dtYdata.AcceptChanges();
                        //    }


                        //    if(bHasHigestDate==false)
                        //    {
                        //        DataRow dr0;
                        //        dr0 = dtYdata.NewRow();
                        //        dr0[0] = ((DateTime)ViewState["dtToDateTime"]) ;
                        //        dr0[1] = DBNull.Value;

                        //        dtYdata.Rows.InsertAt(dr0, 0);
                        //        dtYdata.AcceptChanges();
                        //    }

                        //    bool bHasLowestDate = false;

                        //    if (DateTime.Parse(dtYdata.Rows[dtYdata.Rows.Count - 1][0].ToString()) == (DateTime)ViewState["dtFromDateTime"])
                        //    {
                        //        bHasLowestDate = true;
                        //    }

                        //    if(bHasLowestDate==false)
                        //    {
                        //        DataRow drL;
                        //        drL = dtYdata.NewRow();
                        //        drL[0] = (DateTime)ViewState["dtFromDateTime"];
                        //        drL[1] = DBNull.Value;

                        //        dtYdata.Rows.Add(drL);
                        //        dtYdata.AcceptChanges();
                        //    }
                            
                        //}


                        /*
                        int m = 0;

                        foreach (DataRow drow in dtYdata.Rows)
                        {

                            if (!dcDateTimeRecorded.ContainsKey(DateTime.Parse(Convert.ToDateTime(drow[dtColumn.SystemName].ToString()).ToString("d/M/yyyy HH:m"))))
                                dcDateTimeRecorded.Add(DateTime.Parse(Convert.ToDateTime(drow[dtColumn.SystemName].ToString()).ToString("d/M/yyyy HH:m")), m);

                            m = m + 1;
                        }


                        DateTime[] dtDateTimeRecordedA = new DateTime[dcDateTimeRecorded.Keys.Count];

                        dcDateTimeRecorded.Keys.CopyTo(dtDateTimeRecordedA, 0);
                        List<DateTime> strTempList = new List<DateTime>(dcDateTimeRecorded.Keys);
                        strTempList.Sort();

                        labels = strTempList.ToArray();

                        //string[] strLocations = new string[hSSs.Keys.Count];
                        //hSSs.Keys.CopyTo(strLocations, 0);
                        
                        string[] lblDateTime = new string[labels.Length];
                        */

                        string[] lblDateTime = new string[0];
                        labels = new DateTime[0];

                        //if (drEachST["LocationID"] != DBNull.Value)
                        //{
                        //    strLocations = new string[1];
                        //    strLocations[0] = drEachST["LocationID"].ToString();
                        //}

                        string strColor = "";

                        if (drEachST["Colour"] != DBNull.Value)
                        {
                            strColor = drEachST["Colour"].ToString();
                        }

                        double? dLow = null;
                        double? dHigh = null;
                        if (drEachST["Low"] != DBNull.Value)
                            dLow = double.Parse(drEachST["Low"].ToString());

                        if (drEachST["High"] != DBNull.Value)
                            dHigh = double.Parse(drEachST["High"].ToString());

                        string strLabel = "-1";

                        if (drEachST["GraphLabel"] != DBNull.Value)
                            strLabel = drEachST["GraphLabel"].ToString();

                        string strSeriesLabel = String.Empty;
                        if (drEachST["GraphSeriesID"] != DBNull.Value)
                            strSeriesLabel = drEachST["GraphSeriesID"].ToString();

                        if (SampleSitesList.Items.FindByValue(strSeriesLabel) != null)
                            strSeriesLabel = SampleSitesList.Items.FindByValue(strSeriesLabel).Text;

                        if (strSeriesLabel!="")
                        {
                             dtYdata.TableName = strSeriesLabel;
                        }
                        else
                        {
                            dtYdata.TableName = "Average";
                        }
                           
                        //MakeOneChart(ref dcDateTimeRecorded, ref labels, ref lblDateTime, (DateTime)ViewState["dtFromDateTime"],
                        //    (DateTime)ViewState["dtToDateTime"], ref c, sPeriod,
                        //    dtYdata, theColumn, ssColumn,
                        //    bLeft, hashRightCaption, hashLeftCaption, strGraphType, ref _iNoOfSeries,
                        //    strColor, strSeriesLabel, bPercent, dLow, dHigh, strLabel, ref layerStackedBar, ref layerBar);

                        if ((graphDef != null) && !String.IsNullOrEmpty(graphDef.DefinitionKey))
                            csf.AddAllSeries(dtYdata, dtColumn.SystemName, theColumn.SystemName, strSeriesLabel);
                        else
                            csf.AddSeries(dtYdata, dtColumn.SystemName, theColumn.SystemName, strSeriesLabel);

                        isCol0DateTime = isCol0DateTime && (dtYdata.Columns[0].DataType == typeof(DateTime));


                        dtYDT[iChartCount-1] = dtYdata;
                    }//end drEachST

                    CSVData.Text = csf.CSV;
                }


                DateTime[] dtDateTimeRecordedA2 = new DateTime[dcDateTimeRecorded.Keys.Count];

                dcDateTimeRecorded.Keys.CopyTo(dtDateTimeRecordedA2, 0);
                List<DateTime> strTempList2 = new List<DateTime>(dcDateTimeRecorded.Keys);
                strTempList2.Sort();

                labels = strTempList2.ToArray();

                dcDateTimeRecorded.Keys.CopyTo(dtDateTimeRecordedA2, 0);
                List<DateTime> strTempListDate = new List<DateTime>(dcDateTimeRecorded.Keys);
                strTempListDate.Sort();

                labels = strTempListDate.ToArray();
                int p = 0;

                string[] lblDateTime2 = new string[labels.Length];
                foreach (DateTime dtTemp in labels)
                {
                    lblDateTime2[p] = Convert.ToDateTime(labels[p].ToString()).ToString("d/M/yyyy HH:m");
                    p = p + 1;
                }

                int iPlotAreaWidth = (ChartWidth / 6) * 2;
                int iPlotAreaHeight = (ChartHeight / 6) * 3;

                int iPlotX = (ChartWidth / 6);
                int iPlotY = (ChartHeight / 6);

                //switch (rblLegendPosition.SelectedValue.ToLower())
                //{

                //    case "top":

                //        iPlotY = (ChartHeight / 6) + 50;

                //        iPlotAreaWidth = (ChartWidth / 6) * 4;

                //        if (ChartWidth > 800)
                //        {
                //            iPlotAreaWidth = ((ChartWidth / 6) * 4) + 80;
                //            iPlotX = (ChartWidth / 8);
                //        }
                //        if (ChartHeight > 600)
                //        {
                //            iPlotAreaHeight = (ChartHeight / 6) * 4 - 30;
                //            iPlotY = (ChartHeight / 7) + 40;
                //        }

                //        if (ParentPage == "home" && _theAccount.Layout != null && _theAccount.Layout == 0)
                //        {
                //            if (IsDashBorad == false)
                //            {
                //                if (ChartHeight < 500)
                //                {
                //                    iPlotAreaHeight = 200;
                //                }
                //            }

                //        }

                //        //c.addLegend2(iPlotX, 30, 3, "Verdana Bold", 9).setBackground(Chart.Transparent);
                //        if (_iNoOfSeries > 0)
                //        {
                //            if (ParentPage == "home" && _theAccount.Layout != null && _theAccount.Layout == 0 && ChartHeight < 500)
                //            {
                //                if (IsDashBorad == false)
                //                {
                //                }
                //                else
                //                {
                //                }
                //            }
                //            else
                //            {
                //            }
                //        }

                //        break;
                //}

                string strLeftCaption = string.Empty;
                foreach (string s in hashLeftCaption)
                    strLeftCaption = strLeftCaption + s + " & ";
                if (!String.IsNullOrEmpty(strLeftCaption))
                    strLeftCaption = strLeftCaption.Substring(0, strLeftCaption.Length - 3);
                string strRightCaption = string.Empty;
                foreach (string s in hashRightCaption)
                    strRightCaption = strRightCaption + s + " & ";
                if (!String.IsNullOrEmpty(strRightCaption))
                    strRightCaption = strRightCaption.Substring(0, strRightCaption.Length - 3);

                ////

                if (strError.Trim() != "")
                {
                    MakeNoDataChart();
                }

                int patchID = 0;

                if (isCol0DateTime)
                {
                    switch (sPeriod)
                    {
                        case "Hour":
                            break;
                        case "Day":
                            patchID = 1;
                            break;
                        case "Week":
                            patchID = 1;
                            break;
                        case "Month":
                            patchID = 1;
                            break;
                        case "Year":
                            patchID = 1;
                            break;
                    }
                }

                SetChartObject(patchID);
                SetChartElements();
            }
            catch (Exception ex)
            {
                //throw;
                MakeNoDataChart();
            }


            //if (ddlTimePeriodDisplay.SelectedValue == "C" && bReducedRecords==true)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Problem", "alert('The date range you have selected has resulted in too many data points to show on a graph.  The " + strMeanTime + " averages have been used instead.');", true);
            //}

            _bGotFirstDate = false;


            if(SampleSitesList.SelectedItem!=null)
            {
                CheckBoxList tempCBL = new CheckBoxList();
                //SampleSitesList.CopyBaseAttributes(tempCBL);

                foreach (ListItem li in SampleSitesList.Items)
                {
                    tempCBL.Items.Add(li);
                }

                SampleSitesList.Items.Clear();

                foreach(ListItem li in tempCBL.Items)
                {
                    if(li.Selected)
                    {
                        if (SampleSitesList.Items.FindByValue(li.Value) == null)
                            SampleSitesList.Items.Add(new ListItem(li.Text, li.Value));
                    }
                }
                foreach (ListItem li in SampleSitesList.Items)
                {
                    li.Selected = true;
                }
                foreach (ListItem li in tempCBL.Items)
                {
                    if (li.Selected==false)
                    {
                        if (SampleSitesList.Items.FindByValue(li.Value) == null)
                            SampleSitesList.Items.Add(new ListItem(li.Text, li.Value));
                    }
                }

            }


        }
        catch
        {
            //
        }

    }

    private void SetChartObject(int patchID)
    {
        WebChartViewer1.Style.Add("width", this.ChartWidth.ToString() + "px");
        WebChartViewer1.Style.Add("height", this.ChartHeight.ToString() + "px");

        int definitionID = 0;
        if (!String.IsNullOrEmpty(ddlGraphType_Simple.SelectedValue))
            definitionID = int.Parse(ddlGraphType_Simple.SelectedValue);
        else
            if ((_gGraphOption != null) && _gGraphOption.GraphDefinitionID.HasValue)
                definitionID = _gGraphOption.GraphDefinitionID.Value;

        if (definitionID != 0)
        {
            string script = String.Empty;
            if (definitionID > 0)
            {
                ClearUseCustomScript();
                GraphDefinition definition = GraphManager.ets_GraphDefinition_Detail(definitionID);
                if (definition != null)
                {
                    if (!String.IsNullOrEmpty(definition.Definition))
                    {
                        editableHCS.Value = definition.Definition;
                        script = definition.Definition.Replace("&lt;", "<").Replace("&gt;", ">");
                    }
                    else
                    {
                        MakeNoDataChart();
                    }
                }
                else
                {
                    MakeNoDataChart();
                }
            }
            else
            {
                script = editableHCS.Value.Replace("&lt;", "<").Replace("&gt;", ">");
            }

            if (!String.IsNullOrEmpty(script))
            {
                string preScript =
                        @"$(function () {
                            Highcharts.setOptions({
                                plotOptions: {
                                    series: {
                                        connectNulls: true
                                    }
                                },";
                if ((DocumentID != null) || (ParentPage.ToLower() == "home"))
                {
                    preScript +=
                              @"navigation: {
                                    buttonOptions: {
                                        enabled: false
                                    }
                                },";
                }
                preScript +=
                          @"})
                        });";

                script = preScript +Environment.NewLine + script;
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "highchart_script", script, true);

                string strJS = string.Empty;
                switch (patchID)
                {
                    case 1:
                        strJS = @"
                            $(function () {
                                $.each($('#WebChartViewer1').highcharts().series,	function (i, series) {
  	                                series.update({
    	                                cursor: 'pointer'
                                    });
    
                                    $.each(series.data, function (j, point) {
      	                                point.update({
        	                                events: {
          	                                    click: function () {
            	                                    ChangeTimePeriod(this.x);
                                                }
                                            }
                                        });
                                    });
                                });
                            });
                        ";
                        break;
                }
                if (!string.IsNullOrEmpty(strJS))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "highchart_update_script", strJS, true);
            }
            else
            {
                MakeNoDataChart();
            }
        }
        else
        {
            MakeNoDataChart();
        }
    }


    private void SetChartElements()
    {
        chartHeight.Value = this.ChartHeight.ToString();
        chartWidth.Value = this.ChartWidth.ToString();

        chartTitle.Text = txtGraphTitle_Simple.Text;
        chartSubtitle.Text = txtGraphSubtitle_Simple.Text;

        if (ddlEachAnalyte_Simple.SelectedItem!=null)
        YAxisTile.Text = ddlEachAnalyte_Simple.SelectedItem.Text;

        showWarnings.Value = "0";
        showErrors.Value = "0";
        

        if (chkShowLimits_Simple.Checked)
        {
            if (!String.IsNullOrEmpty(txtWarningValue_Simple.Text))
            {
                double d = 0;
                if (double.TryParse(txtWarningValue_Simple.Text, out d))
                {
                    showWarnings.Value = "1";
                    warningHigh.Value = d.ToString();
                    warningHighColor.Value = ddlWarningColor_Simple.Value;
                    warningHighCaption.Value = txtWarningCaption_Simple.Text;
                }
            }
            if (!String.IsNullOrEmpty(txtExceedanceValue_Simple.Text))
            {
                double d = 0;
                if (double.TryParse(txtExceedanceValue_Simple.Text, out d))
                {
                    showErrors.Value = "1";
                    errorHigh.Value = d.ToString();
                    errorHighColor.Value = ddlExceedanceColor_Simple.Value;
                    errorHighCaption.Value = txtExceedanceCaption_Simple.Text;
                }
            }
        }

        YAxisMin.Value = txtYLowestValue.Text;
        YAxisMax.Value = txtYHighestValue.Text;
        YAxisInterval.Value = txtYInterval.Text;
    }

    private void SetUseCustomScript()
    {
        int currentDefinitionID = 0;
        int.TryParse(ddlGraphType_Simple.SelectedValue, out currentDefinitionID);
        ddlGraphType_Simple.ClearSelection();
        bool bFound = false;
        foreach(ListItem li in ddlGraphType_Simple.Items)
        {
            if (li.Value.StartsWith("-"))
            {
                li.Selected = true;
                bFound = true;
                break;
            }
        }
        if (!bFound)
        {
            ListItem li = new ListItem("-- Custom --", (-1 * currentDefinitionID).ToString());
            li.Selected = true;
            ddlGraphType_Simple.Items.Add(li);
        }
    }

    private void ClearUseCustomScript()
    {
        foreach (ListItem li in ddlGraphType_Simple.Items)
        {
            if ((li.Value.StartsWith("-")) && !li.Selected)
            {
                ddlGraphType_Simple.Items.Remove(li);
                break;
            }
        }
    }

    protected void lbHCSEdit_OK_Click(object sender, EventArgs e)
    {
        SetUseCustomScript();
        MakeChart();
    }

    //protected void gvTheGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{


    //}


    //protected string GetLocationIDs(int iTableID)
    //{
    //    string strLocationIDs = "";

    //    int iTN = 0;
    //    List<Location> lstLocation = SiteManager.ets_Location_Select(null, iTableID, null, "",
    //        "",  true, null, null, null, null,
    //        (int)AccountID,
    //        "LocationName", "ASC", null, null, ref iTN, "");




    //    if (strLocationIDs == "")
    //    {
    //        foreach (Location item in lstLocation)
    //        {
    //            strLocationIDs = strLocationIDs + item.LocationID.ToString() + ",";

    //        }
    //    }

    //    if (strLocationIDs.Length > 0)
    //    {
    //        strLocationIDs = strLocationIDs.Substring(0, strLocationIDs.Length - 1);
    //    }
    //    return strLocationIDs;

    //}


    protected void MakeOneChart(ref  Dictionary<DateTime, int> dcDateTimeRecorded,
        ref DateTime[] labels, ref string[] lblDateTime, DateTime dtFromDateTime, DateTime dtToDateTime, ref  XYChart c,
        string sPeriod, DataTable dtYdata, Column theColumn,
        Column ssColumn, bool bLeft, HashSet<String> hashRightCaption,
        HashSet<String> hashLeftCaption, string strGraphType, ref int iNoOfFields, string strColor, string strSeriesLabel,
        bool bPercent, double? dLow, double? dHigh, string strLabel, ref BarLayer layerStackedBar, ref BarLayer layerBar)
    {
        System.Diagnostics.Debug.Print("{0:G} {1:G}", dtFromDateTime, dtToDateTime);



        //if (chkShowLimits.Checked && _bWarningExceedShown==false)
        //{               

        //    if (txtWarningValue.Text != "")
        //    {
        //        c.yAxis().addMark(double.Parse(txtWarningValue.Text), c.dashLineColor(Common.GetIntColorFromName(ddlWarningColor.Value))).setLineWidth(1);
        //        c.getLegend().addKey(txtWarningCaption.Text, c.dashLineColor(Common.GetIntColorFromName(ddlWarningColor.Value)), 1);
        //    }

        //    if (txtExceedanceValue.Text != "")
        //    {
        //        c.yAxis().addMark(double.Parse(txtExceedanceValue.Text), c.dashLineColor(Common.GetIntColorFromName(ddlExceedanceColor.Value))).setLineWidth(1);
        //        c.getLegend().addKey(txtExceedanceCaption.Text, c.dashLineColor(Common.GetIntColorFromName(ddlExceedanceColor.Value)),1);
        //    }


        //    _bWarningExceedShown = true;
        //}



        string strYCaption = "";
        string strLegendLabel = "";
        int iTableID = -1;
        string strYSys = "";
        if (theColumn != null)
        {
            strYSys = theColumn.SystemName;
            iTableID = (int)theColumn.TableID;

            if (strLabel == "-1")
            {
                if (theColumn.GraphLabel != "")
                {
                    strYCaption = theColumn.GraphLabel;
                    strLegendLabel = theColumn.GraphLabel;
                }
                else
                {
                    strYCaption = theColumn.DisplayName;
                    strLegendLabel = theColumn.DisplayName; ;
                }
            }
            else
            {
                strYCaption = strLabel;
                strLegendLabel = strLabel;
                if (strLabel == "")
                {
                    if (theColumn.GraphLabel != "")
                    {
                        strLegendLabel = theColumn.GraphLabel;
                    }
                    else
                    {
                        strLegendLabel = theColumn.DisplayName; ;
                    }

                }

            }

            if (!String.IsNullOrEmpty(strSeriesLabel))
                strLegendLabel = strSeriesLabel;

        }

        if (bPercent)
        {
            strYCaption = "";
        }

        Table _qsTable = RecordManager.ets_Table_Details(iTableID);
        //Times New Roman Bold Italic  //Verdana Bold 
        //string strLocationIDs = GetLocationIDs(iTableID);
        //strLocationIDs=strLocationIDs.Split(",");
        if (_qsTable != null && strYSys != "")
        {
            //Start X
            int iColor = -1;
            if (strColor != "")
                iColor = Common.GetIntColorFromName(strColor);

            int j = 0;
            //foreach (string strLocation in strLocations)
            //{
            //a Location

            if (j > 0)
                iColor = -1;

            //Location theLocation = SiteManager.ets_Location_Details(int.Parse(strLocation));

            //DataRow[] drSSDataEach = dtYdata.Rows;// .Select("[LocationID]='" + strLocation.Replace("'", "''") + "'");
            double[] dTempData = new double[labels.Length];
            double[] dTempDataMax = new double[labels.Length];
            double[] dTempDataMin = new double[labels.Length];

            int k = 0;
            foreach (DateTime dtTemp3 in labels)
            {
                bool bFound = false;
                foreach (DataRow drSSEachRecord in dtYdata.Rows)
                {

                    if (dtTemp3 == DateTime.Parse(Convert.ToDateTime(drSSEachRecord[0].ToString()).ToString("d/M/yyyy HH:m")))
                    {
                        bFound = true;
                        if (drSSEachRecord[1].ToString() != "")
                        {
                            try
                            {
                                double dTheData;


                                dTheData = double.Parse(drSSEachRecord[1].ToString());
                                if (dLow != null || dHigh != null)
                                {
                                    dTempData[k] = Chart.NoValue;
                                }
                                else
                                {
                                    dTempData[k] = dTheData;
                                }

                                if (dLow != null && dHigh != null)
                                {
                                    if (dTheData >= (double)dLow && dTheData <= (double)dHigh)
                                    {
                                        dTempData[k] = dTheData;
                                    }
                                }
                                else
                                {
                                    if (dLow != null)
                                    {
                                        if (dTheData >= (double)dLow)
                                        {
                                            dTempData[k] = dTheData;
                                        }
                                    }

                                    if (dHigh != null)
                                    {
                                        if (dTheData <= (double)dHigh)
                                        {
                                            dTempData[k] = dTheData;
                                        }
                                    }

                                }


                            }
                            catch (Exception ex)
                            { dTempData[k] = Chart.NoValue; }
                        }
                        else
                        { dTempData[k] = Chart.NoValue; }
                    }
                }
                if (bFound == false)
                { dTempData[k] = Chart.NoValue; }

                k = k + 1;

            }

            int k2 = 0;
            if (strGraphType.ToLower() == "mmm")
            {
                foreach (DateTime dtTemp3 in labels)
                {
                    bool bFound = false;
                    foreach (DataRow drSSEachRecord in dtYdata.Rows)
                    {

                        if (dtTemp3 == DateTime.Parse(Convert.ToDateTime(drSSEachRecord[0].ToString()).ToString("d/M/yyyy HH:m")))
                        {
                            bFound = true;
                            if (drSSEachRecord[2].ToString() != "")
                            {
                                try
                                {
                                    double dTheData;
                                    dTheData = double.Parse(drSSEachRecord[2].ToString());
                                    //dTempDataMax[k2] = dTheData;

                                    if (dLow != null || dHigh != null)
                                    {
                                        dTempDataMax[k2] = Chart.NoValue;
                                    }
                                    else
                                    {
                                        dTempDataMax[k2] = dTheData;
                                    }

                                    if (dLow != null && dHigh != null)
                                    {
                                        if (dTheData >= (double)dLow && dTheData <= (double)dHigh)
                                        {
                                            dTempDataMax[k2] = dTheData;
                                        }
                                    }
                                    else
                                    {
                                        if (dLow != null)
                                        {
                                            if (dTheData >= (double)dLow)
                                            {
                                                dTempDataMax[k2] = dTheData;
                                            }
                                        }

                                        if (dHigh != null)
                                        {
                                            if (dTheData <= (double)dHigh)
                                            {
                                                dTempDataMax[k2] = dTheData;
                                            }
                                        }

                                    }


                                }
                                catch (Exception ex)
                                { dTempDataMax[k2] = Chart.NoValue; }
                            }
                            else
                            { dTempDataMax[k2] = Chart.NoValue; }
                        }
                    }
                    if (bFound == false)
                    { dTempDataMax[k2] = Chart.NoValue; }
                    k2 = k2 + 1;
                }
            }

            int k3 = 0;

            if (strGraphType.ToLower() == "mmm")
            {

                foreach (DateTime dtTemp3 in labels)
                {
                    bool bFound = false;
                    foreach (DataRow drSSEachRecord in dtYdata.Rows)
                    {

                        if (dtTemp3 == DateTime.Parse(Convert.ToDateTime(drSSEachRecord[0].ToString()).ToString("d/M/yyyy HH:m")))
                        {
                            bFound = true;
                            if (drSSEachRecord[3].ToString() != "")
                            {
                                try
                                {
                                    double dTheData;
                                    dTheData = double.Parse(drSSEachRecord[3].ToString());
                                    // dTempDataMin[k3] = dTheData;

                                    if (dLow != null || dHigh != null)
                                    {
                                        dTempDataMin[k3] = Chart.NoValue;
                                    }
                                    else
                                    {
                                        dTempDataMin[k3] = dTheData;
                                    }

                                    if (dLow != null && dHigh != null)
                                    {
                                        if (dTheData >= (double)dLow && dTheData <= (double)dHigh)
                                        {
                                            dTempDataMin[k3] = dTheData;
                                        }
                                    }
                                    else
                                    {
                                        if (dLow != null)
                                        {
                                            if (dTheData >= (double)dLow)
                                            {
                                                dTempDataMin[k3] = dTheData;
                                            }
                                        }

                                        if (dHigh != null)
                                        {
                                            if (dTheData <= (double)dHigh)
                                            {
                                                dTempDataMin[k3] = dTheData;
                                            }
                                        }

                                    }


                                }
                                catch (Exception ex)
                                { dTempDataMin[k3] = Chart.NoValue; }
                            }
                            else
                            { dTempDataMin[k3] = Chart.NoValue; }
                        }
                    }
                    if (bFound == false)
                    { dTempDataMin[k3] = Chart.NoValue; }

                    k3 = k3 + 1;
                }
            }

            if (j == 0)
            {
                if (bLeft == false)
                {

                    if ((strYCaption != "") && !hashRightCaption.Contains(strYCaption))
                    {
                        hashRightCaption.Add(strYCaption);
                    }
                }
                else
                {
                    if ((strYCaption != "") && !hashLeftCaption.Contains(strYCaption))
                    {
                        hashLeftCaption.Add(strYCaption);
                    }
                }
            }

            j = j + 1;
            iNoOfFields = iNoOfFields + 1;


            //End of SSs

            //c.yAxis2().setTitle(strYCaption2);

            //end Y 2
        }
    }


    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        if (hfGraphImageURL.Value != "")
        {
            string fileFullName = hfGraphImageURL.Value;

            Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/SendEmail.aspx?Source=" + Cryptography.Encrypt("jsgraph") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&FileName=" + Cryptography.Encrypt(fileFullName) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() , false);

        }
    }
    protected void MakeNoDataChart()
    {
        chartHeight.Value = this.ChartHeight.ToString();
        chartWidth.Value = this.ChartWidth.ToString();
        CSVData.Text = "0";

        if (DocumentID == null)
        {
            string aspxPath = "http://" + Request.Url.Authority + Request.ApplicationPath + "Pages/Graph/TableGraphConfig.aspx" +
                "?TableID=" + Cryptography.Encrypt(OneTableID.ToString());

            int iNT = 0;
            DataTable dt = GraphManager.ets_GraphDefinition_Select(null, "Data Error Full",
                null, true, true, null, null, null, true,
                null, null, null, null,
                null, null, null, null, ref iNT);
            if ((dt.Rows.Count > 0) && (dt.Rows[0]["Definition"] != DBNull.Value)
                && !String.IsNullOrEmpty(dt.Rows[0]["Definition"].ToString()))
            {
                string script = String.Format("var graphConfigPath = '{0}';", aspxPath) + Environment.NewLine;
                script += dt.Rows[0]["Definition"].ToString().Replace("&lt;", "<").Replace("&gt;", ">") + Environment.NewLine;
                script += @"$(function () {
                $('#graphConfig').fancybox({
                    scrolling: 'no',
                    type: 'iframe',
                    'transitionIn': 'elastic',
                    'transitionOut': 'none',
                    width: 500,
                    height: 250,
                    titleShow: false
                });
            });" + Environment.NewLine;
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "graph_error_2", script, true);
            }
        }
        else
        {
            int iNT = 0;
            DataTable dt = GraphManager.ets_GraphDefinition_Select(null, "No Graph Selected",
                null, true, true, null, null, null, true,
                null, null, null, null,
                null, null, null, null, ref iNT);
            if ((dt.Rows.Count > 0) && (dt.Rows[0]["Definition"] != DBNull.Value)
                && !String.IsNullOrEmpty(dt.Rows[0]["Definition"].ToString()))
            {
                string script = dt.Rows[0]["Definition"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");

                script =
                    @"$(function () {
                        Highcharts.setOptions({
                            navigation: {
                                buttonOptions: {
                                    enabled: false
                                }
                            }
                        });
                    });" + Environment.NewLine + script;

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "graph_error_2", script, true);
            }
        }
    }



    protected void MakeNoGraphChart()
    {
        chartHeight.Value = this.ChartHeight.ToString();
        chartWidth.Value = this.ChartWidth.ToString();
        CSVData.Text = "0";

       
        int iNT = 0;
        DataTable dt = GraphManager.ets_GraphDefinition_Select(null, "Empty Chart",
            null, true, true, null, null, null, true,
            null, null, null, null,
            null, null, null, null, ref iNT);
        if ((dt.Rows.Count > 0) && (dt.Rows[0]["Definition"] != DBNull.Value)
            && !String.IsNullOrEmpty(dt.Rows[0]["Definition"].ToString()))
        {
            string script = dt.Rows[0]["Definition"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");

            script =
                @"$(function () {
                    Highcharts.setOptions({
                        navigation: {
                            buttonOptions: {
                                enabled: false
                            }
                        }
                    });
                });" + Environment.NewLine + script;

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "empty_chart_1", script, true);
        }
        
    }
    protected void toAdvanced_Click(object sender, EventArgs e)
    {
        this.divMain.ActiveViewIndex = 1;
        _bOldDate = true;
        MakeChart();
    }

    protected void toBasic_Click(object sender, EventArgs e)
    {
        this.divMain.ActiveViewIndex = 0;
        _bOldDate = true;
        MakeChart();
    }

    private string GetGraphXAxisColumnName(string TableID)
    {
        int tableID = int.Parse(TableID);

        if (tableID < 0)
        {
            if (ViewState["GraphOptionDetail"] != null)
            {
                DataTable dtRecord = (DataTable)ViewState["GraphOptionDetail"];
                if ((dtRecord.Rows.Count > 0) && !dtRecord.Rows[0].IsNull("TableID"))
                    tableID = (int)dtRecord.Rows[0]["TableID"];
            }
        }

        Table theTable = RecordManager.ets_Table_Details(tableID);
        if ((theTable != null) && theTable.GraphXAxisColumnID.HasValue)
        {
            Column theColumn = RecordManager.ets_Column_Details(theTable.GraphXAxisColumnID.Value);
            return theColumn.SystemName;
        }
        return "";
    }

    private string GetGraphSeriesColumnName(string TableID)
    {
        Table theTable = RecordManager.ets_Table_Details(int.Parse(TableID));
        if ((theTable != null) && theTable.GraphSeriesColumnID.HasValue)
        {
            Column theColumn = RecordManager.ets_Column_Details(theTable.GraphSeriesColumnID.Value);
            return theColumn.SystemName;
        }
        return "";
    }

    private int GetGraphSeriesColumnID(string TableID)
    {
        Table theTable = RecordManager.ets_Table_Details(int.Parse(TableID));
        if ((theTable != null) && theTable.GraphSeriesColumnID.HasValue)
        {
            return theTable.GraphSeriesColumnID.Value;
        }
        return -1;
    }

    protected void ibExcel_Click(object sender, ImageClickEventArgs e)
    {


        try
        {
            CleanUpTempFiles("~\\ExportedFiles", ".xlsx", _tempFileLifeTime);
        }
        catch
        {
            //
        }


        string strInFileName = "chart";

        if (txtGraphTitle_Simple.Text != "")
            strInFileName = txtGraphTitle_Simple.Text ;

        //string strInFileName = ExportToXLSX();
        //strOutFileName += ".xlsx";
        //string s = String.Format("window.open('ExcelResponser.ashx?I={0}&O={1}');",
        //    HttpUtility.UrlEncode(strInFileName),
        //    HttpUtility.UrlEncode(strOutFileName));
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CallAshx", s, true);

        _bOldDate = true;
        MakeChart();

        string strFileFullNamePath = TempOfficeExcel.ExportToXLSXByOffice(dtYDT, strInFileName, ddlTimePeriodDisplay_Simple.SelectedValue,
            ddlEachAnalyte_Simple.SelectedItem.Text,_strXAxisLabel,txtGraphTitle_Simple.Text,
            chkShowLimits_Simple.Checked ? txtWarningValue_Simple.Text : "", chkShowLimits_Simple.Checked ? txtExceedanceValue_Simple.Text : "");




        if (strFileFullNamePath!="" && File.Exists(strFileFullNamePath))
        {
            try
            {


                this.Page.Response.ContentType = "application/octet-stream";

                //Response.AppendHeader("Content-Disposition", "attachment; filename=" +  theDocument.FileUniqename.Substring(37));
                this.Page.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + txtGraphTitle_Simple.Text + ".xlsx\"");
                this.Page.Response.WriteFile(strFileFullNamePath);
                this.Page.Response.End();
            }
            catch
            {
                //
            }
        }
       
        //return;

        //if (strFileFullNamePath != "")
        //{
        //    //string s = String.Format("window.open('ExcelResponser.ashx?I={0}&O={1}');",
        //    //    HttpUtility.UrlEncode(strOutFileName),
        //    //    HttpUtility.UrlEncode(strInFileName+ ".xlsx"));
        //    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CallAshx", s, true);

        //    try
        //    {
        //        strInFileName = strInFileName + ".xlsx";
        //        FileStream inFileStream = new FileStream(strFileFullNamePath, FileMode.Open);
        //        MemoryStream memoryStream = new MemoryStream();
        //        inFileStream.CopyTo(memoryStream);
        //        inFileStream.Close();
        //        //File.Delete(inFileName);
        //        memoryStream.Flush();

        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(strInFileName).Replace("+", "%20")));
        //        Response.Clear();

        //        Response.BinaryWrite(memoryStream.GetBuffer());
        //        Response.End();

        //        //Response.Clear();
        //        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        //Response.AddHeader("Content-Disposition",
        //        //    "attachment; filename*=UTF-8''" + HttpUtility.UrlEncode(strInFileName).Replace("+", "%20"));

        //        //FileStream inFileStream = new FileStream(strFileFullNamePath, FileMode.Open);
        //        //long inFileSize = inFileStream.Length;

        //        //byte[] buffer = new byte[(int)inFileSize];
        //        //inFileStream.Read(buffer, 0, (int)inFileSize);
        //        //inFileStream.Close();
        //        //File.Delete(strFileFullNamePath);

        //        //Response.AddHeader("Content-Length", inFileSize.ToString());
        //        //Response.BinaryWrite(buffer);
        //        //Response.End();
        //    }
        //    catch
        //    {

        //    }

        //}
    }


    //public string ExportToXLSX()
    //{
    //    CleanUpTempFiles(_tempDirectory, ".xlsx", _tempFileLifeTime);

    //    // create generic name
    //    string fileName = System.Guid.NewGuid().ToString() + ".xlsx";
    //    string fileFullName = HttpContext.Current.Server.MapPath(_tempDirectory) + fileName;

    //    ExcelWriter exwr = new ExcelWriter();
    //    try
    //    {
    //        DataTable dtRecord = (DataTable)ViewState["GraphOptionDetail"];
    //        int iChartCount = 0;
    //        int iTN = 0;

    //        Dictionary<DateTime, int> dcDateTimeRecorded = new Dictionary<DateTime, int>();

    //        string sPeriod = "Hour";
    //        switch (ddlTimePeriod.Text)
    //        {
    //            case "H":
    //                sPeriod = "Hour";
    //                break;
    //            case "D":
    //                sPeriod = "Day";
    //                break;
    //            case "W":
    //                sPeriod = "Week";
    //                break;
    //            case "M":
    //                sPeriod = "Month";
    //                break;
    //            case "Y":
    //                sPeriod = "Year";
    //                break;
    //        }
    //        if (ddlTimePeriodDisplay.SelectedValue == "C")
    //        {
    //            sPeriod = "Hour";
    //        }

    //        foreach (DataRow drEachST in dtRecord.Rows)
    //        {
    //            if (drEachST["ColumnID"] == DBNull.Value)
    //            {
    //                continue;
    //            }

    //            // one chart
    //            iChartCount = iChartCount + 1;
    //            Column theColumn = RecordManager.ets_Column_Details(int.Parse(drEachST["ColumnID"].ToString()));

    //            string strColumnSystemName = theColumn.SystemName;
    //            string strColumnGraphLabel = theColumn.GraphLabel;

    //            string strGraphType = "";
    //            if (drEachST["GraphType"] != DBNull.Value && drEachST["GraphType"].ToString() != "")
    //            {
    //                strGraphType = drEachST["GraphType"].ToString();
    //            }

    //            bool bLeft = true;
    //            bool bPercent = false;
    //            if (drEachST["Axis"] != DBNull.Value && drEachST["Axis"].ToString() != "")
    //            {
    //                if (drEachST["Axis"].ToString() == "Right")
    //                    bLeft = false;

    //                if (drEachST["Axis"].ToString() == "Percentage")
    //                    bPercent = true;
    //            }

    //            string strGraphSeriesColumnName = drEachST["GraphSeriesColumnID"].ToString();
    //            string strGraphSeriesValue = drEachST["GraphSeriesID"].ToString();

    //            List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(drEachST["TableID"].ToString()),
    //                    null, null, ref iTN);

    //            Column ssColumn = new Column();
    //            Column dtColumn = new Column();

    //            string graphXAxisColumnName = GetGraphXAxisColumnName(drEachST["TableID"].ToString());

    //            foreach (Column eachColumn in lstColumns)
    //            {
    //                if (eachColumn.SystemName == graphXAxisColumnName)
    //                {
    //                    dtColumn = eachColumn;
    //                }

    //            }

    //            if (iChartCount == 1)
    //            {
    //                string strXSummary = dtColumn.GraphLabel;
    //            }

    //            //DataTable dtYdata;

    //            if (strGraphType.ToLower() == "mmm")
    //            {

    //                dtYdata = RecordManager.ets_Chart_GetData_MeanMaxMin(int.Parse(drEachST["TableID"].ToString()),
    //                    dtColumn.SystemName, strColumnSystemName, (DateTime)ViewState["dtFromDateTime"],
    //                    (DateTime)ViewState["dtToDateTime"], sPeriod, Common.MaxGraphRecords);
    //            }
    //            else
    //            {

    //                dtYdata = RecordManager.ets_Chart_GetData_New(int.Parse(drEachST["TableID"].ToString()),
    //                    dtColumn.SystemName, strColumnSystemName,
    //                    (DateTime)ViewState["dtFromDateTime"], (DateTime)ViewState["dtToDateTime"],
    //                    strGraphSeriesColumnName, strGraphSeriesValue,
    //                    sPeriod, bPercent, /*Common.MaxGraphRecords*/ null);
    //            }

    //            exwr.AddSeries(dtYdata, dtColumn.SystemName, theColumn.SystemName);

    //            //MakeOneChart(ref dcDateTimeRecorded, ref labels, ref lblDateTime, (DateTime)ViewState["dtFromDateTime"],
    //            //    (DateTime)ViewState["dtToDateTime"], ref c, sPeriod,
    //            //    dtYdata, theColumn, ssColumn,
    //            //    bLeft, hashRightCaption, hashLeftCaption, strGraphType, ref _iNoOfSeries,
    //            //    strColor, strSeriesLabel, bPercent, dLow, dHigh, strLabel, ref layerStackedBar, ref layerBar);

    //        }//end drEachST

    //        exwr.CreateWorkbook(fileFullName);

    //        return fileFullName;
    //    }
    //    catch (IOException ioe)
    //    {
    //        DBGurus.AddErrorLog(ioe.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        DBGurus.AddErrorLog(ex.Message);
    //        MakeNoDataChart();

    //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Problem", "alert('There is no graph to export.');", true);
    //    }

    //    return "";
    //}

    //protected void chkShowLimits_Simple_CheckedChanged(object sender, EventArgs e)
    //{
    //    chkShowLimits.Checked = chkShowLimits_Simple.Checked;
    //    chkShowLimits_CheckedChanged(sender, e);
    //}

    protected void HiddenButtonRefresh_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_fancybox", "$.fancybox.close();", true);
        PopulateSampleSitesList(ddlGraphOption.SelectedValue);
        MakeChart();
    }
}

public class CSVFormatter
{
    private int _nSeries;
    private int _nRows;
    Dictionary<string, int> _categories;
    private bool _sort;
    Dictionary<int, decimal?>[] _series;
    string _firstColumnLabel;
    string[] _seriesNames;

    public CSVFormatter()
    {
        _nSeries = 0;
        _nRows = 0;
        _categories = new Dictionary<string, int>();
        _sort = false;
        _series = new Dictionary<int, decimal?>[0];
        _seriesNames = new string[0];
    }

    public void AddAllSeries(System.Data.DataTable table, string dateColumnName, string valueColumnName, string SeriesName)
    {
        _nSeries = table.Columns.Count - 1;
        Array.Resize<Dictionary<int, decimal?>>(ref _series, _nSeries);
        Array.Resize<String>(ref _seriesNames, _nSeries);
        for (int i = 1; i < table.Columns.Count; i++)
        {
            _seriesNames[i - 1] = table.Columns[i].ColumnName;
            _series[i - 1] = new Dictionary<int, decimal?>();
        }

        bool hasDateTimeColumn = table.Columns.Contains(dateColumnName);

        for (int i = 0; i < table.Rows.Count; i++)
        {
            string category = String.Empty;
            if (hasDateTimeColumn)
            {
                DateTime dt;
                if (table.Rows[i][dateColumnName] is DateTime)
                    dt = (DateTime)table.Rows[i][dateColumnName];
                else
                    dt = DateTime.Parse(table.Rows[i][dateColumnName].ToString());
                category = dt.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
                category = table.Rows[i][0].ToString();

            int n = -1;
            if (_categories.ContainsKey(category))
            {
                n = _categories[category];
            }
            else
            {
                _nRows++;
                _categories.Add(category, _nRows);
                n = _nRows;
            }

            for (int j = 1; j < table.Columns.Count; j++)
            {
                Decimal value = 0m;
                if (!Decimal.TryParse(table.Rows[i][j].ToString(), out value))
                    value = 0m;
                _series[j - 1].Add(n, value);
            }
        }
    }

    public void AddSeries(System.Data.DataTable table, string dateColumnName, string valueColumnName, string SeriesName)
    {
        bool hasDateTimeColumn = table.Columns.Contains(dateColumnName);
        _sort = _sort || hasDateTimeColumn;

        Dictionary<int, decimal?> dict = new Dictionary<int, decimal?>();
        foreach(DataRow row in table.Rows)
        {
            Decimal value = 0m;

            //Decimal value =null;

            if (!Decimal.TryParse(row[valueColumnName].ToString(), out value))
                value = 0m;
            int n = -1;

            string category = String.Empty;
            if (hasDateTimeColumn)
            {
                DateTime dt;
                if (row[dateColumnName] is DateTime)
                    dt = (DateTime)row[dateColumnName];
                else
                    dt = DateTime.Parse(row[dateColumnName].ToString());
                category = dt.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
                category = row[0].ToString();

            if (_categories.ContainsKey(category))
            {
                n = _categories[category];
            }
            else
            {
                _nRows++;
                _categories.Add(category, _nRows);
                n = _nRows;
            }

            if (!dict.ContainsKey(n)) //MR
            {
                if (row[valueColumnName]==DBNull.Value)
                {
                    dict.Add(n, null);
                }
                else
                {
                    dict.Add(n, value);
                }
                
            }
        }
        Array.Resize<Dictionary<int, decimal?>>(ref _series, _nSeries + 1);
        Array.Resize<String>(ref _seriesNames, _nSeries + 1);
        _series[_nSeries] = dict;
        _seriesNames[_nSeries] = String.IsNullOrEmpty(SeriesName) ? "Average" : SeriesName;
        _nSeries++;
    }

    public string CSV
    {
        get
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(_firstColumnLabel);
            sb.Append(",");
            for (int i = 0; i < _nSeries; i++)
            {
                sb.Append(_seriesNames[i]);
                if (i != _nSeries - 1)
                {
                    sb.Append(",");
                }
                else
                {
                    sb.AppendLine();
                }
            }

            List<string> listCategories = _categories.Keys.ToList<String>();
            if (_sort)
                listCategories.Sort();

            foreach (string category in listCategories)
            {
                sb.Append(category);
                sb.Append(",");
                for (int i = 0; i < _nSeries; i++)
                {
                    if (_series[i].ContainsKey(_categories[category]))
                    {
                        sb.Append(_series[i][_categories[category]].ToString());
                    }
                    if (i != _nSeries - 1)
                    {
                        sb.Append(",");
                    }
                    else
                    {
                        sb.AppendLine();
                    }
                }
            }
            return sb.ToString();
        }
    }

    public string FirstColumnLabel
    {
        get { return _firstColumnLabel; }
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                _firstColumnLabel = "?";
            }
            else
            {
                _firstColumnLabel = value;
            }
        }
    }
}




namespace ExcelNS
{
    using Microsoft.Office.Interop.Excel;
    public class TempOfficeExcel
    {


        public static string ExportToXLSXByOffice(System.Data.DataTable[] dtYDT, string strFileName, string strTimePeriod,string strYAxisLabel
            , string strXAxisLabel, string strChartTitle,string strWarning,string strExceedance)
        {
            strTimePeriod = strTimePeriod.ToLower();
            string strOutPutFile = "";
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            object misValue = System.Reflection.Missing.Value;

            excel = new Microsoft.Office.Interop.Excel.Application();

            excelworkBook = excel.Workbooks.Add(misValue);
            excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.Worksheets.get_Item(1);

            try
            {

                int iEachSS = 0;
                int iMaxRC = 0;
                int iNumberOfSeries = 0;


                System.Data.DataTable dtXAxis=new System.Data.DataTable();
                dtXAxis.Columns.Add("X");
                dtXAxis.Columns[0].DataType =typeof( System.DateTime);
                dtXAxis.AcceptChanges();

                foreach (System.Data.DataTable dtYdata in dtYDT)
                {
                    if (dtYdata!=null)
                    {
                        foreach (DataRow dr in dtYdata.Rows)
                        {
                            dtXAxis.Rows.Add(DateTime.Parse(dr[0].ToString()));
                        }
                    }
                    
                    
                }
                dtXAxis.AcceptChanges();

                DataView view = new DataView(dtXAxis);
                System.Data.DataTable distinctValues = view.ToTable(true);

                DataView dvX = distinctValues.DefaultView;
                dvX.Sort = distinctValues.Columns[0].ColumnName + " ASC";
                System.Data.DataTable dtX = dvX.ToTable();

                if (strWarning!="")
                 {
                     System.Data.DataTable dtWarning = dtX.Copy();
                     dtWarning.Columns.Add("Warning", typeof(System.Double));
                     dtWarning.TableName = "Warning";
                     dtWarning.AcceptChanges();
                    if(dtWarning.Rows.Count>0)
                    {
                        dtWarning.Rows[0][1] = double.Parse(strWarning);
                        dtWarning.Rows[dtWarning.Rows.Count-1][1] = double.Parse(strWarning);
                        dtWarning.AcceptChanges();
                        dtYDT[dtYDT.Length - 2] = dtWarning;
                    }
                 }

                if (strExceedance != "")
                {
                    System.Data.DataTable dtExceedance = dtX.Copy();
                    dtExceedance.Columns.Add("Exceedance", typeof(System.Double));
                    dtExceedance.TableName = "Exceedance";
                    dtExceedance.AcceptChanges();
                    if (dtExceedance.Rows.Count > 0)
                    {
                        dtExceedance.Rows[0][1] = double.Parse(strExceedance);
                        dtExceedance.Rows[dtExceedance.Rows.Count - 1][1] = double.Parse(strExceedance);
                        dtExceedance.AcceptChanges();
                        dtYDT[dtYDT.Length - 1] = dtExceedance;
                    }
                }



                int y = 2;
                iMaxRC = dtX.Rows.Count;
                foreach (DataRow dr in dtX.Rows)
                {
                    if (dr[0] != DBNull.Value)
                    {
                        try
                        {
                            if (strTimePeriod == "d" || strTimePeriod == "h")
                            {
                                excelSheet.Cells[y, 1] = DateTime.Parse(dr[0].ToString()).ToLongTimeString();

                            }
                            else
                            {
                                excelSheet.Cells[y, 1] = DateTime.Parse(dr[0].ToString());
                            }


                        }
                        catch
                        {
                            //

                        }
                    }
                   
                    y = y + 1;
                }


                foreach (System.Data.DataTable dtYdata in dtYDT)
                {
                    if (dtYdata == null)
                        continue;


                    int iRC = dtYdata.Rows.Count;
                    

                    if (iEachSS==0)
                    {
                        excelSheet.Cells[1, 1] = strXAxisLabel;
                    }



                    excelSheet.Cells[1, iEachSS + 2] = dtYdata.TableName + "(" + strYAxisLabel  + ")";
                    
                   


                    int i = iEachSS+2;

                    DataView dv = dtYdata.DefaultView;
                    dv.Sort = dtYdata.Columns[0].ColumnName + " ASC";
                    System.Data.DataTable sortedDT = dv.ToTable();

                    foreach (DataRow dr in sortedDT.Rows)
                    {

                        int z = 2;
                        bool bAddY = false;
                        foreach (DataRow dr2 in dtX.Rows)
                        {
                            if (dr2[0] != DBNull.Value)
                            {
                                try
                                {
                                    
                                        if (DateTime.Parse(dr2[0].ToString()) == DateTime.Parse(dr[0].ToString()))
                                        {
                                            bAddY = true;
                                            break;
                                        }                                      

                                }
                                catch
                                {
                                    //

                                }
                            }

                            z = z + 1;
                        }

                        if (dr[1] != DBNull.Value && bAddY)
                        {
                            try
                            {
                                excelSheet.Cells[z, iEachSS+2] = double.Parse(dr[1].ToString());

                                if (z > 2 && dtYdata.TableName.ToLower() == "warning")
                                {
                                    (excelSheet.Cells[z, iEachSS + 2] as Microsoft.Office.Interop.Excel.Range).Formula = "=" + (excelSheet.Cells[2, iEachSS + 2] as Microsoft.Office.Interop.Excel.Range).Address + "";
                                    //(excelSheet.Cells[z, iEachSS + 2] as Microsoft.Office.Interop.Excel.Range).Locked = true;
                                }

                                if (z > 2 && dtYdata.TableName.ToLower() == "exceedance")
                                {
                                    (excelSheet.Cells[z, iEachSS + 2] as Microsoft.Office.Interop.Excel.Range).Formula = "=" + (excelSheet.Cells[2, iEachSS + 2] as Microsoft.Office.Interop.Excel.Range).Address + "";
                                    //(excelSheet.Cells[z, iEachSS + 2] as Microsoft.Office.Interop.Excel.Range).Locked = true;
                                }
                            }
                            catch
                            {
                                //

                            }
                        }
                       

                        //i = i + 1;
                    }

                    int z2 = 2;
                    foreach (DataRow dr2 in dtX.Rows)
                    {
                        if ((excelSheet.Cells[z2, iEachSS + 2] as Microsoft.Office.Interop.Excel.Range).Value == null)
                        {
                            excelSheet.Cells[z2, iEachSS + 2] = "#N/A";
                        }

                            z2 = z2 + 1;
                    }




                    iEachSS = iEachSS + 1;
                }

                //System.Data.DataTable dtYdata = dtYDT[0];

                iNumberOfSeries = iEachSS;
                

                Microsoft.Office.Interop.Excel.ChartObjects charts = (Microsoft.Office.Interop.Excel.ChartObjects)excelSheet.ChartObjects(Type.Missing);

                Microsoft.Office.Interop.Excel.ChartObject chartObject = (Microsoft.Office.Interop.Excel.ChartObject)charts.Add(300, 100, 500, 300);
                Microsoft.Office.Interop.Excel.Chart chart = chartObject.Chart;

                // Set chart range.
                Range startCell = (Range)excelSheet.Cells[2, 1];
                Range endCell = (Range)excelSheet.Cells[iMaxRC + 1, iNumberOfSeries+1];
                Range range = excelSheet.get_Range(startCell, endCell);


                chart.SetSourceData(range, misValue);
                chart.HasTitle = true;
                chart.ChartTitle.Text = strChartTitle;
                
                //chart.Legend.Clear();
               

                Microsoft.Office.Interop.Excel.SeriesCollection oSeriesCollection = (Microsoft.Office.Interop.Excel.SeriesCollection)chart.SeriesCollection(misValue);

                int s = 0;
                foreach( Microsoft.Office.Interop.Excel.Series series1 in oSeriesCollection)
                {
                    if (dtYDT[s]!=null)
                    {
                        series1.Name = dtYDT[s].TableName;
                    }
                    else
                    {
                        series1.Name = "Exceedance";
                    }
                   
                    s = s + 1;
                }


                var yAxis = (Microsoft.Office.Interop.Excel.Axis)chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                yAxis.HasTitle = true;
                yAxis.AxisTitle.Text = strYAxisLabel;
               // yAxis.MaximumScale = 20;
                yAxis.AxisTitle.Orientation = Microsoft.Office.Interop.Excel.XlOrientation.xlUpward;



                // Set chart properties.
                if (strTimePeriod == "d" || strTimePeriod == "h")
                {
                    chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlXYScatterLines;
                }
                else
                {
                    chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLineMarkers;
                }


                //Axis xA = (Axis)chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory);

                //xA.CategoryType = XlCategoryType.xlTimeScale;


                string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName + ".xlsx";

                strUniqueName = Common.GetValidFileName(strUniqueName);
                string strFolderPath = System.Web.HttpContext.Current.Server.MapPath("~\\ExportedFiles");
                string strPath = strFolderPath + "\\" + strUniqueName;

                strOutPutFile = strPath;

                excelworkBook.SaveAs(strPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue,
                    misValue, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlUserResolution, true,
                    misValue, misValue, misValue);

                excelworkBook.Close(true, misValue, misValue);
                excel.Quit();

                releaseObject(excelSheet);
                releaseObject(excelworkBook);
                releaseObject(excel);

            }
            catch
            {
                releaseObject(excelSheet);
                releaseObject(excelworkBook);
                releaseObject(excel);
                strOutPutFile = "";
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }


            return strOutPutFile;
        }

        //public static string ExportToXLSXByOffice(System.Data.DataTable[] dtYDT, string strFileName,string strTimePeriod)
        //{
        //    strTimePeriod=strTimePeriod.ToLower();
        //    string strOutPutFile = "";
        //    Microsoft.Office.Interop.Excel.Application excel;
        //    Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        //    Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    excel = new Microsoft.Office.Interop.Excel.Application();

        //    excelworkBook = excel.Workbooks.Add(misValue);
        //    excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.Worksheets.get_Item(1);

        //    try
        //    {
        //        System.Data.DataTable dtYdata = dtYDT[0];
        //        int iRC = dtYdata.Rows.Count;
                
        //        excelSheet.Cells[1, 1] = dtYdata.Columns[0].ColumnName;


        //        excelSheet.Cells[1, 2] = dtYdata.Columns[1].ColumnName;

        //        int i = 2;

        //        DataView dv = dtYdata.DefaultView;
        //        dv.Sort = dtYdata.Columns[0].ColumnName + " ASC";
        //      System.Data.DataTable sortedDT =dv.ToTable();

        //        foreach (DataRow dr in sortedDT.Rows)
        //        {
        //            if (dr[0] != DBNull.Value)
        //            {
        //                try
        //                {
        //                    if (strTimePeriod == "d" || strTimePeriod == "h")
        //                    {
        //                        excelSheet.Cells[i, 1] = DateTime.Parse(dr[0].ToString()).ToLongTimeString();
                                
        //                    }
        //                    else
        //                    {
        //                        excelSheet.Cells[i, 1] = DateTime.Parse(dr[0].ToString());
        //                    }
                            
                            
        //                }
        //                catch
        //                {
        //                    //

        //                }
        //            }

        //            if (dr[1] != DBNull.Value)
        //            {
        //                try
        //                {
        //                    excelSheet.Cells[i, 2] = double.Parse(dr[1].ToString());
        //                }
        //                catch
        //                {
        //                    //

        //                }
        //            }

        //            i = i + 1;
        //        }

        //        Microsoft.Office.Interop.Excel.ChartObjects charts = (Microsoft.Office.Interop.Excel.ChartObjects)excelSheet.ChartObjects(Type.Missing);

        //        Microsoft.Office.Interop.Excel.ChartObject chartObject = (Microsoft.Office.Interop.Excel.ChartObject)charts.Add(300, 100, 500, 300);
        //        Microsoft.Office.Interop.Excel.Chart chart = chartObject.Chart;

        //        // Set chart range.
        //        Range startCell = (Range)excelSheet.Cells[2, 1];
        //        Range endCell = (Range)excelSheet.Cells[iRC + 1, 2];
        //        Range range = excelSheet.get_Range(startCell, endCell);


        //        chart.SetSourceData(range, misValue);

        //        // Set chart properties.
        //        if (strTimePeriod == "d" || strTimePeriod == "h")
        //        {
        //            chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlXYScatterLines;
        //        }
        //        else
        //        {
        //            chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;
        //        }
                

        //        //Axis xA = (Axis)chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory);

        //        //xA.CategoryType = XlCategoryType.xlTimeScale;
               
                
        //        string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName + ".xlsx";

        //        strUniqueName = Common.GetValidFileName(strUniqueName);
        //        string strFolderPath = System.Web.HttpContext.Current.Server.MapPath("~\\ExportedFiles");
        //        string strPath = strFolderPath + "\\" + strUniqueName;

        //        strOutPutFile = strPath;

        //        excelworkBook.SaveAs(strPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue,
        //            misValue, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
        //            Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlUserResolution, true,
        //            misValue, misValue, misValue);

        //        excelworkBook.Close(true, misValue, misValue);
        //        excel.Quit();

        //        releaseObject(excelSheet);
        //        releaseObject(excelworkBook);
        //        releaseObject(excel);

        //    }
        //    catch
        //    {
        //        releaseObject(excelSheet);
        //        releaseObject(excelworkBook);
        //        releaseObject(excel);
        //        strOutPutFile ="";
        //    }


        //    return strOutPutFile;
        //}
        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                //MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


    }

}
