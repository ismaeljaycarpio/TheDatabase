using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Text;
using DocGen.Utility;
using DocGen.DAL;
using BaseClasses;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using DocGen;

namespace DocGen.DocumentNS.ReportDetail
{
    public partial class Edit : SecurePage
    {

        string _strActionMode = "view";
        int? _iDocumentID;
        string _qsMode = "";
        string _qsDocumentID = "";
        User _ObjUser;




        protected void PopulateMenuDDL(int iAccountID)
        {
            //ddlMenu.Items.Clear();


            int iTemp = 0;
            DataTable dtMenu =  RecordManager.ets_Menu_Select(null, string.Empty, null,
                iAccountID, true,
                "Menu", "ASC", null, null, ref iTemp, null, null);



            TheDatabaseS.PopulateMenuDDL(ref ddlMenu);

            string strNone = "";
            foreach (DataRow dr in dtMenu.Rows)
            {
                  if (dr["Menu"].ToString() == "--None--" && dr["ParentMenuID"] == DBNull.Value)
                {
                    strNone = dr["MenuID"].ToString();
                    //lstMenuSelect.Remove(aMenu);
                    if (ddlMenu.Items.FindByValue(strNone) != null)
                        ddlMenu.Items.Remove(ddlMenu.Items.FindByValue(strNone));

                    System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("--None--", strNone);
                    ddlMenu.Items.Insert(0, liNone);

                    break;
                }

             
            }


            //ddlMenu.DataSource = lstMenuSelect;
            //ddlMenu.DataBind();

            //System.Web.UI.WebControls.ListItem liNone = new System.Web.UI.WebControls.ListItem("--None--", "");
            //ddlMenu.Items.Insert(0, liNone);
           


        }

        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (Request.QueryString["popup"] != null)
            {
                this.Page.MasterPageFile = "~/Home/PopUp.master";
                //this.Page.b
            }
        }

        //protected void ddlStyle_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
        //    {
        //        DAL.DocumentSectionStyle docStyle = ctx.DocumentSectionStyles.SingleOrDefault<DAL.DocumentSectionStyle>(d => d.DocumentSectionStyleID==Int32.Parse(ddlStyle.SelectedValue));

        //        if (docStyle != null)
        //        {
        //            txtStyle.Text = docStyle.StyleDefinition;

