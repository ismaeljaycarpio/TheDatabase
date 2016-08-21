using System;
using System.Web.UI;
using System.Web.Security;

public class SecurePage : Page
{
    //private void GoHome()
    //{

    //    if (Session["LoginAccount"] == null)
    //    {
    //        Session.Clear();
    //        Response.Redirect(base.Request.Url.Scheme +"://" + base.Request.Url.Authority + base.Request.ApplicationPath + "/Login.aspx?ReturnURL=" + Server.UrlEncode(base.Request.RawUrl),false);
    //        return;
    //    }
    //    else
    //    {
    //        string strLoginAccount=Session["LoginAccount"].ToString();
    //        Session.Clear();
    //        Response.Redirect(base.Request.Url.Scheme +"://" + base.Request.Url.Authority + base.Request.ApplicationPath + "/Login.aspx?ReturnURL=" + Server.UrlEncode(base.Request.RawUrl) + "&" + strLoginAccount,false);
    //        return;
    //    }
       
    //}

    protected bool IsValidUser()
    {
     
        if(Session["User"]==null)
        {
            return false; 
        }

        User loggedUser = (User)Session["User"];
        if (loggedUser == null)
        {
            return false;
        }
        //if (loggedUser.UserRoles.Count < 1)
        //{
        //    return false;
        //}
        return true;
    }
//    public void FindTheAccount()
//    {
//        if (!IsPostBack)//&& Session["roletype"] != null && Session["roletype"] != "1"
//        {
//            try
//            {
//                int iTemp = 0;
//                string strPageItemAccountID = "";

//                if (strPageItemAccountID == "" && Request.QueryString["TableID"] != null)
//                {
//                    iTemp = 0;
//                    if (int.TryParse(Request.QueryString["TableID"].ToString(), out iTemp))
//                    {
//                        strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Table] T 
//                              WHERE T.TableID=" + Request.QueryString["TableID"].ToString());
//                    }
//                    else
//                    {
//                        strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Table] T 
//                              WHERE T.TableID=" + Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));
//                    }

//                }

//                if (strPageItemAccountID == "" && Request.QueryString["ColumnID"] != null)
//                {
//                    strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Table] T 
//                INNER JOIN [Column] C ON T.TableID=C.TableID  WHERE C.ColumnID=" + Cryptography.Decrypt(Request.QueryString["ColumnID"].ToString()));
//                }

//                if (strPageItemAccountID == "" && Request.QueryString["DocumentID"] != null)
//                {
//                    iTemp = 0;
//                    if (int.TryParse(Request.QueryString["DocumentID"].ToString(), out iTemp))
//                    {
//                        strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Document]
//                            WHERE DocumentID=" + Request.QueryString["DocumentID"].ToString());
//                    }
//                    else
//                    {
//                        strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Document]
//                            WHERE DocumentID=" + Cryptography.Decrypt(Request.QueryString["DocumentID"].ToString()));
//                    }

//                }

//                if (strPageItemAccountID == "" && Request.QueryString["GraphOptionID"] != null)
//                {
//                    strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [GraphOption]
//                            WHERE GraphOptionID=" + Cryptography.Decrypt(Request.QueryString["GraphOptionID"].ToString()));
//                }

//                if (strPageItemAccountID == "" && Request.QueryString["MenuID"] != null)
//                {
//                    strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Menu] 
//                            WHERE MenuID=" + Cryptography.Decrypt(Request.QueryString["MenuID"].ToString()));
//                }

//                if (strPageItemAccountID == "" && Request.QueryString["TerminologyID"] != null)
//                {
//                    strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Terminology] 
//                            WHERE TerminologyID=" + Cryptography.Decrypt(Request.QueryString["TerminologyID"].ToString()));
//                }
//                if (strPageItemAccountID == "" && Request.QueryString["UploadID"] != null)
//                {
//                    strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Table] T INNER JOIN [Upload] U ON 
//                            T.TableID=U.TableID  WHERE U.UploadID=" + Cryptography.Decrypt(Request.QueryString["UploadID"].ToString()));
//                }


//                if (strPageItemAccountID == "" && Request.QueryString["AccountID"] != null
//                    && Common.HaveAccess(Session["roletype"].ToString(), "1") == false)
//                {
//                    strPageItemAccountID = Cryptography.Decrypt(Request.QueryString["AccountID"].ToString());
//                }


//                if (strPageItemAccountID != "")
//                {
//                    if (int.Parse(Session["AccountID"].ToString()) != int.Parse(strPageItemAccountID))
//                    {
//                        //different account
//                        User loggedUser = (User)Session["User"];
//                        if (loggedUser != null)
//                        {
//                            if (Common.ChangeAccount((int)loggedUser.UserID, int.Parse(strPageItemAccountID)))
//                            {
//                                try
//                                {
//                                    Response.Redirect(Request.RawUrl, true);
//                                    return;
//                                }
//                                catch
//                                {
//                                    return;
//                                }

//                            }
//                        }

//                        Response.Redirect("~/Empty.aspx", true);
//                        return;

//                    }
//                }


//            }
//            catch (Exception ex)
//            {
//                //unknown
//                //ErrorLog theErrorLog = new ErrorLog(null, "Secure page - wrong account", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
//                //SystemData.ErrorLog_Insert(theErrorLog);
//            }


//        }

//    }
    protected override void OnPreInit(EventArgs e)
    {

       

        bool bGoHome = false;
        if (this.ValidateUser()==false)
        {
            bGoHome = true;
        }
        base.OnPreInit(e);

        if (Session["AccountID"] != null)
        {

            if (!IsPostBack)//&& Session["roletype"] != null && Session["roletype"] != "1"
            {
                Common.FindTheAccount();
            }

            //FindTheAccount();

            Account theAccount;
            theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
            if ( theAccount!=null)
            {
                if (Page.MasterPageFile!=null && theAccount.MasterPage != "" && Page.MasterPageFile != "")
                {

                    if (Page.MasterPageFile.Substring(Page.MasterPageFile.LastIndexOf("/") + 1) == "Home.master")
                    {
                        Page.MasterPageFile = theAccount.MasterPage;
                    }

                    if (Page.MasterPageFile.Substring(Page.MasterPageFile.LastIndexOf("/") + 1) == "Popup.master")
                    {
                        string strPop = theAccount.MasterPage.Substring(theAccount.MasterPage.LastIndexOf("/") + 1);
                        Page.MasterPageFile = "~/Home/Pop" + strPop;
                    }

                }
            }
        }
        else
        {

            bGoHome = true;
        }

        if(bGoHome)
        {
            if (Session["LoginAccount"] == null)
            {
                Session.Clear();
               base.Response.Redirect(base.Request.Url.Scheme +"://" + base.Request.Url.Authority + base.Request.ApplicationPath + "/Login.aspx?ReturnURL=" + Server.UrlEncode(base.Request.RawUrl), false);
                
            }
            else
            {
                string strLoginAccount = Session["LoginAccount"].ToString();
                Session.Clear();
                base.Response.Redirect(base.Request.Url.Scheme + "://" + base.Request.Url.Authority + base.Request.ApplicationPath + "/Login.aspx?ReturnURL=" + Server.UrlEncode(base.Request.RawUrl) + "&" + strLoginAccount, false);
               
            }
            return;
        }

        
//put speed test here       

        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            SecurityManager.AddSpeedLog(theSpeedLog);
        }
    }

    protected bool ValidateUser()
    {
        if(Session["User"]==null)
        {            
            return false;
        }
               
        User loggedUser = (User)Session["User"];
        //if ((loggedUser == null) || (loggedUser.UserRoles.Count < 1))
        if (loggedUser == null)
        {
            //loggedUser = null;
            return false;
        }
        return true;
    }

  

    public enum UserType
    {
        Administrator = 1,
        Public = 2
    }
}
