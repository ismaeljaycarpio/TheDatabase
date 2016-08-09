using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Record_TableAudit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["UpdatedDate"] != null && Request.QueryString["TableID"] != null)
            {

                DateTime dtUpdatedDate = DateTime.Parse(Request.QueryString["UpdatedDate"].ToString());
                int iTableID = int.Parse(Request.QueryString["TableID"].ToString());
                grdAuditDetail.DataSource = RecordManager.Table_Audit_Detail(iTableID, dtUpdatedDate);
                grdAuditDetail.DataBind();
            }
        }

    }
}