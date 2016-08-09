using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class paypal_thanks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginAccount"] == null)
        {
            hlBack.NavigateUrl = "~/Login.aspx";
        }
        else
        {
            hlBack.NavigateUrl = "~/Login.aspx?" + Session["LoginAccount"].ToString();
        }

        if (!IsPostBack)
        {

            Session.Clear();
            FormsAuthentication.SignOut();

            Content theContent = SystemData.Content_Details_ByKey("Payment_thanks", null);
            if (theContent != null)
            {
                lblContentCommon.Text = theContent.ContentP;
            }
        }
    }
}
