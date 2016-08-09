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
public partial class Pages_Record_MenuDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iMenuID;
    string _qsMode = "";
    string _qsMenuID = "";
    //Common_Pager _gvPager;

    protected void optMenuType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (optMenuType.SelectedValue == "d")
        {
            trMenu.Visible = false;
            trCustomPageLink.Visible = false;
            //txtMenu.Text = Common.MenuDividerText;
        }
        else if (optMenuType.SelectedValue == "c")
        {
            if (txtMenu.Text == Common.MenuDividerText) 
            { 
                txtMenu.Text = ""; 
            }
            trMenu.Visible = true;
            trCustomPageLink.Visible = true;
        }
        else
        {
            trMenu.Visible = true;
            if (txtMenu.Text == Common.MenuDividerText)
            {
                txtMenu.Text = "";
            }

            trCustomPageLink.Visible = false;            
        }     

    }
    protected void Page_Load(object sender, EventArgs e)
    {

        int iTemp = 0;
        // checking action mode

        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);
            if (Request.QueryString["MenuID"] != null)
            {
                _qsMenuID = Cryptography.Decrypt(Request.QueryString["MenuID"]);
                _iMenuID = int.Parse(_qsMenuID);

            }

            if (!IsPostBack)
            {

                PopulateMenuDDL(ref ddlShowUnder);
                


                if (Request.QueryString["default"] != null)
                {
                    if (ddlShowUnder.Items.FindByValue(Request.QueryString["default"].ToString()) != null)
                        ddlShowUnder.SelectedValue = Request.QueryString["default"].ToString();
                }
            }
               

                if (_qsMode == "add" ||
                    _qsMode == "view" ||
                    _qsMode == "edit")
                {
                    _strActionMode = _qsMode;

                if (Request.QueryString["MenuID"] != null)
                {
                  

                    

                    if (!IsPostBack)
                    {

                        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                        { Response.Redirect("~/Default.aspx", false); }
                       

                            //    if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                            //    {
                            //        gvTheGrid.PageSize = int.Parse(Session["GridPageSize"].ToString());

                            //    }

                            //    gvTheGrid.GridViewSortColumn = "TableName";
                            //    BindTheGrid(0, gvTheGrid.PageSize);
                    }
                            //GridViewRow gvr = gvTheGrid.TopPagerRow;
                            //if (gvr != null)
                            //    _gvPager = (Common_Pager)gvr.FindControl("Pager");
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
        

        string strTitle = "Menu Detail";

        // checking permission


        if (!IsPostBack)
        {
           
            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx";
            }
        }

        switch (_strActionMode.ToLower())
        {
            case "add":
                divDelete.Visible = false;
                divUnDelete.Visible = false;
                strTitle = "Add Menu";
                break;

            case "view":


                strTitle = "View Menu";
                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Menu";

                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }

        if (!IsPostBack)
        {
            PopulateTerminology();
        }

        Title = strTitle;
        lblTitle.Text = strTitle;

    }

    protected void PopulateTerminology()
    {
        //gvTheGrid.Columns[1].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), gvTheGrid.Columns[1].HeaderText, gvTheGrid.Columns[1].HeaderText);
    }

//    protected void PopulateSubMenu(ref DropDownList ddlShowUnder, int iParentMenuID,int iLD)
//    {
//        DataTable dtSubMenu = Common.DataTableFromText(@"SELECT MenuID,Menu FROM Menu WHERE IsActive=1 AND 
//                AccountID=" + Session["AccountID"].ToString() + @" AND ParentMenuID=" + iParentMenuID.ToString()
//                            + @"  AND (TableID IS NULL AND DocumentID IS NULL AND CustomPageLink IS NULL AND Menu<>'---')  ORDER BY DisplayOrder");

//        iLD = iLD + 1;
//        string strLD = "";
//        for (int i = 1; i <= iLD; i++)
//        {
//            strLD = strLD + "-";
//        }

//        foreach (DataRow drSubMenu in dtSubMenu.Rows)
//        {
//            ListItem liItem = new ListItem(strLD + drSubMenu["Menu"].ToString(), drSubMenu["MenuID"].ToString());
//            ddlShowUnder.Items.Add(liItem);
//            PopulateSubMenu(ref ddlShowUnder, int.Parse(drSubMenu["MenuID"].ToString()), iLD);
//        }
//    }

    protected void PopulateMenuDDL(ref DropDownList ddlShowUnder)
    {


        TheDatabaseS.PopulateMenuDDL(ref ddlShowUnder);
       
        ListItem liTop = new ListItem("-- Top Level --", "");
        ddlShowUnder.Items.Insert(0,liTop);
    }



