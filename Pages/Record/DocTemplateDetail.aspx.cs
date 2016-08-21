using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Pages_Record_DocTemplateDetail : SecurePage
{

    int? _iTableID = null;
    int? _iDocTemplateID = null;
    protected void Page_Load(object sender, EventArgs e)
    {

       

        _iTableID=int.Parse(Request.QueryString["TableID"].ToString());


        if (Request.QueryString["DocTemplateID"] != null)
            _iDocTemplateID = int.Parse(Request.QueryString["DocTemplateID"].ToString());


        if (!IsPostBack)
        {
            PopulateDataRetriever();
            if (_iDocTemplateID != null)
            {
                PopulateRecord();
                lblDetailTitle.Text = "Edit Template";
            }
            else
            {
                //oliver <begin> ticket 1451
                AutoSelectDataRetriever();
                //oliver <end>

                lblDetailTitle.Text = "Add Template";              
            }
        }


    }

    //oliver <begin> Ticket 1451
    protected void AutoSelectDataRetriever()
    {
        ddlDataRetriever.SelectedValue = Common.GetValueFromSQL(@"SELECT DataRetrieverID FROM DataRetriever WHERE TableID=" + _iTableID.ToString());
        ddlDataRetriever_SelectedIndexChanged(null, null);
    }
    //oliver <end>

    protected void PopulateRecord()
    {
        DocTemplate theDocTemplate = DocumentManager.dbg_DocTemplate_Detail((int)_iDocTemplateID);
        if (theDocTemplate != null)
        {
            lblWordDocument.Text = "<a href='" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/DocTemplates/" + theDocTemplate.FileUniqueName + "' target='_blank' >" + theDocTemplate.FileName.ToString() + " </a>";
            ddlDataRetriever.Text = theDocTemplate.DataRetrieverID.ToString();
           
            ViewState["theDocTemplate"] = theDocTemplate;
            
        }

        ddlDataRetriever_SelectedIndexChanged(null, null);
       
    }

    protected void PopulateDataRetriever()
    {

        ddlDataRetriever.DataSource = Common.DataTableFromText(@"SELECT * FROM DataRetriever WHERE TableID=" + _iTableID.ToString());

        ddlDataRetriever.DataBind();

        System.Web.UI.WebControls.ListItem liPlease = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddlDataRetriever.Items.Insert(0, liPlease);

    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if (fuWordDocument.HasFile)
        {

            string strFileExtension = fuWordDocument.FileName.Substring(fuWordDocument.FileName.LastIndexOf('.') + 1).ToLower();

            if (strFileExtension.ToLower() == "docx" || strFileExtension.ToLower() == "dotx")
            {

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a .docx or .dotx (Word 2007 or later).');", true);
                return;
            }
        }


        if (ViewState["theDocTemplate"] == null)
        {
            //add

            if (fuWordDocument.HasFile == false)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a file.');", true);
                return;
            }
            if (ddlDataRetriever.SelectedValue == "")
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a Data Retriever.');", true);
                return;
            }

            if (ddlDataRetriever.SelectedValue != ""
                && fuWordDocument.HasFile)
            {
                string strFolder = "DocTemplates";
                string strFileName = fuWordDocument.FileName;
                string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;// +"." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                string strPath = Server.MapPath(strFolder + "\\" + strUniqueName);

                fuWordDocument.SaveAs(strPath);

                DocTemplate newDocTemplate = new DocTemplate(null, strUniqueName, strFileName, int.Parse(ddlDataRetriever.SelectedValue));
                
                DocumentManager.dbg_DocTemplate_Insert(newDocTemplate);
            }
            
        }
        else
        {
            //edit
            DocTemplate editDocTemplate = (DocTemplate)ViewState["theDocTemplate"];

            if (fuWordDocument.HasFile)
            {
                string strFolder = "DocTemplates";
                string strFileName = fuWordDocument.FileName;
                string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;// +"." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                string strPath = Server.MapPath(strFolder + "\\" + strUniqueName);

                fuWordDocument.SaveAs(strPath);
                
                editDocTemplate.FileUniqueName = strUniqueName;
                editDocTemplate.FileName = strFileName;                

            }

            editDocTemplate.DataRetrieverID = int.Parse(ddlDataRetriever.SelectedValue);

            DocumentManager.dbg_DocTemplate_Update(editDocTemplate);
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshTemplates", "CloseAndRefresh();", true);


    }



    protected void ddlDataRetriever_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblTokens.Text="";

        if (ddlDataRetriever.SelectedValue == "")
        {
            lblTokens.Text = "";
            return;
        }

        try
        {
            if (ddlDataRetriever.SelectedValue != "")
            {
                DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail(int.Parse(ddlDataRetriever.SelectedValue));

                DataTable dtDR = DocumentManager.DataRetrieverSP(_iTableID, null, theDataRetriever.SPName);

                if (dtDR != null)
                {
                    foreach (DataColumn dc in dtDR.Columns)
                    {
                        //oliver <begin> Ticket 1451
                        lblTokens.Text = lblTokens.Text + "«" + dc.ColumnName + "»<br/>";
                        //oliver <end>

                        //lblTokens.Text = lblTokens.Text + "[" + dc.ColumnName + "]<br/>";
                    }
                }

            }
        }
        catch(Exception ex)
        {
            if (ex.Message.IndexOf("not find stored procedure") > -1)
            {
                lblTokens.Text = "Coluld not find Data Retriever for " + ddlDataRetriever.SelectedItem.Text + ". Please contact with Admin.";
            }

        }
    }
}