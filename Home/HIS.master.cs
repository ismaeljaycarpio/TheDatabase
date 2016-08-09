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


public partial class HIS : System.Web.UI.MasterPage
{

    bool _bDemoUser = false;
    Account _theAccount;
    string _strProjectID = "QQlxU7sRJz0=";

    string _strStaffTableID = "2223";
    string _strStaffID = "bbuhIc16qDs=";

    string _strStaff_InternalViewID = "344";
    string _strStaff_ExternalID = "350";
    string _strStaff_PastViewID = "362";
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
        User ObjUser = (User)Session["User"];

        _CurrentUserRole = (UserRole)Session["UserRole"];
        if (_CurrentUserRole.IsAccountHolder != null && (bool)_CurrentUserRole.IsAccountHolder)
        {
            _bIsAccountHolder = true;
            hfIsAccountHolder.Value = "yes";
        }
        string strProjectTableID = SystemData.SystemOption_ValueByKey_Account("HISProjectTableID", null, null);
        if (strProjectTableID != "")
            _strProjectID = Cryptography.Encrypt(strProjectTableID);

        string strCurrentViewID = Common.GetValueFromSQL("SELECT ViewID FROM [View] WHERE TableID=" + strProjectTableID + " AND ViewName='Current-Projects'");
        string strPastProjectsViewID = Common.GetValueFromSQL("SELECT ViewID FROM [View] WHERE TableID=" + strProjectTableID + " AND ViewName='Past-Projects'");

        string strTemp = SystemData.SystemOption_ValueByKey_Account("HISStaffTableID", null, null);

          if(strTemp!="")
          {
              _strStaffTableID = strTemp;
              _strStaffID = Cryptography.Encrypt(strTemp);

          }
         strTemp = Common.GetValueFromSQL("SELECT ViewID FROM [View] WHERE TableID=" + _strStaffTableID + " AND ViewName='Staff-Internal'");
        if (strTemp != "")
            _strStaff_InternalViewID = strTemp;

        strTemp = Common.GetValueFromSQL("SELECT ViewID FROM [View] WHERE TableID=" + _strStaffTableID + " AND ViewName='Staff-External'");
        if (strTemp != "")
            _strStaff_ExternalID = strTemp;

        strTemp = Common.GetValueFromSQL("SELECT ViewID FROM [View] WHERE TableID=" + _strStaffTableID + " AND ViewName='Staff-Past'");
        if (strTemp != "")
            _strStaff_PastViewID = strTemp;

        //hlStaffInternal.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID="+_strStaffID+"&View=" + _strStaff_InternalViewID;

        //

        hlStaffInternal.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + _strStaffID + "&View=" + _strStaff_InternalViewID;
        hlStaffCW.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + _strStaffID + "&View=" + _strStaff_ExternalID;
        hlStaffPast.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + _strStaffID + "&View=" + _strStaff_PastViewID;
        hlSTAFF_CURRENT_TICKETS.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + _strStaffID;
        hlSTAFF_FUTURE_TRAINING.NavigateUrl = hlSTAFF_CURRENT_TICKETS.NavigateUrl;

        //
        
        hlProjects.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + _strProjectID + "&View=" + strCurrentViewID;
        hlProjectTop.NavigateUrl = hlProjects.NavigateUrl;
        hlCurrentProjects.NavigateUrl = hlProjects.NavigateUrl; 
        hlPastProjects.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + _strProjectID + "&View=" + strPastProjectsViewID; 
        
        //



