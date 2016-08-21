using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_SystemData_SystemOptionDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iSystemOptionID;
    string _qsMode = "";
    string _qsSystemOptionID = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;

        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
            { Response.Redirect("~/Default.aspx", false); }

            PopulateAccount();
            PopulateTable();
            if (Request.QueryString["SearchCriteria"] != null)
            {

                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/SystemOption.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {

                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/SystemOption.aspx", false);//i think no need
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
                if (Request.QueryString["SystemOptionID"] != null)
                {

                    _qsSystemOptionID = Cryptography.Decrypt(Request.QueryString["SystemOptionID"]);
                    _iSystemOptionID = int.Parse(_qsSystemOptionID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
        Title = "System Option - " + _strActionMode;
        lblTitle.Text = "System Option - " + _strActionMode;


        // checking permission


        switch (_strActionMode.ToLower())
        {
            case "add":


                break;

            case "view":



                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }




    }

     
    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<SystemOption> listSystemOption = SystemData.SystemOption_Select(_iSystemOptionID, "", "", "", null, null, "SystemOptionID", "ASC", null, null, ref iTemp);

            SystemOption theSystemOption = SystemData.SystemOption_Details((int)_iSystemOptionID);

            txtOptionKey.Text = theSystemOption.OptionKey;
            txtOptionValue.Text = theSystemOption.OptionValue;
            txtOptionNotes.Text = theSystemOption.OptionNotes;

            if (theSystemOption.AccountID!=null)
            {
                if(ddlAccount.Items.FindByValue(theSystemOption.AccountID.ToString())!=null)
                {
                    ddlAccount.SelectedValue = theSystemOption.AccountID.ToString();
                    PopulateTable();
                }
            }
            if(theSystemOption.TableID!=null)
            {
                if (ddlTable.Items.FindByValue(theSystemOption.TableID.ToString()) != null)
                    ddlTable.SelectedValue = theSystemOption.TableID.ToString();
            }

            if (_strActionMode == "edit")
            {
                ViewState["theSystemOption"] = theSystemOption;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/SystemOptionDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&SystemOptionID=" + Cryptography.Encrypt(theSystemOption.SystemOptionID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "System Option Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }

    protected void PopulateAccount()
    {
        ddlAccount.Items.Clear();
        ddlAccount.DataSource = Common.DataTableFromText("SELECT AccountID,AccountName FROM Account WHERE IsActive=1 ORDER BY AccountName");
        ddlAccount.DataBind();

        ListItem liAll = new ListItem("--All--", "");
        ddlAccount.Items.Insert(0, liAll);
    }

    protected void PopulateTable()
    {
        ddlTable.Items.Clear();

        string strAccountID = "";
        if (ddlAccount.SelectedItem != null && ddlAccount.SelectedValue != "")
        {
            strAccountID = ddlAccount.SelectedValue;
        }

        if (strAccountID != "")
        {
            ddlTable.DataSource = Common.DataTableFromText("SELECT TableID,TableName FROM [Table] WHERE IsActive=1 and AccountID=" + strAccountID + " ORDER BY TableName");
            ddlTable.DataBind();
        }


        ListItem liAll = new ListItem("--All--", "");
        ddlTable.Items.Insert(0, liAll);
    }
    protected void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
    {

        PopulateTable();
    }
    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtOptionKey.Enabled = p_bEnable;
        txtOptionValue.Enabled = p_bEnable;
        txtOptionNotes.Enabled = p_bEnable;

        ddlAccount.Enabled = p_bEnable;
        ddlTable.Enabled = p_bEnable;
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

                        SystemOption newSystemOption = new SystemOption(null, txtOptionKey.Text, txtOptionValue.Text, txtOptionNotes.Text, DateTime.Now, DateTime.Now);

                        newSystemOption.AccountID = ddlAccount.SelectedValue == "" ? null : (int?)int.Parse(ddlAccount.SelectedValue);
                        newSystemOption.TableID = ddlTable.SelectedValue == "" ? null : (int?)int.Parse(ddlTable.SelectedValue);

                        SystemData.SystemOption_Insert(newSystemOption);

                        break;

                    case "view":


                        break;

                    case "edit":
                        SystemOption editSystemOption = (SystemOption)ViewState["theSystemOption"];

                        editSystemOption.OptionKey = txtOptionKey.Text;
                        editSystemOption.OptionValue = txtOptionValue.Text;
                        editSystemOption.OptionNotes = txtOptionNotes.Text;


                        editSystemOption.AccountID = ddlAccount.SelectedValue == "" ? null : (int?)int.Parse(ddlAccount.SelectedValue);
                        editSystemOption.TableID = ddlTable.SelectedValue == "" ? null : (int?)int.Parse(ddlTable.SelectedValue);


                        SystemData.SystemOption_Update(editSystemOption);


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
            ErrorLog theErrorLog = new ErrorLog(null, "System Option Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

     

    }
   
}
