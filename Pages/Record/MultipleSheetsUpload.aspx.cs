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
public partial class Pages_Record_MultipleSheetsUpload : SecurePage
{
    Table _theTable;
    Menu _qsMenu;
    User _ObjUser;
    string _qsTableID = "";
    int _iTotalDynamicColumns = 0;
    int _iValidRecordIDIndex = 0;
   
    string _strFilesPhisicalPath = "";
    protected void Page_Load(object sender, EventArgs e)
    {

      
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();


        if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2,3,4,5,7,8,9"))
        { Response.Redirect("~/Default.aspx", false); }

        try
        {
            _ObjUser = (User)Session["User"];

            if (Request.QueryString["TableID"] != null)
            {
                _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"]);
                _theTable = RecordManager.ets_Table_Details(int.Parse(_qsTableID));
                //_qsMenu = RecordManager.ets_Menu_Details((int)_theTable.MenuID);

               
            }
            else
            {
                Response.Redirect("~/Default.aspx", false);
            }

            int iTemp = 0;
           

            if(!IsPostBack)
            {

                if (_theTable.DataUpdateUniqueColumnID != null)
                {
                    trDataUpdateUniqueColumnID.Visible = true;
                    Column theDataUpdateUniqueColumn = RecordManager.ets_Column_Details((int)_theTable.DataUpdateUniqueColumnID);
                    chkDataUpdateUniqueColumnID.Text = "Update existing data, matching on [" + theDataUpdateUniqueColumn.DisplayName + "]";
                }


                if (Request.QueryString["FileInfo"] != null)
                {
                    //PopulateLocationDDL();
                    PopulateFileInfo(int.Parse(Cryptography.Decrypt(Request.QueryString["FileInfo"].ToString())));
                    //populate sheet drop down
                    Guid guidNew = Guid.Empty;
                    string strFileExtension = "";
                    strFileExtension = hfFileExtension.Value;

                    guidNew = Guid.Parse(hfguidNew.Value);

                    string strFileUniqueName;
                    strFileUniqueName = guidNew.ToString() + strFileExtension;

                    //List<string> lstSheets = OfficeManager.GetExcelSheetNames(Server.MapPath("../../UserFiles/AppFiles"), strFileUniqueName);

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

                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordUpload.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString(); ;
            }
            
            Title = "Multiple Sheets file upload - " + _theTable.TableName;
            lblTitle.Text = "Multiple Sheets file upload - " + _theTable.TableName; 
                      
       
        }
        catch (Exception ex)
        {

        }

    }


//    protected void CheckLocationColumn()
//    {
//        if ((bool)_theTable.IsImportPositional)
//        {
//            DataTable dtTemp = Common.DataTableFromText(@"Select Columnid from [Column] WHERE   
//                                TableID=" + _theTable.TableID.ToString() + " AND SystemName='LocationID' AND PositionOnImport is not null");

//            if (dtTemp.Rows.Count > 0)
//            {
//                trLocation.Visible = false;
//            }
//            else
//            {
//                trLocation.Visible = true;
//            }
//        }
//        else
//        {
//            DataTable dtTemp = Common.DataTableFromText(@"Select Columnid from [Column] WHERE   
//                                TableID=" + _theTable.TableID.ToString() + " AND SystemName='LocationID' AND NameOnImport is not null");

//            if (dtTemp.Rows.Count > 0)
//            {
//                trLocation.Visible = false;
//            }
//            else
//            {
//                trLocation.Visible = true;
//            }
//        }
//    }

    protected void PopulateFileInfo(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                string strImortTemplateID = xmlDoc.FirstChild["ddlTemplate"].InnerText;
                if (strImortTemplateID!="")
                {
                    hfImportTemlateID.Value = strImortTemplateID;
                    ImportTemplate theImportTemplate = ImportManager.dbg_ImportTemplate_Detail(int.Parse(hfImportTemlateID.Value));
                    if(theImportTemplate!=null)
                    {
                        lblTemlate.Text = theImportTemplate.ImportTemplateName;
                    }

                }
                else
                {
                    lblTemlate.Text = "No Template";
                }
                lblExcelFileName.Text = xmlDoc.FirstChild["FileName"].InnerText;
                lblBatchDesc.Text = xmlDoc.FirstChild["txtBatchDescription"].InnerText;
                hfguidNew.Value = xmlDoc.FirstChild["guidNew"].InnerText;
                if (xmlDoc.FirstChild["chkDataUpdateUniqueColumnID"]!=null)
                    chkDataUpdateUniqueColumnID.Checked = bool.Parse( xmlDoc.FirstChild["chkDataUpdateUniqueColumnID"].InnerText);

                hfFileExtension.Value ="." + lblExcelFileName.Text.Substring(lblExcelFileName.Text.LastIndexOf('.') + 1).ToLower();
                if (lblBatchDesc.Text == "")
                    lblBatchDesc.Text = lblExcelFileName.Text;

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

        //if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        //{
        //    lblMsg.Text = "Global user can not import!";
        //    return;

        //}


        //Session["CurrentBatch"] = txtBatchDescription.Text;
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
        string strMsg = "";

        //SqlTransaction tn;
        //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);

        //connection.Open();
        //tn = connection.BeginTransaction();

        try
        {
            int iBatchID=-1;
            UploadManager.UploadCSV(_ObjUser.UserID, _theTable, lblBatchDesc.Text,
          lblExcelFileName.Text, guidNew, _strFilesPhisicalPath + "\\UserFiles\\AppFiles", 
           out strMsg, out iBatchID,  strFileExtension, strSelectedSheet,
           int.Parse(Session["AccountID"].ToString()), chkDataUpdateUniqueColumnID.Checked,
           hfImportTemlateID.Value == "" ? null : (int?)int.Parse(hfImportTemlateID.Value), null);

            if (strMsg == "")
            {
                //tn.Commit();
                //connection.Close();
                //connection.Dispose();
            }
            else
            {
                //tn.Rollback();
                //connection.Close();
                //connection.Dispose();

                lblMsg.Text = strMsg;
                return;

            }


            Response.Redirect("~/Pages/Record/UploadValidation.aspx?TableID=" + Request.QueryString["TableID"] + "&BatchID=" + Cryptography.Encrypt(iBatchID.ToString()) + "&FileInfo=" + Request.QueryString["FileInfo"] + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString(), false);
        }
        catch (Exception ex)
        {

            //tn.Rollback();
            //connection.Close();
            //connection.Dispose();
            lblMsg.Text = strMsg;
            //throw;
        }


    }




    //protected void PopulateLocationDDL()
    //{
    //    int iTN = 0;
    //    ddlLocation.DataSource = SiteManager.ets_Location_Select(null, int.Parse(_qsTableID), null,
    //            string.Empty, string.Empty,  true, null, null, null, null,
    //            int.Parse(Session["AccountID"].ToString()),
    //            "LocationName", "ASC",
    //            null, null, ref  iTN, "");

    //    ddlLocation.DataBind();
    //    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "-1");
    //    ddlLocation.Items.Insert(0, liSelect);


    //}




    
}



