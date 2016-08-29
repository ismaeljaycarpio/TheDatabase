using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
public partial class Pages_Home_Private : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack &&  Request.QueryString["SC_ID"]!=null)
        {
            try
            {
                SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(int.Parse(Cryptography.Decrypt(Request.QueryString["SC_ID"].ToString())));
                if (theSearchCriteria != null)
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                    xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                    string strUserID = xmlDoc.FirstChild["UserID"].InnerText;
                    string strAccountID = xmlDoc.FirstChild["AccountID"].InnerText;
                    string strURL = xmlDoc.FirstChild["URL"].InnerText;
                    string strUserHostAddress = xmlDoc.FirstChild["UserHostAddress"].InnerText;

                    string strUserAgent = xmlDoc.FirstChild["UserAgent"].InnerText;

                    if ( Cryptography.Encrypt(Request.UserHostAddress) == strUserHostAddress
                        && Cryptography.Encrypt(Request.UserAgent) == strUserAgent)
                    {
                        if (Common.ChangeAccount(int.Parse(strUserID), int.Parse(strAccountID), true))
                        {
                              User etUser = SecurityManager.User_Details(int.Parse(strUserID));
                              string strUserInfor = string.Format("{0};{1};{2};{3};{4}", etUser.Email, etUser.Password, etUser.FirstName, etUser.UserID, int.Parse(strAccountID));

                            FormsAuthentication.SetAuthCookie(strUserInfor, false);
                             FormsAuthentication.RedirectFromLoginPage(etUser.Email, false);
                             Session["HideAccountMenu"] = "yes";
                            Response.Redirect(strURL, true);
                            return;
                        }
                    }


                   
                }
            }
            catch
            {
                //
            }
           

        }
       
    }
}