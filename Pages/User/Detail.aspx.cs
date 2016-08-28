using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;
public partial class User_Detail : SecurePage
{

    bool _bShowTableGrid = false;
    string _strActionMode = "view";
    int? _iUserID=-1;
    UserRole _thisUserRole = null;
    //List<UserRole> _lstUserRole;
    string _qsMode = "";
    string _qsUserID = "";
    bool _bIsAccountHolder = false;

    Common_Pager _gvPager;

    int _iStartIndex = 0;
    int _iMaxRows = 10;
    Account _theAccount;
    User _CurrentUser = null;
    UserRole _CurrentUserRole = null;
    Role _CurrentRole = null;
    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {

        lblMsg.Text = "";

        try
        {
            int iTN = 0;


            gvTheGrid.DataSource = SecurityManager.ets_UserRoleAccount_Select(_iUserID,
                "", "",
                iStartIndex, iMaxRows, ref iTN);

            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.AddURL = GetAddURL();
                _gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);

                
            }


            if (iTN == 0)
            {
                divEmptyData.Visible = true;
                hplNewData.NavigateUrl = GetAddURL();

            }
            else
            {
                divEmptyData.Visible = false;

            }


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Linked Account", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected string GetAddURL()
    {
        return "javascript:OpenAddAccount();";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;
        // checking action mode
        _CurrentUser = (User)Session["User"];
        _CurrentUserRole = (UserRole)Session["UserRole"];
        _CurrentRole = SecurityManager.Role_Details((int)_CurrentUserRole.RoleID);
        if (Request.QueryString["mode"] == null)
        {
            Response.Redirect("~/Default.aspx",false);
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


                if (Request.QueryString["userid"] != null)
                {
                    _qsUserID = Cryptography.Decrypt(Request.QueryString["userid"]);
                    _iUserID = int.Parse(_qsUserID);
                    _thisUserRole = SecurityManager.GetUserRole((int)_iUserID, int.Parse(Session["AccountID"].ToString()));
                    if (_thisUserRole.IsPrimaryAccount != null && (bool)_thisUserRole.IsPrimaryAccount)
                    {
                        //
                    }
                    else
                    {
                        txtEmail.Enabled = false;
                        txtFirstName.Enabled = false;
                        txtLastName.Enabled = false;
                        txtPassword.Enabled = false;
                        txtPhoneNumber.Enabled = false;
                        TabContainer1.Tabs[2].Visible = false;
                        int? iPA = SecurityManager.GetPrimaryAccountID((int)_iUserID);

                        if(iPA!=null)
                        {
                            Account thePA = SecurityManager.Account_Details((int)iPA);
                          if(thePA!=null)
                          {
                              lblPrimaryAccount.Text = thePA.AccountName;
                              trPrimaryAccount.Visible = true;
                          }
                        }
                        
                    }
                }
                if (_strActionMode == "add")
                {
                    //trAccountHolder.Visible = false;
                    //lnkResetDashBoard.Visible = false;
                    divDelete.Visible = false;
                    divUnDelete.Visible = false;
                    //trActive.Visible = false;
                    if (!IsPostBack)
                    {
                        divUserTable.Visible = false;
                        trViewAllTable.Visible = true;
                    }
                       
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
        //Title = "User - " + _strActionMode;
        //lblTitle.Text = "User - " + _strActionMode;

        string strTitle = "";
        // checking permission

        string Password = txtPassword.Text;
        txtPassword.Attributes.Add("value", Password);



        //

        _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            hlRoleGroupNew.NavigateUrl = "~/Pages/Security/RoleGroupDetail.aspx?mode=" + Cryptography.Encrypt("add");
          

            if (_theAccount != null)
            {
                if (_theAccount.UseDataScope != null)
                {
                    if ((bool)_theAccount.UseDataScope)
                    {
                        trDataScopeTable.Visible = true;
                        trDataScopeValue.Visible = true;

                        ddlScopeTable.DataSource = Common.DataTableFromText("SELECT TableID,TableName FROM [Table] WHERE IsActive=1 AND AccountID=" + _theAccount.AccountID.ToString());
                        ddlScopeTable.DataBind();

                        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
                        ddlScopeTable.Items.Insert(0, liSelect);

                        System.Web.UI.WebControls.ListItem liSelect2 = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
                        ddlScopeField.Items.Insert(0, liSelect2);

                        System.Web.UI.WebControls.ListItem liSelect3 = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
                        ddlScopeValue.Items.Insert(0, liSelect3);
                      
                    }
                }
            }



            //PopulateRoleGroup();
            PopulateRole();
            PopulateTerminology();
            PopulateRole_ViewAndDash();
            //lblTitle.Text = Title;
            //PopulateAccountDDL();
            //PopulateRolesGrid();



        }


        //

        string strFancy = @" $(function () {
                                $("".popuplink"").fancybox({
                                    scrolling: 'auto',
                                    type: 'iframe',
                                    width: 900,
                                    height: 350,
                                    titleShow: false
                                });
                            });
                        
                              $(function () {
                                    $("".popuplink2"").fancybox({
                                        scrolling: 'auto',
                                        type: 'iframe',
                                        width: 500,
                                        height: 300,
                                        titleShow: false
                                    });
                                });

                                $(function () {
                                    $("".rolepopuplink"").fancybox({
                                        scrolling: 'auto',
                                        type: 'iframe',
                                        width: 550,
                                        height: 250,
                                        titleShow: false
                                    });
                                });

                                 $(function () {
                           $('.popupaddlinkuser').fancybox({
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

        ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);


        if (Request.QueryString["SearchCriteria"] != null)
        {

            hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/List.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
        }
        else
        {

            hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/List.aspx";
        }


        switch (_strActionMode.ToLower())
        {
            case "add":
                hfMode.Value = "add";
                strTitle = "Add User";
                Title = strTitle;
                lblTitle.Text = strTitle;

                if (!IsPostBack)
                {
                 
                    //TabContainer1.Tabs[1].Visible = false;
                    //TabContainer1.Tabs[2].Visible = false;
                    //TabContainer1.Tabs[3].Visible = false;
                    chkNotifyUser.Checked = true;
                    lnkRoleGroupSave.Visible = false;
                    lnkRoleGroupDelete.Visible = false;
                }

                break;

            case "view":
                hfMode.Value = "view";
                strTitle = "View User";
                Title = strTitle;
                lblTitle.Text = strTitle;
                //_lstUserRole = SecurityManager.UserRole_Select(null, _iUserID, null, null, null,
                //    "UserRoleID", "ASC", null, null, ref iTemp, int.Parse(Session["AccountID"].ToString()));

                if (!IsPostBack)
                {
                    //PopulateTableDropDown();
                    PopulateTheRecord();
                }

                EnableTheRecordControls(false);
                //cmdSave.Visible = false;
                divSave.Visible = false;

                break;

            case "edit":
                hfMode.Value = "edit";
                strTitle = "Edit User";
                Title = strTitle;
                lblTitle.Text = strTitle;
                //_lstUserRole = SecurityManager.UserRole_Select(null, _iUserID, null, null, null,
                //    "UserRoleID", "ASC", null, null, ref iTemp, int.Parse(Session["AccountID"].ToString()));

                if (!IsPostBack)
                {
                    //PopulateTableDropDown();
                    PopulateTheRecord();

                    //if (Request.QueryString["fromadd"] != null)
                    //{

                    //    lblMsg.Text = "Successfully Saved.";
                    //    lblMsg.ForeColor = System.Drawing.Color.Green;
                    //    TabContainer1.ActiveTab = TabContainer1.Tabs[1];
                    //}
                }
                break;


            default:
                //?

                break;
        }

        GridViewRow gvr = gvTheGrid.TopPagerRow;
        if (gvr != null)
            _gvPager = (Common_Pager)gvr.FindControl("Pager");

        if (!IsPostBack)
        {
            if (SystemData.SystemOption_ValueByKey_Account("HideUserTabs",null,null).ToLower() == "yes")
            {
                TabContainer1.Tabs[1].Visible = false;
                TabContainer1.Tabs[2].Visible = false;
                //TabContainer1.Tabs[3].Visible = false;
                //chkAdvancedSecurity.Visible = false;
                divUserTable.Visible = false;
                trViewAllTable.Visible = true;
                divBasicRoles.Visible = true;
            }
            //if(_thisUserRole!=null)  //if(chkAdvancedSecurity.Checked==false)
            //{
            //    if ((bool)_thisUserRole.IsAdvancedSecurity)
            //        divUserTable.Visible = false;
            //}
            if(ddlBasicRoles.SelectedValue!="")
            {
                Role theRole = SecurityManager.Role_Details(int.Parse(ddlBasicRoles.SelectedValue));
                if((bool)theRole.IsSystemRole)
                {
                    divUserTable.Visible = false;
                    trViewAllTable.Visible = true;
                }
            }
           


        }


    }


    protected void Pager_DeleteAction(object sender, EventArgs e)
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
        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
        }
        else
        {
            DeleteItem(sCheck);
            BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
            _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
            if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
            {
                BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
            }
        }

    }



