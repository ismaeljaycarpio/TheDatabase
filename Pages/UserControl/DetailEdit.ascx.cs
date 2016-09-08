
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

    string _strClickFirstTab = "";
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
    Record _qsRecord = null;
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
    RegularExpressionValidator[] _revValue;
    RequiredFieldValidator[] _rfvValue;
    CompareValidator[] _cvValue;
    CustomValidator[] _cusvValue;
    FilteredTextBoxExtender[] _ftbExt;
    SliderExtender[] _seValue;

    FileUpload[] _fuValue;
    FileUpload[] _fuValue2;
    Panel[] _pnlDIV;
    Panel[] _pnlDIV2;
    Label[] _lblValue;
    string _strJS = "";
    string _strJSPostBack = "";
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
    AjaxControlToolkit.CalendarExtender[] _ceDateTimeRecorded;

    //AjaxControlToolkit.MaskedEditExtender[] _meeDate;
    //AjaxControlToolkit.MaskedEditValidator[] _mevDate;
    TextBoxWatermarkExtender[] _twmValue;
    RangeValidator[] _rvDate;

    AjaxControlToolkit.MaskedEditExtender[] _meeTime;
    CustomValidator[] _cvTime;

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
        //if (RecordID != null)
        //{
        //    _qsRecord = RecordManager.ets_Record_Detail_Full((int)RecordID);
        //}

        if (Request.QueryString["mode"] != null)
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]).ToString().ToLower();
        }
        if (Request.QueryString["Recordid"] != null)
        {
            _iParentRecordID = int.Parse(Cryptography.Decrypt(Request.QueryString["Recordid"].ToString()));
            _ParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);
        }

        if (this.Page.MasterPageFile!=null && this.Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        {
            _bCustomDDL = true;
        }

        if (ContentPage == "record")
        {
            if (Request.RawUrl.IndexOf("Pages/Mobile") > -1)
            {
                _strRecordFolder = "Mobile";
            }
        }

        if (!IsPostBack)
        {
            if (ContentPage == "record")
            {
                lnkPrevious.Visible = true;
                hlAdd.Visible = true;
                hlEdit.Visible = true;
                lnkNext.Visible = true;

            }

        }

        _strDynamictabPart = lnkSaveClose.ClientID.Substring(0, lnkSaveClose.ClientID.Length - 12);

        if (ContentPage == "record")
        lnkSaveClose.ValidationGroup = _strDynamictabPart;

        //_qsTableID = Cryptography.Decrypt(TableID);

        if (_bPrivate)
        {
            _objUser = (User)Session["User"];
        }
        else
        {
            _objUser = SecurityManager.User_Details(int.Parse(SystemData.SystemOption_ValueByKey_Account("AnonymousUser",null,TableID)));
        }

        UserRole theUserRole = (UserRole)Session["UserRole"];

        if ((bool)theUserRole.IsAdvancedSecurity)
        {
            //DataTable dtUserTable = SecurityManager.ets_UserTable_Select(null,
            //    TableID, _objUser.UserID, null);

            DataTable dtUserTable = null;

            //if (_objUser.RoleGroupID == null)
            //{
                dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
               TableID, theUserRole.RoleID, null);
            //}
            //else
            //{

            //    dtUserTable = SecurityManager.dbg_RoleGroupTable_Select((int)_objUser.RoleGroupID, null,
            //  TableID, null);
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



        if (_strRecordRightID == Common.UserRoleType.None) //none role -- 
        {
            //Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Default.aspx", false);
            divDynamic.Visible = false;
            return;
        }



        _theTable = RecordManager.ets_Table_Details(TableID);



        //put speed test here   




        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Child- Init - START ";
            theSpeedLog.FunctionLineNumber = 270;
            SecurityManager.AddSpeedLog(theSpeedLog);
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
            //_strSessionRoleType = Session["roletype"].ToString();
            _iSessionAccountID = int.Parse(Session["AccountID"].ToString());

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




        //Menu theMenu = RecordManager.ets_Menu_Details((int)_theTable.MenuID);


        _lbl = new Label[_dtColumnsDetail.Rows.Count];
        _txtValue = new TextBox[_dtColumnsDetail.Rows.Count];
        _txtValue2 = new TextBox[_dtColumnsDetail.Rows.Count];
        _ibValue = new ImageButton[_dtColumnsDetail.Rows.Count];
        _lnkValue = new LinkButton[_dtColumnsDetail.Rows.Count];
        _hlValue = new HyperLink[_dtColumnsDetail.Rows.Count];
        //_heeValue = new HtmlEditorExtender[_dtRecordTypleColumlns.Rows.Count];
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

        //_hlSensorInfo = new HyperLink[_dtRecordTypleColumlns.Rows.Count];
        _imgWarning = new Image[_dtColumnsDetail.Rows.Count];
        _imgValues = new Image[_dtColumnsDetail.Rows.Count];
        _revValue = new RegularExpressionValidator[_dtColumnsDetail.Rows.Count];
        _rfvValue = new RequiredFieldValidator[_dtColumnsDetail.Rows.Count];
        _cvValue = new CompareValidator[_dtColumnsDetail.Rows.Count];
        _cusvValue = new CustomValidator[_dtColumnsDetail.Rows.Count];
        _ftbExt = new FilteredTextBoxExtender[_dtColumnsDetail.Rows.Count];
        _fuValue = new FileUpload[_dtColumnsDetail.Rows.Count];
        _fuValue2 = new FileUpload[_dtColumnsDetail.Rows.Count];
        _pnlDIV = new Panel[_dtColumnsDetail.Rows.Count];
        _pnlDIV2 = new Panel[_dtColumnsDetail.Rows.Count];
        _lblValue = new Label[_dtColumnsDetail.Rows.Count];

        _seValue = new SliderExtender[_dtColumnsDetail.Rows.Count];

        _ceDateTimeRecorded = new AjaxControlToolkit.CalendarExtender[_dtColumnsDetail.Rows.Count];
        //_meeDate = new AjaxControlToolkit.MaskedEditExtender[_dtRecordTypleColumlns.Rows.Count];
        //_mevDate = new AjaxControlToolkit.MaskedEditValidator[_dtRecordTypleColumlns.Rows.Count];
        _twmValue = new AjaxControlToolkit.TextBoxWatermarkExtender[_dtColumnsDetail.Rows.Count];

        _rvDate = new RangeValidator[_dtColumnsDetail.Rows.Count];
        _meeTime = new AjaxControlToolkit.MaskedEditExtender[_dtColumnsDetail.Rows.Count];
        _cvTime = new CustomValidator[_dtColumnsDetail.Rows.Count];
        _lblTime = new Label[_dtColumnsDetail.Rows.Count];
        _txtTime = new TextBox[_dtColumnsDetail.Rows.Count];

        trX = new HtmlTableRow[_dtColumnsDetail.Rows.Count + 4];
        cell = new HtmlTableCell[(_dtColumnsDetail.Rows.Count + 4) * 2];
        ////HtmlTableCell[] cellB = new HtmlTableCell[(_dtRecordTypleColumlns.Rows.Count + 4) * 2];


        //HtmlTableRow[] trXB = new HtmlTableRow[_dtRecordTypleColumlns.Rows.Count + 4];


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




        _dtDBTableTab = Common.DataTableFromText("SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + " ORDER BY DisplayOrder");
        string strHiddenTableTabID = "-1";
        if (_dtDBTableTab != null)
        {
            if (_dtDBTableTab.Rows.Count > 1)
            {
                pnlDetailTab.CssClass = "showhidedivs" + _theTable.TableID.ToString();
              
                _bTableTabYes = true;



                //Tab Show When
                _strChildSession="child" + _iParentRecordID.ToString() + "_" + _theTable.TableID.ToString();

                if(!IsPostBack)
                {
                    Session[_strChildSession] = null;
                }

                Record childRecord = null;
                if (Session[_strChildSession]!=null)
                {
                    string strRecordID = Session[_strChildSession].ToString();
                    if (Request.Params.Get("__EVENTTARGET")!=null)
                    {
                        if (Request.Params.Get("__EVENTTARGET").ToString().Replace("$", "_")==lnkNext.ClientID)
                        {
                            strRecordID = GetNextRecordID(strRecordID);
                        }
                        if (Request.Params.Get("__EVENTTARGET").ToString().Replace("$", "_") == lnkPrevious.ClientID)
                        {
                            strRecordID = GetPreviousRecordID(strRecordID);
                        }
                    }

                    childRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strRecordID));
                }
                if (childRecord == null && strFirstRecord!="")
                {
                    childRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strFirstRecord));
                }

                if (childRecord != null)
                {
                    try
                    {
                        string strHaveShowWhen = Common.GetValueFromSQL(@"SELECT TOP 1 SW.TableTabID FROM [ShowWhen] SW JOIN [TableTab] TT
                                             ON SW.TableTabID=TT.TableTabID
	                                            WHERE TT.TableID=" + childRecord.TableID.ToString());
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

                                                if (Common.IsDataValidCommon(theHideColumn.ColumnType, RecordManager.GetRecordValue(ref childRecord, theHideColumn.SystemName),
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


                            //if (strHiddenTableTabID != "-1")
                            //{
                            //    _dtDBTableTab = Common.DataTableFromText("SELECT * FROM TableTab WHERE TableID=" +
                            //        _theTable.TableID.ToString() + " AND TableTabID NOT IN (" + strHiddenTableTabID + ")  ORDER BY DisplayOrder");
                            //}


                        }

                    }
                    catch
                    {
                        //
                    }

                }



                _pnlDetailTabD = new Panel[_dtDBTableTab.Rows.Count];
                _tblMainD = new HtmlTable[_dtDBTableTab.Rows.Count];
                _tblLeftD = new HtmlTable[_dtDBTableTab.Rows.Count];
                _tblRightD = new HtmlTable[_dtDBTableTab.Rows.Count];


                //lets create Tabs

                string strHavePageColour = "";

                if (childRecord != null)
                {
                    strHavePageColour = Common.GetValueFromSQL(@"SELECT TOP 1 CC.ColumnColourID FROM [ColumnColour] CC JOIN [TableTab] TT
                                             ON CC.ID=TT.TableTabID
	                                            WHERE CC.Context='tabletabid' AND TT.TableID=" + childRecord.TableID.ToString());
                }
               
                for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                {
                    string strColour = "";

                    if (childRecord != null && strHavePageColour != "")
                        strColour = Cosmetic.fnGetColumnColour((int)childRecord.RecordID, int.Parse(_dtDBTableTab.Rows[t]["TableTabID"].ToString()), "tabletabid");


                    if (t == 0)
                    {
                        LinkButton lnkDetialTab = new LinkButton();
                        lnkDetialTab.ID = "lnkDetialTab" + _theTable.TableID.ToString();
                        //lnkDetialTab.ClientIDMode = ClientIDMode.Static;
                        lnkDetialTab.Text = _dtDBTableTab.Rows[t]["TabName"].ToString(); //"Detail";
                        lnkDetialTab.Font.Bold = true;
                        _strClickFirstTab = "ShowHideMainDivs" + _theTable.TableID.ToString() + "('" + pnlDetailTab.ClientID + "',this," + _dtDBTableTab.Rows[t]["TableTabID"].ToString() + ");";
                        lnkDetialTab.Attributes.Add("onclick", "ShowHideMainDivs" + _theTable.TableID.ToString() + "('" + pnlDetailTab.ClientID + "',this," + _dtDBTableTab.Rows[t]["TableTabID"].ToString() + "); return false");
                        lnkDetialTab.CssClass = "TablLinkClass" + _theTable.TableID.ToString();

                        if (strColour != "")
                        {
                            lnkDetialTab.Style.Add("color", "#" + strColour);
                        }
                        
                        
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
                        //lnkDetialTabD.OnClientClick = "ShowHideMainDivs" + _theTable.TableID.ToString() + "(" + "ctl00_HomeContentPlaceHolder_tabDetail_ctl" + t.ToString("00") + "_ctDetail" + t.ToString() + _pnlDetailTabD[t].ClientID + ",this); return false";

                        lnkDetialTabD.OnClientClick = "ShowHideMainDivs" + _theTable.TableID.ToString() + "('" + _pnlDetailTabD[t].ClientID + "',this," + _dtDBTableTab.Rows[t]["TableTabID"].ToString() + "); return false";

                        lnkDetialTabD.CssClass = "TablLinkClass" + _theTable.TableID.ToString();

                        if (strColour != "")
                        {
                            lnkDetialTabD.Style.Add("color", "#" + strColour );
                        }


                        lnkDetialTabD.CausesValidation = false;
                        if (Common.IsIn(_dtDBTableTab.Rows[t]["TableTabID"].ToString(), strHiddenTableTabID))
                        {
                            _pnlDetailTabD[t].CssClass = "showhidedivs_hide";

                            _pnlDetailTabD[t].Style.Add("display", "none");
                            lnkDetialTabD.Style.Add("display", "none");
                        }

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
                                                if ($('.TablLinkClass" + _theTable.TableID.ToString()+@"') != null && lnk != null) {
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

        int iTN = 0;

        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {
            bool bDisplayRight = false;
            bool bSlider = false;
            bDisplayRight = bool.Parse(_dtColumnsDetail.Rows[i]["DisplayRight"].ToString());

            _lbl[i] = new Label();
            _lbl[i].ID = "lbl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
            if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
            {
                _lbl[i].Text = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "*";
                if(Mode=="add")
                _lbl[i].ForeColor = System.Drawing.Color.Red;
            }
            else if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "r")
            {
                _lbl[i].Text = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() ;
                if (Mode == "add")
                    _lbl[i].ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                _lbl[i].Text = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "";
            }

            _lbl[i].Text = _lbl[i].Text.Replace("\r\n", "<br/>");
            _lbl[i].Text = _lbl[i].Text.Replace("\n", "<br/>");

            _lbl[i].Font.Bold = true;

            cell[i * 2] = new HtmlTableCell();
            //cellB[i * 2] = new HtmlTableCell();



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
                cell[i * 2].Align = "Right";
            }


            switch (_dtColumnsDetail.Rows[i]["SystemName"].ToString().ToLower())
            {

                case "datetimerecorded":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 100;
                    _txtValue[i].AutoCompleteType = AutoCompleteType.Disabled;
                    _txtValue[i].CssClass = "NormalTextBox";
                    if (ContentPage == "record")
                    _txtValue[i].ValidationGroup = _strDynamictabPart;

                    _ibValue[i] = new ImageButton();
                    _ibValue[i].ID = "ib" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _ibValue[i].ImageUrl = "~/Images/Calendar.png";
                    _ibValue[i].Style.Add("padding-left", "3px");
                    _ibValue[i].CausesValidation = false;
                    _ibValue[i].Visible = false;

                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    _ceDateTimeRecorded[i] = new AjaxControlToolkit.CalendarExtender();
                    _ceDateTimeRecorded[i].ID = "ceDateTimeRecorded";
                    _ceDateTimeRecorded[i].TargetControlID = _txtValue[i].ID;
                    _ceDateTimeRecorded[i].Format = "dd/MM/yyyy";
                    _ceDateTimeRecorded[i].PopupButtonID = _ibValue[i].ID;
                    _ceDateTimeRecorded[i].FirstDayOfWeek = FirstDayOfWeek.Monday;
                    

                    _twmValue[i] = new TextBoxWatermarkExtender();
                    _twmValue[i].ID = "twm" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _twmValue[i].TargetControlID = _txtValue[i].ID;
                    _twmValue[i].WatermarkText = "dd/mm/yyyy";
                    _twmValue[i].WatermarkCssClass = "MaskText";
                  
                    //_meeDate[i] = new AjaxControlToolkit.MaskedEditExtender();
                    //_meeDate[i].ID = "meeDate" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                    //_meeDate[i].TargetControlID = _txtValue[i].ClientID;
                    //_meeDate[i].MessageValidatorTip = true;
                    //_meeDate[i].Mask = "99/99/9999";
                    //_meeDate[i].MaskType = MaskedEditType.Date;
                    //_meeDate[i].CultureName = "en-GB";
                    //_meeDate[i].AcceptNegative = MaskedEditShowSymbol.None;
                    //_meeDate[i].DisplayMoney = MaskedEditShowSymbol.None;
                    //_meeDate[i].ErrorTooltipEnabled = true;
                    //_meeDate[i].OnFocusCssClass = "MaskedEditFocus";
                    //_meeDate[i].OnInvalidCssClass = "MaskedEditFocus";



                    //_mevDate[i] = new AjaxControlToolkit.MaskedEditValidator();
                    //_mevDate[i].ID = "mevDate" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                    //_mevDate[i].ControlExtender = _meeDate[i].ID;
                    //_mevDate[i].ControlToValidate = _txtValue[i].ID;
                    //_mevDate[i].InvalidValueMessage = _dtRecordTypleColumlns.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid.";
                    //_mevDate[i].Display = ValidatorDisplay.None;
                    //_mevDate[i].InvalidValueBlurredMessage = "*";
                    //_mevDate[i].IsValidEmpty = true;

                    _rvDate[i] = new RangeValidator();
                    _rvDate[i].ID = "rvDate" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _rvDate[i].Display = ValidatorDisplay.None;
                    _rvDate[i].ControlToValidate = _txtValue[i].ID;
                    _rvDate[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid.";
                    _rvDate[i].Type = ValidationDataType.Date;
                    _rvDate[i].Font.Bold = true;
                    _rvDate[i].MinimumValue = "1/1/1753";
                    _rvDate[i].MaximumValue = "1/1/3000";
                    if (ContentPage == "record")
                    _rvDate[i].ValidationGroup = _strDynamictabPart;

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        _lblTime[i] = new Label();
                        _lblTime[i].ID = "lblTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lblTime[i].Text = "  Time ";
                        _lblTime[i].Font.Bold = true;

                        _txtTime[i] = new TextBox();
                        _txtTime[i].ID = "txtTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtTime[i].Width = 80;
                        _txtTime[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtTime[i].CssClass = "NormalTextBox";
                        _txtTime[i].Text = "00:00";
                        if (ContentPage == "record")
                        _txtTime[i].ValidationGroup = _strDynamictabPart;

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtTime[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        _meeTime[i] = new AjaxControlToolkit.MaskedEditExtender();
                        _meeTime[i].ID = "meeTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _meeTime[i].TargetControlID = _txtTime[i].ClientID; //"ctl00_HomeContentPlaceHolder_txtTime";
                        _meeTime[i].AutoCompleteValue = "00:00";
                        _meeTime[i].Mask = "99:99";
                        _meeTime[i].MaskType = MaskedEditType.Time;
                       


                        _cvTime[i] = new CustomValidator();
                        _cvTime[i].ID = "cvTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _cvTime[i].ControlToValidate = _txtTime[i].ClientID;  //"ctl00_HomeContentPlaceHolder_txtTime";
                        _cvTime[i].ClientValidationFunction = "CheckMyText";
                        _cvTime[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- hh:mm format (24 hrs) please!";
                        _cvTime[i].Display = ValidatorDisplay.None;
                        if (ContentPage == "record")
                        _cvTime[i].ValidationGroup = _strDynamictabPart;

                        _txtTime[i].Text = "00:00";
                    }


                    _iDateTimeRecorded = i;

                    _txtValue[i].Text = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();







                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();
                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                    cell[(i * 2) + 1].Controls.Add(_ibValue[i]);
                    cell[(i * 2) + 1].Controls.Add(_ceDateTimeRecorded[i]);

                    //cell[(i * 2) + 1].Controls.Add(_meeDate[i]);
                    //cell[(i * 2) + 1].Controls.Add(_mevDate[i]);
                    cell[(i * 2) + 1].Controls.Add(_rvDate[i]);
                    cell[(i * 2) + 1].Controls.Add(_twmValue[i]);

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        cell[(i * 2) + 1].Controls.Add(_lblTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_txtTime[i]);
                    }



                    if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                    {

                        _rfvValue[i] = new RequiredFieldValidator();
                        _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _rfvValue[i].Display = ValidatorDisplay.None;//
                        _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                        _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                        if (ContentPage == "record")
                        _rfvValue[i].ValidationGroup = _strDynamictabPart;

                        cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        cell[(i * 2) + 1].Controls.Add(_meeTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_cvTime[i]);
                    }

                    break;

                case "recordid":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 198;
                    _txtValue[i].Enabled = false;
                    _txtValue[i].CssClass = "NormalTextBox";
                    _txtValue[i].Text = "Assigned on Save";
                    if (ContentPage == "record")
                    _txtValue[i].ValidationGroup = _strDynamictabPart;

                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();
                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                    break;

                case "isactive":

                    _chkIsActive = new CheckBox();
                    _chkIsActive.ID = "chkIsActive";
                    _chkIsActive.CssClass = "NormalTextBox";
                    _chkIsActive.Checked = true;
                    _iIsActiveIndex = i;
                    _chkIsActive.Visible = false;
                    _lbl[i].Visible = false;

                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();

                    cell[(i * 2) + 1].Controls.Add(_chkIsActive);

                    break;

                case "notes":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 270; //30*9
                    _txtValue[i].Height = 54; //18 * 3
                    _txtValue[i].TextMode = TextBoxMode.MultiLine;
                    if (ContentPage == "record")
                    _txtValue[i].ValidationGroup = _strDynamictabPart;
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

                    _txtValue[i].CssClass = "MultiLineTextBox";

                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    if (Mode != null)
                    {
                        if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                        {

                            if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                            {
                                _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                            }

                        }
                    }

                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();
                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);


                    if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                    {

                        _rfvValue[i] = new RequiredFieldValidator();
                        _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _rfvValue[i].Display = ValidatorDisplay.None;
                        _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                        _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                        if (ContentPage == "record")
                        _rfvValue[i].ValidationGroup = _strDynamictabPart;

                        cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                    }


                    break;

                case "tableid":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 198;
                    _txtValue[i].Enabled = false;
                    _txtValue[i].CssClass = "NormalTextBox";
                    if (ContentPage == "record")
                    _txtValue[i].ValidationGroup = _strDynamictabPart;
                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    _iTableIndex = i;

                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();

                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);

                    break;


                case "enteredby":

                    _ddlEnteredBy = new DropDownList();
                    _ddlEnteredBy.ID = "ddlEnteredBy";
                    _ddlEnteredBy.DataValueField = "UserID";
                    _ddlEnteredBy.DataTextField = "FirstName";
                    _ddlEnteredBy.CssClass = "NormalTextBox";
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

                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();
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
                        if (ContentPage == "record")
                        _txtValue[i].ValidationGroup = _strDynamictabPart;

                        _ibValue[i] = new ImageButton();
                        _ibValue[i].ID = "ib" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _ibValue[i].ImageUrl = "~/Images/Calendar.png";
                        _ibValue[i].Style.Add("padding-left", "3px");
                        _ibValue[i].CausesValidation = false;


                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        _ceDateTimeRecorded[i] = new AjaxControlToolkit.CalendarExtender();
                        _ceDateTimeRecorded[i].ID = "ceDateTimeRecorded" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _ceDateTimeRecorded[i].TargetControlID = _txtValue[i].ID;
                        _ceDateTimeRecorded[i].Format = "dd/MM/yyyy";
                        _ceDateTimeRecorded[i].PopupButtonID = _ibValue[i].ID;
                        _ceDateTimeRecorded[i].FirstDayOfWeek = FirstDayOfWeek.Monday;

                        _twmValue[i] = new TextBoxWatermarkExtender();
                        _twmValue[i].ID = "twm" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _twmValue[i].TargetControlID = _txtValue[i].ID;
                        _twmValue[i].WatermarkText = "dd/mm/yyyy";
                        _twmValue[i].WatermarkCssClass = "MaskText";

                        //_meeDate[i] = new AjaxControlToolkit.MaskedEditExtender();
                        //_meeDate[i].ID = "meeDate" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                        //_meeDate[i].TargetControlID = _txtValue[i].ClientID;
                        //_meeDate[i].MessageValidatorTip = true;
                        //_meeDate[i].Mask = "99/99/9999";
                        //_meeDate[i].MaskType = MaskedEditType.Date;
                        //_meeDate[i].CultureName = "en-GB";
                        //_meeDate[i].AcceptNegative = MaskedEditShowSymbol.None;
                        //_meeDate[i].DisplayMoney = MaskedEditShowSymbol.None;
                        //_meeDate[i].ErrorTooltipEnabled = true;
                        //_meeDate[i].OnFocusCssClass = "MaskedEditFocus";
                        //_meeDate[i].OnInvalidCssClass = "MaskedEditFocus";



                        //_mevDate[i] = new AjaxControlToolkit.MaskedEditValidator();
                        //_mevDate[i].ID = "mevDate" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                        //_mevDate[i].ControlExtender = _meeDate[i].ID;
                        //_mevDate[i].ControlToValidate = _txtValue[i].ID;
                        //_mevDate[i].InvalidValueMessage = _dtRecordTypleColumlns.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid";
                        //_mevDate[i].Display = ValidatorDisplay.None;
                        //_mevDate[i].InvalidValueBlurredMessage = "*";
                        //_mevDate[i].IsValidEmpty = true;

                        _rvDate[i] = new RangeValidator();
                        _rvDate[i].ID = "rvDate" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _rvDate[i].ControlToValidate = _txtValue[i].ID;
                        _rvDate[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid.";
                        _rvDate[i].Type = ValidationDataType.Date;
                        _rvDate[i].Font.Bold = true;
                        _rvDate[i].MinimumValue = "1/1/1753";
                        _rvDate[i].MaximumValue = "1/1/3000";
                        _rvDate[i].Display = ValidatorDisplay.None;
                        if (ContentPage == "record")
                        _rvDate[i].ValidationGroup = _strDynamictabPart;


                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        //cellB[(i * 2) + 1] = new HtmlTableCell();

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ibValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ceDateTimeRecorded[i]);

                        //cell[(i * 2) + 1].Controls.Add(_meeDate[i]);
                        //cell[(i * 2) + 1].Controls.Add(_mevDate[i]);
                        cell[(i * 2) + 1].Controls.Add(_rvDate[i]);
                        cell[(i * 2) + 1].Controls.Add(_twmValue[i]);

                        if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                        {
                            //_txtValue[i].Text = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();
                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";

                            if (ContentPage == "record")
                            _rfvValue[i].ValidationGroup = _strDynamictabPart;

                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }

                        if (Mode != null)
                        {
                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {

                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    //_txtValue[i].Text = _dtRecordTypleColumlns.Rows[i]["DefaultValue"].ToString();
                                    _txtValue[i].Text = DateTime.Today.Date.ToShortDateString();
                                }

                            }
                        }



                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                    {
                        _txtValue[i] = new TextBox();
                        _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtValue[i].Width = 100;
                        _txtValue[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtValue[i].CssClass = "NormalTextBox";
                        if (ContentPage == "record")
                        _txtValue[i].ValidationGroup = _strDynamictabPart;

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }


                        _meeTime[i] = new AjaxControlToolkit.MaskedEditExtender();
                        _meeTime[i].ID = "meeTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _meeTime[i].TargetControlID = _txtValue[i].ClientID; //"ctl00_HomeContentPlaceHolder_txtTime";
                        _meeTime[i].AutoCompleteValue = "00:00";
                        _meeTime[i].Mask = "99:99";
                        _meeTime[i].MaskType = MaskedEditType.Time;

                        _cvTime[i] = new CustomValidator();
                        _cvTime[i].ID = "cvTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _cvTime[i].ControlToValidate = _txtValue[i].ClientID;  //"ctl00_HomeContentPlaceHolder_txtTime";
                        _cvTime[i].ClientValidationFunction = "CheckMyText";
                        _cvTime[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- hh:mm:ss format (24 hrs) please!";
                        _cvTime[i].Display = ValidatorDisplay.None;
                        if (ContentPage == "record")
                        _cvTime[i].ValidationGroup = _strDynamictabPart;

                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        //cellB[(i * 2) + 1] = new HtmlTableCell();

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);



                        if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                        {
                            //_txtValue[i].Text = "00:00:00";
                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                            if (ContentPage == "record")
                            _rfvValue[i].ValidationGroup = _strDynamictabPart;

                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }

                        cell[(i * 2) + 1].Controls.Add(_meeTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_cvTime[i]);

                        if (Mode != null)
                        {
                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {

                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    //_txtValue[i].Text = _dtRecordTypleColumlns.Rows[i]["DefaultValue"].ToString();
                                    //_txtValue[i].Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                                    _txtValue[i].Text = DateTime.Now.ToString("HH:m");//HH:mm:ss
                                }

                            }
                        }
                    }




                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        _txtValue[i] = new TextBox();
                        _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtValue[i].Width = 100;
                        _txtValue[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtValue[i].CssClass = "NormalTextBox";
                        if (ContentPage == "record")
                        _txtValue[i].ValidationGroup = _strDynamictabPart;


                        _ibValue[i] = new ImageButton();
                        _ibValue[i].ID = "ib" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _ibValue[i].ImageUrl = "~/Images/Calendar.png";
                        _ibValue[i].AlternateText = "Click to show calendar";
                        _ibValue[i].Style.Add("padding-left", "3px");
                        _ibValue[i].CausesValidation = false;

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }


                        _ceDateTimeRecorded[i] = new AjaxControlToolkit.CalendarExtender();
                        _ceDateTimeRecorded[i].ID = "ceDateTimeRecorded" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _ceDateTimeRecorded[i].TargetControlID = _txtValue[i].ID;
                        _ceDateTimeRecorded[i].Format = "dd/MM/yyyy";
                        _ceDateTimeRecorded[i].PopupButtonID = _ibValue[i].ID;
                        _ceDateTimeRecorded[i].FirstDayOfWeek = FirstDayOfWeek.Monday;

                        _twmValue[i] = new TextBoxWatermarkExtender();
                        _twmValue[i].ID = "twm" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _twmValue[i].TargetControlID = _txtValue[i].ID;
                        _twmValue[i].WatermarkText = "dd/mm/yyyy";
                        _twmValue[i].WatermarkCssClass = "MaskText";

                        //_meeDate[i] = new AjaxControlToolkit.MaskedEditExtender();
                        //_meeDate[i].ID = "meeDate" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                        //_meeDate[i].TargetControlID = _txtValue[i].ClientID;
                        //_meeDate[i].MessageValidatorTip = true;
                        //_meeDate[i].Mask = "99/99/9999";
                        //_meeDate[i].MaskType = MaskedEditType.Date;
                        //_meeDate[i].CultureName = "en-GB";
                        //_meeDate[i].AcceptNegative = MaskedEditShowSymbol.None;
                        //_meeDate[i].DisplayMoney = MaskedEditShowSymbol.None;
                        //_meeDate[i].ErrorTooltipEnabled = true;
                        //_meeDate[i].OnFocusCssClass = "MaskedEditFocus";
                        //_meeDate[i].OnInvalidCssClass = "MaskedEditFocus";



                        //_mevDate[i] = new AjaxControlToolkit.MaskedEditValidator();
                        //_mevDate[i].ID = "mevDate" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                        //_mevDate[i].Display = ValidatorDisplay.None;
                        //_mevDate[i].ControlExtender = _meeDate[i].ID;
                        //_mevDate[i].ControlToValidate = _txtValue[i].ID;
                        //_mevDate[i].InvalidValueMessage = _dtRecordTypleColumlns.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid";

                        //_mevDate[i].InvalidValueBlurredMessage = "*";
                        //_mevDate[i].IsValidEmpty = true;

                        _rvDate[i] = new RangeValidator();
                        _rvDate[i].ID = "rvDate" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _rvDate[i].Display = ValidatorDisplay.None;
                        _rvDate[i].ControlToValidate = _txtValue[i].ID;
                        _rvDate[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid";
                        _rvDate[i].Type = ValidationDataType.Date;
                        //_rvDate[i].Font.Bold = true;
                        _rvDate[i].MinimumValue = "1/1/1753";
                        _rvDate[i].MaximumValue = "1/1/3000";
                        _rvDate[i].Display = ValidatorDisplay.None;
                        if (ContentPage == "record")
                        _rvDate[i].ValidationGroup = _strDynamictabPart;

                        _lblTime[i] = new Label();
                        _lblTime[i].ID = "lblTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lblTime[i].Text = "  Time ";
                        _lblTime[i].Font.Bold = true;

                        _txtTime[i] = new TextBox();
                        _txtTime[i].ID = "txtTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtTime[i].Width = 80;
                        _txtTime[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtTime[i].CssClass = "NormalTextBox";
                        if (ContentPage == "record")
                        _txtTime[i].ValidationGroup = _strDynamictabPart;

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtTime[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }


                        _meeTime[i] = new AjaxControlToolkit.MaskedEditExtender();
                        _meeTime[i].ID = "meeTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _meeTime[i].TargetControlID = _txtTime[i].ClientID; //"ctl00_HomeContentPlaceHolder_txtTime";
                        _meeTime[i].AutoCompleteValue = "00:00";
                        _meeTime[i].Mask = "99:99";
                        _meeTime[i].MaskType = MaskedEditType.Time;

                        _cvTime[i] = new CustomValidator();
                        _cvTime[i].ID = "cvTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _cvTime[i].ControlToValidate = _txtTime[i].ClientID;  //"ctl00_HomeContentPlaceHolder_txtTime";
                        _cvTime[i].ClientValidationFunction = "CheckMyText";
                        _cvTime[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- hh:mm format (24 hrs) please!";
                        _cvTime[i].Display = ValidatorDisplay.None;
                        if (ContentPage == "record")
                        _cvTime[i].ValidationGroup = _strDynamictabPart;





                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        //cellB[(i * 2) + 1] = new HtmlTableCell();

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ibValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ceDateTimeRecorded[i]);

                        //cell[(i * 2) + 1].Controls.Add(_meeDate[i]);
                        //cell[(i * 2) + 1].Controls.Add(_mevDate[i]);
                        cell[(i * 2) + 1].Controls.Add(_rvDate[i]);
                        cell[(i * 2) + 1].Controls.Add(_twmValue[i]);


                        cell[(i * 2) + 1].Controls.Add(_lblTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_txtTime[i]);

                        if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                        {
                            //_txtValue[i].Text = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();
                            //_txtTime[i].Text = "00:00";
                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                            if (ContentPage == "record")
                            _rfvValue[i].ValidationGroup = _strDynamictabPart;

                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }


                        cell[(i * 2) + 1].Controls.Add(_meeTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_cvTime[i]);

                        if (_qsMode != "")
                        {
                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {

                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    try
                                    {

                                        DateTime dtTempDateTime = DateTime.Now;  // DateTime.Parse(_dtRecordTypleColumlns.Rows[i]["DefaultValue"].ToString());

                                        _txtValue[i].Text = dtTempDateTime.Day.ToString("00") + "/" + dtTempDateTime.Month.ToString("00") + "/" + dtTempDateTime.Year.ToString();


                                        _txtTime[i].Text = Convert.ToDateTime(dtTempDateTime.ToString()).ToString("HH:m");
                                    }
                                    catch
                                    {
                                        //
                                    }
                                }
                            }
                        }
                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "trafficlight")
                    {
                        _imgValues[i] = new Image();
                        //_imgValues[i].ImageUrl = "~/App_Themes/Default/Images/dropdown.png";
                        _imgValues[i].ToolTip = "Traffic Light";

                        cell[(i * 2) + 1].Controls.Add(_imgValues[i]);
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "button")
                    {
                        _lnkValue[i] = new LinkButton();
                        _lnkValue[i].ID = "lnk" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lnkValue[i].ClientIDMode = ClientIDMode.Static;
                        _lnkValue[i].CausesValidation = false ;
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
                                    //_lnkValue[i].Text = "<img src='" + theButtonInfo.ImageFullPath + "' />";

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
                                    //if (_dtRecordTypleColumlns.Rows[i]["TextWidth"] != null && _dtRecordTypleColumlns.Rows[i]["TextWidth"].ToString() != "")
                                    //{
                                    //    _lnkValue[i].Width = int.Parse(_dtRecordTypleColumlns.Rows[i]["TextWidth"].ToString()) * 9;
                                    //}
                                    //if (_dtRecordTypleColumlns.Rows[i]["TextHeight"] != null && _dtRecordTypleColumlns.Rows[i]["TextHeight"].ToString() != "")
                                    //{
                                    //    _lnkValue[i].Height = int.Parse(_dtRecordTypleColumlns.Rows[i]["TextHeight"].ToString()) * 18;
                                    //}
                                }
                            }

                        }
                        _lnkValue[i].Visible = bVisible;
                        cell[(i * 2) + 1].Controls.Add(_lnkValue[i]);
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                    {

                        //ScriptManager theScriptManager1 = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");

                        //if (theScriptManager1 != null)
                        //    theScriptManager1.EnablePartialRendering = false;

                        _fuValue[i] = new FileUpload();
                        _fuValue[i].ID = "fu" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                        //_fuValue[i].ClientIDMode = ClientIDMode.Static;
                        _hfValue[i] = new HiddenField();
                        _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_hfValue[i].ClientIDMode = ClientIDMode.Static;

                        _hfValue2[i] = new HiddenField();
                        _hfValue2[i].ID = "hf2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_hfValue2[i].ClientIDMode = ClientIDMode.Static;


                        _lblValue[i] = new Label();
                        _lblValue[i].ID = "lblV" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_lblValue[i].ClientIDMode = ClientIDMode.Static;

                        _pnlDIV[i] = new Panel();
                        _pnlDIV[i].ID = "pnl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_pnlDIV[i].ClientIDMode = ClientIDMode.Static;

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _lblValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        //cellB[(i * 2) + 1] = new HtmlTableCell();

                        _pnlDIV[i].Controls.Add(_fuValue[i]);

                        string strFileExtension = "*.*";
                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                        {

                            strFileExtension = "*.jpg;*.gif;*.png";
                            cell[(i * 2) + 1].Controls.Add(_lblValue[i]);
                            cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);

                        }
                        else
                        {
                            cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);
                            cell[(i * 2) + 1].Controls.Add(_lblValue[i]);
                        }
                        cell[(i * 2) + 1].Controls.Add(_hfValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_hfValue2[i]);


                        _fuValue2[i] = new FileUpload();
                        _fuValue2[i].ID = "fu2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_fuValue2[i].ClientIDMode = ClientIDMode.Static;

                        cell[(i * 2) + 1].Controls.Add(_fuValue2[i]);

                        _lnkValue[i] = new LinkButton();
                        _lnkValue[i].ID = "lnk" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lnkValue[i].Text = "Use basic version";
                        _lnkValue[i].CausesValidation = false;
                        _lnkValue[i].OnClientClick = "UserBasic" + _strDynamictabPart + i.ToString() + "(); return false";
                        if (ContentPage == "record")
                        _lnkValue[i].ValidationGroup = _strDynamictabPart;
                        //_lnkValue[i].ClientIDMode = ClientIDMode.Static;
                        if (Mode == "view")
                        {
                            _lnkValue[i].Text = "";
                        }
                        //if (Mode.ToLower() == "edit")
                        //{
                        cell[(i * 2) + 1].Controls.Add(new LiteralControl("</br>"));
                        //}
                        cell[(i * 2) + 1].Controls.Add(_lnkValue[i]);

                        string strUserBasicMandatory = "";

                        //if (bool.Parse(_dtRecordTypleColumlns.Rows[i]["IsMandatory"].ToString()))
                        //{

                        //    _rfvValue[i] = new RequiredFieldValidator();
                        //    _rfvValue[i].ID = "rfv" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                        //    _rfvValue[i].Display = ValidatorDisplay.None;//
                        //    _rfvValue[i].ControlToValidate = _fuValue[i].ClientID;
                        //    _rfvValue[i].ErrorMessage = _dtRecordTypleColumlns.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";


                        //    cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        //    strUserBasicMandatory = @"if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";

                        //}


                        string strJSBasicUPload = @" 
                                                     var y = document.getElementById('" + _strDynamictabPart + _fuValue2[i].ID + @"');
                                                     if(y!=null)
                                                        y.style.display = 'none';
                                                    function UserBasic" + _strDynamictabPart + i.ToString() + @"() {
                                                    if(y==null){return;};
                                                    var y = document.getElementById('" + _strDynamictabPart + _fuValue2[i].ID + @"');
                                                    y.style.display = 'block'; 
                                                    document.getElementById('" + _strDynamictabPart + _hfValue2[i].ID + @"').value='yes';" + strUserBasicMandatory + @"

                                                    y = document.getElementById('" + _strDynamictabPart + _pnlDIV[i].ID + @"');
                                                    y.style.display = 'none';
                                                    y = document.getElementById('" + _strDynamictabPart + _lblValue[i].ID + @"');
                                                    y.style.display = 'none';

                                                    y = document.getElementById('" + _strDynamictabPart + _lnkValue[i].ID + @"');
                                                    y.style.display = 'none';

                                                     if (swfobject.hasFlashPlayerVersion('1')) {
                                                        //what to do
                                                        y = document.getElementById('" + _strDynamictabPart+ _lnkValue[i].ID + @"');
                                                        y.style.display = 'none';        
                                                    }
                                                    else {
                                                      UserBasic"+ _strDynamictabPart + i.ToString() + @"();
                                                    }

                                                }";

                        //if (!IsPostBack)
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strJSBasicUPload" + _strDynamictabPart + i.ToString(), strJSBasicUPload, true);




                        string strValidatorT = "";
                        string strValidatorF = "";
                        if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m"
                && _rfvValue[i] != null)
                        {
                            strValidatorT = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), true)};";
                            strValidatorF = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";

                            if (RecordID != null)
                            {
                                _strJS = _strJS + strValidatorF;
                            }
                        }

                        string strScriptPath = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/"+_strRecordFolder+"/Handler.ashx";

                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                        {
                            string strMaxHeight = "50";
                            if (_dtColumnsDetail.Rows[i]["TextHeight"] != DBNull.Value)
                            {
                                strMaxHeight = _dtColumnsDetail.Rows[i]["TextHeight"].ToString();
                            }
                            string strFilePath = _strFilesLocation + "/UserFiles/AppFiles/";

                            string strInnerHTML = "<img  title=\"Remove this image\" style=\"cursor:pointer;\"  id=\"dimg" + _strDynamictabPart + _hfValue[i].ID + "\" src=\"" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                                + "/App_Themes/Default/Images/icon_delete.gif\" />" +
                                "<a id=\"a" + _strDynamictabPart + _hfValue[i].ID + "\" target=\"_blank\" >"
                            + " <img style=\"padding-bottom:7px; max-height:"
                            + strMaxHeight + "px;\" id=\"img" + _strDynamictabPart + _hfValue[i].ID + "\"  />" + "</a><br/>";

                            _strJSPostBack = _strJSPostBack + @" $(document).ready(function () {
                                        $('#" + _strDynamictabPart + _fuValue[i].ID + @"').uploadify({
                                            'uploader': '../Document/uploadify/uploadify.swf',
                                            'script': '" + strScriptPath + @"',
                                            'cancelImg': '../Document/uploadify/cancel.png',
                                            'auto': true,
                                            'multi': false,
                                            'fileDesc': 'Files',
                                            'fileExt': '" + strFileExtension + @"',
                                            'queueSizeLimit': 90,
                                            'sizeLimit': 1000000000,
                                            'buttonText': 'Browse...',
                                            'folder': 'UserFiles/AppFiles',
                                            'onComplete': function (event, queueID, fileObj, response, data) { 
                                                jo = JSON.parse(response); 

                                                                                             
                                               document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value=jo.filename;

                                                $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html('" + strInnerHTML + @"'); "
                                                + strValidatorF + @"
                                                
                                                document.getElementById('img" + _strDynamictabPart + _hfValue[i].ID + @"').src=jo.fullpath;
                                                document.getElementById('img" + _strDynamictabPart + _hfValue[i].ID + @"').alt=fileObj.name;
                                                document.getElementById('img" + _strDynamictabPart + _hfValue[i].ID + @"').title=fileObj.name;
                                                document.getElementById('a" + _strDynamictabPart + _hfValue[i].ID + @"').href=jo.fullpath;

                                               

                                                  document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value='';
                                                      $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html(''); 
                                            });
                                               // alert(response);jo.fullpath
                   
                                            },
                                            'onSelect': function (event, ID, fileObj) {
                                                $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html('');
                                                document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value='';
                                                $('#" + _strDynamictabPart + _fuValue[i].ID + @"').uploadifySettings(
                                                'scriptData', { 'foo': 'UserFiles/AppFiles' }
                                                    );


                                                $('#" + _strDynamictabPart + _fuValue[i].ID + @"').uploadifyUpload();



                                            },
                                            'onCancel': function (event, ID, fileObj, data) {
                                               document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value=''; " + strValidatorT + @"
                                               $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html('');
                                            }

                                        });

                                          
                                    });";
                        }
                        else
                        {
                            string strInnerHTML = "<img  title=\"Remove this file\" style=\"cursor:pointer;\"  id=\"dimg" + _strDynamictabPart + _hfValue[i].ID + "\" src=\"" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />";

                            _strJSPostBack = _strJSPostBack + @" $(document).ready(function () {
                                        $('#" + _strDynamictabPart + _fuValue[i].ID + @"').uploadify({
                                            'uploader': '../Document/uploadify/uploadify.swf',
                                            'script': '" + strScriptPath + @"',
                                            'cancelImg': '../Document/uploadify/cancel.png',
                                            'auto': true,
                                            'multi': false,
                                            'fileDesc': 'Files',
                                            'fileExt': '" + strFileExtension + @"',
                                            'queueSizeLimit': 90,
                                            'sizeLimit': 1000000000,
                                            'buttonText': 'Browse...',
                                            'folder': 'UserFiles/AppFiles',
                                            'onComplete': function (event, queueID, fileObj, response, data) { 
                                                 jo = JSON.parse(response);                                               
                                                document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value=jo.filename;
                                                $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html('" + strInnerHTML + @"' + fileObj.name ); " + strValidatorF + @"                                                 
                                               
                                                     document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                         document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value='';
                                                            $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html(''); 
                                                    });
                                                 alert(fileObj.name + ' File has been uploaded.');
                   
                                            },
                                            'onSelect': function (event, ID, fileObj) {
                                                $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html('');
                                                document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value='';
                                                $('#" + _strDynamictabPart + _fuValue[i].ID + @"').uploadifySettings(
                                                'scriptData', { 'foo': 'UserFiles/AppFiles' }
                                                    );


                                                $('#" + _strDynamictabPart + _fuValue[i].ID + @"').uploadifyUpload();



                                            },
                                            'onCancel': function (event, ID, fileObj, data) {
                                               document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value=''; " + strValidatorT + @"
                                               $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html('');
                                            }

                                        });


                                    });";



                        }

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "uploadfile" + i.ToString(), "strFUJS", true);

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
                                //_txtValue[i].ClientIDMode = ClientIDMode.Static;
                                _txtValue[i].ToolTip = "Start typing and matching values will be shown";
                                if (ContentPage == "record")
                                _txtValue[i].ValidationGroup = _strDynamictabPart;

                                if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                                {
                                    _txtValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                                }

                                _imgValues[i] = new Image();
                                _imgValues[i].ImageUrl = "~/App_Themes/Default/Images/dropdown.png";
                                _imgValues[i].ToolTip = "Start typing and matching values will be shown";

                                _hfValue[i] = new HiddenField();
                                _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                //_hfValue[i].ClientIDMode = ClientIDMode.Static;



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
                                

                                
                                //cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                                // cell[(i * 2) + 1].Controls.Add(_imgValues[i]);

                                cell[(i * 2) + 1].Controls.Add(tblQuickLink);
                                cell[(i * 2) + 1].Controls.Add(_hfValue[i]);

                                if (_dtColumnsDetail.Rows[i]["QuickAddLink"] != DBNull.Value
                                    && Request.QueryString["quickadd"] == null)
                                {
                                    if (_dtColumnsDetail.Rows[i]["QuickAddLink"].ToString().ToLower() == "true")
                                    {
                                        _hlValue[i] = new HyperLink();
                                        _hlValue[i].ID = "hl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                        _hlValue[i].Text = "Add";

                                        string xml = null;
                                        xml = @"<root>" +
                                               " <mode>" + HttpUtility.HtmlEncode(Mode.ToString()) + "</mode>" +
                                               " <TableID>" + HttpUtility.HtmlEncode(TableID.ToString()) + "</TableID>" +
                                               " <SearchCriteriaID>" + HttpUtility.HtmlEncode(Request.QueryString["SearchCriteriaID"].ToString()) + "</SearchCriteriaID>" +
                                               " <control>" + HttpUtility.HtmlEncode(_txtValue[i].ID.ToString()) + "</control>" +
                                                " <TableTableID>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()) + "</TableTableID>" +
                                                 " <DisplayColumn>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["DisplayColumn"].ToString()) + "</DisplayColumn>" +
                                                  " <LinkedParentColumnID>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()) + "</LinkedParentColumnID>" +
                                                  " <DropDownType>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["DropDownType"].ToString()) + "</DropDownType>" +
                                                  " <_hfValue>" + HttpUtility.HtmlEncode(_hfValue[i].ID.ToString()) + "</_hfValue>" +
                                                  " <RecordID>" + HttpUtility.HtmlEncode(RecordID == null ? "-1" : RecordID.ToString()) + "</RecordID>" +
                                              "</root>";
                                        //
                                        SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
                                        int iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);

                                        _hlValue[i].NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" +
                                            Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(_dtColumnsDetail.Rows[i]["TableTableID"].ToString())
                                            + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&quickadd=" + Cryptography.Encrypt(iSearchCriteriaID.ToString());
                                        _hlValue[i].Visible = false;
                                        cellQL2.Controls.Add(new LiteralControl("&nbsp;"));
                                        cellQL2.Controls.Add(_hlValue[i]);
                                    }
                                }





                                if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                                {

                                    _rfvValue[i] = new RequiredFieldValidator();
                                    _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    _rfvValue[i].Display = ValidatorDisplay.None;
                                    _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                                    _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                                    if (ContentPage == "record")
                                    _rfvValue[i].ValidationGroup = _strDynamictabPart;

                                    cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                                }

                                string strAutoDDJS = @" $(function () {
                                $(""#" + _strDynamictabPart + _txtValue[i].ID.ToString() + @""").autocomplete({
                                    source: function (request, response) {
                                        $.ajax({
                                            url: ""../../CascadeDropdown.asmx/GetDisplayColumns"",
                                            data: ""{'Columnid':'" + _dtColumnsDetail.Rows[i]["ColumnID"].ToString() + @"', 'search': '"" + request.term + ""' }"",
                                            dataType: ""json"",
                                            type: ""POST"",
                                            contentType: ""application/json; charset=utf-8"",
                                            dataFilter: function (data) { return data; },
                                            success: function (data) {
                                                response($.map(data.d, function (item) {
                                                    return {
                                                        value: item.Text,
                                                        id: item.ID
                                                    }
                                                }))
                                            },
                                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                                alert(textStatus);
                                            }
                                        });
                                    },
                                                minLength: 1,
                                                select: function (event, ui) {
                                                    if (ui.item.id == null) {
                                                        document.getElementById('" + _strDynamictabPart + _hfValue[i].ID.ToString() + @"').value = '';
                                                    }
                                                    else {
                                                        document.getElementById('" + _strDynamictabPart + _hfValue[i].ID.ToString() + @"').value = ui.item.id;
                                                    }
                                                }
                                            });
                                        });

                                ";

                                if (Mode.ToLower() != "view")
                                {
                                    //if (!IsPostBack)
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AutoCompleteJS" + _strDynamictabPart + i.ToString(), strAutoDDJS, true);
                                }
                            }

                            //end of Table Predictive



                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                            {
                                //this is drop down
                                _ddlValue[i] = new DropDownList();
                                _ddlValue[i].ID = "ddl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _ddlValue[i].Width = 198;
                                _ddlValue[i].CssClass = "NormalTextBox";
                                if (ContentPage == "record")
                                _ddlValue[i].ValidationGroup = _strDynamictabPart;


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
                                //cell[(i * 2) + 1] = new HtmlTableCell();
                                //cell[(i * 2) + 1].Controls.Add(_ddlValue[i]);

                                _pnlDIV[i] = new Panel();

                                if (_bCustomDDL)
                                {
                                    _pnlDIV[i].CssClass = "ddlDIV";
                                    _ddlValue[i].CssClass = "ddlrrp";
                                    _ddlValue[i].Width = (int)_ddlValue[i].Width.Value + 22;
                                    _pnlDIV[i].Width = (int)_ddlValue[i].Width.Value - 20;
                                }
                                _pnlDIV[i].Controls.Add(_ddlValue[i]);
                                //cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);



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



                                if (_dtColumnsDetail.Rows[i]["QuickAddLink"] != DBNull.Value
                                    && Request.QueryString["quickadd"] == null)
                                {
                                    if (_dtColumnsDetail.Rows[i]["QuickAddLink"].ToString().ToLower() == "true")
                                    {
                                        _hlValue[i] = new HyperLink();
                                        _hlValue[i].ID = "hl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                        _hlValue[i].Text = "Add";
                                        string xml = null;
                                        xml = @"<root>" +
                                                " <mode>" + HttpUtility.HtmlEncode(Mode.ToString()) + "</mode>" +
                                                " <TableID>" + HttpUtility.HtmlEncode(TableID.ToString()) + "</TableID>" +
                                                " <SearchCriteriaID>" + HttpUtility.HtmlEncode(Request.QueryString["SearchCriteriaID"].ToString()) + "</SearchCriteriaID>" +
                                                " <control>" + HttpUtility.HtmlEncode(_ddlValue[i].ID.ToString()) + "</control>" +
                                                 " <TableTableID>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()) + "</TableTableID>" +
                                                 " <DisplayColumn>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["DisplayColumn"].ToString()) + "</DisplayColumn>" +
                                                 " <LinkedParentColumnID>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()) + "</LinkedParentColumnID>" +
                                                  " <DropDownType>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["DropDownType"].ToString()) + "</DropDownType>" +
                                                  " <RecordID>" + HttpUtility.HtmlEncode(RecordID == null ? "-1" : RecordID.ToString()) + "</RecordID>" +
                                               "</root>";

                                        SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
                                        int iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);

                                        _hlValue[i].NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" +
                                            Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(_dtColumnsDetail.Rows[i]["TableTableID"].ToString())
                                            + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&quickadd=" + Cryptography.Encrypt(iSearchCriteriaID.ToString());
                                        _hlValue[i].Visible = false;
                                        cellQL2.Controls.Add(new LiteralControl("&nbsp;"));
                                        cellQL2.Controls.Add(_hlValue[i]);
                                    }
                                }


                                if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                                {

                                    _rfvValue[i] = new RequiredFieldValidator();
                                    _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    _rfvValue[i].Display = ValidatorDisplay.None;
                                    _rfvValue[i].ControlToValidate = _ddlValue[i].ClientID;
                                    _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                                    if (ContentPage == "record")
                                    _rfvValue[i].ValidationGroup = _strDynamictabPart;

                                    cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                                }



                            }


                        }
                        else
                        {
                            //filterted table

                            _ddlValue2[i] = new DropDownList();
                            _ddlValue2[i].ID = "ddl2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _ddlValue2[i].Width = 198;
                            _ddlValue2[i].CssClass = "NormalTextBox";
                            if (ContentPage == "record")
                            _ddlValue2[i].ValidationGroup = _strDynamictabPart;


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


                            _ddlValue[i] = new DropDownList();
                            _ddlValue[i].ID = "ddl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _ddlValue[i].Width = 198;
                            _ddlValue[i].CssClass = "NormalTextBox";
                            if (ContentPage == "record")
                            _ddlValue[i].ValidationGroup = _strDynamictabPart;


                            if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                            {
                                _ddlValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                            }

                            if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                            {
                                _ddlValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                            }

                            _ccddl[i] = new CascadingDropDown();
                            _ccddl[i].ID = "ccddl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                            _ccddl[i].Category = _dtColumnsDetail.Rows[i]["ColumnID"].ToString() + "," + _dtColumnsDetail.Rows[i]["ParentColumnID"].ToString();
                            _ccddl[i].TargetControlID = _ddlValue[i].ID;
                            _ccddl[i].ParentControlID = "ddl2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                            _ccddl[i].ServicePath = "~/CascadeDropdown.asmx";
                            _ccddl[i].ServiceMethod = "GetFilteredData"; //filtered


                            //cell[(i * 2) + 1] = new HtmlTableCell();
                            //cell[(i * 2) + 1].Controls.Add(_ddlValue2[i]);

                            _pnlDIV2[i] = new Panel();
                            if (_bCustomDDL)
                            {
                                _pnlDIV2[i].CssClass = "ddlDIV";
                                _ddlValue2[i].CssClass = "ddlrrp";
                                _ddlValue2[i].Width = (int)_ddlValue2[i].Width.Value + 22;
                                _pnlDIV2[i].Width = (int)_ddlValue2[i].Width.Value - 20;
                            }
                            _pnlDIV2[i].Controls.Add(_ddlValue2[i]);
                            cell[(i * 2) + 1].Controls.Add(_pnlDIV2[i]);

                            //cell[(i * 2) + 1].Controls.Add(new LiteralControl("<br/>"));
                            //cell[(i * 2) + 1].Controls.Add(_ddlValue[i]);

                            _pnlDIV[i] = new Panel();

                            if (_bCustomDDL)
                            {
                                _pnlDIV[i].CssClass = "ddlDIV";
                                _ddlValue[i].CssClass = "ddlrrp";
                                _ddlValue[i].Width = (int)_ddlValue[i].Width.Value + 22;
                                _pnlDIV[i].Width = (int)_ddlValue[i].Width.Value - 20;
                            }
                            _pnlDIV[i].Controls.Add(_ddlValue[i]);
                            cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);

                            cell[(i * 2) + 1].Controls.Add(_ccddl[i]);

                            if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                            {

                                _rfvValue[i] = new RequiredFieldValidator();
                                _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _rfvValue[i].Display = ValidatorDisplay.None;
                                _rfvValue[i].ControlToValidate = _ddlValue[i].ClientID;
                                _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                                if (ContentPage == "record")
                                _rfvValue[i].ValidationGroup = _strDynamictabPart;

                                cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                            }





                        }





                    }



                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                    {
                        _radioList[i] = new RadioButtonList();
                        _radioList[i].RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                        _radioList[i].RepeatLayout = RepeatLayout.Flow;
                        //_radioList[i].CssClass = "";//_dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                        _radioList[i].ID = "radio" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        if (ContentPage == "record")
                        _radioList[i].ValidationGroup = _strDynamictabPart;
                        _radioList[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();

                        //cell[(i * 2) + 1] = new HtmlTableCell();

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

                        //_radioList[i].CssClass = "NormalTextBox";
                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutRadioList(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _radioList[i]);

                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {
                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    _radioList[i].SelectedValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                }
                            }
                        }
                        else if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text")
                        {
                            Common.PutRadioListValue_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _radioList[i]);

                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {
                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    string strDefaultValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                    if (strDefaultValue.IndexOf(",") > -1)
                                    {
                                        _radioList[i].SelectedValue = strDefaultValue.Substring(0, strDefaultValue.IndexOf(","));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Common.PutRadioListValue_Image(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _radioList[i],_strFilesLocation);
                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {
                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    string strDefaultValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                    if (strDefaultValue!="")
                                    {
                                        _radioList[i].SelectedValue = strDefaultValue;
                                    }
                                }
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

                        if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _radioList[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                            if (ContentPage == "record")
                            _rfvValue[i].ValidationGroup = _strDynamictabPart;

                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }


                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                    {


                        //cell[(i * 2) + 1] = new HtmlTableCell();

                        _hfValue[i] = new HiddenField();
                        _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_hfValue[i].ClientIDMode = ClientIDMode.Static;
                        if (_theAccount.MapCentreLat != null)
                        {
                            _hfValue[i].Value = _theAccount.MapCentreLat.ToString();
                        }
                        else
                        {
                            _hfValue[i].Value = "-25.80339840765346";
                        }

                        _hfValue2[i] = new HiddenField();
                        _hfValue2[i].ID = "hf2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_hfValue2[i].ClientIDMode = ClientIDMode.Static;
                        if (_theAccount.MapCentreLong != null)
                        {
                            _hfValue2[i].Value = _theAccount.MapCentreLong.ToString();
                        }
                        else
                        {
                            _hfValue2[i].Value = "135.56235835000007";
                        }

                        _hfValue3[i] = new HiddenField();
                        _hfValue3[i].ID = "hf3" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_hfValue3[i].ClientIDMode = ClientIDMode.Static;
                        _hfValue3[i].Value = "3";

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
                       

                        if(bShowMap)
                            cell[(i * 2) + 1].Controls.Add(new LiteralControl("<div align='left' id='map" + _strDynamictabPart + i.ToString() + "' style='width: " + strMapWidth + "px; height: " + strMapHeight + "px; margin-top: 10px;'></div>"));

                        if (_dtColumnsDetail.Rows[i]["ShowTotal"] != DBNull.Value)
                        {
                            if (_dtColumnsDetail.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
                            {
                                if (bShowMap)
                                    cell[(i * 2) + 1].Controls.Add(new LiteralControl("<br/>"));

                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("Address:&nbsp;"));
                                //cell[(i * 2) + 1].Controls.Add(new LiteralControl("Search Address: <input type='text' value='' id='searchbox" + i.ToString() + "' style='width: 280px; height: 18px; font-size: 12px;' >"));
                                _txtValue2[i] = new TextBox();
                                _txtValue2[i].ID = "searchbox" + i.ToString();
                                _txtValue2[i].Width = 280;
                                _txtValue2[i].CssClass = "NormalTextBox";
                                //_txtValue2[i].ClientIDMode = ClientIDMode.Static;
                                if (ContentPage == "record")
                                _txtValue2[i].ValidationGroup = _strDynamictabPart;

                                cell[(i * 2) + 1].Controls.Add(_txtValue2[i]);

                                if (bShowMap)
                                {
                                    strSearchAddressJS = @"                                                   
                                                        function showAddress" + _strDynamictabPart + i.ToString() + @"() {                                                      
                                                        if(document.getElementById('" + _strDynamictabPart + "searchbox" + i.ToString() + @"')==null){return;};
                                                        var address = document.getElementById('" + _strDynamictabPart + "searchbox" + i.ToString() + @"').value;         
                                                        var geocoder = new google.maps.Geocoder();
                                                        geocoder.geocode({ 'address': address }, function (results, status) {
                                                            if (status == google.maps.GeocoderStatus.OK) {
                                                                results[0].geometry.location;
                                                                var b = new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng());
                                                                map" + _strDynamictabPart + i.ToString() + @".setCenter(b);

                                                            } else {
                                                                alert('Google Maps had some trouble finding ' + address + '.');
                                                            }
                                                        });
                                                    };  

                                                   ";

                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchAddressJS" + i.ToString(), strSearchAddressJS, true);

                                    cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                                    _lnkValue[i] = new LinkButton();
                                    _lnkValue[i].ID = "lnk" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    _lnkValue[i].Text = "Search";
                                    _lnkValue[i].CausesValidation = false;
                                    _lnkValue[i].OnClientClick = "showAddress" + _strDynamictabPart + i.ToString() + "(); return false";
                                    if (ContentPage == "record")
                                        _lnkValue[i].ValidationGroup = _strDynamictabPart;
                                    //_lnkValue[i].ClientIDMode = ClientIDMode.Static;

                                    cell[(i * 2) + 1].Controls.Add(_lnkValue[i]);

                                    strShowAddress = @"$(function () {
                                                $('#" + _strDynamictabPart + "searchbox" + i.ToString() + @"').autocomplete({

                                                    source: function (request, response) {

                                                        if (geocoder == null) {
                                                            geocoder = new google.maps.Geocoder();
                                                        }
                                                        geocoder.geocode({ 'address': request.term }, function (results, status) {
                                                            if (status == google.maps.GeocoderStatus.OK) {

                                                                var searchLoc = results[0].geometry.location;
                                                                var lat = results[0].geometry.location.lat();
                                                                var lng = results[0].geometry.location.lng();
                                                                var latlng = new google.maps.LatLng(lat, lng);
                                                                var bounds = results[0].geometry.bounds;

                                                                geocoder.geocode({ 'latLng': latlng }, function (results1, status1) {
                                                                    if (status1 == google.maps.GeocoderStatus.OK) {
                                                                        if (results1[1]) {
                                                                            response($.map(results1, function (loc) {
                                                                                return {
                                                                                    label: loc.formatted_address,
                                                                                    value: loc.formatted_address,
                                                                                    bounds: loc.geometry.bounds
                                                                                }
                                                                            }));
                                                                        }
                                                                    }
                                                                });
                                                            }
                                                        });
                                                    },
                                                    select: function (event, ui) {
                                                        var pos = ui.item.position;
                                                        var lct = ui.item.locType;
                                                        var bounds = ui.item.bounds;

                                                        if (bounds) {
                                                            map" + _strDynamictabPart + i.ToString() + @".fitBounds(bounds);
                                                        }
                                                    }
                                                });
                                            });                                               


                                            ";

                                }
                                
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
                                if (ContentPage == "record")
                                _txtValue[i].ValidationGroup = _strDynamictabPart;
                                //_txtValue[i].ClientIDMode = ClientIDMode.Static;

                                cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;&nbsp;Longitude:&nbsp;"));

                                _txtTime[i] = new TextBox();
                                _txtTime[i].ID = "txtLong" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _txtTime[i].Width = 145;
                                _txtTime[i].CssClass = "NormalTextBox";
                                if (ContentPage == "record")
                                _txtTime[i].ValidationGroup = _strDynamictabPart;
                                //_txtTime[i].ClientIDMode = ClientIDMode.Static;

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

                              //if (strSearchAddressJS != "")
                              //    strMapJS = strMapJS + strSearchAddressJS;
                              //if (!IsPostBack)
                              ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AutoCompleteJS" + _strDynamictabPart + i.ToString(), strMapJS, true);
                              // ScriptManager.RegisterStartupScript(this, this.GetType(), "AutoCompleteJS" + i.ToString(), strMapJS, true);


                              //ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchAddressJS" + i.ToString(), strSearchAddressJS, true);
                          }

                       
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                    {
                        _chkValue[i] = new CheckBox();
                        _chkValue[i].ID = "chk" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        if (ContentPage == "record")
                        _chkValue[i].ValidationGroup = _strDynamictabPart;
                        Common.PutCheckBoxDefault(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _chkValue[i]);

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _chkValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        cell[(i * 2) + 1].Controls.Add(_chkValue[i]);
                    }


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                    {

                        _htmValue[i] = new WYSIWYGEditor();
                        _htmValue[i].ID = "htm" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _htmValue[i].AssetManager = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";
                        _htmValue[i].ButtonFeatures = new string[] { "FullScreen", "XHTMLFullSource", "RemoveFormat", "Undo", "Redo", "|", "Paragraph", "FontName", "FontSize", "|", "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyFull", "Bold", "Italic", "Underline", "Hyperlink" };
                        _htmValue[i].scriptPath = "../../Editor/scripts/";
                        _htmValue[i].ToolbarMode = 0;
                        _htmValue[i].Width = 520;
                        _htmValue[i].Height = 250;

                        if (_dtColumnsDetail.Rows[i]["DropDownValues"].ToString() != "")
                        {
                            _htmValue[i].Text = _dtColumnsDetail.Rows[i]["DropDownValues"].ToString();
                        }

                        //cell[(i * 2) + 1] = new HtmlTableCell();
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

                        if (_dtColumnsDetail.Rows[i]["DropDownValues"].ToString() != "")
                        {
                            _lblValue[i].Text = Server.HtmlDecode(_dtColumnsDetail.Rows[i]["DropDownValues"].ToString());
                        }
                        //cell[(i * 2) + 1] = new HtmlTableCell();
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
                        if (ContentPage == "record")
                        _lstValue[i].ValidationGroup = _strDynamictabPart;

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

                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        cell[(i * 2) + 1].Controls.Add(_lstValue[i]);


                        if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _lstValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                            if (ContentPage == "record")
                            _rfvValue[i].ValidationGroup = _strDynamictabPart;
                            
                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }


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


                        foreach (ListItem li in _cblValue[i].Items)
                        {
                            li.Attributes.Add("DataValue", li.Value);
                        }

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _cblValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        cell[(i * 2) + 1].Controls.Add(_cblValue[i]);


                        //if (bool.Parse(_dtRecordTypleColumlns.Rows[i]["IsMandatory"].ToString()))
                        //{

                        //    _rfvValue[i] = new RequiredFieldValidator();
                        //    _rfvValue[i].ID = "rfv" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                        //    _rfvValue[i].Display = ValidatorDisplay.None;
                        //    _rfvValue[i].ControlToValidate = _cblValue[i].ClientID;
                        //    _rfvValue[i].ErrorMessage = _dtRecordTypleColumlns.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                        //    cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        //}
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

                        if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                        {
                            _ddlValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                        }

                        if (ContentPage == "record")
                        _ddlValue[i].ValidationGroup = _strDynamictabPart;


                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutDDLValues(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _ddlValue[i]);

                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {
                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    _ddlValue[i].SelectedValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                }
                            }
                        }
                        else
                        {
                            Common.PutDDLValue_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _ddlValue[i]);

                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {
                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    string strDefaultValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                    if (strDefaultValue.IndexOf(",") > -1)
                                    {
                                        _ddlValue[i].SelectedValue = strDefaultValue.Substring(0, strDefaultValue.IndexOf(","));
                                    }
                                }
                            }
                        }


                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _ddlValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        //cell[(i * 2) + 1].Controls.Add(_ddlValue[i]);

                        _pnlDIV[i] = new Panel();

                        if (_bCustomDDL)
                        {
                            _pnlDIV[i].CssClass = "ddlDIV";
                            _ddlValue[i].CssClass = "ddlrrp";
                            _ddlValue[i].Width = (int)_ddlValue[i].Width.Value + 22;
                            _pnlDIV[i].Width = (int)_ddlValue[i].Width.Value - 20;
                        }
                        _pnlDIV[i].Controls.Add(_ddlValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_pnlDIV[i]);


                        if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _ddlValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                            if (ContentPage == "record")
                            _rfvValue[i].ValidationGroup = _strDynamictabPart;

                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }


                        _imgWarning[i] = new Image();
                        _imgWarning[i].ImageUrl = "~/Images/warning.png";
                        _imgWarning[i].Visible = false;
                        //cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                        cell[(i * 2) + 1].Controls.Add(_imgWarning[i]);

                    }

                    //if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() == "data_retriever")
                    //{
                    //    _txtValue[i] = new TextBox();
                    //    _txtValue[i].ID = "txt" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
                    //    _txtValue[i].Width = 198;
                    //    _txtValue[i].CssClass = "NormalTextBox";
                    //    _txtValue[i].ToolTip = "Calculated value - will be refreshed on save.";
                    //    _txtValue[i].Enabled = false;

                    //    cell[(i * 2) + 1] = new HtmlTableCell();

                    //    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                    //}



                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation")
                    {
                        _txtValue[i] = new TextBox();
                        _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtValue[i].Width = 198;
                        _txtValue[i].CssClass = "NormalTextBox";
                        _txtValue[i].ToolTip = "Calculated value - will be refreshed on save.";
                        _txtValue[i].Enabled = false;
                        if (ContentPage == "record")
                        _txtValue[i].ValidationGroup = _strDynamictabPart;

                        if (_dtColumnsDetail.Rows[i]["TextType"].ToString() == "f"
                         && _dtColumnsDetail.Rows[i]["RegEx"].ToString() != "")
                        {
                            _lbl[i].Text = _lbl[i].Text + "&nbsp;" + _dtColumnsDetail.Rows[i]["RegEx"].ToString();
                        }

                        //cell[(i * 2) + 1] = new HtmlTableCell();

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);

                        _imgWarning[i] = new Image();
                        _imgWarning[i].ImageUrl = "~/Images/warning.png";
                        _imgWarning[i].Visible = false;
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
                        if (ContentPage == "record")
                        _txtValue[i].ValidationGroup = _strDynamictabPart;

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


                        //
                        //cell[(i * 2) + 1] = new HtmlTableCell();

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



                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {
                                if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                {
                                    _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                }
                            }

                            //cell[(i * 2) + 1] = new HtmlTableCell();

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
                                    _revValue[i] = new RegularExpressionValidator();
                                    _revValue[i].ID = "rev" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    _revValue[i].Display = ValidatorDisplay.None;
                                    _revValue[i].ControlToValidate = _txtValue[i].ClientID;
                                    _revValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Invalid!";
                                    if (ContentPage == "record")
                                    _revValue[i].ValidationGroup = _strDynamictabPart;

                                    switch (_dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower())
                                    {
                                        case "text":
                                            _revValue[i].ValidationExpression = TextTypeRegEx.text;
                                            _revValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Only Text!";
                                            break;

                                        case "email":
                                            _revValue[i].ValidationExpression = TextTypeRegEx.email;
                                            _revValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Invalid Email!";

                                            break;

                                        case "isbn":
                                            _revValue[i].ValidationExpression = TextTypeRegEx.isbn;
                                            _revValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Invalid ISBN!";
                                            break;

                                        case "link":
                                            //if (Mode.ToLower() != "add")
                                            //{
                                            _hlValue[i] = new HyperLink();
                                            _hlValue[i].ID = "hl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                            _hlValue[i].Target = "_blank";
                                            _hlValue[i].Text = "Go";
                                            cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp"));
                                            cell[(i * 2) + 1].Controls.Add(_hlValue[i]);

                                            string strJSLink = @"$('#" + _strDynamictabPart + _txtValue[i].ID + @"').keypress(function () {
                                                 var strURL=document.getElementById('" + _strDynamictabPart + _txtValue[i].ID + @"').value;
	                                                if (strURL.indexOf('http')==-1)
                                                            {
                                                            strURL='http://' + strURL;
                                                            }
                                                            document.getElementById('" + _strDynamictabPart + _hlValue[i].ID + @"').href =strURL;

                                                   });
                                                        $('#" + _strDynamictabPart + _txtValue[i].ID + @"').change(function () {
                                                                var strURL=document.getElementById('" + _strDynamictabPart + _txtValue[i].ID + @"').value;
	                                                            if (strURL.indexOf('http')==-1)
                                                                        {
                                                                        strURL='http://' + strURL;
                                                                        }
                                                                        document.getElementById('" + _strDynamictabPart + _hlValue[i].ID + @"').href =strURL;

                                                                });


                                                        ";
                                            //if (!IsPostBack)
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "linkURL" + _strDynamictabPart + i.ToString(), strJSLink, true);
                                            //}


                                            _revValue[i].ValidationExpression = TextTypeRegEx.link;
                                            _revValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Invalid Link!";
                                            break;

                                        case "own":
                                            if (_dtColumnsDetail.Rows[i]["RegEx"] != DBNull.Value)
                                            {
                                                if (_dtColumnsDetail.Rows[i]["RegEx"].ToString() != "")
                                                {
                                                    _revValue[i].ValidationExpression = _dtColumnsDetail.Rows[i]["RegEx"].ToString();
                                                }
                                            }

                                            break;

                                        default:
                                            break;
                                    }


                                    cell[(i * 2) + 1].Controls.Add(_revValue[i]);
                                }
                            }



                        }
                        //

                        //
                        _imgWarning[i] = new Image();
                        _imgWarning[i].ImageUrl = "~/Images/warning.png";
                        _imgWarning[i].Visible = false;
                        cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                        cell[(i * 2) + 1].Controls.Add(_imgWarning[i]);



                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number" && bSlider == false)
                        {

                            //Disabled calculation field
                            if (_dtColumnsDetail.Rows[i]["Calculation"].ToString() != "")
                            {
                                //_txtValue[i].ReadOnly = true;
                                if (Mode != null)
                                {
                                    if (Mode.ToLower() == "add")
                                    {
                                        _txtValue[i].Text = "Calculated on Save";
                                    }

                                }

                                _txtValue[i].Enabled = false;

                            }


                            //Set constant/default

                            if (Mode != null)
                            {

                                //make constant readonly
                                if (_dtColumnsDetail.Rows[i]["Constant"].ToString() != "")
                                {
                                    //_txtValue[i].ReadOnly = true;
                                    if (Mode.ToLower() == "add")
                                    {
                                        _txtValue[i].Text = _dtColumnsDetail.Rows[i]["Constant"].ToString();
                                    }
                                    _txtValue[i].Enabled = false;
                                }


                                if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                                {
                                    if (Mode.ToLower() == "add" || Mode.ToLower() == "edit")
                                    {
                                        _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                    }
                                }

                            }


                            if (bool.Parse(_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString()) == false
                                && _dtColumnsDetail.Rows[i]["Calculation"].ToString() == "")
                            {
                                _revValue[i] = new RegularExpressionValidator();
                                _revValue[i].ID = "rev" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _revValue[i].Display = ValidatorDisplay.None;
                                _revValue[i].ControlToValidate = _txtValue[i].ClientID;
                                _revValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Numeric value please!";
                                //_revValue[i].ErrorMessage = " Numeric please!";
                                _revValue[i].ValidationExpression = @"(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,15}$)";
                                if (ContentPage == "record")
                                _revValue[i].ValidationGroup = _strDynamictabPart;



                                _ftbExt[i] = new FilteredTextBoxExtender();
                                _ftbExt[i].ID = "ftb" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _ftbExt[i].TargetControlID = _txtValue[i].ClientID;
                                _ftbExt[i].FilterType = FilterTypes.Custom;
                                _ftbExt[i].FilterMode = FilterModes.ValidChars;
                                _ftbExt[i].ValidChars = "-.0123456789";

                                cell[(i * 2) + 1].Controls.Add(_revValue[i]);

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

                        }


                        if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m")
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Mandatory.";
                            if (ContentPage == "record")
                            _rfvValue[i].ValidationGroup = _strDynamictabPart;

                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }
                    }





                    break;
            }




            trX[i] = new HtmlTableRow();





            trX[i].Cells.Add(cell[i * 2]);
            trX[i].Cells.Add(cell[(i * 2) + 1]);

            //if (bDisplayRight)
            //{
            //    tblRight.Rows.Add(trX[i]);
            //}
            //else
            //{
            //    tblLeft.Rows.Add(trX[i]);

            //}

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


        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Child- Created  CONTROLS - Begin DATA load ";
            theSpeedLog.FunctionLineNumber = 3230;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }


        //added & updated

        if (Mode != null)
        {
            if (Mode.ToLower() == "view")
            {
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
        }

        //

        if (!IsPostBack)
        {
            //RecordID == null && 
            if (Request.QueryString["public"] == null && Request.QueryString["parentRecordid"] != null)
            {
                //add put the defalut value for parent table 

                Record theParentRecord = RecordManager.ets_Record_Detail_Full(int.Parse(Cryptography.Decrypt(Request.QueryString["parentRecordid"].ToString())));

                if (theParentRecord != null)
                {

                    DataTable dtRecordedetail = Common.DataTableFromText(@"SELECT ColumnID, DisplayTextDetail, SystemName
			                FROM [Column] 
			                WHERE TableID = " + TableID.ToString() + @" AND DisplayTextDetail IS NOT NULL 
			                AND LEN(DisplayTextDetail) > 0
			                ORDER BY DisplayOrder");
                    for (int i = 0; i < dtRecordedetail.Rows.Count; i++)
                    {
                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                                && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                                || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                              && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                              && _dtColumnsDetail.Rows[i]["LinkedParentColumnID"] != DBNull.Value
                              && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "") //
                        {
                            if ((int)theParentRecord.TableID == int.Parse(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()))
                            {

                                string strParentRecordID = Cryptography.Decrypt(Request.QueryString["parentRecordid"].ToString());
                                if (strParentRecordID != "")
                                {

                                    int iTableRecordID = int.Parse(strParentRecordID);
                                    Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()));

                                    Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(iTableRecordID);
                                    string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);

                                    if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                                    {

                                        _hfValue[i].Value = strLinkedColumnValue;


                                        DataTable dtTableTableSC = Common.DataTableFromText(@"SELECT SystemName,DisplayName FROM 
                                            [Column] WHERE   TableID ="
                                   + _dtColumnsDetail.Rows[i]["TableTableID"].ToString());

                                        string strDisplayColumn = _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString();

                                        foreach (DataRow dr in dtTableTableSC.Rows)
                                        {
                                            strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                                        }

                                        DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + strParentRecordID);
                                        if (dtTheRecord.Rows.Count > 0)
                                        {
                                            foreach (DataColumn dc in dtTheRecord.Columns)
                                            {
                                                strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());


                                            }
                                        }


                                        _txtValue[i].Text = strDisplayColumn;
                                        _txtValue[i].Enabled = false;

                                    }
                                    if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                                    {
                                        _ddlValue[i].Text = strLinkedColumnValue;// strParentRecordID;
                                        _ddlValue[i].Enabled = false;
                                    }
                                }
                            }
                            //
                        }


                    }
                }


                //

            }


            if (Request.QueryString["public"] != null && Request.QueryString["ParentID"] != null && Request.QueryString["parentRecordid"] != null)
            {
                Record theParentRecord = RecordManager.ets_Record_Detail_Full(int.Parse(Cryptography.Decrypt(Request.QueryString["parentRecordid"].ToString())));

                if (theParentRecord != null && theParentRecord.TableID.ToString() == Request.QueryString["ParentID"].ToString())
                {

                    DataTable dtRecordedetail = Common.DataTableFromText(@"SELECT ColumnID, DisplayTextDetail, SystemName
			                FROM [Column] 
			                WHERE TableID = " + TableID.ToString() + @" AND DisplayTextDetail IS NOT NULL 
			                AND LEN(DisplayTextDetail) > 0
			                ORDER BY DisplayOrder");
                    for (int i = 0; i < dtRecordedetail.Rows.Count; i++)
                    {
                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                                && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                                || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                              && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                              && _dtColumnsDetail.Rows[i]["LinkedParentColumnID"] != DBNull.Value
                              && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "") //
                        {
                            if ((int)theParentRecord.TableID == int.Parse(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()))
                            {

                                string strParentRecordID = Cryptography.Decrypt(Request.QueryString["parentRecordid"].ToString());
                                if (strParentRecordID != "")
                                {

                                    int iTableRecordID = int.Parse(strParentRecordID);
                                    Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()));

                                    Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(iTableRecordID);
                                    string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);
                                    if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                                    {


                                        _hfValue[i].Value = strLinkedColumnValue;

                                        DataTable dtTableTableSC = Common.DataTableFromText(@"SELECT SystemName,DisplayName FROM 
                                            [Column] WHERE   TableID ="
                                   + _dtColumnsDetail.Rows[i]["TableTableID"].ToString());

                                        string strDisplayColumn = _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString();

                                        foreach (DataRow dr in dtTableTableSC.Rows)
                                        {
                                            strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                                        }

                                        DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + strParentRecordID);
                                        if (dtTheRecord.Rows.Count > 0)
                                        {
                                            foreach (DataColumn dc in dtTheRecord.Columns)
                                            {

                                                strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                            }
                                        }


                                        _txtValue[i].Text = strDisplayColumn;
                                        _txtValue[i].Enabled = false;


                                    }
                                    if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                                    {
                                        _ddlValue[i].Text = strLinkedColumnValue;
                                        _ddlValue[i].Enabled = false;

                                    }
                                }
                            }
                            //
                        }

                    }
                }
                else
                {
                    Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Public.aspx?ParentRecordID=" + Request.QueryString["ParentRecordID"].ToString() + "&TableID=" + TableID.ToString(), true);
                    return;

                }




            }

        }





        //if (RecordID != null)
        //{

        //    //MakeChildTables();

        //}
        //else
        //{
        //    //add mode
        //    //MakeChildTables();
        //}


        

        if (strFirstRecord != "")
        {
           
            if(!IsPostBack)
            {               
                PopulateRecord();
            }
               
        }
        else
        {
            if (OnlyOneRecord)
            {
                //create a blank record
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
                                && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value && _ParentRecord!=null)
                            {
                                Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));

                                if (theDefaultColumn != null )
                                {
                                    //strValue = RecordManager.GetRecordValue(ref _ParentRecord, theDefaultColumn.SystemName);
                                    strValue=  TheDatabaseS.spGetValueFromRelatedTable((int)_ParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);

                                    RecordManager.MakeTheRecord(ref theRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strValue);
                                }

                            }

                        }


                      


                        int iNewRecordID = RecordManager.ets_Record_Insert(theRecord);

                        if (_theTable.AddRecordSP != "")
                            RecordManager.AddRecordSP(_theTable.AddRecordSP, iNewRecordID, null);


                        ViewState["RecordID"] = iNewRecordID.ToString();
                        PopulateRecord();
                    }
                }

            }
            else
            {
                divDynamic.Visible = false;
            }

            if (ShowAddButton)
            {
                if (RecordID == null)
                {
                    divNorecordAdd.Visible = true;
                }
            }
            else
            {
                divNorecord.Visible = true;
            }
        }




        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Child- DATA load done - Begin ShowWhen";
            theSpeedLog.FunctionLineNumber = 3606;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }

        // copy show hide code





        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {
            if (_dtColumnsDetail.Rows[i]["CompareColumnID"] != DBNull.Value
                && _dtColumnsDetail.Rows[i]["CompareOperator"] != DBNull.Value)
            {
                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime"
                    || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                {
                    _cusvValue[i] = new CustomValidator();
                    _cusvValue[i].ID = "cusv" + _dtColumnsDetail.Rows[i]["SystemName"];
                    _cusvValue[i].ErrorMessage = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " " + Common.CompareOperatorErrorMsg( _dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " ";

                    if (ContentPage == "record")
                        _cusvValue[i].ValidationGroup = _strDynamictabPart;
                    
                    string strComparerOperator = "";
                    switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                    {
                        case "Equal":
                            strComparerOperator = "==";
                            break;
                        case "DataTypeCheck":
                            strComparerOperator = "===";
                            break;
                        case "GreaterThan":
                            strComparerOperator = ">";
                            break;
                        case "GreaterThanEqual":
                            strComparerOperator = ">=";
                            break;
                        case "LessThan":
                            strComparerOperator = "<";
                            break;
                        case "LessThanEqual":
                            strComparerOperator = "<=";
                            break;
                        case "NotEqual":
                            strComparerOperator = "!=";
                            break;
                        default:
                            strComparerOperator = "==";
                            break;

                    }



                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                    {

                        if (_txtValue[i] != null)
                            _cusvValue[i].ControlToValidate = _txtValue[i].ID;

                        string strControlToCompare = "";
                        for (int j = 0; j < _dtColumnsDetail.Rows.Count; j++)
                        {
                            if (_dtColumnsDetail.Rows[i]["CompareColumnID"].ToString()
                                == _dtColumnsDetail.Rows[j]["ColumnID"].ToString())
                            {
                                _cusvValue[i].ErrorMessage = _cusvValue[i].ErrorMessage + _dtColumnsDetail.Rows[j]["DisplayName"].ToString();


                                if (_dtColumnsDetail.Rows[j]["ColumnType"].ToString() == "time")
                                {
                                    if (_txtValue[j] != null)
                                        strControlToCompare = _txtValue[j].ClientID;
                                }
                                else
                                {
                                    if (_txtTime[j] != null)
                                        strControlToCompare = _txtTime[j].ClientID;
                                }
                            }

                        }


                        if (strControlToCompare != "")
                        {
                            string strJSCustomValidation = @" function compareTime" + _strDynamictabPart + i.ToString() + @"(sender, args) {
                                try
                                {
                                    var start = document.getElementById('" + strControlToCompare + @"');
                                    var end = document.getElementById('" + _txtValue[i].ClientID + @"');

                                    var time = start.value.split(':');  

                                    var d = new Date(); 

                                    d.setHours  (time[0]); 
                                    d.setMinutes(time[1]);


                                    var time2 = end.value.split(':');  

                                    var d2 = new Date(); 

                                    d2.setHours  (time2[0]); 
                                    d2.setMinutes(time2[1]);
                             
                                    //alert(d.getMilliseconds() + ' and ' + d2.getMilliseconds());
                                    args.IsValid = (d2.getTime()" + strComparerOperator + @"d.getTime());
                                }
                                catch(err)
                                {
                                //
                                }
                            }";

                            _cusvValue[i].ClientValidationFunction = "compareTime" + _strDynamictabPart + i.ToString();
                            cell[(i * 2) + 1].Controls.Add(_cusvValue[i]);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCustomValidation" + i.ToString(), strJSCustomValidation, true);

                        }


                    }
                    else
                    {
                        //this is date and time

                        _cvValue[i] = new CompareValidator();
                        _cvValue[i].ID = "cv" + _dtColumnsDetail.Rows[i]["SystemName"];
                        _cvValue[i].ErrorMessage = "";
                        _cvValue[i].Type = ValidationDataType.Date;

                        if (ContentPage == "record")
                            _cvValue[i].ValidationGroup = _strDynamictabPart;

                        switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                        {
                            case "Equal":
                                _cvValue[i].Operator = ValidationCompareOperator.Equal;
                                break;
                            case "DataTypeCheck":
                                _cvValue[i].Operator = ValidationCompareOperator.DataTypeCheck;
                                break;
                            case "GreaterThan":
                                _cvValue[i].Operator = ValidationCompareOperator.GreaterThanEqual;
                                break;
                            case "GreaterThanEqual":
                                _cvValue[i].Operator = ValidationCompareOperator.GreaterThanEqual;
                                break;
                            case "LessThan":
                                _cvValue[i].Operator = ValidationCompareOperator.LessThanEqual;
                                break;
                            case "LessThanEqual":
                                _cvValue[i].Operator = ValidationCompareOperator.LessThanEqual;
                                break;
                            case "NotEqual":
                                _cvValue[i].Operator = ValidationCompareOperator.NotEqual;
                                break;
                            default:
                                _cvValue[i].Operator = ValidationCompareOperator.Equal;
                                break;

                        }

                        if (_txtValue[i] != null)
                            _cvValue[i].ControlToValidate = _txtValue[i].ID;


                        if (_txtTime[i] != null)
                            _cusvValue[i].ControlToValidate = _txtTime[i].ID;



                        string strControlToCompareDate = "";
                        string strControlToCompareTime = "";
                        for (int j = 0; j < _dtColumnsDetail.Rows.Count; j++)
                        {
                            if (_dtColumnsDetail.Rows[i]["CompareColumnID"].ToString()
                                == _dtColumnsDetail.Rows[j]["ColumnID"].ToString())
                            {
                                _cusvValue[i].ErrorMessage = _cusvValue[i].ErrorMessage + _dtColumnsDetail.Rows[j]["DisplayName"].ToString();
                                if (_txtValue[j] != null && _txtTime[j] != null)
                                {
                                    strControlToCompareDate = _txtValue[j].ClientID;
                                    strControlToCompareTime = _txtTime[j].ClientID;

                                    if (_txtValue[j] != null)
                                        _cvValue[i].ControlToCompare = _txtValue[j].ID;
                                }

                                if (_txtValue[j] != null && _txtTime[j] == null)
                                {
                                    //it's a date
                                    //strControlToCompareDate = _txtValue[j].ClientID;
                                    //strControlToCompareTime = _txtTime[j].ClientID;

                                    if (_txtValue[j] != null)
                                        _cvValue[i].ControlToCompare = _txtValue[j].ID;
                                }
                            }

                        }




                        if (strControlToCompareDate != "" && strControlToCompareTime != "")
                        {
                            string strJSCustomValidation = @" function compareTime" + _strDynamictabPart + i.ToString() + @"(sender, args) {
                                    try
                                    {
                                        var startD = document.getElementById('" + strControlToCompareDate + @"');
                                        var startT = document.getElementById('" + strControlToCompareTime + @"');
                                        var endD = document.getElementById('" + _txtValue[i].ClientID + @"');
                                        var endT = document.getElementById('" + _txtTime[i].ClientID + @"');
                              

                                         var d = new Date(startD.value.replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') + ' ' + startT.value); 
                                      var d2 = new Date(endD.value.replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') + ' ' + endT.value); 

                         
                              
                             
                                        //alert(d.getMilliseconds() + ' and ' + d2.getMilliseconds());
                                        args.IsValid = (d2.getTime()" + strComparerOperator + @"d.getTime());
                                    }
                                    catch(err)
                                    {
                                    //
                                    }
                            }";

                            _cusvValue[i].ClientValidationFunction = "compareTime" + _strDynamictabPart + i.ToString();
                            cell[(i * 2) + 1].Controls.Add(_cusvValue[i]);

                            if (_cvValue[i].ControlToCompare != "" && _cvValue[i].ControlToValidate != "")
                                cell[(i * 2) + 1].Controls.Add(_cvValue[i]);

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JSCustomValidation" + _strDynamictabPart + i.ToString(), strJSCustomValidation, true);

                        }

                        if (strControlToCompareDate == "" && strControlToCompareTime == "")
                        {
                            //its a date

                            if (_cvValue[i].ControlToCompare != "" && _cvValue[i].ControlToValidate != "")
                            {
                                if (_cusvValue[i] != null)
                                    _cvValue[i].ErrorMessage = _cusvValue[i].ErrorMessage;

                                cell[(i * 2) + 1].Controls.Add(_cvValue[i]);
                            }


                        }
                    }
                }
                else
                {
                    _cvValue[i] = new CompareValidator();
                    _cvValue[i].ID = "cv" + _dtColumnsDetail.Rows[i]["SystemName"];
                    _cvValue[i].ErrorMessage = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " " + Common.CompareOperatorErrorMsg( _dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " ";
                    if (ContentPage == "record")
                        _cvValue[i].ValidationGroup = _strDynamictabPart;
                    switch (_dtColumnsDetail.Rows[i]["ColumnType"].ToString())
                    {
                        case "datetime":
                            _cvValue[i].Type = ValidationDataType.Date;
                            break;

                        //case "time":
                        //    _cvValue[i].Type = ValidationDataType.Date;
                        //    break;

                        case "date":
                            _cvValue[i].Type = ValidationDataType.Date;
                            break;

                        case "number":
                            _cvValue[i].Type = ValidationDataType.Double;
                            break;

                        default:
                            _cvValue[i].Type = ValidationDataType.String;
                            break;
                    }



                    switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                    {
                        case "Equal":
                            _cvValue[i].Operator = ValidationCompareOperator.Equal;
                            break;
                        case "DataTypeCheck":
                            _cvValue[i].Operator = ValidationCompareOperator.DataTypeCheck;
                            break;
                        case "GreaterThan":
                            _cvValue[i].Operator = ValidationCompareOperator.GreaterThan;
                            break;
                        case "GreaterThanEqual":
                            _cvValue[i].Operator = ValidationCompareOperator.GreaterThanEqual;
                            break;
                        case "LessThan":
                            _cvValue[i].Operator = ValidationCompareOperator.LessThan;
                            break;
                        case "LessThanEqual":
                            _cvValue[i].Operator = ValidationCompareOperator.LessThanEqual;
                            break;
                        case "NotEqual":
                            _cvValue[i].Operator = ValidationCompareOperator.NotEqual;
                            break;
                        default:
                            _cvValue[i].Operator = ValidationCompareOperator.Equal;
                            break;

                    }

                    if (_ddlValue[i] != null)
                        _cvValue[i].ControlToValidate = _ddlValue[i].ID;

                    if (_ddlValue2[i] != null)
                        _cvValue[i].ControlToValidate = _ddlValue2[i].ID;
                    if (_chkValue[i] != null)
                        _cvValue[i].ControlToValidate = _chkValue[i].ID;
                    if (_lstValue[i] != null)
                        _cvValue[i].ControlToValidate = _lstValue[i].ID;
                    //if (_cblValue[i] != null)
                    //    _cvValue[i].ControlToValidate = _cblValue[i].ID;
                    if (_radioList[i] != null)
                        _cvValue[i].ControlToValidate = _radioList[i].ID;
                    if (_lstValue[i] != null)
                        _cvValue[i].ControlToValidate = _lstValue[i].ID;
                    if (_txtValue[i] != null)
                        _cvValue[i].ControlToValidate = _txtValue[i].ID;


                    //now lets find the ControlToCompare function


                    for (int j = 0; j < _dtColumnsDetail.Rows.Count; j++)
                    {
                        if (_dtColumnsDetail.Rows[i]["CompareColumnID"].ToString()
                            == _dtColumnsDetail.Rows[j]["ColumnID"].ToString())
                        {

                            _cvValue[i].ErrorMessage = _cvValue[i].ErrorMessage  + _dtColumnsDetail.Rows[j]["DisplayName"].ToString();

                            if (_ddlValue[j] != null)
                                _cvValue[i].ControlToCompare = _ddlValue[j].ID;
                            if (_ddlValue2[j] != null)
                                _cvValue[i].ControlToCompare = _ddlValue2[j].ID;

                            if (_chkValue[j] != null)
                                _cvValue[i].ControlToCompare = _chkValue[j].ID;
                            if (_lstValue[j] != null)
                                _cvValue[i].ControlToCompare = _lstValue[j].ID;
                            //if (_cblValue[j] != null)
                            //    _cvValue[i].ControlToCompare = _cblValue[j].ID;
                            if (_radioList[j] != null)
                                _cvValue[i].ControlToCompare = _radioList[j].ID;
                            if (_lstValue[j] != null)
                                _cvValue[i].ControlToCompare = _lstValue[j].ID;

                            if (_txtValue[j] != null)
                                _cvValue[i].ControlToCompare = _txtValue[j].ID;
                        }
                    }
                    if (_cvValue[i].ControlToCompare != "" && _cvValue[i].ControlToValidate != "")
                        cell[(i * 2) + 1].Controls.Add(_cvValue[i]);



                }



            }
        }
       
        //for (int i = 0; i < _dtRecordTypleColumlns.Rows.Count; i++)
        //{
        //    if (_dtRecordTypleColumlns.Rows[i]["CompareColumnID"] != DBNull.Value
        //        && _dtRecordTypleColumlns.Rows[i]["CompareOperator"] != DBNull.Value)
        //    {
        //        _cvValue[i] = new CompareValidator();
        //        _cvValue[i].ID = "cv" + _dtRecordTypleColumlns.Rows[i]["SystemName"];
        //        _cvValue[i].ErrorMessage = "Not Correct: Between " + _dtRecordTypleColumlns.Rows[i]["DisplayName"].ToString();

        //        switch (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString())
        //        {
        //            case "datetime":
        //                _cvValue[i].Type = ValidationDataType.Date;
        //                break;

        //            case "time":
        //                _cvValue[i].Type = ValidationDataType.Date;
        //                break;

        //            case "date":
        //                _cvValue[i].Type = ValidationDataType.Date;
        //                break;

        //            case "number":
        //                _cvValue[i].Type = ValidationDataType.Double;
        //                break;

        //            default:
        //                _cvValue[i].Type = ValidationDataType.String;
        //                break;
        //        }



        //        switch (_dtRecordTypleColumlns.Rows[i]["CompareOperator"].ToString())
        //        {
        //            case "Equal":
        //                _cvValue[i].Operator = ValidationCompareOperator.Equal;
        //                break;
        //            case "DataTypeCheck":
        //                _cvValue[i].Operator = ValidationCompareOperator.DataTypeCheck;
        //                break;
        //            case "GreaterThan":
        //                _cvValue[i].Operator = ValidationCompareOperator.GreaterThan;
        //                break;
        //            case "GreaterThanEqual":
        //                _cvValue[i].Operator = ValidationCompareOperator.GreaterThanEqual;
        //                break;
        //            case "LessThan":
        //                _cvValue[i].Operator = ValidationCompareOperator.LessThan;
        //                break;
        //            case "LessThanEqual":
        //                _cvValue[i].Operator = ValidationCompareOperator.LessThanEqual;
        //                break;
        //            case "NotEqual":
        //                _cvValue[i].Operator = ValidationCompareOperator.NotEqual;
        //                break;
        //            default:
        //                _cvValue[i].Operator = ValidationCompareOperator.Equal;
        //                break;

        //        }

        //        if (_ddlValue[i] != null)
        //            _cvValue[i].ControlToValidate = _ddlValue[i].ID;

        //        if (_ddlValue2[i] != null)
        //            _cvValue[i].ControlToValidate = _ddlValue2[i].ID;
        //        if (_chkValue[i] != null)
        //            _cvValue[i].ControlToValidate = _chkValue[i].ID;
        //        if (_lstValue[i] != null)
        //            _cvValue[i].ControlToValidate = _lstValue[i].ID;
        //        if (_cblValue[i] != null)
        //            _cvValue[i].ControlToValidate = _cblValue[i].ID;
        //        if (_radioList[i] != null)
        //            _cvValue[i].ControlToValidate = _radioList[i].ID;
        //        if (_lstValue[i] != null)
        //            _cvValue[i].ControlToValidate = _lstValue[i].ID;
        //        if (_txtValue[i] != null)
        //            _cvValue[i].ControlToValidate = _txtValue[i].ID;


        //        //now lets find the ControlToCompare function


        //        for (int j = 0; j < _dtRecordTypleColumlns.Rows.Count; j++)
        //        {
        //            if (_dtRecordTypleColumlns.Rows[i]["CompareColumnID"].ToString()
        //                == _dtRecordTypleColumlns.Rows[j]["ColumnID"].ToString())
        //            {

        //                _cvValue[i].ErrorMessage = _cvValue[i].ErrorMessage + " and " + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString();

        //                if (_ddlValue[j] != null)
        //                    _cvValue[i].ControlToCompare = _ddlValue[j].ID;
        //                if (_ddlValue2[j] != null)
        //                    _cvValue[i].ControlToCompare = _ddlValue2[j].ID;

        //                if (_chkValue[j] != null)
        //                    _cvValue[i].ControlToCompare = _chkValue[j].ID;
        //                if (_lstValue[j] != null)
        //                    _cvValue[i].ControlToCompare = _lstValue[j].ID;
        //                if (_cblValue[j] != null)
        //                    _cvValue[i].ControlToCompare = _cblValue[j].ID;
        //                if (_radioList[j] != null)
        //                    _cvValue[i].ControlToCompare = _radioList[j].ID;
        //                if (_lstValue[j] != null)
        //                    _cvValue[i].ControlToCompare = _lstValue[j].ID;

        //                if (_txtValue[j] != null)
        //                    _cvValue[i].ControlToCompare = _txtValue[j].ID;
        //            }
        //        }

        //        if (_cvValue[i].ControlToCompare != "")
        //            cell[(i * 2) + 1].Controls.Add(_cvValue[i]);




        //    }
        //}





        ShowHideControls();






        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Child- Init - End ";
            theSpeedLog.FunctionLineNumber = 3570;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }
        




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
                            && _qsRecord != null)
                        {

                            if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                            {
                                if (_qsRecord.EnteredBy != _objUser.UserID)
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

                    if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m"
                        && _rfvValue[i] != null)
                    {
                        strOnlyAdminJS = strOnlyAdminJS + "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";
                    }
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

                string strValidatorT = "";
                string strValidatorF = "";
                string strBeforeShowHideFunction = "";
                 if (_dtColumnsDetail.Rows[i]["Importance"].ToString() == "m" && _rfvValue[i] != null)
                {
                    strValidatorT = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), true)};";
                    strValidatorF = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";
                }


                foreach (DataRow drSW in dtShowWhen.Rows)
                {
                    if (drSW["ColumnID"] != DBNull.Value!=null && drSW["HideColumnID"] != DBNull.Value)
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
                                        strBeforeShowHideFunction = strBeforeShowHideFunction + "$('" + strTargetTRID + "').fadeOut(); " + strValidatorF;
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
                                                            $('" + strTargetTRID + @"').fadeOut();" + strValidatorF + @"
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

                                                            $('" + strTargetTRID + @"').fadeOut();" + strValidatorF + @"
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

                                                    if (_cvTime[m] != null)
                                                        _cvTime[m].Enabled = false;
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

                                                    if (_cvTime[m] != null)
                                                        _cvTime[m].Enabled = false;

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
                                                                               $('" + strTargetTRID + @"').stop(true,true); $('" + strTargetTRID + @"').fadeIn();" + strValidatorT + @"
                                                                            }
                                                                            else {
                                                                                $('" + strTargetTRID + @"').fadeOut();" + strValidatorF + @"
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





//            if (dtShowWhen.Rows.Count ==-1) //not used
//            {
                                
//                if (dtShowWhen.Rows.Count == -1)
//                {

//                    trX[i].ID = "trX" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
//                    string strHideColumnValue = dtShowWhen.Rows[0]["HideColumnValue"].ToString();
//                    bool bEquals = true;
//                    string strHideOperatorLogic = "";
//                    if (dtShowWhen.Rows[0]["HideOperator"].ToString() == "equals")
//                    {
//                        bEquals = true;
//                    }
//                    else
//                    {
//                        bEquals = false;
//                    }


//                    for (int m = 0; m < _dtRecordTypleColumlns.Rows.Count; m++)
//                    {
//                        if (dtShowWhen.Rows[0]["HideColumnID"].ToString()
//                            == _dtRecordTypleColumlns.Rows[m]["ColumnID"].ToString())
//                        {
//                            if (trX[m] != null)
//                            {
//                                trX[m].ID = "trX" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                string driverID = "#" + trX[m].ClientID.Substring(0, trX[m].ClientID.Length - 7);
//                                string targetID = driverID + trX[i].ID;
//                                string dirverGroupID = driverID;
//                                if (_ddlValue[m] != null && _ddlValue2[m] == null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_ddlValue[m] != null && _ddlValue2[m] != null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                if (_txtValue[m] != null)
//                                    driverID = driverID + "txt" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_chkValue[m] != null)
//                                    driverID = driverID + "chk" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_lstValue[m] != null)
//                                    driverID = driverID + "lst" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_cblValue[m] != null)
//                                    driverID = driverID + "cbl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                dirverGroupID = driverID;

//                                string strHideShow = "";

//                                string strValidatorT = "";
//                                string strValidatorF = "";
//                                if (bool.Parse(_dtRecordTypleColumlns.Rows[i]["IsMandatory"].ToString())
//                        && _rfvValue[i] != null)
//                                {
//                                    strValidatorT = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), true)};";
//                                    strValidatorF = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";
//                                }


//                                if (_radioList[m] != null)
//                                {
//                                    driverID = driverID + "radio" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                    dirverGroupID = driverID;
//                                    driverID = driverID + "_";
//                                    strHideShow = strHideShow + "$('" + targetID + "').fadeOut(); " + strValidatorF;
//                                    string strDriverIDMain = driverID;
//                                    for (int n = 0; n < _radioList[m].Items.Count; n++)
//                                    {


//                                        driverID = strDriverIDMain + n.ToString();
//                                        strHideShow = strHideShow + @"  
//                                            $('" + driverID + @"').change(function (e) {
//                                            var strDDDC = $('" + driverID + @"').val();
//                                             if (strDDDC == '" + strHideColumnValue + @"') {
//                                                $('" + targetID + @"').fadeIn(); " + strValidatorT + @"
//                                            }
//                                            else {
//                                                $('" + targetID + @"').fadeOut(); " + strValidatorF + @"
//                                            }
//                                        });
//                                        ";

//                                        if (_radioList[m].SelectedIndex == n)
//                                        {
//                                            strHideShow = strHideShow + "$('" + driverID + "').trigger('change');";
//                                        }


//                                    }

//                                }
//                                else if (_chkValue[m] != null)
//                                {

//                                    string strTrue = "";
//                                    string strFalse = "";
//                                    Common.GetCheckTcikedUnTicked(_dtRecordTypleColumlns.Rows[m]["DropDownValues"].ToString(), ref strTrue, ref strFalse);
//                                    strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//                                    var chk = document.getElementById('" + driverID.Substring(1, driverID.Length - 1) + @"');
//                                    var strDDDC ='';
//                                    if (chk.checked == true) { strDDDC = '" + strTrue + @"'; }
//                                    if (chk.checked == false) { strDDDC = '" + strFalse + @"'; }                                 
//                                    if (strDDDC == '" + strHideColumnValue + @"') {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }
//                                });
//                                $('" + driverID + @"').trigger('change');  ";
//                                }
//                                else if (_ddlValue[m] != null || _ddlValue2[m] != null)
//                                {

//                                    strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//                                    var strDDDC = $('" + dirverGroupID + @"').val();
//                                    if (strDDDC == '" + strHideColumnValue + @"') {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }
//                                });
//                                $('" + driverID + @"').trigger('change');  ";
//                                }
//                                else if (_lstValue[m] != null)
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                        strHideShow = @"                                    
//                                        $('" + driverID + @"').change(function (e) {
//                                        var strDDDC = $('" + dirverGroupID + @"').val();
//                                        if (" + strHideOperatorLogic + @") {
//                                            $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//                                    });
//                                    $('" + driverID + @"').trigger('change');  ";

//                                    }
//                                    else
//                                    {


//                                        strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        var bShow=false;
//                                        var strHideValues='" + strHideColumnValue + @"';
//                                        if(strHideValues==null || strHideValues=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array=strHideValues.split(',');
//                                        var strDDDC = $('" + dirverGroupID + @"').val();
//                                      
//                                        if(strDDDC==null || strDDDC=='' || strDDDC=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC=strDDDC.toString();
//                                        var strDDDC_array=strDDDC.split(',');
//                                         for(var i = 0; i < strHideValues_array.length; i++) {
//                                            
//                                                if(strHideValues_array[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array.length; j++) 
//                                                    {
//                                                         if(strDDDC_array[j]!='')
//                                                        {
//                                                            if(strHideValues_array[i]==strDDDC_array[j])
//                                                                {
//                                                                bShow=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          }
//
//                                        if ( bShow==true) {
//                                            $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//
//                                        
//                                        });
//                                    $('" + driverID + @"').trigger('change');  ";
//                                    }



//                                }
//                                else if (_cblValue[m] != null)
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                        strHideShow = @"                                    
//                                        $('" + driverID + @"').click(function (e) {
//                                        //var strDDDC = $('" + dirverGroupID + @"').val();
//                                        var chkBox = document.getElementById('" + driverID.Replace("#", "") + @"');
//                                         var options = chkBox.getElementsByTagName('input');
//                                         var listOfSpans = chkBox.getElementsByTagName('span');
//                                         var strDDDC='';
//                                         for (var i = 0; i < options.length; i++) {
//                                             if (options[i].checked) {
//                                                 strDDDC=listOfSpans[i].attributes['DataValue'].value;
//                                             }
//                                         }
//
//                                        if (" + strHideOperatorLogic + @") {
//                                              $('" + targetID + @"').stop(true,true);$('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//                                    });
//                                    $('" + driverID + @"').trigger('click');  ";

//                                    }
//                                    else
//                                    {


//                                        strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        var bShow=false;
//                                        var strHideValues='" + strHideColumnValue + @"';
//                                        if(strHideValues==null || strHideValues=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array=strHideValues.split(',');
//                                        //var strDDDC = $('" + dirverGroupID + @"').val();
//                                      
//
//
//                                    var chkBox = document.getElementById('" + driverID.Replace("#", "") + @"');
//                                         var options = chkBox.getElementsByTagName('input');
//                                         var listOfSpans = chkBox.getElementsByTagName('span');
//                                        var strDDDC='';
//                                         for (var i = 0; i < options.length; i++) {
//                                             if (options[i].checked) {
//                                                 strDDDC=strDDDC+ ',' + listOfSpans[i].attributes['DataValue'].value;
//                                             }
//                                         }
//                                        if(strDDDC==null || strDDDC=='' || strDDDC=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC=strDDDC.toString();
//                                        var strDDDC_array=strDDDC.split(',');
//                                         for(var i = 0; i < strHideValues_array.length; i++) {
//                                            
//                                                if(strHideValues_array[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array.length; j++) 
//                                                    {
//                                                         if(strDDDC_array[j]!='')
//                                                        {
//                                                            if(strHideValues_array[i]==strDDDC_array[j])
//                                                                {
//                                                                bShow=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          }
//
//                                        if ( bShow==true) {
//                                              $('" + targetID + @"').stop(true,true);$('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//
//                                        
//                                        });
//                                    $('" + driverID + @"').trigger('change');  ";
//                                    }



//                                }
//                                else
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                    }
//                                    else
//                                    {
//                                        strHideOperatorLogic = "strDDDC.indexOf('" + strHideColumnValue + "')>=0";
//                                    }

