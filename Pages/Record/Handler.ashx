<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Globalization;
public class Handler : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        HttpPostedFile file = context.Request.Files["Filedata"];

        if (file != null)
        {

            string strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath",null,null);
            string strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation", null, null);
            //int id = (Int32.Parse(context.Request["id"]));
            string strFolder = context.Request["foo"];
            string strFileName = file.FileName;
            string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;
            //string strPath = context.Server.MapPath(strFolder + "\\" + strUniqueName);
            strUniqueName = Common.GetValidFileName(strUniqueName);
            string strPath = strFilesPhisicalPath + "\\" + strFolder + "\\" + strUniqueName;
           
            file.SaveAs(strPath);

            //string strFilePath = "http://" +
            //                   context.Request.Url.Authority + context.Request.ApplicationPath + "/UserFiles/AppFiles/" + strUniqueName;

            string strFilePath = strFilesLocation + "/UserFiles/AppFiles/" + strUniqueName;

         
            
            string strJSON = "{\"filename\":\"" + strUniqueName + "\",\"fullpath\":\"" + strFilePath + "\"}";


            context.Response.Write(strJSON);
            
            //context.Response.Write(strUniqueName);
            //context.Response.Redirect("http://" + context.Request.Url.Authority + context.Request.ApplicationPath + "/Pages/Document/Document.aspx", false);
        }
        else
        {
            context.Response.Write("");
        }

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}