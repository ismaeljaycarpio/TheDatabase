using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;

public partial class Pages_UserControl_ControlByColumnValue : System.Web.UI.UserControl
{
   
    public int? TableID { get; set; }
    public int? ViewID { get; set; }
    public event EventHandler ddlYAxis_Changed;

   

    public string ddlYAxisClientID
    {
        get
        {            
           
            return ddlYAxis.ClientID;
        }
       
    }

    public bool HideColumn
    {
        set
        {
            tdFilterYAxis.Visible = !value;
        }

    }

    public string ddlYAxisV
    {
         get
        {

            return ddlYAxis.SelectedValue == null ? "" : ddlYAxis.SelectedValue;
        }
        set
        {
           
                PopulateYAxis();
                ddlYAxis.SelectedValue = value;
                ddlYAxis_SelectedIndexChanged(null, null);
            
            
        }

    }


 

    public string txtLowerLimitV
    {
        get
        {
            return txtLowerLimit.Text;
        }
        set
        {
            txtLowerLimit.Text = value;
        }

    }

  
    public string txtLowerDateV
    {
        get
        {
            return txtLowerDate.Text;
        }
        set
        {
            txtLowerDate.Text = value;
        }

    }

   

    public string ddlDropdownColumnSearchV
    {
        get
        {
            return ddlDropdownColumnSearch.SelectedValue == null ? "" : ddlDropdownColumnSearch.SelectedValue;
        }
        set
        {
            if(value!="")
                ddlDropdownColumnSearch.SelectedValue = value;
        }

    }


    public string txtSearchTextV
    {
        get
        {
            return txtSearchText.Text;
        }
        set
        {
            txtSearchText.Text = value;
        }

    }




   

