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


public partial class RRP : System.Web.UI.MasterPage
{

    bool _bDemoUser = false;
    Account _theAccount;
    User _objUser;
    bool _bIsAccountHolder = false;
    UserRole _CurrentUserRole = null;

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
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.QueryString["public"] != null)
        {
            return;
        }

        _objUser = (User)Session["User"];
        _CurrentUserRole = (UserRole)Session["UserRole"];

        if (_CurrentUserRole.IsAccountHolder != null && (bool)_CurrentUserRole.IsAccountHolder)
        {
            _bIsAccountHolder = true;
            hfIsAccountHolder.Value = "yes";
        }
        _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

        if (Request.RawUrl.IndexOf("Record/RecordList.aspx") > -1
            || Request.RawUrl.IndexOf("Record/RecordDetail.aspx") > -1)
        {
            if (Request.QueryString["TableID"] != null)
            {
                Table theTable = RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())));

                if (theTable != null)
                {
                    if (theTable.HeaderColor != "")
                    {
                        divHeaderColor.Style.Add("background", "#" + theTable.HeaderColor);
                    }
                    else
                    {
                        divHeaderColor.Style.Add("background", "#0089a5");
                    }
                }
            }
        }
        else
        {
            divHeaderColor.Style.Add("background", "#0089a5" );
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


            if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
            {
                menuETS.Visible = false;
            }

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
                                        if (Request.RawUrl.IndexOf(ccitem.NavigateUrl.Replace("~", "")) > -1)
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
                    Request.RawUrl.IndexOf("/Security/ChangePassword.aspx") > -1)
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
            if (Session["LoginAccount"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Response.Redirect("~/Login.aspx?" + Session["LoginAccount"].ToString());
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

        if (!Page.IsPostBack)
        {
            //hlRenewNow.NavigateUrl = SystemData.SystemOption_ValueByKey("ContactUsRenewal");
           

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

                                                            var close = document.getElementById('aNotificationMessageClose');


                                                            if (close != null) {
                                                                close.addEventListener('click', function () {
                                                                    var note = document.getElementById('divNotificationMessage');
                                                                    note.style.display = 'none';
                                                                }, false);
                                                            }

                                                            function HidedivNotificationMessage() {
                                                                $('#divNotificationMessage').fadeOut();
                                                                var note = document.getElementById('divNotificationMessage');
                                                                if (note != null)
                                                                    note.style.display = 'none';
                                                            }


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

//            string strHidedivNotificationMessage = @"                                                     
//                                                  $(document).ready(function () {
//
//                                                      try
//                                                        {
//                                                            window.setTimeout(HidedivNotificationMessage," + strTopMsgNoOfMS + @");
//                                                        }
//                                                      catch(err)
//                                                        {
//                                                            //
//                                                        }
//     
//                                                            //$('#divNotificationMessage').fadeIn(1000).delay(" + strTopMsgNoOfMS + @").fadeOut(1000);
//                                                           //window.setTimeout(HidedivNotificationMessage," + strTopMsgNoOfMS + @");
//                                                           // setTimeout(function () { HidedivNotificationMessage(); }," + strTopMsgNoOfMS + @");
//                                                       });
//                                                ";
//            ScriptManager.RegisterStartupScript(this, this.GetType(), "strHidedivNotificationMessage", strHidedivNotificationMessage, true);



            //if (Session["IsFlashSupported"] == null )
            //{
            string strIsFlashSupportedJS = @"                                                     
                                                   $(document).ready(function () {

                                                        function IsFlashSupported() {
                                                            try
                                                            {
                                                                var hfFlashSupport = document.getElementById('" + hfFlashSupport.ClientID + @"');
                                                                if (swfobject.hasFlashPlayerVersion('1')) {
                                                                    hfFlashSupport.value = 'yes';
                                                                }
                                                                else {
                                                                    hfFlashSupport.value = 'no';
                                                                }
                                                                //alert(hfFlashSupport.value);

                                                                $.ajax({
                                                                    url: '" + ResolveUrl("~/Pages/DocGen/REST/SectionREST.ashx") + @"?type=FlashSupport&hfFlashSupport=' + hfFlashSupport.value,
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
                    trEndDemo.Visible = true;
                }
            }
            //CheckConcurrent();
            CheckDoNotAllow();
        }

        //notifications
        if (!IsPostBack)
        {
            if (Session["tdbmsg"] != null)
            {
                lblNotificationMessage.Text = Session["tdbmsg"].ToString();
                Session["tdbmsg"] = null;
            }
            
            if (lblNotificationMessage.Text != "")
            {
                lblNotificationMessage.Text = lblNotificationMessage.Text + "&nbsp; <a id='aNotificationMessageClose' href='#'>Close</a>";
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

                    if (theAccount != null)
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
                if (Session["DoNotAllow"].ToString() == "true")
                {
                    //Response.Redirect("~/Pages/Security/AccountTypeChange.aspx?type=renew", false);
                    //Response.Redirect("~/Default.aspx", false);
                }

            }

        }

    }


    protected void PopulateSubMenu(ref MenuItem menuRoot, ref MenuItem menuParent, int iParentMenuID)
    {
        DataTable dtSubMenu = Common.DataTableFromText(@"SELECT MenuID,Menu,Menu.TableID,TableName,
        ExternalPageLink,OpenInNewWindow FROM Menu left JOIN [Table] ON
    [Table].TableID=Menu.TableID WHERE Menu.IsActive=1 AND 
                Menu.AccountID=" + Session["AccountID"].ToString() + @" AND ParentMenuID=" + iParentMenuID.ToString()
                + @"  ORDER BY Menu.DisplayOrder");

        //AND (Menu.TableID IS NULL OR Menu.TableID IN (" + Session["STs"].ToString() + @"))
        //if ((bool)ObjUser.IsAdvancedSecurity)

        UserRole theUserRole = (UserRole)Session["UserRole"];


        foreach (DataRow drSubMenu in dtSubMenu.Rows)
        {
            MenuItem miTempChild = new MenuItem();
            miTempChild.Text = drSubMenu["Menu"].ToString();
            miTempChild.Value = menuRoot.Value;
            if (drSubMenu["TableID"] != DBNull.Value)
            {
                miTempChild.Text = drSubMenu["TableName"].ToString();
                miTempChild.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" +
                    Cryptography.Encrypt(drSubMenu["TableID"].ToString());
                if ((bool)theUserRole.IsAdvancedSecurity)
                {
                    string[] strSTs = Session["STs"].ToString().Split(',');
                    foreach (string strST in strSTs)
                    {
                        if (strST == drSubMenu["TableID"].ToString())
                        {
                            menuParent.ChildItems.Add(miTempChild);
                            break;
                        }
                    }

                }
                else
                {
                    if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
                    {
                        //string[] strSTs = Session["STs"].ToString().Split(',');
                        //foreach (string strST in strSTs)
                        //{
                        //    if (strST == drSubMenu["TableID"].ToString())
                        //    {
                        //        menuParent.ChildItems.Add(miTempChild);
                        //        break;
                        //    }
                        //}

                        menuParent.ChildItems.Add(miTempChild);
                    }
                    else
                    {
                        menuParent.ChildItems.Add(miTempChild);
                    }
                }
            }
            else
            {
                menuParent.ChildItems.Add(miTempChild);

                PopulateSubMenu(ref menuRoot, ref miTempChild, int.Parse(drSubMenu["MenuID"].ToString()));
            }

        }
    }

    protected void BindMenuProfile()
    {
        menuProfile.Items.Clear();
        string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;

        //User _objUser = (User)Session["User"];

        if (_objUser != null && _objUser.UserID > 0)
        {
            //<asp:Label runat="server" ID="lblProfileName" Text=""></asp:Label> 
            lblProfileName.Text = _objUser.FirstName + " " + _objUser.LastName;

            MenuItem miUserName = new MenuItem();
            miUserName.Text = _objUser.FirstName + " " + _objUser.LastName;
            miUserName.NavigateUrl = "~/Default.aspx";
            miUserName.Value = "UserName";
            menuProfile.Items.Add(miUserName);
            if (!Common.HaveAccess(Session["roletype"].ToString(), Common.UserRoleType.ReadOnly))
            {

                if (_theAccount.HideMyAccount != null && (bool)_theAccount.HideMyAccount)
                {

                }
                else
                {
                    if (!Common.HaveAccess(Session["roletype"].ToString(), Common.UserRoleType.OwnData))
                    {
                        MenuItem miMyAccount = new MenuItem();
                        miMyAccount.Text = "My Account";
                        miMyAccount.Value = "UserName";
                        miMyAccount.NavigateUrl = "~/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(Session["AccountID"].ToString());
                        miUserName.ChildItems.Add(miMyAccount);
                        //hlMyAccount.NavigateUrl = miMyAccount.NavigateUrl;
                    }
                }


            }
            //MenuItem miChangePassword = new MenuItem();
            //miChangePassword.Text = "Change Password";
            //miChangePassword.Value = "UserName";
            //miChangePassword.NavigateUrl = "~/Security/ChangePassword.aspx";
            //miUserName.ChildItems.Add(miChangePassword);


            MenuItem miSignOut = new MenuItem();
            miSignOut.Text = "Sign Out";
            miSignOut.Value = "UserName";
            miSignOut.NavigateUrl = "javascript:__doPostBack('ctl00$lkLogout','')";
            miUserName.ChildItems.Add(miSignOut);


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
            MenuItem miOpen = new MenuItem();
            miOpen.Text = "Open";
            miOpen.NavigateUrl = "~/Default.aspx";
            menuOpen.Items.Add(miOpen);

            MenuItem miDocument = new MenuItem();
            miDocument.Text = "Documents";
            miDocument.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
            miOpen.ChildItems.Add(miDocument);

            MenuItem miReports = new MenuItem();
            miReports.Text = "Reports";
            miReports.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
            miOpen.ChildItems.Add(miReports);

            //MenuItem miCalendar = new MenuItem();
            //miCalendar.Text = "Calendar";
            //miCalendar.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/MonitorSchedules.aspx?SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1");
            //miOpen.ChildItems.Add(miCalendar);
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


        if (_objUser.Email.ToLower() == SystemData.SystemOption_ValueByKey_Account("DemoAccountCreator",null,null).ToLower())
        {
            _bDemoUser = true;
        }

        UserRole theUserRole = SecurityManager.GetUserRole((int)_objUser.UserID, int.Parse(Session["AccountID"].ToString()));

        if (theUserRole == null)
        {
            if (Session["roletype"].ToString() == "1")
            {
                theUserRole = SecurityManager.GetGlobalUserRole((int)_objUser.UserID);
            }

        }
        Session["UserRole"] = theUserRole;

        if ((bool)theUserRole.IsAdvancedSecurity)
        {
            //Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)_objUser.UserID, "-1,3,4,5,7,8,9");
            Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)theUserRole.RoleID);
        }
        else
        {
            Session["STs"] = "";
            //string roletype = "";
            //roletype = SecurityManager.GetUserRoleTypeID((int)ObjUser.UserID, int.Parse(Session["AccountID"].ToString()), null, null);
            //Session["roletype"] = roletype;

//            if (Session["roletype"].ToString() == Common.UserRoleType.OwnData)
//            {
//                //
//                DataTable dtTables = Common.DataTableFromText(@"SELECT DISTINCT Record.TableID FROM Record INNER JOIN [Table] 
//                    ON Record.TableID=[Table].TableID WHERE OwnerUserID=" + _objUser.UserID.ToString());

//                string strTableIDs = "-1,";
//                foreach (DataRow dr in dtTables.Rows)
//                {
//                    strTableIDs = strTableIDs + dr[0].ToString() + ",";
//                }

//                strTableIDs = strTableIDs.Substring(0, strTableIDs.Length - 1);
//                Session["STs"] = strTableIDs;
//            }


            if (Common.HaveAccess(Session["roletype"].ToString(), Common.UserRoleType.None))
            {
                Session["STs"] = "-1";
            }
        }



        if (_objUser != null && _objUser.UserID > 0)
        {

            //

            Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

            //populate accountwise things

            hlMyAccount.NavigateUrl = "~/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(Session["AccountID"].ToString());

            if (theAccount.Logo != null)
            {
                if ((bool)theAccount.UseDefaultLogo == false)
                    imgLogo.ImageUrl = "~/SSPhoto.ashx?AccountID=" + Session["AccountID"].ToString() + "&type=o";
            }


            //POPULATE RRP Specific menus
            //Contractor
            string strContractorTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                    WHERE TableName='Company' AND AccountID=" + theAccount.AccountID.ToString());

            if (strContractorTableID != "")
            {
                hlNewContractor.NavigateUrl = "~/Pages/Record/RecordDetail.aspx?mode=hBe/iopMPf0=&TableID=" + Cryptography.Encrypt(strContractorTableID.ToString()) + "&SearchCriteriaID=VrWdxOe30yE=&stackzero=y";
                hlSearchContractors.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(strContractorTableID.ToString());
                //hlReportContractor.NavigateUrl = "~/Pages/RRP/Riskmatrix.aspx?TableID=" + Cryptography.Encrypt(strContractorTableID.ToString());

            }

            //Risk
            string strRiskTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                    WHERE TableName='Hazard' AND AccountID=" + theAccount.AccountID.ToString());

            if (strRiskTableID != "")
            {
                hlNewRisk.NavigateUrl = "~/Pages/Record/RecordDetail.aspx?mode=hBe/iopMPf0=&TableID=" + Cryptography.Encrypt(strRiskTableID.ToString()) + "&SearchCriteriaID=VrWdxOe30yE=&stackzero=y";
                hlSearchRisks.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(strRiskTableID.ToString());
                //hlReportRisk.NavigateUrl = "~/Pages/RRP/ReportView.aspx?TableID=" + Cryptography.Encrypt(strRiskTableID.ToString());

            }



            //Incident
            string strIncidentTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                    WHERE TableName='Incidents' AND AccountID=" + theAccount.AccountID.ToString());

            if (strIncidentTableID != "")
            {
                hlNewIncident.NavigateUrl = "~/Pages/Record/RecordDetail.aspx?mode=hBe/iopMPf0=&TableID=" + Cryptography.Encrypt(strIncidentTableID.ToString()) + "&SearchCriteriaID=VrWdxOe30yE=&stackzero=y";
                hlSearchIncidents.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(strIncidentTableID.ToString());
                //hlReportIncident.NavigateUrl = "~/Pages/RRP/Riskmatrix.aspx?TableID=" + Cryptography.Encrypt(strIncidentTableID.ToString());

            }


            //Injury
            string strInjuryTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                    WHERE TableName='Injury' AND AccountID=" + theAccount.AccountID.ToString());

            if (strInjuryTableID != "")
            {
                hlNewInjury.NavigateUrl = "~/Pages/Record/RecordDetail.aspx?mode=hBe/iopMPf0=&TableID=" + Cryptography.Encrypt(strInjuryTableID.ToString()) + "&SearchCriteriaID=VrWdxOe30yE=&stackzero=y";
                hlSearchInjuries.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(strInjuryTableID.ToString());
                //hlReportInjury.NavigateUrl = "~/Pages/RRP/Riskmatrix.aspx?TableID=" + Cryptography.Encrypt(strInjuryTableID.ToString());

            }

            if (Request.Url.Authority != "dev.thedatabase.net")
            {
                hlReportRisk.NavigateUrl = "~/Pages/RRP/ReportView.aspx?TableID=iy1ZZ8RpqbM=";
                hlReportContractor.NavigateUrl = "~/Pages/RRP/ReportView.aspx?TableID=A3Ifwv/GWZQ=";
                hlReportIncident.NavigateUrl = "~/Pages/RRP/ReportView.aspx?TableID=cpTzvM1SVJk=";
                hlReportInjury.NavigateUrl = "~/Pages/RRP/ReportView.aspx?TableID=BoPPjx0TNrQ=";
            }
            //end RRP Menu


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

                if (!string.IsNullOrEmpty(item.ExternalPageLink))
                {
                    miTemp.NavigateUrl = item.ExternalPageLink;
                }


                bool bOkToAdd = true;

                if (!string.IsNullOrEmpty(item.ExternalPageLink))
                {
                    miTemp.NavigateUrl = item.ExternalPageLink;
                }
                if (item.DocumentID != null)
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



                if (item.TableID != null)
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
                                break;
                            }
                        }

                    }
                    else
                    {
                        bFound = true;
                    }
                    if (bFound == false)
                    {
                        bOkToAdd = false;
                    }
                }

                if (bOkToAdd)
                {
                    PopulateSubMenu(ref miTemp, ref miTemp, (int)item.MenuID);

                    if (miTemp.ChildItems.Count > 0 || !string.IsNullOrEmpty(item.ExternalPageLink)
                        || item.TableID != null || item.DocumentID != null)
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

            if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            {

                MenuItem miAdmin = new MenuItem();
                miAdmin.Text = "Admin";
                miAdmin.Value = "Admin";
                miAdmin.Selectable = true;
                miAdmin.NavigateUrl = "~/Pages/User/List.aspx";
                menuETS.Items.Add(miAdmin);

                if (_bIsAccountHolder)
                {
                    MenuItem miUploads = new MenuItem();
                    miUploads.Text = "Auto Uploads";
                    miUploads.Value = "Admin";
                    miUploads.NavigateUrl = "~/Pages/Record/Upload.aspx";
                    miAdmin.ChildItems.Add(miUploads);
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



                    MenuItem miBatches = new MenuItem();
                    miBatches.Text = "Batches";
                    miBatches.Value = "Admin";
                    miBatches.NavigateUrl = "~/Pages/Record/Batches.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt("-1");
                    miAdmin.ChildItems.Add(miBatches);

                    if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                    {
                        MenuItem miContactAdmin = new MenuItem();
                        miContactAdmin.Text = "Contacts";
                        miContactAdmin.Value = "Admin";
                        miContactAdmin.NavigateUrl = "~/Pages/Company/ContactUsAdmin.aspx";
                        miAdmin.ChildItems.Add(miContactAdmin);
                    }

                }

                if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                {
                    MenuItem miContents = new MenuItem();
                    miContents.Text = "Contents";
                    miContents.Value = "Admin";
                    miContents.NavigateUrl = "~/Pages/SystemData/Content.aspx";
                    miAdmin.ChildItems.Add(miContents);

                    if (_bIsAccountHolder)
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
                    MenuItem miMenus = new MenuItem();
                    miMenus.Text = "Menus";
                    miMenus.Value = "Admin";
                    miMenus.NavigateUrl = "~/Pages/Record/TableGroup.aspx";
                    miAdmin.ChildItems.Add(miMenus);

                    MenuItem miMessages = new MenuItem();
                    miMessages.Text = "Messages";
                    miMessages.Value = "Admin";
                    miMessages.NavigateUrl = "~/Pages/Record/MessageList.aspx";
                    miAdmin.ChildItems.Add(miMessages);

                }


                if (_bDemoUser == false)
                {
                    if (_bIsAccountHolder)
                    {
                        MenuItem miNotifications = new MenuItem();
                        miNotifications.Text = "Notifications";
                        miNotifications.Value = "Admin";
                        miNotifications.NavigateUrl = "~/Pages/Record/Notification.aspx";
                        miAdmin.ChildItems.Add(miNotifications);
                    }
                }

                if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                {
                    //MenuItem miPayments = new MenuItem();
                    //miPayments.Text = "Payments";
                    //miPayments.Value = "Admin";
                    //miPayments.NavigateUrl = "~/Pages/Security/Payment.aspx";
                    //miAdmin.ChildItems.Add(miPayments);

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

                        MenuItem miReports = new MenuItem();
                        miReports.Text = "Reports";
                        miReports.Value = "Admin";
                        miReports.NavigateUrl = "";
                        miReports.Selectable = false;
                        miAdmin.ChildItems.Add(miReports);


                        MenuItem miHazards = new MenuItem();
                        miHazards.Text = "Open/Closed Hazards";
                        miHazards.Value = "Admin";
                        miHazards.NavigateUrl = "~/Pages/Custom/HazardsReport.aspx";
                        miReports.ChildItems.Add(miHazards);

                        if (_bIsAccountHolder)
                        {
                            MenuItem miAudit = new MenuItem();
                            miAudit.Text = "Audit";
                            miAudit.Value = "Admin";
                            miAudit.NavigateUrl = "~/Pages/Document/AuditReport.aspx";
                            miReports.ChildItems.Add(miAudit);
                        }

                        MenuItem miReportNew = new MenuItem();
                        miReportNew.Text = "New";
                        miReportNew.Value = "Admin";
                        miReportNew.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/ReportDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SSearchCriteriaID=" + Cryptography.Encrypt("-1");
                        miReports.ChildItems.Add(miReportNew);


                        MenuItem miDocGen = new MenuItem();
                        miDocGen.Text = "Reports";
                        miDocGen.Value = "Admin";
                        miDocGen.Selectable = true;
                        miDocGen.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                        miReports.ChildItems.Add(miDocGen);

                        MenuItem miSchedule = new MenuItem();
                        miSchedule.Text = "Schedule";
                        miSchedule.Value = "Admin";
                        miSchedule.NavigateUrl = "~/Pages/Schedule/ScheduleReport.aspx";
                        miReports.ChildItems.Add(miSchedule);



                    }








                    MenuItem miST = new MenuItem();
                    //miST.Text = "Tables";
                    miST.Text = SecurityManager.etsTerminology("", "Tables", "Tables");
                    miST.Value = "Admin";
                    miST.NavigateUrl = "~/Pages/Record/TableList.aspx";
                    miAdmin.ChildItems.Add(miST);




                    //MenuItem miSensor = new MenuItem();
                    //miSensor.Text = "Sensors";
                    //miSensor.Value = "Admin";
                    //miSensor.NavigateUrl = "~/Pages/Instrument/Sensor.aspx";
                    //miAdmin.ChildItems.Add(miSensor);








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
                        if (_bIsAccountHolder)
                        {

                            MenuItem miTerminology = new MenuItem();
                            miTerminology.Text = "Terminology";
                            miTerminology.Value = "Admin";
                            miTerminology.NavigateUrl = "~/Pages/Security/Terminology.aspx";
                            miAdmin.ChildItems.Add(miTerminology);
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


                        MenuItem miWorkFlow = new MenuItem();
                        miWorkFlow.Text = "WorkFlow";
                        miWorkFlow.Value = "Admin";
                        miWorkFlow.NavigateUrl = "~/Pages/WorkFlow/WorkFlow.aspx";
                        miAdmin.ChildItems.Add(miWorkFlow);

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
            _objUser = null;
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
                if (roletype.Contains("1"))
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
            Session["AccountID"] = hfAccountIDToChangeAccount.Value;


            User ObjUser = (User)Session["User"];

            string roletype = "";
            roletype = SecurityManager.GetUserRoleTypeID((int)ObjUser.UserID, int.Parse(Session["AccountID"].ToString()));



            Session["roletype"] = roletype;

            UserRole theUserRole = SecurityManager.GetUserRole((int)ObjUser.UserID, int.Parse(Session["AccountID"].ToString()));

            Session["UserRole"] = theUserRole;


            try
            {
                Session["DoNotAllow"] = null;
                //Session["ExpireLeftDay"] = null;
                Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));


                //if (theAccount.ExpiryDate != null)
                //{
                //    if (theAccount.ExpiryDate.Value.AddDays(0) < DateTime.Today)
                //    {
                //        Session["DoNotAllow"] = "true";

                //    }
                //}             


                


                if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())))
                {
                    Session["DoNotAllow"] = "true";
                    //Response.Redirect("~/Pages/Security/AccountTypeChange.aspx?type=renew", false);
                    //return;
                }


            }
            catch
            {
                //
            }

            //Page_Init(null, null);
            //Page_Load(null, null);
            Response.Redirect("~/Default.aspx", false);
        }

    }

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

        //bool bIsDemoUser = false;

        //if (Session["User"] != null)
        //{
        //    User oUser = (User)Session["User"];
        //    if (oUser != null && oUser.Email.ToLower() == Common.DemoEmail.ToLower())
        //    {
        //        bIsDemoUser = true;
        //    }
        //}

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



        //if (bIsDemoUser)
        //{
        //    Response.Redirect("~/Login.aspx?Logout=yes&demo=yes", false);
        //}
        //else
        //{

        if (Session["client"] != null)
        {
            Response.Redirect("~/Login.aspx?Logout=yes&Account=" + Session["client"].ToString(), false);
            Session["client"] = null;
            return;
        }

        if (Session["LoginAccount"] == null)
        {
            Response.Redirect("~/Login.aspx?Logout=yes", false);
        }
        else
        {
            Response.Redirect("~/Login.aspx?Logout=yes&" + strLoginAccount, false);
        }
        //}


    }




    protected void lnkChangeAccount_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Security/AccountList.aspx", false);
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


}

