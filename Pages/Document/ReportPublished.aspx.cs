using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Document_ReportPublished :SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer!=null)
        hlBack.NavigateUrl = Request.UrlReferrer.AbsoluteUri;
        
        if (Request.QueryString["ReportID"] != null)
        {
            //hlReportAccount.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/DocReports/Document.aspx?AccountID=" + Session["AccountID"].ToString();
            //hlReportAccount.Text = hlReportAccount.NavigateUrl;

            hlReportOne.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/DocReports/Report.aspx?ReportID=" + Request.QueryString["ReportID"].ToString();
            hlReportOne.Text = hlReportOne.NavigateUrl;

            hlProperties.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + "-1" + "&TableID=" + "-1" + "&SSearchCriteriaID=" + "-1" + "&DocumentID=" + Request.QueryString["ReportID"].ToString() + "&popup=yes";
        }

    }
}