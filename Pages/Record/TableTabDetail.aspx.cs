using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Record_TableTabDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iTableTabID;
    string _qsMode = "";
    string _qsTableTabID = "";
    string _qsTableID = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        //int iTemp = 0;

       

        //int iTableTabID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableTabID"].ToString()));

        _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
           
            
           
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


                if (Request.QueryString["TableTabID"] != null)
                {

                    _qsTableTabID = Cryptography.Decrypt(Request.QueryString["TableTabID"]);

                    _iTableTabID = int.Parse(_qsTableTabID);
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


                strTitle = "View Page";
                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":
                strTitle = "Edit Page";
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

   

    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<TableTab> listTableTab = SystemData.TableTab_Select(_iTableTabID, "", "", "", null, null, "TableTabID", "ASC", null, null, ref iTemp);

            TableTab theTableTab = RecordManager.ets_TableTab_Detail((int)_iTableTabID);

           
            
            txtTabName.Text = theTableTab.TabName;

            if (_strActionMode == "edit")
            {
                ViewState["theTableTab"] = theTableTab;
            }
          
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Tab Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
       
        txtTabName.Enabled = p_bEnable;
       

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
                        TableTab WHERE TableID=" + _qsTableID);

                        int iDisplayOrder = 0;
                        if (strDisplayOrder != "")
                            iDisplayOrder = int.Parse(strDisplayOrder) + 1;

                        TableTab newTableTab = new TableTab(null, int.Parse(_qsTableID), txtTabName.Text,
                            iDisplayOrder);
                       

                        RecordManager.dbg_TableTab_Insert(newTableTab);

                        break;

                    case "view":


                        break;

                    case "edit":
                        TableTab editTableTab = (TableTab)ViewState["theTableTab"];

                        editTableTab.TabName = txtTabName.Text;

                        RecordManager.dbg_TableTab_Update(editTableTab);


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
