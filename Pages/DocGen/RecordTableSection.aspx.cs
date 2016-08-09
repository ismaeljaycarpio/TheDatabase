using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;
using DocGen.Utility;
using System.Data;

namespace DocGen.Document.STRecordTableSection
{
    public partial class Edit : SecurePage
    {
        //string _strNumericSearch = "";
        //string _strTextSearch = "";
        //string _strGridViewSortColumn = "DBGSystemRecordID";
        //string _strGridViewSortDirection = "DESC";

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

        protected void PopulateTerminology()
        {
            //lblLocationCap.Text = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), lblLocationCap.Text, lblLocationCap.Text);

            stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                   
                    int DocumentID1 = 0;
                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID1);
                    DAL.Document doc1 = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID1);
                    

                    PopulateTableDDL();

                    if (Request.QueryString["TableID"] != null)
                    {
                        ListItem liFind = ddlTable.Items.FindByValue(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
                        if (liFind != null)
                        {
                            ddlTable.SelectedValue = liFind.Value;
                        }
                        else
                        {
                            rlOne.Visible = false;
                        }
                    }
                    else
                    {
                        rlOne.Visible = false;
                    }

                    //string strFilterSystemName = "";
                    DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                    if (section != null)
                    {
                        CheckPermission(section.DocumentID);
                        //txtTitle.Text = section.SectionName;
                        RecordTableSectionDetail rtSection = JSONField.GetTypedObject<RecordTableSectionDetail>(section.Details);


                        if (rtSection != null)
                        {
                            //rlOne.TableID = rtSection.TableID;
                            if (Request.QueryString["RecordTable"] == null)
                            {
                                Response.Redirect("~/Pages/DocGen/RecordTableSection.aspx?RecordTable=yes&TableID=" + Cryptography.Encrypt(rtSection.TableID.ToString()) + "&PrevID=" + Request.QueryString["PrevID"].ToString() + "&DocumentID=" + Request.QueryString["DocumentID"].ToString() + "&DocumentSectionID=" + Request.QueryString["DocumentSectionID"].ToString() + "&ViewID=" + Cryptography.Encrypt(rtSection.ViewID.ToString()), true);

                                return;
                            }
                        }

                        


                        //CancelButton.CommandArgument = section.DocumentID.ToString();
                    }
                    else
                    {
                        if (Request.QueryString["PrevID"].ToString() != "-1")
                        {
                            hfRemoveSection.Value = "1";
                        }
                        //new
                        if (Request.QueryString["DocumentID"] != null)
                        {

                            int DocumentID = 0;
                            Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);
                            DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);
                            if (doc != null)
                            {
                                if (doc.ForDashBoard != null)
                                {
                                    if ((bool)doc.ForDashBoard)
                                    {
                                      

                                    }
                                }                            
                                       
                            }
                        }
                     

                        //Response.Redirect("../Summary.aspx", true);
                    }
                }

                PopulateTerminology();
            }
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

     
      
        protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
        {

            //rlOne.TableID = int.Parse(ddlTable.SelectedValue);
            //rlOne.CallSearch();
            //lbTest_Click(null, null);

            if (ddlTable.SelectedValue != "")
            {
                if (Request.QueryString["DocumentSectionID"] == null)
                {
                    Response.Redirect("~/Pages/DocGen/RecordTableSection.aspx?RecordTable=yes&TableID=" + Cryptography.Encrypt(ddlTable.SelectedValue) + "&PrevID=" + Request.QueryString["PrevID"].ToString() + "&DocumentID=" + Request.QueryString["DocumentID"].ToString(), false); //+ "&ViewID=" + Cryptography.Encrypt(rlOne.ViewID.ToString())
                }
                else
                {
                    Response.Redirect("~/Pages/DocGen/RecordTableSection.aspx?RecordTable=yes&TableID=" + Cryptography.Encrypt(ddlTable.SelectedValue) + "&PrevID=" + Request.QueryString["PrevID"].ToString() + "&DocumentID=" + Request.QueryString["DocumentID"].ToString() + "&DocumentSectionID=" + Request.QueryString["DocumentSectionID"].ToString(), false); //+ "&ViewID=" + Cryptography.Encrypt(rlOne.ViewID.ToString())
                }
            }

        }




   
        protected void lbTest_Click(object sender, EventArgs e)
        {
            //ErrorMessage.Text = "";
            //if (ddlStoredProcedure.SelectedIndex > 0)
            //rlOne.TableID = int.Parse(ddlTable.SelectedValue);
            //rlOne.CallSearch();
            //rlOne.




        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {                
                ErrorMessage.Text = "";
                try
                {
                    int ID = 0;
                    if (Request.QueryString["PrevID"].ToString() != "-1")
                    {
                        int DocumentID = 0;
                        Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);
                        int iPosition = 1;                                             
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

                            bool ValidParams = true;
                            DAL.DocumentSection newSection = new DAL.DocumentSection();

                            ctx.ExecuteCommand("UPDATE DocumentSection SET Position=Position + 1 WHERE DocumentID={0}  AND Position>{1}", DocumentID.ToString(), (iPosition - 1).ToString());
                            
                            newSection.DocumentID = DocumentID;
                            newSection.DocumentSectionTypeID = 10; //Table Table
                            
                            newSection.Position = iPosition;
                            newSection.DateAdded = DateTime.Now;
                            newSection.DateUpdated = DateTime.Now;

                            RecordTableSectionDetail rtSec = new RecordTableSectionDetail();

                            rtSec.TableID = int.Parse(ddlTable.SelectedValue);
                            //rtSec.SearchCriteriaID = rlOne.SearchCriteriaID;
                            rtSec.ViewID = rlOne.ViewID;
                            newSection.Details = rtSec.GetJSONString(); 
                         
                            ctx.DocumentSections.InsertOnSubmit(newSection);
                            if (ValidParams)
                            {
                                ctx.SubmitChanges();
                                NewSectionID = newSection.DocumentSectionID;
                                ID = NewSectionID;
                                hfRemoveSection.Value = "0";
                            }
                            else
                            {
                                ErrorMessage.Text = "Invalid parameter";
                                return;
                            }

                        }

                    }
                    else
                    {
                        

                        ID = DocumentSectionID;



                        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                        {
                            DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == ID);
                            bool ValidParams = true;
                            if (section != null)
                            {


                                RecordTableSectionDetail rtSec = new RecordTableSectionDetail();

                                rtSec.TableID = int.Parse(ddlTable.SelectedValue);
                                //rtSec.SearchCriteriaID = rlOne.SearchCriteriaID;
                                rtSec.ViewID = rlOne.ViewID;

                                section.Details = rtSec.GetJSONString();
                               


                            }


                            if (ValidParams)
                            {
                                ctx.SubmitChanges();
                                hfRemoveSection.Value = "0";
                            }
                            else
                            {
                                ErrorMessage.Text = "Invalid parameter";
                            }
                        }


                    }

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + ID.ToString() + ");", true); 

                   
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
        }

      

        protected void PopulateTableDDL()
        {
            int iTN = 0;
            ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                    null,
                    null,
                    int.Parse(Session["AccountID"].ToString()),
                    null, null, true,
                    "st.TableName", "ASC",
                    null, null, ref  iTN, Session["STs"].ToString());

            ddlTable.DataBind();
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
            ddlTable.Items.Insert(0, liSelect);
        }
   

       



    }
}