using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Graph_TableGraphConfig : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int tableID = 0;

        if (!IsPostBack)
        {
            if ((Request.QueryString["TableID"] != null) && int.TryParse(Cryptography.Decrypt(Request.QueryString["TableID"]), out tableID))
            {
                PopulateGraphXAxisColumnDDL(tableID);
                PopulateGraphSeriesColumnDDL(tableID);

                Table editTable = RecordManager.ets_Table_Details(tableID);
                if (editTable != null)
                {
                    if (editTable.GraphXAxisColumnID.HasValue)
                        ddlGraphXAxisColumnID.SelectedValue = editTable.GraphXAxisColumnID.ToString();
                    if (editTable.GraphSeriesColumnID.HasValue)
                        ddlGraphSeriesColumnID.SelectedValue = editTable.GraphSeriesColumnID.ToString();
                    if (editTable.GraphDefaultPeriod.HasValue)
                        ddlDefaultGraphPeriod.SelectedValue = editTable.GraphDefaultPeriod.ToString();
                }
            }
        }
    }

    protected void PopulateGraphXAxisColumnDDL(int tableID)
    {
        ddlGraphXAxisColumnID.Items.Clear();
        ddlGraphXAxisColumnID.DataSource = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
            IsStandard = 0 AND TableID = " + tableID);

        ddlGraphXAxisColumnID.DataBind();
        ListItem liSeletec = new ListItem("-- Please Select --", "");

        ddlGraphXAxisColumnID.Items.Insert(0, liSeletec);
    }

    protected void PopulateGraphSeriesColumnDDL(int tableID)
    {
        ddlGraphSeriesColumnID.Items.Clear();
        ddlGraphSeriesColumnID.DataSource = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] WHERE 
            IsStandard = 0 AND TableID=" + tableID);

        ddlGraphSeriesColumnID.DataBind();
        ListItem liSeletec = new ListItem("-- Please Select --", "");

        ddlGraphSeriesColumnID.Items.Insert(0, liSeletec);
    }

    protected void LinkButtonOk_Click(object sender, EventArgs e)
    {
        int tableID = 0;

        if ((Request.QueryString["TableID"] != null) && int.TryParse(Cryptography.Decrypt(Request.QueryString["TableID"]), out tableID))
        {
            Table editTable = RecordManager.ets_Table_Details(tableID);

            if (ddlGraphXAxisColumnID.SelectedValue == "")
            {
                editTable.GraphXAxisColumnID = null;
            }
            else
            {
                editTable.GraphXAxisColumnID = int.Parse(ddlGraphXAxisColumnID.SelectedValue);
            }

            if (ddlGraphSeriesColumnID.SelectedValue == "")
            {
                editTable.GraphSeriesColumnID = null;
            }
            else
            {
                editTable.GraphSeriesColumnID = int.Parse(ddlGraphSeriesColumnID.SelectedValue);
            }

            if (ddlDefaultGraphPeriod.SelectedValue == "")
            {
                editTable.GraphDefaultPeriod = null;
            }
            else
            {
                editTable.GraphDefaultPeriod = int.Parse(ddlDefaultGraphPeriod.SelectedValue);
            }
            
            //SqlTransaction tn;
            //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
            //connection.Open();
            //tn = connection.BeginTransaction();

            try
            {
                RecordManager.ets_Table_Update(editTable);
                //tn.Commit();
            }
            catch (Exception ex)
            {
                //tn.Rollback();
                ErrorLog theErrorLog = new ErrorLog(null, "Table Graph Config", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }
            
        }
        string script = "window.parent.document.getElementById('HiddenButtonRefresh').click();OnClose();";
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "close_script", script, true);
    }
}