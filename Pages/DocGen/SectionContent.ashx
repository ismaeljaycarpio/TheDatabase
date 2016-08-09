<%@ WebHandler Language="C#"  Class="DocGen.DragAndDrop.SectionContent" %>

using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using DocGen.DAL;
using System.Text;
namespace DocGen.DragAndDrop
{
    /// <summary>
    /// Summary description for SectionContent
    /// </summary>
    public class SectionContent : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            int SectionID = Convert.ToInt32(context.Request.QueryString["DocumentSectionID"]);
            string Mode = Convert.ToString(context.Request.QueryString["Mode"]).ToUpper();
            DocumentSection sct = null;
            using (DocGenDataContext ctx = new DocGenDataContext())
            {
                sct = ctx.DocumentSections.SingleOrDefault<DocumentSection>(s => s.DocumentSectionID == SectionID);
            }
            if (sct != null)
            {
                switch (sct.DocumentSectionTypeID.ToString())
                {
                    case "2": //Text
                        ShowTextSection(sct, context);
                        break;
                    case "3":     //Image
                        ShowImageSection(sct, context);
                        break;
                    case "6":        //Table
                        ShowTableSection(sct, context);
                        break;
                    case "1":      //HTML
                        ShowHTMLSection(sct, context);
                        break;
                    case "5":         //Chart
                        ShowChartSection(sct, context);
                        break;
                    case "4":   //Columns
                        ShowColumnsSection(sct, Mode, context);
                        break;
                    case "7":   //Columns
                        ShowMapSection(sct,  context);
                        break;
                    case "9":         //Chart
                        ShowDashChartSection(sct, context);
                        break;
                    case "10":         //Record Table
                        ShowRecordTableSection(sct, context);
                        break;
                    case "8":         //Dial
                        ShowDialSection(sct, context);
                        break;
                    case "11":         //calendar
                        ShowCalendarSection(sct, context);
                        break;
                }
            }
        }

        private void ShowDialSection(DocumentSection section, HttpContext httpCtx)
        {
            string strSectionContent = SectionGenerator.GenerateDialSection(section);

            httpCtx.Response.Write(strSectionContent);
        }

        private void ShowTextSection(DocumentSection section, HttpContext httpCtx)
        {
            httpCtx.Response.ContentType = "text/plain";

            StringBuilder sbTOC = new StringBuilder();
            string strSectionContent = SectionGenerator.GenerateTextSection(section, ref sbTOC);

            httpCtx.Response.Write(strSectionContent);
        }

        private void ShowImageSection(DocumentSection section, HttpContext httpCtx)
        {
            string strSectionContent = SectionGenerator.GenerateImageSection(section);

            httpCtx.Response.Write(strSectionContent);
        }

        private void ShowTableSection(DocumentSection section, HttpContext httpCtx)
        {

            string strSectionContent = SectionGenerator.GenerateTableSection(section);

            httpCtx.Response.Write(strSectionContent);
        }

        private void ShowHTMLSection(DocumentSection section, HttpContext httpCtx)
        {
            string strSectionContent = SectionGenerator.GenerateHTMLSection(section);

            httpCtx.Response.Write(strSectionContent);
        }

        private void ShowMapSection(DocumentSection section, HttpContext httpCtx)
        {
            string strSectionContent = SectionGenerator.GenerateMapSection(section);

            httpCtx.Response.Write(strSectionContent);
        }

        private void ShowRecordTableSection(DocumentSection section, HttpContext httpCtx)
        {
            if (section.ParentSectionID != null)
            {
                section.ValueFields = "half";
            }
            
            string strSectionContent = SectionGenerator.GenerateRecordTableSection(section);

            httpCtx.Response.Write(strSectionContent);
        }


        private void ShowCalendarSection(DocumentSection section, HttpContext httpCtx)
        {
            
            string strSectionContent = SectionGenerator.GenerateCalendarSection(section);

            httpCtx.Response.Write(strSectionContent);
        }
        private void ShowDashChartSection(DocumentSection section, HttpContext httpCtx)
        {
            string strSectionContent = SectionGenerator.GenerateDashChartSection(section);

            httpCtx.Response.Write(strSectionContent);
        }
        
        private void ShowChartSection(DocumentSection section, HttpContext httpCtx)
        {
            string strSectionContent = SectionGenerator.GenerateChartSection(section,-1);

            httpCtx.Response.Write(strSectionContent);
        }

        private void ShowColumnsSection(DocumentSection section, string Mode, HttpContext httpCtx)
        {
            string strSectionContent = SectionGenerator.GenerateColumnsSection(section, Mode == "EDIT");
            httpCtx.Response.Write(strSectionContent);
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}