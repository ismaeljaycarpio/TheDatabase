using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Document_DocumentTypeDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iDocumentTypeID;
    string _qsMode = "";
    string _qsDocumentTypeID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;

        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentType.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {
                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentType.aspx";
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


                if (Request.QueryString["DocumentTypeID"] != null)
                {

                    _qsDocumentTypeID = Cryptography.Decrypt(Request.QueryString["DocumentTypeID"]);

                    _iDocumentTypeID = int.Parse(_qsDocumentTypeID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
       


        // checking permission

        string strTitle = "Document Type Detail";
        switch (_strActionMode.ToLower())
        {
            case "add":

                strTitle = "Add Document Type";

                break;

            case "view":


                strTitle = "View Document Type";
                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Document Type";

                if (!IsPostBack)
                {
                    PopulateTheRecord();
                    divDelete.Visible = true;
                }
                break;


            default:
                //?

                break;
        }


        Title = strTitle;
        lblTitle.Text = strTitle;

    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        

        try
        {

            DocumentType theDocumentType = DocumentManager.ets_DocumentType_Detail((int)_iDocumentTypeID);

            if (theDocumentType.DocumentTypeName == "Audit Report")
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('We can not delete Audit Report Document Type.');", true);
                return;

            }
            if (theDocumentType.DocumentTypeName == "Custom Reports")
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('We can not delete Custom Reports Document Type.');", true);
                return;

            }

            DocumentManager.ets_DocumentType_Delete((int)_iDocumentTypeID);
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
            }
        }


    }
     
    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0; Custom Reports
            //List<DocumentType> listSystemOption = SystemData.SystemOption_Select(_iDocumentTypeID, "", "", "", null, null, "DocumentTypeID", "ASC", null, null, ref iTemp);

            DocumentType theDocumentType = DocumentManager.ets_DocumentType_Detail((int)_iDocumentTypeID);

            txtDocumentType.Text = theDocumentType.DocumentTypeName;

            if (theDocumentType.DocumentTypeName == "Audit Report")
            {
                txtDocumentType.Enabled = false;               

            }

            if (theDocumentType.DocumentTypeName == "Custom Reports")
            {
                txtDocumentType.Enabled = false;

            }

            if (_strActionMode == "edit")
            {
                ViewState["theDocumentType"] = theDocumentType;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                if (Request.QueryString["SearchCriteria"] != null)
                {
                    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentTypeDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&DocumentTypeID=" + Cryptography.Encrypt(theDocumentType.DocumentTypeID.ToString()) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                }
                else
                {
                    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentTypeDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&DocumentTypeID=" + Cryptography.Encrypt(theDocumentType.DocumentTypeID.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Document Type Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtDocumentType.Enabled = p_bEnable;
       
            

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

                        DocumentType newDocumentType = new DocumentType(null, int.Parse(Session["AccountID"].ToString()),
                            txtDocumentType.Text,DateTime.Now, DateTime.Now);
                        DocumentManager.ets_DocumentType_Insert(newDocumentType);

                        break;

                    case "view":


                        break;

                    case "edit":
                        DocumentType editDocumentType = (DocumentType)ViewState["theDocumentType"];

                        editDocumentType.DocumentTypeName = txtDocumentType.Text;                       

                        DocumentManager.ets_DocumentType_Update(editDocumentType);


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
            Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentType.aspx", false);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Document Type Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

     

    }

  
    //protected void cmdBack_Click(object sender, ImageClickEventArgs e)
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentType.aspx", false);
    }
}
