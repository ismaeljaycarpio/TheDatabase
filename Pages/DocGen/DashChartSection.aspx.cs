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

namespace DocGen.Document.DashChartSection
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
                gcTest.IsDashBorad = true;
                gcTest.AvoidSize = true;
                gcTest.ChartWidth = 550;
                gcTest.ChartHeight=500;
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

                    using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
                    {
                        int DocumentSectionID = 0;
                        Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out DocumentSectionID);
                        DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);



                        DocGen.DAL.Document Doc = ctx.Documents.SingleOrDefault<DocGen.DAL.Document>(s => s.DocumentID == section.DocumentID);


                        gcTest.AccountID = Doc.AccountID;
                        gcTest.STs = "";
                        gcTest.IsDashBorad = true;

                        if (section.Details != "")
                        {
                            GraphOption theGraphOption = GraphManager.ets_GraphOption_Detail(int.Parse(section.Details));

                            if (theGraphOption != null)
                            {
                                if (theGraphOption.Width != null)
                                    gcTest.ChartWidth = (int)theGraphOption.Width;

                                if (theGraphOption.Height != null)
                                    gcTest.ChartHeight = (int)theGraphOption.Height;

                            }
                        }


                        if (section.ValueFields != "")
                        {
                            gcTest.RecentDays = section.ValueFields;
                        }

                        gcTest.GraphOptionID = int.Parse(section.Details);

                        gcTest.Mode = "edit";



                    }


                    
                }

                gcTest.BackURL = "javascript:parent.$.fancybox.close();";
                //gcTest.ShowCustomDate = false;
               
                
            //}

        }

      
       

    }
}


