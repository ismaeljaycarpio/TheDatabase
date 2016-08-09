using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class Pages_User_SendEmail : SecurePage 
{
    Table _theTable;
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Send Email";

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", " $('textarea').autogrow();", true);
        if (!IsPostBack)
        {
           

            hfFileName.Value =  Cryptography.Decrypt(Request.QueryString["FileName"].ToString());

            if(Request.QueryString["TableID"]!=null)
             _theTable = RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())));


            if (Cryptography.Decrypt(Request.QueryString["Source"].ToString()).ToLower() == "recordlist")
            {
                if (Request.QueryString["SearchCriteriaID2"] != null)
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&SearchCriteriaID2=" + Request.QueryString["SearchCriteriaID2"].ToString();
                }
                else
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() ;
                }
                txtSubject.Text = _theTable.TableName + " Records";
                lblAttachmnet.Text = _theTable.TableName + " Records.csv";
                txtMessage.Text = "Please find attached " + _theTable.TableName + " Records in CSV format";
            }
            else if (Cryptography.Decrypt(Request.QueryString["Source"].ToString()).ToLower() == "jsgraph")
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/RecordChart.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() ;
                txtMessage.Text = "Please find attached graph image in PNG format.";
                string strFileName = Common.GetValidFileName(hfFileName.Value);
                txtSubject.Text = _theTable.TableName + " - Graph";
                lblAttachmnet.Text = _theTable.TableName + " - Graph.png";
            }
            else if (Cryptography.Decrypt(Request.QueryString["Source"].ToString()).ToLower() == "graph")
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/RecordChart.aspx?TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaID=" + Request.QueryString["SearchCriteriaID"].ToString() + "&SearchCriteriaID2=" + Request.QueryString["SearchCriteriaID2"].ToString();
                txtMessage.Text = "Please find attached graph image in PDF format.";

                txtSubject.Text = _theTable.TableName + " - Graph";
                lblAttachmnet.Text = _theTable.TableName + " - Graph.pdf";
            }
            else if (Cryptography.Decrypt(Request.QueryString["Source"].ToString()).ToLower() == "graphlist")
            {
                if (Request.QueryString["GraphOptionID"] != null)
                {

                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/GraphOptionDetail.aspx?page=list&mode=" + Request.QueryString["mode"].ToString() +
                        "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() +
                        "&GraphOptionID=" + Request.QueryString["GraphOptionID"].ToString() +
                        "&SearchCriteriaID2=" + Request.QueryString["SearchCriteriaID2"].ToString();
                    txtMessage.Text = "Please find attached graph image in PDF format.";

                    GraphOption theGraphOption = GraphManager.ets_GraphOption_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["GraphOptionID"].ToString())));

                    if (theGraphOption != null)
                    {
                        txtSubject.Text = theGraphOption.Heading;
                        lblAttachmnet.Text = theGraphOption.Heading + ".pdf";
                    }
                }
                else
                {
                    hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/GraphOptionDetail.aspx?page=list&mode=" + Request.QueryString["mode"].ToString() +
                        "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() +
                        "&SearchCriteriaID2=" + Request.QueryString["SearchCriteriaID2"].ToString();
                    txtMessage.Text = "Please find attached graph image in PDF format.";

                   
                        txtSubject.Text = "Graph";
                        lblAttachmnet.Text = "Graph.pdf";
                   

                }
            }
            
        }


        
    }

   

    protected void lnkSend_Click(object sender, EventArgs e)
    {

        try
        {
            lblMsg.Text = "";

            if (txtTo.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please enter To email address.');", true);
                txtTo.Focus();
                return;
            }
            else
            {
                foreach (string strTo in txtTo.Text.Split(';'))
                {
                    if (strTo.Trim() != "")
                    {

                        if (!Common.IsEmailFormatOK(strTo.Trim()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('" + strTo + " is not a valid email address.');", true);
                            txtTo.Focus();
                            return;
                        }

                    }
                }
            }

            if (txtCC.Text.Trim() != "")
            {
                foreach (string strCC in txtCC.Text.Split(';'))
                {
                    if (strCC.Trim() != "")
                    {

                        if (!Common.IsEmailFormatOK(strCC.Trim()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('" + strCC + " is not a valid email address.');", true);
                            txtCC.Focus();
                            return;
                        }

                    }
                }

            }

            if (txtBCC.Text.Trim() != "")
            {
                foreach (string strBCC in txtBCC.Text.Split(';'))
                {
                    if (strBCC.Trim() != "")
                    {

                        if (!Common.IsEmailFormatOK(strBCC.Trim()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('" + strBCC + " is not a valid email address.');", true);
                            txtBCC.Focus();
                            return;
                        }

                    }
                }
            }


            //now lets send the email

            string strFolderPath = Server.MapPath("~\\ExportedFiles");
          


            string strFileName = hfFileName.Value;
            string strFullFileName = strFolderPath + "\\" + strFileName;


            //string strEmail = SystemData.SystemOption_ValueByKey("EmailFrom");
            //string strEmailServer = SystemData.SystemOption_ValueByKey("EmailServer");
            //string strEmailUserName = SystemData.SystemOption_ValueByKey("EmailUserName");
            //string strEmailPassword = SystemData.SystemOption_ValueByKey("EmailPassword");
            MailMessage msg = new MailMessage();
            //msg.From = new MailAddress(strEmail);
            //msg.Subject = txtSubject.Text;
            //msg.IsBodyHtml = true;


            System.Net.Mail.Attachment attNew = new System.Net.Mail.Attachment(strFullFileName, System.Net.Mime.MediaTypeNames.Application.Octet);
            attNew.Name = lblAttachmnet.Text;
            msg.Attachments.Add(attNew);
          

            //string strTheBody = txtMessage.Text;
            //msg.Body = strTheBody;

            //if (txtTo.Text != "")
            //{
            //    foreach (string strTo in txtTo.Text.Split(';'))
            //    {
            //        if (strTo.Trim() != "")
            //        {
            //            msg.To.Add(strTo.Trim());
            //        }
            //    }
            //}

            //if (txtCC.Text != "")
            //{
            //    foreach (string strCC in txtCC.Text.Split(';'))
            //    {
            //        if (strCC.Trim() != "")
            //        {
            //            msg.CC.Add(strCC.Trim());
            //        }
            //    }
            //}

            //if (txtBCC.Text != "")
            //{
            //    foreach (string strBCC in txtBCC.Text.Split(';'))
            //    {
            //        if (strBCC.Trim() != "")
            //        {
            //            msg.Bcc.Add(strBCC.Trim());
            //        }
            //    }
            //}
            

            //SmtpClient smtpClient = new SmtpClient(strEmailServer);
            //smtpClient.Timeout = 99999;
            //smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
            //smtpClient.Port = DBGurus.StringToInt(DBGurus.GetSystemOption("SmtpPort"));
            //smtpClient.EnableSsl = Convert.ToBoolean(DBGurus.GetSystemOption("EnableSSL"));
           
#if (!DEBUG)
                //smtpClient.Send(msg);
#endif

                //if (msg.To.Count > 0)
                //{
            //Guid guidNew = Guid.NewGuid();
            //string strEmailUID = guidNew.ToString();

            //EmailLog theEmailLog = new EmailLog(null, int.Parse(Session["AccountID"].ToString()), txtSubject.Text,
            //  txtTo.Text, DateTime.Now, int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())),
            //  null,
            //  "Email Record", txtMessage.Text);
            //theEmailLog.EmailUID = strEmailUID;
                //    EmailAndIncoming.dbg_EmailLog_Insert(theEmailLog, null, null);


                //}

            string sError = "";

            Message theMessage = new Message(null, null, int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())), int.Parse(Session["AccountID"].ToString()),
                DateTime.Now, "E", "E",
                    null, txtTo.Text, txtSubject.Text, txtMessage.Text, null, "");


            DBGurus.SendEmail("Email Record", true, null, txtSubject.Text, txtMessage.Text, "", txtTo.Text, txtCC.Text, txtBCC.Text, msg.Attachments, theMessage, out sError);


                //if (System.Web.HttpContext.Current.Session["AccountID"] != null)
                //{

                //    SecurityManager.Account_SMS_Email_Count(int.Parse(System.Web.HttpContext.Current.Session["AccountID"].ToString()), true, null, null, null);
                //}


            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Email has been sent successfully.');", true);

            txtTo.Text = "";
            txtCC.Text = "";
            txtBCC.Text = "";
            Session["tdbmsg"] = "Email has been sent successfully.";
            Response.Redirect(hlBack.NavigateUrl , false);
        }
        catch(Exception ex)
        {
            lblMsg.Text = ex.Message + ex.StackTrace;
        }

    }

    protected void btnUpdateEmail_Click(object sender, EventArgs e)
    {
        string strEmail = hfSelectedEmails.Value.Replace("__", ";");
        if (hfType.Value == "to")
        {
            if (txtTo.Text != "")
            {
                if (txtTo.Text.Substring(txtTo.Text.Length - 1, 1) != ";")
                {
                    txtTo.Text = txtTo.Text + ";" + strEmail;
                }
                else
                {
                    txtTo.Text = txtTo.Text + strEmail;
                }

            }
            else
            {
                txtTo.Text = strEmail;
            }
        }
        if (hfType.Value == "cc")
        {
            if (txtCC.Text != "")
            {
                if (txtCC.Text.Substring(txtCC.Text.Length - 1, 1) != ";")
                {
                    txtCC.Text = txtCC.Text + ";" + strEmail;
                }
                else
                {
                    txtCC.Text = txtCC.Text + strEmail;
                }
            }
            else
            {
                txtCC.Text = strEmail;
            }
        }
        if (hfType.Value == "bcc")
        {
            if (txtBCC.Text != "")
            {
                if (txtBCC.Text.Substring(txtBCC.Text.Length - 1, 1) != ";")
                {
                    txtBCC.Text = txtBCC.Text + ";" + strEmail;
                }
                else
                {
                    txtBCC.Text = txtBCC.Text + strEmail;
                }
            }
            else
            {
                txtBCC.Text = strEmail;
            }
        }
    }

   

}




