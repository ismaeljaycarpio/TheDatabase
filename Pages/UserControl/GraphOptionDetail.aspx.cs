using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_UserControl_GraphOptionDetail : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          
            PopulateTable();

            //PopulateSS();
            PopulateRecord();
            PopulateTerminology();
        }
     
    }

    protected void PopulateTerminology()
    {
        lblTable.Text = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), lblTable.Text, lblTable.Text);
        lblAnalyte.Text = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), lblAnalyte.Text, lblAnalyte.Text);
        //lblLocation.Text = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), lblLocation.Text, lblLocation.Text);
        
    }

    protected void ddlAnalyte_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAnalyte.SelectedValue != "")
        {
            Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlAnalyte.SelectedValue));
            if (theColumn != null )
            {
                txtAxisLabel.Text = theColumn.GraphLabel;
            }
        }

    }

    protected void PopulateRecord()
    {

        if (Request.QueryString["ModeDetail"].ToString() == "add")
        {
            lblDetailTitle.Text = "Add Series";
        }
        else
        {
            lblDetailTitle.Text = "Edit Series";
        }

        if (Request.QueryString["TableID"].ToString() != "-1")
            ddlTable.Text = Request.QueryString["TableID"].ToString();

        PopulateAnalyte();
        //PopulateSS();

        if (Request.QueryString["ColumnID"].ToString() != "-1")
            ddlAnalyte.Text = Request.QueryString["ColumnID"].ToString();

        //if (Request.QueryString["LocationID"]!=null && Request.QueryString["LocationID"].ToString() != "")
        //    ddlLocation.Text = Request.QueryString["LocationID"].ToString();




        if (Request.QueryString["GraphType"] != null && Request.QueryString["GraphType"].ToString() != "")
            ddlGraphType.Text = Request.QueryString["GraphType"].ToString();

        if (Request.QueryString["Axis"] != null && Request.QueryString["Axis"].ToString() != "")
            ddlAxes.Text = Request.QueryString["Axis"].ToString();
        if (Request.QueryString["Colour"] != null && Request.QueryString["Colour"].ToString() != "")
            cpColour.ColorHEX = "#" + Request.QueryString["Colour"].ToString();

        if (Request.QueryString["High"] != null && Request.QueryString["High"].ToString() != "")
            txtHighestValue.Text = Request.QueryString["High"].ToString();
        if (Request.QueryString["Low"] != null && Request.QueryString["Low"].ToString() != "")
            txtLowestValue.Text = Request.QueryString["Low"].ToString();

        if (Request.QueryString["Label"] != null && Request.QueryString["Label"].ToString() != "-1")
        {
            txtAxisLabel.Text = Request.QueryString["Label"].ToString();
        }
        else
        {
            if (Request.QueryString["GraphLabel"] != null)
            {
                txtAxisLabel.Text = Request.QueryString["GraphLabel"].ToString();
            }
            else
            {

                ddlAnalyte_SelectedIndexChanged(null, null);
            }
        }

        if (ddlAxes.Text == "Percentage")
        {
            txtAxisLabel.Visible = false;
        }
        else
        {
            txtAxisLabel.Visible = true;
        }
    }




    //protected void PopulateSS()
    //{
       
    //        ddlLocation.Items.Clear();
    //        int iTN = 0;

    //        if (ddlTable.SelectedValue != "")
    //        {
    //            ddlLocation.DataSource = SiteManager.ets_Location_Select(null, int.Parse(ddlTable.SelectedValue), null, null, "",
    //                            true, null, null, null, null,
    //                           int.Parse(Session["AccountID"].ToString()),
    //                           "LocationName", "ASC", null, null, ref iTN, "");

    //            ddlLocation.DataBind();


    //            System.Web.UI.WebControls.ListItem liAll = new System.Web.UI.WebControls.ListItem("--All--", "");
    //            ddlLocation.Items.Insert(0, liAll);
    //        }
       
    //}
    protected void PopulateTable()
    {
        int iTN = 0;
        ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                null,
                null,
                int.Parse(Session["AccountID"].ToString()),
                null, null, true,
                "st.TableName", "ASC",
                null, null, ref  iTN, Session["STs"].ToString());
        ddlTable.DataBind();

        System.Web.UI.WebControls.ListItem liPlease = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
        ddlTable.Items.Insert(0, liPlease);

    }

    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateAnalyte();
        //PopulateSS();
        ddlAnalyte_SelectedIndexChanged(null, null);
    }
    protected void PopulateAnalyte()
    {
        ddlAnalyte.Items.Clear();
        int iTN = 0;
        string strTableID = ddlTable.SelectedValue;
        if (ddlTable.SelectedValue != "")
        {
            List<Column> lstColumns = RecordManager.ets_Table_Columns(int.Parse(strTableID),
                   null, null, ref iTN);

            Column dtColumn = new Column();
            foreach (Column eachColumn in lstColumns)
            {
                if (eachColumn.IsStandard == false)
                {

                    if (eachColumn.GraphLabel != "" && eachColumn.ColumnType == "number")
                    {
                        System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString());

                        ddlAnalyte.Items.Insert(ddlAnalyte.Items.Count, aItem);
                    }
                }

            }

        }


    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {

        if (ddlTable.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select a " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table") + ".');", true);
            ddlTable.Focus();
            return;
        }
        if (ddlAnalyte.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please select " + SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field") + ".');", true);
            ddlAnalyte.Focus();
            return;
        }


        string xml = null;
        xml = @"<root>" +
               " <GraphOptionDetailID>" + HttpUtility.HtmlEncode(Request.QueryString["GraphOptionDetailID"].ToString()) + "</GraphOptionDetailID>" +
               " <ModeDetail>" + HttpUtility.HtmlEncode(Request.QueryString["ModeDetail"].ToString()) + "</ModeDetail>" +
               " <TableID>" + HttpUtility.HtmlEncode(ddlTable.SelectedValue) + "</TableID>" +
               " <ColumnID>" + HttpUtility.HtmlEncode(ddlAnalyte.SelectedValue) + "</ColumnID>" +
               //" <LocationID>" + HttpUtility.HtmlEncode(ddlLocation.SelectedValue) + "</LocationID>" +
               //" <LocationName>" + HttpUtility.HtmlEncode(ddlLocation.SelectedItem.Text) + "</LocationName>" +
               " <GraphType>" + HttpUtility.HtmlEncode(ddlGraphType.SelectedValue) + "</GraphType>" +
               " <Axis>" + HttpUtility.HtmlEncode(ddlAxes.SelectedValue) + "</Axis>" +
               " <Colour>" + HttpUtility.HtmlEncode(cpColour.ColorHEX) + "</Colour>" +
               " <High>" + HttpUtility.HtmlEncode(txtHighestValue.Text) + "</High>" +
               " <Low>" + HttpUtility.HtmlEncode(txtLowestValue.Text) + "</Low>" +
               " <Label>" + HttpUtility.HtmlEncode(txtAxisLabel.Text) + "</Label>" +
              "</root>";

        SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
        int iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);


        ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshMap", "CloseAndRefresh(" + iSearchCriteriaID.ToString()+ ");", true);
    }
    protected void ddlAxes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAxes.Text == "Percentage")
        {
            txtAxisLabel.Visible = false;
        }
        else
        {
            txtAxisLabel.Visible = true;
        }
    }
}