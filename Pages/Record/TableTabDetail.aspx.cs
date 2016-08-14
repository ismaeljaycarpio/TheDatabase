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

        if (Request.QueryString["TableTabID"] != null)
        {

            _qsTableTabID = Cryptography.Decrypt(Request.QueryString["TableTabID"]);

            _iTableTabID = int.Parse(_qsTableTabID);
        }
        if (!IsPostBack)
        {
            Session["dtShowWhen"] = null;
            string strShowWhenID = "";
            if(_qsTableTabID=="")
            {
                hlShowWhen.NavigateUrl = "~/Pages/Record/ShowHide.aspx?TableID="+_qsTableID+"&Context=tabletab";
            }
            else
            {
                hlShowWhen.NavigateUrl = "~/Pages/Record/ShowHide.aspx?TableID=" + _qsTableID + "&Context=tabletab&tabletabid=" + _qsTableTabID;
                strShowWhenID = Common.GetValueFromSQL("SELECT TOP 1 ShowWhenID FROM ShowWhen WHERE tabletabid=" + _qsTableTabID);
            } 

            if (strShowWhenID != "")
            {
                chkShowWhen.Checked = true;
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


                        int iTableTabID=RecordManager.dbg_TableTab_Insert(newTableTab);

                        if (chkShowWhen.Checked && Session["dtShowWhen"] != null)
                        {
                            //insert new show when
                            DataTable dtShowWhen = (DataTable)Session["dtShowWhen"];
                            int iDO = 1;
                            foreach (DataRow drSW in dtShowWhen.Rows)
                            {
                                if (iDO == 1)
                                {
                                    if (drSW["HideColumnID"].ToString() == "" || drSW["HideColumnValue"].ToString() == "")
                                    {
                                        continue;
                                    }
                                    ShowWhen theShowWhen1 = new ShowWhen();
                                    theShowWhen1.TableTabID = iTableTabID;
                                    theShowWhen1.Context = "tabletab";
                                    theShowWhen1.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                                    theShowWhen1.HideColumnValue = drSW["HideColumnValue"].ToString();
                                    theShowWhen1.HideOperator = drSW["HideOperator"].ToString();
                                    theShowWhen1.DisplayOrder = 1;
                                    theShowWhen1.JoinOperator = "";
                                    theShowWhen1.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen1);

                                    iDO = iDO + 1;
                                    continue;
                                }
                                else
                                {
                                    if (drSW["HideColumnID"].ToString() == "" || drSW["HideColumnValue"].ToString() == "" || drSW["JoinOperator"].ToString() == "")
                                    {
                                        continue;
                                    }


                                    ShowWhen theShowWhenJoin = new ShowWhen();
                                    theShowWhenJoin.TableTabID = iTableTabID;
                                    theShowWhenJoin.Context = "tabletab";
                                    theShowWhenJoin.HideColumnID = null;
                                    theShowWhenJoin.HideColumnValue = "";
                                    theShowWhenJoin.HideOperator = "";
                                    theShowWhenJoin.DisplayOrder = iDO;
                                    theShowWhenJoin.JoinOperator = drSW["JoinOperator"].ToString();

                                    theShowWhenJoin.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhenJoin);
                                    iDO = iDO + 1;

                                    ShowWhen theShowWhen = new ShowWhen();
                                    theShowWhen.TableTabID = iTableTabID;
                                    theShowWhen.Context = "tabletab";
                                    theShowWhen.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                                    theShowWhen.HideColumnValue = drSW["HideColumnValue"].ToString();
                                    theShowWhen.HideOperator = drSW["HideOperator"].ToString();
                                    theShowWhen.DisplayOrder = iDO;
                                    theShowWhen.JoinOperator = "";

                                    theShowWhen.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen);
                                    iDO = iDO + 1;

                                }
                            }

                        }

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