//    protected void PopulateMenuDDL(ref DropDownList ddlShowUnder)
//    {



//        ddlShowUnder.Items.Clear();

//        DataTable dtTopLevel = Common.DataTableFromText(@"SELECT MenuID,Menu FROM Menu WHERE IsActive=1 AND 
//                AccountID=" + Session["AccountID"].ToString() +
//                            @" AND ParentMenuID IS  NULL   AND(TableID IS NULL AND DocumentID IS NULL AND CustomPageLink IS NULL AND Menu<>'---') ORDER BY DisplayOrder");

//        foreach (DataRow drTop in dtTopLevel.Rows)
//        {
//            ListItem liItem = new ListItem(drTop["Menu"].ToString(), drTop["MenuID"].ToString());
//            ddlShowUnder.Items.Add(liItem);

//            PopulateSubMenu(ref ddlShowUnder, int.Parse(drTop["MenuID"].ToString()), 0);

//        }

//        ListItem liTop = new ListItem("-- Top Level --", "");
//        ddlShowUnder.Items.Insert(0, liTop);
//    }
    
    protected void PopulateTheRecord()
    {

        Menu theMenu = RecordManager.ets_Menu_Details((int)_iMenuID);

        if (theMenu.ParentMenuID != null)
        {
            if (ddlShowUnder.Items.FindByValue(theMenu.ParentMenuID.ToString()) != null)
                ddlShowUnder.SelectedValue = theMenu.ParentMenuID.ToString();
        }

        txtMenu.Text = theMenu.MenuP;       
        chkShowOnMenu.Checked = (bool)theMenu.ShowOnMenu;

        //txtCustomPageLink.Text = theMenu.CustomPageLink;


        if (theMenu.MenuP == Common.MenuDividerText)
        {
            optMenuType.SelectedValue = "d";
            //optMenuType_SelectedIndexChanged(null, null);
        }
        //else if (!string.IsNullOrEmpty(theMenu.CustomPageLink))
        //{
        //    optMenuType.SelectedValue = "c";            
        //}
        optMenuType_SelectedIndexChanged(null, null);

        //ddlAccount.Text = theMenu.AccountID.ToString();
        if (_strActionMode == "edit")
        {
            ViewState["theMenu"] = theMenu;

            if (theMenu.IsActive == true)
            {
                divUnDelete.Visible = false;
            }
            else
            {
                divDelete.Visible = false;
            }
        }

        else if (_strActionMode == "view")
        {
            divEdit.Visible = true;
            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroupDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&MenuID=" + Cryptography.Encrypt(theMenu.MenuID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {
                hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroupDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&MenuID=" + Cryptography.Encrypt(theMenu.MenuID.ToString());
            }
            divDelete.Visible = false;
            divUnDelete.Visible = false;
        }

        

    }


    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            RecordManager.ets_Menu_Delete((int)_iMenuID);
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
            }
        }


    }

    protected void lnkUnDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            RecordManager.ets_Menu_UnDelete((int)_iMenuID);
            Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            if (ex is SqlException)
            {
                if (ex.Message.IndexOf("UQ_Menu_Name_Parent_AccountID") > -1)
                {
                    lblMsg.Text = "A Menu '" + txtMenu.Text.Trim() + "' already exists.";
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
        }


    }


    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtMenu.Enabled = p_bEnable;
        chkShowOnMenu.Enabled = p_bEnable;
        //ddlAccount.Enabled = p_bEnable;       

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }



    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {

        try
        {
            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        Menu newMenu = new Menu(null, txtMenu.Text,
                            int.Parse(Session["AccountID"].ToString()), chkShowOnMenu.Checked, true);


                        newMenu.ParentMenuID = ddlShowUnder.SelectedValue == "" ? null : (int?)int.Parse(ddlShowUnder.SelectedValue);
                        if (optMenuType.SelectedValue == "d")
                        {
                            newMenu.MenuP = Common.MenuDividerText;
                            //newMenu.CustomPageLink = null;
                        }
                        //if (optMenuType.SelectedValue == "c")
                        //{
                        //    newMenu.CustomPageLink = string.IsNullOrEmpty(txtCustomPageLink.Text) ? null : txtCustomPageLink.Text;
                        //}
                        RecordManager.ets_Menu_Insert(newMenu);

                        break;

                    case "view":


                        break;

                    case "edit":
                        Menu editMenu = (Menu)ViewState["theMenu"];

                        editMenu.MenuP = txtMenu.Text;
                        editMenu.ShowOnMenu = chkShowOnMenu.Checked;
                        editMenu.AccountID = int.Parse(Session["AccountID"].ToString());

                        editMenu.ParentMenuID = ddlShowUnder.SelectedValue == "" ? null : (int?)int.Parse(ddlShowUnder.SelectedValue);
                        if (optMenuType.SelectedValue == "d")
                        {
                            editMenu.MenuP = Common.MenuDividerText;
                            //editMenu.CustomPageLink = null;
                        }
                        //if (optMenuType.SelectedValue == "c")
                        //{
                        //    editMenu.CustomPageLink = string.IsNullOrEmpty(txtCustomPageLink.Text) ? null : txtCustomPageLink.Text;
                        //}
                        //else
                        //{
                        //    editMenu.CustomPageLink = null;
                        //}

                        int iIsUpdated = RecordManager.ets_Menu_Update(editMenu);

                        break;

                    default:
                        //?
                        break;
                }
            }
            else
            {
                //user input is not ok

            }
            //temp
            Response.Redirect(hlBack.NavigateUrl, false);
        }

        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table Group", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            if (ex.Message.IndexOf("UQ_Menu_Name_Parent_AccountID") > -1)
            {
                lblMsg.Text = "A Menu '" + txtMenu.Text + "' is already in same level, please try another Menu name.";
                txtMenu.Focus();
               
            }
            else if (ex.Message.IndexOf("CHK_Menu_NotSelfParent") > -1)
            {
                lblMsg.Text = "A Menu can not be under it's own , please try another Menu name.";
                txtMenu.Focus();
               
            }
            else
            {
                lblMsg.Text = ex.Message;
            }
        }
    }
    //protected void cmdBack_Click(object sender, ImageClickEventArgs e)
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx" , false);
    }


   
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

}



