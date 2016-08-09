using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Help_Help : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["contentkey"] != null)
        {

            Content theContent = SystemData.Content_Details_ByKey(Request.QueryString["contentkey"].ToString(),null);

            if (theContent != null)
            {
                lblHelpContent.Text = theContent.ContentP;
                if (theContent.Heading.Trim() != "")
                {
                    lblHeading.Text = theContent.Heading;
                }
                else
                {
                    lblHeading.Text = "Help";
                }
            }

        }

    }
}