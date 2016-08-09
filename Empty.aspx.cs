using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Empty : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnPreInit(EventArgs e)
    {
        bool bChangeToPoup = false;
        if (Request.RawUrl.IndexOf("EachRecordTable.aspx") > -1
            || Request.QueryString["DocumentSectionID"] != null)
        {
            bChangeToPoup = true;
        }

        if (Request.UrlReferrer!=null)
        {
            if (Request.UrlReferrer.AbsolutePath.IndexOf("Default.aspx") > -1
                || Request.UrlReferrer.AbsolutePath=="/")
            {
                bChangeToPoup = true;
            }

        }

        if(bChangeToPoup)
        {
            Page.MasterPageFile = "~/Home/Popup.master";
        }

    }


}