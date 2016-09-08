using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Home_DashBoardDetail : SecurePage
{

    string _strActionMode = "view";
    int? _iDocumentID;
    Document _theDoc = null;
    string _qsMode = "";
    string _qsDocumentID = "";
    User _objUser;

    protected void grdUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
                e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

                HyperLink hlFirstName = (HyperLink)e.Row.FindControl("hlFirstName");
                //Label lblFirstName = (Label)e.Row.FindControl("lblFirstName");
                Label lblLastName = (Label)e.Row.FindControl("lblLastName");
              
                string strThisUser = DataBinder.Eval(e.Row.DataItem, "UserID").ToString();
                if (_theDoc != null && _theDoc.UserID != null)
                {
                    if (strThisUser == _theDoc.UserID.ToString())
                    {
                        hlFirstName.Font.Bold = true;
                        hlFirstName.ForeColor = System.Drawing.Color.Red;

                        lblLastName.Font.Bold = true;
                        lblLastName.ForeColor = System.Drawing.Color.Red;
                        lblLastName.ToolTip = "Owner of this dashboard";
                    }
                }
                hlFirstName.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath
     + "/Pages/User/Detail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" +
     Cryptography.Encrypt("-1") + "&userid=" + Cryptography.Encrypt(strThisUser) + "&fixedurl=" + Cryptography.Encrypt(Request.RawUrl);

                //string xml = null;
                //xml = @"<root>" +
                //       " <UserID>" + HttpUtility.HtmlEncode(strThisUser) + "</UserID>" +
                //       " <AccountID>" + HttpUtility.HtmlEncode(Session["AccountID"].ToString()) + "</AccountID>" +
                //       " <URL>" + HttpUtility.HtmlEncode("~/Default.aspx") + "</URL>" +
                //       " <UserHostAddress>" + HttpUtility.HtmlEncode(Cryptography.Encrypt(Request.UserHostAddress)) + "</UserHostAddress>" +
                //       " <UserAgent>" + HttpUtility.HtmlEncode(Cryptography.Encrypt(Request.UserAgent)) + "</UserAgent>" +
                //      "</root>";

                //SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
                //int iSC_ID = SystemData.SearchCriteria_Insert(theSearchCriteria);

                //hlFirstName.NavigateUrl = "~/Pages/Home/Private.aspx?SC_ID=" + Cryptography.Encrypt(iSC_ID.ToString());
            }
            catch
            {
                //
            }
            
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
        _objUser = (User)Session["User"];
        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }



            if (Request.QueryString["SearchCriteria"] != null)
            {

                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Home/Dashboard.aspx?SearchCriteria=" 
                    + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {

                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Home/Dashboard.aspx", false);//i think no need
            }


        }
        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;


                if (Request.QueryString["DocumentID"] != null)
                {

                    _qsDocumentID = Cryptography.Decrypt(Request.QueryString["DocumentID"]);

                    _iDocumentID = int.Parse(_qsDocumentID);

                    _theDoc = DocumentManager.ets_Document_Detail((int)_iDocumentID);
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
       


        // checking permission


        switch (_strActionMode.ToLower())
        {
            case "add":
                trLink.Visible = false;
                trUsers.Visible = false;
                trDashboard.Visible = false;
                lblLisOfUsers.Visible = false;
                lblTitle.Text = "Add Dashboard";
                break;

            case "view":

                lblTitle.Text = "View Dashboard";
                
                PopulateTheRecord();         

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":
                lblTitle.Text = "Edit Dashboard";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }


       
        
        Title = lblTitle.Text;
    }



    protected void PopulateUserGrid(int iDocumentID)
    {

        try
        {
            Document theDoc = DocumentManager.ets_Document_Detail(iDocumentID);
            string strUserIDs = Common.GetUserIDsForDashboard(iDocumentID, int.Parse(Session["AccountID"].ToString()));
            DataTable dtUserDash = Common.GetUsersByDashboard(strUserIDs, int.Parse(Session["AccountID"].ToString()));
            if (dtUserDash != null && dtUserDash.Rows.Count>0)
            {
                lblLisOfUsers.Text = "Users (" + dtUserDash.Rows.Count.ToString() + ")";
              
            }
            else
            {
                lblLisOfUsers.Text = "Users";
                if(theDoc!=null && theDoc.UserID!=null)
                {
                    User ownerUser = SecurityManager.User_Details((int)theDoc.UserID);
                    if(ownerUser!=null)
                    {
                        string strRoleID = Common.GetValueFromSQL("SELECT RoleID FROM UserRole WHERE AccountID=" + Session["AccountID"].ToString() + " AND UserID=" + ownerUser.UserID.ToString());

                        if (strRoleID != "")
                        {
                            int? DashID = DocumentManager.dbg_Dashboard_BestFitting("", int.Parse(ownerUser.UserID.ToString()), int.Parse(strRoleID));
                            if (DashID != null)
                            {
                                string strURl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Home/DashBoardDetail.aspx?mode=" + Request.QueryString["mode"].ToString() + "&SearchCriteria="
                     + Request.QueryString["SearchCriteria"].ToString() + "&DocumentID=" + Cryptography.Encrypt(DashID.ToString());
                                lblLisOfUsers.Font.Bold = false;
                                lblLisOfUsers.Text = "<strong>Owner:</strong> " + ownerUser.FirstName + " " + ownerUser.LastName + "</br>"
                           + "To view " + ownerUser.FirstName + " " + ownerUser.LastName + " latest dashboard <a href='" + strURl + "'>Click here</a>"
                           + "</br>" + "</br>" + "<strong>Users(0)</strong>";
                            }
                              
                        }


                       

                    }
                }
            }

            grdUser.DataSource = dtUserDash;
            grdUser.DataBind();
           


        }
        catch
        {
            //
        }

        

    }

    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<Document> listDocument = SystemData.Document_Select(_iDocumentID, "", "", "", null, null, "DocumentID", "ASC", null, null, ref iTemp);

            PopulateDashBoard();
            Document theDocument = DocumentManager.ets_Document_Detail((int)_iDocumentID);

            if (ddlDashboard.Items.FindByValue(theDocument.DocumentID.ToString()) != null)
                ddlDashboard.SelectedValue = theDocument.DocumentID.ToString();
            
            string strURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Default.aspx?DashboardID=" + Cryptography.Encrypt(_iDocumentID.ToString());
            lblLink.Text = "<a href='" + strURL + "' target='_blank'>" + strURL + "</a>";


            if (theDocument.UserID!=null)
            {
                if(_objUser.UserID==theDocument.UserID)
                {
                    trLink.Visible = true;
                }
            }

            PopulateUserGrid((int)_iDocumentID);

            txtDashboardName.Text = theDocument.DocumentText;
          
            if (_strActionMode == "edit")
            {
                ViewState["theDocument"] = theDocument;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath 
                    + "/Pages/Home/DashBoardDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() 
                    + "&DocumentID=" + Cryptography.Encrypt(theDocument.DocumentID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Dashboard Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }
    protected void PopulateDashBoard()
    {
        ddlDashboard.Items.Clear();
        int iTN = 0;
        DataTable dtTemp = DocumentManager.ets_Dashboard_Select(int.Parse(Session["AccountID"].ToString()),
               "", "",
                "", "ASC",
                null, null, ref iTN);
        ddlDashboard.DataSource = dtTemp;
        ddlDashboard.DataBind();
    }

    protected void ddlDashboard_SelectedIndexChanged(object sender, EventArgs e)
    {
       try
       {
           if(ddlDashboard.SelectedItem!=null && ddlDashboard.SelectedValue!="")
           {
               Response.Redirect(Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Home/DashBoardDetail.aspx?mode=" + Request.QueryString["mode"].ToString() + "&SearchCriteria="
                     + Request.QueryString["SearchCriteria"].ToString() + "&DocumentID="+ Cryptography.Encrypt(ddlDashboard.SelectedValue), true);
           }
       }
        catch
       {
            //
       }
    }
    protected void EnableTheRecordControls(bool p_bEnable)
    {        
        txtDashboardName.Enabled = p_bEnable;
    
            

    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }



    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        //Menu newMenu = new Menu(null, txtMenu.Text, int.Parse(ddlAccount.SelectedValue), chkShowOnMenu.Checked, "");
                        //SecurityManager.test_TestTable_Insert(newMenu);


                        DataTable dtTemp = Common.DataTableFromText("SELECT DocumentID FROM Document WHERE AccountID=" + Session["AccountID"].ToString() + " AND ForDashBoard=1 AND DocumentText='" + txtDashboardName.Text + "'");

                        if (dtTemp.Rows.Count > 0)
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Dashboard name " + txtDashboardName.Text + " already exist, please try another name.');", true);
                            return;
                            break;
                        }
                        else
                        {
                            Document theDocument = new Document(null, int.Parse(Session["AccountID"].ToString()), txtDashboardName.Text, null, "DashBoard", "DashBoard",
                                null, null, null, _objUser.UserID, null);
                            int id = DocumentManager.ets_Document_Insert(theDocument);
                            Common.ExecuteText("UPDATE Document SET ForDashBoard=1 WHERE DocumentID=" + id.ToString());
                            break;
                        }


                    case "view":


                        break;

                    case "edit":


                        Document editDocument = (Document)ViewState["theDocument"];


                        DataTable dtTemp2 = Common.DataTableFromText("SELECT DocumentID FROM Document WHERE DocumentID<>" + editDocument.DocumentID.ToString()+ " AND AccountID=" + Session["AccountID"].ToString() + " AND ForDashBoard=1 AND DocumentText='" + txtDashboardName.Text + "'");

                        if (dtTemp2.Rows.Count > 0)
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Dashboard name " + txtDashboardName.Text + " already exist, please try another name.');", true);
                            return;
                            break;
                        }
                        else
                        {
                            editDocument.DocumentText = txtDashboardName.Text;

                            DocumentManager.ets_Document_Update(editDocument);
                        }

                        break;

                    default:
                        //?
                        break;
                }
            }
            else
            {
                //user input is not ok

            }
            Response.Redirect(hlBack.NavigateUrl, false);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Document Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

     

    }
   
}
