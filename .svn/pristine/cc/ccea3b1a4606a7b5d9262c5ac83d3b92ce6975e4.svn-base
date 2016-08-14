using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Popup : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string strRefSite = "";
            if (Request.QueryString["Ref"] != null)
            {
                strRefSite = Request.QueryString["Ref"].ToString();
            }
            SystemData.VisitorInsert((User)Session["User"], Request.UserHostAddress, Request.UserAgent, Request.AppRelativeCurrentExecutionFilePath,strRefSite);

            if (Request.RawUrl.IndexOf("DocGen/ChartSection.aspx") > -1)
            {
                Content.Attributes.Add("style", "text-align: left; padding-left:0px; padding-right:5px;min-height: 300px;");
            }
        }

    }
}
