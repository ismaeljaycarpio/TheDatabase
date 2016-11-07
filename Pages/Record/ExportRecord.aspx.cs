using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
public partial class Pages_Record_ExportRecord : SecurePage
{

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            Session["ExportClass"] = null;
            
                string strParentObject = "";
                if (Request.QueryString["sc_id"] != null)
                {
                    SearchCriteria theSC = SystemData.SearchCriteria_Detail(int.Parse(Request.QueryString["sc_id"].ToString()));
                    if (theSC != null)
                    {
                        System.Xml.XmlDocument xmlSC_Doc = new System.Xml.XmlDocument();

                        xmlSC_Doc.Load(new StringReader(theSC.SearchText));

                        strParentObject = strParentObject + " var p_lnkExportRecords = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["lnkExportRecords"].InnerText + @"');";
                        //strParentObject = strParentObject + " var p_chkAll = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["chkAll"].InnerText + @"');";
                        ViewState["strParentObject"] = strParentObject;
                        
                        ViewState["TableID"] = xmlSC_Doc.FirstChild["TableID"].InnerText;
                        ViewState["ViewID"] = xmlSC_Doc.FirstChild["ViewID"].InnerText;
                        ViewState["TableName"] = xmlSC_Doc.FirstChild["TableName"].InnerText;
                        lblTitle.Text = ViewState["TableName"].ToString() + " - Export Records";
                        ViewState["_strRecordRightID"] = xmlSC_Doc.FirstChild["RecordRightID"].InnerText;
                        ViewState["_strDynamictabPart"] = xmlSC_Doc.FirstChild["DynamictabPart"].InnerText;                        
                    }
                }


                PopulateExportTemplate(int.Parse(ViewState["TableID"].ToString()));
                ddlTemplate_SelectedIndexChanged(null, null);

                if (ddlTemplate.Items.Count == 0)
                {
                    //no template, so lets create one

                    ExportManager.CreateDefaultExportTemplate(int.Parse(ViewState["TableID"].ToString()));
                    PopulateExportTemplate(int.Parse(ViewState["TableID"].ToString()));

                    ddlTemplate_SelectedIndexChanged(null, null);


                }

