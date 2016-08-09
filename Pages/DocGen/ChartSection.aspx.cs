using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;
using DocGen.Utility;
using System.Data;
//using System.Web.UI.DataVisualization;
//using System.Web.UI.DataVisualization.Charting;
using System.Configuration;
using ChartDirector;
using System.IO;
using System.Globalization;

namespace DocGen.Document.ChartSection
{
    public partial class Edit : SecurePage
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                gcTest.AccountID = int.Parse(Session["AccountID"].ToString());
                gcTest.STs = Session["STs"].ToString();
                gcTest.ParentPage = "section";
                gcTest.AvoidSize = true;
                gcTest.ChartWidth = 550;
                gcTest.ChartHeight = 500;

                if (Request.QueryString["DocumentID"] != null)
                {
                    gcTest.DocumentID=int.Parse(Convert.ToString(Request.QueryString["DocumentID"]));
                }

                if (Request.QueryString["PrevID"] != null)
                {
                    gcTest.PreDocumentSectionID = int.Parse(Request.QueryString["PrevID"].ToString());
                }

                if (Request.QueryString["DocumentSectionID"] != null && Request.QueryString["DocumentSectionID"].ToString()!="")
                {
                    gcTest.DocumentSectionID = int.Parse(Request.QueryString["DocumentSectionID"].ToString());
                }

                gcTest.BackURL = "javascript:parent.$.fancybox.close();";
                gcTest.ShowCustomDate = false;
               
                
            //}

        }

      
       

    }
}


