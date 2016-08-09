using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Graph_GraphOptionDetail : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        gcTest.AccountID = int.Parse(Session["AccountID"].ToString());
        gcTest.STs = Session["STs"].ToString();

        if (Request.QueryString["GraphOptionID"] != null)
        {
            gcTest.GraphOptionID = int.Parse(Cryptography.Decrypt(Request.QueryString["GraphOptionID"].ToString()));

            GraphOption theGraphOption = GraphManager.ets_GraphOption_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["GraphOptionID"].ToString())));
            if (theGraphOption != null)
            {
                if (theGraphOption.Height != null)
                    gcTest.ChartHeight = (int)theGraphOption.Height;

                if (theGraphOption.Width != null)
                    gcTest.ChartWidth = (int)theGraphOption.Width;


            }

        }

        gcTest.Mode = Cryptography.Decrypt(Request.QueryString["Mode"].ToString());
        gcTest.ParentPage = "list";
        gcTest.BackURL = "~/Pages/Graph/GraphOptions.aspx";


        //if (!IsPostBack)
        //{
            
        //    if (Request.QueryString["emailsent"] != null)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Email has been sent successfully.');", true);
        //    }
        //}
       
       
    }
}