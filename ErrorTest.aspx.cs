using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int i = 0;
        int j = 15 / i;
    }
}