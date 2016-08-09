using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["DemoEmail"] = "demo@carbonmonitoring.com.au";
        Response.Redirect("~/Login.aspx?Email=demo@carbonmonitoring.com.au&Password=Secret123&RememberMe=No");
    }
}