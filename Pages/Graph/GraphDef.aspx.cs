using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Graph_GraphDef : SecurePage
{
    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "DefinitionName";
    string _strGridViewSortDirection = "ASC";


    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Graph Definitions";

        try
        {
            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                PopulateEachTableDDL();

                if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                { Response.Redirect("~/Default.aspx", false); }

                if (Request.QueryString["SearchCriteria"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
                }

                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }

                if (Request.QueryString["SearchCriteria"] != null)
                {
                    gvTheGrid.PageSize = _iMaxRows;
                    gvTheGrid.GridViewSortColumn = _strGridViewSortColumn;
                    if (_strGridViewSortDirection.ToUpper() == "ASC")
                    {
                        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    }
                    else
                    {
                        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                    }
                    BindTheGrid(_iStartIndex, _iMaxRows);
                }
                else
                {
                    gvTheGrid.GridViewSortColumn = "DefinitionName";
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                }
            }
            else
            {
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

            string script = @"
                $(function () {
                    $('#hlConfig').fancybox({
                        scrolling: 'auto',
                        type: 'iframe',
                        width: 400,
                        height: 220,
                        titleShow: false
                    });
                });";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JSforAjax", script, true);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void PopulateSearchCriteria(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                txtDefinitionName.Text = xmlDoc.FirstChild[txtDefinitionName.ID].InnerText;
                string s = xmlDoc.FirstChild[ddlEachTable.ID].InnerText;
                if (!String.IsNullOrEmpty(s))
                {
                    ddlEachTable.SelectedValue = s;
                    PopulateEachAnalyteDDL(int.Parse(s));
                }
                s = xmlDoc.FirstChild[ddlEachAnalyte.ID].InnerText;
                if (!String.IsNullOrEmpty(s))
                {
                    ddlEachAnalyte.SelectedValue = s;
                }
                s = xmlDoc.FirstChild[chkIsActive.ID].InnerText;
                if (!String.IsNullOrEmpty(s))
                {
                    chkIsActive.Checked = bool.Parse(s);
                }
                s = xmlDoc.FirstChild[chkIsHidden.ID].InnerText;
                if (!String.IsNullOrEmpty(s))
                {
                    chkIsHidden.Checked = bool.Parse(s);
                }

                _iStartIndex = int.Parse(xmlDoc.FirstChild["iStartIndex"].InnerText);
                _iMaxRows = int.Parse(xmlDoc.FirstChild["iMaxRows"].InnerText);
                _strGridViewSortColumn = xmlDoc.FirstChild["GridViewSortColumn"].InnerText;
                _strGridViewSortDirection = xmlDoc.FirstChild["GridViewSortDirection"].InnerText;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }



    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        lblMsg.Text = "";

        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + txtDefinitionName.ID + ">" + HttpUtility.HtmlEncode(txtDefinitionName.Text) + "</" + txtDefinitionName.ID + ">" +
                   " <" + ddlEachTable.ID + ">" + HttpUtility.HtmlEncode(ddlEachTable.SelectedValue) + "</" + ddlEachTable.ID + ">" +
                   " <" + ddlEachAnalyte.ID + ">" + HttpUtility.HtmlEncode(ddlEachAnalyte.SelectedValue) + "</" + ddlEachAnalyte.ID + ">" +
                   " <" + chkIsActive.ID + ">" + HttpUtility.HtmlEncode(chkIsActive.Checked.ToString()) + "</" + chkIsActive.ID + ">" +
                   " <" + chkIsHidden.ID + ">" + HttpUtility.HtmlEncode(chkIsHidden.Checked.ToString()) + "</" + chkIsHidden.ID + ">" +
                   " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
                   " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
                   " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                   " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
        //End Searchcriteria

        if (gvTheGrid.Columns.Count > 6)
        {
            gvTheGrid.Columns[6].Visible = chkIsHidden.Checked;
        }
        if (gvTheGrid.Columns.Count > 9)
        {
            gvTheGrid.Columns[9].Visible = chkIsActive.Checked;
        }

        try
        {
            int iTN = 0;
            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = GraphManager.ets_GraphDefinition_Select(null /*int.Parse(Session["AccountID"].ToString())*/,
                txtDefinitionName.Text.Trim().Replace("'", "''"), null,
                null, chkIsHidden.Checked,
                ddlEachTable.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEachTable.SelectedValue),
                ddlEachAnalyte.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEachAnalyte.SelectedValue),
                null,
                chkIsActive.Checked ? null : (bool?)true,
                null, null, null, null,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
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
                //_gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
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
                    divEmptyData.Visible = false;
                }
                else
                {
                    divEmptyData.Visible = true;
                    divNoFilter.Visible = false;
                }
                hplNewData.NavigateUrl = GetAddURL();
                hplNewDataFilter.NavigateUrl = GetAddURL();
            }
            else
            {
                divEmptyData.Visible = false;
                divNoFilter.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {
        BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
            if (e.Row.Cells.Count > 8)
            {
                CheckBox cb = e.Row.Cells[8].FindControl("cbIsActive") as CheckBox;
                if (cb != null)
                {
                    cb.Checked = !(bool)((DataRowView)e.Row.DataItem)["IsActive"];
                }
            }
        }
    }
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

    public string GetEditURL()
    {
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath +
            "/Pages/Graph/GraphDefDetail.aspx?mode=" + Cryptography.Encrypt("edit") +
            "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) +
            "&GraphDefinitionID=";
    }

    public string GetAddURL()
    {
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/GraphDefDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());
    }


    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "GraphDefinition";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtDefinitionName.Text = "";
        ddlEachTable.SelectedValue = "-1";
        PopulateEachAnalyteDDL(-1);
        chkIsActive.Checked = false;
        chkIsHidden.Checked = false;
        gvTheGrid.GridViewSortColumn = "DefinitionName";
        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
        lnkSearch_Click(null, null);
        //BindTheGrid(0, gvTheGrid.PageSize);
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
            //_gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
            //if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
            //{
            //    BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
            //}
        }
    }


    private void DeleteItem(string keys)
    {
        bool isScriptRegistered = false;
        try
        {
            if (!string.IsNullOrEmpty(keys))
            {
                foreach (string sTemp in keys.Split(','))
                {
                    if (!string.IsNullOrEmpty(sTemp))
                    {
                        if (GraphManager.ets_GraphDefinition_Delete(int.Parse(sTemp)) == -2)
                        {
                            if (!isScriptRegistered)
                            {
                                ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", @"alert('Couldn\'t delete System Graph Definition');", true);
                                isScriptRegistered = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
        Response.AddHeader("content-disposition", "attachment;filename=GraphDefinition.csv");
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
                        case 4:
                            Label lbDefinitionName = (Label)dr.FindControl("lbDefinitionName");
                            sw.Write("\"" + lbDefinitionName.Text + "\"");
                            break;
                        case 5:
                            CheckBox cbIsSystem = (CheckBox)dr.FindControl("cbIsSystem");
                            sw.Write("\"" + (cbIsSystem.Checked ? "Y" : "N") + "\"");
                            break;
                        case 6:
                            CheckBox cbIsHidden = (CheckBox)dr.FindControl("cbIsHidden");
                            sw.Write("\"" + (cbIsHidden.Checked ? "Y" : "N") + "\"");
                            break;
                        case 7:
                            Label lbTableName = (Label)dr.FindControl("lbTableName");
                            sw.Write("\"" + lbTableName.Text + "\"");
                            break;
                        case 8:
                            Label lbColumnName = (Label)dr.FindControl("lbColumnName");
                            sw.Write("\"" + lbColumnName.Text + "\"");
                            break;
                        case 9:
                            CheckBox cbIsActive = (CheckBox)dr.FindControl("cbIsActive");
                            sw.Write("\"" + (cbIsActive.Checked ? "Y" : "N") + "\"");
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


    protected void PopulateEachTableDDL()
    {
        int iTN = 0;

        ddlEachTable.DataSource = RecordManager.ets_Table_Select(null,
            null,
            null,
            int.Parse(Session["AccountID"].ToString()),
            null, null, true,
            "st.TableName", "ASC",
            null, null, ref iTN, null);
        ddlEachTable.DataBind();
    }

    protected void PopulateEachAnalyteDDL(int TableID)
    {
        try
        {
            ddlEachAnalyte.Items.Clear();
            string strTableID = ddlEachTable.SelectedValue;

            int iTN = 0;

            List<Column> lstColumns = RecordManager.ets_Table_Columns(TableID,
                    null, null, ref iTN);

            Column dtColumn = new Column();
            foreach (Column eachColumn in lstColumns.AsQueryable().Where(eachColumn =>
                (eachColumn.IsStandard == false) && (!String.IsNullOrEmpty(eachColumn.GraphLabel) && (eachColumn.ColumnType == "number"))))
            {
                System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString());
                ddlEachAnalyte.Items.Insert(ddlEachAnalyte.Items.Count, aItem);
            }
            ddlEachAnalyte.Items.Insert(0, new ListItem("-- All --", "-1"));
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void ddlEachTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateEachAnalyteDDL(int.Parse(ddlEachTable.SelectedValue));
        BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected void ddlEachAnalyte_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected bool IsFiltered()
    {
        if (txtDefinitionName.Text != ""
            || ddlEachAnalyte.SelectedValue != "-1"
            || ddlEachTable.SelectedValue != "-1"
            || chkIsHidden.Checked
            || chkIsActive.Checked
            )
        {
            return true;
        }

        return false;
    }

    protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
    {
        BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected void chkIsHidden_CheckedChanged(object sender, EventArgs e)
    {
        BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected void ibDuplicate_Command(object sender, CommandEventArgs e)
    {
        try
        {
            GraphDefinition theGraphDefinition = GraphManager.ets_GraphDefinition_Detail(Convert.ToInt32(e.CommandArgument));
            theGraphDefinition.GraphDefinitionID = null;
            int id = GraphManager.ets_GraphDefinition_Insert(theGraphDefinition);
            BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }
}