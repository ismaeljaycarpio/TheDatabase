using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Pages_SystemData_ContentDetail : SecurePage
{
    string _strActionMode = "view";
    int? _iContentID;
    string _qsMode = "";
    User _ObjUser;
    bool _bGlobalUser = false;
    int _iAccountID;

    protected void PopulateContentType()
    {
        ddlContentType.Items.Clear();
        ddlContentType.DataSource = Common.DataTableFromText(@"SELECT * FROM ContentType");
        ddlContentType.DataBind();
        ListItem liSelect = new ListItem("--Please Select--", "");
        ddlContentType.Items.Insert(0, liSelect);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //int iTemp = 0;
        _ObjUser = (User)Session["User"];

        if(_ObjUser==null)
        {
            Session.Clear();
            base.Response.Redirect("http://" + base.Request.Url.Authority + base.Request.ApplicationPath + "/Login.aspx?ReturnURL=" + Server.UrlEncode(base.Request.RawUrl), false);
            return;
        }


        string strHelpJS = @" $(function () {
            $('#hlHelpCommon').fancybox({
                scrolling: 'auto',
                type: 'iframe',
                'transitionIn': 'elastic',
                'transitionOut': 'none',
                width: 600,
                height: 350,
                titleShow: false
            });
        });";


        ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);

        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
        { Response.Redirect("~/Default.aspx", false); }

        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            _bGlobalUser = true;
        }
        else
        {
            _bGlobalUser = false;
            if (_ObjUser.Email.ToLower() == SystemData.SystemOption_ValueByKey_Account("DemoAccountCreator",null,null).ToLower())
            {
                _bGlobalUser = true;
            }
        }

        if (Request.QueryString["AccountID"] != null)
        {
            _iAccountID = int.Parse(Cryptography.Decrypt(Request.QueryString["AccountID"].ToString()));
        }
        else
        {

            _iAccountID = int.Parse(Session["AccountID"].ToString());
        }


        if (!IsPostBack)
        {

            PopulateContentType();

            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.IndexOf("SystemData/Content.aspx") < 0 && Request.UrlReferrer.AbsoluteUri.IndexOf("/Login.aspx") < 0)
            {
                hlBack.NavigateUrl = Request.UrlReferrer.AbsoluteUri;
            }
            else
            {
            
                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/Content.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }
                else
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/Content.aspx";
                }
                //}
            }

            if (Request.QueryString["fixedurl"] != null)
            {
                hlBack.NavigateUrl = Request.QueryString["fixedurl"].ToString();
            }

            if (!_bGlobalUser)
            {
                trContentKey.Visible = false;
                trOnlyGlobalUser.Visible = false;
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


                if (Request.QueryString["ContentID"] != null)
                {

                    _iContentID = int.Parse(Cryptography.Decrypt(Request.QueryString["ContentID"]));

                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }



        edtContent.AssetManager = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";

        // checking permission


        string strTitle = "Content detail";
        switch (_strActionMode.ToLower())
        {
            case "add":

                strTitle = "Add Content";

                break;

            case "view":

                strTitle = "View Content" + " - " + txtContentKey.Text;


                PopulateTheRecord();

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Content";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                    strTitle = "Edit Content" + " - " + txtContentKey.Text;
                }
                break;


            default:
                //?

                break;
        }


        Title = strTitle;
        lblTitle.Text = strTitle;

    }


    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;

            Content theContent = SystemData.Content_Details((int)_iContentID);


            txtContentKey.Text = theContent.ContentKey;
            txtHeading.Text = theContent.Heading;
            if (theContent.ContentTypeID != null)
                ddlContentType.SelectedValue = theContent.ContentTypeID.ToString();

            txtStoredProcedure.Text = theContent.StoredProcedure;
            edtContent.Text = theContent.ContentP;


            if (theContent.StoredProcedure.Trim().Length > 0)
            {
                trDatabaseField.Visible = true;            

                DataTable theSPTable = SystemData.Run_ContentSP(theContent.StoredProcedure, "");
                ddlDatabaseField.DataSource = Common.GetColumnsFromTable(theSPTable);              
                ddlDatabaseField.DataBind();
               
                if (theContent.ContentKey.IndexOf("SMS") > -1)
                {
                    lnlAddDataBaseField.Visible = false;
                    lnlAddDataBaseFieldText.Visible = true;

                }

            }


            if (theContent.ContentKey.IndexOf("SMS") > -1)
            {
                edtContent.Visible = false;
                txtContent.Visible = true;
                txtContent.Text = theContent.ContentP;
            }


            if (_strActionMode == "edit")
            {
                ViewState["theContent"] = theContent;
                //chkAllAccount.Visible = false;
                if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                {
                    divDelete.Visible = true;

                    if (theContent.ForAllAccount != null && theContent.ForAllAccount == true)
                    {
                        divDelete.Visible = false;                     
                        chkAllAccount.Checked = true;
                    }

                    if (theContent.AccountID == null && theContent.ForAllAccount != null && theContent.ForAllAccount == false)
                    {
                        chkOnlyGlobal.Checked = true;
                    }

                }

            }
            else if (_strActionMode == "view")
            {
                chkAllAccount.Visible = false;
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ContentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&ContentID=" + Cryptography.Encrypt(theContent.ContentID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Content Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }


    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            SystemData.Content_Delete((int)_iContentID);
            Response.Redirect(hlBack.NavigateUrl, false);

        }
        catch (Exception ex)
        {
            if (ex is SqlException)
            {
                lblMsg.Text = "Delete failed! Please try again.";
            }
            else
            {

                lblMsg.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alart Messagee.", "alert('" + lblMsg.Text + "');", true);
                lblMsg.Text = "";
            }
        }


    }

    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtContentKey.Enabled = p_bEnable;
        txtHeading.Enabled = p_bEnable;
        ddlContentType.Enabled = p_bEnable;
        txtStoredProcedure.Enabled = p_bEnable;
        edtContent.Enabled = p_bEnable;


    }
    protected void lnlAddDataBaseField_Click(object sender, EventArgs e)
    {
        edtContent.Text = String.Concat(edtContent.Text, " [", ddlDatabaseField.SelectedItem.Value, "]");
    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }



    protected void lnkSave_Click(object sender, EventArgs e)
    {
        
        bool bForAllAccount = false;
        if (chkAllAccount.Checked && chkOnlyGlobal.Checked)
        {
            lblMsg.Text = "A template can not be a global content.";
            return;

        }

        int? iTempAccountID = null;

        if (chkAllAccount.Checked)
        {
            iTempAccountID = null;
            bForAllAccount = true;
        }
        else
        {
            iTempAccountID = _iAccountID;
            bForAllAccount = false;
        }

        if (chkOnlyGlobal.Checked)
        {
            iTempAccountID = null;
            bForAllAccount = false;
        }


        try
        {
            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        if (!_bGlobalUser)
                        {
                            lblMsg.Text = "You can not add a new content.";
                            return;
                        }
                        Content newContent = new Content(null, txtContentKey.Text, txtHeading.Text,
                            edtContent.Text, txtStoredProcedure.Text,
                            null, null, iTempAccountID, bForAllAccount);
                        newContent.ContentTypeID = ddlContentType.SelectedValue == "" ? null : (int?)int.Parse(ddlContentType.SelectedValue);
                        SystemData.Content_Insert(newContent);

                        break;

                    case "view":


                        break;

                    case "edit":
                        Content editContent = (Content)ViewState["theContent"];

                        editContent.ContentTypeID = ddlContentType.SelectedValue == "" ? null : (int?)int.Parse(ddlContentType.SelectedValue);


                        if (_bGlobalUser && _iAccountID==int.Parse(Session["AccountID"].ToString()))
                        {
                            //global
                            editContent.ContentKey = txtContentKey.Text;
                            editContent.Heading = txtHeading.Text;

                            editContent.ContentP = edtContent.Text;

                            if (txtContent.Visible)
                            {
                                editContent.ContentP = txtContent.Text;
                            }

                            editContent.AccountID = iTempAccountID;
                            editContent.ForAllAccount = bForAllAccount;

                            editContent.StoredProcedure = txtStoredProcedure.Text;
                            SystemData.Content_Update(editContent);
                          

                        }
                        else
                        {
                            //admin user
                            if (editContent.AccountID == null && Request.QueryString["allowadmin"]==null)
                            {
                                //add this content

                                Content newCopyContent = new Content(null, txtContentKey.Text, txtHeading.Text,
                                   edtContent.Text, txtStoredProcedure.Text,
                                   null, null, _iAccountID, false);
                                newCopyContent.ContentTypeID = ddlContentType.SelectedValue == "" ? null : (int?)int.Parse(ddlContentType.SelectedValue);
                                SystemData.Content_Insert(newCopyContent);
                            }
                            else
                            {

                                editContent.ContentKey = txtContentKey.Text;
                                editContent.Heading = txtHeading.Text;
                                editContent.ContentP = edtContent.Text;

                                if (txtContent.Visible)
                                {
                                    editContent.ContentP = txtContent.Text;
                                }
                                SystemData.Content_Update(editContent);
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
            Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            if (ex.Message.IndexOf("IX_Content_And_AccountID") > -1)
            {
                lblMsg.Text = "Content Key already exists!";
            }
            else if (ex.Message.IndexOf("string was not in a correct") > -1)
            {
                lblMsg.Text = "Content Key already exists!";
            }
            else
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Content Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;
            }
        }



    }
    protected void lnkBack_Click(object sender, EventArgs e)
    {
     
        if (Request.QueryString["search"] != null)
        {
            Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/Content.aspx" + "?search=" + Request.QueryString["search"].ToString(), false);
        }
        else
        {
            Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/Content.aspx", false);
        }
       
    }



}
