using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using DocGen.DAL;
using System.Text.RegularExpressions;
using  DocGen.Utility;


using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;


namespace DocGen.Document
{
    public partial class View : SecurePage
    {
        public int DocumentID
        {
            get
            {
                int _DocumentID = 0;
                Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out _DocumentID);
                return _DocumentID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.UrlReferrer != null)
                    hlBack.NavigateUrl = Request.UrlReferrer.AbsoluteUri;

                if (Request.QueryString["TableID"] != null && Request.QueryString["SSearchCriteriaID"] != null && Request.QueryString["SearchCriteria"] != null)
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();
                    hlEdit.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/EditReport.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" + Request.QueryString["DocumentID"].ToString();
                }
                else
                {
                    hlEdit.NavigateUrl = hlBack.NavigateUrl;
                }

                
                int DocID = DocumentID;
                if (DocID <= 0)
                {
                    //Response.Redirect("Summary.aspx");
                }
                else
                {
                    StringBuilder sbStyles = new StringBuilder();
                    sbStyles.AppendLine("<style>");
                    //sbStyles.AppendLine("@page { size: 8.27in 11.69in; mso-page-orientation: Portrait Orientation; }");
                    //sbStyles.AppendLine("@page Section1 {margin:1.0in 1.0in 1.0in 1.0in; mso-paper-source:0;}");
                    //sbStyles.AppendLine("@page Section2 {size: 11.69in 8.27in; mso-page-orientation:landscape; margin:1.0in 1.0in 1.0in 1.0in; mso-paper-source:0;}");
                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {
                        var Styles = from s in ctx.DocumentSectionStyles
                                     where s.AccountID == this.AccountID
                                     orderby s.StyleName
                                     select s;
                        foreach (DAL.DocumentSectionStyle style in Styles)
                        {
                            sbStyles.AppendLine("/*" + style.StyleName + "*/");
                            sbStyles.Append(".DOCGEN_TextStyle_");
                            sbStyles.AppendLine(style.DocumentSectionStyleID.ToString());
                            sbStyles.AppendLine("{");
                            sbStyles.AppendLine(style.StyleDefinition);
                            sbStyles.AppendLine("}");
                            sbStyles.AppendLine();
                        }

                        DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocID);
                        if (doc != null)
                        {
                            if (doc.AccountID != this.AccountID)
                            {
                                Response.Redirect("../AccessDenied.aspx");
                            }
                            ltTitle.Text = doc.DocumentText;
                            bool HasContent = false;
                            int SCounter = 1;
                            StringBuilder sbTOC = new StringBuilder();
                            StringBuilder reportContent = new StringBuilder();
                            sbTOC.Append("<w:sdt sdtdocpart=\"t\" docparttype=\"Table of Contents\" docpartunique=\"t\" id=\"1\">");
                            sbTOC.Append("<p class=\"MsoTocHeading\">TABLE OF CONTENTS<w:sdtpr></w:sdtpr></p>");
                            reportContent.Append(String.Format("<div class=\"Section{0}\">", SCounter)); //Open new MS Word Section
                            foreach (DAL.DocumentSection section in doc.DocumentSections.OrderBy(s => s.Position))
                            {
                                switch (section.DocumentSectionTypeID)
                                {
                                    case 7: //Map Section
                                        if (section.ParentSectionID == null)
                                        {
                                            reportContent.Append(SectionGenerator.GenerateMapSection(section));
                                            reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;

                                    case 9: //DashChart Section
                                        if (section.ParentSectionID == null)
                                        {
                                            reportContent.Append(SectionGenerator.GenerateDashChartSection(section));
                                            reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;

                                    case 8: //Dial Section
                                        if (section.ParentSectionID == null)
                                        {
                                            reportContent.Append(SectionGenerator.GenerateDialSection(section));
                                            reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;

                                    case 1: //HTML Section
                                        if (section.ParentSectionID == null)
                                        {
                                            reportContent.Append(SectionGenerator.GenerateHTMLSection(section));
                                            //reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;
                                    case 2: //Text Section
                                        if (section.ParentSectionID == null)
                                        {
                                            string strTextPart = SectionGenerator.GenerateTextSection(section, ref sbTOC);
                                            strTextPart = DocGenManager.TextSectionValues(section, strTextPart);
                                            reportContent.Append(strTextPart);
                                            //reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;
                                    case 3: //Image Section
                                        if (section.ParentSectionID == null)
                                        {
                                            reportContent.Append(SectionGenerator.GenerateImageSection(section));
                                            //reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;
                                    case 4: //Columns Section
                                        if (section.ParentSectionID == null)
                                        {
                                            reportContent.Append(SectionGenerator.GenerateColumnsSection(section, false));
                                            //reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;
                                    case 5: //Chart Section
                                        if (section.ParentSectionID == null)
                                        {
                                            int iSearchCriteriaID = -1;
                                            //so far we are not using any filte, so it is now -1
                                            //if (Request.QueryString["SearchCriteria"] != null)
                                            //    iSearchCriteriaID = int.Parse( Cryptography.Decrypt( Request.QueryString["SearchCriteria"].ToString()));

                                            reportContent.Append(SectionGenerator.GenerateChartSection(section, iSearchCriteriaID));
                                            //reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;
                                    case 6: //Table Section
                                        if (section.ParentSectionID == null)
                                        {
                                            TableSectionFilter sectionFilter = JSONField.GetTypedObject<TableSectionFilter>(section.Filter);
                                            string strStartDate = "";
                                            string strEndDate = "";
                                            //DAL.DocumentFilter docFilter = DAL.JSONField.GetTypedObject<DAL.DocumentFilter>(doc.Filter);
                                            //   if (docFilter != null)
                                            //   {
                                            //       if (docFilter.StartDate.ToLocalTime() > DateTime.MinValue)
                                            //           strStartDate = ConvertUtil.GetDateString(docFilter.StartDate.ToLocalTime());
                                            //       if (docFilter.EndDate.ToLocalTime() > DateTime.MinValue)
                                            //           strEndDate = ConvertUtil.GetDateString(docFilter.EndDate.ToLocalTime());
                                            //   }

                                            for (int i = 0; i < sectionFilter.Params.Count - 1; i++)
                                            {

                                                if (strStartDate != "")
                                                {
                                                    if (sectionFilter.Params[i].Name == "@dDateFrom")
                                                    {
                                                        sectionFilter.Params[i].Value = ConvertUtil.GetDate(strStartDate);
                                                    }
                                                }
                                                if (strEndDate != "")
                                                {
                                                    if (sectionFilter.Params[i].Name == "@dDateTo")
                                                    {
                                                        sectionFilter.Params[i].Value = ConvertUtil.GetDate(strEndDate);
                                                    }
                                                }
                                            }

                                            sectionFilter.MaxRow = null;
                                            section.Filter = sectionFilter.GetJSONString();
                                            
                                            reportContent.Append(SectionGenerator.GenerateTableSection(section));
                                            //reportContent.Append("<br/>");
                                            HasContent = true;
                                        }
                                        break;
                                        //if (HasContent)
                                        //{
                                        //    reportContent.Append("</div>"); //Close last MS Word Section
                                        //    reportContent.Append("<br style=\"page-break-before:always\"/>");
                                        //    SCounter++;
                                        //    reportContent.Append(String.Format("<div class=\"Section{0}\">", SCounter)); //Open new MS Word Section
                                        //}
                                        //sbStyles.AppendLine("div.Section" + SCounter.ToString() + " {page:Section2;}");
                                        //break;
                                }
                            }
                            reportContent.Append("</div>"); //Close last MS Word Section    
                            sbTOC.Append("<p class=\"MsoNormal\"><!--[if supportFields]><span style='mso-element:field-end'></span><![endif]--><o:p>&nbsp;</o:p></p></w:sdt><br style=\"page-break-before:always\"/>");
                            Regex rTOC = new Regex("{TOC}");
                            ltReportContent.Text = rTOC.Replace(reportContent.ToString(), sbTOC.ToString());
                        }
                    }
                    sbStyles.AppendLine("</style>");
                    ltTextStyles.Text = sbStyles.ToString();
                }


                if (Request.QueryString["Export"] != null)
                {
                    lbSave_Click(null, null);
                }

                if (Request.QueryString["publish"] != null)
                {
                    PublishReport();
                }

            }
        }


        public int AccountID
        {
            get
            {
                int retVal = 0;
                if (Session["AccountID"] != null)
                    retVal = Convert.ToInt32(Session["AccountID"]);
                return retVal;
            }
        }

        protected void PublishReport()
        {

            //ExportPDF();
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);

                if (doc != null)
                {
                    doc.IsReportPublic = true;
                    doc.ReportHTML = ltTextStyles.Text + ltCommonStyles.Text + ltReportContent.Text;

                }
                ctx.SubmitChanges();

            }




            Response.Redirect("~/Pages/Document/ReportPublished.aspx?ReportID=" + Cryptography.Encrypt(DocumentID.ToString()), false);

        }

        protected void ExportPDF()
        {

            string strFileName = "Report.pdf";

            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);

                if (doc != null)
                {
                    if (doc.DocumentText.Trim()!="")
                        strFileName = doc.DocumentText + ".pdf";
                }              

            }


            //string strHTML="";


            //strHTML = strHTML+ (@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'>");
            //strHTML = strHTML+ ("<head>");
            //strHTML = strHTML+ ("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            //strHTML = strHTML+ ("<meta name=ProgId content=Word.Document>");
            //strHTML = strHTML+ ("<meta name=Generator content='Microsoft Word 9'>");
            //strHTML = strHTML+ ("<meta name=Originator content='Microsoft Word 9'>");
            //strHTML = strHTML+ (ltTextStyles.Text);
            //strHTML = strHTML+ (ltCommonStyles.Text);
            //strHTML = strHTML+ ("<head>");
            //strHTML = strHTML+ ("<body>");
            //strHTML = strHTML+ (ReFormatForMSWord(ltReportContent.Text));
            //strHTML = strHTML+ ("<!--[if gte mso 9]>");
            //strHTML = strHTML+ ("<xml>");
            //strHTML = strHTML+ ("<w:WordDocument>");
            //strHTML = strHTML+ ("<w:View>Print</w:View>");
            //strHTML = strHTML+ ("<w:Zoom>100</w:Zoom>");
            //strHTML = strHTML+ ("</w:WordDocument>");
            //strHTML = strHTML+ ("</xml>");
            //strHTML = strHTML+ ("<![endif]-->");
            //strHTML = strHTML+ ("</body>");
            //strHTML = strHTML+ ("</html>");




            //strHTML = ltReportContent.Text;


                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
            ltReportContent.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());          
            

            //StringReader sr = new StringReader(strHTML);

            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

            MemoryStream ms = new MemoryStream();
            PdfWriter w = PdfWriter.GetInstance(pdfDoc, ms);

            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();



            string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;

            string strFolder = Server.MapPath("~\\Pages\\Document\\Uploads");

            string strPath = strFolder + "\\" + strUniqueName;

            System.IO.FileStream file = System.IO.File.Create(strPath);
            file.Write(ms.ToArray(), 0, ms.ToArray().Length);
            file.Close();


            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);

