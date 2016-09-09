using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public partial class Record_Record_List : SecurePage
{

    Table _theTable;

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " Page_LoadComplete ";
            theSpeedLog.FunctionLineNumber = 33;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }

        
    }




    protected override void OnSaveStateComplete(EventArgs e)
    {
        if (Session["RunSpeedLog"] != null)
        {
            SpeedLog theSpeedLog = new SpeedLog();
            theSpeedLog.FunctionName = _theTable.TableName + " OnSaveStateComplete ";
            theSpeedLog.FunctionLineNumber = 270;
            SecurityManager.AddSpeedLog(theSpeedLog);
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {


        if (Request.QueryString["TableID"] != null)
        {
            _theTable = RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())));
            //reset stack
            //if (_theTable != null)
            //{
            //    Session["stackURL"] = null;
            //}
        }
        if(!IsPostBack)
        {
            Session["stackURL"] = null;
            Session["stackTabIndex"] = null;
            Session["CopyRecordID"] = null;
            Session["quickdone"] = null;
            Session["controlvalue"] = null;
            //Session["tabindex"] = null;
            Session["edittab"] = null;
        }

        //if (this.MasterPageFile.ToLower().IndexOf("rrp") > -1)
        //{          
        //    if (_theTable.HeaderColor != "")
        //    {
        //        ltTextStyles.Text = "<style>.pagerstyle{ background: #" + _theTable.HeaderColor + ";}.pagergradient{ background: #" + _theTable.HeaderColor + ";}.TopTitle{color:#FFFFFF;}</style>";                
        //    }
        //}


    }

    protected void btnReloadMe_Click(object sender, EventArgs e)
    {

        string strRawURL = Request.RawUrl;
        
        Response.Redirect(strRawURL, false);
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            if(_theTable!=null && _theTable.SummaryPageContent!="")
            {
                lblSummaryPageContent.Text = _theTable.SummaryPageContent;
            }
            //Session["stackURL"] = null;
            //Session["CopyRecordID"] = null;
            //Session["quickdone"] = null;
            //Session["controlvalue"] = null;
            //Session["tabindex"] = null;
            //Session["edittab"] = null;
        }
       
    }
}