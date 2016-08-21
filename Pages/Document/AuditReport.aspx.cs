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
//using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

public partial class Pages_Document_AuditReport : SecurePage
{
    Common_Pager _gvPager;

    //int _iSearchCriteriaID = -1;
    //int _iStartIndex = 0;
    //int _iMaxRows = 10;
    //string _strGridViewSortColumn = "DocumentText";
    //string _strGridViewSortDirection = "ASC";
    User _objUser;

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Audit Report";
       
        try
        {


            User _objUser = (User)Session["User"];


            string strFancy = @"
                        $(function () {
                            $("".popuplink"").fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 600,
                                height: 650,
                                titleShow: false
                            });
                        });";


            ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);


            if (!IsPostBack)
            {
                PopulateTerminology();
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                { 
                   
                }


                //if (Request.QueryString["SearchCriteria"] != null)
                //{
                //    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                //}
                //else
                //{
                //    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx";
                //}

                Common.PopulateAdminDropDown(ref ddlAdminArea);
                ddlAdminArea.Text = "Audit";

                PopulateUser();
                PopulateTableDDL();
                lblTable.Visible = false;
                ddlTable1.Visible = false;

                txtDateFrom.Text = DateTime.Today.AddDays(-7).ToShortDateString();
                txtDateTo.Text = DateTime.Today.ToShortDateString();

                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvChangedLog.PageSize =50; }
                

                BindTheGrid(0, gvChangedLog.PageSize);
                
            }
            else
            {
            }

            GridViewRow gvr = gvChangedLog.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("CL_Pager");


        }
        catch (Exception ex)
        {
           
            lblMsg.Text = ex.Message;
        }


        if (Request.UserAgent.Contains("Android"))
        {
            ddlAdminArea.Visible = true;
        }
        else
        {
            ddlAdminArea.Visible = false;
            lblAdminArea.Text = ddlAdminArea.SelectedItem.Text;
        }

    }

    protected void PopulateTerminology()
    {
        gvChangedLog.Columns[5].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), gvChangedLog.Columns[5].HeaderText, gvChangedLog.Columns[5].HeaderText);
    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {

        lblMsg.Text = "";

        if (txtDateFrom.Text == "")
        {
            if (txtDateTo.Text != "")
            {
                lblMsg.Text = "Please enter the Changes From date."; ;
                txtDateFrom.Focus();
                return;
            }
        }
        if (txtDateTo.Text == "")
        {
            if (txtDateFrom.Text != "")
            {
                lblMsg.Text = "Please enter the Changes To date."; ;
                txtDateFrom.Focus();
                return;
            }
        }


        DateTime? dtDateFrom = null;
        DateTime? dtDateTo = null;

        if (txtDateFrom.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtDateFrom.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                dtDateFrom = dtTemp;
            }
        }
        if (txtDateTo.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtDateTo.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                dtDateTo = dtTemp;
            }
        }


        if (dtDateFrom != null || dtDateTo != null)
        {
            if (dtDateFrom > dtDateTo)
            {
                lblMsg.Text = "Changes From date can not be greater than Chnages To date "; ;
                txtDateFrom.Focus();
                return;

            }
        }



    
        try
        {
            int iTN = 0;
           

            gvChangedLog.DataSource = RecordManager.Audit_Summary( int.Parse(Session["AccountID"].ToString()), ddlTable.SelectedValue,
                 dtDateFrom,
                dtDateTo.Value.AddDays(1),
                ddlTable1.SelectedValue == "-1" ? null : (int?)int.Parse(ddlTable1.SelectedValue),
                txtSearch.Text.Trim(),
                iStartIndex, iMaxRows,
                 ddlUser.SelectedValue == "-1" ? null : (int?)int.Parse(ddlUser.SelectedValue),
                ref iTN);

            gvChangedLog.VirtualItemCount = iTN;
            gvChangedLog.DataBind();
            if (gvChangedLog.TopPagerRow != null)
                gvChangedLog.TopPagerRow.Visible = true;

            GridViewRow gvr = gvChangedLog.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("CL_Pager");                
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

    protected void gvChangedLog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
                    e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
                }

                HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");
                DateTime dtUpdateDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "DateAdded"));
                hlView.NavigateUrl = "AuditReportDetail.aspx?UpdatedDate=" + Server.UrlEncode(dtUpdateDate.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                    + "&PK=" + Cryptography.Encrypt( Convert.ToString(DataBinder.Eval(e.Row.DataItem, "PrimaryKeyValue")))
                    + "&TableName=" + Cryptography.Encrypt (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "TableName")));


                string strColumnList = DataBinder.Eval(e.Row.DataItem, "ColumnList").ToString();
                string[] arrColumnList = strColumnList.Split(',');

                Label lblColumnList = (Label)e.Row.FindControl("lblColumnList");

                if (arrColumnList.Length > 3)
                {
                    //get first 3 names
                    int i = 0;
                    string strThreeeColumns = "";
                    foreach (string aColumn in arrColumnList)
                    {
                        if (i == 0)
                            strThreeeColumns = aColumn;
                        if (i == 1)
                            strThreeeColumns = strThreeeColumns + "," + aColumn;

                        if (i == 2)
                            strThreeeColumns = strThreeeColumns + " and " + aColumn;

                        i = i + 1;

                        if (i == 3)
                            break;
                    }

                    lblColumnList.Text = arrColumnList.Length + " fields including " + strThreeeColumns;

                }
                else
                {
                    lblColumnList.Text = strColumnList;
                }

            }

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }

    protected void gvChangedLog_PreRender(object sender, EventArgs e)
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
   
 

    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable1.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null,  true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());

        ddlTable1.DataBind();
        //if (iTN == 0)
        //{
        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("All", "-1");
        ddlTable1.Items.Insert(0, liAll);
        //}


    }
    
    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
      

        BindTheGrid(0, gvChangedLog.PageSize);      

    }





    protected void CL_Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Audit";
        BindTheGrid(0, _gvPager.TotalRows);
    }



    protected void CL_Pager_OnCustomExportPDF(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (hfSaveToDocument.Value == "true")
        {
            ExportPDFAsDocument();

        }
        else
        {
            ExportPDF();
        }

    }

    private void ExportPDFAsDocument()
    {

        //do the custom save
        //lets get the Document Type
        try
        {
            DataTable theDataTable = Common.DataTableFromText("SELECT * FROM DocumentType WHERE DocumentTypeName='Audit Report' AND AccountID=" + Session["AccountID"].ToString() );

            if (theDataTable.Rows.Count > 0)
            {
                int iDocumentTypeID = int.Parse(theDataTable.Rows[0]["DocumentTypeID"].ToString());

                gvChangedLog.AllowPaging = false;
                CL_Pager_BindTheGridToExport(null, null);

                string strFileName = "Audit_Report_" + ddlTable.SelectedItem.Text + ".pdf";
                


                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //BindGridAgainToExport();                

                for (int i = 0; i < gvChangedLog.Columns.Count; i++)
                {
                    if (string.IsNullOrEmpty(gvChangedLog.Columns[i].HeaderText))
                        gvChangedLog.Columns[i].Visible = false;
                }
                gvChangedLog.RenderControl(hw);

                StringReader sr = new StringReader(sw.ToString());
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

                MemoryStream ms = new MemoryStream();
                PdfWriter w = PdfWriter.GetInstance(pdfDoc, ms);

                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                _objUser = (User)Session["User"];

                string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;
               
                string strFolder = Server.MapPath("~\\Pages\\Document\\Uploads");

                string strPath = strFolder + "\\" + strUniqueName;

                System.IO.FileStream file = System.IO.File.Create(strPath);
                file.Write(ms.ToArray(), 0, ms.ToArray().Length);
                file.Close();


                Document theDocument = new Document(null, int.Parse(Session["AccountID"].ToString()), strFileName, iDocumentTypeID,
                   strUniqueName, strFileName, DateTime.Now, null, null, _objUser.UserID, null);

                DocumentManager.ets_Document_Insert(theDocument);

                gvChangedLog.AllowPaging = true;
                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvChangedLog.PageSize = 50; }


                BindTheGrid(0, gvChangedLog.PageSize);


                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Audit report has beend saved as a Document of Audit Report Document Type.');", true);


            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message + " " + ex.StackTrace;

        }




    }
    private void ExportPDF()
    {

        gvChangedLog.AllowPaging = false;

        CL_Pager_BindTheGridToExport(null, null);

        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition",
         "attachment;filename=\"" + "Audit_Report_" + ddlTable.SelectedItem.Text + ".pdf\"");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        //BindGridAgainToExport();


        for (int i = 0; i < gvChangedLog.Columns.Count; i++)
        {
            if (string.IsNullOrEmpty(gvChangedLog.Columns[i].HeaderText))
                gvChangedLog.Columns[i].Visible = false;
        }
        gvChangedLog.RenderControl(hw);

        StringReader sr = new StringReader(sw.ToString());
        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
       

        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();

        Response.Write(pdfDoc);
        Response.End();
    }


    protected void CL_Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void CL_Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        ddlUser.SelectedIndex = 0;
        ddlTable.SelectedIndex = 0;
        ddlTable1.SelectedIndex = 0;
        lnkSearch_Click(null, null);
    }


    protected bool IsFiltered()
    {
        if (txtSearch.Text != "" || ddlUser.SelectedIndex != 0 || ddlTable.SelectedIndex != 0 || ddlTable1.SelectedIndex != 0
            || txtDateFrom.Text != "" || txtDateTo.Text != "")
        {
            return true;
        }

        return false;
    }


   


    protected void CL_Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvChangedLog.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Document.csv");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);


        int iColCount = gvChangedLog.Columns.Count;
        for (int i = 0; i < iColCount; i++)
        {
            if (string.IsNullOrEmpty(gvChangedLog.Columns[i].HeaderText))
            {
            }
            else
            {
                if (i != 6)
                {
                    sw.Write(gvChangedLog.Columns[i].HeaderText);
                    if (i < iColCount - 1)
                    {

                        sw.Write(",");

                    }
                }
            }
        }

        sw.Write(sw.NewLine);

        // Now write all the rows.
        foreach (GridViewRow dr in gvChangedLog.Rows)
        {

            for (int i = 0; i < iColCount; i++)
            {
                if (string.IsNullOrEmpty(gvChangedLog.Columns[i].HeaderText))
                {
                }
                else
                {
                    switch (i)
                    {
                        case 1:
                            Label lblTableName1 = (Label)dr.FindControl("lblTableName1");
                            sw.Write("\"" + lblTableName1.Text + "\"");
                            break;
                        case 2:
                            Label UpdateDate = (Label)dr.FindControl("UpdateDate");
                            sw.Write("\"" + UpdateDate.Text + "\"");
                            break;
                        case 3:
                            Label lblUser = (Label)dr.FindControl("lblUser");
                            sw.Write("\"" + lblUser.Text + "\"");
                            break;
                        case 4:
                            Label lblColumnList = (Label)dr.FindControl("lblColumnList");
                            sw.Write("\"" + lblColumnList.Text + "\"");
                            break;

                        case 5:
                            Label lblTableName = (Label)dr.FindControl("lblTableName");
                            sw.Write("\"" + lblTableName.Text + "\"");
                            break;
                        case 6:
                           //
                            break;

                        case 7:
                            Label lblResonForChange = (Label)dr.FindControl("lblResonForChange");
                            sw.Write("\"" + lblResonForChange.Text + "\"");
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
                        if (i != 6)
                        {
                            sw.Write(",");
                        }
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



    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlTable.SelectedValue == "Record")
        {
            lblTable.Visible = true;
            ddlTable1.Visible = true;

        }
        else
        {
            ddlTable1.SelectedIndex = 0;
            lblTable.Visible = false;
            ddlTable1.Visible = false;
        }
        lnkSearch_Click(null, null);
    }
    protected void ddlTable1_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);

    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);

    }

    protected void PopulateUser()
    {
        int iTN = 0;
        ddlUser.DataSource = SecurityManager.User_Select(null, "", "",
       "", "", "", true, null, null,
       int.Parse(Session["AccountID"].ToString()),
        "FirstName", "ASC", null, null, ref iTN);
        ddlUser.DataBind();
        ListItem liAll = new ListItem("All", "-1");
        ddlUser.Items.Insert(0, liAll);

    }

    protected void ddlAdminArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(Common.GetNavigateURL(ddlAdminArea.SelectedValue, int.Parse(Session["AccountID"].ToString())), false);
    }

}
