using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class Pages_Template_TableList : SecurePage
{
    Common_Pager _gvPager;

    User _ObjUser;

    public string GetViewURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&MenuID=" + Cryptography.Encrypt("-1") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&TableID=";
    }

   

        

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        { Response.Redirect("~/Default.aspx", false); }



        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            divAccounts.Visible = true;
        }




//        string strHelpJS = @" $(function () {
//            $('#hlHelpCommon').fancybox({
//                scrolling: 'auto',
//                type: 'iframe',
//                'transitionIn': 'elastic',
//                'transitionOut': 'none',
//                width: 600,
//                height: 350,
//                titleShow: false
//            });
//        });";


        //ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);


      


        Title = "Copy Tables";
        lblMsg.Text = "";
        try
        {
            _ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                PopulateHelp("CopyTableHelp");
                hlCancel.NavigateUrl = "~/Default.aspx";

                if (Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.IndexOf("Record/TableOption.aspx") > -1)
                {
                    hlCancel.NavigateUrl = Request.UrlReferrer.ToString();
                }

                if (Request.UrlReferrer != null)
                {
                    hlBack.NavigateUrl = Request.UrlReferrer.ToString();
                }
                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableOption.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }

                PopulateAccountsDDL();

                ddlFromAccount.Text = SystemData.SystemOption_ValueByKey_Account("TemplateAccountID",null,null);
                ddlToAccount.Text =  Session["AccountID"].ToString();
                
              
                gvTheGrid.GridViewSortColumn = "Menu,TableName";
                gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                BindTheGrid();
            }
            else
            {
                GridViewRow gvr = gvTheGrid.TopPagerRow;
                if (gvr!=null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

            }

            if (!IsPostBack)
            {
                PopulateTerminology();
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table Copy", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void PopulateTerminology()
    {
        spanCaption.InnerText = spanCaption.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));
        spanAvailableTablesCap.InnerText = spanAvailableTablesCap.InnerText.Replace("Tables", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Tables", "Tables"));

        gvTheGrid.Columns[2].HeaderText = gvTheGrid.Columns[2].HeaderText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));

    }


    protected void PopulateHelp(string strContentKey)
    {
        Content theContent = SystemData.Content_Details_ByKey(strContentKey, null);

        if (theContent != null)
        {
            lblHelpContent.Text = theContent.ContentP;
        }
    }

    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {
        BindTheGrid();
    }

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");
            hlView.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?template=true&mode=" + Cryptography.Encrypt("view") +  "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt((DataBinder.Eval(e.Row.DataItem, "TableID").ToString()));
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


    protected void PopulateAccountsDDL()
    {
        int iTN = 0;

        //DataTable dtTemp = SecurityManager.Account_Summary(null, "", "", "", "", true, null, null,
        //    "AccountName", "ASC", null, null, ref iTN);

        DataTable dtTemp = Common.DataTableFromText(@"SELECT AccountID, AccountName FROM Account 
            ORDER BY AccountName");
        
        ddlFromAccount.DataSource = dtTemp;
        ddlFromAccount.DataBind();

        ddlToAccount.DataSource=dtTemp;
        ddlToAccount.DataBind();
       
    } 

    
    protected void BindTheGrid()
    {

        try
        {
            int iTN = 0;
            //ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = RecordManager.ets_Table_Select(null, 
                "",
                null,
                int.Parse(ddlFromAccount.Text),
                null, null,true,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                null, null, ref  iTN, Session["STs"].ToString());

            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                //if (ViewState[gvTheGrid.ID + "PageIndex"] != null)
                //    _gvPager.PageIndex = int.Parse(ViewState[gvTheGrid.ID + "PageIndex"].ToString());
                gvr.Visible = false;
                
            }
          


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table Copy", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


  


    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Template Tables";
       
        BindTheGrid();
    }


    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid();
    }


    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        BindTheGrid();
    }








    protected void lnkCopyExampleData_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";


        try
        {


            string sCheck = "";
            string strTableID = "";
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    strTableID = ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text;

                    RecordManager.ets_CopyTables(int.Parse(strTableID), int.Parse(ddlToAccount.Text),
                       (int)_ObjUser.UserID, true);

                    sCheck = sCheck + strTableID + ",";
                }
            }
            if (string.IsNullOrEmpty(sCheck))
            {
                ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            }
            else
            {

                if (Request.QueryString["wizard"] != null)
                {
                    Response.Redirect("~/Pages/Security/DemoEmail.aspx?wizard=yes&AccountID=" + Cryptography.Encrypt(Session["AccountID"].ToString()), false);
                }
                else
                {
                    Response.Redirect("~/Pages/Record/TableList.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;

        }
    }



    protected void lnkCopyTemplateOnly_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        
        try
        {


            string sCheck = "";
            string strTableID = "";
            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
                if (ischeck)
                {
                    strTableID = ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text;

                    RecordManager.ets_CopyTables(int.Parse(strTableID), int.Parse(ddlToAccount.Text),
                       (int)_ObjUser.UserID, false);

                    sCheck = sCheck + strTableID + ",";
                }
            }
            if (string.IsNullOrEmpty(sCheck))
            {
                ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
            }
            else
            {                  
               

                if (Request.QueryString["wizard"] != null)
                {
                    Response.Redirect("~/Pages/Security/DemoEmail.aspx?wizard=yes&AccountID=" + Cryptography.Encrypt(Session["AccountID"].ToString()), false);
                }
                else
                {
                    Response.Redirect("~/Pages/Record/TableList.aspx", false);
                }

            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;            

        }
        //}
    }

    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid();



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
                        case 2:
                            HyperLink hlView = (HyperLink)dr.FindControl("hlView");
                            sw.Write("\"" + hlView.Text + "\"");
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

   
    

    protected void ddlFromAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindTheGrid();
    }
}
