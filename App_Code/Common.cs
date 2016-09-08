using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Net.Mail;
using System.Web.UI.WebControls;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml;
using System.Globalization;
using DocGen.DAL;

/// <summary>
/// Summary description for Common
/// </summary>


public struct CommonStruct
{
    public static string DefaultValue_Login_Fixed = "--login--";
}
public struct WarningMsg
{
   
    public static string MaxtimebetweenRecords = "Max time between Records exceeded";
}
public class Common
{
    public Common()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    //public bool bUseSourceBacthForALS = true;

    public static DataTable GetUsersByDashboard(string strUserIDs, int iAccountID)
    {
        if (strUserIDs == "")
            strUserIDs = "-1";
        
            DataTable dtUserDash = Common.DataTableFromText(@"SELECT U.* ,R.[Role]
                FROM dbo.Account A
                JOIN [UserRole]  UR ON UR.AccountID = A.AccountID 
                JOIN [User] U ON U.UserID = UR.UserID 
                JOIN [Role] R ON UR.RoleID=R.RoleID
                WHERE A.AccountID=" + iAccountID.ToString() +
                          @" AND  U.UserID IN (" + strUserIDs + @") ORDER BY R.[Role],U.FirstName,U.LastName");
            return dtUserDash;
       
    }
    public static string GetUserIDsForDashboard(int iDocumentID,int iAccountID)
    {
        string strUserIDs = "";
        try
        {
           
            DataTable dtUsers = Common.DataTableFromText(@"SELECT U.UserID,UR.RoleID
                                FROM dbo.Account A
                                JOIN [UserRole]  UR ON UR.AccountID = A.AccountID 
                                JOIN [User] U ON U.UserID = UR.UserID 
                                WHERE U.IsActive=1 AND  A.AccountID=" + iAccountID.ToString());


            foreach (DataRow dr in dtUsers.Rows)
            {
                int? DashID = DocumentManager.dbg_Dashboard_BestFitting("", int.Parse(dr["UserID"].ToString()), int.Parse(dr["RoleID"].ToString()));

                if (DashID != null && (int)DashID == iDocumentID)
                {
                    strUserIDs = strUserIDs + dr["UserID"].ToString() + ",";
                }
            }


            
        }
        catch
        {
            //
        }
        if (strUserIDs != "")
            strUserIDs = strUserIDs.Substring(0, strUserIDs.Length - 1);
        return strUserIDs;
    }
    public static DateTime? GetDateTimeFromString(string strDateTime,string strFormat)
    {
        if (strFormat == "" || strFormat == "GB")
        {
            try
            {
                DateTime dateValue;
                if (DateTime.TryParseExact(strDateTime, DateTimeformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dateValue))
                {
                    return dateValue;
                }
                else
                {
                    //old
                    DateTime? oDateTime = null;
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-GB");
                    if (strDateTime.IndexOf(" ") > 0)
                    {
                        if (strDateTime.Substring(0, strDateTime.IndexOf(" ")).Length < 7)
                        {
                            string strTempDateTime = strDateTime.Substring(0, strDateTime.IndexOf(" ")) + "-" + DateTime.Now.Year.ToString() + " " + strDateTime.Substring(strDateTime.IndexOf(" ") + 1);
                            oDateTime = Convert.ToDateTime(strTempDateTime, culture);
                        }
                        else
                        {
                            if (strDateTime.Length == 16)
                            {
                                //oDateTime = DateTime.ParseExact(strDateTime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                                oDateTime = DateTime.ParseExact(strDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                oDateTime = Convert.ToDateTime(strDateTime, culture);
                            }
                        }
                    }
                    else
                    {
                        oDateTime = Convert.ToDateTime(strDateTime, culture);
                    }
                    return oDateTime;
                }
            }
            catch
            {
                try
                {
                    return Convert.ToDateTime(strDateTime);
                }
                catch
                {
                    //
                }
            }
            
        }
        return null;
         
    }

    public static void FindTheAccount()
    {
       
            try
            {
                   if (HttpContext.Current != null && HttpContext.Current.Session!=null)
                   {

                       int iTemp = 0;
                       string strPageItemAccountID = "";

                       if (strPageItemAccountID == "" && HttpContext.Current.Request.QueryString["TableID"] != null)
                       {
                           iTemp = 0;
                           if (int.TryParse(HttpContext.Current.Request.QueryString["TableID"].ToString(), out iTemp))
                           {
                               strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Table] T 
                              WHERE T.TableID=" + HttpContext.Current.Request.QueryString["TableID"].ToString());
                           }
                           else
                           {
                               strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Table] T 
                              WHERE T.TableID=" + Cryptography.Decrypt(HttpContext.Current.Request.QueryString["TableID"].ToString()));
                           }

                       }

                       if (strPageItemAccountID == "" && HttpContext.Current.Request.QueryString["ColumnID"] != null)
                       {
                           strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Table] T 
                INNER JOIN [Column] C ON T.TableID=C.TableID  WHERE C.ColumnID=" + Cryptography.Decrypt(HttpContext.Current.Request.QueryString["ColumnID"].ToString()));
                       }

                       if (strPageItemAccountID == "" && HttpContext.Current.Request.QueryString["DocumentID"] != null)
                       {
                           iTemp = 0;
                           if (int.TryParse(HttpContext.Current.Request.QueryString["DocumentID"].ToString(), out iTemp))
                           {
                               strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Document]
                            WHERE DocumentID=" + HttpContext.Current.Request.QueryString["DocumentID"].ToString());
                           }
                           else
                           {
                               strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Document]
                            WHERE DocumentID=" + Cryptography.Decrypt(HttpContext.Current.Request.QueryString["DocumentID"].ToString()));
                           }

                       }

                       if (strPageItemAccountID == "" && HttpContext.Current.Request.QueryString["GraphOptionID"] != null)
                       {
                           strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [GraphOption]
                            WHERE GraphOptionID=" + Cryptography.Decrypt(HttpContext.Current.Request.QueryString["GraphOptionID"].ToString()));
                       }

                       if (strPageItemAccountID == "" && HttpContext.Current.Request.QueryString["MenuID"] != null)
                       {
                           strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Menu] 
                            WHERE MenuID=" + Cryptography.Decrypt(HttpContext.Current.Request.QueryString["MenuID"].ToString()));
                       }

                       if (strPageItemAccountID == "" && HttpContext.Current.Request.QueryString["TerminologyID"] != null)
                       {
                           strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Terminology] 
                            WHERE TerminologyID=" + Cryptography.Decrypt(HttpContext.Current.Request.QueryString["TerminologyID"].ToString()));
                       }
                       if (strPageItemAccountID == "" && HttpContext.Current.Request.QueryString["UploadID"] != null)
                       {
                           strPageItemAccountID = Common.GetValueFromSQL(@"SELECT AccountID FROM [Table] T INNER JOIN [Upload] U ON 
                            T.TableID=U.TableID  WHERE U.UploadID=" + Cryptography.Decrypt(HttpContext.Current.Request.QueryString["UploadID"].ToString()));
                       }


                       if (strPageItemAccountID == "" && HttpContext.Current.Request.QueryString["AccountID"] != null
                           && Common.HaveAccess(HttpContext.Current.Session["roletype"].ToString(), "1") == false)
                       {
                           strPageItemAccountID = Cryptography.Decrypt(HttpContext.Current.Request.QueryString["AccountID"].ToString());
                       }


                       if (strPageItemAccountID != "")
                       {
                           if (int.Parse(HttpContext.Current.Session["AccountID"].ToString()) != int.Parse(strPageItemAccountID))
                           {
                               //different account
                               User loggedUser = (User)HttpContext.Current.Session["User"];
                               if (loggedUser != null)
                               {
                                   if (Common.HaveAccess(HttpContext.Current.Session["roletype"].ToString(), "1") == true)
                                   {
                                       HttpContext.Current.Session["AccountID"] = strPageItemAccountID;
                                       HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl, true);
                                       return;
                                   }


                                   if (Common.ChangeAccount((int)loggedUser.UserID, int.Parse(strPageItemAccountID),true))
                                   {
                                       try
                                       {
                                           HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl, true);
                                           return;
                                       }
                                       catch
                                       {
                                          // return;
                                       }

                                   }
                               }

                               HttpContext.Current.Response.Redirect("~/Empty.aspx", true);
                               return;

                           }
                       }
                   }
                


            }
            catch (Exception ex)
            {
                //unknown
                //ErrorLog theErrorLog = new ErrorLog(null, "Secure page - wrong account", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                //SystemData.ErrorLog_Insert(theErrorLog);
            }

    }
    public static bool ChangeAccount(int iUserID,int iAccountID,bool bClearSession)
    {
       

        try
        {
            if (HttpContext.Current != null && HttpContext.Current.Session!=null)
            {
                //User etUser = (User)HttpContext.Current.Session["User"];
                User etUser = SecurityManager.User_Details(iUserID);
                if (bClearSession)
                 HttpContext.Current.Session.Clear();

                HttpContext.Current.Session["User"] = etUser;
                Account theAccount = SecurityManager.Account_Details(iAccountID);
                string roletype = "";
                roletype = SecurityManager.GetUserRoleTypeID(iUserID, iAccountID);
              
                UserRole theUserRole = SecurityManager.GetUserRole(iUserID, iAccountID);
                if (theUserRole == null)
                    return false;
                string strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation", null, null);
                string strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath", null, null);

                if (strFilesLocation != "")
                {
                    HttpContext.Current.Session["FilesLocation"] = strFilesLocation;
                }
                else
                {
                    HttpContext.Current.Session["FilesLocation"] = HttpContext.Current.Request.Url.Scheme +"://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
                }


                if (strFilesPhisicalPath != "")
                {
                    HttpContext.Current.Session["FilesPhisicalPath"] = strFilesPhisicalPath;
                }
                else
                {
                    HttpContext.Current.Session["FilesPhisicalPath"] = HttpContext.Current.Server.MapPath("~");
                }
                HttpContext.Current.Session["roletype"] = roletype;
                HttpContext.Current.Session["AccountID"] = iAccountID.ToString();
                HttpContext.Current.Session["UserRole"] = theUserRole;
                HttpContext.Current.Session["client"] = iAccountID.ToString();
                if ((bool)theUserRole.IsAdvancedSecurity)
                {
                    //Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)_objUser.UserID, "-1,3,4,5,7,8,9");
                    HttpContext.Current.Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)theUserRole.RoleID);                   
                }
                else
                {
                    HttpContext.Current.Session["STs"] = "";
                    if (Common.HaveAccess(HttpContext.Current.Session["roletype"].ToString(), Common.UserRoleType.None))
                    {
                        HttpContext.Current.Session["STs"] = "-1";
                    }
                }

                try
                {
                    HttpContext.Current.Session["DoNotAllow"] = null;
                  
                    if (SecurityManager.IsRecordsExceeded(iAccountID))
                    {
                        HttpContext.Current.Session["DoNotAllow"] = "true";                       
                    }
                   

                }
                catch
                {
                    //
                }
                
                HttpContext.Current.Session["GridPageSize"] = SystemData.SystemOption_ValueByKey_Account("GridPageSize", iAccountID, null);
                            
                HttpContext.Current.Session["DoNotAllow"] = null;
               
                if (SecurityManager.IsRecordsExceeded(iAccountID))
                {
                    HttpContext.Current.Session["DoNotAllow"] = "true";
                }
                
                if (theAccount != null && (bool)theAccount.IsActive)
                {
                    //HttpContext.Current.Session["tdbmsg"] = "Your are now in "+theAccount.AccountName+" account.";
                    return true;
                }
          
            }

            
        }
        catch
        {
            //
        }       
      

        return false;
    }
    public static bool HasRecord(string strSQL)
    {
      if(!string.IsNullOrEmpty(strSQL))
      {
          string strRecordValue = GetValueFromSQL(strSQL);
          if (!string.IsNullOrEmpty(strRecordValue))
              return true;

      }
        return false;
    }

    //Import Template
    public static string LongDateWithTimeStringFormat = @"dd\/MM\/yyyy h\:mm tt";

    public static string ToolTip_Today = @"You can use TODAY keyword; e.g TODAY - 10, TODAY, TODAY + 20 etc.";
    public static int? GetDefaultImportTemplate(int? iTableID)
    {
        if (iTableID == null)
            return null;
        try
        {
            string strID = Common.GetValueFromSQL("SELECT  TOP 1 ImportTemplateID  FROM ImportTemplate WHERE TableID=" + iTableID.ToString() 
                + " AND ImportTemplateName='Default' ORDER BY ImportTemplateName");

            if(strID=="")
            {
                strID = Common.GetValueFromSQL("SELECT  TOP 1 ImportTemplateID  FROM ImportTemplate WHERE TableID=" + iTableID.ToString() + "  ORDER BY ImportTemplateName");
            }
            if(strID!="")
            {
                return int.Parse(strID);
            }
        }
        catch
        {
            //
        }
      
        return null;
    }

    public static bool SO_ImportTemplateAsDefault(int? iAccountID, int? iTableID)
    {
        string strOptionValue = SystemData.SystemOption_ValueByKey_Account("Import Template As Default", iAccountID, iTableID);
        if (strOptionValue != "" && strOptionValue.ToLower() == "no")
        {
            return false;
        }
        return true;
    }

    public static bool SO_ShowExceedances(int? iAccountID, int? iTableID)
    {
        string strOptionValue = SystemData.SystemOption_ValueByKey_Account("Show Exceedances", iAccountID, iTableID);
        if (strOptionValue != "" && strOptionValue.ToLower() == "no")
        {
            return false;
        }
        return true;
    }

    //END of Import Template 

    public static bool SO_SearchAllifToIsNull(int? iAccountID, int? iTableID)
    {

        string strOptionValue = SystemData.SystemOption_ValueByKey_Account("Use Lower Limit As Minimum When Upper Empty", iAccountID, iTableID);
        if (strOptionValue != "" && strOptionValue.ToLower() == "yes")
        {
            return true;
        }
        return false;
    }

    public static bool SO_ShowRecordFirstLastButtons(int? iAccountID, int? iTableID)
    {
        string strOptionValue = SystemData.SystemOption_ValueByKey_Account("Show Record First-Last Buttons", iAccountID, iTableID);
        if (strOptionValue != "" && strOptionValue.ToLower() == "yes")
        {
            return true;
        }
        return false;
    }


    public static string GetDatabaseName()
    {
        string strDBName = "";

        strDBName = Common.GetValueFromSQL("SELECT DB_NAME()");

        return strDBName;
    }
    public static string GetValidFileName(string strFileName)
    {
        string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        foreach (char c in invalidChars)
        {
            strFileName = strFileName.Replace(c.ToString(), "_"); // or with "."
        }
        return strFileName;
    }
    public static string RecordExceededMessage = @"You have exceeded the number of records allowed. Please contact DB Gurus if you wish to extend your plan. Alternatively you can permanently delete records.";
    public static string ReturnDateStringFromToken(string strDateToken)
    {
        string strTempDateToken = strDateToken.Trim().ToLower();
        if (strTempDateToken == "today")
        {
            return DateTime.Today.ToShortDateString();
        }

        if (strTempDateToken.IndexOf("today") > -1)
        {
            strTempDateToken = strTempDateToken.Replace("today", "");

            if (strTempDateToken.IndexOf("+") > -1)
            {
                strTempDateToken = strTempDateToken.Replace("+", "");
                strTempDateToken = strTempDateToken.Trim();
                int iTotalDayAdd = 0;

                if (int.TryParse(strTempDateToken, out iTotalDayAdd))
                {
                    return DateTime.Today.AddDays(iTotalDayAdd).ToShortDateString();
                }
            }

            if (strTempDateToken.IndexOf("-") > -1)
            {
                strTempDateToken = strTempDateToken.Replace("-", "");
                strTempDateToken = strTempDateToken.Trim();
                int iTotalDayAdd = 0;

                if (int.TryParse(strTempDateToken, out iTotalDayAdd))
                {
                    return DateTime.Today.AddDays(-iTotalDayAdd).ToShortDateString();
                }
            }

        }



        return strDateToken;
    }

    public static  string EncodeTo64(string toEncode)
    {
        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
        string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
        return returnValue;
    }

    public static string DecodeFrom64(string encodedData)
    {
        byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
        string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
        return returnValue;
    }

   public static int DaysBetween(DateTime d1, DateTime d2)
    {
        TimeSpan span = d2.Subtract(d1);
        return (int)span.TotalDays;
    }

    public static string[] Dateformats = {"d/M/yyyy",
                                    "dd/MM/yyyy","dd/M/yyyy","d/M/yy","dd/MM/yyyy","dd/M/yy","d/MM/yyyy","d/MM/yy"};

    public static string[] DateTimeformats = {"d/M/yyyy hh:mm","dd/MM/yyyy hh:mm","dd/M/yyyy hh:mm","d/M/yy hh:mm","dd/MM/yyyy hh:mm","d/MM/yyyy hh:mm","d/MM/yy hh:mm",
                                             "d/M/yyyy hh:mm:ss","dd/MM/yyyy hh:mm:ss","dd/M/yyyy hh:mm:ss","d/M/yy hh:mm:ss","dd/MM/yyyy hh:mm:ss",
                                             "d/MM/yyyy hh:mm:ss","d/MM/yy hh:mm:ss",
                                             "d/M/yyyy hh:mm tt","dd/MM/yyyy hh:mm tt","dd/M/yyyy hh:mm tt","d/M/yy hh:mm tt","dd/MM/yyyy hh:mm tt",
                                             "d/MM/yyyy hh:mm tt","d/MM/yy hh:mm tt",
                                             "d/M/yyyy hh:mm:ss tt","dd/MM/yyyy hh:mm:ss tt","dd/M/yyyy hh:mm:ss tt","d/M/yy hh:mm:ss","dd/MM/yyyy hh:mm:ss tt",
                                             "d/MM/yyyy hh:mm:ss  tt","d/MM/yy hh:mm:ss tt",
                                             "d/M/yyyy h:mm","dd/MM/yyyy h:mm","dd/M/yyyy h:mm","d/M/yy h:mm","dd/MM/yyyy h:mm","d/MM/yyyy h:mm","d/MM/yy h:mm",
                                             "d/M/yyyy h:mm:ss","dd/MM/yyyy h:mm:ss","dd/M/yyyy h:mm:ss","d/M/yy h:mm:ss","dd/MM/yyyy h:mm:ss",
                                             "d/MM/yyyy h:mm:ss","d/MM/yy h:mm:ss",
                                             "d/M/yyyy h:mm tt","dd/MM/yyyy h:mm tt","dd/M/yyyy h:mm tt","d/M/yy h:mm tt","dd/MM/yyyy h:mm tt",
                                             "d/MM/yyyy h:mm tt","d/MM/yy h:mm tt",
                                             "d/M/yyyy h:mm:ss tt","dd/MM/yyyy h:mm:ss tt","dd/M/yyyy h:mm:ss tt","d/M/yy h:mm:ss","dd/MM/yyyy h:mm:ss tt",
                                             "d/MM/yyyy h:mm:ss  tt","d/MM/yy h:mm:ss tt",
                                             "d/M/yyyy hh:m","dd/MM/yyyy hh:m","dd/M/yyyy hh:m","d/M/yy hh:m","dd/MM/yyyy hh:m","d/MM/yyyy hh:m","d/MM/yy hh:m",
                                             "d/M/yyyy hh:m:ss","dd/MM/yyyy hh:m:ss","dd/M/yyyy hh:m:ss","d/M/yy hh:m:ss","dd/MM/yyyy hh:m:ss",
                                             "d/MM/yyyy hh:m:ss","d/MM/yy hh:m:ss",
                                             "d/M/yyyy hh:m tt","dd/MM/yyyy hh:m tt","dd/M/yyyy hh:m tt","d/M/yy hh:m tt","dd/MM/yyyy hh:m tt",
                                             "d/MM/yyyy hh:m tt","d/MM/yy hh:m tt",
                                             "d/M/yyyy hh:m:ss tt","dd/MM/yyyy hh:m:ss tt","dd/M/yyyy hh:m:ss tt","d/M/yy hh:m:ss","dd/MM/yyyy hh:m:ss tt",
                                             "d/MM/yyyy hh:m:ss  tt","d/MM/yy hh:m:ss tt",
                                             "d/M/yyyy h:m","dd/MM/yyyy h:m","dd/M/yyyy h:m","d/M/yy h:m","dd/MM/yyyy h:m","d/MM/yyyy h:m","d/MM/yy h:m",
                                             "d/M/yyyy h:m:ss","dd/MM/yyyy h:m:ss","dd/M/yyyy h:m:ss","d/M/yy h:m:ss","dd/MM/yyyy h:m:ss",
                                             "d/MM/yyyy h:m:ss","d/MM/yy h:m:ss",
                                             "d/M/yyyy h:m tt","dd/MM/yyyy h:m tt","dd/M/yyyy h:m tt","d/M/yy h:m tt","dd/MM/yyyy h:m tt",
                                             "d/MM/yyyy h:m tt","d/MM/yy h:m tt",
                                             "d/M/yyyy h:m:ss tt","dd/MM/yyyy h:m:ss tt","dd/M/yyyy h:m:ss tt","d/M/yy h:m:ss","dd/MM/yyyy h:m:ss tt",
                                             "d/MM/yyyy h:m:ss  tt","d/MM/yy h:m:ss tt"};

    public static string DemoEmail="demo@carbonmonitoring.com.au";
    public static string DemoReadyOnlyMsg = "Demo user is read only.";
    public static string  MenuDividerText="---";
    //public static int _iMaxRecordsExport = 5;
    public static int MinSTDEVRecords = 3;
    public static int MaxGraphRecords = 200; //1000
    public static int MaxRowForListBoxTable = 1000; //1000
    public static string NumberRegExDC = @"\[(.*?)\]"; //@"^.*?\([^\d]*(\d+)[^\d]*\).*$";
    public static string AusMobileRegEx = @"^04[0-9]{2}\s?([0-9]{3}\s?[0-9]{3}|[0-9]{2}\s?[0-9]{2}\s?[0-9]{2})$"; 
