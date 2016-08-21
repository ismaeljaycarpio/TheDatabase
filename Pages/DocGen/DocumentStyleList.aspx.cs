using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;

namespace DocGen.DocumentTextStyle
{
    public partial class List : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowList();

                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/EditReport.aspx?mode=" + Request.QueryString["rmode"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" +  Request.QueryString["DocumentID"].ToString();
                lnkNew.NavigateUrl = "DocumentStypeEdit.aspx?rmode=" + Request.QueryString["rmode"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" + Request.QueryString["DocumentID"].ToString() ;

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
            using (DocGenDataContext ctx = new DocGenDataContext())
            {
                gvDocumentTextStyle.DataSource = from t in ctx.DocumentSectionStyles
                                                 where t.AccountID == this.AccountID
                                                 orderby t.StyleName
                                                 select t;
                gvDocumentTextStyle.DataBind();
            }
        }



        protected void gvDocumentTextStyle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (DataBinder.Eval(e.Row.DataItem, "IsSystem").ToString() == "True")
                {
                    LinkButton lbDelete = (LinkButton)e.Row.FindControl("lbDelete");
                    if (lbDelete != null)
                    {
                        lbDelete.Visible = false;
                    }
                }
                else
                {
                   //
                }

                HyperLink hlEditRecord = (HyperLink)e.Row.FindControl("hlEditRecord");

                if (hlEditRecord != null)
                {
                    hlEditRecord.NavigateUrl = "DocumentStypeEdit.aspx?rmode=" + Request.QueryString["rmode"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" +  Request.QueryString["DocumentID"].ToString() + "&DocumentSectionStyleID=" + DataBinder.Eval(e.Row.DataItem, "DocumentSectionStyleID").ToString();
                }

                HyperLink hlEditRecord2 = (HyperLink)e.Row.FindControl("hlEditRecord2");

                if (hlEditRecord2 != null)
                {
                    hlEditRecord2.NavigateUrl = "DocumentStypeEdit.aspx?rmode=" + Request.QueryString["rmode"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" + Request.QueryString["DocumentID"].ToString() + "&DocumentSectionStyleID=" + DataBinder.Eval(e.Row.DataItem, "DocumentSectionStyleID").ToString();
                }

            }
        }



        protected void gvDocumentTextStyle_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // No need to implement code here

        }


        protected void gvDocumentTextStyle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int ID = Convert.ToInt32(e.CommandArgument);

                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                    DAL.DocumentSectionStyle textStyle = ctx.DocumentSectionStyles.SingleOrDefault<DAL.DocumentSectionStyle>(d => d.DocumentSectionStyleID == ID);
                    if (textStyle != null)
                    {
                        if ((bool)textStyle.IsSystem)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Can not delete System Style');", true);
                        }
                        else
                        {
                            ctx.DocumentSectionStyles.DeleteOnSubmit(textStyle);
                            ctx.SubmitChanges();
                            ShowList();
                        }
                    }

                }
            }
        }


    }
}