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


public partial class Pages_Reports_Document : System.Web.UI.Page
{
    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "DocumentText";
    string _strGridViewSortDirection = "ASC";
    string _strAccountID = "-1";
    string _strFilesLocation = "";
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Documents and Reports";
       
        try
        {
            _strAccountID = Request.QueryString["AccountID"].ToString();

            _strFilesLocation = Session["FilesLocation"].ToString();

            if (!IsPostBack)
            {
                               
                

                PopulateDocumentType();
                PopulateTableDDL();
                             


             

                
                     gvTheGrid.PageSize=30;         
                    gvTheGrid.GridViewSortColumn = "DocumentText";
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
            ErrorLog theErrorLog = new ErrorLog(null, "Report", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
           

            gvTheGrid.DataSource = DocumentManager.ets_Document_Select(null,int.Parse(_strAccountID),
                txtDocumentText.Text.Trim(),ddlDocumentType.SelectedValue=="-1"?null:(int?) int.Parse(ddlDocumentType.SelectedValue),
                txtDateFrom.Text==""?null: (DateTime?)DateTime.ParseExact(txtDateFrom.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
                txtDateTo.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
                null, null,null,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN,
                ddlTable.SelectedValue == "-1" ? null : (int?)int.Parse(ddlTable.SelectedValue), "",true,null);

            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;
            
            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                //_gvPager.AddURL = GetAddURL();
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
            ErrorLog theErrorLog = new ErrorLog(null, "Document", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }
    protected void PopulateDocumentType()
    {
        ddlDocumentType.Items.Clear();
        ddlDocumentType.DataSource = Common.DataTableFromText("SELECT DocumentTypeID,DocumentTypeName FROM DocumentType WHERE AccountID=" + _strAccountID);
        ddlDocumentType.DataBind();
        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("All", "-1");
        ddlDocumentType.Items.Insert(0, liSelect);     
    }

    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(_strAccountID),
                null, null,  true,
                "st.TableName", "ASC",
                null, null, ref  iTN, "");

        ddlTable.DataBind();
        //if (iTN == 0)
        //{
        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("All", "-1");
        ddlTable.Items.Insert(0, liAll);
        //}


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

  


    public string GetFileURL(string strDocumentID)
    {
        Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));

        if (theDocument != null)
        {
            if (theDocument.FileUniqename != "")
            {
                return _strFilesLocation + "/UserFiles/Documents/" + theDocument.FileUniqename;
            }
            else
            {
                return "http://" + Request.Url.Authority + Request.ApplicationPath + "/DocReports/Report.aspx?ReportID=" + theDocument.DocumentID.ToString();
            }

        }

        return "#";

    }

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Document";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtDocumentText.Text = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        ddlDocumentType.SelectedIndex = 0;
        ddlTable.SelectedIndex = 0;
        gvTheGrid.GridViewSortColumn = "DocumentText";
        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;

        lnkSearch_Click(null, null);
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
                        DocumentManager.ets_Document_Delete(int.Parse(sTemp));
                        //SystemData.SystemOption_Delete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Report", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }


    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Report.csv");
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
                        case 3:
                            HyperLink hlView = (HyperLink)dr.FindControl("hlView");
                            sw.Write("\"" + hlView.Text + "\"");
                            break;
                        case 4:
                            Label lblOptionValue = (Label)dr.FindControl("lblOptionValue");
                            sw.Write("\"" + lblOptionValue.Text + "\"");
                            break;
                        case 5:
                            Label lblOptionNotes = (Label)dr.FindControl("lblOptionNotes");
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



    protected void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);

    }



    protected bool IsFiltered()
    {
        if (txtDocumentText.Text != "" || ddlTable.SelectedIndex != 0 || ddlDocumentType.SelectedIndex != 0
            || txtDateFrom.Text!="" || txtDateTo.Text !="")
        {
            return true;
        }

        return false;
    }

    //protected void ddlAdminArea_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Response.Redirect(Common.GetNavigateURL(ddlAdminArea.SelectedValue,int.Parse(_strAccountID)), false);
    //}

}
