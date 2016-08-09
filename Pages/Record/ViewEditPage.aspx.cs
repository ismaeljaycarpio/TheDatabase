using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Record_ViewEditPage : SecurePage
{
    Account _theAccount;
    protected override void OnPreInit(EventArgs e)
    {

        if (Request.QueryString["noajax"] != null)
        {
            _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
            if (_theAccount.MasterPage != "")
            {
                Page.MasterPageFile = _theAccount.MasterPage;
            }
            else
            {
                Page.MasterPageFile = "~/Home/Home.master";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        string strViewItemfancy = @"
                    $(function () {
                            $('.popuplinkVT').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 1000,
                                height: 400,
                                titleShow: false
                            });
                        });

                ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "strViewItemfancy", strViewItemfancy, true);

        if(!IsPostBack)
        {
            if (Request.RawUrl.IndexOf("TableDetail.aspx") > -1)
            {

            }
            else if (Request.QueryString["noajax"] != null)
            {

            }
            else
            {
                if (Request.QueryString["tabindex"] != null)
                {
                    Session["viewtabindex"] = Request.QueryString["tabindex"].ToString();
                }
                else
                {
                    Session["viewtabindex"] = null;
                }
            }

        }
       

        

    }
}