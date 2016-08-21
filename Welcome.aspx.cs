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
        Title = "Welcome to TheDatabase";

        if (!IsPostBack)
        {

            if (Request.QueryString["notable"] != null)
            {
                trContinue.Visible = true;
                hlContinue.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableOption.aspx?FirstTime=yes&MenuID=" + Cryptography.Encrypt("-1") + "&SearchCriteria=" + Cryptography.Encrypt("-1");
                Content theContent = SystemData.Content_Details_ByKey("WelcomeTheDatabaseNoTable", null);
                if (theContent != null)
                {

                    lblWelcomeTips.Text = theContent.ContentP;
                }
            }
            else
            {
                Content theContent = SystemData.Content_Details_ByKey("WelcomeTheDatabase", null);
                if (theContent != null)
                {

                    lblWelcomeTips.Text = theContent.ContentP;
                }
            }

        }

    }
}