                hlExportTemplateNew.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?mode=" + Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(ViewState["TableID"].ToString()) + "&SearchCriteriaET=" + Cryptography.Encrypt("-1") + "&fixedbackurl=" + Cryptography.Encrypt(Request.RawUrl);
                //hlExportTemplate.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString()) +"&SearchCriteriaET=" + Cryptography.Encrypt("-1");
            

        }

    }


    protected void lnkExportRecords_Click(object sender, EventArgs e)
    {
        try
        {
            List<IDnText> lstValueText = new List<IDnText>();

            foreach(ListItem item in chklstFields.Items)
            {
                if(item.Selected)
                {
                    IDnText aIDnText = new IDnText(item.Value, item.Text);
                    lstValueText.Add(aIDnText);
                }
            }

            ExportClass aExportClass = new ExportClass();
            aExportClass.strRecords = rdbRecords.SelectedValue;
            aExportClass.strExportFiletype = ddlExportFiletype.SelectedValue;
            aExportClass.objCheckBoxList= lstValueText;
            Session["ExportClass"] = aExportClass;
            //CheckBoxList
            string strXX = @"
                 
                                " + ViewState["strParentObject"].ToString() + @"
                                    parent.$.fancybox.close();
                                     $(p_lnkExportRecords).trigger('click');

                   
                    ";

            ScriptManager.RegisterStartupScript(upMain, upMain.GetType(), "doneExport" + ViewState["_strDynamictabPart"].ToString(), strXX, true);
        }
        catch (Exception ex)
        {
            //
            if (ex.Message.IndexOf("Thread was being aborted") == -1)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Record export", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

        }

        //mpeExportRecords.Hide();
    }
    protected void rdbRecords_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (rdbRecords.SelectedValue == "d")
        {
            ddlExportFiletype.SelectedValue = "c";
            ddlExportFiletype.Enabled = false;
            ddlTemplate.Enabled = false;
            chklstFields.Enabled = true;
            hlExportTemplate.Visible = false;
            hlExportTemplateNew.Visible = false;

            chklstFields.Enabled = true;
            chklstFields.Items.Clear();
            DataTable dtColumns = TheDatabase.spBulkExportColumns(int.Parse(ViewState["TableID"].ToString()));

            if (dtColumns != null)
            {
                foreach (DataRow dr in dtColumns.Rows)
                {
                    string strColumnText = dr["TableName"].ToString() + " " + dr["ColumnDisplayName"].ToString();

                    ListItem liTemp = new ListItem(strColumnText, dr["ColumnID"].ToString());
                    liTemp.Selected = true;
                    //liTemp.Attributes.Add("DataValue", dr["SystemName"].ToString());
                    chklstFields.Items.Add(liTemp);
                }
            }
        }
        else
        {
            if (hlExportTemplate.Visible == false)
                ddlTemplate_SelectedIndexChanged(null, null);


            ddlExportFiletype.Enabled = true;
            ddlTemplate.Enabled = true;
            chklstFields.Enabled = true;
            hlExportTemplate.Visible = true;
            hlExportTemplateNew.Visible = true;
        }
        //mpeExportRecords.Show();
    }


    protected void PopulateExportTemplate(int iTableID)
    {
        ddlTemplate.Items.Clear();
        ddlTemplate.DataSource = Common.DataTableFromText("SELECT * FROM ExportTemplate WHERE TableID=" + iTableID + " ORDER BY ExportTemplateName");
        ddlTemplate.DataBind();

        //ListItem liSelect = new ListItem("--Please select--", "");
        //ddlTemplate.Items.Insert(0, liSelect);

    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {

        chklstFields.Items.Clear();

        if (ddlTemplate.SelectedValue == "")
        {
            //[Column]
            DataTable dtColumns = Common.DataTableFromText(@" SELECT ColumnID AS FieldID,DisplayName AS Heading,SystemName 
                            FROM [Column] WHERE TableID=" + ViewState["TableID"].ToString() + @" and Systemname not in('IsActive','TableID') AND DisplayName IS NOT NULL AND LEN(DisplayName) > 0
            AND ColumnType NOT IN ('staticcontent') ORDER BY DisplayRight,DisplayOrder");
            //chklstFields.DataBind();

            foreach (DataRow dr in dtColumns.Rows)
            {
                ListItem liTemp = new ListItem(dr["Heading"].ToString(), dr["FieldID"].ToString());
                liTemp.Selected = true;
                //liTemp.Attributes.Add("DataValue", dr["SystemName"].ToString());
                chklstFields.Items.Add(liTemp);
            }
            hlExportTemplate.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(ViewState["TableID"].ToString()) + "&SearchCriteriaET=" + Cryptography.Encrypt("-1");

        }
        else
        {
            //[ExportTemplateItem]
            DataTable dtColumns = Common.DataTableFromText(@" SELECT ColumnID AS FieldID,ExportHeaderName AS Heading  FROM 
                    ExportTemplateItem WHERE ExportTemplateID=" + ddlTemplate.SelectedValue + " ORDER BY ColumnIndex");

            foreach (DataRow dr in dtColumns.Rows)
            {
                ListItem liTemp = new ListItem(dr["Heading"].ToString(), dr["FieldID"].ToString());
                liTemp.Selected = true;
                //liTemp.Attributes.Add("DataValue", dr["SystemName"].ToString());
                chklstFields.Items.Add(liTemp);
            }
            hlExportTemplate.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Export/ExportTemplateItem.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(ViewState["TableID"].ToString()) + "&SearchCriteriaET=" + Cryptography.Encrypt("-1") + "&ExportTemplateID=" + Cryptography.Encrypt(ddlTemplate.SelectedValue) + "&fixedbackurl=" + Cryptography.Encrypt(Request.RawUrl);

        }

        //if (IsPostBack)
        //    mpeExportRecords.Show();
    }
    protected void lnkUntickAllExport_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in chklstFields.Items)
        {
            item.Selected = false;
        }

    }

}