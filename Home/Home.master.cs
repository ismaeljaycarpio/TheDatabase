using System;
using System.Data;
using System.Configuration;
using System.Collections;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using System.Xml.Linq;
//using System.Web.DynamicData;
using System.Collections.Generic;
using System.Text;
using System.Xml;




public partial class Home : System.Web.UI.MasterPage
{
   
    bool _bDemoUser = false;
    Account _theAccount;
    User _objUser;
    bool _bIsAccountHolder = false;
    UserRole _CurrentUserRole = null;
    bool _bGod = false;
    protected void Page_Init(object sender, EventArgs e)
    {

        if (Request.QueryString["public"] != null)
        {
            return;
        }

        if (Session["AccountID"]==null)
        {
          
            Response.Redirect("~/Login.aspx?Logout=yes", false);
            return;
        }

        _objUser = (User)Session["User"];
        _CurrentUserRole = (UserRole)Session["UserRole"];

        //string strUserAccountCount = Common.GetValueFromSQL("SELECT COUNT(UserRoleID) FROM UserRole WHERE UserID=" + _objUser.UserID.ToString());

        if (!IsPostBack )
        {
            if (Request.QueryString["TableID"] != null || Request.QueryString["ColumnID"] != null
                           || Request.QueryString["DocumentID"] != null || Request.QueryString["GraphOptionID"] != null
                           || Request.QueryString["TerminologyID"] != null || Request.QueryString["MenuID"] != null
                           || Request.QueryString["UploadID"] != null || Request.QueryString["AccountID"] != null)
            {
                Common.FindTheAccount();
            }
        }

        
        

       

        if (_CurrentUserRole.IsAccountHolder != null && (bool)_CurrentUserRole.IsAccountHolder )
        {
            _bIsAccountHolder = true;
            hfIsAccountHolder.Value = "yes";
        }

        _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            _bGod = true;
        }

