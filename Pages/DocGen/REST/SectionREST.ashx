<%@ WebHandler Language="C#" Class="SectionREST"  %>

using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using DocGen.DAL;
using System.Text;
using System.Web.SessionState;
public class SectionREST : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        bool bSuccess = false;

        if (context.Request.QueryString["type"].ToString() == "FlashSupport" && context.Request.QueryString["hfFlashSupport"] != null)
        {            
            
            context.Session["IsFlashSupported"] = context.Request.QueryString["hfFlashSupport"].ToString();
            bSuccess = true;
        }
        
        if (context.Request.QueryString["type"].ToString() == "section_delete" && context.Request.QueryString["sectionId"] != null)
        {
            bSuccess = DeleteSection(context.Request.QueryString["sectionId"].ToString());
        }
        if (context.Request.QueryString["type"].ToString() == "sections_displayorder" && context.Request.QueryString["sectionIds"] != null)
        {
            bSuccess = SortSections(context.Request.QueryString["sectionIds"].ToString());
        }


        if (context.Request.QueryString["type"].ToString() == "wfsection_delete" && context.Request.QueryString["sectionId"] != null)
        {
            bSuccess = WFDeleteSection(context.Request.QueryString["sectionId"].ToString());
        }

        if (context.Request.QueryString["type"].ToString() == "wfsections_displayorder" && context.Request.QueryString["sectionIds"] != null)
        {
            bSuccess = WFSortSections(context.Request.QueryString["sectionIds"].ToString());
        }
        context.Response.Write(bSuccess.ToString());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    protected bool DeleteSection(string sectionId)
    {
        bool success = true;
        try
        {
            int ID = 0;
            if (Int32.TryParse(sectionId, out ID))
            {
                using (DocGenDataContext ctx = new DocGenDataContext())
                {

                    DocumentSection sct = ctx.DocumentSections.SingleOrDefault<DocumentSection>(s => s.DocumentSectionID == ID);
                    if(sct.DocumentSectionTypeID==10)
                    {
                        //so it's a Record Table section, need to delete View
                        if (sct.Details != "")
                         {
                             DocGen.DAL.RecordTableSectionDetail rtDetail = DocGen.DAL.JSONField.GetTypedObject<DocGen.DAL.RecordTableSectionDetail>(sct.Details);
                            
                             if (rtDetail != null)
                             {
                                 if(rtDetail.ViewID!=null)
                                 {
                                     ViewManager.dbg_View_Delete(rtDetail.ViewID);
                                 }
                             }
                         }                        
                    }
                    ctx.ExecuteCommand("DELETE FROM [ShowWhen] WHERE DocumentSectionID = {0}", ID);
                    ctx.ExecuteCommand("DELETE FROM DocumentSection WHERE DocumentSectionID = {0}", ID);
                }
            }
            else
            {
                success = false;
            }
        }
        catch
        {
            success = false;
        }
        return success;
    }



    protected bool WFDeleteSection(string sectionId)
    {
        bool success = true;
        try
        {
            int ID = 0;
            if (Int32.TryParse(sectionId, out ID))
            {
                using (DocGenDataContext ctx = new DocGenDataContext())
                {
                    ctx.ExecuteCommand("DELETE FROM WorkFlowSection WHERE WorkFlowSectionID = {0}", ID);
                    ctx.ExecuteCommand("DELETE FROM WorkFlowSection WHERE ParentSectionID = {0}", ID);
                }
            }
            else
            {
                success = false;
            }
        }
        catch
        {
            success = false;
        }
        return success;
    }

    protected bool WFSortSections(string sectionIds)
    {
        bool success = true;
        try
        {
            int ID = 0;
            int Counter = 1;
            bool validID = true;
            StringBuilder updateScript = new StringBuilder();
            foreach (string strId in sectionIds.Split(','))
            {
                if (validID)
                {
                    string[] arrTemp = strId.Split('-');
                    if (arrTemp.Length == 3)
                    {
                        if (Int32.TryParse(arrTemp[0], out ID))
                        {
                            if (arrTemp[1] == "")
                            {
                                updateScript.AppendLine(String.Format("UPDATE WorkFlowSection SET Position = {0}, ParentSectionID = NULL  WHERE WorkFlowSectionID = {1};", Counter++, ID));
                            }
                            else
                            {
                                updateScript.AppendLine(String.Format("UPDATE WorkFlowSection SET Position = {0}, ParentSectionID = {1}, ColumnIndex = {2} WHERE WorkFlowSectionID = {3};", Counter++, arrTemp[1], arrTemp[2], ID));
                            }
                        }
                        else
                            validID = false;
                    }
                }
            }
            if (validID)
            {
                using (DocGenDataContext ctx = new DocGenDataContext())
                {
                    ctx.ExecuteCommand(updateScript.ToString());
                }
            }
            else
            {
                success = false;
            }
        }
        catch (Exception ex)
        {
            success = false;
        }
        return success;
    }

    protected bool SortSections(string sectionIds)
    {
        bool success = true;
        try
        {
            int ID = 0;
            int Counter = 1;
            bool validID = true;
            StringBuilder updateScript = new StringBuilder();
            foreach (string strId in sectionIds.Split(','))
            {
                if (validID)
                {
                    string[] arrTemp = strId.Split('-');
                    if (arrTemp.Length == 3)
                    {
                        if (Int32.TryParse(arrTemp[0], out ID))
                        {
                            if (arrTemp[1] == "")
                            {
                                updateScript.AppendLine(String.Format("UPDATE DocumentSection SET Position = {0}, ParentSectionID = NULL  WHERE DocumentSectionID = {1};", Counter++, ID));
                            }
                            else
                            {
                                updateScript.AppendLine(String.Format("UPDATE DocumentSection SET Position = {0}, ParentSectionID = {1}, ColumnIndex = {2} WHERE DocumentSectionID = {3};", Counter++, arrTemp[1], arrTemp[2], ID));
                            }
                        }
                        else
                            validID = false;
                    }
                }
            }
            if (validID)
            {
                using (DocGenDataContext ctx = new DocGenDataContext())
                {
                    ctx.ExecuteCommand(updateScript.ToString());
                }
            }
            else
            {
                success = false;
            }
        }
        catch (Exception ex)
        {
            success = false;
        }
        return success;
    }


}