using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.Utility;
using DocGen.DAL;
using BaseClasses;
using System.Text;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
namespace DocGen.Document
{
    public partial class DashBoard : SecurePage
    {
        User _objUser;
        //int _i = 1;
        Account _theAccount = null;
        UserRole _theUserRole;
        Role _theRole;
        bool _bReturn = false;
        public int DocumentID
        {
            get
            {
                return int.Parse(hfDocumentID.Value);              
            }
        }

        protected void PlayWithDashBoard()
        {

             if(!IsPostBack)
                {

                    string strDashboardName = "";
                    global::Document theDocument = null;

                     if (Request.QueryString["Dashboard"] != null)
                     {
                         strDashboardName = Request.QueryString["Dashboard"].ToString();
                     }
                     if (strDashboardName=="" && Request.QueryString["DashboardID"] != null)
                     {
                         string strDocumentID = Cryptography.Decrypt( Request.QueryString["DashboardID"].ToString());
                         theDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));
                     }

                     if (theDocument == null)
                     {
                         int? DashID = DocumentManager.dbg_Dashboard_BestFitting(strDashboardName, (int)_theUserRole.UserID, (int)_theRole.RoleID);
                         if (DashID != null)
                             theDocument = DocumentManager.ets_Document_Detail((int)DashID);
                     }
                     //theDocument = DocumentManager.dbg_Document_BestFittingDash((int)_theUserRole.UserRoleID, strDashboardName);

                    if(theDocument!=null)
                    {
                        hfDocumentID.Value = theDocument.DocumentID.ToString();
                        //when we design a dashboard from Dashboard list page (Pages/Home/DashBoard.aspx)
                        if (Request.QueryString["mode"] != null && Request.QueryString["DashboardID"] != null)
                        {
                            lnkEditDashboard_Click(null, null);
                        }                       
                    }
                    else
                    {
                        //CreateOrCloneDashboards();
                        string strDocText = _objUser.FirstName + " " + _objUser.LastName + " Dashboard";
                        global::Document newDocument = new global::Document(null, AccountID, strDocText, null, "DashBoard", "DashBoard",
                        null, null, null, _objUser.UserID, null);
                        newDocument.ForDashBoard = true;
                        int iNewDocumentID = DocumentManager.ets_Document_Insert(newDocument);
                        newDocument.DocumentID = iNewDocumentID;
                        theDocument = newDocument;
                        hfDocumentID.Value = iNewDocumentID.ToString();


                        ShowList();
                    }

