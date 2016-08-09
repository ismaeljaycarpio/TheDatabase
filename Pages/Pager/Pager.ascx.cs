
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.Web.UI.HtmlControls;
using iTextSharp.text.html.simpleparser;
using DBGServerControl;
using System.Data;
using System.Data.SqlClient;


public partial class Common_Pager : System.Web.UI.UserControl
{

    bool _bOpenInParent = false;

    public dbgGridView _gridView;
    private ScriptManager _scriptManager;
   
    private string _strTable = "User";
    //private int? _iAccontID ;
    //private string _strFullName = "";
    private string _strAddURL = "#";
    private string _strAddURL2 = "#";
    private string _strEditURL = "#";
    
    private bool _bIsInLine=false ;
    private bool _bIsDynamicColumn = false;
    private bool _bHideAdd = false;
    private bool _bHideGo = false;
    private bool _bHideNavigation = false;
    private bool _bHideEdit = false;
    private bool _bHideDelete = false;
    private bool _bHideUnDelete = true;
    private bool _bHideParmanenetDel = true;
    private bool _bHideEditMany = true;
    private bool _bHideSendEmail = true;

    private bool _bHideExcelExport = true;
    private bool _bHideAllExport = true;


    private bool _bHideFilter = false;
    private string _strAddImageURL="Images/add.png";
    private string _strAddImageURL2 = "Images/add.png";
    private bool _bShowAdd2 = false;
    private string _strAddToolTip = "Add";
    private string _strAddToolTip2 = "Add";

    private bool _bHideExport = false;
    private bool _bHideRefresh = false;
    private bool _bHidePageSize = false;
    private bool _bHidePageSizeButton = true;

    public string _strExportFileName = "GridViewExport";

    public event EventHandler DeleteAction;
    public event EventHandler UnDeleteAction;
    public event EventHandler ParmanenetDelAction;
    public event EventHandler EditManyAction;
    public event EventHandler AllExport;
    public event EventHandler ShowCog;
    public event EventHandler CopyRecordAction;
    public event EventHandler SendEmailAction;

    public event EventHandler BindTheGridToExport;

    public event EventHandler BindTheGridAgain;

    public event EventHandler ApplyFilter;

    public event EventHandler ExportForCSV;
    public event EventHandler ExportForExcel;

    public event EventHandler CustomExportPDF;

    private bool _bDOCustomPDF = false;
    private bool _bCustomPager = false;

    //public int? TableID { get; set; }

    public int? TableID
    {
        get
        {
            return ViewState["TableID"] == null ? null : (int?)int.Parse(ViewState["TableID"].ToString());
        }
        set
        {
            ViewState["TableID"] = value;
            if(IsPostBack)
            {
                ShowHideSizeButtonMethod();
            }
        }
    }




    public bool CustomPager
    {
         get
        {
            return _bCustomPager;
        }
        set
        {
            _bCustomPager = value;
        }
    }

    public string HyperAdd_CSS
    {
        
        set
        {
            HyperAdd.CssClass = value;
        }
    }

    public bool DOCustomPDF
    {
        get
        {
            return _bDOCustomPDF;
        }
        set
        {
            _bDOCustomPDF = value;

            if (_bDOCustomPDF)
            {
                cmdPDF.OnClientClick = "Javascript:confirmation();";
            }
        }

    }

    public bool ShowAdd2
    {
        get
        {
            return _bShowAdd2;
        }
        set
        {
            _bShowAdd2 = value;
            HyperAdd2.Visible = _bShowAdd2;           
        }
    }


    public bool HidePagerGoButton
    {
       
        set
        {
            tdGoS.Visible = !value;
        }
    }

    public bool ShowUploadFile
    {       
        set
        {
           
            hlUploadFile.Visible = value;
        }
    }


    public bool ShowCopyRecord
    {
        set
        {

            lnkCopyRecord.Visible = value;
        }
    }


    public string UploadFileURL
    {
        get
        {
            return hlUploadFile.NavigateUrl;
        }
        set
        {

            hlUploadFile.NavigateUrl = value;
        }
    }

    public bool HidePageSize
    {
        get
        {
            return _bHidePageSize;
        }
        set
        {
            _bHidePageSize = value;
            tdPageSize.Visible = !_bHidePageSize;
            tdPageSizeRow.Visible = !_bHidePageSize;
            tdPageSizeRow1.Visible = !_bHidePageSize;
            tdPageSizeRow2.Visible = !_bHidePageSize;
            tdPageSizeRow3.Visible = !_bHidePageSize;
            
        }
    }

