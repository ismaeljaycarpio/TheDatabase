using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;
using DocGen.Utility;

namespace DocGen.Document.HTMLSection
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
            if (!IsPostBack)
            {

                fckeTemplate.AssetManager = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                    //var dms = from sp in ctx.DataMaps
                    //          orderby sp.DisplayText
                    //          select sp;

                    //foreach (DataMap dm in dms)
                    //{
                    //    ddlStoredProcedure.Items.Add(new ListItem(dm.DisplayText, dm.StoredProcedureName));
                    //}

                    DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                    if (section != null)
                    {
                        CheckPermission(section.DocumentID);
                     
                        //fckeTemplate.Value = section.Content;
                        fckeTemplate.Text = section.Content;
                        HTMLSectionFilter datasource = JSONField.GetTypedObject<HTMLSectionFilter>(section.Filter);
                        //if (datasource != null)
                        //{
                        //    pnSPConfig.Visible = true;
                        //    ddlStoredProcedure.SelectedValue = datasource.SPName;
                        //    gvParams.DataSource = datasource.Params;
                        //    gvParams.DataBind();

                        //    DAL.DataMap dmap = ctx.DataMaps.SingleOrDefault<DAL.DataMap>(d => d.StoredProcedureName == datasource.SPName);
                        //    ddlReturnFields.DataSource = dmap.ReturnFields.Split(',');
                        //    ddlReturnFields.DataBind();
                        //}
                        //CancelButton.CommandArgument = section.DocumentID.ToString();
                    }
                    else
                    {

                        if (Request.QueryString["PrevID"].ToString() != "-1")
                        {
                            hfRemoveSection.Value = "1";
                        }

                    }
                }
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

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int ID = DocumentSectionID;
                //bool ValidParams = true;
                int iPosition = 1;
                //if (Request.QueryString["Position"] != null)
                //{
                //    iPosition = int.Parse(Convert.ToString(Request.QueryString["Position"])) + 1;
                //}

                ErrorMessage.Text = "";
                try
                {
                   
                    if (Request.QueryString["PrevID"].ToString() != "-1")
                    {
                        int DocumentID = 0;
                        Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);
                      
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
                            newSection.DocumentSectionTypeID = 1; //HTML

                            newSection.Content = fckeTemplate.Text;
                            newSection.Position = iPosition;
                            newSection.DateAdded = DateTime.Now;
                            newSection.DateUpdated = DateTime.Now;
                            ctx.DocumentSections.InsertOnSubmit(newSection);
                            ctx.SubmitChanges();
                            NewSectionID = newSection.DocumentSectionID;
                            ID = NewSectionID;
                            hfRemoveSection.Value = "0";
                        }
                    }
                    else
                    {
                        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                        {
                            DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == ID);
                            if (section != null)
                            {
                               
                                section.Content = fckeTemplate.Text;
                               
                            }
                            ctx.SubmitChanges();
                            hfRemoveSection.Value = "0";
                           
                        }
                    }

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Refresh", "SavedAndRefresh();", true);

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + ID.ToString() + ");", true); 


                    
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
        }

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("../Edit.aspx?DocumentID=" + CancelButton.CommandArgument);
        //}

        //protected void ddlStoredProcedure_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string SPName = ddlStoredProcedure.SelectedValue;
        //    List<SPInputParam> lstParams = null;
        //    if (SPName != "")
        //    {
        //        lstParams = DBUtil.GetSPInputParams(SPName);
        //        using (DAL.DocGenDataContext ctx = new DocGenDataContext())
        //        {
        //            DAL.DataMap dmap = ctx.DataMaps.SingleOrDefault<DAL.DataMap>(d => d.StoredProcedureName == SPName);
        //            ddlReturnFields.DataSource = dmap.ReturnFields.Split(',');
        //            ddlReturnFields.DataBind();
        //        }
        //        gvParams.DataSource = lstParams;
        //        gvParams.DataBind();
        //        pnSPConfig.Visible = true;
        //    }
        //    else
        //    {
        //        pnSPConfig.Visible = false;
        //    }
        //    gvTest.DataSource = null;
        //    gvTest.DataBind();
        //    gvTest.Visible = false;
        //}

        //protected List<SPInputParam> GetParamValues(out bool Success)
        //{
        //    Success = true;
        //    List<SPInputParam> lstParams = new List<SPInputParam>();

        //    foreach (GridViewRow r in gvParams.Rows)
        //    {
        //        Label lblError = (Label)r.FindControl("lblError");
        //        lblError.Visible = false;
        //        try
        //        {
        //            TextBox txtValue = (TextBox)r.FindControl("txtValue");
        //            SPInputParam p = new SPInputParam();
        //            p.Name = r.Cells[0].Text;
        //            p.DataType = txtValue.CssClass.Split('_')[1];
        //            switch (p.DataType)
        //            {
        //                //Exact Numerics
        //                case "bigint":
        //                    p.Value = Convert.ToInt64(txtValue.Text);
        //                    break;
        //                case "bit":
        //                    switch (txtValue.Text.ToLower())
        //                    {
        //                        case "yes":
        //                        case "true":
        //                        case "male":
        //                        case "1":
        //                        case "y":
        //                            p.Value = 1;
        //                            break;
        //                        default:
        //                            p.Value = 0;
        //                            break;
        //                    }
        //                    break;
        //                case "decimal":
        //                case "money":
        //                case "numeric":
        //                case "smallmoney":
        //                    p.Value = Convert.ToDecimal(txtValue.Text);
        //                    break;
        //                case "int":
        //                    p.Value = Convert.ToInt32(txtValue.Text);
        //                    break;
        //                case "smallint":
        //                    p.Value = Convert.ToInt16(txtValue.Text);
        //                    break;
        //                case "tinyint":
        //                    p.Value = Convert.ToByte(txtValue.Text);
        //                    break;

        //                //Approximate Numerics
        //                case "float":
        //                    p.Value = Convert.ToDouble(txtValue.Text);
        //                    break;
        //                case "real":
        //                    p.Value = Convert.ToSingle(txtValue.Text);
        //                    break;

        //                //Date and Time
        //                case "date":
        //                case "datetime":
        //                    p.Value = ConvertUtil.GetDate(txtValue.Text);
        //                    break;

        //                //Character Strings
        //                case "char":
        //                case "varchar":
        //                case "nchar":
        //                case "nvarchar":
        //                case "text":
        //                case "ntext":
        //                    p.Value = txtValue.Text;
        //                    break;

        //            }
        //            lstParams.Add(p);
        //        }
        //        catch
        //        {
        //            lblError.Visible = true;
        //            Success = false;
        //        }
        //    }

        //    return lstParams;
        //}

        //protected void lbTest_Click(object sender, EventArgs e)
        //{            
        //    ErrorMessage.Text = "";
        //    string SPName = ddlStoredProcedure.SelectedValue;
        //    string Message = "";
        //    bool ValidParams = true;
        //    gvTest.Visible = true;
        //    List<SPInputParam> spParams = GetParamValues(out ValidParams);
        //    if (ValidParams)
        //    {
        //        gvTest.DataSource = DBUtil.ExecuteSP(SPName, spParams, out Message);
        //        gvTest.DataBind();
        //        gvTest.Visible = true;
        //        lblTest.Visible = false;
        //        lblTest.Text = "";
        //        if (Message != "")
        //        {
        //            ErrorMessage.Text = Message;
        //        }
        //    }
        //    else
        //    {
        //        lblTest.Text = "<table class=\"listingTable\"><tr><th>Invalid parameter</th></tr></table>";
        //        gvTest.Visible = false;
        //        lblTest.Visible = true;
        //    }
        //}

        //protected void gvParams_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItemIndex >= 0)
        //    {
        //        SPInputParam p = (SPInputParam)e.Row.DataItem;
        //        TextBox txtValue = (TextBox)e.Row.FindControl("txtValue");
        //        txtValue.CssClass = "Param_" + p.DataType;
        //        if (p.MaxCharLength > 0)
        //        {
        //            txtValue.MaxLength = p.MaxCharLength;
        //        }
        //    }
        //}

        //protected void ParamsValidator_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    bool ValidParams = true;
        //    GetParamValues(out ValidParams);
        //    args.IsValid = ValidParams;
        //}
    }
}