//                                    strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//                                    var strDDDC = $('" + dirverGroupID + @"').val();
//                                    if (" + strHideOperatorLogic + @") {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }
//                                });
//                                $('" + driverID + @"').trigger('change');  ";

//                                }



//                                //if (!IsPostBack)
//                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHide" + _strDynamictabPart + i.ToString(), strHideShow, true);

//                            }
//                        }
//                    }



//                }



//                if (dtShowWhen.Rows.Count == -3)
//                {

//                    trX[i].ID = "trX" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
//                    string strHideColumnValue = dtShowWhen.Rows[0]["HideColumnValue"].ToString();
//                    string strHideColumnValue2 = dtShowWhen.Rows[2]["HideColumnValue"].ToString();
//                    string strJoinOperator = " && ";
//                    if (dtShowWhen.Rows[1]["JoinOperator"].ToString() == "and")
//                    {
//                        strJoinOperator = " && ";
//                    }
//                    else
//                    {
//                        strJoinOperator = " || ";
//                    }

//                    bool bEquals = true;
//                    bool bEquals2 = true;
//                    string strHideOperatorLogic = "";
//                    string strHideOperatorLogic2 = "";
//                    if (dtShowWhen.Rows[0]["HideOperator"].ToString() == "equals")
//                    {
//                        bEquals = true;
//                    }
//                    else
//                    {
//                        bEquals = false;
//                    }

