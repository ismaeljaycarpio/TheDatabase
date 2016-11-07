using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class Pages_Custom_JCU_Participants :SecurePage
{
    int iSiteTableID = 3140;
    int iParticipantTableID = 3376;
    int iVisitTableID = 3377;
    int iQueryTableID = 3392;

    string _strCommonURL = "";
    string _strparticipatURL = "";
    string _strVisitEditURL = "";
    string _strVisitAddURL="";
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = lblTitle.Text;
        _strVisitEditURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID=[TableID]&fixedurl=" + Cryptography.Encrypt("~/Pages/Custom/JCU_Participants.aspx") + "&Recordid=";
        _strparticipatURL = _strVisitEditURL.Replace("[TableID]", Cryptography.Encrypt(iParticipantTableID.ToString()));
        _strVisitEditURL = _strVisitEditURL.Replace("[TableID]", Cryptography.Encrypt(iVisitTableID.ToString()));

        _strVisitAddURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteriaID=" + Cryptography.Encrypt("-1") + "&TableID="+Cryptography.Encrypt(iVisitTableID.ToString())+"&fixedurl=" + Cryptography.Encrypt("~/Pages/Custom/JCU_Participants.aspx") + "&parentRecordid=";
        
        
        if(!IsPostBack)
        {
            PopulateSite();
            if(Session["jcu_participant_scID"]!=null)
            {
                PopulateSearchCriteria(int.Parse(Session["jcu_participant_scID"].ToString()));
                lnkSearch_Click(null, null);
            }
        }

    }
    protected void PopulateSite()
    {
        DataTable dtSite = Common.DataTableFromText("SELECT RecordID,V001 FROM [Record] WHERE TableID=" + iSiteTableID.ToString() + " AND IsActive=1 ORDER BY V001");

        ListItem aSelect = new ListItem("--Please Select--", "");
        ddlSite.Items.Add(aSelect);
        foreach(DataRow dr in dtSite.Rows)
        {
            ListItem aItem = new ListItem( dr["V001"].ToString(),dr["RecordID"].ToString());
            ddlSite.Items.Add(aItem);
        }
      
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
      
        string strParticipantTextSearch = "";
        if(ddlSite.SelectedValue!="")
        {
            strParticipantTextSearch = strParticipantTextSearch + " AND V021='" + ddlSite.SelectedValue + "'";
        }
     
        if(txtParticipantID.Text!="")
        {
            strParticipantTextSearch = strParticipantTextSearch + " AND V002 LIKE '%" + txtParticipantID.Text.Replace("'","''")  + "%'";
        }
        if (txtInitials.Text != "")
        {
            strParticipantTextSearch = strParticipantTextSearch + " AND V001 '" + txtInitials.Text.Replace("'", "''") + "'";
        }
        if (ddlSite.SelectedValue == "" && txtParticipantID.Text == "" && txtInitials.Text == "")
        {
            iParticipantTableID = -1;
        }
   //     DataTable dtParticipants = RecordManager.ets_Record_List(iParticipantTableID,
   // null,true,null, null, null,
   // "", "", null, null, ref iTN, ref _iTotalDynamicColumns, "allcolumns", "", strTextSearch,
   //null, null, "", "", "", null, ref strReturnSQL, ref sReturnHeaderSQL);


        grdVisit.DataSource = Common.DataTableFromText(@"SELECT RecordID,V001,V002,V021 FROM [Record] WHERE TableID=" + iParticipantTableID.ToString() + " AND IsActive=1 " + strParticipantTextSearch);
        grdVisit.DataBind();

        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + ddlSite.ID + ">" + HttpUtility.HtmlEncode(ddlSite.Text) + "</" + ddlSite.ID + ">" +
                   " <" + txtParticipantID.ID + ">" + HttpUtility.HtmlEncode(txtParticipantID.Text) + "</" + txtParticipantID.ID + ">" +

                    " <" + txtInitials.ID + ">" + HttpUtility.HtmlEncode(txtInitials.Text) + "</" + txtInitials.ID + ">" +
                      "</root>";
            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            int? iSCID = SystemData.SearchCriteria_Insert(theSearchCriteria);
            if (iSCID != null)
                Session["jcu_participant_scID"] = iSCID;
        }
        catch
        {

        }

         

        
    }
    protected void PopulateSearchCriteria(int iSearchCriteriaID)
    {
        try
        {
              SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


              if (theSearchCriteria != null)
              {
                  System.Xml.XmlDocument xmlSC_Doc = new System.Xml.XmlDocument();

                  xmlSC_Doc.Load(new StringReader(theSearchCriteria.SearchText));

                  if (xmlSC_Doc.FirstChild[ddlSite.ID]!=null && ddlSite.Items.FindByValue(xmlSC_Doc.FirstChild[ddlSite.ID].InnerText) != null)
                  {
                      ddlSite.Text = xmlSC_Doc.FirstChild[ddlSite.ID].InnerText;
                  }


                  txtParticipantID.Text = xmlSC_Doc.FirstChild[txtParticipantID.ID].InnerText;
                  txtInitials.Text = xmlSC_Doc.FirstChild[txtInitials.ID].InnerText;
              }
        }
        catch
        {

        }
    }
    protected void lnkReset_Click(object sender, EventArgs e)
    {
        ddlSite.SelectedIndex = 0;
        txtParticipantID.Text = "";
        txtInitials.Text = "";
        lnkSearch_Click(null, null);
    }
    protected void grdTable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strParticipantRecordID=DataBinder.Eval(e.Row.DataItem, "RecordID").ToString();
            HyperLink hfIDnInitils = e.Row.FindControl("hfIDnInitils") as HyperLink;
            hfIDnInitils.Text = DataBinder.Eval(e.Row.DataItem, "V002").ToString() + " (" + DataBinder.Eval(e.Row.DataItem, "V001").ToString() + ")";
            hfIDnInitils.NavigateUrl = _strparticipatURL + Cryptography.Encrypt(strParticipantRecordID);


            //visit V002=visit template,V134=Visit Status,V139=Monitor Reviewed-Yes/No
            DataTable dtVisit = Common.DataTableFromText(@"SELECT RecordID,V002,V134,V139 FROM [Record] WHERE TableID=" + iVisitTableID.ToString()
                + " AND IsActive=1 AND V001='" + strParticipantRecordID + "'");

           
            string strVisitEdit = "<a href='" + _strVisitEditURL + "[RecordID]" + "'><img src='jcu_images/[imagename].png' title='[imagename]' /></a>[query]";

            Label lbl6 = e.Row.FindControl("lbl6") as Label;//1755822
            Label lbl2 = e.Row.FindControl("lbl2") as Label;//1755823
            Label lbl0 = e.Row.FindControl("lbl0") as Label;//1755824
            Label lbl8 = e.Row.FindControl("lbl8") as Label;//1755825
            Label lbl12 = e.Row.FindControl("lbl12") as Label;//1755826
            Label lbl24 = e.Row.FindControl("lbl24") as Label;//1755827
            Label lbl36 = e.Row.FindControl("lbl36") as Label;//1755828
            Label lbl42 = e.Row.FindControl("lbl42") as Label;//1755829
            Label lbl44 = e.Row.FindControl("lbl44") as Label;//1755830
            Label lbl46 = e.Row.FindControl("lbl46") as Label;//1755831
            Label lbl66 = e.Row.FindControl("lbl66") as Label;//1755832
            Label lbl94 = e.Row.FindControl("lbl94") as Label;//1755833
            Label lblET = e.Row.FindControl("lblET") as Label;//1755834
            Label lbl96 = e.Row.FindControl("lbl96") as Label;//1755835
           

            foreach(DataRow drV in dtVisit.Rows)
            {
                string strOneLabel = strVisitEdit.Replace("[RecordID]", Cryptography.Encrypt(drV["RecordID"].ToString()));
                string strImageName = "";
                if (drV["V134"].ToString() != "")
                {
                    strImageName = drV["V134"].ToString();
                }

                if (drV["V139"].ToString() != "" && drV["V139"].ToString().ToLower() == "yes")
                {
                    strImageName = "verified by monitor";
                }                
                string strQuery = "";
                string strQueryCount = Common.GetValueFromSQL("SELECT COUNT(RecordID) FROM [Record] WHERE TableID=" + iQueryTableID.ToString()
                    + " AND IsActive=1 AND V001='" + drV["RecordID"].ToString() + "' AND V004<>'Yes'");

                if (strQueryCount != "" && int.Parse(strQueryCount) > 0)
                {
                    strQuery = strQueryCount;
                    if(strImageName=="Visit data is complete")
                    {
                        strQuery = "*";
                    }
                    if(strImageName=="")
                    {
                        strImageName = "Visit has an open query";
                    }
                }
                if (strImageName == "")
                {
                    strImageName = "Visit has not started yet";
                }

                strOneLabel = strOneLabel.Replace("[imagename]", strImageName);
                strOneLabel = strOneLabel.Replace("[query]","<strong>" +strQuery + "</strong>");
                switch (drV["V002"].ToString())
                {
                    case "1755822":
                        lbl6.Text = strOneLabel;
                        break;
                    case "1755823":
                        lbl2.Text = strOneLabel;
                        break;
                    case "1755824":
                        lbl0.Text = strOneLabel;
                        break;
                    case "1755825":
                        lbl8.Text = strOneLabel;
                        break;
                    case "1755826":
                        lbl12.Text = strOneLabel;
                        break;
                    case "1755827":
                        lbl24.Text = strOneLabel;
                        break;
                    case "1755828":
                        lbl36.Text = strOneLabel;
                        break;
                    case "1755829":
                        lbl42.Text = strOneLabel;
                        break;
                    case "1755830":
                        lbl44.Text = strOneLabel;
                        break;
                    case "1755831":
                        lbl46.Text = strOneLabel;
                        break;
                    case "1755832":
                        lbl66.Text = strOneLabel;
                        break;
                    case "1755833":
                        lbl94.Text = strOneLabel;
                        break;
                    case "1755834":
                        lblET.Text = strOneLabel;
                        break;
                    case "1755835":
                        lbl96.Text = strOneLabel;
                        break;
                }
              
            }

            string strVisitAdd = "<a href='" + _strVisitAddURL + Cryptography.Encrypt(strParticipantRecordID) + "'><img src='jcu_images/Visit has not started yet.png' title='Visit has not started yet' /></a>";
            if(lbl6.Text=="")
            {
                lbl6.Text = strVisitAdd;
            }
            if (lbl2.Text == "")
            {
                lbl2.Text = strVisitAdd;
            }
            if (lbl0.Text == "")
            {
                lbl0.Text = strVisitAdd;
            }
            if (lbl8.Text == "")
            {
                lbl8.Text = strVisitAdd;
            }
            if (lbl12.Text == "")
            {
                lbl12.Text = strVisitAdd;
            }
            if (lbl24.Text == "")
            {
                lbl24.Text = strVisitAdd;
            }
            if (lbl36.Text == "")
            {
                lbl36.Text = strVisitAdd;
            }
            if (lbl42.Text == "")
            {
                lbl42.Text = strVisitAdd;
            }
            if (lbl44.Text == "")
            {
                lbl44.Text = strVisitAdd;
            }
            if (lbl46.Text == "")
            {
                lbl46.Text = strVisitAdd;
            }
            if (lbl66.Text == "")
            {
                lbl66.Text = strVisitAdd;
            }
            if (lbl94.Text == "")
            {
                lbl94.Text = strVisitAdd;
            }
            if (lblET.Text == "")
            {
                lblET.Text = strVisitAdd;
            }
            if (lbl96.Text == "")
            {
                lbl96.Text = strVisitAdd;
            }
            
        }
    }
}