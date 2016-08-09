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

    public string GetAddURL()
    {

        if (SecurityManager.CanThisAccountAddUser(int.Parse(ddlAccount.Text)))
        {
            return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());
        }
        else
        {
            return "javascript:alert('You have reached the maximum number of users allowed for your account type.  In order to add a new user you must either delete an existing user or upgrade your account. See My Account page for options.');window.location.href='" + "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountTypeChange.aspx?type=renew" + "'";
                        
        }
        
       

    }

    public string GetEditURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&userid=";

    }


    public string GetViewURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&userid=";

    }

    public string GetAccountViewURL()
    {

        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&accountid=";

    }
  


    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e) 
    {

        if (Session["AccountID"] == null)
            return;

        Title = "Users";
        try
        {
          

            User ObjUser = (User)Session["User"];
           
            if (!IsPostBack)
            {

                PopulateAccountsDDL();

                ddlAccount.Text = Session["AccountID"].ToString();

                if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                {Response.Redirect("~/Default.aspx", false); }


                if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                {
                    lblAccount.Visible = true;
                    ddlAccount.Visible = true;
                }
                else
                {
                    lblAccount.Visible = false;
                    ddlAccount.Visible = false;
                }

                Common.PopulateAdminDropDown(ref ddlAdminArea);
                ddlAdminArea.Text = "Users";

                if (Request.QueryString["SearchCriteria"] != null)
                {
                    PopulateSearchCriteria(int.Parse( Cryptography.Decrypt(   Request.QueryString["SearchCriteria"].ToString())));
                }


                
                //PopulateAccountDDL();
                int iTN = 0;
                
                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }

                if (Request.QueryString["SearchCriteria"] != null)
                {
                    gvTheGrid.PageSize = _iMaxRows;
                    gvTheGrid.GridViewSortColumn = _strGridViewSortColumn;
                    if (_strGridViewSortDirection == "ASC")
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
                    gvTheGrid.GridViewSortColumn = "FirstName";
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
            ErrorLog theErrorLog = new ErrorLog(null, "Users Load", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
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


    protected void PopulateSearchCriteria(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                txtFirstName.Text = xmlDoc.FirstChild[txtFirstName.ID].InnerText;
                txtLastName.Text = xmlDoc.FirstChild[txtLastName.ID].InnerText;
                txtEmailSearch.Text = xmlDoc.FirstChild[txtEmailSearch.ID].InnerText;
                chkIsActive.Checked = bool.Parse(xmlDoc.FirstChild[chkIsActive.ID].InnerText);
                ddlAccount.Text = xmlDoc.FirstChild[ddlAccount.ID].InnerText;
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
    protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        try
        {


            lblMsg.Text = "";

            //SearchCriteria 
            try
            {
                string xml = null;
                xml = @"<root>" +
                       " <" + txtFirstName.ID + ">" + HttpUtility.HtmlEncode(txtFirstName.Text) + "</" + txtFirstName.ID + ">" +
                       " <" + txtLastName.ID + ">" + HttpUtility.HtmlEncode(txtLastName.Text) + "</" + txtLastName.ID + ">" +
                       " <" + txtEmailSearch.ID + ">" + HttpUtility.HtmlEncode(txtEmailSearch.Text) + "</" + txtEmailSearch.ID + ">" +
                       " <" + chkIsActive.ID + ">" + HttpUtility.HtmlEncode(chkIsActive.Checked.ToString()) + "</" + chkIsActive.ID + ">" +
                       " <" + ddlAccount.ID + ">" + HttpUtility.HtmlEncode(ddlAccount.Text) + "</" + ddlAccount.ID + ">" +
                       " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
                       " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
                       " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                       " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                      "</root>";

                SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
                _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }

            //End Searchcriteria
            


            int iTN = 0;

            gvTheGrid.DataSource = SecurityManager.User_Select(null,
                txtFirstName.Text.Replace("'", "''"),
            txtLastName.Text.Replace("'", "''"),"", txtEmailSearch.Text.Replace("'", "''"), 
            string.Empty, 
            !chkIsActive.Checked,
            null, null,
            int.Parse(ddlAccount.Text),
            gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
            iStartIndex, iMaxRows, ref iTN);

         
            gvTheGrid.VirtualItemCount = iTN;

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


            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
            {
                gvTheGrid.TopPagerRow.Visible = true;               
            }
            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.AddURL = GetAddURL();
                _gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);

                string strVirtualUsersTableID = SystemData.SystemOption_ValueByKey_Account("VirtualUsersTableID", int.Parse(Session["AccountID"].ToString()), null);

                    if(strVirtualUsersTableID!="")
                    {
                        _gvPager.ShowUploadFile=true;
                        _gvPager.UploadFileURL="~/Pages/Record/RecordUpload.aspx?SearchCriteriaID="+Cryptography.Encrypt("-1")+"&TableID=" + Cryptography.Encrypt(strVirtualUsersTableID);

                    }

                    //string strShowFirstLast = SystemData.SystemOption_ValueByKey_Account("Show Record First-Last Buttons", int.Parse(Session["AccountID"].ToString()), null);

                    //if (strShowFirstLast != "" && strShowFirstLast.ToLower() == "yes")
                    //{
                    //    _gvPager.HidePageSizeButton = false;
                    //}
                    //else
                    //{
                    //    _gvPager.HidePageSizeButton = true;
                    //}

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
        gvTheGrid.Columns[2].Visible = false;
        gvTheGrid.Columns[3].Visible = false;
        gvTheGrid.Columns[4].Visible = true;
        gvTheGrid.Columns[5].Visible = true;

    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {

        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtEmailSearch.Text = "";
        chkIsActive.Checked = false;
        gvTheGrid.GridViewSortColumn = "FirstName";
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
        string strID="";
        lblMsg.Text = "";
        try
        {
            if (!string.IsNullOrEmpty(keys))
            {

                foreach (string sTemp in keys.Split(','))
                {
                    if (!string.IsNullOrEmpty(sTemp))
                    {
                       
                        strID = sTemp;
                        User theUser=SecurityManager.User_Details(int.Parse(strID));
                        UserRole theUserRole = SecurityManager.GetUserRole((int)theUser.UserID, int.Parse(Session["AccountID"].ToString()));

                        if (!(bool)theUserRole.IsAccountHolder)
                        {
                            SecurityManager.User_Delete(int.Parse(sTemp));
                        }

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Users Delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            User oUser = SecurityManager.User_Details(int.Parse(strID));            

            if (ex.Message.IndexOf("Record_EnteredBy") > -1)
            {
                lblMsg.Text = "Delete failed! User -" + oUser.FirstName + " " + oUser.LastName + "- has Records, please remove those Records.";
            }
            else if (ex.Message.IndexOf("FK_Batch_User") > -1)
            {
                lblMsg.Text = "Delete failed! User -" + oUser.FirstName + " " + oUser.LastName + "- has batches.";
            }

            else
            {
                lblMsg.Text = ex.Message;               
                
            }

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
        "attachment;filename=Users.csv");
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
                if (i != 4 && i != 5)
                {
                    sw.Write(gvTheGrid.Columns[i].HeaderText);
                }
                if (i < iColCount - 1 && i!=4 && i!=5)
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
                        case 3:
                            HyperLink hlView2 = (HyperLink)dr.FindControl("hlView2");
                            sw.Write("\"" + hlView2.Text + "\"");
                            break;

                        case 4:
                             //do nothing
                            break;
                        case 5:
                            //do nothing
                            break;
                        default:
                            if (!Convert.IsDBNull(dr.Cells[i]))
                            {
                                sw.Write("\"" + (dr.Cells[i].Text.Trim() == "&nbsp;" ? "" : dr.Cells[i].Text) + "\"");
                            }

                            break;
                    }

                    if (i < iColCount - 1 && i!=4 && i!=5)
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


    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");


            if (DataBinder.Eval(e.Row.DataItem, "IsAccountHolder").ToString() == "True"
                && DataBinder.Eval(e.Row.DataItem, "IsPrimaryAccount").ToString() == "True")
            {
                e.Row.Cells[7].Text = "Yes";
            }
            else
            {
                e.Row.Cells[7].Text = "No";
            }

//            Label lblUserRole =(Label) e.Row.FindControl("lblUserRole");
//            if (lblUserRole != null)
//            {
//                if (DataBinder.Eval(e.Row.DataItem, "IsAdvancedSecurity").ToString() == "True")
//                {
//                    lblUserRole.Text = "Advanced";
//                }
//                else
//                {
//                    string strRoleName = Common.GetValueFromSQL(@"SELECT TOP 1 R.Role FROM [Role] R
//                        INNER JOIN UserRole UR ON R.RoleID=UR.RoleID
//                        WHERE UR.UserID=" + DataBinder.Eval(e.Row.DataItem, "UserID").ToString() 
//                                          + @" AND UR.AccountID=" + Session["AccountID"].ToString());
//                    lblUserRole.Text = strRoleName;
//                }
//            }


        }

    }


    protected void ddlAdminArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(Common.GetNavigateURL(ddlAdminArea.SelectedValue,int.Parse(Session["AccountID"].ToString())), false);
    }

    protected bool IsFiltered()
    {
        if (txtFirstName.Text != "" || txtLastName.Text != "" || txtEmailSearch.Text != ""
            || chkIsActive.Checked != false)
        {
            return true;
        }

        return false;
    }

    protected void PopulateAccountsDDL()
    {
        int iTN = 0;

        DataTable dtTemp = SecurityManager.Account_Summary(null, "", "", "", "", null, null, null,
            "AccountName", "ASC", null, null, ref iTN);

        ddlAccount.DataSource = dtTemp;
        ddlAccount.DataBind();


    }


    protected void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
}
