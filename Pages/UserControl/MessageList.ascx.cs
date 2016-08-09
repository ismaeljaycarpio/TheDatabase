using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Globalization;
using DBG.Common;
public partial class Pages_UserControl_MessageList : System.Web.UI.UserControl
{
    Common_Pager _gvPager;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "MessageID";
    string _strGridViewSortDirection = "DESC";

    public int? RecordID { get; set; }

    protected void PopulateTerminology()
    {
        //stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));

    }

   

    protected void PopulateTableDDL()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());

        ddlTable.DataBind();
        //if (iTN == 0)
        //{
        System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("All", "");
        ddlTable.Items.Insert(0, liAll);
        //}


    }


    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkSearch_Click(null, null);
    }




    protected void Page_Load(object sender, EventArgs e)
    {
        

        try
        {


            User ObjUser = (User)Session["User"];


            string strViewItemPop = @"
                    $(function () {
                            $('.popupmessage').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 1200,
                                height: 1000,
                                titleShow: false
                            });
                        });

                ";

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strViewItemPop", strViewItemPop, true);



            if (!IsPostBack)
            {
                PopulateTableDDL();
                if (Request.QueryString["TableID"] != null)
                {
                    if (ddlTable.Items.FindByValue(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())) != null)
                    {
                        ddlTable.SelectedValue = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
                        Table theTable = RecordManager.ets_Table_Details(int.Parse(ddlTable.SelectedValue));
                        if(theTable!=null)
                        {
                            if(theTable.ShowSentEmails==null
                                || (theTable.ShowSentEmails != null && (bool)theTable.ShowSentEmails==false))
                            {
                                ddlIsIncoming.SelectedValue = "True";
                                ddlIsIncoming.Enabled = false;
                            }
                            if (theTable.ShowReceivedEmails == null
                                || (theTable.ShowReceivedEmails!=null && (bool)theTable.ShowReceivedEmails==false))
                            {
                                ddlIsIncoming.SelectedValue = "False";
                                ddlIsIncoming.Enabled = false;
                            }
                        }
                    }
                        

                }
                else
                {
                    tdTableFilter.Visible = true;
                }

                if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
                { Response.Redirect("~/Default.aspx", false); }

                if (Request.QueryString["SearchCriteriaM"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteriaM"].ToString())));
                }


                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }

                if (Request.QueryString["SearchCriteriaM"] != null)
                {
                    gvTheGrid.PageSize = _iMaxRows;
                    gvTheGrid.GridViewSortColumn = _strGridViewSortColumn;
                    if (_strGridViewSortDirection.ToUpper() == "ASC")
                    {
                        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    }
                    else
                    {
                        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                    }
                    BindTheGrid(_iStartIndex, _iMaxRows);
                }
                else
                {
                    gvTheGrid.GridViewSortColumn = "MessageID";
                    gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                }
            }
            else
            {
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");

            if (!IsPostBack)
            {
                PopulateTerminology();
            }


            string strJSDynamicShowHide =  @"
                        $('#" + txtLowerDate.ClientID + @"').on('keyup',function () {
                                                                var strLowerValue = $('#" + txtLowerDate.ClientID + @"').val();
                                                                 if (strLowerValue.trim() != '') {
                                                                    $('#" + trUpperMessageDate.ClientID + @"').fadeIn();
                                                                }
                                                                else {
                                                                     $('#" + txtUpperDate.ClientID + @"').val('');
                                                                    $('#" + trUpperMessageDate.ClientID + @"').fadeOut(); 
                                                                }
                                                            });
                         $('#" + txtLowerDate.ClientID + @"').trigger('keyup');

                    ";

            strJSDynamicShowHide = strJSDynamicShowHide + @"
                        $('#" + txtLowerDate.ClientID + @"').change(function () {
                                                                var strLowerValue = $('#" + txtLowerDate.ClientID + @"').val();
                                                                 if (strLowerValue.trim() != '') {
                                                                    $('#" + trUpperMessageDate.ClientID + @"').fadeIn();
                                                                }
                                                                else {
                                                                     $('#" + txtUpperDate.ClientID + @"').val('');
                                                                    $('#" + trUpperMessageDate.ClientID + @"').fadeOut(); 
                                                                }
                                                            });
                         $('#" + txtLowerDate.ClientID + @"').trigger('change');

                    ";

            if (strJSDynamicShowHide != "")
            {
                strJSDynamicShowHide = @"$(document).ready(function () { 
                        try {  
                                " + strJSDynamicShowHide + @" 
                            }
                        catch(err) {
                                //do ntohing
                                }
                            });";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "strJSDynamicShowHideMessageDate" , strJSDynamicShowHide, true);
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Message", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }




    protected void PopulateSearchCriteria(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                ddlTable.Text = xmlDoc.FirstChild[ddlTable.ID].InnerText;
                txtLowerDate.Text = xmlDoc.FirstChild[txtLowerDate.ID].InnerText;
                txtLowerTime.Text = xmlDoc.FirstChild[txtLowerTime.ID].InnerText;
                txtUpperDate.Text = xmlDoc.FirstChild[txtUpperDate.ID].InnerText;
                txtUpperTime.Text = xmlDoc.FirstChild[txtUpperTime.ID].InnerText;

                ddlMessageType.Text = xmlDoc.FirstChild[ddlMessageType.ID].InnerText;
                ddlIsIncoming.Text = xmlDoc.FirstChild[ddlIsIncoming.ID].InnerText;

                txtOtherparty.Text = xmlDoc.FirstChild[txtOtherparty.ID].InnerText;
                txtSubject.Text = xmlDoc.FirstChild[txtSubject.ID].InnerText;

                _iStartIndex = int.Parse(xmlDoc.FirstChild["iStartIndex"].InnerText);
                _iMaxRows = int.Parse(xmlDoc.FirstChild["iMaxRows"].InnerText);
                _strGridViewSortColumn = xmlDoc.FirstChild["GridViewSortColumn"].InnerText;
                _strGridViewSortDirection = xmlDoc.FirstChild["GridViewSortDirection"].InnerText;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }


    //protected void Pager_OnExportForExcel(object sender, EventArgs e)
    //{

    //}
    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {




        lblMsg.Text = "";

        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + ddlTable.ID + ">" + HttpUtility.HtmlEncode(ddlTable.Text) + "</" + ddlTable.ID + ">" +
                    " <" + txtLowerDate.ID + ">" + HttpUtility.HtmlEncode(txtLowerDate.Text) + "</" + txtLowerDate.ID + ">" +
                     " <" + txtLowerTime.ID + ">" + HttpUtility.HtmlEncode(txtLowerTime.Text) + "</" + txtLowerTime.ID + ">" +
                      " <" + txtUpperDate.ID + ">" + HttpUtility.HtmlEncode(txtUpperDate.Text) + "</" + txtUpperDate.ID + ">" +
                       " <" + txtUpperTime.ID + ">" + HttpUtility.HtmlEncode(txtUpperTime.Text) + "</" + txtUpperTime.ID + ">" +
                        " <" + ddlMessageType.ID + ">" + HttpUtility.HtmlEncode(ddlMessageType.Text) + "</" + ddlMessageType.ID + ">" +
                         " <" + ddlIsIncoming.ID + ">" + HttpUtility.HtmlEncode(ddlIsIncoming.Text) + "</" + ddlIsIncoming.ID + ">" +
                          " <" + txtOtherparty.ID + ">" + HttpUtility.HtmlEncode(txtOtherparty.Text) + "</" + txtOtherparty.ID + ">" +
                           " <" + txtSubject.ID + ">" + HttpUtility.HtmlEncode(txtSubject.Text) + "</" + txtSubject.ID + ">" +
                   " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
                   " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
                   " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                   " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }

        //End Searchcriteria





        try
        {
            int iTN = 0;

            //gvTheGrid.Columns[4].HeaderText = gvTheGrid.Columns[4].HeaderText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));
            if (RecordID!=null)
                hfRecordID.Value = RecordID.ToString();

            DateTime? dDateTimeFrom=null;
           DateTime? dDateTimeTo=null;
            if(txtLowerDate.Text.Trim()!="")
            {
                txtLowerDate.Text = txtLowerDate.Text.Replace(" ", "");
                 string strLowerDate = Common.ReturnDateStringFromToken(txtLowerDate.Text.Trim());
                 string strLowerTime = txtLowerTime.Text.Trim() == "" ? " 00:00" : " " + txtLowerTime.Text.Trim();
                DateTime dateValue;
                if (DateTime.TryParseExact(strLowerDate.Trim() + strLowerTime, Common.DateTimeformats,
                                 new CultureInfo("en-GB"),
                                 DateTimeStyles.None,
                                 out dateValue))
                {
                    dDateTimeFrom=dateValue;
                }
            }
             if(txtUpperDate.Text.Trim()!="")
             {
                 txtUpperDate.Text = txtUpperDate.Text.Replace(" ", "");
                  string strUpperDate = Common.ReturnDateStringFromToken(txtUpperDate.Text.Trim());
                  string strUpperTime = txtUpperTime.Text.Trim() == "" ? " 23:59" : " " + txtUpperTime.Text.Trim();
                   DateTime dateValue;
                   if (DateTime.TryParseExact(strUpperDate.Trim() + strUpperTime, Common.DateTimeformats,
                                 new CultureInfo("en-GB"),
                                 DateTimeStyles.None,
                                 out dateValue))
                  {
                       dDateTimeTo=dateValue;
                  }
             }


            Message objMessage=new Message(null,hfRecordID.Value==""?null:(int?)int.Parse(hfRecordID.Value),
                ddlTable.SelectedValue==""?null:(int?)int.Parse(ddlTable.SelectedValue),int.Parse(Session["AccountID"].ToString()),null,
                ddlMessageType.SelectedValue==""?"":ddlMessageType.SelectedValue,"",
                ddlIsIncoming.SelectedValue==""?null:(bool?)bool.Parse(ddlIsIncoming.SelectedValue),
                txtOtherparty.Text==""?"":txtOtherparty.Text,
                txtSubject.Text==""?"":txtSubject.Text,"","",""
                );
            DataTable dtTemp = EmailManager.Message_Select(objMessage,dDateTimeFrom,dDateTimeTo, 
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN);
            gvTheGrid.DataSource = dtTemp;
            //iTN = dtTemp.Rows.Count;
                      

            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                //_gvPager.AddURL = GetAddURL();
                _gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
            }
                       
            if(iTN>0 && tdTableFilter.Visible)
            {
                gvTheGrid.Columns[2].Visible = true;
            }

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Message", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected void lnkReset_Click(object sender, EventArgs e)
    {
        Pager_OnApplyFilter(null, null);        
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);

    }

    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");


            
            //lblTable.Text = Common.GetValueFromSQL("SELECT TableName FROM [Table] WHERE TableID=" + (DataBinder.Eval(e.Row.DataItem, "TableID").ToString()));

            if (DataBinder.Eval(e.Row.DataItem, "TableName") != DBNull.Value)
            {
                Label lblTable = (Label)e.Row.FindControl("lblTable");
                if(lblTable!=null)
                {
                    lblTable.Text = DataBinder.Eval(e.Row.DataItem, "TableName").ToString();
                }
            }
            Label lblMessageType = (Label)e.Row.FindControl("lblMessageType");
            Label lblDeliveryMethod = (Label)e.Row.FindControl("lblDeliveryMethod");


            
            HyperLink hlViewMessage = (HyperLink)e.Row.FindControl("hlViewMessage");
            if (hlViewMessage != null)
            {
                hlViewMessage.Visible = true;
                hlViewMessage.NavigateUrl = GetViewURL() + Cryptography.Encrypt(DataBinder.Eval(e.Row.DataItem, "MessageID").ToString());
            }

            

            //if (DataBinder.Eval(e.Row.DataItem, "ExternalMessageKey") != DBNull.Value)
            //    {
                   
            //            HyperLink hlViewMessage = (HyperLink)e.Row.FindControl("hlViewMessage");
            //            if (hlViewMessage != null)
            //            {
            //                hlViewMessage.Visible = true;
            //                hlViewMessage.NavigateUrl = GetViewURL() + DataBinder.Eval(e.Row.DataItem, "ExternalMessageKey");
            //            }
                    
            //   }

            if (DataBinder.Eval(e.Row.DataItem, "MessageType") != DBNull.Value && lblMessageType != null)
            {
                switch (DataBinder.Eval(e.Row.DataItem, "MessageType").ToString())
                {
                    case "W":
                        lblMessageType.Text = "Warning";
                        break;
                    case "E":
                        lblMessageType.Text = "General Email";
                        break;                    
                }
            }
            if (DataBinder.Eval(e.Row.DataItem, "DeliveryMethod") != DBNull.Value && lblDeliveryMethod != null)
            {
                switch (DataBinder.Eval(e.Row.DataItem, "DeliveryMethod").ToString())
                {
                    case "E":
                        lblDeliveryMethod.Text = "Email";
                        break;
                    case "S":
                        lblDeliveryMethod.Text = "SMS";
                        break;
                }
            }


            Label lblIsIncoming = (Label)e.Row.FindControl("lblIsIncoming");

            if (DataBinder.Eval(e.Row.DataItem, "IsIncoming") != DBNull.Value && lblIsIncoming != null)
            {
                lblIsIncoming.Text = "Incoming";
            }
            else
            {
                lblIsIncoming.Text = "Outgoing";
            }

        }
    }

    protected void gvTheGrid_PreRender(object sender, EventArgs e)
    {
        GridView grid = (GridView)sender;
        if (grid != null)
        {
            GridViewRow pagerRow = (GridViewRow)grid.TopPagerRow;
            if (pagerRow != null)
            {
                pagerRow.Visible = true;
            }
        }
    }

    //public string GetEditURL()
    //{

    //    return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Import/MessageItem.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaIT=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&MessageID=";

    //}



    public string GetViewURL()
    {

       // return "http://www.gmail.com/#search/rfc822msgid:";
        return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/MessageDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&MessageID="; 
    }
    //public string GetAddURL()
    //{

    //    return "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Import/MessageItem.aspx?mode=" + Cryptography.Encrypt("add") + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SearchCriteriaIT=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString());

    //}




    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Messages";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
       
        //ddlTable.Text = "";
        gvTheGrid.GridViewSortColumn = "MessageID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
        txtLowerDate.Text = "";
        txtLowerTime.Text = "";
        txtUpperDate.Text = "";
        txtUpperTime.Text = "";
        txtOtherparty.Text = "";
        txtSubject.Text = "";
        ddlMessageType.SelectedValue = "";
        if (ddlIsIncoming.Enabled)
            ddlIsIncoming.SelectedValue = "";

        if (tdTableFilter.Visible)
            ddlTable.SelectedValue = "";

        lnkSearch_Click(null, null);
       
    }




    protected void Pager_OnExportForExcel(object sender, EventArgs e)
    {

        gvTheGrid.AllowPaging = false;
        BindTheGrid(0, _gvPager.TotalRows);



        //Response.Clear();
        //Response.Buffer = true;
        //Response.AddHeader("content-disposition",
        //"attachment;filename=Messages.csv");
        //Response.Charset = "";
        //Response.ContentType = "text/csv";

        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);

        DataTable dtMessage = new DataTable();
        dtMessage.Columns.Add("Table");
        dtMessage.Columns.Add("Date and Time");
        dtMessage.Columns.Add("Message Type");
        dtMessage.Columns.Add("Delivery Method");
        dtMessage.Columns.Add("Direction");
        dtMessage.Columns.Add("Other Party");
        dtMessage.Columns.Add("Subject");
        dtMessage.Columns.Add("Body");

        dtMessage.AcceptChanges();
        int iColCount = gvTheGrid.Columns.Count;
        //for (int i = 0; i < iColCount; i++)
        //{
        //    if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
        //    {
        //    }
        //    else
        //    {
        //        sw.Write(gvTheGrid.Columns[i].HeaderText);
                            
        //        if (i < iColCount - 1 )
        //        {
        //            sw.Write(",");
        //        }
        //    }
        //}

        //sw.Write(sw.NewLine);

        // Now write all the rows.
        foreach (GridViewRow dr in gvTheGrid.Rows)
        {
            DataRow drNew = dtMessage.NewRow();
            for (int i = 0; i < iColCount; i++)
            {
                if (string.IsNullOrEmpty(gvTheGrid.Columns[i].HeaderText))
                {
                }
                else
                {
                    switch (i)
                    {
                        case 2:
                            Label lblTable = (Label)dr.FindControl("lblTable");
                            //sw.Write("\"" + lblTable.Text + "\"");
                            drNew["Table"] = lblTable.Text;
                            break;
                        case 3:
                            Label lblDateTime = (Label)dr.FindControl("lblDateTime");
                            //sw.Write("\"" + lblDateTime.Text + "\"");
                            drNew["Date and Time"] = lblDateTime.Text;
                            break;
                        case 4:
                            Label lblMessageType = (Label)dr.FindControl("lblMessageType");
                            //sw.Write("\"" + lblMessageType.Text + "\"");
                            drNew["Message Type"] = lblMessageType.Text;
                            break;
                        case 5:
                            Label lblDeliveryMethod = (Label)dr.FindControl("lblDeliveryMethod");
                           // sw.Write("\"" + lblDeliveryMethod.Text + "\"");
                            drNew["Delivery Method"] = lblDeliveryMethod.Text;
                            break;
                        case 6:
                            Label lblIsIncoming = (Label)dr.FindControl("lblIsIncoming");
                            //sw.Write("\"" + lblIsIncoming.Text + "\"");
                            drNew["Direction"] = lblIsIncoming.Text;
                            break;
                        case 7:
                            Label lblOtherParty = (Label)dr.FindControl("lblOtherParty");
                            //sw.Write("\"" + lblOtherParty.Text + "\"");
                            drNew["Other Party"] = lblOtherParty.Text;
                            break;
                        case 8:
                            Label lblSubject = (Label)dr.FindControl("lblSubject");
                           // sw.Write("\"" + lblSubject.Text + "\"");
                            drNew["Subject"] = lblSubject.Text;
                            break;
                        case 9:
                            Label lblBody = (Label)dr.FindControl("lblBody");
                            //sw.Write("\"" + lblBody.Text + "\"");
                            drNew["Body"] = lblBody.Text;
                            break;

                    }

                    //if (i < iColCount - 1)
                    //{
                    //    sw.Write(",");
                    //}
                }
            }
            dtMessage.Rows.Add(drNew);
            //sw.Write(sw.NewLine);
        }
        //sw.Close();
        dtMessage.AcceptChanges();

        ExportUtil.ExportToExcel(dtMessage, "Message.xls");

        //HttpContext.Current.Response.Output.Write(sw.ToString());
        //HttpContext.Current.Response.Flush();
        //HttpContext.Current.Response.End();

        //HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        return;
    }


    protected bool IsFiltered()
    {
        if (txtLowerDate.Text!="" || txtUpperDate.Text!="" || ddlIsIncoming.SelectedValue!="" || ddlMessageType.SelectedValue!=""
            || txtOtherparty.Text!="" || txtSubject.Text!="")
        {
            return true;
        }

        return false;
    }

}
