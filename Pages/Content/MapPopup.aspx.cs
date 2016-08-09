using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Pages_Content_MapPopup : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        edtContent.AssetManager = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";

        if (!IsPostBack)
        {
            PopulateDatabaseField();

            if (Session["Mappopup"] != null)
            {
                edtContent.Text = Session["Mappopup"].ToString();
            }
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {

        Session["Mappopup"] = edtContent.Text;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Save mappopup", "parent.$.fancybox.close();", true);
    }

    protected void PopulateDatabaseField()
    {

        int iTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
        ddlDatabaseField.Items.Clear();

        DataTable dtColumns = Common.DataTableFromText("SELECT DisplayName FROM [Column] WHERE IsStandard=0 AND TableTableID IS NULL AND   TableID=" + iTableID.ToString() + "  ORDER BY DisplayName");
        foreach (DataRow dr in dtColumns.Rows)
        {
            ListItem aItem = new ListItem(dr["DisplayName"].ToString(), dr["DisplayName"].ToString());
            ddlDatabaseField.Items.Add(aItem);
        }


        //Work with 1 top level Parent tables.
        DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + iTableID.ToString()); //AND DetailPageType<>'not'

        if (dtPT.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPT.Rows)
            {
                DataTable dtPColumns = Common.DataTableFromText(@"SELECT distinct TableName + ':' + DisplayName AS DP FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE IsStandard=0 AND TableTableID IS NULL AND  [Column].TableID=" + dr["ParentTableID"].ToString());
                foreach (DataRow drP in dtPColumns.Rows)
                {
                    ListItem aItem = new ListItem(drP["DP"].ToString(), drP["DP"].ToString());
                    ddlDatabaseField.Items.Add(aItem);
                }
            }
        }


    }
}