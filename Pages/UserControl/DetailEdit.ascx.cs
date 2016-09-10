
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using AjaxControlToolkit;
using System.Globalization;
using System.Net.Mail;
using System.CodeDom.Compiler;
using System.IO;
using System.Text.RegularExpressions;
using InnovaStudio;
using DocGen.DAL;


public partial class Pages_UserControl_DetailEdit : System.Web.UI.UserControl
{

    string _jsClickFirstTab = "";
    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";
    string _strChildSession = "";
    public string ContentPage { get; set; }
    public int TableID { get; set; }
    public int? RecordID { get; set; }
    public string Mode { get; set; }

    public bool SaveOK = false;
    public bool DoValidation = true;
    public int? FormSetFormID = null;

    public string TextSearch { get; set; }

    public bool OnlyOneRecord { get; set; }

    public string SystemName { get; set; }
    public string LinkedColumnValue { get; set; }

    public bool ShowAddButton { get; set; }
    public bool ShowEditButton { get; set; }

    public int DetailTabIndex { get; set; }

    string _strRecordFolder = "Record";
    bool _bLabelOnTop = false;


    private DataTable _dtDBTableTab;
    bool _bTableTabYes = false;


    bool _bPrivate = true;
    int _TabIndex = 0;
    string _strDynamictabPart = "";
    bool _bCustomDDL = false;

    Panel[] _pnlDetailTabD;
    HtmlTable[] _tblMainD;
    HtmlTable[] _tblLeftD;
    HtmlTable[] _tblRightD;

    //string Mode = "view";
    //int RecordID;
    Record _theRecord = null;
    Record _ParentRecord = null;
    int? _iParentRecordID = null;
    int _iNewRecordID = -1;
    Account _theAccount;
    Label[] _lbl;
    TextBox[] _txtValue;
    TextBox[] _txtValue2;
    LinkButton[] _lnkValue;
    HyperLink[] _hlValue;
    ImageButton[] _ibValue;
    //HtmlEditorExtender[] _heeValue;
    WYSIWYGEditor[] _htmValue;


    HtmlTableRow[] trX;
    HtmlTableCell[] cell;
    DropDownList[] _ddlValue;
    DropDownList[] _ddlValue2;
    RadioButtonList[] _radioList;
    ListBox[] _lstValue;
    CheckBoxList[] _cblValue;
    CheckBox[] _chkValue;
    HiddenField[] _hfValue;
    HiddenField[] _hfValue2;
    HiddenField[] _hfValue3;

    CascadingDropDown[] _ccddl;

    //HyperLink[] _hlSensorInfo;
    Image[] _imgWarning;
    Image[] _imgValues;
    //RegularExpressionValidator[] _revValue;
    //RequiredFieldValidator[] _rfvValue;
    //CompareValidator[] _cvValue;
    //CustomValidator[] _cusvValue;
    FilteredTextBoxExtender[] _ftbExt;
    SliderExtender[] _seValue;

    FileUpload[] _fuValue;
    FileUpload[] _fuValue2;
    Panel[] _pnlDIV;
    Panel[] _pnlDIV2;
    Label[] _lblValue;
 
    
    DataTable _dtColumnsDetail;
    DataTable _dtColumnsNotDetail;
    DataTable _dtColumnsAll;
    DataTable _dtRecordedetail;
    DropDownList _ddlEnteredBy;
    //HyperLink _hlSSAdd;
    Label _lblWarningResults;
    Label _lblWarningResultsValue;

    Label _lblValidationResults;
    TextBox _txtValidationResults;


    CheckBox _chkIsActive;
    TextBox[] _txtTime;
    Label[] _lblTime;
    Label _lblAddedCaption, _lblUpdatedCaption, _lblAddedTimeEmail, _lblUpdatedTimeEmail;
    //Image _imgTrigger;
    //CalendarExtender[] _ceDateTimeRecorded;

    //AjaxControlToolkit.MaskedEditExtender[] _meeDate;
    //AjaxControlToolkit.MaskedEditValidator[] _mevDate;
    //TextBoxWatermarkExtender[] _twmValue;
    //RangeValidator[] _rvDate;

    //MaskedEditExtender[] _meeTime;
    // CustomValidator[] _cvTime;

    //int _iLocationIndex = -1;
    int _iTableIndex = -1;
    int _iDateTimeRecorded = -1;
    int _iEnteredByIndex = -1;
    int _iIsActiveIndex = -1;
    User _objUser;
    string _qsMode = "";
    //string _qsRecordID = "";
    Table _theTable;
    string _strRecordRightID = Common.UserRoleType.None;
    string _strURL;


    //string _strSessionRoleType = "";
    int _iSessionAccountID = -1;



