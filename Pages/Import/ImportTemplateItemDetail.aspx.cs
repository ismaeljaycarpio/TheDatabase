using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Pages_Import_ImportTemplateItemDetail : SecurePage
{

    int? _iImportTemplateID = null;
    //int? _iImportTemplateItemID = null;
    ImportTemplate _theImportTemplate = null;


    protected void PopulateTerminology()
    {

        //stgParentFieldCap.InnerText = stgParentFieldCap.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        
        _iImportTemplateID=int.Parse( Cryptography.Decrypt(Request.QueryString["ImportTemplateID"].ToString()));

        _theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)_iImportTemplateID);

        //if (Request.QueryString["ImportTemplateItemID"] != null)
        //    _iImportTemplateItemID = int.Parse(Cryptography.Decrypt(Request.QueryString["ImportTemplateItemID"].ToString()));


        if (!IsPostBack)
        {
          
            //PopulateColumns();
            PopulateListBoxes();
            
            //lblDetailTitle.Text = _theImportTemplate.ImportTemplateName + " ImportTemplate Items";
          
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
            Common.ExecuteText("DELETE ImportTemplateItem WHERE ImportTemplateID=" + _iImportTemplateID.ToString() + " AND ColumnID=" + li.Value);
        }

        //add new items

        foreach (ListItem li in lstUsed.Items)
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT ImportTemplateItemID FROM ImportTemplateItem WHERE ImportTemplateID=" + _iImportTemplateID.ToString() + " AND ColumnID=" + li.Value);

            if (dtTemp.Rows.Count == 0)
            {
                ImportTemplateItem newImportTemplateItem = new ImportTemplateItem(null, _iImportTemplateID, int.Parse(li.Value),
              li.Text,null);

                string strMaxColumnIndex = Common.GetValueFromSQL("SELECT MAX(ColumnIndex) FROM ImportTemplateItem WHERE ImportTemplateID=" + _iImportTemplateID.ToString());

                if (strMaxColumnIndex == "")
                {
                    strMaxColumnIndex = "-1";
                }

                newImportTemplateItem.ColumnIndex = int.Parse(strMaxColumnIndex) + 1;

                ImportManager.dbg_ImportTemplateItem_Insert(newImportTemplateItem);
            }
        }


        ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);

    }



   


    protected void PopulateListBoxes()
    {
        DataTable dtNotUsed = Common.DataTableFromText(@"SELECT ColumnID,NameOnImport,Systemname FROM [Column] WHERE 
            Systemname not in('IsActive','TableID')  AND NameOnImport IS NOT NULL AND LEN(NameOnImport) > 0  
            AND ColumnType NOT IN ('staticcontent') AND TableID=" + _theImportTemplate.TableID.ToString()
            + @" AND ColumnID NOT IN(SELECT ColumnID FROM ImportTemplateItem WHERE ImportTemplateID=" + _theImportTemplate.ImportTemplateID.ToString() + @") ORDER BY NameOnImport ASC");


        lstNotUsed.Items.Clear();

        foreach (DataRow dr in dtNotUsed.Rows)
        {
            ListItem liTemp = new ListItem(dr["NameOnImport"].ToString(), dr["ColumnID"].ToString());
            lstNotUsed.Items.Add(liTemp);
        }


        DataTable dtUsed = Common.DataTableFromText(@"SELECT ColumnID,NameOnImport,Systemname FROM [Column] WHERE 
            Systemname not in('IsActive','TableID')   AND NameOnImport IS NOT NULL AND LEN(NameOnImport) > 0  
            AND ColumnType NOT IN ('staticcontent') AND TableID=" + _theImportTemplate.TableID.ToString()
    + @" AND ColumnID IN(SELECT ColumnID FROM ImportTemplateItem WHERE ImportTemplateID=" + _theImportTemplate.ImportTemplateID.ToString() + @") ORDER BY NameOnImport ASC");

        lstUsed.Items.Clear();

        foreach (DataRow dr in dtUsed.Rows)
        {
            ListItem liTemp = new ListItem(dr["NameOnImport"].ToString(), dr["ColumnID"].ToString());
            lstUsed.Items.Add(liTemp);
        }



    }

//    protected void PopulateColumns()
//    {
//        ddlColumn.Items.Clear();


//        ddlColumn.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName,Systemname FROM [Column] WHERE 
//                Systemname not in('IsActive','EnteredBy','TableID') 
//                AND ColumnType NOT IN ('staticcontent') AND TableID=" + _theImportTemplate.TableID.ToString());
        
//        ddlColumn.DataBind();

//        ListItem liSelect = new ListItem("--Please Select--", "");
//        ddlColumn.Items.Insert(0, liSelect);
//    }


}