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

public partial class Pages_Company_ContactUsAdmin : SecurePage
{
    Common_Pager _gvPager;

   

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Quotes and Contacts";
       
        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {

                if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
                { Response.Redirect("~/Default.aspx", false); }

                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }

                gvTheGrid.GridViewSortColumn = "ContactID";
                gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
              
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
            lblMsg.Text = ex.Message;
        }
    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
            int iTN = 0;
           

            gvTheGrid.DataSource = SecurityManager.Contact_Select(null,
                txtEmail.Text.Trim().Replace("'", "''"), 
                 null, 
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
            lblMsg.Text = ex.Message;
        }
    }


    protected bool IsFiltered()
    {
        if (txtEmail.Text!="" )
        {
            return true;
        }

        return false;
    }

    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }


    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblContactType = (Label)e.Row.FindControl("lblContactType");


            if (DataBinder.Eval(e.Row.DataItem, "ContactTypeID").ToString() == "1")
            {
                lblContactType.Text = "Quote";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "ContactTypeID").ToString() == "2")
            {
                lblContactType.Text = "Contact us";
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

    //public string GetEditURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/SystemOptionDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SystemOptionID=";

    //}


    //public string GetViewURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/SystemOptionDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SystemOptionID=";

    //}
    //public string GetAddURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/SystemOptionDetail.aspx?mode=" + Cryptography.Encrypt("add");

    //}

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "ContactUs";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtEmail.Text = "";
        gvTheGrid.GridViewSortColumn = "ContactID";
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

                        SecurityManager.Contact_Delete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
           
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
        "attachment;filename=Subscription.csv");
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
                            Label lblOptionNotes = (Label)dr.FindControl("lblEmail");
                            sw.Write("\"" + lblOptionNotes.Text + "\"");
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



}
