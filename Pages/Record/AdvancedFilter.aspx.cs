using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Record_AdvancedFilter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                
                //}
            }
            catch (Exception ex)
            {
                //f.Text = "Sorry that bible reference cannot be found.";
               
            }
        }
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {

    }

    void close()
    {

//        string script = @"
//
// 
//
//fireEvent(window.parent.document.getElementById('fancybox-close'), 'click');
//
// 
//
//function fireEvent(obj, evt) {
//
//var fireOnThis = obj;
//
//if (document.createEvent) {
//
//var evObj = document.createEvent('MouseEvents');
//
//evObj.initEvent(evt, true, false);
//
//fireOnThis.dispatchEvent(evObj);
//
//} else if (document.createEventObject) {
//
//fireOnThis.fireEvent('on' + evt);
//
//}
//
//}
//
// 
//
//";
        string script = "parent.$.fancybox.close() ;";
        Page.ClientScript.RegisterStartupScript(

            typeof(Page),

            "close",

            string.Format("<script type='text/javascript'>{0}</script>", script));

    }
    protected void lbCancel_Click(object sender, EventArgs e)
    {
        close();
    }
}