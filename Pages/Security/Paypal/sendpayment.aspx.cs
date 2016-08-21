using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class sendpayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

       
    }
    public string GetReturnURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Paypal/thanks.aspx";
    }
    public string Getcancel_returnURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Paypal/cancel.aspx";
    }
    public string Getnotify_urlURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Paypal/paypal.aspx";
    }
}
