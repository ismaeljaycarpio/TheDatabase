using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
public partial class Pages_SystemData_NewContent: SecurePage
{

   

    

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        hfContentKey.Value = txtContentKey.Text;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Saved Action", "GetBackAndReFresh()", true);
    }

   
   
}
