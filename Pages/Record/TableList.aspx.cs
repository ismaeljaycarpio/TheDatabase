using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Pages_Record_TableList : SecurePage
{
    Common_Pager _gvPager;
    User _ObjUser;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "TableName";
    string _strGridViewSortDirection = "ASC";


    public string GetEditURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&MenuID=" + Cryptography.Encrypt("-1") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=";

    }


    public string GetAddURL()
    {

        //return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&MenuID=" + Cryptography.Encrypt(ddlRecordGroupFilter.SelectedValue) + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableOption.aspx?MenuID=" + Cryptography.Encrypt("-1") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());
        
    }

    public string GetViewURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=";

    }

    public string GetRootURL()
    {
        
        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/";

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

                txtTableSearch.Text = xmlDoc.FirstChild[txtTableSearch.ID].InnerText;
                chkIsActive.Checked = bool.Parse( xmlDoc.FirstChild[chkIsActive.ID].InnerText);

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



    //protected void PopulateRecordGroupFilter()
    //{
    //    int iTN=0;
    //    ddlRecordGroupFilter.DataSource = RecordManager.ets_Menu_Select(null, "", null,
    //    int.Parse(Session["AccountID"].ToString()),true,
    //    "Menu", "ASC", null, null, ref iTN);
    //    ddlRecordGroupFilter.DataBind();
    //    ListItem liAll = new ListItem("All", "-1");
    //    ddlRecordGroupFilter.Items.Insert(0, liAll);

    //}

    protected void PopulateTerminology()
    {
        lblAdminArea.Text = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Tables", "Tables");
        stgTable.InnerText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), stgTable.InnerText, stgTable.InnerText);
        gvTheGrid.Columns[3].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), gvTheGrid.Columns[3].HeaderText, gvTheGrid.Columns[3].HeaderText);

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        { Response.Redirect("~/Default.aspx", false); }




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


        ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);




        Title = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Tables", "Tables");
        lblMsg.Text = "";
        try
        {
            _ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                Session["DataReminder"] = null;
                Session["DataReminderUser"] = null;

                Common.PopulateAdminDropDown(ref ddlAdminArea);
                ddlAdminArea.Text = "TableList";

                PopulateTerminology();

                //if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                //{
                //    hlMenuEdit.Visible = false;
                //}

                if (Request.QueryString["SearchCriteria"] != null &&  Request.QueryString["SearchCriteria"].ToString().Trim().Length>0)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
                }

                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 100; }

                //PopulateRecordGroupFilter();
                //if (Request.QueryString["MenuID"] != null)
                //    ddlRecordGroupFilter.Text = Cryptography.Decrypt( Request.QueryString["MenuID"]);


                if (Request.QueryString["SearchCriteria"] != null && Request.QueryString["SearchCriteria"].ToString().Trim().Length>0)
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
                    gvTheGrid.GridViewSortColumn = "TableName";
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                
                }

                           

            }
            else
            {
                GridViewRow gvr = gvTheGrid.TopPagerRow;
                if (gvr!=null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");


            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
            //lblAdminArea.Text = ddlAdminArea.SelectedItem.Text;
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


    protected void ddlRecordGroupFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindTheGrid(0, gvTheGrid.PageSize);

    }


    protected void ddlAccountFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        //PopulateRecordGroupFilter();
        BindTheGrid(0, gvTheGrid.PageSize);
    }

    
    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {


        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + txtTableSearch.ID + ">" + HttpUtility.HtmlEncode(txtTableSearch.Text) + "</" + txtTableSearch.ID + ">" +
                   " <" + chkIsActive.ID + ">" + HttpUtility.HtmlEncode(chkIsActive.Checked.ToString()) + "</" + chkIsActive.ID + ">" +
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
           
            gvTheGrid.DataSource = RecordManager.ets_Table_Select_dt(null,
                txtTableSearch.Text.Trim().Replace("'","''"), null, int.Parse(Session["AccountID"].ToString()),
                null, null,!chkIsActive.Checked,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref  iTN, Session["STs"].ToString());

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

            if (this.Master.FindControl("hfIsAccountHolder") != null)
            {
                HiddenField hfIsAccountHolder = (HiddenField)this.Master.FindControl("hfIsAccountHolder");
                if(hfIsAccountHolder!=null && hfIsAccountHolder.Value=="")
                {
                    divEmptyData.Visible = false;
                    hplNewDataFilter.Visible = false;
                    spnOr.Visible = false;

                    if (_gvPager != null)
                     {
                         _gvPager.HideAdd = true;
                     }
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }


    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName ="Tables";
        BindTheGrid(0, _gvPager.TotalRows);
    }


    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }


    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtTableSearch.Text = "";
        //ddlRecordGroupFilter.SelectedIndex = 0;
        chkIsActive.Checked = false;

        gvTheGrid.GridViewSortColumn = "TableName";
        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;

        lnkSearch_Click(null, null);
    }
      

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
            Label lblMenu = (Label)e.Row.FindControl("lblMenu");
                if (DataBinder.Eval(e.Row.DataItem, "MenuID")!=DBNull.Value)//acutally parent menu id
                {
                    Menu theMenu = RecordManager.ets_Menu_Details(int.Parse(DataBinder.Eval(e.Row.DataItem, "MenuID").ToString()));
                    if (theMenu != null)
                    {
                       
                        lblMenu.Text = theMenu.MenuP;
                    }
                }
                else
                {
                    lblMenu.Text = "--Top Level--";
                }
               
            }
    }

       

    
    protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    protected void gvTheGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetype")
        {
            lblMsg.Text = "";
            try
            {
                RecordManager.ets_Table_Delete(Convert.ToInt32(e.CommandArgument));
                Response.Redirect(Request.RawUrl , false);
                //BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);

                //_gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
                //if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
                //{
                //    BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
                //}
                  
            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                //lblMsg.Text = ex.Message;
                Table oTable = RecordManager.ets_Table_Details(Convert.ToInt32(e.CommandArgument));

                if (ex.Message.IndexOf("FK_TempRecord_Table") > -1)
                {
                    lblMsg.Text = "Delete failed! " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " -" + oTable.TableName + "- is used in Temp Records!";
                }
                //else if (ex.Message.IndexOf("LocationTable_Table") > -1)
                //{
                //    lblMsg.Text = "Delete failed! " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " -" + oTable.TableName + "- is used in " +
                //        SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Locations", "Locations") + "!";
                //}
                else if (ex.Message.IndexOf("Record_Table") > -1)
                {
                    lblMsg.Text = "Delete failed! " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " -" + oTable.TableName + "- is used in Records!";
                }
                else if (ex.Message.IndexOf("FK_Batch_Table") > -1)
                {
                    lblMsg.Text = "Delete failed! " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " -" + oTable.TableName + "- is used in Batches!";
                }
                else
                {
                    lblMsg.Text = ex.Message;
                }
            }
        }
    }



    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Tables.csv");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);


        int iColCount = gvTheGrid.Columns.Count;
        for (int i = 0; i < iColCount; i++)
        {
            if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText) || gvTheGrid.Columns[i].HeaderText=="Map Pin")
            {
            }
            else
            {
                sw.Write(gvTheGrid.Columns[i].HeaderText);
                if (i < iColCount - 1 && i!=2)
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
                        case 3:
                            HyperLink hlView = (HyperLink)dr.FindControl("hlView");
                            sw.Write("\"" + hlView.Text + "\"");
                            break;
                        case 4:
                            //
                            Label lblMenu = (Label)dr.FindControl("lblMenu");
                            sw.Write("\"" + lblMenu.Text + "\"");

                            break;
                        //default:
                        //    if (!Convert.IsDBNull(dr.Cells[i]))
                        //    {
                        //        sw.Write("\"" + dr.Cells[i].Text + "\"");
                        //    }

                        //    break;
                    }

                    if (i < iColCount - 1 && i!=2)
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
        if (txtTableSearch.Text != "" 
            || chkIsActive.Checked != false)
        {
            return true;
        }

        return false;
    }


}
