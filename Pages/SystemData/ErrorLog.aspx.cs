using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


public partial class Pages_SystemData_ErrorLog : SecurePage
{
    Common_Pager _gvPager;

   

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Error Logs";
       
        try
        {


            User ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {
                if (!Common.HaveAccess(Session["roletype"].ToString(), "1"))
                { Response.Redirect("~/Default.aspx", false); }

                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = 50; }

                gvTheGrid.GridViewSortColumn = "ErrorLogID";
                gvTheGrid.GridViewSortDirection = SortDirection.Descending;
               
                BindTheGrid(0, gvTheGrid.PageSize);
            }
            else
            {
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "ErrorLog", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
            int iTN = 0;
           

            gvTheGrid.DataSource = SystemData.ErrorLog_Select(null,
                "", txtErrorMessageSearch.Text.Trim().Replace("'", "''"), 
                "", null, null,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN);
            
            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;


            if (iTN == 0)
            {
                if (IsFiltered())
                {
                    divNoFilter.Visible = true;
                }
                else
                {
                    divNoFilter.Visible = false;
                }

            }
            else
            {

                divNoFilter.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "ErrorLog", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }
    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);

       

    }


    protected void lnkErrorURL_Click(object sender, EventArgs e)
    {
        bool bFound = false;

        

        if(txtErrorURL.Text!="")
        {
            int? iAccountID = null;
           
            string strURL = txtErrorURL.Text;
            if(strURL.IndexOf("?")>-1)
            {
                strURL = strURL.Substring(strURL.IndexOf("?") + 1);
            }
            var nameValues = HttpUtility.ParseQueryString(strURL);

            if(nameValues.GetValues("TableID")!=null)
            {
                try
                {
                    string strTableID = nameValues.GetValues("TableID")[0];
                    int iTemp = 0;

                    if(int.TryParse(strTableID,out iTemp))
                    {
                        //
                    }
                    else
                    {
                        strTableID = Cryptography.Decrypt(strTableID);
                    }
                   
                    Table aTable = RecordManager.ets_Table_Details(int.Parse(strTableID));

                    if (aTable != null)
                        iAccountID = aTable.AccountID;
                }
                catch
                {
                    //
                }
                

            }


            if(iAccountID==null & nameValues.GetValues("AccountID")!=null)
            {
                try
                {
                    string strAccountID = nameValues.GetValues("AccountID")[0];
                    int iTemp = 0;

                    if (int.TryParse(strAccountID, out iTemp))
                    {
                        //
                    }
                    else
                    {
                        strAccountID = Cryptography.Decrypt(strAccountID);
                    }

                    Account aAccount = SecurityManager.Account_Details(int.Parse(strAccountID));

                    if (aAccount != null)
                        iAccountID = aAccount.AccountID;
                }
                catch
                {
                    //
                }
            }

            if (nameValues.GetValues("DocumentID") != null)
            {
                try
                {
                    string strDocumentID = nameValues.GetValues("DocumentID")[0];
                    int iTemp = 0;

                    if (int.TryParse(strDocumentID, out iTemp))
                    {
                        //
                    }
                    else
                    {
                        strDocumentID = Cryptography.Decrypt(strDocumentID);
                    }

                    Document aDocument = DocumentManager.ets_Document_Detail(int.Parse(strDocumentID));

                    if (aDocument != null)
                        iAccountID = aDocument.AccountID;
                }
                catch
                {
                    //
                }


            }

            if (nameValues.GetValues("userID") != null)
            {
                try
                {
                    string struserID = nameValues.GetValues("userID")[0];
                    int iTemp = 0;

                    if (int.TryParse(struserID, out iTemp))
                    {
                        //
                    }
                    else
                    {
                        struserID = Cryptography.Decrypt(struserID);
                    }

                    iAccountID = SecurityManager.GetPrimaryAccountID(int.Parse(struserID));
                }
                catch
                {
                    //
                }


            }


            if (iAccountID!=null)
            {
                Account theAccount = SecurityManager.Account_Details((int)iAccountID);
                User etUser = SecurityManager.User_AccountHolder((int)iAccountID);
                if (etUser != null)
                {

                    //log out
                    //Session["User"] = null;
                    //Session.Abandon();

                    //FormsAuthentication.SignOut();
                    //HttpCookie oUseInfor1 = new HttpCookie("UserInformation", "nothing");
                    //oUseInfor1.Expires = DateTime.Now.AddDays(-3d);
                    //Response.Cookies.Add(oUseInfor1);

                    //Login 


                    string strRedirectURL = txtErrorURL.Text;

                    if (strRedirectURL.IndexOf("http:")==-1)
                    {
                        strRedirectURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/" + strRedirectURL;
                    }

                    Session["LoginAccount"] = null;

                    if (theAccount.HideMyAccount != null && (bool)theAccount.HideMyAccount)
                    {
                        Session["LoginAccount"] = "AccountID=" + theAccount.AccountID.ToString();

                    }

                    Session["GridPageSize"] = SystemData.SystemOption_ValueByKey_Account("GridPageSize", iAccountID, null);

                    Session["User"] = etUser;
                    Session["AccountID"] = iAccountID;
                    SecurityManager.User_SessionID_Update((int)etUser.UserID, Session.SessionID.ToString(), (int)iAccountID);

                    string strUserInfor = string.Format("{0};{1};{2};{3};{4}", etUser.Email, etUser.Password, etUser.FirstName, etUser.UserID, iAccountID);

                    FormsAuthentication.SetAuthCookie(strUserInfor, false);
                    HttpCookie oUseInfor2 = new HttpCookie("UserInformation", "");
                    oUseInfor2.Expires = DateTime.Now.AddDays(-3d);
                    Response.Cookies.Add(oUseInfor2);

                    string roletype = "";
                    roletype = SecurityManager.GetUserRoleTypeID((int)etUser.UserID, (int)iAccountID);



                    Session["roletype"] = roletype;

                    UserRole theUserRole = SecurityManager.GetUserRole((int)etUser.UserID, (int)iAccountID);

                    Session["UserRole"] = theUserRole;

                    if ((bool)theUserRole.IsAdvancedSecurity)
                    {
                        if (Session["roletype"].ToString() != Common.UserRoleType.OwnData)
                        {
                            Session["roletype"] = Common.UserRoleType.ReadOnly;
                        }
                    }

                    if (roletype.Length > 0)
                    {
                        //FormsAuthentication.RedirectFromLoginPage(etUser.Email, false);

                        if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())))
                        {
                            Session["DoNotAllow"] = "true";
                        }

                        Response.Redirect(strRedirectURL, false);
                        return;

                    }

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "jsNotValid", "alert('Not Valid URL.Please try another URL or update this URL.')", true);

            }
        }

        lnkSearch_Click(null, null);

    }

    public static string GetErrorMessage(string strMsg)
    {
        if (strMsg.Length > 200)
        {
            strMsg = strMsg.Substring(0, 200) + "...";
        }
        return strMsg;

    }

    public string GetViewURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/SystemData/ErrorLogDetail.aspx?mode=" + Cryptography.Encrypt("view") + "&errorlogid=";

    }


    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Error Logs";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtErrorMessageSearch.Text = "";
        gvTheGrid.GridViewSortColumn = "ErrorLogID";
        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
        
        lnkSearch_Click(null, null);
        //BindTheGrid(0, gvTheGrid.PageSize);

    }


    protected void Pager_DeleteAction(object sender, EventArgs e)
    {
        string sCheck = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
            }
        }
        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
        }
        else
        {
            DeleteItem(sCheck);
            BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
            _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
            if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
            {
                BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
            }
        }

    }



    private void DeleteItem(string keys)
    {
        try
        {
            if (!string.IsNullOrEmpty(keys))
            {

                foreach (string sTemp in keys.Split(','))
                {
                    if (!string.IsNullOrEmpty(sTemp))
                    {

                        SystemData.ErrorLog_Delete(int.Parse(sTemp));

                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "ErrorLog", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }

    protected bool IsFiltered()
    {
        if (txtErrorMessageSearch.Text != "" )
        {
            return true;
        }

        return false;
    }

}
