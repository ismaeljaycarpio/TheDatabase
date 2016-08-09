using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

namespace DocGen.Document.TextSection
{
    public partial class Edit : SecurePage
    {

        public int DocumentSectionID
        {
            get
            {
                int _DocumentSectionID = 0;
                if (Request.QueryString["DocumentSectionID"] != null)
                {
                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out _DocumentSectionID);
                }
                return _DocumentSectionID;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            //CancelButton.Text = " <strong> Back</strong>";
            if (!IsPostBack)
            {
                //string strFancy = @" parent.$.fancybox.close();";

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox", strFancy, true);

                PopulateStyles();



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
                   
                }
                sbStyles.AppendLine("</style>");
                ltTextStyles.Text = sbStyles.ToString();


                if (DocumentSectionID <= 0)
                {
                    if (Request.QueryString["PrevID"].ToString() != "-1")
                    {
                        hfRemoveSection.Value = "1";
                    }

                    //Response.Redirect("../Summary.aspx");
                }
                else
                {
                   
                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {
                       


                        DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                        if (section != null)
                        {
                            //CheckPermission(section.DocumentID);
                            txtContent.Text = section.Content;
                            ddlStyle.Text = section.DocumentSectionStyleID.ToString();
                            //CancelButton.CommandArgument = section.DocumentID.ToString();
                        }
                        else
                        {
                            //Response.Redirect("../Summary.aspx", true);
                        }
                    }
                   

                  
                    //ShowList();
                }

                txtContent.CssClass = "DOCGEN_TextStyle_" + ddlStyle.SelectedValue;
            }



