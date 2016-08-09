using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class Pages_Graph_ExcelResponser : IHttpHandler
{
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void ProcessRequest(HttpContext context)
    {
        string inFileName = context.Request.QueryString["I"];
        string outFileName = context.Request.QueryString["O"];
        if (String.IsNullOrEmpty(inFileName))
            return;
        if (String.IsNullOrEmpty(outFileName))
            return;

        try
        {
            context.Response.Clear();
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("Content-Disposition",
                "attachment; filename*=UTF-8''" + HttpUtility.UrlEncode(outFileName).Replace("+", "%20"));

            FileStream inFileStream = new FileStream(inFileName, FileMode.Open);
            long inFileSize = inFileStream.Length;

            byte[] buffer = new byte[(int)inFileSize];
            inFileStream.Read(buffer, 0, (int)inFileSize);
            inFileStream.Close();
            File.Delete(inFileName);

            context.Response.AddHeader("Content-Length", inFileSize.ToString());
            context.Response.BinaryWrite(buffer);
            context.Response.End();
        }
        catch
        {

        }
    }
}