//@"/^(?:\+?61|0)4(?:[01]\d{3}|(?:2[1-9]|3[0-57-9]|4[7-9]|5[0-15-9]|6[679]|7[3-8]|8[1478]|9[07-9])\d{2}|(?:20[2-9]|444|52[0-6]|68[3-9]|70[0-7]|79[01]|820|890|91[0-4])\d|(?:200[0-3]|201[01]|8984))\d{4}$/";  // @"^((61|\+61)?\s?)04[0-9]{2}\s?([0-9]{3}\s?[0-9]{3}|[0-9]{2}\s?[0-9]{2}\s?[0-9]{2})$";
    public static int MaxRecordsExport(int? iAccountID,int? iTableID)
    {
       

            return int.Parse(SystemData.SystemOption_ValueByKey_Account("MaxRecordsExport",iAccountID,iTableID));         
        
      
    }

    public static string GetUpdatedFullURLWithQueryString(string strFullURL, string theQueryString, string theNewValue)
    {
        var nameValues = HttpUtility.ParseQueryString(strFullURL);
        nameValues.Set(theQueryString, theNewValue);
        string strReturnURL = nameValues.ToString();
        strReturnURL = HttpContext.Current.Server.UrlDecode(strReturnURL);

        return strReturnURL;
    }
    public static string GetUpdatedFullURLRemoveQueryString(string strFullURL, string theRemoveQueryString)
    {
        try
        {
            if(strFullURL.IndexOf('?')>-1)
            {
                string strBase = strFullURL.Substring(0, strFullURL.IndexOf('?') + 1);
                var nameValues = HttpUtility.ParseQueryString(strFullURL.Substring(strFullURL.IndexOf('?') + 1));
                nameValues.Remove(theRemoveQueryString);
                string strReturnURL = nameValues.ToString();
                strReturnURL = HttpContext.Current.Server.UrlDecode(strReturnURL);

                return strBase + strReturnURL;
            }
          
        }
        catch
        {
            //
        }

        return strFullURL;
    }

    public static bool IsThisDouble(string strValue)
    {
        try
        {
            if (strValue.ToLower() == "nan")
            {
                return false;
            }

            double dTest = double.Parse(strValue);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string MakeDecimal(string strValue)
    {
        if (strValue.IndexOf(".", 0) == -1)
        {
            return strValue + ".0";
        }
        return strValue;
    }


    public  static string  FixCrLfAndOtherNonPrint(string value)
    {

        if (string.IsNullOrEmpty(value)) { return string.Empty; }


        return value.Replace(Environment.NewLine, "<br />").Replace("\n", "<br/>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
    }


    public static DateTime PreviousMonday(DateTime dt)
    {
        var dateDayOfWeek = (int)dt.DayOfWeek;
        if (dateDayOfWeek == 0)
        {
            dateDayOfWeek = dateDayOfWeek + 7;
        }
        var alterNumber = dateDayOfWeek - ((dateDayOfWeek * 2) - 1);

        return dt.AddDays(alterNumber);
    }


    public static string GetSystemFromDisplay(string strDisplay, int iTableID)
    {

        if(strDisplay!="")
        {


            DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,DisplayName,ColumnType FROM [Column] WHERE TableID=" + iTableID.ToString());

            foreach(DataRow dr in dtTemp.Rows)
            {
                if (dr["ColumnType"].ToString() == "number" || dr["ColumnType"].ToString() == "dropdown")
                {
                    strDisplay = strDisplay.Replace("[" + dr["DisplayName"].ToString() + "]", "CAST(dbo.RemoveNonNumericChar([" + dr["SystemName"].ToString() + "]) as decimal(20,10))");
                }
                else if (dr["ColumnType"].ToString() == "date")
                {
                    strDisplay = strDisplay.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");
                }
                else
                {
                    strDisplay = strDisplay.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");
                } 

            }



            //Work with 1 top level Parent tables.
            DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + iTableID.ToString()); //AND DetailPageType<>'not'

            if (dtPT.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPT.Rows)
                {
                    DataTable dtPColumns = Common.DataTableFromText(@"SELECT distinct TableName + ':' + DisplayName AS DP,TableName + ':' +  SystemName AS SP,ColumnType
                                            FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE IsStandard=0 AND TableTableID IS NULL AND  [Column].TableID=" + dr["ParentTableID"].ToString());
                    foreach (DataRow drp in dtPColumns.Rows)
                    {
                        if (drp["ColumnType"].ToString() == "number" || drp["ColumnType"].ToString() == "dropdown")
                        {
                            strDisplay = strDisplay.Replace("[" + drp["DP"].ToString() + "]", "CAST(dbo.RemoveNonNumericChar([" + drp["SP"].ToString() + "]) as decimal(20,10))");
                        }
                        else if (drp["ColumnType"].ToString() == "date")
                        {
                            strDisplay = strDisplay.Replace("[" + drp["DP"].ToString() + "]", "[" + drp["SP"].ToString() + "]");
                        }
                        else
                        {
                            strDisplay = strDisplay.Replace("[" + drp["DP"].ToString() + "]", "[" + drp["SP"].ToString() + "]");
                        }

                    }
                }
            }

        }

        return strDisplay;

    }


    public static string GetDisplayFromSystem(string strSystem, int iTableID)
    {

        if (strSystem != "")
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE TableID=" + iTableID.ToString());

            foreach (DataRow dr in dtTemp.Rows)
            {
                strSystem = strSystem.Replace("CAST(dbo.RemoveNonNumericChar([" + dr["SystemName"].ToString() + "]) as decimal(20,10))", "[" + dr["DisplayName"].ToString() + "]");

               // strSystem = strSystem.Replace("CONVERT(Datetime,[" + dr["SystemName"].ToString() + "],103)", "[" + dr["DisplayName"].ToString() + "]");

                strSystem = strSystem.Replace("[" + dr["SystemName"].ToString() + "]", "[" + dr["DisplayName"].ToString() + "]");
                
            }


            //Work with 1 top level Parent tables.
            DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + iTableID.ToString()); //AND DetailPageType<>'not'
            if (dtPT.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPT.Rows)
                {
                    DataTable dtPColumns = Common.DataTableFromText(@"SELECT distinct TableName + ':' + DisplayName AS DP,TableName + ':' +  SystemName AS SP,ColumnType
                                            FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE IsStandard=0 AND TableTableID IS NULL AND  [Column].TableID=" + dr["ParentTableID"].ToString());
                    foreach (DataRow drp in dtPColumns.Rows)
                    {

                        strSystem = strSystem.Replace("CAST(dbo.RemoveNonNumericChar([" + drp["SP"].ToString() + "]) as decimal(20,10))", "[" + drp["DP"].ToString() + "]");

                        strSystem = strSystem.Replace("[" + drp["SP"].ToString() + "]", "[" + drp["DP"].ToString() + "]");

                    }
                }

            }



        }

        return strSystem;

    }


    public static string EvaluateCalculationFormula(string expression)
    {
        try
        {
            var loDataTable = new DataTable();
            var loDataColumn = new DataColumn("Eval", typeof(double), expression);
            loDataTable.Columns.Add(loDataColumn);
            loDataTable.Rows.Add(0);
            return ((double)(loDataTable.Rows[0]["Eval"])).ToString();
        }
        catch
        {
            return "";
        }
        
    }
    public static string GetCalculationSystemNameOnly(string strSystem, int iTableID)
    {

        if (strSystem != "")
        {
            DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE TableID=" + iTableID.ToString());

            foreach (DataRow dr in dtTemp.Rows)
            {
                strSystem = strSystem.Replace("CAST(dbo.RemoveNonNumericChar([" + dr["SystemName"].ToString() + "]) as decimal(20,10))", "[" + dr["SystemName"].ToString() + "]");

            }


            //Work with 1 top level Parent tables.
            DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + iTableID.ToString()); //AND DetailPageType<>'not'
            if (dtPT.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPT.Rows)
                {
                    DataTable dtPColumns = Common.DataTableFromText(@"SELECT distinct TableName + ':' + DisplayName AS DP,TableName + ':' +  SystemName AS SP,ColumnType
                                            FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE IsStandard=0 AND TableTableID IS NULL AND  [Column].TableID=" + dr["ParentTableID"].ToString());
                    foreach (DataRow drp in dtPColumns.Rows)
                    {

                        strSystem = strSystem.Replace("CAST(dbo.RemoveNonNumericChar([" + drp["SP"].ToString() + "]) as decimal(20,10))", "[" + drp["SP"].ToString() + "]");


                    }
                }

            }



        }

        return strSystem;

    }


    public static int GetIntColorFromName(string strColorName)
    {
        try
        {
            switch (strColorName)
            {
                case "Aqua":
                    return 0x00FFFF;
                case "Black":
                    return 0x000000;
                case "Blue":
                    return 0x0000FF;
                case "Fuchsia":
                    return 0xFF00FF;
                case "Gray":
                    return 0x808080;
                case "Green":
                    return 0x008000;
                case "Lime":
                    return 0x00FF00;
                case "Maroon":
                    return 0x800000;
                case "Navy":
                    return 0x000080;
                case "Olive":
                    return 0x808000;
                case "Orange":
                    return 0xFFA500;
                case "Purple":
                    return 0x800080;
                case "Red":
                    return 0xFF0000;
                case "Silver":
                    return 0xC0C0C0;
                case "Teal":
                    return 0x008080;
                case "Yellow":
                    return 0xFFFF00;
                default:
                    return int.Parse(strColorName.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);

            }
        }
        catch( Exception ex)
        {
            return -1;
        }


    }

    public int DefaultTextWidth = 22;//200/22=9px for each word
    public int DefaultTextHeight = 1;//18*1
    public enum NumberType
    {
        Normal=1,
        Constant=2,
        Calculated=3,
        Average=4
    }


    public static class UserRoleType
    {
       public static string GOD = "1";
         public static string Administrator = "2";
         public static string EditRecordSite = "3";
         public static string AddEditRecord = "4";
         public static string ReadOnly = "5";
         public static string None = "6";
         public static string AddRecord = "7";
         public static string OwnData = "8";
         public static string EditOwnViewOther = "9";     

    }



    //public static string GetMinVaue(string strFormula)
    //{
    //    strFormula = strFormula.ToLower();
    //    strFormula = strFormula.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
    //    strFormula = strFormula.Replace("  ", " ");
    //    strFormula = strFormula.Replace("value >=", "value>=");
    //    strFormula = strFormula.Replace("value > =", "value>=");

    //    if (strFormula.IndexOf("value>=") > -1)
    //    {
    //        //we have MIN
    //        string strMin = strFormula.Substring(strFormula.IndexOf("value>=") + 7);
    //        strMin = strMin.Trim();
    //        if (strMin.IndexOf(" ") > -1)
    //        {
    //            return strMin.Substring(0, strMin.IndexOf(" "));
    //        }
    //        else
    //        {
    //            return strMin;
    //        }

    //    }


    //    return "";
    //}


    public static string GetMaxFromFormula (string strFormula)
    {
        strFormula = strFormula.ToLower();
        strFormula = strFormula.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
        strFormula = strFormula.Replace("  ", " ");
        strFormula = strFormula.Replace("value <=", "value<=");
        strFormula = strFormula.Replace("value < =", "value<=");

        if (strFormula.IndexOf("value<=") > -1)
        {
            //we have max
            string strMax = strFormula.Substring(strFormula.IndexOf("value<=") + 7);
            strMax = strMax.Trim();
            if (strMax.IndexOf(" ") > -1)
            {
                return strMax.Substring(0, strMax.IndexOf(" "));
            }
            else
            {
                return strMax;
            }

        }


        return "";
    }

    //public static string GetMaxVaue(string strFormula)
    //{
    //    strFormula = strFormula.ToLower();
    //    strFormula = strFormula.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
    //    strFormula = strFormula.Replace("  ", " ");
    //    strFormula = strFormula.Replace("value <=", "value<=");
    //    strFormula = strFormula.Replace("value < =", "value<=");

    //    if (strFormula.IndexOf("value<=") > -1)
    //    {
    //        //we have Max
    //        string strMax = strFormula.Substring(strFormula.IndexOf("value<=") + 7);
    //        strMax = strMax.Trim();
    //        if (strMax.IndexOf(" ") > -1)
    //        {
    //            return strMax.Substring(0, strMax.IndexOf(" "));
    //        }
    //        else
    //        {
    //            return strMax;
    //        }

    //    }

    //    return "";
    //}


    public static string GetFullFormula(string strMin,string strMax,string strFull)
    {
        if (strMin != "" || strMax != "")
        {
            string strFormula = "";
            if (strMin != "")
            {
                strFormula = "value>=" + strMin;
            }

            if (strMax != "")
            {
                strFormula = "value<=" + strMax;
            }

            if (strMin != ""
            && strMax != "")
            {
                strFormula = "value>=" + strMin + " AND " + "value<=" + strMax;
            }

            return strFormula;

        }
        else
        {
            return strFull;
        }

       
    }
    public static string GetMinFromFormula(string strFormula)
    {
        strFormula = strFormula.ToLower();
        strFormula = strFormula.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
        strFormula = strFormula.Replace("  ", " ");
        strFormula = strFormula.Replace("value >=", "value>=");
        strFormula = strFormula.Replace("value > =", "value>=");      

        if (strFormula.IndexOf("value>=") > -1)
        {
            //we have Min
            string strMin = strFormula.Substring(strFormula.IndexOf("value>=") + 7);
            strMin = strMin.Trim();
            if (strMin.IndexOf(" ") > -1)
            {
                return strMin.Substring(0, strMin.IndexOf(" "));
            }
            else
            {
                return strMin;
            }

        }

        return "";
    }

    public static void ShowFromula(string strFormula,ref TextBox txtMin,ref TextBox txtMax,ref TextBox txtFullFormula,ref bool bAdvanced  )
    {
        string strMin = "";
        string strMax = "";
        strMin = Common.GetMinFromFormula(strFormula);
        strMax = Common.GetMaxFromFormula(strFormula);

        int iTotalValue = Common.GetNumberOfValue(strFormula);
        bAdvanced = false;

        if (iTotalValue > 2)
        {

            bAdvanced = true;
        }
        else
        {
            if (iTotalValue > 1)
            {
                if (strMin != "" && strMax != "")
                {

                }
                else
                {
                    bAdvanced = true;
                }
            }
            else
            {
                if (strMin != "" || strMax != "")
                {

                }
                else
                {
                    bAdvanced = true;
                }

            }
        }

        if (bAdvanced)
        {
            txtFullFormula.Text = strFormula;           
        }
        else
        {
            txtMin.Text = strMin;
            txtMax.Text = strMax;          
        }

    }

    public static string GetFromulaMsg(string sType,string strDisplayName, string strFormula)
    {
        string strReturnMessage = "";
        string strMin = "";
        string strMax = "";
        strMin = Common.GetMinFromFormula(strFormula);
        strMax = Common.GetMaxFromFormula(strFormula);

        int iTotalValue = Common.GetNumberOfValue(strFormula);
        bool bAdvanced = false;

        if (iTotalValue > 2)
        {

            bAdvanced = true;
        }
        else
        {
            if (iTotalValue > 1)
            {
                if (strMin != "" && strMax != "")
                {

                }
                else
                {
                    bAdvanced = true;
                }
            }
            else
            {
                if (strMin != "" || strMax != "")
                {

                }
                else
                {
                    bAdvanced = true;
                }

            }
        }

        switch (sType)
        {
            case "i":
                strReturnMessage = "INVALID: ";
                break;
            case "e":
                strReturnMessage = "EXCEEDANCE: ";
                break;
            case "w":
                strReturnMessage = "WARNING: ";
                break;
        }

        if (strReturnMessage!="")
            strReturnMessage = strReturnMessage + strDisplayName + " ";

        if (bAdvanced)
        {
            if (strFormula=="")
            {
                strReturnMessage = strReturnMessage + "Not set.";
            }
            else
            {
                strReturnMessage = strReturnMessage + "Value outside accepted range(" + strFormula + ").";
            }
           
        }
        else
        {           
           if(sType=="")
           {
               if (strMin == "")
                   strMin = "Not set";
               if (strMax == "")
                   strMax = "Not set";
           }

            if (strMin != "" && strMax!="")
            {
                if (sType == "")
                {
                    strReturnMessage = "Value greater than: " + strMin + "<br/> Value less than: " + strMax ;
                }
                else
                {
                    strReturnMessage = strReturnMessage + "is less than " + strMin + " or greater than " + strMax;
                }
               
            }
            else
            {
                if (strMin != "")
                {
                    strReturnMessage = strReturnMessage + "is less than " + strMin;
                }
                if (strMax != "")
                {
                    strReturnMessage = strReturnMessage + "is greater than " + strMax;
                }
            }
                

        }
        return strReturnMessage;
    }
    public static int GetNumberOfValue(string strFormula)
    {

        strFormula = strFormula.ToLower();

        if (strFormula.IndexOf("value") > -1)
        {
            strFormula = strFormula.Substring(strFormula.IndexOf("value") + 5);
            if (strFormula.IndexOf("value") > -1)
            {
                strFormula = strFormula.Substring(strFormula.IndexOf("value") + 5);

                if (strFormula.IndexOf("value") > -1)
                {
                    return 3;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        return 0;
    }




    public static void  PopulateAdminDropDown(ref DropDownList ddl)
    {
       

        System.Web.UI.WebControls.ListItem liAudit = new System.Web.UI.WebControls.ListItem("Audit", "Audit");
        ddl.Items.Insert(0, liAudit);

        System.Web.UI.WebControls.ListItem liBatches = new System.Web.UI.WebControls.ListItem("Batches", "Batches");
        ddl.Items.Insert(1, liBatches);

        System.Web.UI.WebControls.ListItem liContents = new System.Web.UI.WebControls.ListItem("Contents", "Contents");
        ddl.Items.Insert(2, liContents);

        //System.Web.UI.WebControls.ListItem liDocuments = new System.Web.UI.WebControls.ListItem("Documents and Reports", "Documents");
        //ddl.Items.Insert(3, liDocuments);

        System.Web.UI.WebControls.ListItem liNotifications = new System.Web.UI.WebControls.ListItem("Notifications", "Notifications");
        ddl.Items.Insert(3, liNotifications);

        System.Web.UI.WebControls.ListItem liTableList = new System.Web.UI.WebControls.ListItem("Tables", "TableList");
        ddl.Items.Insert(4, liTableList);

        System.Web.UI.WebControls.ListItem liSensors = new System.Web.UI.WebControls.ListItem("Sensors", "Sensors");
        ddl.Items.Insert(5, liSensors);

        //System.Web.UI.WebControls.ListItem liLocationList = new System.Web.UI.WebControls.ListItem( "Locations","LocationList");
        //ddl.Items.Insert(6, liLocationList);

        System.Web.UI.WebControls.ListItem liUsers = new System.Web.UI.WebControls.ListItem("Users", "Users");
        ddl.Items.Insert(6, liUsers);

        System.Web.UI.WebControls.ListItem liWorkFlow = new System.Web.UI.WebControls.ListItem("WorkFlows", "WorkFlows");
        ddl.Items.Insert(7, liWorkFlow);

       

    }
    public static string GetNavigateURL(string strValue,int iAccontID)
    {
        switch (strValue.Trim())
        {
            //case "Account":
            //    return "~/Pages/Security/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(iAccontID.ToString()) ;

            case "Batches":
                return "~/Pages/Record/Batches.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt("-1");
            
            case "Contents":
                return "~/Pages/SystemData/Content.aspx";

            case "Audit":
                return "~/Pages/Document/AuditReport.aspx";

            case "Notifications":
                return "~/Pages/Record/Notification.aspx";

            case "TableList":
                return "~/Pages/Record/TableList.aspx";

            case "Sensors":
                return "~/Pages/Instrument/Sensor.aspx";

            //case "LocationList":
            //    return "~/Pages/Site/LocationList.aspx?MenuID=" +   Cryptography.Encrypt("-1");

            case "Users":
                return "~/Pages/User/List.aspx";

            

            default:
                return "#";
                
        }
    }

    public static string CompareOperatorErrorMsg(string strCompareOperator)
    {
        string strUserFriendlyMsg = "should be equal to";
        switch (strCompareOperator)
        {
            case "Equal":
                strUserFriendlyMsg = "should be equal to";
                break;
            case "DataTypeCheck":
                strUserFriendlyMsg = "should be same data type of";
                break;
            case "GreaterThan":
                strUserFriendlyMsg = "should be greater than";
                break;
            case "GreaterThanEqual":
                strUserFriendlyMsg = "should be greater than or equal to";
                break;
            case "LessThan":
                strUserFriendlyMsg = "should be less than";
                break;
            case "LessThanEqual":
                strUserFriendlyMsg = "should be less than or equal to";
                break;
            case "NotEqual":
                strUserFriendlyMsg = "should not be equal to";
                break;
            default:
                strUserFriendlyMsg = "should be equal to";
                break;

        }

        return strUserFriendlyMsg;

    }

    public static bool IsEmailFormatOK(string inputEmail)
    {
        if (inputEmail == null || inputEmail.Length == 0)
        {
            return false;
        }

        const string expression = "^([a-zA-Z0-9_\\-\\.])+@(([0-2]?[0-5]?[0-5]\\.[0-2]?[0-5]?[0-5]\\.[0-2]?[0-5]?[0-5]\\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\\-])+\\.)+([a-zA-Z\\-])+))$";

        Regex regex = new Regex(expression);
        return regex.IsMatch(inputEmail);
    }

    public static List<string> GetColumnStringListFromTable(DataTable theDatatable)
    {

        List<string> list = new List<string>();
       
        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {
            list.Add( theDatatable.Columns[i].ColumnName);
          
        }


        return list;

    }
    public static ListItemCollection GetColumnsFromTable(DataTable theDatatable)
    {

        ListItemCollection list = new ListItemCollection();
        // ArrayList list = new ArrayList();
        string sItem = "";
        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {
            string sColumn = theDatatable.Columns[i].ColumnName;
            string sValue = "";
            if (theDatatable.Rows.Count > 0)
                sValue = theDatatable.Rows[0][i].ToString();
            if (sValue.Length > 20) sValue = String.Concat(sValue.Substring(0, 20), "...");
            if (sValue.Length > 0)
                sItem = string.Concat(sColumn, " (e.g. ", sValue, ")");
            else
                sItem = sColumn;
            list.Add(new System.Web.UI.WebControls.ListItem(sItem, sColumn));
        }


        return list;

    }

    public static ListItemCollection GetColumnsFromTableNoEG(DataTable theDatatable)
    {

        ListItemCollection list = new ListItemCollection();
        // ArrayList list = new ArrayList();
        string sItem = "";
        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {
            string sColumn = theDatatable.Columns[i].ColumnName;
            string sValue = "";
            if (theDatatable.Rows.Count > 0)
                sValue = theDatatable.Rows[0][i].ToString();
            if (sValue.Length > 20) sValue = String.Concat(sValue.Substring(0, 20), "...");
            if (sValue.Length > 0)
                sItem = string.Concat(sColumn, " (e.g. ", sValue, ")");
            else
                sItem = sColumn;
            list.Add(new System.Web.UI.WebControls.ListItem(sItem, sColumn));
        }


        return list;

    }
    public static List<string> GetTokensFromTable(DataTable theDatatable)
    {

        List<string> list=new List<string>();
      
        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {
            list.Add("[" + theDatatable.Columns[i].ColumnName + "]");            
        }


        return list;

    }

    static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
   public  static string SizeSuffix(double value)
    {
        int mag = (int)Math.Log(value, 1024);
        decimal adjustedSize = (decimal)value / (1 << (mag * 10));

        return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
    }

   public static void SendSingleEmail(string strTo, string strHeading, string strBody, ref string strError)
   {



       strError = "";

       string strEmail = SystemData.SystemOption_ValueByKey_Account("EmailFrom",null,null);
       string strEmailServer = SystemData.SystemOption_ValueByKey_Account("EmailServer", null, null);
       string strEmailUserName = SystemData.SystemOption_ValueByKey_Account("EmailUserName", null, null);
       string strEmailPassword = SystemData.SystemOption_ValueByKey_Account("EmailPassword", null, null);
       string strEnableSSL = SystemData.SystemOption_ValueByKey_Account("EnableSSL", null, null);
       string strSmtpPort = SystemData.SystemOption_ValueByKey_Account("SmtpPort", null, null);




       //strBody = strBody.Replace("[Table]", theTable.TableName);



       MailMessage msg = new MailMessage();
       msg.From = new MailAddress(strEmail);


       msg.Subject = strHeading;

       msg.IsBodyHtml = true;

       msg.Body = strBody;


       SmtpClient smtpClient = new SmtpClient(strEmailServer);
       smtpClient.Timeout = 99999;
       smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
       smtpClient.EnableSsl = bool.Parse(strEnableSSL);
       smtpClient.Port = int.Parse(strSmtpPort);

       msg.To.Clear();
       msg.To.Add(strTo);

       try
       {



#if (!DEBUG)
           smtpClient.Send(msg);
#endif


       }
       catch (Exception)
       {

           strError = "Server could not send this email, please try again.";
       }





   }


    //public static void SendSingleEmail(string strTo, Content theContent, ref string strError)
    //{

    //    if (theContent == null)
    //    {
    //        return;
    //    }

    //    strError = "";

    //    string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
    //    string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
    //    string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
    //    string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
    //    string strWarningSMSEMail = SystemData.SystemOption_ValueByKey("WarningSMSEmail");
    //    string strEnableSSL = SystemData.SystemOption_ValueByKey("EnableSSL");
    //    string strSmtpPort = SystemData.SystemOption_ValueByKey("SmtpPort");



    //    string strBody = theContent.ContentP;


    //    //strBody = strBody.Replace("[Table]", theTable.TableName);



    //    MailMessage msg = new MailMessage();
    //    msg.From = new MailAddress(strEmail);


    //    msg.Subject = theContent.Heading;

    //    msg.IsBodyHtml = true;

    //    msg.Body = strBody;


    //    SmtpClient smtpClient = new SmtpClient(strEmailServer);
    //    smtpClient.Timeout = 99999;
    //    smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
    //    smtpClient.EnableSsl = bool.Parse(strEnableSSL);
    //    smtpClient.Port = int.Parse(strSmtpPort);

    //    msg.To.Clear();
    //    msg.To.Add(strTo);

    //    try
    //    {



    //        #if (!DEBUG)
    //         smtpClient.Send(msg);
    //        #endif



    //         if (System.Web.HttpContext.Current.Session["AccountID"] != null)
    //         {

    //             SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
    //         }


    //         if (System.Web.HttpContext.Current.Session["AccountID"] != null)
    //         {
    //             if (msg.To.Count > 0)
    //             {
    //                 Guid guidNew = Guid.NewGuid();
    //                 string strEmailUID = guidNew.ToString();

    //                 EmailLog theEmailLog = new EmailLog(null, int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), msg.Subject,
    //                   msg.To[0].ToString(), DateTime.Now, null,
    //                   null,
    //                   theContent.ContentKey, msg.Body);

    //                 theEmailLog.EmailUID = strEmailUID;
    //                 EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);


    //             }
    //         }
            
    //    }
    //    catch (Exception)
    //    {

    //        strError = "Server could not send this email, please try again.";
    //    }





    //}


    public static string ReplaceDataFiledByValue(DataTable theDatatable, string strContent)
    {

        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {

            strContent = strContent.Replace("[" + theDatatable.Columns[i].ColumnName + "]", theDatatable.Rows[0][i].ToString());

        }

        return strContent;
    }


    //public static void GenerateWORDDoc(string documentFileName,DataTable theDatatable, out string error)
    //{
    //    error = String.Empty;

    //    using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(documentFileName, true))
    //    {
    //        string path = System.IO.Path.GetDirectoryName(documentFileName);
    //        MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
    //        DocumentFormat.OpenXml.Wordprocessing.Document document = mainPart.Document;
    //        IEnumerable<Text> texts = document.Body.Descendants<Text>();
    //        string strPreValue = "";
    //        foreach (Text text in texts)
    //        {
    //            //Console.WriteLine(text.Text);

    //            for (int i = 0; i < theDatatable.Columns.Count; i++)
    //            {

    //                if (text.Text == "[" + theDatatable.Columns[i].ColumnName + "]")
    //                    text.Text = theDatatable.Rows[0][i].ToString();

    //                if (strPreValue == "[")
    //                {
    //                    if ("[" + text.Text + "]" == "[" + theDatatable.Columns[i].ColumnName + "]")
    //                        text.Text = theDatatable.Rows[0][i].ToString();
    //                }
    //            }

    //            strPreValue = text.Text;

    //            if (text.Text == "[")
    //                text.Text = "";

    //            if (text.Text == "]")
    //                text.Text = "";

    //        }
    //    }
    //    //Console.ReadLine();
    //}


    //oliver <begin> Ticket 1451
    public static void GenerateWORDDoc2(string documentFileName, DataTable theDatatable, out string error)
    {
        error = String.Empty;


        using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(documentFileName, true))
        {
            string path = System.IO.Path.GetDirectoryName(documentFileName);
            MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
            DocumentFormat.OpenXml.Wordprocessing.Document document = mainPart.Document;
            IEnumerable<Paragraph> paragraphs = document.Body.Descendants<Paragraph>();
            foreach (Paragraph paragraph in paragraphs)
            {
                IEnumerable<Run> runs = from x in paragraph.Descendants<Run>()
                                        where (from y in x.Descendants<Text>()
                                               where y.Text.Contains('«')
                                               select y).Any()
                                        select x;

                foreach (Run run in runs)
                {
                    Text text = run.Descendants<Text>().FirstOrDefault();
                    int startPos = text.Text.IndexOf('«');
                    int endPos = text.Text.IndexOf('»', startPos);
                    if (endPos != -1)
                    {
                        //Console.WriteLine(text.Text.Substring(startPos, endPos - startPos + 1));
                        string newText = null;
                        if (TryReplace2(text.Text, out newText, theDatatable))
                            text.Text = newText;
                    }
                    else
                    {
                        List<Run> runsToDelete = new List<Run>();
                        string s = text.Text;
                        Run nextRun = run;
                        while (nextRun != null)
                        {
                            nextRun = nextRun.NextSibling<Run>();
                            if (nextRun != null)
                            {
                                runsToDelete.Add(nextRun);
                                Text nextText = nextRun.Descendants<Text>().FirstOrDefault();
                                if (nextText != null)
                                {
                                    s = s + nextText.Text;
                                    if (nextText.Text.IndexOf('»', startPos) != -1)
                                        break;
                                }
                            }
                        }
                        //Console.WriteLine(s);
                        string newText = null;
                        if (TryReplace2(s, out newText, theDatatable))
                        {
                            text.Text = newText;
                            foreach (Run x in runsToDelete)
                                paragraph.RemoveChild<Run>(x);
                        }
                    }
                }
            }
        }

    }
    //oliver <end>

    //oliver <begin> Ticket 1451
    static private bool TryReplace2(string oldText, out string newText, DataTable theDatatable)
    {
        newText = string.Empty;

        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {

            if (oldText.Contains("«" + theDatatable.Columns[i].ColumnName + "»"))
            {
                newText = oldText.Replace("«" + theDatatable.Columns[i].ColumnName + "»", theDatatable.Rows[0][i].ToString());
                return true;
            }
        }

        return false;

    }
    //oliver <end> Ticket 1451

    public static void GenerateWORDDoc(string documentFileName, DataTable theDatatable, out string error)
    {
        error = String.Empty;

        
        using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(documentFileName, true))
        {
            string path = System.IO.Path.GetDirectoryName(documentFileName);
            MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
            DocumentFormat.OpenXml.Wordprocessing.Document document = mainPart.Document;
            IEnumerable<Paragraph> paragraphs = document.Body.Descendants<Paragraph>();
            foreach (Paragraph paragraph in paragraphs)
            {
                IEnumerable<Run> runs = from x in paragraph.Descendants<Run>()
                                        where (from y in x.Descendants<Text>()
                                               where y.Text.Contains('[')
                                               select y).Any()
                                        select x;

                foreach (Run run in runs)
                {
                    Text text = run.Descendants<Text>().FirstOrDefault();
                    int startPos = text.Text.IndexOf('[');
                    int endPos = text.Text.IndexOf(']', startPos);
                    if (endPos != -1)
                    {
                        //Console.WriteLine(text.Text.Substring(startPos, endPos - startPos + 1));
                        string newText = null;
                        if (TryReplace(text.Text, out newText, theDatatable))
                            text.Text = newText;
                    }
                    else
                    {
                        List<Run> runsToDelete = new List<Run>();
                        string s = text.Text;
                        Run nextRun = run;
                        while (nextRun != null)
                        {
                            nextRun = nextRun.NextSibling<Run>();
                            if (nextRun != null)
                            {
                                runsToDelete.Add(nextRun);
                                Text nextText = nextRun.Descendants<Text>().FirstOrDefault();
                                if (nextText != null)
                                {
                                    s = s + nextText.Text;
                                    if (nextText.Text.IndexOf(']', startPos) != -1)
                                        break;
                                }
                            }
                        }
                        //Console.WriteLine(s);
                        string newText = null;
                        if (TryReplace(s, out newText,theDatatable))
                        {
                            text.Text = newText;
                            foreach (Run x in runsToDelete)
                                paragraph.RemoveChild<Run>(x);
                        }
                    }
                }
            }
        }

    }


    static private bool TryReplace(string oldText, out string newText, DataTable theDatatable)
    {
        newText = string.Empty;

        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {           

            if (oldText.Contains("[" + theDatatable.Columns[i].ColumnName + "]"))
            {
                newText = oldText.Replace("[" + theDatatable.Columns[i].ColumnName + "]", theDatatable.Rows[0][i].ToString());
                return true;
            }
        }

        return false;
       
    }

    public static string ReplaceDataFiledByValueWORD(DataTable theDatatable, string strContent)
    {

        for (int i = 0; i < theDatatable.Columns.Count; i++)
        {

            strContent = strContent.Replace("[" + theDatatable.Columns[i].ColumnName + "]", theDatatable.Rows[0][i].ToString());

        }

        return strContent;
    }
    public static string StripTagsCharArray(string HTMLText)
    {
        string strR = "";
        using (SqlConnection conn = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand comm = new SqlCommand("dbo.udf_StripHTML", conn))
            {
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter p1 = new SqlParameter("@HTMLText", SqlDbType.VarChar);
                // You can call the return value parameter anything, .e.g. "@Result".
                SqlParameter p2 = new SqlParameter("@Result", SqlDbType.VarChar);

                p1.Direction = ParameterDirection.Input;
                p2.Direction = ParameterDirection.ReturnValue;

                p1.Value = HTMLText;

                comm.Parameters.Add(p1);
                comm.Parameters.Add(p2);

                conn.Open();

                try
                {
                    comm.ExecuteNonQuery();

                    if (p2.Value != DBNull.Value)
                        strR = (string)p2.Value;
                }
                catch
                {

                }
                conn.Close();
                conn.Dispose();
                
            }
        }
        return strR;
    }
    //public static string StripTagsCharArray(string source)
    //{
    //    char[] array = new char[source.Length];
    //    int arrayIndex = 0;
    //    bool inside = false;

    //    for (int i = 0; i < source.Length; i++)
    //    {
    //        char let = source[i];
    //        if (let == '<')
    //        {
    //            inside = true;
    //            continue;
    //        }
    //        if (let == '>')
    //        {
    //            inside = false;
    //            continue;
    //        }
    //        if (!inside)
    //        {
    //            array[arrayIndex] = let;
    //            arrayIndex++;
    //        }
    //    }
    //    return new string(array, 0, arrayIndex);
    //}









        //Then you can  override Set function, eg :

