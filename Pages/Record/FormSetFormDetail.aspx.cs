using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Record_FormSetFormDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iFormSetFormID;
    string _qsMode = "";
    string _qsFormSetFormID = "";
    FormSet _theFormSet;
    FormSetGroup _theFormSetGroup;
    protected void ddlTable_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateColumn();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //int iTemp = 0;

        string strJS = @"  $(document).ready(function () {
            
            $('#ctl00_HomeContentPlaceHolder_ddlUpdateColumn').change(function (e) {
                var strDDDC = $('#ctl00_HomeContentPlaceHolder_ddlUpdateColumn').val();
                if (strDDDC == '') {
                    $('#trUpdateColumnValue').fadeOut();
                }
                else {
                    if(strDDDC == null)
                    {
                       $('#trUpdateColumnValue').fadeOut();
                    }
                    else
                    {
                                        $('#trUpdateColumnValue').fadeIn();
                    }
                }
            });

           $('#ctl00_HomeContentPlaceHolder_ddlUpdateColumn').change(); 
        });";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JS Code", strJS, true);

        int iFormSetID = int.Parse(Cryptography.Decrypt(Request.QueryString["FormSetID"].ToString()));

        _theFormSet = FormSetManager.dbg_FormSet_Detail(iFormSetID);
        _theFormSetGroup = FormSetManager.dbg_FormSetGroup_Detail((int)_theFormSet.FormSetGroupID);
        if (!IsPostBack)
        {
            PoppulateForms();
            ddlFormSet.SelectedValue = _theFormSet.FormSetID.ToString();

            PoppulateTable();
            
            //if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
            //{ Response.Redirect("~/Default.aspx", false); }

            //if (Request.QueryString["SearchCriteria"] != null)
            //{

            //    hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/FormSetForm.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            //}
            //else
            //{

            //    Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/FormSetForm.aspx", false);//i think no need
            //}


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


                if (Request.QueryString["FormSetFormID"] != null)
                {

                    _qsFormSetFormID = Cryptography.Decrypt(Request.QueryString["FormSetFormID"]);

                    _iFormSetFormID = int.Parse(_qsFormSetFormID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
      


        // checking permission

        string strTitle = "Add Page";
        switch (_strActionMode.ToLower())
        {
            case "add":


                break;

            case "view":


                strTitle = "View page";
                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":
                strTitle = "Edit page";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }

        Title =strTitle;
        lblTitle.Text = strTitle;


    }

    protected void PoppulateTable()
    {


        DataTable dtTableChild = RecordManager.ets_TableChild_Select((int)_theFormSetGroup.ParentTableID);

        DataView dv = dtTableChild.DefaultView;
        dv.Sort = "ChildTableName ASC";
        DataTable dtTableChilds = dv.ToTable();

        ddlTable.DataSource = dtTableChilds;
        ddlTable.DataBind();

        ListItem liTemp = new ListItem("-- Please select --", "");
        ddlTable.Items.Insert(0, liTemp);

    }

    protected void PoppulateForms()
    {

       
        int iTR = 0;
        DataTable dtFormSet= FormSetManager.dbg_FormSet_Select((int)_theFormSet.FormSetGroupID, 
            null, "", "", "", null, null, ref iTR);
        ddlFormSet.DataSource = dtFormSet;
        ddlFormSet.DataBind();

       

    }

    protected void PopulateColumn()
    {
        if (ddlTable.SelectedValue == "")
            return;

        DataTable dtColumns = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column]
                    WHERE IsStandard=0 and TableID=" + ddlTable.SelectedValue);

        ddlUpdateColumn.DataSource = dtColumns;
        ddlUpdateColumn.DataBind();

        ListItem liTemp = new ListItem("-- Please select --", "");
        ddlUpdateColumn.Items.Insert(0, liTemp);

    }


    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<FormSetForm> listFormSetForm = SystemData.FormSetForm_Select(_iFormSetFormID, "", "", "", null, null, "FormSetFormID", "ASC", null, null, ref iTemp);

            FormSetForm theFormSetForm = FormSetManager.dbg_FormSetForm_Detail((int)_iFormSetFormID);

            ddlTable.SelectedValue = theFormSetForm.TableID.ToString();
            PopulateColumn();

            if (theFormSetForm.UpdateColumnID != null)
            {
                ddlUpdateColumn.SelectedValue = theFormSetForm.UpdateColumnID.ToString();
                txtUpdateColumnValue.Text = theFormSetForm.UpdateColumnValue;
            }
            txtIncompleteImage.Text = theFormSetForm.IncompleteImage;

            if (_strActionMode == "edit")
            {
                ViewState["theFormSetForm"] = theFormSetForm;
            }
            //else if (_strActionMode == "view")
            //{
            //    divEdit.Visible = true;
            //    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/FormSetFormDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&FormSetFormID=" + Cryptography.Encrypt(theFormSetForm.FormSetFormID.ToString());
            //}
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Form Set Form Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        ddlTable.Enabled = p_bEnable;
        ddlUpdateColumn.Enabled = p_bEnable;
        txtUpdateColumnValue.Enabled = p_bEnable;
        txtIncompleteImage.Enabled = p_bEnable;  

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

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        //Menu newMenu = new Menu(null, txtMenu.Text, int.Parse(ddlAccount.SelectedValue), chkShowOnMenu.Checked, "");
                        //SecurityManager.test_TestTable_Insert(newMenu);

                        string strDisplayOrder = Common.GetValueFromSQL(@"SELECT MAX(DisplayOrder) FROM 
                        FormSetForm WHERE FormSetID=" + _theFormSet.FormSetID.ToString());

                        int iDisplayOrder = 0;
                        if (strDisplayOrder != "")
                            iDisplayOrder = int.Parse(strDisplayOrder) + 1;

                        FormSetForm newFormSetForm = new FormSetForm(null, (int)_theFormSet.FormSetID, int.Parse(ddlTable.SelectedValue),
                            iDisplayOrder, ddlUpdateColumn.SelectedValue == "" ? null : (int?)int.Parse(ddlUpdateColumn.SelectedValue),
                            txtUpdateColumnValue.Text);
                        newFormSetForm.IncompleteImage = txtIncompleteImage.Text;

                        FormSetManager.dbg_FormSetForm_Insert(newFormSetForm);

                        break;

                    case "view":


                        break;

                    case "edit":
                        FormSetForm editFormSetForm = (FormSetForm)ViewState["theFormSetForm"];

                        editFormSetForm.FormSetID = int.Parse(ddlFormSet.SelectedValue);
                        editFormSetForm.TableID = int.Parse(ddlTable.SelectedValue);
                        if (ddlUpdateColumn.SelectedValue == "")
                        {
                            editFormSetForm.UpdateColumnID = null;
                            editFormSetForm.UpdateColumnValue = "";
                        }
                        else
                        {
                            editFormSetForm.UpdateColumnID = int.Parse(ddlUpdateColumn.SelectedValue);
                            editFormSetForm.UpdateColumnValue = txtUpdateColumnValue.Text;
                        }
                        editFormSetForm.IncompleteImage = txtIncompleteImage.Text;

                        FormSetManager.dbg_FormSetForm_Update(editFormSetForm);


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
            //Response.Redirect(hlBack.NavigateUrl, false);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Form set form Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

     

    }
   
}
