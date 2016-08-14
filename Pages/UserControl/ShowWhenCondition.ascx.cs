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

public partial class Pages_UserControl_ShowWhenCondition : System.Web.UI.UserControl
{
    public int? TableID { get; set; }
    public int? ColumnID { get; set; }
    
    public int? DocumentSectionID { get; set; }
    public int? TableTabID { get; set; }
    public bool? ShowTable { get; set; }

    public event EventHandler ddlHideColumn_Changed;
    public bool ShowJoinOperator
    {
       set
        {
            if(value==false)
            {
                ddlJoinOperator.SelectedValue = "";
                trJoinOperator.Visible = false;
            }
            else
            {
                trJoinOperator.Visible = true;
            }
        }
    }

    public string ddlJoinOperatorV
    {
        get
        {

            return ddlJoinOperator.SelectedValue == null ? "" : ddlJoinOperator.SelectedValue;
        }
        set
        {           
            ddlJoinOperator.SelectedValue = value;       

        }

    }

    public string ddlOperatorV
    {
        get
        {

            return ddlOperator.SelectedValue;
        }
        set
        {
            ddlOperator.SelectedValue = value;

        }

    }

    public string ddlHideColumnV
    {
        get
        {

            return ddlHideColumn.SelectedValue == null ? "" : ddlHideColumn.SelectedValue;
        }
        set
        {


            if (ShowTable != null && (bool)ShowTable && value!="")
            {
                if (ddlTable.SelectedItem == null)
                    PopulateTable();

                string strTableID = Common.GetValueFromSQL("SELECT TableID FROM [Column] WHERE ColumnID=" + value);
                if (strTableID != "" && ddlTable.Items.FindByValue(strTableID) != null)
                {
                    ddlTable.SelectedValue = strTableID;
                    TableID = int.Parse(ddlTable.SelectedValue);
                }
            }


            PopulateHideColumns();
            if(ddlHideColumn.Items.FindByValue(value)!=null)
            {
                ddlHideColumn.SelectedValue = value;

               

                ddlHideColumn_SelectedIndexChanged(null, null);
            }
            


        }

    }

    public string hfHideColumnValueV
    {
        get
        {
            hfHideColumnValue.Value = "";
            if (ddlHideColumn.SelectedValue != "")
            {
                hfHideColumnValue.Value = cuiHideColumnValue.ColumnValue;
            }

            return hfHideColumnValue.Value;
        }
        set
        {
           
            hfHideColumnValue.Value = value;

            if (ddlHideColumn.SelectedValue != "")
            {
                //Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(ddlHideColumn.SelectedValue));
                cuiHideColumnValue.ColumnValue = hfHideColumnValue.Value;
            }


        }

    }


    //public string hfHideColumnValueV
    //{
    //    get
    //    {
    //        hfHideColumnValue.Value = "";
    //        if (ddlHideColumn.SelectedValue != "")
    //        {
    //            Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(ddlHideColumn.SelectedValue));
    //            switch (theHideColumn.ColumnType)
    //            {
    //                case "listbox":
    //                    hfHideColumnValue.Value = Common.GetListValues(lstHideColumnValue);
    //                    break;
    //                case "checkbox":
    //                    hfHideColumnValue.Value = Common.GetCheckBoxValue(theHideColumn.DropdownValues, ref chkHideColumnValue);
    //                    break;

    //                case "radiobutton":
    //                    if (radioHideColumnValue.SelectedItem != null)
    //                        hfHideColumnValue.Value = radioHideColumnValue.SelectedItem.Value;
    //                    break;


    //                case "dropdown":
    //                    if (ddlHideColumnValue.SelectedItem != null)
    //                        hfHideColumnValue.Value = ddlHideColumnValue.SelectedValue;
    //                    break;

    //                default:
    //                    hfHideColumnValue.Value = txtHideColumnValue.Text;
    //                    break;
    //            }
    //        }

    //        return hfHideColumnValue.Value;
    //    }
    //    set
    //    {

    //        hfHideColumnValue.Value = value;

