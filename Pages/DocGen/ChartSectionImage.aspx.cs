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
using System.IO;
using System.Configuration;
using System.Globalization;
namespace DocGen.Document.ChartSection
{
    public partial class ChartSectionImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                int DocumentSectionID = 0;
                Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out DocumentSectionID);
                DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                //string FileName = Request.PhysicalApplicationPath + @"\Images\Charts\" + section.DocumentSectionID.ToString() + ".png";

                Guid newGuid = Guid.NewGuid();

                ViewState["_strUniqueName"] = newGuid.ToString();
                string strFileName = Request.PhysicalApplicationPath + @"Images\Charts\" + newGuid.ToString() + ".png";

                bool NeedNewChartImg = true;
                if (File.Exists(strFileName))
                {
                    if (File.GetLastWriteTime(strFileName) > section.DateUpdated)
                    {
                        NeedNewChartImg = false;
                    }
                }
                if (NeedNewChartImg)
                {
                    DAL.Document Doc = ctx.Documents.SingleOrDefault<DAL.Document>(s => s.DocumentID == section.DocumentID);

                    gcTest.ParentPage = "section";
                    gcTest.AccountID = Doc.AccountID;
                    gcTest.STs = "";
                    
                    gcTest.DocumentID = section.DocumentID;
                    gcTest.DocumentSectionID = DocumentSectionID;
                    gcTest.SaveFileName = strFileName;
                    gcTest.Mode = "edit";
                    //Chart1 = gcTest.ChartControl;
                }



                //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance((byte[])HttpContext.Current.Session[ViewState["_strUniqueName"].ToString()]);

                //Chart1.StreamChart();
                Response.ContentType = "image/png";
                Response.WriteFile(strFileName);
            }
        }

        
    }
}