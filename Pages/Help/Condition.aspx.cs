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

public partial class Pages_Schedule_Condition : SecurePage
{
    Common_Pager _gvPager;

    //int _iSearchCriteriaID = -1;
    //int _iStartIndex = 0;
    //int _iMaxRows = 10;
    //string _strGridViewSortColumn = "ConditionID";
    //string _strGridViewSortDirection = "ASC";
    int _iColumnID = -1;
    string _strConTypeDisplay = "Invalid";
    Table _theTable;

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            ViewState["ConditionType"] = Request.QueryString["ConditionType"].ToString().Trim();

        _theTable = RecordManager.ets_Table_Details(int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString())));

        switch (ViewState["ConditionType"].ToString().ToUpper())
        {
            case "W":
                _strConTypeDisplay = "Warning";
                break;
            case "E":
                _strConTypeDisplay = "Exceedance";
                break;
            default:
                _strConTypeDisplay = "Invalid";
                break;
        }


        lblTopTitle.Text = "Data " + _strConTypeDisplay + " if outside the range";
        Title = lblTopTitle.Text;

        try
        {


            User ObjUser = (User)Session["User"];
            _iColumnID = int.Parse(Request.QueryString["ColumnID"].ToString());
         
            //if (_iColumnID == -1)
            //{
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Please add the column first.');parent.$.fancybox.close();", true);
            //    //return;
            //}

            if (!IsPostBack)
            {

                if (_iColumnID == -1)
                {
                    lblSubTitle.Text = _theTable.TableName + " - New Column"; 
                }
                else
                {
                    Column thisColumn = RecordManager.ets_Column_Details(_iColumnID);
                    lblSubTitle.Text = _theTable.TableName + " - " + thisColumn.DisplayName;
                }
                if (_iColumnID == -1)
                {
                    if (Session["Condition"] == null)
                    {

                        DataTable dtCondition = new DataTable();
                        dtCondition.Columns.Add("ConditionID");
                        dtCondition.Columns.Add("ColumnID");
                        dtCondition.Columns.Add("ConditionType");
                        dtCondition.Columns.Add("CheckColumnID");
                        dtCondition.Columns.Add("CheckValue");
                        dtCondition.Columns.Add("CheckFormula");
                        dtCondition.Columns.Add("DisplayName");
                        Session["Condition"] = dtCondition;
                    }

                }

                
                //if (Request.QueryString["SearchCriteria"] != null)
                //{
                //    PopulateSearchCriteria(int.Parse(Cryptography.Decrypt(Request.QueryString["SearchCriteria"].ToString())));
                //}


                //if (Session["GridPageSize"] != null && Session["GridPageSize"].ToString() != "")
                //{ 
                    gvTheGrid.PageSize = 50; 
                //}

                //if (Request.QueryString["SearchCriteria"] != null)
                //{
                //    gvTheGrid.PageSize = _iMaxRows;
                //    gvTheGrid.GridViewSortColumn = _strGridViewSortColumn;
                //    if (_strGridViewSortDirection.ToUpper() == "ASC")
                //    {
                //        gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                //    }
                //    else
                //    {
                //        gvTheGrid.GridViewSortDirection = SortDirection.Descending;
                //    }
                //    BindTheGrid(_iStartIndex, _iMaxRows);
                //}
                //else
                //{
                    //gvTheGrid.GridViewSortColumn = "ConditionID";
                    //gvTheGrid.GridViewSortDirection = SortDirection.Ascending;
                    BindTheGrid(0, gvTheGrid.PageSize);
                //}
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
            ErrorLog theErrorLog = new ErrorLog(null, "Condition", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }




    //protected void PopulateSearchCriteria(int iSearchCriteriaID)
    //{
    //    try
    //    {
    //        SearchCriteria theSearchCriteria = SystemData.SearchCriteria_Detail(iSearchCriteriaID);


    //        if (theSearchCriteria != null)
    //        {

    //            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

    //            xmlDoc.Load(new StringReader(theSearchCriteria.SearchText));

    //            //txtOptionKeySearch.Text = xmlDoc.FirstChild[txtOptionKeySearch.ID].InnerText;

    //            _iStartIndex = int.Parse(xmlDoc.FirstChild["iStartIndex"].InnerText);
    //            _iMaxRows = int.Parse(xmlDoc.FirstChild["iMaxRows"].InnerText);
    //            _strGridViewSortColumn = xmlDoc.FirstChild["GridViewSortColumn"].InnerText;
    //            _strGridViewSortDirection = xmlDoc.FirstChild["GridViewSortDirection"].InnerText;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.Message;
    //    }


    //}



    protected void BindTheGrid(int iStartIndex, int iMaxRows)
    {




        lblMsg.Text = "";

        //SearchCriteria 
        //try
        //{
        //    string xml = null;
        //    xml = @"<root>" +
        //           //" <" + txtOptionKeySearch.ID + ">" + HttpUtility.HtmlEncode(txtOptionKeySearch.Text) + "</" + txtOptionKeySearch.ID + ">" +
        //           " <GridViewSortColumn>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortColumn) + "</GridViewSortColumn>" +
        //           " <GridViewSortDirection>" + HttpUtility.HtmlEncode(gvTheGrid.GridViewSortDirection == SortDirection.Ascending ? "ASC" : "DESC") + "</GridViewSortDirection>" +
        //           " <iStartIndex>" + HttpUtility.HtmlEncode(iStartIndex.ToString()) + "</iStartIndex>" +
        //           " <iMaxRows>" + HttpUtility.HtmlEncode(iMaxRows.ToString()) + "</iMaxRows>" +
        //          "</root>";

        //    SearchCriteria theSearchCriteria = new SearchCriteria(null, xml);
        //    _iSearchCriteriaID = SystemData.SearchCriteria_Insert(theSearchCriteria);
        //}
        //catch (Exception ex)
        //{
        //    lblMsg.Text = ex.Message;
        //}

        //End Searchcriteria





        try
        {
            int iTN = 0;
            DataTable dtCondition;

            if (_iColumnID == -1 && Session["Condition"]!=null)
            {
                dtCondition = (DataTable)Session["Condition"];               
            }
            else
            {
                dtCondition = UploadWorld.dbg_Condition_Select(_iColumnID, null, "", "");
            }

            DataTable dtConX=new DataTable();
            if(dtCondition.Rows.Count>0)
            {
                DataRow[] drCon = dtCondition.Select("ConditionType='" + ViewState["ConditionType"].ToString() + "'");
                if (drCon.Length > 0)
                    dtConX = dtCondition.Select("ConditionType='" + ViewState["ConditionType"].ToString() + "'").CopyToDataTable();
            }

            if (dtConX==null)
                dtConX = dtCondition;
            
            gvTheGrid.DataSource = dtConX;
            iTN = dtConX.Rows.Count;

            //Session["ConditionCount"] = iTN;
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
                if (IsFiltered())
                {
                    divNoFilter.Visible = true;
                    divEmptyData.Visible = false;
                }
                else
                {
                    divEmptyData.Visible = true;
                    divNoFilter.Visible = false;
                }
                hplNewData.NavigateUrl = GetAddURL();
                hplNewDataFilter.NavigateUrl = GetAddURL();
            }
            else
            {
                divEmptyData.Visible = false;
                divNoFilter.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Condition", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void gvTheGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Column theCheckColumn = RecordManager.ets_Column_Details(int.Parse(DataBinder.Eval(e.Row.DataItem, "CheckColumnID").ToString()));
            if (DataBinder.Eval(e.Row.DataItem, "CheckValue") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "CheckValue").ToString()!="")
            {
                Label lblCheckValue = (Label)e.Row.FindControl("lblCheckValue");
                lblCheckValue.Text = GetColumnDisplay(theCheckColumn, DataBinder.Eval(e.Row.DataItem, "CheckValue").ToString());
            }
            if (DataBinder.Eval(e.Row.DataItem, "CheckFormula") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "CheckFormula").ToString() != "")
            {
                string strFormula = DataBinder.Eval(e.Row.DataItem, "CheckFormula").ToString();
                Label lblMin = (Label)e.Row.FindControl("lblMin");
                Label lblMax = (Label)e.Row.FindControl("lblMax");
               lblMin.Text= Common.GetMinFromFormula(strFormula);
               lblMax.Text = Common.GetMaxFromFormula(strFormula); 
            }
        }
    }


    protected string GetColumnDisplay(Column theColumn, string strValue)
    {
        string strDisplay = "";

        try
        {
            if (theColumn != null)
            {
                switch (theColumn.ColumnType)
                {
                    case "listbox":

                        if (theColumn.DateCalculationType == "")
                        {

                            if (theColumn.DropDownType == "values")
                            {
                                strDisplay = strValue;
                            }
                            else if (theColumn.DropDownType == "value_text")
                            {
                                strDisplay = Common.GetTextFromValueForList(theColumn.DropdownValues, strValue);
                            }
                            else
                            {
                                if (theColumn.DropDownType == "table" && theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                                {
                                  strDisplay=  Common.GetListTableDisplay(theColumn.DisplayColumn, (int)theColumn.TableTableID, " AND Record.RecordID IN (" + strValue + ")", "");
                                }

                            }

                        }
                        if (theColumn.DateCalculationType == "checkbox")
                        {                           

                            if (theColumn.DropDownType == "values")
                            {
                                strDisplay = strValue;
                            }
                            else if (theColumn.DropDownType == "value_text")
                            {
                                strDisplay = Common.GetTextFromValueForList(theColumn.DropdownValues, strValue);
                            }
                            else
                            {
                                if (theColumn.DropDownType == "table" && theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                                {
                                    strDisplay = Common.GetListTableDisplay(theColumn.DisplayColumn, (int)theColumn.TableTableID, " AND Record.RecordID IN (" + strValue + ")",
                                        "");
                                }

                            }

                        }

                        break;
                    case "radiobutton":

                        if (theColumn.DropDownType == "values")
                        {
                            strDisplay = strValue;
                        }
                        else if (theColumn.DropdownValues!="")
                        {
                            strDisplay = Common.GetTextFromValue(theColumn.DropdownValues, strValue);
                        }


                        break;
                    case "checkbox":
                        strDisplay = strValue;
                        break;
                    case "number":
                        strDisplay = strValue;
                        break;
                    case "datetime":
                        strDisplay = strValue;
                      
                        break;

                    case "date":
                        DateTime dtTempDate = DateTime.Parse(strValue);
                        strDisplay = dtTempDate.Day.ToString() + "/" + dtTempDate.Month.ToString("00") + "/" + dtTempDate.Year.ToString();

                        break;

                    case "time":
                        strDisplay = Convert.ToDateTime(strValue).ToString("HH:m");
                        break;

                    case "dropdown":
                        if (theColumn.DropDownType == "values")
                        {
                            strDisplay = strValue;
                        }
                        else if (theColumn.DropDownType == "value_text")
                        {
                            strDisplay = Common.GetTextFromValue(theColumn.DropdownValues, strValue);
                        }
                        else
                        {
                            if (theColumn.TableTableID != null && theColumn.DisplayColumn != "")
                            {
                                strDisplay = Common.GetLinkedDisplayText(theColumn.DisplayColumn, (int)theColumn.TableTableID, null, " AND Record.RecordID=" + strValue + "",  "");
                            }

                        }
                        break;

                    default:
                        strDisplay = strValue;
                        break;
                }

            }

        }
        catch
        {
            //
        }
       
        return strDisplay;
    }
    protected void gvTheGrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        BindTheGrid(0, gvTheGrid.PageSize);
    }

    //protected void btnSearch_Click(object sender, ImageClickEventArgs e)

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

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/ConditionDetail.aspx?ConditionType=" + ViewState["ConditionType"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&mode=" + Cryptography.Encrypt("edit") + "&ConditionID=";

    }


    public string GetViewURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/ConditionDetail.aspx?ConditionType=" + ViewState["ConditionType"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&mode=" + Cryptography.Encrypt("view") + "&ConditionID=";

    }
    public string GetAddURL()
    {

        return Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Help/ConditionDetail.aspx?ConditionType=" + ViewState["ConditionType"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&ColumnID=" + Request.QueryString["ColumnID"].ToString() + "&mode=" + Cryptography.Encrypt("add");

    }



    //protected void Pager_BindTheGridToExport(object sender, EventArgs e)
    //{
    //    _gvPager.ExportFileName = "Data Reminders";
    //    BindTheGrid(0, _gvPager.TotalRows);
    //}

    protected void Pager_BindTheGridAgain(object sender, EventArgs e)
    {
        BindTheGrid(_gvPager.StartIndex, _gvPager._gridView.PageSize);
    }

    //protected void Pager_OnApplyFilter(object sender, EventArgs e)
    //{
    //    //txtOptionKeySearch.Text = "";
    //    gvTheGrid.GridViewSortColumn = "ConditionID";
    //    gvTheGrid.GridViewSortDirection = SortDirection.Descending;
    //    lnkSearch_Click(null, null);
    //    //BindTheGrid(0, gvTheGrid.PageSize);
    //}


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
                        if (_iColumnID == -1)
                        {
                            if (Session["Condition"] != null )
                            {
                                DataTable dtCondition = (DataTable)Session["Condition"];
                                for (int i = 0; i < dtCondition.Rows.Count; i++)
                                {
                                    if (sTemp == dtCondition.Rows[i]["ConditionID"].ToString())
                                    {
                                        dtCondition.Rows.RemoveAt(i);
                                    }
                                }

                                dtCondition.AcceptChanges();
                                Session["Condition"] = dtCondition;

                            }

                        }
                        else
                        {
                            UploadWorld.dbg_Condition_Delete(Convert.ToInt32(sTemp));
                        }
                    }
                }


            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Condition delete", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            //lblMsg.Text = ex.Message;

            //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "msg_delete", "alert('Delete User has failed!');", true);
        }
    }


    protected bool IsFiltered()
    {
        //if (txtOptionKeySearch.Text != "")
        //{
        //    return true;
        //}

        return false;
    }

}
