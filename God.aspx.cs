using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class God : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        User objUser = (User)Session["User"];
        if (Common.HaveAccess(Session["roletype"].ToString(), "1") || objUser.Email == "mohsinrazuan@gmail.com"
            || Request.Url.Authority.IndexOf("localhost") == 0 || objUser.Email=="r_mohsin@yahoo.com"
            || objUser.Email == "jon@dbgurus.com.au" || objUser.Email == "jarrod@dbgurus.com.au")
        {
        }
        else
        {
            Response.Redirect("~/Default.aspx", false);
            return;
        }
         
        
    }
    protected void btnGetInt_Click(object sender, EventArgs e)
    {
       try
       {
           lblDecrypt.Text = "";
            lblDecrypt.Text=  Cryptography.Decrypt(txtEncrypted.Text.Trim());
       }
        catch(Exception ex)
       {
            lblDecrypt.Text=ex.Source + "---Stack --->" + ex.StackTrace;
        }
        
    }
}