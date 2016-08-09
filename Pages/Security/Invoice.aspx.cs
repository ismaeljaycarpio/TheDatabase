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

public partial class Pages_Security_Invoice : SecurePage
{
    Common_Pager _gvPager;
    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "InvoiceID";
    string _strGridViewSortDirection = "DESC";
   

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Invoices";
       
        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {

                if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
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
                    gvTheGrid.GridViewSortColumn = "InvoiceID";
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
            ErrorLog theErrorLog = new ErrorLog(null, "Invoice", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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

                txtAccountName.Text = xmlDoc.FirstChild[txtAccountName.ID].InnerText;
                txtEmail.Text = xmlDoc.FirstChild[txtEmail.ID].InnerText;
                txtInvoiceDateFrom.Text = xmlDoc.FirstChild[txtInvoiceDateFrom.ID].InnerText;
                txtInvoiceDateTo.Text = xmlDoc.FirstChild[txtInvoiceDateTo.ID].InnerText;
                ddlIsPaid.Text = xmlDoc.FirstChild[ddlIsPaid.ID].InnerText;

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
                    " <" + txtAccountName.ID + ">" + HttpUtility.HtmlEncode(txtAccountName.Text) + "</" + txtAccountName.ID + ">" +
                     " <" + txtEmail.ID + ">" + HttpUtility.HtmlEncode(txtEmail.Text) + "</" + txtEmail.ID + ">" +
                      " <" + txtInvoiceDateFrom.ID + ">" + HttpUtility.HtmlEncode(txtInvoiceDateFrom.Text) + "</" + txtInvoiceDateFrom.ID + ">" +
                       " <" + txtInvoiceDateTo.ID + ">" + HttpUtility.HtmlEncode(txtInvoiceDateTo.Text) + "</" + txtInvoiceDateTo.ID + ">" +
                        " <" + ddlIsPaid.ID + ">" + HttpUtility.HtmlEncode(ddlIsPaid.Text) + "</" + ddlIsPaid.ID + ">" +
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

            bool? bIsPaid=null;

            if( ddlIsPaid.SelectedValue != "-1")
            {
                bIsPaid=bool.Parse(ddlIsPaid.SelectedValue);
            }

            gvTheGrid.DataSource = InvoiceManager.ets_Invoice_Select(null, txtAccountName.Text.Trim().Replace("'", "''"), null,
                                txtInvoiceDateFrom.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtInvoiceDateFrom.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                txtInvoiceDateTo.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtInvoiceDateTo.Text + " 23:59", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                "", bIsPaid, "", txtEmail.Text.Trim().Replace("'", "''"),
                "","","",null,null, 
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
            ErrorLog theErrorLog = new ErrorLog(null, "Sensor Type", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected bool IsFiltered()
    {
        if (txtAccountName.Text != ""
            || txtEmail.Text != "" || txtInvoiceDateFrom.Text != "" || txtInvoiceDateTo.Text != ""
            || ddlIsPaid.SelectedIndex != 0 )
        {
            return true;
        }

        return false;
    }

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //Label lblIsPaid = (Label)e.Row.FindControl("lblIsPaid");

            //if (DataBinder.Eval(e.Row.DataItem, "IsPaid").ToString() == "True")
            //{
            //    lblIsPaid.Text = "Yes";
            //}
            //else if (DataBinder.Eval(e.Row.DataItem, "IsPaid").ToString() == "False")
            //{
            //    lblIsPaid.Text = "No";
            //}
            
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

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/InvoiceDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&InvoiceID=";

    }


    public string GetViewURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/InvoiceDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&InvoiceID=";

    }
    public string GetAddURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/InvoiceDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());

    }

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Invoices";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
       
        txtAccountName.Text = "";
        txtEmail.Text = "";
        txtInvoiceDateFrom.Text = "";
        txtInvoiceDateTo.Text = "";
        ddlIsPaid.SelectedIndex = 0;
        gvTheGrid.GridViewSortColumn = "InvoiceID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;

        lnkSearch_Click(null, null);
    }













    protected void ddlIsPaid_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
}
