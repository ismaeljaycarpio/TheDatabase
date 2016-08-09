using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;
using System.Data;
using System.Globalization;
using System.Xml;
using System.IO;
public partial class Pages_DocGen_EachCalendar : System.Web.UI.Page
{
    int _DocumentSectionID = 0;
    DataTable _dtMonthData;
    string _strTableID = "";

    protected void lnkPre_OnClick(object sender, EventArgs e)
    {
        if (ViewState["MonthDate"] != null && ViewState["ViewType"].ToString() == "m")
        {
            ViewState["MonthDate"] = ((DateTime)ViewState["MonthDate"]).AddMonths(-1);          
        }
        else
        {

            ViewState["MonthDate"] = ((DateTime)ViewState["MonthDate"]).AddDays(-7);
            SetMonday();
        }


        cldDate.TodaysDate = (DateTime)ViewState["MonthDate"];
        PopulateCalerdar();

    }
    protected void lnkNext_OnClick(object sender, EventArgs e)
    {
        if (ViewState["MonthDate"] != null && ViewState["ViewType"].ToString() == "m")
        {
            ViewState["MonthDate"] = ((DateTime)ViewState["MonthDate"]).AddMonths(1);

        }
        else
        {

            ViewState["MonthDate"] = ((DateTime)ViewState["MonthDate"]).AddDays(7);
            SetMonday();
        }

        cldDate.TodaysDate = (DateTime)ViewState["MonthDate"];
        PopulateCalerdar();

    }

    protected void lnkMonthWeekView_OnClick(object sender, EventArgs e)
    {
        if (ViewState["ViewType"].ToString() == "m")
        {
            ViewState["ViewType"] = "w";
            lnkMonthWeekView.Text = "Month View";
            SetMonday();

        }
        else
        {
            ViewState["ViewType"] = "m";
            lnkMonthWeekView.Text = "Week View";
        }

        PopulateCalerdar();
    }




