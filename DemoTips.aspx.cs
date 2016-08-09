using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DemoTips : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Demo Tips";

        if (!IsPostBack)
        {
            Content theContent = SystemData.Content_Details_ByKey("DemoTips",null);
            if (theContent != null)
            {

                lblDemoTips.Text = theContent.ContentP;
            }

        }

    }
}