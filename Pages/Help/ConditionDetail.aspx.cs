using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Schedule_ConditionDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iConditionID;
    string _qsMode = "";
    string _qsConditionID = "-1";
    int _iColumnID = -1;
    User _ObjUser;
    string _strConTypeDisplay = "Validation";

    protected void Page_Load(object sender, EventArgs e)
    {

//        string strJS = @" $(document).ready(function () {
//
//              var sUser = $('#ddlUser').val();
//                if (sUser == '-1') {
//                    $('#tdReminderColumn').fadeIn();
//                }
//                else {
//                    $('#tdReminderColumn').fadeOut();
//                };
//            $('#ddlUser').change(function () {
//                var sUser = $('#ddlUser').val();
//                if (sUser == '-1') {
//                    $('#tdReminderColumn').fadeIn();
//                }
//                else {
//                    $('#tdReminderColumn').fadeOut();
//                }
//            });
//        });";

//        ScriptManager.RegisterStartupScript(this, this.GetType(), "strJS", strJS, true);

        _ObjUser = (User)Session["User"];

        if (!IsPostBack)
        {
            ViewState["ConditionType"] = Request.QueryString["ConditionType"].ToString().Trim();
            int iTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
            Table theTable = RecordManager.ets_Table_Details(iTableID);
            ViewState["theTable"] = theTable;
        }
           


        switch (ViewState["ConditionType"].ToString().ToUpper())
        {
            case "W":
                _strConTypeDisplay = "Warning";
                break;
            case "E":
                _strConTypeDisplay = "Exceedance";
                break;
            default:
                _strConTypeDisplay = "Validation";
                break;
        }


       _iColumnID = int.Parse(Request.QueryString["ColumnID"].ToString());
        if (!IsPostBack)
        {           

            PopulateCheckColumn();
           
            //if (Request.QueryString["SearchCriteria"] != null)
            //{
            //    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/Condition.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            //}
            //else
            //{
            hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/Condition.aspx?ConditionType=" + Request.QueryString["ConditionType"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString();//i think no need
            //}


        }
        if (Request.QueryString["mode"] == null)
        {
            Response.Redirect("~/Default.aspx",false);
            return;
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;


                if (Request.QueryString["ConditionID"] != null)
                {
                    _qsConditionID = Cryptography.Decrypt(Request.QueryString["ConditionID"]);
                   
                }
                else
                {
                    if (Session["Condition"] != null)
                    {
                        DataTable dtCondition=(DataTable)Session["Condition"];
                        if (dtCondition.Rows.Count > 0)
                        {
                            _qsConditionID = (int.Parse(dtCondition.Rows[dtCondition.Rows.Count - 1]["ConditionID"].ToString()) - 1).ToString();
                        }
                    }
                }

                _iConditionID = int.Parse(_qsConditionID);
            }
            else
            {
                Response.Redirect("~/Default.aspx", false);
                return;
            }


        }

        string strTitle = "Data Validation Conditions";
        


        // checking permission


        switch (_strActionMode.ToLower())
        {
            case "add":
                strTitle = "Add Data " + _strConTypeDisplay + " Conditions";
              
                break;

            case "view":

                strTitle = "View Data " + _strConTypeDisplay + " Conditions";


                PopulateTheRecord();

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Data " + _strConTypeDisplay + " Conditions";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }


        Title = strTitle;
        lblTitle.Text = strTitle;

    }

   

   
    protected void PopulateCheckColumn()
    {
        

        ddlCheckColumn.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE IsStandard=0 
                AND ColumnType not in ('button','staticcontent') AND TableID=" + ((Table)ViewState["theTable"]).TableID.ToString());
        ddlCheckColumn.DataBind();

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
        ddlCheckColumn.Items.Insert(0, liSelect);

    }

  

    protected void ddlCheckColumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlCheckColumn.SelectedValue=="")
        {
            cuiCheckValue.ColumnID = null;
        }
        else
        {
            cuiCheckValue.ColumnID=int.Parse(ddlCheckColumn.SelectedValue);
        }
    }


    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<Condition> listCondition = SystemData.Condition_Select(_iConditionID, "", "", "", null, null, "ConditionID", "ASC", null, null, ref iTemp);
            TextBox txtFull = new TextBox();
            bool bAdvanced = false;
            Condition theCondition = UploadWorld.dbg_Condition_Detail((int)_iConditionID);

            if (theCondition == null && Session["Condition"]!=null)
            {
                DataTable dtCondition = (DataTable)Session["Condition"];
                DataRow[] drRows = dtCondition.Select("ConditionID='" + _iConditionID.ToString() + "'");

                foreach (DataRow dr in drRows)
                {

                    theCondition = new Condition(int.Parse(dr["ConditionID"].ToString()), _iColumnID, ViewState["ConditionType"].ToString(),
                        int.Parse(dr["CheckColumnID"].ToString()), dr["CheckFormula"].ToString(), dr["CheckValue"].ToString());                    

                    break;
                }
            }

            if (theCondition != null)
            {
                if (ddlCheckColumn.Items.FindByValue(theCondition.CheckColumnID.ToString()) != null)
                    ddlCheckColumn.SelectedValue = theCondition.CheckColumnID.ToString();
                ddlCheckColumn_SelectedIndexChanged(null, null);

                cuiCheckValue.ColumnValue = theCondition.CheckValue;
                Common.ShowFromula(theCondition.CheckFormula, ref txtMin, ref txtMax, ref txtFull, ref bAdvanced);
            }          

            if (_strActionMode == "edit")
            {
                ViewState["theCondition"] = theCondition;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/ConditionDetail.aspx?ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&mode=" + Cryptography.Encrypt("edit") + "&ConditionID=" + Cryptography.Encrypt(theCondition.ConditionID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Condition Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }


    protected void EnableTheRecordControls(bool p_bEnable)
    {
        //txtDays.Enabled = p_bEnable;
        //txtReminderHeader.Enabled = p_bEnable;
        //edtContent.Enabled = p_bEnable;
        //ddlUser.Visible = p_bEnable;
        //grdUsers.Enabled = p_bEnable;
        //lnkAddUser.Visible = p_bEnable;
        txtMax.Enabled = false;
        txtMin.Enabled = false;
        //cuiCheckValue.EnableDynamicData = false;
        ddlCheckColumn.Enabled = false;
    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action
        string strFullFormula = Common.GetFullFormula(txtMin.Text.Trim(), txtMax.Text.Trim(), "");
        if (strFullFormula=="")
        {
            lblMsg.Text = "Please select Greater Than, or Less Than or Both.";
            return false;
        }

        return true;
    }



    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsUserInputOK())
            {
                string strFullFormula = Common.GetFullFormula(txtMin.Text.Trim(), txtMax.Text.Trim(), "");

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        Condition newCondition = new Condition(null, _iColumnID,ViewState["ConditionType"].ToString(),
                              int.Parse(ddlCheckColumn.SelectedValue), strFullFormula, cuiCheckValue.ColumnValue);
                        if (_iColumnID == -1)
                        {
                            DataTable dtCondition = (DataTable)Session["Condition"];

                            dtCondition.Rows.Add(int.Parse(_qsConditionID),_iColumnID, newCondition.ConditionType,
                                newCondition.CheckColumnID, newCondition.CheckValue, newCondition.CheckFormula,ddlCheckColumn.SelectedItem.Text);

                            dtCondition.AcceptChanges();

                            Session["Condition"] = dtCondition;

                        }
                        else
                        {                           

                            int iConditionID = UploadWorld.dbg_Condition_Insert(newCondition);
                          
                        }


                        break;

                    case "view":


                        break;

                    case "edit":

                        Condition editCondition = (Condition)ViewState["theCondition"];

                        editCondition.ConditionType = ViewState["ConditionType"].ToString();
                        editCondition.CheckColumnID = int.Parse(ddlCheckColumn.SelectedValue);
                        editCondition.CheckValue = cuiCheckValue.ColumnValue;
                        editCondition.CheckFormula = strFullFormula;
                        if (_iColumnID == -1)
                        {
                            DataTable dtCondition = (DataTable)Session["Condition"];

                            for (int i = 0; i < dtCondition.Rows.Count; i++)
                            {
                                if (dtCondition.Rows[i]["ConditionID"].ToString() == _qsConditionID)
                                {
                                    dtCondition.Rows[i]["ConditionType"] = editCondition.ConditionType;
                                    dtCondition.Rows[i]["CheckColumnID"] = (int)editCondition.CheckColumnID;
                                    dtCondition.Rows[i]["CheckValue"] = editCondition.CheckValue;
                                    dtCondition.Rows[i]["CheckFormula"] = editCondition.CheckFormula;
                                    dtCondition.Rows[i]["DisplayName"] = ddlCheckColumn.SelectedItem.Text;
                                }
                            }

                            dtCondition.AcceptChanges();

                            Session["Condition"] = dtCondition;
                         
                        }
                        else
                        {                          
                            UploadWorld.dbg_Condition_Update(editCondition);
                           
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
            Response.Redirect(hlBack.NavigateUrl, false);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, lblTitle.Text, ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //if (ex.Message.IndexOf("FK_Condition_Column") > -1)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please add the Column first and then try again.');", true);
            //}

        }



    }

}
