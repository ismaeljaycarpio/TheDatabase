using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Page_AccountCreated : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "ETS - Account Created";

        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
        }

        string strContentKey = "";
        if (Request.QueryString["Contentkey"] == null)
        {
            Response.Redirect("~/Default.aspx", false);
            return;
        }

        if (Request.QueryString["Email"] != null)
        {
            lnkBack.Text = "<strong> Continue</strong>";
            hfEmail.Value = Cryptography.Decrypt(Request.QueryString["Email"].ToString());
        }


        try
        {
            strContentKey = Cryptography.DecryptStatic(Request.QueryString["Contentkey"].ToString());
        }
        catch (Exception ex)
        {
            // dbgContentCommon.ContentKey = Request.QueryString["Contentkey"].ToString();
        }

        if (strContentKey == "")
        {
            strContentKey = Request.QueryString["Contentkey"].ToString();
        }



        Content theContent = SystemData.Content_Details_ByKey(strContentKey, null);

        if (theContent != null)
        {
            lblHeading.Text = theContent.Heading;
            lblContentCommon.Text = theContent.ContentP;
            if (theContent.Heading != "")
            {
                Title = theContent.Heading;
              
            }
        }




    }

    protected void lnkBack_Click(object sender, EventArgs e)
    {

        if (Request.QueryString["type"] != null)
        {

            if (Request.QueryString["type"].ToString() == "login")
            {

                if (Session["LoginAccount"] == null)
                {
                    Response.Redirect("~/Login.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString(), false);
                }
                else
                {
                    Response.Redirect("~/Login.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&" + Session["LoginAccount"].ToString(), false);
                }

            }
        }
        else
        {

            if (Request.QueryString["Email"] != null)
            {
                if (Session["LoginAccount"] == null)
                {
                    Response.Redirect("~/Login.aspx?Email=" + Request.QueryString["Email"].ToString(), false);
                }
                else
                {
                    Response.Redirect("~/Login.aspx?Email=" + Request.QueryString["Email"].ToString() + "&" + Session["LoginAccount"].ToString(), false);
                }
            }
            else
            {
                object refUrl = ViewState["RefUrl"];
                if (refUrl != null)
                    Response.Redirect((string)refUrl);
            }
        }
    }

}