    //        if (ddlHideColumn.SelectedValue != "")
    //        {
    //            Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(ddlHideColumn.SelectedValue));
    //            if (theHideColumn != null)
    //            {
    //                txtHideColumnValue.Visible = false;
    //                ddlHideColumnValue.Visible = false;
    //                lstHideColumnValue.Visible = false;
    //                chkHideColumnValue.Visible = false;
    //                radioHideColumnValue.Visible = false;



    //                switch (theHideColumn.ColumnType)
    //                {
    //                    case "listbox":
    //                        lstHideColumnValue.Visible = true;

    //                        if (theHideColumn.DropDownType == "values")
    //                        {
    //                            Common.PutListValues(theHideColumn.DropdownValues, ref lstHideColumnValue);
    //                        }
    //                        else
    //                        {
    //                            Common.PutListValues_Text(theHideColumn.DropdownValues, ref lstHideColumnValue);
    //                        }

    //                        Common.SetListValues(hfHideColumnValue.Value, ref lstHideColumnValue);


    //                        break;

    //                    case "checkbox":

    //                        chkHideColumnValue.Visible = true;

    //                        Common.PutCheckBoxDefault(theHideColumn.DropdownValues, ref chkHideColumnValue);
    //                        Common.SetCheckBoxValue(theHideColumn.DropdownValues, hfHideColumnValue.Value, ref chkHideColumnValue);
    //                        break;

    //                    case "radiobutton":

    //                        radioHideColumnValue.Visible = true;



    //                        if (theHideColumn.DropDownType == "values")
    //                        {
    //                            Common.PutRadioList(theHideColumn.DropdownValues, ref radioHideColumnValue);
    //                        }
    //                        else
    //                        {
    //                            Common.PutRadioListValue_Text(theHideColumn.DropdownValues, ref radioHideColumnValue);
    //                        }

    //                        radioHideColumnValue.SelectedValue = hfHideColumnValue.Value;
    //                        break;

    //                    case "dropdown":
    //                        ddlHideColumnValue.Visible = true;
    //                        if (theHideColumn.DropDownType == "values")
    //                        {
    //                            Common.PutDDLValues(theHideColumn.DropdownValues, ref ddlHideColumnValue);

    //                            if (ddlHideColumnValue.Items.FindByValue(hfHideColumnValue.Value) != null)
    //                                ddlHideColumnValue.SelectedValue = hfHideColumnValue.Value;
    //                        }
    //                        else if (theHideColumn.DropDownType == "value_text")
    //                        {
    //                            Common.PutDDLValue_Text(theHideColumn.DropdownValues, ref ddlHideColumnValue);

    //                            if (ddlHideColumnValue.Items.FindByValue(hfHideColumnValue.Value) != null)
    //                                ddlHideColumnValue.SelectedValue = hfHideColumnValue.Value;
    //                        }
    //                        else
    //                        {
    //                            ddlHideColumnValue.Visible = true;
    //                            RecordManager.PopulateTableDropDown((int)theHideColumn.ColumnID, ref ddlHideColumnValue);

    //                            if (ddlHideColumnValue.Items.FindByValue(hfHideColumnValue.Value) != null)
    //                                ddlHideColumnValue.SelectedValue = hfHideColumnValue.Value;
    //                        }


    //                        break;
    //                    default:
    //                        txtHideColumnValue.Text = hfHideColumnValue.Value;
    //                        txtHideColumnValue.Visible = true;

    //                        break;
    //                }
    //            }
    //        }


    //    }

    //}
    protected void Page_Load(object sender, EventArgs e)
    {

        if(!IsPostBack)
        {
            PopulateTerminology();

            if(ddlTable.SelectedItem==null)
                PopulateTable();
            if(ShowTable!=null && (bool)ShowTable)
            {
                ddlTable.Visible = true;
            }
            
        }

    }

