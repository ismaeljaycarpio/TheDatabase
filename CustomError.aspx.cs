using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "ETS - Error"; 

        Content theContent = SystemData.Content_Details_ByKey("CustomErrorMessage",null);

        if (theContent != null)
        {
            lblHeading.Text = theContent.Heading;
            if (theContent.Heading != "")
            {
                Title = theContent.Heading;
            }
            lblContent.Text = Server.HtmlDecode(theContent.ContentP);
        }


    }
}