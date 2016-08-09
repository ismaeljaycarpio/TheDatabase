using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Security_PaymentAudit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["UpdatedDate"] != null && Request.QueryString["PaymentID"] != null)
            {
                DateTime dtUpdatedDate = DateTime.Parse(Request.QueryString["UpdatedDate"].ToString());
                int iPaymentID = int.Parse(Request.QueryString["PaymentID"].ToString());
                grdAuditDetail.DataSource = RecordManager.Payment_Audit_Detail(iPaymentID, dtUpdatedDate);
                grdAuditDetail.DataBind();
            }
        }

    }
}