    public bool HidePageSizeButton
    {
        get
        {
            return _bHidePageSizeButton;
        }
        set
        {
            _bHidePageSizeButton = value;

          

            tdPageSize.Visible = !_bHidePageSizeButton;
          
            tdPageSizeRow1.Visible = !_bHidePageSizeButton;
            tdPageSizeRow2.Visible = !_bHidePageSizeButton;
            tdPageSizeRow3.Visible = !_bHidePageSizeButton;
            cmdLast.Visible = !_bHidePageSizeButton;
            cmdFirst.Visible = !_bHidePageSizeButton;
            //cmdCog.Visible = _bHidePageSizeButton;
           
            tdFirst.Visible = !_bHidePageSizeButton;
            tdLast.Visible = !_bHidePageSizeButton;
            cmdUp.Visible = !_bHidePageSizeButton;
            cmdDown.Visible = !_bHidePageSizeButton; ;

            if (_bHidePageSizeButton)
            {
                tdDown.Style.Add("width", "1px");
            }

        }
    }
    public bool HideAdd
    {
        get
        {
            return _bHideAdd;
        }
        set
        {
            _bHideAdd = value;
            HyperAdd.Visible = !_bHideAdd;
            cmdAdd.Visible =false;
        }
    }

    public bool HideEditView
    {
        get
        {
            return hlEditView.Visible;
        }
        set
        {
            hlEditView.Visible = !value;
            
        }
    }

    public bool HideGo
    {
        get
        {
            return _bHideGo;
        }
        set
        {
            _bHideGo = value;
            divGo.Visible = !_bHideGo;
            
        }
    }

    public bool HideNavigation
    {
        get
        {
            return _bHideNavigation;
        }
        set
        {
            _bHideNavigation = value;
            tblNavigation.Visible = !_bHideNavigation;

        }
    }

    public bool HideRefresh
    {
        get
        {
            return _bHideRefresh;
        }
        set
        {
            _bHideRefresh = value;
            cmdRefresh.Visible = !_bHideRefresh;
            imgRefresh.Visible = !_bHideRefresh;
        }
    }

    public bool HideExport
    {
        get
        {
            return _bHideExport;
        }
        set
        {
            _bHideExport = value;
            cmdWord.Visible = !_bHideExport;
            cmdCSV.Visible = !_bHideExport;
            cmdPDF.Visible = !_bHideExport;
            imgCSV.Visible = !_bHideExport;
            imgWord.Visible = !_bHideExport;
            imgPDF.Visible = !_bHideExport;
            if (_bHideExport)
            {

                
                tdAddPart.Width="100";

                if(HideAdd)
                    tdAddPart.Width = "50";

                if(HideDelete)
                    tdAddPart.Width = "50";

            }
        }
    }

  


    public bool HideUnDelete
    {
        get
        {
            return _bHideUnDelete;
        }
        set
        {
            _bHideUnDelete = value;
            lnkUnDelete.Visible = !_bHideUnDelete;
        }
    }

    public bool HideParmanentDelete
    {
        get
        {
            return _bHideParmanenetDel;
        }
        set
        {
            _bHideParmanenetDel = value;
            lnkParmanentDelete.Visible = !_bHideParmanenetDel;
        }
    }

    public bool HideEditMany
    {
        get
        {
            return _bHideEditMany;
        }
        set
        {
            _bHideEditMany = value;
            lnkEditMany.Visible = !_bHideEditMany;
        }
    }


    public bool HideSendEmail
    {
        get
        {
            return _bHideSendEmail;
        }
        set
        {
            _bHideSendEmail = value;
            lnkSendEmail.Visible = !_bHideSendEmail;
        }
    }


    public bool HideExcelExport
    {
        get
        {
            return _bHideExcelExport;
        }
        set
        {
            _bHideExcelExport = value;
            cmdExcel.Visible = !_bHideExcelExport;
            imgExcel.Visible = !_bHideExcelExport;
        }
    }

    public bool HideAllExport
    {
        get
        {
            return _bHideAllExport;
        }
        set
        {
            _bHideAllExport = value;
            cmdAllExport.Visible = !_bHideAllExport;
            imgAllExportS.Visible = !_bHideAllExport;
        }
    }



    public bool HideDelete
    {
        get
        {
            return _bHideDelete;
        }
        set
        {
            _bHideDelete = value;
            DeleteLinkButton.Visible = !_bHideDelete;
        }
    }


    public bool HideFilter
    {
        get
        {
            return _bHideFilter;
        }
        set
        {
            _bHideFilter = value;
            cmdFilter.Visible = !_bHideFilter;
            imgFilter.Visible = !_bHideFilter;
        }
    }



    public bool HideEdit
    {
        get
        {
            return _bHideEdit;
        }
        set
        {
            _bHideEdit = value;
            divEdit.Visible = !_bHideEdit;
            
        }
    }

