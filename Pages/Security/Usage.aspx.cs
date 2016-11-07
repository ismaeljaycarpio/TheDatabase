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
public partial class Pages_Security_Usage : SecurePage
{
    Common_Pager _gvPager;

   

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Usage";
       
        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
                { Response.Redirect("~/Default.aspx", false); }

                PopulateAccountsDDL();

                ddlAccount.Text = Cryptography.Decrypt(Request.QueryString["AccountID"].ToString());

                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountList.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }
                else
                {
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountList.aspx";
                }

                if (Request.QueryString["Type"] != null)
                {
                    ddlCountBy.Text =Cryptography.Decrypt( Request.QueryString["Type"].ToString());
                    
                }
               


                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = int.Parse(Session["GridPageSize"].ToString()); }

                gvTheGrid.GridViewSortColumn = "UsageID";
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
            ErrorLog theErrorLog = new ErrorLog(null, "Usage", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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


            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = SecurityManager.Usage_Select(null,
                ddlAccount.Text=="-1"?null:(int?)int.Parse(ddlAccount.Text),
                null, ddlCountBy.Text=="S"?(bool?)true:null,
                 ddlCountBy.Text == "U" ? (bool?)true : null,
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
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
            if (_gvPager!=null)
            {
                if (ViewState[gvTheGrid.ID + "PageIndex"] != null)
                    _gvPager.PageIndex = int.Parse(ViewState[gvTheGrid.ID + "PageIndex"].ToString());
                _gvPager.PageSize = gvTheGrid.PageSize;
                _gvPager.TotalRows = iTN;
            }

            if (ddlCountBy.Text == "S")
            {
                gvTheGrid.Columns[3].Visible = true;
                gvTheGrid.Columns[4].Visible = false;
            }
            else
            {
                gvTheGrid.Columns[3].Visible = false;
                gvTheGrid.Columns[4].Visible = true;
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
            ErrorLog theErrorLog = new ErrorLog(null, "Usage", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

       



    }

    protected bool IsFiltered()
    {
        if (ddlAccount.Text != "-1" || ddlCountBy.Text != "S" 
            || txtDateTo.Text != "" || txtDateFrom.Text != "" )
        {
            return true;
        }

        return false;
    }

    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        if (txtDateTo.Text != "" && txtDateFrom.Text != "")
        {
            if (DateTime.ParseExact(txtDateFrom.Text + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture) >
                DateTime.ParseExact(txtDateTo.Text + " 23:59", "d/M/yyyy HH:m", CultureInfo.InvariantCulture))
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('From date can not be greater than to date!');",true);
                txtDateFrom.Focus();
                return;


            }
        }


        BindTheGrid(0, gvTheGrid.PageSize);       

    }



  


    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Usages";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        
        gvTheGrid.GridViewSortColumn = "UsageID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;

        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        ddlAccount.Text = "-1";
        ddlCountBy.Text = "S";
        lnkSearch_Click(null, null);
      

    }


    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Usages.csv");
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
                
                if (i < iColCount - 1)
                {

                    switch (i)
                    {
                        case 3:
                            
                                sw.Write(gvTheGrid.Columns[i].HeaderText);
                                sw.Write(",");
                           
                            break;

                        //case 4:
                        //    if (ddlCountBy.Text == "U")
                        //    {
                        //        sw.Write(gvTheGrid.Columns[i].HeaderText);
                        //        sw.Write(",");
                        //    }
                        //    break;

                        default:
                            sw.Write(gvTheGrid.Columns[i].HeaderText);
                            sw.Write(",");

                            break;

                    }
                  
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
                       
                        case 1:
                            Label lblAccountName = (Label)dr.FindControl("lblAccountName");
                            sw.Write("\"" + lblAccountName.Text + "\"");
                            sw.Write(",");
                            break;

                       
                        case 3:
                            if (ddlCountBy.Text == "S")
                            {
                                Label lblSignedInCount = (Label)dr.FindControl("lblSignedInCount");
                                sw.Write("\"" + lblSignedInCount.Text + "\"");
                                sw.Write(",");
                            }
                            break;
                       
                        case 4:
                            if (ddlCountBy.Text == "U")
                            {
                                Label lblUploadedCount = (Label)dr.FindControl("lblUploadedCount");
                                sw.Write("\"" + lblUploadedCount.Text + "\"");
                                sw.Write(",");
                            }
                            break;

                        default:
                            if (!Convert.IsDBNull(dr.Cells[i]))
                            {
                                sw.Write("\"" + (dr.Cells[i].Text.Trim() == "&nbsp;" ? "" : dr.Cells[i].Text) + "\"");
                                sw.Write(",");
                            }

                            break;
                    }

                    //if (i < iColCount - 1)
                    //{
                    //    sw.Write(",");
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



    protected void ddlCountBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
    protected void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    protected void PopulateAccountsDDL()
    {
        int iTN = 0;

        DataTable dtTemp = SecurityManager.Account_Summary(null, "", "", "", "", null, null, null,
            "AccountName", "ASC", null, null, ref iTN);

        ddlAccount.DataSource = dtTemp;
        ddlAccount.DataBind();

        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("All", "-1");
        ddlAccount.Items.Insert(0, liAll);
    }

}
