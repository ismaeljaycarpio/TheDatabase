using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DocGen.DAL;
using System.IO;
using System.Web.Security;
using OpenPop.Common;
using OpenPop.Mime;
using OpenPop.Pop3;
public partial class DBEmail_Emd : System.Web.UI.Page
{

    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";
    string _strPhyFolder = "";
    string _strHTTPFolder = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        _strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath", null, null);
        _strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation", null, null);

        _strPhyFolder=_strFilesPhisicalPath+ "\\UserFiles\\Export\\";

        _strHTTPFolder = _strFilesLocation + "/UserFiles/Export/";
        Server.ScriptTimeout = 1800;
        if(!IsPostBack)
        {
            
            if (Request.Url.Authority.IndexOf("localhost")>-1 || Request.QueryString["als"]!=null ||
               Request.Url.Authority == "emd.thedatabase.net" || Request.Url.Authority == "emdtraining.thedatabase.net")
            {
                try
                {
                    ALSLive();

                    Response.Redirect("~/DBEmail/WinSchedules.aspx?AutoImportRecords=yes", false);
                    return;
                }
                catch (Exception ex)
                {
                    ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -ALSLive", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                    SystemData.ErrorLog_Insert(theErrorLog);
                }

            }
        }
    }



    protected void ALSLive()
    {
        try
        {
            ALSLiveImport als = new ALSLiveImport();
            als.ImportALSLiveData();
            //AutoImportRecords();
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, " DBEmail -WinSchedules -ALSLiveImport ", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
        }
    }


}