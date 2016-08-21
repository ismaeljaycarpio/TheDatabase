using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;

public partial class Pages_Record_TableOption : SecurePage
{
    User _ObjUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Title = "Add Table Options";
        _ObjUser = (User)Session["User"];
        if (!IsPostBack)
        {
            PopulateHelp("TableOptionsHelp");
            PopulateTerminology();
            hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableList.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();

        }


//        string strHelpJS = @" $(function () {
//            $('#hlHelpCommon').fancybox({
//                scrolling: 'auto',
//                type: 'iframe',
//                'transitionIn': 'elastic',
//                'transitionOut': 'none',
//                width: 600,
//                height: 350,
//                titleShow: false
//            });
//        });";


//        ScriptManager.RegisterStartupScript(this, this.GetType(), "HelpJS", strHelpJS, true);



    }
    protected void PopulateHelp(string strContentKey)
    {
        Content theContent = SystemData.Content_Details_ByKey(strContentKey, null);

        if (theContent != null)
        {
            lblHelpContent.Text = theContent.ContentP;           
        }
    }

    protected void PopulateTerminology()
    {
        optBrandNew.Text = optBrandNew.Text.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table").ToLower());
        if (Request.QueryString["FirstTime"] != null)
        {
            lblTitle.Text = "Add " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") ;
            //lblSuggest.Visible = true;
            //lblSuggest.Text = "We strongly suggest you create a new " + 
            //    SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + ".";
        }
        else
        {
            lblTitle.Text = "Add " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + " Options";
        }
        Title = lblTitle.Text;

        if (!IsPostBack)
        {
            // check if there are any active table
            DataTable dtActiveTableList = Common.DataTableFromText("SELECT * FROM [Table] WHERE IsActive=1 AND AccountID=" + Session["AccountID"].ToString());
            if (dtActiveTableList.Rows.Count == 0)
            {
                trFirstTable.Visible = true;
                lblFirstTableInfo.Text = "Congratulations! You have signed up. Now you need to create your first" +
                    " " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + "…";

                divCancle.Visible = false;

            }
        }
    }

    protected void lnkContinue_Click(object sender, EventArgs e)
    {
        string strNextPage = "#";
        if (optBrandNew.Checked)
        {
            strNextPage = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableProperty.aspx?mode=" + Cryptography.Encrypt("add") + "&MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
        }
        else if (optCopyFromTemplate.Checked)
        {
            strNextPage = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Template/TableList.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
        }
        else if (optCreateFromSpreadSheet.Checked)
        {
            strNextPage = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableSheet.aspx?MenuID=" + Request.QueryString["MenuID"] + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
        }
        Response.Redirect(strNextPage, false);
     
    }

   

}