//protected void Pager_BindTheGridAgain(object sender, EventArgs e)
//{
//    BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
//}
//protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
//{
//    BindTheGrid(0, gvTheGrid.PageSize);

//}
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


//protected void gvTheGrid_RowCommand(object sender, GridViewCommandEventArgs e)
//{

//    if (e.CommandName == "uporder")
//    {
//        try
//        {
//            RecordManager.ets_Table_OrderChange(Convert.ToInt32(e.CommandArgument), true);

//            BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);

//        }
//        catch (Exception ex)
//        {
//            ErrorLog theErrorLog = new ErrorLog(null, "Table Order", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
//            SystemData.ErrorLog_Insert(theErrorLog);
//            lblMsg.Text = ex.Message;

//            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Order failed!');", true);
//        }
//    }
//    if (e.CommandName == "downorder")
//    {
//        try
//        {
//            RecordManager.ets_Table_OrderChange(Convert.ToInt32(e.CommandArgument), false);

//            BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);


//        }
//        catch (Exception ex)
//        {
//            ErrorLog theErrorLog = new ErrorLog(null, "Table Order", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
//            SystemData.ErrorLog_Insert(theErrorLog);
//            lblMsg.Text = ex.Message;

//            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Order failed!');", true);
//        }
//    }
//    Response.Redirect(Request.RawUrl, false);
//}


//protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
//{


//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
//        e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");


//    }



//}

//protected void Pager_OnApplyFilter(object sender, EventArgs e)
//{
//    BindTheGrid(0, gvTheGrid.PageSize);
//}

//protected void Pager_OnExportForCSV(object sender, EventArgs e)
//{

//    gvTheGrid.AllowPaging = false;
//    BindTheGrid(0, _gvPager.TotalRows);



//    Response.Clear();
//    Response.Buffer = true;
//    Response.AddHeader("content-disposition",
//    "attachment;filename=Tables.csv");
//    Response.Charset = "";
//    Response.ContentType = "text/csv";

//    StringWriter sw = new StringWriter();
//    HtmlTextWriter hw = new HtmlTextWriter(sw);


