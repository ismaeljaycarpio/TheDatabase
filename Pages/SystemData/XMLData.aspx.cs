using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_SystemData_XMLData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if(txtSearchCriteriaID.Text!="")
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail_1(int.Parse(Cryptography.Decrypt(txtSearchCriteriaID.Text.Replace("%20"," "))));

            if (theSearchCriteria != null)
            {
                XMLData theXMLData = new XMLData(null, theSearchCriteria.SearchText, theSearchCriteria.SearchCriteriaID);
                SystemData.dbg_XMLData_Insert(theXMLData);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "message_alert", "alert('Copy successful.');", true);

            }


        }
    }
}