    protected bool IsDataInFilterRange(string strColumnType, string strNumberType, string strRecordValue, string strFilterValue, string strCompareOperator)
    {
        

        try
        {
            if (strCompareOperator == "")
                strCompareOperator = "=";

            if (strCompareOperator == "empty")
            {
                if(strRecordValue=="")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (strCompareOperator == "notempty")
            {
                if (strRecordValue != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (strRecordValue == "")
            {
                return false;
            }

            strColumnType = strColumnType.ToLower();
            if (strColumnType == "number")
            {
                if (strFilterValue.IndexOf("____") > -1)
                {
                    int iRecordValue=int.Parse(strRecordValue);
                    string strLowerLimit = strFilterValue.Substring(0, strFilterValue.IndexOf("____"));
                    string strUpperLimit = strFilterValue.Substring(strFilterValue.IndexOf("____") + 4);
                    if (strLowerLimit != "" && strUpperLimit != "")
                    {
                       
                        if(iRecordValue>=int.Parse(strLowerLimit) && iRecordValue<=int.Parse(strUpperLimit))
                        {
                            return true;
                        }                       
                    }
                    else
                    {
                         if (strLowerLimit != "" )
                         {
                             if (strCompareOperator == "=")
                             {
                                 if (iRecordValue == int.Parse(strLowerLimit))
                                 {
                                     return true;
                                 }
                             }
                             if (strCompareOperator == ">")
                             {
                                 if (iRecordValue > int.Parse(strLowerLimit))
                                 {
                                     return true;
                                 }
                             }
                             if (strCompareOperator == "<")
                             {
                                 if (iRecordValue < int.Parse(strLowerLimit))
                                 {
                                     return true;
                                 }
                             }

                             if (strCompareOperator == "<>")
                             {
                                 if (iRecordValue != int.Parse(strLowerLimit))
                                 {
                                     return true;
                                 }
                             }
                         }
                         else if (strUpperLimit != "")
                         {
                             if (iRecordValue <= int.Parse(strUpperLimit))
                             {
                                 return true;
                             }                           
                         }

                    }

                }
            }
            else if (strColumnType == "datetime" || strColumnType == "date" || strColumnType == "time")
            {
                if (strFilterValue.IndexOf("____") > -1)
                {
                    DateTime dtRecordValue = DateTime.Parse(strRecordValue);
                    string strLowerLimit = strFilterValue.Substring(0, strFilterValue.IndexOf("____"));
                    string strUpperLimit = strFilterValue.Substring(strFilterValue.IndexOf("____") + 4);
                    strLowerLimit = Common.ReturnDateStringFromToken(strLowerLimit);
                    strUpperLimit = Common.ReturnDateStringFromToken(strUpperLimit);
                    if (strLowerLimit != "" && strUpperLimit != "")
                    {

                        if (dtRecordValue >= DateTime.Parse(strLowerLimit) && dtRecordValue <= DateTime.Parse(strUpperLimit))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (strLowerLimit != "")
                        {
                            if(strCompareOperator=="=")
                            {
                                if (dtRecordValue == DateTime.Parse(strLowerLimit))
                                {
                                    return true;
                                }
                            }
                            if (strCompareOperator == ">")
                            {
                                if (dtRecordValue > DateTime.Parse(strLowerLimit))
                                {
                                    return true;
                                }
                            }
                            if (strCompareOperator == "<")
                            {
                                if (dtRecordValue < DateTime.Parse(strLowerLimit))
                                {
                                    return true;
                                }
                            }

                            if (strCompareOperator == "<>")
                            {
                                if (dtRecordValue != DateTime.Parse(strLowerLimit))
                                {
                                    return true;
                                }
                            }

                        }
                        else if (strUpperLimit != "")
                        {
                            if (dtRecordValue <= DateTime.Parse(strUpperLimit))
                            {
                                return true;
                            }
                        }

                    }

                }
            }
            else
            {
                if (strCompareOperator=="<>")
                {
                    if (strRecordValue != strFilterValue)
                    {
                        return true;
                    }
                }
                else
                {
                    if (strRecordValue == strFilterValue)
                    {
                        return true;
                    }
                }
                

            }
        }
        catch
        {
            return false;
        }
        
               

        return false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out _DocumentSectionID);

            _dtMonthData = new DataTable();
            _dtMonthData.Clear();

            _dtMonthData.Columns.Add("RecordID", typeof(int));
            _dtMonthData.Columns.Add("RecordDate", typeof(DateTime));
            _dtMonthData.Columns.Add("DisplayField", typeof(string));


            string strCellToolTip = @"var mouseX;
                                var mouseY;
                                $(document).mousemove(function (e) {
                                    try
                                    {
                                        mouseX = e.pageX;
                                        mouseY = e.pageY;
                                    }
                                    catch (err)
                                    {
                                       // alert(err.message)
                                    }
        
                                });
    

                                $(function () {
        
                                    $('.js-tooltip-container').hover(function () {
                                        //$(this).find('.js-tooltip').show();
                                        try {
                                            $(this).find('.js-tooltip').addClass('ajax-tooltip');
                                            $(this).find('.ajax-tooltip').css({ 'top': mouseY, 'left': mouseX }).fadeIn('slow');
                                        }
                                        catch (err) {
                                           // alert(err.message);
                                        }
                                    }, function () {
                                        try {
                                            $(this).find('.js-tooltip').hide();
                                            $(this).find('.js-tooltip').removeClass('ajax-tooltip');
                                            $(this).find('.ajax-tooltip').css({ 'top': mouseY, 'left': mouseX }).fadeOut('slow');
                                        }
                                        catch (err) {
                                            //alert(err.message);
                                        }
                                    });
       
                                });";



           
              ScriptManager.RegisterStartupScript(this, this.GetType(), "strCellToolTip", strCellToolTip, true);
           


            if (!IsPostBack)
            {
                //ViewState["ViewType"] = "m";

                if (Session["ViewType" + _DocumentSectionID.ToString()] != null && Session["MonthDate" + _DocumentSectionID.ToString()] != null)
                {
                    ViewState["ViewType"] = Session["ViewType" + _DocumentSectionID.ToString()].ToString();
                    ViewState["MonthDate"] = (DateTime)Session["MonthDate" + _DocumentSectionID.ToString()];
                    cldDate.TodaysDate = (DateTime)ViewState["MonthDate"];
                    //if (ViewState["ViewType"].ToString() == "m")
                    //{
                    //    DateTime dtTemp = (DateTime)ViewState["MonthDate"];
                    //    dtTemp = new DateTime(dtTemp.Year, dtTemp.Month, 1, 0, 0, 0);
                    //    ViewState["MonthDate"] = dtTemp;
                    //}
                }
                else
                {
                    ViewState["ViewType"] = GetDefaultView();
                    ViewState["MonthDate"] = DateTime.Today;
                }

               

                if (ViewState["ViewType"].ToString() == "m")
                {
                    lnkMonthWeekView.Text = "Week View";                  
                }
                else
                {
                    lnkMonthWeekView.Text = "Month View";
                    SetMonday();
                   
                }

                PopulateCalerdar();
            }

            

           
        }
        catch(Exception ex)
        {
            //

        }


    }

    protected string GetDefaultView()
    {
        using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
        {

            DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == _DocumentSectionID);
            if (section != null)
            {
                CalendarSectionDetail calDetail = JSONField.GetTypedObject<CalendarSectionDetail>(section.Details);
                if (calDetail != null)
                {
                    if(calDetail.CalendarDefaultView!=null && calDetail.CalendarDefaultView!="")
                    {
                        if(calDetail.CalendarDefaultView=="week")
                        {
                            return "w";
                        }
                    }

                }
            }
        }


        return "m";
    }
    protected void SetMonday()
    {
        DateTime input = (DateTime)ViewState["MonthDate"];
        int delta = DayOfWeek.Monday - input.DayOfWeek;
        DateTime monday = input.AddDays(delta);
        if (delta>0)
        {
            monday = monday.AddDays(-7);
        }
        ViewState["MonthDate"] = monday;
    }

