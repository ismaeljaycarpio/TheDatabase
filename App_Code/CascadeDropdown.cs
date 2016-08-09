using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using AjaxControlToolkit;
using System.Collections.Specialized;
using System.Data;
using System.Text.RegularExpressions;
/// <summary>
/// Summary description for CascadeDropdown
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CascadeDropdown : System.Web.Services.WebService {

    public CascadeDropdown () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }


    [WebMethod (EnableSession=true)]
    public CascadingDropDownNameValue[] GetTables(string knownCategoryValues, string category)
    {
        int iTN = 0;
        List<Table> listTable = RecordManager.ets_Table_Select(null,
                 null,
                 null,
                 int.Parse(Session["AccountID"].ToString()),
                 null, null, true,
                 "st.TableName", "ASC",
                 null, null, ref  iTN, Session["STs"].ToString());

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();
        
        foreach (Table eachTable in listTable)
        {
            if (eachTable.TableID.ToString() != Session["TableID"].ToString())
            {
                l.Add(new CascadingDropDownNameValue(eachTable.TableName, eachTable.TableID.ToString()));
            }
        }
        l.Add(new CascadingDropDownNameValue("User", "-1"));
        return l.ToArray();
        //return categorydetails.ToArray();

    }



    [WebMethod(EnableSession = true)]
    public CascadingDropDownNameValue[] GetRelatedTablesCompare(string knownCategoryValues, string category)
    {



        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

       

        DataTable dtTemp = Common.DataTableFromText(@"SELECT DISTINCT TC.ParentTableID,TableName
                        FROM TableChild TC INNER JOIN [Table] T
                        ON TC.ParentTableID=T.TableID
                        WHERE TC.ChildTableID=" + Session["TableID"].ToString() + " ORDER BY TableName");

       

        foreach (DataRow dr in dtTemp.Rows)
        {
            l.Add(new CascadingDropDownNameValue(dr["TableName"].ToString(), dr["ParentTableID"].ToString()));
        }


        //Now add sibling

        string strParentTableIDs = "";
        foreach (CascadingDropDownNameValue cv in l)
        {
            strParentTableIDs = cv.value + "," + strParentTableIDs;
        }

        strParentTableIDs = strParentTableIDs + "-1";

        DataTable dtOtherTable = Common.DataTableFromText(@"SELECT DISTINCT TC.ChildTableID,TableName
                        FROM TableChild TC INNER JOIN [Table] T
                        ON TC.ChildTableID=T.TableID
                        WHERE TC.ChildTableID<> " + Session["TableID"].ToString() + " AND TC.ParentTableID IN (" + strParentTableIDs + ") ORDER BY TableName");


        foreach (DataRow dr in dtOtherTable.Rows)
        {
            l.Add(new CascadingDropDownNameValue(dr["TableName"].ToString(), dr["ChildTableID"].ToString()));
        }


        l.Insert(0,new CascadingDropDownNameValue("--Please Select--", ""));

        Table theTable = RecordManager.ets_Table_Details(int.Parse(Session["TableID"].ToString()));

        if (theTable != null)
        {
            l.Insert(1,new CascadingDropDownNameValue(theTable.TableName, theTable.TableID.ToString()));
        }

        return l.ToArray();


    }


    [WebMethod(EnableSession = true)]
    public CascadingDropDownNameValue[] GetRelatedTables(string knownCategoryValues, string category)
    {


        DataTable dtTemp = Common.DataTableFromText(@"SELECT DISTINCT TC.ParentTableID,TableName
                        FROM TableChild TC INNER JOIN [Table] T
                        ON TC.ParentTableID=T.TableID
                        WHERE TC.ChildTableID=" + Session["TableID"].ToString() + " ORDER BY TableName");

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

        foreach (DataRow dr in dtTemp.Rows)
        {
            l.Add(new CascadingDropDownNameValue(dr["TableName"].ToString(), dr["ParentTableID"].ToString()));
        }


        //Now add sibling

        string strParentTableIDs = "";
        foreach (CascadingDropDownNameValue cv in l)
        {
            strParentTableIDs = cv.value + "," + strParentTableIDs;
        }

        strParentTableIDs = strParentTableIDs + "-1";

        DataTable dtOtherTable = Common.DataTableFromText(@"SELECT DISTINCT TC.ChildTableID,TableName
                        FROM TableChild TC INNER JOIN [Table] T
                        ON TC.ChildTableID=T.TableID
                        WHERE TC.ChildTableID<> " + Session["TableID"].ToString() + " AND TC.ParentTableID IN (" + strParentTableIDs + ") ORDER BY TableName");


        foreach (DataRow dr in dtOtherTable.Rows)
        {
            l.Add(new CascadingDropDownNameValue(dr["TableName"].ToString(), dr["ChildTableID"].ToString()));
        }


        return l.ToArray();


    }


    [WebMethod(EnableSession = true)]
    public CascadingDropDownNameValue[] GetParentTables(string knownCategoryValues, string category)
    {
       

        DataTable dtTemp = Common.DataTableFromText(@"SELECT DISTINCT TC.ParentTableID,TableName
                        FROM TableChild TC INNER JOIN [Table] T
                        ON TC.ParentTableID=T.TableID
                        WHERE TC.ChildTableID=" + Session["TableID"].ToString() + " ORDER BY TableName");
            
        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

        foreach (DataRow dr in dtTemp.Rows)
        {
            l.Add(new CascadingDropDownNameValue(dr["TableName"].ToString(), dr["ParentTableID"].ToString()));
        }

       
        return l.ToArray();
        

    }



    [WebMethod]
    public CascadingDropDownNameValue[] GetLinkedColumnValues(string knownCategoryValues, string category)
    {
        int iRecordID;
      
        StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        iRecordID = Convert.ToInt32(categorydetails["undefined"]);

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();
        Column theColumn = RecordManager.ets_Column_Details(int.Parse(category));


        if (theColumn != null)
        {
            if (theColumn.TableTableID != null)
            {
                string regex = @"\[(.*?)\]";
                string strDisplayColumn = theColumn.DisplayColumn;
                string text = theColumn.DisplayColumn;

                string strDCForSQL = strDisplayColumn.Replace("'", "''");
                int i = 1;
                string strFirstSystemName = "";
                string strFirstDisplayName = "";
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
                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR),'') +'");
                    }
                    else
                    {
                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR),'') +'");
                    }
                    i = i + 1;
                }
                //strDCForSQL = strDCForSQL.Trim();

                //strDCForSQL = strDCForSQL.Substring(0, strDCForSQL.Length - 2);
                strDCForSQL = strDCForSQL.Trim() + "'";

                //get system name

                DataTable dtSys = Common.DataTableFromText(" SELECT SystemName FROM [Column] WHERE   TableID=" + theColumn.TableTableID.ToString() + " and ColumnType='dropdown' and DropdownType='table'");

                if (dtSys.Rows.Count > 0)
                {
                    strFirstSystemName = dtSys.Rows[0][0].ToString();
                }

                DataTable dtData = Common.DataTableFromText(@"SELECT RecordID," + strDCForSQL + @"
                    FROM Record WHERE  IsActive= 1 AND 
                    TableID=" + theColumn.TableTableID.ToString() + " and CAST(" + strFirstSystemName + @" AS VARCHAR) = '" + iRecordID.ToString() + @"'");

                foreach (DataRow dr in dtData.Rows)
                {
                    l.Add(new CascadingDropDownNameValue(dr[1].ToString(), dr[0].ToString()));
                    //IDnText theIDnText = new IDnText();
                    //theIDnText.ID = dr[0].ToString();
                    //theIDnText.Text = dr[1].ToString();
                    //lstIDnText.Add(theIDnText);
                }

            }

        }

        //return lstIDnText;


        //l.Add(new CascadingDropDownNameValue("city 1", "city 1"));
        //l.Add(new CascadingDropDownNameValue("city 2", "city 2"));

        return l.ToArray();
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetFilteredData(string knownCategoryValues, string category)
    {
        string  strFilterText;
        //This method will return a StringDictionary containing the name/value pairs of the currently selected values
        StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        strFilterText = categorydetails["undefined"].ToString();

        if (categorydetails.Keys.Count > 1)
        {
            string strTempCat = knownCategoryValues.Substring(0, knownCategoryValues.Length - 1);
            strFilterText = strTempCat.Substring(strTempCat.LastIndexOf(":") + 1);
        }

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

        int iColumnID=int.Parse(category.Substring(0,category.IndexOf(",")));
        int iParentColumnID=int.Parse(category.Substring(category.IndexOf(",")+1));


        Column theColumn = RecordManager.ets_Column_Details(iColumnID);
        Column scParentColumn = RecordManager.ets_Column_Details(iParentColumnID);
        if (theColumn != null)
        {
            if (theColumn.TableTableID != null)
            {
//                string regex = @"\[(.*?)\]";
//                string strDisplayColumn = theColumn.DisplayColumn;
//                string text = theColumn.DisplayColumn;

//                string strDCForSQL = strDisplayColumn.Replace("'", "''");
//                int i = 1;
//                string strFirstSystemName = "";
//                string strFirstDisplayName = "";
//                foreach (Match match in Regex.Matches(text, regex))
//                {
//                    string strEachDisplayName = match.Groups[1].Value;

//                    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName FROM [Column] WHERE   TableID ="
//                        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

//                    string strEachSystemName = "";
//                    if (dtTableTableSC.Rows.Count > 0)
//                    {
//                        strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
//                    }


//                    if (i == 1)
//                    {

//                        strFirstDisplayName = strEachDisplayName;
//                        strFirstSystemName = strEachSystemName;
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    else
//                    {
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    i = i + 1;
//                }
//                strDCForSQL = strDCForSQL.Trim() + "'";

//                //get system name

//                DataTable dtSys = Common.DataTableFromText(" SELECT SystemName FROM [Column] WHERE   TableID=" + theColumn.TableTableID.ToString() + " and ColumnType='dropdown' and (DropdownType='table' OR DropdownType='tabledd')");

//                if (dtSys.Rows.Count > 0)
//                {
//                    strFirstSystemName = dtSys.Rows[0][0].ToString();
//                }
//                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

//                DataTable dtData = Common.DataTableFromText(@"SELECT " + theLinkedColumn.SystemName + "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " and CAST(" + scParentColumn.SystemName + @" AS VARCHAR) = '" + strFilterText.Replace("'","''").ToString() + @"'");

                string strWhere = "";

                //if(theColumn.FilterOperator=="" || theColumn.FilterOperator=="equals")
                //{
                //    strWhere = " AND Record." + scParentColumn.SystemName + "=" + strFilterText + "";
                //}
                //else
                //{

                //}
                strWhere = " AND Record." + scParentColumn.SystemName + "=" + strFilterText + "";
                DataTable dtData = Common.spGetLinkedRecordIDnDisplayText(theColumn.DisplayColumn, (int)theColumn.TableTableID, null, strWhere, "");


                if (dtData.Rows.Count > 0)
                {
                    l.Add(new CascadingDropDownNameValue("--Please select--", ""));
                }
                
                foreach (DataRow dr in dtData.Rows)
                {
                    l.Add(new CascadingDropDownNameValue(dr[1].ToString(), dr[0].ToString()));
                }

            }

        }

     

        return l.ToArray();


    }



//    [WebMethod]
//    public CascadingDropDownNameValue[] GetFilteredData(string knownCategoryValues, string category)
//    {
//        string strFilterText;
//        //This method will return a StringDictionary containing the name/value pairs of the currently selected values
//        StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
//        strFilterText = categorydetails["undefined"].ToString();

//        if (categorydetails.Keys.Count > 1)
//        {
//            string strTempCat = knownCategoryValues.Substring(0, knownCategoryValues.Length - 1);
//            strFilterText = strTempCat.Substring(strTempCat.LastIndexOf(":") + 1);
//        }

//        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

//        int iColumnID = int.Parse(category.Substring(0, category.IndexOf(",")));
//        int iParentColumnID = int.Parse(category.Substring(category.IndexOf(",") + 1));


//        Column theColumn = RecordManager.ets_Column_Details(iColumnID);
//        Column scParentColumn = RecordManager.ets_Column_Details(iParentColumnID);
//        if (theColumn != null)
//        {
//            if (theColumn.TableTableID != null)
//            {
//                string regex = @"\[(.*?)\]";
//                string strDisplayColumn = theColumn.DisplayColumn;
//                string text = theColumn.DisplayColumn;

//                string strDCForSQL = strDisplayColumn.Replace("'", "''");
//                int i = 1;
//                string strFirstSystemName = "";
//                string strFirstDisplayName = "";
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
//                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    else
//                    {
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    i = i + 1;
//                }
//                //strDCForSQL = strDCForSQL.Trim();

//                //strDCForSQL = strDCForSQL.Substring(0, strDCForSQL.Length - 2);
//                strDCForSQL = strDCForSQL.Trim() + "'";

//                //get system name

//                DataTable dtSys = Common.DataTableFromText(" SELECT SystemName FROM [Column] WHERE   TableID=" + theColumn.TableTableID.ToString() + " and ColumnType='dropdown' and (DropdownType='table' OR DropdownType='tabledd')");

//                if (dtSys.Rows.Count > 0)
//                {
//                    strFirstSystemName = dtSys.Rows[0][0].ToString();
//                }
//                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

//                DataTable dtData = Common.DataTableFromText(@"SELECT " + theLinkedColumn.SystemName + "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " and CAST(" + scParentColumn.SystemName + @" AS VARCHAR) = '" + strFilterText.Replace("'", "''").ToString() + @"'");

//                if (dtData.Rows.Count > 0)
//                {
//                    l.Add(new CascadingDropDownNameValue("--Please select--", ""));
//                }


//                foreach (DataRow dr in dtData.Rows)
//                {
//                    l.Add(new CascadingDropDownNameValue(dr[1].ToString(), dr[0].ToString()));
//                    //IDnText theIDnText = new IDnText();
//                    //theIDnText.ID = dr[0].ToString();
//                    //theIDnText.Text = dr[1].ToString();
//                    //lstIDnText.Add(theIDnText);
//                }

//            }

//        }



//        return l.ToArray();


//    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetFilteredColumns(string knownCategoryValues, string category)
    {
        int iTableID;
        //This method will return a StringDictionary containing the name/value pairs of the currently selected values
        StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        iTableID = Convert.ToInt32(categorydetails["Tableid"]);

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

        if (iTableID < 0)
        {
            l.Add(new CascadingDropDownNameValue("--NA--", ""));
            return l.ToArray();
        }

        int iTN = 0;
        DataTable dtColumns = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE   
                TableID=" + iTableID.ToString() + @" ");
        
        foreach (DataRow dr in dtColumns.Rows)
        {
            l.Add(new CascadingDropDownNameValue(dr["DisplayName"].ToString(), dr["ColumnID"].ToString()));
        }
        
        return l.ToArray();
        //return categorydetails.ToArray();


    }


    [WebMethod]
    public CascadingDropDownNameValue[] GetDefaultValueOption(string knownCategoryValues, string category)
    {
        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

        l.Add(new CascadingDropDownNameValue("None", "none"));
        l.Add(new CascadingDropDownNameValue("Value", "value"));
        l.Add(new CascadingDropDownNameValue("From Related Table", "parent"));
        try
        {
            int iTableID;
            //This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            iTableID = Convert.ToInt32(categorydetails["Tableid"]);

            if(iTableID>0)
            {
                string strColumnID = Common.GetValueFromSQL(@"SELECT TOP 1 ColumnID FROM [Column] WHERE TableID="+iTableID.ToString()+@" AND 
                                ColumnType='dropdown' AND DisplayColumn IS NOT NULL
                                            AND TableTableID=-1");

                if(strColumnID!="")
                {
                    l.Add(new CascadingDropDownNameValue("From Login", "login"));
                }
            }

        }
        catch
        {
            //
        }

        return l.ToArray();
        

    }

    [WebMethod] 
    public CascadingDropDownNameValue[] GetColumns(string knownCategoryValues, string category)
    {

        int iTableID;
        //This method will return a StringDictionary containing the name/value pairs of the currently selected values
        StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        iTableID = Convert.ToInt32(categorydetails["Tableid"]);

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

        if (iTableID < 0)
        {
            l.Add(new CascadingDropDownNameValue("Email", "email"));
            l.Add(new CascadingDropDownNameValue("Name", "name"));
            return l.ToArray();
        }


        int iTN = 0;
        List<Column> lstColumns = RecordManager.ets_Table_Columns(iTableID,
               null, null, ref iTN);
        Column dtColumn = new Column();

       


        l.Add(new CascadingDropDownNameValue("--Advanced--", ""));

               
        foreach (Column eachColumn in lstColumns)
        {
           
            if (eachColumn.IsStandard == false)
            {
                l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString()));
            }
        }
        return l.ToArray();
        //return categorydetails.ToArray();

    }


    [WebMethod]
    public CascadingDropDownNameValue[] GetColumnsWithDefault(string knownCategoryValues, string category)
    {

        int iTableID;
        //This method will return a StringDictionary containing the name/value pairs of the currently selected values
        StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        iTableID = Convert.ToInt32(categorydetails["Tableid"]);

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();
        if (iTableID < 0)
        {
            l.Add(new CascadingDropDownNameValue("Email", "email",true));
            l.Add(new CascadingDropDownNameValue("Name", "name"));
            return l.ToArray();
        }


        int iTN = 0;
        List<Column> lstColumns = RecordManager.ets_Table_Columns(iTableID,
               null, null, ref iTN);
        Column dtColumn = new Column();

       
        l.Add(new CascadingDropDownNameValue("--Advanced--", ""));

        int? iFirstTextColumn = null;

        foreach (Column eachColumn in lstColumns)
        {
            if (eachColumn.IsStandard == false)
            {
                if (eachColumn.ColumnType == "text")
                {
                    iFirstTextColumn = eachColumn.ColumnID;
                    //l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString()));
                    break;
                }
            }
        }

        bool bFirstTextColumn = false;
        foreach (Column eachColumn in lstColumns)
        {
            bFirstTextColumn = false;
            if (eachColumn.IsStandard == false)
            {
                if (iFirstTextColumn == null)
                {
                }
                else
                {
                    if (iFirstTextColumn == eachColumn.ColumnID)
                    {
                        bFirstTextColumn = true;
                    }
                }
                if (bFirstTextColumn)
                {
                    l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString(), true));
                }
                else
                {
                    l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString()));
                }
            }
        }
        return l.ToArray();
        //return categorydetails.ToArray();

    }


    [WebMethod]
    public CascadingDropDownNameValue[] GetColumnsLink(string knownCategoryValues, string category)
    {

        int iTableID;
        //This method will return a StringDictionary containing the name/value pairs of the currently selected values
        StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        iTableID = Convert.ToInt32(categorydetails["Tableid"]);

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

        if(iTableID<0)
        {
            l.Add(new CascadingDropDownNameValue("--NA--", ""));
            return l.ToArray();
        }

        int iTN = 0;
        List<Column> lstColumns = RecordManager.ets_Table_Columns(iTableID,
               null, null, ref iTN);
        Column dtColumn = new Column();

     
        l.Add(new CascadingDropDownNameValue("--Select--", ""));

        //string strRecordIDColumnID = "";

        foreach (Column eachColumn in lstColumns)
        {
            //System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
            //ddDDDisplayColumn.Items.Insert(ddDDDisplayColumn.Items.Count, aItem);
            if (eachColumn.IsStandard == false)
            {
                if (eachColumn.ColumnType.ToLower() != "datetime" && eachColumn.ColumnType.ToLower() != "date"
                    && eachColumn.ColumnType.ToLower() != "time") //&& eachColumn.TableTableID == null
                {
                    l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString()));
                }
            }
            else
            {
                if (eachColumn.SystemName.ToLower() == "recordid")
                {
                    bool bEdit = false;
                    if (category == "edit")
                    {
                        bEdit = true;
                    }
                    l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString(), !bEdit));
                    //strRecordIDColumnID = eachColumn.ColumnID.ToString();
                }
            }
        }

       
        return l.ToArray();
        //return categorydetails.ToArray();

    }



    [WebMethod]
    public CascadingDropDownNameValue[] GetDefaultParentColumns(string knownCategoryValues, string category)
    {

        int iTableID;
        //This method will return a StringDictionary containing the name/value pairs of the currently selected values
        StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        iTableID = Convert.ToInt32(categorydetails["Tableid"]);
        int iTN = 0;
        List<Column> lstColumns = RecordManager.ets_Table_Columns(iTableID,
               null, null, ref iTN);
        Column dtColumn = new Column();

        List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();
        l.Add(new CascadingDropDownNameValue("--Select--", ""));

        //string strRecordIDColumnID = "";

        foreach (Column eachColumn in lstColumns)
        {
            //System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.DisplayName, eachColumn.ColumnID.ToString());
            //ddDDDisplayColumn.Items.Insert(ddDDDisplayColumn.Items.Count, aItem);
            if (eachColumn.IsStandard == false)
            {
                //if (eachColumn.ColumnType.ToLower() != "datetime" && eachColumn.ColumnType.ToLower() != "date"
                //    && eachColumn.ColumnType.ToLower() != "time") //&& eachColumn.TableTableID == null
                //{
                    l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString()));
                //}
            }
            else
            {
                if (eachColumn.SystemName.ToLower() == "recordid")
                {
                    bool bEdit = false;
                    if (category == "edit")
                    {
                        bEdit = true;
                    }
                    l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString(), !bEdit));
                    //strRecordIDColumnID = eachColumn.ColumnID.ToString();
                }

                if (eachColumn.SystemName.ToLower() == "datetimerecorded")
                {
                    l.Add(new CascadingDropDownNameValue(eachColumn.DisplayName, eachColumn.ColumnID.ToString()));
                }

            }
        }


        return l.ToArray();
       

    }


    //[WebMethod]
    //public string[] GetTestList(string strAccountName)
    //{
    //    strAccountName=strAccountName.Replace("'","''");
    //    DataTable dtAccounts = Common.DataTableFromText("SELECT AccountName FROM Account WHERE AccountName like '" + strAccountName + "%'");

    //    List<string> lstAccounts=new List<string>();
    //    foreach (DataRow dr in dtAccounts.Rows)
    //    {
    //        lstAccounts.Add(dr[0].ToString());
    //    }

    //    return lstAccounts.ToArray();
    //}

    [WebMethod]
    public List<Account> GetTestList(string AccountName)
    {
        AccountName = AccountName.Replace("'", "''");
        DataTable dtAccounts = Common.DataTableFromText("SELECT * FROM Account WHERE AccountName like '%" + AccountName + "%'");

        List<Account> lstAccounts = new List<Account>();
        foreach (DataRow dr in dtAccounts.Rows)
        {
            Account aAccount = new Account(int.Parse(dr["AccountID"].ToString()), dr["AccountName"].ToString(),
                null, null, null, null);

            lstAccounts.Add(aAccount);
        }

        return lstAccounts;
    }


    [WebMethod]
    public List<IDnText> GetDisplayColumns(string Columnid, string search)
    {
        search = search.Replace("'", "''");

        //string regex = @"\[(.*?)\]";

        Column theColumn = RecordManager.ets_Column_Details(int.Parse(Columnid));
        List<IDnText> lstIDnText = new List<IDnText>();

        if (theColumn != null)
        {
            if (theColumn.TableTableID != null && (int)theColumn.TableTableID != -1)
            {
//                string strDisplayColumn = theColumn.DisplayColumn;
//                string text = theColumn.DisplayColumn;

//                string strDCForSQL = strDisplayColumn.Replace("'", "''");
//                int i = 1;
//                string strFirstSystemName = "";
                
//                foreach (Match match in Regex.Matches(text, regex))
//                {
//                    string strEachDisplayName = match.Groups[1].Value;

//                    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,ColumnType FROM [Column] WHERE   TableID ="
//                        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

//                    string strEachSystemName = "";
//                    if (dtTableTableSC.Rows.Count > 0)
//                    {
//                        if (dtTableTableSC.Rows[0]["ColumnType"].ToString() == "date")
//                        {
//                            strEachSystemName = "CONVERT(VARCHAR, CONVERT (DATE," + dtTableTableSC.Rows[0]["SystemName"].ToString() + ",103),103)";
//                        }
//                        else
//                        {
//                            strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
//                        }

//                    }


//                    if (i == 1)
//                    {

//                        strFirstSystemName = strEachSystemName;
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    else
//                    {
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    i = i + 1;
//                }
//                strDCForSQL = strDCForSQL.Trim() + "'";

//                //strDCForSQL = strDCForSQL.Substring(0, strDCForSQL.Length - 2);

//                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

                string strWhere = "";

                if (theColumn.FilterParentColumnID != null && theColumn.FilterValue != "")
                {
                    Column theFilterParentColumnID = RecordManager.ets_Column_Details((int)theColumn.FilterParentColumnID);
                    //strWhere = " AND Record." + theFilterParentColumnID.SystemName + "='" + theColumn.FilterValue.Replace("'", "''") + "'";

                    if (theColumn.FilterOperator == "contains")
                    {
                        strWhere = " AND Record." + theFilterParentColumnID.SystemName + " LIKE '%" + theColumn.FilterValue.Replace("'", "''") + "%'";
                    }
                    else if (theColumn.FilterOperator == "greaterthan")
                    {
                        strWhere = " AND ( IsNumeric(Record." + theFilterParentColumnID.SystemName + ")=1 AND  CONVERT(decimal(20,10),Record." + theFilterParentColumnID.SystemName + ") > " + Common.IgnoreSymbols(theColumn.FilterValue) + ")";
                    }
                    else if (theColumn.FilterOperator == "lessthan")
                    {
                        strWhere = " AND ( IsNumeric(Record." + theFilterParentColumnID.SystemName + ")=1 AND  CONVERT(decimal(20,10),Record." + theFilterParentColumnID.SystemName + ") < " + Common.IgnoreSymbols(theColumn.FilterValue) + ")";
                    }
                    else if (theColumn.FilterOperator == "empty")
                    {
                        strWhere = " AND (Record." + theFilterParentColumnID.SystemName + " IS NULL OR LEN(Record." + theFilterParentColumnID.SystemName + ")=0) ";
                    }
                    else if (theColumn.FilterOperator == "notempty")
                    {
                        strWhere = " AND (Record." + theFilterParentColumnID.SystemName + " IS NOT NULL AND LEN(Record." + theFilterParentColumnID.SystemName + ")>0) ";
                    }
                    else
                    {
                        strWhere = " AND Record." + theFilterParentColumnID.SystemName + " IN ('" + theColumn.FilterValue.Replace("'", "''").Replace(",", "','") + "')";

                    }
                }



//                DataTable dtData = Common.DataTableFromText(@"SELECT TOP 1000 " + theLinkedColumn.SystemName + "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " " + strWhere + " and CAST(" + strFirstSystemName + @" AS VARCHAR) like '%" + search.Replace("'", "''") + @"%'");


                DataTable dtData = Common.spGetLinkedRecordIDnDisplayText(theColumn.DisplayColumn, (int)theColumn.TableTableID, 100, strWhere, search.Replace("'", "''"));

                foreach (DataRow dr in dtData.Rows)
                {
                    IDnText theIDnText = new IDnText();
                    theIDnText.ID = dr[0].ToString();
                    theIDnText.Text = dr[1].ToString();
                    lstIDnText.Add(theIDnText);
                }

            }
            else if (theColumn.TableTableID != null && (int)theColumn.TableTableID == -1)
            {
                try
                {
                    if (theColumn.DisplayColumn != "")
                    {
                        Table thisTable = RecordManager.ets_Table_Details((int)theColumn.TableID);
                        DataTable dtData;
                        if (theColumn.DisplayColumn == "[Name]")
                        {
                            dtData = Common.DataTableFromText(@"SELECT * FROM (SELECT U.UserID,ISNULL(U.FirstName,'') +' ' + ISNULL(U.LastName,'') AS FullName 
                            FROM [User] U INNER JOIN [UserRole] UR ON U.UserID=UR.UserID 
                            WHERE U.IsActive=1 AND UR.AccountID=" + thisTable.AccountID.ToString() + @") FU WHERE FullName LIKE '%" + search + @"%' ORDER BY FullName");
                        }
                        else
                        {
                            dtData = Common.DataTableFromText(@"SELECT U.UserID,U.Email 
                                                    FROM [User] U INNER JOIN [UserRole] UR
                                                    ON U.UserID=UR.UserID WHERE  U.IsActive=1 AND U.Email LIKE'%" + search + @"%' AND  UR.AccountID=" + thisTable.AccountID.ToString() + @" ORDER BY U.Email ");
                        }


                        foreach (DataRow dr in dtData.Rows)
                        {

                            IDnText theIDnText = new IDnText();
                            theIDnText.ID = dr[0].ToString();
                            theIDnText.Text = dr[1].ToString();
                            lstIDnText.Add(theIDnText);
                        }

                    }
                }
                catch
                {
                    //
                }
            }

        }


        return lstIDnText;
    }


//    [WebMethod]
//    public List<IDnText> GetDisplayColumns(string Columnid, string search)
//    {
//        search = search.Replace("'", "''");

//        string regex = @"\[(.*?)\]";
       
//        Column theColumn = RecordManager.ets_Column_Details(int.Parse(Columnid));
//        List<IDnText> lstIDnText = new List<IDnText>();

//        if (theColumn != null)
//        {
//            if (theColumn.TableTableID != null && (int)theColumn.TableTableID!=-1)
//            {
//                //DataTable dtTableTableSC = Common.DataTableFromText("SELECT * FROM [Column] WHERE  TableID =" + theColumn.TableTableID.ToString());
//                string strDisplayColumn = theColumn.DisplayColumn;
//                string text = theColumn.DisplayColumn;

//                string strDCForSQL = strDisplayColumn.Replace("'", "''");
//                int i = 1;
//                string strFirstSystemName = "";
//                //string strFirstDisplayName = "";
//                //List<string> lstDisplayName = new List<string>();
//                //List<string> lstSystemName = new List<string>();
//                foreach (Match match in Regex.Matches(text, regex))
//                {
//                    string strEachDisplayName= match.Groups[1].Value;

//                    //lstDisplayName.Add(strEachDisplayName);

//                    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,ColumnType FROM [Column] WHERE   TableID ="
//                        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

//                    string strEachSystemName = "";
//                    if (dtTableTableSC.Rows.Count > 0)
//                    {
//                        //strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
//                        if (dtTableTableSC.Rows[0]["ColumnType"].ToString() == "date")
//                        {
//                            strEachSystemName = "CONVERT(VARCHAR, CONVERT (DATE," + dtTableTableSC.Rows[0]["SystemName"].ToString() + ",103),103)";
//                        }
//                        else
//                        {
//                            strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
//                        }
                        
//                    }


//                    if (i == 1)
//                    {

//                        strFirstSystemName = strEachSystemName;
//                        strDCForSQL = strDCForSQL.Replace("[" +strEachDisplayName+ "]",
//                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    else
//                    {
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    i = i + 1;
//                }
//                strDCForSQL = strDCForSQL.Trim() + "'";

//                //strDCForSQL = strDCForSQL.Substring(0, strDCForSQL.Length - 2);

//                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

//                string strWhere = "";

//                if (theColumn.FilterParentColumnID != null && theColumn.FilterValue != "")
//                {
//                    Column theFilterParentColumnID = RecordManager.ets_Column_Details((int)theColumn.FilterParentColumnID);
//                    strWhere = " AND " + theFilterParentColumnID.SystemName + "='" + theColumn.FilterValue.Replace("'", "''") + "'";
//                }



//                DataTable dtData = Common.DataTableFromText(@"SELECT TOP 1000 " + theLinkedColumn.SystemName+ "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " " + strWhere + " and CAST(" + strFirstSystemName + @" AS VARCHAR) like '%" + search.Replace("'", "''") + @"%'");

//                foreach (DataRow dr in dtData.Rows)
//                {
//                    IDnText theIDnText = new IDnText();
//                    theIDnText.ID = dr[0].ToString();
//                    theIDnText.Text = dr[1].ToString();
//                    lstIDnText.Add(theIDnText);
//                }

//            }
//            else if (theColumn.TableTableID != null && (int)theColumn.TableTableID == -1)
//            {
//                try
//                {
//                    if (theColumn.DisplayColumn != "")
//                    {
//                        Table thisTable = RecordManager.ets_Table_Details((int)theColumn.TableID);
//                        DataTable dtData;
//                        if (theColumn.DisplayColumn == "[Name]")
//                        {
//                            dtData = Common.DataTableFromText(@"SELECT * FROM (SELECT U.UserID,ISNULL(U.FirstName,'') +' ' + ISNULL(U.LastName,'') AS FullName 
//                            FROM [User] U INNER JOIN [UserRole] UR ON U.UserID=UR.UserID 
//                            WHERE U.IsActive=1 AND UR.AccountID="+thisTable.AccountID.ToString()+@") FU WHERE FullName LIKE '%"+search+@"%' ORDER BY FullName");
//                        }
//                        else
//                        {
//                            dtData = Common.DataTableFromText(@"SELECT U.UserID,U.Email 
//                                                    FROM [User] U INNER JOIN [UserRole] UR
//                                                    ON U.UserID=UR.UserID WHERE  U.IsActive=1 AND U.Email LIKE'%" + search + @"%' AND  UR.AccountID=" + thisTable.AccountID.ToString() + @" ORDER BY U.Email ");
//                        }
                        

//                        foreach (DataRow dr in dtData.Rows)
//                        {

//                            IDnText theIDnText = new IDnText();
//                            theIDnText.ID = dr[0].ToString();
//                            theIDnText.Text = dr[1].ToString();
//                            lstIDnText.Add(theIDnText);
//                        }

//                    }
//                }
//                catch
//                {
//                    //
//                }
//            }

//        }
        
       
//        return lstIDnText;
//    }


//    [WebMethod]
//    public List<IDnText> GetFilteredValue_TB(string Columnid, string search)
//    {
//        search = search.Replace("'", "''");

//        string regex = @"\[(.*?)\]";

//        Column theColumn = RecordManager.ets_Column_Details(int.Parse(Columnid));
//        List<IDnText> lstIDnText = new List<IDnText>();

//        if (theColumn != null)
//        {
//            if (theColumn.TableTableID != null)
//            {
//                //DataTable dtTableTableSC = Common.DataTableFromText("SELECT * FROM [Column] WHERE  TableID =" + theColumn.TableTableID.ToString());
//                string strDisplayColumn = theColumn.DisplayColumn;
//                string text = theColumn.DisplayColumn;

//                string strDCForSQL = strDisplayColumn.Replace("'", "''");
//                int i = 1;
//                string strFirstSystemName = "";
//                //string strFirstDisplayName = "";
//                //List<string> lstDisplayName = new List<string>();
//                //List<string> lstSystemName = new List<string>();
//                foreach (Match match in Regex.Matches(text, regex))
//                {
//                    string strEachDisplayName = match.Groups[1].Value;

//                    //lstDisplayName.Add(strEachDisplayName);

//                    DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,ColumnType FROM [Column] WHERE   TableID ="
//                        + theColumn.TableTableID.ToString() + " AND DisplayName='" + strEachDisplayName + "'");

//                    string strEachSystemName = "";
//                    if (dtTableTableSC.Rows.Count > 0)
//                    {
//                        //strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
//                        if (dtTableTableSC.Rows[0]["ColumnType"].ToString() == "date")
//                        {
//                            strEachSystemName = "CONVERT(VARCHAR, CONVERT (DATE," + dtTableTableSC.Rows[0]["SystemName"].ToString() + ",103),103)";
//                        }
//                        else
//                        {
//                            strEachSystemName = dtTableTableSC.Rows[0]["SystemName"].ToString();
//                        }

//                    }


//                    if (i == 1)
//                    {

//                        strFirstSystemName = strEachSystemName;
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    else
//                    {
//                        strDCForSQL = strDCForSQL.Replace("[" + strEachDisplayName + "]",
//                            "'+ ISNULL(CAST(" + strEachSystemName + " AS VARCHAR(max)),'') +'");
//                    }
//                    i = i + 1;
//                }
//                strDCForSQL = strDCForSQL.Trim() + "'";

//                //strDCForSQL = strDCForSQL.Substring(0, strDCForSQL.Length - 2);

//                Column theLinkedColumn = RecordManager.ets_Column_Details((int)theColumn.LinkedParentColumnID);

//                string strWhere = "";

//                if (theColumn.FilterParentColumnID != null && theColumn.FilterValue != "")
//                {
//                    Column theFilterParentColumnID = RecordManager.ets_Column_Details((int)theColumn.FilterParentColumnID);
//                    strWhere = " AND " + theFilterParentColumnID.SystemName + "='" + theColumn.FilterValue.Replace("'", "''") + "'";
//                }

//                DataTable dtData = Common.DataTableFromText(@"SELECT TOP 1000 " + theLinkedColumn.SystemName + "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " " + strWhere + " and CAST(" + strFirstSystemName + @" AS VARCHAR) like '%" + search.Replace("'", "''") + @"%'");


//                if (theColumn.FilterParentColumnID != null && theColumn.FilterOtherColumnID != null)
//                {
//                    Column theFilterParentColumnID = RecordManager.ets_Column_Details((int)theColumn.FilterParentColumnID);
//                    //strWhere = " AND " + theFilterParentColumnID.SystemName + "='" + theColumn.FilterValue.Replace("'", "''") + "'";

//                    dtData = Common.DataTableFromText(@"SELECT " + theLinkedColumn.SystemName + "," + strDCForSQL + @"
//                    FROM Record WHERE  IsActive= 1 AND 
//                    TableID=" + theColumn.TableTableID.ToString() + " and CAST(" + theFilterParentColumnID.SystemName + @" AS VARCHAR) = '" + search.Replace("'", "''").ToString() + @"'");
                
//                }



               
//                foreach (DataRow dr in dtData.Rows)
//                {
//                    IDnText theIDnText = new IDnText();
//                    theIDnText.ID = dr[0].ToString();
//                    theIDnText.Text = dr[1].ToString();
//                    lstIDnText.Add(theIDnText);
//                }

//            }

//        }


//        return lstIDnText;
//    }
}
