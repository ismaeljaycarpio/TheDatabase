using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Page_DisplayMarketingContent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "ETS - Content";

        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
        }

        if (Request.QueryString["Contentkey"] == null)
        {
            Response.Redirect("~/Default.aspx", false);
            return;
        }

        if (Request.QueryString["Email"] != null)
        {
            lnkBack.Text = "Continue";
        }


        try
        {
            //dbgContentCommon.ContentKey = Cryptography.DecryptStatic(Request.QueryString["Contentkey"].ToString());

            Content theContent = SystemData.Content_Details_ByKey(Cryptography.DecryptStatic(Request.QueryString["Contentkey"].ToString()),null);

            if (theContent != null)
            {
                lblHeading.Text = theContent.Heading;
                lblContent.Text = theContent.ContentP;
                if (theContent.Heading != "")
                {
                    Title = theContent.Heading;
                }
            }
            else
            {
                Content theContent1 = SystemData.Content_Details_ByKey(Request.QueryString["Contentkey"].ToString(),null);

                if (theContent1 != null)
                {
                    lblContent.Text = theContent1.ContentP;
                    lblHeading.Text = theContent1.Heading;
                    if (theContent1.Heading != "")
                    {
                        Title = theContent1.Heading;
                    }
                }

            }

        }
        catch (Exception ex)
        {
            lblContent.Text = "Content not found.";
        }

        //if (dbgContentCommon.ContentKey.Trim() == "")
        //{
        //    dbgContentCommon.ContentKey = Request.QueryString["Contentkey"].ToString();
        //}



        //Content theContent = SystemData.Content_Details_ByKey(dbgContentCommon.ContentKey.Trim());

        //if (theContent != null)
        //{
        //    lblHeading.Text = theContent.Heading;
        //    if (theContent.Heading != "")
        //    {
        //        Title = theContent.Heading;
        //    }
        //}




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
