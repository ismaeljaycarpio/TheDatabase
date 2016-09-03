<%@ Application Language="C#" %>

<script runat="server">
      

    //void Application_BeginRequest(Object sender, EventArgs e)
    //{

    //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
    //    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
    //}                             
    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        //// Code that runs when an unhandled error occurs        
        //Exception exc = Server.GetLastError();

        //if (exc != null)
        //{
        //    Exception ex = exc.GetBaseException();
        //    if (ex != null)
        //    {
        //        ErrorLog theErrorLog = new ErrorLog(null, "The DB- Global.asax", ex.Message, ex.StackTrace, DateTime.Now, HttpContext.Current.Request.Url.ToString());
        //       int iErrroLogID= SystemData.ErrorLog_Insert(theErrorLog);
                                  
                
        //        //now send the email   
        //       string strAccountInfo = "</br> No Account and user info";
                
        //        try
        //        {
        //            if (HttpContext.Current.Session != null && HttpContext.Current.Session["AccountID"] != null)
        //            {
        //                strAccountInfo = " </br> A ID: ";
                        
        //                strAccountInfo = strAccountInfo + HttpContext.Current.Session["AccountID"].ToString();

        //                if (HttpContext.Current.Session["User"] != null)
        //                {
        //                    User theUser = (User)HttpContext.Current.Session["User"];
        //                    if(theUser!=null)
        //                    {
        //                        strAccountInfo = strAccountInfo + " </br>and U ID: " + theUser.UserID.ToString();
        //                    }
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            //
        //        }
               
                
        //       string strError = "";
        //       string strTo = SystemData.SystemOption_ValueByKey_Account("ErrorMessageEmail",-1,null);
        //       foreach (string sEach in strTo.Split(','))
        //       {
        //           if(sEach!="" && sEach.Length>3)
        //           {
        //               Common.SendSingleEmail(sEach, "TheDB Error - " + ex.Message + " Path=" + HttpContext.Current.Request.Url.ToString(),
        //            "<a href='" + HttpContext.Current.Request.Url.ToString() + "'>" + HttpContext.Current.Request.Url.ToString() + "</a></br>"
        //            + ex.Message + " </br> Error Stack: " + ex.StackTrace + strAccountInfo, ref strError);
        //           }
        //       }
                

        //        //DBGurus.SendEmail("TheDB Error - " + ex.Message + " Path=" + HttpContext.Current.Request.Url.ToString(),
        //        //    "<a href='" + HttpContext.Current.Request.Url.ToString() + "'>" + HttpContext.Current.Request.Url.ToString() + "</a></br>" + ex.Message + " </br> Error Stack: " + ex.StackTrace,
        //        //    "", strTo, "", "", out strError);

        //        Server.ClearError();

        //        //if (HttpContext.Current.Session["LoginAccount"] == null)
        //        //{
        //        Response.Redirect("~/ErrorRedirect.aspx?ErrorLogID=" + Cryptography.Encrypt(iErrroLogID.ToString()),false);
        //        //}
        //        //else
        //        //{
        //        //    Response.Redirect("~/Login.aspx?ErrorLogID=" + Cryptography.Encrypt(iErrroLogID.ToString()) + "&" + HttpContext.Current.Session["LoginAccount"].ToString());
        //        //}

        //        return;
        //    } 

        //}

        //Server.ClearError();

        ////Response.Redirect("~/Login.aspx");

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

        // Redirect mobile users to the mobile home page
        //HttpRequest httpRequest = HttpContext.Current.Request;
        //if (httpRequest.Browser.IsMobileDevice)
        //{
        //    HttpContext.Current.Response.Redirect("~/Pages/Mobile/Login.aspx");
        //    string path = httpRequest.Url.PathAndQuery;
        //    //bool isOnMobilePage = path.StartsWith("/Mobile/",
        //    //                       StringComparison.OrdinalIgnoreCase);
        //    //if (!isOnMobilePage)
        //    //{
        //    //    string redirectTo = "~/Pages/Mobile/Default.aspx";

        //    //    // Could also add special logic to redirect from certain 
        //    //    // recognized pages to the mobile equivalents of those 
        //    //    // pages (where they exist). For example,
        //    //    // if (HttpContext.Current.Handler is UserRegistration)
        //    //    //     redirectTo = "~/Mobile/Register.aspx";

        //    //    HttpContext.Current.Response.Redirect(redirectTo);
        //    //}
        //}

        

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    //protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
    //{
    //    if (Session["UserID"] != null)
    //    {
    //        string strCacheKey = Session["UserID"].ToString();
    //        string strUser = HttpContext.Current.Cache[strCacheKey].ToString();
    //    }
    //}


       
</script>
