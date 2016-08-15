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
using System.Xml;

public partial class Record_Record_Detail : System.Web.UI.Page//SecurePage
{
    bool _bShowExceedances = false;
    string _strWarningResults = "";
    string _strExceedanceResults = "";
    string _strInValidResults = "";
    //bool _bDataWarning = false;
    //bool _bDataExceedance = false;


    string _strValidationError = "";

    string _strWarningEmailFullBody = "";
    string _strWarningSMSFullBody = "";

    string _strExceedanceEmailFullBody = "";
    string _strExceedanceSMSFullBody = "";

    int _iWarningColumnCount = 0;
    int _iExceedanceColumnCount = 0;


    bool _bCheckIgnoreMidnight = false;
    bool _bCopyRecord = false;
    bool _bNeedFullPostback = false;
    int? _iParentRecordID = null;
    int? _iViewID = null;
    View _theView = null;
    string _strListType = "";
    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";
    string _strSaveAndStay = "no";
    bool _bRedirect = true;
    bool _bCustomDDL = false;
    bool _bPrivate = true;
    string _strPreProgress = "-1";
    bool _bLabelOnTop = false;
    //bool _bUsePublicMasterPage = false;
    //private Pages_UserControl_TableList[] oneCTList;
    private DataTable _dtDBTableTab;
    bool _bTableTabYes = false;
    Account _theAccount;
    Panel[] _pnlDetailTabD;
    HtmlTable[] _tblMainD;
    HtmlTable[] _tblLeftD;
    HtmlTable[] _tblRightD;

    private Pages_UserControl_RecordList[] oneCTList;
    //private Pages_UserControl_DetailView[] oneCTDetail;
    private Pages_UserControl_DetailEdit[] oneCTDetail;
    private Pages_UserControl_MessageList oneMLList;
    //private Panel[] divPanel;
    int _TabIndex = 0;

    string _strActionMode = "view";
    int _iRecordID;
    Record _theRecord = null;
    int _iNewRecordID = -1;
    Label[] _lbl;
    TextBox[] _txtValue;
    TextBox[] _txtValue2;
    LinkButton[] _lnkValue;
    HyperLink[] _hlValue;
    HyperLink[] _hlValue2;
    ImageButton[] _ibValue;
    //HtmlEditorExtender[] _heeValue;
    WYSIWYGEditor[] _htmValue;

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
    RangeValidator[] _rvDate;
    TextBoxWatermarkExtender[] _twmValue;
    AjaxControlToolkit.MaskedEditExtender[] _meeTime;
    CustomValidator[] _cvTime;

    //int _iLocationIndex = -1;
    int _iTableIndex = -1;
    int _iDateTimeRecorded = -1;
    int _iEnteredByIndex = -1;
    int _iIsActiveIndex = -1;
    User _objUser;
    UserRole _theUserRole;
    string _qsMode = "";
    string _qsTableID = "";
    string _qsRecordID = "";
    Table _theTable;
    string _strRecordRightID = Common.UserRoleType.None;
    string _strURL;

    Common_Pager _gvCL_Pager;
    int _iCLColumnCount = 0;


    int _iCLStartIndex = 0;
    int _iCLMaxRows = 0;
    int _iCLTN = 0;
    //string _strSessionRoleType = "";
    int _iSessionAccountID = -1;

    int _iRowCount = 0;