    protected void Page_Init(object sender, EventArgs e)
    {


        if (this.Page.MasterPageFile != null && this.Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        {
            ddlYAxis.Width = 220;
            divYAxis.Width = 200;

            ddlYAxis.CssClass = "ddlrrp";
            divYAxis.CssClass = "ddlDIV";

        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (TableID == null)
        {
            if (Request.QueryString["TableID"] != null)
            {
                TableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
            }
        }

        if (!IsPostBack)
        {
            if(TableID!=null)
                PopulateYAxis();
        }
    }

   

    protected void PopulateYAxis()
    {

        //DataTable dtSCs = RecordManager.ets_Table_Columns_All((int)TableID,null,null);
        //ddlYAxis.Items.Clear();

        if (ddlYAxis.Items.Count > 0)
            return;

        DataTable dtSCs ;

        if (ViewID == null)
        {
            dtSCs = Common.DataTableFromText("SELECT * FROM [Column] WHERE IsStandard=0 AND TableID=" + TableID.ToString()
                + "  ORDER BY DisplayOrder");

            foreach (DataRow dr in dtSCs.Rows)
            {
                if (bool.Parse(dr["IsStandard"].ToString()) == false)
                {
                    if (dr["DisplayTextSummary"] != DBNull.Value || dr["DisplayName"] != DBNull.Value)
                    {

                        System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(
                            dr["DisplayTextSummary"] == DBNull.Value ? dr["DisplayName"].ToString() : dr["DisplayTextSummary"].ToString(),
                            dr["ColumnID"].ToString());

                        ddlYAxis.Items.Insert(ddlYAxis.Items.Count, aItem);
                    }
                }

            }
        }
        else
        {
            dtSCs = Common.DataTableFromText("  SELECT VT.*,ISNULL(C.DisplayTextSummary,C.DisplayName) AS Heading FROM ViewItem VT INNER JOIN [Column] C ON VT.ColumnID=C.ColumnID WHERE ViewID=" + ViewID.ToString() + @" AND FilterField=1 ORDER BY Heading");

            foreach (DataRow dr in dtSCs.Rows)
            {                
                System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(
                    dr["Heading"].ToString(),
                    dr["ColumnID"].ToString());

                ddlYAxis.Items.Insert(ddlYAxis.Items.Count, aItem);                 
            }
        }

       

        


        System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("-- None --", "");

        ddlYAxis.Items.Insert(0, fItem);

    }


    public string GetValue
    {

        get
        {
            if (ddlYAxis.SelectedValue == "")
            {

            }
            else
            {

                int iC = int.Parse(ddlYAxis.SelectedValue);



                //if (iC > 0)
                //{
                Column theColumn = RecordManager.ets_Column_Details(iC);

                if (theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false)
                {
                    return txtLowerLimit.Text;

                }
                else if (theColumn.ColumnType == "dropdown" && theColumn.DropdownValues != "" &&
                    (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
                {


                    return ddlDropdownColumnSearch.SelectedValue;

                }
                else if (theColumn.ColumnType == "dropdown" &&
                    (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
                    theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                {
                    return ddlDropdownColumnSearch.SelectedValue;

                }
                else if (theColumn.ColumnType == "date" || theColumn.ColumnType == "datetime")
                {
                    return txtLowerDate.Text;
                }
                else
                {
                    return txtSearchText.Text;
                }
                
            }

            return "";
        }

    }


    public string SetValue
    {

        set
        {
            if (ddlYAxis.SelectedValue == "")
            {

            }
            else
            {

                int iC = int.Parse(ddlYAxis.SelectedValue);



                //if (iC > 0)
                //{
                Column theColumn = RecordManager.ets_Column_Details(iC);

                if (theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false)
                {
                     txtLowerLimit.Text=value;

                }
                else if (theColumn.ColumnType == "dropdown" && theColumn.DropdownValues != "" &&
                    (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
                {


                     ddlDropdownColumnSearch.SelectedValue=value;

                }
                else if (theColumn.ColumnType == "dropdown" &&
                    (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
                    theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                {
                    ddlDropdownColumnSearch.SelectedValue=value;

                }
                else if (theColumn.ColumnType == "date" || theColumn.ColumnType == "datetime")
                {
                     txtLowerDate.Text=value;
                }
                else
                {
                    txtSearchText.Text=value;
                }

            }

           
        }

    }

    protected void ddlYAxis_SelectedIndexChanged(object sender, EventArgs e)
    {
        //do the show hide

        if (ddlYAxis_Changed != null)
            ddlYAxis_Changed(this, EventArgs.Empty);


        if (ddlYAxis.SelectedValue == "")
        {
            txtLowerLimit.Visible = false;          
            txtSearchText.Visible = false;
            ddlDropdownColumnSearch.Visible = false;

            txtLowerLimit.Text = "";
           
            txtSearchText.Text = "";
            if (ddlDropdownColumnSearch.Items.Count > 0)
                ddlDropdownColumnSearch.SelectedIndex = 0;

           

        }
        else
        {

            int iC = int.Parse(ddlYAxis.SelectedValue);

            

            //if (iC > 0)
            //{
                Column theColumn = RecordManager.ets_Column_Details(iC);

                if (theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false)
                {
                    txtLowerLimit.Visible = true;
               
                    txtSearchText.Visible = false;

                    txtLowerDate.Visible = false;
                 
                    ibLowerDate.Visible = false;
                  
                    ddlDropdownColumnSearch.Visible = false;
                }
                else if (theColumn.ColumnType == "dropdown" && theColumn.DropdownValues != "" &&
                    (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
                {
                    txtLowerLimit.Visible = false;
                   
                  
                    txtSearchText.Visible = false;

                    txtLowerDate.Visible = false;
                   
                    ibLowerDate.Visible = false;
                   
                    ddlDropdownColumnSearch.Visible = true;

                    if (theColumn.DropDownType == "values")
                    {
                        TheDatabase.PutDDLValues(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
                    }
                    else
                    {
                        TheDatabase.PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
                    }

                }
                else if (theColumn.ColumnType == "dropdown" &&
                    (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
                    theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                {
                    txtLowerLimit.Visible = false;
                   
                    txtSearchText.Visible = false;

                    txtLowerDate.Visible = false;
                 
                    ibLowerDate.Visible = false;
                
                    ddlDropdownColumnSearch.Visible = true;


                    ddlDropdownColumnSearch.Items.Clear();
                    RecordManager.PopulateTableDropDown((int)theColumn.ColumnID, ref ddlDropdownColumnSearch);

                }
                else if (theColumn.ColumnType == "date" || theColumn.ColumnType == "datetime")
                {
                    txtLowerLimit.Visible = false;
                   
                    txtSearchText.Visible = false;

                    txtLowerDate.Visible = true;
                  
                    ibLowerDate.Visible = true;
                   
                    ddlDropdownColumnSearch.Visible = false;
                }
                else
                {
                    txtLowerLimit.Visible = false;
                   
                    txtSearchText.Visible = true;

                    txtLowerDate.Visible = false;
                  
                    ibLowerDate.Visible = false;
                   
                    ddlDropdownColumnSearch.Visible = false;
                }
                txtLowerLimit.Text = "";
               
                txtSearchText.Text = "";
                txtLowerDate.Text = "";
               
                if (ddlDropdownColumnSearch.Items.Count > 0)
                    ddlDropdownColumnSearch.SelectedIndex = 0;
            
        }

    }


}