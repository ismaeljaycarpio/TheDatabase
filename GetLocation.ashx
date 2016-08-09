<%@ WebHandler Language="C#" Class="GetLocation" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using DocGen.DAL;
using System.Collections.Generic;

public class GetLocation : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        string strJSON = "";
        try
        {


            //if (context.Request.QueryString["Reminder"] != null)
            //{
            //    context.Response.Clear();
            //    context.Response.ContentType = "application/json";

            //    if (context.Session["DataReminderCount"] != null)
            //    {
            //        strJSON = context.Session["DataReminderCount"].ToString();
            //    }
            //    context.Response.Write(strJSON);

            //    //Response.End();

            //}

            //if (context.Request.QueryString["Conditions"] != null && context.Request.QueryString["ConditionType"] != null)
            //{
            //    strJSON = "0";
            //    context.Response.Clear();
            //    context.Response.ContentType = "application/json";

            //    if (context.Session["Condition"] != null)
            //    {
            //        DataTable dtCondition = (DataTable)context.Session["Condition"];
                    
            //        if(dtCondition!=null && dtCondition.Rows.Count>0)
            //        {
            //            DataTable dtConFiltered=new DataTable();
            //            DataRow[] drCon = dtCondition.Select("ConditionType='" + context.Request.QueryString["ConditionType"].ToString().Trim() + "'");
            //            if (drCon.Length > 0)
            //                dtConFiltered = dtCondition.Select("ConditionType='" + context.Request.QueryString["ConditionType"].ToString().Trim() + "'").CopyToDataTable();
                        
            //            //DataTable dtConFiltered = dtCondition.Select("ConditionType='" + context.Request.QueryString["ConditionType"].ToString().Trim()+ "'").CopyToDataTable();
            //            if(dtConFiltered!=null && dtConFiltered.Rows.Count>0)
            //            {
            //                strJSON = dtConFiltered.Rows.Count.ToString();
            //            }
            //        }
            //    }
            //    context.Response.Write(strJSON);

            //    //Response.End();

            //}

            if (context.Request.QueryString["Location"] != null)
            {
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                int TableID = 0;
                Int32.TryParse(Convert.ToString(context.Request.QueryString["TableID"]), out TableID);



                StringBuilder sb = new StringBuilder();

                DataTable dtSysName = Common.DataTableFromText(@"SELECT SystemName,TableID,MapPinHoverColumnID,ColumnID  From [Column] WHERE 
                    TableID=" + TableID.ToString() + @" AND  ColumnType='location' ORDER BY DisplayOrder");


                DataTable dtListOfLatLong = new DataTable();
                dtListOfLatLong.Columns.Add("Latitude", typeof(double));
                dtListOfLatLong.Columns.Add("Longitude", typeof(double));
                dtListOfLatLong.AcceptChanges();
                
                if (TableID == -1)
                {
                    dtSysName = Common.DataTableFromText(@" SELECT   DISTINCT  SystemName, [Table].TableID ,MapPinHoverColumnID,ColumnID,PinDisplayOrder
                            FROM         [Column] INNER JOIN
                                                  [Table] ON [Column].TableID = [Table].TableID 
                                                  WHERE [Column].ColumnType='location' AND [Table].IsActive=1 AND [Table].AccountID=" 
                        + context.Session["AccountID"].ToString() + @" ORDER BY PinDisplayOrder");
                }

                foreach (DataRow drSys in dtSysName.Rows)
                {
                   
                     Column theMapPinHoderColumnID    =null;
                     Column theColumn = RecordManager.ets_Column_Details(int.Parse(drSys["ColumnID"].ToString()));
                    if (drSys["MapPinHoverColumnID"] != DBNull.Value)
                    {
                        if (drSys["MapPinHoverColumnID"].ToString() != "")
                        {
                            theMapPinHoderColumnID = RecordManager.ets_Column_Details(int.Parse(drSys["MapPinHoverColumnID"].ToString()));
                        }
                    }
                    
                    Table theTable = RecordManager.ets_Table_Details(int.Parse(drSys["TableID"].ToString()));
                    //DataTable dtRecords = Common.DataTableFromText(" SELECT " + drSys["SystemName"].ToString() + ",RecordID FROM Record WHERE TableID=" + drSys["TableID"].ToString() + " AND IsActive=1 AND " + drSys["SystemName"].ToString() + " IS NOT NULL");

                    DataTable dtRecords = Common.DataTableFromText(" SELECT * FROM Record WHERE TableID=" + drSys["TableID"].ToString() + " AND IsActive=1 AND " + drSys["SystemName"].ToString() + " IS NOT NULL");
                    
                    
                    if (theTable.PinImage == "")
                    {
                        theTable.PinImage = "Pages/Record/PINImages/DefaultPin.png";
                    }

                    foreach (DataRow drRecord in dtRecords.Rows)
                    {

                        if (drRecord[drSys["SystemName"].ToString()] != DBNull.Value)
                        {
                            if (drRecord[drSys["SystemName"].ToString()].ToString() != "")
                            {
                                try
                                {
                                    LocationColumn theLocationColumn = JSONField.GetTypedObject<LocationColumn>(drRecord[drSys["SystemName"].ToString()].ToString());
                                    if (theLocationColumn != null)
                                    {

                                        if (theLocationColumn.Latitude != null && theLocationColumn.Longitude != null)
                                        {
                                             if (TableID == -1)
                                             {
                                                 if(IsLatLongExist(dtListOfLatLong,theLocationColumn))
                                                 {
                                                     continue;
                                                 }
                                                 DataRow drNewLL = dtListOfLatLong.NewRow();
                                                 drNewLL[0] = double.Parse(theLocationColumn.Latitude);
                                                 drNewLL[1] = double.Parse(theLocationColumn.Longitude);
                                                 dtListOfLatLong.Rows.Add(drNewLL);
                                            
                                             }
                                           
                                            string strTT = "VIEW";
                                            if (theMapPinHoderColumnID != null)
                                            {
                                                strTT = Common.GetValueFromSQL("SELECT " + theMapPinHoderColumnID.SystemName+ " FROM Record WHERE RecordID=" + drRecord["RecordID"].ToString());

                                                strTT = TheDatabaseS.EscapeStringValue(strTT);
                                            }
                                           
                                             string strPopup=strTT;
                                             if (theColumn.MapPopup != "")
                                             {
                                                 Record thisRecord = RecordManager.ets_Record_Detail_Full(int.Parse(drRecord["RecordID"].ToString()));
                                                 DataTable _dtRecordColums = RecordManager.ets_Table_Columns_Summary((int)thisRecord.TableID, null);
                                                 
                                                 strPopup = theColumn.MapPopup;//"yes"; //

                                                

                                                 DataTable dtColumns = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE IsStandard=0 AND   TableID="
                               + thisRecord.TableID.ToString() + "  ORDER BY DisplayName");
                                                 foreach (DataRow drC in dtColumns.Rows)
                                                 {
                                                     strPopup = strPopup.Replace("[" + drC["DisplayName"].ToString() + "]", drRecord[drC["SystemName"].ToString()].ToString());

                                                 }
                                                 

                                                 //Work with 1 top level Parent tables.

                                                 try
                                                 {

                                                     if (strPopup.IndexOf("[") > -1 && strPopup.IndexOf(":") > 1 && strPopup.IndexOf("]") > -1)
                                                     {

                                                         DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID="
                                                        + thisRecord.TableID.ToString()); //AND DetailPageType<>'not'

                                                         if (dtPT.Rows.Count > 0)
                                                         {

                                                             foreach (DataRow drPT in dtPT.Rows)
                                                             {

                                                                 if (strPopup.IndexOf("[") > -1 && strPopup.IndexOf(":") > 1 && strPopup.IndexOf("]") > -1)
                                                                 {
                                                                     //
                                                                 }
                                                                 else
                                                                 {
                                                                     break;
                                                                 }

                                                                 for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                                                                 {
                                                                     if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                                                && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                                                || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "tabledd")
                                                                 && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                                                && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                                                                     {
                                                                         if (_dtRecordColums.Rows[i]["TableTableID"].ToString() == drPT["ParentTableID"].ToString())
                                                                         {
                                                                             if (drRecord[_dtRecordColums.Rows[i]["SystemName"].ToString()].ToString() != "")
                                                                             {


                                                                                 Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));

                                                                                 //DataTable dtParentRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + drRecord[_dtRecordColums.Rows[i]["SystemName"].ToString()].ToString());


                                                                                 DataTable dtParentRecord = null;
                                                                                 if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                                                                 {
                                                                                     dtParentRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + drRecord[_dtRecordColums.Rows[i]["SystemName"].ToString()].ToString());
                                                                                 }
                                                                                 else
                                                                                 {
                                                                                     dtParentRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "='" +
                                                                                         drRecord[_dtRecordColums.Rows[i]["SystemName"].ToString()].ToString().Replace("'", "''") + "'");
                                                                                 }

                                                                                 if (dtParentRecord.Rows.Count > 0)
                                                                                 {

                                                                                     DataTable dtColumnsPT = Common.DataTableFromText(@"SELECT distinct SystemName, TableName + ':' + DisplayName AS DP FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE  [Column].IsStandard=0 AND  [Column].TableID=" + drPT["ParentTableID"].ToString());
                                                                                     foreach (DataRow drC in dtColumnsPT.Rows)
                                                                                     {
                                                                                         strPopup = strPopup.Replace("[" + drC["DP"].ToString() + "]", dtParentRecord.Rows[0][drC["SystemName"].ToString()].ToString());

                                                                                     }
                                                                                 }
                                                                             }
                                                                         }
                                                                     }
                                                                 }
                                                             }
                                                         }
                                                         
                                                     }
                                                     
                                                 }
                                                 catch
                                                 {
                                                    //
                                                 }

                                                                                         
                                                 
                                             }



                                             EachPinInfo aPin = new EachPinInfo();
                                             aPin.lat = theLocationColumn.Latitude.ToString();
                                             aPin.lon = theLocationColumn.Longitude.ToString();
                                             aPin.title = strTT;
                                             aPin.pin = "http://" + context.Request.Url.Authority + context.Request.ApplicationPath + "/" + theTable.PinImage;
                                             aPin.ssid = drRecord["RecordID"].ToString();
                                             aPin.url = "http://" + context.Request.Url.Authority + context.Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?fixedurl=" + Cryptography.Encrypt("~/Default.aspx") + "&stackzero=yes&mode=" + Cryptography.Encrypt("view") +
                                                 "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt(drSys["TableID"].ToString()) + "&Recordid=" +
                                                 Cryptography.Encrypt(drRecord["RecordID"].ToString());
                                             aPin.mappopup = strPopup;

                                             sb.Append(",");
                                             sb.Append(aPin.GetJSONString());
                                            
                                            //sb.Append(",{");

                                            //sb.Append(String.Format("\"lat\":{0}, \"lon\":{1}, \"title\":\"{2}\", \"pin\":\"{3}\", \"ssid\":\"{4}\", \"url\":\"{5}\", \"mappopup\":\"{6}\"", theLocationColumn.Latitude.ToString(), theLocationColumn.Longitude.ToString(),
                                            //       HttpUtility.JavaScriptStringEncode(strTT),HttpUtility.JavaScriptStringEncode( "http://" + context.Request.Url.Authority + context.Request.ApplicationPath + "/" +
                                            //       theTable.PinImage),HttpUtility.JavaScriptStringEncode( drRecord["RecordID"].ToString()),HttpUtility.JavaScriptStringEncode( "http://" + context.Request.Url.Authority +
                                            //       context.Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?fixedurl=" + Cryptography.Encrypt("~/Default.aspx") + "&stackzero=yes&mode=" + Cryptography.Encrypt("view") +
                                            //       "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt(drSys["TableID"].ToString()) + "&Recordid=" +
                                            //       Cryptography.Encrypt(drRecord["RecordID"].ToString())), HttpUtility.JavaScriptStringEncode(strPopup)));
                                            
                                            
                                           
                                            
                                            
                                            //sb.Append("}");
                                        }

                                    }

                                }
                                catch (Exception ex)
                                {
                                    ErrorLog theErrorLog = new ErrorLog(null, "Get location each error", ex.Message, ex.StackTrace, DateTime.Now, context.Request.Path);
                                    SystemData.ErrorLog_Insert(theErrorLog);

                                }
                            }

                        }



                    }

                }

                strJSON = sb.ToString();
                if (!String.IsNullOrEmpty(strJSON))
                {
                    strJSON = "[" + strJSON.Substring(1) + "]";
                }


                context.Response.Write(strJSON);
                ////Response.Flush();
                ////Response.End();
                //HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                //HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                //HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Get Locations", ex.Message, ex.StackTrace, DateTime.Now, context.Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);

        }

        
        
        
    }

    private bool IsLatLongExist(DataTable dtListOfLatLong, LocationColumn theLocationColumn)
    {
        foreach (DataRow dr in dtListOfLatLong.Rows)
        {
            if (double.Parse(theLocationColumn.Latitude) == (double)dr[0] && double.Parse(theLocationColumn.Longitude) == (double)dr[1])
            {
                return true;
            }
            
        }
        return false;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}