//protected void CreateCSVFile()
//{
//    try
//    {
//        lblMsg.Text = "";
//        //string strFolderPath = SystemData.SystemOption_ValueByKey("BulkExportPath");
//        string strFolderPath = Server.MapPath("~\\ExportedFiles");
//        string strFileName = Guid.NewGuid().ToString() + ".csv";
//        hfFileName.Value = strFileName;
//        string strFullFileName = strFolderPath + "\\" + strFileName;


//        SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteriaID"].ToString())));



//        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

//        xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));





//        int iTN = 0;


//        string strOrderDirection = "DESC";
//        string sOrder = GetDataKeyNames()[0];


//        strOrderDirection = xmlDoc.FirstChild["GridViewSortDirection"].InnerText;

//        sOrder = xmlDoc.FirstChild["GridViewSortColumn"].InnerText;


//        if (sOrder.Trim() == "")
//        {
//            sOrder = "DBGSystemRecordID";
//        }


//        string strSSIDs = xmlDoc.FirstChild["SSIDs"].InnerText;
//        if (strSSIDs != "")
//        {
//            if (strSSIDs.Length > 0)
//            {
//                strSSIDs = strSSIDs.Substring(0, strSSIDs.Length - 1);
//            }

//        }
//        int _iTotalDynamicColumns = 0;
//        DataTable dt = RecordManager.ets_Record_List(_theTable.TableID,
//               xmlDoc.FirstChild["ddlEnteredBy"].InnerText == "-1" ? null : (int?)int.Parse(xmlDoc.FirstChild["ddlEnteredBy"].InnerText),
//                !bool.Parse(xmlDoc.FirstChild["chkIsActive"].InnerText),
//                bool.Parse(xmlDoc.FirstChild["chkShowOnlyWarning"].InnerText) == false ? null : (bool?)true,
//                strSSIDs,
//                xmlDoc.FirstChild["txtDateFrom"].InnerText.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(xmlDoc.FirstChild["txtDateFrom"].InnerText.Trim() + " 00:00", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
//                xmlDoc.FirstChild["txtDateTo"].InnerText.Trim() == "" ? null : (DateTime?)DateTime.ParseExact(xmlDoc.FirstChild["txtDateTo"].InnerText.Trim() + " 23:59", "d/M/yyyy HH:m", CultureInfo.InvariantCulture),
//                  sOrder, strOrderDirection, null, null, ref iTN, ref _iTotalDynamicColumns, "export", _strNumericSearch, _strTextSearch);


