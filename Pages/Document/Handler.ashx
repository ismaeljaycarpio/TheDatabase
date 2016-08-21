<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Globalization;
public class Handler : IHttpHandler
{
    
  

    public void ProcessRequest(HttpContext context)
    {
        HttpPostedFile file = context.Request.Files["Filedata"];

        if (file != null)
        {
            //int id = (Int32.Parse(context.Request["id"]));


            string strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath",null,null);
            string strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation",null,null);
            
            string strFolder = context.Request["foo"];
            string strFileName = file.FileName;
            string strUniqueName = context.Request["AccountID"].ToString() +"_"+ Guid.NewGuid().ToString() + "_" + strFileName;// +"." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
            //string strPath = context.Server.MapPath(strFolder + "\\" + strUniqueName);

            string strPath = strFilesPhisicalPath + "\\" + strFolder + "\\" + strUniqueName;

            file.SaveAs(strPath);

            context.Response.Write(strUniqueName + "," + file.ContentLength.ToString());
            //context.Response.Redirect(context.Request.Url.Scheme +"://" + context.Request.Url.Authority + context.Request.ApplicationPath + "/Pages/Document/Document.aspx", false);
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




    //public void ProcessRequest (HttpContext context) {

    //    try
    //    {
    //        HttpPostedFile file = context.Request.Files["Filedata"]; 

    //        if (file != null)
    //        {
    //            //int id = (Int32.Parse(context.Request["id"]));
    //            string strFolder = context.Request["foo"];

    //            if (strFolder == "" || strFolder == null)
    //                strFolder = "uploads";

    //            string strFileName = file.FileName;
    //            string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;// +"." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
    //            string strPath = context.Server.MapPath(strFolder + "\\" + strUniqueName);

    //            file.SaveAs(strPath);



    //            string strActionMode = context.Request["ActionMode"];

    //            string strDocumentDate = context.Request["DocumentDate"];
    //            string strDocumentTypeID = context.Request["DocumentTypeID"];
    //            string strDocumentText = context.Request["DocumentText"];
    //            string strAccountID = context.Request["AccountID"];
    //            string strUserID = context.Request["UserID"];
    //            string strTableID = context.Request["TableID"];

    //            string strFolderID = context.Request["FolderID"];

    //            int iFolderID = -1;

    //            if (strFolderID != "")
    //                iFolderID = int.Parse(strFolderID);

    //            if (strDocumentText == null)
    //                strDocumentText = strFileName;

    //            if (strDocumentText == "")
    //                strDocumentText = strFileName;

    //            switch (strActionMode.ToLower())
    //            {
    //                case "add":



    //                    Document newDocument = new Document(null, int.Parse(strAccountID), strDocumentText,
    //                          strDocumentTypeID == "-1" ? null : (int?)int.Parse(strDocumentTypeID),
    //                            strUniqueName,
    //                            strFileName,
    //                            DateTime.ParseExact(strDocumentDate, "d/M/yyyy", CultureInfo.InvariantCulture),
    //                            null, null, int.Parse(strUserID),
    //                            strTableID == "-1" ? null : (int?)int.Parse(strTableID));

    //                    if (iFolderID != -1)
    //                    {
    //                        newDocument.FolderID = iFolderID;
    //                    }

    //                    newDocument.Size = file.ContentLength;
    //                    DocumentManager.ets_Document_Insert(newDocument);


    //                    break;

    //                case "edit":
    //                    string strDocumentID = context.Request["DocumentID"];
    //                    Document editDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));

    //                    editDocument.DocumentText = strDocumentText;
    //                    editDocument.DocumentDate = DateTime.ParseExact(strDocumentDate, "d/M/yyyy", CultureInfo.InvariantCulture);
    //                    editDocument.DocumentTypeID = strDocumentTypeID == "-1" ? null : (int?)int.Parse(strDocumentTypeID);
    //                    editDocument.UserID = int.Parse(strUserID);

    //                    editDocument.FileTitle = strFileName;
    //                    editDocument.FileUniqename = strUniqueName;
    //                    editDocument.TableID = strTableID == "-1" ? null : (int?)int.Parse(strTableID);

    //                    //editDocument.FolderID = null;
    //                    //   if (iFolderID != -1)
    //                    //   {
    //                    //       editDocument.FolderID = iFolderID;
    //                    //   }

    //                    editDocument.Size = file.ContentLength;
    //                    DocumentManager.ets_Document_Update(editDocument);


    //                    break;

    //                default:
    //                    //?
    //                    break;
    //            }




    //            context.Response.Write("ok");
    //            //context.Response.Redirect(context.Request.Url.Scheme +"://" + context.Request.Url.Authority + context.Request.ApplicationPath + "/Pages/Document/Document.aspx", false);
    //        }
    //        else
    //        {
    //            context.Response.Write("");
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        ErrorLog theErrorLog = new ErrorLog(null, "Handler.ashx", ex.Message, ex.StackTrace, DateTime.Now, "Handler.ashx");
    //        SystemData.ErrorLog_Insert(theErrorLog);
    //      context.Response.Write("");
    //    }

    //}

}