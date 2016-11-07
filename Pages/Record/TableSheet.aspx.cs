using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.IO;
public partial class Pages_Record_TableSheet : SecurePage
{
    User _ObjUser;
    Table _theTable;
    string _strNone = "";
  
    string _strFilesPhisicalPath = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Add " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table");
        _ObjUser = (User)Session["User"];

        
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();


        if (!IsPostBack)
        {
            DataTable dtActiveTableList = Common.DataTableFromText("SELECT * FROM [Table] WHERE IsActive=1 AND AccountID=" + Session["AccountID"].ToString());
            if (dtActiveTableList.Rows.Count == 0)
            {

                hfFirstTable.Value = "Yes";
            }


            PopulateHelp("TableSpreadSheetRole");
            PopulateTerminology();
            PopulateRecordGroupDDL(int.Parse(Session["AccountID"].ToString()));
            if (Request.UrlReferrer != null)
            {
                hlBack.NavigateUrl = Request.UrlReferrer.ToString();
            }
            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableOption.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            //FileInfo

            if (Request.QueryString["FileInfo"] != null)
            {
                PopulateFileInfo(int.Parse(Cryptography.Decrypt(Request.QueryString["FileInfo"].ToString())));
            }
        }

//        string strHelpJS = @" $(function () {
//            $('#hlHelpCommon').fancybox({
//                scrolling: 'auto',
//                type: 'iframe',
//                'transitionIn': 'elastic',
//                'transitionOut': 'none',
//                width: 600,
//                height: 350,
//                titleShow: false
//            });
//        });";


