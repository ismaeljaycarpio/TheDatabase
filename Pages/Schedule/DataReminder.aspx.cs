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

public partial class Pages_Schedule_DataReminder : SecurePage
{
    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "DataReminderID";
    string _strGridViewSortDirection = "ASC";
    int _iColumnID = -1;



    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Reminders";

        try
        {


            User ObjUser = (User)Session["User"];
            _iColumnID = int.Parse(Request.QueryString["ColumnID"].ToString());

            //if (_iColumnID == -1)
            //{
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please add the column first.');parent.$.fancybox.close();", true);
            //    //return;
            //}

            if (!IsPostBack)
            {
                if (_iColumnID == -1)
                {
                    if (Session["DataReminder"] == null)
                    {

                        DataTable dtDataReminder = new DataTable();
                        dtDataReminder.Columns.Add("DataReminderID");
                        dtDataReminder.Columns.Add("ColumnID");
                        dtDataReminder.Columns.Add("NumberOfDays");
                        dtDataReminder.Columns.Add("ReminderHeader");
                        dtDataReminder.Columns.Add("ReminderContent");

                        Session["DataReminder"] = dtDataReminder;
                    }

                }

                
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
                    gvTheGrid.GridViewSortColumn = "DataReminderID";
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


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "DataReminder", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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

                //txtOptionKeySearch.Text = xmlDoc.FirstChild[txtOptionKeySearch.ID].InnerText;

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
                   //" <" + txtOptionKeySearch.ID + ">" + HttpUtility.HtmlEncode(txtOptionKeySearch.Text) + "</" + txtOptionKeySearch.ID + ">" +
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

            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            if (_iColumnID == -1)
            {
                DataTable dtDataReminder= (DataTable)Session["DataReminder"];
                gvTheGrid.DataSource = dtDataReminder;
                iTN = dtDataReminder.Rows.Count;
            }
            else
            {
                gvTheGrid.DataSource = ReminderManager.ets_DataReminder_Select(_iColumnID, null, "", "",
                  gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                    iStartIndex, iMaxRows, ref iTN);
            }

            Session["DataReminderCount"] = iTN;
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
            ErrorLog theErrorLog = new ErrorLog(null, "DataReminder", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminderDetail.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&DataReminderID=";

    }


    public string GetViewURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminderDetail.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&mode=" + Cryptography.Encrypt("view") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&DataReminderID=";

    }
    public string GetAddURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminderDetail.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());

    }


    //public string GetEditURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminderDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&DataReminderID=";

    //}


    //public string GetViewURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminderDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&DataReminderID=";

    //}
    //public string GetAddURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminderDetail.aspx?mode=" + Cryptography.Encrypt("add");

    //}

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Data Reminders";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        //txtOptionKeySearch.Text = "";
        gvTheGrid.GridViewSortColumn = "DataReminderID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
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




    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=DataReminders.csv");
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
                            Label lblNumberOfDays = (Label)dr.FindControl("lblNumberOfDays");
                            sw.Write("\"" + lblNumberOfDays.Text + "\"");
                            break;
                        case 5:
                            Label lblReminderHeader = (Label)dr.FindControl("lblReminderHeader");
                            sw.Write("\"" + lblReminderHeader.Text + "\"");
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
                        if (_iColumnID == -1)
                        {
                            if (Session["DataReminder"] != null )
                            {
                                DataTable dtDataReminder = (DataTable)Session["DataReminder"];
                                for (int i = 0; i < dtDataReminder.Rows.Count; i++)
                                {
                                    if (sTemp == dtDataReminder.Rows[i]["DataReminderID"].ToString())
                                    {
                                        dtDataReminder.Rows.RemoveAt(i);
                                    }
                                }

                                dtDataReminder.AcceptChanges();
                                Session["DataReminder"] = dtDataReminder;

                            }

                            if (Session["DataReminderUser"] != null)
                            {
                                DataTable dtDataReminderUser = (DataTable)Session["DataReminderUser"];

                                foreach (DataRow dr in dtDataReminderUser.Rows)
                                {
                                    if (sTemp == dr["DataReminderID"].ToString())
                                    {
                                        dr.Delete();
                                        //dtDataReminderUser.Rows.Remove(dr);
                                    }
                                }
                                dtDataReminderUser.AcceptChanges();
                                Session["DataReminderUser"] = dtDataReminderUser;
                            }


                        }
                        else
                        {
                            ReminderManager.ets_DataReminder_Delete(Convert.ToInt32(sTemp));
                        }
                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Data Reminder delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            //lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }


    protected bool IsFiltered()
    {
        //if (txtOptionKeySearch.Text != "")
        //{
        //    return true;
        //}

        return false;
    }

}
