using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Globalization;

public partial class Pages_Record_Notification : SecurePage
{
    Common_Pager _gvPager;


    
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    void Page_PreInit(object sender, EventArgs e)
    {
        //Account _theAccount;
        //_theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
        //if (!IsPostBack)
        //{
        //    if (_theAccount.MasterPage != "")
        //        Page.MasterPageFile = _theAccount.MasterPage;
        //}
    }



    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");


        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Notifications";
       
        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {

                //if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
                //{ Response.Redirect("~/Default.aspx", false); }
                PopulateTerminology();
                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }


                Common.PopulateAdminDropDown(ref ddlAdminArea);
                ddlAdminArea.Text = "Notifications";


                //PopulateTable();

                gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                BindTheGrid(0, gvTheGrid.PageSize);
            }
            else
            {
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Notifications", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

        if (Request.UserAgent.Contains("Android"))
        {
            ddlAdminArea.Visible = true;
        }
        else
        {
            ddlAdminArea.Visible = false;
            lblAdminArea.Text = ddlAdminArea.SelectedItem.Text;
        }


    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
            int iTN = 0;

            DateTime? dtDateFrom = null;
            DateTime? dtDateTo = null;

            if (txtDateFrom.Text != "")
            {
                DateTime dtTemp;
                if (DateTime.TryParseExact(txtDateFrom.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                {
                    dtDateFrom = dtTemp;
                }
            }
            if (txtDateTo.Text != "")
            {
                DateTime dtTemp;
                if (DateTime.TryParseExact(txtDateTo.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                {
                    dtDateTo = dtTemp;
                    dtDateTo = dtDateTo.Value.AddHours(23).AddMinutes(59);
                }
            }


            gvTheGrid.DataSource = RecordManager.ets_Notification_Select(null, int.Parse(Session["AccountID"].ToString()),
                dtDateFrom, dtDateTo,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN, Session["STs"].ToString());

            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                //_gvPager.AddURL = GetAddURL();
            }


            if (iTN == 0)
            {
                if (IsFiltered())
                {
                    divNoFilter.Visible = true;
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

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Notification", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }
    //protected void PopulateTable()
    //{
    //    ddlDocumentType.DataSource = Common.DataTableFromText("SELECT DocumentTypeID,DocumentTypeName FROM DocumentType");
    //    ddlDocumentType.DataBind();
    //    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("All", "-1");
    //    ddlDocumentType.Items.Insert(0, liSelect);     
    //}
    
    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    
    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);      

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

    //public string GetEditURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&DocumentID=";

    //}


    public string GetViewURL() //link with Record page
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?warning=yes&TableID=";

    }
    //public string GetAddURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("add");

    //}

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Notifications";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {

        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        lnkSearch_Click(null, null);
        //BindTheGrid(0, gvTheGrid.PageSize);
    }


    //protected void Pager_DeleteAction(object sender, EventArgs e)
    //{
    //    string sCheck = "";
    //    for (int i = 0; i < gvTheGrid.Rows.Count; i++)
    //    {
    //        bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
    //        if (ischeck)
    //        {
    //            sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
    //        }
    //    }
    //    if (string.IsNullOrEmpty(sCheck))
    //    {
    //        ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
    //    }
    //    else
    //    {
    //        DeleteItem(sCheck);
    //        BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
    //        _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
    //        if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
    //        {
    //            BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
    //        }
    //    }

    //}



    //private void DeleteItem(string keys)
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(keys))
    //        {

    //            foreach (string sTemp in keys.Split(','))
    //            {
    //                if (!string.IsNullOrEmpty(sTemp))
    //                {

    //                    SystemData.SystemOption_Delete(int.Parse(sTemp));

    //                }
    //            }


    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLog theErrorLog = new ErrorLog(null, "SystemOption", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //        lblMsg.Text = ex.Message;

    //        //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
    //    }
    //}


    protected void PopulateTerminology()
    {
        gvTheGrid.Columns[2].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), gvTheGrid.Columns[2].HeaderText, gvTheGrid.Columns[2].HeaderText);
    }

    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Notifications.csv");
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
                        case 1:
                             Label lblLastWarningTime = (Label)dr.FindControl("lblLastWarningTime");
                            sw.Write("\"" + lblLastWarningTime.Text + "\"");
                            break;
                        case 2:
                            Label lblTableName = (Label)dr.FindControl("lblTableName");
                            sw.Write("\"" + lblTableName.Text + "\"");
                            break;
                        case 3:
                            HyperLink hlView = (HyperLink)dr.FindControl("hlView");
                            sw.Write("\"" + hlView.Text + "\"");
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




    protected void ddlAdminArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(Common.GetNavigateURL(ddlAdminArea.SelectedValue,int.Parse(Session["AccountID"].ToString())), false);
    }

    protected bool IsFiltered()
    {
        if (txtDateFrom.Text != "" || txtDateTo.Text != "" )
        {
            return true;
        }

        return false;
    }


    //protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    lnkSearch_Click(null, null);
    //}
}
