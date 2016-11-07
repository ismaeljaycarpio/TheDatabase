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

public partial class Pages_Security_AccountList : SecurePage
{

   
    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "AccountName";
    string _strGridViewSortDirection = "ASC";

    public string GetEditURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&accountid=";

    }


    public string GetViewURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&accountid=";

    }

    public string GetAddURL()
    {

        //return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/SystemSignUp.aspx";
    }


    public string GetSignedURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Usage.aspx?Type=" + Cryptography.Encrypt("S") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&accountid=";

    }

    public string GetUploadedURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Usage.aspx?Type=" + Cryptography.Encrypt("U") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&accountid=";

    }

    public string GetUseCaption(string strAccountID)
    {

        if (strAccountID==Session["AccountID"].ToString())
       {
            return "Current Account";
       }
       else
       {
           return "Use this Account";
       }

    }

    public System.Drawing.Color GetUseColor(string strAccountID)
    {

        if (strAccountID == Session["AccountID"].ToString())
        {
            return System.Drawing.Color.Red;
        }
        else
        {
            return System.Drawing.Color.Blue;
        }

    }



    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
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
                   " <" + txtAccountNameSearch.ID + ">" + HttpUtility.HtmlEncode(txtAccountNameSearch.Text) + "</" + txtAccountNameSearch.ID + ">" +
                   " <" + txtNameSearch.ID + ">" + HttpUtility.HtmlEncode(txtNameSearch.Text) + "</" + txtNameSearch.ID + ">" +
                   " <" + txtEmailSearch.ID + ">" + HttpUtility.HtmlEncode(txtEmailSearch.Text) + "</" + txtEmailSearch.ID + ">" +
                   " <" + chkIsActive.ID + ">" + HttpUtility.HtmlEncode(chkIsActive.Checked.ToString()) + "</" + chkIsActive.ID + ">" +                   
                   " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
                   " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
                   " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                   " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
            Session["SCid_AccountList"] = _iSearchCriteriaID;
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }

        //End Searchcriteria




        try
        {
            int iTN = 0;



            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = SecurityManager.Account_Summary(null,
               txtAccountNameSearch.Text.Trim().Replace("'", "''"),
               txtNameSearch.Text.Trim().Replace("'", "''"),
              txtEmailSearch.Text.Trim().Replace("'", "''"),
              string.Empty,!chkIsActive.Checked, null, null,
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
                if (ViewState[gvTheGrid.ID + "PageIndex"] != null)
                    _gvPager.PageIndex = int.Parse(ViewState[gvTheGrid.ID + "PageIndex"].ToString());
                //_gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
                _gvPager.PageSize = gvTheGrid.PageSize;
                _gvPager.TotalRows = iTN;

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
            ErrorLog theErrorLog = new ErrorLog(null, "Account", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Accounts";

        try
        {

            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
                {
                    Response.Redirect("~/Default.aspx", false); 
                    //User objUser = (User)Session["User"];

                    //if (objUser.Email.ToLower() == SystemData.SystemOption_ValueByKey("DemoAccountCreator").ToLower())
                    //{

                    //}
                    //else
                    //{

                    //    Response.Redirect("~/Default.aspx", false); 
                    //}


                   
                
                }

                if (Request.QueryString["SearchCriteria"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
                }
                else if (Session["SCid_AccountList"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Session["SCid_AccountList"].ToString()));
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
                    gvTheGrid.GridViewSortColumn = "AccountName";
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                }

                gvTheGrid.Attributes.Add("bordercolor", "#acc0e9");
               
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
            ErrorLog theErrorLog = new ErrorLog(null, "Account", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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

                txtAccountNameSearch.Text = xmlDoc.FirstChild[txtAccountNameSearch.ID].InnerText;
                txtNameSearch.Text = xmlDoc.FirstChild[txtNameSearch.ID].InnerText;
                txtEmailSearch.Text = xmlDoc.FirstChild[txtEmailSearch.ID].InnerText;
                chkIsActive.Checked = bool.Parse( xmlDoc.FirstChild[chkIsActive.ID].InnerText);


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

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Accounts";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtAccountNameSearch.Text = "";
        txtEmailSearch.Text = "";
        txtNameSearch.Text = "";
        chkIsActive.Checked = false;
        gvTheGrid.GridViewSortColumn = "AccountName";
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
            //_gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
            //if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
            //{
            //    BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
            //}
        }

    }


    protected void Pager_UnDeleteAction(object sender, EventArgs e)
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
            UnDeleteItem(sCheck);
            BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
            //_gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
            //if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
            //{
            //    BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
            //}
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

                        SecurityManager.Account_Delete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Account", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete Account has failed!');", true);
        }
    }



    private void UnDeleteItem(string keys)
    {
        try
        {
            if (!string.IsNullOrEmpty(keys))
            {

                foreach (string sTemp in keys.Split(','))
                {
                    if (!string.IsNullOrEmpty(sTemp))
                    {

                        SecurityManager.Account_UnDelete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Account", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete Account has failed!');", true);
        }
    }


    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Accounts.csv");
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

                        case 6:
                            Label lblAccountTypeID = (Label)dr.FindControl("lblAccountTypeID");
                            sw.Write("\"" + lblAccountTypeID.Text + "\"");
                            break;

                        case 8:
                            HyperLink hlSignedInCount = (HyperLink)dr.FindControl("hlSignedInCount");
                            sw.Write("\"" + hlSignedInCount.Text + "\"");
                            break;

                        case 9:
                            HyperLink hlUploadedCount = (HyperLink)dr.FindControl("hlUploadedCount");
                            sw.Write("\"" + hlUploadedCount.Text + "\"");
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


    protected void gvTheGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Use")
        {
            try
            {
                Session["AccountID"] = e.CommandArgument.ToString();
                BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
                //_gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
                Response.Redirect(Request.RawUrl,false);
            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Account Switch", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;

                //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Switch to another account is failed!');", true);
            }

        }
    }

    protected bool IsFiltered()
    {
        if (txtAccountNameSearch.Text != "" || txtEmailSearch.Text != "" || txtNameSearch.Text != ""
            || chkIsActive.Checked != false)
        {
            return true;
        }

        return false;
    }


    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblAccountTypeID = (Label)e.Row.FindControl("lblAccountTypeID");
            Label lblCreatedByWizard = (Label)e.Row.FindControl("lblCreatedByWizard");

            if (DataBinder.Eval(e.Row.DataItem, "CreatedByWizard").ToString() == "True")
            {
                lblCreatedByWizard.Text = "Yes";
            }
            else
            {
                lblCreatedByWizard.Text = "";
            }

            if (DataBinder.Eval(e.Row.DataItem, "AccountTypeID").ToString() == "1")
            {
                lblAccountTypeID.Text = "Free";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "AccountTypeID").ToString() == "2")
            {
                lblAccountTypeID.Text = "Large";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "AccountTypeID").ToString() == "3")
            {
                lblAccountTypeID.Text = "Corporate";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "AccountTypeID").ToString() == "4")
            {
                lblAccountTypeID.Text = "Custom";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "AccountTypeID").ToString() == "5")
            {
                lblAccountTypeID.Text = "Small";
            }
            else if (DataBinder.Eval(e.Row.DataItem, "AccountTypeID").ToString() == "4")
            {
                lblAccountTypeID.Text = "Medium";
            }


        }

    }
    
}
