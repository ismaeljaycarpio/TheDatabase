using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_UserControl_ColorPicker : System.Web.UI.UserControl
{
    public event EventHandler ColorUpdated;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strJS = @" $(function () {
                    $('#hlColorSelect').fancybox({
                        scrolling: 'no',
                        type: 'iframe',
                        'transitionIn': 'elastic',
                        'transitionOut': 'none',
                        width: 300,
                        height: 400,
                        titleShow: false
                    });
                });";

        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JSforAjax", strJS, true);

        if (IsPostBack)
        {
            if (ViewState["ColorHEX"] != null)
                ColorHEX = ViewState["ColorHEX"].ToString();
        }
        if (ColorHEX != "")
        {
            hfColorValue.Value = ColorHEX;
            //divpreviewtxtfinal.InnerHtml = ColorHEX;
            divpreviewtxtfinal.Style.Add("background-color", ColorHEX);
            hlColorSelect.NavigateUrl = "~/Pages/UserControl/ColorPicker.aspx?color=" + ColorHEX.Replace("#", "");
            if ((ColorUpdated != null) && !hlColorSelect.NavigateUrl.Contains("PostbackOnSet"))
                hlColorSelect.NavigateUrl += "&PostbackOnSet=1";
        }
        else
        {
            if ((ColorUpdated != null) && !hlColorSelect.NavigateUrl.Contains("PostbackOnSet"))
                hlColorSelect.NavigateUrl += "?PostbackOnSet=1";
        }
    }

    public string ColorHEX
    {
        get
        {
            return hfColorValue.Value;
        }
        set
        {
            hfColorValue.Value = value;
        }
    }

    protected void btnPostback_Click(object sender, EventArgs e)
    {
        if (ColorUpdated != null)
        {
            ColorUpdated(this, EventArgs.Empty);
        }
    }
}