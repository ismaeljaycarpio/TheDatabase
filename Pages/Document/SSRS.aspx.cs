using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Security;
using System.Net;
using System.Collections; 
public partial class Pages_Document_SSRS : SecurePage
{
    //private IEnumerable GetDateParameters()
    //{
    //    // I'm assuming report view control id as reportViewer
    //    foreach (ReportParameterInfo info in ReportViewer1.ServerReport.GetParameters())
    //    {
    //        if (info.DataType == ParameterDataType.DateTime)
    //        {
    //            yield return string.Format("[{0}]", info.Prompt);
    //        }
    //    }
    //}

    //protected override void OnPreRender(EventArgs e)
    //{
    //    base.OnPreRender(e);
    //    DatePickers.Value = string.Join(",", (new List(GetDateParameters()).ToArray()));
    //}

    //private IEnumerable<string> GetDateParameters()
    //{
    //    // I'm assuming report view control id as reportViewer
    //    foreach (ReportParameterInfo info in ReportViewer1.ServerReport.GetParameters())
    //    {
    //        if (info.DataType == ParameterDataType.DateTime)
    //        {
    //            yield return string.Format("[{0}]", info.Prompt);
    //        }
    //    }
    //}

    private IEnumerable<string> GetDateParameters()
    {
        // I'm assuming report view control id as reportViewer
        foreach (ReportParameterInfo info in ReportViewer1.ServerReport.GetParameters())
        {
            if (info.DataType == ParameterDataType.DateTime)
            {
                yield return string.Format("[{0}]", info.Prompt);
            }
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        DatePickers.Value = string.Join(",", (GetDateParameters().ToList().ToArray()));
        //lblNoReport.Visible = true;
        //lblNoReport.Text = DatePickers.Value;
    }

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

        string strJS = @"

 $(document).ready(function () {
            
//            function fixParameters() {
//                if ($.browser.webkit) {
//
//                    // add date picker
//                    $($("":hidden[id*='DatePickers']"").val().split("","")).each(function (i, item) {
//                        var h = $(""table[id*='ParametersGrid'] span"").filter(function (i) {
//                            var v = ""["" + $(this).text() + ""]"";
//                            return (v != null && item != """" && v.indexOf(item) >= 0);
//                        }).parent(""td"").next(""td"").find("":input:not(:checkbox)"").datepicker({
//                            showOn: ""button""
//                           , buttonImage: '/Reserved.ReportViewerWebControl.axd?OpType=Resource&Name=Microsoft.Reporting.WebForms.calendar.gif'
//                           , buttonImageOnly: true
//                           , dateFormat: 'dd/mm/yyyy'
//                           , changeMonth: true
//                           , changeYear: true
//                        });
//                    });
//
//                    // remove time from date  parent(""td"").next(""td"").find("":input:not(:checkbox)"").datepicker
//                    $($("":hidden[id*='DatePickers']"").val().split("","")).each(function (i, item) {
//                        var h = $(""table[id*='ParametersGrid'] span"").filter(function (i) {
//                            var v = ""["" + $(this).text() + ""]"";
//                            return (v != null && item != """" && v.indexOf(item) >= 0);
//                        }).parent(""td"").next(""td"").find(""input"").parent().children(""input"").each(function () {
//                            $(this).val($(this).val().substring(0, 10));
//                        });
//                    });
//                }
//            }
//
//            fixParameters();
//            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(applicationInitHandler);
//
//            function applicationInitHandler() {
//                fixParameters();
//            }


           

        });

";


        ScriptManager.RegisterStartupScript(this, this.GetType(), "SSRS_JS", strJS, true);


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