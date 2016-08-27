using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;
using DocGen.DAL;
using System.Xml;

public partial class Pages_Record_ColumnDetail : SecurePage
{

    bool _bShowExceedances = false;
    string _strActionMode = "view";
    string _qsMode = "";
    string _qsColumnID = "-1";
    string _qsTableID = "";
    User _ObjUser;
    Table _theTable;
    //int? _iTableTabID = null;
    Common_Pager _gvCL_Pager;
    Common_Pager _gvPager;
    int _iCLColumnCount = 0;
    int _iColumnID;
    //bool _bOK = false;
    int _iCLStartIndex = 0;
    int _iCLMaxRows = 0;
    int _iCLTN = 0;

    bool _bIsAccountHolder = false;
    bool _bGodUser = false;
    UserRole _CurrentUserRole = null;
    Role _CurrentRole = null;
    string _strFilesLocation = "";
    //protected void gvChangedLog_DataBound(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (gvChangedLog.Rows.Count > 0)
    //        {


    //            int iTN = 0;
    //            DataTable dtTemp = RecordManager.ets_Column_Changes_Select(
    //                int.Parse(_qsColumnID), _iCLStartIndex, _iCLMaxRows + 1, ref  iTN, ref _iCLColumnCount);
    //            if (dtTemp != null)
    //            {

    //                for (int i = dtTemp.Rows.Count - 2; i > -1; i--)
    //                {

    //                    for (int j = 3; j < _iCLColumnCount - 1; j++)
    //                    {
    //                        if (dtTemp.Rows[i + 1][j].ToString() != dtTemp.Rows[i][j].ToString())
    //                        {
    //                            gvChangedLog.Rows[i].Cells[j].BackColor = System.Drawing.Color.Yellow;
    //                        }
    //                    }

    //                }


    //            }



    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        lblMsg.Text = ex.Message;
    //    }

    //}


    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ShowHide(); 
    }

    protected void PopulateTerminology()
    {
        //stgTableNameCaption
        stgTableNameCaption.InnerText = stgTableNameCaption.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));
        stgFieldNameCap.InnerText = stgFieldNameCap.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        rfvColumnName.ErrorMessage = rfvColumnName.ErrorMessage.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));

        lblColumnMessage.Text = lblColumnMessage.Text.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field").ToLower());
        optSingle.Text = optSingle.Text.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        optDouble.Text = optDouble.Text.Replace("Fields", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Fields", "Fields"));

        lblFieldType.Text = lblFieldType.Text.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        string strConditions_TL = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Conditions", "Conditions");
        hfConditions_T.Value = strConditions_TL;

        hlValidConditions.Text = hlValidConditions.Text.Replace("Conditions", strConditions_TL);
        hlWarningConditions.Text = hlWarningConditions.Text.Replace("Conditions", strConditions_TL);
        hlExceedanceConditions.Text = hlExceedanceConditions.Text.Replace("Conditions", strConditions_TL);
    }

    protected void PopulateChildTables()
    {
        ddlRecordCountTable.Items.Clear();

        ddlRecordCountTable.DataSource = Common.DataTableFromText(@"SELECT   DISTINCT  [Table].TableID, [Table].TableName
FROM         [Table] INNER JOIN
                      TableChild ON [Table].TableID = TableChild.ChildTableID
                      WHERE TableChild.ParentTableID=" + _qsTableID + @" ORDER BY [Table].TableName");

        ddlRecordCountTable.DataBind();



    }

    protected void PopulateTabs()
    {
        ddlTableTab.Items.Clear();

        DataTable dtTemp = Common.DataTableFromText(@"SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + "  AND DisplayOrder<>0 ORDER BY DisplayOrder");

        if (dtTemp.Rows.Count == 0)
        {
            divTableTab.Visible = false;
        }


        ddlTableTab.DataSource = dtTemp;
        ddlTableTab.DataBind();

        DataTable dtTempp = Common.DataTableFromText(@"SELECT * FROM TableTab WHERE TableID=" + _theTable.TableID.ToString() + " AND DisplayOrder=0 ORDER BY DisplayOrder");

        if (dtTempp.Rows.Count > 0)
        {

            ListItem liSelect = new ListItem(dtTempp.Rows[0]["TabName"].ToString(), "");
            ddlTableTab.Items.Insert(0, liSelect);
        }


    }

    protected void BindOptionImage()
    {
        if (Session["OptionImage"] != null)
        {
            DataTable dtOptionImage = (DataTable)Session["OptionImage"];
            gvOptionImage.DataSource = dtOptionImage;
            gvOptionImage.DataBind();
            if (gvOptionImage.TopPagerRow != null)
                gvOptionImage.TopPagerRow.Visible = true;



            if (dtOptionImage.Rows.Count == 0)
            {
                HyperLink hplNewData = (HyperLink)gvOptionImage.Controls[0].Controls[0].FindControl("hplNewData");
                hplNewData.NavigateUrl = GetOptionImageAddURL();
            }
            else
            {

                GridViewRow gvr = gvOptionImage.TopPagerRow;
                if (gvr != null)
                    _gvPager = (Common_Pager)gvr.FindControl("Pager");


                if (_gvPager != null)
                {
                    _gvPager.AddURL = GetOptionImageAddURL();
                    _gvPager.HyperAdd_CSS = "optionimgelink";
                }

            }


        }

    }



    protected void Pager_DeleteAction(object sender, EventArgs e)
    {




        string sCheck = "";
        for (int i = 0; i < gvOptionImage.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvOptionImage.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)gvOptionImage.Rows[i].FindControl("LblID")).Text + ",";
            }
        }



        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvOptionImage, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            return;
        }
        else
        {
            DeleteOptionImage(sCheck);

        }

        BindOptionImage();


    }

    private void DeleteOptionImage(string keys)
    {
        try
        {
            if (!string.IsNullOrEmpty(keys))
            {

                foreach (string sTemp in keys.Split(','))
                {
                    if (!string.IsNullOrEmpty(sTemp))
                    {
                        if (Session["OptionImage"] != null)
                        {
                            DataTable dtOptionImage = (DataTable)Session["OptionImage"];
                            for (int i = 0; i < dtOptionImage.Rows.Count; i++)
                            {
                                if (sTemp == dtOptionImage.Rows[i]["OptionImageID"].ToString())
                                {
                                    dtOptionImage.Rows.RemoveAt(i);
                                }
                            }

                            dtOptionImage.AcceptChanges();
                            Session["OptionImage"] = dtOptionImage;

                        }

                    }
                }


            }

        }
        catch
        {

        }

    }

    protected string GetOptionImageEditURL()
    {
        return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/OptionImageDetaill.aspx?mode=" + Cryptography.Encrypt("edit") + "&OptionImageID=";
    }

    protected string GetOptionImageAddURL()
    {
        return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/OptionImageDetaill.aspx?mode=" + Cryptography.Encrypt("add");
    }

    protected void gvOptionImage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgImage = (Image)e.Row.FindControl("imgImage");
                //imgImage.Width = 30;
                //imgImage.Height = 30;
                imgImage.ImageUrl = Session["FilesLocation"].ToString() + "/UserFiles/AppFiles/" + DataBinder.Eval(e.Row.DataItem, "UniqueFileName");
            }
        }
        catch
        {

        }

    }

    protected void PopulateFilterDDL()
    {

        ddlFilter.Items.Clear();
        if (ddlDDTable.SelectedValue != "")
        {
            ddlFilter.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE   
                TableID=" + ddlDDTable.SelectedValue + @" AND (DropDownType<>'table' OR DropDownType<>'tabledd')");

            ddlFilter.DataBind();

        }

    }


    protected void PopulateCompareDDL()
    {

        ddlCompareColumnID.Items.Clear();


        ddlCompareColumnID.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE IsStandard=0  
            and ColumnType NOT IN ('staticcontent','button') and TableID=" + _theTable.TableID.ToString() + " AND ColumnID<>" + _qsColumnID);

        ddlCompareColumnID.DataBind();

        ListItem liAll = new ListItem("--Please Select--", "");
        ddlCompareColumnID.Items.Insert(0, liAll);

    }

    //protected void PopulateRetriverDDL()
    //{

    //    ddlDataRetriver.Items.Clear();

    //    ddlDataRetriver.DataSource = Common.DataTableFromText(@"SELECT * FROM DataRetriever WHERE TableID =" + _theTable.TableID.ToString());        
    //    ddlDataRetriver.DataBind();

    //    ListItem liSelect = new ListItem("-- Please Select --", "");
    //    ddlDataRetriver.Items.Insert(0,liSelect); 

    //}

    protected void PopulateGraphDefinition()
    {
        int iTN = 0;

        ddlDefaultGraphDefinition.DataSource = GraphManager.ets_GraphDefinition_Select(null /*int.Parse(Session["AccountID"].ToString())*/,
            null, null, null, null,
            this._theTable.TableID,
            this._iColumnID,
            null,
            true,
            null, null, null, null,
            null, null, null, null, ref iTN);
        ddlDefaultGraphDefinition.DataBind();
    }

    protected void ddlTLControllingField_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTLControllingField.SelectedValue == "")
        {
            trTLValueImage.Visible = false;
        }
        else
        {
            trTLValueImage.Visible = true;
            cbcValue1.TableID = int.Parse(_qsTableID);
            cbcValue1.ddlYAxisV = ddlTLControllingField.SelectedValue;
            lnkAddTL1.Visible = true;

            trTLValueLight2.Visible = false;
            trTLValueLight3.Visible = false;
            trTLValueLight4.Visible = false;
            trTLValueLight5.Visible = false;

            cbcValue2.TableID = null;
            //cbcValue2.ddlYAxisV = "";

            cbcValue3.TableID = null;
            //cbcValue3.ddlYAxisV = "";

            cbcValue4.TableID = null;
            //cbcValue4.ddlYAxisV = "";

            cbcValue5.TableID = null;
            //cbcValue5.ddlYAxisV = "";


        }

    }


    protected void PopulateTable()
    {
        ddlDDTable.Items.Clear();
        int iTN = 0;
        List<Table> listTable = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());
        foreach (Table eachTable in listTable)
        {
            if (eachTable.TableID.ToString() != _qsTableID)
            {
                System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachTable.TableName, eachTable.TableID.ToString());
                ddlDDTable.Items.Insert(ddlDDTable.Items.Count, aItem);
            }
        }

        System.Web.UI.WebControls.ListItem aUser = new System.Web.UI.WebControls.ListItem("User", "-1");
        ddlDDTable.Items.Insert(ddlDDTable.Items.Count, aUser);

    }



    protected void PopulateAvgColumn()
    {
        ddlAvgColumn.DataSource = Common.DataTableFromText("SELECT ColumnID,DisplayName FROM [Column] WHERE   TableID=" + _theTable.TableID.ToString() + " AND IsStandard=0 AND ColumnType='number' AND ColumnID<>" + _qsColumnID);
        ddlAvgColumn.DataBind();
        ListItem liAll = new ListItem("-Select Average Field-", "-1");
        ddlAvgColumn.Items.Insert(0, liAll);

    }


    protected string GetCBCXML(Pages_UserControl_ControlByColumn cbcX)
    {
        string xml = "";
        if (cbcX.ddlYAxisV != "")
        {

            xml = @"<root>" +
                   " <ddlYAxisV>" + HttpUtility.HtmlEncode(cbcX.ddlYAxisV) + "</ddlYAxisV>" +
                   " <txtUpperLimitV>" + HttpUtility.HtmlEncode(cbcX.txtUpperLimitV) + "</txtUpperLimitV>" +
                   " <txtLowerLimitV>" + HttpUtility.HtmlEncode(cbcX.txtLowerLimitV) + "</txtLowerLimitV>" +
                   " <hfTextSearchV>" + HttpUtility.HtmlEncode(cbcX.hfTextSearchV) + "</hfTextSearchV>" +
                   " <txtLowerDateV>" + HttpUtility.HtmlEncode(cbcX.txtLowerDateV) + "</txtLowerDateV>" +
                   " <txtUpperDateV>" + HttpUtility.HtmlEncode(cbcX.txtUpperDateV) + "</txtUpperDateV>" +
                   " <ddlDropdownColumnSearchV>" + HttpUtility.HtmlEncode(cbcX.ddlDropdownColumnSearchV) + "</ddlDropdownColumnSearchV>" +
                   " <txtSearchTextV>" + HttpUtility.HtmlEncode(cbcX.txtSearchTextV) + "</txtSearchTextV>" +
                  "</root>";


        }

        return xml;
    }


    protected void PopulateddlTLImage(ref DropDownList ddlTLImage)
    {
        try
        {
            string strXMLFilePath = Server.MapPath("~/XMLFiles/TraficImages.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strXMLFilePath);

            XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

            DataSet ds = new DataSet();
            ds.ReadXml(r);

            if (ds.Tables[0] != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ListItem liTemp = new ListItem(dr[0].ToString(), dr[1].ToString());
                    ddlTLImage.Items.Add(liTemp);
                }
            }

        }
        catch
        {
            //
        }
    }
    protected void PopulateMapPinHoverColumn()
    {
        ddlMapPinHoverColumnID.DataSource = Common.DataTableFromText("SELECT ColumnID,DisplayName FROM [Column] WHERE   TableID=" + _theTable.TableID.ToString() + " AND IsStandard=0  AND ColumnID<>" + _qsColumnID);
        ddlMapPinHoverColumnID.DataBind();
        ListItem liAll = new ListItem("--Please Select-", "");
        ddlMapPinHoverColumnID.Items.Insert(0, liAll);

    }

    protected void PopulateTLControllingField()
    {
        ddlTLControllingField.DataSource = Common.DataTableFromText("SELECT ColumnID,DisplayName FROM [Column] WHERE   TableID=" + _theTable.TableID.ToString() + " AND IsStandard=0  AND ColumnID<>" + _qsColumnID);
        ddlTLControllingField.DataBind();
        ListItem liAll = new ListItem("--Please Select-", "");
        ddlTLControllingField.Items.Insert(0, liAll);

    }

    protected void lnkShowHistory_Click(object sender, EventArgs e)
    {
        lnkHideHistory.Visible = true;
        divHistory.Visible = true;
        lnkShowHistory.Visible = false;

        if (_qsColumnID != "")
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

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta tag = new HtmlMeta();
        tag.HttpEquiv = "X-UA-Compatible";
        tag.Content = "IE=10,chrome=1";
        Header.Controls.Add(tag);

        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        { Response.Redirect("~/Default.aspx", false); }

        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            _bGodUser = true;
        }
        _strFilesLocation = Session["FilesLocation"].ToString();
        _qsColumnID = "-1";
        if (Request.QueryString["ColumnID"] != null)
            _qsColumnID = Cryptography.Decrypt(Request.QueryString["ColumnID"].ToString());

        hfColumnID.Value = _qsColumnID;


        _ObjUser = (User)Session["User"];
        _CurrentUserRole = (UserRole)Session["UserRole"];
        _CurrentRole = SecurityManager.Role_Details((int)_CurrentUserRole.RoleID);
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


        //ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);


        if (this.Master.FindControl("hfIsAccountHolder") != null)
        {
            HiddenField hfIsAccountHolder = (HiddenField)this.Master.FindControl("hfIsAccountHolder");
            if (hfIsAccountHolder != null && hfIsAccountHolder.Value != "")
            {
                _bIsAccountHolder = true;
            }
        }

        if (_bGodUser)
        {
            _bIsAccountHolder = true;
        }

        string strFancy = @"

        $(document).ready(function () {

                    var hfConditions_T = document.getElementById('hfConditions_T');
                
                    $(function () {
                        $("".validationlink"").fancybox({
                            scrolling: 'auto',
                            type: 'iframe',
                            width: 700,
                            height: 650,
                            titleShow: false
                        });
                    });


                        $(function () {
                        $("".optionimgelink"").fancybox({
                            scrolling: 'auto',
                            type: 'iframe',
                            width: 700,
                            height: 650,
                            titleShow: false
                        });
                    });

                      $(function () {
                        $(""#hlDDEdit"").fancybox({
                            scrolling: 'auto',
                            type: 'iframe',
                            width: 600,
                            height: 300,
                            titleShow: false
                        });
                    });


                      $(function () {
                        $(""#hlEditMappopup"").fancybox({
                            scrolling: 'auto',
                            type: 'iframe',
                            width: 800,
                            height: 600,
                            titleShow: false
                        });
                    });

                    $(function () {
                        $(""#hlReminders"").fancybox({
                            scrolling: 'auto',
                            type: 'iframe',
                            width: 900,
                            height: 800,
                            titleShow: false,
                             onClosed : function(){
                                //$('#btnRefreshRminder').trigger('click');                    
                                var d = new Date();
                            $.ajax({
                    
                                url: '../../GetLocations.aspx?Reminder=yes&t=' + d,
                                dataType: 'json',
                                success: function (res) {
                                    if(res>0)
                                    {
                                    document.getElementById('hlReminders').innerHTML='Reminders('+ res+')';
                                    }
                                    else
                                    {
                                    document.getElementById('hlReminders').innerHTML='Reminders';
                                    }
                        
                                },
                                error: function (xhr, err) {
                                   //                    
                                }
                            });


                            }
                        });
                    });


                    $(function () {
                        $(""#hlValidConditions"").fancybox({
                            scrolling: 'auto',
                            type: 'iframe',
                            width: 900,
                            height: 800,
                            titleShow: false,
                             onClosed : function(){
                                var d = new Date();
                                $.ajax({                        
                                      url: '../../GetLocations.aspx?Conditions=yes&ConditionType=V&ColumnID=" + _qsColumnID + @"&t=' + d,
                                    dataType: 'json',
                                    success: function (res) {
                                         var chkValidConditions=document.getElementById('chkValidConditions');
                                        if(res>0)
                                        {
                                             document.getElementById('hlValidConditions').innerHTML=hfConditions_T.value +'('+ res+')';
                                            chkValidConditions.checked=true;
                                               var chkValidFormula = document.getElementById('chkValidFormula');
                                                if (chkValidFormula != null)
                                                    chkValidFormula.checked = false;
                                        }
                                        else
                                        {
                                             document.getElementById('hlValidConditions').innerHTML=hfConditions_T.value;
                                            chkValidConditions.checked=false;
                                        }
                                        //alert(res);
                                    }
                                });


                            }
                        });
                    });


                $(function () {
                        $(""#hlWarningConditions"").fancybox({
                            scrolling: 'auto',
                            type: 'iframe',
                            width: 900,
                            height: 800,
                            titleShow: false,
                             onClosed : function(){
                                var d = new Date();
                                $.ajax({
                                url: '../../GetLocations.aspx?Conditions=yes&ConditionType=W&ColumnID=" + _qsColumnID + @"&t=' + d,
                                dataType: 'json',
                                success: function (res) {
                                     var chkWarningConditions=document.getElementById('chkWarningConditions');
                                    if(res>0)
                                    {
                                        chkWarningConditions.checked=true;
                                         document.getElementById('hlWarningConditions').innerHTML=hfConditions_T.value + '('+ res+')';
                               
                                               var chkWarningFormula = document.getElementById('chkWarningFormula');
                                                if (chkWarningFormula != null)
                                                    chkWarningFormula.checked = false;
                                    }
                                    else
                                    {
                                        document.getElementById('hlWarningConditions').innerHTML=hfConditions_T.value;
                                        chkWarningConditions.checked=false;
                                    }
                        
                                },
                                error: function (xhr, err) {
                                   //                    
                                }
                            });


                            }
                        });
                    });

                 $(function () {
                            $(""#hlExceedanceConditions"").fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 900,
                                height: 800,
                                titleShow: false,
                                 onClosed : function(){
                                    var d = new Date();
                                        $.ajax({
                                    url: '../../GetLocations.aspx?Conditions=yes&ConditionType=E&ColumnID=" + _qsColumnID + @"&t=' + d,
                                    dataType: 'json',
                                    success: function (res) {
                                         var chkExceedanceConditions=document.getElementById('chkExceedanceConditions');
                                        if(res>0)
                                        {
                                            chkExceedanceConditions.checked=true;
                                             document.getElementById('hlExceedanceConditions').innerHTML=hfConditions_T.value + '('+ res+')';
                               
                                                   var chkExceedanceFormula = document.getElementById('chkExceedanceFormula');
                                                    if (chkExceedanceFormula != null)
                                                        chkExceedanceFormula.checked = false;
                                        }
                                        else
                                        {
                                            chkExceedanceConditions.checked=false;
                                             document.getElementById('hlExceedanceConditions').innerHTML=hfConditions_T.value;
                                        }
                        
                                    },
                                    error: function (xhr, err) {
                                       //                    
                                    }
                                });


                                }
                            });
                        });
                        $(function () {
                            $("".calculationlink"").fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 600,
                                height: 550,
                                titleShow: false
                            });
                        });

                        $(function () {
                            $("".popuplink"").fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 600,
                                height: 650,
                                titleShow: false
                            });
                        });

                         $(function () {
                            $("".showlink"").fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 900,
                                height: 500,
                                titleShow: false
                            });
                        });

                        $(function () {
                            $("".colourlink"").fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 900,
                                height: 500,
                                titleShow: false
                            });
                        });

                     $(function () {
                                $("".showfilteredlink"").fancybox({
                                    scrolling: 'auto',
                                    type: 'iframe',
                                    width: 650,
                                    height: 350,
                                    titleShow: false
                                });
                            });


                $(function () {
                           $('.popupresetIDs').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 500,
                                height: 250,
                                titleShow: false
                            });
                        });  

                     $(function () {
                           $('.popupresetCals').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 500,
                                height: 250,
                                titleShow: false
                            });
                        });               

                        document.getElementById('hlFiltered').href = 'Filtered.aspx?hfFilterOperator=' + encodeURIComponent(document.getElementById('hfFilterOperator').value)
                            + '&hfFilterParentColumnID=' + encodeURIComponent(document.getElementById('hfFilterParentColumnID').value) 
                            + '&hfFilterOtherColumnID=' + encodeURIComponent(document.getElementById('hfFilterOtherColumnID').value) 
                            + '&hfFilterValue=' + encodeURIComponent(document.getElementById('hfFilterValue').value) + '&ParentTableID=' 
                            + encodeURIComponent(document.getElementById('hfParentTableID').value) + '&Tableid=' 
                            + document.getElementById('ctl00_HomeContentPlaceHolder_hfTableID').value 
                            + '&Columnid=' + document.getElementById('ctl00_HomeContentPlaceHolder_hfColumnID').value;

                        document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?formula=' + encodeURIComponent(document.getElementById('hfDisplayColumnsFormula').value) 
                            + '&Tableid=' + document.getElementById('hf_ddlDDTable').value; // $('#ddlDDTable').val();

                        document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = '../Help/CalculationTest.aspx?type=calculation&formula=' 
                            + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtCalculation').value) 
                            + '&Tableid=' + document.getElementById('ctl00_HomeContentPlaceHolder_hfTableID').value + '&Columnid=' 
                            + document.getElementById('ctl00_HomeContentPlaceHolder_hfColumnID').value;
                       document.getElementById('hfCalculationType').value = document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href;


                     document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced').href = '../Help/FormulaTest.aspx?type=exceedance&formula='
                        + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').value)
                        + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinExceedance').value)
                        + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxExceedance').value)
                        + '&Tableid=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_hfTableID').value)
                        + '&Columnid=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_hfColumnID').value); 

                     document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningAdvanced').href = '../Help/FormulaTest.aspx?type=warning&formula='
                        + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value)
                        + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinWaring').value)
                        + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxWrning').value)
                        + '&Tableid=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_hfTableID').value)
                        + '&Columnid=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_hfColumnID').value);   

                     document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced').href = '../Help/FormulaTest.aspx?type=valid&formula='
                        + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value)
                        + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinValid').value)
                        + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxValid').value)
                        + '&Tableid=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_hfTableID').value)
                        + '&Columnid=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_hfColumnID').value);     

                    document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = '../Help/CalculationTest.aspx?type=calculation&formula=' 
                        + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtCalculation').value) + ""&Tableid="" 
                        + document.getElementById(""ctl00_HomeContentPlaceHolder_hfTableID"").value + ""&Columnid="" 
                        + document.getElementById(""ctl00_HomeContentPlaceHolder_hfColumnID"").value;

  });
