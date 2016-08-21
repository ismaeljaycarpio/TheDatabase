using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
public partial class Pages_Export_ExportTemplateItem : SecurePage
{

    string _strActionMode = "view";
    int? _iExportTemplateID;
    string _qsMode = "";
    string _qsExportTemplateID = "";
    Common_Pager _ExportTemplateItemPager;

    protected void PopulateTerminology()
    {
        stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;


        string strJSForSortingExportTemplateItem = @"

                         $(document).ready(function () {


                                    $(function () {
                                        $('#divExportTemplateItemSingleInstance').sortable({
                                            items: '.gridview_row',
                                            cursor: 'crosshair',
                                            helper: fixHelper,
                                            cursorAt: { left: 10, top: 10 },
                                            connectWith: '#divExportTemplateItemSingleInstance',
                                            handle: '.sortHandleVT',
                                            axis: 'y',
                                            distance: 15,
                                            dropOnEmpty: true,
                                            receive: function (e, ui) {
                                                $(this).find('tbody').append(ui.item);

                                            },
                                            start: function (e, ui) {
                                                ui.placeholder.css('border-top', '2px solid #00FFFF');
                                                ui.placeholder.css('border-bottom', '2px solid #00FFFF');

                                            },
                                            update: function (event, ui) {
                                                var TC = '';
                                                $('.ExportTemplateItemID').each(function () {
                                                    TC = TC + this.value.toString() + ',';
                                                });
                                                //alert(TC);
                                                document.getElementById('hfExportTemplateItemIDForColumnIndex').value = TC;
                        
                                                $('#btnExportTemplateItemIDForColumnIndex').trigger('click');

                                            }
                                        });
                                    });

                                });

                        ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "strJSForSortingExportTemplateItem", strJSForSortingExportTemplateItem, true);