//                    if (dtShowWhen.Rows[2]["HideOperator"].ToString() == "equals")
//                    {
//                        bEquals2 = true;
//                    }
//                    else
//                    {
//                        bEquals2 = false;
//                    }


//                    string strDriverID2 = "";
//                    //string strDirverID2Logic = "";
//                    string strShowHideJS2 = "";
//                    string strShowHideEventJS2 = "";
//                    for (int m = 0; m < _dtRecordTypleColumlns.Rows.Count; m++)
//                    {
//                        if (dtShowWhen.Rows[2]["HideColumnID"].ToString()
//                            == _dtRecordTypleColumlns.Rows[m]["ColumnID"].ToString())
//                        {
//                            if (trX[m] != null)
//                            {
//                                trX[m].ID = "trX" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                string driverID = "#" + trX[m].ClientID.Substring(0, trX[m].ClientID.Length - 7);
//                                string dirverGroupID = driverID;
//                                if (_ddlValue[m] != null && _ddlValue2[m] == null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_ddlValue[m] != null && _ddlValue2[m] != null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                if (_txtValue[m] != null)
//                                    driverID = driverID + "txt" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_chkValue[m] != null)
//                                    driverID = driverID + "chk" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_lstValue[m] != null)
//                                    driverID = driverID + "lst" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                if (_cblValue[m] != null)
//                                    driverID = driverID + "cbl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                dirverGroupID = driverID;
//                                strDriverID2 = dirverGroupID;


