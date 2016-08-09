using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_SystemData_CopyRecordField :SecurePage
{
    string _strRecordRightID = Common.UserRoleType.None;
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Copy Field";
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

  
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if(ddlParentTable.SelectedItem!=null )
        {
            if(ddlParentTable.SelectedValue!=""
                && ddlParentjoinfield.SelectedValue != "" && ddlParentjoinfield2.SelectedValue != "")
            {

               try
               {
                   SystemData.Record_Copy_Field(int.Parse(ddlParentTable.SelectedValue), ddlParentjoinfield.Text, ddlParentjoinfield2.Text);

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
        
        DataTable dtTable = Common.DataTableFromText("SELECT TableID,TableName FROM [Table] WHERE IsActive=1 and AccountID=" + Session["AccountID"].ToString() + " ORDER BY TableName");
        
        ddlParentTable.DataSource = dtTable;
      
        ddlParentTable.DataBind();
      


        ListItem liSelect = new ListItem("--Please Select--", "");
        ddlParentTable.Items.Insert(0, liSelect);

        
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