        string strExportTemplateItemPop = @"
                    $(function () {
                            $('.popuplinkVT').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 800,
                                height: 500,
                                titleShow: false
                            });
                        });

                ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "strExportTemplateItemPop", strExportTemplateItemPop, true);




        if (!IsPostBack)
        {
            PopulateTableDDL();
            //PopulateLocationDDL();
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            if (Request.QueryString["SearchCriteriaET"] != null)
            {

                PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteriaET"].ToString())));

                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaET=" + Request.QueryString["SearchCriteriaET"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString();
            }
            else
            {

               // Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaET=" + Cryptography.Encrypt("-1") + "&TableID=" + Request.QueryString["TableID"].ToString(), false);//i think no need
            }

            if (Request.QueryString["fixedbackurl"] != null)
             {
                 hlBack.NavigateUrl = Cryptography.Decrypt(Request.QueryString["fixedbackurl"].ToString());
             }

        }
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


                if (Request.QueryString["ExportTemplateID"] != null)
                {

                    _qsExportTemplateID = Cryptography.Decrypt(Request.QueryString["ExportTemplateID"]);

                    _iExportTemplateID = int.Parse(_qsExportTemplateID);

                    if (!IsPostBack)
                    {
                        PopulateExportTemplateItem((int)_iExportTemplateID);

                        if (Request.QueryString["popupitem"] != null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopItem", "  setTimeout(function () { OpenExportTemplateItem(); }, 1000); ", true);

                        }
                    }
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }



        GridViewRow ExportTemplateItemPager = grdExportTemplateItem.TopPagerRow;
        if (ExportTemplateItemPager != null)
            _ExportTemplateItemPager = (Common_Pager)ExportTemplateItemPager.FindControl("ExportTemplateItemPager");



        // checking permission


        switch (_strActionMode.ToLower())
        {
            case "add":
                lblTitle.Text = "Add Export Template";
                

                break;

            case "view":

                lblTitle.Text = "View Export Template";



                if (!IsPostBack)
                 PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                lblTitle.Text = "Edit Export Template";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }

        Title = lblTitle.Text;

        if (!IsPostBack)
        {
            PopulateTerminology();
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

                ddlTable.Text = xmlDoc.FirstChild[ddlTable.ID].InnerText;
               
            }
            else
            {
                ddlTable.Text = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }



    protected void ExportTemplateItemPager_DeleteAction(object sender, EventArgs e)
    {

   
        string sCheck = "";
     
        for (int i = 0; i < grdExportTemplateItem.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)grdExportTemplateItem.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)grdExportTemplateItem.Rows[i].FindControl("LblID")).Text + ",";
            }
        }

        



        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message_alert", "alert('Please select a record.');", true);

            return;
        }

        sCheck = sCheck + "-1";

        Common.ExecuteText("DELETE ExportTemplateItem WHERE ExportTemplateItemID IN (" + sCheck + ") ");

        PopulateExportTemplateItem((int)_iExportTemplateID);



    }


    protected void btnExportTemplateItemIDForColumnIndex_Click(object sender, EventArgs e)
    {
        //
        if (hfExportTemplateItemIDForColumnIndex.Value != "")
        {

            try
            {
                string strNewVIT = hfExportTemplateItemIDForColumnIndex.Value.Substring(0, hfExportTemplateItemIDForColumnIndex.Value.Length - 1);
                string[] newVT = strNewVIT.Split(',');

                //string strFilter = "";



                DataTable dtDO = Common.DataTableFromText("SELECT ColumnIndex,ExportTemplateItemID FROM [ExportTemplateItem] WHERE ExportTemplateItemID IN (" + strNewVIT + ") ORDER BY ColumnIndex");
                if (newVT.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newVT.Length; i++)
                    {
                        Common.ExecuteText("UPDATE ExportTemplateItem SET ColumnIndex =" + i.ToString() + " WHERE ExportTemplateItemID=" + newVT[i]);

                    }
                }


            }
            catch (Exception ex)
            {

                //

            }
            PopulateExportTemplateItem((int)_iExportTemplateID);
        }
    }


    public string GetAddExportTemplateItemURL(int iExportTemplateID)
    {
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItemDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&ExportTemplateID=" + Cryptography.Encrypt(iExportTemplateID.ToString());
    }


    protected void PopulateExportTemplateItem(int iExportTemplateID)
    {
        int iTN = 0;

        hlAddExportTemplateItem.NavigateUrl = GetAddExportTemplateItemURL(iExportTemplateID);
        hlAddExportTemplateItem2.NavigateUrl = hlAddExportTemplateItem.NavigateUrl;
        DataTable dtExportTemplateItems = Common.DataTableFromText("SELECT * FROM ExportTemplateItem WHERE ExportTemplateID=" + iExportTemplateID.ToString() + " ORDER BY ColumnIndex");

        grdExportTemplateItem.DataSource = dtExportTemplateItems;
        iTN = dtExportTemplateItems.Rows.Count;

        grdExportTemplateItem.VirtualItemCount = iTN;
        grdExportTemplateItem.DataBind();

        if (grdExportTemplateItem.TopPagerRow != null)
            grdExportTemplateItem.TopPagerRow.Visible = true;

        GridViewRow gvr = grdExportTemplateItem.TopPagerRow;



        if (gvr != null)
        {
            _ExportTemplateItemPager = (Common_Pager)gvr.FindControl("ExportTemplateItemPager");
            _ExportTemplateItemPager.AddURL = GetAddExportTemplateItemURL(iExportTemplateID);
            _ExportTemplateItemPager.HyperAdd_CSS = "popuplinkVT";
            _ExportTemplateItemPager.AddToolTip = "Add/Remove";
        }

        if (iTN == 0)
        {
            divEmptyAddExportTemplateItem.Visible = true;
        }
        else
        {
            divEmptyAddExportTemplateItem.Visible = false;
        }

    }

    protected void grdExportTemplateItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }


    protected void grdExportTemplateItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        //if (e.Row.RowType == DataControlRowType.Header)
        //{

        //    HyperLink hlAddDetail = e.Row.FindControl("hlAddDetail") as HyperLink;
        //    hlAddDetail.NavigateUrl = GetAddExportTemplateItemURL( int.Parse( hfCurrentExportTemplateID.Value));

        //}

        if (e.Row.RowType == DataControlRowType.DataRow)
        {



            Column theColumn = RecordManager.ets_Column_Details(int.Parse(DataBinder.Eval(e.Row.DataItem, "ColumnID").ToString()));
            if (theColumn != null)
            {
                Label lblFieldName = e.Row.FindControl("lblFieldName") as Label;
                if (lblFieldName != null)
                {
                    lblFieldName.Text = theColumn.DisplayName;

                }


            }


            TextBox txtHeading = e.Row.FindControl("txtHeading") as TextBox;
            if (txtHeading != null)
            {
                txtHeading.Text = DataBinder.Eval(e.Row.DataItem, "ExportHeaderName").ToString();

            }

           


        }

    }

    protected void btnRefreshExportTemplateItem_Click(object sender, EventArgs e)
    {
        if(_iExportTemplateID!=null)
            PopulateExportTemplateItem((int)_iExportTemplateID);
        
    }


    protected void ExportTemplateItemPager_BindTheGridAgain(object sender, EventArgs e)
    {
        PopulateExportTemplateItem((int)_iExportTemplateID);
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
        //if (iTN == 0)
        //{
        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddlTable.Items.Insert(0, liAll);
        //}


    }


    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        //PopulateLocationDDL();

    }

  

     
    protected void PopulateTheRecord()
    {
        try
        {

            ExportTemplate theExportTemplate = ExportManager.dbg_ExportTemplate_Detail((int)_iExportTemplateID);
            lnkRefesh.Visible = true;
            ddlTable.SelectedValue = theExportTemplate.TableID.ToString();

           
            txtExportTemplateName.Text = theExportTemplate.ExportTemplateName;
           

            if (_strActionMode == "edit")
            {
                ViewState["theExportTemplate"] = theExportTemplate;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString()
                    + "&SearchCriteriaET=" + Request.QueryString["SearchCriteriaET"].ToString() + "&ExportTemplateID=" + Cryptography.Encrypt(theExportTemplate.ExportTemplateID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "ExportTemplate Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtExportTemplateName.Enabled = p_bEnable;
       
        ddlTable.Enabled = p_bEnable;
        //ddlLocation.Enabled = p_bEnable;
            

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }

    protected void lnkRefesh_Click(object sender, EventArgs e)
    {
        try
        {
            if(_iExportTemplateID!=null)
            {

                ExportManager.spRefreshExportTemplateFields((int)_iExportTemplateID);
                PopulateExportTemplateItem((int)_iExportTemplateID);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ResetExportTemplateOrder", "alert('Done :-)');", true);

            }

        }
        catch
        {

        }
    }


    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        string strEditURL = hlBack.NavigateUrl;
        try
        {
            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        
                        ExportTemplate newExportTemplate = new ExportTemplate(null, int.Parse(ddlTable.SelectedValue),                        
                            txtExportTemplateName.Text);

                       int iNewExportTemplateID= ExportManager.dbg_ExportTemplate_Insert(newExportTemplate);

                       string strExtarQueryString = "";
                       if (Request.QueryString["SearchCriteriaET"] != null)
                           strExtarQueryString = "&SearchCriteriaET=" + Request.QueryString["SearchCriteriaET"].ToString();

                       if (Request.QueryString["fixedbackurl"] != null)
                           strExtarQueryString =strExtarQueryString+ "&fixedbackurl=" + Request.QueryString["fixedbackurl"].ToString();

                       strEditURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?popupitem=yes&mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&ExportTemplateID=" + Cryptography.Encrypt(iNewExportTemplateID.ToString()) + strExtarQueryString;



                        break;

                    case "view":


                        break;

                    case "edit":
                        
                        ExportTemplate editExportTemplate = (ExportTemplate)ViewState["theExportTemplate"];
                       
                        editExportTemplate.ExportTemplateName = txtExportTemplateName.Text; 
                        editExportTemplate.TableID = int.Parse(ddlTable.SelectedValue);
                       
                        ExportManager.dbg_ExportTemplate_Update(editExportTemplate);
                        
                        //now lets update Items



                        for (int i = 0; i < grdExportTemplateItem.Rows.Count; i++)
                        {

                            string strExportTemplateItemID = ((Label)grdExportTemplateItem.Rows[i].FindControl("LblID")).Text;

                            ExportTemplateItem theExportTemplateItem = ExportManager.dbg_ExportTemplateItem_Detail(int.Parse(strExportTemplateItemID));
                                                        
                            if (theExportTemplateItem != null)
                            {
                                theExportTemplateItem.ColumnIndex = i;
                                theExportTemplateItem.ExportHeaderName = ((TextBox)grdExportTemplateItem.Rows[i].FindControl("txtHeading")).Text;
                                ExportManager.dbg_ExportTemplateItem_Update(theExportTemplateItem);
                            }
                           
                        }



                        break;

                    default:
                        //?
                        break;
                }
            }
            else
            {
                //user input is not ok

            }
            Response.Redirect(strEditURL, false);

        }
        catch (Exception ex)
        {

            ErrorLog theErrorLog = new ErrorLog(null, "ExportTemplate Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
            
        }

     

    }
   
}
