using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Data;

public partial class Pages_Record_TableProperty : SecurePage
{
    string _strActionMode = "add";
    string _qsMode = "";
    string _qsTableID = "";
    User _ObjUser;
    //bool _bFirstChangePosition = false;
    int _iSearchCriteriaID = -1;
    string _strNone = "";



    protected void PopulateRecordGroupDDL(int iAccountID)
    {
        //ddlMenu.Items.Clear();
        int iTemp = 0;
        DataTable dtMenu = RecordManager.ets_Menu_Select(null, string.Empty, null,
            iAccountID, true,
            "Menu", "ASC", null, null, ref iTemp, null, null);



        TheDatabaseS.PopulateMenuDDL(ref ddlMenu);
     

        System.Web.UI.WebControls.ListItem liNew = new System.Web.UI.WebControls.ListItem("--New--", "new");
        ddlMenu.Items.Insert(0, liNew);


        foreach (DataRow dr in dtMenu.Rows)
        {
            if (dr["Menu"].ToString() == "--None--" && dr["ParentMenuID"] == DBNull.Value)
            {
                _strNone = dr["MenuID"].ToString();

                if (ddlMenu.Items.FindByValue(_strNone) != null)
                    ddlMenu.Items.Remove(ddlMenu.Items.FindByValue(_strNone));

                System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("--None--", _strNone);
                ddlMenu.Items.Insert(1, liNone);
                break;
            }
        }


     

        if (!IsPostBack)
        {

            foreach (DataRow dr in dtMenu.Rows)
            {
                if (dr["Menu"].ToString() == "Tables")
                {
                    ddlMenu.SelectedValue = dr["MenuID"].ToString();
                    break;
                }
            }
        }




    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        {
            Response.Redirect("~/Default.aspx", false);

        }

        _ObjUser = (User)Session["User"];


        string strChildTables = @"
                    $(function () {
                            $('.popuplink').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 600,
                                height: 350,
                                titleShow: false
                            });
                        });

                ";



        //Title = "Detail";
        lblMsg.Text = "";

        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;

                if (Request.QueryString["TableID"] != null)
                {
                    _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"]);
                    hfTableID.Value = _qsTableID;

                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }

        }

        if (_strActionMode.ToLower() == "add")
        {
            tblAddButton.Visible = true;

            lblTitle.Text = "Add " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + ": Brand New";

        }
        else
        {

            tblAddButton.Visible = false;
        }

        try
        {

            if (!IsPostBack)
            {


                            DataTable dtActiveTableList = Common.DataTableFromText("SELECT * FROM [Table] WHERE IsActive=1 AND AccountID=" + Session["AccountID"].ToString());
                            if (dtActiveTableList.Rows.Count == 0)
                            {
                                
                                hfFirstTable.Value = "Yes";
                            }

                PopulateTerminology();

                Session["Fields"] = null;
                if (Session["Fields"] == null)
                {
                    DataTable dtFields = new DataTable();
                    dtFields.Columns.Add("DisplayName");
                    dtFields.Columns.Add("ColumnType");
                    dtFields.Rows.Add("", "text");
                    dtFields.AcceptChanges();
                    grdFields.DataSource = dtFields;
                    grdFields.DataBind();
                    Session["Fields"] = dtFields;
                }


                hfWebroot.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath;
                if (_qsMode == "add")
                {
                    if (hfFirstTable.Value == "Yes")
                    {
                        PopulateHelp("TableBrandNew_First");
                    }
                    else
                    {
                        PopulateHelp("TableBrandNew");
                    }
                    if (Request.QueryString["SearchCriteria"] != null)
                    {
                        hlBack2.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableOption.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                    }
                    else
                    {
                        hlBack2.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableList.aspx?MenuID=" + Request.QueryString["MenuID"];

                        if (Request.UrlReferrer != null)
                        {
                            if (Request.UrlReferrer.ToString().IndexOf("RecordList.aspx") > -1)
                            {
                                hlBack2.NavigateUrl = Request.UrlReferrer.ToString();
                            }
                            else if (Request.UrlReferrer.ToString().IndexOf("TableSheet.aspx") > -1)
                            {
                                //importinfo
                                hlBack2.NavigateUrl = Request.UrlReferrer.ToString();
                            }
                            else
                            {

                                hlBack2.NavigateUrl = Request.UrlReferrer.ToString();
                            }

                        }

                    }
                }
                else
                {
                    hlMenuEdit.Attributes.Add("Target", "_blank");
                    //hlBack2.NavigateUrl = "#";
                    //hlBack.Attributes.Add("onclick", "parent.$.fancybox.close();");

                    if (Request.QueryString["SearchCriteria"] != null)
                    {
                        hlBack2.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableOption.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                    }
                    else
                    {
                        hlBack2.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableList.aspx?MenuID=" + Request.QueryString["MenuID"];

                        if (Request.UrlReferrer != null)
                        {
                            if (Request.UrlReferrer.ToString().IndexOf("RecordList.aspx") > -1)
                            {
                                //importinfo
                                hlBack2.NavigateUrl = Request.UrlReferrer.ToString();
                            }
                            else if (Request.UrlReferrer.ToString().IndexOf("TableSheet.aspx") > -1)
                            {
                                //importinfo
                                hlBack2.NavigateUrl = Request.UrlReferrer.ToString();
                            }
                            else
                            {

                                hlBack2.NavigateUrl = Request.UrlReferrer.ToString();
                            }

                        }

                    }


                }


                //PopulateWarningEmail();



                PopulateRecordGroupDDL(int.Parse(Session["AccountID"].ToString()));
                if (Request.QueryString["MenuID"] != null)
                {
                    if (Cryptography.Decrypt(Request.QueryString["MenuID"]) != "-1")
                    {
                        ddlMenu.Text = Cryptography.Decrypt(Request.QueryString["MenuID"]);
                    }

                }

            }









            if (!IsPostBack)
            {
                try
                {

                    DataTable dtTemp = Common.DataTableFromText("SELECT COUNT(*) FROM [Table] WHERE AccountID=" + Session["AccountID"].ToString());

                    if (dtTemp.Rows.Count > 0)
                    {
                        int iTotalST = int.Parse(dtTemp.Rows[0][0].ToString());

                        if (iTotalST > 17)
                            iTotalST = iTotalST % 17;

                        ddlPinImages.SelectedIndex = iTotalST + 1;
                    }
                }
                catch
                {
                    //
                }
            }








            Title = lblTitle.Text;


            ScriptManager.RegisterStartupScript(this, this.GetType(), "ChildTables", strChildTables, true);


            string strJS = @"   $(document).ready(function () {
                                $('#ctl00_HomeContentPlaceHolder_ddlMenu').change(function (e) {
                                    if (document.getElementById('ctl00_HomeContentPlaceHolder_ddlMenu').value == 'new') {
                                        $('#trNewMenuName').fadeIn();
                                        var txtNewMenuName = document.getElementById('ctl00_HomeContentPlaceHolder_txtNewMenuName');
                                        txtNewMenuName.value ='';
                                        $('#ctl00_HomeContentPlaceHolder_lblMsg').text('');
                                    }
                                    else {
                                        $('#trNewMenuName').fadeOut();
                                        var txtNewMenuName = document.getElementById('ctl00_HomeContentPlaceHolder_txtNewMenuName');
                                        txtNewMenuName.value = '';
                                        $('#ctl00_HomeContentPlaceHolder_lblMsg').text('');
                                    }
                                });

                            });";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "trNewMenuName1", strJS, true);


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }






    protected void PopulateHelp(string strContentKey)
    {
        Content theContent = SystemData.Content_Details_ByKey(strContentKey, null);

        if (theContent != null)
        {
            lblHelpContent.Text = theContent.ContentP;
        }
    }


    protected void PopulateTerminology()
    {

        stgNameCaption.InnerText = "Name for the new " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table").ToLower() + "*:";
        //lblAddFields.Text = "Add your " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Fields", "Fields").ToLower() + ":";
        grdFields.Columns[0].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field") + " Name";

    }



    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        DataTable dtFields = new DataTable();
        dtFields.Columns.Add("DisplayName");
        dtFields.Columns.Add("ColumnType");

        for (int i = 0; i < grdFields.Rows.Count; i++)
        {
            string strDisplayName = ((TextBox)grdFields.Rows[i].FindControl("txtDisplayName")).Text;
            string strColumnType = ((DropDownList)grdFields.Rows[i].FindControl("ddlType")).SelectedValue;
            if (strDisplayName != "")
            {
                dtFields.Rows.Add(strDisplayName, strColumnType);

            }
        }


        dtFields.Rows.Add("", "text");
        dtFields.AcceptChanges();
        grdFields.DataSource = dtFields;
        grdFields.DataBind();
        Session["Fields"] = dtFields;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHide2", "ShowHide();", true);

    }

    protected void grdFields_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtDisplayName = (TextBox)e.Row.FindControl("txtDisplayName");
            txtDisplayName.Text = DataBinder.Eval(e.Row.DataItem, "DisplayName").ToString();


            DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlType");
            ddlType.Text = DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString();
        }
    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }
    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (IsUserInputOK())
            {

                int iParentMenuID = -1;

                if (ddlMenu.SelectedValue == "")
                {
                    Menu newMenu = new Menu(null, "--None--",
                int.Parse(Session["AccountID"].ToString()), false,  true);
                    iParentMenuID = RecordManager.ets_Menu_Insert(newMenu);
                }
                else if (ddlMenu.SelectedValue == "new")
                {
                    if (txtNewMenuName.Text == "")
                    {
                        lblMsg.Text = "New Menu Name - Required.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "trNewMenuName2", "$('#trNewMenuName').fadeIn();", true);
                        return;
                    }

                    if (hfFirstTable.Value == "Yes")
                    {
                        DataTable dtActiveMenuList = Common.DataTableFromText("SELECT * FROM [Menu] WHERE Menu='Tables' AND AccountID=" + Session["AccountID"].ToString());

                        if (dtActiveMenuList.Rows.Count == 0)
                        {
                            txtNewMenuName.Text = "Tables";
                        }
                    }

                    Menu newMenu = new Menu(null, txtNewMenuName.Text,
                int.Parse(Session["AccountID"].ToString()), true,  true);
                    iParentMenuID = RecordManager.ets_Menu_Insert(newMenu);
                }
                else
                {

                    iParentMenuID = int.Parse(ddlMenu.SelectedValue);
                }


                Table newTable = new Table(null,
                    txtTable.Text,                   
                    null, null, false, true);
                newTable.AccountID = int.Parse(Session["AccountID"].ToString());



                newTable.PinImage = ddlPinImages.SelectedValue;


                int iTableID = RecordManager.ets_Table_Insert(newTable,iParentMenuID);





                for (int i = 0; i < grdFields.Rows.Count; i++)
                {
                    string strDisplayName = ((TextBox)grdFields.Rows[i].FindControl("txtDisplayName")).Text;
                    string strColumnType = ((DropDownList)grdFields.Rows[i].FindControl("ddlType")).SelectedValue;

                    if (strDisplayName != "")
                    {
                        string strAutoSystemName = "";

                        strAutoSystemName = RecordManager.ets_Column_NextSystemName(iTableID);
                        if (strAutoSystemName == "NO")
                        {
                            lblMsg.Text = "You can add 100 columns, there is no column available!";
                            return;
                        }


                        int? iDisplayOrder = RecordManager.ets_Table_MaxOrder(iTableID);

                        if (iDisplayOrder == null)
                            iDisplayOrder = -1;

                        Column newColumn = new Column(null, iTableID,
                       strAutoSystemName, iDisplayOrder + 1, strDisplayName, strDisplayName, "", "", null, "",
                        "", null, null, "", "", false, strDisplayName, null, "", null, null, false, "", "", "");

                        newColumn.ColumnType = strColumnType;

                        if(i==0)
                        newColumn.SummarySearch = true;

                        switch (newColumn.ColumnType)
                        {
                            case "checkbox":
                                newColumn.DropdownValues = "Yes" + Environment.NewLine + "No"
                       + Environment.NewLine + "no";


                                newColumn.NameOnImport = strDisplayName;
                                newColumn.NameOnExport = strDisplayName;

                                break;
                            case "staticcontent":
                                newColumn.DisplayTextSummary = "";
                                newColumn.DropdownValues = "Content goes here";
                                break;
                            case "content":
                                newColumn.DisplayTextSummary = "";
                                break;
                            case "date_time":
                                newColumn.ColumnType = "date";
                                 newColumn.NameOnImport = strDisplayName;
                                newColumn.NameOnExport = strDisplayName;

                                break;
                            case "dropdown":
                                newColumn.DropDownType = "values";
                                newColumn.DropdownValues = "Item 1" + Environment.NewLine + "Item 2"
                      + Environment.NewLine + "Item 3";

                                 newColumn.NameOnImport = strDisplayName;
                                newColumn.NameOnExport = strDisplayName;

                                break;
                            case "file":
                                //ok
                                 
                                newColumn.NameOnExport = strDisplayName;
                                break;
                            case "image":
                                //ok
                                
                                newColumn.NameOnExport = strDisplayName;
                                break;
                            case "listbox":
                                newColumn.DropdownValues = "Item 1" + Environment.NewLine + "Item 2"
                    + Environment.NewLine + "Item 3";
                                newColumn.DropDownType = "values";
                                break;
                            case "location":
                                newColumn.ShowTotal = true;
                                newColumn.IsRound = true;
                                newColumn.TextHeight = 200;
                                newColumn.TextWidth = 400;

                                 newColumn.NameOnImport = strDisplayName;
                                newColumn.NameOnExport = strDisplayName;

                                break;
                            case "number":
                                newColumn.NumberType = 1;

                                 newColumn.NameOnImport = strDisplayName;
                                newColumn.NameOnExport = strDisplayName;

                                break;
                            case "radiobutton":
                                newColumn.DropdownValues = "Option 1" + Environment.NewLine + "Option 2"
                  + Environment.NewLine + "Option 3";
                                newColumn.DropDownType = "values";

                                 newColumn.NameOnImport = strDisplayName;
                                newColumn.NameOnExport = strDisplayName;

                                break;
                            case "text":
                                //
                                 newColumn.NameOnImport = strDisplayName;
                                newColumn.NameOnExport = strDisplayName;

                                break;

                        }

                        try
                        {
                            RecordManager.ets_Column_Insert(newColumn);
                        }
                        catch
                        {
                            //
                        }



                    }
                }



                Session["Fields"] = null;
                //Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(iTableID.ToString()) , false);


                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID="  + Cryptography.Encrypt(iTableID.ToString()) + "&MenuID=" + Request.QueryString["MenuID"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString(), false);



                return;
            }
            else
            {
                //user input is not ok

            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Table Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

            string strTable = "Table";
            strTable = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table");

            lblMsg.Text = "Can not add this " + strTable + ", you may have this " + strTable + "!";
        }
    }



}
