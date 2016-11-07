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

public partial class Record_Menu : SecurePage
{

    Common_Pager _gvPager;
    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "Menu";
    string _strGridViewSortDirection = "ASC";

    string _strActionMode = "";
    int? _iMenuID;
    Menu _theMenu;
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }


    protected void PopulateDcouments()
    {
        try
        {

            ddlReports.DataSource = Common.DataTableFromText("SELECT DocumentID,DocumentText FROM Document WHERE AccountID=" + Session["AccountID"].ToString() + " AND (ForDashBoard=0 or ForDashBoard IS NULL) ORDER BY DocumentText");
            ddlReports.DataBind();
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
            ddlReports.Items.Insert(0, liSelect);

        }
        catch
        {
            //
        }

    }
    protected void PopulateDcoumentType()
    {
        try
        {

            ddlDocumentType.DataSource = Common.DataTableFromText("SELECT DocumentTypeID,DocumentTypeName FROM DocumentType WHERE AccountID=" + Session["AccountID"].ToString());
            ddlDocumentType.DataBind();
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
            ddlDocumentType.Items.Insert(0, liSelect);

        }
        catch
        {
            //
        }

    }

    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());

        ddlTable.DataBind();
        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddlTable.Items.Insert(0, liSelect);
    }
    protected void optMenuType_SelectedIndexChanged(object sender, EventArgs e)
    {
        trOpenInNewWindow.Visible = false;

        stgShowUnder.InnerText = "Show Under*:";
        trMenu.Visible = false;
        trExternalPageLink.Visible = false;
        trTable.Visible = false;
        trReport.Visible = false;
        trDocumentType.Visible = false;
        rfvShowUnder.Enabled = false;

        if (optMenuType.SelectedValue != "d")
        {
            if (txtMenu.Text == Common.MenuDividerText)
            {
                txtMenu.Text = "";
            }     
           
        }

        if (optMenuType.SelectedValue == "d")
        {           
            rfvShowUnder.Enabled = true;        
            
        }       
        else if (optMenuType.SelectedValue == "l")
        {
            trExternalPageLink.Visible = true;
            trMenu.Visible = true;         
            trOpenInNewWindow.Visible = true;
           
        }
        else if (optMenuType.SelectedValue == "t")
        {           
            trMenu.Visible = true;
            trTable.Visible = true;         
        }
        else if (optMenuType.SelectedValue == "r")
        {
           
            trMenu.Visible = true;
            trReport.Visible = true;
        }
        else if (optMenuType.SelectedValue == "doc")
        {

            trMenu.Visible = true;
            trDocumentType.Visible = true;
        }
        else
        {
            trMenu.Visible = true;                
        }

    }

    protected void PopulateMenuDDL(ref DropDownList ddlShowUnder)
    {


        TheDatabaseS.PopulateMenuDDL(ref ddlShowUnder);

        ListItem liTop = new ListItem("-- Top Level --", "");
        ddlShowUnder.Items.Insert(0, liTop);
    }



    protected void PopulateTheRecord()
    {

        Menu theMenu = RecordManager.ets_Menu_Details((int)_iMenuID);

        if (theMenu.ParentMenuID != null)
        {
            if (ddlShowUnder.Items.FindByValue(theMenu.ParentMenuID.ToString()) != null)
                ddlShowUnder.SelectedValue = theMenu.ParentMenuID.ToString();

            hfParentMenuID.Value = theMenu.ParentMenuID.ToString();
        }

        txtMenu.Text = theMenu.MenuP;
        chkShowOnMenu.Checked = (bool)theMenu.ShowOnMenu;
        txtExternalPageLink.Text = theMenu.ExternalPageLink;


        if (theMenu.OpenInNewWindow != null)
            chkOpenInNewWindow.Checked = (bool)theMenu.OpenInNewWindow;

        

       

       
        if (theMenu.TableID != null)
        {
            optMenuType.SelectedValue = "t";
            optMenuType.Enabled = false;

            if (ddlTable.Items.FindByValue(theMenu.TableID.ToString()) != null)
                ddlTable.SelectedValue = theMenu.TableID.ToString();

        }
        else if (theMenu.DocumentID != null)
        {
            optMenuType.SelectedValue = "r";

            if (ddlReports.Items.FindByValue(theMenu.DocumentID.ToString()) != null)
                ddlReports.SelectedValue = theMenu.DocumentID.ToString();

        }
        else if (theMenu.DocumentTypeID != null)
        {
            optMenuType.SelectedValue = "doc";

            if (ddlDocumentType.Items.FindByValue(theMenu.DocumentTypeID.ToString()) != null)
                ddlDocumentType.SelectedValue = theMenu.DocumentTypeID.ToString();

        }
        else if (theMenu.MenuType =="doc")
        {
            optMenuType.SelectedValue = "doc";            
            ddlDocumentType.SelectedValue = "";

        }
        else if (theMenu.MenuP == Common.MenuDividerText)
        {
            optMenuType.SelectedValue = "d";
        }
        else if (!string.IsNullOrEmpty(theMenu.ExternalPageLink))
        {
            optMenuType.SelectedValue = "l";
        }
        else
        {
            optMenuType.SelectedValue = "m";
        }


        optMenuType_SelectedIndexChanged(null, null);

        if (_strActionMode == "edit")
        {
            ViewState["theMenu"] = theMenu;

            if (theMenu.IsActive == true)
            {
                divUnDelete.Visible = false;
            }
            else
            {
                divDelete.Visible = false;
            }

            if(theMenu.MenuP.ToLower()=="--none--")
            {
                EnableTheRecordControls(false);
                lnkDelete.Visible = false;
                lnkUnDelete.Visible = false;
                lnkSave.Visible = false;
                 
            }
        }

     

    }


    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            RecordManager.ets_Menu_Delete((int)_iMenuID);
            Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            if (ex is SqlException)
            {
                lblMsg.Text = "Delete failed! Please try again.";
            }
            else
            {

                lblMsg.Text = ex.Message;
            }
        }


    }

    protected void lnkUnDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            RecordManager.ets_Menu_UnDelete((int)_iMenuID);
            Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            if (ex is SqlException)
            {
                if (ex.Message.IndexOf("UQ_Menu_Name_Parent_AccountID") > -1)
                {
                    lblMsg.Text = "A Menu '" + txtMenu.Text.Trim() + "' already exists.";
                }
                else
                {
                    lblMsg.Text = "Restore failed! Please try again.";
                }
            }
            else
            {

                lblMsg.Text = ex.Message;
            }
        }


    }


    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtMenu.Enabled = p_bEnable;
        chkShowOnMenu.Enabled = p_bEnable;
        optMenuType.Enabled = p_bEnable;
        //txtCustomPageLink.Enabled = p_bEnable;
        txtExternalPageLink.Enabled = p_bEnable;
        //chkIsActive.Enabled = p_bEnable;
        chkOpenInNewWindow.Enabled = p_bEnable;
        ddlShowUnder.Enabled = p_bEnable;
        //ddlAccount.Enabled = p_bEnable;       

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {


        if (IsUserInputOK())
        {
           

            try
            {


                switch (_strActionMode.ToLower())
                {
                    case "add":

                        Menu newMenu = new Menu(null, txtMenu.Text,
                            int.Parse(Session["AccountID"].ToString()), chkShowOnMenu.Checked, true);


                        newMenu.ParentMenuID = ddlShowUnder.SelectedValue == "" ? null : (int?)int.Parse(ddlShowUnder.SelectedValue);
                        newMenu.OpenInNewWindow = chkOpenInNewWindow.Checked;
                        if (optMenuType.SelectedValue == "d")
                        {
                            newMenu.MenuP = Common.MenuDividerText;
                            newMenu.OpenInNewWindow = false;
                        }
                        else if (optMenuType.SelectedValue == "t")
                        {
                            newMenu.TableID = int.Parse(ddlTable.SelectedValue);

                        }
                        else if (optMenuType.SelectedValue == "doc")
                        {

                            if (ddlDocumentType.SelectedValue == "")
                            {
                                newMenu.MenuType = "doc";
                            }
                            else
                            {
                                newMenu.DocumentTypeID = int.Parse(ddlDocumentType.SelectedValue);
                            }

                        }
                        else if (optMenuType.SelectedValue == "r")
                        {
                            newMenu.DocumentID = int.Parse(ddlReports.SelectedValue);

                        }
                        else if (optMenuType.SelectedValue == "l")
                        {
                            newMenu.ExternalPageLink = string.IsNullOrEmpty(txtExternalPageLink.Text) ? "" : txtExternalPageLink.Text;
                        }
                        else
                        {
                            //menu
                        }


                        //if (newMenu.ParentMenuID == null && newMenu.TableID != null)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ProblemParentMenuID", "alert('This is a table menu, this menu can not be Top Level menu.');", true);
                        //    return;
                        //}


                        int iNewMenuID = RecordManager.ets_Menu_Insert(newMenu);

                        if (optMenuType.SelectedValue == "t")
                        {
                            Table theTable = RecordManager.ets_Table_Details(int.Parse(ddlTable.SelectedValue));
                            if (theTable != null) // && newMenu.ParentMenuID != null
                            {
                                //remove any other link this table with anyy menu
                                Common.ExecuteText("UPDATE Menu SET TableID=NULL WHERE MenuID<>" + iNewMenuID.ToString() + " AND TableID=" + newMenu.TableID.ToString());
                            }
                        }
                        if (optMenuType.SelectedValue == "r")
                        {
                            ////remove any other link this document with any menu
                            Common.ExecuteText("UPDATE Menu SET DocumentID=NULL WHERE MenuID<>" + iNewMenuID.ToString() + " AND DocumentID=" + newMenu.DocumentID.ToString());

                        }

                        break;

                    case "view":


                        break;

                    case "edit":
                        Menu editMenu = (Menu)ViewState["theMenu"];

                        editMenu.MenuP = txtMenu.Text;
                        editMenu.ShowOnMenu = chkShowOnMenu.Checked;
                        //editMenu.AccountID = int.Parse(Session["AccountID"].ToString());

                        editMenu.ParentMenuID = ddlShowUnder.SelectedValue == "" ? null : (int?)int.Parse(ddlShowUnder.SelectedValue);

                      

                        editMenu.DocumentTypeID = null;
                        editMenu.TableID = null;
                        editMenu.DocumentID = null;
                        editMenu.ExternalPageLink = "";
                        editMenu.OpenInNewWindow = false;
                        editMenu.MenuType = "";
                        if (optMenuType.SelectedValue == "d")
                        {                          
                            editMenu.MenuP = Common.MenuDividerText;                                                   
                        }
                        else if (optMenuType.SelectedValue == "t")
                        {
                            editMenu.TableID = int.Parse(ddlTable.SelectedValue);
                        }
                        else if (optMenuType.SelectedValue == "doc")
                        {
                            if (ddlDocumentType.SelectedValue=="")
                            {
                                editMenu.MenuType = "doc";
                            }
                            else
                            {
                                editMenu.DocumentTypeID = int.Parse(ddlDocumentType.SelectedValue);
                            }
                            
                        }
                        else if (optMenuType.SelectedValue == "r")
                        {
                            editMenu.DocumentID = int.Parse(ddlReports.SelectedValue);
                        }
                        else if (optMenuType.SelectedValue == "d")
                        {
                            editMenu.MenuP = Common.MenuDividerText;
                        }
                        else if (optMenuType.SelectedValue == "l")
                        {
                            editMenu.ExternalPageLink = string.IsNullOrEmpty(txtExternalPageLink.Text) ? "" : txtExternalPageLink.Text;
                            editMenu.OpenInNewWindow = chkOpenInNewWindow.Checked;
                        }
                        else
                        {
                            //
                        }

                        //if (editMenu.ParentMenuID == null && editMenu.TableID != null)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ProblemParentMenuID", "alert('This is a table menu, this menu can not be Top Level menu.');", true);
                        //    return;
                        //}

                        int iIsUpdated = RecordManager.ets_Menu_Update(editMenu);


                        if (optMenuType.SelectedValue == "t")
                        {
                            Table theTable = RecordManager.ets_Table_Details(int.Parse(ddlTable.SelectedValue));
                            if (theTable != null) //&& editMenu.ParentMenuID != null
                            {
                                //remove any other link this table with anyy menu
                                Common.ExecuteText("UPDATE Menu SET TableID=NULL WHERE MenuID<>" + editMenu.MenuID.ToString() + " AND TableID=" + editMenu.TableID.ToString());
                            }
                        }

                        if (optMenuType.SelectedValue == "r")
                        {
                            ////remove any other link this document with any menu
                            Common.ExecuteText("UPDATE Menu SET DocumentID=NULL WHERE MenuID<>" + editMenu.MenuID.ToString() + " AND DocumentID=" + editMenu.DocumentID.ToString());

                        }

                        break;

                    default:
                        //?
                        break;
                }

              

                Response.Redirect(hlBack.NavigateUrl, false);
            }

            catch (Exception ex)
            {

           

                ErrorLog theErrorLog = new ErrorLog(null, "Table Group", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                if (ex.Message.IndexOf("UQ_Menu_Name_Parent_AccountID") > -1)
                {
                    lblMsg.Text = "A Menu '" + txtMenu.Text + "' is already in same level, please try another Menu name.";
                    txtMenu.Focus();

                }
                else if (ex.Message.IndexOf("CHK_Menu_NotSelfParent") > -1)
                {
                    lblMsg.Text = "A Menu can not be under it's own , please try another Menu name.";
                    txtMenu.Focus();

                }
                else
                {
                    lblMsg.Text = ex.Message;
                }


            }

        }

    }


    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx", false);
    }

    protected void btnOrderMT_Click(object sender, EventArgs e)
    {
        //
        if (hfOrderMT.Value != "")
        {
          
            try
            {
                string strNewMT = hfOrderMT.Value.Substring(0, hfOrderMT.Value.Length - 1);
                string[] newMT = strNewMT.Split(',');


                DataTable dtDO = Common.DataTableFromText("SELECT DisplayOrder,MenuID FROM [Menu] WHERE MenuID IN (" + strNewMT + ") ORDER BY DisplayOrder");
                if (newMT.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newMT.Length; i++)
                    {
                        if(dtDO.Rows[i][0].ToString()!="")
                        {
                            Common.ExecuteText("UPDATE [Menu] SET DisplayOrder =" + dtDO.Rows[i][0].ToString() + " WHERE MenuID=" + newMT[i]);
                        }

                    }
                }


            }
            catch (Exception ex)
            {

             //

            }

            Response.Redirect(Request.RawUrl, false);
            //Response.Redirect("~/Pages/Record/TableGroup.aspx?ParentMenuID=" + hfParentMenuID.Value, false);

            //lnkSearch_Click(null, null);
            //if (Request.QueryString["SearchCriteria"] != null)
            //{
            //    Response.Redirect("~/Pages/Record/TableGroup.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString(), false);
            //}
            //else
            //{
            //    Response.Redirect("~/Pages/Record/TableGroup.aspx?ParentMenuID=" + hfParentMenuID.Value, false);
            //}
            //Home master = (Home)this.Master;
             
            
        }
    }



    protected void Page_Load(object sender, EventArgs e) 
    {
       


        Title = "Menus";
        try
        {


            User ObjUser = (User)Session["User"];

            if (Request.QueryString["mode"] != null)
            {
                _strActionMode = Cryptography.Decrypt(Request.QueryString["mode"]);
            }

            if (Request.QueryString["MenuID"] != null)
            {
                _iMenuID = int.Parse(Cryptography.Decrypt(Request.QueryString["MenuID"].ToString()));
                _theMenu = RecordManager.ets_Menu_Details((int)_iMenuID);
            }


            if (!IsPostBack)
            {
                PopulateTableDDL();
                PopulateDcouments();
                PopulateDcoumentType();
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                { Response.Redirect("~/Default.aspx", false); }

                if (Request.QueryString["ParentMenuID"] == null)
                {
                    hfParentMenuID.Value = "-1";
                }
                else
                {
                    hfParentMenuID.Value =Request.QueryString["ParentMenuID"].ToString();
                }
                if (Session["Menu_chkIsActive"]!=null)
                {
                    chkIsActive.Checked = (bool)Session["Menu_chkIsActive"];
                }

                PopulateMenuDDL(ref ddlShowUnder);

                if (Request.QueryString["default"] != null)
                {
                    if (ddlShowUnder.Items.FindByValue(Request.QueryString["default"].ToString()) != null)
                        ddlShowUnder.SelectedValue = Request.QueryString["default"].ToString();

                    hfParentMenuID.Value = Request.QueryString["default"].ToString();
                }


                //if (Request.QueryString["SearchCriteria"] != null)
                //{
                //    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
                //}

                            

                MakeMenuPath();

                //if (Request.QueryString["SearchCriteria"] != null)
                //{
                //    gvTheGrid.PageSize = _iMaxRows;
                //    gvTheGrid.GridViewSortColumn = _strGridViewSortColumn;
                //    if (_strGridViewSortDirection.ToUpper() == "ASC")
                //    {
                //        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                //    }
                //    else
                //    {
                //        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                //    }
                //    BindTheGrid(_iStartIndex, _iMaxRows);
                //}
                //else
                //{
                    gvTheGrid.GridViewSortColumn = "Menu";
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                //}                

                               
            }
            else
            {
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.AddURL = GetAddURL();
            }

            string strTitle = "Edit Menu";

            switch (_strActionMode.ToLower())
            {
                case "add":
                    divDelete.Visible = false;
                    divUnDelete.Visible = false;
                    strTitle = "Add Menu";
                    break;

                case "view":


                    strTitle = "View Menu";
                    break;

                case "edit":

                    strTitle = "Edit Menu";
                    if (!IsPostBack)
                        PopulateTheRecord();

                    break;


                default:
                    //?

                    break;
            }

            if(!IsPostBack)
            {
                

                if(hfParentMenuID.Value!="" && hfParentMenuID.Value!="-1")
                {
                    hlBack.NavigateUrl = GetEditURL() + Cryptography.Encrypt( hfParentMenuID.Value);
                }
                else
                {
                    
                    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx";

                    if(_strActionMode=="" && (hfParentMenuID.Value=="" || hfParentMenuID.Value=="-1") )
                    {
                        trEditPart.Visible = false;
                    }

                }
            }



            Title = strTitle;
            lblTitle.Text = strTitle;

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Menu Load", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
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

                //txtMenuSearch.Text = xmlDoc.FirstChild[txtMenuSearch.ID].InnerText;
                chkIsActive.Checked = bool.Parse(xmlDoc.FirstChild[chkIsActive.ID].InnerText);
                hfParentMenuID.Value = xmlDoc.FirstChild[hfParentMenuID.ID].InnerText;
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




    public string GetEditURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx?mode=" + Cryptography.Encrypt("edit") +  "&MenuID=";

    }

    public string GetAddURL()
    {
        if (hfParentMenuID.Value == "")
            hfParentMenuID.Value = "-1";

        string strparentForAdd = hfParentMenuID.Value;

        if (_iMenuID != null)
            strparentForAdd = _iMenuID.ToString();
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx?mode=" + Cryptography.Encrypt("add") + "&default=" + strparentForAdd;

    }

    public string GetRoot()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx";

    }
    //public string GetViewURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableGroup.aspx?SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&ParentMenuID=";

    //}

    //public string GetAccountViewURL()
    //{

    //    return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&accountid=";

    //}


    protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
    {
        
        Session["Menu_chkIsActive"] = chkIsActive.Checked;

        lnkSearch_Click(null, null);
    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {

        //lblMsg.Text = "";

        //SearchCriteria 
        //try
        //{
        //    string xml = null;
        //    xml = @"<root>" +
        //           " <" + txtMenuSearch.ID + ">" + HttpUtility.HtmlEncode(txtMenuSearch.Text) + "</" + txtMenuSearch.ID + ">" +
        //           " <" + chkIsActive.ID + ">" + HttpUtility.HtmlEncode(chkIsActive.Checked.ToString()) + "</" + chkIsActive.ID + ">" +
        //           " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
        //           " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
        //           " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
        //           " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
        //            " <" + hfParentMenuID.ID + ">" + HttpUtility.HtmlEncode(hfParentMenuID.Value) + "</" + hfParentMenuID.ID + ">" +
        //          "</root>";

        //    SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
        //    _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
        //}
        //catch (Exception ex)
        //{
        //    lblMsg.Text = ex.Message;
        //}

        //End Searchcriteria



        
        try
        {
            int iTN = 0;
            if (hfParentMenuID.Value == "")
                hfParentMenuID.Value = "-1";

            int? iParentMenuID = int.Parse(hfParentMenuID.Value);

            if (_iMenuID != null)
                iParentMenuID = _iMenuID;
            ViewState[gvTheGrid.ID + "PageIndex"] = (iStartIndex / gvTheGrid.PageSize) + 1;
            gvTheGrid.DataSource = RecordManager.ets_Menu_Select(null,
                "",
                null, int.Parse(Session["AccountID"].ToString()), !chkIsActive.Checked,
                "M.DisplayOrder",  "ASC" ,
                iStartIndex, iMaxRows, ref iTN, iParentMenuID, true);

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

                _gvPager.PageSize = gvTheGrid.PageSize;
                _gvPager.TotalRows = iTN;
                //_gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
            }

            if (iTN == 0)
            {
                if (IsFiltered())
                {
                    //divNoFilter.Visible = true;
                    divEmptyData.Visible = false;
                }
                else
                {
                    divEmptyData.Visible = true;
                    //divNoFilter.Visible = false;
                }
                hplNewData.NavigateUrl = GetAddURL();
                //hplNewDataFilter.NavigateUrl = GetAddURL();
                if(_strActionMode=="add")
                {
                    //divNoFilter.Visible = false;
                    divEmptyData.Visible = false;
                    chkIsActive.Visible = false;
                }
            }
            else
            {
                divEmptyData.Visible = false;
                //divNoFilter.Visible = false;
            }

            //if(_theMenu!=null && (_theMenu.TableID!=null || _theMenu.DocumentID!=null 
            //    || _theMenu.CustomPageLink!="" || _theMenu.ExternalPageLink!="" ))
            //{
            //    divEmptyData.Visible = false;
            //    divNoFilter.Visible = false;
            //    chkIsActive.Visible = false;
            //}

            lblMsg.Text = "";
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Group", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void MakeMenuPath()
    {
        string strMenu = "";
        
        GetMenuPath(_iMenuID==null? int.Parse(hfParentMenuID.Value):(int)_iMenuID, ref strMenu);

        //strMenu = "<a href='javascript:SetMenu(-1)'>Top Level</a>/" + strMenu;

        strMenu = "<a href='" + GetRoot() + "'>Top Level</a>/" + strMenu;

        lblCurrentMenu.Text = strMenu;
    }
    protected void GetMenuPath(int iMenuID, ref string strMenu)
    {
        //Folder theFolder = DocumentManager.ets_Folder_Detail(iFolderID, null, null);
        Menu theMenu = RecordManager.ets_Menu_Details(iMenuID);
        if (theMenu != null)
        {
            //strMenu = "<a href='javascript:SetMenu(" + iMenuID.ToString() + ")'>" + theMenu.MenuP + "</a>/" + strMenu;

            strMenu = "<a href='"+ GetEditURL() + Cryptography.Encrypt( iMenuID.ToString()) + "'>" + theMenu.MenuP + "</a>/" + strMenu;

            if (theMenu.ParentMenuID != null)
            {
                GetMenuPath((int)theMenu.ParentMenuID, ref strMenu);
            }
        }

    }
   
    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }


    protected void gvTheGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName == "delete")
        //{
        //    try
        //    {
        //        RecordManager.ets_Column_Delete(Convert.ToInt32(e.CommandArgument));

        //        BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);

        //        _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
        //        if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
        //        {
        //            BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog theErrorLog = new ErrorLog(null, "Table Group", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
        //        SystemData.ErrorLog_Insert(theErrorLog);
        //        lblMsg.Text = ex.Message;

        //        //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete failed!');", true);
        //    }
        //}
        //if (e.CommandName == "uporder")
        //{
        //    try
        //    {
        //        RecordManager.ets_Menu_OrderChange(Convert.ToInt32(e.CommandArgument), true);

        //        BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog theErrorLog = new ErrorLog(null, "Table Group", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
        //        SystemData.ErrorLog_Insert(theErrorLog);
        //        lblMsg.Text = ex.Message;

        //        //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Order failed!');", true);
        //    }
        //}
        //if (e.CommandName == "downorder")
        //{
        //    try
        //    {
        //        RecordManager.ets_Menu_OrderChange(Convert.ToInt32(e.CommandArgument), false);

        //        BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);


        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog theErrorLog = new ErrorLog(null, "Table Group", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
        //        SystemData.ErrorLog_Insert(theErrorLog);
        //        lblMsg.Text = ex.Message;

        //        //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Order failed!');", true);
        //    }
        //}

        //Response.Redirect(Request.RawUrl, false);
    }

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

            Label lblType = (Label)e.Row.FindControl("lblType");

            Label lblMenuP = (Label)e.Row.FindControl("lblMenuP");
            //HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");

            if (DataBinder.Eval(e.Row.DataItem, "TableID") == DBNull.Value && DataBinder.Eval(e.Row.DataItem, "Menu").ToString() != Common.MenuDividerText)
            {
                lblType.Text = "Menu";
               
                
            }
            else
            {
                lblType.Text = "Table";
               
            }
            if (DataBinder.Eval(e.Row.DataItem, "DocumentID") != DBNull.Value)
            {
                lblType.Text = "Report";
                
            }

            if (DataBinder.Eval(e.Row.DataItem, "Menu").ToString() == Common.MenuDividerText)
            {
                lblType.Text = "Divider";               
                lblMenuP.Text = "-- Divider --";
               
            }

            //if (DataBinder.Eval(e.Row.DataItem, "CustomPageLink")!=null && !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "CustomPageLink").ToString()))
            //{
            //    lblType.Text = "Custom";
               
            //}

            if (DataBinder.Eval(e.Row.DataItem, "ExternalPageLink") != DBNull.Value && !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "ExternalPageLink").ToString()))
            {
                lblType.Text = "Link";

            }

            if (DataBinder.Eval(e.Row.DataItem, "DocumentTypeID") != DBNull.Value
                || (DataBinder.Eval(e.Row.DataItem, "MenuType") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "MenuType").ToString()=="doc"))
            {
                lblType.Text = "Documents";

            }
           
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


    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
        MakeMenuPath();
    }
    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        //string strTable = "Table";
        //strTable = SecurityManager.etsTerminology("", "Table", "Table");

        _gvPager.ExportFileName = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " Groups";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {

        txtMenuSearch.Text = "";
        chkIsActive.Checked = false;
        gvTheGrid.GridViewSortColumn = "Menu";
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
            Response.Redirect(Request.RawUrl, false);
            //BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
            //_gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
            //if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
            //{
            //    BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
            //}
        }

    }



    private void DeleteItem(string keys)
    {
        string strID = "";
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
                        RecordManager.ets_Menu_Delete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Record Group Delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            Menu oMenu = RecordManager.ets_Menu_Details(int.Parse(strID));

            if (ex.Message.IndexOf("FK_Table_Menu") == -1)
            {
                lblMsg.Text = ex.Message;
            }
            else
            {
                string strTable = "Table";
                strTable = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table");
                lblMsg.Text = "Delete failed! " + strTable + " Group -" + oMenu.MenuP + "- has " + strTable + "s, please remove those " + strTable + "s then try again to delete.";

            }
        }
    }



    protected void Pager_OnExportForCSV(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
        "attachment;filename=Menus.csv");
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

    protected bool IsFiltered()
    {
        if (txtMenuSearch.Text != "" 
            || chkIsActive.Checked != false)
        {
            return true;
        }

        return false;
    }
  
}
