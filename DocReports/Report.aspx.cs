using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.Security;

public partial class Page_Reports_Report : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
         Title = "ETS - Report";

                   
             if (Request.QueryString["ReportID"]==null)
             {
                 Response.Redirect("~/Default.aspx", false);
                 return;
             }

            

             try
             {
                 Document theDocument=DocumentManager.ets_Document_Detail(int.Parse( Cryptography.Decrypt( Request.QueryString["ReportID"].ToString())));
                 if (theDocument != null && theDocument.IsReportPublic != null && (bool)theDocument.IsReportPublic)
                 {
                     //if (theDocument.FileUniqename == "")
                     //{
                         lblContent.Text = theDocument.ReportHTML;
                         Title = theDocument.DocumentText;
                     //}
                     //else
                     //{
                     //    Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Uploads/" + theDocument.FileUniqename, false);
                     //}
                 }
                 else
                 {
                     lblContent.Text="Report not found.";
                 }
             }
             catch (Exception ex)
             {
                //
             }

                  

    }

   
}
