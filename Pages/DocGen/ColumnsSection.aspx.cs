using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

namespace DocGen.Document.ColumnsSection
{
    public partial class Edit : SecurePage
    {

        public int DocumentSectionID
        {
            get
            {
                int _DocumentSectionID = 0;
                Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out _DocumentSectionID);
                return _DocumentSectionID;
            }
        }

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

                try
                {
                    //Populate 

                    ddlSpacing.Items.Clear();

                    for (int i = 0; i < 101; i++)
                    {
                        ddlSpacing.Items.Add(i.ToString());
                    }
                    ddlSpacing.Text = "2";


                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {
                        DAL.DocumentSection section = null;
                        if (DocumentSectionID > 0)
                        {
                            section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                            ddlNumberOfCols.SelectedValue = section.Content;

                            DAL.ColumnsSectionDetail sectionDetail = DAL.JSONField.GetTypedObject<DAL.ColumnsSectionDetail>(section.Details);

                            if (sectionDetail.Spacing != null)
                            {
                                ddlSpacing.SelectedValue = sectionDetail.Spacing.ToString();
                            }

                        }

                    }
                }
                catch
                {
                    //
                }

            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            int ReturnID = DocumentSectionID;
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.DocumentSection section = null;
                if (DocumentSectionID > 0)
                {
                    section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                }
                if (section == null)
                {
                    section = new DAL.DocumentSection();
                    int NewPosition = 1;
                    int PrevSectionID = 0;
                    Int32.TryParse(Request.QueryString["PrevID"], out PrevSectionID);
                    if (PrevSectionID > 0)
                    {
                        DAL.DocumentSection PreSection = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == PrevSectionID);
                        if (PreSection != null)
                        {
                            NewPosition = PreSection.Position + 1;
                            ctx.ExecuteCommand("UPDATE DocumentSection SET Position = Position + 1 WHERE DocumentID= {0} AND Position > {1}", DocumentID, PreSection.Position);
                        }
                    }
                    section.DocumentID = DocumentID;
                    section.DocumentSectionTypeID = 4; //Columns Section
                    section.Position = NewPosition;
                    section.DateAdded = section.DateUpdated = DateTime.Now;
                    ctx.DocumentSections.InsertOnSubmit(section);
                }
                section.SectionName = "N/A";
                section.Content = ddlNumberOfCols.SelectedItem.Text;
                DAL.ColumnsSectionDetail csd = new DAL.ColumnsSectionDetail();
                string[] arr = txtWidths.Text.Split('\n');
                int width = 0;
                csd.Widths = new List<int>();
                foreach (string w in arr)
                {
                    if (Int32.TryParse(w.Trim(), out width))
                    {
                        csd.Widths.Add(width);
                    }
                }

                csd.Spacing = int.Parse(ddlSpacing.SelectedValue);
                section.Details = csd.GetJSONString();
                section.DateUpdated = DateTime.Now;
                try
                {
                    ctx.SubmitChanges();
                }
                catch(Exception ex)
                {
                    string s = ex.Message;
                }
                if (ReturnID <= 0)
                    ReturnID = section.DocumentSectionID;
            }
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + ReturnID.ToString() + ");", true);
        }

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionCancelEditting();", true);
        //}
    }
}