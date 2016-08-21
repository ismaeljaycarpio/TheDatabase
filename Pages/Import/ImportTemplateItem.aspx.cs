using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
//using System.Runtime.InteropServices;
using System.Collections;
using System.Text;
public partial class Pages_Import_ImportTemplateItem : SecurePage
{

    string _strActionMode = "view";
    int? _iImportTemplateID;
    string _qsMode = "";
    string _qsImportTemplateID = "";
    Common_Pager _ImportTemplateItemPager;
    string _strFilesPhisicalPath = "";
    string _strFilesLocation = "";


    //// This is the file header for a DBF. We do this special layout with everything
    //// packed so we can read straight from disk into the structure to populate it
    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    //private struct DBFHeader
    //{
    //    public byte version;
    //    public byte updateYear;
    //    public byte updateMonth;
    //    public byte updateDay;
    //    public Int32 numRecords;
    //    public Int16 headerLen;
    //    public Int16 recordLen;
    //    public Int16 reserved1;
    //    public byte incompleteTrans;
    //    public byte encryptionFlag;
    //    public Int32 reserved2;
    //    public Int64 reserved3;
    //    public byte MDX;
    //    public byte language;
    //    public Int16 reserved4;
    //}

    //// This is the field descriptor structure. 
    //// There will be one of these for each column in the table.
    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    //private struct FieldDescriptor
    //{
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
    //    public string fieldName;
    //    public char fieldType;
    //    public Int32 address;
    //    public byte fieldLen;
    //    public byte count;
    //    public Int16 reserved1;
    //    public byte workArea;
    //    public Int16 reserved2;
    //    public byte flag;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    //    public byte[] reserved3;
    //    public byte indexFlag;
    //}

    //// Read an entire standard DBF file into a DataTable
    //public static DataTable ReadDBF(string dbfFile)
    //{
    //    long start = DateTime.Now.Ticks;
    //    DataTable dt = new DataTable();
    //    BinaryReader recReader;
    //    string number;
    //    string year;
    //    string month;
    //    string day;
    //    long lDate;
    //    long lTime;
    //    DataRow row;
    //    int fieldIndex;

    //    // If there isn't even a file, just return an empty DataTable
    //    if ((false == File.Exists(dbfFile)))
    //    {
    //        return dt;
    //    }

    //    BinaryReader br = null;
    //    try
    //    {
    //        // Read the header into a buffer
    //        br = new BinaryReader(File.OpenRead(dbfFile));
    //        byte[] buffer = br.ReadBytes(Marshal.SizeOf(typeof(DBFHeader)));

    //        // Marshall the header into a DBFHeader structure
    //        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
    //        DBFHeader header = (DBFHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(DBFHeader));
    //        handle.Free();

    //        // Read in all the field descriptors. Per the spec, 13 (0D) marks the end of the field descriptors
    //ArrayList fields = new ArrayList();
    //        while ((13 != br.PeekChar()))
    //        {
    //            buffer = br.ReadBytes(Marshal.SizeOf(typeof(FieldDescriptor)));
    //            handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
    //            fields.Add((FieldDescriptor)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(FieldDescriptor)));
    //            handle.Free();
    //        }

    //        // Read in the first row of records, we need this to help determine column types below
    //        ((FileStream)br.BaseStream).Seek(header.headerLen + 1, SeekOrigin.Begin);
    //        buffer = br.ReadBytes(header.recordLen);
    //        recReader = new BinaryReader(new MemoryStream(buffer));

    //        // Create the columns in our new DataTable
    //        DataColumn col = null;
    //        foreach (FieldDescriptor field in fields)
    //        {
    //            number = Encoding.ASCII.GetString(recReader.ReadBytes(field.fieldLen));
    //            switch (field.fieldType)
    //            {
    //                case 'N':
    //                    if (number.IndexOf(".") > -1)
    //                    {
    //                        col = new DataColumn(field.fieldName, typeof(decimal));
    //                    }
    //                    else
    //                    {
    //                        col = new DataColumn(field.fieldName, typeof(int));
    //                    }
    //                    break;
    //                case 'C':
    //                    col = new DataColumn(field.fieldName, typeof(string));
    //                    break;
    //                case 'T':
    //                    // You can uncomment this to see the time component in the grid
    //                    //col = new DataColumn(field.fieldName, typeof(string));
    //                    col = new DataColumn(field.fieldName, typeof(DateTime));
    //                    break;
    //                case 'D':
    //                    col = new DataColumn(field.fieldName, typeof(DateTime));
    //                    break;
    //                case 'L':
    //                    col = new DataColumn(field.fieldName, typeof(bool));
    //                    break;
    //                case 'F':
    //                    col = new DataColumn(field.fieldName, typeof(Double));
    //                    break;

    //                //default:
    //                //     col = new DataColumn(field.fieldName, typeof(string));
    //                //    break;
    //            }
    //            if (col!=null)
    //            dt.Columns.Add(col);
    //        }

    //        // Skip past the end of the header. 
    //        ((FileStream)br.BaseStream).Seek(header.headerLen, SeekOrigin.Begin);

    //        // Read in all the records
    //        for (int counter = 0; counter <= header.numRecords - 1; counter++)
    //        {
    //            // First we'll read the entire record into a buffer and then read each field from the buffer
    //            // This helps account for any extra space at the end of each record and probably performs better
    //            buffer = br.ReadBytes(header.recordLen);
    //            recReader = new BinaryReader(new MemoryStream(buffer));

    //            // All dbf field records begin with a deleted flag field. Deleted - 0x2A (asterisk) else 0x20 (space)
    //            if (recReader.ReadChar() == '*')
    //            {
    //                continue;
    //            }

