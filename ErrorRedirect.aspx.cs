using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorRedirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["LoginAccount"] == null)
        {
            //Server.Transfer("~/Login.aspx?ErrorLogID=" + Request.QueryString["ErrorLogID"].ToString());
            Response.Redirect("~/Login.aspx?ErrorLogID=" + Request.QueryString["ErrorLogID"].ToString(),false);
        }
        else
        {
            //Server.Transfer("~/Login.aspx?ErrorLogID=" + Request.QueryString["ErrorLogID"].ToString() + "&" + Session["LoginAccount"].ToString());

            Response.Redirect("~/Login.aspx?ErrorLogID=" + Request.QueryString["ErrorLogID"].ToString() + "&" + Session["LoginAccount"].ToString(),false);
        }
    }
}