using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Pages_Record_ViewItemDetail : SecurePage
{

    int? _iViewID = null;
    //int? _iViewItemID = null;
    View _theView = null;


    protected void PopulateTerminology()
    {

        //stgParentFieldCap.InnerText = stgParentFieldCap.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        
        _iViewID=int.Parse( Cryptography.Decrypt(Request.QueryString["ViewID"].ToString()));

        _theView = ViewManager.dbg_View_Detail((int)_iViewID);

        //if (Request.QueryString["ViewItemID"] != null)
        //    _iViewItemID = int.Parse(Cryptography.Decrypt(Request.QueryString["ViewItemID"].ToString()));


        if (!IsPostBack)
        {
          
            //PopulateColumns();
            PopulateListBoxes();


            //lblDetailTitle.Text = _theView.ViewName + " View Items";
          

            PopulateTerminology();
        }


    }


    //protected void PopulateRecord()
    //{
    //    ViewItem theViewItem = ViewManager.dbg_ViewItem_Detail((int)_iViewItemID, null, null);
    //    if (theViewItem != null)
    //    {
    //        nothing to populate as Listboxes are already populated

    //        ddlColumn.SelectedValue = theViewItem.ColumnID.ToString();
    //        txtHeading.Text = theViewItem.Heading;
    //        if (theViewItem.SearchField != null)
    //            chkSearchField.Checked = (bool)theViewItem.SearchField;

    //        if (theViewItem.FilterField != null)
    //            chkFilterField.Checked = (bool)theViewItem.FilterField;

    //        if (theViewItem.Alignment != "")
    //            ddlAlignment.SelectedValue = theViewItem.Alignment;


    //        if (theViewItem.Width != null)
    //            txtWidth.Text = theViewItem.Width.ToString();

    //        if (theViewItem.ShowTotal != null)
    //            chkShowTotal.Checked = (bool)theViewItem.ShowTotal;


            

    //        ViewState["theViewItem"] = theViewItem;

           
            
    //    }
       
    //}


    protected void lnkRemove_Click(object sender, EventArgs e)
    {
        if (lstUsed.SelectedItem != null)
        {
            for (int i = lstUsed.Items.Count - 1; i >= 0; --i)
            {
                if (lstUsed.Items[i].Selected)
                {
                    lstNotUsed.Items.Add(new ListItem(lstUsed.Items[i].Text, lstUsed.Items[i].Value));

                    lstUsed.Items.RemoveAt(i);
                }
            }

        }
        else
        {
            lstNotUsed.Focus();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RemoveMessage", "alert('Please select a column from right side list.');", true);
        }


    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        

        if (lstNotUsed.SelectedItem != null)
        {
            for (int i = lstNotUsed.Items.Count - 1; i >= 0; --i)
            {
                if (lstNotUsed.Items[i].Selected)
                {
                    lstUsed.Items.Add(new ListItem(lstNotUsed.Items[i].Text, lstNotUsed.Items[i].Value));

                    lstNotUsed.Items.RemoveAt(i);
                }
            }

        }
        else
        {
            lstUsed.Focus();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RemoveMessage", "alert('Please select a column from left side list.');", true);
        }



    }


    protected void lnkSaveNew_Click(object sender, EventArgs e)
    {

        //DELETE Removed item.

        foreach (ListItem li in lstNotUsed.Items)
        {
            Common.ExecuteText("DELETE ViewItem WHERE ViewID=" + _iViewID.ToString() + " AND ColumnID=" + li.Value);
        }

        //add new items

        foreach (ListItem li in lstUsed.Items)
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT ViewItemID FROM ViewItem WHERE ViewID=" + _iViewID.ToString() + " AND ColumnID=" + li.Value);

            if (dtTemp.Rows.Count == 0)
            {
                ViewItem newViewItem = new ViewItem(null, _iViewID, int.Parse(li.Value),
              false, false, "left",
              null, false, "");

                string strMaxColumnIndex = Common.GetValueFromSQL("SELECT MAX(ColumnIndex) FROM ViewItem WHERE ViewID=" + _iViewID.ToString());

                if (strMaxColumnIndex == "")
                {
                    strMaxColumnIndex = "-1";
                }

                newViewItem.ColumnIndex = int.Parse(strMaxColumnIndex) + 1;

                ViewManager.dbg_ViewItem_Insert(newViewItem);
            }
        }


        //ViewManager.dbg_View_Default_ResetColumns((int)_iViewID);


        ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);

    }



    //protected void lnkSave_Click(object sender, EventArgs e)
    //{
       



    //    if (ViewState["theViewItem"] == null)
    //    {
    //        //add

    //        if (ddlColumn.SelectedValue != "")
    //        {
    //            //lets get the column

    //            if (ddlColumn.SelectedValue == "")
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a display field.');", true);
    //                return;
    //            }

    //            ViewItem newViewItem = new ViewItem(null, _iViewID, int.Parse(ddlColumn.SelectedValue),
    //          txtHeading.Text,chkSearchField.Checked,chkFilterField.Checked,ddlAlignment.SelectedValue,
    //          txtWidth.Text==""? null:(int?)int.Parse(txtWidth.Text),chkShowTotal.Checked,"");

    //            ViewManager.dbg_ViewItem_Insert(newViewItem, null, null);

    //        }
    //    }
    //    else
    //    {
    //        //edit

    //        if (ddlColumn.SelectedValue != "")
    //        {
    //            ViewItem editViewItem = (ViewItem)ViewState["theViewItem"];

    //            editViewItem.ColumnID = int.Parse(ddlColumn.SelectedValue);
    //            editViewItem.Heading = txtHeading.Text;
    //            editViewItem.SearchField=chkSearchField.Checked;
    //            editViewItem.FilterField = chkFilterField.Checked;
    //            editViewItem.Alignment = ddlAlignment.SelectedValue;
    //            editViewItem.Width = txtWidth.Text == "" ? null : (int?)int.Parse(txtWidth.Text);
    //            editViewItem.ShowTotal = chkShowTotal.Checked;

    //            ViewManager.dbg_ViewItem_Update(editViewItem, null);
    //        }         
            
    //    }

    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);


    //}


    protected void PopulateListBoxes()
    {
        DataTable dtNotUsed = Common.DataTableFromText(@"SELECT ColumnID,DisplayName,Systemname FROM [Column] WHERE 
            Systemname not in('IsActive','TableID') 
            AND ColumnType NOT IN ('staticcontent') AND TableID="+_theView.TableID.ToString()
            + @" AND (Systemname IN ('RecordID') OR (DisplayTextSummary IS NOT NULL AND LEN(DisplayTextSummary)>0)) AND ColumnID NOT IN(SELECT ColumnID FROM ViewItem WHERE ViewID=" + _theView.ViewID.ToString() + @") ORDER BY DisplayName ASC");


        lstNotUsed.Items.Clear();

        foreach (DataRow dr in dtNotUsed.Rows)
        {
            ListItem liTemp = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
            lstNotUsed.Items.Add(liTemp);
        }

        
        DataTable dtUsed = Common.DataTableFromText(@"SELECT ColumnID,DisplayName,Systemname FROM [Column] WHERE 
            Systemname not in('IsActive','TableID') 
            AND ColumnType NOT IN ('staticcontent') AND TableID=" + _theView.TableID.ToString()
    + @" AND ColumnID IN(SELECT ColumnID FROM ViewItem WHERE ViewID=" + _theView.ViewID.ToString() + @") ORDER BY DisplayName ASC");

        lstUsed.Items.Clear();

        foreach (DataRow dr in dtUsed.Rows)
        {
            ListItem liTemp = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
            lstUsed.Items.Add(liTemp);
        }



    }

//    protected void PopulateColumns()
//    {
//        ddlColumn.Items.Clear();


//        ddlColumn.DataSource = Common.DataTableFromText(@"SELECT ColumnID,DisplayName,Systemname FROM [Column] WHERE 
//                Systemname not in('IsActive','EnteredBy','TableID') 
//                AND ColumnType NOT IN ('staticcontent') AND TableID=" + _theView.TableID.ToString());
        
//        ddlColumn.DataBind();

//        ListItem liSelect = new ListItem("--Please Select--", "");
//        ddlColumn.Items.Insert(0, liSelect);
//    }


}