//                                if (_radioList[m] != null)
//                                {
//                                    driverID = driverID + "radio" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                    dirverGroupID = driverID;
//                                    //var strDDDC2 = document.querySelector('input[name="""+dirverGroupID.Replace("#","").Replace("_","$")+@"""]:checked').value;   
//                                    strShowHideJS2 = @" 
//                                               var strDDDC2 =GetOptValue('" + dirverGroupID.Replace("#", "").Replace("_", "$") + @"');                                                                                   
//                                         ";

//                                    driverID = driverID + "_";
//                                    string strDriverIDMain = driverID;
//                                    for (int n = 0; n < _radioList[m].Items.Count; n++)
//                                    {
//                                        driverID = strDriverIDMain + n.ToString();

//                                        strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";

//                                        strShowHideEventJS2 = strShowHideEventJS2 + @" $('" + driverID + @"').change(function (e) {
//                                            ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                        }); ";

//                                        if (_radioList[m].SelectedIndex == n)
//                                        {
//                                            strShowHideEventJS2 = strShowHideEventJS2 + "$('" + driverID + "').trigger('change');";
//                                        }
//                                    }
//                                }
//                                else if (_chkValue[m] != null)
//                                {
//                                    string strTrue = "";
//                                    string strFalse = "";
//                                    Common.GetCheckTcikedUnTicked(_dtRecordTypleColumlns.Rows[m]["DropDownValues"].ToString(), ref strTrue, ref strFalse);
//                                    strShowHideJS2 = @"                                    
//                                   
//                                    var chk2 = document.getElementById('" + driverID.Substring(1, driverID.Length - 1) + @"');
//                                    var strDDDC2 ='';
//                                    if (chk2.checked == true) { strDDDC2 = '" + strTrue + @"'; }
//                                    if (chk2.checked == false) { strDDDC2 = '" + strFalse + @"'; }                                 
//                                   ";

//                                    strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";
//                                    strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";
//                                }
//                                else if (_ddlValue[m] != null || _ddlValue2[m] != null)
//                                {

//                                    strShowHideJS2 = @" var strDDDC2 = $('" + dirverGroupID + @"').val();";

//                                    strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";
//                                    strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";
//                                }
//                                else if (_lstValue[m] != null)
//                                {
//                                    if (bEquals2)
//                                    {
//                                        strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";

//                                        strShowHideJS2 = @"                                   
//                                       
//                                        var strDDDC2 = $('" + dirverGroupID + @"').val();
//                                        ";
//                                        strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";

//                                    }
//                                    else
//                                    {


//                                        strShowHideJS2 = @"                                  
//                                 
//                                        var bShow2=false;
//                                        var strHideValues2='" + strHideColumnValue2 + @"';
//                                        if(strHideValues2==null || strHideValues2=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array2=strHideValues2.split(',');
//                                        var strDDDC2 = $('" + dirverGroupID + @"').val();
//                                      
//                                        if(strDDDC2==null || strDDDC2=='' || strDDDC2=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC2=strDDDC2.toString();
//                                        var strDDDC_array2=strDDDC2.split(',');
//                                         for(var i = 0; i < strHideValues_array2.length; i++) {
//                                            
//                                                if(strHideValues_array2[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array2.length; j++) 
//                                                    {
//                                                         if(strDDDC_array2[j]!='')
//                                                        {
//                                                            if(strHideValues_array2[i]==strDDDC_array2[j])
//                                                                {
//                                                                bShow2=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          }";

//                                        strHideOperatorLogic2 = "bShow2";
//                                        strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";
//                                    }



//                                }
//                                else if (_cblValue[m] != null)
//                                {
//                                    if (bEquals2)
//                                    {
//                                        strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";

//                                        strShowHideJS2 = @"                                   
//                                        var chkBox = document.getElementById('" + driverID.Replace("#", "") + @"');
//                                         var options = chkBox.getElementsByTagName('input');
//                                         var listOfSpans = chkBox.getElementsByTagName('span');
//                                         var strDDDC2='';
//                                         for (var i = 0; i < options.length; i++) {
//                                             if (options[i].checked) {
//                                                 strDDDC2=listOfSpans[i].attributes['DataValue'].value;
//                                             }
//                                         }
//                                        //var strDDDC2 = $('" + dirverGroupID + @"').val();
//                                        ";
//                                        strShowHideEventJS2 = @"$('" + driverID + @"').click(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('click');  ";

//                                    }
//                                    else
//                                    {


//                                        strShowHideJS2 = @"                                  
//                                 
//                                        var bShow2=false;
//                                        var strHideValues2='" + strHideColumnValue2 + @"';
//                                        if(strHideValues2==null || strHideValues2=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array2=strHideValues2.split(',');
//                                        var strDDDC2 = $('" + dirverGroupID + @"').val();
//                                      
//                                        if(strDDDC2==null || strDDDC2=='' || strDDDC2=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC2=strDDDC2.toString();
//                                        var strDDDC_array2=strDDDC2.split(',');
//                                         for(var i = 0; i < strHideValues_array2.length; i++) {
//                                            
//                                                if(strHideValues_array2[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array2.length; j++) 
//                                                    {
//                                                         if(strDDDC_array2[j]!='')
//                                                        {
//                                                            if(strHideValues_array2[i]==strDDDC_array2[j])
//                                                                {
//                                                                bShow2=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          }";

//                                        strHideOperatorLogic2 = "bShow2";
//                                        strShowHideEventJS2 = @"$('" + driverID + @"').click(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('click');  ";
//                                    }



//                                }
//                                else
//                                {
//                                    if (bEquals2)
//                                    {
//                                        strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";

//                                    }
//                                    else
//                                    {
//                                        strHideOperatorLogic2 = "strDDDC2.indexOf('" + strHideColumnValue2 + "')>=0";
//                                    }

//                                    strShowHideJS2 = @"                                  
//                                   
//                                    var strDDDC2 = $('" + dirverGroupID + @"').val();
//                                   ";

//                                    strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";

//                                }


//                            }
//                        }
//                    }

//                    for (int m = 0; m < _dtRecordTypleColumlns.Rows.Count; m++)
//                    {
//                        if (dtShowWhen.Rows[0]["HideColumnID"].ToString()
//                            == _dtRecordTypleColumlns.Rows[m]["ColumnID"].ToString())
//                        {
//                            if (trX[m] != null)
//                            {
//                                trX[m].ID = "trX" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                string driverID = "#" + trX[m].ClientID.Substring(0, trX[m].ClientID.Length - 7);
//                                string targetID = driverID + trX[i].ID;
//                                string dirverGroupID = driverID;
//                                if (_ddlValue[m] != null && _ddlValue2[m] == null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_ddlValue[m] != null && _ddlValue2[m] != null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                if (_txtValue[m] != null)
//                                    driverID = driverID + "txt" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_chkValue[m] != null)
//                                    driverID = driverID + "chk" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_lstValue[m] != null)
//                                    driverID = driverID + "lst" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                if (_cblValue[m] != null)
//                                    driverID = driverID + "cbl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                dirverGroupID = driverID;

//                                string strHideShow = "";

//                                string strValidatorT = "";
//                                string strValidatorF = "";
//                                if (bool.Parse(_dtRecordTypleColumlns.Rows[i]["IsMandatory"].ToString())
//                        && _rfvValue[i] != null)
//                                {
//                                    strValidatorT = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), true)};";
//                                    strValidatorF = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";
//                                }


//                                if (_radioList[m] != null)
//                                {
//                                    driverID = driverID + "radio" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                    dirverGroupID = driverID;
//                                    driverID = driverID + "_";
//                                    strHideShow = strHideShow + "$('" + targetID + "').fadeOut(); " + strValidatorF;
//                                    string strDriverIDMain = driverID;

//                                    //var strDDDC =  document.querySelector('input[name=""" + dirverGroupID.Replace("#", "").Replace("_", "$") + @"""]:checked').value;" + strShowHideJS2 + @"

//                                    strHideShow = strHideShow + @"function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                            
//                                            var strDDDC =GetOptValue('" + dirverGroupID.Replace("#", "").Replace("_", "$") + @"');" + strShowHideJS2 + @"
//                                             if (strDDDC == '" + strHideColumnValue + @"' " + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                                $('" + targetID + @"').fadeIn(); " + strValidatorT + @"
//                                            }
//                                            else {
//                                                $('" + targetID + @"').fadeOut(); " + strValidatorF + @"
//                                            } } ";

//                                    for (int n = 0; n < _radioList[m].Items.Count; n++)
//                                    {

//                                        driverID = strDriverIDMain + n.ToString();
//                                        strHideShow = strHideShow + @"                                           
//                                           
//                                          $('" + driverID + @"').change(function (e) {
//                                            ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                        }); " + strShowHideEventJS2;

//                                        if (_radioList[m].SelectedIndex == n)
//                                        {
//                                            strHideShow = strHideShow + "$('" + driverID + "').trigger('change');";
//                                        }


//                                    }

//                                }
//                                else if (_chkValue[m] != null)
//                                {

//                                    string strTrue = "";
//                                    string strFalse = "";
//                                    Common.GetCheckTcikedUnTicked(_dtRecordTypleColumlns.Rows[m]["DropDownValues"].ToString(), ref strTrue, ref strFalse);
//                                    strHideShow = @"                                    
//                                     function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                    var chk = document.getElementById('" + driverID.Substring(1, driverID.Length - 1) + @"');
//                                    var strDDDC ='';
//                                    if (chk.checked == true) { strDDDC = '" + strTrue + @"'; }
//                                    if (chk.checked == false) { strDDDC = '" + strFalse + @"'; }   " + strShowHideJS2 + @"                              
//                                    if (strDDDC == '" + strHideColumnValue + @"' " + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }}
//                                 $('" + driverID + @"').change(function (e) {
//                                      ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                });
//                                $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2;
//                                }
//                                else if (_ddlValue[m] != null || _ddlValue2[m] != null)
//                                {

