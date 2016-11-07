using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Data;
using DocGen.DAL;

public partial class Pages_UserControl_ViewDetail : System.Web.UI.UserControl
{

    Common_Pager _ViewItemPager;
    string _qsTableID = "";
    User _ObjUser;
    string _strRecordRightID = Common.UserRoleType.None;
    View _theView;
    int? _iParentTableID = null;
    bool _bOwnOrNull_View = false;
    string _strViewSession = "";
    UserRole _CurrentUserRole = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.Url.Authority == "his.thedatabase.net")
        {
            txtViewName.Enabled = false;
            
        }
        _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"]);
        _strViewSession = Cryptography.Decrypt(Request.QueryString["ViewSession"]);
        if (Request.QueryString["ParentTableID"] != null)
            _iParentTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["ParentTableID"].ToString()));


        _ObjUser = (User)Session["User"];
        _CurrentUserRole = (UserRole)Session["UserRole"];
        if(Request.QueryString["ViewID"]!=null && hfCurrentViewID.Value=="")
        {            
             _theView = ViewManager.dbg_View_Detail(int.Parse(Request.QueryString["ViewID"].ToString()));   
            if(!IsPostBack)
            {
                if(Request.QueryString["Copy"]!=null)
                {
                    int? iNewViewID = ViewManager.dbg_View_Copy((int)_theView.ViewID, (int)_ObjUser.UserID);
                    hfCurrentViewID.Value = iNewViewID.ToString();
                    Session[_strViewSession] = hfCurrentViewID.Value;
                    Session["SCid" + hfCurrentViewID.Value] = -1;
                }
            }

        }
        if (hfCurrentViewID.Value!="")
        {
            _theView = ViewManager.dbg_View_Detail(int.Parse(hfCurrentViewID.Value));
        }


        if(!IsPostBack)
        {
            hlSaveDefault.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                    Cryptography.Encrypt("Do you want to use default view?")
                    + "&okbutton=" + Cryptography.Encrypt(btnSaveDefaultOK.ClientID) ;
        
            if(_theView!=null && (_theView.UserID==null || _theView.UserID==_ObjUser.UserID ))
            {
                //null or own view
                _bOwnOrNull_View = true;

            }
        
        }
        //|| (_CurrentUserRole.IsAccountHolder!=null && (bool)_CurrentUserRole.IsAccountHolder == true)
        if (Common.HaveAccess(Session["roletype"].ToString(), "1") )
        {
            lnkAddView.Visible = true;
            lnlDeleteView.Visible = true;
            lnkNavigatePrev.Visible = true;
            lnkNavigateNext.Visible = true;
        }
        else
        {
            lnkAddView.Visible = false;
            lnlDeleteView.Visible = false;
            lnkNavigatePrev.Visible = false;
            lnkNavigateNext.Visible = false;
        }
       

        //if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        //{
        //    lnkResetDefault.Visible = false;
        //}

        //if (Request.RawUrl.IndexOf("TableDetail.aspx") == -1)
        //{
          
            //ddlViewUser.Enabled = false;

        //}
            UserRole theUserRole = (UserRole)Session["UserRole"];
            if ((bool)theUserRole.IsAdvancedSecurity)
        {
            //DataTable dtUserTable = SecurityManager.ets_UserTable_Select(null,
            //    int.Parse(_qsTableID), _ObjUser.UserID, null);

            DataTable dtUserTable = null;

            //if (_ObjUser.RoleGroupID == null)
            //{
                dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
               int.Parse(_qsTableID), theUserRole.RoleID, null);
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



        string strJSForSortingViewItem = @"

                         $(document).ready(function () {
                                    

                                    $(function () {
                                        $('#divViewItemSingleInstance').sortable({
                                            items: '.gridview_row',
                                            cursor: 'crosshair',
                                            helper: fixHelper,
                                            cursorAt: { left: 10, top: 10 },
                                            connectWith: '#divViewItemSingleInstance',
                                            handle: '.sortHandleVT',
                                            axis: 'y',
                                            distance: 15,
                                            dropOnEmpty: true,
                                            receive: function (e, ui) {
                                                $(this).find('tbody').append(ui.item);

                                            },
                                            start: function (e, ui) {
                                                ui.placeholder.css('border-top', '2px solid #00FFFF');
                                                ui.placeholder.css('border-bottom', '2px solid #00FFFF');

                                            },
                                            update: function (event, ui) {
                                                var TC = '';
                                                $('.ViewItemID').each(function () {
                                                    TC = TC + this.value.toString() + ',';
                                                });
                                                //alert(TC);
                                                document.getElementById('hfViewItemIDForColumnIndex').value = TC;
                        
                                                $('#btnViewItemIDForColumnIndex').trigger('click');

                                            }
                                        });
                                    });

                             $(function () {
                           $('.popupsavedefault').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 500,
                                height: 250,
                                titleShow: false
                            });
                        });


                                });

                        ";

        ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "strJSForSortingViewItem", strJSForSortingViewItem, true);



        string strViewItemPop = @"
                    $(function () {
                            $('.popuplinkVT').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 1000,
                                height: 400,
                                titleShow: false
                            });
                        });

                ";

        ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "strViewItemPop", strViewItemPop, true);


//        string strViewItemfancy = @"
//                    $(function () {
//                            $('.popuplinkVT').fancybox({
//                                scrolling: 'auto',
//                                type: 'iframe',
//                                'transitionIn': 'elastic',
//                                'transitionOut': 'none',
//                                width: 1000,
//                                height: 400,
//                                titleShow: false
//                            });
//                        });
//
//                ";

