using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Pages_LookUp_LookUp : SecurePage
{
    Common_Pager _gvPager;
    User _ObjUser;
    string _strContentKeySearch = "";
    int? _iLookupTypeID=null;
    LookupType _theLookupType;

    int _iSearchCriteriaID = -1;
    int _iStartIndex = 0;
    int _iMaxRows = 10;
    string _strGridViewSortColumn = "DisplayText";
    string _strGridViewSortDirection = "ASC";


    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
               
        try
        {
            _iLookupTypeID = int.Parse(Cryptography.Decrypt(Request.QueryString["LookupTypeID"].ToString()));

            _theLookupType = SystemData.LookUpType_Detail((int)_iLookupTypeID);

            Title = _theLookupType.LookupTypeName;
            lblTitle.Text = _theLookupType.LookupTypeName;
            gvTheGrid.Columns[3].HeaderText = _theLookupType.LookupTypeName; ;
            _ObjUser = (User)Session["User"];

            if (!IsPostBack)
            {

                if (!Common.HaveAccess(Session["roletype"].ToString(), "1" ))
                { Response.Redirect("~/Default.aspx", false); }


                if (Request.QueryString["SearchCriteria"] != null)
                {
                    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
                }

                if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                { gvTheGrid.PageSize = int.Parse(Session["GridPageSize"].ToString()); }




                if (Request.QueryString["SearchCriteria"] != null)
                {
                    gvTheGrid.PageSize = _iMaxRows;
                    gvTheGrid.GridViewSortColumn = _strGridViewSortColumn;
                    if (_strGridViewSortDirection.ToUpper() == "ASC")
                    {
                        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    }
                    else
                    {
                        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                    }
                    BindTheGrid(_iStartIndex, _iMaxRows);
                }
                else
                {
                    gvTheGrid.GridViewSortColumn = "DisplayText";
                    gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                }
            }
            else
            {
            }

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
                _gvPager = (Common_Pager)gvr.FindControl("Pager");


        }
        catch (Exception ex)
        {
            
            lblMsg.Text = ex.Message;
        }
    }



    protected void PopulateSearchCriteria(int iSearchCriteriaID)
    {
        try
        {
            SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


            if (theSearchCriteria != null)
            {

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

                txtDisplayText.Text = xmlDoc.FirstChild[txtDisplayText.ID].InnerText;

                _iStartIndex = int.Parse(xmlDoc.FirstChild["iStartIndex"].InnerText);
                _iMaxRows = int.Parse(xmlDoc.FirstChild["iMaxRows"].InnerText);
                _strGridViewSortColumn = xmlDoc.FirstChild["GridViewSortColumn"].InnerText;
                _strGridViewSortDirection = xmlDoc.FirstChild["GridViewSortDirection"].InnerText;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }


    }



    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {



        lblMsg.Text = "";

        //SearchCriteria 
        try
        {
            string xml = null;
            xml = @"<root>" +
                   " <" + txtDisplayText.ID + ">" + HttpUtility.HtmlEncode(txtDisplayText.Text) + "</" + txtDisplayText.ID + ">" +
                   " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
                   " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
                   " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
                   " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
                  "</root>";

            SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
            _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }

        //End Searchcriteria





        try
        {
            int iTN = 0;         

           

            gvTheGrid.DataSource = SystemData.LookUpData_Select(null,
                _iLookupTypeID, 
                txtDisplayText.Text,"",null,null,
                gvTheGrid.GridViewSortColumn, gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC",
                iStartIndex, iMaxRows, ref iTN,  "");

            gvTheGrid.VirtualItemCount = iTN;
            gvTheGrid.DataBind();
            if (gvTheGrid.TopPagerRow != null)
                gvTheGrid.TopPagerRow.Visible = true;

            GridViewRow gvr = gvTheGrid.TopPagerRow;
            if (gvr != null)
            {
                _gvPager = (Common_Pager)gvr.FindControl("Pager");
                _gvPager.AddURL = GetAddURL();
                _gvPager.PageIndexTextSet = (int)(iStartIndex / iMaxRows + 1);
                
            }
                       
            if (iTN == 0)
            {
                divEmptyData.Visible = true;
                hplNewData.NavigateUrl = GetAddURL();
               
            }
            else
            {
                divEmptyData.Visible = false;
            }

        }
        catch (Exception ex)
        {
           
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }
    
   
    
    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);      

    }

    protected void gvTheGrid_PreRender(object sender, EventArgs e)
    {
        GridView grid = (GridView)sender;
        if (grid != null)
        {
            GridViewRow pagerRow = (GridViewRow)grid.TopPagerRow;
            if (pagerRow != null)
            {
                pagerRow.Visible = true;
            }
        }
    }

    public string GetEditURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/LookUp/LookUpDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&LookupTypeID=" + Request.QueryString["LookupTypeID"].ToString() + "&LookupDataID=";

    }

   
    public string GetAddURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/LookUp/LookUpDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&SearchCriteria=" + Cryptography.Encrypt(_iSearchCriteriaID.ToString()) + "&LookupTypeID=" + Request.QueryString["LookupTypeID"].ToString();

    }


   

    protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    {
        _gvPager.ExportFileName = "Contents";
        BindTheGrid(0, _gvPager.TotalRows);
    }

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    protected void Pager_OnApplyFilter(object sender, EventArgs e)
    {
        txtDisplayText.Text = "";
        lnkSearch_Click(null, null);

        //BindTheGrid(0, gvTheGrid.PageSize);
    }

    protected void Pager_DeleteAction(object sender, EventArgs e)
    {
        string sCheck = "";
        for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        {
            bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
            if (ischeck)
            {
                sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
            }
        }
        if (string.IsNullOrEmpty(sCheck))
        {
            ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
        }
        else
        {
            DeleteItem(sCheck);
            BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
            _gvPager._gridView.PageIndex = _gvPager.PageIndex - 1;
            if (_gvPager._gridView.Rows.Count == 0 && _gvPager._gridView.PageIndex > 0)
            {
                BindTheGrid(_gvPager.StartIndex - gvTheGrid.PageSize, gvTheGrid.PageSize);
            }
        }

    }



    private void DeleteItem(string keys)
    {
        try
        {
            if (!string.IsNullOrEmpty(keys))
            {

                foreach (string sTemp in keys.Split(','))
                {
                    if (!string.IsNullOrEmpty(sTemp))
                    {

                        LookUpData theLookUpData = SystemData.LookUpData_Detail(int.Parse(sTemp));

                        if (theLookUpData != null)
                        {
                            if ((int)theLookUpData.LookupDataID < 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('We can not delete System Data.');", true);
                                return;
                            }
                            else
                            {
                                SystemData.LookUpData_Delete(int.Parse(sTemp));
                            }
                        }
                       

                    }
                }


            }
        }
        catch (Exception ex)
        {
           
            lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }



    protected void chkShowOnlyGlobal_CheckedChanged(object sender, EventArgs e)
    {
        BindTheGrid(0, gvTheGrid.PageSize);      

    }
}
