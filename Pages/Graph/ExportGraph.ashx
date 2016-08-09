<%@ WebHandler Language="C#" Class="ExportGraph" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using Svg;
using System.IO;
using System.Drawing.Imaging;
public class ExportGraph : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("Hello World");
        if (context.Request.Form["svg"] != null)
         {


             //string strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath", null, null);
             //string strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation", null, null);

             string strFileName = "Chart.png";

             if (context.Request.Form["filename"] != null)
                 strFileName = context.Request.Form["filename"].ToString() + ".png";
            
             string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;

             strUniqueName = Common.GetValidFileName(strUniqueName);
             string strFolderPath = context.Server.MapPath("~\\ExportedFiles");
             string strPath = strFolderPath + "\\" + strUniqueName;
            
             System.Xml.XmlDocument theDocument = new System.Xml.XmlDocument();
             theDocument.LoadXml( context.Request.Form["svg"].ToString());
             var svgDocument = SvgDocument.Open(theDocument);
             var bitmap = svgDocument.Draw();
             bitmap.Save(strPath, ImageFormat.Png);
            
             //var byteArray = Encoding.ASCII.GetBytes(context.Request.Form["svg"]);
             //using (var stream = new MemoryStream(byteArray))
             //{
             //    var svgDocument = SvgDocument.Open(stream);
             //    var bitmap = svgDocument.Draw();
             //    bitmap.Save(path, ImageFormat.Png);
             //}
             context.Response.Write(strUniqueName);
         }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}