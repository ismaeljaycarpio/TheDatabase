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

public partial class Pages_SystemData_VisitorLog : SecurePage
{
    Common_Pager _gvPager;

   

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Visitor Logs";
       
        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
                { Response.Redirect("~/Default.aspx", false); }


                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/PageCount.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }
                else
                {
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/PageCount.aspx";
                }


                if (Request.QueryString["visiteddate"] != null)
                {
                    txtDateFrom.Text = Cryptography.Decrypt(Request.QueryString["visiteddate"].ToString());
                    txtDateFrom.Text = txtDateFrom.Text.Substring(0, txtDateFrom.Text.IndexOf(" "));
                    txtDateTo.Text =  txtDateFrom.Text;
                    
                }
                if (Request.QueryString["PageURL"] != null)
                {
                    txtPageURL.Text = Cryptography.Decrypt(Request.QueryString["PageURL"].ToString());

                }



                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }

                gvTheGrid.GridViewSortColumn = "VisitorLogID";
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
            ErrorLog theErrorLog = new ErrorLog(null, "VisitorLog", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {

        lblMsg.Text = "";
        try
        {
            int iTN = 0;
           

            gvTheGrid.DataSource = SystemData.ets_VisitorLog_Select(null,null,
                 txtIPAddress.Text.Replace("'", "''"),
                txtBrowser.Text.Replace("'","''"),
                 txtPageURL.Text.Trim().Replace("'", "''"),
                 txtEmail.Text.Trim().Replace("'", "''"),
                                txtDateFrom.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateFrom.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                txtDateTo.Text.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text + " 23:59", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN);
            
            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

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
            ErrorLog theErrorLog = new ErrorLog(null, "VisitorLog", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
        txtEmail.Text = "";
        txtBrowser.Text = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        txtPageURL.Text = "";
        txtIPAddress.Text = "";
        gvTheGrid.GridViewSortColumn = "VisitorLogID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
        lnkSearch_Click(null, null);
      

    }

    protected bool IsFiltered()
    {
        if (txtEmail.Text != "" || txtBrowser.Text != "" || txtDateFrom.Text != ""
            || txtDateTo.Text != "" || txtPageURL.Text != "" || txtIPAddress.Text != "")
        {
            return true;
        }

        return false;
    }


   

}
