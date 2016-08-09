using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Pages_Export_ExportTemplateItemDetail : SecurePage
{

    int? _iExportTemplateID = null;
    //int? _iExportTemplateItemID = null;
    ExportTemplate _theExportTemplate = null;


    protected void PopulateTerminology()
    {

        //stgParentFieldCap.InnerText = stgParentFieldCap.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        
        _iExportTemplateID=int.Parse( Cryptography.Decrypt(Request.QueryString["ExportTemplateID"].ToString()));

        _theExportTemplate = ExportManager.dbg_ExportTemplate_Detail((int)_iExportTemplateID);

        //if (Request.QueryString["ExportTemplateItemID"] != null)
        //    _iExportTemplateItemID = int.Parse(Cryptography.Decrypt(Request.QueryString["ExportTemplateItemID"].ToString()));


        if (!IsPostBack)
        {
          
            //PopulateColumns();
            PopulateListBoxes();


            //lblDetailTitle.Text = _theExportTemplate.ExportTemplateName + " ExportTemplate Items";
          

            PopulateTerminology();
        }


    }


  


    protected void lnkRemove_Click(object sender, EventArgs e)
    {
        if (lstUsed.SelectedItem != null)
        {
            for (int i = lstUsed.Items.Count - 1; i >= 0; --i)
            {
                if (lstUsed.Items[i].Selected)
                {
                    lstNotUsed.Items.Add(new ListItem(lstUsed.Items[i].Text, lstUsed.Items[i].Value));

                    lstUsed.Items.RemoveAt(i);
                }
            }

        }
        else
        {
            lstNotUsed.Focus();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RemoveMessage", "alert('Please select a column from right side list.');", true);
        }


    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        

        if (lstNotUsed.SelectedItem != null)
        {
            for (int i = lstNotUsed.Items.Count - 1; i >= 0; --i)
            {
                if (lstNotUsed.Items[i].Selected)
                {
                    lstUsed.Items.Add(new ListItem(lstNotUsed.Items[i].Text, lstNotUsed.Items[i].Value));

                    lstNotUsed.Items.RemoveAt(i);
                }
            }

        }
        else
        {
            lstUsed.Focus();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RemoveMessage", "alert('Please select a column from left side list.');", true);
        }



    }


    protected void lnkSaveNew_Click(object sender, EventArgs e)
    {

        //DELETE Removed item.

        foreach (ListItem li in lstNotUsed.Items)
        {
            Common.ExecuteText("DELETE ExportTemplateItem WHERE ExportTemplateID=" + _iExportTemplateID.ToString() + " AND ColumnID=" + li.Value);
        }

        //add new items

        foreach (ListItem li in lstUsed.Items)
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT ExportTemplateItemID FROM ExportTemplateItem WHERE ExportTemplateID=" + _iExportTemplateID.ToString() + " AND ColumnID=" + li.Value);

            if (dtTemp.Rows.Count == 0)
            {
                ExportTemplateItem newExportTemplateItem = new ExportTemplateItem(null, _iExportTemplateID, int.Parse(li.Value),
              li.Text,null);

                string strMaxColumnIndex = Common.GetValueFromSQL("SELECT MAX(ColumnIndex) FROM ExportTemplateItem WHERE ExportTemplateID=" + _iExportTemplateID.ToString());

                if (strMaxColumnIndex == "")
                {
                    strMaxColumnIndex = "-1";
                }

                newExportTemplateItem.ColumnIndex = int.Parse(strMaxColumnIndex) + 1;

                ExportManager.dbg_ExportTemplateItem_Insert(newExportTemplateItem);
            }
        }


        ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);

    }



   


    protected void PopulateListBoxes()
    {
        DataTable dtNotUsed = Common.DataTableFromText(@"SELECT ColumnID,NameOnExport,Systemname FROM [Column] WHERE 
            Systemname not in('IsActive','TableID')  AND NameOnExport IS NOT NULL AND LEN(NameOnExport) > 0  
            AND ColumnType NOT IN ('staticcontent') AND TableID=" + _theExportTemplate.TableID.ToString()
            + @" AND ColumnID NOT IN(SELECT ColumnID FROM ExportTemplateItem WHERE ExportTemplateID=" + _theExportTemplate.ExportTemplateID.ToString() + @") ORDER BY NameOnExport ASC");


        lstNotUsed.Items.Clear();

        foreach (DataRow dr in dtNotUsed.Rows)
        {
            ListItem liTemp = new ListItem(dr["NameOnExport"].ToString(), dr["ColumnID"].ToString());
            lstNotUsed.Items.Add(liTemp);
        }


        DataTable dtUsed = Common.DataTableFromText(@"SELECT ColumnID,NameOnExport,Systemname FROM [Column] WHERE 
            Systemname not in('IsActive','TableID')   AND NameOnExport IS NOT NULL AND LEN(NameOnExport) > 0  
            AND ColumnType NOT IN ('staticcontent') AND TableID=" + _theExportTemplate.TableID.ToString()
    + @" AND ColumnID IN(SELECT ColumnID FROM ExportTemplateItem WHERE ExportTemplateID=" + _theExportTemplate.ExportTemplateID.ToString() + @") ORDER BY NameOnExport ASC");

        lstUsed.Items.Clear();

        foreach (DataRow dr in dtUsed.Rows)
        {
            ListItem liTemp = new ListItem(dr["NameOnExport"].ToString(), dr["ColumnID"].ToString());
            lstUsed.Items.Add(liTemp);
        }



    }

//    protected void PopulateColumns()
//    {
//        ddlColumn.Items.Clear();


//        ddlColumn.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName,Systemname FROM [Column] WHERE 
//                Systemname not in('IsActive','EnteredBy','TableID') 
//                AND ColumnType NOT IN ('staticcontent') AND TableID=" + _theExportTemplate.TableID.ToString());
        
//        ddlColumn.DataBind();

//        ListItem liSelect = new ListItem("--Please Select--", "");
//        ddlColumn.Items.Insert(0, liSelect);
//    }


}