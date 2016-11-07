using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;

public partial class Pages_Record_Batches :SecurePage
{
    Common_Pager _gvPager;

    Table _qsTable;
    User _ObjUser;
    string _strBatchName="Batches";

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2,3,5"))
        { Response.Redirect("~/Default.aspx", false); }


        Title = "Batches";
        try
        {


            _ObjUser = (User)Session["User"];
            _qsTable = RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"])));

            if (_qsTable != null)
            {
                _strBatchName = "Batches - " + _qsTable.TableName;
            }
           


            //lblTableName.Text =  _qsTable.TableName;
            if (!IsPostBack)
            {
                PopulateTerminology();
                Common.PopulateAdminDropDown(ref ddlAdminArea);
                ddlAdminArea.Text = "Batches";

                //txtDateTo.Attributes.Add("onClick", "javascript:setYearRange();");
              
                

                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }

                PopulateTableDDL();
                ddlTable.Text=Cryptography.Decrypt(Request.QueryString["TableID"]);


                gvTheGrid.GridViewSortColumn = "BatchID";
                gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                BindTheGrid(0, gvTheGrid.PageSize);
            }
            else
            {
                if (ddlTable.SelectedValue != "-1")
                {
                    _strBatchName = "Batches - " + ddlTable.SelectedItem.Text;
                }
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                //_gvPager.AddURL = GetAddURL();
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Batches", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

        //if (Request.UserAgent.Contains("Android"))
        //{
        //    ddlAdminArea.Visible = true;
        //}
        //else
        //{
        //    ddlAdminArea.Visible = false;
        //    lblAdminArea.Text = ddlAdminArea.SelectedItem.Text;
        //}

        //Ticket 846: Removing dropdown when in mobile
        //modified by: Ismael
        ddlAdminArea.Visible = false;
        lblAdminArea.Text = ddlAdminArea.SelectedItem.Text;
        //End Ticket 846

    }