//        DataRow drFooter = dt.NewRow();

//        for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
//        {
//            for (int j = 0; j < dt.Columns.Count; j++)
//            {
//                if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
//                {
//                    if (_dtRecordColums.Rows[i]["ShowTotal"].ToString().ToLower() == "true")
//                    {

//                        //drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = _dtDataSource.Compute("SUM([" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "])", "[" + _dtRecordColums.Rows[i]["NameOnExport"].ToString() + "]<>''");
//                        drFooter[_dtRecordColums.Rows[i]["NameOnExport"].ToString()] = CalculateTotalForAColumn(dt, dt.Columns[j].ColumnName, bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString().ToLower()));

//                    }
//                }

//            }

//        }

//        dt.Rows.Add(drFooter);

//        //Round export

//        foreach (DataRow dr in dt.Rows)
//        {
//            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
//            {
//                for (int j = 0; j < dt.Columns.Count; j++)
//                {
//                    if (_dtRecordColums.Rows[i]["DisplayTextSummary"].ToString() == dt.Columns[j].ColumnName)
//                    {
//                        if (IsStandard(_dtRecordColums.Rows[i]["SystemName"].ToString()) == false)
//                        {
//                            if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
//                            {
//                                if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
//                                {
//                                    if (dr[j].ToString() != "")
//                                    {
//                                        dr[j] = Math.Round(double.Parse(dr[j].ToString()), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
//                                    }
//                                }