    protected void PopulateCalerdar()
    {

        if (ViewState["ViewType"].ToString() == "m")
        {
           
            lblTitle.Text = ((DateTime)ViewState["MonthDate"]).ToString("MMMM yyyy");

            lnkNext.ToolTip = "Go to the next month";
            lnkPre.ToolTip = "Go to the previous month";
        }
        else
        {
            lblTitle.Text="Week " + TheDatabaseS.GetIso8601WeekOfYear((DateTime)ViewState["MonthDate"]);
            lnkNext.ToolTip = "Go to the next week";
            lnkPre.ToolTip = "Go to the previous week";
        }
        Session["ViewType" + _DocumentSectionID.ToString()] = ViewState["ViewType"].ToString();
        Session["MonthDate" + _DocumentSectionID.ToString()] = (DateTime)ViewState["MonthDate"];
        //First Chart
        using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
        {

            DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == _DocumentSectionID);
            if (section != null)
            {
                CalendarSectionDetail calDetail = JSONField.GetTypedObject<CalendarSectionDetail>(section.Details);
                if (calDetail != null)
                 {
                     if (calDetail.TableID != null  & calDetail.DateFieldColumnID!=null)
                     {
                         _strTableID = calDetail.TableID.ToString();

                         Table theTable = RecordManager.ets_Table_Details(int.Parse(_strTableID));
                         if (calDetail.CalendarTitle != null && calDetail.CalendarTitle!="")
                         {
                             lblHeading.Text = calDetail.CalendarTitle;
                         }
                         else
                         {
                             if (theTable != null)
                             {
                                 lblHeading.Text = theTable.TableName;
                             }
                         }

                         if (calDetail.ShowAddRecordIcon != null)
                         {
                             if((bool)calDetail.ShowAddRecordIcon)
                             {
                                 hlAddNewRecord.Visible = true;
                                 hlAddNewRecord.Target = "_parent";
                                 hlAddNewRecord.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?fixedurl=" + Cryptography.Encrypt("~/Default.aspx") + "&stackzero=yes&mode="
                                    + Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(_strTableID) + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                             }
                         }

                         Column theDateColumn = RecordManager.ets_Column_Details((int)calDetail.DateFieldColumnID);

                         DateTime calMonthDate = (DateTime)ViewState["MonthDate"];
                         DateTime firstDayOfMonth = new DateTime(calMonthDate.Year, calMonthDate.Month, 1);
                         DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                         string strTextSearch = " CONVERT(Datetime,Record." + theDateColumn.SystemName + ",103) >= CONVERT(Datetime,'" + firstDayOfMonth.ToShortDateString() + "',103)"; ;
                         strTextSearch = strTextSearch + " AND CONVERT(Datetime,Record." + theDateColumn.SystemName + ",103) <= CONVERT(Datetime,'" + lastDayOfMonth.ToShortDateString() + "',103)";


                         if (calDetail.FilterTextSearch != null && calDetail.FilterTextSearch != "")
                         {
                             strTextSearch = calDetail.FilterTextSearch + " AND (" + strTextSearch + ")";
                         }



                   //      DataTable dtRecords = Common.DataTableFromText("SELECT * FROM Record WHERE IsActive=1 AND TableID=" + calDetail.TableID.ToString()
                         //+ " AND " + theDateColumn.SystemName + " IS NOT NULL AND " + strTextSearch);  allcolumns

                         int iTN=0;
                         //DataTable dtRecords = RecordManager.ets_Record_List((int)calDetail.TableID, null, true, null, null, null, "DBGSystemRecordID", "DESC", null, null, ref iTN, ref iTN, "nonstandard", "",
                         //    strTextSearch, null, null, "", "", "", null);
                         string strReturnSQL = "";
                         DataTable dtRecords = RecordManager.ets_Record_List((int)calDetail.TableID, null, true, null, null, null, "DBGSystemRecordID", "DESC", null, null, ref iTN, ref iTN, "allcolumns", "",
                            strTextSearch, null, null, "", "", "", null, ref strReturnSQL, ref strReturnSQL);

                         DataTable dtColumns = Common.DataTableFromText("SELECT ColumnID,SystemName,DisplayName,ColumnType,NumberType,LinkedParentColumnID,TableTableID,DisplayColumn  FROM [Column] WHERE IsStandard=0 AND   TableID="
                               + calDetail.TableID.ToString() + "  ORDER BY DisplayName");

                         DataTable dtColour = null;
                         if (calDetail.TextColourInfo != null && calDetail.TextColourInfo != "")
                         {
                             string strXML = calDetail.TextColourInfo;

                             XmlDocument xmlDoc = new XmlDocument();
                             xmlDoc.LoadXml(strXML);

                             XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

                             DataSet ds = new DataSet();
                             ds.ReadXml(r);
                             if (ds.Tables[0] != null)
                             {
                                 dtColour = ds.Tables[0];
                                 bool bCompareOperatorFound = false;
                                 foreach (DataColumn dc in dtColour.Columns)
                                 {
                                     if (dc.ColumnName.ToLower() == "CompareOperator".ToLower())
                                     {
                                         bCompareOperatorFound = true;
                                         break;
                                     }
                                 }
                                 if (bCompareOperatorFound == false)
                                 {
                                     dtColour.Columns.Add("CompareOperator");
                                     dtColour.AcceptChanges();
                                 }
                             }
                                 
                         }

                         DataTable _dtRecordColums = RecordManager.ets_Table_Columns_All(int.Parse(_strTableID));
                             DataTable dtPT = Common.DataTableFromText(@"SELECT distinct TC.ParentTableID,TableName FROM TableChild TC INNER JOIN [Table] T
                                                ON TC.ParentTableID=T.TableID
                                                 WHERE ChildTableID=" + _strTableID); //AND DetailPageType<>'not'

                         foreach (DataRow dr in dtRecords.Rows)
                         {
                             if (dr[theDateColumn.DisplayName] != DBNull.Value)
                             {
                                 string strEachFieldDisplay = calDetail.FieldDisplay;

                                 foreach (DataRow drC in dtColumns.Rows)
                                 {

                                     if (strEachFieldDisplay.IndexOf("TIME[" + drC["DisplayName"].ToString() + "]") > -1)
                                     {
                                         if (dr[drC["DisplayName"].ToString()].ToString().LastIndexOf(" ") > -1)
                                         {
                                             strEachFieldDisplay = strEachFieldDisplay.Replace("TIME[" + drC["DisplayName"].ToString() + "]", "TIME:"
                                                 + dr[drC["DisplayName"].ToString()].ToString().Substring(dr[drC["DisplayName"].ToString()].ToString().LastIndexOf(" ")));
                                         }
                                         else
                                         {
                                             strEachFieldDisplay = strEachFieldDisplay.Replace("TIME[" + drC["DisplayName"].ToString() + "]", "TIME:"
                                                                                          + dr[drC["DisplayName"].ToString()].ToString());
                                         }
                                     }
                                 }


                                 foreach (DataRow drC in dtColumns.Rows)
                                 {
                                     strEachFieldDisplay = strEachFieldDisplay.Replace("[" + drC["DisplayName"].ToString() + "]", dr[drC["DisplayName"].ToString()].ToString());
                                 }



                                 try
                                 {

                                     //Work with 1 top level Parent tables.

                                     if (dtPT.Rows.Count > 0)
                                     {

                                         foreach (DataRow drPT in dtPT.Rows)
                                         {
                                             if (strEachFieldDisplay.IndexOf("[" + drPT["TableName"].ToString() + ":") == -1)
                                             {
                                                 continue;
                                             }
                                            
                                             for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                                             {
                                            //     if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value && _dtRecordColums.Rows[i]["LinkedParentColumnID"] != DBNull.Value
                                            //&& (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                            //|| _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                                            // && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                            //&& _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                                            //     {
                                                     if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                                         && dr.Table.Columns.Contains("**" + _dtRecordColums.Rows[i]["DisplayName"].ToString() + "_ID**")
                                                         && _dtRecordColums.Rows[i]["TableTableID"].ToString() == drPT["ParentTableID"].ToString())
                                                     {
                                                         if (dr["**" + _dtRecordColums.Rows[i]["DisplayName"].ToString() + "_ID**"].ToString() != "")
                                                         {
                                                             Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));
                                                             DataTable dtParentRecord = null;
                                                             if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                                             {
                                                                 dtParentRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + dr["**" +_dtRecordColums.Rows[i]["DisplayName"].ToString() + "_ID**"].ToString());
                                                             }
                                                             else
                                                             {
                                                                 dtParentRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "='" +
                                                                     dr["**" + _dtRecordColums.Rows[i]["DisplayName"].ToString() + "_ID**"].ToString().Replace("'", "''") + "'");
                                                             }

                                                             if (dtParentRecord.Rows.Count > 0)
                                                             {

                                                                 DataTable dtColumnsPT = Common.DataTableFromText(@"SELECT distinct SystemName, TableName + ':' + DisplayName AS DP FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE  [Column].IsStandard=0 AND  [Column].TableID=" + drPT["ParentTableID"].ToString());
                                                                 foreach (DataRow drC in dtColumnsPT.Rows)
                                                                 {
                                                                     strEachFieldDisplay = strEachFieldDisplay.Replace("[" + drC["DP"].ToString() + "]", dtParentRecord.Rows[0][drC["SystemName"].ToString()].ToString());

                                                                 }
                                                             }
                                                         }
                                                     }
                                                 //}
                                             }
                                         }
                                     }


                                 }
                                 catch
                                 {
                                     //
                                 }




                                 DateTime dtTemp;


                                
                                if (dtColour != null)
                                {
                                    foreach (DataRow drC in dtColumns.Rows)
                                    {
                                        foreach (DataRow drTC in dtColour.Rows)
                                        {
                                            if (drC["ColumnID"].ToString() == drTC[0].ToString()                                               
                                               && drTC[2].ToString() != "" && strEachFieldDisplay != "")
                                            {
                                                string strCompareOperator = "";

                                                if (drTC[3] != DBNull.Value)
                                                    strCompareOperator = drTC[3].ToString();

                                                string strRecordDisplayValue = dr[drC["DisplayName"].ToString()].ToString();

                                                if (drC["TableTableID"] != DBNull.Value && drC["DisplayColumn"] != DBNull.Value
                                                    && dtRecords.Columns.Contains("**" + drC["DisplayName"].ToString() + "_ID**"))
                                                {
                                                    if (dr["**" + drC["DisplayName"].ToString() + "_ID**"]!=DBNull.Value)
                                                    {
                                                        strRecordDisplayValue = dr["**" + drC["DisplayName"].ToString() + "_ID**"].ToString(); 
                                                    }
                                                    
                                                }

                                                if (IsDataInFilterRange(drC["ColumnType"].ToString(), drC["NumberType"].ToString(),
                                                    strRecordDisplayValue, drTC[1].ToString(), strCompareOperator))
                                                {
                                                    strEachFieldDisplay = "<span style='color:#" + drTC[2].ToString() + "'>" + strEachFieldDisplay + "</span>";
                                                }
                                                
                                            }

                                        }
                                    }                                         
                                }




                                string strTempDate = dr[theDateColumn.DisplayName].ToString();
                                 if (strTempDate.IndexOf(" ")>-1)
                                 {
                                     strTempDate = strTempDate.Substring(0, strTempDate.IndexOf(" "));
                                 }

                                 if (DateTime.TryParseExact(strTempDate, Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                 {
                                     _dtMonthData.Rows.Add(int.Parse(dr["DBGSystemRecordID"].ToString()), dtTemp, strEachFieldDisplay);
                                    
                                 }



                                 //_dtMonthData.Rows.Add(int.Parse(dr["RecordID"].ToString()),
                             }
                         }  
                     }

                 }

            }

        }


               

    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        if (hfSourceDate.Value != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(hfSourceDate.Value, Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                ViewState["MonthDate"] = dtTemp;
                lnkMonthWeekView_OnClick(null, null);
            }
        }
        hfSourceDate.Value = "";
    }
    protected void cldDate_DayRender(object sender, DayRenderEventArgs e)
    {


        e.Cell.Controls.Clear();

        if (ViewState["ViewType"].ToString() == "w")
        {
            DateTime dtMonday = (DateTime)ViewState["MonthDate"];


            if (e.Day.Date >= dtMonday && e.Day.Date < dtMonday.AddDays(7))
            {
                //go ahead
                e.Cell.Height = 630;
            }
            else
            {
                e.Cell.Visible = false;
                return;
            }
        }
        else
        {
           // e.Cell.Style.Add("height","105");
            e.Cell.Height = 105;
        }


        DataRow[] rows = _dtMonthData.Select("RecordDate=#" + e.Day.Date.Month.ToString() + "/" +
            e.Day.Date.Day.ToString() + "/" + e.Day.Date.Year.ToString() + "#");

        string strDisplay = "";
        int i = 1;
        bool bSkip = false;
        foreach (DataRow dr in rows)
        {

            if (ViewState["ViewType"].ToString() == "m")
            {
                if (i > 2)
                {
                    //bSkip = true;
                }
            }

            if (bSkip)
            {

            }
            else
            {
                strDisplay = strDisplay + "<div style='padding-top:5px;padding-left:5px;padding-right:5px;color:#000000;font-size:10px;font-family:Arial;'><div ><a  target='_parent' href='"
                     + "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?fixedurl=" + Cryptography.Encrypt("~/Default.aspx") + "&stackzero=yes&mode="
                     + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(_strTableID) + "&SearchCriteriaID=" + Cryptography.Encrypt("-1")
                     + "&RecordID=" + Cryptography.Encrypt(dr["RecordID"].ToString()) +
                     "' style='text-decoration:none;'>" + dr["DisplayField"].ToString() + " </a> </div></div>";
            }
            i = i + 1;
            //break;
        }

        
        if (ViewState["ViewType"].ToString() == "m")
        {
            string strOriginalDisplay = strDisplay;

            if (strDisplay!="")//i > 3
            {
               
                //int j = i - 2-1;
                int j = i-1;
                string strNoTagDislay = Common.StripTagsCharArray(strDisplay);

                int count = 0;
                string line;
                TextReader reader = new StringReader(strNoTagDislay);
                while ((line = reader.ReadLine()) != null)
                {
                    count++;
                }
                reader.Close();

                if (strNoTagDislay.Length > 75) ////|| count>4
                {
                    
                    strDisplay = "<div style='padding-top:5px;display:block;overflow: hidden;font-weight: normal;'>" + strNoTagDislay.Substring(0, 74) + ".." + "</div>"; //height:55px;
                    string strClick = "<a onclick=\"SetWeekView('" + e.Day.Date.ToShortDateString() + "')\" class='BottomRightLink' >=" + j.ToString() + "</a>"; //width:135px;height:15px;

                    string strCellContent = "</br><div class='js-tooltip-container'> <div class='js-tooltip' style='display:none;'><span>" + strOriginalDisplay + "</span> </div>";
                    strCellContent = strCellContent + "<div  style='text-align:right;padding-right:7px;'><span>" + strClick + "</span></div>"; //
                                        strDisplay = strDisplay + strCellContent;
                
                }
                else
                {
                    strDisplay = "<div style='font-weight: normal;'>" + strOriginalDisplay + "</div>"; //height:75px;
                }
               
            }
            else
            {
                strDisplay = "<div style='font-weight: normal;'>" + strOriginalDisplay + "</div>"; //height:75px;
            }
        }


        string strDateday = e.Day.Date.Day.ToString();

        if (ViewState["ViewType"].ToString() == "m")
        {
            if (e.Day.Date == DateTime.Today)
            {
                strDateday = "<a onclick=\"SetWeekView('" + e.Day.Date.ToShortDateString() + "')\" style='color:Gray;cursor: pointer;text-decoration:none;font-size:13px;' >" + e.Day.Date.Day.ToString() + " (today)</a>";
            }
            else
            {
                strDateday = "<a onclick=\"SetWeekView('" + e.Day.Date.ToShortDateString() + "')\" style='color:Gray;cursor: pointer;text-decoration:none;' >" + e.Day.Date.Day.ToString() + "</a>";
            }
            
        }
        else
        {
            if (e.Day.Date == DateTime.Today)
            {
                strDateday = "<span style='font-size:13px;' >" + e.Day.Date.Day.ToString() + " (today)</span>";
            }
        }

        strDisplay = strDateday + strDisplay;

        if (e.Day.Date == DateTime.Today)
        {
            e.Cell.Controls.Add(new LiteralControl("<div class='TodayTop'><strong>" + strDisplay +  "</strong></div>"));

        }
        else if (e.Day.Date.Month == ((DateTime)ViewState["MonthDate"]).Date.Month)
        {
            e.Cell.Controls.Add(new LiteralControl("<div class='OtherDayTop'><strong>" + strDisplay + "</strong></div>"));

        }
        else
        {
            e.Cell.Controls.Add(new LiteralControl("<div class='OtherMonthDayTop'><strong>" +strDisplay + "</strong></div>"));
            //e.Cell.Height = 200;
            //e.Cell.Visible = false;
        }



       

        //foreach (DataRow dr in rows)
        //{
        //    e.Cell.Controls.Add(new LiteralControl("</br><div style='padding-left:5px;padding-right:5px;'><div class='eachschedule'><a  target='_parent' href='"
        //        + "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?stackzero=yes&mode="
        //        + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(_strTableID) + "&SearchCriteriaID="+ Cryptography.Encrypt("-1")
        //        + "&RecordID=" + Cryptography.Encrypt(dr["RecordID"].ToString()) +
        //        "' style='font-size:9pt;text-decoration:none;'>" + dr["DisplayField"].ToString() + " </a> </div></div>"));

        //    break;
        //}


    }


    protected void cldDate_SelectionChanged(object sender, EventArgs e)
    {


    }

    protected void cldDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {

        ViewState["MonthDate"] = e.NewDate;
        PopulateCalerdar();


        //cldDate.SelectedDate = DateTime.Now;
        //ViewState["MonthDate"] = DateTime.Now;
        //PopulateCalerdar();

       
    }

}