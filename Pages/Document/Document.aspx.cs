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


public partial class Pages_Document_Document : SecurePage
{

    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";

    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "DocumentDate";
    string _strGridViewSortDirection = "DESC";

    User _objUser;
    UserRole _theUserRole;
    string _strFolderRight = "full";
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Documents";
       
        try
        {
            //if (Request.QueryString["type"] == null)
            //{
            //    Response.Redirect("~/Default.aspx");
            //    return;
            //}

            _objUser = (User)Session["User"];
            _theUserRole = (UserRole)Session["UserRole"];

            _strFilesLocation = Session["FilesLocation"].ToString();
            _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();


            if (!IsPostBack)
            {
                //lnkCreateFolder.Attributes.Add("onclick", "OpenAddFolder();return false;");

                User etUser = SecurityManager.User_LoginByEmail(_objUser.Email, _objUser.Password);

                Session["User"] = etUser;
                string roletype = "";
                roletype = SecurityManager.GetUserRoleTypeID((int)_objUser.UserID, int.Parse(Session["AccountID"].ToString()));
                Session["roletype"] = roletype;

                if ((bool)_theUserRole.IsAdvancedSecurity)
                {
                    if (Session["roletype"].ToString() != Common.UserRoleType.OwnData)
                    {
                        Session["roletype"] = Common.UserRoleType.ReadOnly;
                    }
                }


                hlAddFolder.NavigateUrl = GetAddFolderURL();
                lnkCreateFolder.NavigateUrl = GetAddFolderURL();
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                { 
                   hlDocumentTypeEdit.Visible=false;
                   chkAllFolder.Visible = false;
                }

                if (Request.QueryString["SSearchCriteriaID"] != null)
                {

                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();
                }
                else
                {
                    //hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString();
                    hlBack.NavigateUrl = "#";
                }


                //hlCreateReport.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/AuditReport.aspx?SearchCriteria=" + Cryptography.Encrypt("-1");

                //Common.PopulateAdminDropDown(ref ddlAdminArea);
                //ddlAdminArea.SelectedIndex = 3;

                PopulateDocumentType();
                //PopulateTableDDL();

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
                        //ddlTable.Text = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
                        //if (ddlTable.Text == "-1")
                        //{
                        hlBack.Visible = false;
                        //}
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

                if (Request.QueryString["Category"] != null)
                {
                    string strDocumentTypeID= Cryptography.Decrypt(Request.QueryString["Category"].ToString());

                    if (ddlDocumentType.Items.FindByValue(strDocumentTypeID) != null)
                        ddlDocumentType.SelectedValue = strDocumentTypeID;

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
                    gvTheGrid.GridViewSortColumn = "DocumentDate";
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                }
            }
            else
            {
              

                if (txtDateFrom.Text != "")
                {
                    DateTime dtTemp;
                    if (DateTime.TryParseExact(txtDateFrom.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                    {
                        txtDateFrom.Text = dtTemp.ToShortDateString();
                    }
                }
                if (txtDateTo.Text != "")
                {
                    DateTime dtTemp;
                    if (DateTime.TryParseExact(txtDateTo.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                    {
                        txtDateTo.Text = dtTemp.ToShortDateString();
                       
                    }
                }


            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "SystemOption", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }


        string strJS = @" $(document).ready(function () {
      
        $(function () {
            $('.popuplink').fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 500,
                height: 250,
                titleShow: false
            });
        });

    });";


        ScriptManager.RegisterStartupScript(this, this.GetType(), "fancyJS", strJS, true);


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
                hfParentFolderID.Value = xmlDoc.FirstChild["hfParentFolderID"].InnerText;
                chkAllFolder.Checked = bool.Parse( xmlDoc.FirstChild["chkAllFolder"].InnerText);
                if (!IsPostBack)
                {
                    MakeFolderPath();
                }
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


        if (chkAllFolder.Checked)
        {
            lblCurrentFolder.Visible = false;
        }
        else
        {
            lblCurrentFolder.Visible = true;
        }

        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + ddlDocumentType.ID + ">" + HttpUtility.HtmlEncode(ddlDocumentType.Text) + "</" + ddlDocumentType.ID + ">" +
                   " <" + txtDateFrom.ID + ">" + HttpUtility.HtmlEncode(txtDateFrom.Text) + "</" + txtDateFrom.ID + ">" +
                   " <" + txtDateTo.ID + ">" + HttpUtility.HtmlEncode(txtDateTo.Text) + "</" + txtDateTo.ID + ">" +
                   " <" + txtDocumentText.ID + ">" + HttpUtility.HtmlEncode(txtDocumentText.Text) + "</" + txtDocumentText.ID + ">" +
                   " <" + hfParentFolderID.ID + ">" + HttpUtility.HtmlEncode(hfParentFolderID.Value) + "</" + hfParentFolderID.ID + ">" +
                   " <" + chkAllFolder.ID + ">" + HttpUtility.HtmlEncode(chkAllFolder.Checked.ToString()) + "</" + chkAllFolder.ID + ">" +
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




        try
        {
            int iTN = 0;

            int? iParentFolderID = hfParentFolderID.Value == "-1" ? null : (int?)int.Parse(hfParentFolderID.Value);

            if (chkAllFolder.Checked)
                iParentFolderID = -1;
            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = DocumentManager.ets_FolderDocument_Select(null,int.Parse(Session["AccountID"].ToString()),
                txtDocumentText.Text.Trim(),ddlDocumentType.SelectedValue=="-1"?null:(int?) int.Parse(ddlDocumentType.SelectedValue),
                txtDateFrom.Text==""?null: (DateTime?)DateTime.ParseExact(txtDateFrom.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
                txtDateTo.Text == "" ? null : (DateTime?)DateTime.ParseExact(txtDateTo.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
                null, null,null,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN,
                null, Session["STs"].ToString(),null,
                iParentFolderID);

            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.AddURL2 = GetAddURL();
                _gvPager.AddToolTip2 = "Upload";
                //_gvPager.AddToolTip = "Add";
                //_gvPager.AddURL = "javascript:OpenAddFolder();";
                hlAddFolder.NavigateUrl = GetAddFolderURL();
                lnkCreateFolder.NavigateUrl = GetAddFolderURL();
                //_gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
                if (ViewState[gvTheGrid.ID + "PageIndex"] != null)
                    _gvPager.PageIndex = int.Parse(ViewState[gvTheGrid.ID + "PageIndex"].ToString());

                _gvPager.PageSize = gvTheGrid.PageSize;
                _gvPager.TotalRows = iTN;
                    //_gvPager.AddImageURL2 = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/add_s.png";
                    //_gvPager.AddURL2 = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

                    //_gvPager.AddImageURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/add_s.png";
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
                
                    
                    //_gvPager.AddToolTip2 = "New Report";
               
                
            }




            //hplNewData.NavigateUrl = GetAddURL();
            //hlNewReport.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();
            
            
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


                    //hpNew2.ImageUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/Icon_NewDoc.png";
                    //hpNew2.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

                hpNew.ImageUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/Icon_Upload.png";
                    hpNew.NavigateUrl = GetAddURL();
                    //hpNewFolder.ImageUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/add32.png";
                    //hpNewFolder.NavigateUrl = "javascript:OpenAddFolder();";
                    //hplNewDataFilter2.ImageUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/Icon_NewDoc.png";
                    //hplNewDataFilter2.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();


                    hplNewDataFilter.ImageUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/App_Themes/Default/Images/Icon_Upload.png";
                    hplNewDataFilter.NavigateUrl = GetAddURL();
                
            }
            else
            {
                divEmptyData.Visible = false;
                divNoFilter.Visible = false;
            }

            CheckFolderSecurity();
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
        ddlDocumentType.DataSource = Common.DataTableFromText("SELECT DocumentTypeID,DocumentTypeName FROM DocumentType WHERE DocumentTypeName <>'Custom Reports' AND AccountID=" + Session["AccountID"].ToString());
        ddlDocumentType.DataBind();
        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("All", "-1");
        ddlDocumentType.Items.Insert(0, liSelect);     
    }

    //protected void PopulateTableDDL()
    //{
    //    int iTN = 0;
    //    ddlTable.DataSource = RecordManager.ets_Table_Select(null,
    //            null,
    //            null,
    //            int.Parse(Session["AccountID"].ToString()),
    //            null, null,  true,
    //            "st.TableName", "ASC",
    //            null, null, ref  iTN, Session["STs"].ToString());

    //    ddlTable.DataBind();
    //    //if (iTN == 0)
    //    //{
    //    System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("All", "-1");
    //    ddlTable.Items.Insert(0, liAll);
    //    //}


    //}
    
   
    protected void btnFolderSaved_Click(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);

        MakeFolderPath();
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

    protected void CheckFolderSecurity()
    {

        string strFolderRight = "full";
        if (_theUserRole.IsDocSecurityAdvanced != null)
        {
            if ((bool)_theUserRole.IsDocSecurityAdvanced)
            {

                string strDBRight="";

                if (hfParentFolderID.Value == "-1")
                {
                    strDBRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE UserID=" + _objUser.UserID.ToString() + " AND FolderID IS NULL ");

                }
                else
                {
                    strDBRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE UserID=" + _objUser.UserID.ToString() + " AND FolderID=" + hfParentFolderID.Value);
                }

                if (strDBRight == "")
                {
                    if(hfParentFolderID.Value!="-1")
                        FindParentRight(ref strDBRight,int.Parse(hfParentFolderID.Value));
                }

                if (strDBRight == "")
                {
                    strFolderRight = "full";
                }
                else
                {
                    strFolderRight = strDBRight;
                }
            }
            else
            {
                if (_theUserRole.DocSecurityType != "")
                    strFolderRight = _theUserRole.DocSecurityType;
            }
        }
        else
        {
            if (_theUserRole.DocSecurityType != "")
                strFolderRight = _theUserRole.DocSecurityType;
        }

        GridViewRow gvr = gvTheGrid.TopPagerRow;

        switch (strFolderRight)
        {
            case "none":
                gvTheGrid.Visible = false;
                divEmptyData.Visible = false;
                lnkCreateFolder.Visible = false;
                lblCurrentFolder.Visible = false;
                break;
            case "read":
                gvTheGrid.Visible = true;
                divEmptyData.Visible = false;
                gvTheGrid.Columns[0].Visible=false;//check box column  
                gvTheGrid.Columns[4].Visible=false;//edit column  
                lnkCreateFolder.Visible = false;

                if (gvr != null)
                {
                    //_gvPager.HideAdd = true;
                    _gvPager.ShowAdd2 = false;
                    _gvPager.HideDelete = true;
                }

                break;
            case "upload":
                gvTheGrid.Visible = true;
                 gvTheGrid.Columns[0].Visible=false;//check box column  
                 gvTheGrid.Columns[4].Visible=false;//edit column  
                 lnkCreateFolder.Visible = false;
                 if (gvr != null)
                 {
                     //_gvPager.HideAdd = true;
                     _gvPager.ShowAdd2 = true;
                     _gvPager.HideDelete = true;
                 }

                break;
            case "full":
                gvTheGrid.Visible = true;
                 gvTheGrid.Columns[0].Visible=true;//check box column  
                gvTheGrid.Columns[4].Visible=true;//edit column  

                if (gvr != null)
                {
                    //_gvPager.HideAdd = false;
                    _gvPager.ShowAdd2 = true;
                    _gvPager.HideDelete = false;
                }
                break;

            default:
                break;
        }


        _strFolderRight = strFolderRight;
        if (!IsPostBack)
        {
            Session["_strFolderRight"] = _strFolderRight;
        }


        if (_strFolderRight == "none")
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This page is not accessible, Please contact your administrator....');", true);
        }
    }

