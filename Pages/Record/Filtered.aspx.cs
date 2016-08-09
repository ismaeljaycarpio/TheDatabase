using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Record_Filtered : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            //POPULATE Other Field
            hfTableID.Value = Request.QueryString["TableID"].ToString();
            hfColumnID.Value = Request.QueryString["ColumnID"].ToString();
            hfParentTableID.Value = Request.QueryString["ParentTableID"].ToString();


            DataTable dtParentTable = Common.DataTableFromText("SELECT ColumnID, DisplayName FROM [Column] WHERE IsStandard=0 AND TableID=" + hfParentTableID.Value + " ORDER BY DisplayName DESC");

            foreach (DataRow dr in dtParentTable.Rows)
            {
                ListItem liItem = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
                ddlFilterParentColumnID.Items.Add(liItem);
            }
            ListItem liItemPS2 = new ListItem("--Please select--", "");
            ddlFilterParentColumnID.Items.Insert(0,liItemPS2);

            DataTable dtChildTable = Common.DataTableFromText("SELECT ColumnID, DisplayName FROM [Column] WHERE IsStandard=0 AND TableID=" + hfTableID.Value + " AND ColumnID<>" + hfColumnID.Value + " ORDER BY DisplayName DESC");

            foreach (DataRow dr in dtChildTable.Rows)
            {
                ListItem liItem=new ListItem(dr["DisplayName"].ToString(),dr["ColumnID"].ToString());
                ddlFilterOtherColumnID.Items.Add(liItem);
            }
            ListItem liItemPS = new ListItem("--Please select--", "");
            ddlFilterOtherColumnID.Items.Insert(0,liItemPS);

            if (Request.QueryString["hfFilterParentColumnID"] != null && Request.QueryString["hfFilterParentColumnID"].ToString() != "")
            {
                ddlFilterParentColumnID.SelectedValue = Request.QueryString["hfFilterParentColumnID"].ToString();

                if (Request.QueryString["hfFilterOperator"] != null)
                {
                    string strFilterOperator = Request.QueryString["hfFilterOperator"].ToString().Trim() == "" ? "equals" : Request.QueryString["hfFilterOperator"].ToString();

                    if (ddlFilterOperator.Items.FindByValue(strFilterOperator) != null)
                        ddlFilterOperator.SelectedValue = strFilterOperator;
                                    
                }


                bool bHasOtherColumnID = false;
                if (Request.QueryString["hfFilterOtherColumnID"] != null && Request.QueryString["hfFilterOtherColumnID"].ToString() != "")
                {
                    ddlFilterOtherColumnID.SelectedValue = Request.QueryString["hfFilterOtherColumnID"].ToString();
                    bHasOtherColumnID = true;
                }

                if (Request.QueryString["hfFilterValue"] != null && Request.QueryString["hfFilterValue"].ToString() != "")
                {
                    txtFilterValue.Text = Request.QueryString["hfFilterValue"].ToString();
                }

                if (bHasOtherColumnID)
                {
                    txtFilterValue.Text = "";
                    trFilterValue.Visible = false;
                    trOtherField.Visible = true;
                    optFilterType.SelectedValue = "o";
                    ddlFilterOperator.SelectedValue = "equals";
                    ddlFilterOperator.Enabled = false;
                }
                else
                {
                    ddlFilterOtherColumnID.SelectedValue = "";
                    trFilterValue.Visible = true;
                    trOtherField.Visible = false;
                    optFilterType.SelectedValue = "f";
                }

            }

            PopulateTerminology();
        }
    }


    protected void PopulateTerminology()
    {

        stgField.InnerText = stgField.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        stgOtherField.InnerText = stgOtherField.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Save Action", "GetBackValue();", true);

    }

    protected void optFilterType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //do stuff here
        if (optFilterType.SelectedValue == "f")
        {
            trFilterValue.Visible = true;
            trOtherField.Visible = false;
            ddlFilterOtherColumnID.SelectedValue = "";
            ddlFilterOperator.Enabled = true;
        }
        else
        {
            trFilterValue.Visible = false;
            trOtherField.Visible = true;
            txtFilterValue.Text = "";
            ddlFilterOperator.SelectedValue = "equals";
            ddlFilterOperator.Enabled = false;
        }
    }

}