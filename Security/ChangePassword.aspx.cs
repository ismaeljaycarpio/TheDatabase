using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Data.Linq;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

public partial class Security_ChangePassword :SecurePage
{
    //LFDataContext srcLF = new LFDataContext();
    User _ObjUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Change Password";
         _ObjUser = (User)Session["User"];
        if (_ObjUser != null && _ObjUser.UserID > 0)
        {
             lblUserName.Text = _ObjUser.FirstName + " " + _ObjUser.LastName;
        }
        else
        {
            if (Session["LoginAccount"] == null)
            {
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                Response.Redirect("~/Login.aspx?" + Session["LoginAccount"].ToString(), false);
            }
            return;
        }

        if (!IsPostBack)
        {
            if(Request.UrlReferrer!=null)
            {
                hlBack.NavigateUrl = Request.UrlReferrer.AbsoluteUri;
            }

            OldPassword.Text = "";
            Password.Text = "";
            ConformPassword.Text = "";
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

    }


    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        //Trap empty
        lblMeg.Text = "";
        if (Session["DemoEmail"] != null)
        {
            if (_ObjUser.Email.ToLower() == Session["DemoEmail"].ToString().ToLower())
            {
                lblMeg.Text = Common.DemoReadyOnlyMsg;
                return;

            }
        }


        if (Password.Text.Trim().Length<6)
        {
            lblMeg.Text = "Password Minimum length is 6.";
            Password.Focus();
            return;
        }

        if (Password.Text.Trim() != ConformPassword.Text.Trim())
        {
            lblMeg.Text = "New & Confirm password should be same!";
            return;
        }

        if (Password.Text.ToLower().IndexOf(_ObjUser.FirstName.ToLower()) > 0)
        {
            lblMeg.Text = "Password should not have first name!";
            Password.Focus();
            return;
        }
        if (Password.Text.ToLower().IndexOf(_ObjUser.LastName.ToLower()) > 0)
        {
            lblMeg.Text = "Password should not have last name!";
            Password.Focus();
            return;
        }
        if (Password.Text.ToLower().IndexOf(_ObjUser.Email.ToLower().Substring(0, _ObjUser.Email.IndexOf("@"))) > 0)
        {
            lblMeg.Text = "Password should not have email address!";
            Password.Focus();
            return;
        }


        if (Password.Text.Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PassEmptyErr", "alert('" + "Please enter your password." + "');", true);
            
        }
        else
        {
           
                User ObjUser = (User)Session["User"];
                int result = 0;

                result=ChangePassword(OldPassword.Text.Trim(), Password.Text.Trim(), (int) ObjUser.UserID);

                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Password Changed", "alert('Password is changed successfully!');", true);

                    Response.Redirect(hlBack.NavigateUrl, false);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Old Password", "alert('" + "Invalid Old Password." + "');", true);
                }
            
        }
    }


    protected  int ChangePassword(string strOldPassword, string strNewPassword, int iUserID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ChangePassword", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@Result", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@OldPassword", strOldPassword));
                command.Parameters.Add(new SqlParameter("@Password", strNewPassword));
                command.Parameters.Add(new SqlParameter("@userId", iUserID));


                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return -1;
            }
        }
    }



    //protected void lnkCancel_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/Default.aspx", false);
    //}

   
}
