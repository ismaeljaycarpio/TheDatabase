using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_DocGen_SectionSelect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["Position"] != null)
        {
            hfTextPath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/TextSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString() + "&Position=" + Request.QueryString["Position"].ToString();
            hfHTMLPath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/HTMLSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString() + "&Position=" + Request.QueryString["Position"].ToString();
            hfTablePath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/TableSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString() + "&Position=" + Request.QueryString["Position"].ToString();
            hfPhotoPath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/ImageSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString() + "&Position=" + Request.QueryString["Position"].ToString();
            hfChartPath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/ChartSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString() + "&Position=" + Request.QueryString["Position"].ToString();

        }
        else
        {
            hfTextPath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/TextSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString();
            hfHTMLPath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/HTMLSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString() ;
            hfTablePath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/TableSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString();
            hfPhotoPath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/ImageSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString() ;
            hfChartPath.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/ChartSection.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString() ;
        }


//        string strFancy = @"$(function () {
//            $("".popuplink"").fancybox({
//                scrolling: 'auto',
//                type: 'iframe',
//                width: 800,
//                height: 700,
//                titleShow: false
//            });
//        });";

//        ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);

    }
}