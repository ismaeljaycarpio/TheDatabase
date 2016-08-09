using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Graph_DefaultGraphDef : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int iTN = 0;

            DataTable dt = GraphManager.ets_GraphDefinition_Select(null, null, null,
                true, null,
                null, null, null,
                true,
                null, null, null, null, 
                null, null, null, null, ref iTN);
            ddlGraphDefinition.DataSource = dt;
            ddlGraphDefinition.DataBind();

            string s = SystemData.SystemOption_ValueByKey_Account("DefaultGraphDefinitionID",null,null);
            if (!String.IsNullOrEmpty(s))
            {
                ddlGraphDefinition.SelectedValue = s;
            }
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //int iTN = 0;
        //
        //List<SystemOption> options = 
        //    SystemData.SystemOption_Select(null, "DefaultGraphDefinitionID", null, null, null, null, null, null, null, null, ref iTN);

        SystemOption theSystemOption = SystemData.SystemOption_Detail_Key_Account("DefaultGraphDefinitionID", null, null);

        if (theSystemOption == null)
        {
            SystemData.SystemOption_Insert(new SystemOption(null, "DefaultGraphDefinitionID",
                ddlGraphDefinition.SelectedValue, "Default Graph Definition ID",
                null, null));
        }
        else
        {
            theSystemOption.OptionValue = ddlGraphDefinition.SelectedValue;
            SystemData.SystemOption_Update(theSystemOption);
        }
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "close_script", "OnClose();", true);
    }
}