using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.Utility;
using DocGen.DAL;
using BaseClasses;
using System.Text;
using System.Configuration;



namespace DocGen.Document
{
    public partial class DashBoard : SecurePage
    {
        public int DocumentID
        {
            get
            {
                //260, 237
                hfDocumentID.Value = 260.ToString();
                return 260;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AddStyleCSS();

                int ID = DocumentID;
                if (ID > 0)
                {
                    
                    ShowList();
                }
                else
                {
                    //Response.Redirect("Summary.aspx");
                }
            }



            string strFancy = @"
                
                jQuery(document).ready(function() {
                     
                   

                         $(""#hlProperties"").fancybox({                                   
                        'transitionIn'  : 'elastic',
                        'transitionOut' : 'none',
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 750,
                                height: 450,
                                titleShow: false                               
                            });




                     });                

                                       

                    ";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);




        }


        protected void AddStyleCSS()
        {
            StringBuilder sbStyles = new StringBuilder();
            sbStyles.AppendLine("<style>");

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
                sbStyles.AppendLine("</style>");
                ltTextStyles.Text = sbStyles.ToString();



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

        protected void ShowList()
        {


            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                var query = from s in ctx.DocumentSections
                            where s.DocumentID == DocumentID && (!s.ParentSectionID.HasValue || s.ParentSectionID == 0)
                            orderby s.Position
                            select s;

                rptSection.DataSource = query;
                rptSection.DataBind();

                DAL.DocumentSectionStyle docStyle = ctx.DocumentSectionStyles.SingleOrDefault<DAL.DocumentSectionStyle>(d => d.StyleName == "Heading 1" && d.AccountID == this.AccountID);

                if (rptSection.Items.Count == 0)
                {
                    //insert a empty Text

                    DAL.DocumentSection newSection = new DAL.DocumentSection();
                    newSection.DocumentID = DocumentID;
                    //newSection.SectionName = txtTitle.Text;
                    newSection.DocumentSectionTypeID = 2; //Text
                    newSection.DocumentSectionStyleID = docStyle.DocumentSectionStyleID;  //int.Parse(ConfigurationManager.AppSettings["DefaultTextStyleID"].ToString());
                    newSection.Content = "Empty Text";
                    newSection.Position = 1;
                    newSection.DateAdded = DateTime.Now;
                    newSection.DateUpdated = DateTime.Now;

                    DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);
                    if (doc != null)
                    {
                        newSection.Content = doc.DocumentText;
                    }
                    ctx.DocumentSections.InsertOnSubmit(newSection);



                    ctx.SubmitChanges();

                    var query2 = from s in ctx.DocumentSections
                                 where s.DocumentID == DocumentID
                                 orderby s.Position
                                 select s;

                    rptSection.DataSource = query2;
                    rptSection.DataBind();

                }
            }
        }




        protected void btnSectionFilter_Click(object sender, EventArgs e)
        {
            ShowList();
        }

        protected void ViewButton_Click(object sender, EventArgs e)
        {
            int iSearchCriteriaID = -1;
            //if (txtStartDate.Text != "" && txtEndDate.Text != "")
            //{

            //SearchCriteria 

            //try
            //{
            //    string xml = null;
            //    xml = @"<root>" +
            //           " <" + txtStartDate.ID + ">" + HttpUtility.HtmlEncode(txtStartDate.Text) + "</" + txtStartDate.ID + ">" +
            //           " <" + txtEndDate.ID + ">" + HttpUtility.HtmlEncode(txtEndDate.Text) + "</" + txtEndDate.ID + ">" +
            //          "</root>";

            //    SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            //    iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
            //}
            //catch (Exception ex)
            //{
            //   //
            //}

            //End Searchcriteria


            //}

            Response.Redirect("View.aspx?DocumentID=" + DocumentID.ToString()
                    + "&SearchCriteria=" + iSearchCriteriaID.ToString(), false);

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {

        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowList();
        }
        protected void lnkExporttoWord_Click(object sender, EventArgs e)
        {
            Response.Redirect("View.aspx?Export=yes&DocumentID=" + DocumentID.ToString()
                   + "&SearchCriteria=" + Cryptography.Encrypt("-1"), false);
        }
        protected void lnkPublish_Click(object sender, EventArgs e)
        {

            Response.Redirect("View.aspx?publish=yes&DocumentID=" + DocumentID.ToString()
                   + "&SearchCriteria=" + Cryptography.Encrypt("-1"), false);

        }
    }
}