    public string EditURL
    {
        get
        {
            return _strEditURL;
        }
        set
        {
            _strEditURL = value;
            hlEdit.NavigateUrl = _strEditURL;
        }
    }

   


   
    public bool IsInLine
    {
        get
        {
            return _bIsInLine;
        }
        set
        {
            _bIsInLine = value;
        }
    }



    public string TableName
    {
        get
        {
            return _strTable;
        }
        set
        {
            _strTable = value;
        }
    }


    public string ExportFileName
    {
        get
        {
            return _strExportFileName;
        }
        set
        {
            _strExportFileName = value;            
        }
    }
    



    public string AddURL
    {
        get
        {
            return _strAddURL;
        }
        set
        {
            _strAddURL = value;
            HyperAdd.NavigateUrl = _strAddURL;           
               

        }
    }

    //public string AddImageURL
    //{
    //    get
    //    {
    //        return HyperAdd.ImageUrl;
    //    }
    //    set
    //    {
    //        HyperAdd.ImageUrl = value;
            


    //    }
    //}

    public string EditViewURL
    {
        get
        {
            return hlEditView.NavigateUrl;
        }
        set
        {
            hlEditView.NavigateUrl = value;           


        }
    }


    public string EditViewTarget
    {
        get
        {
            return hlEditView.Target;
        }
        set
        {
            hlEditView.Target = value;


        }
    }

    public string EditViewCSSClass
    {
        get
        {
            return hlEditView.CssClass;
        }
        set
        {
            hlEditView.CssClass = value;


        }
    }


    public string AddURL2
    {
        get
        {
            return _strAddURL2;
        }
        set
        {
            _strAddURL2 = value;
            HyperAdd2.NavigateUrl = _strAddURL2;
            
        }
    }

    public string AddToolTip
    {
        get
        {
            return _strAddToolTip;
        }
        set
        {
            _strAddToolTip = value;
            HyperAdd.ToolTip = _strAddToolTip;
        }
    }

    public string EditViewToolTip
    {
        get
        {
            return hlEditView.ToolTip;
        }
        set
        {
            hlEditView.ToolTip = value;           
        }
    }

    public string EditViewOnClick
    {
        
        set
        {
            hlEditView.Attributes.Add("onclick", value);
        }
    }

    public string EditManyToolTip
    {
        get
        {
            return lnkEditMany.ToolTip;
        }
        set
        {
            
            lnkEditMany.ToolTip = value;
        }
    }

    public string AddToolTip2
    {
        get
        {
            return _strAddToolTip2;
        }
        set
        {
            _strAddToolTip2 = value;
            HyperAdd2.ToolTip = _strAddToolTip2;
        }
    }


    public string AddImageURL
    {
        get
        {
            return HyperAdd.ImageUrl;
        }
        set
        {
            HyperAdd.ImageUrl = value;
            //HyperAdd.ImageUrl = _strAddImageURL;
        }
    }

    public string AddImageURL2
    {
        get
        {
            return _strAddImageURL2;
        }
        set
        {
            _strAddImageURL2 = value;
            HyperAdd2.ImageUrl = _strAddImageURL2;
        }
    }


    public int PageIndex
    {
        get
        {
            return Convert.ToInt32(txtPageIndex.Text);
        }
        set
        {
            txtPageIndex.Text = value.ToString();
        }
    }

  

    public int PageSize
    {
        get
        {
            return Convert.ToInt32(txtPageSize.Text);
        }
        set
        {
            txtPageSize.Text = value.ToString();
        }
    }

    public int TotalRows
    {
        get
        {
            return Convert.ToInt32(lblTotalRows.Text);
        }
        set
        {
            lblTotalRows.Text = value.ToString();
        }
    }

    public int TotalPages
    {
        get
        {
            return Convert.ToInt32(lblPageCount.Text);
        }
        set
        {
            lblPageCount.Text = value.ToString();
        }
    }