        //        }
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            int iTemp = 0;
            _ObjUser = (User)Session["User"];
            if (!IsPostBack)
            {
                //if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2,"))
                //{ Response.Redirect("~/Default.aspx", false); }

                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();



                PopulateMenuDDL(int.Parse(Session["AccountID"].ToString()));


                PopulateDocumentType();
                PopulateTableDDL();
                //PopulateStyles();

                try
                {
                    //ddlAllTable.Text = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
                    //ddlAllTable_SelectedIndexChanged(null, null);
                }
                catch
                {
                }

                DataTable theDataTable = Common.DataTableFromText("SELECT * FROM DocumentType WHERE DocumentTypeName='Custom Reports' AND AccountID=" + Session["AccountID"].ToString());

                if (theDataTable.Rows.Count > 0)
                {
                    hfDocumentTypeID.Value = theDataTable.Rows[0]["DocumentTypeID"].ToString();
                    ddlDocumentType.Text = hfDocumentTypeID.Value;
                }

                PopulateDcouments();

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

                    if (_qsMode != "add")
                        trMenu.Visible = false;

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



            // checking permission

            string strTitle = "Report Detail";
            switch (_strActionMode.ToLower())
            {
                case "add":

                    strTitle = "Create Report";
                    trPublished.Visible = false;
                    //divStyle.Visible = false;

                    break;

                case "view":


                    strTitle = "View Report";

                    PopulateTheRecord();

                    EnableTheRecordControls(false);
                    divSave.Visible = false;

                    break;

                case "edit":

                    strTitle = "Report Properties";

                    if (!IsPostBack)
                    {
                        tdCopyReports.Visible = false;
                        chkCopyFromExisting.Visible = false;

                        PopulateTheRecord();

                        divBack.Visible = false;
                        divClose.Visible = true;


                        if (Request.QueryString["popup"] == null)
                        {
                            divBack.Visible = true;
                            divClose.Visible = false;
                        }


                    }
                    break;


                default:
                    //?

                    break;
            }


            Title = strTitle;
            lblTitle.Text = strTitle;


            string strJS = @" $(document).ready(function () {
            var x = document.getElementById('ddlReports');

            if (x != null)
            { x.style.display = 'none'; }

            $('#chkCopyFromExisting').change(function (e) {
                ShowHideReportDDL();
            });

        });";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "AjaxJS", strJS, true);

        }





        protected bool IsUserInputOK()
        {
            //this is the final server side vaidation before database action


            return true;
        }

        protected void PopulateTheRecord()
        {
            try
            {
                //int iTemp = 0;
                //List<SystemOption> listSystemOption = SystemData.SystemOption_Select(_iSystemOptionID, "", "", "", null, null, "SystemOptionID", "ASC", null, null, ref iTemp);

                Document theDocument = DocumentManager.ets_Document_Detail((int)_iDocumentID);
                txtDocumentText.Text = theDocument.DocumentText;
                txtDocumentDescription.Text = theDocument.DocumentDescription;

                DataTable dtMenu = Common.DataTableFromText("SELECT * FROM Menu WHERE IsActive=1 AND DocumentID=" + _iDocumentID.ToString());
                if (dtMenu.Rows.Count > 0)
                {
                    Menu theMenu = RecordManager.ets_Menu_Details(int.Parse(dtMenu.Rows[0]["MenuID"].ToString()));
                    if (theMenu != null)
                    {
                        if (theMenu.ParentMenuID != null)
                        {
                            System.Web.UI.WebControls.ListItem liMenu = ddlMenu.Items.FindByValue(theMenu.ParentMenuID.ToString());
                            if (liMenu != null)
                            {
                                ddlMenu.SelectedValue = theMenu.ParentMenuID.ToString();
                                hfOldMenuID.Value = theMenu.MenuID.ToString();
                            }

                        }
                    }

                 
                }

                if (theDocument.TableID != null)
                {
                    System.Web.UI.WebControls.ListItem liTemp = ddlTable.Items.FindByValue(theDocument.TableID.ToString());
                    if (liTemp != null)
                    {
                        ddlTable.Text = theDocument.TableID.ToString();
                    }
                }


                //ddlDocumentType.SelectedValue = theDocument.DocumentTypeID.ToString();
                if (theDocument.DocumentDate != null)
                    txtStartDate.Text = theDocument.DocumentDate.Value.ToShortDateString();

                if (theDocument.DocumentEndDate != null)
                    txtEndDate.Text = theDocument.DocumentEndDate.Value.ToShortDateString();

                ddlDocumentType.Text = theDocument.DocumentTypeID.ToString();

                trFileName.Visible = true;
                lblPublishedAddress.Visible = true;

                if (theDocument.IsReportPublic != null && (bool)theDocument.IsReportPublic == true)
                {
                    chkPublished.Checked = true;

                    lblPublishedAddress.Text = "<a href='" + "http://" + Request.Url.Authority + Request.ApplicationPath + "/DocReports/Report.aspx?ReportID=" + Cryptography.Encrypt(theDocument.DocumentID.ToString()) + "' target='_blank'>"
                        + "http://" + Request.Url.Authority + Request.ApplicationPath + "/DocReports/Report.aspx?ReportID=" + Cryptography.Encrypt(theDocument.DocumentID.ToString()) + "</a>";
                }
                else
                {
                    chkPublished.Checked = false;
                    lblPublishedAddress.Text = "Private";
                }

                //edtContent.Text = theDocument.ReportHTML;
                //trReportContent.Visible = true;
                //divCreateReport.Visible = false;
                divSave.Visible = true;
                //divSaveToWord.Visible = true;
                //trReprortDates.Visible = false;
                //trReportTemplate.Visible = false;

                //if (theDocument.DocumentTypeID != null)
                //{
                //    try
                //    {
                //        ddlAllTable.Text = theDocument.TableID.ToString();
                //    }
                //    catch
                //    {
                //    }
                //}

                if (theDocument.ReportType != "")
                {
                    optReportType.SelectedValue = theDocument.ReportType.ToLower();

                    optReportType_SelectedIndexChanged(null, null);
                }
                

                if (_strActionMode == "edit")
                {
                    ViewState["theDocument"] = theDocument;
                }
                else if (_strActionMode == "view")
                {
                    //divEdit.Visible = true;

                }
            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Report Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;
            }

        }




        protected void EnableTheRecordControls(bool p_bEnable)
        {
            txtDocumentDescription.Enabled = p_bEnable;
            txtDocumentText.Enabled = p_bEnable;
            ddlDocumentType.Enabled = p_bEnable;


        }

        //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            DateTime? dtDateFrom = null;
            DateTime? dtDateTo = null;
            int? iMenuID = null;
            string strMenuName = "";

            if (ddlMenu.SelectedItem != null)
            {
                if (ddlMenu.SelectedValue != "")
                {
                    iMenuID = int.Parse(ddlMenu.SelectedValue);
                }
            }

            try
            {
                if (IsUserInputOK())
                {


                    switch (_strActionMode.ToLower())
                    {
                        case "add":
                            int iDocumentID = -1;

                            if (chkCopyFromExisting.Checked && ddlReports.SelectedValue != "-1")
                            {
                                iDocumentID =  DocumentManager.CloneDocument(int.Parse(ddlReports.SelectedValue),txtStartDate.Text,txtEndDate.Text,
                                    txtDocumentText.Text,txtDocumentDescription.Text);

                            }
                            else
                            {

                                

                                if (txtStartDate.Text != "")
                                {
                                    DateTime dtTemp;
                                    if (DateTime.TryParseExact(txtStartDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                    {
                                        dtDateFrom = dtTemp;
                                    }
                                }
                                if (txtEndDate.Text != "")
                                {
                                    DateTime dtTemp;
                                    if (DateTime.TryParseExact(txtEndDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                    {
                                        dtDateTo = dtTemp;                                        
                                    }
                                }

                                strMenuName = txtDocumentText.Text;
                                Document newDocument = new Document(null, int.Parse(Session["AccountID"].ToString()), txtDocumentText.Text,
                                      int.Parse(hfDocumentTypeID.Value), "", "",
                                     dtDateFrom, null, null, _ObjUser.UserID,
                                      null);
                                
                                newDocument.IsReportPublic = chkPublished.Checked;
                                newDocument.DocumentDescription = txtDocumentDescription.Text;
                                newDocument.DocumentEndDate = dtDateTo;
                                newDocument.ReportType = optReportType.SelectedValue;
                                if (ddlTable.SelectedValue != "")
                                {
                                    newDocument.TableID = int.Parse(ddlTable.SelectedValue);
                                }
                               
                                iDocumentID = DocumentManager.ets_Document_Insert(newDocument);

                                if (iMenuID != null)
                                {
                                    Menu newMenu = new Menu(null, strMenuName, newDocument.AccountID, true, true);
                                    newMenu.ParentMenuID = iMenuID;
                                    newMenu.DocumentID = iDocumentID;
                                    RecordManager.ets_Menu_Insert(newMenu);

                                }

                            }

                            if (optReportType.SelectedValue == "ssrs")
                            {
                                //Response.Redirect(txtDocumentDescription.Text, true);
                                string strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/SSRS.aspx?DocumentID=" + Cryptography.Encrypt(iDocumentID.ToString()) + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SSearchCriteriaID=" + Cryptography.Encrypt("-1");
                               Response.Redirect(strURL, true);
                            }
                            else if (optReportType.SelectedValue == "webpage")
                            {
                                //Response.Redirect(txtDocumentDescription.Text, true);
                                string strURL = hlBack.NavigateUrl;
                                Document theDoc = DocumentManager.ets_Document_Detail(iDocumentID);
                                if (theDoc.TableID != null)
                                {
                                    strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/RRP/ReportView.aspx?TableID=" + Cryptography.Encrypt(theDoc.TableID.ToString()) ;
                                }
                                
                                Response.Redirect(strURL, true);

                            }
                            else
                            {
                                Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Docgen/EditReport.aspx?DocumentID=" + iDocumentID.ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString(), false);
                            }

                            

                            break;

                        case "view":


                            break;

                        case "edit":
                            Document editDocument = (Document)ViewState["theDocument"];


                            if (txtStartDate.Text != "")
                                {
                                    DateTime dtTemp;
                                    if (DateTime.TryParseExact(txtStartDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                    {
                                        dtDateFrom = dtTemp;
                                    }
                                }
                                if (txtEndDate.Text != "")
                                {
                                    DateTime dtTemp;
                                    if (DateTime.TryParseExact(txtEndDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
                                    {
                                        dtDateTo = dtTemp;                                        
                                    }
                                }


                                strMenuName = txtDocumentText.Text;
                            editDocument.DocumentText = txtDocumentText.Text;
                            editDocument.DocumentDate = dtDateFrom;
                            editDocument.UserID = _ObjUser.UserID;
                            editDocument.DocumentDescription = txtDocumentDescription.Text;
                            editDocument.DocumentEndDate = dtDateTo;
                            editDocument.IsReportPublic = chkPublished.Checked;

                            editDocument.ReportType = optReportType.SelectedValue;

                            if (ddlTable.SelectedValue != "")
                            {
                                editDocument.TableID = int.Parse(ddlTable.SelectedValue);
                            }
                            else
                            {
                                editDocument.TableID = null;
                            }

                            DocumentManager.ets_Document_Update(editDocument);

                            if (ddlMenu.SelectedValue != "")
                            {
                                if (hfOldMenuID.Value != "")
                                {
                                    Menu theMenu = RecordManager.ets_Menu_Details(int.Parse(hfOldMenuID.Value));
                                    if (theMenu != null)
                                    {
                                        if (theMenu.ParentMenuID != null)
                                        {
                                            if (theMenu.ParentMenuID.ToString() == ddlMenu.SelectedValue)
                                            {
                                                //do nothing
                                            }
                                            else
                                            {

                                                //RecordManager.ets_Menu_Delete(int.Parse(hfOldMenuID.Value));
                                                Common.ExecuteText("DELETE Menu WHERE MenuID=" + hfOldMenuID.Value);

                                                Menu newMenu = new Menu(null, txtDocumentText.Text, editDocument.AccountID, true,  true);
                                                newMenu.ParentMenuID = iMenuID;
                                                newMenu.DocumentID = editDocument.DocumentID;
                                                RecordManager.ets_Menu_Insert(newMenu);

                                            }
                                        }

                                    }

                                }
                                else
                                {

                                    Menu newMenu = new Menu(null, txtDocumentText.Text, editDocument.AccountID, true, true);
                                    newMenu.ParentMenuID = iMenuID;
                                    newMenu.DocumentID = editDocument.DocumentID;
                                    RecordManager.ets_Menu_Insert(newMenu);

                                }

                            }
                            else
                            {
                                if (hfOldMenuID.Value != "")
                                {


                                    //RecordManager.ets_Menu_Delete(int.Parse(hfOldMenuID.Value));
                                    Common.ExecuteText("DELETE Menu WHERE MenuID=" + hfOldMenuID.Value);
                                }

                            }

                            
                            


                            if (Request.QueryString["popup"] != null)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Refresh", "SavedAndRefresh();", true);
                            }
                            else
                            {
                                Response.Redirect(hlBack.NavigateUrl);
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
                //lblMsg.Text = "Saved!";

                //Response.Redirect(hlBack.NavigateUrl, false);

            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "Report Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
                lblMsg.Text = ex.Message;
            }



        }



        //CLONE DOCUMENT OBJECT
        //protected int CloneDocument(int DocumentID)
        //{

        //    int NewDocID = -1;
        //    Dictionary<int, string> dicSectionImages = new Dictionary<int, string>();
        //    int TextSectionTypeID = Convert.ToInt32(ConfigurationManager.AppSettings["TextSectionTypeID"]);
        //    int ImageSectionTypeID = Convert.ToInt32(ConfigurationManager.AppSettings["ImageSectionTypeID"]);


        //    DateTime? dtDateFrom = null;
        //    DateTime? dtDateTo = null;

        //    if (txtStartDate.Text != "")
        //    {
        //        DateTime dtTemp;
        //        if (DateTime.TryParseExact(txtStartDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
        //        {
        //            dtDateFrom = dtTemp;
        //        }
        //    }
        //    if (txtEndDate.Text != "")
        //    {
        //        DateTime dtTemp;
        //        if (DateTime.TryParseExact(txtEndDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
        //        {
        //            dtDateTo = dtTemp;
                   
        //        }
        //    }

        //    using (DAL.DocGenDataContext ctx = new DocGenDataContext())
        //    {
        //        DateTime ThisMoment = DateTime.Now;
        //        DAL.Document newDoc;
        //        DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);

        //        //Clone common info
        //        newDoc = new DAL.Document()
        //        {
        //            DocumentTypeID = doc.DocumentTypeID,
        //            DocumentText = txtDocumentText.Text,
        //            DocumentDescription=txtDocumentDescription.Text,
        //            DocumentDate=dtDateFrom,
        //            DocumentEndDate=dtDateTo,
        //            DateAdded = ThisMoment,
        //            DateUpdated = ThisMoment,
        //            AccountID = int.Parse(Session["AccountID"].ToString()),
        //            UserID=(int)((User)Session["User"]).UserID,
        //            UniqueName="",
        //            FileTitle=""
        //        };

        //        //Clone document sections
        //        foreach (DAL.DocumentSection section in doc.DocumentSections)
        //        {
        //            DAL.DocumentSection newSection = new DAL.DocumentSection()
        //            {
        //                DocumentSectionTypeID = section.DocumentSectionTypeID,
        //                Position = section.Position,
        //                SectionName = section.SectionName,
        //                Content = section.Content,
        //                Filter = section.Filter,
        //                Details = section.Details,
        //                DateAdded  = ThisMoment,
        //                DateUpdated = ThisMoment,
        //                ValueFields=section.ValueFields,
        //                DocumentID=newDoc.DocumentID,
        //                DocumentSectionStyleID=section.DocumentSectionStyleID 
        //            };


                   
                        
        //            if (section.DocumentSectionTypeID == ImageSectionTypeID)//Type = Image
        //            {
        //                string FilePath = Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", section.DocumentSectionID));
        //                if (File.Exists(FilePath))
        //                {
        //                    dicSectionImages.Add(section.Position, FilePath);
        //                }
        //            }
        //            newDoc.DocumentSections.Add(newSection);
        //        }

        //        ctx.Documents.InsertOnSubmit(newDoc);
        //        ctx.SubmitChanges();

        //        NewDocID = newDoc.DocumentID;
        //        foreach (int sectionPosition in dicSectionImages.Keys.ToList<int>())
        //        {
        //            DocumentSection sectionWithAttachedImage = newDoc.DocumentSections.SingleOrDefault<DocumentSection>(iSection => iSection.Position == sectionPosition);
        //            if (sectionWithAttachedImage != null)
        //            {
        //                File.Copy(dicSectionImages[sectionPosition], Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", sectionWithAttachedImage.DocumentSectionID)));
        //            }
        //        }
        //    }

        //    return NewDocID;

        //}
       
        
        
        
        
        public int AccountID
        {
            get
            {
                int retVal = 0;
                if (Session["AccountID"] != null)
                    retVal = Convert.ToInt32(Session["AccountID"]);
                return retVal;
            }
        }

        //protected void PopulateStyles()
        //{
        //    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
        //    {
        //        var Styles = from s in ctx.DocumentSectionStyles
        //                     where s.AccountID == this.AccountID
        //                     orderby s.StyleName
        //                     select s;
        //        ddlStyle.DataSource = Styles;
        //        ddlStyle.DataTextField = "StyleName";
        //        ddlStyle.DataValueField = "DocumentSectionStyleID";
        //        ddlStyle.DataBind();
        //    }

        //    try
        //    {
        //        //ddlStyle.Text = ConfigurationManager.AppSettings["DefaultTextStyleID"].ToString();
        //        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
        //        {
        //            DAL.DocumentSectionStyle docStyle = ctx.DocumentSectionStyles.SingleOrDefault<DAL.DocumentSectionStyle>(d => d.StyleName == "Heading 1" && d.AccountID == this.AccountID);

        //            ddlStyle.Text = docStyle.DocumentSectionStyleID.ToString();
        //            txtStyle.Text = docStyle.StyleDefinition;
        //        }
        //    }
        //    catch
        //    {

        //    }



        //}

        protected void PopulateDcouments()
        {
            try
            {

                ddlReports.DataSource = Common.DataTableFromText("SELECT DocumentID,DocumentText FROM Document WHERE AccountID=" + Session["AccountID"].ToString() + " AND DocumentTypeID=" + hfDocumentTypeID.Value + " ORDER BY DocumentText");
                ddlReports.DataBind();
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "-1");
                ddlReports.Items.Insert(0, liSelect);

            }
            catch
            {
                //
            }

        }

        protected void PopulateDocumentType()
        {
            try
            {

                ddlDocumentType.DataSource = Common.DataTableFromText("SELECT DocumentTypeID,DocumentTypeName FROM DocumentType WHERE AccountID=" + Session["AccountID"].ToString());
                ddlDocumentType.DataBind();
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "-1");
                ddlDocumentType.Items.Insert(0, liSelect);

                //ddlDocumentType.Text = "Custom Reports";
                ddlDocumentType.Enabled = false;
            }
            catch
            {
                //
            }

        }


        protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
        {
           
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
            System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("--None--", "");
            ddlTable.Items.Insert(0, liAll);
            //}


        }

        protected void ddlReports_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlReports.SelectedValue != "-1")
            {
                Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(ddlReports.SelectedValue));
                if (theDocument != null)
                {
                    txtDocumentText.Text = theDocument.DocumentText;
                    txtDocumentDescription.Text = theDocument.DocumentDescription;
                }

            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "ShowHideReportDDL();", true);
        }
        protected void optReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (optReportType.SelectedValue.ToLower() == "ssrs")
            {
                trCopyReports.Visible = false;
                trDateStart.Visible = false;
                trDateEnd.Visible = false;
                trFileName.Visible = false;
                trPublished.Visible = false;

                trEditor.Visible = true;

                string strEditor = "http://localhost/reportserver";

                string strSSRS_Ediotr = SystemData.SystemOption_ValueByKey_Account("SSRS Report Editor", int.Parse(Session["AccountID"].ToString()), null);

                if (strSSRS_Ediotr != "")
                    strEditor = strSSRS_Ediotr;
                lblEditor.Text = "<a href='" + strEditor + "' >" + strEditor + "</a>";
                stgDescription.InnerText = "URL";
                trTable.Visible = true;
               
            }
            else if (optReportType.SelectedValue.ToLower() == "webpage")
            {
                trCopyReports.Visible = false;
                trDateStart.Visible = false;
                trDateEnd.Visible = false;
                trFileName.Visible = false;
                trPublished.Visible = false;

                trEditor.Visible = false;              
                stgDescription.InnerText = "URL";
                trTable.Visible = true;
            }
            else
            {
                stgDescription.InnerText = "Description";
                
                trCopyReports.Visible = true;
                trDateStart.Visible = true;
                trDateEnd.Visible = true;

                trFileName.Visible = true;
                trPublished.Visible = true;

                trEditor.Visible = false;
                trTable.Visible = false;

            }

            if (this.MasterPageFile.ToLower().IndexOf("home/rrp.master")==-1)
            {
                trTable.Visible = false;
            }
        }
}
}






//protected void lnkCreateReport_Click(object sender, EventArgs e)
//{
//    try
//    {



//        string strContent = "";
//        StringBuilder sbStyles = new StringBuilder();
//        sbStyles.AppendLine("<style>");


//        using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
//        {

//            //create <style>
//            var Styles = from s in ctx.DocumentSectionStyles
//                         orderby s.StyleName
//                         select s;
//            foreach (DocGen.DAL.DocumentSectionStyle style in Styles)
//            {
//                sbStyles.AppendLine("/*" + style.StyleName + "*/");
//                sbStyles.Append(".DOCGEN_TextStyle_");
//                sbStyles.AppendLine(style.DocumentSectionStyleID.ToString());
//                sbStyles.AppendLine("{");
//                sbStyles.AppendLine(style.StyleDefinition);
//                sbStyles.AppendLine("}");
//                sbStyles.AppendLine();
//            }

//            DocGen.DAL.Document doc = ctx.Documents.SingleOrDefault<DocGen.DAL.Document>(d => d.DocumentID ==1);
//            if (doc != null)
//            {
//                //have got the Report Template
//                if (doc.AccountID != int.Parse(Session["AccountID"].ToString()))
//                {
//                    Response.Redirect("~/LogIn.aspx");
//                }
//                //ltTitle.Text = doc.DocumentName;
//                bool HasContent = false;
//                int SCounter = 1;
//                StringBuilder sbTOC = new StringBuilder();
//                StringBuilder reportContent = new StringBuilder();
//                sbTOC.Append("<w:sdt sdtdocpart=\"t\" docparttype=\"Table of Contents\" docpartunique=\"t\" id=\"1\">");
//                sbTOC.Append("<p class=\"MsoTocHeading\">TABLE OF CONTENTS<w:sdtpr></w:sdtpr></p>");
//                reportContent.Append(String.Format("<div class=\"Section{0}\">", SCounter)); //Open new MS Word Section
//                foreach (DocGen.DAL.DocumentSection section in doc.DocumentSections.OrderBy(s => s.Position))
//                {
//                    switch (section.DocumentSectionTypeID)
//                    {
//                        case 1: //HTML Section
//                            reportContent.Append(SectionGenerator.GenerateHTMLSection(section));
//                            HasContent = true;
//                            break;
//                        case 2: //Text Section
//                            reportContent.Append(SectionGenerator.GenerateTextSection(section, ref sbTOC));
//                            HasContent = true;
//                            break;
//                        case 3: //Image Section
//                            reportContent.Append(SectionGenerator.GenerateImageSection(section));
//                            HasContent = true;
//                            break;
//                        case 4: //Table Section
//                            reportContent.Append(SectionGenerator.GenerateTableSection(section));
//                            HasContent = true;
//                            break;
//                        case 5: //Chart Section
//                            reportContent.Append("<br/>");

//                            reportContent.Append(SectionGenerator.GenerateChartSection(section,-1));
//                            HasContent = true;
//                            break;
//                        case 6: //Page Break Section
//                                 TableSectionFilter sectionFilter = JSONField.GetTypedObject<TableSectionFilter>(section.Filter);
//                                 string strStartDate = txtStartDate.Text;
//                                 string strEndDate = txtEndDate.Text;


//                                    for (int i = 0; i < sectionFilter.Params.Count -1; i++)
//                                    {

//                                        if (strStartDate != "")
//                                        {
//                                            if (sectionFilter.Params[i].Name == "@dDateFrom")
//                                            {
//                                                sectionFilter.Params[i].Value = ConvertUtil.GetDate(strStartDate);
//                                            }
//                                        }
//                                        if (strEndDate != "")
//                                        {
//                                            if (sectionFilter.Params[i].Name == "@dDateTo")
//                                            {
//                                                sectionFilter.Params[i].Value = ConvertUtil.GetDate(strEndDate);
//                                            }
//                                        }
//                                        //if (sectionFilter.Params[i].Name == "@sLocations")
//                                        //{
//                                        //    sectionFilter.Params[i].Value = GetLocationIDs();
//                                        //}
//                                    }

//                                 section.Filter = sectionFilter.GetJSONString();
//                                reportContent.Append(SectionGenerator.GenerateTableSection(section));
//                                HasContent = true;


//                            break;
//                    }
//                }
//                reportContent.Append("</div>"); //Close last MS Word Section    
//                sbTOC.Append("<p class=\"MsoNormal\"><!--[if supportFields]><span style='mso-element:field-end'></span><![endif]--><o:p>&nbsp;</o:p></p></w:sdt><br style=\"page-break-before:always\"/>");
//                Regex rTOC = new Regex("{TOC}");
//                strContent = rTOC.Replace(reportContent.ToString(), sbTOC.ToString());
//                ViewState["Content"] = strContent;
//            }
//        }
//        sbStyles.AppendLine("</style>");
//        strContent = ltCommonStyles.Text + sbStyles.ToString() + strContent;

//        ltTextStyles.Text = sbStyles.ToString();

//        //edtContent.Text = strContent;
//        //trReportContent.Visible = true;



//    }
//    catch
//    {
//        //
//    }

//}
//protected void lnkSaveToWord_Click(object sender, EventArgs e)
//{
//    Response.Clear();
//    Response.AddHeader("content-disposition", "attachment;filename=Report.doc");
//    Response.Charset = "";
//    Response.Cache.SetCacheability(HttpCacheability.NoCache);
//    Response.ContentType = "application/vnd.word";
//    Response.Write(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'>");
//    Response.Write("<head>");
//    Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
//    Response.Write("<meta name=ProgId content=Word.Document>");
//    Response.Write("<meta name=Generator content='Microsoft Word 9'>");
//    Response.Write("<meta name=Originator content='Microsoft Word 9'>");
//    if (_strActionMode.ToLower() == "add")
//    {
//        Response.Write(ltTextStyles.Text);
//        Response.Write(ltCommonStyles.Text);
//    }
//    Response.Write("<head>");
//    Response.Write("<body>");
//    if (_strActionMode.ToLower() == "add")
//    {
//        Response.Write(ReFormatForMSWord(ViewState["Content"].ToString()));
//    }
//    else
//    {
//        //Response.Write(ReFormatForMSWord(ReFormatForMSWord(edtContent.Text)));
//    }
//    Response.Write("<!--[if gte mso 9]>");
//    Response.Write("<xml>");
//    Response.Write("<w:WordDocument>");
//    Response.Write("<w:View>Print</w:View>");
//    Response.Write("<w:Zoom>100</w:Zoom>");
//    Response.Write("</w:WordDocument>");
//    Response.Write("</xml>");
//    Response.Write("<![endif]-->");
//    Response.Write("</body>");
//    Response.Write("</html>");
//    Response.End();

//}
//protected string ReFormatForMSWord(string input)
//{
//    string output = "";
//    Regex rPageBreak = new Regex("<div style=\"page-break-after: always;\">(.*?)</div>");
//    output = rPageBreak.Replace(input, "<br style=\"page-break-before:always\"/>");
//    return output;
//}
//protected void lnkPublishHTML_Click(object sender, EventArgs e)
//{
//Document newDocument = new Document(null, int.Parse(Session["AccountID"].ToString()), txtDocumentText.Text,
//                     int.Parse(hfDocumentTypeID.Value), "", "",
//                     DateTime.Now, null, null, _ObjUser.UserID,
//                     ddlAllTable.SelectedValue == "-1" ? null : (int?)int.Parse(ddlAllTable.SelectedValue));
//newDocument.IsReportPublic = true;
//newDocument.ReportHTML = edtContent.Text;

//int iDocumentID=  DocumentManager.ets_Document_Insert(newDocument);

//Response.Redirect("ReportPublished.aspx?ReportID=" + iDocumentID.ToString(), false);


//}
//protected void lnkPublishPDF_Click(object sender, EventArgs e)
//{




//    //BindGridAgainToExport();






//    string strFileName = "Report.pdf";



//    StringWriter sw = new StringWriter();
//    HtmlTextWriter hw = new HtmlTextWriter(sw);

//    //BindGridAgainToExport();                
//    //lblContent.Text = edtContent.Text;

//    //lblContent.RenderControl(hw);


//    StringReader sr = new StringReader(sw.ToString());
//    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0f);
//    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

//    MemoryStream ms = new MemoryStream();
//    PdfWriter w = PdfWriter.GetInstance(pdfDoc, ms);

//    pdfDoc.Open();
//    htmlparser.Parse(sr);
//    pdfDoc.Close();

//    _ObjUser = (User)Session["User"];

//    string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;

//    string strFolder = Server.MapPath("~\\Pages\\Document\\Uploads");

//    string strPath = strFolder + "\\" + strUniqueName;

//    System.IO.FileStream file = System.IO.File.Create(strPath);
//    file.Write(ms.ToArray(), 0, ms.ToArray().Length);
//    file.Close();


//    //Document theDocument = new Document(null, int.Parse(Session["AccountID"].ToString()), txtDocumentText.Text, int.Parse(hfDocumentTypeID.Value),
//    //   strUniqueName, strFileName, DateTime.Now, null, null, _ObjUser.UserID,
//    //   ddlAllTable.SelectedValue == "-1" ? null : (int?)int.Parse(ddlAllTable.SelectedValue));
//    //theDocument.IsReportPublic = true;
//    //theDocument.ReportHTML = edtContent.Text;

//    //int iDocumentID = DocumentManager.ets_Document_Insert(theDocument);

//    //Response.Redirect("ReportPublished.aspx?ReportID=" + iDocumentID.ToString(), false);
//}
//protected void ddlAllTable_SelectedIndexChanged(object sender, EventArgs e)
//{
//    PopulateLocationList();
//}
