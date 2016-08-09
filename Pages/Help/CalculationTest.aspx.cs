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
    int _iColumnID = -1;
    int _iTableID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {

        //headername

        Title = "TheDatabase - Calculation";

        if (Request.QueryString["date"] != null)
        {
            hfCalculationType.Value = "d";
        }


        if (!IsPostBack)
        {
            if (Request.QueryString["formula"] != null)
            {
                txtFormula.Text = Request.QueryString["formula"].ToString();
            }

        }



        if (Request.QueryString["Columnid"] != null)
        {
            if (Request.QueryString["Columnid"].ToString() != "" && Request.QueryString["Columnid"].ToString() != "-1")
            {
                _iColumnID = int.Parse(Request.QueryString["Columnid"].ToString());
                Column tempColumn = RecordManager.ets_Column_Details(_iColumnID);
                _iTableID = (int)tempColumn.TableID;

            }
            else
            {
                _iColumnID = -1;
                _iTableID = int.Parse(Request.QueryString["Tableid"].ToString());

            }
        }




        hfTableID.Value = _iTableID.ToString();
        hfColumnID.Value = _iColumnID.ToString();
      



        if (!IsPostBack)
        {

            PopulateDatabaseColumns();
            this.lnkSave.Attributes.Add("onclick", "javascript:return GetBackValue()");
            PopulateTerminology();


        }


    }


    protected void PopulateTerminology()
    {

        stgField.InnerText = stgField.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
    }

    protected void PopulateDatabaseColumns()
    {
        int iTN = 0;
        string strReturnSQL = "";
        DataTable theTable = RecordManager.ets_Record_List(_iTableID, null, null, null, null, null,
            "", "", 0, 1, ref iTN, ref iTN, "nonstandard", "", "", null, null, "", "", "", null, ref strReturnSQL, ref strReturnSQL);
        if (theTable.Columns.Count > 1)
        {
            theTable.Columns.RemoveAt(theTable.Columns.Count - 1);
            theTable.AcceptChanges();

        }
        if (theTable.Columns.Count > 1)
        {
            theTable.Columns.RemoveAt(theTable.Columns.Count - 1);
            theTable.AcceptChanges();

        }

        if (hfCalculationType.Value == "d")
        {
            ddlDatabaseField.DataSource = GetDateColumnsFromTable(theTable,_iTableID);
            ddlDatabaseField.DataBind();
        }
        else
        {
            PopulateDataSource(_iTableID);
            //ddlDatabaseField.DataSource = Common.GetColumnsFromTable(theTable);
        }


        

    }

    public  ListItemCollection GetDateColumnsFromTable(DataTable theDatatable,int iTableID)
    {



        ListItemCollection list = new ListItemCollection();
        // ArrayList list = new ArrayList();
        string sItem = "";
        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {
            string sColumn = theDatatable.Columns[i].ColumnName;

            DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnType,NumberTYpe FROM [Column]
	                    WHERE TableID=" + iTableID.ToString() + @" AND DisplayName='" + sColumn.Replace("'","''") + "'");

            bool bAddColumn = false;
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["ColumnType"].ToString() == "datetime" || dtTemp.Rows[0]["ColumnType"].ToString() == "date"
                    || dtTemp.Rows[0]["ColumnType"].ToString() == "time")
                {
                    bAddColumn = true;
                }
                if (dtTemp.Rows[0]["ColumnType"].ToString() == "number" && dtTemp.Rows[0]["NumberTYpe"] != DBNull.Value)
                {
                    if (dtTemp.Rows[0]["NumberTYpe"].ToString() == "1")
                    {
                        bAddColumn = true;
                    }
                }
            }


            if (bAddColumn)
            {
                string sValue = "";
                if (theDatatable.Rows.Count > 0)
                    sValue = theDatatable.Rows[0][i].ToString();
                if (sValue.Length > 20) sValue = String.Concat(sValue.Substring(0, 20), "...");
                if (sValue.Length > 0)
                    sItem = string.Concat(sColumn, " (e.g. ", sValue, ")");
                else
                    sItem = sColumn;
                list.Add(new System.Web.UI.WebControls.ListItem(sItem, sColumn));
            }
        }



        //Work with 1 top level Parent tables.
        DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + iTableID.ToString()); //AND DetailPageType<>'not'

        if (dtPT.Rows.Count > 0)
        {

            bool bAddColumn = false;

            foreach (DataRow dr in dtPT.Rows)
            {
                DataTable dtPColumns = Common.DataTableFromText(@"SELECT distinct TableName + ':' + DisplayName AS DP,ColumnType,NumberTYpe FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE IsStandard=0 AND TableTableID IS NULL AND  [Column].TableID=" + dr["ParentTableID"].ToString());
                foreach (DataRow drP in dtPColumns.Rows)
                {
                    bAddColumn = false;
                    if (drP["ColumnType"].ToString() == "datetime" || drP["ColumnType"].ToString() == "date"
                    || drP["ColumnType"].ToString() == "time")
                    {
                        bAddColumn = true;
                    }
                    if (drP["ColumnType"].ToString() == "number" && drP["NumberTYpe"] != DBNull.Value)
                    {
                        if (drP["NumberTYpe"].ToString() == "1")
                        {
                            bAddColumn = true;
                        }
                    }

                    if (bAddColumn == true)
                    {
                        ListItem aItem = new ListItem(drP["DP"].ToString(), drP["DP"].ToString());
                        list.Add(aItem);
                    }
                }
            }
        }



        return list;

    }


    protected void PopulateDataSource(int iTableID)
    {

        
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
