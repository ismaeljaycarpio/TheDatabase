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


public partial class Pages_Document_Report : SecurePage
{
    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "DocumentText";
    string _strGridViewSortDirection = "ASC";
   

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Reports";
       
        try
        {
            //if (Request.QueryString["type"] == null)
            //{
            //    Response.Redirect("~/Default.aspx");
            //    return;
            //}

            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                PopulateMenuDDL(int.Parse(Session["AccountID"].ToString()));
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                { 
                   hlDocumentTypeEdit.Visible=false; 
                }

                if (Request.QueryString["SSearchCriteriaID"] != null)
                {

                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();
                }
                else
                {
                    //hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString();
                    hlBack.NavigateUrl = "#";
                }


                //hlCreateReport.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/AuditReport.aspx?SearchCriteria=" + Cryptography.Encrypt("-1");

                //Common.PopulateAdminDropDown(ref ddlAdminArea);
                //ddlAdminArea.SelectedIndex = 3;

                PopulateDocumentType();
                PopulateTableDDL();

                //populate custom report id

                DataTable dtTemp=Common.DataTableFromText("SELECT * FROM DocumentType WHERE DocumentTypeName='Custom Reports' AND AccountID=" + Session["AccountID"].ToString());
               
                if (dtTemp.Rows.Count>0)
                {
                    hfCRDocumentTypeID.Value = dtTemp.Rows[0]["DocumentTypeID"].ToString();
                }


                if (Request.QueryString["TableID"] != null)
                {
                    try
                    {
                        ddlTable.Text = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
                        if (ddlTable.Text == "-1")
                        {
                            hlBack.Visible = false;
                        }
                    }
                    catch
                    {
                        //
                    }
                }


                if (Request.QueryString["SearchCriteria"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
                }


                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = int.Parse(Session["GridPageSize"].ToString()); }

                

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
                    gvTheGrid.GridViewSortColumn = "DocumentText";
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
            ErrorLog theErrorLog = new ErrorLog(null, "Reports", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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

                ddlDocumentType.Text = xmlDoc.FirstChild[ddlDocumentType.ID].InnerText;
                txtDateFrom.Text = xmlDoc.FirstChild[txtDateFrom.ID].InnerText;
                txtDateTo.Text = xmlDoc.FirstChild[txtDateTo.ID].InnerText;
                txtDocumentText.Text = xmlDoc.FirstChild[txtDocumentText.ID].InnerText;
              
                _iStartIndex = int.Parse(xmlDoc.FirstChild["iStartIndex"].InnerText);
                _iMaxRows = int.Parse(xmlDoc.FirstChild["iMaxRows"].InnerText);
                _strGridViewSortColumn = xmlDoc.FirstChild["GridViewSortColumn"].InnerText;
                _strGridViewSortDirection = xmlDoc.FirstChild["GridViewSortDirection"].InnerText;
                ddlMenu.Text = xmlDoc.FirstChild[ddlMenu.ID].InnerText;

            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }




    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {

        lblMsg.Text = "";

        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + ddlDocumentType.ID + ">" + HttpUtility.HtmlEncode(ddlDocumentType.Text) + "</" + ddlDocumentType.ID + ">" +
                   " <" + txtDateFrom.ID + ">" + HttpUtility.HtmlEncode(txtDateFrom.Text) + "</" + txtDateFrom.ID + ">" +
                   " <" + txtDateTo.ID + ">" + HttpUtility.HtmlEncode(txtDateTo.Text) + "</" + txtDateTo.ID + ">" +
                   " <" + txtDocumentText.ID + ">" + HttpUtility.HtmlEncode(txtDocumentText.Text) + "</" + txtDocumentText.ID + ">" +
                   " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
                   " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
                   " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                   " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                    " <" + ddlMenu.ID + ">" + HttpUtility.HtmlEncode(ddlMenu.Text) + "</" + ddlMenu.ID + ">" +
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
                    dtDateTo = dtDateTo.Value.AddHours(23).AddMinutes(59);
                }
            }



            gvTheGrid.DataSource = DocumentManager.ets_Document_Select(null,int.Parse(Session["AccountID"].ToString()),
                txtDocumentText.Text.Trim(),ddlDocumentType.SelectedValue=="-1"?null:(int?) int.Parse(ddlDocumentType.SelectedValue),
                dtDateFrom,
                dtDateTo,
                null, null,null,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN,
                ddlTable.SelectedValue == "-1" ? null : (int?)int.Parse(ddlTable.SelectedValue), Session["STs"].ToString(),null,
                ddlMenu.SelectedValue == "" ? null : (int?)int.Parse(ddlMenu.SelectedValue));

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

                
                   
