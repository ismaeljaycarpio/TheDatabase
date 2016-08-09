using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Pages_Record_MessageDetail : System.Web.UI.Page
{
    string _strActionMode = "view";
    int? _iMessageID;
    string _qsMode = "";
    string _qsMessageID = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        // checking action mode

        if (!IsPostBack)
        {
            //if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
            //{ Response.Redirect("~/Default.aspx", false); }

            //hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/Message.aspx";
        }

        if (Request.QueryString["mode"] == null)
        {
             Response.Redirect("~/Default.aspx",false);
             return;
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "view")
            {
                _strActionMode = _qsMode;

                if (Request.QueryString["Messageid"] != null)
                {
                    _qsMessageID = Cryptography.Decrypt(Request.QueryString["Messageid"]);
                    _iMessageID = int.Parse(_qsMessageID);
                }

            }
            else
            {
                Response.Redirect("~/Default.aspx", false);
            }


        }

     
        lblTitle.Text = "Message Detail";
        Title = lblTitle.Text;

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
            //List<Message> listMessage = SystemData.Message_Select(_iMessageID, "", "", "", null, "", "MessageID", "ASC", null, null, ref iTemp);
            Message theMessage = EmailManager.Message_Detail((int)_iMessageID);

            lblDateandTime.Text = theMessage.DateTimeP.ToString();
            lblBody.Text = theMessage.Body;

            if (theMessage.ExternalMessageKey != "")
            {
                hlViewEmail.NavigateUrl = GetViewURL() + theMessage.ExternalMessageKey;
            }

            switch (theMessage.DeliveryMethod)
            {
                case "E":
                    lblDeliveryMethod.Text = "Email";
                    break;
                case "S":
                    lblDeliveryMethod.Text = "SMS";
                    hlViewEmail.Visible = false;
                    break;
            }

            if (theMessage.IsIncoming != null && (bool)theMessage.IsIncoming)
            {
                lblDirection.Text = "Incoming";
            }
            else
            {
                lblDirection.Text = "Outgoing";
            }
                        

            switch (theMessage.MessageType)
            {
                case "W":
                    lblMessageType.Text = "Warning";
                    break;
                case "E":
                    lblMessageType.Text = "General Email";
                    break;
            }
            

            lblOtherParty.Text = theMessage.OtherParty;
            lblSubject.Text = theMessage.Subject;
            if (theMessage.TableID!=null)
            {
                Table theTable = RecordManager.ets_Table_Details((int)theMessage.TableID);
                if(theTable!=null)
                {
                    lblTable.Text = theTable.TableName;
                }
            }
          
           

        }
        catch (Exception ex)
        {
           //
            //lblMsg.Text = ex.Message;
        }



    }


    public string GetViewURL()
    {

        return "http://www.gmail.com/#search/rfc822msgid:";

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }


}