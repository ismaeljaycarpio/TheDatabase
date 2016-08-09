using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SignUpDone :SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        hfRoot.Value = "http://" + Request.Url.Authority + Request.ApplicationPath;
    }
}