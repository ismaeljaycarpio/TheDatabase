//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;
//using System.Globalization;
//using System.IO;
//using System.IO.Compression;
//using System.Net.Mail;
//using System.Collections.Generic;
//using System.Text;
//using System.Text.RegularExpressions;
//using DocGen.DAL;

//using AjaxControlToolkit;


using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DocGen.DAL;

using AjaxControlToolkit;


public partial class Pages_UserControl_RecordList : System.Web.UI.UserControl
{

    //Label[] _lbl;
    //TextBox[] _txtValue;   

    //DropDownList[] _ddlValue;
    //bool _bDBGSortColumnHide = false;\
    System.Xml.XmlDocument _xmlView_FC_Doc;
    string _strEqualOrGreaterOperator = " = ";
    bool _bBindWithSC = true;
    bool _bShowGraphIcon = true;
    bool _bReset = false;
    bool _bHideAllExport = false;
    bool _bDeleteReason = false;
    string _strNoAjaxView = "";
    int? _iParentRecordID = null;
    int? _iParentTableID = null;
    bool _bOpenInParent = false;
    //int? _iViewID = null;
    View _theView = null;
    string _strListType = "";
    string _strViewName = "";
    Table _theTable;
    bool _bCustomDDL = false;
    //bool _bDynamicSearch = false;
    int _iTotalDynamicColumns = 0;
    int _iMaxCharactersInCell = 250;
    Common_Pager _gvPager;
    DataTable _dtDataSource;
    DataTable _dtRecordColums;
    DataTable _dtDynamicSearchColumns;
    DataTable _dtSearchGroup;
    DataTable _dtColumnsAll;
    bool _bIsForExport = false;
    User _ObjUser;
    UserRole _theUserRole;
    Role _theRole;
    RoleTable _theRoleTable;
    string _strRecordRightID = Common.UserRoleType.None;
    string _strNumericSearch = "";
    //string _strTextSearch = "";

    //int _iStartIndex = 0;
    //int _iGridPageSize = 10;
    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "DBGSystemRecordID";
    string _strGridViewSortDirection = "DESC";
    string _qsTableID = "-1";

    string _strRecordFolder = "Record";
    //bool _bReadOnly = false;
    //public string[] formats = {"d/M/yyyy",
    //"dd/MM/yyyy","dd/M/yyyy","d/M/yy","dd/MM/yyyy","dd/M/yy","d/MM/yyyy","d/MM/yy"};
    public bool ShowTitle = true;

    public int TableID { get; set; }
    public string _strTextSearch;
    public bool _bEqualOrGreaterOperator = false;

    public string TextSearch
    {
        set
        {
            //if (_strTextSearch != value)
            //    System.Diagnostics.Debugger.Break();

            _strTextSearch = value;
        }
        get
        {
            return _strTextSearch;
        }
    }

    public string TextSearchParent { get; set; }

    public bool ShowAddButton { get; set; }
    public bool ShowEditButton { get; set; }


    public string PageType { get; set; }
    public int DetailTabIndex { get; set; }

    Account _theAccount;

    DateTime? _dtDateFrom = null;
    DateTime? _dtDateTo = null;

    string _strFilesLocation = "";
    string _strViewID = "";
    string _strViewPageType = "";
    string _strViewSession = "";
    string _strViewSessionExtra = "";
    string _strTodayShortDate = DateTime.Today.ToShortDateString();
    string _strDynamictabPart = "";
    public int SearchCriteriaID
    {
        get
        {
            if (ViewState["_iSearchCriteriaID"] != null)
            {
                return int.Parse(ViewState["_iSearchCriteriaID"].ToString());
            }
            else
            {
                return -1;
            }
        }
    }

    public int ViewID
    {
        get
        {
            if (hfViewID.Value != "")
            {
                return int.Parse(hfViewID.Value);
            }
            else
            {
                return -1;
            }
        }
    }


    public void CallSearch()
    {

        Page_Init(null, null);
        Page_Load(null, null);
        PopulateUser();
        PopulateBatch();
        lnkSearch_Click(null, null);
    }

    public string GetEditURL()
    {
        if (PageType == "p")
        {

            string strExtra = "";

            //if (Request.QueryString["viewname"] != null)
            //{
            //    strExtra = "&viewname=" + Request.QueryString["viewname"].ToString();
            //}
            if (Request.QueryString["View"] != null && Request.RawUrl.IndexOf("RecordList.aspx") > -1)
            {
                strExtra = "&View=" + Request.QueryString["View"].ToString();
            }

            return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + strExtra + "&Recordid=";
        }
        else
        {
            if (Request.QueryString["Recordid"] == null)
            {
                return "";
            }
            else
            {
                return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&parentRecordid=" + Request.QueryString["Recordid"].ToString() + "&Recordid=";
            }

        }
    }


    public string GetViewURL()
    {

        string strExtra = "";

        //if (Request.QueryString["viewname"] != null)
        //{
        //    strExtra = "&viewname=" + Request.QueryString["viewname"].ToString();
        //}
        if (Request.QueryString["View"] != null && Request.RawUrl.IndexOf("RecordList.aspx") > -1)
        {
            strExtra = "&View=" + Request.QueryString["View"].ToString();
        }

        return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + strExtra + "&Recordid=";

    }

    public string GetAccountViewURL()
    {

        return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&accountid=";

    }



   

    protected void GetRoleRight()
    {
        _ObjUser = (User)Session["User"];
        _theUserRole = (UserRole)Session["UserRole"];

        _theRole = SecurityManager.Role_Details((int)_theUserRole.RoleID);

        if (Request.QueryString["Dashboard"] != null)
        {
            _theTable.HideAdvancedOption = true;
            chkShowAdvancedOptions.Checked = false;
            chkShowAdvancedOptions.Visible = false;
            tdFilterYAxis.Visible = false;
            lblAdvancedCaption.Visible = false;
        }



        if ((bool)_theUserRole.IsAdvancedSecurity)
        {

            DataTable dtUserTable = null;
            dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
           int.Parse(_qsTableID), _theUserRole.RoleID, null);

            if (dtUserTable.Rows.Count > 0)
            {
                _strRecordRightID = dtUserTable.Rows[0]["RoleType"].ToString();
                _bHideAllExport = !bool.Parse(dtUserTable.Rows[0]["CanExport"].ToString());
                if (_theRole.IsSystemRole == null || (_theRole.IsSystemRole != null && (bool)_theRole.IsSystemRole == false))
                {
                    _theRoleTable = SecurityManager.dbg_RoleTable_Detail(int.Parse(dtUserTable.Rows[0]["RoleTableID"].ToString()));
                }
            }
        }
        else
        {
            _strRecordRightID = Session["roletype"].ToString();
        }
    }

    protected void FindTheTable()
    {

        if (PageType == "p")
        {
            if (Request.QueryString["TableID"] != null)
                TableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
        }
        else
        {
            if (Request.QueryString["RecordID"] != null)
                _iParentRecordID = int.Parse(Cryptography.Decrypt(Request.QueryString["RecordID"].ToString()));
        }


        _qsTableID = TableID.ToString();

        if (TableID != null)
        {
            _theTable = RecordManager.ets_Table_Details((int)TableID);
            if (_theTable != null)
            {
                _dtColumnsAll = RecordManager.ets_Table_Columns_All((int)_theTable.TableID);
            }
        }
    }
    protected void FindTheView()
    {
        if (Request.QueryString["View"] != null && Request.RawUrl.IndexOf("RecordList.aspx") > -1)
        {
            _strViewID = Request.QueryString["View"].ToString();
        }
        _strViewPageType = "";

        if (_theTable != null)
        {
            if (Request.RawUrl.IndexOf("RecordTableSection.aspx") > -1
                || Request.RawUrl.IndexOf("EachRecordTable.aspx") > -1)
            {
                _strViewPageType = "dash";
                if (Request.QueryString["DocumentSectionID"] != null)
                {
                    _strViewSessionExtra = "_ds" + Request.QueryString["DocumentSectionID"].ToString();
                }
                else
                {
                    if (Request.QueryString["DocumentID"] != null)
                    {
                        _strViewSessionExtra = "_d" + Request.QueryString["DocumentID"].ToString();
                    }
                }

            }
            else
            {
                if (PageType == "c")
                {
                    _strViewPageType = "child"; //same table apear more than once as a child table then
                    if (Request.QueryString["RecordID"] != null && Request.QueryString["TableID"] != null)
                    {
                        _iParentTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
                        _strViewSessionExtra = "_pt" + _iParentTableID.ToString();
                    }
                }
                if (PageType == "p")
                {
                    _strViewPageType = "list";
                }
            }
        }

        _strViewSession = "View" + _strViewPageType + _qsTableID + _strViewSessionExtra;


        if (Request.QueryString["ViewID"] != null)
        {
            _strViewID = Cryptography.Decrypt(Request.QueryString["ViewID"].ToString());
        }

        if (_strViewID == "" && Request.QueryString["View"] != null && Request.RawUrl.IndexOf("RecordList.aspx") > -1)
        {
            _strViewID = Request.QueryString["View"].ToString();
        }

        if (!IsPostBack && _strViewID != "" && (Request.QueryString["ViewID"] != null || Request.QueryString["View"] != null))
        {
            _theView = ViewManager.dbg_View_Detail(int.Parse(_strViewID));
            if (_theView == null)
            {
                Response.Redirect("~/Default.aspx", false);//
                return;
            }
        }
        if (_strViewPageType == "dash")
        {
            if (Request.QueryString["DocumentSectionID"] == null || Request.QueryString["DocumentSectionID"].ToString() == "")
            {
                Session[_strViewSession] = null;
            }

        }

        if (!IsPostBack && _strViewID == "" && Session[_strViewSession] != null)// && _strViewPageType != "dash"
        {
            _strViewID = Session[_strViewSession].ToString();
            _theView = ViewManager.dbg_View_Detail(int.Parse(_strViewID));
            if (_theView == null)
            {
                Session[_strViewSession] = null;
            }
        }

        if (_strViewID == "" && hfViewID.Value != "")// && _strViewPageType != "dash"
        {
            _strViewID = hfViewID.Value;
        }
        if (!IsPostBack && _strViewID == "" && _strViewPageType == "dash" && Request.QueryString["ViewID"] == null)
        {
            int? iNewViewID = ViewManager.dbg_View_CreateDash((int)_ObjUser.UserID, (int)_theTable.TableID);
            if (iNewViewID != null)
                _strViewID = iNewViewID.ToString();
        }

        if (_strViewID != "")
        {
            _theView = ViewManager.dbg_View_Detail(int.Parse(_strViewID));
        }
        if (_theView != null)
        {
            if ((int)_theView.TableID != (int)_theTable.TableID)
            {
                _theView = null;
                Session[_strViewSession] = null;
            }
        }
        if (_theView == null)
        {
            int? iBfViewID = ViewManager.dbg_View_BestFittingNew((int)_ObjUser.UserID, _strViewPageType, int.Parse(_qsTableID), _iParentTableID);
            if (iBfViewID != null)
            {
                _theView = ViewManager.dbg_View_Detail((int)iBfViewID);
            }
        }
        if (_theView != null)
        {
            _strListType = "view";
            if (!IsPostBack)
            {
                Session[_strViewSession] = _theView.ViewID.ToString();
                hfViewID.Value = _theView.ViewID.ToString();
            }
        }

    }

    protected void JSCode()
    {

        string strMouseoverBC = "76BAF2";
        string strCheckedBC = "96FFFF";
        string strAlterBC1 = "ffffff";
        string strAlterBC2 = "DCF2F0";

        if (this.Page.MasterPageFile != null && this.Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        {
            strMouseoverBC = "D3D9EA";
            strCheckedBC = "D3E8E4";
            strAlterBC2 = "ECECED";

        }


       


        string strMainListJS = "";



        ltScriptHere.Text = @"
                            <script type='text/javascript'>



                                function MouseEvents(objRef, evt) {
                                    var checkbox = objRef.getElementsByTagName('input')[0];



                                    if (evt.type == 'mouseover') {
                                        objRef.style.backgroundColor = '#" + strMouseoverBC + @"';
                                        objRef.style.cursor = 'pointer';
                                    }
                                    else {          

                                        if (checkbox != null && checkbox.checked) {
                                            objRef.style.backgroundColor = '#" + strCheckedBC + @"';
                                        }
                                        else if (evt.type == 'mouseout') {
                                            if (objRef.rowIndex % 2 == 0) {
                                                //Alternating Row Color
                                                objRef.style.backgroundColor = '#" + strAlterBC1 + @"';
                                            }
                                            else {
                                                objRef.style.backgroundColor = '#" + strAlterBC2 + @"';
                                            }
                                        }
                                    }
                                }
                               function toggleAndOr(t, hf) {
                                        if (t.text == 'and') {
                                            t.text = 'or';
                                        } else {
                                            t.text = 'and';
                                        }
                                        document.getElementById(hf).value = t.text;
                                    }

                                    function abc() {
                                        var b = document.getElementById('" + lnkSearch.ClientID + @"');
                                        if (b && typeof (b.click) == 'undefined') {
                                            b.click = function () {
                                                var result = true;
                                                if (b.onclick) result = b.onclick();
                                                if (typeof (result) == 'undefined' || result) {
                                                    eval(b.getAttribute('href'));
                                                }
                                            }
                                        }

                                    }

                                    function AddClick() {

                                        $('#ctl00_HomeContentPlaceHolder_rlOne_btnAdd').trigger('click');
                                    }

                                    function CreateFile() {

                                            document.getElementById('btnEmail').click();

                                        }


                                function Check_Click(objRef) {
                                    //Get the Row based on checkbox
                                    var row = objRef.parentNode.parentNode;
                                    if (objRef.checked) {
                                        //If checked change color to Aqua
                                        row.style.backgroundColor = '#" + strCheckedBC + @"';
                                    }
                                    else {
                                        //If not checked change back to original color
                                        if (row.rowIndex % 2 == 0) {
                                            //Alternating Row Color
                                            row.style.backgroundColor = '#" + strAlterBC1 + @"';

                                        }
                                        else {
                                            row.style.backgroundColor = '#" + strAlterBC2 + @"';
                                        }
                                    }

                                    //Get the reference of GridView
                                    var GridView = row.parentNode;
         
                                    //Get all input elements in Gridview
                                    var inputList = GridView.getElementsByTagName('input');
                                     var tblHR = document.getElementById('ctl00_HomeContentPlaceHolder_rlOne_gvTheGridHR');
                                     var headerCheckBox;
                                    for (var i = 0; i < inputList.length; i++) {
                                        //The First element is the Header Checkbox
                                        if(headerCheckBox==null && inputList[i].type == 'checkbox')
                                         headerCheckBox = inputList[i];

                                        //Based on all or none checkboxes
                                        //are checked check/uncheck Header Checkbox
                                        var checked = true;
                                        if(tblHR==null)
                                        {
                                            if (inputList[i].type == 'checkbox' && inputList[i] != headerCheckBox) {
                                                        if (!inputList[i].checked) {
                                                            checked = false;
                                                            break;
                                                        }
                                                    }
                                        }
                                        else
                                        {
                                            if (inputList[i].type == 'checkbox') {
                                                        if (!inputList[i].checked) {
                                                            checked = false;
                                                            break;
                                                        }
                                                    }
                                        }

           
                                    }
                                     if(tblHR==null)
                                        {
                                            if(headerCheckBox!=null)
                                                {
                                                     headerCheckBox.checked = checked;
                                                }
            
                                        }
        
                                    if (objRef.checked==false)
                                        {
                                            if(tblHR==null && headerCheckBox!=null)
                                            {
                                                headerCheckBox.checked=false;
                   
                                            } 
                                            else
                                            {
                                                 //var tbl = document.getElementById('ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid');
                                                  if(tblHR!=null)
                                                        {
                                                            var chkAll =  document.getElementById('ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid_ctl02_chkAll');
                                                            if(chkAll!=null)
                                                            {
                                                                chkAll.checked=false;
                                                                //alert(chkAll.id);
                                                            }
                                                        }         
                                            }                  
                                        }

                                }

                                function checkAll(objRef) {
                                    var GridView = objRef.parentNode.parentNode.parentNode;
                                    var inputList = GridView.getElementsByTagName('input');
                            //alert(inputList.length);
                                    for (var i = 0; i < inputList.length; i++) {
                                        //Get the Cell To find out ColumnIndex
                                        var row = inputList[i].parentNode.parentNode;
                                        if (inputList[i].type == 'checkbox' && objRef != inputList[i]) {
                                            if (objRef.checked) {
                                                //If the header checkbox is checked
                                                //check all checkboxes
                                                //and highlight all rows
                                                row.style.backgroundColor = '#" + strCheckedBC + @"';
                                                inputList[i].checked = true;
                                            }
                                            else {
                                                //If the header checkbox is checked
                                                //uncheck all checkboxes
                                                //and change rowcolor back to original 
                                                if (row.rowIndex % 2 == 0) {
                                                    //Alternating Row Color
                                                    row.style.backgroundColor = '#" + strAlterBC1 + @"';

                                                }
                                                else {
                                                    row.style.backgroundColor = '#" + strAlterBC2 + @"';
                                                }
                                                inputList[i].checked = false;
                                            }
                                        }
                                    }
                                }

                            function checkAllHR(objRef,GridView) {
                                    var inputList = GridView.getElementsByTagName('input');
                            //alert(inputList.length);
                                    for (var i = 0; i < inputList.length; i++) {
                                        //Get the Cell To find out ColumnIndex
                                        var row = inputList[i].parentNode.parentNode;
                                        if (inputList[i].type == 'checkbox' && objRef != inputList[i]) {
                                            if (objRef.checked) {
                                                //If the header checkbox is checked
                                                //check all checkboxes
                                                //and highlight all rows
                                                row.style.backgroundColor = '#" + strCheckedBC + @"';
                                                inputList[i].checked = true;
                                            }
                                            else {
                                                //If the header checkbox is checked
                                                //uncheck all checkboxes
                                                //and change rowcolor back to original 
                                                if (row.rowIndex % 2 == 0) {
                                                    //Alternating Row Color
                                                    row.style.backgroundColor = '#" + strAlterBC1 + @"';

                                                }
                                                else {
                                                    row.style.backgroundColor = '#" + strAlterBC2 + @"';
                                                }
                                                inputList[i].checked = false;
                                            }
                                        }
                                    }
                                }

                              function SelectAllCheckboxes(spanChk) {

                                        // alert($(spanChk).attr('id'));
                                        checkAll(spanChk);
                                        var GridView = spanChk.parentNode.parentNode.parentNode;

                                        //alert($(GridView).attr('id'));
                                        // alert(GridView.id);

                                        var inputList = GridView.getElementsByTagName('input');
                                        for (var i = 0; i < inputList.length; i++) {
                                            var row = inputList[i].parentNode.parentNode;
                                            if (inputList[i].type == 'checkbox' && spanChk != inputList[i]) {
                                                if (spanChk.checked) {
                                                    inputList[i].checked = true;
                                                }
                                                else {
                                                    inputList[i].checked = false;
                                                }
                                            }

                                        }
                                    }


                              function SelectAllCheckboxesHR(spanChk, GridView) {

                                        checkAllHR(spanChk, GridView);
                                        var inputList = GridView.getElementsByTagName('input');
                                        for (var i = 0; i < inputList.length; i++) {
                                            var row = inputList[i].parentNode.parentNode;
                                            if (inputList[i].type == 'checkbox' && spanChk != inputList[i]) {
                                                if (spanChk.checked) {
                                                    inputList[i].checked = true;
                                                }
                                                else {
                                                    inputList[i].checked = false;
                                                }
                                            }

                                        }
                                    }

                                   " + strMainListJS + @"



                            </script>
                            ";

    }

    protected void PopulateDynamicControls()
    {
        if (_dtDynamicSearchColumns == null)
            return;


        if (_dtDynamicSearchColumns!=null && _dtDynamicSearchColumns.Rows.Count == 0)
            pnlSearch.Visible = false;

      

        foreach (DataRow dr in _dtDynamicSearchColumns.Rows)
        {
            int iOneColumnConCount = 0;

            if (dr["ColumnType"].ToString() == "number" || dr["ColumnType"].ToString() == "calculation")
            {
                
                string strValue = "";
                string strOperator = "=";
                string strWaterViewmark = " ";
                if (_xmlView_FC_Doc != null)
                {
                    TextBox txtLowerLimit = (TextBox)tblSearchControls.FindControl("txtLowerLimit_" + dr["SystemName"].ToString());
                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    string strLowerValue = "";
                    string strUpperValue = "";
                    if (strValue != null && strValue.IndexOf("____") > -1)
                    {
                        strLowerValue = strValue.Substring(0, strValue.IndexOf("____"));
                        strUpperValue = strValue.Substring(strValue.IndexOf("____") + 4);
                    }
                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, "");
                    if (iOneColumnConCount < 2 && ThisIsOR(strWaterViewmark) == false)
                    {
                        if ((strOperator == "=" && _strEqualOrGreaterOperator == "=") || strOperator == "between")
                        {
                            TextBox txtUpperLimit = (TextBox)tblSearchControls.FindControl("txtUpperLimit_" + dr["SystemName"].ToString());
                            txtLowerLimit.Text = strLowerValue;
                            txtUpperLimit.Text = strUpperValue;

                            //ViewState[txtLowerLimit.ID + dr["ColumnID"].ToString()] = strLowerValue;
                            //ViewState[txtUpperLimit.ID + dr["ColumnID"].ToString()] = strUpperValue;

                        }
                    }
                    TextBoxWatermarkExtender twmNumberLowerLimit = (TextBoxWatermarkExtender)tblSearchControls.FindControl("twmNumberLowerLimit_" + dr["SystemName"].ToString());
                    twmNumberLowerLimit.WatermarkText = strWaterViewmark == "" ? " " : strWaterViewmark;
                    txtLowerLimit.ToolTip = strWaterViewmark;
                }
            }

            else if (dr["ColumnType"].ToString() == "text")
            {

               
                string strValue = "";
                string strOperator = "=";
                string strWaterViewmark = " ";

                if (_xmlView_FC_Doc != null)
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());

                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, "");

                    if (iOneColumnConCount < 2 && strValue != null && strOperator == "=" && ThisIsOR(strWaterViewmark) == false)
                    {
                        txtSearch.Text = strValue;
                        //ViewState[txtSearch.ID + dr["ColumnID"].ToString()] = strValue;
                    }
                    TextBoxWatermarkExtender twmTextSearch = (TextBoxWatermarkExtender)tblSearchControls.FindControl("twmTextSearch_" + dr["SystemName"].ToString());
                    twmTextSearch.WatermarkText = strWaterViewmark == "" ? " " : strWaterViewmark;
                    txtSearch.ToolTip = strWaterViewmark;
                }               
            }
            else if (dr["ColumnType"].ToString() == "date")
            {
                
                string strValue = "";
                string strOperator = "=";
                string strWaterViewmark = " ";

                if (_xmlView_FC_Doc != null)
                {
                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());

                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    string strLowerValue = "";
                    string strUpperValue = "";
                    if (strValue != null && strValue.IndexOf("____") > -1)
                    {
                        strLowerValue = strValue.Substring(0, strValue.IndexOf("____"));
                        strUpperValue = strValue.Substring(strValue.IndexOf("____") + 4);
                    }

                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, "");

                    if (iOneColumnConCount < 2 && ThisIsOR(strWaterViewmark) == false)
                    {
                        if ((strOperator == "=" && _strEqualOrGreaterOperator == "=") || strOperator == "between")
                        {
                            TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());
                            txtLowerDate.Text = strLowerValue;
                            txtUpperDate.Text = strUpperValue;

                            //ViewState[txtLowerDate.ID + dr["ColumnID"].ToString()] = strLowerValue;
                            //ViewState[txtUpperDate.ID + dr["ColumnID"].ToString()] = strUpperValue;
                        }
                    }

                    if (strWaterViewmark.Trim() != "")
                    {
                        TextBoxWatermarkExtender twmLowerDate = (TextBoxWatermarkExtender)tblSearchControls.FindControl("twmLowerDate_" + dr["SystemName"].ToString());
                        twmLowerDate.WatermarkText = strWaterViewmark;
                        txtLowerDate.ToolTip = strWaterViewmark;
                    }
                }
            }
            else if (dr["ColumnType"].ToString() == "datetime")
            {
                              
                string strValue = "";

                string strOperator = "=";
                string strWaterViewmark = " ";

                if (_xmlView_FC_Doc != null)
                {

                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());
                    TextBoxWatermarkExtender twmLowerDate = (TextBoxWatermarkExtender)tblSearchControls.FindControl("twmLowerDate_" + dr["SystemName"].ToString());

                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    string strLowerDatetime = "";
                    string strUpperDateTime = "";

                    if (strValue != null && strValue.IndexOf("____") > -1)
                    {
                        strLowerDatetime = strValue.Substring(0, strValue.IndexOf("____"));
                        strUpperDateTime = strValue.Substring(strValue.IndexOf("____") + 4);

                    }

                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, "");
                    if (iOneColumnConCount < 2 && ThisIsOR(strWaterViewmark) == false)
                    {
                        if ((strOperator == "=" && _strEqualOrGreaterOperator == "=") || strOperator == "between")
                        {
                            if (strLowerDatetime.IndexOf(" ") > -1)
                            {
                                txtLowerDate.Text = strLowerDatetime.Substring(0, strLowerDatetime.IndexOf(" "));
                                txtLowerTime.Text = strLowerDatetime.Substring(strLowerDatetime.IndexOf(" ") + 1);

                                //ViewState[txtLowerDate.ID + dr["ColumnID"].ToString()] = txtLowerDate.Text;
                                //ViewState[txtLowerTime.ID + dr["ColumnID"].ToString()] = txtLowerTime.Text;
                            }

                            if (strUpperDateTime.IndexOf(" ") > -1)
                            {
                                txtUpperDate.Text = strUpperDateTime.Substring(0, strUpperDateTime.IndexOf(" "));
                                txtUpperTime.Text = strUpperDateTime.Substring(strUpperDateTime.IndexOf(" ") + 1);

                                //ViewState[txtUpperDate.ID + dr["ColumnID"].ToString()] = txtUpperDate.Text;
                                //ViewState[txtUpperTime.ID + dr["ColumnID"].ToString()] = txtUpperTime.Text;
                            }
                        }
                    }

                    if (strWaterViewmark.Trim() != "")
                    {
                        twmLowerDate.WatermarkText = strWaterViewmark;
                        txtLowerDate.ToolTip = strWaterViewmark;
                    }
                }
            }
            else if (dr["ColumnType"].ToString() == "time")
            {
                                
                string strValue = "";
                string strOperator = "=";
                string strWaterViewmark = " ";

                if (_xmlView_FC_Doc != null)
                {
                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    string strLowertime = "";
                    string strUpperTime = "";

                    if (strValue != null && strValue.IndexOf("____") > -1)
                    {
                        strLowertime = strValue.Substring(0, strValue.IndexOf("____"));
                        strUpperTime = strValue.Substring(strValue.IndexOf("____") + 4);

                    }

                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, "");
                    if (iOneColumnConCount < 2 && ThisIsOR(strWaterViewmark) == false)
                    {
                        if ((strOperator == "=" && _strEqualOrGreaterOperator == "=") || strOperator == "between")
                        {
                            txtLowerTime.Text = strLowertime;
                            txtUpperTime.Text = strUpperTime;

                            //ViewState[txtLowerTime.ID + dr["ColumnID"].ToString()] = txtLowerTime.Text;
                            //ViewState[txtUpperTime.ID + dr["ColumnID"].ToString()] = txtUpperTime.Text;
                        }
                    }
                    TextBoxWatermarkExtender twmLowerTime = (TextBoxWatermarkExtender)tblSearchControls.FindControl("twmTime_" + dr["SystemName"].ToString());
                   
                    twmLowerTime.WatermarkText = strWaterViewmark.Trim() == "" ? "hh:mm" : strWaterViewmark;
                    txtLowerTime.ToolTip = strWaterViewmark;                    
                }


            }
            else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "values"
                || dr["DropDownType"].ToString() == "value_text"))
            {
                DropDownList ddlSearch =(DropDownList) tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                string strVT = "";
                if (dr["DropDownType"].ToString() == "values")
                {
                    Common.PutDDLValues(dr["DropdownValues"].ToString(), ref ddlSearch);
                }
                else
                {
                    strVT = dr["DropdownValues"].ToString();
                    Common.PutDDLValue_Text(dr["DropdownValues"].ToString(), ref ddlSearch);
                }

                string strValue = "";
                string strOperator = "=";
                string strWaterViewmark = " ";

                if (_xmlView_FC_Doc != null)
                {
                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());

                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, strVT);

                    if (strWaterViewmark != "")
                    {
                        if (ddlSearch.Items.Count > 0)
                        {
                            ddlSearch.Items.RemoveAt(0);

                            ListItem liViewCondition = new ListItem(strWaterViewmark, "");
                            ddlSearch.Items.Insert(0, liViewCondition);

                            ListItem liSelect = new ListItem("--Please Select--", "vf_vf_vf");
                            ddlSearch.Items.Insert(1, liSelect);

                            ddlSearch.ToolTip = strWaterViewmark;
                        }
                    }


                    if (iOneColumnConCount < 2 && strValue != null && strOperator == "=" && ThisIsOR(strWaterViewmark) == false)
                    {
                        if (ddlSearch.Items.FindByValue(strValue) != null)
                        {
                            ddlSearch.SelectedValue = strValue;
                            ViewState[ddlSearch.ID + dr["ColumnID"].ToString()] = strValue;
                        }
                    }
                }
               
            }
            else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "table" || dr["DropDownType"].ToString() == "tabledd") &&
            dr["TableTableID"] != DBNull.Value && dr["DisplayColumn"].ToString() != "")
            {
                DropDownList ddlParentSearch = (DropDownList)tblSearchControls.FindControl("ddlParentSearch_" + dr["SystemName"].ToString());


                RecordManager.PopulateTableDropDown(int.Parse(dr["ColumnID"].ToString()), ref ddlParentSearch);

                string strVT = "ColumnID";

                string strValue = "";
                string strOperator = "=";
                string strWaterViewmark = " ";
                if (_xmlView_FC_Doc != null)
                {

                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());

                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, strVT);

                    if (strWaterViewmark != "")
                    {
                        if (ddlParentSearch.Items.Count > 0)
                        {
                            ddlParentSearch.Items.RemoveAt(0);

                            ListItem liViewCondition = new ListItem(strWaterViewmark, "");
                            ddlParentSearch.Items.Insert(0, liViewCondition);

                            ListItem liSelect = new ListItem("--Please Select--", "vf_vf_vf");
                            ddlParentSearch.Items.Insert(1, liSelect);


                            ddlParentSearch.ToolTip = strWaterViewmark;
                        }
                    }



                    if (strValue == "-user-" && dr["LinkedParentColumnID"] != DBNull.Value)
                    {
                        string strColumnuserID = Common.GetValueFromSQL(@"SELECT TOP 1 ColumnID FROM [Column] WHERE TableID=" + dr["TableTableID"].ToString() + @" AND 
                                ColumnType='dropdown' AND DisplayColumn IS NOT NULL
                                            AND TableTableID=-1");

                        if (strColumnuserID != "")
                        {
                            string strLoginText = "";
                            SecurityManager.ProcessLoginUserDefault(dr["TableTableID"].ToString(), "",
                            dr["LinkedParentColumnID"].ToString(), _ObjUser.UserID.ToString(), ref strValue, ref strLoginText);

                        }
                    }

                    if (iOneColumnConCount < 2 && strValue != null && strOperator == "=" && ThisIsOR(strWaterViewmark) == false)
                    {
                        if (!IsPostBack)
                        {
                            if (ddlParentSearch.Items.FindByValue(strValue) != null)
                            {
                                ddlParentSearch.SelectedValue = strValue;
                                //ViewState[ddlParentSearch.ID + dr["ColumnID"].ToString()] = strValue;
                            }

                        }

                    }



                }
            }
            else if (dr["ColumnType"].ToString() == "radiobutton" || dr["ColumnType"].ToString() == "checkbox" ||
                dr["ColumnType"].ToString() == "listbox")
            {

                DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                string strVT = "";

                if (dr["ColumnType"].ToString() == "radiobutton")
                {
                    if (dr["DropDownType"].ToString() == "values")
                    {
                        TheDatabase.PutDDLValues(dr["DropdownValues"].ToString(), ref ddlSearch);
                    }
                    else if (dr["DropDownType"].ToString() == "value_text")
                    {
                        strVT = dr["DropdownValues"].ToString();
                        TheDatabase.PutDDLValue_Text(dr["DropdownValues"].ToString(), ref ddlSearch);
                    }
                    else
                    {
                        //strVT
                        Common.PutRadioImageInto_DDL(dr["DropdownValues"].ToString(), ref ddlSearch);
                    }

                }
                else if (dr["ColumnType"].ToString() == "listbox")
                {
                    if (dr["DropDownType"].ToString() == "values")
                    {
                        TheDatabase.PutDDLValues(dr["DropdownValues"].ToString(), ref ddlSearch);
                    }
                    else if (dr["DropDownType"].ToString() == "value_text")
                    {
                        strVT = dr["DropdownValues"].ToString();
                        TheDatabase.PutDDLValue_Text(dr["DropdownValues"].ToString(), ref ddlSearch);
                    }
                    else
                    {
                        if (dr["DropDownType"].ToString() == "table" && dr["TableTableID"] != DBNull.Value
                          && dr["DisplayColumn"].ToString() != "") //&& dr["LinkedParentColumnID"] != DBNull.Value
                        {
                            strVT = "ColumnID";
                            RecordManager.PopulateTableDropDown(int.Parse(dr["ColumnID"].ToString()), ref ddlSearch);
                        }
                    }
                }
                else
                {
                    TheDatabase.PutCheckboxIntoDDL(dr["DropdownValues"].ToString(), ref ddlSearch);
                }


                string strValue = "";
                string strOperator = "=";
                string strWaterViewmark = " ";

                if (_xmlView_FC_Doc != null)
                {
                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());

                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, strVT);
                    if (strWaterViewmark != "")
                    {
                        if (ddlSearch.Items.Count > 0)
                        {
                            ddlSearch.Items.RemoveAt(0);

                            ListItem liViewCondition = new ListItem(strWaterViewmark, "");
                            ddlSearch.Items.Insert(0, liViewCondition);
                            ListItem liSelect = new ListItem("--Please Select--", "vf_vf_vf");
                            ddlSearch.Items.Insert(1, liSelect);

                            ddlSearch.ToolTip = strWaterViewmark;
                        }
                    }


                    if (iOneColumnConCount < 2 && strValue != null && strOperator == "=" && ThisIsOR(strWaterViewmark) == false)
                    {
                        if (!IsPostBack)
                        {
                            if (ddlSearch.Items.FindByValue(strValue) != null)
                            {
                                ddlSearch.SelectedValue = strValue;
                                //ViewState[ddlSearch.ID + dr["ColumnID"].ToString()] = strValue;
                            }
                        }

                    }
                }

            }
            else
            {

                TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                TextBoxWatermarkExtender twmTextSearch = (TextBoxWatermarkExtender)tblSearchControls.FindControl("twmTextSearch_" + dr["SystemName"].ToString());
                string strValue = "";
                string strOperator = "=";
                string strWaterViewmark = " ";

                if (_xmlView_FC_Doc != null)
                {
                    strValue = GetValueFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());
                    strOperator = GetOperatorFromViewFilter(_xmlView_FC_Doc, dr["ColumnID"].ToString());

                    strWaterViewmark = GetMultipleCondition(_xmlView_FC_Doc, dr["ColumnID"].ToString(), ref iOneColumnConCount, "");

                    if (iOneColumnConCount < 2 && strValue != null && strOperator == "=" && ThisIsOR(strWaterViewmark) == false)
                    {
                        txtSearch.Text = strValue;
                        //ViewState[txtSearch.ID + dr["ColumnID"].ToString()] = strValue;
                    }

                    twmTextSearch.WatermarkText = strWaterViewmark == "" ? " " : strWaterViewmark;
                    txtSearch.ToolTip = strWaterViewmark;
                    //txtSearch.Text = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                }                               
            }


        }         
       

    }


    protected void CreateDynamicControls()
    {
        string strOnlyAdmin = "";
        if (!Common.HaveAccess(_strRecordRightID, "1,2"))
        {
            strOnlyAdmin = " AND (OnlyForAdmin<>1 OR OnlyForAdmin IS NULL) ";
        }

        if (_theView == null)
        {
            _dtDynamicSearchColumns = Common.DataTableFromText("SELECT C.*,c.DisplayTextSummary as Heading FROM [Column] C WHERE TableID=" + TableID.ToString()
                + strOnlyAdmin + " AND SummarySearch=1  AND ColumnID NOT IN (SELECT ColumnID FROM SearchGroupColumn SGC INNER JOIN SearchGroup SG " +
                     " ON SGC.SearchGroupID=SG.SearchGroupID WHERE TableID=" + TableID.ToString() + " ) ORDER BY DisplayOrder");
        }
        else
        {

            if ((bool)_theView.ShowSearchFields == false)
            {
                strOnlyAdmin = strOnlyAdmin + " AND C.ColumnID=-1 ";
                //if (Request.QueryString["Dashboard"] != null)
                //{
                //    pnlSearch.Visible = false;
                //}
            }


            _dtDynamicSearchColumns = Common.DataTableFromText(@"SELECT C.*,ISNULL(C.DisplayTextSummary,C.DisplayName) as Heading FROM [Column] C 
                INNER JOIN ViewItem VT ON C.ColumnID=VT.ColumnID
                WHERE VT.ViewID=" + _theView.ViewID.ToString() + @" AND C.TableID=" + TableID.ToString() + strOnlyAdmin +
                  @" AND VT.SearchField=1  AND VT.ColumnID NOT IN 
                 (SELECT ColumnID FROM SearchGroupColumn SGC INNER JOIN SearchGroup SG 
                  ON SGC.SearchGroupID=SG.SearchGroupID WHERE TableID=" + TableID.ToString() + @" ) ORDER BY ColumnIndex");


        }

        

        _dtSearchGroup = Common.DataTableFromText("SELECT SearchGroupID,GroupName FROM SearchGroup WHERE SummarySearch=1  AND TableID=" + TableID.ToString() + " ORDER BY DisplayOrder");





        _xmlView_FC_Doc = new System.Xml.XmlDocument();

        if (_theView.Filter != "" && _theView.FilterControlsInfo != "")
        {

            _xmlView_FC_Doc.Load(new StringReader(_theView.FilterControlsInfo));

        }

        HtmlTableRow theRow = new HtmlTableRow();
        int s = 0;

        string strJSDynamicShowHide = "";

        foreach (DataRow dr in _dtDynamicSearchColumns.Rows)
        {
            int iOneColumnConCount = 0;

            if (dr["ColumnType"].ToString() == "number" || dr["ColumnType"].ToString() == "calculation")
            {

                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }

                s = s + 1;
                TextBox txtLowerLimit = new TextBox();
                txtLowerLimit.ID = "txtLowerLimit_" + dr["SystemName"].ToString();
                txtLowerLimit.CssClass = "NormalTextBox";
                txtLowerLimit.Width = 105;
                txtLowerLimit.Attributes.Add("onblur", "this.value=this.value.trim()");

                TextBox txtUpperLimit = new TextBox();
                txtUpperLimit.ID = "txtUpperLimit_" + dr["SystemName"].ToString();
                txtUpperLimit.CssClass = "NormalTextBox";
                txtUpperLimit.Width = 105;
                txtUpperLimit.Attributes.Add("placeholder", "To Upper Limit");
                txtUpperLimit.Attributes.Add("onblur", "this.value=this.value.trim()");


                TextBoxWatermarkExtender twmNumberLowerLimit = new TextBoxWatermarkExtender();
                twmNumberLowerLimit.ID = "twmNumberLowerLimit_" + dr["SystemName"].ToString();
                twmNumberLowerLimit.TargetControlID = txtLowerLimit.ID;
                twmNumberLowerLimit.WatermarkText = " ";
                twmNumberLowerLimit.WatermarkCssClass = "MaskText";

                RegularExpressionValidator revNumberLowerLimit = new RegularExpressionValidator();
                revNumberLowerLimit.ID = "revNumberLowerLimit_" + dr["SystemName"].ToString();
                revNumberLowerLimit.ControlToValidate = txtLowerLimit.ID;
                revNumberLowerLimit.ErrorMessage = "*";
                revNumberLowerLimit.ToolTip = "Number only!";
                revNumberLowerLimit.ValidationExpression = @"(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)";

                RegularExpressionValidator revNumberUpperLimit = new RegularExpressionValidator();
                revNumberUpperLimit.ID = "revNumberUpperLimit_" + dr["SystemName"].ToString();
                revNumberUpperLimit.ControlToValidate = txtUpperLimit.ID;
                revNumberUpperLimit.ErrorMessage = "*";
                revNumberUpperLimit.ToolTip = "Number only!";
                revNumberUpperLimit.ValidationExpression = @"(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)";

                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));


                HtmlTable tblDate = new HtmlTable();
                HtmlTableRow rowOne = new HtmlTableRow();
                HtmlTableRow rowTwo = new HtmlTableRow();

                rowOne.ID = "numrowOne_" + dr["SystemName"].ToString();
                rowTwo.ID = "numrowTwo_" + dr["SystemName"].ToString();

                tblDate.CellPadding = 0;
                tblDate.CellSpacing = 0;


                HtmlTableCell cellBox1 = new HtmlTableCell();
                HtmlTableCell cellBox12 = new HtmlTableCell();
                HtmlTableCell cellBox2 = new HtmlTableCell();
                HtmlTableCell cellBox22 = new HtmlTableCell();


                cellBox1.Controls.Add(txtLowerLimit);
                cellBox12.Controls.Add(revNumberLowerLimit);
                cellBox12.Controls.Add(twmNumberLowerLimit);

                //cellBox.Controls.Add(new LiteralControl("</br>"));
                txtUpperLimit.Style.Add("margin-top", "5px");


                cellBox2.Controls.Add(txtUpperLimit);
                cellBox22.Controls.Add(revNumberUpperLimit);
                //cellBox22.Controls.Add(twmNumberUpperLimit);

                rowOne.Cells.Add(cellBox1);
                rowOne.Cells.Add(cellBox12);

                rowTwo.Cells.Add(cellBox2);
                rowTwo.Cells.Add(cellBox22);

                tblDate.Rows.Add(rowOne);
                tblDate.Rows.Add(rowTwo);
                cellBox.Controls.Add(tblDate);





                theRow.Cells.Add(cellBox);


                //                strJSDynamicShowHide = strJSDynamicShowHide + @"
                //                        $('#" + _strDynamictabPart + txtLowerLimit.ID + @"').on('keyup',function () {
                //                                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerLimit.ID + @"').val();
                //                                                                 if (strLowerValue.trim() != '') {
                //                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeIn();
                //                                                                }
                //                                                                else {
                //                                                                     $('#" + _strDynamictabPart + txtUpperLimit.ID + @"').val('');
                //                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeOut(); 
                //                                                                }
                //                                                            });
                //                         $('#" + _strDynamictabPart + txtLowerLimit.ID + @"').trigger('keyup');
                //
                //                    ";



                //if (strLowerValue.trim() != '') {     and style.display<>'none' if we want to avoid
                strJSDynamicShowHide = strJSDynamicShowHide + @"
                    function ShowHideUP_" + _strDynamictabPart + txtUpperLimit.ID + @"(keepvalue) {                                                               
                            try
                            {
                                    var strLowerValue = $('#" + _strDynamictabPart + txtLowerLimit.ID + @"').val();
                                                                  
                                    if (strLowerValue.trim() != '') {                        
                                        $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeIn();
                                         var strUpperValue = $('#" + _strDynamictabPart + txtUpperLimit.ID + @"').val();
                                         if (strUpperValue.trim() == '' && keepvalue=='0') {
                                          $('#" + _strDynamictabPart + txtUpperLimit.ID + @"').val(strLowerValue);
                                        }               
                                    }
                                    else {
                                            $('#" + _strDynamictabPart + txtUpperLimit.ID + @"').val('');
                                            $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeOut();
                                    }
                            }
                            catch(err)
                            {
                                 //
                            }
                         };

                          
                            $('#" + _strDynamictabPart + txtLowerLimit.ID + @"').on('keyup',function () {
                                    ShowHideUP_" + _strDynamictabPart + txtUpperLimit.ID + @"('1');                                                          
                            });
                             $('#" + _strDynamictabPart + txtLowerLimit.ID + @"').change(function () {
                                    ShowHideUP_" + _strDynamictabPart + txtUpperLimit.ID + @"('0');                                                          
                            });
                        ShowHideUP_" + _strDynamictabPart + txtUpperLimit.ID + @"('1');  

                                ";



            }

            else if (dr["ColumnType"].ToString() == "text")
            {

                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }

                s = s + 1;
                TextBox txtSearch = new TextBox();
                txtSearch.ID = "txtSearch_" + dr["SystemName"].ToString();
                txtSearch.CssClass = "NormalTextBox";
                txtSearch.Width = 105;
                txtSearch.Attributes.Add("onblur", "this.value=this.value.trim()");

                TextBoxWatermarkExtender twmTextSearch = new TextBoxWatermarkExtender();
                twmTextSearch.ID = "twmTextSearch_" + dr["SystemName"].ToString();
                twmTextSearch.TargetControlID = txtSearch.ID;
                twmTextSearch.WatermarkText = " ";
                twmTextSearch.WatermarkCssClass = "MaskText";
                                
                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));
                cellBox.Controls.Add(txtSearch);
                cellBox.Controls.Add(twmTextSearch);

                theRow.Cells.Add(cellBox);
            }
            else if (dr["ColumnType"].ToString() == "date")
            {

                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }

                s = s + 1;
                TextBox txtLowerDate = new TextBox();
                txtLowerDate.ID = "txtLowerDate_" + dr["SystemName"].ToString();
                txtLowerDate.CssClass = "NormalTextBox";
                txtLowerDate.Width = 105;
                txtLowerDate.AutoCompleteType = AutoCompleteType.Disabled;
                txtLowerDate.Attributes.Add("onblur", "this.value=this.value.trim()");
                txtLowerDate.ToolTip = Common.ToolTip_Today;

                TextBox txtUpperDate = new TextBox();
                txtUpperDate.ID = "txtUpperDate_" + dr["SystemName"].ToString();
                txtUpperDate.CssClass = "NormalTextBox";
                txtUpperDate.Width = 105;
                txtUpperDate.AutoCompleteType = AutoCompleteType.Disabled;
                txtUpperDate.Attributes.Add("placeholder", "To dd/mm/yyyy");
                //txtUpperDate.Attributes.Add("onfocus", "if(this.value=='To dd/mm/yyyy') {this.value = '';this.style.color='black';}");
                txtUpperDate.Attributes.Add("onblur", "this.value=this.value.trim()");
                txtUpperDate.ToolTip = Common.ToolTip_Today;

                ImageButton imgLowerDate = new ImageButton();
                imgLowerDate.ID = "imgLowerDate_" + dr["SystemName"].ToString();
                imgLowerDate.ImageUrl = "~/Images/Calendar.png";
                imgLowerDate.Style.Add("padding-left", "3px");
                imgLowerDate.CausesValidation = false;


                CalendarExtender ceLowerDate = new CalendarExtender();
                ceLowerDate.ID = "ceLowerDate_" + dr["SystemName"].ToString();
                ceLowerDate.TargetControlID = txtLowerDate.ID;
                ceLowerDate.Format = "dd/MM/yyyy";
                ceLowerDate.PopupButtonID = imgLowerDate.ID;
                ceLowerDate.FirstDayOfWeek = FirstDayOfWeek.Monday;



                TextBoxWatermarkExtender twmLowerDate = new TextBoxWatermarkExtender();
                twmLowerDate.ID = "twmLowerDate_" + dr["SystemName"].ToString();
                twmLowerDate.TargetControlID = txtLowerDate.ID;
                twmLowerDate.WatermarkText = "dd/mm/yyyy";
                twmLowerDate.WatermarkCssClass = "MaskText";


                




                ImageButton imgUpperDate = new ImageButton();
                imgUpperDate.ID = "imgUpperDate_" + dr["SystemName"].ToString();
                imgUpperDate.ImageUrl = "~/Images/Calendar.png";
                imgUpperDate.Style.Add("padding-left", "3px");
                imgUpperDate.CausesValidation = false;


                CalendarExtender ceUpperDate = new CalendarExtender();
                ceUpperDate.ID = "ceUpperDate_" + dr["SystemName"].ToString();
                ceUpperDate.TargetControlID = txtUpperDate.ID;
                ceUpperDate.Format = "dd/MM/yyyy";
                ceUpperDate.PopupButtonID = imgUpperDate.ID;
                ceUpperDate.FirstDayOfWeek = FirstDayOfWeek.Monday;



                //TextBoxWatermarkExtender twmUpperDate = new TextBoxWatermarkExtender();
                //twmUpperDate.ID = "twmUpperDate_" + dr["SystemName"].ToString();
                //twmUpperDate.TargetControlID = txtUpperDate.ID;
                //twmUpperDate.WatermarkText = "To dd/mm/yyyy";
                //twmUpperDate.WatermarkCssClass = "MaskText";
                //twmUpperDate.BehaviorID = _strDynamictabPart + "BIDtwmUpperDate_" + dr["SystemName"].ToString();



                HtmlTable tblDate = new HtmlTable();
                HtmlTableRow rowOne = new HtmlTableRow();
                HtmlTableRow rowTwo = new HtmlTableRow();

                rowOne.ID = "rowOne_" + dr["SystemName"].ToString();
                rowTwo.ID = "rowTwo_" + dr["SystemName"].ToString();

                tblDate.CellPadding = 0;
                tblDate.CellSpacing = 0;

                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));

                HtmlTableCell cellBox1 = new HtmlTableCell();
                HtmlTableCell cellBox12 = new HtmlTableCell();
                HtmlTableCell cellBox2 = new HtmlTableCell();
                HtmlTableCell cellBox22 = new HtmlTableCell();

                cellBox1.Controls.Add(txtLowerDate);
                cellBox12.Controls.Add(imgLowerDate);
                cellBox12.Controls.Add(ceLowerDate);
                cellBox12.Controls.Add(twmLowerDate);
                //cellBox12.Controls.Add(rvLowerDate);
                rowOne.Cells.Add(cellBox1);
                rowOne.Cells.Add(cellBox12);
                //cellBox.Controls.Add(new LiteralControl("</br>"));
                txtUpperDate.Style.Add("margin-top", "5px");
                imgUpperDate.Style.Add("margin-top", "5px");

                cellBox2.Controls.Add(txtUpperDate);
                cellBox22.Controls.Add(imgUpperDate);
                cellBox22.Controls.Add(ceUpperDate);
                //cellBox22.Controls.Add(twmUpperDate);
                //cellBox22.Controls.Add(rvUpperDate);

                rowTwo.Cells.Add(cellBox2);
                rowTwo.Cells.Add(cellBox22);




                tblDate.Rows.Add(rowOne);
                tblDate.Rows.Add(rowTwo);
                cellBox.Controls.Add(tblDate);
                theRow.Cells.Add(cellBox);



                //                strJSDynamicShowHide = strJSDynamicShowHide + @"
                //                        $('#" + _strDynamictabPart + txtLowerDate.ID + @"').on('keyup',function () {
                //                                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerDate.ID + @"').val();
                //                                                                 if (strLowerValue.trim() != '') {
                //                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeIn();
                //                                                                }
                //                                                                else {
                //                                                                     $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val('');
                //                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeOut(); 
                //                                                                }
                //                                                            });
                //                         $('#" + _strDynamictabPart + txtLowerDate.ID + @"').trigger('keyup');
                //
                //                    ";

                strJSDynamicShowHide = strJSDynamicShowHide + @"


                                function ShowHideUP_" + _strDynamictabPart + txtUpperDate.ID + @"(keepvalue) {                                                               
                                        try
                                        {
                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerDate.ID + @"').val();
                                                                  
                                                if (strLowerValue.trim() != '') {                        
                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeIn();
                                                     var strUpperValue = $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val();
                                                     if (strUpperValue.trim() == '' && keepvalue=='0') {
                                                      $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val(strLowerValue);
                                                    }               
                                                }
                                                else {
                                                        $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val('');
                                                        $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeOut();
                                                }
                                        }
                                        catch(err)
                                        {
                                             //
                                        }
                                     };

                                        $('#" + _strDynamictabPart + txtLowerDate.ID + @"').change(function () {
                                                ShowHideUP_" + _strDynamictabPart + txtUpperDate.ID + @"('0');                                                          
                                        });
                                         $('#" + _strDynamictabPart + txtLowerDate.ID + @"').on('keyup',function () {
                                                ShowHideUP_" + _strDynamictabPart + txtUpperDate.ID + @"('1');                                                          
                                        });
                                    ShowHideUP_" + _strDynamictabPart + txtUpperDate.ID + @"('1');  

                                            ";

            }
            else if (dr["ColumnType"].ToString() == "datetime")
            {

                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }

                s = s + 1;
                TextBox txtLowerDate = new TextBox();
                txtLowerDate.ID = "txtLowerDate_" + dr["SystemName"].ToString();
                txtLowerDate.CssClass = "NormalTextBox";
                txtLowerDate.Width = 105;
                txtLowerDate.AutoCompleteType = AutoCompleteType.Disabled;
                txtLowerDate.Attributes.Add("onblur", "this.value=this.value.trim()");
                txtLowerDate.ToolTip = Common.ToolTip_Today;

                TextBox txtUpperDate = new TextBox();
                txtUpperDate.ID = "txtUpperDate_" + dr["SystemName"].ToString();
                txtUpperDate.CssClass = "NormalTextBox";
                txtUpperDate.Width = 105;
                txtUpperDate.AutoCompleteType = AutoCompleteType.Disabled;
                txtUpperDate.Attributes.Add("placeholder", "To dd/mm/yyyy");
                txtUpperDate.Attributes.Add("onblur", "this.value=this.value.trim()");
                txtUpperDate.ToolTip = Common.ToolTip_Today;


                TextBox txtLowerTime = new TextBox();
                txtLowerTime.ID = "txtLowerTime_" + dr["SystemName"].ToString();
                txtLowerTime.CssClass = "NormalTextBox";
                txtLowerTime.Width = 70;
                txtLowerTime.AutoCompleteType = AutoCompleteType.Disabled;
                txtLowerTime.Attributes.Add("placeholder", "hh:mm");

                TextBox txtUpperTime = new TextBox();
                txtUpperTime.ID = "txtUpperTime_" + dr["SystemName"].ToString();
                txtUpperTime.CssClass = "NormalTextBox";
                txtUpperTime.Width = 70;
                txtUpperTime.AutoCompleteType = AutoCompleteType.Disabled;
                txtUpperTime.Attributes.Add("placeholder", "hh:mm");




                ImageButton imgLowerDate = new ImageButton();
                imgLowerDate.ID = "imgLowerDate_" + dr["SystemName"].ToString();
                imgLowerDate.ImageUrl = "~/Images/Calendar.png";
                imgLowerDate.Style.Add("padding-left", "3px");
                imgLowerDate.CausesValidation = false;


                CalendarExtender ceLowerDate = new CalendarExtender();
                ceLowerDate.ID = "ceLowerDate_" + dr["SystemName"].ToString();
                ceLowerDate.TargetControlID = txtLowerDate.ID;
                ceLowerDate.Format = "dd/MM/yyyy";
                ceLowerDate.PopupButtonID = imgLowerDate.ID;
                ceLowerDate.FirstDayOfWeek = FirstDayOfWeek.Monday;



                TextBoxWatermarkExtender twmLowerDate = new TextBoxWatermarkExtender();
                twmLowerDate.ID = "twmLowerDate_" + dr["SystemName"].ToString();
                twmLowerDate.TargetControlID = txtLowerDate.ID;
                twmLowerDate.WatermarkText = "dd/mm/yyyy";
                twmLowerDate.WatermarkCssClass = "MaskText";
                
                ImageButton imgUpperDate = new ImageButton();
                imgUpperDate.ID = "imgUpperDate_" + dr["SystemName"].ToString();
                imgUpperDate.ImageUrl = "~/Images/Calendar.png";
                imgUpperDate.Style.Add("padding-left", "3px");
                imgUpperDate.CausesValidation = false;


                CalendarExtender ceUpperDate = new CalendarExtender();
                ceUpperDate.ID = "ceUpperDate_" + dr["SystemName"].ToString();
                ceUpperDate.TargetControlID = txtUpperDate.ID;
                ceUpperDate.Format = "dd/MM/yyyy";
                ceUpperDate.PopupButtonID = imgUpperDate.ID;
                ceUpperDate.FirstDayOfWeek = FirstDayOfWeek.Monday;

                
                MaskedEditExtender meeLowerTime = new MaskedEditExtender();
                meeLowerTime.ID = "meeLowerTime" + dr["SystemName"].ToString();
                meeLowerTime.TargetControlID = txtLowerTime.ClientID; //"ctl00_HomeContentPlaceHolder_txtTime";
                meeLowerTime.AutoCompleteValue = "00:00"; //"00:00:00"
                meeLowerTime.Mask = "99:99"; //99:99:99
                meeLowerTime.MaskType = MaskedEditType.Time;

                MaskedEditExtender meeUpperTime = new MaskedEditExtender();
                meeUpperTime.ID = "meeUpperTime" + dr["SystemName"].ToString();
                meeUpperTime.TargetControlID = txtUpperTime.ClientID; //"ctl00_HomeContentPlaceHolder_txtTime";
                meeUpperTime.AutoCompleteValue = "00:00"; //"00:00:00"
                meeUpperTime.Mask = "99:99"; //99:99:99
                meeUpperTime.MaskType = MaskedEditType.Time;


                HtmlTable tblDate = new HtmlTable();
                HtmlTableRow rowOne = new HtmlTableRow();
                HtmlTableRow rowTwo = new HtmlTableRow();

                rowOne.ID = "rowOne_" + dr["SystemName"].ToString();
                rowTwo.ID = "rowTwo_" + dr["SystemName"].ToString();

                tblDate.CellPadding = 0;
                tblDate.CellSpacing = 0;

                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));

                HtmlTableCell cellBox1 = new HtmlTableCell();
                HtmlTableCell cellBox12 = new HtmlTableCell();
                HtmlTableCell cellBox13 = new HtmlTableCell();
                HtmlTableCell cellBox2 = new HtmlTableCell();
                HtmlTableCell cellBox22 = new HtmlTableCell();
                HtmlTableCell cellBox23 = new HtmlTableCell();

                txtLowerTime.Style.Add("margin-left", "5px");

                cellBox1.Controls.Add(txtLowerDate);
                cellBox12.Controls.Add(imgLowerDate);
                cellBox13.Controls.Add(txtLowerTime);
                cellBox12.Controls.Add(ceLowerDate);
                cellBox12.Controls.Add(twmLowerDate);
                //cellBox12.Controls.Add(rvLowerDate);
                cellBox13.Controls.Add(meeLowerTime);

                rowOne.Cells.Add(cellBox1);
                rowOne.Cells.Add(cellBox12);
                rowOne.Cells.Add(cellBox13);
                //cellBox.Controls.Add(new LiteralControl("</br>"));
                txtUpperDate.Style.Add("margin-top", "5px");
                imgUpperDate.Style.Add("margin-top", "5px");
                txtUpperTime.Style.Add("margin-top", "5px");
                txtUpperTime.Style.Add("margin-left", "5px");

                cellBox2.Controls.Add(txtUpperDate);
                cellBox22.Controls.Add(imgUpperDate);
                cellBox23.Controls.Add(txtUpperTime);
                cellBox22.Controls.Add(ceUpperDate);
                //cellBox22.Controls.Add(twmUpperDate);
                //cellBox22.Controls.Add(rvUpperDate);
                cellBox23.Controls.Add(meeUpperTime);

                rowTwo.Cells.Add(cellBox2);
                rowTwo.Cells.Add(cellBox22);
                rowTwo.Cells.Add(cellBox23);



                tblDate.Rows.Add(rowOne);
                tblDate.Rows.Add(rowTwo);
                cellBox.Controls.Add(tblDate);
                theRow.Cells.Add(cellBox);



                //                strJSDynamicShowHide = strJSDynamicShowHide + @"
                //                        $('#" + _strDynamictabPart + txtLowerDate.ID + @"').on('keyup',function () {
                //                                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerDate.ID + @"').val();
                //                                                                 if (strLowerValue.trim() != '') {
                //                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeIn();
                //                                                                }
                //                                                                else {
                //                                                                     $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val('');
                //                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeOut(); 
                //                                                                }
                //                                                            });
                //                         $('#" + _strDynamictabPart + txtLowerDate.ID + @"').trigger('keyup');
                //
                //                    ";

                //                strJSDynamicShowHide = strJSDynamicShowHide + @"
                //                        $('#" + _strDynamictabPart + txtLowerDate.ID + @"').change(function () {
                //                                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerDate.ID + @"').val();
                //                                                                 if (strLowerValue.trim() != '') {
                //                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeIn();
                //                                                                }
                //                                                                else {
                //                                                                     $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val('');
                //                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeOut(); 
                //                                                                }
                //                                                            });
                //                         $('#" + _strDynamictabPart + txtLowerDate.ID + @"').trigger('change');
                //
                //                    ";

                strJSDynamicShowHide = strJSDynamicShowHide + @"


                                function ShowHideUP_" + _strDynamictabPart + txtUpperDate.ID + @"(keepvalue) {                                                               
                                        try
                                        {
                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerDate.ID + @"').val();
                                                var strLowerValueT = $('#" + _strDynamictabPart + txtLowerTime.ID + @"').val();   
                                                var strUpperValue = $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val();               
                                                if (strLowerValue.trim() != '') {                        
                                                         $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeIn();
                                                       
                                                         if (strUpperValue.trim() == '' && keepvalue=='0') {
                                                                $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val(strLowerValue);
                                                                var strUpperValueT = $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val();
                                                                if (strUpperValueT.trim() == ''){
                                                                                 $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val(strLowerValueT);
                                                                            }
                                                      
                                                    }               
                                                }
                                                else {
                                                        
                                                                    $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val('');
                                                                    $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val('');  
                                                                    $('#" + _strDynamictabPart + txtLowerTime.ID + @"').val('');  
                                                                    $('#" + _strDynamictabPart + rowTwo.ID + @"').fadeOut();
                                                            
                                                     
                                                }
                                        }
                                        catch(err)
                                        {
                                             //
                                        }
                                     };

                                        $('#" + _strDynamictabPart + txtLowerDate.ID + @"').change(function () {
                                                ShowHideUP_" + _strDynamictabPart + txtUpperDate.ID + @"('0');                                                          
                                        });
    
                                        $('#" + _strDynamictabPart + txtLowerDate.ID + @"').on('keyup',function () {
                                                ShowHideUP_" + _strDynamictabPart + txtUpperDate.ID + @"('1');                                                          
                                        });

                                    ShowHideUP_" + _strDynamictabPart + txtUpperDate.ID + @"('1');  

                                            ";


                strJSDynamicShowHide = strJSDynamicShowHide + @"


                                function ShowHideUP_" + _strDynamictabPart + txtUpperTime.ID + @"(keepvalue) {                                                               
                                        try
                                        {
                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerTime.ID + @"').val();
                                                  var strUpperValueD = $('#" + _strDynamictabPart + txtUpperDate.ID + @"').val();  
                                                   var strUpperValue = $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val();               
                                                if (strLowerValue.trim() != '') {                     
                                                                                                     
                                                     if (strUpperValue.trim() == '' && keepvalue=='0') {
                                                      $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val(strLowerValue);
                                                    }               
                                                }
                                                else {
                                                          
                                                         
                                                           if (strLowerValue.trim() == '' && strUpperValueD.trim() == '') {
                                                                  $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val('');                                                                 
                                                            }                                                         
                                                    
                                                }
                                        }
                                        catch(err)
                                        {
                                             //
                                        }
                                     };

                                        $('#" + _strDynamictabPart + txtLowerTime.ID + @"').change(function () {
                                                ShowHideUP_" + _strDynamictabPart + txtUpperTime.ID + @"('0');                                                          
                                        });
    
                                    ShowHideUP_" + _strDynamictabPart + txtUpperTime.ID + @"('1');  

                                            ";


            }
            else if (dr["ColumnType"].ToString() == "time")
            {

                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }

                s = s + 1;
                TextBox txtLowerTime = new TextBox();
                txtLowerTime.ID = "txtLowerTime_" + dr["SystemName"].ToString();
                txtLowerTime.CssClass = "NormalTextBox";
                txtLowerTime.Width = 70;
                txtLowerTime.AutoCompleteType = AutoCompleteType.Disabled;



                TextBox txtUpperTime = new TextBox();
                txtUpperTime.ID = "txtUpperTime_" + dr["SystemName"].ToString();
                txtUpperTime.CssClass = "NormalTextBox";
                txtUpperTime.Width = 70;
                txtUpperTime.AutoCompleteType = AutoCompleteType.Disabled;
                txtUpperTime.Attributes.Add("placeholder", "To hh:mm");

                MaskedEditExtender meeLowerTime = new MaskedEditExtender();
                meeLowerTime.ID = "meeLowerTime" + dr["SystemName"].ToString();
                meeLowerTime.TargetControlID = txtLowerTime.ClientID; //"ctl00_HomeContentPlaceHolder_txtTime";
                meeLowerTime.AutoCompleteValue = "00:00"; //"00:00:00"
                meeLowerTime.Mask = "99:99"; //99:99:99
                meeLowerTime.MaskType = MaskedEditType.Time;

                MaskedEditExtender meeUpperTime = new MaskedEditExtender();
                meeUpperTime.ID = "meeUpperTime" + dr["SystemName"].ToString();
                meeUpperTime.TargetControlID = txtUpperTime.ClientID; //"ctl00_HomeContentPlaceHolder_txtTime";
                meeUpperTime.AutoCompleteValue = "00:00"; //"00:00:00"
                meeUpperTime.Mask = "99:99"; //99:99:99
                meeUpperTime.MaskType = MaskedEditType.Time;



                TextBoxWatermarkExtender twmLowerTime = new TextBoxWatermarkExtender();
                twmLowerTime.ID = "twmTime_" + dr["SystemName"].ToString();
                twmLowerTime.TargetControlID = txtLowerTime.ID;
                twmLowerTime.WatermarkText = "hh:mm";
                twmLowerTime.WatermarkCssClass = "MaskText";

                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));
                cellBox.Controls.Add(txtLowerTime);
                cellBox.Controls.Add(new LiteralControl("</br>"));
                txtUpperTime.Style.Add("margin-top", "5px");
                cellBox.Controls.Add(txtUpperTime);
                cellBox.Controls.Add(meeLowerTime);
                cellBox.Controls.Add(meeUpperTime);
                cellBox.Controls.Add(twmLowerTime);


                theRow.Cells.Add(cellBox);

                //                strJSDynamicShowHide = strJSDynamicShowHide + @"
                //                        $('#" + _strDynamictabPart + txtLowerTime.ID + @"').on('keyup',function () {
                //                                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerTime.ID + @"').val();
                //                                                                 if (strLowerValue.trim() != '') {
                //                                                                    $('#" + _strDynamictabPart + txtUpperTime.ID + @"').fadeIn();
                //                                                                }
                //                                                                else {
                //                                                                     $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val('');
                //                                                                    $('#" + _strDynamictabPart + txtUpperTime.ID + @"').fadeOut(); 
                //                                                                }
                //                                                            });
                //                         $('#" + _strDynamictabPart + txtLowerTime.ID + @"').trigger('keyup');
                //
                //                    ";

                //                strJSDynamicShowHide = strJSDynamicShowHide + @"
                //                        $('#" + _strDynamictabPart + txtLowerTime.ID + @"').change(function () {
                //                                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerTime.ID + @"').val();
                //                                                                 if (strLowerValue.trim() != '') {
                //                                                                    $('#" + _strDynamictabPart + txtUpperTime.ID + @"').fadeIn();
                //                                                                }
                //                                                                else {
                //                                                                     $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val('');
                //                                                                    $('#" + _strDynamictabPart + txtUpperTime.ID + @"').fadeOut(); 
                //                                                                }
                //                                                            });
                //                         $('#" + _strDynamictabPart + txtLowerTime.ID + @"').trigger('change');
                //
                //                    ";


                strJSDynamicShowHide = strJSDynamicShowHide + @"


                                function ShowHideUP_" + _strDynamictabPart + txtUpperTime.ID + @"(keepvalue) {                                                               
                                        try
                                        {
                                                var strLowerValue = $('#" + _strDynamictabPart + txtLowerTime.ID + @"').val();
                                                                  
                                                if (strLowerValue.trim() != '') {                       
                                                      $('#" + _strDynamictabPart + txtUpperTime.ID + @"').fadeIn();
                                                     var strUpperValue = $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val();
                                                     if (strUpperValue.trim() == '' && keepvalue=='0') {
                                                      $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val(strLowerValue);
                                                    }               
                                                }
                                                else {
                                                        $('#" + _strDynamictabPart + txtUpperTime.ID + @"').val('');  
                                                         $('#" + _strDynamictabPart + txtUpperTime.ID + @"').fadeOut();                                                    
                                                }
                                        }
                                        catch(err)
                                        {
                                             //
                                        }
                                     };

                                        $('#" + _strDynamictabPart + txtLowerTime.ID + @"').change(function () {
                                                ShowHideUP_" + _strDynamictabPart + txtUpperTime.ID + @"('0');                                                          
                                        });
                                         $('#" + _strDynamictabPart + txtLowerTime.ID + @"').on('keyup',function () {
                                                ShowHideUP_" + _strDynamictabPart + txtUpperTime.ID + @"('1');                                                          
                                        });
    
                                    ShowHideUP_" + _strDynamictabPart + txtUpperTime.ID + @"('1');  

                                            ";

            }
            else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "values"
                || dr["DropDownType"].ToString() == "value_text"))
            {
                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }
                s = s + 1;

                DropDownList ddlSearch = new DropDownList();
                ddlSearch.ID = "ddlSearch_" + dr["SystemName"].ToString();
                ddlSearch.CssClass = "NormalTextBox";
                ddlSearch.Width = 115;

                ddlSearch.AutoPostBack = true;
                ddlSearch.SelectedIndexChanged += new EventHandler(ddl_search);
                               
                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));
                cellBox.Controls.Add(ddlSearch);

                theRow.Cells.Add(cellBox);
            }
            else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "table" || dr["DropDownType"].ToString() == "tabledd") &&
            dr["TableTableID"] != DBNull.Value && dr["DisplayColumn"].ToString() != "")
            {
                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }

                s = s + 1;
             
                DropDownList ddlParentSearch = new DropDownList();
                ddlParentSearch.ID = "ddlParentSearch_" + dr["SystemName"].ToString();
                ddlParentSearch.CssClass = "NormalTextBox";
                ddlParentSearch.Width = 115;

                ddlParentSearch.AutoPostBack = true;
                ddlParentSearch.SelectedIndexChanged += new EventHandler(ddl_search);
                
                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));
                cellBox.Controls.Add(ddlParentSearch);
                theRow.Cells.Add(cellBox);

            }
            else if (dr["ColumnType"].ToString() == "radiobutton" || dr["ColumnType"].ToString() == "checkbox" ||
                dr["ColumnType"].ToString() == "listbox")
            {
                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }
                s = s + 1;

                DropDownList ddlSearch = new DropDownList();
                ddlSearch.ID = "ddlSearch_" + dr["SystemName"].ToString();
                ddlSearch.CssClass = "NormalTextBox";
                ddlSearch.Width = 115;

                ddlSearch.AutoPostBack = true;
                ddlSearch.SelectedIndexChanged += new EventHandler(ddl_search);
              
                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));
                cellBox.Controls.Add(ddlSearch);

                theRow.Cells.Add(cellBox);
            }
            else
            {
                if (s == 0)
                {
                    HtmlTableCell cellBoxS = new HtmlTableCell();
                    cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                    theRow.Cells.Add(cellBoxS);
                }

                s = s + 1;
                TextBox txtSearch = new TextBox();
                txtSearch.ID = "txtSearch_" + dr["SystemName"].ToString();
                txtSearch.CssClass = "NormalTextBox";
                txtSearch.Width = 105;
                txtSearch.Attributes.Add("onblur", "this.value=this.value.trim()");

                TextBoxWatermarkExtender twmTextSearch = new TextBoxWatermarkExtender();
                twmTextSearch.ID = "twmTextSearch_" + dr["SystemName"].ToString();
                twmTextSearch.TargetControlID = txtSearch.ID;
                twmTextSearch.WatermarkText = " ";
                twmTextSearch.WatermarkCssClass = "MaskText";                              

                HtmlTableCell cellBox = new HtmlTableCell();
                cellBox.Controls.Add(new LiteralControl("" + dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() + "<br/>" : dr["Heading"].ToString() + "<br/>"));
                cellBox.Controls.Add(txtSearch);
                cellBox.Controls.Add(twmTextSearch);

                theRow.Cells.Add(cellBox);

            }


        }



        foreach (DataRow dr in _dtSearchGroup.Rows)
        {
            if (s == 0)
            {
                HtmlTableCell cellBoxS = new HtmlTableCell();
                cellBoxS.Controls.Add(new LiteralControl("<br/><strong>Search:</strong>"));
                theRow.Cells.Add(cellBoxS);
            }

            s = s + 1;
            TextBox txtSearch = new TextBox();
            txtSearch.ID = "txtSearch_" + dr["SearchGroupID"].ToString();
            txtSearch.CssClass = "NormalTextBox";
            txtSearch.Width = 105;
            HtmlTableCell cellBox = new HtmlTableCell();
            cellBox.Controls.Add(new LiteralControl("" + dr["GroupName"].ToString() + "<br/>"));
            cellBox.Controls.Add(txtSearch);

            theRow.Cells.Add(cellBox);

        }

        int v = 0;
        foreach (HtmlTableCell eachCell in theRow.Cells)
        {
            if (v > 0)
                eachCell.Style.Add("vertical-align", "bottom");

            v = v + 1;
        }

        tblSearchControls.Rows.Add(theRow);

        if (strJSDynamicShowHide != "")
        {
            strJSDynamicShowHide = @"$(document).ready(function () { 
                        try {  
                                " + strJSDynamicShowHide + @" 
                            }
                        catch(err) {
                                //do ntohing
                                }
                            });";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strJSDynamicShowHide" + _strDynamictabPart, strJSDynamicShowHide, true);
        }

    }

    protected void SetCosmetic()
    {
        //need to use class instead of fixed color
        string strAlterBC2 = "DCF2F0";

        if (this.Page.MasterPageFile != null && this.Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        {
          
            strAlterBC2 = "ECECED";

        }
        if (_theTable.FilterTopColour != "")
        {
            trFiletrTop.Style.Add("background-color", "#" + _theTable.FilterTopColour);
        }
        if (_theTable.FilterBottomColour != "")
        {
            trFilterBottom.Style.Add("background-color", "#" + _theTable.FilterBottomColour);
        }

        if (this.Page.MasterPageFile != null && this.Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        {
            _bCustomDDL = true;
            if (PageType == "p")
            {
                if (_theTable.FilterTopColour == "")
                {
                    trFiletrTop.Style.Add("background-color", "#238DA3");
                }
                if (_theTable.FilterBottomColour == "")
                {
                    trFilterBottom.Style.Add("background-color", "#40A5B7");
                }

                ddlEnteredBy.Width = 220;
                divEnteredBy.Width = 200;

                ddlEnteredBy.CssClass = "ddlrrp";
                divEnteredBy.CssClass = "ddlDIV";
                stgFilter.Style.Add("color", "#ffffff");
                tblAdvancedOptionChk.Style.Add("color", "#ffffff");
                imgShowGraph.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Pager/Images/rrp/Graph.png";
                imgUpload.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Pager/Images/rrp/upload.png";
                ibEmail.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Pager/Images/rrp/email.png";
                imgConfig.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Pager/Images/rrp/config.png";

                if (_theTable.HeaderColor != "")
                {
                    ltTextStyles.Text = "<style>.pagerstyle{ background: #" + _theTable.HeaderColor + ";}.pagergradient{ background: #" + _theTable.HeaderColor + ";}.TopTitle{color:#FFFFFF;}</style>";
                }
                else
                {
                    ltTextStyles.Text = "<style>.pagerstyle{ background: #0089a5;}.pagergradient{ background: #0089a5;}.TopTitle{color:#FFFFFF;}</style>";
                }


                divSearch.Attributes["class"] = "searchcornerRRP";
                divRecordListTop.Style.Add("padding-left", "170px");
                if (_theTable.HeaderColor != "")
                {
                    divRecordListTop.Style.Add("background-color", "#" + _theTable.HeaderColor);

                }
                else
                {
                    divRecordListTop.Style.Add("background-color", "#0089a5");
                }
            }
            gvTheGrid.AlternatingRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#" + strAlterBC2);
        }
        else
        {
            gvTheGrid.HeaderStyle.ForeColor = System.Drawing.Color.Black;
        }
    }

    protected void DoOtherInit()
    {

    }

    // get last minute controls
    //protected override void OnPreRender(EventArgs e)
    //{
    //    base.OnPreRender(e);

    //    // start scanning from page subcontrols
    //    ControlCollection _collection = this.Controls;
    //    lblMsg.Visible = true;
    //    lblMsg.Text =TheDatabase.GetCode(_collection,_strDynamictabPart).Replace("\r\n", "<br/>");
    //}
    protected void Page_Init(object sender, EventArgs e)
    {
        
        if (Session["User"] == null || Session["FilesLocation"] == null)
        {
            try
            {
                Response.Redirect("~/Login.aspx", true);
            }
           catch
            {
               //
            }            
        }


        _strDynamictabPart = lnkSearch.ClientID.Substring(0, lnkSearch.ClientID.Length - 9);    

        _strFilesLocation = Session["FilesLocation"].ToString();

        FindTheTable();

        if (_theTable == null)
            return;

        GetRoleRight();

        _bEqualOrGreaterOperator = Common.SO_SearchAllifToIsNull(_theTable.AccountID, _theTable.TableID);

        if (_bEqualOrGreaterOperator == true)// && strLowerDate.Trim() != "" && strUpperDate.Trim() == "")
        {
            _strEqualOrGreaterOperator = " >= ";
        }

        FindTheView();
        if (_theView == null)
            return;
       
        if (_theTable == null)
        {
            goto EndSub;//why?
        }

        SetCosmetic();

        JSCode();

        CreateDynamicControls();


    EndSub:
        int Fxx;
        //
    }

    public string[] GetDataKeyNames()
    {
        string[] strRecordID = new string[1];
        strRecordID[0] = "DBGSystemRecordID";
        return strRecordID;
    }

    protected string GetTextFromValueForDD(string strDropdownValues, string strMainValue)
    {
        if (strMainValue == "")
            return "";

        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        strMainValue = Server.HtmlDecode(strMainValue);
        foreach (string s in result)
        {
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue == strMainValue)
                {
                    return strText;
                }
            }
        }
        return strText;
    }


    //protected string GetImageFromValueForDD(string strDropdownValues, string strMainValue)
    //{
    //    OptionImageList theOptionImageList = JSONField.GetTypedObject<OptionImageList>(strDropdownValues);
    //    foreach (OptionImage aOptionImage in theOptionImageList.ImageList)
    //    {
    //        if(aOptionImage.Value==strMainValue)
    //        {
    //            return "<img src='" + _strFilesLocation + "/UserFiles/AppFiles/" + aOptionImage.UniqueFileName + "' title='" + aOptionImage .Value+ "' />";
    //        }

    //    }
    //    return strMainValue;
    //}
    //protected string GetTextFromValueForList(string strDropdownValues, string strMainValue)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    string[] values = strMainValue.Split(new string[] { ","}, StringSplitOptions.RemoveEmptyEntries);

    //    string strValue = "";
    //    string strText = "";
    //    string strTotalText = "";
    //    foreach (string s in result)
    //    {
    //        strValue = "";
    //        strText = "";
    //        if (s.IndexOf(",") > -1)
    //        {
    //            strValue = s.Substring(0, s.IndexOf(","));
    //            strText = s.Substring(strValue.Length + 1);

    //            foreach (string v in values)
    //            {
    //                if (strValue == v)
    //                {
    //                    strTotalText = strTotalText + strText + ",";
    //                }
    //            }


    //        }
    //    }
    //    if(strTotalText.Length>0)
    //        strTotalText=strTotalText.Substring(0,strTotalText.Length-1);

    //    return strTotalText;
    //}

    //protected string GetTextFromTableForList(int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn, string strMainValue)
    //{

    //    //it's a new dev so iLinkedParentColumnID must be RecordID

    //    DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable, null, null);

    //    string[] values = strMainValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


    //    string strTotalText = "";
    //    foreach (DataRow dr in dtParents.Rows)
    //    {            
    //            foreach (string v in values)
    //            {
    //                if (dr[0].ToString() == v)
    //                {
    //                    strTotalText = strTotalText + dr[1].ToString() + ",";
    //                }
    //            }            
    //    }
    //    if (strTotalText.Length > 0)
    //        strTotalText = strTotalText.Substring(0, strTotalText.Length - 1);

    //    return strTotalText;
    //}


    protected string GetValueFromTextForList(string strDropdownValues, string strMainText)
    {
        if (strMainText == "")
            return "";

        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);


        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);

                if (strText == strMainText || strText.IndexOf(strMainText) > -1)
                {
                    return strValue;
                }

            }
        }


        return "";
    }

    protected void PopulateUser()
    {
        //int iTN = 0;


        //ddlEnteredBy.DataSource = Common.DataTableFromText("SELECT (FirstName + ' ' + LastName) as FullName, UserID FROM [User]" +
        //   " WHERE AccountID=" + Session["AccountID"].ToString() + " OR UserID=" + SystemData.SystemOption_ValueByKey("AutoUploadUserID") +
        //    " ORDER BY FirstName", null, null);
        ddlEnteredBy.Items.Clear();
        ddlEnteredBy.DataSource = Common.DataTableFromText("SELECT (FirstName + ' ' + LastName) as FullName, [User].UserID FROM [User] INNER JOIN UserRole ON [User].UserID=UserRole.UserID " +
           " WHERE UserRole.AccountID=" + Session["AccountID"].ToString() + " OR UserRole.UserID=" + SystemData.SystemOption_ValueByKey_Account("AutoUploadUserID", null, TableID) +
            " ORDER BY FirstName");

        ddlEnteredBy.DataBind();
        ListItem liAll = new ListItem("All", "-1");
        ddlEnteredBy.Items.Insert(0, liAll);

    }

    //public string fnReplaceDisplayColumns(string sDisplay, int nTableID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("dbo.fnReplaceDisplayColumns", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            SqlParameter pRV = new SqlParameter("@Result", SqlDbType.VarChar);
    //            pRV.Direction = ParameterDirection.ReturnValue;

    //            command.Parameters.Add(pRV);
    //            command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

    //            command.Parameters.Add(new SqlParameter("@sDisplay",sDisplay));


    //            connection.Open();
    //            command.ExecuteNonQuery();
    //            connection.Close();
    //            connection.Dispose();

    //            return pRV.Value.ToString();
    //        }
    //    }
    //}





    protected void PopulateBatch()
    {

        ddlUploadedBatch.Items.Clear();
        ddlUploadedBatch.DataSource = Common.DataTableFromText("SELECT BatchID,BatchDescription FROM Batch WHERE  IsImported=1 and  TableID=" + _qsTableID);
        ddlUploadedBatch.DataBind();
        ListItem liAll = new ListItem("All", "");
        ddlUploadedBatch.Items.Insert(0, liAll);

    }


    //protected void PopulateRecordGroupFilter()
    //{
    //    int iTN = 0;
    //    ddlRecordGroupFilter.DataSource = RecordManager.ets_Menu_Select(null, "", null,
    //    int.Parse(Session["AccountID"].ToString()), true,
    //    "Menu", "ASC", null, null, ref iTN);
    //    ddlRecordGroupFilter.DataBind();
    //    ListItem liAll = new ListItem("All", "-1");
    //    ddlRecordGroupFilter.Items.Insert(0, liAll);

    //    PopulateTableDDL();

    //}


    //protected string GetDDLValueFromText(string strDropdownValues, string strSearchText)
    //{

    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    string strValue = "";
    //    string strText = "";

    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        strValue = "";
    //        strText = "";
    //        if (s.IndexOf(",") > -1)
    //        {
    //            strValue = s.Substring(0, s.IndexOf(","));
    //            strText = s.Substring(strValue.Length + 1);
    //            if (strValue != "" && strText != "")
    //            {
    //                if (strText.ToLower() == strSearchText.ToLower())
    //                {
    //                    return strValue;
    //                }
    //            }
    //        }
    //    }

    //    return "";

    //}


    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null, null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, null,
                "st.TableName", "ASC",
                null, null, ref iTN, Session["STs"].ToString());

        ddlTable.DataBind();
        if (iTN == 0)
        {
            ListItem liAll = new ListItem("None", "-1");
            ddlTable.Items.Insert(0, liAll);
        }

    }



    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    return;
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            PopulateDynamicControls();
        }

        if (_theTable == null)
            return;




        if (Request.RawUrl.IndexOf("EachRecordTable.aspx") > -1)
        {
            _strNoAjaxView = "noajax=yes&";
        }
        else
        {
            _strNoAjaxView = "";
        }

        try
        {
            
            string strMaxCharactersInCell = SystemData.SystemOption_ValueByKey_Account("MaxCharactersInCell", _theTable.AccountID, _theTable.TableID);
            if (strMaxCharactersInCell != "")
                _iMaxCharactersInCell = int.Parse(strMaxCharactersInCell);

            if (!IsPostBack)
            {
                string HidePagerGoButton = SystemData.SystemOption_ValueByKey_Account("Hide Pager Go Button", _theTable.AccountID, _theTable.TableID);
                if (HidePagerGoButton != "")
                {
                    if (HidePagerGoButton.ToLower() == "yes")
                    {
                        hfHidePagerGoButton.Value = "yes";
                    }
                }

            }

        }
        catch
        {
            //
        }
        string strDeleteReasonRequired = SystemData.SystemOption_ValueByKey_Account("DeleteReasonRequired",
           int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), int.Parse(TableID.ToString()));

        if (strDeleteReasonRequired != "")
        {
            if (strDeleteReasonRequired.ToLower() == "yes")
            {
                _bDeleteReason = true;
            }
        }


        //string strGraphIcon = SystemData.SystemOption_ValueByKey_Account("Graph Icon",
        //  int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), int.Parse(TableID.ToString()));

        string strGraphIcon = SystemData.SystemOption_ValueByKey_Account("Graph Icon", int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), int.Parse(TableID.ToString()));

        if (strGraphIcon != "")
        {
            if (strGraphIcon.ToLower() == "no")
            {
                _bShowGraphIcon = false;
            }
        }

        if (_bShowGraphIcon == false)
        {
            divShowGraph.Visible = false;
        }

      
        //Title = "Records";
        if (Request.RawUrl.IndexOf("Default.aspx") > -1)
        {
            _bOpenInParent = true;

        }
        //gvTheGrid.Style.Add("width", "100%");
        if (Request.QueryString["Dashboard"] != null)
        {
            _bOpenInParent = true;
            //pnlSearch.Visible = false;
            //chkShowAdvancedOptions.Checked = false;
            hplNewData.Target = "_parent";
            hplNewDataFilter.Target = "_parent";
            hplNewDataFilter2.Target = "_parent";

            string strW = "1";
            if (Request.QueryString["width"] != null)
            {
                //gvTheGrid.Width = 450;
                //lblTitle.Width = 450;
                gvTheGrid.Style.Add("min-width", "450px");

                //tblTopCaption.Width = "100%";
                strW = "2";
            }
            else
            {

                gvTheGrid.Style.Add("min-width", "1000px");
                //tblTopCaption.Width = "100%";
            }

            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "autoResizeMe", "<script>autoResizeMe('" + UpdatePanel1.ClientID + "'," + strW + " );autoResizeMe('" + tblTopCaption.ClientID + "'," + strW + " ); </script>", false);
        }
        else
        {

            gvTheGrid.Style.Add("min-width", "1000px");
            //tblTopCaption.Width = "1000px";
            //tblTopCaption.Width = "100%";
        }




        try
        {
           
            _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

            //this.Page.Master.
            //if (Request.QueryString["viewname"] != null)
            //{
            //    _strViewName = Request.QueryString["viewname"].ToString().Trim();
            //}

            if (PageType == "p")
            {
                if (_strViewPageType == "dash")
                {
                    tdTopButtons.Visible = false;
                    tdTopTitile.Style.Add("width", "100%");
                }
                else
                {
                    tblTopCaption.Style.Add("width", "1000px;");
                }

                //TableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
                if (!IsPostBack)
                {

                    //Standardised_Field_Table
                    if ((int)_theTable.AccountID != (int)_theUserRole.AccountID && !Common.HaveAccess(_strRecordRightID, "1"))
                    {
                        Response.Redirect("~/Empty.aspx", true);
                        return;
                    }

                    if (!Common.HaveAccess(_strRecordRightID, "1"))
                    {
                        string strSFTID = SystemData.SystemOption_ValueByKey_Account("Standardised_Field_Table", _theTable.AccountID, _theTable.TableID);
                        if (strSFTID != "")
                        {
                            if (strSFTID == _theTable.TableID.ToString())
                            {
                                Response.Redirect("~/Empty.aspx", true);
                                return;
                            }
                        }
                    }

                    mpeDeleteAll.BehaviorID = "DA_BehaviorID_" + DetailTabIndex.ToString();

                    if (_theTable.AddOpensForm != null && (bool)_theTable.AddOpensForm && _theTable.AddRecordSP != "")
                    {
                        lblAddRecordTitle.Text = "Add " + _theTable.TableName;

                        ddlFormSet.DataSource = Common.DataTableFromText(@"SELECT FormSetID, FormSetName FROM FormSet FS
                            INNER JOIN FormSetGroup FSG ON FS.FormSetGroupID=FSG.FormSetGroupID
                            WHERE ParentTableID=" + _theTable.TableID.ToString() + @" AND ShowOnAdd=1 
                            ORDER BY FormSetName");

                        ddlFormSet.DataBind();
                    }

                }
            }
            else
            {
                trRecordListTitle.Visible = false;

                if (!IsPostBack)
                {
                    mpeDeleteAll.BehaviorID = "DA_BehaviorID_" + TableID.ToString();
                }
            }


            hlBatches.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Batches.aspx?TableID=" + Cryptography.Encrypt(TableID.ToString());
            _qsTableID = TableID.ToString();

            cbcSearch1.TableID = TableID;
            cbcSearch2.TableID = TableID;
            cbcSearch3.TableID = TableID;
            cbcSearchMain.TableID = TableID;

            if (_theView != null)
            {
                cbcSearch1.ViewID = _theView.ViewID;
                cbcSearch2.ViewID = _theView.ViewID;
                cbcSearch3.ViewID = _theView.ViewID;
                cbcSearchMain.ViewID = _theView.ViewID;
                //cbcvSumFilter.ViewID = _theView.ViewID;
            }

           

            if (_strRecordRightID == Common.UserRoleType.None) //none role
            {
                Response.Redirect(Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Empty.aspx", false);
                return;
            }

           

            //Ticket 846: Removing dropdown when in mobile
            //modified by: Ismael
            ddlTableMenu.Visible = false;
            if (ShowTitle)
            {
                lblTitle.Visible = true;
            }
            //End Ticket 846


            if (!IsPostBack)
            {

                if (_theTable != null)
                {
                    PopulateExportTemplate((int)_theTable.TableID);
                    ddlTemplate_SelectedIndexChanged(null, null);

                    hlExportTemplateNew.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?mode=" + Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&SearchCriteriaET=" + Cryptography.Encrypt("-1") + "&fixedbackurl=" + Cryptography.Encrypt(Request.RawUrl);
                    //hlExportTemplate.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) +"&SearchCriteriaET=" + Cryptography.Encrypt("-1");
                }


                if (Request.QueryString["TextSearch"] != null)
                {
                    hfTextSearch.Value = Cryptography.Decrypt(Request.QueryString["TextSearch"].ToString());
                }


                lnkAndOr1.Attributes.Add("onclick", "toggleAndOr(this,'" + hfAndOr1.ClientID + "');return false;");
                lnkAndOr2.Attributes.Add("onclick", "toggleAndOr(this,'" + hfAndOr2.ClientID + "');return false;");
                lnkAndOr3.Attributes.Add("onclick", "toggleAndOr(this,'" + hfAndOr3.ClientID + "');return false;");

                //lnkAndOr1.Attributes.Add("text", "and");

                lnkAddSearch1.Attributes.Add("onclick", "$('#" + trSearch1.ClientID + "').fadeIn();$('#" + lnkAddSearch1.ClientID + "').fadeOut();if ($('#" + hfAndOr1.ClientID + "').val()==''){ $('#" + hfAndOr1.ClientID + "').val(document.getElementById('" + lnkAndOr1.ClientID + "').text)};");//return false;
                lnkAddSearch2.Attributes.Add("onclick", "$('#" + trSearch2.ClientID + "').fadeIn();$('#" + lnkAddSearch2.ClientID + "').fadeOut();if ($('#" + hfAndOr2.ClientID + "').val()==''){$('#" + hfAndOr2.ClientID + "').val(document.getElementById('" + lnkAndOr2.ClientID + "').text)};");//return false;
                lnkAddSearch3.Attributes.Add("onclick", "$('#" + trSearch3.ClientID + "').fadeIn();$('#" + lnkAddSearch3.ClientID + "').fadeOut();if ($('#" + hfAndOr3.ClientID + "').val()==''){$('#" + hfAndOr3.ClientID + "').val(document.getElementById('" + lnkAndOr3.ClientID + "').text)};");//return false;

                lnkMinusSearch1.Attributes.Add("onclick", "$('#" + trSearch1.ClientID + "').fadeOut();$('#" + lnkAddSearch1.ClientID + "').fadeIn();$('#" + hfAndOr1.ClientID + "').val('');return false;");
                lnkMinusSearch2.Attributes.Add("onclick", "$('#" + trSearch2.ClientID + "').fadeOut();$('#" + lnkAddSearch2.ClientID + "').fadeIn();$('#" + hfAndOr2.ClientID + "').val('');return false;");
                lnkMinusSearch3.Attributes.Add("onclick", "$('#" + trSearch3.ClientID + "').fadeOut();$('#" + lnkAddSearch3.ClientID + "').fadeIn();$('#" + hfAndOr3.ClientID + "').val('');return false;");


                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PutDefaultSearcUI", "$('#" + trSearch1.ClientID + "').fadeOut();$('#" + trSearch2.ClientID + "').fadeOut();$('#" + trSearch3.ClientID + "').fadeOut();", true);


                PopulateTerminology();
                //hfFileName.Value = Guid.NewGuid().ToString() + ".csv";
                //PopulateYAxis();
                PopulateYAxisBulk();


                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = int.Parse(Session["GridPageSize"].ToString()); }

                if (_theView != null)
                {
                    if (_theView.RowsPerPage != null)
                    {
                        gvTheGrid.PageSize = (int)_theView.RowsPerPage;
                        _iMaxRows = gvTheGrid.PageSize;
                    }
                }

                if (TableID != null)
                {
                    if (Request.QueryString["warning"] != null)
                    {
                        chkShowOnlyWarning.Checked = true;
                    }

                    Table qsTable = RecordManager.ets_Table_Details(TableID);                   

                    if (qsTable.HideAdvancedOption != null)
                    {
                        if ((bool)qsTable.HideAdvancedOption)
                        {
                            hfHideAdvancedOption.Value = "yes";
                        }
                    }


                    hlConfig.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + "#topline";

                    if (Common.HaveAccess(_strRecordRightID, "1,2"))
                    {
                        divConfig.Visible = true;
                    }


                    PopulateUser();
                    PopulateBatch();


                    PopulateMenuTableDDL();
                    ddlTableMenu.Text = qsTable.TableID.ToString();

                    PopulateTableDDL();
                    ddlTable.Text = TableID.ToString();

                }
                else
                {

                    PopulateUser();
                    PopulateBatch();
                    PopulateTableDDL();

                }



                if (Request.QueryString["SearchCriteriaID"] != null && Session["SCid" + hfViewID.Value] == null && PageType == "p") //&& PageType == "p"
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteriaID"].ToString())));

                }
                else if (Session["SCid" + hfViewID.Value] != null)
                {
                    PopulateSearchCriteria(int.Parse(Session["SCid" + hfViewID.Value].ToString()));
                }
                else
                {
                    _bBindWithSC = false;

                }

                if (_bBindWithSC)
                {
                    gvTheGrid.PageSize = _iMaxRows;
                    gvTheGrid.GridViewSortColumn = _strGridViewSortColumn;
                    if (_strGridViewSortDirection.ToUpper() == "ASC")
                    {
                        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    }
                    else
                    {
                        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                    }
                    //BindTheGrid(_iStartIndex, _iMaxRows);
                }
                else
                {

                    if (_qsTableID == null || _theView == null)
                        return;

                    //this is 1st screen

                    Table qsTable = RecordManager.ets_Table_Details(TableID);

                    gvTheGrid.GridViewSortColumn = "DBGSystemRecordID";
                    gvTheGrid.GridViewSortDirection = SortDirection.Descending;

                    if (_theView.SortOrder != "")
                    {
                        string strSortColumn = _theView.SortOrder.Substring(0, _theView.SortOrder.LastIndexOf(" ") + 1);
                        string strDirection = _theView.SortOrder.Substring(_theView.SortOrder.LastIndexOf(" ") + 1);

                        gvTheGrid.GridViewSortColumn = strSortColumn.Trim();

                        if (strDirection.Trim().ToLower() == "desc")
                        {
                            gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                        }
                        else
                        {
                            gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                        }

                    }

                    //PopulateSearchParams();
                    //BindTheGrid(0, gvTheGrid.PageSize);

                }
            }
            else
            {
            }


            string strJSSearchShowHide = "";

            if (hfAndOr1.Value != "")
            {
                strJSSearchShowHide = "$('#" + trSearch1.ClientID + "').fadeIn();$('#" + lnkAddSearch1.ClientID + "').fadeOut();";
            }

            if (hfAndOr2.Value != "")
            {
                //strJSSearchShowHide = "$('#" + trSearch1.ClientID + "').fadeIn();$('#" + lnkAddSearch1.ClientID + "').fadeOut();$('#" + trSearch2.ClientID + "').fadeIn();$('#" + lnkAddSearch2.ClientID + "').fadeOut();";
                strJSSearchShowHide = strJSSearchShowHide + "$('#" + trSearch2.ClientID + "').fadeIn();$('#" + lnkAddSearch2.ClientID + "').fadeOut();";
            }

            if (hfAndOr3.Value != "")
            {
                //strJSSearchShowHide = "$('#" + trSearch1.ClientID + "').fadeIn();$('#" + lnkAddSearch1.ClientID + "').fadeOut();$('#" + trSearch2.ClientID + "').fadeIn();$('#" + lnkAddSearch2.ClientID + "').fadeOut();$('#" + trSearch3.ClientID + "').fadeIn();$('#" + lnkAddSearch3.ClientID + "').fadeOut();";
                strJSSearchShowHide = strJSSearchShowHide + "$('#" + trSearch3.ClientID + "').fadeIn();$('#" + lnkAddSearch3.ClientID + "').fadeOut();";
            }


            if (strJSSearchShowHide != "")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PutDefaultSearcUI_PB", strJSSearchShowHide, true);


            if (PageType == "p")
            {
                //Page.Title = _theTable.TableName + " - " + "Records";
                if (_theView != null)
                {
                    Page.Title = _theView.ViewName;
                    lblTitle.Text = _theView.ViewName;
                }
                else
                {
                    Page.Title = _theTable.TableName + " - " + "Records";
                    lblTitle.Text = "Records - " + _theTable.TableName; ;
                }
            }

            //Title = _theTable.TableName;// +" - Records"; 
            //lblTitle.Text = Title;
            if (!IsPostBack)
            {
                BindTheGrid(0, gvTheGrid.PageSize);
            }
            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

                if (PageType == "p")
                {
                    if (_strRecordRightID == Common.UserRoleType.Administrator
                                 || _strRecordRightID == Common.UserRoleType.GOD)
                    {
                        if (chkIsActive.Checked == false)
                            _gvPager.HideEditMany = false;

                        if (_theView != null)
                        {
                            if ((bool)_theView.ShowBulkUpdateIcon == false)
                                _gvPager.HideEditMany = true;
                        }
                    }
                }

            }


            //PopulateSearchParams();

        }
        catch (Exception ex)
        {
            //
        }

        string strXX = @"

                              $(document).ready(function () {
                                  function ShowHide() {

                                    var chk = document.getElementById('ctl00_HomeContentPlaceHolder_rlOne_chkShowAdvancedOptions');
                                    var x = document.getElementById('" + tblAdvancedOption.ClientID + @"');
                                    var trChkOnlyWarning = document.getElementById('" + trChkShowOnlyWarning.ClientID + @"');

                                    if(chk==null)
                                    {
                                        return;
                                    }
                                   
                                    if (chk.checked == false) {
                                            $('#" + tdFilterDynamic.ClientID + @"').fadeIn();
                                            $('#" + tdFilterYAxis.ClientID + @"').fadeOut();
                                            trChkOnlyWarning.style.display = 'none';
                                        }  
                                    if (chk.checked == true) {
                                             $('#" + tdFilterYAxis.ClientID + @"').fadeIn();
                                             $('#" + tdFilterDynamic.ClientID + @"').fadeOut();
                                            trChkOnlyWarning.style.display = 'table-row';
                                        } 

                                     if (chk.checked == false) { x.style.display = 'none'; }
                                    if (chk.checked == true) { x.style.display = 'inline'; }   


                                    var hfHideAdvancedOption = document.getElementById('" + hfHideAdvancedOption.ClientID + @"');
                                    if (hfHideAdvancedOption.value == 'yes') {
                                        $('#" + tblAdvancedOption.ClientID + @"').fadeOut();
                                        $('#" + tblAdvancedOptionChk.ClientID + @"').fadeOut();
                                        $('#" + tblAdvancedOptionChkC.ClientID + @"').fadeOut();
                                        
                                    }   
                                    else
                                    {
                                        
                                    }      
                                }
                                ShowHide();
                                   if (window.addEventListener)
                                    window.addEventListener('load', ShowHide, false);
                                else if (window.attachEvent)
                                    window.attachEvent('onload', ShowHide);
                                else if (document.getElementById)
                                    window.onload = ShowHide;
                         });



                         $(document).ready(function () {


                                    $('#ctl00_HomeContentPlaceHolder_rlOne_chkShowAdvancedOptions').click(function () {
                                    var chk = document.getElementById('ctl00_HomeContentPlaceHolder_rlOne_chkShowAdvancedOptions');

                                        if (chk.checked == true) {
                                            $('#" + tblAdvancedOption.ClientID + @"').fadeIn();
                                            $('#" + tdFilterYAxis.ClientID + @"').fadeIn();
                                            $('#" + tdFilterDynamic.ClientID + @"').fadeOut();
                                        }
                                        else {
                                            $('#" + tblAdvancedOption.ClientID + @"').fadeOut();
                                            $('#" + tdFilterDynamic.ClientID + @"').fadeIn();
                                            $('#" + tdFilterYAxis.ClientID + @"').fadeOut();
                                        }

                                    });

                                 $('#" + chkDeleteParmanent.ClientID + @"').click(function () {
                                        var chk = document.getElementById('" + chkDeleteParmanent.ClientID + @"');
                                        if (chk.checked == true) {
                                            $('#" + trUndo.ClientID + @"').fadeIn();

                                        }
                                        else {
                                            $('#" + trUndo.ClientID + @"').fadeOut();
                                             var chkUndo = document.getElementById('" + chkUndo.ClientID + @"');
                                            chkUndo.checked=false;
                                        }
                                    });
                         if( document.getElementById('" + hfParmanentDelete.ClientID + @"').value=='yes' && document.getElementById('" + chkDelateAllEvery.ClientID + @"')==null)
                                         {
                                            $('#" + trUndo.ClientID + @"').fadeIn();
                                            }
                        $('#" + chkDelateAllEvery.ClientID + @"').click(function () {
                                            if( document.getElementById('" + hfParmanentDelete.ClientID + @"').value=='yes')
                                            {
                                                    var chk = document.getElementById('" + chkDelateAllEvery.ClientID + @"');
                                                    if (chk.checked == true) {
                                                        $('#" + trUndo.ClientID + @"').fadeIn();

                                                    }
                                            }

                                    });

                                });

                ";




        string strCellToolTip = @"var mouseX;
                                var mouseY;
                                $(document).mousemove(function (e) {
                                    try
                                    {
                                        mouseX = e.pageX;
                                        mouseY = e.pageY;
                                    }
                                    catch (err)
                                    {
                                       // alert(err.message)
                                    }
        
                                });
    

                                $(function () {
        
                                    $('.js-tooltip-container').hover(function () {
                                        //$(this).find('.js-tooltip').show();
                                        try {
                                            $(this).find('.js-tooltip').addClass('ajax-tooltip');
                                            $(this).find('.ajax-tooltip').css({ 'top': mouseY, 'left': mouseX }).fadeIn('slow');
                                        }
                                        catch (err) {
                                           // alert(err.message);
                                        }
                                    }, function () {
                                        try {
                                            $(this).find('.js-tooltip').hide();
                                            $(this).find('.js-tooltip').removeClass('ajax-tooltip');
                                            $(this).find('.ajax-tooltip').css({ 'top': mouseY, 'left': mouseX }).fadeOut('slow');
                                        }
                                        catch (err) {
                                            //alert(err.message);
                                        }
                                    });
       
                                });";



        if (PageType == "p")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strCellToolTip", strCellToolTip, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strCellToolTip" + (DetailTabIndex - 1).ToString(), strCellToolTip, true);
        }

        // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strCellToolTip", strCellToolTip, true);


        if (PageType == "c")
        {
            strXX = @"

                              $(document).ready(function () {
                                  function ShowHide" + (DetailTabIndex - 1).ToString() + @"() {

                                    var chk = document.getElementById('" + chkShowAdvancedOptions.ClientID + @"');
                                    var x = document.getElementById('" + tblAdvancedOption.ClientID + @"');
                                    var trChkOnlyWarning = document.getElementById('" + trChkShowOnlyWarning.ClientID + @"');
                                    
                                    var tdFilterDynamic = document.getElementById('" + tdFilterDynamic.ClientID + @"');
                                    var tdFilterYAxis = document.getElementById('" + tdFilterYAxis.ClientID + @"');

                                    if (chk!=null &&  chk.checked == false) {  
                                           tdFilterDynamic.style.display = 'inline';tdFilterYAxis.style.display = 'none';
                                           trChkOnlyWarning.style.display = 'none';
                                        }
                                    if (chk!=null && chk.checked == true) { 
                                            tdFilterDynamic.style.display = 'none';tdFilterYAxis.style.display = 'inline';
                                            trChkOnlyWarning.style.display = 'table-row';
                                        } 

 
                                    if (chk!=null && chk.checked == false) { x.style.display = 'none'; }
                                    if (chk!=null && chk.checked == true) { x.style.display = 'inline'; } 
                                        var hfHideAdvancedOption = document.getElementById('" + hfHideAdvancedOption.ClientID + @"');
                                    if (hfHideAdvancedOption!=null && hfHideAdvancedOption.value == 'yes') {
                                        //$('#" + tblAdvancedOption.ClientID + @"').fadeOut();
                                       // $('#" + tblAdvancedOptionChk.ClientID + @"').fadeOut();
                                        var x1 = document.getElementById('" + tblAdvancedOptionChk.ClientID + @"');
                                        x1.style.display = 'none';
                                        var x2 = document.getElementById('" + tblAdvancedOptionChkC.ClientID + @"');
                                        x2.style.display = 'none';
                                    }            
                                }
                                ShowHide" + (DetailTabIndex - 1).ToString() + @"();
//                                   if (window.addEventListener)
//                                    window.addEventListener('load', ShowHide" + (DetailTabIndex - 1).ToString() + @", false);
//                                else if (window.attachEvent)
//                                    window.attachEvent('onload', ShowHide" + (DetailTabIndex - 1).ToString() + @");
//                                else if (document.getElementById)
//                                    window.onload = ShowHide" + (DetailTabIndex - 1).ToString() + @";
                         });


                         $(document).ready(function () {


                                    $('#" + chkShowAdvancedOptions.ClientID + @"').click(function () {
                                        var chk = document.getElementById('" + chkShowAdvancedOptions.ClientID + @"');

                                    var tdFilterDynamic = document.getElementById('" + tdFilterDynamic.ClientID + @"');
                                     var tdFilterYAxis = document.getElementById('" + tdFilterYAxis.ClientID + @"');

                                        if (chk.checked == true) {
                                            $('#" + tblAdvancedOption.ClientID + @"').fadeIn();
                                             tdFilterDynamic.style.display = 'none';tdFilterYAxis.style.display = 'inline';

                                        }
                                        else {
                                            $('#" + tblAdvancedOption.ClientID + @"').fadeOut();
                                           tdFilterDynamic.style.display = 'inline';tdFilterYAxis.style.display = 'none';

                                        }
                                    });  

                                $('#" + chkDeleteParmanent.ClientID + @"').click(function () {
                                        var chk = document.getElementById('" + chkDeleteParmanent.ClientID + @"');
                                        if (chk.checked == true) {
                                            $('#" + trUndo.ClientID + @"').fadeIn();

                                        }
                                        else {
                                            $('#" + trUndo.ClientID + @"').fadeOut();
                                             var chkUndo = document.getElementById('" + chkUndo.ClientID + @"');
                                            chkUndo.checked=false;
                                        }
                                    });
                         if( document.getElementById('" + hfParmanentDelete.ClientID + @"').value=='yes' && document.getElementById('" + chkDelateAllEvery.ClientID + @"')==null)
                                         {
                                            $('#" + trUndo.ClientID + @"').fadeIn();
                                            }
                        $('#" + chkDelateAllEvery.ClientID + @"').click(function () {
                                            if( document.getElementById('" + hfParmanentDelete.ClientID + @"').value=='yes')
                                            {
                                                    var chk = document.getElementById('" + chkDelateAllEvery.ClientID + @"');
                                                    if (chk.checked == true) {
                                                        $('#" + trUndo.ClientID + @"').fadeIn();

                                                    }
                                            }

                                    });
                               

                                });

                        ";
        }

        if (this.Page.MasterPageFile != null && this.Page.MasterPageFile.ToLower().IndexOf("his") > -1)
        {
            //divBatches.Visible = false;
            //divConfig.Visible = false;
            //divEmail.Visible = false;
            //divShowGraph.Visible = false;
            //divUpload.Visible = false;
            //lblTitle.Visible = false;
            trRecordListTitle.Visible = false;
        }

        if (PageType == "p")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JSCode", strXX, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JSCode" + (DetailTabIndex - 1).ToString(), strXX, true);
        }




        string strFancy = @"
                    $(function () {
                            $('.popuplink2').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 1000,
                                height: 800,
                                titleShow: false,                        
                              onClosed: function () { $('#loadingredirect').fadeIn(); window.parent.location.reload();}
                            });
                        });

                ";

        // onClosed: function () { window.parent.document.getElementById('btnReloadMe').click();}
        if (Request.RawUrl.IndexOf("EachRecordTable.aspx") > -1)
        {
            //_strNoAjaxView = "noajax=yes&";
        }
        else
        {
            //_strNoAjaxView = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strFancy", strFancy, true);
        }
        if (IsPostBack && Request.Params["__EVENTTARGET"] != null && Request.Params["__EVENTTARGET"].ToString() == "")
        {
            mpeExportRecords.Hide();
        }


        ControlCollection _collection = this.Controls;
        //lblMsg.Visible = true;
        //lblMsg.Text = TheDatabase.GetCode(_collection, _strDynamictabPart).Replace("\r\n", "<br/>");
        TheDatabase.SetValidationGroup(_collection, _strDynamictabPart);
        SetOtherValidationGroup();
        //put speed test here       
       
    EndSub:
        int iFXX;

    }

    protected void EnsureSecurity()
    {
        GridViewRow gvr = gvTheGrid.TopPagerRow;
        if (_strRecordRightID == Common.UserRoleType.ReadOnly)
        {
            //gvTheGrid.Columns[0].Visible = false;//delete check
            gvTheGrid.Columns[2].Visible = false;//edit   
            gvTheGrid.Columns[3].Visible = true;//view
            divAddAndViewControls.Visible = false;
            if (_theView != null)
            {
                if ((bool)_theView.ShowViewIcon == false)
                    gvTheGrid.Columns[3].Visible = false;//view
            }

            divEmptyData.Visible = false;
            //_bReadOnly = true;
            divUpload.Visible = false;

            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.HideAdd = true;
                _gvPager.HideDelete = true;
            }
        }
        else if (_strRecordRightID == Common.UserRoleType.None)
        {
            Response.Redirect("~/Empty.aspx", true);
            return;
        }
        else if (_strRecordRightID == Common.UserRoleType.OwnData)
        {
            //gvTheGrid.Columns[0].Visible = false;//delete check
            gvTheGrid.Columns[2].Visible = true;//edit   
            gvTheGrid.Columns[3].Visible = true;//view
            divEmptyData.Visible = false;
            divAddAndViewControls.Visible = false;
            if (_theView != null)
            {
                if ((bool)_theView.ShowViewIcon == false)
                    gvTheGrid.Columns[3].Visible = false;//view
                if ((bool)_theView.ShowEditIcon == false)
                    gvTheGrid.Columns[2].Visible = false;//edit
            }

            //_bReadOnly = false;
            divUpload.Visible = true;
            //hlLocations.Visible = false;
            divEmail.Visible = true;

            if (_bShowGraphIcon)
                divShowGraph.Visible = true;

            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.HideAdd = true;
                _gvPager.HideDelete = true;
            }
        }
        else if (_strRecordRightID == Common.UserRoleType.EditOwnViewOther)
        {
            //gvTheGrid.Columns[0].Visible = false;//delete check
            gvTheGrid.Columns[2].Visible = true;//edit   
            gvTheGrid.Columns[3].Visible = true;//view
            //divEmptyData.Visible = false;
            //divAddAndViewControls.Visible = false;
            if (_theView != null)
            {
                if ((bool)_theView.ShowViewIcon == false)
                    gvTheGrid.Columns[3].Visible = false;//view
                if ((bool)_theView.ShowEditIcon == false)
                    gvTheGrid.Columns[2].Visible = false;//edit
            }
            divUpload.Visible = true;

            divEmail.Visible = true;
            if (_bShowGraphIcon)
                divShowGraph.Visible = true;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                //_gvPager.HideAdd = true;
                _gvPager.HideDelete = true;
            }
        }
        else if (_strRecordRightID == Common.UserRoleType.AddRecord)
        {
            //gvTheGrid.Columns[0].Visible = false;//delete check


            gvTheGrid.Columns[2].Visible = false;//edit   
            gvTheGrid.Columns[3].Visible = true;//view

            if (_theView != null)
            {
                if ((bool)_theView.ShowViewIcon == false)
                    gvTheGrid.Columns[3].Visible = false;//view
            }
            //_bReadOnly = true;
            divUpload.Visible = true;

            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

                _gvPager.HideDelete = true;
            }




        }
        else
        {
            gvTheGrid.Columns[2].Visible = true;//edit   
            gvTheGrid.Columns[3].Visible = false;//view

            if (_theView != null)
            {
                if ((bool)_theView.ShowEditIcon == false)
                    gvTheGrid.Columns[2].Visible = false;//edit
            }


            if (_theView != null)
            {
                if ((bool)_theView.ShowAddIcon == false)
                {
                    divEmptyData.Visible = false;

                }
            }

        }
    }

    protected string GetRecursiveDataScope(int iParentTableID, int iScopeTableID, string strReocrdIDs)
    {
        int iCurrentTableID = int.Parse(_qsTableID);

        DataTable dtChildTable = Common.DataTableFromText("	SELECT DISTINCT ChildTableID FROM TableChild WHERE ParentTableID=" + iParentTableID);

        bool bFoundLevel = false;



        if (dtChildTable.Rows.Count > 0)
        {

            foreach (DataRow drCT in dtChildTable.Rows)
            {
                if (iCurrentTableID == int.Parse(drCT[0].ToString()))
                {
                    bFoundLevel = true;
                }
            }

            foreach (DataRow drCT in dtChildTable.Rows)
            {
                int iChildTableID = int.Parse(drCT[0].ToString());
                DataTable dtChildColumn = Common.DataTableFromText(@"	SELECT ColumnID FROM [Column] WHERE ColumnType='dropdown' 
                    AND (Dropdowntype='table' OR Dropdowntype='tabledd')
	                AND TableID=" + iChildTableID.ToString() + @" AND TableTableID=" + iParentTableID.ToString());

                if (dtChildColumn.Rows.Count > 0)
                {

                    //we have a column
                    Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(dtChildColumn.Rows[0][0].ToString()));
                    Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                    string strLinkedValues = "";
                    foreach (string strARecord in strReocrdIDs.Split(','))
                    {
                        if (!string.IsNullOrEmpty(strARecord))
                        {
                            Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strARecord));
                            string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);
                            if (theLinkedColumn.SystemName.ToLower() == "recordid")
                            {
                                //
                            }
                            else
                            {
                                strLinkedColumnValue = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                            }
                            strLinkedValues = strLinkedValues + strLinkedColumnValue + ",";
                        }
                    }
                    if (strLinkedValues != "")
                        strLinkedValues = strLinkedValues.Substring(0, strLinkedValues.LastIndexOf(','));

                    DataTable dtChildRecordID = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theChildColumn.TableID.ToString() + " AND  " + theChildColumn.SystemName + " IN (" + strLinkedValues + ")");

                    //here we will return filter SQL

                    string strChildRecordIDs = "";
                    foreach (DataRow dr in dtChildRecordID.Rows)
                    {
                        strChildRecordIDs = strChildRecordIDs + dr[0].ToString() + ",";
                    }
                    if (strChildRecordIDs != "")
                        strChildRecordIDs = strChildRecordIDs.Substring(0, strChildRecordIDs.LastIndexOf(','));


                    if (iCurrentTableID == (int)theChildColumn.TableID)
                    {
                        return " AND Record.RecordID IN (" + strChildRecordIDs + ")";
                    }
                    else
                    {
                        if (bFoundLevel == false)
                        {
                            string strWhere = GetRecursiveDataScope((int)theChildColumn.TableID, iScopeTableID, strChildRecordIDs);
                            if (strWhere != "")
                            {
                                return strWhere;
                            }
                        }
                    }


                }
                else
                {
                    //no link
                }
            }
        }
        else
        {
            //no child tables
        }

        return "";
    }

    protected string GetDataScopeWhere(int iParentTableID, int iScopeTableID, string strScopeSystemName)
    {
        int iCurrentTableID = int.Parse(_qsTableID);

        DataTable dtRecordID = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + iScopeTableID + " AND  " + strScopeSystemName + "='" + _theUserRole.DataScopeValue.Replace("'", "''") + "'");
        string strScopeRecordID = "";
        if (dtRecordID.Rows.Count > 0)
        {
            strScopeRecordID = dtRecordID.Rows[0][0].ToString();
        }

        if (iCurrentTableID == iScopeTableID)
        {
            return " AND Record." + strScopeSystemName + " ='" + _theUserRole.DataScopeValue.Replace("'", "''") + "'";
        }
        else
        {
            DataTable dtChildTable = Common.DataTableFromText("	SELECT DISTINCT ChildTableID FROM TableChild WHERE ParentTableID=" + iParentTableID);

            bool bFoundLevel = false;

            if (dtChildTable.Rows.Count > 0)
            {

                foreach (DataRow drCT in dtChildTable.Rows)
                {
                    if (iCurrentTableID == int.Parse(drCT[0].ToString()))
                    {
                        bFoundLevel = true;
                    }
                }

                foreach (DataRow drCT in dtChildTable.Rows)
                {


                    int iChildTableID = int.Parse(drCT[0].ToString());
                    DataTable dtChildColumn = Common.DataTableFromText(@"	SELECT ColumnID FROM [Column] WHERE ColumnType='dropdown' 
                    AND (Dropdowntype='table' OR Dropdowntype='tabledd')
	                AND TableID=" + iChildTableID.ToString() + @" AND TableTableID=" + iParentTableID.ToString());



                    if (dtChildColumn.Rows.Count > 0)
                    {

                        //we have a column
                        Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(dtChildColumn.Rows[0][0].ToString()));
                        Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);


                        Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strScopeRecordID));
                        string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);
                        strLinkedColumnValue = strLinkedColumnValue.Replace("'", "''");

                        DataTable dtChildRecordID = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theChildColumn.TableID.ToString() + " AND  " + theChildColumn.SystemName + "='" + strLinkedColumnValue + "'");

                        //here we will return filter SQL

                        string strChildRecordIDs = "";
                        foreach (DataRow dr in dtChildRecordID.Rows)
                        {
                            strChildRecordIDs = strChildRecordIDs + dr[0].ToString() + ",";
                        }
                        if (strChildRecordIDs != "")
                            strChildRecordIDs = strChildRecordIDs.Substring(0, strChildRecordIDs.LastIndexOf(','));


                        if (iCurrentTableID == (int)theChildColumn.TableID)
                        {
                            if (strChildRecordIDs == "")
                                strChildRecordIDs = "-1";
                            return " AND Record.RecordID IN (" + strChildRecordIDs + ")";
                        }
                        else
                        {
                            if (bFoundLevel == false)
                            {
                                string strWhere = GetRecursiveDataScope((int)theChildColumn.TableID, iScopeTableID, strChildRecordIDs);
                                if (strWhere != "")
                                {
                                    return strWhere;
                                }
                            }
                        }


                    }
                    else
                    {
                        //no link
                    }
                }
            }
            else
            {
                //no child tables
            }

        }

        return "";
    }

    //protected void PopulateFilterColumn(Table theSammpleType)
    //{
    //    if (theSammpleType.FilterColumnID != null)
    //    {
    //        Column theColumn = RecordManager.ets_Column_Details((int)theSammpleType.FilterColumnID);


    //            tblFilterByColumn.Visible = true;

    //            if (theSammpleType.HideFilter != null)
    //            {
    //                if ((bool)theSammpleType.HideFilter == true)
    //                {
    //                    trSummaryFilter.Visible = false;
    //                }
    //            }

    //            lblFilterColumnName.Text = theColumn.DisplayTextSummary + ":";
    //            hfFilterColumnSystemName.Value = theColumn.SystemName;

    //            cbcvSumFilter.ddlYAxisV = theSammpleType.FilterColumnID.ToString();

    //            if (theSammpleType.FilterDefaultValue != null && theSammpleType.FilterDefaultValue != "")
    //            {
    //                cbcvSumFilter.SetValue = theSammpleType.FilterDefaultValue;
    //            }          



    //    }

    //}

    protected string GetEditViewPageURL()
    {
        string strExtra = (((_theView.UserID == _ObjUser.UserID) || _strViewPageType == "dash") ? "" : "&Copy=Yes");

        if (Request.QueryString["ViewID"] != null || Request.QueryString["View"] != null)
        {
            strExtra = "";
        }

        return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/ViewEditPage.aspx?" + _strNoAjaxView + "TableID=" + Cryptography.Encrypt(_qsTableID)
                        + "&ViewSession=" + Cryptography.Encrypt(_strViewSession)
                        + "&ViewID=" + _theView.ViewID.ToString() + (PageType == "c" ? "&tabindex=" + DetailTabIndex.ToString() : "")
                        + ((_strViewPageType == "child" && _iParentTableID != null) ? "&ParentTableID=" + Cryptography.Encrypt(_iParentTableID.ToString()) : "")
                        + strExtra;
    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        PopulateSearchParams();

        if (hfViewID.Value != "")
        {
            hlEditView.NavigateUrl = GetEditViewPageURL();
            hlEditView2.NavigateUrl = hlEditView.NavigateUrl;

            if (_strNoAjaxView != "")
            {
                hlEditView.Target = "_parent";
                hlEditView2.Target = "_parent";

            }
        }



        _bIsForExport = false;
        lblMsg.Text = "";
        if (TableID.ToString() == "")
            return;





        try
        {


            int iTN = 0;

            string strOrderDirection = "DESC";
            string sOrder = GetDataKeyNames()[0];
            string sParentColumnSortSQL = "";
            if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
            {
                strOrderDirection = "ASC";
            }
            if (gvTheGrid.GridViewSortColumn != "")
            {
                sOrder = gvTheGrid.GridViewSortColumn;

            }
            else
            {
                strOrderDirection = "DESC";
                gvTheGrid.GridViewSortDirection = SortDirection.Descending;
            }




            string strDynamicSearchXMLPart = "";
            if (chkShowAdvancedOptions.Checked == false)
            {
                strDynamicSearchXMLPart = GetDynamicSeachXMLPart();
            }

            string strOtherXMLTags = @" <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                   " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                 " <sOrder>" + HttpUtility.HtmlEncode(sOrder) + "</sOrder>" +
                 " <strOrderDirection>" + HttpUtility.HtmlEncode(strOrderDirection) + "</strOrderDirection>" +
                 " <sParentColumnSortSQL>" + HttpUtility.HtmlEncode(sParentColumnSortSQL) + "</sParentColumnSortSQL>";

            strOtherXMLTags = strOtherXMLTags + strDynamicSearchXMLPart;

            UpdateSearchCriteriaForTheGrid(strOtherXMLTags);


            //put speed test here       
            if (Session["RunSpeedLog"] != null && _theTable != null)
            {
                SpeedLog theSpeedLog = new SpeedLog();
                theSpeedLog.FunctionName = _theTable.TableName + "ets_Record_List - START ";
                theSpeedLog.FunctionLineNumber = 1950;
                SecurityManager.AddSpeedLog(theSpeedLog);

            }


            string strReturnSQL = "";
            _dtDataSource = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                !chkIsActive.Checked,
                chkShowOnlyWarning.Checked == false ? null : (bool?)true, null, null,
                sOrder, strOrderDirection, iStartIndex, iMaxRows, ref iTN, ref _iTotalDynamicColumns, _strListType, _strNumericSearch, TextSearch + TextSearchParent,
               _dtDateFrom, _dtDateTo, sParentColumnSortSQL, "", _strViewName, int.Parse(hfViewID.Value), ref strReturnSQL, ref strReturnSQL);




            //put speed test here       

            if (Session["RunSpeedLog"] != null && _theTable != null)
            {
                SpeedLog theSpeedLog = new SpeedLog();
                theSpeedLog.FunctionName = _theTable.TableName + "SP ets_Record_List - END ";
                theSpeedLog.FunctionLineNumber = 1960;
                SecurityManager.AddSpeedLog(theSpeedLog);

            }


            //remove the parent sort column here

            if (sParentColumnSortSQL != "" && _dtDataSource != null)
            {
                _dtDataSource.Columns.RemoveAt(_dtDataSource.Columns.Count - 2);
                _dtDataSource.AcceptChanges();
                _iTotalDynamicColumns = _iTotalDynamicColumns - 1;
            }

            _dtRecordColums = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));

            _dtDataSource.AcceptChanges();

            gvTheGrid.DataSource = _dtDataSource;

            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();


            ///
            hfUsingScrol.Value = "";
            if (PageType == "p" && Request.RawUrl.IndexOf("RecordList.aspx") > -1 && gvTheGrid.PageSize > 20 && iTN > 20
                && _theView != null && _theView.ShowFixedHeader != null && (bool)_theView.ShowFixedHeader)
            {
                hfUsingScrol.Value = "yes";

                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Key", "<script>SetStyleEvent();MakeStaticHeader('" + gvTheGrid.ClientID + "', 600, 1100 , 90 ,false); </script>", false);
                string strStaticheaderJS = @"

                             $(document).ready(function () {
        



                                        $.fn.visibleHeight = function () {
                                            var elBottom, elTop, scrollBot, scrollTop, visibleBottom, visibleTop;
                                            scrollTop = $(window).scrollTop();
                                            scrollBot = scrollTop + $(window).height();
                                            elTop = this.offset().top;
                                            elBottom = elTop + this.outerHeight();
                                            visibleTop = elTop < scrollTop ? scrollTop : elTop;
                                            visibleBottom = elBottom > scrollBot ? scrollBot : elBottom;
                                            return visibleBottom - visibleTop
                                        }




                                function SetStyleEvent() {

                                    $('#ctl00_HomeContentPlaceHolder_rlOne_UpdateProgress1').fadeIn();

                                    var DivPR = document.getElementById('DivPagerRow');
                                    var DivHR = document.getElementById('DivHeaderRow');
                                    var DivMC = document.getElementById('DivMainContent');
                                    var DivFR = document.getElementById('DivFooterRow');
                                    var DivR = document.getElementById('DivRoot');

                                    DivMC.style.overflowY = 'auto';
                                    DivMC.style.overflowX = 'hidden';
                                  

                                    DivMC.style.paddingRight = '17px';
                                    DivHR.style.paddingRight = '17px'; 
                                   DivPR.style.paddingRight = '17px';
//                                   DivMC.style.marginRight=-17px;


                                    var tbl = document.getElementById('ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid');


                                    var chkAll = $(tbl.rows[1]).find('#ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid_ctl02_chkAll');
                                    $(chkAll).attr('onclick', 'javascript: SelectAllCheckboxesHR(this,ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid);');
                    

                                }


                                 function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
                                    var tbl = document.getElementById(gridId);
                                    height = $('#DivMainContent').visibleHeight();
                                    height = height - 65;
                                    if (tbl) {
                                        var DivPR = document.getElementById('DivPagerRow');
                                        var DivHR = document.getElementById('DivHeaderRow');
                                        var DivMC = document.getElementById('DivMainContent');
                                        var DivFR = document.getElementById('DivFooterRow');
                                        var DivR = document.getElementById('DivRoot');

                                        //*** Set divheaderRow Properties ****
                                        var oWidth = $(tbl).outerWidth();
                                        width = $(tbl).width();
                                       var iWidth = $(tbl).innerWidth();
                                        headerHeight = DivHR.style.height;
                                        var paregHeight = 45;

                                        //pager
                                        DivPR.style.height = paregHeight + 'px'; // headerHeight / 2

                                        DivPR.style.position = 'relative';
                                        DivPR.style.top = '0px';
                                        //DivPR.style.verticalAlign = 'top';

                                        DivHR.style.height = headerHeight + 'px';// headerHeight/2

                                        DivHR.style.position = 'relative';
                                        DivHR.style.top = '0px';
                                        // DivHR.style.verticalAlign = 'top';
                                        //DivHR.rules = 'none';


                                        //*** Set divMainContent Properties ****
                                        DivMC.style.height = height + 'px';
                                        DivMC.style.position = 'relative';
                                        DivMC.style.top = '0px'; //(headerHeight) + 'px';// //
                                        DivMC.style.zIndex = '0';




                                        if (isFooter) {
                                            //*** Set divFooterRow Properties ****
                                            DivFR.style.width = (parseInt(width)) + 'px';
                                            DivFR.style.position = 'relative';
                                            DivFR.style.top = -(headerHeight) + 'px';
                                            DivFR.style.verticalAlign = 'top';
                                            DivFR.style.paddingtop = '2px';


                                            var tblfr = tbl.cloneNode(true);
                                            tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
                                            var tblBody = document.createElement('tbody');
                                            tblfr.style.width = '100%';
                                            tblfr.cellSpacing = '0';
                                            tblfr.border = '0px';
                                            tblfr.rules = 'none';
                                            //*****In the case of Footer Row *******
                                            tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
                                            tblfr.appendChild(tblBody);
                                            DivFR.appendChild(tblfr);
                                        }
                                        //****Copy Header in divHeaderRow****

                                        var tblPR = tbl.cloneNode(true);
                                        tblPR.removeChild(tblPR.getElementsByTagName('tbody')[0]);
                                        $(tblPR).attr('id', gridId + 'PR');

                                        var tblHR = tbl.cloneNode(true);
                                        tblHR.removeChild(tblHR.getElementsByTagName('tbody')[0]);
                                        $(tblHR).attr('id', gridId + 'HR');


                                        var tblBodyPR = document.createElement('tbody');
                                        var tblBodyHR = document.createElement('tbody');


                                        tblBodyPR.appendChild(tbl.rows[0]);
                                        tblBodyHR.appendChild(tbl.rows[0]);
               

                                        tblPR.appendChild(tblBodyPR);
                                        tblHR.appendChild(tblBodyHR);

                                        DivPR.appendChild(tblPR);
                                        DivHR.appendChild(tblHR);
                                       
                                    var iTD0 = 0;
                                    $(tbl.rows[0]).find('td').each(function () {

                                        var aTH = $(tblHR.rows[0]).find('th').eq(iTD0);
                                        var aTD = $(tbl.rows[0]).find('td').eq(iTD0);                
                                        var iFirstWidth=$(tbl.rows[0]).find('td').eq(iTD0).width();
                                        var iColumnWidth=$(tblHR.rows[0]).find('th').eq(iTD0).width();
                                        if(iColumnWidth>iFirstWidth)
                                            {iFirstWidth=iColumnWidth;}

                                        $(aTD).css({ minWidth: iFirstWidth+'px' });
                                        $(aTD).css({ maxWidth: iFirstWidth+'px' });
                                        $(aTH).css({ minWidth: iFirstWidth+'px' });
                                        $(aTH).css({ maxWidth: iFirstWidth+'px' });

                                        iTD0 = iTD0 + 1;
                                    }); 
                                       
                                        $('#ctl00_HomeContentPlaceHolder_rlOne_UpdateProgress1').fadeOut();
                                    }
                                }



                                     $('#loadingredirect').fadeIn();
                                    SetStyleEvent();
                                    MakeStaticHeader('" + gvTheGrid.ClientID + @"', 600, 1100 , 90 ,false);
                                    $('#loadingredirect').fadeOut();

                            });
 
                        ";


                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strStaticheaderJS_key", strStaticheaderJS, true);


                lnkEditManyCancel.Visible = false;
                lnkEditManyCancel2.Visible = true;
                mpeEditMany.OkControlID = "";


                lnkAddRecordCancel.Visible = false;
                lnkAddRecordCancel2.Visible = true;
                mpeAddRecord.OkControlID = "";

                lnkExportRecordsCancel.Visible = false;
                lnkExportRecordsCancel2.Visible = true;
                mpeExportRecords.OkControlID = "";
            }

            ////

            if (_dtDataSource == null)
            {
                //divEmptyData.Visible = true;

                if (IsFiltered())
                {
                    divNoFilter.Visible = true;
                    divEmptyData.Visible = false;

                }
                else
                {
                    if (_strRecordRightID == Common.UserRoleType.ReadOnly)
                    {
                        divEmptyData.Visible = false;
                    }
                    else
                    {
                        divEmptyData.Visible = true;
                    }
                    divNoFilter.Visible = false;
                }
                hplNewData.NavigateUrl = GetAddURL();

                hplNewDataFilter.NavigateUrl = GetAddURL();
                hplNewDataFilter2.NavigateUrl = hplNewDataFilter.NavigateUrl;

                return;
            }

            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.AddURL = GetAddURL();

                if (hfHidePagerGoButton.Value == "yes")
                {
                    _gvPager.HidePagerGoButton = true;
                }

                if (_gvPager != null)
                {
                    int iViewID = -1;
                    if (hfViewID.Value != "")
                    {
                        iViewID = int.Parse(hfViewID.Value);

                    }

                    _gvPager.EditViewCSSClass = "popuplink2";
                    _gvPager.EditViewToolTip = "Edit View";
                    _gvPager.EditViewURL = GetEditViewPageURL();

                    if (_theTable.AllowCopyRecords != null && (bool)_theTable.AllowCopyRecords)
                    {
                        _gvPager.ShowCopyRecord = true;
                    }


                    if (_strNoAjaxView != "")
                    {
                        _gvPager.EditViewTarget = "_parent";

                    }
                    //_gvPager.EditViewURL = "#";
                    //_gvPager.EditViewOnClick = "window.parent.callMe('" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/ViewEditPage.aspx?TableID=" + Cryptography.Encrypt(_qsTableID) + "&ViewID=" + iViewID.ToString() + "');return false;";

                }

                if (!IsPostBack)
                {
                    string strBulkUpdateSQL = SystemData.SystemOption_ValueByKey_Account("BulkUpdateSQL", null, int.Parse(_qsTableID));

                    if (strBulkUpdateSQL != "")
                    {
                        string strEditManyTooltip = SystemData.SystemOption_NotesByKey_Account("BulkUpdateSQL", null, int.Parse(_qsTableID)); ;
                        if (strEditManyTooltip != "")
                        {
                            _gvPager.EditManyToolTip = strEditManyTooltip;
                        }
                    }
                }

                //if (PageType == "p")
                //{

                if (TableID != null)
                {
                    DataTable dtSendEmailCol = Common.DataTableFromText(@"SELECT ColumnID FROM [Column] WHERE TableID=" + TableID.ToString() + @" AND ColumnType='text' 
                                            AND (TextType='email' OR TextType='mobile')");

                    if (dtSendEmailCol.Rows.Count > 0)
                    {
                        _gvPager.HideSendEmail = false;
                    }
                }
                //}

                _gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
                if (chkIsActive.Checked)
                {
                    _gvPager.HideDelete = true;
                    _gvPager.HideUnDelete = false;
                    if (_strRecordRightID == Common.UserRoleType.Administrator
                               || _strRecordRightID == Common.UserRoleType.GOD)
                    {

                        _gvPager.HideParmanentDelete = false;

                    }
                }
                else
                {
                    _gvPager.HideDelete = false;
                    _gvPager.HideUnDelete = true;

                }
                ShowHidePermanentDelete();
            }


            if (iTN == 0)
            {
                if (IsFiltered())
                {
                    divNoFilter.Visible = true;
                    divEmptyData.Visible = false;

                    if (_theView != null)
                    {
                        if (_theView.ShowAddIcon == false)
                        {
                            divNoFilter.Visible = false;
                        }
                    }
                }
                else
                {
                    divNoFilter.Visible = false;
                }
            }
            else
            {
                divNoFilter.Visible = false;
            }

            if (_strRecordRightID == Common.UserRoleType.ReadOnly)
            {
                //gvTheGrid.Columns[0].Visible = false;//delete check
                gvTheGrid.Columns[2].Visible = false;//edit   
                gvTheGrid.Columns[3].Visible = true;//view
                divAddAndViewControls.Visible = false;
                if (_theView != null)
                {
                    if ((bool)_theView.ShowViewIcon == false)
                        gvTheGrid.Columns[3].Visible = false;//view
                }

                divEmptyData.Visible = false;
                //_bReadOnly = true;
                divUpload.Visible = false;

                if (gvr != null)
                {
                    _gvPager = (Common_Pager)gvr.FindControl("Pager");
                    _gvPager.HideAdd = true;
                    _gvPager.HideDelete = true;
                }
            }
            else if (_strRecordRightID == Common.UserRoleType.None)
            {
                Response.Redirect("~/Empty.aspx", true);
                return;
            }
            else if (_strRecordRightID == Common.UserRoleType.OwnData)
            {
                //gvTheGrid.Columns[0].Visible = false;//delete check
                gvTheGrid.Columns[2].Visible = true;//edit   
                gvTheGrid.Columns[3].Visible = true;//view
                divEmptyData.Visible = false;
                divAddAndViewControls.Visible = false;
                if (_theView != null)
                {
                    if ((bool)_theView.ShowViewIcon == false)
                        gvTheGrid.Columns[3].Visible = false;//view
                    if ((bool)_theView.ShowEditIcon == false)
                        gvTheGrid.Columns[2].Visible = false;//edit
                }

                //_bReadOnly = false;
                divUpload.Visible = true;
                //hlLocations.Visible = false;
                divEmail.Visible = true;

                if (_bShowGraphIcon)
                    divShowGraph.Visible = true;

                if (gvr != null)
                {
                    _gvPager = (Common_Pager)gvr.FindControl("Pager");
                    _gvPager.HideAdd = true;
                    _gvPager.HideDelete = true;
                }
            }
            else if (_strRecordRightID == Common.UserRoleType.EditOwnViewOther)
            {
                //gvTheGrid.Columns[0].Visible = false;//delete check
                gvTheGrid.Columns[2].Visible = true;//edit   
                gvTheGrid.Columns[3].Visible = true;//view
                //divEmptyData.Visible = false;
                //divAddAndViewControls.Visible = false;
                if (_theView != null)
                {
                    if ((bool)_theView.ShowViewIcon == false)
                        gvTheGrid.Columns[3].Visible = false;//view
                    if ((bool)_theView.ShowEditIcon == false)
                        gvTheGrid.Columns[2].Visible = false;//edit
                }
                divUpload.Visible = true;

                divEmail.Visible = true;
                if (_bShowGraphIcon)
                    divShowGraph.Visible = true;
                if (gvr != null)
                {
                    _gvPager = (Common_Pager)gvr.FindControl("Pager");
                    //_gvPager.HideAdd = true;
                    _gvPager.HideDelete = true;
                }
            }
            else if (_strRecordRightID == Common.UserRoleType.AddRecord)
            {
                //gvTheGrid.Columns[0].Visible = false;//delete check


                gvTheGrid.Columns[2].Visible = false;//edit   
                gvTheGrid.Columns[3].Visible = true;//view

                if (_theView != null)
                {
                    if ((bool)_theView.ShowViewIcon == false)
                        gvTheGrid.Columns[3].Visible = false;//view
                }
                //_bReadOnly = true;
                divUpload.Visible = true;

                if (gvr != null)
                {
                    _gvPager = (Common_Pager)gvr.FindControl("Pager");

                    _gvPager.HideDelete = true;
                }

                if (iTN == 0)
                {
                    if (IsFiltered())
                    {
                        divNoFilter.Visible = true;
                        divEmptyData.Visible = false;

                    }
                    else
                    {
                        if (_strRecordRightID == Common.UserRoleType.ReadOnly)
                        {
                            divEmptyData.Visible = false;
                        }
                        else
                        {
                            divEmptyData.Visible = true;
                        }
                        divNoFilter.Visible = false;
                    }
                    hplNewData.NavigateUrl = GetAddURL();

                    hplNewDataFilter.NavigateUrl = GetAddURL();
                    hplNewDataFilter2.NavigateUrl = hplNewDataFilter.NavigateUrl;

                    if (_theView != null)
                    {
                        if ((bool)_theView.ShowAddIcon == false)
                        {
                            divEmptyData.Visible = false;
                            divNoFilter.Visible = false;
                        }
                    }

                }
                else
                {

                    divEmptyData.Visible = false;
                    divNoFilter.Visible = false;
                }


            }
            else
            {
                gvTheGrid.Columns[2].Visible = true;//edit   
                gvTheGrid.Columns[3].Visible = false;//view

                if (_theView != null)
                {
                    if ((bool)_theView.ShowEditIcon == false)
                        gvTheGrid.Columns[2].Visible = false;//edit
                }
                if (iTN == 0)
                {

                    if (_strRecordRightID == Common.UserRoleType.ReadOnly)
                    {
                        divEmptyData.Visible = false;
                    }
                    else
                    {
                        divEmptyData.Visible = true;
                    }
                    hplNewData.NavigateUrl = GetAddURL();
                    hplNewDataFilter.NavigateUrl = GetAddURL();
                    hplNewDataFilter2.NavigateUrl = hplNewDataFilter.NavigateUrl;
                }
                else
                {
                    divEmptyData.Visible = false;
                }

                if (_theView != null)
                {
                    if ((bool)_theView.ShowAddIcon == false)
                    {
                        divEmptyData.Visible = false;

                    }
                }

            }


            if (iTN == 0)
            {
                hplNewData.NavigateUrl = GetAddURL();
                if (ShowAddButton)
                {
                    if (_strRecordRightID == Common.UserRoleType.ReadOnly)
                    {
                        divEmptyData.Visible = false;
                    }
                    //else
                    //{
                    //    divEmptyData.Visible = true;
                    //}
                }

                if (_theView != null)
                {
                    if ((bool)_theView.ShowAddIcon == false)
                    {
                        divEmptyData.Visible = false;

                    }
                }
            }
            else
            {
                divEmptyData.Visible = false;

                //if (ShowEditButton == false)
                //{
                //    gvTheGrid.Columns[2].Visible = false;
                //}

            }

            if (_gvPager != null)
            {

                if (PageType == "c")
                {
                    if (ShowAddButton == false)
                    {
                        _gvPager.HideAdd = true;

                    }
                    _gvPager.HideAllExport = true;
                }
                else
                {
                    if (_strRecordRightID == Common.UserRoleType.Administrator
                        || _strRecordRightID == Common.UserRoleType.GOD)
                    {
                        if (chkIsActive.Checked)
                        {
                            _gvPager.HideParmanentDelete = false;
                            _gvPager.HideEditMany = true;

                        }
                        else
                        {
                            _gvPager.HideEditMany = false;

                        }

                        if (_theView != null)
                        {
                            if ((bool)_theView.ShowBulkUpdateIcon == false)
                                _gvPager.HideEditMany = true;
                        }
                    }
                    ShowHidePermanentDelete();
                }

                if (_bHideAllExport)
                {
                    _gvPager.HideAllExport = true;
                    divEmail.Visible = false;
                }

            }

            if (PageType == "c")
            {
                if (ShowAddButton == false)
                {
                    divEmptyData.Visible = false;

                }
            }




            if (_gvPager != null && Request.QueryString["RecordTable"] != null)
            {
                divShowGraph.Visible = false;
                divUpload.Visible = false;
                divEmail.Visible = false;
                divConfig.Visible = false;
                divBatches.Visible = false;

            }

            //implement view final touch

            if (_theView != null)
            {
                if ((bool)_theView.ShowViewIcon == false)
                    gvTheGrid.Columns[3].Visible = false;//view
                if ((bool)_theView.ShowEditIcon == false)
                    gvTheGrid.Columns[2].Visible = false;//edit
                if ((bool)_theView.ShowDeleteIcon == false)
                {
                    // gvTheGrid.Columns[0].Visible = false;//delete check
                    if (_gvPager != null)
                        _gvPager.HideDelete = true;
                }

                if ((bool)_theView.ShowAddIcon == false)
                {
                    divEmptyData.Visible = false;
                    divNoFilter.Visible = false;
                    if (_gvPager != null)
                        _gvPager.HideAdd = true;
                }

                if ((bool)_theView.ShowBulkUpdateIcon == false)
                {
                    if (_gvPager != null)
                        _gvPager.HideEditMany = true;
                }


            }

            if (_theRoleTable != null)
            {
                if (_theRoleTable.AllowEditView != null && (bool)_theRoleTable.AllowEditView == false)
                {
                    if (_gvPager != null)
                        _gvPager.HideEditView = true;

                    hlEditView.Visible = false;
                    hlEditView2.Visible = false;

                }
            }
            else
            {
                if (_theRole != null && _theUserRole.IsAccountHolder != null && (bool)_theUserRole.IsAccountHolder == false)
                {
                    if (_theRole.AllowEditView != null && (bool)_theRole.AllowEditView == false)
                    {
                        if (_gvPager != null)
                            _gvPager.HideEditView = true;

                        hlEditView.Visible = false;
                        hlEditView2.Visible = false;

                    }

                }
            }


            if (Request.QueryString["ViewID"] != null || Request.QueryString["View"] != null)
            {
                if ((bool)_theUserRole.IsAccountHolder)
                {
                    if (_gvPager != null)
                        _gvPager.HideEditView = false;

                    hlEditView.Visible = true;
                    hlEditView2.Visible = true;
                }
                else
                {

                    if (_gvPager != null && _strViewPageType != "dash")
                        _gvPager.HideEditView = true;

                    hlEditView.Visible = false;
                    hlEditView2.Visible = false;
                }

            }

            if (Request.RawUrl.IndexOf("RecordTableSection.aspx") > -1)
            {
                if (_gvPager != null)
                    _gvPager.HideEditView = true;

                divEmptyData.Visible = false;
            }



            if (_strViewPageType == "dash" && Session["EditDashboard"] == null)
            {
                if (_gvPager != null)
                    _gvPager.HideEditView = true;

                hlEditView.Visible = false;
                hlEditView2.Visible = false;
            }



            if (_gvPager != null)
            {
                _gvPager.TableID = TableID;
            }
            if (divEmptyData.Visible == true & divNoFilter.Visible == true)
            {
                divEmptyData.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Records", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = "Error! Please check filter number/date format/view sort order.";

            if (_theView != null && Session["fixviewsortorder" + _theView.ViewID.ToString()] == null && ex.Message.IndexOf("Invalid column name") > -1
                && ex.Message.IndexOf("ORDER BY") > -1 && _theView.SortOrder != "" && _theView.FilterControlsInfo != "")
            {
                Session["fixviewsortorder" + _theView.ViewID.ToString()] = "done";
                //System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                //xmlDoc.Load(new StringReader(_theView.FilterControlsInfo));
                //Pages_UserControl_ViewDetail vdFilter = new Pages_UserControl_ViewDetail();
                //vdFilter = (Pages_UserControl_ViewDetail)LoadControl( "~/Pages/UserControl/ViewDetail.ascx");
                //vdFilter.PopulateFilterControl(xmlDoc.OuterXml, ((int)_theView.TableID));
                //_theView.SortOrder=vdFilter.GetViewOrderString();
                _theView.SortOrder = "";
                ViewManager.dbg_View_Update(_theView);


                if (_iSearchCriteriaID > 0)
                {
                    Common.ExecuteText("DELETE SearchCriteria WHERE SearchCriteriaID=" + _iSearchCriteriaID.ToString());

                }
                Session["SCid" + hfViewID.Value] = null;
                ViewState["_iSearchCriteriaID"] = null;

                lblMsg.Text = "Please try again. If the error still persist please update this view sort order and try again.";
            }

            divEmptyData.Visible = true;
        }


        if (_theTable != null)
        {
            string strHideEmailButton = SystemData.SystemOption_ValueByKey_Account("Hide Record Email Button", _theTable.AccountID, _theTable.TableID);

            if (strHideEmailButton != "" && strHideEmailButton.ToLower() == "yes")
            {
                divEmail.Visible = false;
            }
        }

    }


    protected bool IsFiltered()
    {
        if (txtDateFrom.Text != "" || txtDateTo.Text != "" || ddlEnteredBy.SelectedIndex != 0
            || chkIsActive.Checked != false || chkShowOnlyWarning.Checked != false
            || cbcSearchMain.ddlYAxisV != ""
            //|| ddlYAxis.SelectedIndex != 0 || txtSearchText.Text != "" || txtLowerLimit.Text != ""
            //|| txtUpperLimit.Text != "" || ddlDropdownColumnSearch.SelectedValue!="" 
            )
        {


            return true;
        }

        if (chkShowAdvancedOptions.Checked == false)//_bDynamicSearch
        {
            foreach (DataRow dr in _dtDynamicSearchColumns.Rows)
            {

                if (dr["ColumnType"].ToString() == "number" || dr["ColumnType"].ToString() == "calculation")
                {
                    TextBox txtLowerLimit = (TextBox)tblSearchControls.FindControl("txtLowerLimit_" + dr["SystemName"].ToString());
                    TextBox txtUpperLimit = (TextBox)tblSearchControls.FindControl("txtUpperLimit_" + dr["SystemName"].ToString());
                    if (txtLowerLimit != null && txtUpperLimit != null)
                    {
                        if (txtLowerLimit.Text != "" || txtUpperLimit.Text != "")
                        {
                            return true;
                        }
                    }

                }
                else if (dr["ColumnType"].ToString() == "text")
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    if (txtSearch != null)
                    {
                        if (txtSearch.Text != "")
                        {
                            return true;
                        }
                    }

                }
                else if (dr["ColumnType"].ToString() == "date" || dr["ColumnType"].ToString() == "datetime")
                {
                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());
                    if (txtLowerDate != null && txtUpperDate != null)
                    {
                        if (txtLowerDate.Text != "" || txtUpperDate.Text != "")
                        {
                            return true;
                        }
                    }

                }
                else if (dr["ColumnType"].ToString() == "time")
                {
                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());
                    if (txtLowerTime != null && txtUpperTime != null)
                    {
                        if (txtLowerTime.Text != "" || txtUpperTime.Text != "")
                        {
                            return true;
                        }
                    }

                }
                else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                {
                    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                    if (ddlSearch != null)
                    {
                        if (ddlSearch.SelectedValue != "")
                        {
                            return true;
                        }
                    }
                }


                else if (dr["ColumnType"].ToString() == "radiobutton" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                {
                    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                    if (ddlSearch != null)
                    {
                        if (ddlSearch.SelectedValue != "")
                        {
                            return true;
                        }
                    }
                }

                else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "table" || dr["DropDownType"].ToString() == "tabledd") &&
           dr["TableTableID"] != DBNull.Value && dr["DisplayColumn"].ToString() != "")
                {
                    DropDownList ddlParentSearch = (DropDownList)tblSearchControls.FindControl("ddlParentSearch_" + dr["SystemName"].ToString());

                    if (ddlParentSearch != null)
                    {
                        if (ddlParentSearch.Text != "")
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    if (txtSearch != null)
                    {
                        if (txtSearch.Text != "")
                        {
                            return true;
                        }
                    }
                }
            }

            //
            foreach (DataRow dr in _dtSearchGroup.Rows)
            {

                TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SearchGroupID"].ToString());
                if (txtSearch != null)
                {
                    if (txtSearch.Text != "")
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    protected void ShowHidePermanentDelete()
    {
        if (chkIsActive.Checked == true)
        {
            if (_gvPager != null && _theUserRole != null && _theUserRole.IsAccountHolder != null && (bool)_theUserRole.IsAccountHolder)
            {
                _gvPager.HideParmanentDelete = false;
            }
            else
            {
                _gvPager.HideParmanentDelete = true;
                if (_theRole != null && _theRole.RoleType == "2")
                {
                    if (_theUserRole != null && _theUserRole.AllowDeleteRecord != null && (bool)_theUserRole.AllowDeleteRecord)
                    {
                        _gvPager.HideParmanentDelete = false;
                    }
                }
            }
        }
    }

    protected string GetDynamicSeachXMLPart()
    {
        string strDynamicSearch = "";
        try
        {
            foreach (DataRow dr in _dtDynamicSearchColumns.Rows)
            {

                if (dr["ColumnType"].ToString() == "number" || dr["ColumnType"].ToString() == "calculation")
                {
                    TextBox txtLowerLimit = (TextBox)tblSearchControls.FindControl("txtLowerLimit_" + dr["SystemName"].ToString());
                    TextBox txtUpperLimit = (TextBox)tblSearchControls.FindControl("txtUpperLimit_" + dr["SystemName"].ToString());
                    if (txtLowerLimit != null && txtUpperLimit != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + txtLowerLimit.ID + ">" + HttpUtility.HtmlEncode(txtLowerLimit.Text) + "</" + txtLowerLimit.ID + ">";
                        strDynamicSearch = strDynamicSearch + " <" + txtUpperLimit.ID + ">" + HttpUtility.HtmlEncode(txtUpperLimit.Text) + "</" + txtUpperLimit.ID + ">";
                    }

                }


                else if (dr["ColumnType"].ToString() == "text")
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    if (txtSearch != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + txtSearch.ID + ">" + HttpUtility.HtmlEncode(txtSearch.Text) + "</" + txtSearch.ID + ">";
                    }

                }

                else if (dr["ColumnType"].ToString() == "date")
                {

                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());

                    if (txtLowerDate != null && txtUpperDate != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + txtLowerDate.ID + ">" + HttpUtility.HtmlEncode(txtLowerDate.Text) + "</" + txtLowerDate.ID + ">";
                        strDynamicSearch = strDynamicSearch + " <" + txtUpperDate.ID + ">" + HttpUtility.HtmlEncode(txtUpperDate.Text) + "</" + txtUpperDate.ID + ">";
                    }

                }
                else if (dr["ColumnType"].ToString() == "datetime")
                {

                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());

                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                    if (txtLowerDate != null && txtUpperDate != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + txtLowerDate.ID + ">" + HttpUtility.HtmlEncode(txtLowerDate.Text) + "</" + txtLowerDate.ID + ">";
                        strDynamicSearch = strDynamicSearch + " <" + txtUpperDate.ID + ">" + HttpUtility.HtmlEncode(txtUpperDate.Text) + "</" + txtUpperDate.ID + ">";
                    }

                    if (txtLowerTime != null && txtUpperTime != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + txtLowerTime.ID + ">" + HttpUtility.HtmlEncode(txtLowerTime.Text) + "</" + txtLowerTime.ID + ">";
                        strDynamicSearch = strDynamicSearch + " <" + txtUpperTime.ID + ">" + HttpUtility.HtmlEncode(txtUpperTime.Text) + "</" + txtUpperTime.ID + ">";
                    }

                }
                else if (dr["ColumnType"].ToString() == "time")
                {

                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                    if (txtLowerTime != null && txtUpperTime != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + txtLowerTime.ID + ">" + HttpUtility.HtmlEncode(txtLowerTime.Text) + "</" + txtLowerTime.ID + ">";
                        strDynamicSearch = strDynamicSearch + " <" + txtUpperTime.ID + ">" + HttpUtility.HtmlEncode(txtUpperTime.Text) + "</" + txtUpperTime.ID + ">";
                    }

                }

                //if (dr["ColumnType"].ToString() == "datetime")
                //{
                //    TextBox txtDate = (TextBox)tblSearchControls.FindControl("txtDate_" + dr["SystemName"].ToString());
                //    if (txtDate != null)
                //    {
                //        strDynamicSearch = strDynamicSearch + " <" + txtDate.ID + ">" + HttpUtility.HtmlEncode(txtDate.Text) + "</" + txtDate.ID + ">";
                //    }

                //    TextBox txtTime = (TextBox)tblSearchControls.FindControl("txtTime_" + dr["SystemName"].ToString());
                //    if (txtTime != null)
                //    {
                //        strDynamicSearch = strDynamicSearch + " <" + txtTime.ID + ">" + HttpUtility.HtmlEncode(txtTime.Text) + "</" + txtTime.ID + ">";
                //    }

                //}


                //else if (dr["ColumnType"].ToString() == "time")
                //{

                //    TextBox txtTime = (TextBox)tblSearchControls.FindControl("txtTime_" + dr["SystemName"].ToString());
                //    if (txtTime != null)
                //    {
                //        strDynamicSearch = strDynamicSearch + " <" + txtTime.ID + ">" + HttpUtility.HtmlEncode(txtTime.Text) + "</" + txtTime.ID + ">";
                //    }

                //}

                else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                {
                    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                    if (ddlSearch != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + ddlSearch.ID + ">" + HttpUtility.HtmlEncode(ddlSearch.Text) + "</" + ddlSearch.ID + ">";
                    }

                }
                //else if (dr["ColumnType"].ToString() == "radiobutton" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                else if (dr["ColumnType"].ToString() == "radiobutton" || dr["ColumnType"].ToString() == "listbox"
                               || dr["ColumnType"].ToString() == "checkbox")
                {
                    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                    if (ddlSearch != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + ddlSearch.ID + ">" + HttpUtility.HtmlEncode(ddlSearch.Text) + "</" + ddlSearch.ID + ">";
                    }

                }

                else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "table" || dr["DropDownType"].ToString() == "tabledd") &&
           dr["TableTableID"] != DBNull.Value && dr["DisplayColumn"].ToString() != "")
                {
                    DropDownList ddlParentSearch = (DropDownList)tblSearchControls.FindControl("ddlParentSearch_" + dr["SystemName"].ToString());

                    if (ddlParentSearch != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + ddlParentSearch.ID + ">" + HttpUtility.HtmlEncode(ddlParentSearch.Text) + "</" + ddlParentSearch.ID + ">";
                    }

                }
                else
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    if (txtSearch != null)
                    {
                        strDynamicSearch = strDynamicSearch + " <" + txtSearch.ID + ">" + HttpUtility.HtmlEncode(txtSearch.Text) + "</" + txtSearch.ID + ">";
                    }

                }
            }

            //

            foreach (DataRow dr in _dtSearchGroup.Rows)
            {

                TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SearchGroupID"].ToString());
                if (txtSearch != null)
                {
                    strDynamicSearch = strDynamicSearch + " <" + txtSearch.ID + ">" + HttpUtility.HtmlEncode(txtSearch.Text) + "</" + txtSearch.ID + ">";
                }
            }
        }
        catch
        {
            //
        }
        return strDynamicSearch;


    }

    protected void UpdateSearchCriteriaForTheGrid(string strOtherXMLTags)
    {
        try
        {
            bool? bIsNumericY = null;
            string xml = null;
            xml = @"<root>" +
                   " <" + txtDateFrom.ID + ">" + HttpUtility.HtmlEncode(txtDateFrom.Text) + "</" + txtDateFrom.ID + ">" +
                   " <" + txtDateTo.ID + ">" + HttpUtility.HtmlEncode(txtDateTo.Text) + "</" + txtDateTo.ID + ">" +

                    " <" + hfAndOr1.ID + ">" + HttpUtility.HtmlEncode(hfAndOr1.Value) + "</" + hfAndOr1.ID + ">" +
                     " <" + hfAndOr2.ID + ">" + HttpUtility.HtmlEncode(hfAndOr2.Value) + "</" + hfAndOr2.ID + ">" +
                      " <" + hfAndOr3.ID + ">" + HttpUtility.HtmlEncode(hfAndOr3.Value) + "</" + hfAndOr3.ID + ">" +

                     " <" + cbcSearchMain.ID + ">" + HttpUtility.HtmlEncode(GetSearchCriteriaIDForCBC(cbcSearchMain)) + "</" + cbcSearchMain.ID + ">" +
                       " <" + cbcSearch1.ID + ">" + HttpUtility.HtmlEncode(GetSearchCriteriaIDForCBC(cbcSearch1)) + "</" + cbcSearch1.ID + ">" +
                         " <" + cbcSearch2.ID + ">" + HttpUtility.HtmlEncode(GetSearchCriteriaIDForCBC(cbcSearch2)) + "</" + cbcSearch2.ID + ">" +
                           " <" + cbcSearch3.ID + ">" + HttpUtility.HtmlEncode(GetSearchCriteriaIDForCBC(cbcSearch3)) + "</" + cbcSearch3.ID + ">" +
                   " <bIsNumericY>" + HttpUtility.HtmlEncode(bIsNumericY.ToString()) + "</bIsNumericY>" +

                   " <" + ddlEnteredBy.ID + ">" + HttpUtility.HtmlEncode(ddlEnteredBy.Text) + "</" + ddlEnteredBy.ID + ">" +
                //" <" + cbcvSumFilter.ID + ">" + HttpUtility.HtmlEncode(cbcvSumFilter.GetValue) + "</" + cbcvSumFilter.ID + ">" +
                   " <" + chkIsActive.ID + ">" + HttpUtility.HtmlEncode(chkIsActive.Checked.ToString()) + "</" + chkIsActive.ID + ">" +
                   " <" + chkShowOnlyWarning.ID + ">" + HttpUtility.HtmlEncode(chkShowOnlyWarning.Checked.ToString()) + "</" + chkShowOnlyWarning.ID + ">" +
                   " <" + chkShowAdvancedOptions.ID + ">" + HttpUtility.HtmlEncode(chkShowAdvancedOptions.Checked.ToString()) + "</" + chkShowAdvancedOptions.ID + ">" +
                   " <" + hfTextSearch.ID + ">" + HttpUtility.HtmlEncode(hfTextSearch.Value) + "</" + hfTextSearch.ID + ">" +
                   " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
                   " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +


                   //" <_strNumericSearch>" + HttpUtility.HtmlEncode(_strNumericSearch) + "</_strNumericSearch>" +
                   " <TextSearch>" + HttpUtility.HtmlEncode(TextSearch) + "</TextSearch>" +
                   " <TextSearchParent>" + HttpUtility.HtmlEncode(TextSearchParent) + "</TextSearchParent>" +

                   " <_strViewName>" + HttpUtility.HtmlEncode(_strViewName) + "</_strViewName>" +
                   " <strViewID>" + HttpUtility.HtmlEncode(hfViewID.Value) + "</strViewID>" +
                    " <" + ddlUploadedBatch.ID + ">" + HttpUtility.HtmlEncode(ddlUploadedBatch.Text) + "</" + ddlUploadedBatch.ID + ">" +
                   strOtherXMLTags +
                //" <" + ddlDropdownColumnSearch.ID + ">" + HttpUtility.HtmlEncode(ddlDropdownColumnSearch.SelectedValue == null ? "" : ddlDropdownColumnSearch.SelectedValue) + "</" + ddlDropdownColumnSearch.ID + ">" + strDynamicSearch +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
            ViewState["_iSearchCriteriaID"] = _iSearchCriteriaID;
            Session["SCid" + hfViewID.Value] = _iSearchCriteriaID;

            hlShowGraph.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/RecordChart.aspx?SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString());
            //hlSchedule.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/MonitorSchedules.aspx?SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString());

            hlUpload.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordUpload.aspx?SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString());

            //hlDocuments.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SSearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString());

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }

    protected void BindTheGridForExport(int iStartIndex, int iMaxRows)
    {
        lblMsg.Text = "";
        _bIsForExport = true;
        try
        {
            int iTN = 0;

            string strOrderDirection = "DESC";
            string sOrder = "DBGSystemRecordID";

            _dtRecordColums = RecordManager.ets_Table_Columns_Summary_Export(TableID, int.Parse(hfViewID.Value));

            bool bSortColumnFound = false;
            string strNameOnExport1 = "";
            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {

                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() != "" && strNameOnExport1 == "")
                {
                    strNameOnExport1 = _dtRecordColums.Rows[i]["NameOnExport"].ToString();
                }

                if (gvTheGrid.GridViewSortColumn.IndexOf(_dtRecordColums.Rows[i]["Heading"].ToString(), 0) >= 0
                    && _dtRecordColums.Rows[i]["NameOnExport"].ToString() != "")
                {
                    SortDirection sdTEmp = gvTheGrid.GridViewSortDirection;
                    gvTheGrid.GridViewSortColumn = gvTheGrid.GridViewSortColumn.Replace(_dtRecordColums.Rows[i]["Heading"].ToString(), _dtRecordColums.Rows[i]["NameOnExport"].ToString());
                    gvTheGrid.GridViewSortDirection = sdTEmp;
                    bSortColumnFound = true;
                }
            }

            if (bSortColumnFound == false && strNameOnExport1 != "")
            {
                gvTheGrid.GridViewSortColumn = strNameOnExport1;
            }

            if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
            {
                strOrderDirection = "ASC";
            }
            if (gvTheGrid.GridViewSortColumn != "")
            {
                sOrder = gvTheGrid.GridViewSortColumn;
            }

            TextSearch = TextSearch + hfTextSearch.Value;

            if ((bool)_theUserRole.IsAdvancedSecurity)
            {
                if (_strRecordRightID == Common.UserRoleType.OwnData)
                {
                    TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
                }
            }
            else
            {
                if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
                {
                    TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
                }
            }

            PopulateDateAddedSearch();

            if (chkShowAdvancedOptions.Checked && ddlUploadedBatch.SelectedValue != "")
            {
                TextSearch = TextSearch + "  AND Record.BatchID=" + ddlUploadedBatch.SelectedValue + "";
            }
            string strReturnSQL = "";
            _dtDataSource = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                !chkIsActive.Checked,
                chkShowOnlyWarning.Checked == false ? null : (bool?)true,
                null, null,
                sOrder, strOrderDirection, iStartIndex, iStartIndex, ref iTN, ref _iTotalDynamicColumns, "export", _strNumericSearch, TextSearch + TextSearchParent,
               _dtDateFrom, _dtDateTo, "", "", "", null, ref strReturnSQL, ref strReturnSQL);



            //now lets play with Record list and columns list
            _dtRecordColums = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));

            DataRow drFooter = _dtDataSource.NewRow();

            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {
                for (int j = 0; j < _dtDataSource.Columns.Count; j++)
                {

                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == _dtDataSource.Columns[j].ColumnName)
                    {
                        if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
                        {
                            drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(_dtDataSource, _dtDataSource.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));
                        }
                    }
                }
            }

            _dtDataSource.Rows.Add(drFooter);

            gvTheGrid.ShowFooter = false;
            gvTheGrid.DataSource = _dtDataSource;
            gvTheGrid.VirtualItemCount = iTN;//+ 1
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.AddURL = GetAddURL();

                if (_gvPager != null)
                {
                    int iViewID = -1;
                    if (hfViewID.Value != "")
                        iViewID = int.Parse(hfViewID.Value);


                    _gvPager.EditViewCSSClass = "popuplink2";
                    _gvPager.EditViewToolTip = "Edit View";
                    _gvPager.EditViewURL = GetEditViewPageURL();

                    if (_strNoAjaxView != "")
                    {
                        _gvPager.EditViewTarget = "_parent";
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Records", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }

    protected void ddl_search(Object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
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
                    //if (bIgnoreSymbols)
                    //{
                    amount += double.Parse(Common.IgnoreSymbols(row[strColumn].ToString()), System.Globalization.NumberStyles.Any);
                    //}
                    //else
                    //{
                    //    amount += double.Parse(row[strColumn].ToString(), System.Globalization.NumberStyles.Any);
                    //}
                }
                catch (Exception ex)
                {
                    //
                }
            }
        }

        return amount;
    }

    public string GetAddURL()
    {
        if (PageType == "p")
        {
            string strExtra = "";

            //if (Request.QueryString["viewname"] != null)
            //{
            //    strExtra = "&viewname=" + Request.QueryString["viewname"].ToString();
            //}
            if (Request.QueryString["View"] != null && Request.RawUrl.IndexOf("RecordList.aspx") > -1)
            {
                strExtra = "&View=" + Request.QueryString["View"].ToString();
            }


            if (_bOpenInParent)
            {
                strExtra = strExtra + "&fixedurl=" + Cryptography.Encrypt("~/Default.aspx");
            }

            string strAddURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + strExtra;

            if (_theTable.AddOpensForm != null && (bool)_theTable.AddOpensForm && _theTable.AddRecordSP != "")
            {
                strAddURL = "javascript:AddClick();";
            }
            return strAddURL;
        }
        else
        {
            if (Request.QueryString["Recordid"] == null)
            {
                return "";
            }
            else
            {
                return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&tabindex=" + DetailTabIndex.ToString() + "&onlyback=yes&parentRecordid=" + Request.QueryString["Recordid"].ToString() + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
            }
        }

    }

    protected void lnkAddRecordCancel2_Click(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    protected void lnkExportRecordsCancel2_Click(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    protected void lnkAddRecordOK_Click(object sender, EventArgs e)
    {
        lblMsgAddRecord.Text = "";
        if (ddlFormSet.SelectedItem == null)
        {
            lblMsgAddRecord.Text = "Please select a Form.";
            mpeAddRecord.Show();
            return;
        }



        if (SecurityManager.IsRecordsExceeded((int)_theUserRole.AccountID))
        {
            Session["DoNotAllow"] = "true";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DoNotAllow", "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);
            return;
        }


        //add a record and goto the edit record page

        Record theRecord = new Record();
        theRecord.TableID = _theTable.TableID;
        theRecord.IsActive = true;
        theRecord.EnteredBy = _ObjUser.UserID;
        //DataTable _dtColumnsAll = RecordManager.ets_Table_Columns_All((int)_theTable.TableID, null, null);
        for (int i = 0; i < _dtColumnsAll.Rows.Count; i++)
        {
            if (_dtColumnsAll.Rows[i]["ColumnType"].ToString() == "number")
            {
                if (_dtColumnsAll.Rows[i]["NumberType"] != null)
                {
                    if (_dtColumnsAll.Rows[i]["NumberType"].ToString() == "8")
                    {
                        string strValue = "1";
                        try
                        {
                            string strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + _dtColumnsAll.Rows[i]["SystemName"].ToString() + ")) FROM  Record  WHERE  IsNumeric(" + _dtColumnsAll.Rows[i]["SystemName"].ToString() + ")=1 AND  TableID=" + _qsTableID);
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
                        RecordManager.MakeTheRecord(ref theRecord, _dtColumnsAll.Rows[i]["SystemName"].ToString(), strValue);
                    }

                }
            }

        }
        for (int i = 0; i < _dtColumnsAll.Rows.Count; i++)
        {
            if (_dtColumnsAll.Rows[i]["DefaultValue"].ToString() != "")
            {
                if (_dtColumnsAll.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "datetime"
                    || _dtColumnsAll.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "date"
                    || _dtColumnsAll.Rows[i]["ColumnType"].ToString().Trim().ToLower() == "time")
                {


                    RecordManager.MakeTheRecord(ref theRecord, _dtColumnsAll.Rows[i]["SystemName"].ToString(), DateTime.Now);
                }
                else
                {



                    RecordManager.MakeTheRecord(ref theRecord, _dtColumnsAll.Rows[i]["SystemName"].ToString(),
                        _dtColumnsAll.Rows[i]["DefaultValue"].ToString());
                }

            }

        }




        int iNewRecordID = RecordManager.ets_Record_Insert(theRecord);


        //call the SP

        RecordManager.AddRecordSP(_theTable.AddRecordSP, iNewRecordID, int.Parse(ddlFormSet.SelectedValue));

        string strSearch = Cryptography.Encrypt("-1");
        if (Request.QueryString["SearchCriteriaID"] != null)
        {
            strSearch = Request.QueryString["SearchCriteriaID"].ToString();
        }
        string strURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetWizard.aspx?SearchCriteriaID=" + strSearch
    + "&FormSetID=" + Cryptography.Encrypt(ddlFormSet.SelectedValue)
    + "&ParentTableID=" + Cryptography.Encrypt(_theTable.TableID.ToString())
    + "&ParentRecordID=" + Cryptography.Encrypt(iNewRecordID.ToString()) + "&ps=0";
        Response.Redirect(strURL);

    }

    protected HorizontalAlign GetHorizontalAlign(string strAlignment)
    {
        switch (strAlignment.ToLower())
        {
            case "left":
                return HorizontalAlign.Left;
            case "right":
                return HorizontalAlign.Right;
            case "center":
                return HorizontalAlign.Center;

        }

        return HorizontalAlign.Center;

    }

    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {
        gvTheGrid.PageIndex = 0;// why???
        _dtRecordColums = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));

        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
        {
            if (_dtRecordColums.Rows[i]["Heading"].ToString() == gvTheGrid.GridViewSortColumn)
            {

                ViewState["SortOrderColumnID"] = _dtRecordColums.Rows[i]["ColumnID"].ToString();
                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "number" || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "calculation")
                {
                    //if (!bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString()))
                    //{

                    gvTheGrid.GridViewSortColumn = "CONVERT(decimal(20,10), dbo.RemoveSpecialChars([" + _dtRecordColums.Rows[i]["Heading"].ToString() + "])) ";

                    if (ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()] == null)
                    {
                        ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()] = "ASC";
                        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    }
                    else
                    {
                        if (ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()].ToString() == "ASC")
                        {
                            ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()] = "DESC";
                            gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                        }
                        else
                        {
                            gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                            ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()] = "ASC";
                        }

                    }

                    //}
                    // return;
                }


                //datetime, date & time

                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "date"
                    || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "datetime"
                    || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "time")
                {

                    //DateTime dateValue;
                    //if (DateTime.TryParseExact(_dtRecordColums.Rows[i]["Heading"].ToString(), Common.Dateformats,
                    //             new CultureInfo("en-GB"),
                    //             DateTimeStyles.None,
                    //             out dateValue))
                    //{
                    //    gvTheGrid.GridViewSortColumn = "CONVERT(Datetime, [" + dateValue.ToShortDateString() + "],103) ";

                    //}


                    gvTheGrid.GridViewSortColumn = "CONVERT(Datetime,[dbo].[fnRemoveNonDate]( [" + _dtRecordColums.Rows[i]["Heading"].ToString() + "]),103) ";

                    if (ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()] == null)
                    {
                        ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()] = "ASC";
                        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    }
                    else
                    {
                        if (ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()].ToString() == "ASC")
                        {
                            ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()] = "DESC";
                            gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                        }
                        else
                        {
                            gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                            ViewState[_dtRecordColums.Rows[i]["Heading"].ToString()] = "ASC";
                        }

                    }


                    // return;
                }



            }
        }

        BindTheGrid(0, gvTheGrid.PageSize);

    }


    //protected void bntShowGraph_Click(object sender, ImageClickEventArgs e)
    //{
    //    SearchCriteria newSearchCriteria= new SearchCriteria(null,
    //        txtDateFrom.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateFrom.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
    //            txtDateTo.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text,"d/M/yyyy",CultureInfo.InvariantCulture),
    //            GetLocationIDs(),
    //            null,null,int.Parse(Session["AccountID"].ToString()));

    //    int iSearchCriteriaID = RecordManager.ets_SearchCriteria_Insert(newSearchCriteria);
    //    Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/RecordChart.aspx?SearchCriteriaID="+iSearchCriteriaID.ToString()+"&TableID=" + TableID.ToString() );

    //}



    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)


    protected void lnkReset_Click(object sender, EventArgs e)
    {
        Pager_OnApplyFilter(sender, e);
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        //gvTheGrid.GridViewSortColumn = "";

        gvTheGrid.PageIndex = 0;// why???
        BindTheGrid(0, gvTheGrid.PageSize);

    }

    protected int GetSearchCriteriaIDForCBC(Pages_UserControl_ControlByColumn cbcX)
    {
        if (cbcX.ddlYAxisV != "")
        {
            string xml = null;
            xml = @"<root>" +
                   " <ddlYAxisV>" + HttpUtility.HtmlEncode(cbcX.ddlYAxisV) + "</ddlYAxisV>" +
                   " <txtUpperLimitV>" + HttpUtility.HtmlEncode(cbcX.txtUpperLimitV) + "</txtUpperLimitV>" +
                   " <txtLowerLimitV>" + HttpUtility.HtmlEncode(cbcX.txtLowerLimitV) + "</txtLowerLimitV>" +
                   " <hfTextSearchV>" + HttpUtility.HtmlEncode(cbcX.hfTextSearchV) + "</hfTextSearchV>" +
                   " <txtLowerDateV>" + HttpUtility.HtmlEncode(cbcX.txtLowerDateV) + "</txtLowerDateV>" +
                   " <txtUpperDateV>" + HttpUtility.HtmlEncode(cbcX.txtUpperDateV) + "</txtUpperDateV>" +
                   " <ddlDropdownColumnSearchV>" + HttpUtility.HtmlEncode(cbcX.ddlDropdownColumnSearchV) + "</ddlDropdownColumnSearchV>" +
                   " <txtSearchTextV>" + HttpUtility.HtmlEncode(cbcX.txtSearchTextV) + "</txtSearchTextV>" +
                    " <CompareOperator>" + HttpUtility.HtmlEncode(cbcX.CompareOperator) + "</CompareOperator>" +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            return SystemData.SearchCriteria_Insert(theSearchCriteria);
        }

        return -1;
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



    protected void gvTheGrid_DataBound(object sender, EventArgs e)
    {

        if (Session["RunSpeedLog"] != null && _theTable != null)
        {

            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Databound Event - START ";
            theSpeedLog.FunctionLineNumber = 3058;
            SecurityManager.AddSpeedLog(theSpeedLog);

        }




        if (_bIsForExport)
        {


            //if (gvTheGrid.Rows.Count > 0)
            //{

            //    for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            //    {
            //        for (int j = 0; j < _dtDataSource.Columns.Count; j++)
            //        {
            //            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == _dtDataSource.Columns[j].ColumnName)
            //            {
            //                if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
            //                {

            //                    double amount = 0;

            //                    foreach (GridViewRow row in gvTheGrid.Rows)
            //                    {
            //                        if (row.Cells[i + 3].Text != "")
            //                        {
            //                            try
            //                            {
            //                                amount += double.Parse(row.Cells[i + 3].Text, System.Globalization.NumberStyles.Any);
            //                            }
            //                            catch (Exception ex)
            //                            {
            //                                //
            //                            }
            //                        }
            //                    }
            //                    //update footer
            //                    gvTheGrid.FooterRow.Cells[i + 3].Text = string.Format("{0:N2}", amount);

            //                }
            //            }

            //        }

            //    }

            //}


        }
        else
        {
            if (gvTheGrid.Rows.Count > 0)
            {
                int m = 0;
                if (chkShowOnlyWarning.Checked)
                {
                    m = 4;//5;
                }
                else
                {
                    m = 3;// 4;
                }
                if (chkIsActive.Checked)
                {
                    m = m + 2;
                }

                bool iFirstTotal = false;

                //for (int j = 0; j < _dtDataSource.Rows.Count; j++)
                //{
                //    if (_dtDataSource.Rows[j]["SystemName"].ToString() == "RecordID")
                //   {
                //       m = m + 1;
                //       break;
                //   }
                //}

                for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                {
                    for (int j = 0; j < _dtDataSource.Columns.Count; j++)
                    {
                        if (_dtRecordColums.Rows[i]["Heading"].ToString() == _dtDataSource.Columns[j].ColumnName)
                        {
                            if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "RecordID")
                            {
                                m = m + 1;
                                break;
                            }
                        }
                    }
                }

                for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                {
                    for (int j = 0; j < _dtDataSource.Columns.Count; j++)
                    {
                        if (_dtRecordColums.Rows[i]["Heading"].ToString() == _dtDataSource.Columns[j].ColumnName)
                        {



                            if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true" &&
                                (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "number"
                                || _dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "calculation"))
                            {

                                double amount = 0;

                                foreach (GridViewRow row in gvTheGrid.Rows)
                                {
                                    if (row.Cells[i + m].Text != "" && row.Cells[i + m].Text != "&nbsp;")
                                    {
                                        try
                                        {

                                            amount += double.Parse(Common.IgnoreSymbols(row.Cells[i + m].Text), System.Globalization.NumberStyles.Any);

                                        }
                                        catch (Exception ex)
                                        {
                                            //
                                        }
                                    }
                                }
                                //update footer
                                gvTheGrid.FooterRow.Cells[i + m].Text = string.Format("{0:N2}", amount);

                                double dGamount = 0;
                                bool bGotGrandTotal = false;

                                if (iFirstTotal == false)
                                {
                                    gvTheGrid.FooterRow.Cells[i + m - 1].Text = "Page Total";// +"</br>" + "Grand Total";
                                    iFirstTotal = true;
                                }



                                if (_dtRecordColums.Rows[i]["Alignment"] == DBNull.Value)
                                {
                                    if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() != "number")
                                    {
                                        gvTheGrid.FooterRow.Cells[i + m].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    else
                                    {
                                        gvTheGrid.FooterRow.Cells[i + m].HorizontalAlign = HorizontalAlign.Right;
                                    }

                                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "RecordID")
                                    {
                                        gvTheGrid.FooterRow.Cells[i + m].HorizontalAlign = HorizontalAlign.Right;
                                    }

                                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "IsActive"
                                   || _dtRecordColums.Rows[i]["SystemName"].ToString() == "DateTimeRecorded"
                                        || _dtRecordColums.Rows[i]["SystemName"].ToString() == "EnteredBy")
                                    {

                                        gvTheGrid.FooterRow.Cells[i + m].HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "Notes")
                                    {
                                        gvTheGrid.FooterRow.Cells[i + m].HorizontalAlign = HorizontalAlign.Left;
                                    }

                                }
                                else
                                {
                                    gvTheGrid.FooterRow.Cells[i + m].HorizontalAlign = GetHorizontalAlign(_dtRecordColums.Rows[i]["Alignment"].ToString());
                                }


                                //if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "number"
                                //   && _dtRecordColums.Rows[i]["NumberType"] != DBNull.Value)
                                //{
                                //    if (_dtRecordColums.Rows[i]["NumberType"].ToString() == "6")
                                //    {                                      

                                //            if (_dtRecordColums.Rows[i]["TextType"].ToString() != ""
                                //                && gvTheGrid.FooterRow.Cells[i + m].Text != "" && gvTheGrid.FooterRow.Cells[i + m].Text.ToString() != "&nbsp;")
                                //            {

                                //                //gvTheGrid.FooterRow.Cells[i + m].Text = _dtRecordColums.Rows[i]["TextType"].ToString()
                                //                //    + gvTheGrid.FooterRow.Cells[i + m].Text;

                                //                try
                                //                {
                                //                    string strMoney=gvTheGrid.FooterRow.Cells[i + m].Text;
                                //                    strMoney = Common.IgnoreSymbols(strMoney);
                                //                    gvTheGrid.FooterRow.Cells[i + m].Text = _dtRecordColums.Rows[i]["TextType"].ToString()
                                //                   + double.Parse(strMoney).ToString("C").Substring(1);



                                //                    //double dGamount = 0;
                                //                    //get the grand total

                                //                    //if (chkShowAdvancedOptions.Checked && ddlUploadedBatch.SelectedValue != "")
                                //                    //{
                                //                    //    TextSearch = TextSearch + "  AND TempRecordID IN  (SELECT RecordID FROM TempRecord WHERE BatchID=" + ddlUploadedBatch.SelectedValue + ")";
                                //                    //}


                                //                    int iTN = 1;
                                //                    DataTable dtGrand = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                                //                       ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                                //                       !chkIsActive.Checked,
                                //                       chkShowOnlyWarning.Checked == false ? null : (bool?)true, null, null,
                                //                       "", "", null, null, ref iTN, ref _iTotalDynamicColumns, "view", _strNumericSearch, TextSearch + TextSearchParent,
                                //                      _dtDateFrom, _dtDateTo, "", "", _strViewName, _theView.ViewID);

                                //                    for (int x = 0; x < _dtRecordColums.Rows.Count; x++)
                                //                    {
                                //                        for (int y = 0; y < dtGrand.Columns.Count; y++)
                                //                        {
                                //                            if (_dtRecordColums.Rows[x]["Heading"].ToString() == dtGrand.Columns[y].ColumnName
                                //                                && _dtRecordColums.Rows[i]["Heading"].ToString() == _dtRecordColums.Rows[x]["Heading"].ToString())
                                //                            {
                                //                                if      (_dtRecordColums.Rows[x]["ColumnType"].ToString().ToLower() == "number"
                                //                                    && _dtRecordColums.Rows[x]["NumberType"] != DBNull.Value)
                                //                                {


                                //                                    foreach (DataRow row in dtGrand.Rows)
                                //                                    {
                                //                                        try
                                //                                        {

                                //                                            dGamount += double.Parse(Common.IgnoreSymbols(row[dtGrand.Columns[y].ColumnName].ToString()),
                                //                                                System.Globalization.NumberStyles.Any);

                                //                                        }
                                //                                        catch (Exception ex)
                                //                                        {
                                //                                            //
                                //                                        }
                                //                                    }

                                //                                }

                                //                            }
                                //                        }

                                //                    }



                                //                        gvTheGrid.FooterRow.Cells[i + m].Text = gvTheGrid.FooterRow.Cells[i + m].Text +
                                //                            "</br>" + _dtRecordColums.Rows[i]["TextType"].ToString()
                                //                   + dGamount.ToString("C").Substring(1);

                                //                        bGotGrandTotal = true;


                                //                }
                                //                catch
                                //                {
                                //                    //

                                //                }
                                //            }

                                //    }
                                //}


                                //if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "calculation"
                                //   && _dtRecordColums.Rows[i]["TextType"].ToString() == "f"
                                //    && _dtRecordColums.Rows[i]["RegEx"].ToString() != "")
                                //{

                                //        if (gvTheGrid.FooterRow.Cells[i + m].Text != "" && gvTheGrid.FooterRow.Cells[i + m].Text.ToString() != "&nbsp;")
                                //        {
                                //            //gvTheGrid.FooterRow.Cells[i + m].Text = _dtRecordColums.Rows[i]["TextType"].ToString()
                                //            //    + gvTheGrid.FooterRow.Cells[i + m].Text;

                                //            try
                                //            {
                                //                string strMoney = gvTheGrid.FooterRow.Cells[i + m].Text;
                                //                strMoney = Common.IgnoreSymbols(strMoney);
                                //                gvTheGrid.FooterRow.Cells[i + m].Text = _dtRecordColums.Rows[i]["RegEx"].ToString()
                                //               + double.Parse(strMoney).ToString("C").Substring(1);

                                //                //double dGamount = 0;
                                //                //get the grand total

                                //                //if (chkShowAdvancedOptions.Checked && ddlUploadedBatch.SelectedValue != "")
                                //                //{
                                //                //    TextSearch = TextSearch + "  AND TempRecordID IN  (SELECT RecordID FROM TempRecord WHERE BatchID=" + ddlUploadedBatch.SelectedValue + ")";
                                //                //}

                                //                int iTN = 1;
                                //               DataTable dtGrand = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                                //                  ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                                //                  !chkIsActive.Checked,
                                //                  chkShowOnlyWarning.Checked == false ? null : (bool?)true, null, null,
                                //                  "", "", null, null, ref iTN, ref _iTotalDynamicColumns, "view", _strNumericSearch, TextSearch + TextSearchParent,
                                //                 _dtDateFrom, _dtDateTo, "", "", _strViewName, _theView.ViewID);

                                //               for (int x = 0; x < _dtRecordColums.Rows.Count; x++)
                                //               {
                                //                   for (int y = 0; y < dtGrand.Columns.Count; y++)
                                //                   {
                                //                       if (_dtRecordColums.Rows[x]["Heading"].ToString() == dtGrand.Columns[y].ColumnName
                                //                            && _dtRecordColums.Rows[i]["Heading"].ToString() == _dtRecordColums.Rows[x]["Heading"].ToString())
                                //                       {
                                //                           if (_dtRecordColums.Rows[x]["ShowTotal"].ToString().ToLower() == "true" &&
                                //                               (_dtRecordColums.Rows[x]["ColumnType"].ToString().ToLower() == "number"
                                //                               || _dtRecordColums.Rows[x]["ColumnType"].ToString().ToLower() == "calculation"))
                                //                           {


                                //                               foreach (DataRow row in dtGrand.Rows)
                                //                               {
                                //                                   try
                                //                                   {

                                //                                       //dGamount += double.Parse(Common.IgnoreSymbols(row[dtGrand.Columns[y].ColumnName].ToString()),
                                //                                       //    System.Globalization.NumberStyles.Any);

                                //                                       string strValue = "";

                                //                                       string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtRecordColums.Rows[x]["Calculation"].ToString());

                                //                                       //strValue = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + row["DBGSystemRecordID"].ToString());
                                //                                       strValue = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, int.Parse(row["DBGSystemRecordID"].ToString()), x, _iParentRecordID);
                                //                                       dGamount += double.Parse(Common.IgnoreSymbols(strValue),
                                //                                           System.Globalization.NumberStyles.Any);

                                //                                   }
                                //                                   catch (Exception ex)
                                //                                   {
                                //                                       //
                                //                                   }
                                //                               }

                                //                           }

                                //                       }
                                //                   }

                                //               }



                                //                   gvTheGrid.FooterRow.Cells[i + m].Text = gvTheGrid.FooterRow.Cells[i + m].Text +
                                //                       "</br>" + _dtRecordColums.Rows[i]["RegEx"].ToString()
                                //              + dGamount.ToString("C").Substring(1);

                                //                   bGotGrandTotal = true;
                                //            }
                                //            catch
                                //            {
                                //                //

                                //            }
                                //        }                                    
                                //}

                                //if (bGotGrandTotal == false && amount>=0)
                                //{
                                //    //
                                //    if (gvTheGrid.FooterRow.Cells[i + m].Text != "" && gvTheGrid.FooterRow.Cells[i + m].Text.ToString() != "&nbsp;")
                                //    {
                                //        //gvTheGrid.FooterRow.Cells[i + m].Text = _dtRecordColums.Rows[i]["TextType"].ToString()
                                //        //    + gvTheGrid.FooterRow.Cells[i + m].Text;

                                //        try
                                //        {

                                //            //if (chkShowAdvancedOptions.Checked && ddlUploadedBatch.SelectedValue != "")
                                //            //{
                                //            //    TextSearch = TextSearch + "  AND TempRecordID IN  (SELECT RecordID FROM TempRecord WHERE BatchID=" + ddlUploadedBatch.SelectedValue + ")";
                                //            //}

                                //            int iTN = 1;
                                //            DataTable dtGrand = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                                //               ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                                //               !chkIsActive.Checked,
                                //               chkShowOnlyWarning.Checked == false ? null : (bool?)true, null, null,
                                //               "", "", null, null, ref iTN, ref _iTotalDynamicColumns, "view", _strNumericSearch, TextSearch + TextSearchParent,
                                //              _dtDateFrom, _dtDateTo, "", "", _strViewName, _theView.ViewID);

                                //            for (int x = 0; x < _dtRecordColums.Rows.Count; x++)
                                //            {
                                //                for (int y = 0; y < dtGrand.Columns.Count; y++)
                                //                {
                                //                    if (_dtRecordColums.Rows[x]["Heading"].ToString() == dtGrand.Columns[y].ColumnName
                                //                         && _dtRecordColums.Rows[i]["Heading"].ToString() == _dtRecordColums.Rows[x]["Heading"].ToString())
                                //                    {
                                //                        if (_dtRecordColums.Rows[x]["ShowTotal"].ToString().ToLower() == "true" &&
                                //                            (_dtRecordColums.Rows[x]["ColumnType"].ToString().ToLower() == "number"))
                                //                        {


                                //                            foreach (DataRow row in dtGrand.Rows)
                                //                            {
                                //                                try
                                //                                {
                                //                                    if (row[dtGrand.Columns[y].ColumnName].ToString().Trim() != "")
                                //                                    {
                                //                                        dGamount += double.Parse(Common.IgnoreSymbols(row[dtGrand.Columns[y].ColumnName].ToString()),
                                //                                           System.Globalization.NumberStyles.Any);
                                //                                    }



                                //                                }
                                //                                catch (Exception ex)
                                //                                {
                                //                                    //
                                //                                }
                                //                            }

                                //                        }

                                //                        if (_dtRecordColums.Rows[x]["ShowTotal"].ToString().ToLower() == "true" &&
                                //                            (_dtRecordColums.Rows[x]["ColumnType"].ToString().ToLower() == "calculation"))
                                //                        {


                                //                            foreach (DataRow row in dtGrand.Rows)
                                //                            {
                                //                                try
                                //                                {


                                //                                    string strValue = "";

                                //                                    string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtRecordColums.Rows[x]["Calculation"].ToString());

                                //                                    //strValue = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + row["DBGSystemRecordID"].ToString());
                                //                                    strValue = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, int.Parse(row["DBGSystemRecordID"].ToString()), x, _iParentRecordID);
                                //                                    dGamount += double.Parse(Common.IgnoreSymbols(strValue),
                                //                                        System.Globalization.NumberStyles.Any);

                                //                                }
                                //                                catch (Exception ex)
                                //                                {
                                //                                    //
                                //                                }
                                //                            }

                                //                        }

                                //                    }
                                //                }

                                //            }



                                //            gvTheGrid.FooterRow.Cells[i + m].Text = gvTheGrid.FooterRow.Cells[i + m].Text +
                                //                "</br>" 
                                //       + dGamount.ToString("C").Substring(1);

                                //            bGotGrandTotal = true;
                                //        }
                                //        catch
                                //        {
                                //            //

                                //        }
                                //    }   

                                //}


                            }
                        }

                    }

                }



            }
        }


        if (HttpContext.Current.Session["RunSpeedLog"] != null && _theTable != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Databound Event - End ";
            theSpeedLog.FunctionLineNumber = 1950;
            SecurityManager.AddSpeedLog(theSpeedLog);

        }

    }

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {



        if (HttpContext.Current.Session["RunSpeedLog"] != null && _theTable != null)
        {

            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " " + e.Row.RowType.ToString() + " Event - RowDataBound - START ";
            theSpeedLog.FunctionLineNumber = 3259;
            SecurityManager.AddSpeedLog(theSpeedLog);

        }




        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");


        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[_iTotalDynamicColumns + 3].Visible = false;
            e.Row.Cells[_iTotalDynamicColumns + 2].Visible = false;
            //if (_bDBGSortColumnHide)
            //    e.Row.Cells[_iTotalDynamicColumns + 1].Visible = false;

            for (int j = 0; j < _dtDataSource.Columns.Count; j++)
            {
                if (_dtDataSource.Columns[j].ColumnName.IndexOf("_ID**") > -1
                    || _dtDataSource.Columns[j].ColumnName.IndexOf("_Colour**") > -1)
                {
                    e.Row.Cells[j + 4].Visible = false;
                }
            }

            for (int j = 0; j < _dtDataSource.Columns.Count; j++)
            {
                if (_dtDataSource.Columns[j].ColumnName == "DBGSystemRecordID")
                {
                    e.Row.Cells[j + 4].Visible = false;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {

            if (chkShowOnlyWarning.Checked == true)
            {
                if (chkIsActive.Checked)
                {
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;

                    if (_bDeleteReason == false)
                    {
                        e.Row.Cells[6].Visible = false;
                    }
                }
                else
                {
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Blue;
                }
            }
            else
            {
                if (chkIsActive.Checked)
                {
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;

                    if (_bDeleteReason == false)
                    {
                        e.Row.Cells[5].Visible = false;
                    }
                }
            }

            e.Row.Cells[_iTotalDynamicColumns + 3].Visible = false;
            e.Row.Cells[_iTotalDynamicColumns + 2].Visible = false;
            //if (_bDBGSortColumnHide)
            //    e.Row.Cells[_iTotalDynamicColumns + 1].Visible = false;

            for (int j = 0; j < _dtDataSource.Columns.Count; j++)
            {
                if (_dtDataSource.Columns[j].ColumnName == "DBGSystemRecordID")
                {
                    e.Row.Cells[j + 4].Visible = false;
                }
            }

            for (int j = 0; j < _dtDataSource.Columns.Count; j++)
            {
                if (_dtDataSource.Columns[j].ColumnName.IndexOf("_ID**") > -1
                    || _dtDataSource.Columns[j].ColumnName.IndexOf("_Colour**") > -1)
                {
                    e.Row.Cells[j + 4].Visible = false;
                }
            }

            //
            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {
                for (int j = 0; j < _dtDataSource.Columns.Count; j++)
                {
                    if (_dtRecordColums.Rows[i]["Heading"].ToString() == _dtDataSource.Columns[j].ColumnName)
                    {
                        if (_bIsForExport == false)
                        {
                            if (_dtRecordColums.Rows[i]["SummaryCellBackColor"] != DBNull.Value)
                            {
                                if (_dtRecordColums.Rows[i]["SummaryCellBackColor"].ToString() != "")
                                {
                                    e.Row.Cells[j + 4].Style.Add("background-color", _dtRecordColums.Rows[i]["SummaryCellBackColor"].ToString());

                                }

                            }
                        }


                        if (_dtRecordColums.Rows[i]["Width"] != DBNull.Value)
                        {
                            e.Row.Cells[j + 4].Width = int.Parse(_dtRecordColums.Rows[i]["Width"].ToString());
                        }





                        if (_dtRecordColums.Rows[i]["Alignment"] == DBNull.Value)
                        {
                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() != "number")
                            {
                                e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Right;
                            }

                            if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "RecordID")
                            {
                                e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Right;
                            }

                            if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "IsActive"
                           || _dtRecordColums.Rows[i]["SystemName"].ToString() == "DateTimeRecorded"
                                || _dtRecordColums.Rows[i]["SystemName"].ToString() == "EnteredBy")
                            {

                                e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "Notes")
                            {
                                e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Left;
                            }

                        }
                        else
                        {
                            e.Row.Cells[j + 4].HorizontalAlign = GetHorizontalAlign(_dtRecordColums.Rows[i]["Alignment"].ToString());
                        }




                        //Help Text

                        if (_dtRecordColums.Rows[i]["Notes"] != DBNull.Value)
                        {
                            if (_dtRecordColums.Rows[i]["Notes"].ToString() != "")
                            {
                                e.Row.Cells[j + 4].ToolTip = _dtRecordColums.Rows[i]["Notes"].ToString();
                            }

                        }



                        //diable sorting for table column

                        //if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                        //        && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                        //        || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                        //         && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                        //        && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                        //{

                        //    e.Row.Cells[j + 4].CssClass = "headerlink";
                        //    e.Row.Cells[j + 4].Enabled = false;

                        //}

                        if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text"
                                 && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown")
                        {

                            e.Row.Cells[j + 4].CssClass = "headerlink";
                            e.Row.Cells[j + 4].Enabled = false;

                        }

                        if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text"
                                && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "radiobutton")
                        {

                            e.Row.Cells[j + 4].CssClass = "headerlink";
                            e.Row.Cells[j + 4].Enabled = false;

                        }

                        //if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "calculation")
                        //{

                        //    e.Row.Cells[j + 4].CssClass = "headerlink";
                        //    e.Row.Cells[j + 4].Enabled = false;

                        //}

                        if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
                        {
                            if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                            {
                                e.Row.Cells[j + 4].Visible = false;
                            }
                        }


                    }

                }
            }


            //


            if (_bIsForExport)
            {
                for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                {
                    for (int j = 0; j < _dtDataSource.Columns.Count; j++)
                    {
                        if (_dtRecordColums.Rows[i]["Heading"].ToString() == _dtDataSource.Columns[j].ColumnName)
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"] == DBNull.Value)
                            {
                                e.Row.Cells[j + 4].Visible = false;

                                break;
                            }
                            else
                            {
                                e.Row.Cells[j + 4].Text = _dtRecordColums.Rows[i]["NameOnExport"].ToString();

                                break;
                            }

                        }

                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            for (int j = 0; j < _dtDataSource.Columns.Count; j++)
            {
                if (_dtDataSource.Columns[j].ColumnName.IndexOf("_ID**") > -1
                    || _dtDataSource.Columns[j].ColumnName.IndexOf("_Colour**") > -1)
                {
                    e.Row.Cells[j + 4].Visible = false;
                }
            }


            DataRowView rowView = (DataRowView)e.Row.DataItem;
            string strDBGSystemRecordID = rowView["DBGSystemRecordID"].ToString();
            string strMode = "edit";

            string strURL = GetEditURL() + Cryptography.Encrypt(strDBGSystemRecordID);


            System.Drawing.Color colWarningColor = System.Drawing.Color.Blue;

            if (_strRecordRightID == Common.UserRoleType.ReadOnly ||
                _strRecordRightID == Common.UserRoleType.AddRecord
                || ShowEditButton == false)
            {
                strURL = GetViewURL() + Cryptography.Encrypt(strDBGSystemRecordID);
                strMode = "view";
            }
            HyperLink viewHyperLink = (HyperLink)e.Row.FindControl("viewHyperLink");
            HyperLink EditHyperLink = (HyperLink)e.Row.FindControl("EditHyperLink");

            if (EditHyperLink != null)
            {
                if (_bCustomDDL)
                {
                    EditHyperLink.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Pager/Images/rrp/edit_s.png";
                }
            }


            if (_strRecordRightID == Common.UserRoleType.EditOwnViewOther
                || _strRecordRightID == Common.UserRoleType.OwnData)
            {
                Record theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(rowView["DBGSystemRecordID"].ToString()));
                if (theRecord != null)
                {
                    if (_ObjUser.UserID == theRecord.EnteredBy
                        || (theRecord.OwnerUserID != null && (_ObjUser.UserID == theRecord.OwnerUserID)))
                    {
                        strURL = GetEditURL() + Cryptography.Encrypt(strDBGSystemRecordID);
                        if (viewHyperLink != null)
                            viewHyperLink.Visible = false;

                        if (EditHyperLink != null)
                            EditHyperLink.Visible = true;
                    }
                    else
                    {

                        strURL = GetViewURL() + Cryptography.Encrypt(strDBGSystemRecordID);
                        if (EditHyperLink != null)
                            EditHyperLink.Visible = false;
                        if (viewHyperLink != null)
                            viewHyperLink.Visible = true;

                    }
                }
            }





            if (PageType == "c")
            {
                strURL = strURL + "&backurl=" + Cryptography.Encrypt(Request.RawUrl.ToString()) + "&onlyback=yes&tabindex=" + DetailTabIndex.ToString();
            }
            //e.Row.Attributes.Add("onClick", "window.open('" + strURL + "', '_self')");

            if (_theView != null)
            {
                if (strMode == "view")
                {
                    if ((bool)_theView.ShowViewIcon == false)
                        strURL = "#";
                }
                if (strMode == "edit")
                {
                    if ((bool)_theView.ShowEditIcon == false)
                        strURL = "#";
                }
            }

            if (_bOpenInParent)
            {
                strURL = strURL + "&fixedurl=" + Cryptography.Encrypt("~/Default.aspx");
            }


            if (viewHyperLink != null)
            {
                viewHyperLink.NavigateUrl = strURL;

                if (_bOpenInParent)
                    viewHyperLink.Target = "_parent";
            }



            if (EditHyperLink != null)
            {
                EditHyperLink.NavigateUrl = strURL;
                if (_bOpenInParent)
                    EditHyperLink.Target = "_parent";
            }

            e.Row.Cells[_iTotalDynamicColumns + 3].Visible = false;
            e.Row.Cells[_iTotalDynamicColumns + 2].Visible = false;

            for (int j = 0; j < _dtDataSource.Columns.Count; j++)
            {
                if (_dtDataSource.Columns[j].ColumnName == "DBGSystemRecordID")
                {
                    e.Row.Cells[j + 4].Visible = false;
                }
            }

            //if (_bDBGSortColumnHide)
            //e.Row.Cells[_iTotalDynamicColumns + 1].Visible = false;

            if (chkShowOnlyWarning.Checked == true)
            {
                string strWarning = "";

                if (rowView.Row.Table.Columns.Contains("Warning"))
                {
                    if (rowView["Warning"] != DBNull.Value && rowView["Warning"].ToString() != "")
                    {
                        strWarning = rowView["Warning"].ToString();

                        if (strWarning.IndexOf("EXCEEDANCE:") > -1 && strWarning.IndexOf("WARNING:") > -1)
                        {
                            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                            {
                                if (strWarning.IndexOf("EXCEEDANCE: " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) > -1)
                                {
                                    strWarning = strWarning.Replace("EXCEEDANCE: " + _dtRecordColums.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.",
                                       "<span style='color:#ffa500;'>" + "EXCEEDANCE: " + _dtRecordColums.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range." + "</span>");
                                }
                            }
                        }
                        else if (strWarning.IndexOf("EXCEEDANCE:") > -1)
                        {
                            colWarningColor = System.Drawing.Color.Orange;
                        }
                        else if (strWarning.IndexOf("INVALID (and ignored):") > -1)
                        {
                            colWarningColor = System.Drawing.Color.Red;
                        }
                    }
                }

                if (chkIsActive.Checked)
                {

                    e.Row.Cells[5].ForeColor = colWarningColor;

                    e.Row.Cells[5].Text = strWarning;
                    if (_bDeleteReason == false)
                    {
                        e.Row.Cells[6].Visible = false;
                    }
                }
                else
                {
                    e.Row.Cells[4].ForeColor = colWarningColor;
                    e.Row.Cells[4].Text = strWarning;
                }
            }
            else
            {
                if (chkIsActive.Checked)
                {

                    if (_bDeleteReason == false)
                    {
                        e.Row.Cells[5].Visible = false;
                    }
                }
            }




            if (_bIsForExport)
            {
                //export to pdf or word

                //Hide columns
                for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                {
                    for (int j = 0; j < _dtDataSource.Columns.Count; j++)
                    {
                        if (_dtRecordColums.Rows[i]["Heading"].ToString() == _dtDataSource.Columns[j].ColumnName)
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"] == DBNull.Value)
                            {
                                e.Row.Cells[j + 4].Visible = false;
                                break;
                            }
                            else
                            {

                                break;
                            }

                        }

                    }
                }

                //Round export
                for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                {
                    for (int j = 0; j < _dtDataSource.Columns.Count; j++)
                    {
                        //DisplayTextSummary
                        if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == _dtDataSource.Columns[j].ColumnName)
                        {
                            if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                            {

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file"
                                    || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "image")
                                {
                                    if (e.Row.Cells[j + 4].Text.ToString() != "" && e.Row.Cells[j + 4].Text.ToString() != "&nbsp;")
                                    {
                                        try
                                        {
                                            if (e.Row.Cells[j + 4].Text.ToString().Length > 37)
                                            {
                                                e.Row.Cells[j + 4].Text = e.Row.Cells[j + 4].Text.Substring(37);

                                            }
                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }

                                }



                                if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
                                {
                                    if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
                                    {
                                        if (e.Row.Cells[j + 4].Text.ToString() != "" && e.Row.Cells[j + 4].Text.ToString() != "&nbsp;")
                                        {
                                            try
                                            {
                                                if (Common.HasSymbols(e.Row.Cells[j + 4].Text) == false)
                                                    e.Row.Cells[j + 4].Text = Math.Round(double.Parse(e.Row.Cells[j + 4].Text),
                                                   int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtRecordColums.Rows[i]["RoundNumber"].ToString());
                                            }
                                            catch
                                            {

                                            }

                                        }
                                    }
                                }
                            }

                        }

                        //hh:mm
                        if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
                        {
                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == _dtDataSource.Columns[j].ColumnName)
                            {
                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "datetime")
                                {
                                    if (e.Row.Cells[j + 4].Text.Length > 15)
                                    {
                                        e.Row.Cells[j + 4].Text = e.Row.Cells[j + 4].Text.Substring(0, 16);
                                    }
                                }
                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "date")
                                {
                                    if (e.Row.Cells[j + 4].Text.Length > 9)
                                    {
                                        e.Row.Cells[j + 4].Text = e.Row.Cells[j + 4].Text.Substring(0, 10);
                                    }
                                }
                            }
                        }


                    }
                }






            }
            else
            {
                //Not export


                //if (DataBinder.Eval(e.Row.DataItem, "DBGSystemRecordID") != DBNull.Value)
                //{
                //    strDBGSystemRecordID = DataBinder.Eval(e.Row.DataItem, "DBGSystemRecordID").ToString();
                //}

                for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                {
                    for (int j = 0; j < _dtDataSource.Columns.Count; j++)
                    {

                        if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "RecordID")
                        {
                            if (_dtRecordColums.Rows[i]["Heading"].ToString() == "" && _dtDataSource.Columns[j].ColumnName == "DBGSystemRecordID")
                            {

                                //strRecordID = e.Row.Cells[j + 4].Text;
                            }

                        }


                        if (_dtRecordColums.Rows[i]["Heading"].ToString() == _dtDataSource.Columns[j].ColumnName)
                        {

                            if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "RecordID")
                            {
                                if (_dtRecordColums.Rows[i]["Alignment"] == DBNull.Value)
                                {
                                    e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Right;
                                }
                                else
                                {
                                    e.Row.Cells[j + 4].HorizontalAlign = GetHorizontalAlign(_dtRecordColums.Rows[i]["Alignment"].ToString());
                                }
                                //strRecordID = e.Row.Cells[j + 4].Text;
                            }



                            if (rowView[_dtRecordColums.Rows[i]["Heading"].ToString()] != DBNull.Value && rowView[_dtRecordColums.Rows[i]["Heading"].ToString()].ToString() != "")
                            {
                                string strValueAsString = rowView[_dtRecordColums.Rows[i]["Heading"].ToString()].ToString();

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "text" && (_dtRecordColums.Rows[i]["TextType"] == DBNull.Value
                                                 || (_dtRecordColums.Rows[i]["TextType"] != DBNull.Value && _dtRecordColums.Rows[i]["TextType"].ToString() == "text")))
                                {
                                    string strDisplaText = Common.StripTagsCharArray(strValueAsString);
                                    if (strDisplaText.Length > _iMaxCharactersInCell)
                                    {
                                        strDisplaText = strDisplaText.Substring(0, _iMaxCharactersInCell) + "...";
                                        e.Row.Cells[j + 4].Text = "";

                                        string strCellContent = "<div class='js-tooltip-container'> <div class='js-tooltip' style='display:none;'><span>" + strValueAsString + "</span> </div>";
                                        strCellContent = strCellContent + "<span>" + strDisplaText + "</span></div>";

                                        e.Row.Cells[j + 4].Text = strCellContent;
                                    }
                                }

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() != "file"
                                                        && _dtRecordColums.Rows[i]["ColumnType"].ToString() != "image")
                                {
                                    if ((_dtRecordColums.Rows[i]["ColumnType"].ToString() == "text"
                                        && _dtRecordColums.Rows[i]["TextType"].ToString() == "link")
                                        ||
                                    (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                    && _dtRecordColums.Rows[i]["ShowViewLink"] != DBNull.Value
                                    && (_dtRecordColums.Rows[i]["ShowViewLink"].ToString().ToLower() == "summary"
                                    || _dtRecordColums.Rows[i]["ShowViewLink"].ToString().ToLower() == "both")))
                                    {

                                    }
                                    else
                                    {
                                        if (_bOpenInParent)
                                        {
                                            e.Row.Cells[j + 4].Attributes.Add("onClick", "window.open('" + strURL + "', '_parent')");
                                        }
                                        else
                                        {
                                            e.Row.Cells[j + 4].Attributes.Add("onClick", "window.open('" + strURL + "', '_self')");
                                        }
                                    }
                                }

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "text" && _dtRecordColums.Rows[i]["TextType"].ToString() == "link")
                                {
                                    string strLinkURL = strValueAsString;

                                    if (strLinkURL.IndexOf("http") == -1)
                                    {
                                        strLinkURL = Request.Url.Scheme + "://" + strLinkURL;
                                    }

                                    e.Row.Cells[j + 4].Text = "<a target='_blank' href='" + strLinkURL + "'>"
                                                        + e.Row.Cells[j + 4].Text + "</a>";
                                }

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "text"
                                    && _dtRecordColums.Rows[i]["TextHeight"] != DBNull.Value && int.Parse(_dtRecordColums.Rows[i]["TextHeight"].ToString()) > 1)
                                {
                                    e.Row.Cells[j + 4].Text = e.Row.Cells[j + 4].Text.Replace("\n", "<br />");
                                }

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "time")
                                {
                                    TimeSpan ts = new TimeSpan();
                                    if (TimeSpan.TryParse(strValueAsString, out ts))
                                    {
                                        e.Row.Cells[j + 4].Text = ts.ToString(@"hh\:mm");
                                    }
                                }

                                if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                                {

                                    if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "image" && strValueAsString.Length > 37)
                                    {
                                        try
                                        {
                                            string strFilePath = _strFilesLocation + "/UserFiles/AppFiles/" + strValueAsString;
                                            string strMaxHeight = "30";
                                            if (_dtRecordColums.Rows[i]["TextWidth"] != DBNull.Value)
                                            {
                                                strMaxHeight = _dtRecordColums.Rows[i]["TextWidth"].ToString();
                                            }
                                            e.Row.Cells[j + 4].Text = "<a target='_blank' href='" + strFilePath + "'>"
                                                        + "<img style='max-height:" + strMaxHeight + "px;' alt='" + strValueAsString.Substring(37)
                                            + "' src='" + strFilePath + "' title='" + strValueAsString.Substring(37) + "'   />" + "</a>";

                                        }
                                        catch
                                        {
                                            //
                                        }

                                    }//image




                                    if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file" && strValueAsString.Length > 37)
                                    {
                                        try
                                        {
                                            string strFilePath = Cryptography.Encrypt(_strFilesLocation + "/UserFiles/AppFiles/" + strValueAsString);
                                            string strFileName = Cryptography.Encrypt(strValueAsString.Substring(37));
                                            string strFileURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Filedownload.aspx?FilePath="
                            + strFilePath + "&FileName=" + strFileName;

                                            e.Row.Cells[j + 4].Text = "<a target='_blank' href='" + strFileURL + "'>"
                                                + strValueAsString.Substring(37) + "</a>";

                                        }
                                        catch
                                        {
                                            //
                                        }

                                    }//File



                                    if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_image" && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "radiobutton"
                                  && _dtRecordColums.Rows[i]["DropdownValues"] != DBNull.Value && _dtRecordColums.Rows[i]["DropdownValues"].ToString() != ""
                                  && _dtRecordColums.Rows[i]["ImageOnSummary"] != DBNull.Value && _dtRecordColums.Rows[i]["ImageOnSummary"].ToString().ToLower() == "true")
                                    {
                                        string strText = Common.GetImageFromValueForDD(_dtRecordColums.Rows[i]["DropdownValues"].ToString(), strValueAsString, _strFilesLocation,
                                              _dtRecordColums.Rows[i]["TextHeight"] == DBNull.Value ? "" : _dtRecordColums.Rows[i]["TextHeight"].ToString());
                                        if (strText != "")
                                            e.Row.Cells[j + 4].Text = strText;
                                    }


                                    if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text" && (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                    || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "radiobutton")
                                   && _dtRecordColums.Rows[i]["DropdownValues"].ToString() != "")
                                    {
                                        string strText = GetTextFromValueForDD(_dtRecordColums.Rows[i]["DropdownValues"].ToString(), strValueAsString);
                                        if (strText != "")
                                            e.Row.Cells[j + 4].Text = strText;

                                    }


                                    if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text"
                                  && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "listbox"
                                 && _dtRecordColums.Rows[i]["DropdownValues"] != DBNull.Value && _dtRecordColums.Rows[i]["DropdownValues"].ToString() != "")
                                    {
                                        string strText = Common.GetTextFromValueForList(_dtRecordColums.Rows[i]["DropdownValues"].ToString(), strValueAsString);
                                        if (strText != "")
                                            e.Row.Cells[j + 4].Text = strText;

                                    }


                                    if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value
                                       && (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "number"
                                       || _dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "calculation")
                                       && _dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
                                    {
                                        try
                                        {
                                            bool bDate = false;

                                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "calculation"
                                                && _dtRecordColums.Rows[i]["TextType"] != DBNull.Value
                                                    && _dtRecordColums.Rows[i]["TextType"].ToString().ToLower() == "d")
                                            {
                                                bDate = true;
                                            }

                                            if (bDate == false)
                                            {
                                                double dTempTest = 0;
                                                if (Common.HasSymbols(strValueAsString) == false)
                                                {
                                                    if (double.TryParse(Common.IgnoreSymbols(strValueAsString), out dTempTest))
                                                    {
                                                        strValueAsString = Math.Round(dTempTest,
                                                           int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtRecordColums.Rows[i]["RoundNumber"].ToString());
                                                        e.Row.Cells[j + 4].Text = strValueAsString;
                                                    }
                                                }


                                            }

                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }


                                    if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "number"
                                        && _dtRecordColums.Rows[i]["TextType"] != DBNull.Value && _dtRecordColums.Rows[i]["TextType"].ToString() != ""
                                       && _dtRecordColums.Rows[i]["NumberType"] != DBNull.Value && _dtRecordColums.Rows[i]["NumberType"].ToString() == "6")
                                    {

                                        try
                                        {

                                            string strMoney = strValueAsString;
                                            strMoney = Common.IgnoreSymbols(strMoney);

                                            e.Row.Cells[j + 4].Text = _dtRecordColums.Rows[i]["TextType"].ToString()
                                                + double.Parse(strMoney).ToString("C").Substring(1);
                                        }
                                        catch
                                        {
                                            //
                                        }

                                    }

                                    if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "calculation"
                                       && _dtRecordColums.Rows[i]["TextType"].ToString() == "f"
                                        && _dtRecordColums.Rows[i]["RegEx"].ToString() != "")
                                    {
                                        try
                                        {
                                            string strMoney = strValueAsString;
                                            strMoney = Common.IgnoreSymbols(strMoney);

                                            e.Row.Cells[j + 4].Text = _dtRecordColums.Rows[i]["RegEx"].ToString()
                                                + double.Parse(strMoney).ToString("C").Substring(1);
                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }

                                    if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "location")
                                    {

                                        try
                                        {
                                            string strFullString = "";

                                            LocationColumn theLocationColumn = JSONField.GetTypedObject<LocationColumn>(strValueAsString);
                                            if (theLocationColumn != null)
                                            {

                                                if (theLocationColumn.Address != null)
                                                {
                                                    strFullString = theLocationColumn.Address;
                                                }
                                                else
                                                {
                                                    if (theLocationColumn.Latitude != null
                                                        && theLocationColumn.Longitude != null)
                                                    {
                                                        strFullString = "Lat:" + theLocationColumn.Latitude.ToString()
                                                            + ",Long:" + theLocationColumn.Longitude.ToString();
                                                    }
                                                }
                                            }
                                            e.Row.Cells[j + 4].Text = strFullString;
                                        }
                                        catch
                                        {
                                            //
                                            e.Row.Cells[j + 4].Text = "";
                                        }

                                    }






                                }//IsStandard==false



                            }







                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "trafficlight"
                       && _dtRecordColums.Rows[i]["TrafficLightColumnID"] != DBNull.Value
                       && _dtRecordColums.Rows[i]["TrafficLightValues"] != DBNull.Value)
                            {
                                Column theTrafficLightColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["TrafficLightColumnID"].ToString()));
                                if (theTrafficLightColumn != null)
                                {
                                    string strTLValue = Common.GetValueFromSQL("SELECT " + theTrafficLightColumn.SystemName + " FROM [Record] WHERE RecordID=" + strDBGSystemRecordID);
                                    string strImageURL = Common.TrafficLightURL(theTrafficLightColumn, strTLValue, _dtRecordColums.Rows[i]["TrafficLightValues"].ToString());


                                    if (strImageURL != "")
                                    {
                                        e.Row.Cells[j + 4].Text = "<img alt='Traffic Light' src='" + Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + strImageURL + "' />";
                                    }

                                }

                            }



                            // C S
                            if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                            {

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "number"
                                    && _dtRecordColums.Rows[i]["NumberType"] != DBNull.Value)
                                {
                                    if (_dtRecordColums.Rows[i]["NumberType"].ToString() == "5")//record count
                                    {


                                        Table theTable = RecordManager.ets_Table_Details(int.Parse(_dtRecordColums.Rows[i]["TableTableID"].ToString()));
                                        string strTextSearch = "";
                                        if (theTable != null)
                                        {
                                            DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,ColumnID FROM [Column] WHERE   TableID=" + _dtRecordColums.Rows[i]["TableTableID"].ToString() + " AND TableTableID=" + TableID.ToString());
                                            foreach (DataRow drCT in dtTemp.Rows)
                                            {
                                                Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(drCT["ColumnID"].ToString()));
                                                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                                                Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strDBGSystemRecordID));
                                                string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);
                                                strLinkedColumnValue = strLinkedColumnValue.Replace("'", "''");

                                                if (strTextSearch == "")
                                                {
                                                    strTextSearch = " Record." + drCT["SystemName"].ToString() + "='" + strLinkedColumnValue + "' ";
                                                }
                                                else
                                                {
                                                    strTextSearch = strTextSearch + " OR " + " Record." + drCT["SystemName"].ToString() + "='" + strLinkedColumnValue + "' ";
                                                }

                                            }

                                            if (strTextSearch.Trim() != "")
                                                strTextSearch = " AND (" + strTextSearch + ")";
                                            int _iTotalDynamicColumnsTem = 0;
                                            int iTNTemp = 0;

                                            string strReturnSQL = "";
                                            RecordManager.ets_Record_List(int.Parse(_dtRecordColums.Rows[i]["TableTableID"].ToString()), null, true,
                                           false, null, null,
                                           "", "", 0, 1, ref iTNTemp, ref _iTotalDynamicColumnsTem, "view", "",
                                           strTextSearch, null, null, "", "", "", _theView.ViewID, ref strReturnSQL, ref strReturnSQL);

                                            if (_dtRecordColums.Rows[i]["DropDownValues"].ToString() == "no")
                                            {
                                                if (iTNTemp == 0)
                                                {
                                                    //e.Row.Cells[j + 4].Text = iTNTemp.ToString();
                                                }
                                                else
                                                {
                                                    e.Row.Cells[j + 4].Text = iTNTemp.ToString();
                                                }
                                            }
                                            else
                                            {
                                                if (iTNTemp == 0)
                                                {
                                                    //e.Row.Cells[j + 4].Text = iTNTemp.ToString();
                                                }
                                                else
                                                {
                                                    string strChildTableLink = " <a href='RecordList.aspx?TableID=" + Cryptography.Encrypt(_dtRecordColums.Rows[i]["TableTableID"].ToString()) + "&TextSearch=" + Cryptography.Encrypt(strTextSearch) + "' target='_blank'>" + iTNTemp.ToString() + "</a>";
                                                    e.Row.Cells[j + 4].Text = Server.HtmlDecode(strChildTableLink);
                                                    e.Row.Cells[j + 4].Attributes.Add("onClick", "");

                                                }

                                            }

                                        }

                                        //}


                                    }


                                }//Record Count Column 







                                //if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                //    && (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "listbox")
                                //   && _dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                //    && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                                //{


                                //    if (e.Row.Cells[j + 4].Text.ToString() != "" && e.Row.Cells[j + 4].Text.ToString() != "&nbsp;")
                                //    {
                                //        //string strText = GetTextFromTableForList((int)_dtRecordColums.Rows[i]["TableTableID"], null, _dtRecordColums.Rows[i]["DisplayColumn"].ToString(), e.Row.Cells[j + 4].Text.ToString());
                                //        //if (strText != "")
                                //        if (e.Row.Cells[j + 4].Text.Trim().Substring(0,1)==",")
                                //            e.Row.Cells[j + 4].Text = e.Row.Cells[j + 4].Text.Trim().Substring(1);
                                //    }

                                //}





                                if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                    && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                    || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                                     && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                      && _dtRecordColums.Rows[i]["LinkedParentColumnID"] != DBNull.Value
                                    && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                                {


                                    if (_dtRecordColums.Rows[i]["ShowViewLink"] != DBNull.Value
                                        && (_dtRecordColums.Rows[i]["ShowViewLink"].ToString().ToLower() == "summary"
                                || _dtRecordColums.Rows[i]["ShowViewLink"].ToString().ToLower() == "both"))
                                    {
                                        if (e.Row.Cells[j + 4].Text.ToString() != "" && e.Row.Cells[j + 4].Text.ToString() != "&nbsp;")
                                        {
                                            try
                                            {
                                                string strPaRecordID = e.Row.Cells[_dtDataSource.Columns["**" + _dtDataSource.Columns[j].ColumnName + "_ID**"].Ordinal + 4].Text.ToString();
                                                int iPRecordID = 0;
                                                bool bIsRecord = false;
                                                if (int.TryParse(strPaRecordID, out iPRecordID))
                                                {
                                                    Record thePaRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strPaRecordID));

                                                    if (thePaRecord != null)
                                                    {

                                                        bIsRecord = true;
                                                    }

                                                }

                                                string strTarget = "_parent";

                                                string strFixedURL = "";

                                                if (_bOpenInParent)
                                                {
                                                    strFixedURL = "&fixedurl=" + Cryptography.Encrypt("~/Default.aspx");
                                                }

                                                if (strPaRecordID != "" && bIsRecord)
                                                {

                                                    string strLink = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" +
                                                Cryptography.Encrypt(strMode) + "&TableID=" + Cryptography.Encrypt(_dtRecordColums.Rows[i]["TableTableID"].ToString())
                                                + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&RecordID=" + Cryptography.Encrypt(strPaRecordID) + "&UrlReferrer=y" + strFixedURL;
                                                    string strViewHTML = "<a  href='" + strLink + "' target='" + strTarget + "'> " + e.Row.Cells[j + 4].Text.ToString() + " <a>";

                                                    e.Row.Cells[j + 4].Text = strViewHTML;
                                                }
                                                else
                                                {
                                                    if (strPaRecordID != "" && bIsRecord == false)
                                                    {
                                                        try
                                                        {
                                                            Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));

                                                            DataTable dtTheRecord = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "='" + strPaRecordID.Replace("'", "''") + "'");

                                                            if (dtTheRecord.Rows.Count > 0)
                                                            {
                                                                strPaRecordID = dtTheRecord.Rows[0]["RecordID"].ToString();
                                                                string strLink = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" +
                                               Cryptography.Encrypt(strMode) + "&TableID=" + Cryptography.Encrypt(_dtRecordColums.Rows[i]["TableTableID"].ToString())
                                               + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&RecordID=" + Cryptography.Encrypt(strPaRecordID) + "&UrlReferrer=y" + strFixedURL;
                                                                string strViewHTML = "<a  href='" + strLink + "'  target='" + strTarget + "'> " + e.Row.Cells[j + 4].Text.ToString() + " <a>";

                                                                e.Row.Cells[j + 4].Text = strViewHTML;
                                                            }

                                                        }
                                                        catch
                                                        {
                                                            //

                                                        }

                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                //
                                            }
                                        }
                                    }


                                    //if (e.Row.Cells[j + 4].Text.ToString() != "" && e.Row.Cells[j + 4].Text.ToString() != "&nbsp;")
                                    //{
                                    //    try
                                    //    {

                                    //        Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));


                                    //        //int iTableRecordID = int.Parse(e.Row.Cells[j + 4].Text);
                                    //        string strPaRecordID = e.Row.Cells[j + 4].Text;
                                    //        string strLinkedColumnValue = e.Row.Cells[j + 4].Text;
                                    //        DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
                                    //         + _dtRecordColums.Rows[i]["TableTableID"].ToString());

                                    //        string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();
                                    //        string sstrDisplayColumnOrg = strDisplayColumn;

                                    //        foreach (DataRow dr in dtTableTableSC.Rows)
                                    //        {
                                    //            strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                                    //        }

                                    //        sstrDisplayColumnOrg = strDisplayColumn;
                                    //        string strFilterSQL = "";
                                    //        if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                    //        {

                                    //            strFilterSQL = strLinkedColumnValue;
                                    //        }
                                    //        else
                                    //        {
                                    //            strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                                    //        }

                                    //        DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);
                                    //        if (dtTheRecord.Rows.Count > 0)
                                    //        {

                                    //            strPaRecordID = dtTheRecord.Rows[0]["RecordID"].ToString();
                                    //            foreach (DataColumn dc in dtTheRecord.Columns)
                                    //            {
                                    //                Column theColumn = RecordManager.ets_Column_Details_By_Sys((int)theLinkedColumn.TableID, dc.ColumnName);
                                    //                if (theColumn != null)
                                    //                {
                                    //                    if (theColumn.ColumnType == "date")
                                    //                    {
                                    //                        string strDatePartOnly = dtTheRecord.Rows[0][dc.ColumnName].ToString();

                                    //                        if (strDatePartOnly.Length > 9)
                                    //                        {
                                    //                            strDatePartOnly = strDatePartOnly.Substring(0, 10);
                                    //                        }

                                    //                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", strDatePartOnly);
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                    //                    }
                                    //                }

                                    //            }
                                    //        }
                                    //        if (sstrDisplayColumnOrg != strDisplayColumn)
                                    //            e.Row.Cells[j + 4].Text = strDisplayColumn;

                                    //        if (_dtRecordColums.Rows[i]["ShowViewLink"] != DBNull.Value
                                    //            && _dtRecordColums.Rows[i]["ShowViewLink"].ToString().ToLower() == "true")
                                    //        {
                                    //            if (strPaRecordID != "")
                                    //            {
                                    //                string strLink = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" +
                                    //            Cryptography.Encrypt(strMode) + "&TableID=" + Cryptography.Encrypt(_dtRecordColums.Rows[i]["TableTableID"].ToString())
                                    //            + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&RecordID=" + Cryptography.Encrypt(strPaRecordID) + "&UrlReferrer=y";
                                    //                string strViewHTML = "<a  href='" + strLink + "'> " + strDisplayColumn + " <a>";

                                    //                if (sstrDisplayColumnOrg != strDisplayColumn)
                                    //                    e.Row.Cells[j + 4].Text = strViewHTML;
                                    //            }
                                    //        }

                                    //    }
                                    //    catch
                                    //    {
                                    //        //
                                    //    }


                                    //}


                                }

                            }


                            if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
                            {

                                if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                                {
                                    e.Row.Cells[j + 4].Visible = false;
                                }

                            }



                            if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                            {


                                if (_dtRecordColums.Rows[i]["SummaryCellBackColor"] != DBNull.Value && _dtRecordColums.Rows[i]["SummaryCellBackColor"].ToString() != "")
                                {
                                    e.Row.Cells[j + 4].Style.Add("background-color", _dtRecordColums.Rows[i]["SummaryCellBackColor"].ToString());
                                }


                                if (_dtRecordColums.Rows[i]["Alignment"] == DBNull.Value)
                                {
                                    if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() != "number")
                                    {
                                        e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    else
                                    {
                                        e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                }
                                else
                                {
                                    e.Row.Cells[j + 4].HorizontalAlign = GetHorizontalAlign(_dtRecordColums.Rows[i]["Alignment"].ToString());
                                }

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "date")
                                {
                                    if (e.Row.Cells[j + 4].Text.Length > 9)
                                    {
                                        e.Row.Cells[j + 4].Text = e.Row.Cells[j + 4].Text.Substring(0, 10);
                                    }
                                }

                            }

                            //mm:hh
                            if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
                            {

                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "datetime")
                                {
                                    if (e.Row.Cells[j + 4].Text.Length > 15)
                                    {
                                        e.Row.Cells[j + 4].Text = e.Row.Cells[j + 4].Text.Substring(0, 16);
                                    }
                                }
                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString().ToLower() == "date")
                                {
                                    if (e.Row.Cells[j + 4].Text.Length > 9)
                                    {
                                        e.Row.Cells[j + 4].Text = e.Row.Cells[j + 4].Text.Substring(0, 10);
                                    }
                                }

                            }

                            if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "IsActive"
                            || _dtRecordColums.Rows[i]["SystemName"].ToString() == "DateTimeRecorded"
                            || _dtRecordColums.Rows[i]["SystemName"].ToString() == "EnteredBy")
                            {

                                if (_dtRecordColums.Rows[i]["Alignment"] == DBNull.Value)
                                {
                                    e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    e.Row.Cells[j + 4].HorizontalAlign = GetHorizontalAlign(_dtRecordColums.Rows[i]["Alignment"].ToString());
                                }

                            }

                            if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "Notes")
                            {

                                if (_dtRecordColums.Rows[i]["Alignment"] == DBNull.Value)
                                {
                                    e.Row.Cells[j + 4].HorizontalAlign = HorizontalAlign.Left;
                                }
                                else
                                {
                                    e.Row.Cells[j + 4].HorizontalAlign = GetHorizontalAlign(_dtRecordColums.Rows[i]["Alignment"].ToString());
                                }


                            }


                            if (strDBGSystemRecordID != "-1")
                            {
                                Record aRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strDBGSystemRecordID));

                                if (aRecord != null)
                                {
                                    if (aRecord.WarningResults != "")
                                    {


                                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                                        {
                                            string strToolTips = "";


                                            string strEachFormulaV = "";
                                            string strEachFormulaW = "";
                                            string strEachFormulaE = "";

                                            if (_dtRecordColums.Rows[i]["ConV"] != DBNull.Value)
                                            {
                                                Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["ConV"].ToString()));
                                                if (theCheckColumn != null)
                                                {
                                                    string strCheckValue = RecordManager.GetRecordValue(ref aRecord, theCheckColumn.SystemName);
                                                    strEachFormulaV = UploadWorld.Condition_GetFormula(int.Parse(_dtRecordColums.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                                        "V", strCheckValue);
                                                }
                                            }
                                            else
                                            {
                                                if (_dtRecordColums.Rows[i]["ValidationOnEntry"] != DBNull.Value && _dtRecordColums.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                                                {
                                                    strEachFormulaV = _dtRecordColums.Rows[i]["ValidationOnEntry"].ToString();
                                                }
                                            }

                                            if (_dtRecordColums.Rows[i]["ConW"] != DBNull.Value)
                                            {
                                                Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["ConW"].ToString()));
                                                if (theCheckColumn != null)
                                                {
                                                    string strCheckValue = RecordManager.GetRecordValue(ref aRecord, theCheckColumn.SystemName);
                                                    strEachFormulaW = UploadWorld.Condition_GetFormula(int.Parse(_dtRecordColums.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                                        "W", strCheckValue);
                                                }
                                            }
                                            else
                                            {
                                                if (_dtRecordColums.Rows[i]["ValidationOnWarning"] != DBNull.Value && _dtRecordColums.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                                                {
                                                    strEachFormulaW = _dtRecordColums.Rows[i]["ValidationOnWarning"].ToString();
                                                }
                                            }



                                            if (_dtRecordColums.Rows[i]["ConE"] != DBNull.Value)
                                            {
                                                Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["ConE"].ToString()));
                                                if (theCheckColumn != null)
                                                {
                                                    string strCheckValue = RecordManager.GetRecordValue(ref aRecord, theCheckColumn.SystemName);
                                                    strEachFormulaE = UploadWorld.Condition_GetFormula(int.Parse(_dtRecordColums.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                                        "E", strCheckValue);
                                                }
                                            }
                                            else
                                            {
                                                if (_dtRecordColums.Rows[i]["ValidationOnExceedance"] != DBNull.Value && _dtRecordColums.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                                                {
                                                    strEachFormulaE = _dtRecordColums.Rows[i]["ValidationOnExceedance"].ToString();
                                                }
                                            }





                                            if (aRecord.WarningResults.IndexOf("EXCEEDANCE: " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0)
                                            {
                                                e.Row.Cells[j + 4].ForeColor = System.Drawing.Color.Orange;
                                                strToolTips = strToolTips + Common.GetFromulaMsg("e", _dtRecordColums.Rows[i]["DisplayName"].ToString(), strEachFormulaE); //"EXCEEDANCE: Value outside accepted range(" + strEachFormulaE + "). ";
                                            }
                                            else if (aRecord.WarningResults.IndexOf("INVALID (and ignored): " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0)
                                            {
                                                e.Row.Cells[j + 4].ForeColor = System.Drawing.Color.Red;
                                                strToolTips = Common.GetFromulaMsg("i", _dtRecordColums.Rows[i]["DisplayName"].ToString(), strEachFormulaV);// "INVALID (and ignored):" + strEachFormulaV + ". ";
                                            }
                                            else if (aRecord.WarningResults.IndexOf("WARNING: " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0)
                                            {
                                                e.Row.Cells[j + 4].ForeColor = System.Drawing.Color.Blue;
                                                strToolTips = "";
                                                if (aRecord.WarningResults.IndexOf("SENSOR WARNING: " + _dtRecordColums.Rows[i]["DisplayName"].ToString() + " - Value below minimum detectable limit") >= 0)
                                                {
                                                    strToolTips = "SENSOR: Value below minimum detectable limit. ";
                                                }
                                                if (aRecord.WarningResults.IndexOf("SENSOR WARNING: " + _dtRecordColums.Rows[i]["DisplayName"].ToString() + " - Sensor out of Calibration") >= 0)
                                                {
                                                    strToolTips = strToolTips + "SENSOR: Sensor out of Calibration. ";
                                                }
                                                if (aRecord.WarningResults.IndexOf("WARNING: " + _dtRecordColums.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range") >= 0)
                                                {
                                                    strToolTips = strToolTips + Common.GetFromulaMsg("w", _dtRecordColums.Rows[i]["DisplayName"].ToString(), strEachFormulaW);// "WARNING: Value outside accepted range(" + strEachFormulaW + "). ";
                                                }

                                                if (aRecord.WarningResults.IndexOf("WARNING: " + _dtRecordColums.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.") >= 0)
                                                {
                                                    strToolTips = strToolTips + "WARNING:  – Unlikely data – outside 3 standard deviations.";
                                                }


                                            }



                                            if (aRecord.ValidationResults != "" && chkIsActive.Checked == false)
                                            {
                                                if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
                                                {

                                                    if (aRecord.ValidationResults.IndexOf("INVALID (and ignored): " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0
                                                        || aRecord.ValidationResults.IndexOf("INVALID: " + _dtRecordColums.Rows[i]["DisplayName"].ToString()) >= 0)
                                                    {
                                                        e.Row.Cells[j + 4].ForeColor = System.Drawing.Color.Red;
                                                        strToolTips = Common.GetFromulaMsg("i", _dtRecordColums.Rows[i]["DisplayName"].ToString(), strEachFormulaV);// "INVALID (and ignored):" + strEachFormulaV + ". ";

                                                    }

                                                }
                                            }


                                            e.Row.Cells[j + 4].ToolTip = strToolTips;
                                        }
                                        else
                                        {
                                            if (_dtRecordColums.Rows[i]["SystemName"].ToString() == "DateTimeRecorded")
                                            {

                                                if (aRecord.WarningResults.IndexOf("" + WarningMsg.MaxtimebetweenRecords + "") >= 0)
                                                {
                                                    e.Row.Cells[j + 4].ForeColor = System.Drawing.Color.Blue;
                                                    e.Row.Cells[j + 4].ToolTip = "WARNING: " + WarningMsg.MaxtimebetweenRecords + ".";
                                                }


                                            }
                                        }


                                    }

                                    //validation





                                }

                            }

                            if (_dtRecordColums.Rows[i]["ColourCells"] != null && _dtRecordColums.Rows[i]["ColourCells"].ToString().ToLower() == "true")
                            {
                                string strCN = "**" + _dtDataSource.Columns[j].ColumnName + "_Colour**";
                                if (_dtDataSource.Columns.Contains(strCN))
                                {
                                    if (rowView[strCN] != DBNull.Value && rowView[strCN].ToString() != "")
                                    {
                                        e.Row.Cells[j + 4].Style.Add("color", "#" + rowView[strCN].ToString());
                                    }


                                }


                            }

                            //if (_dtDataSource.Columns[j].ColumnName.IndexOf("_Colour**") > -1 && rowView[_dtDataSource.Columns[j].ColumnName] != DBNull.Value)
                            //{
                            //    e.Row.Cells[j + 4 - 1].Style.Add("color", "#" + rowView[_dtDataSource.Columns[j].ColumnName].ToString());
                            //}

                        }  //            
                    }
                }








            }
        }



        if (Session["RunSpeedLog"] != null && _theTable != null)
        {


            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " " + e.Row.RowType.ToString() + " RowDataBound - END ";
            theSpeedLog.FunctionLineNumber = 4385;
            SecurityManager.AddSpeedLog(theSpeedLog);

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

    protected void ddlAccountFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        //PopulateRecordGroupFilter();
        //PopulateLocationList();
        PopulateUser();
        PopulateBatch();

    }


    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);

    }


    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {


        _gvPager.ExportFileName = lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd");
        gvTheGrid.PageIndex = 0;// why???
        //BindTheGridForExport(0, _gvPager.TotalRows);
        BindTheGrid(0, _gvPager.TotalRows);

    }

    protected void cbcSearchMain_OnddlCompareOperator_Changed(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
    protected void cbcSearch1_OnddlCompareOperator_Changed(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
    protected void cbcSearch2_OnddlCompareOperator_Changed(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
    protected void cbcSearch3_OnddlCompareOperator_Changed(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
    protected void cbcSearchMain_OnddlYAxis_Changed(object sender, EventArgs e)
    {
        if (cbcSearchMain.ddlYAxisV == "")
        {
            //cbcSearchMain.PopulateSearchParams();
            lnkAddSearch1.Visible = false;
            lnkSearch_Click(null, null);
        }
        else
        {
            lnkAddSearch1.Visible = true;
        }
    }
    protected void cbcSearch1_OnddlYAxis_Changed(object sender, EventArgs e)
    {
        if (cbcSearch1.ddlYAxisV == "")
        {
            lnkAddSearch2.Visible = false;
            lnkSearch_Click(null, null);
        }
        else
        {
            lnkAddSearch2.Visible = true;
        }
    }

    protected void cbcSearch2_OnddlYAxis_Changed(object sender, EventArgs e)
    {
        if (cbcSearch2.ddlYAxisV == "")
        {
            lnkAddSearch3.Visible = false;
            lnkSearch_Click(null, null);
        }
        else
        {
            lnkAddSearch3.Visible = true;
        }
    }


    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {

        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }


    protected void PopulateExportTemplate(int iTableID)
    {
        ddlTemplate.Items.Clear();
        ddlTemplate.DataSource = Common.DataTableFromText("SELECT * FROM ExportTemplate WHERE TableID=" + iTableID + " ORDER BY ExportTemplateName");
        ddlTemplate.DataBind();

        //ListItem liSelect = new ListItem("--Please select--", "");
        //ddlTemplate.Items.Insert(0, liSelect);

    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {

        chklstFields.Items.Clear();

        if (ddlTemplate.SelectedValue == "")
        {
            //[Column]
            DataTable dtColumns = Common.DataTableFromText(@" SELECT ColumnID AS FieldID,NameOnExport AS Heading,SystemName 
                            FROM [Column] WHERE TableID=" + _theTable.TableID.ToString() + @" and Systemname not in('IsActive','TableID') AND NameOnExport IS NOT NULL AND LEN(NameOnExport) > 0
            AND ColumnType NOT IN ('staticcontent') ORDER BY DisplayRight,DisplayOrder");
            //chklstFields.DataBind();

            foreach (DataRow dr in dtColumns.Rows)
            {
                ListItem liTemp = new ListItem(dr["Heading"].ToString(), dr["FieldID"].ToString());
                liTemp.Selected = true;
                //liTemp.Attributes.Add("DataValue", dr["SystemName"].ToString());
                chklstFields.Items.Add(liTemp);
            }
            hlExportTemplate.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&SearchCriteriaET=" + Cryptography.Encrypt("-1");

        }
        else
        {
            //[ExportTemplateItem]
            DataTable dtColumns = Common.DataTableFromText(@" SELECT ColumnID AS FieldID,ExportHeaderName AS Heading  FROM 
                    ExportTemplateItem WHERE ExportTemplateID=" + ddlTemplate.SelectedValue + " ORDER BY ColumnIndex");

            foreach (DataRow dr in dtColumns.Rows)
            {
                ListItem liTemp = new ListItem(dr["Heading"].ToString(), dr["FieldID"].ToString());
                liTemp.Selected = true;
                //liTemp.Attributes.Add("DataValue", dr["SystemName"].ToString());
                chklstFields.Items.Add(liTemp);
            }
            hlExportTemplate.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&SearchCriteriaET=" + Cryptography.Encrypt("-1") + "&ExportTemplateID=" + Cryptography.Encrypt(ddlTemplate.SelectedValue) + "&fixedbackurl=" + Cryptography.Encrypt(Request.RawUrl);

        }

        if (IsPostBack)
            mpeExportRecords.Show();
    }



    protected void PopulateDateAddedSearch()
    {


        if (txtDateFrom.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtDateFrom.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                _dtDateFrom = dtTemp;
            }
        }




        if (txtDateTo.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtDateTo.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                _dtDateTo = dtTemp;
                _dtDateTo = _dtDateTo.Value.AddHours(23).AddMinutes(59);
            }
        }

    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {

        Session["SCid" + hfViewID.Value] = null;
        ViewState["_iSearchCriteriaID"] = null;

        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        ddlEnteredBy.SelectedIndex = 0;
        //chkIsActive.Checked = false;
        //chkShowOnlyWarning.Checked = false;

        _strNumericSearch = "";
        TextSearch = "";

        if (_theView != null)
        {
            _bReset = true;
        }

        //chk if reset button is raised
        LinkButton lnkResetSender = (LinkButton)sender;
        if (lnkResetSender != null && lnkResetSender == lnkReset)
        {
            chkIsActive.Checked = false;
            chkShowOnlyWarning.Checked = false;
        }

        if (chkShowAdvancedOptions.Checked == false)//_bDynamicSearch
        {
            chkShowOnlyWarning.Checked = false;

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            if (_bReset)
            {
                if (_theView.Filter != "" && _theView.FilterControlsInfo != "")
                {

                    xmlDoc.Load(new StringReader(_theView.FilterControlsInfo));

                }
            }

            foreach (DataRow dr in _dtDynamicSearchColumns.Rows)
            {

                if (dr["ColumnType"].ToString() == "number" || dr["ColumnType"].ToString() == "calculation")
                {

                    TextBox txtLowerLimit = (TextBox)tblSearchControls.FindControl("txtLowerLimit_" + dr["SystemName"].ToString());
                    TextBox txtUpperLimit = (TextBox)tblSearchControls.FindControl("txtUpperLimit_" + dr["SystemName"].ToString());

                    if (txtLowerLimit != null && txtUpperLimit != null)
                    {
                        txtLowerLimit.Text = "";
                        txtUpperLimit.Text = "";

                        //if (xmlDoc != null && _bReset)
                        //{
                        //    if (ViewState[txtLowerLimit.ID + dr["ColumnID"].ToString()] != null)
                        //        txtLowerLimit.Text = ViewState[txtLowerLimit.ID + dr["ColumnID"].ToString()].ToString();

                        //    if (ViewState[txtUpperLimit.ID + dr["ColumnID"].ToString()] != null)
                        //        txtUpperLimit.Text = ViewState[txtUpperLimit.ID + dr["ColumnID"].ToString()].ToString();
                        //}

                    }

                }
                else if (dr["ColumnType"].ToString() == "text")
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    if (txtSearch != null)
                    {
                        txtSearch.Text = "";

                        //if (xmlDoc != null && _bReset)
                        //{
                        //    if (ViewState[txtSearch.ID + dr["ColumnID"].ToString()] != null)
                        //        txtSearch.Text = ViewState[txtSearch.ID + dr["ColumnID"].ToString()].ToString();
                        //    //txtSearch.Text = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                        //}
                    }

                }

                else if (dr["ColumnType"].ToString() == "date")
                {
                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());
                    if (txtLowerDate != null && txtUpperDate != null)
                    {
                        txtLowerDate.Text = "";
                        txtUpperDate.Text = "";
                        //if (xmlDoc != null && _bReset)
                        //{
                        //    if (ViewState[txtLowerDate.ID + dr["ColumnID"].ToString()] != null)
                        //        txtLowerDate.Text = ViewState[txtLowerDate.ID + dr["ColumnID"].ToString()].ToString();

                        //    if (ViewState[txtUpperDate.ID + dr["ColumnID"].ToString()] != null)
                        //        txtUpperDate.Text = ViewState[txtUpperDate.ID + dr["ColumnID"].ToString()].ToString();

                        //}
                    }

                }
                else if (dr["ColumnType"].ToString() == "datetime")
                {
                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());

                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                    if (txtLowerDate != null && txtUpperDate != null && txtLowerTime != null && txtUpperTime != null)
                    {
                        txtLowerDate.Text = "";
                        txtUpperDate.Text = "";
                        txtLowerTime.Text = "";
                        txtUpperTime.Text = "";

                        //if (xmlDoc != null && _bReset)
                        //{
                        //    if (ViewState[txtLowerDate.ID + dr["ColumnID"].ToString()] != null)
                        //        txtLowerDate.Text = ViewState[txtLowerDate.ID + dr["ColumnID"].ToString()].ToString();
                        //    if (ViewState[txtUpperDate.ID + dr["ColumnID"].ToString()] != null)
                        //        txtUpperDate.Text = ViewState[txtUpperDate.ID + dr["ColumnID"].ToString()].ToString();
                        //    if (ViewState[txtLowerTime.ID + dr["ColumnID"].ToString()] != null)
                        //        txtLowerTime.Text = ViewState[txtLowerTime.ID + dr["ColumnID"].ToString()].ToString();
                        //    if (ViewState[txtUpperTime.ID + dr["ColumnID"].ToString()] != null)
                        //        txtUpperTime.Text = ViewState[txtUpperTime.ID + dr["ColumnID"].ToString()].ToString();
                        //}
                    }

                }
                else if (dr["ColumnType"].ToString() == "time")
                {
                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                    if (txtLowerTime != null && txtUpperTime != null)
                    {
                        txtLowerTime.Text = "";
                        txtUpperTime.Text = "";
                        //if (xmlDoc != null && _bReset)
                        //{
                        //    if (ViewState[txtLowerTime.ID + dr["ColumnID"].ToString()] != null)
                        //        txtLowerTime.Text = ViewState[txtLowerTime.ID + dr["ColumnID"].ToString()].ToString();
                        //    if (ViewState[txtUpperTime.ID + dr["ColumnID"].ToString()] != null)
                        //        txtUpperTime.Text = ViewState[txtUpperTime.ID + dr["ColumnID"].ToString()].ToString();
                        //}
                    }

                }

                //if (dr["ColumnType"].ToString() == "datetime")
                //{
                //    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                //    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());

                //    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                //    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                //    if (txtDate != null)
                //    {
                //        txtDate.Text = "";
                //    }

                //    TextBox txtTime = (TextBox)tblSearchControls.FindControl("txtTime_" + dr["SystemName"].ToString());
                //    if (txtTime != null)
                //    {
                //        txtTime.Text = "";
                //    }
                //}

                //else if (dr["ColumnType"].ToString() == "time")
                //{

                //    TextBox txtTime = (TextBox)tblSearchControls.FindControl("txtTime_" + dr["SystemName"].ToString());
                //    if (txtTime != null)
                //    {
                //        txtTime.Text = "";
                //        if (xmlDoc != null && _bReset)
                //        {
                //            txtTime.Text = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                //        }
                //    }
                //}

                else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                {
                    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                    if (ddlSearch != null)
                    {
                        ddlSearch.SelectedIndex = 0;
                        if (xmlDoc != null && _bReset)
                        {

                            //if (ViewState[ddlSearch.ID + dr["ColumnID"].ToString()] != null)
                            //{
                            //    if (ddlSearch.Items.FindByValue(ViewState[ddlSearch.ID + dr["ColumnID"].ToString()].ToString()) != null)
                            //        ddlSearch.SelectedValue = ViewState[ddlSearch.ID + dr["ColumnID"].ToString()].ToString();
                            //}



                        }
                    }

                }

                //else if (dr["ColumnType"].ToString() == "radiobutton" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                //{
                //    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                //    if (ddlSearch != null)
                //    {
                //        ddlSearch.SelectedIndex = 0;
                //        if (xmlDoc != null && _bReset)
                //        {

                //            if (ViewState[ddlSearch.ID + dr["ColumnID"].ToString()] != null)
                //            {
                //                if (ddlSearch.Items.FindByValue(ViewState[ddlSearch.ID + dr["ColumnID"].ToString()].ToString()) != null)
                //                    ddlSearch.SelectedValue = ViewState[ddlSearch.ID + dr["ColumnID"].ToString()].ToString();
                //            }



                //        }
                //    }

                //}

                else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "table" || dr["DropDownType"].ToString() == "tabledd") &&
           dr["TableTableID"] != DBNull.Value && dr["DisplayColumn"].ToString() != "")
                {
                    DropDownList ddlParentSearch = (DropDownList)tblSearchControls.FindControl("ddlParentSearch_" + dr["SystemName"].ToString());

                    if (ddlParentSearch != null)
                    {
                        ddlParentSearch.SelectedIndex = 0;
                        if (xmlDoc != null && _bReset)
                        {
                            //if (ViewState[ddlParentSearch.ID + dr["ColumnID"].ToString()] != null)
                            //{
                            //    if (ddlParentSearch.Items.FindByValue(ViewState[ddlParentSearch.ID + dr["ColumnID"].ToString()].ToString()) != null)
                            //        ddlParentSearch.SelectedValue = ViewState[ddlParentSearch.ID + dr["ColumnID"].ToString()].ToString();
                            //}
                        }
                    }

                }
                else if (dr["ColumnType"].ToString() == "radiobutton" || dr["ColumnType"].ToString() == "listbox"
                    || dr["ColumnType"].ToString() == "checkbox")
                {
                    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                    if (ddlSearch != null)
                    {
                        ddlSearch.SelectedIndex = 0;
                        if (xmlDoc != null && _bReset)
                        {
                            //if (ViewState[ddlSearch.ID + dr["ColumnID"].ToString()] != null)
                            //{
                            //    if (ddlSearch.Items.FindByValue(ViewState[ddlSearch.ID + dr["ColumnID"].ToString()].ToString()) != null)
                            //        ddlSearch.SelectedValue = ViewState[ddlSearch.ID + dr["ColumnID"].ToString()].ToString();
                            //}
                        }
                    }
                }
                else
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    if (txtSearch != null)
                    {
                        txtSearch.Text = "";

                        //if (xmlDoc != null && _bReset)
                        //{
                        //    if (ViewState[txtSearch.ID + dr["ColumnID"].ToString()] != null)
                        //        txtSearch.Text = ViewState[txtSearch.ID + dr["ColumnID"].ToString()].ToString();
                        //}
                    }
                }
            }
            //
            foreach (DataRow dr in _dtSearchGroup.Rows)
            {

                TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SearchGroupID"].ToString());
                if (txtSearch != null)
                {
                    if (txtSearch.Text != "")
                    {
                        txtSearch.Text = "";
                    }
                }
            }

        }
        else
        {
            cbcSearchMain.ddlYAxisV = "";
            cbcSearch1.ddlYAxisV = "";
            cbcSearch2.ddlYAxisV = "";
            cbcSearch3.ddlYAxisV = "";
            hfAndOr1.Value = "";
            hfAndOr2.Value = "";
            hfAndOr3.Value = "";

            string strJSSearchShowHide = "$('#" + trSearch1.ClientID + "').fadeOut();$('#" + trSearch2.ClientID + "').fadeOut();$('#" + trSearch3.ClientID + "').fadeOut();";

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PutDefaultSearcUI_CS", strJSSearchShowHide, true);
        }

        gvTheGrid.GridViewSortColumn = "DBGSystemRecordID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;

        if (_bReset)
        {

            if (_theView.SortOrder != "")
            {
                string strSortColumn = _theView.SortOrder.Substring(0, _theView.SortOrder.LastIndexOf(" ") + 1);
                string strDirection = _theView.SortOrder.Substring(_theView.SortOrder.LastIndexOf(" ") + 1);

                gvTheGrid.GridViewSortColumn = strSortColumn.Trim();

                if (strDirection.Trim().ToLower() == "desc")
                {
                    gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                }
                else
                {
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                }

            }

        }


        //PopulateSearchParams();

        BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected void Pager_DeleteAction(object sender, EventArgs e)
    {
        //Ticket 1013
        trUndo.Style.Add("display", "none");
        //End

        EnsureSecurity();
        bool bIsAllCheckeD = false;

        bool bHeaderChecked = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;
        string sCheck = "";
        if (bHeaderChecked)
        {
            bIsAllCheckeD = true;
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
                else
                {
                    bIsAllCheckeD = false;
                }
            }

        }
        else
        {
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
            }

        }



        if (string.IsNullOrEmpty(sCheck))
        {
            Session["tdbmsgpb"] = "Please select a record.";
            if (hfUsingScrol.Value == "yes" && _gvPager != null)
            {
                BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
            }
            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            return;
        }


        chkDelateAllEvery.Checked = false;
        lblDeleteRestoreMessage.Text = "Are you sure you want to delete selected item(s)?";
        chkDelateAllEvery.Text = "I would like to delete EVERY item in this table";
        hfParmanentDelete.Value = "no";
        lblDeleteMessageNote.Visible = true;

        chkDelateAllEvery.Checked = false;
        chkDeleteParmanent.Checked = false;
        chkUndo.Checked = false;


        if (bIsAllCheckeD)
        {
            trDeleteAllEvery.Visible = true;
        }
        else
        {
            trDeleteAllEvery.Visible = false;
        }

        //trDeleteParmanent.Visible = true;
        trUndo.Visible = true;

        if (_strRecordRightID == Common.UserRoleType.Administrator
                       || _strRecordRightID == Common.UserRoleType.GOD)
        {
        }
        else
        {
            trDeleteParmanent.Visible = false;
            trUndo.Visible = false;
        }




        if (_bDeleteReason)
        {
            trDeleteRestoreMessage.Visible = false;
            trDeleteReason.Visible = true;
        }

        txtDeleteReason.Text = "";
        mpeDeleteAll.Show();



    }


    protected void DeleteAction()
    {

        bool bAllChecked = true;
        string sCheck = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
            }
            else
            {
                bAllChecked = false;
            }
        }

        if (bAllChecked && chkDelateAllEvery.Checked == true && chkDeleteParmanent.Checked == false)
        {
            //DataTable dtAllRecordIDs;
            //if (PageType == "c")
            //{
            //    dtAllRecordIDs = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=1 AND TableID=" + _qsTableID + " " + TextSearchParent);
            //}
            //else
            //{
            //    dtAllRecordIDs = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=1 AND TableID=" + _qsTableID);
            //}

            //sCheck = "";
            //foreach (DataRow dr in dtAllRecordIDs.Rows)
            //{
            //    sCheck = sCheck + dr[0].ToString()+ ",";
            //}
            string strExtraWHERE = "";
            if (PageType == "c")
                strExtraWHERE = " " + TextSearchParent;

            if (_bDeleteReason)
            {
                Common.ExecuteText("Update [Record] SET IsActive=0,DeleteReason='" + txtDeleteReason.Text.Replace("'", "''") + "',LastUpdatedUserID=" + _ObjUser.UserID + " WHERE IsActive=1 AND TableID=" + _qsTableID + strExtraWHERE);
            }
            else
            {
                Common.ExecuteText("Update [Record] SET IsActive=0,LastUpdatedUserID=" + _ObjUser.UserID + " WHERE IsActive=1 AND TableID=" + _qsTableID + strExtraWHERE);
            }
        }
        else
        {
            if (string.IsNullOrEmpty(sCheck))
            {
                Session["tdbmsgpb"] = "Please select a record.";
                //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            }
            else
            {
                DeleteItem(sCheck);

            }

        }



        BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
        _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
        if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
        {
            BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
        }


    }


    protected void DeleteParmanentAction()
    {

        string sCheck = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
            }
        }


        //if (chkDelateAllEvery.Checked == true)
        //{
        //    DataTable dtAllRecordIDs;

        //    if (PageType == "c")
        //    {
        //        dtAllRecordIDs = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=0 AND TableID=" + _qsTableID + " " + TextSearchParent);
        //    }
        //    else
        //    {

        //        dtAllRecordIDs = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=0 AND TableID=" + _qsTableID);
        //    }

        //    sCheck = "";
        //    foreach (DataRow dr in dtAllRecordIDs.Rows)
        //    {
        //        sCheck = sCheck + dr[0].ToString() + ",";
        //    }
        //}



        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
        }
        else
        {
            if (chkDelateAllEvery.Checked == true)
            {
                if (PageType == "c")
                {
                    Common.ExecuteText("DELETE Record WHERE IsActive=0 AND TableID=" + _qsTableID + " " + TextSearchParent);
                }
                else
                {

                    Common.ExecuteText("DELETE Record WHERE IsActive=0 AND TableID=" + _qsTableID);
                }
            }
            else
            {
                DeleteParmanentItem(sCheck);
            }


        }

        BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
        _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
        if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
        {
            BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
        }


    }


    //protected void Pager_ShowCog(object sender, EventArgs e)
    //{
    //    mpeCog.Show();
    //}
    protected void SetOtherValidationGroup()
    {
        //delete related

        TheDatabase.SetValidationGroup(pnlDeleteAll.Controls, "DE" + _strDynamictabPart);
        //edit many related
        TheDatabase.SetValidationGroup(pnlEditMany.Controls, "EM" + _strDynamictabPart);


        //delete related
        //rfvDeleteReason.ValidationGroup = "DR";
        //lnkDeleteAllOK.ValidationGroup = "DR";
    }
    protected void Pager_AllExport(object sender, EventArgs e)
    {
        EnsureSecurity();
        if (ddlTemplate.Items.Count == 0)
        {
            //no template, so lets create one

            ExportManager.CreateDefaultExportTemplate(TableID);
            PopulateExportTemplate(TableID);

            ddlTemplate_SelectedIndexChanged(null, null);


        }


        mpeExportRecords.Show();
    }

    //protected void lnkCogOK_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txtPageSize.Text != "")
    //        {
    //            gvTheGrid.PageSize = int.Parse(txtPageSize.Text);
    //            BindTheGrid(0, gvTheGrid.PageSize);
    //        }

    //    }
    //    catch
    //    {

    //    }

    //    mpeCog.Hide();
    //}

    protected void lnkExportRecords_Click(object sender, EventArgs e)
    {
        try
        {
            mpeExportRecords.Hide();
            //UpdatePanel1.Update();
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "hide_AllExpoerMPE", "$find('" + mpeExportRecords.BehaviorID + "').hide();", true);


            lblExportRecords.Text = "";
            PopulateSearchParams();
            switch (ddlExportFiletype.SelectedValue)
            {
                case "e":
                    Pager_OnExportForExcel(null, null);
                    break;
                case "c":
                    Pager_OnExportForCSV(null, null);
                    break;
                case "w":

                    if (_gvPager != null)
                        _gvPager.ExportWord();

                    break;
                case "p":
                    if (_gvPager != null)
                        _gvPager.ExportPDF();

                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            //
            if (ex.Message.IndexOf("Thread was being aborted") == -1)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Record export", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

        }

        mpeExportRecords.Hide();
    }
    protected void lnkEditManyCancel2_Click(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    protected void lnkUntickAllExport_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in chklstFields.Items)
        {
            item.Selected = false;
        }


        if (IsPostBack)
            mpeExportRecords.Show();
    }
    protected void lnkEditManyOK_Click(object sender, EventArgs e)
    {
        lblMsgBullk.Text = "";
        string strValue = "";
        if (ddlYAxisBulk.SelectedValue == "")
        {
            lblMsgBullk.Text = "Please select a column.";
            mpeEditMany.Show();
            return;
        }
        else
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlYAxisBulk.SelectedValue));

            if (theColumn.ColumnType == "checkbox")
            {
                strValue = Common.GetCheckBoxValue(theColumn.DropdownValues, ref chkCheckboxBulk);
            }
            else if (theColumn.ColumnType == "number")
            {
                strValue = txtNumberBulk.Text;
            }
            else if (theColumn.ColumnType == "text")
            {
                strValue = txtTextBulk.Text;
            }
            else if (theColumn.ColumnType == "date")
            {
                strValue = txtDateBulk.Text;
            }
            else if (theColumn.ColumnType == "datetime")
            {
                try
                {
                    string strDateTime = "";
                    if (txtDateBulk.Text.Trim() == "")
                    {
                        //strDateTime = DateTime.Now.ToShortDateString() + " 12:00:00 AM";
                    }
                    else
                    {
                        DateTime dtTemp;
                        if (DateTime.TryParseExact(txtDateBulk.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                        {
                            txtDateBulk.Text = dtTemp.ToShortDateString();
                        }
                        string strTimePart = "";
                        if (txtBulkTime != null)
                        {
                            if (txtBulkTime.Text == "")
                            {
                                strTimePart = "00:00";
                            }
                            else
                            {
                                if (txtBulkTime.Text.ToLower().IndexOf(":am") > 0)
                                {
                                    strTimePart = txtBulkTime.Text.ToLower().Replace(":am", ":00");
                                }
                                else
                                {
                                    strTimePart = txtBulkTime.Text.ToLower().Replace(":pm", ":00");
                                }
                            }
                        }
                        else
                        {
                            strTimePart = "00:00";
                        }

                        strDateTime = txtDateBulk.Text + " " + strTimePart;
                        strDateTime = strDateTime.Replace("  ", " ");
                    }
                    strValue = strDateTime;
                }
                catch
                {
                    //
                    strValue = "";
                }
            }
            else if (theColumn.ColumnType == "dropdown")
            {
                strValue = ddlDropdownBulk.SelectedValue;
            }

            if (strValue == "")
            {
                lblMsgBullk.Text = "Please enter the new value.";
                mpeEditMany.Show();
                return;
            }

            string sCheck = "";

            bool bAll = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;


            if (chkUpdateEveryItem.Checked && bAll)
            {
                //DataTable dtTable
                //Common.ExecuteText("UPDATE Record SET " + theColumn.SystemName + "='" + strValue.Replace("'", "''") 
                //    + "' WHERE TableID=" + theColumn.TableID.ToString());

                gvTheGrid.PageIndex = 0;// why???
                //BindTheGridForExport(0, _gvPager.TotalRows);
                int iOldPS = gvTheGrid.PageSize;
                gvTheGrid.PageSize = _gvPager.TotalRows + 1;
                BindTheGrid(0, _gvPager.TotalRows);
                for (int i = 0; i < gvTheGrid.Rows.Count; i++)
                {

                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";

                }
                sCheck = sCheck + "-1";
                gvTheGrid.PageSize = iOldPS;
            }
            else
            {
                for (int i = 0; i < gvTheGrid.Rows.Count; i++)
                {
                    bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                    if (ischeck)
                    {
                        sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                    }
                }
                sCheck = sCheck + "-1";

            }
            DateTime dtRightNow = DateTime.Now;
            RecordManager.Record_Audit(null, sCheck, false, dtRightNow);
            Common.ExecuteText("UPDATE Record SET " + theColumn.SystemName + "='" + strValue.Replace("'", "''") + "' WHERE RecordID IN (" + sCheck + ")");
            DataTable dtRecords = Common.DataTableFromText(@"SELECT " + theColumn.SystemName + @",RecordID,WarningResults,ValidationResults FROM Record
	                    WHERE  RecordID IN (" + sCheck + @")");

            string strInvalidRecordIDs = "";
            string strValidRecordIDs = "";
            string strSQL = @"SELECT " + theColumn.SystemName + @",RecordID,WarningResults,ValidationResults FROM Record
	                    WHERE  RecordID IN (" + sCheck + @")";
            RecordManager.ets_AdjustValidFormulaChanges(theColumn, ref strInvalidRecordIDs, ref strValidRecordIDs, true, strSQL);

            RecordManager.Record_Audit(null, sCheck, true, dtRightNow);

            if (strInvalidRecordIDs != "")
            {
                Session["tdbmsgpb"] = "Total invalid records:" + (strInvalidRecordIDs.Split(',').Length - 1).ToString();
            }

            lnkSearch_Click(null, null);
            mpeEditMany.Hide();

        }



    }
    protected void lnkDeleteAllOK_Click(object sender, EventArgs e)
    {

        if (chkIsActive.Checked && hfParmanentDelete.Value == "no")
        {
            UnDeleteAction();
        }

        if (chkIsActive.Checked && hfParmanentDelete.Value == "yes")
        {
            if (chkUndo.Checked)
            {
                DeleteParmanentAction();
            }
            else
            {
                if (chkIsActive.Checked)
                {
                    if (_gvPager != null)
                    {
                        _gvPager.HideDelete = true;
                        _gvPager.HideUnDelete = false;
                        if (_strRecordRightID == Common.UserRoleType.Administrator
                                || _strRecordRightID == Common.UserRoleType.GOD)
                        {

                            _gvPager.HideParmanentDelete = false;

                        }
                        ShowHidePermanentDelete();
                        ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alertPD", "alert('I will not be able to undo this action checkbox must be ticked to delete PERMANENTLY.');", true);
                    }

                }

            }
        }

        if (chkIsActive.Checked == false)
        {
            DeleteAction();
        }

        mpeDeleteAll.Hide();
    }

    protected void ddlYAxisBulk_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtNumberBulk.Visible = false;
        txtNumberBulk.Text = "";
        txtTextBulk.Visible = false;
        txtTextBulk.Text = "";
        txtDateBulk.Visible = false;
        txtDateBulk.Text = "";
        ibBulkDate.Visible = false;
        txtBulkTime.Visible = false;
        txtBulkTime.Text = "";
        ddlDropdownBulk.Visible = false;
        chkCheckboxBulk.Visible = false;
        chkCheckboxBulk.Checked = false;

        if (ddlDropdownBulk.Items.Count > 0)
            ddlDropdownBulk.SelectedIndex = 0;

        if (ddlYAxisBulk.SelectedValue == "")
        {
            //
        }
        else
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlYAxisBulk.SelectedValue));

            if (theColumn.ColumnType == "checkbox")
            {
                chkCheckboxBulk.Visible = true;
            }
            else if (theColumn.ColumnType == "number")
            {
                txtNumberBulk.Visible = true;

            }
            else if (theColumn.ColumnType == "text")
            {
                txtTextBulk.Visible = true;

            }
            else if (theColumn.ColumnType == "date")
            {
                txtDateBulk.Visible = true;
                ibBulkDate.Visible = true;

            }
            else if (theColumn.ColumnType == "datetime")
            {
                txtDateBulk.Visible = true;
                ibBulkDate.Visible = true;
                txtBulkTime.Visible = true;

            }
            else if (theColumn.ColumnType == "dropdown")
            {

                ddlDropdownBulk.Visible = true;

                if (theColumn.DropDownType == "values")
                {
                    Common.PutDDLValues(theColumn.DropdownValues, ref ddlDropdownBulk);
                }
                else if (theColumn.DropDownType == "value_text")
                {
                    Common.PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownBulk);
                }
                else if ((theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd")
                    && theColumn.ParentColumnID == null)
                {
                    ddlDropdownBulk.Items.Clear();
                    RecordManager.PopulateTableDropDown((int)theColumn.ColumnID, ref ddlDropdownBulk);
                    // PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownBulk);
                }
                else
                {
                    ddlDropdownBulk.Items.Clear();
                }

            }
        }

        bool bAll = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;

        if (bAll)
        {
            trUpdateEveryItem.Visible = true;
        }
        else
        {
            trUpdateEveryItem.Visible = false;
        }

        mpeEditMany.Show();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mpeAddRecord.Show();
    }
    protected void lnkDeleteAllNo_Click(object sender, EventArgs e)
    {
        if (hfUsingScrol.Value == "yes")
        {
            lnkSearch_Click(null, null);
        }
        else
        {

            if (chkIsActive.Checked)
            {
                if (_gvPager != null)
                {
                    _gvPager.HideDelete = true;
                    _gvPager.HideUnDelete = false;
                    if (_strRecordRightID == Common.UserRoleType.Administrator
                            || _strRecordRightID == Common.UserRoleType.GOD)
                    {

                        _gvPager.HideParmanentDelete = false;

                    }
                    ShowHidePermanentDelete();
                }

            }
        }


        chkDelateAllEvery.Checked = false;
        mpeDeleteAll.Hide();
    }

    protected void Pager_CopyRecordAction(object sender, EventArgs e)
    {
        EnsureSecurity();
        string sCheck = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text;
                break;
            }
        }


        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select (tick) a record you want to copy.');", true);
            return;
        }
        else
        {
            //record found
            string strCopyAddURL = GetAddURL();// +"&CopyRecordID=" + Cryptography.Encrypt(sCheck);

            Session["CopyRecordID"] = Cryptography.Encrypt(sCheck);

            if (_bOpenInParent == false)
            {
                Response.Redirect(strCopyAddURL, false);
            }
            else
            {
                //this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "myUniqueKey",
                //    "self.parent.location='" + GetAddURL() + "&CopyRecordID=" + Cryptography.Encrypt(sCheck) + "';", true);

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "strCopyAddURL", "window.parent.location.href='" + strCopyAddURL + "';", true);

            }
        }
    }
    protected void Pager_EditManyAction(object sender, EventArgs e)
    {
        EnsureSecurity();
        string sCheck = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
            }
        }

        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select (tick) the records you want to change.');", true);
            return;
        }


        string strBulkUpdateSQL = SystemData.SystemOption_ValueByKey_Account("BulkUpdateSQL", null, int.Parse(TableID.ToString()));

        //string sCheck = "";
        if (strBulkUpdateSQL != "")
        {
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    string strRecordID = ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text;
                    try
                    {
                        Common.ExecuteText(strBulkUpdateSQL.Replace("@RecordID", strRecordID));
                    }
                    catch
                    {
                        //
                    }

                }
            }
            lnkSearch_Click(null, null);


            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Records have been updated.');", true);

            return;

        }



        bool bAll = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;

        if (bAll)
        {
            trUpdateEveryItem.Visible = true;
        }
        else
        {
            trUpdateEveryItem.Visible = false;
        }

        mpeEditMany.Show();


    }

    protected void Pager_OnSendEmailAction(object sender, EventArgs e)
    {
        EnsureSecurity();
        string sCheck = "";

        bool bHeaderChecked = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;

        if (bHeaderChecked == false)
        {
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
            }
        }
        else
        {
            int iTN = 0;
            string strReturnSQL = "";
            DataTable dtTemp = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                       ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                       !chkIsActive.Checked,
                       chkShowOnlyWarning.Checked == false ? null : (bool?)true,
                       null, null,
                         "DBGSystemRecordID", "DESC", 0, _gvPager.TotalRows, ref iTN, ref _iTotalDynamicColumns,
                         _strListType, _strNumericSearch, TextSearch + TextSearchParent,
                       _dtDateFrom, _dtDateTo, "", "", "", int.Parse(hfViewID.Value), ref strReturnSQL, ref strReturnSQL);

            foreach (DataRow drR in dtTemp.Rows)
            {
                sCheck = sCheck + drR["DBGSystemRecordID"].ToString() + ",";
            }
        }


        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            return;
        }

        string strSC = "";

        if (PageType == "p")
        {
            strSC = "&SearchCriteria=" + Cryptography.Encrypt(SearchCriteriaID.ToString());
        }
        else
        {
            if (Request.QueryString["SearchCriteria"] != null)
            {
                strSC = "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
        }


        string xml = null;
        xml = @"<root>" +
              " <recordids>" + HttpUtility.HtmlEncode(sCheck) + "</recordids>" +
                "</root>";

        SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
        int iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
        string strSendEmailURl = "";
        if (PageType == "p")
        {
            //Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/SendEmail.aspx?TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&recordids=" + Cryptography.Encrypt(iSearchCriteriaID.ToString()) + strSC, false);
            strSendEmailURl = "/Pages/Record/SendEmail.aspx?TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&recordids=" + Cryptography.Encrypt(iSearchCriteriaID.ToString()) + strSC;
        }
        else
        {
            if (Session["stackURL"] != null)
            {
                Stack<string> stack = (Stack<string>)Session["stackURL"];
                if (stack.Count > 0)
                {
                    stack.Pop();
                    Session["stackURL"] = stack;
                }
            }
            strSendEmailURl = "/Pages/Record/SendEmail.aspx?TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&recordids=" + Cryptography.Encrypt(iSearchCriteriaID.ToString()) + strSC + "&fixedurl=" + Cryptography.Encrypt(Request.RawUrl) + "&tabindex=" + DetailTabIndex.ToString();
            //Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/SendEmail.aspx?TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&recordids=" + Cryptography.Encrypt(iSearchCriteriaID.ToString()) + strSC + "&fixedurl=" + Cryptography.Encrypt(Request.RawUrl) + "&tabindex=" + DetailTabIndex.ToString(), false);

        }

        if (strSendEmailURl != "")
        {
            strSendEmailURl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + strSendEmailURl;
            if (_bOpenInParent == false)
            {
                Response.Redirect(strSendEmailURl, false);
            }
            else
            {
                strSendEmailURl = strSendEmailURl + "&fixedurl=" + Cryptography.Encrypt("~/Default.aspx?a=1");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "strSendEmailURl", "window.parent.location.href='" + strSendEmailURl + "';", true);

            }
        }

    }
    protected void Pager_UnDeleteAction(object sender, EventArgs e)
    {
        EnsureSecurity();
        bool bIsAllCheckeD = false;

        bool bHeaderChecked = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;
        string sCheck = "";
        if (bHeaderChecked)
        {
            bIsAllCheckeD = true;
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
                else
                {
                    bIsAllCheckeD = false;
                }
            }

        }
        else
        {
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
            }

        }

        if (bIsAllCheckeD)
        {
            trDeleteAllEvery.Visible = true;
        }
        else
        {
            trDeleteAllEvery.Visible = false;
        }

        if (_gvPager != null)
        {
            _gvPager.HideDelete = true;
            _gvPager.HideUnDelete = false;
            if (_strRecordRightID == Common.UserRoleType.Administrator
                       || _strRecordRightID == Common.UserRoleType.GOD)
            {
                if (chkIsActive.Checked)
                {
                    _gvPager.HideParmanentDelete = false;
                }
            }
            ShowHidePermanentDelete();
        }

        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            return;
        }


        chkDelateAllEvery.Checked = false;

        lblDeleteRestoreMessage.Text = "Are you sure you want to restore selected item(s)?";
        chkDelateAllEvery.Text = "I would like to restore EVERY item in this table";
        lblDeleteMessageNote.Visible = false;

        hfParmanentDelete.Value = "no";


        chkDelateAllEvery.Checked = false;
        chkDeleteParmanent.Checked = false;
        chkUndo.Checked = false;

        trDeleteParmanent.Visible = false;
        trUndo.Visible = false;

        if (_strRecordRightID == Common.UserRoleType.Administrator
                       || _strRecordRightID == Common.UserRoleType.GOD)
        {
        }
        else
        {
            trDeleteParmanent.Visible = false;
            trUndo.Visible = false;
        }


        trDeleteRestoreMessage.Visible = true;
        trDeleteReason.Visible = false;

        mpeDeleteAll.Show();


    }




    protected void Pager_OnParmanenetDelAction(object sender, EventArgs e)
    {

        bool bIsAllCheckeD = false;

        bool bHeaderChecked = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;
        string sCheck = "";
        if (bHeaderChecked)
        {
            //Ticket 1013
            trUndo.Style.Add("display", "table-row");
            //End

            bIsAllCheckeD = true;
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
                else
                {
                    bIsAllCheckeD = false;
                }
            }

        }
        else
        {
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
            }
        }



        if (_gvPager != null)
        {
            _gvPager.HideDelete = true;
            _gvPager.HideUnDelete = false;
            _gvPager.HideParmanentDelete = false;

            ShowHidePermanentDelete();
        }

        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            return;
        }

        trDeleteAllEvery.Visible = true;
        chkDelateAllEvery.Checked = false;

        lblDeleteRestoreMessage.Text = "Are you sure you want to PERMANENTLY delete the selected item(s)?";
        chkDelateAllEvery.Text = "I would like to PERMANENTLY delete EVERY item in this table";

        hfParmanentDelete.Value = "yes";

        chkDelateAllEvery.Checked = false;
        chkDeleteParmanent.Checked = false;
        chkUndo.Checked = false;


        trDeleteParmanent.Visible = false;
        trUndo.Visible = true;
        if (bIsAllCheckeD)
        {
            trDeleteAllEvery.Visible = true;
        }
        else
        {
            trDeleteAllEvery.Visible = false;
        }


        if (_strRecordRightID == Common.UserRoleType.Administrator
                       || _strRecordRightID == Common.UserRoleType.GOD)
        {
        }
        else
        {
            trDeleteParmanent.Visible = false;
            trUndo.Visible = false;
        }

        mpeDeleteAll.Show();

    }

    protected void chkShowAdvancedOptions_OnCheckedChanged(Object sender, EventArgs args)
    {
        lnkReset_Click(null, null);
    }

    protected void chkDeleteParmanent_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkDeleteParmanent.Checked)
        {
            trUndo.Visible = false;
        }
        else
        {
            trUndo.Visible = true;
        }

        mpeDeleteAll.Show();
    }
    protected void UnDeleteAction()
    {

        bool bAllChecked = true;
        string sCheck = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
            }
            else
            {
                bAllChecked = false;
            }
        }

        if (bAllChecked && chkDelateAllEvery.Checked == true)
        {
            DataTable dtAllRecordIDs;

            if (PageType == "c")
            {
                dtAllRecordIDs = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=0 AND TableID=" + _qsTableID + " " + TextSearchParent);
            }
            else
            {
                dtAllRecordIDs = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=0 AND TableID=" + _qsTableID);
            }

            sCheck = "";
            foreach (DataRow dr in dtAllRecordIDs.Rows)
            {
                sCheck = sCheck + dr[0].ToString() + ",";
            }

            //string strExtraWHERE = "";
            //if (PageType == "c")
            //    strExtraWHERE = " " + TextSearchParent;
        }

        if (string.IsNullOrEmpty(sCheck))
        {
            // ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            Session["tdbmsgpb"] = "Please select a record.";
        }
        else
        {
            RestoreRecords(sCheck);

        }


        BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
        _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
        if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
        {
            BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
        }

    }



    private void DeleteItem(string keys)
    {
        try
        {
            if (!string.IsNullOrEmpty(keys) && chkDeleteParmanent.Checked && chkUndo.Checked)
            {
                keys = keys + "-1";

                Common.ExecuteText("DELETE [Record] WHERE RecordID IN(" + keys + ")");
                return;
            }


            if (!string.IsNullOrEmpty(keys))
            {
                keys = keys + "-1";



                if (_bDeleteReason)
                {
                    Common.ExecuteText("Update [Record] SET IsActive=0,DeleteReason='" + txtDeleteReason.Text.Replace("'", "''") + "',LastUpdatedUserID=" + _ObjUser.UserID + " WHERE RecordID in (" + keys + ")");
                }
                else
                {
                    Common.ExecuteText("Update [Record] SET IsActive=0,LastUpdatedUserID=" + _ObjUser.UserID + " WHERE RecordID in (" + keys + ")");
                }

                //DataTable dtCT = Common.DataTableFromText("SELECT DISTINCT ChildTableID FROM TableChild WHERE ParentTableID=" + _qsTableID);
                //bool? bOneChildOnly = null;
                //string strOneSys = "";
                //string  strOneChildTableID = "";
                //int? iOneLinkedParentColumnID = null;
                //if (dtCT.Rows.Count == 1)
                //{
                //    bOneChildOnly = true;

                //    foreach (DataRow dr in dtCT.Rows)
                //    {
                //        //string strTextSearch = "";
                //        DataTable dtChildSys = Common.DataTableFromText("SELECT SystemName,LinkedParentColumnID FROM [Column] WHERE LinkedParentColumnID IS NOT NULL AND ColumnType='dropdown' AND   TableID=" + dr["ChildTableID"].ToString() + " AND TableTableID=" + _qsTableID);

                //        foreach (DataRow drCT in dtChildSys.Rows)
                //        {
                //            strOneSys = drCT["SystemName"].ToString();
                //            iOneLinkedParentColumnID = int.Parse(drCT["LinkedParentColumnID"].ToString());
                //            strOneChildTableID = dr["ChildTableID"].ToString();                            
                //        }

                //    }
                //}
                //else if (dtCT.Rows.Count > 1)
                //{
                //    bOneChildOnly = false;
                //}

                //if (bOneChildOnly == null)
                //{
                //    //no child records

                //    keys = keys + "-1";

                //    Common.ExecuteText("Update [Record] SET IsActive=0,LastUpdatedUserID=" + _ObjUser.UserID.ToString() + " 	WHERE RecordID IN(" + keys + ")");
                //    return;
                //}


                //foreach (string sTemp in keys.Split(','))
                //{
                //    if (!string.IsNullOrEmpty(sTemp))
                //    {
                //        //bool bHaveChilds = false;
                //        ////check if it has child records
                //        //if (bOneChildOnly != null)
                //        //{
                //        //    if ((bool)bOneChildOnly)
                //        //    {
                //        //        if (strOneSys != "" && strOneChildTableID != "" && iOneLinkedParentColumnID!=null)
                //        //        {
                //        //            Column theLinkedColumn = RecordManager.ets_Column_Details((int)iOneLinkedParentColumnID);
                //        //            Record theLikedRecord=RecordManager.ets_Record_Detail_Full(int.Parse(sTemp));

                //        //            string strLinkedValue = RecordManager.GetRecordValue(ref theLikedRecord, theLinkedColumn.SystemName);

                //        //            DataTable dtChildRecords = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=1 AND " + strOneSys + "='" + strLinkedValue.Replace("'","''") + "' AND TableID=" + strOneChildTableID);
                //        //            if (dtChildRecords.Rows.Count > 0)
                //        //            {
                //        //                bHaveChilds = true;
                //        //            }
                //        //        }
                //        //    }
                //        //    else
                //        //    {

                //        //        foreach (DataRow dr in dtCT.Rows)
                //        //        {
                //        //            //string strTextSearch = "";
                //        //            DataTable dtChildSys = Common.DataTableFromText("SELECT SystemName,LinkedParentColumnID FROM [Column] WHERE LinkedParentColumnID IS NOT NULL AND ColumnType='dropdown' AND   TableID=" + dr["ChildTableID"].ToString() + " AND TableTableID=" + _qsTableID);

                //        //            foreach (DataRow drCT in dtChildSys.Rows)
                //        //            {
                //        //                Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(drCT["LinkedParentColumnID"].ToString()));
                //        //                Record theLikedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(sTemp));

                //        //                string strLinkedValue = RecordManager.GetRecordValue(ref theLikedRecord, theLinkedColumn.SystemName);


                //        //                DataTable dtChildRecords = Common.DataTableFromText("SELECT RecordID FROM Record WHERE IsActive=1 AND " + drCT["SystemName"].ToString() + "='" + strLinkedValue.Replace("'", "''") + "' AND TableID=" + dr["ChildTableID"].ToString());
                //        //                if (dtChildRecords.Rows.Count > 0)
                //        //                {
                //        //                    bHaveChilds = true;

                //        //                    break;
                //        //                }
                //        //            }

                //        //        }
                //        //    }
                //        //}



                //        //if (bHaveChilds)
                //        //{
                //        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Problem" + sTemp, "alert('This record (RecordID:" + sTemp + ") has associated child record(s), please delete those associated child record(s) first and then try again.');", true);
                //        //    return;
                //        //}
                //        //else
                //        //{
                //        //    RecordManager.ets_Record_Delete(int.Parse(sTemp), (int)_ObjUser.UserID);
                //        //}
                //        RecordManager.ets_Record_Delete(int.Parse(sTemp), (int)_ObjUser.UserID);
                //    }
                //}


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Record Delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }



    private void DeleteParmanentItem(string keys)
    {
        try
        {

            if (!string.IsNullOrEmpty(keys))
            {
                keys = keys + "-1";

                Common.ExecuteText("DELETE [Record] WHERE RecordID IN(" + keys + ")");
                return;
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Record", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }

    private void RestoreRecords(string strRecordIDs)
    {
        try
        {
            if (!string.IsNullOrEmpty(strRecordIDs))
            {

                _dtRecordColums = RecordManager.ets_Table_Columns_All(TableID);

                bool _bShowExceedances = false;
                string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", _theTable.AccountID, _theTable.TableID);

                if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
                {
                    _bShowExceedances = true;
                }

                int iDuplicateRecord = 0;
                int iTotalInvalid = 0;
                int iTotalRestoredRecords = 0;


                string strUniqueColumnIDSys = "";
                string strUniqueColumnID2Sys = "";

                if (_theTable.UniqueColumnID != null)
                    strUniqueColumnIDSys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + _theTable.UniqueColumnID.ToString());

                if (_theTable.UniqueColumnID2 != null)
                    strUniqueColumnID2Sys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + _theTable.UniqueColumnID2.ToString());



                string strTempErr = "";
                foreach (string sTemp in strRecordIDs.Split(','))
                {

                    if (!string.IsNullOrEmpty(sTemp))
                    {
                        //get the Record
                        Record aRecord = RecordManager.ets_Record_Detail_Full(int.Parse(sTemp));
                        bool bIsValid = true;

                        if (TheDatabase.IsRecordDuplicate(aRecord, strUniqueColumnIDSys, strUniqueColumnID2Sys, (int)aRecord.RecordID))
                        {
                            iDuplicateRecord = iDuplicateRecord + 1;
                            bIsValid = false;
                        }
                        DataTable dtValidWarning = null;
                        string strtempRef = "";
                        string _strInValidResults = "";
                        string _strExceedanceResults = "";
                        string _strWarningResults = "";
                        int _iWarningColumnCount = 0;
                        int _iExceedanceColumnCount = 0;

                        TheDatabase.PerformAllValidation(ref aRecord, ref dtValidWarning, false, false, _dtColumnsAll, ref strtempRef, ref _strInValidResults, _bShowExceedances,
                            ref _strExceedanceResults, (int)_theTable.AccountID, strtempRef, ref strtempRef, ref strtempRef, ref _iExceedanceColumnCount, ref _strWarningResults,
                            ref strtempRef, ref strtempRef, ref _iWarningColumnCount);

                        if (_strInValidResults != "")
                        {
                            aRecord.ValidationResults = _strExceedanceResults;
                            bIsValid = false;
                            iTotalInvalid = iTotalInvalid + 1;
                        }
                        else
                        {
                            aRecord.ValidationResults = "";
                        }
                        aRecord.WarningResults = "";
                        if (_strWarningResults.Length > 0)
                        {
                            aRecord.WarningResults = _strWarningResults.Trim();
                        }

                        if (_bShowExceedances && _strExceedanceResults.Length > 0)
                        {
                            aRecord.WarningResults = aRecord.WarningResults == "" ? _strExceedanceResults : aRecord.WarningResults + " " + _strExceedanceResults;
                        }


                        if (bIsValid)
                        {
                            aRecord.IsActive = true;
                            iTotalRestoredRecords = iTotalRestoredRecords + 1;
                        }
                        else
                        {
                            aRecord.IsActive = false;

                        }
                        aRecord.LastUpdatedUserID = _ObjUser.UserID;
                        RecordManager.ets_Record_Update(aRecord, false);
                    }
                }

                string strNotification = "";

                if (iDuplicateRecord > 0)
                    strNotification = "Total duplicate records:" + iDuplicateRecord.ToString() + ". ";
                if (iTotalInvalid > 0)
                    strNotification = strNotification + "Total invalid records:" + iTotalInvalid.ToString() + ". ";
                if (iTotalRestoredRecords > 0)
                    strNotification = strNotification + "Total restored records:" + iTotalRestoredRecords.ToString() + ".";

                if (strNotification != "")
                    Session["tdbmsgpb"] = strNotification;
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Record", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {
        ExportExcelorCSV(sender, e, "csv");

    }
    //    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    //    {

    //        //DataTable dtExportColumn = RecordManager.ets_Table_Columns_Export(int.Parse(TableID.ToString()), null, null);

    //        //if (dtExportColumn.Rows.Count == 0)
    //        //{
    //        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoExportMSG", "alert('Sorry it is not possible to export this table because none of the fields have been marked for export. Please check the table configuration and try again');", true);
    //        //    return;
    //        //}

    //        if (gvTheGrid.VirtualItemCount > Common.MaxRecordsExport)
    //        {

    //            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page),
    //                "message_alert", "alert('There are " + gvTheGrid.VirtualItemCount.ToString() + " Records, we are going to send this file to your email address.');", true);

    //            string strBulkExportPath = SystemData.SystemOption_ValueByKey("BulkExportPath");
    //            string strFileName = Guid.NewGuid().ToString() + "_" + lblTitle.Text.Replace(" ", "").ToString() + ".csv";
    //            string strFullFileName = strBulkExportPath + "\\" + strFileName;
    //            PopulateDateAddedSearch();
    //            int iIsBulkExportOK = RecordManager.ets_Record_List_BulkExport(int.Parse(TableID.ToString()),
    //                ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
    //                !chkIsActive.Checked,
    //                chkShowOnlyWarning.Checked == false ? null : (bool?)true,
    //                _dtDateFrom, _dtDateTo,
    //               strFullFileName);


    //            if (iIsBulkExportOK == 1)
    //            {
    //                //lets zip the file

    //                string filename = strFullFileName;


    //                FileStream infile = File.OpenRead(filename);
    //                byte[] buffer = new byte[infile.Length];
    //                infile.Read(buffer, 0, buffer.Length);
    //                infile.Close();

    //                //FileStream outfile = File.Create(Path.ChangeExtension(filename, "zip"));
    //                FileStream outfile = File.Create(filename + ".zip");

    //                GZipStream gzipStream = new GZipStream(outfile, CompressionMode.Compress);
    //                gzipStream.Write(buffer, 0, buffer.Length);
    //                gzipStream.Close();

    //                //now lets email this to the user.

    //                string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
    //                string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
    //                string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
    //                string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
    //                MailMessage msg = new MailMessage();
    //                msg.From = new MailAddress(strEmail);
    //                msg.Subject = lblTitle.Text + " - data file";
    //                msg.IsBodyHtml = true;

    //                string strBulkExportHTTPPath = SystemData.SystemOption_ValueByKey("BulkExportHTTPPath");

    //                string strTheBody = "<div>Please click the file to download.<a href='" + strBulkExportHTTPPath + "/" + strFileName + ".zip" + "'>" + strFileName + ".zip" + "</a></div>";

    //                msg.Body = strTheBody;
    //                msg.To.Add(_ObjUser.Email);

    //                SmtpClient smtpClient = new SmtpClient(strEmailServer);
    //                smtpClient.Timeout = 99999;
    //                smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

    //#if (!DEBUG)
    //                smtpClient.Send(msg);
    //#endif

    //                if (System.Web.HttpContext.Current.Session["AccountID"] != null)
    //                {

    //                    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
    //                }

    //            }
    //            else
    //            {
    //                //failed
    //            }

    //            return;
    //        }


    //        //gvTheGrid.AllowPaging = false;
    //        //gvTheGrid.PageIndex = 0;


    //        //BindTheGridForExport(0, _gvPager.TotalRows);



    //        StringWriter sw = new StringWriter();
    //        HtmlTextWriter hw = new HtmlTextWriter(sw);



    //        int iTN = 0;
    //        gvTheGrid.PageIndex = 0;

    //        string strOrderDirection = "DESC";
    //        string sOrder = GetDataKeyNames()[0];

    //        if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
    //        {
    //            strOrderDirection = "ASC";
    //        }
    //        sOrder = gvTheGrid.GridViewSortColumn + " ";


    //        if (sOrder.Trim() == "")
    //        {
    //            sOrder = "DBGSystemRecordID";
    //        }




    //        TextSearch = TextSearch + hfTextSearch.Value;
    //        if ((bool)_theUserRole.IsAdvancedSecurity)
    //        {
    //            if (_strRecordRightID == Common.UserRoleType.OwnData)
    //            {
    //                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
    //            }
    //        }
    //        else
    //        {
    //            if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
    //            {
    //                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
    //            }
    //        }
    //        PopulateDateAddedSearch();

    //        if (chkShowAdvancedOptions.Checked && ddlUploadedBatch.SelectedValue != "")
    //        {
    //            TextSearch = TextSearch + "  AND TempRecordID IN  (SELECT RecordID FROM TempRecord WHERE BatchID=" + ddlUploadedBatch.SelectedValue + ")";
    //        }

    //        string strHeaderXML = "";
    //        if (rdbRecords.SelectedValue == "a")
    //        {
    //            TextSearch = "";
    //            _strNumericSearch = "";
    //            _dtDateFrom = null;
    //            _dtDateTo = null;
    //        }
    //        if (rdbRecords.SelectedValue == "t")
    //        {
    //            TextSearch = "";
    //            _strNumericSearch = "";
    //            _dtDateFrom = null;
    //            _dtDateTo = null;


    //            string sCheck = "";
    //            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
    //            {
    //                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
    //                if (ischeck)
    //                {
    //                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
    //                }

    //            }

    //            if (string.IsNullOrEmpty(sCheck))
    //            {
    //                ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
    //                return;
    //            }

    //            sCheck = sCheck + "-1";
    //            TextSearch = " AND RecordID IN(" + sCheck + ")";

    //        }


    //        Response.Clear();
    //        Response.Buffer = true;
    //        Response.AddHeader("content-disposition",
    //        "attachment;filename=\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".csv\"");
    //        Response.Charset = "";
    //        Response.ContentType = "text/csv";


    //        if (rdbRecords.SelectedValue == "d")
    //        {

    //            try
    //            {
    //                DataTable dtDump = TheDatabaseS.spExportAllTables(TableID);
    //                //DBG.Common.ExportUtil.ExportToExcel(dtDump, "\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".xls" + "\"");
    //                int iColCountD = dtDump.Columns.Count;
    //                //for (int i = 0; i < iColCountD; i++)
    //                //{
    //                //    sw.Write(dtDump.Columns[i]);

    //                //}

    //                //sw.Write(sw.NewLine);



    //                foreach (DataRow dr in dtDump.Rows)
    //                {
    //                    for (int i = 0; i < iColCountD; i++)
    //                    {
    //                        if (!Convert.IsDBNull(dr[i]))
    //                        {
    //                            //sw.Write("\"" + dr[i].ToString().Replace("\"", "'") + "\"");
    //                            sw.Write(dr[i].ToString());
    //                        }
    //                    }

    //                    sw.Write(sw.NewLine);

    //                }

    //                sw.Close();

    //                Response.Output.Write(sw.ToString());
    //                Response.Flush();
    //                Response.End();

    //                return;
    //            }
    //            catch (Exception ex)
    //            {
    //                ErrorLog theErrorLog = new ErrorLog(null, "Dump Export", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //                SystemData.ErrorLog_Insert(theErrorLog);
    //                return;
    //            }

    //        }

    //        bool bFoundHeader = false;

    //        foreach (ListItem item in chklstFields.Items)
    //        {
    //            if (item.Selected)
    //            {
    //                bFoundHeader = true;
    //                break;
    //            }
    //        }

    //        if (bFoundHeader == false)
    //        {
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoExportMSG", "alert('Sorry it is not possible to export this table because none of the fields have been marked for export.Please select fields for export.');", true);
    //            return;
    //        }
    //        else
    //        {
    //            strHeaderXML = "<ExportXML>";
    //            foreach (ListItem item in chklstFields.Items)
    //            {
    //                if (item.Selected)
    //                {
    //                    strHeaderXML = strHeaderXML + "<Records>";

    //                    Column theColumn = RecordManager.ets_Column_Details(int.Parse(item.Value));
    //                    strHeaderXML = strHeaderXML + "<ColumnID>" + item.Value + "</ColumnID>";
    //                    strHeaderXML = strHeaderXML + "<DisplayText>" + System.Security.SecurityElement.Escape(item.Text) + "</DisplayText>";
    //                    strHeaderXML = strHeaderXML + "<SystemName>" + theColumn.SystemName + "</SystemName>";

    //                    strHeaderXML = strHeaderXML + "</Records>";

    //                }
    //            }

    //            strHeaderXML = strHeaderXML + "</ExportXML>";
    //        }


    //        _dtRecordColums = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));

    //        bool bFoundExportColumn = false;
    //        if (sOrder != "DBGSystemRecordID" && sOrder != "" && strHeaderXML != "" && ViewState["SortOrderColumnID"] != null)
    //        {
    //            DataSet ds = new DataSet();
    //            StringReader sr = new StringReader(strHeaderXML);
    //            ds.ReadXml(sr);
    //            DataTable dtHeader = ds.Tables[0];


    //            Column theSortColumn = RecordManager.ets_Column_Details(int.Parse(ViewState["SortOrderColumnID"].ToString()));

    //            if (theSortColumn != null)
    //            {
    //                for (int j = 0; j < dtHeader.Rows.Count; j++)
    //                {
    //                    for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //                    {
    //                        if (_dtRecordColums.Rows[i]["ColumnID"].ToString() == dtHeader.Rows[j]["ColumnID"].ToString()
    //                            && theSortColumn.ColumnID.ToString() == _dtRecordColums.Rows[i]["ColumnID"].ToString())
    //                        {

    //                            if (sOrder.IndexOf("CONVERT") > -1)
    //                            {
    //                                sOrder = sOrder.Replace("[" + _dtRecordColums.Rows[i]["Heading"].ToString() + "]",
    //                                    "[" + dtHeader.Rows[j]["DisplayText"].ToString() + "]");
    //                                bFoundExportColumn = true;
    //                                break;

    //                            }
    //                            else
    //                            {
    //                                sOrder = sOrder.Replace(_dtRecordColums.Rows[i]["Heading"].ToString(), dtHeader.Rows[j]["DisplayText"].ToString());
    //                                bFoundExportColumn = true;
    //                                break;
    //                            }


    //                        }
    //                    }

    //                    if (bFoundExportColumn)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }

    //        }

    //        if (bFoundExportColumn == false)
    //        {

    //            sOrder = "DBGSystemRecordID";
    //        }


    //        if (TextSearchParent == null)
    //            TextSearchParent = "";



    //        DataTable dt = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
    //                ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
    //                !chkIsActive.Checked,
    //                chkShowOnlyWarning.Checked == false ? null : (bool?)true,
    //                null, null,
    //                  sOrder, strOrderDirection, 0,null, ref iTN, ref _iTotalDynamicColumns, "export", _strNumericSearch, TextSearch + TextSearchParent,
    //                  _dtDateFrom, _dtDateTo, "", strHeaderXML, "", null);








    //        if (strHeaderXML != "")
    //        {

    //            DataSet ds = new DataSet();
    //            StringReader sr = new StringReader(strHeaderXML);
    //            ds.ReadXml(sr);
    //            DataTable dtHeader = ds.Tables[0];

    //            for (int j = 0; j < dtHeader.Rows.Count; j++)
    //            {
    //                for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //                {
    //                    if (_dtRecordColums.Rows[i]["ColumnID"].ToString() == dtHeader.Rows[j]["ColumnID"].ToString())
    //                    {
    //                        _dtRecordColums.Rows[i]["NameOnExport"] = dtHeader.Rows[j]["DisplayText"];
    //                    }
    //                }
    //            }


    //            _dtRecordColums.AcceptChanges();

    //        }





    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = 0; j < dt.Columns.Count; j++)
    //            {
    //                //if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "data_retriever")
    //                //{
    //                //    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                //    {
    //                //        DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail(int.Parse(_dtRecordColums.Rows[i]["DataRetrieverID"].ToString()), null, null);

    //                //        if (theDataRetriever.CodeSnippet != "")
    //                //        {
    //                //            foreach (DataRow drDS in dt.Rows)
    //                //            {
    //                //                if (drDS["DBGSystemRecordID"].ToString() != "")
    //                //                {
    //                //                    drDS[dt.Columns[j].ColumnName] = Common.GetValueFromSQL(theDataRetriever.CodeSnippet.Replace("#ID#",
    //                //                        drDS["DBGSystemRecordID"].ToString()));
    //                //                }
    //                //            }

    //                //        }
    //                //    }
    //                //}


    //                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "calculation")
    //                {
    //                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                    {

    //                        if (_dtRecordColums.Rows[i]["Calculation"] != DBNull.Value)
    //                        {

    //                            bool bDateCal = false;
    //                            if (_dtRecordColums.Rows[i]["TextType"] != DBNull.Value
    //                                && _dtRecordColums.Rows[i]["TextType"].ToString().ToLower() == "d")
    //                            {
    //                                bDateCal = true;
    //                            }

    //                            foreach (DataRow drDS in dt.Rows)
    //                            {
    //                                if (drDS["DBGSystemRecordID"].ToString() != "")
    //                                {

    //                                    if (bDateCal == true)
    //                                    {
    //                                        try
    //                                        {
    //                                            string strCalculation = _dtRecordColums.Rows[i]["Calculation"].ToString();
    //                                            drDS[_dtDataSource.Columns[j].ColumnName] = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, int.Parse(drDS["DBGSystemRecordID"].ToString()), _iParentRecordID,
    //                                                _dtRecordColums.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtRecordColums.Rows[i]["DateCalculationType"].ToString());
    //                                        }
    //                                        catch
    //                                        {

    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtRecordColums.Rows[i]["Calculation"].ToString());
    //                                        //drDS[dt.Columns[j].ColumnName] = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + drDS["DBGSystemRecordID"].ToString());

    //                                        drDS[dt.Columns[j].ColumnName] = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, int.Parse(drDS["DBGSystemRecordID"].ToString()), i, _iParentRecordID);
    //                                    }

    //                                    if (bDateCal == false && _dtRecordColums.Rows[i]["IsRound"] != DBNull.Value && _dtRecordColums.Rows[i]["RoundNumber"] != DBNull.Value)
    //                                    {
    //                                        try
    //                                        {

    //                                            drDS[dt.Columns[j].ColumnName] = Math.Round(double.Parse(drDS[dt.Columns[j].ColumnName].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();


    //                                        }
    //                                        catch
    //                                        {
    //                                            //
    //                                        }
    //                                    }
    //                                }
    //                            }

    //                        }

    //                    }
    //                }


    //            }
    //        }

    //        //}
    //        dt.AcceptChanges();

    //        if (chkShowAdvancedOptions.Checked == true)//_bDynamicSearch
    //        {
    //            //if (ddlYAxis.SelectedValue != "")
    //            //{
    //            //    Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlYAxis.SelectedValue));
    //            //    if (theColumn != null)
    //            //    {
    //            //        if (theColumn.ColumnType == "calculation")
    //            //        {
    //            //            if (txtSearchText.Text != "")
    //            //            {
    //            //                DataView dtView = new DataView(dt);

    //            //                dtView.RowFilter = theColumn.DisplayTextSummary + " LIKE '%" + txtSearchText.Text.Replace("'", "''") + "%'";
    //            //                dt = dtView.ToTable();
    //            //            }
    //            //        }

    //            //    }
    //            //}

    //        }


    //        DataRow drFooter = dt.NewRow();

    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = 0; j < dt.Columns.Count; j++)
    //            {
    //                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                {
    //                    if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
    //                    {
    //                        //drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = _dtDataSource.Compute("SUM([" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "])", "[" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "]<>''");
    //                        drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(dt, dt.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));

    //                    }


    //                }

    //            }

    //        }

    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = dt.Columns.Count - 1; j >= 0; j--)
    //            {
    //                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                {
    //                    if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
    //                    {
    //                        if (!Common.HaveAccess(_strRecordRightID, "1,2"))
    //                        {
    //                            dt.Columns.RemoveAt(j);
    //                        }
    //                    }


    //                }

    //            }

    //        }


    //        dt.Rows.Add(drFooter);




    //        //Round export

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //            {
    //                for (int j = 0; j < dt.Columns.Count; j++)
    //                {
    //                    //DisplayTextSummary
    //                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                    {
    //                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
    //                        {

    //                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file"
    //                                || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "image")
    //                            {
    //                                if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
    //                                {
    //                                    try
    //                                    {
    //                                        if (dr[j].ToString().Length > 37)
    //                                        {
    //                                            dr[j] = dr[j].ToString().Substring(37);

    //                                        }
    //                                    }
    //                                    catch
    //                                    {
    //                                        //
    //                                    }
    //                                }

    //                            }

    //                            if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text"
    //                                  && (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
    //                                 || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "radiobutton")
    //                                 && _dtRecordColums.Rows[i]["DropdownValues"].ToString() != "")
    //                            {
    //                                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                                {

    //                                    if (dr[j].ToString() != "")
    //                                    {
    //                                        string strText = GetTextFromValueForDD(_dtRecordColums.Rows[i]["DropdownValues"].ToString(), dr[j].ToString());
    //                                        if (strText != "")
    //                                            dr[j] = strText;
    //                                    }
    //                                }

    //                            }


    //                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "time")
    //                            {
    //                                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                                {

    //                                    if (dr[j].ToString() != "")
    //                                    {

    //                                        TimeSpan ts = TimeSpan.Parse(dr[j].ToString());
    //                                        dr[j] = ts.ToString(@"hh\:mm");
    //                                    }
    //                                }

    //                            }

    //                            //if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "data_retriever"
    //                            //    && _dtRecordColums.Rows[i]["DataRetrieverID"] != DBNull.Value)
    //                            //{
    //                            //    if (_dtRecordColums.Rows[i]["Heading"].ToString() == dt.Columns[j].ColumnName)
    //                            //    {
    //                            //        DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail(int.Parse(_dtRecordColums.Rows[i]["DataRetrieverID"].ToString()), null, null);

    //                            //        if (theDataRetriever.CodeSnippet != "")
    //                            //        {
    //                            //            dr[j] = Common.GetValueFromSQL(theDataRetriever.CodeSnippet.Replace("#ID#",
    //                            //                dr["DBGSystemRecordID"].ToString()));
    //                            //        }
    //                            //    }
    //                            //}

    //                            if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
    //                                && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
    //                                || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
    //                                 && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
    //                                && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
    //                            {
    //                                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                                {

    //                                    if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
    //                                    {
    //                                        try
    //                                        {

    //                                            Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));

    //                                            //int iTableRecordID = int.Parse(dr[j].ToString());
    //                                            string strLinkedColumnValue = dr[j].ToString();
    //                                            DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
    //                                             + _dtRecordColums.Rows[i]["TableTableID"].ToString());

    //                                            string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

    //                                            foreach (DataRow dr2 in dtTableTableSC.Rows)
    //                                            {
    //                                                strDisplayColumn = strDisplayColumn.Replace("[" + dr2["DisplayName"].ToString() + "]", "[" + dr2["SystemName"].ToString() + "]");

    //                                            }
    //                                            string sstrDisplayColumnOrg = strDisplayColumn;
    //                                            string strFilterSQL = "";
    //                                            if (theLinkedColumn.SystemName.ToLower() == "recordid")
    //                                            {
    //                                                strFilterSQL = strLinkedColumnValue;
    //                                            }
    //                                            else
    //                                            {
    //                                                strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
    //                                            }

    //                                            //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());

    //                                            DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

    //                                            if (dtTheRecord.Rows.Count > 0)
    //                                            {
    //                                                foreach (DataColumn dc in dtTheRecord.Columns)
    //                                                {
    //                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
    //                                                }
    //                                            }
    //                                            if (sstrDisplayColumnOrg != strDisplayColumn)
    //                                                dr[j] = strDisplayColumn;
    //                                        }
    //                                        catch
    //                                        {
    //                                            //
    //                                        }


    //                                    }
    //                                }

    //                            }



    //                            if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
    //                            {
    //                                if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
    //                                {
    //                                    if (dr[j].ToString() != "")
    //                                    {
    //                                        dr[j] = Math.Round(double.Parse(dr[j].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
    //                                    }
    //                                }

    //                            }
    //                        }

    //                    }

    //                    //mm:hh
    //                    if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
    //                    {

    //                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "datetime")
    //                        {
    //                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                            {
    //                                if (dr[j].ToString().Length > 15)
    //                                {
    //                                    dr[j] = dr[j].ToString().Substring(0, 16);
    //                                }
    //                            }
    //                        }

    //                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "date")
    //                        {
    //                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                            {
    //                                if (dr[j].ToString().Length > 9)
    //                                {
    //                                    dr[j] = dr[j].ToString().Substring(0, 10);
    //                                }
    //                            }
    //                        }


    //                    }

    //                }
    //            }
    //        }

    //        // First we will write the headers.

    //        int iColCount = dt.Columns.Count;



    //        for (int i = 0; i < iColCount - 2; i++)
    //        {
    //            sw.Write(dt.Columns[i]);
    //            if (i < iColCount - 3)
    //            {
    //                sw.Write(",");
    //            }

    //        }

    //        sw.Write(sw.NewLine);



    //        // Now write all the rows.


    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            for (int i = 0; i < iColCount - 2; i++)
    //            {
    //                if (!Convert.IsDBNull(dr[i]))
    //                {
    //                    sw.Write("\"" + dr[i].ToString().Replace("\"", "'") + "\"");
    //                }

    //                if (i < iColCount - 3)
    //                {
    //                    sw.Write(",");
    //                }
    //            }

    //            sw.Write(sw.NewLine);

    //        }

    //        sw.Close();

    //        Response.Output.Write(sw.ToString());
    //        Response.Flush();
    //        Response.End();
    //    }


    protected void rdbRecords_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (rdbRecords.SelectedValue == "d")
        {
            ddlExportFiletype.SelectedValue = "c";
            ddlExportFiletype.Enabled = false;
            ddlTemplate.Enabled = false;
            chklstFields.Enabled = true;
            hlExportTemplate.Visible = false;
            hlExportTemplateNew.Visible = false;

            chklstFields.Enabled = true;
            chklstFields.Items.Clear();
            DataTable dtColumns = TheDatabase.spBulkExportColumns((int)_theTable.TableID);

            if (dtColumns != null)
            {
                foreach (DataRow dr in dtColumns.Rows)
                {
                    string strColumnText = dr["TableName"].ToString() + " " + dr["ColumnDisplayName"].ToString();

                    ListItem liTemp = new ListItem(strColumnText, dr["ColumnID"].ToString());
                    liTemp.Selected = true;
                    //liTemp.Attributes.Add("DataValue", dr["SystemName"].ToString());
                    chklstFields.Items.Add(liTemp);
                }
            }
        }
        else
        {
            if (hlExportTemplate.Visible == false)
                ddlTemplate_SelectedIndexChanged(null, null);


            ddlExportFiletype.Enabled = true;
            ddlTemplate.Enabled = true;
            chklstFields.Enabled = true;
            hlExportTemplate.Visible = true;
            hlExportTemplateNew.Visible = true;
        }
        mpeExportRecords.Show();
    }


    protected void Pager_OnExportForExcel(object sender, EventArgs e)
    {
        ExportExcelorCSV(sender, e, "excel");

    }

    protected void ExportExcelorCSV(object sender, EventArgs e, string strExportType)
    {


        try
        {

            //if (gvTheGrid.VirtualItemCount > Common.MaxRecordsExport(_theTable.AccountID, _theTable.TableID))
            //{

            //    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page),
            //        "message_alert", "alert('There are " + gvTheGrid.VirtualItemCount.ToString() + " Records, we are going to send this file to your email address.');", true);

            //    string strBulkExportPath = SystemData.SystemOption_ValueByKey_Account("BulkExportPath", _theTable.AccountID, _theTable.TableID);
            //    string strFileName = Guid.NewGuid().ToString() + "_" + lblTitle.Text.Replace(" ", "").ToString() + ".csv";
            //    string strFullFileName = strBulkExportPath + "\\" + strFileName;
            //    PopulateDateAddedSearch();
            //    int iIsBulkExportOK = RecordManager.ets_Record_List_BulkExport(int.Parse(TableID.ToString()),
            //        ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
            //        !chkIsActive.Checked,
            //        chkShowOnlyWarning.Checked == false ? null : (bool?)true,
            //        _dtDateFrom, _dtDateTo,
            //       strFullFileName);


            //    if (iIsBulkExportOK == 1)
            //    {
            //        //lets zip the file

            //        string filename = strFullFileName;

            //        FileStream infile = File.OpenRead(filename);
            //        byte[] buffer = new byte[infile.Length];
            //        infile.Read(buffer, 0, buffer.Length);
            //        infile.Close();

            //        FileStream outfile = File.Create(filename + ".zip");

            //        GZipStream gzipStream = new GZipStream(outfile, CompressionMode.Compress);
            //        gzipStream.Write(buffer, 0, buffer.Length);
            //        gzipStream.Close();

            //        string strSubject = lblTitle.Text + " - data file";
            //        string strBulkExportHTTPPath = SystemData.SystemOption_ValueByKey_Account("BulkExportHTTPPath", _theTable.AccountID, _theTable.TableID);

            //        string strTheBody = "<div>Please click the file to download.<a href='" + strBulkExportHTTPPath + "/" + strFileName + ".zip" + "'>" + strFileName + ".zip" + "</a></div>";

            //        string strTo = _ObjUser.Email;
            //        string strError = "";
            //        DBGurus.SendEmail("Bulk Export", true, null, strSubject, strTheBody, "", strTo, "", "", null, null, out strError);

            //    }
            //    else
            //    {
            //        //failed
            //    }

            //    return;
            //}



            HttpContext.Current.Response.Clear();

            if (strExportType == "email")
            {
                if (_gvPager == null)
                {
                    lblMsg.Text = "There is no records to email.";
                    return;

                }
            }

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);




            int iTN = 0;
            gvTheGrid.PageIndex = 0;

            string strOrderDirection = "DESC";
            string sOrder = GetDataKeyNames()[0];

            if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
            {
                strOrderDirection = "ASC";
            }
            sOrder = gvTheGrid.GridViewSortColumn + " ";


            if (sOrder.Trim() == "")
            {
                sOrder = "DBGSystemRecordID";
            }





            string strHeaderXML = "";
            if (rdbRecords.SelectedValue == "a" && strExportType != "email")
            {
                TextSearch = "";
                _strNumericSearch = "";
                _dtDateFrom = null;
                _dtDateTo = null;
            }


            if ((rdbRecords.SelectedValue == "t" || rdbRecords.SelectedValue == "d") && strExportType != "email")
            {
                TextSearch = "";
                _strNumericSearch = "";
                _dtDateFrom = null;
                _dtDateTo = null;


                string sCheck = "";
                for (int i = 0; i < gvTheGrid.Rows.Count; i++)
                {
                    bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                    if (ischeck)
                    {
                        sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                    }

                }

                if (string.IsNullOrEmpty(sCheck) && rdbRecords.SelectedValue == "t")
                {
                    Session["tdbmsgpb"] = "Please select a record.";
                    //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
                    return;
                }


                if (rdbRecords.SelectedValue == "t")
                {
                    sCheck = sCheck + "-1";
                    TextSearch = TextSearch + " AND Record.RecordID IN(" + sCheck + ")";
                }

                if (rdbRecords.SelectedValue == "d")
                {
                    if (sCheck != "")
                    {
                        sCheck = sCheck.Substring(0, sCheck.Length - 1);
                        TextSearch = TextSearch + " AND Record.RecordID IN(" + sCheck + ")";
                    }


                }


            }

            //if (rdbRecords.SelectedValue == "d")
            //{
            //    DataTable dtDump = TheDatabaseS.spExportAllTables(TableID);
            //    DBG.Common.ExportUtil.ExportToExcel(dtDump, "\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".xls" + "\"");

            //    return;
            //}



            bool bFoundHeader = false;

            if (strExportType != "email" && rdbRecords.SelectedValue != "d")
            {

                foreach (ListItem item in chklstFields.Items)
                {
                    if (item.Selected)
                    {
                        bFoundHeader = true;
                        break;
                    }
                }

                if (bFoundHeader == false)
                {
                    Session["tdbmsgpb"] = "Sorry it is not possible to export this table because none of the fields have been marked for export.Please select fields for export.";
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoExportMSG", "alert('Sorry it is not possible to export this table because none of the fields have been marked for export.Please select fields for export.');", true);
                    return;
                }
                else
                {
                    strHeaderXML = "<ExportXML>";
                    foreach (ListItem item in chklstFields.Items)
                    {
                        if (item.Selected)
                        {
                            //oliver <begin> Ticket 1461
                            //strHeaderXML = strHeaderXML + "<Records>";
                            //oliver <end>

                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(item.Value));

                            string strParentJoinColumnName = "";
                            string strChildJoinColumnName = "";
                            string strParentTableID = "";
                            string strShowViewLink = "";
                            string strFieldsToShow = "";
                            if (theColumn.TableTableID != null && theColumn.DisplayColumn != "" &&
                                (theColumn.ColumnType == "dropdown" || theColumn.ColumnType == "listbox")) //||theColumn.ColumnType=="listbox"
                            {

                                strParentTableID = theColumn.TableTableID.ToString();

                                if (theColumn.LinkedParentColumnID != null & (int)theColumn.TableTableID != -1)
                                {
                                    Column theLinkedParentColumn = RecordManager.ets_Column_Details(((int)theColumn.LinkedParentColumnID));
                                    strParentJoinColumnName = theLinkedParentColumn.SystemName;
                                    strChildJoinColumnName = theColumn.SystemName;
                                    strFieldsToShow = RecordManager.fnReplaceDisplayColumns(theColumn.DisplayColumn, (int)theColumn.TableTableID, theColumn.ColumnID);

                                    strShowViewLink = theColumn.ShowViewLink;

                                }
                                else
                                {
                                    if ((int)theColumn.TableTableID == -1)
                                    {
                                        strFieldsToShow = theColumn.DisplayColumn;
                                    }
                                }
                            }

                            if ((theColumn.ColumnType == "datetime") && (theColumn.SystemName.ToString().ToLower() != "datetimerecorded"))
                            {
                                //oliver <begin> Ticket 1461

                                //split the date/time by creating separate export field for Date and Time

                                //Create export header for Date:
                                strHeaderXML = strHeaderXML + "<Records>";
                                strHeaderXML = strHeaderXML + "<ColumnID>" + item.Value + "</ColumnID>";
                                strHeaderXML = strHeaderXML + "<DisplayText>" + System.Security.SecurityElement.Escape(item.Text) + ",Date" + "</DisplayText>";
                                strHeaderXML = strHeaderXML + "<SystemName>" + theColumn.SystemName + "</SystemName>";
                                strHeaderXML = strHeaderXML + "<FieldsToShow>" + System.Security.SecurityElement.Escape(strFieldsToShow) + "</FieldsToShow>";
                                strHeaderXML = strHeaderXML + "<ParentTableID>" + strParentTableID + "</ParentTableID>";
                                strHeaderXML = strHeaderXML + "<ParentJoinColumnName>" + strParentJoinColumnName + "</ParentJoinColumnName>";
                                strHeaderXML = strHeaderXML + "<ChildJoinColumnName>" + strChildJoinColumnName + "</ChildJoinColumnName>";
                                strHeaderXML = strHeaderXML + "<ShowViewLink>" + strShowViewLink + "</ShowViewLink>";
                                strHeaderXML = strHeaderXML + "<ColumnType>" + theColumn.ColumnType + "</ColumnType>";
                                strHeaderXML = strHeaderXML + "</Records>";

                                //Create export header for Time:
                                strHeaderXML = strHeaderXML + "<Records>";
                                strHeaderXML = strHeaderXML + "<ColumnID>" + item.Value + "</ColumnID>";
                                strHeaderXML = strHeaderXML + "<DisplayText>" + System.Security.SecurityElement.Escape(item.Text) + ",Time" + "</DisplayText>";
                                strHeaderXML = strHeaderXML + "<SystemName>" + theColumn.SystemName + "</SystemName>";
                                strHeaderXML = strHeaderXML + "<FieldsToShow>" + System.Security.SecurityElement.Escape(strFieldsToShow) + "</FieldsToShow>";
                                strHeaderXML = strHeaderXML + "<ParentTableID>" + strParentTableID + "</ParentTableID>";
                                strHeaderXML = strHeaderXML + "<ParentJoinColumnName>" + strParentJoinColumnName + "</ParentJoinColumnName>";
                                strHeaderXML = strHeaderXML + "<ChildJoinColumnName>" + strChildJoinColumnName + "</ChildJoinColumnName>";
                                strHeaderXML = strHeaderXML + "<ShowViewLink>" + strShowViewLink + "</ShowViewLink>";
                                strHeaderXML = strHeaderXML + "<ColumnType>" + theColumn.ColumnType + "</ColumnType>";
                                strHeaderXML = strHeaderXML + "</Records>";

                                continue;

                                //oliver <end>
                            }
                            else
                            {
                                strHeaderXML = strHeaderXML + "<Records>";
                                strHeaderXML = strHeaderXML + "<ColumnID>" + item.Value + "</ColumnID>";
                                strHeaderXML = strHeaderXML + "<DisplayText>" + System.Security.SecurityElement.Escape(item.Text) + "</DisplayText>";
                                strHeaderXML = strHeaderXML + "<SystemName>" + theColumn.SystemName + "</SystemName>";
                                strHeaderXML = strHeaderXML + "<FieldsToShow>" + System.Security.SecurityElement.Escape(strFieldsToShow) + "</FieldsToShow>";
                                strHeaderXML = strHeaderXML + "<ParentTableID>" + strParentTableID + "</ParentTableID>";
                                strHeaderXML = strHeaderXML + "<ParentJoinColumnName>" + strParentJoinColumnName + "</ParentJoinColumnName>";
                                strHeaderXML = strHeaderXML + "<ChildJoinColumnName>" + strChildJoinColumnName + "</ChildJoinColumnName>";
                                strHeaderXML = strHeaderXML + "<ShowViewLink>" + strShowViewLink + "</ShowViewLink>";
                                strHeaderXML = strHeaderXML + "<ColumnType>" + theColumn.ColumnType + "</ColumnType>";
                                strHeaderXML = strHeaderXML + "</Records>";
                            }

                            //strHeaderXML = strHeaderXML + "<ColumnID>" + item.Value + "</ColumnID>";
                            //strHeaderXML = strHeaderXML + "<DisplayText>" + System.Security.SecurityElement.Escape(item.Text) + "</DisplayText>";
                            //strHeaderXML = strHeaderXML + "<SystemName>" + theColumn.SystemName + "</SystemName>";
                            //strHeaderXML = strHeaderXML + "<FieldsToShow>" + System.Security.SecurityElement.Escape(strFieldsToShow) + "</FieldsToShow>";
                            //strHeaderXML = strHeaderXML + "<ParentTableID>" + strParentTableID + "</ParentTableID>";
                            //strHeaderXML = strHeaderXML + "<ParentJoinColumnName>" + strParentJoinColumnName + "</ParentJoinColumnName>";
                            //strHeaderXML = strHeaderXML + "<ChildJoinColumnName>" + strChildJoinColumnName + "</ChildJoinColumnName>";
                            //strHeaderXML = strHeaderXML + "<ShowViewLink>" + strShowViewLink + "</ShowViewLink>";
                            //strHeaderXML = strHeaderXML + "<ColumnType>" + theColumn.ColumnType + "</ColumnType>";

                            //strHeaderXML = strHeaderXML + "</Records>";
                        }
                    }

                    strHeaderXML = strHeaderXML + "</ExportXML>";
                }


            }




            //_dtRecordColums = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));

            _dtRecordColums = RecordManager.ets_Table_Columns_All((int)_theTable.TableID);

            bool bFoundExportColumn = false;

            if (sOrder != "DBGSystemRecordID" && sOrder != "" && strHeaderXML != "" && ViewState["SortOrderColumnID"] != null && strExportType != "email")
            {
                DataSet ds = new DataSet();
                StringReader sr = new StringReader(strHeaderXML);
                ds.ReadXml(sr);
                DataTable dtHeader = ds.Tables[0];


                Column theSortColumn = RecordManager.ets_Column_Details(int.Parse(ViewState["SortOrderColumnID"].ToString()));

                if (theSortColumn != null)
                {
                    for (int j = 0; j < dtHeader.Rows.Count; j++)
                    {
                        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                        {
                            if (_dtRecordColums.Rows[i]["ColumnID"].ToString() == dtHeader.Rows[j]["ColumnID"].ToString()
                                && theSortColumn.ColumnID.ToString() == _dtRecordColums.Rows[i]["ColumnID"].ToString())
                            {

                                if (sOrder.IndexOf("CONVERT") > -1)
                                {
                                    sOrder = sOrder.Replace("[" + _dtRecordColums.Rows[i]["Heading"].ToString() + "]",
                                        "[" + dtHeader.Rows[j]["DisplayText"].ToString() + "]");
                                    bFoundExportColumn = true;
                                    break;

                                }
                                else
                                {
                                    sOrder = sOrder.Replace(_dtRecordColums.Rows[i]["Heading"].ToString(), dtHeader.Rows[j]["DisplayText"].ToString());
                                    bFoundExportColumn = true;
                                    break;
                                }


                            }
                        }

                        if (bFoundExportColumn)
                        {
                            break;
                        }
                    }
                }

            }

            if (strExportType != "email")
            {

                if (bFoundExportColumn == false)
                {

                    sOrder = "DBGSystemRecordID";
                }

                if (TextSearchParent == null)
                    TextSearchParent = "";

            }

            DataTable dt = new DataTable();

            if (strExportType == "csv" && rdbRecords.SelectedValue == "d")
                strHeaderXML = "";

            //Is it a bulk export?
            string strReturnSQL = "";
            string sReturnHeaderSQL = "";
            DataTable dtBulk = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                       ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                       !chkIsActive.Checked,
                       chkShowOnlyWarning.Checked == false ? null : (bool?)true,
                        null, null,
                         sOrder, strOrderDirection, 0, 10, ref iTN, ref _iTotalDynamicColumns, "SQLOnly", _strNumericSearch, TextSearch + TextSearchParent,
                         _dtDateFrom, _dtDateTo, "", strHeaderXML, "", null, ref strReturnSQL, ref sReturnHeaderSQL);





            if (strExportType == "csv" && rdbRecords.SelectedValue == "d")
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".csv\"");
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "text/csv";
                string strSelectedColumnIDs = "";
                foreach (ListItem item in chklstFields.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Selected)
                        {
                            strSelectedColumnIDs = strSelectedColumnIDs + item.Value + ",";
                        }
                    }
                }
                if (strSelectedColumnIDs != "")
                    strSelectedColumnIDs = strSelectedColumnIDs.Substring(0, strSelectedColumnIDs.Length - 1);

                try
                {
                    //DataTable dtDump = TheDatabaseS.spExportAllTables(TableID);

                    DataTable dtDump = TheDatabase.spBulkExportData(_theTable.TableID.ToString(), strReturnSQL, strSelectedColumnIDs);

                    int iColCountD = dtDump.Columns.Count;

                    foreach (DataRow dr in dtDump.Rows)
                    {
                        for (int i = 0; i < iColCountD; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                //sw.Write("\"" + dr[i].ToString().Replace("\"", "'") + "\"");
                                sw.Write(dr[i].ToString());
                            }
                        }

                        sw.Write(sw.NewLine);

                    }

                    sw.Close();

                    HttpContext.Current.Response.Output.Write(sw.ToString());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();

                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                    return;
                }
                catch (Exception ex)
                {
                    ErrorLog theErrorLog = new ErrorLog(null, "Dump Export", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                    SystemData.ErrorLog_Insert(theErrorLog);
                    return;
                }


            }



            if (iTN > Common.MaxRecordsExport(_theTable.AccountID, _theTable.TableID))
            {
                if (strReturnSQL != "" && sReturnHeaderSQL != "")
                {
                    OfflineTaskParameters jsonOTParam = new OfflineTaskParameters();
                    jsonOTParam.ReturnSQL = strReturnSQL;
                    jsonOTParam.ReturnHeaderSQL = sReturnHeaderSQL;
                    jsonOTParam.UniqueFileName = Guid.NewGuid().ToString();
                    jsonOTParam.FileFriendlyName = _theTable.TableName + " - Data File";
                    jsonOTParam.TableID = _theTable.TableID;
                    jsonOTParam.TableName = _theTable.TableName;
                    jsonOTParam.TotalNumberOfRecords = iTN;
                    OfflineTask newOfflineTask = new OfflineTask(null, _theTable.AccountID, _ObjUser.UserID, 3,
                        "ExportRecords", jsonOTParam.GetJSONString(), null, DateTime.Now, null, null);
                    OfflineTaskManager.dbg_OfflineTask_Insert(newOfflineTask);


                    //celan up  task
                    newOfflineTask.Parameters = jsonOTParam.UniqueFileName + ".csv";
                    newOfflineTask.ScheduledToRun = DateTime.Now.AddHours(25);
                    newOfflineTask.Processtorun = "DeleteFile";
                    OfflineTaskManager.dbg_OfflineTask_Insert(newOfflineTask);

                    Session["tdbmsgpb"] = "There are " + iTN.ToString() + " Records, we are going to send this file to your email address.";
                }
                else
                {
                    //send error email to coder
                }

                return;
            }

            //string strReturnSQL = "";
            if (strExportType == "email")
            {
                dt = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                       ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                       !chkIsActive.Checked,
                       chkShowOnlyWarning.Checked == false ? null : (bool?)true,
                       null, null,
                         sOrder, strOrderDirection, 0, _gvPager.TotalRows, ref iTN, ref _iTotalDynamicColumns,
                         _strListType, _strNumericSearch, TextSearch + TextSearchParent,
                       _dtDateFrom, _dtDateTo, "", "", "", int.Parse(hfViewID.Value), ref strReturnSQL, ref strReturnSQL);
            }
            else
            {

                dt = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
                       ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
                       !chkIsActive.Checked,
                       chkShowOnlyWarning.Checked == false ? null : (bool?)true,
                        null, null,
                         sOrder, strOrderDirection, 0, null, ref iTN, ref _iTotalDynamicColumns, "export", _strNumericSearch, TextSearch + TextSearchParent,
                         _dtDateFrom, _dtDateTo, "", strHeaderXML, "", null, ref strReturnSQL, ref strReturnSQL);

            }





            if (strHeaderXML != "" && strExportType != "email")
            {

                DataSet ds = new DataSet();
                StringReader sr = new StringReader(strHeaderXML);
                ds.ReadXml(sr);
                DataTable dtHeader = ds.Tables[0];

                for (int j = 0; j < dtHeader.Rows.Count; j++)
                {
                    for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                    {
                        if (_dtRecordColums.Rows[i]["ColumnID"].ToString() == dtHeader.Rows[j]["ColumnID"].ToString())
                        {

                            _dtRecordColums.Rows[i]["NameOnExport"] = dtHeader.Rows[j]["DisplayText"];
                        }
                    }
                }


                _dtRecordColums.AcceptChanges();

            }



            //for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            //{
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {

            //        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "calculation")
            //        {
            //            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
            //            {

            //                if (_dtRecordColums.Rows[i]["Calculation"] != DBNull.Value)
            //                {

            //                    bool bDateCal = false;
            //                    if (_dtRecordColums.Rows[i]["TextType"] != DBNull.Value
            //                        && _dtRecordColums.Rows[i]["TextType"].ToString().ToLower() == "d")
            //                    {
            //                        bDateCal = true;
            //                    }


            //                    foreach (DataRow drDS in dt.Rows)
            //                    {
            //                        if (drDS["DBGSystemRecordID"].ToString() != "")
            //                        {

            //                            try
            //                            {
            //                                if (bDateCal == true)
            //                                {
            //                                    string strCalculation = _dtColumnsAll.Rows[i]["Calculation"].ToString();
            //                                    drDS[dt.Columns[j].ColumnName] = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, int.Parse(drDS["DBGSystemRecordID"].ToString()), _iParentRecordID,
            //                                        _dtRecordColums.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtRecordColums.Rows[i]["DateCalculationType"].ToString());
            //                                }
            //                                else
            //                                {
            //                                    string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtRecordColums.Rows[i]["Calculation"].ToString());

            //                                    //drDS[dt.Columns[j].ColumnName] = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + drDS["DBGSystemRecordID"].ToString());
            //                                    drDS[dt.Columns[j].ColumnName] = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, int.Parse(drDS["DBGSystemRecordID"].ToString()), i, _iParentRecordID);


            //                                }
            //                                if (bDateCal == false && _dtRecordColums.Rows[i]["IsRound"] != DBNull.Value && _dtRecordColums.Rows[i]["RoundNumber"] != DBNull.Value)
            //                                {
            //                                    drDS[dt.Columns[j].ColumnName] = Math.Round(double.Parse(drDS[dt.Columns[j].ColumnName].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtRecordColums.Rows[i]["RoundNumber"].ToString());

            //                                }
            //                            }
            //                            catch
            //                            {
            //                                //
            //                            }
            //                        }
            //                    }

            //                }

            //            }
            //        }
            //    }
            //}

            //}
            dt.AcceptChanges();


            if (chkShowAdvancedOptions.Checked == true)//_bDynamicSearch
            {


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

                            //drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = dt.Compute("SUM([" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "])", "[" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "]<>''");
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
                        if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
                        {
                            if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                            {
                                dt.Columns.RemoveAt(j);
                            }
                        }


                    }

                }

            }




            if (strExportType == "email")
            {
                for (int j = dt.Columns.Count - 1; j >= 0; j--)
                {
                    if (dt.Columns[j].ColumnName.IndexOf("_ID**") > -1)
                    {
                        dt.Columns.RemoveAt(j);
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

                                if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text"
                                      && (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                     || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "radiobutton")
                                     && _dtRecordColums.Rows[i]["DropdownValues"].ToString() != "")
                                {
                                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                                    {

                                        if (dr[j].ToString() != "")
                                        {
                                            string strText = GetTextFromValueForDD(_dtRecordColums.Rows[i]["DropdownValues"].ToString(), dr[j].ToString());
                                            if (strText != "")
                                                dr[j] = strText;
                                        }
                                    }

                                }

                                //if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                //      && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "listbox"
                                //     && _dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                //     && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                                //{
                                //    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                                //    {

                                //        if (dr[j].ToString() != "")
                                //        {
                                //            if (dr[j].ToString().Substring(0,1)==",")
                                //                dr[j] = dr[j].ToString().Substring(1);
                                //        }
                                //    }

                                //}



                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "time")
                                {
                                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                                    {

                                        if (dr[j].ToString() != "")
                                        {

                                            TimeSpan ts = TimeSpan.Parse(dr[j].ToString());
                                            dr[j] = ts.ToString(@"hh\:mm");
                                        }
                                    }

                                }


                                //if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                //    && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                //    || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                                //     && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                //    && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                                //{
                                //    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
                                //    {

                                //        if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
                                //        {
                                //            try
                                //            {
                                //                Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));

                                //                //int iTableRecordID = int.Parse(dr[j].ToString());
                                //                string strLinkedColumnValue = dr[j].ToString();
                                //                DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
                                //                 + _dtRecordColums.Rows[i]["TableTableID"].ToString());

                                //                string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

                                //                foreach (DataRow dr2 in dtTableTableSC.Rows)
                                //                {
                                //                    strDisplayColumn = strDisplayColumn.Replace("[" + dr2["DisplayName"].ToString() + "]", "[" + dr2["SystemName"].ToString() + "]");

                                //                }
                                //                string sstrDisplayColumnOrg = strDisplayColumn;
                                //                string strFilterSQL = "";
                                //                if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                //                {
                                //                    strFilterSQL = strLinkedColumnValue;
                                //                }
                                //                else
                                //                {
                                //                    strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                                //                }

                                //                //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());
                                //                DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

                                //                if (dtTheRecord.Rows.Count > 0)
                                //                {
                                //                    foreach (DataColumn dc in dtTheRecord.Columns)
                                //                    {
                                //                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                //                    }
                                //                }
                                //                if (sstrDisplayColumnOrg != strDisplayColumn)
                                //                    dr[j] = strDisplayColumn;
                                //            }
                                //            catch
                                //            {
                                //                //
                                //            }


                                //        }
                                //    }

                                //}



                                if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
                                {
                                    if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
                                    {
                                        if (dr[j].ToString() != "")
                                        {
                                            try
                                            {
                                                if (Common.HasSymbols(dr[j].ToString()) == false)
                                                    dr[j] = Math.Round(double.Parse(Common.IgnoreSymbols(dr[j].ToString())), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtRecordColums.Rows[i]["RoundNumber"].ToString());
                                            }
                                            catch
                                            {
                                                //
                                            }
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

                        //oliver <begin> Ticket 1461
                        if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() != "datetimerecorded")
                        {
                            if (dr[j].ToString() != "")
                            {
                                DateTime chkDateTime;
                                if (DateTime.TryParse(dr[j].ToString(), out chkDateTime))
                                {
                                    string[] ColumnTypeSplit = dt.Columns[j].ColumnName.ToString().Split(',');
                                    if (ColumnTypeSplit.Length > 1)
                                    {
                                        if (ColumnTypeSplit[1].ToLower() == "date")
                                        {
                                            DateTime dtDate = Convert.ToDateTime(Convert.ToDateTime(dr[j]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                                            dr[j] = dtDate.ToShortDateString();
                                        }
                                        if (ColumnTypeSplit[1].ToLower() == "time")
                                        {
                                            TimeSpan dtTime = TimeSpan.Parse(Convert.ToDateTime(dr[j]).ToString("hh:mm", CultureInfo.InvariantCulture));
                                            dr[j] = dtTime.ToString(@"hh\:mm");
                                        }
                                    }
                                }

                            }
                        }
                        //oliver <end>

                    }
                }
            }

            if (strExportType == "excel")
            {
                dt.Columns.RemoveAt(dt.Columns.Count - 1);
                dt.Columns.RemoveAt(dt.Columns.Count - 1);

                dt.AcceptChanges();

                //oliver <begin> Ticket 1461
                DBG.Common.ExportUtil.ExportToExcel2(dt, "\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".xls" + "\"");
                //oliver <end>

                //DBG.Common.ExportUtil.ExportToExcel(dt, "\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".xls" + "\"");
            }
            if (strExportType == "csv" || strExportType == "email")
            {

                int iColCount = dt.Columns.Count;



                for (int i = 0; i < iColCount - 2; i++)
                {
                    sw.Write(dt.Columns[i]);
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

                if (strExportType == "csv")
                {

                    try
                    {

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Buffer = true;
                        HttpContext.Current.Response.AddHeader("content-disposition",
                        "attachment;filename=\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".csv\"");
                        HttpContext.Current.Response.Charset = "";
                        HttpContext.Current.Response.ContentType = "text/csv";

                        HttpContext.Current.Response.Output.Write(sw.ToString());
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                        HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                        return;
                    }
                    catch
                    {
                        //
                        return;
                    }

                }
                if (strExportType == "email")
                {
                    string strFolderPath = Server.MapPath("~\\ExportedFiles");
                    string strFileName = Guid.NewGuid().ToString() + ".csv";
                    string strFullFileName = strFolderPath + "\\" + strFileName;

                    FileStream Fs = new FileStream(strFullFileName, FileMode.Create);

                    //MemoryStream stream = new MemoryStream();
                    //StreamWriter csvWriter = new StreamWriter(stream, Encoding.UTF8);



                    //////BinaryWriter BWriter = new BinaryWriter(Fs, Encoding.ASCII);  .GetEncoding("UTF-8")
                    //Fs.Flush();
                    //BinaryWriter BWriter = new BinaryWriter(Fs,Encoding.Default);
                    ////BWriter.Flush();
                    //BWriter.Write(sw.ToString());
                    //BWriter.Close();

                    StreamWriter csvWriter = new StreamWriter(Fs, Encoding.UTF8);

                    csvWriter.Write(sw.ToString());
                    csvWriter.Close();

                    Fs.Close();
                    HttpContext.Current.Response.Redirect(Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/SendEmail.aspx?Source=" + Cryptography.Encrypt("Recordlist") + "&SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&FileName=" + Cryptography.Encrypt(strFileName), false);

                }
            }

        }
        catch (Exception ex)
        {

            //
        }




    }

    //    protected void Pager_OnExportForExcel(object sender, EventArgs e)
    //    {

    //        //DataTable dtExportColumn = RecordManager.ets_Table_Columns_Export(int.Parse(TableID.ToString()),null,null);

    //        //if (dtExportColumn.Rows.Count == 0)
    //        //{
    //        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoExportMSG", "alert('Sorry it is not possible to export this table because none of the fields have been marked for export. Please check the table configuration and try again');", true);
    //        //    return;
    //        //}


    //        if (gvTheGrid.VirtualItemCount > Common.MaxRecordsExport)
    //        {

    //            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page),
    //                "message_alert", "alert('There are " + gvTheGrid.VirtualItemCount.ToString() + " Records, we are going to send this file to your email address.');", true);

    //            string strBulkExportPath = SystemData.SystemOption_ValueByKey("BulkExportPath");
    //            string strFileName = Guid.NewGuid().ToString() + "_" + lblTitle.Text.Replace(" ", "").ToString() + ".csv";
    //            string strFullFileName = strBulkExportPath + "\\" + strFileName;
    //            PopulateDateAddedSearch();
    //            int iIsBulkExportOK = RecordManager.ets_Record_List_BulkExport(int.Parse(TableID.ToString()),
    //                ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
    //                !chkIsActive.Checked,
    //                chkShowOnlyWarning.Checked == false ? null : (bool?)true,
    //                _dtDateFrom, _dtDateTo,
    //               strFullFileName);


    //            if (iIsBulkExportOK == 1)
    //            {
    //                //lets zip the file

    //                string filename = strFullFileName;


    //                FileStream infile = File.OpenRead(filename);
    //                byte[] buffer = new byte[infile.Length];
    //                infile.Read(buffer, 0, buffer.Length);
    //                infile.Close();

    //                //FileStream outfile = File.Create(Path.ChangeExtension(filename, "zip"));
    //                FileStream outfile = File.Create(filename + ".zip");

    //                GZipStream gzipStream = new GZipStream(outfile, CompressionMode.Compress);
    //                gzipStream.Write(buffer, 0, buffer.Length);
    //                gzipStream.Close();

    //                //now lets email this to the user.

    //                string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
    //                string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
    //                string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
    //                string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
    //                MailMessage msg = new MailMessage();
    //                msg.From = new MailAddress(strEmail);
    //                msg.Subject = lblTitle.Text + " - data file";
    //                msg.IsBodyHtml = true;

    //                string strBulkExportHTTPPath = SystemData.SystemOption_ValueByKey("BulkExportHTTPPath");

    //                string strTheBody = "<div>Please click the file to download.<a href='" + strBulkExportHTTPPath + "/" + strFileName + ".zip" + "'>" + strFileName + ".zip" + "</a></div>";

    //                msg.Body = strTheBody;
    //                msg.To.Add(_ObjUser.Email);

    //                SmtpClient smtpClient = new SmtpClient(strEmailServer);
    //                smtpClient.Timeout = 99999;
    //                smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

    //#if (!DEBUG)
    //                smtpClient.Send(msg);
    //#endif


    //                if (System.Web.HttpContext.Current.Session["AccountID"] != null)
    //                {

    //                    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
    //                }

    //            }
    //            else
    //            {
    //                //failed
    //            }

    //            return;
    //        }



    //        Response.Clear();


    //        int iTN = 0;
    //        gvTheGrid.PageIndex = 0;

    //        string strOrderDirection = "DESC";
    //        string sOrder = GetDataKeyNames()[0];

    //        if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
    //        {
    //            strOrderDirection = "ASC";
    //        }
    //        sOrder = gvTheGrid.GridViewSortColumn + " ";


    //        if (sOrder.Trim() == "")
    //        {
    //            sOrder = "DBGSystemRecordID";
    //        }




    //        TextSearch = TextSearch + hfTextSearch.Value;
    //        if ((bool)_theUserRole.IsAdvancedSecurity)
    //        {
    //            if (_strRecordRightID == Common.UserRoleType.OwnData)
    //            {
    //                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
    //            }
    //        }
    //        else
    //        {
    //            if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
    //            {
    //                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
    //            }
    //        }
    //        PopulateDateAddedSearch();

    //        if (chkShowAdvancedOptions.Checked && ddlUploadedBatch.SelectedValue != "")
    //        {
    //            TextSearch = TextSearch + "  AND TempRecordID IN  (SELECT RecordID FROM TempRecord WHERE BatchID=" + ddlUploadedBatch.SelectedValue + ")";
    //        }

    //        string strHeaderXML = "";
    //        if (rdbRecords.SelectedValue == "a")
    //        {
    //            TextSearch = "";
    //            _strNumericSearch = "";
    //            _dtDateFrom = null;
    //            _dtDateTo = null;
    //        }


    //        if (rdbRecords.SelectedValue == "t")
    //        {
    //            TextSearch = "";
    //            _strNumericSearch = "";
    //            _dtDateFrom = null;
    //            _dtDateTo = null;


    //            string sCheck = "";
    //            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
    //            {
    //                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
    //                if (ischeck)
    //                {
    //                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
    //                }               

    //            }

    //            if (string.IsNullOrEmpty(sCheck))
    //            {
    //                ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
    //                return;
    //            }

    //            sCheck = sCheck + "-1";
    //            TextSearch = " AND RecordID IN(" + sCheck + ")";

    //        }

    //        //if (rdbRecords.SelectedValue == "d")
    //        //{
    //        //    DataTable dtDump = TheDatabaseS.spExportAllTables(TableID);
    //        //    DBG.Common.ExportUtil.ExportToExcel(dtDump, "\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".xls" + "\"");

    //        //    return;
    //        //}

    //        bool bFoundHeader = false;

    //        foreach (ListItem item in chklstFields.Items)
    //        {
    //            if (item.Selected)
    //            {
    //                bFoundHeader = true;
    //                break;
    //            }
    //        }

    //        if (bFoundHeader == false)
    //        {
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoExportMSG", "alert('Sorry it is not possible to export this table because none of the fields have been marked for export.Please select fields for export.');", true);
    //            return;
    //        }
    //        else
    //        {
    //            strHeaderXML = "<ExportXML>";
    //            foreach (ListItem item in chklstFields.Items)
    //            {
    //                if (item.Selected)
    //                {
    //                    strHeaderXML = strHeaderXML + "<Records>";

    //                    Column theColumn = RecordManager.ets_Column_Details(int.Parse(item.Value));

    //                    string strParentJoinColumnName = "";
    //                    string strChildJoinColumnName = "";
    //                    string strParentTableID = "";
    //                    string strShowViewLink = "0";
    //                    string strFieldsToShow = "";
    //                    if(theColumn.LinkedParentColumnID!=null && theColumn.TableTableID!=null && theColumn.DisplayColumn!="" &&
    //                        (theColumn.ColumnType == "dropdown" || theColumn.ColumnType == "listbox")) //||theColumn.ColumnType=="listbox"
    //                    {
    //                        Column theLinkedParentColumn = RecordManager.ets_Column_Details(((int)theColumn.LinkedParentColumnID));
    //                        strParentJoinColumnName = theLinkedParentColumn.SystemName;
    //                        strChildJoinColumnName = theColumn.SystemName;
    //                        strParentTableID = theColumn.TableTableID.ToString();
    //                        strFieldsToShow = RecordManager.fnReplaceDisplayColumns(theColumn.DisplayColumn, (int)theColumn.TableTableID);
    //                        if(theColumn.ShowViewLink!=null && (bool)theColumn.ShowViewLink)
    //                        {
    //                            strShowViewLink = "1";
    //                        }
    //                    }

    //                    strHeaderXML = strHeaderXML + "<ColumnID>"+item.Value+"</ColumnID>";
    //                    strHeaderXML = strHeaderXML + "<DisplayText>"+ System.Security.SecurityElement.Escape(item.Text)+"</DisplayText>";
    //                    strHeaderXML = strHeaderXML + "<SystemName>" + theColumn.SystemName + "</SystemName>";
    //                    strHeaderXML = strHeaderXML + "<FieldsToShow>" + System.Security.SecurityElement.Escape(strFieldsToShow) + "</FieldsToShow>";
    //                    strHeaderXML = strHeaderXML + "<ParentTableID>" + strParentTableID + "</ParentTableID>";
    //                    strHeaderXML = strHeaderXML + "<ParentJoinColumnName>" + strParentJoinColumnName + "</ParentJoinColumnName>";
    //                    strHeaderXML = strHeaderXML + "<ChildJoinColumnName>" + strChildJoinColumnName + "</ChildJoinColumnName>";
    //                    strHeaderXML = strHeaderXML + "<ShowViewLink>" + strShowViewLink + "</ShowViewLink>";
    //                    strHeaderXML = strHeaderXML + "<ColumnType>" + theColumn.ColumnType + "</ColumnType>";

    //                    strHeaderXML = strHeaderXML + "</Records>";

    //                }
    //            }

    //            strHeaderXML = strHeaderXML + "</ExportXML>";
    //        }


    //        _dtRecordColums = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));

    //         bool bFoundExportColumn = false;

    //        if (sOrder != "DBGSystemRecordID" && sOrder != "" && strHeaderXML != "" && ViewState["SortOrderColumnID"]!=null)
    //        {
    //            DataSet ds = new DataSet();
    //            StringReader sr = new StringReader(strHeaderXML);
    //            ds.ReadXml(sr);
    //            DataTable dtHeader = ds.Tables[0];


    //            Column theSortColumn = RecordManager.ets_Column_Details(int.Parse(ViewState["SortOrderColumnID"].ToString()));

    //            if (theSortColumn != null)
    //            {
    //                for (int j = 0; j < dtHeader.Rows.Count; j++)
    //                {
    //                    for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //                    {
    //                        if (_dtRecordColums.Rows[i]["ColumnID"].ToString() == dtHeader.Rows[j]["ColumnID"].ToString()
    //                            && theSortColumn.ColumnID.ToString() == _dtRecordColums.Rows[i]["ColumnID"].ToString())
    //                        {

    //                            if (sOrder.IndexOf("CONVERT") > -1)
    //                            {
    //                                sOrder=sOrder.Replace("[" + _dtRecordColums.Rows[i]["Heading"].ToString() + "]",
    //                                    "[" + dtHeader.Rows[j]["DisplayText"].ToString() + "]");
    //                                bFoundExportColumn = true;
    //                                break;

    //                            }
    //                            else
    //                            {
    //                               sOrder= sOrder.Replace(_dtRecordColums.Rows[i]["Heading"].ToString(), dtHeader.Rows[j]["DisplayText"].ToString());
    //                                bFoundExportColumn = true;
    //                                break;
    //                            }


    //                        }
    //                    }

    //                    if (bFoundExportColumn)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }

    //        }

    //        if (bFoundExportColumn==false)
    //        {

    //            sOrder = "DBGSystemRecordID";
    //        }

    //        if (TextSearchParent == null)
    //            TextSearchParent = "";




    //        DataTable dt = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
    //                ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
    //                !chkIsActive.Checked,
    //                chkShowOnlyWarning.Checked == false ? null : (bool?)true,
    //                 null, null,
    //                  sOrder, strOrderDirection, 0, null, ref iTN, ref _iTotalDynamicColumns, "export", _strNumericSearch, TextSearch + TextSearchParent,
    //                  _dtDateFrom, _dtDateTo, "", strHeaderXML, "", null);







    //        if (strHeaderXML != "")
    //        {

    //            DataSet ds = new DataSet();
    //            StringReader sr = new StringReader(strHeaderXML);
    //            ds.ReadXml(sr);
    //            DataTable dtHeader = ds.Tables[0];

    //            for (int j = 0; j < dtHeader.Rows.Count; j++)
    //            {
    //                for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //                {
    //                    if (_dtRecordColums.Rows[i]["ColumnID"].ToString() == dtHeader.Rows[j]["ColumnID"].ToString())
    //                    {

    //                        _dtRecordColums.Rows[i]["NameOnExport"] = dtHeader.Rows[j]["DisplayText"];
    //                    }
    //                }
    //            }


    //            _dtRecordColums.AcceptChanges();

    //        }



    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = 0; j < dt.Columns.Count; j++)
    //            {               

    //                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "calculation")
    //                {
    //                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                    {

    //                        if (_dtRecordColums.Rows[i]["Calculation"] != DBNull.Value)
    //                        {

    //                            bool bDateCal = false;
    //                            if (_dtRecordColums.Rows[i]["TextType"] != DBNull.Value
    //                                && _dtRecordColums.Rows[i]["TextType"].ToString().ToLower() == "d")
    //                            {
    //                                bDateCal = true;
    //                            }


    //                            foreach (DataRow drDS in dt.Rows)
    //                            {
    //                                if (drDS["DBGSystemRecordID"].ToString() != "")
    //                                {

    //                                    try
    //                                    {
    //                                        if (bDateCal == true)
    //                                        {
    //                                            string strCalculation = _dtRecordColums.Rows[i]["Calculation"].ToString();
    //                                            drDS[_dtDataSource.Columns[j].ColumnName] = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, int.Parse(drDS["DBGSystemRecordID"].ToString()),_iParentRecordID,
    //                                                _dtRecordColums.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtRecordColums.Rows[i]["DateCalculationType"].ToString());
    //                                        }
    //                                        else
    //                                        {
    //                                            string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtRecordColums.Rows[i]["Calculation"].ToString());

    //                                            //drDS[dt.Columns[j].ColumnName] = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + drDS["DBGSystemRecordID"].ToString());
    //                                            drDS[dt.Columns[j].ColumnName] = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, int.Parse(drDS["DBGSystemRecordID"].ToString()), i, _iParentRecordID);


    //                                        }
    //                                        if (bDateCal == false && _dtRecordColums.Rows[i]["IsRound"] != DBNull.Value && _dtRecordColums.Rows[i]["RoundNumber"] != DBNull.Value)
    //                                        {
    //                                            drDS[dt.Columns[j].ColumnName] = Math.Round(double.Parse(drDS[dt.Columns[j].ColumnName].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();

    //                                        }
    //                                    }
    //                                    catch
    //                                    {
    //                                        //
    //                                    }
    //                                }
    //                            }

    //                        }

    //                    }
    //                }
    //            }
    //        }

    //        //}
    //        dt.AcceptChanges();


    //        if (chkShowAdvancedOptions.Checked == true)//_bDynamicSearch
    //        {


    //        }



    //        DataRow drFooter = dt.NewRow();

    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = 0; j < dt.Columns.Count; j++)
    //            {
    //                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                {
    //                    if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
    //                    {

    //                        //drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = dt.Compute("SUM([" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "])", "[" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "]<>''");
    //                        drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(dt, dt.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));

    //                    }
    //                }

    //            }

    //        }


    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = dt.Columns.Count - 1; j >= 0; j--)
    //            {
    //                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                {
    //                    if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
    //                    {
    //                        if (!Common.HaveAccess(_strRecordRightID, "1,2"))
    //                        {
    //                            dt.Columns.RemoveAt(j);
    //                        }
    //                    }


    //                }

    //            }

    //        }
    //        dt.Rows.Add(drFooter);

    //        //Round export

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //            {
    //                for (int j = 0; j < dt.Columns.Count; j++)
    //                {
    //                    //DisplayTextSummary
    //                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                    {
    //                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
    //                        {

    //                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file"
    //                                || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "image")
    //                            {
    //                                if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
    //                                {
    //                                    try
    //                                    {
    //                                        if (dr[j].ToString().Length > 37)
    //                                        {
    //                                            dr[j] = dr[j].ToString().Substring(37);

    //                                        }
    //                                    }
    //                                    catch
    //                                    {
    //                                        //
    //                                    }
    //                                }

    //                            }

    //                            if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text"
    //                                  && (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
    //                                 || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "radiobutton")
    //                                 && _dtRecordColums.Rows[i]["DropdownValues"].ToString() != "")
    //                            {
    //                                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                                {

    //                                    if (dr[j].ToString() != "")
    //                                    {
    //                                        string strText = GetTextFromValueForDD(_dtRecordColums.Rows[i]["DropdownValues"].ToString(), dr[j].ToString());
    //                                        if (strText != "")
    //                                            dr[j] = strText;
    //                                    }
    //                                }

    //                            }

    //                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "time")
    //                            {
    //                                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                                {

    //                                    if (dr[j].ToString() != "" )
    //                                    {

    //                                        TimeSpan ts = TimeSpan.Parse(dr[j].ToString());
    //                                        dr[j] = ts.ToString(@"hh\:mm");
    //                                    }
    //                                }

    //                            }


    //                            //if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
    //                            //    && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
    //                            //    || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
    //                            //     && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
    //                            //    && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
    //                            //{
    //                            //    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                            //    {

    //                            //        if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
    //                            //        {
    //                            //            try
    //                            //            {
    //                            //                Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));

    //                            //                //int iTableRecordID = int.Parse(dr[j].ToString());
    //                            //                string strLinkedColumnValue = dr[j].ToString();
    //                            //                DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
    //                            //                 + _dtRecordColums.Rows[i]["TableTableID"].ToString());

    //                            //                string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

    //                            //                foreach (DataRow dr2 in dtTableTableSC.Rows)
    //                            //                {
    //                            //                    strDisplayColumn = strDisplayColumn.Replace("[" + dr2["DisplayName"].ToString() + "]", "[" + dr2["SystemName"].ToString() + "]");

    //                            //                }
    //                            //                string sstrDisplayColumnOrg = strDisplayColumn;
    //                            //                string strFilterSQL = "";
    //                            //                if (theLinkedColumn.SystemName.ToLower() == "recordid")
    //                            //                {
    //                            //                    strFilterSQL = strLinkedColumnValue;
    //                            //                }
    //                            //                else
    //                            //                {
    //                            //                    strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
    //                            //                }

    //                            //                //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());
    //                            //                DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

    //                            //                if (dtTheRecord.Rows.Count > 0)
    //                            //                {
    //                            //                    foreach (DataColumn dc in dtTheRecord.Columns)
    //                            //                    {
    //                            //                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
    //                            //                    }
    //                            //                }
    //                            //                if (sstrDisplayColumnOrg != strDisplayColumn)
    //                            //                    dr[j] = strDisplayColumn;
    //                            //            }
    //                            //            catch
    //                            //            {
    //                            //                //
    //                            //            }


    //                            //        }
    //                            //    }

    //                            //}



    //                            if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
    //                            {
    //                                if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
    //                                {
    //                                    if (dr[j].ToString() != "")
    //                                    {
    //                                        try
    //                                        {
    //                                            dr[j] = Math.Round(double.Parse(Common.IgnoreSymbols( dr[j].ToString())), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
    //                                        }
    //                                        catch
    //                                        {
    //                                            //
    //                                        }
    //                                    }
    //                                }

    //                            }
    //                        }

    //                    }

    //                    //mm:hh
    //                    if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
    //                    {

    //                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "datetime")
    //                        {
    //                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                            {
    //                                if (dr[j].ToString().Length > 15)
    //                                {
    //                                    dr[j] = dr[j].ToString().Substring(0, 16);
    //                                }
    //                            }
    //                        }

    //                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "date")
    //                        {
    //                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                            {
    //                                if (dr[j].ToString().Length > 9)
    //                                {
    //                                    dr[j] = dr[j].ToString().Substring(0, 10);
    //                                }
    //                            }
    //                        }


    //                    }

    //                }
    //            }
    //        }

    //        dt.Columns.RemoveAt(dt.Columns.Count - 1);
    //        dt.Columns.RemoveAt(dt.Columns.Count - 1);

    //        dt.AcceptChanges();

    //        DBG.Common.ExportUtil.ExportToExcel(dt, "\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".xls" + "\"");


    //    }
    protected void ddlEnteredBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }



    protected void ddlUploadedBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }


    protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    protected void chkShowOnlyWarning_CheckedChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    //protected void PopulateYAxis()
    //{

    //    //DataTable dtSCs = RecordManager.ets_Table_Columns_Summary(TableID);
    //    DataTable dtSCs = Common.DataTableFromText("SELECT * FROM [Column] WHERE SummarySearch=1 AND TableID=" + TableID.ToString() + " ORDER BY DisplayOrder");


    //    foreach (DataRow dr in dtSCs.Rows)
    //    {
    //        if (bool.Parse(dr["IsStandard"].ToString()) == false)
    //        {
    //            System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(
    //                dr["Heading"].ToString() == "" ? dr["DisplayName"].ToString() : dr["Heading"].ToString(), dr["ColumnID"].ToString());

    //            ddlYAxis.Items.Insert(ddlYAxis.Items.Count, aItem);
    //        }

    //    }

    //    System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("-- None --", "-1");

    //    ddlYAxis.Items.Insert(0, fItem);

    //}

    protected void PopulateYAxisBulk()
    {

        DataTable dtSCs = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));

        foreach (DataRow dr in dtSCs.Rows)
        {
            if (bool.Parse(dr["IsStandard"].ToString()) == false)
            {
                if (dr["ColumnType"].ToString() == "text" || dr["ColumnType"].ToString() == "checkbox"
                    || dr["ColumnType"].ToString() == "number"
                 || dr["ColumnType"].ToString() == "date" || dr["ColumnType"].ToString() == "datetime"
                    || (dr["ColumnType"].ToString() == "dropdown" && dr["ParentColumnID"] == DBNull.Value))
                {
                    System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(dr["Heading"].ToString(), dr["ColumnID"].ToString());

                    ddlYAxisBulk.Items.Insert(ddlYAxisBulk.Items.Count, aItem);
                }
            }

        }

        System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("-- None --", "");

        ddlYAxisBulk.Items.Insert(0, fItem);

    }


    //protected void ddlFilterValue_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    lnkSearch_Click(null, null);
    //}

    //protected void cbcvSumFilter_OnddlYAxis_Changed(object sender, EventArgs e)
    //{

    //    lnkSearch_Click(null, null);
    //}
    //protected void PutDDLValue_Text(string strDropdownValues, ref  DropDownList ddl)
    //{
    //    ddl.Items.Clear();
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    string strValue = "";
    //    string strText = "";

    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        strValue = "";
    //        strText = "";
    //        if (s.IndexOf(",") > -1)
    //        {
    //            strValue = s.Substring(0, s.IndexOf(","));
    //            strText = s.Substring(strValue.Length + 1);
    //            if (strValue != "" && strText != "")
    //            {
    //                ListItem liTemp = new ListItem(strText, strValue);
    //                ddl.Items.Add(liTemp);
    //            }
    //        }
    //    }

    //    ListItem liSelect = new ListItem("--Please Select--", "");
    //    ddl.Items.Insert(0, liSelect);

    //}

    //protected void PutDDLValues(string strDropdownValues, ref  DropDownList ddl)
    //{
    //    ddl.Items.Clear();

    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        ListItem liTemp = new ListItem(s, s);
    //        ddl.Items.Add(liTemp);
    //    }

    //    ListItem liSelect = new ListItem("--Please Select--", "");
    //    ddl.Items.Insert(0, liSelect);

    //}
    //protected void ddlYAxis_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //do the show hide

    //    if (ddlYAxis.SelectedValue == "-1")
    //    {
    //        txtLowerLimit.Visible = false;
    //        txtUpperLimit.Visible = false;
    //        lblTo.Visible = false;
    //        txtSearchText.Visible = false;
    //        ddlDropdownColumnSearch.Visible = false;

    //        txtLowerLimit.Text = "";
    //        txtUpperLimit.Text = "";
    //        txtSearchText.Text = "";
    //        if (ddlDropdownColumnSearch.Items.Count > 0)
    //            ddlDropdownColumnSearch.SelectedIndex = 0;

    //        Pager_OnApplyFilter(null, null);
    //        PopulateSearchParams();
    //        lnkSearch_Click(null, null);

    //    }
    //    else
    //    {
    //        Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlYAxis.SelectedValue));

    //        if (theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false)
    //        {
    //            txtLowerLimit.Visible = true;
    //            txtUpperLimit.Visible = true;
    //            lblTo.Visible = true;
    //            txtSearchText.Visible = false;

    //            txtLowerDate.Visible = false;
    //            txtUpperDate.Visible = false;
    //            ibLowerDate.Visible = false;
    //            ibUpperDate.Visible = false;
    //            ddlDropdownColumnSearch.Visible = false;
    //        }
    //        else if (theColumn.ColumnType == "dropdown" && theColumn.DropdownValues!=""  && (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
    //        {
    //            txtLowerLimit.Visible = false;
    //            txtUpperLimit.Visible = false;
    //            lblTo.Visible = false;
    //            txtSearchText.Visible = false;

    //            txtLowerDate.Visible = false;
    //            txtUpperDate.Visible = false;
    //            ibLowerDate.Visible = false;
    //            ibUpperDate.Visible = false;
    //            ddlDropdownColumnSearch.Visible = true;

    //            if (theColumn.DropDownType == "values")
    //            {
    //                PutDDLValues(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
    //            }
    //            else
    //            {
    //                PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
    //            }

    //        }
    //        else if (theColumn.ColumnType == "date" || theColumn.ColumnType == "datetime")
    //        {
    //            txtLowerLimit.Visible = false;
    //            txtUpperLimit.Visible = false;
    //            lblTo.Visible = true;
    //            txtSearchText.Visible = false;

    //            txtLowerDate.Visible = true;
    //            txtUpperDate.Visible = true;
    //            ibLowerDate.Visible = true;
    //            ibUpperDate.Visible = true;
    //            ddlDropdownColumnSearch.Visible = false;
    //        }
    //        else
    //        {
    //            txtLowerLimit.Visible = false;
    //            txtUpperLimit.Visible = false;
    //            lblTo.Visible = false;
    //            txtSearchText.Visible = true;

    //            txtLowerDate.Visible = false;
    //            txtUpperDate.Visible = false;
    //            ibLowerDate.Visible = false;
    //            ibUpperDate.Visible = false;
    //            ddlDropdownColumnSearch.Visible = false;
    //        }
    //        txtLowerLimit.Text = "";
    //        txtUpperLimit.Text = "";
    //        txtSearchText.Text = "";
    //        txtLowerDate.Text = "";
    //        txtUpperDate.Text = "";
    //        if (ddlDropdownColumnSearch.Items.Count > 0)
    //            ddlDropdownColumnSearch.SelectedIndex = 0;

    //    }

    //}

    protected void PopulateSearchCriteria(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlSC_Doc = new System.Xml.XmlDocument();

                xmlSC_Doc.Load(new StringReader(theSearchCriteria.SearchText));

                txtDateFrom.Text = xmlSC_Doc.FirstChild[txtDateFrom.ID].InnerText;
                txtDateTo.Text = xmlSC_Doc.FirstChild[txtDateTo.ID].InnerText;

                if (xmlSC_Doc.FirstChild[hfAndOr1.ID] != null)
                    hfAndOr1.Value = xmlSC_Doc.FirstChild[hfAndOr1.ID].InnerText;

                if (xmlSC_Doc.FirstChild[hfAndOr2.ID] != null)
                    hfAndOr2.Value = xmlSC_Doc.FirstChild[hfAndOr2.ID].InnerText;

                if (xmlSC_Doc.FirstChild[hfAndOr3.ID] != null)
                    hfAndOr3.Value = xmlSC_Doc.FirstChild[hfAndOr3.ID].InnerText;

                string strcbcSearchMain = xmlSC_Doc.FirstChild[cbcSearchMain.ID].InnerText;

                if (strcbcSearchMain != "-1")
                {
                    //PopulateSearchCriteriaCBCMain(int.Parse(strcbcSearchMain));
                    PopulateSearchCriteriaCBC(int.Parse(strcbcSearchMain), cbcSearchMain);
                }

                string strcbcSearch1 = xmlSC_Doc.FirstChild[cbcSearch1.ID].InnerText;

                if (strcbcSearch1 != "-1")
                {
                    //PopulateSearchCriteriaCBC1(int.Parse(strcbcSearch1));
                    PopulateSearchCriteriaCBC(int.Parse(strcbcSearch1), cbcSearch1);
                }

                string strcbcSearch2 = xmlSC_Doc.FirstChild[cbcSearch2.ID].InnerText;

                if (strcbcSearch2 != "-1")
                {
                    //PopulateSearchCriteriaCBC2(int.Parse(strcbcSearch2));
                    PopulateSearchCriteriaCBC(int.Parse(strcbcSearch2), cbcSearch2);
                }

                string strcbcSearch3 = xmlSC_Doc.FirstChild[cbcSearch3.ID].InnerText;

                if (strcbcSearch3 != "-1")
                {
                    //PopulateSearchCriteriaCBC3(int.Parse(strcbcSearch3));
                    PopulateSearchCriteriaCBC(int.Parse(strcbcSearch3), cbcSearch3);
                }

                hfTextSearch.Value = xmlSC_Doc.FirstChild[hfTextSearch.ID].InnerText;

                bool? bIsNumericY = null;
                if (xmlSC_Doc.FirstChild["bIsNumericY"].InnerText != "")
                {
                    bIsNumericY = bool.Parse(xmlSC_Doc.FirstChild["bIsNumericY"].InnerText);
                }
                //txtSearchText.Text = xmlDoc.FirstChild[txtSearchText.ID].InnerText;
                ddlEnteredBy.Text = xmlSC_Doc.FirstChild[ddlEnteredBy.ID].InnerText;

                ddlUploadedBatch.Text = xmlSC_Doc.FirstChild[ddlUploadedBatch.ID].InnerText;

                //ddlDropdownColumnSearch.Items.FindByValue(xmlDoc.FirstChild[ddlDropdownColumnSearch.ID].InnerText);

                chkIsActive.Checked = bool.Parse(xmlSC_Doc.FirstChild[chkIsActive.ID].InnerText);
                chkShowOnlyWarning.Checked = bool.Parse(xmlSC_Doc.FirstChild[chkShowOnlyWarning.ID].InnerText);

                chkShowAdvancedOptions.Checked = bool.Parse(xmlSC_Doc.FirstChild[chkShowAdvancedOptions.ID].InnerText);

                //ddlFilterValue.Text = xmlDoc.FirstChild[ddlFilterValue.ID].InnerText;
                //cbcvSumFilter.SetValue = xmlSC_Doc.FirstChild[cbcvSumFilter.ID].InnerText;

                _iStartIndex = int.Parse(xmlSC_Doc.FirstChild["iStartIndex"].InnerText);
                _iMaxRows = int.Parse(xmlSC_Doc.FirstChild["iMaxRows"].InnerText);

                if (_theView != null)
                {
                    if (_theView.RowsPerPage != null)
                    {
                        _iMaxRows = (int)_theView.RowsPerPage;
                    }
                }

                _strGridViewSortColumn = xmlSC_Doc.FirstChild["GridViewSortColumn"].InnerText;
                _strGridViewSortDirection = xmlSC_Doc.FirstChild["GridViewSortDirection"].InnerText;

                if (chkShowAdvancedOptions.Checked == false)
                {
                    foreach (DataRow dr in _dtDynamicSearchColumns.Rows)
                    {

                        try
                        {

                            if (dr["ColumnType"].ToString() == "number" || dr["ColumnType"].ToString() == "calculation")
                            {
                                TextBox txtLowerLimit = (TextBox)tblSearchControls.FindControl("txtLowerLimit_" + dr["SystemName"].ToString());
                                TextBox txtUpperLimit = (TextBox)tblSearchControls.FindControl("txtUpperLimit_" + dr["SystemName"].ToString());

                                if (txtLowerLimit != null && txtUpperLimit != null)
                                {
                                    if (xmlSC_Doc.FirstChild[txtLowerLimit.ID] != null)
                                        txtLowerLimit.Text = xmlSC_Doc.FirstChild[txtLowerLimit.ID].InnerText;

                                    if (xmlSC_Doc.FirstChild[txtUpperLimit.ID] != null)
                                        txtUpperLimit.Text = xmlSC_Doc.FirstChild[txtUpperLimit.ID].InnerText;
                                }

                            }
                            else if (dr["ColumnType"].ToString() == "text")
                            {
                                TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                                if (txtSearch != null)
                                {
                                    if (xmlSC_Doc.FirstChild[txtSearch.ID] != null)
                                        txtSearch.Text = xmlSC_Doc.FirstChild[txtSearch.ID].InnerText;
                                }

                            }
                            else if (dr["ColumnType"].ToString() == "date")
                            {
                                TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                                TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());

                                if (txtLowerDate != null && txtUpperDate != null)
                                {
                                    if (xmlSC_Doc.FirstChild[txtLowerDate.ID] != null)
                                        txtLowerDate.Text = xmlSC_Doc.FirstChild[txtLowerDate.ID].InnerText;

                                    if (xmlSC_Doc.FirstChild[txtUpperDate.ID] != null)
                                        txtUpperDate.Text = xmlSC_Doc.FirstChild[txtUpperDate.ID].InnerText;
                                }

                            }
                            else if (dr["ColumnType"].ToString() == "datetime")
                            {
                                TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                                TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());

                                TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                                TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                                if (txtLowerDate != null && txtUpperDate != null)
                                {
                                    if (xmlSC_Doc.FirstChild[txtLowerDate.ID] != null)
                                        txtLowerDate.Text = xmlSC_Doc.FirstChild[txtLowerDate.ID].InnerText;

                                    if (xmlSC_Doc.FirstChild[txtUpperDate.ID] != null)
                                        txtUpperDate.Text = xmlSC_Doc.FirstChild[txtUpperDate.ID].InnerText;
                                }
                                if (txtLowerTime != null && txtUpperTime != null)
                                {
                                    if (xmlSC_Doc.FirstChild[txtLowerTime.ID] != null)
                                        txtLowerTime.Text = xmlSC_Doc.FirstChild[txtLowerTime.ID].InnerText;

                                    if (xmlSC_Doc.FirstChild[txtUpperTime.ID] != null)
                                        txtUpperTime.Text = xmlSC_Doc.FirstChild[txtUpperTime.ID].InnerText;
                                }


                            }
                            else if (dr["ColumnType"].ToString() == "time")
                            {
                                TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                                TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                                if (txtLowerTime != null && txtUpperTime != null)
                                {
                                    if (xmlSC_Doc.FirstChild[txtLowerTime.ID] != null)
                                        txtLowerTime.Text = xmlSC_Doc.FirstChild[txtLowerTime.ID].InnerText;

                                    if (xmlSC_Doc.FirstChild[txtUpperTime.ID] != null)
                                        txtUpperTime.Text = xmlSC_Doc.FirstChild[txtUpperTime.ID].InnerText;
                                }

                            }
                            else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                            {
                                DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                                if (ddlSearch != null)
                                {
                                    try
                                    {
                                        ddlSearch.Text = xmlSC_Doc.FirstChild[ddlSearch.ID].InnerText;
                                    }
                                    catch
                                    {
                                        //
                                    }
                                }

                            }
                            //else if (dr["ColumnType"].ToString() == "radiobutton" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                            else if (dr["ColumnType"].ToString() == "radiobutton" || dr["ColumnType"].ToString() == "listbox"
                                        || dr["ColumnType"].ToString() == "checkbox")
                            {
                                DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                                if (ddlSearch != null)
                                {
                                    ddlSearch.Text = xmlSC_Doc.FirstChild[ddlSearch.ID].InnerText;
                                }

                            }
                            else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "table" || dr["DropDownType"].ToString() == "tabledd") &&
                       dr["TableTableID"] != DBNull.Value && dr["DisplayColumn"].ToString() != "")
                            {
                                DropDownList ddlParentSearch = (DropDownList)tblSearchControls.FindControl("ddlParentSearch_" + dr["SystemName"].ToString());

                                if (ddlParentSearch != null && xmlSC_Doc.FirstChild[ddlParentSearch.ID] != null)
                                {
                                    ddlParentSearch.Text = xmlSC_Doc.FirstChild[ddlParentSearch.ID].InnerText;
                                }

                            }
                            else
                            {
                                TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                                if (txtSearch != null)
                                {
                                    if (xmlSC_Doc.FirstChild[txtSearch.ID] != null)
                                        txtSearch.Text = xmlSC_Doc.FirstChild[txtSearch.ID].InnerText;
                                }

                            }
                        }
                        catch
                        {
                            //
                        }
                    }

                    //
                    foreach (DataRow dr in _dtSearchGroup.Rows)
                    {

                        TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SearchGroupID"].ToString());
                        if (txtSearch != null)
                        {


                            txtSearch.Text = xmlSC_Doc.FirstChild[txtSearch.ID].InnerText;

                        }
                    }

                }

                //PopulateSearchParams();

            }//theSearchCriteria
            else
            {
                // theSearchCriteria is null
                _bBindWithSC = false;
                Session["SCid" + hfViewID.Value] = null;
                ViewState["_iSearchCriteriaID"] = null;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }



    protected void PopulateSearchCriteriaCBC(int iSearchCriteriaID, Pages_UserControl_ControlByColumn cbcX)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                cbcX.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;
                cbcX.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;
                cbcX.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;
                cbcX.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;
                cbcX.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;
                cbcX.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;
                cbcX.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;
                cbcX.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;

                if (xmlDoc.FirstChild["CompareOperator"] != null)
                    cbcX.CompareOperator = xmlDoc.FirstChild["CompareOperator"].InnerText;


                //PopulateSearchParams();


            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }

    //    protected void PopulateSearchCriteriaCBCMain(int iSearchCriteriaID)
    //    {
    //        try
    //        {
    //            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


    //            if (theSearchCriteria != null)
    //            {

    //                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

    //                cbcSearchMain.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;
    //                cbcSearchMain.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;
    //                cbcSearchMain.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;
    //                cbcSearchMain.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;
    //                cbcSearchMain.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;
    //                cbcSearchMain.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;
    //                cbcSearchMain.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;
    //                cbcSearchMain.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;


    //                //PopulateSearchParams();


    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            lblMsg.Text = ex.Message;
    //        }


    //    }



    //    protected void PopulateSearchCriteriaCBC1(int iSearchCriteriaID)
    //    {
    //        try
    //        {
    //            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


    //            if (theSearchCriteria != null)
    //            {

    //                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

    //                cbcSearch1.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;
    //                cbcSearch1.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;
    //                cbcSearch1.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;
    //                cbcSearch1.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;
    //                cbcSearch1.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;
    //                cbcSearch1.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;
    //                cbcSearch1.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;
    //                cbcSearch1.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;




    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            lblMsg.Text = ex.Message;
    //        }


    //    }


    //    protected void PopulateSearchCriteriaCBC2(int iSearchCriteriaID)
    //    {
    //        try
    //        {
    //            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


    //            if (theSearchCriteria != null)
    //            {

    //                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

    //                cbcSearch2.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;
    //                cbcSearch2.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;
    //                cbcSearch2.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;
    //                cbcSearch2.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;
    //                cbcSearch2.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;
    //                cbcSearch2.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;
    //                cbcSearch2.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;
    //                cbcSearch2.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;




    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            lblMsg.Text = ex.Message;
    //        }


    //    }


    //    protected void PopulateSearchCriteriaCBC3(int iSearchCriteriaID)
    //    {
    //        try
    //        {
    //            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


    //            if (theSearchCriteria != null)
    //            {

    //                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

    //                cbcSearch3.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;
    //                cbcSearch3.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;
    //                cbcSearch3.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;
    //                cbcSearch3.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;
    //                cbcSearch3.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;
    //                cbcSearch3.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;
    //                cbcSearch3.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;
    //                cbcSearch3.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;                

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            lblMsg.Text = ex.Message;
    //        }


    //    }

    protected void PopulateSearchGroup(int iC, ref TextBox txtSearchText)
    {

        if (txtSearchText.Text == "")
            return;

        //if (TextSearch == "")
        TextSearch = TextSearch + " OR ";

        Column theColumn = RecordManager.ets_Column_Details(iC);





        if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
        {
            if (theColumn.DropDownType == "values")
            {
                if (txtSearchText.Text != "")
                    TextSearch = TextSearch + " Record." + theColumn.SystemName + " like '%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
            }
            if (theColumn.DropDownType == "value_text")
            {
                string strSearchValue = Common.GetDDLValueFromText(theColumn.DropdownValues, txtSearchText.Text);
                if (strSearchValue != "")
                {
                    TextSearch = TextSearch + " Record." + theColumn.SystemName + " like '%" + strSearchValue.Trim().Replace("'", "''") + "%'";
                }
            }
        }
        else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
            theColumn.TableTableID != null && theColumn.DisplayColumn != "")
        {


            if (txtSearchText.Text != "")
            {



                //string search = txtSearchText.Text.Replace("'", "''");
                string search = txtSearchText.Text;

                if (search.Trim() == "")
                {
                    return;
                }

                string regex = @"\[(.*?)\]";
                string strDisplayColumn = theColumn.DisplayColumn;
                string text = theColumn.DisplayColumn;

                string strDCForSQL = strDisplayColumn.Replace("'", "''");
                int i = 1;
                string strFirstSystemName = "";
                string strFirstDisplayName = "";
                string strSecondSystemName = "";
                string strSecondDisplayName = "";
                bool bHaveSecond = false;
                //List<string> lstDisplayName = new List<string>();
                //List<string> lstSystemName = new List<string>();
                foreach (Match match in Regex.Matches(text, regex))
                {
                    string strEachDisplayName = match.Groups[1].Value;

                    //lstDisplayName.Add(strEachDisplayName);

                    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName FROM [Column] WHERE   TableID ="
                        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

                    string strEachSystemName = "";
                    if (dtTableTableSC.Rows.Count > 0)
                    {
                        strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
                        //lstSystemName.Add(strEachSystemName);
                    }


                    if (i == 1)
                    {

                        strFirstDisplayName = strEachDisplayName;
                        strFirstSystemName = strEachSystemName;
                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
                    }
                    if (i == 2)
                    {
                        bHaveSecond = true;
                        strSecondDisplayName = strEachDisplayName;
                        strSecondSystemName = strEachSystemName;
                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
                    }
                    else
                    {
                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
                    }
                    i = i + 1;
                }
                strDCForSQL = strDCForSQL.Trim() + "'";

                //strDCForSQL = strDCForSQL.Substring(0, strDCForSQL.Length - 2);
                string strSecondSQL = "";
                if (bHaveSecond)
                {
                    strSecondSQL = " OR CAST(" + strSecondSystemName + " AS VARCHAR(MAX)) like '%" + search.Replace("'", "''") + "%'";
                }

                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

                DataTable dtData = Common.DataTableFromText(@"SELECT TOP 1000 " + theLinkedColumn.SystemName + "," + strDCForSQL + @"
                    FROM Record WHERE  IsActive= 1 AND 
                    TableID=" + theColumn.TableTableID.ToString() + " and (CAST(" + strFirstSystemName + @" AS VARCHAR(MAX)) like '%" + search.Replace("'", "''") + @"%'" + strSecondSQL + ")");

                string strRecordIDs = "";
                foreach (DataRow dr in dtData.Rows)
                {

                    strRecordIDs = strRecordIDs + "'" + dr[0].ToString() + "'" + ",";
                }

                strRecordIDs = strRecordIDs + "'---1---'";


                TextSearch = TextSearch + " Record." + theColumn.SystemName + " IN (" + strRecordIDs + ")";

            }

        }
        else
        {

            if (txtSearchText.Text != "")
            {
                TextSearch = TextSearch + " Record." + theColumn.SystemName + " like'%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
            }

        }
    }


    protected void AddVisible(bool bVisible)
    {

    }

    protected void PopulateSearchParams()
    {
        //if (ddlYAxis.SelectedValue == "-1")
        //{
        //_strNumericSearch = "";
        TextSearch = "";


        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

        if (_theView.Filter != "" && _theView.FilterControlsInfo != "")
            xmlDoc.Load(new StringReader(_theView.FilterControlsInfo));



        if (chkShowAdvancedOptions.Checked == false)//_bDynamicSearch
        {
            foreach (DataRow dr in _dtDynamicSearchColumns.Rows)
            {
                string strLowerOperator = "=";
                string strViewOperator = "";
                string strActiveOperator = "=";
                bool bFromView = false;
                if (xmlDoc != null)
                {
                    strViewOperator = GetOperatorFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                    if (strViewOperator == "between" || strViewOperator == "")
                        strLowerOperator = "=";

                }

                if (strViewOperator != "")
                    bFromView = true;

                if (strLowerOperator == "empty")
                {

                    TextSearch = TextSearch + " AND (Record." + dr["SystemName"].ToString() + " IS NULL OR LEN(Record." + dr["SystemName"].ToString() + ")=0)";
                }
                else if (strLowerOperator == "notempty")
                {

                    TextSearch = " (Record." + dr["SystemName"].ToString() + " IS NOT NULL AND LEN(Record." + dr["SystemName"].ToString() + ")>0)";
                }


                if (dr["ColumnType"].ToString() == "text")
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    if (txtSearch != null)
                    {
                        //if (txtSearch.Text != "")
                        //{
                        string strViewFiltervalue = "";

                        string strTextSearch = "";
                        strActiveOperator = strLowerOperator;

                        if (strViewOperator == "" || strViewOperator == "between")
                            strViewOperator = "=";

                        if (txtSearch.Text != "")
                        {
                            strActiveOperator = "=";
                            strLowerOperator = "=";
                            if (strLowerOperator == "=")
                            {
                                strTextSearch = " Record." + dr["SystemName"].ToString() + " LIKE'%" + txtSearch.Text.Trim().Replace("'", "''") + "%'";
                            }
                            //if (strLowerOperator == "<>")
                            //{
                            //    strTextSearch = " AND  CHARINDEX('" + txtSearch.Text.Trim().Replace("'", "''") + "',Record." + dr["SystemName"].ToString()+")=0 ";
                            //}
                            //else
                            //{
                            //    strTextSearch =  " AND Record." + dr["SystemName"].ToString() + " " + strLowerOperator + "'" + txtSearch.Text.Trim().Replace("'", "''") + "'";
                            //}
                        }

                        if (xmlDoc != null)
                        {
                            strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                            if (strViewFiltervalue != null)
                            {
                                string strActiveValue = strViewFiltervalue;

                                if (strTextSearch != "")
                                {
                                    strActiveValue = txtSearch.Text;
                                    bFromView = false;
                                }
                                else
                                {
                                    strActiveOperator = strViewOperator;
                                }
                                UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                            }
                        }

                        if (strViewFiltervalue == null)
                        {
                            if (strTextSearch != "")
                                TextSearch = TextSearch + " AND " + strTextSearch;
                        }



                        //}
                    }
                }
                else if (dr["ColumnType"].ToString() == "date")
                {
                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());

                    if (txtLowerDate != null && txtUpperDate != null)
                    {

                        string strViewFiltervalue = "";
                        string strDateTextSearch = "";

                        if (txtLowerDate.Text != "" && txtUpperDate.Text == "")
                        {
                            strLowerOperator = _strEqualOrGreaterOperator.Trim();
                            strActiveOperator = strLowerOperator;
                        }

                        if (txtLowerDate.Text != "" && txtUpperDate.Text != "")
                        {
                            strLowerOperator = ">=";
                            strActiveOperator = "between";
                        }


                        //if (strViewFiltervalue == "")
                        //{
                        DateTime dateValue;
                        //string strDateTextSearch = "";
                        string strLowerPart = "";
                        txtLowerDate.Text = txtLowerDate.Text.Replace(" ", "");
                        txtUpperDate.Text = txtUpperDate.Text.Replace(" ", "");

                        string strLowerDate = Common.ReturnDateStringFromToken(txtLowerDate.Text.Trim());
                        string strUpperDate = Common.ReturnDateStringFromToken(txtUpperDate.Text.Trim());


                        if (_bEqualOrGreaterOperator == true && strLowerDate.Trim() != "" && strUpperDate.Trim() == "")
                        {
                            if (xmlDoc != null)
                                RemoveViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString());


                        }


                        if (strLowerDate != "")
                        {
                            if (DateTime.TryParseExact(strLowerDate, Common.Dateformats,
                                         new CultureInfo("en-GB"),
                                         DateTimeStyles.None,
                                         out dateValue))
                            {

                                strDateTextSearch = " CONVERT(Datetime,Record." + dr["SystemName"].ToString() + ",103) " + _strEqualOrGreaterOperator + " CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
                                strLowerPart = " CONVERT(Datetime,Record." + dr["SystemName"].ToString() + ",103) >= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";

                            }
                        }

                        if (txtUpperDate.Text != "" && txtLowerDate.Text != "")
                        {
                            if (DateTime.TryParseExact(strUpperDate, Common.Dateformats,
                                         new CultureInfo("en-GB"),
                                         DateTimeStyles.None,
                                         out dateValue))
                            {

                                strDateTextSearch = strLowerPart + " AND CONVERT(Datetime,Record." + dr["SystemName"].ToString() + ",103) <= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
                            }
                        }
                        else
                        {
                            if (txtUpperDate.Text != "")
                            {
                                if (DateTime.TryParseExact(strUpperDate, Common.Dateformats,
                                             new CultureInfo("en-GB"),
                                             DateTimeStyles.None,
                                             out dateValue))
                                {
                                    strDateTextSearch = " CONVERT(Datetime,Record." + dr["SystemName"].ToString() + ",103) <= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
                                }
                            }
                        }


                        //if (txtLowerDate.Text != "" && txtUpperDate.Text != "")
                        //{
                        if (strDateTextSearch != "")
                            strDateTextSearch = " (" + strDateTextSearch + ")";
                        //}


                        if (xmlDoc != null)
                        {
                            strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                            if (strViewFiltervalue != null)
                            {
                                string strActiveValue = strViewFiltervalue;
                                if (strDateTextSearch != "")
                                {
                                    strActiveValue = txtLowerDate.Text + "____" + txtUpperDate.Text;
                                    bFromView = false;
                                }
                                else
                                {
                                    strActiveOperator = strViewOperator;
                                }

                                UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                            }

                        }

                        if (strViewFiltervalue == null)
                        {
                            string strTimeGarbage = " ISDATE(Record." + dr["SystemName"].ToString() + ")=1 AND ";

                            if (strDateTextSearch != "")
                                TextSearch = TextSearch + " AND (" + strTimeGarbage + strDateTextSearch + ")"; ;
                        }


                    }
                }
                else if (dr["ColumnType"].ToString() == "datetime")
                {
                    TextBox txtLowerDate = (TextBox)tblSearchControls.FindControl("txtLowerDate_" + dr["SystemName"].ToString());
                    TextBox txtUpperDate = (TextBox)tblSearchControls.FindControl("txtUpperDate_" + dr["SystemName"].ToString());

                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                    string strLowerTime = " 00:00";
                    string strUpperTime = " 23:59";
                    if (txtLowerTime != null && txtUpperTime != null)
                    {

                        strLowerTime = txtLowerTime.Text.Trim() == "" ? " 00:00" : " " + txtLowerTime.Text.Trim();
                        strUpperTime = txtUpperTime.Text.Trim() == "" ? " 23:59" : " " + txtUpperTime.Text.Trim();
                    }



                    if (txtLowerDate != null && txtUpperDate != null)
                    {
                        string strViewFiltervalue = "";
                        string strDateTextSearch = "";

                        if (txtLowerDate.Text != "" && txtUpperDate.Text == "")
                        {
                            strLowerOperator = _strEqualOrGreaterOperator.Trim();
                            strActiveOperator = strLowerOperator;
                        }

                        if (txtLowerDate.Text != "" && txtUpperDate.Text != "")
                        {
                            strLowerOperator = ">=";
                            strActiveOperator = "between";
                        }


                        DateTime dateValue;
                        string strLowerPart = "";

                        txtLowerDate.Text = txtLowerDate.Text.Replace(" ", "");
                        txtUpperDate.Text = txtUpperDate.Text.Replace(" ", "");

                        string strLowerDate = Common.ReturnDateStringFromToken(txtLowerDate.Text.Trim());
                        string strUpperDate = Common.ReturnDateStringFromToken(txtUpperDate.Text.Trim());

                        if (_bEqualOrGreaterOperator == true && txtLowerDate.Text.Trim() != "" && txtUpperDate.Text.Trim() == "")
                        {
                            if (xmlDoc != null)
                                RemoveViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString());
                        }

                        if (txtLowerDate.Text != "")
                        {
                            if (DateTime.TryParseExact(strLowerDate.Trim() + strLowerTime, Common.DateTimeformats,
                                         new CultureInfo("en-GB"),
                                         DateTimeStyles.None,
                                         out dateValue))
                            {

                                strDateTextSearch = " CONVERT(Datetime,Record." + dr["SystemName"].ToString() + ",103) " + _strEqualOrGreaterOperator + " CONVERT(Datetime,'" + dateValue.ToString() + "',103)";

                                strLowerPart = " CONVERT(Datetime,Record." + dr["SystemName"].ToString() + ",103) >= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                            }
                        }

                        if (txtUpperDate.Text != "" && txtLowerDate.Text != "")
                        {
                            if (DateTime.TryParseExact(strUpperDate.Trim() + strUpperTime, Common.DateTimeformats,
                                         new CultureInfo("en-GB"),
                                         DateTimeStyles.None,
                                         out dateValue))
                            {
                                strDateTextSearch = strLowerPart + " AND CONVERT(Datetime,Record." + dr["SystemName"].ToString() + ",103) <= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                            }
                        }
                        else
                        {
                            if (txtUpperDate.Text != "")
                            {
                                if (DateTime.TryParseExact(strUpperDate.Trim() + strUpperTime, Common.DateTimeformats,
                                             new CultureInfo("en-GB"),
                                             DateTimeStyles.None,
                                             out dateValue))
                                {
                                    strDateTextSearch = " CONVERT(Datetime,Record." + dr["SystemName"].ToString() + ",103) <= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                                }
                            }
                        }


                        //if (txtLowerDate.Text != "" && txtUpperDate.Text != "")
                        //{
                        if (strDateTextSearch != "")
                            strDateTextSearch = " (" + strDateTextSearch + ")";
                        //}


                        if (xmlDoc != null)
                        {
                            strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                            if (strViewFiltervalue != null)
                            {
                                string strActiveValue = strViewFiltervalue;
                                if (strDateTextSearch != "")
                                {
                                    strActiveValue = txtLowerDate.Text + strLowerTime + "____" + txtUpperDate.Text + strUpperTime;
                                    bFromView = false;
                                }
                                else
                                {
                                    strActiveOperator = strViewOperator;
                                }

                                UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                            }



                        }


                        if (strViewFiltervalue == null)
                        {
                            string strTimeGarbage = " ISDATE(Record." + dr["SystemName"].ToString() + ")=1 AND ";
                            if (strDateTextSearch != "")
                                TextSearch = TextSearch + " AND (" + strTimeGarbage + strDateTextSearch + ")";

                        }


                    }
                }
                else if (dr["ColumnType"].ToString() == "time")
                {
                    TextBox txtLowerTime = (TextBox)tblSearchControls.FindControl("txtLowerTime_" + dr["SystemName"].ToString());
                    TextBox txtUpperTime = (TextBox)tblSearchControls.FindControl("txtUpperTime_" + dr["SystemName"].ToString());

                    if (txtLowerTime != null && txtUpperTime != null)
                    {

                        string strLowerPart = "";
                        string strViewFiltervalue = "";
                        string strDateTextSearch = "";

                        if (txtLowerTime.Text != "" && txtUpperTime.Text.Trim() == "")
                        {
                            strLowerOperator = _strEqualOrGreaterOperator.Trim();
                            strActiveOperator = strLowerOperator;
                        }

                        if (txtLowerTime.Text != "" && txtUpperTime.Text != "")
                        {
                            strLowerOperator = ">=";
                            strActiveOperator = "between";
                        }


                        if (_bEqualOrGreaterOperator == true && txtLowerTime.Text.Trim() != "" && txtUpperTime.Text.Trim() == "")
                        {
                            if (xmlDoc != null)
                                RemoveViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString());

                        }

                        DateTime dateValue;

                        if (txtLowerTime.Text != "")
                        {
                            if (DateTime.TryParseExact(_strTodayShortDate + " " + txtLowerTime.Text.Trim(), Common.DateTimeformats,
                                         new CultureInfo("en-GB"),
                                         DateTimeStyles.None,
                                         out dateValue))
                            {
                                strDateTextSearch = " CONVERT(Datetime,CONVERT(varchar(11),getdate(),103) + ' ' + Record." + dr["SystemName"].ToString() + ",103) " + _strEqualOrGreaterOperator + " CONVERT(Datetime,'" + dateValue.ToString() + "',103)";

                            }
                        }

                        if (txtUpperTime.Text != "" && txtLowerTime.Text != "")
                        {
                            if (DateTime.TryParseExact(_strTodayShortDate + " " + txtLowerTime.Text.Trim(), Common.DateTimeformats,
                                        new CultureInfo("en-GB"),
                                        DateTimeStyles.None,
                                        out dateValue))
                            {
                                strLowerPart = " CONVERT(Datetime,CONVERT(varchar(11),getdate(),103) + ' ' + Record." + dr["SystemName"].ToString() + ",103) >= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                            }

                            if (DateTime.TryParseExact(_strTodayShortDate + " " + txtUpperTime.Text.Trim(), Common.DateTimeformats,
                                         new CultureInfo("en-GB"),
                                         DateTimeStyles.None,
                                         out dateValue))
                            {
                                strDateTextSearch = strLowerPart + " AND CONVERT(Datetime,CONVERT(varchar(11),getdate(),103) + ' ' + Record." + dr["SystemName"].ToString() + ",103) <= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                            }
                        }
                        else
                        {
                            if (txtUpperTime.Text != "")
                            {
                                if (DateTime.TryParseExact(_strTodayShortDate + " " + txtUpperTime.Text.Trim(), Common.DateTimeformats,
                                             new CultureInfo("en-GB"),
                                             DateTimeStyles.None,
                                             out dateValue))
                                {
                                    strDateTextSearch = " CONVERT(Datetime,CONVERT(varchar(11),getdate(),103) + ' ' + Record." + dr["SystemName"].ToString() + ",103) <= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                                }
                            }
                        }


                        //if (txtLowerTime.Text != "" && txtUpperTime.Text != "")
                        //{
                        if (strDateTextSearch != "")
                            strDateTextSearch = " (" + strDateTextSearch + ")";
                        //}





                        if (xmlDoc != null)
                        {
                            strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                            if (strViewFiltervalue != null)
                            {
                                string strActiveValue = strViewFiltervalue;
                                if (strDateTextSearch != "")
                                {
                                    strActiveValue = txtLowerTime.Text + "____" + txtUpperTime.Text;
                                    bFromView = false;
                                }
                                else
                                {
                                    strActiveOperator = strViewOperator;
                                }

                                UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);

                            }
                        }

                        if (strViewFiltervalue == null)
                        {
                            string strTimeGarbage = " ISDATE(Record." + dr["SystemName"].ToString() + ")=1 AND ";

                            if (strDateTextSearch != "")
                                TextSearch = TextSearch + " AND (" + strTimeGarbage + strDateTextSearch + ")"; ;
                        }



                    }
                }

                else if (dr["ColumnType"].ToString() == "number" || dr["ColumnType"].ToString() == "calculation")
                {
                    TextBox txtLowerLimit = (TextBox)tblSearchControls.FindControl("txtLowerLimit_" + dr["SystemName"].ToString());
                    TextBox txtUpperLimit = (TextBox)tblSearchControls.FindControl("txtUpperLimit_" + dr["SystemName"].ToString());

                    if (txtLowerLimit != null && txtUpperLimit != null)
                    {

                        string strViewFiltervalue = "";
                        string strNumberTextSearch = "";


                        if (txtLowerLimit.Text != "" && txtUpperLimit.Text.Trim() == "")
                        {
                            strLowerOperator = _strEqualOrGreaterOperator.Trim();
                            strActiveOperator = strLowerOperator;
                        }


                        if (txtLowerLimit.Text.Trim() != "" && txtUpperLimit.Text.Trim() != "")
                        {
                            strLowerOperator = ">=";
                            strActiveOperator = "between";
                        }



                        if (_bEqualOrGreaterOperator == true && txtLowerLimit.Text.Trim() != "" && txtUpperLimit.Text.Trim() == "")
                        {
                            if (xmlDoc != null)
                                RemoveViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString());
                        }


                        if (txtLowerLimit.Text != "")
                        {
                            strNumberTextSearch = " dbo.RemoveNonNumericChar(Record." + dr["SystemName"].ToString() + ") " + _strEqualOrGreaterOperator + " CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
                        }

                        if (txtLowerLimit.Text != "" && txtUpperLimit.Text != "")
                        {
                            strActiveOperator = "between";
                            strNumberTextSearch = " dbo.RemoveNonNumericChar(Record." + dr["SystemName"].ToString() + ") >= CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
                            strNumberTextSearch = strNumberTextSearch + " AND dbo.RemoveNonNumericChar(Record." + dr["SystemName"].ToString() + ") <= CONVERT(decimal(20,10)," + txtUpperLimit.Text.Trim() + ")";
                        }
                        else
                        {
                            //if (txtLowerLimit.Text != "")
                            //{
                            //    strNumberTextSearch = " dbo.RemoveNonNumericChar(Record." + dr["SystemName"].ToString() + ") = CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
                            //}

                            if (txtUpperLimit.Text != "")
                            {

                                strNumberTextSearch = "  dbo.RemoveNonNumericChar(Record." + dr["SystemName"].ToString() + ") <= CONVERT(decimal(20,10)," + txtUpperLimit.Text.Trim() + ")";
                            }
                        }


                        if (strNumberTextSearch != "")
                        {
                            strNumberTextSearch = " dbo.RemoveNonNumericChar(Record." + dr["SystemName"].ToString() + ")<>'' AND " + strNumberTextSearch;
                            strNumberTextSearch = "(" + strNumberTextSearch + ")";
                        }


                        if (xmlDoc != null)
                        {
                            strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                            if (strViewFiltervalue != null)
                            {
                                //UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), txtLowerLimit.Text + "____" + txtUpperLimit.Text);

                                string strActiveValue = strViewFiltervalue;

                                if (strNumberTextSearch != "")
                                {
                                    strActiveValue = txtLowerLimit.Text + "____" + txtUpperLimit.Text;
                                    bFromView = false;
                                }
                                else
                                {
                                    strActiveOperator = strViewOperator;
                                }

                                UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                            }


                        }

                        if (strViewFiltervalue == null)
                        {
                            if (strNumberTextSearch != "")
                                TextSearch = TextSearch + " AND " + strNumberTextSearch;
                        }




                    }
                }

                else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "values"
                    || dr["DropDownType"].ToString() == "value_text"))
                {

                    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                    if (ddlSearch != null)
                    {
                        string strViewFiltervalue = "";
                        string strTextSearch = "";
                        strActiveOperator = strLowerOperator;

                        if (strViewOperator == "" || strViewOperator == "between")
                            strViewOperator = "=";

                        if (ddlSearch.SelectedItem != null)
                        {
                            if (ddlSearch.SelectedValue != "" && ddlSearch.SelectedValue != "vf_vf_vf")
                            {
                                strActiveOperator = "=";
                                strLowerOperator = "=";
                                if (strLowerOperator == "=")
                                {
                                    strTextSearch = " Record." + dr["SystemName"].ToString() + " ='" + ddlSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                                }
                            }

                            if (xmlDoc != null)
                            {
                                strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                                if (strViewFiltervalue != null)
                                {
                                    string strActiveValue = strViewFiltervalue;

                                    if (strTextSearch != "")
                                    {
                                        strActiveValue = ddlSearch.SelectedValue;
                                        bFromView = false;
                                    }
                                    else
                                    {
                                        strActiveOperator = strViewOperator;
                                    }
                                    if (ddlSearch.SelectedValue == "vf_vf_vf")
                                    {
                                        strActiveValue = "";
                                        bFromView = false;
                                        strActiveOperator = "=";
                                    }
                                    UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                                }

                            }

                            if (strViewFiltervalue == null && ddlSearch.SelectedValue != "")
                            {
                                TextSearch = TextSearch + " AND Record." + dr["SystemName"].ToString() + " ='" + ddlSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                            }
                        }
                    }
                }

                //else if (dr["ColumnType"].ToString() == "radiobutton" && (dr["DropDownType"].ToString() == "values" || dr["DropDownType"].ToString() == "value_text"))
                //{

                //    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                //    if (ddlSearch != null)
                //    {
                //        string strViewFiltervalue = "";
                //        string strTextSearch = "";
                //        strActiveOperator = strLowerOperator;

                //        if (strViewOperator == "" || strViewOperator == "between")
                //            strViewOperator = "=";

                //        if (ddlSearch.SelectedItem != null) // && ddlSearch.SelectedValue != ""
                //        {
                //            if (ddlSearch.SelectedValue != "")
                //            {
                //                strActiveOperator = "=";
                //                strLowerOperator = "=";
                //                if (strLowerOperator == "=")
                //                {
                //                    strTextSearch = " Record." + dr["SystemName"].ToString() + " ='" + ddlSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                //                }
                //            }

                //            if (xmlDoc != null)
                //            {
                //                strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                //                if (strViewFiltervalue != null)
                //                {
                //                    string strActiveValue = strViewFiltervalue;

                //                    if (strTextSearch != "")
                //                    {
                //                        strActiveValue = ddlSearch.SelectedValue;
                //                        bFromView = false;
                //                    }
                //                    else
                //                    {
                //                        strActiveOperator = strViewOperator;
                //                    }

                //                    UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                //                }

                //            }

                //            if (strViewFiltervalue == null && ddlSearch.SelectedValue != "")
                //            {
                //                TextSearch = TextSearch + " AND Record." + dr["SystemName"].ToString() + " ='" + ddlSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                //            }

                //        }
                //    }
                //}

                else if (dr["ColumnType"].ToString() == "dropdown" && (dr["DropDownType"].ToString() == "table" || dr["DropDownType"].ToString() == "tabledd") &&
            dr["TableTableID"] != DBNull.Value && dr["DisplayColumn"].ToString() != "")
                {

                    DropDownList ddlParentSearch = (DropDownList)tblSearchControls.FindControl("ddlParentSearch_" + dr["SystemName"].ToString());

                    if (ddlParentSearch != null)
                    {
                        string strViewFiltervalue = "";
                        string strTextSearch = "";
                        strActiveOperator = strLowerOperator;

                        if (strViewOperator == "" || strViewOperator == "between")
                            strViewOperator = "=";

                        if (ddlParentSearch.SelectedItem != null)
                        {
                            if (ddlParentSearch.SelectedValue != "" && ddlParentSearch.SelectedValue != "vf_vf_vf")
                            {
                                strActiveOperator = "=";
                                strLowerOperator = "=";
                                if (strLowerOperator == "=")
                                {
                                    strTextSearch = " Record." + dr["SystemName"].ToString() + " ='" + ddlParentSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                                }
                            }



                            if (xmlDoc != null)
                            {

                                strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());

                                if (strViewFiltervalue != null)
                                {
                                    string strActiveValue = strViewFiltervalue;

                                    if (strActiveValue == "-user-" && dr["LinkedParentColumnID"] != DBNull.Value)
                                    {
                                        string strColumnuserID = Common.GetValueFromSQL(@"SELECT TOP 1 ColumnID FROM [Column] WHERE TableID=" + dr["TableTableID"].ToString() + @" AND 
                                ColumnType='dropdown' AND DisplayColumn IS NOT NULL
                                            AND TableTableID=-1");

                                        if (strColumnuserID != "")
                                        {
                                            string strLoginText = "";
                                            SecurityManager.ProcessLoginUserDefault(dr["TableTableID"].ToString(), "",
                                            dr["LinkedParentColumnID"].ToString(), _ObjUser.UserID.ToString(), ref strActiveValue, ref strLoginText);

                                        }
                                    }

                                    if (strTextSearch != "")
                                    {
                                        strActiveValue = ddlParentSearch.SelectedValue;
                                        bFromView = false;
                                    }
                                    else
                                    {
                                        strActiveOperator = strViewOperator;
                                    }
                                    if (ddlParentSearch.SelectedValue == "vf_vf_vf")
                                    {
                                        strActiveValue = "";
                                        bFromView = false;
                                        strActiveOperator = "=";
                                    }
                                    UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                                }



                            }

                            if (strViewFiltervalue == null && ddlParentSearch.SelectedValue != "")
                            {
                                TextSearch = TextSearch + " AND Record." + dr["SystemName"].ToString() + " ='" + ddlParentSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                            }
                        }

                    }


                }
                //else if (dr["ColumnType"].ToString() == "listbox" && (dr["DropDownType"].ToString() == "value_text" ))
                //{


                //    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                //    if (txtSearch != null)
                //    {

                //        //if (txtSearch.Text != "")
                //        //{
                //            string strText = GetValueFromTextForList(dr["DropdownValues"].ToString(), txtSearch.Text);

                //            string strViewFiltervalue = "";
                //            if (xmlDoc != null)
                //            {
                //                strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                //                if (strViewFiltervalue != "")
                //                    UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strText, strLowerOperator, bFromView);
                //            }

                //            if (strViewFiltervalue == null && strText != "")
                //                TextSearch = TextSearch + " AND Record." + dr["SystemName"].ToString() + " like'%" + strText.Trim().Replace("'", "''") + "%'";
                //        //}
                //    }
                //}
                else if (dr["ColumnType"].ToString() == "radiobutton" || dr["ColumnType"].ToString() == "listbox"
                    || dr["ColumnType"].ToString() == "checkbox")
                {

                    DropDownList ddlSearch = (DropDownList)tblSearchControls.FindControl("ddlSearch_" + dr["SystemName"].ToString());
                    if (ddlSearch != null)
                    {
                        string strViewFiltervalue = "";
                        string strTextSearch = "";
                        strActiveOperator = strLowerOperator;

                        if (strViewOperator == "" || strViewOperator == "between")
                            strViewOperator = "=";

                        if (ddlSearch.SelectedItem != null)
                        {
                            if (ddlSearch.SelectedValue != "" && ddlSearch.SelectedValue != "vf_vf_vf")
                            {
                                strActiveOperator = "=";
                                strLowerOperator = "=";
                                if (strLowerOperator == "=")
                                {

                                    if (dr["ColumnType"].ToString() == "listbox")
                                    {
                                        strTextSearch = " CHARINDEX('," + ddlSearch.SelectedValue.Trim().Replace("'", "''") + ",' ,',' + Record." + dr["SystemName"].ToString() + " + ',')>0";
                                    }
                                    else
                                    {
                                        strTextSearch = " Record." + dr["SystemName"].ToString() + " ='" + ddlSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                                    }

                                }
                            }

                            if (xmlDoc != null)
                            {
                                strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                                if (strViewFiltervalue != null)
                                {
                                    string strActiveValue = strViewFiltervalue;

                                    if (strTextSearch != "")
                                    {
                                        strActiveValue = ddlSearch.SelectedValue;
                                        bFromView = false;
                                    }
                                    else
                                    {
                                        strActiveOperator = strViewOperator;
                                    }
                                    if (ddlSearch.SelectedValue == "vf_vf_vf")
                                    {
                                        strActiveValue = "";
                                        bFromView = false;
                                        strActiveOperator = "=";
                                    }
                                    UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                                }

                            }

                            if (strViewFiltervalue == null && ddlSearch.SelectedValue != "")
                            {
                                //TextSearch = TextSearch + " AND Record." + dr["SystemName"].ToString() + " ='" + ddlSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                                if (dr["ColumnType"].ToString() == "listbox")
                                {
                                    TextSearch = TextSearch + " AND CHARINDEX('," + ddlSearch.SelectedValue.Trim().Replace("'", "''") + ",' ,',' + Record." + dr["SystemName"].ToString() + " + ',')>0";
                                }
                                else
                                {
                                    TextSearch = TextSearch + " AND Record." + dr["SystemName"].ToString() + " ='" + ddlSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                                }

                            }

                        }
                    }
                }
                else
                {
                    TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SystemName"].ToString());
                    if (txtSearch != null)
                    {

                        string strViewFiltervalue = "";

                        string strTextSearch = "";
                        strActiveOperator = strLowerOperator;

                        if (strViewOperator == "" || strViewOperator == "between")
                            strViewOperator = "=";

                        if (txtSearch.Text != "")
                        {
                            strActiveOperator = "=";
                            strLowerOperator = "=";
                            if (strLowerOperator == "=")
                            {
                                strTextSearch = " Record." + dr["SystemName"].ToString() + " LIKE'%" + txtSearch.Text.Trim().Replace("'", "''") + "%'";
                            }
                        }

                        if (xmlDoc != null)
                        {
                            strViewFiltervalue = GetValueFromViewFilter(xmlDoc, dr["ColumnID"].ToString());
                            if (strViewFiltervalue != null)
                            {
                                string strActiveValue = strViewFiltervalue;

                                if (strTextSearch != "")
                                {
                                    strActiveValue = txtSearch.Text;
                                    bFromView = false;
                                }
                                else
                                {
                                    strActiveOperator = strViewOperator;
                                }

                                UpdateViewFilterControlsInfo(ref xmlDoc, dr["ColumnID"].ToString(), strActiveValue, strActiveOperator, bFromView);
                            }

                        }

                        if (strViewFiltervalue == null)
                        {
                            if (strTextSearch != "")
                                TextSearch = TextSearch + " AND " + strTextSearch;
                        }
                    }

                }

            }

            //

            foreach (DataRow dr in _dtSearchGroup.Rows)
            {

                TextBox txtSearch = (TextBox)tblSearchControls.FindControl("txtSearch_" + dr["SearchGroupID"].ToString());
                if (txtSearch != null)
                {
                    if (txtSearch.Text != "")
                    {

                        DataTable dtSearchGroupClumn = Common.DataTableFromText(" SELECT * FROM SearchGroupColumn WHERE SearchGroupID=" + dr["SearchGroupID"].ToString());
                        foreach (DataRow drC in dtSearchGroupClumn.Rows)
                        {
                            PopulateSearchGroup(int.Parse(drC["ColumnID"].ToString()), ref txtSearch);
                        }
                    }
                }
            }
        }




        //if (tblFilterByColumn.Visible && ddlFilterValue.SelectedIndex != 0)
        //{
        //    if (ddlFilterValue.SelectedValue != "" && hfFilterColumnSystemName.Value != "")
        //    {
        //        TextSearch = TextSearch + " AND Record." + hfFilterColumnSystemName.Value + "='" + ddlFilterValue.SelectedValue.Trim().Replace("'", "''") + "'";
        //    }
        //}

        //if (tblFilterByColumn.Visible && cbcvSumFilter.ddlYAxisV != "" && hfFilterColumnSystemName.Value != "" && cbcvSumFilter.GetValue != "")
        //{

        //    TextSearch = TextSearch + " AND Record." + hfFilterColumnSystemName.Value + "='" + cbcvSumFilter.GetValue.Trim().Replace("'", "''") + "'";

        //}

        //lets check 4 search control

        if (_theView != null)
        {
            if (chkShowAdvancedOptions.Checked == false)
            {
                if (xmlDoc != null)
                {
                    Pages_UserControl_ViewDetail vdFilter = new Pages_UserControl_ViewDetail();
                    vdFilter = (Pages_UserControl_ViewDetail)LoadControl("~/Pages/UserControl/ViewDetail.ascx");
                    vdFilter.PopulateFilterControl(xmlDoc.OuterXml, ((int)_theView.TableID));

                    TextSearch = TextSearch + " " + vdFilter.GetViewFilter();
                    vdFilter = null;
                }
                else
                {
                    TextSearch = TextSearch + " " + _theView.Filter;
                }
            }
            else
            {
                TextSearch = TextSearch + " " + _theView.Filter;
            }

        }

        if (chkShowAdvancedOptions.Checked)
        {
            //int iSearchCount = 0;
            string strTSA = "";
            string strTSB = "";
            string strTSC = "";
            string strTSD = "";



            string strAO1 = "";
            string strAO2 = "";
            string strAO3 = "";

            string strSearchMainTextSearch = cbcSearchMain.TextSearch;
            if (!string.IsNullOrEmpty(strSearchMainTextSearch))
            {
                strTSA = "(" + strSearchMainTextSearch + ")";
            }

            //if (cbcSearchMain.NumericSearch != "" && cbcSearchMain.NumericSearch != null)
            //{
            //    strTSA = "(" + cbcSearchMain.NumericSearch + ")";
            //}




            string strAndOr1 = hfAndOr1.Value;
            string strAndOr2 = hfAndOr2.Value;
            string strAndOr3 = hfAndOr3.Value;

            if (strAndOr1 != "")
                lnkAndOr1.Text = strAndOr1;

            if (strAndOr2 != "")
                lnkAndOr2.Text = strAndOr2;

            if (strAndOr3 != "")
                lnkAndOr3.Text = strAndOr3;


            string strSearch1TextSearch = cbcSearch1.TextSearch;

            if (!string.IsNullOrEmpty(strSearch1TextSearch) && hfAndOr1.Value != "")
            {
                if (strTSA == "")
                {
                    strTSA = "(" + strSearch1TextSearch + ")";
                }
                else
                {
                    strTSB = "(" + strSearch1TextSearch + ")";
                }

                strAO1 = hfAndOr1.Value;

            }

            //if (cbcSearch1.NumericSearch != "" && cbcSearch1.NumericSearch != null && hfAndOr1.Value != "")
            //{
            //    if (strTSA == "")
            //    {
            //        strTSA = "(" + cbcSearch1.NumericSearch + ")";
            //    }
            //    else
            //    {
            //        strTSB = "(" + cbcSearch1.NumericSearch + ")";
            //    }

            //    strAO1 = hfAndOr1.Value;

            //}




            string strSearch2TextSearch = cbcSearch2.TextSearch;
            if (!string.IsNullOrEmpty(strSearch2TextSearch) && hfAndOr2.Value != "")
            {

                if (strTSA == "")
                {
                    strTSA = "(" + strSearch2TextSearch + ")";
                }
                else if (strTSB == "")
                {
                    strTSB = "(" + strSearch2TextSearch + ")";
                }
                else
                {
                    strTSC = "(" + strSearch2TextSearch + ")";
                }

                if (strAO1 == "")
                {
                    strAO1 = hfAndOr2.Value;
                }
                else
                {
                    strAO2 = hfAndOr2.Value;
                }

            }

            //if (cbcSearch2.NumericSearch != "" && cbcSearch2.NumericSearch != null && hfAndOr2.Value != "")
            //{

            //    if (strTSA == "")
            //    {
            //        strTSA = "(" + cbcSearch2.NumericSearch + ")";
            //    }
            //    else if (strTSB == "")
            //    {
            //        strTSB = "(" + cbcSearch2.NumericSearch + ")";
            //    }
            //    else
            //    {
            //        strTSC = "(" + cbcSearch2.NumericSearch + ")";
            //    }

            //    if (strAO1 == "")
            //    {
            //        strAO1 = hfAndOr2.Value;
            //    }
            //    else
            //    {
            //        strAO2 = hfAndOr2.Value;
            //    }

            //}




            string strSearch3TextSearch = cbcSearch3.TextSearch;

            if (!string.IsNullOrEmpty(strSearch3TextSearch) && hfAndOr3.Value != "")
            {

                if (strTSA == "")
                {
                    strTSA = "(" + strSearch3TextSearch + ")";
                }
                else if (strTSB == "")
                {
                    strTSB = "(" + strSearch3TextSearch + ")";
                }
                else if (strTSC == "")
                {
                    strTSC = "(" + strSearch3TextSearch + ")";
                }
                else
                {
                    strTSD = "(" + strSearch3TextSearch + ")";
                }


                if (strAO1 == "")
                {
                    strAO1 = hfAndOr3.Value;
                }
                else if (strAO2 == "")
                {
                    strAO2 = hfAndOr3.Value;
                }
                else
                {
                    strAO3 = hfAndOr3.Value;
                }

            }



            //if (cbcSearch3.NumericSearch != "" && cbcSearch3.NumericSearch != null && hfAndOr3.Value != "")
            //{

            //    if (strTSA == "")
            //    {
            //        strTSA = "(" + cbcSearch3.NumericSearch + ")";
            //    }
            //    else if (strTSB == "")
            //    {
            //        strTSB = "(" + cbcSearch3.NumericSearch + ")";
            //    }
            //    else if (strTSC == "")
            //    {
            //        strTSC = "(" + cbcSearch3.NumericSearch + ")";
            //    }
            //    else
            //    {
            //        strTSD = "(" + cbcSearch3.NumericSearch + ")";
            //    }


            //    if (strAO1 == "")
            //    {
            //        strAO1 = hfAndOr3.Value;
            //    }
            //    else if (strAO2 == "")
            //    {
            //        strAO2 = hfAndOr3.Value;
            //    }
            //    else
            //    {
            //        strAO3 = hfAndOr3.Value;
            //    }

            //}



            string stringTotalTS = " AND ((([AAAAAAA] [AO1] [BBBBBBB]) [AO2] [CCCCCCC]) [AO3] [DDDDDDD])";
            string strOneOne = " 1=1 ";

            if (strTSA == "")
                strTSA = strOneOne;
            if (strTSB == "")
                strTSB = strOneOne;
            if (strTSC == "")
                strTSC = strOneOne;
            if (strTSD == "")
                strTSD = strOneOne;

            if (strAO1 == "")
                strAO1 = " AND ";

            if (strAO2 == "")
                strAO2 = " AND ";

            if (strAO3 == "")
                strAO3 = " AND ";

            stringTotalTS = stringTotalTS.Replace("[AO1]", strAO1);
            stringTotalTS = stringTotalTS.Replace("[AO2]", strAO2);
            stringTotalTS = stringTotalTS.Replace("[AO3]", strAO3);

            stringTotalTS = stringTotalTS.Replace("[AAAAAAA]", strTSA);
            stringTotalTS = stringTotalTS.Replace("[BBBBBBB]", strTSB);
            stringTotalTS = stringTotalTS.Replace("[CCCCCCC]", strTSC);
            stringTotalTS = stringTotalTS.Replace("[DDDDDDD]", strTSD);

            TextSearch = TextSearch + stringTotalTS;

        }

        TextSearch = TextSearch + hfTextSearch.Value;

        if ((bool)_theUserRole.IsAdvancedSecurity)
        {
            if (_strRecordRightID == Common.UserRoleType.OwnData)
            {
                //TextSearch = TextSearch + " AND Record.OwnerUserID=" + _ObjUser.UserID.ToString();
                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
            }

        }
        else
        {
            if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
            {
                //TextSearch = TextSearch + " AND Record.OwnerUserID=" + _ObjUser.UserID.ToString();
                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
            }


        }

        //lets play with data scope

        if (_theAccount.UseDataScope != null)
        {
            if ((bool)_theAccount.UseDataScope)
            {
                if (_theUserRole.DataScopeColumnID != null && _theUserRole.DataScopeValue != "")
                {
                    //this user need Data Scope

                    Column theDataScopeColumn = RecordManager.ets_Column_Details((int)_theUserRole.DataScopeColumnID);

                    if (theDataScopeColumn != null)
                    {

                        TextSearch = TextSearch + GetDataScopeWhere((int)theDataScopeColumn.TableID, (int)theDataScopeColumn.TableID, theDataScopeColumn.SystemName);

                    }

                }
            }
        }

        if (TextSearchParent == null)
            TextSearchParent = "";

        //ddlActiveFilter.SelectedValue == "-1" ? null : (bool?)bool.Parse(ddlActiveFilter.SelectedValue)


        PopulateDateAddedSearch();



        string strSummaryTableFilterText = SystemData.SystemOption_ValueByKey_Account("SummaryTableFilterText", null, int.Parse(TableID.ToString()));

        if (strSummaryTableFilterText != "")
            TextSearch = TextSearch + " " + strSummaryTableFilterText;


        //if (Request.QueryString["viewname"] == null)
        //{
        //    TextSearch = TextSearch + " " + SystemData.SystemOption_ValueByKey_Account("NoViewName", null, int.Parse(TableID.ToString()));
        //}
        //else
        //{
        //    TextSearch = TextSearch + " " + SystemData.SystemOption_ValueByKey_Account(Request.QueryString["viewname"].ToString(), null, int.Parse(TableID.ToString()));

        //}

        if (chkShowAdvancedOptions.Checked && ddlUploadedBatch.SelectedValue != "")
        {
            TextSearch = TextSearch + "  AND Record.BatchID=" + ddlUploadedBatch.SelectedValue;
        }

        if (TextSearch.Trim() == "")
            TextSearch = "";







    }

    protected void UpdateViewFilterControlsInfo(ref System.Xml.XmlDocument xmlDoc, string strColumnID, string strValue, string strOperator, bool bFromView)
    {
        if (xmlDoc == null || xmlDoc.FirstChild == null)
            return;


        try
        {


            if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"] != null &&
                    xmlDoc.FirstChild["cbcSearchMain_TextValue"] != null && xmlDoc.FirstChild["cbcSearchMain_CompareOperator"] != null)
            {
                if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"].InnerText == strColumnID)
                {
                    xmlDoc.FirstChild["cbcSearchMain_TextValue"].InnerText = strValue;
                    if (strOperator != "")
                        xmlDoc.FirstChild["cbcSearchMain_CompareOperator"].InnerText = strOperator;

                    if (bFromView)
                    {
                        return;
                    }

                }
            }
            if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch1_TextValue"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"].InnerText == strColumnID)
                {
                    xmlDoc.FirstChild["cbcSearch1_TextValue"].InnerText = strValue;
                    if (strOperator != "")
                        xmlDoc.FirstChild["cbcSearch1_CompareOperator"].InnerText = strOperator;
                    if (bFromView)
                    {
                        return;
                    }
                    else
                    {
                        if (xmlDoc.FirstChild["hfAndOr1"] != null)
                            xmlDoc.FirstChild["hfAndOr1"].InnerText = "and";
                    }
                }
            }

            if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch2_TextValue"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"].InnerText == strColumnID)
                {
                    xmlDoc.FirstChild["cbcSearch2_TextValue"].InnerText = strValue;
                    if (strOperator != "")
                        xmlDoc.FirstChild["cbcSearch2_CompareOperator"].InnerText = strOperator;
                    if (bFromView)
                    {
                        return;
                    }
                    else
                    {
                        if (xmlDoc.FirstChild["hfAndOr2"] != null)
                            xmlDoc.FirstChild["hfAndOr2"].InnerText = "and";
                    }
                }
            }


            if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch3_TextValue"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"].InnerText == strColumnID)
                {
                    xmlDoc.FirstChild["cbcSearch3_TextValue"].InnerText = strValue;
                    if (strOperator != "")
                        xmlDoc.FirstChild["cbcSearch3_CompareOperator"].InnerText = strOperator;
                    if (bFromView)
                    {
                        return;
                    }
                    else
                    {
                        if (xmlDoc.FirstChild["hfAndOr3"] != null)
                            xmlDoc.FirstChild["hfAndOr3"].InnerText = "and";
                    }
                }
            }

        }
        catch
        {
            //
        }

        return;
    }


    protected void RemoveViewFilterControlsInfo(ref System.Xml.XmlDocument xmlDoc, string strColumnID)
    {
        if (xmlDoc == null || xmlDoc.FirstChild == null)
            return;
        try
        {
            if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"] != null &&
                    xmlDoc.FirstChild["cbcSearchMain_TextValue"] != null && xmlDoc.FirstChild["cbcSearchMain_CompareOperator"] != null)
            {
                if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"].InnerText == strColumnID)
                {
                    xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"].InnerText = "";
                    xmlDoc.FirstChild["cbcSearchMain_TextValue"].InnerText = "";
                    xmlDoc.FirstChild["cbcSearchMain_CompareOperator"].InnerText = "";
                }
            }
            if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch1_TextValue"] != null)
            {
                if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"].InnerText == strColumnID)
                {
                    xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"].InnerText = "";
                    xmlDoc.FirstChild["cbcSearch1_TextValue"].InnerText = "";
                    xmlDoc.FirstChild["cbcSearch1_CompareOperator"].InnerText = "";
                    if (xmlDoc.FirstChild["hfAndOr1"] != null)
                        xmlDoc.FirstChild["hfAndOr1"].InnerText = "and";
                }
            }

            if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch2_TextValue"] != null)
            {
                if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"].InnerText == strColumnID)
                {
                    xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"].InnerText = "";
                    xmlDoc.FirstChild["cbcSearch2_TextValue"].InnerText = "";
                    xmlDoc.FirstChild["cbcSearch2_CompareOperator"].InnerText = "";
                    if (xmlDoc.FirstChild["hfAndOr2"] != null)
                        xmlDoc.FirstChild["hfAndOr2"].InnerText = "and";
                }
            }


            if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch3_TextValue"] != null)
            {
                if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"].InnerText == strColumnID)
                {
                    xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"].InnerText = "";
                    xmlDoc.FirstChild["cbcSearch3_TextValue"].InnerText = "";
                    xmlDoc.FirstChild["cbcSearch3_CompareOperator"].InnerText = "";
                    if (xmlDoc.FirstChild["hfAndOr3"] != null)
                        xmlDoc.FirstChild["hfAndOr3"].InnerText = "and";
                }
            }

        }
        catch
        {
            //
        }

        return;
    }



    //protected void  UpdateViewFilterControlsInfo(ref System.Xml.XmlDocument xmlDoc, string strColumnID,string strValue)
    //{
    //    try
    //    {
    //        if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"] != null &&
    //                xmlDoc.FirstChild["cbcSearchMain_TextValue"] != null)
    //        {               
    //            if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"].InnerText == strColumnID)
    //            {
    //                xmlDoc.FirstChild["cbcSearchMain_TextValue"].InnerText = strValue;
    //                return;
    //            }
    //        }
    //        if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"] != null &&
    //               xmlDoc.FirstChild["cbcSearch1_TextValue"] != null)
    //        {

    //            if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"].InnerText == strColumnID)
    //            {
    //                xmlDoc.FirstChild["cbcSearch1_TextValue"].InnerText=strValue;
    //                return;
    //            }
    //        }

    //        if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"] != null &&
    //               xmlDoc.FirstChild["cbcSearch2_TextValue"] != null)
    //        {

    //            if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"].InnerText == strColumnID)
    //            {
    //                 xmlDoc.FirstChild["cbcSearch2_TextValue"].InnerText=strValue;
    //                 return;
    //            }
    //        }


    //        if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"] != null &&
    //               xmlDoc.FirstChild["cbcSearch3_TextValue"] != null)
    //        {

    //            if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"].InnerText == strColumnID)
    //            {
    //                xmlDoc.FirstChild["cbcSearch3_TextValue"].InnerText=strValue;
    //                return;
    //            }
    //        }

    //    }
    //    catch
    //    {
    //        //
    //    }

    //    return;
    //}
    protected string GetValueFromViewFilter(System.Xml.XmlDocument xmlDoc, string strColumnID)
    {
        try
        {
            if (xmlDoc.OuterXml == "")
                return null;

            if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"] != null &&
                    xmlDoc.FirstChild["cbcSearchMain_TextValue"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"].InnerText == strColumnID)
                {
                    return xmlDoc.FirstChild["cbcSearchMain_TextValue"].InnerText;
                }
            }
            if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch1_TextValue"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"].InnerText == strColumnID)
                {
                    return xmlDoc.FirstChild["cbcSearch1_TextValue"].InnerText;
                }
            }

            if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch2_TextValue"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"].InnerText == strColumnID)
                {
                    return xmlDoc.FirstChild["cbcSearch2_TextValue"].InnerText;
                }
            }


            if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch3_TextValue"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"].InnerText == strColumnID)
                {
                    return xmlDoc.FirstChild["cbcSearch3_TextValue"].InnerText;
                }
            }

        }
        catch
        {
            //
        }

        return null;
    }



    protected string GetOneCondition(string strOneOperator, string strOneValue, string strColumnID, string strVT)
    {
        string strOneCondition = "";
        if (strOneValue.IndexOf("____") > -1)
        {
            if (strOneOperator == "between")
            {
                strOneValue = strOneValue.Replace("____", " to ");
            }
            else
            {
                strOneValue = strOneValue.Replace("____", "");
            }

        }

        if (strOneValue != "" && strVT != "")
        {
            if (strVT == "ColumnID" && strColumnID != "")
            {
                try
                {
                    Column theColumn = RecordManager.ets_Column_Details(int.Parse(strColumnID));
                    if (theColumn != null && theColumn.TableTableID != null && theColumn.DisplayColumn != "" && theColumn.LinkedParentColumnID != null
                        && (int)theColumn.TableTableID > -1)
                    {
                        //Column theLinkedParentColumn = RecordManager.ets_Column_Details(((int)theColumn.LinkedParentColumnID));

                        // string strFieldsToShow = RecordManager.fnReplaceDisplayColumns(theColumn.DisplayColumn, (int)theColumn.TableTableID, theColumn.ColumnID);

                        //string strFieldsToShow = RecordManager.fnReplaceDisplayColumns_NoAlias((int)theColumn.ColumnID);

                        //if (theLinkedParentColumn != null && strFieldsToShow != "")
                        //{
                        //string strFilterSQL = "";

                        if (strOneValue == "-user-")
                        {
                            //need RnD
                            Column theLinkedParentColumn = RecordManager.ets_Column_Details(((int)theColumn.LinkedParentColumnID));
                            string strFieldsToShow = RecordManager.fnReplaceDisplayColumns_NoAlias((int)theColumn.ColumnID);

                            string strFilterSQL = "";
                            string strColumnuserID = Common.GetValueFromSQL(@"SELECT TOP 1 ColumnID FROM [Column] WHERE TableID=" + theColumn.TableTableID.ToString() + @" AND 
                                ColumnType='dropdown' AND DisplayColumn IS NOT NULL
                                            AND TableTableID=-1");

                            if (strColumnuserID != "")
                            {
                                string strLoginText = "";
                                SecurityManager.ProcessLoginUserDefault(theColumn.TableTableID.ToString(), "",
                                theColumn.LinkedParentColumnID.ToString(), _ObjUser.UserID.ToString(), ref strOneValue, ref strLoginText);

                            }

                            if (theLinkedParentColumn.SystemName.ToLower() == "recordid")
                            {
                                strFilterSQL = strOneValue;
                            }
                            else
                            {
                                strFilterSQL = "'" + strOneValue.Replace("'", "''") + "'";
                            }

                            string strDisplayColumnText = Common.GetValueFromSQL("SELECT (" + strFieldsToShow + ") as DisplayColumnText FROM Record WHERE TableID=" + theColumn.TableTableID.ToString() + " AND " + theLinkedParentColumn.SystemName + "=" + strFilterSQL);

                            if (strDisplayColumnText != "")
                            {
                                strOneValue = strDisplayColumnText;
                            }
                        }
                        else
                        {
                            strOneValue = Common.GetLinkedDisplayText(theColumn.DisplayColumn, (int)theColumn.TableTableID, null, " AND Record.RecordID=" + strOneValue, "");
                        }

                        //if (theLinkedParentColumn.SystemName.ToLower() == "recordid")
                        //{
                        //    strFilterSQL = strOneValue;
                        //}
                        //else
                        //{
                        //    strFilterSQL = "'" + strOneValue.Replace("'", "''") + "'";
                        //}

                        //string strDisplayColumnText = Common.GetValueFromSQL("SELECT (" + strFieldsToShow + ") as DisplayColumnText FROM Record WHERE TableID=" + theColumn.TableTableID.ToString() + " AND " + theLinkedParentColumn.SystemName + "=" + strFilterSQL);

                        //if (strDisplayColumnText != "")
                        //{
                        //    strOneValue = strDisplayColumnText;
                        //}

                        //}
                    }
                    else if (theColumn != null && theColumn.TableTableID != null && theColumn.DisplayColumn != "" && (int)theColumn.TableTableID == -1)
                    {
                        if (strOneValue != "")
                            strOneValue = RecordManager.fnGetSystemUserDisplayText(theColumn.DisplayColumn, strOneValue);

                    }

                }
                catch
                {
                    //
                }


            }
            else
            {
                strOneValue = Common.GetTextFromValue(strVT, strOneValue);
            }
        }

        if (strOneOperator == "between")
        {
            strOneCondition = strOneValue;
        }
        else if (strOneOperator == "empty")
        {
            strOneCondition = "Empty";
        }
        else if (strOneOperator == "notempty")
        {
            strOneCondition = "Not Empty";
        }
        else if (strOneOperator == "<>")
        {
            strOneCondition = "Not Equal " + strOneValue;
        }
        else if (strOneOperator == "=")
        {
            strOneCondition = strOneValue;
        }
        else
        {
            strOneCondition = strOneOperator + " " + strOneValue;
        }
        return strOneCondition;
    }

    protected bool ThisIsOR(string strWaterViewmark)
    {
        if (strWaterViewmark.Trim() == "")
        {
            return false;
        }
        else
        {
            if (strWaterViewmark.Length > 2)
            {
                if (strWaterViewmark.Substring(0, 2) == "OR")
                {
                    return true;
                }
            }
        }
        return false;
    }
    protected string GetMultipleCondition(System.Xml.XmlDocument xmlDoc, string strColumnID, ref int iConditionCount, string strVT)
    {
        iConditionCount = 0;
        string strConditions = "";
        string strJoinOperator = "";
        try
        {
            if (xmlDoc.OuterXml == "")
                return "";


            string strEachCondition = "";
            //string strOneOperator = "=";
            //string strOneValue = "";
            if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"] != null &&
                    xmlDoc.FirstChild["cbcSearchMain_TextValue"] != null && xmlDoc.FirstChild["cbcSearchMain_CompareOperator"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"].InnerText == strColumnID)
                {
                    iConditionCount = iConditionCount + 1;
                    strEachCondition = GetOneCondition(xmlDoc.FirstChild["cbcSearchMain_CompareOperator"].InnerText,
                        xmlDoc.FirstChild["cbcSearchMain_TextValue"].InnerText, strColumnID, strVT);
                    if (strEachCondition != "")
                        strConditions = "(" + strEachCondition + ")";


                }
            }
            if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch1_TextValue"] != null && xmlDoc.FirstChild["cbcSearch1_CompareOperator"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"].InnerText == strColumnID)
                {
                    iConditionCount = iConditionCount + 1;
                    strEachCondition = GetOneCondition(xmlDoc.FirstChild["cbcSearch1_CompareOperator"].InnerText,
                      xmlDoc.FirstChild["cbcSearch1_TextValue"].InnerText, strColumnID, strVT);
                    if (strEachCondition != "")
                    {
                        if (strConditions != "")
                        {
                            if (xmlDoc.FirstChild["hfAndOr1"] != null)
                            {

                                strConditions = strConditions + " " + xmlDoc.FirstChild["hfAndOr1"].InnerText + " ";
                            }
                        }
                        else
                        {
                            if (xmlDoc.FirstChild["hfAndOr1"] != null)
                                strJoinOperator = xmlDoc.FirstChild["hfAndOr1"].InnerText;
                        }

                        strConditions = strConditions + "(" + strEachCondition + ")";
                    }

                }
            }

            if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch2_TextValue"] != null && xmlDoc.FirstChild["cbcSearch2_CompareOperator"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"].InnerText == strColumnID)
                {
                    iConditionCount = iConditionCount + 1;
                    strEachCondition = GetOneCondition(xmlDoc.FirstChild["cbcSearch2_CompareOperator"].InnerText,
                        xmlDoc.FirstChild["cbcSearch2_TextValue"].InnerText, strColumnID, strVT);
                    if (strEachCondition != "")
                    {
                        if (strConditions != "")
                        {
                            if (xmlDoc.FirstChild["hfAndOr2"] != null)
                            {
                                strConditions = strConditions + " " + xmlDoc.FirstChild["hfAndOr2"].InnerText + " ";
                            }
                        }
                        else
                        {
                            if (xmlDoc.FirstChild["hfAndOr2"] != null)
                                strJoinOperator = xmlDoc.FirstChild["hfAndOr2"].InnerText;
                        }

                        strConditions = strConditions + "(" + strEachCondition + ")";
                    }
                }
            }


            if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch3_TextValue"] != null && xmlDoc.FirstChild["cbcSearch3_CompareOperator"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"].InnerText == strColumnID)
                {
                    iConditionCount = iConditionCount + 1;
                    strEachCondition = GetOneCondition(xmlDoc.FirstChild["cbcSearch3_CompareOperator"].InnerText,
                        xmlDoc.FirstChild["cbcSearch3_TextValue"].InnerText, strColumnID, strVT);
                    if (strEachCondition != "")
                    {
                        if (strConditions != "")
                        {
                            if (xmlDoc.FirstChild["hfAndOr3"] != null)
                            {
                                strConditions = strConditions + " " + xmlDoc.FirstChild["hfAndOr3"].InnerText + " ";
                            }
                        }
                        else
                        {
                            if (xmlDoc.FirstChild["hfAndOr3"] != null)
                                strJoinOperator = xmlDoc.FirstChild["hfAndOr3"].InnerText;
                        }

                        strConditions = strConditions + "(" + strEachCondition + ")";
                    }
                }
            }

        }
        catch
        {
            //
        }

        if (strConditions != "" && iConditionCount == 1 && strJoinOperator.Trim() == "or")
        {
            strConditions = "OR " + strConditions;
        }

        return strConditions;
    }
    protected string GetOperatorFromViewFilter(System.Xml.XmlDocument xmlDoc, string strColumnID)
    {
        try
        {
            if (xmlDoc.OuterXml == "")
                return "";

            if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"] != null &&
                    xmlDoc.FirstChild["cbcSearchMain_CompareOperator"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearchMain_ddlYAxisV"].InnerText == strColumnID)
                {
                    return xmlDoc.FirstChild["cbcSearchMain_CompareOperator"].InnerText;
                }
            }
            if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch1_CompareOperator"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch1_ddlYAxisV"].InnerText == strColumnID)
                {
                    return xmlDoc.FirstChild["cbcSearch1_CompareOperator"].InnerText;
                }
            }

            if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch2_CompareOperator"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch2_ddlYAxisV"].InnerText == strColumnID)
                {
                    return xmlDoc.FirstChild["cbcSearch2_CompareOperator"].InnerText;
                }
            }


            if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"] != null &&
                   xmlDoc.FirstChild["cbcSearch3_CompareOperator"] != null)
            {

                if (xmlDoc.FirstChild["cbcSearch3_ddlYAxisV"].InnerText == strColumnID)
                {
                    return xmlDoc.FirstChild["cbcSearch3_CompareOperator"].InnerText;
                }
            }

        }
        catch
        {
            //
        }

        return "";
    }
    protected void ibEmail_Click(object sender, ImageClickEventArgs e)
    {
        ExportExcelorCSV(sender, e, "email");

    }
    //protected void ibEmail_Click_2(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        lblMsg.Text = "";
    //        //string strFolderPath = SystemData.SystemOption_ValueByKey("BulkExportPath");
    //        string strFolderPath = Server.MapPath("~\\ExportedFiles");
    //        //string strFileName = Guid.NewGuid().ToString() + "_" + lblTitle.Text.Replace(" ", "").ToString() + ".csv";
    //        string strFileName = Guid.NewGuid().ToString() + ".csv";
    //        string strFullFileName = strFolderPath + "\\" + strFileName;


    //        //gvTheGrid.AllowPaging = false;
    //        //gvTheGrid.PageIndex = 0;

    //        if (_gvPager != null)
    //        {
    //            //BindTheGridForExport(0, _gvPager.TotalRows);
    //        }
    //        else
    //        {
    //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('There is not records to email.');", true);
    //            lblMsg.Text = "There is no records to email.";
    //            return;
    //        }


    //        StringWriter sw = new StringWriter();
    //        HtmlTextWriter hw = new HtmlTextWriter(sw);

    //        int iTN = 0;
    //        gvTheGrid.PageIndex = 0;

    //        string strOrderDirection = "DESC";
    //        string sOrder = GetDataKeyNames()[0];

    //        if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
    //        {
    //            strOrderDirection = "ASC";
    //        }
    //        sOrder = gvTheGrid.GridViewSortColumn + " ";


    //        if (sOrder.Trim() == "")
    //        {
    //            sOrder = "DBGSystemRecordID";
    //        }




    //        TextSearch = TextSearch + hfTextSearch.Value;

    //        if ((bool)_theUserRole.IsAdvancedSecurity)
    //        {
    //            if (_strRecordRightID == Common.UserRoleType.OwnData)
    //            {
    //                //TextSearch = TextSearch + " AND Record.OwnerUserID=" + _ObjUser.UserID.ToString();
    //                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
    //            }
    //        }
    //        else
    //        {
    //            if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
    //            {
    //                //TextSearch = TextSearch + " AND Record.OwnerUserID=" + _ObjUser.UserID.ToString();
    //                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
    //            }
    //        }

    //        PopulateDateAddedSearch();
    //        //DataTable dt = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
    //        //        ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
    //        //        !chkIsActive.Checked,
    //        //        chkShowOnlyWarning.Checked == false ? null : (bool?)true,
    //        //        null, null,
    //        //          sOrder, strOrderDirection, 0, _gvPager.TotalRows, ref iTN, ref _iTotalDynamicColumns, "export", _strNumericSearch, TextSearch + TextSearchParent,
    //        //        _dtDateFrom, _dtDateTo, "", "", "", null);
    //        if (chkShowAdvancedOptions.Checked && ddlUploadedBatch.SelectedValue != "")
    //        {
    //            TextSearch = TextSearch + "  AND Record.TempRecordID IN  (SELECT TempRecord.RecordID FROM TempRecord WHERE TempRecord.BatchID=" + ddlUploadedBatch.SelectedValue + ")";
    //        }

    //        DataTable dt = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
    //               ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
    //               !chkIsActive.Checked,
    //               chkShowOnlyWarning.Checked == false ? null : (bool?)true,
    //               null, null,
    //                 sOrder, strOrderDirection, 0, _gvPager.TotalRows, ref iTN, ref _iTotalDynamicColumns,
    //                 _strListType, _strNumericSearch, TextSearch + TextSearchParent,
    //               _dtDateFrom, _dtDateTo, "", "", "", int.Parse(hfViewID.Value));


    //        _dtRecordColums = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));


    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = 0; j < dt.Columns.Count; j++)
    //            {
    //                //if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "data_retriever")
    //                //{
    //                //    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                //    {
    //                //        DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail(int.Parse(_dtRecordColums.Rows[i]["DataRetrieverID"].ToString()), null, null);

    //                //        if (theDataRetriever.CodeSnippet != "")
    //                //        {
    //                //            foreach (DataRow drDS in dt.Rows)
    //                //            {
    //                //                if (drDS["DBGSystemRecordID"].ToString() != "")
    //                //                {
    //                //                    drDS[dt.Columns[j].ColumnName] = Common.GetValueFromSQL(theDataRetriever.CodeSnippet.Replace("#ID#",
    //                //                        drDS["DBGSystemRecordID"].ToString()));
    //                //                }
    //                //            }

    //                //        }
    //                //    }
    //                //}


    //                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "calculation")
    //                {
    //                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                    {
    //                        if (_dtRecordColums.Rows[i]["Calculation"] != DBNull.Value)
    //                        {

    //                            bool bDateCal = false;
    //                            if (_dtRecordColums.Rows[i]["TextType"] != DBNull.Value
    //                                && _dtRecordColums.Rows[i]["TextType"].ToString().ToLower() == "d")
    //                            {
    //                                bDateCal = true;
    //                            }

    //                            foreach (DataRow drDS in dt.Rows)
    //                            {
    //                                if (drDS["DBGSystemRecordID"].ToString() != "")
    //                                {
    //                                    try
    //                                    {
    //                                        if (bDateCal == true)
    //                                        {
    //                                            string strCalculation = _dtRecordColums.Rows[i]["Calculation"].ToString();
    //                                            drDS[_dtDataSource.Columns[j].ColumnName] = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, int.Parse(drDS["DBGSystemRecordID"].ToString()),_iParentRecordID,
    //                                                _dtRecordColums.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtRecordColums.Rows[i]["DateCalculationType"].ToString());
    //                                        }
    //                                        else
    //                                        {
    //                                            string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtRecordColums.Rows[i]["Calculation"].ToString());

    //                                            //drDS[dt.Columns[j].ColumnName] = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + drDS["DBGSystemRecordID"].ToString());

    //                                            drDS[dt.Columns[j].ColumnName] = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, int.Parse(drDS["DBGSystemRecordID"].ToString()), i, _iParentRecordID);

    //                                        }
    //                                        if (bDateCal == false && _dtRecordColums.Rows[i]["IsRound"] != DBNull.Value && _dtRecordColums.Rows[i]["RoundNumber"] != DBNull.Value)
    //                                        {
    //                                            drDS[dt.Columns[j].ColumnName] = Math.Round(double.Parse(drDS[dt.Columns[j].ColumnName].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();

    //                                        }
    //                                    }
    //                                    catch
    //                                    {
    //                                        //
    //                                    }
    //                                }
    //                            }

    //                        }
    //                    }
    //                }

    //            }
    //        }

    //        //}
    //        dt.AcceptChanges();


    //        if (chkShowAdvancedOptions.Checked == true)//_bDynamicSearch
    //        {
    //            //if (ddlYAxis.SelectedValue != "")
    //            //{
    //            //    Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlYAxis.SelectedValue));
    //            //    if (theColumn != null)
    //            //    {
    //            //        if (theColumn.ColumnType == "calculation")
    //            //        {
    //            //            if (txtSearchText.Text != "")
    //            //            {
    //            //                DataView dtView = new DataView(dt);

    //            //                dtView.RowFilter = theColumn.DisplayTextSummary + " LIKE '%" + txtSearchText.Text.Replace("'", "''") + "%'";
    //            //                dt = dtView.ToTable();
    //            //            }
    //            //        }

    //            //    }
    //            //}

    //        }




    //        DataRow drFooter = dt.NewRow();

    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = 0; j < dt.Columns.Count; j++)
    //            {
    //                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                {
    //                    if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
    //                    {

    //                        //drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = dt.Compute("SUM([" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "])", "[" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "]<>''");
    //                        drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(dt, dt.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));

    //                    }
    //                }

    //            }

    //        }

    //        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //        {
    //            for (int j = dt.Columns.Count - 1; j >= 0; j--)
    //            {
    //                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                {
    //                    if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
    //                    {
    //                        if (!Common.HaveAccess(_strRecordRightID, "1,2"))
    //                        {
    //                            dt.Columns.RemoveAt(j);
    //                        }
    //                    }
    //                }

    //            }

    //        }

    //        for (int j = dt.Columns.Count - 1; j >= 0; j--)
    //        {
    //            if (dt.Columns[j].ColumnName.IndexOf("_ID**") > -1)
    //            {
    //                dt.Columns.RemoveAt(j);
    //            }

    //        }

    //        dt.Rows.Add(drFooter);

    //        //Round export

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
    //            {
    //                for (int j = 0; j < dt.Columns.Count; j++)
    //                {
    //                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                    {
    //                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
    //                        {

    //                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file"
    //                            || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "image")
    //                            {
    //                                if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
    //                                {
    //                                    try
    //                                    {
    //                                        if (dr[j].ToString().Length > 37)
    //                                        {
    //                                            dr[j] = dr[j].ToString().Substring(37);

    //                                        }
    //                                    }
    //                                    catch
    //                                    {

    //                                        //
    //                                    }
    //                                }

    //                            }

    //                            if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text"
    //                            && (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
    //                             || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "radiobutton")
    //                           && _dtRecordColums.Rows[i]["DropdownValues"].ToString() != "")
    //                            {

    //                                if (dr[j].ToString() != "")
    //                                {
    //                                    string strText = GetTextFromValueForDD(_dtRecordColums.Rows[i]["DropdownValues"].ToString(), dr[j].ToString());
    //                                    if (strText != "")
    //                                        dr[j] = strText;
    //                                }

    //                            }

    //                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "time")
    //                            {
    //                                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                                {

    //                                    if (dr[j].ToString() != "")
    //                                    {

    //                                        TimeSpan ts = TimeSpan.Parse(dr[j].ToString());
    //                                        dr[j] = ts.ToString(@"hh\:mm");
    //                                    }
    //                                }

    //                            }

    //                            //if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "data_retriever"
    //                            //&& _dtRecordColums.Rows[i]["DataRetrieverID"] != DBNull.Value)
    //                            //{
    //                            //    if (_dtRecordColums.Rows[i]["Heading"].ToString() == dt.Columns[j].ColumnName)
    //                            //    {
    //                            //        DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail(int.Parse(_dtRecordColums.Rows[i]["DataRetrieverID"].ToString()), null, null);

    //                            //        if (theDataRetriever.CodeSnippet != "")
    //                            //        {
    //                            //            dr[j] = Common.GetValueFromSQL(theDataRetriever.CodeSnippet.Replace("#ID#",
    //                            //                dr["DBGSystemRecordID"].ToString()));
    //                            //        }
    //                            //    }
    //                            //}

    //                            //if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
    //                            //    && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
    //                            //    || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
    //                            //     && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
    //                            //    && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
    //                            //{
    //                            //    if (_dtRecordColums.Rows[i]["Heading"].ToString() == dt.Columns[j].ColumnName)
    //                            //    {

    //                            //        if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
    //                            //        {
    //                            //            try
    //                            //            {

    //                            //                Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));

    //                            //                //int iTableRecordID = int.Parse(dr[j].ToString());
    //                            //                string strLinkedColumnValue = dr[j].ToString();
    //                            //                DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
    //                            //                 + _dtRecordColums.Rows[i]["TableTableID"].ToString());

    //                            //                string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

    //                            //                foreach (DataRow dr2 in dtTableTableSC.Rows)
    //                            //                {
    //                            //                    strDisplayColumn = strDisplayColumn.Replace("[" + dr2["DisplayName"].ToString() + "]", "[" + dr2["SystemName"].ToString() + "]");

    //                            //                }
    //                            //                string sstrDisplayColumnOrg = strDisplayColumn;
    //                            //                string strFilterSQL = "";
    //                            //                if (theLinkedColumn.SystemName.ToLower() == "recordid")
    //                            //                {
    //                            //                    strFilterSQL = strLinkedColumnValue;
    //                            //                }
    //                            //                else
    //                            //                {
    //                            //                    strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
    //                            //                }

    //                            //                //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());

    //                            //                DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

    //                            //                if (dtTheRecord.Rows.Count > 0)
    //                            //                {
    //                            //                    foreach (DataColumn dc in dtTheRecord.Columns)
    //                            //                    {
    //                            //                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
    //                            //                    }
    //                            //                }
    //                            //                if (sstrDisplayColumnOrg != strDisplayColumn)
    //                            //                    dr[j] = strDisplayColumn;
    //                            //            }
    //                            //            catch
    //                            //            {
    //                            //                //
    //                            //            }


    //                            //        }
    //                            //    }

    //                            //}


    //                            if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
    //                            {
    //                                if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
    //                                {
    //                                    if (dr[j].ToString() != "")
    //                                    {
    //                                        dr[j] = Math.Round(double.Parse(dr[j].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
    //                                    }
    //                                }

    //                            }
    //                        }

    //                    }

    //                    //mm:hh
    //                    if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
    //                    {

    //                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "datetime")
    //                        {
    //                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                            {
    //                                if (dr[j].ToString().Length > 15)
    //                                {
    //                                    dr[j] = dr[j].ToString().Substring(0, 16);
    //                                }
    //                            }
    //                        }

    //                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "date")
    //                        {
    //                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
    //                            {
    //                                if (dr[j].ToString().Length > 9)
    //                                {
    //                                    dr[j] = dr[j].ToString().Substring(0, 10);
    //                                }
    //                            }
    //                        }


    //                    }

    //                }
    //            }
    //        }


    //        // First we will write the headers.

    //        int iColCount = dt.Columns.Count;


    //        for (int i = 0; i < iColCount - 2; i++)
    //        {
    //            sw.Write(dt.Columns[i]);
    //            if (i < iColCount - 3)
    //            {
    //                sw.Write(",");
    //            }

    //        }

    //        sw.Write(sw.NewLine);



    //        // Now write all the rows.


    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            for (int i = 0; i < iColCount - 2; i++)
    //            {
    //                if (!Convert.IsDBNull(dr[i]))
    //                {
    //                    sw.Write("\"" + dr[i].ToString().Replace("\"", "'") + "\"");
    //                }
    //                if (i < iColCount - 3)
    //                {
    //                    sw.Write(",");
    //                }
    //            }
    //            sw.Write(sw.NewLine);
    //        }
    //        sw.Close();

    //        FileStream Fs = new FileStream(strFullFileName, FileMode.Create);
    //        BinaryWriter BWriter = new BinaryWriter(Fs, Encoding.GetEncoding("UTF-8"));
    //        BWriter.Write(sw.ToString());
    //        BWriter.Close();
    //        Fs.Close();


    //        //now lets email this to the user.

    //        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/SendEmail.aspx?Source=" + Cryptography.Encrypt("Recordlist") + "&SearchCriteriaID=" + Cryptography.Encrypt(SearchCriteriaID.ToString()) + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&FileName=" + Cryptography.Encrypt(strFileName), false);

    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message + ex.StackTrace;
    //    }


    //}


    protected void ddlTableMenu_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordList.aspx?TableID=" + Cryptography.Encrypt(ddlTableMenu.SelectedValue), false);
    }


    protected void PopulateTerminology()
    {

        stgFieldToUpdate.InnerText = stgFieldToUpdate.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));

        //lblLocations.Text = SecurityManager.etsTerminology( Request.Path.Substring(Request.Path.LastIndexOf("/")+1), lblLocations.Text, lblLocations.Text);
        //imgSchedule.ToolTip = SecurityManager.etsTerminology( Request.Path.Substring(Request.Path.LastIndexOf("/")+1), imgSchedule.ToolTip, imgSchedule.ToolTip);
        //imgSites.ToolTip = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/")+1), imgSites.ToolTip, imgSites.ToolTip);
        //imgDoc.ToolTip = SecurityManager.etsTerminology( Request.Path.Substring(Request.Path.LastIndexOf("/")+1), imgDoc.ToolTip, imgDoc.ToolTip);

    }

    protected void PopulateMenuTableDDL()
    {
        int iTN = 0;
        ddlTableMenu.DataSource = RecordManager.ets_Table_Select(null,
                null, null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, null,
                "st.TableName", "ASC",
                null, null, ref iTN, Session["STs"].ToString());

        ddlTableMenu.DataBind();

    }


}



//public partial class Pages_UserControl_RecordList : System.Web.UI.UserControl
//{
//    protected void Page_Load(object sender, EventArgs e)
//    {

//    }
//}


//  protected string GetDataScopeWhere(int iParentTableID,int iScopeTableID,string strScopeSystemName)
//    {
//        int iCurrentTableID = int.Parse(_qsTableID);

//        DataTable dtRecordID = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + iScopeTableID + " AND  " + strScopeSystemName + "='"+_ObjUser.DataScopeValue.Replace("'","''")+"'");
//        string strScopeRecordID = "";
//        if (dtRecordID.Rows.Count > 0)
//        {
//            strScopeRecordID = dtRecordID.Rows[0][0].ToString();
//        }

//        if (iCurrentTableID == iScopeTableID)
//        {
//            return " AND Record." + strScopeSystemName + " ='" + _ObjUser.DataScopeValue + "'";
//        }
//        else
//        {
//            DataTable dtChildTable = Common.DataTableFromText("	SELECT DISTINCT ChildTableID FROM TableChild WHERE ParentTableID=" + iParentTableID);

//            bool bFoundLevel = false;

//            if (dtChildTable.Rows.Count > 0)
//            {

//                foreach (DataRow drCT in dtChildTable.Rows)
//                {
//                    if (iCurrentTableID == int.Parse(drCT[0].ToString()))
//                    {
//                        bFoundLevel = true;
//                    }
//                }

//                foreach (DataRow drCT in dtChildTable.Rows)
//                {


//                    int iChildTableID = int.Parse(drCT[0].ToString());
//                    DataTable dtChildColumn = Common.DataTableFromText(@"	SELECT ColumnID FROM [Column] WHERE ColumnType='dropdown' 
//                    AND (Dropdowntype='table' OR Dropdowntype='tabledd')
//	                AND TableID=" + iChildTableID.ToString() + @" AND TableTableID=" + iParentTableID.ToString());



//                    if (dtChildColumn.Rows.Count > 0)
//                    {



//                        //we have a column
//                        Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(dtChildColumn.Rows[0][0].ToString()));

//                        DataTable dtChildRecordID = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theChildColumn.TableID.ToString() + " AND  " + theChildColumn.SystemName + "='" + strScopeRecordID + "'");

//                        //here we will return filter SQL

//                        string strChildRecordIDs = "";
//                        foreach (DataRow dr in dtChildRecordID.Rows)
//                        {
//                            strChildRecordIDs = strChildRecordIDs + dr[0].ToString() + ",";
//                        }
//                        if (strChildRecordIDs != "")
//                            strChildRecordIDs = strChildRecordIDs.Substring(0, strChildRecordIDs.LastIndexOf(','));


//                        if (iCurrentTableID == (int)theChildColumn.TableID)
//                        {
//                            return " AND Record.RecordID IN (" + strChildRecordIDs + ")";
//                        }
//                        else
//                        {
//                            if (bFoundLevel == false)
//                            {
//                              string strWhere= GetRecursiveDataScope((int)theChildColumn.TableID, iScopeTableID, strChildRecordIDs);
//                              if (strWhere != "")
//                              {
//                                  return strWhere;
//                              }
//                            }
//                        }


//                    }
//                    else
//                    {
//                        //no link
//                    }
//                }
//            }
//            else
//            {
//                //no child tables
//            }

//        }

//        return "";
//    }

//protected string GetRecursiveDataScope(int iParentTableID,int iScopeTableID, string strReocrdIDs)
//    {
//        int iCurrentTableID = int.Parse(_qsTableID);

//        DataTable dtChildTable = Common.DataTableFromText("	SELECT DISTINCT ChildTableID FROM TableChild WHERE ParentTableID=" + iParentTableID);

//        bool bFoundLevel = false;



//        if (dtChildTable.Rows.Count > 0)
//        {

//            foreach (DataRow drCT in dtChildTable.Rows)
//            {
//                if (iCurrentTableID == int.Parse(drCT[0].ToString()))
//                {
//                    bFoundLevel = true;
//                }
//            }

//            foreach (DataRow drCT in dtChildTable.Rows)
//            {
//                int iChildTableID = int.Parse(drCT[0].ToString());
//                DataTable dtChildColumn = Common.DataTableFromText(@"	SELECT ColumnID FROM [Column] WHERE ColumnType='dropdown' 
//                    AND (Dropdowntype='table' OR Dropdowntype='tabledd')
//	                AND TableID=" + iChildTableID.ToString() + @" AND TableTableID=" + iParentTableID.ToString());

//                if (dtChildColumn.Rows.Count > 0)
//                {

//                    //we have a column
//                    Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(dtChildColumn.Rows[0][0].ToString()));

//                    DataTable dtChildRecordID = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theChildColumn.TableID.ToString() + " AND  " + theChildColumn.SystemName + " IN (" + strReocrdIDs + ")");

//                    //here we will return filter SQL

//                    string strChildRecordIDs = "";
//                    foreach (DataRow dr in dtChildRecordID.Rows)
//                    {
//                        strChildRecordIDs = strChildRecordIDs + dr[0].ToString() + ",";
//                    }
//                    if (strChildRecordIDs != "")
//                        strChildRecordIDs = strChildRecordIDs.Substring(0, strChildRecordIDs.LastIndexOf(','));


//                    if (iCurrentTableID == (int)theChildColumn.TableID)
//                    {
//                        return " AND Record.RecordID IN (" + strChildRecordIDs + ")";
//                    }
//                    else
//                    {
//                        if (bFoundLevel == false)
//                        {
//                            string strWhere = GetRecursiveDataScope((int)theChildColumn.TableID, iScopeTableID, strChildRecordIDs);
//                            if (strWhere != "")
//                            {
//                                return strWhere;
//                            }
//                        }
//                    }


//                }
//                else
//                {
//                    //no link
//                }
//            }
//        }
//        else
//        {
//            //no child tables
//        }

//        return "";
//    }






// protected void Pager_OnExportForExcel(object sender, EventArgs e)
//    {

//        DataTable dtExportColumn = RecordManager.ets_Table_Columns_Export(int.Parse(TableID.ToString()),null,null);

//        if (dtExportColumn.Rows.Count == 0)
//        {
//            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoExportMSG", "alert('Sorry it is not possible to export this table because none of the fields have been marked for export. Please check the table configuration and try again');", true);
//            return;
//        }


//        if (gvTheGrid.VirtualItemCount > Common.MaxRecordsExport)
//        {

//            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page),
//                "message_alert", "alert('There are " + gvTheGrid.VirtualItemCount.ToString() + " Records, we are going to send this file to your email address.');", true);

//            string strBulkExportPath = SystemData.SystemOption_ValueByKey("BulkExportPath");
//            string strFileName = Guid.NewGuid().ToString() + "_" + lblTitle.Text.Replace(" ", "").ToString() + ".csv";
//            string strFullFileName = strBulkExportPath + "\\" + strFileName;
//            PopulateDateAddedSearch();
//            int iIsBulkExportOK = RecordManager.ets_Record_List_BulkExport(int.Parse(TableID.ToString()),
//                ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
//                !chkIsActive.Checked,
//                chkShowOnlyWarning.Checked == false ? null : (bool?)true,
//                _dtDateFrom, _dtDateTo,
//               strFullFileName);


//            if (iIsBulkExportOK == 1)
//            {
//                //lets zip the file

//                string filename = strFullFileName;


//                FileStream infile = File.OpenRead(filename);
//                byte[] buffer = new byte[infile.Length];
//                infile.Read(buffer, 0, buffer.Length);
//                infile.Close();

//                //FileStream outfile = File.Create(Path.ChangeExtension(filename, "zip"));
//                FileStream outfile = File.Create(filename + ".zip");

//                GZipStream gzipStream = new GZipStream(outfile, CompressionMode.Compress);
//                gzipStream.Write(buffer, 0, buffer.Length);
//                gzipStream.Close();

//                //now lets email this to the user.

//                string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
//                string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
//                string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
//                string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress(strEmail);
//                msg.Subject = lblTitle.Text + " - data file";
//                msg.IsBodyHtml = true;

//                string strBulkExportHTTPPath = SystemData.SystemOption_ValueByKey("BulkExportHTTPPath");

//                string strTheBody = "<div>Please click the file to download.<a href='" + strBulkExportHTTPPath + "/" + strFileName + ".zip" + "'>" + strFileName + ".zip" + "</a></div>";

//                msg.Body = strTheBody;
//                msg.To.Add(_ObjUser.Email);

//                SmtpClient smtpClient = new SmtpClient(strEmailServer);
//                smtpClient.Timeout = 99999;
//                smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);

//#if (!DEBUG)
//                smtpClient.Send(msg);
//#endif


//                if (System.Web.HttpContext.Current.Session["AccountID"] != null)
//                {

//                    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
//                }

//            }
//            else
//            {
//                //failed
//            }

//            return;
//        }



//        Response.Clear();


//        int iTN = 0;
//        gvTheGrid.PageIndex = 0;

//        string strOrderDirection = "DESC";
//        string sOrder = GetDataKeyNames()[0];

//        if (gvTheGrid.GridViewSortDirection == SortDirection.Ascending)
//        {
//            strOrderDirection = "ASC";
//        }
//        sOrder = gvTheGrid.GridViewSortColumn + " ";


//        if (sOrder.Trim() == "")
//        {
//            sOrder = "DBGSystemRecordID";
//        }




//        TextSearch = TextSearch + hfTextSearch.Value;
//        if ((bool)_ObjUser.IsAdvancedSecurity)
//        {
//            if (_strRecordRightID == Common.UserRoleType.OwnData)
//            {
//                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
//            }
//        }
//        else
//        {
//            if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
//            {
//                TextSearch = TextSearch + " AND (Record.OwnerUserID=" + _ObjUser.UserID.ToString() + " OR Record.EnteredBy=" + _ObjUser.UserID.ToString() + ")";
//            }
//        }
//        PopulateDateAddedSearch();
//        DataTable dt = RecordManager.ets_Record_List(int.Parse(TableID.ToString()),
//                ddlEnteredBy.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEnteredBy.SelectedValue),
//                !chkIsActive.Checked,
//                chkShowOnlyWarning.Checked == false ? null : (bool?)true,
//                 null, null,
//                  sOrder, strOrderDirection, 0, _gvPager.TotalRows, ref iTN, ref _iTotalDynamicColumns, "export", _strNumericSearch, TextSearch + TextSearchParent,
//                  _dtDateFrom, _dtDateTo, "", "", "", null);




//        _dtRecordColums = RecordManager.ets_Table_Columns_Summary(TableID, int.Parse(hfViewID.Value));


//        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
//        {
//            for (int j = 0; j < dt.Columns.Count; j++)
//            {               

//                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "calculation")
//                {
//                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
//                    {

//                        if (_dtRecordColums.Rows[i]["Calculation"] != DBNull.Value)
//                        {

//                            bool bDateCal = false;
//                            if (_dtRecordColums.Rows[i]["TextType"] != DBNull.Value
//                                && _dtRecordColums.Rows[i]["TextType"].ToString().ToLower() == "d")
//                            {
//                                bDateCal = true;
//                            }


//                            foreach (DataRow drDS in dt.Rows)
//                            {
//                                if (drDS["DBGSystemRecordID"].ToString() != "")
//                                {

//                                    try
//                                    {
//                                        if (bDateCal == true)
//                                        {
//                                            string strCalculation = _dtRecordColums.Rows[i]["Calculation"].ToString();
//                                            drDS[_dtDataSource.Columns[j].ColumnName] = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, int.Parse(drDS["DBGSystemRecordID"].ToString()),_iParentRecordID,
//                                                _dtRecordColums.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtRecordColums.Rows[i]["DateCalculationType"].ToString());
//                                        }
//                                        else
//                                        {
//                                            string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtRecordColums.Rows[i]["Calculation"].ToString());

//                                            //drDS[dt.Columns[j].ColumnName] = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + drDS["DBGSystemRecordID"].ToString());
//                                            drDS[dt.Columns[j].ColumnName] = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, int.Parse(drDS["DBGSystemRecordID"].ToString()), i, _iParentRecordID);


//                                        }
//                                        if (bDateCal == false && _dtRecordColums.Rows[i]["IsRound"] != DBNull.Value && _dtRecordColums.Rows[i]["RoundNumber"] != DBNull.Value)
//                                        {
//                                            drDS[dt.Columns[j].ColumnName] = Math.Round(double.Parse(drDS[dt.Columns[j].ColumnName].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();

//                                        }
//                                    }
//                                    catch
//                                    {
//                                        //
//                                    }
//                                }
//                            }

//                        }

//                    }
//                }
//            }
//        }

//        //}
//        dt.AcceptChanges();


//        if (chkShowAdvancedOptions.Checked == true)//_bDynamicSearch
//        {


//        }



//        DataRow drFooter = dt.NewRow();

//        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
//        {
//            for (int j = 0; j < dt.Columns.Count; j++)
//            {
//                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
//                {
//                    if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
//                    {

//                        //drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = dt.Compute("SUM([" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "])", "[" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "]<>''");
//                        drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(dt, dt.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));

//                    }
//                }

//            }

//        }


//        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
//        {
//            for (int j = dt.Columns.Count - 1; j >= 0; j--)
//            {
//                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
//                {
//                    if (_dtRecordColums.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
//                    {
//                        if (!Common.HaveAccess(_strRecordRightID, "1,2"))
//                        {
//                            dt.Columns.RemoveAt(j);
//                        }
//                    }


//                }

//            }

//        }
//        dt.Rows.Add(drFooter);

//        //Round export

//        foreach (DataRow dr in dt.Rows)
//        {
//            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
//            {
//                for (int j = 0; j < dt.Columns.Count; j++)
//                {
//                    //DisplayTextSummary
//                    if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
//                    {
//                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
//                        {

//                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file"
//                                || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "image")
//                            {
//                                if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
//                                {
//                                    try
//                                    {
//                                        if (dr[j].ToString().Length > 37)
//                                        {
//                                            dr[j] = dr[j].ToString().Substring(37);

//                                        }
//                                    }
//                                    catch
//                                    {
//                                        //
//                                    }
//                                }

//                            }

//                            if (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "value_text"
//                                  && (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
//                                 || _dtRecordColums.Rows[i]["ColumnType"].ToString() == "radiobutton")
//                                 && _dtRecordColums.Rows[i]["DropdownValues"].ToString() != "")
//                            {
//                                if (_dtRecordColums.Rows[i]["Heading"].ToString() == dt.Columns[j].ColumnName)
//                                {

//                                    if (dr[j].ToString() != "")
//                                    {
//                                        string strText = GetTextFromValueForDD(_dtRecordColums.Rows[i]["DropdownValues"].ToString(), dr[j].ToString());
//                                        if (strText != "")
//                                            dr[j] = strText;
//                                    }
//                                }

//                            }

//                            if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "time")
//                            {
//                                if (_dtRecordColums.Rows[i]["Heading"].ToString() == dt.Columns[j].ColumnName)
//                                {

//                                    if (dr[j].ToString() != "" )
//                                    {

//                                        TimeSpan ts = TimeSpan.Parse(dr[j].ToString());
//                                        dr[j] = ts.ToString(@"hh\:mm");
//                                    }
//                                }

//                            }


//                            if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
//                                && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
//                                || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
//                                 && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
//                                && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
//                            {
//                                if (_dtRecordColums.Rows[i]["Heading"].ToString() == dt.Columns[j].ColumnName)
//                                {

//                                    if (dr[j].ToString() != "" && dr[j].ToString() != "&nbsp;")
//                                    {
//                                        try
//                                        {
//                                            Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));

//                                            //int iTableRecordID = int.Parse(dr[j].ToString());
//                                            string strLinkedColumnValue = dr[j].ToString();
//                                            DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
//                                             + _dtRecordColums.Rows[i]["TableTableID"].ToString());

//                                            string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

//                                            foreach (DataRow dr2 in dtTableTableSC.Rows)
//                                            {
//                                                strDisplayColumn = strDisplayColumn.Replace("[" + dr2["DisplayName"].ToString() + "]", "[" + dr2["SystemName"].ToString() + "]");

//                                            }
//                                            string sstrDisplayColumnOrg = strDisplayColumn;
//                                            string strFilterSQL = "";
//                                            if (theLinkedColumn.SystemName.ToLower() == "recordid")
//                                            {
//                                                strFilterSQL = strLinkedColumnValue;
//                                            }
//                                            else
//                                            {
//                                                strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
//                                            }

//                                            //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());
//                                            DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

//                                            if (dtTheRecord.Rows.Count > 0)
//                                            {
//                                                foreach (DataColumn dc in dtTheRecord.Columns)
//                                                {
//                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
//                                                }
//                                            }
//                                            if (sstrDisplayColumnOrg != strDisplayColumn)
//                                                dr[j] = strDisplayColumn;
//                                        }
//                                        catch
//                                        {
//                                            //
//                                        }


//                                    }
//                                }

//                            }



//                            if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
//                            {
//                                if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
//                                {
//                                    if (dr[j].ToString() != "")
//                                    {
//                                        try
//                                        {
//                                            dr[j] = Math.Round(double.Parse(Common.IgnoreSymbols( dr[j].ToString())), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
//                                        }
//                                        catch
//                                        {
//                                            //
//                                        }
//                                    }
//                                }

//                            }
//                        }

//                    }

//                    //mm:hh
//                    if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
//                    {

//                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "datetime")
//                        {
//                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
//                            {
//                                if (dr[j].ToString().Length > 15)
//                                {
//                                    dr[j] = dr[j].ToString().Substring(0, 16);
//                                }
//                            }
//                        }

//                        if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "date")
//                        {
//                            if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
//                            {
//                                if (dr[j].ToString().Length > 9)
//                                {
//                                    dr[j] = dr[j].ToString().Substring(0, 10);
//                                }
//                            }
//                        }


//                    }

//                }
//            }
//        }

//        dt.Columns.RemoveAt(dt.Columns.Count - 1);
//        dt.Columns.RemoveAt(dt.Columns.Count - 1);

//        dt.AcceptChanges();

//        DBG.Common.ExportUtil.ExportToExcel(dt, "\"" + lblTitle.Text.Replace("Records - ", "") + " " + DateTime.Today.ToString("yyyyMMdd") + ".xls" + "\"");


//    }