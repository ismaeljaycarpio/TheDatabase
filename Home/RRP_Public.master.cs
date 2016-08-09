using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RRP_Public : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {

            if (Request.QueryString["AccountID"] != null)
            {
                Account theAccount = SecurityManager.Account_Details(int.Parse(Request.QueryString["AccountID"].ToString()));

                if (theAccount != null)
                {
                    

                }

            }
            else
            {
                if (Request.RawUrl.ToLower().IndexOf("/login.aspx") > -1)
                {

                }

                string strRoot = "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;

                SubDomainInfo theSubDomain = SecurityManager.SubDomainInfo_Details(strRoot);
                if (theSubDomain != null)
                {
                    //imgLogo.ImageUrl = strRoot + "/Images/" + theSubDomain.LogoFileName;

                }




            }



            string strRefSite = "";
            if (Request.QueryString["Ref"] != null)
            {
                strRefSite = Request.QueryString["Ref"].ToString();
            }
            SystemData.VisitorInsert((User)Session["User"], Request.UserHostAddress, Request.UserAgent, Request.AppRelativeCurrentExecutionFilePath, strRefSite);

        }
    }


}