//        ScriptManager.RegisterStartupScript(this, this.GetType(), "strViewItemfancy", strViewItemfancy, true);



        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                string strViewPageType = "";
                if (Request.UrlReferrer.OriginalString != "")
                {
                    //if (Request.UrlReferrer.OriginalString.IndexOf("EachRecordTable.aspx") > -1)
                    //{
                    //    strViewPageType = "dash";
                    //    lnkBack.OnClientClick = "";
                    //}
                    //
                    if (Request.QueryString["noajax"] != null)
                    {
                        lnkBack.OnClientClick = "return true;";

                    }

                    if (Request.UrlReferrer.OriginalString.IndexOf("RecordTableSection.aspx") > -1)
                    {
                        strViewPageType = "dash";
                        lnkBack.OnClientClick = "";
                       
                    }

                    if (Request.UrlReferrer.OriginalString.IndexOf("RecordList.aspx") > -1)
                    {
                        strViewPageType = "list";
                    }

                    if (Request.UrlReferrer.OriginalString.IndexOf("RecordDetail.aspx") > -1)
                    {
                        strViewPageType = "child";
                    }
                    ddlViewPageType.SelectedValue = strViewPageType;
                }
            }

            //PopulateViewSortColumnDDL();
            
        }
        

        lnkAndOr1.Attributes.Add("onclick", "toggleAndOr(this,'" + hfAndOr1.ClientID + "');return false;");
        lnkAndOr2.Attributes.Add("onclick", "toggleAndOr(this,'" + hfAndOr2.ClientID + "');return false;");
        lnkAndOr3.Attributes.Add("onclick", "toggleAndOr(this,'" + hfAndOr3.ClientID + "');return false;");

        //lnkAndOr1.Attributes.Add("text", "and");

        lnkAddSearch1.Attributes.Add("onclick", "$('#" + trSearch1.ClientID + "').show();$('#" + lnkAddSearch1.ClientID + "').hide();if ($('#" + hfAndOr1.ClientID + "').val()==''){ $('#" + hfAndOr1.ClientID + "').val(document.getElementById('" + lnkAndOr1.ClientID + "').text)};return false;");//return false;
        lnkAddSearch2.Attributes.Add("onclick", "$('#" + trSearch2.ClientID + "').show();$('#" + lnkAddSearch2.ClientID + "').hide();if ($('#" + hfAndOr2.ClientID + "').val()==''){$('#" + hfAndOr2.ClientID + "').val(document.getElementById('" + lnkAndOr2.ClientID + "').text)};return false;");//return false;
        lnkAddSearch3.Attributes.Add("onclick", "$('#" + trSearch3.ClientID + "').show();$('#" + lnkAddSearch3.ClientID + "').hide();if ($('#" + hfAndOr3.ClientID + "').val()==''){$('#" + hfAndOr3.ClientID + "').val(document.getElementById('" + lnkAndOr3.ClientID + "').text)};return false;");//return false;

        lnkMinusSearch1.Attributes.Add("onclick", "$('#" + trSearch1.ClientID + "').hide();$('#" + lnkAddSearch1.ClientID + "').show();$('#" + hfAndOr1.ClientID + "').val('');return false;");
        lnkMinusSearch2.Attributes.Add("onclick", "$('#" + trSearch2.ClientID + "').hide();$('#" + lnkAddSearch2.ClientID + "').show();$('#" + hfAndOr2.ClientID + "').val('');return false;");
        lnkMinusSearch3.Attributes.Add("onclick", "$('#" + trSearch3.ClientID + "').hide();$('#" + lnkAddSearch3.ClientID + "').show();$('#" + hfAndOr3.ClientID + "').val('');return false;");


        GridViewRow ViewItemPager = grdViewItem.TopPagerRow;
        if (ViewItemPager != null)
            _ViewItemPager = (Common_Pager)ViewItemPager.FindControl("ViewItemPager");


        cbcSearch1.TableID = int.Parse(_qsTableID);
        cbcSearch2.TableID = int.Parse(_qsTableID);
        cbcSearch3.TableID = int.Parse(_qsTableID);
        cbcSearchMain.TableID = int.Parse(_qsTableID);

        if (!IsPostBack)
        {
            PopulateViewUserDropDown();

            if (_theView == null)
            {
                //PopulateView(null); // not going to happen
            }
            else
            {
                PopulateAView((int)_theView.ViewID);

            }
        }

        if ((Request.Params["__EVENTTARGET"] != null) && (Request.Params["__EVENTTARGET"].ToString().IndexOf("lnkNavigateNext") > -1
               || Request.Params["__EVENTTARGET"].ToString().IndexOf("lnkNavigatePrev") > -1))
        {

        }
        else
        {
            CallFilterShowHide();
        }

    }



    protected void lnkShowView_Click(object sender, EventArgs e)
    {

        if (hfCurrentViewID.Value != "")
        {
            Session[_strViewSession ] = hfCurrentViewID.Value;
                        
            Session["SCid" + hfCurrentViewID.Value] = -1;
            if(Request.RawUrl.IndexOf("TableDetail.aspx")>-1)
            {
                Response.Redirect("~/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(_qsTableID), false);
            }
            else if (Request.QueryString["noajax"] != null)
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if(Request.QueryString["tabindex"]!=null)
                {
                    Session["viewtabindex"] = Request.QueryString["tabindex"].ToString();
                }
                else
                {
                    Session["viewtabindex"] = null;
                }
                ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "ReloadRecordListPage", "parent.$.fancybox.close();", true);
            }            
        }
    }

    protected void lnlDeleteView_Click(object sender, EventArgs e)
    {
        if (hfCurrentViewID.Value != "")
        {
            Common.ExecuteText("DELETE ViewItem WHERE ViewID=" + hfCurrentViewID.Value);
            Common.ExecuteText("DELETE [View] WHERE ViewID=" + hfCurrentViewID.Value);
            hfCurrentViewRowIndex.Value = "";
            hfCurrentViewID.Value = "";
            lnkNavigateNext_Click(null, null);
        }
    }
    protected void lnkAddView_Click(object sender, EventArgs e)
    {
        //Mohsin

        //lblViewTitle.Text = "Edit View";

        ddlViewUser.Enabled = true;
        hlShowThisView.Visible = false;
        PopulateAView(-1);
    }


    protected void ViewItemPager_DeleteAction(object sender, EventArgs e)
    {


        string sCheck = "";

        for (int i = 0; i < grdViewItem.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)grdViewItem.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)grdViewItem.Rows[i].FindControl("LblID")).Text + ",";
            }
        }





        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message_alert", "alert('Please select a record.');", true);

            return;
        }

        sCheck = sCheck + "-1";

        Common.ExecuteText("DELETE ViewItem WHERE ViewItemID IN (" + sCheck + ") ");

        PopulateViewItem((int)_theView.ViewID);



    }



  

    protected void PopulateViewSortColumnDDL(int iViewID)
    {
        string sSQL;

        string strOldValue = "";

        if (ddlViewSortOrder.SelectedItem != null && ddlViewSortOrder.SelectedValue!="")
        {
            strOldValue = ddlViewSortOrder.SelectedValue;
        }



        ddlViewSortOrder.Items.Clear();
        //oliver: Ticket 1298
        if (_theView.ViewID.ToString() != "")
        {
            sSQL = "SELECT c.ColumnID, c.DisplayName ";
            sSQL += "FROM [Column] c ";
            sSQL += "WHERE c.ColumnId IN (SELECT COALESCE(ColumnId, 0) FROM ViewItem WHERE ViewID = " + Convert.ToString(iViewID) + ") ";
            sSQL += "AND c.TableID = " + _qsTableID;
            ddlViewSortOrder.DataSource = Common.DataTableFromText(sSQL);
        }

        //ddlViewSortOrder.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE 
        //    TableID=" + _qsTableID); // IsStandard=0 AND DisplayTextSummary IS NOT NULL AND LEN(DisplayTextSummary) > 0 AND

        ddlViewSortOrder.DataBind();
        ListItem liSeletec = new ListItem("-- Please Select --", "");

        ddlViewSortOrder.Items.Insert(0, liSeletec);
        if(strOldValue!="")
        {
            if (ddlViewSortOrder.Items.FindByValue(strOldValue) != null)
                ddlViewSortOrder.SelectedValue = strOldValue;
        }

    }

    protected void PopulateViewUserDropDown()
    {

        ddlViewUser.Items.Clear();



        //[User].FirstName + ' ' + [User].LastName
        ddlViewUser.DataSource = Common.DataTableFromText(@"SELECT    ([User].Email) As UserName, [User].Userid
	                    FROM [User] INNER JOIN UserRole ON [User].UserID=UserRole.UserID WHERE [User].IsActive=1 and
	                    UserRole.AccountID=" + Session["AccountID"].ToString() + @" 
	                    ORDER BY [User].FirstName");

        ddlViewUser.DataBind();

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--All--", "");
        ddlViewUser.Items.Insert(0, liSelect);

        //if (!Common.HaveAccess(_strRecordRightID, "1,2"))
        //{
        //    ddlViewUser.SelectedValue = _ObjUser.UserID.ToString();
        //    ddlViewUser.Enabled = false;
        //}

    }

    protected void NavigateView(int? iViewRowIndex)
    {

        string strWhere = "";

        if(_iParentTableID==null)
        {
            strWhere =" AND ParentTableID IS NULL";
        }
        else
        {
            strWhere = " AND ParentTableID=" + _iParentTableID.ToString();
        }
       
        //if (!Common.HaveAccess(_strRecordRightID, "1,2"))
        //{
        //    strWhere =strWhere + " AND UserID="+_ObjUser.UserID.ToString();
        //}

        DataTable dtViews = Common.DataTableFromText("SELECT * FROM [View] WHERE ViewPageType='"+ ddlViewPageType.SelectedValue 
            +"' AND TableID= " + _qsTableID + strWhere + " ORDER BY ViewID");


        //if (!Common.HaveAccess(_strRecordRightID, "1,2") && dtViews.Rows.Count==0)
        //{
        //    strWhere = " AND UserID IS NULL";
        //    dtViews = Common.DataTableFromText("SELECT * FROM [View] WHERE TableID= " + _qsTableID + strWhere + " ORDER BY ViewID");
        //}

        if (dtViews.Rows.Count > 0)
        {
            //lblViewTitle.Text = "Edit View";

            if (iViewRowIndex == null)
            {
                //this is the first time
                hfCurrentViewID.Value = dtViews.Rows[0]["ViewID"].ToString();
                hfCurrentViewRowIndex.Value = "0";
                PopulateAView(int.Parse(hfCurrentViewID.Value));
            }
            else
            {
                if ((int)iViewRowIndex > (dtViews.Rows.Count - 1))
                {
                    iViewRowIndex = 0;
                }

                if ((int)iViewRowIndex < 0)
                {
                    iViewRowIndex = dtViews.Rows.Count - 1;
                }


                hfCurrentViewID.Value = dtViews.Rows[(int)iViewRowIndex]["ViewID"].ToString();
                hfCurrentViewRowIndex.Value = iViewRowIndex.ToString();
                PopulateAView(int.Parse(hfCurrentViewID.Value));



            }




        }
        else
        {
            //lblViewTitle.Text = "Edit View";
        }

    }


    public string GetViewFilterControlXML()
    {

        string strXML = "";
        //strXML = @"<root>" +
        //    " <" + ddlViewSortOrder.ID + ">" + HttpUtility.HtmlEncode(ddlViewSortOrder.SelectedValue) + "</" + ddlViewSortOrder.ID + ">" +
        //    " <" + rbSortOrderDirection.ID + ">"
        //    + HttpUtility.HtmlEncode(rbSortOrderDirection.SelectedIndex == null ? "" : rbSortOrderDirection.SelectedValue) + "</"
        //    + rbSortOrderDirection.ID + ">" +
        //        " <" + hfAndOr1.ID + ">" + HttpUtility.HtmlEncode(hfAndOr1.Value) + "</" + hfAndOr1.ID + ">" +
        //         " <" + hfAndOr2.ID + ">" + HttpUtility.HtmlEncode(hfAndOr2.Value) + "</" + hfAndOr2.ID + ">" +
        //          " <" + hfAndOr3.ID + ">" + HttpUtility.HtmlEncode(hfAndOr3.Value) + "</" + hfAndOr3.ID + ">" +
        //         " <" + cbcSearchMain.ID + ">" + HttpUtility.HtmlEncode(GetSearchCriteriaIDForCBC(cbcSearchMain)) + "</" + cbcSearchMain.ID + ">" +
        //           " <" + cbcSearch1.ID + ">" + HttpUtility.HtmlEncode(GetSearchCriteriaIDForCBC(cbcSearch1)) + "</" + cbcSearch1.ID + ">" +
        //             " <" + cbcSearch2.ID + ">" + HttpUtility.HtmlEncode(GetSearchCriteriaIDForCBC(cbcSearch2)) + "</" + cbcSearch2.ID + ">" +
        //               " <" + cbcSearch3.ID + ">" + HttpUtility.HtmlEncode(GetSearchCriteriaIDForCBC(cbcSearch3)) + "</" + cbcSearch3.ID + ">" +
        //       " <" + hfTextSearch.ID + ">" + HttpUtility.HtmlEncode(hfTextSearch.Value) + "</" + hfTextSearch.ID + ">" +
        //      "</root>";


        strXML = @"<root>" +
            " <" + ddlViewSortOrder.ID + ">" + HttpUtility.HtmlEncode(ddlViewSortOrder.SelectedValue) + "</" + ddlViewSortOrder.ID + ">" +
            " <" + rbSortOrderDirection.ID + ">"
            + HttpUtility.HtmlEncode(rbSortOrderDirection.SelectedIndex == null ? "" : rbSortOrderDirection.SelectedValue) + "</"
            + rbSortOrderDirection.ID + ">" +
                " <" + hfAndOr1.ID + ">" + HttpUtility.HtmlEncode(hfAndOr1.Value) + "</" + hfAndOr1.ID + ">" +
                 " <" + hfAndOr2.ID + ">" + HttpUtility.HtmlEncode(hfAndOr2.Value) + "</" + hfAndOr2.ID + ">" +
                  " <" + hfAndOr3.ID + ">" + HttpUtility.HtmlEncode(hfAndOr3.Value) + "</" + hfAndOr3.ID + ">" +
                 " <" + cbcSearchMain.ID + "_ddlYAxisV>" + HttpUtility.HtmlEncode(cbcSearchMain.ddlYAxisV) + "</" + cbcSearchMain.ID + "_ddlYAxisV>" +
                  " <" + cbcSearchMain.ID + "_TextValue>" + HttpUtility.HtmlEncode(cbcSearchMain.TextValue) + "</" + cbcSearchMain.ID + "_TextValue>" +
                  " <" + cbcSearchMain.ID + "_CompareOperator>" + HttpUtility.HtmlEncode(cbcSearchMain.CompareOperator) + "</" + cbcSearchMain.ID + "_CompareOperator>" +
                  " <" + cbcSearch1.ID + "_ddlYAxisV>" + HttpUtility.HtmlEncode(hfAndOr1.Value==""?"": cbcSearch1.ddlYAxisV) + "</" + cbcSearch1.ID + "_ddlYAxisV>" +
                    " <" + cbcSearch1.ID + "_TextValue>" + HttpUtility.HtmlEncode(hfAndOr1.Value == "" ? "" : cbcSearch1.TextValue) + "</" + cbcSearch1.ID + "_TextValue>" +
                    " <" + cbcSearch1.ID + "_CompareOperator>" + HttpUtility.HtmlEncode(hfAndOr1.Value == "" ? "" : cbcSearch1.CompareOperator) + "</" + cbcSearch1.ID + "_CompareOperator>" +
                     " <" + cbcSearch2.ID + "_ddlYAxisV>" + HttpUtility.HtmlEncode(hfAndOr2.Value == "" ? "" : cbcSearch2.ddlYAxisV) + "</" + cbcSearch2.ID + "_ddlYAxisV>" +
                     " <" + cbcSearch2.ID + "_TextValue>" + HttpUtility.HtmlEncode(hfAndOr2.Value == "" ? "" : cbcSearch2.TextValue) + "</" + cbcSearch2.ID + "_TextValue>" +
                     " <" + cbcSearch2.ID + "_CompareOperator>" + HttpUtility.HtmlEncode(hfAndOr2.Value == "" ? "" : cbcSearch2.CompareOperator) + "</" + cbcSearch2.ID + "_CompareOperator>" +
                       " <" + cbcSearch3.ID + "_ddlYAxisV>" + HttpUtility.HtmlEncode(hfAndOr3.Value == "" ? "" : cbcSearch3.ddlYAxisV) + "</" + cbcSearch3.ID + "_ddlYAxisV>" +
                        " <" + cbcSearch3.ID + "_TextValue>" + HttpUtility.HtmlEncode(hfAndOr3.Value == "" ? "" : cbcSearch3.TextValue) + "</" + cbcSearch3.ID + "_TextValue>" +
                        " <" + cbcSearch3.ID + "_CompareOperator>" + HttpUtility.HtmlEncode(hfAndOr3.Value == "" ? "" : cbcSearch3.CompareOperator) + "</" + cbcSearch3.ID + "_CompareOperator>" +
               " <" + hfTextSearch.ID + ">" + HttpUtility.HtmlEncode(hfTextSearch.Value) + "</" + hfTextSearch.ID + ">" +
              "</root>";

        return strXML;
    }



    protected void PopulateViewFilter()
    {

        string TextSearch = "";


        //lets check 4 search control



        //int iSearchCount = 0;
        string strTSA = "";
        string strTSB = "";
        string strTSC = "";
        string strTSD = "";



        string strAO1 = "";
        string strAO2 = "";
        string strAO3 = "";

        string strSearchMainTextSearch=cbcSearchMain.TextSearch;
        if (strSearchMainTextSearch != "" )
        {
            strTSA = "(" + strSearchMainTextSearch + ")";
        }

        //if (cbcSearchMain.NumericSearch != "" && cbcSearchMain.NumericSearch!=null)
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

        string strSearch1TextSearch=cbcSearch1.TextSearch;
        if (strSearch1TextSearch != ""  && hfAndOr1.Value != "")
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

        //if (cbcSearch1.NumericSearch != "" && cbcSearch1.NumericSearch!=null && hfAndOr1.Value != "")
        //{
        //    //TextSearch = TextSearch + " " + strAndOr1 + " " + "(" + cbcSearch1.TextSearch + ")";
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



        string strSearch2TextSearch=cbcSearch2.TextSearch;
        if (strSearch2TextSearch != ""  && hfAndOr2.Value != "")
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

        //if (cbcSearch2.NumericSearch != "" && cbcSearch2.NumericSearch!=null && hfAndOr2.Value != "")
        //{
        //    //TextSearch = TextSearch + " " + strAndOr2 + " " + "(" + cbcSearch2.TextSearch + ")";

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

        //if (cbcSearch2.NumericSearch != "" && hfAndOr2.Value != "")
        //{
        //    _strNumericSearch = _strNumericSearch + " " + strAndOr2 + " " + "(" + cbcSearch2.NumericSearch + ")";
        //}


        string strSearch3TextSearch = cbcSearch3.TextSearch;

        if (strSearch3TextSearch != "" && hfAndOr3.Value != "")
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



        //if (cbcSearch3.NumericSearch != "" && cbcSearch3.NumericSearch!=null && hfAndOr3.Value != "")
        //{
        //    //TextSearch = TextSearch + " " + strAndOr3 + " " + "(" + cbcSearch3.TextSearch + ")";

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

        hfTextSearch.Value = TextSearch;
        


    }


    //protected int GetSearchCriteriaIDForCBC(Pages_UserControl_ControlByColumn cbcX)
    //{
    //    if (cbcX.ddlYAxisV != "")
    //    {
    //        string xml = null;
    //        xml = @"<root>" +
    //               " <ddlYAxisV>" + HttpUtility.HtmlEncode(cbcX.ddlYAxisV) + "</ddlYAxisV>" +
    //               " <txtUpperLimitV>" + HttpUtility.HtmlEncode(cbcX.txtUpperLimitV) + "</txtUpperLimitV>" +
    //               " <txtLowerLimitV>" + HttpUtility.HtmlEncode(cbcX.txtLowerLimitV) + "</txtLowerLimitV>" +
    //               " <hfTextSearchV>" + HttpUtility.HtmlEncode(cbcX.hfTextSearchV) + "</hfTextSearchV>" +
    //               " <txtLowerDateV>" + HttpUtility.HtmlEncode(cbcX.txtLowerDateV) + "</txtLowerDateV>" +
    //               " <txtUpperDateV>" + HttpUtility.HtmlEncode(cbcX.txtUpperDateV) + "</txtUpperDateV>" +
    //               " <ddlDropdownColumnSearchV>" + HttpUtility.HtmlEncode(cbcX.ddlDropdownColumnSearchV) + "</ddlDropdownColumnSearchV>" +
    //               " <txtSearchTextV>" + HttpUtility.HtmlEncode(cbcX.txtSearchTextV) + "</txtSearchTextV>" +
    //              "</root>";

    //        //SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
    //        //return SystemData.SearchCriteria_Insert(theSearchCriteria);

    //        XMLData theXMLData = new XMLData(null, xml);
    //        return SystemData.dbg_XMLData_Insert(theXMLData,null,null);
    //    }

    //    return -1;
    //}


    protected void btnViewItemIDForColumnIndex_Click(object sender, EventArgs e)
    {
        //
        if (hfViewItemIDForColumnIndex.Value != "")
        {          

            try
            {
                string strNewVIT = hfViewItemIDForColumnIndex.Value.Substring(0, hfViewItemIDForColumnIndex.Value.Length - 1);
                string[] newVT = strNewVIT.Split(',');

                //string strFilter = "";



                DataTable dtDO = Common.DataTableFromText("SELECT ColumnIndex,ViewItemID FROM [ViewItem] WHERE ViewItemID IN (" + strNewVIT + ") ORDER BY ColumnIndex");
                if (newVT.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newVT.Length; i++)
                    {
                        Common.ExecuteText("UPDATE ViewItem SET ColumnIndex =" + i.ToString() + " WHERE ViewItemID=" + newVT[i]);

                    }
                }


            }
            catch (Exception ex)
            {

              //

            }
            PopulateViewItem(int.Parse(hfCurrentViewID.Value));
        }
    }


    protected void lnkResetDefault_Click(object sender, EventArgs e)
    {
        //ViewState["Reset"] = "Yes";
        //lnkSaveView_Click(null, null);
       
    }
    protected void lnkSaveView_Click(object sender, EventArgs e)
    {

        //if (ViewState["Reset"] !=null && ViewState["ResetViewConfirm"] == null)
        //{
        //    //trResetViews.Visible = true;
        //    ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "jsShowDefaultCofrimForView", "OpenDefaultConfirm();", true);
        //    return;

        //}      

        PopulateViewFilter();
        
        if (hfCurrentViewID.Value == "")
        {
           

            View theView = new View();
            theView.TableID = int.Parse(_qsTableID);
            theView.ViewName = txtViewName.Text;
            theView.UserID = ddlViewUser.SelectedValue == "" ? null : (int?)int.Parse(ddlViewUser.SelectedValue);
            theView.RowsPerPage = txtRowsPerPage.Text == "" ? null : (int?)int.Parse(txtRowsPerPage.Text);
            theView.ViewPageType = ddlViewPageType.SelectedValue;           
            theView.SortOrder = GetViewOrderString();// ddlViewSortOrder.SelectedValue + " " + strSortDirection;


            theView.ShowViewIcon = chkViewIcon.Checked;
            theView.ShowEditIcon = chkViewEditIcon.Checked;
            theView.ShowDeleteIcon = chkViewDeleteIcon.Checked;
            theView.ShowBulkUpdateIcon = chkViewBulkEditIcon.Checked;
            theView.ShowFixedHeader = chkShowFixedHeader.Checked;
            theView.ShowAddIcon = chkViewAddIcon.Checked;
            theView.Filter = hfTextSearch.Value;
            theView.ShowSearchFields = chkShowSearchFields.Checked;
            theView.FilterControlsInfo = GetViewFilterControlXML();
            theView.ParentTableID = _iParentTableID;
            int iNewViewID = ViewManager.dbg_View_Insert(theView);
            hfCurrentViewID.Value = iNewViewID.ToString();


            theView = ViewManager.dbg_View_Detail(int.Parse(hfCurrentViewID.Value));
            txtViewName.Text = theView.ViewName;

            PopulateAView(iNewViewID);
            //PopulateViewItem(int.Parse(hfCurrentViewID.Value));
            return;
            //lblViewTitle.Text = "Edit View";
        }
        else
        {
            View theView = ViewManager.dbg_View_Detail(int.Parse(hfCurrentViewID.Value));
            if (theView != null)
            {
                try
                {
                    theView.ViewName = txtViewName.Text;
                    theView.UserID = ddlViewUser.SelectedValue == "" ? null : (int?)int.Parse(ddlViewUser.SelectedValue);
                    theView.RowsPerPage = txtRowsPerPage.Text == "" ? null : (int?)int.Parse(txtRowsPerPage.Text);
                    theView.ViewPageType = ddlViewPageType.SelectedValue;
                    theView.SortOrder = GetViewOrderString();// ddlViewSortOrder.SelectedValue + " " + strSortDirection;
                    theView.ShowViewIcon = chkViewIcon.Checked;
                    theView.ShowEditIcon = chkViewEditIcon.Checked;
                    theView.ShowDeleteIcon = chkViewDeleteIcon.Checked;
                    theView.ShowBulkUpdateIcon = chkViewBulkEditIcon.Checked;
                    theView.ShowFixedHeader = chkShowFixedHeader.Checked;
                    theView.ShowAddIcon = chkViewAddIcon.Checked;
                    theView.ShowSearchFields = chkShowSearchFields.Checked;

                    theView.Filter = hfTextSearch.Value;

                    theView.FilterControlsInfo = GetViewFilterControlXML();
                    theView.ParentTableID = _iParentTableID;
                    ViewManager.dbg_View_Update(theView);

                    theView = ViewManager.dbg_View_Detail((int)theView.ViewID);
                    txtViewName.Text = theView.ViewName;
                }
                catch
                {
                    //
                }

                //now let's update ViewItem
                for (int i = 0; i < grdViewItem.Rows.Count; i++)
                {
                    string strViewItemID = ((Label)grdViewItem.Rows[i].FindControl("LblID")).Text;
                    ViewItem theViewItem = ViewManager.dbg_ViewItem_Detail(int.Parse(strViewItemID));
                    try
                    {
                        if (theViewItem != null)
                        {
                            theViewItem.ColumnIndex = i;
                            //theViewItem.Heading = ((TextBox)grdViewItem.Rows[i].FindControl("txtHeading")).Text;
                            theViewItem.SearchField = ((CheckBox)grdViewItem.Rows[i].FindControl("chkSearchField")).Checked;
                            //theViewItem.FilterField = ((CheckBox)grdViewItem.Rows[i].FindControl("chkFilterField")).Checked;
                            theViewItem.Alignment = ((DropDownList)grdViewItem.Rows[i].FindControl("ddlAlignment")).SelectedValue;
                            TextBox txtWidth = (TextBox)grdViewItem.Rows[i].FindControl("txtWidth");
                            if (txtWidth.Text == "")
                            {
                                theViewItem.Width = null;
                            }
                            else
                            {
                                theViewItem.Width = int.Parse(txtWidth.Text);
                            }

                            theViewItem.ShowTotal = ((CheckBox)grdViewItem.Rows[i].FindControl("chkShowTotal")).Checked;

                            ViewManager.dbg_ViewItem_Update(theViewItem);
                        }
                    }
                    catch
                    {
                        //
                    }
                }

                //let's update sort order again
                if (theView.SortOrder != "")
                {
                    theView.ParentTableID = _iParentTableID;
                    theView.SortOrder = GetViewOrderString();
                    ViewManager.dbg_View_Update(theView);
                    theView = ViewManager.dbg_View_Detail((int)theView.ViewID);
                    txtViewName.Text = theView.ViewName;
                }
            }
        }

        //if (ViewState["Reset"]!=null)
        //{
        //    if (ViewState["ResetViewConfirm"] != null && ViewState["ResetViewConfirm"].ToString() == "ok")
        //    {
        //        //ViewManager.dbg_ResetViews(int.Parse(hfCurrentViewID.Value));
        //        int? iNewViewID = ViewManager.dbg_View_Reset(int.Parse(hfCurrentViewID.Value));
        //        hfCurrentViewID.Value = iNewViewID.ToString();
        //    }
        //    if (ViewState["ResetViewConfirm"] != null && ViewState["ResetViewConfirm"].ToString() != "ok")
        //    {
        //       //
        //          View theView = ViewManager.dbg_View_Detail(int.Parse(hfCurrentViewID.Value));
        //          if (theView != null)
        //          {
        //              theView.UserID = null;
        //              theView.ParentTableID = _iParentTableID;
        //              ViewManager.dbg_View_Update(theView);
        //              theView = ViewManager.dbg_View_Detail((int)theView.ViewID);
        //              txtViewName.Text = theView.ViewName;
        //          }
        //    }      
        //}


        //ViewManager.dbg_View_Default_ResetColumns((int)_theView.ViewID);
        //ViewState["Reset"] = null;
        //ViewState["ResetViewConfirm"] = null;
        //trResetViews.Visible = false;
        lnkShowView_Click(null, null);
    }

    protected void lnkNavigatePrev_Click(object sender, EventArgs e)
    {
        if (hfCurrentViewRowIndex.Value == "")
            hfCurrentViewRowIndex.Value = "0";

        PopulateAView(-1);
        NavigateView(int.Parse(hfCurrentViewRowIndex.Value) - 1);

        CallFilterShowHide();
    }


    protected void lnkOK_Click(object sender, EventArgs e)
    {
        //trResetViews.Visible = false;
        //ViewState["ResetViewConfirm"] = "ok";
        //lnkSaveView_Click(null, null);

        int? iNewViewID = ViewManager.dbg_View_Reset(int.Parse(hfCurrentViewID.Value));
        hfCurrentViewID.Value = iNewViewID.ToString();
        lnkShowView_Click(null, null);
    }

    protected void lnkNo_Click(object sender, EventArgs e)
    {
        //trResetViews.Visible = false;
        //ViewState["ResetViewConfirm"] = "no";
        //lnkSaveView_Click(null, null);
    }
    protected void lnkNavigateNext_Click(object sender, EventArgs e)
    {
        if (hfCurrentViewRowIndex.Value == "")
            hfCurrentViewRowIndex.Value = "0";
        PopulateAView(-1);

        NavigateView(int.Parse(hfCurrentViewRowIndex.Value) + 1);

        CallFilterShowHide();
    }


    protected void CallFilterShowHide()
    {

        string strJSSearchShowHide = "";

        if (hfAndOr1.Value != "")
        {
            strJSSearchShowHide = "$('#" + trSearch1.ClientID + "').show();$('#" + lnkAddSearch1.ClientID + "').hide();document.getElementById('" + lnkAndOr1.ClientID + "').text=$('#" + hfAndOr1.ClientID + "').val();";
        }

        if (hfAndOr2.Value != "")
        {
            strJSSearchShowHide = strJSSearchShowHide + "$('#" + trSearch2.ClientID + "').show();$('#" + lnkAddSearch2.ClientID + "').hide();document.getElementById('" + lnkAndOr2.ClientID + "').text=$('#" + hfAndOr2.ClientID + "').val();";
        }

        if (hfAndOr3.Value != "")
        {
            strJSSearchShowHide = strJSSearchShowHide + "$('#" + trSearch3.ClientID + "').show();$('#" + lnkAddSearch3.ClientID + "').hide();document.getElementById('" + lnkAndOr3.ClientID + "').text=$('#" + hfAndOr3.ClientID + "').val();";
        }


        if (strJSSearchShowHide != "")
            ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "PutDefaultSearcUI_PB", strJSSearchShowHide, true);
     }



    public string GetAddViewItemURL(int iViewID)
    {
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/ViewItemDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&ViewID=" + Cryptography.Encrypt(iViewID.ToString());
    }


    protected void PopulateViewItem(int iViewID)
    {
        int iTN = 0;

        hlAddViewItem.NavigateUrl = GetAddViewItemURL(iViewID);

        //grdViewItem.DataSource = ViewManager.dbg_ViewItem_Select(iViewID, null, "", null, null, "", null, null,
        //    "", "", "", null, null, ref iTN);


        DataTable dtViewItems = Common.DataTableFromText("SELECT * FROM ViewItem WHERE ViewID="+iViewID.ToString()+" ORDER BY ColumnIndex");
        PopulateViewSortColumnDDL(iViewID);

        grdViewItem.DataSource = dtViewItems;
        iTN = dtViewItems.Rows.Count;

        grdViewItem.VirtualItemCount = iTN;
        grdViewItem.DataBind();

        if (grdViewItem.TopPagerRow != null)
            grdViewItem.TopPagerRow.Visible = true;

        GridViewRow gvr = grdViewItem.TopPagerRow;



        if (gvr != null)
        {
            _ViewItemPager = (Common_Pager)gvr.FindControl("ViewItemPager");
            _ViewItemPager.TotalRows = iTN;
            _ViewItemPager.AddURL = GetAddViewItemURL(iViewID);
            _ViewItemPager.HyperAdd_CSS = "popuplinkVT";
            _ViewItemPager.AddToolTip = "Add/Remove";
        }

        if (iTN == 0)
        {
            divEmptyAddViewItem.Visible = true;
        }
        else
        {
            divEmptyAddViewItem.Visible = false;
        }

    }


    //protected void ViewItemPager_DeleteAction(object sender, EventArgs e)
    //{
    //    string sCheck = "";
    //    for (int i = 0; i < grdViewItem.Rows.Count; i++)
    //    {
    //        bool ischeck = ((CheckBox)grdViewItem.Rows[i].FindControl("chkDelete")).Checked;
    //        if (ischeck)
    //        {
    //            sCheck = sCheck + ((Label)grdViewItem.Rows[i].FindControl("LblID")).Text + ",";
    //      }
    //    }
    //    if (string.IsNullOrEmpty(sCheck))
    //    {
    //        ScriptManager.RegisterClientScriptBlock(grdViewItem, typeof(Page), "message_alert", "alert('Please select a record.');", true);
    //    }
    //    else
    //    {
    //        DeleteViewItem(sCheck);
    //        PopulateViewItem(int.Parse(hfCurrentViewID.Value), 0, grdViewItem.PageSize);

    //    }

    //}
    

    protected void cbcSearchMain_OnddlYAxis_Changed(object sender, EventArgs e)
    {
        if (cbcSearchMain.ddlYAxisV == "")
        {
            //lnkAddSearch1.Visible = false;// $('#" + lnkAddSearch1.ClientID + "').hide();
            ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "cbcSearchMain_OnddlYAxis_Changed1", "$('#" + lnkAddSearch1.ClientID + "').hide();", true);

        }
        else
        {
            //lnkAddSearch1.Visible = true; //$('#" + lnkAddSearch1.ClientID + "').show();
            ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "cbcSearchMain_OnddlYAxis_Changed2", "$('#" + lnkAddSearch1.ClientID + "').show();", true);
        }


        //if (IsPostBack)
        //    CallFilterShowHide();
    }
    protected void cbcSearch1_OnddlYAxis_Changed(object sender, EventArgs e)
    {
        if (cbcSearch1.ddlYAxisV == "")
        {
            //lnkAddSearch2.Visible = false;
            ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "cbcSearch1_OnddlYAxis_Changed1", "$('#" + lnkAddSearch2.ClientID + "').hide();", true);
        }
        else
        {
            //lnkAddSearch2.Visible = true;
            ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "cbcSearch1_OnddlYAxis_Changed2", "$('#" + lnkAddSearch2.ClientID + "').show();", true);
        }

        //if (IsPostBack)
        //    CallFilterShowHide();
    }

    protected void cbcSearch2_OnddlYAxis_Changed(object sender, EventArgs e)
    {
        if (cbcSearch2.ddlYAxisV == "")
        {
            //lnkAddSearch3.Visible = false;
            ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "cbcSearch2_OnddlYAxis_Changed1", "$('#" + lnkAddSearch3.ClientID + "').hide();", true);
        }
        else
        {
            //lnkAddSearch3.Visible = true;
            ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "cbcSearch2_OnddlYAxis_Changed2", "$('#" + lnkAddSearch3.ClientID + "').show();", true);
        }

        //if (IsPostBack)
        //    CallFilterShowHide();
    }
    protected void grdViewItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }


    //private void DeleteViewItem(string keys)
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(keys))
    //        {

    //            foreach (string sTemp in keys.Split(','))
    //            {
    //                if (!string.IsNullOrEmpty(sTemp))
    //                {
    //                    //SecurityManager.ets_Terminology_Delete(int.Parse(sTemp));

    //                    ViewManager.dbg_ViewItem_Delete(int.Parse(sTemp));
    //                }
    //            }


    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLog theErrorLog = new ErrorLog(null, "View Item Delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //        lblMsg.Text = ex.Message;

    //        //ScriptManager.RegisterClientScriptBlock(grdViewItem, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
    //    }
    //}



    //protected void PopulateSearchCriteriaCBCMain(int iSearchCriteriaID)
    //{
    //    try
    //    {
    //        //SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);

    //        XMLData theXMLData = SystemData.dbg_XMLData_Detail(iSearchCriteriaID, null, null);

    //        if (theXMLData != null)
    //        {

    //            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //            xmlDoc.Load(new StringReader(theXMLData.XMLText));

    //            cbcSearchMain.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;
    //            cbcSearchMain.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;
    //            cbcSearchMain.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;
    //            cbcSearchMain.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;
    //            cbcSearchMain.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;
    //            cbcSearchMain.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;
    //            cbcSearchMain.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;
    //            cbcSearchMain.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;


    //            //PopulateSearchParams();


    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message;
    //    }


    //}



    //protected void PopulateSearchCriteriaCBC(int iSearchCriteriaID, ref Pages_UserControl_ControlByColumn cbcSearch)
    //{
    //    try
    //    {
    //        //SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);

    //        XMLData theXMLData = SystemData.dbg_XMLData_Detail(iSearchCriteriaID,null,null);

    //        if (theXMLData != null)
    //        {

    //            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //            xmlDoc.Load(new StringReader(theXMLData.XMLText));
                
    //            if(xmlDoc.FirstChild["ddlYAxisV"]!=null)
    //                cbcSearch.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;

    //            if(xmlDoc.FirstChild["txtUpperLimitV"]!=null)
    //                cbcSearch.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;

    //            if (xmlDoc.FirstChild["txtLowerLimitV"] != null)
    //                cbcSearch.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;

    //            if (xmlDoc.FirstChild["hfTextSearchV"] != null)
    //                cbcSearch.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;

    //            if (xmlDoc.FirstChild["txtLowerDateV"] != null)
    //                cbcSearch.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;

    //            if (xmlDoc.FirstChild["txtUpperDateV"] != null)
    //                cbcSearch.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;

    //            if (xmlDoc.FirstChild["ddlDropdownColumnSearchV"] != null)
    //                cbcSearch.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;

    //            if (xmlDoc.FirstChild["txtSearchTextV"] != null)
    //                cbcSearch.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;

    //            if (xmlDoc.FirstChild["txtSearchTextVX"] != null)
    //                cbcSearch.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextVX"].InnerText;//test



    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message;
    //    }


    //}


    //protected void PopulateSearchCriteriaCBC1(int iSearchCriteriaID)
    //{
    //    try
    //    {
    //        //SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);

    //        XMLData theXMLData = SystemData.dbg_XMLData_Detail(iSearchCriteriaID, null, null);

    //        if (theXMLData != null)
    //        {

    //            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //            xmlDoc.Load(new StringReader(theXMLData.XMLText));

    //            if (xmlDoc.FirstChild["ddlYAxisV"] != null)
    //                cbcSearch1.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;

    //            if (xmlDoc.FirstChild["txtUpperLimitV"] != null)
    //                cbcSearch1.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;

    //            if (xmlDoc.FirstChild["txtLowerLimitV"] != null)
    //                cbcSearch1.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;

    //            if (xmlDoc.FirstChild["hfTextSearchV"] != null)
    //                cbcSearch1.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;

    //            if (xmlDoc.FirstChild["txtLowerDateV"] != null)
    //                cbcSearch1.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;

    //            if (xmlDoc.FirstChild["txtUpperDateV"] != null)
    //                cbcSearch1.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;

    //            if (xmlDoc.FirstChild["ddlDropdownColumnSearchV"] != null)
    //                cbcSearch1.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;

    //            if (xmlDoc.FirstChild["txtSearchTextV"] != null)
    //                cbcSearch1.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;

    //            if (xmlDoc.FirstChild["txtSearchTextVX"] != null)
    //                cbcSearch1.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextVX"].InnerText;//test



    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message;
    //    }


    //}

    //protected void PopulateSearchCriteriaCBC2(int iSearchCriteriaID)
    //{
    //    try
    //    {
    //        //SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);

    //        XMLData theXMLData = SystemData.dbg_XMLData_Detail(iSearchCriteriaID, null, null);
    //        if (theXMLData != null)
    //        {

    //            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //            xmlDoc.Load(new StringReader(theXMLData.XMLText));

    //            cbcSearch2.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;
    //            cbcSearch2.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;
    //            cbcSearch2.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;
    //            cbcSearch2.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;
    //            cbcSearch2.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;
    //            cbcSearch2.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;
    //            cbcSearch2.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;
    //            cbcSearch2.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;




    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message;
    //    }


    //}


    //protected void PopulateSearchCriteriaCBC3(int iSearchCriteriaID)
    //{
    //    try
    //    {
    //        //SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);
    //        XMLData theXMLData = SystemData.dbg_XMLData_Detail(iSearchCriteriaID, null, null);


    //        if (theXMLData != null)
    //        {

    //            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //            xmlDoc.Load(new StringReader(theXMLData.XMLText));

    //            cbcSearch3.ddlYAxisV = xmlDoc.FirstChild["ddlYAxisV"].InnerText;
    //            cbcSearch3.txtUpperLimitV = xmlDoc.FirstChild["txtUpperLimitV"].InnerText;
    //            cbcSearch3.txtLowerLimitV = xmlDoc.FirstChild["txtLowerLimitV"].InnerText;
    //            cbcSearch3.hfTextSearchV = xmlDoc.FirstChild["hfTextSearchV"].InnerText;
    //            cbcSearch3.txtLowerDateV = xmlDoc.FirstChild["txtLowerDateV"].InnerText;
    //            cbcSearch3.txtUpperDateV = xmlDoc.FirstChild["txtUpperDateV"].InnerText;
    //            cbcSearch3.ddlDropdownColumnSearchV = xmlDoc.FirstChild["ddlDropdownColumnSearchV"].InnerText;
    //            cbcSearch3.txtSearchTextV = xmlDoc.FirstChild["txtSearchTextV"].InnerText;

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message;
    //    }


    //}
    public void PopulateFilterControl(string strFilterControlsInfo, int iTableID)
    {
        if (strFilterControlsInfo != "")
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            xmlDoc.Load(new StringReader(strFilterControlsInfo));

            if (xmlDoc.FirstChild[ddlViewSortOrder.ID]!=null)
            {
                if (ddlViewSortOrder.Items.FindByValue(xmlDoc.FirstChild[ddlViewSortOrder.ID].InnerText)!=null)
                    ddlViewSortOrder.SelectedValue = xmlDoc.FirstChild[ddlViewSortOrder.ID].InnerText;
            }
               

            string strSortDirection = "";

            if (xmlDoc.FirstChild[rbSortOrderDirection.ID]!=null)
                strSortDirection= xmlDoc.FirstChild[rbSortOrderDirection.ID].InnerText;

            if (strSortDirection != "")
            {
                rbSortOrderDirection.SelectedValue = strSortDirection;
            }

            hfAndOr1.Value = xmlDoc.FirstChild[hfAndOr1.ID].InnerText;
            hfAndOr2.Value = xmlDoc.FirstChild[hfAndOr2.ID].InnerText;
            hfAndOr3.Value = xmlDoc.FirstChild[hfAndOr3.ID].InnerText;

            if (xmlDoc.FirstChild[cbcSearchMain.ID + "_ddlYAxisV"] != null &&
                xmlDoc.FirstChild[cbcSearchMain.ID + "_TextValue"] != null)
            {
                cbcSearchMain.TableID = iTableID;
                cbcSearchMain.ddlYAxisV = xmlDoc.FirstChild[cbcSearchMain.ID + "_ddlYAxisV"].InnerText;
                cbcSearchMain.TextValue = xmlDoc.FirstChild[cbcSearchMain.ID + "_TextValue"].InnerText;

                if (xmlDoc.FirstChild[cbcSearchMain.ID + "_CompareOperator"] != null)
                {
                    cbcSearchMain.CompareOperator = xmlDoc.FirstChild[cbcSearchMain.ID + "_CompareOperator"].InnerText;
                }
            }

            if (xmlDoc.FirstChild[cbcSearch1.ID + "_ddlYAxisV"] != null &&
                xmlDoc.FirstChild[cbcSearch1.ID + "_TextValue"] != null)
            {
                cbcSearch1.TableID = iTableID;
                cbcSearch1.ddlYAxisV = xmlDoc.FirstChild[cbcSearch1.ID + "_ddlYAxisV"].InnerText;
                cbcSearch1.TextValue = xmlDoc.FirstChild[cbcSearch1.ID + "_TextValue"].InnerText;
                if (xmlDoc.FirstChild[cbcSearch1.ID + "_CompareOperator"] != null)
                {
                    cbcSearch1.CompareOperator = xmlDoc.FirstChild[cbcSearch1.ID + "_CompareOperator"].InnerText;
                }
            }

            if (xmlDoc.FirstChild[cbcSearch2.ID + "_ddlYAxisV"] != null &&
                xmlDoc.FirstChild[cbcSearch2.ID + "_TextValue"] != null)
            {
                cbcSearch2.TableID = iTableID;
                cbcSearch2.ddlYAxisV = xmlDoc.FirstChild[cbcSearch2.ID + "_ddlYAxisV"].InnerText;
                cbcSearch2.TextValue = xmlDoc.FirstChild[cbcSearch2.ID + "_TextValue"].InnerText;
                if (xmlDoc.FirstChild[cbcSearch2.ID + "_CompareOperator"] != null)
                {
                    cbcSearch2.CompareOperator = xmlDoc.FirstChild[cbcSearch2.ID + "_CompareOperator"].InnerText;
                }
            }


            if (xmlDoc.FirstChild[cbcSearch3.ID + "_ddlYAxisV"] != null &&
                xmlDoc.FirstChild[cbcSearch3.ID + "_TextValue"] != null)
            {
                cbcSearch3.TableID = iTableID;
                cbcSearch3.ddlYAxisV = xmlDoc.FirstChild[cbcSearch3.ID + "_ddlYAxisV"].InnerText;
                cbcSearch3.TextValue = xmlDoc.FirstChild[cbcSearch3.ID + "_TextValue"].InnerText;
                if (xmlDoc.FirstChild[cbcSearch3.ID + "_CompareOperator"] != null)
                {
                    cbcSearch3.CompareOperator = xmlDoc.FirstChild[cbcSearch3.ID + "_CompareOperator"].InnerText;
                }
            }


            hfTextSearch.Value = xmlDoc.FirstChild[hfTextSearch.ID].InnerText;

        }
    }

    public string GetViewFilter()
    {
        PopulateViewFilter();
        return hfTextSearch.Value;
    }
    public string IsDynamic
    {
        get
        {
            return hfIsDanamic.Value;
        }
        set
        {
            hfIsDanamic.Value = value;
        }
    }

    protected void PopulateAView(int iViewID)
    {
        View theView = ViewManager.dbg_View_Detail(iViewID);


       
        if (theView != null)
        {
            ddlViewUser.Enabled = false;
            if (Session["roletype"].ToString() == "1")
            {
                hlShowThisView.Visible = true;
                hlShowThisView.Text = "Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(theView.TableID.ToString()) + "&View=" + theView.ViewName;
                hlShowThisView.NavigateUrl = "~/" + hlShowThisView.Text;
            }

            if ((theView.ViewPageType != "" && theView.ViewPageType.ToString().ToLower() == "dash") || _bOwnOrNull_View == false) //  
            {
                hlSaveDefault.Visible = false;
            }
            else
            {
                hlSaveDefault.Visible = true;
            }

            hfCurrentViewID.Value = theView.ViewID.ToString();

            txtViewName.Text = theView.ViewName;
            if (theView.UserID == null)
            {
                ddlViewUser.SelectedValue = "";
            }
            else
            {             
                if (ddlViewUser.Items.FindByValue(theView.UserID.ToString())!=null)
                    ddlViewUser.SelectedValue = theView.UserID.ToString();
            }


            if (theView.RowsPerPage == null)
            {
                txtRowsPerPage.Text = "";
            }
            else
            {
                txtRowsPerPage.Text = theView.RowsPerPage.ToString();
            }

            ddlViewPageType.SelectedValue = theView.ViewPageType.ToString();


           

            //if (theView.FilterControlsInfo != "")
            //{
            //    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            //    xmlDoc.Load(new StringReader(theView.FilterControlsInfo));

            //    ddlViewSortOrder.SelectedValue = xmlDoc.FirstChild[ddlViewSortOrder.ID].InnerText;

            //    string strSortDirection = xmlDoc.FirstChild[rbSortOrderDirection.ID].InnerText;

            //    if (strSortDirection != "")
            //    {
            //        rbSortOrderDirection.SelectedValue = strSortDirection;
            //    }

            //    hfAndOr1.Value = xmlDoc.FirstChild[hfAndOr1.ID].InnerText;
            //    hfAndOr2.Value = xmlDoc.FirstChild[hfAndOr2.ID].InnerText;
            //    hfAndOr3.Value = xmlDoc.FirstChild[hfAndOr3.ID].InnerText;

            //    if (xmlDoc.FirstChild[cbcSearchMain.ID + "_ddlYAxisV"] != null &&
            //        xmlDoc.FirstChild[cbcSearchMain.ID + "_TextValue"] != null)
            //    {
            //        cbcSearchMain.TableID = theView.TableID;
            //        cbcSearchMain.ddlYAxisV = xmlDoc.FirstChild[cbcSearchMain.ID + "_ddlYAxisV"].InnerText;
            //        cbcSearchMain.TextValue = xmlDoc.FirstChild[cbcSearchMain.ID + "_TextValue"].InnerText;
            //    }

            //    if (xmlDoc.FirstChild[cbcSearch1.ID + "_ddlYAxisV"] != null &&
            //        xmlDoc.FirstChild[cbcSearch1.ID + "_TextValue"] != null)
            //    {
            //        cbcSearch1.TableID = theView.TableID;
            //        cbcSearch1.ddlYAxisV = xmlDoc.FirstChild[cbcSearch1.ID + "_ddlYAxisV"].InnerText;
            //        cbcSearch1.TextValue = xmlDoc.FirstChild[cbcSearch1.ID + "_TextValue"].InnerText;
            //    }

            //    if (xmlDoc.FirstChild[cbcSearch2.ID + "_ddlYAxisV"] != null &&
            //        xmlDoc.FirstChild[cbcSearch2.ID + "_TextValue"] != null)
            //    {
            //        cbcSearch2.TableID = theView.TableID;
            //        cbcSearch2.ddlYAxisV = xmlDoc.FirstChild[cbcSearch2.ID + "_ddlYAxisV"].InnerText;
            //        cbcSearch2.TextValue = xmlDoc.FirstChild[cbcSearch2.ID + "_TextValue"].InnerText;
            //    }


            //    if (xmlDoc.FirstChild[cbcSearch3.ID + "_ddlYAxisV"] != null &&
            //        xmlDoc.FirstChild[cbcSearch3.ID + "_TextValue"] != null)
            //    {
            //        cbcSearch3.TableID = theView.TableID;
            //        cbcSearch3.ddlYAxisV = xmlDoc.FirstChild[cbcSearch3.ID + "_ddlYAxisV"].InnerText;
            //        cbcSearch3.TextValue = xmlDoc.FirstChild[cbcSearch3.ID + "_TextValue"].InnerText;
            //    }
                               

            //    hfTextSearch.Value = xmlDoc.FirstChild[hfTextSearch.ID].InnerText;

            //}

            if (theView.ShowAddIcon != null)
                chkViewAddIcon.Checked = (bool)theView.ShowAddIcon;

            if (theView.ShowBulkUpdateIcon != null)
                chkViewBulkEditIcon.Checked = (bool)theView.ShowBulkUpdateIcon;

            if (theView.ShowFixedHeader != null)
                chkShowFixedHeader.Checked = (bool)theView.ShowFixedHeader;

            if (theView.ShowDeleteIcon != null)
                chkViewDeleteIcon.Checked = (bool)theView.ShowDeleteIcon;
            if (theView.ShowViewIcon != null)
                chkViewIcon.Checked = (bool)theView.ShowViewIcon;
            if (theView.ShowEditIcon != null)
                chkViewEditIcon.Checked = (bool)theView.ShowEditIcon;

            if (theView.ShowSearchFields != null)
                chkShowSearchFields.Checked = (bool)theView.ShowSearchFields;


            //populate view item gird

            if (hfCurrentViewID.Value != "")
            {
                PopulateViewItem(int.Parse(hfCurrentViewID.Value));
            }

            //populate Filter control

            PopulateFilterControl(theView.FilterControlsInfo, (int)theView.TableID);
            if(!IsPostBack)
                Session["ViewItemID" + theView.ViewID.ToString()] = Common.Get_Comma_Sep_IDs("ViewItemID", "[ViewItem]", " ViewID=" + theView.ViewID.ToString() + " AND SearchField=1");
        }
        else
        {
            //go to the add mode

            txtViewName.Text = "";
            ddlViewUser.SelectedValue = "";
            txtRowsPerPage.Text = "";
            //ddlViewPageType.SelectedValue = "";
            if (ddlViewSortOrder.Items.FindByValue("")!=null)
                ddlViewSortOrder.SelectedValue = "";
            rbSortOrderDirection.Items[0].Selected = false;
            rbSortOrderDirection.Items[1].Selected = false;

            hfTextSearch.Value = "";

            cbcSearchMain.ddlYAxisV = "";
            cbcSearch1.ddlYAxisV = "";
            cbcSearch2.ddlYAxisV = "";
            cbcSearch3.ddlYAxisV = "";
            hfAndOr1.Value = "";
            hfAndOr2.Value = "";
            hfAndOr3.Value = "";

            chkViewAddIcon.Checked = true;
            chkViewBulkEditIcon.Checked = true;
            chkShowFixedHeader.Checked = false;
            chkViewDeleteIcon.Checked = true;
            chkViewIcon.Checked = true;
            chkViewEditIcon.Checked = true;
            chkShowSearchFields.Checked = true;

            hfCurrentViewID.Value = "";


            PopulateViewItem(-1);
            divEmptyAddViewItem.Visible = false;

            //string strJSSearchShowHide = "$('#" + trSearch1.ClientID + "').hide();$('#" + trSearch2.ClientID + "').hide();$('#" + trSearch3.ClientID + "').hide();";

            //ScriptManager.RegisterStartupScript(upViewItem, upViewItem.GetType(), "PutDefaultSearcUI_CS", strJSSearchShowHide, true);

        }



        //CallFilterShowHide();



    }



    protected void grdViewItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        //if (e.Row.RowType == DataControlRowType.Header)
        //{

        //    HyperLink hlAddDetail = e.Row.FindControl("hlAddDetail") as HyperLink;
        //    hlAddDetail.NavigateUrl = GetAddViewItemURL( int.Parse( hfCurrentViewID.Value));

        //}

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           


            Column theColumn = RecordManager.ets_Column_Details(int.Parse(DataBinder.Eval(e.Row.DataItem, "ColumnID").ToString()));
            if (theColumn != null)
            {
                Label lblFieldName = e.Row.FindControl("lblFieldName") as Label;
                if (lblFieldName != null)
                {
                    lblFieldName.Text = theColumn.DisplayName;

                }              


            }


            //TextBox txtHeading = e.Row.FindControl("txtHeading") as TextBox;
            //if (txtHeading != null)
            //{
            //    txtHeading.Text = DataBinder.Eval(e.Row.DataItem, "Heading").ToString();

            //}

            if (DataBinder.Eval(e.Row.DataItem, "Width") != null)
            {
                TextBox txtWidth = e.Row.FindControl("txtWidth") as TextBox;
                if (txtWidth != null)
                {
                    txtWidth.Text = DataBinder.Eval(e.Row.DataItem, "Width").ToString();

                }
            }

            if (DataBinder.Eval(e.Row.DataItem, "SearchField") != null)
            {
                if (DataBinder.Eval(e.Row.DataItem, "SearchField").ToString().ToLower() == "true")
                {
                    CheckBox chkSearchField = e.Row.FindControl("chkSearchField") as CheckBox;
                    if (chkSearchField != null)
                    {
                        chkSearchField.Checked=true;

                    }
                }
            }


            //if (DataBinder.Eval(e.Row.DataItem, "FilterField") != null)
            //{
            //    if (DataBinder.Eval(e.Row.DataItem, "FilterField").ToString().ToLower() == "true")
            //    {
            //        CheckBox chkFilterField = e.Row.FindControl("chkFilterField") as CheckBox;
            //        if (chkFilterField != null)
            //        {
            //            chkFilterField.Checked = true;

            //        }
            //    }
            //}

            if (DataBinder.Eval(e.Row.DataItem, "ShowTotal") != null)
            {
                if (DataBinder.Eval(e.Row.DataItem, "ShowTotal").ToString().ToLower() == "true")
                {
                    CheckBox chkShowTotal = e.Row.FindControl("chkShowTotal") as CheckBox;
                    if (chkShowTotal != null)
                    {
                        chkShowTotal.Checked = true;

                    }
                }
            }


           

            if (DataBinder.Eval(e.Row.DataItem, "Alignment") != null)
            {
                DropDownList ddlAlignment = e.Row.FindControl("ddlAlignment") as DropDownList;
                if (ddlAlignment != null)
                {
                    ddlAlignment.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Alignment").ToString().ToLower();

                }
            }

            


        }

    }

    protected void btnRefreshViewItem_Click(object sender, EventArgs e)
    {
        if (hfCurrentViewID.Value != "")
        {
            PopulateViewItem(int.Parse(hfCurrentViewID.Value));
        }
    }


    protected void ViewItemPager_BindTheGridAgain(object sender, EventArgs e)
    {
        PopulateViewItem(int.Parse(hfCurrentViewID.Value));
    }


    public string GetViewOrderString()
    {
        string strSortOrderString = "";

        if (ddlViewSortOrder.SelectedValue != "")
        {
            Column theSortColumn = RecordManager.ets_Column_Details(int.Parse(ddlViewSortOrder.SelectedValue));

            //string strViewItemID = Common.GetValueFromSQL("SELECT ViewItemID FROM ViewItem WHERE ViewID=" + hfCurrentViewID.Value + " AND ColumnID=" + ddlViewSortOrder.SelectedValue);


            //if (strViewItemID != "")
            //{
                //ViewItem theViewItem = ViewManager.dbg_ViewItem_Detail(int.Parse(strViewItemID), null, null);
            if (theSortColumn != null)
                {

                    string strHeading = theSortColumn.DisplayTextSummary == "" ? theSortColumn.DisplayName : theSortColumn.DisplayTextSummary;
                    if (strHeading != "")
                    {

                        if (theSortColumn.ColumnType == "number" || theSortColumn.ColumnType == "calculation")
                        {
                            strSortOrderString = " CONVERT(decimal(20,10), dbo.RemoveSpecialChars([" + strHeading + "]))";
                        }

                        if (theSortColumn.ColumnType == "date"
                               || theSortColumn.ColumnType == "datetime"
                               || theSortColumn.ColumnType == "time")
                        {
                            strSortOrderString = " CONVERT(Datetime, [dbo].[fnRemoveNonDate]([" + strHeading + "]),103) ";
                        }

                        if (strSortOrderString == "")
                        {
                            strSortOrderString = " " + strHeading + "";
                        }

                        if (strSortOrderString != "")
                        {
                            string strSortDirection = "";
                            if (rbSortOrderDirection.SelectedItem != null)
                                strSortDirection = rbSortOrderDirection.SelectedValue;

                            strSortOrderString = strSortOrderString + " " + strSortDirection;
                        }
                    }

                }

            //}



        }

        return strSortOrderString;
    }

}