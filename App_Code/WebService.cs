using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://www.dbgurus.com.au/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {


    

    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

  
     [WebMethod]
     public string GetAccountName(string strEmail, string strPassword)
     {
         User objUser = SecurityManager.User_LoginByEmail(strEmail, strPassword);
         int? iAccountID = SecurityManager.GetPrimaryAccountID((int)objUser.UserID);

         if (objUser != null)
         {
             //User Actived
             if ((bool)objUser.IsActive)
             {
                 //check if the account is active

                 Account theAccount = SecurityManager.Account_Details((int)iAccountID);

                 if (theAccount.IsActive == true)
                 {
                     return theAccount.AccountName;
                 }

             }
         }


         return "";
     }



     [WebMethod]
     public DataSet SyncTable(string strEmail, string strPassword)
     {

         User etUser = SecurityManager.User_LoginByEmail(strEmail, strPassword);
         Account theAccount;
         int? iAccountID = SecurityManager.GetPrimaryAccountID((int)etUser.UserID);
         if (etUser != null)
         {
             //User Actived
             if ((bool)etUser.IsActive)
             {
                 //check if the account is active

                 theAccount = SecurityManager.Account_Details((int)iAccountID);

                 if (theAccount.IsActive == false)
                 {
                     return null;
                 }

             }
             else
             {
                 return null;
             }
         }
         else
         {
             return null;
         }

        

         //make the new dataset


         DataTable dtST = Common.DataTableFromText(@"SELECT  AccountID,TableID, TableName, DateUpdated
                    FROM [Table] WHERE IsActive=1 AND Accountid=" + theAccount.AccountID.ToString());
         dtST.TableName = "ST";

//         DataTable dtSC = Common.DataTableFromText(@"SELECT     [Column].ColumnID, [Column].TableID, [Column].DisplayOrder, [Column].DisplayName, [Column].IsNumeric, 
//                      [Column].DropdownValues, [Column].IsMandatory, [Column].DateUpdated
//                    FROM         [Table] INNER JOIN [Column] ON [Table].TableID = [Column].TableID
//                    WHERE [Table].IsActive=1 AND  [Table].AccountID=" + theAccount.AccountID.ToString());
//         dtSC.TableName = "SC";



         DataTable dtSC = Common.DataTableFromText(@"SELECT     [Column].ColumnID, [Column].TableID, [Column].DisplayOrder, [Column].DisplayName, [Column].IsNumeric, 
                      ISNULL([Column].DropdownValues,'') DropdownValues, [Column].Importance, [Column].DateUpdated, '' as MinValue, '' as MaxValue,ValidationOnEntry
                    FROM         [Table] INNER JOIN [Column] ON [Table].TableID = [Column].TableID
                    WHERE [Table].IsActive=1 AND  [Table].AccountID=" + theAccount.AccountID.ToString());
         dtSC.TableName = "SC";

         for (int i = 0; i < dtSC.Rows.Count; i++)
         {

             if (dtSC.Rows[i]["ValidationOnEntry"] != DBNull.Value && dtSC.Rows[i]["ValidationOnEntry"].ToString() != "")
             {
                 dtSC.Rows[i]["MinValue"] = Common.GetMinFromFormula(dtSC.Rows[i]["ValidationOnEntry"].ToString());
                 dtSC.Rows[i]["MaxValue"] = Common.GetMaxFromFormula(dtSC.Rows[i]["ValidationOnEntry"].ToString());

             }
         }

         dtSC.Columns.RemoveAt(10);

         dtSC.AcceptChanges();




//         DataTable dtSS = Common.DataTableFromText(@"SELECT     Location.LocationID, Location.LocationName, LocationTable.TableID, Location.DateUpdated
//                FROM         LocationTable INNER JOIN
//                Location ON LocationTable.LocationID = Location.LocationID INNER JOIN
//                [Table] ON LocationTable.TableID = [Table].TableID
//                WHERE     (Location.IsActive = 1) AND ([Table].IsActive = 1) AND ([Table].AccountID =" + theAccount.AccountID.ToString() + ")");
         
//         dtSS.TableName = "SS";


         DataSet dsNew = new DataSet();
         dsNew.Tables.Add(dtST);
         dsNew.Tables.Add(dtSC);
         //dsNew.Tables.Add(dtSS);

         DataRelation dtR1 = new DataRelation("STSC", dsNew.Tables["ST"].Columns["TableID"], dsNew.Tables["SC"].Columns["TableID"]);
         dtR1.Nested = true;

         //DataRelation dtR2 = new DataRelation("STSS", dsNew.Tables["ST"].Columns["TableID"], dsNew.Tables["SS"].Columns["TableID"]);
         //dtR2.Nested = true;

         dsNew.Relations.Add(dtR1);
         //dsNew.Relations.Add(dtR2);

         //

       
            return dsNew;
        


         //make the old dataset

         //XmlDocument xmlDoc = new XmlDocument();
         //xmlDoc.LoadXml(strXML);


         //XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

         //DataSet dsOld = new DataSet();
         //dsOld.ReadXml(r);


         /////

         ////lets check [Table] datatable


         //foreach (DataRow drOld in dsOld.Tables["ST"].Rows)
         //{
         //    //DataRow foundRow = dsNew.Tables["ST"].Rows.Find();
             
         //}
         
         
     }



     [WebMethod]
     public DataTable GetRecordXMLTemplate(string strEmail, string strPassword)
     {
         User objUser = SecurityManager.User_LoginByEmail(strEmail, strPassword);
         Account theAccount;
         int? iAccountID = SecurityManager.GetPrimaryAccountID((int)objUser.UserID);
         if (objUser != null)
         {
             //User Actived
             if ((bool)objUser.IsActive)
             {
                 //check if the account is active

                 theAccount = SecurityManager.Account_Details((int)iAccountID);

                 if (theAccount.IsActive == false)
                 {
                     return null;
                 }

             }
             else
             {
                 return null;
             }
         }
         else
         {
             return null;
         }

         DataTable dtTemp = Common.DataTableFromText("SELECT * FROM Batch WHERE BatchID=(SELECT MAX(BatchID) FROM Batch WHERE IsImported=1 AND AccountID=" + theAccount.AccountID.ToString() + ")");
         int iTableID=-1;
         int iBatchID=-1;

         if (dtTemp.Rows.Count > 0)
         {
             iTableID = int.Parse(dtTemp.Rows[0]["TableID"].ToString());
             iBatchID = int.Parse(dtTemp.Rows[0]["BatchID"].ToString());
         }
         else
         {
             return null;

         }
         

         int iTN=0;
         DataTable dtValid = UploadManager.ets_Record_Webservice(iTableID, iBatchID, "", "",null, null, ref iTN, ref iTN);

         DataTable dtInvalidValid = UploadManager.ets_TempRecord_WebService(iTableID, iBatchID, "", "", null, null, ref iTN, ref iTN);

         DataTable dtAll = dtValid.Copy();
         dtAll.Merge(dtInvalidValid);

         dtAll.TableName = "Record";

         dtAll.Columns.RemoveAt(dtAll.Columns.Count - 1);
        


         return dtAll;






     }


     [WebMethod]
     public DataTable SyncRecords(string strEmail, string strPassword, int iTableID, string strXML)
     {

         User objUser = SecurityManager.User_LoginByEmail(strEmail, strPassword);
         int? iAccountID = SecurityManager.GetPrimaryAccountID((int)objUser.UserID);
         Account theAccount;

         if (objUser != null)
         {
             //User Actived
             if ((bool)objUser.IsActive)
             {
                 //check if the account is active

                 theAccount = SecurityManager.Account_Details((int)iAccountID);

                 if (theAccount.IsActive == false)
                 {
                     return null;
                 }

             }
             else
             {
                 return null;
             }
         }
         else
         {
             return null;
         }


         Table theTable = RecordManager.ets_Table_Details(iTableID);

         Guid guidNew = Guid.NewGuid();
         string strFileUniqueName = guidNew.ToString() + ".xml";
         string strRecordsFolder = Server.MapPath("~/UserFiles/AppFiles");
         XmlDocument xmlDoc = new XmlDocument();
         xmlDoc.LoadXml(strXML);

         XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

         DataSet ds = new DataSet();
         ds.ReadXml(r);

         ds.Tables[0].WriteXml(strRecordsFolder + "\\" + strFileUniqueName);

         //SqlTransaction tn;
         //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
         //connection.Open();
         //tn = connection.BeginTransaction();
         try
         {

             int iBatchID;
             string strMsg;

             UploadManager.UploadCSV(objUser.UserID, theTable, "mobile upload by " + objUser.FirstName + " " + objUser.LastName,
          theTable.TableName, guidNew, strRecordsFolder,
          out strMsg, out iBatchID, ".xml", "", (int)iAccountID, null, null, null);


             Batch oBatch = UploadManager.ets_Batch_Details(iBatchID);
             
             //UploadManager.ets_Record_ImportByBatch(iBatchID);

            

             //tn.Commit();
             //tn.Dispose();
             //connection.Close();
             //connection.Dispose();

             string strImportedRecords = "0";
             //UploadManager.MobileSynceEmail(oBatch, ref strImportedRecords);


             string strRollBackSQL = @"DELETE Record WHERE BatchID=" + oBatch.BatchID.ToString() + "; Update Batch set IsImported=0 WHERE BatchID=" + oBatch.BatchID.ToString() + ";";

             try
             {

                 string strImportMsg = UploadManager.ImportClickFucntions(oBatch);

                 if(strImportMsg!="ok")
                 {
                     Common.ExecuteText(strRollBackSQL);
                 }
             }
             catch
             {
                 Common.ExecuteText(strRollBackSQL);
             }



             int iTN = 0;
             DataTable dtValid = UploadManager.ets_Record_Webservice(iTableID, iBatchID, "", "", null, null, ref iTN, ref iTN);

             DataTable dtInvalidValid = UploadManager.ets_TempRecord_WebService(iTableID, iBatchID, "", "", null, null, ref iTN, ref iTN);

             DataTable dtAll = dtValid.Copy();
             dtAll.Merge(dtInvalidValid);

             dtAll.TableName = "Record";

             dtAll.Columns.RemoveAt(dtAll.Columns.Count - 1);



             return dtAll;



         }
         catch (Exception ex)
         {
             //tn.Rollback();
             //tn.Dispose();
             //connection.Close();
             //connection.Dispose();
         }

         return null;


     }
}


