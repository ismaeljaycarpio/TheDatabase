using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LateDataSnooze : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["AccountID"] != null && Request.QueryString["TableID"] != null
                && Request.QueryString["Period"] != null && Request.QueryString["emailuid"] != null
                &&  Request.QueryString["datetime"] != null)
            {

                Message theMessage=EmailManager.Message_Detail_BY_ExternalMessageKey(Cryptography.Decrypt(Request.QueryString["emailuid"].ToString()));

                if (theMessage != null)
                {
                    if (theMessage.DateTimeP.Value.AddDays(1) > DateTime.Now)
                    {

                        ScheduledTask theScheduledTask = ScheduleManager.dbg_ScheduledTask_Detail_By_MessageID((int)theMessage.MessageID);


                        ScheduledTask newScheduledTask = new ScheduledTask(null, int.Parse(Cryptography.Decrypt(Request.QueryString["AccountID"].ToString())),
                            int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())),
                            Request.QueryString["Period"].ToString().ToString().ToLower(), "",
                            "Late Data", theMessage.MessageID);

                        if (theScheduledTask == null)
                        {
                            newScheduledTask.LastEmailSentDate = theMessage.DateTimeP;
                            newScheduledTask.RecordDateAdded = DateTime.Parse(Cryptography.Decrypt(Request.QueryString["datetime"].ToString()));
                            ScheduleManager.dbg_ScheduledTask_Insert(newScheduledTask);
                        }
                        else
                        {
                            theScheduledTask.Frequency = newScheduledTask.Frequency;
                            ScheduleManager.dbg_ScheduledTask_Update(theScheduledTask);
                        }
                    }
                    //newScheduledTask.RecordDateAdded = DateTime.Parse();

                }

            }
        }


        Response.Redirect("~/Default.aspx", true);
    }
}