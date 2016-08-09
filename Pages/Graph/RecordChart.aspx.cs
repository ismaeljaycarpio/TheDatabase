using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChartDirector;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Net.Mail;

public partial class Pages_Graph_RecordChart : SecurePage
{
   

    protected void Page_Load(object sender, EventArgs e)
    {
        gcTest.AccountID = int.Parse(Session["AccountID"].ToString());
        gcTest.STs = Session["STs"].ToString();
        gcTest.ParentPage = "main";
        gcTest.ShowUseReportDates = false;
        gcTest.ShowLegend = false;
        gcTest.ShowHideWidth = false;
        gcTest.ShowHideDate = false;
        gcTest.ShowDisplay3d = false;


        //if (!IsPostBack)
        //{
            string strBackURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"] + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"];

            if (Request.QueryString["fromhome"] != null)
            {
                strBackURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Default.aspx";
            }

            gcTest.BackURL = strBackURL;

        //}
       
        //gcTest.Mode = "edit";

        gcTest.Mode = "add";
        string strSID = Cryptography.Decrypt(Request.QueryString["TableID"]);

        if (strSID.IndexOf("-") > -1)
        {
            gcTest.Mode = "edit";
            gcTest.GraphOptionID = int.Parse(strSID.Replace("-",""));
        }
        else
        {
            gcTest.OneTableID = int.Parse(strSID);
        }

        if (!IsPostBack)
        {          

            if (Request.QueryString["SearchCriteriaID2"] != null)
            {
                //PopulateSearchCriteria2(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteriaID2"].ToString())));
            }
                     

        }
      
    }



    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }



}



