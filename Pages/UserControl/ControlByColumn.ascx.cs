//using System;
//using System.Collections.Generic;
////using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Data;
//using System.Data.SqlClient;
//using System.Globalization;
//using System.Text.RegularExpressions;

using System;
//using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
public partial class Pages_UserControl_ControlByColumn : System.Web.UI.UserControl
{
    public string _strTextSearch="";
    //public string _strNumericSearch;
    public int? TableID { get; set; }
    public int? ViewID { get; set; }
    public event EventHandler ddlYAxis_Changed;

    public event EventHandler ddlCompareOperator_Changed;

    private string _strTodayShortDate = DateTime.Today.ToShortDateString();

    //public string NumericSearch //need to remove this
    //{
    //    get
    //    {
    //        PopulateSearchParams();
    //        return _strNumericSearch;
    //    }
    //    set
    //    {
    //        _strNumericSearch = value;

    //    }
    //}
    public string TextSearch
    {
        get
        {
            PopulateSearchParams();
            return _strTextSearch;
        }
        set
        {
            _strTextSearch = value;
            
        }
    }

    public string ddlYAxisClientID
    {
        get
        {            
           
            return ddlYAxis.ClientID;
        }
       
    }



    public bool ShowColumnDDL
    {

        get
        {
            return divYAxis.Visible;
        }
        set
        {
            divYAxis.Visible = value;

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

    public void ClearYAxis()
    {
        ddlYAxis.Items.Clear();

    }

    public string txtUpperLimitV
    {
        get
        {

            return txtUpperLimit.Text;
        }
        set
        {
           
                txtUpperLimit.Text = value;

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

    public string hfTextSearchV
    {
        get
        {
            return hfTextSearch.Value;
        }
        set
        {
            hfTextSearch.Value = value;
        }

    }

    public string ColumnTypeOut
    {
        get
        {
            return hfColumnTypeOut.Value;
        }
        set
        {
            hfColumnTypeOut.Value = value;
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

    public string txtUpperDateV
    {
        get
        {
            return txtUpperDate.Text;
        }
        set
        {
            txtUpperDate.Text = value;
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

    public string CompareOperator
    {
        get
        {
            return ddlCompareOperator.SelectedValue;
        }
        set
        {
            if (ddlCompareOperator.Items.FindByValue(value)!=null)
                ddlCompareOperator.SelectedValue = value;
        }

    }

    public string TextValue
    {

        get
        {

            string strValue = "";
            if (ddlYAxis.SelectedValue == "")
            {
                return strValue;
            }
            else
            {
                int iC = int.Parse(ddlYAxis.SelectedValue);
                if (iC < 0)
                {
                    return strValue;
                }
                else
                {
                    if (ddlCompareOperator.SelectedValue == "empty" || ddlCompareOperator.SelectedValue == "notempty")
                    {
                        strValue = "";
                        return strValue;
                    }


                    Column theColumn = RecordManager.ets_Column_Details(iC);
                    if ((theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false)|| theColumn.ColumnType=="calculation")
                    {
                        strValue= txtLowerLimit.Text + "____" + txtUpperLimit.Text;
                    }
                    else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
                    {
                        if (ddlDropdownColumnSearch.SelectedItem != null && ddlDropdownColumnSearch.SelectedValue != "")
                            strValue= ddlDropdownColumnSearch.SelectedValue;
                    }
                    //else if (theColumn.ColumnType == "calculation")
                    //{
                    //    return strValue;
                    //}
                    else if (theColumn.ColumnType == "date" )
                    {
                        txtLowerDate.Text = txtLowerDate.Text.Replace(" ", "");
                        txtUpperDate.Text = txtUpperDate.Text.Replace(" ", "");

                        strValue= txtLowerDate.Text + "____" + txtUpperDate.Text;
                    }
                    else if ( theColumn.ColumnType == "datetime")
                    {
                        txtLowerDate.Text = txtLowerDate.Text.Replace(" ", "");
                        txtUpperDate.Text = txtUpperDate.Text.Replace(" ", "");

                        strValue = (txtLowerDate.Text.Trim()==""?"":( txtLowerDate.Text + (txtLowerTime.Text.Trim()==""?" 00:00":" " + txtLowerTime.Text))) +  "____" 
                            + (txtUpperDate.Text.Trim()==""?"":( txtUpperDate.Text + (txtUpperTime.Text.Trim()==""?" 23:59":" " + txtUpperTime.Text)));
                    }
                    else if (theColumn.ColumnType == "time" )
                    {
                        strValue = txtLowerTime.Text + "____" + txtUpperTime.Text;
                    }
                    else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
              theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                    {
                        if (ddlDropdownColumnSearch.SelectedItem != null && ddlDropdownColumnSearch.SelectedValue != "")
                            strValue= ddlDropdownColumnSearch.SelectedValue;
                    }
                    else if (theColumn.ColumnType == "radiobutton" || theColumn.ColumnType == "checkbox"
                        || theColumn.ColumnType == "listbox")
                    {
                        if (ddlDropdownColumnSearch.SelectedItem != null && ddlDropdownColumnSearch.SelectedValue != "")
                            strValue = ddlDropdownColumnSearch.SelectedValue;
                    }
                    else
                    {
                        strValue= txtSearchText.Text;
                    }


                }
            }


            if (strValue.Trim() == "____")
            {
                strValue = "";
            }
            return strValue;

        }
        set
        {
            if (ddlYAxis.SelectedValue == "")
            {
               //
               
            }
            else
            {
                int iC = int.Parse(ddlYAxis.SelectedValue);
                if (iC < 0)
                {
                   //
                }
                else
                {
                    string strValue = value;

                    

                    Column theColumn = RecordManager.ets_Column_Details(iC);
                    if (strValue == "")
                    {
                        HideResetAllControls();
                        if (CompareOperator != null && CompareOperator != "")
                            ddlCompareOperator.SelectedValue = CompareOperator;

                    }
                    else if ((theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false) || theColumn.ColumnType=="calculation")
                    {
                       
                        if (strValue.IndexOf("____") > -1)
                        {
                            txtLowerLimit.Text = strValue.Substring(0, strValue.IndexOf("____"));
                            txtUpperLimit.Text = strValue.Substring(strValue.IndexOf("____") + 4);
                            if (txtUpperLimit.Text.Trim() != "")
                            {
                                ddlCompareOperator.SelectedValue = "between";
                                ddlCompareOperator_SelectedIndexChanged(null, null);
                            }
                               
                        }
                    }
                    else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
                    {
                        if (ddlDropdownColumnSearch.Items.FindByValue(strValue) != null)
                            ddlDropdownColumnSearch.SelectedValue = strValue;
                    }
                    //else if (theColumn.ColumnType == "calculation")
                    //{
                    //   //
                    //}
                    else if (theColumn.ColumnType == "date")
                    {
                       
                        if (strValue.IndexOf("____") > -1)
                        {
                            txtLowerDate.Text = strValue.Substring(0, strValue.IndexOf("____"));
                            txtUpperDate.Text = strValue.Substring(strValue.IndexOf("____") + 4);

                            if (txtUpperDate.Text.Trim() != "")
                            {
                                ddlCompareOperator.SelectedValue = "between";
                                ddlCompareOperator_SelectedIndexChanged(null, null);
                            }
                        }
                    }
                    else if (theColumn.ColumnType == "datetime")
                    {

                        if (strValue.IndexOf("____") > -1)
                        {
                           string strLowerDatetime = strValue.Substring(0, strValue.IndexOf("____"));
                           string strUpperDateTime = strValue.Substring(strValue.IndexOf("____") + 4);

                            if(strLowerDatetime.IndexOf(" ")>-1)
                            {
                                txtLowerDate.Text = strLowerDatetime.Substring(0, strLowerDatetime.IndexOf(" "));
                                txtLowerTime.Text = strLowerDatetime.Substring(strLowerDatetime.IndexOf(" ") + 1);
                            }

                            if (strUpperDateTime.IndexOf(" ") > -1)
                            {
                                txtUpperDate.Text = strUpperDateTime.Substring(0, strUpperDateTime.IndexOf(" "));
                                txtUpperTime.Text = strUpperDateTime.Substring(strUpperDateTime.IndexOf(" ") + 1);

                                if (txtUpperDate.Text.Trim() != "")
                                {
                                    ddlCompareOperator.SelectedValue = "between";
                                    ddlCompareOperator_SelectedIndexChanged(null, null);
                                }
                            }
                        }
                    }
                    else if (theColumn.ColumnType == "time" )
                    {

                        if (strValue.IndexOf("____") > -1)
                        {
                            txtLowerTime.Text = strValue.Substring(0, strValue.IndexOf("____"));
                            txtUpperTime.Text = strValue.Substring(strValue.IndexOf("____") + 4);

                            if (txtUpperTime.Text.Trim() != "")
                            {
                                ddlCompareOperator.SelectedValue = "between";
                                ddlCompareOperator_SelectedIndexChanged(null, null);
                            }
                        }
                    }
                    else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
              theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                    {
                        if (ddlDropdownColumnSearch.Items.FindByValue(strValue) != null)
                            ddlDropdownColumnSearch.SelectedValue = strValue;
                    }
                    else if (theColumn.ColumnType == "radiobutton" || theColumn.ColumnType == "checkbox"
                  || theColumn.ColumnType == "listbox")
                    {
                        if (ddlDropdownColumnSearch.Items.FindByValue(strValue)!=null)
                            ddlDropdownColumnSearch.SelectedValue = strValue;
                    }
                    else
                    {
                        txtSearchText.Text=strValue;
                    }


                }
            }
        }

    }

   protected void PopulateComparators()
    {
        ddlCompareOperator.Items.Clear();

        ddlCompareOperator.Items.Add("=");

        bool bIsDropdown = false;

        if (ddlYAxis.SelectedItem!=null && ddlYAxis.SelectedValue!="")
        {
            int iC = int.Parse(ddlYAxis.SelectedValue);
             if (iC > 0)
             {
                 Column theColumn = RecordManager.ets_Column_Details(iC);
                 if (theColumn.ColumnType == "dropdown")
                     bIsDropdown = true;
             }

        }

        if (bIsDropdown)
       {

       }
       else
       {
           ddlCompareOperator.Items.Add(">");
           ddlCompareOperator.Items.Add("<");
           ListItem liBetween = new ListItem("Between", "between");
           ddlCompareOperator.Items.Add(liBetween);
       }

       ListItem liempty = new ListItem("Is Empty", "empty");
       ddlCompareOperator.Items.Add(liempty);

       ListItem linotempty = new ListItem("Is Not Empty", "notempty");
       ddlCompareOperator.Items.Add(linotempty);

       ListItem liNotEqual = new ListItem("Not Equal", "<>");
       ddlCompareOperator.Items.Add(liNotEqual);


    }


    public void PopulateSearchParams()
    {
        string strCompareOperator = "=";

        if(ddlCompareOperator.SelectedValue!="between")
        {
            strCompareOperator = ddlCompareOperator.SelectedValue;
        }


        ddlCompareOperator_SelectedIndexChanged(null, null);

        if (ddlYAxis.SelectedValue=="")
        {
            _strTextSearch = "";
            //_strNumericSearch = "";
            return;
        }

        int iC = int.Parse(ddlYAxis.SelectedValue);

        if (iC < 0)
        {
            //_strNumericSearch = "";
            _strTextSearch = "";
            iC= int.Parse( iC.ToString().Replace("-",""));

            DataTable dtSearchGroupClumn = Common.DataTableFromText(" SELECT * FROM SearchGroupColumn WHERE SearchGroupID=" + iC.ToString());

            //int i = 0;
            foreach (DataRow dr in dtSearchGroupClumn.Rows)
            {

                //Column theColumnSG = RecordManager.ets_Column_Details(int.Parse(dr["ColumnID"].ToString()));
                PopulateSearchGroup(int.Parse(dr["ColumnID"].ToString()));
                //if (i == 0)
                //{
                //    _strTextSearch = " Record." + theColumnSG.SystemName + " like'%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
                //}
                //else
                //{
                //    _strTextSearch = _strTextSearch + " OR Record." + theColumnSG.SystemName + " like'%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
                //}

             

                //i = i + 1;
            }

            if (_strTextSearch != "")
            {
                _strTextSearch = "(" + _strTextSearch + ")";
            }

            return;
        }

        Column theColumn = RecordManager.ets_Column_Details(iC);

        //bool bSearchAllifToIsNull = Common.SO_SearchAllifToIsNull( int.Parse(Session["AccountID"].ToString()), theColumn.TableID);


            if (ddlCompareOperator.SelectedValue == "empty")
            {
               
                //_strNumericSearch = "";
                _strTextSearch = " (Record." + theColumn.SystemName + " IS NULL OR LEN(Record." + theColumn.SystemName + ")=0)";
            }
            else if (ddlCompareOperator.SelectedValue == "notempty")
            {

                //_strNumericSearch = "";
                _strTextSearch = " (Record." + theColumn.SystemName + " IS NOT NULL AND LEN(Record." + theColumn.SystemName + ")>0)";
            }
            else if ((theColumn.ColumnType == "number"  && theColumn.IgnoreSymbols == false) || theColumn.ColumnType=="calculation")
            {
                _strTextSearch = "";
                //_strNumericSearch = "";
                if (txtLowerLimit.Text != "")
                {
                    _strTextSearch = " dbo.RemoveNonNumericChar(Record." + theColumn.SystemName + ") " + strCompareOperator + " CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
                }

                if (txtLowerLimit.Text != "" && txtUpperLimit.Text != "")
                {

                    _strTextSearch = " dbo.RemoveNonNumericChar(Record." + theColumn.SystemName + ") >= CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
                    _strTextSearch = _strTextSearch + " AND dbo.RemoveNonNumericChar(Record." + theColumn.SystemName + ") <= CONVERT(decimal(20,10)," + txtUpperLimit.Text.Trim() + ")";
                }
                else
                {                  

                    if ( txtUpperLimit.Text != "")
                    {

                        _strTextSearch = "  dbo.RemoveNonNumericChar(Record." + theColumn.SystemName + ") <= CONVERT(decimal(20,10)," + txtUpperLimit.Text.Trim() + ")";
                    }
                }


                if (_strTextSearch != "")
                {
                    _strTextSearch = " dbo.RemoveNonNumericChar(Record." + theColumn.SystemName + ")<>'' AND " + _strTextSearch;
                    _strTextSearch = "(" + _strTextSearch + ")";
                }

            }
            else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
            {
                _strTextSearch = "";
                //_strNumericSearch = "";
                if (ddlDropdownColumnSearch.SelectedItem != null && ddlDropdownColumnSearch.SelectedValue != "")
                    _strTextSearch = " Record." + theColumn.SystemName + " " + strCompareOperator + "'" + ddlDropdownColumnSearch.SelectedValue.Trim().Replace("'", "''") + "'";
            }
            //else if (theColumn.ColumnType == "calculation")
            //{
            //    //do nothing
            //}
            else if (theColumn.ColumnType == "date" )
            {
                _strTextSearch = "";
                //_strNumericSearch = "";

                txtLowerDate.Text = txtLowerDate.Text.Replace(" ", "");
                txtUpperDate.Text = txtUpperDate.Text.Replace(" ", "");

                DateTime dateValue;

                string strLowerDate = Common.ReturnDateStringFromToken(txtLowerDate.Text.Trim());
                string strUpperDate = Common.ReturnDateStringFromToken(txtUpperDate.Text.Trim());
                string strLowerPart = "";

                if (txtLowerDate.Text != "")
                {
                    if (DateTime.TryParseExact(strLowerDate, Common.Dateformats,
                                 new CultureInfo("en-GB"),
                                 DateTimeStyles.None,
                                 out dateValue))
                    {
                        //X
                        _strTextSearch = " CONVERT(Datetime,Record." + theColumn.SystemName + ",103) " + strCompareOperator + " CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";

                        strLowerPart = " CONVERT(Datetime,Record." + theColumn.SystemName + ",103) >= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
                    }
                }

                if (txtUpperDate.Text != "" && txtLowerDate.Text != "")
                {
                    if (DateTime.TryParseExact(strUpperDate, Common.Dateformats,
                                 new CultureInfo("en-GB"),
                                 DateTimeStyles.None,
                                 out dateValue))
                    {
                        _strTextSearch = strLowerPart + " AND CONVERT(Datetime,Record." + theColumn.SystemName + ",103) <= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
                    }
                }
                else
                {
                    if (txtUpperDate.Text != "" )
                    {
                        if (DateTime.TryParseExact(strUpperDate, Common.Dateformats,
                                     new CultureInfo("en-GB"),
                                     DateTimeStyles.None,
                                     out dateValue))
                        {
                            _strTextSearch = " CONVERT(Datetime,Record." + theColumn.SystemName + ",103) <= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
                        }
                    }
                }

                string strTimeGarbage = " ISDATE(Record." + theColumn.SystemName + ")=1 AND ";
                if (_strTextSearch != "")
                    _strTextSearch = "(" + strTimeGarbage + _strTextSearch + ")";

                //if (txtLowerDate.Text != "" && txtUpperDate.Text != "")
                //{
                //    _strNumericSearch = "(" + _strNumericSearch + ")";
                //}

            }
            else if ( theColumn.ColumnType == "datetime")
            {
                _strTextSearch = "";
                //_strNumericSearch = "";

                txtLowerDate.Text = txtLowerDate.Text.Replace(" ", "");
                txtUpperDate.Text = txtUpperDate.Text.Replace(" ", "");

                string strLowerDate = Common.ReturnDateStringFromToken(txtLowerDate.Text.Trim());
                string strUpperDate = Common.ReturnDateStringFromToken(txtUpperDate.Text.Trim());

                string strLowerTime = txtLowerTime.Text.Trim() == "" ? " 00:00" : " " + txtLowerTime.Text.Trim();
                string strUpperTime = txtUpperTime.Text.Trim() == "" ? " 23:59" : " " + txtUpperTime.Text.Trim();

                DateTime dateValue;

                string strLowerPart = "";

                if (txtLowerDate.Text != "")
                {
                    if (DateTime.TryParseExact(strLowerDate.Trim() + strLowerTime, Common.DateTimeformats,
                                 new CultureInfo("en-GB"),
                                 DateTimeStyles.None,
                                 out dateValue))
                    {

                        _strTextSearch = " CONVERT(Datetime,Record." + theColumn.SystemName + ",103) " + strCompareOperator + " CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                        strLowerPart = " CONVERT(Datetime,Record." + theColumn.SystemName + ",103) >= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
                    }
                }

                if (txtUpperDate.Text != "" && txtLowerDate.Text != "")
                {
                    if (DateTime.TryParseExact(strUpperDate.Trim() + strUpperTime, Common.DateTimeformats,
                                 new CultureInfo("en-GB"),
                                 DateTimeStyles.None,
                                 out dateValue))
                    {
                        _strTextSearch = strLowerPart + " AND CONVERT(Datetime,Record." + theColumn.SystemName + ",103) <= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                    }
                }
                else
                {
                    if (txtUpperDate.Text != "")
                    {
                        if (DateTime.TryParseExact(strUpperDate.Trim() + strUpperTime, Common.DateTimeformats,
                                     new CultureInfo("en-GB"),
                                     DateTimeStyles.None,
                                     out dateValue))
                        {
                            _strTextSearch = " CONVERT(Datetime,Record." + theColumn.SystemName + ",103) <= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                        }
                    }
                }

                string strTimeGarbage = " ISDATE(Record." + theColumn.SystemName + ")=1 AND ";
                if (_strTextSearch != "")
                    _strTextSearch = "(" + strTimeGarbage + _strTextSearch + ")";

                //if (txtLowerDate.Text != "" && txtUpperDate.Text != "")
                //{
                //    _strNumericSearch = "(" + _strNumericSearch + ")";
                //}

            }
            else if (theColumn.ColumnType == "time")
            {
                _strTextSearch = "";
                //_strNumericSearch = "";

                //DateTime dtToday = DateTime.Today.ToShortDateString();
                DateTime dateValue;
                string strLowerPart = "";
                if (txtLowerTime.Text != "")
                {
                    if (DateTime.TryParseExact(_strTodayShortDate + " " + txtLowerTime.Text.Trim(), Common.DateTimeformats,
                                 new CultureInfo("en-GB"),
                                 DateTimeStyles.None,
                                 out dateValue))
                    {

                        _strTextSearch = " CONVERT(Datetime,CONVERT(varchar(11),getdate(),103) + ' ' + Record." + theColumn.SystemName + ",103) " + strCompareOperator + " CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                        strLowerPart = " CONVERT(Datetime,CONVERT(varchar(11),getdate(),103) + ' ' + Record." + theColumn.SystemName + ",103) >= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";

                    }
                }

                if (txtUpperTime.Text != "" && txtLowerTime.Text != "")
                {
                    if (DateTime.TryParseExact(_strTodayShortDate + " " + txtUpperTime.Text.Trim(), Common.DateTimeformats,
                                 new CultureInfo("en-GB"),
                                 DateTimeStyles.None,
                                 out dateValue))
                    {
                        _strTextSearch = strLowerPart + " AND CONVERT(Datetime,CONVERT(varchar(11),getdate(),103) + ' ' + Record." + theColumn.SystemName + ",103) <= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                    }
                }
                else
                {
                    if (txtUpperTime.Text != "")
                    {
                        if (DateTime.TryParseExact(_strTodayShortDate + " " + txtUpperTime.Text.Trim(), Common.DateTimeformats,
                                     new CultureInfo("en-GB"),
                                     DateTimeStyles.None,
                                     out dateValue))
                        {
                            _strTextSearch = " CONVERT(Datetime,CONVERT(varchar(11),getdate(),103) + ' ' + Record." + theColumn.SystemName + ",103) <= CONVERT(Datetime,'" + dateValue.ToString() + "',103)";
                        }
                    }
                }


//                string strTimeGarbage = @" CHARINDEX(':',Record." + theColumn.SystemName + @")>0 AND ISNUMERIC( SUBSTRING(Record." + theColumn.SystemName + @",0,PATINDEX('%:%',Record." + theColumn.SystemName + @")))=1 AND
//                                            ISNUMERIC(SUBSTRING(Record." + theColumn.SystemName + @",PATINDEX('%:%',Record." + theColumn.SystemName + @")+1,2))=1 AND ";


                string strTimeGarbage = " ISDATE(Record." + theColumn.SystemName + ")=1 AND ";
                if (_strTextSearch != "")
                    _strTextSearch = "(" + strTimeGarbage + _strTextSearch + ")";

                //if (txtUpperTime.Text != "" && txtLowerTime.Text != "")
                //{
                //    _strNumericSearch = "(" + _strNumericSearch + ")";
                //}

            }
            else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
                theColumn.TableTableID != null && theColumn.DisplayColumn != "")
            {
                _strTextSearch = "";
                //_strNumericSearch = "";

                if (ddlDropdownColumnSearch.SelectedItem != null && ddlDropdownColumnSearch.SelectedValue != "")
                    _strTextSearch = " Record." + theColumn.SystemName + " " + strCompareOperator + "'" + ddlDropdownColumnSearch.SelectedValue.Trim().Replace("'", "''") + "'";

            }
            else if (theColumn.ColumnType == "radiobutton" || theColumn.ColumnType == "checkbox"
                  || theColumn.ColumnType == "listbox")
            {
                _strTextSearch = "";
                //_strNumericSearch = "";
                if (ddlDropdownColumnSearch.SelectedItem != null && ddlDropdownColumnSearch.SelectedValue != "")
                {
                    if (theColumn.ColumnType == "listbox")
                    {
                        if (strCompareOperator=="=")
                        {
                            _strTextSearch = " CHARINDEX('," + ddlDropdownColumnSearch.SelectedValue.Trim().Replace("'", "''") + ",' ,',' + Record." + theColumn.SystemName + " + ',')>0";
                        }
                        else if (strCompareOperator == "<>")
                        {
                            _strTextSearch = " CHARINDEX('," + ddlDropdownColumnSearch.SelectedValue.Trim().Replace("'", "''") + ",' ,',' + Record." + theColumn.SystemName + " + ',')=0";
                        }
                        else
                        {
                            _strTextSearch = " Record." + theColumn.SystemName + " " + strCompareOperator + "'" + ddlDropdownColumnSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                        }
                    }
                    else
                    {
                        _strTextSearch = " Record." + theColumn.SystemName + " " + strCompareOperator + "'" + ddlDropdownColumnSearch.SelectedValue.Trim().Replace("'", "''") + "'";
                    }
                }
                    
            }
            else
            {
                //_strNumericSearch = "";
                _strTextSearch = "";

                if (txtSearchText.Text != "")
                {
                    //if (theColumn.ColumnType == "radiobutton")
                    //{
                    //    _strTextSearch = " Record." + theColumn.SystemName + " " + strCompareOperator + "'" + txtSearchText.Text.Trim().Replace("'", "''") + "'";
                    //}
                    //else
                    //{
                        if (strCompareOperator == "=")
                        {
                            _strTextSearch = " Record." + theColumn.SystemName + " LIKE'%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
                        }
                        else if (strCompareOperator == "<>")
                        {
                            _strTextSearch = " CHARINDEX('" + txtSearchText.Text.Trim().Replace("'", "''") + "',Record." + theColumn.SystemName + ")=0 ";
                        }
                        else
                        {
                            _strTextSearch = " Record." + theColumn.SystemName + " " + strCompareOperator + "'" + txtSearchText.Text.Trim().Replace("'", "''") + "'";
                        }
                    //}

                    
                                        
                }

            }     

       

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //if (TableID == null)
        //{
        //    if (Request.QueryString["TableID"] != null)
        //    {
        //        TableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
        //    }
        //}

        txtLowerDate.ToolTip = Common.ToolTip_Today;
        txtUpperDate.ToolTip = txtLowerDate.ToolTip;

        txtLowerDate.Attributes.Add("onblur", "this.value=this.value.trim()");
        txtUpperDate.Attributes.Add("onblur", "this.value=this.value.trim()");
        txtLowerLimit.Attributes.Add("onblur", "this.value=this.value.trim()");
        txtUpperLimit.Attributes.Add("onblur", "this.value=this.value.trim()");
        txtSearchText.Attributes.Add("onblur", "this.value=this.value.trim()");

        txtLowerDate.Attributes.Add("placeholder", "dd/mm/yyyy");
        txtLowerTime.Attributes.Add("placeholder", "hh:mm");
        txtUpperDate.Attributes.Add("placeholder", "Upper");
        txtUpperTime.Attributes.Add("placeholder", "hh:mm");
        txtUpperLimit.Attributes.Add("placeholder", "Upper");

        if (this.Page.MasterPageFile!=null && this.Page.MasterPageFile.ToLower().IndexOf("rrp") > -1)
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
            if (ddlCompareOperator.Items.Count==0)
                 PopulateComparators();
            
            if(TableID!=null)
                PopulateYAxis();
        }
    }


    protected void PopulateSearchGroup(int iC)
    {

        if (txtSearchText.Text == "")
            return;

        Column theColumn = RecordManager.ets_Column_Details(iC);

        if (_strTextSearch != "")
            _strTextSearch = _strTextSearch + " OR ";

        //if (_strNumericSearch != "")
        //    _strNumericSearch = _strNumericSearch + " OR ";

        if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
        {
            if (theColumn.DropDownType == "values")
            {
                if (txtSearchText.Text != "")
                    _strTextSearch = _strTextSearch + " Record." + theColumn.SystemName + " like '%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
            }
            if (theColumn.DropDownType == "value_text")
            {
                string strSearchValue = Common.GetDDLValueFromText(theColumn.DropdownValues, txtSearchText.Text);
                if (strSearchValue != "")
                {
                    _strTextSearch = _strTextSearch + " Record." + theColumn.SystemName + " like '%" + strSearchValue.Trim().Replace("'", "''") + "%'";
                }
            }
        }        
        else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
            theColumn.TableTableID != null && theColumn.DisplayColumn != "")
        {


            if (txtSearchText.Text != "")
            {



                //string search = txtSearchText.Text.Replace("'", "''");
                string search = txtSearchText.Text;

                if (search.Trim() == "")
                {
                    return;
                }

                string regex = @"\[(.*?)\]";
                string strDisplayColumn = theColumn.DisplayColumn;
                string text = theColumn.DisplayColumn;

                string strDCForSQL = strDisplayColumn.Replace("'", "''");
                int i = 1;
                string strFirstSystemName = "";
                string strFirstDisplayName = "";
                string strSecondSystemName = "";
                string strSecondDisplayName = "";
                bool bHaveSecond = false;
                //List<string> lstDisplayName = new List<string>();
                //List<string> lstSystemName = new List<string>();
                foreach (Match match in Regex.Matches(text, regex))
                {
                    string strEachDisplayName = match.Groups[1].Value;

                    //lstDisplayName.Add(strEachDisplayName);

                    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName FROM [Column] WHERE   TableID ="
                        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

                    string strEachSystemName = "";
                    if (dtTableTableSC.Rows.Count > 0)
                    {
                        strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
                        //lstSystemName.Add(strEachSystemName);
                    }


                    if (i == 1)
                    {

                        strFirstDisplayName = strEachDisplayName;
                        strFirstSystemName = strEachSystemName;
                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
                    }
                    if (i == 2)
                    {
                        bHaveSecond = true;
                        strSecondDisplayName = strEachDisplayName;
                        strSecondSystemName = strEachSystemName;
                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
                    }
                    else
                    {
                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
                    }
                    i = i + 1;
                }
                strDCForSQL = strDCForSQL.Trim() + "'";

                //strDCForSQL = strDCForSQL.Substring(0, strDCForSQL.Length - 2);
                string strSecondSQL = "";
                if (bHaveSecond)
                {
                    strSecondSQL = " OR CAST(" + strSecondSystemName + " AS VARCHAR(MAX)) like '%" + search.Replace("'", "''") + "%'";
                }

                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

                DataTable dtData = Common.DataTableFromText(@"SELECT TOP 1000 " + theLinkedColumn.SystemName + "," + strDCForSQL + @"
                    FROM Record WHERE  IsActive= 1 AND 
                    TableID=" + theColumn.TableTableID.ToString() + " and (CAST(" + strFirstSystemName + @" AS VARCHAR(MAX)) like '%" + search.Replace("'", "''") + @"%'" + strSecondSQL + ")");

                string strRecordIDs = "";
                foreach (DataRow dr in dtData.Rows)
                {

                    strRecordIDs = strRecordIDs + "'" + dr[0].ToString() + "'" + ",";
                }

                strRecordIDs = strRecordIDs + "'---1---'";


                _strTextSearch = _strTextSearch + " Record." + theColumn.SystemName + " IN (" + strRecordIDs + ")";

            }

        }
        else
        {

            if (txtSearchText.Text != "")
            {
                _strTextSearch = _strTextSearch + " Record." + theColumn.SystemName + " like'%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
            }

        }
    }



    protected void PopulateYAxis()
    {

        //DataTable dtSCs = RecordManager.ets_Table_Columns_All((int)TableID,null,null);
        //ddlYAxis.Items.Clear(); 
        //AND  ColumnID NOT IN (SELECT ColumnID FROM SearchGroupColumn ) //SummarySearch=1 AND

        string strColumnTypeNotIN = "";

        if (hfColumnTypeOut.Value!="")
        {
            strColumnTypeNotIN = " AND ColumnType NOT IN (" + hfColumnTypeOut.Value + ") ";
        }

        if (ddlYAxis.Items.Count > 0)
            return;

        DataTable dtSearchGroup = Common.DataTableFromText(" SELECT SearchGroupID,GroupName FROM SearchGroup WHERE TableID=" + TableID.ToString() + "  ORDER BY DisplayOrder");

        foreach (DataRow dr in dtSearchGroup.Rows)
        {
            System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(
                      dr["GroupName"].ToString() + "*",
                      "-" + dr["SearchGroupID"].ToString());

            ddlYAxis.Items.Insert(ddlYAxis.Items.Count, aItem);
        }


        DataTable dtSCs;


        if (ViewID == null)
        {
            dtSCs = Common.DataTableFromText("SELECT * FROM [Column] WHERE ColumnType<>'staticcontent' " + strColumnTypeNotIN + " AND TableID=" + TableID.ToString()
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
            //dtSCs = Common.DataTableFromText("  SELECT * FROM ViewItem WHERE ViewID=" + ViewID.ToString() + @"  ORDER BY Heading");//AND FilterField=1

//            dtSCs = Common.DataTableFromText(@"SELECT VT.* FROM ViewItem VT INNER JOIN [Column] C 
//            ON VT.ColumnID=C.ColumnID WHERE ViewID=" + ViewID.ToString() + strColumnTypeNotIN + @" AND ColumnType NOT IN ('staticcontent')  ORDER BY Heading ");


            string strTableID=TableID==null?"-1":TableID.ToString();

            dtSCs = Common.DataTableFromText(@"SELECT VT.ColumnID,ISNULL(C.DisplayTextSummary,C.DisplayName) AS Heading FROM ViewItem VT INNER JOIN [Column] C 
                ON VT.ColumnID=C.ColumnID  WHERE ViewID=" + ViewID.ToString() + strColumnTypeNotIN + @" and ColumnType NOT IN ('staticcontent')
                UNION
                SELECT ColumnID, DisplayName as Heading FROM [Column] WHERE IsStandard=0 and 
                TableID=" + strTableID + strColumnTypeNotIN + @" AND ColumnType NOT IN ('staticcontent')  AND 
                ColumnID NOT IN (SELECT ColumnID FROM ViewItem WHERE ViewID="+ ViewID.ToString() + @")
                ORDER BY Heading");

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



//    protected void PopulateSearchGroup(int iC)
//    {

//        Column theColumn = RecordManager.ets_Column_Details(iC);

//        if (_strTextSearch != "")
//            _strTextSearch = _strTextSearch + " OR ";

//        if (_strNumericSearch != "")
//            _strNumericSearch = _strNumericSearch + " OR ";

//        if (theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false)
//        {

//            if (txtLowerLimit.Text != "")
//            {
//                _strNumericSearch = _strNumericSearch + " Record." + theColumn.SystemName + " >= CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
//            }

//            if (txtLowerLimit.Text != "" && txtUpperLimit.Text != "")
//            {

//                _strNumericSearch = _strNumericSearch + " Record." + theColumn.SystemName + " >= CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
//                _strNumericSearch = _strNumericSearch + " AND Record." + theColumn.SystemName + " <= CONVERT(decimal(20,10)," + txtUpperLimit.Text.Trim() + ")";
//            }
//            else
//            {
//                if (txtLowerLimit.Text != "")
//                {
//                    _strNumericSearch = _strNumericSearch + " Record." + theColumn.SystemName + " >= CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
//                }

//                if (txtUpperLimit.Text != "")
//                {

//                    _strNumericSearch = _strNumericSearch + "  Record." + theColumn.SystemName + " <= CONVERT(decimal(20,10)," + txtUpperLimit.Text.Trim() + ")";
//                }
//            }


//            if (_strNumericSearch != "")
//            {
//                _strNumericSearch = "(" + _strNumericSearch + ")";
//            }

//        }
//        else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
//        {

//            if (ddlDropdownColumnSearch.SelectedItem != null && ddlDropdownColumnSearch.SelectedValue != "")
//                _strTextSearch = _strTextSearch + " Record." + theColumn.SystemName + " ='" + ddlDropdownColumnSearch.SelectedValue.Trim().Replace("'", "''") + "'";
//        }
//        else if (theColumn.ColumnType == "calculation")
//        {
//            //do nothing
//        }
//        else if (theColumn.ColumnType == "date" || theColumn.ColumnType == "datetime")
//        {

//            DateTime dateValue;

//            if (txtLowerDate.Text != "")
//            {
//                if (DateTime.TryParseExact(txtLowerDate.Text.Trim(), Common.Dateformats,
//                             new CultureInfo("en-GB"),
//                             DateTimeStyles.None,
//                             out dateValue))
//                {

//                    _strNumericSearch = _strNumericSearch + " CONVERT(Datetime,Record." + theColumn.SystemName + ",103) >= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
//                }
//            }

//            if (txtUpperDate.Text != "")
//            {
//                if (DateTime.TryParseExact(txtUpperDate.Text.Trim(), Common.Dateformats,
//                             new CultureInfo("en-GB"),
//                             DateTimeStyles.None,
//                             out dateValue))
//                {
//                    _strNumericSearch = _strNumericSearch + " AND CONVERT(Datetime,Record." + theColumn.SystemName + ",103) <= CONVERT(Datetime,'" + dateValue.ToShortDateString() + "',103)";
//                }
//            }


//            if (txtLowerDate.Text != "" && txtUpperDate.Text != "")
//            {
//                _strNumericSearch = "(" + _strNumericSearch + ")";
//            }

//        }
//        else if (theColumn.ColumnType == "dropdown" && (theColumn.DropDownType == "table" || theColumn.DropDownType == "tabledd") &&
//            theColumn.TableTableID != null && theColumn.DisplayColumn != "")
//        {


//            if (ddlDropdownColumnSearch.SelectedItem != null)
//            {



//                //string search = txtSearchText.Text.Replace("'", "''");
//                string search = ddlDropdownColumnSearch.SelectedItem.Text;

//                if (search.Trim() == "")
//                {
//                    return;
//                }

//                string regex = @"\[(.*?)\]";
//                string strDisplayColumn = theColumn.DisplayColumn;
//                string text = theColumn.DisplayColumn;

//                string strDCForSQL = strDisplayColumn.Replace("'", "''");
//                int i = 1;
//                string strFirstSystemName = "";
//                string strFirstDisplayName = "";
//                string strSecondSystemName = "";
//                string strSecondDisplayName = "";
//                bool bHaveSecond = false;
//                //List<string> lstDisplayName = new List<string>();
//                //List<string> lstSystemName = new List<string>();
//                foreach (Match match in Regex.Matches(text, regex))
//                {
//                    string strEachDisplayName = match.Groups[1].Value;

//                    //lstDisplayName.Add(strEachDisplayName);

//                    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName FROM [Column] WHERE   TableID ="
//                        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

//                    string strEachSystemName = "";
//                    if (dtTableTableSC.Rows.Count > 0)
//                    {
//                        strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
//                        //lstSystemName.Add(strEachSystemName);
//                    }


//                    if (i == 1)
//                    {

//                        strFirstDisplayName = strEachDisplayName;
//                        strFirstSystemName = strEachSystemName;
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
//                    }
//                    if (i == 2)
//                    {
//                        bHaveSecond = true;
//                        strSecondDisplayName = strEachDisplayName;
//                        strSecondSystemName = strEachSystemName;
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
//                    }
//                    else
//                    {
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(MAX)),'') +'");
//                    }
//                    i = i + 1;
//                }
//                strDCForSQL = strDCForSQL.Trim() + "'";

//                //strDCForSQL = strDCForSQL.Substring(0, strDCForSQL.Length - 2);
//                string strSecondSQL = "";
//                if (bHaveSecond)
//                {
//                    strSecondSQL = " OR CAST(" + strSecondSystemName + " AS VARCHAR(MAX)) like '%" + search.Replace("'", "''") + "%'";
//                }

//                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

//                DataTable dtData = Common.DataTableFromText(@"SELECT TOP 1000 " + theLinkedColumn.SystemName + "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " and (CAST(" + strFirstSystemName + @" AS VARCHAR(MAX)) like '%" + search.Replace("'", "''") + @"%'" + strSecondSQL + ")");

//                string strRecordIDs = "";
//                foreach (DataRow dr in dtData.Rows)
//                {

//                    strRecordIDs = strRecordIDs + "'" + dr[0].ToString() + "'" + ",";
//                }

//                strRecordIDs = strRecordIDs + "'---1---'";


//                _strTextSearch = _strTextSearch + " Record." + theColumn.SystemName + " IN (" + strRecordIDs + ")";

//            }

//        }
//        else
//        {

//            if (txtSearchText.Text != "")
//            {
//                _strTextSearch = _strTextSearch + " Record." + theColumn.SystemName + " like'%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
//            }

//        }
//    }


    //protected string GetDDLValueFromText(string strDropdownValues, string strSearchText)
    //{

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
    //                if (strText.ToLower() == strSearchText.ToLower())
    //                {
    //                    return strValue;
    //                }
    //            }
    //        }
    //    }

    //    return "";

    //}

    protected void HideResetAllControls()
    {
        //tdCompareOperator.Visible = false;
        txtLowerTime.Visible = false;
        txtLowerLimit.Visible = false;       
        txtSearchText.Visible = false;
        ddlDropdownColumnSearch.Visible = false;
        txtLowerDate.Visible = false;       
        ibLowerDate.Visible = false;    

        txtLowerTime.Text = "";      

        txtLowerLimit.Text = "";     
        txtSearchText.Text = "";
        txtLowerDate.Text = "";      
        
                
        ddlDropdownColumnSearch.ClearSelection();

        HideUpperControls();

       
    }
    //protected void ResetCompareOperator()
    //{
    //    //tdCompareOperator.Visible = false;
    //    ddlCompareOperator.ClearSelection();
    //    //ddlCompareOperator.Width = 50;
    //}

    protected void HideUpperControls()
    {
        lblTo.Visible = false;   

         
        txtUpperLimit.Visible = false;
        txtUpperDate.Visible = false;
        ibUpperDate.Visible = false;  
        txtUpperTime.Visible = false;              
            
        
        txtUpperLimit.Text = "";             
        txtUpperDate.Text = "";
        txtUpperTime.Text = "";
       
    }
    protected void ddlCompareOperator_SelectedIndexChanged(object sender, EventArgs e)
    {
       

        if (ddlYAxis.SelectedValue != "" && int.Parse(ddlYAxis.SelectedValue)>0)
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlYAxis.SelectedValue));
            if (ddlCompareOperator.SelectedValue == "between")
            {
                //ddlCompareOperator.Width = 90;
                lblTo.Visible = true;
                if (theColumn.ColumnType == "number" || theColumn.ColumnType == "calculation")
                {
                    txtLowerLimit.Visible = true;      
                    txtUpperLimit.Visible = true;
                }
                else if (theColumn.ColumnType == "date")
                {
                    txtLowerDate.Visible = true;
                    ibLowerDate.Visible = true;    
                    txtUpperDate.Visible = true;
                    ibUpperDate.Visible = true;
                }
                else if (theColumn.ColumnType == "datetime")
                {
                    txtLowerDate.Visible = true;
                    ibLowerDate.Visible = true;
                    txtLowerTime.Visible = true;
                    txtUpperDate.Visible = true;
                    ibUpperDate.Visible = true;
                    txtUpperTime.Visible = true; 
                }
                else if (theColumn.ColumnType == "time")
                {
                    txtLowerTime.Visible = true;
                    txtUpperTime.Visible = true;
                }
            }
            else if (ddlCompareOperator.SelectedValue == "empty" || ddlCompareOperator.SelectedValue == "notempty")
            {
                HideResetAllControls();
                //tdCompareOperator.Visible = true;
                //ddlCompareOperator.Width = 90;
            }
            else
            {
                //ddlCompareOperator.Width = 50;

                if (theColumn.ColumnType == "number" || theColumn.ColumnType=="calculation")
                {
                    txtLowerLimit.Visible = true;
                   
                }
                else if (theColumn.ColumnType == "date")
                {
                    txtLowerDate.Visible = true;
                    ibLowerDate.Visible = true;
                   
                }
                else if (theColumn.ColumnType == "datetime")
                {
                    txtLowerDate.Visible = true;
                    ibLowerDate.Visible = true;
                    txtLowerTime.Visible = true;
                  
                }
                else if (theColumn.ColumnType == "time")
                {
                    txtLowerTime.Visible = true;
                  
                }
                else if (theColumn.ColumnType == "dropdown" || theColumn.ColumnType == "radiobutton" || theColumn.ColumnType == "checkbox"
                  || theColumn.ColumnType == "listbox")
                {
                    ddlDropdownColumnSearch.Visible = true;
                }
                else
                {
                    txtSearchText.Visible = true;
                }

                HideUpperControls();
            }
        }
        else
        {
            //tdCompareOperator.Visible = false;
            HideUpperControls();
        }


        if (ddlCompareOperator_Changed != null && sender!=null)
            ddlCompareOperator_Changed(this, EventArgs.Empty);

    }
    protected void ddlYAxis_SelectedIndexChanged(object sender, EventArgs e)
    {
        //do the show hide

        if (ddlYAxis_Changed != null)
            ddlYAxis_Changed(this, EventArgs.Empty);

        HideResetAllControls();
        //ResetCompareOperator();

        PopulateComparators();
        ddlCompareOperator.ClearSelection();
        if (ddlYAxis.SelectedValue == "")
        {
               //do nothing
            tdCompareOperator.Visible = false;
        }
        else
        {
            tdCompareOperator.Visible = true;
            int iC = int.Parse(ddlYAxis.SelectedValue);

            if (iC > 0)
            {
                // do nothing here
            }
            else
            {
                //iC = int.Parse(iC.ToString().Replace("-", ""));
                //DataTable dtSearchGroupClumn = Common.DataTableFromText(" SELECT * FROM SearchGroupColumn WHERE SearchGroupID=" + iC.ToString());


                //foreach (DataRow dr in dtSearchGroupClumn.Rows)
                //{
                //    Column theColumnSG = RecordManager.ets_Column_Details(int.Parse(dr["ColumnID"].ToString()));
                //    iC =(int) theColumnSG.ColumnID;
                //    break;
                //}
            }

            if (iC > 0)
            {
                Column theColumn = RecordManager.ets_Column_Details(iC);

                if (theColumn.ColumnType == "number" || theColumn.ColumnType=="calculation")
                {
                    //tdCompareOperator.Visible = true;
                    txtLowerLimit.Visible = true;
                    //lblTo.Visible = true;  
                    //txtUpperLimit.Visible = true;
                                     
                }
                else if (theColumn.ColumnType == "dropdown" && theColumn.DropdownValues != "" &&
                    (theColumn.DropDownType == "values" || theColumn.DropDownType == "value_text"))
                {
                   
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
                   
                    ddlDropdownColumnSearch.Visible = true;
                    ddlDropdownColumnSearch.Items.Clear();
                    RecordManager.PopulateTableDropDown((int)theColumn.ColumnID, ref ddlDropdownColumnSearch);

                    //if(Request.RawUrl.IndexOf("ViewEditPage.aspx")>-1)
                    //{
                        string strColumnID = Common.GetValueFromSQL(@"SELECT TOP 1 ColumnID FROM [Column] WHERE TableID=" + theColumn.TableTableID.ToString() + @" AND 
                                ColumnType='dropdown' AND DisplayColumn IS NOT NULL
                                            AND TableTableID=-1");

                        if (strColumnID != "")
                        {
                            ListItem li2 = new ListItem("--Logged In User--", "-user-");
                            if (ddlDropdownColumnSearch.Items.Count > 0)
                                ddlDropdownColumnSearch.Items.Insert(1, li2);
                        }
                    //}
                    


                }
                else if (theColumn.ColumnType == "listbox")
                {
                    ddlDropdownColumnSearch.Visible = true;
                    ddlDropdownColumnSearch.Items.Clear();
                     if (theColumn.DropDownType == "values")
                     {
                         TheDatabase.PutDDLValues(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
                     }
                     else if (theColumn.DropDownType == "value_text")
                     {
                         TheDatabase.PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
                     }
                     else
                     {
                          if (theColumn.DropDownType == "table" && theColumn.TableTableID != null
                              && theColumn.DisplayColumn != "" && theColumn.LinkedParentColumnID!=null)
                          {
                              RecordManager.PopulateTableDropDown((int)theColumn.ColumnID, ref ddlDropdownColumnSearch);
                          }
                     }


                }
                else if (theColumn.ColumnType == "radiobutton")
                {
                    ddlDropdownColumnSearch.Visible = true;
                    ddlDropdownColumnSearch.Items.Clear();
                    if (theColumn.DropDownType == "values")
                    {
                        TheDatabase.PutDDLValues(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
                    }
                    else if (theColumn.DropDownType == "value_text")
                    {
                        TheDatabase.PutDDLValue_Text(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
                    }
                    else
                    {
                        Common.PutRadioImageInto_DDL(theColumn.DropdownValues, ref ddlDropdownColumnSearch);
                    }
                }
                else if (theColumn.ColumnType == "checkbox")
                {
                    ddlDropdownColumnSearch.Visible = true;
                    ddlDropdownColumnSearch.Items.Clear();
                    TheDatabase.PutCheckboxIntoDDL(theColumn.DropdownValues, ref ddlDropdownColumnSearch);

                }
                else if (theColumn.ColumnType == "date")
                {                

                    txtLowerDate.Visible = true;
                    ibLowerDate.Visible = true;                   

                }
                else if (theColumn.ColumnType == "datetime")
                {
                    //tdCompareOperator.Visible = true;
                    txtLowerDate.Visible = true;
                    ibLowerDate.Visible = true;
                    txtLowerTime.Visible = true;                     

                }
                else if (theColumn.ColumnType == "time")
                {                   
                    txtLowerTime.Visible = true;                 
                }
                else
                {
                    txtSearchText.Visible = true;
                }
               
            }
            else
            {              
                txtSearchText.Visible = true;             

            }
        }

    }


}