using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;


public partial class Pages_Document_DocumentDetail : SecurePage
{

    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";

    string _strActionMode = "view";
    int? _iDocumentID;
    string _qsMode = "";
    string _qsDocumentID = "";
    User _ObjUser;

    int _iFolderID = -1;
    //private static string UPLOADFOLDER = "..\\..\\UserFiles\\Documents";

    private static string UPLOADFOLDER = "\\UserFiles\\Documents";

    protected void SaveNonFlash()
    {

        if (fuReadOnly.HasFile)
        {
            try
            {
                
                //string strFolder = "..\\..\\UserFiles\\Documents";

                string strFolder = "\\UserFiles\\Documents";

                string strFileName = fuReadOnly.FileName;
                string strUniqueName = Session["AccountID"].ToString() + "_" + Guid.NewGuid().ToString() + "_" + strFileName;// +"." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                //string strPath = Server.MapPath(strFolder + "\\" + strUniqueName);
                string strPath = _strFilesPhisicalPath + strFolder + "\\" + strUniqueName;

                fuReadOnly.SaveAs(strPath);



                string strActionMode = _strActionMode;

                string strDocumentDate = txtDocumentDate.Text;
                string strDocumentTypeID = ddlDocumentType.SelectedValue;
                string strDocumentText = txtDocumentText.Text;
                string strAccountID = Session["AccountID"].ToString();
                string strUserID = _ObjUser.UserID.ToString();
                //string strTableID = ddlAllTable.SelectedValue;

                if (strDocumentText == null)
                    strDocumentText = strFileName;

                if (strDocumentText == "")
                    strDocumentText = strFileName;

                switch (strActionMode.ToLower())
                {
                    case "add":



                        Document newDocument = new Document(null, int.Parse(strAccountID), strDocumentText,
                              strDocumentTypeID == "-1" ? null : (int?)int.Parse(strDocumentTypeID),
                                strUniqueName,
                                strFileName,
                                DateTime.ParseExact(strDocumentDate, "d/M/yyyy", CultureInfo.InvariantCulture),
                                null, null, int.Parse(strUserID),
                                null);


                        if (_iFolderID != -1)
                        {
                            newDocument.FolderID = _iFolderID;
                        }
                        newDocument.Size = fuReadOnly.PostedFile.ContentLength;
                        DocumentManager.ets_Document_Insert(newDocument);


                        break;

                    case "edit":
                        string strDocumentID = hfDocumentID.Value.ToString();
                        Document editDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));

                        editDocument.DocumentText = strDocumentText;
                        editDocument.DocumentDate = DateTime.ParseExact(strDocumentDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                        editDocument.DocumentTypeID = strDocumentTypeID == "-1" ? null : (int?)int.Parse(strDocumentTypeID);
                        editDocument.UserID = int.Parse(strUserID);

                        editDocument.FileTitle = strFileName;
                        editDocument.FileUniqename = strUniqueName;
                        //editDocument.TableID = strTableID == "-1" ? null : (int?)int.Parse(strTableID);

                        //editDocument.FolderID = null;
                        //if (_iFolderID != -1)
                        //{
                        //    editDocument.FolderID = _iFolderID;
                        //}

                        editDocument.Size = fuReadOnly.PostedFile.ContentLength;

                        DocumentManager.ets_Document_Update(editDocument);


                        break;

                    default:
                        //?
                        break;
                }
                //}








                //PopulateImageControl();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                return;
            }
            //Response.Redirect(hlBack.NavigateUrl);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveOK", "GoBack();", true);
        }//File upload done

    }





    protected void  Page_Load(object sender, EventArgs e)
    {
        _ObjUser = (User)Session["User"];


        _strFilesLocation = Session["FilesLocation"].ToString();
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();

        if (Request.QueryString["FolderID"] != null)
        {
            if (Request.QueryString["FolderID"].ToString() != "")
            {
                _iFolderID = int.Parse(Request.QueryString["FolderID"].ToString());
            }
        }

        if (!IsPostBack)
        {
            hfRootURL.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath;
            //if (Request.Browser.IsMobileDevice)
            //{
            //    fuReadOnly.Visible = true;
            //}

            //if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
            //{ Response.Redirect("~/Default.aspx", false); }

            string strRight = "full";
            if (_iFolderID == -1)
            {
                strRight = Common.GetValueFromSQL("SELECT TOP 1 RightType FROM UserFolder WHERE UserID=" + _ObjUser.UserID.ToString() + " AND FolderID IS NULL");
            }
            else
            {
                strRight = Common.GetValueFromSQL("SELECT TOP 1 RightType FROM UserFolder WHERE UserID=" + _ObjUser.UserID.ToString() + " AND FolderID=" + _iFolderID.ToString());

            }
            if (strRight == "")
                strRight = "full";

            if (Common.HaveAccess(strRight, "none"))
            {
                lnkUploadTest.Enabled = false;
                //file_upload.Enabled = false;
                fuReadOnly5.Visible = true;
                divUploadButton.Visible = false;
                lblMsg.Text = "Read Only Account. To upload please ask your Account Administrator for appropriate rights.";
            }

            PopulateDocumentType();
            //PopulateAllTableDDL();


            if (Request.QueryString["SearchCriteria"] != null)
            {
                PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
            }


            string strExtra = "";

            if (Request.QueryString["SSearchCriteriaID"]!=null)
                strExtra = "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

            hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + strExtra;
           
            hfDocURL.Value = hlBack.NavigateUrl;
        }

        if (IsPostBack)
        {
            if (txtDocumentDate.Text != "")
            {
                DateTime dtTemp;
                if (DateTime.TryParseExact(txtDocumentDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                {
                    txtDocumentDate.Text = dtTemp.ToShortDateString();
                }
            }
        }

       

        hfFolderID.Value = _iFolderID.ToString();
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


                if (Request.QueryString["DocumentID"] != null)
                {

                    _qsDocumentID = Cryptography.Decrypt(Request.QueryString["DocumentID"]);

                    _iDocumentID = int.Parse(_qsDocumentID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }

        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            {
                hlDocumentTypeEdit.Visible = false;
            }

        }

      

        // checking permission

        hfAccountID.Value = Session["AccountID"].ToString();
        hfUserID.Value = _ObjUser.UserID.ToString();
        hfActionMode.Value = _strActionMode.ToLower();

        string strTitle = "Document Detail";

        switch (_strActionMode.ToLower())
        {
            case "add":
                strTitle = "Upload Document";
                if (!IsPostBack)
                {
                    txtDocumentDate.Text = DateTime.Now.ToShortDateString();

                    //try
                    //{
                    //    ddlAllTable.Text = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
                    //}
                    //catch
                    //{

                    //}

                }              


                lblFileNameCaption.Visible = false;
                break;

            case "view":

                strTitle = "View Document";
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSaveAll.Visible = false;
                lblFileNameCaption.Visible = true;
                break;

            case "edit":
                strTitle = "Edit Document";

                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                lblFileNameCaption.Visible = false;
                break;


            default:
                //?

                break;
        }


        //SaveNonFlash();

        Title = strTitle;
        lblTitle.Text = strTitle;


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
                ddlDocumentType.Text = xmlDoc.FirstChild[ddlDocumentType.ID].InnerText;
               
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
        }


    }
    protected void PopulateDocumentType()
    {
        ddlDocumentType.DataSource = Common.DataTableFromText("SELECT DocumentTypeID,DocumentTypeName FROM DocumentType WHERE DocumentTypeName <>'Custom Reports' AND AccountID=" + Session["AccountID"].ToString());
        ddlDocumentType.DataBind();
        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "-1");
        ddlDocumentType.Items.Insert(0, liSelect);
    }
     
    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<SystemOption> listSystemOption = SystemData.SystemOption_Select(_iSystemOptionID, "", "", "", null, null, "SystemOptionID", "ASC", null, null, ref iTemp);

            Document theDocument = DocumentManager.ets_Document_Detail((int)_iDocumentID);

            hfDocumentID.Value = _iDocumentID.ToString();
            txtDocumentText.Text = theDocument.DocumentText;

            if (theDocument.DocumentTypeID!=null)
                ddlDocumentType.SelectedValue = theDocument.DocumentTypeID.ToString();

            txtDocumentDate.Text = theDocument.DocumentDate.Value.ToShortDateString();
            lblFileName.Visible = true;

           
            //try
            //{
            //    ddlAllTable.Text = theDocument.TableID.ToString();
            //}
            //catch
            //{
            //}
            

            lblFileName.Text = theDocument.FileTitle;

            if (_strActionMode == "edit")
            {
                ViewState["theDocument"] = theDocument;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;

                //if (Request.QueryString["SearchCriteria"] != null)
                //{
                //    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&DocumentID=" + Cryptography.Encrypt(_qsDocumentID) + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
                //}
                //else
                //{
                //    hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/DocumentDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&DocumentID=" + Cryptography.Encrypt(_qsDocumentID);
                //}
                
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Uploadify Hide.", "HideUplodify();", true);
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "DocumentDetail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtDocumentDate.Enabled = p_bEnable;
        txtDocumentText.Enabled = p_bEnable;
        ddlDocumentType.Enabled = p_bEnable;
        //file_upload.Enabled = p_bEnable;
        hlDocumentTypeEdit.Visible = p_bEnable;
        //ddlAllTable.Enabled = p_bEnable;
    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }



    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)

    //public void SaveTemp()
    //{
    //    lnkSave_Click();
    //}

    //protected static void lnkSave_Click()
    //{
    //    lnkSave_Click(null, null);
    
    //}

    //protected void lnkSave_Click()
    //{
    //    try
    //    {
    //        if (IsUserInputOK())
    //        {

    //            switch (_strActionMode.ToLower())
    //            {
    //                case "add":



    //                    Document newDocument = new Document(null, int.Parse(Session["AccountID"].ToString()), txtDocumentText.Text,
    //                 int.Parse(ddlDocumentType.SelectedValue),
    //                 txtDocumentText.Text,
    //                 txtDocumentText.Text,
    //                 DateTime.ParseExact(txtDocumentDate.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
    //                 null, null, _objUser.UserID);
    //                    DocumentManager.ets_Document_Insert(newDocument);

    //                    break;

    //                case "view":


    //                    break;

    //                case "edit":
    //                    Document editDocument = (Document)ViewState["theDocument"];

    //                    editDocument.DocumentText = txtDocumentText.Text;
    //                    editDocument.DocumentDate = DateTime.ParseExact(txtDocumentDate.Text, "d/M/yyyy", CultureInfo.InvariantCulture);
    //                    editDocument.DocumentTypeID = int.Parse(ddlDocumentType.SelectedValue);

    //                    DocumentManager.ets_Document_Update(editDocument);


    //                    break;

    //                default:
    //                    //?
    //                    break;
    //            }
    //        }
    //        else
    //        {
    //            //user input is not ok

    //        }
    //        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx", false);

    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorLog theErrorLog = new ErrorLog(null, "Document Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //        lblMsg.Text = ex.Message;
    //    }



    //}


  


    protected void lnkSave_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
      
        try
        {
            if (IsUserInputOK())
            {
                //if (ddlDocumentType.SelectedValue == "-1")
                //{
                //    lblMsg.Text = "Please select Document Type.";
                //    return;
                //}

                SaveNonFlash();


                switch (_strActionMode.ToLower())
                {
                    case "add":

                     //if (hfFileName.Value !="")
                     //{


                        
                     //   strFileName = hfFileName.Value.ToString().Substring(0, hfFileName.Value.ToString().IndexOf("___"));
                     //   strFileUniqueName = hfFileName.Value.ToString().Substring(hfFileName.Value.ToString().IndexOf("___") + 3);
                        
                       

                     //        Document newDocument = new Document(null, int.Parse(Session["AccountID"].ToString()), txtDocumentText.Text,
                     //  int.Parse(ddlDocumentType.SelectedValue),
                     //  strFileUniqueName,
                     //  strFileName,
                     //  DateTime.ParseExact(txtDocumentDate.Text, "d/M/yyyy", CultureInfo.InvariantCulture),
                     //  null, null, _objUser.UserID);
                     //        DocumentManager.ets_Document_Insert(newDocument);
                     //}
                     //else
                     //{
                     //    lblMsg.Text = "Please browse a file.";
                     //    return;
                     //}
                        if (fuReadOnly.HasFile == false)
                        {
                            if (hfFileName.Value != "")
                            {


                                //string strFolder = "..\\..\\UserFiles\\Documents";

                                string strFolder = "\\UserFiles\\Documents";

                                string strUniqueName = hfFileName.Value.Substring(0, hfFileName.Value.IndexOf(','));
                                string strSize = hfFileName.Value.Substring(hfFileName.Value.IndexOf(',') + 1);
                               
                                string strFileName = (strUniqueName.Substring(strUniqueName.IndexOf("_") + 1)).Substring(37);
                              
                                //string strPath = Server.MapPath(strFolder + "\\" + strUniqueName);
                                string strPath = _strFilesPhisicalPath + strFolder + "\\" + strUniqueName;

                                string strDocumentDate = txtDocumentDate.Text;
                                string strDocumentTypeID = ddlDocumentType.SelectedValue;
                                string strDocumentText = txtDocumentText.Text;
                                string strAccountID = Session["AccountID"].ToString();
                                string strUserID = _ObjUser.UserID.ToString();
                                //string strTableID = ddlAllTable.SelectedValue;

                                if (strDocumentText == null)
                                    strDocumentText = strFileName;

                                if (strDocumentText == "")
                                    strDocumentText = strFileName;

                                Document newDocument = new Document(null, int.Parse(strAccountID), strDocumentText,
                                  strDocumentTypeID == "-1" ? null : (int?)int.Parse(strDocumentTypeID),
                                    strUniqueName,
                                    strFileName,
                                    DateTime.ParseExact(strDocumentDate, "d/M/yyyy", CultureInfo.InvariantCulture),
                                    null, null, int.Parse(strUserID),
                                    null);


                                if (_iFolderID != -1)
                                {
                                    newDocument.FolderID = _iFolderID;
                                }
                                newDocument.Size = double.Parse(strSize);
                                DocumentManager.ets_Document_Insert(newDocument);



                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a file.');", true);
                                return;
                            }
                        }
                      

                        break;

                    case "view":


                        break;

                    case "edit":

                        if (fuReadOnly.HasFile == false)
                        {



                            Document editDocument = (Document)ViewState["theDocument"];
                            string strFileName2 = "";
                            if (hfFileName.Value != "")
                            {


                                string strFolder = "\\UserFiles\\Documents";
                                string strUniqueName = hfFileName.Value.Substring(0, hfFileName.Value.IndexOf(','));
                                string strSize = hfFileName.Value.Substring(hfFileName.Value.IndexOf(',') + 1);
                                //string strFileName = strUniqueName.Substring(37);
                                string strFileName = strUniqueName.Substring(strUniqueName.IndexOf("_") + 1).Substring(37);
                                string strPath = _strFilesPhisicalPath + strFolder + "\\" + strUniqueName;

                                string strDocumentDate = txtDocumentDate.Text;
                                string strDocumentTypeID = ddlDocumentType.SelectedValue;
                                string strDocumentText = txtDocumentText.Text;

                                if (strDocumentText == null)
                                    strDocumentText = strFileName;

                                if (strDocumentText == "")
                                    strDocumentText = strFileName;


                                editDocument.Size = double.Parse(strSize);


                                editDocument.FileTitle = strFileName;
                                editDocument.FileUniqename = strUniqueName;
                                strFileName2 = strFileName;





                            }
                            editDocument.DocumentText = txtDocumentText.Text == "" ? strFileName2 : txtDocumentText.Text;
                            editDocument.DocumentDate = DateTime.ParseExact(txtDocumentDate.Text, "d/M/yyyy", CultureInfo.InvariantCulture);
                            editDocument.DocumentTypeID = ddlDocumentType.SelectedValue == "-1" ? null : (int?)int.Parse(ddlDocumentType.SelectedValue);
                            editDocument.UserID = _ObjUser.UserID;
                            DocumentManager.ets_Document_Update(editDocument);
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

            //Thread.Sleep(1000);
            lblMsg.Text = "Saved!";
            
            Response.Redirect(hlBack.NavigateUrl, false);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Document Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

     

    }


    //protected void PopulateAllTableDDL()
    //{
    //    int iTN = 0;
    //    _ObjUser = (User)Session["User"];

    //    if (_strActionMode.ToLower() == "add")
    //    {
    //        if ((bool)_ObjUser.IsAdvancedSecurity)
    //        {
    //            string strSTs = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)_ObjUser.UserID, "-1,3");

    //            ddlAllTable.DataSource = RecordManager.ets_Table_Select(null,
    //             null,
    //             null,
    //             int.Parse(Session["AccountID"].ToString()),
    //             null, null,  true,
    //             "st.TableName", "ASC",
    //             null, null, ref  iTN, strSTs);

    //            ddlAllTable.DataBind();
    //        }
    //        else
    //        {
    //            ddlAllTable.DataSource = RecordManager.ets_Table_Select(null,
    //                null,
    //                null,
    //                int.Parse(Session["AccountID"].ToString()),
    //                null, null,  true,
    //                "st.TableName", "ASC",
    //                null, null, ref  iTN, "");

    //            ddlAllTable.DataBind();

    //        }
    //    }
    //    else
    //    {

    //        if ((bool)_ObjUser.IsAdvancedSecurity)
    //        {

    //            ddlAllTable.DataSource = RecordManager.ets_Table_Select(null,
    //                    null,
    //                    null,
    //                    int.Parse(Session["AccountID"].ToString()),
    //                    null, null,  true,
    //                    "st.TableName", "ASC",
    //                    null, null, ref  iTN, Session["STs"].ToString());

    //            ddlAllTable.DataBind();
    //        }
    //        else
    //        {

    //            ddlAllTable.DataSource = RecordManager.ets_Table_Select(null,
    //                  null,
    //                  null,
    //                  int.Parse(Session["AccountID"].ToString()),
    //                  null, null,  true,
    //                  "st.TableName", "ASC",
    //                  null, null, ref  iTN, "");

    //            ddlAllTable.DataBind();

    //        }
    //    }

    //    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "-1");
    //    ddlAllTable.Items.Insert(0, liSelect);

    //} 

   


    #region Support Methods
   
  
    public void DownloadFile(string filePath)
    {
        if (System.IO.File.Exists(Server.MapPath(filePath)))
        {
            string strFileName = Path.GetFileName(filePath).Replace(" ", "%20");
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName);
            Response.Clear();
            Response.WriteFile(Server.MapPath(filePath));
            Response.End();
        }
    }
    //public string DeleteFile(string FileName)
    //{
    //    string strMessage = "";
    //    try
    //    {
    //        string strPath = Path.Combine(UPLOADFOLDER, FileName);
    //        if (System.IO.File.Exists(Server.MapPath(strPath)) == true)
    //        {
    //            System.IO.File.Delete(Server.MapPath(strPath));
    //            strMessage = "File Deleted";
    //        }
    //        else
    //            strMessage = "File Not Found";
    //    }
    //    catch (Exception ex)
    //    {
    //        strMessage = ex.Message;
    //    }
    //    return strMessage;
    //}
  
    #endregion

   
}
