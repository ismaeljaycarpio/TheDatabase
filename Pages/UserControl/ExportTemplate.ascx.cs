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

public partial class Pages_UserControl_ExportTemplate : System.Web.UI.UserControl
{
    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "ExportTemplateID";
    string _strGridViewSortDirection = "DESC";

    protected void PopulateTerminology()
    {
        stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));

    }

   

    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());

        ddlTable.DataBind();
        //if (iTN == 0)
        //{
        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("All", "");
        ddlTable.Items.Insert(0, liAll);
        //}


    }


    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }




    protected void Page_Load(object sender, EventArgs e)
    {
        //Title = "Export Templates";

        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                PopulateTableDDL();
                if (Request.QueryString["TableID"] != null)
                {
                    if (ddlTable.Items.FindByValue(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())) != null)
                        ddlTable.SelectedValue = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());

                }

                if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                { Response.Redirect("~/Default.aspx", false); }

                if (Request.QueryString["SearchCriteriaET"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteriaET"].ToString())));
                }


                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }

                if (Request.QueryString["SearchCriteriaET"] != null)
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
                    gvTheGrid.GridViewSortColumn = "ExportTemplateID";
                    gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                }
            }
            else
            {
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

            if (!IsPostBack)
            {
                PopulateTerminology();
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "ExportTemplate", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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

                ddlTable.Text = xmlDoc.FirstChild[ddlTable.ID].InnerText;
                txtSearch.Text = xmlDoc.FirstChild[txtSearch.ID].InnerText;

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
                   " <" + ddlTable.ID + ">" + HttpUtility.HtmlEncode(ddlTable.Text) + "</" + ddlTable.ID + ">" +
                    " <" + txtSearch.ID + ">" + HttpUtility.HtmlEncode(txtSearch.Text) + "</" + txtSearch.ID + ">" +
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





        try
        {
            int iTN = 0;

            gvTheGrid.Columns[4].HeaderText = gvTheGrid.Columns[4].HeaderText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));

            gvTheGrid.DataSource = ExportManager.dbg_ExportTemplate_Select(int.Parse(Session["AccountID"].ToString()),
                ddlTable.SelectedValue == "" ? null : (int?)int.Parse(ddlTable.SelectedValue),
                txtSearch.Text.Replace("'", "''"), gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN);


            if(iTN==0 & !IsPostBack & txtSearch.Text=="" & ddlTable.SelectedValue!="")
            {
                ExportManager.CreateDefaultExportTemplate(int.Parse(ddlTable.SelectedValue));

                //always copy above
                gvTheGrid.DataSource = ExportManager.dbg_ExportTemplate_Select(int.Parse(Session["AccountID"].ToString()),
               ddlTable.SelectedValue == "" ? null : (int?)int.Parse(ddlTable.SelectedValue),
               txtSearch.Text.Replace("'", "''"), gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
               iStartIndex, iMaxRows, ref iTN);
            }

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
            ErrorLog theErrorLog = new ErrorLog(null, "ExportTemplate", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }

    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)

    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);

    }

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");


            Label lblTable = (Label)e.Row.FindControl("lblTable");
            lblTable.Text = Common.GetValueFromSQL("SELECT TableName FROM [Table] WHERE TableID=" + (DataBinder.Eval(e.Row.DataItem, "TableID").ToString()));


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

    public string GetEditURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaET=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&ExportTemplateID=";

    }



    public string GetViewURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?mode=" + Cryptography.Encrypt("view") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaET=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&ExportTemplateID=";

    }
    public string GetAddURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?mode=" + Cryptography.Encrypt("add") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaET=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());

    }




    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "ExportTemplates";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        //ddlTable.Text = "";
        gvTheGrid.GridViewSortColumn = "ExportTemplateID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
        lnkSearch_Click(null, null);
        
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

                        //SystemData.ExportTemplate_Delete(int.Parse(sTemp));
                        ExportManager.dbg_ExportTemplate_Delete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Export Template", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=ExportTemplates.csv");
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

                if (i == 3)
                    sw.Write(",");
                //if (i < iColCount - 1)
                //{
                //    if (i == 3 || i == 4)
                //    {
                //        sw.Write(",");
                //    }
                //}
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
                            Label lblExportTemplateName = (Label)dr.FindControl("lblExportTemplateName");
                            sw.Write("\"" + lblExportTemplateName.Text + "\"");
                            break;
                        case 3:
                            Label lblTable = (Label)dr.FindControl("lblTable");
                            sw.Write("\"" + lblTable.Text + "\"");
                            break;


                    }

                    if (i == 3)
                        sw.Write(",");

                    //if (i < iColCount - 1)
                    //{
                    //    if (i >= 4 && i <= 7)
                    //    {
                    //        sw.Write(",");
                    //    }
                    //}
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();


        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }


    protected bool IsFiltered()
    {
        if (ddlTable.Text != "")
        {
            return true;
        }

        return false;
    }

}
