using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Schedule_DataReminderDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iDataReminderID;
    string _qsMode = "";
    string _qsDataReminderID = "-1";
    int _iColumnID = -1;
    User _ObjUser;
    protected void Page_Load(object sender, EventArgs e)
    {

        string strJS = @" $(document).ready(function () {

              var sUser = $('#ddlUser').val();
                if (sUser == '-1') {
                    $('#tdReminderColumn').fadeIn();
                }
                else {
                    $('#tdReminderColumn').fadeOut();
                };
            $('#ddlUser').change(function () {
                var sUser = $('#ddlUser').val();
                if (sUser == '-1') {
                    $('#tdReminderColumn').fadeIn();
                }
                else {
                    $('#tdReminderColumn').fadeOut();
                }
            });
        });";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "strJS", strJS, true);

        _ObjUser = (User)Session["User"];


        edtContent.AssetManager = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";


        _iColumnID = int.Parse(Request.QueryString["ColumnID"].ToString());
        if (!IsPostBack)
        {
            PopulateDatabaseField();
            if (Session["DataReminderUser"] == null)
            {
                DataTable dtDataReminderUser = new DataTable();

                dtDataReminderUser.Columns.Add("DataReminderUserID");
                dtDataReminderUser.Columns.Add("DataReminderID");
                dtDataReminderUser.Columns.Add("UserID");
                dtDataReminderUser.Columns.Add("ReminderColumnID");
                dtDataReminderUser.Columns.Add("UserName");               
                dtDataReminderUser.Columns.Add("DisplayName");
                Session["DataReminderUser"] = dtDataReminderUser;
            }

            PopulateReminderColumn();
            PopulateUserDropDown();
            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminder.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {
                Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminder.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString(), false);//i think no need
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


                if (Request.QueryString["DataReminderID"] != null)
                {
                    _qsDataReminderID = Cryptography.Decrypt(Request.QueryString["DataReminderID"]);
                   
                }
                else
                {
                    if (Session["DataReminder"] != null)
                    {
                        DataTable dtTemp=(DataTable)Session["DataReminder"];
                        if (dtTemp.Rows.Count > 0)
                        {
                            _qsDataReminderID = (int.Parse(dtTemp.Rows[dtTemp.Rows.Count - 1]["DataReminderID"].ToString()) - 1).ToString();
                        }
                    }
                }

                _iDataReminderID = int.Parse(_qsDataReminderID);
            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }

        string strTitle = "Reminder Detail";
        


        // checking permission


        switch (_strActionMode.ToLower())
        {
            case "add":
                strTitle = "Add Reminder";
                if (!IsPostBack)
                {
                    PopulateUsersGrid();
                }

                break;

            case "view":

                strTitle = "View Reminder";


                PopulateTheRecord();

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Reminder";
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

    protected void grdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDelete");
            Label lblUsers = (Label)e.Row.FindControl("lblUsers");
            Label lblReminderColumn = (Label)e.Row.FindControl("lblReminderColumn");

            if (DataBinder.Eval(e.Row.DataItem, "UserID") != DBNull.Value)
            {
                imgbtnDelete.CommandArgument = DataBinder.Eval(e.Row.DataItem, "UserID").ToString();
                lblUsers.Text = DataBinder.Eval(e.Row.DataItem, "UserName").ToString();
            }
            if (DataBinder.Eval(e.Row.DataItem, "ReminderColumnID") != DBNull.Value)
            {
                imgbtnDelete.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ReminderColumnID").ToString();
                lblReminderColumn.Text = DataBinder.Eval(e.Row.DataItem, "DisplayName").ToString() + " -- Current Record --";
            }

        }
    }

    protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetype")
        {
            try
            {
                lblMsg.Text = "";

                if (Session["DataReminderUser"] != null)
                {
                    DataTable dtDataReminderUser = (DataTable)Session["DataReminderUser"];
                    for (int i = 0; i < dtDataReminderUser.Rows.Count; i++)
                    {
                        if (dtDataReminderUser.Rows[i]["UserID"] != DBNull.Value)
                        {
                            if (e.CommandArgument.ToString() == dtDataReminderUser.Rows[i]["UserID"].ToString()
                                && _qsDataReminderID == dtDataReminderUser.Rows[i]["DataReminderID"].ToString())
                            {
                                dtDataReminderUser.Rows.RemoveAt(i);
                            }
                        }
                        else  if (dtDataReminderUser.Rows[i]["ReminderColumnID"] != DBNull.Value)
                        {
                            if (e.CommandArgument.ToString() == dtDataReminderUser.Rows[i]["ReminderColumnID"].ToString()
                                && _qsDataReminderID == dtDataReminderUser.Rows[i]["DataReminderID"].ToString())
                            {
                                dtDataReminderUser.Rows.RemoveAt(i);
                            }
                        }
                    }
                    dtDataReminderUser.AcceptChanges();
                    Session["DataReminderUser"] = dtDataReminderUser;
                    PopulateUsersGrid();
                }


            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Reminder User delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;

                //ScriptManager.RegisterClientScriptBlock(grdTable, typeof(Page), "msg_delete", "alert('Delete failed!');", true);
            }
        }
    }


    protected void PopulateUsersGrid()
    {
        int iTN = 0;
        DataTable dtDataReminderUser = new DataTable();
        if (int.Parse(_qsDataReminderID) > 0)
        {
            //edit
                       
              dtDataReminderUser = (DataTable)Session["DataReminderUser"];           

            
        }
        else
        {
            //Add
            
            dtDataReminderUser = (DataTable)Session["DataReminderUser"];

            if (!IsPostBack)
            {
                DataRow[] drRowsTemp = dtDataReminderUser.Select("DataReminderID='" + _qsDataReminderID + "'");
                if (drRowsTemp.Length == 0)
                {
                    dtDataReminderUser.Rows.Add(-1, int.Parse(_qsDataReminderID), (int)_ObjUser.UserID,null, _ObjUser.FirstName + " " + _ObjUser.LastName,"");
                    dtDataReminderUser.AcceptChanges();
                    Session["DataReminderUser"] = dtDataReminderUser;
                }
            }
         

        }

        DataTable dtTemp = dtDataReminderUser.Clone();

        DataRow[] drRows = dtDataReminderUser.Select("DataReminderID='" + _qsDataReminderID + "'");

        foreach (DataRow dr in drRows)
        {
            dtTemp.ImportRow(dr);
        }

        dtTemp.AcceptChanges();

        grdUsers.DataSource = dtTemp;
        grdUsers.DataBind();

        Session["DataReminderUser"] = dtDataReminderUser;

    }



    protected void DeleteAddUserUPDATE(int iDataReminderID)
    {
        //delete users
        Common.ExecuteText("DELETE FROM DataReminderUser WHERE DataReminderID=" + iDataReminderID.ToString());
        //add users
                
            DataTable dtDataReminderUser = (DataTable)Session["DataReminderUser"];

            DataRow[] drRows = dtDataReminderUser.Select("DataReminderID='" + _qsDataReminderID + "'");
          foreach (DataRow drU in drRows)
            {
                DataReminderUser newDataReminderUser = new DataReminderUser(null,
                    iDataReminderID, drU["UserID"]==DBNull.Value?null:(int?) int.Parse(drU["UserID"].ToString()));

                newDataReminderUser.ReminderColumnID = drU["ReminderColumnID"] == DBNull.Value ? null : (int?)int.Parse(drU["ReminderColumnID"].ToString());

                ReminderManager.ets_DataReminderUser_Insert(newDataReminderUser);
            }
        

    }

    protected void lnkAddUser_Click(object sender, EventArgs e)
    {
        //add time

        if (ddlUser.Text == "" )
        {
            //ddlUser.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a user.');", true);
            return;
        }

        if (ddlUser.Text == "-1" && ddlReminderColumn.Text=="")
        {
            //ddlUser.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a reminder column.');", true);
            return;
        }


        lblMsg.Text = "";

        if (Session["DataReminderUser"] != null)
        {
            DataTable dtDataReminderUser = (DataTable)Session["DataReminderUser"];

            if (ddlUser.Text != "-1")
            {
                for (int i = 0; i < dtDataReminderUser.Rows.Count; i++)
                {
                    if (ddlUser.Text == dtDataReminderUser.Rows[i]["UserID"].ToString()
                        && dtDataReminderUser.Rows[i]["DataReminderID"].ToString() == _qsDataReminderID)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This user is already added.');", true);
                        return;
                    }
                }
                dtDataReminderUser.Rows.Add(-1, int.Parse(_qsDataReminderID), int.Parse(ddlUser.Text), null,ddlUser.SelectedItem.Text,"");
            }
            else
            {
                for (int i = 0; i < dtDataReminderUser.Rows.Count; i++)
                {
                    if (ddlReminderColumn.Text == dtDataReminderUser.Rows[i]["ReminderColumnID"].ToString()
                        && dtDataReminderUser.Rows[i]["DataReminderID"].ToString() == _qsDataReminderID)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This field is already added.');", true);
                        return;
                    }
                }

                dtDataReminderUser.Rows.Add(-1, int.Parse(_qsDataReminderID), null, int.Parse(ddlReminderColumn.Text),"", ddlReminderColumn.SelectedItem.Text);
            }


            
            dtDataReminderUser.AcceptChanges();

            Session["DataReminderUser"] = dtDataReminderUser;
            PopulateUsersGrid();
        }
             




    }

    protected void PopulateReminderColumn()
    {
        int iTableID = int.Parse( Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));

        ddlReminderColumn.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE IsStandard=0 
                AND ColumnType='text' AND TextType='email' AND TableID=" + iTableID.ToString());
        ddlReminderColumn.DataBind();

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
        ddlReminderColumn.Items.Insert(0, liSelect);

    }

    protected void PopulateDatabaseField()
    {

        int iTableID = int.Parse( Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
        ddlDatabaseField.Items.Clear();

        DataTable dtColumns = Common.DataTableFromText("SELECT DisplayName FROM [Column] WHERE IsStandard=0 AND   TableID=" + iTableID.ToString() + "  ORDER BY DisplayName");
        foreach (DataRow dr in dtColumns.Rows)
        {
            ListItem aItem = new ListItem(dr["DisplayName"].ToString(), dr["DisplayName"].ToString());
            ddlDatabaseField.Items.Add(aItem);
        }


        //Work with 1 top level Parent tables.
        DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + iTableID.ToString()); //AND DetailPageType<>'not'

        if (dtPT.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPT.Rows)
            {
                DataTable dtPColumns = Common.DataTableFromText(@"SELECT distinct TableName + ':' + DisplayName AS DP FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE IsStandard=0 AND TableTableID IS NULL AND  [Column].TableID=" + dr["ParentTableID"].ToString());
                foreach (DataRow drP in dtPColumns.Rows)
                {
                    ListItem aItem = new ListItem(drP["DP"].ToString(), drP["DP"].ToString());
                    ddlDatabaseField.Items.Add(aItem);
                }
            }
        }


    }

    protected void PopulateUserDropDown()
    {

        ddlUser.Items.Clear();

        //ddlUser.DataSource = SecurityManager.ets_User_ByAccount(int.Parse(Session["AccountID"].ToString()));

        ddlUser.DataSource = Common.DataTableFromText(@"SELECT    ([User].FirstName + ' ' + [User].LastName ) As UserName, [User].Userid
	                    FROM [User] INNER JOIN UserRole ON [User].UserID=[UserRole].UserID WHERE [User].IsActive=1 and
	                    [UserRole].AccountID=" + Session["AccountID"].ToString() + @" AND [User].UserID NOT IN (SELECT     DISTINCT DataReminderUser.UserID
                    FROM         [User] INNER JOIN
                      DataReminderUser ON [User].UserID = DataReminderUser.UserID
                      WHERE DataReminderID=" + _qsDataReminderID + @" )
	                    ORDER BY [User].FirstName");


        ddlUser.DataBind();

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Select User-", "");
        ddlUser.Items.Insert(0, liSelect);
        System.Web.UI.WebControls.ListItem liCR = new System.Web.UI.WebControls.ListItem("-- Current Record --", "-1");
        ddlUser.Items.Insert(1, liCR);
    }




    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<DataReminder> listDataReminder = SystemData.DataReminder_Select(_iDataReminderID, "", "", "", null, null, "DataReminderID", "ASC", null, null, ref iTemp);

            DataReminder theDataReminder = ReminderManager.ets_DataReminder_Detail((int)_iDataReminderID);

            if (theDataReminder != null)
            {
                txtDays.Text = theDataReminder.NumberOfDays.ToString();
                txtReminderHeader.Text = theDataReminder.ReminderHeader;
                edtContent.Text = theDataReminder.ReminderContent;
            }
            else
            {
                DataTable dtDataReminder = (DataTable)Session["DataReminder"];
                DataRow[] drRows = dtDataReminder.Select("DataReminderID='" + _iDataReminderID.ToString() + "'");

                foreach (DataRow dr in drRows)
                {
                    txtDays.Text = dr["NumberOfDays"].ToString();
                    txtReminderHeader.Text = dr["ReminderHeader"].ToString();
                    edtContent.Text = dr["ReminderContent"].ToString();

                    break;
                }
                
                   

                

            }


            if (_strActionMode != "add" && int.Parse(_qsDataReminderID)>0)
            {
                int iTN = 0;
                DataTable dtDataReminderUser = ReminderManager.ets_DataReminderUser_Select(int.Parse(_qsDataReminderID), null, "", "", null, null, ref iTN);
                Session["DataReminderUser"] = dtDataReminderUser;
            }

            PopulateUsersGrid();
            if (_strActionMode == "edit")
            {
                ViewState["theDataReminder"] = theDataReminder;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/DataReminderDetail.aspx?ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&DataReminderID=" + Cryptography.Encrypt(theDataReminder.DataReminderID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Reminder Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }


    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtDays.Enabled = p_bEnable;
        txtReminderHeader.Enabled = p_bEnable;
        edtContent.Enabled = p_bEnable;
        ddlUser.Visible = p_bEnable;
        grdUsers.Enabled = p_bEnable;
        lnkAddUser.Visible = p_bEnable;
        ddlReminderColumn.Enabled = false;
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


                        if (_iColumnID == -1)
                        {
                            DataTable dtDataReminder = (DataTable)Session["DataReminder"];

                            dtDataReminder.Rows.Add(int.Parse(_qsDataReminderID), _iColumnID,
                                int.Parse(txtDays.Text), txtReminderHeader.Text, edtContent.Text);

                            dtDataReminder.AcceptChanges();

                            Session["DataReminder"] = dtDataReminder;

                        }
                        else
                        {

                            DataReminder newDataReminder = new DataReminder(null, _iColumnID,
                               int.Parse(txtDays.Text), txtReminderHeader.Text, edtContent.Text);

                            int iDataReminderID = ReminderManager.ets_DataReminder_Insert(newDataReminder);

                            DeleteAddUserUPDATE(iDataReminderID);
                        }


                        break;

                    case "view":


                        break;

                    case "edit":

                        if (_iColumnID == -1)
                        {
                            DataTable dtDataReminder = (DataTable)Session["DataReminder"];

                            for (int i = 0; i < dtDataReminder.Rows.Count; i++)
                            {
                                if (dtDataReminder.Rows[i]["DataReminderID"].ToString() == _qsDataReminderID)
                                {
                                    dtDataReminder.Rows[i]["NumberOfDays"] = int.Parse(txtDays.Text);
                                    dtDataReminder.Rows[i]["ReminderHeader"]=txtReminderHeader.Text;
                                    dtDataReminder.Rows[i]["ReminderContent"] = edtContent.Text;
                                }
                            }

                            dtDataReminder.AcceptChanges();

                            Session["DataReminder"] = dtDataReminder;
                         
                        }
                        else
                        {
                            DataReminder editDataReminder = (DataReminder)ViewState["theDataReminder"];

                            editDataReminder.NumberOfDays = int.Parse(txtDays.Text);
                            editDataReminder.ReminderHeader = txtReminderHeader.Text;
                            editDataReminder.ReminderContent = edtContent.Text;

                            ReminderManager.ets_DataReminder_Update(editDataReminder);
                            //SystemData.DataReminder_Update(editDataReminder);
                            DeleteAddUserUPDATE((int)editDataReminder.DataReminderID);
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
            ErrorLog theErrorLog = new ErrorLog(null, "Reminder Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            if (ex.Message.IndexOf("FK_DataReminder_Column") > -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please add the Column first and then try again.');", true);
            }

        }



    }

}
