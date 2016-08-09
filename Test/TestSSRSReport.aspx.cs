using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Security;
using System.Net;
//using System.Net.net
public partial class Test_TestSSRSReport : System.Web.UI.Page
{



    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           
            ReportViewer1.Width = 800;
            ReportViewer1.Height = 600;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;

            IReportServerCredentials irsc = new CustomReportCredentials("rrponline", "Secret123", "119.148.66.174");
            ReportViewer1.ServerReport.ReportServerCredentials = irsc;

            ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://119.148.66.174/Reportserver");

            ReportViewer1.ServerReport.ReportPath = "/RRPOnline/CompanyIndividual";
            
            //ReportViewer1.ServerReport.ReportPath = "http://119.148.66.174/Reports/Pages/CompanyIndividual";
            
            ReportViewer1.ServerReport.Refresh();

        }


        //string strTest = "#532624#";

        //lblTest.Text = strTest;

        //strTest = "#532624# some more text here";

        //lblTest.Text = strTest;

        //strTest = "Some text here #532624#";

        //lblTest.Text = strTest;

        //strTest = "Some text here #532624# test";


        //lblTest.Text = strTest;

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