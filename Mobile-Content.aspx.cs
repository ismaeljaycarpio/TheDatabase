using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text.RegularExpressions;

public partial class Mobile_Content : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Request.QueryString["key"] != null)
        {
            try
            {
                this.Title = Request.QueryString["key"] + " | DB Gurus Australia";
                MobileContent.Text = Common.MobileContentByKey(Request.QueryString["key"].ToString());
                if (Request.QueryString["key"] == "Database-Solutions")
                {
                    flexslider.Visible = true;
                }
                if (Request.QueryString["key"] == "Contact-Us")
                {
                    divContactUs.Visible = true;
                }
                else
                {
                    divContactUs.Visible = false;
                }


                if (Request.QueryString["key"] == "ThankYouContact")
                {
                    divGoogleconversion.Visible = true;
                }
                else
                {
                    divGoogleconversion.Visible = false;
                }
            }
            catch
            {
                //do nothing 
            }
        }
    }


    protected void btnContactUsSend_Click(object sender, EventArgs e)
    {
             

        lblContactUsMsg.Text = "";
        
        

        if (txtName.Text == "" || txtEmail.Text == "" || txtPhoneNumber.Text == "" || txtMessage.Text == "")
        {
            lblContactUsMsg.Text = "All above fields are required!";
            return;
        }
        if (IsEmailFormatOK(txtEmail.Text.Trim()) == false)
        {
            lblContactUsMsg.Text = "Invalid Email!";
            return;

        }


        string strEmail = "DoNotReply@thedatabase.net";
        string strEmailServer = "119.148.66.174";
        string strEmailUserName = "DoNotReply@thedatabase.net";
        string strEmailPassword = "Mohsin2515";
        string strEnableSSL = "False";
        string strSmtpPort = "25";
        string strTo = "info@dbgurus.com.au";


        string strBody = "<strong>DB Gurus Request For Further Info</strong>"
            + "<p><table border=1 cellpadding=10>"
            + "<tr><td>Name:</td><td>" + txtName.Text + "</td></tr>"
            + "<tr><td>Email:</td><td>" + txtEmail.Text + "</td></tr>"
            + "<tr><td>Phone:</td><td>" + txtPhoneNumber.Text + "</td></tr>"
            + "<tr><td>Message:</td><td>" + txtMessage.Text + "</td></tr>"
            + "</table></p>";


        //strBody = strBody.Replace("[Table]", theTable.TableName);



        MailMessage msg = new MailMessage();
        msg.From = new MailAddress(strEmail);


        msg.Subject = "DBG website new contact us";

        msg.IsBodyHtml = true;

        msg.Body = strBody;


        SmtpClient smtpClient = new SmtpClient(strEmailServer);
        smtpClient.Timeout = 99999;
        smtpClient.Credentials = new System.Net.NetworkCredential(strEmailUserName, strEmailPassword);
        smtpClient.EnableSsl = bool.Parse(strEnableSSL);
        smtpClient.Port = int.Parse(strSmtpPort);

        msg.To.Clear();
        msg.To.Add(strTo);

        try
        {



#if (!DEBUG)
            smtpClient.Send(msg);
#endif


        }
        catch (Exception)
        {

            //
        }

        txtEmail.Text = "";
        txtName.Text = "";
        txtPhoneNumber.Text = "";
        txtMessage.Text = "";

        Response.Redirect("Mobile-Content.aspx?key=ThankYouContact", false);



    }

    public static bool IsEmailFormatOK(string inputEmail)
    {
        if (inputEmail == null || inputEmail.Length == 0)
        {
            return false;
        }

        const string expression = "^([a-zA-Z0-9_\\-\\.])+@(([0-2]?[0-5]?[0-5]\\.[0-2]?[0-5]?[0-5]\\.[0-2]?[0-5]?[0-5]\\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\\-])+\\.)+([a-zA-Z\\-])+))$";

        Regex regex = new Regex(expression);
        return regex.IsMatch(inputEmail);
    }

}