        _theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
        if (ObjUser != null && ObjUser.UserID > 0)
        {
            if (Context.User.Identity.IsAuthenticated)
            {

                //lkProfile.Text = ObjUser.FirstName + " " + ObjUser.LastName;
            }
            BindMenu();
            BindMenuProfile();
            BindMenuReport();
            if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            {
                BindAccountMenu();
            }

            if (!IsPostBack)
            {
                ShowHideHBMenus();
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

        //if (bGotSelected == false)
        //{
        //    if (Request.RawUrl.IndexOf("Site/LocationDetail.aspx") > -1 )
        //    {
        //        foreach (MenuItem item in menuETS.Items)
        //        {
        //            if ("AllRecordSites" == item.Value)
        //            {
        //                item.Selectable = true;
        //                item.Selected = true;
        //                bGotSelected = true;
        //                break;
        //            }

        //        }
        //    }

        //}



        //if (bGotSelected == false)
        //{
        //    if (Request.RawUrl.IndexOf("Document/DocumentDetail.aspx") > -1)
        //    {
        //        foreach (MenuItem item in menuETS.Items)
        //        {
        //            if ("AllDocuments" == item.Value)
        //            {
        //                item.Selectable = true;
        //                item.Selected = true;
        //                bGotSelected = true;
        //                break;
        //            }

        //        }
        //    }

        //}


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
                Request.RawUrl.IndexOf("Site/LocationDetail.aspx") > -1 ||
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


    protected void Page_Load(object sender, EventArgs e)
    {

        

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

       

        //if (!Page.IsPostBack)
        //{
        //    if (Session["LoginAccount"] != null)
        //    {
        //        divMarketingMenu.Visible = false;
        //        Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

        //        if (theAccount != null)
        //        {
        //            //divCopyright.InnerText = theAccount.CopyRightInfo;                  

        //        }

        //    }
        //}


        //if (Session["ExpireLeftDay"] != null)
        //{
        //    divRenew.Visible = true;

        //    if (int.Parse(Session["ExpireLeftDay"].ToString()) > 7)
        //    {
        //        lblRenewMessage.Text = "Your account is overdue by " + Session["ExpireLeftDay"].ToString() + " days.";
        //    }
        //    else
        //    {

        //        lblRenewMessage.Text = "Your account is due to expire in "+
        //           ( 7-int.Parse(Session["ExpireLeftDay"].ToString())).ToString() + " days. ";
        //    }
        //}
        //else
        //{
        //    divRenew.Visible = false;
        //}

        if (!Page.IsPostBack)
        {
           

            if (!Context.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        //CheckSelectedMenu();



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

            SystemData.VisitorInsert((User)Session["User"], Request.UserHostAddress, Request.UserAgent, Request.AppRelativeCurrentExecutionFilePath, strRefSite);

            User ObjUser = (User)Session["User"];
            if (ObjUser != null && ObjUser.UserID > 0)
            {
                if (ObjUser.Email.ToLower() == "demo@carbonmonitoring.com.au")
                {
                   trEndDemo.Visible=true;
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

    //protected void CheckConcurrent()
    //{
    //    if (Session["User"] != null)
    //    {
    //        User ObjUser = (User)Session["User"];

    //        string strSessionID = SecurityManager.User_SessionID_Get((int)ObjUser.UserID);

    //        if (strSessionID != "")
    //        {
    //            if (strSessionID != Session.SessionID.ToString())
    //            {
    //                //ops 
    //                SecurityManager.User_LoggedOutCount_Increment((int)ObjUser.UserID);
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
        if (Request.RawUrl.IndexOf("/Security/AccountTypeChange.aspx") > -1 ||
            Request.RawUrl.IndexOf("Security/AccountDetail.aspx") > -1)
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
                    //Response.Redirect("~/Default.aspx", false);
                }

            }

        }

    }

      protected void BindMenuProfile()
    {
        menuProfile.Items.Clear();
        string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;

        User ObjUser = (User)Session["User"];

        if (ObjUser != null && ObjUser.UserID > 0)
        {
            MenuItem miUserName = new MenuItem();
            miUserName.Text = ObjUser.FirstName + " " + ObjUser.LastName;
            miUserName.NavigateUrl = "~/Default.aspx";
            miUserName.Value = "UserName";
            menuProfile.Items.Add(miUserName);
            //if (!Common.HaveAccess(Session["roletype"].ToString(), "5"))
            //{

                if (_theAccount.HideMyAccount != null && (bool)_theAccount.HideMyAccount)
                {

                }
                else
                {
                    MenuItem miMyAccount = new MenuItem();
                    miMyAccount.Text = "My Account";
                    miMyAccount.Value = "UserName";
                    miMyAccount.NavigateUrl = "~/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(Session["AccountID"].ToString());
                    miUserName.ChildItems.Add(miMyAccount);
                }
               

            //}
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

          hlReport.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
          hlDocuments.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Document.aspx?SSearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt("-1") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1");
                   
      }




      protected void BindAccountMenu()
      {
          menuAccount.Items.Clear();
          string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;

          User ObjUser = (User)Session["User"];

          if (ObjUser != null && ObjUser.UserID > 0)
          {
              int? iAccountID = SecurityManager.GetPrimaryAccountID((int)ObjUser.UserID);
              
//              DataTable dtTemp = Common.DataTableFromText(@"SELECT     LinkedUser.LinkedUserID, Account.AccountName, LinkedUser.UserID, LinkedUser.AccountID
//                                            FROM         Account INNER JOIN
//                                            LinkedUser ON Account.AccountID = LinkedUser.AccountID
//                                            WHERE  LinkedUser.UserID=" + ObjUser.UserID);

              DataTable dtTemp = Common.DataTableFromText(@"SELECT DISTINCT UserRole.AccountID,  AccountName FROM 
                    UserRole INNER JOIN Account ON UserRole.AccountID=Account.AccountID
                    WHERE UserID=" + ObjUser.UserID.ToString() + " ORDER BY AccountName");
              if (dtTemp.Rows.Count > 0)
              {
                  menuAccount.Visible = true;
                  

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
                  if (theAccount != null)
                  {
                      MenuItem miTempChild = new MenuItem();
                      miTempChild.Text = theAccount.AccountName;
                      miTempChild.NavigateUrl = "javascript:document.getElementById('hfAccountIDToChangeAccount').value = '" + theAccount.AccountID + "';__doPostBack('ctl00$lkChangeAccount','')"; ;
                      miAccounts.ChildItems.Add(miTempChild);
                  }

                  Account theAccountRoot = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

                  if (theAccountRoot != null)
                  {
                      miAccounts.Text = theAccountRoot.AccountName;
                  }

              }
            


          }
      }

      protected void ShowHideLeftDIV(ref  HtmlGenericControl divShow)
      {
          divHow_we_Work.Visible = false;
          divHome.Visible = false;
          divHutchies_Sydney.Visible = false;

          divHB_Contacts.Visible = false;
          divHB_PROJECT_MANAGEMENT.Visible = false;
          divProjects.Visible = false;
          divQUALIFICATIONS_TRAININGCOURSES.Visible = false;
          divFIRST_AID_ON_LINE.Visible = false;
          divCommunity_Involvement.Visible = false;

          divShow.Visible = true;
      }

      protected void ShowHideHBMenus()
      {

          if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBCommunityInvolvement".ToLower()) > 1
            || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBIndustryInvolvement".ToLower()) > 1
            || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBSocialPartnership".ToLower()) > 1
              || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_SYDNEY_CHARITY_GALA_EVENTS".ToLower()) > 1
              || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_COMMUNITY_INVESTMENT_PROGRAMS".ToLower()) > 1
             )
          {
              ShowHideLeftDIV(ref divCommunity_Involvement);

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBCommunityInvolvement".ToLower()) > 1)
              {
                  hlHBCommunityInvolvement.CssClass = "selectedleftmenu";
              }

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBIndustryInvolvement".ToLower()) > 1)
              {
                  hlHBIndustryInvolvement.CssClass = "selectedleftmenu";
              }

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBSocialPartnership".ToLower()) > 1)
              {
                  hlHBSocialPartnership.CssClass = "selectedleftmenu";
              }

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_SYDNEY_CHARITY_GALA_EVENTS".ToLower()) > 1)
              {
                  hlHB_SYDNEY_CHARITY_GALA_EVENTS.CssClass = "selectedleftmenu";
              }

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_COMMUNITY_INVESTMENT_PROGRAMS".ToLower()) > 1)
              {
                  hlHB_COMMUNITY_INVESTMENT_PROGRAMS.CssClass = "selectedleftmenu";
              }


          }

          if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBFirstAidOnLine".ToLower()) > 1
              || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=RbS6zECXdF0=".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=imaraZDiQII=".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=gRFM7avNEtE=".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=0KGAw9tioMQ=".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=WhtLHiPtfHQ=".ToLower()) > 1
              )
          {
             

              ShowHideLeftDIV(ref divFIRST_AID_ON_LINE);

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBFirstAidOnLine".ToLower()) > 1)
              {
                  hlHBFirstAidOnLine.CssClass = "selectedleftmenu";
              }

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=RbS6zECXdF0=".ToLower()) > 1)
              {
                  hlHB_PROJECT_DETAILS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=imaraZDiQII=".ToLower()) > 1)
              {
                  hlHB_INJURY_DETAILS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=gRFM7avNEtE=".ToLower()) > 1)
              {
                  hlHB_PLANT_INFORMATION.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=0KGAw9tioMQ=".ToLower()) > 1)
              {
                  hlHB_ACTIONS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("TableID=WhtLHiPtfHQ=".ToLower()) > 1)
              {
                  hlHB_ATTACHMENTS.CssClass = "selectedleftmenu";
              }
          }

          if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBQualificationTrainingCourse".ToLower()) > 1
             || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_COURSES".ToLower()) > 1
             || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBTickets".ToLower()) > 1
               || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_Various_EHS_courses".ToLower()) > 1
               || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_IT_Courses".ToLower()) > 1
               || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_Management_Courses".ToLower()) > 1
              )
          {
              
              ShowHideLeftDIV(ref divQUALIFICATIONS_TRAININGCOURSES);

              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBQualificationTrainingCourse".ToLower()) > 1)
              {
                  hlQualificationTrainingCourse.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_COURSES".ToLower()) > 1)
              {
                  hlCOURSES.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBTickets".ToLower()) > 1)
              {
                  hlTickets.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_Various_EHS_courses".ToLower()) > 1)
              {
                  hlHB_Various_EHS_courses.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_IT_Courses".ToLower()) > 1)
              {
                  hlHB_IT_Courses.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_Management_Courses".ToLower()) > 1)
              {
                  hlHB_Management_Courses.CssClass = "selectedleftmenu";
              }

          }
          if (Request.RawUrl.ToString().ToLower().Replace("%20"," ").IndexOf("Contentkey=Hutchies Sydney".ToLower()) > 1
              || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_OUR_COMPANY_STRUCTURE".ToLower()) > 1
              || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_BRANDING_AND_TEMPLATES".ToLower()) > 1
              || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBLogo".ToLower()) > 1
              || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_ACCREDITATIONS_INSURANCES".ToLower()) > 1
              || Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBPolicies".ToLower()) > 1)
          {
              
              ShowHideLeftDIV(ref divHutchies_Sydney);

              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=Hutchies Sydney".ToLower()) > 1)
              {
                  hlHUTCHIES_SYDNEY.CssClass = "selectedleftmenu";
              }

              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_OUR_COMPANY_STRUCTURE".ToLower()) > 1)
              {
                  hlOur_Company_Structure.CssClass = "selectedleftmenu";
              }

              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_BRANDING_AND_TEMPLATES".ToLower()) > 1)
              {
                  hlHB_Branding_Templates.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBLogo".ToLower()) > 1)
              {
                  hlLOGOS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_ACCREDITATIONS_INSURANCES".ToLower()) > 1)
              {
                  hlACCREDITATIONS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBPolicies".ToLower()) > 1)
              {
                  hlPOLICIES.CssClass = "selectedleftmenu";
              }
          }

          if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=How WE WORK".ToLower()) > 1
              || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=Systems".ToLower()) > 1
              || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_".ToLower()) > 1
              || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBBooklet".ToLower()) > 1)
          {
              

              ShowHideLeftDIV(ref divHow_we_Work);

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=How WE WORK".ToLower()) > 1)
              {
                  hlHOW_WE_WORK.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=Systems".ToLower()) > 1)
              {
                  hlSYSTEMS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBBooklet".ToLower()) > 1)
              {
                  hlHUTCHIES_QA_BOOKLET.CssClass = "selectedleftmenu";
              }



              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Intranet".ToLower()) > 1)
              {
                  hlSYSTEMS_Intranet.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Aconex".ToLower()) > 1)
              {
                  if (Request.QueryString["Contentkey"].ToLower() == "HB_System_Aconex".ToLower())
                  {
                      hlSYSTEMS_Aconex.CssClass = "selectedleftmenu";
                  }
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Aconex_Field_Management".ToLower()) > 1)
              {
                  hlSYSTEMS_Aconex_Field_Management.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Aconex_Smart_Manuals".ToLower()) > 1)
              {
                  hlSystem_Aconex_Smart_Manuals.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_View_Point".ToLower()) > 1)
              {
                  hlSystem_View_Point.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_POMS".ToLower()) > 1)
              {
                  hlSystem_POMS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Filenet".ToLower()) > 1)
              {
                  hlSystem_Filenet.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Chemwatch".ToLower()) > 1)
              {
                  hlSystem_Chemwatch.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Blue_Glue".ToLower()) > 1)
              {
                  hlSystem_Blue_Glue.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_RWW_Group".ToLower()) > 1)
              {
                  hlSystem_RWW_Group.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Staples_OnLine".ToLower()) > 1)
              {
                  hlSystem_Staples_OnLine.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_System_Gencom".ToLower()) > 1)
              {
                  hlSystem_Gencom.CssClass = "selectedleftmenu";
              }

          }


          if ((Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf(("TableID=" + _strProjectID).ToLower()) > 1
              && (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Record/RecordDetail.aspx".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Record/RecordList.aspx".ToLower()) > 1)))
          {
             
              ShowHideLeftDIV(ref divProjects);

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("View=945".ToLower()) > 1)
              {
                  hlPastProjects.CssClass = "selectedleftmenu";
              }
              else
              {
                  hlCurrentProjects.CssClass = "selectedleftmenu";
              }
          }


          if ( Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB Contacts".ToLower()) > 1
              ||
              (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf(("TableID="+_strStaffID).ToLower()) > 1
              && (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Record/RecordDetail.aspx".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Record/RecordList.aspx".ToLower()) > 1)))
          {              

              ShowHideLeftDIV(ref divHB_Contacts);

              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB Contacts".ToLower()) > 1)
              {
                  hlHB_CONTACTS.CssClass = "selectedleftmenu";
              }


              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf(("View="+_strStaff_InternalViewID).ToLower()) > 1)
              {
                  hlStaffInternal.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf(("View="+_strStaff_ExternalID).ToLower()) > 1)
              {
                  hlStaffCW.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf(("View="+_strStaff_PastViewID).ToLower()) > 1)
              {
                  hlStaffPast.CssClass = "selectedleftmenu";
              }

          }

          if (Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB Project Management".ToLower()) > 1
              || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBInductions".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_PROCEDURES_MANUALS_GUIDELINES".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HBHealthSafety".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_HR".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_CONTRACTS_ADMINISTRATION".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_PROJECT_REPORTING".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_PROJECT_COMPLETION".ToLower()) > 1
               || Request.RawUrl.ToString().Replace("%20", " ").ToLower().IndexOf("Contentkey=HB_GENERAL_FORMS".ToLower()) > 1
              )
          {
             
              ShowHideLeftDIV(ref divHB_PROJECT_MANAGEMENT);

              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB Project Management".ToLower()) > 1)
              {
                  hlHB_PROJECT_MANAGEMENT.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBInductions".ToLower()) > 1)
              {
                  hlINDUCTIONS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_PROCEDURES_MANUALS_GUIDELINES".ToLower()) > 1)
              {
                  hlPROCEDURES_MANUALS_GUIDELINES.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HBHealthSafety".ToLower()) > 1)
              {
                  hlEHSSYSTEMS.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_HR".ToLower()) > 1)
              {
                  hlHR.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_CONTRACTS_ADMINISTRATION".ToLower()) > 1)
              {
                  hlCONTRACTS_ADMINISTRATION.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_PROJECT_REPORTING".ToLower()) > 1)
              {
                  hlPROJECT_REPORTING.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_PROJECT_COMPLETION".ToLower()) > 1)
              {
                  hlPROJECT_COMPLETION.CssClass = "selectedleftmenu";
              }
              if (Request.RawUrl.ToString().ToLower().Replace("%20", " ").IndexOf("Contentkey=HB_GENERAL_FORMS".ToLower()) > 1)
              {
                  hlGENERAL_FORMS.CssClass = "selectedleftmenu";
              }
              
          }

      }

    protected void BindMenu()
    {
        menuETS.Items.Clear();
        string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;

         int iTN=0;
         int iTempCount = 0;
         User ObjUser = (User)Session["User"];

         ObjUser = SecurityManager.User_Details((int)ObjUser.UserID);
         Session["User"] = ObjUser;


         if (ObjUser.Email.ToLower() == SystemData.SystemOption_ValueByKey_Account("DemoAccountCreator",null,null).ToLower())
         {
             _bDemoUser = true;
         }

         UserRole theUserRole = SecurityManager.GetUserRole((int)ObjUser.UserID, int.Parse(Session["AccountID"].ToString()));
       

         if (theUserRole == null)
         {
             if (Session["roletype"].ToString() == "1")
             {
                 theUserRole = SecurityManager.GetGlobalUserRole((int)ObjUser.UserID);
             }

         }
         Session["UserRole"] = theUserRole;

         if ((bool)theUserRole.IsAdvancedSecurity)
         {
             //Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)ObjUser.UserID, "-1,3,4,5,7");
             Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)theUserRole.RoleID);
         }
         else
         {
             Session["STs"] = "";
            
             string roletype = "";
             roletype = SecurityManager.GetUserRoleTypeID((int)ObjUser.UserID, int.Parse(Session["AccountID"].ToString()));
            

             Session["roletype"] = roletype;
             if (roletype == "6")
             {
                 Session["STs"] = "-1";
             }
         }



         if (ObjUser != null && ObjUser.UserID > 0)
         {

             //

             Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
                                     
             //populate accountwise things

             

             if (theAccount.Logo != null)
             {
                 if ((bool)theAccount.UseDefaultLogo==false)
                    imgLogo.ImageUrl = "~/SSPhoto.ashx?AccountID=" + Session["AccountID"].ToString() + "&type=o";
             }
             
             //populate dynamic Table Group 
             //List<Menu> listSTG = RecordManager.ets_Menu_List(int.Parse(Session["AccountID"].ToString()));



           
         


             if (Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
             {

                 MenuItem miAdmin = new MenuItem();
                 miAdmin.Text = "ADMIN";
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

                     //MenuItem miGraph = new MenuItem();
                     //miGraph.Text = "Graphs";
                     //miGraph.Value = "Admin";
                     //miGraph.NavigateUrl = "~/Pages/Graph/GraphOptions.aspx";
                     //miAdmin.ChildItems.Add(miGraph);     
                    
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


                         MenuItem miAudit = new MenuItem();
                         miAudit.Text = "Audit";
                         miAudit.Value = "Admin";
                         miAudit.NavigateUrl = "~/Pages/Document/AuditReport.aspx";
                         miReports.ChildItems.Add(miAudit);


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






                     //MenuItem miRecordSite = new MenuItem();
                     ////miRecordSite.Text = "Locations";
                     //miRecordSite.Text = SecurityManager.etsTerminology("", "Locations", "Locations");
                     //miRecordSite.Value = "Admin";
                     //miRecordSite.NavigateUrl = "~/Pages/Site/LocationList.aspx?MenuID=" +
                     //Cryptography.Encrypt("-1");
                     //miAdmin.ChildItems.Add(miRecordSite);

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


                     }

                     //MenuItem miWorkSite = new MenuItem();
                     //miWorkSite.Text = "Work Sites";
                     //miWorkSite.Value = "Admin";
                     //miWorkSite.NavigateUrl = "~/Pages/Site/WorkSite.aspx";
                     //miAdmin.ChildItems.Add(miWorkSite);
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
            User ObjUser = (User)Session["User"];
            //List<UserRole> uroles = ObjUser.UserRoles.ToList();
            //List<UserRole> uroles = SecurityManager.UserRole_Select(null, ObjUser.UserID, null, null, null, "", "", null, null, ref iTN);

            
            //foreach (UserRole item in uroles)
            //{
            //    roletype += item.RoleType.ToString() + ",";
            //}

            string roletype = Session["roletype"].ToString();
            if (roletype.Length > 0)
            {
                if (roletype.Contains("2"))
                {
                    kq = true;
                }
            }
            ObjUser = null;
        }
        return kq;
    }
    public bool DisplayGod()
    {
        bool kq = false;
        //int iTN = 0;
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
            User ObjUser = (User)Session["User"];

            Session["AccountID"] = hfAccountIDToChangeAccount.Value;

            string roletype = "";
            roletype = SecurityManager.GetUserRoleTypeID((int)ObjUser.UserID, int.Parse(Session["AccountID"].ToString()));



            Session["roletype"] = roletype;

            UserRole theUserRole = SecurityManager.GetUserRole((int)ObjUser.UserID, int.Parse(Session["AccountID"].ToString()));

            Session["UserRole"] = theUserRole;



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
        Response.Redirect("~/Environmental-Monitoring-Management-Database-System.aspx?demo=yes", false);

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



    //MenuItem GetSelectedMenuItem(Menu menu)
    //{
    //    MenuItem selectedItem = null;
    //    if (Request.Form["__EVENTTARGET"] == menu.ID)
    //    {
    //        string value = Request.Form["__EVENTARGUMENT"];
    //        if (!string.IsNullOrEmpty(value))
    //        {
    //            foreach (MenuItem mi in menu.Items)
    //            {
    //                if (mi.Value == value)
    //                {
    //                    selectedItem = mi;
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        selectedItem = menu.SelectedItem;
    //    }
    //    return selectedItem;
    //}


}


//if (IdMenu.Equals(1))
//    sbMenu.AppendLine("<li><a href=\"../Persons/List.aspx\" class=\"current\" onclick=\"Onclick(1,'../Persons/List.aspx')\">People</a></li>");
//else
//    sbMenu.AppendLine("<li><a href=\"../Persons/List.aspx\" class=\"menulink\" onclick=\"Onclick(1,'../Persons/List.aspx')\">People</a></li>");

//if (DisplayGod())
//{
//sbMenu.AppendLine("<li><a href=\"#\" class=\"sub\">Security</a>");
//sbMenu.AppendLine("<ul class=\"topline\">");
//sbMenu.AppendLine("</ul>");


//sbMenu.AppendLine("<li><a href=\"#\" class=\"sub\">System Data</a>");
//sbMenu.AppendLine("<ul class=\"topline\">");
//sbMenu.AppendLine("<li><a href=\"" + strAppPath + "/Pages/Record/TableList.aspx\" onclick=\"Onclick(2,'" + strAppPath + "/Pages/Record/TableList.aspx')\">Table Summary</a></li>");
//sbMenu.AppendLine("</ul>");


//sbMenu.AppendLine("<ul>");

//sbMenu.AppendLine("<li><a href=\"#\" class=\"sub\">Record</a>");
//sbMenu.AppendLine("<ul class=\"topline\">");
//sbMenu.AppendLine("<li><a href=\"" + strAppPath + "/Pages/Record/RecordList.aspx?menu=no\" onclick=\"Onclick(2,'" + strAppPath + "/Pages/Record/RecordList.aspx?menu=no')\">Records</a></li>");

//sbMenu.AppendLine("</ul>");


//sbMenu.AppendLine("<li><a href=\"" + strAppPath + "/Pages/Site/LocationList.aspx?menu=no\" onclick=\"Onclick(2,'" + strAppPath + "/Pages/Site/LocationList.aspx?menu=no')\">RecordSites</a></li>");
//sbMenu.AppendLine("<li><a href=\"" + strAppPath + "/Pages/Graph/RecordChart.aspx\" onclick=\"Onclick(2,'" + strAppPath + "/Pages/Graph/RecordChart.aspx')\">Chart Test Page</a></li>");


//sbMenu.AppendLine("<li><a href=\"#\" class=\"sub\">Test</a>");
//sbMenu.AppendLine("<ul class=\"topline\">");
//sbMenu.AppendLine("<li><a href=\"" + strAppPath + "/Pages/Test/TestInLine.aspx\" onclick=\"Onclick(2,'" + strAppPath + "/Pages/Test/TestInLine.aspx')\">Test InLine(not good)</a></li>");

//sbMenu.AppendLine("</ul>");



//sbMenu.AppendLine("<li><a href=\"../Accounts/List.aspx\" onclick=\"Onclick(2,'../Accounts/List.aspx')\">Account</a></li>");
//sbMenu.AppendLine("<li><a href=\"../Rights/List.aspx\" onclick=\"Onclick(2,'../Rights/List.aspx')\">Rights</a></li>");

//sbMenu.AppendLine("<li><a href=\"../EMails/EmailFollowup.aspx\" onclick=\"Onclick(2,'../EMails/EmailFollowup.aspx')\">Emails</a></li>");
//sbMenu.AppendLine("<li><a href=\"../fileupload/fileuploadcommon.aspx\" onclick=\"Onclick(2,'../fileupload/fileuploadcommon.aspx')\">File Upload</a></li>");
//sbMenu.AppendLine("<li><a href=\"../OutLook/OutLookContact.aspx\" onclick=\"Onclick(2,'../OutLook/OutLookContact.aspx')\">Upload Outlook Contact</a></li>");


//sbMenu.AppendLine("<li><a href=\"#\" class=\"sub\">Master Settings</a>");
//sbMenu.AppendLine("<ul class=\"topline\">");
//sbMenu.AppendLine("<li><a href=\"../Contents/List.aspx\" onclick=\"Onclick(2,'../Contents/List.aspx')\">Contents</a></li>");
//sbMenu.AppendLine("<li><a href=\"../Organisations/List.aspx\" onclick=\"Onclick(2,'../Organisations/List.aspx')\">Organisations</a></li>");
//sbMenu.AppendLine("<li><a href=\"../Attributes/List.aspx\" onclick=\"Onclick(2,'../Attributes/List.aspx')\">Attributes</a></li>");
//sbMenu.AppendLine("<li><a href=\"../PersonStatus/List.aspx\" onclick=\"Onclick(2,'../PersonStatus/List.aspx')\">PersonStatus</a></li>");
//sbMenu.AppendLine("<li><a href=\"../PersonTypes/List.aspx\" onclick=\"Onclick(2,'../PersonTypes/List.aspx')\">PersonType</a></li>");
//sbMenu.AppendLine("<li><a href=\"../States/List.aspx\" onclick=\"Onclick(2,'../States/List.aspx')\">State</a></li>");
//sbMenu.AppendLine("<li><a href=\"../Countries/List.aspx\" onclick=\"Onclick(2,'../Countries/List.aspx')\">Country</a></li>");
//sbMenu.AppendLine("<li><a href=\"../ErrorLogs/List.aspx\" onclick=\"Onclick(2,'../ErrorLogs/List.aspx')\">Error Log</a></li>");

// sbMenu.AppendLine("<li><a href=\"#\" onclick=\"Onclick(2,'../UserExportFileldLists/List.aspx')\">User Export Fileld Lists</a></li>");
//}
//else
//if (DisplayAdmin())
//{
//    if (IdMenu.Equals(2))
//        sbMenu.AppendLine("<li><a href=\"#\" class=\"current\">Admin</a>");
//    else
//        sbMenu.AppendLine("<li><a href=\"#\" class=\"menulink\">Admin</a>");

//    sbMenu.AppendLine("<ul>");
//    sbMenu.AppendLine("<li><a href=\"../Users/List.aspx\" onclick=\"Onclick(2,'../Users/List.aspx')\">Users</a></li>");


//    sbMenu.AppendLine("<li><a href=\"../Accounts/List.aspx\" onclick=\"Onclick(2,'../Accounts/List.aspx')\">Account</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../EMails/EmailFollowup.aspx\" onclick=\"Onclick(2,'../EMails/EmailFollowup.aspx')\">Emails</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../fileupload/fileuploadcommon.aspx\" onclick=\"Onclick(2,'../fileupload/fileuploadcommon.aspx')\">File Upload</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../OutLook/OutLookContact.aspx\" onclick=\"Onclick(2,'../OutLook/OutLookContact.aspx')\">Upload Outlook Contact</a></li>");


//    sbMenu.AppendLine("<li><a href=\"#\" class=\"sub\">Master Settings</a>");
//    sbMenu.AppendLine("<ul class=\"topline\">");
//    sbMenu.AppendLine("<li><a href=\"../Contents/List.aspx\" onclick=\"Onclick(2,'../Contents/List.aspx')\">Contents</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../Organisations/List.aspx\" onclick=\"Onclick(2,'../Organisations/List.aspx')\">Organisations</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../Attributes/List.aspx\" onclick=\"Onclick(2,'../Attributes/List.aspx')\">Attributes</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../PersonStatus/List.aspx\" onclick=\"Onclick(2,'../PersonStatus/List.aspx')\">PersonStatus</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../PersonTypes/List.aspx\" onclick=\"Onclick(2,'../PersonTypes/List.aspx')\">PersonType</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../States/List.aspx\" onclick=\"Onclick(2,'../States/List.aspx')\">State</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../Countries/List.aspx\" onclick=\"Onclick(2,'../Countries/List.aspx')\">Country</a></li>");
//    sbMenu.AppendLine("<li><a href=\"../ErrorLogs/List.aspx\" onclick=\"Onclick(2,'../ErrorLogs/List.aspx')\">Error Log</a></li>");

//    // sbMenu.AppendLine("<li><a href=\"#\" onclick=\"Onclick(2,'../UserExportFileldLists/List.aspx')\">User Export Fileld Lists</a></li>");

//    sbMenu.AppendLine("</ul>");
//    sbMenu.AppendLine("</li>");

//    sbMenu.AppendLine("</ul>");
//    sbMenu.AppendLine("</li>");
//}

