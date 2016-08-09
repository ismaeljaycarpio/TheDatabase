using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DocGen.DAL;

public partial class GetLocations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            string strJSON = "";

            if (Request.QueryString["Reminder"] != null)
            {
                Response.Clear();
                Response.ContentType = "application/json";

                if (Session["DataReminderCount"] != null)
                {
                    strJSON = Session["DataReminderCount"].ToString();
                }
                Response.Write(strJSON);

                Response.End();

            }

           if (Request.QueryString["Conditions"] != null && Request.QueryString["ConditionType"] != null)
            {
                strJSON = "0";
               Response.Clear();
               Response.ContentType = "application/json";

                if (Session["Condition"] != null)
                {
                    DataTable dtCondition = (DataTable)Session["Condition"];

                    if (dtCondition != null && dtCondition.Rows.Count > 0)
                    {
                        DataTable dtConFiltered = new DataTable();
                        DataRow[] drCon = dtCondition.Select("ConditionType='" + Request.QueryString["ConditionType"].ToString().Trim() + "'");
                        if (drCon.Length > 0)
                            dtConFiltered = dtCondition.Select("ConditionType='" + Request.QueryString["ConditionType"].ToString().Trim() + "'").CopyToDataTable();

                        //DataTable dtConFiltered = dtCondition.Select("ConditionType='" + context.Request.QueryString["ConditionType"].ToString().Trim()+ "'").CopyToDataTable();
                        if (dtConFiltered != null && dtConFiltered.Rows.Count > 0)
                        {
                            strJSON = dtConFiltered.Rows.Count.ToString();
                        }
                    }
                }
                else
                {
                    DataTable dtCondition = UploadWorld.dbg_Condition_Select(int.Parse(Request.QueryString["ColumnID"].ToString()), null, Request.QueryString["ConditionType"].ToString(), "");
                    if (dtCondition != null && dtCondition.Rows.Count > 0)
                    {
                        strJSON = dtCondition.Rows.Count.ToString();
                    }
                }
               Response.Write(strJSON);

               Response.End();

            }

//            if (Request.QueryString["Location"] != null)
//            {
//                Response.Clear();
//                Response.ContentType = "application/json";
//                int TableID = 0;
//                Int32.TryParse(Convert.ToString(Request.QueryString["TableID"]), out TableID);



//                StringBuilder sb = new StringBuilder();

//                DataTable dtSysName = Common.DataTableFromText(@"SELECT SystemName,TableID,MapPinHoverColumnID  From [Column] WHERE 
//                    TableID=" + TableID.ToString() + @" AND  ColumnType='location' ORDER BY DisplayOrder");

//                if (TableID == -1)
//                {
//                    dtSysName = Common.DataTableFromText(@" SELECT   DISTINCT  SystemName, [Table].TableID,MapPinHoverColumnID
//                            FROM         [Column] INNER JOIN
//                                                  [Table] ON [Column].TableID = [Table].TableID 
//                                                  WHERE [Column].ColumnType='location' AND [Table].IsActive=1 AND [Table].AccountID=" + Session["AccountID"].ToString());
//                }

//                foreach (DataRow drSys in dtSysName.Rows)
//                {

//                    Column theMapPinHoderColumnID = null;
//                    if (drSys["MapPinHoverColumnID"] != DBNull.Value)
//                    {
//                        if (drSys["MapPinHoverColumnID"].ToString() != "")
//                        {
//                            theMapPinHoderColumnID = RecordManager.ets_Column_Details(int.Parse(drSys["MapPinHoverColumnID"].ToString()));
//                        }
//                    }

//                    Table theTable = RecordManager.ets_Table_Details(int.Parse(drSys["TableID"].ToString()));
//                    DataTable dtRecords = Common.DataTableFromText(" SELECT " + drSys["SystemName"].ToString() + ",RecordID FROM Record WHERE TableID=" + drSys["TableID"].ToString() + " AND IsActive=1 AND " + drSys["SystemName"].ToString() + " IS NOT NULL");


//                    foreach (DataRow drRecord in dtRecords.Rows)
//                    {

//                        if (drRecord[0] != DBNull.Value)
//                        {
//                            if (drRecord[0].ToString() != "")
//                            {
//                                try
//                                {
//                                    LocationColumn theLocationColumn = JSONField.GetTypedObject<LocationColumn>(drRecord[0].ToString());
//                                    if (theLocationColumn != null)
//                                    {

//                                        if (theLocationColumn.Latitude != null && theLocationColumn.Longitude != null)
//                                        {

//                                            string strTT = "VIEW";
//                                            if (theMapPinHoderColumnID != null)
//                                            {
//                                                strTT = Common.GetValueFromSQL("SELECT " + theMapPinHoderColumnID.SystemName + " FROM Record WHERE RecordID=" + drRecord[1].ToString());
//                                                strTT = TheDatabaseS.EscapeStringValue(strTT);
//                                            }

//                                            sb.Append(",{");

//                                            sb.Append(String.Format("\"lat\":{0}, \"lon\":{1}, \"title\":\"{2}\", \"pin\":\"{3}\", \"ssid\":\"{4}\", \"url\":\"{5}\"", theLocationColumn.Latitude.ToString(), theLocationColumn.Longitude.ToString(),
//                                                    HttpUtility.HtmlEncode(strTT), "http://" + Request.Url.Authority + Request.ApplicationPath + "/" +
//                                                    theTable.PinImage, drRecord[1].ToString(), "http://" + Request.Url.Authority +
//                                                    Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("view") +
//                                                    "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=" + Cryptography.Encrypt(drSys["TableID"].ToString()) + "&Recordid=" +
//                                                    Cryptography.Encrypt(drRecord[1].ToString())));
//                                            sb.Append("}");
//                                        }

//                                    }

//                                }
//                                catch
//                                {
//                                    //
//                                }
//                            }

//                        }



//                    }

//                }

//                strJSON = sb.ToString();
//                if (!String.IsNullOrEmpty(strJSON))
//                {
//                    strJSON = "[" + strJSON.Substring(1) + "]";
//                }


//                Response.Write(strJSON);
//                //Response.Flush();
//                //Response.End();
//                HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
//                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
//                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
//            }
        }
        catch (Exception ex)
        {

            //ErrorLog theErrorLog = new ErrorLog(null, "Get Locations", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            //SystemData.ErrorLog_Insert(theErrorLog);

        }

    }
}


