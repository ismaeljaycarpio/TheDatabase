using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Security;
using System.Net;

public partial class Pages_Document_SSRS : SecurePage
{

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Document/Report.aspx?SearchCriteria="
                + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() 
                + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString();

            Document theDocument= DocumentManager.ets_Document_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["DocumentID"].ToString())));

            if (theDocument.DocumentDescription != "")
            {
                try
                {

                    //ReportViewer1.Width = 1000;
                    //ReportViewer1.Height = 800;
                    ReportViewer1.ProcessingMode = ProcessingMode.Remote;


                    Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

                    //if (theAccount.ReportUser == "")
                    //    theAccount.ReportUser = "rrponline";

                    //if (theAccount.ReportPW == "")
                    //    theAccount.ReportPW = "Secret123";

                    if (theAccount.ReportServer == "")
                        theAccount.ReportServer = "localhost";


                    if (theAccount.ReportServerUrl == "")
                        theAccount.ReportServerUrl = "http://localhost/reportserver";


                    if (theAccount.ReportUser != "" && theAccount.ReportPW != "")
                    {
                        IReportServerCredentials irsc = new CustomReportCredentials(theAccount.ReportUser, theAccount.ReportPW, theAccount.ReportServer);
                        ReportViewer1.ServerReport.ReportServerCredentials = irsc;
                    }
                    else
                    {
                        try
                        {
                            //ReportViewer1.ServerReport.ReportServerCredentials =  System.Security. WindowsIdentity.GetCurrent();
                        }
                        catch
                        {

                        }
                       
                    }

                    
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