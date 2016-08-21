using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Security_AccountDetail : SecurePage
{


    string _strActionMode = "view";
    int? _iErrorLogID ;
    string _qsMode = "";
    string _qsErrorLogID = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        // checking action mode

        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
            { Response.Redirect("~/Default.aspx", false); }

            hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ErrorLog.aspx";
        }

        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "view")
            {
                _strActionMode = _qsMode;

                if (Request.QueryString["errorlogid"] != null)
                {
                    _qsErrorLogID = Cryptography.Decrypt(Request.QueryString["errorlogid"]);
                    _iErrorLogID = int.Parse(_qsErrorLogID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }

        Title = "Error Log Detail - " + _strActionMode;
        lblTitle.Text = "Error Log Detail - " + _strActionMode;


        // checking permission
       

        switch (_strActionMode.ToLower())
        {
            case "add":


                break;

            case "view":


                PopulateTheRecord();
               

                break;

            case "edit":
                

            default:
                //?

                break;
        }

        


    }

   



    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<ErrorLog> listErrorLog = SystemData.ErrorLog_Select(_iErrorLogID, "", "", "", null, "", "ErrorLogID", "ASC", null, null, ref iTemp);
            ErrorLog theErrorLog = SystemData.ErrorLog_Details((int)_iErrorLogID);

            lblModule.Text = theErrorLog.Module;
            lblErrorMessage.Text = theErrorLog.ErrorMessage;
            lblErrorTrack.Text = theErrorLog.ErrorTrack;
            lblErrorPath.Text = theErrorLog.Path;
            lblErrorTime.Text = theErrorLog.ErrorTime.ToString();
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Error Log Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            //lblMsg.Text = ex.Message;
        }
     


}
   

    

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }



   
   

}
