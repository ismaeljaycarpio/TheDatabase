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
public partial class Pages_Security_RoleGroupDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iRoleID;
    string _qsMode = "";
    string _qsRoleID = "";
    //int? _iParentRoleID = null;

    User _objUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        _objUser = (User)Session["User"];

               


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


                if (Request.QueryString["RoleID"] != null)
                {

                    _qsRoleID = Cryptography.Decrypt( Request.QueryString["RoleID"].ToString());

                    _iRoleID = int.Parse(_qsRoleID);
                    hfRoleID.Value = _iRoleID.ToString();
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }



        // checking permission

        string strTitle = "Role Detail";
        switch (_strActionMode.ToLower())
        {
            case "add":

                strTitle = "Add Role";

                break;

            case "view":


                strTitle = "View Role";

                PopulateTheRecord();

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Role";

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
        //    CheckRoleSecurity();

        Title = strTitle;
        lblTitle.Text = strTitle;

    }



    

    protected void PopulateTheRecord()
    {
        try
        {

            //Role theRole = SecurityManager.dbg_Role_Detail((int)_iRoleID,null,null);
            Role theRole = SecurityManager.Role_Details((int)_iRoleID);

            txtRoleName.Text = theRole.RoleName;           

            if (_strActionMode == "edit")
            {
                ViewState["theRole"] = theRole;
            }
           
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Role Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }


    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtRoleName.Enabled = p_bEnable;



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
                        
                        
                        dtTemp = Common.DataTableFromText("SELECT RoleID FROM Role WHERE AccountID=" + Session["AccountID"].ToString() + " AND [Role]='" + txtRoleName.Text.Replace("'", "''") + "'");

                        

                        if (dtTemp != null)
                        {
                            if (dtTemp.Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This Role name already exists, please try another Role name.');", true);
                                return;
                            }
                        }


                        Role newRole = new Role(null,txtRoleName.Text,"","",null,null);

                        newRole.OwnerUserID = _objUser.UserID;
                        newRole.AccountID = int.Parse(Session["AccountID"].ToString());
                        newRole.IsActive = true;
                        newRole.IsSystemRole = false;

                       int iID= SecurityManager.Role_Insert(newRole);

                       hfRoleID.Value = iID.ToString();

                       Session["tdbmsgpb"] = "New Role "+txtRoleName.Text+" has been added.";

                        break;

                    case "view":


                        break;

                    case "edit":

                        Role editRole = (Role)ViewState["theRole"];

                        dtTemp = Common.DataTableFromText("SELECT RoleID FROM Role WHERE AccountID=" + Session["AccountID"].ToString() + " AND [Role]='" + txtRoleName.Text.Replace("'", "''") + "' AND RoleID<>" + editRole.RoleID.ToString());

                        

                        if (dtTemp != null)
                        {
                            if (dtTemp.Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This Role name already exists, please try another Role name.');", true);
                                return;
                            }
                        }


                        

                        editRole.RoleName = txtRoleName.Text;

                        SecurityManager.Role_Update(editRole);

                        Session["tdbmsgpb"] = "Role Updated.";
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
            ErrorLog theErrorLog = new ErrorLog(null, "Role Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }

   
}