    protected void PopulateTerminology()
    {
        stgField.InnerText = stgField.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(
            Request.Path.LastIndexOf("/") + 1), "Field", "Field"));

       

    }

    protected void PopulateHideColumns()
    {
        if (TableID == null)
            PopulateTable();

        if (TableID == null)
        {
            return;
        }
        string strExtra = "";

        if (ColumnID != null)
            strExtra = "  AND ColumnID<>" + ColumnID.ToString();

        ddlHideColumn.Items.Clear();
        DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] 
                WHERE  IsStandard=0 AND TableID=" + TableID.ToString() + strExtra+ " ORDER BY DisplayName");
        ddlHideColumn.DataSource = dtTemp;
        ddlHideColumn.DataBind();
        ListItem li = new ListItem("--Please Select--", "");
        ddlHideColumn.Items.Insert(0, li);
       
    }

    protected void PopulateTable()
    {
        if (TableID != null)
            return;

        ddlTable.Items.Clear();
        DataTable dtTemp = Common.DataTableFromText(@"SELECT T.TableID,T.TableName FROM [Table] T INNER JOIN [Column] C 
    ON T.TableID=C.TableID WHERE C.TableTableID=-1 AND T.AccountID=" + Session["AccountID"].ToString() + " ORDER BY T.TableName");
        ddlTable.DataSource = dtTemp;
        ddlTable.DataBind();

        if(ddlTable.SelectedItem!=null)
        {
            TableID = int.Parse(ddlTable.SelectedValue);
        }
        else
        {
            return;
        }
        PopulateHideColumns();

        if(DocumentSectionID!=null && ddlTable.Visible==false)
        {
            ddlTable.Visible = true;
        }

        //ListItem li = new ListItem("--Please Select--", "");
        //ddlHideColumn.Items.Insert(0, li);

    }

    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        TableID = int.Parse(ddlTable.SelectedValue);
        PopulateHideColumns();
        
    }

    protected void ddlOperator_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlOperator.SelectedValue=="empty" || ddlOperator.SelectedValue=="notempty")
        {
            cuiHideColumnValue.ColumnValue = "";
            //cuiHideColumnValue.Visible = false;
            cuiHideColumnValue.ShowControls = false;
            hfHideColumnValue.Value = "";
        }
        else
        {
            //cuiHideColumnValue.Visible = true;
            cuiHideColumnValue.ShowControls = true;
            //cuiHideColumnValu
        }
    }
    protected void ddlHideColumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHideColumn_Changed != null)
            ddlHideColumn_Changed(this, EventArgs.Empty);

        if (ddlHideColumn.SelectedValue == "")
        {
            cuiHideColumnValue.ColumnID = null;
        }
        else
        {
            cuiHideColumnValue.ColumnID = int.Parse(ddlHideColumn.SelectedValue);

            Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(ddlHideColumn.SelectedValue));
            if(theHideColumn.ColumnType=="radiobutton")
            {
                ddlOperator.SelectedValue = "equals";
                ddlOperator.Enabled = false;
            }
            else
            {
                ddlOperator.Enabled = true;
            }
        }

    }
    
    //protected void ddlHideColumn_SelectedIndexChanged(object sender, EventArgs e)
    //{


    //    if (ddlHideColumn_Changed != null)
    //        ddlHideColumn_Changed(this, EventArgs.Empty);

    //    if (ddlHideColumn.SelectedValue != "")
    //    {
    //        Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(ddlHideColumn.SelectedValue));
    //        if (theHideColumn != null)
    //        {
    //            txtHideColumnValue.Visible = false;
    //            ddlHideColumnValue.Visible = false;
    //            lstHideColumnValue.Visible = false;
    //            chkHideColumnValue.Visible = false;
    //            radioHideColumnValue.Visible = false;


    //            switch (theHideColumn.ColumnType)
    //            {
    //                case "listbox":
    //                    lstHideColumnValue.Visible = true;
    //                    lstHideColumnValue.Items.Clear();
    //                    if (theHideColumn.DropDownType == "values")
    //                    {
    //                        Common.PutListValues(theHideColumn.DropdownValues, ref lstHideColumnValue);
    //                    }
    //                    else
    //                    {
    //                        Common.PutListValues_Text(theHideColumn.DropdownValues, ref lstHideColumnValue);
    //                    }
    //                    //hfHideColumnValueControl.Value = "listbox";
    //                    break;
    //                case "radiobutton":
    //                    radioHideColumnValue.Visible = true;
    //                    ddlOperator.SelectedValue = "equals";
    //                    ddlOperator.Enabled = false;
    //                    if (theHideColumn.DropDownType == "values")
    //                    {
    //                        Common.PutRadioList(theHideColumn.DropdownValues, ref radioHideColumnValue);
    //                    }
    //                    else
    //                    {
    //                        Common.PutRadioListValue_Text(theHideColumn.DropdownValues, ref radioHideColumnValue);
    //                    }
    //                    //hfHideColumnValueControl.Value = "radiobutton";
    //                    break;
    //                case "checkbox":
    //                    chkHideColumnValue.Visible = true;
    //                    Common.PutCheckBoxDefault(theHideColumn.DropdownValues, ref chkHideColumnValue);
    //                    //hfHideColumnValueControl.Value = "checkbox";
    //                    break;
    //                case "dropdown":
    //                    ddlHideColumnValue.Visible = true;
    //                    ddlHideColumnValue.Items.Clear();
    //                    if (theHideColumn.DropDownType == "values")
    //                    {
    //                        Common.PutDDLValues(theHideColumn.DropdownValues, ref ddlHideColumnValue);
    //                        //hfHideColumnValueControl.Value = "dropdown";
    //                    }
    //                    else if (theHideColumn.DropDownType == "value_text")
    //                    {
    //                        Common.PutDDLValue_Text(theHideColumn.DropdownValues, ref ddlHideColumnValue);
    //                        //hfHideColumnValueControl.Value = "dropdown";
    //                    }
    //                    else
    //                    {
    //                        //ddlHideColumnValue.Visible = false;
    //                        //txtHideColumnValue.Visible = true;
    //                        RecordManager.PopulateTableDropDown((int)theHideColumn.ColumnID, ref ddlHideColumnValue);
    //                        //hfHideColumnValueControl.Value = "dropdown";
    //                    }


    //                    break;
    //                default:
    //                    txtHideColumnValue.Visible = true;
    //                    //hfHideColumnValueControl.Value = "text";
    //                    break;
    //            }

    //        }

    //    }




    //}

    //protected void PutListValues_Text(string strDropdownValues, ref  ListBox lb)
    //{
    //    lb.Items.Clear();
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    string strValue = "";
    //    string strText = "";

    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        strValue = "";
    //        strText = "";
    //        if (s.IndexOf(",") > -1)
    //        {
    //            strValue = s.Substring(0, s.IndexOf(","));
    //            strText = s.Substring(strValue.Length + 1);
    //            if (strValue != "" && strText != "")
    //            {
    //                ListItem liTemp = new ListItem(strText, strValue);
    //                lb.Items.Add(liTemp);
    //            }
    //        }
    //    }


    //}

    //protected void SetListValues(string strDBValues, ref  ListBox lb)
    //{
    //    if (strDBValues != "")
    //    {
    //        string[] strSS = strDBValues.Split(',');
    //        foreach (string SS in strSS)
    //        {
    //            try
    //            {
    //                if (SS != "")
    //                    lb.Items.FindByValue(SS).Selected = true;
    //            }
    //            catch
    //            {
    //                //
    //            }
    //        }
    //    }

    //}
    //protected void PutListValues(string strDropdownValues, ref  ListBox lb)
    //{
    //    lb.Items.Clear();
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    foreach (string s in result)
    //    {
    //        ListItem liTemp = new ListItem(s, s);
    //        lb.Items.Add(liTemp);
    //    }


    //}
    //protected string GetListValues(ListBox lb)
    //{
    //    string strSelectedValues = "";

    //    foreach (ListItem item in lb.Items)
    //    {
    //        if (item.Selected)
    //        {
    //            strSelectedValues = strSelectedValues + item.Value + ",";
    //        }
    //    }

    //    if (strSelectedValues != "")
    //        strSelectedValues = strSelectedValues.Substring(0, strSelectedValues.Length - 1);
    //    return strSelectedValues;
    //}

    //protected string GetCheckBoxValue(string strDropdownValues, ref  CheckBox chk)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    int i = 0;
    //    foreach (string s in result)
    //    {
    //        if (i == 0)
    //        {
    //            if (chk.Checked)
    //            {
    //                return s;
    //            }
    //        }
    //        if (i == 1)
    //        {
    //            if (chk.Checked == false)
    //            {
    //                return s;
    //            }
    //        }
    //        i = i + 1;
    //    }
    //    return "";
    //}
    //protected void SetCheckBoxValue(string strDropdownValues, string strValue, ref  CheckBox chk)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    int i = 0;
    //    foreach (string s in result)
    //    {
    //        if (i == 0)
    //        {
    //            if (s.ToLower() == strValue.ToLower())
    //            {
    //                chk.Checked = true;
    //            }
    //        }
    //        if (i == 1)
    //        {
    //            if (s.ToLower() == strValue.ToLower())
    //            {
    //                chk.Checked = false;
    //            }
    //        }
    //        i = i + 1;
    //    }


    //}
    //protected void PutCheckBoxDefault(string strDropdownValues, ref  CheckBox chk)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    int i = 0;
    //    foreach (string s in result)
    //    {
    //        if (i == 2)
    //        {
    //            if (s.ToLower() == "yes")
    //            {
    //                chk.Checked = true;
    //            }
    //        }
    //        i = i + 1;
    //    }


    //}

    //protected void PutDDLValues(string strDropdownValues, ref  DropDownList ddl)
    //{
    //    ddl.Items.Clear();
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    foreach (string s in result)
    //    {
    //        ListItem liTemp = new ListItem(s, s);
    //        ddl.Items.Add(liTemp);
    //    }

    //    ListItem liSelect = new ListItem("--Please Select--", "");
    //    ddl.Items.Insert(0, liSelect);

    //}


    //protected void PutDDLValue_Text(string strDropdownValues, ref  DropDownList ddl)
    //{
    //    ddl.Items.Clear();
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    string strValue = "";
    //    string strText = "";

    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        strValue = "";
    //        strText = "";
    //        if (s.IndexOf(",") > -1)
    //        {
    //            strValue = s.Substring(0, s.IndexOf(","));
    //            strText = s.Substring(strValue.Length + 1);
    //            if (strValue != "" && strText != "")
    //            {
    //                ListItem liTemp = new ListItem(strText, strValue);
    //                ddl.Items.Add(liTemp);
    //            }
    //        }
    //    }

    //    ListItem liSelect = new ListItem("--Please Select--", "");
    //    ddl.Items.Insert(0, liSelect);

    //}

    //protected void PutRadioList(string strDropdownValues, ref  RadioButtonList rl)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

    //    rl.Items.Clear();
    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        ListItem liTemp = new ListItem(s + "&nbsp;&nbsp;", s);
    //        rl.Items.Add(liTemp);
    //    }

    //}


    //protected void PutRadioListValue_Text(string strDropdownValues, ref  RadioButtonList rl)
    //{
    //    string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    rl.Items.Clear();
    //    string strValue = "";
    //    string strText = "";

    //    foreach (string s in result)
    //    {
    //        //ListItem liTemp = new ListItem(s, s.ToLower());
    //        strValue = "";
    //        strText = "";
    //        if (s.IndexOf(",") > -1)
    //        {
    //            strValue = s.Substring(0, s.IndexOf(","));
    //            strText = s.Substring(strValue.Length + 1);
    //            if (strValue != "" && strText != "")
    //            {
    //                ListItem liTemp = new ListItem(strText + "&nbsp;&nbsp;", strValue);
    //                rl.Items.Add(liTemp);
    //            }
    //        }
    //    }


    //}




}