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
using System.Globalization;
using System.IO;

public partial class Pages_SystemData_PageCount : SecurePage
{
    Common_Pager _gvPager;
    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "visiteddate";
    string _strGridViewSortDirection = "DESC";

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Visitors";
       
        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
                { Response.Redirect("~/Default.aspx", false); }

                //populate the table.

                SystemData.PageCountPopulate();


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
                    gvTheGrid.GridViewSortColumn = "visiteddate";
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

                txtPageURL.Text = xmlDoc.FirstChild[txtPageURL.ID].InnerText;
                txtDateFrom.Text = xmlDoc.FirstChild[txtDateFrom.ID].InnerText;
                txtDateTo.Text = xmlDoc.FirstChild[txtDateTo.ID].InnerText;

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


    public string GetViewURL(string strvisiteddate, string strPageURL)
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/VisitorLog.aspx?visiteddate=" + Cryptography.Encrypt(strvisiteddate) + "&PageURL=" + Cryptography.Encrypt(strPageURL)  +"&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());

    }



    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {

        lblMsg.Text = "";



        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + txtPageURL.ID + ">" + HttpUtility.HtmlEncode(txtPageURL.Text) + "</" + txtPageURL.ID + ">" +
                   " <" + txtDateFrom.ID + ">" + HttpUtility.HtmlEncode(txtDateFrom.Text) + "</" + txtDateFrom.ID + ">" +
                   " <" + txtDateTo.ID + ">" + HttpUtility.HtmlEncode(txtDateTo.Text) + "</" + txtDateTo.ID + ">" +
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
           

            gvTheGrid.DataSource = SystemData.PageCount_Select(
                 txtPageURL.Text.Trim().Replace("'", "''"),                 
                 txtDateFrom.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateFrom.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                txtDateTo.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text + " 23:59", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
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
                _gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);

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
      


    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Visitor Logs";
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
        txtPageURL.Text = "";

        gvTheGrid.GridViewSortColumn = "visiteddate";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
       
        lnkSearch_Click(null, null);
      

    }

    protected bool IsFiltered()
    {
        if (txtDateFrom.Text != "" || txtDateTo.Text != "" || txtPageURL.Text != "")
        {
            return true;
        }

        return false;
    }

   

}