//    int iColCount = gvTheGrid.Columns.Count;
//    for (int i = 0; i < iColCount; i++)
//    {
//        if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
//        {
//        }
//        else
//        {
//            sw.Write(gvTheGrid.Columns[i].HeaderText);
//            if (i < iColCount - 1)
//            {
//                sw.Write(",");
//            }
//        }
//    }

//    sw.Write(sw.NewLine);

//    // Now write all the rows.
//    foreach (GridViewRow dr in gvTheGrid.Rows)
//    {

//        for (int i = 0; i < iColCount; i++)
//        {
//            if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
//            {
//            }
//            else
//            {
//                switch (i)
//                {
//                    case 3:
//                        HyperLink hlView = (HyperLink)dr.FindControl("hlView");
//                        sw.Write("\"" + hlView.Text + "\"");
//                        break;

//                    default:
//                        if (!Convert.IsDBNull(dr.Cells[i]))
//                        {
//                            sw.Write("\"" + dr.Cells[i].Text + "\"");
//                        }

//                        break;
//                }

//                if (i < iColCount - 1)
//                {
//                    sw.Write(",");
//                }
//            }
//        }
//        sw.Write(sw.NewLine);
//    }
//    sw.Close();


//    Response.Output.Write(sw.ToString());
//    Response.Flush();
//    Response.End();
//}


//protected void btnOrderMT_Click(object sender, EventArgs e)
//{
//    //
//    if (hfOrderMT.Value != "")
//    {
//        SqlTransaction tn;
//        SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
//        connection.Open();
//        tn = connection.BeginTransaction();

//        try
//        {
//            string strNewMT = hfOrderMT.Value.Substring(0, hfOrderMT.Value.Length - 1);
//            string[] newMT = strNewMT.Split(',');


//            DataTable dtDO = Common.DataTableFromText("SELECT DisplayOrder,TableID FROM [Table] WHERE TableID IN (" + strNewMT + ") ORDER BY DisplayOrder", ref connection, ref tn);
//            if (newMT.Length == dtDO.Rows.Count)
//            {
//                for (int i = 0; i < newMT.Length; i++)
//                {
//                    Common.ExecuteText("UPDATE [Table] SET DisplayOrder =" + dtDO.Rows[i][0].ToString() + " WHERE TableID=" + newMT[i], tn);

//                }
//            }


//            tn.Commit();
//            connection.Close();
//            connection.Dispose();
//        }
//        catch (Exception ex)
//        {

//            tn.Rollback();
//            connection.Close();
//            connection.Dispose();

//        }
//        Response.Redirect(Request.RawUrl, false);
//    }
//}


//public string GetViewURL()
//{

//    return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&MenuID=" + Cryptography.Encrypt(_qsMenuID) + "&TableID=";

//}

//public string GetEditURL()
//{

//    return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&MenuID=" + Cryptography.Encrypt(_qsMenuID) + "&TableID=";

//}


//public string GetAddURL()
//{

//    return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&MenuID=" + Cryptography.Encrypt(_qsMenuID);

//}
//protected void BindTheGrid(int iStartIndex, int iMaxRows)
//{
//    try
//    {
//        int iTN = 0;

//        gvTheGrid.DataSource = RecordManager.ets_Table_Select(null,
//            "",
//            int.Parse(_qsMenuID),
//            int.Parse(Session["AccountID"].ToString()),
//            null, null, true,
//            "st.DisplayOrder", "ASC" ,
//            iStartIndex, iMaxRows, ref  iTN, Session["STs"].ToString());

//        gvTheGrid.VirtualItemCount = iTN;
//        gvTheGrid.DataBind();
//        if (gvTheGrid.TopPagerRow != null)
//            gvTheGrid.TopPagerRow.Visible = true;
//        GridViewRow gvr = gvTheGrid.TopPagerRow;
//        if (gvr != null)
//        {
//            _gvPager = (Common_Pager)gvr.FindControl("Pager");
//            _gvPager.AddURL = GetAddURL();
//        }
//    }
//    catch (Exception ex)
//    {
//        ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
//        SystemData.ErrorLog_Insert(theErrorLog);
//        lblMsg.Text = ex.Message;
//    }
//}
//protected void Pager_BindTheGridToExport(object sender, EventArgs e)
//{
//    _gvPager.ExportFileName = "Tables";
//    BindTheGrid(0, _gvPager.TotalRows);
//}
