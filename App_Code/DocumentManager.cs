using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using BaseClasses;
using DocGen;
using System.IO;
/// <summary>
/// Summary description for DocumentManager
/// </summary>
public class DocumentManager
{
	public DocumentManager()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region ETS Document



    public static int CloneDocument(int DocumentID, string txtStartDate, string txtEndDate, string txtDocumentText, string txtDocumentDescription)
    {

        int NewDocID = -1;
        Dictionary<int, string> dicSectionImages = new Dictionary<int, string>();
        int TextSectionTypeID = Convert.ToInt32(ConfigurationManager.AppSettings["TextSectionTypeID"]);
        int ImageSectionTypeID = Convert.ToInt32(ConfigurationManager.AppSettings["ImageSectionTypeID"]);


        DateTime? dtDateFrom = null;
        DateTime? dtDateTo = null;

        Document theDocument = DocumentManager.ets_Document_Detail(DocumentID);
        dtDateFrom = theDocument.DocumentDate;
        dtDateTo = theDocument.DocumentEndDate;

        if (txtStartDate != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtStartDate.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                dtDateFrom = dtTemp;
            }
        }
        if (txtEndDate != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtEndDate.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                dtDateTo = dtTemp;

            }
        }

        using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
        {
            DateTime ThisMoment = DateTime.Now;
            DocGen.DAL.Document newDoc;
            DocGen.DAL.Document doc = ctx.Documents.SingleOrDefault<DocGen.DAL.Document>(d => d.DocumentID == DocumentID);

            //Clone common info
            newDoc = new DocGen.DAL.Document()
            {
                DocumentTypeID = doc.DocumentTypeID,
                DocumentText = txtDocumentText==""?theDocument.DocumentText:txtDocumentText,
                DocumentDescription =txtDocumentDescription==""? theDocument.DocumentDescription:txtDocumentDescription,
                DocumentDate = dtDateFrom,
                DocumentEndDate = dtDateTo,
                DateAdded = ThisMoment,
                DateUpdated = ThisMoment,
                AccountID = int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()),
                UserID = (int)((User)System.Web.HttpContext.Current.Session["User"]).UserID,
                UniqueName = "",
                FileTitle = ""
            };

            //Clone document sections
            foreach (DocGen.DAL.DocumentSection section in doc.DocumentSections)
            {
                bool bAdd=true;
                DocGen.DAL.DocumentSection newSection = new DocGen.DAL.DocumentSection()
                {
                    DocumentSectionTypeID = section.DocumentSectionTypeID,
                    Position = section.Position,
                    SectionName = section.SectionName,
                    Content = section.Content,
                    Filter = section.Filter,
                    Details = section.Details,
                    DateAdded = ThisMoment,
                    DateUpdated = ThisMoment,
                    ValueFields = section.ValueFields,
                    DocumentID = newDoc.DocumentID,
                    DocumentSectionStyleID = section.DocumentSectionStyleID,
                    ColumnIndex=section.ColumnIndex,
                    ParentSectionID = section.ParentSectionID
                    
                };


                if (section.DocumentSectionTypeID == 10)//Record Table
                {
                    if (section.Details != "")
                    {
                        DocGen.DAL.RecordTableSectionDetail rtDetail = DocGen.DAL.JSONField.GetTypedObject<DocGen.DAL.RecordTableSectionDetail>(section.Details);

                        if (rtDetail != null)
                        {
                            if (rtDetail.TableID != null && rtDetail.ViewID!=null)
                            {
                                
                              //copt view items
                                try
                                {
                                    int? iNewViewID = ViewManager.dbg_View_Copy((int)rtDetail.ViewID, (int)((User)System.Web.HttpContext.Current.Session["User"]).UserID);
                                    rtDetail.ViewID =(int) iNewViewID;
                                    section.Details = rtDetail.GetJSONString();
                                }
                                catch
                                {
                                    //
                                    try
                                    {
                                        // Create dashboard view
                                        User objUser = (User)System.Web.HttpContext.Current.Session["User"];
                                        //int iNewViewID = ViewManager.CreateDashView(rtDetail.TableID);
                                        int? iNewViewID = ViewManager.dbg_View_CreateDash((int)objUser.UserID, (int)rtDetail.TableID);
                                        rtDetail.ViewID = (int)iNewViewID;
                                        section.Details = rtDetail.GetJSONString();
                                        //ViewManager.dbg_CreateDefaultViewItem(iNewViewID);
                                    }
                                    catch
                                    {
                                        bAdd = false;
                                        ctx.ExecuteCommand("DELETE FROM DocumentSection WHERE DocumentSectionID = {0}", section.DocumentSectionID.ToString());
                                    }
                                   
                                }
                                
                                ctx.SubmitChanges();
                            }

                        }

                    }
                }


                if (section.DocumentSectionTypeID == ImageSectionTypeID)//Type = Image
                {
                    string FilePath = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", section.DocumentSectionID));
                    if (File.Exists(FilePath))
                    {
                        dicSectionImages.Add(section.Position, FilePath);
                    }
                }
                if(bAdd)
                    newDoc.DocumentSections.Add(newSection);
            }

            ctx.Documents.InsertOnSubmit(newDoc);
            ctx.SubmitChanges();

            NewDocID = newDoc.DocumentID;
            foreach (int sectionPosition in dicSectionImages.Keys.ToList<int>())
            {
                DocGen.DAL.DocumentSection sectionWithAttachedImage = newDoc.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(iSection => iSection.Position == sectionPosition);
                if (sectionWithAttachedImage != null)
                {
                    File.Copy(dicSectionImages[sectionPosition], System.Web.HttpContext.Current.Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", sectionWithAttachedImage.DocumentSectionID)));
                }
            }



            DocGen.DAL.Document docNew = ctx.Documents.SingleOrDefault<DocGen.DAL.Document>(d => d.DocumentID == NewDocID);

            foreach (DocGen.DAL.DocumentSection sectionNew in docNew.DocumentSections)
            {

                if (sectionNew.ParentSectionID != null)
                {
                    DocGen.DAL.DocumentSection sectionP = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(d => d.DocumentSectionID == sectionNew.ParentSectionID);

                    if (sectionP != null)
                    {
                        DocGen.DAL.DocumentSection sectionPNew = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(d => d.DocumentID == newDoc.DocumentID && d.Position == sectionP.Position);

                        if (sectionPNew != null)
                        {
                            sectionNew.ParentSectionID = sectionPNew.DocumentSectionID;
                        }
                    }
                    else
                    {
                        sectionNew.ParentSectionID = null;
                    }
                    ctx.SubmitChanges();
                }

            }
            
           
           
        }

        

        return NewDocID;

    }

    public static int ets_Document_Insert(Document p_Document)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Document_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@nAccountID", p_Document.AccountID));
                command.Parameters.Add(new SqlParameter("@sDocumentText", p_Document.DocumentText));
                
                if (p_Document.DocumentTypeID!=null)
                command.Parameters.Add(new SqlParameter("@nDocumentTypeID", p_Document.DocumentTypeID));

                command.Parameters.Add(new SqlParameter("@sUniqueName", p_Document.FileUniqename));
                command.Parameters.Add(new SqlParameter("@sFileTitle", p_Document.FileTitle));

                if (p_Document.DocumentDate!=null)
                command.Parameters.Add(new SqlParameter("@dDocumentDate", p_Document.DocumentDate));

                if (p_Document.UserID != null)
                 command.Parameters.Add(new SqlParameter("@nUserID", p_Document.UserID));

                if ( p_Document.TableID!=null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_Document.TableID));


                if (p_Document.ReportHTML != "")
                    command.Parameters.Add(new SqlParameter("@sReportHTML", p_Document.ReportHTML));
                if (p_Document.IsReportPublic != null)
                    command.Parameters.Add(new SqlParameter("@bIsReportPublic", p_Document.IsReportPublic));

                if (p_Document.DocumentDescription != "")
                    command.Parameters.Add(new SqlParameter("@sDocumentDescription", p_Document.DocumentDescription));

                if (p_Document.DocumentEndDate != null)
                    command.Parameters.Add(new SqlParameter("@dateDocumentEndDate", p_Document.DocumentEndDate));

                if (p_Document.FolderID != null)
                    command.Parameters.Add(new SqlParameter("@nFolderID", p_Document.FolderID));

                if (p_Document.Size != null)
                    command.Parameters.Add(new SqlParameter("@dSize", p_Document.Size));

                if (p_Document.ReportType != "")
                    command.Parameters.Add(new SqlParameter("@sReportType", p_Document.ReportType));

                if (p_Document.ForDashBoard != null)
                    command.Parameters.Add(new SqlParameter("@bForDashBoard", p_Document.ForDashBoard));

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return -1;
            }
        }
    }



    public static int ets_Document_Update(Document p_Document)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Document_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nDocumentID", p_Document.DocumentID));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Document.AccountID));
                command.Parameters.Add(new SqlParameter("@sDocumentText", p_Document.DocumentText));

                if (p_Document.DocumentTypeID != null)
                command.Parameters.Add(new SqlParameter("@nDocumentTypeID", p_Document.DocumentTypeID));

                command.Parameters.Add(new SqlParameter("@sUniqueName", p_Document.FileUniqename));
                command.Parameters.Add(new SqlParameter("@sFileTitle", p_Document.FileTitle));

                if (p_Document.DocumentDate != null)
                command.Parameters.Add(new SqlParameter("@dDocumentDate", p_Document.DocumentDate));

                if (p_Document.UserID != null)
                command.Parameters.Add(new SqlParameter("@nUserID", p_Document.UserID));

                if (p_Document.TableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", p_Document.TableID));

                if (p_Document.ReportHTML != "")
                    command.Parameters.Add(new SqlParameter("@sReportHTML", p_Document.ReportHTML));
                if (p_Document.IsReportPublic != null)
                    command.Parameters.Add(new SqlParameter("@bIsReportPublic", p_Document.IsReportPublic));


                if (p_Document.DocumentDescription != "")
                    command.Parameters.Add(new SqlParameter("@sDocumentDescription", p_Document.DocumentDescription));

                if (p_Document.DocumentEndDate != null)
                    command.Parameters.Add(new SqlParameter("@dateDocumentEndDate", p_Document.DocumentEndDate));

                if (p_Document.FolderID != null)
                    command.Parameters.Add(new SqlParameter("@nFolderID", p_Document.FolderID));

                if (p_Document.Size != null)
                    command.Parameters.Add(new SqlParameter("@dSize", p_Document.Size));

                if (p_Document.ReportType != "")
                    command.Parameters.Add(new SqlParameter("@sReportType", p_Document.ReportType));

                if (p_Document.ForDashBoard != null)
                    command.Parameters.Add(new SqlParameter("@bForDashBoard", p_Document.ForDashBoard));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;



            }
        }
    }




    public static DataTable ets_Document_Select(int? nDocumentID,
    int? nAccountID, string sDocumentText, int? nDocumentTypeID,
        DateTime? dFromDocumentDate, DateTime? dToDocumentDate, DateTime? dDateAdded, DateTime? dDateUpdated, int? nUserID,
        string sOrder,
  string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, int? nTableID,
        string sTableIn, bool? bForPuiblic, int? nParentMenuID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Document_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));

                if (nParentMenuID != null)
                    command.Parameters.Add(new SqlParameter("@nParentMenuID", nParentMenuID));

                if (sTableIn != "")
                    command.Parameters.Add(new SqlParameter("@sTableIn", sTableIn));

                if (bForPuiblic != null)
                    command.Parameters.Add(new SqlParameter("@bForPuiblic", bForPuiblic));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nDocumentID != null)
                command.Parameters.Add(new SqlParameter("@nDocumentID", nDocumentID));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sDocumentText != "")
                    command.Parameters.Add(new SqlParameter("@sDocumentText", sDocumentText));

                if (nDocumentTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nDocumentTypeID", nDocumentTypeID));

               

                if (dFromDocumentDate != null)
                    command.Parameters.Add(new SqlParameter("@dFromDocumentDate", dFromDocumentDate));
                if (dToDocumentDate != null)
                    command.Parameters.Add(new SqlParameter("@dToDocumentDate", dToDocumentDate));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));
               
                if (sOrder=="")
                sOrder = "DocumentDate";

                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();

                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }
                connection.Close();
                connection.Dispose();


                iTotalRowsNum = 0;
                if (ds != null && ds.Tables.Count > 1)
                {                 
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
        }
    }







    public static DataTable ets_Dashboard_Select(  int? nAccountID, string sDocumentText, string sDocumentIDs,   string sOrder,
string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Dashboard_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));
               

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sDocumentText != "")
                    command.Parameters.Add(new SqlParameter("@sDocumentText", sDocumentText));

                if (sDocumentIDs != "")
                    command.Parameters.Add(new SqlParameter("@sDocumentIDs", sDocumentIDs));


                if (sOrder == "")
                    sOrder = "DocumentText";

                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();

                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();


                iTotalRowsNum = 0;
                if (ds != null && ds.Tables.Count > 1)
                {
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
        }
    }



    public static int? dbg_Dashboard_BestFitting(string DocumentText, int UserID, int RoleID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Dashboard_BestFitting", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (DocumentText!="")
                    command.Parameters.Add(new SqlParameter("@DocumentText", DocumentText));

                command.Parameters.Add(new SqlParameter("@RoleID", RoleID));
                command.Parameters.Add(new SqlParameter("@UserID", UserID));

                SqlParameter opDocumentID = new SqlParameter("@DocumentID", SqlDbType.Int);
                opDocumentID.Direction = ParameterDirection.Output;

                command.Parameters.Add(opDocumentID);


                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    if (opDocumentID.Value.ToString()!="")
                        return int.Parse(opDocumentID.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();
                }

                return null;

            }
        }


    }


    public static Document dbg_Document_BestFittingDash(int? nUserRoleID, string sDocumentText)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Document_BestFittingDash", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserRoleID", nUserRoleID));


                if (sDocumentText != "")
                    command.Parameters.Add(new SqlParameter("@sDocumentText", sDocumentText));


                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Document temp = new Document(
                                    (int)reader["DocumentID"], (int)reader["AccountID"], (string)reader["DocumentText"],
                                     reader["DocumentTypeID"] == DBNull.Value ? null : (int?)reader["DocumentTypeID"],
                                (string)reader["UniqueName"], (string)reader["FileTitle"],
                                reader["DocumentDate"] == DBNull.Value ? null : (DateTime?)reader["DocumentDate"],
                                (DateTime)reader["DateAdded"], (DateTime)reader["DateUpdated"],
                               reader["UserID"] == DBNull.Value ? null : (int?)reader["UserID"], reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"]
                                );
                            temp.ReportHTML = reader["ReportHTML"] == DBNull.Value ? "" : (string)reader["ReportHTML"];
                            temp.IsReportPublic = reader["IsReportPublic"] == DBNull.Value ? null : (bool?)reader["IsReportPublic"];

                            temp.DocumentDescription = reader["DocumentDescription"] == DBNull.Value ? "" : (string)reader["DocumentDescription"];
                            temp.DocumentEndDate = reader["DocumentEndDate"] == DBNull.Value ? null : (DateTime?)reader["DocumentEndDate"];

                            temp.ForDashBoard = reader["ForDashBoard"] == DBNull.Value ? null : (bool?)reader["ForDashBoard"];
                            temp.FolderID = reader["FolderID"] == DBNull.Value ? null : (int?)reader["FolderID"];

                            temp.Size = reader["Size"] == DBNull.Value ? null : (double?)double.Parse(reader["Size"].ToString());

                            temp.ReportType = reader["ReportType"] == DBNull.Value ? "" : (string)reader["ReportType"];

                           
                                connection.Close();
                                connection.Dispose();
                            

                            return temp;
                        }

                    }
                }
                catch
                {
                    
                }
                connection.Close();
                connection.Dispose();
                return null;

            }

        }

    }


    public static DataTable ets_FolderDocument_Select(int? nDocumentID,
  int? nAccountID, string sDocumentText, int? nDocumentTypeID,
      DateTime? dFromDocumentDate, DateTime? dToDocumentDate, DateTime? dDateAdded, DateTime? dDateUpdated, int? nUserID,
      string sOrder,
string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum, int? nTableID,
      string sTableIn, bool? bForPuiblic, int? nParentFolderID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_FolderDocument_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                //bool filter = !(HttpContext.Current.User.IsInRole("Friends") || HttpContext.Current.User.IsInRole("Administrators"));
                if (sTableIn != "")
                    command.Parameters.Add(new SqlParameter("@sTableIn", sTableIn));

                if (bForPuiblic != null)
                    command.Parameters.Add(new SqlParameter("@bForPuiblic", bForPuiblic));

                if (nTableID != null)
                    command.Parameters.Add(new SqlParameter("@nTableID", nTableID));

                if (nDocumentID != null)
                    command.Parameters.Add(new SqlParameter("@nDocumentID", nDocumentID));

                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sDocumentText != "")
                    command.Parameters.Add(new SqlParameter("@sDocumentText", sDocumentText));

                if (nDocumentTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nDocumentTypeID", nDocumentTypeID));



                if (dFromDocumentDate != null)
                    command.Parameters.Add(new SqlParameter("@dFromDocumentDate", dFromDocumentDate));
                if (dToDocumentDate != null)
                    command.Parameters.Add(new SqlParameter("@dToDocumentDate", dToDocumentDate));

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

                if (sOrder == "")
                    sOrder = "DocumentDate";

                command.Parameters.Add(new SqlParameter("@sOrder", "[" + sOrder + "] " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));

                //nParentFolderID

                if (nParentFolderID != null)
                    command.Parameters.Add(new SqlParameter("@nParentFolderID", nParentFolderID));


              
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();

                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();


                iTotalRowsNum = 0;
                if (ds != null && ds.Tables.Count > 1)
                {
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
        }
    }


    public static int ets_Document_Delete(int nDocumentID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Document_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nDocumentID", nDocumentID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;




            }
        }
    }



    public static int dbg_Dashboard_ResetUsers_ByRole(int RoleID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_Dashboard_ResetUsers_ByRole", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RoleID", RoleID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;




            }
        }
    }

    public static Document ets_Document_Detail(int nDocumentID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Document_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nDocumentID", nDocumentID));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Document temp = new Document(
                                (int)reader["DocumentID"], (int)reader["AccountID"], (string)reader["DocumentText"],
                                 reader["DocumentTypeID"] == DBNull.Value ? null : (int?)reader["DocumentTypeID"],
                            (string)reader["UniqueName"], (string)reader["FileTitle"],
                            reader["DocumentDate"] == DBNull.Value ? null : (DateTime?)reader["DocumentDate"],
                            (DateTime)reader["DateAdded"], (DateTime)reader["DateUpdated"],
                           reader["UserID"] == DBNull.Value ? null : (int?)reader["UserID"], reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"]
                            );
                            temp.ReportHTML = reader["ReportHTML"] == DBNull.Value ? "" : (string)reader["ReportHTML"];
                            temp.IsReportPublic = reader["IsReportPublic"] == DBNull.Value ? null : (bool?)reader["IsReportPublic"];

                            temp.DocumentDescription = reader["DocumentDescription"] == DBNull.Value ? "" : (string)reader["DocumentDescription"];
                            temp.DocumentEndDate = reader["DocumentEndDate"] == DBNull.Value ? null : (DateTime?)reader["DocumentEndDate"];

                            temp.ForDashBoard = reader["ForDashBoard"] == DBNull.Value ? null : (bool?)reader["ForDashBoard"];
                            temp.FolderID = reader["FolderID"] == DBNull.Value ? null : (int?)reader["FolderID"];

                            temp.Size = reader["Size"] == DBNull.Value ? null : (double?)double.Parse(reader["Size"].ToString());

                            temp.ReportType = reader["ReportType"] == DBNull.Value ? "" : (string)reader["ReportType"];


                            connection.Close();
                            connection.Dispose();
                            return temp;
                        }
                    }
                }
                catch
                {

                    
                }
                connection.Close();
                connection.Dispose();

                return null;
            }
        }
    }


    #endregion

    #region ETS DocumentType

    public static int ets_DocumentType_Insert(DocumentType p_DocumentType)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DocumentType_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@sDocumentTypeName", p_DocumentType.DocumentTypeName));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_DocumentType.AccountID));

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return -1;
            }
        }
    }

    public static int ets_DocumentType_Update(DocumentType p_DocumentType)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DocumentType_Update", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nDocumentTypeID", p_DocumentType.DocumentTypeID));
                command.Parameters.Add(new SqlParameter("@sDocumentTypeName", p_DocumentType.DocumentTypeName));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_DocumentType.AccountID));


                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }



    public static int ets_DocumentType_Delete(int nDocumentTypeID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DocumentType_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nDocumentTypeID", nDocumentTypeID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }



    public static List<DocumentType> ets_DocumentType_Select(int? nDocumentTypeID, string sDocumentTypeName, int? nAccountID,
       DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
     string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DocumentType_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (nDocumentTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nDocumentTypeID", nDocumentTypeID));

                if (sDocumentTypeName != string.Empty)
                    command.Parameters.Add(new SqlParameter("@sDocumentTypeName", sDocumentTypeName));
                if (nAccountID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));
               

                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "DocumentTypeID"; sOrderDirection = "DESC"; }
                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                List<DocumentType> list = new List<DocumentType>();
                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DocumentType temp = new DocumentType(
                                (int)reader["DocumentTypeID"],
                                (int)reader["AccountID"],
                                (string)reader["DocumentTypeName"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]);

                            list.Add(temp);

                        }

                        reader.NextResult();
                        while (reader.Read())
                        {
                            iTotalRowsNum = (int)reader["TotalRows"];
                        }
                    }
                }
                catch
                {
                    list = null;
                }

                

                connection.Close();
                connection.Dispose();

                return list;
            }
        }
    }



    public static DocumentType ets_DocumentType_Detail(int nDocumentTypeID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_DocumentType_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nDocumentTypeID", nDocumentTypeID));
                connection.Open();

                try
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DocumentType temp = new DocumentType(
                                 (int)reader["DocumentTypeID"],
                                 (int)reader["AccountID"],
                                 (string)reader["DocumentTypeName"],
                                 (DateTime)reader["DateAdded"],
                                 (DateTime)reader["DateUpdated"]);

                            connection.Close();
                            connection.Dispose();

                            return temp;
                        }
                    }

                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();
                return null;
            }
        }
    }



    #endregion





    #region Folder



    public static int ets_Folder_Insert(Folder p_Folder)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Folder_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Folder.AccountID));
                if (p_Folder.ParentFolderID != null)
                    command.Parameters.Add(new SqlParameter("@nParentFolderID", p_Folder.ParentFolderID));
                command.Parameters.Add(new SqlParameter("@sFolderName", p_Folder.FolderName));


                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    
                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                    return -1;
                }    
            }
        }

    }


    public static int ets_Folder_Update(Folder p_Folder)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_Folder_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@nFolderID", p_Folder.FolderID));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Folder.AccountID));
                if (p_Folder.ParentFolderID != null)
                    command.Parameters.Add(new SqlParameter("@nParentFolderID", p_Folder.ParentFolderID));
                command.Parameters.Add(new SqlParameter("@sFolderName", p_Folder.FolderName));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;
            }
        }



    }



    public static DataTable ets_Folder_Select(int? nAccountID, int? nParentFolderID, string sFolderName,
        string sOrder,string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Folder_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (nParentFolderID != null)
                    command.Parameters.Add(new SqlParameter("@nParentFolderID", nParentFolderID));

                if (sFolderName != "")
                    command.Parameters.Add(new SqlParameter("@sReminderHeader", sFolderName));


                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "FolderID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();

                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();

                iTotalRowsNum = 0;
                if (ds != null && ds.Tables.Count > 1)
                {
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }


            }
        }
    }




    public static int ets_Folder_Delete(int nFolderID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Folder_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nFolderID ", nFolderID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }




    public static Folder ets_Folder_Detail(int nFolderID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("ets_Folder_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nFolderID", nFolderID));

                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Folder temp = new Folder(
                                (int)reader["FolderID"], (int)reader["AccountID"],
                               reader["ParentFolderID"] == DBNull.Value ? null : (int?)reader["ParentFolderID"],
                              (string)reader["FolderName"]
                                );


                            connection.Close();
                            connection.Dispose();


                            return temp;
                        }

                    }
                }
                catch
                {
                  
                }
                connection.Close();
                connection.Dispose();

                return null;

            }
        }



    }












    #endregion



    #region UserFolder


    public static int ets_UserFolder_Insert(UserFolder p_UserFolder)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserFolder_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nFolderID", p_UserFolder.FolderID));
                command.Parameters.Add(new SqlParameter("@nUserID", p_UserFolder.UserID));
                command.Parameters.Add(new SqlParameter("@sRightType", p_UserFolder.RightType));


                connection.Open();
                try
                {
                    command.ExecuteNonQuery();

                    connection.Close();
                    connection.Dispose();


                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();


                    return -1;
                }

            }
        }

    }


    public static int ets_UserFolder_Update(UserFolder p_UserFolder)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserFolder_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserFolderID", p_UserFolder.UserFolderID));
                command.Parameters.Add(new SqlParameter("@nFolderID", p_UserFolder.FolderID));
                command.Parameters.Add(new SqlParameter("@nUserID", p_UserFolder.UserID));
                command.Parameters.Add(new SqlParameter("@sRightType", p_UserFolder.RightType));



                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;
            }
        }
        

        

    }



    public static DataTable ets_UserFolder_Select(int? nUserFolderID, int? nFolderID, int? nUserID,
       string sRightType, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserFolder_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (nUserFolderID != null)
                command.Parameters.Add(new SqlParameter("@nUserFolderID", nUserFolderID));

                if (nFolderID != null)
                    command.Parameters.Add(new SqlParameter("@nFolderID", nFolderID));

                if (nUserID != null)
                    command.Parameters.Add(new SqlParameter("@nUserID", nUserID));

                if (sRightType != "")
                    command.Parameters.Add(new SqlParameter("@sRightType", sRightType));



                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "UserFolderID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();

                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();

                iTotalRowsNum = 0;

                if (ds == null) return null;

                if (ds.Tables.Count > 1)
                {
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }


            }
        }
    }




    public static int ets_UserFolder_Delete(int nUserFolderID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserFolder_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nUserFolderID ", nUserFolderID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }




    public static UserFolder ets_UserFolder_Detail(int nUserFolderID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_UserFolder_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nUserFolderID", nUserFolderID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserFolder temp = new UserFolder(
                                (int)reader["UserFolderID"], (int)reader["FolderID"], (int)reader["UserID"],
                               (string)reader["RightType"]);


                            connection.Close();
                            connection.Dispose();


                            return temp;
                        }

                    }

                }
                catch
                {
                   
                }


                connection.Close();
                connection.Dispose();

                    
                return null;
            }

        }              

    }






    #endregion



    #region DataRetriever



  


    public static DataRetriever dbg_DataRetriever_Detail(int nDataRetrieverID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_DataRetriever_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
              
                command.Parameters.Add(new SqlParameter("@nDataRetrieverID", nDataRetrieverID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataRetriever temp = new DataRetriever(
                                (int)reader["DataRetrieverID"],
                              reader["TableID"] == DBNull.Value ? null : (int?)reader["TableID"],
                              reader["SPName"] == DBNull.Value ? string.Empty : (string)reader["SPName"], (string)reader["DataRetrieverName"]);

                            temp.CodeSnippet = reader["CodeSnippet"] == DBNull.Value ? string.Empty : (string)reader["CodeSnippet"];


                            connection.Close();
                            connection.Dispose();


                            return temp;
                        }
                    }
                }
                catch
                {
                   
                }
                connection.Close();
                connection.Dispose();

                return null;

            }

        }

    }




    #endregion




    #region DocTemplate



    public static int dbg_DocTemplate_Insert(DocTemplate p_DocTemplate)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_DocTemplate_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nDataRetrieverID", p_DocTemplate.DataRetrieverID));
                command.Parameters.Add(new SqlParameter("@sFileName", p_DocTemplate.FileName));
                command.Parameters.Add(new SqlParameter("@sFileUniqueName", p_DocTemplate.FileUniqueName));



                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return -1;


               
              
            }

        }

        

    }


    public static int dbg_DocTemplate_Update(DocTemplate p_DocTemplate)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_DocTemplate_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nDocTemplateID", p_DocTemplate.DocTemplateID));
                command.Parameters.Add(new SqlParameter("@nDataRetrieverID", p_DocTemplate.DataRetrieverID));
                command.Parameters.Add(new SqlParameter("@sFileName", p_DocTemplate.FileName));
                command.Parameters.Add(new SqlParameter("@sFileUniqueName", p_DocTemplate.FileUniqueName));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }

        }       

    }



    public static DataTable dbg_DocTemplate_Select( int? nDataRetrieverID, string sFileUniqueName,
        string sFileName, 
        string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_DocTemplate_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nDataRetrieverID", nDataRetrieverID));

                if (sFileUniqueName != "")
                    command.Parameters.Add(new SqlParameter("@sFileUniqueName", sFileUniqueName));

                if (sFileName != "")
                    command.Parameters.Add(new SqlParameter("@sFileName", sFileName));

                

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "DocTemplateID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();

                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();

                iTotalRowsNum = 0;

                if (ds == null) return null;

                if (ds.Tables.Count > 1)
                {
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }


            }
        }
    }




    public static DataTable DataRetrieverSP(int? nTableID, int? nRecordID,string SPName)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand(SPName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nTableID", nTableID));


                if (nRecordID == null)
                    nRecordID = -1;
                command.Parameters.Add(new SqlParameter("@nRecordID", nRecordID));

              
                
               

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();


                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();

                if (ds == null) return null;

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }


            }
        }
    }


    public static int dbg_DocTemplate_Delete(int nDocTemplateID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_DocTemplate_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nDocTemplateID ", nDocTemplateID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;

            }
        }
    }




    public static DocTemplate dbg_DocTemplate_Detail(int nDocTemplateID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {

            using (SqlCommand command = new SqlCommand("dbg_DocTemplate_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nDocTemplateID", nDocTemplateID));

                connection.Open();


                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DocTemplate temp = new DocTemplate(
                                (int)reader["DocTemplateID"],
                              reader["FileUniqueName"] == DBNull.Value ? string.Empty : (string)reader["FileUniqueName"],
                              reader["FileName"] == DBNull.Value ? string.Empty : (string)reader["FileName"],
                              reader["DataRetrieverID"] == DBNull.Value ? null : (int?)reader["DataRetrieverID"]);

                            
                                connection.Close();
                                connection.Dispose();
                            

                            return temp;
                        }

                    }
                }
                catch
                {
                  
                }

                connection.Close();
                connection.Dispose();
                

                return null;

            }


        }

    }







    #endregion











}
