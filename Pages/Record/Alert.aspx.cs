using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Record_Alert : SecurePage
{
    User _ObjUser;
    UserRole _theUserRole;
    protected void Page_Load(object sender, EventArgs e)
    {
        _ObjUser = (User)Session["User"];
        _theUserRole = (UserRole)Session["UserRole"];
        if (!IsPostBack)
        {
            gvWarning.GridViewSortDirection = SortDirection.Descending;
            BindWarningGrid(0, gvWarning.PageSize);
        }
    }

    protected void BindWarningGrid(int iStartIndex, int iMaxRows)
    {
        try
        {
           
            int iAccountID = int.Parse(Session["AccountID"].ToString());

            
            DateTime dtLastAlertDate = DateTime.Now.AddDays(-365);
            User infoUser = SecurityManager.User_Details((int)_ObjUser.UserID);



            if (infoUser != null && _theUserRole.AlertSeen != null)
            {
                dtLastAlertDate = (DateTime)_theUserRole.AlertSeen;
            }

            
            DataTable dtTemp = RecordManager.ets_DashBorad_Alert(iAccountID, dtLastAlertDate);

            gvWarning.DataSource = dtTemp;
            gvWarning.VirtualItemCount = dtTemp.Rows.Count;
            gvWarning.DataBind();


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Notification", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            //lblMsg.Text = ex.Message;
        }
    }
    public string GetWarningViewURL() //link with Record page
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?warning=yes&TableID=";

    }
    protected void chkDoNotShowMe_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDoNotShowMe.Checked)
        {
            _theUserRole.AlertSeen = DateTime.Now;
            //SecurityManager.User_Update(_ObjUser);
            SecurityManager.UserRole_Update(_theUserRole);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "parent.$.fancybox.close();", true);
        }
        else
        {
          //do nothing

        }
    }
}