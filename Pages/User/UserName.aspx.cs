using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_User_UserName : SecurePage
{


    User _ObjUser;
    protected void Page_Load(object sender, EventArgs e)
    {

        //_ObjUser=SecurityManager.User_Details(int.Parse(Request.QueryString["userid"].ToString()));
        _ObjUser = (User)Session["User"];

        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            txtEmail.Text = Request.QueryString["Email"].ToString().Trim();
            txtUsername.Text = Request.QueryString["UserName"].ToString().Trim();
            if (txtUsername.Text.Trim() == "")
                txtUsername.Text = txtEmail.Text;

            PopulateHelpSection();
            hfMode.Value=Request.QueryString["mode"].ToString().Trim();
            hfUserID.Value = Request.QueryString["UserID"].ToString().Trim(); ;

            if (hfMode.Value == "view")
            {
                txtEmail.Enabled = false;
                txtUsername.Enabled = false;
                divSave.Visible = false;
            }
        }

//        string strJS = @"function GetBackValue() 
//        {
//            window.parent.document.getElementById('txtEmail').value = document.getElementById('txtEmail').value;
//             window.parent.document.getElementById('hfUserName').value = document.getElementById('txtUsername').value;
//             parent.$.fancybox.close();
//        }";

//        ScriptManager.RegisterStartupScript(this, this.GetType(), "PageJS", strJS, true);
                
    }


    protected void PopulateHelpSection()
    {
        Content theHelpContent = SystemData.Content_Details_ByKey("EmailUserNameHelp",null);
        dbgHelpContent.Text = theHelpContent.ContentP;

        UserContent theUserContent = SystemData.ets_UserContent_Details((int)_ObjUser.UserID, (int)theHelpContent.ContentID);
        if (theUserContent == null)
        {
            //do nothing
            chkDoNotShowHelp.Checked = true;
        }
        else
        {
            if ((bool)theUserContent.IsDefaultShow)
            {
                lbHelp.Visible = false;
                lbHide.Visible = true;
                dbgHelpContent.Visible = true;
                chkDoNotShowHelp.Visible = true;
                chkDoNotShowHelp.Checked = true;
            }
            else
            {
                lbHide.Visible = false;
                lbHelp.Visible = true;
                dbgHelpContent.Visible = false;
                chkDoNotShowHelp.Visible = false;
                chkDoNotShowHelp.Checked = false;

            }
        }

    }
    protected void lbHide_Click(object sender, EventArgs e)
    {
        lbHide.Visible = false;
        lbHelp.Visible = true;
        dbgHelpContent.Visible = false;
        chkDoNotShowHelp.Visible = false;

    }
    protected void lbHelp_Click(object sender, EventArgs e)
    {
        lbHelp.Visible = false;
        lbHide.Visible = true;
        dbgHelpContent.Visible = true;
        chkDoNotShowHelp.Visible = true;
    }


    protected void chkDoNotShowHelp_CheckedChanged(object sender, EventArgs e)
    {
        Content theHelpContent = SystemData.Content_Details_ByKey("EmailUserNameHelp",null);
        dbgHelpContent.Text = theHelpContent.ContentP;
        UserContent theUserContent = SystemData.ets_UserContent_Details((int)_ObjUser.UserID, (int)theHelpContent.ContentID);

        if (!chkDoNotShowHelp.Checked)
        {
            if (theUserContent == null)
            {
                //insert
                UserContent newUserContent = new UserContent(null, (int)_ObjUser.UserID, (int)theHelpContent.ContentID, false);
                SystemData.ets_UserContent_Insert(newUserContent);
            }
            else
            {
                //update
                theUserContent.IsDefaultShow = false;
                SystemData.ets_UserContent_Update(theUserContent);
            }

            lbHide.Visible = false;
            lbHelp.Visible = true;
            dbgHelpContent.Visible = false;
            chkDoNotShowHelp.Visible = false;
        }
        else
        {
            if (theUserContent == null)
            {
                //insert
                UserContent newUserContent = new UserContent(null, (int)_ObjUser.UserID, (int)theHelpContent.ContentID, true);
            }
            else
            {
                //update
                theUserContent.IsDefaultShow = true;
                SystemData.ets_UserContent_Update(theUserContent);
            }

            lbHelp.Visible = false;
            lbHide.Visible = true;
            dbgHelpContent.Visible = true;
            chkDoNotShowHelp.Visible = true;

        }

    }




    protected void lnkSave_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        DataTable dtTemp = Common.DataTableFromText("SELECT * FROM [User] WHERE UserID<>" + hfUserID.Value + " AND UserName='" + txtUsername.Text + "'");
        if (dtTemp.Rows.Count > 0)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "UserNamePopUp", "Sorry this Username(" + txtUsername.Text + ") is already used by another user. Please try another Username.", true);
            lblMsg.Text =  "That Username(" + txtUsername.Text + ") is already being used on another account. Please try another Username.";
            txtUsername.Focus();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UserNamePopUp", "GetBackValue();", true);
        }

    }
}
