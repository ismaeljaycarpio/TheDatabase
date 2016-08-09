using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Company_ContactUs : System.Web.UI.Page
{

    //User _ObjUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            Title = "Contact Us";

            if (!IsPostBack)
            {

                //Content theContentCU=SystemData.Content_Details_ByKey("ContactUs");

                //if (theContentCU!=null)
                //lblContentContactUs.Text = theContentCU.ContentP;

                //if (Request.UrlReferrer != null )
                //{
                //    hlBack.NavigateUrl = Request.UrlReferrer.ToString();
                //}
                //else
                //{
                //    hlBack.NavigateUrl = "~/Default.aspx";
                //}


                //_ObjUser = (User)Session["User"];
                //if (_ObjUser != null)
                //{
                //    if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
                //    {
                //        dbgContent.ShowInlineContentEditor = true;
                //    }

                //}

            }
        }
        catch (Exception ex)
        {
            //
        }
    }
    protected void lnkSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            Contact theContact = new Contact(null, txtEmail.Text, null, txtName.Text, "", txtMessage.Text, 2);

            int iNewContactID = SecurityManager.Contact_Insert(theContact);
            //now lets send email

            Content theConent = SystemData.Content_Details_ByKey("ContactUsEmail",null);

            DataTable theSPTable = SystemData.Run_ContentSP("ContactUs_Email_Content", iNewContactID.ToString());
            string strBody = Common.ReplaceDataFiledByValue(theSPTable, theConent.ContentP);

            string strTo = SystemData.SystemOption_ValueByKey_Account("ContactUsEmail",null,null);

            string strError = "";

            theConent.ContentP = strBody;


            DBGurus.SendEmail("ContactUsEmail", null, null, theConent.Heading, theConent.ContentP, "", strTo, "", "", null, null, out strError);
           
            
            if (strError == "")
            {
                txtEmail.Text = "";
                txtMessage.Text = "";
                txtName.Text = "";
                //txtPhoneNumber.Text = "";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "CommonMsg", "alert(' Your infromation has been submitted. Thanks. ');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CommonMsg", "alert(' Error:" + strError + "');", true); ;
            }
        }
        catch (Exception ex)
        {
            //
        }
    }
}