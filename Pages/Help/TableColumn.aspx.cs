using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.CodeDom.Compiler;
using System.Data;
public partial class Page_Help_CalculationTest : SecurePage
{
    //string _strType = "calculation";
    //int _iColumnID = -1;
    int _iTableID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {

        //headername

        if (Request.QueryString["headername"] != null)
        {
            lblValidationType.Text = "Display Header";
            Title = "Display Header";
        }
        else
        {
            Title = "Display Fields";
            lblValidationType.Text = "Display Fields";
        }

      
        if (!IsPostBack)
        {
            if (Request.QueryString["formula"] != null)
            {
                txtFormula.Text = Request.QueryString["formula"].ToString();
            }

        }

        
                
        _iTableID = int.Parse(Request.QueryString["Tableid"].ToString());
               
           
       

        hfTableID.Value = _iTableID.ToString();
        //hfColumnID.Value = _iColumnID.ToString();


        
        //Content theContent = SystemData.Content_Details_ByKey("DisplayColumnsHelp", null);
        //if (theContent != null)
        //{
        //    lblContentCommon.Text = theContent.ContentP;
        //}
       
       

        if (!IsPostBack)
        {

            PopulateDatabaseColumns();
            this.lnkSave.Attributes.Add("onclick", "javascript:return GetBackValue()");
            PopulateTerminology();
          
           
        }
        

    }


    protected void PopulateTerminology()
    {

        lblValidationType.Text = lblValidationType.Text.Replace("Fields", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Fields", "Fields"));
    }

    protected void PopulateDatabaseColumns()
    {
        //int iTN = 0;

        //DataTable theTable = RecordManager.ets_Record_List(_iTableID, null, null, null, null, null,
        //    "","", 0, 1, ref iTN, ref iTN, "nonstandard", "", "", null, null,"","","");
        //if (theTable.Columns.Count > 1)
        //{
        //    theTable.Columns.RemoveAt(theTable.Columns.Count - 1);
        //    theTable.AcceptChanges();

        //}
        //if (theTable.Columns.Count > 1)
        //{
        //    theTable.Columns.RemoveAt(theTable.Columns.Count - 1);
        //    theTable.AcceptChanges();

        //}

        //ddlDatabaseField.DataSource = Common.GetColumnsFromTable(theTable);
        //ddlDatabaseField.DataBind();

        int iTableID = int.Parse(_iTableID.ToString());
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
   
   
   

    protected void lnkSave_Click(object sender, EventArgs e)
    {
       
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Save Action", "GetBackValue();", true);
        
    }

   

}
