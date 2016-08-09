using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

public partial class UploadEngine : SecurePage
{
    //protected override void OnInit(EventArgs e)
    //{
    //    Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TATi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
    //}
    #region Web Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack)
        {
            UploadDetail Upload = (UploadDetail)Session["UploadDetail"];
            //Let the webservie know that we are not yet ready
            Upload.IsReady = false;
            if (this.fileUpload.PostedFile != null && this.fileUpload.PostedFile.ContentLength > 0)
            {
                //build the local path where upload all the files
                string path = this.Server.MapPath(@"Uploads");
                string fileName = Path.GetFileName(this.fileUpload.FileName);

                //Build the strucutre and stuff it into session
                Upload.ContentLength = this.fileUpload.PostedFile.ContentLength;
                Upload.FileName = fileName;
                Upload.UploadedLength = 0;
                //Let the polling process know that we are done initializing ...
                Upload.IsReady = true;

                //set the buffer size to something larger.
                //the smaller the buffer the longer it will take to download, 
                //but the more precise your progress bar will be.
                int bufferSize = 1;
                byte[] buffer = new byte[bufferSize];

                //Writing the byte to disk
                string strFileUniqueName = Guid.NewGuid().ToString() + "_" + fileName;
                using (FileStream fs = new FileStream(Path.Combine(path, strFileUniqueName), FileMode.Create))
                {
                    //Aslong was we haven't written everything ...
                    while (Upload.UploadedLength < Upload.ContentLength)
                    {
                        //Fill the buffer from the input stream
                        int bytes = this.fileUpload.PostedFile.InputStream.Read(buffer, 0, bufferSize);
                        //Writing the bytes to the file stream
                        fs.Write(buffer, 0, bytes);
                        //Update the number the webservice is polling on to the session
                        Upload.UploadedLength += bytes;
                    }
                }
                Upload.FileUniqueName = strFileUniqueName;
                //lets write cookie
                hfFileName.Value = Upload.FileName;
                hfFileUniqueName.Value = Upload.FileUniqueName;
                //Create a new cookie, passing the name into the constructor
                //HttpCookie cookieFN = new HttpCookie("FileName");
                //HttpCookie cookieUFN = new HttpCookie("FileUniqueName");
                ////Set the cookies value
                //cookieFN.Value = Upload.FileName;
                //cookieUFN.Value = Upload.FileUniqueName;
                ////Set the cookie to expire in 1 minute
                //DateTime dtNow = DateTime.Now;
                //TimeSpan tsMinute = new TimeSpan(0, 0, 59, 0);
                //cookieFN.Expires = dtNow + tsMinute;
                //cookieUFN.Expires = dtNow + tsMinute;

                ////Add the cookie
                //Response.Cookies.Add(cookieFN);
                //Response.Cookies.Add(cookieUFN);

               //User _objUser = (User)Session["User"];

               //Common.ExecuteText("Delete from TempFileUpload where UserID=" + _objUser.UserID.ToString());

               //Common.ExecuteText("INSERT INTO TempFileUpload  VALUES(" + _objUser.UserID.ToString() + ",'"+Upload.FileName+"','"+Upload.FileUniqueName+"')");

                //Call parent page know we have processed the uplaod
                const string js = "window.parent.onComplete(1,'File uploaded successfully.','{0}','{1} of {2} Bytes');";
                ScriptManager.RegisterStartupScript(this, typeof(UploadEngine), "progress", string.Format(js, fileName, Upload.UploadedLength, Upload.ContentLength), true);
            }
            else
            {
                //Call parent page know we have processed the uplaod
                const string js = "window.parent.onComplete(4, 'There was a problem with the file.','','0 of 0 Bytes');";
                ScriptManager.RegisterStartupScript(this, typeof(UploadEngine), "progress", js, true);
            }
            //Let webservie know that we are not yet ready
            Upload.IsReady = false;
           

            Session["UploadDetail"] = Upload;
        }
    }
    #endregion
}
