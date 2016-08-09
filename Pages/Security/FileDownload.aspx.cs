using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

public partial class Pages_Security_FileDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["FilePath"] != null && Request.QueryString["FileName"]!=null)
        {
            try
            {
                string strFilePath = Cryptography.Decrypt(Request.QueryString["FilePath"].ToString());
                string strFileName = Cryptography.Decrypt(Request.QueryString["FileName"].ToString());

                var webClient = new WebClient();

                byte[] data = webClient.DownloadData(new Uri(strFilePath));



                Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();

                Response.ContentType = "application/octet-stream";

                Response.AppendHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", strFileName));
                Response.OutputStream.Write(data, 0, data.Length);

                Response.End();
           
            }
            catch
            {
                // 
            }
            


        }
       


    }
}