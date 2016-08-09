using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Security_EmailValidation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)

    {

        Title = "ETS - Email Validation";
        try
        {
            //divLogIn.Visible = false;
            lblMessage.Attributes.Add("style", "color:red");
            lblMessage.Text = "Sorry! Your registration email validation request seems not valid, please contact with us.";
            if (Request.QueryString["key"] != null && Request.QueryString["key2"] != null)
            {
                int iAccountID = int.Parse(Cryptography.DecryptStatic(Request.QueryString["key"].ToString()));
                string strDateUpdated = Cryptography.DecryptStatic(Request.QueryString["key2"].ToString());
                //get current dateupdated
                Account theAccount = SecurityManager.Account_Details(iAccountID);

                if (strDateUpdated == theAccount.DateUpdated.ToString())
                {
                    //evertthing ok, so lets active it
                    theAccount.IsActive = true;
                    SecurityManager.Account_Update(theAccount);

                    DataTable dtUsers = Common.DataTableFromText("SELECT * FROM [USER] WHERE IsAccountHolder=1 AND  AccountID=" + theAccount.AccountID.ToString());
                    //divLogIn.Visible = true;
                    if (dtUsers.Rows.Count > 0)
                    {
                        User theUser = SecurityManager.User_Details(int.Parse(dtUsers.Rows[0]["UserID"].ToString()));
                        if (theUser != null)
                        {
                            if (Session["LoginAccount"] == null)
                            {
                                hpLogIn.NavigateUrl = "~/Login.aspx?Email=" + Cryptography.Encrypt(theUser.Email.Trim());
                            }
                            else
                            {
                                hpLogIn.NavigateUrl = "~/Login.aspx?Email=" + Cryptography.Encrypt(theUser.Email.Trim()) + "&" + Session["LoginAccount"].ToString();
                                Title =theAccount.AccountName + " Email Validation";
                            }
                        }

                    }

                    lblMessage.Attributes.Add("style", "color:blue");
                    lblMessage.Text = "Congratulations! You have successfully validated your email address. Your account is now active, you can now log in.";
                }

            }
        }
        catch
        {
            //divLogIn.Visible = false;
            lblMessage.Text = "Sorry! Your registration email validation request seems not valid, please contact with us.";
        }

    }
}