//                                    strHideShow = @"                                    
//                                    function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                    var strDDDC = $('" + dirverGroupID + @"').val();  " + strShowHideJS2 + @"    
//                                     if (strDDDC == '" + strHideColumnValue + @"' " + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }}
//
//                                 $('" + driverID + @"').change(function (e) {
//                                      ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                });
//                                $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2; ;
//                                }
//                                else if (_lstValue[m] != null)
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                        strHideShow = @"                                    
//                                        function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                        var strDDDC = $('" + dirverGroupID + @"').val();  " + strShowHideJS2 + @"               
//                                        if (" + strHideOperatorLogic + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                            $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }}
//                                     $('" + driverID + @"').change(function (e) {
//                                               ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                    });
//                                    $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2;

//                                    }
//                                    else
//                                    {


//                                        strHideShow = @"                                    
//                                  
//                                     function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        var bShow=false;
//                                        var strHideValues='" + strHideColumnValue + @"';
//                                        if(strHideValues==null || strHideValues=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array=strHideValues.split(',');
//                                        var strDDDC = $('" + dirverGroupID + @"').val();
//                                      
//                                        if(strDDDC==null || strDDDC=='' || strDDDC=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC=strDDDC.toString();
//                                        var strDDDC_array=strDDDC.split(',');
//                                         for(var i = 0; i < strHideValues_array.length; i++) {
//                                            
//                                                if(strHideValues_array[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array.length; j++) 
//                                                    {
//                                                         if(strDDDC_array[j]!='')
//                                                        {
//                                                            if(strHideValues_array[i]==strDDDC_array[j])
//                                                                {
//                                                                bShow=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          } " + strShowHideJS2 + @" 
//
//                                        if ( bShow==true" + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                            $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//                                        }
//                                        $('" + driverID + @"').change(function (e) {
//                                              ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                        });
//                                    $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2;
//                                    }



//                                }
//                                else if (_cblValue[m] != null)
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                        strHideShow = @"                                    
//                                        function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                        //var strDDDC = $('" + dirverGroupID + @"').val(); 
//                                        var chkBox = document.getElementById('" + driverID.Replace("#", "") + @"');
//                                         var options = chkBox.getElementsByTagName('input');
//                                         var listOfSpans = chkBox.getElementsByTagName('span');
//                                         var strDDDC='';
//                                         for (var i = 0; i < options.length; i++) {
//                                             if (options[i].checked) {
//                                                 strDDDC=listOfSpans[i].attributes['DataValue'].value;
//                                             }
//                                         }
//
//
//                                        " + strShowHideJS2 + @"               
//                                        if (" + strHideOperatorLogic + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                              $('" + targetID + @"').stop(true,true);$('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }}
//                                     $('" + driverID + @"').click(function (e) {
//                                               ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                    });
//                                    $('" + driverID + @"').trigger('click');  " + strShowHideEventJS2;

//                                    }
//                                    else
//                                    {


//                                        strHideShow = @"                                    
//                                  
//                                     function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        var bShow=false;
//                                        var strHideValues='" + strHideColumnValue + @"';
//                                        if(strHideValues==null || strHideValues=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array=strHideValues.split(',');
//                                        //var strDDDC = $('" + dirverGroupID + @"').val();
//                                       var chkBox = document.getElementById('" + driverID.Replace("#", "") + @"');
//                                         var options = chkBox.getElementsByTagName('input');
//                                         var listOfSpans = chkBox.getElementsByTagName('span');
//                                        var strDDDC='';
//                                         for (var i = 0; i < options.length; i++) {
//                                             if (options[i].checked) {
//                                                 strDDDC=strDDDC+ ',' + listOfSpans[i].attributes['DataValue'].value;
//                                             }
//                                         }  
//                                        if(strDDDC==null || strDDDC=='' || strDDDC=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC=strDDDC.toString();
//                                        var strDDDC_array=strDDDC.split(',');
//                                         for(var i = 0; i < strHideValues_array.length; i++) {
//                                            
//                                                if(strHideValues_array[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array.length; j++) 
//                                                    {
//                                                         if(strDDDC_array[j]!='')
//                                                        {
//                                                            if(strHideValues_array[i]==strDDDC_array[j])
//                                                                {
//                                                                bShow=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          } " + strShowHideJS2 + @" 
//
//                                        if ( bShow==true" + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                              $('" + targetID + @"').stop(true,true);$('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//                                        }
//                                        $('" + driverID + @"').click(function (e) {
//                                              ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                        });
//                                    $('" + driverID + @"').trigger('click');  " + strShowHideEventJS2;
//                                    }



//                                }
//                                else
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                    }
//                                    else
//                                    {
//                                        strHideOperatorLogic = "strDDDC.indexOf('" + strHideColumnValue + "')>=0";
//                                    }

//                                    strHideShow = @"                                    
//                                   function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                    var strDDDC = $('" + dirverGroupID + @"').val(); " + strShowHideJS2 + @"   
//                                    if (" + strHideOperatorLogic + strJoinOperator + strHideOperatorLogic2 + @") {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }}
//
//                                  $('" + driverID + @"').change(function (e) {
//                                      ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                });
//                                $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2;

//                                }
//                                //if (!IsPostBack)
//                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHide2" + _strDynamictabPart + i.ToString(), strHideShow, true);

//                            }
//                        }
//                    }

//                }

//            }

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

    //protected string GetListValues(ListBox lb)
    //{
    //    string strSelectedValues = "";

    //    foreach (ListItem item in lb.Items)
    //    {
    //        if (item.Selected)
    //        {
    //            strSelectedValues = strSelectedValues + item.Value + ",";
    //        }
    //    }

    //    if (strSelectedValues != "")
    //        strSelectedValues = strSelectedValues.Substring(0, strSelectedValues.Length - 1);
    //    return strSelectedValues;
    //}

    //protected void PutDDLValues(string strDropdownValues, ref  DropDownList ddl)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    foreach (string s in result)
    //    {
    //        ListItem liTemp = new ListItem(s, s);
    //        ddl.Items.Add(liTemp);
    //    }

    //    ListItem liSelect = new ListItem("--Please Select--", "");
    //    ddl.Items.Insert(0, liSelect);

    //}



    //protected void PutDDLValue_Text(string strDropdownValues, ref  DropDownList ddl)
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
    //                ListItem liTemp = new ListItem(strText, strValue);
    //                ddl.Items.Add(liTemp);
    //            }
    //        }
    //    }

    //    ListItem liSelect = new ListItem("--Please Select--", "");
    //    ddl.Items.Insert(0, liSelect);

    //}


    //protected void PutRadioListValue_Text(string strDropdownValues, ref  RadioButtonList rl)
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
    //                ListItem liTemp = new ListItem(strText + "&nbsp;&nbsp;", strValue);
    //                rl.Items.Add(liTemp);
    //            }
    //        }
    //    }


    //}


    //protected void PutListValues_Text(string strDropdownValues, ref  ListBox lb)
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
    //                ListItem liTemp = new ListItem(strText, strValue);
    //                lb.Items.Add(liTemp);
    //            }
    //        }
    //    }


    //}

    //protected void PutRadioListValue_Image(string strDropdownValues, ref  RadioButtonList rl)
    //{
    //    //string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    OptionImageList theOptionImageList = JSONField.GetTypedObject<OptionImageList>(strDropdownValues);

    //    foreach (OptionImage aOptionImage in theOptionImageList.ImageList)
    //    {
    //        ListItem liTemp = new ListItem("<img src='" + _strFilesLocation + "/UserFiles/AppFiles/" + aOptionImage.UniqueFileName + "' title='"+aOptionImage.Value+"' />" + "&nbsp;&nbsp;", aOptionImage.Value);
    //        rl.Items.Add(liTemp);
    //    }




    //}


    //protected void SetListValues_ForTable(string strDBValues, ref  ListBox lb, int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn)
    //{
    //    if (strDBValues != "")
    //    {
    //        //it's a new dev so iLinkedParentColumnID must be RecordID

    //        DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable, null, null);


    //        lb.Items.Clear();

    //        string[] strSS = strDBValues.Split(',');



    //        foreach (DataRow dr in dtParents.Rows)
    //        {

    //            foreach (string SS in strSS)
    //            {
    //                if (SS == dr[0].ToString())
    //                {
    //                    ListItem liTemp = new ListItem(dr[1].ToString(), dr[0].ToString());
    //                    lb.Items.Add(liTemp);
    //                }
    //            }


    //        }

    //        foreach (DataRow dr in dtParents.Rows)
    //        {
    //            if (lb.Items.FindByValue(dr[0].ToString()) == null)
    //            {
    //                ListItem liTemp = new ListItem(dr[1].ToString(), dr[0].ToString());
    //                lb.Items.Add(liTemp);
    //            }
    //        }
    //        foreach (string SS in strSS)
    //        {
    //            try
    //            {
    //                if (SS != "")
    //                    lb.Items.FindByValue(SS).Selected = true;
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }



    //    }

    //}


    //protected void SetListValues_Text(string strDBValues, ref  ListBox lb, string strDropdownValues)
    //{
    //    if (strDBValues != "")
    //    {
    //        lb.Items.Clear();

    //        string[] strSS = strDBValues.Split(',');

    //        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //        string strValue = "";
    //        string strText = "";
    //        foreach (string s in result)
    //        {
    //            strValue = "";
    //            strText = "";
    //            if (s.IndexOf(",") > -1)
    //            {
    //                strValue = s.Substring(0, s.IndexOf(","));
    //                strText = s.Substring(strValue.Length + 1);
    //            }

    //            foreach (string SS in strSS)
    //            {
    //                if (SS == strValue)
    //                {
    //                    ListItem liTemp = new ListItem(strText, strValue);
    //                    lb.Items.Add(liTemp);
    //                }
    //            }


    //        }

    //        foreach (string s in result)
    //        {
    //            strValue = "";
    //            strText = "";

    //            if (s.IndexOf(",") > -1)
    //            {
    //                strValue = s.Substring(0, s.IndexOf(","));
    //                strText = s.Substring(strValue.Length + 1);
    //            }

    //            if (lb.Items.FindByValue(strValue) == null)
    //            {
    //                ListItem liTemp = new ListItem(strText, strValue);
    //                lb.Items.Add(liTemp);
    //            }
    //        }
    //        foreach (string SS in strSS)
    //        {
    //            try
    //            {
    //                if (SS != "")
    //                    lb.Items.FindByValue(SS).Selected = true;
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }



    //    }

    //}

    //protected void SetListValues(string strDBValues, ref  ListBox lb, string strDropdownValues)
    //{
    //    if (strDBValues != "")
    //    {
    //        lb.Items.Clear();

    //        string[] strSS = strDBValues.Split(',');

    //        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //        foreach (string s in result)
    //        {
    //            foreach (string SS in strSS)
    //            {
    //                if (SS == s)
    //                {
    //                    ListItem liTemp = new ListItem(s, s);
    //                    lb.Items.Add(liTemp);
    //                }
    //            }

    //        }

    //        foreach (string s in result)
    //        {
    //            if (lb.Items.FindByValue(s) == null)
    //            {
    //                ListItem liTemp = new ListItem(s, s);
    //                lb.Items.Add(liTemp);
    //            }

    //        }

    //        foreach (string SS in strSS)
    //        {
    //            try
    //            {
    //                if (SS != "")
    //                    lb.Items.FindByValue(SS).Selected = true;
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }
    //    }

    //}

    //protected void SetListValues(string strDBValues, ref  ListBox lb)
    //{
    //    if (strDBValues != "")
    //    {
    //        string[] strSS = strDBValues.Split(',');
    //        foreach (string SS in strSS)
    //        {
    //            try
    //            {
    //                if (SS != "")
    //                    lb.Items.FindByValue(SS).Selected = true;
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }
    //    }

    //}

    //protected void GetCheckTcikedUnTicked(string strDropdownValues, ref string strTrue, ref string strFalse)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    int i = 0;
    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        if (i == 0)
    //        {
    //            strTrue = s;
    //        }
    //        else if (i == 1)
    //        {
    //            strFalse = s;
    //        }
    //        i = i + 1;
    //    }

    //}

    //protected void PutCheckBoxDefault(string strDropdownValues, ref  CheckBox chk)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    int i = 0;
    //    foreach (string s in result)
    //    {
    //        if (i == 2)
    //        {
    //            if (s.ToLower() == "yes")
    //            {
    //                chk.Checked = true;
    //            }
    //        }
    //        i = i + 1;
    //    }


    //}

    //protected void SetCheckBoxValue(string strDropdownValues, string strValue, ref  CheckBox chk)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    int i = 0;
    //    foreach (string s in result)
    //    {
    //        if (i == 0)
    //        {
    //            if (s.ToLower() == strValue.ToLower())
    //            {
    //                chk.Checked = true;
    //            }
    //        }
    //        if (i == 1)
    //        {
    //            if (s.ToLower() == strValue.ToLower())
    //            {
    //                chk.Checked = false;
    //            }
    //        }
    //        i = i + 1;
    //    }


    //}

    //protected string GetCheckBoxValue(string strDropdownValues, ref  CheckBox chk)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    int i = 0;
    //    foreach (string s in result)
    //    {
    //        if (i == 0)
    //        {
    //            if (chk.Checked)
    //            {
    //                return s;
    //            }
    //        }
    //        if (i == 1)
    //        {
    //            if (chk.Checked == false)
    //            {
    //                return s;
    //            }
    //        }
    //        i = i + 1;
    //    }
    //    return "";
    //}

    //protected void PutListValues(string strDropdownValues, ref  ListBox lb)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    foreach (string s in result)
    //    {
    //        ListItem liTemp = new ListItem(s, s);
    //        lb.Items.Add(liTemp);
    //    }


    //}

    //protected void PutRadioList(string strDropdownValues, ref  RadioButtonList rl)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        ListItem liTemp = new ListItem(s + "&nbsp;&nbsp;", s);
    //        rl.Items.Add(liTemp);
    //    }

    //}



    protected void Page_Load(object sender, EventArgs e)
    {





        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Child - Load - Start ";
            theSpeedLog.FunctionLineNumber = 4968;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }
        




        _strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.RawUrl;

        if (!IsPostBack)
        {

            if (_bTableTabYes)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHideMainDivsOnlyOne"+_theTable.TableID.ToString(), "ShowHideMainDivs"+_theTable.TableID.ToString()+"('" + pnlDetailTab.ClientID + "',this,0);", true);
            }

//            if (Request.QueryString["quickdone"] != null && Request.QueryString["controlvalue"] != null)
//            {
//                try
//                {
//                    SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["quickdone"].ToString())));
//                    if (theSearchCriteria != null)
//                    {
//                        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

//                        xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));
//                        string strControl = xmlDoc.FirstChild["control"].InnerText;
//                        string strControlValue = Cryptography.Decrypt(Request.QueryString["controlvalue"].ToString());

//                        string strDisplayColumn = xmlDoc.FirstChild["DisplayColumn"].InnerText;
//                        string strTableTableID = xmlDoc.FirstChild["TableTableID"].InnerText;
//                        string strLinkedParentColumnID = xmlDoc.FirstChild["LinkedParentColumnID"].InnerText;
//                        string strDropDownType = xmlDoc.FirstChild["DropDownType"].InnerText;




//                        if (strDropDownType == "table" )
//                        {
//                            string str_hfValue = xmlDoc.FirstChild["_hfValue"].InnerText;
//                            TextBox txtP = (TextBox)pnlDetail.FindControl(strControl);
//                            HiddenField _hfValue = (HiddenField)pnlDetail.FindControl(str_hfValue);


//                            try
//                            {
//                                //int iTableRecordID = int.Parse(_dtRecordedetail.Rows[0][i].ToString());

//                                Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(strLinkedParentColumnID));

//                                Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strControlValue));
//                                string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);


//                                //Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(iTableRecordID);
//                                //string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);




//                                _hfValue.Value = strLinkedColumnValue;
//                                DataTable dtTableTableSC = Common.DataTableFromText(@"SELECT SystemName,DisplayName 
//                                FROM [Column] WHERE   TableID ="
//                           + strTableTableID);

//                                //string strDisplayColumn = _dtRecordTypleColumlns.Rows[i]["DisplayColumn"].ToString();

//                                foreach (DataRow dr in dtTableTableSC.Rows)
//                                {
//                                    strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

//                                }

//                                string sstrDisplayColumnOrg = strDisplayColumn;
//                                string strFilterSQL = "";
//                                if (theLinkedColumn.SystemName.ToLower() == "recordid")
//                                {
//                                    strFilterSQL = strLinkedColumnValue;
//                                }
//                                else
//                                {
//                                    strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
//                                }



//                                //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + _dtRecordedetail.Rows[0][i].ToString());

//                                DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

//                                if (dtTheRecord.Rows.Count > 0)
//                                {

//                                    foreach (DataColumn dc in dtTheRecord.Columns)
//                                    {

//                                        //strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());

//                                        Column theColumn = RecordManager.ets_Column_Details_By_Sys((int)theLinkedColumn.TableID, dc.ColumnName);
//                                        if (theColumn != null)
//                                        {
//                                            if (theColumn.ColumnType == "date")
//                                            {
//                                                string strDatePartOnly = dtTheRecord.Rows[0][dc.ColumnName].ToString();

//                                                if (strDatePartOnly.Length > 9)
//                                                {
//                                                    strDatePartOnly = strDatePartOnly.Substring(0, 10);
//                                                }

//                                                strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", strDatePartOnly);
//                                            }
//                                            else
//                                            {
//                                                strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
//                                            }
//                                        }

//                                    }
//                                }
//                                if (sstrDisplayColumnOrg != strDisplayColumn)
//                                    txtP.Text = strDisplayColumn;



//                            }
//                            catch
//                            {
//                                //
//                            }



//                        }
//                        else
//                        {

//                            Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(strLinkedParentColumnID));

//                            Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strControlValue));
//                            string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);


//                            DropDownList ddlP = (DropDownList)divDynamic.FindControl(strControl);
//                            ddlP.SelectedValue = strLinkedColumnValue;
//                        }
//                    }
//                }
//                catch
//                {

//                }

//            }

         

        }
        else
        {
            //POSTBACK

            if (_bTableTabYes)
            {
                
            }
        }

        // checking action mode
        if (Mode == null)
        {
            Server.Transfer("~/Empty.aspx");
        }
        else
        {


            if (Mode == "add" ||
                Mode == "view" ||
                Mode == "edit")
            {


                if (RecordID != null)
                {

                    //_qsRecordID = Cryptography.Decrypt(Request.QueryString["RecordID"]);
                    //RecordID = int.Parse(_qsRecordID);

                    if (!IsPostBack)
                    {

                        if (_theTable.ReasonChangeType == "mandatory")
                        {
                            if (Mode != "view")
                            {
                                rfvReasonForChange.Enabled = true;
                            }
                            stgReasonForChange.InnerText = "Reason for change*";
                            stgReasonForChange.Style.Add("color", "red");
                        }

                        if (_theTable.ReasonChangeType == "none" || _theTable.ReasonChangeType == "")
                        {
                            trReasonForChange.Visible = false;
                        }                       

                    }



                }
                else
                {



                    if (_strRecordRightID == Common.UserRoleType.OwnData || _strRecordRightID == Common.UserRoleType.ReadOnly
                        || _strRecordRightID == Common.UserRoleType.None)
                    {
                        Response.Redirect("~/Empty.aspx", true);
                        return;
                    }


                }

            }
            else
            {
                Server.Transfer("~/Empty.aspx");
            }


        }


        string strTitle = _theTable.TableName + Mode;

        if (!IsPostBack)
        {
            if (_strJS != "")
            {
                if (Mode.ToLower() != "view")
                {
                    //if (!IsPostBack)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_strJS" + _strDynamictabPart, _strJS, true);
                }
            }

            PopulateTable();

        }

        if (Mode.ToLower() != "view" && _strJSPostBack != "")
        {
            //if (!IsPostBack)
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_strJSPostBack" + _strDynamictabPart, _strJSPostBack, true);

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

            //hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&Recordid=" + Request.QueryString["ParentRecordID"].ToString() + "&TableID=" + Request.QueryString["ParentTableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();



            if (_bPrivate == false)
            {
                if (!IsPostBack)
                {
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Public.aspx?TableID=" + TableID.ToString();
                }
            }

            //if (Request.QueryString["quickadd"] != null)
            //{
            //    try
            //    {
            //        SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["quickadd"].ToString())));
            //        if (theSearchCriteria != null)
            //        {
            //            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            //            xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

            //            if (xmlDoc.FirstChild["RecordID"].InnerText == "-1")
            //            {
            //                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode="
            //                    + xmlDoc.FirstChild["mode"].InnerText + "&TableID=" + xmlDoc.FirstChild["TableID"].InnerText
            //                    + "&SearchCriteriaID=" + xmlDoc.FirstChild["SearchCriteriaID"].InnerText;
            //            }
            //            else
            //            {
            //                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode="
            //                  + xmlDoc.FirstChild["mode"].InnerText + "&TableID=" + xmlDoc.FirstChild["TableID"].InnerText
            //                  + "&SearchCriteriaID=" + xmlDoc.FirstChild["SearchCriteriaID"].InnerText + "&RecordID=" + xmlDoc.FirstChild["RecordID"].InnerText;
            //            }
            //        }
            //    }
            //    catch
            //    {

            //    }


            //}
        }



        switch (Mode.ToLower())
        {
            case "add":

                strTitle = "Add " + _theTable.TableName;
                break;

            case "view":

                strTitle = "View " + _theTable.TableName;
                //if (!IsPostBack)
                //{

                //    //added and updated part
                //    if (RecordID != null)
                //    {
                //        AddEditInfo();
                //    }

                //}

                EnableTheRecordControls(false);
                //divSave.Visible = false;
                divSaveClose.Visible = false;


                break;

            case "edit":

                strTitle = "Edit " + _theTable.TableName;
                if (!IsPostBack)
                {
                    try
                    {
                        Record theRecord = RecordManager.ets_Record_Detail_Full((int)RecordID);
                    }
                    catch
                    {

                    }
                    //txtReasonForChange.Text = theRecord.ChangeReason;
                }

                break;


            default:
                //?

                break;
        }


        //lblTitle.Text = strTitle;

        string strFancy = @"$(function () {
            $("".popuplink"").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 600,
                height: 650,
                titleShow: false
            });
        });";
        //if (!IsPostBack)
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "FancyBox" + _strDynamictabPart, strFancy, true);


        //if (Request.QueryString["onlyback"] != null)
        //{

        //    divEdit.Visible = false;
        //    hlEdit.Visible = false;

        //}


        if (IsPostBack)
        {
            //if (Request.QueryString["RecordID"] != null)
            //{
            try
            {
                for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                {
                    if (_hfValue2[i] != null && _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file")
                    {
                        if (_hfValue2[i].Value != "")
                        {
                            //if (!IsPostBack)
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "UserBasicPostFile" + _strDynamictabPart + i.ToString(), "UserBasic" + _strDynamictabPart + i.ToString() + @"();", true);

                            if (_rfvValue[i] != null)
                            {
                                //if (!IsPostBack)
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "UserBasicPostFileV" + _strDynamictabPart + i.ToString(), "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};", true);
                            }
                        }
                    }
                    if (_hfValue2[i] != null && _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                    {
                        if (_hfValue2[i].Value != "")
                        {
                            //if (!IsPostBack)
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "UserBasicPostImage" + _strDynamictabPart + i.ToString(), "UserBasic" + _strDynamictabPart + i.ToString() + @"();", true);

                            if (_rfvValue[i] != null)
                            {
                                //if (!IsPostBack)
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "UserBasicPostImageV" + _strDynamictabPart + i.ToString(), "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};", true);
                            }
                        }
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file" &&
                        _hfValue[i] != null && Mode.ToLower() != "view"
                        && _hfValue2[i] != null)
                    {
                        if (_hfValue[i].Value != "" && _hfValue2[i].Value == "")
                        {
                            //_lblValue[i].Text = "<a target='_blank' href='" + _strFilesLocation + "/UserFiles/AppFiles/"
                            //       + _hfValue[i].Value + "'>" +
                            //       _hfValue[i].Value.Substring(37) + "</a>";


                            string strFilePath = Cryptography.Encrypt(_strFilesLocation + "/UserFiles/AppFiles/" + _hfValue[i].Value);
                            string strFileName = Cryptography.Encrypt(_hfValue[i].Value.Substring(37));

                            _lblValue[i].Text = "<a target='_blank' href='" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Filedownload.aspx?FilePath="
                                + strFilePath + "&FileName=" + strFileName + "'>" +
                                  _hfValue[i].Value.Substring(37) + "</a>";


                            _lblValue[i].Text = "<img  title=\"Remove this file\" style=\"cursor:pointer;\"  id=\"dimg" + _strDynamictabPart + _hfValue[i].ID + "\" src=\"" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />" + _lblValue[i].Text;

                            string strTempJS = @" if(document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"')!=null){  document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value='';
                                                      $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html(''); 
                                            });};";

                            //if (!IsPostBack)
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "filedelete2" + _strDynamictabPart + i.ToString(), strTempJS, true);


                        }
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image" &&
                        _hfValue[i] != null && Mode.ToLower() != "view"
                        && _hfValue2[i] != null)
                    {
                        if (_hfValue[i].Value != "" && _hfValue2[i].Value == "")
                        {
                            string strMaxHeight = "50";
                            if (_dtColumnsDetail.Rows[i]["TextHeight"] != DBNull.Value)
                            {
                                strMaxHeight = _dtColumnsDetail.Rows[i]["TextHeight"].ToString();
                            }

                            string strFilePath = _strFilesLocation + "/UserFiles/AppFiles/"
                                    + _hfValue[i].Value;
                            _lblValue[i].Text = "<a target='_blank' href='" + strFilePath + "'>"
                                + "<img style='padding-bottom:7px; max-height:" + strMaxHeight + "px;' alt='" + _hfValue[i].Value.Substring(37)
                                + "' src='" + strFilePath + "' title='" + _hfValue[i].Value.Substring(37) + "'  />" + "</a><br/>";


                            _lblValue[i].Text = "<img title=\"Remove this image\" style=\"cursor:pointer;\"  id=\"dimg" + _strDynamictabPart + _hfValue[i].ID + "\" src=\"" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />" + _lblValue[i].Text;


                            string strTempJS = @" if(document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"')!=null){ document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value='';
                                                      $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html(''); 
                                            });};";

                            //if (!IsPostBack)
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "imagedelete2" + _strDynamictabPart + i.ToString(), strTempJS, true);


                        }
                    }

                }
            }
            catch
            {
                //
            }
            //}

        }





        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Child- Load - End ";
            theSpeedLog.FunctionLineNumber = 5501;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }
        




    }
    protected void AddEditInfo()
    {
        try
        {
            
            //Record theRecord = RecordManager.ets_Record_Detail_Full((int)RecordID);
            User userAdded = SecurityManager.User_Details((int)_qsRecord.EnteredBy);
            _lblAddedTimeEmail.Text = _qsRecord.DateAdded.ToString() + "   By " + userAdded.Email;
            //txtReasonForChange.Text = theRecord.ChangeReason;
            if (_qsRecord.LastUpdatedUserID != null)
            {
                User userUpdated = SecurityManager.User_Details((int)_qsRecord.LastUpdatedUserID);
                _lblUpdatedTimeEmail.Text = _qsRecord.DateUpdated.ToString() + "   By " + userUpdated.Email;
            }
        }
        catch
        {

        }
    }
    protected void PopulateTable()
    {
        //int iTemp = 0;
        if (_iTableIndex >= 0)
        {
            //Table theTable = RecordManager.ets_Table_Details(int.Parse(TableID.ToString()));
            _txtValue[_iTableIndex].Text = _theTable.TableName;
        }
    }


    //protected string GetCheckBoxListValues(CheckBoxList lb)
    //{
    //    string strSelectedValues = "";

    //    foreach (ListItem item in lb.Items)
    //    {
    //        if (item.Selected)
    //        {
    //            strSelectedValues = strSelectedValues + item.Value + ",";
    //        }
    //    }

    //    if (strSelectedValues != "")
    //        strSelectedValues = strSelectedValues.Substring(0, strSelectedValues.Length - 1);
    //    return strSelectedValues;
    //}


    //protected void SetCheckBoxListValues_ForTable(string strDBValues, ref  CheckBoxList lb, int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn)
    //{


    //    if (strDBValues != "")
    //    {
    //        //it's a new dev so iLinkedParentColumnID must be RecordID

    //        DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable, null, null);


    //        lb.Items.Clear();

    //        string[] strSS = strDBValues.Split(',');

    //        foreach (DataRow dr in dtParents.Rows)
    //        {

    //            foreach (string SS in strSS)
    //            {
    //                if (SS == dr[0].ToString())
    //                {
    //                    ListItem liTemp = new ListItem(dr[1].ToString(), dr[0].ToString());
    //                    lb.Items.Add(liTemp);
    //                }
    //            }


    //        }



    //        foreach (DataRow dr in dtParents.Rows)
    //        {




    //            if (lb.Items.FindByValue(dr[0].ToString()) == null)
    //            {
    //                ListItem liTemp = new ListItem(dr[1].ToString(), dr[0].ToString());
    //                lb.Items.Add(liTemp);
    //            }

    //        }

    //        foreach (string SS in strSS)
    //        {
    //            try
    //            {
    //                if (SS != "")
    //                    lb.Items.FindByValue(SS).Selected = true;
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }

    //        foreach (ListItem li in lb.Items)
    //        {
    //            li.Attributes.Add("DataValue", li.Value);
    //        }
    //    }

    //}

    //protected void PutCheckBoxListValues_Text(string strDropdownValues, ref  CheckBoxList lb)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    lb.Items.Clear();
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
    //                lb.Items.Add(liTemp);
    //            }
    //        }
    //    }


    //}

    //protected void PutCheckBoxListValues(string strDropdownValues, ref  CheckBoxList lb)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    lb.Items.Clear();
    //    foreach (string s in result)
    //    {
    //        ListItem liTemp = new ListItem(s, s);
    //        lb.Items.Add(liTemp);
    //    }

    //}

    //protected void SetCheckBoxListValues(string strDBValues, ref  CheckBoxList lb, string strDropdownValues)
    //{


    //    if (strDBValues != "")
    //    {

    //        lb.Items.Clear();


    //        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);


    //        string[] strSS = strDBValues.Split(',');

    //        foreach (string s in result)
    //        {

    //            foreach (string SS in strSS)
    //            {
    //                if (SS == s)
    //                {
    //                    ListItem liTemp = new ListItem(s, s);
    //                    lb.Items.Add(liTemp);
    //                }
    //            }
    //        }

    //        foreach (string s in result)
    //        {

    //            if (lb.Items.FindByValue(s) == null)
    //            {
    //                ListItem liTemp = new ListItem(s, s);
    //                lb.Items.Add(liTemp);
    //            }

    //        }

    //        foreach (string SS in strSS)
    //        {
    //            try
    //            {
    //                if (SS != "")
    //                    lb.Items.FindByValue(SS).Selected = true;
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }

    //        foreach (ListItem li in lb.Items)
    //        {
    //            li.Attributes.Add("DataValue", li.Value);
    //        }
    //    }

    //}


    //protected void SetCheckBoxListValues_Text(string strDBValues, ref  CheckBoxList lb, string strDropdownValues)
    //{


    //    if (strDBValues != "")
    //    {

    //        lb.Items.Clear();


    //        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //        string strValue = "";
    //        string strText = "";

    //        string[] strSS = strDBValues.Split(',');

    //        foreach (string s in result)
    //        {
    //            strValue = "";
    //            strText = "";

    //            if (s.IndexOf(",") > -1)
    //            {
    //                strValue = s.Substring(0, s.IndexOf(","));
    //                strText = s.Substring(strValue.Length + 1);
    //            }

    //            foreach (string SS in strSS)
    //            {
    //                if (SS == strValue)
    //                {
    //                    ListItem liTemp = new ListItem(strText, strValue);
    //                    lb.Items.Add(liTemp);
    //                }
    //            }
    //        }

    //        foreach (string s in result)
    //        {


    //            strValue = "";
    //            strText = "";

    //            if (s.IndexOf(",") > -1)
    //            {
    //                strValue = s.Substring(0, s.IndexOf(","));
    //                strText = s.Substring(strValue.Length + 1);
    //            }

    //            if (lb.Items.FindByValue(strValue) == null)
    //            {
    //                ListItem liTemp = new ListItem(strText, strValue);
    //                lb.Items.Add(liTemp);
    //            }

    //        }

    //        foreach (string SS in strSS)
    //        {
    //            try
    //            {
    //                if (SS != "")
    //                    lb.Items.FindByValue(SS).Selected = true;
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }

    //        foreach (ListItem li in lb.Items)
    //        {
    //            li.Attributes.Add("DataValue", li.Value);
    //        }
    //    }

    //}

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
            if(_ddlValue[i]!=null)
            {
                if (_ddlValue[i].Items.Count > 0)
                    _ddlValue[i].ClearSelection();
            }

            if (_ddlValue2[i] != null)
            {
                if (_ddlValue2[i].Items.Count > 0)
                    _ddlValue2[i].ClearSelection();
            }

            if(_ccddl[i]!=null)
            {
                _ccddl[i].SelectedValue = null;
            }
            

            if(_radioList[i]!=null)
            {
                _radioList[i].ClearSelection();
            }

            if(_chkValue[i]!=null)
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
    public void PopulateRecord()
    {

        //_qsRecordID = Cryptography.Decrypt(Request.QueryString["RecordID"]);



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

        if (_strChildSession!="")
        {
            Session[_strChildSession] = RecordID.ToString();
        }

        //RecordID = int.Parse(_qsRecordID);
        _dtRecordedetail = RecordManager.ets_Record_Details((int)RecordID).Tables[1];
        _qsRecord = RecordManager.ets_Record_Detail_Full((int)RecordID);
        if (Mode.ToLower()=="view")
            AddEditInfo();

        if (_theTable.ReasonChangeType == "none" || _theTable.ReasonChangeType == "")
        {
            trReasonForChange.Visible = false;
        }   
        else
        {
            trReasonForChange.Visible=true;
        }

        if (_strClickFirstTab!="")
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strTableTabsJSFirstTab" + _strDynamictabPart, _strClickFirstTab, true);

        if (_strRecordRightID == Common.UserRoleType.OwnData)
        {

            hlEdit.Visible = false;


            if (_qsRecord != null)
            {
                if (_qsRecord.OwnerUserID.ToString() == _objUser.UserID.ToString())
                {
                    hlEdit.Visible = true;
                }
            }

        }

        if (_strRecordRightID == Common.UserRoleType.EditOwnViewOther)
        {

            hlEdit.Visible = false;

            if (_qsRecord != null)
            {
                if (_qsRecord.EnteredBy.ToString() == _objUser.UserID.ToString())
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
                    if (strWarning.IndexOf( WarningMsg.MaxtimebetweenRecords ) >= 0)
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
                        string strCheckValue = RecordManager.GetRecordValue(ref _qsRecord, theCheckColumn.SystemName);
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
                        string strCheckValue = RecordManager.GetRecordValue(ref _qsRecord, theCheckColumn.SystemName);
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
                        string strCheckValue = RecordManager.GetRecordValue(ref _qsRecord, theCheckColumn.SystemName);
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

                    _lblValue[i].Text = "<a target='_blank' href='" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Filedownload.aspx?FilePath="
                        + strFilePath + "&FileName=" + strFileName + "'>" +
                          _dtRecordedetail.Rows[0][i].ToString().Substring(37) + "</a>";

                    if (Mode.ToLower() == "view")
                    {
                        _fuValue[i].Visible = false;
                    }
                    else
                    {
                        _hfValue[i].Value = _dtRecordedetail.Rows[0][i].ToString();

                        _lblValue[i].Text = "<img  title=\"Remove this file\" style=\"cursor:pointer;\"  id=\"dimg" + _strDynamictabPart + _hfValue[i].ID + "\" src=\"" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                           + "/App_Themes/Default/Images/icon_delete.gif\" />" + _lblValue[i].Text;

                        string strTempJS = @" if(document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"')!=null){  document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value='';
                                                      $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html(''); 
                                            });};";

                        //if (!IsPostBack)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "filedelete" + _strDynamictabPart + i.ToString(), strTempJS, true);
                    }
                }

                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "trafficlight"
                        && _dtColumnsDetail.Rows[i]["TrafficLightColumnID"] != DBNull.Value
                        && _dtColumnsDetail.Rows[i]["TrafficLightValues"] != DBNull.Value)
                {
                    Column theTrafficLightColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["TrafficLightColumnID"].ToString()));
                    if (theTrafficLightColumn != null && _imgValues[i] != null)
                    {
                        string strTLValue = Common.GetValueFromSQL("SELECT " + theTrafficLightColumn.SystemName + " FROM [Record] WHERE RecordID=" + _qsRecord.RecordID.ToString());
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

                    if (Mode.ToLower() == "view")
                    {
                        _fuValue[i].Visible = false;
                    }
                    else
                    {

                        _lblValue[i].Text = "<img title=\"Remove this image\" style=\"cursor:pointer;\"  id=\"dimg" + _strDynamictabPart + _hfValue[i].ID + "\" src=\"" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                           + "/App_Themes/Default/Images/icon_delete.gif\" />" + _lblValue[i].Text;

                        _hfValue[i].Value = _dtRecordedetail.Rows[0][i].ToString();

                        string strTempJS = @" if(document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"')!=null){  document.getElementById('dimg" + _strDynamictabPart + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _strDynamictabPart + _hfValue[i].ID + @"').value='';
                                                      $('#" + _strDynamictabPart + _lblValue[i].ID + @"').html(''); 
                                            });};";

                       //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "imagedelete" + _strDynamictabPart + i.ToString(), strTempJS, true);
                        //if (!IsPostBack)
                       ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "imagedelete" + _strDynamictabPart + i.ToString(), strTempJS, true);


                    }
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

                        if (_dtColumnsDetail.Rows[i]["RoundNumber"] != DBNull.Value && bDateCal == false && bTextCal==false)
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

                                //_txtValue[i].Text = double.Parse(_txtValue[i].Text).ToString("C").Substring(1);
                                if (_revValue[i] != null)
                                    _revValue[i].Enabled = false;

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
                                strLinkURL = Request.Url.Scheme +"://" + strLinkURL;
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
                                    if (_revValue[i] != null)
                                        _revValue[i].Enabled = false;

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
                                    string strReturnSQL="";
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


        //move to page_int

