using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Pages_Help_CopyTableHelp : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Copy " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/")+1),"Table","Table") + " Help";
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

             Content theContent = SystemData.Content_Details_ByKey("CopyTableHelp", null);
             if (theContent != null)
             {
                 lblContentCommon.Text = theContent.ContentP;
             }


         }

    }
}