//public override void SetCompanyDetail()
//    {
//        //base.SetCompanyDetail();
//        if (DataSource.CompanyDetailSpecified)
//        {
//            string strCompanyDetail = DBGurus.StripTagsCharArray(DataSource.CompanyDetail);
//            string param = string.Format("<key><cv><c>CompanyID<%2fc><v>{0}<%2fv><%2fcv><%2fkey>",DataSource.CompanyID);
//            string sLink = string.Format("<a href=\"javascript:void()\" onclick=\"SelectCompany('{0}')\" > more >></a>", ViewButton.ClientID);
//            if (strCompanyDetail.Length > 500)
//            {
//                string sCompanyDetail = strCompanyDetail.Substring(500);
//                sCompanyDetail = sCompanyDetail.Substring(0, sCompanyDetail.IndexOf(" "));
//                strCompanyDetail = strCompanyDetail.Substring(0, 500) + sCompanyDetail + "...";
//            }

//            CompanyDetail.Text = string.Format("{0} {1}", strCompanyDetail, sLink);
//        }
          
//    }












    public static byte[] ReadFile(string sPath)
    {
        //Initialize byte array with a null value initially.
        byte[] data = null;

        //Use FileInfo object to get file size.
        FileInfo fInfo = new FileInfo(sPath);
        long numBytes = fInfo.Length;

        //Open FileStream to read file
        FileStream fStream = new FileStream(sPath, FileMode.Open,
                                                FileAccess.Read);

        //Use BinaryReader to read file stream into byte array.
        BinaryReader br = new BinaryReader(fStream);

        //When you use BinaryReader, you need to 

        //supply number of bytes to read from file.
        //In this case we want to read entire file. 

        //So supplying total number of bytes.
        data = br.ReadBytes((int)numBytes);
        return data;
    }


    public static bool IsIn(string sEach, string sAll)
    {
        foreach (string sE in sAll.Split(','))
        {
            if (sE.Length > 0)
            {
                if (sE==sEach)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static bool HaveAccess(string strUserRoleTypes, string strAllowedRoleTypes)
    {
       foreach (string strRoleType in strAllowedRoleTypes.Split(','))
       {
           if (strRoleType.Length > 0)
           {
               if (strUserRoleTypes.Contains(strRoleType))
               {
                   return true;
               }
           }
       }
       return false;
    }

    //public static bool IsUserInThisRoleTypes(User ObjUser,string strRoleTypes)
    //{
    //    bool kq = false;
    //    int iTN = 0;
       
    //    List<UserRole> uroles = SecurityManager.UserRole_Select(null, ObjUser.UserID, null, null, null, "", "", null, null, ref iTN);

        
    //    foreach (UserRole item in uroles)
    //    {
    //        if (strRoleTypes.Contains(item.RoleType))
    //        {
    //            kq = true;
    //        }
    //    }
        
    //    ObjUser = null;
       
    //    return kq;
    //}


    public static string TrafficLightURL(Column theTrafficLightColumn, string strTLValue, string strXML)
    {

        
        string strImageURL = "";

        try
        {

            if (strTLValue != "" && strXML != "")
            {


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strXML);

                XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

                DataSet ds = new DataSet();
                ds.ReadXml(r);

                bool bhasCompare = false;
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr[0].ToString().IndexOf("____") > -1)
                        {
                            bhasCompare = true;
                            break;
                        }
                    }

                }

                if (bhasCompare)
                {

                    if (ds.Tables[0] != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string strEachValue = dr[0].ToString();

                            string strLowerTL = "";
                            string strUpperTL = "";

                            if (strEachValue.IndexOf("____") > -1)
                            {
                                strLowerTL = strEachValue.Substring(0, strEachValue.IndexOf("____"));
                                strUpperTL = strEachValue.Substring(strEachValue.IndexOf("____") + 4);
                            }

                            if (theTrafficLightColumn.ColumnType == "number")
                            {
                                if (strLowerTL != "" && strUpperTL != "")
                                {
                                    if (int.Parse(strTLValue) >= int.Parse(strLowerTL)
                                        && int.Parse(strTLValue) <= int.Parse(strUpperTL))
                                    {
                                        strImageURL = dr[1].ToString();
                                        break;
                                    }
                                }
                                else if (strLowerTL == "" && strUpperTL != "")
                                {
                                    if (int.Parse(strTLValue) <= int.Parse(strUpperTL))
                                    {
                                        strImageURL = dr[1].ToString();
                                        break;
                                    }
                                }
                                else if (strLowerTL != "" && strUpperTL == "")
                                {
                                    if (int.Parse(strTLValue) >= int.Parse(strUpperTL))
                                    {
                                        strImageURL = dr[1].ToString();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //date

                                DateTime? dtTLLowwer = null;
                                DateTime? dtTLUpper = null;
                                DateTime dtTLValue;
                                DateTime.TryParseExact(strTLValue.Trim(), Common.Dateformats, new CultureInfo("en-GB"),
                                         DateTimeStyles.None, out dtTLValue);
                                if (strLowerTL != "")
                                {
                                    DateTime dtTemp;
                                    if (DateTime.TryParseExact(strLowerTL.Trim(), Common.Dateformats, new CultureInfo("en-GB"),
                                         DateTimeStyles.None, out dtTemp))
                                    {
                                        dtTLLowwer = dtTemp;
                                    }
                                }

                                if (strUpperTL != "")
                                {
                                    DateTime dtTemp;
                                    if (DateTime.TryParseExact(strUpperTL.Trim(), Common.Dateformats, new CultureInfo("en-GB"),
                                         DateTimeStyles.None, out dtTemp))
                                    {
                                        dtTLUpper = dtTemp;
                                    }
                                }

                                if (dtTLLowwer != null && dtTLUpper != null)
                                {
                                    if (dtTLValue >= dtTLLowwer
                                        && dtTLValue <= dtTLUpper)
                                    {
                                        strImageURL = dr[1].ToString();
                                        break;
                                    }
                                }
                                if (dtTLLowwer == null && dtTLUpper != null)
                                {
                                    if (dtTLValue <= dtTLUpper)
                                    {
                                        strImageURL = dr[1].ToString();
                                        break;
                                    }
                                }
                                if (dtTLLowwer != null && dtTLUpper == null)
                                {
                                    if (dtTLValue >= dtTLLowwer)
                                    {
                                        strImageURL = dr[1].ToString();
                                        break;
                                    }
                                }


                            }

                        }
                    }


                }
                else
                {
                    if (ds.Tables[0] != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr[0].ToString().ToLower() == strTLValue.ToLower())
                            {
                                strImageURL = dr[1].ToString();
                                break;
                            }
                        }

                    }
                }


            }

        }
        catch
        {
            //
        }


        return strImageURL;
    
    }

    public static string GetLinkedDisplayText(string sDisplayColumn, int nTableTableID, int? nMaxRow,
        string WhereEqual, string DisplayPredict)
    {
        string strDisplayText = "";

        DataTable dtData = Common.spGetLinkedRecordIDnDisplayText(sDisplayColumn, nTableTableID, null, WhereEqual, "");
        if(dtData!=null && dtData.Rows.Count>0)
        {
            if(dtData.Rows[0][1]!=DBNull.Value)
            {
                strDisplayText = dtData.Rows[0][1].ToString();
            }
        }
        return strDisplayText;
    }


    public static DataTable spGetLinkedRecordIDnDisplayText(string sDisplayColumn, int nTableTableID, int? nMaxRow,
        string WhereEqual, string DisplayPredict)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("spGetLinkedRecordIDnDisplayText", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;

                command.Parameters.Add(new SqlParameter("@sDisplayColumn", sDisplayColumn));
                command.Parameters.Add(new SqlParameter("@nTableTableID", nTableTableID));

                if (WhereEqual!="")
                    command.Parameters.Add(new SqlParameter("@WhereEqual", WhereEqual));

                if (DisplayPredict != "")
                    command.Parameters.Add(new SqlParameter("@DisplayPredict", DisplayPredict));

                if (nMaxRow != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRow", nMaxRow));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
              

                connection.Open();
                try
                {
                    da.Fill(dt);
                }
                catch
                {
                    //
                }
                connection.Close();
                connection.Dispose();

                return dt;
            }
        }
    }

    public static string GetSQLOperator(string strTextOperator)
    {
        string strSQLOperator = "=";
        switch (strTextOperator.ToLower())
        {
            case "equals":
                strSQLOperator = "=";
                break;
            case "equal":
                strSQLOperator = "=";
                break;
            case "greaterthan":
                strSQLOperator = ">";
                break;
            case "greaterthanequal":
                strSQLOperator = ">=";
                break;
            case "lessthan":
                strSQLOperator = "<";
                break;
            case "lessthanequal":
                strSQLOperator = "<=";
                break;
            case "notequal":
                strSQLOperator = "<>";
                break;
            case "contains":
                strSQLOperator = "IN";
                break;
            case "notcontains":
                strSQLOperator = "NOT IN";
                break;
            default:
                strSQLOperator = "=";
                break;

        }
        return strSQLOperator;
    }

    public static bool IsDataValidCommon(string sColumnType,string sData,string strOperator, string sComparevalue)
    {
        try
        {

            if (sData == "")
            {
                if (strOperator.ToLower() == "empty")
                {
                    return true;
                }
                if (strOperator.ToLower() == "notempty")
                {
                    return false;
                }
            }
            else
            {
                if (strOperator.ToLower() == "empty")
                {
                    return false;
                }
                if (strOperator.ToLower() == "notempty")
                {
                    return true;
                }
            }


            if (sColumnType == "number" || sColumnType == "calculation")
            {
                if(sColumnType == "calculation")
                {
                    if (sData != "" && sData.IndexOf("year") > -1 && sData.IndexOf(" ") > -1)
                    {
                        sData = sData.Substring(0, sData.IndexOf(" "));
                    }
                }
               

                double dTemp=0;
                double dData ;
                double dComparevalue;
                if (double.TryParse(sData, out dTemp))
                {
                    dData = dTemp;
                }
                else
                {
                    return false;
                }

                if (double.TryParse(sComparevalue, out dTemp))
                {
                    dComparevalue = dTemp;
                }
                else
                {
                    return false;
                }

                bool bResult = false;
                switch (strOperator.ToLower())
                {
                    case "equals":               
                    case "equal":
                        bResult = dData == dComparevalue;
                        break;
                    case "greaterthan":
                        bResult = dData > dComparevalue;
                        break;
                    case "greaterthanequal":
                        bResult = dData >= dComparevalue;
                        break;
                    case "lessthan":
                        bResult = dData < dComparevalue;
                        break;
                    case "lessthanequal":
                        bResult = dData <= dComparevalue;
                        break;
                    case "notequal":
                        bResult = dData != dComparevalue;
                        break;               
                    default:
                        bResult = dData == dComparevalue;
                        break;

                }

                return bResult;
            }
            else if (sColumnType == "datetime" || sColumnType == "date" || sColumnType == "time")
            {
                DateTime dTemp;
                DateTime dData;
                DateTime dComparevalue;
                if (DateTime.TryParse(sData, out dTemp))
                {
                    dData = dTemp;
                }
                else
                {
                    return false;
                }

                if (DateTime.TryParse(sComparevalue, out dTemp))
                {
                    dComparevalue = dTemp;
                }
                else
                {
                    return false;
                }

                bool bResult = false;

                switch (strOperator.ToLower())
                {
                    case "equals":
                    case "equal":
                        bResult = dData == dComparevalue;
                        break;
                    case "greaterthan":
                        bResult = dData > dComparevalue;
                        break;
                    case "greaterthanequal":
                        bResult = dData >= dComparevalue;
                        break;
                    case "lessthan":
                        bResult = dData < dComparevalue;
                        break;
                    case "lessthanequal":
                        bResult = dData <= dComparevalue;
                        break;
                    case "notequal":
                        bResult = dData != dComparevalue;
                        break;
                    default:
                        bResult = dData == dComparevalue;
                        break;

                }

                return bResult;
            }
            else
            {
                sData = sData.ToLower();
                sComparevalue = sComparevalue.ToLower();
                bool bResult = false;
                switch (strOperator.ToLower())
                {
                    case "equals":
                    case "equal":
                        bResult = sData == sComparevalue;
                        break;
                    case "greaterthan":
                        bResult = sData.Length > sComparevalue.Length;
                        break;
                    case "greaterthanequal":
                        bResult = sData.Length >= sComparevalue.Length;
                        break;
                    case "lessthan":
                        bResult = sData.Length < sComparevalue.Length;
                        break;
                    case "lessthanequal":
                        bResult = sData.Length <= sComparevalue.Length;
                        break;
                    case "notequal":
                        bResult = sData != sComparevalue;
                        break;
                    case "contains":
                        //listbox,dropdown,

                        bResult = sData.IndexOf(sComparevalue) > -1;
                        break;
                    case "notcontains":
                        //listbox,dropdown,
                        bResult = sData.IndexOf(sComparevalue) == -1;
                        break;
                    default:
                        bResult = sData == sComparevalue;
                        break;

                }
                return bResult;
            }



        }
        catch
        {
            //
        }

        return false;
    }


    public static DataTable DataTableFromText(string strCommandText)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_DataTable_FromSQL", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;

                command.Parameters.Add(new SqlParameter("@sSQL", strCommandText));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();

                connection.Open();
                try
                {
                    da.Fill(dt);
                }
                catch
                {
                    //
                }
                connection.Close();
                connection.Dispose();

                return dt;


            }

        }


    }


   

    public static void AddOneSysRecord(int iTableID,int iEnteredBy,string strSystemName,string strValue)
    {
        ExecuteText("INSERT INTO Record (TableID,EnteredBy," + strSystemName.Trim().ToUpper() + ") VALUES (" + iTableID.ToString() + "," + iEnteredBy.ToString()+ ",'"+strValue.Replace("'","''")+"')");
    }
   


    public static int ExecuteText(string strCommandText)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbg_EXEC_SQL";
                command.CommandTimeout = 0;
                command.Parameters.Add(new SqlParameter("@sSQL", strCommandText));
                

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //
                }
                
                connection.Close();
                connection.Dispose();

                return 1;

            }
        }
    }

    //public static string GetValueFromSQL(string strSQL)
    //{

    //    string strValue = "";


    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand(strSQL, connection))
    //        {
    //            command.CommandType = CommandType.Text;

    //            connection.Open();
    //            try
    //            {
    //                using (SqlDataReader reader = command.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        if (reader[0] != DBNull.Value)
    //                        {
    //                            strValue = reader[0].ToString();
    //                            break;
    //                        }
    //                    }
    //                }                
    //            }
    //            catch
    //            {
    //               //
    //            }
    //            connection.Close();
    //            connection.Dispose();
    //        }
    //    }

    //    return strValue;

    //}



    public static string GetValueFromSQL(string strSQL)
    {

        string strValue = "";


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_DataTable_FromSQL", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@sSQL", strSQL));

                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader[0] != DBNull.Value)
                            {
                                strValue = reader[0].ToString();
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    //
                }
                connection.Close();
                connection.Dispose();
            }
        }

        return strValue;

    }
    

    

   


    public static bool IsDataValid(string strData, string strExpression)
    {
        if (strData == null || strData.Length == 0)
        {
            return false;
        }

        Regex regex = new Regex(strExpression);
        return regex.IsMatch(strData);
    }


    public static string RemoveSpecialCharacters(string str)
    {

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < str.Length; i++)
        {
            if ((int)str[i] >= 32 && (int)str[i] <= 127)
            {
                if ((int)str[i] != 63 && (int)str[i] != 35 && (int)str[i] != 46)
                {
                    sb.Append(str[i]);
                }
            }
        }

        return sb.ToString();
    }


    public static bool HasSymbols(string str)
    {

        StringBuilder sb = new StringBuilder();
        bool bOnlyOneDot = false;
        bool bOnlyOneMinus = false;
        for (int i = 0; i < str.Length; i++)
        {
            bool bSymbol = true;
            if ((int)str[i] >= 48 && (int)str[i] <= 57)
            {

                sb.Append(str[i]);
                bSymbol = false;
            }
            if ((int)str[i] == 46 && bOnlyOneDot == false)
            {
                sb.Append(str[i]);
                bOnlyOneDot = true;
                bSymbol = false;
            }
            if ((int)str[i] == 45 && bOnlyOneMinus == false && sb.Length == 0)
            {
                sb.Append(str[i]);
                bOnlyOneMinus = true;
                bSymbol = false;
            }
            if(bSymbol)
            {
                return true;
            }
        }

        return false;
    }
    public static string IgnoreSymbols(string str)
    {

        StringBuilder sb = new StringBuilder();
        bool bOnlyOneDot = false;
        bool bOnlyOneMinus = false;
        for (int i = 0; i < str.Length; i++)
        {
            if ((int)str[i] >= 48 && (int)str[i] <= 57)
            {
               
                    sb.Append(str[i]);
               
            }
            if ((int)str[i] == 46 && bOnlyOneDot==false)
            {
                sb.Append(str[i]);
                bOnlyOneDot = true;
            }
            if ((int)str[i] == 45 && bOnlyOneMinus==false && sb.Length==0)
            {
                sb.Append(str[i]);
                bOnlyOneMinus = true;
            }
        }

        return sb.ToString();
    }

    public static byte[] ResizeImageFile(byte[] imageFile, int targetSize)
    {
        using (System.Drawing.Image oldImage = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
        {
            Size newSize = CalculateDimensions(oldImage.Size, targetSize);
            using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb))
            {
                using (Graphics canvas = Graphics.FromImage(newImage))
                {
                    canvas.SmoothingMode = SmoothingMode.AntiAlias;
                    canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));
                    MemoryStream m = new MemoryStream();
                    newImage.Save(m, ImageFormat.Jpeg);
                    return m.GetBuffer();
                }
            }
        }
    }


    private static Size CalculateDimensions(Size oldSize, int targetSize)
    {
        Size newSize = new Size();
        if (oldSize.Height > oldSize.Width)
        {
            newSize.Width = (int)(oldSize.Width * ((float)targetSize / (float)oldSize.Height));
            newSize.Height = targetSize;
        }
        else
        {
            newSize.Width = targetSize;
            newSize.Height = (int)(oldSize.Height * ((float)targetSize / (float)oldSize.Width));
        }
        return newSize;
    }

    public static string MobileContentByKey(string key)
    {
        string strContent = "";
        using (SqlConnection conn = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            SqlCommand cmd = new SqlCommand(String.Format("SELECT TOP 1 Content FROM Content WHERE ContentKey='{0}' AND AccountID is null", key), conn);
            conn.Open();
            strContent = Convert.ToString(cmd.ExecuteScalar());
            conn.Close();
            conn.Dispose();
        }
        return strContent;
    }




    public static void PutListValues_Text(string strDropdownValues, ref  ListBox lb)
    {
        lb.Items.Clear();
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(strText, strValue);
                    lb.Items.Add(liTemp);
                }
            }
        }


    }

    public static void SetListValues(string strDBValues, ref  ListBox lb)
    {
        if (strDBValues != "")
        {
            string[] strSS = strDBValues.Split(',');
            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }
        }

    }

    public static string GetListTableDisplay(string sDisplayColumn, int nTableTableID, string WhereEqual, string DisplayPredict)
    {
        string strDisplayText = "";

        DataTable dtData = Common.spGetLinkedRecordIDnDisplayText(sDisplayColumn, nTableTableID, null, WhereEqual, "");
        if (dtData != null && dtData.Rows.Count > 0)
        {

            foreach(DataRow dr in dtData.Rows)
            {
                if(dr[1]!=DBNull.Value)
                {
                    strDisplayText = strDisplayText + "," + dr[1].ToString();
                }
            }
            if(strDisplayText.Length>1)
            {
                strDisplayText = strDisplayText.Substring(1);
            }
        }

        return strDisplayText;
    }

    public static void SetListValues_ForTable(string strDBValues, ref  ListBox lb, int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn)
    {
        if (strDBValues != "")
        {
            //it's a new dev so iLinkedParentColumnID must be RecordID

            DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable,"","");


            lb.Items.Clear();

            string[] strSS = strDBValues.Split(',');



            foreach (DataRow dr in dtParents.Rows)
            {

                foreach (string SS in strSS)
                {
                    if (SS == dr[0].ToString())
                    {
                        System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(dr[1].ToString(), dr[0].ToString());
                        lb.Items.Add(liTemp);
                    }
                }


            }

            foreach (DataRow dr in dtParents.Rows)
            {
                if (lb.Items.FindByValue(dr[0].ToString()) == null)
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(dr[1].ToString(), dr[0].ToString());
                    lb.Items.Add(liTemp);
                }
            }
            foreach (string SS in strSS)
            {
               
                if (SS != "" & lb.Items.FindByValue(SS)!=null)
                {
                    lb.Items.FindByValue(SS).Selected = true;
                }
                        
                
            }



        }

    }


    public static void SetListValues_Text(string strDBValues, ref  ListBox lb, string strDropdownValues)
    {
        if (strDBValues != "")
        {
            lb.Items.Clear();

            string[] strSS = strDBValues.Split(',');

            string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string strValue = "";
            string strText = "";
            foreach (string s in result)
            {
                strValue = "";
                strText = "";
                if (s.IndexOf(",") > -1)
                {
                    strValue = s.Substring(0, s.IndexOf(","));
                    strText = s.Substring(strValue.Length + 1);
                }

                foreach (string SS in strSS)
                {
                    if (SS == strValue)
                    {
                        System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(strText, strValue);
                        lb.Items.Add(liTemp);
                    }
                }


            }

            foreach (string s in result)
            {
                strValue = "";
                strText = "";

                if (s.IndexOf(",") > -1)
                {
                    strValue = s.Substring(0, s.IndexOf(","));
                    strText = s.Substring(strValue.Length + 1);
                }

                if (lb.Items.FindByValue(strValue) == null)
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(strText, strValue);
                    lb.Items.Add(liTemp);
                }
            }
            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }



        }

    }

    public static void SetListValues(string strDBValues, ref  ListBox lb, string strDropdownValues)
    {
        if (strDBValues != "")
        {
            lb.Items.Clear();

            string[] strSS = strDBValues.Split(',');

            string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in result)
            {
                foreach (string SS in strSS)
                {
                    if (SS == s)
                    {
                        System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(s, s);
                        lb.Items.Add(liTemp);
                    }
                }

            }

            foreach (string s in result)
            {
                if (lb.Items.FindByValue(s) == null)
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(s, s);
                    lb.Items.Add(liTemp);
                }

            }

            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }
        }

    }


    public static void PutListValues(string strDropdownValues, ref  ListBox lb)
    {
        lb.Items.Clear();
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in result)
        {
            System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(s, s);
            lb.Items.Add(liTemp);
        }


    }
    public static string GetListValues(ListBox lb)
    {
        string strSelectedValues = "";

        foreach (System.Web.UI.WebControls.ListItem item in lb.Items)
        {
            if (item.Selected)
            {
                strSelectedValues = strSelectedValues + item.Value + ",";
            }
        }

        if (strSelectedValues != "")
            strSelectedValues = strSelectedValues.Substring(0, strSelectedValues.Length - 1);

        return strSelectedValues;
    }

    public static string GetDisplayTextFromColumnAndValue(Column theColumn, string strDBValue)
    {
        if (strDBValue.Trim() == "" || theColumn==null)
            return strDBValue;

        string strDisplayText = strDBValue;

        try
        {
            if (theColumn.TableTableID != null && theColumn.DisplayColumn != "" && theColumn.LinkedParentColumnID != null
                && (int)theColumn.TableTableID > -1)
            {
                strDisplayText = Common.GetLinkedDisplayText(theColumn.DisplayColumn, (int)theColumn.TableTableID, null, " AND Record.RecordID=" + strDBValue, "");

            }
            else if (theColumn.TableTableID != null && theColumn.DisplayColumn != "" && (int)theColumn.TableTableID == -1)
            {
                strDisplayText = RecordManager.fnGetSystemUserDisplayText(theColumn.DisplayColumn, strDBValue);
            }
            else if(theColumn.DropdownValues!="" && (theColumn.ColumnType=="dropdown" || theColumn.ColumnType=="listbox"))
            {
                strDisplayText = Common.GetTextFromValue(theColumn.DropdownValues, strDBValue);
            }
            else
            {
                strDisplayText = strDBValue;
            }

        }
        catch
        {
            strDisplayText = strDBValue;
        }

        return strDisplayText;
    }
    public static string GetTextFromValue(string strDropdownValues, string strDBValue)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    if (strDBValue.ToLower() == strValue.ToLower())
                    {
                        return strText;
                    }
                }
            }
        }
        return strDBValue;

    }

    public static void GetCheckTcikedUnTicked(string strDropdownValues, ref string strTrue, ref string strFalse)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());

            if (i == 0)
            {
                strTrue = s;
            }
            else if (i == 1)
            {
                strFalse = s;
            }
            i = i + 1;
        }

    }


    public static void PutCheckBoxListValues(string strDropdownValues, ref  CheckBoxList lb)
    {
        lb.Items.Clear();
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in result)
        {
            System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(s, s);
            lb.Items.Add(liTemp);
        }

    }

    public static void PutCheckBoxListValues_Text(string strDropdownValues, ref  CheckBoxList lb)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";
        lb.Items.Clear();
        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(strText, strValue);
                    lb.Items.Add(liTemp);
                }
            }
        }


    }
    public static void PutCheckBoxList_ForTable(int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn, ref  CheckBoxList lb)
    {
        lb.Items.Clear();
        DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable,"","");
        foreach (DataRow dr in dtParents.Rows)
        {
            System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(dr[1].ToString(), dr[0].ToString());
            lb.Items.Add(liTemp);
        }

    }
    public static string GetCheckBoxValue(string strDropdownValues, ref  System.Web.UI.WebControls.CheckBox chk)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in result)
        {
            if (i == 0)
            {
                if (chk.Checked)
                {
                    return s;
                }
            }
            if (i == 1)
            {
                if (chk.Checked == false)
                {
                    return s;
                }
            }
            i = i + 1;
        }
        return "";
    }


    public static string GetCheckBoxListValues(CheckBoxList lb)
    {
        string strSelectedValues = "";

        foreach (System.Web.UI.WebControls.ListItem item in lb.Items)
        {
            if (item.Selected)
            {
                strSelectedValues = strSelectedValues + item.Value + ",";
            }
        }

        if (strSelectedValues != "")
            strSelectedValues = strSelectedValues.Substring(0, strSelectedValues.Length - 1);
        return strSelectedValues;
    }


    public static void SetCheckBoxValue(string strDropdownValues, string strValue, ref  System.Web.UI.WebControls.CheckBox chk)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in result)
        {
            if (i == 0)
            {
                if (s.ToLower() == strValue.ToLower())
                {
                    chk.Checked = true;
                }
            }
            if (i == 1)
            {
                if (s.ToLower() == strValue.ToLower())
                {
                    chk.Checked = false;
                }
            }
            i = i + 1;
        }


    }
    public static void PutCheckBoxDefault(string strDropdownValues, ref  System.Web.UI.WebControls.CheckBox chk)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in result)
        {
            if (i == 2)
            {
                if (s.ToLower() == "yes")
                {
                    chk.Checked = true;
                }
            }
            i = i + 1;
        }


    }



    public static string GetDDLValueFromText(string strDropdownValues, string strSearchText)
    {

        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    if (strText.ToLower() == strSearchText.ToLower())
                    {
                        return strValue;
                    }
                }
            }
        }

        return "";

    }

    public static string GetTextFromTableForList(int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn, string strMainValue)
    {

        //it's a new dev so iLinkedParentColumnID must be RecordID

        DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable,"","");

        string[] values = strMainValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


        string strTotalText = "";
        foreach (DataRow dr in dtParents.Rows)
        {
            foreach (string v in values)
            {
                if (dr[0].ToString() == v)
                {
                    strTotalText = strTotalText + dr[1].ToString() + ",";
                }
            }
        }
        if (strTotalText.Length > 0)
            strTotalText = strTotalText.Substring(0, strTotalText.Length - 1);

        return strTotalText;
    }



    public static void PutDDLValues(string strDropdownValues, ref  DropDownList ddl)
    {
        ddl.Items.Clear();
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in result)
        {
            System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(s, s);
            ddl.Items.Add(liTemp);
        }

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddl.Items.Insert(0, liSelect);

    }

    public static void PutList_FromTable(int iTableTableID, int? iLinkedParentColumnID, string strDisplayColumn, ref  ListBox lb)
    {
        //it's a new dev so iLinkedParentColumnID must be RecordID
        DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable,"","");

        lb.Items.Clear();
        foreach (DataRow dr in dtParents.Rows)
        {
            System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(dr[1].ToString(), dr[0].ToString());
            lb.Items.Add(liTemp);
        }

    }

    public static string GetDatestringFromD(string strDate)
    {
        string strDateTime = "";
        if (strDate.Trim() == "")
        {
            // strDateTime = DateTime.Now.ToShortDateString();// +" 12:00:00 AM";
        }
        else
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(strDate.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                strDate = dtTemp.ToShortDateString();
            }
            strDateTime = strDate;// +" " + "12:00:00 AM";
        }
        return strDateTime;

    }
    public static string GetDateTimeFromDnT(string  strDate, string  strTime)
    {
        string strDateTime = "";
        if (strDate.Trim() == "")
        {
            // strDateTime = DateTime.Now.ToShortDateString() + " 12:00:00 AM";
        }
        else
        {

            DateTime dtTemp;
            if (DateTime.TryParseExact(strDate.Trim(), Common.Dateformats, new CultureInfo("en-GB"),
                DateTimeStyles.None, out dtTemp))
            {
                strDate = dtTemp.ToShortDateString();
            }

            string strTimePart = "";
           
            if (strTime == "")
            {
                strTimePart = " 12:00:00 AM";
            }
            else
            {
                if (strTime.ToLower().IndexOf(":am") > 0)
                {
                    strTimePart = strTime.ToLower().Replace(":am", ":00 AM");
                }
                else
                {
                    strTimePart = strTime.ToLower().Replace(":pm", ":00 PM");
                }
            }
           

            strDateTime = strDate + " " + strTimePart;
        }
        return  strDateTime;


    }
    public static void PutDDLValue_Text(string strDropdownValues, ref  DropDownList ddl)
    {
        ddl.Items.Clear();
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(strText, strValue);
                    ddl.Items.Add(liTemp);
                }
            }
        }

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddl.Items.Insert(0, liSelect);

    }

    public static void PutRadioList(string strDropdownValues, ref  RadioButtonList rl)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        rl.Items.Clear();
        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(s + "&nbsp;&nbsp;", s);
            rl.Items.Add(liTemp);
        }

    }


    public static void PutRadioListValue_Text(string strDropdownValues, ref  RadioButtonList rl)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        rl.Items.Clear();
        string strValue = "";
        string strText = "";

        foreach (string s in result)
        {
            //ListItem liTemp = new ListItem(s, s.ToLower());
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);
                if (strValue != "" && strText != "")
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(strText + "&nbsp;&nbsp;", strValue);
                    rl.Items.Add(liTemp);
                }
            }
        }


    }




    public static void SetCheckBoxListValues(string strDBValues, ref  CheckBoxList lb, string strDropdownValues)
    {


        if (strDBValues != "")
        {

            lb.Items.Clear();


            string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);


            string[] strSS = strDBValues.Split(',');

            foreach (string s in result)
            {

                foreach (string SS in strSS)
                {
                    if (SS == s)
                    {
                        System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(s, s);
                        lb.Items.Add(liTemp);
                    }
                }
            }

            foreach (string s in result)
            {

                if (lb.Items.FindByValue(s) == null)
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(s, s);
                    lb.Items.Add(liTemp);
                }

            }

            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }

            foreach (System.Web.UI.WebControls.ListItem li in lb.Items)
            {
                li.Attributes.Add("DataValue", li.Value);
            }
        }

    }


    public static void SetCheckBoxListValues_ForTable(string strDBValues, ref  CheckBoxList lb, int iTableTableID,
       int? iLinkedParentColumnID, string strDisplayColumn)
    {


        if (strDBValues != "")
        {
            //it's a new dev so iLinkedParentColumnID must be RecordID

            DataTable dtParents = Common.spGetLinkedRecordIDnDisplayText(strDisplayColumn, iTableTableID, Common.MaxRowForListBoxTable,"","");


            lb.Items.Clear();

            string[] strSS = strDBValues.Split(',');

            foreach (DataRow dr in dtParents.Rows)
            {

                foreach (string SS in strSS)
                {
                    if (SS == dr[0].ToString())
                    {
                        System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(dr[1].ToString(), dr[0].ToString());
                        lb.Items.Add(liTemp);
                    }
                }


            }



            foreach (DataRow dr in dtParents.Rows)
            {




                if (lb.Items.FindByValue(dr[0].ToString()) == null)
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(dr[1].ToString(), dr[0].ToString());
                    lb.Items.Add(liTemp);
                }

            }

            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }

            foreach (System.Web.UI.WebControls.ListItem li in lb.Items)
            {
                li.Attributes.Add("DataValue", li.Value);
            }
        }

    }

    public static void SetCheckBoxListValues_Text(string strDBValues, ref  CheckBoxList lb, string strDropdownValues)
    {


        if (strDBValues != "")
        {

            lb.Items.Clear();


            string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string strValue = "";
            string strText = "";

            string[] strSS = strDBValues.Split(',');

            foreach (string s in result)
            {
                strValue = "";
                strText = "";

                if (s.IndexOf(",") > -1)
                {
                    strValue = s.Substring(0, s.IndexOf(","));
                    strText = s.Substring(strValue.Length + 1);
                }

                foreach (string SS in strSS)
                {
                    if (SS == strValue)
                    {
                        System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(strText, strValue);
                        lb.Items.Add(liTemp);
                    }
                }
            }

            foreach (string s in result)
            {


                strValue = "";
                strText = "";

                if (s.IndexOf(",") > -1)
                {
                    strValue = s.Substring(0, s.IndexOf(","));
                    strText = s.Substring(strValue.Length + 1);
                }

                if (lb.Items.FindByValue(strValue) == null)
                {
                    System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(strText, strValue);
                    lb.Items.Add(liTemp);
                }

            }

            foreach (string SS in strSS)
            {
                try
                {
                    if (SS != "")
                        lb.Items.FindByValue(SS).Selected = true;
                }
                catch
                {
                    //
                }
            }

            foreach (System.Web.UI.WebControls.ListItem li in lb.Items)
            {
                li.Attributes.Add("DataValue", li.Value);
            }
        }

    }


    public static string GetTextFromValueForList(string strDropdownValues, string strMainValue)
    {
        string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        string[] values = strMainValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        string strValue = "";
        string strText = "";
        string strTotalText = "";
        foreach (string s in result)
        {
            strValue = "";
            strText = "";
            if (s.IndexOf(",") > -1)
            {
                strValue = s.Substring(0, s.IndexOf(","));
                strText = s.Substring(strValue.Length + 1);

                foreach (string v in values)
                {
                    if (strValue == v)
                    {
                        strTotalText = strTotalText + strText + ",";
                    }
                }


            }
        }
        if (strTotalText.Length > 0)
            strTotalText = strTotalText.Substring(0, strTotalText.Length - 1);

        return strTotalText;
    }

    public static string GetImageFromValueForDD(string strDropdownValues, string strMainValue, string _strFilesLocation, string strHeight)
    {
        OptionImageList theOptionImageList = JSONField.GetTypedObject<OptionImageList>(strDropdownValues);
        foreach (OptionImage aOptionImage in theOptionImageList.ImageList)
        {
            if (aOptionImage.Value == strMainValue)
            {
                if (strHeight!="")
                {
                    strHeight = " height='" + strHeight + "'";
                }

                return "<img " + strHeight + " src='" + _strFilesLocation + "/UserFiles/AppFiles/" + aOptionImage.UniqueFileName + "' title='" + aOptionImage.Value + "' />";
            }

        }
        return strMainValue;
    }
    public static void PutRadioListValue_Image(string strDropdownValues, ref  RadioButtonList rl, string _strFilesLocation)
    {
        //string[] result = strDropdownValues.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        OptionImageList theOptionImageList = JSONField.GetTypedObject<OptionImageList>(strDropdownValues);
        rl.Items.Clear();
        foreach (OptionImage aOptionImage in theOptionImageList.ImageList)
        {
            System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem("<img src='" + _strFilesLocation + "/UserFiles/AppFiles/" + aOptionImage.UniqueFileName + "' title='" + aOptionImage.Value + "' />" + "&nbsp;&nbsp;", aOptionImage.Value);
            rl.Items.Add(liTemp);
        }

    }

    public static void PutRadioImageInto_DDL(string strDropdownValues, ref  DropDownList ddl)
    {

        OptionImageList theOptionImageList = JSONField.GetTypedObject<OptionImageList>(strDropdownValues);
        ddl.Items.Clear();
        foreach (OptionImage aOptionImage in theOptionImageList.ImageList)
        {
            System.Web.UI.WebControls.ListItem liTemp = new System.Web.UI.WebControls.ListItem(aOptionImage.FileName, aOptionImage.Value);
            ddl.Items.Add(liTemp);
        }

        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddl.Items.Insert(0, liSelect);

    }

}



