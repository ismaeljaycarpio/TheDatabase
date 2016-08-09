using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DocGen.DAL;

namespace DocGen.WebServices
{
    /// <summary>
    /// Summary description for DocGenWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DocGenWS : System.Web.Services.WebService
    {
        //[WebMethod]
        //public int UpdateSectionText(DocumentSectionText sectionText)
        //{
        //    using (DocGenDataContext ctx = new DocGenDataContext())
        //    {
        //        DocumentSectionText st = ctx.DocumentSectionTexts.SingleOrDefault<DocumentSectionText>(stext => stext.DocumentSectionTextID == sectionText.DocumentSectionTextID);
        //        if (st != null)
        //        {
        //            st.Content = sectionText.Content;
        //            st.DocumentTextStyleID = sectionText.DocumentTextStyleID;
        //            ctx.SubmitChanges();
        //        }
        //    }
        //    return sectionText.DocumentSectionTextID;
        //}

        [WebMethod]
        public int UpdateSectionText(DocumentSection sectionText)
        {
            using (DocGenDataContext ctx = new DocGenDataContext())
            {
                DocumentSection st = ctx.DocumentSections.SingleOrDefault<DocumentSection>(stext => stext.DocumentSectionID == sectionText.DocumentSectionID);
                if (st != null)
                {
                    st.Content = sectionText.Content;
                    st.DocumentSectionStyleID = sectionText.DocumentSectionStyleID;
                    ctx.SubmitChanges();
                }
            }
            return sectionText.DocumentSectionID;
        }

    }
}