    private void DeleteItem(string keys)
    {
        try
        {
            if (!string.IsNullOrEmpty(keys))
            {

                foreach (string sTemp in keys.Split(','))
                {
                    if (!string.IsNullOrEmpty(sTemp))
                    {

                        //GraphManager.ets_GraphOption_Delete(int.Parse(sTemp));

                        UserRole theUserRole = SecurityManager.UserRole_Details(Convert.ToInt32(sTemp));

                        if (theUserRole != null)
                        {

                            Common.ExecuteText(@"DELETE TableUser WHERE TableUserID IN 
                                (SELECT TableUserID FROM TableUser INNER JOIN [Table] 
                                ON TableUser.TableID=[Table] .TableID
                                WHERE UserID=" + _iUserID.ToString() + " AND AccountID=" + Session["AccountID"].ToString() + " )");

                         

                            Common.ExecuteText(@"DELETE MonitorScheduleUser WHERE MonitorScheduleUserID IN 
                                (SELECT MonitorScheduleUserID FROM MonitorScheduleUser INNER JOIN MonitorSchedule 
                                ON MonitorScheduleUser.MonitorScheduleID=MonitorSchedule.MonitorScheduleID
                                WHERE UserID=" + theUserRole.UserID.ToString() + " AND AccountID=" + theUserRole.AccountID.ToString() + " )");

                        }

                        SecurityManager.UserRole_Delete(Convert.ToInt32(sTemp));



                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Linked user delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }


    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Linked_Accounts.csv");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);


        int iColCount = gvTheGrid.Columns.Count;
        for (int i = 0; i < iColCount; i++)
        {
            if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
            {
            }
            else
            {
                sw.Write(gvTheGrid.Columns[i].HeaderText);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
        }

        sw.Write(sw.NewLine);

        // Now write all the rows.
        foreach (GridViewRow dr in gvTheGrid.Rows)
        {

            for (int i = 0; i < iColCount; i++)
            {
                if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
                {
                }
                else
                {
                    switch (i)
                    {

                        case 2:
                            Label lblAccountName = (Label)dr.FindControl("lblAccountName");
                            sw.Write("\"" + lblAccountName.Text + "\"");
                            break;

                        case 3:
                            Label lblRole = (Label)dr.FindControl("lblRole");
                            sw.Write("\"" + lblRole.Text + "\"");
                            break;
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

    protected void PopulateTerminology()
    {
        gvUserTable.Columns[1].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table");
        //grdTableUser.Columns[2].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table");
        //stgAddTable.InnerText ="Add " +  SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table");

    }
    protected DataTable  ViewUserDT()
    {
        return Common.DataTableFromText(@"SELECT U.UserID, U.Email FROM [User] U INNER JOIN UserRole UR ON U.UserID=UR.UserID
            INNER JOIN [Role] R ON UR.RoleID=R.RoleID
            WHERE UR.IsPrimaryAccount=1 AND U.IsActive=1  AND UR.AccountID=" + Session["AccountID"].ToString());
    }
    protected void PopulateRole_ViewAndDash()
    {

//        DataTable dtDashboard = Common.DataTableFromText(@"SELECT D.DocumentID,(D.DocumentText + ' (' + U.Email + ')') as DocumentText
//        FROM Document D
//        JOIN [User] U ON U.UserID = D.UserID
//        WHERE D.ForDashBoard=1 AND D.UserID IS NOT NULL AND D.AccountID=" + Session["AccountID"].ToString());

        DataTable dtDashboard = ViewUserDT();

        if (dtDashboard.Rows.Count > 1)
        {
            //trDashboard.Visible = true;
            ddlDashboard.DataSource = dtDashboard;
            ddlDashboard.DataBind();
           
        }
        ListItem liSelect = new ListItem("--Please Select--", "");
        ddlDashboard.Items.Insert(0, liSelect);


        DataTable dtRole_Views = ViewUserDT();

        if (dtRole_Views.Rows.Count > 1)
        {
            //trDashboard.Visible = true;
            ddlRole_ViewsDefaultFromUserID.DataSource = dtRole_Views;
            ddlRole_ViewsDefaultFromUserID.DataBind();

        }
        ListItem liSelect2 = new ListItem("--Please Select--", "");
        ddlRole_ViewsDefaultFromUserID.Items.Insert(0, liSelect2);

        //else
        //{
        //    trDashboard.Visible = false;
        //}


        //string strHideDashboard = SystemData.SystemOption_ValueByKey_Account("Hide User Dashboard Dropdown", int.Parse(Session["AccountID"].ToString()), null);
        //if(strHideDashboard!="" && strHideDashboard.ToLower()=="yes")
        //{
        //    trDashboard.Visible = false;
        //}



    }

    protected bool IsAdminChecked()
    {

        //for (int i = 0; i < grdRoles.Rows.Count; i++)
        //{
        //    bool ischeck = ((CheckBox)grdRoles.Rows[i].FindControl("chkAssigned")).Checked;
        //    if (ischeck)
        //    {
        //        Role oRole= SecurityManager.Role_Details(int.Parse(grdRoles.DataKeys[i].Value.ToString()));

        //        if (oRole.RoleType=="2")
        //        {
        //            return true;

        //        }
        //    }
        //}

        int iTN = 0;
        List<Role> lstRole = SecurityManager.Role_Select(null, "", ddlBasicRoles.SelectedValue, "", null, null, "", "",
            null, null, ref iTN, int.Parse(Session["AccountID"].ToString()), null, null);
        Role theRole = new Role(null, "", "", "", null, null);
        foreach (Role tempRole in lstRole)
        {
            theRole = tempRole;
        }

        if (theRole.RoleType == "2")
        {
            return true;

        }

        return false;

    }

    //protected void InsertUserBasicRole(int iUserID)
    //{
    //    try
    //    {
            


    //        int iTN = 0;
    //        List<Role> lstRole = SecurityManager.Role_Select(null, "", ddlBasicRoles.SelectedValue, "", null, null, "", "",
    //            null, null, ref iTN, int.Parse(Session["AccountID"].ToString()), null, null);
    //        Role theRole = new Role(null, "", "", "", null, null);
    //        foreach (Role tempRole in lstRole)
    //        {
    //            theRole = tempRole;
    //        }

    //        UserRole newUserRole = new UserRole(null, iUserID, (int)theRole.RoleID, DateTime.Now, DateTime.Now);
    //        newUserRole.AccountID = int.Parse(Session["AccountID"].ToString());
    //        newUserRole.IsPrimaryAccount = true;
    //        SecurityManager.UserRole_Insert(newUserRole);


    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLog theErrorLog = new ErrorLog(null, "User Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //        lblMsg.Text = ex.Message;
    //    }



    //}

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Linked Accounts";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

   
    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                 if (_CurrentUserRole.IsAccountHolder == null || (bool)_CurrentUserRole.IsAccountHolder == true)
                 {

                 }
                 else
                 {
                     Label lblRole = (Label)e.Row.FindControl("lblRole");
                     if (lblRole!=null && lblRole.Text == "Administrator")
                     {
                         CheckBox chkDelete = (CheckBox)e.Row.FindControl("chkDelete");
                         if(chkDelete!=null)
                         {
                             chkDelete.Visible = false;
                         }
                     }

                 }

                //Label lblRole = (Label)e.Row.FindControl("lblRole");
                //switch ((string)DataBinder.Eval(e.Row.DataItem, "RoleType"))
                //{
                //    case "2":
                //        lblRole.Text = "Administrator";
                //        break;
                //    case "3":
                //        lblRole.Text = "Edit Sample and Site Data";
                //        break;
                //    case "4":
                //        lblRole.Text = "Add and Edit Sample Data";
                //        break;
                //    case "5":
                //        lblRole.Text = "Read Only";
                //        break;
                //    case "6":
                //        lblRole.Text = "None";
                //        break;
                //    case "7":
                //        lblRole.Text = "Add Sample Data Only";
                //        break;
                //}

            }

        }
        catch
        {
            //
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void PopulateTheRecord()
    {
        try
        {

            hlAddAccount.NavigateUrl = "~/Pages/User/AddAccount.aspx?UserID=" + _iUserID.ToString();

            //PopulateLinkedUser();
            BindTheGrid(_iStartIndex, _iMaxRows);
            lblMsg.ForeColor = System.Drawing.Color.Red;


            User theUser = SecurityManager.User_Details((int)_iUserID);
            hfUserID.Value = _iUserID.ToString();

            txtFirstName.Text = theUser.FirstName;
            txtLastName.Text = theUser.LastName;
            txtPhoneNumber.Text = theUser.PhoneNumber;
            txtEmail.Text = theUser.Email;
         
            UserRole theUserRole = SecurityManager.GetUserRole((int)_iUserID, int.Parse(Session["AccountID"].ToString()));

            Role theRole = SecurityManager.Role_Details((int)theUserRole.RoleID);

            

            ddlBasicRoles.SelectedValue = theUserRole.RoleID.ToString();

            if (ddlBasicRoles.SelectedValue=="")
            {
                Response.Redirect(hlBack.NavigateUrl, false);
                return;
            }

            

            ddlBasicRoles_SelectedIndexChanged(null, null);

            //if (theUserRole.DashBoardDocumentID != null)
            //    ddlDashboard.SelectedValue = theUserRole.DashBoardDocumentID.ToString();


            if (theRole != null && theRole.RoleType == "2")
            {
                divUserRole.Visible = true;
                if(theUserRole.AllowDeleteTable!=null && (bool)theUserRole.AllowDeleteTable)
                {
                    chkAllowDeleteTable.Checked = true;
                }
                if (theUserRole.AllowDeleteColumn != null && (bool)theUserRole.AllowDeleteColumn)
                {
                    chkAllowDeleteColumn.Checked = true;
                }
                if (theUserRole.AllowDeleteRecord != null && (bool)theUserRole.AllowDeleteRecord)
                {
                    chkAllowDeleteRecord.Checked = true;
                }

                if (_CurrentUserRole.IsAccountHolder != null && (bool)_CurrentUserRole.IsAccountHolder)
                {
                    chkAllowDeleteTable.Enabled = true;
                    chkAllowDeleteColumn.Enabled = true;
                    chkAllowDeleteRecord.Enabled = true;
                }
                else
                {
                    chkAllowDeleteTable.Enabled = false;
                    chkAllowDeleteColumn.Enabled = false;
                    chkAllowDeleteRecord.Enabled = false;
                }

            }
            else
            {
                divUserRole.Visible = false;
                chkAllowDeleteTable.Checked = false;
                chkAllowDeleteColumn.Checked = false;
                chkAllowDeleteRecord.Checked = false;
            }

            if (theUserRole.IsDocSecurityAdvanced != null)
            {
                if ((bool)theUserRole.IsDocSecurityAdvanced)
                {
                    chkDocAdvancedSec.Checked = true;
                    divBasicDocSec.Visible = false;
                    divDocAdvancedSec.Visible = true;

                    BindUserFolderGrid();
                }
                else
                {
                    if (theUserRole.DocSecurityType != "")
                        ddlBasicDocSec.Text = theUserRole.DocSecurityType;
                }
            }


            if ((bool)theUserRole.IsAccountHolder)
            {
                divDelete.Visible = false;
               
                if (_strActionMode.ToLower() == "edit")
                {
                    lblTitle.Text = "Edit User (Account Holder)";
                    Title = lblTitle.Text;
                }
                else
                {
                    lblTitle.Text = "View User (Account Holder)";
                    Title = lblTitle.Text;
                }

            }

            _bIsAccountHolder = (bool)theUserRole.IsAccountHolder;

           


            lnkRoleGroupSave.Visible = false;
            lnkRoleGroupDelete.Visible = false;
            if ((bool)theUserRole.IsAdvancedSecurity)
            {
                divUserTable.Visible = false;
                trViewAllTable.Visible = false;
                //lnkRoleEdit.Visible = true;
                BindUserTableGrid();
            }
            else
            {
                BindTheRole();
                divUserTable.Visible = false;
                trViewAllTable.Visible = true;
                //lnkRoleGroupSave.Visible = true;
                if (theRole.RoleType == "1")
                {
                    //global user
                    Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/List.aspx", false);
                    return;
                }

                

            }

            //ddlAccount.Text = theUser.AccountID.ToString();
            if (_strActionMode == "edit")
            {
                ViewState["theUser"] = theUser;
                chkAccountHolder.Enabled = false;
                if (theUser.IsActive == true)
                {
                    divUnDelete.Visible = false;
                }
                else
                {
                    divDelete.Visible = false;
                }
                lblPassword.Text = "New Password";
                rfvPassword.Enabled = false;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("edit") + "&userid=" + Cryptography.Encrypt(theUser.UserID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }
                else
                {
                    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("edit") + "&userid=" + Cryptography.Encrypt(theUser.UserID.ToString());
                }


                divDelete.Visible = false;
                divUnDelete.Visible = false;
                trPassword.Visible = false;
            }
            //get user roles
            PopulateSTUserGrid();
            if((bool)theUserRole.IsAccountHolder)
            {
                ddlBasicRoles.Enabled = false;
            }

            //PopulateRolesGridForUser();

            //UserRole theUserRole = (UserRole)Session["UserRole"];

            if (theUserRole.DataScopeColumnID != null)
            {
                Column theColumn = RecordManager.ets_Column_Details((int)theUserRole.DataScopeColumnID);
                if (theColumn != null)
                {
                    try
                    {
                        ddlScopeTable.SelectedValue = theColumn.TableID.ToString();
                        ddlScopeTable_SelectedIndexChanged(null, null);
                        ddlScopeField.SelectedValue = theUserRole.DataScopeColumnID.ToString();
                        ddlScopeField_SelectedIndexChanged(null, null);
                        ddlScopeValue.SelectedValue = theUserRole.DataScopeValue;

                    }
                    catch
                    {
                        //
                    }

                }

            }

            if (_CurrentUser.UserID != _iUserID && _bIsAccountHolder)
            {
                if (_CurrentUserRole.IsAccountHolder == null || (bool)_CurrentUserRole.IsAccountHolder == false)
                {
                    txtEmail.Enabled = false;
                    txtPassword.Enabled = false;
                }
            }
            if (_CurrentUserRole.IsAccountHolder == null || (bool)_CurrentUserRole.IsAccountHolder == true)
            {
                if (theUser.IsActive == true)
                {
                    divDelete.Visible = true;
                   
                }
                else
                {
                    divUnDelete.Visible = true;
                }
            }
            else
            {
                if(ddlBasicRoles.SelectedItem!=null && ddlBasicRoles.SelectedItem.Text.ToLower()=="administrator")
                {
                    ddlBasicRoles.Enabled = false;
                    hlRoleGroupNew.Visible = false;
                }

                if(ddlBasicRoles.SelectedItem!=null && ddlBasicRoles.SelectedItem.Text.ToLower()!="administrator")
                {
                    if (ddlBasicRoles.Items.FindByText("Administrator") != null)
                        ddlBasicRoles.Items.Remove(ddlBasicRoles.Items.FindByText("Administrator"));
                }
            }

            if(!IsPostBack)
            {
                ShowHideControlByRole();
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "User Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    protected void PopulateRolesGridForUser()
    {



        //for (int i = 0; i < grdRoles.Rows.Count; i++)
        //{

        //    int iTempRoleID = int.Parse(((Label)grdRoles.Rows[i].FindControl("lblRoleID")).Text);

        //    foreach (UserRole tempUserRole in _lstUserRole)
        //    {
        //        if (tempUserRole.RoleID == iTempRoleID)
        //        {
        //          CheckBox chkA=  ((CheckBox)grdRoles.Rows[i].FindControl("chkAssigned"));
        //          chkA.Checked = true;
        //          if (tempUserRole.RoleType == "2" && _bIsAccountHolder)
        //          {
        //              chkA.Enabled = false;
        //          }
        //          break;

        //        }
        //    }
        //}

        //foreach (UserRole tempUserRole in _lstUserRole)
        //{
        //    if (tempUserRole.AccountID.ToString() == Session["AccountID"].ToString())
        //    {
        //        if (_bIsAccountHolder) //tempUserRole.RoleType == "2" &&
        //        {
        //            ddlBasicRoles.Enabled = false;
        //        }
        //    }

        //}

        

    }

    //protected void PopulateRolesGrid()
    //{
    //    int iTemp=0;

    //    List<Role> lstRoles = SecurityManager.Role_Select(null, string.Empty, string.Empty, string.Empty, null, null, "RoleID", "ASC", null, null, ref iTemp); ;
    //    if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
    //    {
    //    }
    //    else
    //    {
    //        lstRoles = lstRoles.Where(s => s.RoleType != "1").ToList();
    //    }

    //    grdRoles.DataSource = lstRoles;
    //    grdRoles.DataBind();

    //}

    protected void EnableTheRecordControls(bool p_bEnable)
    {


        txtFirstName.Enabled = p_bEnable;
        txtPhoneNumber.Enabled = p_bEnable;
        txtPassword.Enabled = p_bEnable;
        txtLastName.Enabled = p_bEnable;
        txtEmail.Enabled = p_bEnable;

        //grdRoles.Enabled = p_bEnable;
        ddlBasicRoles.Enabled = p_bEnable;
        chkAccountHolder.Enabled = p_bEnable;
        //chkAdvancedSecurity.Enabled = p_bEnable;
        chkNotifyUser.Enabled = p_bEnable;
        //divTableAdd.Visible = p_bEnable;
        hlAddAccount.Visible = p_bEnable;

        gvTheGrid.Enabled = p_bEnable;

        chkDocAdvancedSec.Enabled = p_bEnable;
        ddlBasicDocSec.Enabled = p_bEnable;
        ddlDashboard.Enabled = p_bEnable;
        ddlScopeTable.Enabled = p_bEnable;
        ddlScopeField.Enabled = p_bEnable;
        ddlScopeValue.Enabled = p_bEnable;
        //gvDocAdvancedSec.Enabled = p_bEnable;
    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action

        if (chkAccountHolder.Checked)
        {
            if (!IsAdminChecked())
            {
                lblMsg.Text = "Account holder must need Administrator Role!";
                return false;
            }
        }

        if (txtPassword.Text.Trim().Length > 0)
        {

            if (txtPassword.Text.Trim().Length < 6)
            {
                lblMsg.Text = "Password Minimum length is 6.";
                txtPassword.Focus();
                return false;

            }

            if (txtPassword.Text.ToLower().IndexOf(txtFirstName.Text.ToLower()) > -1)
            {
                lblMsg.Text = "Password should not have first name!";
                txtPassword.Focus();
                return false;
            }
            if (txtPassword.Text.ToLower().IndexOf(txtLastName.Text.ToLower()) > -1)
            {
                lblMsg.Text = "Password should not have last name!";
                txtPassword.Focus();
                return false;
            }
            if (txtPassword.Text.ToLower().IndexOf(txtEmail.Text.ToLower().Substring(0, txtEmail.Text.IndexOf("@"))) > -1)
            {
                lblMsg.Text = "Password should not have email address!";
                txtPassword.Focus();
                return false;
            }
          
        }


        if (ddlBasicRoles.SelectedValue == "")
        {
            lblMsg.Text = "Please select a role.";

            return false;

        }

        return true;
    }
    protected void btnResetDashBoard_Click(object sender, EventArgs e)
    {
        if (ddlBasicRoles.SelectedValue != "")
        {
            try
            {
                DocumentManager.dbg_Dashboard_ResetUsers_ByRole(int.Parse(ddlBasicRoles.SelectedValue));
                Session["tdbmsgpb"] = "Dashboard reset has been completed successfully.";
            }
            catch
            {
                //
            }
        }
        
    }
    protected void btnRestoreUser_Click(object sender, EventArgs e)
    {

        try
        {
            User vUser = SecurityManager.User_By_Email(txtEmail.Text);
            if (vUser != null)
            {

                vUser.IsActive = true;
                SecurityManager.User_Update(vUser);

                Session["tdbmsg"] = vUser.Email + " user has been restored.";
                string strExtra = Request.QueryString["SearchCriteria"] == null ? "" : "&" + Request.QueryString["SearchCriteria"].ToString();

                string strRawURL = "~/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("edit") + "&UserID=" + Cryptography.Encrypt(vUser.UserID.ToString()) + strExtra;
                
                //strRawURL = Common.GetUpdatedFullURLWithQueryString(strRawURL, "mode", Cryptography.Encrypt("edit"));
                //strRawURL = Common.GetUpdatedFullURLWithQueryString(strRawURL, "UserID", Cryptography.Encrypt(vUser.UserID.ToString()));
                Response.Redirect(strRawURL, false);
                return;
            }
        }
        catch
        {
            //

        }

       
    }
    protected void btnAddUserLinkOK_Click(object sender, EventArgs e)
    {
        string strUserID = Common.GetValueFromSQL("SELECT UserID FROM [User] WHERE Email='"+txtEmail.Text.Replace("'","''")+"'");
        if(strUserID!="")
        {
            Role theRole = SecurityManager.Role_Details(int.Parse(ddlBasicRoles.SelectedValue));
            UserRole newUserRole = new UserRole();
            newUserRole.AccountID = int.Parse(Session["AccountID"].ToString());
            newUserRole.IsAdvancedSecurity = !(bool)theRole.IsSystemRole;
            newUserRole.IsAccountHolder = false;
            //if (ddlDashboard.Items.Count > 0 && ddlDashboard.SelectedValue != "")
            //{
            //    newUserRole.DashBoardDocumentID = int.Parse(ddlDashboard.SelectedValue);
            //}

            if (chkDocAdvancedSec.Checked)
            {
                newUserRole.IsDocSecurityAdvanced = true;

            }
            else
            {
                newUserRole.IsDocSecurityAdvanced = false;
                newUserRole.DocSecurityType = ddlBasicDocSec.SelectedValue;

            }

            if (_theAccount.UseDataScope != null)
            {
                if ((bool)_theAccount.UseDataScope && ddlScopeField.SelectedItem != null && ddlScopeValue.SelectedItem != null)
                {
                    if (ddlScopeField.SelectedValue != "" && ddlScopeValue.SelectedValue != "")
                    {
                        //all good
                        newUserRole.DataScopeColumnID = int.Parse(ddlScopeField.SelectedValue);
                        newUserRole.DataScopeValue = ddlScopeValue.SelectedValue;

                    }

                }
            }

            newUserRole.UserID = int.Parse(strUserID);
            newUserRole.RoleID = int.Parse(ddlBasicRoles.SelectedValue);
            newUserRole.IsPrimaryAccount = false;

            int iNewUserRoleID = SecurityManager.UserRole_Insert(newUserRole);

            if (chkDocAdvancedSec.Checked)
            {
                UpdateUserFolder(int.Parse(strUserID));
            }

            Session["tdbmsg"] = "An existing user " + txtEmail.Text + " has been added to this account, you can find " + txtEmail.Text + " in user list.";
            Response.Redirect(hlBack.NavigateUrl, false);
        }
    }
    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        lblMsg.ForeColor = System.Drawing.Color.Red;
        int? iTheUserID = null;
        try
        {
            if (IsUserInputOK())
            {
                Role theRole = SecurityManager.Role_Details(int.Parse(ddlBasicRoles.SelectedValue));

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        if (!SecurityManager.CanThisAccountAddUser(int.Parse(Session["AccountID"].ToString())))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", @"alert('You have reached the maximum number of users 
                            allowed for your account type.  In order to add a new user you must either delete an existing user or upgrade your account. 
                            See My Account page for options.');", true);

                            //Response.Redirect("~/Pages/Security/AccountTypeChange.aspx?type=renew", false);

                            return;
                        }



                        User newUser = new User(null, txtFirstName.Text,
                        txtLastName.Text, txtPhoneNumber.Text, txtEmail.Text, txtPassword.Text,
                        true, DateTime.Now, DateTime.Now);//,  "", false, chkAdvancedSecurity.Checked

                        UserRole newUserRole = new UserRole();
                        newUserRole.AccountID = int.Parse(Session["AccountID"].ToString());
                        newUserRole.IsAdvancedSecurity =!(bool) theRole.IsSystemRole;
                        newUserRole.IsAccountHolder = false;
                        //if (ddlDashboard.Items.Count > 0 && ddlDashboard.SelectedValue!="")
                        //{
                        //    newUserRole.DashBoardDocumentID = int.Parse(ddlDashboard.SelectedValue);
                        //}

                        if (chkDocAdvancedSec.Checked)
                        {
                            newUserRole.IsDocSecurityAdvanced = true;

                        }
                        else
                        {
                            newUserRole.IsDocSecurityAdvanced = false;
                            newUserRole.DocSecurityType = ddlBasicDocSec.SelectedValue;

                        }

                        if (theRole != null && theRole.RoleType == "2")
                        {
                            if (_CurrentUserRole.IsAccountHolder != null && (bool)_CurrentUserRole.IsAccountHolder)
                            {
                                newUserRole.AllowDeleteTable = chkAllowDeleteTable.Checked;
                                newUserRole.AllowDeleteColumn = chkAllowDeleteColumn.Checked;
                                newUserRole.AllowDeleteRecord = chkAllowDeleteRecord.Checked;
                            }

                        }
                        if (_theAccount.UseDataScope != null)
                        {
                            if ((bool)_theAccount.UseDataScope && ddlScopeField.SelectedItem!=null && ddlScopeValue.SelectedItem!=null)
                            {
                                if (ddlScopeField.SelectedValue != "" && ddlScopeValue.SelectedValue != "")
                                {
                                    //all good
                                    newUserRole.DataScopeColumnID = int.Parse(ddlScopeField.SelectedValue);
                                    newUserRole.DataScopeValue = ddlScopeValue.SelectedValue;

                                }

                            }
                        }


                         //if (chkAdvancedSecurity.Checked)
                         //{
                         //    if(ddlRoleGroup.SelectedValue!="")
                         //    {
                         //        newUser.RoleGroupID = int.Parse(ddlRoleGroup.SelectedValue);
                         //    }

                         //}

                        int iNewUserID = SecurityManager.User_Insert(newUser);

                        if(iNewUserID==-1)
                        {
                            if (Session["tdbmsgpb"] != null && Session["tdbmsgpb"].ToString().IndexOf("email address")>-1)
                            {
                                //Session["tdbmsg"] = null;

                                User vUser = SecurityManager.User_By_Email(txtEmail.Text);
                                if(vUser!=null )
                                {
                                   int? iAccountID= SecurityManager.GetPrimaryAccountID((int)vUser.UserID);
                                   if (iAccountID != null && iAccountID==int.Parse(Session["AccountID"].ToString()) && vUser.IsActive != null)
                                    {
                                       if((bool)vUser.IsActive)
                                       {
                                           return;
                                       }
                                       else
                                       {
                                            //oliver <begin> Ticket 940
                                            Session["tdbmsgpb"] = null;
                                            //oliver <end>

                                            btnRestoreUser.CommandArgument = vUser.UserID.ToString();
                                           hlAddUserLink.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                                                Cryptography.Encrypt("The user with the email " + vUser.Email + " is already in the database but has been deleted. Do you wish to restore " + vUser.Email + "?")
                                                + "&okbutton=" + Cryptography.Encrypt(btnRestoreUser.ClientID);
                                           ScriptManager.RegisterStartupScript(this, this.GetType(), "jsActivateUserConfirm", "setTimeout(function () { OpenAddUserConfirm(); }, 1000);", true);
                                           return;
                                       }

                                    }
                                }


                                //Session["tdbmsgpb"] = null;
                                hlAddUserLink.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                  Cryptography.Encrypt("User " + txtEmail.Text + " has at least one account. Do you want to add " + txtEmail.Text + " to your account too?")
                  + "&okbutton=" + Cryptography.Encrypt(btnAddUserLinkOK.ClientID);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "jsAddUserConfirm", "setTimeout(function () { OpenAddUserConfirm(); }, 1000);", true);

                            }
                            else
                            {
                                Response.Redirect(Request.RawUrl, false);
                            }

                           
                            return;
                        }

                        newUserRole.UserID = iNewUserID;
                        newUserRole.RoleID = int.Parse(ddlBasicRoles.SelectedValue);
                        newUserRole.IsPrimaryAccount = true;

                        //if(theRole.AllowEditDashboard!=null)
                        //{
                        //    if((bool)theRole.AllowEditDashboard)
                        //    {
                        //        if(chkUserRoleEditDashBoard.Checked)
                        //        {
                        //            newUserRole.AllowEditDashboard = true;
                        //        }
                        //    }
                        //}



                        int iNewUserRoleID = SecurityManager.UserRole_Insert(newUserRole);


                        if (chkDocAdvancedSec.Checked)
                        {
                            UpdateUserFolder(iNewUserID);
                        }

                        //if ((bool)newUserRole.IsAdvancedSecurity)
                        //{
                        //    //if (newUser.RoleGroupID==null)
                        //    //{
                        //        //UpdateUserTable();
                        //    //}
                           
                        //    InsertUserBasicRole(iNewUserID);
                        //}
                        //else
                        //{
                        //    InsertUserBasicRole(iNewUserID);
                        //}

                        iTheUserID = iNewUserID;


                        //if (Request.QueryString["SearchCriteria"] != null)
                        //{
                        //    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("edit") + "&userid=" + Cryptography.Encrypt(iTheUserID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&fromadd=yes";
                        //}
                        //else
                        //{
                        //    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("edit") + "&userid=" + Cryptography.Encrypt(iTheUserID.ToString()) + "&fromadd=yes";
                        //}

                        Session["tdbmsg"] = txtEmail.Text + " user has been added";
                        break;



                    case "edit":
                        User editUser = (User)ViewState["theUser"];


                        UserRole editUserRole = SecurityManager.GetUserRole((int)editUser.UserID, int.Parse(Session["AccountID"].ToString()));
                        string strUserInfo = "";

                        strUserInfo = strUserInfo + "<div><table>";

                        if (editUser.FirstName != txtFirstName.Text)
                        {
                            editUser.FirstName = txtFirstName.Text;
                            strUserInfo = strUserInfo + "<tr style='color:blue;'><td >First Name: </td><td>" + editUser.FirstName + "</td></tr><tr>";
                        }
                        else
                        {
                            strUserInfo = strUserInfo + "<tr><td >First Name: </td><td>" + editUser.FirstName + "</td></tr><tr>";
                        }


                        if (txtPassword.Text.Trim().Length > 0)
                        {
                            editUser.Password = txtPassword.Text;
                        }


                        if (editUser.LastName != txtLastName.Text)
                        {
                            editUser.LastName = txtLastName.Text;
                            strUserInfo = strUserInfo + "<tr style='color:blue;'><td >Last Name: </td><td>" + editUser.LastName + "</td></tr><tr>";
                        }
                        else
                        {
                            strUserInfo = strUserInfo + "<tr><td >Last Name: </td><td>" + editUser.LastName + "</td></tr><tr>";
                        }

                        strUserInfo = strUserInfo + "</table></div>";

                        editUser.PhoneNumber = txtPhoneNumber.Text;
                        editUser.Email = txtEmail.Text;

                        //editUser.IsActive = chkIsActive.Checked;
                        //editUser.IsAccountHolder = chkAccountHolder.Checked;

                        //editUser.AccountID = int.Parse(Session["AccountID"].ToString());
                        editUser.DateUpdated = DateTime.Now;
                       

                        //if (hfUserName.Value == "")
                        //{
                        //    editUser.UserName = txtEmail.Text;
                        //}
                        //else
                        //{
                        //    editUser.UserName = hfUserName.Value;
                        //}

                        //if (ddlDashboard.Items.Count > 0 && ddlDashboard.SelectedValue != "")
                        //{
                        //    editUserRole.DashBoardDocumentID = int.Parse(ddlDashboard.SelectedValue);
                        //}
                        //else
                        //{
                        //    editUserRole.DashBoardDocumentID = null;
                        //}

                        if (theRole != null && theRole.RoleType == "2")
                        {
                            if (_CurrentUserRole.IsAccountHolder != null && (bool)_CurrentUserRole.IsAccountHolder)
                            {
                                editUserRole.AllowDeleteTable = chkAllowDeleteTable.Checked;
                                editUserRole.AllowDeleteColumn = chkAllowDeleteColumn.Checked;
                                editUserRole.AllowDeleteRecord = chkAllowDeleteRecord.Checked;
                            }

                        }
                        else
                        {
                            editUserRole.AllowDeleteTable = null;
                            editUserRole.AllowDeleteColumn = null;
                            editUserRole.AllowDeleteRecord = null;
                        }


                        if (chkDocAdvancedSec.Checked)
                        {
                            editUserRole.IsDocSecurityAdvanced = true;

                        }
                        else
                        {
                            editUserRole.IsDocSecurityAdvanced = false;
                            editUserRole.DocSecurityType = ddlBasicDocSec.SelectedValue;

                        }

                        editUserRole.DataScopeColumnID = null;
                        editUserRole.DataScopeValue = null;
                        if (_theAccount.UseDataScope != null)
                        {
                            if ((bool)_theAccount.UseDataScope && ddlScopeField.SelectedItem != null && ddlScopeValue.SelectedItem != null)
                            {
                                if (ddlScopeField.SelectedValue != "" && ddlScopeValue.SelectedValue != "")
                                {
                                    //all good
                                    editUserRole.DataScopeColumnID = int.Parse(ddlScopeField.SelectedValue);
                                    editUserRole.DataScopeValue = ddlScopeValue.SelectedValue;
                                }

                            }
                        }


                       

                        int iIsUpdated = SecurityManager.User_Update(editUser);

                        if (iIsUpdated == -1)
                        {
                            Response.Redirect(Request.RawUrl, false);
                            return;
                        }

                        //if (ddlBasicRoles.Enabled==true)
                            

                        User theAccountHolder = SecurityManager.User_AccountHolder(int.Parse(Session["AccountID"].ToString()));
                        if (theAccountHolder!=null)
                        {
                            if((int)theAccountHolder.UserID==(int)editUser.UserID)
                            {
                                //
                            }
                            else
                            {
                                editUserRole.RoleID = int.Parse(ddlBasicRoles.SelectedValue);
                                editUserRole.IsAdvancedSecurity = !(bool)theRole.IsSystemRole;
                            }
                        }
                        else
                        {
                            editUserRole.RoleID = int.Parse(ddlBasicRoles.SelectedValue);
                            editUserRole.IsAdvancedSecurity = !(bool)theRole.IsSystemRole;
                        }
                       
                        
                        //editUserRole.AllowEditDashboard = false;
                        //if (theRole.AllowEditDashboard != null)
                        //{
                        //    if ((bool)theRole.AllowEditDashboard)
                        //    {
                        //        if (chkUserRoleEditDashBoard.Checked)
                        //        {
                        //            editUserRole.AllowEditDashboard = true;
                        //        }
                        //    }
                        //}



                        SecurityManager.UserRole_Update(editUserRole);


                        iTheUserID = (int)editUser.UserID;

                        //now update roles
                        if (iIsUpdated == 1)
                        {
                            if (chkDocAdvancedSec.Checked)
                            {
                                UpdateUserFolder((int)editUser.UserID);
                            }

                            if (ddlBasicRoles.SelectedValue == "6")
                            {
                                editUser.IsActive = false;
                                SecurityManager.User_Update(editUser);
                            }

                            //if ((bool)editUserRole.IsAdvancedSecurity)
                            //{
                               
                                   
                            //}
                            //else
                            //{

                            //    //it's a basic role
                                                             

                            //    UserRole theUserRole = new UserRole(null, null, null, null, null);
                            //    foreach (UserRole tempUserRole in _lstUserRole)
                            //    {
                            //        if (tempUserRole.AccountID.ToString() == Session["AccountID"].ToString())
                            //        {
                            //            theUserRole = tempUserRole;
                            //            break;
                            //        }
                            //    }

                            //    theUserRole.RoleID = theRole.RoleID;
                            //    theUserRole.AccountID = int.Parse(Session["AccountID"].ToString());
                            //    if (theUserRole.UserRoleID != null)
                            //    {
                            //        SecurityManager.UserRole_Update(theUserRole);
                            //    }
                            //    else
                            //    {
                            //        //InsertUserBasicRole((int)editUser.UserID);
                            //    }

                            //    if (ddlBasicRoles.SelectedValue == "6")
                            //    {
                            //        editUser.IsActive = false;
                            //        SecurityManager.User_Update(editUser);
                            //    }

                            //}

                            //Send email

                        }

                        Session["tdbmsg"] = txtEmail.Text + " user has been updated";

                        break;

                    default:
                        //?
                        break;
                }

                //now update user Table

            }
            else
            {
                //user input is not ok
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alart Messagee.", "alert('" + lblMsg.Text + "');", true);
                lblMsg.Text = "";
                return;

            }

            // Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/List.aspx", false);


            if (chkNotifyUser.Checked)
            {
                //send email to the user
                if (iTheUserID != null)
                {
                    Content theConent = SystemData.Content_Details_ByKey("UserAccountDetails",null);

                    DataTable theSPTable = SystemData.Run_ContentSP("ets_UserAccountDetails", iTheUserID.ToString());
                    string strBody = Common.ReplaceDataFiledByValue(theSPTable, theConent.ContentP);

                    strBody = strBody.Replace("[URL]", Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath);

                    string strTo = txtEmail.Text.Trim();

                    string strError = "";

                    theConent.ContentP = strBody;

                    try
                    {
                        //Common.SendSingleEmail(strTo, theConent, ref strError);
                        DBGurus.SendEmail(theConent.ContentKey, true, null, theConent.Heading, theConent.ContentP, "", strTo, "", "", null,null, out strError);


                        Session["tdbmsg"]=Session["tdbmsg"]==null?"":Session["tdbmsg"].ToString()+ " and";
                        Session["tdbmsg"] = Session["tdbmsg"].ToString() + " a notification email has been sent to this user.";

                    }
                    catch
                    {
                        //
                    }


                }

            }


            //if (_strActionMode.ToLower() == "add")
            //{
            //    Response.Redirect(hlEditLink.NavigateUrl, false);
            //}
            //else
            //{
            Response.Redirect(hlBack.NavigateUrl, false);
            //}

        }

        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "User Save", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

            //string strUserName = hfUserName.Value == "" ? txtEmail.Text : hfUserName.Value;
            if (ex.Message.IndexOf("UQ_User_Email") > -1)
            {

                //if (txtEmail.Text == hfUserName.Value || hfUserName.Value=="")
                //{
                lblMsg.Text = "That email(" + txtEmail.Text + ") is already being used. Please use another email.";
                //}
                //else
                //{
                //    lblMsg.Text = "That Username(" + strUserName + ") is already being used on another account. You can use another Username, please press Advanced…";
                //}
                txtEmail.Focus();
            }
            else
            {

                lblMsg.Text = ex.Message;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alart Messagee.", "alert('" + lblMsg.Text + "');", true);
            lblMsg.Text = "";
        }


    }

    protected void btnRoleSaved_Click(object sender, EventArgs e)
    {

        //PopulateRoleGroup();
        PopulateRole();
        
        ddlBasicRoles.SelectedValue = hfRoleGroupID.Value;
        Role theRole = SecurityManager.Role_Details(int.Parse(ddlBasicRoles.SelectedValue));

        theRole.AllowEditDashboard = chkRoleEditDashboard.Checked;
        theRole.DashboardDefaultFromUserID = ddlDashboard.SelectedValue==""?null:(int?)int.Parse(ddlDashboard.SelectedValue);

        theRole.AllowEditView = chkRole_AllowEditView.Checked;
        theRole.ViewsDefaultFromUserID = ddlRole_ViewsDefaultFromUserID.SelectedValue == "" ? null : (int?)int.Parse(ddlRole_ViewsDefaultFromUserID.SelectedValue);


        SecurityManager.Role_Update(theRole);

        if(hfRoleGroupID.Value!="")
        {
             int iRoleGroupID = int.Parse(hfRoleGroupID.Value);
             if ((bool)theRole.IsSystemRole==false)
             {
                 //add records into RoleGroupTable
                 for (int i = 0; i < gvUserTable.Rows.Count; i++)
                 {
                     int iTableID = int.Parse(((Label)gvUserTable.Rows[i].FindControl("lblTableID")).Text);
                     int iRecordRightID = int.Parse(((DropDownList)gvUserTable.Rows[i].FindControl("ddlRecordRightID")).SelectedValue);
                     bool bCanExport = ((CheckBox)gvUserTable.Rows[i].FindControl("chkCaExport")).Checked;
                     DropDownList ddlViewsDefaultFromUserID = (DropDownList)gvUserTable.Rows[i].FindControl("ddlViewsDefaultFromUserID");
                     CheckBox chkAllowEditView = (CheckBox)gvUserTable.Rows[i].FindControl("chkAllowEditView");
                     CheckBox chkShowMenu = (CheckBox)gvUserTable.Rows[i].FindControl("chkShowMenu");
                     //RoleGroupTable newRoleGroupTable = new RoleGroupTable(null, iRoleGroupID, iRecordRightID, iTableID, bCanExport);

                     //SecurityManager.dbg_RoleGroupTable_Insert(newRoleGroupTable,null,null);
                     RoleTable newRoleTable = new RoleTable(null, iTableID, iRecordRightID, null, null);

                    
                     newRoleTable.RoleID = theRole.RoleID;

                     newRoleTable.ViewsDefaultFromUserID = ddlViewsDefaultFromUserID.SelectedValue == "" ? null : (int?)int.Parse(ddlViewsDefaultFromUserID.SelectedValue);
                     newRoleTable.AllowEditView = chkAllowEditView.Checked;
                     newRoleTable.ShowMenu = chkShowMenu.Checked;

                     SecurityManager.dbg_RoleTable_Insert(newRoleTable);
                     //make sure this user has View
                     if(newRoleTable.ViewsDefaultFromUserID!=null)
                     {
                         ViewManager.dbg_View_BestFittingNew((int)newRoleTable.ViewsDefaultFromUserID, "list", (int)newRoleTable.TableID,null);
                         //ViewManager.dbg_View_BestFittingNew((int)newRoleTable.ViewsDefaultFromUserID, "child", (int)newRoleTable.TableID);

                     }
                     
                 }

             }
        }

        ddlBasicRoles_SelectedIndexChanged(null, null);
       
    }

    protected void UpdateUserTable()
    {
        Role theRole = SecurityManager.Role_Details(int.Parse(ddlBasicRoles.SelectedValue));


        if ((bool)theRole.IsSystemRole==false)
        {
            //if (_strActionMode.ToLower() == "add")
            //{

            //    for (int i = 0; i < gvUserTable.Rows.Count; i++)
            //    {
            //        int iTableID = int.Parse(((Label)gvUserTable.Rows[i].FindControl("lblTableID")).Text);
            //        int iRecordRightID = int.Parse(((DropDownList)gvUserTable.Rows[i].FindControl("ddlRecordRightID")).SelectedValue);
            //        bool bCanExport = ((CheckBox)gvUserTable.Rows[i].FindControl("chkCaExport")).Checked;

            //        RoleTable newUserTable = new RoleTable(null, iTableID, iRecordRightID,null,null);
            //        newUserTable.RoleID = int.Parse(ddlBasicRoles.SelectedValue);
            //        newUserTable.CanExport = bCanExport;
            //        SecurityManager.dbg_RoleTable_Insert(newUserTable);
            //    }


            //}
            //else if (_strActionMode.ToLower() == "edit")
            //{

                for (int i = 0; i < gvUserTable.Rows.Count; i++)
                {
                    int iTableID = int.Parse(((Label)gvUserTable.Rows[i].FindControl("lblTableID")).Text);
                    int iRecordRightID = int.Parse(((DropDownList)gvUserTable.Rows[i].FindControl("ddlRecordRightID")).SelectedValue);
                    //int iSiteRightID = int.Parse(((DropDownList)gvUserTable.Rows[i].FindControl("ddlSiteRightID")).SelectedValue);
                    bool bCanExport = ((CheckBox)gvUserTable.Rows[i].FindControl("chkCaExport")).Checked;
                    DropDownList ddlViewsDefaultFromUserID = (DropDownList)gvUserTable.Rows[i].FindControl("ddlViewsDefaultFromUserID");
                    CheckBox chkAllowEditView = (CheckBox)gvUserTable.Rows[i].FindControl("chkAllowEditView");
                    CheckBox chkShowMenu = (CheckBox)gvUserTable.Rows[i].FindControl("chkShowMenu");

                    DataTable dtTemp = SecurityManager.dbg_RoleTable_Select(null, iTableID, theRole.RoleID, null);

                    if (dtTemp.Rows.Count > 0)
                    {
                        RoleTable editRoleTable = SecurityManager.dbg_RoleTable_Detail(int.Parse(dtTemp.Rows[0]["RoleTableID"].ToString()));
                        editRoleTable.RoleType = iRecordRightID;
                        editRoleTable.CanExport = bCanExport;
                        editRoleTable.RoleID = int.Parse(ddlBasicRoles.SelectedValue);

                        editRoleTable.ViewsDefaultFromUserID = ddlViewsDefaultFromUserID.SelectedValue == "" ? null : (int?)int.Parse(ddlViewsDefaultFromUserID.SelectedValue);
                        editRoleTable.AllowEditView = chkAllowEditView.Checked;
                        editRoleTable.ShowMenu = chkShowMenu.Checked;
                        
                        SecurityManager.dbg_RoleTable_Update(editRoleTable);
                        if (editRoleTable.ViewsDefaultFromUserID != null)
                        {
                            ViewManager.dbg_View_BestFittingNew((int)editRoleTable.ViewsDefaultFromUserID, "list", (int)editRoleTable.TableID,null);
                            //ViewManager.dbg_View_BestFittingNew((int)editRoleTable.ViewsDefaultFromUserID, "child", (int)editRoleTable.TableID);

                        }
                    }
                    else
                    {

                        RoleTable newRoleTable = new RoleTable(null, iTableID, iRecordRightID, null, null);
                        newRoleTable.CanExport = bCanExport;
                        newRoleTable.RoleID = int.Parse(ddlBasicRoles.SelectedValue);

                        newRoleTable.ViewsDefaultFromUserID = ddlViewsDefaultFromUserID.SelectedValue == "" ? null : (int?)int.Parse(ddlViewsDefaultFromUserID.SelectedValue);
                        newRoleTable.AllowEditView = chkAllowEditView.Checked;
                        newRoleTable.ShowMenu = chkShowMenu.Checked;


                        SecurityManager.dbg_RoleTable_Insert(newRoleTable);
                        if (newRoleTable.ViewsDefaultFromUserID != null)
                        {
                            ViewManager.dbg_View_BestFittingNew((int)newRoleTable.ViewsDefaultFromUserID, "list", (int)newRoleTable.TableID,null);
                            //ViewManager.dbg_View_BestFittingNew((int)newRoleTable.ViewsDefaultFromUserID, "child", (int)newRoleTable.TableID);

                        }
                    }
                }

            //}
        }

    }


    protected void UpdateUserFolder(int iUserID)
    {
        if (chkDocAdvancedSec.Checked)
        {
            if (_strActionMode.ToLower() == "add")
            {

                for (int i = 0; i < gvDocAdvancedSec.Rows.Count; i++)
                {
                    int? iFolderID = int.Parse(((Label)gvDocAdvancedSec.Rows[i].FindControl("LblID")).Text);
                    string  strRight= ((DropDownList)gvDocAdvancedSec.Rows[i].FindControl("ddlBasicDocSecEach")).SelectedValue;

                    if (iFolderID == -1)
                        iFolderID = null;

                    UserFolder newUserFolder = new UserFolder(null, iFolderID, iUserID, strRight);
                    DocumentManager.ets_UserFolder_Insert(newUserFolder);
                }


            }
            else if (_strActionMode.ToLower() == "edit")
            {

                for (int i = 0; i < gvDocAdvancedSec.Rows.Count; i++)
                {
                    int? iFolderID = int.Parse(((Label)gvDocAdvancedSec.Rows[i].FindControl("LblID")).Text);
                    string strRight = ((DropDownList)gvDocAdvancedSec.Rows[i].FindControl("ddlBasicDocSecEach")).SelectedValue;

                    if (iFolderID == -1)
                        iFolderID = null;

                    string strSQL = "";

                    if (iFolderID == null)
                    {
                        strSQL = "SELECT UserFolderID FROM UserFolder WHERE UserID="+iUserID.ToString()+" AND FolderID IS NULL";
                    }
                    else
                    {
                        strSQL = "SELECT UserFolderID FROM UserFolder WHERE UserID=" + iUserID.ToString() + " AND FolderID=" + iFolderID.ToString();
                    }

                    DataTable dtTemp = Common.DataTableFromText(strSQL);

                    if (dtTemp.Rows.Count > 0)
                    {
                        string strUpdateSQL = "";
                        if (iFolderID == null)
                        {
                            Common.ExecuteText("UPDATE UserFolder SET RightType='" + strRight + "' WHERE UserID="+iUserID.ToString()+" AND FolderID IS NULL   ");
                        }
                        else
                        {
                            Common.ExecuteText("UPDATE UserFolder SET RightType='" + strRight + "' WHERE UserID="+iUserID.ToString()+" AND FolderID=" + iFolderID.ToString());
                        }
                    }
                    else
                    {
                      
                        if (iFolderID == -1)
                            iFolderID = null;

                        UserFolder newUserFolder = new UserFolder(null, iFolderID, iUserID, strRight);
                        DocumentManager.ets_UserFolder_Insert(newUserFolder);
                    }
                }

            }
        }

    }



    //protected void lnkBack_Click(object sender, EventArgs e)
    //{
    //  Response.Redirect (Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/List.aspx",false );
    //}


    protected void lnkRoleGroupSave_Click(object sender, EventArgs e)
    {


        if (ddlBasicRoles.SelectedValue == "")
        {
        }
        else
        {
            int iRoleID = int.Parse(ddlBasicRoles.SelectedValue);

            Role theRole = SecurityManager.Role_Details(iRoleID);

            if (theRole != null)
            {
                if ((bool)_CurrentUserRole.IsAccountHolder || (theRole.OwnerUserID != null && theRole.OwnerUserID == _CurrentUser.UserID)
                    || (_CurrentRole != null && _CurrentRole.RoleType=="2"))
                {
                    theRole.AllowEditDashboard = chkRoleEditDashboard.Checked;
                    theRole.DashboardDefaultFromUserID = ddlDashboard.SelectedValue == "" ? null : (int?)int.Parse(ddlDashboard.SelectedValue);
                    theRole.AllowEditView = chkRole_AllowEditView.Checked;
                    theRole.ViewsDefaultFromUserID = ddlRole_ViewsDefaultFromUserID.SelectedValue == "" ? null : (int?)int.Parse(ddlRole_ViewsDefaultFromUserID.SelectedValue);


                    
                    SecurityManager.Role_Update(theRole);

                    if (theRole.IsSystemRole == false)
                        UpdateUserTable();

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Role Saved.');", true);
                    Session["tdbmsgpb"] = theRole.RoleName + " role has been saved.";
                    ViewState["rolesaved"] = "yes";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('You can not update this role.');", true);
                    Session["tdbmsgpb"] = "You can not update " + ddlBasicRoles.SelectedItem.Text + " role.";
                    ViewState["rolesaved"]=null;
                }

            }


        }



        //if (ddlRoleGroup.SelectedValue == "")
        //{
        //}
        //else
        //{
        //    int iRoleGroupID = int.Parse(ddlRoleGroup.SelectedValue);

        //    RoleGroup theRoleGroup = SecurityManager.dbg_RoleGroup_Detail(iRoleGroupID, null, null);

        //    if (theRoleGroup!=null)
        //    {
        //        if (theRoleGroup.OwnerUserID == _CurrentUser.UserID)
        //        {
        //            for (int i = 0; i < gvUserTable.Rows.Count; i++)
        //            {
        //                int iTableID = int.Parse(((Label)gvUserTable.Rows[i].FindControl("lblTableID")).Text);
        //                int iRecordRightID = int.Parse(((DropDownList)gvUserTable.Rows[i].FindControl("ddlRecordRightID")).SelectedValue);
        //                bool bCanExport = ((CheckBox)gvUserTable.Rows[i].FindControl("chkCaExport")).Checked;

        //                RoleGroupTable newRoleGroupTable = new RoleGroupTable(null, iRoleGroupID, iRecordRightID, iTableID, bCanExport);


        //                DataTable dtTemp = Common.DataTableFromText("SELECT RoleGroupTableID FROM RoleGroupTable WHERE RoleGroupID=" + iRoleGroupID.ToString() + " AND TableID=" + iTableID.ToString());

        //                if (dtTemp.Rows.Count > 0)
        //                {
        //                    newRoleGroupTable.RoleGroupTableID = int.Parse(dtTemp.Rows[0]["RoleGroupTableID"].ToString());
        //                    SecurityManager.dbg_RoleGroupTable_Update(newRoleGroupTable, null);
        //                }
        //                else
        //                {
        //                    SecurityManager.dbg_RoleGroupTable_Insert(newRoleGroupTable, null, null);
        //                }


        //            }

        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('You can not update this role.');", true);

        //        }

        //    }
           

        //}

    }

    protected void lnkRoleGroupDelete_Click(object sender, EventArgs e)
    {
        if (ddlBasicRoles.SelectedValue == "")
        {
        }
        else
        {
            int iRoleID = int.Parse(ddlBasicRoles.SelectedValue);

            Role theRole = SecurityManager.Role_Details(iRoleID);

            if (theRole != null)
            {
                if (theRole.IsSystemRole == false 
                    //&&
                    //(theRole.OwnerUserID == _CurrentUser.UserID || (bool)_CurrentUserRole.IsAccountHolder
                    //|| (_CurrentRole != null && _CurrentRole.RoleType == "2"))
                    )
                {
                    string strRoleName = ddlBasicRoles.SelectedItem.Text;
                    SecurityManager.Role_Delete(iRoleID);
                    PopulateRole();
                    BindUserTableGrid();
                    lnkRoleEdit.Visible = false;
                    lnkRoleGroupDelete.Visible = false;
                    lnkRoleGroupSave.Visible = false;
                    divUserTable.Visible = false;
                    Session["tdbmsgpb"] = strRoleName + " role has been deleted.";
                   // ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Role Deleted.');", true);
                }
                //else if (theRole.OwnerUserID == _CurrentUser.UserID && theRole.IsSystemRole == true)
                //{
                //    lnkRoleGroupSave.Visible = true;
                //    trViewAllTable.Visible = true;
                //    BindTheRole();
                //}
                else
                {
                    //if (theRole.IsSystemRole == true) //&& _bIsAccountHolder
                    //{
                        Common.ExecuteText("UPDATE [Role] SET IsActive=0 WHERE RoleID=" + theRole.RoleID);
                        Session["tdbmsgpb"] = "Role Deleted.";
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('You can not delete this role.');", true);
                    //}
                }

            }

           
        }

        
        



        //if (ddlRoleGroup.SelectedValue == "")
        //{
        //}
        //else
        //{
        //    int iRoleGroupID = int.Parse(ddlRoleGroup.SelectedValue);

        //    RoleGroup theRoleGroup = SecurityManager.dbg_RoleGroup_Detail(iRoleGroupID, null, null);

        //    if (theRoleGroup != null)
        //    {
        //        if (theRoleGroup.OwnerUserID == _CurrentUser.UserID)
        //        {
        //            SecurityManager.dbg_RoleGroup_Delete(iRoleGroupID);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('You can not delete this role.');", true);

        //        }
        //    }
            
        //    PopulateRoleGroup();
        //}
    }

    protected void lnkRoleEdit_Click(object sender, EventArgs e)
    {
       
        lnkRoleGroupSave.Visible = true;

        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
            lnkRoleGroupDelete.Visible = true;
        
        divRoleSpecialRights.Visible = true;
        _bShowTableGrid = true;
        ShowHideControlByRole();

        lnkRoleEdit.Visible = false;
        //BindUserTableGrid();
        //if (ddlBasicRoles.SelectedValue == "")
        //{

        //    //
        //}
        //else
        //{

           
        //    trViewAllTable.Visible = false;
            
        //    divUserTable.Visible = true;
        //    BindUserTableGrid();

        //}

    }

    protected void lnkResetDashBoard_Click(object sender, EventArgs e)
    {
        ViewState["rolesaved"] = null;
        if(ddlBasicRoles.SelectedValue!="")
        {
            lnkRoleGroupSave_Click(null, null);

            if (ViewState["rolesaved"]!=null)
            {
                hlResetDashBoard.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
Cryptography.Encrypt("Do you want to reset dashboard of all users of the role " + ddlBasicRoles.SelectedItem.Text + "?")
+ "&okbutton=" + Cryptography.Encrypt(btnResetDashBoard.ClientID);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "jsOpenDashResetConfirm", "setTimeout(function () { OpenDashResetConfirm(); }, 1000);", true);

            }


        }
        else
        {
            Session["tdbmsgpb"] = "Please select a role.";
        }

    }


//    protected void lnkResetViews_Click(object sender, EventArgs e)
//    {
//        ViewState["rolesaved"] = null;
//        if (ddlBasicRoles.SelectedValue != "")
//        {
//            lnkRoleGroupSave_Click(null, null);

//            if (ViewState["rolesaved"] != null)
//            {
//                hlResetDashBoard.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
//Cryptography.Encrypt("Do you want to reset dashboard of all users of the role " + ddlBasicRoles.SelectedItem.Text + "?")
//+ "&okbutton=" + Cryptography.Encrypt(btnResetDashBoard.ClientID);
//                ScriptManager.RegisterStartupScript(this, this.GetType(), "jsOpenDashResetConfirm", "setTimeout(function () { OpenDashResetConfirm(); }, 1000);", true);

//            }


//        }
//        else
//        {
//            Session["tdbmsgpb"] = "Please select a role.";
//        }

//    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            UserRole theUserRole = SecurityManager.GetUserRole((int)_iUserID, int.Parse(Session["AccountID"].ToString()));


            //foreach (UserRole tempUserRole in _lstUserRole)
            //{
            if (theUserRole.AccountID.ToString() == Session["AccountID"].ToString())
                {
                    if (theUserRole.IsPrimaryAccount != null && (bool)theUserRole.IsPrimaryAccount)
                    {
                        //
                    }
                    else
                    {
                        //not primary account

                        Common.ExecuteText("DELETE UserRole WHERE UserID=" + _iUserID.ToString() + " AND AccountID=" + Session["AccountID"].ToString() + " AND IsPrimaryAccount=0");


//                        Common.ExecuteText(@"DELETE TableUser WHERE TableUserID IN 
//                                (SELECT TableUserID FROM TableUser INNER JOIN [Table] 
//                                ON TableUser.TableID=[Table] .TableID
//                                WHERE UserID=" + _iUserID.ToString() + " AND AccountID=" + Session["AccountID"].ToString() + " )");

                     

                        Common.ExecuteText(@"DELETE MonitorScheduleUser WHERE MonitorScheduleUserID IN 
                                (SELECT MonitorScheduleUserID FROM MonitorScheduleUser INNER JOIN MonitorSchedule 
                                ON MonitorScheduleUser.MonitorScheduleID=MonitorSchedule.MonitorScheduleID
                                WHERE UserID=" + _iUserID.ToString() + " AND AccountID=" + Session["AccountID"].ToString() + " )");

                        Session["tdbmsg"] =txtEmail.Text + " user has been removed from this account.";
                        Response.Redirect(hlBack.NavigateUrl, false);

                        return;

                    }
                }
            //}
            Session["tdbmsg"] = txtEmail.Text + " user has been deactivated.";
            SecurityManager.User_Delete((int)_iUserID);
            //Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/List.aspx", false);
            Response.Redirect(hlBack.NavigateUrl, false);

        }
        catch (Exception ex)
        {
            if (ex is SqlException)
            {
                lblMsg.Text = "Delete failed! Please try again.";
            }
            else
            {

                lblMsg.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alart Messagee.", "alert('" + lblMsg.Text + "');", true);
                lblMsg.Text = "";
            }
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

    protected void lnkUnDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            if (!SecurityManager.CanThisAccountAddUser(int.Parse(Session["AccountID"].ToString())))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('You have reached the maximum number of users allowed for your account type.  In order to add a new user you must either delete an existing user or upgrade your account. See My Account page for options.');", true);
                return;
            }



            SecurityManager.User_UnDelete((int)_iUserID);
            //Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/List.aspx", false);

            Session["tdbmsg"] = txtEmail.Text + " user has been activated.";

            Response.Redirect(hlBack.NavigateUrl, false);

        }
        catch (Exception ex)
        {
            if (ex is SqlException)
            {
                if (ex.Message.IndexOf("UQ_UserEmail") > -1)
                {
                    lblMsg.Text = "A User Email '" + txtEmail.Text.Trim() + "' already exists!";
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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alart Messagee.", "alert('" + lblMsg.Text + "');", true);
            lblMsg.Text = "";
        }


    }

    //protected void chkAdvancedSecurity_CheckedChanged(object sender, EventArgs e)
    //{

    //    if (chkAdvancedSecurity.Checked)
    //    {
    //        divUserTable.Visible = true;
    //        divBasicRoles.Visible = false;
    //        BindUserTableGrid();
    //    }
    //    else
    //    {
    //        divUserTable.Visible = false;
    //        divBasicRoles.Visible = true;
    //    }
    //}




    protected void chkDocAdvancedSec_CheckedChanged(object sender, EventArgs e)
    {

        if (chkDocAdvancedSec.Checked)
        {
            divDocAdvancedSec.Visible = true;
            divBasicDocSec.Visible = false;
            BindUserFolderGrid();
        }
        else
        {
            divDocAdvancedSec.Visible = false;
            divBasicDocSec.Visible = true;
        }
    }

    protected void BindTheRole()
    {
        if (ddlBasicRoles.SelectedValue != "")
        {
            Role theRole = SecurityManager.Role_Details(int.Parse(ddlBasicRoles.SelectedValue));
            if (theRole.DashboardDefaultFromUserID != null)
            {
                if (ddlDashboard.Items.FindByValue(theRole.DashboardDefaultFromUserID.ToString()) != null)
                    ddlDashboard.SelectedValue = theRole.DashboardDefaultFromUserID.ToString();
            }

            chkRoleEditDashboard.Checked = false;
            if (theRole.AllowEditDashboard != null && (bool)theRole.AllowEditDashboard)
            {
                chkRoleEditDashboard.Checked = true;
            }


            if (theRole.ViewsDefaultFromUserID != null)
            {
                if (ddlRole_ViewsDefaultFromUserID.Items.FindByValue(theRole.ViewsDefaultFromUserID.ToString()) != null)
                    ddlRole_ViewsDefaultFromUserID.SelectedValue = theRole.ViewsDefaultFromUserID.ToString();
            }

            chkRole_AllowEditView.Checked = false;
            if (theRole.AllowEditView != null && (bool)theRole.AllowEditView)
            {
                chkRole_AllowEditView.Checked = true;
            }
        }
    }

    protected void BindUserTableGrid()
    {
        int iTN = 0;


        BindTheRole();

            gvUserTable.DataSource = RecordManager.ets_Table_Select(null, "", null, int.Parse(Session["AccountID"].ToString()),
                   null, null, true, "TableName", "ASC", null, null, ref iTN, Session["STs"].ToString());
            gvUserTable.DataBind();
        


    }

    protected void BindUserFolderGrid()
    {
        DataTable dtFolder;

        if (hfParentFolderID.Value == "-1")
        {
            dtFolder = Common.DataTableFromText(@"SELECT     Folder.FolderID, Folder.AccountID, Folder.ParentFolderID, Folder.FolderName
                FROM         Folder
                WHERE Folder.AccountID="+Session["AccountID"].ToString()+@" AND Folder.ParentFolderID IS  NULL
                                      ");

            DataRow drRoot=dtFolder.NewRow();
            
            dtFolder.Rows.InsertAt(drRoot,0);

             dtFolder.Rows[0][0]=-1;
            dtFolder.Rows[0][1]=int.Parse(Session["AccountID"].ToString());
            dtFolder.Rows[0][2]=-1;
            dtFolder.Rows[0][3]="Root";
            //dtFolder.Rows.Add(-1, int.Parse(Session["AccountID"].ToString()), -1, "Root");
            dtFolder.AcceptChanges();
        }
        else
        {
            dtFolder = Common.DataTableFromText(@"SELECT     Folder.FolderID, Folder.AccountID, Folder.ParentFolderID, Folder.FolderName
                FROM         Folder
                WHERE Folder.ParentFolderID =" + hfParentFolderID.Value.ToString());
        }

        gvDocAdvancedSec.DataSource = dtFolder;
        gvDocAdvancedSec.DataBind();

    }

    protected void GoToFolder(object sender, EventArgs e)
    {
        LinkButton lnkFolderName = sender as LinkButton;

        if (lnkFolderName != null)
        {
            //lblCurrentFolder.Text = lnkFolderName.Text;



            GridViewRow row = lnkFolderName.NamingContainer as GridViewRow;
            Label lblID = row.FindControl("LblID") as Label;
            hfParentFolderID.Value = lblID.Text;

            MakeFolderPath();

            BindUserFolderGrid();
        }

    }

    protected void MakeFolderPath()
    {
        string strFolder = "";
        GetFolderPath(int.Parse(hfParentFolderID.Value), ref strFolder);

        strFolder = "<a href='javascript:SetFolder(-1)'>Home</a>/" + strFolder;
        lblCurrentFolder.Text = strFolder;
    }

    protected void GetFolderPath(int iFolderID, ref string strFolder)
    {
        Folder theFolder = DocumentManager.ets_Folder_Detail(iFolderID);
        if (theFolder != null)
        {
            strFolder = "<a href='javascript:SetFolder(" + iFolderID.ToString() + ")'>" + theFolder.FolderName + "</a>/" + strFolder;
            if (theFolder.ParentFolderID != null)
            {
                GetFolderPath((int)theFolder.ParentFolderID, ref strFolder);
            }
        }

    }

    protected void gvUserTable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ddlBasicRoles.SelectedValue == "")
            return;

        if (e.Row.RowType == DataControlRowType.DataRow )
        {

            Label lblTableID = (Label)e.Row.FindControl("lblTableID");
            CheckBox chkCaExport = (CheckBox)e.Row.FindControl("chkCaExport");
            CheckBox chkAllowEditView = (CheckBox)e.Row.FindControl("chkAllowEditView");
            CheckBox chkShowMenu = (CheckBox)e.Row.FindControl("chkShowMenu");


            DropDownList ddlViewsDefaultFromUserID = (DropDownList)e.Row.FindControl("ddlViewsDefaultFromUserID");
            DataTable dtDashboard = ViewUserDT();
            if (dtDashboard.Rows.Count > 1)
            {
                ddlViewsDefaultFromUserID.DataSource = dtDashboard;
                ddlViewsDefaultFromUserID.DataBind();
            }

            ListItem liSelect = new ListItem("--Please Select--", "");
            ddlViewsDefaultFromUserID.Items.Insert(0, liSelect);

            DropDownList ddlRecordRightID = (DropDownList)e.Row.FindControl("ddlRecordRightID");
            DataTable dtUserTable =null;

            if (_qsMode == "add")
                _iUserID = -1;

            dtUserTable = SecurityManager.dbg_RoleTable_Select(null,
            int.Parse(lblTableID.Text.Trim()),int.Parse(ddlBasicRoles.SelectedValue), null);
                    

            if (dtUserTable.Rows.Count > 0)
            {
               
                ddlRecordRightID.Text = dtUserTable.Rows[0]["RoleType"].ToString();

                if (chkCaExport != null && dtUserTable.Rows[0]["CanExport"] != DBNull.Value)
                {
                    if (bool.Parse(dtUserTable.Rows[0]["CanExport"].ToString()))
                    {
                        chkCaExport.Checked = true;
                    }
                    else
                    {
                        chkCaExport.Checked = false;
                    }
                }


                if (dtUserTable.Rows[0]["ViewsDefaultFromUserID"] != DBNull.Value)
                {
                    if (ddlViewsDefaultFromUserID.Items.FindByValue(dtUserTable.Rows[0]["ViewsDefaultFromUserID"].ToString()) != null)
                        ddlViewsDefaultFromUserID.SelectedValue = dtUserTable.Rows[0]["ViewsDefaultFromUserID"].ToString();
                }

                if (chkAllowEditView != null && dtUserTable.Rows[0]["AllowEditView"] != DBNull.Value)
                {
                    if (bool.Parse(dtUserTable.Rows[0]["AllowEditView"].ToString()))
                    {
                        chkAllowEditView.Checked = true;
                    }
                    else
                    {
                        chkAllowEditView.Checked = false;
                    }
                }

                if (chkShowMenu != null && dtUserTable.Rows[0]["ShowMenu"] != DBNull.Value)
                 {
                     chkShowMenu.Checked = bool.Parse(dtUserTable.Rows[0]["ShowMenu"].ToString());
                 }

            }

            if (_qsMode == "view")
            {
                
                ddlRecordRightID.Enabled = false;
                ddlViewsDefaultFromUserID.Enabled = false;
                chkCaExport.Enabled = false;
                chkAllowEditView.Enabled = false;
                chkShowMenu.Enabled = false;
            }


        }

    }




    protected void gvDocAdvancedSec_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblFolderID = (Label)e.Row.FindControl("LblID");
            Label lblFolderName = (Label)e.Row.FindControl("lblFolderName");

            DropDownList ddlBasicDocSecEach = (DropDownList)e.Row.FindControl("ddlBasicDocSecEach");
            string strRight="";

            int iUserID = -1;

            if (_iUserID != null)
                iUserID = (int)_iUserID;
            if (lblFolderID.Text != "-1")
            {
                strRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE UserID=" + iUserID.ToString() + " AND FolderID=" + lblFolderID.Text);
            }
            else
            {
                strRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE UserID=" + iUserID.ToString() + " AND FolderID  IS NULL ");

            }

            if (strRight != "")
            {
                ddlBasicDocSecEach.Text = strRight;
            }
            else
            {
                FindParentRight(ref strRight, int.Parse(lblFolderID.Text),iUserID);

                if (strRight == "")
                {
                    strRight = "full";
                }
                ddlBasicDocSecEach.Text = strRight;
            }

            if (_strActionMode.ToLower() == "view")
            {
                ddlBasicDocSecEach.Enabled = false;
            }

            if (hfParentFolderID.Value == "-1" && lblFolderID.Text == "-1")
            {
                //LinkButton lnkFolderName = (LinkButton)e.Row.FindControl("lnkFolderName");
                Image imgFolderIcon = (Image)e.Row.FindControl("imgFolderIcon");

                if (lblFolderName.Text.ToUpper() == "ROOT")
                {
                    //lnkFolderName.CssClass = "headerlink";
                    //lnkFolderName.Style.Add("text-decoration", "none");
                    //lnkFolderName.Style.Add("color", "Black");
                    //lnkFolderName.Style.Add("cursor", "default");
                    imgFolderIcon.Visible = false;
                }               

            }

        }

    }


    protected void FindParentRight(ref string strRight,  int iFolderID,int iUserID)
    {
        Folder theFolder = DocumentManager.ets_Folder_Detail(iFolderID);
        if (theFolder != null)
        {
            if (strRight == "")
            {
                if (theFolder.ParentFolderID != null)
                {
                    Folder theFolderP = DocumentManager.ets_Folder_Detail((int)theFolder.ParentFolderID);
                    strRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE UserID=" + iUserID.ToString() + " AND FolderID=" + theFolderP.FolderID.ToString());

                    if (strRight == "")
                    {
                        FindParentRight(ref strRight, (int)theFolder.ParentFolderID, iUserID);
                    }
                }

            }


        }
    }




//    protected void PopulateTableDropDown()
//    {

//        ddlTable.Items.Clear();

//        //ddlTable.DataSource = SecurityManager.ets_User_ByAccount(int.Parse(Session["AccountID"].ToString()));

//        ddlTable.DataSource = Common.DataTableFromText(@"SELECT TableName,TableID FROM [Table]
//                WHERE IsActive=1 and AccountID=" + Session["AccountID"].ToString() + @"
//                AND TableID NOT IN (SELECT  DISTINCT [Table].TableID
//                FROM     [Table] INNER JOIN
//                  TableUser ON [Table].TableID = TableUser.TableID
//                  WHERE [Table].AccountID=" + Session["AccountID"].ToString() + @" AND TableUser.UserID=" + _iUserID.ToString() + @"
//                  UNION SELECT -1) ORDER BY TableName");


//        ddlTable.DataBind();

//        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "-1");
//        ddlTable.Items.Insert(0, liSelect);
//    }


    protected void PopulateFieldsByTable(int iTableID)
    {
        ddlScopeField.Items.Clear();

        ddlScopeField.DataSource = Common.DataTableFromText("SELECT DisplayName, ColumnID FROM [Column] WHERE IsStandard=0 AND TableID=" + iTableID.ToString());
        ddlScopeField.DataBind();

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
        ddlScopeField.Items.Insert(0, liSelect);
    }

    

    //protected void PopulateRoleGroup()
    //{
    //    ddlRoleGroup.Items.Clear();

    //    ddlRoleGroup.DataSource = Common.DataTableFromText("SELECT RoleGroupID,RoleGroupName FROM RoleGroup WHERE AccountID=" 
    //        + Session["AccountID"].ToString() + " ORDER BY RoleGroupName");
    //    ddlRoleGroup.DataBind();

    //    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
    //    ddlRoleGroup.Items.Insert(0, liSelect);
    //}


    protected void PopulateRole()
    {
        ddlBasicRoles.Items.Clear();

        string strExtraWhere = "";

        if (_strActionMode == "add" && (_CurrentUserRole.IsAccountHolder == null || (bool)_CurrentUserRole.IsAccountHolder == false))
        {
            strExtraWhere = " AND RoleType<>'2' ";
        }

        ddlBasicRoles.DataSource = Common.DataTableFromText("SELECT * FROM [Role] WHERE (IsActive IS NULL OR IsActive=1) AND AccountID="
            + Session["AccountID"].ToString() + strExtraWhere +  " ORDER BY [Role]");
        ddlBasicRoles.DataBind();

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddlBasicRoles.Items.Insert(0, liSelect);

        

    }

    protected void PopulateValuesByColumn(int iColumnID)
    {
        ddlScopeValue.Items.Clear();

        Column theColumn=RecordManager.ets_Column_Details(iColumnID);
        if(theColumn!=null)
        {
            
            if(theColumn.TableTableID!=null && theColumn.LinkedParentColumnID!=null)
            {
                RecordManager.PopulateTableDropDown((int)theColumn.ColumnID, ref ddlScopeValue);

            }
            else
            {
                DataTable dtTemp = Common.DataTableFromText("SELECT DISTINCT " + theColumn.SystemName + " FROM Record WHERE IsActive=1 AND TableID=" + theColumn.TableID.ToString());
                foreach (DataRow dr in dtTemp.Rows)
                {
                    System.Web.UI.WebControls.ListItem liOne = new System.Web.UI.WebControls.ListItem(dr[0].ToString(), dr[0].ToString());
                    ddlScopeValue.Items.Add(liOne);
                }

                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
                ddlScopeValue.Items.Insert(0, liSelect);
            }                      

        }
        
        //DataTable dtTemp=
        
       
    }

    //protected void grdTableUser_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "deletetype")
    //    {
    //        try
    //        {
    //            lblMsgTab.Text = "";

    //            RecordManager.ets_TableUser_Delete(Convert.ToInt32(e.CommandArgument));
    //            PopulateTableDropDown();
    //            PopulateSTUserGrid();

    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog theErrorLog = new ErrorLog(null, "Linked Account Delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //            SystemData.ErrorLog_Insert(theErrorLog);
    //            lblMsgTab.Text = ex.Message;

    //            //ScriptManager.RegisterClientScriptBlock(grdTable, typeof(Page), "msg_delete", "alert('Delete failed!');", true);
    //        }
    //    }
    //}



    //protected void grdLinkedUser_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "deletetype")
    //    {
    //        try
    //        {
    //            lblMsgTab.Text = "";
    //            //SiteManager.ets_LocationTable_Delete(Convert.ToInt32(e.CommandArgument));

    //            //SecurityManager.ets_LinkedUser_Delete(Convert.ToInt32(e.CommandArgument));
    //            PopulateLinkedUser();


    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog theErrorLog = new ErrorLog(null, "Table User", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //            SystemData.ErrorLog_Insert(theErrorLog);
    //            lblMsgTab.Text = ex.Message;

    //            //ScriptManager.RegisterClientScriptBlock(grdTable, typeof(Page), "msg_delete", "alert('Delete failed!');", true);
    //        }
    //    }
    //}


//    protected void PopulateLinkedUser()
//    {
//        grdLinkedUser.DataSource = Common.DataTableFromText(@"SELECT     LinkedUser.LinkedUserID, Account.AccountName, LinkedUser.UserID, LinkedUser.AccountID
//                                            FROM         Account INNER JOIN
//                                            LinkedUser ON Account.AccountID = LinkedUser.AccountID
//                                            WHERE  LinkedUser.UserID=" + _iUserID);

//        grdLinkedUser.DataBind();
//    }

    protected void btnFolderSaved_Click(object sender, EventArgs e)
    {
        BindUserFolderGrid();
        MakeFolderPath();
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

                //if (_iUserID.ToString() != "")
                //{
                //    grdTableUser.DataSource = RecordManager.ets_TableUser_Select(null, null, _iUserID, null, null, null, null, null, null, null, null);
                //    grdTableUser.DataBind();
                //}
                break;

            case "view":
                //if (_iUserID.ToString() != "")
                //{
                //    grdTableUser.DataSource = RecordManager.ets_TableUser_Select(null, null, _iUserID, null, null, null, null, null, null, null, null);
                //    grdTableUser.DataBind();

                //    grdTableUser.Columns[1].Visible = false;
                //    grdTableUser.Enabled = false;

                //}
                break;
        }



        //AddHeaderForTableUserGridView();


    }

    protected void grdLinkedUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ib = (ImageButton)e.Row.FindControl("imgbtnDelete");
            ib.Attributes.Add("onclick", "javascript:return " +
            "confirm('Are you sure you want to remove this account --" +
            DataBinder.Eval(e.Row.DataItem, "AccountName") + "?');");

        }
    }

    //protected void grdTableUser_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {

    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            ImageButton ib = (ImageButton)e.Row.FindControl("imgbtnDelete");
    //            ib.Attributes.Add("onclick", "javascript:return " +
    //            "confirm('Are you sure you want to remove this " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " --" +
    //            DataBinder.Eval(e.Row.DataItem, "TableName") + "?');");


    //            DropDownList ddlUploadOption = (DropDownList)e.Row.FindControl("ddlUploadOption");
    //            DropDownList ddlUploadWarningOption = (DropDownList)e.Row.FindControl("ddlUploadWarningOption");
    //            DropDownList ddlLateWarningOption = (DropDownList)e.Row.FindControl("ddlLateWarningOption");

    //            TableUser theTableUser = RecordManager.ets_TableUser_Detail(int.Parse(DataBinder.Eval(e.Row.DataItem, "TableUserID").ToString()));
    //            if (theTableUser != null)
    //            {
    //                if ((bool)theTableUser.UploadEmail && (bool)theTableUser.UploadSMS)
    //                {
    //                    ddlUploadOption.Text = "both";
    //                }
    //                else
    //                {
    //                    if ((bool)theTableUser.UploadEmail == false && (bool)theTableUser.UploadSMS == false)
    //                    {
    //                        ddlUploadOption.Text = "none";
    //                    }
    //                    else
    //                    {
    //                        if ((bool)theTableUser.UploadEmail)
    //                        {
    //                            ddlUploadOption.Text = "email";
    //                        }
    //                        else
    //                        {
    //                            ddlUploadOption.Text = "sms";
    //                        }

    //                    }

    //                }

    //                ///

    //                if ((bool)theTableUser.UploadWarningEmail && (bool)theTableUser.UploadWarningSMS)
    //                {
    //                    ddlUploadWarningOption.Text = "both";
    //                }
    //                else
    //                {
    //                    if ((bool)theTableUser.UploadWarningEmail == false && (bool)theTableUser.UploadWarningSMS == false)
    //                    {
    //                        ddlUploadWarningOption.Text = "none";
    //                    }
    //                    else
    //                    {
    //                        if ((bool)theTableUser.UploadWarningEmail)
    //                        {
    //                            ddlUploadWarningOption.Text = "email";
    //                        }
    //                        else
    //                        {
    //                            ddlUploadWarningOption.Text = "sms";
    //                        }

    //                    }

    //                }
    //                //

    //                if ((bool)theTableUser.LateWarningEmail && (bool)theTableUser.LateWarningSMS)
    //                {
    //                    ddlLateWarningOption.Text = "both";
    //                }
    //                else
    //                {
    //                    if ((bool)theTableUser.LateWarningEmail == false && (bool)theTableUser.LateWarningSMS == false)
    //                    {
    //                        ddlLateWarningOption.Text = "none";
    //                    }
    //                    else
    //                    {
    //                        if ((bool)theTableUser.LateWarningEmail)
    //                        {
    //                            ddlLateWarningOption.Text = "email";
    //                        }
    //                        else
    //                        {
    //                            ddlLateWarningOption.Text = "sms";
    //                        }

    //                    }

    //                }

    //            }






    //        }

    //        if (e.Row.RowType == DataControlRowType.Footer)
    //        {


    //            HyperLink hlUploadEmail = (HyperLink)e.Row.FindControl("hlUploadEmail");
    //            Content xContent = SystemData.Content_Details_ByKey("DataUploadEmail",int.Parse(Session["AccountID"].ToString()));
    //            if (xContent != null)
    //                hlUploadEmail.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(xContent.ContentID.ToString());

    //            HyperLink hlUploadSMS = (HyperLink)e.Row.FindControl("hlUploadSMS");
    //            Content yContent = SystemData.Content_Details_ByKey("DataUploadSMS",int.Parse( Session["AccountID"].ToString()));

    //            if (yContent != null)
    //                hlUploadSMS.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(yContent.ContentID.ToString());






    //            HyperLink hlLateWarningEmail = (HyperLink)e.Row.FindControl("hlLateWarningEmail");
    //            Content cContent = SystemData.Content_Details_ByKey("LateWarningEmail",int.Parse(Session["AccountID"].ToString()));

    //            if (cContent != null)
    //                hlLateWarningEmail.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(cContent.ContentID.ToString());


    //            HyperLink hlLateWarningSMS = (HyperLink)e.Row.FindControl("hlLateWarningSMS");
    //            Content dContent = SystemData.Content_Details_ByKey("LateWarningSMS",int.Parse(Session["AccountID"].ToString()));

    //            if (dContent != null)
    //                hlLateWarningSMS.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(dContent.ContentID.ToString());


    //            HyperLink hlUploadWarningEmail = (HyperLink)e.Row.FindControl("hlUploadWarningEmail");
    //            Content pContent = SystemData.Content_Details_ByKey("DataUploadWarningEmail",int.Parse(Session["AccountID"].ToString()));
    //            if (pContent != null)
    //                hlUploadWarningEmail.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(pContent.ContentID.ToString());

    //            HyperLink hlUploadWarningSMS = (HyperLink)e.Row.FindControl("hlUploadWarningSMS");
    //            Content qContent = SystemData.Content_Details_ByKey("DataUploadWarningSMS",int.Parse(Session["AccountID"].ToString()));

    //            if (qContent != null)
    //                hlUploadWarningSMS.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&global=" + Cryptography.Encrypt("false") + "&ContentID=" + Cryptography.Encrypt(qContent.ContentID.ToString());



    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //
    //    }





    //}



    //protected void lnkSmallSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblMsgTab.Text = "";

    //        if (ddlTable.SelectedValue == "-1")
    //        {
    //            lblMsgTab.Text = "Please select a " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + "!";
    //            return;
    //        }


    //        TableUser newTableUser;
    //        switch (_strActionMode.ToLower())
    //        {
    //            case "add":

    //                //newLocationTable = new LocationTable(null,
    //                //    int.Parse(ViewState["iNewLocationID"].ToString()),
    //                //    int.Parse(ddlTable.SelectedValue), "", "");
    //                //SiteManager.ets_LocationTable_Insert(newLocationTable);


    //                break;

    //            case "edit":

    //                newTableUser = new TableUser(null, int.Parse(ddlTable.SelectedValue),
    //                    _iUserID, true, false, true, false, true, false, null, null);
    //                RecordManager.ets_TableUser_Insert(newTableUser);

    //                //PopulateTableDropDown();

    //                break;
    //        }


    //        PopulateSTUserGrid();

    //    }
    //    catch (Exception ex)
    //    {

    //        if (ex.Message.IndexOf("UQ_SampleTypeUser") > -1)
    //        {
    //            lblMsgTab.Text = "This user is already added! ";
    //        }
    //        else
    //        {
    //            lblMsgTab.Text = ex.Message;
    //        }
    //    }
    //}


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




    //protected void UpdateSupplyLed(object sender, EventArgs e)
    //{
    //    CheckBox chkBx = sender as CheckBox;

    //    if (chkBx != null)
    //    {
    //        GridViewRow row = chkBx.NamingContainer as GridViewRow;
    //        Label lblID = row.FindControl("lblID") as Label;

    //        TableUser theTableUser = RecordManager.ets_TableUser_Detail(int.Parse(lblID.Text));
    //        switch (chkBx.ID)
    //        {
    //            case "chkUploadEmail":
    //                theTableUser.UploadEmail = chkBx.Checked;
    //                break;

    //            case "chkUploadSMS":
    //                theTableUser.UploadSMS = chkBx.Checked;
    //                break;

    //            case "chkWarningEmail":
    //                theTableUser.WarningEmail = chkBx.Checked;
    //                break;

    //            case "chkWarningSMS":
    //                theTableUser.WarningSMS = chkBx.Checked;
    //                break;

    //            case "chkLateWarningEmail":
    //                theTableUser.LateWarningEmail = chkBx.Checked;
    //                break;

    //            case "chkLateWarningSMS":
    //                theTableUser.LateWarningSMS = chkBx.Checked;
    //                break;

    //            case "chkUploadWarningEmail":
    //                theTableUser.UploadWarningEmail = chkBx.Checked;
    //                break;

    //            case "chkUploadWarningSMS":
    //                theTableUser.UploadWarningSMS = chkBx.Checked;
    //                break;

    //        }

    //        RecordManager.ets_TableUser_Update(theTableUser);

    //        //update database logic here.
    //    }


    //}

    protected void btnRefreshLinkedUser_Click(object sender, EventArgs e)
    {
        //PopulateLinkedUser();
        //BindTheGrid(_iStartIndex, _iMaxRows);
        Response.Redirect("~/Pages/User/Detail.aspx?mode="+Cryptography.Encrypt("edit")+"&SearchCriteria=" + Cryptography.Encrypt("-1") + "&userid=" + Request.QueryString["userid"].ToString(), false);
    }
    protected void ddlScopeTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlScopeTable.SelectedValue == "")
        {
            ddlScopeField.Items.Clear();
            ddlScopeValue.Items.Clear();
        }
        else
        {
            PopulateFieldsByTable(int.Parse(ddlScopeTable.SelectedValue));
        }
    }


    protected void ddlBasicRoles_SelectedIndexChanged(object sender, EventArgs e)
    {

        lnkRoleGroupSave.Visible = false;
        lnkRoleGroupDelete.Visible = false;
        divUserTable.Visible = false;
        trViewAllTable.Visible = false;
        divRoleSpecialRights.Visible = false;
        ShowHideControlByRole();
    }

    protected void ShowHideControlByRole()
    {
        bool bAdvanced = true;
        if (ddlBasicRoles.SelectedValue == "")
        {

            lnkRoleEdit.Visible = false;
            divUserRole.Visible = false;
        }
        else
        {
            lnkRoleEdit.Visible = true;
            Role theRole = SecurityManager.Role_Details(int.Parse(ddlBasicRoles.SelectedValue));

            if (theRole != null && theRole.RoleType=="2")
            {
                divUserRole.Visible = true;
                if (_CurrentUserRole.IsAccountHolder != null && (bool)_CurrentUserRole.IsAccountHolder)
                {
                    chkAllowDeleteTable.Enabled = true;
                    chkAllowDeleteColumn.Enabled = true;
                    chkAllowDeleteRecord.Enabled = true;
                }
                else
                {
                    chkAllowDeleteTable.Enabled = false;
                    chkAllowDeleteColumn.Enabled = false;
                    chkAllowDeleteRecord.Enabled = false;
                }
            }
            else
            {
                divUserRole.Visible = false;
            }

            if (theRole.IsSystemRole == null)
            {
                bAdvanced = false;
            }
            else
            {
                if ((bool)theRole.IsSystemRole)
                {
                    bAdvanced = false;
                }
            }

            if (bAdvanced)
            {
                trViewAllTable.Visible = false;

                if (_bShowTableGrid)
                {
                    divUserTable.Visible = true;
                    BindUserTableGrid();
                }
               
            }
            else
            {

                divUserTable.Visible = false;
                trViewAllTable.Visible = true;
            }

            if (theRole.RoleType == "5" || theRole.RoleType == "6")//readonly and none
            {
                chkRoleEditDashboard.Checked = false;
                chkRoleEditDashboard.Enabled = false;
                chkRole_AllowEditView.Checked = false;
                chkRole_AllowEditView.Enabled = false;

            }
            else
            {
                chkRoleEditDashboard.Enabled = true;
                chkRole_AllowEditView.Enabled = true;
               
            }

        }
        UserRole theUserRole = SecurityManager.GetUserRole((int)_iUserID, int.Parse(Session["AccountID"].ToString()));
        if (theUserRole!=null && (bool)theUserRole.IsAccountHolder)
        {
            chkAllowDeleteTable.Checked = true;
            chkAllowDeleteColumn.Checked = true;
            chkAllowDeleteRecord.Checked = true;

            chkAllowDeleteTable.Enabled = false;
            chkAllowDeleteColumn.Enabled = false;
            chkAllowDeleteRecord.Enabled = false;
        }

    }
    //protected void ddlRoleGroup_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    BindUserTableGrid();

    //    if(ddlRoleGroup.SelectedValue=="")
    //    {
    //        lnkRoleGroupSave.Visible = false;
    //        lnkRoleGroupDelete.Visible = false;
    //    }
    //    else
    //    {
    //        lnkRoleGroupSave.Visible = true;
    //        lnkRoleGroupDelete.Visible = true;

    //    }
    //}

    protected void ddlScopeField_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlScopeField.SelectedValue == "")
        {           
            ddlScopeValue.Items.Clear();
        }
        else
        {
            PopulateValuesByColumn(int.Parse(ddlScopeField.SelectedValue));
        }
    }
}