//        for (int i = 0; i < _dtRecordTypleColumlns.Rows.Count; i++)
//        {
//            if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() == "image")
//            {
//                trX[i].Style.Add("vertical-align", "top");
//            }


//            if (_dtRecordTypleColumlns.Rows[i]["OnlyForAdmin"] != DBNull.Value)
//            {

//                bool bHide = false;

//                if (_dtRecordTypleColumlns.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "1")
//                {
//                    if (!Common.HaveAccess(_strRecordRightID, "1,2"))
//                    {
//                        bHide = true;

                       
//                    }
//                }
//                if (_dtRecordTypleColumlns.Rows[i]["OnlyForAdmin"].ToString().ToLower() == "2")
//                {
//                    if (Request.QueryString["mode"] != null)
//                    {
//                        if (Cryptography.Decrypt(Request.QueryString["mode"]).ToString().ToLower() != "add"
//                            && _qsRecord != null)
//                        {

//                            if (!Common.HaveAccess(_strRecordRightID, "1,2"))
//                            {
//                                if (_qsRecord.EnteredBy != _objUser.UserID)
//                                {
//                                    bHide = true;
//                                }
//                            }

//                        }
//                    }

//                }

//                if (bHide)
//                {
//                    trX[i].ID = "trX" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
//                    string targetID = "#" + _strDynamictabPart + trX[i].ID;
//                    string strOnlyAdminJS = @"$('" + targetID + @"').fadeOut();";

//                    if (bool.Parse(_dtRecordTypleColumlns.Rows[i]["IsMandatory"].ToString())
//                        && _rfvValue[i] != null)
//                    {
//                        strOnlyAdminJS = strOnlyAdminJS + "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";
//                    }
//                    //if (!IsPostBack)
//                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHideAdmin" + _strDynamictabPart + i.ToString(), strOnlyAdminJS, true);


//                }

//            }


//            //perform new showhide

//            DataTable dtShowHide = RecordManager.dbg_ShowWhen_Select(int.Parse(_dtRecordTypleColumlns.Rows[i]["ColumnID"].ToString()));
//            if (dtShowHide.Rows.Count > 0)
//            {
//                if (dtShowHide.Rows.Count == 1)
//                {

//                    trX[i].ID = "trX" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
//                    string strHideColumnValue = dtShowHide.Rows[0]["HideColumnValue"].ToString();
//                    bool bEquals = true;
//                    string strHideOperatorLogic = "";
//                    if (dtShowHide.Rows[0]["HideOperator"].ToString() == "equals")
//                    {
//                        bEquals = true;
//                    }
//                    else
//                    {
//                        bEquals = false;
//                    }


//                    for (int m = 0; m < _dtRecordTypleColumlns.Rows.Count; m++)
//                    {
//                        if (dtShowHide.Rows[0]["HideColumnID"].ToString()
//                            == _dtRecordTypleColumlns.Rows[m]["ColumnID"].ToString())
//                        {
//                            if (trX[m] != null)
//                            {
//                                trX[m].ID = "trX" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                string driverID = "#" + trX[m].ClientID.Substring(0, trX[m].ClientID.Length - 7);
//                                string targetID = driverID + trX[i].ID;
//                                string dirverGroupID = driverID;
//                                if (_ddlValue[m] != null && _ddlValue2[m] == null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_ddlValue[m] != null && _ddlValue2[m] != null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                if (_txtValue[m] != null)
//                                    driverID = driverID + "txt" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_chkValue[m] != null)
//                                    driverID = driverID + "chk" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_lstValue[m] != null)
//                                    driverID = driverID + "lst" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                dirverGroupID = driverID;

//                                string strHideShow = "";

//                                string strValidatorT = "";
//                                string strValidatorF = "";
//                                if (bool.Parse(_dtRecordTypleColumlns.Rows[i]["IsMandatory"].ToString())
//                        && _rfvValue[i] != null)
//                                {
//                                    strValidatorT = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), true)};";
//                                    strValidatorF = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";
//                                }


//                                if (_radioList[m] != null)
//                                {
//                                    driverID = driverID + "radio" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                    dirverGroupID = driverID;
//                                    driverID = driverID + "_";
//                                    strHideShow = strHideShow + "$('" + targetID + "').fadeOut(); " + strValidatorF;
//                                    string strDriverIDMain = driverID;
//                                    for (int n = 0; n < _radioList[m].Items.Count; n++)
//                                    {


//                                        driverID = strDriverIDMain + n.ToString();
//                                        strHideShow = strHideShow + @"  
//                                            $('" + driverID + @"').change(function (e) {
//                                            var strDDDC = $('" + driverID + @"').val();
//                                             if (strDDDC == '" + strHideColumnValue + @"') {
//                                                $('" + targetID + @"').fadeIn(); " + strValidatorT + @"
//                                            }
//                                            else {
//                                                $('" + targetID + @"').fadeOut(); " + strValidatorF + @"
//                                            }
//                                        });
//                                        ";

//                                        if (_radioList[m].SelectedIndex == n)
//                                        {
//                                            strHideShow = strHideShow + "$('" + driverID + "').trigger('change');";
//                                        }


//                                    }

//                                }
//                                else if (_chkValue[m] != null)
//                                {

//                                    string strTrue = "";
//                                    string strFalse = "";
//                                    GetCheckTcikedUnTicked(_dtRecordTypleColumlns.Rows[m]["DropDownValues"].ToString(), ref strTrue, ref strFalse);
//                                    strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//                                    var chk = document.getElementById('" + driverID.Substring(1, driverID.Length - 1) + @"');
//                                    var strDDDC ='';
//                                    if (chk.checked == true) { strDDDC = '" + strTrue + @"'; }
//                                    if (chk.checked == false) { strDDDC = '" + strFalse + @"'; }                                 
//                                    if (strDDDC == '" + strHideColumnValue + @"') {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }
//                                });
//                                $('" + driverID + @"').trigger('change');  ";
//                                }
//                                else if (_ddlValue[m] != null || _ddlValue2[m] != null)
//                                {

//                                    strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//                                    var strDDDC = $('" + dirverGroupID + @"').val();
//                                    if (strDDDC == '" + strHideColumnValue + @"') {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }
//                                });
//                                $('" + driverID + @"').trigger('change');  ";
//                                }
//                                else if (_lstValue[m] != null)
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                        strHideShow = @"                                    
//                                        $('" + driverID + @"').change(function (e) {
//                                        var strDDDC = $('" + dirverGroupID + @"').val();
//                                        if (" + strHideOperatorLogic + @") {
//                                            $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//                                    });
//                                    $('" + driverID + @"').trigger('change');  ";

//                                    }
//                                    else
//                                    {


//                                        strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        var bShow=false;
//                                        var strHideValues='" + strHideColumnValue + @"';
//                                        if(strHideValues==null || strHideValues=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array=strHideValues.split(',');
//                                        var strDDDC = $('" + dirverGroupID + @"').val();
//                                      
//                                        if(strDDDC==null || strDDDC=='' || strDDDC=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC=strDDDC.toString();
//                                        var strDDDC_array=strDDDC.split(',');
//                                         for(var i = 0; i < strHideValues_array.length; i++) {
//                                            
//                                                if(strHideValues_array[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array.length; j++) 
//                                                    {
//                                                         if(strDDDC_array[j]!='')
//                                                        {
//                                                            if(strHideValues_array[i]==strDDDC_array[j])
//                                                                {
//                                                                bShow=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          }
//
//                                        if ( bShow==true) {
//                                            $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//
//                                        
//                                        });
//                                    $('" + driverID + @"').trigger('change');  ";
//                                    }



//                                }
//                                else
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                    }
//                                    else
//                                    {
//                                        strHideOperatorLogic = "strDDDC.indexOf('" + strHideColumnValue + "')>=0";
//                                    }

//                                    strHideShow = @"                                    
//                                    $('" + driverID + @"').change(function (e) {
//                                    var strDDDC = $('" + dirverGroupID + @"').val();
//                                    if (" + strHideOperatorLogic + @") {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }
//                                });
//                                $('" + driverID + @"').trigger('change');  ";

//                                }



//                                //if (!IsPostBack)
//                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHide" + _strDynamictabPart + i.ToString(), strHideShow, true);

//                            }
//                        }
//                    }



//                }



//                if (dtShowHide.Rows.Count == 3)
//                {

//                    trX[i].ID = "trX" + _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString();
//                    string strHideColumnValue = dtShowHide.Rows[0]["HideColumnValue"].ToString();
//                    string strHideColumnValue2 = dtShowHide.Rows[2]["HideColumnValue"].ToString();
//                    string strJoinOperator = " && ";
//                    if (dtShowHide.Rows[1]["JoinOperator"].ToString() == "and")
//                    {
//                        strJoinOperator = " && ";
//                    }
//                    else
//                    {
//                        strJoinOperator = " || ";
//                    }

//                    bool bEquals = true;
//                    bool bEquals2 = true;
//                    string strHideOperatorLogic = "";
//                    string strHideOperatorLogic2 = "";
//                    if (dtShowHide.Rows[0]["HideOperator"].ToString() == "equals")
//                    {
//                        bEquals = true;
//                    }
//                    else
//                    {
//                        bEquals = false;
//                    }

//                    if (dtShowHide.Rows[2]["HideOperator"].ToString() == "equals")
//                    {
//                        bEquals2 = true;
//                    }
//                    else
//                    {
//                        bEquals2 = false;
//                    }


//                    string strDriverID2 = "";
//                    //string strDirverID2Logic = "";
//                    string strShowHideJS2 = "";
//                    string strShowHideEventJS2 = "";
//                    for (int m = 0; m < _dtRecordTypleColumlns.Rows.Count; m++)
//                    {
//                        if (dtShowHide.Rows[2]["HideColumnID"].ToString()
//                            == _dtRecordTypleColumlns.Rows[m]["ColumnID"].ToString())
//                        {
//                            if (trX[m] != null)
//                            {
//                                trX[m].ID = "trX" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                string driverID = "#" + trX[m].ClientID.Substring(0, trX[m].ClientID.Length - 7);
//                                string dirverGroupID = driverID;
//                                if (_ddlValue[m] != null && _ddlValue2[m] == null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_ddlValue[m] != null && _ddlValue2[m] != null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                if (_txtValue[m] != null)
//                                    driverID = driverID + "txt" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_chkValue[m] != null)
//                                    driverID = driverID + "chk" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_lstValue[m] != null)
//                                    driverID = driverID + "lst" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                dirverGroupID = driverID;
//                                strDriverID2 = dirverGroupID;


//                                if (_radioList[m] != null)
//                                {
//                                    driverID = driverID + "radio" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                    dirverGroupID = driverID;
//                                    //var strDDDC2 = document.querySelector('input[name="""+dirverGroupID.Replace("#","").Replace("_","$")+@"""]:checked').value;   
//                                    strShowHideJS2 = @" 
//                                               var strDDDC2 =GetOptValue('" + dirverGroupID.Replace("#", "").Replace("_", "$") + @"');                                                                                   
//                                         ";

//                                    driverID = driverID + "_";
//                                    string strDriverIDMain = driverID;
//                                    for (int n = 0; n < _radioList[m].Items.Count; n++)
//                                    {
//                                        driverID = strDriverIDMain + n.ToString();

//                                        strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";

//                                        strShowHideEventJS2 = strShowHideEventJS2 + @" $('" + driverID + @"').change(function (e) {
//                                            ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                        }); ";

//                                        if (_radioList[m].SelectedIndex == n)
//                                        {
//                                            strShowHideEventJS2 = strShowHideEventJS2 + "$('" + driverID + "').trigger('change');";
//                                        }
//                                    }
//                                }
//                                else if (_chkValue[m] != null)
//                                {
//                                    string strTrue = "";
//                                    string strFalse = "";
//                                    GetCheckTcikedUnTicked(_dtRecordTypleColumlns.Rows[m]["DropDownValues"].ToString(), ref strTrue, ref strFalse);
//                                    strShowHideJS2 = @"                                    
//                                   
//                                    var chk2 = document.getElementById('" + driverID.Substring(1, driverID.Length - 1) + @"');
//                                    var strDDDC2 ='';
//                                    if (chk2.checked == true) { strDDDC2 = '" + strTrue + @"'; }
//                                    if (chk2.checked == false) { strDDDC2 = '" + strFalse + @"'; }                                 
//                                   ";

//                                    strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";
//                                    strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";
//                                }
//                                else if (_ddlValue[m] != null || _ddlValue2[m] != null)
//                                {

//                                    strShowHideJS2 = @" var strDDDC2 = $('" + dirverGroupID + @"').val();";

//                                    strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";
//                                    strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";
//                                }
//                                else if (_lstValue[m] != null)
//                                {
//                                    if (bEquals2)
//                                    {
//                                        strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";

//                                        strShowHideJS2 = @"                                   
//                                       
//                                        var strDDDC2 = $('" + dirverGroupID + @"').val();
//                                        ";
//                                        strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";

//                                    }
//                                    else
//                                    {


//                                        strShowHideJS2 = @"                                  
//                                 
//                                        var bShow2=false;
//                                        var strHideValues2='" + strHideColumnValue2 + @"';
//                                        if(strHideValues2==null || strHideValues2=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array2=strHideValues2.split(',');
//                                        var strDDDC2 = $('" + dirverGroupID + @"').val();
//                                      
//                                        if(strDDDC2==null || strDDDC2=='' || strDDDC2=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC2=strDDDC2.toString();
//                                        var strDDDC_array2=strDDDC2.split(',');
//                                         for(var i = 0; i < strHideValues_array2.length; i++) {
//                                            
//                                                if(strHideValues_array2[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array2.length; j++) 
//                                                    {
//                                                         if(strDDDC_array2[j]!='')
//                                                        {
//                                                            if(strHideValues_array2[i]==strDDDC_array2[j])
//                                                                {
//                                                                bShow2=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          }";

//                                        strHideOperatorLogic2 = "bShow2";
//                                        strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";
//                                    }



//                                }
//                                else
//                                {
//                                    if (bEquals2)
//                                    {
//                                        strHideOperatorLogic2 = "strDDDC2 == '" + strHideColumnValue2 + "'";

//                                    }
//                                    else
//                                    {
//                                        strHideOperatorLogic2 = "strDDDC2.indexOf('" + strHideColumnValue2 + "')>=0";
//                                    }

//                                    strShowHideJS2 = @"                                  
//                                   
//                                    var strDDDC2 = $('" + dirverGroupID + @"').val();
//                                   ";

//                                    strShowHideEventJS2 = @"$('" + driverID + @"').change(function (e) {
//                                                    ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                                });
//                                                $('" + driverID + @"').trigger('change');  ";

//                                }


//                            }
//                        }
//                    }

//                    for (int m = 0; m < _dtRecordTypleColumlns.Rows.Count; m++)
//                    {
//                        if (dtShowHide.Rows[0]["HideColumnID"].ToString()
//                            == _dtRecordTypleColumlns.Rows[m]["ColumnID"].ToString())
//                        {
//                            if (trX[m] != null)
//                            {
//                                trX[m].ID = "trX" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                string driverID = "#" + trX[m].ClientID.Substring(0, trX[m].ClientID.Length - 7);
//                                string targetID = driverID + trX[i].ID;
//                                string dirverGroupID = driverID;
//                                if (_ddlValue[m] != null && _ddlValue2[m] == null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_ddlValue[m] != null && _ddlValue2[m] != null)
//                                    driverID = driverID + "ddl" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();


//                                if (_txtValue[m] != null)
//                                    driverID = driverID + "txt" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_chkValue[m] != null)
//                                    driverID = driverID + "chk" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                if (_lstValue[m] != null)
//                                    driverID = driverID + "lst" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                dirverGroupID = driverID;

//                                string strHideShow = "";

//                                string strValidatorT = "";
//                                string strValidatorF = "";
//                                if (bool.Parse(_dtRecordTypleColumlns.Rows[i]["IsMandatory"].ToString())
//                        && _rfvValue[i] != null)
//                                {
//                                    strValidatorT = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), true)};";
//                                    strValidatorF = "if(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "')!=null){ValidatorEnable(document.getElementById('" + _strDynamictabPart + _rfvValue[i].ID.ToString() + "'), false)};";
//                                }


//                                if (_radioList[m] != null)
//                                {
//                                    driverID = driverID + "radio" + _dtRecordTypleColumlns.Rows[m]["SystemName"].ToString();

//                                    dirverGroupID = driverID;
//                                    driverID = driverID + "_";
//                                    strHideShow = strHideShow + "$('" + targetID + "').fadeOut(); " + strValidatorF;
//                                    string strDriverIDMain = driverID;

//                                    //var strDDDC =  document.querySelector('input[name=""" + dirverGroupID.Replace("#", "").Replace("_", "$") + @"""]:checked').value;" + strShowHideJS2 + @"

//                                    strHideShow = strHideShow + @"function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                            
//                                            var strDDDC =GetOptValue('" + dirverGroupID.Replace("#", "").Replace("_", "$") + @"');" + strShowHideJS2 + @"
//                                             if (strDDDC == '" + strHideColumnValue + @"' " + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                                $('" + targetID + @"').fadeIn(); " + strValidatorT + @"
//                                            }
//                                            else {
//                                                $('" + targetID + @"').fadeOut(); " + strValidatorF + @"
//                                            } } ";

//                                    for (int n = 0; n < _radioList[m].Items.Count; n++)
//                                    {

//                                        driverID = strDriverIDMain + n.ToString();
//                                        strHideShow = strHideShow + @"                                           
//                                           
//                                          $('" + driverID + @"').change(function (e) {
//                                            ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                        }); " + strShowHideEventJS2;

//                                        if (_radioList[m].SelectedIndex == n)
//                                        {
//                                            strHideShow = strHideShow + "$('" + driverID + "').trigger('change');";
//                                        }


//                                    }

//                                }
//                                else if (_chkValue[m] != null)
//                                {

//                                    string strTrue = "";
//                                    string strFalse = "";
//                                    GetCheckTcikedUnTicked(_dtRecordTypleColumlns.Rows[m]["DropDownValues"].ToString(), ref strTrue, ref strFalse);
//                                    strHideShow = @"                                    
//                                     function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                    var chk = document.getElementById('" + driverID.Substring(1, driverID.Length - 1) + @"');
//                                    var strDDDC ='';
//                                    if (chk.checked == true) { strDDDC = '" + strTrue + @"'; }
//                                    if (chk.checked == false) { strDDDC = '" + strFalse + @"'; }   " + strShowHideJS2 + @"                              
//                                    if (strDDDC == '" + strHideColumnValue + @"' " + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }}
//                                 $('" + driverID + @"').change(function (e) {
//                                      ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                });
//                                $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2;
//                                }
//                                else if (_ddlValue[m] != null || _ddlValue2[m] != null)
//                                {

//                                    strHideShow = @"                                    
//                                    function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                    var strDDDC = $('" + dirverGroupID + @"').val();  " + strShowHideJS2 + @"    
//                                     if (strDDDC == '" + strHideColumnValue + @"' " + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }}
//
//                                 $('" + driverID + @"').change(function (e) {
//                                      ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                });
//                                $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2; ;
//                                }
//                                else if (_lstValue[m] != null)
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                        strHideShow = @"                                    
//                                        function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                        var strDDDC = $('" + dirverGroupID + @"').val();  " + strShowHideJS2 + @"               
//                                        if (" + strHideOperatorLogic + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                            $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }}
//                                     $('" + driverID + @"').change(function (e) {
//                                               ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                    });
//                                    $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2;

//                                    }
//                                    else
//                                    {


//                                        strHideShow = @"                                    
//                                  
//                                     function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        var bShow=false;
//                                        var strHideValues='" + strHideColumnValue + @"';
//                                        if(strHideValues==null || strHideValues=='')
//                                        {
//                                            return;
//                                        }
//                                        var strHideValues_array=strHideValues.split(',');
//                                        var strDDDC = $('" + dirverGroupID + @"').val();
//                                      
//                                        if(strDDDC==null || strDDDC=='' || strDDDC=='undefined')
//                                        {
//                                            return;
//                                        }
//                                     strDDDC=strDDDC.toString();
//                                        var strDDDC_array=strDDDC.split(',');
//                                         for(var i = 0; i < strHideValues_array.length; i++) {
//                                            
//                                                if(strHideValues_array[i]!='')
//                                                {
//                                                     for(var j = 0; j < strDDDC_array.length; j++) 
//                                                    {
//                                                         if(strDDDC_array[j]!='')
//                                                        {
//                                                            if(strHideValues_array[i]==strDDDC_array[j])
//                                                                {
//                                                                bShow=true;
//                                                                }
//                                                        }
//                                                    }
//
//                                                }
//
//                                          } " + strShowHideJS2 + @" 
//
//                                        if ( bShow==true" + strJoinOperator + strHideOperatorLogic2 + @" ) {
//                                            $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                        }
//                                        else {
//                                            $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                        }
//                                        }
//                                        $('" + driverID + @"').change(function (e) {
//                                              ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                        });
//                                    $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2;
//                                    }



//                                }
//                                else
//                                {
//                                    if (bEquals)
//                                    {
//                                        strHideOperatorLogic = "strDDDC == '" + strHideColumnValue + "'";

//                                    }
//                                    else
//                                    {
//                                        strHideOperatorLogic = "strDDDC.indexOf('" + strHideColumnValue + "')>=0";
//                                    }

//                                    strHideShow = @"                                    
//                                   function  ShowHideFunction" + _strDynamictabPart + i.ToString() + @"(){
//                                    var strDDDC = $('" + dirverGroupID + @"').val(); " + strShowHideJS2 + @"   
//                                    if (" + strHideOperatorLogic + strJoinOperator + strHideOperatorLogic2 + @") {
//                                        $('" + targetID + @"').fadeIn();" + strValidatorT + @"
//                                    }
//                                    else {
//                                        $('" + targetID + @"').fadeOut();" + strValidatorF + @"
//                                    }}
//
//                                  $('" + driverID + @"').change(function (e) {
//                                      ShowHideFunction" + _strDynamictabPart + i.ToString() + @"();
//                                });
//                                $('" + driverID + @"').trigger('change');  " + strShowHideEventJS2;

//                                }
//                                //if (!IsPostBack)
//                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHide2" + _strDynamictabPart + i.ToString(), strHideShow, true);

//                            }
//                        }
//                    }



//                }


//            }



