using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Schedule_ScheduleReportDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iScheduleReportID;
    string _qsMode = "";
    string _qsScheduleReportID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;

        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            if (Request.QueryString["SearchCriteria"] != null)
            {

                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/ScheduleReport.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {

                Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/ScheduleReport.aspx", false);//i think no need
            }


            DataTable theDataTable = Common.DataTableFromText("SELECT * FROM DocumentType WHERE DocumentTypeName='Custom Reports' AND AccountID=" + Session["AccountID"].ToString());

            if (theDataTable.Rows.Count > 0)
            {
                hfDocumentTypeID.Value = theDataTable.Rows[0]["DocumentTypeID"].ToString();                
            }

            PopulateDcouments();
            PopulateWhen();


        }
        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;


                if (Request.QueryString["ScheduleReportID"] != null)
                {

                    _qsScheduleReportID = Cryptography.Decrypt(Request.QueryString["ScheduleReportID"]);

                    _iScheduleReportID = int.Parse(_qsScheduleReportID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
       


        // checking permission
        string strTitle = "Schedule Report Detail";

        switch (_strActionMode.ToLower())
        {
            case "add":
                strTitle = "Add Schedule Report";

                break;

            case "view":


                strTitle = "View Schedule Report";
                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":
                strTitle = "Edit Schedule Report";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }


        Title = strTitle;
        lblTitle.Text = strTitle;

    }


    protected void PopulateWhen()
    {
        ddlWhen.Items.Clear();
        switch (ddlFrequency.SelectedValue)
        {
            case "M":
                ddlWhen.Items.Add(new ListItem("1", "1"));
                ddlWhen.Items.Add(new ListItem("2", "2"));
                ddlWhen.Items.Add(new ListItem("3", "3"));
                ddlWhen.Items.Add(new ListItem("4", "4"));
                ddlWhen.Items.Add(new ListItem("5", "5"));
                ddlWhen.Items.Add(new ListItem("6", "6"));
                ddlWhen.Items.Add(new ListItem("7", "7"));
                ddlWhen.Items.Add(new ListItem("8", "8"));
                ddlWhen.Items.Add(new ListItem("9", "9"));
                ddlWhen.Items.Add(new ListItem("10", "10"));
                ddlWhen.Items.Add(new ListItem("11", "11"));
                ddlWhen.Items.Add(new ListItem("12", "12"));
                ddlWhen.Items.Add(new ListItem("13", "13"));
                ddlWhen.Items.Add(new ListItem("14", "14"));
                ddlWhen.Items.Add(new ListItem("15", "15"));
                ddlWhen.Items.Add(new ListItem("16", "16"));
                ddlWhen.Items.Add(new ListItem("17", "17"));
                ddlWhen.Items.Add(new ListItem("18", "18"));
                ddlWhen.Items.Add(new ListItem("19", "19"));
                ddlWhen.Items.Add(new ListItem("20", "20"));
                ddlWhen.Items.Add(new ListItem("21", "21"));
                ddlWhen.Items.Add(new ListItem("22", "22"));
                ddlWhen.Items.Add(new ListItem("23", "23"));
                ddlWhen.Items.Add(new ListItem("24", "24"));
                ddlWhen.Items.Add(new ListItem("25", "25"));
                ddlWhen.Items.Add(new ListItem("26", "26"));
                ddlWhen.Items.Add(new ListItem("27", "27"));
                ddlWhen.Items.Add(new ListItem("28", "28"));
                //ddlWhen.Items.Add(new ListItem("29", "29"));
                //ddlWhen.Items.Add(new ListItem("30", "30"));
                //ddlWhen.Items.Add(new ListItem("31", "31"));
                break;
            case "W":
                ddlWhen.Items.Add(new ListItem("Monday", "Monday"));
                ddlWhen.Items.Add(new ListItem("Tuesday", "Tuesday"));
                ddlWhen.Items.Add(new ListItem("Wednesday", "Wednesday"));
                ddlWhen.Items.Add(new ListItem("Thursday", "Thursday"));
                ddlWhen.Items.Add(new ListItem("Friday", "Friday"));
                ddlWhen.Items.Add(new ListItem("Saturday", "Saturday"));
                ddlWhen.Items.Add(new ListItem("Sunday", "Sunday"));
                break;
            case "D":
                ddlWhen.Items.Add(new ListItem("01:00", "01:00"));
                ddlWhen.Items.Add(new ListItem("02:00", "02:00"));
                ddlWhen.Items.Add(new ListItem("03:00", "03:00"));
                ddlWhen.Items.Add(new ListItem("04:00", "04:00"));
                ddlWhen.Items.Add(new ListItem("05:00", "05:00"));
                ddlWhen.Items.Add(new ListItem("06:00", "06:00"));
                ddlWhen.Items.Add(new ListItem("07:00", "07:00"));
                ddlWhen.Items.Add(new ListItem("08:00", "08:00"));
                ddlWhen.Items.Add(new ListItem("09:00", "09:00"));
                ddlWhen.Items.Add(new ListItem("10:00", "10:00"));
                ddlWhen.Items.Add(new ListItem("11:00", "11:00"));
                ddlWhen.Items.Add(new ListItem("12:00", "12:00"));
                ddlWhen.Items.Add(new ListItem("13:00", "13:00"));
                ddlWhen.Items.Add(new ListItem("14:00", "14:00"));
                ddlWhen.Items.Add(new ListItem("15:00", "15:00"));
                ddlWhen.Items.Add(new ListItem("16:00", "16:00"));
                ddlWhen.Items.Add(new ListItem("17:00", "17:00"));
                ddlWhen.Items.Add(new ListItem("18:00", "18:00"));
                ddlWhen.Items.Add(new ListItem("19:00", "19:00"));
                ddlWhen.Items.Add(new ListItem("20:00", "20:00"));
                ddlWhen.Items.Add(new ListItem("21:00", "21:00"));
                ddlWhen.Items.Add(new ListItem("22:00", "22:00"));
                ddlWhen.Items.Add(new ListItem("23:00", "23:00"));
                break;
           
        }


    }

    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<SystemOption> listSystemOption = SystemData.SystemOption_Select(_iScheduleReportID, "", "", "", null, null, "ScheduleReportID", "ASC", null, null, ref iTemp);

            ScheduleReport theScheduleReport = ScheduleManager.ets_ScheduleReport_Detail((int)_iScheduleReportID);

            ddlReport.Text = theScheduleReport.MainDocumentID.ToString();

            ddlFrequency.Text = theScheduleReport.Frequency;
            PopulateWhen();

            ddlWhen.Text = theScheduleReport.FrequencyWhen;

            if (theScheduleReport.ReportPeriod != null)
                txtReportPeriod.Text = theScheduleReport.ReportPeriod.ToString();

            if (theScheduleReport.ReportPeriodUnit != "")
                ddlTimePeriod.Text = theScheduleReport.ReportPeriodUnit.ToString();


            txtEmails.Text = theScheduleReport.Emails.ToString();

            if (_strActionMode == "edit")
            {
                ViewState["theScheduleReport"] = theScheduleReport;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Schedule/ScheduleReportDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&ScheduleReportID=" + Cryptography.Encrypt(theScheduleReport.ScheduleReportID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Schedule Report Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    

    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        ddlReport.Enabled = p_bEnable;
        ddlFrequency.Enabled = p_bEnable;
        ddlWhen.Enabled = p_bEnable;

        ddlTimePeriod.Enabled = p_bEnable;
        txtEmails.Enabled = p_bEnable;
        txtReportPeriod.Enabled = p_bEnable;
            

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }

    protected void PopulateDcouments()
    {
        try
        {

            ddlReport.DataSource = Common.DataTableFromText("SELECT DocumentID,DocumentText FROM Document WHERE AccountID=" + Session["AccountID"].ToString() + " AND DocumentTypeID=" + hfDocumentTypeID.Value + " ORDER BY DocumentText");
            ddlReport.DataBind();
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("-Please Select-", "");
            ddlReport.Items.Insert(0, liSelect);

        }
        catch
        {
            //
        }

    }


    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        //Menu newMenu = new Menu(null, txtMenu.Text, int.Parse(ddlAccount.SelectedValue), chkShowOnMenu.Checked, "");
                        //SecurityManager.test_TestTable_Insert(newMenu);

                        ScheduleReport newScheduleReport = new ScheduleReport(null,int.Parse(ddlReport.SelectedValue),
                            ddlFrequency.SelectedValue, ddlWhen.SelectedValue, 
                            DateTime.Now, DateTime.Now);

                        newScheduleReport.ReportPeriod = int.Parse(txtReportPeriod.Text);
                        newScheduleReport.ReportPeriodUnit = ddlTimePeriod.Text;
                        newScheduleReport.Emails = txtEmails.Text;

                        ScheduleManager.ets_ScheduleReport_Insert(newScheduleReport);
                       

                        break;

                    case "view":


                        break;

                    case "edit":
                        ScheduleReport editScheduleReport = (ScheduleReport)ViewState["theScheduleReport"];

                        editScheduleReport.MainDocumentID = int.Parse(ddlReport.SelectedValue);
                        editScheduleReport.Frequency = ddlFrequency.SelectedValue;
                        editScheduleReport.FrequencyWhen = ddlWhen.SelectedValue;

                        editScheduleReport.ReportPeriod = int.Parse(txtReportPeriod.Text);
                        editScheduleReport.ReportPeriodUnit = ddlTimePeriod.Text;
                        editScheduleReport.Emails = txtEmails.Text;

                        ScheduleManager.ets_ScheduleReport_Update(editScheduleReport);


                        break;

                    default:
                        //?
                        break;
                }
            }
            else
            {
                //user input is not ok

            }
            Response.Redirect(hlBack.NavigateUrl, false);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Schedule Report Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

     

    }

    protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateWhen();
    }
}