    bool _bCancelSave = false;
    protected void Page_LoadComplete(object sender, EventArgs e)
    {

        if (_iSessionAccountID == -1)
        {
            if (Session["User"] == null)
            {
                return;
            }
        }

        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " -Detail -  Page_LoadComplete ";
            theSpeedLog.FunctionLineNumber = 33;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }
    }




    protected override void OnSaveStateComplete(EventArgs e)
    {
        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " -Detail - OnSaveStateComplete ";
            theSpeedLog.FunctionLineNumber = 270;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }
    }

    protected override void OnPreInit(EventArgs e)
    {
        if (Request.QueryString["CopyRecordID"] != null)
            _bCopyRecord = true;


        _strFilesLocation = Session["FilesLocation"].ToString();
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();
        _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"]);

        int iTableID = int.Parse(_qsTableID);

        _theTable = RecordManager.ets_Table_Details(iTableID);


        string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", _theTable.AccountID, _theTable.TableID);

        if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
        {
            _bShowExceedances = true;
        }

        string strIgnoreMidnight = SystemData.SystemOption_ValueByKey_Account("Time Calculation Ignore Midnight", (int)_theTable.AccountID, _theTable.TableID);

        if (strIgnoreMidnight != "" && strIgnoreMidnight.ToString().ToLower() == "yes")
        {
            _bCheckIgnoreMidnight = true;
        }


        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Parent- Init - START ";
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

            if (Session["User"] == null)
            {
                Response.Redirect("~/Login.aspx", false);

                return;
            }

            _iSessionAccountID = int.Parse(Session["AccountID"].ToString());



            _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

            if (_theAccount.MasterPage != "")
                Page.MasterPageFile = _theAccount.MasterPage;





        }



        if (_bPrivate)
        {
            _objUser = (User)Session["User"];
            _theUserRole = (UserRole)Session["UserRole"];
        }
        else
        {
            _objUser = SecurityManager.User_Details(int.Parse(SystemData.SystemOption_ValueByKey_Account("AnonymousUser", null, _theTable.TableID)));
            _theUserRole = SecurityManager.GetUserRole((int)_objUser.UserID, (int)_iSessionAccountID);
        }

        if ((bool)_theUserRole.IsAdvancedSecurity)
        {
            //DataTable dtUserTable = SecurityManager.ets_UserTable_Select(null,
            //    int.Parse(_qsTableID), _objUser.UserID, null);

            DataTable dtUserTable = null;

            //if (_objUser.RoleGroupID == null)
            //{
            dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
           int.Parse(_qsTableID), _theUserRole.RoleID, null);
            //}
            //else
            //{

            //    dtUserTable = SecurityManager.dbg_RoleGroupTable_Select((int)_objUser.RoleGroupID, null,
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
            Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Default.aspx", false);
            return;
        }




    }

    protected void Page_Init(object sender, EventArgs e)
    {


        if (_iSessionAccountID == -1)
        {
            if (Session["User"] == null)
            {
                return;
            }
        }

        //

        if (Request.QueryString["parentRecordid"] != null)
            _iParentRecordID = int.Parse(Cryptography.Decrypt(Request.QueryString["parentRecordid"].ToString()));


        //cosmetic

        if (_theTable.HeaderColor != "")
        {
            divHeaderColorDetail.Style.Add("background", "#" + _theTable.HeaderColor);
        }

        if (Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        {
            if (_theTable.HeaderColor == "")
            {
                divHeaderColorDetail.Style.Add("background", "#0089A5");
            }

            _bCustomDDL = true;
            imgSave.ImageUrl = "~/Pages/Pager/Images/rrp/save.png";
            imgBack.ImageUrl = "~/Pages/Pager/Images/rrp/arrow-left.png";
            divSaveBottom.Visible = true;
            if (_theTable.HeaderColor != "")
            {
                divSaveBottonSave.Style.Add("background", "#" + _theTable.HeaderColor);
            }
            else
            {
                divSaveBottonSave.Style.Add("background", "#0089A5");
            }
        }

        if (!IsPostBack)
        {
            if (_theTable.TabColour != "")
            {
                ltTextStyles.Text = @"<style> .DBGTab .ajax__tab_inner{ background-color: #" + _theTable.TabColour
                    + @";} .DBGTab .ajax__tab_outer{ background-color: #" + _theTable.TabColour + @";}</style>";
            }

            if (Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
            {
                ltTextStyles.Text = ltTextStyles.Text + @"<style>
                              .TopTitle
                              {
                               color:#FFFFFF;
                              }
                              #ctl00_HomeContentPlaceHolder_lblHeaderName
                              {
                                 color:#FFFFFF;
                              }
                            </style>";

                divDynamic.Style.Add("padding-left", "170px");
                divHeaderColorDetail.Style.Add("padding-left", "170px");

            }

        }


        bool bHasLetft = false;
        bool bHasRight = false;
        int iTableID = int.Parse(_qsTableID);

        if (Request.QueryString["mode"] != null)
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);
            _qsMode = _qsMode.ToLower();
        }

        _dtColumnsDetail = RecordManager.ets_Table_Columns_Detail(iTableID);
        _dtColumnsNotDetail = RecordManager.ets_Table_Columns_NotDetail(iTableID);
        _dtColumnsAll = RecordManager.ets_Table_Columns_All(iTableID);

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




        //formset & not public
        //if (!IsPostBack)
        //{
        //    if (Request.QueryString["RecordID"] == null && Request.QueryString["public"] == null)
        //    {
        //        try
        //        {
        //            string strParentTable=Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
        //            //DataTable dtFormSet = Common.DataTableFromText("SELECT DISTINCT ProgressColumnID FROM FormSet WHERE ParentTableID=" + strParentTable);
        //            DataTable dtFormSetGroup = Common.DataTableFromText("SELECT * FROM FormSetGroup WHERE ParentTableID=" + strParentTable + " ORDER BY ColumnPosition");
        //            if (dtFormSetGroup.Rows.Count > 0)
        //            {
        //                //wow this need progress history


        //                Record theRecord = new Record();
        //                theRecord.TableID = int.Parse(strParentTable);
        //                theRecord.IsActive = true;
        //                theRecord.EnteredBy = _objUser.UserID;

        //                for (int i = 0; i < _dtColumnsAll.Rows.Count; i++)
        //                {
        //                    if (_dtColumnsAll.Rows[i]["ColumnType"].ToString() == "number")
        //                    {
        //                        if (_dtColumnsAll.Rows[i]["NumberType"] != null)
        //                        {
        //                            if (_dtColumnsAll.Rows[i]["NumberType"].ToString() == "8")
        //                            {
        //                                string strValue = "1";
        //                                try
        //                                {
        //                                    string strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + _dtColumnsAll.Rows[i]["SystemName"].ToString() + ")) FROM Record WHERE TableID=" + _qsTableID);
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
        //                                RecordManager.MakeTheRecord(ref theRecord, _dtColumnsAll.Rows[i]["SystemName"].ToString(), strValue);
        //                            }

        //                        }
        //                    }

        //                }


        //                int iNewRecordID = RecordManager.ets_Record_Insert(theRecord);

        //                string strFormSetID = Common.GetValueFromSQL("SELECT top 1 FormSetID FROM FormSet WHERE FormSetGroupID IN (SELECT top 1 FormSetGroupID FROM FormSetGroup WHERE ParentTableID=" + strParentTable + " ORDER BY ColumnPosition) ORDER BY RowPosition");

        //                //FormSetManager.StartProgressHistory(int.Parse(strFormSetID), iNewRecordID);

        //                string strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetWizard.aspx?SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString()
        //            + "&FormSetID=" + Cryptography.Encrypt(strFormSetID)
        //            + "&ParentTableID=" + Cryptography.Encrypt(strParentTable)
        //            + "&ParentRecordID=" + Cryptography.Encrypt(iNewRecordID.ToString()) + "&showparent=yes&ps=0&veryfirst=yes";
        //                Response.Redirect(strURL);
        //                return;
        //            }
        //        }
        //        catch
        //        {
        //            //
        //        }
        //    }


        //}




        //Menu theMenu = RecordManager.ets_Menu_Details((int)_theTable.MenuID);


        _lbl = new Label[_dtColumnsDetail.Rows.Count];
        _txtValue = new TextBox[_dtColumnsDetail.Rows.Count];
        _txtValue2 = new TextBox[_dtColumnsDetail.Rows.Count];
        _ibValue = new ImageButton[_dtColumnsDetail.Rows.Count];
        _lnkValue = new LinkButton[_dtColumnsDetail.Rows.Count];
        _hlValue = new HyperLink[_dtColumnsDetail.Rows.Count];
        _hlValue2 = new HyperLink[_dtColumnsDetail.Rows.Count];
        //_heeValue = new HtmlEditorExtender[_dtColumnsDetail.Rows.Count];
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

        //_hlSensorInfo = new HyperLink[_dtColumnsDetail.Rows.Count];
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
        //_meeDate = new AjaxControlToolkit.MaskedEditExtender[_dtColumnsDetail.Rows.Count];
        //_mevDate = new AjaxControlToolkit.MaskedEditValidator[_dtColumnsDetail.Rows.Count];
        _twmValue = new AjaxControlToolkit.TextBoxWatermarkExtender[_dtColumnsDetail.Rows.Count];

        _rvDate = new RangeValidator[_dtColumnsDetail.Rows.Count];
        _meeTime = new AjaxControlToolkit.MaskedEditExtender[_dtColumnsDetail.Rows.Count];
        _cvTime = new CustomValidator[_dtColumnsDetail.Rows.Count];
        _lblTime = new Label[_dtColumnsDetail.Rows.Count];
        _txtTime = new TextBox[_dtColumnsDetail.Rows.Count];

        HtmlTableRow[] trX = new HtmlTableRow[_dtColumnsDetail.Rows.Count + 4];
        HtmlTableCell[] cell = new HtmlTableCell[(_dtColumnsDetail.Rows.Count + 4) * 2];
        ////HtmlTableCell[] cellB = new HtmlTableCell[(_dtColumnsDetail.Rows.Count + 4) * 2];


        //HtmlTableRow[] trXB = new HtmlTableRow[_dtColumnsDetail.Rows.Count + 4];

        lblFristTabTableName.Text = _theTable.TableName;

        if (Request.QueryString["RecordID"] != null || _bCopyRecord == true)
        {
            if (Request.QueryString["RecordID"] != null)
            {
                _qsRecordID = Cryptography.Decrypt(Request.QueryString["RecordID"]);
            }
            if (Request.QueryString["CopyRecordID"] != null)
            {
                _qsRecordID = Cryptography.Decrypt(Request.QueryString["CopyRecordID"]);
            }
            if (int.TryParse(_qsRecordID, out _iRecordID) == false)
            {
                //not a Record ID, what to do
            }
            hfRecordID.Value = _qsRecordID;
            _dtRecordedetail = RecordManager.ets_Record_Details(_iRecordID).Tables[1];
            _theRecord = RecordManager.ets_Record_Detail_Full(_iRecordID);

        }



        _dtDBTableTab = Common.DataTableFromText("SELECT * FROM TableTab WHERE TableID=" +
            _theTable.TableID.ToString() + " ORDER BY DisplayOrder");

        if (_dtDBTableTab != null)
        {
            if (_dtDBTableTab.Rows.Count > 1)
            {

                _bTableTabYes = true;

               
                //Tab Show When
                if (_theRecord!=null)
                {
                    try
                    {
                        string strHaveShowWhen = Common.GetValueFromSQL(@"SELECT TOP 1 SW.TableTabID FROM [ShowWhen] SW JOIN [TableTab] TT
                                             ON SW.TableTabID=TT.TableTabID
	                                            WHERE TT.TableID=" + _theRecord.TableID.ToString());
                        if(strHaveShowWhen!="")
                        {
                            string strHiddenTableTabID = "-1";
                             for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                             {
                                 DataTable dtTabShowWhen = RecordManager.dbg_ShowWhen_ForGrid(null, null, int.Parse(_dtDBTableTab.Rows[t]["TableTabID"].ToString()));

                                 if(dtTabShowWhen.Rows.Count>0)
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


                            if(strHiddenTableTabID!="-1")
                            {
                                _dtDBTableTab = Common.DataTableFromText("SELECT * FROM TableTab WHERE TableID=" +
                                    _theTable.TableID.ToString() + " AND TableTabID NOT IN (" + strHiddenTableTabID + ")  ORDER BY DisplayOrder");
                            }


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



                for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                {

                    if (t == 0)
                    {
                        LinkButton lnkDetialTab = new LinkButton();
                        lnkDetialTab.ID = "lnkDetialTab";
                        lnkDetialTab.Text = _dtDBTableTab.Rows[t]["TabName"].ToString(); //"Detail";
                        lnkDetialTab.Font.Bold = true;
                        lnkDetialTab.Attributes.Add("onclick", "ShowHideMainDivs(" + pnlDetailTab.ClientID + ",this); return false");
                        lnkDetialTab.CssClass = "TablLinkClass";
                        pnlTabHeading.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp"));
                        pnlTabHeading.Controls.Add(lnkDetialTab);
                        pnlTabHeading.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp"));
                    }
                    else
                    {

                        _pnlDetailTabD[t] = new Panel();
                        _pnlDetailTabD[t].ID = "pnlDetailTabD" + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        _pnlDetailTabD[t].CssClass = "showhidedivs";


                        LinkButton lnkDetialTabD = new LinkButton();
                        lnkDetialTabD.ID = "lnkDetialTabD" + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        lnkDetialTabD.Text = _dtDBTableTab.Rows[t]["TabName"].ToString();
                        lnkDetialTabD.OnClientClick = "ShowHideMainDivs(" + "ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _pnlDetailTabD[t].ClientID + ",this); return false";
                        lnkDetialTabD.CssClass = "TablLinkClass";

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
                        _tblLeftD[t].ID = "tblLeftD" + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        cellLeft.Controls.Add(_tblLeftD[t]);

                        _tblRightD[t] = new HtmlTable();
                        _tblRightD[t].ID = "tblRightD" + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        cellRight.Controls.Add(_tblRightD[t]);



                        _pnlDetailTabD[t].Controls.Add(_tblMainD[t]);

                        pnlMain.Controls.Add(_pnlDetailTabD[t]);
                        //HtmlTableCell cellTemp = new HtmlTableCell();
                        //cellTemp.Controls.Add(_pnlDetailTabD[t]);
                        //trMain.Cells.Add(cellTemp);

                    }


                }

                if (!IsPostBack)
                {
                    if (Request.QueryString["TableTabID"] != null)
                    {
                        for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                        {

                            string strPanelID = "";
                            string strLnk = "";
                            if (_dtDBTableTab.Rows[t]["TableTabID"].ToString() == Request.QueryString["TableTabID"].ToString())
                            {
                                if (t == 0)
                                {
                                    strPanelID = pnlDetailTab.ClientID;
                                    strLnk = "ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_lnkDetialTab";
                                }
                                else
                                {
                                    strPanelID = _pnlDetailTabD[t].ClientID;
                                    strLnk = "ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_lnkDetialTabD" + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                                }
                            }

                            if (strPanelID != "" && strLnk != "")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideMainDivsTableTab", "ShowHideMainDivs(" + strPanelID + "," + strLnk + ");", true);

                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideMainDivsTableTab", "alert('hhh');", true);
                            }

                        }

                    }

                }

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
            if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
            {
                _lbl[i].Text = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "*";
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
                    //_lbl[i].Text = "";
                }
            }
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

                    _rvDate[i] = new RangeValidator();
                    _rvDate[i].ID = "rvDate" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _rvDate[i].Display = ValidatorDisplay.None;
                    _rvDate[i].ControlToValidate = _txtValue[i].ID;
                    _rvDate[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid.";
                    _rvDate[i].Type = ValidationDataType.Date;
                    _rvDate[i].Font.Bold = true;
                    _rvDate[i].MinimumValue = "1/1/1753";
                    _rvDate[i].MaximumValue = "1/1/3000";


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
                        //_txtTime[i].Text = "00:00";


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
                        //_txtTime[i].Text = "00:00";

                        _txtTime[i].Text = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HH:m");
                    }


                    _iDateTimeRecorded = i;

                    _txtValue[i].Text = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();







                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();

                    _txtValue[i].Enabled = false;
                    //_ceDateTimeRecorded[i].Enabled = false;
                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                    cell[(i * 2) + 1].Controls.Add(_ibValue[i]);
                    cell[(i * 2) + 1].Controls.Add(_ceDateTimeRecorded[i]);

                    //cell[(i * 2) + 1].Controls.Add(_meeDate[i]);
                    //cell[(i * 2) + 1].Controls.Add(_mevDate[i]);
                    cell[(i * 2) + 1].Controls.Add(_twmValue[i]);
                    cell[(i * 2) + 1].Controls.Add(_rvDate[i]);

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                    {
                        _txtTime[i].Enabled = false;
                        cell[(i * 2) + 1].Controls.Add(_lblTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_txtTime[i]);
                    }



                    if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                    {

                        _rfvValue[i] = new RequiredFieldValidator();
                        _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _rfvValue[i].Display = ValidatorDisplay.None;//
                        _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                        _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


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

                    if (Request.QueryString["mode"] != null)
                    {

                        if (_qsMode.ToLower() == "add")
                        {
                            if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                            {
                                _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                            }

                            if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                && _iParentRecordID != null)
                            {

                                Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                if (theDefaultColumn != null && theParentRecord != null)
                                {
                                    //_txtValue[i].Text=RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                    _txtValue[i].Text = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                }

                            }
                        }
                    }

                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();
                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);


                    if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                    {

                        _rfvValue[i] = new RequiredFieldValidator();
                        _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _rfvValue[i].Display = ValidatorDisplay.None;
                        _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                        _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                        cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                    }


                    break;

                case "tableid":

                    _txtValue[i] = new TextBox();
                    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    _txtValue[i].Width = 198;
                    _txtValue[i].Enabled = false;
                    _txtValue[i].CssClass = "NormalTextBox";

                    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                    {
                        _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                    }

                    _iTableIndex = i;

                    //cell[(i * 2) + 1] = new HtmlTableCell();
                    //cellB[(i * 2) + 1] = new HtmlTableCell();

                    cell[(i * 2) + 1].Controls.Add(_txtValue[i]);

                    break;

                //case "locationid":

                //    _ddlLocation = new DropDownList();
                //    _ddlLocation.ID = "ddlLocation";
                //    _ddlLocation.DataValueField = "LocationID";
                //    _ddlLocation.DataTextField = "LocationName";
                //    _ddlLocation.CssClass = "NormalTextBox";
                //    _ddlLocation.DataSource = SiteManager.ets_Location_Select(null, int.Parse(_qsTableID), null, null, "",
                //         true, null, null, null, null,
                //        theMenu.AccountID,
                //        "LocationName", "ASC", null, null, ref iTN, "");
                //    _ddlLocation.DataBind();

                //    System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("-None-", "");
                //    _ddlLocation.Items.Insert(0, liNone);


                //    if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                //    {
                //        _ddlLocation.ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                //    }
                //    //_ddlLocation.Text=
                //    _iLocationIndex = i;

                //    cell[(i * 2) + 1] = new HtmlTableCell();
                //    ////cellB[(i * 2) + 1] = new HtmlTableCell();

                //    cell[(i * 2) + 1].Controls.Add(_ddlLocation);
                //    cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                //    //_objUser = (User)Session["User"];
                //    _hlSSAdd = new HyperLink();

                //    if (Common.HaveAccess(_strRecordRightID, "1,2,3"))
                //    {

                //        _hlSSAdd.Text = "Add";
                //        _hlSSAdd.CssClass = "NormalTextBox";
                //        _hlSSAdd.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Site/LocationDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&MenuID=" + Cryptography.Encrypt("-1") + "&GoBack=yes";
                //        cell[(i * 2) + 1].Controls.Add(_hlSSAdd);
                //    }

                //    if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                //    {
                //        try
                //        {
                //            _ddlLocation.Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                //        }
                //        catch
                //        {

                //        }

                //    }

                //    if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                //    {

                //        _rfvValue[i] = new RequiredFieldValidator();
                //        _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                //        _rfvValue[i].Display = ValidatorDisplay.None;
                //        _rfvValue[i].ControlToValidate = _ddlLocation.ClientID;
                //        _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                //        cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                //    }



                //    break;

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

                    try
                    {
                        _ddlEnteredBy.Text = _objUser.UserID.ToString();
                    }
                    catch
                    {
                        //
                    }

                    _ddlEnteredBy.Enabled = false;
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
                        //_meeDate[i].ID = "meeDate" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
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
                        //_mevDate[i].ID = "mevDate" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_mevDate[i].ControlExtender = _meeDate[i].ID;
                        //_mevDate[i].ControlToValidate = _txtValue[i].ID;
                        //_mevDate[i].InvalidValueMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid";
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



                        // cell[(i * 2) + 1] = new HtmlTableCell();
                        //cellB[(i * 2) + 1] = new HtmlTableCell();

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ibValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ceDateTimeRecorded[i]);

                        //cell[(i * 2) + 1].Controls.Add(_meeDate[i]);
                        //cell[(i * 2) + 1].Controls.Add(_mevDate[i]);
                        cell[(i * 2) + 1].Controls.Add(_twmValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_rvDate[i]);

                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        {
                            //_txtValue[i].Text = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();
                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }

                        if (Request.QueryString["mode"] != null)
                        {
                            //if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            //{

                            //    if (_qsMode.ToLower() == "add")
                            //    {

                            //        _txtValue[i].Text = DateTime.Today.Date.ToShortDateString();
                            //    }

                            //}

                            if (_qsMode.ToLower() == "add")
                            {
                                if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                {
                                    _txtValue[i].Text = DateTime.Today.Date.ToShortDateString();
                                }

                                if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                    && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                    && _iParentRecordID != null)
                                {

                                    Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                    Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                    if (theDefaultColumn != null && theParentRecord != null)
                                    {
                                        //_txtValue[i].Text = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                        _txtValue[i].Text = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                    }

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

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }


                        _meeTime[i] = new AjaxControlToolkit.MaskedEditExtender();
                        _meeTime[i].ID = "meeTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _meeTime[i].TargetControlID = _txtValue[i].ClientID; //"ctl00_HomeContentPlaceHolder_txtTime";
                        _meeTime[i].AutoCompleteValue = "00:00"; //"00:00:00"
                        _meeTime[i].Mask = "99:99"; //99:99:99
                        _meeTime[i].MaskType = MaskedEditType.Time;

                        _cvTime[i] = new CustomValidator();
                        _cvTime[i].ID = "cvTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _cvTime[i].ControlToValidate = _txtValue[i].ClientID;  //"ctl00_HomeContentPlaceHolder_txtTime";
                        _cvTime[i].ClientValidationFunction = "CheckMyText";
                        _cvTime[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- hh:mm format (24 hrs) please!";//hh:mm:ss
                        _cvTime[i].Display = ValidatorDisplay.None;


                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        //cellB[(i * 2) + 1] = new HtmlTableCell();

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);



                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        {
                            //_txtValue[i].Text = "00:00:00";
                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }

                        cell[(i * 2) + 1].Controls.Add(_meeTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_cvTime[i]);

                        if (Request.QueryString["mode"] != null)
                        {
                            //if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            //{

                            //    if (_qsMode == "add")
                            //    {
                            //        _txtValue[i].Text = DateTime.Now.ToString("HH:mm:ss");
                            //    }

                            //}

                            if (_qsMode == "add")
                            {
                                if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                {
                                    _txtValue[i].Text = DateTime.Now.ToString("HH:m");//HH:mm:ss
                                }

                                if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                    && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                    && _iParentRecordID != null)
                                {

                                    Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                    Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                    if (theDefaultColumn != null && theParentRecord != null)
                                    {
                                        //_txtValue[i].Text = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                        _txtValue[i].Text = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                    }

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
                        //_meeDate[i].ID = "meeDate" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
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
                        //_mevDate[i].ID = "mevDate" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_mevDate[i].Display = ValidatorDisplay.None;
                        //_mevDate[i].ControlExtender = _meeDate[i].ID;
                        //_mevDate[i].ControlToValidate = _txtValue[i].ID;
                        //_mevDate[i].InvalidValueMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Date is invalid";

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


                        _lblTime[i] = new Label();
                        _lblTime[i].ID = "lblTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lblTime[i].Text = "  Time ";
                        _lblTime[i].Font.Bold = true;

                        _txtTime[i] = new TextBox();
                        _txtTime[i].ID = "txtTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _txtTime[i].Width = 80;
                        _txtTime[i].AutoCompleteType = AutoCompleteType.Disabled;
                        _txtTime[i].CssClass = "NormalTextBox";

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






                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        //cellB[(i * 2) + 1] = new HtmlTableCell();

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ibValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ceDateTimeRecorded[i]);

                        //cell[(i * 2) + 1].Controls.Add(_meeDate[i]);
                        //cell[(i * 2) + 1].Controls.Add(_mevDate[i]);
                        cell[(i * 2) + 1].Controls.Add(_twmValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_rvDate[i]);


                        cell[(i * 2) + 1].Controls.Add(_lblTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_txtTime[i]);

                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        {
                            //_txtValue[i].Text = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();
                            //_txtTime[i].Text = "00:00";
                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }


                        cell[(i * 2) + 1].Controls.Add(_meeTime[i]);
                        cell[(i * 2) + 1].Controls.Add(_cvTime[i]);

                        if (Request.QueryString["mode"] != null)
                        {
                            if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            {

                                //if (_qsMode == "add")
                                //{
                                //    try
                                //    {

                                //        DateTime dtTempDateTime = DateTime.Now;  // DateTime.Parse(_dtColumnsDetail.Rows[i]["DefaultValue"].ToString());

                                //        _txtValue[i].Text = dtTempDateTime.Day.ToString("00") + "/" + dtTempDateTime.Month.ToString("00") + "/" + dtTempDateTime.Year.ToString();


                                //        _txtTime[i].Text = Convert.ToDateTime(dtTempDateTime.ToString()).ToString("HH:m");
                                //    }
                                //    catch
                                //    {
                                //        //
                                //    }

                                //}


                                try
                                {
                                    if (_qsMode == "add")
                                    {
                                        if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                        {
                                            DateTime dtTempDateTime = DateTime.Now;  // DateTime.Parse(_dtColumnsDetail.Rows[i]["DefaultValue"].ToString());

                                            _txtValue[i].Text = dtTempDateTime.Day.ToString() + "/" + dtTempDateTime.Month.ToString("00") + "/" + dtTempDateTime.Year.ToString();


                                            _txtTime[i].Text = Convert.ToDateTime(dtTempDateTime.ToString()).ToString("HH:m");
                                        }

                                        if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                            && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                            && _iParentRecordID != null)
                                        {

                                            Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                            Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                            if (theDefaultColumn != null && theParentRecord != null)
                                            {

                                                string strDefaultValue = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                                //DateTime dtTempDateTime =Convert.ToDateTime( RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName));

                                                if (strDefaultValue != "")
                                                {
                                                    DateTime dtTempDateTime = Convert.ToDateTime(strDefaultValue);
                                                    _txtValue[i].Text = dtTempDateTime.Day.ToString() + "/" + dtTempDateTime.Month.ToString("00") + "/" + dtTempDateTime.Year.ToString();


                                                    _txtTime[i].Text = Convert.ToDateTime(dtTempDateTime.ToString()).ToString("HH:m");
                                                }
                                            }

                                        }
                                    }

                                }
                                catch
                                {

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
                        _lnkValue[i].CausesValidation = true;
                        bool bVisible = false;
                        if (_dtColumnsDetail.Rows[i]["ButtonInfo"] != DBNull.Value && _dtColumnsDetail.Rows[i]["ButtonInfo"].ToString() != "")
                        {
                            ColumnButtonInfo theButtonInfo = JSONField.GetTypedObject<ColumnButtonInfo>(_dtColumnsDetail.Rows[i]["ButtonInfo"].ToString());
                            if (theButtonInfo != null)
                            {
                                if (!string.IsNullOrEmpty(theButtonInfo.SPToRun))
                                {
                                    bVisible = true;
                                    _lnkValue[i].CommandArgument = _dtColumnsDetail.Rows[i]["ColumnID"].ToString(); //theButtonInfo.SPToRun;
                                }

                                if (!string.IsNullOrEmpty(theButtonInfo.ImageFullPath))
                                {
                                    if (theButtonInfo.ImageFullPath.IndexOf("http")>-1)
                                    {

                                    }
                                    else
                                    {
                                        theButtonInfo.ImageFullPath =_strFilesLocation + "/UserFiles/AppFiles/" + theButtonInfo.ImageFullPath;
                                    }
                                    _lnkValue[i].Text = "<img src='" + theButtonInfo.ImageFullPath + "' />";
                                }
                                else
                                {
                                    _lnkValue[i].CssClass = "btn";
                                    if (_dtColumnsDetail.Rows[i]["DisplayTextDetail"] != DBNull.Value && _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() != "")
                                    {
                                        _lnkValue[i].Text = "<strong>" + _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "</strong>";
                                    }
                                    else
                                    {
                                        //
                                        _lnkValue[i].Text = "<strong>" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + "</strong>";
                                    }
                                    //if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                                    //{
                                    //    _lnkValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                                    //}
                                    //if (_dtColumnsDetail.Rows[i]["TextHeight"] != null && _dtColumnsDetail.Rows[i]["TextHeight"].ToString() != "")
                                    //{
                                    //    _lnkValue[i].Height = int.Parse(_dtColumnsDetail.Rows[i]["TextHeight"].ToString()) * 18;
                                    //}
                                }

                                if (!string.IsNullOrEmpty(theButtonInfo.WarningMessage))
                                {
                                    _lnkValue[i].OnClientClick = "javascript:return confirm('" + HttpUtility.JavaScriptStringEncode(theButtonInfo.WarningMessage) + "')";
                                }
                            }

                        }
                        _lnkValue[i].Visible = bVisible;

                        //_lnkValue[i].Text = "Use basic version";
                        _lnkValue[i].Click += new EventHandler(LB_SPRun_Click);

                        cell[(i * 2) + 1].Controls.Add(_lnkValue[i]);
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                    {


                        //ScriptManager theScriptManager1= (ScriptManager) this.Page.Master.FindControl("ScriptManager1");

                        //if (theScriptManager1 != null)
                        //    theScriptManager1.EnablePartialRendering = false;

                        _fuValue[i] = new FileUpload();
                        _fuValue[i].ID = "fu" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _fuValue[i].ClientIDMode = ClientIDMode.Static;
                        _hfValue[i] = new HiddenField();
                        _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _hfValue[i].ClientIDMode = ClientIDMode.Static;

                        _hfValue2[i] = new HiddenField();
                        _hfValue2[i].ID = "hf2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _hfValue2[i].ClientIDMode = ClientIDMode.Static;


                        _lblValue[i] = new Label();
                        _lblValue[i].ID = "lblV" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lblValue[i].ClientIDMode = ClientIDMode.Static;

                        _pnlDIV[i] = new Panel();
                        _pnlDIV[i].ID = "pnl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _pnlDIV[i].ClientIDMode = ClientIDMode.Static;

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
                        _fuValue2[i].ClientIDMode = ClientIDMode.Static;
                        _bNeedFullPostback = true;



                        cell[(i * 2) + 1].Controls.Add(_fuValue2[i]);

                        _lnkValue[i] = new LinkButton();
                        _lnkValue[i].ID = "lnk" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _lnkValue[i].Text = "Use basic version";
                        _lnkValue[i].CausesValidation = false;
                        _lnkValue[i].OnClientClick = "UserBasic" + i.ToString() + "(); return false";
                        _lnkValue[i].ClientIDMode = ClientIDMode.Static;

                        //if (_qsMode == "edit")
                        //{
                        cell[(i * 2) + 1].Controls.Add(new LiteralControl("</br>"));
                        //}
                        cell[(i * 2) + 1].Controls.Add(_lnkValue[i]);

                        string strUserBasicMandatory = "";

                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;//
                            _rfvValue[i].ControlToValidate = _fuValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                            strUserBasicMandatory = @"ValidatorEnable(document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _rfvValue[i].ID.ToString() + "'), false);";

                        }


                        //                        string strJSBasicUPload = @" 
                        //                                                     var y = document.getElementById('"+_fuValue2[i].ID+@"');
                        //                                                    y.style.display = 'none';
                        //                                                    function UserBasic" + i.ToString()+@"() {
                        //                                                    var y = document.getElementById('" + _fuValue2[i].ID + @"');
                        //                                                    y.style.display = 'block'; 
                        //                                                    document.getElementById('" + _hfValue2[i].ID + @"').value='yes';" + strUserBasicMandatory + @"
                        //
                        //                                                    y = document.getElementById('" + _pnlDIV[i].ID + @"');
                        //                                                    y.style.display = 'none';
                        //                                                    y = document.getElementById('" + _lblValue[i].ID + @"');
                        //                                                    y.style.display = 'none';
                        //
                        //                                                    y = document.getElementById('" + _lnkValue[i].ID + @"');
                        //                                                    y.style.display = 'none';
                        //
                        //                                                }";



                        string strJSBasicUPload = @" 
                                var y = document.getElementById('" + _fuValue2[i].ID + @"');
                                y.style.display = 'none';
                                function UserBasic" + i.ToString() + @"() {
                                var y = document.getElementById('" + _fuValue2[i].ID + @"');
                                y.style.display = 'block'; 
                                document.getElementById('" + _hfValue2[i].ID + @"').value='yes';" + strUserBasicMandatory + @"

                                y = document.getElementById('" + _pnlDIV[i].ID + @"');
                                y.style.display = 'none';
                                y = document.getElementById('" + _lblValue[i].ID + @"');
                                y.style.display = 'none';

                                y = document.getElementById('" + _lnkValue[i].ID + @"');
                                y.style.display = 'none';

                            }

                        if (swfobject.hasFlashPlayerVersion('1')) {
                                    //what to do
                                    y = document.getElementById('" + _lnkValue[i].ID + @"');
                                    y.style.display = 'none';        
                                }
                                else {
                                  UserBasic" + i.ToString() + @"();
                                   y = document.getElementById('" + _lblValue[i].ID + @"');
                                    y.style.display = 'block';
                                }



";



                        ScriptManager.RegisterStartupScript(this, this.GetType(), "strJSBasicUPload" + i.ToString(), strJSBasicUPload, true);




                        string strValidatorT = "";
                        string strValidatorF = "";
                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString())
                && _rfvValue[i] != null)
                        {
                            strValidatorT = "ValidatorEnable(document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _rfvValue[i].ID.ToString() + "'), true);";
                            strValidatorF = "ValidatorEnable(document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _rfvValue[i].ID.ToString() + "'), false);";

                            if (Request.QueryString["RecordID"] != null)
                            {
                                _strJS = _strJS + strValidatorF;
                            }
                        }

                        string strScriptPath = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Handler.ashx";

                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                        {
                            string strMaxHeight = "50";
                            if (_dtColumnsDetail.Rows[i]["TextHeight"] != DBNull.Value)
                            {
                                strMaxHeight = _dtColumnsDetail.Rows[i]["TextHeight"].ToString();
                            }
                            string strFilePath = _strFilesLocation + "/UserFiles/AppFiles/";

                            string strInnerHTML = "<img  title=\"Remove this image\" style=\"cursor:pointer;\"  id=\"dimg" + _hfValue[i].ID + "\" src=\"" + "http://" + Request.Url.Authority + Request.ApplicationPath
                                + "/App_Themes/Default/Images/icon_delete.gif\" />" +
                                "<a id=\"a" + _hfValue[i].ID + "\" target=\"_blank\" >"
                            + " <img style=\"padding-bottom:7px; max-height:"
                            + strMaxHeight + "px;\" id=\"img" + _hfValue[i].ID + "\"  />" + "</a><br/>";

                            _strJSPostBack = _strJSPostBack + @" $(document).ready(function () {
                                        $('#" + _fuValue[i].ID + @"').uploadify({
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
                                               document.getElementById('" + _hfValue[i].ID + @"').value=jo.filename;

                                                $('#" + _lblValue[i].ID + @"').html('" + strInnerHTML + @"'); "
                                                + strValidatorF + @"
                                                
                                                document.getElementById('img" + _hfValue[i].ID + @"').src=jo.fullpath;
                                                document.getElementById('img" + _hfValue[i].ID + @"').alt=fileObj.name;
                                                document.getElementById('img" + _hfValue[i].ID + @"').title=fileObj.name;
                                                document.getElementById('a" + _hfValue[i].ID + @"').href=jo.fullpath;

                                               

                                                  document.getElementById('dimg" + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _hfValue[i].ID + @"').value='';
                                                      $('#" + _lblValue[i].ID + @"').html(''); 
                                            });
                                               // alert(response);jo.fullpath
                   
                                            },
                                            'onSelect': function (event, ID, fileObj) {
                                                $('#" + _lblValue[i].ID + @"').html('');
                                                document.getElementById('" + _hfValue[i].ID + @"').value='';
                                                $('#" + _fuValue[i].ID + @"').uploadifySettings(
                                                'scriptData', { 'foo': 'UserFiles/AppFiles' }
                                                    );


                                                $('#" + _fuValue[i].ID + @"').uploadifyUpload();



                                            },
                                            'onCancel': function (event, ID, fileObj, data) {
                                               document.getElementById('" + _hfValue[i].ID + @"').value=''; " + strValidatorT + @"
                                               $('#" + _lblValue[i].ID + @"').html('');
                                            }

                                        });

                                          
                                    });";
                        }
                        else
                        {
                            string strInnerHTML = "<img  title=\"Remove this file\" style=\"cursor:pointer;\"  id=\"dimg" + _hfValue[i].ID + "\" src=\"" + "http://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />";

                            _strJSPostBack = _strJSPostBack + @" $(document).ready(function () {
                                        $('#" + _fuValue[i].ID + @"').uploadify({
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
                                                document.getElementById('" + _hfValue[i].ID + @"').value=jo.filename;
                                                $('#" + _lblValue[i].ID + @"').html('" + strInnerHTML + @"' + fileObj.name ); " + strValidatorF + @"                                                 
                                               
                                                     document.getElementById('dimg" + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                         document.getElementById('" + _hfValue[i].ID + @"').value='';
                                                            $('#" + _lblValue[i].ID + @"').html(''); 
                                                    });
                                                 alert(fileObj.name + ' File has been uploaded.');
                   
                                            },
                                            'onSelect': function (event, ID, fileObj) {
                                                $('#" + _lblValue[i].ID + @"').html('');
                                                document.getElementById('" + _hfValue[i].ID + @"').value='';
                                                $('#" + _fuValue[i].ID + @"').uploadifySettings(
                                                'scriptData', { 'foo': 'UserFiles/AppFiles' }
                                                    );


                                                $('#" + _fuValue[i].ID + @"').uploadifyUpload();



                                            },
                                            'onCancel': function (event, ID, fileObj, data) {
                                               document.getElementById('" + _hfValue[i].ID + @"').value=''; " + strValidatorT + @"
                                               $('#" + _lblValue[i].ID + @"').html('');
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
                                _txtValue[i].ClientIDMode = ClientIDMode.Static;
                                _txtValue[i].ToolTip = "Start typing and matching values will be shown";

                                if (_dtColumnsDetail.Rows[i]["TextWidth"] != null && _dtColumnsDetail.Rows[i]["TextWidth"].ToString() != "")
                                {
                                    _txtValue[i].Width = int.Parse(_dtColumnsDetail.Rows[i]["TextWidth"].ToString()) * 9;
                                }

                                _imgValues[i] = new Image();
                                _imgValues[i].ImageUrl = "~/App_Themes/Default/Images/dropdown.png";
                                _imgValues[i].ToolTip = "Start typing and matching values will be shown";

                                _hfValue[i] = new HiddenField();
                                _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _hfValue[i].ClientIDMode = ClientIDMode.Static;


                                // cell[(i * 2) + 1] = new HtmlTableCell();
                                //cellB[(i * 2) + 1] = new HtmlTableCell();
                                //cell[(i * 2) + 1].VAlign = "middle";

                                HtmlTable tblQuickLink = new HtmlTable();
                                HtmlTableCell cellQL1 = new HtmlTableCell();
                                HtmlTableCell cellQL2 = new HtmlTableCell();
                                HtmlTableCell cellQL3 = new HtmlTableCell();
                                HtmlTableRow hrQL = new HtmlTableRow();

                                tblQuickLink.CellPadding = 0;
                                tblQuickLink.CellSpacing = 0;

                                hrQL.Cells.Add(cellQL1);
                                hrQL.Cells.Add(cellQL2);
                                hrQL.Cells.Add(cellQL3);

                                tblQuickLink.Rows.Add(hrQL);

                                cellQL1.Controls.Add(_txtValue[i]);
                                cellQL1.Controls.Add(_imgValues[i]);

                                //cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                                //cell[(i * 2) + 1].Controls.Add(_imgValues[i]);

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
                                               " <mode>" + HttpUtility.HtmlEncode(Request.QueryString["mode"].ToString()) + "</mode>" +
                                               " <TableID>" + HttpUtility.HtmlEncode(Request.QueryString["TableID"].ToString()) + "</TableID>" +
                                               " <SearchCriteriaID>" + HttpUtility.HtmlEncode(Request.QueryString["SearchCriteriaID"].ToString()) + "</SearchCriteriaID>" +
                                               " <control>" + HttpUtility.HtmlEncode(_txtValue[i].ID.ToString()) + "</control>" +
                                                " <TableTableID>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()) + "</TableTableID>" +
                                                 " <DisplayColumn>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["DisplayColumn"].ToString()) + "</DisplayColumn>" +
                                                  " <LinkedParentColumnID>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()) + "</LinkedParentColumnID>" +
                                                  " <DropDownType>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["DropDownType"].ToString()) + "</DropDownType>" +
                                                  " <_hfValue>" + HttpUtility.HtmlEncode(_hfValue[i].ID.ToString()) + "</_hfValue>" +
                                                  " <RecordID>" + HttpUtility.HtmlEncode(Request.QueryString["RecordID"] == null ? "-1" : Request.QueryString["RecordID"].ToString()) + "</RecordID>" +
                                              "</root>";
                                        //
                                        SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
                                        int iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);

                                        _hlValue[i].NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" +
                                            Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(_dtColumnsDetail.Rows[i]["TableTableID"].ToString())
                                            + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&quickadd=" + Cryptography.Encrypt(iSearchCriteriaID.ToString());

                                        //cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                                        //cell[(i * 2) + 1].Controls.Add(_hlValue[i]);

                                        cellQL2.Controls.Add(new LiteralControl("&nbsp;"));
                                        cellQL2.Controls.Add(_hlValue[i]);
                                    }
                                }

                                if (_dtColumnsDetail.Rows[i]["ShowViewLink"] != DBNull.Value)
                                {
                                    if (_dtColumnsDetail.Rows[i]["ShowViewLink"].ToString().ToLower() == "detail"
                                        || _dtColumnsDetail.Rows[i]["ShowViewLink"].ToString().ToLower() == "both")
                                    {
                                        _hlValue2[i] = new HyperLink();
                                        _hlValue2[i].ID = "hl2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                        _hlValue2[i].Text = "View";

                                        _hlValue2[i].NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" +
                                            Cryptography.Encrypt(_qsMode.ToString()) + "&TableID=" + Cryptography.Encrypt(_dtColumnsDetail.Rows[i]["TableTableID"].ToString())
                                            + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");

                                        if (_qsMode != "" && _qsMode.ToLower() == "add")
                                        {
                                            _hlValue2[i].Visible = false;
                                        }

                                        cellQL3.Controls.Add(new LiteralControl("&nbsp;"));
                                        cellQL3.Controls.Add(_hlValue2[i]);

                                    }
                                }



                                if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                                {

                                    _rfvValue[i] = new RequiredFieldValidator();
                                    _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    _rfvValue[i].Display = ValidatorDisplay.None;
                                    _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                                    _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                                    cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                                }

                                string strAutoDDJS = @" $(function () {
                                $(""#" + _txtValue[i].ID.ToString() + @""").autocomplete({
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
                                                        document.getElementById('" + _hfValue[i].ID.ToString() + @"').value = '';
                                                    }
                                                    else {
                                                        document.getElementById('" + _hfValue[i].ID.ToString() + @"').value = ui.item.id;
                                                    }
                                                }
                                            });
                                        });

                                ";

                                if (_qsMode != "view")
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AutoCompleteJS" + i.ToString(), strAutoDDJS, true);
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
                                //_ddlValue[i].ClientIDMode = ClientIDMode.Static;

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
                                HtmlTableCell cellQL3 = new HtmlTableCell();
                                HtmlTableRow hrQL = new HtmlTableRow();

                                tblQuickLink.CellPadding = 0;
                                tblQuickLink.CellSpacing = 0;

                                hrQL.Cells.Add(cellQL1);
                                hrQL.Cells.Add(cellQL2);
                                hrQL.Cells.Add(cellQL3);

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
                                                " <mode>" + HttpUtility.HtmlEncode(Request.QueryString["mode"].ToString()) + "</mode>" +
                                                " <TableID>" + HttpUtility.HtmlEncode(Request.QueryString["TableID"].ToString()) + "</TableID>" +
                                                " <SearchCriteriaID>" + HttpUtility.HtmlEncode(Request.QueryString["SearchCriteriaID"].ToString()) + "</SearchCriteriaID>" +
                                                " <control>" + HttpUtility.HtmlEncode(_ddlValue[i].ID.ToString()) + "</control>" +
                                                 " <TableTableID>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()) + "</TableTableID>" +
                                                 " <DisplayColumn>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["DisplayColumn"].ToString()) + "</DisplayColumn>" +
                                                 " <LinkedParentColumnID>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()) + "</LinkedParentColumnID>" +
                                                  " <DropDownType>" + HttpUtility.HtmlEncode(_dtColumnsDetail.Rows[i]["DropDownType"].ToString()) + "</DropDownType>" +
                                                  " <RecordID>" + HttpUtility.HtmlEncode(Request.QueryString["RecordID"] == null ? "-1" : Request.QueryString["RecordID"].ToString()) + "</RecordID>" +
                                               "</root>";

                                        SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
                                        int iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);

                                        _hlValue[i].NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" +
                                            Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(_dtColumnsDetail.Rows[i]["TableTableID"].ToString())
                                            + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&quickadd=" + Cryptography.Encrypt(iSearchCriteriaID.ToString());

                                        //cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                                        //cell[(i * 2) + 1].Controls.Add(_hlValue[i]);
                                        cellQL2.Controls.Add(new LiteralControl("&nbsp;"));
                                        cellQL2.Controls.Add(_hlValue[i]);
                                    }
                                }

                                if (_dtColumnsDetail.Rows[i]["ShowViewLink"] != DBNull.Value)
                                {
                                    if (_dtColumnsDetail.Rows[i]["ShowViewLink"].ToString().ToLower() == "detail"
                                        || _dtColumnsDetail.Rows[i]["ShowViewLink"].ToString().ToLower() == "both")
                                    {
                                        _hlValue2[i] = new HyperLink();
                                        _hlValue2[i].ID = "hl2" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                        _hlValue2[i].Text = "View";

                                        _hlValue2[i].NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" +
                                            Cryptography.Encrypt(_qsMode.ToString()) + "&TableID=" + Cryptography.Encrypt(_dtColumnsDetail.Rows[i]["TableTableID"].ToString())
                                            + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");

                                        if (_qsMode != "" && _qsMode.ToLower() == "add")
                                        {
                                            _hlValue2[i].Visible = false;
                                        }

                                        cellQL3.Controls.Add(new LiteralControl("&nbsp;"));
                                        cellQL3.Controls.Add(_hlValue2[i]);

                                    }
                                }

                                if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                                {

                                    _rfvValue[i] = new RequiredFieldValidator();
                                    _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    _rfvValue[i].Display = ValidatorDisplay.None;
                                    _rfvValue[i].ControlToValidate = _ddlValue[i].ClientID;
                                    _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                                    cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                                }
                            }

                            if (_dtColumnsDetail.Rows[i]["DefaultType"] != DBNull.Value
                               && _dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "login" && _qsMode == "add")
                            {
                                if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                                    && _txtValue[i] != null && _hfValue[i] != null)
                                {
                                    string strLoginValue = "";
                                    string strLoginText = "";

                                    SecurityManager.ProcessLoginUserDefault(_dtColumnsDetail.Rows[i]["TableTableID"].ToString(), _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString(),
                                    _dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString(), _objUser.UserID.ToString(), ref  strLoginValue, ref  strLoginText);

                                    _hfValue[i].Value = strLoginValue;
                                    _txtValue[i].Text = strLoginText;
                                }

                                if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd"
                                    && _ddlValue[i] != null)
                                {
                                    string strLoginValue = "";
                                    string strLoginText = "";

                                    SecurityManager.ProcessLoginUserDefault(_dtColumnsDetail.Rows[i]["TableTableID"].ToString(), "",
                                     _dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString(), _objUser.UserID.ToString(), ref  strLoginValue, ref  strLoginText);

                                    if (_ddlValue[i].Items.FindByValue(strLoginValue) != null)
                                        _ddlValue[i].SelectedValue = strLoginValue;

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
                            //_ddlValue2[i].ClientIDMode = ClientIDMode.Static;

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
                            //_ddlValue[i].ClientIDMode = ClientIDMode.Static;


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
                            //_ccddl[i].ClientIDMode = ClientIDMode.Static;
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

                            if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                            {

                                _rfvValue[i] = new RequiredFieldValidator();
                                _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _rfvValue[i].Display = ValidatorDisplay.None;
                                _rfvValue[i].ControlToValidate = _ddlValue[i].ClientID;
                                _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                                cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                            }
                        }
                    }



                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                    {
                        _radioList[i] = new RadioButtonList();
                        _radioList[i].RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                        _radioList[i].RepeatLayout = RepeatLayout.Flow;
                        //_radioList[i].CssClass = "radiolistcss"; //_dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _radioList[i].ID = "radio" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

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

                            //if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            //{
                            //    if (_qsMode == "add")
                            //    {
                            //        _radioList[i].SelectedValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                            //    }
                            //}

                            if (_qsMode == "add")
                            {
                                if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                {
                                    _radioList[i].SelectedValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                }

                                if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                    && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                    && _iParentRecordID != null)
                                {

                                    Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                    Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                    if (theDefaultColumn != null && theParentRecord != null)
                                    {
                                        //_radioList[i].SelectedValue = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                        _radioList[i].SelectedValue = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);

                                    }

                                }
                            }

                        }
                        else if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text")
                        {
                            Common.PutRadioListValue_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _radioList[i]);

                            //if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            //{
                            //    if (_qsMode == "add")
                            //    {                                   
                            //        string strDefaultValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                            //        if (strDefaultValue.IndexOf(",") > -1)
                            //        {
                            //            _radioList[i].SelectedValue = strDefaultValue.Substring(0, strDefaultValue.IndexOf(","));
                            //        }
                            //    }
                            //}

                            if (Request.QueryString["mode"] != null)
                            {

                                if (_qsMode == "add")
                                {
                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                    {
                                        string strDefaultValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                        if (strDefaultValue.IndexOf(",") > -1)
                                        {
                                            _radioList[i].SelectedValue = strDefaultValue.Substring(0, strDefaultValue.IndexOf(","));
                                        }
                                    }

                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                        && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                        && _iParentRecordID != null)
                                    {

                                        Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                        Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                        if (theDefaultColumn != null && theParentRecord != null)
                                        {
                                            //string strDefaultValue = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                            string strDefaultValue = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                            if (strDefaultValue.IndexOf(",") > -1)
                                            {
                                                _radioList[i].SelectedValue = strDefaultValue.Substring(0, strDefaultValue.IndexOf(","));
                                            }
                                        }

                                    }




                                }
                            }


                        }
                        else
                        {
                            Common.PutRadioListValue_Image(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _radioList[i], _strFilesLocation);

                            if (Request.QueryString["mode"] != null)
                            {

                                if (_qsMode == "add")
                                {
                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                    {
                                        string strDefaultValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                        if (strDefaultValue != "")
                                        {
                                            _radioList[i].SelectedValue = strDefaultValue;
                                        }
                                    }

                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                        && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                        && _iParentRecordID != null)
                                    {

                                        Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                        Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                        if (theDefaultColumn != null && theParentRecord != null)
                                        {
                                            //string strDefaultValue = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                            string strDefaultValue = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                            if (strDefaultValue != "")
                                            {
                                                _radioList[i].SelectedValue = strDefaultValue;
                                            }
                                        }

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



                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _radioList[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }


                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                    {


                        //cell[(i * 2) + 1] = new HtmlTableCell();

                        _hfValue[i] = new HiddenField();
                        _hfValue[i].ID = "hf" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _hfValue[i].ClientIDMode = ClientIDMode.Static;

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
                        _hfValue2[i].ClientIDMode = ClientIDMode.Static;

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
                        _hfValue3[i].ClientIDMode = ClientIDMode.Static;
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


                        if (bShowMap)
                            cell[(i * 2) + 1].Controls.Add(new LiteralControl("<div align='left' id='map" + i.ToString() + "' style='width: " + strMapWidth + "px; height: " + strMapHeight + "px; margin-top: 10px;'></div>"));

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
                                _txtValue2[i].ClientIDMode = ClientIDMode.Static;

                                cell[(i * 2) + 1].Controls.Add(_txtValue2[i]);

                                if (bShowMap)
                                {
                                    strSearchAddressJS = @"                                                   
                                                        function showAddress" + i.ToString() + @"() {                                                      

                                                        var address = document.getElementById('searchbox" + i.ToString() + @"').value;         
                                                        var geocoder = new google.maps.Geocoder();
                                                        geocoder.geocode({ 'address': address }, function (results, status) {
                                                            if (status == google.maps.GeocoderStatus.OK) {
                                                                results[0].geometry.location;
                                                                var b = new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng());
                                                                map" + i.ToString() + @".setCenter(b);

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
                                    _lnkValue[i].OnClientClick = "showAddress" + i.ToString() + "(); return false";
                                    _lnkValue[i].ClientIDMode = ClientIDMode.Static;

                                    cell[(i * 2) + 1].Controls.Add(_lnkValue[i]);

                                    strShowAddress = @"$(function () {
                                                $('#searchbox" + i.ToString() + @"').autocomplete({

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
                                                            map" + i.ToString() + @".fitBounds(bounds);
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
                                _txtValue[i].ClientIDMode = ClientIDMode.Static;

                                cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                                cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;&nbsp;Longitude:&nbsp;"));

                                _txtTime[i] = new TextBox();
                                _txtTime[i].ID = "txtLong" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                _txtTime[i].Width = 145;
                                _txtTime[i].CssClass = "NormalTextBox";
                                _txtTime[i].ClientIDMode = ClientIDMode.Static;

                                cell[(i * 2) + 1].Controls.Add(_txtTime[i]);

                                if (bShowMap)
                                {
                                    strShowLatLong = @"  var txtLatitude = document.getElementById('" + _txtValue[i].ClientID.ToString() + @"');
                                            txtLatitude.value = map" + i.ToString() + @".getCenter().lat();
                                            var txtLongitude = document.getElementById('" + _txtTime[i].ClientID.ToString() + @"');
                                            txtLongitude.value = map" + i.ToString() + @".getCenter().lng(); ";
                                }
                            }
                        }


                        if (bShowMap)
                        {

                            string strMapJS = @"//$(document).ready(function () {

                                         var mapOptions = {
                                                zoom:parseFloat(document.getElementById('" + _hfValue3[i].ClientID.ToString() + @"').value),
                                                mapTypeId: google.maps.MapTypeId.ROADMAP,scrollwheel: false,
                                                center: new google.maps.LatLng(document.getElementById('" + _hfValue[i].ClientID.ToString() + @"').value, document.getElementById('" + _hfValue2[i].ClientID.ToString() + @"').value)          
                                            };
                                     var map" + i.ToString() + @" = new google.maps.Map(document.getElementById('map" + i.ToString() + @"'), mapOptions);
                                     var geocoder = new google.maps.Geocoder();" + strShowAddress + @"

                                     google.maps.event.addListener(map" + i.ToString() + @", 'center_changed', function () {
                                            document.getElementById('" + _hfValue[i].ClientID.ToString() + @"').value = map" + i.ToString() + @".getCenter().lat();
                                            document.getElementById('" + _hfValue2[i].ClientID.ToString() + @"').value = map" + i.ToString() + @".getCenter().lng();
                                                  " + strShowLatLong + @" 
                                        });

                                    google.maps.event.addListener(map" + i.ToString() + @", 'zoom_changed', function () {
                                                    document.getElementById('" + _hfValue3[i].ClientID.ToString() + @"').value = map" + i.ToString() + @".getZoom();

                                                });
                                        " + strSearchAddressJS + @"
                                   // });

                                    ";

                            //if (strSearchAddressJS != "")
                            //    strMapJS = strMapJS + strSearchAddressJS;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "AutoCompleteJS" + i.ToString(), strMapJS, true);
                            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AutoCompleteJS" + i.ToString(), strMapJS, true);


                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchAddressJS" + i.ToString(), strSearchAddressJS, true);
                        }

                        //put default

                        if (_qsMode == "add")
                        {
                            if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                       && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                       && _iParentRecordID != null)
                            {
                                Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                if (theDefaultColumn != null && theParentRecord != null)
                                {
                                    string strDefaultValue = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);

                                    if (strDefaultValue != "")
                                    {
                                        try
                                        {
                                            LocationColumn theLocationColumn = JSONField.GetTypedObject<LocationColumn>(strDefaultValue);
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

                                    }

                                }

                            }

                        }

                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                    {
                        _chkValue[i] = new CheckBox();
                        _chkValue[i].ID = "chk" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
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
                        _htmValue[i].AssetManager = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";
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

                        if (_lblValue[i].Text.ToLower().IndexOf("[ParentRecordID]".ToLower()) > -1)
                        {
                            if (Request.QueryString["Recordid"] != null)
                            {
                                _lblValue[i].Text = _lblValue[i].Text.Replace("[ParentRecordID]", Request.QueryString["Recordid"].ToString());
                            }
                            else
                            {
                                _lblValue[i].Visible = false;
                            }

                        }


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





                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutListValues(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _lstValue[i]);
                        }
                        else if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text")
                        {
                            Common.PutListValues_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _lstValue[i]);
                        }
                        else
                        {
                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                                && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                           && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "")
                            {
                                Common.PutList_FromTable((int)_dtColumnsDetail.Rows[i]["TableTableID"], null, _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString(), ref _lstValue[i]);
                            }

                        }

                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _lstValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        //cell[(i * 2) + 1] = new HtmlTableCell();
                        cell[(i * 2) + 1].Controls.Add(_lstValue[i]);

                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _lstValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";
                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }

                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                        && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "checkbox")
                    {
                        _cblValue[i] = new CheckBoxList();
                        _cblValue[i].ID = "cbl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //_cblValue[i].SelectionMode = ListSelectionMode.Multiple;

                        //_cblValue[i].Style.Add("height", "120px");
                        _cblValue[i].Style.Add("display", "block");
                        _cblValue[i].Style.Add("overflow", "auto");
                        //_cblValue[i].Style.Add("width", "198px");

                        _cblValue[i].Style.Add("min-width", "198px");
                        _cblValue[i].Style.Add("min-height", "100px");
                        _cblValue[i].Style.Add("max-width", "350px");

                        //_cblValue[i].Height = 100;



                        _cblValue[i].Style.Add("border", "solid 1px black");



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

                        //_cblValue[i].CssClass = "multiple_cbl";
                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutCheckBoxListValues(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _cblValue[i]);
                        }
                        else if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text")
                        {
                            Common.PutCheckBoxListValues_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _cblValue[i]);
                        }
                        else
                        {
                            if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                               && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                          && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "")
                            {
                                Common.PutCheckBoxList_ForTable((int)_dtColumnsDetail.Rows[i]["TableTableID"], null, _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString(), ref _cblValue[i]);
                            }
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


                        //if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        //{

                        //    _rfvValue[i] = new RequiredFieldValidator();
                        //    _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        //    _rfvValue[i].Display = ValidatorDisplay.None;
                        //    _rfvValue[i].ControlToValidate = _cblValue[i].ClientID;
                        //    _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";
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

                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values")
                        {
                            Common.PutDDLValues(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _ddlValue[i]);

                            //if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            //{
                            //    if (_qsMode == "add")
                            //    {
                            //        _ddlValue[i].SelectedValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                            //    }
                            //}

                            if (Request.QueryString["mode"] != null)
                            {

                                if (_qsMode == "add")
                                {
                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                    {
                                        _ddlValue[i].SelectedValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                    }

                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                        && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                        && _iParentRecordID != null)
                                    {

                                        Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                        Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                        if (theDefaultColumn != null && theParentRecord != null)
                                        {
                                            //_ddlValue[i].SelectedValue = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);

                                            _ddlValue[i].SelectedValue = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            Common.PutDDLValue_Text(_dtColumnsDetail.Rows[i]["DropdownValues"].ToString(), ref _ddlValue[i]);

                            //if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            //{
                            //    if (_qsMode == "add")
                            //    {
                            //        string strDefaultValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                            //        if (strDefaultValue.IndexOf(",") > -1)
                            //        {
                            //            _ddlValue[i].SelectedValue = strDefaultValue.Substring(0, strDefaultValue.IndexOf(","));
                            //        }
                            //    }
                            //}
                            if (Request.QueryString["mode"] != null)
                            {

                                if (_qsMode == "add")
                                {
                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                    {
                                        string strDefaultValue = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                        if (strDefaultValue.IndexOf(",") > -1)
                                        {
                                            _ddlValue[i].SelectedValue = strDefaultValue.Substring(0, strDefaultValue.IndexOf(","));
                                        }
                                    }

                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                        && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                        && _iParentRecordID != null)
                                    {

                                        Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                        Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                        if (theDefaultColumn != null && theParentRecord != null)
                                        {
                                            //string strDefaultValue = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                            string strDefaultValue = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                            if (strDefaultValue.IndexOf(",") > -1)
                                            {
                                                _ddlValue[i].SelectedValue = strDefaultValue.Substring(0, strDefaultValue.IndexOf(","));
                                            }
                                        }

                                    }
                                }
                            }

                        }


                        if (_dtColumnsDetail.Rows[i]["Notes"] != DBNull.Value)
                        {
                            _ddlValue[i].ToolTip = _dtColumnsDetail.Rows[i]["Notes"].ToString();
                        }

                        // cell[(i * 2) + 1] = new HtmlTableCell();
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


                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _ddlValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";

                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

                        }


                        _imgWarning[i] = new Image();
                        _imgWarning[i].ImageUrl = "~/Images/warning.png";
                        _imgWarning[i].Visible = false;
                        //cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp;"));
                        cell[(i * 2) + 1].Controls.Add(_imgWarning[i]);

                    }

                    //if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "data_retriever")
                    //{
                    //    _txtValue[i] = new TextBox();
                    //    _txtValue[i].ID = "txt" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
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

                        if (_dtColumnsDetail.Rows[i]["TextType"].ToString() == "f"
                           && _dtColumnsDetail.Rows[i]["RegEx"].ToString() != "")
                        {
                            _lbl[i].Text = _lbl[i].Text + "&nbsp;" + _dtColumnsDetail.Rows[i]["RegEx"].ToString();
                        }

                        //cell[(i * 2) + 1] = new HtmlTableCell();



                        _ibValue[i] = new ImageButton();
                        _ibValue[i].ID = "ib" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                        _ibValue[i].ImageUrl = "~/Pages/Pager/Images/refresh.png";
                        _ibValue[i].Style.Add("padding-left", "5px");
                        _ibValue[i].Style.Add("width", "12px");
                        _ibValue[i].CausesValidation = true;
                        _ibValue[i].ToolTip = "Refresh to get the calculated value.";
                        _ibValue[i].Click += new ImageClickEventHandler(IB_CalRef_Click);

                        cell[(i * 2) + 1].Controls.Add(_txtValue[i]);
                        cell[(i * 2) + 1].Controls.Add(_ibValue[i]);
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

                            if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "8")
                            {
                                _txtValue[i].Enabled = false;
                                if (_qsMode != "")
                                {
                                    if (_qsMode.ToLower() == "add")
                                    {
                                        _txtValue[i].Text = "Assigned on Save";
                                    }
                                }

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



                            //if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                            //{
                            //    if (_qsMode == "add")
                            //    {
                            //        _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                            //    }
                            //}

                            if (Request.QueryString["mode"] != null)
                            {

                                if (_qsMode == "add")
                                {
                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                    {
                                        _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                    }

                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                        && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                        && _iParentRecordID != null)
                                    {

                                        Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                        Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                        if (theDefaultColumn != null && theParentRecord != null)
                                        {
                                            //_txtValue[i].Text = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                            _txtValue[i].Text = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                        }

                                    }
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

                            if (Request.QueryString["mode"] != null)
                            {

                                if (_qsMode == "add")
                                {
                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                    {
                                        _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                    }

                                    if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                        && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                        && _iParentRecordID != null)
                                    {

                                        Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                        Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                        if (theDefaultColumn != null && theParentRecord != null)
                                        {
                                            //_txtValue[i].Text = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);

                                            _txtValue[i].Text = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                        }

                                    }
                                }
                            }


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
                                            //if (_qsMode != "add")
                                            //{
                                            _hlValue[i] = new HyperLink();
                                            _hlValue[i].ID = "hl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                            _hlValue[i].Target = "_blank";
                                            _hlValue[i].Text = "Go";
                                            cell[(i * 2) + 1].Controls.Add(new LiteralControl("&nbsp"));
                                            cell[(i * 2) + 1].Controls.Add(_hlValue[i]);

                                            string strJSLink = @"$('#ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _txtValue[i].ID + @"').keypress(function () {
                                                 var strURL=document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _txtValue[i].ID + @"').value;
	                                                if (strURL.indexOf('http')==-1)
                                                            {
                                                            strURL='http://' + strURL;
                                                            }
                                                            document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _hlValue[i].ID + @"').href =strURL;

                                                   });
                                                        $('#ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _txtValue[i].ID + @"').change(function () {
                                                                var strURL=document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _txtValue[i].ID + @"').value;
	                                                            if (strURL.indexOf('http')==-1)
                                                                        {
                                                                        strURL='http://' + strURL;
                                                                        }
                                                                        document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _hlValue[i].ID + @"').href =strURL;

                                                                });


                                                        ";
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "linkURL" + i.ToString(), strJSLink, true);
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
                                        case "mobile":
                                            if (_dtColumnsDetail.Rows[i]["RegEx"] != DBNull.Value)
                                            {
                                                if (_dtColumnsDetail.Rows[i]["RegEx"].ToString() != "")
                                                {
                                                    _revValue[i].ValidationExpression = _dtColumnsDetail.Rows[i]["RegEx"].ToString();
                                                    _revValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Invalid Mobile Number!";
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


                        //
                        //_hlSensorInfo[i] = new HyperLink();
                        //cell[(i * 2) + 1].Controls.Add(_hlSensorInfo[i]);


                        //



                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number" && bSlider == false)
                        {

                            //Disabled calculation field
                            if (_dtColumnsDetail.Rows[i]["Calculation"].ToString() != "")
                            {
                                //_txtValue[i].ReadOnly = true;
                                if (Request.QueryString["mode"] != null)
                                {
                                    if (_qsMode == "add")
                                    {
                                        _txtValue[i].Text = "Calculated on Save";
                                    }

                                }

                                _txtValue[i].Enabled = false;

                            }


                            //Set constant/default

                            if (Request.QueryString["mode"] != null)
                            {

                                //make constant readonly
                                if (_dtColumnsDetail.Rows[i]["Constant"].ToString() != "")
                                {
                                    //_txtValue[i].ReadOnly = true;
                                    if (_qsMode == "add")
                                    {
                                        _txtValue[i].Text = _dtColumnsDetail.Rows[i]["Constant"].ToString();
                                    }
                                    _txtValue[i].Enabled = false;
                                }


                                //if (_dtColumnsDetail.Rows[i]["DefaultValue"].ToString() != "")
                                //{
                                //    if (_qsMode == "add")
                                //    {
                                //        _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                //    }
                                //}

                                if (Request.QueryString["mode"] != null)
                                {

                                    if (_qsMode == "add")
                                    {
                                        if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "value")
                                        {
                                            _txtValue[i].Text = _dtColumnsDetail.Rows[i]["DefaultValue"].ToString();
                                        }

                                        if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                            && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                            && _iParentRecordID != null)
                                        {

                                            Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                            Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                                            if (theDefaultColumn != null && theParentRecord != null)
                                            {
                                                //_txtValue[i].Text = RecordManager.GetRecordValue(ref theParentRecord, theDefaultColumn.SystemName);
                                                _txtValue[i].Text = TheDatabaseS.spGetValueFromRelatedTable((int)theParentRecord.RecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                            }

                                        }
                                    }
                                }

                            }


                            if (bool.Parse(_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString()) == false
                                && _dtColumnsDetail.Rows[i]["Calculation"].ToString() == "")
                            {
                                if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() != "8")
                                {
                                    _revValue[i] = new RegularExpressionValidator();
                                    _revValue[i].ID = "rev" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    _revValue[i].Display = ValidatorDisplay.None;
                                    _revValue[i].ControlToValidate = _txtValue[i].ClientID;
                                    _revValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "- Numeric value please!";
                                    //_revValue[i].ErrorMessage = " Numeric please!";
                                    _revValue[i].ValidationExpression = @"(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,15}$)";




                                    _ftbExt[i] = new FilteredTextBoxExtender();
                                    _ftbExt[i].ID = "ftb" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    _ftbExt[i].TargetControlID = _txtValue[i].ClientID;
                                    _ftbExt[i].FilterType = FilterTypes.Custom;
                                    _ftbExt[i].FilterMode = FilterModes.ValidChars;
                                    _ftbExt[i].ValidChars = "-.0123456789";

                                    cell[(i * 2) + 1].Controls.Add(_revValue[i]);

                                    cell[(i * 2) + 1].Controls.Add(_ftbExt[i]);
                                }
                            }



                            if (_dtColumnsDetail.Rows[i]["NumberType"] != null)
                            {
                                //Avg
                                if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "4")
                                {

                                    _txtValue[i].Enabled = false;

                                }

                                //      if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number" &&
                                //_dtColumnsDetail.Rows[i]["NumberType"] != null)
                                //      {
                                //          if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "8")
                                //          {
                                //              _txtValue[i].Enabled = false;
                                //              if (_qsMode != "")
                                //              {
                                //                  if (_qsMode.ToLower() == "add")
                                //                  {
                                //                      _txtValue[i].Text = "Assigned on Save";
                                //                  }
                                //              }
                                //              //if(_qsMode
                                //          }
                                //      }



                                //Record Count
                                if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "5")
                                {

                                    _txtValue[i].Visible = false;
                                    _lblValue[i] = new Label();
                                    _lblValue[i].ID = "lblV" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                                    //_lblValue[i].ClientIDMode = ClientIDMode.Static;

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


                        if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()))
                        {

                            _rfvValue[i] = new RequiredFieldValidator();
                            _rfvValue[i].ID = "rfv" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                            _rfvValue[i].Display = ValidatorDisplay.None;
                            _rfvValue[i].ControlToValidate = _txtValue[i].ClientID;
                            _rfvValue[i].ErrorMessage = _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " is Required";


                            cell[(i * 2) + 1].Controls.Add(_rfvValue[i]);

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

        } //end of the For




        //here Show and Hide code






        _lblWarningResults = new Label();
        _lblWarningResults.ID = "lblWarningResults";
        _lblWarningResults.Text = "Warning Results";
        _lblWarningResults.Font.Bold = true;
        _lblWarningResults.Visible = false;

        _lblWarningResultsValue = new Label();
        _lblWarningResultsValue.ID = "lblWarningResultsValue";
        _lblWarningResultsValue.Width = 300;
        _lblWarningResults.Visible = false;
        //_lblWarningResultsValue.Height = 50;
        //_lblWarningResultsValue.TextMode = TextBoxMode.MultiLine;
        //_lblWarningResultsValue.ReadOnly = true;
        //_txtWarningResults.Enabled = false;
        //_txtWarningResults.CssClass = "MultiLineTextBox";NormalTextBox
        //_lblWarningResultsValue.CssClass = "MultiLineTextBox";
        //_lblWarningResultsValue.BackColor = System.Drawing.Color.Gray;
        _lblWarningResultsValue.ForeColor = System.Drawing.Color.Blue;
        _lblWarningResultsValue.Visible = false;


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



        //HyperLink hlTest=new HyperLink();
        //hlTest.Text = " Hello this is test, how are you";
        //hlTest.NavigateUrl = "#";


        cell[_dtColumnsDetail.Rows.Count * 2] = new HtmlTableCell();
        //cellB[_dtColumnsDetail.Rows.Count * 2] = new HtmlTableCell();

        cell[_dtColumnsDetail.Rows.Count * 2].Align = "Right";
        cell[_dtColumnsDetail.Rows.Count * 2].Controls.Add(_lblWarningResults);
        cell[(_dtColumnsDetail.Rows.Count * 2) + 1] = new HtmlTableCell();
        //cellB[(_dtColumnsDetail.Rows.Count * 2) + 1] = new HtmlTableCell();

        cell[(_dtColumnsDetail.Rows.Count * 2) + 1].Controls.Add(_lblWarningResultsValue);

        //cell[(_dtColumnsDetail.Rows.Count * 2) + 1].Controls.Add(hlTest);

        trX[_dtColumnsDetail.Rows.Count] = new HtmlTableRow();
        //trXB[_dtColumnsDetail.Rows.Count] = new HtmlTableRow();

        trX[_dtColumnsDetail.Rows.Count].Cells.Add(cell[_dtColumnsDetail.Rows.Count * 2]);
        trX[_dtColumnsDetail.Rows.Count].Cells.Add(cell[(_dtColumnsDetail.Rows.Count * 2) + 1]);

        tblLeft.Rows.Add(trX[_dtColumnsDetail.Rows.Count]);


        //validation

        cell[(_dtColumnsDetail.Rows.Count + 1) * 2] = new HtmlTableCell();
        //cellB[(_dtColumnsDetail.Rows.Count + 1) * 2] = new HtmlTableCell();

        cell[(_dtColumnsDetail.Rows.Count + 1) * 2].Align = "Right";
        cell[(_dtColumnsDetail.Rows.Count + 1) * 2].Controls.Add(_lblValidationResults);
        cell[((_dtColumnsDetail.Rows.Count + 1) * 2) + 1] = new HtmlTableCell();
        //cellB[((_dtColumnsDetail.Rows.Count + 1) * 2) + 1] = new HtmlTableCell();

        cell[((_dtColumnsDetail.Rows.Count + 1) * 2) + 1].Controls.Add(_txtValidationResults);

        //cell[(_dtColumnsDetail.Rows.Count * 2) + 1].Controls.Add(hlTest);

        trX[_dtColumnsDetail.Rows.Count + 1] = new HtmlTableRow();
        //trXB[_dtColumnsDetail.Rows.Count + 1] = new HtmlTableRow();

        trX[_dtColumnsDetail.Rows.Count + 1].Cells.Add(cell[(_dtColumnsDetail.Rows.Count + 1) * 2]);
        trX[_dtColumnsDetail.Rows.Count + 1].Cells.Add(cell[((_dtColumnsDetail.Rows.Count + 1) * 2) + 1]);

        tblLeft.Rows.Add(trX[_dtColumnsDetail.Rows.Count + 1]);

        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Parent- Created  CONTROLS - Begin DATA load ";
            theSpeedLog.FunctionLineNumber = 3270;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }

        //added & updated

        if (Request.QueryString["mode"] != null)
        {
            if (_qsMode == "view")
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
                //cellB[(_dtColumnsDetail.Rows.Count + 2) * 2] = new HtmlTableCell();

                cell[(_dtColumnsDetail.Rows.Count + 2) * 2].Align = "Right";
                cell[(_dtColumnsDetail.Rows.Count + 2) * 2].Controls.Add(_lblAddedCaption);
                cell[((_dtColumnsDetail.Rows.Count + 2) * 2) + 1] = new HtmlTableCell();
                //cellB[((_dtColumnsDetail.Rows.Count + 2) * 2) + 1] = new HtmlTableCell();
                cell[((_dtColumnsDetail.Rows.Count + 2) * 2) + 1].Controls.Add(_lblAddedTimeEmail);
                trX[_dtColumnsDetail.Rows.Count + 2] = new HtmlTableRow();
                //trXB[_dtColumnsDetail.Rows.Count + 2] = new HtmlTableRow();

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
                //cellB[(_dtColumnsDetail.Rows.Count + 3) * 2] = new HtmlTableCell();

                cell[(_dtColumnsDetail.Rows.Count + 3) * 2].Align = "Right";
                cell[(_dtColumnsDetail.Rows.Count + 3) * 2].Controls.Add(_lblUpdatedCaption);
                cell[((_dtColumnsDetail.Rows.Count + 3) * 2) + 1] = new HtmlTableCell();
                //cellB[((_dtColumnsDetail.Rows.Count + 3) * 2) + 1] = new HtmlTableCell();

                cell[((_dtColumnsDetail.Rows.Count + 3) * 2) + 1].Controls.Add(_lblUpdatedTimeEmail);
                trX[_dtColumnsDetail.Rows.Count + 3] = new HtmlTableRow();
                //trXB[_dtColumnsDetail.Rows.Count + 3] = new HtmlTableRow();

                trX[_dtColumnsDetail.Rows.Count + 3].Cells.Add(cell[(_dtColumnsDetail.Rows.Count + 3) * 2]);
                trX[_dtColumnsDetail.Rows.Count + 3].Cells.Add(cell[((_dtColumnsDetail.Rows.Count + 3) * 2) + 1]);

                tblLeft.Rows.Add(trX[_dtColumnsDetail.Rows.Count + 3]);
            }
        }

        //




        if (!IsPostBack)
        {
            if (Request.QueryString["public"] == null && _iParentRecordID != null)
            {
                //add put the defalut value for parent table -- Request.QueryString["RecordID"] == null && 

                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                if (theParentRecord != null)
                {

                    DataTable dtRecordedetail = Common.DataTableFromText(@"SELECT ColumnID, DisplayTextDetail, SystemName
			                FROM [Column] 
			                WHERE TableID = " + _qsTableID + @" AND DisplayTextDetail IS NOT NULL 
			                AND LEN(DisplayTextDetail) > 0
			                ORDER BY DisplayOrder");


                    for (int i = 0; i < dtRecordedetail.Rows.Count; i++)
                    {
                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                               && _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                              && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                              && _dtColumnsDetail.Rows[i]["LinkedParentColumnID"] != DBNull.Value
                              && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "") //
                        {
                            if ((int)theParentRecord.TableID == int.Parse(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()))
                            {

                                if (_iParentRecordID.ToString() != "")
                                {
                                    if (_dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "" && _lstValue[i] != null)
                                    {
                                        Common.SetListValues_ForTable(_iParentRecordID.ToString(), ref _lstValue[i],
                                        (int)_dtColumnsDetail.Rows[i]["TableTableID"], null, _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString());
                                    }
                                    if (_dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "checkbox" && _cblValue[i] != null)
                                    {
                                        Common.SetCheckBoxListValues_ForTable(_iParentRecordID.ToString(), ref _cblValue[i],
                                            (int)_dtColumnsDetail.Rows[i]["TableTableID"], null, _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString());
                                    }


                                }
                            }
                            //
                        }


                    }

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

                                string strParentRecordID = _iParentRecordID.ToString();
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


            if (Request.QueryString["public"] != null && Request.QueryString["ParentID"] != null && _iParentRecordID != null)
            {
                Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);

                if (theParentRecord != null && theParentRecord.TableID.ToString() == Request.QueryString["ParentID"].ToString())
                {

                    DataTable dtRecordedetail = Common.DataTableFromText(@"SELECT ColumnID, DisplayTextDetail, SystemName
			                FROM [Column] 
			                WHERE TableID = " + _qsTableID + @" AND DisplayTextDetail IS NOT NULL 
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

                                string strParentRecordID = _iParentRecordID.ToString();
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
                    Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Public.aspx?ParentRecordID=" + _iParentRecordID.ToString() + "&TableID=" + _qsTableID, true);
                    return;

                }




            }

        }





        if (Request.QueryString["RecordID"] != null || _bCopyRecord == true)
        {



            


            if (_theTable != null && _bCopyRecord == false)
            {
                if (_theTable.HeaderName != "")
                {

                    try
                    {
                        //int iTN1 = 0;
                        //DataTable dtRecordInfo = RecordManager.ets_Record_List(int.Parse(_qsTableID), null, null, null, null, null, "", "",
                        //    null, null, ref iTN1, ref iTN1, "nonstandard", "", " AND Record.RecordID=" + _qsRecordID, null, null, "", "", "", null);

                        // string strHeader = _theTable.HeaderName;



                        //if (dtRecordInfo.Rows.Count > 0)
                        //{
                        //    foreach (DataColumn dc in dtRecordInfo.Columns)
                        //    {
                        //        strHeader = strHeader.Replace("[" + dc.ColumnName + "]", dtRecordInfo.Rows[0][dc.ColumnName].ToString());
                        //    }
                        //}



                        //if (_iParentRecordID != null)
                        //{
                        //    Record theParentRecord = RecordManager.ets_Record_Detail_Full((int)_iParentRecordID);
                        //    Table theParentTable = RecordManager.ets_Table_Details((int)theParentRecord.TableID);
                        //    DataTable dtRecordInfoP = RecordManager.ets_Record_List((int)theParentRecord.TableID, null, null, null, null, null, "", "",
                        //    null, null, ref iTN1, ref iTN1, "nonstandard", "", " AND Record.RecordID=" + theParentRecord.RecordID.ToString(), null, null, "", "", "", null);


                        //    if (dtRecordInfoP.Rows.Count > 0)
                        //    {
                        //        foreach (DataColumn dc in dtRecordInfoP.Columns)
                        //        {
                        //            strHeader = strHeader.Replace("[" + theParentTable.TableName + ":" + dc.ColumnName + "]", dtRecordInfoP.Rows[0][dc.ColumnName].ToString());
                        //        }
                        //    }

                        //}


                        //lblHeaderName.Text = strHeader;


                        lblHeaderName.Text = Common.GetLinkedDisplayText(_theTable.HeaderName, (int)_theTable.TableID, null, " AND Record.RecordID=" + _qsRecordID, "");

                    }
                    catch (Exception ex)
                    {

                        ErrorLog theErrorLog = new ErrorLog(null, "Header Name", ex.Message, ex.StackTrace, DateTime.Now, "");
                        SystemData.ErrorLog_Insert(theErrorLog);
                    }
                }
            }


           

          

            if (_qsMode == "edit")
                trReasonForChange.Visible = true;

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
                if (i == _iDateTimeRecorded && _bCopyRecord == false)
                {
                    //_txtValue[i].Text = _dtRecordedetail.Rows[0][i].ToString().Substring(0, _dtRecordedetail.Rows[0][i].ToString().IndexOf(' '));
                    DateTime dtTempDateTimeRecorded = DateTime.Parse(_dtRecordedetail.Rows[0][i].ToString());
                    _txtValue[i].Text = dtTempDateTimeRecorded.Day.ToString() + "/" + dtTempDateTimeRecorded.Month.ToString("00") + "/" + dtTempDateTimeRecorded.Year.ToString();

                    //if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() != "date")
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
                


                if (i == _iEnteredByIndex)
                {
                    if (_bCopyRecord)
                        continue;

                    _ddlEnteredBy.Text = _dtRecordedetail.Rows[0][i].ToString();
                }
                else if (i == _iIsActiveIndex)
                {
                    if (_bCopyRecord)
                        continue;
                    _chkIsActive.Checked = bool.Parse(_dtRecordedetail.Rows[0][i].ToString());

                }
                else if (i == _iDateTimeRecorded)
                {
                    if (_bCopyRecord)
                        continue;
                    //_txtValue[i].Text = _dtRecordedetail.Rows[0][i].ToString().Substring(0, _dtRecordedetail.Rows[0][i].ToString().IndexOf(' '));
                    DateTime dtTempDateTimeRecorded = DateTime.Parse(_dtRecordedetail.Rows[0][i].ToString());
                    _txtValue[i].Text = dtTempDateTimeRecorded.Day.ToString() + "/" + dtTempDateTimeRecorded.Month.ToString("00") + "/" + dtTempDateTimeRecorded.Year.ToString();

                    //if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() != "date")
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
                else if (i == _iTableIndex)
                {
                    //Table theTable = RecordManager.ets_Table_Details(int.Parse(_dtRecordedetail.Rows[0][i].ToString()));

                    _txtValue[i].Text = _theTable.TableName;
                }
                else if (_dtRecordedetail.Columns[i].ColumnName.ToLower() == "warningresults")
                {
                    if (_bCopyRecord)
                        continue;

                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                    {
                        //_lblWarningResults.Visible = true;
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
                    if (_bCopyRecord)
                        continue;
                    _txtValidationResults.Text = _dtRecordedetail.Rows[0][i].ToString();
                }
                else
                {

                    if (_bCopyRecord == true)
                    {
                        if (_dtColumnsDetail.Rows[i]["AllowCopy"] == null
                            || (_dtColumnsDetail.Rows[i]["AllowCopy"] != null &&
                            (_dtColumnsDetail.Rows[i]["AllowCopy"].ToString() == "" || (bool)_dtColumnsDetail.Rows[i]["AllowCopy"] == false)))
                        {
                            continue;
                        }

                    }


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
                            _txtValue[i].Text = dtTempDateTime.Day.ToString() + "/" + dtTempDateTime.Month.ToString("00") + "/" + dtTempDateTime.Year.ToString();
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
                            _txtValue[i].Text = Convert.ToDateTime(_dtRecordedetail.Rows[0][i].ToString()).ToString("HH:m"); //HH:mm:ss
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

                        _lblValue[i].Text = "<a target='_blank' href='" + "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Filedownload.aspx?FilePath="
                            + strFilePath + "&FileName=" + strFileName + "'>" +
                              _dtRecordedetail.Rows[0][i].ToString().Substring(37) + "</a>";

                        if (_qsMode == "view")
                        {
                            _fuValue[i].Visible = false;
                        }
                        else
                        {
                            _hfValue[i].Value = _dtRecordedetail.Rows[0][i].ToString();

                            _lblValue[i].Text = "<img  title=\"Remove this file\" style=\"cursor:pointer;\"  id=\"dimg" + _hfValue[i].ID + "\" src=\"" + "http://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />" + _lblValue[i].Text;

                            string strTempJS = @"  document.getElementById('dimg" + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _hfValue[i].ID + @"').value='';
                                                      $('#" + _lblValue[i].ID + @"').html(''); 
                                            });";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "filedelete" + i.ToString(), strTempJS, true);
                        }
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "trafficlight"
                        && _dtColumnsDetail.Rows[i]["TrafficLightColumnID"] != DBNull.Value
                        && _dtColumnsDetail.Rows[i]["TrafficLightValues"] != DBNull.Value)
                    {
                        Column theTrafficLightColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["TrafficLightColumnID"].ToString()));
                        if (theTrafficLightColumn != null && _imgValues[i] != null)
                        {
                            string strTLValue = Common.GetValueFromSQL("SELECT " + theTrafficLightColumn.SystemName + " FROM [Record] WHERE RecordID=" + _qsRecordID);
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

                        if (_qsMode == "view")
                        {
                            _fuValue[i].Visible = false;
                        }
                        else
                        {

                            _lblValue[i].Text = "<img title=\"Remove this image\" style=\"cursor:pointer;\"  id=\"dimg" + _hfValue[i].ID + "\" src=\"" + "http://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />" + _lblValue[i].Text;

                            _hfValue[i].Value = _dtRecordedetail.Rows[0][i].ToString();

                            string strTempJS = @"  document.getElementById('dimg" + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _hfValue[i].ID + @"').value='';
                                                      $('#" + _lblValue[i].ID + @"').html(''); 
                                            });";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "imagedelete" + i.ToString(), strTempJS, true);
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

                        //if (_dtColumnsDetail.Rows[i]["ParentColumnID"] == DBNull.Value)
                        //{
                        if (_dtRecordedetail.Rows[0][i].ToString() != "")
                        {
                            try
                            {
                                string strParentRecordID = _dtRecordedetail.Rows[0][i].ToString();
                                //Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()));

                                //string strLinkedColumnValue = _dtRecordedetail.Rows[0][i].ToString();

                                if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table")
                                {

                                    _hfValue[i].Value = strParentRecordID;
                                    _txtValue[i].Text = Common.GetLinkedDisplayText(_dtColumnsDetail.Rows[i]["DisplayColumn"].ToString(), int.Parse(_dtColumnsDetail.Rows[i]["TableTableID"].ToString()), null, " AND Record.RecordID=" + strParentRecordID, "");
                                    //                                        DataTable dtTableTableSC = Common.DataTableFromText(@"SELECT SystemName,DisplayName 
                                    //                                FROM [Column] WHERE   TableID ="
                                    //                                   + _dtColumnsDetail.Rows[i]["TableTableID"].ToString());

                                    //                                        string strDisplayColumn = _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString();

                                    //                                        foreach (DataRow dr in dtTableTableSC.Rows)
                                    //                                        {
                                    //                                            strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                                    //                                        }

                                    //                                        string sstrDisplayColumnOrg = strDisplayColumn;
                                    //                                        string strFilterSQL = "";
                                    //                                        if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                    //                                        {
                                    //                                            strFilterSQL = strLinkedColumnValue;
                                    //                                        }
                                    //                                        else
                                    //                                        {
                                    //                                            strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                                    //                                        }
                                    //                                        DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

                                    //                                        if (dtTheRecord.Rows.Count > 0)
                                    //                                        {
                                    //                                            strRecordID = dtTheRecord.Rows[0]["RecordID"].ToString();
                                    //                                            foreach (DataColumn dc in dtTheRecord.Columns)
                                    //                                            {

                                    //                                                Column theColumn = RecordManager.ets_Column_Details_By_Sys((int)theLinkedColumn.TableID, dc.ColumnName);
                                    //                                                if (theColumn != null)
                                    //                                                {
                                    //                                                    if (theColumn.ColumnType == "date")
                                    //                                                    {
                                    //                                                        string strDatePartOnly = dtTheRecord.Rows[0][dc.ColumnName].ToString();

                                    //                                                        if (strDatePartOnly.Length > 9)
                                    //                                                        {
                                    //                                                            strDatePartOnly = strDatePartOnly.Substring(0, 10);
                                    //                                                        }

                                    //                                                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", strDatePartOnly);
                                    //                                                    }
                                    //                                                    else
                                    //                                                    {
                                    //                                                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                    //                                                    }
                                    //                                                }

                                    //                                            }
                                    //                                        }
                                    //if (sstrDisplayColumnOrg != strDisplayColumn)
                                    //_txtValue[i].Text = strDisplayColumn;

                                }

                                if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                                {

                                    //_ddlValue[i].Text = strLinkedColumnValue;
                                    if (_ddlValue[i].Items.FindByValue(strParentRecordID) != null)
                                        _ddlValue[i].SelectedValue = strParentRecordID;

                                    //                                        if (_hlValue2[i] != null)
                                    //                                        {
                                    //                                            try
                                    //                                            {
                                    //                                                DataTable dtTableTableSC = Common.DataTableFromText(@"SELECT SystemName,DisplayName 
                                    //                                FROM [Column] WHERE   TableID ="
                                    //                                          + _dtColumnsDetail.Rows[i]["TableTableID"].ToString());

                                    //                                                string strDisplayColumn = _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString();

                                    //                                                foreach (DataRow dr in dtTableTableSC.Rows)
                                    //                                                {
                                    //                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                                    //                                                }

                                    //                                                string sstrDisplayColumnOrg = strDisplayColumn;
                                    //                                                string strFilterSQL = "";
                                    //                                                if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                    //                                                {
                                    //                                                    strFilterSQL = strLinkedColumnValue;
                                    //                                                }
                                    //                                                else
                                    //                                                {
                                    //                                                    strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                                    //                                                }

                                    //                                                DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

                                    //                                                if (dtTheRecord.Rows.Count > 0)
                                    //                                                {
                                    //                                                    strRecordID = dtTheRecord.Rows[0]["RecordID"].ToString();

                                    //                                                }

                                    //                                            }
                                    //                                            catch
                                    //                                            {

                                    //                                                //

                                    //                                            }
                                    //                                        }
                                }

                                if (_hlValue2[i] != null)
                                {


                                    try
                                    {
                                        //int iPRecordID = 0;
                                        //bool bIsRecord = false;
                                        //if (int.TryParse(strRecordID, out iPRecordID))
                                        //{
                                        //    Record thePaRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strRecordID));

                                        //    if (thePaRecord != null)
                                        //    {

                                        //        bIsRecord = true;
                                        //    }

                                        //}

                                        //if (strRecordID != "" && bIsRecord)
                                        //{

                                        //    // strRecordID is ok
                                        //}
                                        //else
                                        //{
                                        //    if (strRecordID != "" && bIsRecord == false)
                                        //    {
                                        //        try
                                        //        {
                                        //            Column theViewLinkLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()));

                                        //            DataTable dtTheRecord = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theViewLinkLinkedColumn.TableID.ToString() + " AND " + theViewLinkLinkedColumn.SystemName + "='" + strRecordID.Replace("'", "''") + "'");

                                        //            if (dtTheRecord.Rows.Count > 0)
                                        //            {
                                        //                strRecordID = dtTheRecord.Rows[0]["RecordID"].ToString();

                                        //            }

                                        //        }
                                        //        catch
                                        //        {
                                        //            //

                                        //        }

                                        //    }
                                        //}
                                        _hlValue2[i].NavigateUrl = _hlValue2[i].NavigateUrl + "&RecordID=" + Cryptography.Encrypt(strParentRecordID);

                                    }
                                    catch
                                    {
                                        _hlValue2[i].NavigateUrl = "#";
                                    }



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
                        //    //OldFiltered
                        //    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                        //    {
                        //        try
                        //        {
                        //            //this will not happen

                        //            Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()));

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

                        //            Record theRecord=null;
                        //            DataTable dtTheRecord = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);
                        //            if (dtTheRecord.Rows.Count > 0)
                        //            {
                        //                foreach (DataRow drR in dtTheRecord.Rows)
                        //                {
                        //                    theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(drR["RecordID"].ToString()));
                        //                    break;
                        //                }
                        //            }

                        //            Column scParentColumnID = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ParentColumnID"].ToString()));
                        //            if (scParentColumnID != null)
                        //            {
                        //                if(theRecord!=null)
                        //                _ddlValue2[i].SelectedValue = RecordManager.GetRecordValue(ref theRecord, scParentColumnID.SystemName);

                        //                _ccddl[i].SelectedValue = _dtRecordedetail.Rows[0][i].ToString();
                        //            }

                        //            if (_hlValue2[i] != null)
                        //            {

                        //                try
                        //                {
                        //                    int iPRecordID = 0;
                        //                    bool bIsRecord = false;
                        //                    if (int.TryParse(strLinkedColumnValue, out iPRecordID))
                        //                    {
                        //                        Record thePaRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strLinkedColumnValue));

                        //                        if (thePaRecord != null)
                        //                        {

                        //                            bIsRecord = true;
                        //                        }

                        //                    }

                        //                    if (strLinkedColumnValue != "" && bIsRecord)
                        //                    {

                        //                        // strRecordID is ok
                        //                    }
                        //                    else
                        //                    {
                        //                        if (strLinkedColumnValue != "" && bIsRecord == false)
                        //                        {
                        //                            try
                        //                            {
                        //                                Column theViewLinkLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["LinkedParentColumnID"].ToString()));

                        //                                DataTable dtviewLinkTheRecord = Common.DataTableFromText("SELECT RecordID FROM Record WHERE TableID=" + theViewLinkLinkedColumn.TableID.ToString() + " AND " + theViewLinkLinkedColumn.SystemName + "='" + strLinkedColumnValue.Replace("'", "''") + "'");

                        //                                if (dtviewLinkTheRecord.Rows.Count > 0)
                        //                                {
                        //                                    strLinkedColumnValue = dtviewLinkTheRecord.Rows[0]["RecordID"].ToString();

                        //                                }

                        //                            }
                        //                            catch
                        //                            {
                        //                                //

                        //                            }

                        //                        }
                        //                    }
                        //                    _hlValue2[i].NavigateUrl = _hlValue2[i].NavigateUrl + "&RecordID=" + Cryptography.Encrypt(strLinkedColumnValue);

                        //                }
                        //                catch
                        //                {
                        //                    _hlValue2[i].NavigateUrl = "#";
                        //                }


                        //                //_hlValue2[i].NavigateUrl = _hlValue2[i].NavigateUrl + "&RecordID=" + Cryptography.Encrypt(strLinkedColumnValue);
                        //            }
                        //        }
                        //        catch
                        //        {
                        //            //
                        //        }
                        //    }

                        //}



                    }


                    //if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "data_retriever")
                    //{
                    //    if (_dtColumnsDetail.Rows[i]["DataRetrieverID"] != DBNull.Value)
                    //    {
                    //        DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail(int.Parse(_dtColumnsDetail.Rows[i]["DataRetrieverID"].ToString()),null,null);

                    //        if(theDataRetriever.CodeSnippet!="")
                    //        {

                    //            _txtValue[i].Text = Common.GetValueFromSQL(theDataRetriever.CodeSnippet.Replace("#ID#", _iRecordID.ToString()));
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

                            }
                            if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                               && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "t")
                            {
                                bTextCal = true;

                            }

                            if (_dtColumnsDetail.Rows[i]["RoundNumber"] != DBNull.Value
                                && bDateCal == false && bTextCal == false)
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
                                            _txtValue[i].Text = Math.Round(double.Parse(_txtValue[i].Text), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                                        }

                                    }
                                    catch
                                    {
                                        //
                                    }
                                }
                            }

                            if (_txtValue[i].Text != "" && bDateCal == false && bTextCal==false)
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

                        //if (_dtColumnsDetail.Rows[i]["Calculation"] != DBNull.Value)
                        //{
                        //    bool bDateCal = false;
                        //    if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                        //        && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "d")
                        //    {
                        //        bDateCal = true;
                        //        string strCalculation = _dtColumnsDetail.Rows[i]["Calculation"].ToString();

                        //        try
                        //        {
                        //            _txtValue[i].Text = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, _iRecordID, _iParentRecordID,
                        //                _dtColumnsDetail.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString());
                        //        }
                        //        catch
                        //        {
                        //            //
                        //        }

                        //    }
                        //    else
                        //    {
                        //        string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtColumnsDetail.Rows[i]["Calculation"].ToString());
                        //        _txtValue[i].Text = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, _iRecordID, i, _iParentRecordID);

                        //    }


                        //    if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value
                        //        && bDateCal==false)
                        //    {

                        //            if (_txtValue[i].Text.ToString() != "")
                        //            {
                        //                try
                        //                {
                        //                    if (_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
                        //                    {

                        //                        _txtValue[i].Text = Math.Round(double.Parse(Common.IgnoreSymbols(_txtValue[i].Text)), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                        //                    }
                        //                    else
                        //                    {
                        //                        _txtValue[i].Text = Math.Round(double.Parse(_txtValue[i].Text), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                        //                    }

                        //                }
                        //                catch
                        //                {
                        //                    //
                        //                }

                        //            }


                        //    }

                        //    if (_txtValue[i].Text != "" && bDateCal==false)
                        //    {
                        //        try
                        //        {
                        //            _txtValue[i].Text = Common.IgnoreSymbols(_txtValue[i].Text);

                        //            _txtValue[i].Text = double.Parse(_txtValue[i].Text).ToString("C").Substring(1);
                        //            if (_revValue[i] != null)
                        //                _revValue[i].Enabled = false;

                        //            if (_ftbExt[i] != null)
                        //                _ftbExt[i].Enabled = false;
                        //        }
                        //        catch
                        //        {

                        //        }

                        //    }


                        //}
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
                                    strLinkURL = "http://" + strLinkURL;
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

                                        DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,ColumnID FROM [Column] WHERE   TableID=" + _dtColumnsDetail.Rows[i]["TableTableID"].ToString() + " AND TableTableID=" + _qsTableID);
                                        foreach (DataRow drCT in dtTemp.Rows)
                                        {

                                            Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(drCT["ColumnID"].ToString()));
                                            Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                                            Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(_qsRecordID));
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
                                       "", "", 0, 1, ref iTNTemp, ref _iTotalDynamicColumnsTem, "", "", strTextSearch, null, null, "", "", "", null, ref strReturnSQL, ref strReturnSQL);

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




                        //check warning and validation
                        if (strWarning.IndexOf("WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                        {
                            _txtValue[i].ForeColor = System.Drawing.Color.Blue;
                        }
                        if (strWarning.IndexOf("EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                        {
                            _txtValue[i].ForeColor = System.Drawing.Color.Orange;
                        }
                        if (strWarning.IndexOf("INVALID (and ignored): " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
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
                            if (strWarning.IndexOf("WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range") >= 0)
                            {
                                _imgWarning[i].Visible = true;
                                strToopTip = strEachFormulaW_Msg;// "Value outside accepted range(" + strEachFormulaW + ").";
                                _imgWarning[i].ToolTip = strToopTip;
                            }

                            if (strWarning.IndexOf("EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range") >= 0)
                            {
                                _imgWarning[i].Visible = true;
                                strToopTip = strEachFormulaE_Msg;// "Value outside accepted range(" + strEachFormulaE + ").";
                                _imgWarning[i].ToolTip = strToopTip;
                                _imgWarning[i].ImageUrl = _imgWarning[i].ImageUrl.Replace("warning.png", "exceedance.png");
                            }

                            if (strWarning.IndexOf("INVALID (and ignored): " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() ) >= 0)
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
                            if (strWarning.IndexOf("WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.") >= 0)
                            {
                                _imgWarning[i].Visible = true;
                                _imgWarning[i].ToolTip = strToopTip + "Unlikely data – outside 3 standard deviations.";
                            }
                        }
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

                    //if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "staticcontent")
                    //{
                    //    if (_dtColumnsDetail.Rows[i]["DropDownValues"].ToString() != "")
                    //    {
                    //        _lblValue[i].Text = Server.HtmlDecode(_dtColumnsDetail.Rows[i]["DropDownValues"].ToString());
                    //    }
                    //}


                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                        && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "")
                    {
                        if (_dtRecordedetail.Rows[0][i].ToString() != "")
                        {
                            try
                            {
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
                            if (strWarning.IndexOf("WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                            {
                                _ddlValue[i].ForeColor = System.Drawing.Color.Blue;
                            }

                            if (strWarning.IndexOf("EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                            {
                                _ddlValue[i].ForeColor = System.Drawing.Color.Orange;
                            }


                            if (strValidation.IndexOf(": " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                            {
                                _ddlValue[i].ForeColor = System.Drawing.Color.Red;
                            }

                            //check specific warning
                            string strToopTip = "";
                            if (strWarning != "")
                            {
                                

                                if (strWarning.IndexOf("WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range") >= 0)
                                {
                                    _imgWarning[i].Visible = true;
                                    strToopTip = strEachFormulaW_Msg;// "Value outside accepted range(" + strEachFormulaW + ").";
                                    _imgWarning[i].ToolTip = strToopTip;
                                }
                                if (strWarning.IndexOf("EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range") >= 0)
                                {
                                    _imgWarning[i].Visible = true;
                                    strToopTip = strEachFormulaE_Msg;// "Value outside accepted range(" + strEachFormulaE + ").";
                                    _imgWarning[i].ToolTip = strToopTip;
                                    _imgWarning[i].ImageUrl = _imgWarning[i].ImageUrl.Replace("warning.png", "exceedance.png");
                                }

                                if (strWarning.IndexOf("INVALID (and ignored): " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString()) >= 0)
                                {
                                    _imgWarning[i].Visible = true;
                                    strToopTip = strEachFormulaV_Msg;// "INVALID (and ignored):" + strEachFormulaV + ".";
                                    _imgWarning[i].ToolTip = strToopTip;
                                    _imgWarning[i].ImageUrl = _imgWarning[i].ImageUrl.Replace("warning.png", "Invalid.png");
                                    _imgWarning[i].ImageUrl = _imgWarning[i].ImageUrl.Replace("exceedance.png", "Invalid.png");
                                }
                            }
                        }

                    }

                }
            }
            // END OF Populate record








            //try
            //{
            //    for (int i = 0; i < _dtRecordedetail.Columns.Count; i++)
            //    {
            //        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
            //               && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
            //               || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
            //               && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
            //                && _dtColumnsDetail.Rows[i]["FilterParentColumnID"] != DBNull.Value
            //                 && _dtColumnsDetail.Rows[i]["FilterOtherColumnID"] != DBNull.Value
            //               && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != ""
            //               )
            //        {
            //            for (int j = 0; j < _dtColumnsDetail.Rows.Count; j++)
            //            {
            //                if (_dtColumnsDetail.Rows[i]["FilterOtherColumnID"].ToString() ==
            //                    _dtColumnsDetail.Rows[j]["ColumnID"].ToString())
            //                {
            //                    if (_ddlValue[i] != null && _txtValue[j] != null)
            //                    {
            //                        _ddlValue[i].Items.Clear();
            //                        RecordManager.PopulateTableDropDownWithFilter(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), ref _ddlValue[i], _txtValue[j].Text);

            //                        try
            //                        {
            //                            _ddlValue[i].SelectedValue = _dtRecordedetail.Rows[0][i].ToString();
            //                        }
            //                        catch
            //                        {
            //                            //
            //                        }
            //                    }

            //                }

            //            }

            //        }
            //    }
            //}
            //catch
            //{
            //    //
            //}


            if (Session["RunSpeedLog"] != null)
            {
                SpeedLog theSpeedLog = new SpeedLog();
                theSpeedLog.FunctionName = _theTable.TableName + " Parent- DATA load done - Begin all child TAB";
                theSpeedLog.FunctionLineNumber = 4985;
                SecurityManager.AddSpeedLog(theSpeedLog);
            }


            MakeChildTables();


            if (Session["RunSpeedLog"] != null)
            {
                SpeedLog theSpeedLog = new SpeedLog();
                theSpeedLog.FunctionName = _theTable.TableName + " Parent- End child TAB";
                theSpeedLog.FunctionLineNumber = 4985;
                SecurityManager.AddSpeedLog(theSpeedLog);
            }

        }
        else
        {
            //add mode
            //MakeChildTables();
        }







        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {
            if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                        && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"
                        || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                        && _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                         && _dtColumnsDetail.Rows[i]["FilterParentColumnID"] != DBNull.Value
                          && _dtColumnsDetail.Rows[i]["FilterOtherColumnID"] != DBNull.Value
                        && _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != ""
                        )
            {
                for (int j = 0; j < _dtColumnsDetail.Rows.Count; j++)
                {
                    if (_dtColumnsDetail.Rows[i]["FilterOtherColumnID"].ToString() ==
                        _dtColumnsDetail.Rows[j]["ColumnID"].ToString())
                    {

                        if (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "tabledd")
                        {
                            if (_ddlValue[i] != null && _ddlValue[j] != null)
                            {

                                _ccddl[i] = new CascadingDropDown();
                                _ccddl[i].ID = "ccddl" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();

                                _ccddl[i].Category = _dtColumnsDetail.Rows[i]["ColumnID"].ToString() + "," + _dtColumnsDetail.Rows[i]["FilterParentColumnID"].ToString();
                                _ccddl[i].TargetControlID = _ddlValue[i].ID;


                                _ccddl[i].ParentControlID = _ddlValue[j].ID; //"ddl" + _dtColumnsDetail.Rows[j]["SystemName"].ToString();


                                _ccddl[i].ServicePath = "~/CascadeDropdown.asmx";
                                _ccddl[i].ServiceMethod = "GetFilteredData"; //filtered

                                if (_qsMode.ToLower() != "add")
                                {
                                    _ccddl[i].SelectedValue = _dtRecordedetail.Rows[0][i].ToString();
                                }

                                cell[(i * 2) + 1].Controls.Add(_ccddl[i]);
                            }


                            //                            if (_ddlValue[i] != null && _txtValue[j] != null)
                            //                            {

                            //                                //it's a Textbox
                            //                                string strTextChange = @" 
                            //                               
                            //                                
                            //                                $('#" + _txtValue[j].ClientID.ToString() + @"').change(function (){
                            //                                    
                            //                                        $.ajax({
                            //                                            url: ""../../CascadeDropdown.asmx/GetFilteredValue_TB"",
                            //                                            data: ""{'Columnid':'" + _dtColumnsDetail.Rows[i]["ColumnID"].ToString() + @"', 'search': '""+document.getElementById('" + _txtValue[j].ClientID.ToString() + @"').value +""' }"",
                            //                                            dataType: ""json"",
                            //                                            type: ""POST"",
                            //                                            contentType: ""application/json; charset=utf-8"",
                            //                                            dataFilter: function (data) { return data; },
                            //                                            success: function (response) {
                            //                                                    var branches = response.d;
                            //                                                    $('#" + _ddlValue[i].ClientID.ToString() + @"').empty();  
                            //                                                    for (var i in branches) {
                            //                                                        $('#" + _ddlValue[i].ClientID.ToString() + @"').append('<option value=""' + branches[i].ID + '"" >' + branches[i].Text + '</option>');
                            //                                                    }
                            //                                               }
                            //                                            
                            //                                        });
                            //                                    
                            //                                });
                            //                                   // $('#" + _txtValue[j].ClientID.ToString() + @"').trigger('change');
                            //                                     
                            //                                ";

                            //                                if (_qsMode.ToLower() != "add")
                            //                                {
                            //                                    //strTextChange = strTextChange + "$('#" + _txtValue[j].ClientID.ToString() + @"').trigger('change');";

                            //                                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                            //                                    {

                            //                                        if (!IsPostBack)
                            //                                        {
                            //                                            _ddlValue[i].Items.Clear();
                            //                                            RecordManager.PopulateTableDropDownWithFilter(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), ref _ddlValue[i], _txtValue[j].Text);
                            //                                            _ddlValue[i].SelectedValue = _dtRecordedetail.Rows[0][i].ToString();
                            //                                        }
                            //                                    }
                            //                                    else
                            //                                    {
                            //                                        strTextChange = strTextChange + " $('#" + _ddlValue[i].ClientID.ToString() + @"').empty(); ";
                            //                                    }
                            //                                }
                            //                                else
                            //                                {
                            //                                    strTextChange = strTextChange + " $('#" + _ddlValue[i].ClientID.ToString() + @"').empty(); ";
                            //                                }

                            //                                ScriptManager.RegisterStartupScript(this, this.GetType(), "strTextChange" + i.ToString(), strTextChange, true);


                            //                            }

                            //when other control is checkbox
                            //                            if (_ddlValue[i] != null && _chkValue[j] != null)
                            //                            {
                            //                                string strTrue = "";
                            //                                string strFalse = "";
                            //                                GetCheckTcikedUnTicked(_dtColumnsDetail.Rows[j]["DropDownValues"].ToString(), ref strTrue, ref strFalse);

                            //                                string chkValue = "";

                            //                                if (_chkValue[j].Checked)
                            //                                {
                            //                                    chkValue = strTrue;
                            //                                }
                            //                                else
                            //                                {
                            //                                    chkValue = strFalse;
                            //                                }

                            //                                string strTextChange = @"   
                            //                               
                            //                        
                            //                                
                            //                                $('#" + _chkValue[j].ClientID.ToString() + @"').change(function (){   
                            //
                            //                                    var chk = document.getElementById('" + _chkValue[j].ClientID.ToString() + @"');  
                            //                                    var strCheckValue='';
                            //                                    if (chk.checked == true) { strCheckValue = '" + strTrue + @"'; }
                            //                                    if (chk.checked == false) { strCheckValue = '" + strFalse + @"'; } 
                            //                                 
                            //                                        $.ajax({
                            //                                            url: ""../../CascadeDropdown.asmx/GetFilteredValue_TB"",
                            //                                            data: ""{'Columnid':'" + _dtColumnsDetail.Rows[i]["ColumnID"].ToString() + @"', 'search': '"" + strCheckValue + ""' }"",
                            //                                            dataType: ""json"",
                            //                                            type: ""POST"",
                            //                                            contentType: ""application/json; charset=utf-8"",
                            //                                            dataFilter: function (data) { return data; },
                            //                                            success: function (response) {
                            //                                                    var branches = response.d;
                            //                                                    $('#" + _ddlValue[i].ClientID.ToString() + @"').empty();  
                            //                                                    for (var i in branches) {
                            //                                                        $('#" + _ddlValue[i].ClientID.ToString() + @"').append('<option value=""' + branches[i].ID + '"" >' + branches[i].Text + '</option>');
                            //                                                    }
                            //                                               }                                            
                            //                                        });                                    
                            //                                });                                
                            //                                     
                            //                                ";

                            //                                if (_qsMode.ToLower() != "add")
                            //                                {
                            //                                    if (_dtRecordedetail.Rows[0][i].ToString() != "")
                            //                                    {
                            //                                        if (!IsPostBack)
                            //                                        {
                            //                                            _ddlValue[i].Items.Clear();
                            //                                            RecordManager.PopulateTableDropDownWithFilter(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), ref _ddlValue[i], chkValue);
                            //                                            _ddlValue[i].SelectedValue = _dtRecordedetail.Rows[0][i].ToString();
                            //                                        }
                            //                                    }
                            //                                    else
                            //                                    {
                            //                                        strTextChange = strTextChange + " $('#" + _ddlValue[i].ClientID.ToString() + @"').empty(); ";
                            //                                    }
                            //                                }
                            //                                else
                            //                                {
                            //                                    strTextChange = strTextChange + " $('#" + _ddlValue[i].ClientID.ToString() + @"').empty(); ";
                            //                                }


                            //                                ScriptManager.RegisterStartupScript(this, this.GetType(), "chkTextChange" + i.ToString(), strTextChange, true);

                            //                            }






                        }

                    }
                }


            }

        }


        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Parent- Start ShowWhen";
            theSpeedLog.FunctionLineNumber = 5260;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }




        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        {
            bool bValidationCanIgnore = false;
            string sCanignore = "no";
            if (_dtColumnsDetail.Rows[i]["ValidationCanIgnore"] != DBNull.Value)
            {
                if ((bool)_dtColumnsDetail.Rows[i]["ValidationCanIgnore"])
                {
                    bValidationCanIgnore = true;
                    sCanignore = "yes";
                }
            }

            if (_dtColumnsDetail.Rows[i]["CompareColumnID"] != DBNull.Value
                && _dtColumnsDetail.Rows[i]["CompareOperator"] != DBNull.Value && bValidationCanIgnore == false)
            {

                //string strCompareTableID = Common.GetValueFromSQL("SELECT TableID FROM [Column] WHERE ColumnID=" + _dtColumnsDetail.Rows[i]["CompareColumnID"].ToString());
                Column theComparisonColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["CompareColumnID"].ToString()));

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


                if (theComparisonColumn.TableID.ToString() != "" && theComparisonColumn.TableID.ToString() == _qsTableID)
                {

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime"
                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                    {
                        _cusvValue[i] = new CustomValidator();
                        _cusvValue[i].ID = "cusv" + _dtColumnsDetail.Rows[i]["SystemName"];
                        _cusvValue[i].ErrorMessage = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " " + Common.CompareOperatorErrorMsg(_dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " ";





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
                                string strJSCustomValidation = @" function compareTime" + i.ToString() + @"(sender, args) {
                                try
                                {
                                    var start = document.getElementById('" + strControlToCompare + @"');
                                    var end = document.getElementById('" + _txtValue[i].ClientID + @"');

                                    if(start.value.trim()=='' || end.value.trim()=='')
                                    { args.IsValid=true; }
                                    else
                                    {
                                        var time = start.value.split(':');  

                                        var d = new Date(); 

                                        d.setHours  (time[0]); 
                                        d.setMinutes(time[1]);


                                        var time2 = end.value.split(':');  

                                        var d2 = new Date(); 

                                        d2.setHours  (time2[0]); 
                                        d2.setMinutes(time2[1]);
                             
                                       // alert(d + ' and ' + d2);
                                        args.IsValid = (d2.getTime()" + strComparerOperator + @"d.getTime());
                                    }
                                }
                                catch(err)
                                {
                                //
                                }
                            }";

                                _cusvValue[i].ClientValidationFunction = "compareTime" + i.ToString();
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
                                string strJSCustomValidation = @" function compareTime" + i.ToString() + @"(sender, args) {
                                try
                                {
                                    var startD = document.getElementById('" + strControlToCompareDate + @"');
                                    var startT = document.getElementById('" + strControlToCompareTime + @"');
                                    var endD = document.getElementById('" + _txtValue[i].ClientID + @"');
                                    var endT = document.getElementById('" + _txtTime[i].ClientID + @"');                             
                                    //alert(startD.value);alert(endD.value);
                                    if(startD.value.trim()=='' || endD.value.trim()=='' || startD.value.trim()=='dd/mm/yyyy' || endD.value.trim()=='dd/mm/yyyy')
                                        { args.IsValid=true; }
                                    else
                                    {
                                   var d = new Date(startD.value.replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') + ' ' + startT.value); 
                                  var d2 = new Date(endD.value.replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') + ' ' + endT.value); 
                                    args.IsValid = (d2.getTime()" + strComparerOperator + @"d.getTime());
                                    }
                                }
                                catch(err)
                                {
                                //
                                }
                            }";

                                _cusvValue[i].ClientValidationFunction = "compareTime" + i.ToString();
                                cell[(i * 2) + 1].Controls.Add(_cusvValue[i]);

                                if (_cvValue[i].ControlToCompare != "" && _cvValue[i].ControlToValidate != "")
                                    cell[(i * 2) + 1].Controls.Add(_cvValue[i]);

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCustomValidation" + i.ToString(), strJSCustomValidation, true);

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
                        _cvValue[i].ErrorMessage = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " " + Common.CompareOperatorErrorMsg(_dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " ";

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

                        if (_cblValue[i] != null)
                        {
                            //_cvValue[i].ControlToValidate = _cblValue[i].ID;
                            //_cvValue[i] = null;
                        }


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

                                _cvValue[i].ErrorMessage = _cvValue[i].ErrorMessage + _dtColumnsDetail.Rows[j]["DisplayName"].ToString();

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

                if (theComparisonColumn.TableID.ToString() != "" && theComparisonColumn.TableID.ToString() != _qsTableID && _iParentRecordID != null)
                {
                    string strComparisonValue = TheDatabaseS.spGetValueFromRelatedTable((int)_iParentRecordID, (int)theComparisonColumn.TableID, theComparisonColumn.SystemName);

                    if (strComparisonValue != "")
                    {
                        _cusvValue[i] = new CustomValidator();
                        _cusvValue[i].ID = "cusv" + _dtColumnsDetail.Rows[i]["SystemName"];
                        _cusvValue[i].ErrorMessage = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " " + Common.CompareOperatorErrorMsg(_dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " ";

                        string strJSCustomValidation = "";

                        _cusvValue[i].ErrorMessage = _cusvValue[i].ErrorMessage + theComparisonColumn.DisplayName;

                        string strControlToValidateClientID = "";
                        if (_ddlValue[i] != null)
                        {
                            _cusvValue[i].ControlToValidate = _ddlValue[i].ID;
                            strControlToValidateClientID = _ddlValue[i].ClientID;
                        }

                        if (_ddlValue2[i] != null)
                        {
                            _cusvValue[i].ControlToValidate = _ddlValue2[i].ID;
                            strControlToValidateClientID = _ddlValue2[i].ClientID;
                        }
                        if (_chkValue[i] != null)
                        {
                            _cusvValue[i].ControlToValidate = _chkValue[i].ID;
                            strControlToValidateClientID = _chkValue[i].ClientID;
                        }
                        if (_lstValue[i] != null)
                        {
                            _cusvValue[i].ControlToValidate = _lstValue[i].ID;
                            strControlToValidateClientID = _lstValue[i].ClientID;
                        }
                        if (_cblValue[i] != null)
                        {
                            _cusvValue[i].ControlToValidate = _cblValue[i].ID; //??
                            strControlToValidateClientID = _cblValue[i].ClientID;
                        }
                        if (_radioList[i] != null)
                        {
                            _cvValue[i].ControlToValidate = _radioList[i].ID;
                            strControlToValidateClientID = _radioList[i].ClientID;
                        }
                        if (_lstValue[i] != null)
                        {
                            _cvValue[i].ControlToValidate = _lstValue[i].ID;
                            strControlToValidateClientID = _lstValue[i].ClientID;
                        }
                        if (_txtValue[i] != null && _txtTime[i] == null)
                        {
                            _cusvValue[i].ControlToValidate = _txtValue[i].ID;
                            strControlToValidateClientID = _txtValue[i].ClientID;
                        }


                        if (_txtTime[i] != null)
                        {
                            _cusvValue[i].ControlToValidate = _txtTime[i].ID;
                            strControlToValidateClientID = _txtTime[i].ClientID;
                        }

                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                        {

                            if (strComparisonValue.IndexOf(" ") > -1)
                            {
                                strComparisonValue = strComparisonValue.Substring(strComparisonValue.IndexOf(" ") + 1);
                                strComparisonValue = strComparisonValue.Trim();

                            }

                            strJSCustomValidation = @" function compareTime" + i.ToString() + @"(sender, args) {
                                    try
                                    {
                                         var start = '" + strComparisonValue + @"';
                                        var end = document.getElementById('" + _txtValue[i].ClientID + @"');


                                     if(start.trim()=='' || end.value.trim()=='')
                                    { args.IsValid=true; }
                                    else
                                    {
                                            var time = start.split(':');  

                                            var d = new Date(); 

                                            d.setHours  (time[0]); 
                                            d.setMinutes(time[1]);


                                            var time2 = end.value.split(':');  

                                            var d2 = new Date(); 

                                            d2.setHours  (time2[0]); 
                                            d2.setMinutes(time2[1]);
                             
                                           // alert(d + ' and ' + d2);
                                            args.IsValid = (d2.getTime()" + strComparerOperator + @"d.getTime());
                                        }
                                    }
                                    catch(err)
                                    {
                                    //
                                    }
                                }";



                        }
                        else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                        {
                            strJSCustomValidation = @" function compareTime" + i.ToString() + @"(sender, args) {
                                try
                                {
                                    var startDT = '" + strComparisonValue + @"';
                                    var endD = document.getElementById('" + _txtValue[i].ClientID + @"');
                                    var endT = document.getElementById('" + _txtTime[i].ClientID + @"'); //alert('called'); 
                                    if(startDT.trim()=='' || endD.value.trim()=='' || endD.value.trim()=='dd/mm/yyyy')
                                        { args.IsValid=true; }
                                    else
                                    {     
                                       // alert('');                               
                                        var d = new Date(startDT.replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') ); 
                                        var d2 = new Date(endD.value.replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') + ' ' + endT.value); 

                                         args.IsValid = (d2.getTime()" + strComparerOperator + @"d.getTime());
                                    }           
                                }
                                catch(err)
                                {
                                 //alert(err.message);
                                }
                            }";
                        }
                        else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                        {
                            strJSCustomValidation = @" function compareTime" + i.ToString() + @"(sender, args) {
                                try
                                {
                                    var startDT = '" + strComparisonValue + @"';
                                    var endD = document.getElementById('" + _txtValue[i].ClientID + @"');
                                    if(startDT.trim()=='' || endD.value.trim()=='' || endD.value.trim()=='dd/mm/yyyy')
                                        { args.IsValid=true; }
                                    else
                                    {    

                                       var d = new Date(startDT.replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3') ); 
                                      var d2 = new Date(endD.value.replace( /(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3')); 
                                        args.IsValid = (d2.getTime()" + strComparerOperator + @"d.getTime());
                                    }
                                 }
                                catch(err)
                                {
                                //
                                }

                            }";
                        }
                        else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                        {
                            strJSCustomValidation = @" function compareTime" + i.ToString() + @"(sender, args) {
                                try
                                {
                                    var startDT = " + strComparisonValue + @";
                                    var endD = document.getElementById('" + strControlToValidateClientID + @"');    
                                    if(startDT.trim()=='' || endD.value.trim()=='' )
                                        { args.IsValid=true; }
                                    else
                                    {  
                          
                                        args.IsValid = (parseFloat(endD.value,10)" + strComparerOperator + @"parseFloat(startDT,10));
                                    }
                                }
                                catch(err)
                                {
                                //
                                }
                            }";
                        }
                        else
                        {
                            strJSCustomValidation = @" function compareTime" + i.ToString() + @"(sender, args) {
                                try
                                {
                                    var startDT = " + strComparisonValue + @";
                                    var endD = document.getElementById('" + strControlToValidateClientID + @"');    

                                 if(startDT.trim()=='' || endD.value.trim()=='' )
                                        { args.IsValid=true; }
                                    else
                                    {  
                          
                                        args.IsValid = (endD.value" + strComparerOperator + @"startDT);
                                    }
                                }
                                catch(err)
                                {
                                //
                                }
                            }";

                        }



                        _cusvValue[i].ClientValidationFunction = "compareTime" + i.ToString();
                        cell[(i * 2) + 1].Controls.Add(_cusvValue[i]);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSCustomValidation" + i.ToString(), strJSCustomValidation, true);

                    }

                }
            }
        }



        //Compare things done

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

                if (bHide)
                {

                    trX[i].ID = "trX" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                    string targetID = "#ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + trX[i].ID;
                    string strOnlyAdminJS = @"$('" + targetID + @"').fadeOut();";

                    if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString())
                        && _rfvValue[i] != null)
                    {
                        strOnlyAdminJS = strOnlyAdminJS + "ValidatorEnable(document.getElementById('" + _rfvValue[i].ClientID.ToString() + "'), false);";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideAdmin" + i.ToString(), strOnlyAdminJS, true);

                }

            }

            //perform unlimited level showwhen



            DataTable dtShowWhen = RecordManager.dbg_ShowWhen_Select(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), null, null);

            if (dtShowWhen.Rows.Count > 0)
            {
                trX[i].ID = "trX" + _dtColumnsDetail.Rows[i]["SystemName"].ToString();
                string strTargetTRID = "#ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + trX[i].ID;


                string strAllDriverValue = "";
                string strAllLogic = "";
                string strAllDriverTrigger = "";

                string strEachPreJoinOperator = "";

                string strValidatorT = "";
                string strValidatorF = "";
                string strBeforeShowHideFunction = "";
                if (bool.Parse(_dtColumnsDetail.Rows[i]["IsMandatory"].ToString()) && _rfvValue[i] != null)
                {
                    strValidatorT = "ValidatorEnable(document.getElementById('" + _rfvValue[i].ClientID.ToString() + "'), true);";
                    strValidatorF = "ValidatorEnable(document.getElementById('" + _rfvValue[i].ClientID.ToString() + "'), false);";
                }


                foreach (DataRow drSW in dtShowWhen.Rows)
                {
                    if (drSW["ColumnID"] != DBNull.Value && drSW["HideColumnID"] != DBNull.Value)
                    {
                        //Lets's go inside eash driver column

                        for (int m = 0; m < _dtColumnsDetail.Rows.Count; m++)
                        {
                            if (drSW["HideColumnID"].ToString() == _dtColumnsDetail.Rows[m]["ColumnID"].ToString() && trX[m] != null)
                            {
                                trX[m].ID = "trX" + _dtColumnsDetail.Rows[m]["SystemName"].ToString();
                                string strEachDriverID = "#ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_";
                                string strControlClientIDPrefix = "ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_";
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
                                               " + strVariableDeclare + @" strEachValue" + m.ToString() + @" =GetOptValue('" + strDriverGroupID.Replace("#", "").Replace("_", "$") + @"');  
                                                ";

                                    strAllLogic = strAllLogic + strEachPreJoinOperator + " (strEachValue" + m.ToString() + @" " + strEachHideOperator + @" '" + strEachHideColumnValue + "') ";

                                    for (int n = 0; n < _radioList[m].Items.Count; n++)
                                    {
                                        strEachDriverID = strDriverIDMain + n.ToString();
                                        strAllDriverTrigger = strAllDriverTrigger + @"                                           
                                           
                                                  $('" + strEachDriverID + @"').change(function (e) {
                                                    ShowHideFunction" + i.ToString() + @"();
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
                                                              ShowHideFunction" + i.ToString() + @"();
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
                                                              ShowHideFunction" + i.ToString() + @"();
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
                                                              ShowHideFunction" + i.ToString() + @"();
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
                                                                          ShowHideFunction" + i.ToString() + @"();
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

                    //$('" + strTargetTRID + @"').stop(true);
                    string strShowHideFunction = @"
                                            $(document).ready(function () {
                                                 try { 
                                                               " + strBeforeShowHideFunction + @"
                                                        function  ShowHideFunction" + i.ToString() + @"()
                                                            {
                                                                                                    " + strAllDriverValue + @"
                                                                if (" + strAllLogic + @") {
                                                                   $('" + strTargetTRID + @"').stop(true,true); $('" + strTargetTRID + @"').fadeIn();" + strValidatorT + @"
                                                                }
                                                                else {
                                                                    $('" + strTargetTRID + @"').fadeOut();" + strValidatorF + @"
                                                                }
                                                            }

                                                                " + strAllDriverTrigger + @"

                                                        }
                                                    catch(err) {
                                                                //email developer
                                                        }
                                                    });
                                        ";


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "strShowHideFunction" + i.ToString(), strShowHideFunction, true);
                }
            }





        }

        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Parent- End ShowWhen";
            theSpeedLog.FunctionLineNumber = 6300;
            SecurityManager.AddSpeedLog(theSpeedLog);
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



    //protected void PutRadioListValue_Image(string strDropdownValues, ref  RadioButtonList rl)
    //{
    //    //string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    OptionImageList theOptionImageList = JSONField.GetTypedObject<OptionImageList>(strDropdownValues);

    //    foreach (OptionImage aOptionImage in theOptionImageList.ImageList)
    //    {
    //        ListItem liTemp = new ListItem("<img src='" + _strFilesLocation + "/UserFiles/AppFiles/" + aOptionImage.UniqueFileName + "' title='" + aOptionImage.Value +  "' />" + "&nbsp;&nbsp;", aOptionImage.Value);
    //        rl.Items.Add(liTemp);
    //    }




    //}

    //protected string GetTextFromValue(string strDropdownValues, string strDBValue)
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
    //                if (strDBValue.ToLower() == strValue.ToLower())
    //                {
    //                    return strText;
    //                }
    //            }
    //        }
    //    }
    //    return strDBValue;

    //}


    //protected void PutList_FromTable(int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn, ref  ListBox lb)
    //{
    //    //it's a new dev so iLinkedParentColumnID must be RecordID
    //    DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID,Common.MaxRowForListBoxTable, null, null);


    //    foreach(DataRow dr in dtParents.Rows)
    //    {
    //        ListItem liTemp = new ListItem(dr[1].ToString(), dr[0].ToString());
    //        lb.Items.Add(liTemp);
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
    //                ListItem liTemp = new ListItem(strText , strValue);
    //                lb.Items.Add(liTemp);
    //            }
    //        }
    //    }


    //}


    //protected void PutCheckBoxList_ForTable(int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn, ref  CheckBoxList lb)
    //{
    //    DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable, null, null);
    //    foreach (DataRow dr in dtParents.Rows)
    //    {
    //        ListItem liTemp = new ListItem(dr[1].ToString(), dr[0].ToString());
    //        lb.Items.Add(liTemp);
    //    }

    //}
    //protected void PutCheckBoxListValues_Text(string strDropdownValues, ref  CheckBoxList lb)
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

    //protected void SetCheckBoxListValues_ForTable(string strDBValues, ref  CheckBoxList lb, int iTableTableID, 
    //    int? iLinkedParentColumnID, string strDisplayColumn)
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


    //protected void PutCheckBoxListValues(string strDropdownValues, ref  CheckBoxList lb)
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

    protected void MakeChildTables()
    {
        DataTable dtCT = Common.DataTableFromText("SELECT * FROM TableChild WHERE ParentTableID=" + _qsTableID + " AND DetailPageType<>'not' ORDER BY DisplayOrder");

        if (dtCT.Rows.Count > 0)
        {
            oneCTList = new Pages_UserControl_RecordList[dtCT.Rows.Count];
            //oneCTDetail = new Pages_UserControl_DetailView[dtCT.Rows.Count];

            oneCTDetail = new Pages_UserControl_DetailEdit[dtCT.Rows.Count];

            //divPanel = new Panel[dtCT.Rows.Count];
            TabPanel[] cTabPanel = new TabPanel[dtCT.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dtCT.Rows)
            {
                //check advanced security
                string strChildTableRight = "2";
                if ((bool)_theUserRole.IsAdvancedSecurity)
                {
                    //DataTable dtUserTable = SecurityManager.ets_UserTable_Select(null,
                    //    int.Parse(dr["ChildTableID"].ToString()), _objUser.UserID, null);

                    DataTable dtUserTable = null;

                    //if (_objUser.RoleGroupID == null)
                    //{
                    dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
                   int.Parse(dr["ChildTableID"].ToString()), _theUserRole.RoleID, null);
                    //}
                    //else
                    //{

                    //    dtUserTable = SecurityManager.dbg_RoleGroupTable_Select((int)_objUser.RoleGroupID, null,
                    //  int.Parse(dr["ChildTableID"].ToString()), null);
                    //}

                    if (dtUserTable.Rows.Count > 0)
                    {
                        strChildTableRight = dtUserTable.Rows[0]["RoleType"].ToString();
                    }

                }
                if (strChildTableRight == Common.UserRoleType.None) //none role
                {
                    continue;
                }

                //check show hide :-)
                if (_qsMode != "add" && _theRecord != null)
                {
                    if (dr["HideColumnID"] != DBNull.Value && 
                            dr["HideColumnValue"] != DBNull.Value && dr["HideOperator"] != DBNull.Value)
                    {
                        Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(dr["HideColumnID"].ToString()));
                        if (theHideColumn != null)
                        {
                            string strThisHideColumnValue = RecordManager.GetRecordValue(ref _theRecord, theHideColumn.SystemName);
                            string strHideColumnValue = "";
                            bool bShowTab = false;
                            if (strThisHideColumnValue != "")
                            {

                                strHideColumnValue = dr["HideColumnValue"].ToString();

                                if (theHideColumn.ColumnType == "listbox")
                                {
                                    string[] strAllHideColumnValue = strHideColumnValue.Split(',');
                                    if (dr["HideOperator"].ToString() == "contains")
                                    {
                                        string[] strAllThisHideCValue = strThisHideColumnValue.Split(',');


                                        foreach (string eachHideValue in strAllHideColumnValue)
                                        {
                                            if (eachHideValue != "")
                                            {
                                                foreach (string eachThisValue in strAllThisHideCValue)
                                                {
                                                    if (eachThisValue != "")
                                                    {
                                                        if (eachHideValue == eachThisValue)
                                                        {
                                                            bShowTab = true;
                                                            continue;

                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }

                                    //or all

                                    if (dr["HideOperator"].ToString() == "equals")
                                    {
                                        if (strHideColumnValue == strThisHideColumnValue)
                                        {
                                            bShowTab = true;
                                        }
                                    }

                                }
                                else
                                {

                                    if (theHideColumn.DropDownType == "value_text" && theHideColumn.DropdownValues != "")
                                    {
                                        strThisHideColumnValue = Common.GetTextFromValue(theHideColumn.DropdownValues, strThisHideColumnValue);
                                    }
                                    strThisHideColumnValue = strThisHideColumnValue.ToLower();
                                    strHideColumnValue = strHideColumnValue.ToLower();
                                    if (dr["HideOperator"].ToString() == "equals")
                                    {
                                        if (strHideColumnValue == strThisHideColumnValue)
                                        {
                                            bShowTab = true;
                                        }
                                    }
                                    if (dr["HideOperator"].ToString() == "contains")
                                    {
                                        if (strThisHideColumnValue.IndexOf(strHideColumnValue) > -1)
                                        {
                                            bShowTab = true;
                                        }
                                    }

                                }

                            }



                            if (bShowTab == false)
                            {
                                continue;
                            }


                        }

                    }
                }


                string strCaption = dr["Description"].ToString();

                if (strCaption == "")
                {
                    Table theTable = RecordManager.ets_Table_Details(int.Parse(dr["ChildTableID"].ToString()));

                    if (theTable != null)
                    {
                        strCaption = theTable.TableName;
                    }

                }
                strCaption = "<strong>" + strCaption + "</strong>";

                if (dr["DetailPageType"].ToString() == "list")
                {


                    string strTextSearch = "";
                    if (_qsRecordID != "")
                    {
                        DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,ColumnID,ColumnType FROM [Column] WHERE LinkedParentColumnID IS NOT NULL AND (ColumnType='dropdown' OR ColumnType='listbox') AND   TableID=" + dr["ChildTableID"].ToString() + " AND TableTableID=" + _qsTableID);


                        foreach (DataRow drCT in dtTemp.Rows)
                        {

                            Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(drCT["ColumnID"].ToString()));
                            Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                            Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(_qsRecordID));
                            string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);
                            strLinkedColumnValue = strLinkedColumnValue.Replace("'", "''");

                            if (drCT["ColumnType"].ToString() == "dropdown")
                            {
                                if (strTextSearch == "")
                                {
                                    strTextSearch = " Record." + drCT["SystemName"].ToString() + "='" + strLinkedColumnValue + "' ";
                                }
                                else
                                {
                                    strTextSearch = strTextSearch + " OR " + " Record." + drCT["SystemName"].ToString() + "='" + strLinkedColumnValue + "' ";
                                }
                            }
                            else
                            {
                                if (strTextSearch == "")
                                {
                                    strTextSearch = " CHARINDEX('," + _theRecord.RecordID.ToString() + ",' ,',' + Record." + drCT["SystemName"].ToString() + " + ',')>0";
                                }
                                else
                                {
                                    strTextSearch = strTextSearch + " OR " + " CHARINDEX('," + _theRecord.RecordID.ToString() + ",' ,',' + Record." + drCT["SystemName"].ToString() + " + ',')>0";
                                }

                            }

                        }





                    }
                    //if (strTextSearch != "")
                    //{
                    _TabIndex = _TabIndex + 1;
                    oneCTList[i] = (Pages_UserControl_RecordList)LoadControl("~/Pages/UserControl/RecordList.ascx");
                    oneCTList[i].TableID = int.Parse(dr["ChildTableID"].ToString());
                    oneCTList[i].ID = "ctList" + i.ToString();
                    oneCTList[i].DetailTabIndex = _TabIndex;
                    oneCTList[i].ShowAddButton = bool.Parse(dr["ShowAddButton"].ToString());
                    oneCTList[i].ShowEditButton = bool.Parse(dr["ShowEditButton"].ToString());
                    oneCTList[i].PageType = "c";

                    strTextSearch = " AND (" + (strTextSearch == "" ? "1=1" : strTextSearch) + ")";
                    oneCTList[i].TextSearchParent = strTextSearch;
                    cTabPanel[i] = new TabPanel();

                    cTabPanel[i].HeaderText = strCaption;

                    //divPanel[i] = new Panel();

                    //divPanel[i].Controls.Add(oneCTList[i]);
                    //cTabPanel[i].Controls.Add(divPanel[i]);
                    cTabPanel[i].Controls.Add(oneCTList[i]);
                    tabDetail.Tabs.Add(cTabPanel[i]);
                    //}


                }
                else
                {



                    string strTextSearch = "";

                    string strSystemName = "";
                    string strLinkedColume = "";
                    if (_qsRecordID != "")
                    {
                        DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,ColumnID,ColumnType FROM [Column] WHERE (ColumnType='dropdown' OR ColumnType='listbox') AND  TableID=" + dr["ChildTableID"].ToString() + " AND TableTableID=" + _qsTableID);
                        foreach (DataRow drCT in dtTemp.Rows)
                        {
                            Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(drCT["ColumnID"].ToString()));
                            Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                            Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(_qsRecordID));
                            string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);

                            if (strLinkedColumnValue != null)
                            {
                                strLinkedColumnValue = strLinkedColumnValue.Replace("'", "''");
                                strSystemName = drCT["SystemName"].ToString();
                                strLinkedColume = strLinkedColumnValue;

                                if (drCT["ColumnType"].ToString() == "dropdown")
                                {
                                    if (strTextSearch == "")
                                    {
                                        strTextSearch = " Record." + drCT["SystemName"].ToString() + "='" + strLinkedColumnValue + "' ";
                                    }
                                    else
                                    {
                                        strTextSearch = strTextSearch + " OR " + " Record." + drCT["SystemName"].ToString() + "='" + strLinkedColumnValue + "' ";
                                    }
                                }
                                else
                                {
                                    if (strTextSearch == "")
                                    {
                                        strTextSearch = " CHARINDEX('," + _theRecord.RecordID.ToString() + ",' ,',' + Record." + drCT["SystemName"].ToString() + " + ',')>0";
                                    }
                                    else
                                    {
                                        strTextSearch = strTextSearch + " OR " + " CHARINDEX('," + _theRecord.RecordID.ToString() + ",' ,',' + Record." + drCT["SystemName"].ToString() + " + ',')>0";
                                    }

                                }
                            }

                        }
                    }
                    //if (strTextSearch != "")
                    //{
                    _TabIndex = _TabIndex + 1;

                    //oneCTDetail[i] = (Pages_UserControl_DetailView)LoadControl("~/Pages/UserControl/DetailView.ascx");

                    oneCTDetail[i] = (Pages_UserControl_DetailEdit)LoadControl("~/Pages/UserControl/DetailEdit.ascx");

                    oneCTDetail[i].ContentPage = "record";
                    oneCTDetail[i].TableID = int.Parse(dr["ChildTableID"].ToString());
                    oneCTDetail[i].ID = "ctDetail" + i.ToString();
                    oneCTDetail[i].ShowAddButton = bool.Parse(dr["ShowAddButton"].ToString());
                    oneCTDetail[i].ShowEditButton = bool.Parse(dr["ShowEditButton"].ToString());
                    oneCTDetail[i].DetailTabIndex = _TabIndex;
                    oneCTDetail[i].Mode = "view";
                    // strTextSearch = " AND (" + strTextSearch + ")";
                    if (dr["DetailPageType"].ToString() == "alone")
                    {
                        oneCTDetail[i].OnlyOneRecord = true;
                        oneCTDetail[i].SystemName = strSystemName;
                        oneCTDetail[i].LinkedColumnValue = strLinkedColume;

                    }
                    else
                    {
                        oneCTDetail[i].OnlyOneRecord = false;
                    }

                    oneCTDetail[i].TextSearch = strTextSearch;

                    cTabPanel[i] = new TabPanel();

                    cTabPanel[i].HeaderText = strCaption;
                    //divPanel[i] = new Panel();

                    //divPanel[i].Controls.Add(oneCTDetail[i]);
                    //cTabPanel[i].Controls.Add(divPanel[i]);

                    cTabPanel[i].Controls.Add(oneCTDetail[i]);
                    tabDetail.Tabs.Add(cTabPanel[i]);

                    //}



                }

                i = i + 1;
            }


        }

        //Message list
        if (_qsMode != "add")
        {
            if ((_theTable.ShowSentEmails != null && (bool)_theTable.ShowSentEmails)
            || (_theTable.ShowReceivedEmails != null && (bool)_theTable.ShowReceivedEmails))
            {
                oneMLList = new Pages_UserControl_MessageList();
                oneMLList = (Pages_UserControl_MessageList)LoadControl("~/Pages/UserControl/MessageList.ascx");
                oneMLList.ID = "mlList";
                oneMLList.RecordID = _theRecord.RecordID;
                TabPanel cTabPanel = new TabPanel();
                cTabPanel.HeaderText = "<strong>Messages</strong>";
                //cTabPanel.
                cTabPanel.Controls.Add(oneMLList);
                tabDetail.Tabs.Add(cTabPanel);
            }
        }



    }



    protected void grdProgressHisotry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            _iRowCount = _iRowCount + 1;
            string strWWWRoot = "http://" + Request.Url.Authority + Request.ApplicationPath;
            string strRootURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/";

            string strEmpty = strRootURL + "empty.png";
            string strComplete = strRootURL + "complete.png";
            string strIncomplete = strRootURL + "incomplete.png";


            DataTable dtFormSetGroup = Common.DataTableFromText("SELECT * FROM FormSetGroup WHERE ParentTableID=" + _qsTableID + " ORDER BY ColumnPosition");

            if (dtFormSetGroup.Rows.Count > 0)
            {

                int i = 1;
                foreach (DataRow drG in dtFormSetGroup.Rows)
                {

                    string strImageURL = strEmpty;
                    string strWizardURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetWizard.aspx?SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString()
                            + "&ParentTableID=" + Cryptography.Encrypt(_qsTableID)
                            + "&ParentRecordID=" + Cryptography.Encrypt(_qsRecordID);


                    FormSetGroup theFormSetGroup = FormSetManager.dbg_FormSetGroup_Detail(int.Parse(drG["FormSetGroupID"].ToString()));

                    if (DataBinder.Eval(e.Row.DataItem, drG["FormSetGroupName"].ToString()) != DBNull.Value)
                    {
                        if (DataBinder.Eval(e.Row.DataItem, drG["FormSetGroupName"].ToString()) != "")
                        {
                            int iFormSetID = int.Parse(DataBinder.Eval(e.Row.DataItem, drG["FormSetGroupName"].ToString()).ToString());
                            HiddenField hf = e.Row.FindControl("hf" + i.ToString()) as HiddenField;
                            HiddenField hfC = e.Row.FindControl("hfC" + i.ToString()) as HiddenField;

                            hf.Value = iFormSetID.ToString();
                            FormSet aFromSet = FormSetManager.dbg_FormSet_Detail(iFormSetID);

                            if (aFromSet != null)
                            {
                                strWizardURL = strWizardURL + "&FormSetID=" + Cryptography.Encrypt(aFromSet.FormSetID.ToString());
                                DataTable dtFromSetProgress = Common.DataTableFromText("SELECT * FROM FormSetProgress WHERE RecordID=" + _qsRecordID + " AND FormSetID=" + aFromSet.FormSetID.ToString());
                                DataTable dtFromSetForm = Common.DataTableFromText("SELECT * FROM FormSetForm WHERE FormSetID=" + aFromSet.FormSetID.ToString() + "  ORDER BY DisplayOrder");
                                int iCom = 0;
                                if (dtFromSetProgress.Rows.Count > 0)
                                {

                                    if (dtFromSetProgress.Rows.Count == dtFromSetForm.Rows.Count)
                                    {
                                        foreach (DataRow drFSP in dtFromSetProgress.Rows)
                                        {
                                            if (drFSP["Completed"] == DBNull.Value)
                                            {
                                                strImageURL = strIncomplete;
                                                break;
                                            }
                                            else
                                            {
                                                if ((bool)drFSP["Completed"] == false)
                                                {
                                                    strImageURL = strIncomplete;
                                                    break;
                                                }
                                                else
                                                {
                                                    strImageURL = strComplete;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strImageURL = strIncomplete;


                                    }

                                }
                                else
                                {
                                    strImageURL = strEmpty;
                                }

                                if (strImageURL == strEmpty)
                                {
                                    strWizardURL = strWizardURL + "&ps=0";
                                }
                                if (strImageURL == strIncomplete)
                                {
                                    //strWizardURL = strWizardURL + "&ps=1";
                                    //iCom = 1;

                                    DataTable dtProgreesFormIncomplete = Common.DataTableFromText(@"SELECT TOP 1 FormSetProgressID FROM FormSetProgress INNER JOIN FormSetForm
                                        ON FormSetForm.FormSetFormID=FormSetProgress.FormSetFormID
                                         WHERE RecordID=" + _qsRecordID + @" AND FormSetProgress.FormSetID=" + aFromSet.FormSetID.ToString() + @" AND 
                                    (FormSetProgress.Completed is null OR FormSetProgress.Completed=0)  ORDER BY FormSetForm.DisplayOrder");

                                    if (dtProgreesFormIncomplete.Rows.Count > 0)
                                    {
                                        FormSetProgress theFormSetProgress = FormSetManager.dbg_FormSetProgress_Detail(int.Parse((dtProgreesFormIncomplete.Rows[0][0].ToString())));

                                        if (theFormSetProgress != null)
                                        {
                                            FormSetForm theIncompleteFormSetForm = FormSetManager.dbg_FormSetForm_Detail((int)theFormSetProgress.FormSetFormID);
                                            if (theIncompleteFormSetForm != null)
                                            {
                                                if (theIncompleteFormSetForm.IncompleteImage != "")
                                                {
                                                    strImageURL = strWWWRoot + theIncompleteFormSetForm.IncompleteImage;
                                                }
                                            }

                                        }
                                    }



                                    strWizardURL = strWizardURL + "&ps=1";
                                    iCom = 1;

                                }
                                if (strImageURL == strComplete)
                                {
                                    strWizardURL = strWizardURL + "&ps=2";
                                    iCom = 2;
                                }

                                hfC.Value = iCom.ToString();
                                if (_iRowCount == 1 && i == 1)
                                {
                                    strWizardURL = strWizardURL + "&showparent=yes";
                                }

                                string strImagees = "<a href='" + strWizardURL + "'> <img src='" + strImageURL + "'/></a>";

                                Label lbl = e.Row.FindControl("lbl" + i.ToString()) as Label;
                                lbl.Text = strImagees + "&nbsp" + aFromSet.FormSetName;

                                if ((bool)theFormSetGroup.Sequential)
                                {
                                    if (_iRowCount > 1)
                                    {
                                        GridViewRow gvPreRow = grdProgressHisotry.Rows[e.Row.RowIndex - 1];

                                        HiddenField prehfC = gvPreRow.FindControl("hfC" + i.ToString()) as HiddenField;
                                        if (prehfC != null)
                                        {
                                            if (prehfC.Value != "2")
                                            {
                                                lbl.Text = "<img src='" + strImageURL + "'/>" + "&nbsp" + aFromSet.FormSetName;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }

                    i = i + 1;
                }

            }
        }

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    HyperLink hlProgressColumnID = e.Row.FindControl("hlProgressColumnID") as HyperLink;
        //    CheckBox chkProgressStatus = e.Row.FindControl("chkProgressStatus") as CheckBox;
        //    Label lblFormSetName = e.Row.FindControl("lblFormSetName") as Label;

        //    if (hlProgressColumnID != null && chkProgressStatus != null && lblFormSetName!=null)
        //    {
        //        hlProgressColumnID.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetWizard.aspx?SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() 
        //            + "&ProgressColumnID=" + Cryptography.Encrypt( DataBinder.Eval(e.Row.DataItem, "ProgressColumnID").ToString())
        //            + "&ParentTableID=" + Cryptography.Encrypt(_qsTableID)
        //            + "&ParentRecordID=" + Cryptography.Encrypt(_qsRecordID);

        //        if (e.Row.RowIndex == 0)
        //        {
        //            hlProgressColumnID.NavigateUrl = hlProgressColumnID.NavigateUrl + "&showparent=yes";
        //        }

        //        Column pcColumn = RecordManager.ets_Column_Details(int.Parse(DataBinder.Eval(e.Row.DataItem, "ProgressColumnID").ToString()));
        //        lblFormSetName.Text = pcColumn.DisplayName;
        //        string strValue = Common.GetValueFromSQL("SELECT " + pcColumn.SystemName+ " FROM Record WHERE RecordID=" + _qsRecordID);

        //        if (strValue == "")
        //        {
        //            hlProgressColumnID.Text = "Start";
        //        }
        //        else if (strValue == "0")
        //        {
        //            hlProgressColumnID.Text = "Start";
        //        }
        //        else if (strValue == "1")
        //        {
        //            hlProgressColumnID.Text = "Resume";
        //        }
        //        else if (strValue == "2")
        //        {
        //            hlProgressColumnID.Text = "View";
        //            chkProgressStatus.Checked = true;
        //        }

        //        if (_strPreProgress != "-1")
        //        {
        //            if (_strPreProgress != "2")
        //            {
        //                hlProgressColumnID.Visible = false;
        //            }
        //        }
        //        _strPreProgress = strValue;

        //    }

        //}
    }

    protected void LB_SPRun_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton theButton = (LinkButton)sender;
            if (theButton != null)
            {
                if (theButton.CommandArgument != "")
                {
                    Column theColumn = RecordManager.ets_Column_Details(int.Parse(theButton.CommandArgument));
                    ColumnButtonInfo theButtonInfo = JSONField.GetTypedObject<ColumnButtonInfo>(theColumn.ButtonInfo);

                    if (_iRecordID > 0)
                    {
                        if (PerformSave())
                        {
                            RecordManager.RunSPForRecord(theButtonInfo.SPToRun,
                                _iRecordID, (int)_objUser.UserID);
                            if ( string.IsNullOrEmpty( theButtonInfo.OpenLink))
                            {
                                 Response.Redirect(Request.RawUrl, false);
                            }
                            else
                            {
                                Response.Redirect(theButtonInfo.OpenLink, false);
                            }
                           
                        }

                    }
                    else
                    {
                        if (PerformSave())
                        {
                            if (_iRecordID > 0)
                            {
                                RecordManager.RunSPForRecord(theButtonInfo.SPToRun,
                                 _iRecordID, (int)_objUser.UserID);
                                if (string.IsNullOrEmpty(theButtonInfo.OpenLink))
                                {
                                    Response.Redirect(GetEditURLAfterAdd(), false);
                                }
                                else
                                {
                                    Response.Redirect(theButtonInfo.OpenLink, false);
                                }
                                
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

    protected void IB_CalRef_Click(object sender, ImageClickEventArgs e)
    {
        //_bRedirect = false;
        //_theTable.ShowEditAfterAdd = false;

        _bCancelSave = true;
        PerformSave();
        ResetTabs();
        //lnkSaveClose_Click(null,null);


        //for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
        //{
        //    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation")
        //    {
        //        if (_dtColumnsDetail.Rows[i]["Calculation"] != DBNull.Value)
        //        {
        //             bool bDateCal = false;
        //             if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
        //                 && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "d")
        //             {
        //                 bDateCal = true;
        //                 string strCalculation = _dtColumnsDetail.Rows[i]["Calculation"].ToString();

        //                 _txtValue[i].Text = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, _iRecordID,_iParentRecordID,
        //                     _dtColumnsDetail.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString());
        //                 //_txtValue[i].Text = GetDateCalculationResult(_dtRecordTypleColumlns, strCalculation, _iRecordID, i,_iParentRecordID);




        //             }
        //             else
        //             {
        //                 string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtColumnsDetail.Rows[i]["Calculation"].ToString());
        //                 //_txtValue[i].Text = Common.GetValueFromSQL("SELECT " + strFormula + " FROM Record WHERE RecordID=" + _iRecordID.ToString());
        //                 _txtValue[i].Text = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, _iRecordID, i, _iParentRecordID);
        //             }

        //             if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value && bDateCal == false)
        //            {

        //                if (_txtValue[i].Text.ToString() != "")
        //                {
        //                    try
        //                    {
        //                        if (_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
        //                        {

        //                            _txtValue[i].Text = Math.Round(double.Parse(Common.IgnoreSymbols(_txtValue[i].Text)), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
        //                        }
        //                        else
        //                        {
        //                            _txtValue[i].Text = Math.Round(double.Parse(_txtValue[i].Text), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
        //                        }

        //                    }
        //                    catch
        //                    {
        //                        //
        //                    }

        //                }


        //            }

        //             if (_txtValue[i].Text != "" && bDateCal == false)
        //            {
        //                try
        //                {
        //                    _txtValue[i].Text = Common.IgnoreSymbols(_txtValue[i].Text);

        //                    _txtValue[i].Text = double.Parse(_txtValue[i].Text).ToString("C").Substring(1);
        //                    if (_revValue[i] != null)
        //                        _revValue[i].Enabled = false;

        //                    if (_ftbExt[i] != null)
        //                        _ftbExt[i].Enabled = false;
        //                }
        //                catch
        //                {

        //                }

        //            }


        //        }
        //    }
        //}





    }

    protected void ResetTabs()
    {
        if (_bTableTabYes && _strActionMode!="add")
        {



            if (Request.QueryString["TableTabID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideMainDivsOnlyOne", "ShowHideMainDivs(" + pnlDetailTab.ClientID + ");", true);
            }
            else
            {
                for (int t = 0; t < _dtDBTableTab.Rows.Count; t++)
                {

                    string strPanelID = "";
                    string strLnk = "";
                    string strTableTabLink = "ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_lnkDetialTabD" + Request.QueryString["TableTabID"].ToString();
                    if (hfCurrentSelectedTabLink.Value != "")
                    {
                        strTableTabLink = hfCurrentSelectedTabLink.Value;
                    }

                    if (_dtDBTableTab.Rows[t]["TableTabID"].ToString() == Request.QueryString["TableTabID"].ToString())
                    {
                        if (t == 0)
                        {
                            strPanelID = pnlDetailTab.ClientID;
                            strLnk = "ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_lnkDetialTab";
                        }
                        else
                        {
                            strPanelID = _pnlDetailTabD[t].ClientID;
                            strLnk = "ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_lnkDetialTabD" + _dtDBTableTab.Rows[t]["TableTabID"].ToString();
                        }
                    }

                    if (strPanelID != "" && strLnk != "")
                    {
                        if (strTableTabLink == "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideMainDivsTableTab", "ShowHideMainDivs(" + strPanelID + "," + strLnk + ");", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideMainDivsTableTab", "ShowHideMainDivs(" + strPanelID + "," + strTableTabLink + ");", true);

                        }
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideMainDivsTableTab", "alert('hhh');", true);
                    }

                }
            }
        }
    }


    //public  string GetDateCalculationResult(DataTable _dtRecordTypleColumlns, string strCalculation, int _iRecordID, int i)
    //{
    //    string strResult = "";
    //    string strResultFormat="day";

    //    if (_dtColumnsDetail.Rows[i]["DateCalculationType"] != DBNull.Value)
    //     strResultFormat = _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString().ToLower();



    //    for (int j = 0; j < _dtColumnsDetail.Rows.Count; j++)
    //    {
    //        if (strCalculation.IndexOf(_dtColumnsDetail.Rows[j]["SystemName"].ToString()) > -1)
    //        {
    //            string strValue = Common.GetValueFromSQL("SELECT " + _dtColumnsDetail.Rows[j]["SystemName"].ToString() + " FROM Record WHERE RecordID=" + _iRecordID.ToString());
    //            if (strValue != "")
    //            {
    //                //lets convert it to datetime
    //                DateTime dtTempDateTime = DateTime.Today;
    //                try
    //                {
    //                    dtTempDateTime = DateTime.Parse(strValue);

    //                }
    //                catch
    //                {

    //                }

    //                strValue = TheDatabaseS.ToJulianDate(dtTempDateTime).ToString();
    //                strCalculation = strCalculation.Replace("[" + _dtColumnsDetail.Rows[j]["SystemName"].ToString() + "]", strValue);
    //            }


    //        }

    //    }

    //    if (strCalculation.IndexOf("[") > -1)
    //    {
    //        //we have number here
    //        string regex = Common.NumberRegExDC; //@"\[(.*?)\]";  // Common.NumberRegExDC; //@"^.*?\([^\d]*(\d+)[^\d]*\).*$"
    //        string text = strCalculation;

    //        foreach (Match match in Regex.Matches(text, regex))
    //        {
    //            string strEachNumber = match.Groups[1].Value;
    //            string strEachJulianNumber = strEachNumber;

    //                switch (strResultFormat)
    //                {
    //                    case "datetime":
    //                        strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute

    //                        break;
    //                    case "date":
    //                        strEachJulianNumber = strEachNumber; //day
    //                        break;
    //                    case "time":
    //                          strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
    //                        break;
    //                    case "minute":
    //                          strEachJulianNumber = (double.Parse(strEachNumber) / (24 * 60)).ToString(); //minute
    //                        break;
    //                    case "hour":
    //                          strEachJulianNumber = (double.Parse(strEachNumber) / (24 )).ToString(); //hour
    //                        break;
    //                    case "day":
    //                         strEachJulianNumber = strEachNumber; //day
    //                        break;

    //                    default:
    //                          strEachJulianNumber = strEachNumber; //day
    //                        break;
    //                }

    //                strCalculation = strCalculation.Replace("[" + strEachNumber + "]", strEachJulianNumber);
    //        }




    //    }

    //    strCalculation = strCalculation.Replace("[", "");
    //    strCalculation = strCalculation.Replace("]", "");

    //    string strJulianValue = Common.GetValueFromSQL("SELECT " + strCalculation);


    //    //implement Result Format

    //    if (_dtColumnsDetail.Rows[i]["DateCalculationType"] != DBNull.Value)
    //    {

    //        strResult = strJulianValue;
    //        switch (strResultFormat)
    //        {
    //            case "datetime":

    //                DateTime dtResult = DateTime.FromOADate(double.Parse(strResult));
    //                //if (dtResult > DateTime.Today.AddYears(1000))
    //                //    dtResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                strResult = dtResult.ToString();
    //                break;
    //            case "date":
    //                DateTime dResult = DateTime.FromOADate(double.Parse(strResult));
    //                //if (dResult > DateTime.Today.AddYears(1000))
    //                //    dResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                strResult = dResult.ToShortDateString();
    //                break;
    //            case "time":
    //                DateTime tResult = DateTime.FromOADate(double.Parse(strResult));
    //                //if (tResult > DateTime.Today.AddYears(1000))
    //                //    tResult = DateTime.FromOADate(double.Parse(strResult) - 2415018.5);

    //                strResult = tResult.ToLongTimeString();
    //                break;
    //            case "minute":
    //                double dMinutes = double.Parse(strJulianValue) * 24 * 60;
    //                strResult = ((int)Math.Round(dMinutes)).ToString();
    //                break;
    //            case "hour":
    //                double dHours = double.Parse(strJulianValue) * 24;
    //                strResult = ((int)Math.Round(dHours)).ToString();
    //                break;
    //            case "day":
    //                double dDays = double.Parse(strJulianValue);
    //                strResult = ((int)Math.Round(dDays)).ToString();


    //                break;

    //            default:
    //                break;
    //        }

    //    }

    //    return strResult;

    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        //int iTemp=0;
        //_objUser = (User)Session["User"];


        if (_iSessionAccountID == -1)
        {
            if (Session["User"] == null)
            {
                return;
            }
        }


        //if (!IsPostBack)
        //{
        //    if ((int)_theTable.AccountID != (int)_theUserRole.AccountID && !Common.HaveAccess(_strRecordRightID, "1"))
        //    {
        //        Response.Redirect("~/Empty.aspx", true);
        //        return;
        //    }
        //}


        _strURL = "http://" + Request.Url.Authority + Request.RawUrl;


        _strSaveAndStay = SystemData.SystemOption_ValueByKey_Account("SaveAndStay", _iSessionAccountID, int.Parse(_qsTableID));
        if (_strSaveAndStay == "")
            _strSaveAndStay = SystemData.SystemOption_ValueByKey_Account("SaveAndStay", null, int.Parse(_qsTableID));

        if (_strSaveAndStay.ToLower() == "yes")
        {
            hlBack.Visible = false;
            imgSave.ImageUrl = "~/App_Themes/Default/images/Back.png";
            imgSave.ToolTip = "Save and Return";
            divSaveAndStay.Visible = true;
        }

        if (_theTable.SaveAndAdd != null && (bool)_theTable.SaveAndAdd)
        {

            lnkSaveAndAdd.Visible = true;
            //_theTable.ShowEditAfterAdd = true;
            //_theTable.SaveAndAdd = null;
            if (_qsMode.ToLower() == "edit")
            {
                //string strAddURL = _strURL;
                //divSaveAndAdd.Visible = true;

                //strAddURL = strAddURL.Replace("mode=" + Cryptography.Encrypt("edit"), "mode=" + Cryptography.Encrypt("add"));
                //strAddURL = strAddURL.Replace("&Recordid=" + Cryptography.Encrypt(_qsRecordID), "");
                //strAddURL = strAddURL.Replace("&stack=n", "");
                //hlSaveAndAdd.NavigateUrl = strAddURL;
            }
        }

        if (!IsPostBack)
        {
            CreateValidWarningDataTable();
            if (_qsMode.ToLower() == "add")
            {
                divPrint.Visible = false;
            }
            else
            {
                if (_theTable != null)
                {

                    string strShowPrintOnRecordDetail = SystemData.SystemOption_ValueByKey_Account("ShowPrintOnRecordDetail", null, (int)_theTable.TableID);

                    if (strShowPrintOnRecordDetail != "")
                    {
                        if (strShowPrintOnRecordDetail.ToLower() == "yes")
                        {
                            divPrint.Visible = true;
                            hlPrint.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetPrint.aspx?NoFormSet=yes&ParentRecordID=" + Cryptography.Encrypt(_qsRecordID) + "&ParentTableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();
                        }
                    }
                }
            }


            if (_bNeedFullPostback)
            {
                try
                {
                    //may be i need to check FLASH active or not

                    if (Session["IsFlashSupported"] != null && Session["IsFlashSupported"].ToString() == "no")
                    {
                        //we have flash


                        PostBackTrigger trigger = new PostBackTrigger();
                        trigger.ControlID = lnkSaveClose.ID;
                        upDetailDynamic.Triggers.Add(trigger);

                        PostBackTrigger trigger2 = new PostBackTrigger();
                        trigger2.ControlID = lnkSaveAndAdd.ID;
                        upDetailDynamic.Triggers.Add(trigger2);

                        PostBackTrigger trigger3 = new PostBackTrigger();
                        trigger3.ControlID = lnkSaveAndStay.ID;
                        upDetailDynamic.Triggers.Add(trigger3);


                        if (Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
                        {
                            PostBackTrigger trigger4 = new PostBackTrigger();
                            trigger4.ControlID = tabDetail.ID;
                            upDetailDynamic.Triggers.Add(trigger4);

                        }
                    }



                }
                catch
                {
                    //
                }




            }

            //navigate records

            if (_qsMode.ToLower() != "add" && _theTable.NavigationArrows != null)
            {

                if (Request.QueryString["SearchCriteriaID"] != null && (bool)_theTable.NavigationArrows)
                {
                    SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteriaID"].ToString())));
                    if (theSearchCriteria != null)
                    {
                        try
                        {
                            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                            xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));
                            string strddlEnteredBy = xmlDoc.FirstChild["ddlEnteredBy"].InnerText;
                            string strchkIsActive = xmlDoc.FirstChild["chkIsActive"].InnerText;
                            string strchkShowOnlyWarning = xmlDoc.FirstChild["chkShowOnlyWarning"].InnerText;
                            string sOrder = xmlDoc.FirstChild["sOrder"].InnerText;
                            string strOrderDirection = xmlDoc.FirstChild["strOrderDirection"].InnerText;
                            string strNumericSearch = "";// xmlDoc.FirstChild["_strNumericSearch"].InnerText;
                            string TextSearch = xmlDoc.FirstChild["TextSearch"].InnerText;
                            string TextSearchParent = xmlDoc.FirstChild["TextSearchParent"].InnerText;
                            string strDateFrom = xmlDoc.FirstChild["txtDateFrom"].InnerText;
                            string strDateTo = xmlDoc.FirstChild["txtDateTo"].InnerText;
                            string sParentColumnSortSQL = xmlDoc.FirstChild["sParentColumnSortSQL"].InnerText;
                            string strViewName = xmlDoc.FirstChild["_strViewName"].InnerText;
                            string strViewID = xmlDoc.FirstChild["strViewID"].InnerText;

                            if (strViewID != "")
                            {
                                _iViewID = int.Parse(strViewID);
                                _strListType = "view";
                            }

                            int iTN = 0;
                            int iTotalDynamicColumns = 0;


                            DateTime? dtDateFrom = null;
                            DateTime? dtDateTo = null;


                            if (strDateFrom != "")
                            {
                                DateTime dtTemp;
                                if (DateTime.TryParseExact(strDateFrom.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                {
                                    dtDateFrom = dtTemp;
                                }
                            }
                            if (strDateTo != "")
                            {
                                DateTime dtTemp;
                                if (DateTime.TryParseExact(strDateTo.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                {
                                    dtDateTo = dtTemp;
                                    dtDateTo = dtDateTo.Value.AddHours(23).AddMinutes(59);
                                }
                            }

                            string strReturnSQL = "";
                            DataTable dtAllRecords = RecordManager.ets_Record_List(int.Parse(_qsTableID),
                            strddlEnteredBy == "-1" ? null : (int?)int.Parse(strddlEnteredBy), !bool.Parse(strchkIsActive),
                            bool.Parse(strchkShowOnlyWarning) == false ? null : (bool?)true, null, null, sOrder, strOrderDirection,
                            null, null, ref iTN, ref iTotalDynamicColumns, _strListType, strNumericSearch, TextSearch + TextSearchParent, dtDateFrom, dtDateTo,
                            sParentColumnSortSQL, "", strViewName, _iViewID, ref strReturnSQL, ref strReturnSQL);

                            int iRecordNo = 0;
                            int iPreRecordID = -1;
                            int iNextRecordID = -1;

                            for (int i = 0; i < dtAllRecords.Rows.Count; i++)
                            {
                                if (dtAllRecords.Rows[i]["DBGSystemRecordID"].ToString() == _qsRecordID)
                                {
                                    iRecordNo = i;
                                    iNextRecordID = int.Parse(dtAllRecords.Rows[i]["DBGSystemRecordID"].ToString());
                                    iPreRecordID = int.Parse(dtAllRecords.Rows[i]["DBGSystemRecordID"].ToString());

                                    if (i < (dtAllRecords.Rows.Count - 1))
                                    {
                                        iNextRecordID = int.Parse(dtAllRecords.Rows[i + 1]["DBGSystemRecordID"].ToString());

                                    }

                                    if (i == (dtAllRecords.Rows.Count - 1))
                                    {
                                        iNextRecordID = int.Parse(dtAllRecords.Rows[0]["DBGSystemRecordID"].ToString());

                                    }

                                    if (i > 0)
                                    {
                                        iPreRecordID = int.Parse(dtAllRecords.Rows[i - 1]["DBGSystemRecordID"].ToString());
                                    }

                                    if (i == 0)
                                    {
                                        iPreRecordID = int.Parse(dtAllRecords.Rows[dtAllRecords.Rows.Count - 1]["DBGSystemRecordID"].ToString());
                                    }

                                    break;
                                }

                                iRecordNo = iRecordNo + 1;
                            }

                            //make the URL



                            string strRawURL = Request.RawUrl;

                            strRawURL = strRawURL.Replace("&stackzero=y", "");
                            strRawURL = strRawURL + "&stackzero=y";
                            string strPreRawURL = strRawURL.Replace("&Recordid=" + Cryptography.Encrypt(_qsRecordID.ToString()), "&Recordid=" + Cryptography.Encrypt(iPreRecordID.ToString()));
                            string strNextRawURL = strRawURL.Replace("&Recordid=" + Cryptography.Encrypt(_qsRecordID.ToString()), "&Recordid=" + Cryptography.Encrypt(iNextRecordID.ToString()));

                            if (iPreRecordID != -1)
                                hlNavigatePrev.NavigateUrl = strPreRawURL;

                            if (iNextRecordID != -1)
                                hlNavigateNext.NavigateUrl = strNextRawURL;

                        }
                        catch
                        {
                            //
                        }



                    }
                    else
                    {
                        tblNavigateRecords.Visible = false;

                    }

                }
                else
                {
                    tblNavigateRecords.Visible = false;
                }

            }




            if (_bTableTabYes)
            {
                if (Request.QueryString["TableTabID"] == null)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideMainDivsOnlyOne", "ShowHideMainDivs(" + pnlDetailTab.ClientID + ");", true);
            }

            if (Request.QueryString["quickdone"] != null && Request.QueryString["controlvalue"] != null)
            {
                try
                {
                    SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["quickdone"].ToString())));
                    if (theSearchCriteria != null)
                    {
                        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                        xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));
                        string strControl = xmlDoc.FirstChild["control"].InnerText;
                        string strControlValue = Cryptography.Decrypt(Request.QueryString["controlvalue"].ToString());

                        string strDisplayColumn = xmlDoc.FirstChild["DisplayColumn"].InnerText;
                        string strTableTableID = xmlDoc.FirstChild["TableTableID"].InnerText;
                        string strLinkedParentColumnID = xmlDoc.FirstChild["LinkedParentColumnID"].InnerText;
                        string strDropDownType = xmlDoc.FirstChild["DropDownType"].InnerText;




                        if (strDropDownType == "table")
                        {
                            string str_hfValue = xmlDoc.FirstChild["_hfValue"].InnerText;
                            TextBox txtP = (TextBox)pnlDetail.FindControl(strControl);
                            HiddenField _hfValue = (HiddenField)pnlDetail.FindControl(str_hfValue);


                            try
                            {
                                //int iTableRecordID = int.Parse(_dtRecordedetail.Rows[0][i].ToString());

                                Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(strLinkedParentColumnID));

                                Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strControlValue));
                                string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);


                                //Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(iTableRecordID);
                                //string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);




                                _hfValue.Value = strLinkedColumnValue;
                                DataTable dtTableTableSC = Common.DataTableFromText(@"SELECT SystemName,DisplayName 
                                FROM [Column] WHERE   TableID ="
                           + strTableTableID);

                                //string strDisplayColumn = _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString();

                                foreach (DataRow dr in dtTableTableSC.Rows)
                                {
                                    strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                                }

                                string sstrDisplayColumnOrg = strDisplayColumn;
                                string strFilterSQL = "";
                                if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                {
                                    strFilterSQL = strLinkedColumnValue;
                                }
                                else
                                {
                                    strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                                }



                                //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + _dtRecordedetail.Rows[0][i].ToString());

                                DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);

                                if (dtTheRecord.Rows.Count > 0)
                                {

                                    foreach (DataColumn dc in dtTheRecord.Columns)
                                    {

                                        //strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());

                                        Column theColumn = RecordManager.ets_Column_Details_By_Sys((int)theLinkedColumn.TableID, dc.ColumnName);
                                        if (theColumn != null)
                                        {
                                            if (theColumn.ColumnType == "date")
                                            {
                                                string strDatePartOnly = dtTheRecord.Rows[0][dc.ColumnName].ToString();

                                                if (strDatePartOnly.Length > 9)
                                                {
                                                    strDatePartOnly = strDatePartOnly.Substring(0, 10);
                                                }

                                                strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", strDatePartOnly);
                                            }
                                            else
                                            {
                                                strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                            }
                                        }

                                    }
                                }
                                if (sstrDisplayColumnOrg != strDisplayColumn)
                                    txtP.Text = strDisplayColumn;



                            }
                            catch
                            {
                                //
                            }



                        }
                        else
                        {

                            Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(strLinkedParentColumnID));

                            Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(strControlValue));
                            string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);


                            DropDownList ddlP = (DropDownList)pnlDetail.FindControl(strControl);
                            ddlP.SelectedValue = strLinkedColumnValue;
                        }
                    }
                }
                catch
                {

                }

            }

            if (Request.QueryString["FromAdd"] != null)
            {
                lblMsg.Text = "Successfully saved!";
                lblMsg.ForeColor = System.Drawing.Color.Green;
            }

            if (Request.QueryString["mode"] != null)
            {
                if (_qsMode != "add")
                {
                    tblChangeHistory.Visible = true;
                    //check if it has DocTemplate records
                    if (_qsTableID != "")
                    {
                        DataTable dtTemp = Common.DataTableFromText(@"
                            SELECT DocTemplateID,SPName,FileName FROM DocTemplate INNER JOIN DataRetriever
                            ON DocTemplate.DataRetrieverID=DataRetriever.DataRetrieverID
                            WHERE TableID=" + _qsTableID);

                        if (dtTemp.Rows.Count > 0)
                        {
                            divWordExport.Visible = true;
                            ddlDataRetriever.DataSource = dtTemp;
                            ddlDataRetriever.DataBind();
                        }
                        else
                        {
                            divWordExport.Visible = false;
                        }


                    }

                }

            }
        }
        else
        {

        }

        // checking action mode
        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            //_qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;

                if (Request.QueryString["RecordID"] != null)
                {
                    //progress history
                    //                    if (!IsPostBack)
                    //                    {
                    //                        try
                    //                        {

                    //                            DataTable dtFormSetGroup = Common.DataTableFromText("SELECT * FROM FormSetGroup WHERE ParentTableID=" + _qsTableID + " ORDER BY ColumnPosition");

                    //                            DataTable dtFormSet = Common.DataTableFromText(@"SELECT FormSetID FROM FormSet FS
                    //                                INNER JOIN FormSetGroup FSG ON FS.FormSetGroupID=FSG.FormSetGroupID
                    //                                WHERE  ParentTableID=" + _qsTableID);

                    //                            if (dtFormSetGroup.Rows.Count > 0 && dtFormSet.Rows.Count>0)
                    //                            {
                    //                                //wow this need progress history
                    //                                divProgressHisotry.Visible = true;
                    //                                grdProgressHisotry.ShowHeaderWhenEmpty = true;
                    //                                DataTable dt = new DataTable();
                    //                                int i=1;
                    //                                foreach (DataRow drG in dtFormSetGroup.Rows)
                    //                                {
                    //                                    TemplateField temp1 = new TemplateField();  //Create instance of Template field
                    //                                    temp1.HeaderText = drG["FormSetGroupName"].ToString(); //Give the header text
                    //                                    temp1.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    //                                    temp1.ItemStyle.CssClass = "paddingClass";
                    //                                    temp1.ItemTemplate = new DynamicTemplateField(i); //Set the properties **ItemTemplate** as the instance of DynamicTemplateField class.

                    //                                    i = i + 1;
                    //                                    grdProgressHisotry.Columns.Add(temp1); //add the instance if template field in columns of grid view

                    //                                    dt.Columns.Add(new DataColumn(drG["FormSetGroupName"].ToString(), typeof(string)));

                    //                                    if (drG["HideColumnID"] != DBNull.Value && drG["HideColumnValue"] != DBNull.Value
                    //                                        && drG["HideOperator"] != DBNull.Value)
                    //                                    {
                    //                                        Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(drG["HideColumnID"].ToString()));
                    //                                        if (theHideColumn != null)
                    //                                        {
                    //                                            string strHideOperator = drG["HideOperator"].ToString();

                    //                                            string strHideColumnValue = drG["HideColumnValue"].ToString().ToLower();
                    //                                            string strValue = RecordManager.GetRecordValue(ref _qsRecord, theHideColumn.SystemName);

                    //                                            if (theHideColumn.DropDownType == "value_text" && theHideColumn.DropdownValues != "")
                    //                                            {
                    //                                                strValue = GetTextFromValue(theHideColumn.DropdownValues, strValue);
                    //                                            }

                    //                                            strValue = strValue.ToLower();
                    //                                            bool bFound = false;
                    //                                            if (strValue != "")
                    //                                            {
                    //                                                if (strHideOperator == "equals")
                    //                                                {
                    //                                                    if (strValue == strHideColumnValue)
                    //                                                    {
                    //                                                        bFound = true;

                    //                                                    }
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    if (strValue.IndexOf(strHideColumnValue) > -1)
                    //                                                    {
                    //                                                        bFound = true;

                    //                                                    }
                    //                                                }
                    //                                            }

                    //                                            if (bFound == false)
                    //                                            {
                    //                                                grdProgressHisotry.Columns[i - 2].Visible = false;
                    //                                            }                                            

                    //                                        }

                    //                                    }

                    //                                }



                    //                                string strMaxRowCount = Common.GetValueFromSQL("SELECT MAX(RowPosition) FROM FormSet WHERE FormSetGroupID IN (SELECT FormSetGroupID FROM FormSetGroup WHERE ParentTableID="+_qsTableID+")");

                    //                                if (strMaxRowCount == "")
                    //                                    strMaxRowCount = "0";

                    //                                int iMaxRowCount = int.Parse(strMaxRowCount);

                    //                                for (int j = 0; j <= iMaxRowCount; j++)
                    //                                {

                    //                                    DataRow dr = null;

                    //                                    dr = dt.NewRow();

                    //                                    foreach (DataRow drG in dtFormSetGroup.Rows)
                    //                                    {
                    //                                        dr[drG["FormSetGroupName"].ToString()] = Common.GetValueFromSQL("SELECT FormSetID FROM FormSet WHERE FormSetGroupID=" + drG["FormSetGroupID"].ToString() + " AND RowPosition=" + j.ToString());

                    //                                    }
                    //                                    dt.Rows.Add(dr);


                    //                                }



                    //                                grdProgressHisotry.DataSource = dt;
                    //                                grdProgressHisotry.DataBind();

                    //                            }
                    //                        }
                    //                        catch(Exception ex)
                    //                        {
                    //                            //
                    //                        }

                    //                    }

                    //end progress history



                    _qsRecordID = Cryptography.Decrypt(Request.QueryString["RecordID"]);
                    _iRecordID = int.Parse(_qsRecordID);
                    
                    //                    if (_theTable.ChangeHistoryType == "none" || _theTable.ChangeHistoryType == "")
                    //                    {
                    //                        //trTab.Visible = false;
                    //                        string strHistoryJS = @"  $(document).ready(function () {
                    //                                                     $('#lnkHideHistory').fadeOut();
                    //                                                    $('#divHistory').fadeOut();
                    //                                                 });";
                    //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "strHistoryJS", strHistoryJS, true);
                    //                    }
                    //                    else
                    //                    {
                    //                        lnkShowHistory.Visible = false;
                    //                        lnkHideHistory.Visible = false;

                    //                    }

                    if (!IsPostBack)
                    {

                        if (_theTable.ReasonChangeType == "mandatory")
                        {
                            rfvReasonForChange.Enabled = true;
                            stgReasonForChange.InnerText = "Reason for change*";
                            lnkNavigateNext.OnClientClick = "ValidatorEnable(document.getElementById('" + rfvReasonForChange.ClientID.ToString() + "'), false);" + "return true;";
                            lnkNavigatePrev.OnClientClick = lnkNavigateNext.OnClientClick;
                            //lnkSaveClose.OnClientClick = "ValidatorEnable(document.getElementById('" + rfvReasonForChange.ClientID.ToString() + "'), true);" + "return true;";
                        }

                        if (_theTable.ReasonChangeType == "none" || _theTable.ReasonChangeType == "")
                        {
                            trReasonForChange.Visible = false;
                        }




                        //if (_theTable.ChangeHistoryType == "admin")
                        //{
                        //    if (!Common.HaveAccess(_strRecordRightID, "1,2"))
                        //    {
                        //        trTab.Visible = false;
                        //    }
                        //}



                        if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                        {

                            gvChangedLog.PageSize = int.Parse(Session["GridPageSize"].ToString());

                        }


                        //BindTheChangedLogGrid(0, gvChangedLog.PageSize);

                    }

                    GridViewRow gvrCL = gvChangedLog.TopPagerRow;
                    if (gvrCL != null)
                        _gvCL_Pager = (Common_Pager)gvrCL.FindControl("CL_Pager");

                    if (!IsPostBack)
                    {
                        tabDetail.OnClientActiveTabChanged = "ClientActiveTabChangedEdit";
                    }

                }
                else
                {

                    //_strSessionRoleType == "8" || _strSessionRoleType == "5" || _strSessionRoleType == "6"
                    //    || 
                    tblNavigateRecords.Visible = false;
                    if (_strRecordRightID == Common.UserRoleType.ReadOnly
                        || _strRecordRightID == Common.UserRoleType.None)
                    {
                        Response.Redirect("~/Default.aspx", true);
                        return;
                    }


                    lnkShowHistory.Visible = false;
                    lnkHideHistory.Visible = false;

                    if (!IsPostBack)
                    {
                        //tabDetail.AutoPostBack = true;
                        tabDetail.OnClientActiveTabChanged = "ClientActiveTabChanged";
                    }

                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
        //Title = _theTable.TableName +  " Record - " + _strActionMode;
        //lblTitle.Text = _theTable.TableName + " Record - " + _strActionMode;


        // checking permission


        string strTitle = _theTable.TableName + _strActionMode;

        if (_theTable.ShowTabVertically != null)
        {
            if ((bool)_theTable.ShowTabVertically)
            {
                tabDetail.UseVerticalStripPlacement = true;
            }
        }

        if (!IsPostBack)
        {
            if (_strJS != "")
            {
                if (_qsMode != "view")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "_strJS", _strJS, true);
                }
            }


            PopulateTable();

        }

        if (_qsMode != "view" && _strJSPostBack != "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "_strJSPostBack", _strJSPostBack, true);

        }


        if (!IsPostBack)
        {

            //hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();

            //if (Request.QueryString["onlyback"] != null)
            //{
            //Temp

            //if (Request.UrlReferrer != null)
            //{
            //    hlBack.NavigateUrl = Request.UrlReferrer.AbsoluteUri;

            //}
            //else
            //{
            //    if (Request.QueryString["backurl"] != null)
            //    {
            //        hlBack.NavigateUrl = Cryptography.Decrypt(Request.QueryString["backurl"].ToString());
            //    }
            //}


            //if (Request.QueryString["tabindex"] != null)
            //{
            //    if (hlBack.NavigateUrl.IndexOf("&btabindex=") > -1)
            //    {
            //        hlBack.NavigateUrl = hlBack.NavigateUrl.Substring(0, hlBack.NavigateUrl.IndexOf("&btabindex="));
            //    }

            //    hlBack.NavigateUrl = hlBack.NavigateUrl + "&btabindex=" + Request.QueryString["tabindex"].ToString();
            //}

            //}

            StackProcess();





            //is it ok? process Tab
            if (Request.QueryString["tabindex"] != null)
            {
                if (hlBack.NavigateUrl.IndexOf("&btabindex=") > -1)
                {
                    hlBack.NavigateUrl = hlBack.NavigateUrl.Substring(0, hlBack.NavigateUrl.IndexOf("&btabindex="));
                }

                hlBack.NavigateUrl = hlBack.NavigateUrl + "&btabindex=" + Request.QueryString["tabindex"].ToString();
            }




            //OK Back
            if (_bPrivate == false)
            {
                if (!IsPostBack)
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Public.aspx?TableID=" + _qsTableID;
                }
            }


            //OK back
            if (Request.QueryString["quickadd"] != null)
            {
                try
                {
                    SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["quickadd"].ToString())));
                    if (theSearchCriteria != null)
                    {
                        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                        xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                        if (xmlDoc.FirstChild["RecordID"].InnerText == "-1")
                        {
                            hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode="
                                + xmlDoc.FirstChild["mode"].InnerText + "&TableID=" + xmlDoc.FirstChild["TableID"].InnerText
                                + "&SearchCriteriaID=" + xmlDoc.FirstChild["SearchCriteriaID"].InnerText;
                        }
                        else
                        {
                            hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode="
                              + xmlDoc.FirstChild["mode"].InnerText + "&TableID=" + xmlDoc.FirstChild["TableID"].InnerText
                              + "&SearchCriteriaID=" + xmlDoc.FirstChild["SearchCriteriaID"].InnerText + "&RecordID=" + xmlDoc.FirstChild["RecordID"].InnerText;
                        }
                    }
                }
                catch
                {

                }


            }


            if (Request.QueryString["UrlReferrer"] != null && Request.UrlReferrer != null)
            {
                hlBack.NavigateUrl = Request.UrlReferrer.AbsoluteUri;
            }

            if (Request.QueryString["fixedurl"] != null)
            {
                hlBack.NavigateUrl = Cryptography.Decrypt(Request.QueryString["fixedurl"].ToString());
            }



        }



        switch (_strActionMode.ToLower())
        {
            case "add":
                trTab.Visible = false;
                strTitle = "Add " + _theTable.TableName;
                break;

            case "view":

                strTitle = "View " + _theTable.TableName;
                if (!IsPostBack)
                {
                    if ((bool)_theUserRole.IsAdvancedSecurity)
                    {
                        if (_strRecordRightID == Common.UserRoleType.EditRecordSite || _strRecordRightID == Common.UserRoleType.AddEditRecord)
                        {
                            divEdit.Visible = true;
                            hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&Recordid=" + Request.QueryString["RecordID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();
                        }
                        if (_strRecordRightID == Common.UserRoleType.None)
                        {
                            Server.Transfer("~/Default.aspx");
                            return;
                        }
                    }
                    else
                    {
                        if (Common.HaveAccess(_strRecordRightID, "1,2,3,4"))
                        {
                            divEdit.Visible = true;
                            hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&Recordid=" + Request.QueryString["RecordID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();
                        }
                    }
                    //added and updated part
                    try
                    {
                        Record theRecord = RecordManager.ets_Record_Detail_Full(_iRecordID);
                        User userAdded = SecurityManager.User_Details((int)theRecord.EnteredBy);
                        _lblAddedTimeEmail.Text = theRecord.DateAdded.ToString() + "   By " + userAdded.Email;
                        //txtReasonForChange.Text = theRecord.ChangeReason;
                        if (theRecord.LastUpdatedUserID != null)
                        {
                            User userUpdated = SecurityManager.User_Details((int)theRecord.LastUpdatedUserID);
                            _lblUpdatedTimeEmail.Text = theRecord.DateUpdated.ToString() + "   By " + userUpdated.Email;
                        }
                    }
                    catch (Exception ex)
                    {


                    }

                }

                EnableTheRecordControls(false);
                //divSave.Visible = false;
                divSaveClose.Visible = false;


                break;

            case "edit":

                strTitle = "Edit " + _theTable.TableName;
                if (!IsPostBack)
                {
                    Record theRecord = RecordManager.ets_Record_Detail_Full(_iRecordID);
                    //txtReasonForChange.Text = theRecord.ChangeReason;
                }

                break;


            default:
                //?

                break;
        }

        Title = strTitle;
        lblTitle.Text = strTitle;

        string strFancy = @"$(function () {
            $("".popuplink"").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 600,
                height: 650,
                titleShow: false
            });
        });";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);


        if (Request.QueryString["onlyback"] != null)
        {

            divEdit.Visible = false;

        }

        if (!IsPostBack)
        {
            if (Request.QueryString["btabindex"] != null)
            {
                try
                {
                    tabDetail.ActiveTabIndex = int.Parse(Request.QueryString["btabindex"].ToString());
                }
                catch (Exception ex)
                {
                    //
                }
            }
            else
            {
                if (Request.QueryString["edittab"] != null)
                {
                    try
                    {
                        tabDetail.ActiveTabIndex = int.Parse(Request.QueryString["edittab"].ToString());

                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }


            }

            if (Session["viewtabindex"] != null)
            {
                try
                {
                    tabDetail.ActiveTabIndex = int.Parse(Session["viewtabindex"].ToString());
                }
                catch
                {

                }
            }

            Session["viewtabindex"] = null;
            if (tabDetail.ActiveTabIndex > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ActiveTabIndex", "$('#divMainSaveEditAddetc').fadeOut();", true);
            }


        }



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
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "UserBasicPostFile" + i.ToString(), "UserBasic" + i.ToString() + @"();", true);

                            if (_rfvValue[i] != null)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "UserBasicPostFileV" + i.ToString(), "ValidatorEnable(document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _rfvValue[i].ID.ToString() + "'), false);", true);
                            }
                        }
                    }
                    if (_hfValue2[i] != null && _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                    {
                        if (_hfValue2[i].Value != "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "UserBasicPostImage" + i.ToString(), "UserBasic" + i.ToString() + @"();", true);

                            if (_rfvValue[i] != null)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "UserBasicPostImageV" + i.ToString(), "ValidatorEnable(document.getElementById('ctl00_HomeContentPlaceHolder_tabDetail_tpDetail_" + _rfvValue[i].ID.ToString() + "'), false);", true);
                            }
                        }
                    }

                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file" &&
                        _hfValue[i] != null && _qsMode != "view"
                        && _hfValue2[i] != null)
                    {
                        if (_hfValue[i].Value != "" && _hfValue2[i].Value == "")
                        {
                            //_lblValue[i].Text = "<a target='_blank' href='" + _strFilesLocation + "/UserFiles/AppFiles/"
                            //       + _hfValue[i].Value + "'>" +
                            //       _hfValue[i].Value.Substring(37) + "</a>";


                            string strFilePath = Cryptography.Encrypt(_strFilesLocation + "/UserFiles/AppFiles/" + _hfValue[i].Value);
                            string strFileName = Cryptography.Encrypt(_hfValue[i].Value.Substring(37));

                            _lblValue[i].Text = "<a target='_blank' href='" + "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Filedownload.aspx?FilePath="
                                + strFilePath + "&FileName=" + strFileName + "'>" +
                                  _hfValue[i].Value.Substring(37) + "</a>";




                            _lblValue[i].Text = "<img  title=\"Remove this file\" style=\"cursor:pointer;\"  id=\"dimg" + _hfValue[i].ID + "\" src=\"" + "http://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />" + _lblValue[i].Text;

                            string strTempJS = @"  document.getElementById('dimg" + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _hfValue[i].ID + @"').value='';
                                                      $('#" + _lblValue[i].ID + @"').html(''); 
                                            });";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "filedelete2" + i.ToString(), strTempJS, true);


                        }
                    }
                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image" &&
                        _hfValue[i] != null && _qsMode != "view"
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


                            _lblValue[i].Text = "<img title=\"Remove this image\" style=\"cursor:pointer;\"  id=\"dimg" + _hfValue[i].ID + "\" src=\"" + "http://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />" + _lblValue[i].Text;


                            string strTempJS = @"  document.getElementById('dimg" + _hfValue[i].ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + _hfValue[i].ID + @"').value='';
                                                      $('#" + _lblValue[i].ID + @"').html(''); 
                                            });";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "imagedelete2" + i.ToString(), strTempJS, true);


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





    }

    protected void getLastUpdatedInfo()
    {
        if (_theRecord != null)
        {
            lblCreatedBy.Text = RecordManager.GetUserDisplayName("[Name]", Convert.ToString(_theRecord.EnteredBy));
            lblDateCreated.Text = _theRecord.DateAdded.ToString();

            if (_theRecord.LastUpdatedUserID != null)
            {
                lblUpdatedBy.Text = RecordManager.GetUserDisplayName("[Name]", Convert.ToString(_theRecord.LastUpdatedUserID));
                lblDateUpdated.Text = _theRecord.DateUpdated.ToString();
            }

            lblUpdatedBy.Visible = (lblUpdatedBy.Text.Trim() != "");
            lblUpdatedByText.Visible = (lblUpdatedBy.Text.Trim() != "");

            lblDateUpdated.Visible = (lblUpdatedBy.Text.Trim() != "");
            lblDateUpdatedText.Visible = (lblUpdatedBy.Text.Trim() != "");
        }
    }

    protected void AddDataNotification(Record newRecord)
    {
        DataTable dtAddDataNotification = Common.DataTableFromText(@"SELECT [TableUser].*,FirstName,LastName,PhoneNumber,Email 
                            FROM [TableUser] INNER JOIN [User] 
                            ON [TableUser].UserID=[User].UserID 
                            WHERE (AddDataEmail =1 OR AddDataSMS =1) AND IsActive=1 AND TableID=" + newRecord.TableID.ToString());


        if (dtAddDataNotification.Rows.Count > 0)
        {
            string strWarningSMSEMail = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail", null, _theTable.TableID);

            Content theContentEmail = SystemData.Content_Details_ByKey("AddDataEmail", (int)_theTable.AccountID);
            Content theContentSMS = SystemData.Content_Details_ByKey("AddDataSMS", (int)_theTable.AccountID);
            string strBody = "";
            string strBodySMS = "";


            foreach (DataRow drAD in dtAddDataNotification.Rows)
            {

                bool bEmail = false;
                bool bSMS = false;

                if (drAD["AddDataEmail"] != DBNull.Value)
                {
                    if ((bool)drAD["AddDataEmail"])
                    {
                        bEmail = true;
                    }
                }

                if (drAD["AddDataSMS"] != DBNull.Value)
                {
                    if ((bool)drAD["AddDataSMS"])
                    {
                        bSMS = true;
                    }
                }

                strBody = theContentEmail.ContentP;

                strBodySMS = theContentSMS.ContentP;

                strBody = strBody.Replace("[FirstName]", drAD["FirstName"].ToString());
                strBodySMS = strBodySMS.Replace("[FirstName]", drAD["FirstName"].ToString());

                strBody = strBody.Replace("[Table]", _theTable.TableName);
                strBodySMS = strBodySMS.Replace("[Table]", _theTable.TableName);

                strBody = strBody.Replace("[AddedBy]", _objUser.FirstName + " " + _objUser.LastName);
                strBodySMS = strBodySMS.Replace("[AddedBy]", _objUser.FirstName + " " + _objUser.LastName);

                //RecordURL
                string strRecordURL = "http://" + Request.Url.Authority + Request.ApplicationPath
                    + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaID="
                    + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&Recordid=" + Cryptography.Encrypt(newRecord.RecordID.ToString());

                strBody = strBody.Replace("[RecordURL]", strRecordURL);
                strBodySMS = strBodySMS.Replace("[RecordURL]", strRecordURL);

                string strWarning = "";
                if (newRecord.WarningResults == null)
                {
                    strWarning = "";
                }
                else
                {
                    strWarning = newRecord.WarningResults;
                }

                strBody = strBody.Replace("[Valid_with_warning]", strWarning);
                strBodySMS = strBodySMS.Replace("[Valid_with_warning]", strWarning);

                string strSubject = theContentEmail.Heading.Replace("[Table]", _theTable.TableName);


                try
                {

                    if (bEmail)
                    {
                        string sSendEmailError = "";

                        Message theMessage = new Message(null, newRecord.RecordID, newRecord.TableID, (int)_theTable.AccountID,
                   DateTime.Now, "E", "E",
                       null, drAD["Email"].ToString(), strSubject, strBody, null, "");

                        DBGurus.SendEmail("AddDataEmail", true, null, strSubject, strBody, "", drAD["Email"].ToString(), "", "", null, theMessage, out sSendEmailError);

                    }


                }
                catch (Exception ex)
                {

                    //strErrorMsg = "Server could not send warning Email & SMS";
                }




                if (drAD["PhoneNumber"] != DBNull.Value)
                {
                    if (drAD["PhoneNumber"].ToString() != "")
                    {
                        strSubject = theContentSMS.Heading.Replace("[Table]", _theTable.TableName);

                        try
                        {


                            if (bSMS)
                            {

                                string sSendEmailError = "";

                                Message theMessage = new Message(null, newRecord.RecordID, newRecord.TableID, (int)_theTable.AccountID,
                                         DateTime.Now, "E", "S",
                     null, drAD["PhoneNumber"].ToString() + strWarningSMSEMail, strSubject, strBody, null, "");  
                                DBGurus.SendEmail("AddDataSMS", null, true, strSubject, strBodySMS, "",
                                    drAD["PhoneNumber"].ToString() + strWarningSMSEMail, "", "", null, theMessage, out sSendEmailError);


                            }


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

    protected void PerformAllValidation(ref Record theRecord, ref DataTable dtValidWarning,
        bool bAddToGrid, bool bSendEmail)
    {
        bool bEachColumnExceedance = false;
        bool bEachColumnInValid = false;
        string strTemp = "";
        if (bSendEmail)
            bAddToGrid = false;//in case

        for (int i = 0; i < _dtColumnsAll.Rows.Count; i++)
        {
            //ALL Validation
            bEachColumnExceedance = false;
            bEachColumnInValid = false;
            string strValue = RecordManager.GetRecordValue(ref theRecord, _dtColumnsAll.Rows[i]["SystemName"].ToString());
            if (strValue != "")
            {
                //if (bSendEmail == false)
                //{

                    string strFormulaV = "";

                    if (_dtColumnsAll.Rows[i]["ConV"] != DBNull.Value)
                    {
                        Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsAll.Rows[i]["ConV"].ToString()));
                        if(theCheckColumn!=null)
                        {
                            string strCheckValue = RecordManager.GetRecordValue(ref theRecord, theCheckColumn.SystemName);
                            strFormulaV = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                "V", strCheckValue);
                        }
                    }
                    else
                    {
                        if(_dtColumnsAll.Rows[i]["ValidationOnEntry"] != DBNull.Value && _dtColumnsAll.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                        {
                            strFormulaV = _dtColumnsAll.Rows[i]["ValidationOnEntry"].ToString();
                        }
                    }

                    if (strFormulaV != "" && !UploadManager.IsDataValid(strValue, strFormulaV, ref _strValidationError))
                    {

                        _strInValidResults = _strInValidResults + " INVALID (and ignored): " + _dtColumnsAll.Rows[i]["DisplayName"].ToString() + ".";

                        bEachColumnInValid = true;

                        if (bAddToGrid)
                        {
                            dtValidWarning.Rows.Add(_dtColumnsAll.Rows[i]["ColumnID"].ToString(), "i", "no",
                            Common.GetFromulaMsg("i", _dtColumnsAll.Rows[i]["DisplayName"].ToString(), strFormulaV)//    "Invalid data - " + _dtColumnsAll.Rows[i]["DisplayName"].ToString()
                                ,strFormulaV, strValue);
                           
                        }
                    }
                //}

                //bEachColumnExceedance = false;
                if (_bShowExceedances && bEachColumnInValid==false)
                {

                    string strFormulaE = "";

                    if (_dtColumnsAll.Rows[i]["ConE"] != DBNull.Value)
                    {
                        Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsAll.Rows[i]["ConE"].ToString()));
                        if (theCheckColumn != null)
                        {
                            string strCheckValue = RecordManager.GetRecordValue(ref theRecord, theCheckColumn.SystemName);
                            strFormulaE = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                "E", strCheckValue);
                        }
                    }
                    else
                    {
                        if (_dtColumnsAll.Rows[i]["ValidationOnExceedance"] != DBNull.Value && _dtColumnsAll.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                        {
                            strFormulaE = _dtColumnsAll.Rows[i]["ValidationOnExceedance"].ToString();
                        }
                    }


                    if (strFormulaE!="")
                    {
                        if (!UploadManager.IsDataValid(strValue, strFormulaE, ref _strValidationError))
                        {
                            _strExceedanceResults = _strExceedanceResults + " EXCEEDANCE: " + _dtColumnsAll.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
                            bEachColumnExceedance = true;
                            //_bDataExceedance = true;
                            if (bAddToGrid)
                            {
                                dtValidWarning.Rows.Add(_dtColumnsAll.Rows[i]["ColumnID"].ToString(), "e", "yes",
                                  Common.GetFromulaMsg("e", _dtColumnsAll.Rows[i]["DisplayName"].ToString(), strFormulaE)//  "EXCEEDANCE: " + _dtColumnsAll.Rows[i]["DisplayName"].ToString() + " –  Value outside accepted range."
                                    , strFormulaE, strValue);
                            }

                            if (bSendEmail)
                            {
                                RecordManager.BuildDataExceedanceSMSandEmail(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), strValue, theRecord.DateTimeRecorded.ToString(),
                                    ref strTemp, _iSessionAccountID, _strURL, ref _strExceedanceEmailFullBody, ref _strExceedanceSMSFullBody, ref _iExceedanceColumnCount);

                            }
                        }

                    }
                }

                if (bEachColumnExceedance == false && bEachColumnInValid == false)
                {

                    string strFormulaW = "";

                    if (_dtColumnsAll.Rows[i]["ConW"] != DBNull.Value)
                    {
                        Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsAll.Rows[i]["ConW"].ToString()));
                        if (theCheckColumn != null)
                        {
                            string strCheckValue = RecordManager.GetRecordValue(ref theRecord, theCheckColumn.SystemName);
                            strFormulaW = UploadWorld.Condition_GetFormula(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), theCheckColumn.ColumnID,
                                "W", strCheckValue);
                        }
                    }
                    else
                    {
                        if (_dtColumnsAll.Rows[i]["ValidationOnWarning"] != DBNull.Value && _dtColumnsAll.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                        {
                            strFormulaW = _dtColumnsAll.Rows[i]["ValidationOnWarning"].ToString();
                        }
                    }


                    if (strFormulaW != "" && !UploadManager.IsDataValid(strValue, strFormulaW, ref _strValidationError))
                    {
                        _strWarningResults = _strWarningResults + " WARNING: " + _dtColumnsAll.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
                        //_bDataWarning = true;
                        if (bAddToGrid)
                        {
                            dtValidWarning.Rows.Add(_dtColumnsAll.Rows[i]["ColumnID"].ToString(), "w", "no",
                               Common.GetFromulaMsg("w", _dtColumnsAll.Rows[i]["DisplayName"].ToString(), strFormulaW)// "WARNING: " + _dtColumnsAll.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range."
                                , strFormulaW, strValue);
                        }

                        if (bSendEmail)
                        {
                            RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsAll.Rows[i]["ColumnID"].ToString()), strValue, theRecord.DateTimeRecorded.ToString(),
                                ref strTemp, _iSessionAccountID, _strURL, ref _strWarningEmailFullBody, ref _strWarningSMSFullBody, ref _iWarningColumnCount);
                        }
                    }

                }
            }
        }
    }
    protected void PopulateTable()
    {
        //int iTemp = 0;
        if (_iTableIndex >= 0)
        {
            //Table theTable = RecordManager.ets_Table_Details(int.Parse(_qsTableID));
            _txtValue[_iTableIndex].Text = _theTable.TableName;
        }
    }

    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtReasonForChange.Enabled = p_bEnable;
        trReasonForChange.Visible = false;
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

                    if (_txtValue[i] != null)
                        _txtValue[i].Enabled = p_bEnable;

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



    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)

    protected void tabDetail_ActiveTabChanged(object sender, EventArgs e)
    {
        if (tabDetail.ActiveTabIndex != 0)
        {
            if (PerformSave())
            {
                //string strEditURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&Recordid=" + Cryptography.Encrypt(_iNewRecordID.ToString()) + "&edittab=" + tabDetail.ActiveTabIndex.ToString();

                Response.Redirect(GetEditURLAfterAdd(), false);
            }
            else
            {
                tabDetail.ActiveTabIndex = 0;
            }
        }
        ResetTabs();
    }

    protected void lnkWordWxport_Click(object sender, EventArgs e)
    {
        if (ddlDataRetriever.Items.Count == 1)
        {
            lnkWordExportOK_Click(null, null);
        }
        else
        {

            mpeModalWordExport.Show();
        }
    }

    protected void lnkWordExportOK_Click(object sender, EventArgs e)
    {

        //

        try
        {
            string strError = "";
            if (ddlDataRetriever.SelectedItem != null)
            {


                DocTemplate theDocTemplate = DocumentManager.dbg_DocTemplate_Detail(int.Parse(ddlDataRetriever.SelectedValue));

                DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail((int)theDocTemplate.DataRetrieverID);

                DataTable dtRecordInfo = DocumentManager.DataRetrieverSP(null, _iRecordID, theDataRetriever.SPName);
                //string strFolder = "DocTemplates";
                string strFileToCopy = Server.MapPath("DocTemplates\\" + theDocTemplate.FileUniqueName);
                string strNewCopy = Server.MapPath("Temp\\" + theDocTemplate.FileUniqueName);
                if (System.IO.File.Exists(strNewCopy))
                    System.IO.File.Delete(strNewCopy);


                System.IO.File.Copy(strFileToCopy, strNewCopy);
                Common.GenerateWORDDoc(strNewCopy, dtRecordInfo, out strError);

                mpeModalWordExport.Hide();

                //System.Threading.Thread.Sleep(5000);

                Page.Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Temp/" + theDocTemplate.FileUniqueName, false);
                //Response.Flush();
                //System.Threading.Thread.Sleep(5000);
                //Server.Transfer( Server.MapPath( "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Temp/" + theDocTemplate.FileUniqueName));
            }
            mpeModalWordExport.Hide();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This template file type is not supported, please use .docx format file.');", true);
        }

    }

    protected void lnkSaveAndStay_Click(object sender, EventArgs e)
    {
        if (_strActionMode.ToLower() == "add")
        {
            _theTable.ShowEditAfterAdd = true;

        }

        if (_strActionMode.ToLower() == "edit")
        {
            _bRedirect = false;
        }

        lnkSaveClose_Click(null, null);
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveAndStay", "alert('Record Saved.');", true);
    }


    private void StackProcess()
    {
        //if (Request.QueryString["stackhault"] != null)
        //{
        //    return;
        //}


        try
        {

            //stack process



            if (Request.QueryString["stackzero"] != null && Request.QueryString["stackhault"] == null)
            {
                Session["stack"] = null;
                Session["backcount"] = null;
            }


            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.AbsoluteUri.IndexOf("DocGen/EachRecordTable.aspx") > -1)
                {
                    Session["stack"] = null;
                    Session["backcount"] = null;
                }
            }

            if (Session["stack"] == null)
            {
                BuildFreshStack();
            }
            else
            {

                //ok we have new deep level

                Stack<string> stack = (Stack<string>)Session["stack"];

                if (stack.Count > 0)
                {

                    string strRawURL = GetStackRawURL();

                    if (stack.Contains(strRawURL) == false)
                    {
                        if (Request.QueryString["stack"] != null)
                        {
                            if (Session["backcount"] != null)
                            {
                                stack.Pop();
                                hlBack.NavigateUrl = stack.Peek();
                            }
                            else
                            {
                                Session["backcount"] = 1;
                                stack.Pop();
                                if (stack.Count > 1)
                                {
                                    stack.Pop();
                                    hlBack.NavigateUrl = stack.Peek();
                                }
                                else if (stack.Count == 1)
                                {
                                    hlBack.NavigateUrl = stack.Peek();
                                }
                            }

                        }
                        else
                        {
                            hlBack.NavigateUrl = stack.Peek();

                            if (Session["backcount"] != null)
                            {
                                Session["backcount"] = null;

                                if (Request.UrlReferrer != null && Request.QueryString["stackhault"] == null)
                                {
                                    hlBack.NavigateUrl = Request.UrlReferrer.AbsoluteUri;

                                    if (stack.Contains(hlBack.NavigateUrl) == false)
                                    {
                                        stack.Push(hlBack.NavigateUrl);
                                    }
                                }

                            }

                            string strURL = Common.GetUpdatedFullURLWithQueryString(Request.RawUrl, "stack", "n");

                            if (stack.Contains(strURL) == false)
                            {
                                stack.Push(strURL);
                            }
                        }
                        if (Request.QueryString["stackhault"] == null)
                        {
                            Session["stack"] = stack;
                        }

                    }
                    else
                    {
                        stack.Pop();
                        hlBack.NavigateUrl = stack.Peek();
                        stack.Push(GetStackRawURL());
                        Session["stack"] = stack;

                    }

                }
            }


        }
        catch
        {
            //
        }



    }
    protected void BuildFreshStack()
    {
        hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();
        if (Request.QueryString["View"] != null)
        {
            hlBack.NavigateUrl = hlBack.NavigateUrl + "&View=" + Request.QueryString["View"].ToString();
        }
        Stack<string> stack = new Stack<string>();
        stack.Push(hlBack.NavigateUrl);
        stack.Push(GetStackRawURL());
        Session["stack"] = stack;
    }
    protected string GetStackRawURL()
    {
        string strRawURL = Request.RawUrl;
        strRawURL = Common.GetUpdatedFullURLRemoveQueryString(strRawURL, "stack");
        strRawURL = Common.GetUpdatedFullURLRemoveQueryString(strRawURL, "stackzero");
        strRawURL = Common.GetUpdatedFullURLWithQueryString(strRawURL, "stack", "n");
        return strRawURL;
    }
    protected void lnkSaveAndAdd_Click(object sender, EventArgs e)
    {

        try
        {
            if (PerformSave())
            {

                //if (Request.QueryString["stackhault"] == null && Session["stack"]!=null)
                //{
                //    Stack<string> stack = (Stack<string>)Session["stack"];
                //    if (stack.Count > 0)
                //    {
                //        stack.Pop();
                //        Session["stack"] = stack;
                //    }
                //}
                string strURL = "http://" + Request.Url.Authority + Request.RawUrl;
                strURL = strURL.Replace("&stack=n", "");
                strURL = strURL.Replace("&stackzero=y", "");
                //strURL = strURL.Replace("&stackhault=y", ""); //onlyback=yes
                strURL = strURL.Replace("&onlyback=yes", "");
                //strURL = strURL + "&stackhault=y";


                strURL = strURL.Replace("mode=" + Cryptography.Encrypt("edit").Replace("=", "%3d"), "mode=" + Cryptography.Encrypt("add").Replace("=", "%3d"));
                strURL = strURL.Replace("mode=" + Cryptography.Encrypt("view").Replace("=", "%3d"), "mode=" + Cryptography.Encrypt("add").Replace("=", "%3d"));
                strURL = strURL.Replace("&Recordid=" + Cryptography.Encrypt(_qsRecordID).Replace("=", "%3d"), "");

                Stack<string> stack = (Stack<string>)Session["stack"];
                stack.Pop();
                Session["stack"] = stack;
                Response.Redirect(strURL, true);
            }
        }
        catch
        {

        }
        ResetTabs();
    }
    protected void lnkSaveClose_Click(object sender, EventArgs e)
    {

        if (sender != null)
        {

            ViewState["SaveCaller"] = "save";
        }

        try
        {
            if (PerformSave())
            {
                if (_strActionMode.ToLower() == "add")
                {
                    //Response.Redirect(hlEditLink.NavigateUrl, false);



                    if (_bPrivate == false)
                    {
                        Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx" + "?frompublic=yes", false);
                    }
                    else
                    {

                        if (Request.QueryString["quickadd"] != null)
                        {
                            hlBack.NavigateUrl = hlBack.NavigateUrl + "&quickdone=" + Request.QueryString["quickadd"].ToString() + "&controlvalue=" + Cryptography.Encrypt(_iNewRecordID.ToString());
                        }


                        if (_theTable.ShowEditAfterAdd != null && (bool)_theTable.ShowEditAfterAdd)
                        {

                            Response.Redirect(GetEditURLAfterAdd(), false);
                        }
                        else
                        {
                            Response.Redirect(hlBack.NavigateUrl, false);
                        }
                    }


                }
                else
                {
                    if (_bRedirect)
                    {
                        Response.Redirect(hlBack.NavigateUrl, false);
                    }

                }
            }
            
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message + "  " + ex.StackTrace;
           
        }
        ResetTabs();
    }


    protected void lnkOk_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        lblMsg.ToolTip = "";
        //ViewState["ok"] = "ok";

        lnkSaveClose_Click(null, null);

    }




    protected void gvValidWarningGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {



            try
            {


                string strColumnID = ((Label)e.Row.FindControl("lblColumnID")).Text;

                string strValidationType = ((Label)e.Row.FindControl("lblValidationType")).Text;
                CheckBox chkIgnore = ((CheckBox)e.Row.FindControl("chkIgnore"));
                string strValidWarningMsg = ((Label)e.Row.FindControl("lblValidWarningMsg")).Text;
                string strValidWarningFormula = ((Label)e.Row.FindControl("lblValidWarningFormula")).Text;
                string strVWValue = ((Label)e.Row.FindControl("lblValue")).Text;

                Label lblFullMsg = ((Label)e.Row.FindControl("lblFullMsg"));

                lblFullMsg.Text = strValidWarningMsg;// +"<img src='../../Images/GetInfo.png' title='" + strValidWarningFormula + "' />";

                chkIgnore.Checked = false;

                if (strValidationType == "w")
                {
                    lblFullMsg.ForeColor = System.Drawing.Color.Blue;
                    //chkIgnore.Enabled = false;
                }
                else if (strValidationType == "e")
                {
                    lblFullMsg.ForeColor = System.Drawing.Color.Orange;
                    //chkIgnore.Checked = true;
                    //chkIgnore.Enabled = false;
                }
                else if (strValidationType == "c")
                {
                    lblFullMsg.ForeColor = System.Drawing.Color.DarkRed;
                    //chkIgnore.Enabled = false;
                }
                else
                {
                    lblFullMsg.ForeColor = System.Drawing.Color.Red;                  

                }

                if (strValidationType == "c" || strValidationType == "i")
                {
                    try
                    {
                        Column theColumn = RecordManager.ets_Column_Details(int.Parse(strColumnID));
                        if (theColumn.ValidationCanIgnore != null && (bool)theColumn.ValidationCanIgnore)
                        {

                            chkIgnore.Enabled = true;
                        }
                        else
                        {
                            chkIgnore.Checked = false;
                            chkIgnore.Enabled = false;
                        }
                    }
                    catch
                    {

                    }
                }
                

            }
            catch
            {
                //

            }
        }
    }

    protected void CreateValidWarningDataTable()
    {
        DataTable dtValidWarning = new DataTable();
        dtValidWarning.Columns.Add("ColumnID");
        dtValidWarning.Columns.Add("ValidationType");
        dtValidWarning.Columns.Add("Ignore");
        dtValidWarning.Columns.Add("ValidWarningMsg");
        dtValidWarning.Columns.Add("ValidWarningFormula");
        dtValidWarning.Columns.Add("Value");

        ViewState["dtValidWarning"] = dtValidWarning;
    }

    protected void lnkValidWarningRefresh_Click(object sender, EventArgs e)
    {
        ViewState["SaveCaller"] = "refresh";
        lblMsg.Text = "";
        lblMsg.ToolTip = "";
        lnkSaveClose_Click(null, null);

    }
    protected void lnkNavigatePrev_Click(object sender, EventArgs e)
    {

        try
        {
            if (PerformSave())
            {

                Response.Redirect(hlNavigatePrev.NavigateUrl, false);
            }
        }
        catch
        {

        }

    }

    protected string GetEditURLAfterAdd()
    {
        Stack<string> stack = (Stack<string>)Session["stack"];


        string strEditURL = stack.Peek();


        strEditURL = strEditURL.Replace("mode=" + Cryptography.Encrypt("add"), "mode=" + Cryptography.Encrypt("edit"));
        if (Request.QueryString["CopyRecordID"] != null)
        {
            strEditURL = strEditURL.Replace("&CopyRecordID=" + Request.QueryString["CopyRecordID"].ToString(), "");
        }


        strEditURL = strEditURL + "&Recordid=" + Cryptography.Encrypt(_iNewRecordID.ToString()) + "&edittab=" + tabDetail.ActiveTabIndex.ToString();
        strEditURL = strEditURL.Replace("&stack=n", "");
        stack.Pop();
        Session["stack"] = stack;
        return strEditURL;
    }


    protected void lnkShowHistory_Click(object sender, EventArgs e)
    {
        lnkHideHistory.Visible = true;
        trTab.Visible = true;
        lnkShowHistory.Visible = false;

        if (_qsRecordID != "")
        {
            TheDatabaseS.spAuditRawToAudit(int.Parse(_qsRecordID));

            BindTheChangedLogGrid(0, gvChangedLog.PageSize);
        }
    }

    protected void lnkHideHistory_Click(object sender, EventArgs e)
    {
        lnkShowHistory.Visible = true;
        lnkHideHistory.Visible = false;
        trTab.Visible = false;

    }

    protected void lnkNavigateNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (PerformSave())
            {

                Response.Redirect(hlNavigateNext.NavigateUrl, false);
            }
        }
        catch
        {

        }

    }

    protected void lnkNo_Click(object sender, EventArgs e)
    {
        //ViewState["ok"] = "no";
        trMainSave.Visible = true;
        //trConfirmation.Visible = false;
        lblMsg.Text = "";
        lblMsg.ToolTip = "";

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
                if (_strActionMode.ToLower() == "add")
                {
                    Response.Redirect(hlEditLink.NavigateUrl, false);
                    return;
                }
            }           

        }
        catch (Exception ex)
        {
            //
        }
        ResetTabs();
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

                        if (strEmail.Trim() == "" || strPassword.Trim() == "")
                            return;

                        User newUser = new User(null, "Auto",
                       "Created", "Phone", strEmail, strPassword,
                       true, DateTime.Now, DateTime.Now);//, "", false, true
                        //newUser.IsDocSecurityAdvanced = true;

                        int iNewUserID = SecurityManager.User_Insert(newUser);

                        int iTN = 0;
                        List<Role> lstRole = SecurityManager.Role_Select(null, "", "8", "", null, null, "", "", null, null, ref iTN,
                            _iSessionAccountID, null, null);
                        Role theRole = new Role(null, "", "", "", null, null);
                        foreach (Role tempRole in lstRole)
                        {
                            theRole = tempRole;
                        }

                        UserRole newUserRole = new UserRole(null, iNewUserID, (int)theRole.RoleID, DateTime.Now, DateTime.Now);
                        newUserRole.AccountID = _iSessionAccountID;// int.Parse(Session["AccountID"].ToString());
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

                                strBody = strBody.Replace("[URL]", "http://" + Request.Url.Authority + Request.ApplicationPath);

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

    protected DataTable GetIgnoreValidWarning(DataTable dtValidWarning, DataTable dtValidWarningOri)
    {

        DataTable dtValidWarningP = dtValidWarning.Copy();
        foreach (DataRow drO in dtValidWarningOri.Rows)
        {
            for (int i = dtValidWarningP.Rows.Count - 1; i >= 0; i--)
            {
                if (drO["Ignore"].ToString() == "yes" && drO["ColumnID"].ToString() == dtValidWarningP.Rows[i]["ColumnID"].ToString()
                    && drO["ValidationType"].ToString() == dtValidWarningP.Rows[i]["ValidationType"].ToString())
                {
                    if (drO["ValidationType"].ToString() == "e" || drO["ValidationType"].ToString() == "w")
                    {
                        if (drO["Value"].ToString().Trim() == dtValidWarningP.Rows[i]["Value"].ToString().Trim())
                        {
                            dtValidWarningP.Rows[i].Delete();
                            dtValidWarningP.AcceptChanges();
                        }
                    }
                    else
                    {
                        dtValidWarningP.Rows[i].Delete();
                        dtValidWarningP.AcceptChanges();
                    }
                }
            }

        }


        //dtValidWarning.AcceptChanges();
        return dtValidWarningP;
    }

    protected bool PerformSave()
    {
        bool bPeformSave = true;

        lblMsg.ForeColor = System.Drawing.Color.Red;

        DataTable dtValidWarning = ((DataTable)ViewState["dtValidWarning"]).Clone();

        DataTable dtValidWarningOri = ((DataTable)ViewState["dtValidWarning"]).Clone();

        string strUniqueColumnIDSys = "";
        string strUniqueColumnID2Sys = "";

        if (_theTable.UniqueColumnID != null)
            strUniqueColumnIDSys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + _theTable.UniqueColumnID.ToString());

        if (_theTable.UniqueColumnID2 != null)
            strUniqueColumnID2Sys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE ColumnID=" + _theTable.UniqueColumnID2.ToString());




        if (gvValidWarningGrid.Rows.Count > 0)
        {
            for (int i = 0; i < gvValidWarningGrid.Rows.Count; i++)
            {
                string strColumnID = ((Label)gvValidWarningGrid.Rows[i].FindControl("lblColumnID")).Text;
                string strValidationType = ((Label)gvValidWarningGrid.Rows[i].FindControl("lblValidationType")).Text;
                bool bIgnore = ((CheckBox)gvValidWarningGrid.Rows[i].FindControl("chkIgnore")).Checked;
                string strIgnore = bIgnore == true ? "yes" : "no";
                string strValidWarningMsg = ((Label)gvValidWarningGrid.Rows[i].FindControl("lblValidWarningMsg")).Text;
                string strValidWarningFormula = ((Label)gvValidWarningGrid.Rows[i].FindControl("lblValidWarningFormula")).Text;
                string strVWValue = ((Label)gvValidWarningGrid.Rows[i].FindControl("lblValue")).Text;

                dtValidWarningOri.Rows.Add(strColumnID, strValidationType, strIgnore, strValidWarningMsg, strValidWarningFormula, strVWValue);


            }

        }

        while (dtValidWarning.Rows.Count > 0)
        {
            dtValidWarning.Rows[0].Delete();
        }

        if (ViewState["SaveCaller"] != null)
        {
            if (ViewState["SaveCaller"].ToString() == "refresh")
            {
                bPeformSave = false;

            }
        }
        if (_bCancelSave)
        {
            bPeformSave = false;
        }

        lblMsg.Text = "";
        lblMsg.ToolTip = "";


        //bool bEachColumnExceedance = false;



        string strValue = "";
        try
        {

            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":


                        if (SecurityManager.IsRecordsExceeded(_iSessionAccountID))
                        {
                            Session["DoNotAllow"] = "true";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "DoNotAllow", "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);

                            return false;
                        }


                        Record newRecord = new Record();

                        newRecord.TableID = _theTable.TableID;

                        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                        {
                            //bEachColumnExceedance = false;

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
                                                //strTimePart = " 12:00:00 AM";
                                                strTimePart = "00:00";
                                            }
                                            else
                                            {
                                                if (_txtTime[i].Text.ToLower().IndexOf(":am") > 0)
                                                {
                                                    //strTimePart = _txtTime[i].Text.ToLower().Replace(":am", ":00 AM");
                                                    strTimePart = _txtTime[i].Text.ToLower().Replace(":am", ":00");
                                                }
                                                else
                                                {
                                                    //strTimePart = _txtTime[i].Text.ToLower().Replace(":pm", ":00 PM");
                                                    strTimePart = _txtTime[i].Text.ToLower().Replace(":pm", ":00");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strTimePart = "00:00";
                                        }

                                        strDateTime = _txtValue[i].Text + " " + strTimePart;
                                        strDateTime = strDateTime.Replace("  ", " ");
                                    }
                                    strValue = strDateTime;

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                                {
                                    string strDateTime = "";
                                    if (_txtValue[i].Text.Trim() == "")
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
                                        strUniqueName = Common.GetValidFileName(strUniqueName);
                                        string strPath = _strFilesPhisicalPath + strFolder + "\\" + strUniqueName;

                                        _fuValue2[i].SaveAs(strPath);
                                        _hfValue[i].Value = strUniqueName;
                                    }

                                    strValue = _hfValue[i].Value;
                                }

                                //if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "slider")
                                //{
                                //    strValue = _txtValue[i].Text;
                                //}

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "text"
                                        || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                                {
                                    strValue = _txtValue[i].Text;
                                    if (_dtColumnsDetail.Rows[i]["NumberType"] != null)
                                    {
                                        if (_dtColumnsDetail.Rows[i]["NumberType"].ToString() == "8")
                                        {
                                            try
                                            {
                                                string strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + _dtColumnsDetail.Rows[i]["SystemName"].ToString() + ")) FROM Record WHERE IsNumeric(" + _dtColumnsDetail.Rows[i]["SystemName"].ToString() + ")=1 and  TableID=" + _qsTableID);
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
                                    if (_ddlValue[i] != null && _ddlValue[i].SelectedIndex != null && _ddlValue[i].SelectedIndex != 0)
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
                                            _hfValue3[i].Value = "3";

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

                                    if ((newLocationColumn.Latitude != null && newLocationColumn.Longitude != null) || !string.IsNullOrEmpty(newLocationColumn.Address))
                                    {
                                        strValue = newLocationColumn.GetJSONString();
                                        if (_dtColumnsDetail.Rows[i]["ShowTotal"] != DBNull.Value && _dtColumnsDetail.Rows[i]["ShowTotal"].ToString().ToLower() == "true"
                                            && string.IsNullOrEmpty(newLocationColumn.Address))
                                        {
                                            strValue = "";
                                        }

                                        if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value && _dtColumnsDetail.Rows[i]["IsRound"].ToString().ToLower() == "true"
                                            && (newLocationColumn.Latitude == null || newLocationColumn.Longitude == null))
                                        {
                                            strValue = "";
                                        }
                                    }
                                    else
                                    {
                                        strValue = "";
                                    }


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




                                if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value
                                    && _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
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

                                //          if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                                //&& _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "linked"
                                //&& _dtColumnsDetail.Rows[i]["TableTableID"] != DBNull.Value
                                //&& _dtColumnsDetail.Rows[i]["ParentColumnID"] != DBNull.Value
                                //&& _dtColumnsDetail.Rows[i]["DisplayColumn"].ToString() != "")
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
                                            if (_txtValue[i].Text.Trim() == "")
                                                _hfValue[i].Value = "";

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

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation"
                                                && _dtColumnsDetail.Rows[i]["Calculation"] != DBNull.Value
                                && _txtValue[i] != null)
                                {
                                    strValue = "";
                                    string strTempValue = "";
                                    //string strOrginalValue = "";

                                    if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                                    && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "d")
                                    {
                                        //datetime calculation

                                        string strCalculation = _dtColumnsDetail.Rows[i]["Calculation"].ToString();

                                        try
                                        {
                                            strTempValue = TheDatabaseS.GetDateCalculationResult(ref _dtColumnsAll, strCalculation, null, newRecord, _iParentRecordID,
                                           _dtColumnsDetail.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString(),
                                           null, _theTable, _bCheckIgnoreMidnight);

                                            strValue = strTempValue;
                                        }
                                        catch
                                        {
                                            //
                                        }

                                    }
                                    else if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                                   && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "t")
                                    {
                                        //text calculation
                                        try
                                        {
                                            string strFormula = Common.GetCalculationSystemNameOnly(_dtColumnsDetail.Rows[i]["Calculation"].ToString(), (int)_theTable.TableID);

                                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()));
                                            strTempValue = TheDatabaseS.GetTextCalculationResult(ref _dtColumnsAll, strFormula, null, newRecord, _iParentRecordID, null, _theTable, theColumn);
                                            strValue = strTempValue;
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
                                            //string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtColumnsDetail.Rows[i]["Calculation"].ToString(),null);

                                            string strFormula = Common.GetCalculationSystemNameOnly(_dtColumnsDetail.Rows[i]["Calculation"].ToString(), (int)_theTable.TableID);

                                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()));
                                            strTempValue = TheDatabaseS.GetCalculationResult(ref _dtColumnsAll, strFormula, null, newRecord, _iParentRecordID, null, _theTable, theColumn);
                                            strValue = strTempValue;
                                            //if (_dtColumnsDetail.Rows[i]["RoundNumber"]!=DBNull.Value)
                                            //{
                                            //    strValue = Math.Round(double.Parse(Common.IgnoreSymbols(strValue)), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                                            //}
                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }

                                    _txtValue[i].Text = strValue;

                                }



                                RecordManager.MakeTheRecord(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strValue);
                                if (strValue.Length > 0)
                                {




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
                                                    strRecordedate = Common.IgnoreSymbols(strValue);
                                                }
                                                else
                                                {
                                                    strRecordedate = strValue;
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
                                                        _strWarningResults = _strWarningResults + " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";

                                                        dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "w", "no", "WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.",
                                                "CheckUnlikelyValue", strRecordedate.ToString());

                                                    }

                                                }
                                            }
                                        }

                                    }

                                }//                                    

                            }
                        }

                        try
                        {
                            for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                            {
                                strValue = RecordManager.GetRecordValue(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString());

                                ////ALL Validation
                                //if (strValue != "")
                                //{
                                //    if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"] == DBNull.Value)
                                //    {
                                //        //do nothing
                                //    }
                                //    else
                                //    {
                                //        if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                                //        {
                                //            if (UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString(), ref strValidationError))
                                //            {
                                //                //strValidationResults = strValidationResults + "\n" + _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " - " + "VALID";
                                //            }
                                //            else
                                //            {
                                //                //lblMsg.Text = "Invalid data - " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString();

                                //                dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "i", "no", "Invalid data - " + _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString(),
                                //             _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString(), strValue);

                                //                if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() == ""
                                //                    && _dtColumnsDetail.Rows[i]["TableTableID"] == DBNull.Value)
                                //                {
                                //                    if (_txtValue[i] != null)
                                //                        _txtValue[i].Focus();
                                //                    //_txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString();
                                //                }
                                //                else if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                                //                   && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                                //                    || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                                //                {
                                //                    if (_ddlValue[i] != null)
                                //                        _ddlValue[i].Focus();
                                //                    //_ddlValue[i].ToolTip = _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString();
                                //                }


                                //                //lblMsg.ToolTip = _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString();

                                //                //return false;
                                //            }
                                //        }
                                //    }

                                //    bEachColumnExceedance = false;
                                //    if (_bShowExceedances)
                                //    {
                                //        if (_dtColumnsDetail.Rows[i]["ValidationOnExceedance"] != DBNull.Value)
                                //        {
                                //            if (_dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                                //            {
                                //                if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString(), ref strValidationError))
                                //                {
                                //                    strExceedanceResults = strExceedanceResults + " EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";
                                //                    bEachColumnExceedance = true;
                                //                    dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "e", "yes", "EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.",
                                //            _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString(), strValue);

                                //                    bDataExceedance = true;

                                //                }
                                //            }
                                //        }
                                //    }

                                //    if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"] != DBNull.Value && bEachColumnExceedance == false)
                                //    {

                                //        if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                                //        {
                                //            if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), ref strValidationError))
                                //            {
                                //                strWarningResults = strWarningResults + " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";

                                //                dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "w", "no", "WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.",
                                //                _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), strValue);

                                //                bDataWarning = true;
                                //            }
                                //        }
                                //    }
                                //}

                                if (_dtColumnsDetail.Rows[i]["DefaultType"].ToString() == "parent"
                                && _dtColumnsDetail.Rows[i]["DefaultColumnID"] != DBNull.Value
                                && _iParentRecordID == null)
                                {

                                    if (strValue == "")
                                    {
                                        Column theDefaultColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["DefaultColumnID"].ToString()));
                                        int? iParentRecordID = _iParentRecordID;

                                        string strParentSys = Common.GetValueFromSQL(@"SELECT S2.SystemName FROM [Column] S2 INNER JOIN [Column] S
                                                                        ON S2.TableTableID=S.TableTableID
                                                                        WHERE S2.TableID=" + newRecord.TableID.ToString() + " AND S.TableID =" + theDefaultColumn.TableID.ToString());

                                        if (strParentSys != "")
                                        {
                                            string strParentID = RecordManager.GetRecordValue(ref newRecord, strParentSys);
                                            if (strParentID != "")
                                            {
                                                int iTempPID = 0;

                                                if (int.TryParse(strParentID, out iTempPID))
                                                {
                                                    iParentRecordID = iTempPID;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string strChildSys = Common.GetValueFromSQL(@"SELECT C.SystemName FROM [Column] C INNER JOIN [Column] P
                                                                        ON C.TableTableID=P.TableID
                                                                        WHERE P.TableID=" + theDefaultColumn.TableID.ToString() + " AND C.TableID =" + newRecord.TableID.ToString());

                                            if (strChildSys != "")
                                            {
                                                string strPR = RecordManager.GetRecordValue(ref newRecord, strChildSys);
                                                int iTestD = 0;
                                                if (strPR != "" && int.TryParse(strPR, out iTestD))
                                                {
                                                    iParentRecordID = iTestD;
                                                }
                                            }
                                        }

                                        if (iParentRecordID != null)
                                        {
                                            strValue = TheDatabaseS.spGetValueFromRelatedTable((int)iParentRecordID, (int)theDefaultColumn.TableID, theDefaultColumn.SystemName);
                                            RecordManager.MakeTheRecord(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strValue);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            //
                        }

                        try
                        {
                            for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                            {
                                string strValue1 = "";
                                strValue1 = RecordManager.GetRecordValue(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString());

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation" && strValue1 != "" && strValue1.IndexOf(" ") > -1)
                                {
                                    strValue1 = strValue1.Substring(0, strValue1.IndexOf(" "));
                                    strValue1 = strValue1.Trim();
                                    strValue1 = Common.IgnoreSymbols(strValue1);
                                }


                                if (_dtColumnsDetail.Rows[i]["CompareColumnID"] != DBNull.Value
                    && _dtColumnsDetail.Rows[i]["CompareOperator"] != DBNull.Value && strValue1.Length > 0)
                                {
                                    bool bValidationCanIgnore = false;
                                    string sCanignore = "no";
                                    if (_dtColumnsDetail.Rows[i]["ValidationCanIgnore"] != DBNull.Value)
                                    {
                                        if ((bool)_dtColumnsDetail.Rows[i]["ValidationCanIgnore"])
                                        {
                                            bValidationCanIgnore = true;
                                            sCanignore = "yes";
                                        }
                                    }

                                    Column theComparisonColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["CompareColumnID"].ToString()));

                                    string strValue2 = "";
                                    if (theComparisonColumn.TableID.ToString() != "" && theComparisonColumn.TableID.ToString() == _qsTableID)
                                    {
                                        strValue2 = RecordManager.GetRecordValue(ref newRecord, theComparisonColumn.SystemName);
                                    }
                                    if (theComparisonColumn.TableID.ToString() != "" && theComparisonColumn.TableID.ToString() != _qsTableID && _iParentRecordID != null)
                                    {
                                        strValue2 = TheDatabaseS.spGetValueFromRelatedTable((int)_iParentRecordID, (int)theComparisonColumn.TableID, theComparisonColumn.SystemName);
                                    }

                                    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation" && strValue2 != "" && strValue2.IndexOf(" ") > -1)
                                    {
                                        strValue2 = strValue2.Substring(0, strValue2.IndexOf(" "));
                                        strValue2 = strValue2.Trim();
                                        strValue2 = Common.IgnoreSymbols(strValue2);
                                    }

                                    string strComparisonErrorMessage = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " " + Common.CompareOperatorErrorMsg(_dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " " + theComparisonColumn.DisplayName;

                                    string strComparisonErrorToolTip = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + "(" + strValue1 + ")" + " "
                                        + Common.CompareOperatorErrorMsg(_dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " " + theComparisonColumn.DisplayName + "(" + strValue2 + ")";

                                    if (strValue2.Length > 0) //bValidationCanIgnore && 
                                    {
                                        //need server side validation                                        

                                        bool bValid = false;
                                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime"
                       || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time"
                                            || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                                        {
                                            DateTime comValue1 = DateTime.Today;
                                            DateTime comValue2 = DateTime.Today;
                                            try
                                            {
                                                comValue1 = DateTime.Parse(strValue1);
                                                comValue2 = DateTime.Parse(strValue2);

                                            }
                                            catch
                                            {

                                            }

                                            try
                                            {
                                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                                                {
                                                    comValue1 = DateTime.Parse(comValue1.Hour.ToString() + ":" + comValue1.Minute.ToString());
                                                    comValue2 = DateTime.Parse(comValue2.Hour.ToString() + ":" + comValue2.Minute.ToString());

                                                }
                                            }
                                            catch
                                            {

                                            }
                                            switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                                            {
                                                case "Equal":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "DataTypeCheck":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThan":

                                                    if (comValue1 > comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThanEqual":

                                                    if (comValue1 >= comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThan":

                                                    if (comValue1 < comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThanEqual":

                                                    if (comValue1 <= comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "NotEqual":

                                                    if (comValue1 != comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                default:

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;

                                            }


                                        }
                                        else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                                        {
                                            double comValue1 = 0;
                                            double comValue2 = 0;
                                            try
                                            {
                                                comValue1 = double.Parse(strValue1);
                                                comValue2 = double.Parse(strValue2);

                                            }
                                            catch
                                            {

                                            }


                                            switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                                            {
                                                case "Equal":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "DataTypeCheck":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThan":

                                                    if (comValue1 > comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThanEqual":

                                                    if (comValue1 >= comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThan":

                                                    if (comValue1 < comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThanEqual":

                                                    if (comValue1 <= comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "NotEqual":

                                                    if (comValue1 != comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                default:

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;

                                            }

                                        }
                                        else
                                        {
                                            string comValue1 = strValue1;
                                            string comValue2 = strValue2;

                                            switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                                            {
                                                case "Equal":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "DataTypeCheck":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThan":

                                                    if (comValue1.Length > comValue2.Length)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThanEqual":

                                                    if (comValue1.Length >= comValue2.Length)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThan":

                                                    if (comValue1.Length < comValue2.Length)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThanEqual":

                                                    if (comValue1.Length <= comValue2.Length)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "NotEqual":

                                                    if (comValue1 != comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                default:

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;

                                            }


                                        }

                                        if (bValid == false)
                                        {
                                            dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "c", sCanignore, strComparisonErrorMessage,
                                        strComparisonErrorToolTip, strValue);

                                        }
                                    }
                                }
                            }
                        }//End Try Comparison
                        catch
                        {
                            //

                        }


                        bool bMaxTime = false;

                        if (newRecord.DateTimeRecorded == null)
                            newRecord.DateTimeRecorded = DateTime.Now;

                        if (!RecordManager.IsTimeBetweenRecordOK((int)newRecord.TableID, -1, (DateTime)newRecord.DateTimeRecorded))
                        {
                            _strWarningResults = _strWarningResults + " WARNING: " + WarningMsg.MaxtimebetweenRecords + "!";

                            dtValidWarning.Rows.Add("DateTimeRecorded", "w", "no", "WARNING: Date Time Recorded - " + WarningMsg.MaxtimebetweenRecords + "!",
                                                "Max time between Records:" + _theTable.MaxTimeBetweenRecords.ToString() + " " + _theTable.MaxTimeBetweenRecordsUnit.ToString(), newRecord.DateTimeRecorded.ToString());

                            bMaxTime = true;

                        }



                        if (newRecord.EnteredBy == null)
                        {
                            newRecord.EnteredBy = _objUser.UserID;
                        }

                        //check duplicate
                        if (strUniqueColumnIDSys != "" || strUniqueColumnID2Sys != "")
                        {
                            string strUniqueColumnIDValue = "";
                            string strUniqueColumnID2Value = "";
                            if (strUniqueColumnIDSys != "")
                                strUniqueColumnIDValue = RecordManager.GetRecordValue(ref newRecord, strUniqueColumnIDSys);

                            if (strUniqueColumnID2Sys != "")
                                strUniqueColumnID2Value = RecordManager.GetRecordValue(ref newRecord, strUniqueColumnID2Sys);

                            if (RecordManager.ets_Record_IsDuplicate_Entry((int)newRecord.TableID, -1, strUniqueColumnIDSys, strUniqueColumnIDValue,
                                strUniqueColumnID2Sys, strUniqueColumnID2Value))
                            {
                                lblMsg.Text = "Duplicate Record!";
                                return false;
                            }

                        }



                        //check AutoCreateUser
                        if (_theTable.AddUserRecord != null && _theTable.AddUserUserColumnID != null && _theTable.AddUserPasswordColumnID != null)
                        {
                            Column theEmailColumn = RecordManager.ets_Column_Details((int)_theTable.AddUserUserColumnID);
                            if (theEmailColumn != null)
                            {
                                string strEmail = RecordManager.GetRecordValue(ref newRecord, theEmailColumn.SystemName);
                                DataTable dtUsers = Common.DataTableFromText("SELECT Email FROM [User] WHERE Email='" + strEmail + "'");
                                if (dtUsers.Rows.Count > 0 && strEmail.Trim() != "")
                                {
                                    lblMsg.Text = "A user has this email(" + strEmail + ") address, please try another email address.";
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('A user has this email(" + strEmail + ") address, please try another email address.');", true);
                                    return false;
                                }
                            }
                        }



                        PerformAllValidation(ref newRecord, ref dtValidWarning, true, false);

                        if (_strWarningResults.Length > 0)
                        {
                            newRecord.WarningResults = _strWarningResults.Trim();
                        }

                        if (_bShowExceedances && _strExceedanceResults.Length > 0)
                        {
                            newRecord.WarningResults = newRecord.WarningResults == "" ? _strExceedanceResults : newRecord.WarningResults + " " + _strExceedanceResults;
                        }

                        if (_strInValidResults.Length > 0)
                        {
                            newRecord.WarningResults = newRecord.WarningResults == "" ? _strInValidResults : newRecord.WarningResults + " " + _strInValidResults;
                        }

                        if (newRecord.WarningResults != null && newRecord.WarningResults != "")
                        {
                            //_lblWarningResults.Visible = true;
                            _lblWarningResultsValue.Text = newRecord.WarningResults.ToString();
                        }
                        else
                        {
                            _lblWarningResults.Visible = false;
                        }

                        DataTable dtValidWarningResult = GetIgnoreValidWarning(dtValidWarning, dtValidWarningOri);



                        if (dtValidWarningResult.Rows.Count > 0)
                        {
                            divValidWarningGrid.Visible = true;
                            gvValidWarningGrid.DataSource = dtValidWarning;
                            gvValidWarningGrid.DataBind();
                            return false;
                        }
                        else
                        {
                            divValidWarningGrid.Visible = false;
                        }


                        if (bPeformSave == false)
                            return false;





                        int iNewRecordID = RecordManager.ets_Record_Insert(newRecord);

                        _iRecordID = iNewRecordID;

                        _iNewRecordID = iNewRecordID;


                        //do client specific code
                        newRecord.RecordID = _iNewRecordID;

                        if (_theTable.AddRecordSP != "")
                            RecordManager.AddRecordSP(_theTable.AddRecordSP, iNewRecordID, null);

                        //if (newRecord.TableID == 1941 && _iParentRecordID != null && newRecord.V003!="")
                        //{
                        //    try
                        //    {
                        //        TheDatabase.Account24769_spAddNewMedication((int)_iParentRecordID,
                        //            _iNewRecordID);
                        //    }
                        //    catch
                        //    {
                        //        //
                        //    }
                        //}






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

                        _strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&Recordid=" + Cryptography.Encrypt(iNewRecordID.ToString());

                        //hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&Recordid=" + Cryptography.Encrypt(iNewRecordID.ToString()) + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&FromAdd=yes";
                        //now send emails

                        //Check SPDefaultValue

                        bool bNeedUpdate = false;
                        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                        {
                            if (_dtColumnsDetail.Rows[i]["SPDefaultValue"].ToString() != "")
                            {
                                try
                                {
                                    bNeedUpdate = true;
                                    string strSPDefaultValue = RecordManager.Column_SPDefaultValue(_dtColumnsDetail.Rows[i]["SPDefaultValue"].ToString(),
                                        (int)iNewRecordID, (int)_objUser.UserID, int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()));

                                    RecordManager.MakeTheRecord(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strSPDefaultValue);
                                }
                                catch
                                {
                                    //
                                }
                            }
                            //RecordManager.Column_SPDefaultValue(
                        }






                        if (bNeedUpdate)
                        {
                            RecordManager.ets_Record_Update(newRecord, null);
                        }


                        if (_theTable.SPSaveRecord != "")
                        {
                            try
                            {
                                RecordManager.Table_SPSaveRecord(_theTable.SPSaveRecord,
                                      (int)iNewRecordID, (int)_objUser.UserID);
                                bNeedUpdate = true;
                            }
                            catch
                            {
                                //
                            }

                        }


                        if (bNeedUpdate)
                            newRecord = RecordManager.ets_Record_Detail_Full(iNewRecordID); //refresh

                        if (bMaxTime)
                        {
                            string strTemp = "";

                            RecordManager.SendMaxTimeWanrningSMSandEmail((int)_theTable.TableID, newRecord.DateTimeRecorded.ToString(), _iSessionAccountID, ref strTemp, _strURL);
                            if (strTemp != "")
                            {
                                lblMsg.Text = lblMsg.Text + " " + strTemp;
                                bNeedUpdate = true;
                            }
                        }

                        PerformAllValidation(ref newRecord, ref dtValidWarning, false, true);
                        //if (_bShowExceedances && _bDataExceedance)
                        //{
                        //    for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                        //    {
                        //        strValue = "";
                        //        if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() == ""
                        //            && _dtColumnsDetail.Rows[i]["TableTableID"] == DBNull.Value)
                        //        {
                        //            if (_txtValue[i] != null)
                        //                strValue = _txtValue[i].Text;
                        //        }
                        //        else if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                        //            && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                        //            || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                        //        {
                        //            if (_ddlValue[i] != null && _ddlValue[i].SelectedIndex != null && _ddlValue[i].SelectedIndex != 0)
                        //            {
                        //                strValue = _ddlValue[i].Text;
                        //            }
                        //        }

                        //        if (_bShowExceedances && _dtColumnsDetail.Rows[i]["ValidationOnExceedance"] != DBNull.Value)
                        //        {

                        //            if (_dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                        //            {
                        //                if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString(), ref _strValidationError))
                        //                {
                        //                    string strTemp = "";
                        //                    RecordManager.BuildDataExceedanceSMSandEmail(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), strValue, newRecord.DateTimeRecorded.ToString(),
                        //                        ref strTemp, _iSessionAccountID, _strURL, ref _strExceedanceEmailFullBody, ref _strExceedanceSMSFullBody, ref _iExceedanceColumnCount);

                        //                }
                        //            }
                        //        }

                        //    }

                        //    for (int i = 0; i < _dtColumnsNotDetail.Rows.Count; i++)
                        //    {
                        //        if (_dtColumnsNotDetail.Rows[i]["ValidationOnExceedance"] != DBNull.Value)
                        //        {

                        //            if (_dtColumnsNotDetail.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                        //            {
                        //                if (RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()) != null)
                        //                    if (!UploadManager.IsDataValid(RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()), _dtColumnsNotDetail.Rows[i]["ValidationOnExceedance"].ToString(), ref _strValidationError))
                        //                    {

                        //                        string strTemp = "";
                        //                        RecordManager.BuildDataExceedanceSMSandEmail(int.Parse(_dtColumnsNotDetail.Rows[i]["ColumnID"].ToString()), RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()), newRecord.DateTimeRecorded.ToString(),
                        //                            ref strTemp, _iSessionAccountID, _strURL, ref _strExceedanceEmailFullBody, ref _strExceedanceSMSFullBody, ref _iExceedanceColumnCount);

                        //                    }
                        //            }
                        //        }
                        //    }


                        //}

                        //if (_bDataWarning)
                        //{
                        //    for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                        //    {
                        //        strValue = "";
                        //        if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() == ""
                        //            && _dtColumnsDetail.Rows[i]["TableTableID"] == DBNull.Value)
                        //        {
                        //            if (_txtValue[i] != null)
                        //                strValue = _txtValue[i].Text;
                        //        }
                        //        else if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                        //            && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                        //            || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                        //        {
                        //            if (_ddlValue[i]!=null && _ddlValue[i].SelectedIndex!=null &&  _ddlValue[i].SelectedIndex != 0)
                        //            {
                        //                strValue = _ddlValue[i].Text;
                        //            }
                        //        }

                        //        if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"] != DBNull.Value)
                        //        {

                        //            if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                        //            {
                        //                if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), ref _strValidationError))
                        //                {

                        //                    string strTemp = "";
                        //                    //RecordManager.SendDataWanrningSMSandEmail(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), strValue, newRecord.DateTimeRecorded.ToString(), ref strTemp, _iSessionAccountID, _strURL);
                        //                    RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), strValue, newRecord.DateTimeRecorded.ToString(),
                        //                        ref strTemp, _iSessionAccountID, _strURL, ref _strWarningEmailFullBody, ref _strWarningSMSFullBody, ref _iWarningColumnCount);

                        //                }
                        //                else
                        //                {

                        //                }
                        //            }
                        //        }

                        //    }



                        //    for (int i = 0; i < _dtColumnsNotDetail.Rows.Count; i++)
                        //    {
                        //        if (_dtColumnsNotDetail.Rows[i]["ValidationOnWarning"] != DBNull.Value)
                        //        {

                        //            if (_dtColumnsNotDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                        //            {
                        //                if (RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()) != null)
                        //                    if (!UploadManager.IsDataValid(RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()), _dtColumnsNotDetail.Rows[i]["ValidationOnWarning"].ToString(), ref _strValidationError))
                        //                    {

                        //                        string strTemp = "";
                        //                        //RecordManager.SendDataWanrningSMSandEmail(int.Parse(_dtColumnsNotDetail.Rows[i]["ColumnID"].ToString()), RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()), newRecord.DateTimeRecorded.ToString(), ref strTemp, _iSessionAccountID, _strURL);

                        //                        RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsNotDetail.Rows[i]["ColumnID"].ToString()), RecordManager.GetRecordValue(ref newRecord, _dtColumnsNotDetail.Rows[i]["SystemName"].ToString()), newRecord.DateTimeRecorded.ToString(),
                        //                            ref strTemp, _iSessionAccountID, _strURL, ref _strWarningEmailFullBody, ref _strWarningSMSFullBody, ref _iWarningColumnCount);

                        //                    }
                        //            }
                        //        }
                        //    }
                        //}





                        //now check Records exceeded

                        //if (SecurityManager.IsRecordsExceeded(_iSessionAccountID))
                        //{
                        //    Session["DoNotAllow"] = "true";
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Sorry you have reached the limit of your account.');window.location.href='" + "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountTypeChange.aspx?type=renew" + "'", true);

                        //    return false;
                        //}


                        //lets check add data notification

                        AddDataNotification(newRecord);


                        //


                        break;

                    case "view":


                        break;

                    case "edit":
                        //Record editRecord = new Record();
                        //editRecord.RecordID = _iRecordID;
                        //editRecord.DateTimeRecorded = DateTime.Now;


                        strValue = "";

                        Record editRecord = RecordManager.ets_Record_Detail_Full(_iRecordID);

                        //Record originalRecord = RecordManager.ets_Record_Detail_Full(_iRecordID);

                        //editRecord.ChangeReason = txtReasonForChange.Text.Trim();



                        _strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&Recordid=" + Cryptography.Encrypt(_iRecordID.ToString());

                        for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                        {
                            //bEachColumnExceedance = false;

                            //if (i == _iLocationIndex)
                            //{


                            //    RecordManager.MakeTheRecord(ref editRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _ddlLocation.SelectedValue);

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
                                    strDateTime = strDateTime.Replace("  ", " ");
                                }

                                //RecordManager.MakeTheRecord(ref newRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _txtValue[i].Text);
                                RecordManager.MakeTheRecord(ref editRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strDateTime);
                            }
                            else
                            {




                                //perform caculation here
                                //if (_dtColumnsDetail.Rows[i]["Calculation"] != DBNull.Value)
                                //{
                                //    if (_dtColumnsDetail.Rows[i]["Calculation"].ToString().Length > 0)
                                //    {
                                //        //replace params by value
                                //        string strFormula = _dtColumnsDetail.Rows[i]["Calculation"].ToString().ToLower();

                                //        for (int j = 0; j < _dtColumnsDetail.Rows.Count; j++)
                                //        {

                                //            if (_radioList[j] != null)
                                //            {
                                //                if (_radioList[j].SelectedItem != null)
                                //                    strFormula = strFormula.Replace("[" + _dtColumnsDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_radioList[j].SelectedValue));
                                //            }
                                //            if (_ddlValue[j] != null)
                                //            {
                                //                if (_dtColumnsDetail.Rows[j]["ColumnType"].ToString() == "dropdown"
                                //                        && (_dtColumnsDetail.Rows[j]["DropDownType"].ToString() == "table"
                                //                        || _dtColumnsDetail.Rows[j]["DropDownType"].ToString() == "tabledd")
                                //                        && _dtColumnsDetail.Rows[j]["TableTableID"] != DBNull.Value
                                //                         && _dtColumnsDetail.Rows[j]["LinkedParentColumnID"] != DBNull.Value
                                //                        && _dtColumnsDetail.Rows[j]["DisplayColumn"].ToString() != ""
                                //                        )
                                //                {
                                //                    if (_ddlValue[j].SelectedItem != null)
                                //                        strFormula = strFormula.Replace("[" + _dtColumnsDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_ddlValue[j].SelectedValue));
                                //                }

                                //            }

                                //            if (_txtValue[j] != null)
                                //            {

                                //                if ((_dtColumnsDetail.Rows[j]["ColumnType"].ToString() == "datetime"
                                //                    || _dtColumnsDetail.Rows[j]["ColumnType"].ToString() == "date"
                                //                    || _dtColumnsDetail.Rows[j]["ColumnType"].ToString() == "time")
                                //                    && (_txtValue[j].Text.IndexOf("/") > -1
                                //                    || _txtValue[j].Text.IndexOf(":") > -1)
                                //                    && _dtColumnsDetail.Rows[i]["DateCalculationType"] != DBNull.Value)
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
                                //                    strFormula = strFormula.Replace("[" + _dtColumnsDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", dtValue.Ticks.ToString());
                                //                }
                                //                else
                                //                {
                                //                    if (_dtColumnsDetail.Rows[j]["IgnoreSymbols"].ToString().ToLower() == "true")
                                //                    {
                                //                        strFormula = strFormula.Replace("[" + _dtColumnsDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", Common.IgnoreSymbols(_txtValue[j].Text));
                                //                    }
                                //                    else
                                //                    {
                                //                        strFormula = strFormula.Replace("[" + _dtColumnsDetail.Rows[j]["DisplayName"].ToString().ToLower() + "]", _txtValue[j].Text);
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
                                //                    && _dtColumnsDetail.Rows[i]["DateCalculationType"] != DBNull.Value)
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
                                //            if (_dtColumnsDetail.Rows[i]["DateCalculationType"] != DBNull.Value)
                                //            {

                                //                double lResult = double.Parse(strResult);

                                //                switch (_dtColumnsDetail.Rows[i]["DateCalculationType"].ToString())
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



                                if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value
                                     && _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
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
                                        strDateTime = strDateTime.Replace("  ", " ");
                                    }
                                    strValue = strDateTime;

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                                {
                                    string strDateTime = "";
                                    if (_txtValue[i].Text.Trim() == "")
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
                                        strUniqueName = Common.GetValidFileName(strUniqueName);
                                        string strPath = _strFilesPhisicalPath + strFolder + "\\" + strUniqueName;

                                        _fuValue2[i].SaveAs(strPath);
                                        _hfValue[i].Value = strUniqueName;
                                    }

                                    strValue = _hfValue[i].Value;
                                }

                                //if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "slider")
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

                                }

                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown"
                                    && _dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                                    && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                                    || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                                {
                                    if (_ddlValue[i] != null && _ddlValue[i].SelectedIndex != null && _ddlValue[i].SelectedIndex != 0)
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
                                            _hfValue3[i].Value = "3";

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
                                    if ((newLocationColumn.Latitude != null && newLocationColumn.Longitude != null) || !string.IsNullOrEmpty(newLocationColumn.Address))
                                    {
                                        strValue = newLocationColumn.GetJSONString();
                                        if (_dtColumnsDetail.Rows[i]["ShowTotal"] != DBNull.Value && _dtColumnsDetail.Rows[i]["ShowTotal"].ToString().ToLower() == "true"
                                           && string.IsNullOrEmpty(newLocationColumn.Address))
                                        {
                                            strValue = "";
                                        }

                                        if (_dtColumnsDetail.Rows[i]["IsRound"] != DBNull.Value && _dtColumnsDetail.Rows[i]["IsRound"].ToString().ToLower() == "true"
                                            && (newLocationColumn.Latitude == null || newLocationColumn.Longitude == null))
                                        {
                                            strValue = "";
                                        }
                                    }
                                    else
                                    {
                                        strValue = "";
                                    }

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
                                            if (_txtValue[i].Text.Trim() == "")
                                                _hfValue[i].Value = "";

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


                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation"
                                                && _dtColumnsDetail.Rows[i]["Calculation"] != DBNull.Value
                                && _txtValue[i] != null)
                                {
                                    strValue = "";
                                    string strTempValue = "";
                                    //string strOrginalValue = "";

                                    if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                                    && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "d")
                                    {
                                        //datetime calculation
                                        string strCalculation = _dtColumnsDetail.Rows[i]["Calculation"].ToString();
                                        try
                                        {
                                            strTempValue = TheDatabaseS.GetDateCalculationResult(ref _dtColumnsAll, strCalculation, null, editRecord, _iParentRecordID,
                                           _dtColumnsDetail.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString(),
                                           null, _theTable, _bCheckIgnoreMidnight);

                                            strValue = strTempValue;
                                        }
                                        catch
                                        {
                                            //
                                        }

                                    }
                                    else if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                                    && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "t")
                                    {
                                        //text calculation
                                        try
                                        {
                                            string strFormula = Common.GetCalculationSystemNameOnly(_dtColumnsDetail.Rows[i]["Calculation"].ToString(), (int)_theTable.TableID);

                                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()));
                                            strTempValue = TheDatabaseS.GetTextCalculationResult(ref _dtColumnsAll, strFormula, null, editRecord, _iParentRecordID, null, _theTable, theColumn);
                                            strValue = strTempValue;
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
                                            string strFormula = Common.GetCalculationSystemNameOnly(_dtColumnsDetail.Rows[i]["Calculation"].ToString(), (int)_theTable.TableID);

                                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()));
                                            strTempValue = TheDatabaseS.GetCalculationResult(ref _dtColumnsAll, strFormula, null, editRecord, _iParentRecordID, null, _theTable, theColumn);
                                            strValue = strTempValue;
                                            //if (_dtColumnsDetail.Rows[i]["RoundNumber"] != DBNull.Value)
                                            //{
                                            //    strValue = Math.Round(double.Parse(Common.IgnoreSymbols(strValue)), int.Parse(_dtColumnsDetail.Rows[i]["RoundNumber"].ToString())).ToString("N" + _dtColumnsDetail.Rows[i]["RoundNumber"].ToString());
                                            //}
                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }

                                    _txtValue[i].Text = strValue;

                                }



                                RecordManager.MakeTheRecord(ref editRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), strValue);


                                if (strValue.Length > 0)
                                {
                                    //if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"] == DBNull.Value)
                                    //{
                                    //    //do nothing
                                    //}
                                    //else
                                    //{
                                    //    if (_dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString().Length > 0)
                                    //    {
                                    //        if (UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString(), ref _strValidationError))
                                    //        {
                                    //            //strValidationResults = strValidationResults + "\n" + _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + " - " + "VALID";
                                    //        }
                                    //        else
                                    //        {
                                    //            //lblMsg.Text = "Invalid data - " + _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString();

                                    //            dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "i", "no", "Invalid data - " + _dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString(),
                                    //           _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString(), strValue);



                                    //            if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() == ""
                                    //                && _dtColumnsDetail.Rows[i]["TableTableID"] == DBNull.Value)
                                    //            {
                                    //                if(_txtValue[i]!=null)
                                    //                    _txtValue[i].Focus();
                                    //                //_txtValue[i].ToolTip = _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString();
                                    //            }
                                    //            else if (_dtColumnsDetail.Rows[i]["DropdownValues"].ToString() != ""
                                    //               && (_dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "values"
                                    //                || _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "value_text"))
                                    //            {
                                    //                if( _ddlValue[i]!=null)
                                    //                    _ddlValue[i].Focus();
                                    //                //_ddlValue[i].ToolTip = _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString();
                                    //            }

                                    //            //lblMsg.ToolTip = _dtColumnsDetail.Rows[i]["ValidationOnEntry"].ToString();

                                    //            //return false;
                                    //        }
                                    //    }
                                    //}


                                    //if (_bShowExceedances && _dtColumnsDetail.Rows[i]["ValidationOnExceedance"] != DBNull.Value)
                                    //{
                                    //    if (_dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString().Length > 0)
                                    //    {
                                    //        if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString(), ref _strValidationError))
                                    //        {
                                    //            _strExceedanceResults = _strExceedanceResults + " EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";

                                    //            dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "e", "yes", "EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.",
                                    //     _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString(), strValue);
                                    //            bEachColumnExceedance = true;
                                    //            string strTemp = "";
                                    //            RecordManager.BuildDataExceedanceSMSandEmail(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), strValue, editRecord.DateTimeRecorded.ToString(),
                                    //                ref strTemp, _iSessionAccountID, _strURL, ref _strExceedanceEmailFullBody, ref _strExceedanceSMSFullBody, ref _iExceedanceColumnCount);

                                    //        }
                                    //    }
                                    //}

                                    //if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"] != DBNull.Value && bEachColumnExceedance==false)
                                    //{
                                    //   if (_dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString().Length > 0)
                                    //    {
                                    //        if (!UploadManager.IsDataValid(strValue, _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), ref _strValidationError))
                                    //        {
                                    //            _strWarningResults = _strWarningResults + " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.";

                                    //            dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "w", "no", "WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.",
                                    //        _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), strValue);

                                    //            string strTemp = "";                                                 

                                    //            RecordManager.BuildDataWanrningSMSandEmail(int.Parse(_dtColumnsDetail.Rows[i]["ColumnID"].ToString()), strValue, editRecord.DateTimeRecorded.ToString(),
                                    //                ref strTemp, _iSessionAccountID, _strURL, ref _strWarningEmailFullBody, ref _strWarningSMSFullBody, ref _iWarningColumnCount);

                                    //        }                                             
                                    //    }
                                    //}




                                    //check SD
                                    if (bool.Parse(_dtColumnsDetail.Rows[i]["CheckUnlikelyValue"].ToString()))
                                    {
                                        int? iCount = RecordManager.ets_Table_GetCount((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _iRecordID);
                                        if (iCount != null)
                                        {
                                            if (iCount >= Common.MinSTDEVRecords)
                                            {
                                                string strRecordedate;
                                                if (_dtColumnsDetail.Rows[i]["IgnoreSymbols"].ToString().ToLower() == "true")
                                                {
                                                    strRecordedate = Common.IgnoreSymbols(strValue);
                                                }
                                                else
                                                {
                                                    strRecordedate = strValue;
                                                }

                                                double? dAVG = RecordManager.ets_Table_GetAVG((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _iRecordID);

                                                double? dSTDEV = RecordManager.ets_Table_GetSTDEV((int)_theTable.TableID, _dtColumnsDetail.Rows[i]["SystemName"].ToString(), _iRecordID);

                                                double dRecordedate = double.Parse(strRecordedate);
                                                if (dAVG != null && dSTDEV != null)
                                                {
                                                    dSTDEV = dSTDEV * 3;
                                                    if (dRecordedate > (dAVG + dSTDEV) || dRecordedate < (dAVG - dSTDEV))
                                                    {
                                                        //deviation happaned
                                                        _strWarningResults = _strWarningResults + " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.";
                                                        dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "w", "no", "WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Unlikely data – outside 3 standard deviations.",
                                                    "CheckUnlikelyValue", strRecordedate.ToString());
                                                    }

                                                }
                                            }
                                        }

                                    }
                                    /// 

                                }//
                            }
                        }









                        editRecord.TableID = int.Parse(_qsTableID);

                        bMaxTime = false;

                        if (!RecordManager.IsTimeBetweenRecordOK((int)editRecord.TableID, (int)editRecord.RecordID, (DateTime)editRecord.DateTimeRecorded))
                        {
                            bMaxTime = true;
                            _strWarningResults = _strWarningResults + " WARNING: " + WarningMsg.MaxtimebetweenRecords + "!";


                            dtValidWarning.Rows.Add("DateTimeRecorded", "w", "no", "WARNING: Date Time Recorded - " + WarningMsg.MaxtimebetweenRecords + "!",
                                                "Max time between Records:" + _theTable.MaxTimeBetweenRecords.ToString() + " " + _theTable.MaxTimeBetweenRecordsUnit.ToString(), editRecord.DateTimeRecorded.ToString());






                            //RecordManager.SendMaxTimeWanrningSMSandEmail((int)_theTable.TableID, editRecord.DateTimeRecorded.ToString(), _iSessionAccountID, ref strTemp, _strURL);



                            //if (strTemp != "")
                            //{
                            //    //lblMsg.Text = lblMsg.Text + " " + strTemp;
                            //}
                        }




                        if (editRecord.EnteredBy == null)
                        {
                            editRecord.EnteredBy = _objUser.UserID;
                        }

                        editRecord.LastUpdatedUserID = _objUser.UserID;


                        //check duplicate
                        //if ((bool)_theTable.IsRecordDateUnique)
                        //{

                        //    if (RecordManager.ets_Record_IsDuplicate_Entry((int)editRecord.TableID, (DateTime)editRecord.DateTimeRecorded, (int)editRecord.RecordID))
                        //    {
                        //        lblMsg.Text = "Duplicate Record!";
                        //        return false;
                        //    }

                        //}

                        if (strUniqueColumnIDSys != "" || strUniqueColumnID2Sys != "")
                        {
                            string strUniqueColumnIDValue = "";
                            string strUniqueColumnID2Value = "";
                            if (strUniqueColumnIDSys != "")
                                strUniqueColumnIDValue = RecordManager.GetRecordValue(ref editRecord, strUniqueColumnIDSys);

                            if (strUniqueColumnID2Sys != "")
                                strUniqueColumnID2Value = RecordManager.GetRecordValue(ref editRecord, strUniqueColumnID2Sys);

                            if (RecordManager.ets_Record_IsDuplicate_Entry((int)editRecord.TableID, (int)editRecord.RecordID, strUniqueColumnIDSys, strUniqueColumnIDValue,
                                strUniqueColumnID2Sys, strUniqueColumnID2Value))
                            {
                                lblMsg.Text = "Duplicate Record!";
                                return false;
                            }

                        }






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




                        try
                        {
                            for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                            {
                                string strValue1 = "";
                                strValue1 = RecordManager.GetRecordValue(ref editRecord, _dtColumnsDetail.Rows[i]["SystemName"].ToString());

                                if (_dtColumnsDetail.Rows[i]["CompareColumnID"] != DBNull.Value
                    && _dtColumnsDetail.Rows[i]["CompareOperator"] != DBNull.Value && strValue1.Length > 0)
                                {
                                    bool bValidationCanIgnore = false;
                                    string sCanignore = "no";
                                    if (_dtColumnsDetail.Rows[i]["ValidationCanIgnore"] != DBNull.Value)
                                    {
                                        if ((bool)_dtColumnsDetail.Rows[i]["ValidationCanIgnore"])
                                        {
                                            bValidationCanIgnore = true;
                                            sCanignore = "yes";
                                        }
                                    }

                                    Column theComparisonColumn = RecordManager.ets_Column_Details(int.Parse(_dtColumnsDetail.Rows[i]["CompareColumnID"].ToString()));



                                    string strValue2 = "";
                                    if (theComparisonColumn.TableID.ToString() != "" && theComparisonColumn.TableID.ToString() == _qsTableID)
                                    {
                                        strValue2 = RecordManager.GetRecordValue(ref editRecord, theComparisonColumn.SystemName);
                                    }
                                    if (theComparisonColumn.TableID.ToString() != "" && theComparisonColumn.TableID.ToString() != _qsTableID && _iParentRecordID != null)
                                    {
                                        strValue2 = TheDatabaseS.spGetValueFromRelatedTable((int)_iParentRecordID, (int)theComparisonColumn.TableID, theComparisonColumn.SystemName);

                                    }

                                    string strComparisonErrorMessage = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " " + Common.CompareOperatorErrorMsg(_dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " " + theComparisonColumn.DisplayName;

                                    string strComparisonErrorToolTip = "Comparison error:" + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + "(" + strValue1 + ")" + " "
                                        + Common.CompareOperatorErrorMsg(_dtColumnsDetail.Rows[i]["CompareOperator"].ToString()) + " " + theComparisonColumn.DisplayName + "(" + strValue2 + ")";

                                    if (strValue2.Length > 0)//bValidationCanIgnore &&
                                    {
                                        //need server side validation





                                        bool bValid = false;
                                        if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime"
                       || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time"
                                            || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "date")
                                        {
                                            DateTime comValue1 = DateTime.Today;
                                            DateTime comValue2 = DateTime.Today;
                                            try
                                            {
                                                comValue1 = DateTime.Parse(strValue1);
                                                comValue2 = DateTime.Parse(strValue2);

                                            }
                                            catch
                                            {

                                            }

                                            try
                                            {
                                                if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "time")
                                                {
                                                    comValue1 = DateTime.Parse(comValue1.Hour.ToString() + ":" + comValue1.Minute.ToString());
                                                    comValue2 = DateTime.Parse(comValue2.Hour.ToString() + ":" + comValue2.Minute.ToString());

                                                }
                                            }
                                            catch
                                            {

                                            }
                                            switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                                            {
                                                case "Equal":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "DataTypeCheck":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThan":

                                                    if (comValue1 > comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThanEqual":

                                                    if (comValue1 >= comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThan":

                                                    if (comValue1 < comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThanEqual":

                                                    if (comValue1 <= comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "NotEqual":

                                                    if (comValue1 != comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                default:

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;

                                            }


                                        }
                                        else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "number")
                                        {
                                            double comValue1 = 0;
                                            double comValue2 = 0;
                                            try
                                            {
                                                comValue1 = double.Parse(strValue1);
                                                comValue2 = double.Parse(strValue2);

                                            }
                                            catch
                                            {

                                            }


                                            switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                                            {
                                                case "Equal":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "DataTypeCheck":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThan":

                                                    if (comValue1 > comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThanEqual":

                                                    if (comValue1 >= comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThan":

                                                    if (comValue1 < comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThanEqual":

                                                    if (comValue1 <= comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "NotEqual":

                                                    if (comValue1 != comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                default:

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;

                                            }

                                        }
                                        else
                                        {
                                            string comValue1 = strValue1;
                                            string comValue2 = strValue2;

                                            switch (_dtColumnsDetail.Rows[i]["CompareOperator"].ToString())
                                            {
                                                case "Equal":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "DataTypeCheck":

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThan":

                                                    if (comValue1.Length > comValue2.Length)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "GreaterThanEqual":

                                                    if (comValue1.Length >= comValue2.Length)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThan":

                                                    if (comValue1.Length < comValue2.Length)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "LessThanEqual":

                                                    if (comValue1.Length <= comValue2.Length)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                case "NotEqual":

                                                    if (comValue1 != comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;
                                                default:

                                                    if (comValue1 == comValue2)
                                                    {
                                                        bValid = true;
                                                    }
                                                    break;

                                            }


                                        }


                                        if (bValid == false)
                                        {
                                            dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "c", sCanignore, strComparisonErrorMessage,
                                        strComparisonErrorToolTip, strValue);

                                        }

                                    }


                                }
                            }
                        }
                        catch
                        {


                        }

                        PerformAllValidation(ref editRecord, ref dtValidWarning, true, false);


                        editRecord.WarningResults = _strWarningResults.Trim();


                        if (_bShowExceedances && _strExceedanceResults.Length > 0)
                        {
                            editRecord.WarningResults = editRecord.WarningResults == "" ? _strExceedanceResults : editRecord.WarningResults + " " + _strExceedanceResults;
                        }

                        if (_strInValidResults.Length > 0)
                        {
                            editRecord.WarningResults = editRecord.WarningResults == "" ? _strInValidResults : editRecord.WarningResults + " " + _strInValidResults;
                        }


                        if (editRecord.WarningResults != "" && editRecord.WarningResults != null)
                        {
                            //_lblWarningResults.Visible = true;
                            _lblWarningResultsValue.Text = editRecord.WarningResults.ToString();
                        }
                        else
                        {
                            _lblWarningResults.Visible = false;
                        }



                        DataTable dtValidWarningResult2 = GetIgnoreValidWarning(dtValidWarning, dtValidWarningOri);



                        if (dtValidWarningResult2.Rows.Count > 0)
                        {
                            divValidWarningGrid.Visible = true;
                            gvValidWarningGrid.DataSource = dtValidWarning;
                            gvValidWarningGrid.DataBind();
                            return false;
                        }
                        else
                        {
                            divValidWarningGrid.Visible = false;
                        }

                        if (bPeformSave == false)
                            return false;


                        int iIsUpdated = RecordManager.ets_Record_Update(editRecord, true);

                        if (bMaxTime)
                        {
                            string strTemp = "";
                            RecordManager.SendMaxTimeWanrningSMSandEmail((int)_theTable.TableID, editRecord.DateTimeRecorded.ToString(), _iSessionAccountID, ref strTemp, _strURL);
                        }

                        //for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
                        //{
                        //    if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "calculation"
                        //                        && _dtColumnsDetail.Rows[i]["Calculation"] != DBNull.Value
                        //        && _dtColumnsDetail.Rows[i]["ValidationOnWarning"]!=DBNull.Value
                        //        && _txtValue[i]!=null)
                        //    {


                        //        string strTempValue = "";
                        //        string strOrginalValue = "";
                        //        bool bDateCal = false;
                        //        if (_dtColumnsDetail.Rows[i]["TextType"] != DBNull.Value
                        //            && _dtColumnsDetail.Rows[i]["TextType"].ToString().ToLower() == "d")
                        //        {
                        //            bDateCal = true;
                        //            string strCalculation = _dtColumnsDetail.Rows[i]["Calculation"].ToString();

                        //            try
                        //            {
                        //                strTempValue = TheDatabaseS.GetDateCalculationResult(_dtColumnsAll, strCalculation, _iRecordID,editRecord, _iParentRecordID,
                        //                    _dtColumnsDetail.Rows[i]["DateCalculationType"] == DBNull.Value ? "" : _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString());

                        //                strOrginalValue = strTempValue;
                        //                if (strTempValue.IndexOf(" ") > -1)
                        //                {
                        //                    strTempValue = strTempValue.Substring(0, strTempValue.IndexOf(" "));
                        //                    strTempValue = strTempValue.Trim();
                        //                    strTempValue = Common.IgnoreSymbols(strTempValue);
                        //                }
                        //            }
                        //            catch
                        //            {
                        //                //
                        //            }



                        //        }
                        //        else
                        //        {
                        //            string strFormula = TheDatabaseS.GetCalculationFormula(int.Parse(_qsTableID), _dtColumnsDetail.Rows[i]["Calculation"].ToString());
                        //            strTempValue = TheDatabaseS.GetCalculationResult(_dtColumnsAll, strFormula, _iRecordID, editRecord, _iParentRecordID);
                        //            strOrginalValue = strTempValue;
                        //        }                              



                        //        if (strTempValue.Length > 0)
                        //        {
                        //            if (!UploadManager.IsDataValid(strTempValue, _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), ref strValidationError, true))
                        //            {
                        //                strWarningResults = " WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value (" + strTempValue + ") outside accepted range";

                        //                dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "w", "no", "WARNING: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.",
                        //                       _dtColumnsDetail.Rows[i]["ValidationOnWarning"].ToString(), strTempValue);


                        //                lblMsg.Text = lblMsg.Text + " " + strWarningResults;
                        //                _txtValue[i].Text = strOrginalValue;
                        //                return false;
                        //            }

                        //            if(_bShowExceedances)
                        //            {
                        //                if (!UploadManager.IsDataValid(strTempValue, _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString(), ref strValidationError, true))
                        //                {
                        //                    strExceedanceResults = " EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value (" + strTempValue + ") outside accepted range";

                        //                    dtValidWarning.Rows.Add(_dtColumnsDetail.Rows[i]["ColumnID"].ToString(), "e", "yes", "EXCEEDANCE: " + _dtColumnsDetail.Rows[i]["DisplayName"].ToString() + " – Value outside accepted range.",
                        //                     _dtColumnsDetail.Rows[i]["ValidationOnExceedance"].ToString(), strTempValue);

                        //                    lblMsg.Text = lblMsg.Text + " " + strExceedanceResults;
                        //                    _txtValue[i].Text = strOrginalValue;
                        //                    return false;
                        //                }
                        //            }
                        //        }                                                              


                        //    }
                        //}


                        if (_theTable.SPSaveRecord != "")
                        {
                            try
                            {
                                RecordManager.Table_SPSaveRecord(_theTable.SPSaveRecord,
                                      (int)editRecord.RecordID, (int)_objUser.UserID);
                            }
                            catch
                            {
                                //
                            }

                        }

                        RecordManager.ets_Record_Avg_ForARecordID((int)editRecord.RecordID);



                        //HB Start


                        //HB End


                        //let's check Default Sync

                        //if (_iParentRecordID != null)
                        //{
                        try
                        {
                            DataTable dtDefaultSync = Common.DataTableFromText(@"SELECT ColumnID,DefaultColumnID FROM [Column] WHERE DefaultColumnID IN 
                                (SELECT ColumnID FROM [Column] WHERE TableID=" + editRecord.TableID.ToString() + @")
                                AND DefaultUpdateValues=1");


                            if (dtDefaultSync.Rows.Count > 0)
                            {
                                //we need to perform Sync
                                editRecord = RecordManager.ets_Record_Detail_Full((int)editRecord.RecordID);//refresh the record
                                foreach (DataRow dr in dtDefaultSync.Rows)
                                {
                                    Column theRelatedColumn = RecordManager.ets_Column_Details(int.Parse(dr["ColumnID"].ToString()));
                                    Column thisDefaultColumn = RecordManager.ets_Column_Details(int.Parse(dr["DefaultColumnID"].ToString()));
                                    string strNewDefaultValue = RecordManager.GetRecordValue(ref editRecord, thisDefaultColumn.SystemName);


                                    int? iParentRecordID = _iParentRecordID;

                                    if (_iParentRecordID == null)
                                    {

                                        string strParentSys = Common.GetValueFromSQL(@"SELECT S2.SystemName FROM [Column] S2 INNER JOIN [Column] S
                                                                        ON S2.TableTableID=S.TableTableID
                                                                        WHERE S2.TableID=" + editRecord.TableID.ToString() + " AND S.TableID =" + theRelatedColumn.TableID.ToString());

                                        if (strParentSys != "")
                                        {
                                            string strParentID = RecordManager.GetRecordValue(ref editRecord, strParentSys);
                                            if (strParentID != "")
                                            {
                                                int iTempPID = 0;

                                                if (int.TryParse(strParentID, out iTempPID))
                                                {
                                                    iParentRecordID = iTempPID;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string strChildSys = Common.GetValueFromSQL(@"SELECT C.SystemName FROM [Column] C INNER JOIN [Column] P
                                                                        ON C.TableTableID=P.TableID
                                                                        WHERE P.TableID=" + editRecord.TableID.ToString() + " AND C.TableID =" + theRelatedColumn.TableID.ToString());

                                            if (strChildSys != "")
                                            {
                                                iParentRecordID = (int)editRecord.RecordID;
                                            }
                                        }

                                    }

                                    if (iParentRecordID != null)
                                        TheDatabaseS.spUpdateRelatedTable((int)iParentRecordID,
                                            (int)theRelatedColumn.TableID, theRelatedColumn.SystemName, strNewDefaultValue);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            //
                            ErrorLog theErrorLog = new ErrorLog(null, "Default Sync", ex.Message, ex.StackTrace, DateTime.Now, Request.RawUrl);
                            SystemData.ErrorLog_Insert(theErrorLog);
                        }
                        //}

                        editRecord = RecordManager.ets_Record_Detail_Full((int)editRecord.RecordID);
                        PerformAllValidation(ref editRecord, ref dtValidWarning, false, true);

                        break;

                    default:
                        //?
                        break;
                }


                if (_iWarningColumnCount > 0)
                {
                    string strError = "";

                    string strRecordID = "";
                    if (_theRecord != null)
                    {
                        strRecordID = _theRecord.RecordID.ToString();
                    }
                    else
                    {
                        strRecordID = _iNewRecordID.ToString();
                    }

                    RecordManager.SendDataWanrningSMSandEmailBatch(int.Parse(_qsTableID), ref strError, _iSessionAccountID,
                        _strWarningEmailFullBody, _strWarningSMSFullBody, _iWarningColumnCount, int.Parse(strRecordID));

                    if (strError != "")
                    {
                        lblMsg.Text = strError;
                        //return false;
                    }
                }

                if (_iExceedanceColumnCount > 0)
                {
                    string strError = "";

                    string strRecordID = "";
                    if (_theRecord != null)
                    {
                        strRecordID = _theRecord.RecordID.ToString();
                    }
                    else
                    {
                        strRecordID = _iNewRecordID.ToString();
                    }

                    RecordManager.SendDataExceedanceSMSandEmailBatch(int.Parse(_qsTableID), ref strError, _iSessionAccountID,
                        _strExceedanceEmailFullBody, _strExceedanceSMSFullBody, _iExceedanceColumnCount, int.Parse(strRecordID));

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
            //ViewState["ok"] = "no";
            return true;
            //Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            //ViewState["ok"] = "no";
            lblMsg.Text = ex.Message;

            ErrorLog theErrorLog = new ErrorLog(null, "Perform Save", ex.Message, ex.StackTrace, DateTime.Now, Request.RawUrl);
            SystemData.ErrorLog_Insert(theErrorLog);

            return false;
        }

    }


    protected void CL_Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheChangedLogGrid(_gvCL_Pager.StartIndex, _gvCL_Pager._gridView.PageSize);
    }

    protected void btnReloadMe_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl, false);
        return;
    }


    protected void CL_Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvCL_Pager.ExportFileName = "Record Change Log ";
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
        "attachment;filename=RecordChangedLog.csv");
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


    protected void BindTheChangedLogGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
            int iTN = 0;

            //gvChangedLog.DataSource = RecordManager.ets_Record_Changes_Select(
            //        (int)_iRecordID,int.Parse(_qsTableID), iStartIndex, iMaxRows, ref  iTN, ref _iCLColumnCount);

            getLastUpdatedInfo();

            gvChangedLog.DataSource = RecordManager.Record_Audit_Summary(
                   (int)_iRecordID, iStartIndex, iMaxRows, ref  iTN);

            gvChangedLog.VirtualItemCount = iTN;

            _iCLColumnCount = 4;

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

            if (_theTable.ReasonChangeType == "none" || _theTable.ReasonChangeType == "")
            {
                gvChangedLog.Columns[4].Visible = false;
            }


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Record Detail Change Log", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
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

                HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");
                DateTime dtUpdateDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "DateAdded"));
                hlView.NavigateUrl = "AuditDetail.aspx?UpdatedDate=" + Server.UrlEncode(dtUpdateDate.ToString("yyyy-MM-dd HH:mm:ss.fff")) + "&RecordID=" + _iRecordID.ToString();

                string strColumnList = DataBinder.Eval(e.Row.DataItem, "ColumnList").ToString();
                string[] arrColumnList = strColumnList.Split(',');

                Label lblColumnList = (Label)e.Row.FindControl("lblColumnList");

                if (arrColumnList.Length > 3)
                {
                    //get first 3 names
                    int i = 0;
                    string strThreeeColumns = "";
                    foreach (string aColumn in arrColumnList)
                    {
                        if (i == 0)
                            strThreeeColumns = aColumn;

                        if (i == 1)
                            strThreeeColumns = strThreeeColumns + "," + aColumn;

                        if (i == 2)
                            strThreeeColumns = strThreeeColumns + " and " + aColumn;

                        i = i + 1;

                        if (i == 3)
                            break;
                    }

                    lblColumnList.Text = arrColumnList.Length + " fields including " + strThreeeColumns;

                }
                else
                {
                    lblColumnList.Text = strColumnList;
                }
            }

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }





}



public class DynamicTemplateField : ITemplate
{

    int iCount;
    public DynamicTemplateField(int i)
    {
        iCount = i;
    }

    public void InstantiateIn(Control container)
    {
        //define the control to be added
        Label lbl = new Label();
        lbl.ID = "lbl" + iCount.ToString();

        HiddenField hf = new HiddenField();
        hf.ID = "hf" + iCount.ToString();

        HiddenField hfC = new HiddenField();
        hfC.ID = "hfC" + iCount.ToString();

        container.Controls.Add(lbl);
        container.Controls.Add(hf);

        container.Controls.Add(hfC);

        container.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp"));



    }
}