using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Public : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //BindMenu();
        //CheckSelectedMenu();
        if (!Page.IsPostBack)
        {
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

        if (Request.RawUrl.IndexOf("SignIn.aspx") > -1)
        {
            hlLogIn.CssClass = "current";
        }


    }
    //protected void CheckSelectedMenu()
    //{
    //    string path = Request.AppRelativeCurrentExecutionFilePath;
    //    bool bGotSelected = false;
    //    foreach (MenuItem item in menuMarketing.Items)
    //    {

    //        if (item.NavigateUrl.IndexOf(path) > -1)
    //        {
    //            item.Selectable = true;
    //            item.Selected = true;
    //            bGotSelected = true;
    //            break;
    //        }


    //    }
    //    if (bGotSelected == false)
    //    {
    //        if (Request.RawUrl.ToLower().IndexOf("menu=help") > -1)
    //        {
    //            foreach (MenuItem item in menuMarketing.Items)
    //            {
    //                if (item.Value == "Help")
    //                {
    //                    item.Selectable = true;
    //                    item.Selected = true;
    //                    bGotSelected = true;
    //                    break;
    //                }
    //            }

    //        }
    //    }
    //}
    //protected void BindMenu()
    //{
    //    menuMarketing.Items.Clear();
    //    string strAppPath = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath;



    //    //MenuItem miDefault = new MenuItem();
    //    //miDefault.Text = "Home";
    //    //miDefault.NavigateUrl = "~/Pages/Marketing/Default.aspx";
    //    //menuMarketing.Items.Add(miDefault);

    //    //MenuItem miTour = new MenuItem();
    //    //miTour.Text = "Tour";
    //    //miTour.NavigateUrl = "~/Pages/Marketing/Tour.aspx";
    //    //menuMarketing.Items.Add(miTour);

    //    //MenuItem miHelp = new MenuItem();
    //    //miHelp.Text = "Help";
    //    //miHelp.Value = "Help";
    //    //miHelp.NavigateUrl = "~/Pages/Marketing/Help.aspx";
    //    //menuMarketing.Items.Add(miHelp);

    //    //MenuItem miPricing = new MenuItem();
    //    //miPricing.Text = "Pricing";
    //    //miPricing.NavigateUrl = "~/Pages/Marketing/Pricing.aspx";
    //    //menuMarketing.Items.Add(miPricing);

    //    MenuItem miSignUp = new MenuItem();
    //    miSignUp.Text = "Start";
    //    miSignUp.NavigateUrl = "~/Login.aspx";
    //    menuMarketing.Items.Add(miSignUp);

    //    //MenuItem miContact = new MenuItem();
    //    //miContact.Text = "Contact";
    //    //miContact.NavigateUrl = "~/Pages/Company/ContactUs.aspx";
    //    //menuMarketing.Items.Add(miContact);



    //}
}