    public string GetEditURL()
    {
        if (Request.QueryString["menu"] != null &&  Cryptography.Decrypt(Request.QueryString["menu"]) == "yes")
        {
            return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/UploadValidation.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Request.QueryString["TableID"] + "&BatchID=";
        }
        else
        {
            return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=";
        }
    }

  


    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        lblMsg.Text = "";
        try
        {
            int iTN = 0;


            int? iUserID = _ObjUser.UserID;
            if (Request.QueryString["menu"] != null && Cryptography.Decrypt(Request.QueryString["menu"]) == "yes")
            {
                iUserID=null;      
            } 

            //gvTheGrid.DataSource = UploadManager.ets_Batch_Select(null, 
            //    int.Parse(ddlTable.SelectedValue) == -1 ? null : (int?)int.Parse(ddlTable.SelectedValue),
            //    "", "",txtSearch.Text.Replace("'","''"),
            //   txtDateFrom.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateFrom.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
            //    txtDateTo.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text + " 23:59", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
            //    iUserID, int.Parse(Session["AccountID"].ToString()),
            //    gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
            //    iStartIndex, iMaxRows, ref iTN, Session["STs"].ToString());

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
                   dtDateTo= dtDateTo.Value.AddHours(23).AddMinutes(59);
                }
            }
            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = UploadManager.ets_Batch_Select(null,
               int.Parse(ddlTable.SelectedValue) == -1 ? null : (int?)int.Parse(ddlTable.SelectedValue),
               "", "", txtSearch.Text.Replace("'", "''"),
              dtDateFrom,dtDateTo,
               iUserID, int.Parse(Session["AccountID"].ToString()),
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
                if (ViewState[gvTheGrid.ID + "PageIndex"] != null)
                    _gvPager.PageIndex = int.Parse(ViewState[gvTheGrid.ID + "PageIndex"].ToString());

                _gvPager.PageSize = gvTheGrid.PageSize;
                _gvPager.TotalRows = iTN;
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
            ErrorLog theErrorLog = new ErrorLog(null, "Batches", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

            Label lblIsImportPositional = (Label)e.Row.FindControl("lblIsImportPositional");
            Label lblIsImported = (Label)e.Row.FindControl("lblIsImported");

            Label lblUserName = (Label)e.Row.FindControl("lblUserName");

            if (DataBinder.Eval(e.Row.DataItem, "IsImportPositional").ToString() == "True")
            {
                lblIsImportPositional.Text = "Yes";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "IsImportPositional").ToString() == "False")
            {
                lblIsImportPositional.Text = "No";
            }

            if (DataBinder.Eval(e.Row.DataItem, "IsImported").ToString() == "True")
            {
                lblIsImported.Text = "Yes";
            }
            else 
            {
                lblIsImported.Text = "No";
            }

            if (DataBinder.Eval(e.Row.DataItem, "UserIDUploaded") == DBNull.Value)
            {
                lblUserName.Text = "Auto Upload";
                lblUserName.ForeColor = System.Drawing.Color.Purple;
            }
          


        }

    }


    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);



    }
    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {

        _gvPager.ExportFileName = _strBatchName;
        BindTheGrid(0, _gvPager.TotalRows);
    }


    protected void PopulateTerminology()
    {
        stgTable.InnerText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), stgTable.InnerText, stgTable.InnerText);
        gvTheGrid.Columns[2].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), gvTheGrid.Columns[2].HeaderText, gvTheGrid.Columns[2].HeaderText);
    }

    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null,  true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());

        ddlTable.DataBind();
        //if (iTN == 0)
        //{
        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("All", "-1");
        ddlTable.Items.Insert(0, liAll);
        //}


    }

    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindTheGrid(0, gvTheGrid.PageSize);

    }



    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        txtSearch.Text = "";
        ddlTable.SelectedIndex = 0;

        gvTheGrid.GridViewSortColumn = "BatchID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
        lnkSearch_Click(null, null);
    }

    protected bool IsFiltered()
    {
        if (txtDateFrom.Text != "" || txtDateTo.Text != "" || txtSearch.Text != "" 
            || ddlTable.SelectedIndex != 0)
        {
            return true;
        }

        return false;
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

    //                    RecordManager.ets_Menu_Delete(int.Parse(sTemp));

    //                }
    //            }


    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLog theErrorLog = new ErrorLog(null, "Batches", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //        lblMsg.Text = ex.Message;

    //        ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
    //    }
    //}

    //protected void cmdBack_Click(object sender, ImageClickEventArgs e)

    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"], false);

    }


    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=\"" + _strBatchName + ".csv\"");
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
                            Label lblUserName = (Label)dr.FindControl("lblUserName");
                            sw.Write("\"" + lblUserName.Text + "\"");
                            break;
                        case 3:
                            Label lblDateAdded = (Label)dr.FindControl("lblDateAdded");
                            sw.Write("\"" + lblDateAdded.Text + "\"");
                            break;
                        case 4:
                            Label lblBatchDescription = (Label)dr.FindControl("lblBatchDescription");
                            sw.Write("\"" + lblBatchDescription.Text + "\"");
                            break;
                        case 5:
                            Label lblUploadedFileName = (Label)dr.FindControl("lblUploadedFileName");
                            sw.Write("\"" + lblUploadedFileName.Text + "\"");
                            break;
                        case 6:
                            Label lblValidCount = (Label)dr.FindControl("lblValidCount");
                            sw.Write("\"" + lblValidCount.Text + "\"");
                            break;
                        case 7:
                            Label lblNotValidCount = (Label)dr.FindControl("lblNotValidCount");
                            sw.Write("\"" + lblNotValidCount.Text + "\"");
                            break;
                        case 8:
                            Label lblIsImported = (Label)dr.FindControl("lblIsImported");
                            sw.Write("\"" + lblIsImported.Text + "\"");
                            break;
                        case 9:
                            Label lblIsImportPositional = (Label)dr.FindControl("lblIsImportPositional");
                            sw.Write("\"" + lblIsImportPositional.Text + "\"");
                            break;

                        default:
                            if (!Convert.IsDBNull(dr.Cells[i]))
                            {
                                sw.Write("\"" + dr.Cells[i].Text + "\"");
                            }

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
}