    protected void Page_Init(object sender, EventArgs e)
    {

        _strFilesLocation = Session["FilesLocation"].ToString();
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();

        if (Request.QueryString["mode"] != null)
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]).ToString().ToLower();
        }
        if (Request.QueryString["Recordid"] != null)
        {
            _iParentRecordID = int.Parse(Cryptography.Decrypt(Request.QueryString["Recordid"].ToString()));
            _ParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);
        }

        if (this.Page.MasterPageFile != null && this.Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        {
            _bCustomDDL = true;
        }

        

        _strDynamictabPart = lnkSaveClose.ClientID.Substring(0, lnkSaveClose.ClientID.Length - 12);

        _theTable = RecordManager.ets_Table_Details(TableID);


        GetRoleRight();

        if (_strRecordRightID == Common.UserRoleType.None) //none role -- 
        {
            //Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Default.aspx", false);
            divDynamic.Visible = false;
            return;
        }


        _dtColumnsDetail = RecordManager.ets_Table_Columns_Detail(TableID);
        _dtColumnsNotDetail = RecordManager.ets_Table_Columns_NotDetail(TableID);
        _dtColumnsAll = RecordManager.ets_Table_Columns_All(TableID);

        bool bHasLetft = false;
        bool bHasRight = false;
        if (_theTable.BoxAroundField != null)
        {
            if ((bool)_theTable.BoxAroundField)
            {
                for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                {
                    if (bool.Parse(_dtColumnsDetail.Rows[i]["DisplayRight"].ToString()))
                    {
                        bHasRight = true;
                    }
                    else
                    {
                        bHasLetft = true;
                    }
                }

                if (bHasLetft)
                {
                    tblLeft.Style.Add("border", "1px solid #ABAAAA");
                    tblLeft.Style.Add("width", "400px");
                }
                if (bHasRight)
                {
                    tblRight.Style.Add("border", "1px solid #ABAAAA");
                    tblRight.Style.Add("width", "400px");
                }

            }
        }



        _lbl = new Label[_dtColumnsDetail.Rows.Count];
        _txtValue = new TextBox[_dtColumnsDetail.Rows.Count];
        _txtValue2 = new TextBox[_dtColumnsDetail.Rows.Count];
        _ibValue = new ImageButton[_dtColumnsDetail.Rows.Count];
        _lnkValue = new LinkButton[_dtColumnsDetail.Rows.Count];
        _hlValue = new HyperLink[_dtColumnsDetail.Rows.Count];
        _htmValue = new WYSIWYGEditor[_dtColumnsDetail.Rows.Count];

        _ddlValue = new DropDownList[_dtColumnsDetail.Rows.Count];
        _ddlValue2 = new DropDownList[_dtColumnsDetail.Rows.Count];

        _radioList = new RadioButtonList[_dtColumnsDetail.Rows.Count];
        _lstValue = new ListBox[_dtColumnsDetail.Rows.Count];
        _cblValue = new CheckBoxList[_dtColumnsDetail.Rows.Count];
        _chkValue = new CheckBox[_dtColumnsDetail.Rows.Count];
        _ccddl = new CascadingDropDown[_dtColumnsDetail.Rows.Count];
        _hfValue = new HiddenField[_dtColumnsDetail.Rows.Count];
        _hfValue2 = new HiddenField[_dtColumnsDetail.Rows.Count];
        _hfValue3 = new HiddenField[_dtColumnsDetail.Rows.Count];

        _imgWarning = new Image[_dtColumnsDetail.Rows.Count];
        _imgValues = new Image[_dtColumnsDetail.Rows.Count];
        _ftbExt = new FilteredTextBoxExtender[_dtColumnsDetail.Rows.Count];
        _fuValue = new FileUpload[_dtColumnsDetail.Rows.Count];
        _fuValue2 = new FileUpload[_dtColumnsDetail.Rows.Count];
        _pnlDIV = new Panel[_dtColumnsDetail.Rows.Count];
        _pnlDIV2 = new Panel[_dtColumnsDetail.Rows.Count];
        _lblValue = new Label[_dtColumnsDetail.Rows.Count];

        _seValue = new SliderExtender[_dtColumnsDetail.Rows.Count];

        _lblTime = new Label[_dtColumnsDetail.Rows.Count];
        _txtTime = new TextBox[_dtColumnsDetail.Rows.Count];

        trX = new HtmlTableRow[_dtColumnsDetail.Rows.Count + 4];
        cell = new HtmlTableCell[(_dtColumnsDetail.Rows.Count + 4) * 2];

        



        _dtDBTableTab = Common.DataTableFromText("SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + " ORDER BY DisplayOrder");



        CreateTableTab();
        CreateDynamicControls();




        //for (int i = 0; i < _dtRecordTypleColumlns.Rows.Count; i++)
        //{
        //    if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() == "dropdown"
        //                && (_dtRecordTypleColumlns.Rows[i]["DropDownType"].ToString() == "table"
        //                || _dtRecordTypleColumlns.Rows[i]["DropDownType"].ToString() == "tabledd")
        //                && _dtRecordTypleColumlns.Rows[i]["TableTableID"] != DBNull.Value
        //                 && _dtRecordTypleColumlns.Rows[i]["FilterParentColumnID"] != DBNull.Value
        //                  && _dtRecordTypleColumlns.Rows[i]["FilterOtherColumnID"] != DBNull.Value
        //                && _dtRecordTypleColumlns.Rows[i]["DisplayColumn"].ToString() != ""
        //                )
        //    {
        //        for (int j = 0; j < _dtRecordTypleColumlns.Rows.Count; j++)
        //        {
        //            if (_dtRecordTypleColumlns.Rows[i]["FilterOtherColumnID"].ToString() ==
        //                _dtRecordTypleColumlns.Rows[j]["ColumnID"].ToString())
        //            {

        //                if (_dtRecordTypleColumlns.Rows[i]["DropDownType"].ToString() == "tabledd")
        //                {
        //                    if (_ddlValue[i] != null && _ddlValue[j] != null)
        //                    {

        //                        _ccddl[i] = new CascadingDropDown();
        //                        _ccddl[i].ID = "ccddl" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();

        //                        _ccddl[i].Category = _dtRecordTypleColumlns.Rows[i]["ColumnID"].ToString() + "," + _dtRecordTypleColumlns.Rows[i]["FilterParentColumnID"].ToString();
        //                        _ccddl[i].TargetControlID = _ddlValue[i].ID;


        //                        _ccddl[i].ParentControlID = _ddlValue[j].ID; //"ddl" + _dtRecordTypleColumlns.Rows[j]["SystemName"].ToString();


        //                        _ccddl[i].ServicePath = "~/CascadeDropdown.asmx";
        //                        _ccddl[i].ServiceMethod = "GetFilteredData"; //filtered


        //                        cell[(i * 2) + 1].Controls.Add(_ccddl[i]);
        //                    }

        //                }

        //            }
        //        }


        //    }

        //}

        //here Show and Hide code



        _lblWarningResults = new Label();
        _lblWarningResults.ID = "lblWarningResults";
        _lblWarningResults.Text = "Warning Results";
        _lblWarningResults.Font.Bold = true;

        _lblWarningResultsValue = new Label();
        _lblWarningResultsValue.ID = "lblWarningResultsValue";
        _lblWarningResultsValue.Width = 300;
        _lblWarningResults.Visible = false;

        _lblWarningResultsValue.ForeColor = System.Drawing.Color.Blue;


        _lblValidationResults = new Label();
        _lblValidationResults.ID = "lblValidationResults";
        _lblValidationResults.Text = "Validation Results";
        _lblValidationResults.Font.Bold = true;
        _lblValidationResults.Visible = false;

        _txtValidationResults = new TextBox();
        _txtValidationResults.ID = "txtValidationResults";
        _txtValidationResults.Width = 300;
        //_txtValidationResults.Height = 50;
        _txtValidationResults.TextMode = TextBoxMode.SingleLine;
        //_txtValidationResults.ReadOnly = true;
        _txtValidationResults.Enabled = false;
        //_txtValidationResults.BackColor = System.Drawing.Color.Gray;
        _txtValidationResults.CssClass = "NormalTextBox";
        _txtValidationResults.ForeColor = System.Drawing.Color.Red;
        _txtValidationResults.Visible = false;






        cell[_dtColumnsDetail.Rows.Count * 2] = new HtmlTableCell();
        //cellB[_dtRecordTypleColumlns.Rows.Count * 2] = new HtmlTableCell();

        cell[_dtColumnsDetail.Rows.Count * 2].Align = "Right";
        cell[_dtColumnsDetail.Rows.Count * 2].Controls.Add(_lblWarningResults);
        cell[(_dtColumnsDetail.Rows.Count * 2) + 1] = new HtmlTableCell();
        //cellB[(_dtRecordTypleColumlns.Rows.Count * 2) + 1] = new HtmlTableCell();

        cell[(_dtColumnsDetail.Rows.Count * 2) + 1].Controls.Add(_lblWarningResultsValue);

        //cell[(_dtRecordTypleColumlns.Rows.Count * 2) + 1].Controls.Add(hlTest);

        trX[_dtColumnsDetail.Rows.Count] = new HtmlTableRow();
        //trXB[_dtRecordTypleColumlns.Rows.Count] = new HtmlTableRow();

        trX[_dtColumnsDetail.Rows.Count].Cells.Add(cell[_dtColumnsDetail.Rows.Count * 2]);
        trX[_dtColumnsDetail.Rows.Count].Cells.Add(cell[(_dtColumnsDetail.Rows.Count * 2) + 1]);

        tblLeft.Rows.Add(trX[_dtColumnsDetail.Rows.Count]);


        //validation

        cell[(_dtColumnsDetail.Rows.Count + 1) * 2] = new HtmlTableCell();
        //cellB[(_dtRecordTypleColumlns.Rows.Count + 1) * 2] = new HtmlTableCell();

        cell[(_dtColumnsDetail.Rows.Count + 1) * 2].Align = "Right";
        cell[(_dtColumnsDetail.Rows.Count + 1) * 2].Controls.Add(_lblValidationResults);
        cell[((_dtColumnsDetail.Rows.Count + 1) * 2) + 1] = new HtmlTableCell();
        //cellB[((_dtRecordTypleColumlns.Rows.Count + 1) * 2) + 1] = new HtmlTableCell();

        cell[((_dtColumnsDetail.Rows.Count + 1) * 2) + 1].Controls.Add(_txtValidationResults);

        //cell[(_dtRecordTypleColumlns.Rows.Count * 2) + 1].Controls.Add(hlTest);

        trX[_dtColumnsDetail.Rows.Count + 1] = new HtmlTableRow();
        //trXB[_dtRecordTypleColumlns.Rows.Count + 1] = new HtmlTableRow();

        trX[_dtColumnsDetail.Rows.Count + 1].Cells.Add(cell[(_dtColumnsDetail.Rows.Count + 1) * 2]);
        trX[_dtColumnsDetail.Rows.Count + 1].Cells.Add(cell[((_dtColumnsDetail.Rows.Count + 1) * 2) + 1]);

        tblLeft.Rows.Add(trX[_dtColumnsDetail.Rows.Count + 1]);




        //added & updated

     
                _lblAddedCaption = new Label();
                _lblAddedCaption.ID = "lblAddedCaption";
                _lblAddedCaption.Text = "Added";
                _lblAddedCaption.Font.Bold = true;

                _lblAddedTimeEmail = new Label();
                _lblAddedTimeEmail.ID = "lblAddedTimeEmail";
                _lblAddedTimeEmail.Text = "";
                //_lblAddedTimeEmail.Font.Bold = true;


                cell[(_dtColumnsDetail.Rows.Count + 2) * 2] = new HtmlTableCell();
                //cellB[(_dtRecordTypleColumlns.Rows.Count + 2) * 2] = new HtmlTableCell();

                cell[(_dtColumnsDetail.Rows.Count + 2) * 2].Align = "Right";
                cell[(_dtColumnsDetail.Rows.Count + 2) * 2].Controls.Add(_lblAddedCaption);
                cell[((_dtColumnsDetail.Rows.Count + 2) * 2) + 1] = new HtmlTableCell();
                //cellB[((_dtRecordTypleColumlns.Rows.Count + 2) * 2) + 1] = new HtmlTableCell();
                cell[((_dtColumnsDetail.Rows.Count + 2) * 2) + 1].Controls.Add(_lblAddedTimeEmail);
                trX[_dtColumnsDetail.Rows.Count + 2] = new HtmlTableRow();
                //trXB[_dtRecordTypleColumlns.Rows.Count + 2] = new HtmlTableRow();

                trX[_dtColumnsDetail.Rows.Count + 2].Cells.Add(cell[(_dtColumnsDetail.Rows.Count + 2) * 2]);
                trX[_dtColumnsDetail.Rows.Count + 2].Cells.Add(cell[((_dtColumnsDetail.Rows.Count + 2) * 2) + 1]);

                tblLeft.Rows.Add(trX[_dtColumnsDetail.Rows.Count + 2]);


                _lblUpdatedCaption = new Label();
                _lblUpdatedCaption.ID = "lblUpdatedCaption";
                _lblUpdatedCaption.Text = "Updated";
                _lblUpdatedCaption.Font.Bold = true;

                _lblUpdatedTimeEmail = new Label();
                _lblUpdatedTimeEmail.ID = "lblUpdatedTimeEmail";
                _lblUpdatedTimeEmail.Text = "";
                //_lblUpdatedTimeEmail.Font.Bold = true;


                cell[(_dtColumnsDetail.Rows.Count + 3) * 2] = new HtmlTableCell();
                //cellB[(_dtRecordTypleColumlns.Rows.Count + 3) * 2] = new HtmlTableCell();

                cell[(_dtColumnsDetail.Rows.Count + 3) * 2].Align = "Right";
                cell[(_dtColumnsDetail.Rows.Count + 3) * 2].Controls.Add(_lblUpdatedCaption);
                cell[((_dtColumnsDetail.Rows.Count + 3) * 2) + 1] = new HtmlTableCell();
                //cellB[((_dtRecordTypleColumlns.Rows.Count + 3) * 2) + 1] = new HtmlTableCell();

                cell[((_dtColumnsDetail.Rows.Count + 3) * 2) + 1].Controls.Add(_lblUpdatedTimeEmail);
                trX[_dtColumnsDetail.Rows.Count + 3] = new HtmlTableRow();
                //trXB[_dtRecordTypleColumlns.Rows.Count + 3] = new HtmlTableRow();

                trX[_dtColumnsDetail.Rows.Count + 3].Cells.Add(cell[(_dtColumnsDetail.Rows.Count + 3) * 2]);
                trX[_dtColumnsDetail.Rows.Count + 3].Cells.Add(cell[((_dtColumnsDetail.Rows.Count + 3) * 2) + 1]);

                tblLeft.Rows.Add(trX[_dtColumnsDetail.Rows.Count + 3]);
       
        
       

    }




    public void ShowHideControls()
    {

        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {
            if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
            {
                trX[i].Style.Add("vertical-align", "top");
            }


            if (_dtColumnsDetail.Rows[i]["OnlyForAdmin"] != DBNull.Value)
            {

                bool bHide = false;

                if (_dtColumnsDetail.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
                {
                    if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                    {
                        bHide = true;


                    }
                }
                if (_dtColumnsDetail.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "2")
                {
                    if (_qsMode != "")
                    {
                        if (_qsMode != "add"
                            && _theRecord != null)
                        {

                            if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                            {
                                if (_theRecord.EnteredBy != _objUser.UserID)
                                {
                                    bHide = true;
                                }
                            }

                        }
                    }

                }

                if (bHide)
                {
                    trX[i].ID = "trX" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    string targetID = "#" + _strDynamictabPart + trX[i].ID;
                    string strOnlyAdminJS = @"$('" + targetID + @"').fadeOut();";


                    //if (!IsPostBack)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHideAdmin" + _strDynamictabPart + i.ToString(), strOnlyAdminJS, true);


                }

            }


            //perform unlimited showwhen

            DataTable dtShowWhen = RecordManager.dbg_ShowWhen_Select(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), null, null);

            if (dtShowWhen.Rows.Count > 0)
            {
                trX[i].ID = "trX" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                //string strTargetTRID = "#" + trX[i].ClientID.Substring(0, trX[i].ClientID.Length - 7) + trX[i].ID;
                string strTargetTRID = "#" + _strDynamictabPart + trX[i].ID;


                string strAllDriverValue = "";
                string strAllLogic = "";
                string strAllDriverTrigger = "";

                string strEachPreJoinOperator = "";


                string strBeforeShowHideFunction = "";



                foreach (DataRow drSW in dtShowWhen.Rows)
                {
                    if (drSW["ColumnID"] != DBNull.Value != null && drSW["HideColumnID"] != DBNull.Value)
                    {
                        //Lets's go inside eash driver column

                        for (int m = 0; m < _dtColumnsDetail.Rows.Count; m++)
                        {
                            if (drSW["HideColumnID"].ToString() == _dtColumnsDetail.Rows[m]["ColumnID"].ToString() && trX[m] != null)
                            {
                                trX[m].ID = "trX" + _dtColumnsDetail.Rows[m]["SystemName"].ToString();
                                //string strEachDriverID = "#" + trX[m].ClientID.Substring(0, trX[m].ClientID.Length - 7);

                                string strEachDriverID = "#" + _strDynamictabPart;

                                string strControlClientIDPrefix = _strDynamictabPart;

                                string strEachHideColumnValue = HttpUtility.JavaScriptStringEncode(drSW["HideColumnValue"].ToString()); //drSW["HideColumnValue"].ToString().Replace("'","\\'");
                                string strEachHideOperator = "";

                                if (drSW["HideOperator"].ToString() != "")
                                {
                                    if (drSW["HideOperator"].ToString() == "equals")
                                    {
                                        strEachHideOperator = " == ";
                                    }
                                    else if (drSW["HideOperator"].ToString() == "notequal")
                                    {
                                        strEachHideOperator = " != ";
                                    }
                                    else if (drSW["HideOperator"].ToString() == "greaterthan")
                                    {
                                        strEachHideOperator = " > ";
                                    }
                                    else if (drSW["HideOperator"].ToString() == "greaterthanequal")
                                    {
                                        strEachHideOperator = " >= ";
                                    }
                                    else if (drSW["HideOperator"].ToString() == "lessthan")
                                    {
                                        strEachHideOperator = " < ";
                                    }
                                    else if (drSW["HideOperator"].ToString() == "lessthanequal")
                                    {
                                        strEachHideOperator = " <= ";
                                    }
                                    else
                                    {
                                        strEachHideOperator = drSW["HideOperator"].ToString();//contains,notcontains
                                    }
                                }

                                if (_ddlValue[m] != null)
                                    strEachDriverID = strEachDriverID + "ddl" + _dtColumnsDetail.Rows[m]["SystemName"].ToString();

                                if (_txtValue[m] != null)
                                    strEachDriverID = strEachDriverID + "txt" + _dtColumnsDetail.Rows[m]["SystemName"].ToString();

                                if (_chkValue[m] != null)
                                    strEachDriverID = strEachDriverID + "chk" + _dtColumnsDetail.Rows[m]["SystemName"].ToString();

                                if (_lstValue[m] != null)
                                    strEachDriverID = strEachDriverID + "lst" + _dtColumnsDetail.Rows[m]["SystemName"].ToString();

                                if (_cblValue[m] != null)
                                    strEachDriverID = strEachDriverID + "cbl" + _dtColumnsDetail.Rows[m]["SystemName"].ToString();

                                if (_radioList[m] != null)
                                    strEachDriverID = strEachDriverID + "radio" + _dtColumnsDetail.Rows[m]["SystemName"].ToString();

                                bool bUseCommonCode = false;


                                string strDriverGroupID = strEachDriverID;

                                if (_radioList[m] != null)
                                {
                                    string strVariableDeclare = "";
                                    if (strAllDriverValue.IndexOf("var strEachValue" + m.ToString()) == -1)//???
                                    {
                                        strVariableDeclare = " var ";
                                    }
                                    strEachDriverID = strEachDriverID + "_";
                                    strBeforeShowHideFunction = strBeforeShowHideFunction + "$('" + strTargetTRID + "').fadeOut(); ";
                                    string strDriverIDMain = strEachDriverID;

                                    strAllDriverValue = strAllDriverValue + @"
                                               " + strVariableDeclare + @"  strEachValue" + m.ToString() + @" =GetOptValue('" + strDriverGroupID.Replace("#", "").Replace("_", "$") + @"');  
                                                ";

                                    strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @" " + strEachHideOperator + @" '" + strEachHideColumnValue + "') ";

                                    for (int n = 0; n < _radioList[m].Items.Count; n++)
                                    {

                                        strEachDriverID = strDriverIDMain + n.ToString();
                                        strAllDriverTrigger = strAllDriverTrigger + @"                                           
                                           
                                                  $('" + strEachDriverID + @"').change(function (e) {
                                                       ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
                                                }); ";

                                        if (_radioList[m].SelectedIndex == n)
                                        {
                                            strAllDriverTrigger = strAllDriverTrigger + "$('" + strEachDriverID + "').trigger('change');";
                                        }


                                    }



                                }
                                else if (_chkValue[m] != null)
                                {
                                    string strTrue = "";
                                    string strFalse = "";
                                    Common.GetCheckTcikedUnTicked(_dtColumnsDetail.Rows[m]["DropDownValues"].ToString(), ref strTrue, ref strFalse);
                                    strTrue = HttpUtility.JavaScriptStringEncode(strTrue);
                                    strFalse = HttpUtility.JavaScriptStringEncode(strFalse);

                                    bUseCommonCode = true;
                                    strAllDriverValue = strAllDriverValue + @"
                                              var chk" + m.ToString() + @" = document.getElementById('" + strDriverGroupID.Substring(1, strDriverGroupID.Length - 1) + @"');
                                                var strEachValue" + m.ToString() + @" ='';
                                                if (chk" + m.ToString() + @".checked == true) { strEachValue" + m.ToString() + @" = '" + strTrue + @"'; }
                                                if (chk" + m.ToString() + @".checked == false) { strEachValue" + m.ToString() + @" = '" + strFalse + @"'; }     
                                                ";

                                }
                                else if (_lstValue[m] != null)
                                {
                                    if (strEachHideOperator == "contains" || strEachHideOperator == "notcontains")
                                    {
                                        bUseCommonCode = false;


                                        strAllDriverValue = strAllDriverValue + @"
                                                            $('" + strTargetTRID + @"').fadeOut();" + @"
                                                            var bShow" + m.ToString() + @"=false;
                                                            var strHideValues" + m.ToString() + @"='" + strEachHideColumnValue + @"';
                                                            if(strHideValues" + m.ToString() + @"==null || strHideValues" + m.ToString() + @"=='')
                                                            {
                                                                return;
                                                            }
                                                            var strHideValues_array" + m.ToString() + @"=strHideValues" + m.ToString() + @".split(',');
                                                            var strEachValue" + m.ToString() + @" = $('" + strDriverGroupID + @"').val();
                                      
                                                            if(strEachValue" + m.ToString() + @"==null || strEachValue" + m.ToString() + @"=='' || strEachValue" + m.ToString() + @"=='undefined')
                                                            {
                                                                return;
                                                            }
                                                         strEachValue" + m.ToString() + @"=strEachValue" + m.ToString() + @".toString();
                                                            var strEachValue_array" + m.ToString() + @"=strEachValue" + m.ToString() + @".split(',');
                                                             for(var i = 0; i < strHideValues_array" + m.ToString() + @".length; i++) {
                                            
                                                                    if(strHideValues_array" + m.ToString() + @"[i]!='')
                                                                    {
                                                                         for(var j = 0; j < strEachValue_array" + m.ToString() + @".length; j++) 
                                                                        {
                                                                             if(strEachValue_array" + m.ToString() + @"[j]!='')
                                                                            {
                                                                                if(strHideValues_array" + m.ToString() + @"[i]==strEachValue_array" + m.ToString() + @"[j])
                                                                                    {
                                                                                        bShow" + m.ToString() + @"=true;
                                                                                    }
                                                                            }
                                                                        }

                                                                    }

                                                              }
                                            ";


                                        if (strEachHideOperator == "contains")
                                        {
                                            strAllLogic = strAllLogic + strEachPreJoinOperator + " (bShow" + m.ToString() + @"==true) ";
                                        }
                                        else
                                        {
                                            strAllLogic = strAllLogic + strEachPreJoinOperator + " (bShow" + m.ToString() + @"==false) ";
                                        }



                                        strAllDriverTrigger = strAllDriverTrigger + @"
                                                     $('" + strDriverGroupID + @"').change(function (e) {
                                                              ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
                                                        });
                                                        $('" + strDriverGroupID + @"').trigger('change');  
                                                ";



                                    }
                                    else
                                    {
                                        strAllDriverValue = strAllDriverValue + @"
                                            var strEachValue" + m.ToString() + @" = $('" + strDriverGroupID + @"').val();";

                                        bUseCommonCode = true;


                                    }


                                }
                                else if (_cblValue[m] != null)
                                {
                                    bUseCommonCode = false;
                                    if (strEachHideOperator == "contains" || strEachHideOperator == "notcontains")
                                    {
                                        strAllDriverValue = strAllDriverValue + @"

                                                            $('" + strTargetTRID + @"').fadeOut();" + @"
                                                            var bShow" + m.ToString() + @"=false;
                                                            var strHideValues" + m.ToString() + @"='" + strEachHideColumnValue + @"';
                                                            if(strHideValues" + m.ToString() + @"==null || strHideValues" + m.ToString() + @"=='')
                                                            {
                                                                return;
                                                            }
                                                            var strHideValues_array" + m.ToString() + @"=strHideValues" + m.ToString() + @".split(',');                                                          
                                      
                                                            var chkBox" + m.ToString() + @" = document.getElementById('" + strDriverGroupID.Replace("#", "") + @"');
                                                             var options" + m.ToString() + @" = chkBox" + m.ToString() + @".getElementsByTagName('input');
                                                             var listOfSpans" + m.ToString() + @" = chkBox" + m.ToString() + @".getElementsByTagName('span');
                                                            var strEachValue" + m.ToString() + @"='';
                                                             for (var i = 0; i < options" + m.ToString() + @".length; i++) {
                                                                 if (options" + m.ToString() + @"[i].checked) {
                                                                     strEachValue" + m.ToString() + @"=strEachValue" + m.ToString() + @"+ ',' + listOfSpans" + m.ToString() + @"[i].attributes['DataValue'].value;
                                                                 }
                                                             }  

                                                            if(strEachValue" + m.ToString() + @"==null || strEachValue" + m.ToString() + @"=='' || strEachValue" + m.ToString() + @"=='undefined')
                                                            {
                                                                return;
                                                            }
                                                         strEachValue" + m.ToString() + @"=strEachValue" + m.ToString() + @".toString();
                                                            var strEachValue_array" + m.ToString() + @"=strEachValue" + m.ToString() + @".split(',');
                                                             for(var i = 0; i < strHideValues_array" + m.ToString() + @".length; i++) {
                                            
                                                                    if(strHideValues_array" + m.ToString() + @"[i]!='')
                                                                    {
                                                                         for(var j = 0; j < strEachValue_array" + m.ToString() + @".length; j++) 
                                                                        {
                                                                             if(strEachValue_array" + m.ToString() + @"[j]!='')
                                                                            {
                                                                                if(strHideValues_array" + m.ToString() + @"[i]==strEachValue_array" + m.ToString() + @"[j])
                                                                                    {
                                                                                        bShow" + m.ToString() + @"=true;
                                                                                    }
                                                                            }
                                                                        }

                                                                    }

                                                              }
                                          

                                                ";

                                        if (strEachHideOperator == "contains")
                                        {
                                            strAllLogic = strAllLogic + strEachPreJoinOperator + " (bShow" + m.ToString() + @"==true) ";
                                        }
                                        else
                                        {
                                            strAllLogic = strAllLogic + strEachPreJoinOperator + " (bShow" + m.ToString() + @"==false) ";
                                        }


                                    }
                                    else
                                    {

                                        strAllDriverValue = strAllDriverValue + @"
                                                var chkBox" + m.ToString() + @" = document.getElementById('" + strDriverGroupID.Replace("#", "") + @"');
                                                 var options" + m.ToString() + @" = chkBox" + m.ToString() + @".getElementsByTagName('input');
                                                 var listOfSpans" + m.ToString() + @" = chkBox" + m.ToString() + @".getElementsByTagName('span');
                                                 var strEachValue" + m.ToString() + @"='';
                                                 for (var i = 0; i < options" + m.ToString() + @".length; i++) {
                                                     if (options" + m.ToString() + @"[i].checked) {
                                                         strEachValue" + m.ToString() + @"=strEachValue" + m.ToString() + @" + ',' + listOfSpans" + m.ToString() + @"[i].attributes['DataValue'].value;
                                                     }
                                                 }    
                                                 if(strEachValue" + m.ToString() + @"!='')
                                                    {
                                                        strEachValue" + m.ToString() + @"=strEachValue" + m.ToString() + @".substring(1);
                                                    }
                                       
                                                ";


                                        strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @" " + strEachHideOperator + @" '" + strEachHideColumnValue + "') ";


                                    }

                                    strAllDriverTrigger = strAllDriverTrigger + @"
                                                     $('" + strDriverGroupID + @"').click(function (e) {
                                                              ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
                                                        });
                                                        $('" + strDriverGroupID + @"').trigger('click');  
                                                ";
                                }
                                else
                                {
                                    //textbox, dropdown - Number, Text etc
                                    bUseCommonCode = true;
                                    strAllDriverValue = strAllDriverValue + @"
                                            var strEachValue" + m.ToString() + @" = $('" + strDriverGroupID + @"').val();";

                                }

                                if (bUseCommonCode)
                                {

                                    strAllDriverTrigger = strAllDriverTrigger + @"
                                                     $('" + strDriverGroupID + @"').change(function (e) {
                                                              ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
                                                        });
                                                        $('" + strDriverGroupID + @"').trigger('change');  
                                                ";
                                    if (strEachHideOperator != "")
                                    {
                                        if (strEachHideOperator == "contains")
                                        {
                                            strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @".indexOf('" + strEachHideColumnValue + "')>=0) ";
                                        }
                                        else if (strEachHideOperator == "notcontains")
                                        {
                                            strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @".indexOf('" + strEachHideColumnValue + "')<0) ";
                                        }
                                        else if (strEachHideOperator == "empty")
                                        {
                                            strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @"=='') ";
                                        }
                                        else if (strEachHideOperator == "notempty")
                                        {
                                            strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @"!='') ";
                                        }
                                        else
                                        {
                                            string strCY = _dtColumnsDetail.Rows[m]["ColumnType"].ToString();
                                            if (_txtValue[m] != null && (strCY == "number" || strCY == "date" || strCY == "datetime" || strCY == "time")) //
                                            {
                                                if (strCY == "number")
                                                {
                                                    strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @" " + strEachHideOperator + @" " + strEachHideColumnValue + ") ";
                                                }
                                                else if (strCY == "date")
                                                {
                                                    string strDateLogic = @"     
                                                                                 var strHideColumnValue" + m.ToString() + @"='" + strEachHideColumnValue + @"';
                                                                                 var d1" + m.ToString() + @" =new Date(strEachValue" + m.ToString() + @".replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') );
                                                                                 var d2" + m.ToString() + @" =new Date(strHideColumnValue" + m.ToString() + @".replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') );                     
                                                                                ";

                                                    strAllDriverValue = strAllDriverValue + strDateLogic;
                                                    strAllLogic = strAllLogic + strEachPreJoinOperator + " (d1" + m.ToString() + @".getTime() " + strEachHideOperator + @"  d2" + m.ToString() + @".getTime()) ";

                                                }
                                                else if (strCY == "datetime")
                                                {
                                                    string strDateLogic = @" var strHideColumnValue" + m.ToString() + @"='" + strEachHideColumnValue + @"';
                                                                                  var time1" + m.ToString() + @"= document.getElementById('" + strControlClientIDPrefix + _txtTime[m].ID + @"');
                                                                                  var t1" + m.ToString() + @"=time1" + m.ToString() + @".value;
                                                                                  if(t1" + m.ToString() + @"==''){t1" + m.ToString() + @"='00:00'};
                                                                                  var d1" + m.ToString() + @" =new Date(strEachValue" + m.ToString() + @".replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') + ' ' + t1" + m.ToString() + @" );
                                                                                  var d2" + m.ToString() + @" =new Date(strHideColumnValue" + m.ToString() + @".replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') );                     
                                                                                ";



                                                    strAllDriverValue = strAllDriverValue + strDateLogic;
                                                    strAllLogic = strAllLogic + strEachPreJoinOperator + " (d1" + m.ToString() + @".getTime() " + strEachHideOperator + @"  d2" + m.ToString() + @".getTime()) ";


                                                    //add time trigger too
                                                    strAllDriverTrigger = strAllDriverTrigger + @"
                                                                 $('#" + strControlClientIDPrefix + _txtTime[m].ID + @"').change(function (e) {
                                                                          ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
                                                                    });
                                                                    $('#" + strControlClientIDPrefix + _txtTime[m].ID + @"').trigger('change');  
                                                            ";


                                                }
                                                else if (strCY == "time")
                                                {
                                                    string strDateLogic = @" var strHideColumnValue" + m.ToString() + @"='" + strEachHideColumnValue + @"';
                                                                                 if (strEachValue" + m.ToString() + @"==''){strEachValue" + m.ToString() + @"='00:00'};
                                                                                 if (strHideColumnValue" + m.ToString() + @"==''){strHideColumnValue" + m.ToString() + @"='00:00'};
                                                                                 
                                                                                var time1" + m.ToString() + @" = strEachValue" + m.ToString() + @".split(':'); 
                                                                                var d1" + m.ToString() + @" = new Date(); 
                                                                                d1" + m.ToString() + @".setHours  (time1" + m.ToString() + @"[0]); 
                                                                                d1" + m.ToString() + @".setMinutes(time1" + m.ToString() + @"[1]); 
                                                                                var time2" + m.ToString() + @" = strHideColumnValue" + m.ToString() + @".split(':');  

                                                                                var d2" + m.ToString() + @" = new Date(); 

                                                                                d2" + m.ToString() + @".setHours  (time2" + m.ToString() + @"[0]); 
                                                                                d2" + m.ToString() + @".setMinutes(time2" + m.ToString() + @"[1]);
                                                                                                     
                                                                                ";

                                                    strAllDriverValue = strAllDriverValue + strDateLogic;
                                                    strAllLogic = strAllLogic + strEachPreJoinOperator + " (d1" + m.ToString() + @".getTime() " + strEachHideOperator + @"  d2" + m.ToString() + @".getTime()) ";



                                                }
                                                else
                                                {
                                                    strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @" " + strEachHideOperator + @" '" + strEachHideColumnValue + "') ";

                                                }
                                            }
                                            else
                                            {
                                                strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @" " + strEachHideOperator + @" '" + strEachHideColumnValue + "') ";
                                            }

                                        }

                                    }

                                }







                            }
                        }

                    }
                    else
                    {
                        strEachPreJoinOperator = "";
                        if (drSW["JoinOperator"].ToString() == "and")
                        {
                            strEachPreJoinOperator = " && ";
                        }
                        else if (drSW["JoinOperator"].ToString() == "or")
                        {
                            strEachPreJoinOperator = " || ";
                        }
                    }

                }

                if (strAllDriverValue != "" && strAllLogic != "")
                {


                    string strShowHideFunction = @"
                                            $(document).ready(function () {
                                                 try { 
                                                               " + strBeforeShowHideFunction + @"
                                                        function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"()
                                                            {
                                                                    try {

                                                                                                    " + strAllDriverValue + @"
                                                                            if (" + strAllLogic + @") {
                                                                               $('" + strTargetTRID + @"').stop(true,true); $('" + strTargetTRID + @"').fadeIn();" + @"
                                                                            }
                                                                            else {
                                                                                $('" + strTargetTRID + @"').fadeOut();" + @"
                                                                            }
                                                                         }
                                                                        catch(err) {
                                                                                    //email developer
                                                                            }
                                                            }

                                                                " + strAllDriverTrigger + @"

                                                        }
                                                    catch(err) {
                                                                //email developer
                                                        }
                                                    });
                                        ";


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "strShowHideFunction" + _strDynamictabPart + i.ToString(), strShowHideFunction, true);
                }
            }
        }
    }

    public string GetEditURL()
    {

        string strparentRecordid = Cryptography.Encrypt("-1");

        if (Request.QueryString["Recordid"] != null)
        {
            strparentRecordid = Request.QueryString["Recordid"].ToString();
        }
        else if (Session["CopyRecordID"] != null)
        {
            strparentRecordid = Session["CopyRecordID"].ToString();
        }


        return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&tabindex=" + DetailTabIndex.ToString() + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&Recordid=" + Cryptography.Encrypt(RecordID.ToString()) + "&parentRecordid=" + strparentRecordid;

    }

    public string GetAddURL()
    {

        if (Request.QueryString["Recordid"] == null)
        {
            return "";
        }
        else
        {
            return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&tabindex=" + DetailTabIndex.ToString() + "&parentRecordid=" + Request.QueryString["Recordid"].ToString() + "&TableID=" + Cryptography.Encrypt(TableID.ToString()) + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
        }


    }

    protected void GetRoleRight()
    {

        _strChildSession = "child" + _iParentRecordID.ToString() + "_" + _theTable.TableID.ToString();

        if (!IsPostBack)
        {
            Session[_strChildSession] = null;
            if (ContentPage == "record")
            {
                lnkPrevious.Visible = true;
                hlAdd.Visible = true;
                hlEdit.Visible = true;
                lnkNext.Visible = true;
            }
        }
        if (_bPrivate)
        {
            _objUser = (User)Session["User"];
        }
        else
        {
            _objUser = SecurityManager.User_Details(int.Parse(SystemData.SystemOption_ValueByKey_Account("AnonymousUser", null, TableID)));
        }

        UserRole theUserRole = (UserRole)Session["UserRole"];


        if ((bool)theUserRole.IsAdvancedSecurity)
        {
            DataTable dtUserTable = null;
            dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
           TableID, theUserRole.RoleID, null);
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

        if (_strRecordRightID == Common.UserRoleType.AddRecord)
        {
            hlEdit.Visible = false;
        }

        if (_strRecordRightID == Common.UserRoleType.ReadOnly)
        {
            hlEdit.Visible = false;
            hlAdd.Visible = false;
            divNorecordAdd.Visible = false;
        }

        if (Request.QueryString["public"] != null)
        {
            if (_theTable.AddWithoutLogin != null)
            {
                if ((bool)_theTable.AddWithoutLogin)
                {
                    _bPrivate = false;
                    _strRecordRightID = Common.UserRoleType.AddEditRecord;
                    _iSessionAccountID = (int)_theTable.AccountID;
                }
                else
                {
                    _bPrivate = true;
                }
            }
            else
            {
                _bPrivate = true;
            }
        }
        else
        {
            _iSessionAccountID = int.Parse(Session["AccountID"].ToString());
        }


        if (_strRecordRightID == Common.UserRoleType.ReadOnly
                       || _strRecordRightID == Common.UserRoleType.None)
        {
            Response.Redirect("~/Empty.aspx", true);
            return;
        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {

        _strURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.RawUrl;

       
        string strTitle = _theTable.TableName + " View";

        if (!IsPostBack)
        {
            PopulateDynamicControls();
            PopulateRecord();
            PopulateTable();

            if (_bTableTabYes)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHideMainDivsOnlyOne" + _theTable.TableID.ToString(), "ShowHideMainDivs" + _theTable.TableID.ToString() + "('" + pnlDetailTab.ClientID + "',this,0);", true);
            }

        }

       


        if (!IsPostBack)
        {

            hlAdd.NavigateUrl = GetAddURL();
            hlNewData.NavigateUrl = GetAddURL();

            if (ShowAddButton == false)
            {
                hlAdd.Visible = false;
            }

            if (ShowEditButton == false)
            {
                hlEdit.Visible = false;
            }


            if (OnlyOneRecord)
            {
                lnkPrevious.Visible = false;
                lnkNext.Visible = false;
            }


        }



        if (!IsPostBack)
        {
            EnableTheRecordControls(false);
            if (_bPrivate == false)
            {                
                hlBack.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Public.aspx?TableID=" + TableID.ToString();
            }
        }

        strTitle = "View " + _theTable.TableName;
       


        string strFancy = @"$(function () {
            $("".popuplink"").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 600,
                height: 650,
                titleShow: false
            });
        });";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "FancyBox" + _strDynamictabPart, strFancy, true);

      

        if (!IsPostBack)
        {
            ControlCollection _collection = this.Controls;
            TheDatabase.CausesValidationFalse(_collection);
        }


       

    }



    protected void AddEditInfo()
    {
        try
        {

            User userAdded = SecurityManager.User_Details((int)_theRecord.EnteredBy);
            _lblAddedTimeEmail.Text = _theRecord.DateAdded.ToString() + "   By " + userAdded.Email;
            if (_theRecord.LastUpdatedUserID != null)
            {
                User userUpdated = SecurityManager.User_Details((int)_theRecord.LastUpdatedUserID);
                _lblUpdatedTimeEmail.Text = _theRecord.DateUpdated.ToString() + "   By " + userUpdated.Email;
            }
        }
        catch
        {

        }
    }
    protected void PopulateTable()
    {
        if (_iTableIndex >= 0)
        {
            _txtValue[_iTableIndex].Text = _theTable.TableName;
        }
    }


    protected void EmptyControl()
    {

        if (ViewState["RecordID"] == null || ViewState["RecordID"].ToString() == "")
        {
            return;
        }

        _lblWarningResultsValue.Text = "";
        _txtValidationResults.Text = "";
        _lblAddedTimeEmail.Text = "";
        _lblUpdatedTimeEmail.Text = "";

        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {
            if (_txtValue[i] != null)
                _txtValue[i].Text = "";

            if (_txtTime[i] != null)
                _txtTime[i].Text = "";



            if (_lblValue[i] != null)
                _lblValue[i].Text = "";

            if (_hfValue[i] != null)
                _hfValue[i].Value = "";

            if (_hfValue2[i] != null)
                _hfValue2[i].Value = "";

            if (_hfValue3[i] != null)
                _hfValue3[i].Value = "";

            if (_imgValues[i] != null)
            {
                _imgValues[i].ImageUrl = "#";
                _imgValues[i].ToolTip = "";
            }


            if (_imgWarning[i] != null)
            {
                _imgWarning[i].Visible = false;
                _imgWarning[i].ToolTip = "";
            }
            if (_ddlValue[i] != null)
            {
                if (_ddlValue[i].Items.Count > 0)
                    _ddlValue[i].ClearSelection();
            }

            if (_ddlValue2[i] != null)
            {
                if (_ddlValue2[i].Items.Count > 0)
                    _ddlValue2[i].ClearSelection();
            }

            if (_ccddl[i] != null)
            {
                _ccddl[i].SelectedValue = null;
            }


            if (_radioList[i] != null)
            {
                _radioList[i].ClearSelection();
            }

            if (_chkValue[i] != null)
            {
                _chkValue[i].Checked = false;
            }
            if (_htmValue[i] != null)
                _htmValue[i].Text = "";

            if (_lstValue[i] != null)
                _lstValue[i].ClearSelection();

            if (_cblValue[i] != null)
                _cblValue[i].ClearSelection();


        }
    }


    protected void CreatABlank()
    {
        if (!IsPostBack && RecordID == null && OnlyOneRecord)
        {
            if (SystemName != null && LinkedColumnValue != null)
            {
                if (SystemName != "" && LinkedColumnValue != "")
                {
                    if (SecurityManager.IsRecordsExceeded(_iSessionAccountID))
                    {
                        Session["DoNotAllow"] = "true";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DoNotAllow" + _strDynamictabPart, "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);
                        return;
                    }


                    Record theRecord = new Record();
                    theRecord.TableID = TableID;
                    theRecord.IsActive = true;
                    theRecord.EnteredBy = _objUser.UserID;
                    RecordManager.MakeTheRecord(ref theRecord, SystemName, LinkedColumnValue);


                    //find default


                    for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                    {
                        string strValue = "";
                        if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                        {
                            strValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();

                            RecordManager.MakeTheRecord(ref theRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strValue);
                        }

                        if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                            && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value && _ParentRecord != null)
                        {
                            Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));

                            if (theDefaultColumn != null)
                            {
                                //strValue = RecordManager.GetRecordValue(ref _ParentRecord, theDefaultColumn.SystemName);
                                strValue = TheDatabaseS.spGetValueFromRelatedTable((int)_ParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);

                                RecordManager.MakeTheRecord(ref theRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strValue);
                            }

                        }

                    }

                    int iNewRecordID = RecordManager.ets_Record_Insert(theRecord);

                    if (_theTable.AddRecordSP != "")
                        RecordManager.AddRecordSP(_theTable.AddRecordSP, iNewRecordID, null);

                    ViewState["RecordID"] = iNewRecordID.ToString();

                }
            }
        }
    }
    public void PopulateRecord()
    {


        if (!IsPostBack && RecordID == null && OnlyOneRecord)
        {
            CreatABlank();
        }




        if(!IsPostBack)
        {
            

            if (TextSearch == null)
            {
                TextSearch = "";
            }

            string strFirstRecord = "";
            if (TextSearch != "")
            {
                strFirstRecord = Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1 AND (" + TextSearch + ") ORDER BY RecordID");
            }
            else
            {
                strFirstRecord = "";
            }

            if (strFirstRecord == "" && RecordID != null)
            {
                strFirstRecord = RecordID.ToString();
            }
            if (strFirstRecord != "")
            {
                ViewState["RecordID"] = strFirstRecord;
            }

        }
        else
        {
            if (Session[_strChildSession] != null)
            {
                string strRecordID = Session[_strChildSession].ToString();
                if (Request.Params.Get("__EVENTTARGET") != null)
                {
                    if (Request.Params.Get("__EVENTTARGET").ToString().Replace("$", "_") == lnkNext.ClientID)
                    {
                        strRecordID = GetNextRecordID(strRecordID);
                    }
                    if (Request.Params.Get("__EVENTTARGET").ToString().Replace("$", "_") == lnkPrevious.ClientID)
                    {
                        strRecordID = GetPreviousRecordID(strRecordID);
                    }
                }

                if (strRecordID != "")
                {
                    ViewState["RecordID"] = strRecordID;
                }
            }
        }
       

        //added and updated part
        if (ViewState["RecordID"] == null || ViewState["RecordID"].ToString() == "")
        {
            return;
        }
        else
        {
            RecordID = int.Parse(ViewState["RecordID"].ToString());

            if (OnlyOneRecord)
            {
                hlAdd.Visible = false;
            }

        }

        if (ContentPage == "record")
        {
            hlEdit.NavigateUrl = GetEditURL();
            hfEdit.Value = hlEdit.NavigateUrl;
        }



        hfRecordID.Value = RecordID.ToString();

        if (_strChildSession != "")
        {
            Session[_strChildSession] = RecordID.ToString();
        }

        //RecordID = int.Parse(_qsRecordID);
        _dtRecordedetail = RecordManager.ets_Record_Details((int)RecordID).Tables[1];
        _theRecord = RecordManager.ets_Record_Detail_Full((int)RecordID);
       
        AddEditInfo();



       

        if (_strRecordRightID == Common.UserRoleType.OwnData)
        {

            hlEdit.Visible = false;


            if (_theRecord != null)
            {
                if (_theRecord.OwnerUserID.ToString() == _objUser.UserID.ToString())
                {
                    hlEdit.Visible = true;
                }
            }

        }

        if (_strRecordRightID == Common.UserRoleType.EditOwnViewOther)
        {

            hlEdit.Visible = false;

            if (_theRecord != null)
            {
                if (_theRecord.EnteredBy.ToString() == _objUser.UserID.ToString())
                {
                    hlEdit.Visible = true;
                }
            }

        }



        //Get warning and validation here
        string strWarning = "";
        string strValidation = "";

        for (int i = 0; i < _dtRecordedetail.Columns.Count; i++)
        {
            if (_dtRecordedetail.Columns[i].ColumnName.ToLower() == "warningresults")
            {
                strWarning = _dtRecordedetail.Rows[0][i].ToString();
            }
            else if (_dtRecordedetail.Columns[i].ColumnName.ToLower() == "validationresults")
            {
                strValidation = _dtRecordedetail.Rows[0][i].ToString();
            }

        }


        //populate record

        for (int i = 0; i < _dtRecordedetail.Columns.Count; i++)
        {
            if (i == _iDateTimeRecorded)
            {
                //_txtValue[i].Text = _dtRecordedetail.Rows[0][i].ToString().Substring(0, _dtRecordedetail.Rows[0][i].ToString().IndexOf(' '));
                DateTime dtTempDateTimeRecorded = DateTime.Parse(_dtRecordedetail.Rows[0][i].ToString());
                _txtValue[i].Text = dtTempDateTimeRecorded.Day.ToString("00") + "/" + dtTempDateTimeRecorded.Month.ToString("00") + "/" + dtTempDateTimeRecorded.Year.ToString();

                //if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() != "date")
                //{
                if (_txtTime[i] != null)
                    _txtTime[i].Text = Convert.ToDateTime(dtTempDateTimeRecorded.ToString()).ToString("HH:m");
                //}

                //check max time between Records.
                if (strWarning.IndexOf(WarningMsg.MaxtimebetweenRecords) >= 0)
                {
                    _txtValue[i].ForeColor = System.Drawing.Color.Blue;
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() != "date")
                    {
                        if (_txtTime[i] != null)
                            _txtTime[i].ForeColor = System.Drawing.Color.Blue;
                    }
                }

            }

        }

        for (int i = 0; i < _dtRecordedetail.Columns.Count; i++)
        {
            //if (i == _iLocationIndex)
            //{
            //    _ddlLocation.Text = _dtRecordedetail.Rows[0][i].ToString();
            //}
            //else 
            string strEachFormulaV = "";
            string strEachFormulaW = "";
            string strEachFormulaE = "";
            string strEachFormulaV_Msg = "";
            string strEachFormulaW_Msg = "";
            string strEachFormulaE_Msg = "";

            if (i < _dtColumnsDetail.Columns.Count && i < _lbl.Length && (_dtColumnsDetail.Rows[i]["Importance"].ToString().ToLower() == "m" || _dtColumnsDetail.Rows[i]["Importance"].ToString().ToLower() == "r"))
            {
                if (_dtRecordedetail.Rows[0][i].ToString() != "")
                {
                    _lbl[i].Style.Add("color", "#565656");
                }
                else
                {
                    _lbl[i].Style.Add("color", "Red");
                }
            }





            if (i == _iEnteredByIndex)
            {
                _ddlEnteredBy.Text = _dtRecordedetail.Rows[0][i].ToString();
            }
            else if (i == _iIsActiveIndex)
            {
                _chkIsActive.Checked = bool.Parse(_dtRecordedetail.Rows[0][i].ToString());

            }
            else if (i == _iDateTimeRecorded)
            {
                //_txtValue[i].Text = _dtRecordedetail.Rows[0][i].ToString().Substring(0, _dtRecordedetail.Rows[0][i].ToString().IndexOf(' '));
                DateTime dtTempDateTimeRecorded = DateTime.Parse(_dtRecordedetail.Rows[0][i].ToString());
                _txtValue[i].Text = dtTempDateTimeRecorded.Day.ToString("00") + "/" + dtTempDateTimeRecorded.Month.ToString("00") + "/" + dtTempDateTimeRecorded.Year.ToString();

                //if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() != "date")
                //{
                if (_txtTime[i] != null)
                    _txtTime[i].Text = Convert.ToDateTime(dtTempDateTimeRecorded.ToString()).ToString("HH:m");
                //}

                //check max time between Records.
                if (strWarning.IndexOf("" + WarningMsg.MaxtimebetweenRecords + "") >= 0)
                {
                    _txtValue[i].ForeColor = System.Drawing.Color.Blue;
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() != "date")
                    {
                        _txtTime[i].ForeColor = System.Drawing.Color.Blue;
                    }
                }

            }
            else if (i == _iTableIndex)
            {
                //Table theTable = RecordManager.ets_Table_Details(int.Parse(_dtRecordedetail.Rows[0][i].ToString()));

                _txtValue[i].Text = _theTable.TableName;
            }
            else if (_dtRecordedetail.Columns[i].ColumnName.ToLower() == "warningresults")
            {
                if (_dtRecordedetail.Rows[0][i].ToString() != "")
                {
                    _lblWarningResults.Visible = true;
                    _lblWarningResultsValue.Text = _dtRecordedetail.Rows[0][i].ToString();
                }
                else
                {
                    _lblWarningResults.Visible = false;
                }
            }
            else if (_dtRecordedetail.Columns[i].ColumnName.ToLower() == "validationresults")
            {
                //do nothing
                _txtValidationResults.Text = _dtRecordedetail.Rows[0][i].ToString();
            }
            else
            {

                if (_dtColumnsDetail.Rows[i]["ConV"] != DBNull.Value)
                {
                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ConV"].ToString()));
                    if (theCheckColumn != null)
                    {
                        string strCheckValue = RecordManager.GetRecordValue(ref _theRecord, theCheckColumn.SystemName);
                        strEachFormulaV = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                            "V", strCheckValue);
                    }
                }
                else
                {
                    if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"] != DBNull.Value && _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                    {
                        strEachFormulaV = _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString();
                    }
                }

                if (_dtColumnsDetail.Rows[i]["ConW"] != DBNull.Value)
                {
                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ConW"].ToString()));
                    if (theCheckColumn != null)
                    {
                        string strCheckValue = RecordManager.GetRecordValue(ref _theRecord, theCheckColumn.SystemName);
                        strEachFormulaW = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                            "W", strCheckValue);
                    }
                }
                else
                {
                    if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"] != DBNull.Value && _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                    {
                        strEachFormulaW = _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString();
                    }
                }



                if (_dtColumnsDetail.Rows[i]["ConE"] != DBNull.Value)
                {
                    Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ConE"].ToString()));
                    if (theCheckColumn != null)
                    {
                        string strCheckValue = RecordManager.GetRecordValue(ref _theRecord, theCheckColumn.SystemName);
                        strEachFormulaE = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                            "E", strCheckValue);
                    }
                }
                else
                {
                    if (_dtColumnsDetail.Rows[i]["ValidationOnExceedance"] != DBNull.Value && _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                    {
                        strEachFormulaE = _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString();
                    }
                }

                strEachFormulaV_Msg = Common.GetFromulaMsg("i", _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), strEachFormulaV);
                strEachFormulaW_Msg = Common.GetFromulaMsg("w", _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), strEachFormulaW);
                strEachFormulaE_Msg = Common.GetFromulaMsg("e", _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), strEachFormulaE);




                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date" &&
                    _dtRecordedetail.Rows[0][i].ToString() != "")
                {
                    try
                    {
                        DateTime dtTempDateTime = DateTime.Parse(_dtRecordedetail.Rows[0][i].ToString());
                        _txtValue[i].Text = dtTempDateTime.Day.ToString("00") + "/" + dtTempDateTime.Month.ToString("00") + "/" + dtTempDateTime.Year.ToString();
                    }
                    catch
                    {

                    }


                }

                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time" &&
                   _dtRecordedetail.Rows[0][i].ToString() != "")
                {
                    try
                    {
                        _txtValue[i].Text = Convert.ToDateTime(_dtRecordedetail.Rows[0][i].ToString()).ToString("HH:m");//HH:mm:ss
                    }
                    catch
                    {
                        //
                    }
                }

                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime" &&
                   _dtRecordedetail.Rows[0][i].ToString() != "")
                {
                    try
                    {
                        DateTime dtTempDateTime = DateTime.Parse(_dtRecordedetail.Rows[0][i].ToString());
                        _txtValue[i].Text = dtTempDateTime.Day.ToString("00") + "/" + dtTempDateTime.Month.ToString("00") + "/" + dtTempDateTime.Year.ToString();


                        _txtTime[i].Text = Convert.ToDateTime(dtTempDateTime.ToString()).ToString("HH:m");
                    }
                    catch
                    {

                    }


                }


                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file" &&
                    _dtRecordedetail.Rows[0][i].ToString() != "")
                {
                    //_lblValue[i].Text = "<a target='_blank' href='" + _strFilesLocation + "/UserFiles/AppFiles/"
                    //       + _dtRecordedetail.Rows[0][i].ToString() + "'>" +
                    //       _dtRecordedetail.Rows[0][i].ToString().Substring(37) + "</a>";

                    string strFilePath = Cryptography.Encrypt(_strFilesLocation + "/UserFiles/AppFiles/" + _dtRecordedetail.Rows[0][i].ToString());
                    string strFileName = Cryptography.Encrypt(_dtRecordedetail.Rows[0][i].ToString().Substring(37));

                    _lblValue[i].Text = "<a target='_blank' href='" + Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Filedownload.aspx?FilePath="
                        + strFilePath + "&FileName=" + strFileName + "'>" +
                          _dtRecordedetail.Rows[0][i].ToString().Substring(37) + "</a>";

                    
                }

                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "trafficlight"
                        && _dtColumnsDetail.Rows[i]["TrafficLightColumnID"] != DBNull.Value
                        && _dtColumnsDetail.Rows[i]["TrafficLightValues"] != DBNull.Value)
                {
                    Column theTrafficLightColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["TrafficLightColumnID"].ToString()));
                    if (theTrafficLightColumn != null && _imgValues[i] != null)
                    {
                        string strTLValue = Common.GetValueFromSQL("SELECT " + theTrafficLightColumn.SystemName + " FROM [Record] WHERE RecordID=" + _theRecord.RecordID.ToString());
                        string strImageURL = Common.TrafficLightURL(theTrafficLightColumn, strTLValue, _dtColumnsDetail.Rows[i]["TrafficLightValues"].ToString());


                        if (strImageURL != "")
                        {
                            _imgValues[i].ImageUrl = "~" + strImageURL;
                        }

                    }
                }


                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image" &&
                    _dtRecordedetail.Rows[0][i].ToString() != "")
                {
                    string strMaxHeight = "50";
                    if (_dtColumnsDetail.Rows[i]["TextHeight"] != DBNull.Value)
                    {
                        strMaxHeight = _dtColumnsDetail.Rows[i]["TextHeight"].ToString();
                    }

                    string strFilePath = _strFilesLocation + "/UserFiles/AppFiles/"
                            + _dtRecordedetail.Rows[0][i].ToString();
                    _lblValue[i].Text = "<a target='_blank' href='" + strFilePath + "'>"
                        + "<img style='padding-bottom:7px; max-height:" + strMaxHeight + "px;' alt='" + _dtRecordedetail.Rows[0][i].ToString().Substring(37)
                        + "' src='" + strFilePath + "' title='" + _dtRecordedetail.Rows[0][i].ToString().Substring(37) + "'  />" + "</a><br/>";

                   
                }


                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                        && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                        || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                      && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                      && _dtColumnsDetail.Rows[i]["TableTableID"].ToString() == "-1"
                        && _dtColumnsDetail.Rows[i]["LinkedParentColumnID"] == DBNull.Value
                      && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "")
                {
                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        string strColumnUserID = _dtRecordedetail.Rows[0][i].ToString();
                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                        {
                            _hfValue[i].Value = strColumnUserID;
                            _txtValue[i].Text = RecordManager.GetUserDisplayName(_dtColumnsDetail.Rows[i]["DisplayColumn"].ToString(),
                                _hfValue[i].Value);
                        }
                        else
                        {
                            if (_ddlValue[i].Items.FindByValue(strColumnUserID) != null)
                                _ddlValue[i].SelectedValue = strColumnUserID;

                        }
                    }
                }


                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                    && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                    || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                  && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                    && _dtColumnsDetail.Rows[i]["LinkedParentColumnID"] != DBNull.Value
                  && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "")
                {

                    //if (_dtRecordTypleColumlns.Rows[i]["ParentColumnID"] == DBNull.Value)
                    //{
                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        try
                        {
                            //Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordTypleColumlns.Rows[i]["LinkedParentColumnID"].ToString()));

                            //string strLinkedColumnValue = _dtRecordedetail.Rows[0][i].ToString();
                            string strParentRecordID = _dtRecordedetail.Rows[0][i].ToString();
                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                            {
                                _hfValue[i].Value = strParentRecordID;
                                _txtValue[i].Text = Common.GetLinkedDisplayText(_dtColumnsDetail.Rows[i]["DisplayColumn"].ToString(), int.Parse(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()), null, " AND Record.RecordID=" + strParentRecordID, "");
                                //                                    _hfValue[i].Value = strLinkedColumnValue;
                                //                                    DataTable dtTableTableSC = Common.DataTableFromText(@"SELECT SystemName,DisplayName 
                                //                                FROM [Column] WHERE   TableID ="
                                //                               + _dtRecordTypleColumlns.Rows[i]["TableTableID"].ToString());

                                //                                    string strDisplayColumn = _dtRecordTypleColumlns.Rows[i]["DisplayColumn"].ToString();

                                //                                    foreach (DataRow dr in dtTableTableSC.Rows)
                                //                                    {
                                //                                        strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                                //                                    }

                                //                                    string sstrDisplayColumnOrg = strDisplayColumn;
                                //                                    string strFilterSQL = "";
                                //                                    if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                //                                    {
                                //                                        strFilterSQL = strLinkedColumnValue;
                                //                                    }
                                //                                    else
                                //                                    {
                                //                                        strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                                //                                    }

                                //                                    DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

                                //                                    if (dtTheRecord.Rows.Count > 0)
                                //                                    {

                                //                                        foreach (DataColumn dc in dtTheRecord.Columns)
                                //                                        {
                                //                                            Column theColumn = RecordManager.ets_Column_Details_By_Sys((int)theLinkedColumn.TableID, dc.ColumnName);
                                //                                            if (theColumn != null)
                                //                                            {
                                //                                                if (theColumn.ColumnType == "date")
                                //                                                {
                                //                                                    string strDatePartOnly = dtTheRecord.Rows[0][dc.ColumnName].ToString();

                                //                                                    if (strDatePartOnly.Length > 9)
                                //                                                    {
                                //                                                        strDatePartOnly = strDatePartOnly.Substring(0, 10);
                                //                                                    }

                                //                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", strDatePartOnly);
                                //                                                }
                                //                                                else
                                //                                                {
                                //                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                //                                                }
                                //                                            }

                                //                                        }
                                //                                    }
                                //                                    if (sstrDisplayColumnOrg != strDisplayColumn)
                                //                                        _txtValue[i].Text = strDisplayColumn;

                            }

                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                            {
                                //_ddlValue[i].Text = strLinkedColumnValue;
                                if (_ddlValue[i].Items.FindByValue(strParentRecordID) != null)
                                    _ddlValue[i].SelectedValue = strParentRecordID;
                            }
                        }
                        catch
                        {
                            //
                        }

                    }

                    //}
                    //else
                    //{
                    //    //Filtered
                    //    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    //    {
                    //        try
                    //        {

                    //            Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordTypleColumlns.Rows[i]["LinkedParentColumnID"].ToString()));

                    //            string strLinkedColumnValue = _dtRecordedetail.Rows[0][i].ToString();
                    //            string strFilterSQL = "";
                    //            if (theLinkedColumn.SystemName.ToLower() == "recordid")
                    //            {
                    //                strFilterSQL = strLinkedColumnValue;
                    //            }
                    //            else
                    //            {
                    //                strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                    //            }

                    //            //Record theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(_dtRecordedetail.Rows[0][i].ToString()));
                    //            Record theRecord = null;
                    //            DataTable dtTheRecord = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);
                    //            if (dtTheRecord.Rows.Count > 0)
                    //            {
                    //                foreach (DataRow drR in dtTheRecord.Rows)
                    //                {
                    //                    theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(drR["RecordID"].ToString()));
                    //                    break;
                    //                }
                    //            }

                    //            Column scParentColumnID = RecordManager.ets_Column_Details(int.Parse(_dtRecordTypleColumlns.Rows[i]["ParentColumnID"].ToString()));
                    //            if (scParentColumnID != null)
                    //            {
                    //                if (theRecord != null)
                    //                    _ddlValue2[i].SelectedValue = RecordManager.GetRecordValue(ref theRecord, scParentColumnID.SystemName);

                    //                _ccddl[i].SelectedValue = _dtRecordedetail.Rows[0][i].ToString();
                    //            }
                    //        }
                    //        catch
                    //        {
                    //            //
                    //        }
                    //    }

                    //}



                }


                //if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() == "data_retriever")
                //{
                //    if (_dtRecordTypleColumlns.Rows[i]["DataRetrieverID"] != DBNull.Value)
                //    {
                //        DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail(int.Parse(_dtRecordTypleColumlns.Rows[i]["DataRetrieverID"].ToString()), null, null);

                //        if (theDataRetriever.CodeSnippet != "")
                //        {

                //            _txtValue[i].Text = Common.GetValueFromSQL(theDataRetriever.CodeSnippet.Replace("#ID#", RecordID.ToString()));
                //        }
                //    }
                //}




                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation")
                {

                    _txtValue[i].Text = _dtRecordedetail.Rows[0][i].ToString();

                    if (_dtColumnsDetail.Rows[i]["Calculation"] != DBNull.Value)
                    {
                        bool bDateCal = false;
                        bool bTextCal = false;
                        if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                            && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "d")
                        {
                            bDateCal = true;
                            //string strCalculation = _dtRecordTypleColumlns.Rows[i]["Calculation"].ToString();

                            //try
                            //{
                            //    _txtValue[i].Text = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, (int)RecordID, _iParentRecordID,
                            //        _dtRecordTypleColumlns.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtRecordTypleColumlns.Rows[i]["DateCalculationType"].ToString());
                            //}
                            //catch
                            //{

                            //}
                        }
                        else if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                           && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "t")
                        {
                            bTextCal = true;
                        }
                        //else
                        //{
                        //    string strFormula = TheDatabaseS.GetCalculationFormula(TableID, _dtRecordTypleColumlns.Rows[i]["Calculation"].ToString());


                        //    //_txtValue[i].Text = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + RecordID.ToString());
                        //    _txtValue[i].Text = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, (int)RecordID, i, _iParentRecordID);
                        //}

                        if (_dtColumnsDetail.Rows[i]["RoundNumber"] != DBNull.Value && bDateCal == false && bTextCal == false)
                        {

                            if (_txtValue[i].Text.ToString() != "")
                            {
                                try
                                {
                                    if (_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
                                    {
                                        if (Common.HasSymbols(_txtValue[i].Text) == false)
                                            _txtValue[i].Text = Math.Round(double.Parse(Common.IgnoreSymbols(_txtValue[i].Text)), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                                    }
                                    else
                                    {
                                        _txtValue[i].Text = Math.Round(double.Parse(Common.IgnoreSymbols(_txtValue[i].Text)), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                                    }

                                }
                                catch
                                {
                                    //
                                }

                            }


                        }


                        if (_txtValue[i].Text != "" && bDateCal == false && bTextCal == false)
                        {
                            try
                            {
                                _txtValue[i].Text = Common.IgnoreSymbols(_txtValue[i].Text);



                                if (_ftbExt[i] != null)
                                    _ftbExt[i].Enabled = false;
                            }
                            catch
                            {

                            }

                        }



                    }
                }




                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "text"
                     || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                {

                    _txtValue[i].Text = _dtRecordedetail.Rows[0][i].ToString();


                    if (_dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "link"
                        && _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "text" && _hlValue[i] != null)
                    {
                        if (_txtValue[i].Text != "")
                        {
                            string strLinkURL = _txtValue[i].Text;

                            if (strLinkURL.IndexOf("http") == -1)
                            {
                                strLinkURL = Request.Url.Scheme + "://" + strLinkURL;
                            }
                            _hlValue[i].NavigateUrl = strLinkURL;
                            _hlValue[i].Text = "Go";
                        }
                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number"
                        && _dtColumnsDetail.Rows[i]["NumberType"] != null)
                    {


                        //Financial
                        if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "6")
                        {
                            if (_txtValue[i].Text != "")
                            {
                                try
                                {
                                    _txtValue[i].Text = Common.IgnoreSymbols(_txtValue[i].Text);
                                    _txtValue[i].Text = double.Parse(_txtValue[i].Text).ToString("C").Substring(1);

                                    if (_ftbExt[i] != null)
                                        _ftbExt[i].Enabled = false;

                                }
                                catch
                                {
                                    //
                                }
                            }
                        }


                        //Record Count
                        if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "5")
                        {

                            if (_lblValue[i] != null)
                            {
                                //get recrod count on the fly
                                //DataTable dtCT = Common.DataTableFromText("SELECT * FROM TableChild WHERE ParentTableID=" + _qsTableID + " AND DetailPageType<>'not' ORDER BY TableChildID");

                                //foreach (DataRow dr in dtCT.Rows)
                                //{
                                Table theTable = RecordManager.ets_Table_Details(int.Parse(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()));
                                string strTextSearch = "";
                                if (theTable != null)
                                {

                                    DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,ColumnID FROM [Column] WHERE   TableID=" + _dtColumnsDetail.Rows[i]["TableTableID"].ToString() + " AND TableTableID=" + TableID.ToString());
                                    foreach (DataRow drCT in dtTemp.Rows)
                                    {

                                        Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(drCT["ColumnID"].ToString()));
                                        Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                                        Record theLinkedRecord = RecordManager.ets_Record_Detail_Full((int)RecordID);
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
                                    RecordManager.ets_Record_List(int.Parse(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()), null, true,
                                   false, null, null,
                                   "", "", 0, 1, ref iTNTemp, ref _iTotalDynamicColumnsTem, "", "", strTextSearch, null, null,
                                   "", "", "", null, ref strReturnSQL, ref strReturnSQL);

                                    if (_dtColumnsDetail.Rows[i]["DropDownValues"].ToString() == "no")
                                    {
                                        _lblValue[i].Text = iTNTemp.ToString();
                                    }
                                    else
                                    {
                                        if (iTNTemp == 0)
                                        {
                                            _lblValue[i].Text = iTNTemp.ToString();
                                        }
                                        else
                                        {
                                            string strChildTableLink = " <a href='RecordList.aspx?TableID=" + Cryptography.Encrypt(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()) + "&TextSearch=" + Cryptography.Encrypt(strTextSearch) + "' target='_blank'>" + iTNTemp.ToString() + "</a>";
                                            _lblValue[i].Text = Server.HtmlDecode(strChildTableLink);
                                        }

                                    }

                                }

                                //}

                                //}



                            }
                        }
                    }


                    if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value)
                    {
                        if (_dtColumnsDetail.Rows[i]["IsRound"].ToString().ToLower() == "true")
                        {
                            if (_txtValue[i].Text.ToString() != "")
                            {
                                try
                                {
                                    if (_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
                                    {
                                        if (Common.HasSymbols(_txtValue[i].Text) == false)
                                            _txtValue[i].Text = Math.Round(double.Parse(Common.IgnoreSymbols(_txtValue[i].Text)), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                                    }
                                    else
                                    {
                                        _txtValue[i].Text = Math.Round(double.Parse(Common.IgnoreSymbols(_txtValue[i].Text)), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                                    }

                                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number"
                       && _dtColumnsDetail.Rows[i]["NumberType"] != null)
                                    {
                                        //Financial
                                        if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "6")
                                        {
                                            if (_txtValue[i].Text != "")
                                            {
                                                _txtValue[i].Text = Common.IgnoreSymbols(_txtValue[i].Text);
                                                _txtValue[i].Text = double.Parse(_txtValue[i].Text).ToString("C").Substring(1);
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

                    }
                }


                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation"
                    || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                {

                    //check warning and validation
                    if (TheDatabase.HasWarning_msg(strWarning, _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), ""))
                    {
                        _txtValue[i].ForeColor = System.Drawing.Color.Blue;
                    }
                    if (TheDatabase.HasExceedance_msg(strWarning, _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), ""))
                    {
                        _txtValue[i].ForeColor = System.Drawing.Color.Orange;
                    }
                    if (TheDatabase.HasInvalidIgnored_msg(strWarning, _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), ""))
                    {
                        _txtValue[i].ForeColor = System.Drawing.Color.Red;
                    }



                    if (strValidation.IndexOf(": " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                    {
                        _txtValue[i].ForeColor = System.Drawing.Color.Red;
                    }

                    //check specific warning
                    string strToopTip = "";
                    if (strWarning != "")
                    {
                        if (TheDatabase.HasWarning_msg(strWarning, _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), "l"))
                        {
                            _imgWarning[i].Visible = true;
                            strToopTip = strEachFormulaW_Msg;// "Value outside accepted range(" + strEachFormulaW + ").";
                            _imgWarning[i].ToolTip = strToopTip;
                        }

                        if (TheDatabase.HasExceedance_msg(strWarning, _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), ""))
                        {
                            _imgWarning[i].Visible = true;
                            strToopTip = strEachFormulaE_Msg;// "Value outside accepted range(" + strEachFormulaE + ").";
                            _imgWarning[i].ToolTip = strToopTip;
                            _imgWarning[i].ImageUrl = _imgWarning[i].ImageUrl.Replace("warning.png", "exceedance.png");
                        }

                        if (TheDatabase.HasInvalidIgnored_msg(strWarning, _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), ""))
                        {
                            _imgWarning[i].Visible = true;
                            strToopTip = strEachFormulaV_Msg;// "INVALID (and ignored):" + strEachFormulaV + ".";
                            _imgWarning[i].ToolTip = strToopTip;
                            _imgWarning[i].ImageUrl = _imgWarning[i].ImageUrl.Replace("warning.png", "Invalid.png");
                            _imgWarning[i].ImageUrl = _imgWarning[i].ImageUrl.Replace("exceedance.png", "Invalid.png");
                        }
                    }

                    if (strWarning != "")
                    {
                        if (TheDatabase.HasWarningUnlikely_msg(strWarning, _dtColumnsDetail.Rows[i]["DisplayName"].ToString(), "l"))
                        {
                            _imgWarning[i].Visible = true;
                            _imgWarning[i].ToolTip = strToopTip + "Unlikely data – outside 3 standard deviations.";
                        }
                    }



                    ////check warning and validation
                    //if (strWarning.IndexOf(": " + _dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString()) >= 0)
                    //{
                    //    _txtValue[i].ForeColor = System.Drawing.Color.Blue;
                    //}

                    //if (strValidation.IndexOf(": " + _dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString()) >= 0)
                    //{
                    //    _txtValue[i].ForeColor = System.Drawing.Color.Red;
                    //}

                    ////check specific warning
                    //string strToopTip = "";
                    //if (strWarning != "")
                    //{
                    //    if (strWarning.IndexOf(": " + _dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range") >= 0)
                    //    {
                    //        _imgWarning[i].Visible = true;
                    //        strToopTip = "Value outside accepted range(" + _dtRecordTypleColumlns.Rows[i]["ValidationOnWarning"].ToString() + ").";
                    //        _imgWarning[i].ToolTip = strToopTip;
                    //    }
                    //}

                    //if (strWarning != "")
                    //{
                    //    if (strWarning.IndexOf(": " + _dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.") >= 0)
                    //    {
                    //        _imgWarning[i].Visible = true;
                    //        _imgWarning[i].ToolTip = strToopTip + "Unlikely data – outside 3 standard deviations.";
                    //    }
                    //}
                }

                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                {
                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        try
                        {
                            _radioList[i].SelectedValue = _dtRecordedetail.Rows[0][i].ToString();
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = ex.StackTrace;
                        }
                    }
                }

                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                {
                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        try
                        {
                            Common.SetCheckBoxValue(_dtColumnsDetail.Rows[i]["DropDownValues"].ToString(), _dtRecordedetail.Rows[0][i].ToString(), ref _chkValue[i]);
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = ex.StackTrace;
                        }
                    }
                }
                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                {
                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        try
                        {
                            LocationColumn theLocationColumn = JSONField.GetTypedObject<LocationColumn>(_dtRecordedetail.Rows[0][i].ToString());
                            if (theLocationColumn != null)
                            {
                                if (_hfValue[i] != null && theLocationColumn.Latitude != null)
                                    _hfValue[i].Value = theLocationColumn.Latitude.ToString();

                                if (_hfValue2[i] != null && theLocationColumn.Longitude != null)
                                    _hfValue2[i].Value = theLocationColumn.Longitude.ToString();

                                if (_hfValue3[i] != null && theLocationColumn.ZoomLevel != null)
                                    _hfValue3[i].Value = theLocationColumn.ZoomLevel.ToString();

                                if (_txtTime[i] != null && _txtValue[i] != null)
                                {
                                    if (theLocationColumn.Latitude != null)
                                        _txtValue[i].Text = theLocationColumn.Latitude.ToString();

                                    if (theLocationColumn.Longitude != null)
                                        _txtTime[i].Text = theLocationColumn.Longitude.ToString();
                                }

                                if (_txtValue2[i] != null)
                                {
                                    if (theLocationColumn.Address != "")
                                        _txtValue2[i].Text = theLocationColumn.Address;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = ex.StackTrace;
                        }
                        //
                    }
                }

                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                {
                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        _htmValue[i].Text = _dtRecordedetail.Rows[0][i].ToString();
                    }
                }




                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                     && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "")
                {
                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        try
                        {
                            //SetListValues(_dtRecordedetail.Rows[0][i].ToString(), ref _lstValue[i]);

                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                            {
                                Common.SetListValues(_dtRecordedetail.Rows[0][i].ToString(), ref _lstValue[i], _dtColumnsDetail.Rows[i]["DropdownValues"].ToString());
                            }
                            else if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text")
                            {
                                Common.SetListValues_Text(_dtRecordedetail.Rows[0][i].ToString(), ref _lstValue[i], _dtColumnsDetail.Rows[i]["DropdownValues"].ToString());
                            }
                            else
                            {
                                Common.SetListValues_ForTable(_dtRecordedetail.Rows[0][i].ToString(), ref _lstValue[i],
                                    (int)_dtColumnsDetail.Rows[i]["TableTableID"], null, _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString());
                            }


                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = ex.StackTrace;
                        }
                    }
                }


                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                       && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "checkbox")
                {

                    if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                    {
                        Common.PutCheckBoxListValues(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _cblValue[i]);
                    }
                    else
                    {
                        Common.PutCheckBoxListValues_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _cblValue[i]);
                    }


                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        try
                        {
                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                            {
                                Common.SetCheckBoxListValues(_dtRecordedetail.Rows[0][i].ToString(), ref _cblValue[i], _dtColumnsDetail.Rows[i]["DropdownValues"].ToString());
                            }
                            else if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text")
                            {
                                Common.SetCheckBoxListValues_Text(_dtRecordedetail.Rows[0][i].ToString(), ref _cblValue[i], _dtColumnsDetail.Rows[i]["DropdownValues"].ToString());
                            }
                            else
                            {
                                if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                             && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                        && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "")
                                {
                                    Common.SetCheckBoxListValues_ForTable(_dtRecordedetail.Rows[0][i].ToString(), ref _cblValue[i],
                                        (int)_dtColumnsDetail.Rows[i]["TableTableID"], null, _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString());
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = ex.StackTrace;
                        }
                    }
                    //else
                    //{
                    //    foreach (ListItem li in _cblValue[i].Items)
                    //    {
                    //        li.Selected=false;
                    //    }

                    //}

                }




                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                   && _dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                    && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                    || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                {
                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        try
                        {
                            _ddlValue[i].Text = _dtRecordedetail.Rows[0][i].ToString();
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = ex.StackTrace;
                        }


                        //check warning and validation
                        if (strWarning.IndexOf(": " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                        {
                            _ddlValue[i].ForeColor = System.Drawing.Color.Blue;
                        }

                        if (strValidation.IndexOf(": " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                        {
                            _ddlValue[i].ForeColor = System.Drawing.Color.Red;
                        }

                        //check specific warning
                        string strToopTip = "";
                        if (strWarning != "")
                        {
                            if (strWarning.IndexOf(": " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range") >= 0)
                            {
                                _imgWarning[i].Visible = true;
                                strToopTip = "Value outside accepted range(" + _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString() + ").";
                                _imgWarning[i].ToolTip = strToopTip;
                            }
                        }
                    }

                }

            }
        }


        ShowHideNextPrev();
        ShowHideControls();
        ShowHideTableTab();

        //if (!IsPostBack)
        //{
            if (_jsClickFirstTab != "")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strTableTabsJSFirstTab" + _strDynamictabPart, _jsClickFirstTab, true);

        //}



    }

    protected void EnableTheRecordControls(bool p_bEnable)
    {     
        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {

            switch (_dtColumnsDetail.Rows[i]["SystemName"].ToString().ToLower())
            {
                case "recordid":
                    break;
                //case "locationid":
                //    _ddlLocation.Enabled = p_bEnable;
                //    _hlSSAdd.Visible = p_bEnable;
                //    break;
                case "enteredby":
                    _ddlEnteredBy.Enabled = p_bEnable;
                    break;
                default:

                    if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString().ToLower() == "")
                    {
                        if (_txtValue[i] != null)
                            _txtValue[i].Enabled = p_bEnable;

                        if (_ddlValue[i] != null)
                            _ddlValue[i].Enabled = p_bEnable;
                    }
                    else
                    {
                        if (_ddlValue[i] != null)
                            _ddlValue[i].Enabled = p_bEnable;
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() != "date")
                    {
                        if (_txtTime[i] != null)
                        {
                            _txtTime[i].Enabled = p_bEnable;
                            //_imgTrigger.Visible = false;
                        }
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                    {
                        if (_fuValue[i] != null)
                            _fuValue[i].Enabled = false;
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                        && _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                    {
                        if (_ddlValue2[i] != null)
                            _ddlValue2[i].Enabled = false;

                        if (_txtValue[i] != null)
                            _txtValue[i].Enabled = false;
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                        && _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                    {
                        if (_ddlValue[i] != null)
                            _ddlValue[i].Enabled = false;

                        if (_ddlValue2[i] != null)
                            _ddlValue2[i].Enabled = false;

                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                    {
                        if (_radioList[i] != null)
                            _radioList[i].Enabled = false;
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox")
                    {
                        if (_lstValue[i] != null)
                            _lstValue[i].Enabled = false;
                        if (_cblValue[i] != null)
                            _cblValue[i].Enabled = false;
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                    {
                        if (_chkValue[i] != null)
                            _chkValue[i].Enabled = false;
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                    {
                        if (_txtValue[i] != null)
                            _txtValue[i].Enabled = false;
                        if (_txtValue2[i] != null)
                            _txtValue2[i].Enabled = false;
                        if (_txtTime[i] != null)
                            _txtTime[i].Enabled = false;
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                    {
                        if (_htmValue[i] != null)
                            _htmValue[i].Enabled = false;
                        //if (_heeValue[i] != null)
                        //    _heeValue[i].Enabled = false;
                    }


                    break;

            }
        }

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }

    protected void CreateTableTab()
    {
     
        if (_dtDBTableTab != null)
        {
            if (_dtDBTableTab.Rows.Count > 1)
            {            
                _bTableTabYes = true;
                pnlDetailTab.CssClass = "showhidedivs" + _theTable.TableID.ToString();  
                _pnlDetailTabD = new Panel[_dtDBTableTab.Rows.Count];
                _tblMainD = new HtmlTable[_dtDBTableTab.Rows.Count];
                _tblLeftD = new HtmlTable[_dtDBTableTab.Rows.Count];
                _tblRightD = new HtmlTable[_dtDBTableTab.Rows.Count];
                
                //lets create Tabs

                for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                {                   
                    if (t == 0)
                    {
                        LinkButton lnkDetialTab = new LinkButton();
                        lnkDetialTab.ID = "lnkDetialTab" + _theTable.TableID.ToString();
                        lnkDetialTab.Text = _dtDBTableTab.Rows[t]["TabName"].ToString(); //"Detail";
                        lnkDetialTab.Font.Bold = true;
                        _jsClickFirstTab = "ShowHideMainDivs" + _theTable.TableID.ToString() + "('" + pnlDetailTab.ClientID + "',this," + _dtDBTableTab.Rows[t]["TableTabID"].ToString() + ");";
                        lnkDetialTab.Attributes.Add("onclick", "ShowHideMainDivs" + _theTable.TableID.ToString() + "('" + pnlDetailTab.ClientID + "',this," + _dtDBTableTab.Rows[t]["TableTabID"].ToString() + "); return false");
                        lnkDetialTab.CssClass = "TablLinkClass" + _theTable.TableID.ToString();
                        
                        lnkDetialTab.CausesValidation = false;
                        pnlTabHeading.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp"));
                        pnlTabHeading.Controls.Add(lnkDetialTab);
                        pnlTabHeading.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp"));

                    }
                    else
                    {


                        _pnlDetailTabD[t] = new Panel();
                        _pnlDetailTabD[t].ID = "pnlDetailTabD" + _theTable.TableID.ToString() + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        _pnlDetailTabD[t].CssClass = "showhidedivs" + _theTable.TableID.ToString();
                        _pnlDetailTabD[t].ClientIDMode = ClientIDMode.Static;

                        LinkButton lnkDetialTabD = new LinkButton();
                        lnkDetialTabD.ID = "lnkDetialTabD" + _theTable.TableID.ToString() + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        lnkDetialTabD.Text = _dtDBTableTab.Rows[t]["TabName"].ToString();
                        lnkDetialTabD.OnClientClick = "ShowHideMainDivs" + _theTable.TableID.ToString() + "('" + _pnlDetailTabD[t].ClientID + "',this," + _dtDBTableTab.Rows[t]["TableTabID"].ToString() + "); return false";
                        lnkDetialTabD.CssClass = "TablLinkClass" + _theTable.TableID.ToString();
                       
                        pnlTabHeading.Controls.Add(lnkDetialTabD);
                        pnlTabHeading.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp"));

                        _tblMainD[t] = new HtmlTable();

                        HtmlTableRow hrTemp = new HtmlTableRow();
                        HtmlTableCell cellLeft = new HtmlTableCell();
                        HtmlTableCell cellRight = new HtmlTableCell();

                        hrTemp.Cells.Add(cellLeft);
                        hrTemp.Cells.Add(cellRight);
                        _tblMainD[t].Rows.Add(hrTemp);

                        _tblLeftD[t] = new HtmlTable();
                        _tblLeftD[t].ID = "tblLeftD" + _theTable.TableID.ToString() + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        cellLeft.Controls.Add(_tblLeftD[t]);

                        _tblRightD[t] = new HtmlTable();
                        _tblRightD[t].ID = "tblRightD" + _theTable.TableID.ToString() + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        cellRight.Controls.Add(_tblRightD[t]);

                        _pnlDetailTabD[t].Controls.Add(_tblMainD[t]);

                        pnlMain.Controls.Add(_pnlDetailTabD[t]);

                    }

                }



                string strTableTabsJS = @" function ShowHideMainDivs" + _theTable.TableID.ToString() + @"(divSelected,lnk,ttID) {
                                        var divSelectedO = document.getElementsByName(divSelected.toString());
                                            $('.showhidedivs_hide').hide();
                                            if (divSelectedO != null) 
                                            {                                              
                                                if(ttID!=0)
                                                {
                                                    var hlEdit= document.getElementById('" + hlEdit.ClientID + @"');
                                                    if(hlEdit!=null)
                                                    {
                                                        hlEdit.href = document.getElementById('" + hfEdit.ClientID + @"').value + '&TableTabID=' + ttID;
                                                    }
                                                }

                                                $('.showhidedivs" + _theTable.TableID.ToString() + @"').hide();
                                                //divSelectedO.style.display = 'block';
                                                $('#'+divSelected).fadeIn();
                                                if ($('.TablLinkClass" + _theTable.TableID.ToString() + @"') != null && lnk != null) {
                                                    $('.TablLinkClass" + _theTable.TableID.ToString() + @"').css('font-weight', 'normal');
                                                }
                                                if (lnk != null) {
                                                    try{
                                                    lnk.style.fontWeight = 'bold';}
                                                    catch(err) { 
                                                        //
                                                        }
                                                }
                                            }

                                        }

                                        ";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strTableTabsJS" + _strDynamictabPart, strTableTabsJS, true);
            }
        }
    }
    protected void ShowHideTableTab()
    {
        string strHiddenTableTabID = "-1";
        if (_dtDBTableTab != null)
        {
            if (_dtDBTableTab.Rows.Count > 1)
            {
                           
                //Tab Show When

                if (_theRecord != null)
                {
                    try
                    {
                        string strHaveShowWhen = Common.GetValueFromSQL(@"SELECT TOP 1 SW.TableTabID FROM [ShowWhen] SW JOIN [TableTab] TT
                                             ON SW.TableTabID=TT.TableTabID
	                                            WHERE TT.TableID=" + _theRecord.TableID.ToString());
                        if (strHaveShowWhen != "")
                        {

                            for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                            {
                                DataTable dtTabShowWhen = RecordManager.dbg_ShowWhen_ForGrid(null, null, int.Parse(_dtDBTableTab.Rows[t]["TableTabID"].ToString()));

                                if (dtTabShowWhen.Rows.Count > 0)
                                {
                                    string strFullFormula = "";
                                    foreach (DataRow drSW in dtTabShowWhen.Rows)
                                    {
                                        if (drSW["TableTabID"] != DBNull.Value && drSW["HideColumnID"] != DBNull.Value
                                            && drSW["HideColumnValue"] != DBNull.Value && drSW["HideOperator"] != DBNull.Value)
                                        {
                                            Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(drSW["HideColumnID"].ToString()));
                                            if (theHideColumn != null)
                                            {
                                                string strEachFormula = "Value=0";

                                                if (Common.IsDataValidCommon(theHideColumn.ColumnType, RecordManager.GetRecordValue(ref _theRecord, theHideColumn.SystemName),
                                                   drSW["HideOperator"].ToString(), drSW["HideColumnValue"].ToString()))
                                                {
                                                    strEachFormula = "Value=1";

                                                }
                                                else
                                                {
                                                    strEachFormula = "Value=0";
                                                }



                                                if (strEachFormula != "")
                                                    strFullFormula = strFullFormula + " " + drSW["JoinOperator"].ToString() + " " + strEachFormula;

                                            }
                                        }
                                    }

                                    if (strFullFormula != "")
                                    {
                                        strFullFormula = strFullFormula.Trim();
                                        string strError = "";

                                        if (UploadManager.IsDataValid("1", strFullFormula, ref strError))
                                        {
                                            //
                                        }
                                        else
                                        {
                                            strHiddenTableTabID = strHiddenTableTabID + "," + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                                        }
                                    }
                                }

                            }

                        }

                    }
                    catch
                    {
                        //
                    }

                }


                string strHavePageColour = "";

                if (_theRecord != null)
                {
                    strHavePageColour = Common.GetValueFromSQL(@"SELECT TOP 1 CC.ColumnColourID FROM [ColumnColour] CC JOIN [TableTab] TT
                                             ON CC.ID=TT.TableTabID
	                                            WHERE CC.Context='tabletabid' AND TT.TableID=" + _theRecord.TableID.ToString());
                }

                for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                {
                    string strColour = "";

                    if (_theRecord != null && strHavePageColour != "")
                        strColour = Cosmetic.fnGetColumnColour((int)_theRecord.RecordID, int.Parse(_dtDBTableTab.Rows[t]["TableTabID"].ToString()), "tabletabid");


                    if (t == 0)
                    {
                        LinkButton lnkDetialTab =(LinkButton) pnlMain.FindControl("lnkDetialTab" + _theTable.TableID.ToString());                    
                        if (strColour != "")
                        {
                            lnkDetialTab.Style.Add("color", "#" + strColour);
                        }
                        else
                        {
                            lnkDetialTab.Style.Remove("color");
                        }
                    }
                    else
                    {


                        LinkButton lnkDetialTabD = (LinkButton)pnlMain.FindControl("lnkDetialTabD" + _theTable.TableID.ToString() + _dtDBTableTab.Rows[t]["TableTabID"].ToString());          
                  
                        if (strColour != "")
                        {
                            lnkDetialTabD.Style.Add("color", "#" + strColour);
                        }
                        else
                        {
                            lnkDetialTabD.Style.Remove("color");
                        }

                                             
                        if (Common.IsIn(_dtDBTableTab.Rows[t]["TableTabID"].ToString(), strHiddenTableTabID))
                        {
                            _pnlDetailTabD[t].CssClass = "showhidedivs_hide";
                            _pnlDetailTabD[t].Style.Add("display", "none");
                            lnkDetialTabD.Style.Add("display", "none");
                        }
                        else
                        {
                            //_pnlDetailTabD[t].CssClass = "TablLinkClass" + _theTable.TableID.ToString();
                            //_pnlDetailTabD[t].Style.Add("display", "inline");
                            lnkDetialTabD.Style.Add("display", "inline");
                        }
                    }
                }



                string strTableTabsJS = @" function ShowHideMainDivs" + _theTable.TableID.ToString() + @"(divSelected,lnk,ttID) {
                                        var divSelectedO = document.getElementsByName(divSelected.toString());
                                            $('.showhidedivs_hide').hide();
                                            if (divSelectedO != null) 
                                            {
                                              
                                                 if(ttID!=0)
                                                {
                                                    var hlEdit= document.getElementById('" + hlEdit.ClientID + @"');
                                                    if(hlEdit!=null)
                                                    {
                                                        hlEdit.href = document.getElementById('" + hfEdit.ClientID + @"').value + '&TableTabID=' + ttID;
                                                    }
                                                }

                                                $('.showhidedivs" + _theTable.TableID.ToString() + @"').hide();
                                                //divSelectedO.style.display = 'block';
                                                $('#'+divSelected).fadeIn();
                                                if ($('.TablLinkClass" + _theTable.TableID.ToString() + @"') != null && lnk != null) {
                                                    $('.TablLinkClass" + _theTable.TableID.ToString() + @"').css('font-weight', 'normal');
                                                }
                                                if (lnk != null) {
                                                    try{
                                                    lnk.style.fontWeight = 'bold';}
                                                    catch(err) { 
                                                        //
                                                        }
                                                }
                                            }

                                        }

                                        ";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strTableTabsJS" + _strDynamictabPart, strTableTabsJS, true);
            }
        }
    }
    protected void CreateDynamicControls()
    {
        int iTN = 0;
        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {
            bool bDisplayRight = false;
            bool bSlider = false;
            bDisplayRight = bool.Parse(_dtColumnsDetail.Rows[i]["DisplayRight"].ToString());

            _lbl[i] = new Label();
            _lbl[i].ID = "lbl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

            cell[i * 2] = new HtmlTableCell();


            if (_theTable != null)
            {
                _theAccount = SecurityManager.Account_Details((int)_theTable.AccountID);
                if (_theAccount != null)
                {
                    if (_theAccount.LabelOnTop != null)
                    {
                        if ((bool)_theAccount.LabelOnTop)
                        {
                            _bLabelOnTop = true;
                        }
                    }
                }
            }



            cell[(i * 2) + 1] = new HtmlTableCell();//
            if (_bLabelOnTop)
            {
                cell[(i * 2) + 1].Controls.Add(_lbl[i]);
                cell[(i * 2) + 1].Controls.Add(new LiteralControl("<br />"));
            }
            else
            {
                cell[i * 2].Controls.Add(_lbl[i]);

            }


            switch (_dtColumnsDetail.Rows[i]["SystemName"].ToString().ToLower())
            {

                case "datetimerecorded":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 100;
                    _txtValue[i].AutoCompleteType = AutoCompleteType.Disabled;
                    _txtValue[i].CssClass = "NormalTextBox";


                    _ibValue[i] = new ImageButton();
                    _ibValue[i].ID = "ib" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _ibValue[i].ImageUrl = "~/Images/Calendar.png";
                    _ibValue[i].Style.Add("padding-left", "3px");



                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        _lblTime[i] = new Label();
                        _lblTime[i].ID = "lblTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();


                        _txtTime[i] = new TextBox();
                        _txtTime[i].ID = "txtTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtTime[i].Width = 80;
                        _txtTime[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtTime[i].CssClass = "NormalTextBox";

                    }

                    _iDateTimeRecorded = i;

                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                    cell[(i * 2) + 1].Controls.Add(_ibValue[i]);

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        cell[(i * 2) + 1].Controls.Add(_lblTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_txtTime[i]);
                    }

                    break;

                case "recordid":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 198;
                    _txtValue[i].Enabled = false;
                    _txtValue[i].CssClass = "NormalTextBox";
                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                    break;

                case "isactive":

                    _chkIsActive = new CheckBox();
                    _chkIsActive.ID = "chkIsActive";
                    _chkIsActive.CssClass = "NormalTextBox";
                    _iIsActiveIndex = i;

                    cell[(i * 2) + 1].Controls.Add(_chkIsActive);

                    break;

                case "notes":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 270; //30*9
                    _txtValue[i].Height = 54; //18 * 3
                    _txtValue[i].TextMode = TextBoxMode.MultiLine;
                    _txtValue[i].CssClass = "MultiLineTextBox";

                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);



                    break;

                case "tableid":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 198;
                    _txtValue[i].Enabled = false;
                    _txtValue[i].CssClass = "NormalTextBox";

                    _iTableIndex = i;

                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);

                    break;


                case "enteredby":

                    _ddlEnteredBy = new DropDownList();
                    _ddlEnteredBy.ID = "ddlEnteredBy";
                    _ddlEnteredBy.CssClass = "NormalTextBox";
                    _iEnteredByIndex = i;

                    cell[(i * 2) + 1].Controls.Add(_ddlEnteredBy);

                    break;

                default:


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                    {
                        _txtValue[i] = new TextBox();
                        _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtValue[i].Width = 100;
                        _txtValue[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtValue[i].CssClass = "NormalTextBox";


                        _ibValue[i] = new ImageButton();
                        _ibValue[i].ID = "ib" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _ibValue[i].ImageUrl = "~/Images/Calendar.png";
                        _ibValue[i].Style.Add("padding-left", "3px");

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ibValue[i]);
                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                    {
                        _txtValue[i] = new TextBox();
                        _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtValue[i].Width = 100;
                        _txtValue[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtValue[i].CssClass = "NormalTextBox";

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        _txtValue[i] = new TextBox();
                        _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtValue[i].Width = 100;
                        _txtValue[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtValue[i].CssClass = "NormalTextBox";

                        _ibValue[i] = new ImageButton();
                        _ibValue[i].ID = "ib" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _ibValue[i].ImageUrl = "~/Images/Calendar.png";
                        _ibValue[i].AlternateText = "Click to show calendar";
                        _ibValue[i].Style.Add("padding-left", "3px");


                        _lblTime[i] = new Label();
                        _lblTime[i].ID = "lblTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        _txtTime[i] = new TextBox();
                        _txtTime[i].ID = "txtTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtTime[i].Width = 80;
                        _txtTime[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtTime[i].CssClass = "NormalTextBox";

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ibValue[i]);

                        cell[(i * 2) + 1].Controls.Add(_lblTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_txtTime[i]);

                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "trafficlight")
                    {
                        _imgValues[i] = new Image();
                        cell[(i * 2) + 1].Controls.Add(_imgValues[i]);
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "button")
                    {
                        _lnkValue[i] = new LinkButton();
                        _lnkValue[i].ID = "lnk" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lnkValue[i].ClientIDMode = ClientIDMode.Static;

                        cell[(i * 2) + 1].Controls.Add(_lnkValue[i]);
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                    {

                        _hfValue[i] = new HiddenField();
                        _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();


                        _lblValue[i] = new Label();
                        _lblValue[i].ID = "lblV" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        _pnlDIV[i] = new Panel();
                        _pnlDIV[i].ID = "pnl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        _fuValue[i] = new FileUpload();
                        _fuValue[i].ID = "fu" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _pnlDIV[i].Controls.Add(_fuValue[i]);


                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                        {
                            cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);
                            cell[(i * 2) + 1].Controls.Add(_lblValue[i]);
                        }
                        else
                        {
                            cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);
                            cell[(i * 2) + 1].Controls.Add(_lblValue[i]);
                        }
                        cell[(i * 2) + 1].Controls.Add(_hfValue[i]);

                    }




                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                        && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                        || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                        && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                        && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != ""
                        )
                    {
                        if (_dtColumnsDetail.Rows[i]["ParentColumnID"] == DBNull.Value)
                        {
                            //this is a table

                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                            {
                                _txtValue[i] = new TextBox();
                                _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _txtValue[i].Width = 198;
                                _txtValue[i].CssClass = "NormalTextBox";

                                _imgValues[i] = new Image();
                                _imgValues[i].ImageUrl = "~/App_Themes/Default/Images/dropdown.png";

                                _hfValue[i] = new HiddenField();
                                _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();



                                HtmlTable tblQuickLink = new HtmlTable();
                                HtmlTableCell cellQL1 = new HtmlTableCell();
                                HtmlTableCell cellQL2 = new HtmlTableCell();
                                HtmlTableRow hrQL = new HtmlTableRow();

                                tblQuickLink.CellPadding = 0;
                                tblQuickLink.CellSpacing = 0;

                                hrQL.Cells.Add(cellQL1);
                                hrQL.Cells.Add(cellQL2);

                                tblQuickLink.Rows.Add(hrQL);

                                cellQL1.Controls.Add(_txtValue[i]);
                                cellQL1.Controls.Add(_imgValues[i]);


                                cell[(i * 2) + 1].Controls.Add(tblQuickLink);
                                cell[(i * 2) + 1].Controls.Add(_hfValue[i]);

                            }

                            //end of Table Predictive

                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                            {
                                //this is drop down
                                _ddlValue[i] = new DropDownList();
                                _ddlValue[i].ID = "ddl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _ddlValue[i].Width = 198;
                                _ddlValue[i].CssClass = "NormalTextBox";


                                _pnlDIV[i] = new Panel();
                                _pnlDIV[i].Controls.Add(_ddlValue[i]);

                                HtmlTable tblQuickLink = new HtmlTable();
                                HtmlTableCell cellQL1 = new HtmlTableCell();
                                HtmlTableCell cellQL2 = new HtmlTableCell();
                                HtmlTableRow hrQL = new HtmlTableRow();

                                tblQuickLink.CellPadding = 0;
                                tblQuickLink.CellSpacing = 0;

                                hrQL.Cells.Add(cellQL1);
                                hrQL.Cells.Add(cellQL2);

                                tblQuickLink.Rows.Add(hrQL);
                                cellQL1.Controls.Add(_pnlDIV[i]);

                                cell[(i * 2) + 1].Controls.Add(tblQuickLink);


                            }


                        }
                        else
                        {
                            //filterted table

                            _ddlValue2[i] = new DropDownList();
                            _ddlValue2[i].ID = "ddl2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _ddlValue2[i].Width = 198;
                            _ddlValue2[i].CssClass = "NormalTextBox";




                            _ddlValue[i] = new DropDownList();
                            _ddlValue[i].ID = "ddl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _ddlValue[i].Width = 198;
                            _ddlValue[i].CssClass = "NormalTextBox";




                            _ccddl[i] = new CascadingDropDown();
                            _ccddl[i].ID = "ccddl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                            _ccddl[i].Category = _dtColumnsDetail.Rows[i]["ColumnID"].ToString() + "," + _dtColumnsDetail.Rows[i]["ParentColumnID"].ToString();
                            _ccddl[i].TargetControlID = _ddlValue[i].ID;
                            _ccddl[i].ParentControlID = "ddl2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                            _ccddl[i].ServicePath = "~/CascadeDropdown.asmx";
                            _ccddl[i].ServiceMethod = "GetFilteredData"; //filtered



                            _pnlDIV2[i] = new Panel();
                            _pnlDIV2[i].Controls.Add(_ddlValue2[i]);
                            cell[(i * 2) + 1].Controls.Add(_pnlDIV2[i]);


                            _pnlDIV[i] = new Panel();

                            _pnlDIV[i].Controls.Add(_ddlValue[i]);
                            cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);

                            cell[(i * 2) + 1].Controls.Add(_ccddl[i]);

                        }

                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                    {
                        _radioList[i] = new RadioButtonList();
                        _radioList[i].RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                        _radioList[i].RepeatLayout = RepeatLayout.Flow;
                        _radioList[i].ID = "radio" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();



                        if (_dtColumnsDetail.Rows[i]["VerticalList"] != DBNull.Value)
                        {
                            if (bool.Parse(_dtColumnsDetail.Rows[i]["VerticalList"].ToString()))
                            {
                                _radioList[i].RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Vertical;

                                cell[i * 2].Controls.Remove(_lbl[i]);
                                cell[(i * 2) + 1].Controls.Add(_lbl[i]);
                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("<br />"));
                            }
                        }

                        cell[(i * 2) + 1].Controls.Add(_radioList[i]);
                        if (_dtColumnsDetail.Rows[i]["VerticalList"] != DBNull.Value)
                        {
                            if (bool.Parse(_dtColumnsDetail.Rows[i]["VerticalList"].ToString()))
                            {
                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("<hr />"));
                            }
                        }

                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                    {
                        _hfValue[i] = new HiddenField();
                        _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        _hfValue2[i] = new HiddenField();
                        _hfValue2[i].ID = "hf2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        _hfValue3[i] = new HiddenField();
                        _hfValue3[i].ID = "hf3" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        cell[(i * 2) + 1].Controls.Add(_hfValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_hfValue2[i]);
                        cell[(i * 2) + 1].Controls.Add(_hfValue3[i]);

                        string strShowAddress = @"";
                        string strMapHeight = "";
                        string strMapWidth = "";
                        string strSearchAddressJS = "";
                        bool bShowMap = false;
                        if (_dtColumnsDetail.Rows[i]["TextHeight"] != DBNull.Value
                            && _dtColumnsDetail.Rows[i]["TextWidth"] != DBNull.Value)
                        {
                            strMapHeight = _dtColumnsDetail.Rows[i]["TextHeight"].ToString();
                            strMapWidth = _dtColumnsDetail.Rows[i]["TextWidth"].ToString();
                            bShowMap = true;
                        }


                        if (bShowMap)
                            cell[(i * 2) + 1].Controls.Add(new LiteralControl("<div align='left' id='map" + _strDynamictabPart + i.ToString() + "' style='width: " + strMapWidth + "px; height: " + strMapHeight + "px; margin-top: 10px;'></div>"));

                        if (_dtColumnsDetail.Rows[i]["ShowTotal"] != DBNull.Value)
                        {
                            if (_dtColumnsDetail.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
                            {
                                if (bShowMap)
                                    cell[(i * 2) + 1].Controls.Add(new LiteralControl("<br/>"));

                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("Address:&nbsp;"));
                                _txtValue2[i] = new TextBox();
                                _txtValue2[i].ID = "searchbox" + i.ToString();
                                _txtValue2[i].Width = 280;
                                _txtValue2[i].CssClass = "NormalTextBox";
                                cell[(i * 2) + 1].Controls.Add(_txtValue2[i]);

                            }
                        }

                        string strShowLatLong = @"";

                        if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value)
                        {
                            if (_dtColumnsDetail.Rows[i]["IsRound"].ToString().ToLower() == "true")
                            {
                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("<br/>"));
                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("<br/>"));
                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("Latitude:&nbsp;"));
                                _txtValue[i] = new TextBox();
                                _txtValue[i].ID = "txtLat" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _txtValue[i].Width = 145;
                                _txtValue[i].CssClass = "NormalTextBox";

                                //_txtValue[i].ClientIDMode = ClientIDMode.Static;

                                cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;&nbsp;Longitude:&nbsp;"));

                                _txtTime[i] = new TextBox();
                                _txtTime[i].ID = "txtLong" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _txtTime[i].Width = 145;
                                _txtTime[i].CssClass = "NormalTextBox";


                                cell[(i * 2) + 1].Controls.Add(_txtTime[i]);

                                if (bShowMap)
                                {
                                    strShowLatLong = @"  var txtLatitude = document.getElementById('" + _strDynamictabPart + _txtValue[i].ID.ToString() + @"');
                                            if(txtLatitude!=null){
                                            txtLatitude.value = map" + _strDynamictabPart + i.ToString() + @".getCenter().lat();}
                                            var txtLongitude = document.getElementById('" + _strDynamictabPart + _txtTime[i].ID.ToString() + @"');
                                             if(txtLongitude!=null){
                                            txtLongitude.value = map" + _strDynamictabPart + i.ToString() + @".getCenter().lng(); }";

                                }

                            }
                        }


                        if (bShowMap)
                        {

                            string strMapJS = @"//$(document).ready(function () {
                                        if(document.getElementById('" + _strDynamictabPart + _hfValue3[i].ID.ToString() + @"')!=null){
                                         var mapOptions = {
                                                zoom:parseFloat(document.getElementById('" + _strDynamictabPart + _hfValue3[i].ID.ToString() + @"').value),
                                                mapTypeId: google.maps.MapTypeId.ROADMAP,scrollwheel: false,
                                                center: new google.maps.LatLng(document.getElementById('" + _strDynamictabPart + _hfValue[i].ID.ToString() + @"').value, document.getElementById('" + _strDynamictabPart + _hfValue2[i].ID.ToString() + @"').value)          
                                            };
                                     var map" + _strDynamictabPart + i.ToString() + @" = new google.maps.Map(document.getElementById('map" + _strDynamictabPart + i.ToString() + @"'), mapOptions);
                                     var geocoder = new google.maps.Geocoder();" + strShowAddress + @"

                                     google.maps.event.addListener(map" + _strDynamictabPart + i.ToString() + @", 'center_changed', function () {
                                            document.getElementById('" + _strDynamictabPart + _hfValue[i].ID.ToString() + @"').value = map" + _strDynamictabPart + i.ToString() + @".getCenter().lat();
                                            document.getElementById('" + _strDynamictabPart + _hfValue2[i].ID.ToString() + @"').value = map" + _strDynamictabPart + i.ToString() + @".getCenter().lng();
                                                  " + strShowLatLong + @" 
                                        });

                                    google.maps.event.addListener(map" + _strDynamictabPart + i.ToString() + @", 'zoom_changed', function () {
                                                    document.getElementById('" + _strDynamictabPart + _hfValue3[i].ID.ToString() + @"').value = map" + _strDynamictabPart + i.ToString() + @".getZoom();

                                                });
                                        " + strSearchAddressJS + @" }
                                   // });

                                    ";

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AutoCompleteJS" + _strDynamictabPart + i.ToString(), strMapJS, true);
                        }


                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                    {
                        _chkValue[i] = new CheckBox();
                        _chkValue[i].ID = "chk" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        cell[(i * 2) + 1].Controls.Add(_chkValue[i]);
                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                    {

                        _htmValue[i] = new WYSIWYGEditor();
                        _htmValue[i].ID = "htm" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _htmValue[i].AssetManager = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";
                        _htmValue[i].ButtonFeatures = new string[] { "FullScreen", "XHTMLFullSource", "RemoveFormat", "Undo", "Redo", "|", "Paragraph", "FontName", "FontSize", "|", "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyFull", "Bold", "Italic", "Underline", "Hyperlink" };
                        _htmValue[i].scriptPath = "../../Editor/scripts/";
                        _htmValue[i].ToolbarMode = 0;
                        _htmValue[i].Width = 520;
                        _htmValue[i].Height = 250;

                        cell[(i * 2) + 1].Controls.Add(_htmValue[i]);

                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                    {
                        _lbl[i].Visible = false;
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "button")
                    {
                        _lbl[i].Visible = false;
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "staticcontent")
                    {
                        _lbl[i].Visible = false;
                        _lblValue[i] = new Label();
                        _lblValue[i].ID = "lblV" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        cell[(i * 2) + 1].Controls.Add(_lblValue[i]);
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                        && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "")
                    {
                        _lstValue[i] = new ListBox();
                        _lstValue[i].ID = "lst" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lstValue[i].SelectionMode = ListSelectionMode.Multiple;
                        _lstValue[i].Style.Add("min-width", "198px");
                        _lstValue[i].Style.Add("min-height", "100px");
                        _lstValue[i].Style.Add("max-width", "350px");


                        cell[(i * 2) + 1].Controls.Add(_lstValue[i]);

                    }



                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                       && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "checkbox")
                    {
                        _cblValue[i] = new CheckBoxList();
                        _cblValue[i].ID = "cbl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_cblValue[i].SelectionMode = ListSelectionMode.Multiple;
                        _cblValue[i].Style.Add("min-width", "198px");
                        _cblValue[i].Style.Add("min-height", "100px");
                        _cblValue[i].Style.Add("max-width", "350px");
                        _cblValue[i].Style.Add("display", "block");
                        _cblValue[i].Style.Add("overflow", "auto");
                        _cblValue[i].Style.Add("border", "solid 1px black");
                        _cblValue[i].CssClass = "multiple_cbl";

                        foreach (ListItem li in _cblValue[i].Items)
                        {
                            li.Attributes.Add("DataValue", li.Value);
                        }

                        cell[(i * 2) + 1].Controls.Add(_cblValue[i]);
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                        && _dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                        && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                        || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                    {
                        _ddlValue[i] = new DropDownList();
                        _ddlValue[i].Width = 198;
                        _ddlValue[i].ID = "ddl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _ddlValue[i].CssClass = "NormalTextBox";


                        _pnlDIV[i] = new Panel();

                        _pnlDIV[i].Controls.Add(_ddlValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);

                        _imgWarning[i] = new Image();
                        _imgWarning[i].ImageUrl = "~/Images/warning.png";

                        cell[(i * 2) + 1].Controls.Add(_imgWarning[i]);

                    }




                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation")
                    {
                        _txtValue[i] = new TextBox();
                        _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtValue[i].Width = 198;
                        _txtValue[i].CssClass = "NormalTextBox";

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);

                        _imgWarning[i] = new Image();
                        _imgWarning[i].ImageUrl = "~/Images/warning.png";
                        cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                        cell[(i * 2) + 1].Controls.Add(_imgWarning[i]);


                    }




                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "text"
                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                    {
                        _txtValue[i] = new TextBox();
                        _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtValue[i].Width = 198;
                        _txtValue[i].CssClass = "NormalTextBox";


                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number" &&
                            _dtColumnsDetail.Rows[i]["NumberType"] != null)
                        {
                            if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "7")
                            {
                                bSlider = true;
                            }
                        }



                        if (bSlider == false)
                        {
                            cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        }
                        else
                        {
                            _txtValue[i].AutoPostBack = false;
                            _txtValue[i].Text = "0";

                            _lblValue[i] = new Label();
                            _lblValue[i].ID = "lblV" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                            _seValue[i] = new SliderExtender();
                            _seValue[i].ID = "se" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _seValue[i].BehaviorID = _txtValue[i].ID;
                            _seValue[i].TargetControlID = _txtValue[i].ID;
                            _seValue[i].BoundControlID = _lblValue[i].ID;
                            _seValue[i].Minimum = 0;
                            _seValue[i].Maximum = 100;
                            _seValue[i].Steps = 100;


                            HtmlTable subTable = new HtmlTable();
                            HtmlTableRow subRow = new HtmlTableRow();
                            HtmlTableCell cell1 = new HtmlTableCell();
                            HtmlTableCell cell2 = new HtmlTableCell();
                            cell1.Controls.Add(_txtValue[i]);
                            cell1.Controls.Add(_seValue[i]);
                            cell2.Controls.Add(_lblValue[i]);
                            subRow.Cells.Add(cell1);
                            subRow.Cells.Add(cell2);
                            subTable.Rows.Add(subRow);
                            cell[(i * 2) + 1].Controls.Add(subTable);

                        }


                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "text")
                        {
                            if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value)
                            {
                                if (_dtColumnsDetail.Rows[i]["TextType"].ToString() == "readonly")
                                {
                                    if (_txtValue[i] != null)
                                        _txtValue[i].Enabled = false;

                                }
                                if (_dtColumnsDetail.Rows[i]["TextType"].ToString() != ""
                                    && _dtColumnsDetail.Rows[i]["TextType"].ToString() != "readonly")
                                {

                                    switch (_dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower())
                                    {

                                        case "link":
                                            _hlValue[i] = new HyperLink();
                                            _hlValue[i].ID = "hl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                            _hlValue[i].Target = "_blank";
                                            _hlValue[i].Text = "Go";
                                            cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp"));
                                            cell[(i * 2) + 1].Controls.Add(_hlValue[i]);

                                            break;


                                        default:
                                            break;
                                    }
                                }
                            }

                        }
                        //

                        //
                       


                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number" && bSlider == false)
                        {

                            if (bool.Parse(_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString()) == false
                                && _dtColumnsDetail.Rows[i]["Calculation"].ToString() == "")
                            {

                                _ftbExt[i] = new FilteredTextBoxExtender();
                                _ftbExt[i].ID = "ftb" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _ftbExt[i].TargetControlID = _txtValue[i].ClientID;
                                _ftbExt[i].FilterType = FilterTypes.Custom;
                                _ftbExt[i].FilterMode = FilterModes.ValidChars;
                                _ftbExt[i].ValidChars = "-.0123456789";

                                cell[(i * 2) + 1].Controls.Add(_ftbExt[i]);
                            }



                            if (_dtColumnsDetail.Rows[i]["NumberType"] != null)
                            {
                                //Avg
                                if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "4")
                                {

                                    _txtValue[i].Enabled = false;

                                }
                                //Record Count
                                if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "5")
                                {

                                    _txtValue[i].Visible = false;
                                    _lblValue[i] = new Label();
                                    _lblValue[i].ID = "lblV" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();


                                    cell[(i * 2) + 1].Controls.Add(_lblValue[i]);

                                }

                            }


                            _imgWarning[i] = new Image();
                            _imgWarning[i].ImageUrl = "~/Images/warning.png";
                           
                            cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                            cell[(i * 2) + 1].Controls.Add(_imgWarning[i]);
                        }
                    }
                    break;
            }


            trX[i] = new HtmlTableRow();


            trX[i].Cells.Add(cell[i * 2]);
            trX[i].Cells.Add(cell[(i * 2) + 1]);


            if (_dtColumnsDetail.Rows[i]["TableTabID"] == DBNull.Value)
            {
                if (bDisplayRight)
                {

                    tblRight.Rows.Add(trX[i]);
                }
                else
                {

                    tblLeft.Rows.Add(trX[i]);

                }
            }
            else
            {

                if (bDisplayRight)
                {

                    for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                    {
                        if (_dtDBTableTab.Rows[t]["TableTabID"].ToString() == _dtColumnsDetail.Rows[i]["TableTabID"].ToString())
                        {
                            _tblRightD[t].Rows.Add(trX[i]);
                        }
                    }


                }
                else
                {
                    for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                    {
                        if (_dtDBTableTab.Rows[t]["TableTabID"].ToString() == _dtColumnsDetail.Rows[i]["TableTabID"].ToString())
                        {
                            _tblLeftD[t].Rows.Add(trX[i]);
                        }
                    }

                }
            }

        }

    }

    protected void PopulateDynamicControls()
    {
        int iTN = 0;
        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {
            bool bSlider = false;
            if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
            {
                _lbl[i].Text = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "*";
                    _lbl[i].ForeColor = System.Drawing.Color.Red;
            }
            else if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "r")
            {
                _lbl[i].Text = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString();
                    _lbl[i].ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                _lbl[i].Text = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "";
            }

            _lbl[i].Text = _lbl[i].Text.Replace("\r\n", "<br/>");
            _lbl[i].Text = _lbl[i].Text.Replace("\n", "<br/>");

            _lbl[i].Font.Bold = true;



            if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                       || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
            {
                cell[i * 2].VAlign = "top";
            }
            if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton"
                      && _dtColumnsDetail.Rows[i]["VerticalList"] != DBNull.Value)
            {
                if (bool.Parse(_dtColumnsDetail.Rows[i]["VerticalList"].ToString()))
                {
                    cell[i * 2].VAlign = "top";
                }
            }
            if (_bLabelOnTop)
            {
                //
            }
            else
            {

                cell[i * 2].Align = "Right";
            }


            switch (_dtColumnsDetail.Rows[i]["SystemName"].ToString().ToLower())
            {

                case "datetimerecorded":

                    _ibValue[i].Visible = false;

                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        _lblTime[i].Text = "  Time ";
                        _lblTime[i].Font.Bold = true;
                        _txtTime[i].Text = "00:00";
                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtTime[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }
                    }


                    _iDateTimeRecorded = i;
                    _txtValue[i].Text = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();

                    break;

                case "recordid":

                    _txtValue[i].Text = "Assigned on Save";
                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    break;

                case "isactive":
                    _chkIsActive.Checked = true;
                    _iIsActiveIndex = i;
                    _chkIsActive.Visible = false;
                    _lbl[i].Visible = false;
                    break;

                case "notes":

                    if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                    {
                        _txtValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                    }

                    if (_dtColumnsDetail.Rows[i]["TextHeight"] != null && _dtColumnsDetail.Rows[i]["TextHeight"].ToString() != "")
                    {
                        if (int.Parse(_dtColumnsDetail.Rows[i]["TextHeight"].ToString()) > 1)
                        {
                            _txtValue[i].Height = int.Parse(_dtColumnsDetail.Rows[i]["TextHeight"].ToString()) * 18;

                        }
                    }

                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                  

                    break;

                case "tableid":

                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    _iTableIndex = i;

                    break;


                case "enteredby":

                    _ddlEnteredBy.DataValueField = "UserID";
                    _ddlEnteredBy.DataTextField = "FirstName";
                    _ddlEnteredBy.DataSource = SecurityManager.User_Select(null, "", "", "", "",
                        "", null, null, null,
                        _theTable.AccountID,
                        "FirstName", "ASC", null, null, ref iTN);

                    _ddlEnteredBy.DataBind();
                    _iEnteredByIndex = i;

                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _ddlEnteredBy.ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    break;

                default:


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                    {


                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                       
                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                    {

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }


                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {


                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        _lblTime[i].Text = "  Time ";
                        _lblTime[i].Font.Bold = true;

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtTime[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        
                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "trafficlight")
                    {
                        _imgValues[i].ToolTip = "Traffic Light";
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "button")
                    {
                        _lnkValue[i].Enabled = false;
                        bool bVisible = false;
                        if (_dtColumnsDetail.Rows[i]["ButtonInfo"] != DBNull.Value && _dtColumnsDetail.Rows[i]["ButtonInfo"].ToString() != "")
                        {
                            ColumnButtonInfo theButtonInfo = JSONField.GetTypedObject<ColumnButtonInfo>(_dtColumnsDetail.Rows[i]["ButtonInfo"].ToString());
                            if (theButtonInfo != null)
                            {
                                if (!string.IsNullOrEmpty(theButtonInfo.SPToRun))
                                {
                                    bVisible = true;
                                }

                                if (!string.IsNullOrEmpty(theButtonInfo.ImageFullPath))
                                {
                                    if (theButtonInfo.ImageFullPath.IndexOf("http") > -1)
                                    {

                                    }
                                    else
                                    {
                                        theButtonInfo.ImageFullPath = _strFilesLocation + "/UserFiles/AppFiles/" + theButtonInfo.ImageFullPath;
                                    }
                                    _lnkValue[i].Text = "<img src='" + theButtonInfo.ImageFullPath + "' />";

                                }
                                else
                                {
                                    _lnkValue[i].CssClass = "btn";
                                    _lnkValue[i].ToolTip = "Disabled button";
                                    _lnkValue[i].ForeColor = System.Drawing.Color.Gray;
                                    if (_dtColumnsDetail.Rows[i]["DisplayTextDetail"] != DBNull.Value && _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() != "")
                                    {
                                        _lnkValue[i].Text = "<strong>" + _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "</strong>";
                                    }
                                    else
                                    {
                                        //
                                        _lnkValue[i].Text = "<strong>" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + "</strong>";
                                    }
                                }
                            }

                        }
                        _lnkValue[i].Visible = bVisible;
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                    {


                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _lblValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }
                    }




                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                        && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                        || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                        && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                        && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != ""
                        )
                    {
                        if (_dtColumnsDetail.Rows[i]["ParentColumnID"] == DBNull.Value)
                        {
                            //this is a table

                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                            {

                                _txtValue[i].ToolTip = "Start typing and matching values will be shown";
                                if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                                {
                                    _txtValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                                }
                                _imgValues[i].ToolTip = "Start typing and matching values will be shown";

                            }

                            //end of Table Predictive

                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                            {

                                if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                                {
                                    _ddlValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                                }


                                if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                                {
                                    _ddlValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                                }
                                try
                                {
                                    RecordManager.PopulateTableDropDown(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), ref _ddlValue[i]);
                                }
                                catch
                                {
                                    //
                                }


                                if (_bCustomDDL)
                                {
                                    _pnlDIV[i].CssClass = "ddlDIV";
                                    _ddlValue[i].CssClass = "ddlrrp";
                                    _ddlValue[i].Width = (int)_ddlValue[i].Width.Value + 22;
                                    _pnlDIV[i].Width = (int)_ddlValue[i].Width.Value - 20;
                                }


                            }


                        }
                        else
                        {
                            //filterted table



                            if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                            {
                                _ddlValue2[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                            }

                            if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                            {
                                _ddlValue2[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                            }

                            //ParentColumnID

                            Column scParentColumnID = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ParentColumnID"].ToString()));


                            if (scParentColumnID != null)
                            {
                                DataTable dtParentData = Common.DataTableFromText("SELECT DISTINCT(" + scParentColumnID.SystemName + ") FilterValue FROM Record WHERE IsActive=1 AND TableID=" + _dtColumnsDetail.Rows[i]["TableTableID"].ToString());
                                _ddlValue2[i].DataTextField = "FilterValue";
                                _ddlValue2[i].DataValueField = "FilterValue";
                                _ddlValue2[i].DataSource = dtParentData;
                                _ddlValue2[i].DataBind();

                                ListItem liSelect = new ListItem("--Select " + scParentColumnID.DisplayName + "--", "");
                                _ddlValue2[i].Items.Insert(0, liSelect);
                            }

                            if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                            {
                                _ddlValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                            }

                            if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                            {
                                _ddlValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                            }

                            if (_bCustomDDL)
                            {
                                _pnlDIV2[i].CssClass = "ddlDIV";
                                _ddlValue2[i].CssClass = "ddlrrp";
                                _ddlValue2[i].Width = (int)_ddlValue2[i].Width.Value + 22;
                                _pnlDIV2[i].Width = (int)_ddlValue2[i].Width.Value - 20;
                            }

                            if (_bCustomDDL)
                            {
                                _pnlDIV[i].CssClass = "ddlDIV";
                                _ddlValue[i].CssClass = "ddlrrp";
                                _ddlValue[i].Width = (int)_ddlValue[i].Width.Value + 22;
                                _pnlDIV[i].Width = (int)_ddlValue[i].Width.Value - 20;
                            }

                        }

                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                    {

                        _radioList[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();

                        //_radioList[i].CssClass = "NormalTextBox";
                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutRadioList(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _radioList[i]);

                           
                        }
                        else if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text")
                        {
                            Common.PutRadioListValue_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _radioList[i]);

                           
                        }
                        else
                        {
                            Common.PutRadioListValue_Image(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _radioList[i], _strFilesLocation);
                           
                        }




                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                    {


                        if (_theAccount.MapCentreLat != null)
                        {
                            _hfValue[i].Value = _theAccount.MapCentreLat.ToString();
                        }
                        else
                        {
                            _hfValue[i].Value = "-25.80339840765346";
                        }

                        if (_theAccount.MapCentreLong != null)
                        {
                            _hfValue2[i].Value = _theAccount.MapCentreLong.ToString();
                        }
                        else
                        {
                            _hfValue2[i].Value = "135.56235835000007";
                        }

                        _hfValue3[i].Value = "3";
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                    {

                        Common.PutCheckBoxDefault(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _chkValue[i]);

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _chkValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }
                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                    {
                        if (_dtColumnsDetail.Rows[i]["DropDownValues"].ToString() != "")
                        {
                            _htmValue[i].Text = _dtColumnsDetail.Rows[i]["DropDownValues"].ToString();
                        }
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                    {
                        //_lbl[i].Visible = false;
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "button")
                    {
                        // _lbl[i].Visible = false;
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "staticcontent")
                    {
                        if (_dtColumnsDetail.Rows[i]["DropDownValues"].ToString() != "")
                        {
                            _lblValue[i].Text = Server.HtmlDecode(_dtColumnsDetail.Rows[i]["DropDownValues"].ToString());
                        }
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                        && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "")
                    {

                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutListValues(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _lstValue[i]);
                        }
                        else
                        {
                            Common.PutListValues_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _lstValue[i]);
                        }

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _lstValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                    }



                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                       && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "checkbox")
                    {

                        if (_dtColumnsDetail.Rows[i]["TextHeight"] != null && _dtColumnsDetail.Rows[i]["TextHeight"].ToString() != "")
                        {
                            if (int.Parse(_dtColumnsDetail.Rows[i]["TextHeight"].ToString()) > 1)
                            {
                                int iH = int.Parse(_dtColumnsDetail.Rows[i]["TextHeight"].ToString());

                                if (iH < 7)
                                    iH = 7;

                                _cblValue[i].Height = iH * 18;

                            }
                        }


                        if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                        {
                            _cblValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                        }

                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutCheckBoxListValues(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _cblValue[i]);
                        }
                        else
                        {
                            Common.PutCheckBoxListValues_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _cblValue[i]);
                        }

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _cblValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                        && _dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                        && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                        || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                    {


                        if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                        {
                            _ddlValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                        }

                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutDDLValues(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _ddlValue[i]);

                           
                        }
                        else
                        {
                            Common.PutDDLValue_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _ddlValue[i]);

                           
                        }


                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _ddlValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }


                        if (_bCustomDDL)
                        {
                            _pnlDIV[i].CssClass = "ddlDIV";
                            _ddlValue[i].CssClass = "ddlrrp";
                            _ddlValue[i].Width = (int)_ddlValue[i].Width.Value + 22;
                            _pnlDIV[i].Width = (int)_ddlValue[i].Width.Value - 20;
                        }
                        _imgWarning[i].Visible = false;
                    }




                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation")
                    {

                        _txtValue[i].ToolTip = "Calculated value - will be refreshed on save.";
                        _txtValue[i].Enabled = false;


                        if (_dtColumnsDetail.Rows[i]["TextType"].ToString() == "f"
                         && _dtColumnsDetail.Rows[i]["RegEx"].ToString() != "")
                        {
                            _lbl[i].Text = _lbl[i].Text + "&nbsp;" + _dtColumnsDetail.Rows[i]["RegEx"].ToString();
                        }
                        _imgWarning[i].Visible = false;
                    }




                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "text"
                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                    {

                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number" &&
                            _dtColumnsDetail.Rows[i]["NumberType"] != null)
                        {
                            if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "7")
                            {
                                bSlider = true;
                            }
                            if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "8")
                            {
                                _txtValue[i].Enabled = false;
                            }
                        }


                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                        {
                            _txtValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                        }

                        if (_dtColumnsDetail.Rows[i]["TextHeight"] != null && _dtColumnsDetail.Rows[i]["TextHeight"].ToString() != "")
                        {
                            if (int.Parse(_dtColumnsDetail.Rows[i]["TextHeight"].ToString()) > 1)
                            {
                                _txtValue[i].TextMode = TextBoxMode.MultiLine;
                                _txtValue[i].Height = int.Parse(_dtColumnsDetail.Rows[i]["TextHeight"].ToString()) * 18;

                            }
                        }


                        if (bSlider == false)
                        {
                            //cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        }
                        else
                        {

                            if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != "")
                            {
                                try
                                {
                                    SliderField theSliderField = JSONField.GetTypedObject<SliderField>(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString());
                                    if (theSliderField != null)
                                    {
                                        if (theSliderField.Min != null && theSliderField.Max != null)
                                        {
                                            _seValue[i].Minimum = (int)theSliderField.Min;
                                            _seValue[i].Maximum = (int)theSliderField.Max;
                                            _seValue[i].Steps = (int)theSliderField.Max - (int)theSliderField.Min;
                                        }
                                    }
                                }
                                catch
                                {
                                    //
                                }
                            }

                        }

                        


                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number" && bSlider == false)
                        {

                          
                           

                           
                                //make constant readonly
                                if (_dtColumnsDetail.Rows[i]["Constant"].ToString() != "")
                                {
                                  
                                    _txtValue[i].Enabled = false;
                                }





                            if (_dtColumnsDetail.Rows[i]["NumberType"] != null)
                            {
                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number"
                                     && _dtColumnsDetail.Rows[i]["NumberType"] != null)
                                {
                                    if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "6")
                                    {
                                        if (_dtColumnsDetail.Rows[i]["TextType"].ToString() != "")
                                            _lbl[i].Text = _lbl[i].Text + "&nbsp;" + _dtColumnsDetail.Rows[i]["TextType"].ToString();
                                    }
                                }
                            }

                            if (_imgWarning[i]!=null)
                                _imgWarning[i].Visible = false;
                        }
                    }
                    break;
            }

        }

    }


    //public void lnkSaveClose_Click(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        if (PerformSave())
    //        {
    //            //do nothing
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message + "  " + ex.StackTrace;
    //    }
    //}


    //protected void lnkOk_Click(object sender, EventArgs e)
    //{
    //    lblMsg.Text = "";
    //    ViewState["ok"] = "ok";

    //    lnkSaveClose_Click(null, null);

    //}










    //protected void lnkNo_Click(object sender, EventArgs e)
    //{
    //    ViewState["ok"] = "no";
    //    trMainSave.Visible = true;
    //    trConfirmation.Visible = false;
    //    lblMsg.Text = "";

    //}

    //protected void lnkSave_Click(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        if (PerformSave())
    //        {
    //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Successfully saved!');", true);
    //            lblMsg.Text = "Successfully saved!";
    //            lblMsg.ForeColor = System.Drawing.Color.Green;
    //            if (Mode.ToLower() == "add")
    //            {
    //                Response.Redirect(hlEdit.NavigateUrl, false);
    //            }

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        //
    //    }

    //}


    protected void ShowHideNextPrev()
    {
        if (OnlyOneRecord == false)
        {
            string strCount = Common.GetValueFromSQL("SELECT Count(RecordID) FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1  AND (" + TextSearch + ") ");

            if (strCount == "")
                strCount = "0";

            if (int.Parse(strCount) < 2)
            {
                lnkNext.Visible = false;
                lnkPrevious.Visible = false;
            }
            else
            {
                string strNextRecordID = Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1 AND RecordID>" + ViewState["RecordID"].ToString() + " AND (" + TextSearch + ") ORDER BY RecordID");

                if (strNextRecordID == "")
                {
                    lnkNext.Visible = false;
                }
                else
                {
                    lnkNext.Visible = true;
                }

                string strPreRecordID = Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1 AND RecordID<" + ViewState["RecordID"].ToString() + " AND (" + TextSearch + ") ORDER BY RecordID DESC");
                if (strPreRecordID == "")
                {
                    lnkPrevious.Visible = false;
                }
                else
                {
                    lnkPrevious.Visible = true;
                }
            }
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {

        if (ViewState["RecordID"].ToString() != "")
        {
            string strRecordID = GetNextRecordID(ViewState["RecordID"].ToString());// Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1 AND RecordID>" + ViewState["RecordID"].ToString() + " AND (" + TextSearch + ") ORDER BY RecordID");

            if (strRecordID != "")
            {
                ViewState["RecordID"] = strRecordID;

            }
            EmptyControl();
            PopulateRecord();
        }
    }
    protected string GetNextRecordID(string baseRecordID)
    {
        return Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1 AND RecordID>" + baseRecordID + " AND (" + TextSearch + ") ORDER BY RecordID");
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        if (ViewState["RecordID"].ToString() != "")
        {
            string strRecordID = GetPreviousRecordID(ViewState["RecordID"].ToString());// Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1 AND RecordID<" + ViewState["RecordID"].ToString() + " AND (" + TextSearch + ") ORDER BY RecordID DESC");

            if (strRecordID != "")
            {
                ViewState["RecordID"] = strRecordID;

            }

            EmptyControl();
            PopulateRecord();
        }
    }

    protected string GetPreviousRecordID(string baseRecordID)
    {
        return Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1 AND RecordID<" + baseRecordID + " AND (" + TextSearch + ") ORDER BY RecordID DESC");
    }
    protected void AutoCreateUser()
    {
        if (_theTable != null)
        {
            if (_theTable.AddUserRecord != null && _theTable.AddUserUserColumnID != null && _theTable.AddUserPasswordColumnID != null)
            {

                Record theRecord = RecordManager.ets_Record_Detail_Full(_iNewRecordID);
                if (theRecord != null)
                {
                    Column theEmailColumn = RecordManager.ets_Column_Details((int)_theTable.AddUserUserColumnID);
                    Column thePasswordColumn = RecordManager.ets_Column_Details((int)_theTable.AddUserPasswordColumnID);

                    if (theEmailColumn != null && thePasswordColumn != null)
                    {
                        string strEmail = Common.GetValueFromSQL("SELECT " + theEmailColumn.SystemName + " FROM [Record] WHERE RecordID=" + theRecord.RecordID.ToString());
                        string strPassword = Common.GetValueFromSQL("SELECT " + thePasswordColumn.SystemName + " FROM [Record] WHERE RecordID=" + theRecord.RecordID.ToString());

                        if (strEmail.Trim() == "" && strPassword == "")
                            return;

                        User newUser = new User(null, "Auto",
                       "Created", "Phone", strEmail, strPassword,
                       true, DateTime.Now, DateTime.Now);//, "", false, true

                        //newUser.IsDocSecurityAdvanced = true;

                        int iNewUserID = SecurityManager.User_Insert(newUser);

                        int iTN = 0;
                        List<Role> lstRole = SecurityManager.Role_Select(null, "", "8", "", null, null, "", "", null,
                            null, ref iTN, _iSessionAccountID, null, null);
                        Role theRole = new Role(null, "", "", "", null, null);
                        foreach (Role tempRole in lstRole)
                        {
                            theRole = tempRole;
                        }

                        UserRole newUserRole = new UserRole(null, iNewUserID, (int)theRole.RoleID, DateTime.Now, DateTime.Now);
                        newUserRole.AccountID = _iSessionAccountID;
                        newUserRole.IsPrimaryAccount = true;
                        newUserRole.IsAccountHolder = false;
                        newUserRole.IsAdvancedSecurity = true;
                        newUserRole.IsDocSecurityAdvanced = true;
                        SecurityManager.UserRole_Insert(newUserRole);


                        RoleTable newUserTable = new RoleTable(null, (int)theRecord.TableID, 8, null, null);//8 own role
                        newUserTable.RoleID = theRole.RoleID;
                        SecurityManager.dbg_RoleTable_Insert(newUserTable);


                        //lets updated owneruserif
                        theRecord.OwnerUserID = iNewUserID;
                        RecordManager.ets_Record_Update(theRecord, null);

                        if (_theTable.AddUserNotification != null)
                        {
                            if ((bool)_theTable.AddUserNotification)
                            {
                                //send email

                                Content theConent = SystemData.Content_Details_ByKey("UserAccountDetails", null);

                                DataTable theSPTable = SystemData.Run_ContentSP("ets_UserAccountDetails", iNewUserID.ToString());
                                string strBody = Common.ReplaceDataFiledByValue(theSPTable, theConent.ContentP);

                                strBody = strBody.Replace("[URL]", Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath);

                                string strTo = newUser.Email;

                                string strError = "";

                                theConent.ContentP = strBody;

                                try
                                {
                                    //Common.SendSingleEmail(strTo, theConent, ref strError);
                                    DBGurus.SendEmail(theConent.ContentKey, true, null, theConent.Heading, theConent.ContentP, "", strTo, "", "", null, null, out strError);
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
        }
    }    
}