//        ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);


    }


    protected void PopulateRecordGroupDDL(int iAccountID)
    {
        //ddlMenu.Items.Clear();
        int iTemp = 0;
        DataTable dtMenu = RecordManager.ets_Menu_Select(null, string.Empty, null,
            iAccountID, true,
            "Menu", "ASC", null, null, ref iTemp,null,null);


        TheDatabaseS.PopulateMenuDDL(ref ddlMenu);

        System.Web.UI.WebControls.ListItem liNew = new System.Web.UI.WebControls.ListItem("--New--", "new");
        ddlMenu.Items.Insert(0, liNew);

        foreach (DataRow dr in dtMenu.Rows)
        {
            if (dr["Menu"].ToString() == "--None--" && dr["ParentMenuID"] == DBNull.Value)
            {
                _strNone = dr["MenuID"].ToString();
                //lstMenuSelect.Remove(aMenu);
                if (ddlMenu.Items.FindByValue(_strNone) != null)
                    ddlMenu.Items.Remove(ddlMenu.Items.FindByValue(_strNone));

                System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("--None--", _strNone);
                ddlMenu.Items.Insert(1, liNone);
                break;
            }
        }

           
        

       

        if (!IsPostBack)
        {

            foreach (DataRow dr in dtMenu.Rows)
            {
                if (dr["Menu"].ToString() == "Tables")
                {
                    ddlMenu.SelectedValue = dr["MenuID"].ToString();
                    break;
                }
            }
        }

    }

    protected void PopulateTerminology()
    {
        //stgTable.InnerText = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + "*:";
        //rfvTable.ErrorMessage = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " - Required";

        lblTitle.Text = "Add " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " From Spreadsheet";
    }

    protected void PopulateHelp(string strContentKey)
    {
        Content theContent = SystemData.Content_Details_ByKey(strContentKey, null);

        if (theContent != null)
        {
            lblHelpContent.Text = theContent.ContentP;
        }
    }


    protected void PopulateFileInfo(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                //lblExcelFileName.Text = xmlDoc.FirstChild["FileName"].InnerText;

                txtTable.Text = xmlDoc.FirstChild["txtTable"].InnerText;

                chkRecordsData.Checked=bool.Parse( xmlDoc.FirstChild["chkRecordsData"].InnerText);
                txtImportColumnHeaderRow.Text = xmlDoc.FirstChild["txtImportColumnHeaderRow"].InnerText;
                txtImportDataStartRow.Text = xmlDoc.FirstChild["txtImportDataStartRow"].InnerText;

                ddlMenu.SelectedValue = xmlDoc.FirstChild["ddlMenuValue"].InnerText;
                txtNewMenuName.Text = xmlDoc.FirstChild["txtNewMenuName"].InnerText;          
                               

            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }
    protected int GetFileInforSC(Guid guidNew, string strFileExtension)
    {

        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                     " <FileName>" + HttpUtility.HtmlEncode(fuRecordFile.FileName) + "</FileName>" +
                   " <txtTable>" + HttpUtility.HtmlEncode(txtTable.Text) + "</txtTable>" +
                   " <chkRecordsData>" + HttpUtility.HtmlEncode(chkRecordsData.Checked.ToString()) + "</chkRecordsData>" +
                   " <guidNew>" + HttpUtility.HtmlEncode(guidNew.ToString()) + "</guidNew>" +
                   " <ddlMenuText>" + HttpUtility.HtmlEncode(ddlMenu.SelectedItem.Text) + "</ddlMenuText>" +
                   " <ddlMenuValue>" + HttpUtility.HtmlEncode(ddlMenu.SelectedValue) + "</ddlMenuValue>" +
                   " <txtNewMenuName>" + HttpUtility.HtmlEncode(txtNewMenuName.Text) + "</txtNewMenuName>" +
                   " <txtImportColumnHeaderRow>" + HttpUtility.HtmlEncode(txtImportColumnHeaderRow.Text) + "</txtImportColumnHeaderRow>" +
                   " <txtImportDataStartRow>" + HttpUtility.HtmlEncode(txtImportDataStartRow.Text) + "</txtImportDataStartRow>" +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            return SystemData.SearchCriteria_Insert(theSearchCriteria);
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
        return -1;
        //End Searchcriteria

    }
    //protected int GetFileInforSC(Guid guidNew, string strFileExtension)
    //{

    //    //SearchCriteria 
    //    try
    //    {
    //        string xml = null;
    //        xml = @"<root>" +
    //               " <txtBatchDescription>" + HttpUtility.HtmlEncode(txtBatchDescription.Text) + "</txtBatchDescription>" +
    //               " <FileName>" + HttpUtility.HtmlEncode(fuRecordFile.FileName) + "</FileName>" +
    //               " <guidNew>" + HttpUtility.HtmlEncode(guidNew.ToString()) + "</guidNew>" +
    //               " <ddlLocation>" + HttpUtility.HtmlEncode(ddlLocation.SelectedValue.ToString()) + "</ddlLocation>" +
    //              "</root>";

    //        SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
    //        return SystemData.SearchCriteria_Insert(theSearchCriteria);
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message;
    //    }
    //    return -1;
    //    //End Searchcriteria

    //}


    protected void lnkSave_Click(object sender, EventArgs e)
    {
        //lblMsg.Visible = true;
        lblMsg.Text = "";

        //if (ddlMenu.SelectedValue == "-1")
        //{
        //    lblMsg.Text = "Please select a menu.";
        //    ddlMenu.Focus();
        //    return;
        //}


        if (txtTable.Text.Trim() == "")
        {
            lblMsg.Text = "Please enter " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + ".";
            txtTable.Focus();
            return;
        }


        int iParentMenuID = -1;

        if (ddlMenu.SelectedValue == "new")
        {
            if (txtNewMenuName.Text == "")
            {
                lblMsg.Text = "New Menu Name - Required.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "trNewMenuName2", "$('#trNewMenuName').fadeIn();", true);
                return;
            }

            if (hfFirstTable.Value == "Yes")
            {
                DataTable dtActiveMenuList = Common.DataTableFromText("SELECT * FROM [Menu] WHERE Menu='Tables' AND AccountID=" + Session["AccountID"].ToString());

                if (dtActiveMenuList.Rows.Count == 0)
                {
                    txtNewMenuName.Text = "Tables";
                }
            }

        }

        //Session["CurrentBatch"] = txtBatchDescription.Text;
       
        Guid guidNew = Guid.NewGuid();

        string strFileExtension = "";

        switch (fuRecordFile.FileName.Substring(fuRecordFile.FileName.LastIndexOf('.') + 1).ToLower())
        {
            case "dbf":
                strFileExtension = ".dbf";
                break;
            case "txt":
                strFileExtension = ".txt";
                break;
            case "xls":
                strFileExtension = ".xls";
                break;
            case "xlsx":
                strFileExtension = ".xlsx";
                break;
            case "csv":
                strFileExtension = ".csv";
                break;
            case "xml":
                strFileExtension = ".xml";
                break;
            default:
                strFileExtension = "";
                break;
        }



        string strFileUniqueName;
        strFileUniqueName = guidNew.ToString() + strFileExtension;
        if (fuRecordFile.HasFile)
            try
            {

                if (strFileExtension == "")
                {
                    lblMsg.Text = "Please select a .xls/.xlsx/.csv file";
                    return;
                }


                //fuRecordFile.SaveAs(Server.MapPath("../../UserFiles/AppFiles") + "\\" + strFileUniqueName);

                fuRecordFile.SaveAs( _strFilesPhisicalPath + "\\UserFiles\\AppFiles" + "\\" + strFileUniqueName);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
                return;
            }
        else
        {
            lblMsg.Text = "You have not specified a file.";
            return;
        }



        //check if it has multiple sheets
        string strSelectedSheet = "";
        int iSCID = GetFileInforSC(guidNew, strFileExtension);
        if (strFileExtension == ".xls" || strFileExtension == ".xlsx")
        {

            //List<string> lstSheets = OfficeManager.GetExcelSheetNames(Server.MapPath("../../UserFiles/AppFiles"), strFileUniqueName);

            List<string> lstSheets = OfficeManager.GetExcelSheetNames( _strFilesPhisicalPath + "\\UserFiles\\AppFiles", strFileUniqueName);

            if (lstSheets.Count > 1)
            {
                //strSelectedSheet = lstSheets[0];

               
                Response.Redirect("~/Pages/Record/MultipleSheetsNewTable.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&FileInfo=" + Cryptography.Encrypt(iSCID.ToString()), false);
                return;


            }
            else
            {
               

            }
        }

        /////



        
        string strTemp = "";
        strFileUniqueName = guidNew.ToString() + strFileExtension;

        //string strImportFolder = Server.MapPath("../../UserFiles/AppFiles");

        string strImportFolder = _strFilesPhisicalPath +  "\\UserFiles\\AppFiles";

        DataTable dtImportFileTable;


        dtImportFileTable = null;

        int iRowIndex = 1;
        string strMsg = "";
        switch (strFileExtension.ToLower())
        {
            case ".dbf":
                dtImportFileTable = UploadManager.GetImportFileTableFromDBF(strImportFolder, strFileUniqueName, ref strMsg);               
                break;
            case ".txt":
                dtImportFileTable = UploadManager.GetImportFileTableFromText(strImportFolder, strFileUniqueName, ref strMsg);
                break; 
            case ".xls":
                dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet,false);
                break;
            case ".xlsx":
                dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
                break;
            case ".csv":
                dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName, ref strMsg);
                iRowIndex = 1;
                break;
                   
        }

        if (strMsg != "")
        {
            lblMsg.Text = strMsg;
            return;
        }
        //Menu theSTG=RecordManager.ets_Menu_Details(int.Parse(ddlMenu.SelectedValue));
        //Account theAccount = SecurityManager.Account_Details((int)theSTG.AccountID, null, null);

        //if (theAccount != null)
        //{
        //    dtImportFileTable =UploadManager.ReShapDatatable(dtImportFileTable, theAccount.AccountName);
        //}


        int iBatchID = -1;
       
        //string strImportMsg;
        Batch theBatch=null;
        //SqlTransaction tn;
        //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

        //connection.Open();
        //tn = connection.BeginTransaction();

        try
        {

            if (ddlMenu.SelectedValue == "")
            {
                Menu newMenu = new Menu(null, "--None--",
            int.Parse(Session["AccountID"].ToString()), false, true);

                iParentMenuID = RecordManager.ets_Menu_Insert(newMenu);
                
               
            }
            else if (ddlMenu.SelectedValue == "new")
            {
              
                Menu newMenu = new Menu(null, txtNewMenuName.Text,
            int.Parse(Session["AccountID"].ToString()), true, true);

                iParentMenuID = RecordManager.ets_Menu_Insert(newMenu);                
               
            }
            else
            {

                iParentMenuID = int.Parse(ddlMenu.SelectedValue);
            }



            int iColulmnCount = 0;

            Table newTable = new Table(null,
                         txtTable.Text, 
                         null, null,  true);
            newTable.AccountID = int.Parse(Session["AccountID"].ToString());




            int iTableID = RecordManager.ets_Table_Insert(newTable, iParentMenuID);

            Table theTable = RecordManager.ets_Table_Details(iTableID);

            //if (txtImportColumnHeaderRow.Text.Trim() != ""
            //&& txtImportDataStartRow.Text.Trim() != "")
            //{

            //    theTable.ImportColumnHeaderRow = int.Parse(txtImportColumnHeaderRow.Text.Trim());
            //    theTable.ImportDataStartRow = int.Parse(txtImportDataStartRow.Text.Trim());
            //    RecordManager.ets_Table_Update(theTable);
            //}



            //lets check column header

            if (((txtImportColumnHeaderRow.Text.Trim() == "1"
           && txtImportDataStartRow.Text.Trim() == "2") ||
               (txtImportColumnHeaderRow.Text.Trim() == ""
           && txtImportDataStartRow.Text.Trim() == "")) && strFileExtension != ".dbf" && strFileExtension != ".txt" && strFileExtension!=".xml")
            {

                //int iBlankColumnHeadrCount = 0;
                if (dtImportFileTable.Rows.Count > 0)
                {
                    //is there any balnk column header?

                    //if (strFileExtension.ToLower() == ".csv")
                    //{
                    //    int i = 1;
                    //    foreach (DataColumn aColumn in dtImportFileTable.Columns)
                    //    {
                    //        if (aColumn.ColumnName.ToLower() == "column" + i.ToString())
                    //        {
                    //            iBlankColumnHeadrCount = iBlankColumnHeadrCount + 1;
                    //        }
                    //        i = i + 1;
                    //    }
                    //}
                    //else
                    //{
                    //    int i = 1;
                    //    foreach (DataColumn aColumn in dtImportFileTable.Columns)
                    //    {
                    //        if (aColumn.ColumnName.ToLower() == "f" + i.ToString())
                    //        {
                    //            iBlankColumnHeadrCount = iBlankColumnHeadrCount + 1;
                    //        }
                    //        i = i + 1;
                    //    }
                    //}


                    //if (iBlankColumnHeadrCount > 0)
                    //{
                        //
                        List<int> lstColumnCount = new List<int>();

                        int j = 0;
                        //lstColumnCount.Add(iBlankColumnHeadrCount);

                        foreach (DataRow dr in dtImportFileTable.Rows)
                        {
                            int iThisRowBlankColumnCount = 0;
                            foreach (DataColumn aColumn in dtImportFileTable.Columns)
                            {
                                if (dr[aColumn.ColumnName] == DBNull.Value)
                                {
                                    iThisRowBlankColumnCount = iThisRowBlankColumnCount + 1;
                                }
                                else
                                {
                                    if (dr[aColumn.ColumnName].ToString() == "")
                                    {
                                        iThisRowBlankColumnCount = iThisRowBlankColumnCount + 1;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double dTemp=0;
                                            if (double.TryParse(dr[aColumn.ColumnName].ToString(),out dTemp))
                                            {
                                                double dTest = double.Parse(dr[aColumn.ColumnName].ToString());
                                                iThisRowBlankColumnCount = iThisRowBlankColumnCount + 1;
                                            }
                                            else
                                            {                                                
                                                DateTime dateValue;

                                                if (DateTime.TryParseExact(dr[aColumn.ColumnName].ToString(), Common.Dateformats,
                                                new CultureInfo("en-GB"),
                                                DateTimeStyles.None,
                                                out dateValue))
                                                {
                                                    iThisRowBlankColumnCount = iThisRowBlankColumnCount + 1;
                                                }
                                            }                                           
                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }
                                }
                            }

                            lstColumnCount.Add(iThisRowBlankColumnCount);
                            j = j + 1;

                            if (j == 49)
                            {
                                break;
                            }
                        }
                    
                        //get the minimum blank count

                        int iMinValue = lstColumnCount.Min();

                        int iMinIndex = 0;

                        int m = 0;
                        foreach (int aInt in lstColumnCount)
                        {
                            if (aInt == iMinValue)
                            {
                                iMinIndex = m;
                                break;
                            }
                            m = m + 1;
                        }

                        ViewState["ImportColumnHeaderRow"] = iMinIndex + 1;
                        ViewState["ImportDataStartRow"] = iMinIndex + 2;



                        RecordManager.ets_Table_Update(theTable);
                    //}

                }



            }
           

           
            //    && strFileExtension != ".dbf" && strFileExtension != ".txt" && strFileExtension!=".xml"

            if (strFileExtension.ToLower() != ".dbf")
            {
                if (ViewState["ImportColumnHeaderRow"] != null)
                {
                    //if ((int)theTable.ImportColumnHeaderRow > 1)
                    //{
                    if (dtImportFileTable.Rows.Count >= int.Parse(ViewState["ImportColumnHeaderRow"].ToString()))
                    {
                        for (int i = 0; i <= dtImportFileTable.Columns.Count - 1; i++)
                        {
                            if (dtImportFileTable.Rows[int.Parse(ViewState["ImportColumnHeaderRow"].ToString()) - 1][i].ToString() == "")
                            {
                                //do nothing for it
                                if (strFileExtension.ToLower() == ".csv")
                                {
                                    try
                                    {
                                        dtImportFileTable.Columns[i].ColumnName = "Column" + (i + 1).ToString();
                                    }
                                    catch
                                    {
                                        //
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (dtImportFileTable.Rows[int.Parse(ViewState["ImportColumnHeaderRow"].ToString()) - 1][i].ToString() != "")
                                        dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[int.Parse(ViewState["ImportColumnHeaderRow"].ToString()) - 1][i].ToString();
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.IndexOf("already belongs to this DataTable") > -1)
                                    {
                                        for (int j = 1; j < 20; j++)
                                        {
                                            bool bOK = true;
                                            foreach (DataColumn dc in dtImportFileTable.Columns)
                                            {
                                                if (dc.ColumnName == dtImportFileTable.Rows[int.Parse(ViewState["ImportColumnHeaderRow"].ToString()) - 1][i].ToString() + j.ToString())
                                                {
                                                    bOK = false;
                                                }
                                            }

                                            if (bOK)
                                            {
                                                dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[int.Parse(ViewState["ImportColumnHeaderRow"].ToString()) - 1][i].ToString() + j.ToString();
                                                dtImportFileTable.AcceptChanges();
                                                break;
                                            }

                                        }

                                    }
                                }
                            }

                        }
                        dtImportFileTable.AcceptChanges();
                    }
                    //}

                }


                if (dtImportFileTable.Columns.Count > 0)
                {
                    if (strFileExtension.ToLower() == ".csv")
                    {
                        if (dtImportFileTable.Columns[0].ColumnName.ToLower() == "column1")
                        {
                            dtImportFileTable.Columns.RemoveAt(0);
                            dtImportFileTable.AcceptChanges();
                        }

                    }
                    else
                    {
                        if (dtImportFileTable.Columns[0].ColumnName.ToLower() == "f1")
                        {
                            dtImportFileTable.Columns.RemoveAt(0);
                            dtImportFileTable.AcceptChanges();
                        }
                    }

                }


                if (ViewState["ImportDataStartRow"] != null)
                {
                    for (int i = 1; i <= int.Parse(ViewState["ImportDataStartRow"].ToString()) - 1; i++)
                    {
                        dtImportFileTable.Rows.RemoveAt(0);

                    }
                    dtImportFileTable.AcceptChanges();

                }





                int xy = 0;
                foreach (DataColumn dc in dtImportFileTable.Columns)
                {
                    dtImportFileTable.Columns[xy].ColumnName = Common.RemoveSpecialCharacters(dc.ColumnName);
                    xy = xy + 1;
                }
                dtImportFileTable.AcceptChanges();


            }

           

            iColulmnCount = 5;


            int iColCount = 0;
            foreach (DataColumn dc in dtImportFileTable.Columns)
            {
                bool bUsedColumn = false;
                iColCount = iColCount + 1;
                if (iColCount > 495)
                {

                    break;
                }


                if (bUsedColumn == false && dc.ColumnName.ToLower().IndexOf("dbgsystemrecordid") == -1)
                {
                    //non standard column
                
                    string strAutoSystemName = "";

                    strAutoSystemName = RecordManager.ets_Column_NextSystemName(iTableID);
                    if (strAutoSystemName == "NO")
                    {
                        lblMsg.Text = "You can add 500 columns, please remove other columns!";
                        //tn.Rollback();
                        //connection.Close();

                        return;
                    }

                    int? iDisplayOrder = RecordManager.ets_Table_MaxOrder(iTableID);

                    if (iDisplayOrder == null)
                        iDisplayOrder = -1;

                    iColulmnCount = iColulmnCount + 1;

                   

                    string strSummaryName = "";
                    string strColumnName = dc.ColumnName.Replace("'", "").Replace(",", "");

                    strSummaryName = strColumnName;
                    if (iColulmnCount > 55)
                    {
                        strSummaryName = "";

                    }

                    Column newColumn = new Column( null, iTableID,
                  strAutoSystemName, iDisplayOrder + 1, strSummaryName, strColumnName, 
                   null, "", "", null, null, "", "", false, strColumnName, "", null,
                   null, null, strColumnName, "", null);

                    newColumn.ColumnType = "text";
                    //newColumn.NumberType = 1; //normal
                    if (dtImportFileTable.Rows.Count > iRowIndex)
                    {
                        if (dtImportFileTable.Rows[iRowIndex][dc.ColumnName] != null)
                        {
                            try
                            {
                                if (dtImportFileTable.Rows[iRowIndex][dc.ColumnName].ToString() != "")
                                {
                                    double dTest = 0;

                                    if(double.TryParse(dtImportFileTable.Rows[iRowIndex][dc.ColumnName].ToString(),out dTest))
                                    {

                                        newColumn.ColumnType = "number";
                                        newColumn.GraphLabel = strColumnName;
                                        newColumn.NumberType = 1; //normal
                                    }
                                    else
                                    {
                                        DateTime dateValue;

                                        if (DateTime.TryParseExact(dtImportFileTable.Rows[iRowIndex][dc.ColumnName].ToString(), Common.Dateformats,
                                      new CultureInfo("en-GB"),
                                      DateTimeStyles.None,
                                      out dateValue))
                                        {
                                            newColumn.ColumnType = "date";
                                            newColumn.NumberType = null;
                                        }
                                        else
                                        {
                                            newColumn.ColumnType = "text";
                                            newColumn.NumberType = null;
                                        }
                                    }                                    
                                }
                            }
                            catch
                            {
                                newColumn.ColumnType = "text";
                                newColumn.NumberType = null;                                
                            }

                        }
                    }



                    try
                    {
                        DataTable dtColumnID = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE TableID="+newColumn.TableID+" AND IsStandard=1 AND DisplayName='"+ newColumn.DisplayName.Replace("'","''") +"'");

                        if(dtColumnID.Rows.Count==0)
                        {

                            if(iColCount==1)
                                newColumn.SummarySearch = true;


                            RecordManager.ets_Column_Insert(newColumn);
                        }
                        else
                        {
                            Column theColumn = RecordManager.ets_Column_Details(int.Parse(dtColumnID.Rows[0][0].ToString()));
                            if (theColumn != null)
                            {
                                theColumn.DisplayTextSummary = newColumn.DisplayTextSummary;
                                theColumn.DisplayTextDetail = newColumn.DisplayTextDetail;
                                //theColumn.Name_OnImport = newColumn.Name_OnImport;
                                RecordManager.ets_Column_Update(theColumn);

                            }
                        }
                    }
                    catch
                    {
                        //
                    }

                }

            }

            //so we have made the Table now lets see if we can make Locations

            //foreach (DataRow dr in dtImportFileTable.Rows)
            //{
            //    try
            //    {

            //        DataTable dtSS = Common.DataTableFromText("SELECT LocationID FROM Location WHERE AccountID=" + Session["AccountID"].ToString() + " AND LocationName='" + dr[strLocation].ToString().Replace("'", "''") + "'", tn, null);
            //        if (dtSS.Rows.Count > 0)
            //        {
            //            List<LocationTable> lstSSTS = SiteManager.ets_LocationTable_Select(iTableID, int.Parse(dtSS.Rows[0][0].ToString()), "", tn, null);

            //            if (lstSSTS.Count > 0)
            //            {


            //            }
            //            else
            //            {
            //                LocationTable theLocationTable = new LocationTable(null, int.Parse(dtSS.Rows[0][0].ToString()), iTableID,
            //                    "", "");

            //                SiteManager.ets_LocationTable_Insert(theLocationTable, tn, null);


            //            }
            //        }
            //        else
            //        {
            //            Location newLocation = new Location(null, dr[strLocation].ToString().Replace("'", "''"),"","", null, null, null, null,null, int.Parse(Session["AccountID"].ToString()), "");

            //            int iLocationID = SiteManager.ets_Location_Insert(newLocation, tn, null);

            //            LocationTable theLocationTable = new LocationTable(null, iLocationID, iTableID,
            //                  "", "");

            //            SiteManager.ets_LocationTable_Insert(theLocationTable, tn, null);

            //        }
            //    }
            //    catch
            //    {
            //        //
            //    }

            //}


            if (chkRecordsData.Checked)
            {
                //lets import the file

                _theTable = RecordManager.ets_Table_Details(iTableID);


                UploadManager.UploadCSV(_ObjUser.UserID, _theTable, fuRecordFile.FileName,
              fuRecordFile.FileName, guidNew, _strFilesPhisicalPath + "\\UserFiles\\AppFiles",
              out strMsg, out iBatchID,  strFileExtension,
              strSelectedSheet, int.Parse(Session["AccountID"].ToString()), null,null, null);

                theBatch = UploadManager.ets_Batch_Details(iBatchID);
                //strImportMsg = UploadManager.ImportClickFucntions(theBatch, ref connection, ref tn);

            }
            
      
            //tn.Commit();
            //connection.Close();
            //connection.Dispose();
            if (chkRecordsData.Checked)
            {
               
             

                //string strImportedRecords = "0";
                //UploadManager.RecordsImportEmail(theBatch, ref strImportedRecords,
                //    Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath);

                //if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())))
                //{
                //    Session["DoNotAllow"] = "true";
                //    Response.Redirect("~/Pages/Security/AccountTypeChange.aspx?type=renew", false);
                //    return;
                //}


                Response.Redirect("~/Pages/Record/UploadValidation.aspx?auto=yes&TableID=" + Cryptography.Encrypt(theBatch.TableID.ToString()) + "&BatchID=" + Cryptography.Encrypt(theBatch.BatchID.ToString()) + "&SearchCriteriaID="
                    + Cryptography.Encrypt("-1") + "&FileInfo=" + Cryptography.Encrypt(iSCID.ToString()), false);
            }
            else
            {
                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?fromsheet=yes&mode="
                    + Cryptography.Encrypt("edit") + "&MenuID=" + Request.QueryString["MenuID"].ToString() + "&TableID=" + Cryptography.Encrypt(iTableID.ToString()) + "#topline", false); 
            }

        }
        catch (Exception ex)
        {

            //tn.Rollback();
            //connection.Close();
            //connection.Dispose();

            //if (ex.Message.IndexOf("Location") > -1)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('The file should have a " +
            //        SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Location", "Location") + " column.');", true);
            //}
            //else 
                
                if (ex.Message.IndexOf("Transaction count") > -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This menu has a same " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " name, please try another name.');", true);
            }
            else if (ex.Message.IndexOf("UQ_SampleTypeDisplayName") > -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('The file has duplicate column name.');", true);
            }
            else if (ex.Message.IndexOf("UQ_STG_StgAndAccountID") > -1)
            {
                lblMsg.Text = "This menu already exist, please try anohter menu name.";
                txtNewMenuName.Focus();
                return;
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Error: " + ex.Message.Replace("'", "") + "');", true);
                
                ErrorLog theErrorLog = new ErrorLog(null, "Table Sheet", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }
            //throw; 

          
        } 
     
    }


   
   

}