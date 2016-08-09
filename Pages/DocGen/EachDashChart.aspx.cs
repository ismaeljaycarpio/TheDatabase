using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DocGen;
using DocGen.DAL;
using DocGen.Utility;
using System.IO;

public partial class Pages_DocGen_EachDashChart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //gcGraph.AccountID = int.Parse(Session["AccountID"].ToString());
        //gcGraph.STs = Session["STs"].ToString();

        using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
        {
            int DocumentSectionID = 0;
            Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out DocumentSectionID);
            DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);


            
            DocGen.DAL.Document Doc = ctx.Documents.SingleOrDefault<DocGen.DAL.Document>(s => s.DocumentID == section.DocumentID);

            gcGraph.ParentPage = "home";
            gcGraph.AccountID = Doc.AccountID;
            gcGraph.STs = "";
            gcGraph.IsDashBorad = true;

            if (section.Details != "")
            {
                GraphOption theGraphOption = GraphManager.ets_GraphOption_Detail(int.Parse(section.Details));

                if (theGraphOption != null)
                {
                    if(theGraphOption.Width!=null)
                        gcGraph.ChartWidth = (int)theGraphOption.Width;

                    if (theGraphOption.Height != null)
                        gcGraph.ChartHeight = (int)theGraphOption.Height;

                }
            }


            if (section.ValueFields != "")
            {
                gcGraph.RecentDays = section.ValueFields;
            }

            //gcGraph.DocumentID = section.DocumentID;
            //gcGraph.DocumentSectionID = DocumentSectionID;

           
            gcGraph.GraphOptionID = int.Parse(section.Details);                
              

            //gcGraph.SaveFileName = strFileName;
            gcGraph.Mode = "edit";
                
            
                        
        }




    }
}