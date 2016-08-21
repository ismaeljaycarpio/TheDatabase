using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Marketing2 : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {

       
        if (!IsPostBack)
        {

            if (Session["AccountID"] != null)
            {
                Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
                if (theAccount.ExpiryDate != null)
                {
                    if (theAccount.ExpiryDate.Value.AddDays(0) < DateTime.Today)
                    {
                        //divRenew.Visible = true;

                    }
                }

            }

            //hlRenewNow.NavigateUrl = SystemData.SystemOption_ValueByKey("ContactUsRenewal");
           
            //if (Session["DoNotAllow"] != null)
            //{
            //    divRenew.Visible = true;
            //}

            if (Request.QueryString["AccountID"] != null)
            {
                Account theAccount = SecurityManager.Account_Details(int.Parse(Request.QueryString["AccountID"].ToString()));

                if (theAccount != null)
                {
                    divCopyright.InnerText = theAccount.CopyRightInfo;
                    divPhone.Visible = false;
                    divFooterTop.Visible = false;
                    navigation.Visible = false;

                    if (theAccount.Logo != null)
                    {
                        if ((bool)theAccount.UseDefaultLogo == false)
                            imgLogo.ImageUrl = "~/SSPhoto.ashx?AccountID=" + theAccount.AccountID.ToString() + "&type=o";
                    }

                }

            }
            else
            {
                if (Request.RawUrl.ToLower().IndexOf("/login.aspx") > -1)
                {
                    divPhone.Visible = false;
                    divFooterTop.Visible = false;
                    navigation.Visible = false;
                    //imgLogo.Visible = false;
                    //h1Logo.Visible = true;

                    //divCopyright.InnerText = "Copyright © 2012 - All rights reserved.";
                }

                string strRoot = HttpContext.Current.Request.Url.Scheme +"://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;

                SubDomainInfo theSubDomain = SecurityManager.SubDomainInfo_Details(strRoot);
                if (theSubDomain != null)
                {
                    imgLogo.ImageUrl = strRoot + "/Images/" + theSubDomain.LogoFileName;

                }




            }
            

            CheckSelectedMenu();
            string strRefSite = "";
            if (Request.QueryString["Ref"] != null)
            {
                strRefSite = Request.QueryString["Ref"].ToString();
            }
            SystemData.VisitorInsert((User)Session["User"], Request.UserHostAddress, Request.UserAgent, Request.AppRelativeCurrentExecutionFilePath, strRefSite);

        }
    }

    protected void CheckSelectedMenu()
    {
       

        string path = Request.AppRelativeCurrentExecutionFilePath;


        

       
        if (Request.RawUrl.IndexOf("Environmental-Monitoring-Management-Database-System.aspx") > -1)
        {
            hlHome.CssClass = "current";
        }
        if (Request.RawUrl.IndexOf("Features-Include-Unlimited-Record-Types-Alerts-Warnings-Report-Generator.aspx") > -1)
        {
            hlFeatures.CssClass = "current";
        }

        if (Request.RawUrl.IndexOf("Case-Studies-Environment-Tracking-System.aspx") > -1)
        {
            hlCaseStudy.CssClass = "current";
        }

        if (Request.RawUrl.IndexOf("Pricing-Environmental-Monitoring-System.aspx") > -1)
        {
            hlPricing.CssClass = "current";
        }
        if (Request.RawUrl.IndexOf("Contact-Us-About-Envionmental-Monitoring-System.aspx") > -1)
        {
            hlContactUs.CssClass = "current";
        }

        if (Request.RawUrl.IndexOf("SignIn.aspx") > -1)
        {
            hlSignUp.CssClass = "current";
        }


    }
}
