using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Help_FancyContent : System.Web.UI.Page
{


    protected void lnkSave_Click(object sender, EventArgs e)
    {

        if (Request.QueryString["update"] != null)
        {
            Common.ExecuteText(string.Format(Cryptography.Decrypt(Request.QueryString["update"].ToString()),edtContent.Text.Replace("'","''")));
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if(!IsPostBack)
        {
            edtContent.AssetManager = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";

            if (Request.QueryString["title"] != null)
            {
                lblTopTitle.Text = Cryptography.Decrypt(Request.QueryString["title"].ToString());
            }
            if (Request.QueryString["select"] != null)
            {
                edtContent.Text = Common.GetValueFromSQL(Cryptography.Decrypt(Request.QueryString["select"].ToString()));
            }

            string strSaveJS = "";

           
            if (Request.QueryString["okjs"] != null)
            {
                strSaveJS = strSaveJS + Cryptography.Decrypt(Request.QueryString["okjs"].ToString());
            }
            if (Request.QueryString["okbutton"] != null)
            {
                string strOkButton = Cryptography.Decrypt(Request.QueryString["okbutton"].ToString());
                strSaveJS = strSaveJS + "window.parent.document.getElementById('" + strOkButton + @"').click();";

            }


            string strCloseAndRefresh = @"function CloseAndRefresh() {
                                 " + strSaveJS + @"
                                 parent.$.fancybox.close();             
                                      }";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "strCloseAndRefresh", strCloseAndRefresh, true);

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