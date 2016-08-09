using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Pages_Record_ChildTableDetail : SecurePage
{

    int? _iParentTableID = null;
    int? _iTableChildID = null;



    protected void PopulateTerminology()
    {

        stgParentFieldCap.InnerText = stgParentFieldCap.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        stgChildFieldCap.InnerText = stgChildFieldCap.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
        stgDisplayField.InnerText = stgDisplayField.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        string strFancy = @"
    
                    $(function () {
                                $(""#hlDDEdit"").fancybox({
                                    scrolling: 'auto',
                                    type: 'iframe',
                                    width: 600,
                                    height: 300,
                                    titleShow: false
                                });
                            });

                    ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "FancyBox2", strFancy, true);
        
        _iParentTableID=int.Parse(Request.QueryString["ParentTableID"].ToString());

        if (Request.QueryString["TableChildID"] != null)
            _iTableChildID = int.Parse(Request.QueryString["TableChildID"].ToString());


        if (!IsPostBack)
        {
            hfParentTableID.Value = _iParentTableID.ToString();
            hlDDEdit.NavigateUrl = "~//Pages/Help/TableColumn.aspx?ct=yes&formula= &Tableid="+ _iParentTableID.ToString();
            PopulateHideColumns();
            PopulateChildTable();            

            if (_iTableChildID != null)
            {
                PopulateRecord();
                lblDetailTitle.Text = "Edit Child Table";
            }
            else
            {
                lblDetailTitle.Text = "Add Child Table";
                trConnection.Visible = true;
                trDisplayColumn.Visible = true;
                PopulateChildColumns();
                PopulateParentColumns();
                PopulateDisplayColumns();
            }
            PopulateTerminology();
        }


    }
    protected void PopulateRecord()
    {
        TableChild theTableChild = RecordManager.ets_TableChild_Detail((int)_iTableChildID);
        if (theTableChild != null)
        {
            ddlChildTable.Text = theTableChild.ChildTableID.ToString();
            txtDescription.Text = theTableChild.Description;
            ddlDetailPageType.Text = theTableChild.DetailPageType;
            chkShowAddButton.Checked = (bool)theTableChild.ShowAddButton;
            chkShowEditButton.Checked = (bool)theTableChild.ShowEditButton;

            if (theTableChild.HideColumnID != null)
            {
                chkShowWhen.Checked = true;
                if (ddlHideColumn.Items.FindByValue(theTableChild.HideColumnID.ToString()) != null)
                    ddlHideColumn.SelectedValue = theTableChild.HideColumnID.ToString();


                if (ddlOperator.Items.FindByValue(theTableChild.HideOperator) != null)
                    ddlOperator.SelectedValue = theTableChild.HideOperator;

                Column theHideColumn=RecordManager.ets_Column_Details(int.Parse(theTableChild.HideColumnID.ToString()));
                if(theHideColumn!=null)
                {
                    txtHideColumnValue.Visible = false;
                    lstHideColumnValue.Visible = false;
                   

                    switch (theHideColumn.ColumnType)
                    {
                        case "listbox":
                            lstHideColumnValue.Visible = true;
                            hfHideColumnValueControl.Value = "listbox";
                            if (theHideColumn.DropDownType == "values")
                            {
                                PutListValues(theHideColumn.DropdownValues, ref lstHideColumnValue);
                            }
                            else
                            {
                                PutListValues_Text(theHideColumn.DropdownValues, ref lstHideColumnValue);
                            }
                           
                            SetListValues(theTableChild.HideColumnValue, ref lstHideColumnValue);
                           

                            break;
                      
                        default:
                            txtHideColumnValue.Text = theTableChild.HideColumnValue;
                            txtHideColumnValue.Visible = true;
                            hfHideColumnValueControl.Value = "text";

                            break;
                    }
                }
            }

            ViewState["theTableChild"] = theTableChild;

            trConnection.Visible = false;
            trDisplayColumn.Visible = false;
            ddlChildTable.Enabled = false;
            
        }
       
    }

    protected void PopulateChildTable()
    {
        //int iTN = 0;
        //ddlChildTable.DataSource = RecordManager.ets_Table_Select(null,
        //        null,
        //        null,
        //        int.Parse(Session["AccountID"].ToString()),
        //        null, null, true,
        //        "st.TableName", "ASC",
        //        null, null, ref  iTN, Session["STs"].ToString());

        ddlChildTable.DataSource = Common.DataTableFromText(@"SELECT TableName,TableID FROM [Table]
            WHERE IsActive=1 AND AccountID=" + Session["AccountID"].ToString() + @" AND TableID <>" + _iParentTableID.ToString() + " ORDER BY TableName");

        ddlChildTable.DataBind();

        System.Web.UI.WebControls.ListItem liPlease = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddlChildTable.Items.Insert(0, liPlease);

    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        string strHideColumnValue = "";
        if (chkShowWhen.Checked)
        {
            if (ddlHideColumn.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a field for Show When.');", true);
                ddlHideColumn.Focus();
                return;
            }
            bool bHasValue = true;
            if (hfHideColumnValueControl.Value == "text" && txtHideColumnValue.Text == "")
            {
                bHasValue = false;
            }
            if (hfHideColumnValueControl.Value == "text" && txtHideColumnValue.Text != "")
            {
                strHideColumnValue = txtHideColumnValue.Text;
            }
            if (hfHideColumnValueControl.Value == "listbox" && lstHideColumnValue.SelectedItem == null)
            {
                bHasValue = false;
            }
            if (hfHideColumnValueControl.Value == "listbox" && lstHideColumnValue.SelectedItem != null)
            {
                strHideColumnValue = GetListValues(lstHideColumnValue);
            }

            if (bHasValue==false)
            {
               
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select value for Show When.');", true);                
                return;
            }
            //input is ok

        }



        if (ViewState["theTableChild"] == null)
        {
            //add

            if (ddlChildColumn.SelectedValue != "")
            {
                //lets get the column

                if (hfDisplayColumnsFormula.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a display field.');", true);
                    return;
                }


                TableChild newTableChild = new TableChild(null, _iParentTableID, int.Parse(ddlChildTable.SelectedValue),
              txtDescription.Text, ddlDetailPageType.SelectedValue);

                newTableChild.ShowAddButton = chkShowAddButton.Checked;
                newTableChild.ShowEditButton = chkShowEditButton.Checked;

                if (chkShowWhen.Checked)
                {
                    newTableChild.HideColumnID = int.Parse(ddlHideColumn.SelectedValue);
                    newTableChild.HideOperator = ddlOperator.SelectedValue;
                    newTableChild.HideColumnValue = strHideColumnValue;
                }

                Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(ddlChildColumn.SelectedValue));
                theChildColumn.ColumnType = "dropdown";
                theChildColumn.NumberType = null;
                theChildColumn.DropDownType = "tabledd";
                theChildColumn.DropdownValues = "";
                theChildColumn.TableTableID = _iParentTableID;
                theChildColumn.DisplayColumn = hfDisplayColumnsFormula.Value;
                theChildColumn.ParentColumnID = null;
                theChildColumn.LinkedParentColumnID = int.Parse(ddlParentColumn.SelectedValue);

                //SqlTransaction tn;
                //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

                //connection.Open();
                //tn = connection.BeginTransaction();

                try
                {

                    RecordManager.ets_Column_Update(theChildColumn);
                    RecordManager.ets_TableChild_Insert(newTableChild);

                    
                }
                catch (Exception ex)
                {
                    
                   //
                }

                //update child table's record


                //SqlTransaction tn;
                //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

                //connection.Open();
                //tn = connection.BeginTransaction();

                //try
                //{

                //    DataTable dtChildRecords = Common.DataTableFromText("SELECT " + theChildColumn.SystemName + ",RecordID FROM Record WHERE TableID=" + theChildColumn.TableID.ToString(),tn,connection);

                //    Column theParentColumn = RecordManager.ets_Column_Details(int.Parse(ddlParentColumn.SelectedValue),ref connection,ref tn);
                //    foreach (DataRow drChildR in dtChildRecords.Rows)
                //    {
                //        if (drChildR[0] != DBNull.Value)
                //        {
                //            if (drChildR[0].ToString() != "")
                //            {
                //                DataTable dtParentRecords = Common.DataTableFromText("SELECT RecordID FROM Record WHERE " + theParentColumn.SystemName + "='" + drChildR[0].ToString().Replace("'", "''") + "'  AND TableID=" + theParentColumn.TableID.ToString() + " ORDER BY RecordID",tn,connection);

                //                if (dtParentRecords.Rows.Count > 0)
                //                {
                //                    Common.ExecuteText("UPDATE Record SET " + theChildColumn.SystemName + "='" + dtParentRecords.Rows[0][0].ToString() + "' WHERE RecordID=" + drChildR[1].ToString(),tn);
                //                }
                //                else
                //                {
                //                    //Common.ExecuteText("UPDATE Record SET " + theChildColumn.SystemName + "=NULL WHERE RecordID=" + drChildR[1].ToString(),tn);
                //                }

                //            }

                //        }
                //    }

                //    tn.Commit();
                //    connection.Close();
                //    connection.Dispose();
                //}
                //catch
                //{
                //    tn.Rollback();
                //    connection.Close();
                //    connection.Dispose();
                //}


                


            }


           

           

        }
        else
        {
            //edit
            TableChild editTableChild = (TableChild)ViewState["theTableChild"];

            editTableChild.Description = txtDescription.Text;
            editTableChild.DetailPageType = ddlDetailPageType.SelectedValue;

            editTableChild.ShowAddButton = chkShowAddButton.Checked;
            editTableChild.ShowEditButton = chkShowEditButton.Checked;
            if (chkShowWhen.Checked)
            {
                editTableChild.HideColumnID = int.Parse(ddlHideColumn.SelectedValue);
                editTableChild.HideOperator = ddlOperator.SelectedValue;
                editTableChild.HideColumnValue = strHideColumnValue;
            }
            else
            {
                editTableChild.HideColumnID = null;
                editTableChild.HideOperator = "";
                editTableChild.HideColumnValue = "";
            }
             
            RecordManager.ets_TableChild_Update(editTableChild);
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);


    }

    protected void PopulateChildColumns()
    {
        ddlChildColumn.Items.Clear();
        if (ddlChildTable.SelectedItem != null)
        {
            if (ddlChildTable.SelectedValue != "")
            {
                int iTN = 0;
                List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(ddlChildTable.SelectedValue),
              null, null, ref iTN);             
                ListItem liAdvanced = new ListItem("--Please select--", "");
                ddlChildColumn.Items.Add(liAdvanced);
                foreach (Column eachColumn in lstColumns)
                {
                    if (eachColumn.IsStandard == false)
                    {
                        if (eachColumn.ColumnType.ToLower() != "datetime" && eachColumn.ColumnType.ToLower() != "date"
                && eachColumn.ColumnType.ToLower() != "time" )
                        {
                            ListItem liTemp = new ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
                            ddlChildColumn.Items.Add(liTemp);
                        }
                    }
                    //else
                    //{
                    //    if (eachColumn.SystemName.ToLower() == "recordid")
                    //    {
                    //        ListItem liTemp = new ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
                    //        ddlChildColumn.Items.Add(liTemp);
                    //    }
                    //}                   

                }
            }
        }

        //lets put default

        if (ddlParentColumn.Items.Count > 0 && ddlChildColumn.Items.Count > 0)
        {
            bool bFound = false;

            foreach (ListItem liChild in ddlChildColumn.Items)
            {
                foreach (ListItem liParent in ddlParentColumn.Items)
                {

                    if (liChild.Text == liParent.Text)
                    {
                        bFound = true;
                        ddlChildColumn.SelectedValue = liChild.Value;
                        ddlParentColumn.SelectedValue = liParent.Value;
                        break;
                    }
                }
                if (bFound)
                {
                    break;
                }
            }
        }


    }


    protected void PopulateDisplayColumns()
    {
        ddDDDisplayColumn.Items.Clear();

        int iTN = 0;
        List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(_iParentTableID.ToString()),
      null, null, ref iTN);

        ListItem liAdvanced = new ListItem("--Advanced--", "");
        ddDDDisplayColumn.Items.Add(liAdvanced);

        foreach (Column eachColumn in lstColumns)
        {
            if (eachColumn.IsStandard == false)
            {
                ListItem liTemp = new ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
                ddDDDisplayColumn.Items.Add(liTemp);
            }

        }

    }
   

    protected void PopulateParentColumns()
    {
        ddlParentColumn.Items.Clear();

        int iTN = 0;
        List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(_iParentTableID.ToString()),
        null, null, ref iTN); 
              
        foreach (Column eachColumn in lstColumns)
        {
            if (eachColumn.IsStandard == false)
            {
                ListItem liTemp = new ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
                ddlParentColumn.Items.Add(liTemp);

            }
            else
            {
                if (eachColumn.SystemName.ToLower() == "recordid")
                {
                    ListItem liTemp = new ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
                    ddlParentColumn.Items.Add(liTemp);
                }
            }        

        }

      
    }




    protected void PopulateHideColumns()
    {
        ddlHideColumn.Items.Clear();

        int iTN = 0;
        List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(_iParentTableID.ToString()),
        null, null, ref iTN);

        foreach (Column eachColumn in lstColumns)
        {
            if (eachColumn.IsStandard == false)
            {
                ListItem liTemp = new ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
                ddlHideColumn.Items.Add(liTemp);
            }

        }

        ListItem liSelect = new ListItem("--Please Select--", "");
        ddlHideColumn.Items.Insert(0,liSelect);
    }


    protected void ddlHideColumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        //
        if (ddlHideColumn.SelectedValue != "")
        {
            Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(ddlHideColumn.SelectedValue));
            if (theHideColumn != null)
            {
                txtHideColumnValue.Visible = false;
                //ddlHideColumnValue.Visible = false;
                lstHideColumnValue.Visible = false;

                switch (theHideColumn.ColumnType)
                {
                    case "listbox":
                        lstHideColumnValue.Visible=true;
                        PutListValues(theHideColumn.DropdownValues, ref lstHideColumnValue);
                        hfHideColumnValueControl.Value = "listbox";
                        break;
                  
                    default:
                        txtHideColumnValue.Visible = true;
                        hfHideColumnValueControl.Value = "text";
                        break;
                }

            }

        }
        



    }


    protected void PutListValues_Text(string strDropdownValues, ref  ListBox lb)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    ListItem liTemp = new ListItem(strText, strValue);
                    lb.Items.Add(liTemp);
                }
            }
        }


    }


    protected void SetListValues(string strDBValues, ref  ListBox lb)
    {
        if (strDBValues != "")
        {
            string[] strSS = strDBValues.Split(',');
            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }
        }

    }

    protected void PutListValues(string strDropdownValues, ref  ListBox lb)
    {
        lb.Items.Clear();
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in result)
        {
            ListItem liTemp = new ListItem(s, s);
            lb.Items.Add(liTemp);
        }


    }
    protected string GetListValues(ListBox lb)
    {
        string strSelectedValues = "";

        foreach (ListItem item in lb.Items)
        {
            if (item.Selected)
            {
                strSelectedValues = strSelectedValues + item.Value + ",";
            }
        }

        if (strSelectedValues != "")
            strSelectedValues = strSelectedValues.Substring(0, strSelectedValues.Length - 1);
        return strSelectedValues;
    }

    protected void ddlChildTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateChildColumns();
        if (ddlChildTable.SelectedItem != null)
        {
            if (ddlChildTable.SelectedIndex == 0)
            {
                txtDescription.Text = "";
            }
            else
            {
                txtDescription.Text = ddlChildTable.SelectedItem.Text;
            }
        }
    }

}