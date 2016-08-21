using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AjaxControlToolkit;

public partial class Pages_Record_FormSetWizard : SecurePage
{
    private Pages_UserControl_DetailEdit[] _ctDetailEdit;
    public int ParentTableID { get; set; }
    //public int ProgressColumnID { get; set; }
    public int FormSetID { get; set; }
    public int ParentRecordID { get; set; }
    DataTable _dtFormSet;
    User _objUser;
    UserRole _theUserRole;
    Table _theTable;
    Record _theRecord;
    //Column _pcColumn;
    string _strMode = "edit";
    int _iHideTabCount = 0;
    int _iStepForParent = 0;
    //Column _csColumn = null;
    int? _iCurrentStep = null;
    FormSetForm _csFormSetForm = null;
    FormSet _theFromSet = null;
    FormSetGroup _theFormSetGroup = null;
    protected void Page_Init(object sender, EventArgs e)
    {
        _objUser = (User)Session["User"];
        _theUserRole = (UserRole)Session["UserRole"];
        ParentTableID =int.Parse( Cryptography.Decrypt(Request.QueryString["ParentTableID"].ToString()));
        //ProgressColumnID = int.Parse(Cryptography.Decrypt(Request.QueryString["ProgressColumnID"].ToString()));
        FormSetID = int.Parse(Cryptography.Decrypt(Request.QueryString["FormSetID"].ToString()));
        ParentRecordID = int.Parse(Cryptography.Decrypt(Request.QueryString["ParentRecordID"].ToString()));

        if (Request.QueryString["showparent"] != null)
        {
            _iStepForParent = 1;
        }


        //_pcColumn = RecordManager.ets_Column_Details(ProgressColumnID);
        _theFromSet = FormSetManager.dbg_FormSet_Detail(FormSetID);
        //lblFormSetName.Text = _pcColumn.DisplayName;

        _theFormSetGroup = FormSetManager.dbg_FormSetGroup_Detail((int)_theFromSet.FormSetGroupID);
        
        lblFormSetName.Text = _theFromSet.FormSetName;

        if (_theFormSetGroup != null)
        {
            lblFormSetName.Text = _theFromSet.FormSetName + " " + _theFormSetGroup.FormSetGroupName;
        }

        _theTable = RecordManager.ets_Table_Details(ParentTableID);
        _theRecord = RecordManager.ets_Record_Detail_Full((int)ParentRecordID);

        //string strProgressStatus = RecordManager.GetRecordValue(ref _theRecord, _pcColumn.SystemName);
        string _strProgressStatus = Request.QueryString["ps"].ToString();
        if (_strProgressStatus == "2")
        {
            _strMode = "view";
            lnkNext.CausesValidation = false;
            lnkNext2.CausesValidation = false; 
            lnkCancel.Visible = false;
            lnkCancel2.Visible = false;
            lnkSaveForLater.Visible = false;
            lnkSaveForLater2.Visible = false;
            lnkSubmit.Visible = false;
            lnkSubmit2.Visible = false;
            hlBack.Visible = true;
            hlBack2.Visible = true;
        }

        if (!IsPostBack)
        {
            if (_strProgressStatus == "0")
            {
                FormSetManager.StartProgressHistory(FormSetID, ParentRecordID);
            }
        }
       
       
//        _dtFormSet = Common.DataTableFromText(@"SELECT FormSet.FormSetID,FormSet.DisplayOrder,FormSet.FormSetName,
//                FormSet.ParentTableID,FormSet.ProgressColumnID,FormSet.ChildTableID,
//                FormSet.UpdateColumnID,FormSet.UpdateColumnValue,CurrentStepColumnID,
//                (SELECT TOP 1 HideColumnID FROM TableChild WHERE 
//                TableChild.ParentTableID=FormSet.ParentTableID AND TableChild.ChildTableID=FormSet.ChildTableID )  AS HideColumnID,
//                (SELECT TOP 1 HideColumnValue FROM TableChild WHERE 
//                TableChild.ParentTableID=FormSet.ParentTableID AND TableChild.ChildTableID=FormSet.ChildTableID )  AS HideColumnValue,
//                (SELECT TOP 1 HideOperator FROM TableChild WHERE 
//                TableChild.ParentTableID=FormSet.ParentTableID AND TableChild.ChildTableID=FormSet.ChildTableID )  AS HideOperator
//                FROM FormSet WHERE FormSet.ParentTableID=" + ParentTableID.ToString() + @"
//                 AND FormSet.ProgressColumnID=" + ProgressColumnID.ToString() + @" ORDER BY FormSet.DisplayOrder");


        _dtFormSet = Common.DataTableFromText(@"SELECT FormSetForm.FormSetFormID,FormSetForm.DisplayOrder,UpdateColumnID,UpdateColumnValue,FormSetForm.TableID AS ChildTableID,
                        (SELECT TOP 1 HideColumnID FROM TableChild WHERE 
                        TableChild.ParentTableID="+ParentTableID.ToString()+@" AND TableChild.ChildTableID=FormSetForm.TableID )  AS HideColumnID,
                        (SELECT TOP 1 HideColumnValue FROM TableChild WHERE 
                        TableChild.ParentTableID=" + ParentTableID.ToString() + @" AND TableChild.ChildTableID=FormSetForm.TableID )  AS HideColumnValue,
                        (SELECT TOP 1 HideOperator FROM TableChild WHERE 
                        TableChild.ParentTableID=" + ParentTableID.ToString() + @" AND TableChild.ChildTableID=FormSetForm.TableID )  AS HideOperator
                        FROM FormSetForm INNER JOIN [Table]
                        ON FormSetForm.TableID=[Table].TableID
                        INNER JOIN FormSet ON FormSetForm.FormSetID=FormSet.FormSetID
                        INNER JOIN FormSetGroup ON FormSetGroup.FormSetGroupID=FormSet.FormSetGroupID                       
                        WHERE FormSetGroup.ParentTableID=" + ParentTableID.ToString() + @" AND 
                        FormSetForm.FormSetID="+FormSetID.ToString()+@"     ORDER BY FormSetForm.DisplayOrder");
        
        //, FormSetProgress.Completed LEFT JOIN FormSetProgress ON FormSetForm.FormSetFormID=FormSetProgress.FormSetFormID
        
        if (_dtFormSet.Rows.Count > 0)
        {
            int iCS=0;
            foreach (DataRow dr in _dtFormSet.Rows)
            {
                string strFormSetProgressID = Common.GetValueFromSQL("SELECT TOP 1 FormSetProgressID FROM FormSetProgress WHERE RecordID=" +
                    ParentRecordID.ToString() + " AND FormSetID=" + FormSetID.ToString() + " AND FormSetFormID=" + dr["FormSetFormID"].ToString());

                if (strFormSetProgressID == "")
                {
                    _iCurrentStep = iCS+_iStepForParent;
                    break;
                }
                else
                {
                    FormSetProgress theFormSetProgress = FormSetManager.dbg_FormSetProgress_Detail(int.Parse(strFormSetProgressID));
                    if (theFormSetProgress.Completed == null)
                    {
                        _iCurrentStep = iCS + _iStepForParent;
                        break;
                    }
                    else
                    {
                        if ((bool)theFormSetProgress.Completed == false)
                        {
                            _iCurrentStep = iCS + _iStepForParent;
                            break;
                        }
                    }
                }

               
                iCS=iCS+1;
            }

            //if (_dtFormSet.Rows[0]["CurrentStepColumnID"] != DBNull.Value)
            //{
            //    _csColumn = RecordManager.ets_Column_Details(int.Parse(_dtFormSet.Rows[0]["CurrentStepColumnID"].ToString()));
            //}
            
            MakeChildTables(_dtFormSet);

           
                        
        }


    }

    protected string GetPrevNextURL(int index)
    {
        string strURL = Request.RawUrl;
        strURL = strURL.Replace("veryfirstok", "old");
        if (Request.QueryString["pageindex"] == null)
        {
            strURL = strURL + "&pageindex=" + ViewState["TabIndex"].ToString();
        }
        else
        {
            strURL = strURL.Substring(0, strURL.IndexOf("&pageindex="));
            strURL = strURL + "&pageindex=" + ViewState["TabIndex"].ToString();
        }

        return strURL;
    }

 

    protected void ShowHideTab()
    {
        if (_strMode == "edit")
        {
            if ((int)ViewState["TabIndex"] == 0)
            {
                lnkPrev.Visible = false;
                lnkCancel.Visible = true;

                lnkPrev2.Visible = false;
                lnkCancel2.Visible = true;
            }
            else
            {
                lnkPrev.Visible = true;
                lnkCancel.Visible = false;

                lnkPrev2.Visible = true;
                lnkCancel2.Visible = false;
            }

            if ((int)ViewState["TabIndex"] == _dtFormSet.Rows.Count + _iStepForParent - _iHideTabCount - 1)
            {
                lnkNext.Visible = false;
                lnkSubmit.Visible = true;

                lnkNext2.Visible = false;
                lnkSubmit2.Visible = true;
            }
            else
            {
                lnkNext.Visible = true;
                lnkSubmit.Visible = false;

                lnkNext2.Visible = true;
                lnkSubmit2.Visible = false;
            }
        }


        if (_strMode == "view")
        {
            if ((int)ViewState["TabIndex"] == 0)
            {
                lnkPrev.Visible = false;               
                lnkPrev2.Visible = false;
               
            }
            else
            {
                lnkPrev.Visible = true;             
                lnkPrev2.Visible = true;
               
            }

            if ((int)ViewState["TabIndex"] == _dtFormSet.Rows.Count + _iStepForParent - _iHideTabCount - 1)
            {
                lnkNext.Visible = false;
               
                lnkNext2.Visible = false;
                
            }
            else
            {
                lnkNext.Visible = true;
             
                lnkNext2.Visible = true;
               
            }
        }


        for (int i = 0; i < tabDetail.Tabs.Count; i++)
        {
            if (i == (int)ViewState["TabIndex"])
            {
                tabDetail.Tabs[i].Visible = true;
            }
            else
            {
                tabDetail.Tabs[i].Visible = false;
            }
        }
        
        if (((int)ViewState["TabIndex"] + 1)>(_dtFormSet.Rows.Count + _iStepForParent - _iHideTabCount))
        {
            lblStep.Text = "No Form. Please set up forms or contact with Admin.";
            hlBack.Visible = true;
            //hlBack2.Visible = true;

            lnkSaveForLater.Visible = false;
            lnkSaveForLater2.Visible = false;
            lnkNext.Visible = false;
            lnkNext2.Visible = false;
            lnkPrev.Visible = false;
            lnkPrev2.Visible = false;
            hlPrint.Visible = false;
            lnkCancel2.Visible = false;

            lblStep2.Text = "";
        }
        else
        {
            lblStep.Text = "Step " + ((int)ViewState["TabIndex"] + 1).ToString() + " of " + (_dtFormSet.Rows.Count + _iStepForParent - _iHideTabCount).ToString();
            lblStep2.Text = lblStep.Text;
        }

        


    }
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["pageindex"] == null)
            {
                ViewState["TabIndex"] = 0;
            }
            else
            {
                ViewState["TabIndex"] = int.Parse(Request.QueryString["pageindex"].ToString());
            }

