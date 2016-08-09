using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UpdateConfirm : System.Web.UI.Page
{
    User _objUser=null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Update Confirmed";
        try
        {
            if(Session["User"]!=null)
        {
            _objUser = (User)Session["User"];
        }

        if (!IsPostBack && _objUser != null & Request.QueryString["RecordID"] != null)
        {
            hfRecordID.Value = Cryptography.Decrypt(Request.QueryString["RecordID"].ToString());
            if(Session["SPUpdateConfirmMessage"]!=null)
            {
                divDefaultMessage.Visible = false;
                divSPUpdateConfirmMsg.Visible = true;
                divSPUpdateConfirmMsg.InnerHtml = Session["SPUpdateConfirmMessage"].ToString();
            }

            //Record theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(hfRecordID.Value));
            //Table theTable = RecordManager.ets_Table_Details((int)theRecord.TableID);

            //if(theTable.SPUpdateConfirm!=null && theTable.SPUpdateConfirm!="")
            //{
            //    string strHTML = RecordManager.Table_SPUpdateConfirm(theTable.SPUpdateConfirm, theRecord.RecordID, _objUser.UserID, null);
            //    if(strHTML!="")
            //    {
            //        divDefaultMessage.Visible = false;
            //        divSPUpdateConfirmMsg.Visible = true;
            //        divSPUpdateConfirmMsg.InnerHtml = strHTML;
            //    }
            //}
        }

        }
        catch(Exception ex)
        {
             ErrorLog theErrorLog = new ErrorLog(null, "Update Confirmed", ex.Message+ "-- RecordID="+hfRecordID.Value, ex.StackTrace
                 , DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
        }
        

    }
}