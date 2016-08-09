using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Security;
using System.Net;
using System.Data;
using System.Data.SqlClient;

public partial class Pages_RRP_ReportView : SecurePage
{

    protected void PopulateSSRSReport(Document theDocument)
    {
        if (theDocument.DocumentDescription != "")
        {
            try
            {

                ReportViewer1.Width = 1000;
                ReportViewer1.Height = 800;
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;



                Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

                if (theAccount.ReportUser == "")
                    theAccount.ReportUser = "rrponline";

                if (theAccount.ReportPW == "")
                    theAccount.ReportPW = "Secret123";

                if (theAccount.ReportServer == "")
                    theAccount.ReportServer = "119.148.66.174";


                if (theAccount.ReportServerUrl == "")
                    theAccount.ReportServerUrl = "http://119.148.66.174/Reportserver";


                IReportServerCredentials irsc = new CustomReportCredentials(theAccount.ReportUser, theAccount.ReportPW, theAccount.ReportServer);
                ReportViewer1.ServerReport.ReportServerCredentials = irsc;

                ReportViewer1.ServerReport.ReportServerUrl = new Uri(theAccount.ReportServerUrl);

                //ReportViewer1.ServerReport.ReportPath = "/RRPOnline/Hazards/Hazards By Category";

                ReportViewer1.ServerReport.ReportPath = theDocument.DocumentDescription;

                try
                {
                    ReportParameter[] parameters = new ReportParameter[1];
                    parameters[0] = new ReportParameter("AccountID", Session["AccountID"].ToString());
                    ReportViewer1.ServerReport.SetParameters(parameters);
                }
                catch
                {
                    // do nothing
                }

                try
                {
                    ReportParameter[] parameters = new ReportParameter[1];
                    parameters[0] = new ReportParameter("nAccountID", Session["AccountID"].ToString());
                    ReportViewer1.ServerReport.SetParameters(parameters);
                }
                catch
                {
                    // do nothing
                }

                ReportViewer1.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                ErrorLog theErrorLog = new ErrorLog(null, "SSRS Report", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                SystemData.ErrorLog_Insert(theErrorLog);
            }

        }
        else
        {
            lblNoReport.Visible = true;
            ReportViewer1.Visible = false;

        }
    }

    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReport.SelectedValue != "")
        {
            Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(ddlReport.SelectedValue));

            if (theDocument.ReportType.ToLower() == "ssrs")
            {
                ReportViewer1.Visible = true;
                ifReport.Visible = false;
                PopulateSSRSReport(theDocument);
            }
            else if (theDocument.ReportType.ToLower()=="webpage")           
            {
                ReportViewer1.Visible = false;
                ifReport.Visible = true;

                string strRiskTableID = Common.GetValueFromSQL(@"SELECT TOP 1 TableID FROM [Table]
                    WHERE TableName='Hazard' AND AccountID=" +Session["AccountID"].ToString());

                if (strRiskTableID != "")
                {
                    //"http://" + Request.Url.Authority + Request.ApplicationPath 
                    ifReport.Attributes.Add("src", "http://" + Request.Url.Authority + Request.ApplicationPath 
                        + theDocument.DocumentDescription.Replace("[TableID]",Cryptography.Encrypt( strRiskTableID.ToString())));

                }
            }
        }
        else
        {
            ReportViewer1.Visible = false;
            ifReport.Visible = false;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            hlBack.NavigateUrl = "~/Default.aspx";

            Table theTable = RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())));

            if (theTable != null)
            {
                DataTable dtTemp = Common.DataTableFromText("SELECT DocumentID,DocumentText FROM Document WHERE Tableid=" + theTable.TableID.ToString()
                    + "  ORDER BY DocumentText");

                if (dtTemp.Rows.Count > 0)
                {
                    ddlReport.Items.Clear();

                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        ListItem liTemp = new ListItem(dr["DocumentText"].ToString(), dr["DocumentID"].ToString());
                        ddlReport.Items.Add(liTemp);
                    }

                    ddlReport_SelectedIndexChanged(null, null);
                }
               
            }

        }

                

    }


    protected void Page_Load(object sender, EventArgs e)
    {

    }
}


[Serializable]
public class CustomReportCredentials : IReportServerCredentials
{
    private string _UserName;
    private string _PassWord;
    private string _DomainName;

    public CustomReportCredentials(string UserName, string PassWord, string DomainName)
    {
        _UserName = UserName;
        _PassWord = PassWord;
        _DomainName = DomainName;
    }

    public System.Security.Principal.WindowsIdentity ImpersonationUser
    {
        get { return null; }
    }

    public ICredentials NetworkCredentials
    {
        get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
    }

    public bool GetFormsCredentials(out Cookie authCookie, out string user,
     out string password, out string authority)
    {
        authCookie = null;
        user = password = authority = null;
        return false;
    }
}