using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;

public partial class Page_Security_DemoEmail : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "ETS - DemoEmail";


        if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            User objUser = (User)Session["User"];

            if (objUser.Email.ToLower() == SystemData.SystemOption_ValueByKey_Account("DemoAccountCreator",null,null))
            {
                //go 
            }
            else if (Request.QueryString["wizard"] != null)
            {
                //go
            }
            else
            {
                Response.Redirect("~/Default.aspx", false);
            }
        
        }

        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            }
        }

        if (Request.QueryString["AccountID"] == null)
        {
            Response.Redirect("~/Default.aspx", false);
            return;
        }

        Content theContent = SystemData.Content_Details_ByKey("DemoEmail",null);

       

        if (theContent != null)
        {
            DataTable theSPTable = SystemData.Run_ContentSP("ets_DemoEmail", Cryptography.Decrypt(Request.QueryString["AccountID"].ToString()), Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath);
            theContent.ContentP = Common.ReplaceDataFiledByValue(theSPTable, theContent.ContentP);

            lblHeading.Text = theContent.Heading;
            lblContent.Text = theContent.ContentP;
            //lblContent.Text = theContent.ContentP.Replace("[URL]", strURL).Replace("[TableList]", strTable);
        }


//        Account theAccount = SecurityManager.Account_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["AccountID"].ToString())));

        

//        string strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx";
//        DataTable dtUser = Common.DataTableFromText(@"SELECT     Email, Password, IsAccountHolder, UserID
//            FROM         [User] WHERE  IsAccountHolder=1 AND  AccountID=" + theAccount.AccountID.ToString(), null, null);

//        if (dtUser.Rows.Count > 0)
//        {
//            strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Login.aspx?Email="+dtUser.Rows[0]["Email"].ToString()
//                + "&Password=" + dtUser.Rows[0]["Password"].ToString() + "&RememberMe=Yes";

//        }


//        DataTable dtTable = Common.DataTableFromText(@"SELECT     TableID, TableName FROM Table WHERE IsActive=1 AND AccountID="+ theAccount.AccountID.ToString() +@"
//                                            ORDER BY MenuID, TableName", null, null);


//        string strTable = "";

//        if (dtTable.Rows.Count > 0)
//        {
//            strTable = "<ul>";
//        }

//        foreach (DataRow dr in dtTable.Rows)
//        {
//            strTable = strTable + "<li>" + dr["TableName"].ToString() + "</li>";
//        }
//        strTable = strTable + "</ul>";

       

       


    }

    protected void lnkBack_Click(object sender, EventArgs e)
    {
        if (ViewState["RefUrl"] != null)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }
        else
        {
            Response.Redirect("~/Default.aspx", false);
        }
    }

}
