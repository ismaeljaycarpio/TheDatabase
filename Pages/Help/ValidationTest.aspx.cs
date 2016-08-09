using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Help_ValidationTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "ETS- Date Validation";
    }

    protected void btnShowResult_Click(object sender, EventArgs e)
    {
        lblResult.Text = "";
        string strError = "";
        if (txtData.Text.Trim() == "" || txtValidation.Text.Trim() == "")
        {
            lblResult.Text = "Please enter validation Formula and Data";
            return;
        }
        if (UploadManager.IsDataValid(txtData.Text, txtValidation.Text, ref strError))
        {
            lblResult.Text = "Valid";
        }
        else
        {
            if (strError == "")
            {
                lblResult.Text = "Invalid";
            }
            else
            {
                lblResult.Text = "ERROR:" + strError;
                lblResult.Text = lblResult.Text + "<br/> <I>Please correct the validation Formula.</I>";
            }
        }

    }
}
