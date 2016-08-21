using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Record_TableFields :Page
{
    int _iTableID = -1;

    protected void PopulateTerminology()
    {
        lblTitle.Text = "Add " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Fields", "Fields");
        grdFields.Columns[0].HeaderText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field") +  " Name";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        _iTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));

        if (!IsPostBack)
        {
            PopulateTerminology();
            PopulateHelp("Table-Fields-New");
       
            Session["Fields"] = null;
            if (Session["Fields"] == null)
            {
                DataTable dtFields = new DataTable();
                dtFields.Columns.Add("DisplayName");
                dtFields.Columns.Add("ColumnType");
                dtFields.Rows.Add("", "text");
                dtFields.AcceptChanges();
                grdFields.DataSource = dtFields;
                grdFields.DataBind();
                Session["Fields"] = dtFields;
            }

        }

    }
    protected void PopulateHelp(string strContentKey)
    {
        Content theContent = SystemData.Content_Details_ByKey(strContentKey, null);

        if (theContent != null)
        {
            lblHelpContent.Text = theContent.ContentP;
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {

        
           
            for (int i = 0; i < grdFields.Rows.Count; i++)
            {
                string strDisplayName = ((TextBox)grdFields.Rows[i].FindControl("txtDisplayName")).Text;
                string strColumnType = ((DropDownList)grdFields.Rows[i].FindControl("ddlType")).SelectedValue;

                if (strDisplayName != "")
                {
                    string strAutoSystemName = "";

                    strAutoSystemName = RecordManager.ets_Column_NextSystemName(_iTableID);
                    if (strAutoSystemName == "NO")
                    {
                        lblMsg.Text = "You can add 100 columns, there is no column available!";
                        return;
                    }
                    

                    int? iDisplayOrder = RecordManager.ets_Table_MaxOrder(_iTableID);

                    if (iDisplayOrder == null)
                        iDisplayOrder = -1;                   

                    Column newColumn = new Column(null, _iTableID,
                   strAutoSystemName, iDisplayOrder + 1, strDisplayName, strDisplayName, "", "", null, "",
                    "", null, null, "", "", false, strDisplayName, null, "", null, null, false, "", "", "");

                    newColumn.ColumnType = strColumnType;


                    switch (newColumn.ColumnType)
                    {
                        case "checkbox":
                            newColumn.DropdownValues = "Yes" + Environment.NewLine + "No"
                   + Environment.NewLine +  "no";
                            break;
                        case "staticcontent":
                            newColumn.DisplayTextSummary = "";
                            newColumn.DropdownValues = "Content goes here";
                            break;
                        case "content":
                            newColumn.DisplayTextSummary = "";
                            break;
                        case "date_time":
                            newColumn.ColumnType = "date";
                            break;
                        case "dropdown":
                            newColumn.DropDownType = "values";
                            newColumn.DropdownValues = "Item 1" + Environment.NewLine + "Item 2"
                  + Environment.NewLine + "Item 3";
                            break;
                        case "file":
                            //ok
                            break;
                        case "image":
                            //ok
                            break;
                        case "listbox":
                            newColumn.DropdownValues = "Item 1" + Environment.NewLine + "Item 2"
                + Environment.NewLine + "Item 3";
                            newColumn.DropDownType = "values";
                            break;
                        case "location":
                            newColumn.ShowTotal= true;
                            newColumn.IsRound = true;
                            newColumn.TextHeight =200;                        
                            newColumn.TextWidth = 400;
                            break;
                        case "number":
                            newColumn.NumberType = 1;
                            break;
                        case "radiobutton":
                            newColumn.DropdownValues = "Option 1" + Environment.NewLine + "Option 2"
              + Environment.NewLine + "Option 3";
                            newColumn.DropDownType = "values";
                            break;
                        case "text":
                            //
                            break;
                      
                    }

                    try
                    {
                        RecordManager.ets_Column_Insert(newColumn);
                    }
                    catch
                    {
                        //
                    }



                }
            }


        Session["Fields"] = null;
        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&MenuID=" + Request.QueryString["MenuID"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString(), false);

    }
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        DataTable dtFields = new DataTable();
        dtFields.Columns.Add("DisplayName");
        dtFields.Columns.Add("ColumnType");

        for (int i = 0; i < grdFields.Rows.Count; i++)
        {
            string strDisplayName = ((TextBox)grdFields.Rows[i].FindControl("txtDisplayName")).Text;
            string strColumnType = ((DropDownList)grdFields.Rows[i].FindControl("ddlType")).SelectedValue;
            if (strDisplayName != "")
            {
                dtFields.Rows.Add(strDisplayName, strColumnType);
                
            }
        }


        dtFields.Rows.Add("", "text");
        dtFields.AcceptChanges();
        grdFields.DataSource = dtFields;
        grdFields.DataBind();
        Session["Fields"] = dtFields;

    }

    protected void grdFields_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtDisplayName = (TextBox)e.Row.FindControl("txtDisplayName");
            txtDisplayName.Text = DataBinder.Eval(e.Row.DataItem, "DisplayName").ToString();
            

            DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlType");
            ddlType.Text = DataBinder.Eval(e.Row.DataItem, "ColumnType").ToString();
        }
    }
}