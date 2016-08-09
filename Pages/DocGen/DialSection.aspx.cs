using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;

namespace DocGen.Document.DialSection
{
    public partial class Edit : SecurePage
    {
        int _iAccountID;

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
            _iAccountID = int.Parse(Session["AccountID"].ToString());

            if (!IsPostBack)
            {
                PopulateTerminology();
                PopulateTableDDL();
                //PopulateDialYAxis1();
               
                PopulateTheRecord();
            }

        }
        protected void PopulateTheRecord()
        {
            if (DocumentSectionID <= 0)
            {
                if (Request.QueryString["PrevID"].ToString() != "-1")
                {
                    //hfRemoveSection.Value = "1";
                }
            }
            else
            {
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                    DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                    if (section != null)
                    {
                        //CheckPermission(section.DocumentID);
                        //txtTitle.Text = section.SectionName;
                        //txtDescription.Text = section.Content;
                        DialSectionDetail dialSecDetail = JSONField.GetTypedObject<DialSectionDetail>(section.Details);
                        if (dialSecDetail != null)
                        {
                            ddlDialTable1.SelectedValue = dialSecDetail.TableID.ToString();
                            PopulateDialYAxis1();
                            ddlDailYaxis1.SelectedValue = dialSecDetail.ColumnID.ToString();
                            ddlDialType1.SelectedValue = dialSecDetail.Dial;
                            txtDialLable1.Text = dialSecDetail.Label;
                            txtHeading.Text = dialSecDetail.Heading;
                            
                            if(dialSecDetail.Scale!=null)
                            txtScale1.Text = dialSecDetail.Scale.ToString();

                            if (dialSecDetail.Height != null)
                                txtHeight.Text = dialSecDetail.Height.ToString();

                            if (dialSecDetail.Width != null)
                                txtWidth.Text = dialSecDetail.Width.ToString(); 
                        }
                        
                        
                    }
                    else
                    {
                        //Response.Redirect("../Summary.aspx", true);
                    }
                }
            }


        }



        protected void PopulateTerminology()
        {
            stgField.InnerText = stgField.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
            stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));


        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                lblMsg.Text = "";
                DialSectionDetail dialSecDetail = new DialSectionDetail();

                dialSecDetail.TableID = int.Parse(ddlDialTable1.SelectedValue);
                dialSecDetail.ColumnID = int.Parse(ddlDailYaxis1.SelectedValue);
                dialSecDetail.Dial = ddlDialType1.SelectedValue;
                dialSecDetail.Label = txtDialLable1.Text;
                dialSecDetail.Heading = txtHeading.Text;

                if (txtScale1.Text.Trim() != "")
                {
                    dialSecDetail.Scale = int.Parse(txtScale1.Text);
                }
                else
                {
                    dialSecDetail.Scale = null;
                }

                if (txtHeight.Text.Trim() != "")
                {
                    dialSecDetail.Height = int.Parse(txtHeight.Text);
                }
                else
                {
                    dialSecDetail.Height = null;
                }


                if (txtWidth.Text.Trim() != "")
                {
                    dialSecDetail.Width = int.Parse(txtWidth.Text);
                }
                else
                {
                    dialSecDetail.Width = null;
                }


                try
                {
                    int ID = 0;

                    if (Request.QueryString["PrevID"].ToString() != "-1")
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
                            ctx.ExecuteCommand("UPDATE DocumentSection SET Position=Position + 1 WHERE DocumentID={0}  AND Position>{1}", DocumentID.ToString(), (iPosition - 1).ToString());
                            //}

                            newSection.DocumentID = DocumentID;
                            //newSection.SectionName = txtTitle.Text;
                            newSection.DocumentSectionTypeID = 8; //Dial

                            newSection.Details = dialSecDetail.GetJSONString();
                            newSection.Position = iPosition;
                            newSection.DateAdded = DateTime.Now;
                            newSection.DateUpdated = DateTime.Now;
                            ctx.DocumentSections.InsertOnSubmit(newSection);

                            ctx.SubmitChanges();
                            NewSectionID = newSection.DocumentSectionID;
                            ID = NewSectionID;
                        }

                        
                       

                    }
                    else
                    {
                        ID = DocumentSectionID;

                        
                        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                        {
                            DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == ID);
                            if (section != null)
                            {
                              
                                section.Details = dialSecDetail.GetJSONString();
                            }
                            ctx.SubmitChanges();
                            
                        }


                    }

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + ID.ToString() + ");", true);
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message;
                }

            }
        }


        protected void PopulateTableDDL()
        {
            int iTN = 0;

            ddlDialTable1.DataSource = RecordManager.ets_Table_Select(null,
                    null,
                    null,
                    _iAccountID,
                    null, null, true,
                    "st.TableName", "ASC",
                    null, null, ref  iTN, "");

                       
            ddlDialTable1.DataBind();
           
            System.Web.UI.WebControls.ListItem liSelect4 = new System.Web.UI.WebControls.ListItem("-None-", "");
            ddlDialTable1.Items.Insert(0, liSelect4);         
        }

        protected void ddlDialTable1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDialYAxis1();
            if (ddlDailYaxis1.SelectedItem != null)
                txtDialLable1.Text = ddlDailYaxis1.SelectedItem.Text;
        }

        protected void PopulateDialYAxis1()
        {
            ddlDailYaxis1.Items.Clear();

            int iTN = 0;
            List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(ddlDialTable1.SelectedValue),
                        null, null, ref iTN);

            Column dtColumn = new Column();
            foreach (Column eachColumn in lstColumns)
            {
                if (eachColumn.IsStandard == true)
                {

                }
                else
                {
                    if (eachColumn.DisplayTextSummary != "")
                    {
                        System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.DisplayTextSummary, eachColumn.ColumnID.ToString());
                        ddlDailYaxis1.Items.Insert(ddlDailYaxis1.Items.Count, aItem);
                    }
                }

            }

            if (iTN == 0)
            {
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-None-", "");
                ddlDailYaxis1.Items.Insert(0, liSelect);


            }

        }




        
        protected void ddlDailYaxis1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDailYaxis1.SelectedItem != null)
                txtDialLable1.Text = ddlDailYaxis1.SelectedItem.Text;
        }
}

}