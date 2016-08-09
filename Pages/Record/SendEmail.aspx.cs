using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
public partial class Pages_Record_SendEmail :SecurePage
{
    string _qsTableID = "";

    User _objUser;
    Table _theTable;
    protected void optDeliveryMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        //edtContent.Text = "";
        //txtContent.Text = "";
        //txtSuject.Text = "";
        ShowHideEmailSMS();
        PopulateTo();
        PopulateTemplate();
        SetDefaultEmailTemplate();
    }

    protected void SetDefaultEmailTemplate()
    {

        SystemOption theSystemOption;

        if (optDeliveryMethod.SelectedValue == "S")
        {
            theSystemOption = SystemData.SystemOption_Detail_Key_Account("Send SMS Default Template", int.Parse(Session["AccountID"].ToString()), int.Parse(_qsTableID));

        }
        else
        {
            theSystemOption = SystemData.SystemOption_Detail_Key_Account("Send Email Default Template", int.Parse(Session["AccountID"].ToString()), int.Parse(_qsTableID));
        }
        if (theSystemOption != null)
        {

            Content theContent = SystemData.Content_Details_ByKey(theSystemOption.OptionValue, int.Parse(Session["AccountID"].ToString()));

            if (theContent!=null && ddlTemplate.Items.FindByValue(theContent.ContentID.ToString()) != null)
            {
                ddlTemplate.SelectedValue = theContent.ContentID.ToString();
                ddlTemplate_SelectedIndexChanged(null, null);
            }

        }


        //if (optDeliveryMethod.SelectedValue == "E")
        //{
        //    //string strDefaultTemplate = SystemData.SystemOption_ValueByKey_Account("Send Email Default Template", int.Parse(Session["AccountID"].ToString()), int.Parse(_qsTableID));

        //    SystemOption theSystemOption = SystemData.SystemOption_Detail_Key_Account("Send Email Default Template", int.Parse(Session["AccountID"].ToString()), int.Parse(_qsTableID));
        //    if (theSystemOption != null)
        //    {
        //        if (ddlTemplate.Items.FindByValue(theSystemOption.OptionValue) != null)
        //        {
        //            ddlTemplate.SelectedValue = theSystemOption.OptionValue;
        //            ddlTemplate_SelectedIndexChanged(null, null);
        //        }
                   
        //    }
        //}

        
        

    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlTemplate.SelectedValue=="")
        {
            //edtContent.Text = "";
            //txtContent.Text = "";
            //txtSuject.Text = "";
           

            ShowHideEmailSMS();

            lnkDeleteTemplate.Visible = false;
            lnkSaveTemplate.Visible = false;
        }
        else
        {
            lnkDeleteTemplate.Visible = true;
            lnkSaveTemplate.Visible = true;
            PopulateContent();
        }
    }

    protected void PopulateContent()
    {
        if (ddlTemplate.SelectedValue == "")
            return;

        Content theContent = SystemData.Content_Details(int.Parse(ddlTemplate.SelectedValue));

        if (theContent == null)
            return;
        txtSuject.Text = theContent.Heading;
        ShowHideEmailSMS();

         if (optDeliveryMethod.SelectedValue == "E")
         {
             edtContent.Text = theContent.ContentP;
         }
         else
         {
             txtContent.Text = theContent.ContentP;
         }
    }
    protected void ShowHideEmailSMS()
    {

        if (optDeliveryMethod.SelectedValue == "E")
        {

            if (ddlTemplate.SelectedValue == "" && IsPostBack)
                edtContent.Text = txtContent.Text;


            trSaveReplies.Visible = true;
            //tdAttachToRecord.Visible = true;
            edtContent.Visible = true;
            txtContent.Visible = false;
            txtContent_fedback.Visible = false;
            stgSubject.Visible = true;
            txtSuject.Visible = true;


            lnkAddDataBaseField.Visible = true;
            lnkAddDataBaseFieldText.Visible = false;

            stgSubject.Visible = true;
            txtSuject.Visible = true;
            Title = "Send Email";

            
            lblTitle.Text = "Send Email";
            if(hfEmailCount.Value!="")
                lblTitle.Text = "Send Email (" + hfEmailCount.Value +" recipients)";
        }
        else
        {
            //tdAttachToRecord.Visible = false;
            if (edtContent.Text != "" && IsPostBack)
                txtContent.Text = Common.StripTagsCharArray(edtContent.Text);

            txtSuject.Text = "";
            trSaveReplies.Visible = false;
            chkSaveReplies.Checked = false;
            edtContent.Visible = false;
            txtContent.Visible = true;
            stgSubject.Visible = false;
            txtSuject.Visible = false;
           
            txtContent_fedback.Visible = true;
            lnkAddDataBaseField.Visible = false;
            lnkAddDataBaseFieldText.Visible = true;

            Title = "Send SMS";
            if (hfEmailCount.Value != "")
                lblTitle.Text = "Send SMS (" + hfEmailCount.Value + " recipients)";
        }
    }
    protected void PopulateTemplate()
    {
        ddlTemplate.Items.Clear();
        ddlTemplate.DataSource = Common.DataTableFromText(@"SELECT ContentID,ContentKey FROM [Content] C INNER JOIN ContentType CT 
                    ON C.ContentTypeID=CT.ContentTypeID WHERE 
                    (AccountID = "+Session["AccountID"].ToString()+@" OR (AccountID IS NULL AND ForAllAccount=1 
                    AND ContentKey not in(SELECT ContentKey FROM Content WHERE ContentKey=ContentKey 
                    AND AccountID = " + Session["AccountID"].ToString() + @" ))) AND CT.ContentTypeKey='"+optDeliveryMethod.SelectedValue+@"'");

        ddlTemplate.DataBind();
        ListItem liSelect = new ListItem("--Please Select--", "");
        ddlTemplate.Items.Insert(0, liSelect);
    }
    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        if (ddlTemplate.SelectedValue == "")
            return;

        Content theContent = SystemData.Content_Details(int.Parse(ddlTemplate.SelectedValue));

        if (theContent == null)
            return;

        if(optDeliveryMethod.SelectedValue=="E")
        {
            theContent.ContentP = edtContent.Text;
        }
        else
        {
            theContent.ContentP = txtContent.Text;
        }
        theContent.Heading = txtSuject.Text;

        string strContentTypeID = Common.GetValueFromSQL("SELECT ContentTypeID FROM ContentType WHERE ContentTypeKey='"+optDeliveryMethod.SelectedValue+"'");

        theContent.ContentTypeID = strContentTypeID == "" ? null : (int?)int.Parse(strContentTypeID);

        SystemData.Content_Update(theContent);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "TemplateSaved", "alert('Template Saved.');", true);
    }

    protected void lnkDeleteTemplate_Click(object sender, EventArgs e)
    {
        if(ddlTemplate.SelectedValue!="")
        {
            SystemData.Content_Delete(int.Parse(ddlTemplate.SelectedValue));
            PopulateTemplate();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "TemplateDelete", "alert('Template Deleted.');", true);
        }
    }

    protected void btnContentSaved_Click(object sender, EventArgs e)
    {
        if(hfContentKey.Value!="")
        {
            try
            {
                Content theContent = new Content(null, hfContentKey.Value, txtSuject.Text,
                                optDeliveryMethod.SelectedValue=="E"? edtContent.Text:txtContent.Text, "",
                                  null, null, int.Parse(Session["AccountID"].ToString()), false);

                string strContentTypeID = Common.GetValueFromSQL("SELECT ContentTypeID FROM ContentType WHERE ContentTypeKey='" + optDeliveryMethod.SelectedValue + "'");

                theContent.ContentTypeID = strContentTypeID == "" ? null : (int?)int.Parse(strContentTypeID);


               int iNewContentID= SystemData.Content_Insert(theContent);
                PopulateTemplate();
                if (ddlTemplate.Items.FindByValue(iNewContentID.ToString()) != null)
                    ddlTemplate.SelectedValue = iNewContentID.ToString();

                ddlTemplate_SelectedIndexChanged(null, null);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "TemplateSaved", "alert('Template Added.');", true);
               
            }
            catch
            {
                //fuck
            }
            
        }
    }

    protected void lnkSendEmailOK_Click(object sender, EventArgs e)
    {
        int iEmailCount = 0;
        if (hfRecordIDs.Value != "")
        {
            //string strEmailFrom = SystemData.SystemOption_ValueByKey_Account("EmailFrom", null, int.Parse(_qsTableID));



            string strSMSEMailDomain = SystemData.SystemOption_ValueByKey_Account("WarningSMSEmail",null,int.Parse(_qsTableID));
            Column theEmailColumn = RecordManager.ets_Column_Details(int.Parse(ddlTo.SelectedValue));
            DataTable _dtRecordColums = RecordManager.ets_Table_Columns_All(int.Parse(_qsTableID));
            string[] strRecordIDs = hfRecordIDs.Value.Split(',');
            int iTempTN = 0;
            string strTextSearch = " AND [Record].RecordID IN (" + hfRecordIDs.Value  + "-1)";

            string strReturnSQL="";
            DataTable dtRecords = RecordManager.ets_Record_List(int.Parse(_qsTableID), null, true, null, null, null, "DBGSystemRecordID", "DESC", null, null, ref iTempTN, ref iTempTN, "allcolumns", "",
                          strTextSearch, null, null, "", "", "", null, ref strReturnSQL, ref strReturnSQL);



            //DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + _qsTableID); //AND DetailPageType<>'not'

            DataTable dtPT = Common.DataTableFromText(@"SELECT distinct TC.ParentTableID,TableName FROM TableChild TC INNER JOIN [Table] T
                                            ON TC.ParentTableID=T.TableID
                                             WHERE ChildTableID=" + _qsTableID); 


            DataTable dtColumns = Common.DataTableFromText("SELECT SystemName,DisplayName,ColumnID,ColumnType,NumberType,LinkedParentColumnID,TableTableID,DisplayColumn FROM [Column] WHERE (IsStandard=0 OR SystemName='RecordID') AND   TableID="
                             + _qsTableID + "  ORDER BY DisplayName");

            //foreach (string strRecordID in strRecordIDs)
            foreach (DataRow dr in dtRecords.Rows)
            {
                if (dr["DBGSystemRecordID"].ToString() != "")
                {
                    Record thisRecord = RecordManager.ets_Record_Detail_Full(int.Parse(dr["DBGSystemRecordID"].ToString()));
                    string strToEmail = RecordManager.GetRecordValue(ref thisRecord, theEmailColumn.SystemName);

                    string strEachRecordBody = "";
                    bool? bSMS=null;
                    bool? bEmail=null;
                    if (optDeliveryMethod.SelectedValue == "S")
                    {
                        strEachRecordBody = txtContent.Text;
                        if (strToEmail!="")
                            strToEmail = strToEmail + strSMSEMailDomain;
                        bSMS = true;
                    }
                    else
                    {
                        strEachRecordBody = edtContent.Text;
                        bEmail = true;
                    }
                       

                    
                    
                    if (thisRecord != null && strToEmail!="")
                    {                    

                        foreach (DataRow drC in dtColumns.Rows)
                        {
                            //strEachRecordBody = strEachRecordBody.Replace("[" + drC["DisplayName"].ToString() + "]", 
                            //    RecordManager.GetRecordValue(ref thisRecord, drC["SystemName"].ToString() ));

                            strEachRecordBody = strEachRecordBody.Replace("[" + drC["DisplayName"].ToString() + "]",
                                dr[drC["DisplayName"].ToString()].ToString());
                        }


                        //Work with 1 top level Parent tables.



                       try
                       {
                           if (dtPT.Rows.Count > 0)
                           {
                               foreach (DataRow drPT in dtPT.Rows)
                               {
                                   if (strEachRecordBody.IndexOf("[" + drPT["TableName"].ToString() + ":") == -1)
                                   {
                                       continue;
                                   }
                                   

                                   for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                                   {

                                       if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                                         && dr.Table.Columns.Contains("**" + _dtRecordColums.Rows[i]["DisplayName"].ToString() + "_ID**")
                                                         && _dtRecordColums.Rows[i]["TableTableID"].ToString() == drPT["ParentTableID"].ToString())
                                       {
                                           if (dr["**" + _dtRecordColums.Rows[i]["DisplayName"].ToString() + "_ID**"].ToString() != "")
                                           {
                                               Column theLinkedColumn = RecordManager.ets_Column_Details(int.Parse(_dtRecordColums.Rows[i]["LinkedParentColumnID"].ToString()));
                                               DataTable dtParentRecord = null;
                                               if (theLinkedColumn.SystemName.ToLower() == "recordid")
                                               {
                                                   dtParentRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + dr["**" + _dtRecordColums.Rows[i]["DisplayName"].ToString() + "_ID**"].ToString());
                                               }
                                               else
                                               {
                                                   dtParentRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "='" +
                                                       dr["**" + _dtRecordColums.Rows[i]["DisplayName"].ToString() + "_ID**"].ToString().Replace("'", "''") + "'");
                                               }

                                               if (dtParentRecord.Rows.Count > 0)
                                               {

                                                   DataTable dtColumnsPT = Common.DataTableFromText(@"SELECT distinct SystemName, TableName + ':' + DisplayName AS DP FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE  [Column].IsStandard=0 AND  [Column].TableID=" + drPT["ParentTableID"].ToString());
                                                   foreach (DataRow drC in dtColumnsPT.Rows)
                                                   {
                                                       strEachRecordBody = strEachRecordBody.Replace("[" + drC["DP"].ToString() + "]", dtParentRecord.Rows[0][drC["SystemName"].ToString()].ToString());

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

                        string sO = "";

                        //if (iEmailCount < 50)
                        //{


                        string strSubject = txtSuject.Text;

                        if(chkSaveReplies.Checked)
                        {
                            strSubject = txtSuject.Text + "  #" + thisRecord.RecordID.ToString() + "#";
                        }

                        Message theMessage = new Message(null, thisRecord.RecordID, _theTable.TableID, _theTable.AccountID, DateTime.Now, "E", "E",
                          null, strToEmail, strSubject, strEachRecordBody, null, "");
                        
                            string strEmailType = "Record Email Send";
                            if (bSMS!=null)
                            {
                                strEmailType = "Record SMS Send";
                                theMessage.DeliveryMethod = "S";
                            }


                            int iOk = DBGurus.SendEmail(strEmailType, bEmail, bSMS, theMessage.Subject, strEachRecordBody, 
                                "", strToEmail, "", "", null,theMessage, out sO); ;
                            if(iOk!=-1)
                            {
                                iEmailCount = iEmailCount + 1;
                            }
                
                            if(_theTable!=null && _theTable.SPSendEmail!="")
                            {
                                try
                                {
                                    RecordManager.Table_SPSendEmail(_theTable.SPSendEmail,
                                          thisRecord.RecordID, (int)_objUser.UserID);
                                }
                                catch
                                {
                                    //
                                }

                            }
                           
                        //}
                       
                    }
                }
            }
        }


        mpeSendEmail.Hide();
        string strURL = hlBack.NavigateUrl;
        //string strExtra = "";
        string strNotifications = "";
        string strSMS_Email = "SMS";
        if (optDeliveryMethod.SelectedValue == "E")
        {
            strSMS_Email = "email";
        }

        if (iEmailCount==0)
        {
            strNotifications = "No " + strSMS_Email + " has been sent.";
        }
        else if (iEmailCount==1)
        {
            strNotifications = "1 " + strSMS_Email + " has been sent successfully.";
        }
        else
        {
            strNotifications = iEmailCount.ToString() + " " + strSMS_Email + "s have been sent successfully.";
        }

        if(optDeliveryMethod.SelectedValue=="E")
        {
           
            strURL = Common.GetUpdatedFullURLRemoveQueryString(strURL, "smsssent");

            Session["tdbmsg"] = strNotifications;

            Response.Redirect(strURL, false);
        }
        else
        {
           
            //strURL =Common.GetUpdatedFullURLRemoveQueryString(strURL, "emailssent");

            Session["tdbmsg"] = strNotifications;

            Response.Redirect(strURL, false);
            
        }
        

    }

  
    protected void Page_Load(object sender, EventArgs e)
    {
       
        
        edtContent.AssetManager = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";
        _qsTableID = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());

        _theTable = RecordManager.ets_Table_Details(int.Parse(_qsTableID));
        _objUser = (User)Session["User"];
        txtContent.Attributes.Add("onkeyup", "UpdateContent();");

        if (!IsPostBack)
        {
            Title = "Send Email";
            
            PopulateTo();
            PopulateDatabaseField();
            ViewState["HasEmail"] = false;
            ViewState["HasMobile"] = false;
                
            DataTable dtSendEmailCol = Common.DataTableFromText(@"SELECT ColumnID FROM [Column] WHERE TableID=" + _qsTableID + @" AND ColumnType='text' 
                                            AND (TextType='email')");
            if (dtSendEmailCol.Rows.Count > 0)
            {
                ViewState["HasEmail"] = true;
            }

            DataTable dtMobileCol = Common.DataTableFromText(@"SELECT ColumnID FROM [Column] WHERE TableID=" + _qsTableID + @" AND ColumnType='text' 
                                            AND (TextType='mobile')");
            if (dtMobileCol.Rows.Count > 0)
            {
                ViewState["HasMobile"] = true;
            }
            else
            {
                optDeliveryMethod.SelectedValue = "E";
                optDeliveryMethod.Enabled = false;
                imgInfoNoSMS.Visible = true;
            }
            string strSC = "";
            if (Request.QueryString["SearchCriteria"] != null)
            {
                strSC = "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }

            hlBack.NavigateUrl = "~/Pages/Record/RecordList.aspx?TableID=" + Request.QueryString["TableID"].ToString() + strSC;

            if (Request.QueryString["fixedurl"]!=null)
            {
                hlBack.NavigateUrl = Cryptography.Decrypt(Request.QueryString["fixedurl"].ToString());
            }
            if (Request.QueryString["tabindex"] != null)
            {
                //hlBack.NavigateUrl = hlBack.NavigateUrl + "&btabindex=" + Request.QueryString["tabindex"].ToString();
                hlBack.NavigateUrl = Common.GetUpdatedFullURLWithQueryString(hlBack.NavigateUrl, "btabindex", Request.QueryString["tabindex"].ToString());
            }

            PopulateTemplate();

            SetDefaultEmailTemplate();



            if (Request.QueryString["recordids"] != null)
            {
                SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt( Request.QueryString["recordids"].ToString())));
                if (theSearchCriteria != null)
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                    xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));
                    hfRecordIDs.Value = xmlDoc.FirstChild["recordids"].InnerText;

                    if (hfRecordIDs.Value != "")
                    {
                        string[] strRecordIDs = hfRecordIDs.Value.Split(',');

                        int iCount = strRecordIDs.Length - 1;
                        hfEmailCount.Value = iCount.ToString();


                        lblMsgSendEmail.Text = "Do you want to send " + iCount.ToString() + " email(s)?";
                        lblMsgSendSMS.Text = "Do you want to send " + iCount.ToString() + " SMS(s)?";

                        Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));
                        if(theAccount!=null)
                        {
                            if (theAccount.EmailCount != null)
                                hfEmailCountMonth.Value = theAccount.EmailCount.ToString();

                            if (theAccount.SMSCount != null)
                                hfSMSCountMonth.Value = theAccount.SMSCount.ToString();

                        }
                        lblEmailCountMonth.Text = "Note: You have sent " + hfEmailCountMonth.Value + " email(s) this month";
                        lblSMSCountMonth.Text = "Note: You have sent " + hfSMSCountMonth.Value + " SMS(s) this month";
                        ShowHideEmailSMS();
                    }

                }
            }


        }
    }


    protected void PopulateDatabaseField()
    {

        int iTableID = int.Parse(_qsTableID);
        ddlDatabaseField.Items.Clear();

        DataTable dtColumns = Common.DataTableFromText(@"SELECT DisplayName FROM [Column] WHERE (IsStandard=0 OR SystemName='RecordID')
        AND TableID=" + iTableID.ToString() + @"  ORDER BY DisplayName");
        foreach (DataRow dr in dtColumns.Rows)
        {
            ListItem aItem = new ListItem(dr["DisplayName"].ToString(), dr["DisplayName"].ToString());
            ddlDatabaseField.Items.Add(aItem);
        }


        //Work with 1 top level Parent tables.
        DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + iTableID.ToString()); //AND DetailPageType<>'not'

        if (dtPT.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPT.Rows)
            {
                DataTable dtPColumns = Common.DataTableFromText(@"SELECT distinct TableName + ':' + DisplayName AS DP FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE (IsStandard=0 OR SystemName='RecordID') AND TableTableID IS NULL AND  [Column].TableID=" + dr["ParentTableID"].ToString());
                foreach (DataRow drP in dtPColumns.Rows)
                {
                    ListItem aItem = new ListItem(drP["DP"].ToString(), drP["DP"].ToString());
                    ddlDatabaseField.Items.Add(aItem);
                }
            }
        }


    }
    protected void PopulateTo()
    {
        ddlTo.Items.Clear();
        DataTable dtTemp;

        if(optDeliveryMethod.SelectedValue=="S")
        {
            dtTemp = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE 
            TableID=" + _qsTableID + @" AND ColumnType='text' AND TextType='mobile'");
        }
        else
        {
            dtTemp = Common.DataTableFromText(@"SELECT ColumnID,DisplayName FROM [Column] WHERE 
            TableID=" + _qsTableID + @" AND ColumnType='text' AND TextType='email'");

        }
        if (dtTemp.Rows.Count > 1)
        {
            trTo.Visible = true;
        }

        foreach (DataRow dr in dtTemp.Rows)
        {
            ListItem liEach = new ListItem(dr[1].ToString(), dr[0].ToString());
            ddlTo.Items.Add(liEach);

        }

    }

    protected void lnkSend_Click(object sender, EventArgs e)
    {
       if(optDeliveryMethod.SelectedValue=="E")
       {
           lblConfirmTitle.Text = "Send Email Confirmation";
           lblMsgSendEmail.Visible = true;
           lblMsgSendSMS.Visible = false;

           lblEmailCountMonth.Visible = true;
           lblSMSCountMonth.Visible = false;
       }
        else
       {
           lblConfirmTitle.Text = "Send SMS Confirmation";
           lblMsgSendEmail.Visible = false;
           lblMsgSendSMS.Visible = true;

           lblEmailCountMonth.Visible = false;
           lblSMSCountMonth.Visible = true;
       }

       string strSendEmailConfirmation = SystemData.SystemOption_ValueByKey_Account("Send Email Confirmation", _theTable.AccountID, _theTable.TableID);
       if (strSendEmailConfirmation != "" && strSendEmailConfirmation.ToLower()=="yes")
       {
           mpeSendEmail.Show();
       }
       else
       {
           lnkSendEmailOK_Click(null, null);
       }


      
    }

}