                if (doc != null)
                {
                    doc.UniqueName = strUniqueName;
                    doc.FileTitle = strFileName;
                }
                ctx.SubmitChanges();

            }


           

            
        }


        protected void lbSave_Click(object sender, EventArgs e)
        {
            string strReportName = "Report";
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);
                if (doc != null)
                {
                    strReportName = doc.DocumentText;
                }
            }


            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=\""+strReportName+".doc\"");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.word";
            Response.Write(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'>");
            Response.Write("<head>");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            Response.Write("<meta name=ProgId content=Word.Document>");
            Response.Write("<meta name=Generator content='Microsoft Word 9'>");
            Response.Write("<meta name=Originator content='Microsoft Word 9'>");
            Response.Write(ltTextStyles.Text);
            Response.Write(ltCommonStyles.Text);
            Response.Write("<head>");
            Response.Write("<body>");
            Response.Write(ReFormatForMSWord(ltReportContent.Text));
            Response.Write("<!--[if gte mso 9]>");
            Response.Write("<xml>");
            Response.Write("<w:WordDocument>");
            Response.Write("<w:View>Print</w:View>");
            Response.Write("<w:Zoom>100</w:Zoom>");
            Response.Write("</w:WordDocument>");
            Response.Write("</xml>");
            Response.Write("<![endif]-->");
            Response.Write("</body>");
            Response.Write("</html>");
            Response.End();
        }

        protected string ReFormatForMSWord(string input)
        {
            string output = "";
            Regex rPageBreak = new Regex("<div style=\"page-break-after: always;\">(.*?)</div>");
            output = rPageBreak.Replace(input, "<br style=\"page-break-before:always\"/>");
            return output;
        }
    }
}