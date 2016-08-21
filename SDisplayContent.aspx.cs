using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Page_SDisplayContent : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Content";

        if (!IsPostBack)
        {

            if (Session["User"] == null && Session["AccountID"] == null)
            {
                Response.Redirect(Request.RawUrl.Replace("SDisplayContent.aspx", "DisplayContent.aspx"));
                return;
            }


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

        //if (Request.QueryString["Email"] != null)
        //{
        //    lnkBack.Text = "Continue";
        //}

       // dbgContentCommon.AssetManager = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";

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
            //lblHeading.Text = theContent.Heading;
            //lblContentCommon.Text = theContent.ContentP;
            if (theContent.Heading != "")
            {
                Title = theContent.Heading;
              
            }

            dbgContentCommon.ContentKey = theContent.ContentKey;

            if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            {
                dbgContentCommon.ShowInlineContentEditor = true;
            }
            else
            {
                dbgContentCommon.ShowInlineContentEditor = false;
            }


        }




    }

  

}