    public int StartIndex
    {
        get
        {
            int startIndex = 0;
            if (this.PageIndex == 1)
                startIndex = 0;
            else
                startIndex = (this.PageIndex - 1) * this.PageSize;
            return startIndex;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (_gridView != null)
        {
            _gridView.PageIndexChanged += GridViewPageIndexChanged;
            //var GridDataSource = _gridView.FindDataSourceControl();
            var GridDataSource = _gridView.DataSource;
            //_table = GridDataSource.GetTable();
            _scriptManager = ScriptManager.GetCurrent(Page);
            if (_scriptManager != null)
                _scriptManager.Navigate += ScriptManagerOnNavigate;
        }

        if (this.Page.MasterPageFile!=null && this.Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        {
            //
            cmdFirst.Visible = false;
            cmdLast.Visible = false;
            cmdUp.Visible = false;
            cmdDown.Visible = false;
            cmdUpSize.Visible = false;
            cmdDownSize.Visible = false;
            lblPer.Text = "per ";
            lblPage.Text = "page";
            imgcach1.Visible = false;
            imgcach2.Visible = false;
            imgcach3.Visible = false;
            imgcach4.Visible = false;

            HyperAdd.ImageUrl = "Images/rrp/add.png";
            cmdAdd.ImageUrl = "Images/rrp/add.png";
            ImgDelete.Src = "Images/rrp/delete.png";
            cmdPDF.ImageUrl = "Images/rrp/file-pdf.png";
            cmdWord.ImageUrl = "Images/rrp/file-doc.png";

            cmdCSV.ImageUrl = "Images/rrp/file-csv.png";
            cmdExcel.ImageUrl = "Images/rrp/file-xls.png";

            cmdRefresh.ImageUrl = "Images/rrp/refresh.png";
            ImgEditMany.Src = "Images/rrp/edit.png";
            ImgSendEmail.Src = "Images/rrp/email.png";
            cmdPrev.ImageUrl = "Images/rrp/arrow-left.png";
            cmdNext.ImageUrl = "Images/rrp/arrow-right.png";

            trPageCount.Style.Add("color", "#ffffff");
            tdPageSizeRow.Style.Add("color", "#ffffff");
            lblPer.ForeColor = System.Drawing.Color.White;
            lblPage.ForeColor = System.Drawing.Color.White;
            if (Request.RawUrl.IndexOf("Record/RecordList.aspx") > -1)
            {
                
            }
            else
            {
                //
                divPagerGradient.Style.Add("background", "#84888E");
            }
            
          
        }
    }

    protected void GridViewPageIndexChanged(object sender, EventArgs e)
    {
        if (_scriptManager != null)
            _scriptManager.AddHistoryPoint("PageIndex", _gridView.PageIndex.ToString());
    }

    protected void ScriptManagerOnNavigate(object sender, HistoryEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.State["PageIndex"]))
            _gridView.PageIndex = int.Parse(e.State["PageIndex"]);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        txtPageIndex.Style.Add("font-size", "11px !important");
        txtPageSize.Style.Add("font-size", "11px !important");
        if (Request.QueryString["Dashboard"] != null)
        {
            _bOpenInParent = true;

            HyperAdd.Target = "_parent";
            HyperAdd2.Target = "_parent";

        }
      
        GetTheGrid();
        if (HideDelete)
        {
            DeleteLinkButton.Visible = false;
        }
        else
        {
            DeleteLinkButton.Visible = true;
        }

        if (HideUnDelete)
        {
            lnkUnDelete.Visible = false;
        }
        else
        {
            lnkUnDelete.Visible = true;
        }
        if (HideParmanentDelete)
        {
            lnkParmanentDelete.Visible = false;
        }
        else
        {
            lnkParmanentDelete.Visible = true;
        }

        if (HideEditMany)
        {
            lnkEditMany.Visible = false;
        }
        else
        {
            lnkEditMany.Visible = true;
        }

        if (IsInLine)
        {
            HyperAdd.Visible = false;
            cmdAdd.Visible = false;
        }
        else
        {
            cmdAdd.Visible = false;
            HyperAdd.Visible = true;
        }

        if (HideAdd)
        {
            HyperAdd.Visible = false;
            cmdAdd.Visible = false;
        }

      
        ShowHideSizeButtonMethod();
        

    }
    protected void ShowHideSizeButtonMethod()
    {
        if (Common.SO_ShowRecordFirstLastButtons(int.Parse(Session["AccountID"].ToString()), TableID))
        {
            HidePageSizeButton = false;
        }
        else
        {
            HidePageSizeButton = true;
        }
           
    }
    private void GetTheGrid()
    {
        Control c = Parent;
        
        while (c != null)
        {
            if (c is GridView)
            {
                _gridView = (dbgGridView)c;
                
                TotalRows = _gridView.VirtualItemCount ;
                //_gridView.TopPagerRow.Visible = true;

              
                break;
            }
            c = c.Parent;
        }
        this.TotalPages = Get_LastPage();
    }


    
  
    protected void cmdFirst_Click(object sender, ImageClickEventArgs e)
    {
        this.PageIndex = 1;
        PageIndexChanged();
    }

    protected void cmdPrev_Click(object sender, ImageClickEventArgs e)
    {
        if (_gridView.PageIndex > 0) _gridView.PageIndex = _gridView.PageIndex - 1;

        if (this.PageIndex > 1) this.PageIndex = this.PageIndex - 1;
        PageIndexChanged();
    }

    protected void cmdGoS_Click(object sender, ImageClickEventArgs e)
    {
        lnkGo_Click(null, null);
    }
    //protected void cmdGo_Click(object sender, ImageClickEventArgs e)
    protected void lnkGo_Click(object sender, EventArgs e)
    {
        if (int.Parse(txtPageIndex.Text) > this.Get_LastPage())
        {
            txtPageIndex.Text = this.Get_LastPage().ToString();
        }
        //PageIndexChanged();

    }

