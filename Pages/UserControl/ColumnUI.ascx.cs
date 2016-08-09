using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_UserControl_ColumnUI : System.Web.UI.UserControl
{
    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";
    public int? ColumnID
    {
        get
        {
            if (ViewState["theColumn"]==null)
            {
                return null;
            }
            else
            {
                Column theColumn = (Column)ViewState["theColumn"];
                return theColumn.ColumnID;
            }
           
        }
        set
        {
            if(value!=null)
            {
                Column theColumn = RecordManager.ets_Column_Details((int)value);

                if (theColumn != null)
                {
                    ViewState["theColumn"] = theColumn;
                    ShowColumnControls();
                }
                else
                {
                    HideAndDisableAllControls();
                }
                   
            }
            else
            {
                HideAndDisableAllControls();
            }
            


        }
    }

    public string ColumnValue
    {
        get
        {
            return GetColumnInputValue();
        }
        set
        {
            if (value!=null)
                SetColumnValue(value);
        }

    }

    public bool ShowControls
    {
        get
        {
            return pnlUIMain.Visible;
        }
        set
        {
            if (value != null)
                pnlUIMain.Visible=value;
        }

    }
    protected void  SetColumnValue(string strValue)
    {
        try
        {
            if (ViewState["theColumn"] != null && strValue.Trim() != "")
            {
                Column theColumn = (Column)ViewState["theColumn"];
                pnlUIMain.Visible = true;
                if (theColumn != null)
                {
                    switch (theColumn.ColumnType)
                    {
                        case "listbox":

                            if (theColumn.DateCalculationType == "")
                            {

                                if (theColumn.DropDownType == "values")
                                {
                                    Common.SetListValues(strValue, ref lstListbox, theColumn.DropdownValues);
                                }
                                else if (theColumn.DropDownType == "value_text")
                                {
                                    Common.SetListValues_Text(strValue, ref lstListbox, theColumn.DropdownValues);
                                }
                                else
                                {
                                    if (theColumn.DropDownType == "table" && theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                                    {
                                        Common.SetListValues_ForTable(strValue, ref lstListbox, (int)theColumn.TableTableID,
                                            theColumn.LinkedParentColumnID, theColumn.DisplayColumn);
                                    }

                                }

                            }
                            if (theColumn.DateCalculationType == "checkbox")
                            {
                                cblListbox.Visible = true;

                                if (theColumn.DropDownType == "values")
                                {
                                    Common.SetCheckBoxListValues(strValue, ref cblListbox, theColumn.DropdownValues);
                                }
                                else if (theColumn.DropDownType == "value_text")
                                {
                                    Common.SetCheckBoxListValues_Text(strValue, ref cblListbox, theColumn.DropdownValues);
                                }
                                else
                                {
                                    if (theColumn.DropDownType == "table" && theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                                    {
                                        Common.SetCheckBoxListValues_ForTable(strValue, ref cblListbox, (int)theColumn.TableTableID,
                                            theColumn.LinkedParentColumnID, theColumn.DisplayColumn);
                                    }

                                }

                            }

                            break;
                        case "radiobutton":

                            radioRadiobutton.SelectedValue = strValue;


                            break;
                        case "checkbox":
                            Common.SetCheckBoxValue(theColumn.DropdownValues, strValue, ref chkCheckbox);
                            break;
                        case "number":
                            txtNumber.Text = strValue;
                            break;
                        case "datetime":

                            DateTime dtTempDateTime = DateTime.Parse(strValue);
                            txtDTDate.Text = dtTempDateTime.Day.ToString("00") + "/" + dtTempDateTime.Month.ToString("00") + "/" + dtTempDateTime.Year.ToString();
                            txtDTTime.Text = Convert.ToDateTime(dtTempDateTime.ToString()).ToString("HH:m");

                            break;

                        case "date":
                            DateTime dtTempDate = DateTime.Parse(strValue);
                            txtDate.Text = dtTempDate.Day.ToString() + "/" + dtTempDate.Month.ToString("00") + "/" + dtTempDate.Year.ToString();

                            break;

                        case "time":
                            txtTime.Text = Convert.ToDateTime(strValue).ToString("HH:m");
                            break;

                        case "dropdown":
                            ddlDropdownCommon.SelectedValue = strValue;
                            break;

                        default:
                            txtTextCommon.Text = strValue;
                            break;
                    }

                }
            }
            else  if (ViewState["theColumn"] != null && strValue.Trim() == "")
            {
                Column theColumn = (Column)ViewState["theColumn"];
                pnlUIMain.Visible = false;
                if (theColumn != null)
                {
                    switch (theColumn.ColumnType)
                    {
                        case "listbox":
                            if (theColumn.DateCalculationType == "")
                            {
                                lstListbox.ClearSelection();
                            }
                            if (theColumn.DateCalculationType == "checkbox")
                            {
                                cblListbox.Visible = true;
                                cblListbox.ClearSelection();
                            }

                            break;
                        case "radiobutton":
                            radioRadiobutton.ClearSelection();                         
                            
                            break;
                        case "checkbox":
                            chkCheckbox.Checked = false;
                            break;
                        case "number":
                            txtNumber.Text = strValue;
                            break;
                        case "datetime":                       
                            txtDTDate.Text = "";
                            txtDTTime.Text = "";
                            break;

                        case "date":                           
                            txtDate.Text = "";
                            break;

                        case "time":
                            txtTime.Text = "";
                            break;

                        case "dropdown":
                            ddlDropdownCommon.ClearSelection();
                            break;

                        default:
                            txtTextCommon.Text = "";
                            break;
                    }

                }
            }
        }
        catch
        {

        }

        

        
    }
    protected string GetColumnInputValue()
    {
        string strInputValue = "";

        try
        {

            if (ViewState["theColumn"] != null)
            {
                Column theColumn = (Column)ViewState["theColumn"];

                if (theColumn != null)
                {
                    switch (theColumn.ColumnType)
                    {
                        case "listbox":

                            if (theColumn.DateCalculationType == "")
                            {
                                strInputValue = Common.GetListValues(lstListbox);

                            }
                            if (theColumn.DateCalculationType == "checkbox")
                            {
                                strInputValue = Common.GetCheckBoxListValues(cblListbox);

                            }

                            break;
                        case "radiobutton":

                            if (radioRadiobutton.SelectedItem != null)
                                strInputValue = radioRadiobutton.SelectedItem.Value;

                            break;
                        case "checkbox":
                            strInputValue = Common.GetCheckBoxValue(theColumn.DropdownValues, ref chkCheckbox);
                            break;
                        case "number":
                            strInputValue = txtNumber.Text;
                            break;
                        case "datetime":
                            strInputValue = Common.GetDateTimeFromDnT(txtDTDate.Text, txtDTTime.Text);
                            break;

                        case "date":
                            strInputValue = Common.GetDatestringFromD(txtDate.Text);
                            break;

                        case "time":
                            strInputValue = txtTime.Text.Trim();
                            break;

                        case "dropdown":
                            strInputValue = ddlDropdownCommon.SelectedValue;
                            break;

                        default:
                            strInputValue = txtTextCommon.Text.Trim();
                            break;
                    }

                }
            }
        }
        catch
        {
            //
        }

        return strInputValue;
    }
    protected void ShowColumnControls()
    {
        if(ViewState["theColumn"]!=null)
        {
            Column theColumn = (Column)ViewState["theColumn"];
            pnlUIMain.Visible = true;
            if(theColumn!=null)
            {
                HideAndDisableAllControls();

                switch (theColumn.ColumnType)
                {
                    case "listbox":
                       
                        if(theColumn.DateCalculationType=="")
                        {
                            lstListbox.Visible = true;
                            if (theColumn.DropDownType == "values")
                            {
                                Common.PutListValues(theColumn.DropdownValues, ref lstListbox);
                            }
                            else if (theColumn.DropDownType == "value_text")
                            {
                                Common.PutListValues_Text(theColumn.DropdownValues, ref lstListbox);
                            }
                            else
                            {
                                if (theColumn.DropDownType == "table" && theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                                {
                                    Common.PutList_FromTable((int)theColumn.TableTableID, null, theColumn.DisplayColumn,
                                        ref lstListbox);
                                }

                            }

                        }
                        if (theColumn.DateCalculationType == "checkbox")
                        {
                            cblListbox.Visible = true;
                            if (theColumn.DropDownType == "values")
                            {
                                Common.PutCheckBoxListValues(theColumn.DropdownValues, ref cblListbox);
                            }
                            else if (theColumn.DropDownType == "value_text")
                            {
                                Common.PutCheckBoxListValues_Text(theColumn.DropdownValues, ref cblListbox);
                            }
                            else
                            {
                                if (theColumn.DropDownType == "table" && theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                                {
                                    Common.PutCheckBoxList_ForTable((int)theColumn.TableTableID, null, theColumn.DisplayColumn,
                                        ref cblListbox);
                                }

                            }

                        }
                       
                       
                        break;
                    case "radiobutton":

                        radioRadiobutton.Visible = true;
                        if (theColumn.DropDownType == "values")
                        {
                            Common.PutRadioList(theColumn.DropdownValues, ref radioRadiobutton);
                        }
                        else if (theColumn.DropDownType == "value_text")
                        {
                            Common.PutRadioListValue_Text(theColumn.DropdownValues, ref radioRadiobutton);
                        }
                        else
                        {
                            Common.PutRadioListValue_Image(theColumn.DropdownValues, ref radioRadiobutton, _strFilesLocation);
                        }
                        
                        break;
                    case "checkbox":
                        chkCheckbox.Visible = true;
                        Common.PutCheckBoxDefault(theColumn.DropdownValues, ref chkCheckbox);
                        break;
                    case "number":
                        txtNumber.Visible = true;
                        revNumber.Enabled = true;                      
                        break;
                    case "datetime":
                        divDateTime.Visible = true;
                        ceDTDate.Enabled = true;
                        meeDTTime.Enabled = true;
                        break;

                    case "date":
                        divDate.Visible = true;
                        ceDate.Enabled = true;                      
                        break;

                    case "time":
                        txtTime.Visible = true;
                        meeTime.Enabled = true;
                        break;

                    case "dropdown":
                        ddlDropdownCommon.Visible = true;
                        ddlDropdownCommon.Items.Clear();
                        if (theColumn.DropDownType == "values")
                        {
                            Common.PutDDLValues(theColumn.DropdownValues, ref ddlDropdownCommon);
                          
                        }
                        else if (theColumn.DropDownType == "value_text")
                        {
                            Common.PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownCommon);
                           
                        }
                        else
                        {                         
                            RecordManager.PopulateTableDropDown((int)theColumn.ColumnID, ref ddlDropdownCommon);                           
                        }
                        break;

                    default:
                        txtTextCommon.Visible = true;                        
                        break;
                }

            }
        }
    }

    protected void HideAndDisableAllControls()
    {

        txtTextCommon.Visible = false;
        txtNumber.Visible = false;

        divDateTime.Visible = false;
        divDate.Visible = false;
        txtTime.Visible = false;

        lstListbox.Visible = false;
        cblListbox.Visible = false;

        ddlDropdownCommon.Visible = false;
        chkCheckbox.Visible = false;
        radioRadiobutton.Visible = false;


        revNumber.Enabled = false;

        ceDTDate.Enabled = false;
        meeDTTime.Enabled = false;

        ceDate.Enabled = false;
        meeTime.Enabled = false;


    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        _strFilesLocation = Session["FilesLocation"].ToString();
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();
    }
}