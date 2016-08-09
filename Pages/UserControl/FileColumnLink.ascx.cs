using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_UserControl_FileColumnLink : System.Web.UI.UserControl
{
    public int? TableID { get; set; }
    public List<string> lstFileColumn { get; set; }
    DataTable _dtTableColumns;
    Table _theTable;
    int _iFileColumn = 0;

    public DataTable GetLinkedColumn
    {
        get
        {
            DataTable dtLinkedColumn = new DataTable();
            dtLinkedColumn.Columns.Add("FileColumn");
            dtLinkedColumn.Columns.Add("ColumnID");
            dtLinkedColumn.AcceptChanges();

            for (int i = 0; i < gvTheGrid.Rows.Count; i++)
            {
                DropDownList ddlColumn = (DropDownList)gvTheGrid.Rows[i].FindControl("ddlColumn");

                if (ddlColumn != null && ddlColumn.SelectedItem != null && ddlColumn.SelectedValue != "")
                {
                    Label lblFileColumn = (Label)gvTheGrid.Rows[i].FindControl("lblFileColumn");
                    if(lblFileColumn!=null && lblFileColumn.Text!="")
                    {
                        dtLinkedColumn.Rows.Add(lblFileColumn.Text, ddlColumn.SelectedValue);
                    }
                    
                }

               
            }

            dtLinkedColumn.AcceptChanges();

            if (dtLinkedColumn.Rows.Count>0)
            {
                return dtLinkedColumn;
            }

            return null;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(TableID==null)
        {
            if(Request.QueryString["TableID"]!=null)
            {
                TableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
            }
        }
        if (TableID == null || lstFileColumn == null || lstFileColumn.Count==0)
            return;

        _theTable = RecordManager.ets_Table_Details((int)TableID);

        _dtTableColumns = Common.DataTableFromText("SELECT ColumnID,DisplayName FROM [Column] WHERE IsStandard=0 AND  TableID=" + TableID.ToString() + " ORDER BY DisplayOrder");

        if(!IsPostBack)
        {
            MakeTheGridTable();

            BindTheGrid();
        }

    }
    public void PopulateGrid()
    {
        if(_dtTableColumns==null)
        {
            _dtTableColumns = Common.DataTableFromText("SELECT ColumnID,DisplayName FROM [Column] WHERE IsStandard=0 AND  TableID=" + TableID.ToString() + " ORDER BY DisplayName");

        }
        MakeTheGridTable();

        BindTheGrid();

    }

    protected void MakeTheGridTable()
    {
        DataTable theGridDatatable = new DataTable();
        theGridDatatable.Columns.Add("ID");
        theGridDatatable.Columns.Add("FileColumn");
        theGridDatatable.Columns.Add("CollumnID");
        theGridDatatable.AcceptChanges();
        foreach (string strFC in lstFileColumn)
        {
            theGridDatatable.Rows.Add(Guid.NewGuid().ToString(), strFC, "");
        }
        theGridDatatable.AcceptChanges();
        ViewState["theGridDatatable"] = theGridDatatable;
    }

   protected void BindTheGrid()
    {
       if(ViewState["theGridDatatable"] !=null)
       {
           DataTable theGridDatatable = (DataTable)ViewState["theGridDatatable"];
           gvTheGrid.DataSource = theGridDatatable;
           gvTheGrid.VirtualItemCount = theGridDatatable.Rows.Count;
           gvTheGrid.DataBind();

       }
    }
   protected void gvTheGrid_PreRender(object sender, EventArgs e)
    {
        GridView grid = (GridView)sender;
        if (grid != null)
        {
            GridViewRow pagerRow = (GridViewRow)grid.TopPagerRow;
            if (pagerRow != null)
            {
                pagerRow.Visible = true;
            }
        }
    }


    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileColumn = (Label)e.Row.FindControl("lblFileColumn");

            if (lstFileColumn[_iFileColumn]!="")
                lblFileColumn.Text =lstFileColumn[_iFileColumn];

           

            DropDownList ddlColumn = (DropDownList)e.Row.FindControl("ddlColumn");
            PopulateTableColumn(ref ddlColumn);


            _iFileColumn = _iFileColumn + 1;
        }
    }


    protected void PopulateTableColumn(ref DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = _dtTableColumns;
        ddl.DataBind();

        ListItem liSelect = new ListItem("--Please Select--", "");
        ddl.Items.Insert(0, liSelect);
        
        if (lstFileColumn[_iFileColumn]!="")
        {
            string strFileColumnName = lstFileColumn[_iFileColumn].ToLower();

            foreach(DataRow dr in _dtTableColumns.Rows)
            {
                string strDN = dr["DisplayName"].ToString().ToLower();
                string strColumnID = dr["ColumnID"].ToString();
                if(dr["DisplayName"].ToString().ToLower()==strFileColumnName)
                {
                    if (ddl.Items.FindByValue(strColumnID) != null)
                        ddl.SelectedValue = strColumnID;
                }
                else
                {
                    if(strDN.IndexOf(strFileColumnName)>-1 || strFileColumnName.IndexOf(strDN)>-1)
                    {
                        if (ddl.Items.FindByValue(strColumnID) != null)
                            ddl.SelectedValue = strColumnID;
                    }
                }

            }
            
        }

    }

}