    //            // Loop through each field in a record
    //            fieldIndex = 0;
    //            row = dt.NewRow();
    //            foreach (FieldDescriptor field in fields)
    //            {
    //                switch (field.fieldType)
    //                {
    //                    case 'N':  // Number
    //                        // If you port this to .NET 2.0, use the Decimal.TryParse method
    //                        number = Encoding.ASCII.GetString(recReader.ReadBytes(field.fieldLen));
    //                        if (IsNumber(number))
    //                        {
    //                            if (number.IndexOf(".") > -1)
    //                            {
    //                                row[fieldIndex] = decimal.Parse(number);
    //                            }
    //                            else
    //                            {
    //                                row[fieldIndex] = int.Parse(number);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            row[fieldIndex] = 0;
    //                        }

    //                        break;

    //                    case 'C': // String
    //                        row[fieldIndex] = Encoding.ASCII.GetString(recReader.ReadBytes(field.fieldLen));
    //                        break;

    //                    case 'D': // Date (YYYYMMDD)
    //                        year = Encoding.ASCII.GetString(recReader.ReadBytes(4));
    //                        month = Encoding.ASCII.GetString(recReader.ReadBytes(2));
    //                        day = Encoding.ASCII.GetString(recReader.ReadBytes(2));
    //                        row[fieldIndex] = System.DBNull.Value;
    //                        try
    //                        {
    //                            if (IsNumber(year) && IsNumber(month) && IsNumber(day))
    //                            {
    //                                if ((Int32.Parse(year) > 1900))
    //                                {
    //                                    row[fieldIndex] = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));
    //                                }
    //                            }
    //                        }
    //                        catch
    //                        { }

    //                        break;

    //                    case 'T': // Timestamp, 8 bytes - two integers, first for date, second for time
    //                        // Date is the number of days since 01/01/4713 BC (Julian Days)
    //                        // Time is hours * 3600000L + minutes * 60000L + Seconds * 1000L (Milliseconds since midnight)
    //                        lDate = recReader.ReadInt32();
    //                        lTime = recReader.ReadInt32() * 10000L;
    //                        row[fieldIndex] = JulianToDateTime(lDate).AddTicks(lTime);
    //                        break;

    //                    case 'L': // Boolean (Y/N)
    //                        if ('Y' == recReader.ReadByte())
    //                        {
    //                            row[fieldIndex] = true;
    //                        }
    //                        else
    //                        {
    //                            row[fieldIndex] = false;
    //                        }

    //                        break;

    //                    case 'F':
    //                        number = Encoding.ASCII.GetString(recReader.ReadBytes(field.fieldLen));
    //                        if (IsNumber(number))
    //                        {
    //                            row[fieldIndex] = double.Parse(number);
    //                        }
    //                        else
    //                        {
    //                            row[fieldIndex] = 0.0F;
    //                        }
    //                        break;
    //                }
    //                fieldIndex++;
    //            }

    //            recReader.Close();
    //            dt.Rows.Add(row);
    //        }
    //    }

    //    catch
    //    {
    //        throw;
    //    }
    //    finally
    //    {
    //        if (null != br)
    //        {
    //            br.Close();
    //        }
    //    }

    //    long count = DateTime.Now.Ticks - start;

    //    return dt;
    //}

    ///// <summary>
    ///// Simple function to test is a string can be parsed. There may be a better way, but this works
    ///// If you port this to .NET 2.0, use the new TryParse methods instead of this
    /////   *Thanks to wu.qingman on code project for fixing a bug in this for me
    ///// </summary>
    ///// <param name="number">string to test for parsing</param>
    ///// <returns>true if string can be parsed</returns>
    //public static bool IsNumber(string numberString)
    //{
    //    char[] numbers = numberString.ToCharArray();
    //    int number_count = 0;
    //    int point_count = 0;
    //    int space_count = 0;

    //    foreach (char number in numbers)
    //    {
    //        if ((number >= 48 && number <= 57))
    //        {
    //            number_count += 1;
    //        }
    //        else if (number == 46)
    //        {
    //            point_count += 1;
    //        }
    //        else if (number == 32)
    //        {
    //            space_count += 1;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }

    //    return (number_count > 0 && point_count < 2);
    //}

    ///// <summary>
    ///// Convert a Julian Date to a .NET DateTime structure
    ///// Implemented from pseudo code at http://en.wikipedia.org/wiki/Julian_day
    ///// </summary>
    ///// <param name="lJDN">Julian Date to convert (days since 01/01/4713 BC)</param>
    ///// <returns>DateTime</returns>
    //private static DateTime JulianToDateTime(long lJDN)
    //{
    //    double p = Convert.ToDouble(lJDN);
    //    double s1 = p + 68569;
    //    double n = Math.Floor(4 * s1 / 146097);
    //    double s2 = s1 - Math.Floor((146097 * n + 3) / 4);
    //    double i = Math.Floor(4000 * (s2 + 1) / 1461001);
    //    double s3 = s2 - Math.Floor(1461 * i / 4) + 31;
    //    double q = Math.Floor(80 * s3 / 2447);
    //    double d = s3 - Math.Floor(2447 * q / 80);
    //    double s4 = Math.Floor(q / 11);
    //    double m = q + 2 - 12 * s4;
    //    double j = 100 * (n - 49) + i + s4;
    //    return new DateTime(Convert.ToInt32(j), Convert.ToInt32(m), Convert.ToInt32(d));
    //}