            if (_iCurrentStep != null)
            {


                if (tabDetail.Tabs.Count >= ((int)_iCurrentStep + 1))
                {
                    //ViewState["TabIndex"] = (int)_iCurrentStep;
                    if (Request.QueryString["pageindex"] == null)
                    {

                        ViewState["TabIndex"] = (int)_iCurrentStep;
                    }
                }
            }

            if (_strMode == "view")
            {
                if (Request.QueryString["pageindex"] == null)
                {
                    ViewState["TabIndex"] = 0;
                }
            }
            else
            {
                if (Request.QueryString["veryfirst"] != null)
                {
                    ViewState["TabIndex"] = 0;
                }
                else
                {
                    if (Request.QueryString["veryfirstok"] != null)
                    {
                        
                        ViewState["TabIndex"] = 1;
                    }
                }
            }

            hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&Recordid=" + Request.QueryString["ParentRecordID"].ToString() + "&TableID=" + Request.QueryString["ParentTableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();
            hlBack2.NavigateUrl = hlBack.NavigateUrl;

            hlPrint.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetPrint.aspx?ParentRecordID=" + Request.QueryString["ParentRecordID"].ToString() + "&ParentTableID=" + Request.QueryString["ParentTableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString();


            if (_theTable != null && Request.QueryString["veryfirst"]==null)
            {
                if (_theTable.HeaderName != "")
                {
                    int iTN1 = 0;
                    string strReturnSQL = "";
                    DataTable dtRecordInfo = RecordManager.ets_Record_List(ParentTableID, null, null, null, null, null, "", "",
                        null, null, ref iTN1, ref iTN1, "nonstandard", "", " AND Record.RecordID=" + ParentRecordID.ToString(), null, null, "",
                        "", "", null, ref strReturnSQL, ref strReturnSQL);

                    string strHeader = _theTable.HeaderName;
                    if (dtRecordInfo.Rows.Count > 0)
                    {
                        foreach (DataColumn dc in dtRecordInfo.Columns)
                        {
                            strHeader = strHeader.Replace("[" + dc.ColumnName + "]", dtRecordInfo.Rows[0][dc.ColumnName].ToString());
                        }
                    }
                    lblHeaderName.Text = strHeader;


                }
            }


        }

        ShowHideTab();
    }

    protected void lnkCancel_Click(object sender, EventArgs e)
    {
        //delete records

        foreach (DataRow dr in _dtFormSet.Rows)
        {
            //check advanced security
           
            Table theChildTable = RecordManager.ets_Table_Details(int.Parse(dr["ChildTableID"].ToString()));
            
            string strCaption = theChildTable.TableName;

            
            string strTextSearch = "";
            string strRecordID = "";
            if (ParentRecordID.ToString() != "")
            {

                DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,ColumnID FROM [Column] WHERE ColumnType='dropdown' AND  TableID=" + dr["ChildTableID"].ToString() + " AND TableTableID=" + ParentTableID.ToString());
                foreach (DataRow drCT in dtTemp.Rows)
                {
                    Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(drCT["ColumnID"].ToString()));
                    Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                    Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(ParentRecordID.ToString()));
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

                    strRecordID = Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" +
                dr["ChildTableID"].ToString() + " AND IsActive= 1 AND (" + strTextSearch + ") ORDER BY RecordID");

                    if(strRecordID!="")
                        Common.ExecuteText("DELETE Record WHERE RecordID=" + strRecordID);

                }
            }

           

        }


        //Common.ExecuteText("UPDATE Record SET " + _pcColumn.SystemName + "='0' WHERE RecordID=" + _theRecord.RecordID.ToString());

        //if (_csColumn != null)
        //{
        //    Common.ExecuteText("UPDATE Record SET " + _csColumn.SystemName + "='0' WHERE RecordID=" + _theRecord.RecordID.ToString());

        //}

        Common.ExecuteText("DELETE FormSetProgress WHERE RecordID="+ParentRecordID.ToString()+" AND FormSetID=" + FormSetID.ToString());
        if (Request.QueryString["veryfirst"] != null)
        {
            Common.ExecuteText("DELETE Record WHERE RecordID=" + ParentRecordID.ToString());
            Response.Redirect("~/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(ParentTableID.ToString()), true);
            return;
        }
        Response.Redirect(hlBack.NavigateUrl, true);

    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {

        if ((int)ViewState["TabIndex"] != 0)
        {
            ViewState["TabIndex"] = (int)ViewState["TabIndex"] - 1;

            //ShowHideTab();
         
            if (_strMode == "edit")
            {
                //if (_ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID != null)
                //{
                //    Common.ExecuteText("UPDATE FormSetProgress SET Completed=0 WHERE RecordID=" + ParentRecordID.ToString() + " AND FormSetFormID=" + _ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID.ToString());
                //}

                //Response.Redirect(Request.RawUrl, true);
            }
            else
            {
                //ShowHideTab();
            }


        }

        Response.Redirect(GetPrevNextURL(int.Parse(ViewState["TabIndex"].ToString())), true);

      
    }



    protected void lnkSaveForLater_Click(object sender, EventArgs e)
    {

        _ctDetailEdit[(int)ViewState["TabIndex"]].SaveOK = false;
        _ctDetailEdit[(int)ViewState["TabIndex"]].DoValidation = false;
        _ctDetailEdit[(int)ViewState["TabIndex"]].lnkSaveClose_Click(null, null);

        if (_ctDetailEdit[(int)ViewState["TabIndex"]].SaveOK == true)
        {
            //it is saved 
          
            //Set the status to resume
            //Common.ExecuteText("UPDATE Record SET " + _pcColumn.SystemName + "='1' WHERE RecordID=" + _theRecord.RecordID.ToString());

            //if (_csColumn != null)
            //{
            //    Common.ExecuteText("UPDATE Record SET " + _csColumn.SystemName + "='" + ViewState["TabIndex"].ToString() + "' WHERE RecordID=" + _theRecord.RecordID.ToString());

            //}
            if (_ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID != null)
            {
                Common.ExecuteText("UPDATE FormSetProgress SET Completed=0 WHERE RecordID=" + ParentRecordID.ToString() + " AND FormSetFormID=" + _ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID.ToString());
            }

            Response.Redirect(hlBack.NavigateUrl, true);
        }

    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {

        if (_strMode == "edit")
        {
            _ctDetailEdit[(int)ViewState["TabIndex"]].SaveOK = false;
            _ctDetailEdit[(int)ViewState["TabIndex"]].lnkSaveClose_Click(null, null);

            if (Request.QueryString["showparent"] != null)
            {
                if ((int)ViewState["TabIndex"] == 0)
                {
                    //if (_csColumn != null)
                    //{
                    //    Common.ExecuteText("UPDATE Record SET " + _pcColumn.SystemName + "='1' WHERE RecordID=" + _theRecord.RecordID.ToString());
                    //    Common.ExecuteText("UPDATE Record SET " + _csColumn.SystemName + "='1' WHERE RecordID=" + _theRecord.RecordID.ToString());

                    //    Response.Redirect(Request.RawUrl, true);
                    //}
                    if (Request.QueryString["veryfirst"] != null)
                    {
                        Response.Redirect(Request.RawUrl.Replace("&veryfirst=yes", "&veryfirstok=yes"), true);
                        return;
                    }
                }
            }
        }
        else
        {
            _ctDetailEdit[(int)ViewState["TabIndex"]].SaveOK = true;
        }


        if (_ctDetailEdit[(int)ViewState["TabIndex"]].SaveOK == true)
        {
            //it is saved 

            if (_strMode != "view")
            {
                if (_ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID != null)
                {
                    Common.ExecuteText("UPDATE FormSetProgress SET Completed=1 WHERE RecordID=" + ParentRecordID.ToString() + " AND FormSetFormID=" + _ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID.ToString());
                }
            }

            ViewState["TabIndex"] = (int)ViewState["TabIndex"] + 1;

            //ShowHideTab();
         
            if (_strMode == "edit")
            {
                if (_ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID != null)
                {
                    Common.ExecuteText("UPDATE FormSetProgress SET Completed=0 WHERE RecordID=" + ParentRecordID.ToString() + " AND FormSetFormID=" + _ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID.ToString());
                }

                //Response.Redirect(Request.RawUrl, true);
            }
            else
            {
                //ShowHideTab();
            }

             
        }

        Response.Redirect(GetPrevNextURL(int.Parse(ViewState["TabIndex"].ToString())),true);
    }


    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        _ctDetailEdit[(int)ViewState["TabIndex"]].SaveOK = false;
        _ctDetailEdit[(int)ViewState["TabIndex"]].lnkSaveClose_Click(null, null);

        if (_ctDetailEdit[(int)ViewState["TabIndex"]].SaveOK == true)
        {
            //it is saved 
                      
            //Set the status to complete
            //Common.ExecuteText("UPDATE Record SET "+_pcColumn.SystemName+"='2' WHERE RecordID=" + _theRecord.RecordID.ToString());

            //if (_csColumn != null)
            //{
            //    Common.ExecuteText("UPDATE Record SET " + _csColumn.SystemName + "='0' WHERE RecordID=" + _theRecord.RecordID.ToString());

            //}
            if (_ctDetailEdit[(int)ViewState["TabIndex"]].FormSetFormID != null)
            {
                Common.ExecuteText("UPDATE FormSetProgress SET Completed=1 WHERE RecordID=" + ParentRecordID.ToString() + " AND FormSetID=" +  FormSetID.ToString());
            }
            Response.Redirect(hlBack.NavigateUrl, true);
        }

    }

    protected string GetTextFromValue(string strDropdownValues, string strDBValue)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    if (strDBValue.ToLower() == strValue.ToLower())
                    {
                        return strText;
                    }
                }
            }
        }
        return strDBValue;

    }

    protected void MakeChildTables(DataTable dtCT)
    {


            _ctDetailEdit = new Pages_UserControl_DetailEdit[dtCT.Rows.Count + _iStepForParent];

            TabPanel[] cTabPanel = new TabPanel[dtCT.Rows.Count + _iStepForParent];

            if (_iStepForParent == 1)
            {
                _ctDetailEdit[0] = (Pages_UserControl_DetailEdit)LoadControl("~/Pages/UserControl/DetailEdit.ascx");
                _ctDetailEdit[0].TableID = ParentTableID;
                _ctDetailEdit[0].ID = "ctDetailEdit0";
                _ctDetailEdit[0].Mode = _strMode;
                _ctDetailEdit[0].RecordID = ParentRecordID;
                cTabPanel[0] = new TabPanel();

                cTabPanel[0].HeaderText = _theTable.TableName;
                cTabPanel[0].Controls.Add(_ctDetailEdit[0]);
                tabDetail.Tabs.Add(cTabPanel[0]);
            }

            int i = _iStepForParent;
            foreach (DataRow dr in dtCT.Rows)
            {
                //check advanced security
                string strChildTableRight = "2";

                Table theChildTable = RecordManager.ets_Table_Details(int.Parse(dr["ChildTableID"].ToString()));

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

                string strCaption = theChildTable.TableName;

                if (strCaption == "")
                {
                    Table theTable = RecordManager.ets_Table_Details(int.Parse(dr["ChildTableID"].ToString()));

                    if (theTable != null)
                    {
                        strCaption = theTable.TableName;
                    }

                }


                //check show hide :-)
                if (_strMode.ToLower() != "add" && _theRecord != null)
                {
                    if (dr["HideColumnID"] != DBNull.Value && dr["HideColumnValue"] != DBNull.Value && dr["HideOperator"] != DBNull.Value)
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
                                        strThisHideColumnValue = GetTextFromValue(theHideColumn.DropdownValues, strThisHideColumnValue);
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
                                _iHideTabCount = _iHideTabCount + 1;
                                continue;
                            }


                        }

                    }
                }



                strCaption = "<strong>" + strCaption + "</strong>";

               
                string strTextSearch = "";
                string strRecordID = "";
                if (ParentRecordID.ToString() != "")
                {

                    DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,ColumnID FROM [Column] WHERE ColumnType='dropdown' AND  TableID=" + dr["ChildTableID"].ToString() + " AND TableTableID=" + ParentTableID.ToString());
                    foreach (DataRow drCT in dtTemp.Rows)
                    {
                        Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(drCT["ColumnID"].ToString()));
                        Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                        Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(ParentRecordID.ToString()));
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

                        strRecordID = Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" +
                    dr["ChildTableID"].ToString() + " AND IsActive= 1 AND (" + strTextSearch + ") ORDER BY RecordID");

                        if (strRecordID == "")
                        {
                            if (SecurityManager.IsRecordsExceeded((int)_theUserRole.AccountID))
                            {
                                Session["DoNotAllow"] = "true";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "DoNotAllow", "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);
                                _iHideTabCount = _iHideTabCount + 1;
                                continue;
                            }


                            //create a balnk record
                            Record theRecord = new Record();
                            theRecord.TableID = int.Parse(dr["ChildTableID"].ToString());
                            theRecord.IsActive = true;
                            theRecord.EnteredBy = _objUser.UserID;
                            RecordManager.MakeTheRecord(ref theRecord, drCT["SystemName"].ToString(), strLinkedColumnValue);

                            DataTable dtIDs = Common.DataTableFromText(@"SELECT ColumnID FROM [Column] WHERE 
                                TableID="+theRecord.TableID.ToString()+@" AND ColumnType='number'
                                AND NumberType=8");

                            if (dtIDs.Rows.Count > 0)
                            {
                                foreach (DataRow drC in dtIDs.Rows)
                                {
                                    Column aColumnID = RecordManager.ets_Column_Details((int)drC[0]);

                                    string strValue = "1";
                                    try
                                    {
                                        string strMax = Common.GetValueFromSQL("SELECT MAX(CONVERT(INT," + aColumnID.SystemName + ")) FROM Record WHERE TableID=" + theRecord.TableID.ToString());
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
                                    RecordManager.MakeTheRecord(ref theRecord, aColumnID.SystemName, strValue);
                                }

                            }


                            int iNewRecordID = RecordManager.ets_Record_Insert(theRecord);
                            strRecordID = iNewRecordID.ToString();
                        }

                    }
                }
                  
                    //_TabIndex = _TabIndex + 1;

                if (strRecordID == "")
                {
                    _iHideTabCount = _iHideTabCount + 1;
                    continue;
                }

                    _ctDetailEdit[i] = (Pages_UserControl_DetailEdit)LoadControl("~/Pages/UserControl/DetailEdit.ascx");
                    _ctDetailEdit[i].TableID = int.Parse(dr["ChildTableID"].ToString());
                    _ctDetailEdit[i].ID = "ctDetailEdit" + i.ToString();
                    _ctDetailEdit[i].Mode = _strMode;
                    _ctDetailEdit[i].FormSetFormID = int.Parse(dr["FormSetFormID"].ToString());
                    if (strRecordID != "")
                    {
                        //update UpdateColumnValue
                        if (!IsPostBack)
                        {
                            if (dr["UpdateColumnID"] != DBNull.Value && dr["UpdateColumnValue"] != DBNull.Value)
                            {
                                if (dr["UpdateColumnValue"].ToString() != "")
                                {
                                    Column theUpdateColumn = RecordManager.ets_Column_Details(int.Parse(dr["UpdateColumnID"].ToString()));
                                    if (theUpdateColumn != null)
                                        Common.ExecuteText("UPDATE Record SET " + theUpdateColumn.SystemName + "='" + dr["UpdateColumnValue"].ToString().Replace("'", "''") + "' WHERE RecordID=" + strRecordID);
                                }
                            }
                        }


                        _ctDetailEdit[i].RecordID = int.Parse(strRecordID);
                    }
                    else
                    {
                        _ctDetailEdit[i].OnlyOneRecord = true;


                    }

                
                    cTabPanel[i] = new TabPanel();

                    cTabPanel[i].HeaderText = strCaption;
                    cTabPanel[i].Controls.Add(_ctDetailEdit[i]);
                    tabDetail.Tabs.Add(cTabPanel[i]);

                    //}

                i = i + 1;
            }


        

    }

}