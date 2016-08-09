using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test_ColorPicker : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        hfPathForcolor.Value="http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/UserControl/";
        
        
        if (Request.QueryString["color"] != null)
        {
            string strColor = "#" + Request.QueryString["color"].ToString();
            divpreviewtxt.InnerHtml = strColor;
            colorhex.Value = strColor;
            divpreview.Style.Add("background-color", strColor);
        }

        if (Request.QueryString["PostbackOnSet"] != null)
            hfPostbackOnSet.Value = Request.QueryString["PostbackOnSet"];
        else
            hfPostbackOnSet.Value = "0";
    }
}