    protected void DownloadFile(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkFileName = sender as LinkButton;
            if (lnkFileName != null)
            {
                GridViewRow row = lnkFileName.NamingContainer as GridViewRow;
                Label lblID = row.FindControl("LblID") as Label;

                Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(lblID.Text));

                if (theDocument != null)
                {
                  
                    //string strFilePath=Server.MapPath("~/UserFiles/Documents/" + theDocument.FileUniqename);
                    string strFilePath = _strFilesPhisicalPath + "\\UserFiles\\Documents\\" + theDocument.FileUniqename;
                    if (File.Exists(strFilePath))
                    {
                        Response.ContentType = "application/octet-stream";
                        
                        //Response.AppendHeader("Content-Disposition", "attachment; filename=" +  theDocument.FileUniqename.Substring(37));
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + (theDocument.FileUniqename.Substring(theDocument.FileUniqename.IndexOf("_") + 1)).Substring(37));
                        Response.WriteFile(strFilePath);
                        Response.End();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This file is missing!');", true);

                    }
                }

            }
        }
        catch
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Problem! Please try again.');", true);

        }
    }
    protected void GoToFolder(object sender, EventArgs e)
    {
        LinkButton lnkFolderName = sender as LinkButton;

        if (lnkFolderName != null)
        {
            //lblCurrentFolder.Text = lnkFolderName.Text;

            

            GridViewRow row = lnkFolderName.NamingContainer as GridViewRow;
            Label lblID = row.FindControl("LblID") as Label;
            hfParentFolderID.Value = lblID.Text;

            MakeFolderPath();

            lnkSearch_Click(null, null);
        }

    }

    protected void MakeFolderPath()
    {
        string strFolder = "";
        GetFolderPath(int.Parse(hfParentFolderID.Value), ref strFolder);

        strFolder = "<a href='javascript:SetFolder(-1)'>Home</a>/" + strFolder;
        lblCurrentFolder.Text = strFolder;
    }

    protected void GetFolderPath(int iFolderID, ref string strFolder)
    {
        Folder theFolder = DocumentManager.ets_Folder_Detail(iFolderID);
        if (theFolder != null)
        {
            strFolder = "<a href='javascript:SetFolder(" + iFolderID.ToString() + ")'>" + theFolder.FolderName + "</a>/" + strFolder;
            if (theFolder.ParentFolderID != null)
            {
                GetFolderPath((int)theFolder.ParentFolderID, ref strFolder);
            }
        }

    }


    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton lnkFileView = (LinkButton)e.Row.FindControl("lnkFileView");
            HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
            Label LblID = (Label)e.Row.FindControl("LblID");
            Label lblType = (Label)e.Row.FindControl("lblType");

            Label lblDate = (Label)e.Row.FindControl("lblDate");
            Label lblSize = (Label)e.Row.FindControl("lblSize");
            HyperLink hlFolderIcon = (HyperLink)e.Row.FindControl("hlFolderIcon");
            LinkButton lnkFileIcon = (LinkButton)e.Row.FindControl("lnkFileIcon");

            if (DataBinder.Eval(e.Row.DataItem, "Size")!=DBNull.Value)
            {
                double dSize = double.Parse(DataBinder.Eval(e.Row.DataItem, "Size").ToString());
                lblSize.Text = Common.SizeSuffix(dSize);
            }
            if (DataBinder.Eval(e.Row.DataItem, "DocumentDate") != DBNull.Value)
            {
                lblDate.Text = ((DateTime)DataBinder.Eval(e.Row.DataItem, "DocumentDate")).ToString("dd/MM/yyyy");
            }

            if (DataBinder.Eval(e.Row.DataItem, "FD").ToString() == "F")
            {
                //folder
                lnkFileIcon.Visible = false;
                hlFolderIcon.Visible = true;
                LinkButton lnkFolderName = (LinkButton)e.Row.FindControl("lnkFolderName");
                lnkFolderName.Visible = true;
                lnkFileView.Visible = false;

                hlEdit.CssClass = "popuplink";
                hlEdit.NavigateUrl = GetEditFolderURL() + Cryptography.Encrypt( LblID.Text);
                lblType.Text = "File folder";

                string strRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE FolderID=" + LblID.Text+ "	AND UserID=" + _objUser.UserID.ToString());

                if (strRight.ToLower() == "none")
                {
                    Image imgFolderIcon = (Image)e.Row.FindControl("imgFolderIcon");
                    imgFolderIcon.ImageUrl = "~/App_Themes/Default/Images/folder_key.png";
                    hlFolderIcon.NavigateUrl = "";
                    hlFolderIcon.Attributes.Add("onclick","javascript:alert('You do not have permission to open this folder');return false;");
                    imgFolderIcon.ToolTip = "You do not have permission to open this folder";
                    hlEdit.Visible = false;

                    lnkFolderName.Attributes.Add("onclick", "javascript:alert('You do not have permission to open this folder');return false;");
                    lnkFolderName.Style.Add("text-decoration", "none");
                    lnkFolderName.Style.Add("color", "Black");
                    lnkFolderName.Style.Add("cursor", "default");
                    lnkFolderName.ToolTip = "You do not have permission to open this folder";

                }
                if (strRight.ToLower() != "" && strRight.ToLower()!="full")
                {
                    hlEdit.Visible = false;
                }
              
            }
            else
            {
                lnkFileIcon.Visible = true;
                hlFolderIcon.Visible = false;
                LinkButton lnkFileName = (LinkButton)e.Row.FindControl("lnkFileName");
                lnkFileName.Visible = true;


                Label lblCategory = (Label)e.Row.FindControl("lblCategory");

                Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(LblID.Text));
                if (theDocument != null)
                {
                    if (theDocument.DocumentTypeID != null)
                    {
                        DocumentType theDocumentType = DocumentManager.ets_DocumentType_Detail((int)theDocument.DocumentTypeID);
                        if (theDocumentType != null)
                        {
                            lblCategory.Text = theDocumentType.DocumentTypeName;
                        }
                    }
                }

                if(DataBinder.Eval(e.Row.DataItem, "UN").ToString()!="")
                {
                    string strFileName = DataBinder.Eval(e.Row.DataItem, "UN").ToString();
                   
                    Image imgFileIcon = (Image)e.Row.FindControl("imgFileIcon");
                    string strFileExtension = "";

                    strFileExtension = strFileName.Substring(strFileName.LastIndexOf('.') + 1).ToLower();

                    if (File.Exists(Server.MapPath( "Images\\" + "file_extension_"+strFileExtension+".png")))
                    {
                        imgFileIcon.ImageUrl = "~/Pages/Document/Images/" + "file_extension_" + strFileExtension + ".png";
                    }
                    else
                    {
                        imgFileIcon.ImageUrl = "~/Pages/Document/Images/" + "file_extension_txt.png";
                    }

                    lblType.Text = strFileExtension.ToUpper() + " File";
                    lnkFileIcon.ToolTip = lblType.Text;
                    imgFileIcon.AlternateText = lblType.Text;

                    //hlFolderIcon.NavigateUrl = GetFileURL(LblID.Text);
                    //hlFileName.NavigateUrl = hlFolderIcon.NavigateUrl;
                    switch (strFileExtension)
                    {
                        case "csv":
                            lblType.Text = "Comma Separated Values";
                            break;
                        case "xls":
                            lblType.Text = "Microsoft Office Excel Document";
                            break;
                        case "xlsx":
                            lblType.Text = "Microsoft Office Excel Document";
                            break;
                        case "txt":
                            break;
                        case "doc":
                            lblType.Text = "Microsoft Office Word Document";
                            break;
                        case "docx":
                            lblType.Text = "Microsoft Office Word Document";
                            break;
                        case "pdf":
                            lblType.Text = "PDF File";
                            break;
                        default:
                            break;

                    }

                }
            }

        }

    }

    public string GetEditURL(string strDocumentID)
    {
        string strURL = "#";
        Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));
        if (theDocument != null)
        {

            string strExtra = "";

            if (Request.QueryString["SSearchCriteriaID"] != null)
                strExtra = "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

            strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + strExtra + "&DocumentID=" + Cryptography.Encrypt(strDocumentID);

            //if (theDocument.DocumentTypeID != null && hfCRDocumentTypeID.Value == theDocument.DocumentTypeID.ToString())
            //{

            //    strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/EditReport.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" + strDocumentID;
            //}
        }
        else
        {
            //
        }
        return strURL;

    }


    //public string GetViewURL()
    //{
    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString())  + "&DocumentID=";
    //}

    public string GetAddURL()
    {

        string strExtra = "";

        if (Request.QueryString["SSearchCriteriaID"] != null)
            strExtra = "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&TableID=" + Request.QueryString["TableID"].ToString() + strExtra + "&FolderID=" + hfParentFolderID.Value.ToString();
    }

    public string GetAddFolderURL()
    {
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/FolderDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&ParentFolderID=" +hfParentFolderID.Value;
    }

    public string GetEditFolderURL()
    {
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/FolderDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&ParentFolderID=" + hfParentFolderID.Value + "&FolderID=" ;
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
            if (theDocument.FileUniqename != "")
            {
                return _strFilesLocation + "/UserFiles/Documents/" + theDocument.FileUniqename;
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
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }


    protected void FindParentRight(ref string strRight, int iFolderID)
    {
        Folder theFolder = DocumentManager.ets_Folder_Detail(iFolderID);
        if (theFolder != null)
        {
            if (strRight == "")
            {
                if (theFolder.ParentFolderID != null)
                {
                    Folder theFolderP = DocumentManager.ets_Folder_Detail((int)theFolder.ParentFolderID);
                    strRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE UserID=" + _objUser.UserID.ToString() + " AND FolderID=" + theFolderP.FolderID.ToString());

                    if (strRight == "")
                    {                       
                            FindParentRight(ref strRight, (int)theFolder.ParentFolderID);
                    }
                }

            }


        }
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtDocumentText.Text = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        ddlDocumentType.SelectedIndex = 0;
        //ddlTable.SelectedIndex = 0;
        gvTheGrid.GridViewSortColumn = "DocumentText";
        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;

        lnkSearch_Click(null, null);
    }


    protected void Pager_DeleteAction(object sender, EventArgs e)
    {
        string sCheck = "";
        string sCheckFol = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;

            Label lblFD = (Label)gvTheGrid.Rows[i].FindControl("lblFD");

            if (ischeck)
            {
                if (lblFD.Text.ToLower() == "d")
                {
                    sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
                else
                {
                    sCheckFol = sCheckFol + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
                }
            }
        }
        if (string.IsNullOrEmpty(sCheck) && string.IsNullOrEmpty(sCheckFol))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a folder or file.');", true);
        }
        else
        {
            if (Session["_strFolderRight"] != null)
                _strFolderRight = Session["_strFolderRight"].ToString();
            if (_strFolderRight!="full") //(Common.HaveAccess(Session["roletype"].ToString(), "5"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('To delete please ask your Account Administrator for appropriate rights.');", true);
                return;
            }

            
            DeleteFileItem(sCheck);

            DeleteFolderItem(sCheckFol);

            BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
            //_gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
            //if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
            //{
            //    BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
            //}
        }

    }



    private void DeleteFileItem(string keys)
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
                      
                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Documents", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }


    private void DeleteFolderItem(string keys)
    {
        try
        {
            if (!string.IsNullOrEmpty(keys))
            {

                foreach (string sTemp in keys.Split(','))
                {
                    if (!string.IsNullOrEmpty(sTemp))
                    {
                        int i = DocumentManager.ets_Folder_Delete(int.Parse(sTemp));
                        
                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Documents", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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
        "attachment;filename=Document.csv");
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
    //protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    lnkSearch_Click(null, null);

    //}



    protected bool IsFiltered()
    {
        if (txtDocumentText.Text != "" ||  ddlDocumentType.SelectedIndex != 0
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

    protected void chkAllFolder_CheckedChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }
}
