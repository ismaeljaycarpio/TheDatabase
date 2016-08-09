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
    string _qsMode = "";
    string _qsDocumentID = "";
    User _objUser;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        _objUser = (User)Session["User"];
        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            if (Request.QueryString["SearchCriteria"] != null)
            {

                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Home/Dashboard.aspx?SearchCriteria=" 
                    + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {

                Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Home/Dashboard.aspx", false);//i think no need
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
        //grdUser.DataSource = Common.DataTableFromText("SELECT * FROM [User] WHERE DashBoardDocumentID=" + iDocumentID.ToString());

//        grdUser.DataSource = Common.DataTableFromText(@"SELECT U.* FROM [User] U INNER JOIN Document D
//                        ON U.UserID=D.UserID WHERE D.DocumentID=" + iDocumentID.ToString());

        grdUser.DataSource = Common.DataTableFromText(@"SELECT U.* FROM [User] U INNER JOIN UserRole UR
ON U.UserID=UR.UserID WHERE UR.DashBoardDocumentID=" + iDocumentID.ToString());


        grdUser.DataBind();

    }

    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<Document> listDocument = SystemData.Document_Select(_iDocumentID, "", "", "", null, null, "DocumentID", "ASC", null, null, ref iTemp);

            Document theDocument = DocumentManager.ets_Document_Detail((int)_iDocumentID);

            string strURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Default.aspx?DashboardID=" + Cryptography.Encrypt(_iDocumentID.ToString());
            lblLink.Text = "<a href='" + strURL + "' target='_blank'>" + strURL + "</a>";

            PopulateUserGrid((int)_iDocumentID);

            txtDashboardName.Text = theDocument.DocumentText;
          
            if (_strActionMode == "edit")
            {
                ViewState["theDocument"] = theDocument;
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath 
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
