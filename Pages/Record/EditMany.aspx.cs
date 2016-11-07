using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using System.Data;
public partial class Pages_Record_EditMany :SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string strParentObject = "";
            if (Request.QueryString["sc_id"] != null)
            {
                SearchCriteria theSC = SystemData.SearchCriteria_Detail(int.Parse(Request.QueryString["sc_id"].ToString()));
                if (theSC != null)
                {
                    System.Xml.XmlDocument xmlSC_Doc = new System.Xml.XmlDocument();

                    xmlSC_Doc.Load(new StringReader(theSC.SearchText));

                    strParentObject = strParentObject + " var p_lnkEditManyOK = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["lnkEditManyOK"].InnerText + @"');";
                    strParentObject = strParentObject + " var p_chkAll = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["chkAll"].InnerText + @"');";
                    ViewState["TableID"] = xmlSC_Doc.FirstChild["TableID"].InnerText;
                    ViewState["ViewID"] = xmlSC_Doc.FirstChild["ViewID"].InnerText;
                    ViewState["TableName"] = xmlSC_Doc.FirstChild["TableName"].InnerText;
                    lblTitle.Text = ViewState["TableName"].ToString() + " - Update Multiple";
                    ViewState["_strRecordRightID"] = xmlSC_Doc.FirstChild["RecordRightID"].InnerText;
                    ViewState["_strDynamictabPart"] = xmlSC_Doc.FirstChild["DynamictabPart"].InnerText;

                    strParentObject = strParentObject + " var p_hfddlYAxisBulk = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["hfddlYAxisBulk"].InnerText + @"');";
                    strParentObject = strParentObject + " var p_hfBulkValue = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["hfBulkValue"].InnerText + @"');";
                    strParentObject = strParentObject + " var p_hfchkUpdateEveryItem = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["hfchkUpdateEveryItem"].InnerText + @"');";

                    PopulateYAxisBulk(int.Parse(ViewState["TableID"].ToString()), int.Parse(ViewState["ViewID"].ToString()));
                }
            }
            PopulateTerminology();

            ViewState["strParentObject"] = strParentObject;
            string strXX = @"
                       

                         $(document).ready(function () {


  
                                " + strParentObject + @"

                                    function EditManyCheckAll()
                                    {
                                       if(p_chkAll!=null )
                                        {
                                            if(p_chkAll.checked)
                                                {
                                                    $('#" + trUpdateEveryItem.ClientID + @"').fadeIn();
                                                     $('#" + hfChkAll.ClientID + @"').value='yes';
                                                }
                                            else
                                                {
                                                    $('#" + trUpdateEveryItem.ClientID + @"').fadeOut();
                                                }
                                        }


                                    }

                                   EditManyCheckAll();

                           });
                               
                ";
            ViewState["strXX"] = strXX;
        }


        ScriptManager.RegisterStartupScript(upMain, upMain.GetType(), "emJSCode" + ViewState["_strDynamictabPart"].ToString(), ViewState["strXX"].ToString(), true);

    }

    protected void lnkEditManyOK_Click(object sender, EventArgs e)
    {
        hfEditManyValue.Value = "";
        string strValue = "";
        if (ddlYAxisBulk.SelectedValue == "")
        {
            Session["tdbmsgpb"] = "Please select a column.";

            return;
        }
        else
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlYAxisBulk.SelectedValue));

            if (theColumn.ColumnType == "checkbox")
            {
                strValue = Common.GetCheckBoxValue(theColumn.DropdownValues, ref chkCheckboxBulk);
            }
            else if (theColumn.ColumnType == "number")
            {
                strValue = txtNumberBulk.Text;
            }
            else if (theColumn.ColumnType == "text")
            {
                strValue = txtTextBulk.Text;
            }
            else if (theColumn.ColumnType == "date")
            {
                strValue = txtDateBulk.Text;
            }
            else if (theColumn.ColumnType == "datetime")
            {
                try
                {
                    string strDateTime = "";
                    if (txtDateBulk.Text.Trim() == "")
                    {
                        //strDateTime = DateTime.Now.ToShortDateString() + " 12:00:00 AM";
                    }
                    else
                    {
                        DateTime dtTemp;
                        if (DateTime.TryParseExact(txtDateBulk.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                        {
                            txtDateBulk.Text = dtTemp.ToShortDateString();
                        }
                        string strTimePart = "";
                        if (txtBulkTime != null)
                        {
                            if (txtBulkTime.Text == "")
                            {
                                strTimePart = "00:00";
                            }
                            else
                            {
                                if (txtBulkTime.Text.ToLower().IndexOf(":am") > 0)
                                {
                                    strTimePart = txtBulkTime.Text.ToLower().Replace(":am", ":00");
                                }
                                else
                                {
                                    strTimePart = txtBulkTime.Text.ToLower().Replace(":pm", ":00");
                                }
                            }
                        }
                        else
                        {
                            strTimePart = "00:00";
                        }

                        strDateTime = txtDateBulk.Text + " " + strTimePart;
                        strDateTime = strDateTime.Replace("  ", " ");
                    }
                    strValue = strDateTime;
                }
                catch
                {
                    //
                    strValue = "";
                }
            }
            else if (theColumn.ColumnType == "dropdown")
            {
                strValue = ddlDropdownBulk.SelectedValue;
            }

            if (strValue == "")
            {
                Session["tdbmsgpb"] = "Please enter the new value.";
                //mpeEditMany.Show();
                return;
            }

            hfEditManyValue.Value = strValue;

            string strJS = @"

                         $(document).ready(function () {

                                " + ViewState["strParentObject"].ToString() + @"

                                p_hfddlYAxisBulk.value=document.getElementById('" + ddlYAxisBulk.ClientID + @"').value;
                                    p_hfBulkValue.value=document.getElementById('" + hfEditManyValue.ClientID + @"').value;
                                p_hfchkUpdateEveryItem.value=document.getElementById('" + chkUpdateEveryItem.ClientID + @"').checked.toString();
                                $(p_lnkEditManyOK).trigger('click');parent.$.fancybox.close();
                                            
                                    

                           });
                    ";
            if(IsPostBack)
                ScriptManager.RegisterStartupScript(upMain, upMain.GetType(), "doneEditMany" + ViewState["_strDynamictabPart"].ToString(), strJS, true);
        }
    }
    protected void PopulateTerminology()
    {

        stgFieldToUpdate.InnerText = stgFieldToUpdate.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));


    }
    protected void ddlYAxisBulk_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtNumberBulk.Visible = false;
        txtNumberBulk.Text = "";
        txtTextBulk.Visible = false;
        txtTextBulk.Text = "";
        txtDateBulk.Visible = false;
        txtDateBulk.Text = "";
        ibBulkDate.Visible = false;
        txtBulkTime.Visible = false;
        txtBulkTime.Text = "";
        ddlDropdownBulk.Visible = false;
        chkCheckboxBulk.Visible = false;
        chkCheckboxBulk.Checked = false;

        if (ddlDropdownBulk.Items.Count > 0)
            ddlDropdownBulk.SelectedIndex = 0;

        if (ddlYAxisBulk.SelectedValue == "")
        {
            //
        }
        else
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlYAxisBulk.SelectedValue));

            if (theColumn.ColumnType == "checkbox")
            {
                chkCheckboxBulk.Visible = true;
            }
            else if (theColumn.ColumnType == "number")
            {
                txtNumberBulk.Visible = true;

            }
            else if (theColumn.ColumnType == "text")
            {
                txtTextBulk.Visible = true;

            }
            else if (theColumn.ColumnType == "date")
            {
                txtDateBulk.Visible = true;
                ibBulkDate.Visible = true;

            }
            else if (theColumn.ColumnType == "datetime")
            {
                txtDateBulk.Visible = true;
                ibBulkDate.Visible = true;
                txtBulkTime.Visible = true;

            }
            else if (theColumn.ColumnType == "dropdown")
            {

                ddlDropdownBulk.Visible = true;

                if (theColumn.DropDownType == "values")
                {
                    Common.PutDDLValues(theColumn.DropdownValues, ref ddlDropdownBulk);
                }
                else if (theColumn.DropDownType == "value_text")
                {
                    Common.PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownBulk);
                }
                else if ((theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd")
                    && theColumn.ParentColumnID == null)
                {
                    ddlDropdownBulk.Items.Clear();
                    RecordManager.PopulateTableDropDown((int)theColumn.ColumnID, ref ddlDropdownBulk);
                    // PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownBulk);
                }
                else
                {
                    ddlDropdownBulk.Items.Clear();
                }

            }
        }

        //if(!IsPostBack)
        //{
        //   if(hfChkAll.Value=="yes")
        //   {
        //       trUpdateEveryItem.Visible = true;
        //   }
        //   else
        //   {
        //       trUpdateEveryItem.Visible = false;
        //   }
        //}

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "emJSCode" + ViewState["_strDynamictabPart"].ToString(), "EditManyCheckAll();", true);

    }

    protected void PopulateYAxisBulk(int TableID, int ViewID)
    {

        DataTable dtSCs = RecordManager.ets_Table_Columns_Summary(TableID, ViewID);

        foreach (DataRow dr in dtSCs.Rows)
        {
            if (bool.Parse(dr["IsStandard"].ToString()) == false && dr["IsReadOnly"].ToString().ToLower() != "true")
            {
                if (dr["ColumnType"].ToString() == "text" || dr["ColumnType"].ToString() == "checkbox"
                    || dr["ColumnType"].ToString() == "number"
                 || dr["ColumnType"].ToString() == "date" || dr["ColumnType"].ToString() == "datetime"
                    || (dr["ColumnType"].ToString() == "dropdown" && dr["ParentColumnID"] == DBNull.Value))
                {
                    System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(dr["Heading"].ToString(), dr["ColumnID"].ToString());

                    ddlYAxisBulk.Items.Insert(ddlYAxisBulk.Items.Count, aItem);
                }
            }

        }

        System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("-- None --", "");

        ddlYAxisBulk.Items.Insert(0, fItem);

    }

}