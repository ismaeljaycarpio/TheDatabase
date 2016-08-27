using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
public partial class Test_DT : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnConvert_Click(object sender, EventArgs e)
    {
        lblResult.Text = "";
        bool bOK = false;
        try
        {
            DateTime? csDateTime = Common.GetDateTimeFromString(txtDTString.Text, ddlFormat.SelectedValue);
            if(csDateTime!=null)
            {
                bOK = true;
                lblResult.Text = csDateTime.Value.ToLongDateString();
                lblResult.Text = lblResult.Text + "<br/>";
                lblResult.Text = lblResult.Text + csDateTime.Value.ToLongTimeString();
            }
            else
            {
                 lblResult.Text="Invalid or we could not convert this format yet.";
            }

        }
        catch(Exception ex)
        {            
            lblResult.Text ="Error:"+ ex.Message + "---" + ex.StackTrace;
        }

        try
        {
            //collecting format
            if(bOK==false)
            {
                lblResult.ForeColor = System.Drawing.Color.Red;
                string sError="";
                DBGurus.SendEmail("E", null, null, "Date Time Format", txtDTString.Text, "", "r_mohsin@yahoo.com", "", "", null, null, out sError);
            }
            else
            {
                lblResult.ForeColor = System.Drawing.Color.Green;
            }
        }
        catch
        {
            //
        }

    }
}