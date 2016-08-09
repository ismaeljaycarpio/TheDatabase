using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GraphMsg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["type"] != null)
        {
            if (Request.QueryString["type"].ToString() == "slow")
            {
                lblMessage.Text = "The graph will be slow to respond. Are you sure you wish to proceed?";

            }
            else
            {
                lblMessage.Text = "The ETS cannot plot more than 5000 Records at a time. Please adjust the time period.";
                divOk.Visible = false;
                hlNo.Text = "<strong>Ok</strong>";
            }
        }

    }

    protected void lnkOk_Click(object sender, EventArgs e)
    {
        Session["PlotGraph"] = "ok";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "SavedAndRefresh();", true);

    }
}