                    if(theDocument!=null)
                    {
                        if(theDocument.UserID!=null && (int)theDocument.UserID==(int)_objUser.UserID)
                        {
                            hlMakeDefaultDashBoard.Visible = true;
                        }
                        else
                        {
                            hlMakeDefaultDashBoard.Visible = false;
                        }
                    }
                }

        }

        //protected void CreateOrCloneDashboards()
        //{

        //    string strDocText = _objUser.FirstName + " " + _objUser.LastName + " Dashboard";


        //    DataTable dtDefaultDoc = Common.DataTableFromText("SELECT DocumentID FROM [Document] WHERE UserID IS NULL AND AccountID=" + AccountID.ToString() + " AND ForDashBoard=1");

        //    if (dtDefaultDoc.Rows.Count > 0)
        //    {
        //        int id = DocumentManager.CloneDocument(int.Parse(dtDefaultDoc.Rows[0][0].ToString()), "", "", "", "");
        //        Common.ExecuteText("UPDATE Document SET ForDashBoard=1 WHERE DocumentID=" + id.ToString());

        //        hfDocumentID.Value = id.ToString();
        //    }
        //    else
        //    {
        //        global::Document newDocument = new global::Document(null, AccountID, strDocText, null, "DashBoard", "DashBoard",
        //                  null, null, null, _objUser.UserID, null);
        //        newDocument.ForDashBoard = true;
        //        int iNewDocumentID = DocumentManager.ets_Document_Insert(newDocument);

        //        hfDocumentID.Value = iNewDocumentID.ToString();
        //    }
        //}


        //public static int CloneDocument(int DocumentID, string txtStartDate, string txtEndDate, string txtDocumentText, string txtDocumentDescription)
        //{

        //    int NewDocID = -1;
        //    Dictionary<int, string> dicSectionImages = new Dictionary<int, string>();
        //    int TextSectionTypeID = Convert.ToInt32(ConfigurationManager.AppSettings["TextSectionTypeID"]);
        //    int ImageSectionTypeID = Convert.ToInt32(ConfigurationManager.AppSettings["ImageSectionTypeID"]);


        //    DateTime? dtDateFrom = null;
        //    DateTime? dtDateTo = null;

        //    global::Document theDocument = DocumentManager.ets_Document_Detail(DocumentID);
        //    dtDateFrom = theDocument.DocumentDate;
        //    dtDateTo = theDocument.DocumentEndDate;

        //    if (txtStartDate != "")
        //    {
        //        DateTime dtTemp;
        //        if (DateTime.TryParseExact(txtStartDate.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
        //        {
        //            dtDateFrom = dtTemp;
        //        }
        //    }
        //    if (txtEndDate != "")
        //    {
        //        DateTime dtTemp;
        //        if (DateTime.TryParseExact(txtEndDate.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
        //        {
        //            dtDateTo = dtTemp;

        //        }
        //    }

        //    using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
        //    {
        //        DateTime ThisMoment = DateTime.Now;
        //        DocGen.DAL.Document newDoc;
        //        DocGen.DAL.Document doc = ctx.Documents.SingleOrDefault<DocGen.DAL.Document>(d => d.DocumentID == DocumentID);

        //        //Clone common info
        //        newDoc = new DocGen.DAL.Document()
        //        {
        //            DocumentTypeID = doc.DocumentTypeID,
        //            DocumentText = txtDocumentText == "" ? theDocument.DocumentText : txtDocumentText,
        //            DocumentDescription = txtDocumentDescription == "" ? theDocument.DocumentDescription : txtDocumentDescription,
        //            DocumentDate = dtDateFrom,
        //            DocumentEndDate = dtDateTo,
        //            DateAdded = ThisMoment,
        //            DateUpdated = ThisMoment,
        //            AccountID = int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()),
        //            UserID = (int)((User)System.Web.HttpContext.Current.Session["User"]).UserID,
        //            UniqueName = "",
        //            FileTitle = ""
        //        };

        //        //Clone document sections
        //        foreach (DocGen.DAL.DocumentSection section in doc.DocumentSections)
        //        {
        //            DocGen.DAL.DocumentSection newSection = new DocGen.DAL.DocumentSection()
        //            {
        //                DocumentSectionTypeID = section.DocumentSectionTypeID,
        //                Position = section.Position,
        //                SectionName = section.SectionName,
        //                Content = section.Content,
        //                Filter = section.Filter,
        //                Details = section.Details,
        //                DateAdded = ThisMoment,
        //                DateUpdated = ThisMoment,
        //                ValueFields = section.ValueFields,
        //                DocumentID = newDoc.DocumentID,
        //                DocumentSectionStyleID = section.DocumentSectionStyleID,
        //                ColumnIndex = section.ColumnIndex,
        //                ParentSectionID = section.ParentSectionID

        //            };

        //            if (section.DocumentSectionTypeID == ImageSectionTypeID)//Type = Image
        //            {

        //            }



        //            if (section.DocumentSectionTypeID == ImageSectionTypeID)//Type = Image
        //            {
        //                string FilePath = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", section.DocumentSectionID));
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
        //            DocGen.DAL.DocumentSection sectionWithAttachedImage = newDoc.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(iSection => iSection.Position == sectionPosition);
        //            if (sectionWithAttachedImage != null)
        //            {
        //                File.Copy(dicSectionImages[sectionPosition], System.Web.HttpContext.Current.Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", sectionWithAttachedImage.DocumentSectionID)));
        //            }
        //        }



        //        DocGen.DAL.Document docNew = ctx.Documents.SingleOrDefault<DocGen.DAL.Document>(d => d.DocumentID == NewDocID);

        //        foreach (DocGen.DAL.DocumentSection sectionNew in docNew.DocumentSections)
        //        {

        //            if (sectionNew.ParentSectionID != null)
        //            {
        //                DocGen.DAL.DocumentSection sectionP = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(d => d.DocumentSectionID == sectionNew.ParentSectionID);

        //                if (sectionP != null)
        //                {
        //                    DocGen.DAL.DocumentSection sectionPNew = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(d => d.DocumentID == newDoc.DocumentID && d.Position == sectionP.Position);

        //                    if (sectionPNew != null)
        //                    {
        //                        sectionNew.ParentSectionID = sectionPNew.DocumentSectionID;
        //                    }
        //                }
        //                else
        //                {
        //                    sectionNew.ParentSectionID = null;
        //                }
        //                ctx.SubmitChanges();
        //            }

        //        }



        //    }



        //    return NewDocID;

        //}

        public string SectionName(string strDocumentSectionType)
        {
            if (strDocumentSectionType.ToLower() == "dashchart")
            {
                return "Chart";
            }
            else
            {
                return strDocumentSectionType;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (this.MasterPageFile.ToLower().IndexOf("rrp") > -1)
            {
                divNotEditDashboard.Style.Add("margin-left", "140px" );
                divEditDashboard.Style.Add("margin-left", "140px");
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
                return;

              _objUser = (User)Session["User"];
              _theUserRole = (UserRole)Session["UserRole"];
              _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
              _theRole = SecurityManager.Role_Details((int)_theUserRole.RoleID);
            if (!IsPostBack)
            {
                hlMakeDefaultDashBoard.NavigateUrl = "~/Pages/Help/FancyConfirm.aspx?message=" +
                    Cryptography.Encrypt("Do you want to use default dashboard?")
                    + "&okbutton=" + Cryptography.Encrypt(btnMakeDefaultDashBoard.ClientID);
                if (_theAccount.DisplayTableID != null)
                {
                    
                    //Session["tdbmsg"] = "Your data has been submitted for review and you will be notified as soon it has been imported.";
                    Response.Redirect("~/Pages/Record/RecordList.aspx?TableID=" +
                                                       Cryptography.Encrypt(_theAccount.DisplayTableID.ToString()), false);
                    return;
                }
                else
                {
                    if (Request.QueryString["Dashboard"] == null)
                    {

                        if (_theAccount.HomePageLink.Trim() != "" && Request.RawUrl != "/" + _theAccount.HomePageLink.Trim())
                        {
                            Response.Redirect("~/" + _theAccount.HomePageLink.Trim(), false);
                            return;
                        }
                    }

                }

              

                if (Session["DemoEmail"] != null)
                {
                    if (_objUser.Email.ToLower() == Session["DemoEmail"].ToString().ToLower())
                    {
                        if (Session["DemoTips"] == null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "AjaxDemoTips", " setTimeout(function () { OpenDemoTips(); }, 100);", true);
                            Session["DemoTips"] = "ok";
                        }

                    }
                }
                else if (Request.QueryString["FromSignUp"] != null)
                {
                    DataTable dtActiveTableList = Common.DataTableFromText("SELECT * FROM [Table] WHERE IsActive=1 AND AccountID=" + Session["AccountID"].ToString());

                    if (dtActiveTableList.Rows.Count == 0)
                    {
                        hfTableCount.Value = "";
                        hlShowWelcomeTips.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Welcome.aspx?notable=yes";
                    }
                    else
                    {
                        hfTableCount.Value = dtActiveTableList.Rows.Count.ToString();
                    }
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "AjaxWelcomeTips", " setTimeout(function () { OpenWelcomeTips(); }, 100);", true);

                    Response.Redirect("~/Pages/Record/TableOption.aspx?FirstTime=yes&MenuID=kdUxjBEM5oo=&SearchCriteria=kdUxjBEM5oo=", false);
                    return;

                }
                else
                {
                    // check if there are any active table
                    DataTable dtActiveTableList = Common.DataTableFromText("SELECT * FROM [Table] WHERE IsActive=1 AND AccountID="+ Session["AccountID"].ToString());

                    if (dtActiveTableList.Rows.Count == 0)
                    {
                        hfTableCount.Value = "";
                        hlShowWelcomeTips.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Welcome.aspx?notable=yes";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "AjaxNoTableTips", " setTimeout(function () { OpenWelcomeTips(); }, 100);", true);

                        Response.Redirect("~/Pages/Record/TableOption.aspx?FirstTime=yes&MenuID=kdUxjBEM5oo=&SearchCriteria=kdUxjBEM5oo=", false);
                        return;

                    }
                    else
                    {
                        hfTableCount.Value = dtActiveTableList.Rows.Count.ToString();
                    }

                }
            }


            if (!IsPostBack)
            {
                PlayWithDashBoard();

                AddStyleCSS();

                if (_theRole.AllowEditDashboard != null)
                {
                    if ((bool)_theRole.AllowEditDashboard)
                    {
                        lnkEditDashboard.Visible = true;
                        lnkViewDashboard.Visible = false;
                    }
                }

                

                if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                {
                    
                    lnkNoEditDashboard.Visible = false;

                    divEditDashboard.Visible = false;
                    divNotEditDashboard.Visible = true;
                    divMakeDefaultDashBoard.Visible = true;
                    int ID = DocumentID;
                    if (ID > 0)
                    {

                        //ShowList();
                        ShowReadonlyList();
                    }
                    else
                    {

                        //Response.Redirect("Summary.aspx");
                    }


                }
                else
                {

                    ShowReadonlyList();
                    
                    lnkNoEditDashboard.Visible = false;

                    divEditDashboard.Visible = false;
                    divNotEditDashboard.Visible = true;
                }

               
            }

            if (!IsPostBack)
            {
                if (Session["EditDashboard"] != null)
                {
                    lnkEditDashboard_Click(null, null);
                }

            }

            string strFancy = @"
                    $(function () {
                           $('.popupdefaultdash').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 500,
                                height: 250,
                                titleShow: false
                            });
                        });

                ";

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strFancyHome", strFancy, true);

        }

        protected void btnMakeDefaultDashBoard_Click(object sender, EventArgs e)
        {
            if(hfDocumentID.Value!="")
            {
                DocumentManager.ets_Document_Delete(int.Parse(hfDocumentID.Value));
                Session["EditDashboard"] = null;
//                Common.ExecuteText("UPDATE Document SET UserID=" + _objUser.UserID.ToString()
//                    + " WHERE ForDashBoard=1 AND UserID IS NULL AND DocumentID<>" + hfDocumentID.Value + " AND AccountID=" + _theUserRole.AccountID.ToString());

//                Common.ExecuteText("UPDATE Document SET UserID=null WHERE DocumentID=" + hfDocumentID.Value);

//                Common.ExecuteText("UPDATE UserRole SET DashBoardDocumentID=NULL WHERE DashBoardDocumentID=" + hfDocumentID.Value);
                
//                //now DELETE Other dashboards

//               DataTable dtDocuments= Common.DataTableFromText(@"SELECT DocumentID FROM Document WHERE ForDashBoard=1 AND AccountID="+_theUserRole.AccountID.ToString()+@" AND UserID IS NOT NULL AND DocumentID NOT IN
//                            (SELECT DashBoardDocumentID FROM UserRole WHERE DashBoardDocumentID IS NOT NULL AND   AccountID=" + _theUserRole.AccountID.ToString() + @" )");

//                foreach (DataRow dr in dtDocuments.Rows)
//                {
//                    DocumentManager.ets_Document_Delete(int.Parse(dr[0].ToString()));
//                }

                Response.Redirect(Request.RawUrl, false);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Default Dash", " alert('This is now the default dashboard.');", true);

            }
        }
        protected void lnkEditDashboard_Click(object sender, EventArgs e)
        {
            lnkEditDashboard.Visible = false;
            lnkViewDashboard.Visible = true;

            lnkNoEditDashboard.Visible = true;
            divEditDashboard.Visible = true;
            divNotEditDashboard.Visible = false;
            
            //check if this dashboard is for this user

            string strUserID = Common.GetValueFromSQL("SELECT UserID FROM Document WHERE DocumentID =" + hfDocumentID.Value);

            if (strUserID != "" && strUserID == _objUser.UserID.ToString())
            {
                //this is this user dashboard
            }
            else
            {
                //not this user so copy the default
                if (hfDocumentID.Value != "" && _theUserRole != null && _theUserRole.DashBoardDocumentID != null && (int)_theUserRole.DashBoardDocumentID == int.Parse(hfDocumentID.Value))
                {
                    //is this dashboard in UserRole
                }
                else
                {
                    //CreateOrCloneDashboards();
                    string strDocText = _objUser.FirstName + " " + _objUser.LastName + " Dashboard";
                    int id = DocumentManager.CloneDocument(int.Parse(hfDocumentID.Value), "", "", "", "");
                    Common.ExecuteText("UPDATE Document SET ForDashBoard=1 WHERE DocumentID=" + id.ToString());

                    hfDocumentID.Value = id.ToString();

                }
            }

       
            ShowList();
            Session["EditDashboard"] = "yes";
        }

        protected void lnkNoEditDashboard_Click(object sender, EventArgs e)
        {

            if (_theRole.AllowEditDashboard != null)
            {
                if ((bool)_theRole.AllowEditDashboard)
                {
                    lnkEditDashboard.Visible = true;
                    lnkViewDashboard.Visible = false;
                }
            }


            lnkViewDashboard.Visible = false;
            lnkNoEditDashboard.Visible = false;
            divEditDashboard.Visible = false;
            divNotEditDashboard.Visible = true;
            ShowReadonlyList();
            Session["EditDashboard"] = null;
        }

        protected void AddStyleCSS()
        {
            StringBuilder sbStyles = new StringBuilder();
            sbStyles.AppendLine("<style>");

            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                var Styles = from s in ctx.DocumentSectionStyles
                             where s.AccountID == this.AccountID
                             orderby s.StyleName
                             select s;
                foreach (DAL.DocumentSectionStyle style in Styles)
                {
                    sbStyles.AppendLine("/*" + style.StyleName + "*/");
                    sbStyles.Append(".DOCGEN_TextStyle_");
                    sbStyles.AppendLine(style.DocumentSectionStyleID.ToString());
                    sbStyles.AppendLine("{");
                    sbStyles.AppendLine(style.StyleDefinition);
                    sbStyles.AppendLine("}");
                    sbStyles.AppendLine();
                }
                sbStyles.AppendLine("</style>");
                ltTextStyles.Text = sbStyles.ToString();



            }


        }

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


        protected bool ShowThisContent(int iDocumentSectionID)
        {
            try
            {
                DataTable dtShowWhen = RecordManager.dbg_ShowWhen_ForGrid(null,iDocumentSectionID,null);
                if (dtShowWhen.Rows.Count > 0)
                {
                   
                    DataTable dtUserData = new DataTable();
                    dtUserData.Columns.Add("DV");
                    dtUserData.Columns.Add("HC");
                    dtUserData.Columns.Add("RecordID");
                    dtUserData.AcceptChanges();
                    DataTable dtUsedTable = Common.DataTableFromText(@"SELECT DISTINCT C.TableID,SW.HideColumnID,C.SystemName FROM [ShowWhen] SW INNER JOIN [Column] C
	                                ON SW.HideColumnID=C.ColumnID
	                                WHERE DocumentSectionID=" + iDocumentSectionID.ToString());

                    if (dtUsedTable.Rows.Count>0)
                    {
                        foreach (DataRow drUT in dtUsedTable.Rows)
                        {                       
                            string strUserColumnSys = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE TableTableID=-1 AND TableID=" + drUT["TableID"].ToString());
                            if(strUserColumnSys!="")
                            {
                                //V00x,HC
                                DataTable dtOneTable = Common.DataTableFromText(@"SELECT DISTINCT " + drUT["SystemName"].ToString() + @"  AS DV," 
                                    + drUT["HideColumnID"].ToString() + @" AS HC,RecordID FROM [Record] 
                                    WHERE TableID=" + drUT["TableID"].ToString() + @" AND IsActive=1 
                                    AND " + strUserColumnSys + @"='" + _objUser.UserID.ToString() + @"'");
                                //AND " + drUT["SystemName"].ToString() + @" IS NOT NULL 
                                 //    AND LEN(" + drUT["SystemName"].ToString() + @")>0
                                int m = 0;
                                foreach (DataRow dr in dtOneTable.Rows) 
                                {
                                    if (m == 0)
                                    {
                                        Session["UserRecordID"] = dr["RecordID"].ToString();
                                    }                                     

                                    dtUserData.Rows.Add(dr.ItemArray);
                                    m = m + 1;
                                }
                                dtUserData.AcceptChanges();
                            }
                        }
                    }

                    if (dtUserData.Rows.Count>0)
                    {

                        string strFullFormula = "";
                        foreach (DataRow drSW in dtShowWhen.Rows)
                        {
                            if (drSW["DocumentSectionID"] != DBNull.Value && drSW["HideColumnID"] != DBNull.Value
                                && drSW["HideColumnValue"] != DBNull.Value && drSW["HideOperator"]!=DBNull.Value)
                            {                                
                                Column theHideColumn = RecordManager.ets_Column_Details(int.Parse(drSW["HideColumnID"].ToString()));
                                if (theHideColumn != null)
                                {
                                    string strEachFormula = "Value=0"; 
                                    foreach (DataRow drUD in dtUserData.Rows)
                                    {
                                        if (int.Parse(drSW["HideColumnID"].ToString()) == int.Parse(drUD["HC"].ToString()))
                                        {                                                                                     
                                            if (Common.IsDataValidCommon(theHideColumn.ColumnType, drUD["DV"].ToString(),
                                                drSW["HideOperator"].ToString(), drSW["HideColumnValue"].ToString()))
                                            {
                                                strEachFormula = "Value=1";
                                                break;
                                            }
                                            else
                                            {
                                                strEachFormula = "Value=0";
                                            }                                            
                                        }
                                    }

                                    if (strEachFormula!="")
                                        strFullFormula = strFullFormula + " " + drSW["JoinOperator"].ToString() + " " + strEachFormula;
                                    
                                }
                            }
                        }

                        if(strFullFormula!="")
                        {
                            strFullFormula = strFullFormula.Trim();
                            string strError = "";

                            if(UploadManager.IsDataValid("1",strFullFormula,ref strError))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                            
                        

                    }

                   
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                //
            }

            return false;
        }

        protected void ShowList()
        {


            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                var query = from s in ctx.DocumentSections
                            where s.DocumentID == DocumentID && (!s.ParentSectionID.HasValue || s.ParentSectionID == 0)
                            orderby s.Position
                            select s;

                rptSection.DataSource = query;
                rptSection.DataBind();

                DAL.DocumentSectionStyle docStyle = ctx.DocumentSectionStyles.SingleOrDefault<DAL.DocumentSectionStyle>(d => d.StyleName == "Heading 1" && d.AccountID == this.AccountID);

                if (rptSection.Items.Count == 0)
                {
                    //insert a HTML

                    DAL.DocumentSection newHTMLSection = new DAL.DocumentSection();
                    newHTMLSection.DocumentID = DocumentID;
                    //newSection.SectionName = txtTitle.Text;
                    newHTMLSection.DocumentSectionTypeID = 1; //HTML

                    newHTMLSection.Content = @"                                
                            <div><span style='font-weight: bold; font-size: 15px;'>The Dashboard</span>
                                        <p>This is your dashboard where you can show:</p>
                                        <ul style='list-style-type: circle;'>
                                            <li>Maps</li>
                                            <li>Graphs</li>
                                            <li>Data</li>
                                            <li>Pictures</li>
                                            <li>Links</li>
                                        </ul>
                                        <p>And so on... If you are an admin user you can edit the dashboard by clicking on the link in the bottom right corner.</p>
                                    </div>
                            ";
                    newHTMLSection.Position = 1;
                    newHTMLSection.DateAdded = DateTime.Now;
                    newHTMLSection.DateUpdated = DateTime.Now;

                    //DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);
                    //if (doc != null)
                    //{
                    //    newHTMLSection.Content = doc.DocumentText;
                    //}
                    ctx.DocumentSections.InsertOnSubmit(newHTMLSection);


                    //Insert Column section

                   // DAL.DocumentSection newColumnSection = new DAL.DocumentSection();
                   // newColumnSection.DocumentID = DocumentID;
                   // newColumnSection.DocumentSectionTypeID = 4; //Columns Section
                   // newColumnSection.Position = 1;
                   // newColumnSection.DateAdded = newColumnSection.DateUpdated = DateTime.Now;
                   // newColumnSection.SectionName = "N/A";
                   // newColumnSection.Content = "2";
                   // DAL.ColumnsSectionDetail csd = new DAL.ColumnsSectionDetail();

                   // csd.Spacing =10;
                   // csd.Widths = new List<int>();
                   // //csd.Widths.Add(10);
                    
                   // newColumnSection.Details = csd.GetJSONString();
                   // newColumnSection.DateUpdated = DateTime.Now;

                   //ctx.DocumentSections.InsertOnSubmit(newColumnSection);

                   //ctx.SubmitChanges();

                    //insert Map section
                    MapSectionDetail mapDetail = new MapSectionDetail();
                    Account theAccount = SecurityManager.Account_Details((int)AccountID);

                    mapDetail.Address = "";
                    mapDetail.Latitude = theAccount.MapCentreLat == null ? -33.87365 : theAccount.MapCentreLat;
                    mapDetail.Longitude = theAccount.MapCentreLong == null ? 151.20688960000007 : theAccount.MapCentreLong;
                    mapDetail.MapScale = theAccount.MapZoomLevel==null?18:theAccount.MapZoomLevel;
                    mapDetail.ShowLocation = -1;

                    mapDetail.Height = 300;
                    mapDetail.Width = 800;

                    DAL.DocumentSection newMapSection = new DAL.DocumentSection();
                                 
                    newMapSection.DocumentID = DocumentID;
                    newMapSection.DocumentSectionTypeID = 7; //Map
                    newMapSection.Content = "";
                    newMapSection.Details = mapDetail.GetJSONString();
                    newMapSection.Position = 2;
                    newMapSection.DateAdded = DateTime.Now;
                    newMapSection.DateUpdated = DateTime.Now;
                    newMapSection.ColumnIndex = 0;
                    ctx.DocumentSections.InsertOnSubmit(newMapSection);

                    

                    //Graph Section
                   // int iNewGraphOptionID;

                   // //DataTable dtGraphSection = Common.DataTableFromText("SELECT * FROM GraphOption WHERE AccountID=" + AccountID.ToString());


                   // GraphOption newGraphOption = new GraphOption(null, (int)AccountID,
                   //2, "Dashboard", "H");


                   // newGraphOption.UserReportDate = true;
                   // newGraphOption.Width = 450;
                   // newGraphOption.Height = 500;
                   // newGraphOption.IsActive = true;

                   // newGraphOption.Legend = "Top";
                   // newGraphOption.ReportChart = false; //??
                   // iNewGraphOptionID = GraphManager.ets_GraphOption_Insert(newGraphOption, null, null);

                   // DocGen.DAL.DocumentSection newDashChartSection = new DocGen.DAL.DocumentSection();
                   // newDashChartSection.DocumentID = (int)DocumentID;
                   // newDashChartSection.SectionName = "Chart";
                   // newDashChartSection.DocumentSectionTypeID = 9; //DashChart 
                   // newDashChartSection.ValueFields = "1";

                   // newDashChartSection.Position = 3;
                   // newDashChartSection.DateAdded = DateTime.Now;
                   // newDashChartSection.DateUpdated = DateTime.Now;

                   // newDashChartSection.Details = iNewGraphOptionID.ToString();

                   // newDashChartSection.ParentSectionID = newColumnSection.DocumentSectionID;
                   // newDashChartSection.ColumnIndex = 1;
                   // ctx.DocumentSections.InsertOnSubmit(newDashChartSection);





                    //Insert Column section

                    //DAL.DocumentSection newColumnSection2 = new DAL.DocumentSection();
                    //newColumnSection2.DocumentID = DocumentID;
                    //newColumnSection2.DocumentSectionTypeID = 4; //Columns Section
                    //newColumnSection2.Position = 4;
                    //newColumnSection2.DateAdded = newColumnSection.DateUpdated = DateTime.Now;
                    //newColumnSection2.SectionName = "N/A";
                    //newColumnSection2.Content = "2";
                    //DAL.ColumnsSectionDetail csd2 = new DAL.ColumnsSectionDetail();

                    //csd2.Spacing = 40;
                    //csd2.Widths = new List<int>();
                    ////csd.Widths.Add(10);

                    //newColumnSection2.Details = csd2.GetJSONString();
                    //newColumnSection2.DateUpdated = DateTime.Now;

                    //ctx.DocumentSections.InsertOnSubmit(newColumnSection2);

                    //ctx.SubmitChanges();



                    //insert a HTML

                    


                    //DAL.DocumentSection newUploadHTML = new DAL.DocumentSection();
                    //newUploadHTML.DocumentID = DocumentID;
                    //newUploadHTML.DocumentSectionTypeID = 1; //HTML

                    //newUploadHTML.Content = "<h2>Recent Uploads</h2>";
                    //newUploadHTML.Position = 5;
                    //newUploadHTML.DateAdded = DateTime.Now;
                    //newUploadHTML.DateUpdated = DateTime.Now;
                    //newUploadHTML.ParentSectionID = newColumnSection2.DocumentSectionID;
                    //newUploadHTML.ColumnIndex = 0;
                    //ctx.DocumentSections.InsertOnSubmit(newUploadHTML);


                    ////upload section

                    //DAL.DocumentSection newUploadsSection = new DAL.DocumentSection();

                    //newUploadsSection.DocumentID = DocumentID;
                    //newUploadsSection.DocumentSectionTypeID = 6; //Table Table

                    //newUploadsSection.Position = 6;
                    //newUploadsSection.DateAdded = DateTime.Now;
                    //newUploadsSection.DateUpdated = DateTime.Now;
                    //newUploadsSection.ParentSectionID = newColumnSection2.DocumentSectionID;
                    //newUploadsSection.ColumnIndex = 0;

                    //TableSectionDetails tsc = new TableSectionDetails();
                    //List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();
                    //TableSectionFilter filter = new TableSectionFilter();
                    //TableSectionOtherInfo tblOtherInfo = new TableSectionOtherInfo();

                    //tblOtherInfo.TableType = "system";
                    //tblOtherInfo.SystemTableName = "uploads";
                    //tblOtherInfo.ShowTimeWithDate = true;

                    //newUploadsSection.Content = "ets_Recent_Uploads";
                    //lstColumns = GetUploadsColumns();
                    //filter.SPName = "ets_Recent_Uploads";

                    //List<SPInputParam> lstParams = new List<SPInputParam>();
                    //SPInputParam pST = new SPInputParam();
                    //pST.Name = "@nAccountID";
                    //pST.DataType = "int";
                    //pST.Value = AccountID;
                    //lstParams.Add(pST);
                    //filter.Params = lstParams;

                    //tsc.Columns = lstColumns;
                    //newUploadsSection.Details = tsc.GetJSONString();
                    //newUploadsSection.Filter = filter.GetJSONString();
                    //newUploadsSection.ValueFields = tblOtherInfo.GetJSONString();
                    //ctx.DocumentSections.InsertOnSubmit(newUploadsSection);


                    ////alert HTML

                    //DAL.DocumentSection newAlertHTML = new DAL.DocumentSection();
                    //newAlertHTML.DocumentID = DocumentID;
                    //newAlertHTML.DocumentSectionTypeID = 1; //HTML

                    //newAlertHTML.Content = "<h2>Recent Notifications</h2>";
                    //newAlertHTML.Position = 7;
                    //newAlertHTML.DateAdded = DateTime.Now;
                    //newAlertHTML.DateUpdated = DateTime.Now;
                    //newAlertHTML.ParentSectionID = newColumnSection2.DocumentSectionID;
                    //newAlertHTML.ColumnIndex = 1;
                    //ctx.DocumentSections.InsertOnSubmit(newAlertHTML);



                    ////Alert section


                    //DAL.DocumentSection newAlertSection = new DAL.DocumentSection();

                    //newAlertSection.DocumentID = DocumentID;
                    //newAlertSection.DocumentSectionTypeID = 6; //Table Table

                    //newAlertSection.Position = 8;
                    //newAlertSection.DateAdded = DateTime.Now;
                    //newAlertSection.DateUpdated = DateTime.Now;
                    //newAlertSection.ParentSectionID = newColumnSection2.DocumentSectionID;
                    //newAlertSection.ColumnIndex = 1;

                    //TableSectionDetails tsc2 = new TableSectionDetails();
                    //List<TableSectionColumn> lstColumns2 = new List<TableSectionColumn>();
                    //TableSectionFilter filter2 = new TableSectionFilter();
                    //TableSectionOtherInfo tblOtherInfo2 = new TableSectionOtherInfo();

                    //tblOtherInfo2.TableType = "system";
                    //tblOtherInfo2.SystemTableName = "alerts";
                    //tblOtherInfo2.ShowTimeWithDate = true;

                    //newAlertSection.Content = "ets_Recent_Alerts";
                    //lstColumns2 = GetAlertsColumns();
                    //filter2.SPName = "ets_Recent_Alerts";

                    //List<SPInputParam> lstParams2 = new List<SPInputParam>();
                    //SPInputParam pST2 = new SPInputParam();
                    //pST2.Name = "@nAccountID";
                    //pST2.DataType = "int";
                    //pST2.Value = AccountID;
                    //lstParams2.Add(pST2);
                    //filter2.Params = lstParams2;

                    //tsc2.Columns = lstColumns2;
                    //newAlertSection.Details = tsc2.GetJSONString();
                    //newAlertSection.Filter = filter2.GetJSONString();
                    //newAlertSection.ValueFields = tblOtherInfo2.GetJSONString();
                    //ctx.DocumentSections.InsertOnSubmit(newAlertSection);
                    


                    ctx.SubmitChanges();
                    

                    

                  

                }
            }

            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                
                var query2 = from s in ctx.DocumentSections
                             where s.DocumentID == DocumentID && (!s.ParentSectionID.HasValue || s.ParentSectionID == 0)
                             orderby s.Position
                             select s;

                rptSection.DataSource = query2;
                rptSection.DataBind();

            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowList();
            //Response.Redirect(Request.RawUrl, true);
        }


        public List<TableSectionColumn> GetUploadsColumns()
        {
            List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "BatchID",
                DisplayName = " ",
                Visible = true,
                Position = 1,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "TableID",
                DisplayName = "TableID",
                Visible = false,
                Position = 1,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "TableName",
                DisplayName = SecurityManager.etsTerminology("", "Table", "Table"),
                Visible = true,
                Position = 1,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "DateAdded",
                DisplayName = "Date Uploaded",
                Visible = true,
                Position = 2,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "ValidCount",
                DisplayName = "Valid",
                Visible = true,
                Position = 3,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "WarningCount",
                DisplayName = "Warning",
                Visible = true,
                Position = 4,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "NotValidCount",
                DisplayName = "Invalid",
                Visible = true,
                Position = 5,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            return lstColumns;
        }


        protected List<TableSectionColumn> GetAlertsColumns()
        {
            List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "LastWarningTime",
                DisplayName = "Last Time",
                Visible = true,
                Position = 1,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "TableName",
                DisplayName = SecurityManager.etsTerminology("", "Table", "Table"),
                Visible = true,
                Position = 2,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "WarningCount",
                DisplayName = "Warning",
                Visible = true,
                Position = 3,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "TableID",
                DisplayName = "TableID",
                Visible = false,
                Position = 4,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });



            return lstColumns;
        }

        protected void ShowReadonlyList()
        {
            try
            {

                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                    DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);
                    StringBuilder sbTOC = new StringBuilder();
                    StringBuilder reportContent = new StringBuilder();
                    bool HasContent = false;
                    int SCounter = 1;
                    reportContent.Append(String.Format("<div class=\"Section{0}\">", SCounter)); //Open new MS Word Section
                    foreach (DAL.DocumentSection section in doc.DocumentSections.OrderBy(s => s.Position))
                    {
                        switch (section.DocumentSectionTypeID)
                        {
                            case 7: //Map Section
                                if (section.ParentSectionID == null)
                                {
                                    reportContent.Append(SectionGenerator.GenerateMapSection(section));
                                    reportContent.Append("<br/>");
                                    HasContent = true;
                                }
                                break;

                            case 9: //DashChart Section
                                if (section.ParentSectionID == null)
                                {
                                    reportContent.Append(SectionGenerator.GenerateDashChartSection(section));
                                    reportContent.Append("<br/>");
                                    HasContent = true;
                                }
                                break;

                            case 8: //Dial Section
                                if (section.ParentSectionID == null)
                                {

                                    reportContent.Append(SectionGenerator.GenerateDialSection(section));
                                    reportContent.Append("<br/>");
                                    HasContent = true;

                                }
                                break;

                            case 1: //HTML Section
                                if (section.ParentSectionID == null)
                                {

                                    reportContent.Append(SectionGenerator.GenerateHTMLSection(section));
                                    //reportContent.Append("<br/>");
                                    HasContent = true;

                                }
                                break;
                            case 2: //Text Section
                                if (section.ParentSectionID == null)
                                {
                                    string strTextPart = SectionGenerator.GenerateTextSection(section, ref sbTOC);
                                    strTextPart = DocGenManager.TextSectionValues(section, strTextPart);
                                    reportContent.Append(strTextPart);
                                    //reportContent.Append("<br/>");
                                    HasContent = true;
                                }
                                break;
                            case 3: //Image Section
                                if (section.ParentSectionID == null)
                                {
                                    if (ShowThisContent(section.DocumentSectionID))
                                    {
                                        reportContent.Append(SectionGenerator.GenerateImageSection(section));
                                        //reportContent.Append("<br/>");
                                        HasContent = true;
                                    }                                  
                                }
                                break;
                            case 4: //Columns Section
                                if (section.ParentSectionID == null)
                                {
                                    reportContent.Append(SectionGenerator.GenerateColumnsSection(section, false));
                                    //reportContent.Append("<br/>");
                                    HasContent = true;
                                }
                                break;
                            case 5: //Chart Section
                                if (section.ParentSectionID == null)
                                {
                                    int iSearchCriteriaID = -1;
                                    //so far we are not using any filte, so it is now -1
                                    //if (Request.QueryString["SearchCriteria"] != null)
                                    //    iSearchCriteriaID = int.Parse( Cryptography.Decrypt( Request.QueryString["SearchCriteria"].ToString()));

                                    reportContent.Append(SectionGenerator.GenerateChartSection(section, iSearchCriteriaID));
                                    //reportContent.Append("<br/>");
                                    HasContent = true;
                                }
                                break;

                            case 10://Record Table Section

                                if (section.ParentSectionID == null)
                                {
                                    reportContent.Append(SectionGenerator.GenerateRecordTableSection(section));
                                    reportContent.Append("<br/>");
                                    HasContent = true;

                                }

                                break;
                            case 11://Calendar Section

                                if (section.ParentSectionID == null)
                                {
                                    reportContent.Append(SectionGenerator.GenerateCalendarSection(section));
                                    reportContent.Append("<br/>");
                                    HasContent = true;

                                }

                                break;


                            case 6: //Table Section
                                if (section.ParentSectionID == null)
                                {
                                    TableSectionFilter sectionFilter = JSONField.GetTypedObject<TableSectionFilter>(section.Filter);
                                    string strStartDate = "";
                                    string strEndDate = "";
                                    //DAL.DocumentFilter docFilter = DAL.JSONField.GetTypedObject<DAL.DocumentFilter>(doc.Filter);
                                    //   if (docFilter != null)
                                    //   {
                                    //       if (docFilter.StartDate.ToLocalTime() > DateTime.MinValue)
                                    //           strStartDate = ConvertUtil.GetDateString(docFilter.StartDate.ToLocalTime());
                                    //       if (docFilter.EndDate.ToLocalTime() > DateTime.MinValue)
                                    //           strEndDate = ConvertUtil.GetDateString(docFilter.EndDate.ToLocalTime());
                                    //   }

                                    for (int i = 0; i < sectionFilter.Params.Count - 1; i++)
                                    {

                                        if (strStartDate != "")
                                        {
                                            if (sectionFilter.Params[i].Name == "@dDateFrom")
                                            {
                                                sectionFilter.Params[i].Value = ConvertUtil.GetDate(strStartDate);
                                            }
                                        }
                                        if (strEndDate != "")
                                        {
                                            if (sectionFilter.Params[i].Name == "@dDateTo")
                                            {
                                                sectionFilter.Params[i].Value = ConvertUtil.GetDate(strEndDate);
                                            }
                                        }
                                    }
                                    sectionFilter.MaxRow = null;
                                    section.Filter = sectionFilter.GetJSONString();
                                    try
                                    {                                        
                                        reportContent.Append(SectionGenerator.GenerateTableSection(section));
                                    }
                                    catch
                                    {

                                    }
                                    //reportContent.Append("<br/>");
                                    HasContent = true;
                                }
                                break;
                            //if (HasContent)
                            //{
                            //    reportContent.Append("</div>"); //Close last MS Word Section
                            //    reportContent.Append("<br style=\"page-break-before:always\"/>");
                            //    SCounter++;
                            //    reportContent.Append(String.Format("<div class=\"Section{0}\">", SCounter)); //Open new MS Word Section
                            //}
                            //sbStyles.AppendLine("div.Section" + SCounter.ToString() + " {page:Section2;}");
                            //break;
                        }
                    }
                    reportContent.Append("</div>"); //Close last MS Word Section  

                    ltReportContent.Text = reportContent.ToString();
                }



            }
            catch
            {
                //
            }

        }
        
      

      
    }
}