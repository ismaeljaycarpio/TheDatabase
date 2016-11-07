using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class User_List : SecurePage
{
    //int? _iAccountFilter;
    //bool? _bActiveFilter = null;
    Common_Pager _gvPager;
    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "FirstName";
    string _strGridViewSortDirection = "Asc";

    


    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e) 
    {
        Title = "Users";
        try
        {
          

            User ObjUser = (User)Session["User"];
           
            if (!IsPostBack)
            {

                //if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2,3"))
                //{Response.Redirect("~/Default.aspx", false); }              


                
                //PopulateAccountDDL();
                int iTN = 0;
                
                              
                    gvTheGrid.GridViewSortColumn = "FirstName";
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    BindTheGrid(0, gvTheGrid.PageSize);
               
            }
            else
            {
               
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

          
                if (Request.QueryString["pagetype"] != null)
                {
                    if (Request.QueryString["pagetype"].ToString() == "m")
                    {
                        if (_gvPager != null)
                        {
                            _gvPager.HideExport = true;
                        }

                    }
                }
            

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Users Load", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
    protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        try
        {


            lblMsg.Text = "";

                      


            int iTN = 0;
            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = SecurityManager.User_PopUp_Select(
                txtSearch.Text.Replace("'", "''"),
            int.Parse(Session["AccountID"].ToString()),
            gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
            iStartIndex, iMaxRows, ref iTN);

         
            gvTheGrid.VirtualItemCount = iTN;          


            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
            {
                gvTheGrid.TopPagerRow.Visible = true;               
            }
            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                if (ViewState[gvTheGrid.ID + "PageIndex"] != null)
                    _gvPager.PageIndex = int.Parse(ViewState[gvTheGrid.ID + "PageIndex"].ToString());
                _gvPager.PageSize = gvTheGrid.PageSize;
                _gvPager.TotalRows = iTN;
                //_gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Users Grid", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Users";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        gvTheGrid.GridViewSortColumn = "FirstName";
        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
        lnkSearch_Click(null, null);
    }


  
  



    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Users.csv");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);


        int iColCount = gvTheGrid.Columns.Count;
        for (int i = 2; i < iColCount; i++)
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
            for (int i = 2; i < iColCount; i++)
            {
                if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
                {
                }
                else
                {
                    switch (i)
                    {
                        case 4:
                            Label lblEmail = (Label)dr.FindControl("lblEmail");
                            sw.Write("\"" + lblEmail.Text + "\"");
                            break;

                        default:
                            if (!Convert.IsDBNull(dr.Cells[i]))
                            {
                                sw.Write("\"" + (dr.Cells[i].Text.Trim() == "&nbsp;" ? "" : dr.Cells[i].Text) + "\"");
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


   



    protected void lnkOk_Click(object sender, EventArgs e)
    {
        string sCheck = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("lblEmail")).Text + "__";


            }
        }

        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select record(s).');", true);
        }
        else
        {
            //ctl00_HomeContentPlaceHolder_hfSelectedTextIDs

            string strJS = @"function GetBackAndReFresh(sEmails,stype) { 
          window.parent.document.getElementById('hfSelectedEmails').value = sEmails;
           window.parent.document.getElementById('hfType').value = stype;
          window.parent.document.getElementById('btnUpdateEmail').click();
          parent.$.fancybox.close();
         }; GetBackAndReFresh('" + sCheck + "','"+Request.QueryString["type"].ToString()+"'); ";


            ScriptManager.RegisterClientScriptBlock(gvTheGrid, this.Page.GetType(), "message_alert", strJS, true);

        }
    }
}