    protected void cmdNext_Click(object sender, ImageClickEventArgs e)
    {

        if (_gridView.PageIndex +1< Get_LastPage())
        {
            _gridView.PageIndex = _gridView.PageIndex + 1;
        } 

        if (this.PageIndex < Get_LastPage())
        {
            this.PageIndex = this.PageIndex + 1;
        }
        PageIndexChanged();
    }

    protected void cmdLast_Click(object sender, ImageClickEventArgs e)
    {
        this.PageIndex = Get_LastPage();
        PageIndexChanged();
    }

    private int Get_LastPage()
    {
        int LastPage = 0;
        if (this.TotalRows > _gridView.PageSize)
        {
            int iRem=0;
            iRem = this.TotalRows % _gridView.PageSize;
            if (iRem>0)
            {
                LastPage = Convert.ToInt32(this.TotalRows / _gridView.PageSize) + 1;
            }
            else
            {
                LastPage = Convert.ToInt32(this.TotalRows / _gridView.PageSize);
            }
            
        }
        else
        {
            LastPage = Convert.ToInt32(this.TotalRows / _gridView.PageSize);
        }

        if (LastPage == 0) LastPage = 1;
        return LastPage;
    }

    protected void cmdFilter_Click(object sender, ImageClickEventArgs e)
    {
        if (ApplyFilter != null)
            ApplyFilter(this, EventArgs.Empty);   
    
    }

    protected void cmdUp_Click(object sender, ImageClickEventArgs e)
    {
        int nPageIndex = Convert.ToInt32(txtPageIndex.Text);
        if (nPageIndex >= Get_LastPage())
        {
            PageIndexChanged(); //mohsin - to remove the blank bug
            return;
        }
        else
        {
            this.PageIndex = nPageIndex + 1;
            PageIndexChanged();
        }
    }

    protected void cmdDown_Click(object sender, ImageClickEventArgs e)
    {
        int nPageIndex = Convert.ToInt32(txtPageIndex.Text);
        if (nPageIndex <= 1)
            return;
        else
        {
            this.PageIndex = nPageIndex - 1;
            PageIndexChanged();
        }
    }
    protected void cmdAdd_Click(object sender, ImageClickEventArgs e)
    {
        int iTemp = 0;
        _gridView.EditIndex = 0;
        BindTheGridForAdd(this.StartIndex, this.StartIndex + _gridView.PageSize);
        //((LinkButton)_gridView.Rows[0].Cells[0].FindControl("lbUpdate")).Text = "Insert";
        ((HiddenField)_gridView.Rows[0].Cells[0].FindControl("hSaveMode")).Value = "Insert";

        switch (this.TableName.ToLower())
        {


            case "testtable":
                //DropDownList ddlAccountT = (DropDownList)_gridView.Rows[0].Cells[0].FindControl("ddlAccount");
                //ddlAccountT.DataSource = SecurityManager.Account_Select(null, string.Empty, null, null, "AccountName", "ASC", null, null, ref iTemp);
                //ddlAccountT.DataBind();

                break;


        }

       

    }


    protected void BindTheGridForAdd(int iStartIndex, int iMaxRows)
    {
        int iTN = 0;
        string strOrderDirection = "DESC";
        string sOrder="" ;

        if (_gridView.GridViewSortDirection == SortDirection.Ascending)
        {
            strOrderDirection = "ASC";
        }
        if (_gridView.GridViewSortColumn != "")
        {
            sOrder = _gridView.GridViewSortColumn;
        }

        switch (this.TableName.ToLower())
        {


            case "testtable":

                //if (sOrder == "")
                //    sOrder = "Menu";

                //List<Menu> listTest = SecurityManager.test_TestTable_Select(null,
                //    this.MenuName, null, this.AccountID, sOrder,
                //    strOrderDirection, this.StartIndex, Convert.ToInt32(txtPageSize.Text),
                //     ref iTN);

                //Menu emptyTest = new Menu(null, "", 2, false, "Default");
                //listTest.Insert(0, emptyTest);

                //_gridView.DataSource = listTest;

                break;

            default:
                break;
        }

        _gridView.VirtualItemCount = iTN;
        _gridView.DataBind();
        _gridView.TopPagerRow.Visible = true;  


    }
    public int PageIndexTextSet
    {
       
        set
        {
            txtPageIndex.Text = value.ToString();
            this.PageIndex = Convert.ToInt32(txtPageIndex.Text);
            if (_gridView == null)
                GetTheGrid();
            _gridView.PageIndex = int.Parse(txtPageIndex.Text) - 1;

        }
    }