        if (_objUser != null && _objUser.UserID > 0)
        {

            if (Context.User.Identity.IsAuthenticated)
            {

                //lkProfile.Text = ObjUser.FirstName + " " + ObjUser.LastName;
            }
            BindMenu();
            BindMenuProfile();
            BindMenuReport();
            if ((bool)_theAccount.ShowOpenMenu)
            {
                BindOpenMenu();
            }
            else
            {
                imgMenuOpen.Visible = false;
                menuOpen.Visible = false;
            }

           
            //if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            //{
            BindAccountMenu();
            //}
        }
        else
        {
            if (Session["LoginAccount"] == null)
            {
                Session.Clear();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                string strLoginAccount = Session["LoginAccount"].ToString();
                Session.Clear();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx?" + strLoginAccount, false);
            }
           
        }
    }

    protected void CheckSelectedMenu()
    {

        string path = Request.AppRelativeCurrentExecutionFilePath;
        bool bGotSelected = false;


        foreach (MenuItem item in menuETS.Items)
        {
            if (Request.RawUrl.IndexOf("Record/RecordList.aspx") > -1)
            {
                //
            }
            else
            {

                if (item.NavigateUrl.IndexOf(path) > -1)
                {
                    item.Selectable = true;
                    item.Selected = true;
                    bGotSelected = true;
                    break;
                }
            }

            
            //    item.Selected = item.NavigateUrl.Equals(path, StringComparison.InvariantCultureIgnoreCase);

            if (bGotSelected == false)
            {
                if (item.ChildItems.Count > 0)
                {

                    foreach (MenuItem citem in item.ChildItems)
                    {
                        if (citem.NavigateUrl != "")
                        {
                            if (Request.RawUrl.IndexOf(citem.NavigateUrl.Replace("~", "")) > -1)
                            {
                                if (citem.Value == item.Value)
                                {
                                    item.Selectable = true;
                                    item.Selected = true;
                                    bGotSelected = true;
                                    break;
                                }
                            }

                            if (bGotSelected == false)
                            {
                                if (citem.ChildItems.Count > 0)
                                {
                                    foreach (MenuItem ccitem in citem.ChildItems)
                                    {
                                        if (Request.RawUrl.IndexOf(ccitem.NavigateUrl.Replace("~", "")) > -1
                                            && ccitem.NavigateUrl.Trim()!="")
                                        {
                                            if (ccitem.Value == item.Value)
                                            {
                                                item.Selectable = true;
                                                item.Selected = true;
                                                bGotSelected = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        if (bGotSelected == false)
        {
            if (Request.RawUrl.IndexOf("Record/RecordDetail.aspx") > -1)
            {
                if (Request.QueryString["TableID"] != null)
                {

                    Table menuTable = RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())));

                    if (menuTable != null)
                    {

                        //Menu menuMenu = RecordManager.ets_Menu_Details((int)menuTable.ParentMenuID);

                        Menu menuMenu = RecordManager.ets_Menu_By_TableID((int)menuTable.TableID);

                        if (menuMenu!=null && menuMenu.ParentMenuID!=null)
                        {
                            Menu pMenu = RecordManager.ets_Menu_Details((int)menuMenu.ParentMenuID);

                            if (pMenu != null)
                                menuMenu = pMenu;
                        }

                        if (menuMenu != null)
                        {
                            foreach (MenuItem item in menuETS.Items)
                            {
                                if (menuMenu.MenuP == item.Value)
                                {
                                    item.Selectable = true;
                                    item.Selected = true;
                                    bGotSelected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        if (bGotSelected == false)
        {
            if (Request.RawUrl.IndexOf("Record/RecordList.aspx") > -1 ||
                Request.RawUrl.IndexOf("Record/RecordUpload.aspx") > -1 ||
                Request.RawUrl.IndexOf("Record/UploadValidation.aspx") > -1 ||
                Request.RawUrl.IndexOf("Graph/RecordChart.aspx") > -1)
            {
                if (Request.QueryString["TableID"] != null)
                {

                    Table menuTable = RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())));

                    if (menuTable != null)
                    {
                        //Menu menuMenu = RecordManager.ets_Menu_Details((int)menuTable.ParentMenuID);
                        Menu menuMenu = RecordManager.ets_Menu_By_TableID((int)menuTable.TableID);

                        if (menuMenu != null && menuMenu.ParentMenuID != null)
                        {
                            Menu pMenu = RecordManager.ets_Menu_Details((int)menuMenu.ParentMenuID);

                            if (pMenu != null)
                                menuMenu = pMenu;
                        }

                        if (menuMenu != null)
                        {

                            foreach (MenuItem item in menuETS.Items)
                            {
                                if (menuMenu.MenuP == item.Value)
                                {
                                    item.Selectable = true;
                                    item.Selected = true;
                                    bGotSelected = true;
                                    break;
                                }

                            }

                        }

                    }

                }
            }

        }


        if (bGotSelected == false)
        {
            if (Request.RawUrl.IndexOf("Record/TableDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Record/RecordColumnDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Instrument/SensorDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Document/DocumentTypeDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Record/TableGroupDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Instrument/SensorTypeDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Site/WorkSiteDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("User/Detail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Security/AccountDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("SystemData/ContentDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("SystemData/ErrorLogDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("SystemData/SystemOptionDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Document/DocumentDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("Instrument/CalibrationDetail.aspx") > -1 ||
                Request.RawUrl.IndexOf("SystemData/VisitorLog.aspx") > -1 ||
                Request.RawUrl.IndexOf("/ChartDefinition/") > -1 ||
                Request.RawUrl.IndexOf("/DemoEmail.aspx") > -1 ||
                 Request.RawUrl.IndexOf("/Security/MakePayment.aspx") > -1 ||
                  Request.RawUrl.IndexOf("/DocGen/DocumentStypeEdit.aspx") > -1 ||
                   Request.RawUrl.IndexOf("/DocGen/DocumentStyleList.aspx") > -1 ||
                    Request.RawUrl.IndexOf("/Security/ChangePassword.aspx") > -1 )
            {
                foreach (MenuItem item in menuETS.Items)
                {
                    if ("Admin" == item.Value)
                    {
                        item.Selectable = true;
                        item.Selected = true;
                        bGotSelected = true;
                        break;
                    }

                }
            }

        }  


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (Session["tdbmsgpb"] != null)
            {
                lblNotificationMessage.Text = Session["tdbmsgpb"].ToString();
                Session["tdbmsgpb"] = null;

                if (lblNotificationMessage.Text != "")
                {
                    lblNotificationMessage.Text = lblNotificationMessage.Text + "&nbsp; <a id=\"aNotificationMessageClose\" onclick=\"document.getElementById('divNotificationMessage').style.display = 'none';return false;\" href=\"#\" >Close</a>";
                }
            }
            else
            {
                lblNotificationMessage.Text = "";
            }
        }
        


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["public"] != null)
        {
            imgHouse.Visible = false;
            imgMenuOpen.Visible = false;
            Image2.Visible = false;
           
            PopulatePublicAccount();



            return;
        }

        if (Session["AccountID"] == null)
        {
            //if (Session["LoginAccount"] == null)
            //{
            //    Response.Redirect("~/Login.aspx",false);
            //}
            //else
            //{
            //    Response.Redirect("~/Login.aspx?" + Session["LoginAccount"].ToString(),false);
            //}

            if (Session["LoginAccount"] == null)
            {
                Session.Clear();
                Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx?ReturnURL=" + Server.UrlEncode(Request.RawUrl), false);

            }
            else
            {
                string strLoginAccount = Session["LoginAccount"].ToString();
                Session.Clear();
                Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx?ReturnURL=" + Server.UrlEncode(Request.RawUrl) + "&" + strLoginAccount, false);

            }
            return;

            
        }


        double iSec = 3;
        string strTopMsgNoOfMS = "3000";
        string strTopMessageDisplayNumberSeconds = SystemData.SystemOption_ValueByKey_Account("Top Message Display Number Seconds", _theAccount.AccountID, null);
        if (strTopMessageDisplayNumberSeconds != "")
        {
            double dTemp = 0;
            if (double.TryParse(strTopMessageDisplayNumberSeconds, out dTemp))
            {
                if (dTemp > 300)
                    dTemp = 300;

                iSec = dTemp;
                dTemp = dTemp * 1000;
                strTopMsgNoOfMS = dTemp.ToString("N0").Replace(",", "");

            }

        }



        ltMasterStyles.Text = @"<style  type='text/css'>
                            .cssanimations.csstransforms #divNotificationMessage {
                    -webkit-transform: translateY(-50px);
                    -webkit-animation: slideDown " + iSec.ToString() + @"s 0.2s 1 ease forwards;
                    -moz-transform:    translateY(-50px);
                    -moz-animation:    slideDown " + iSec.ToString() + @"s 0.2s 1 ease forwards;
                }
 </style>
            ";

        string strWebsiteMasterPageHeader = SystemData.SystemOption_ValueByKey_Account("Website Master Page Header", int.Parse(Session["AccountID"].ToString()), null);

        if (strWebsiteMasterPageHeader != "")
           this.Page.Title = strWebsiteMasterPageHeader;

        if (!Page.IsPostBack)
        {
            hlRenewNow.NavigateUrl = SystemData.SystemOption_ValueByKey_Account("ContactUsRenewal",null,null);
            //DataTable dtActiveTableList = Common.DataTableFromText("SELECT * FROM [Table] WHERE IsActive=1 AND AccountID=" + Session["AccountID"].ToString());
            //if (dtActiveTableList.Rows.Count == 0)
            //{
            //    divFirstTable.Visible = true;
            //}

            string strHideDocumentsMenu = SystemData.SystemOption_ValueByKey_Account("Hide Documents Top Menu", int.Parse(Session["AccountID"].ToString()), null);

            if (strHideDocumentsMenu != "" && strHideDocumentsMenu.ToLower() == "yes")
            {
                //tdDocumnetsMenu1.Visible = false;
                //tdDocumnetsMenu2.Visible = false;
                //tdDocumnetsMenu3.Visible = false;
                //menuOpen.Visible = false;
                menuOpen.Visible = false;
                imgMenuOpen.Visible = false;
            }


            if (Session["LoginAccount"] != null)
            {
                divMarketingMenu.Visible = false;
                Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

                if (theAccount != null)
                {                    
                    divCopyright.InnerText = theAccount.CopyRightInfo;                 

                }

            }
        }

        

        //if (Session["ExpireLeftDay"] != null)
        //{
                     
           


            //Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
            //Session["DoNotAllow"] = null;
            //if (theAccount.ExpiryDate != null)
            //{
            //    Session["ExpireLeftDay"] = Common.DaysBetween((DateTime)DateTime.Today, (DateTime)theAccount.ExpiryDate);

            //    if (theAccount.ExpiryDate.Value.AddDays(0) < DateTime.Today)
            //    {
            //        Session["DoNotAllow"] = "true";

            //    }
              
            //}

            //if (int.Parse(Session["ExpireLeftDay"].ToString()) < 10)
            //{
            //    divRenew.Visible = true;
            //    if (Session["DoNotAllow"] == null)
            //    {
            //        lblRenewMessage.Text = "Account expires in " +
            //        Session["ExpireLeftDay"].ToString() + " days. ";
            //    }
            //    else
            //    {
            //        Response.Redirect("~/Login.aspx?Logout=yes", true);
            //        return;
            //    }
            //}
        //}
        //else
        //{

            //this is for the data exceed
            divRenew.Visible = false;
            if (Session["DoNotAllow"] != null)
            {
                //Response.Redirect("~/Login.aspx?Logout=yes", true);
                //return;
            }
        //}

        if (!Page.IsPostBack)
        {
           

            if (!Context.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        CheckSelectedMenu();



        string strHidedivNotificationMessage = @"                                                     
                                                  $(document).ready(function () {

                                                      try
                                                        {
                                                            window.setTimeout(HidedivNotificationMessage," + strTopMsgNoOfMS + @");
                                                        }
                                                      catch(err)
                                                        {
                                                            //
                                                        }                                                            
                                                    });
                                                ";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "strHidedivNotificationMessage", strHidedivNotificationMessage, true);




        if (!Page.IsPostBack)
        {

          
            //if (Session["IsFlashSupported"] == null )
            //{
                string strIsFlashSupportedJS = @"                                                     
                                                   $(document).ready(function () {

                                                        function IsFlashSupported() {
                                                            try
                                                            {
                                                                var hfFlashSupport = document.getElementById('"+hfFlashSupport.ClientID+@"');
                                                                if (swfobject.hasFlashPlayerVersion('1')) {
                                                                    hfFlashSupport.value = 'yes';
                                                                }
                                                                else {
                                                                    hfFlashSupport.value = 'no';
                                                                }
                                                                //alert(hfFlashSupport.value);

                                                                $.ajax({
                                                                    url: '" + ResolveUrl("~/Pages/DocGen/REST/SectionREST.ashx")+@"?type=FlashSupport&hfFlashSupport=' + hfFlashSupport.value,
                                                                    cache: false,
                                                                    success: function (content) {
                                                                       //
                                                                    },
                                                                    error: function (a, b, c) {
                                                                       //
                                                                    }
                                                                });
                                                            }
                                                            catch(err)
                                                            {
                                                                //
                                                            }             


                                                        }
                                                        IsFlashSupported();
                                                        //setTimeout(function () { IsFlashSupported(); }, 1000);
                                                    });
                                                    
                                                ";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "strIsFlashSupportedJS", strIsFlashSupportedJS, true);
            //}
            //hlReport.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
            //hlDocuments.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");

            string strRefSite = "";

            if (Request.QueryString["Ref"] != null)
            {
                strRefSite = Request.QueryString["Ref"].ToString();
            }

            SystemData.VisitorInsert(_objUser, Request.UserHostAddress, Request.UserAgent, Request.AppRelativeCurrentExecutionFilePath, strRefSite);

            //User _objUser = (User)Session["User"];
            if (_objUser != null && _objUser.UserID > 0 && Session["DemoEmail"] != null)
            {
                if (_objUser.Email.ToLower() == Session["DemoEmail"].ToString().ToLower())
                {
                   trEndDemo.Visible=true;
                }
            }
            //CheckConcurrent();
            CheckDoNotAllow();
        }

//        string  strCellToolTip = @"var mouseX;
//                                var mouseY;
//                                $(document).mousemove(function (e) {
//                                    try
//                                    {
//                                        mouseX = e.pageX;
//                                        mouseY = e.pageY;
//                                    }
//                                    catch (err)
//                                    {
//                                       // alert(err.message)
//                                    }
//        
//                                });
//    
//
//                                $(function () {
//        
//                                    $('.js-tooltip-container').hover(function () {
//                                        //$(this).find('.js-tooltip').show();
//                                        try {
//                                            $(this).find('.js-tooltip').addClass('ajax-tooltip');
//                                            $(this).find('.ajax-tooltip').css({ 'top': mouseY, 'left': mouseX }).fadeIn('slow');
//                                        }
//                                        catch (err) {
//                                           // alert(err.message);
//                                        }
//                                    }, function () {
//                                        try {
//                                            $(this).find('.js-tooltip').hide();
//                                            $(this).find('.js-tooltip').removeClass('ajax-tooltip');
//                                            $(this).find('.ajax-tooltip').css({ 'top': mouseY, 'left': mouseX }).fadeOut('slow');
//                                        }
//                                        catch (err) {
//                                            //alert(err.message);
//                                        }
//                                    });
//       
//                                });";


//        ScriptManager.RegisterStartupScript(this, this.GetType(), "strCellToolTip", strCellToolTip, true);


       //notifications



        if (!IsPostBack)
        {
            if (Session["tdbmsg"] != null)
            {
                lblNotificationMessage.Text = Session["tdbmsg"].ToString();
                Session["tdbmsg"] = null;

                if (lblNotificationMessage.Text != "")
                {
                    lblNotificationMessage.Text = lblNotificationMessage.Text + "&nbsp; <a id='aNotificationMessageClose' href='#'>Close</a>";
                }
            }
            else
            {
                lblNotificationMessage.Text = "";
            }

        }
        else
        {
            Session["tdbmsg"] = null;
        }

    }

    protected void PopulatePublicAccount()
    {
        try
        {
            if (Request.QueryString["TableID"] != null)
            {
                int iTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
                Table theTable = RecordManager.ets_Table_Details(iTableID);
                if (theTable != null)
                {
                    Account theAccount = SecurityManager.Account_Details((int)theTable.AccountID);

                    if(theAccount!=null)
                    {
                        if (theAccount.Logo != null)
                        {
                            if ((bool)theAccount.UseDefaultLogo == false)
                                imgLogo.ImageUrl = "~/SSPhoto.ashx?AccountID=" + theAccount.AccountID.ToString() + "&type=o";
                        }

                    }

                }

            }
        
        }
        catch 
        {
        }
    }

    //protected void CheckConcurrent()
    //{
    //    if (Session["User"] != null)
    //    {
    //        //User _objUser = (User)Session["User"];

    //        string strSessionID = SecurityManager.User_SessionID_Get((int)_objUser.UserID);

    //        if (strSessionID != "")
    //        {
    //            if (strSessionID != Session.SessionID.ToString())
    //            {
    //                //ops 
    //                SecurityManager.User_LoggedOutCount_Increment((int)_objUser.UserID);
    //                //lkLogout_Click(null, null);

    //                //Session["User"] = null;
    //                //Session.Abandon();

    //                //FormsAuthentication.SignOut();

    //                //HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
    //                //oUseInfor.Expires = DateTime.Now.AddDays(-3d);
    //                //Response.Cookies.Add(oUseInfor);                  

    //                //Response.Redirect("~/Login.aspx?Logout=concurrent", false);

    //            }


    //        }
    //    }


    //}

    protected void CheckDoNotAllow()
    {
        if (Request.RawUrl.IndexOf("Security/AccountDetail.aspx") > -1) //Request.RawUrl.IndexOf("/Security/AccountTypeChange.aspx") > -1 ||
        {
            //its ok
        }
        else
        {
            if (Session["DoNotAllow"] != null)
            {
                if (Session["DoNotAllow"].ToString()=="true")
                {
                    //Response.Redirect("~/Pages/Security/AccountTypeChange.aspx?type=renew", false);
                    //Response.Redirect("~/Default.aspx",false);

                }

            }

        }

    }


    protected void PopulateSubMenu(ref MenuItem menuRoot, ref MenuItem menuParent, int iParentMenuID)
    {
        string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;
        DataTable dtSubMenu = Common.DataTableFromText(@"SELECT DocumentID,DocumentTypeID, MenuID,Menu,Menu.TableID,TableName,
        ExternalPageLink,OpenInNewWindow,MenuType FROM Menu left JOIN [Table] ON
    [Table].TableID=Menu.TableID WHERE Menu.IsActive=1 AND 
                Menu.AccountID=" + Session["AccountID"].ToString() + @" AND ParentMenuID=" + iParentMenuID.ToString()
                + @"  ORDER BY Menu.DisplayOrder");

        //AND (Menu.TableID IS NULL OR Menu.TableID IN (" + Session["STs"].ToString() + @"))
        //if ((bool)ObjUser.IsAdvancedSecurity)

        string strHideByShowWhen = "";
        if ((bool)_CurrentUserRole.IsAdvancedSecurity)
        {
            strHideByShowWhen = RecordManager.ets_Table_Hide_ByShowMenu((int)_CurrentUserRole.RoleID);
        }
        
        foreach (DataRow drSubMenu in dtSubMenu.Rows)
        {
            MenuItem miTempChild = new MenuItem();
            miTempChild.Text = drSubMenu["Menu"].ToString();
            miTempChild.Value = menuRoot.Value;


            if (miTempChild.Text == Common.MenuDividerText)
            {
                miTempChild.Selectable = false;
                //miTempChild.Text = "";
                miTempChild.Text = "<hr />";
              
                //miTempChild.
                //miTempChild.SeparatorImageUrl = "~/Images/menu_Divider_1.png";
            }

            if (drSubMenu["OpenInNewWindow"] != DBNull.Value && (bool)drSubMenu["OpenInNewWindow"])
            {
                miTempChild.Target = "_blank";
            }
            if (drSubMenu["TableID"] != DBNull.Value)
            {
                if(drSubMenu["Menu"].ToString()=="")
                    miTempChild.Text = drSubMenu["TableName"].ToString();


                miTempChild.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" +
                    Cryptography.Encrypt(drSubMenu["TableID"].ToString());

                //UserRole theUserRole = (UserRole)Session["UserRole"];
                if ((bool)_CurrentUserRole.IsAdvancedSecurity)
                {
                    string[] strSTs = Session["STs"].ToString().Split(',');
                    foreach (string strST in strSTs)
                    {
                        if (strST == drSubMenu["TableID"].ToString())
                        {
                            bool bHasHideByShowMenu = false;
                            if (strHideByShowWhen != "")
                            {
                                string[] strSTh = strHideByShowWhen.Split(',');
                                foreach (string eachSTh in strSTh)
                                {
                                    if (eachSTh == drSubMenu["TableID"].ToString())
                                    {
                                        bHasHideByShowMenu = true;
                                        break;
                                    }
                                }
                            }

                            if (bHasHideByShowMenu==false)
                                menuParent.ChildItems.Add(miTempChild);


                            break;
                        }
                    }

                }
                else
                {
                    if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
                    {
                      
                        menuParent.ChildItems.Add(miTempChild);
                    }
                    else
                    {
                        menuParent.ChildItems.Add(miTempChild);
                    }
                }
            }
            else if (drSubMenu["DocumentTypeID"] != DBNull.Value)
            {
                string strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?Category=" + Cryptography.Encrypt(drSubMenu["DocumentTypeID"].ToString()) + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                miTempChild.NavigateUrl = strURL;
                menuParent.ChildItems.Add(miTempChild);
            }
            else if (drSubMenu["MenuType"] != DBNull.Value && drSubMenu["MenuType"].ToString() == "doc")
            {
                string strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");

                miTempChild.NavigateUrl = strURL;
                menuParent.ChildItems.Add(miTempChild);

            }
            else if (drSubMenu["DocumentID"] != DBNull.Value)
            {
                //
                string strURL = "#";
                Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(drSubMenu["DocumentID"].ToString()));
                if (theDocument != null)
                {

                    if (theDocument.ReportType == "ssrs")
                    {
                        strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/SSRS.aspx?DocumentID=" + Cryptography.Encrypt(theDocument.DocumentID.ToString()) + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SSearchCriteriaID=" + Cryptography.Encrypt("-1");

                    }
                    else if (theDocument.DocumentTypeID != null)
                    {

                        strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/View.aspx?DocumentID=" + theDocument.DocumentID.ToString() + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SSearchCriteriaID=" + Cryptography.Encrypt("-1");
                    }

                }
                miTempChild.NavigateUrl = strURL;
                menuParent.ChildItems.Add(miTempChild);
            }         
            else if (drSubMenu["ExternalPageLink"] != DBNull.Value)
            {
                string strExternalPageLink = drSubMenu["ExternalPageLink"].ToString();
                if (!string.IsNullOrEmpty(strExternalPageLink))
                {
                    miTempChild.NavigateUrl = strExternalPageLink;
                }
                menuParent.ChildItems.Add(miTempChild);
            }
            else
            {
                menuParent.ChildItems.Add(miTempChild);
                
            }
            PopulateSubMenu(ref menuRoot, ref miTempChild, int.Parse(drSubMenu["MenuID"].ToString()));
        }
    }

      protected void BindMenuProfile()
    {
        menuProfile.Items.Clear();
        string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;

        //User _objUser = (User)Session["User"];

        if (_objUser != null && _objUser.UserID > 0)
        {
            MenuItem miUserName = new MenuItem();
            miUserName.Text = _objUser.FirstName + " " + _objUser.LastName;
            //miUserName.NavigateUrl = "~/Default.aspx";
            miUserName.Value = "UserName";
            menuProfile.Items.Add(miUserName);
            //if (!Common.HaveAccess(Session["roletype"].ToString(), Common.UserRoleType.ReadOnly))
            //{

                if (_theAccount.HideMyAccount != null && (bool)_theAccount.HideMyAccount)
                {

                }
                else
                {
                    //if (!Common.HaveAccess(Session["roletype"].ToString(), Common.UserRoleType.OwnData))
                    //{
                        MenuItem miMyAccount = new MenuItem();
                        miMyAccount.Text = "My Account";
                        miMyAccount.Value = "UserName";
                        miMyAccount.NavigateUrl = "~/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(Session["AccountID"].ToString());
                        miUserName.ChildItems.Add(miMyAccount);
                    //}
                }
               

            //}



            string strHideLinkMenu = SystemData.SystemOption_ValueByKey_Account("Hide Link Account Menu", int.Parse(Session["AccountID"].ToString()), null);

            if(strHideLinkMenu!="" & strHideLinkMenu.ToLower()=="yes")
            {
                //avoid the menu
            }
            else
            {
                MenuItem miLinkToAnotherAccount = new MenuItem();
                miLinkToAnotherAccount.Text = "Link to another Account";
                miLinkToAnotherAccount.Value = "UserName";
                miLinkToAnotherAccount.NavigateUrl = "~/Pages/User/AddAccount.aspx?menu=yes&UserID=" + _objUser.UserID.ToString();
                miUserName.ChildItems.Add(miLinkToAnotherAccount);
            }
            


            //MenuItem miChangePassword = new MenuItem();
            //miChangePassword.Text = "Change Password";
            //miChangePassword.Value = "UserName";
            //miChangePassword.NavigateUrl = "~/Security/ChangePassword.aspx";
            //miUserName.ChildItems.Add(miChangePassword);


            MenuItem miSignOut = new MenuItem();
            miSignOut.Text = "Sign Out";
            miSignOut.Value = "UserName";
            //miSignOut.NavigateUrl = "javascript:__doPostBack('ctl00$lkLogout','')";
            miSignOut.NavigateUrl = "~/Login.aspx?Logout=Yes";
            miUserName.ChildItems.Add(miSignOut);

            //menuProfile.Attributes.Add("onclick", "alert('hello!')");
        }
    }


      protected void BindMenuReport()
      {

          //hlReport.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
          //hlDocuments.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                   
      }




      protected void BindAccountMenu()
      {
          imgHouse.Visible = false;
          menuAccount.Items.Clear();
          string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;

          //User _objUser = (User)Session["User"];
          int? iAccountID = SecurityManager.GetPrimaryAccountID((int)_objUser.UserID);
          if (_objUser != null && _objUser.UserID > 0)
          {


              DataTable dtTemp = Common.DataTableFromText(@"SELECT DISTINCT UserRole.AccountID,  AccountName FROM 
                    UserRole INNER JOIN Account ON UserRole.AccountID=Account.AccountID
                    WHERE UserID=" + _objUser.UserID.ToString() + " ORDER BY AccountName");

              if (dtTemp.Rows.Count > 1)
              {
                  menuAccount.Visible = true;
                  imgHouse.Visible = true;

                  int i = 0;


                  MenuItem miAccounts = new MenuItem();
                  miAccounts.Text = "Accounts";
                  miAccounts.NavigateUrl = "~/Default.aspx";
                  miAccounts.Value = "Account";
                  menuAccount.Items.Add(miAccounts);
                  foreach (DataRow dr in dtTemp.Rows)
                  {
                                         
                      
                          MenuItem miTempChild = new MenuItem();
                          miTempChild.Text = dr["AccountName"].ToString();
                          miTempChild.NavigateUrl = "javascript:document.getElementById('hfAccountIDToChangeAccount').value = '" + dr["AccountID"].ToString() + "';__doPostBack('ctl00$lkChangeAccount','')"; ;
                          miAccounts.ChildItems.Add(miTempChild);
                      
                      i = i + 1;
                  }

                  Account theAccount = SecurityManager.Account_Details((int)iAccountID);

                  //if (theAccount != null)
                  //{
                  //    MenuItem miTempChild = new MenuItem();
                  //    miTempChild.Text = theAccount.AccountName;
                  //    miTempChild.NavigateUrl = "javascript:document.getElementById('hfAccountIDToChangeAccount').value = '" + theAccount.AccountID + "';__doPostBack('ctl00$lkChangeAccount','')"; ;
                  //    miAccounts.ChildItems.Add(miTempChild);
                  //}

                  Account theAccountRoot = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

                  if (theAccountRoot != null)
                  {
                      miAccounts.Text = theAccountRoot.AccountName;
                  }

              }
            


          }
      }


      protected void BindOpenMenu()
      {
          menuOpen.Items.Clear();
          if (!Common.HaveAccess(Session["roletype"].ToString(), Common.UserRoleType.OwnData))
          {
              
              
              //MenuItem miOpen = new MenuItem();
              //miOpen.Text = "Open";
              //miOpen.NavigateUrl = "~/Default.aspx";
              //menuOpen.Items.Add(miOpen);

              MenuItem miOpen = new MenuItem();
              miOpen.Text = "Documents";
              miOpen.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
              menuOpen.Items.Add(miOpen);

              //MenuItem miDocument = new MenuItem();
              //miDocument.Text = "Documents";
              //miDocument.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
              //miOpen.ChildItems.Add(miDocument);

              

             
          }
          else
          {
              menuOpen.Visible = false;
              imgMenuOpen.Visible = false;
          }

      }
    public void BindMenu()
    {
        menuETS.Items.Clear();
        string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;

         //int iTN=0;
         //int iTempCount = 0;
         //User _objUser = (User)Session["User"];

         //_objUser = SecurityManager.User_Details((int)_objUser.UserID);
         //Session["User"] = _objUser;

        UserRole theUserRole = SecurityManager.GetUserRole((int)_objUser.UserID, int.Parse(Session["AccountID"].ToString()));
        if (theUserRole == null)
        {
            if(Session["roletype"].ToString()=="1")
            {
                theUserRole = SecurityManager.GetGlobalUserRole((int)_objUser.UserID);
            }

        }
        Session["UserRole"] = theUserRole;

        if (_objUser.Email.ToLower() == SystemData.SystemOption_ValueByKey_Account("DemoAccountCreator",null,null).ToLower())
         {
             _bDemoUser = true;
         }

        string strHideByShowWhen = "";
         if ((bool)theUserRole.IsAdvancedSecurity)
         {
             //Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)_objUser.UserID, "-1,3,4,5,7,8,9");
             Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)theUserRole.RoleID);

             strHideByShowWhen = RecordManager.ets_Table_Hide_ByShowMenu((int)theUserRole.RoleID);
         }
         else
         {
             Session["STs"] = "";
            

//             if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
//             {
//                 //
//                 DataTable dtTables = Common.DataTableFromText(@"SELECT DISTINCT Record.TableID FROM Record INNER JOIN [Table] 
//                    ON Record.TableID=[Table].TableID WHERE OwnerUserID=" + _objUser.UserID.ToString() + " OR Record.EnteredBy=" + _objUser.UserID.ToString());

//                 string strTableIDs = "-1,";
//                 foreach (DataRow dr in dtTables.Rows)
//                 {
//                     strTableIDs = strTableIDs + dr[0].ToString() + ",";
//                 }

//                 strTableIDs = strTableIDs.Substring(0, strTableIDs.Length - 1);
//                 Session["STs"] = strTableIDs;
//             }


             if (Common.HaveAccess(Session["roletype"].ToString(),Common.UserRoleType.None))
             {
                 Session["STs"] = "-1";
             }
         }



         if (_objUser != null && _objUser.UserID > 0)
         {

             //

             Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
                                     
             //populate accountwise things

             

             if (theAccount.Logo != null)
             {
                 if ((bool)theAccount.UseDefaultLogo==false)
                    imgLogo.ImageUrl = "~/SSPhoto.ashx?AccountID=" + Session["AccountID"].ToString() + "&type=o";
             }
             
             //populate dynamic [Table] Group 
             List<Menu> listSTG = RecordManager.ets_Menu_List(int.Parse(Session["AccountID"].ToString()));


             if (!Common.HaveAccess(Session["roletype"].ToString(), Common.UserRoleType.OwnData))
             {
                 
                 if (theAccount.HomeMenuCaption.Trim() != "")
                 {                    

                     MenuItem miDefault = new MenuItem();
                     miDefault.Text = theAccount.HomeMenuCaption;
                     miDefault.NavigateUrl = "~/Default.aspx";

                     if (theAccount.DisplayTableID != null)
                     {
                         miDefault.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" +
                                Cryptography.Encrypt(theAccount.DisplayTableID.ToString());
                     }

                    
                     menuETS.Items.Add(miDefault);
                 }
                
             }

            

             //foreach (Menu item in listSTG)
             //{
             //    MenuItem miTemp = new MenuItem();
             //    miTemp.Text = item.MenuP;
             //    List<Table> lstTable = RecordManager.ets_Table_Select(null, "", (int)item.MenuID, null, null, null,true,
             //        "st.DisplayOrder", "ASC", null, null, ref iTN, Session["STs"].ToString());
             //    miTemp.Value = item.MenuP;
             //    int iLevel = 1;
             //    if (iTN > 0)
             //    {
             //        menuETS.Items.Add(miTemp);
             //        foreach (Table tempST in lstTable)
             //        {

             //            MenuItem miTempChild = new MenuItem();
             //            miTempChild.Text = tempST.TableName;
             //            miTempChild.Value = miTemp.Value;
             //            miTempChild.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" +
             //                Cryptography.Encrypt(tempST.TableID.ToString());
             //            miTemp.ChildItems.Add(miTempChild);

             //            if (iLevel==1)
             //            {
             //                miTemp.NavigateUrl = miTempChild.NavigateUrl;

             //            }

             //            iLevel = iLevel + 1;
             //        }
             //    }
             //}


             foreach (Menu item in listSTG)
             {
                 MenuItem miTemp = new MenuItem();
                 miTemp.Text = item.MenuP;
                 miTemp.Value = item.MenuP;

                 if (item.OpenInNewWindow != null && (bool)item.OpenInNewWindow)
                 {
                     miTemp.Target = "_blank";
                 }

                 bool bOkToAdd = true;

                 if (!string.IsNullOrEmpty(item.ExternalPageLink))
                 {
                     miTemp.NavigateUrl = item.ExternalPageLink;
                 }
                 if(item.DocumentID!=null)
                 {
                     string strURL = "#";
                     Document theDocument = DocumentManager.ets_Document_Detail((int)item.DocumentID);
                     if (theDocument != null)
                     {

                         if (theDocument.ReportType == "ssrs")
                         {
                             strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/SSRS.aspx?DocumentID=" + Cryptography.Encrypt(theDocument.DocumentID.ToString()) + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SSearchCriteriaID=" + Cryptography.Encrypt("-1");

                         }
                         else if (theDocument.DocumentTypeID != null)
                         {

                             strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/View.aspx?DocumentID=" + theDocument.DocumentID.ToString() + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SSearchCriteriaID=" + Cryptography.Encrypt("-1");
                         }

                     }
                     miTemp.NavigateUrl = strURL;

                 }
                 if (item.DocumentTypeID != null)
                 {
                     string strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?Category=" + Cryptography.Encrypt(item.DocumentTypeID.ToString()) + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                     
                     miTemp.NavigateUrl = strURL;

                 }

                 if (item.MenuType != "" && item.MenuType=="doc")
                 {
                     string strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");

                     miTemp.NavigateUrl = strURL;

                 }

                 if(item.TableID!=null)
                 {
                     bool bFound = false;
                     miTemp.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" +
                         Cryptography.Encrypt(item.TableID.ToString());                   
                     if ((bool)theUserRole.IsAdvancedSecurity)
                     {
                         string[] strSTs = Session["STs"].ToString().Split(',');
                         foreach (string strST in strSTs)
                         {
                             if (strST == item.TableID.ToString())
                             {
                                 bFound = true;

                                 if (strHideByShowWhen!="")
                                 {
                                     string[] strSTh = strHideByShowWhen.Split(',');
                                     foreach (string eachSTh in strSTh)
                                     {
                                         if (eachSTh == item.TableID.ToString())
                                          {
                                              bFound = false;
                                              break;
                                          }
                                     }
                                 }


                                 break;
                             }
                         }

                     }
                     else
                     {
                         bFound = true;
                     }
                    if(bFound==false)
                    {
                        bOkToAdd = false;
                    }
                 }

                 if (bOkToAdd)
                 {
                     PopulateSubMenu(ref miTemp, ref miTemp, (int)item.MenuID);

                     if (miTemp.ChildItems.Count > 0 || !string.IsNullOrEmpty(item.ExternalPageLink)
                         || item.TableID != null || item.DocumentID != null || item.MenuType!="" || item.DocumentTypeID!=null)
                     {
                         menuETS.Items.Add(miTemp);
                     }
                 }
                
             }


             if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
             {
                 if (theAccount.IsReportTopMenu != null)
                 {
                     if ((bool)theAccount.IsReportTopMenu)
                     {
                         MenuItem miTopReports = new MenuItem();
                         miTopReports.Text = "Reports";
                         miTopReports.Value = "Reports";
                         miTopReports.Selectable = true;
                         miTopReports.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                         menuETS.Items.Add(miTopReports);
                     }
                 }
             }

             bool bHideAdminMenusExceptUsers = false;
             if (SystemData.SystemOption_ValueByKey_Account("HideAdminMenusExceptUsers",null,null).ToLower() == "yes"
                 && Common.HaveAccess(Session["roletype"].ToString(), "1")==false)
             {
                 bHideAdminMenusExceptUsers = true;
             }

             if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
             {

                 MenuItem miAdmin = new MenuItem();
                 miAdmin.Text = "Admin";
                 miAdmin.Value = "Admin";
                 miAdmin.Selectable = true;
                 miAdmin.NavigateUrl = "~/Pages/User/List.aspx";
                 menuETS.Items.Add(miAdmin);

                 if (_bIsAccountHolder || _bGod)
                 {
                     MenuItem miAudit = new MenuItem();
                     miAudit.Text = "Audit";
                     miAudit.Value = "Admin";
                     miAudit.NavigateUrl = "~/Pages/Document/AuditReport.aspx";
                     miAdmin.ChildItems.Add(miAudit);
                 }
                

                 if (bHideAdminMenusExceptUsers == false)
                 {
                     if (_bIsAccountHolder || _bGod)
                     {
                         MenuItem miUploads = new MenuItem();
                         miUploads.Text = "Auto Uploads";
                         miUploads.Value = "Admin";
                         miUploads.NavigateUrl = "~/Pages/Record/Upload.aspx";
                         miAdmin.ChildItems.Add(miUploads);
                     }
                 }

                 if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                 {
                     MenuItem miAccounts = new MenuItem();
                     miAccounts.Text = "Accounts";
                     miAccounts.Value = "Admin";
                     miAccounts.NavigateUrl = "~/Pages/Security/AccountList.aspx";
                     miAdmin.ChildItems.Add(miAccounts);

                 }
                 else
                 {
                     //User objUser = (User)Session["User"];

                     if (_bDemoUser)
                     {
                         MenuItem miAccounts = new MenuItem();
                         miAccounts.Text = "Add Account";
                         miAccounts.Value = "Admin";
                         miAccounts.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/SystemSignUp.aspx";
                         miAdmin.ChildItems.Add(miAccounts);
                     }

                 }


                 if (_bDemoUser == false)
                 {
                     //MenuItem miAudit = new MenuItem();
                     //miAudit.Text = "Audit";
                     //miAudit.Value = "Admin";
                     //miAudit.NavigateUrl = "~/Pages/Document/AuditReport.aspx";
                     //miAdmin.ChildItems.Add(miAudit);

                     if (bHideAdminMenusExceptUsers == false && Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                     {
                         string strOptionValue = SystemData.SystemOption_ValueByKey_Account("Show_Admin_Menu_Batches", int.Parse(Session["AccountID"].ToString()), null);
                         if (_bIsAccountHolder || _bGod || (strOptionValue != "" && strOptionValue.ToLower() == "yes"))
                         {
                             MenuItem miBatches = new MenuItem();
                             miBatches.Text = "Batches";
                             miBatches.Value = "Admin";
                             miBatches.NavigateUrl = "~/Pages/Record/Batches.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt("-1");
                             miAdmin.ChildItems.Add(miBatches);

                         }
                     }

                    
                     if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                     {


                        

                         MenuItem miConfiguration = new MenuItem();
                         miConfiguration.Text = "Configuration";
                         miConfiguration.Value = "Admin";
                         //miConfiguration.NavigateUrl = "~/Pages/Company/ContactUsAdmin.aspx";
                         miConfiguration.Selectable = false;
                         miAdmin.ChildItems.Add(miConfiguration);


                         MenuItem miCopyFields = new MenuItem();
                         miCopyFields.Text = "Copy Field";
                         miCopyFields.Value = "Admin";
                         miCopyFields.NavigateUrl = "~/Pages/SystemData/CopyRecordField.aspx";
                         miConfiguration.ChildItems.Add(miCopyFields);

                         MenuItem miResetValues = new MenuItem();
                         miResetValues.Text = "Reset Values";
                         miResetValues.Value = "Admin";
                         miResetValues.NavigateUrl = "~/Pages/SystemData/ResetValues.aspx";
                         miConfiguration.ChildItems.Add(miResetValues);

                         MenuItem miUpdateLinkedTables = new MenuItem();
                         miUpdateLinkedTables.Text = "Update Linked Tables";
                         miUpdateLinkedTables.Value = "Admin";
                         miUpdateLinkedTables.NavigateUrl = "~/Pages/SystemData/UpdateLinkedTables.aspx";
                         miConfiguration.ChildItems.Add(miUpdateLinkedTables);

                         //MenuItem miWorkFlow = new MenuItem();
                         //miWorkFlow.Text = "WorkFlow";
                         //miWorkFlow.Value = "Admin";
                         //miWorkFlow.NavigateUrl = "~/Pages/WorkFlow/WorkFlow.aspx";
                         //miConfiguration.ChildItems.Add(miWorkFlow);

                        


                         MenuItem miContactAdmin = new MenuItem();
                         miContactAdmin.Text = "Contacts";
                         miContactAdmin.Value = "Admin";
                         miContactAdmin.NavigateUrl = "~/Pages/Company/ContactUsAdmin.aspx";
                         miAdmin.ChildItems.Add(miContactAdmin);

                         MenuItem miContents = new MenuItem();
                         miContents.Text = "Contents";
                         miContents.Value = "Admin";
                         miContents.NavigateUrl = "~/Pages/SystemData/Content.aspx";
                         miAdmin.ChildItems.Add(miContents);

                        

                     }

                 }

                 if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                 {

                     if (bHideAdminMenusExceptUsers == false)
                     {
                       

                         if (_bIsAccountHolder || _bGod)
                         {
                             MenuItem miDashboard = new MenuItem();
                             miDashboard.Text = "Dashboards";
                             miDashboard.Value = "Admin";
                             miDashboard.NavigateUrl = "~/Pages/Home/DashBoard.aspx";
                             miAdmin.ChildItems.Add(miDashboard);
                         }

                         MenuItem miGraph = new MenuItem();
                         miGraph.Text = "Graphs";
                         miGraph.Value = "Admin";
                         miGraph.NavigateUrl = "~/Pages/Graph/GraphOptions.aspx";
                         miAdmin.ChildItems.Add(miGraph);
                         if (_bIsAccountHolder || _bGod)
                         {
                             MenuItem miGraphDef = new MenuItem();
                             miGraphDef.Text = "Graph Definitions";
                             miGraphDef.Value = "Admin";
                             miGraphDef.NavigateUrl = "~/Pages/Graph/GraphDef.aspx";
                             miAdmin.ChildItems.Add(miGraphDef);
                         }
                     }
                    
                 }

                

                 if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                 {
                     MenuItem miErrorLogs = new MenuItem();
                     miErrorLogs.Text = "Error Logs";
                     miErrorLogs.Value = "Admin";
                     miErrorLogs.NavigateUrl = "~/Pages/SystemData/ErrorLog.aspx";
                     miAdmin.ChildItems.Add(miErrorLogs);


                     MenuItem miLookup = new MenuItem();
                     miLookup.Text = "Look up data";
                     miLookup.Value = "Admin";
                     miLookup.NavigateUrl = "#";
                     miAdmin.ChildItems.Add(miLookup);




                     MenuItem miCountry = new MenuItem();
                     miCountry.Text = "Country";
                     miCountry.Value = "Admin";
                     miCountry.NavigateUrl = "~/Pages/LookUp/LookUp.aspx?LookUpTypeID=" + Cryptography.Encrypt("-1");
                     miLookup.ChildItems.Add(miCountry);
                 }


                 if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                 {
                     if (bHideAdminMenusExceptUsers == false)
                     {
                         string strOptionValue = SystemData.SystemOption_ValueByKey_Account("Show_Admin_Menu_Menus", int.Parse(Session["AccountID"].ToString()), null);
                         if (_bIsAccountHolder || _bGod || (strOptionValue != "" && strOptionValue.ToLower()=="yes"))
                         {
                             MenuItem miMenus = new MenuItem();
                             miMenus.Text = "Menus";
                             miMenus.Value = "Admin";
                             miMenus.NavigateUrl = "~/Pages/Record/TableGroup.aspx";
                             miAdmin.ChildItems.Add(miMenus);
                         }                        
                     }

                     MenuItem miMessages = new MenuItem();
                     miMessages.Text = "Messages";
                     miMessages.Value = "Admin";
                     miMessages.NavigateUrl = "~/Pages/Record/MessageList.aspx";
                     miAdmin.ChildItems.Add(miMessages);
                 }


                 if (_bDemoUser == false)
                 {
                     if (bHideAdminMenusExceptUsers == false && Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                     {
                         string strOptionValue = SystemData.SystemOption_ValueByKey_Account("Show_Admin_Menu_Notification", int.Parse(Session["AccountID"].ToString()), null);
                         if (_bIsAccountHolder || _bGod || (strOptionValue != "" && strOptionValue.ToLower() == "yes"))
                         {
                             MenuItem miNotifications = new MenuItem();
                             miNotifications.Text = "Notifications";
                             miNotifications.Value = "Admin";
                             miNotifications.NavigateUrl = "~/Pages/Record/Notification.aspx";
                             miAdmin.ChildItems.Add(miNotifications);
                         }
                     }
                 }

                 if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                 {
                   
                     MenuItem miInvoices = new MenuItem();
                     miInvoices.Text = "Invoices";
                     miInvoices.Value = "Admin";
                     miInvoices.NavigateUrl = "~/Pages/Security/Invoice.aspx";
                     miAdmin.ChildItems.Add(miInvoices);


                    

                 }

                 if (_bDemoUser == false)
                 {
                     if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                     {
                         if (bHideAdminMenusExceptUsers == false)
                         {
                             if (_bIsAccountHolder || _bGod)
                             {
                                 MenuItem miReports = new MenuItem();
                                 miReports.Text = "Reports";
                                 miReports.Value = "Admin";
                                 miReports.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                                 //miReports.Selectable = false;
                                 miAdmin.ChildItems.Add(miReports);
                             }
                          
                         }
                     }
                     

                     if (bHideAdminMenusExceptUsers == false && Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                     {
                         string strOptionValue = SystemData.SystemOption_ValueByKey_Account("Show_Admin_Menu_Tables", int.Parse(Session["AccountID"].ToString()), null);
                         if (_bIsAccountHolder || _bGod || (strOptionValue != "" && strOptionValue.ToLower() == "yes"))
                          {
                              MenuItem miST = new MenuItem();
                              miST.Text = SecurityManager.etsTerminology("", "Tables", "Tables");
                              miST.Value = "Admin";
                              miST.NavigateUrl = "~/Pages/Record/TableList.aspx";
                              miAdmin.ChildItems.Add(miST);
                          }                      
                     }

                    

                     if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                     {

                         MenuItem miSystemOptions = new MenuItem();
                         miSystemOptions.Text = "System Options";
                         miSystemOptions.Value = "Admin";
                         miSystemOptions.NavigateUrl = "~/Pages/SystemData/SystemOption.aspx";
                         miAdmin.ChildItems.Add(miSystemOptions);
                     }



                     if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                     {
                         if (bHideAdminMenusExceptUsers == false)
                         {
                             if (_bIsAccountHolder || _bGod)
                             {
                                 MenuItem miTerminology = new MenuItem();
                                 miTerminology.Text = "Terminology";
                                 miTerminology.Value = "Admin";
                                 miTerminology.NavigateUrl = "~/Pages/Security/Terminology.aspx";
                                 miAdmin.ChildItems.Add(miTerminology);
                             }
                         }
                     


                     }


                     MenuItem miUsers = new MenuItem();
                     miUsers.Text = "Users";
                     miUsers.Value = "Admin";
                     miUsers.NavigateUrl = "~/Pages/User/List.aspx";
                     miAdmin.ChildItems.Add(miUsers);

                     if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                     {

                        

                         MenuItem miVisitorPageCount = new MenuItem();
                         miVisitorPageCount.Text = "Visitors";
                         miVisitorPageCount.Value = "Admin";
                         miVisitorPageCount.NavigateUrl = "~/Pages/SystemData/PageCount.aspx";
                         miAdmin.ChildItems.Add(miVisitorPageCount);


                        

                         MenuItem miForm = new MenuItem();
                         miForm.Text = "Form";
                         miForm.Value = "Admin";
                         miForm.NavigateUrl = "~/Pages/Form/Form.aspx";
                         miAdmin.ChildItems.Add(miForm);

                     }

                    
                    
                 }

             }





         }
         else
         {
             if (Session["LoginAccount"] == null)
             {
                 Session.Clear();
                 FormsAuthentication.SignOut();
                 Response.Redirect("~/Login.aspx", false);
             }
             else
             {
                 string strLoginAccount = Session["LoginAccount"].ToString();
                 Session.Clear();
                 FormsAuthentication.SignOut();
                 Response.Redirect("~/Login.aspx?" + strLoginAccount, false);
             }

         }
         
    }
    public bool DisplayAdmin()
    {
        int iTN = 0;
        bool kq = false;
        if (Context.User.Identity.IsAuthenticated)
        {
          
            string roletype = Session["roletype"].ToString();
            if (roletype.Length > 0)
            {
                if (roletype.Contains("2"))
                {
                    kq = true;
                }
            }
           
        }
        return kq;
    }
    public bool DisplayGod()
    {
        bool kq = false;
    
        if (Context.User.Identity.IsAuthenticated)
        {
            string roletype = Session["roletype"].ToString();
            if (roletype.Length > 0)
            {
                if (roletype.Contains("2"))
                {
                    kq = true;
                }
            }
            
        }
        return kq;
    }
    protected void lkChangeAccount_Click(object sender, EventArgs e)
    {
        if (hfAccountIDToChangeAccount.Value != "")
        {
            //Session["AccountID"] = hfAccountIDToChangeAccount.Value;

            //string roletype = "";
            //roletype = SecurityManager.GetUserRoleTypeID((int)_objUser.UserID, int.Parse(Session["AccountID"].ToString()));



            //Session["roletype"] = roletype;

            //UserRole theUserRole = SecurityManager.GetUserRole((int)_objUser.UserID, int.Parse(Session["AccountID"].ToString()));

            //Session["UserRole"] = theUserRole;




            //try
            //{
            //    Session["DoNotAllow"] = null;
             
            //    Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));






            //    if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())))
            //    {
            //        Session["DoNotAllow"] = "true";
                  
            //    }


            //}
            //catch
            //{
            //    //
            //}

         
            try
            {
                if (Common.ChangeAccount((int)_objUser.UserID, int.Parse(hfAccountIDToChangeAccount.Value)))
                {
                    Response.Redirect("~/Default.aspx", true);
                    return;
                }
                else
                {
                    Response.Redirect("~/Empty.aspx", true);
                }
             
            }
            catch
            {
                //
            }         

        }

    }
    //Session["AccountID"] = hfAccountIDToChangeAccount.Value;

    //string roletype = "";
    //roletype = SecurityManager.GetUserRoleTypeID((int)_objUser.UserID,int.Parse( Session["AccountID"].ToString()));



    //Session["roletype"] = roletype;

    //UserRole theUserRole = SecurityManager.GetUserRole((int)_objUser.UserID, int.Parse(Session["AccountID"].ToString()));

    //Session["UserRole"] = theUserRole;




    //try
    //{
    //    Session["DoNotAllow"] = null;
    //    //Session["ExpireLeftDay"] = null;
    //    Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));



    //    //if (theAccount.ExpiryDate != null)
    //    //{
    //    //    if (theAccount.ExpiryDate.Value.AddDays(0) < DateTime.Today)
    //    //    {
    //    //        Session["DoNotAllow"] = "true";

    //    //    }
    //    //}




    //    if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())))
    //    {
    //        Session["DoNotAllow"] = "true";
    //        //Response.Redirect("~/Pages/Security/AccountTypeChange.aspx?type=renew", false);
    //        //return;
    //    }


    //}
    //catch
    //{
    //    //
    //}

    ////Page_Init(null, null);
    ////Page_Load(null, null);
    //Response.Redirect("~/Default.aspx", false);
    protected void lnkEndDemo_Click(object sender, EventArgs e)
    {

        Session["User"] = null;
        Session.Abandon();

        FormsAuthentication.SignOut();
        HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
        oUseInfor.Expires = DateTime.Now.AddDays(-3d);
        Response.Cookies.Add(oUseInfor);
        //Response.Redirect("~/Environmental-Monitoring-Management-Database-System.aspx?demo=yes", false);
        Response.Redirect("~/Login.aspx?demo=yes", false);

    }
    protected void lkLogout_Click(object sender, EventArgs e)
    {

       try
       {
           string strLoginAccount = "";
           if (Session["LoginAccount"] != null)
           {
               strLoginAccount = Session["LoginAccount"].ToString();
           }


           Session["User"] = null;
           Session.Abandon();

           FormsAuthentication.SignOut();
           HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
           oUseInfor.Expires = DateTime.Now.AddDays(-3d);
           Response.Cookies.Add(oUseInfor);




           if (Session["client"] != null)
           {
               Session.Clear();
               Response.Redirect("~/Login.aspx?Logout=yes&Account=" + Session["client"].ToString(), false);
               
               return;
           }

           if (Session["LoginAccount"] == null)
           {
               Session.Clear();
               Response.Redirect("~/Login.aspx?Logout=yes", true);
           }
           else
           {
               Session.Clear();
               Response.Redirect("~/Login.aspx?Logout=yes&" + strLoginAccount, true);
           }
           //}

       }
        catch
       {
            //
       }
        
       
       
    }




    protected void lnkChangeAccount_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Security/AccountList.aspx",false);
    }

    protected void lnkViewAccount_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(Session["AccountID"].ToString()), false);
    }

    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Security/ChangePassword.aspx", false);
    }
    //protected void menuETS_MenuItemDataBound(object sender, MenuEventArgs e)
    //{
    //    if (e.Item.Selected == true)
    //    {
    //        e.Item.Selected = true;
    //        e.Item.Parent.Selectable = true;
    //        e.Item.Parent.Selected = true;
    //    }

    //}
    //protected void menuETS_MenuItemClick(object sender, MenuEventArgs e)
    //{
    //    if (e.Item.Selected == true)
    //    {
    //        e.Item.Selected = true;
    //        e.Item.Parent.Selectable = true;
    //        e.Item.Parent.Selected = true;
    //    }
    //}



    MenuItem GetSelectedMenuItem(System.Web.UI.WebControls.Menu menu)
    {
        MenuItem selectedItem = null;
        if (Request.Form["__EVENTTARGET"] == menu.ID)
        {
            string value = Request.Form["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(value))
            {
                foreach (MenuItem mi in menu.Items)
                {
                    if (mi.Value == value)
                    {
                        selectedItem = mi;
                        break;
                    }
                }
            }
        }
        else
        {
            selectedItem = menu.SelectedItem;
        }
        return selectedItem;
    }


    protected void menuProfile_MenuItemClick(object sender, MenuEventArgs e)
    {
        //show here

    }
}