";

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);



        string strMaxHeight = "50";

        string strFilePath = _strFilesLocation + "/UserFiles/AppFiles/";
        string strScriptPath = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Handler.ashx";
        string strFileExtension = "*.jpg;*.gif;*.png";
        string strInnerHTML = "<img  title=\"Remove this image\" style=\"cursor:pointer;\"  id=\"dimg" + hfButtonValue.ID + "\" src=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath
            + "/App_Themes/Default/Images/icon_delete.gif\" />" +
            "<a id=\"a" + hfButtonValue.ID + "\" target=\"_blank\" >"
        + " <img style=\"padding-bottom:7px; max-height:"
        + strMaxHeight + "px;\" id=\"img" + hfButtonValue.ID + "\"  />" + "</a><br/>";

        string strJSPostBack = @" $(document).ready(function () {
                                        $('#" + fuButtonValue.ClientID + @"').uploadify({
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
                                               document.getElementById('" + hfButtonValue.ID + @"').value=jo.filename;

                                                $('#" + lblButtonValue.ID + @"').html('" + strInnerHTML + @"'); "
                                                    + "" + @"
                                                
                                                document.getElementById('img" + hfButtonValue.ID + @"').src=jo.fullpath;
                                                document.getElementById('img" + hfButtonValue.ID + @"').alt=fileObj.name;
                                                document.getElementById('img" + hfButtonValue.ID + @"').title=fileObj.name;
                                                document.getElementById('a" + hfButtonValue.ID + @"').href=jo.fullpath;

                                               

                                                  document.getElementById('dimg" + hfButtonValue.ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + hfButtonValue.ID + @"').value='';
                                                      $('#" + lblButtonValue.ID + @"').html(''); 
                                            });
                                               // alert(response);jo.fullpath
                   
                                            },
                                            'onSelect': function (event, ID, fileObj) {
                                                $('#" + lblButtonValue.ID + @"').html('');
                                                document.getElementById('" + hfButtonValue.ID + @"').value='';
                                                $('#" + fuButtonValue.ID + @"').uploadifySettings(
                                                'scriptData', { 'foo': 'UserFiles/AppFiles' }
                                                    );


                                                $('#" + fuButtonValue.ID + @"').uploadifyUpload();



                                            },
                                            'onCancel': function (event, ID, fileObj, data) {
                                               document.getElementById('" + hfButtonValue.ID + @"').value=''; " + "" + @"
                                               $('#" + lblButtonValue.ID + @"').html('');
                                            }

                                        });

                                          
                                    });";





        ScriptManager.RegisterStartupScript(this, this.GetType(), "strJSPostBack", strJSPostBack, true);


        if (!IsPostBack)
        {
            Session["DataReminder"] = null;
            Session["Condition"] = null;
            Session["DataReminderUser"] = null;
            Session["DataReminderCount"] = null;
            Session["dtShowWhen"] = null;
            Session["dtColumnColour"] = null;

            DataTable dtOptionImage = new DataTable();

            dtOptionImage.Columns.Add("OptionImageID");
            dtOptionImage.Columns.Add("Value");
            dtOptionImage.Columns.Add("FileName");
            dtOptionImage.Columns.Add("UniqueFileName");
            Session["OptionImage"] = dtOptionImage;
            BindOptionImage();

            //PopulateTable();
            //PopulateAnalyte();
            //txtMinWaring.Attributes.Add("onkeypress", "MakeAdvancedLink();");

            //this.lnkValidEdit.Attributes.Add("onclick", "javascript:return OpenValidPopup()");
            //this.lnkWarningEdit.Attributes.Add("onclick", "javascript:return OpenWarningPopup()");
            //this.lnkCalculationEdit.Attributes.Add("onclick", "javascript:return OpenCalculationPopup()");
            edtContent.AssetManager = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";
            edtContent.ButtonFeatures = new string[] { "XHTMLFullSource", "RemoveFormat", "Undo", "Redo", "|", "Paragraph", "FontName", "FontSize", "|", "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyFull", "Bold", "Italic", "Underline", "Hyperlink" };
        }


        GridViewRow gvr = gvOptionImage.TopPagerRow;
        if (gvr != null)
            _gvPager = (Common_Pager)gvr.FindControl("Pager");


        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;


                if (Request.QueryString["ColumnID"] != null)
                {
                    if (!IsPostBack)
                    {
                        ddDDDisplayColumnC.ServiceMethod = "GetColumns";
                    }


                    hlShowWhen.NavigateUrl = "~/Pages/Record/ShowHide.aspx?ColumnID=" + _qsColumnID;
                    hlColumnColour.NavigateUrl = "~/Pages/Record/ColumnColourList.aspx?ColumnID=" + _qsColumnID;

                    hfColumnID.Value = _qsColumnID;
                    _iColumnID = int.Parse(_qsColumnID);
                    Column theColumn = RecordManager.ets_Column_Details(int.Parse(hfColumnID.Value));
                    _theTable = RecordManager.ets_Table_Details((int)theColumn.TableID);





                    _qsTableID = _theTable.TableID.ToString();
                    Session["TableID"] = _qsTableID;
                    if (!IsPostBack)
                    {
                        if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                        {
                            gvChangedLog.PageSize = int.Parse(Session["GridPageSize"].ToString());
                        }
                        //BindTheChangedLogGrid(0, gvChangedLog.PageSize);
                    }

                    GridViewRow gvrCL = gvChangedLog.TopPagerRow;
                    if (gvrCL != null)
                        _gvCL_Pager = (Common_Pager)gvrCL.FindControl("CL_Pager");

                    //PopulateTheRecord();
                }
                if (Request.QueryString["TableID"] != null)
                {
                    _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"]);
                    hfTableID.Value = _qsTableID;
                    _theTable = RecordManager.ets_Table_Details(int.Parse(hfTableID.Value));
                    txtTable.Text = _theTable.TableName;
                    //ddlDDTableC.Category = _qsTableID;
                    Session["TableID"] = _qsTableID;


                }

                if (!IsPostBack)
                {
                    //PopulateRetriverDDL();



                    PopulateAvgColumn();
                    PopulateMapPinHoverColumn();

                    PopulateTLControllingField();
                    PopulateddlTLImage(ref ddlTLImage1);
                    PopulateddlTLImage(ref ddlTLImage2);
                    PopulateddlTLImage(ref ddlTLImage3);
                    PopulateddlTLImage(ref ddlTLImage4);
                    PopulateddlTLImage(ref ddlTLImage5);

                    PopulateTabs();
                    //PopulateCompareDDL();
                }

                if (_theTable.IsImportPositional == true)
                {
                    hfIsImportPositional.Value = "1";
                    lblImport.Text = "Position";

                    int iNext = RecordManager.ets_Table_MaxPosition((int)_theTable.TableID);
                    iNext = iNext + 1;
                    hfMaxPosition.Value = iNext.ToString();

                }
                else
                {
                    hfIsImportPositional.Value = "0";
                    lblImport.Text = "Heading";
                }

                if (!IsPostBack)
                {

                    hlBack.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Request.QueryString["typemode"] + "&AccountID=" + Request.QueryString["AccountID"] + "&MenuID=" + Request.QueryString["MenuID"] + "&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&SearchCriteria2=" + Request.QueryString["SearchCriteria2"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"] + "#topline";

                    if (Request.QueryString["signup"] != null)
                        hlBack.NavigateUrl = hlBack.NavigateUrl + "&signup=yes";


                    if (Request.QueryString["template"] != null)
                    {
                        if (Request.UrlReferrer != null)
                        {
                            hlBack.NavigateUrl = Request.UrlReferrer.ToString();
                        }

                    }


                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }
        }

        if (!IsPostBack)
        {
            PopulateFilterDDL();
            PopulateChildTables();

            hfTableID.Value = _theTable.TableID.ToString();
           if(Common.HaveAccess(Session["roletype"].ToString(), "1"))
           {
               hfGOD.Value = "yes";
           }
        }

        string strShowExceedances = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", _theTable.AccountID, _theTable.TableID);

        if (strShowExceedances != "" && strShowExceedances.ToLower() == "yes")
        {
            _bShowExceedances = true;
            hfShowExceedance.Value = "yes";
        }
        if (!IsPostBack && !Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            string strHideFormula = SystemData.SystemOption_ValueByKey_Account("Hide Formula", _theTable.AccountID, _theTable.TableID);

            if (strHideFormula != "" && strHideFormula.ToLower() == "yes")
            {
                hfHideFormula.Value = "yes";
            }
            string strHideConditions = SystemData.SystemOption_ValueByKey_Account("Hide Conditions", _theTable.AccountID, _theTable.TableID);

            if (strHideConditions != "" && strHideConditions.ToLower() == "yes")
            {
                hfHideConditions.Value = "yes";
            }
        }

        string strTitle = "Field Detail";

        switch (_strActionMode.ToLower())
        {
            case "add":
                strTitle = "Add Field";
                divDelete.Visible = false;

                if (!IsPostBack)
                {
                    string strTotalNonStdColumn = Common.GetValueFromSQL("SELECT COUNT(*) FROM [Column] WHERE IsStandard=0 AND TableID=" + _qsTableID);

                    if (strTotalNonStdColumn == "" || strTotalNonStdColumn == "0")
                    {
                        chkSummarySearch.Checked = true;
                    }



                    string strTotalNonStdSumColumn = Common.GetValueFromSQL("SELECT COUNT(*) FROM [Column] WHERE DisplayTextSummary is not null and  IsStandard=0 AND TableID=" + _qsTableID);

                    if (strTotalNonStdSumColumn == "")
                    {
                        chkSummaryPage.Checked = true;
                    }
                    else
                    {
                        if (int.Parse(strTotalNonStdSumColumn) < 5)
                        {
                            chkSummaryPage.Checked = true;
                        }
                    }

                }


                if (Request.QueryString["SearchCriteria2"] != null)
                {
                    try
                    {
                        SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria2"].ToString())));
                        if (theSearchCriteria != null)
                        {

                            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                            xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                            if (xmlDoc.FirstChild["ddlTableTabFilter"].InnerText != null)
                            {
                                //_iTableTabID =int.Parse( xmlDoc.FirstChild["ddlTableTabFilter"].InnerText);
                                ddlTableTab.SelectedValue = xmlDoc.FirstChild["ddlTableTabFilter"].InnerText;
                            }
                        }
                    }
                    catch
                    {
                        //
                    }
                }

                if (!IsPostBack)
                {
                    chkDetailPage.Checked = true;
                    chkImport.Checked = true;
                    chkExport.Checked = true;
                }
                divHistoryRoot.Visible = false;
                hlShowWhen.NavigateUrl = "~/Pages/Record/ShowHide.aspx?ColumnID=-1&TableID=" + _qsTableID;
                hlColumnColour.NavigateUrl = "~/Pages/Record/ColumnColourList.aspx?ColumnID=-1&TableID=" + _qsTableID;
                break;

            case "view":
                strTitle = "View Field";
                divDelete.Visible = false;
                PopulateTheRecord();

                EnableTheRecordControls(false);
                divSave.Visible = false;
                hlShowWhen.NavigateUrl = "~/Pages/Record/ShowHide.aspx?ColumnID=" + _qsColumnID + "&TableID=" + _qsTableID + "&mode=view";
                hlColumnColour.NavigateUrl = "~/Pages/Record/ColumnColourList.aspx?ColumnID=" + _qsColumnID + "&TableID=" + _qsTableID + "&mode=view";
                break;

            case "edit":
                hlShowWhen.NavigateUrl = "~/Pages/Record/ShowHide.aspx?ColumnID=" + _qsColumnID + "&TableID=" + _qsTableID;
                hlColumnColour.NavigateUrl = "~/Pages/Record/ColumnColourList.aspx?ColumnID=" + _qsColumnID + "&TableID=" + _qsTableID;
                divHistoryRoot.Visible = true;
                strTitle = "Edit Field";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


        }
        hfShowHref.Value = hlShowWhen.NavigateUrl.Replace("~/Pages/Record/", "");


        strTitle = strTitle.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        Title = strTitle;
        lblTitle.Text = strTitle;
        //divHistoryRoot.ActiveTabIndex = 0;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);

        //if (Request.QueryString["TableID"] != null)
        //{
        //    hlReminders.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminder.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + _qsColumnID;
        //    hlEditMappopup.NavigateUrl = "~/Pages/Content/MapPopup.aspx?TableID=" + Request.QueryString["TableID"].ToString();
        //}
        //else
        //{

        hlReminders.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminder.aspx?TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&ColumnID=" + _qsColumnID;
        hlEditMappopup.NavigateUrl = "~/Pages/Content/MapPopup.aspx?TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString());


        hlValidConditions.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/Condition.aspx?ConditionType=V&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&ColumnID=" + _qsColumnID;
        hlWarningConditions.NavigateUrl = "~/Pages/Help/Condition.aspx?ConditionType=W&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&ColumnID=" + _qsColumnID;
        hlExceedanceConditions.NavigateUrl = "~/Pages/Help/Condition.aspx?ConditionType=E&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) + "&ColumnID=" + _qsColumnID;


        //}

        if (!IsPostBack)
        {
            PopulateTerminology();
            PopulateGraphDefinition();
        }

    }


    protected void BindTheChangedLogGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
            int iTN = 0;

            //gvChangedLog.DataSource = RecordManager.ets_Record_Changes_Select(
            //        (int)_iRecordID,int.Parse(_qsTableID), iStartIndex, iMaxRows, ref  iTN, ref _iCLColumnCount);


            gvChangedLog.DataSource = RecordManager.Column_Audit_Summary(
                   (int)_iColumnID, iStartIndex, iMaxRows, ref  iTN);

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




        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Record Detail Change Log", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
        _gvCL_Pager.ExportFileName = "Record Field Change Log ";
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
                hlView.NavigateUrl = "RecordColumnAudit.aspx?UpdatedDate=" + Server.UrlEncode(dtUpdateDate.ToString("yyyy-MM-dd HH:mm:ss.fff")) + "&ColumnID=" + _iColumnID.ToString();

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




    protected void PutCheckBoxValues(string strDropdownValues)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            if (i == 0)
            {
                txtTickedValue.Text = s;
            }
            else if (i == 1)
            {
                txtUntickedValue.Text = s;
            }
            else
            {
                if (s.ToLower() == "yes")
                {
                    chkTickedByDefault.Checked = true;
                }
            }
            i = i + 1;
        }

    }

    protected void PopulateShowWhen()
    {

        DataTable dtTemp = RecordManager.dbg_ShowWhen_Select(int.Parse(_qsColumnID), null, null);

        if (dtTemp.Rows.Count == 0)
        {
            return;
        }

        if (dtTemp.Rows.Count > 0)
        {
            chkShowWhen.Checked = true;

        }


    }

    protected void PopulateColumnColour()
    {

        DataTable dtTemp = Cosmetic.dbg_ColumnColour_Select(int.Parse(_qsColumnID));

        if (dtTemp.Rows.Count == 0)
        {
            return;
        }

        if (dtTemp.Rows.Count > 0)
        {
            chkColumnColour.Checked = true;

        }


    }

    protected void PopulateTheRecord()
    {

        hlResetIDs.Visible = true;
        hlResetIDs.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                   Cryptography.Encrypt("Would you like to reallocate all records ID?")
                   + "&okbutton=" + Cryptography.Encrypt(btnResetIDsOK.ClientID);


        hlResetCalculations.Visible = true;
        hlResetCalculations.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                   Cryptography.Encrypt("Would you like to recalculate all records?")
                   + "&okbutton=" + Cryptography.Encrypt(btnResetCalValues.ClientID);

        hlResetValidation.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                 Cryptography.Encrypt("Would you like to revalidate all records?")
                 + "&okbutton=" + Cryptography.Encrypt(btnRevalidateRecords.ClientID);

        if (!IsPostBack)
        {
            if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
            {
                lnkCreateLookupTable.Visible = true;
                if (Session["CreateLookupTable" + hfColumnID.Value] != null)
                {
                    lnkCreateLookupTable.Visible = false;
                }
            }
        }



        ddDDLinkedParentColumnC.Category = "edit";
        ddlDefaultParentColumnC.Category = "edit";
        ddlCompareColumnIDC.Category = "edit";
        Column theColumn = RecordManager.ets_Column_Details(int.Parse(hfColumnID.Value));


        lblSystemName.Visible = true;
        lblSystemName.Text = "System Name: " + theColumn.SystemName;
        lblSystemName.ToolTip = theColumn.TableID.ToString() + " - " + theColumn.ColumnID.ToString();

        hfTableID.Value = theColumn.TableID.ToString();

        Session["Mappopup"] = theColumn.MapPopup;

        if (theColumn.CompareColumnID != null && theColumn.CompareOperator != "")
        {
            try
            {
                Column theCompareColumn = RecordManager.ets_Column_Details((int)theColumn.CompareColumnID);
                ddlCompareTableC.SelectedValue = theCompareColumn.TableID.ToString();
                ddlCompareColumnIDC.SelectedValue = theCompareColumn.ColumnID.ToString();
                //ddlCompareColumnID.SelectedValue = theColumn.CompareColumnID.ToString();
                ddlCompareOperator.SelectedValue = theColumn.CompareOperator;
                chkCompareOperator.Checked = true;
            }
            catch
            {

            }
        }

        if (!IsPostBack)
        {
            ViewState["OldFormulaV"] = GetOldFormula(theColumn, "V");
            DataTable dtConditons_V = UploadWorld.dbg_Condition_Select(theColumn.ColumnID, null, "V", "");
            if (dtConditons_V != null && dtConditons_V.Rows.Count > 0)
            {
                ViewState["OldFormulaV_C"] = dtConditons_V;
            }

            ViewState["OldFormulaE"] = GetOldFormula(theColumn, "E");
            ViewState["OldFormulaW"] = GetOldFormula(theColumn, "W");
        }
        //hlFiltered.NavigateUrl = "#";
        if (theColumn.TableTableID != null)
        {
            hfParentTableID.Value = theColumn.TableTableID.ToString();
        }
        if (theColumn.FilterParentColumnID != null)
        {
            hfFilterParentColumnID.Value = theColumn.FilterParentColumnID.ToString();
            chkFiltered.Checked = true;


        }
        if (theColumn.FilterOtherColumnID != null)
        {
            hfFilterOtherColumnID.Value = theColumn.FilterOtherColumnID.ToString();
        }

        if (theColumn.FilterValue != "")
        {
            hfFilterValue.Value = theColumn.FilterValue;
        }
        hfFilterOperator.Value = theColumn.FilterOperator == "" ? "equals" : theColumn.FilterOperator;

        DataTable dtRecordsFrom = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM [Record] WHERE (" + theColumn.SystemName + " IS NOT NULL OR " + theColumn.SystemName + "<>'') AND  IsActive=1 AND TableID=" + _theTable.TableID.ToString());

        if (dtRecordsFrom != null)
        {
            if (dtRecordsFrom.Rows.Count > 0)
            {

                lnkCreateTable.Visible = true;
            }
        }

        if (theColumn.OnlyForAdmin != null)
        {
            ddlOnlyForAdmin.Text = theColumn.OnlyForAdmin.ToString();
        }

        if (theColumn.TableTabID != null)
        {
            ddlTableTab.SelectedValue = theColumn.TableTabID.ToString();
        }

        if (theColumn.SPDefaultValue != "")
        {
            lblSPDefaultValue.Text = "Default:" + theColumn.SPDefaultValue;
        }

        txtTable.Text = theColumn.TableName;

        if (theColumn.TextType != "")
        {
            ddlTextType.Text = theColumn.TextType;

            if (theColumn.TextType == "own")
            {
                txtOwnRegEx.Text = theColumn.RegEx;
            }

        }

        //if (theColumn.SummaryCellBackColor != null)
        //    ddlSumXCellBackColour.Value = theColumn.SummaryCellBackColor;

        txtColumnName.Text = theColumn.DisplayName;

        if (theColumn.QuickAddLink != null)
        {

            chkQuickAddLink.Checked = (bool)theColumn.QuickAddLink;
        }
        //if (theColumn.ShowViewLink != null)
        //{
        //    chkShowViewLink.Checked = (bool)theColumn.ShowViewLink;
        //}
        ddlShowViewLink.SelectedValue = theColumn.ShowViewLink;
        if (theColumn.IsStandard == true)
        {
            txtColumnName.Enabled = true;
            //chkIsNumeric.Enabled = false;
            ddlType.Enabled = false;
            ddlDateTimeType.Enabled = false;

            if (theColumn.SystemName.ToLower() == "datetimerecorded")
            {
                //ddlType.Enabled = true;
                ddlDateTimeType.Enabled = true;
                ddlType.SelectedValue = "date_time";

                ddlDateTimeType.Items.Clear();
                //ListItem liTime=new ListItem("Time","time");
                ListItem liDate = new ListItem("Date", "date");
                ListItem liDateTime = new ListItem("Date & Time", "datetime");

                ddlDateTimeType.Items.Add(liDateTime);
                ddlDateTimeType.Items.Add(liDate);
                //ddlType.Items.Add(liTime);

            }
        }
        else
        {
            txtColumnName.Enabled = true;
            //chkIsNumeric.Enabled = true;
            ddlType.Enabled = true;
            ddlDateTimeType.Enabled = true;
        }


        if (theColumn.DisplayTextSummary != string.Empty)
        {
            chkSummaryPage.Checked = true;
            txtDisplayTextSummary.Text = theColumn.DisplayTextSummary;
            //txtViewName.Text = "";

        }

        if (theColumn.SummarySearch != null)
            chkSummarySearch.Checked = (bool)theColumn.SummarySearch;

        if (theColumn.DisplayTextDetail != string.Empty)
        {
            chkDetailPage.Checked = true;
            txtDisplayTextDetail.Text = theColumn.DisplayTextDetail;
        }

        if (theColumn.GraphLabel != string.Empty)
        {
            chkGraph.Checked = true;
            txtGraph.Text = theColumn.GraphLabel;
        }

        if (theColumn.SystemName.ToLower() == "datetimerecorded")
        {
            //txtColumnName.Enabled = true;
            lblColumnMessage.Text = "Note this field is used as the date on graphs";
            lblColumnMessage.Visible = true;
            ddlDateFormat.Text = _theTable.DateFormat;
        }
        if (theColumn.SystemName.ToLower() == "recordid")
        {
            //txtColumnName.Enabled = true;
            lblColumnMessage.Text = "System Field: Record ID";
            lblColumnMessage.Visible = true;

        }
        //if (theColumn.HideColumnID != null)
        //{
        //    chkShowWhen.Checked = true;
        //    hfHideColumnID.Value = theColumn.HideColumnID.ToString();
        //    hfHideColumnValue.Value = theColumn.HideColumnValue;
        //    hfHideColumnOperator.Value = theColumn.HideOperator;
        //}

        PopulateShowWhen();
        PopulateColumnColour();

        if (theColumn.ValidationCanIgnore != null)
            chkValidationCanIgnore.Checked = (bool)theColumn.ValidationCanIgnore;

        if (_theTable.IsImportPositional == true)
        {
            if (theColumn.PositionOnImport != null)
            {

                chkImport.Checked = true;
                if (theColumn.SystemName.ToLower() == "datetimerecorded")
                {

                    tblDateOptions.Style.Add("display", "block");
                    trDateFormat.Style.Add("display", "block");



                    if (theColumn.IsDateSingleColumn != null && theColumn.IsDateSingleColumn == true)
                    {
                        txtNameOnImport.Text = theColumn.PositionOnImport.ToString();
                        optSingle.Checked = true;
                        lblImport.Text = "Position";
                        lblImportTime.Text = "Time Position";
                    }
                    else
                    {
                        //txtNameOnImport.Text = theColumn.PositionOnImport.ToString() + "," + (int.Parse(theColumn.PositionOnImport.ToString()) + 1).ToString();
                        txtNameOnImport.Text = theColumn.PositionOnImport.ToString();
                        txtNameOnImportTime.Text = (int.Parse(theColumn.PositionOnImport.ToString()) + 1).ToString();
                        optDouble.Checked = true;
                        lblImport.Text = "Date Position";
                        lblImportTime.Text = "Time Position";
                    }


                }
                else
                {
                    //trDateFormat.Style.Add("display", "none");
                    txtNameOnImport.Text = theColumn.PositionOnImport.ToString();
                }
            }
        }
        else
        {
            if (theColumn.NameOnImport != string.Empty)
            {


                chkImport.Checked = true;
                if (theColumn.SystemName.ToLower() == "datetimerecorded")
                {
                    tblDateOptions.Style.Add("display", "block");
                    trDateFormat.Style.Add("display", "block");

                    string strDT = theColumn.NameOnImport;
                    if (strDT.IndexOf(",") > 0)
                    {
                        txtNameOnImport.Text = strDT.Substring(0, strDT.IndexOf(","));
                        txtNameOnImportTime.Text = strDT.Substring(strDT.IndexOf(",") + 1);
                        optDouble.Checked = true;
                        lblImport.Text = "Date Label";
                        lblImportTime.Text = "Time Label";
                    }
                    else
                    {
                        txtNameOnImport.Text = theColumn.NameOnImport;
                        txtNameOnImportTime.Text = "";
                        optSingle.Checked = true;
                        lblImport.Text = "Label";
                        lblImportTime.Text = "Time Label";
                    }


                }
                else
                {
                    //trDateFormat.Style.Add("display", "none");
                    txtNameOnImport.Text = theColumn.NameOnImport;
                }
            }
            else
            {
                lblImportTime.Text = "Time Label";
            }

        }

        //if (theColumn.ColumnType == "data_retriever")
        //{
        //    if (theColumn.DataRetrieverID != null)
        //    {
        //        if (ddlDataRetriver.Items.FindByValue(theColumn.DataRetrieverID.ToString()) != null)
        //            ddlDataRetriver.SelectedValue = theColumn.DataRetrieverID.ToString();
        //    }
        //}

        if (theColumn.Calculation != null)
        {
            if (theColumn.Calculation != "")
            {
                //chkCalculated.Checked = true;
            }

        }

        if (theColumn.NameOnExport != string.Empty)
        {
            chkExport.Checked = true;
            txtNameOnExport.Text = theColumn.NameOnExport;
        }

        if (theColumn.MobileName != string.Empty)
        {
            chkMobile.Checked = true;
            txtMobile.Text = theColumn.MobileName;
        }


        //validation formula
        if (theColumn.ColumnType == "number" || theColumn.ColumnType == "calculation")
        {
            DataTable dtConditons = UploadWorld.dbg_Condition_Select(theColumn.ColumnID, null, "", "");

            if (dtConditons != null && dtConditons.Rows.Count > 0)
            {
                //valid


                if (dtConditons.Rows.Count > 0)
                {
                    DataRow[] drConV = dtConditons.Select("ConditionType='V'");
                    if (drConV.Length > 0)
                    {
                        hlValidConditions.Text = "Conditions(" + drConV.Length + ")";
                        chkValidConditions.Checked = true;
                    }


                    DataRow[] drConW = dtConditons.Select("ConditionType='W'");
                    if (drConW.Length > 0)
                    {
                        hlWarningConditions.Text = "Conditions(" + drConW.Length + ")";
                        chkWarningConditions.Checked = true;
                    }


                    DataRow[] drConE = dtConditons.Select("ConditionType='E'");
                    if (drConE.Length > 0)
                    {
                        hlExceedanceConditions.Text = "Conditions(" + drConE.Length + ")";
                        chkExceedanceConditions.Checked = true;
                    }

                }





                //DataTable dtConV = dtConditons.Select("ConditionType='V'").CopyToDataTable();
                //DataTable dtConW = dtConditons.Select("ConditionType='W'").CopyToDataTable();
                //DataTable dtConE = dtConditons.Select("ConditionType='E'").CopyToDataTable();

                //if (dtConV != null && dtConV.Rows.Count > 0)
                //    chkValidConditions.Checked = true;

                //if (dtConW != null && dtConW.Rows.Count > 0)
                //    chkWarningConditions.Checked = true;

                //if (dtConE != null && dtConE.Rows.Count > 0)
                //    chkExceedanceConditions.Checked = true;
            }
        }




        if (theColumn.ValidationOnEntry != string.Empty && chkValidConditions.Checked == false)
        {

            //txtValidationEntry.Text = theColumn.ValidationOnEntry;
            ShowValidValidation(theColumn.ValidationOnEntry);
            chkValidFormula.Checked = true;
        }
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormulaEdit", "ShowValidMinMax('no');", true);
        //}

        if (theColumn.ValidationOnWarning != string.Empty && chkWarningConditions.Checked == false)
        {
            ShowWarningValidation(theColumn.ValidationOnWarning);
            chkWarningFormula.Checked = true;
        }

        if (theColumn.ValidationOnExceedance != string.Empty && chkExceedanceConditions.Checked == false)
        {
            ShowExceedanceValidation(theColumn.ValidationOnExceedance);
            chkExceedanceFormula.Checked = true;
        }

        txtConstant.Text = theColumn.Constant;


        txtCalculation.Text = Common.GetDisplayFromSystem(theColumn.Calculation, (int)theColumn.TableID);


        //chkMandatory.Checked = (bool)theColumn.IsMandatory;
        ddlImportance.SelectedValue = theColumn.Importance;
        if (theColumn.DefaultValue != "")
        {
            //chkDefaultValue.Checked = true;

            txtDefaultValue.Text = theColumn.DefaultValue;
            if (theColumn.ColumnType == "datetime" || theColumn.ColumnType == "date" || theColumn.ColumnType == "time")
            {
                txtDefaultValue.Text = "";
            }
        }

        if (theColumn.DefaultType != "")
        {
            //ddlDefaultValue.SelectedValue = theColumn.DefaultType;
            ddlDefaultValueC.SelectedValue = theColumn.DefaultType;
            hf_ddlDefaultValue.Value = theColumn.DefaultType;
            if (theColumn.DefaultType == "parent")
            {
                if (theColumn.DefaultColumnID != null)
                {
                    Column theDefaultColumn = RecordManager.ets_Column_Details((int)theColumn.DefaultColumnID);
                    if (theDefaultColumn != null)
                    {
                        ddlDefaultParentTableC.SelectedValue = theDefaultColumn.TableID.ToString();
                        ddlDefaultParentColumnC.SelectedValue = theColumn.DefaultColumnID.ToString();
                        if (theColumn.DefaultUpdateValues != null)
                        {
                            chkDefaultSyncData.Checked = (bool)theColumn.DefaultUpdateValues;
                        }

                    }

                }
            }
        }

        if (theColumn.FlatLineNumber != null)
        {
            chkFlatLine.Checked = true;
            txtFlatLine.Text = theColumn.FlatLineNumber.ToString();
        }

        if (theColumn.ColumnType != "")
        {
            //chkIsNumeric.Checked = (bool)theColumn.IsNumeric;





            if (theColumn.ColumnType == "datetime" || theColumn.ColumnType == "date")
            {
                DataTable dtTemp = Common.DataTableFromText(@"SELECT DataReminderID FROM DataReminder 
                    WHERE ColumnID=" + theColumn.ColumnID.ToString());

                if (dtTemp.Rows.Count > 0)
                {
                    //chkReminders.Checked = true;
                    hlReminders.Text = "Reminders(" + dtTemp.Rows.Count.ToString() + ")";
                }
                else
                {
                    hlReminders.Text = "Reminders";
                }

            }
            if (theColumn.ColumnType == "calculation")
            {
                chkCheckUnlikelyValue.Checked = (bool)theColumn.CheckUnlikelyValue;
                if (theColumn.TextType != "")
                {
                    ListItem liCalType = ddlCalculationType.Items.FindByValue(theColumn.TextType);
                    if (liCalType != null)
                        ddlCalculationType.SelectedValue = theColumn.TextType;

                    if (theColumn.TextType == "d")
                    {
                        if (theColumn.DateCalculationType != "")
                        {
                            ListItem liDateType = ddlDateResultFormat.Items.FindByValue(theColumn.DateCalculationType);
                            if (liDateType != null)
                                ddlDateResultFormat.SelectedValue = theColumn.DateCalculationType;
                        }
                    }
                }
                txtCalFinancialSymbol.Text = theColumn.RegEx;
            }

            if (theColumn.ColumnType == "datetime" || theColumn.ColumnType == "date"
                || theColumn.ColumnType == "time")
            {
                ddlType.Text = "date_time";
                ddlDateTimeType.Text = theColumn.ColumnType;

            }
            else if (theColumn.ColumnType == "number")
            {
                ddlType.Text = theColumn.ColumnType;
                chkIgnoreSymbols.Checked = (bool)theColumn.IgnoreSymbols;
                chkCheckUnlikelyValue.Checked = (bool)theColumn.CheckUnlikelyValue;

                if (theColumn.TextWidth != null)
                    txtTextWidth.Text = theColumn.TextWidth.ToString();

                if (theColumn.NumberType != null)
                {
                    if ((int)theColumn.NumberType == 6)
                    {
                        txtSymbol.Text = theColumn.TextType;
                    }
                }

                if (theColumn.NumberType != null)
                {
                    ddlNumberType.Text = theColumn.NumberType.ToString();

                    if (theColumn.NumberType == 7)
                    {
                        if (theColumn.DropdownValues != "")
                        {
                            try
                            {
                                SliderField theSliderField = JSONField.GetTypedObject<SliderField>(theColumn.DropdownValues);
                                if (theSliderField != null)
                                {
                                    if (theSliderField.Min != null)
                                        txtSliderMin.Text = theSliderField.Min.ToString();

                                    if (theSliderField.Max != null)
                                        txtSliderMax.Text = theSliderField.Max.ToString();
                                }
                            }
                            catch
                            {
                                //
                            }

                        }
                    }

                    if (theColumn.NumberType == 5)
                    {
                        if (theColumn.TableTableID != null)
                            ddlRecordCountTable.SelectedValue = theColumn.TableTableID.ToString();
                        if (theColumn.DropdownValues != "")
                            ddlRecordCountClick.SelectedValue = theColumn.DropdownValues;
                    }
                    if (theColumn.NumberType == 3)
                    {
                        if (theColumn.DateCalculationType != "")
                        {

                        }
                    }

                }
            }
            else if (theColumn.ColumnType == "text")
            {
                ddlType.Text = theColumn.ColumnType;
                if (theColumn.SystemName.ToString().ToLower() == "notes")
                {
                    txtTextWidth.Text = "27";
                    txtTextHeight.Text = "3";

                }

                if (theColumn.TextWidth != null)
                    txtTextWidth.Text = theColumn.TextWidth.ToString();

                if (theColumn.TextHeight != null)
                    txtTextHeight.Text = theColumn.TextHeight.ToString();

            }
            else if (theColumn.ColumnType == "radiobutton")
            {
                ddlType.Text = theColumn.ColumnType;
                ddlOptionType.Text = theColumn.DropDownType;

                if (theColumn.VerticalList != null)
                    chkDisplayVertical.Checked = (bool)theColumn.VerticalList;

                if (theColumn.DropDownType == "value_image")
                {
                    if (theColumn.ImageOnSummary != null)
                    {
                        chkImageOnSummary.Checked = (bool)theColumn.ImageOnSummary;
                        if (theColumn.TextHeight != null && chkImageOnSummary.Checked)
                        {
                            txtImageOnSummaryMaxHeight.Text = theColumn.TextHeight.ToString();
                        }
                        else
                        {
                            txtImageOnSummaryMaxHeight.Text = "";
                        }
                    }

                    if (theColumn.DropdownValues != "")
                    {
                        OptionImageList theOptionImageList = JSONField.GetTypedObject<OptionImageList>(theColumn.DropdownValues);

                        DataTable dtOptionImage = (DataTable)Session["OptionImage"];
                        dtOptionImage.Rows.Clear();
                        foreach (OptionImage aOptionImage in theOptionImageList.ImageList)
                        {
                            dtOptionImage.Rows.Add(aOptionImage.OptionImageID, aOptionImage.Value, aOptionImage.FileName, aOptionImage.UniqueFileName);
                        }
                        dtOptionImage.AcceptChanges();
                        Session["OptionImage"] = dtOptionImage;
                        BindOptionImage();
                    }

                }
                else
                {
                    txtDropdownValues.Text = theColumn.DropdownValues;
                }

            }

            else if (theColumn.ColumnType == "listbox")
            {
                ddlType.Text = theColumn.ColumnType;
                ddlListBoxType.Text = theColumn.DropDownType;
                txtDropdownValues.Text = theColumn.DropdownValues;


                if (theColumn.TextWidth != null)
                    txtTextWidth.Text = theColumn.TextWidth.ToString();

                if (theColumn.TextHeight != null)
                    txtTextHeight.Text = theColumn.TextHeight.ToString();



                if (theColumn.DateCalculationType == "checkbox")
                {
                    chkListCheckBox.Checked = true;
                }
                else
                {
                    chkListCheckBox.Checked = false;
                }

                if (theColumn.DropDownType == "table")
                {
                    ddlDDTableC.SelectedValue = theColumn.TableTableID.ToString();
                    hf_ddlDDTable.Value = theColumn.TableTableID.ToString();
                    ViewState["Old_TableTableID"] = theColumn.TableTableID.ToString();
                    hfDisplayColumnsFormula.Value = theColumn.DisplayColumn;
                    string strDisplayName = theColumn.DisplayColumn.Replace("[", "");
                    if (strDisplayName.IndexOf("[", 0) == -1)
                    {
                        //single Field
                        strDisplayName = strDisplayName.Replace("]", "");
                        strDisplayName = strDisplayName.Replace("'", "''");
                        DataTable dtTempSC = Common.DataTableFromText("SELECT * FROM [Column] WHERE   TableID=" + theColumn.TableTableID.ToString() + " AND DisplayName='" + strDisplayName + "'");

                        if (dtTempSC.Rows.Count > 0)
                        {
                            //ddDDDisplayColumn.Text = dtTempSC.Rows[0]["ColumnID"].ToString();
                            ddDDDisplayColumnC.SelectedValue = dtTempSC.Rows[0]["ColumnID"].ToString();
                        }

                    }
                    else
                    {
                        //advanced
                        //do nothing

                    }

                    if (theColumn.LinkedParentColumnID != null)
                        ddDDLinkedParentColumnC.SelectedValue = theColumn.LinkedParentColumnID.ToString();

                }

            }
            else if (theColumn.ColumnType == "checkbox")
            {
                ddlType.Text = theColumn.ColumnType;

                PutCheckBoxValues(theColumn.DropdownValues);
            }
            else if (theColumn.ColumnType == "location")
            {
                ddlType.Text = theColumn.ColumnType;

                if (theColumn.TextHeight != null && theColumn.TextWidth != null)
                {
                    txtMapHeight.Text = theColumn.TextHeight.ToString();
                    txtMapWidth.Text = theColumn.TextWidth.ToString();
                    chkShowMap.Checked = true;
                }
                else
                {
                    chkShowMap.Checked = false;
                }



                if (theColumn.MapPinHoverColumnID != null)
                {
                    try
                    {
                        ddlMapPinHoverColumnID.SelectedValue = theColumn.MapPinHoverColumnID.ToString();
                    }
                    catch
                    {

                    }
                }




                if (theColumn.ShowTotal != null)
                {
                    if ((bool)theColumn.ShowTotal)
                    {
                        chkLocationAddress.Checked = true;
                    }
                    else
                    {
                        chkLocationAddress.Checked = false;
                    }

                }
                if (theColumn.IsRound != null)
                {
                    if ((bool)theColumn.IsRound)
                    {
                        chkLatLong.Checked = true;
                    }
                    else
                    {
                        chkLatLong.Checked = false;
                    }
                }

            }
            else if (theColumn.ColumnType == "staticcontent" || theColumn.ColumnType == "content")
            {
                ddlType.Text = "staticcontent";
                edtContent.Text = theColumn.DropdownValues;
                if (theColumn.ColumnType == "content")
                    chkAllowContenEdit.Checked = true;
            }
            else if (theColumn.ColumnType == "trafficlight")
            {
                ddlType.Text = theColumn.ColumnType;
                if (theColumn.TrafficLightColumnID != null && theColumn.TrafficLightValues != "")
                {
                    ddlTLControllingField.SelectedValue = theColumn.TrafficLightColumnID.ToString();
                    ddlTLControllingField_SelectedIndexChanged(null, null);
                    string strXML = theColumn.TrafficLightValues;

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(strXML);

                    XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

                    DataSet ds = new DataSet();
                    ds.ReadXml(r);

                    if (ds.Tables[0] != null)
                    {
                        int i = 1;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            if (i == 1)
                            {
                                cbcValue1.TableID = int.Parse(_qsTableID);
                                cbcValue1.ddlYAxisV = ddlTLControllingField.SelectedValue;
                                cbcValue1.TextValue = dr[0].ToString();
                                ddlTLImage1.SelectedValue = dr[1].ToString();
                            }

                            if (i == 2)
                            {
                                lnkAddTL1.Visible = false;
                                trTLValueLight2.Visible = true;
                                cbcValue2.TableID = int.Parse(_qsTableID);
                                cbcValue2.ddlYAxisV = ddlTLControllingField.SelectedValue;
                                cbcValue2.TextValue = dr[0].ToString();
                                ddlTLImage2.SelectedValue = dr[1].ToString();
                            }

                            if (i == 3)
                            {
                                lnkAddTL2.Visible = false;
                                trTLValueLight3.Visible = true;
                                cbcValue3.TableID = int.Parse(_qsTableID);
                                cbcValue3.ddlYAxisV = ddlTLControllingField.SelectedValue;
                                cbcValue3.TextValue = dr[0].ToString();
                                ddlTLImage3.SelectedValue = dr[1].ToString();
                            }
                            if (i == 4)
                            {
                                lnkAddTL3.Visible = false;
                                trTLValueLight4.Visible = true;
                                cbcValue4.TableID = int.Parse(_qsTableID);
                                cbcValue4.ddlYAxisV = ddlTLControllingField.SelectedValue;
                                cbcValue4.TextValue = dr[0].ToString();
                                ddlTLImage4.SelectedValue = dr[1].ToString();
                            }

                            if (i == 5)
                            {
                                lnkAddTL4.Visible = false;
                                trTLValueLight5.Visible = true;
                                cbcValue5.TableID = int.Parse(_qsTableID);
                                cbcValue5.ddlYAxisV = ddlTLControllingField.SelectedValue;
                                cbcValue5.TextValue = dr[0].ToString();
                                ddlTLImage5.SelectedValue = dr[1].ToString();
                            }
                            i = i + 1;
                        }
                    }

                }
            }
            else if (theColumn.ColumnType == "dropdown")
            {
                ddlType.Text = theColumn.ColumnType;


                if (theColumn.TextWidth != null)
                    txtTextWidth.Text = theColumn.TextWidth.ToString();

                if (theColumn.DropDownType == "values"
                    || theColumn.DropDownType == "value_text")
                {
                    txtDropdownValues.Text = theColumn.DropdownValues;
                    ddlDDType.Text = theColumn.DropDownType;
                }
                else
                {
                    //table                    



                    ddlDDTableC.SelectedValue = theColumn.TableTableID.ToString();


                    hf_ddlDDTable.Value = theColumn.TableTableID.ToString();

                    ViewState["Old_TableTableID"] = theColumn.TableTableID.ToString();

                    hfDisplayColumnsFormula.Value = theColumn.DisplayColumn;
                    string strDisplayName = theColumn.DisplayColumn.Replace("[", "");
                    if (strDisplayName.IndexOf("[", 0) == -1)
                    {
                        //single Field
                        strDisplayName = strDisplayName.Replace("]", "");
                        strDisplayName = strDisplayName.Replace("'", "''");
                        DataTable dtTempSC = Common.DataTableFromText("SELECT * FROM [Column] WHERE   TableID=" + theColumn.TableTableID.ToString() + " AND DisplayName='" + strDisplayName + "'");

                        if (dtTempSC.Rows.Count > 0)
                        {
                            //ddDDDisplayColumn.Text = dtTempSC.Rows[0]["ColumnID"].ToString();
                            ddDDDisplayColumnC.SelectedValue = dtTempSC.Rows[0]["ColumnID"].ToString();
                        }

                    }
                    else
                    {
                        //advanced
                        //do nothing

                    }

                    if (theColumn.LinkedParentColumnID != null)
                        ddDDLinkedParentColumnC.SelectedValue = theColumn.LinkedParentColumnID.ToString();

                    if (theColumn.DefaultColumnID != null)
                        ddlDefaultParentColumnC.SelectedValue = theColumn.DefaultColumnID.ToString();

                    if (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd")
                    {
                        ddlDDType.Text = "ct";
                        if (theColumn.DropDownType == "table")
                        {

                            chkPredictive.Checked = true;
                        }
                        if (theColumn.ParentColumnID != null)
                        {
                            //chkFilterValues.Checked = true;
                            ddlDDType.Text = "lt";
                            ddlFilterC.SelectedValue = theColumn.ParentColumnID.ToString();

                        }

                    }
                }
            }
            else
            {
                ddlType.Text = theColumn.ColumnType;

            }
        }
        else
        {
            ddlType.Text = "text";
        }

        if (theColumn.DisplayRight != null && theColumn.DisplayRight == true)
            chkDisplayOnRight.Checked = true;

        if (theColumn.AvgNumberOfRecords != null)
        {
            txtAvgNumValues.Text = theColumn.AvgNumberOfRecords.ToString();
        }

        if (theColumn.ColumnType == "image")
        {
            if (theColumn.TextWidth != null)
                txtImageHeightSummary.Text = theColumn.TextWidth.ToString();

            if (theColumn.TextHeight != null)
                txtImageHeightDetail.Text = theColumn.TextHeight.ToString();
        }

        //if (theColumn.ShowTotal != null)
        //{
        //    chkShowTotal.Checked = (bool)theColumn.ShowTotal;
        //}
        if (theColumn.IsRound != null)
        {
            chkRound.Checked = (bool)theColumn.IsRound;
        }
        txtRoundNumber.Text = theColumn.RoundNumber.ToString();
        txtNotes.Text = theColumn.Notes;

        if (theColumn.ShowGraphExceedance != null)
        {
            chkExceedence.Checked = true;
            txtExceedence.Text = theColumn.ShowGraphExceedance.ToString();
        }

        if (theColumn.ShowGraphWarning != null)
        {
            chkWarning.Checked = true;
            txtWarning.Text = theColumn.ShowGraphWarning.ToString();
        }

        if (theColumn.MaxValueAt != null)
        {
            chkMaximumValueat.Checked = true;
            txtMaximumValueat.Text = theColumn.MaxValueAt.ToString();
        }

        if (theColumn.DefaultGraphDefinitionID != null)
        {
            if (ddlDefaultGraphDefinition.Items.FindByValue(theColumn.DefaultGraphDefinitionID.ToString()) != null)
                ddlDefaultGraphDefinition.SelectedValue = theColumn.DefaultGraphDefinitionID.ToString();
        }


        //if (theColumn.Alignment == "")
        //{
        //    ddlAlignment.Text = "-1";
        //}
        //else
        //{
        //    ddlAlignment.Text = theColumn.Alignment;
        //}

        //if (theColumn.DropdownValues != "")
        //{
        //    chkDropdownValues.Checked = true;
        //    txtDropdownValues.Text = theColumn.DropdownValues;

        //}

        if (theColumn.ColumnType == "button")
        {
            ddlType.Text = theColumn.ColumnType;
            if (theColumn.TextWidth != null)
                txtTextWidth.Text = theColumn.TextWidth.ToString();

            if (theColumn.TextHeight != null)
                txtTextHeight.Text = theColumn.TextHeight.ToString();

            if (theColumn.ButtonInfo != "")
            {
                try
                {
                    ColumnButtonInfo theButtonInfo = JSONField.GetTypedObject<ColumnButtonInfo>(theColumn.ButtonInfo);
                    if (theButtonInfo != null)
                    {
                        if (!string.IsNullOrEmpty(theButtonInfo.SPToRun))
                        {
                            chkSPToRun.Checked = true;
                            txtSPToRun.Text = theButtonInfo.SPToRun;
                        }
                        if (!string.IsNullOrEmpty(theButtonInfo.OpenLink))
                        {
                            chkButtonOpenLink.Checked = true;
                            txtButtonOpenLink.Text = theButtonInfo.OpenLink;
                        }
                        if (!string.IsNullOrEmpty(theButtonInfo.ImageFullPath))
                        {
                            //txtButtonImage.Text = theButtonInfo.ImageFullPath;

                            hfButtonValue.Value = theButtonInfo.ImageFullPath;


                            string strMaxHeight = "50";
                            string strFilePath = _strFilesLocation + "/UserFiles/AppFiles/"
                                    + theButtonInfo.ImageFullPath;
                            lblButtonValue.Text = "<a target='_blank' href='" + strFilePath + "'>"
                                + "<img style='padding-bottom:7px; max-height:" + strMaxHeight + "px;' alt='" + theButtonInfo.ImageFullPath.Substring(37)
                                + "' src='" + strFilePath + "' title='" + theButtonInfo.ImageFullPath.Substring(37) + "'  />" + "</a><br/>";

                            lblButtonValue.Text = "<img title=\"Remove this image\" style=\"cursor:pointer;\"  id=\"dimg" + hfButtonValue.ID + "\" src=\"" + Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath
                              + "/App_Themes/Default/Images/icon_delete.gif\" />" + lblButtonValue.Text;

                            // hfButtonValue.Value = theButtonInfo.ImageFullPath;

                            string strTempJS = @"  document.getElementById('dimg" + hfButtonValue.ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + hfButtonValue.ID + @"').value='';
                                                      $('#" + lblButtonValue.ID + @"').html(''); 
                                            });";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "imagedelete", strTempJS, true);


                        }

                        if (!string.IsNullOrEmpty(theButtonInfo.WarningMessage))
                        {
                            chkButtonWarningMessage.Checked = true;
                            txtButtonWarningMessage.Text = theButtonInfo.WarningMessage;
                        }

                    }
                }
                catch
                {
                    //
                }

            }

        }


        if (_strActionMode == "edit")
        {
            ViewState["theColumn"] = theColumn;

            if (theColumn.IsStandard == true)
            {
                divDelete.Visible = false;
            }

        }
        else if (_strActionMode == "view")
        {
            divEdit.Visible = true;

            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlEditLink.NavigateUrl = "~/Pages/Record/RecordColumnDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&typemode=" + Request.QueryString["typemode"] + "&MenuID=" + Request.QueryString["MenuID"] + "&ColumnID=" + Cryptography.Encrypt(theColumn.ColumnID.ToString()) + "&SearchCriteria2=" + Request.QueryString["SearchCriteria2"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"];
            }
            else
            {
                hlEditLink.NavigateUrl = "~/Pages/Record/RecordColumnDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&typemode=" + Request.QueryString["typemode"] + "&MenuID=" + Request.QueryString["MenuID"] + "&ColumnID=" + Cryptography.Encrypt(theColumn.ColumnID.ToString()) + "&SearchCriteria2=" + Request.QueryString["SearchCriteria2"];
            }

            if (Request.QueryString["signup"] != null)
                hlEditLink.NavigateUrl = hlEditLink.NavigateUrl + "&signup=yes";

        }

        if (theColumn.AvgColumnID != null)
        {
            try
            {
                ddlAvgColumn.Text = theColumn.AvgColumnID.ToString();
            }
            catch
            {

            }
        }

        if (theColumn.SystemName.ToLower() == "notes")
        {
            hfColumnSystemname.Value = "notes";
        }

        if (theColumn.SystemName.ToLower() == "recordid")
        {
            chkSummaryPage.Enabled = false;
            //txtDisplayTextSummary.Enabled = false;
            ////chkDetailPage.Enabled = false;           
            //txtDisplayTextDetail.Enabled = false;
            hfColumnSystemname.Value = "recordid";

            chkImport.Enabled = false;
            txtNameOnImport.Enabled = false;

            //chkExport.Enabled = false;

            //txtNameOnExport.Enabled = false;


            chkGraph.Enabled = false;
            txtGraph.Enabled = false;


            //chkValidationEntry.Enabled = false;
            txtValidationOnWarning.Enabled = false;
            txtValidationOnExceedance.Enabled = false;
            //divSave.Visible = false;
            //divEdit.Visible = false;
        }
        if (theColumn.SystemName.ToLower() == "tableid")
        {
            divSave.Visible = false;
            divEdit.Visible = false;
        }
        if (theColumn.SystemName.ToLower() == "isactive")
        {
            chkSummaryPage.Enabled = false;
            txtDisplayTextSummary.Enabled = false;
            //txtViewName.Enabled = false;
            chkDetailPage.Enabled = false;
            chkImport.Enabled = false;
            chkExport.Enabled = false;
            txtDisplayTextDetail.Enabled = false;
            txtGraph.Enabled = false;
            txtNameOnImport.Enabled = false;
            txtNameOnExport.Enabled = false;
            //chkValidationEntry.Enabled = false;
            txtValidationOnWarning.Enabled = false;
            txtValidationOnExceedance.Enabled = false;
            divSave.Visible = false;
            divEdit.Visible = false;
        }
        //if (theColumn.SystemName.ToLower() == "locationid")
        //{
        //    //chkDetailPage.Checked = true;
        //    //chkDetailPage.Enabled = false;
        //    chkGraph.Checked = true;
        //    chkGraph.Enabled = false;

        //    hfColumnSystemname.Value = "locationid";

        //    lblColumnMessage.Text = "Note this Field is used for geographic positioning";
        //    lblColumnMessage.Visible = true;
        //}
        if (theColumn.SystemName.ToLower() == "datetimerecorded")
        {
            //chkDetailPage.Checked = true;
            //chkDetailPage.Enabled = false;
            chkGraph.Checked = true;
            chkGraph.Enabled = false;
            //chkMobile.Checked = true;
            //chkMobile.Enabled = false;


            hfDateTimeColumn.Value = "yes";

            txtColumnName.Enabled = false;
            //chkMandatory.Enabled = false;
            ddlImportance.Enabled = false;
            ddlType.Enabled = false;
            //chkDefaultValue.Enabled = false;
            ddlDateTimeType.Enabled = false;

        }
        if (theColumn.SystemName.ToLower() == "enteredby")
        {
            chkImport.Checked = false;
            chkImport.Enabled = false;
            //chkDetailPage.Checked = false;
            //chkDetailPage.Enabled = false;

            hfColumnSystemname.Value = "enteredby";
            txtColumnName.Enabled = false;

        }


        if (Request.QueryString["template"] != null)
        {
            divEdit.Visible = false;
            divHistoryRoot.Visible = false;
            divSave.Visible = false;
            divDelete.Visible = false;

        }

        if (theColumn.IsSystemColumn)
        {
            divDelete.Visible = false;
            txtColumnName.Enabled = false;
            //divColumnType.Disabled = true;
            //divColumnType.Attributes.Add("Disabled", "");

            chkDetailPage.Enabled = false;
            chkShowWhen.Enabled = false;
            chkColumnColour.Enabled = false;
            ddlOnlyForAdmin.Enabled = false;

            ddlType.Enabled = false;
            txtTickedValue.Enabled = false;
            txtUntickedValue.Enabled = false;
            chkTickedByDefault.Enabled = false;
            ddlDateTimeType.Enabled = false;
            ddlTextType.Enabled = false;
            txtOwnRegEx.Enabled = false;
            ddlNumberType.Enabled = false;
            txtConstant.Enabled = false;


            //chkFilterValues.Enabled = false;
            chkPredictive.Enabled = false;
            txtDropdownValues.Enabled = false;
            ddlDDTable.Enabled = false;
            ddDDDisplayColumn.Enabled = false;
            ddDDLinkedParentColumn.Enabled = false;
            ddlDefaultParentColumn.Enabled = false;
            ddlDefaultParentTable.Enabled = false;

            ddlFilter.Enabled = false;
            ddlRecordCountTable.Enabled = false;
            ddlRecordCountClick.Enabled = false;
            ddlAvgColumn.Enabled = false;
            txtAvgNumValues.Enabled = false;
            chkRound.Enabled = false;
            txtRoundNumber.Enabled = false;
            txtTextHeight.Enabled = false;
            txtTextWidth.Enabled = false;
            //chkMandatory.Enabled = false;
            ddlImportance.Enabled = false;
            //chkShowTotal.Enabled = false;
            chkIgnoreSymbols.Enabled = false;
            chkCheckUnlikelyValue.Enabled = false;
            //chkDefaultValue.Enabled = false;
            txtDefaultValue.Enabled = false;
            //chkReminders.Enabled = false;
            chkFlatLine.Enabled = false;
            txtFlatLine.Enabled = false;
            ddlDDType.Enabled = false;
            hfIsSystemColumn.Value = "yes";

        }

        if (theColumn.TableTableID != null && theColumn.TableTableID == -1)
        {
            if (theColumn.DisplayColumn == "[Name]")
            {
                ddDDDisplayColumnC.SelectedValue = "name";
            }
        }


        string strSFTID = SystemData.SystemOption_ValueByKey_Account("Standardised_Field_Table", _theTable.AccountID, _theTable.TableID);

        if (!IsPostBack)
        {

            if (strSFTID != "" && !Common.HaveAccess(Session["roletype"].ToString(), "1") && _theTable != null)
            {
                txtColumnName.Enabled = false;
                divDelete.Visible = false;
            }

        }

        if (_bIsAccountHolder == false)
        {
            divDelete.Visible = false;
            if (_CurrentRole != null && _CurrentRole.RoleType == "2")
            {
                if (_CurrentUserRole != null && _CurrentUserRole.AllowDeleteColumn != null && (bool)_CurrentUserRole.AllowDeleteColumn)
                {
                    divDelete.Visible = true;
                }
            }

        }

        //if (_bIsAccountHolder == false)
        //{
        //txtColumnName.Enabled = false;
        //ddlType.Enabled = false;
        //ddlDDType.Enabled = false;
        //divDelete.Visible = false;
        //}


    }


    protected void lnkAddTL1_Click(object sender, EventArgs e)
    {
        trTLValueLight2.Visible = true;
        lnkAddTL1.Visible = false;
        lnkAddTL2.Visible = true;
        if (ddlTLControllingField.SelectedValue != "")
        {
            cbcValue2.TableID = int.Parse(_qsTableID);
            cbcValue2.ddlYAxisV = ddlTLControllingField.SelectedValue;
        }
    }

    protected void lnkAddTL2_Click(object sender, EventArgs e)
    {

        trTLValueLight3.Visible = true;
        lnkAddTL2.Visible = false;
        lnkAddTL3.Visible = true;
        if (ddlTLControllingField.SelectedValue != "")
        {
            cbcValue3.TableID = int.Parse(_qsTableID);
            cbcValue3.ddlYAxisV = ddlTLControllingField.SelectedValue;
        }
    }


    protected void lnkAddTL3_Click(object sender, EventArgs e)
    {
        trTLValueLight4.Visible = true;
        lnkAddTL3.Visible = false;
        lnkAddTL4.Visible = true;
        if (ddlTLControllingField.SelectedValue != "")
        {
            cbcValue4.TableID = int.Parse(_qsTableID);
            cbcValue4.ddlYAxisV = ddlTLControllingField.SelectedValue;
        }
    }

    protected void lnkAddTL4_Click(object sender, EventArgs e)
    {
        trTLValueLight5.Visible = true;
        lnkAddTL4.Visible = false;

        if (ddlTLControllingField.SelectedValue != "")
        {
            cbcValue5.TableID = int.Parse(_qsTableID);
            cbcValue5.ddlYAxisV = ddlTLControllingField.SelectedValue;
        }

    }


    protected void lnkMinusTL2_Click(object sender, EventArgs e)
    {
        lnkAddTL1.Visible = true;
        trTLValueLight2.Visible = false;
        cbcValue2.ddlYAxisV = "";

    }

    protected void lnkMinusTL3_Click(object sender, EventArgs e)
    {

        lnkAddTL2.Visible = true;

        trTLValueLight3.Visible = false;
        cbcValue3.ddlYAxisV = "";
    }

    protected void lnkMinusTL4_Click(object sender, EventArgs e)
    {
        lnkAddTL3.Visible = true;
        trTLValueLight4.Visible = false;
        cbcValue4.ddlYAxisV = "";
    }

    protected void lnkMinusTL5_Click(object sender, EventArgs e)
    {
        lnkAddTL4.Visible = true;
        trTLValueLight5.Visible = false;
        cbcValue5.ddlYAxisV = "";
    }


    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(_qsColumnID));
            if (theColumn != null)
            {
                if (theColumn.IsSystemColumn)
                {
                    lblMsg.Text = "You cannot delete a system field!";
                    return;
                }

                if (ViewState["Old_TableTableID"] != null)
                {

                    //check if parent table has record count Field

                    DataTable dtNumberOfLinked = Common.DataTableFromText(@"SELECT DisplayName FROM [Column] WHERE TableID="
                        + theColumn.TableID.ToString() + " AND TableTableID="
                        + ViewState["Old_TableTableID"].ToString() + " AND ColumnType='dropdown' ");

                    if (dtNumberOfLinked.Rows.Count > 1)
                    {
                        //do nothing
                    }
                    else
                    {

                        DataTable dtParentColumn = Common.DataTableFromText(@"SELECT DisplayName FROM [Column] WHERE TableID="
                            + ViewState["Old_TableTableID"].ToString()
                            + " AND TableTableID=" + theColumn.TableID.ToString() + " AND ColumnType='number' AND NumberType=5");


                        if (dtParentColumn.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please remove the Record Count field " + dtParentColumn.Rows[0][0].ToString() + " link from the parent table and then try again.');", true);
                            return;
                        }
                    }
                }


                //Common.ExecuteText("UPDATE Record SET " + theColumn.SystemName + "=NULL WHERE TableID=" + theColumn.TableID.ToString());

                RecordManager.ets_Column_Delete(int.Parse(_qsColumnID));
                //ViewManager.dbg_View_ResetViewItems((int)theColumn.TableID);
                Session["tdbmsg"] = "Column(" + txtColumnName.Text + ") has been Deleted";
                Response.Redirect(hlBack.NavigateUrl, false);
            }

        }
        catch (Exception ex)
        {
            if (ex.Message.IndexOf("FK_SensorMonitor_Column") > 0)
            {
                lblMsg.Text = "This field is used for sensor monitor, please remove the sensor monitor reference then try again to delete.";
            }
            else if (ex.Message.IndexOf("is used for calculation") > 0)
            {
                lblMsg.Text = ex.Message + ".Please delete this columm from calculation formula and try again.";
            }
            else if (ex.Message.IndexOf("FK_Column_Column") > 0)
            {
                lblMsg.Text = "This field is used in another field for average, please remove the reference and then try again to delete.";
            }
            else
            {

                lblMsg.Text = "Delete failed! Please try again.";
            }
        }


    }

    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtSymbol.Enabled = p_bEnable;
        ddlOnlyForAdmin.Enabled = p_bEnable;
        //chkReminders.Enabled = p_bEnable;
        //hlReminders.Enabled = p_bEnable;

        txtTable.Enabled = p_bEnable;
        txtColumnName.Enabled = p_bEnable;
        txtDisplayTextSummary.Enabled = p_bEnable;
        //txtViewName.Enabled = p_bEnable;
        txtDisplayTextDetail.Enabled = p_bEnable;
        txtGraph.Enabled = p_bEnable;
        txtNameOnExport.Enabled = p_bEnable;
        txtNameOnImport.Enabled = p_bEnable;
        txtValidationEntry.Enabled = p_bEnable;
        txtValidationOnWarning.Enabled = p_bEnable;
        txtValidationOnExceedance.Enabled = p_bEnable;
        optSingle.Enabled = p_bEnable;
        optDouble.Enabled = p_bEnable;
        txtNameOnImportTime.Enabled = p_bEnable;

        chkExceedence.Enabled = p_bEnable;
        chkWarning.Enabled = p_bEnable;
        txtExceedence.Enabled = p_bEnable;
        txtWarning.Enabled = p_bEnable;

        chkMaximumValueat.Enabled = p_bEnable;
        txtMaximumValueat.Enabled = p_bEnable;

        ddlDefaultGraphDefinition.Enabled = p_bEnable;

        if (txtCalculation.Visible)
        {
            txtCalculation.Enabled = p_bEnable;
        }

        chkDetailPage.Enabled = p_bEnable;
        chkSummaryPage.Enabled = p_bEnable;
        chkExport.Enabled = p_bEnable;
        chkImport.Enabled = p_bEnable;
        //chkIsNumeric.Enabled = p_bEnable;
        ddlType.Enabled = p_bEnable;
        ddlDateTimeType.Enabled = p_bEnable;
        ddlNumberType.Enabled = p_bEnable;
        txtAvgNumValues.Enabled = p_bEnable;

        chkMobile.Enabled = p_bEnable;
        txtMobile.Enabled = p_bEnable;
        txtTickedValue.Enabled = p_bEnable;
        txtUntickedValue.Enabled = p_bEnable;
        chkTickedByDefault.Enabled = p_bEnable;


        if (txtConstant.Visible)
        {
            txtConstant.Enabled = p_bEnable;
        }

        hlCalculationEdit.Enabled = p_bEnable;
        //hlValidEdit.Enabled = p_bEnable;
        //hlWarningEdit.Enabled = p_bEnable;
        //hlExceedanceEdit.Enabled = p_bEnable;

        txtMaxWrning.Enabled = p_bEnable;
        txtMinWaring.Enabled = p_bEnable;
        hlWarningAdvanced.Enabled = p_bEnable;
        hlExceedanceAdvanced.Enabled = p_bEnable;

        txtMaxValid.Enabled = p_bEnable;
        txtMinValid.Enabled = p_bEnable;
        hlValidAdvanced.Enabled = p_bEnable;



        if (chkIgnoreSymbols.Visible)
        {
            chkIgnoreSymbols.Enabled = p_bEnable;
        }
        if (chkCheckUnlikelyValue.Visible)
        {
            chkCheckUnlikelyValue.Enabled = p_bEnable;
        }

        //if (chkShowTotal.Visible)
        //{
        //    chkShowTotal.Enabled = p_bEnable;
        //}
        txtNotes.Enabled = p_bEnable;
        //ddlAlignment.Enabled = p_bEnable;
        if (chkRound.Visible)
        {
            chkRound.Enabled = p_bEnable;
            txtRoundNumber.Enabled = p_bEnable;
        }
        //if (chkCalculated.Visible)
        //{
        //    chkCalculated.Enabled = p_bEnable;
        //}
        chkGraph.Enabled = p_bEnable;
        if (txtGraph.Visible)
        {
            txtGraph.Enabled = p_bEnable;
        }
        //chkValidationEntry.Enabled = p_bEnable;

        //if (chkDropdownValues.Checked)
        //    txtDropdownValues.Enabled = p_bEnable;

        //chkDropdownValues.Enabled = p_bEnable;
        //chkMandatory.Enabled = p_bEnable;
        ddlImportance.Enabled = p_bEnable;
        txtDefaultValue.Enabled = p_bEnable;
        //chkDefaultValue.Enabled = p_bEnable;
        ddlAvgColumn.Enabled = p_bEnable;

        chkFlatLine.Enabled = p_bEnable;
        txtFlatLine.Enabled = p_bEnable;

        ddlDateFormat.Enabled = p_bEnable;
        chkDisplayOnRight.Enabled = p_bEnable;
        txtTextHeight.Enabled = p_bEnable;
        txtTextWidth.Enabled = p_bEnable;
        ddlDDType.Enabled = p_bEnable;
        //ddlSumXCellBackColour.Disabled = !p_bEnable;
        //ddlDDTable.Enabled = p_bEnable;
        //ddlDDTableC.Enabled = p_bEnable;
        ddlDDTable.Enabled = p_bEnable;
        ddDDDisplayColumn.Enabled = p_bEnable;
        ddDDLinkedParentColumn.Enabled = p_bEnable;

        ddlFilter.Enabled = p_bEnable;
        ddlTextType.Enabled = p_bEnable;
        txtOwnRegEx.Enabled = p_bEnable;
        ddlRecordCountTable.Enabled = p_bEnable;
        ddlRecordCountClick.Enabled = p_bEnable;
        edtContent.Enabled = p_bEnable;
        chkShowWhen.Enabled = p_bEnable;
        chkColumnColour.Enabled = p_bEnable;
        txtImageHeightDetail.Enabled = p_bEnable;
        txtImageHeightSummary.Enabled = p_bEnable;
        chkLocationAddress.Enabled = p_bEnable;
        chkLatLong.Enabled = p_bEnable;

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action

        if (ddlType.SelectedValue == "listbox" && ddlListBoxType.SelectedValue == "table" && ddlDDTable.SelectedValue != "")
        {
            string strRecordCount = Common.GetValueFromSQL("SELECT COUNT(RecordID) FROM [Record] WHERE IsActive=1 AND TableID=" + ddlDDTable.SelectedValue);

            if (strRecordCount == "")
                strRecordCount = "0";

            if (int.Parse(strRecordCount) > Common.MaxRowForListBoxTable)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "listboax_maxrecord", "alert('The table(" + ddlDDTable.SelectedItem.Text.Replace("'", "") + ") you have selected is not valid as it has over " + Common.MaxRowForListBoxTable.ToString() + " rows.');", true);

                return false;
            }

        }

        if (txtColumnName.Text == "")
        {
            lblMsg.Text = "Field Name Can't be empty!";
            txtColumnName.Focus();
            return false;
        }
        else
        {
            if (txtColumnName.Text.IndexOf(",") > -1)
            {

                lblMsg.Text = "Field Name can not have comma (,)!";
                txtColumnName.Focus();
                return false;
            }
        }

        if (txtDisplayTextSummary.Text != "")
        {
            if (txtDisplayTextSummary.Text.IndexOf(",") > -1)
            {
                lblMsg.Text = "Summary Page Heading can not have comma (,)!";
                txtDisplayTextSummary.Focus();
                return false;
            }

        }
        //if (txtViewName.Text != "")
        //{
        //    if (txtViewName.Text.IndexOf(",") > -1)
        //    {
        //        lblMsg.Text = "Summary Page view name can not have comma (,)!";
        //        txtViewName.Focus();
        //        return false;
        //    }

        //}

        //if (txtDisplayTextDetail.Text != "")
        //{
        //    if (txtDisplayTextDetail.Text.IndexOf(",") > -1)
        //    {
        //        lblMsg.Text = "Detail Page Label can not have comma (,)!";
        //        txtDisplayTextDetail.Focus();
        //        return false;
        //    }

        //}

        if (txtGraph.Text != "")
        {
            if (txtGraph.Text.IndexOf(",") > -1)
            {
                lblMsg.Text = "Graph Label can not have comma (,)!";
                txtGraph.Focus();
                return false;
            }

        }

        if (txtDropdownValues.Text != "" && (ddlDDType.SelectedValue == "values" && ddlType.SelectedValue == "dropdown"))
        {
            if (txtDropdownValues.Text.IndexOf(",") > -1)
            {
                lblMsg.Text = "List can not have comma (,)!";
                txtDropdownValues.Focus();
                return false;
            }

        }

        if (txtDropdownValues.Text != "" && (ddlDDType.SelectedValue == "value_text"
            && ddlType.SelectedValue == "dropdown"))
        {

            string[] result = txtDropdownValues.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in result)
            {
                if (s.IndexOf(",") == -1)
                {
                    lblMsg.Text = "For Value & Text Dropdown you must enter the value, comma(,) and text on each and every line.";
                    txtDropdownValues.Focus();
                    return false;
                }

            }


        }

        if (txtDropdownValues.Text != "" && (ddlOptionType.SelectedValue == "values" && ddlType.SelectedValue == "radiobutton"))
        {
            if (txtDropdownValues.Text.IndexOf(",") > -1)
            {
                lblMsg.Text = "List can not have comma (,)!";
                txtDropdownValues.Focus();
                return false;
            }

        }

        if (txtDropdownValues.Text != "" && (ddlListBoxType.SelectedValue == "values" && ddlType.SelectedValue == "listbox"))
        {
            if (txtDropdownValues.Text.IndexOf(",") > -1)
            {
                lblMsg.Text = "List can not have comma (,)!";
                txtDropdownValues.Focus();
                return false;
            }

        }


        if (txtDropdownValues.Text != "" && (ddlOptionType.SelectedValue == "value_text"
            && ddlType.SelectedValue == "radiobutton"))
        {

            string[] result = txtDropdownValues.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in result)
            {
                if (s.IndexOf(",") == -1)
                {
                    lblMsg.Text = "For Value & Text Radio Buttons you must enter the value, comma(,) and text on each and every line.";
                    txtDropdownValues.Focus();
                    return false;
                }

            }


        }


        if (txtDropdownValues.Text != "" && (ddlListBoxType.SelectedValue == "value_text"
            && ddlType.SelectedValue == "listbox"))
        {

            string[] result = txtDropdownValues.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in result)
            {
                if (s.IndexOf(",") == -1)
                {
                    lblMsg.Text = "For Value & Text Listbox you must enter the value, comma(,) and text on each and every line.";
                    txtDropdownValues.Focus();
                    return false;
                }

            }


        }



        if (txtColumnName.Text != "")
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT DisplayName FROM  [Column] WHERE   TableID=" + hfTableID.Value
                + " AND ColumnID!=" + hfColumnID.Value + " AND DisplayName='" + txtColumnName.Text.Replace("'", "''") + "'");
            if (dtTemp.Rows.Count > 0)
            {
                lblMsg.Text = "'" + txtColumnName.Text + "' name is already used in '" + dtTemp.Rows[0][0].ToString() + "' Field Name, please try another name or change '" + dtTemp.Rows[0][0].ToString() + "' field.";
                txtColumnName.Focus();
                return false;
            }

        }

        if (txtDisplayTextSummary.Text != "")
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT DisplayName FROM  [Column] WHERE   TableID=" + hfTableID.Value
                + " AND ColumnID!=" + hfColumnID.Value + " AND DisplayTextSummary='" + txtDisplayTextSummary.Text.Replace("'", "''") + "'");
            if (dtTemp.Rows.Count > 0)
            {
                lblMsg.Text = "'" + txtDisplayTextSummary.Text + "' name is already used in '" + dtTemp.Rows[0][0].ToString() + "' Summary Page Heading, please try another name or change '" + dtTemp.Rows[0][0].ToString() + "' field.";
                txtDisplayTextSummary.Focus();
                return false;
            }

        }

        //if (txtDisplayTextDetail.Text != "")
        //{
        //    DataTable dtTemp = Common.DataTableFromText("SELECT DisplayName FROM  [Column] WHERE   TableID=" + hfTableID.Value
        //        + " AND ColumnID!=" + hfColumnID.Value + " AND DisplayTextDetail='" + txtDisplayTextDetail.Text.Replace("'", "''") + "'", null, null);
        //    if (dtTemp.Rows.Count > 0)
        //    {
        //        lblMsg.Text = "'" + txtDisplayTextDetail.Text + "' name is already used in '" + dtTemp.Rows[0][0].ToString() + "' Detail Page Label, please try another name or change '" + dtTemp.Rows[0][0].ToString() + "' field.";
        //        txtDisplayTextDetail.Focus();
        //        return false;
        //    }

        //}




        if (txtNameOnImport.Text != "")
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT DisplayName FROM  [Column] WHERE   TableID=" + hfTableID.Value
                + " AND ColumnID!=" + hfColumnID.Value + " AND NameOnImport='" + txtNameOnImport.Text.Replace("'", "''") + "'");
            if (dtTemp.Rows.Count > 0)
            {
                lblMsg.Text = "'" + txtNameOnImport.Text + "' name is already used in '" + dtTemp.Rows[0][0].ToString() + "' Import Label, please try another name or change '" + dtTemp.Rows[0][0].ToString() + "' field.";
                txtNameOnImport.Focus();
                return false;
            }

        }

        if (txtNameOnExport.Text != "")
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT DisplayName FROM  [Column] WHERE   TableID=" + hfTableID.Value
                + " AND ColumnID!=" + hfColumnID.Value + " AND NameOnExport='" + txtNameOnExport.Text.Replace("'", "''") + "'");
            if (dtTemp.Rows.Count > 0)
            {
                lblMsg.Text = "'" + txtNameOnExport.Text + "' name is already used in '" + dtTemp.Rows[0][0].ToString() + "' Export Label, please try another name or change '" + dtTemp.Rows[0][0].ToString() + "' field.";
                txtNameOnExport.Focus();
                return false;
            }

        }


        if (txtGraph.Text != "")
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT DisplayName FROM  [Column] WHERE   TableID=" + hfTableID.Value
                + " AND ColumnID!=" + hfColumnID.Value + " AND GraphLabel='" + txtGraph.Text.Replace("'", "''") + "'");
            if (dtTemp.Rows.Count > 0)
            {
                lblMsg.Text = "'" + txtGraph.Text + "' name is already used in '" + dtTemp.Rows[0][0].ToString() + "' Graph Label, please try another name or change '" + dtTemp.Rows[0][0].ToString() + "' field.";
                txtGraph.Focus();
                return false;
            }

        }




        //if (txtNameOnImport.Text != "")
        //{
        //    if (txtNameOnImport.Text.IndexOf(",") > -1)
        //    {
        //        //Column editColumn = (Column)ViewState["theColumn"];
        //        if (txtColumnName.Text =="Date Time Recorded" && _theTable.IsImportPositional == true)
        //        {

        //        }
        //        else
        //        {
        //            lblMsg.Text = "Import Name/Position can not have comma (,)!";
        //            txtNameOnImport.Focus();
        //            return false;
        //        }
        //    }

        //}

        if (txtNameOnExport.Text != "")
        {
            if (txtNameOnExport.Text.IndexOf(",") > -1)
            {
                lblMsg.Text = "Export Name can not have comma (,)!";
                txtNameOnExport.Focus();
                return false;
            }

        }

        if (txtConstant.Text != "")
        {
            //if (chkIsNumeric.Checked)
            //{

            if (ddlNumberType.Text == "2")
            {
                try
                {
                    double dTest = double.Parse(txtConstant.Text.Trim());
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Default value need to be numeric!";
                    txtConstant.Focus();
                    return false;
                }
            }

            //}

        }

        //if (chkDropdownValues.Checked)
        //{
        //    if (txtDropdownValues.Text == "")
        //    {
        //        lblMsg.Text = "Please enter dropdown value.";
        //        txtDropdownValues.Focus();
        //        return false;
        //    }

        //}

        if (txtMinWaring.Text.Trim() != "" && txtMaxWrning.Text.Trim() != "")
        {
            try
            {
                if (double.Parse(txtMinWaring.Text) > double.Parse(txtMaxWrning.Text))
                {
                    lblMsg.Text = "Warning Max value should be greater than Min value.";
                    txtMaxWrning.Focus();
                    return false;
                }
            }
            catch
            {
            }
        }

        if (txtMinValid.Text.Trim() != "" && txtMaxValid.Text.Trim() != "")
        {
            try
            {
                if (double.Parse(txtMinValid.Text) > double.Parse(txtMaxValid.Text))
                {
                    lblMsg.Text = "Validation Max value should be greater than Min value.";
                    txtMaxValid.Focus();
                    return false;
                }
            }
            catch
            {
            }
        }

        if (ddlType.SelectedValue == "calculation")
        {
            if (txtCalculation.Text.Trim() == "")
            {
                lblMsg.Text = "You have not entered the Calculation for a Calculated field.";
                txtCalculation.Focus();
                return false;

            }

        }


        return true;

    }


    public string GetValidValidation()
    {
        string strFullFormula = "";
        if (chkValidConditions.Checked == false)
        {
            //if(chkValidFormula.Checked)
            strFullFormula = Common.GetFullFormula(txtMinValid.Text.Trim(), txtMaxValid.Text.Trim(), txtValidationEntry.Text.Trim());

            if (strFullFormula != "")
                RemoveFromCondition("V");
        }

        return strFullFormula;


        //if (txtMinValid.Text.Trim() != "" || txtMaxValid.Text.Trim() != "")
        //{
        //    string strFormula = "";
        //    if (txtMinValid.Text.Trim() != "")
        //    {
        //        strFormula = "value>=" + txtMinValid.Text.Trim();
        //    }

        //    if (txtMaxValid.Text.Trim() != "")
        //    {
        //        strFormula = "value<=" + txtMaxValid.Text.Trim();
        //    }

        //    if (txtMinValid.Text.Trim() != ""
        //    && txtMaxValid.Text.Trim() != "")
        //    {
        //        strFormula = "value>=" + txtMinValid.Text.Trim() + " AND " + "value<=" + txtMaxValid.Text.Trim();
        //    }

        //    return strFormula;

        //}
        //else
        //{
        //    return txtValidationEntry.Text.Trim();
        //}

        //return "";
    }


    public string GetWarningValidation()
    {
        string strFullFormula = "";
        if (chkWarningConditions.Checked == false)
        {
            //if(chkWarningFormula.Checked)
            strFullFormula = Common.GetFullFormula(txtMinWaring.Text.Trim(), txtMaxWrning.Text.Trim(), txtValidationOnWarning.Text.Trim());

            if (strFullFormula != "")
                RemoveFromCondition("W");
        }

        return strFullFormula;


        //if (txtMinWaring.Text.Trim() != "" || txtMaxWrning.Text.Trim() != "")
        //{
        //    string strFormula = "";
        //    if (txtMinWaring.Text.Trim() != "")
        //    {
        //        strFormula = "value>" + txtMinWaring.Text.Trim();
        //    }

        //    if (txtMaxWrning.Text.Trim() != "")
        //    {
        //        strFormula = "value<" + txtMaxWrning.Text.Trim();
        //    }

        //    if (txtMinWaring.Text.Trim() != ""
        //    && txtMaxWrning.Text.Trim() != "")
        //    {
        //        strFormula = "value>" + txtMinWaring.Text.Trim() + " AND " + "value<" + txtMaxWrning.Text.Trim();
        //    }

        //    return strFormula;

        //}
        //else
        //{
        //    return txtValidationOnWarning.Text.Trim();
        //}

        //return "";
    }

    protected void RemoveFromCondition(string strConditionType)
    {
        if (Session["Condition"] != null)
        {
            DataTable dtCon = (DataTable)Session["Condition"];
            var rows = dtCon.Select("ConditionType='" + strConditionType + "'");
            foreach (var row in rows)
                row.Delete();

            dtCon.AcceptChanges();
            Session["Condition"] = dtCon;
        }
        else
        {
            if (_iColumnID > 0)
            {
                Common.ExecuteText("DELETE FROM [Condition] WHERE ColumnID=" + _iColumnID.ToString() + " AND  ConditionType='" + strConditionType + "'");
            }
        }

    }
    public string GetExceedanceValidation()
    {
        string strFullFormula = "";
        if (chkExceedanceConditions.Checked == false)
        {
            // if(chkExceedanceFormula.Checked)
            strFullFormula = Common.GetFullFormula(txtMinExceedance.Text.Trim(), txtMaxExceedance.Text.Trim(), txtValidationOnExceedance.Text.Trim());

            if (strFullFormula != "")
                RemoveFromCondition("E");
        }


        return strFullFormula;
        //if (txtMinExceedance.Text.Trim() != "" || txtMaxExceedance.Text.Trim() != "")
        //{
        //    string strFormula = "";
        //    if (txtMinExceedance.Text.Trim() != "")
        //    {
        //        strFormula = "value>" + txtMinExceedance.Text.Trim();
        //    }

        //    if (txtMaxExceedance.Text.Trim() != "")
        //    {
        //        strFormula = "value<" + txtMaxExceedance.Text.Trim();
        //    }

        //    if (txtMinExceedance.Text.Trim() != ""
        //    && txtMaxExceedance.Text.Trim() != "")
        //    {
        //        strFormula = "value>" + txtMinExceedance.Text.Trim() + " AND " + "value<" + txtMaxExceedance.Text.Trim();
        //    }

        //    return strFormula;

        //}
        //else
        //{
        //    return txtValidationOnExceedance.Text.Trim();
        //}

        //return "";
    }
    public void ShowWarningValidation(string strFormula)
    {
        //string strMin = "";
        //string strMax = "";
        //strMin = Common.GetMinFromFormula(strFormula);
        //strMax = Common.GetMaxFromFormula(strFormula);

        //int iTotalValue = Common.GetNumberOfValue(strFormula);
        bool bAdvanced = false;

        //if (iTotalValue > 2)
        //{

        //    bAdvanced = true;
        //}
        //else
        //{
        //    if (iTotalValue > 1)
        //    {
        //        if (strMin != "" && strMax != "")
        //        {

        //        }
        //        else
        //        {

        //            bAdvanced = true;
        //        }
        //    }
        //    else
        //    {
        //        if (strMin != "" || strMax != "")
        //        {

        //        }
        //        else
        //        {

        //            bAdvanced = true;
        //        }

        //    }
        //}


        Common.ShowFromula(strFormula, ref txtMinWaring, ref txtMaxWrning, ref txtValidationOnWarning, ref bAdvanced);


        if (bAdvanced)
        {
            //txtValidationOnWarning.Text = strFormula;
            hfShowWarningMinMax.Value = "no";
            //lblWarningValidation.Text = "Data Warning if";
        }
        else
        {
            //txtMinWaring.Text = strMin;
            //txtMaxWrning.Text = strMax;
            hfShowWarningMinMax.Value = "yes";
            //lblWarningValidation.Text = "Data Warning if outside the range";
        }

    }


    public void ShowExceedanceValidation(string strFormula)
    {
        //string strMin = "";
        //string strMax = "";
        //strMin = Common.GetMinFromFormula(strFormula);
        //strMax = Common.GetMaxFromFormula(strFormula);

        //int iTotalValue = Common.GetNumberOfValue(strFormula);
        bool bAdvanced = false;

        //if (iTotalValue > 2)
        //{

        //    bAdvanced = true;
        //}
        //else
        //{
        //    if (iTotalValue > 1)
        //    {
        //        if (strMin != "" && strMax != "")
        //        {

        //        }
        //        else
        //        {

        //            bAdvanced = true;
        //        }
        //    }
        //    else
        //    {
        //        if (strMin != "" || strMax != "")
        //        {

        //        }
        //        else
        //        {

        //            bAdvanced = true;
        //        }

        //    }
        //}


        Common.ShowFromula(strFormula, ref txtMinExceedance, ref txtMaxExceedance, ref txtValidationOnExceedance, ref bAdvanced);
        if (bAdvanced)
        {
            //txtValidationOnExceedance.Text = strFormula;
            hfShowExceedanceMinMax.Value = "no";
            //lblExceedanceValidation.Text = "Data Exceedance if";
        }
        else
        {
            //txtMinExceedance.Text = strMin;
            //txtMaxExceedance.Text = strMax;
            hfShowExceedanceMinMax.Value = "yes";
            //lblExceedanceValidation.Text = "Data Exceedance if outside the range";
        }

    }
    public void ShowValidValidation(string strFormula)
    {
        //string strMin = "";
        //string strMax = "";
        //strMin = Common.GetMinVaue(strFormula);
        //strMax = Common.GetMaxVaue(strFormula);

        //int iTotalValue = Common.GetNumberOfValue(strFormula);
        bool bAdvanced = false;

        //if (iTotalValue > 2)
        //{

        //    bAdvanced = true;
        //}
        //else
        //{
        //    if (iTotalValue > 1)
        //    {
        //        if (strMin != "" && strMax != "")
        //        {

        //        }
        //        else
        //        {

        //            bAdvanced = true;
        //        }
        //    }
        //    else
        //    {
        //        if (strMin != "" || strMax != "")
        //        {

        //        }
        //        else
        //        {

        //            bAdvanced = true;
        //        }

        //    }
        //}


        Common.ShowFromula(strFormula, ref txtMinValid, ref txtMaxValid, ref txtValidationEntry, ref bAdvanced);


        if (bAdvanced)
        {
            //txtValidationEntry.Text = strFormula;
            hfShowValidMinMax.Value = "no";
            //lblValidationEntry.Text = "Data Valid if";
        }
        else
        {
            //txtMinValid.Text = strMin;
            //txtMaxValid.Text = strMax;
            hfShowValidMinMax.Value = "yes";
            //lblValidationEntry.Text = "Data Valid Between";
        }

    }



    protected void btnResetCalValues_Click(object sender, EventArgs e)
    {
        try
        {
            string strErrors = "";
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(hfColumnID.Value));
            // theColumn.Calculation = txtCalculation.Text.Trim();
            string strResult = RecordManager.ets_AdjustCalculationFormulaChanges(theColumn, ref strErrors, true);

            if (strResult != "error")
            {

                Session["tdbmsg"] = strResult;
                Response.Redirect(Request.RawUrl, false);
                //if(strResult=="")
                //{
                //    hlResetCalculations.Text = "All records value are recalculated.";
                //    hlResetCalculations.ForeColor = System.Drawing.Color.Green;
                //}
                //else
                //{
                //    hlResetCalculations.Text ="Done! "+ strResult;
                //    hlResetCalculations.ForeColor = System.Drawing.Color.Green;
                //}
            }
        }
        catch
        {
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "btnResetCalValues_ClickJS", "alert('Please try again.');", true);
            hlResetCalculations.Text = "Error.Please try again.";
            hlResetCalculations.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnRevalidateRecords_Click(object sender, EventArgs e)
    {
        try
        {
            string strInvalidRecordIDs = "";
            string strValidRecordIDs = "";
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(hfColumnID.Value));
            RecordManager.ets_AdjustValidFormulaChanges(theColumn, ref strInvalidRecordIDs, ref strValidRecordIDs, true, "");
            string strNotifications = "";
            if (strInvalidRecordIDs != "")
            {
                strNotifications = TheDatabase.GetAdjustValidationNotification(strNotifications, strInvalidRecordIDs, strValidRecordIDs, theColumn);
            }
            else
            {
                strNotifications="Done!";
            }

            Session["tdbmsgpb"] = strNotifications;
        }
        catch
        {
            //
        }
       
    }
    protected void btnValidateRecordsOK_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["perform_validation"] = "yes";
            lnkSave_Click(null, null);
        }
        catch
        {
            //
        }

    }

    protected void btnValidateRecordsNO_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["perform_validation"] = "no";
            lnkSave_Click(null, null);
        }
        catch
        {
            //
        }

    }
    protected void btnResetIDsOK_Click(object sender, EventArgs e)
    {
        try
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(hfColumnID.Value));
            SystemData.spResetIDs((int)_theTable.TableID, theColumn.SystemName, false);
            hlResetIDs.Text = "All IDs are reallocated.";
            hlResetIDs.ForeColor = System.Drawing.Color.Green;
        }
        catch
        {
            //
        }


        //ScriptManager.RegisterStartupScript(this, this.GetType(), "btnResetIDsOK_ClickJS", "alert('All IDs are reallocated.');", true);

    }
    protected void btnRefreshRminder_Click(object sender, EventArgs e)
    {
        if (Session["DataReminderCount"] != null)
        {
            int iDataReminderCount = (int)Session["DataReminderCount"];
            if (iDataReminderCount > 0)
            {
                //chkReminders.Checked = true;
                hlReminders.Text = "Reminders(" + iDataReminderCount.ToString() + ")";
            }
            else
            {
                hlReminders.Text = "Reminders";
            }

        }
    }

    protected void btnOptionImage_Click(object sender, EventArgs e)
    {
        if (Session["OptionImage"] != null)
        {
            BindOptionImage();
        }
    }

    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)


    protected void lnkCreateLookupTable_Click(object sender, EventArgs e)
    {
        try
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(hfColumnID.Value));
            User theAccountHolder = SecurityManager.User_AccountHolder((int)_theTable.AccountID);

            if (theAccountHolder == null)
                theAccountHolder = _ObjUser;

            string strColumn_DisplayName = txtColumnName.Text.Replace("'", "''");
            Table newParentTable = new Table(null, strColumn_DisplayName, null, null, false, true);
            newParentTable.AccountID = _theTable.AccountID;
            newParentTable.PinImage = _theTable.PinImage;

            int? iParentMenuID = null;
            Menu theMenu = RecordManager.ets_Menu_By_TableID((int)_theTable.TableID);

            if (theMenu != null)
                iParentMenuID = theMenu.ParentMenuID;

            DataTable dtParentData = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM [Record] WHERE (" + theColumn.SystemName + " IS NOT NULL OR " + theColumn.SystemName + "<>'') AND  IsActive=1 AND TableID=" + _theTable.TableID.ToString());



            try
            {

                newParentTable.TableID = RecordManager.ets_Table_Insert(newParentTable, iParentMenuID);

                int? iDisplayOrder = RecordManager.ets_Table_MaxOrder((int)newParentTable.TableID);

                if (iDisplayOrder == null)
                    iDisplayOrder = -1;

                string strParentjoinfieldSystemName = "";

                strParentjoinfieldSystemName = RecordManager.ets_Column_NextSystemName((int)newParentTable.TableID);


                Column newParentjoinfield = new Column(null, newParentTable.TableID,
                      strParentjoinfieldSystemName, iDisplayOrder + 1, theColumn.DisplayName, theColumn.DisplayName, "", "", null, "",
                       "", null, null, "", "", false, theColumn.DisplayName, null, "", null, null, false, "", "", "");

                newParentjoinfield.ColumnType = "text";

                newParentjoinfield.ColumnID = RecordManager.ets_Column_Insert(newParentjoinfield);

                //insert parent data
                foreach (DataRow dr in dtParentData.Rows)
                {
                    Common.AddOneSysRecord((int)newParentTable.TableID, (int)theAccountHolder.UserID, newParentjoinfield.SystemName, dr[0].ToString());
                }


                //create backup column

                iDisplayOrder = RecordManager.ets_Table_MaxOrder((int)theColumn.TableID);

                if (iDisplayOrder == null)
                    iDisplayOrder = -1;

                string strBUSystemName = "";

                strBUSystemName = RecordManager.ets_Column_NextSystemName((int)theColumn.TableID);


                Column newBUColumn = new Column(null, theColumn.TableID,
                      strBUSystemName, iDisplayOrder + 1, theColumn.DisplayName + " Orig", theColumn.DisplayName + " Orig", "", "", null, "",
                       "", null, null, "", "", false, theColumn.DisplayName + " Orig", null, "", null, null, false, "", "", "");

                newBUColumn.ColumnType = "text";

                newBUColumn.ColumnID = RecordManager.ets_Column_Insert(newBUColumn);

                Common.ExecuteText("UPDATE [Record] SET " + newBUColumn.SystemName + "=" + theColumn.SystemName + " WHERE TableID=" + theColumn.TableID.ToString());

                //backup done
                theColumn.ColumnType = "dropdown";
                theColumn.DropDownType = "tabledd";

                string strLinkedParentColumn = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE SystemName='recordid' AND TableID=" + newParentTable.TableID.ToString());
                theColumn.TableTableID = newParentTable.TableID;

                if (strLinkedParentColumn != "")
                    theColumn.LinkedParentColumnID = int.Parse(strLinkedParentColumn);

                theColumn.DisplayColumn = "[" + newParentjoinfield.DisplayName + "]";
                theColumn.ParentColumnID = null;
                theColumn.FilterParentColumnID = null;
                theColumn.FilterOtherColumnID = null;
                theColumn.FilterValue = "";


                RecordManager.ets_Column_Update(theColumn);


                DataTable dtTemp = Common.DataTableFromText("SELECT * FROM TableChild WHERE ParentTableID=" + theColumn.TableTableID.ToString() + " AND ChildTableID=" + theColumn.TableID.ToString() + " AND DetailPageType='list'");

                if (dtTemp.Rows.Count > 0)
                {
                    //do nothing as we have this relationgship
                }
                else
                {

                    TableChild theTableChild = new TableChild(null, theColumn.TableTableID,
                        theColumn.TableID, "", "list");
                    theTableChild.ShowAddButton = true;
                    theTableChild.ShowEditButton = true;
                    RecordManager.ets_TableChild_Insert(theTableChild);
                }


                SystemData.spUpdateLinkedTables((int)newParentTable.TableID, newParentjoinfield.SystemName, (int)theColumn.TableID,
                    newBUColumn.SystemName, theColumn.SystemName, "", "");



                Session["CreateLookupTable" + hfColumnID.Value] = "done";
                Response.Redirect(Request.RawUrl, false);
                return;
            }
            catch (Exception ex)
            {
                //

                lblMsg.Text = ex.Message + "--->>" + ex.StackTrace;
            }



        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem_lookupTable", "alert('Sorry! We could not create the lookup table, please try again.');", true);
        }

    }
    protected void lnkCreateTable_Click(object sender, EventArgs e)
    {
        //lets create the table


        try
        {


            Column theColumn = RecordManager.ets_Column_Details(int.Parse(hfColumnID.Value));

            if (theColumn != null)
            {

                if (ddlDDType.SelectedValue == "ct" || ddlDDType.SelectedValue == "lt")
                {
                    if (SecurityManager.IsRecordsExceeded((int)_theTable.AccountID))
                    {
                        Session["DoNotAllow"] = "true";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DoNotAllow", "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);

                        return;
                    }


                    Table newTable = new Table(null, _theTable.TableName + txtColumnName.Text.Replace("'", "''"), null, null, false, true);
                    newTable.AccountID = _theTable.AccountID;
                    newTable.PinImage = _theTable.PinImage;

                    int? iParentMenuID = null;
                    Menu theMenu = RecordManager.ets_Menu_By_TableID((int)_theTable.TableID);


                    if (theMenu != null)
                        iParentMenuID = theMenu.ParentMenuID;

                    int iTableID = RecordManager.ets_Table_Insert(newTable, iParentMenuID);

                    //now lets create the column


                    int? iDisplayOrder = RecordManager.ets_Table_MaxOrder(iTableID);

                    if (iDisplayOrder == null)
                        iDisplayOrder = -1;

                    string strAutoSystemName = "";

                    strAutoSystemName = RecordManager.ets_Column_NextSystemName(iTableID);

                    Column newColumn = new Column(null, iTableID,
                      strAutoSystemName, iDisplayOrder + 1, theColumn.DisplayName, theColumn.DisplayName, "", "", null, "",
                       "", null, null, "", "", false, theColumn.DisplayName, null, "", null, null, false, "", "", "");

                    newColumn.ColumnType = "text";

                    RecordManager.ets_Column_Insert(newColumn);
                    //we have created the column,no we can import data into the new table

                    //DataTable dtRecordsFrom = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM [Record] WHERE IsActive=1 AND TableID=" + _theTable.TableID.ToString());

                    DataTable dtRecordsFrom = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM [Record] WHERE (" + theColumn.SystemName + " IS NOT NULL OR " + theColumn.SystemName + "<>'') AND  IsActive=1 AND TableID=" + _theTable.TableID.ToString());


                    foreach (DataRow dr in dtRecordsFrom.Rows)
                    {
                        Record newRecord = new Record();
                        newRecord.TableID = iTableID;
                        newRecord.EnteredBy = _ObjUser.UserID;
                        RecordManager.MakeTheRecord(ref newRecord, strAutoSystemName, dr[0].ToString());
                        RecordManager.ets_Record_Insert(newRecord);
                    }

                    //now refresh the table dropdown
                    PopulateTable();

                    try
                    {
                        ddlDDTableC.SelectedValue = iTableID.ToString();
                    }
                    catch
                    {
                        //
                    }

                    //show user a message

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OkMessage", "alert('Table " + newTable.TableName + " has been created with records.');", true);
                }

                if (ddlDDType.SelectedValue == "values")
                {
                    //DataTable dtRecordsFrom = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM [Record] WHERE IsActive=1 AND TableID=" + _theTable.TableID.ToString());

                    DataTable dtRecordsFrom = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM [Record] WHERE (" + theColumn.SystemName + " IS NOT NULL OR " + theColumn.SystemName + "<>'') AND  IsActive=1 AND TableID=" + _theTable.TableID.ToString());


                    txtDropdownValues.Text = "";
                    foreach (DataRow dr in dtRecordsFrom.Rows)
                    {

                        txtDropdownValues.Text = txtDropdownValues.Text + dr[0].ToString() + Environment.NewLine;
                    }

                    if (txtDropdownValues.Text.IndexOf("\r\n") > -1)
                    {
                        txtDropdownValues.Text = txtDropdownValues.Text.Substring(0, txtDropdownValues.Text.LastIndexOf("\r\n"));
                    }

                }

                if (ddlDDType.SelectedValue == "value_text")
                {
                    //DataTable dtRecordsFrom = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM [Record] WHERE IsActive=1 AND TableID=" + _theTable.TableID.ToString());

                    DataTable dtRecordsFrom = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM [Record] WHERE (" + theColumn.SystemName + " IS NOT NULL OR " + theColumn.SystemName + "<>'') AND  IsActive=1 AND TableID=" + _theTable.TableID.ToString());

                    txtDropdownValues.Text = "";
                    int i = 1;
                    foreach (DataRow dr in dtRecordsFrom.Rows)
                    {

                        txtDropdownValues.Text = txtDropdownValues.Text + i.ToString() + "," + dr[0].ToString() + Environment.NewLine;
                        i = i + 1;
                    }

                    if (txtDropdownValues.Text.IndexOf("\r\n") > -1)
                    {
                        txtDropdownValues.Text = txtDropdownValues.Text.Substring(0, txtDropdownValues.Text.LastIndexOf("\r\n"));
                    }

                }

                lnkCreateTable.Visible = false;
            }

        }
        catch (Exception ex)
        {
            //ex.ShowOpen
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Sorry! We could not create the table, please try again.');", true);
        }

    }

    protected string GetOldFormula(Column theColumn, string strType)
    {
        string strFormula = "";
        string strConF = UploadWorld.Condition_GetFormula_Full((int)theColumn.ColumnID, strType);
        if (strConF != "")
        {
            return strConF;
        }
        else
        {
            if (strType == "V")
            {
                return theColumn.ValidationOnEntry;
            }
            else if (strType == "E")
            {
                return theColumn.ValidationOnExceedance;
            }
            else if (strType == "W")
            {
                return theColumn.ValidationOnWarning;
            }
        }
        return strFormula;
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        string strErrorCal = "";
        string strNotifications = "";
        try
        {
            lblMsg.Text = "";

            //Check input

            if (!IsUserInputOK())
            { return; }

            if (ddlType.SelectedValue == "staticcontent")
            {
                txtDisplayTextDetail.Text = txtColumnName.Text;
            }



            switch (_strActionMode.ToLower())
            {
                case "add":

                    string strAutoSystemName = "";

                    strAutoSystemName = RecordManager.ets_Column_NextSystemName(int.Parse(hfTableID.Value));
                    if (strAutoSystemName == "NO")
                    {
                        //Response.Redirect(Request.RawUrl + "&error=no", false);
                        lblMsg.Text = "You can add 500 columns, there is no field available!";
                        return;
                    }



                    int? iDisplayOrder = RecordManager.ets_Table_MaxOrder(int.Parse(hfTableID.Value));

                    if (iDisplayOrder == null)
                        iDisplayOrder = -1;
                    //int.Parse(Request.QueryString["TableID"])
                    //here lets get the Auto Select SystemName

                    if (chkDetailPage.Checked && txtDisplayTextDetail.Text == "")
                        txtDisplayTextDetail.Text = " ";
                    if (ddlType.SelectedValue == "staticcontent" || ddlType.SelectedValue == "button")
                    {
                        txtDisplayTextSummary.Text = "";
                    }

                    Column newColumn = new Column(null, int.Parse(hfTableID.Value),
                   strAutoSystemName, iDisplayOrder + 1,
                    txtDisplayTextSummary.Text.Trim(), txtDisplayTextDetail.Text,
                    "", txtNameOnExport.Text.Trim(),
                    null, GetWarningValidation(),
                    GetValidValidation(), null, null, "", "", false, txtColumnName.Text.Trim(), null, txtNotes.Text, chkRound.Checked,
                    txtRoundNumber.Text == "" ? null : (int?)int.Parse(txtRoundNumber.Text), chkCheckUnlikelyValue.Checked, txtGraph.Text.Trim(),
                    "", ddlImportance.SelectedValue
                    );
                    //chkDropdownValues.Checked==true?txtDropdownValues.Text:""
                    newColumn.ValidationOnExceedance = GetExceedanceValidation();
                    newColumn.ViewName = "";
                    newColumn.SummarySearch = chkSummarySearch.Checked;

                    newColumn.QuickAddLink = chkQuickAddLink.Checked;

                    if (chkCompareOperator.Checked)
                    {
                        if (ddlCompareColumnID.SelectedValue != "" && ddlCompareOperator.SelectedValue != "")
                        {
                            newColumn.CompareColumnID = int.Parse(ddlCompareColumnID.SelectedValue);
                            newColumn.CompareOperator = ddlCompareOperator.SelectedValue;
                        }
                    }


                    if (Session["Mappopup"] != null)
                    {
                        newColumn.MapPopup = Session["Mappopup"].ToString();
                    }

                    if (ddlTableTab.SelectedValue != "")
                    {
                        newColumn.TableTabID = int.Parse(ddlTableTab.SelectedValue);
                    }

                    //if (_iTableTabID != null && _iTableTabID!=-1)
                    //    newColumn.TableTabID = _iTableTabID;


                    newColumn.OnlyForAdmin = int.Parse(ddlOnlyForAdmin.SelectedValue);
                    if (chkExceedence.Checked)
                    {
                        if (txtExceedence.Text != "")
                        {
                            newColumn.ShowGraphExceedance = double.Parse(txtExceedence.Text);

                        }
                    }




                    if (chkWarning.Checked)
                    {
                        if (txtWarning.Text != "")
                        {
                            newColumn.ShowGraphWarning = double.Parse(txtWarning.Text);
                        }
                    }

                    if (chkMaximumValueat.Checked)
                    {
                        if (txtMaximumValueat.Text != "")
                        {
                            newColumn.MaxValueAt = double.Parse(txtMaximumValueat.Text);
                        }
                    }

                    if (ddlDefaultGraphDefinition.SelectedValue != "-1")
                    {
                        newColumn.DefaultGraphDefinitionID = int.Parse(ddlDefaultGraphDefinition.SelectedValue);
                    }


                    if (chkFlatLine.Checked && txtFlatLine.Text.Trim() != "")
                    {
                        try
                        {
                            newColumn.FlatLineNumber = int.Parse(txtFlatLine.Text.Trim());
                        }
                        catch
                        {
                            return;
                        }
                    }


                    //newColumn.Alignment = ddlAlignment.Text == "-1" ? null : ddlAlignment.Text;


                    newColumn.DefaultType = ddlDefaultValue.SelectedValue;
                    if (ddlDefaultValue.SelectedValue == "value" && txtDefaultValue.Text != "")
                    {

                        newColumn.DefaultValue = txtDefaultValue.Text;


                    }


                    if (ddlDefaultValue.SelectedValue == "value")
                    {

                        if (ddlType.SelectedValue == "date_time")
                        {
                            newColumn.DefaultValue = "yes";
                        }

                    }
                    if (ddlDefaultValue.SelectedValue == "parent")
                    {
                        if (ddlDefaultParentColumn.SelectedValue != "" && ddlDefaultParentTable.SelectedValue != "")
                        {
                            newColumn.DefaultColumnID = int.Parse(ddlDefaultParentColumn.SelectedValue);
                            newColumn.DefaultRelatedTableID = int.Parse(ddlDefaultParentTable.SelectedValue);
                            newColumn.DefaultUpdateValues = chkDefaultSyncData.Checked;
                        }
                    }
                    if (ddlDefaultValue.SelectedValue == "login")
                    {
                        //do nothing
                    }

                    newColumn.MobileName = txtMobile.Text.Trim();

                    if (chkImport.Checked)
                    {
                        txtConstant.Text = "";
                    }
                    newColumn.Constant = txtConstant.Text;
                    if (txtConstant.Text != "")
                    {
                        double dTest;
                        if (double.TryParse(txtConstant.Text, out dTest))
                        {
                        }
                        else
                        {
                            lblMsg.Text = "Constant should be a numeric value!";
                            txtConstant.Focus();
                            return;
                        }

                    }

                    newColumn.Calculation = Common.GetSystemFromDisplay(txtCalculation.Text, (int)newColumn.TableID);



                    //if (ddlSumXCellBackColour.Value != "")
                    //    newColumn.SummaryCellBackColor = ddlSumXCellBackColour.Value;

                    //if (ddlType.Text == "data_retriever")
                    //{
                    //    newColumn.ColumnType = "data_retriever";
                    //    if (ddlDataRetriver.SelectedValue == "")
                    //    {
                    //        lblMsg.Text = "Please select a data retriever!";
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        newColumn.DataRetrieverID = int.Parse(ddlDataRetriver.SelectedValue);
                    //    }
                    //}


                    if (ddlType.Text == "text")
                    {
                        newColumn.ColumnType = "text";
                        newColumn.TextType = ddlTextType.Text;
                        if (txtTextHeight.Text != "")
                            newColumn.TextHeight = int.Parse(txtTextHeight.Text);
                        if (txtTextWidth.Text != "")
                            newColumn.TextWidth = int.Parse(txtTextWidth.Text);


                    }
                    else if (ddlType.Text == "number")
                    {
                        newColumn.NumberType = int.Parse(ddlNumberType.Text);

                        newColumn.ColumnType = "number";

                        if (txtTextWidth.Text != "")
                            newColumn.TextWidth = int.Parse(txtTextWidth.Text);

                        //if (chkShowTotal.Checked)
                        //{
                        //    newColumn.ShowTotal = true;
                        //}
                        if (chkIgnoreSymbols.Checked)
                        {
                            newColumn.IgnoreSymbols = true;
                        }
                        if (chkCheckUnlikelyValue.Checked)
                        {
                            newColumn.CheckUnlikelyValue = true;
                        }

                        if (chkRound.Checked && txtRoundNumber.Text != "")
                        {
                            int iTest;
                            if (int.TryParse(txtRoundNumber.Text, out iTest))
                            {
                            }
                            else
                            {
                                lblMsg.Text = "Round number should be a numeric value!";
                                txtRoundNumber.Focus();
                                return;
                            }

                        }


                        if (ddlNumberType.Text == "6")
                        {
                            newColumn.TextType = txtSymbol.Text;
                        }

                        if (ddlNumberType.Text == "7")
                        {
                            //newColumn.TextType = txtSymbol.Text;
                            SliderField newSliderField = new SliderField();
                            if (txtSliderMin.Text.Trim() == "")
                            {
                                newSliderField.Min = 0;
                            }
                            else
                            {
                                newSliderField.Min = int.Parse(txtSliderMin.Text.Trim());
                            }
                            if (txtSliderMax.Text.Trim() == "")
                            {
                                newSliderField.Max = 100;
                            }
                            else
                            {
                                newSliderField.Max = int.Parse(txtSliderMax.Text.Trim());
                            }
                            newColumn.DropdownValues = newSliderField.GetJSONString();
                        }

                        if (ddlNumberType.Text == "4")
                        {
                            //avg

                            int iTest;
                            if (int.TryParse(txtAvgNumValues.Text, out iTest))
                            {
                            }
                            else
                            {
                                lblMsg.Text = "Avg Num Value should be a numeric value!";
                                txtAvgNumValues.Focus();
                                return;
                            }

                            newColumn.AvgNumberOfRecords = int.Parse(txtAvgNumValues.Text);

                            newColumn.AvgColumnID = int.Parse(ddlAvgColumn.SelectedValue);
                        }
                        else
                        {
                            newColumn.AvgNumberOfRecords = null;

                        }

                        if (ddlNumberType.Text == "5")
                        {
                            //record count
                            if (ddlRecordCountTable.Items.Count == 0)
                            {
                                lblMsg.Text = "No child record for this table!";
                                ddlRecordCountTable.Focus();
                                return;
                            }

                            newColumn.TableTableID = int.Parse(ddlRecordCountTable.SelectedValue);
                            newColumn.DropdownValues = ddlRecordCountClick.SelectedValue;

                        }



                    }
                    else if (ddlType.Text == "radiobutton")
                    {
                        newColumn.ColumnType = ddlType.Text;
                        newColumn.DropDownType = ddlOptionType.SelectedValue;

                        if (newColumn.DropDownType == "value_image")
                        {
                            newColumn.ImageOnSummary = chkImageOnSummary.Checked;
                            if (chkImageOnSummary.Checked && txtImageOnSummaryMaxHeight.Text != "")
                            {
                                newColumn.TextHeight = int.Parse(txtImageOnSummaryMaxHeight.Text);
                            }
                            else
                            {
                                newColumn.TextHeight = null;
                            }

                            if (Session["OptionImage"] != null)
                            {
                                DataTable dtOptionImage = (DataTable)Session["OptionImage"];
                                List<OptionImage> lstOptionImage = new List<OptionImage>();
                                foreach (DataRow dr in dtOptionImage.Rows)
                                {
                                    OptionImage aOptionImage = new OptionImage();
                                    aOptionImage.OptionImageID = dr["OptionImageID"].ToString();
                                    aOptionImage.Value = dr["Value"].ToString();
                                    aOptionImage.FileName = dr["FileName"].ToString();
                                    aOptionImage.UniqueFileName = dr["UniqueFileName"].ToString();
                                    lstOptionImage.Add(aOptionImage);

                                }

                                OptionImageList theOptionImageList = new OptionImageList();
                                theOptionImageList.ImageList = lstOptionImage;

                                newColumn.DropdownValues = theOptionImageList.GetJSONString();
                            }
                        }
                        else
                        {
                            newColumn.DropdownValues = txtDropdownValues.Text;
                        }


                        newColumn.VerticalList = chkDisplayVertical.Checked;
                    }
                    else if (ddlType.Text == "listbox")
                    {
                        newColumn.ColumnType = ddlType.Text;
                        newColumn.DropDownType = ddlListBoxType.SelectedValue;
                        newColumn.DropdownValues = txtDropdownValues.Text;

                        if (chkListCheckBox.Checked)
                        {
                            newColumn.DateCalculationType = "checkbox";
                        }


                        if (txtTextWidth.Text != "")
                        {
                            newColumn.TextWidth = int.Parse(txtTextWidth.Text);
                        }

                        if (txtTextHeight.Text != "")
                        {
                            newColumn.TextHeight = int.Parse(txtTextHeight.Text);

                            if (newColumn.TextHeight < 7)
                            {
                                newColumn.TextHeight = 7;
                            }
                        }

                        if (ddlListBoxType.SelectedValue == "table")
                        {
                            newColumn.DropdownValues = "";
                            if (hfDisplayColumnsFormula.Value == "")
                            {
                                bool bShowMessage = false;
                                if (ddDDDisplayColumn.SelectedValue == "")
                                {
                                    bShowMessage = true;
                                }
                                else
                                {
                                    hfDisplayColumnsFormula.Value = "[" + ddDDDisplayColumn.SelectedItem.Text + "]";
                                }
                                if (bShowMessage)
                                {
                                    lblMsg.Text = "Please select a display field.";
                                    return;
                                }
                            }
                            string strLinkedParentColumn = ddDDLinkedParentColumn.SelectedValue;
                            if (ddDDLinkedParentColumn.SelectedValue == "")
                            {

                                strLinkedParentColumn = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE SystemName='recordid' AND TableID=" + ddlDDTable.SelectedValue);
                                //lblMsg.Text = "Please select a linked field.";
                                //return;
                            }

                            newColumn.TableTableID = int.Parse(ddlDDTable.SelectedValue);

                            if (strLinkedParentColumn != "")
                                newColumn.LinkedParentColumnID = int.Parse(strLinkedParentColumn);

                            newColumn.DisplayColumn = hfDisplayColumnsFormula.Value;


                        }

                    }
                    else if (ddlType.Text == "checkbox")
                    {
                        newColumn.ColumnType = ddlType.Text;
                        newColumn.DropdownValues = txtTickedValue.Text + Environment.NewLine + txtUntickedValue.Text
                            + Environment.NewLine + (chkTickedByDefault.Checked == true ? "yes" : "no");

                    }

                    else if (ddlType.Text == "location")
                    {
                        newColumn.ColumnType = ddlType.Text;
                        newColumn.ShowTotal = chkLocationAddress.Checked;
                        newColumn.IsRound = chkLatLong.Checked;

                        if (chkShowMap.Checked)
                        {
                            if (txtMapHeight.Text == "")
                                txtMapHeight.Text = "200";
                            newColumn.TextHeight = int.Parse(txtMapHeight.Text);

                            if (txtMapWidth.Text == "")
                                txtMapWidth.Text = "400";
                            newColumn.TextWidth = int.Parse(txtMapWidth.Text);
                        }
                        else
                        {
                            newColumn.TextHeight = null;
                            newColumn.TextWidth = null;

                        }


                        if (ddlMapPinHoverColumnID.SelectedValue != "")
                            newColumn.MapPinHoverColumnID = int.Parse(ddlMapPinHoverColumnID.SelectedValue);



                    }
                    else if (ddlType.Text == "dropdown")
                    {
                        newColumn.ColumnType = ddlType.Text;


                        if (txtTextWidth.Text != "")
                            newColumn.TextWidth = int.Parse(txtTextWidth.Text);

                        if (ddlDDType.SelectedValue == "values"
                            || ddlDDType.SelectedValue == "value_text")
                        {
                            newColumn.DropDownType = ddlDDType.SelectedValue;
                            if (txtDropdownValues.Text == "")
                            {
                                lblMsg.Text = "Please enter Dropdown values!";
                                txtDropdownValues.Focus();
                                return;
                            }
                            else
                            {
                                newColumn.DropdownValues = txtDropdownValues.Text;
                            }
                        }
                        if (ddlDDType.SelectedValue == "ct" || ddlDDType.SelectedValue == "lt")
                        {
                            //so its table
                            newColumn.ShowViewLink = ddlShowViewLink.SelectedValue;

                            if (chkPredictive.Checked)
                            {
                                newColumn.DropDownType = "table";
                            }
                            else
                            {
                                newColumn.DropDownType = "tabledd";
                            }

                            if (hfDisplayColumnsFormula.Value == "")
                            {
                                bool bShowMessage = false;
                                if (ddDDDisplayColumn.SelectedValue == "")
                                {
                                    bShowMessage = true;
                                }
                                else
                                {
                                    hfDisplayColumnsFormula.Value = "[" + ddDDDisplayColumn.SelectedItem.Text + "]";
                                }
                                if (bShowMessage)
                                {
                                    lblMsg.Text = "Please select a display field.";
                                    return;
                                }
                            }

                            string strLinkedParentColumn = ddDDLinkedParentColumn.SelectedValue;
                            if (ddDDLinkedParentColumn.SelectedValue == "")
                            {

                                strLinkedParentColumn = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE SystemName='recordid' AND TableID=" + ddlDDTable.SelectedValue);
                                //lblMsg.Text = "Please select a linked field.";
                                //return;
                            }


                            newColumn.TableTableID = int.Parse(ddlDDTable.SelectedValue);

                            if (strLinkedParentColumn != "")
                                newColumn.LinkedParentColumnID = int.Parse(strLinkedParentColumn);

                            newColumn.DisplayColumn = hfDisplayColumnsFormula.Value;


                            if (ddlDDType.SelectedValue == "lt")//chkFilterValues.Checked
                            {
                                //so its filtered field

                                if (ddlFilter.Items.Count == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('There are no filter field.');", true);
                                    return;

                                }


                                newColumn.TableTableID = int.Parse(ddlDDTable.SelectedValue);
                                newColumn.DisplayColumn = hfDisplayColumnsFormula.Value;

                                newColumn.ParentColumnID = int.Parse(ddlFilter.SelectedValue);



                            }
                            else
                            {
                                if (hfFilterParentColumnID.Value != "" && chkFiltered.Checked)
                                {
                                    newColumn.FilterParentColumnID = int.Parse(hfFilterParentColumnID.Value);
                                    newColumn.FilterOperator = hfFilterOperator.Value;
                                    if (hfFilterOtherColumnID.Value != "")
                                    {
                                        newColumn.FilterOtherColumnID = int.Parse(hfFilterOtherColumnID.Value);
                                    }
                                    else
                                    {
                                        newColumn.FilterValue = hfFilterValue.Value;
                                    }
                                }

                            }
                        }

                    }
                    else if (ddlType.SelectedValue == "date_time")
                    {
                        newColumn.ColumnType = ddlDateTimeType.SelectedValue;

                    }
                    else if (ddlType.SelectedValue == "staticcontent")
                    {
                        if (chkAllowContenEdit.Checked)
                        {
                            newColumn.ColumnType = "content";
                            newColumn.DropdownValues = edtContent.Text;
                        }
                        else
                        {
                            newColumn.ColumnType = ddlType.SelectedValue;
                            newColumn.DropdownValues = edtContent.Text;
                        }


                    }
                    else if (ddlType.SelectedValue == "trafficlight")
                    {
                        newColumn.ColumnType = ddlType.SelectedValue;
                        bool bFoundValue = false;

                        if (ddlTLControllingField.SelectedValue == "")
                        {
                            lblMsg.Text = "Please select a Controlling Field.";
                            return;
                        }

                        string strTrafficLightValue = "<?xml version='1.0' encoding='utf-8' ?><items>";

                        if (cbcValue1.ddlYAxisV != "" && cbcValue1.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue1.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage1.SelectedValue) + "</ImageFile></item>";
                        }

                        if (cbcValue2.ddlYAxisV != "" && cbcValue2.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue2.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage2.SelectedValue) + "</ImageFile></item>";
                        }

                        if (cbcValue3.ddlYAxisV != "" && cbcValue3.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue3.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage3.SelectedValue) + "</ImageFile></item>";
                        }

                        if (cbcValue4.ddlYAxisV != "" && cbcValue4.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue4.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage4.SelectedValue) + "</ImageFile></item>";
                        }

                        if (cbcValue5.ddlYAxisV != "" && cbcValue5.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue5.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage5.SelectedValue) + "</ImageFile></item>";
                        }

                        if (bFoundValue == false)
                        {
                            lblMsg.Text = "Please select some Value and Traffic image.";
                            return;
                        }
                        else
                        {
                            strTrafficLightValue = strTrafficLightValue + "</items>";
                            newColumn.TrafficLightColumnID = int.Parse(ddlTLControllingField.SelectedValue);
                            newColumn.TrafficLightValues = strTrafficLightValue;
                        }

                    }
                    else
                    {
                        newColumn.ColumnType = ddlType.SelectedValue;
                    }

                    if (ddlType.Text == "calculation")
                    {

                        //newColumn.ShowTotal = chkShowTotal.Checked;

                        newColumn.TextType = ddlCalculationType.Text;
                        newColumn.RegEx = txtCalFinancialSymbol.Text;

                        if (ddlCalculationType.SelectedValue == "d")
                        {
                            newColumn.DateCalculationType = ddlDateResultFormat.SelectedValue;
                            newColumn.RegEx = "";
                            newColumn.IsRound = null;
                            newColumn.CheckUnlikelyValue = null;
                        }
                        //newColumn.
                    }
                    else
                    {
                        newColumn.Calculation = "";
                    }

                    newColumn.DisplayRight = chkDisplayOnRight.Checked;

                    //if ( chkDetailPage.Checked && chkShowWhen.Checked)
                    //{
                    //    if (hfHideColumnID.Value != "" && hfHideColumnValue.Value != "")
                    //    {
                    //        newColumn.HideColumnID = int.Parse(hfHideColumnID.Value);
                    //        newColumn.HideColumnValue = hfHideColumnValue.Value;
                    //        newColumn.HideOperator = hfHideColumnOperator.Value;
                    //    }
                    //}






                    if (ddlTextType.Text == "own")
                    {
                        newColumn.RegEx = txtOwnRegEx.Text;
                    }
                    if (ddlTextType.Text == "mobile")
                    {
                        newColumn.RegEx = Common.AusMobileRegEx;
                    }


                    if (chkImport.Checked)
                    {
                        if (_theTable.IsImportPositional == true)
                        {
                            newColumn.PositionOnImport = int.Parse(txtNameOnImport.Text.Trim());
                        }
                        else
                        {

                            newColumn.NameOnImport = txtNameOnImport.Text.Trim();

                        }

                        if (txtCalculation.Text.Trim() != "")
                        {
                            newColumn.NameOnImport = "";
                            //lblMsg.Text = "This field has been configured on the import which would cause a conflict with this formula. Please un check import field name or remove calculation formula!";
                            //return;
                        }
                    }

                    if (newColumn.ColumnType == "image")
                    {
                        if (txtImageHeightSummary.Text != "")
                            newColumn.TextWidth = int.Parse(txtImageHeightSummary.Text);
                        if (txtImageHeightDetail.Text != "")
                            newColumn.TextHeight = int.Parse(txtImageHeightDetail.Text);
                    }

                    if (ddlType.SelectedValue == "button")
                    {
                        newColumn.ColumnType = ddlType.SelectedValue;
                        //if (txtSPToRun.Text.Trim() == "")
                        //{
                        //    lblMsg.Text = "Please enter a SP name.";
                        //    return;
                        //}
                        ColumnButtonInfo aButtonInfo = new ColumnButtonInfo();

                        if (chkSPToRun.Checked)
                        {
                            aButtonInfo.SPToRun = txtSPToRun.Text;
                        }
                        else
                        {
                            aButtonInfo.SPToRun = "";
                        }
                        if (chkButtonOpenLink.Checked)
                        {
                            aButtonInfo.OpenLink = txtButtonOpenLink.Text;
                        }
                        else
                        {
                            aButtonInfo.OpenLink = "";
                        }

                        aButtonInfo.ImageFullPath = hfButtonValue.Value;// txtButtonImage.Text;
                        if (chkButtonWarningMessage.Checked && txtButtonWarningMessage.Text != "")
                        {
                            aButtonInfo.WarningMessage = txtButtonWarningMessage.Text;
                        }

                        newColumn.ButtonInfo = aButtonInfo.GetJSONString();
                        if (txtTextHeight.Text != "")
                            newColumn.TextHeight = int.Parse(txtTextHeight.Text);
                        if (txtTextWidth.Text != "")
                            newColumn.TextWidth = int.Parse(txtTextWidth.Text);

                    }
                    else
                    {
                        newColumn.ButtonInfo = "";
                    }

                    if (chkColumnColour.Checked)
                        newColumn.ColourCells = true;

                    //int iColumnID = RecordManager.ets_Column_Insert(newColumn);

                    //SqlTransaction tn1;
                    //SqlConnection connection1 = new SqlConnection(DBGurus.strGlobalConnectionString);
                    //connection1.Open();
                    //tn1 = connection1.BeginTransaction();
                    try
                    {

                        //if (newColumn.ValidationOnEntry != "")
                        newColumn.ValidationCanIgnore = chkValidationCanIgnore.Checked;

                        int iColumnID = RecordManager.ets_Column_Insert(newColumn);

                        //Lets check the child tables

                        if (chkDetailPage.Checked && chkShowWhen.Checked
                            && Session["dtShowWhen"] != null)
                        {
                            //insert new show when
                            DataTable dtShowWhen = (DataTable)Session["dtShowWhen"];
                            int iDO = 1;
                            foreach (DataRow drSW in dtShowWhen.Rows)
                            {
                                if (iDO == 1)
                                {
                                    if (drSW["HideColumnID"].ToString() == "" || drSW["HideColumnValue"].ToString() == "")
                                    {
                                        continue;
                                    }
                                    ShowWhen theShowWhen1 = new ShowWhen();
                                    theShowWhen1.ColumnID = iColumnID;
                                    theShowWhen1.Context = "field";
                                    theShowWhen1.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                                    theShowWhen1.HideColumnValue = drSW["HideColumnValue"].ToString();
                                    theShowWhen1.HideOperator = drSW["HideOperator"].ToString();
                                    theShowWhen1.DisplayOrder = 1;
                                    theShowWhen1.JoinOperator = "";
                                    theShowWhen1.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen1);

                                    iDO = iDO + 1;
                                    continue;
                                }
                                else
                                {
                                    if (drSW["HideColumnID"].ToString() == "" || drSW["HideColumnValue"].ToString() == "" || drSW["JoinOperator"].ToString() == "")
                                    {
                                        continue;
                                    }


                                    ShowWhen theShowWhenJoin = new ShowWhen();
                                    theShowWhenJoin.ColumnID = iColumnID;
                                    theShowWhenJoin.Context = "field";
                                    theShowWhenJoin.HideColumnID = null;
                                    theShowWhenJoin.HideColumnValue = "";
                                    theShowWhenJoin.HideOperator = "";
                                    theShowWhenJoin.DisplayOrder = iDO;
                                    theShowWhenJoin.JoinOperator = drSW["JoinOperator"].ToString();

                                    theShowWhenJoin.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhenJoin);
                                    iDO = iDO + 1;

                                    ShowWhen theShowWhen = new ShowWhen();
                                    theShowWhen.ColumnID = iColumnID;
                                    theShowWhen.Context = "field";
                                    theShowWhen.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                                    theShowWhen.HideColumnValue = drSW["HideColumnValue"].ToString();
                                    theShowWhen.HideOperator = drSW["HideOperator"].ToString();
                                    theShowWhen.DisplayOrder = iDO;
                                    theShowWhen.JoinOperator = "";

                                    theShowWhen.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen);
                                    iDO = iDO + 1;

                                }
                            }

                        }

                        if (chkColumnColour.Checked && Session["dtColumnColour"] != null)
                        {

                            DataTable dtColumnColour = (DataTable)Session["dtColumnColour"];
                            foreach (DataRow drCC in dtColumnColour.Rows)
                            {

                                if (drCC["ControllingColumnID"].ToString() == "" || drCC["Operator"].ToString() == "" || drCC["Value"].ToString() == "" || drCC["Colour"].ToString() == "")
                                {
                                    continue;
                                }

                                ColumnColour theColumnColour = new ColumnColour();

                                theColumnColour.ColumnID = iColumnID;
                                theColumnColour.ControllingColumnID = int.Parse(drCC["ControllingColumnID"].ToString());
                                theColumnColour.Value = drCC["Value"].ToString();
                                theColumnColour.Operator = drCC["Operator"].ToString();
                                theColumnColour.Colour = drCC["Colour"].ToString();

                                Cosmetic.dbg_ColumnColour_Insert(theColumnColour);


                            }

                        }

                        if (newColumn.TableTableID != null && newColumn.LinkedParentColumnID != null)
                        {
                            DataTable dtTemp = Common.DataTableFromText("SELECT * FROM TableChild WHERE ParentTableID=" + newColumn.TableTableID.ToString() + " AND ChildTableID=" + newColumn.TableID.ToString() + " AND DetailPageType='list'");

                            if (dtTemp.Rows.Count > 0)
                            {
                                //do nothing as we have this relationgship
                            }
                            else
                            {

                                TableChild theTableChild = new TableChild(null, newColumn.TableTableID,
                                    newColumn.TableID, "", "list");
                                theTableChild.ShowAddButton = true;
                                theTableChild.ShowEditButton = true;
                                RecordManager.ets_TableChild_Insert(theTableChild);
                            }
                        }


                        //newColumn.ColumnID = iColumnID;


                        newColumn = RecordManager.ets_Column_Details(iColumnID);

                        if (newColumn.Calculation != "" && newColumn.Calculation.Length > 0)
                        {
                            string strCalOutput = RecordManager.ets_AdjustCalculationFormulaChanges(newColumn, ref strErrorCal, false);
                            strNotifications = strNotifications + "Calculation Values Adjusted:" + newColumn.DisplayColumn + " " + strCalOutput;
                        }




                        if (Session["Condition"] != null)
                        {

                            DataTable dtCondition = (DataTable)Session["Condition"];
                            foreach (DataRow drCondition in dtCondition.Rows)
                            {
                                Condition newCondition = new Condition(null, iColumnID, drCondition["ConditionType"].ToString(),
                                    int.Parse(drCondition["CheckColumnID"].ToString()),
                                    drCondition["CheckFormula"].ToString(), drCondition["CheckValue"].ToString());

                                int iConditionID = UploadWorld.dbg_Condition_Insert(newCondition);
                            }
                        }

                        //if (newColumn.Calculation!="")
                        //{
                        //    string strTemp = "";
                        //    string strError1 = "";
                        //    RecordManager.ets_AdjustCalculationFormulaChanges(newColumn,  ref strTemp);


                        //    //check the warning now for new data
                        //    if (newColumn.ValidationOnWarning != "")
                        //    {

                        //        RecordManager.ets_AdjustWarningFormulaChanges(newColumn,ref strTemp, true);
                        //        strError1 = strError1 + strTemp;
                        //    }

                        //    //check unlikely reading
                        //    if (newColumn.CheckUnlikelyValue != chkCheckUnlikelyValue.Checked || chkCheckUnlikelyValue.Checked == true)
                        //    {
                        //        strTemp = "";
                        //        RecordManager.ets_AdjustUnlikelyReadings(newColumn,  ref strTemp);
                        //        strError1 = strError1 + strTemp;
                        //    }
                        //    //check the validation


                        //    if (newColumn.ValidationOnEntry != "")
                        //    {
                        //        DataTable dtRecords = Common.DataTableFromText("Select RecordID," + newColumn.SystemName + " FROM Record where IsActive=1 AND " + newColumn.SystemName + " IS NOT NULL and TableID=" + newColumn.TableID.ToString());

                        //        if (dtRecords.Rows.Count > 0)
                        //        {
                        //            string strRecordIDs = "";
                        //            string strValidationError = "";
                        //            double dCounter = 0;
                        //            foreach (DataRow dr in dtRecords.Rows)
                        //            {
                        //                if (!UploadManager.IsDataValid(dr[1].ToString(), GetValidValidation(), ref strValidationError, chkIgnoreSymbols.Checked))
                        //                {
                        //                    strRecordIDs = strRecordIDs + dr[0].ToString() + ",";
                        //                    dCounter = dCounter + 1;
                        //                }
                        //            }

                        //            if (dCounter > 0)
                        //            {
                        //                strRecordIDs = strRecordIDs.Substring(0, strRecordIDs.Length - 1);
                        //                string strValidResults = " INVALID: " + newColumn.DisplayName;
                        //                Common.ExecuteText("Update Record SET IsActive=0, validationresults=( Case WHEN (ValidationResults IS null) then '" + strValidResults.Replace("'", "''") + "' else (ValidationResults+ '" + strValidResults.Replace("'", "''") + "') END) WHERE RecordID in (" + strRecordIDs + ")");
                        //                //RecordManager.SendAdjustValidationSMSandEmail(newColumn, dCounter, ref strTemp, true);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            newColumn.ValidationOnEntry = GetValidValidation();
                        //        }

                        //    }

                        //}


                        if (newColumn.AvgColumnID != null)
                        {
                            string strTemp = "";
                            DataTable dtTemp = Common.DataTableFromText(@"SELECT RecordID FROM Record WHERE IsActive=1 AND TableID=" + newColumn.TableID.ToString());

                            foreach (DataRow dr in dtTemp.Rows)
                            {
                                RecordManager.ets_Record_Avg_ForARecordID(int.Parse(dr["RecordID"].ToString()));
                            }
                        }




                        //Now lets check if there is any reminders


                        if (newColumn.ColumnType == "datetime" || newColumn.ColumnType == "date")
                        {

                            if (Session["DataReminder"] != null && Session["DataReminderUser"] != null)
                            {

                                DataTable dtDataReminder = (DataTable)Session["DataReminder"];
                                DataTable dtDataReminderUser = (DataTable)Session["DataReminderUser"];

                                foreach (DataRow drReminder in dtDataReminder.Rows)
                                {
                                    DataRow[] drUsers = dtDataReminderUser.Select("DataReminderID='" + drReminder["DataReminderID"].ToString() + "'");

                                    DataReminder newDataReminder = new DataReminder(null, iColumnID, int.Parse(drReminder["NumberOfDays"].ToString()),
                                        drReminder["ReminderHeader"].ToString(), drReminder["ReminderContent"].ToString());


                                    int iDataReminderID = ReminderManager.ets_DataReminder_Insert(newDataReminder);

                                    foreach (DataRow drUser in drUsers)
                                    {
                                        DataReminderUser newDataReminderUser = new DataReminderUser(null, iDataReminderID,
                                           drUser["UserID"] == DBNull.Value ? null : (int?)int.Parse(drUser["UserID"].ToString()));

                                        newDataReminderUser.ReminderColumnID = drUser["ReminderColumnID"] == DBNull.Value ? null : (int?)int.Parse(drUser["ReminderColumnID"].ToString());
                                        ReminderManager.ets_DataReminderUser_Insert(newDataReminderUser);

                                    }
                                }


                            }


                        }

                        //update view items

                        //if (newColumn.DisplayTextSummary != "")
                        //{
                        //    ViewManager.dbg_View_ColumnUpdate((int)newColumn.ColumnID);
                        //}



                        //tn1.Commit();
                        //connection1.Close();
                        //connection1.Dispose();
                    }
                    catch (Exception ex)
                    {
                        //tn1.Rollback();
                        //connection1.Close();
                        //connection1.Dispose();
                        lblMsg.Text = ex.StackTrace;
                        return;
                    }

                    break;

                case "view":


                    break;

                case "edit":
                    Column editColumn = RecordManager.ets_Column_Details(_iColumnID);    //(Column)ViewState["theColumn"];
                    string strError = "";
                    //string strOldValidationOnWarning = editColumn.ValidationOnWarning;
                    //string strOldValidationOnExceedance = editColumn.ValidationOnExceedance;

                    editColumn.ValidationOnWarning = "";
                    editColumn.ValidationOnExceedance = "";
                    //editColumn.ValidationOnEntry = "";
                    editColumn.TrafficLightColumnID = null;
                    editColumn.TrafficLightValues = "";
                    string strOldDisplayTextSummary = editColumn.DisplayTextSummary;

                    editColumn.FilterValue = "";
                    editColumn.FilterParentColumnID = null;
                    editColumn.FilterOtherColumnID = null;

                    editColumn.ParentColumnID = null;
                    editColumn.DisplayRight = chkDisplayOnRight.Checked;

                    editColumn.ShowViewLink = "";

                    if (Session["Mappopup"] != null)
                    {
                        editColumn.MapPopup = Session["Mappopup"].ToString();
                    }

                    if (chkDetailPage.Checked)
                    {
                        if (txtDisplayTextDetail.Text == "")
                            txtDisplayTextDetail.Text = " ";
                        editColumn.DisplayTextDetail = txtDisplayTextDetail.Text;
                    }
                    else
                    {
                        editColumn.DisplayTextDetail = "";
                    }
                    if (chkGraph.Checked)
                    {
                        editColumn.GraphLabel = txtGraph.Text.Trim();
                    }
                    else
                    {
                        editColumn.GraphLabel = "";
                    }

                    editColumn.SummarySearch = chkSummarySearch.Checked;

                    if (chkSummaryPage.Checked)
                    {
                        editColumn.DisplayTextSummary = txtDisplayTextSummary.Text.Trim();
                        editColumn.ViewName = "";

                    }
                    else
                    {
                        //editColumn.SummarySearch = null;
                        editColumn.DisplayTextSummary = "";
                        editColumn.ViewName = "";
                    }


                    if (chkExport.Checked)
                    {
                        editColumn.NameOnExport = txtNameOnExport.Text.Trim();
                    }
                    else
                    {
                        editColumn.NameOnExport = "";
                    }
                    //editColumn.NameOnImport = txtNameOnImport.Text.Trim();
                    //editColumn.DropdownValues = chkDropdownValues.Checked == true ? txtDropdownValues.Text : "";
                    //editColumn.IsMandatory = chkMandatory.Checked;

                    editColumn.Importance = ddlImportance.SelectedValue;
                    editColumn.OnlyForAdmin = int.Parse(ddlOnlyForAdmin.SelectedValue);
                    editColumn.QuickAddLink = chkQuickAddLink.Checked;

                    if (chkCompareOperator.Checked && ddlCompareColumnID.SelectedValue != "" && ddlCompareOperator.SelectedValue != "")
                    {
                        editColumn.CompareColumnID = int.Parse(ddlCompareColumnID.SelectedValue);
                        editColumn.CompareOperator = ddlCompareOperator.SelectedValue;
                    }
                    else
                    {
                        editColumn.CompareColumnID = null;
                        editColumn.CompareOperator = "";
                    }


                    if (chkMobile.Checked)
                    {
                        editColumn.MobileName = txtMobile.Text.Trim();
                    }
                    else
                    {
                        editColumn.MobileName = "";
                    }

                    if (ddlTableTab.SelectedValue != "")
                    {
                        editColumn.TableTabID = int.Parse(ddlTableTab.SelectedValue);
                    }
                    else
                    {
                        editColumn.TableTabID = null;
                    }

                    bool bWarningChanged = false;
                    bool bExceedanceChanged = false;

                    bool bUnlikelyReadingChnaged = false;
                    bool bCalculationChanged = false;
                    bool bAvgDefinitionChanged = false;

                    int? iTableTableID_old = editColumn.TableTableID;

                    editColumn.TableTableID = null;
                    editColumn.LinkedParentColumnID = null;

                    //string strValidationOnWarning = "";
                    //string strValidationOnExceedance = "";

                    //string strValidationOnEntry = "";

                    editColumn.ValidationOnWarning = GetWarningValidation();
                    editColumn.ValidationOnExceedance = GetExceedanceValidation();
                    editColumn.ValidationOnEntry = GetValidValidation();
                    string strNewFormulaW = GetOldFormula(editColumn, "W");
                    string strNewFormulaE = GetOldFormula(editColumn, "E");
                    if (ViewState["OldFormulaW"].ToString().ToUpper() != strNewFormulaW.ToUpper())
                    {
                        //editColumn.ValidationOnWarning = strValidationOnWarning;
                        bWarningChanged = true;
                        //lets update
                    }



                    if (ViewState["OldFormulaE"].ToString().ToUpper() != strNewFormulaE.ToUpper())
                    {
                        bExceedanceChanged = true;
                        //lets update
                    }


                    if (chkFlatLine.Checked && txtFlatLine.Text.Trim() != "")
                    {
                        try
                        {

                            editColumn.FlatLineNumber = int.Parse(txtFlatLine.Text.Trim());
                        }
                        catch
                        {
                            return;
                            //
                        }
                    }
                    else
                    {
                        editColumn.FlatLineNumber = null;
                    }



                    if (editColumn.CheckUnlikelyValue != chkCheckUnlikelyValue.Checked)
                    {

                        bUnlikelyReadingChnaged = true;


                    }

                    //if (editColumn.Calculation != txtCalculation.Text.Trim() )
                    //{
                    //    editColumn.Calculation = txtCalculation.Text.Trim();

                    //}

                    //no need to chnage the system name
                    bool bDisplayNameChanged = false;
                    string strOldDislayName = editColumn.DisplayName;
                    if (editColumn.DisplayName.ToLower() != txtColumnName.Text.Trim().ToLower())
                    {
                        bDisplayNameChanged = true;
                    }

                    editColumn.DisplayName = txtColumnName.Text.Trim();
                    editColumn.ValidationCanIgnore = chkValidationCanIgnore.Checked;


                    if (ddlTextType.Text == "own")
                    {
                        editColumn.RegEx = txtOwnRegEx.Text;
                    }
                    else if (ddlTextType.Text == "mobile")
                    {
                        editColumn.RegEx = Common.AusMobileRegEx;
                    }
                    else
                    {
                        editColumn.RegEx = "";
                    }


                    //editColumn.DisplayOrder - dont change it.
                    int iPosition = 1;
                    if (_theTable.IsImportPositional == true)
                    {
                        try
                        {
                            if (chkImport.Checked)
                            {
                                if (editColumn.SystemName.ToLower() == "datetimerecorded")
                                {
                                    if (optSingle.Checked == true)
                                    {
                                        editColumn.IsDateSingleColumn = true;
                                        iPosition = int.Parse(txtNameOnImport.Text.Trim());
                                    }
                                    else
                                    {
                                        editColumn.IsDateSingleColumn = false;
                                        iPosition = int.Parse(txtNameOnImport.Text.Trim());
                                    }
                                    //if(txtNameOnImport.Text.Trim().IndexOf(",")>-1)
                                    //{
                                    //    iPosition = int.Parse(txtNameOnImport.Text.Trim().Substring(0,txtNameOnImport.Text.Trim().IndexOf(",")));
                                    //    editColumn.IsDateSingleColumn = false;
                                    //}
                                    //else
                                    //{
                                    //    iPosition = int.Parse(txtNameOnImport.Text.Trim());
                                    //    editColumn.IsDateSingleColumn = true;
                                    //}
                                }
                                else
                                {
                                    iPosition = int.Parse(txtNameOnImport.Text.Trim());
                                }
                                //check if position is duplicate and suggest the max

                                if (editColumn.PositionOnImport != null)
                                {
                                    if (editColumn.PositionOnImport != iPosition)
                                    {
                                        Column dupPosColumn = RecordManager.ets_Column_Details_Position((int)_theTable.TableID, iPosition);

                                        if (dupPosColumn != null)
                                        {
                                            lblMsg.Text = dupPosColumn.DisplayName + " " + "has import field position at " + iPosition.ToString().Trim() + ", you can try " + hfMaxPosition.Value + "!";
                                            return;
                                        }

                                    }
                                }

                            }
                        }
                        catch
                        {
                            lblMsg.Text = "Please enter an integer number for Import Position!";
                            return;

                        }
                    }
                    else
                    {
                        //do nothing
                    }




                    if (chkImport.Checked)
                    {
                        if (_theTable.IsImportPositional == true)
                        {
                            editColumn.PositionOnImport = iPosition;// int.Parse(txtNameOnImport.Text.Trim());
                        }
                        else
                        {
                            if (editColumn.SystemName.ToLower() == "datetimerecorded")
                            {
                                if (txtNameOnImport.Text == "")
                                {
                                    lblMsg.Text = "Please enter Date Label.";
                                    return;
                                }

                                if (optSingle.Checked == true)
                                {
                                    editColumn.NameOnImport = txtNameOnImport.Text.Trim();
                                }
                                else
                                {
                                    if (txtNameOnImportTime.Text == "")
                                    {
                                        lblMsg.Text = "Please enter Time Label.";
                                        return;
                                    }
                                    editColumn.NameOnImport = txtNameOnImport.Text.Trim() + "," + txtNameOnImportTime.Text.Trim();
                                }
                            }
                            else
                            {
                                editColumn.NameOnImport = txtNameOnImport.Text.Trim();
                            }
                        }

                        if (txtCalculation.Text.Trim() != "")
                        {
                            lblMsg.Text = "This field has been configured on the import which would cause a conflict with this formula. Please uncheck import field name or remove calculation formula!";
                            return;
                        }
                    }
                    else
                    {
                        editColumn.NameOnImport = "";
                        editColumn.PositionOnImport = null;

                    }
                    editColumn.ColumnType = ddlType.Text;
                    string strOldCalculation = editColumn.Calculation;
                    if (editColumn.ColumnType == "calculation")
                    {
                        editColumn.Calculation = Common.GetSystemFromDisplay(txtCalculation.Text, (int)editColumn.TableID);
                    }
                    else
                    {
                        editColumn.Calculation = "";
                    }

                    if (strOldCalculation.ToLower() != editColumn.Calculation.ToLower())
                    {
                        bCalculationChanged = true;
                    }




                    string strNewFormulaV = GetOldFormula(editColumn, "V");
                    bool bAllowReValidation = false;
                    bool bAdjustValidPreformed = false;
                    //bool bPerformValiationInCalculation = false;
                    if (ViewState["perform_validation"] == null && 
                        (ViewState["OldFormulaV"].ToString().ToUpper() != strNewFormulaV.ToUpper() || bExceedanceChanged || bWarningChanged))
                    {
                        hlResetValidation.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                            Cryptography.Encrypt("Would you like to revalidate all records?")
                            + "&okbutton=" + Cryptography.Encrypt(btnValidateRecordsOK.ClientID)
                            + "&nobutton=" + Cryptography.Encrypt(btnValidateRecordsNO.ClientID);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "perform_validation", "setTimeout(function () { $('#hlResetValidation').trigger('click'); }, 500); ", true);
                        return;
                    }
                    if (ViewState["perform_validation"] != null && ViewState["perform_validation"].ToString() == "yes")
                    {
                        bAllowReValidation = true;
                    }

                    if (bCalculationChanged == true && ViewState["perform_validation"] == null && bAllowReValidation == false
                        && (ViewState["OldFormulaV"] != "" || ViewState["OldFormulaE"] != "" || ViewState["OldFormulaW"] != ""))
                    {
                        hlResetValidation.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                             Cryptography.Encrypt("Calculation formula has been changed, Would you like to revalidate all records?")
                              + "&okbutton=" + Cryptography.Encrypt(btnValidateRecordsOK.ClientID)
                            + "&nobutton=" + Cryptography.Encrypt(btnValidateRecordsNO.ClientID);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "perform_validation", "setTimeout(function () { $('#hlResetValidation').trigger('click'); }, 500);", true);
                        return;
                    }
                  


                    if (bAllowReValidation == true && bCalculationChanged == false && ViewState["OldFormulaV"].ToString().ToUpper() != strNewFormulaV.ToUpper())
                    {
                        if (ddlType.SelectedValue == "number" || ddlType.SelectedValue == "calculation")
                        {

                            if (ViewState["InvalidConfirmed"] != null)
                            {
                                //if (ViewState["InvalidConfirmed"].ToString() == "ok")
                                //{
                                //    editColumn.ValidationOnEntry = strValidationOnEntry;
                                //}
                            }
                            else
                            {
                                //first time
                                //get list of Records
                                bool bUpdateDB = false;
                                if (editColumn.ValidationCanIgnore != null && (bool)editColumn.ValidationCanIgnore)
                                {
                                    bUpdateDB = true;
                                }
                                string strInvalidRecordIDs = "";
                                string strValidRecordIDs = "";
                                RecordManager.ets_AdjustValidFormulaChanges(editColumn, ref strInvalidRecordIDs, ref strValidRecordIDs, bUpdateDB, "");

                                if (strInvalidRecordIDs != "" && bUpdateDB == false)
                                {
                                    ViewState["InvalidRecordIDs"] = strInvalidRecordIDs;
                                    if (strValidRecordIDs != "")
                                        ViewState["ValidRecordIDs"] = strValidRecordIDs;

                                    string strValidInfo = "The Data Validation will make " + (strInvalidRecordIDs.Split(',').Length - 1).ToString() + " existing records inactive. Are you sure you wish to change this?";
                                    //divSaveGroup.Visible = false;
                                    //divValid.Visible = true;
                                    hlResetValidation.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                                              Cryptography.Encrypt(strValidInfo)
                                              + "&okbutton=" + Cryptography.Encrypt(btnConfirmInvalidOK.ClientID)
                                              + "&nobutton=" + Cryptography.Encrypt(btnConfirmInvalidNO.ClientID);

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "perform_validation", "setTimeout(function () { $('#hlResetValidation').trigger('click'); }, 500);", true);

                                    return;
                                }
                                else if (strValidRecordIDs != "" && bUpdateDB == false)
                                {
                                    RecordManager.ets_AdjustValidFormulaChanges(editColumn, ref strInvalidRecordIDs, ref strValidRecordIDs, true, "");
                                    bAdjustValidPreformed = true;
                                }
                                else
                                {
                                    if (bUpdateDB && (strInvalidRecordIDs != ""))
                                    {
                                        strNotifications = TheDatabase.GetAdjustValidationNotification(strNotifications, strInvalidRecordIDs, strValidRecordIDs, editColumn);
                                    }
                                }

                            }
                        }
                    }


                    hlResetValidation.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                          Cryptography.Encrypt("Would you like to revalidate all records?")
                          + "&okbutton=" + Cryptography.Encrypt(btnRevalidateRecords.ClientID);


                    ViewState["perform_validation"] = null;
                    if (chkImport.Checked)
                    {
                        txtConstant.Text = "";
                    }

                    if (txtConstant.Text != "")
                    {
                        double dTest;
                        if (double.TryParse(txtConstant.Text, out dTest))
                        {
                        }
                        else
                        {
                            lblMsg.Text = "Constant should be a numeric value!";
                            txtConstant.Focus();
                            return;
                        }

                    }

                    editColumn.Constant = txtConstant.Text;
                    


                    editColumn.DefaultValue = txtDefaultValue.Text;
                    //if (chkDetailPage.Checked == false
                    //    || chkShowWhen.Checked == false)
                    //{
                    //    editColumn.HideColumnID = null;
                    //    editColumn.HideColumnValue = "";
                    //}

                    //if (chkDetailPage.Checked && chkShowWhen.Checked)
                    //{
                    //    if (hfHideColumnID.Value != "" && hfHideColumnValue.Value != "")
                    //    {
                    //        editColumn.HideColumnID = int.Parse(hfHideColumnID.Value);
                    //        editColumn.HideColumnValue = hfHideColumnValue.Value;
                    //    }
                    //}


                    if (ddlDefaultValue.SelectedValue == "value" && txtDefaultValue.Text.Trim() != "")
                    {

                        editColumn.DefaultValue = txtDefaultValue.Text;
                    }
                    else
                    {
                        editColumn.DefaultValue = "";
                    }
                    if (ddlDefaultValue.SelectedValue == "value")
                    {

                        if (ddlType.SelectedValue == "date_time")
                        {
                            editColumn.DefaultValue = "yes";
                        }

                    }




                    if (chkExceedence.Checked)
                    {
                        if (txtExceedence.Text != "")
                        {
                            editColumn.ShowGraphExceedance = double.Parse(txtExceedence.Text);

                        }
                        else
                        {
                            editColumn.ShowGraphExceedance = null;
                        }
                    }
                    else
                    {
                        editColumn.ShowGraphExceedance = null;
                    }


                    if (chkWarning.Checked)
                    {
                        if (txtWarning.Text != "")
                        {
                            editColumn.ShowGraphWarning = double.Parse(txtWarning.Text);
                        }
                        else
                        {
                            editColumn.ShowGraphWarning = null;
                        }
                    }
                    else
                    {
                        editColumn.ShowGraphWarning = null;
                    }


                    if (chkMaximumValueat.Checked)
                    {
                        if (txtMaximumValueat.Text != "")
                        {
                            editColumn.MaxValueAt = double.Parse(txtMaximumValueat.Text);
                        }
                        else
                        {
                            editColumn.MaxValueAt = null;
                        }
                    }
                    else
                    {
                        editColumn.MaxValueAt = null;
                    }

                    if (ddlDefaultGraphDefinition.SelectedValue != "-1")
                    {
                        editColumn.DefaultGraphDefinitionID = int.Parse(ddlDefaultGraphDefinition.SelectedValue);
                    }
                    else
                    {
                        editColumn.DefaultGraphDefinitionID = null;
                    }


                    //editColumn.IsNumeric = chkIsNumeric.Checked;
                    editColumn.DateCalculationType = "";

                    //if (ddlType.Text == "data_retriever")
                    //{
                    //    editColumn.ColumnType = "data_retriever";
                    //    if (ddlDataRetriver.SelectedValue == "")
                    //    {
                    //        lblMsg.Text = "Please select a data retriever!";
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        editColumn.DataRetrieverID = int.Parse(ddlDataRetriver.SelectedValue);
                    //    }
                    //}
                    //else
                    //{
                    //    editColumn.DataRetrieverID = null;
                    //}

                    if (ddlType.Text != "number")
                    {
                        if (ddlType.Text != "location")
                        {
                            if (ddlType.Text != "calculation")
                            {
                                editColumn.ShowTotal = false;
                                editColumn.IsRound = false;
                            }
                        }
                        editColumn.IgnoreSymbols = false;
                        editColumn.NumberType = null;

                    }



                    //if (ddlSumXCellBackColour.Value != "")
                    //{
                    //    editColumn.SummaryCellBackColor = ddlSumXCellBackColour.Value;
                    //}
                    //else
                    //{
                    //    editColumn.SummaryCellBackColor = "";
                    //}

                    if (ddlType.Text != "text")
                    {
                        editColumn.TextHeight = null;
                        editColumn.TextWidth = null;

                    }
                    if (ddlType.Text == "text")
                    {
                        editColumn.ColumnType = ddlType.Text;
                        editColumn.TextType = ddlTextType.Text;
                        editColumn.DropDownType = "";
                        editColumn.DropdownValues = "";
                        editColumn.TableTableID = null;
                        editColumn.LinkedParentColumnID = null;
                        editColumn.DisplayColumn = "";



                        if (txtTextHeight.Text != "")
                        {
                            editColumn.TextHeight = int.Parse(txtTextHeight.Text);
                        }
                        else
                        {
                            editColumn.TextHeight = null;
                        }
                        if (txtTextWidth.Text != "")
                        {
                            editColumn.TextWidth = int.Parse(txtTextWidth.Text);
                        }
                        else
                        {
                            editColumn.TextWidth = null;
                        }

                    }
                    else if (ddlType.Text == "calculation")
                    {
                        editColumn.ColumnType = ddlType.Text;
                        //editColumn.TextType = ddlCalculationType.Text;
                        //editColumn.RegEx = txtCalFinancialSymbol.Text;
                        editColumn.IsRound = null;
                        if (chkCheckUnlikelyValue.Checked)
                        {
                            editColumn.CheckUnlikelyValue = true;
                        }
                        else
                        {
                            editColumn.CheckUnlikelyValue = false;
                        }
                        if (chkRound.Checked && txtRoundNumber.Text != "")
                        {
                            int iTest;
                            if (int.TryParse(txtRoundNumber.Text, out iTest))
                            {
                            }
                            else
                            {
                                lblMsg.Text = "Round number should be a numeric value!";
                                txtRoundNumber.Focus();
                                return;
                            }
                            editColumn.IsRound = true;
                        }
                        editColumn.RoundNumber = txtRoundNumber.Text == "" ? null : (int?)int.Parse(txtRoundNumber.Text);

                    }
                    else if (ddlType.Text == "number")
                    {
                        editColumn.ColumnType = ddlType.Text;
                        editColumn.NumberType = int.Parse(ddlNumberType.Text);

                        if (txtTextWidth.Text != "")
                        {
                            editColumn.TextWidth = int.Parse(txtTextWidth.Text);
                        }
                        else
                        {
                            editColumn.TextWidth = null;
                        }

                        //if (chkShowTotal.Checked)
                        //{
                        //    editColumn.ShowTotal = true;
                        //}
                        //else
                        //{
                        //    editColumn.ShowTotal = false;
                        //}

                        if (chkIgnoreSymbols.Checked)
                        {
                            editColumn.IgnoreSymbols = true;
                        }
                        else
                        {
                            editColumn.IgnoreSymbols = false;
                        }

                        if (chkCheckUnlikelyValue.Checked)
                        {
                            editColumn.CheckUnlikelyValue = true;
                        }
                        else
                        {
                            editColumn.CheckUnlikelyValue = false;
                        }

                        if (ddlType.SelectedValue != "location")
                        {
                            editColumn.IsRound = chkRound.Checked;
                        }

                        if (chkRound.Checked && txtRoundNumber.Text != "")
                        {
                            int iTest;
                            if (int.TryParse(txtRoundNumber.Text, out iTest))
                            {
                            }
                            else
                            {
                                lblMsg.Text = "Round number should be a numeric value!";
                                txtRoundNumber.Focus();
                                return;
                            }

                        }
                        editColumn.RoundNumber = txtRoundNumber.Text == "" ? null : (int?)int.Parse(txtRoundNumber.Text);

                        if (ddlNumberType.Text == "6")
                        {
                            editColumn.TextType = txtSymbol.Text;
                        }

                        if (ddlNumberType.Text == "7")
                        {
                            //newColumn.TextType = txtSymbol.Text;
                            SliderField newSliderField = new SliderField();
                            if (txtSliderMin.Text.Trim() == "")
                            {
                                newSliderField.Min = 0;
                            }
                            else
                            {
                                newSliderField.Min = int.Parse(txtSliderMin.Text.Trim());
                            }
                            if (txtSliderMax.Text.Trim() == "")
                            {
                                newSliderField.Max = 100;
                            }
                            else
                            {
                                newSliderField.Max = int.Parse(txtSliderMax.Text.Trim());
                            }
                            editColumn.DropdownValues = newSliderField.GetJSONString();
                        }


                        if (ddlNumberType.Text == "4")
                        {
                            //avg

                            int iTest;
                            if (int.TryParse(txtAvgNumValues.Text, out iTest))
                            {
                            }
                            else
                            {
                                lblMsg.Text = "Avg Num Value should be a numeric value!";
                                txtAvgNumValues.Focus();
                                return;
                            }



                            if (editColumn.AvgColumnID == null)
                            {
                                bAvgDefinitionChanged = true;
                            }
                            else
                            {
                                if (int.Parse(ddlAvgColumn.SelectedValue) != editColumn.AvgColumnID
                                    || editColumn.AvgNumberOfRecords != int.Parse(txtAvgNumValues.Text))
                                {
                                    bAvgDefinitionChanged = true;
                                }

                            }
                            editColumn.AvgNumberOfRecords = int.Parse(txtAvgNumValues.Text);

                            editColumn.AvgColumnID = int.Parse(ddlAvgColumn.SelectedValue);
                        }
                        else
                        {
                            editColumn.AvgNumberOfRecords = null;

                        }


                        if (ddlNumberType.Text == "5")
                        {
                            //record count
                            if (ddlRecordCountTable.Items.Count == 0)
                            {
                                lblMsg.Text = "No child record for this table!";
                                ddlRecordCountTable.Focus();
                                return;
                            }

                            editColumn.TableTableID = int.Parse(ddlRecordCountTable.SelectedValue);
                            editColumn.DropdownValues = ddlRecordCountClick.SelectedValue;

                        }

                    }
                    else if (ddlType.Text == "radiobutton")
                    {
                        editColumn.ColumnType = ddlType.Text;
                        editColumn.DropDownType = ddlOptionType.SelectedValue;
                        editColumn.VerticalList = chkDisplayVertical.Checked;
                        if (ddlOptionType.SelectedValue == "value_image")
                        {
                            editColumn.ImageOnSummary = chkImageOnSummary.Checked;
                            if (chkImageOnSummary.Checked && txtImageOnSummaryMaxHeight.Text != "")
                            {
                                editColumn.TextHeight = int.Parse(txtImageOnSummaryMaxHeight.Text);
                            }
                            else
                            {
                                editColumn.TextHeight = null;
                            }
                            if (Session["OptionImage"] != null)
                            {
                                DataTable dtOptionImage = (DataTable)Session["OptionImage"];
                                List<OptionImage> lstOptionImage = new List<OptionImage>();
                                foreach (DataRow dr in dtOptionImage.Rows)
                                {
                                    OptionImage aOptionImage = new OptionImage();
                                    aOptionImage.OptionImageID = dr["OptionImageID"].ToString();
                                    aOptionImage.Value = dr["Value"].ToString();
                                    aOptionImage.FileName = dr["FileName"].ToString();
                                    aOptionImage.UniqueFileName = dr["UniqueFileName"].ToString();
                                    lstOptionImage.Add(aOptionImage);

                                }

                                OptionImageList theOptionImageList = new OptionImageList();
                                theOptionImageList.ImageList = lstOptionImage;

                                editColumn.DropdownValues = theOptionImageList.GetJSONString();
                            }

                        }
                        else
                        {
                            if (txtDropdownValues.Text == "")
                            {
                                lblMsg.Text = "Please enter Radio buttons value!";
                                txtDropdownValues.Focus();
                                return;
                            }
                            else
                            {
                                editColumn.DropdownValues = txtDropdownValues.Text;
                            }
                        }
                    }
                    else if (ddlType.Text == "listbox")
                    {
                        editColumn.ColumnType = ddlType.Text;
                        editColumn.DropDownType = ddlListBoxType.SelectedValue;

                        if (txtTextWidth.Text != "")
                            editColumn.TextWidth = int.Parse(txtTextWidth.Text);

                        if (txtTextHeight.Text != "")
                            editColumn.TextHeight = int.Parse(txtTextHeight.Text);

                        if (txtTextHeight.Text != "")
                        {
                            if (editColumn.TextHeight < 7)
                            {
                                editColumn.TextHeight = 7;
                            }
                        }



                        if (txtDropdownValues.Text == "" && ddlListBoxType.SelectedValue != "table")
                        {
                            lblMsg.Text = "Please enter list box values!";
                            txtDropdownValues.Focus();
                            return;
                        }
                        else
                        {
                            editColumn.DropdownValues = txtDropdownValues.Text;
                        }

                        if (chkListCheckBox.Checked)
                        {
                            editColumn.DateCalculationType = "checkbox";
                        }
                        else
                        {
                            editColumn.DateCalculationType = "";
                        }


                        if (ddlListBoxType.SelectedValue == "table")
                        {
                            editColumn.DropdownValues = "";
                            if (hfDisplayColumnsFormula.Value == "")
                            {
                                bool bShowMessage = false;
                                if (ddDDDisplayColumn.SelectedValue == "")
                                {
                                    bShowMessage = true;
                                }
                                else
                                {
                                    hfDisplayColumnsFormula.Value = "[" + ddDDDisplayColumn.SelectedItem.Text + "]";
                                }
                                if (bShowMessage)
                                {
                                    lblMsg.Text = "Please select a display field.";
                                    return;
                                }
                            }
                            string strLinkedParentColumn = ddDDLinkedParentColumn.SelectedValue;
                            if (ddDDLinkedParentColumn.SelectedValue == "")
                            {

                                strLinkedParentColumn = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE SystemName='recordid' AND TableID=" + ddlDDTable.SelectedValue);
                                //lblMsg.Text = "Please select a linked field.";
                                //return;
                            }

                            editColumn.TableTableID = int.Parse(ddlDDTable.SelectedValue);

                            if (strLinkedParentColumn != "")
                                editColumn.LinkedParentColumnID = int.Parse(strLinkedParentColumn);

                            editColumn.DisplayColumn = hfDisplayColumnsFormula.Value;

                        }



                    }
                    else if (ddlType.Text == "checkbox")
                    {
                        editColumn.ColumnType = ddlType.Text;
                        editColumn.DropdownValues = txtTickedValue.Text + Environment.NewLine + txtUntickedValue.Text
                            + Environment.NewLine + (chkTickedByDefault.Checked == true ? "yes" : "no");


                    }
                    else if (ddlType.Text == "location")
                    {
                        editColumn.ColumnType = ddlType.Text;

                        editColumn.ShowTotal = chkLocationAddress.Checked;
                        editColumn.IsRound = chkLatLong.Checked;

                        if (chkShowMap.Checked)
                        {
                            if (txtMapHeight.Text == "")
                                txtMapHeight.Text = "200";
                            editColumn.TextHeight = int.Parse(txtMapHeight.Text);

                            if (txtMapWidth.Text == "")
                                txtMapWidth.Text = "400";
                            editColumn.TextWidth = int.Parse(txtMapWidth.Text);
                        }
                        else
                        {
                            editColumn.TextHeight = null;
                            editColumn.TextWidth = null;
                        }


                        if (ddlMapPinHoverColumnID.SelectedValue != "")
                        {
                            editColumn.MapPinHoverColumnID = int.Parse(ddlMapPinHoverColumnID.SelectedValue);
                        }
                        else
                        {
                            editColumn.MapPinHoverColumnID = null;
                        }


                    }
                    else if (ddlType.Text == "dropdown")
                    {
                        editColumn.ColumnType = ddlType.Text;


                        if (txtTextWidth.Text != "")
                            editColumn.TextWidth = int.Parse(txtTextWidth.Text);

                        if (ddlDDType.SelectedValue == "values"
                            || ddlDDType.SelectedValue == "value_text")
                        {
                            editColumn.DropDownType = ddlDDType.SelectedValue;
                            if (txtDropdownValues.Text == "")
                            {
                                lblMsg.Text = "Please enter Dropdown values!";
                                txtDropdownValues.Focus();
                                return;
                            }
                            else
                            {
                                editColumn.DropdownValues = txtDropdownValues.Text;
                            }
                        }


                        if (ddlDDType.SelectedValue == "ct" || ddlDDType.SelectedValue == "lt")
                        {
                            //so its table
                            editColumn.ShowViewLink = ddlShowViewLink.SelectedValue;
                            if (chkPredictive.Checked)
                            {
                                editColumn.DropDownType = "table";
                            }
                            else
                            {
                                editColumn.DropDownType = "tabledd";
                            }

                            if (hfDisplayColumnsFormula.Value == "")
                            {
                                bool bShowMessage = false;
                                if (ddDDDisplayColumn.SelectedValue == "")
                                {
                                    bShowMessage = true;
                                }
                                else
                                {
                                    hfDisplayColumnsFormula.Value = "[" + ddDDDisplayColumn.SelectedItem.Text + "]";
                                }
                                if (bShowMessage)
                                {
                                    lblMsg.Text = "Please select a display field.";
                                    return;
                                }
                            }

                            string strLinkedParentColumn = ddDDLinkedParentColumn.SelectedValue;

                            if (ddDDLinkedParentColumn.SelectedValue == "")
                            {

                                strLinkedParentColumn = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE SystemName='recordid' AND TableID=" + ddlDDTable.SelectedValue);

                                //lblMsg.Text = "Please select a linked field.";
                                //return;
                            }

                            if (ViewState["Old_TableTableID"] != null)
                            {
                                if (ViewState["Old_TableTableID"].ToString() != ddlDDTable.SelectedValue)
                                {
                                    //we are changeing ParentTable
                                    //check if parent table has record count field

                                    DataTable dtNumberOfLinked = Common.DataTableFromText(@"SELECT DisplayName FROM [Column] WHERE TableID="
                                        + editColumn.TableID.ToString() + " AND TableTableID="
                                        + ViewState["Old_TableTableID"].ToString() + " AND ColumnType='dropdown' ");

                                    if (dtNumberOfLinked.Rows.Count > 1)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {

                                        DataTable dtParentColumn = Common.DataTableFromText(@"SELECT DisplayName FROM [Column] WHERE TableID="
                                            + ViewState["Old_TableTableID"].ToString()
                                            + " AND TableTableID=" + editColumn.TableID.ToString() + " AND ColumnType='number' AND NumberType=5");


                                        if (dtParentColumn.Rows.Count > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please remove the Record Count field " + dtParentColumn.Rows[0][0].ToString() + " link from the parent table and then try again.');", true);
                                            return;
                                        }
                                    }


                                }
                            }



                            editColumn.TableTableID = int.Parse(ddlDDTable.SelectedValue);

                            if (strLinkedParentColumn != "")
                                editColumn.LinkedParentColumnID = int.Parse(strLinkedParentColumn);



                            editColumn.DisplayColumn = hfDisplayColumnsFormula.Value;



                            if (ddlDDType.SelectedValue == "lt")//chkFilterValues.Checked
                            {
                                //so its filtered

                                editColumn.TableTableID = int.Parse(ddlDDTable.SelectedValue);
                                editColumn.DisplayColumn = hfDisplayColumnsFormula.Value;

                                editColumn.ParentColumnID = int.Parse(ddlFilter.SelectedValue);


                            }
                            else
                            {
                                if (hfFilterParentColumnID.Value != "" && chkFiltered.Checked)
                                {
                                    editColumn.FilterParentColumnID = int.Parse(hfFilterParentColumnID.Value);
                                    editColumn.FilterOperator = hfFilterOperator.Value;
                                    if (hfFilterOtherColumnID.Value != "")
                                    {
                                        editColumn.FilterOtherColumnID = int.Parse(hfFilterOtherColumnID.Value);
                                    }
                                    else
                                    {
                                        editColumn.FilterValue = hfFilterValue.Value;
                                    }
                                }
                            }
                        }



                    }
                    else if (ddlType.SelectedValue == "date_time")
                    {
                        editColumn.ColumnType = ddlDateTimeType.SelectedValue;
                    }
                    else if (ddlType.SelectedValue == "staticcontent")
                    {
                        if (chkAllowContenEdit.Checked)
                        {
                            //content
                            editColumn.ColumnType = "content";
                            editColumn.DropdownValues = edtContent.Text;
                        }
                        else
                        {
                            editColumn.ColumnType = ddlType.SelectedValue;
                            editColumn.DropdownValues = edtContent.Text;
                        }

                    }
                    else if (ddlType.SelectedValue == "trafficlight")
                    {
                        editColumn.ColumnType = ddlType.SelectedValue;
                        bool bFoundValue = false;

                        if (ddlTLControllingField.SelectedValue == "")
                        {
                            lblMsg.Text = "Please select a Controlling Field.";
                            return;
                        }

                        string strTrafficLightValue = "<?xml version='1.0' encoding='utf-8' ?><items>";

                        if (cbcValue1.ddlYAxisV != "" && cbcValue1.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue1.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage1.SelectedValue) + "</ImageFile></item>";
                        }

                        if (cbcValue2.ddlYAxisV != "" && cbcValue2.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue2.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage2.SelectedValue) + "</ImageFile></item>";
                        }

                        if (cbcValue3.ddlYAxisV != "" && cbcValue3.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue3.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage3.SelectedValue) + "</ImageFile></item>";
                        }

                        if (cbcValue4.ddlYAxisV != "" && cbcValue4.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue4.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage4.SelectedValue) + "</ImageFile></item>";
                        }

                        if (cbcValue5.ddlYAxisV != "" && cbcValue5.TextValue != "")
                        {
                            bFoundValue = true;
                            strTrafficLightValue = strTrafficLightValue + "<item><value>" + HttpUtility.HtmlEncode(cbcValue5.TextValue)
                                + "</value><ImageFile>" + HttpUtility.HtmlEncode(ddlTLImage5.SelectedValue) + "</ImageFile></item>";
                        }

                        if (bFoundValue == false)
                        {
                            lblMsg.Text = "Please select some Value and Traffic image.";
                            return;
                        }
                        else
                        {
                            strTrafficLightValue = strTrafficLightValue + "</items>";
                            editColumn.TrafficLightColumnID = int.Parse(ddlTLControllingField.SelectedValue);
                            editColumn.TrafficLightValues = strTrafficLightValue;
                        }

                    }
                    else if (ddlType.SelectedValue == "button")
                    {
                        editColumn.ColumnType = ddlType.SelectedValue;
                        //if (txtSPToRun.Text.Trim() == "")
                        //{
                        //    lblMsg.Text = "Please enter a SP name.";
                        //    return;
                        //}
                        ColumnButtonInfo aButtonInfo = new ColumnButtonInfo();
                        if (chkSPToRun.Checked)
                        {
                            aButtonInfo.SPToRun = txtSPToRun.Text;
                        }
                        else
                        {
                            aButtonInfo.SPToRun = "";
                        }
                        if (chkButtonOpenLink.Checked)
                        {
                            aButtonInfo.OpenLink = txtButtonOpenLink.Text;
                        }
                        else
                        {
                            aButtonInfo.OpenLink = "";
                        }
                        aButtonInfo.ImageFullPath = hfButtonValue.Value;// txtButtonImage.Text;
                        if (chkButtonWarningMessage.Checked && txtButtonWarningMessage.Text != "")
                        {
                            aButtonInfo.WarningMessage = txtButtonWarningMessage.Text;
                        }

                        editColumn.ButtonInfo = aButtonInfo.GetJSONString();
                        if (txtTextHeight.Text != "")
                            editColumn.TextHeight = int.Parse(txtTextHeight.Text);
                        if (txtTextWidth.Text != "")
                            editColumn.TextWidth = int.Parse(txtTextWidth.Text);

                    }
                    else
                    {
                        editColumn.ButtonInfo = "";
                        editColumn.ColumnType = ddlType.SelectedValue;
                    }


                    editColumn.Notes = txtNotes.Text.Trim();
                    //editColumn.Alignment = ddlAlignment.Text == "-1" ? null : ddlAlignment.Text;
                    editColumn.LastUpdatedUserID = (int)_ObjUser.UserID;

                    if (editColumn.ColumnType == "image")
                    {
                        if (txtImageHeightSummary.Text != "")
                        {
                            editColumn.TextWidth = int.Parse(txtImageHeightSummary.Text);
                        }
                        else
                        {
                            editColumn.TextWidth = null;
                        }
                        if (txtImageHeightDetail.Text != "")
                        {
                            editColumn.TextHeight = int.Parse(txtImageHeightDetail.Text);
                        }
                        else
                        {
                            editColumn.TextHeight = null;
                        }
                    }

                    if (editColumn.ColumnType == "calculation")
                    {
                        editColumn.IsRound = chkRound.Checked;
                        //editColumn.ShowTotal = chkShowTotal.Checked;
                        editColumn.CheckUnlikelyValue = chkCheckUnlikelyValue.Checked;

                        editColumn.TextType = ddlCalculationType.Text;
                        editColumn.RegEx = txtCalFinancialSymbol.Text;

                        if (ddlCalculationType.SelectedValue == "d")
                        {
                            editColumn.DateCalculationType = ddlDateResultFormat.SelectedValue;
                            editColumn.RegEx = "";
                            editColumn.IsRound = null;
                            editColumn.CheckUnlikelyValue = null;
                        }
                        else
                        {
                            editColumn.DateCalculationType = "";
                        }
                    }

                  
                    editColumn.DefaultType = ddlDefaultValue.SelectedValue;
                    editColumn.DefaultUpdateValues = false;
                    if (ddlDefaultValue.SelectedValue == "parent")
                    {
                        if (ddlDefaultParentColumn.SelectedValue != "" && ddlDefaultParentTable.SelectedValue != "")
                        {
                            editColumn.DefaultColumnID = int.Parse(ddlDefaultParentColumn.SelectedValue);
                            editColumn.DefaultRelatedTableID = int.Parse(ddlDefaultParentTable.SelectedValue);
                            editColumn.DefaultUpdateValues = chkDefaultSyncData.Checked;
                        }
                        else
                        {
                            editColumn.DefaultColumnID = null;
                            editColumn.DefaultRelatedTableID = null;

                        }

                    }
                    else
                    {
                        editColumn.DefaultColumnID = null;
                        editColumn.DefaultRelatedTableID = null;
                    }

                    try
                    {

                        if (editColumn.ColumnType == "number" || editColumn.ColumnType == "calculation")
                        {

                        }
                        else
                        {
                            editColumn.ValidationOnEntry = "";
                            editColumn.ValidationOnWarning = "";
                            editColumn.ValidationOnExceedance = "";
                        }


                      

                        if (chkColumnColour.Checked)
                        {
                            editColumn.ColourCells = true;
                        }
                        else
                        {
                            editColumn.ColourCells = false;
                        }

                        if (editColumn.ColumnType != "calculation")
                        {
                            editColumn.Calculation = "";
                        }

                        if (ddlType.SelectedValue == "staticcontent" || ddlType.SelectedValue == "button")
                        {
                            editColumn.DisplayTextSummary = "";
                        }

                        RecordManager.ets_Column_Update(editColumn);


                        if (bDisplayNameChanged)
                        {
                            TheDatabaseS.Column_ReplaceDisplayColumn((int)editColumn.TableID, strOldDislayName, editColumn.DisplayName);
                        }


                        if (chkDetailPage.Checked == false
                       || chkShowWhen.Checked == false)
                        {
                            Common.ExecuteText("DELETE ShowWhen WHERE ColumnID=" + _qsColumnID);
                        }
                        if (chkColumnColour.Checked == false)
                        {
                            Common.ExecuteText("DELETE ColumnColour WHERE ColumnID=" + _qsColumnID);
                        }

                        //update child tables

                        if (iTableTableID_old != null || editColumn.TableTableID != null)
                        {
                            if (iTableTableID_old != editColumn.TableTableID)
                            {
                                //changed


                                if (iTableTableID_old != null)
                                {
                                    Common.ExecuteText("DELETE FROM TableChild WHERE ParentTableID=" + iTableTableID_old.ToString() + " AND ChildTableID=" + editColumn.TableID.ToString());
                                }

                                if (editColumn.TableTableID != null && editColumn.LinkedParentColumnID != null)
                                {
                                    DataTable dtTemp = Common.DataTableFromText("SELECT * FROM TableChild WHERE ParentTableID=" + editColumn.TableTableID.ToString() + " AND ChildTableID=" + editColumn.TableID.ToString() + " AND DetailPageType='list'");

                                    if (dtTemp.Rows.Count > 0)
                                    {
                                        //do nothing as we have this relationgship
                                    }
                                    else
                                    {

                                        TableChild theTableChild = new TableChild(null, editColumn.TableTableID,
                                            editColumn.TableID, "", "list");
                                        theTableChild.ShowAddButton = true;
                                        theTableChild.ShowEditButton = true;
                                        RecordManager.ets_TableChild_Insert(theTableChild);
                                    }
                                }



                            }
                        }


                        if (editColumn.SystemName.ToLower() == "datetimerecorded")
                        {
                            if (_theTable.DateFormat != ddlDateFormat.SelectedValue)
                            {
                                _theTable.DateFormat = ddlDateFormat.SelectedValue;
                                RecordManager.ets_Table_Update(_theTable);
                            }

                            if (_theTable.IsImportPositional == true)
                            {
                                //if positional then reserve a field for Time Samled
                                if (editColumn.PositionOnImport != null)
                                {
                                    DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnID, 
                                    PositionOnImport FROM [Column] WHERE   ColumnID<>" + editColumn.ColumnID.ToString() + " AND TableID=" + _theTable.TableID.ToString() + " AND PositionOnImport=" + (editColumn.PositionOnImport + 1).ToString());

                                    if (dtTemp.Rows.Count > 0)
                                    {
                                        //found so lets increase

                                        dtTemp = Common.DataTableFromText(@"SELECT ColumnID, 
                                    PositionOnImport FROM [Column] WHERE   TableID=" + _theTable.TableID.ToString() + " AND PositionOnImport>" + editColumn.PositionOnImport.ToString());

                                        if (dtTemp.Rows.Count > 0)
                                        {
                                            foreach (DataRow dr in dtTemp.Rows)
                                            {
                                                Common.ExecuteText("UPDATE Column SET PositionOnImport=" + (int.Parse(dr["PositionOnImport"].ToString()) + 1).ToString() + " WHERE ColumnID=" + dr["ColumnID"].ToString());

                                            }
                                        }

                                    }
                                 
                                }

                            }
                          

                        }
                       



                        editColumn = RecordManager.ets_Column_Details((int)editColumn.ColumnID);

                        if (ViewState["InvalidConfirmed"] != null && bAllowReValidation)
                        {

                            if (ViewState["InvalidConfirmed"].ToString() == "ok")
                            {
                                if (ViewState["InvalidRecordIDs"] != null)
                                {
                                    string strTemp = "";

                                    string strInvalidRecordIDs = "";
                                    string strValidRecordIDs = "";

                                    RecordManager.ets_AdjustValidFormulaChanges(editColumn, ref strInvalidRecordIDs, ref strValidRecordIDs, true, "");
                                    bAdjustValidPreformed = true;
                                    string strInvalidCount = "";
                                    if (strInvalidRecordIDs != "")
                                    {
                                        strInvalidCount = (strInvalidRecordIDs.Split(',').Length - 1).ToString();
                                        RecordManager.SendAdjustValidationSMSandEmail(editColumn, double.Parse(strInvalidCount), ref strTemp, false);

                                    }
                                    strError = strError + strTemp;
                                    if (strInvalidRecordIDs != "" || strValidRecordIDs != "")
                                        strNotifications = TheDatabase.GetAdjustValidationNotification(strNotifications, strInvalidRecordIDs, strValidRecordIDs, editColumn);



                                }
                                //send emails
                                //if (ViewState["NoOfInvalid"] != null)
                                //{
                                //    string strTemp = "";
                                //    RecordManager.SendAdjustValidationSMSandEmail(editColumn, double.Parse(ViewState["NoOfInvalid"].ToString()), ref strTemp, false);
                                //    strError = strError + strTemp;

                                //    strNotifications = strNotifications + "Validation Adjusted:" + editColumn.DisplayColumn + " " + ViewState["NoOfInvalid"].ToString() + " invalid records.";
                                //}
                            }
                            else
                            {
                                //no was selected
                                if (ViewState["OldFormulaV_C"] != null)
                                {
                                    Common.ExecuteText("DELETE FROM [Condition] WHERE ColumnID=" + editColumn.ColumnID);
                                    DataTable dtConditons = (DataTable)ViewState["OldFormulaV_C"];
                                    foreach (DataRow dr in dtConditons.Rows)
                                    {
                                        Condition newCondition = new Condition(null, editColumn.ColumnID, dr["ConditionType"].ToString(),
                              int.Parse(dr["CheckColumnID"].ToString()), dr["CheckFormula"].ToString(), dr["CheckValue"].ToString());
                                        UploadWorld.dbg_Condition_Insert(newCondition);
                                    }
                                }
                                else
                                {
                                    if (ViewState["OldFormulaV"] != null)
                                    {
                                        editColumn.ValidationOnEntry = ViewState["OldFormulaV"].ToString();
                                        RecordManager.ets_Column_Update(editColumn);
                                    }
                                }
                            }
                        }



                        //

                        if (bAdjustValidPreformed == false && (bExceedanceChanged || bWarningChanged) && bAllowReValidation)
                        {

                            string strInvalidRecordIDs = "";
                            string strValidRecordIDs = "";

                            RecordManager.ets_AdjustValidFormulaChanges(editColumn, ref strInvalidRecordIDs, ref strValidRecordIDs, true, "");
                        }


                        //if (_bShowExceedances && bExceedanceChanged)
                        //{
                        //    //lets do the warning job
                        //    string strTemp = "";
                        //    RecordManager.ets_AdjustExceedanceFormulaChanges(editColumn, ref strTemp, false);

                        //    int iTest = 0;
                        //    if (int.TryParse(strTemp, out iTest))
                        //    {
                        //        if (strTemp != "" & strTemp != "0")
                        //            strNotifications = strNotifications + "Exceedance Adjusted:" + editColumn.DisplayColumn + " " + strTemp + " records.";
                        //    }
                        //    else
                        //    {
                        //        strError = strError + "<br/>" + strTemp;
                        //    }

                        //}


                        //if (bWarningChanged)
                        //{
                        //    //lets do the warning job
                        //    string strTemp = "";
                        //    RecordManager.ets_AdjustWarningFormulaChanges(editColumn, ref strTemp, false);
                        //    int iTest = 0;
                        //    if (int.TryParse(strTemp, out iTest))
                        //    {
                        //        if (strTemp != "" & strTemp!="0")
                        //            strNotifications = strNotifications + "Warning Adjusted:" + editColumn.DisplayColumn + " " + strTemp + " records.";
                        //    }
                        //    else
                        //    {
                        //        strError = strError + "<br/>" + strTemp;
                        //    }
                        //}


                        if (bUnlikelyReadingChnaged)
                        {
                            //lets do UnlikelyReading
                            string strTemp = "";
                            RecordManager.ets_AdjustUnlikelyReadings(editColumn, ref strTemp);
                            strError = strError + strTemp;
                        }




                        if (bCalculationChanged)
                        {
                            string strCalOutput = RecordManager.ets_AdjustCalculationFormulaChanges(editColumn, ref strErrorCal, bAllowReValidation);
                            strNotifications = "";// "Calculation Values Adjusted:" + editColumn.DisplayColumn + " " + strCalOutput;
                        }

                        if (bAvgDefinitionChanged)
                        {
                            string strTemp = "";
                            //RecordManager.ets_AdjustAvgNewColumn(editColumn, ref connection, ref tn, ref strTemp);

                            DataTable dtTemp = Common.DataTableFromText(@"SELECT RecordID FROM Record WHERE IsActive=1 AND TableID=" + editColumn.TableID.ToString());

                            foreach (DataRow dr in dtTemp.Rows)
                            {
                                RecordManager.ets_Record_Avg_ForARecordID(int.Parse(dr["RecordID"].ToString()));
                            }

                        }

                        if (editColumn.ColumnType == "datetime" || editColumn.ColumnType == "date")
                        {

                        }
                        else
                        {
                            //remove reminders
                            DataTable dtReminders = Common.DataTableFromText("SELECT DataReminderID FROM DataReminder WHERE ColumnID=" + editColumn.ColumnID.ToString());

                            foreach (DataRow dr in dtReminders.Rows)
                            {
                                Common.ExecuteText("DELETE DataReminderUser WHERE DataReminderID=" + dr[0].ToString());
                                Common.ExecuteText("DELETE DataReminder WHERE DataReminderID=" + dr[0].ToString());
                            }

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
                        strError = strError + ex.Message;
                        lblMsg.Text = "Duplicate field name!";
                        return;
                        //throw;
                    }


                    lblMsg.Text = "Duplicate field name!";
                    break;

                default:
                    //?
                    break;
            }



            string strBackUrl = hlBack.NavigateUrl;

            if (strNotifications.Trim().Length > 0)
            {
                Session["tdbmsg"] = strNotifications;
                if (strBackUrl.IndexOf("#topline") > -1)
                {
                    strBackUrl = strBackUrl.Replace("#topline", "");
                }

            }




            Response.Redirect(strBackUrl, false);
        }

        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Record field Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

            lblMsg.Text = "Duplicate field name!";
        }
    }


    //protected void lnkBack_Click(object sender, EventArgs e)
    //{
    //    if (Request.QueryString["SearchCriteria2"] != null)
    //    {
    //        Response.Redirect("~/Pages/Record/TableDetail.aspx?mode=" + Request.QueryString["typemode"] + "&AccountID=" + Request.QueryString["AccountID"] + "&MenuID=" + Request.QueryString["MenuID"] + "&TableID=" + Cryptography.Encrypt(hfTableID.Value) + "&SearchCriteria2=" + Request.QueryString["SearchCriteria2"], false);
    //    }
    //    else
    //    {
    //        Response.Redirect("~/Pages/Record/TableDetail.aspx?mode=" + Request.QueryString["typemode"] + "&AccountID=" + Request.QueryString["AccountID"] + "&MenuID=" + Request.QueryString["MenuID"] + "&TableID=" + Cryptography.Encrypt(hfTableID.Value), false);
    //    }
    //}


    protected void btnConfirmInvalidOK_Click(object sender, EventArgs e)
    {

        ViewState["InvalidConfirmed"] = "ok";
        lnkSave_Click(null, null);
    }

    protected void btnConfirmInvalidNO_Click(object sender, EventArgs e)
    {
        ViewState["InvalidConfirmed"] = "no";
        lnkSave_Click(null, null);
        //divSaveGroup.Visible = true;
        //divValid.Visible = false;

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    //protected void ddlDDTable_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    PopulateAnalyte();
    //}
}
