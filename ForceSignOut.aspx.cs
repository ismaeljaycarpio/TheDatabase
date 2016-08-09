using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class ForceSignOut : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["User"] = null;
        Session.Abandon();

        FormsAuthentication.SignOut();
        HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
        oUseInfor.Expires = DateTime.Now.AddDays(-3d);
        Response.Cookies.Add(oUseInfor);

    }
}