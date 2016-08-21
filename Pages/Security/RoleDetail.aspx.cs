using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Security_RoleDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iRoleID;
    string _qsMode = "";
    string _qsRoleID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;

        if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
        { Response.Redirect("~/Default.aspx", false); }
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

                if (Request.QueryString["roleid"] != null)
                {
                    _qsRoleID = Cryptography.Decrypt(Request.QueryString["roleid"]);
                    _iRoleID = int.Parse(_qsRoleID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
        Title = "Role - " + _strActionMode;
        lblTitle.Text = "Role - " + _strActionMode;


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
            //List<Role> listRole = SecurityManager.Role_Select(_iRoleID, "", "", "", null, null, "RoleID", "ASC", null, null, ref iTemp);

            Role theRole = SecurityManager.Role_Details((int)_iRoleID);

            txtRole.Text = theRole.RoleName;
            txtRoleTye.Text = theRole.RoleType;
            txtRoleNotes.Text = theRole.RoleNotes;
            if (_strActionMode == "edit")
            {
                ViewState["theRole"] = theRole;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/RoleDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&roleid=" + Cryptography.Encrypt(theRole.RoleID.ToString());
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
        txtRole.Enabled = p_bEnable;
        txtRoleTye.Enabled = p_bEnable;
        txtRoleNotes.Enabled = p_bEnable;       

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


                        Role newRole = new Role(null, txtRole.Text, txtRoleTye.Text, txtRoleNotes.Text, DateTime.Now, DateTime.Now);
                        SecurityManager.Role_Insert(newRole);
                        break;

                    case "view":


                        break;

                    case "edit":
                        Role editRole = (Role)ViewState["theRole"];

                        editRole.RoleName = txtRole.Text;
                        editRole.RoleType = txtRoleTye.Text;
                        editRole.RoleNotes = txtRoleNotes.Text;

                        SecurityManager.Role_Update(editRole);

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
            Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Role.aspx", false);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Role Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
       

    }
    //protected void cmdBack_Click(object sender, ImageClickEventArgs e)
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Role.aspx", false);
    }
}
