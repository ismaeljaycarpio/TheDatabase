using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExpiredAccountUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       if(!IsPostBack)
       {
           lblMsg.Text = "Nope!";
           try
           {
               if (Request.QueryString["cd"] != null && Request.QueryString["id"] != null && Request.QueryString["ed"] != null && Request.QueryString["p"]!=null)
               {
                   DateTime cd = DateTime.Parse(Cryptography.Decrypt(Request.QueryString["cd"].ToString()));

                   if (cd.AddDays(7) > DateTime.Now)
                   {
                       Account theAccount = SecurityManager.Account_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["id"].ToString())));
                       if (theAccount != null)
                       {
                           if (theAccount.ExpiryDate.Value.ToShortDateString() == Cryptography.Decrypt(Request.QueryString["ed"].ToString()))
                           {
                               string strPeriod = Cryptography.Decrypt(Request.QueryString["p"].ToString());

                               switch (strPeriod)
                               {
                                   case "1M":
                                       theAccount.ExpiryDate = theAccount.ExpiryDate.Value.AddMonths(1);

                                       break;
                                   case "3M":
                                       theAccount.ExpiryDate = theAccount.ExpiryDate.Value.AddMonths(3);
                                       break;
                                   case "1Y":
                                       theAccount.ExpiryDate = theAccount.ExpiryDate.Value.AddYears(1);
                                       break;
                                   default:
                                       break;
                               }
                               SecurityManager.Account_Update(theAccount);
                               lblMsg.Text = theAccount.AccountName + "'s expiry date has been updated to " + theAccount.ExpiryDate.Value.ToLongDateString()  ;
                           }
                       }
                   }
               }
              

           }
           catch (Exception ex)
           {
               lblMsg.Text = ex.Message + "-->" + ex.StackTrace;
           }
           
       }
    }
}