public class Cryptography
{

    #region Fields



    private static byte[] key = { };

    private static byte[] IV = { 38, 55, 206, 48, 28, 64, 20, 16 };

    //private static string stringKey = "!5663a#KN";

    //private static string stringKey = HttpContext.Current.Session.SessionID;

    private static string stringKey = "dbg12!12345";
    #endregion



    #region Public Methods


   



    public static string Encrypt(string text)
    {
        //HttpContext.Current.Session.SessionID

        try
        {
            if (text=="")
                return "";

            key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));



            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            Byte[] byteArray = Encoding.UTF8.GetBytes(text);



            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,

                des.CreateEncryptor(key, IV), CryptoStreamMode.Write);



            cryptoStream.Write(byteArray, 0, byteArray.Length);

            cryptoStream.FlushFinalBlock();


            string strValue = Convert.ToBase64String(memoryStream.ToArray());
            //StringWriter writer = new StringWriter();
            //HttpContext.Current.Server.UrlEncode(strValue, writer);
            System.Web.HttpUtility.UrlEncode(strValue);
            //return writer.ToString();

            return strValue;

        }

        catch (Exception ex)
        {

            // Handle Exception Here

        }



        return string.Empty;

    }



    public static string Decrypt(string text)
    {

        try
        {
            text = text.Replace(' ', '+');
            key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));



            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            Byte[] byteArray = Convert.FromBase64String(text);



            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,

                des.CreateDecryptor(key, IV), CryptoStreamMode.Write);



            cryptoStream.Write(byteArray, 0, byteArray.Length);

            cryptoStream.FlushFinalBlock();



            return Encoding.UTF8.GetString(memoryStream.ToArray());

        }

        catch (Exception ex)
        {

            // Handle Exception Here

        }



        return string.Empty;

    }



    public static string EncryptStatic(string text)
    {
        //HttpContext.Current.Session.SessionID

        try
        {

            key = Encoding.UTF8.GetBytes("tyu367xj");



            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            Byte[] byteArray = Encoding.UTF8.GetBytes(text);



            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,

                des.CreateEncryptor(key, IV), CryptoStreamMode.Write);



            cryptoStream.Write(byteArray, 0, byteArray.Length);

            cryptoStream.FlushFinalBlock();


            string strValue = Convert.ToBase64String(memoryStream.ToArray());
            StringWriter writer = new StringWriter();
            HttpContext.Current.Server.UrlEncode(strValue, writer);

            return writer.ToString();

        }

        catch (Exception ex)
        {

            // Handle Exception Here

        }



        return string.Empty;

    }

    public static string DecryptStatic(string text)
    {

        try
        {
            text = text.Replace(' ', '+');
            key = Encoding.UTF8.GetBytes("tyu367xj");



            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            Byte[] byteArray = Convert.FromBase64String(text);



            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,

                des.CreateDecryptor(key, IV), CryptoStreamMode.Write);



            cryptoStream.Write(byteArray, 0, byteArray.Length);

            cryptoStream.FlushFinalBlock();



            return Encoding.UTF8.GetString(memoryStream.ToArray());

        }

        catch (Exception ex)
        {

            // Handle Exception Here

        }



        return string.Empty;

    }

    #endregion

}


