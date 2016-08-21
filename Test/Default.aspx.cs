using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using ChartDirector;
//using Microsoft.Office.Interop.Excel;

public partial class Test_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //lblMsg.Text = Request.PhysicalPath;
            //lnkRefresh_Click(null, null);
            lblMsg.Text = Request.Url.Scheme;
        }
    }


    protected void lnkText_Click(object sender, EventArgs e)
    {
        //lblMsg.Text = hfAccountID.Value;
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            Batch newSourceBatch = UploadManager.ets_Batch_Details(int.Parse(txtBatchID.Text));

            string strOutMsg = "";
            int iFinalBatchID = 0;
            UploadManager.UploadCSV(null, null, "", "", null, "", out strOutMsg, out iFinalBatchID, "", "",
                newSourceBatch.AccountID, null, null, newSourceBatch.BatchID);
            Batch theBatch = UploadManager.ets_Batch_Details(iFinalBatchID);

            string strImportMsg = UploadManager.ImportClickFucntions(theBatch);

            lblMsg.Text = "Done";
        }
        catch(Exception ex)
        {
            lblMsg.Text = ex.Message + ex.StackTrace;
        }
        
    }
}