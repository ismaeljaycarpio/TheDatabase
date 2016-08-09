using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_SystemData_UpdateLinkedTables :SecurePage
{
    string _strRecordRightID = Common.UserRoleType.None;
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Update Linked Tables";
        if(!IsPostBack)
        {
          
            if (Session["roletype"] != null)
                _strRecordRightID = Session["roletype"].ToString();           

            if (!Common.HaveAccess(_strRecordRightID, "1"))
            { Response.Redirect("~/Default.aspx", false); }

            PopulateTable();
        }



    }

    protected void ddlParentTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateColumns(ddlParentTable.SelectedValue, ref ddlParentjoinfield);
        PopulateColumns(ddlParentTable.SelectedValue, ref ddlParentjoinfield2);
    }

    protected void ddlChildTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateColumns( ddlChildTable.SelectedValue,ref ddlChildFieldtoset);

        PopulateColumns(ddlChildTable.SelectedValue, ref ddlChildjoinfield);
        PopulateColumns(ddlChildTable.SelectedValue, ref ddlChildjoinfield2);
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if(ddlParentTable.SelectedItem!=null && ddlChildTable.SelectedItem!=null)
        {
            if(ddlParentTable.SelectedValue!="" && ddlChildTable.SelectedValue!="" && ddlChildFieldtoset.SelectedValue!=""
                && ddlParentjoinfield.SelectedValue!="" && ddlChildjoinfield.SelectedValue!="")
            {

               try
               {
                   SystemData.spUpdateLinkedTables(int.Parse(ddlParentTable.SelectedValue), ddlParentjoinfield.Text, int.Parse(ddlChildTable.SelectedValue),
                       ddlChildjoinfield.Text, ddlChildFieldtoset.Text, ddlParentjoinfield2.Text, ddlChildjoinfield2.Text);

                   lblMsg.Text="";

                   ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "message_alert", "alert('Done.');", true);
               }
                catch(Exception ex)
               {
                   lblMsg.Text = ex.Message + " --->> " + ex.StackTrace;
               }

            }

        }
    }
    protected void PopulateTable()
    {
        ddlParentTable.Items.Clear();
        ddlChildTable.Items.Clear();
        
        DataTable dtTable = Common.DataTableFromText("SELECT TableID,TableName FROM [Table] WHERE IsActive=1 and AccountID=" + Session["AccountID"].ToString() + " ORDER BY TableName");
        
        ddlParentTable.DataSource = dtTable;
        ddlChildTable.DataSource = dtTable;

        ddlParentTable.DataBind();
        ddlChildTable.DataBind();


        ListItem liSelect = new ListItem("--Please Select--", "");
        ddlParentTable.Items.Insert(0, liSelect);

        ListItem liSelect2 = new ListItem("--Please Select--", "");
        ddlChildTable.Items.Insert(0, liSelect2);
    }


    protected void PopulateColumns(string strTableID,ref DropDownList ddl)
    {
        ddl.Items.Clear();

        if(strTableID!="")
        {
            DataTable dtColuns = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE TableID=" + strTableID);
            ddl.DataSource = dtColuns;
            ddl.DataBind();

            ListItem liSelect = new ListItem("--Please Select--", "");
            ddl.Items.Insert(0, liSelect);
        }

      
    }

}