    protected void txtPageIndex_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtPageIndex.Text))
            txtPageIndex.Text = "1";

        if (txtPageIndex.Text.IndexOf(",") > -1)
        {
            txtPageIndex.Text = txtPageIndex.Text.Substring(0, txtPageIndex.Text.IndexOf(","));
        }

        if (txtPageIndex.Text == "0")
            txtPageIndex.Text = "1";
        

        if (int.Parse(txtPageIndex.Text) > this.Get_LastPage())
        {
            txtPageIndex.Text = this.Get_LastPage().ToString();
        }

        this.PageIndex = Convert.ToInt32(txtPageIndex.Text);
       
        PageIndexChanged();
    }

    protected void txtPageSize_TextChanged(object sender, EventArgs e)
    {
        PageSizeChanged();
    }

    protected void cmdUpSize_Click(object sender, ImageClickEventArgs e)
    {
        int nPageSize = Convert.ToInt32(txtPageSize.Text);
        this.PageIndex = 1;
        
        this.PageSize = nPageSize + 5;
        PageSizeChanged();
        
    }

    protected void cmdDownSize_Click(object sender, ImageClickEventArgs e)
    {
        int nPageSize = Convert.ToInt32(txtPageSize.Text);
        if (nPageSize <= 5)
            return;
        this.PageIndex = 1;
        this.PageSize = nPageSize - 5;
        PageSizeChanged();
    }

    private void PageIndexChanged()
    {
        if (_gridView == null)
            GetTheGrid();

        if (_scriptManager != null)
            _scriptManager.AddHistoryPoint("PageIndex", txtPageIndex.Text);
        _gridView.EditIndex = -1;
        if (BindTheGridAgain != null)
            BindTheGridAgain(this, EventArgs.Empty);

        _gridView.PageIndex = int.Parse(txtPageIndex.Text) - 1;
        _gridView.TopPagerRow.Visible = true;

        if (HideAdd)
        {
            HyperAdd.Visible = false;
            cmdAdd.Visible = false;
        }

    }

    private void PageSizeChanged()
    {
        if (_gridView == null)
            GetTheGrid();

        if (txtPageSize.Text == "" || txtPageSize.Text == "0")
            txtPageSize.Text = _gridView.PageSize.ToString();

        _gridView.PageSize = Convert.ToInt32(txtPageSize.Text);

        // save pager size
        if (ViewState[_strTable + "_PagerSize"] != null)
            ViewState[_strTable + "_PagerSize"] = _gridView.PageSize.ToString();
        else
            ViewState.Add(_strTable + "_PagerSize", _gridView.PageSize.ToString());


        _gridView.EditIndex = -1;
        if (BindTheGridAgain != null)
            BindTheGridAgain(this, EventArgs.Empty);

        _gridView.TopPagerRow.Visible = true;

        //if (HideAdd)
        //{
        //    HyperAdd.Visible = false;
        //    cmdAdd.Visible = false;
        //}
    }

    protected void BindGridAgain()
    {
        int iTN = 0;
        string strOrderDirection = "DESC";
        if (_gridView.GridViewSortDirection == SortDirection.Ascending)
        {
            strOrderDirection = "ASC";
        }
     

        switch (this.TableName.ToLower())
        {

            

           

           
            
          



            case "testtable":
                //if (_gridView.GridViewSortColumn == "")
                //{
                //    _gridView.GridViewSortColumn = "MenuID";
                //    _gridView.GridViewSortDirection = SortDirection.Descending;
                //}

                //_gridView.DataSource = SecurityManager.test_TestTable_Select(null, this.MenuName,
                //    null, 1, _gridView.GridViewSortColumn,
                //    strOrderDirection, this.StartIndex, Convert.ToInt32(txtPageSize.Text), ref iTN);


                break;

          

            default:
                break;
        }

        _gridView.VirtualItemCount = iTN;
        _gridView.DataBind();
        _gridView.PageIndex = this.PageIndex - 1;
        _gridView.TopPagerRow.Visible = true;

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (_gridView != null)
        {

            txtPageIndex.Text = (_gridView.PageIndex + 1).ToString();
            //txtPageIndex.Text = (this.PageIndex).ToString();
            //txtPageIndex.Text = (this.PageIndexBU).ToString();
            txtPageSize.Text = _gridView.PageSize.ToString();
            txtPageIndex.ToolTip = txtPageIndex.Text;
        }
    }

    public void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    //Exports

   
    protected void cmdPDF_Click(object sender, ImageClickEventArgs e)
    {
        if (_bDOCustomPDF == true)
        {
            if (CustomExportPDF != null)
                CustomExportPDF(null, EventArgs.Empty);
        }
        else
        {
            ExportPDF();
        }
    }

    protected void cmdWord_Click(object sender, ImageClickEventArgs e)
    {
        ExportWord();
    }

    protected void cmdCSV_Click(object sender, ImageClickEventArgs e)
    {
        //ExportExcel();
        ExportCSV();
    }
    protected void cmdExcel_Click(object sender, ImageClickEventArgs e)
    {
        if (ExportForExcel != null)
            ExportForExcel(this, EventArgs.Empty);
    }



    public void ExportPDF()
    {

        _gridView.AllowPaging = false;
        if (BindTheGridToExport != null)
            BindTheGridToExport(this, EventArgs.Empty);

        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition",
         "attachment;filename=\"" + this.ExportFileName+ ".pdf\"");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        //BindGridAgainToExport();
       

        for (int i = 0; i < _gridView.Columns.Count; i++)
        {
            if (string.IsNullOrEmpty(_gridView.Columns[i].HeaderText))
                _gridView.Columns[i].Visible = false;
        }
        _gridView.RenderControl(hw);

        StringReader sr = new StringReader(sw.ToString());
        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();

        Response.Write(pdfDoc);
        Response.End();
    }
    public void ExportWord()
    {
        _gridView.AllowPaging = false;
        if (BindTheGridToExport != null)
            BindTheGridToExport(this, EventArgs.Empty);

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=\"" + this.ExportFileName + ".doc\"");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-word ";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        //BindGridAgainToExport();

      

        for (int i = 0; i < _gridView.Columns.Count; i++)
        {
            if (string.IsNullOrEmpty(_gridView.Columns[i].HeaderText))
                _gridView.Columns[i].Visible = false;
        }
        _gridView.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }
    private void ExportExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=GridViewExport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

       
       
        //BindGridAgainToExport();

        _gridView.AllowPaging = false;
        if (BindTheGridToExport != null)
            BindTheGridToExport(this, EventArgs.Empty);

        for (int i = 0; i < _gridView.Columns.Count; i++)
        {
            if (string.IsNullOrEmpty(_gridView.Columns[i].HeaderText))
                _gridView.Columns[i].Visible = false;
        }
        //Change the Header Row back to white color
        _gridView.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //Apply style to Individual Cells
        _gridView.HeaderRow.Cells[0].Style.Add("background-color", "green");
        _gridView.HeaderRow.Cells[1].Style.Add("background-color", "green");
        _gridView.HeaderRow.Cells[2].Style.Add("background-color", "green");
        _gridView.HeaderRow.Cells[3].Style.Add("background-color", "green");

        for (int i = 0; i < _gridView.Rows.Count; i++)
        {
            GridViewRow row = _gridView.Rows[i];
            //Change Color back to white
            row.BackColor = System.Drawing.Color.White;
            //Apply text style to each Row
            row.Attributes.Add("class", "textmode");
            //Apply style to Individual Cells of Alternating Row
            if (i % 2 != 0)
            {
                row.Cells[0].Style.Add("background-color", "#C2D69B");
                row.Cells[1].Style.Add("background-color", "#C2D69B");
                row.Cells[2].Style.Add("background-color", "#C2D69B");
                row.Cells[3].Style.Add("background-color", "#C2D69B");
            }
        }
        _gridView.RenderControl(hw);

        //style to format numbers to string
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }


    private void ExportCSV()
    {

        if (ExportForCSV != null)
            ExportForCSV(this, EventArgs.Empty);


        //Response.Clear();
        //Response.Buffer = true;
        //Response.AddHeader("content-disposition",
        //"attachment;filename=GridViewExport.csv");
        //Response.Charset = "";
        //Response.ContentType = "text/csv";

        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);

        //_gridView.AllowPaging = false;
        //if (BindTheGridToExport != null)
        //    BindTheGridToExport(this, EventArgs.Empty);

        //int iColCount = _gridView.Columns.Count;
        //for (int i = 0; i < iColCount; i++)
        //{
        //    if (string.IsNullOrEmpty(_gridView.Columns[i].HeaderText))
        //    {
        //    }
        //    else
        //    {
        //        sw.Write(_gridView.Columns[i].HeaderText);
        //        if (i < iColCount - 1)
        //        {
        //            sw.Write(",");
        //        }
        //    }
        //}

        //sw.Write(sw.NewLine);
          

        //// Now write all the rows.
        //foreach (GridViewRow dr in _gridView.Rows)
        //{
            
        //    for (int i = 0; i < iColCount; i++)
        //    {
        //        if (string.IsNullOrEmpty(_gridView.Columns[i].HeaderText))
        //        {
        //        }
        //        else
        //        {
        //            if (!Convert.IsDBNull(dr.Cells[i]))
        //            {
        //                sw.Write(dr.Cells[i].Text);
        //            }
        //            if (i < iColCount - 1)
        //            {
        //                sw.Write(",");
        //            }
        //        }
        //    }
        //    sw.Write(sw.NewLine);
        //}
        //sw.Close();
        
      
        ////_gridView.RenderControl(hw);

        
        //Response.Output.Write(sw.ToString());
        //Response.Flush();
        //Response.End();
    }

    protected void BindGridAgainToExport()
    {
        int iTN = 0;
        string strOrderDirection = "DESC";
        if (_gridView.GridViewSortDirection == SortDirection.Ascending)
        {
            strOrderDirection = "ASC";
        }
        _gridView.AllowPaging = false; 

        switch (this.TableName.ToLower())
        {

            

          

           

            case "testtable":
                //if (_gridView.GridViewSortColumn == "")
                //{
                //    _gridView.GridViewSortColumn = "MenuID";
                //    _gridView.GridViewSortDirection = SortDirection.Descending;
                //}

                //_gridView.DataSource = SecurityManager.test_TestTable_Select(null, this.MenuName,
                //    null, 1, _gridView.GridViewSortColumn,
                //    strOrderDirection, 0, this.TotalRows, ref iTN);
                break;

                          

            default:
                break;
        }

        _gridView.VirtualItemCount = iTN;
        _gridView.DataBind();
        //_gridView.TopPagerRow.Visible = true;

    }



    protected void cmdRefresh_Click(object sender, ImageClickEventArgs e)
    {
        if (_gridView != null)
        {
            this.PageIndex = 1;
            //this.PageIndexBU = 1;
            _gridView.PageIndex = 0;
            _gridView.PageIndex = this.PageIndex;
            PageIndexChanged();
        }
    }

    protected void lnkUnDelete_Click(object sender, EventArgs e)
    {
        if (UnDeleteAction != null)
            UnDeleteAction(this, EventArgs.Empty);

    }

    protected void lnkParmanentDelete_Click(object sender, EventArgs e)
    {
        if (ParmanenetDelAction != null)
            ParmanenetDelAction(this, EventArgs.Empty);

    }

    protected void lnkEditMany_Click(object sender, EventArgs e)
    {
        if (EditManyAction != null)
            EditManyAction(this, EventArgs.Empty);

    }

    protected void lnkCopyRecord_Click(object sender, EventArgs e)
    {
        if (CopyRecordAction != null)
            CopyRecordAction(this, EventArgs.Empty);

    }
    protected void cmdCog_Click(object sender, EventArgs e)
    {
        if (ShowCog != null)
            ShowCog(this, EventArgs.Empty);
    }
    protected void cmdAllExport_Click(object sender, EventArgs e)
    {
        if (AllExport != null)
            AllExport(this, EventArgs.Empty);

    }

    protected void lnkSendEmail_Click(object sender, EventArgs e)
    {
        if (SendEmailAction != null)
            SendEmailAction(this, EventArgs.Empty);
    }

    protected void DeleteLinkButton_Click(object sender, EventArgs e)
    {
        if (DeleteAction != null)
            DeleteAction(this, EventArgs.Empty);


        //string sCheck = "";
        //for (int i = 0; i < _gridView.Rows.Count; i++)
        //{
        //    bool ischeck = ((CheckBox)_gridView.Rows[i].FindControl("chkDelete")).Checked;
        //    if (ischeck)
        //    {
        //        sCheck = sCheck+((Label)_gridView.Rows[i].FindControl("LblID")).Text + ",";
        //    }
        //}
        //if (string.IsNullOrEmpty(sCheck))
        //{
        //    ScriptManager.RegisterClientScriptBlock(_gridView, typeof(Page), "message_alert", "alert('Please select a record.');", true);
        //    //DeleteLinkButton.Focus();
        //}
        //else
        //{
        //    //var table = DynamicDataRouteHandler.GetRequestMetaTable(Context);
        //    DeleteItem( sCheck);
        //}
    }


    private void DeleteItem( string keys)
    {

        switch (this.TableName.ToLower() )
        {

            //#region Menu
            //case "Menu":
            //    {
            //        try
            //        {
            //            if (!string.IsNullOrEmpty(keys))
            //            {

            //                foreach (string sTemp in keys.Split(','))
            //                {
            //                    if (!string.IsNullOrEmpty(sTemp))
            //                    {
                                   
            //                        RecordManager.ets_Menu_Delete( int.Parse(sTemp));

            //                    }
            //                }


            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            //DBGurus.AddErrorLog(ex.Message);
            //            ScriptManager.RegisterClientScriptBlock(_gridView, typeof(Page), "msg_delete", "alert('Delete Table Group has failed!');", true);
            //        }

            //        break;
            //    }

            //#endregion


            #region Test
            case "testtable":
                {
               
                    break;
                }

          


            #endregion


           
        }
        
        BindGridAgain();

    }
  
}
