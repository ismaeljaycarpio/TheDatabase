using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Record_FormSetDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iFormSetID;
    string _qsMode = "";
    string _qsFormSetID = "";
    int? _iParentTableID = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;

        _iParentTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));

        string strChildTables = @"
                    $(function () {
                            $('.popuplink').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 800,
                                height: 450,
                                titleShow: false
                            });
                        });

                ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormSet", strChildTables, true);

        
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


                if (Request.QueryString["FormSetID"] != null)
                {

                    _qsFormSetID = Cryptography.Decrypt(Request.QueryString["FormSetID"]);

                    _iFormSetID = int.Parse(_qsFormSetID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
        


        // checking permission
        string strTitle = "Add Form";

        switch (_strActionMode.ToLower())
        {
            case "add":


                if (!IsPostBack)
                {
                    int iTotalRecords = 0;
                    DataTable dtFormGroups = FormSetManager.dbg_FormSetGroup_Select(_iParentTableID, "", null, null, "", "", null, null, ref iTotalRecords);

                    if (dtFormGroups.Rows.Count == 0)
                    {
                        //create one FormSetGroup

                        FormSetGroup firstFormSetGroup = new FormSetGroup(null, "Forms", 1, _iParentTableID, false);
                        int iFormSetGroupID= FormSetManager.dbg_FormSetGroup_Insert(firstFormSetGroup);
                        hfFormSetGroupID.Value = iFormSetGroupID.ToString();

                    }
                    else
                    {

                        hfFormSetGroupID.Value = dtFormGroups.Rows[0]["FormSetGroupID"].ToString();
                    }

                }

                break;

            case "view":


                strTitle = "View Form";
                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":
                strTitle = "Edit Form";
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
        lblTitle.Text =strTitle;

        if (!IsPostBack)
        {
            hlAddFormSetForm.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetFormDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&FormSetID=" + Cryptography.Encrypt(_qsFormSetID);

        }

    }

     
    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<FormSet> listFormSet = SystemData.FormSet_Select(_iFormSetID, "", "", "", null, null, "FormSetID", "ASC", null, null, ref iTemp);

            FormSet theFormSet = FormSetManager.dbg_FormSet_Detail((int)_iFormSetID);
            hfFormSetGroupID.Value = theFormSet.FormSetGroupID.ToString();

            txtFormSetName.Text = theFormSet.FormSetName;

            PopulatePages();
            if (_strActionMode == "edit")
            {
                ViewState["theFormSet"] = theFormSet;
            }
           
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "From Set Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtFormSetName.Enabled = p_bEnable;
      
            

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }

    protected void grdFormSetFormt_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetype")
        {
            FormSetManager.dbg_FormSetForm_Delete(int.Parse(e.CommandArgument.ToString()));
            PopulatePages();
        }
    }

    protected void grdFormSetForm_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            HyperLink hlAddDetail = e.Row.FindControl("hlAddDetail") as HyperLink;
            if (hlAddDetail != null)
            {
                hlAddDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetFormDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&FormSetID=" + Cryptography.Encrypt(_qsFormSetID);
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");



            HyperLink hlEditDetail = e.Row.FindControl("hlEditDetail") as HyperLink;

            if (hlEditDetail != null)
            {
                hlEditDetail.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/FormSetFormDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&FormSetFormID=" + Cryptography.Encrypt(DataBinder.Eval(e.Row.DataItem, "FormSetFormID").ToString()) + "&FormSetID=" + Cryptography.Encrypt(_qsFormSetID);
            }


        }



    }

    protected void PopulatePages()
    {
        DataTable dtPages = Common.DataTableFromText(@"SELECT FormSetFormID,TableName
            FROM FormSetForm FF INNER JOIN [Table] T
            ON FF.TableID=T.TableID 
            WHERE FormSetID=" + _qsFormSetID + " ORDER BY FF.DisplayOrder");

        grdFormSetForm.DataSource = dtPages;
        grdFormSetForm.DataBind();

        if (dtPages.Rows.Count == 0)
        {
            divFormSetForm.Visible = true;
        }
        else
        {
            divFormSetForm.Visible = false;
        }

    }


    protected void btnOrderFF_Click(object sender, EventArgs e)
    {
        //
        if (hfOrderFF.Value != "")
        {
            //SqlTransaction tn;
            //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
            //connection.Open();
            //tn = connection.BeginTransaction();

            try
            {
                string strNewFF = hfOrderFF.Value.Substring(0, hfOrderFF.Value.Length - 1);
                string[] newFF = strNewFF.Split(',');

                //string strFilter = "";

                //if (chkShowSystemFields.Checked == false)
                //    strFilter = " IsStandard=0 AND ";

                DataTable dtDO = Common.DataTableFromText("SELECT DisplayOrder,FormSetFormID FROM [FormSetForm] WHERE FormSetFormID IN (" + strNewFF + ") ORDER BY DisplayOrder");
                if (newFF.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newFF.Length; i++)
                    {
                        Common.ExecuteText("UPDATE FormSetForm SET DisplayOrder =" + dtDO.Rows[i][0].ToString() + " WHERE FormSetFormID=" + newFF[i]);

                    }
                }


                
            }
            catch (Exception ex)
            {

              //

            }
            PopulatePages();
        }
    }

    protected void btnRefreshPages_Click(object sender, EventArgs e)
    {
        PopulatePages();
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


                        string strRowPosition = Common.GetValueFromSQL("SELECT MAX(RowPosition) FROM FormSet WHERE FormSetGroupID=" + hfFormSetGroupID.Value);

                        int iRowPosition = 0;
                        if (strRowPosition != "")
                        {
                            iRowPosition = int.Parse(strRowPosition) + 1;
                        }

                        FormSet newFormSet = new FormSet(null, int.Parse( hfFormSetGroupID.Value),iRowPosition,txtFormSetName.Text);

                        FormSetManager.dbg_FormSet_Insert(newFormSet);
                      

                        break;

                    case "view":


                        break;

                    case "edit":
                        FormSet editFormSet = (FormSet)ViewState["theFormSet"];

                        editFormSet.FormSetName = txtFormSetName.Text;

                        FormSetManager.dbg_FormSet_Update(editFormSet);


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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Form Set Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

     

    }
   
}
