using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class Pages_Record_RecordHistory :SecurePage
{
    Common_Pager _gvCL_Pager;

    int _iCLColumnCount = 0;

    Record _theRecord=null;
    int _iCLStartIndex = 0;
    int _iCLMaxRows = 0;
    int _iCLTN = 0;
    Table _theTable = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request.QueryString["id"]!=null)
        {
            _theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(Request.QueryString["id"].ToString()));
            _theTable = RecordManager.ets_Table_Details((int)_theRecord.TableID);
        }
        if(!IsPostBack)
        {
            if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
            {
                gvChangedLog.PageSize = int.Parse(Session["GridPageSize"].ToString());
            }

            if (_theRecord!=null)
            {
                TheDatabaseS.spAuditRawToAudit((int)_theRecord.RecordID);

                BindTheChangedLogGrid(0, gvChangedLog.PageSize);
            }
          
        }
        GridViewRow gvrCL = gvChangedLog.TopPagerRow;
        if (gvrCL != null)
            _gvCL_Pager = (Common_Pager)gvrCL.FindControl("CL_Pager");

        string strFancy = @"$(function () {
            $("".popuplink"").fancybox({
                scrolling: 'auto',
                type: 'iframe',             
                titleShow: false
            });
        });";

        ScriptManager.RegisterStartupScript(upCommon, upCommon.GetType(), "Pages_Record_RecordHistoryFancyBox", strFancy, true);
    }


    protected void CL_Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheChangedLogGrid(_gvCL_Pager.StartIndex, _gvCL_Pager.PageSize);
    }

    protected void CL_Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvCL_Pager.ExportFileName = "Record Change Log ";
        BindTheChangedLogGrid(0, _gvCL_Pager.TotalRows);
    }
    protected void CL_Pager_OnApplyFilter(object sender, EventArgs e)
    {
        //_gvCL_Pager.ExportFileName = "Sensor Change Log";
        BindTheChangedLogGrid(0, gvChangedLog.PageSize);
    }
    protected void CL_Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvChangedLog.AllowPaging = false;
        BindTheChangedLogGrid(0, _gvCL_Pager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=RecordChangedLog.csv");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        DataTable dtTemp = (DataTable)gvChangedLog.DataSource;

        int iColCount = dtTemp.Columns.Count;
        for (int i = 0; i < iColCount - 1; i++)
        {
            if (string.IsNullOrEmpty(dtTemp.Columns[i].ColumnName))
            {
            }
            else
            {
                sw.Write(dtTemp.Columns[i].ColumnName);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
        }

        sw.Write(sw.NewLine);

        // Now write all the rows.
        foreach (DataRow dr in dtTemp.Rows)
        {

            for (int i = 0; i < iColCount - 1; i++)
            {
                if (string.IsNullOrEmpty(dtTemp.Columns[i].ColumnName))
                {
                }
                else
                {


                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write("\"" + dr[i].ToString() + "\"");
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
    protected void gvChangedLog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");
                DateTime dtUpdateDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "DateAdded"));
                hlView.NavigateUrl = "AuditDetail.aspx?UpdatedDate=" + Server.UrlEncode(dtUpdateDate.ToString("yyyy-MM-dd HH:mm:ss.fff")) + "&RecordID=" + _theRecord.RecordID.ToString();

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
            //lblMsg.Text = ex.Message;
        }
    }

    protected void BindTheChangedLogGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
            int iTN = 0;

            getLastUpdatedInfo();
            ViewState[gvChangedLog.ID + "PageIndex"] = (iStartIndex / gvChangedLog.PageSize) + 1;
            gvChangedLog.DataSource = RecordManager.Record_Audit_Summary(
                   (int)_theRecord.RecordID, iStartIndex, iMaxRows, ref  iTN);

            gvChangedLog.VirtualItemCount = iTN;

            _iCLColumnCount = 4;

            _iCLStartIndex = iStartIndex;
            _iCLMaxRows = iMaxRows;
            _iCLTN = iTN;



            gvChangedLog.DataBind();
            if (gvChangedLog.TopPagerRow != null)
                gvChangedLog.TopPagerRow.Visible = true;
            GridViewRow gvr = gvChangedLog.TopPagerRow;
            if (gvr != null)
            {
                _gvCL_Pager = (Common_Pager)gvr.FindControl("CL_Pager");
                if (ViewState[gvChangedLog.ID + "PageIndex"] != null)
                    _gvCL_Pager.PageIndex = int.Parse(ViewState[gvChangedLog.ID + "PageIndex"].ToString());

                _gvCL_Pager.PageSize = gvChangedLog.PageSize;
                _gvCL_Pager.TotalRows = iTN;
            }

            if (_theTable.ReasonChangeType == "none" || _theTable.ReasonChangeType == "")
            {
                gvChangedLog.Columns[4].Visible = false;
            }


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Record Detail Change Log", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void getLastUpdatedInfo()
    {
        if (_theRecord != null)
        {
            lblCreatedBy.Text = RecordManager.GetUserDisplayName("[Name]", Convert.ToString(_theRecord.EnteredBy));
            lblDateCreated.Text = _theRecord.DateAdded.ToString();

            if (_theRecord.LastUpdatedUserID != null)
            {
                lblUpdatedBy.Text = RecordManager.GetUserDisplayName("[Name]", Convert.ToString(_theRecord.LastUpdatedUserID));
                lblDateUpdated.Text = _theRecord.DateUpdated.ToString();
            }

            lblUpdatedBy.Visible = (lblUpdatedBy.Text.Trim() != "");
            lblUpdatedByText.Visible = (lblUpdatedBy.Text.Trim() != "");

            lblDateUpdated.Visible = (lblUpdatedBy.Text.Trim() != "");
            lblDateUpdatedText.Visible = (lblUpdatedBy.Text.Trim() != "");
        }
    }
}