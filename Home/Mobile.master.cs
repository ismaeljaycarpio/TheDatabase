using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;

public partial class Home_Mobile : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            PopulateMenuDDL();

            if(Request.RawUrl.IndexOf("Mobile/RecordList.aspx")>-1)
            {
                if (Request.QueryString["TableID"] != null)
                {
                    ddlTableMenu.Text = Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
                }
            }

            if (!Context.User.Identity.IsAuthenticated)
            {
                //Response.Redirect("~/Pages/Mobile/Login.aspx",false);
                Response.Redirect("~/Login.aspx", false);
            }

            if (Request.RawUrl.IndexOf("Mobile/RecordList.aspx") > -1
                || Request.RawUrl.IndexOf("Mobile/Default.aspx") > -1)
            {

            }
            else
            {
                trGotoMenu.Visible = false;
            }


        }

        //CheckSelectedMenu();

        if (!Page.IsPostBack)
        {

            string strRefSite = "";
            if (Request.QueryString["Ref"] != null)
            {
                strRefSite = Request.QueryString["Ref"].ToString();
            }

            SystemData.VisitorInsert((User)Session["User"], Request.UserHostAddress, Request.UserAgent, Request.AppRelativeCurrentExecutionFilePath, strRefSite);


            User ObjUser = (User)Session["User"];

            //if (ObjUser != null && ObjUser.UserID > 0)
            //{
            //    lnlUserName.Text = ObjUser.FirstName + " " + ObjUser.LastName;
               
            //}


            //CheckConcurrent();
            CheckDoNotAllow();
        }





    }

    protected void lkLogout_Click(object sender, EventArgs e)
    {
       


        Session["User"] = null;
        Session.Abandon();

        FormsAuthentication.SignOut();

        HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
        oUseInfor.Expires = DateTime.Now.AddDays(-3d);
        Response.Cookies.Add(oUseInfor);

        //Response.Redirect("~/Pages/Mobile/Login.aspx?Logout=yes", false);

        Response.Redirect("~/Login.aspx?Logout=yes", false);

    }


    //protected void CheckConcurrent()
    //{
    //    if (Session["User"] != null)
    //    {
    //        User ObjUser = (User)Session["User"];

    //        string strSessionID = SecurityManager.User_SessionID_Get((int)ObjUser.UserID);

    //        if (strSessionID != "")
    //        {
    //            if (strSessionID != Session.SessionID.ToString())
    //            {
    //                //ops 
    //                SecurityManager.User_LoggedOutCount_Increment((int)ObjUser.UserID);
    //                //lkLogout_Click(null, null);
    //            }


    //        }
    //    }


    //}

    protected void CheckDoNotAllow()
    {
        if (Request.RawUrl.IndexOf("/Security/MakePayment.aspx") > -1 ||
            Request.RawUrl.IndexOf("Security/AccountDetail.aspx") > -1)
        {
            //its ok
        }
        else
        {
            if (Session["DoNotAllow"] != null)
            {
                if (Session["DoNotAllow"].ToString() == "true")
                {
                    //Response.Redirect("~/Pages/Security/MakePayment.aspx?PaymentInfo="
                    //                       + Cryptography.Encrypt("Payment_TrialExpired"), false);

                }

            }

        }

    }

    protected void PopulateMenuDDL()
    {
        ddlTableMenu.Items.Clear();
        int iTN = 0;
        List<Table> lstSTs= RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());

        ListItem liDash=new ListItem("Dashboard","Dash");
        ddlTableMenu.Items.Add(liDash);
       
        foreach (Table stEach in lstSTs)
        {
            ListItem liOne = new ListItem(stEach.TableName, stEach.TableID.ToString());
            ddlTableMenu.Items.Add(liOne);
        }



    }

    protected void ddlTableMenu_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTableMenu.SelectedValue == "Dash")
        {
            Response.Redirect("~/Pages/Mobile/Default.aspx", false);
        }
        else
        {
            Response.Redirect("~/Pages/Mobile/RecordList.aspx?TableID=" + Cryptography.Encrypt(ddlTableMenu.SelectedValue), false);
        }
    }


    protected void BindMenu()
    {

        //menuETS.Items.Clear();
        string strAppPath = "http://" + Request.Url.Authority + Request.ApplicationPath;

        int iTN = 0;
        int iTempCount = 0;
        User ObjUser = (User)Session["User"];

        ObjUser = SecurityManager.User_Details((int)ObjUser.UserID);
        Session["User"] = ObjUser;

        UserRole theUserRole = SecurityManager.GetUserRole((int)ObjUser.UserID, int.Parse(Session["AccountID"].ToString()));
        Session["UserRole"] = theUserRole;


        if ((bool)theUserRole.IsAdvancedSecurity)
        {
            //Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)ObjUser.UserID, "-1,3,4,5,7,8");
            Session["STs"] = RecordManager.ets_Table_ByUser_AdvancedSecurity((int)theUserRole.RoleID);
        }
        else
        {
            Session["STs"] = "";
          
            string roletype = Session["roletype"].ToString();

            Session["roletype"] = roletype;
            if (roletype == "6")
            {
                Session["STs"] = "-1";
            }
        }



        if (ObjUser != null && ObjUser.UserID > 0)
        {

            //

            Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

            if (theAccount.Logo != null)
            {
                if ((bool)theAccount.UseDefaultLogo == false)
                {
                    imgLogo.ImageUrl = "~/SSPhoto.ashx?AccountID=" + Session["AccountID"].ToString() + "&type=m";
                }
            }

            hlAccount.NavigateUrl = "~/Pages/Mobile/AccountDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&accountid=" + Cryptography.Encrypt(Session["AccountID"].ToString());

        }
        else
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            //Response.Redirect("~/Pages/Mobile/Login.aspx", false);
            Response.Redirect("~/Login.aspx", false);

        }

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        User ObjUser = (User)Session["User"];
        if (ObjUser != null && ObjUser.UserID > 0)
        {

            if (Context.User.Identity.IsAuthenticated)
            {
                //lkProfile.Text = ObjUser.FirstName + " " + ObjUser.LastName;
            }
            BindMenu();
           
        }
        else
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            //Response.Redirect("~/Pages/Mobile/Login.aspx", false);
            Response.Redirect("~/Login.aspx", false);
           
        }
    }




}
