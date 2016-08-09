using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Security_TerminologyDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iTerminologyID;
    string _qsMode = "";
    string _qsTerminologyID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;

        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            PopulatePageName();
            if (Request.QueryString["SearchCriteria"] != null)
            {

                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Terminology.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {

                Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Terminology.aspx", false);//i think no need
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


                if (Request.QueryString["TerminologyID"] != null)
                {

                    _qsTerminologyID = Cryptography.Decrypt(Request.QueryString["TerminologyID"]);

                    _iTerminologyID = int.Parse(_qsTerminologyID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }

        lblTitle.Text = "Terminology Detail";


        // checking permission


        switch (_strActionMode.ToLower())
        {
            case "add":
                lblTitle.Text = "Add Terminology";
                if (!IsPostBack)
                {
                    PopulateInputText();
                }
                break;

            case "view":

                lblTitle.Text = "View Terminology";

                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":
                lblTitle.Text = "Edit Terminology";
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


    }

    protected void PopulatePageName()
    {
        ddlPageName.DataSource = Common.DataTableFromText("SELECT DISTINCT PageName FROM Terminology WHERE AccountID IS NULL AND PageName IS Not NULL");

        ddlPageName.DataBind();
        ListItem liAll = new ListItem("All", "");
        ddlPageName.Items.Insert(0, liAll);
    }

    protected void PopulateInputText()
    {
        ddlInputText.Items.Clear();

        if (_strActionMode.ToLower() == "add")
        {
            if (ddlPageName.SelectedValue == "")
            {
                ddlInputText.DataSource = Common.DataTableFromText(@"SELECT DISTINCT InputText FROM Terminology WHERE AccountID IS NULL AND PageName IS  NULL
                        AND InputText NOT IN (SELECT DISTINCT InputText FROM Terminology WHERE AccountID =" + Session["AccountID"].ToString() + @" AND PageName IS  NULL )
                        ");
            }
            else
            {
                ddlInputText.DataSource = Common.DataTableFromText(@"SELECT DISTINCT InputText FROM Terminology WHERE AccountID IS NULL AND PageName ='" + ddlPageName.SelectedValue + @"'
                AND InputText NOT IN (SELECT DISTINCT InputText FROM Terminology WHERE AccountID =" + Session["AccountID"].ToString() + @" AND PageName ='" + ddlPageName.SelectedValue + @"' )
                ");

            }
        }
        else
        {
            if (ddlPageName.SelectedValue == "")
            {
                ddlInputText.DataSource = Common.DataTableFromText("SELECT DISTINCT InputText FROM Terminology WHERE AccountID IS NULL AND PageName IS  NULL");
            }
            else
            {
                ddlInputText.DataSource = Common.DataTableFromText("SELECT DISTINCT InputText FROM Terminology WHERE AccountID IS NULL AND PageName='" + ddlPageName.SelectedValue + "'");

            }
        }



        ddlInputText.DataBind();
        //ListItem liAll = new ListItem("All", "");
        //ddlInputText.Items.Insert(0, liAll);
    }

     
    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<Terminology> listTerminology = Security.Terminology_Select(_iTerminologyID, "", "", "", null, null, "TerminologyID", "ASC", null, null, ref iTemp);

            Terminology theTerminology = SecurityManager.ets_Terminology_Detail((int)_iTerminologyID);

            
            ddlPageName.Text = theTerminology.PageName;

            PopulateInputText();

            ddlInputText.Text = theTerminology.InputText;
            txtOutputText.Text = theTerminology.OutputText;

            if (_strActionMode == "edit")
            {
                ViewState["theTerminology"] = theTerminology;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/TerminologyDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TerminologyID=" + Cryptography.Encrypt(theTerminology.TerminologyID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Terminology Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        ddlPageName.Enabled = p_bEnable;
        ddlInputText.Enabled = p_bEnable;
        txtOutputText.Enabled = p_bEnable;
            

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

                        Terminology newTerminology = new Terminology(null, int.Parse(Session["AccountID"].ToString()), ddlPageName.SelectedValue,
                            ddlInputText.SelectedValue, txtOutputText.Text);
                        SecurityManager.ets_Terminology_Insert(newTerminology);

                        break;

                    case "view":


                        break;

                    case "edit":
                        Terminology editTerminology = (Terminology)ViewState["theTerminology"];

                        editTerminology.PageName = ddlPageName.SelectedValue;
                        editTerminology.InputText = ddlInputText.SelectedValue;
                        editTerminology.OutputText = txtOutputText.Text;

                        SecurityManager.ets_Terminology_Update(editTerminology);


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
            ErrorLog theErrorLog = new ErrorLog(null, "Terminology Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

     

    }

    protected void ddlPageName_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateInputText();
    }

    
}
