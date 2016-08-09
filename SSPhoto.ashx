<%@ WebHandler Language="C#" Class="SSPhoto" %>

using System;
using System.Web;
using System.Drawing;
using System.IO;
public class SSPhoto : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

       
        
        //if (context.Request.QueryString["LocationID"] != null)
        //{

        //    Location theLocation = SiteManager.ets_Location_Details(int.Parse(context.Request.QueryString["LocationID"]));

        //    context.Response.ContentType = "image/gif";
        //    //context.Response.Write("Hello World");
        //    context.Response.BinaryWrite((byte[])theLocation.Photo);
        //}

        
        
        if (context.Request.QueryString["AccountID"] != null)
        {

            Account theAccount = SecurityManager.Account_Details(int.Parse(context.Request.QueryString["AccountID"]));

            context.Response.ContentType = "image/gif";

            if (context.Request.QueryString["Type"] != null)
            {
                if (context.Request.QueryString["Type"].ToString() == "m")
                {
                    Image theImage = Image.FromStream(new MemoryStream((byte[])theAccount.Logo));

                    if (theImage.Width > 200 || theImage.Height > 100)
                    {
                        context.Response.BinaryWrite(Common.ResizeImageFile((byte[])theAccount.Logo, 240));
                    }
                    else
                    {
                        context.Response.BinaryWrite((byte[])theAccount.Logo);
                    }
                    
                }
                context.Response.BinaryWrite((byte[])theAccount.Logo);
            }
            else
            {
                Image theImage = Image.FromStream(new MemoryStream((byte[])theAccount.Logo));

                if (theImage.Width > 400 || theImage.Height > 400)
                {
                    context.Response.BinaryWrite(Common.ResizeImageFile((byte[])theAccount.Logo, 400));
                }
                else
                {
                    context.Response.BinaryWrite((byte[])theAccount.Logo);
                }
               
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    
    

}