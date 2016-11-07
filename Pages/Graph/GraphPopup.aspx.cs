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

public partial class Pages_Graph_GraphPopup : SecurePage
{
    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "GraphOptionID";
    string _strGridViewSortDirection = "DESC";


    protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void lnkOk_Click(object sender, EventArgs e)
    {
        string sCheck = "";

        LinkButton lnkHeading = (LinkButton)sender;
        sCheck = lnkHeading.CommandArgument;
        //for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        //{
        //    bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
        //    if (ischeck)
        //    {
        //        sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text ;

        //        break;
        //    }
        //}

        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select record(s).');", true);
        }
        else
        {
            //ctl00_HomeContentPlaceHolder_hfSelectedTextIDs

            string strJS = @"function GetBackAndReFresh(sGraphs) { 
          window.parent.document.getElementById('hfSelectedGraphs').value = sGraphs;
          window.parent.document.getElementById('btnUpdateGraphs').click();
          parent.$.fancybox.close();
         }; GetBackAndReFresh('" + sCheck + "'); ";


            ScriptManager.RegisterClientScriptBlock(gvTheGrid, this.Page.GetType(), "message_alert", strJS, true);

        }
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.QueryString["graphbase"] != null)
            this.MasterPageFile = "~/Home/Popup.master";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Saved Graphs";
       
        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {

                //if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                //{ Response.Redirect("~/Default.aspx", false); }

                if (Request.QueryString["SearchCriteria"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
                }


                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize =50; }

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
                    gvTheGrid.GridViewSortColumn = "GraphOptionID";
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


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Option load", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
                chkIsActive.Checked = bool.Parse(xmlDoc.FirstChild[chkIsActive.ID].InnerText);

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

            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = GraphManager.ets_GraphOption_Select(int.Parse(Session["AccountID"].ToString()),
                2, false,!chkIsActive.Checked,
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
                if (ViewState[gvTheGrid.ID + "PageIndex"] != null)
                    _gvPager.PageIndex = int.Parse(ViewState[gvTheGrid.ID + "PageIndex"].ToString());

                _gvPager.PageSize = gvTheGrid.PageSize;
                _gvPager.TotalRows = iTN;
                //_gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
            }


            if (iTN == 0)
            {
                if (IsFiltered())
                {
                    //divNoFilter.Visible = true;
                    divEmptyData.Visible = false;
                }
                else
                {
                    divEmptyData.Visible = true;
                    //divNoFilter.Visible = false;
                }
                //hplNewData.NavigateUrl = GetAddURL();
                //hplNewDataFilter.NavigateUrl = GetAddURL();
            }
            else
            {
                divEmptyData.Visible = false;
                //divNoFilter.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Otion select", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/GraphOptionDetail.aspx?page=list&mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&GraphOptionID=";

    }


    public string GetViewURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/GraphOptionDetail.aspx?page=list&mode=" + Cryptography.Encrypt("view") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&GraphOptionID=";

    }
    public string GetAddURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/GraphOptionDetail.aspx?page=list&mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());

    }


   

   

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        chkIsActive.Checked = false;
        gvTheGrid.GridViewSortColumn = "GraphOptionID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
        lnkSearch_Click(null, null);
    }


   



 


  


    protected bool IsFiltered()
    {
        if (chkIsActive.Checked !=false)
        {
            return true;
        }

        return false;
    }

}