//        }




    }

    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtReasonForChange.Enabled = p_bEnable;
        //trReasonForChange.Visible = false;
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
                            _fuValue[i].Visible = false;
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








    public void lnkSaveClose_Click(object sender, EventArgs e)
    {

        try
        {
            if (PerformSave())
            {
                //do nothing
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message + "  " + ex.StackTrace;
        }
    }


    protected void lnkOk_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        ViewState["ok"] = "ok";

        lnkSaveClose_Click(null, null);

    }










    protected void lnkNo_Click(object sender, EventArgs e)
    {
        ViewState["ok"] = "no";
        trMainSave.Visible = true;
        trConfirmation.Visible = false;
        lblMsg.Text = "";

    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {

        try
        {
            if (PerformSave())
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Successfully saved!');", true);
                lblMsg.Text = "Successfully saved!";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                if (Mode.ToLower() == "add")
                {
                    Response.Redirect(hlEdit.NavigateUrl, false);
                }
              
            }

        }
        catch (Exception ex)
        {
            //
        }

    }


    protected void ShowHideNextPrev()
    {
        if (OnlyOneRecord==false)
        {
            string strCount = Common.GetValueFromSQL("SELECT Count(RecordID) FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1  AND (" + TextSearch + ") ");

            if (strCount == "")
                strCount = "0";

            if(int.Parse(strCount)<2)
            {
                lnkNext.Visible = false;
                lnkPrevious.Visible = false;
            }
            else
            {
                string strNextRecordID = Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" + TableID.ToString() + " AND IsActive= 1 AND RecordID>" + ViewState["RecordID"].ToString() + " AND (" + TextSearch + ") ORDER BY RecordID");

                if(strNextRecordID=="")
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

                        if (strEmail.Trim() == "" && strPassword=="")
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

                                strBody = strBody.Replace("[URL]", Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath);

                                string strTo = newUser.Email;

                                string strError = "";

                                theConent.ContentP = strBody;

                                try
                                {
                                    //Common.SendSingleEmail(strTo, theConent, ref strError);
                                    DBGurus.SendEmail(theConent.ContentKey, true, null, theConent.Heading, theConent.ContentP, "", strTo, "", "", null,null, out strError);
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

    protected bool PerformSave()
    {

        lblMsg.ForeColor = System.Drawing.Color.Red;




        lblMsg.Text = "";
        bool bDataWarning = false;
        //bool bSensorWarning = false;
        int iWarning = 0;
        string strValidationError = "";
        string strValue = "";
        string strEmailFullBody = "";
        string strSMSFullBody = "";
        int iWarningColumnCount = 0;
        try
        {

            if (IsUserInputOK())
            {
                string strWarningResults = "";

                switch (Mode.ToLower())
                {
                    case "add":




                        if (SecurityManager.IsRecordsExceeded(_iSessionAccountID))
                        {
                            Session["DoNotAllow"] = "true";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DoNotAllow" + _strDynamictabPart, "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);
                            return false;
                        }


                        Record newRecord = new Record();

                        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                        {


                            if (i == _iEnteredByIndex)
                            {
                                RecordManager.MakeTheRecord(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _ddlEnteredBy.SelectedValue);
                            }
                            else if (i == _iIsActiveIndex)
                            {
                                RecordManager.MakeTheRecord(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _chkIsActive.Checked);
                            }
                            else if (i == _iDateTimeRecorded)
                            {
                                string strDateTime;
                                if (_txtValue[i].Text == "")
                                {
                                    strDateTime = DateTime.Now.ToShortDateString() + " 00:00";
                                }
                                else
                                {
                                    string strTimePart = "";
                                    if (_txtTime[i] != null)
                                    {
                                        if (_txtTime[i].Text == "")
                                        {
                                            strTimePart = "00:00";
                                        }
                                        else
                                        {
                                            if (_txtTime[i].Text.ToLower().IndexOf(":am") > 0)
                                            {
                                                strTimePart = _txtTime[i].Text.ToLower().Replace(":am", ":00");
                                            }
                                            else
                                            {
                                                strTimePart = _txtTime[i].Text.ToLower().Replace(":pm", ":00");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strTimePart = "00:00";
                                    }

                                    strDateTime = _txtValue[i].Text + " " + strTimePart;
                                }

                                RecordManager.MakeTheRecord(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strDateTime);
                            }
                            else
                            {

                                //Each Non Standard Column is going here.

                                //perform caculation here


                                strValue = "";


                                for (int j = 0; j < _dtColumnsNotDetail.Rows.Count; j++)
                                {
                                    if (_dtColumnsNotDetail.Rows[j]["Constant"].ToString().Length > 0)
                                    {
                                        RecordManager.MakeTheRecord(ref newRecord, _dtColumnsNotDetail.Rows[j]["SystemName"].ToString(), _dtColumnsNotDetail.Rows[j]["Constant"].ToString());
                                    }
                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                                {
                                    string strDateTime = "";
                                    if (_txtValue[i].Text.Trim() == "")
                                    {
                                        // strDateTime = DateTime.Now.ToShortDateString() + " 12:00:00 AM";
                                    }
                                    else
                                    {
                                        DateTime dtTemp;
                                        if (DateTime.TryParseExact(_txtValue[i].Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                        {
                                            _txtValue[i].Text = dtTemp.ToShortDateString();
                                        }

                                        string strTimePart = "";
                                        if (_txtTime[i] != null)
                                        {
                                            if (_txtTime[i].Text == "")
                                            {
                                                strTimePart = " 00:00";
                                            }
                                            else
                                            {
                                                if (_txtTime[i].Text.ToLower().IndexOf(":am") > 0)
                                                {
                                                    strTimePart = _txtTime[i].Text.ToLower().Replace(":am", ":00");
                                                }
                                                else
                                                {
                                                    strTimePart = _txtTime[i].Text.ToLower().Replace(":pm", ":00");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strTimePart = " 00:00";
                                        }

                                        strDateTime = _txtValue[i].Text + " " + strTimePart;
                                    }
                                    strValue = strDateTime;

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                                {
                                    string strDateTime = "";
                                    if (_txtValue[i].Text == "")
                                    {
                                        // strDateTime = DateTime.Now.ToShortDateString(); // +" 12:00:00 AM";
                                    }
                                    else
                                    {
                                        DateTime dtTemp;
                                        if (DateTime.TryParseExact(_txtValue[i].Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                        {
                                            _txtValue[i].Text = dtTemp.ToShortDateString();
                                        }
                                        strDateTime = _txtValue[i].Text;// +" " + "12:00:00 AM";
                                    }
                                    strValue = strDateTime;


                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                                {
                                    strValue = _txtValue[i].Text;

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                                    || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                                {

                                    if (_hfValue2[i].Value == "yes" && _fuValue2[i].HasFile)
                                    {
                                        //string strFolder = "~\\UserFiles\\AppFiles";
                                        string strFolder = "\\UserFiles\\AppFiles";
                                        string strFileName = _fuValue2[i].FileName;
                                        string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;// +"." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                                        //string strPath = Server.MapPath(strFolder + "\\" + strUniqueName);
                                        string strPath = _strFilesPhisicalPath + strFolder + "\\" + strUniqueName;
                                        _fuValue2[i].SaveAs(strPath);
                                        _hfValue[i].Value = strUniqueName;
                                    }

                                    strValue = _hfValue[i].Value;
                                }

                                //if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() == "slider")
                                //{
                                //    strValue = _txtValue[i].Text;
                                //}

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "text"
                                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                                {
                                    strValue = _txtValue[i].Text;

                                    if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "6" && _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                                    {
                                        strValue = strValue.Replace(",", "");
                                    }

                                    if (_dtColumnsDetail.Rows[i]["NumberType"] != null)
                                    {
                                        if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "8")
                                        {
                                            try
                                            {
                                                string strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + _dtColumnsDetail.Rows[i]["SystemName"].ToString() + ")) FROM Record WHERE TableID=" + TableID.ToString());
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
                                                strValue = "";
                                            }

                                        }

                                    }

                                }


                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                           && _dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                           && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                           || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                                {
                                    if (_ddlValue[i].SelectedIndex != 0)
                                    {
                                        strValue = _ddlValue[i].Text;
                                    }
                                }
                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                                {
                                    if (_radioList[i].SelectedItem != null)
                                    {
                                        strValue = _radioList[i].SelectedItem.Value;
                                    }

                                }
                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                                {
                                    strValue = Common.GetCheckBoxValue(_dtColumnsDetail.Rows[i]["DropDownValues"].ToString(), ref _chkValue[i]);
                                }


                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                                {
                                    LocationColumn newLocationColumn = new LocationColumn();
                                    if (_hfValue3[i] != null)
                                    {
                                        if (_hfValue3[i].Value == "")
                                            _hfValue3[i].Value = "5";

                                        newLocationColumn.ZoomLevel = int.Parse(_hfValue3[i].Value);

                                        if (_txtValue[i] != null && _txtTime[i] != null)
                                        {
                                            if (_txtValue[i].Text != "")
                                                newLocationColumn.Latitude = _txtValue[i].Text;

                                            if (_txtTime[i].Text != "")
                                                newLocationColumn.Longitude = _txtTime[i].Text;

                                        }
                                        else
                                        {
                                            if (_hfValue[i].Value != "")
                                                newLocationColumn.Latitude = _hfValue[i].Value;

                                            if (_hfValue2[i].Value != "")
                                                newLocationColumn.Longitude = _hfValue2[i].Value;
                                        }

                                        if (_txtValue2[i] != null)
                                        {
                                            if (_txtValue2[i].Text != "")
                                                newLocationColumn.Address = _txtValue2[i].Text;
                                        }
                                    }
                                    strValue = newLocationColumn.GetJSONString();
                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                                {
                                    strValue = _htmValue[i].Text;
                                }
                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "staticcontent")
                                {
                                    strValue = "";
                                }
                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                                    && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "")
                                {
                                    if (_lstValue[i].SelectedItem != null)
                                    {
                                        strValue = Common.GetListValues(_lstValue[i]);
                                    }

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                                 && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "checkbox")
                                {
                                    if (_cblValue[i].SelectedItem != null)
                                    {
                                        strValue =Common.GetCheckBoxListValues(_cblValue[i]);
                                    }

                                }

                                //if (_dtRecordTypleColumlns.Rows[i]["Calculation"] != DBNull.Value)
                                //{
                                //    if (_dtRecordTypleColumlns.Rows[i]["Calculation"].ToString().Length > 0)
                                //    {
                                //        //replace params by value // if there is a constant 
                                //        string strFormula = _dtRecordTypleColumlns.Rows[i]["Calculation"].ToString().ToLower();
                                //        for (int j = 0; j < _dtRecordTypleColumlns.Rows.Count; j++)
                                //        {
                                //            if (_radioList[j] != null)
                                //            {
                                //                if (_radioList[j].SelectedItem != null)
                                //                    strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_radioList[j].SelectedValue));
                                //            }
                                //            if (_ddlValue[j] != null)
                                //            {
                                //                if (_dtRecordTypleColumlns.Rows[j]["ColumnType"].ToString() == "dropdown"
                                //                        && (_dtRecordTypleColumlns.Rows[j]["DropDownType"].ToString() == "table"
                                //                        || _dtRecordTypleColumlns.Rows[j]["DropDownType"].ToString() == "tabledd")
                                //                        && _dtRecordTypleColumlns.Rows[j]["TableTableID"] != DBNull.Value
                                //                         && _dtRecordTypleColumlns.Rows[j]["LinkedParentColumnID"] != DBNull.Value
                                //                        && _dtRecordTypleColumlns.Rows[j]["DisplayColumn"].ToString() != ""
                                //                        )
                                //                {
                                //                    if (_ddlValue[j].SelectedItem != null)
                                //                        strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_ddlValue[j].SelectedValue));
                                //                }

                                //            }

                                //            if (_txtValue[j] != null)
                                //            {
                                //                if ((_dtRecordTypleColumlns.Rows[j]["ColumnType"].ToString() == "datetime"
                                //                    || _dtRecordTypleColumlns.Rows[j]["ColumnType"].ToString() == "date"
                                //                    || _dtRecordTypleColumlns.Rows[j]["ColumnType"].ToString() == "time")
                                //                    && (_txtValue[j].Text.IndexOf("/") > -1
                                //                    || _txtValue[j].Text.IndexOf(":") > -1)
                                //                    && _dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
                                //                {

                                //                    DateTime dtValue = DateTime.Today;
                                //                    if (_txtValue[j].Text.IndexOf("/") > -1)
                                //                    {
                                //                        //dtValue = DateTime.ParseExact(_txtValue[j].Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);

                                //                        if (DateTime.TryParseExact(_txtValue[j].Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtValue))
                                //                        {
                                //                            _txtValue[j].Text = dtValue.ToShortDateString();
                                //                        }
                                //                    }
                                //                    if (_txtValue[j].Text.IndexOf(":") > -1)
                                //                        dtValue = DateTime.ParseExact(dtValue.Date.ToShortDateString() + " " + _txtValue[j].Text, "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                                //                    if (_txtTime[j] != null)
                                //                    {
                                //                        if (_txtTime[j].Text != "")
                                //                        {
                                //                            //dtValue = dtValue + TimeSpan.ParseExact(_txtTime[j].Text,"HH:m",CultureInfo.InvariantCulture);
                                //                            dtValue = DateTime.ParseExact(_txtValue[j].Text + " " + _txtTime[j].Text, "d/M/yyyy HH:m", CultureInfo.InvariantCulture);
                                //                        }
                                //                    }
                                //                    strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", dtValue.Ticks.ToString());
                                //                }
                                //                else
                                //                {

                                //                    if (_dtRecordTypleColumlns.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                                //                    {
                                //                        strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_txtValue[j].Text));
                                //                    }
                                //                    else
                                //                    {
                                //                        strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", _txtValue[j].Text);
                                //                    }
                                //                }
                                //            }
                                //        }

                                //        //now check if any non detail constant/default values are here


                                //        for (int j = 0; j < _dtColumnsNotDetail.Rows.Count; j++)
                                //        {
                                //            if (_dtColumnsNotDetail.Rows[j]["Constant"].ToString().Length > 0)
                                //            {
                                //                if (_dtColumnsNotDetail.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                                //                {
                                //                    strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.MakeDecimal(Common.IgnoreSymbols(_dtColumnsNotDetail.Rows[j]["Constant"].ToString())));
                                //                }
                                //                else
                                //                {
                                //                    strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.MakeDecimal(_dtColumnsNotDetail.Rows[j]["Constant"].ToString()));
                                //                }
                                //            }
                                //        }



                                //        string strResult = RecordManager.CalculationResult(strFormula);

                                //        if (Common.IsThisDouble(strResult) == false)
                                //        {
                                //            strResult = "";
                                //        }
                                //        else
                                //        {
                                //            if (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
                                //            {

                                //                double lResult = double.Parse(strResult);

                                //                switch (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"].ToString())
                                //                {
                                //                    case "year":
                                //                        lResult = lResult / (365 * 24 * 60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;

                                //                    case "month":
                                //                        //strResult = (lResult / (30 * 24 * 60 * 60)).ToString();
                                //                        lResult = lResult / (30 * 24 * 60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();

                                //                        break;

                                //                    case "day":
                                //                        //strResult = (lResult / (24 * 60 * 60)).ToString();
                                //                        lResult = lResult / (24 * 60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;

                                //                    case "week":
                                //                        //strResult = (lResult / (7 * 24 * 60 * 60)).ToString();
                                //                        lResult = lResult / (7 * 24 * 60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;
                                //                    case "hour":
                                //                        //strResult = (lResult / ( 60 * 60)).ToString();
                                //                        lResult = lResult / (60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;
                                //                    case "minute":
                                //                        //strResult = (lResult / ( 60)).ToString();
                                //                        lResult = lResult / (60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;
                                //                    case "second":
                                //                        //strResult = lResult.ToString();                                                         
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;


                                //                    default:
                                //                        break;
                                //                }
                                //            }

                                //        }

                                //        _txtValue[i].Text = strResult;
                                //        strValue = _txtValue[i].Text;
                                //    }
                                //}



                                if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value && _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
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
                                            }
                                            catch
                                            {
                                                //
                                            }


                                            strValue = _txtValue[i].Text;
                                        }
                                    }

                                }

                                //          if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() == "dropdown"
                                //&& _dtRecordTypleColumlns.Rows[i]["DropDownType"].ToString() == "linked"
                                //&& _dtRecordTypleColumlns.Rows[i]["TableTableID"] != DBNull.Value
                                //&& _dtRecordTypleColumlns.Rows[i]["ParentColumnID"] != DBNull.Value
                                //&& _dtRecordTypleColumlns.Rows[i]["DisplayColumn"].ToString() != "")
                                //          {
                                //              strValue = _ddlValue[i].SelectedValue;
                                //          }


                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                           && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                           || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                           && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                           && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "")
                                {
                                    if (_dtColumnsDetail.Rows[i]["ParentColumnID"] == DBNull.Value)
                                    {
                                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                                        {
                                            strValue = _hfValue[i].Value;
                                        }
                                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                                        {
                                            if (_ddlValue[i].SelectedItem != null)
                                                strValue = _ddlValue[i].SelectedValue;
                                        }
                                    }
                                    else
                                    {
                                        if (_ddlValue[i].SelectedItem != null)
                                            strValue = _ddlValue[i].SelectedValue;
                                    }

                                }



                                RecordManager.MakeTheRecord(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strValue);
                                if (strValue.Length > 0)
                                {
                                    if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"] == DBNull.Value)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                                        {
                                            if (UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString(), ref strValidationError))
                                            {
                                                //strValidationResults = strValidationResults + "\n" + _dtRecordTypleColumlns.Rows[i]["DisplayTextDetail"].ToString() + " - " + "VALID";
                                            }
                                            else
                                            {
                                                lblMsg.Text = "Invalid data - " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString();
                                                if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() == ""
                                                    && _dtColumnsDetail.Rows[i]["TableTableID"] == DBNull.Value)
                                                {
                                                    _txtValue[i].Focus();
                                                }
                                                else if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                                                   && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                                                    || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                                                {
                                                    _ddlValue[i].Focus();
                                                }
                                                return false;
                                            }
                                        }
                                    }

                                    if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"] == DBNull.Value)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                                        {
                                            if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), ref strValidationError))
                                            {
                                                strWarningResults = strWarningResults + " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";

                                                string strTemp = "";
                                                bDataWarning = true;
                                                //RecordManager.SendDataWanrningSMSandEmail(int.Parse(_dtRecordTypleColumlns.Rows[i]["ColumnID"].ToString()), _txtValue[i].Text, newRecord.DateTimeRecorded.ToString(), ref strTemp,int.Parse(Session["AccountID"].ToString()),_strURL);
                                                if (strTemp != "")
                                                {
                                                    lblMsg.Text = lblMsg.Text + " " + strTemp;
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }

                                    //check SD
                                    if (bool.Parse(_dtColumnsDetail.Rows[i]["CheckUnlikelyValue"].ToString()))
                                    {
                                        int? iCount = RecordManager.ets_Table_GetCount((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), -1);
                                        if (iCount != null)
                                        {
                                            if (iCount >= Common.MinSTDEVRecords)
                                            {
                                                string strRecordedate;
                                                if (_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
                                                {
                                                    strRecordedate = Common.IgnoreSymbols(_txtValue[i].Text);
                                                }
                                                else
                                                {
                                                    strRecordedate = _txtValue[i].Text;
                                                }

                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), -1);

                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), -1);

                                                double dRecordedate = double.Parse(strRecordedate);
                                                if (dAVG != null && dSTDEV != null)
                                                {
                                                    dSTDEV = dSTDEV * 3;
                                                    if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
                                                    {
                                                        //deviation happaned
                                                        strWarningResults = strWarningResults + " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
                                                    }

                                                }
                                            }
                                        }

                                    }


                                }//

                            }
                        }




                        //perform calculation that are not in detail page

                        //if (_dtColumnsNotDetail.Rows.Count > 0)
                        //{
                        //    for (int x = 0; x < _dtColumnsNotDetail.Rows.Count; x++)
                        //    {
                        //        if (_dtColumnsNotDetail.Rows[x]["IsStandard"].ToString().ToLower() == "false")
                        //        {
                        //            if (_dtColumnsNotDetail.Rows[x]["Calculation"].ToString().Length > 0)
                        //            {
                        //                //ops we got a column that need to be calculated
                        //                //replace params by value
                        //                string strFormula = _dtColumnsNotDetail.Rows[x]["Calculation"].ToString().ToLower();
                        //                for (int j = 0; j < _dtRecordTypleColumlns.Rows.Count; j++)
                        //                {
                        //                    if (_txtValue[j] != null)
                        //                    {
                        //                        if (_dtRecordTypleColumlns.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                        //                        {
                        //                            strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_txtValue[j].Text));
                        //                        }
                        //                        else
                        //                        {
                        //                            strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", _txtValue[j].Text);
                        //                        }
                        //                    }
                        //                }


                        //                for (int j = 0; j < _dtColumnsNotDetail.Rows.Count; j++)
                        //                {
                        //                    if (_dtColumnsNotDetail.Rows[j]["Constant"].ToString().Length > 0)
                        //                    {


                        //                        if (_dtColumnsNotDetail.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                        //                        {
                        //                            strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.MakeDecimal(Common.IgnoreSymbols(_dtColumnsNotDetail.Rows[j]["Constant"].ToString())));
                        //                        }
                        //                        else
                        //                        {
                        //                            strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.MakeDecimal(_dtColumnsNotDetail.Rows[j]["Constant"].ToString()));
                        //                        }


                        //                    }
                        //                }



                        //                string strResult = RecordManager.CalculationResult(strFormula);

                        //                if (Common.IsThisDouble(strResult) == false)
                        //                {
                        //                    strResult = "";
                        //                }

                        //                RecordManager.MakeTheRecord(ref newRecord, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), strResult);

                        //                strValue = strResult;
                        //                if (strValue != "")
                        //                {

                        //                    if (_dtColumnsNotDetail.Rows[x]["ValidationOnWarning"] == DBNull.Value)
                        //                    {
                        //                        //do nothing
                        //                    }
                        //                    else
                        //                    {
                        //                        if (_dtColumnsNotDetail.Rows[x]["ValidationOnWarning"].ToString().Length > 0)
                        //                        {
                        //                            if (UploadManager.IsDataValid(strValue, _dtColumnsNotDetail.Rows[x]["ValidationOnWarning"].ToString(), ref strValidationError, bool.Parse(_dtColumnsNotDetail.Rows[x]["IgnoreSymbols"].ToString())))
                        //                            {
                        //                                strWarningResults = strWarningResults + " WARNING: " + _dtColumnsNotDetail.Rows[x]["DisplayName"].ToString() + " – Value outside accepted range.";

                        //                                string strTemp = "";
                        //                                bDataWarning = true;
                        //                                //RecordManager.SendDataWanrningSMSandEmail(int.Parse(_dtRecordTypleColumlns.Rows[i]["ColumnID"].ToString()), _txtValue[i].Text, newRecord.DateTimeRecorded.ToString(), ref strTemp,int.Parse(Session["AccountID"].ToString()),_strURL);
                        //                                if (strTemp != "")
                        //                                {
                        //                                    lblMsg.Text = lblMsg.Text + " " + strTemp;
                        //                                }
                        //                            }
                        //                            else
                        //                            {

                        //                            }
                        //                        }
                        //                    }



                        //                    //check SD
                        //                    if (bool.Parse(_dtColumnsNotDetail.Rows[x]["CheckUnlikelyValue"].ToString()))
                        //                    {
                        //                        int? iCount = RecordManager.ets_Table_GetCount((int)_theTable.TableID, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), -1);
                        //                        if (iCount != null)
                        //                        {
                        //                            if (iCount >= Common.MinSTDEVRecords)
                        //                            {
                        //                                string strRecordedate;
                        //                                if (_dtColumnsNotDetail.Rows[x]["IgnoreSymbols"].ToString().ToLower() == "true")
                        //                                {
                        //                                    strRecordedate = Common.IgnoreSymbols(strValue);
                        //                                }
                        //                                else
                        //                                {
                        //                                    strRecordedate = strValue;
                        //                                }

                        //                                double? dAVG = RecordManager.ets_Table_GetAVG((int)_theTable.TableID, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), -1);

                        //                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)_theTable.TableID, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), -1);

                        //                                double dRecordedate = double.Parse(strRecordedate);
                        //                                if (dAVG != null && dSTDEV != null)
                        //                                {
                        //                                    dSTDEV = dSTDEV * 3;
                        //                                    if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
                        //                                    {
                        //                                        //deviation happaned
                        //                                        strWarningResults = strWarningResults + " WARNING: " + _dtColumnsNotDetail.Rows[x]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
                        //                                    }

                        //                                }
                        //                            }
                        //                        }

                        //                    }

                        //                    //END SD








                        //                }
                        //            }
                        //        }

                        //    }

                        //}






                        newRecord.TableID = TableID;

                        bool bMaxTime = false;

                        if (newRecord.DateTimeRecorded == null)
                            newRecord.DateTimeRecorded = DateTime.Now;

                        if (!RecordManager.IsTimeBetweenRecordOK((int)newRecord.TableID, -1, (DateTime)newRecord.DateTimeRecorded))
                        {
                            strWarningResults = strWarningResults + " WARNING: " + WarningMsg.MaxtimebetweenRecords + "!";
                            bMaxTime = true;

                        }

                        if (strWarningResults.Length > 0)
                        {
                            newRecord.WarningResults = strWarningResults.Trim();
                        }
                        if (newRecord.EnteredBy == null)
                        {
                            newRecord.EnteredBy = _objUser.UserID;
                        }

                        //check duplicate
                        //if ((bool)_theTable.IsRecordDateUnique)
                        //{

                        //    if (RecordManager.ets_Record_IsDuplicate_Entry((int)newRecord.TableID, (DateTime)newRecord.DateTimeRecorded, -1))
                        //    {
                        //        lblMsg.Text = "Duplicate Record!";
                        //        return false;
                        //    }

                        //}







                        if (newRecord.WarningResults != null && newRecord.WarningResults != "")
                        {
                            _lblWarningResults.Visible = true;
                            _lblWarningResultsValue.Text = newRecord.WarningResults.ToString();
                        }
                        else
                        {
                            _lblWarningResults.Visible = false;
                        }
                        //if (ViewState["ok"] != null)
                        //{
                        //    if (ViewState["ok"].ToString() == "no")
                        //    {
                        //        if (newRecord.WarningResults != null)
                        //        {
                        //            if (newRecord.WarningResults.ToString().Trim().Length > 0)
                        //            {
                        //                lblMsg.Text = "The values entered have caused warnings – would you like to continue?";
                        //                trConfirmation.Visible = true;
                        //                trMainSave.Visible = false;
                        //                return false;
                        //            }
                        //        }
                        //    }

                        //}
                        //else
                        //{
                        //    if (newRecord.WarningResults != null)
                        //    {
                        //        if (newRecord.WarningResults.ToString().Trim().Length > 0)
                        //        {
                        //            lblMsg.Text = "The values entered have caused warnings – would you like to continue?";
                        //            trConfirmation.Visible = true;
                        //            trMainSave.Visible = false;
                        //            return false;
                        //        }
                        //    }

                        //}

                        //check AutoCreateUser
                        if (_theTable.AddUserRecord != null && _theTable.AddUserUserColumnID != null && _theTable.AddUserPasswordColumnID != null)
                        {
                            Column theEmailColumn = RecordManager.ets_Column_Details((int)_theTable.AddUserUserColumnID);
                            if (theEmailColumn != null)
                            {
                                string strEmail = RecordManager.GetRecordValue(ref newRecord, theEmailColumn.SystemName);
                                DataTable dtUsers = Common.DataTableFromText("SELECT Email FROM [User] WHERE Email='" + strEmail + "'");
                                if (dtUsers.Rows.Count > 0 && strEmail.Trim()!="")
                                {
                                    lblMsg.Text = "A user has this email(" + strEmail + ") address, please try another email address.";
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('A user has this email(" + strEmail + ") address, please try another email address.');", true);
                                    return false;
                                }
                            }
                        }




                        int iNewRecordID = RecordManager.ets_Record_Insert(newRecord);
                        _iNewRecordID = iNewRecordID;

                        //check linked column
                        DataTable dtLinkedColumns = Common.DataTableFromText(@"SELECT ColumnID,SystemName FROM [Column]
                            WHERE TableID=" + _theTable.TableID.ToString() + @" AND ColumnID IN (SELECT DISTINCT LinkedParentColumnID FROM [Column] INNER JOIN [Table] ON
                            [Table].TableID=[Column].TableID WHERE [Table].AccountID=" + _theTable.AccountID.ToString() + ")");
                        if (dtLinkedColumns.Rows.Count > 0)
                        {
                            foreach (DataRow drLC in dtLinkedColumns.Rows)
                            {
                                string strLCValue = RecordManager.GetRecordValue(ref newRecord, drLC["SystemName"].ToString());
                                if (strLCValue == null || strLCValue.Trim() == "")
                                {
                                    //Guid newGUID=Guid.NewGuid();
                                    Record tempRecord = RecordManager.ets_Record_Detail_Full(_iNewRecordID);
                                    RecordManager.MakeTheRecord(ref tempRecord, drLC["SystemName"].ToString(), tempRecord.RecordID.ToString());
                                    RecordManager.ets_Record_Update(tempRecord, null);
                                }
                            }
                        }

                        RecordManager.ets_Record_Avg_ForARecordID(iNewRecordID);


                        AutoCreateUser();

                        _strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&TableID=" + TableID.ToString() + "&Recordid=" + Cryptography.Encrypt(iNewRecordID.ToString());

                        //hlEdit.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + TableID.ToString() + "&Recordid=" + Cryptography.Encrypt(iNewRecordID.ToString()) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&FromAdd=yes";
                        //now send emails

                        if (bMaxTime)
                        {
                            string strTemp = "";

                            RecordManager.SendMaxTimeWanrningSMSandEmail((int)_theTable.TableID, newRecord.DateTimeRecorded.ToString(), _iSessionAccountID, ref strTemp, _strURL);
                            if (strTemp != "")
                            {
                                lblMsg.Text = lblMsg.Text + " " + strTemp;
                            }
                        }


                        if (bDataWarning)
                        {
                            for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                            {
                                strValue = "";
                                if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() == ""
                                    && _dtColumnsDetail.Rows[i]["TableTableID"] == DBNull.Value)
                                {
                                    if (_txtValue[i] != null)
                                        strValue = _txtValue[i].Text;
                                }
                                else if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                                    && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                                    || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                                {
                                    if (_ddlValue[i].SelectedIndex != 0)
                                    {
                                        strValue = _ddlValue[i].Text;
                                    }
                                }

                                if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"] == DBNull.Value)
                                {
                                    //do nothing
                                }
                                else
                                {
                                    if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                                    {
                                        if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), ref strValidationError))
                                        {

                                            string strTemp = "";
                                            //RecordManager.SendDataWanrningSMSandEmail(int.Parse(_dtRecordTypleColumlns.Rows[i]["ColumnID"].ToString()), strValue, newRecord.DateTimeRecorded.ToString(), ref strTemp, _iSessionAccountID, _strURL);
                                            RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), strValue, newRecord.DateTimeRecorded.ToString(),
                                                ref strTemp, _iSessionAccountID, _strURL, ref strEmailFullBody, ref strSMSFullBody, ref iWarningColumnCount);

                                        }
                                        else
                                        {

                                        }
                                    }
                                }

                            }



                            for (int i = 0; i < _dtColumnsNotDetail.Rows.Count; i++)
                            {
                                if (_dtColumnsNotDetail.Rows[i]["ValidationOnWarning"] == DBNull.Value)
                                {
                                    //do nothing
                                }
                                else
                                {
                                    if (_dtColumnsNotDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                                    {
                                        if (RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()) != null)
                                            if (!UploadManager.IsDataValid(RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()), _dtColumnsNotDetail.Rows[i]["ValidationOnWarning"].ToString(), ref strValidationError))
                                            {

                                                string strTemp = "";
                                                //RecordManager.SendDataWanrningSMSandEmail(int.Parse(_dtColumnsNotDetail.Rows[i]["ColumnID"].ToString()), RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()), newRecord.DateTimeRecorded.ToString(), ref strTemp, _iSessionAccountID, _strURL);

                                                RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsNotDetail.Rows[i]["ColumnID"].ToString()), RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()), newRecord.DateTimeRecorded.ToString(),
                                                    ref strTemp, _iSessionAccountID, _strURL, ref strEmailFullBody, ref strSMSFullBody, ref iWarningColumnCount);

                                            }

                                    }
                                }

                            }


                        }





                        //now check Records exceeded

                        //if (SecurityManager.IsRecordsExceeded(_iSessionAccountID))
                        //{
                        //    Session["DoNotAllow"] = "true";
                        //    //if (!IsPostBack)
                        //    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Problem" + _strDynamictabPart, "alert('Sorry you have reached the limit of your account.');window.location.href='" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountTypeChange.aspx?type=renew" + "'", true);
                        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DoNotAllow" + _strDynamictabPart, "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);

                        //    return false;
                        //}



                        break;

                    case "view":


                        break;

                    case "edit":
                        //Record editRecord = new Record();
                        //editRecord.RecordID = RecordID;
                        //editRecord.DateTimeRecorded = DateTime.Now;


                        strValue = "";

                        Record editRecord = RecordManager.ets_Record_Detail_Full((int)RecordID);

                        //Record originalRecord = RecordManager.ets_Record_Detail_Full(RecordID);

                        //editRecord.ChangeReason = txtReasonForChange.Text.Trim();



                        _strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/" + _strRecordFolder + "/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&TableID=" + TableID.ToString() + "&Recordid=" + Cryptography.Encrypt(RecordID.ToString());

                        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                        {

                            //if (i == _iLocationIndex)
                            //{


                            //    RecordManager.MakeTheRecord(ref editRecord, _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString(), _ddlLocation.SelectedValue);

                            //}
                            //else
                            if (i == _iEnteredByIndex)
                            {
                                RecordManager.MakeTheRecord(ref editRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _ddlEnteredBy.SelectedValue);
                            }
                            else if (i == _iIsActiveIndex)
                            {
                                RecordManager.MakeTheRecord(ref editRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _chkIsActive.Checked);
                            }

                            else if (i == _iDateTimeRecorded)
                            {
                                string strDateTime;
                                if (_txtValue[i].Text == "")
                                {
                                    strDateTime = DateTime.Now.ToShortDateString() + " 00:00";
                                }
                                else
                                {
                                    string strTimePart = "";
                                    if (_txtTime[i] != null)
                                    {
                                        if (_txtTime[i].Text == "")
                                        {
                                            strTimePart = "00:00";
                                        }
                                        else
                                        {
                                            if (_txtTime[i].Text.ToLower().IndexOf(":am") > 0)
                                            {
                                                strTimePart = _txtTime[i].Text.ToLower().Replace(":am", ":00");
                                            }
                                            else
                                            {
                                                strTimePart = _txtTime[i].Text.ToLower().Replace(":pm", ":00");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strTimePart = "00:00";
                                    }

                                    strDateTime = _txtValue[i].Text + " " + strTimePart;
                                }

                                //RecordManager.MakeTheRecord(ref newRecord, _dtRecordTypleColumlns.Rows[i]["SystemName"].ToString(), _txtValue[i].Text);
                                RecordManager.MakeTheRecord(ref editRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strDateTime);
                            }
                            else
                            {




                                //perform caculation here
                                //if (_dtRecordTypleColumlns.Rows[i]["Calculation"] != DBNull.Value)
                                //{
                                //    if (_dtRecordTypleColumlns.Rows[i]["Calculation"].ToString().Length > 0)
                                //    {
                                //        //replace params by value
                                //        string strFormula = _dtRecordTypleColumlns.Rows[i]["Calculation"].ToString().ToLower();

                                //        for (int j = 0; j < _dtRecordTypleColumlns.Rows.Count; j++)
                                //        {
                                //            if (_radioList[j] != null)
                                //            {
                                //                if (_radioList[j].SelectedItem != null)
                                //                    strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_radioList[j].SelectedValue));
                                //            }

                                //            if (_ddlValue[j] != null)
                                //            {
                                //                if (_dtRecordTypleColumlns.Rows[j]["ColumnType"].ToString() == "dropdown"
                                //                        && (_dtRecordTypleColumlns.Rows[j]["DropDownType"].ToString() == "table"
                                //                        || _dtRecordTypleColumlns.Rows[j]["DropDownType"].ToString() == "tabledd")
                                //                        && _dtRecordTypleColumlns.Rows[j]["TableTableID"] != DBNull.Value
                                //                         && _dtRecordTypleColumlns.Rows[j]["LinkedParentColumnID"] != DBNull.Value
                                //                        && _dtRecordTypleColumlns.Rows[j]["DisplayColumn"].ToString() != ""
                                //                        )
                                //                {
                                //                    if (_ddlValue[j].SelectedItem != null)
                                //                        strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_ddlValue[j].SelectedValue));
                                //                }

                                //            }

                                //            if (_txtValue[j] != null)
                                //            {

                                //                if ((_dtRecordTypleColumlns.Rows[j]["ColumnType"].ToString() == "datetime"
                                //                    || _dtRecordTypleColumlns.Rows[j]["ColumnType"].ToString() == "date"
                                //                    || _dtRecordTypleColumlns.Rows[j]["ColumnType"].ToString() == "time")
                                //                    && (_txtValue[j].Text.IndexOf("/") > -1
                                //                    || _txtValue[j].Text.IndexOf(":") > -1)
                                //                    && _dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
                                //                {

                                //                    DateTime dtValue = DateTime.Today;
                                //                    if (_txtValue[j].Text.IndexOf("/") > -1)
                                //                    {
                                //                        //dtValue = DateTime.ParseExact(_txtValue[j].Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);
                                //                        if (DateTime.TryParseExact(_txtValue[j].Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtValue))
                                //                        {
                                //                            _txtValue[j].Text = dtValue.ToShortDateString();
                                //                        }
                                //                    }
                                //                    if (_txtValue[j].Text.IndexOf(":") > -1)
                                //                        dtValue = DateTime.ParseExact(dtValue.Date.ToShortDateString() + " " + _txtValue[j].Text, "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                                //                    if (_txtTime[j] != null)
                                //                    {
                                //                        if (_txtTime[j].Text != "")
                                //                        {
                                //                            //dtValue = dtValue + TimeSpan.ParseExact(_txtTime[j].Text,"HH:m",CultureInfo.InvariantCulture);
                                //                            dtValue = DateTime.ParseExact(_txtValue[j].Text + " " + _txtTime[j].Text, "d/M/yyyy HH:m", CultureInfo.InvariantCulture);
                                //                        }
                                //                    }
                                //                    strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", dtValue.Ticks.ToString());
                                //                }
                                //                else
                                //                {
                                //                    if (_dtRecordTypleColumlns.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                                //                    {
                                //                        strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_txtValue[j].Text));
                                //                    }
                                //                    else
                                //                    {
                                //                        strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", _txtValue[j].Text);
                                //                    }
                                //                }
                                //            }
                                //        }



                                //        for (int j = 0; j < _dtColumnsNotDetail.Rows.Count; j++)
                                //        {
                                //            if (RecordManager.GetRecordValue(ref editRecord, _dtColumnsNotDetail.Rows[j]["SystemName"].ToString()) != null)
                                //            {
                                //                string strGetRecordValue = RecordManager.GetRecordValue(ref editRecord, _dtColumnsNotDetail.Rows[j]["SystemName"].ToString());

                                //                if ((_dtColumnsNotDetail.Rows[j]["ColumnType"].ToString() == "datetime"
                                //                    || _dtColumnsNotDetail.Rows[j]["ColumnType"].ToString() == "date"
                                //                    || _dtColumnsNotDetail.Rows[j]["ColumnType"].ToString() == "time")
                                //                    && (strGetRecordValue.IndexOf("/") > -1
                                //                    || strGetRecordValue.IndexOf(":") > -1)
                                //                    && _dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
                                //                {

                                //                    DateTime dtValue = DateTime.Today;

                                //                    if (strGetRecordValue.IndexOf("/") > -1 && strGetRecordValue.IndexOf(":") == -1)
                                //                    {
                                //                        //dtValue = DateTime.ParseExact(strGetRecordValue + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture);

                                //                        if (DateTime.TryParseExact(strGetRecordValue, Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtValue))
                                //                        {
                                //                            strGetRecordValue = dtValue.ToShortDateString();
                                //                        }
                                //                    }
                                //                    if (strGetRecordValue.IndexOf("/") > -1 && strGetRecordValue.IndexOf(":") > -1)
                                //                        dtValue = DateTime.ParseExact(strGetRecordValue, "d/M/yyyy HH:m", CultureInfo.InvariantCulture);

                                //                    if (strGetRecordValue.IndexOf("/") == -1 && strGetRecordValue.IndexOf(":") > -1)
                                //                    {
                                //                        dtValue = DateTime.ParseExact(dtValue.Date.ToShortDateString() + " " + strGetRecordValue, "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                //                    }
                                //                    strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", dtValue.Ticks.ToString());

                                //                }
                                //                else
                                //                {

                                //                    if (_dtColumnsNotDetail.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                                //                    {
                                //                        strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.MakeDecimal(Common.IgnoreSymbols(strGetRecordValue)));
                                //                    }
                                //                    else
                                //                    {
                                //                        strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.MakeDecimal(strGetRecordValue));
                                //                    }
                                //                }
                                //            }

                                //        }


                                //        string strResult = RecordManager.CalculationResult(strFormula);
                                //        if (Common.IsThisDouble(strResult) == false)
                                //        {
                                //            strResult = "";
                                //        }
                                //        else
                                //        {
                                //            if (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"] != DBNull.Value)
                                //            {

                                //                double lResult = double.Parse(strResult);

                                //                switch (_dtRecordTypleColumlns.Rows[i]["DateCalculationType"].ToString())
                                //                {
                                //                    case "year":
                                //                        lResult = lResult / (365 * 24 * 60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;

                                //                    case "month":
                                //                        //strResult = (lResult / (30 * 24 * 60 * 60)).ToString();
                                //                        lResult = lResult / (30 * 24 * 60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();

                                //                        break;

                                //                    case "day":
                                //                        //strResult = (lResult / (24 * 60 * 60)).ToString();
                                //                        lResult = lResult / (24 * 60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;

                                //                    case "week":
                                //                        //strResult = (lResult / (7 * 24 * 60 * 60)).ToString();
                                //                        lResult = lResult / (7 * 24 * 60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;
                                //                    case "hour":
                                //                        //strResult = (lResult / ( 60 * 60)).ToString();
                                //                        lResult = lResult / (60 * 60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;
                                //                    case "minute":
                                //                        //strResult = (lResult / ( 60)).ToString();
                                //                        lResult = lResult / (60);
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;
                                //                    case "second":
                                //                        //strResult = lResult.ToString();                                                         
                                //                        lResult = lResult / 10000000;
                                //                        strResult = ((int)lResult).ToString();
                                //                        break;


                                //                    default:
                                //                        break;
                                //                }


                                //                //}
                                //            }


                                //        }

                                //        _txtValue[i].Text = strResult;
                                //        strValue = _txtValue[i].Text;
                                //    }
                                //}



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
                                            }
                                            catch
                                            {
                                                //
                                            }
                                            strValue = _txtValue[i].Text;
                                        }
                                    }

                                }

                                strValue = "";


                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                                {
                                    string strDateTime = "";
                                    if (_txtValue[i].Text.Trim() == "")
                                    {
                                        //strDateTime = DateTime.Now.ToShortDateString() + " 12:00:00 AM";
                                    }
                                    else
                                    {
                                        DateTime dtTemp;
                                        if (DateTime.TryParseExact(_txtValue[i].Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                        {
                                            _txtValue[i].Text = dtTemp.ToShortDateString();
                                        }

                                        string strTimePart = "";
                                        if (_txtTime[i] != null)
                                        {
                                            if (_txtTime[i].Text == "")
                                            {
                                                strTimePart = " 00:00";
                                            }
                                            else
                                            {
                                                if (_txtTime[i].Text.ToLower().IndexOf(":am") > 0)
                                                {
                                                    strTimePart = _txtTime[i].Text.ToLower().Replace(":am", ":00");
                                                }
                                                else
                                                {
                                                    strTimePart = _txtTime[i].Text.ToLower().Replace(":pm", ":00");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strTimePart = " 12:00:00 AM";
                                        }

                                        strDateTime = _txtValue[i].Text + " " + strTimePart;
                                    }
                                    strValue = strDateTime;

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                                {
                                    string strDateTime = "";
                                    if (_txtValue[i].Text == "")
                                    {
                                        // strDateTime = DateTime.Now.ToShortDateString();// +" 12:00:00 AM";
                                    }
                                    else
                                    {
                                        DateTime dtTemp;
                                        if (DateTime.TryParseExact(_txtValue[i].Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                        {
                                            _txtValue[i].Text = dtTemp.ToShortDateString();
                                        }
                                        strDateTime = _txtValue[i].Text;// +" " + "12:00:00 AM";
                                    }
                                    strValue = strDateTime;


                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                                {
                                    strValue = _txtValue[i].Text;

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                                    || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                                {
                                    if (_hfValue2[i].Value == "yes" && _fuValue2[i].HasFile)
                                    {
                                        //string strFolder = "~\\UserFiles\\AppFiles";
                                        string strFolder = "\\UserFiles\\AppFiles";
                                        string strFileName = _fuValue2[i].FileName;
                                        string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;// +"." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                                        //string strPath = Server.MapPath(strFolder + "\\" + strUniqueName);
                                        string strPath = _strFilesPhisicalPath + strFolder + "\\" + strUniqueName;
                                        _fuValue2[i].SaveAs(strPath);
                                        _hfValue[i].Value = strUniqueName;
                                    }

                                    strValue = _hfValue[i].Value;
                                }

                                //if (_dtRecordTypleColumlns.Rows[i]["ColumnType"].ToString() == "slider")
                                //{
                                //    strValue = _txtValue[i].Text;
                                //}
                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "text"
                                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                                {
                                    strValue = _txtValue[i].Text;

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                                    && _dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                                    && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                                    || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                                {
                                    if (_ddlValue[i].SelectedIndex != 0)
                                    {
                                        strValue = _ddlValue[i].Text;
                                    }
                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                                {
                                    if (_radioList[i].SelectedItem != null)
                                    {
                                        strValue = _radioList[i].SelectedItem.Value;
                                    }
                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                                {

                                    strValue = Common.GetCheckBoxValue(_dtColumnsDetail.Rows[i]["DropDownValues"].ToString(), ref _chkValue[i]);

                                }


                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                                {
                                    LocationColumn newLocationColumn = new LocationColumn();
                                    if (_hfValue3[i] != null)
                                    {
                                        if (_hfValue3[i].Value == "")
                                            _hfValue3[i].Value = "5";

                                        newLocationColumn.ZoomLevel = int.Parse(_hfValue3[i].Value);

                                        if (_txtValue[i] != null && _txtTime[i] != null)
                                        {
                                            if (_txtValue[i].Text != "")
                                                newLocationColumn.Latitude = _txtValue[i].Text;

                                            if (_txtTime[i].Text != "")
                                                newLocationColumn.Longitude = _txtTime[i].Text;

                                        }
                                        else
                                        {
                                            if (_hfValue[i].Value != "")
                                                newLocationColumn.Latitude = _hfValue[i].Value;

                                            if (_hfValue2[i].Value != "")
                                                newLocationColumn.Longitude = _hfValue2[i].Value;
                                        }

                                        if (_txtValue2[i] != null)
                                        {
                                            if (_txtValue2[i].Text != "")
                                                newLocationColumn.Address = _txtValue2[i].Text;
                                        }

                                    }
                                    strValue = newLocationColumn.GetJSONString();
                                }




                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "content")
                                {

                                    strValue = _htmValue[i].Text;

                                }
                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "staticcontent")
                                {

                                    strValue = "";

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                                    && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "")
                                {
                                    if (_lstValue[i].SelectedItem != null)
                                    {
                                        strValue = Common.GetListValues(_lstValue[i]);
                                    }
                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                                  && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "checkbox")
                                {
                                    if (_cblValue[i].SelectedItem != null)
                                    {
                                        strValue = Common.GetCheckBoxListValues(_cblValue[i]);
                                    }
                                }


                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown" &&
                                      _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                                      && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != ""
                                   && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                                   || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd"))
                                {
                                    if (_dtColumnsDetail.Rows[i]["ParentColumnID"] == DBNull.Value)
                                    {
                                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                                        {
                                            strValue = _hfValue[i].Value;
                                        }
                                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                                        {
                                            if (_ddlValue[i].SelectedItem != null)
                                                strValue = _ddlValue[i].SelectedValue;
                                        }
                                    }
                                    else
                                    {
                                        if (_ddlValue[i].SelectedItem != null)
                                            strValue = _ddlValue[i].SelectedValue;
                                    }

                                }




                                RecordManager.MakeTheRecord(ref editRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strValue);


                                if (strValue.Length > 0)
                                {
                                    if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"] == DBNull.Value)
                                    {
                                        //do nothing
                                    }
                                    else if (DoValidation == true)
                                    {
                                        if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                                        {
                                            if (UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString(), ref strValidationError))
                                            {
                                                //strValidationResults = strValidationResults + "\n" + _dtRecordTypleColumlns.Rows[i]["DisplayTextDetail"].ToString() + " - " + "VALID";
                                            }
                                            else
                                            {
                                                lblMsg.Text = "Invalid data - " + _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString();
                                                if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() == ""
                                                    && _dtColumnsDetail.Rows[i]["TableTableID"] == DBNull.Value)
                                                {
                                                    _txtValue[i].Focus();
                                                }
                                                else if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                                                   && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                                                    || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                                                {
                                                    _ddlValue[i].Focus();
                                                }
                                                return false;
                                            }
                                        }
                                    }

                                    if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"] == DBNull.Value)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                                        {
                                            if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), ref strValidationError))
                                            {
                                                strWarningResults = strWarningResults + " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
                                                //bWarning = true;
                                                //iWarning = i;
                                                string strTemp = "";



                                                if (ViewState["ok"] != null)
                                                {
                                                    if (ViewState["ok"].ToString() != "no")
                                                    {

                                                        //RecordManager.SendDataWanrningSMSandEmail(int.Parse(_dtRecordTypleColumlns.Rows[i]["ColumnID"].ToString()), strValue, editRecord.DateTimeRecorded.ToString(), ref strTemp, _iSessionAccountID, _strURL);
                                                        RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), strValue, editRecord.DateTimeRecorded.ToString(),
                                                            ref strTemp, _iSessionAccountID, _strURL, ref strEmailFullBody, ref strSMSFullBody, ref iWarningColumnCount);

                                                    }
                                                }

                                                if (strTemp != "")
                                                {
                                                    lblMsg.Text = lblMsg.Text + " " + strTemp;
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }

                                    //check SD
                                    if (bool.Parse(_dtColumnsDetail.Rows[i]["CheckUnlikelyValue"].ToString()))
                                    {
                                        int? iCount = RecordManager.ets_Table_GetCount((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), (int)RecordID);
                                        if (iCount != null)
                                        {
                                            if (iCount >= Common.MinSTDEVRecords)
                                            {
                                                string strRecordedate;
                                                if (_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
                                                {
                                                    strRecordedate = Common.IgnoreSymbols(_txtValue[i].Text);
                                                }
                                                else
                                                {
                                                    strRecordedate = _txtValue[i].Text;
                                                }

                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), (int)RecordID);

                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), (int)RecordID);

                                                double dRecordedate = double.Parse(strRecordedate);
                                                if (dAVG != null && dSTDEV != null)
                                                {
                                                    dSTDEV = dSTDEV * 3;
                                                    if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
                                                    {
                                                        //deviation happaned
                                                        strWarningResults = strWarningResults + " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
                                                    }

                                                }
                                            }
                                        }

                                    }
                                    /// 

                                }//
                            }
                        }







                        //perform calculation that are not in detail page

                        //if (_dtColumnsNotDetail.Rows.Count > 0)
                        //{
                        //    for (int x = 0; x < _dtColumnsNotDetail.Rows.Count; x++)
                        //    {
                        //        if (_dtColumnsNotDetail.Rows[x]["IsStandard"].ToString().ToLower() == "false")
                        //        {
                        //            strValue = "";
                        //            if (_dtColumnsNotDetail.Rows[x]["Calculation"].ToString().Length > 0)
                        //            {
                        //                //ops we got a column that need to be calculated
                        //                //replace params by value
                        //                string strFormula = _dtColumnsNotDetail.Rows[x]["Calculation"].ToString().ToLower();
                        //                for (int j = 0; j < _dtRecordTypleColumlns.Rows.Count; j++)
                        //                {
                        //                    if (_txtValue[j] != null)
                        //                    {
                        //                        if (_dtRecordTypleColumlns.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                        //                        {
                        //                            strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_txtValue[j].Text));
                        //                        }
                        //                        else
                        //                        {
                        //                            strFormula = strFormula.Replace("[" + _dtRecordTypleColumlns.Rows[j]["DisplayName"].ToString().ToLower() + "]", _txtValue[j].Text);
                        //                        }
                        //                    }
                        //                }



                        //                for (int j = 0; j < _dtColumnsNotDetail.Rows.Count; j++)
                        //                {
                        //                    if (RecordManager.GetRecordValue(ref editRecord, _dtColumnsNotDetail.Rows[j]["SystemName"].ToString()) != null)
                        //                    {
                        //                        if (_dtColumnsNotDetail.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                        //                        {
                        //                            strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]",
                        //                               Common.MakeDecimal(Common.IgnoreSymbols(RecordManager.GetRecordValue(ref editRecord, _dtColumnsNotDetail.Rows[j]["SystemName"].ToString()))));
                        //                        }
                        //                        else
                        //                        {
                        //                            strFormula = strFormula.Replace("[" + _dtColumnsNotDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]",
                        //                              Common.MakeDecimal(RecordManager.GetRecordValue(ref editRecord, _dtColumnsNotDetail.Rows[j]["SystemName"].ToString())));
                        //                        }
                        //                    }

                        //                }




                        //                string strResult = RecordManager.CalculationResult(strFormula);

                        //                if (Common.IsThisDouble(strResult) == false)
                        //                {
                        //                    strResult = "";
                        //                }

                        //                RecordManager.MakeTheRecord(ref editRecord, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), strResult);

                        //                strValue = strResult;

                        //                if (strValue.Length > 0)
                        //                {

                        //                    if (_dtColumnsNotDetail.Rows[x]["IsRound"] != DBNull.Value)
                        //                    {
                        //                        if (_dtColumnsNotDetail.Rows[x]["IsRound"].ToString().ToLower() == "true")
                        //                        {
                        //                            if (strValue.ToString() != "")
                        //                            {
                        //                                strValue = Math.Round(double.Parse(strValue), int.Parse(_dtColumnsNotDetail.Rows[x]["RoundNumber"].ToString())).ToString();
                        //                            }
                        //                        }

                        //                    }

                        //                    RecordManager.MakeTheRecord(ref editRecord, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), strValue);



                        //                    if (_dtColumnsNotDetail.Rows[x]["ValidationOnWarning"] == DBNull.Value)
                        //                    {
                        //                        //do nothing
                        //                    }
                        //                    else
                        //                    {
                        //                        if (_dtColumnsNotDetail.Rows[x]["ValidationOnWarning"].ToString().Length > 0)
                        //                        {
                        //                            if (UploadManager.IsDataValid(strValue, _dtColumnsNotDetail.Rows[x]["ValidationOnWarning"].ToString(), ref strValidationError, bool.Parse(_dtColumnsNotDetail.Rows[x]["IgnoreSymbols"].ToString())))
                        //                            {
                        //                                strWarningResults = strWarningResults + " WARNING: " + _dtColumnsNotDetail.Rows[x]["DisplayName"].ToString() + " – Value outside accepted range.";
                        //                                //bWarning = true;
                        //                                //iWarning = i;
                        //                                string strTemp = "";

                        //                                if (ViewState["ok"] != null)
                        //                                {
                        //                                    if (ViewState["ok"].ToString() != "no")
                        //                                    {

                        //                                        //RecordManager.SendDataWanrningSMSandEmail(int.Parse(_dtColumnsNotDetail.Rows[x]["ColumnID"].ToString()), strValue, editRecord.DateTimeRecorded.ToString(), ref strTemp, _iSessionAccountID, _strURL);

                        //                                        RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsNotDetail.Rows[x]["ColumnID"].ToString()), strValue, editRecord.DateTimeRecorded.ToString(),
                        //                                            ref strTemp, _iSessionAccountID, _strURL, ref strEmailFullBody, ref strSMSFullBody, ref iWarningColumnCount);


                        //                                    }
                        //                                }

                        //                                if (strTemp != "")
                        //                                {
                        //                                    lblMsg.Text = lblMsg.Text + " " + strTemp;
                        //                                }
                        //                            }
                        //                            else
                        //                            {

                        //                            }
                        //                        }
                        //                    }

                        //                    //check SD
                        //                    if (bool.Parse(_dtColumnsNotDetail.Rows[x]["CheckUnlikelyValue"].ToString()))
                        //                    {
                        //                        int? iCount = RecordManager.ets_Table_GetCount((int)_theTable.TableID, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), (int)RecordID);
                        //                        if (iCount != null)
                        //                        {
                        //                            if (iCount >= Common.MinSTDEVRecords)
                        //                            {
                        //                                string strRecordedate;
                        //                                if (_dtColumnsNotDetail.Rows[x]["IgnoreSymbols"].ToString().ToLower() == "true")
                        //                                {
                        //                                    strRecordedate = Common.IgnoreSymbols(strValue);
                        //                                }
                        //                                else
                        //                                {
                        //                                    strRecordedate = strValue;
                        //                                }

                        //                                double? dAVG = RecordManager.ets_Table_GetAVG((int)_theTable.TableID, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), (int)RecordID);

                        //                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)_theTable.TableID, _dtColumnsNotDetail.Rows[x]["SystemName"].ToString(), (int)RecordID);

                        //                                double dRecordedate = double.Parse(strRecordedate);
                        //                                if (dAVG != null && dSTDEV != null)
                        //                                {
                        //                                    dSTDEV = dSTDEV * 3;
                        //                                    if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
                        //                                    {
                        //                                        //deviation happaned
                        //                                        strWarningResults = strWarningResults + " WARNING: " + _dtColumnsNotDetail.Rows[x]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
                        //                                    }

                        //                                }
                        //                            }
                        //                        }

                        //                    }



                        //                }
                        //            }

                        //        }

                        //    }

                        //}
                        ///


                        editRecord.TableID = TableID;


                        if (!RecordManager.IsTimeBetweenRecordOK((int)editRecord.TableID, (int)editRecord.RecordID, (DateTime)editRecord.DateTimeRecorded))
                        {
                            strWarningResults = strWarningResults + " WARNING: " + WarningMsg.MaxtimebetweenRecords + "!";
                            string strTemp = "";


                            if (ViewState["ok"] != null)
                            {
                                if (ViewState["ok"].ToString() != "no")
                                {

                                    RecordManager.SendMaxTimeWanrningSMSandEmail((int)_theTable.TableID, editRecord.DateTimeRecorded.ToString(), _iSessionAccountID, ref strTemp, _strURL);
                                }
                            }


                            if (strTemp != "")
                            {
                                lblMsg.Text = lblMsg.Text + " " + strTemp;
                            }
                        }



                        editRecord.WarningResults = strWarningResults.Trim();


                        if (editRecord.EnteredBy == null)
                        {
                            editRecord.EnteredBy = _objUser.UserID;
                        }

                        editRecord.LastUpdatedUserID = _objUser.UserID;


                        //check duplicate
                        //if ((bool)_theTable.IsRecordDateUnique && DoValidation)
                        //{

                        //    if (RecordManager.ets_Record_IsDuplicate_Entry((int)editRecord.TableID, (DateTime)editRecord.DateTimeRecorded, (int)editRecord.RecordID))
                        //    {
                        //        lblMsg.Text = "Duplicate Record!";
                        //        return false;
                        //    }

                        //}










                        if (editRecord.WarningResults != "" && editRecord.WarningResults != null)
                        {
                            _lblWarningResults.Visible = true;
                            _lblWarningResultsValue.Text = editRecord.WarningResults.ToString();
                        }
                        else
                        {
                            _lblWarningResults.Visible = false;
                        }

                        //if (ViewState["ok"] != null)
                        //{
                        //    if (ViewState["ok"].ToString() == "no")
                        //    {
                        //        if (editRecord.WarningResults != null)
                        //        {
                        //            if (editRecord.WarningResults.ToString().Trim().Length > 0)
                        //            {
                        //                lblMsg.Text = "The values entered have caused warnings – would you like to continue?";
                        //                trConfirmation.Visible = true;
                        //                trMainSave.Visible = false;
                        //                return false;
                        //            }
                        //        }
                        //    }

                        //}
                        //else
                        //{
                        //    if (editRecord.WarningResults != null)
                        //    {
                        //        if (editRecord.WarningResults.ToString().Trim().Length > 0)
                        //        {
                        //            lblMsg.Text = "The values entered have caused warnings – would you like to continue?";
                        //            trConfirmation.Visible = true;
                        //            trMainSave.Visible = false;
                        //            return false;
                        //        }
                        //    }

                        //}

                        //if (originalRecord != editRecord)
                        //{
                        editRecord.ChangeReason = txtReasonForChange.Text.Trim();
                        //}

                        //check linked column

                        DataTable dtLinkedColumns2 = Common.DataTableFromText(@"SELECT ColumnID,SystemName FROM [Column]
                            WHERE TableID=" + _theTable.TableID.ToString() + @" AND ColumnID IN (SELECT DISTINCT LinkedParentColumnID FROM [Column] INNER JOIN [Table] ON
                            [Table].TableID=[Column].TableID WHERE [Table].AccountID=" + _theTable.AccountID.ToString() + ")");
                        if (dtLinkedColumns2.Rows.Count > 0)
                        {
                            foreach (DataRow drLC in dtLinkedColumns2.Rows)
                            {
                                string strLCValue = RecordManager.GetRecordValue(ref editRecord, drLC["SystemName"].ToString());
                                if (strLCValue.Trim() == "")
                                {
                                    //Guid newGUID = Guid.NewGuid();
                                    RecordManager.MakeTheRecord(ref editRecord, drLC["SystemName"].ToString(), editRecord.RecordID.ToString());
                                }
                            }
                        }


                        int iIsUpdated = RecordManager.ets_Record_Update(editRecord, null);

                        RecordManager.ets_Record_Avg_ForARecordID((int)editRecord.RecordID);

                        SaveOK = true;
                        return true;

                        //HB Start


                        //HB End





                        break;

                    default:
                        //?
                        break;
                }


                if (iWarningColumnCount > 0)
                {
                    string strError = "";
                    string strRecordID = "";
                    if (_qsRecord != null)
                    {
                        strRecordID = _qsRecord.RecordID.ToString();
                    }
                    else
                    {
                        strRecordID = _iNewRecordID.ToString();
                    }

                    RecordManager.SendDataWanrningSMSandEmailBatch(TableID, ref strError, _iSessionAccountID,
                        strEmailFullBody, strSMSFullBody, iWarningColumnCount, int.Parse(strRecordID));

                    if (strError != "")
                    {
                        lblMsg.Text = strError;
                        return false;
                    }
                }


            }
            else
            {
                //user input is not ok

            }
            ViewState["ok"] = "no";
            return true;
            //Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            ViewState["ok"] = "no";
            lblMsg.Text = ex.Message;
            return false;
        }

    }








}



