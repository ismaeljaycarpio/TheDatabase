using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Help_FancyConfirm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if(!IsPostBack)
        {
            if (Request.QueryString["message"] != null)
            {
                lblMessage.Text = Cryptography.Decrypt(Request.QueryString["message"].ToString());
            }
            if (Request.QueryString["okbutton"] != null)
            {
                string strOkButton = Cryptography.Decrypt(Request.QueryString["okbutton"].ToString());
                string strCloseAndRefresh = @"function CloseAndRefresh() {
                                 window.parent.document.getElementById('" + strOkButton + @"').click();
                                 parent.$.fancybox.close();             
                                      }";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "strCloseAndRefresh", strCloseAndRefresh, true);

            }

            if (Request.QueryString["nobutton"] != null)
            {
                string strNoButton = Cryptography.Decrypt(Request.QueryString["nobutton"].ToString());
                string strCloseAndRefreshNo = @"function CloseAndRefreshNo() {
                                 window.parent.document.getElementById('" + strNoButton + @"').click();
                                 parent.$.fancybox.close();             
                                      }";


                lnkNo.OnClientClick = "CloseAndRefreshNo();return false;";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "strCloseAndRefreshNo", strCloseAndRefreshNo, true);

            }
            else
            {
                lnkNo.OnClientClick = "parent.$.fancybox.close();return false;";
            }

        }

    }
}