                    //_gvPager.AddImageURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/add_s.png";
                    _gvPager.AddURL= "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

                    //_gvPager.AddImageURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/add_s.png";
                    //_gvPager.AddURL = GetAddURL();

                    //if (Request.QueryString["type"] != null)
                    //{
                    //    if (Request.QueryString["type"].ToString() == "r")
                    //    {
                    //        _gvPager.ShowAdd2 = true;
                    //        _gvPager.HideAdd = true;
                    //    }
                    //    else
                    //    {
                    //        _gvPager.ShowAdd2 = false;
                    //        _gvPager.HideAdd = false;
                    //    }

                    //}
                
                    //_gvPager.AddToolTip = "Upload";
                    _gvPager.AddToolTip = "New Report";
               
                
            }




            //hplNewData.NavigateUrl = GetAddURL();
            //hlNewReport.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();
            
            
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


                    hpNew2.ImageUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/add32.png";
                    hpNew2.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

                    //hpNew.ImageUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/Icon_Upload.png";
                    //hpNew.NavigateUrl = GetAddURL();


                    hplNewDataFilter2.ImageUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/add32.png";
                    hplNewDataFilter2.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

                
                    //hplNewDataFilter.ImageUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/Icon_Upload.png";
                    //hplNewDataFilter.NavigateUrl = GetAddURL();
                
            }
            else
            {
                divEmptyData.Visible = false;
                divNoFilter.Visible = false;
            }
            

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Report", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");


        }
    }

    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }
    protected void PopulateDocumentType()
    {
        ddlDocumentType.Items.Clear();
        ddlDocumentType.DataSource = Common.DataTableFromText("SELECT DocumentTypeID,DocumentTypeName FROM DocumentType WHERE DocumentTypeName='Custom Reports' AND  AccountID=" + Session["AccountID"].ToString());
        ddlDocumentType.DataBind();
        //System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("All", "-1");
        //ddlDocumentType.Items.Insert(0, liSelect);     
    }

    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null,  true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());

        ddlTable.DataBind();
        //if (iTN == 0)
        //{
        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("All", "-1");
        ddlTable.Items.Insert(0, liAll);
        //}


    }
    
    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)


    protected void PopulateMenuDDL(int iAccountID)
    {
        //ddlMenu.Items.Clear();
        int iTemp = 0;
        //List<Menu> lstMenuSelect = RecordManager.ets_Menu_Select(null, string.Empty, null,
        //    iAccountID, true,
        //    "Menu", "ASC", null, null, ref iTemp, null, null);

       DataTable dtMenu = RecordManager.ets_Menu_Select(null, string.Empty, null,
           iAccountID, true,
           "Menu", "ASC", null, null, ref iTemp, null, null);

        TheDatabaseS.PopulateMenuDDL(ref ddlMenu);


        string strNone = "";
        foreach (DataRow dr in dtMenu.Rows)
        {
            if (dr["Menu"].ToString() == "--None--" && dr["ParentMenuID"] == DBNull.Value)
            {
                strNone = dr["MenuID"].ToString();
                //lstMenuSelect.Remove(aMenu);
                if (ddlMenu.Items.FindByValue(strNone) != null)
                    ddlMenu.Items.Remove(ddlMenu.Items.FindByValue(strNone));


                break;
            }

            
        }


        //ddlMenu.DataSource = lstMenuSelect;
        //ddlMenu.DataBind();

        System.Web.UI.WebControls.ListItem liEmpty = new System.Web.UI.WebControls.ListItem("", "");
        ddlMenu.Items.Insert(0, liEmpty);

        if(strNone!="")
        {
            System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("--None--", strNone);
            ddlMenu.Items.Insert(1, liNone);
        }
      


    }

    
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

    public string GetEditURL(string strDocumentID)
    {
        string strURL = "#";
        Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));
        if (theDocument != null)
        {
            //strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?type=" + Request.QueryString["type"] .ToString()+ "&mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" + Cryptography.Encrypt(strDocumentID);

            if (theDocument.ReportType == "ssrs")
            {
                strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" + Cryptography.Encrypt(theDocument.DocumentID.ToString());
            }
            else
            {
                if (theDocument.DocumentTypeID != null && hfCRDocumentTypeID.Value == theDocument.DocumentTypeID.ToString())
                {

                    strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/EditReport.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" + strDocumentID;
                }
            }
        }
        else
        {
            //
        }
        return strURL;

    }


    //public string GetViewURL()
    //{
    //    return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString())  + "&DocumentID=";
    //}

    //public string GetAddURL()
    //{
    //    return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();
    //}



    public string GetMenu(string strDocumentID)
    {

        string strMenu = "";

        DataTable dtMenu = Common.DataTableFromText("SELECT * FROM Menu WHERE IsActive=1 AND DocumentID=" + strDocumentID);

        if (dtMenu.Rows.Count > 0)
        {
            Menu theMenu = RecordManager.ets_Menu_Details(int.Parse(dtMenu.Rows[0]["MenuID"].ToString()));
            if (theMenu != null)
            {
                if (theMenu.ParentMenuID != null)
                {
                    Menu thePMenu = RecordManager.ets_Menu_Details((int)theMenu.ParentMenuID);
                    if (thePMenu != null)
                    {
                        strMenu = thePMenu.MenuP;
                    }
                }

            }

        }        

        return strMenu;
    }

    public string GetDate(string strDocumentID)
    {

        string strDate = "";
        Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));

        if (theDocument != null)
        {

            if (theDocument.DocumentDate != null)
            {
                strDate = theDocument.DocumentDate.Value.ToShortDateString();
            }

            if (theDocument.DocumentEndDate != null)
            {
                strDate = theDocument.DocumentEndDate.Value.ToShortDateString();
            }

            if (theDocument.DocumentDate != null && theDocument.DocumentEndDate!=null)
            {

                strDate = theDocument.DocumentDate.Value.ToShortDateString() + " to " + theDocument.DocumentEndDate.Value.ToShortDateString();
            }

        }


        return strDate;
    }


    public string GetFileURL(string strDocumentID)
    {
        Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));

        if (theDocument != null)
        {

            if (theDocument.ReportType == "ssrs")
            {
                string strSSRSURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/SSRS.aspx?DocumentID=" +Cryptography.Encrypt( theDocument.DocumentID.ToString()) + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();
                return strSSRSURL;
            }

            if (theDocument.DocumentTypeID != null && hfCRDocumentTypeID.Value == theDocument.DocumentTypeID.ToString())
            {

                return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/View.aspx?DocumentID=" + theDocument.DocumentID.ToString() + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString(); 
            }
            //else
            //{
            //    if (theDocument.FileUniqename != "")
            //    {
            //        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Uploads/" + theDocument.FileUniqename;
            //    }
            //    //else
            //    //{
            //    //    return "http://" + Request.Url.Authority + Request.ApplicationPath + "/DocReports/Report.aspx?ReportID=" + theDocument.DocumentID.ToString();
            //    //}
            //}

        }

        return "#";

    }

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Reports";
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
            if (Common.HaveAccess(Session["roletype"].ToString(), "5"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Read Only Account. To delete please ask your Account Administrator for appropriate rights.');", true);
                return;
            }




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
                      int i=  DocumentManager.ets_Document_Delete(int.Parse(sTemp));

                      if (i == 2)
                      {
                          ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('This report is scheduled to run automatically. Remove the schedule first and then delete it.');", true);
                      }

                        //SystemData.SystemOption_Delete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Reports", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
        "attachment;filename=Reports.csv");
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
                            HyperLink hlView2 = (HyperLink)dr.FindControl("hlView2");
                            sw.Write("\"" + hlView2.Text + "\"");
                            break;
                        case 5:
                            Label lblDocumentType = (Label)dr.FindControl("lblDocumentType");
                            sw.Write("\"" + lblDocumentType.Text + "\"");
                            break;
                        case 6:
                            Label lblDocumentDescription = (Label)dr.FindControl("lblDocumentDescription");
                            sw.Write("\"" + lblDocumentDescription.Text + "\"");
                            break;
                        case 7:
                            Label lblDocumentDate = (Label)dr.FindControl("lblDocumentDate");
                            sw.Write("\"" + lblDocumentDate.Text + "\"");
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

    protected void ddlMenu_OnSelectedIndexChanged(object sender, EventArgs e)
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
    //    Response.Redirect(Common.GetNavigateURL(ddlAdminArea.SelectedValue,int.Parse(Session["AccountID"].ToString())), false);
    //}

}
