using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Record_UploadDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iUploadID;
    string _qsMode = "";
    string _qsUploadID = "";


    protected void PopulateTerminology()
    {
        stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;

        if (!IsPostBack)
        {

            lblPopEmailFrom.Text = SystemData.SystemOption_ValueByKey_Account("PopEmailFrom", null, null);

            PopulateTableDDL();
            //PopulateLocationDDL();
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            if (Request.QueryString["SearchCriteria"] != null)
            {

                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Upload.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {

                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Upload.aspx", false);//i think no need
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


                if (Request.QueryString["UploadID"] != null)
                {

                    _qsUploadID = Cryptography.Decrypt(Request.QueryString["UploadID"]);

                    _iUploadID = int.Parse(_qsUploadID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
       
        


        // checking permission


        switch (_strActionMode.ToLower())
        {
            case "add":
                lblTitle.Text = "Add Auto Upload";

                break;

            case "view":

                lblTitle.Text = "View Auto Upload";



                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                lblTitle.Text = "Edit Auto Upload";
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

    //protected void PopulateLocationDDL()
    //{
    //    int iTN = 0;
    //    if (ddlTable.SelectedValue != "")
    //    {
    //        ddlLocation.Items.Clear();
    //        ddlLocation.DataSource = SiteManager.ets_Location_Select(null, int.Parse(ddlTable.SelectedValue), null,
    //                string.Empty, string.Empty, true, null, null, null, null,
    //                int.Parse(Session["AccountID"].ToString()),
    //                "LocationName", "ASC",
    //                null, null, ref  iTN, "");

    //        ddlLocation.DataBind();
    //        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--All--", "");
    //        ddlLocation.Items.Insert(0, liSelect);
    //    }
    //    else
    //    {
    //        ddlLocation.Items.Clear();

    //        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--All--", "");
    //        ddlLocation.Items.Insert(0, liSelect);
    //    }

    //}

     
    protected void PopulateTheRecord()
    {
        try
        {

            Upload theUpload = UploadManager.ets_Upload_Detail((int)_iUploadID);

            ddlTable.SelectedValue = theUpload.TableID.ToString();
            chkUseMapping.Checked = theUpload.UseMapping;

            //PopulateLocationDDL();

            //if (theUpload.LocationID != null)
            //    ddlLocation.SelectedValue = theUpload.LocationID.ToString();

            txtEmailFrom.Text = theUpload.EmailFrom;
            txtUploadName.Text = theUpload.UploadName;
            txtFilename.Text = theUpload.Filename;

            if (_strActionMode == "edit")
            {
                ViewState["theUpload"] = theUpload;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/UploadDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&UploadID=" + Cryptography.Encrypt(theUpload.UploadID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Upload Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtUploadName.Enabled = p_bEnable;
        txtEmailFrom.Enabled = p_bEnable;
        txtFilename.Enabled = p_bEnable;
        ddlTable.Enabled = p_bEnable;
        chkUseMapping.Enabled = p_bEnable;
        //ddlLocation.Enabled = p_bEnable;
            

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

                        Upload newUpload = new Upload(null, int.Parse(ddlTable.SelectedValue),
                            txtUploadName.Text, txtEmailFrom.Text, txtFilename.Text, chkUseMapping.Checked);

                        UploadManager.ets_Upload_Insert(newUpload);
                       

                        break;

                    case "view":


                        break;

                    case "edit":
                        Upload editUpload = (Upload)ViewState["theUpload"];

                        editUpload.EmailFrom = txtEmailFrom.Text;
                        editUpload.UploadName = txtUploadName.Text;
                        editUpload.Filename = txtFilename.Text;

                        editUpload.TableID = int.Parse(ddlTable.SelectedValue);
                        editUpload.UseMapping = chkUseMapping.Checked;
                        //if (ddlLocation.SelectedValue == "")
                        //{
                        //    editUpload.LocationID = null;
                        //}
                        //else
                        //{
                        //    editUpload.LocationID = int.Parse(ddlLocation.SelectedValue);
                        //}

                        UploadManager.ets_Upload_Update(editUpload);
                        


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

            if (ex.Message.IndexOf("UQ_EmailFrom_FileName") > -1)
            {
                lblMsg.Text = "The combination of email & file name is already used, please use another email or file name.";
            }
            else
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Upload Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;
            }
            
        }

     

    }
   
}