//                            }
//                        }

//                    }

//                    //mm:hh
//                    if (_dtRecordColums.Rows[i]["SystemName"].ToString().ToLower() == "datetimerecorded")
//                    {
//                        if (_dtRecordColums.Rows[i]["NameOnExport"].ToString() == dt.Columns[j].ColumnName)
//                        {
//                            if (dr[j].ToString().Length > 15)
//                            {
//                                dr[j] = dr[j].ToString().Substring(0, 16);
//                            }
//                        }
//                    }

//                }
//            }
//        }

//        // First we will write the headers.

//        int iColCount = dt.Columns.Count;


//        for (int i = 0; i < iColCount - 1; i++)
//        {
//            sw.Write(dt.Columns[i]);
//            if (i < iColCount - 2)
//            {
//                sw.Write(",");
//            }

//        }

//        sw.Write(sw.NewLine);



//        // Now write all the rows.


//        foreach (DataRow dr in dt.Rows)
//        {
//            for (int i = 0; i < iColCount - 1; i++)
//            {
//                if (!Convert.IsDBNull(dr[i]))
//                {
//                    sw.Write("\"" + dr[i].ToString().Replace("\"", "'") + "\"");
//                }
//                if (i < iColCount - 2)
//                {
//                    sw.Write(",");
//                }
//            }
//            sw.Write(sw.NewLine);
//        }
//        sw.Close();

//        FileStream Fs = new FileStream(strFullFileName, FileMode.Create);
//        BinaryWriter BWriter = new BinaryWriter(Fs, Encoding.GetEncoding("UTF-8"));
//        BWriter.Write(sw.ToString());
//        BWriter.Close();
//        Fs.Close();


//        //now lets email this to the user.



//    }
//    catch (Exception ex)
//    {
//        lblMsg.Text = ex.Message + ex.StackTrace;
//    }


//}
//public string[] GetDataKeyNames()
//{
//    string[] strRecordID = new string[1];
//    strRecordID[0] = "DBGSystemRecordID";
//    return strRecordID;
//}