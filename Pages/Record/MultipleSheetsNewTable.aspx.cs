using System;
//using System.Collections;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
public partial class Pages_Record_MultipleSheetsNewTable : SecurePage
{

    string _strFilesPhisicalPath = "";
    Table _theTable;
    //Menu _qsMenu;
    User _ObjUser;
    //string _qsTableID = "";
    int _iTotalDynamicColumns = 0;
    int _iValidRecordIDIndex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();


        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2,3,4,5,7,8,9"))
        { Response.Redirect("~/Default.aspx", false); }

        try
        {
            _ObjUser = (User)Session["User"];

          

            if (!IsPostBack)
            {
                if (Request.QueryString["FileInfo"] != null)
                {
                    //PopulateLocationDDL();
                    PopulateRecordGroupDDL(int.Parse(Session["AccountID"].ToString()));
                    PopulateFileInfo(int.Parse(Cryptography.Decrypt(Request.QueryString["FileInfo"].ToString())));
                    //populate sheet drop down
                    Guid guidNew = Guid.Empty;
                    string strFileExtension = "";
                    strFileExtension = hfFileExtension.Value;

                    guidNew = Guid.Parse(hfguidNew.Value);

                    string strFileUniqueName;
                    strFileUniqueName = guidNew.ToString() + strFileExtension;

                    List<string> lstSheets = OfficeManager.GetExcelSheetNames(_strFilesPhisicalPath + "\\UserFiles\\AppFiles", strFileUniqueName);
                    if (lstSheets.Count > 1)
                    {
                        ddlSheetNames.Items.Clear();
                        foreach (string item in lstSheets)
                        {
                            System.Web.UI.WebControls.ListItem liItem = new System.Web.UI.WebControls.ListItem(item, item);
                            ddlSheetNames.Items.Add(liItem);
                        }

                    }

                    //CheckLocationColumn();

                }

                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableSheet.aspx?MenuID=" + Request.QueryString["MenuID"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&FileInfo=" + Request.QueryString["FileInfo"].ToString(); 
            }

            Title = "Multiple Sheets file upload";
            lblTitle.Text = "Multiple Sheets file upload";


        }
        catch (Exception ex)
        {

        }

    }



    protected void PopulateRecordGroupDDL(int iAccountID)
    {
        //ddlMenu.Items.Clear();
        int iTemp = 0;
        string strNone = "";
        DataTable dtMenu = RecordManager.ets_Menu_Select(null, string.Empty, null,
            iAccountID, true,
            "Menu", "ASC", null, null, ref iTemp, null, null);


        TheDatabaseS.PopulateMenuDDL(ref ddlMenu);

        System.Web.UI.WebControls.ListItem liNew = new System.Web.UI.WebControls.ListItem("--New--", "new");
        ddlMenu.Items.Insert(0, liNew);

        foreach (DataRow dr in dtMenu.Rows)
        {
            if (dr["Menu"].ToString() == "--None--" && dr["ParentMenuID"] == DBNull.Value)
            {
                strNone = dr["MenuID"].ToString();
                //lstMenuSelect.Remove(aMenu);

                if (ddlMenu.Items.FindByValue(strNone) != null)
                    ddlMenu.Items.Remove(ddlMenu.Items.FindByValue(strNone));

                System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("--None--", strNone);
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
    protected void PopulateFileInfo(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                lblExcelFileName.Text = xmlDoc.FirstChild["FileName"].InnerText;
                lblTable.Text = xmlDoc.FirstChild["txtTable"].InnerText;

                hfRecordsData.Value= xmlDoc.FirstChild["chkRecordsData"].InnerText;
                if (hfRecordsData.Value.ToLower() == "true")
                {
                    lblRecordsData.Text = "Yes";
                }
                else
                {
                    lblRecordsData.Text = "No";
                }

                //lblImportColumnHeaderRow.Text = xmlDoc.FirstChild["txtImportColumnHeaderRow"].InnerText;
                //lblImportDataStartRow.Text = xmlDoc.FirstChild["txtImportDataStartRow"].InnerText;
                ddlMenu.SelectedValue = xmlDoc.FirstChild["ddlMenuValue"].InnerText;
                txtNewMenuName.Text = xmlDoc.FirstChild["txtNewMenuName"].InnerText; 

                //lblImportColumnHeaderRow.Text = xmlDoc.FirstChild["txtImportColumnHeaderRow"].InnerText;
                //lblImportDataStartRow.Text = xmlDoc.FirstChild["txtImportDataStartRow"].InnerText;
                
                hfguidNew.Value = xmlDoc.FirstChild["guidNew"].InnerText;
                hfFileExtension.Value = "." + lblExcelFileName.Text.Substring(lblExcelFileName.Text.LastIndexOf('.') + 1).ToLower();
              

            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }



    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }




    public static byte[] EncodeString(String sourceData, String charSet)
    {
        byte[] bSourceData = System.Text.Encoding.Unicode.GetBytes(sourceData);
        System.Text.Encoding outEncoding = System.Text.Encoding.GetEncoding(charSet);
        return System.Text.Encoding.Convert(System.Text.Encoding.Unicode, outEncoding, bSourceData);
    }


    protected void lnkNext_Click(object sender, EventArgs e)
    {


        lblMsg.Text = "";





        int iParentMenuID = -1;






        Guid guidNew = Guid.Empty;
        string strFileExtension = "";
        strFileExtension = hfFileExtension.Value;

        guidNew = Guid.Parse(hfguidNew.Value);

        string strFileUniqueName;
        strFileUniqueName = guidNew.ToString() + strFileExtension;


        //check if it has multiple sheets
        string strSelectedSheet = "";

        if (ddlSheetNames.Items.Count > 0)
        {
            strSelectedSheet = ddlSheetNames.Text;
        }


        string strImportFolder = _strFilesPhisicalPath + "\\UserFiles\\AppFiles";


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
                dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, true);
                break;
            case ".xlsx":
                dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, true);
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



        strMsg = "";

        int iBatchID = -1;
        string strImportMsg;
        Batch theBatch = null;

        //SqlTransaction tn;
        //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

        //connection.Open();
        //tn = connection.BeginTransaction();

        try
        {

            ViewState["ImportColumnHeaderRow"] = 1;
            ViewState["ImportDataStartRow"] = 2;
           

            if (ddlMenu.SelectedValue == "")
            {
                Menu newMenu = new Menu(null, "--None--",
            int.Parse(Session["AccountID"].ToString()), false, true);
                try
                {
                    iParentMenuID = RecordManager.ets_Menu_Insert(newMenu);
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("UQ_STG_StgAndAccountID") > -1)
                    {
                        lblMsg.Text = "This menu already exist, please try anohter menu name.";

                        return;
                    }
                }
            }
            else if (ddlMenu.SelectedValue == "new")
            {
                if (txtNewMenuName.Text == "")
                {
                    lblMsg.Text = "New Menu Name - Required.";
                    return;
                }

                Menu newMenu = new Menu(null, txtNewMenuName.Text,
            int.Parse(Session["AccountID"].ToString()), true, true);
                try
                {
                    iParentMenuID = RecordManager.ets_Menu_Insert(newMenu);
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("UQ_STG_StgAndAccountID") > -1)
                    {
                        lblMsg.Text = "This menu already exist, please try anohter menu name.";

                        return;
                    }
                }
            }
            else
            {

                iParentMenuID = int.Parse(ddlMenu.SelectedValue);
            }


            int iColulmnCount = 0;

            Table newTable = new Table(null,
                         lblTable.Text, 
                         null, null, true);
            newTable.AccountID = int.Parse(Session["AccountID"].ToString());




            int iTableID = RecordManager.ets_Table_Insert(newTable, iParentMenuID);


            Table theTable = RecordManager.ets_Table_Details(iTableID);

            //if (lblImportColumnHeaderRow.Text.Trim() != ""
            //&& lblImportDataStartRow.Text.Trim() != "")
            //{
               
            //    theTable.ImportColumnHeaderRow = int.Parse(lblImportColumnHeaderRow.Text.Trim());
            //    theTable.ImportDataStartRow = int.Parse(lblImportDataStartRow.Text.Trim());
            //    RecordManager.ets_Table_Update(theTable);
            //}


            iColulmnCount = 5;




            if (dtImportFileTable.Rows.Count > 0)
            {
                //is there any balnk column header?

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
                                    double dTest = double.Parse(dr[aColumn.ColumnName].ToString());
                                    iThisRowBlankColumnCount = iThisRowBlankColumnCount + 1;
                                }
                                catch
                                {
                                    try
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
                                    catch
                                    {
                                        //
                                    }
                                }

                            }
                        }
                    }


                    lstColumnCount.Add(iThisRowBlankColumnCount);
                    j = j + 1;

                    if (j == 495)
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
                    if (iColulmnCount > 10)
                    {
                        strSummaryName = "";

                    }

                    Column newColumn = new Column(null, iTableID,
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
                                    double dTest = double.Parse(dtImportFileTable.Rows[iRowIndex][dc.ColumnName].ToString());
                                    newColumn.ColumnType = "number";
                                    newColumn.GraphLabel = strColumnName;
                                    newColumn.NumberType = 1; //normal
                                }
                            }
                            catch
                            {

                                //             string[] formats = {"d/M/yyyy h:mm:ss tt", "d/M/yyyy h:mm tt", 
                                //"dd/MM/yyyy hh:mm:ss", "d/M/yyyy h:mm:ss", 
                                //"d/M/yyyy hh:mm tt", "d/M/yyyy hh tt", 
                                //"d/M/yyyy h:mm", "d/M/yyyy h:mm", 
                                //"dd/MM/yyyy hh:mm", "dd/M/yyyy hh:mm"};
                                try
                                {
                                    //string[] formats = {"d/M/yyyy",
                                    //"dd/MM/yyyy","dd/M/yyyy"};

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
                                catch
                                {
                                    newColumn.ColumnType = "text";
                                    newColumn.NumberType = null;
                                }

                            }

                        }
                    }


                    if(iColCount==1)
                        newColumn.SummarySearch = true;

                    RecordManager.ets_Column_Insert(newColumn);

                }

            }



            if (hfRecordsData.Value.ToLower() == "true")
            {
                //lets import the file

                _theTable = RecordManager.ets_Table_Details(iTableID);

                int? iDT = ImportManager.CreateDefaultImportTemplate((int)_theTable.TableID,"DisplayName");
                ImportTemplate theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)iDT);
                theImportTemplate.ImportDataStartRow = int.Parse(ViewState["ImportDataStartRow"].ToString());
                theImportTemplate.ImportColumnHeaderRow = int.Parse(ViewState["ImportColumnHeaderRow"].ToString());
                ImportManager.dbg_ImportTemplate_Update(theImportTemplate);


                UploadManager.UploadCSV(_ObjUser.UserID, _theTable, lblExcelFileName.Text,
              lblExcelFileName.Text, guidNew, _strFilesPhisicalPath + "\\UserFiles\\AppFiles",
              out strMsg, out iBatchID,  strFileExtension,
              strSelectedSheet, int.Parse(Session["AccountID"].ToString()), null, theImportTemplate.ImportTemplateID, null);

                theBatch = UploadManager.ets_Batch_Details(iBatchID);
              

            }


            //tn.Commit();
            //connection.Close();
            //connection.Dispose();
            if (hfRecordsData.Value.ToLower() == "true")
            {

                Response.Redirect("~/Pages/Record/UploadValidation.aspx?auto=yes&TableID=" + Cryptography.Encrypt(theBatch.TableID.ToString()) + "&BatchID=" + Cryptography.Encrypt(theBatch.BatchID.ToString()) + "&SearchCriteriaID=" + Cryptography.Encrypt("-1"), false);
                return;
            }
            else
            {
                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?fromsheet=yes&mode=" + Cryptography.Encrypt("edit") + "&MenuID=" + Cryptography.Encrypt(iParentMenuID.ToString()) + "&TableID=" + Cryptography.Encrypt(iTableID.ToString()) + "#topline", false);
                return;
            }


            
        }
        catch (Exception ex)
        {

            //tn.Rollback();
            //connection.Close();
            //connection.Dispose();

            if (ex.Message.IndexOf("Transaction count") > -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This menu has a same " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " name, please try another name.');", true);
            }
            else if (ex.Message.IndexOf("UQ_SampleTypeDisplayName") > -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('The file has duplicate column name.');", true);
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



