using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
public partial class Pages_Document_FolderDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iFolderID;
    string _qsMode = "";
    string _qsFolderID = "";
    int? _iParentFolderID = null;

    User _objUser;
    UserRole _theUserRole;
    protected void Page_Load(object sender, EventArgs e)
    {
        _objUser = (User)Session["User"];
        _theUserRole = (UserRole)Session["UserRole"];

        //if (!IsPostBack)
        //{
        //    if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2,3,4,5,6,7"))
        //    { Response.Redirect("~/Default.aspx", false); }          

        //}

        if (Request.QueryString["ParentFolderID"] != null)
        {

            _iParentFolderID = int.Parse(Request.QueryString["ParentFolderID"].ToString());
            if (_iParentFolderID == -1)
            {
                _iParentFolderID = null;
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


                if (Request.QueryString["FolderID"] != null)
                {

                    _qsFolderID = Cryptography.Decrypt( Request.QueryString["FolderID"].ToString());

                    _iFolderID = int.Parse(_qsFolderID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }



        // checking permission

        string strTitle = "Folder Detail";
        switch (_strActionMode.ToLower())
        {
            case "add":

                strTitle = "Add Folder";

                break;

            case "view":


                strTitle = "View Folder";

                PopulateTheRecord();

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Folder";

                if (!IsPostBack)
                {
                    PopulateTheRecord();
                    //divDelete.Visible = true;
                }
                break;


            default:
                //?

                break;
        }


        //if(!IsPostBack)
        //    CheckFolderSecurity();

        Title = strTitle;
        lblTitle.Text = strTitle;

    }



    protected void CheckFolderSecurity()
    {

        string strFolderRight = "none";
        if (_theUserRole.IsDocSecurityAdvanced != null)
        {
            if ((bool)_theUserRole.IsDocSecurityAdvanced)
            {

                string strDBRight = "";

                if (_iParentFolderID == -1)
                {
                    strDBRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE UserID=" + _theUserRole.UserID.ToString() + " AND FolderID IS NULL ");

                }
                else
                {
                    strDBRight = Common.GetValueFromSQL("SELECT RightType FROM UserFolder WHERE UserID=" + _theUserRole.UserID.ToString() + " AND FolderID=" + _iParentFolderID.ToString());
                }

              

                if (strDBRight == "")
                {
                    strFolderRight = "none";
                }
                else
                {
                    strFolderRight = strDBRight;
                }
            }
            else
            {
                if (_theUserRole.DocSecurityType != "")
                    strFolderRight = _theUserRole.DocSecurityType;
            }
        }
        else
        {
            if (_theUserRole.DocSecurityType != "")
                strFolderRight = _theUserRole.DocSecurityType;
        }


        switch (strFolderRight)
        {
            case "none":
                divDetail.Visible = false;
                break;
            case "read":
                divDetail.Visible = false;

                break;
            case "upload":
                divDetail.Visible = false;
                break;
            case "full":
                divDetail.Visible = true;
                break;

            default:
                break;
        }



    }

    protected void PopulateTheRecord()
    {
        try
        {

            Folder theFolder = DocumentManager.ets_Folder_Detail((int)_iFolderID);

            txtFolderName.Text = theFolder.FolderName;           

            if (_strActionMode == "edit")
            {
                ViewState["theFolder"] = theFolder;
            }
           
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Folder Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }


    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtFolderName.Enabled = p_bEnable;



    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }



    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsUserInputOK())
            {
                DataTable dtTemp;
                switch (_strActionMode.ToLower())
                {
                    case "add":


                       

                        if (_iParentFolderID == null)
                        {
                            dtTemp = Common.DataTableFromText("SELECT FolderID FROM Folder WHERE AccountID=" + Session["AccountID"].ToString() + " AND ParentFolderID IS  NULL AND FolderName='" + txtFolderName.Text.Replace("'", "''") + "'");
                        }
                        else
                        {
                            dtTemp = Common.DataTableFromText("SELECT FolderID FROM Folder WHERE AccountID=" + Session["AccountID"].ToString() + " AND ParentFolderID="+_iParentFolderID.ToString()+" AND FolderName='" + txtFolderName.Text.Replace("'", "''") + "'");

                        }

                        if (dtTemp != null)
                        {
                            if (dtTemp.Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This folder name already exists, please try another folder name.');", true);
                                return;
                            }
                        }




                        Folder newFolder = new Folder(null, int.Parse(Session["AccountID"].ToString()),
                         _iParentFolderID,  txtFolderName.Text);
                        DocumentManager.ets_Folder_Insert(newFolder);

                        break;

                    case "view":


                        break;

                    case "edit":

                        Folder editFolder = (Folder)ViewState["theFolder"];
                        
                        if (_iParentFolderID == null)
                        {
                            dtTemp = Common.DataTableFromText("SELECT FolderID FROM Folder WHERE AccountID=" + Session["AccountID"].ToString() + " AND ParentFolderID IS  NULL AND FolderName='" + txtFolderName.Text.Replace("'", "''") + "' AND FolderID<>" + editFolder.FolderID.ToString());
                        }
                        else
                        {
                            dtTemp = Common.DataTableFromText("SELECT FolderID FROM Folder WHERE AccountID=" + Session["AccountID"].ToString() + " AND ParentFolderID=" + _iParentFolderID.ToString() + " AND FolderName='" + txtFolderName.Text.Replace("'", "''") + "' AND FolderID<>" + editFolder.FolderID.ToString());

                        }

                        if (dtTemp != null)
                        {
                            if (dtTemp.Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This folder name already exists, please try another folder name.');", true);
                                return;
                            }
                        }

                        

                        editFolder.FolderName = txtFolderName.Text;

                        DocumentManager.ets_Folder_Update(editFolder);


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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Saved Action", "GetBackAndReFresh()", true);


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Folder Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }

   
}
