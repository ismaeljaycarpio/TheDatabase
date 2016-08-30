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

public partial class Pages_SystemData_Content : SecurePage
{
    Common_Pager _gvPager;

    User _ObjUser;
    //string _strContentKeySearch = "";

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "ContentKey";
    string _strGridViewSortDirection = "Asc";

    bool _bGlobalUser = false;

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Contents";

        try
        {


            _ObjUser = (User)Session["User"];

            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }


            if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
            {
                _bGlobalUser = true;
            }
            else
            {
                _bGlobalUser = false;
                if (_ObjUser.Email.ToLower() == SystemData.SystemOption_ValueByKey_Account("DemoAccountCreator",null,null).ToLower())
                {
                    _bGlobalUser = true;
                }
            }


            if (!IsPostBack)
            {
                PopulateContentType();
                ddlAccount.DataSource = Common.DataTableFromText("SELECT AccountID,AccountName FROM Account");
                ddlAccount.DataBind();

                ddlAccount.SelectedValue = Session["AccountID"].ToString();

                if (!_bGlobalUser)
                {
                    chkOnlyGlobal.Visible = false;
                    chkOnlyTemplate.Visible = false;
                    ddlAccount.Visible = false;
                }

                Common.PopulateAdminDropDown(ref ddlAdminArea);
                ddlAdminArea.Text = "Contents";

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
                    gvTheGrid.GridViewSortColumn = "ContentKey";
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
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
            ErrorLog theErrorLog = new ErrorLog(null, "Contents", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

        //if (Request.UserAgent.Contains("Android"))
        //{
        //    ddlAdminArea.Visible = true;
        //}
        //else
        //{
        //    ddlAdminArea.Visible = false;
        //    lblAdminArea.Text = ddlAdminArea.SelectedItem.Text;
        //}

        //Ticket 846: Removing dropdown when in mobile
        //modified by: Ismael
        ddlAdminArea.Visible = false;
        lblAdminArea.Text = ddlAdminArea.SelectedItem.Text;
        //End Ticket 846

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

                txtContentKey.Text = xmlDoc.FirstChild[txtContentKey.ID].InnerText;

                _iStartIndex = int.Parse(xmlDoc.FirstChild["iStartIndex"].InnerText);
                _iMaxRows = int.Parse(xmlDoc.FirstChild["iMaxRows"].InnerText);
                _strGridViewSortColumn = xmlDoc.FirstChild["GridViewSortColumn"].InnerText;
                _strGridViewSortDirection = xmlDoc.FirstChild["GridViewSortDirection"].InnerText;
                chkOnlyTemplate.Checked = bool.Parse(xmlDoc.FirstChild[chkOnlyTemplate.ID].InnerText);
                chkOnlyGlobal.Checked = bool.Parse(xmlDoc.FirstChild[chkOnlyGlobal.ID].InnerText);
                ddlAccount.SelectedValue = xmlDoc.FirstChild[ddlAccount.ID].InnerText;
                ddlContentType.SelectedValue = xmlDoc.FirstChild[ddlContentType.ID].InnerText;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }

    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {
        try
        {

            gvTheGrid.AllowPaging = false;
            BindTheGrid(0, _gvPager.TotalRows);


            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
            "attachment;filename=\"" + "Content" + ".csv\"");
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

                    switch (i)
                    {

                        case 4:
                            if (_bGlobalUser)
                            {
                                sw.Write(gvTheGrid.Columns[i].HeaderText);
                                if (i < iColCount - 1)
                                {
                                    sw.Write(",");
                                }
                            }
                            break;

                        case 5:
                            if (_bGlobalUser)
                            {
                                sw.Write(gvTheGrid.Columns[i].HeaderText);
                                if (i < iColCount - 1)
                                {
                                    sw.Write(",");
                                }
                            }

                            break;

                        case 6:
                            if (_bGlobalUser)
                            {
                                sw.Write(gvTheGrid.Columns[i].HeaderText);
                                if (i < iColCount - 1)
                                {
                                    sw.Write(",");
                                }
                            }

                            break;

                        case 7:
                            if (_bGlobalUser)
                            {
                                sw.Write(gvTheGrid.Columns[i].HeaderText);
                                if (i < iColCount - 1)
                                {
                                    sw.Write(",");
                                }
                            }

                            break;

                        case 8:
                            if (_bGlobalUser)
                            {
                                sw.Write(gvTheGrid.Columns[i].HeaderText);
                                if (i < iColCount - 1)
                                {
                                    sw.Write(",");
                                }
                            }

                            break;

                        default:
                            sw.Write(gvTheGrid.Columns[i].HeaderText);
                            if (i < iColCount - 1)
                            {
                                sw.Write(",");
                            }
                            break;
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
                                Label lblView = (Label)dr.FindControl("lblView");
                                sw.Write("\"" + lblView.Text + "\"");
                                if (i < iColCount - 1)
                                {
                                    sw.Write(",");
                                }
                                break;
                            case 3:
                                Label lblHeading = (Label)dr.FindControl("lblHeading");
                                sw.Write("\"" + lblHeading.Text + "\"");
                                if (i < iColCount - 1)
                                {
                                    sw.Write(",");
                                }
                                break;
                            case 4:
                                if (_bGlobalUser)
                                {
                                    Label lblStoredProcedure = (Label)dr.FindControl("lblStoredProcedure");
                                    sw.Write("\"" + lblStoredProcedure.Text + "\"");
                                    if (i < iColCount - 1)
                                    {
                                        sw.Write(",");
                                    }
                                }
                                break;

                            case 5:
                                if (_bGlobalUser)
                                {
                                    Label lblForAllAccount = (Label)dr.FindControl("lblForAllAccount");
                                    sw.Write("\"" + lblForAllAccount.Text + "\"");
                                    if (i < iColCount - 1)
                                    {
                                        sw.Write(",");
                                    }
                                }
                                break;
                            case 6:
                                if (_bGlobalUser)
                                {
                                    Label lblOnlyGlobal = (Label)dr.FindControl("lblOnlyGlobal");
                                    sw.Write("\"" + lblOnlyGlobal.Text + "\"");
                                    if (i < iColCount - 1)
                                    {
                                        sw.Write(",");
                                    }
                                }

                                break;
                            case 7:
                                if (_bGlobalUser)
                                {
                                    if (!Convert.IsDBNull(dr.Cells[i]))
                                    {
                                        sw.Write("\"" + dr.Cells[i].Text + "\"");
                                    }
                                    if (i < iColCount - 1)
                                    {
                                        sw.Write(",");
                                    }
                                }

                                break;

                            case 8:
                                if (_bGlobalUser)
                                {
                                    if (!Convert.IsDBNull(dr.Cells[i]))
                                    {
                                        sw.Write("\"" + dr.Cells[i].Text + "\"");
                                    }
                                    if (i < iColCount - 1)
                                    {
                                        sw.Write(",");
                                    }
                                }

                                break;


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
        catch
        {
            //
        }
    }


    protected void chkOnlyTemplate_CheckedChanged(object sender, EventArgs e)
    {

        lnkSearch_Click(null, null);

    }

    protected void chkOnlyGlobal_CheckedChanged(object sender, EventArgs e)
    {

        lnkSearch_Click(null, null);

    }


    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {

        lblMsg.Text = "";

        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + txtContentKey.ID + ">" + HttpUtility.HtmlEncode(txtContentKey.Text) + "</" + txtContentKey.ID + ">" +
                    " <" + ddlAccount.ID + ">" + HttpUtility.HtmlEncode(ddlAccount.SelectedValue) + "</" + ddlAccount.ID + ">" +
                     " <" + ddlContentType.ID + ">" + HttpUtility.HtmlEncode(ddlContentType.SelectedValue) + "</" + ddlContentType.ID + ">" +
                   " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
                   " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
                   " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                   " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                   " <" + chkOnlyTemplate.ID + ">" + HttpUtility.HtmlEncode(chkOnlyTemplate.Checked.ToString()) + "</" + chkOnlyTemplate.ID + ">" +
                   " <" + chkOnlyGlobal.ID + ">" + HttpUtility.HtmlEncode(chkOnlyGlobal.Checked.ToString()) + "</" + chkOnlyGlobal.ID + ">" +
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
            int? iAccountID = null;

            iAccountID = int.Parse(ddlAccount.SelectedValue);

            bool bGlobal = false;
            if (_bGlobalUser)
            {
                bGlobal = true;
            }


            bool? bOnlyTemplate = null;
            bool? bOnlyGlobal = null;

            if (bGlobal)
            {
                
                if (chkOnlyTemplate.Checked)
                {
                    bOnlyTemplate = true;
                }
                if (chkOnlyGlobal.Checked)
                {
                    bOnlyGlobal = true;
                }

                if(iAccountID!=int.Parse(Session["AccountID"].ToString()))
                {
                    bGlobal = false;
                }
            }


            gvTheGrid.DataSource = SystemData.Content_Select(null,
                txtContentKey.Text.Trim().Replace("'", "''"),
                "", "", "", "", null, null,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, iAccountID, ref iTN, bGlobal, bOnlyTemplate, bOnlyGlobal, ddlContentType.SelectedValue == "" ? null : (int?)int.Parse(ddlContentType.SelectedValue));

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
                if (!_bGlobalUser)
                {
                    _gvPager.HideAdd = true;
                }

            }

            if (!_bGlobalUser)
            {
                gvTheGrid.Columns[4].Visible = false;
                gvTheGrid.Columns[5].Visible = false;
                gvTheGrid.Columns[6].Visible = false;
                gvTheGrid.Columns[7].Visible = false;
                gvTheGrid.Columns[8].Visible = false;
            }

            //if (chkShowOnlyGlobal.Checked == false)
            //{
            //    gvTheGrid.Columns[7].Visible = false;
            //}
            //else
            //{
            //    gvTheGrid.Columns[7].Visible = true;
            //}
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
            ErrorLog theErrorLog = new ErrorLog(null, "Contents", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }



    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");


            Label lblForAllAccount = (Label)e.Row.FindControl("lblForAllAccount");


            if (DataBinder.Eval(e.Row.DataItem, "ForAllAccount").ToString() == "True")
            {
                lblForAllAccount.Text = "Yes";
            }
            else
            {
                lblForAllAccount.Text = "";
            }

            Label lblOnlyGlobal = (Label)e.Row.FindControl("lblOnlyGlobal");

            if (DataBinder.Eval(e.Row.DataItem, "ForAllAccount").ToString() == "False"
                && DataBinder.Eval(e.Row.DataItem, "AccountID")==null)
            {

                lblOnlyGlobal.Text = "Yes";
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

    public string GetEditURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&AccountID=" + Cryptography.Encrypt(ddlAccount.SelectedValue) + "&ContentID=";

    }

    public string GetViewURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&AccountID=" + Cryptography.Encrypt(ddlAccount.SelectedValue) + "&ContentID=";

    }
    public string GetAddURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&AccountID=" + Cryptography.Encrypt(ddlAccount.SelectedValue);

    }




    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Contents";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtContentKey.Text = "";
        gvTheGrid.GridViewSortColumn = "ContentKey";
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

    protected void PopulateContentType()
    {
        ddlContentType.Items.Clear();
        ddlContentType.DataSource = Common.DataTableFromText(@"SELECT * FROM ContentType");
        ddlContentType.DataBind();
        ListItem liAll = new ListItem("--All--", "");
        ddlContentType.Items.Insert(0, liAll);
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

                        SystemData.Content_Delete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Content Delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }



    //protected void chkShowOnlyGlobal_CheckedChanged(object sender, EventArgs e)
    //{
    //    BindTheGrid(0, gvTheGrid.PageSize);      

    //}

    protected void ddlAdminArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(Common.GetNavigateURL(ddlAdminArea.SelectedValue, int.Parse(Session["AccountID"].ToString())), false);
    }



    protected bool IsFiltered()
    {
        if (txtContentKey.Text != "")
        {
            return true;
        }

        return false;
    }


    protected void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
    protected void ddlContentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

}
