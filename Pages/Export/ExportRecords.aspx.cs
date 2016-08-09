using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Export_ExportRecords : System.Web.UI.Page
{
    Table _theTable;
    protected void Page_Load(object sender, EventArgs e)
    {
        _theTable=RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt( Request.QueryString["TableID"].ToString())));


        if (!IsPostBack)
        {
            PopulateExportTemplate((int)_theTable.TableID);
            ddlTemplate_SelectedIndexChanged(null, null);
        }
    }



    protected void lnkExport_Click(object sender, EventArgs e)
    {

    }

    protected void PopulateExportTemplate(int iTableID)
    {
        ddlTemplate.DataSource = Common.DataTableFromText("SELECT * FROM ExportTemplate WHERE TableID=" + iTableID);
        ddlTemplate.DataBind();

        ListItem liSelect = new ListItem("--Please select--", "");
        ddlTemplate.Items.Insert(0, liSelect);

    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {

        chklstFields.Items.Clear();

        if (ddlTemplate.SelectedValue == "")
        {
            //[Column]
            chklstFields.DataSource = Common.DataTableFromText(@" SELECT ColumnID AS FieldID,DisplayName AS Heading 
                            FROM [Column] WHERE TableID="+_theTable.TableID.ToString()+@" ORDER BY DisplayOrder ASC");
            chklstFields.DataBind();

        }
        else
        {
            //[ExportTemplateItem]
            chklstFields.DataSource = Common.DataTableFromText(@" SELECT ExportTemplateItemID AS FieldID,ExportHeaderName AS Heading  FROM 
                    ExportTemplateItem WHERE ExportTemplateID="+ddlExportFiletype.SelectedValue+" ORDER BY ColumnIndex");
            chklstFields.DataBind();
        }

    }
}