            string strJS = @"$(document).ready(function(){
                            $('#ddlMergeField').change(function(e){  
                                    insertAtCaret();    
                            });   
                        });
                        ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "strJS", strJS, true);

        }

        protected void CheckPermission(int DocumentID)
        {
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID && d.AccountID == this.AccountID);
                if (doc == null)
                {
                    Response.Redirect("~/Empty.aspx", false);
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
       

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            int DocumentSectionID = 0;
            if (Request.QueryString["PrevID"].ToString()!="-1")
            {

                int DocumentID = 0;
                Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);

                int iPosition = 1;

                //if (Request.QueryString["Position"] != null)
                //{
                //    iPosition = int.Parse(Convert.ToString(Request.QueryString["Position"])) + 1;
                //}

                int NewSectionID = 0;
              
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {

                    if (Request.QueryString["PrevID"].ToString() != "0")
                    {

                        DAL.DocumentSection PreSection = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == int.Parse(Request.QueryString["PrevID"].ToString()));

                        iPosition = PreSection.Position + 1;
                    }
                    else
                    {
                        iPosition = 1;
                    }

                    DAL.DocumentSection newSection = new DAL.DocumentSection();

                    //if (Request.QueryString["Position"] != null)
                    //{
                        ctx.ExecuteCommand("UPDATE DocumentSection SET Position=Position + 1 WHERE DocumentID={0}  AND Position>{1}"  , DocumentID.ToString(),(iPosition - 1).ToString());
                    //}
                                        
                    newSection.DocumentID = DocumentID;
                    //newSection.SectionName = txtTitle.Text;
                    newSection.DocumentSectionTypeID = 2; //Text
                    newSection.DocumentSectionStyleID =int.Parse( ddlStyle.SelectedValue);
                    newSection.Content = txtContent.Text;
                    newSection.Position = iPosition;
                    newSection.DateAdded = DateTime.Now;
                    newSection.DateUpdated = DateTime.Now;
                    ctx.DocumentSections.InsertOnSubmit(newSection);

                    
                  
                    ctx.SubmitChanges();

                    NewSectionID = newSection.DocumentSectionID;
                    DocumentSectionID = NewSectionID;
                    hfRemoveSection.Value = "0";
                }             


            }
            else
            {
               
                Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out DocumentSectionID);
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                    DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                    section.Content = txtContent.Text;
                    section.DocumentSectionStyleID = int.Parse(ddlStyle.SelectedValue);
                    ctx.SubmitChanges();
                }
            }
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + DocumentSectionID.ToString() + ");", true); 


            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Refresh", "SavedAndRefresh();", true);

        }

        //protected void lbDelete_Click(object sender, EventArgs e)
        //{
        //    List<int> lstDeleteIDs = new List<int>(); 
        //    foreach (GridViewRow r in gvSectionText.Rows)
        //    {
        //        CheckBox chkSelect = (CheckBox)r.FindControl("chkSelect");
        //        if (chkSelect.Checked)
        //        {
        //            lstDeleteIDs.Add(Convert.ToInt32(gvSectionText.DataKeys[r.RowIndex].Value));                    
        //        }
        //    }
        //    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
        //    {
        //        var deleteItems = from st in ctx.DocumentSectionTexts
        //                          where lstDeleteIDs.Contains(st.DocumentSectionTextID)
        //                          select st;
        //        ctx.DocumentSectionTexts.DeleteAllOnSubmit(deleteItems);
        //        ctx.SubmitChanges();
        //    }
        //    ShowList();
        //}

        //protected void lbAdd_Click(object sender, EventArgs e)
        //{
        //    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
        //    {
        //        int? NewPosition = ctx.DocumentSectionTexts.Max(st => st.Position);
        //        if (NewPosition.HasValue)
        //            NewPosition = NewPosition + 1;
        //        else
        //            NewPosition = 1;
        //        DAL.DocumentSectionText newSectionText = new DAL.DocumentSectionText()
        //        {
        //            DocumentSectionID = DocumentSectionID,
        //            DocumentTextStyleID = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultTextStyleID"]),
        //            Position = NewPosition,
        //            Content = "",
        //            DateCreated = DateTime.Now,
        //            DateUpdated = DateTime.Now
        //        };
        //        ctx.DocumentSectionTexts.InsertOnSubmit(newSectionText);
        //        ctx.SubmitChanges();
        //    }
        //    ShowList();
        //}

        //protected void gvSectionText_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItemIndex >= 0)
        //    {
        //        DAL.DocumentSectionText sectionText = (DAL.DocumentSectionText)e.Row.DataItem;
        //        DAL.DocumentTextStyle style = sectionText.DocumentTextStyle;
        //        LinkButton lbDown = (LinkButton)e.Row.FindControl("lbDown");
        //        lbDown.CommandArgument = sectionText.DocumentSectionTextID.ToString();
        //        if (e.Row.DataItemIndex > 0)
        //        {
        //            LinkButton lbUp = (LinkButton)e.Row.FindControl("lbUp");
        //            LinkButton lbDown_Prev = (LinkButton)gvSectionText.Rows[e.Row.RowIndex - 1].FindControl("lbDown");
        //            lbUp.Visible = true;
        //            lbDown_Prev.Visible = true;
        //            lbDown_Prev.CommandArgument += ":" + sectionText.DocumentSectionTextID.ToString();
        //            lbUp.CommandArgument = lbDown_Prev.CommandArgument;
        //        }
                
        //        TextBox txtContent = (TextBox)e.Row.FindControl("txtContent");
        //        DropDownList ddlStyle = (DropDownList)e.Row.FindControl("ddlStyle");
        //        txtContent.Text = sectionText.Content;
        //        txtContent.CssClass = "DOCGEN_TextStyle_" + style.DocumentTextStyleID.ToString();
        //        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
        //        {
        //            var Styles = from s in ctx.DocumentTextStyles
        //                         where s.AccountID == 
        //                            orderby s.StyleName
        //                            select s;
        //            ddlStyle.DataSource = Styles;
        //            ddlStyle.DataTextField = "StyleName";
        //            ddlStyle.DataValueField = "DocumentTextStyleID";
        //            ddlStyle.DataBind();
        //        }
        //        ddlStyle.SelectedValue = sectionText.DocumentTextStyleID.ToString();

        //        txtContent.Attributes.Add("ItemID", sectionText.DocumentSectionTextID.ToString());
        //        ddlStyle.Attributes.Add("ItemID", sectionText.DocumentSectionTextID.ToString());
        //    }
        //}        


        protected void PopulateStyles()
        {
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                var Styles = from s in ctx.DocumentSectionStyles
                             where s.AccountID == this.AccountID
                             orderby s.StyleName
                             select s;
                ddlStyle.DataSource = Styles;
                ddlStyle.DataTextField = "StyleName";
                ddlStyle.DataValueField = "DocumentSectionStyleID";
                ddlStyle.DataBind();
            }

            try
            {
                //ddlStyle.Text = ConfigurationManager.AppSettings["DefaultTextStyleID"].ToString();
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                    DAL.DocumentSectionStyle docStyle = ctx.DocumentSectionStyles.SingleOrDefault<DAL.DocumentSectionStyle>(d => d.StyleName == "Normal" && d.AccountID == this.AccountID);

                    ddlStyle.Text = docStyle.DocumentSectionStyleID.ToString();
                }
            }
            catch
            {

            }

           

        }

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    //Response.Redirect("../Edit.aspx?DocumentID=" + CancelButton.CommandArgument);
        //}

        #region Old code
        /*
        protected void gvSectionText_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int DocumentSectionTextID = Convert.ToInt32(e.Keys["DocumentSectionTextID"]);
            TextBox txtContent = (TextBox)gvSectionText.Rows[e.RowIndex].FindControl("txtContent");
            DropDownList ddlStyle = (DropDownList)gvSectionText.Rows[e.RowIndex].FindControl("ddlStyle");
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.DocumentSectionText sectionText = ctx.DocumentSectionTexts.SingleOrDefault<DAL.DocumentSectionText>(st => st.DocumentSectionTextID == DocumentSectionTextID);
                if (sectionText == null)
                {
                    int? NewPosition = ctx.DocumentSectionTexts.Max(st => st.Position);
                    if (NewPosition.HasValue)
                        NewPosition = NewPosition + 1;
                    else
                        NewPosition = 1;
                    sectionText = new DAL.DocumentSectionText()
                    {
                        DocumentSectionID = DocumentSectionID,
                        DocumentTextStyleID = Convert.ToInt32(ddlStyle.SelectedValue),
                        Position = NewPosition,
                        Content = txtContent.Text,
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now
                    };
                    ctx.DocumentSectionTexts.InsertOnSubmit(sectionText);
                }
                else
                {
                    sectionText.Content = txtContent.Text;
                    sectionText.DocumentTextStyleID = Convert.ToInt32(ddlStyle.SelectedValue);
                }
                ctx.SubmitChanges();
            }
            gvSectionText.EditIndex = -1;
            ShowList();
        }
        */
        #endregion

        //protected void gvSectionText_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    switch (e.CommandName)
        //    {
        //        case "MoveUp":
        //        case "MoveDown":
        //            int ID1 = Convert.ToInt32(e.CommandArgument.ToString().Split(':')[0]);
        //            int ID2 = Convert.ToInt32(e.CommandArgument.ToString().Split(':')[1]);
        //            SwapRow(ID1, ID2);
        //            break;
        //    }
        //}

        //protected void SwapRow(int ID1, int ID2)
        //{
        //    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
        //    {
        //        DAL.DocumentSectionText st1 = ctx.DocumentSectionTexts.SingleOrDefault<DAL.DocumentSectionText>(st => st.DocumentSectionTextID == ID1);
        //        DAL.DocumentSectionText st2 = ctx.DocumentSectionTexts.SingleOrDefault<DAL.DocumentSectionText>(st => st.DocumentSectionTextID == ID2);
        //        if (st1 != null && st2 != null)
        //        {
        //            int TempPosition = st1.Position.Value;
        //            st1.Position = st2.Position;
        //            st2.Position = TempPosition;
        //            ctx.SubmitChanges();
        //        }
        //    }            
        //    ShowList();
        //}
        protected void ddlStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtContent.CssClass = "DOCGEN_TextStyle_" + ddlStyle.SelectedValue;
        }
}
}