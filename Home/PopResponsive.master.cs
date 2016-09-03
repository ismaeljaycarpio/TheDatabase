using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Popup : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SetupNotification();


        if (!Page.IsPostBack)
        {
            string strRefSite = "";
            if (Request.QueryString["Ref"] != null)
            {
                strRefSite = Request.QueryString["Ref"].ToString();
            }
            SystemData.VisitorInsert((User)Session["User"], Request.UserHostAddress, Request.UserAgent, Request.AppRelativeCurrentExecutionFilePath,strRefSite);

            if (Request.RawUrl.IndexOf("DocGen/ChartSection.aspx") > -1)
            {
                Content.Attributes.Add("style", "text-align: left; padding-left:0px; padding-right:5px;min-height: 300px;");
            }
        }

    }

    protected void SetupNotification()
    {
        double iSec = 3;
        string strTopMsgNoOfMS = "3000";
        string strTopMessageDisplayNumberSeconds = SystemData.SystemOption_ValueByKey_Account("Top Message Display Number Seconds", int.Parse(Session["AccountID"].ToString()), null);
        if (strTopMessageDisplayNumberSeconds != "")
        {
            double dTemp = 0;
            if (double.TryParse(strTopMessageDisplayNumberSeconds, out dTemp))
            {
                if (dTemp > 300)
                    dTemp = 300;

                iSec = dTemp;
                dTemp = dTemp * 1000;
                strTopMsgNoOfMS = dTemp.ToString("N0").Replace(",", "");

            }

        }



        ltMasterStyles.Text = @"<style  type='text/css'>
                            .cssanimations.csstransforms #divNotificationMessage {
                    -webkit-transform: translateY(-50px);
                    -webkit-animation: slideDown " + iSec.ToString() + @"s 0.2s 1 ease forwards;
                    -moz-transform:    translateY(-50px);
                    -moz-animation:    slideDown " + iSec.ToString() + @"s 0.2s 1 ease forwards;
                }
 </style>
            ";


        string strHidedivNotificationMessage = @"                                                     
                                                  $(document).ready(function () {

                                                      try
                                                        {
                                                            window.setTimeout(HidedivNotificationMessage," + strTopMsgNoOfMS + @");
                                                        }
                                                      catch(err)
                                                        {
                                                            //
                                                        }
     
                                                    });
                                                ";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "strHidedivNotificationMessage", strHidedivNotificationMessage, true);


    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (Session["tdbmsgpb"] != null)
            {
                lblNotificationMessage.Text = Session["tdbmsgpb"].ToString();
                Session["tdbmsgpb"] = null;

                if (lblNotificationMessage.Text != "")
                {
                    lblNotificationMessage.Text = lblNotificationMessage.Text + "&nbsp; <a id=\"aNotificationMessageClose\" onclick=\"document.getElementById('divNotificationMessage').style.display = 'none';return false;\" href=\"#\" >Close</a>";
                }
            }
            else
            {
                lblNotificationMessage.Text = "";
            }
        }



    }
}