    public DataTable ImportDBF_Odbc(string filePath)
    {
        string ImportDirPath = string.Empty;
        string tableName = string.Empty;

        // This function give the Folder name and table name to use in
        // the connection string and create table statement.
        GetFileNameAndPath(filePath, ref tableName, ref ImportDirPath);

        System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection();
        //conn.ConnectionString = "DRIVER={Microsoft dBase Driver (*.dbf)};Deleted=1";

        //conn.ConnectionString = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;Exclusive=No;Collate=Machine;NULL=NO;DELETED=YES;BACKGROUNDFETCH=NO;SourceDB=" + ImportDirPath + ";";

        conn.ConnectionString = "Driver={Microsoft dBase Driver (*.dbf)};SourceType=DBF;SourceDB=" + ImportDirPath  + "\\;Exclusive=No; Collate=Machine;NULL=NO;DELETED=NO;BACKGROUNDFETCH=NO;";
        
        DataTable dt = new DataTable();
        try
        {



            conn.Open();
            System.Data.Odbc.OdbcCommand comm = new System.Data.Odbc.OdbcCommand();
            comm.CommandText = "SELECT * FROM " + ImportDirPath + "\\dbase_03.dbf"; //tableName

            comm.Connection = conn;

            dt.Load(comm.ExecuteReader());
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.ToString());
        }
        finally
        {
            conn.Close();
        }
        return dt;
    }
    protected void PopulateTerminology()
    {
        stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));

    }

     protected void btnHiddenRefreshHeader_Click(object sender, EventArgs e)
    {
        btnFileUploaded_Click(null, null);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;


        string strJSForSortingImportTemplateItem = @"

                         $(document).ready(function () {


                                    $(function () {
                                        $('#divImportTemplateItemSingleInstance').sortable({
                                            items: '.gridview_row',
                                            cursor: 'crosshair',
                                            helper: fixHelper,
                                            cursorAt: { left: 10, top: 10 },
                                            connectWith: '#divImportTemplateItemSingleInstance',
                                            handle: '.sortHandleVT',
                                            axis: 'y',
                                            distance: 15,
                                            dropOnEmpty: true,
                                            receive: function (e, ui) {
                                                $(this).find('tbody').append(ui.item);

                                            },
                                            start: function (e, ui) {
                                                ui.placeholder.css('border-top', '2px solid #00FFFF');
                                                ui.placeholder.css('border-bottom', '2px solid #00FFFF');

                                            },
                                            update: function (event, ui) {
                                                var TC = '';
                                                $('.ImportTemplateItemID').each(function () {
                                                    TC = TC + this.value.toString() + ',';
                                                });
                                                //alert(TC);
                                                document.getElementById('hfImportTemplateItemIDForColumnIndex').value = TC;
                        
                                                $('#btnImportTemplateItemIDForColumnIndex').trigger('click');

                                            }
                                        });
                                    });

                                });

                        ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "strJSForSortingImportTemplateItem", strJSForSortingImportTemplateItem, true);



        string strImportTemplateItemPop = @"
                    $(function () {
                            $('.popuplinkVT').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 800,
                                height: 500,
                                titleShow: false
                            });
                        });

                ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "strImportTemplateItemPop", strImportTemplateItemPop, true);


        _strFilesLocation = Session["FilesLocation"].ToString();
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();


        txtImportColumnHeaderRow.Attributes["onBlur"] = "RefreshImportHeader()";
        if (!IsPostBack)
        {
            PopulateTableDDL();
            //PopulateLocationDDL();
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            if (Request.QueryString["SearchCriteriaIT"] != null)
            {

                PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteriaIT"].ToString())));

                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaIT=" + Request.QueryString["SearchCriteriaIT"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString();
            }
            else
            {

                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaIT=" + Cryptography.Encrypt("-1") + "&TableID=" + Request.QueryString["TableID"].ToString(), false);//i think no need
            }

            if(Request.QueryString["fixedbackurl"]!=null)
            {
                hlBack.NavigateUrl = Cryptography.Decrypt(Request.QueryString["fixedbackurl"].ToString());
            }
        }
        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;


                if (Request.QueryString["ImportTemplateID"] != null)
                {

                    _qsImportTemplateID = Cryptography.Decrypt(Request.QueryString["ImportTemplateID"]);

                    _iImportTemplateID = int.Parse(_qsImportTemplateID);

                    if (!IsPostBack)
                    {
                        PopulateImportTemplateItem((int)_iImportTemplateID);

                        if (Request.QueryString["popupitem"] != null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopItem", "  setTimeout(function () { OpenImportTemplateItem(); }, 1000); ", true);

                        }
                    }
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }



        GridViewRow ImportTemplateItemPager = grdImportTemplateItem.TopPagerRow;
        if (ImportTemplateItemPager != null)
            _ImportTemplateItemPager = (Common_Pager)ImportTemplateItemPager.FindControl("ImportTemplateItemPager");



        // checking permission


        switch (_strActionMode.ToLower())
        {
            case "add":
                lblTitle.Text = "Add Import Template";
                

                break;

            case "view":

                lblTitle.Text = "View Import Template";



                if (!IsPostBack)
                 PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                lblTitle.Text = "Edit Import Template";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }

        Title = lblTitle.Text;

        if (!IsPostBack)
        {
            PopulateTerminology();
        }


    }

    protected void PopulateSearchCriteria(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                ddlTable.Text = xmlDoc.FirstChild[ddlTable.ID].InnerText;
               
            }
            else
            {
                ddlTable.Text = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());

            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }



    protected void ImportTemplateItemPager_DeleteAction(object sender, EventArgs e)
    {

   
        string sCheck = "";
     
        for (int i = 0; i < grdImportTemplateItem.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)grdImportTemplateItem.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)grdImportTemplateItem.Rows[i].FindControl("LblID")).Text + ",";
            }
        }

        



        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message_alert", "alert('Please select a record.');", true);

            return;
        }

        sCheck = sCheck + "-1";

        Common.ExecuteText("DELETE ImportTemplateItem WHERE ImportTemplateItemID IN (" + sCheck + ") ");

        PopulateImportTemplateItem((int)_iImportTemplateID);



    }


    protected void btnImportTemplateItemIDForColumnIndex_Click(object sender, EventArgs e)
    {
        //
        if (hfImportTemplateItemIDForColumnIndex.Value != "")
        {

            try
            {
                string strNewVIT = hfImportTemplateItemIDForColumnIndex.Value.Substring(0, hfImportTemplateItemIDForColumnIndex.Value.Length - 1);
                string[] newVT = strNewVIT.Split(',');

                //string strFilter = "";



                DataTable dtDO = Common.DataTableFromText("SELECT ColumnIndex,ImportTemplateItemID FROM [ImportTemplateItem] WHERE ImportTemplateItemID IN (" + strNewVIT + ") ORDER BY ColumnIndex");
                if (newVT.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newVT.Length; i++)
                    {
                        Common.ExecuteText("UPDATE ImportTemplateItem SET ColumnIndex =" + i.ToString() + " WHERE ImportTemplateItemID=" + newVT[i]);

                    }
                }


            }
            catch (Exception ex)
            {

                //

            }
            PopulateImportTemplateItem((int)_iImportTemplateID);
        }
    }


    public string GetAddImportTemplateItemURL(int iImportTemplateID)
    {
        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Import/ImportTemplateItemDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&ImportTemplateID=" + Cryptography.Encrypt(iImportTemplateID.ToString());
    }


    protected void PopulateImportTemplateItem(int iImportTemplateID)
    {
        int iTN = 0;

        hlAddImportTemplateItem.NavigateUrl = GetAddImportTemplateItemURL(iImportTemplateID);
        hlAddImportTemplateItem2.NavigateUrl = hlAddImportTemplateItem.NavigateUrl;
        DataTable dtImportTemplateItems = Common.DataTableFromText("SELECT * FROM ImportTemplateItem WHERE ImportTemplateID=" + iImportTemplateID.ToString() + " ORDER BY ColumnIndex");

        grdImportTemplateItem.DataSource = dtImportTemplateItems;
        iTN = dtImportTemplateItems.Rows.Count;

        grdImportTemplateItem.VirtualItemCount = iTN;
        grdImportTemplateItem.DataBind();

        if (grdImportTemplateItem.TopPagerRow != null)
            grdImportTemplateItem.TopPagerRow.Visible = true;

        GridViewRow gvr = grdImportTemplateItem.TopPagerRow;



        if (gvr != null)
        {
            _ImportTemplateItemPager = (Common_Pager)gvr.FindControl("ImportTemplateItemPager");
            _ImportTemplateItemPager.AddURL = GetAddImportTemplateItemURL(iImportTemplateID);
            _ImportTemplateItemPager.HyperAdd_CSS = "popuplinkVT";
            _ImportTemplateItemPager.AddToolTip = "Add/Remove";
        }

        if (iTN == 0)
        {
            divEmptyAddImportTemplateItem.Visible = true;
        }
        else
        {
            divEmptyAddImportTemplateItem.Visible = false;
        }

    }

    protected void grdImportTemplateItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }


    protected void grdImportTemplateItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        //if (e.Row.RowType == DataControlRowType.Header)
        //{

        //    HyperLink hlAddDetail = e.Row.FindControl("hlAddDetail") as HyperLink;
        //    hlAddDetail.NavigateUrl = GetAddImportTemplateItemURL( int.Parse( hfCurrentImportTemplateID.Value));

        //}

        if (e.Row.RowType == DataControlRowType.DataRow)
        {



            Column theColumn = RecordManager.ets_Column_Details(int.Parse(DataBinder.Eval(e.Row.DataItem, "ColumnID").ToString()));
            if (theColumn != null)
            {
                Label lblFieldName = e.Row.FindControl("lblFieldName") as Label;
                if (lblFieldName != null)
                {
                    lblFieldName.Text = theColumn.DisplayName;

                }
                DropDownList ddlParentImportColumnID = e.Row.FindControl("ddlParentImportColumnID") as DropDownList;
                if(theColumn.TableTableID!=null && theColumn.LinkedParentColumnID!=null && theColumn.DisplayColumn!="")
                {
                   
                    if(ddlParentImportColumnID!=null)
                    {
                        PopulateColumns(ref ddlParentImportColumnID, (int)theColumn.TableTableID);
                        if (DataBinder.Eval(e.Row.DataItem, "ParentImportColumnID") != DBNull.Value 
                            && DataBinder.Eval(e.Row.DataItem, "ParentImportColumnID").ToString()!="")
                        {
                            string strParentImportColumnID=DataBinder.Eval(e.Row.DataItem, "ParentImportColumnID").ToString();
                            if (ddlParentImportColumnID.Items.FindByValue(strParentImportColumnID) != null)
                                ddlParentImportColumnID.SelectedValue = strParentImportColumnID;
                        }
                    }
                }
                else
                {
                     if(ddlParentImportColumnID!=null)
                     {
                         ddlParentImportColumnID.Visible = false;
                     }

                }
            }


            TextBox txtHeading = e.Row.FindControl("txtHeading") as TextBox;
            if (txtHeading != null)
            {
                txtHeading.Text = DataBinder.Eval(e.Row.DataItem, "ImportHeaderName").ToString();

            }

           


        }

    }

    protected void PopulateColumns(ref DropDownList ddlC, int iTableID)
    {
        ddlC.Items.Clear();
        DataTable dtTemp = Common.DataTableFromText(@"SELECT ColumnID, DisplayName FROM [Column] 
                WHERE  IsStandard=0 AND TableID=" + iTableID.ToString() + " ORDER BY DisplayName");
        ddlC.DataSource = dtTemp;
        ddlC.DataBind();
        ListItem li = new ListItem("", "");
        ddlC.Items.Insert(0, li);

    }

    protected void btnRefreshImportTemplateItem_Click(object sender, EventArgs e)
    {
        if(_iImportTemplateID!=null)
            PopulateImportTemplateItem((int)_iImportTemplateID);
        
    }


    protected void ImportTemplateItemPager_BindTheGridAgain(object sender, EventArgs e)
    {
        PopulateImportTemplateItem((int)_iImportTemplateID);
    }




    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());

        ddlTable.DataBind();
        //if (iTN == 0)
        //{
        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddlTable.Items.Insert(0, liAll);
        //}


    }


    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        //PopulateLocationDDL();

    }

  

     
    protected void PopulateTheRecord()
    {
        try
        {

            ImportTemplate theImportTemplate = ImportManager.dbg_ImportTemplate_Detail((int)_iImportTemplateID);
            lnkRefesh.Visible = true;
            ddlTable.SelectedValue = theImportTemplate.TableID.ToString();

           
            txtImportTemplateName.Text = theImportTemplate.ImportTemplateName;
            txtHelpText.Text = theImportTemplate.HelpText;
            if(theImportTemplate.FileFormat!="")
                radioFileFormat.SelectedValue = theImportTemplate.FileFormat;
            txtSPName.Text = theImportTemplate.SPName;
            txtNotes.Text = theImportTemplate.Notes;

            
            if (theImportTemplate.ImportColumnHeaderRow != null)
            {
                txtImportColumnHeaderRow.Text = theImportTemplate.ImportColumnHeaderRow.ToString();
            }
            if (theImportTemplate.ImportDataStartRow != null)
            {
                txtImportDataStartRow.Text = theImportTemplate.ImportDataStartRow.ToString();
            }

            if (_strActionMode == "edit")
            {
                ViewState["theImportTemplate"] = theImportTemplate;

                if(theImportTemplate.TemplateUniqueFileName!="")
                {
                    hfImportTemplateFileName.Value = theImportTemplate.TemplateUniqueFileName;

                    string strFilePath = Cryptography.Encrypt(_strFilesLocation + "/UserFiles/AppFiles/" + theImportTemplate.TemplateUniqueFileName);
                    string strFileName = theImportTemplate.TemplateUniqueFileName.Substring(37);

                    lblImportTemplateFile.Text = "<a target='_blank' href='" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Security/Filedownload.aspx?FilePath="
                           + strFilePath + "&FileName=" + Cryptography.Encrypt(strFileName) + "'>" +
                             strFileName + "</a>";

                    lblImportTemplateFile.Text = "<img  title=\"Remove this file\" style=\"cursor:pointer;\"  id=\"dimg" + hfImportTemplateFileName.ID + "\" src=\"" + Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                               + "/App_Themes/Default/Images/icon_delete.gif\" />" + lblImportTemplateFile.Text;

                    string strTempJS = @"  document.getElementById('dimg" + hfImportTemplateFileName.ID + @"').addEventListener('click', function (e) {
                                                     document.getElementById('" + hfImportTemplateFileName.ID + @"').value='';
                                                      $('#" + lblImportTemplateFile.ID + @"').html(''); 
                                            });";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "filedelete" , strTempJS, true);
                }

            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Import/ImportTemplateItem.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString()
                    + "&SearchCriteriaIT=" + Request.QueryString["SearchCriteriaIT"].ToString() + "&ImportTemplateID=" + Cryptography.Encrypt(theImportTemplate.ImportTemplateID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "ImportTemplate Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtImportTemplateName.Enabled = p_bEnable;
       
        ddlTable.Enabled = p_bEnable;
        //ddlLocation.Enabled = p_bEnable;
            

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }

    protected void lnkRefesh_Click(object sender, EventArgs e)
    {
        try
        {
            if(_iImportTemplateID!=null)
            {

                ImportManager.spRefreshImportTemplateFields((int)_iImportTemplateID);
                PopulateImportTemplateItem((int)_iImportTemplateID);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ResetImportTemplateOrder", "alert('Done :-)');", true);

            }

        }
        catch
        {

        }
    }


    //protected void cmdSave_Click(object sender, ImageClickEventArgs e) btnFileUploaded_Click
    private  void GetFileNameAndPath(string completePath, ref string fileName, ref string folderPath)
    {
        string[] fileSep = completePath.Split('\\');
        for (int iCount = 0; iCount < fileSep.Length; iCount++)
        {
            if (iCount == fileSep.Length - 2)
            {
                if (fileSep.Length == 2)
                {
                    folderPath += fileSep[iCount] + "\\";
                }
                else
                {
                    folderPath += fileSep[iCount];
                }
            }
            else
            {
                if (fileSep[iCount].IndexOf(".") > 0)
                {
                    fileName = fileSep[iCount];
                    fileName = fileName.Substring(0, fileName.IndexOf("."));
                }
                else
                {
                    folderPath += fileSep[iCount] + "\\";
                }
            }
        }
    }

    public  DataTable ImportDBF(string filePath)
    {
        try
        {
            string ImportDirPath = string.Empty;
            string tableName = string.Empty;
            // This function give the Folder name and table name to use in
            // the connection string and create table statement.
            GetFileNameAndPath(filePath, ref tableName, ref ImportDirPath);
            //tableName = "[" + tableName + "]";
            DataSet dsImport = new DataSet();
            //string thousandSep = thousandSeparator;
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + ImportDirPath + ";Extended Properties=dBASE IV; "; //Extended Properties=dBASE III;

            //string connString = "Provider=vfpoledb;Data Source=" + ImportDirPath + ";Collating Sequence=machine;"; Collating Sequence=machine;

            //string connString = @"Provider=vfpoledb;Data Source=E:\C\DBG_Code\Website\branches\FileSite\UserFiles\AppFiles;";

            OleDbConnection conn = new OleDbConnection(connString);
            DataSet dsGetData = new DataSet();
            //OleDbDataAdapter daGetTableData = new OleDbDataAdapter("Select * from " +Path.GetFileNameWithoutExtension( filePath), conn); //+ tableName


            OleDbDataAdapter daGetTableData = new OleDbDataAdapter("Select * from dbase_30WW.DBF", conn); //+ tableName

            // fill all the data in to dataset
            daGetTableData.Fill(dsGetData);
            DataTable dt = new DataTable(dsGetData.Tables[0].TableName.ToString());
            dsImport.Tables.Add(dt);
            // here I am copying get Dataset into another dataset because //before return the dataset I want to format the data like change //"datesymbol","thousand symbol" and date format as did while
            // exporting. If you do not want to format the data then you can // directly return the dsGetData
            for (int row = 0; row < dsGetData.Tables[0].Rows.Count; row++)
            {
                DataRow dr = dsImport.Tables[0].NewRow();
                dsImport.Tables[0].Rows.Add(dr);
                for (int col = 0; col < dsGetData.Tables[0].Columns.Count; col++)
                {
                    if (row == 0)
                    {
                        DataColumn dc = new DataColumn(dsGetData.Tables[0].Columns[col].ColumnName.ToString());
                        dsImport.Tables[0].Columns.Add(dc);
                    }
                    if (!String.IsNullOrEmpty(dsGetData.Tables[0].Rows[row][col].
                    ToString()))
                    {
                        dsImport.Tables[0].Rows[row][col] = Convert.ToString(dsGetData.Tables[0].Rows[row][col].ToString().Trim());
                    }
                } // close inner for loop
            }// close ouer for loop

            return dsImport.Tables[0];

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "DBF Read", ex.Message, ex.StackTrace, DateTime.Now,Request.RawUrl);
            SystemData.ErrorLog_Insert(theErrorLog);

            return null;
        }

    } // close function

    protected void btnFileUploaded_Click(object sender, EventArgs e)
    {
        hfHasFileColumn.Value = "";
        lblMsg.Text = "";
        Guid guidNew = Guid.NewGuid();
        string strFileUniqueName = "";

        //if (_strActionMode == "add")
        //{
        //    hfImportTemplateFileName.Value = "";
        //}

        if (fuImportUploadTemplate.HasFile || hfImportTemplateFileName.Value != "")
        {
            try
            {
                string strFileExtension = "";

                bool bNewFile = false;
                if (fuImportUploadTemplate.HasFile)
                    bNewFile = true;

                if (bNewFile)
                {
                    strFileExtension = fuImportUploadTemplate.FileName.Substring(fuImportUploadTemplate.FileName.LastIndexOf('.') + 1).ToLower();

                    strFileUniqueName = guidNew.ToString() + "_" + fuImportUploadTemplate.FileName;

                    hfImportTemplateFileName.Value = strFileUniqueName;
                }
                else
                {
                    strFileExtension = hfImportTemplateFileName.Value.Substring(hfImportTemplateFileName.Value.LastIndexOf('.') + 1).ToLower();
                    strFileUniqueName = hfImportTemplateFileName.Value;
                }
                
             
                string strFileFullPath = _strFilesPhisicalPath + "\\UserFiles\\AppFiles" + "\\" + strFileUniqueName;

                if (bNewFile)
                    fuImportUploadTemplate.SaveAs(strFileFullPath);

                radioFileFormat.SelectedValue = "csv_excel";
                if(strFileExtension.ToLower()=="csv" || strFileExtension.ToLower()=="xls" || strFileExtension.ToLower()=="xlsx"
                    || strFileExtension.ToLower() == "dbf" || strFileExtension.ToLower() == "txt")
                {
                    try
                    {
                        switch (strFileExtension)
                        {
                            case "csv":
                                strFileExtension = ".csv";
                                break;
                            case "xls":
                                strFileExtension = ".xls";
                                break;
                            case "xlsx":
                                strFileExtension = ".xlsx";
                                break;
                            case "dbf":
                                strFileExtension = ".dbf";
                                radioFileFormat.SelectedValue = "dbf";
                                break;
                            case "txt":
                                strFileExtension = ".txt";
                                radioFileFormat.SelectedValue = "text";
                                break;
                        }
                        string strSelectedSheet = "";
                        if (strFileExtension == ".xls" || strFileExtension == ".xlsx")
                        {
                            List<string> lstSheets = OfficeManager.GetExcelSheetNames(_strFilesPhisicalPath + "\\UserFiles\\AppFiles", strFileUniqueName);
                            if (lstSheets.Count>1)
                                strSelectedSheet = lstSheets[0];

                        }


                        string strImportFolder = _strFilesPhisicalPath + "\\UserFiles\\AppFiles";
                        DataTable dtImportFileTable;


                        dtImportFileTable = null;

                        string strMsg = "";
                        switch (strFileExtension.ToLower())
                        {
                            case ".xls":
                                dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
                                break;
                            case ".xlsx":
                                dtImportFileTable = OfficeManager.GetImportFileTableFromXLSX(strImportFolder, strFileUniqueName, strSelectedSheet, false);
                                break;
                            case ".csv":
                                dtImportFileTable = UploadManager.GetImportFileTableFromCSV(strImportFolder, strFileUniqueName, ref strMsg);
                                //iRowIndex = 1;
                                break;
                            case ".dbf":
                                //dtImportFileTable = ImportDBF(strImportFolder + "\\" + strFileUniqueName);
                                dtImportFileTable = UploadManager.GetImportFileTableFromDBF(strImportFolder, strFileUniqueName, ref strMsg);

                                break;
                            case ".txt":
                                dtImportFileTable = UploadManager.GetImportFileTableFromText(strImportFolder, strFileUniqueName, ref strMsg);
                                break;
                        }

                        if (dtImportFileTable != null )
                        {
                            if(strFileExtension.ToLower() != ".dbf")
                            {
                                int iImportColumnHeaderRow = 1;
                                int iImportDataStartRow = 2;

                                if (txtImportColumnHeaderRow.Text.Trim() != "")
                                    iImportColumnHeaderRow = int.Parse(txtImportColumnHeaderRow.Text);

                                if (txtImportDataStartRow.Text.Trim() != "")
                                    iImportDataStartRow = int.Parse(txtImportDataStartRow.Text);

                                if (dtImportFileTable.Rows.Count >= iImportColumnHeaderRow)
                                {
                                    for (int i = 0; i <= dtImportFileTable.Columns.Count - 1; i++)
                                    {
                                        if (dtImportFileTable.Rows[iImportColumnHeaderRow - 1][i].ToString() == "")
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
                                                if (dtImportFileTable.Rows[iImportColumnHeaderRow - 1][i].ToString() != "")
                                                    dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[iImportColumnHeaderRow - 1][i].ToString();
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
                                                            if (dc.ColumnName == dtImportFileTable.Rows[iImportColumnHeaderRow - 1][i].ToString() + j.ToString())
                                                            {
                                                                bOK = false;
                                                            }
                                                        }

                                                        if (bOK)
                                                        {
                                                            dtImportFileTable.Columns[i].ColumnName = dtImportFileTable.Rows[iImportColumnHeaderRow - 1][i].ToString() + j.ToString();
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


                                if (iImportDataStartRow > 1)
                                {
                                    for (int i = 1; i <= (int)iImportDataStartRow - 1; i++)
                                    {
                                        dtImportFileTable.Rows.RemoveAt(0);

                                    }
                                    dtImportFileTable.AcceptChanges();

                                }
                                

                                int xy = 0;
                                foreach (DataColumn dc in dtImportFileTable.Columns)
                                {
                                    dtImportFileTable.Columns[xy].ColumnName = dc.ColumnName.Replace(",", ".").Trim(); //Common.RemoveSpecialCharacters(dc.ColumnName);
                                    xy = xy + 1;
                                }
                                dtImportFileTable.AcceptChanges();
                            }
                            

                          List<string> lstFileColumns=  Common.GetColumnStringListFromTable(dtImportFileTable);
                          fclOne.TableID =int.Parse(ddlTable.SelectedValue) ;
                          fclOne.lstFileColumn = lstFileColumns;
                          fclOne.PopulateGrid();
                          trFileColumn.Visible = true;
                          hfHasFileColumn.Value = "yes";
                        }


                    }
                    catch(Exception ex2)
                    {
                        lblMsg.Text = "File column link ERROR: " + ex2.Message.ToString();
                        hfHasFileColumn.Value = "";
                    }
                }
                else
                {
                    lblMsg.Text = "Please select a valid file";
                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = lblMsg.Text  + "ERROR: " + ex.Message.ToString();
                hfHasFileColumn.Value = "";
                return;
            }
        }

         //if ((fuImportUploadTemplate.HasFile || hfImportTemplateFileName.Value != "") && radioFileFormat.SelectedValue == "text")
         //{
         //    string strFileExtension = "";

         //    bool bNewFile = false;
         //    if (fuImportUploadTemplate.HasFile)
         //        bNewFile = true;

         //    if (bNewFile)
         //    {
         //        strFileExtension = fuImportUploadTemplate.FileName.Substring(fuImportUploadTemplate.FileName.LastIndexOf('.') + 1).ToLower();

         //        strFileUniqueName = guidNew.ToString() + "_" + fuImportUploadTemplate.FileName;

         //        hfImportTemplateFileName.Value = strFileUniqueName;
         //    }
         //    else
         //    {
         //        strFileExtension = hfImportTemplateFileName.Value.Substring(hfImportTemplateFileName.Value.LastIndexOf('.') + 1).ToLower();
         //        strFileUniqueName = hfImportTemplateFileName.Value;
         //    }


         //    string strFileFullPath = _strFilesPhisicalPath + "\\UserFiles\\AppFiles" + "\\" + strFileUniqueName;

         //    if (bNewFile)
         //        fuImportUploadTemplate.SaveAs(strFileFullPath);

         //     if(strFileExtension.ToLower()=="txt")
         //     {
         //         strFileExtension = ".txt";
         //     }
         //     else
         //     {
         //         lblMsg.Text = "Please select a Text file.";
         //         return;
         //     }

         //     trFileColumn.Visible = false;
         //     hfHasFileColumn.Value = "";
         //}

    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        

        string strEditURL = hlBack.NavigateUrl;
        try
        {
            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        
                        ImportTemplate newImportTemplate = new ImportTemplate(null, int.Parse(ddlTable.SelectedValue),
                            txtImportTemplateName.Text, txtHelpText.Text, hfImportTemplateFileName.Value, radioFileFormat.SelectedValue, txtSPName.Text, txtNotes.Text);


                        newImportTemplate.ImportColumnHeaderRow = txtImportColumnHeaderRow.Text.Trim() == "" ? null : (int?)int.Parse(txtImportColumnHeaderRow.Text);
                        newImportTemplate.ImportDataStartRow = txtImportDataStartRow.Text.Trim() == "" ? null : (int?)int.Parse(txtImportDataStartRow.Text);


                        int iNewImportTemplateID= ImportManager.dbg_ImportTemplate_Insert(newImportTemplate);


                        if(hfHasFileColumn.Value!="")
                        {
                            try
                            {
                                DataTable dtLinkedColumn = fclOne.GetLinkedColumn;
                                if(dtLinkedColumn!=null)
                                {
                                    int i=0;
                                    foreach(DataRow dr in dtLinkedColumn.Rows)
                                    {
                                        ImportTemplateItem theImportTemplateItem = new ImportTemplateItem(null, iNewImportTemplateID,
                                            int.Parse(dr["ColumnID"].ToString()), dr["FileColumn"].ToString(), i);

                                        try
                                        {
                                            ImportManager.dbg_ImportTemplateItem_Insert(theImportTemplateItem);
                                        }
                                        catch
                                        {
                                            //avoid duplicate

                                        }                                    
                                    }
                                }
                            }
                            catch
                            {
                                //
                            }
                        }

                       string strExtarQueryString = "";
                       if (Request.QueryString["SearchCriteriaIT"] != null)
                           strExtarQueryString = "&SearchCriteriaIT=" + Request.QueryString["SearchCriteriaIT"].ToString();

                       if (Request.QueryString["fixedbackurl"] != null)
                           strExtarQueryString =strExtarQueryString+ "&fixedbackurl=" + Request.QueryString["fixedbackurl"].ToString();

                       strEditURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Import/ImportTemplateItem.aspx?popupitem=yes&mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&ImportTemplateID=" + Cryptography.Encrypt(iNewImportTemplateID.ToString()) + strExtarQueryString;



                        break;

                    case "view":


                        break;

                    case "edit":
                        
                        ImportTemplate editImportTemplate = (ImportTemplate)ViewState["theImportTemplate"];
                       
                        editImportTemplate.ImportTemplateName = txtImportTemplateName.Text; 
                        editImportTemplate.TableID = int.Parse(ddlTable.SelectedValue);

                        editImportTemplate.TemplateUniqueFileName = hfImportTemplateFileName.Value;
                        
                        editImportTemplate.HelpText = txtHelpText.Text;
                        editImportTemplate.FileFormat = radioFileFormat.SelectedValue;
                        editImportTemplate.Notes = txtNotes.Text;
                        editImportTemplate.SPName = txtSPName.Text;

                        editImportTemplate.ImportColumnHeaderRow = txtImportColumnHeaderRow.Text.Trim() == "" ? null : (int?)int.Parse(txtImportColumnHeaderRow.Text);
                        editImportTemplate.ImportDataStartRow = txtImportDataStartRow.Text.Trim() == "" ? null : (int?)int.Parse(txtImportDataStartRow.Text);


                        ImportManager.dbg_ImportTemplate_Update(editImportTemplate);
                        
                        //now lets update Items

                        if (hfHasFileColumn.Value != "")
                        {
                            try
                            {
                                Common.ExecuteText("DELETE ImportTemplateItem WHERE ImportTemplateID=" + editImportTemplate.ImportTemplateID.ToString());
                                DataTable dtLinkedColumn = fclOne.GetLinkedColumn;
                                if (dtLinkedColumn != null)
                                {
                                    int i = 0;
                                    foreach (DataRow dr in dtLinkedColumn.Rows)
                                    {
                                        ImportTemplateItem theImportTemplateItem = new ImportTemplateItem(null, editImportTemplate.ImportTemplateID,
                                            int.Parse(dr["ColumnID"].ToString()), dr["FileColumn"].ToString(), i);

                                        try
                                        {
                                            ImportManager.dbg_ImportTemplateItem_Insert(theImportTemplateItem);
                                        }
                                        catch
                                        {
                                            //avoid duplicate

                                        }

                                    }
                                }
                            }
                            catch
                            {
                                //
                            }
                        }
                        else
                        {
                            for (int i = 0; i < grdImportTemplateItem.Rows.Count; i++)
                            {
                                string strImportTemplateItemID = ((Label)grdImportTemplateItem.Rows[i].FindControl("LblID")).Text;

                                ImportTemplateItem theImportTemplateItem = ImportManager.dbg_ImportTemplateItem_Detail(int.Parse(strImportTemplateItemID));

                                if (theImportTemplateItem != null)
                                {
                                    theImportTemplateItem.ColumnIndex = i;
                                    theImportTemplateItem.ImportHeaderName = ((TextBox)grdImportTemplateItem.Rows[i].FindControl("txtHeading")).Text;

                                    Column theColumn = RecordManager.ets_Column_Details((int)theImportTemplateItem.ColumnID);
                                    if (theColumn.TableTableID != null && theColumn.LinkedParentColumnID != null && theColumn.DisplayColumn != "")
                                    {
                                        DropDownList ddlParentImportColumnID = grdImportTemplateItem.Rows[i].FindControl("ddlParentImportColumnID") as DropDownList;
                                        if(ddlParentImportColumnID!=null)
                                        {
                                            if (ddlParentImportColumnID.SelectedValue != "")
                                            {
                                                theImportTemplateItem.ParentImportColumnID = int.Parse(ddlParentImportColumnID.SelectedValue);
                                            }
                                            else
                                            {
                                                theImportTemplateItem.ParentImportColumnID = null;
                                            }
                                                
                                        }
                                    }
                                    else
                                    {
                                        theImportTemplateItem.ParentImportColumnID = null;
                                    }
                                    ImportManager.dbg_ImportTemplateItem_Update(theImportTemplateItem);
                                }
                            }

                        }

                        



                        break;

                    default:
                        //?
                        break;
                }
            }
            else
            {
                //user input is not ok

            }
            Response.Redirect(strEditURL, false);

        }
        catch (Exception ex)
        {

            ErrorLog theErrorLog = new ErrorLog(null, "ImportTemplate Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
            
        }

     

    }
   
}
