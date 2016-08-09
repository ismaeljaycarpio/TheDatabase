using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Pages_Content_Common : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
         Title = "Content";
         if (!IsPostBack)
         {
             ViewState["RefUrl"] = Request.UrlReferrer.ToString();
         }
         if (Context.User.Identity.IsAuthenticated)
         {
             User ObjUser = (User)Session["User"];
             if (ObjUser == null)
             {
                 if (Session["LoginAccount"] == null)
                 {
                     Session.Clear();
                     FormsAuthentication.SignOut();
                     Response.Redirect("~/Login.aspx", false);
                 }
                 else
                 {
                     string strLoginAccount = Session["LoginAccount"].ToString();
                     Session.Clear();
                     FormsAuthentication.SignOut();
                     Response.Redirect("~/Login.aspx?" + strLoginAccount, false);
                 }
                 return;
             }
             string strContentKey = "";

             if (Request.QueryString["Contentkey"]==null)
             {
                 Response.Redirect("~/Default.aspx", false);
                 return;
             }

             strContentKey = Cryptography.Decrypt(Request.QueryString["Contentkey"].ToString());


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

    }


    protected void lnkBack_Click(object sender, EventArgs e)
    {
        object refUrl = ViewState["RefUrl"];
        if (refUrl != null)
            Response.Redirect((string)refUrl);

    }

}
