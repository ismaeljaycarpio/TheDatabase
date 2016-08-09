using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Record_ColumnAudit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["UpdatedDate"] != null && Request.QueryString["ColumnID"] != null)
            {
                DateTime dtUpdatedDate = DateTime.Parse(Request.QueryString["UpdatedDate"].ToString());
                int iColumnID = int.Parse(Request.QueryString["ColumnID"].ToString());
                grdAuditDetail.DataSource = RecordManager.Column_Audit_Detail(iColumnID, dtUpdatedDate);
                grdAuditDetail.DataBind();
            }
        }

    }
}