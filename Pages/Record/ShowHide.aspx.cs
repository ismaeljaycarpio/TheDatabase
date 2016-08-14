using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Pages_Record_ShowHide : System.Web.UI.Page
{
   
    int _iTableID = -1;
    int _iColumnID = -1;
    int _iDocumentSectionID = -1;
    int _iTableTabID = -1;
    string _strContext = "field";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["TableID"]!=null)
            _iTableID =int.Parse( Request.QueryString["TableID"].ToString());

        if (Request.QueryString["ColumnID"] != null)
            _iColumnID = int.Parse( Request.QueryString["ColumnID"].ToString());

        if (Request.QueryString["DocumentSectionID"] != null)
            _iDocumentSectionID = int.Parse(Request.QueryString["DocumentSectionID"].ToString());

        if (Request.QueryString["TableTabID"] != null)
            _iTableTabID = int.Parse(Request.QueryString["TableTabID"].ToString());

        if (Request.QueryString["Context"] != null)
            _strContext = Request.QueryString["Context"].ToString().ToLower();


        if (!IsPostBack)
        {

            PopulateShowWhen();
           
        }
    }


    protected void SWCHideColumnChanged(object sender, EventArgs e)
    {
        Pages_UserControl_ShowWhenCondition swcShowWhen = sender as Pages_UserControl_ShowWhenCondition;

        if (swcShowWhen != null)
        {
            GridViewRow row = swcShowWhen.NamingContainer as GridViewRow;
            Label lblID = row.FindControl("lblID") as Label;
            Label lblShowWhenID = row.FindControl("lblShowWhenID") as Label;
            ImageButton imgbtnMinus = row.FindControl("imgbtnMinus") as ImageButton;
            ImageButton imgbtnPlus = row.FindControl("imgbtnPlus") as ImageButton;

            if (swcShowWhen.ddlHideColumnV == "")
            {
                imgbtnPlus.Visible = false;
            }
            else
            {
                imgbtnPlus.Visible = true;
            }
        }


    }

    protected void grdShowWhen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblID = e.Row.FindControl("lblID") as Label;
                Label lblShowWhenID = e.Row.FindControl("lblShowWhenID") as Label;
                ImageButton imgbtnMinus = e.Row.FindControl("imgbtnMinus") as ImageButton;
                ImageButton imgbtnPlus = e.Row.FindControl("imgbtnPlus") as ImageButton;

                //imgbtnPlus.CommandArgument = e.Row.RowIndex.ToString();
                //if(grdShowWhen.Rows.Count==1)
                //{
                //    imgbtnMinus.Visible = false;
                //}


                Pages_UserControl_ShowWhenCondition swcShowWhen = e.Row.FindControl("swcShowWhen") as Pages_UserControl_ShowWhenCondition;

                if (_strContext=="dashboard")
                {
                   // swcShowWhen.TableID = _iTableID;
                    swcShowWhen.ShowTable = true;
                    swcShowWhen.DocumentSectionID = _iDocumentSectionID;
                }
                else if (_strContext == "tabletab")
                {                  
                    swcShowWhen.TableID = _iTableID;
                    swcShowWhen.TableTabID = _iTableTabID;
                }
                else
                {
                    swcShowWhen.TableID = _iTableID;
                    swcShowWhen.ColumnID = _iColumnID;
                }
                


                if (e.Row.RowIndex == 0)
                {
                    swcShowWhen.ShowJoinOperator = false;
                    swcShowWhen.ddlJoinOperatorV = "";
                }
                else
                {
                    swcShowWhen.ShowJoinOperator = true;
                    swcShowWhen.ddlJoinOperatorV = DataBinder.Eval(e.Row.DataItem, "JoinOperator").ToString();
                }

                swcShowWhen.ddlOperatorV = DataBinder.Eval(e.Row.DataItem, "HideOperator").ToString();
                swcShowWhen.ddlHideColumnV = DataBinder.Eval(e.Row.DataItem, "HideColumnID").ToString();
                swcShowWhen.hfHideColumnValueV = DataBinder.Eval(e.Row.DataItem, "HideColumnValue").ToString();


                lblShowWhenID.Text = DataBinder.Eval(e.Row.DataItem, "ShowWhenID").ToString();
                lblID.Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                imgbtnMinus.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                imgbtnPlus.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ID").ToString();


            }
        }
        catch
        {
            //
        }
    }


    protected void SetShowWhenRowData()
    {

        if (ViewState["dtShowWhen"] != null)
        {
            DataTable dtShowWhen = (DataTable)ViewState["dtShowWhen"];
            dtShowWhen.Rows.Clear();

            int iDisplayOrder = 0;
            if (grdShowWhen.Rows.Count > 0)
            {
                foreach (GridViewRow gvRow in grdShowWhen.Rows)
                {

                    Label lblID = gvRow.FindControl("lblID") as Label;
                    Label lblShowWhenID = gvRow.FindControl("lblShowWhenID") as Label;
                    Pages_UserControl_ShowWhenCondition swcShowWhen = gvRow.FindControl("swcShowWhen") as Pages_UserControl_ShowWhenCondition;


                    dtShowWhen.Rows.Add(lblID.Text, lblShowWhenID.Text,_strContext=="field"? _iColumnID.ToString():"", swcShowWhen.ddlHideColumnV,
                        swcShowWhen.hfHideColumnValueV, swcShowWhen.ddlOperatorV, iDisplayOrder.ToString(), swcShowWhen.ddlJoinOperatorV,
                        _strContext == "dashboard" ? _iDocumentSectionID.ToString() : "",
                         _strContext == "tabletab" ? _iTableTabID.ToString() : "",
                        _strContext);
                    iDisplayOrder = iDisplayOrder + 1;
                }

            }
            dtShowWhen.AcceptChanges();
            ViewState["dtShowWhen"] = dtShowWhen;


        }
    }
    protected void PopulateShowWhen()
    {
        DataTable dtShowWhen = new DataTable();
        dtShowWhen.Columns.Add("ID");
        dtShowWhen.Columns.Add("ShowWhenID");
        dtShowWhen.Columns.Add("ColumnID");
        dtShowWhen.Columns.Add("HideColumnID");
        dtShowWhen.Columns.Add("HideColumnValue");
        dtShowWhen.Columns.Add("HideOperator");
        dtShowWhen.Columns.Add("DisplayOrder");
        dtShowWhen.Columns.Add("JoinOperator");
        dtShowWhen.Columns.Add("DocumentSectionID");       
        dtShowWhen.Columns.Add("Context");
        dtShowWhen.Columns.Add("TableTabID");
        dtShowWhen.AcceptChanges();

        if ((_iColumnID != -1 || _iDocumentSectionID!=-1 || _iTableTabID!=-1) && ViewState["dtShowWhen"] == null)
        {


            DataTable dtShowWhenDB = RecordManager.dbg_ShowWhen_ForGrid(_strContext == "field" ? (int?)_iColumnID : null,
                                                                    _strContext == "dashboard" ? (int?)_iDocumentSectionID : null,
                                                                    _strContext=="tabletab"?(int?)_iTableTabID:null);

            if (dtShowWhen != null && dtShowWhenDB != null)
            {

                //dtShowWhen.Columns.Add("ID");

                //dtShowWhen.AcceptChanges();

                foreach (DataRow dr in dtShowWhenDB.Rows)
                {
                    DataRow newRow = dtShowWhen.NewRow();
                    newRow[0] = Guid.NewGuid().ToString();
                    newRow[1] = dr[1].ToString();
                    newRow[2] = dr[2].ToString();
                    newRow[3] = dr[3].ToString();
                    newRow[4] = dr[4].ToString();
                    newRow[5] = dr[5].ToString();
                    newRow[6] = dr[6].ToString();
                    newRow[7] = dr[7].ToString();
                    newRow[8] = dr[8].ToString();
                    newRow[8] = dr[9].ToString();
                    newRow[9] = dr[10].ToString();
                    dtShowWhen.Rows.Add(newRow);
                }

                //foreach(DataRow dr in dtShowWhen.Rows)
                //{
                //    dr["ID"] = Guid.NewGuid().ToString();
                //}
                dtShowWhen.AcceptChanges();

                if(dtShowWhen.Rows.Count==0)
                {
                    dtShowWhen.Rows.Add(Guid.NewGuid().ToString(),"-1", _strContext=="field"?"-1":"",
                        "", "", "equals", "1", "", _strContext == "dashboard" ? "-1" : "", _strContext, _strContext == "tabletab" ? "-1" : "");
                    dtShowWhen.AcceptChanges();
                }

                ViewState["dtShowWhen"] = dtShowWhen;

                grdShowWhen.DataSource = dtShowWhen;
                grdShowWhen.DataBind();

            }
        }
        else
        {
            if (ViewState["dtShowWhen"] == null)
            {
                if (Session["dtShowWhen"] == null)
               {
                   dtShowWhen.Rows.Add(Guid.NewGuid().ToString(), "-1",  _strContext == "field" ? "-1" : "", "", "",
                       "equals", "1", "", _strContext == "dashboard" ? "-1" : "", _strContext, _strContext == "tabletab" ? "-1" : "");

                   grdShowWhen.DataSource = dtShowWhen;
                   grdShowWhen.DataBind();

                   ViewState["dtShowWhen"] = dtShowWhen;
               }
               else
               {
                   dtShowWhen = (DataTable)Session["dtShowWhen"];
                   grdShowWhen.DataSource = dtShowWhen;
                   grdShowWhen.DataBind();

                   ViewState["dtShowWhen"] = dtShowWhen;

               }
              
            }
            else
            {
                dtShowWhen = (DataTable)ViewState["dtShowWhen"];
                grdShowWhen.DataSource = dtShowWhen;
                grdShowWhen.DataBind();
            }


        }
    }


    protected void grdShowWhen_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            SetShowWhenRowData();

            if (e.CommandName == "minus")
            {
                if (ViewState["dtShowWhen"] != null)
                {
                    DataTable dtShowWhen = (DataTable)ViewState["dtShowWhen"];

                    if (dtShowWhen.Rows.Count == 1)
                    {
                        return;
                    }

                    for (int i = dtShowWhen.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dtShowWhen.Rows[i];
                        if (dr["id"].ToString() == e.CommandArgument.ToString())
                        {
                            dr.Delete();
                            break;
                        }

                    }


                    dtShowWhen.AcceptChanges();

                    ViewState["dtShowWhen"] = dtShowWhen;

                    PopulateShowWhen();
                    //PopulatePreviousColorShowWhen();


                }
            }
            if (e.CommandName == "plus")
            {
                if (ViewState["dtShowWhen"] != null)
                {
                    DataTable dtShowWhen = (DataTable)ViewState["dtShowWhen"];


                    int iPos = 0;

                    for (int i = 0; i < dtShowWhen.Rows.Count; i++)
                    {
                        if (dtShowWhen.Rows[i][0].ToString() == e.CommandArgument.ToString())
                        {
                            iPos = i;
                            break;
                        }
                    }


                    DataRow newRow = dtShowWhen.NewRow();
                    newRow[0] = Guid.NewGuid().ToString();
                    newRow[1] = "-1";
                    newRow[2] = _strContext == "field" ? _iColumnID.ToString() : "";
                    newRow[3] = "";
                    newRow[4] = "";
                    newRow[5] = "equals";
                    newRow[6] = (dtShowWhen.Rows.Count + 1).ToString();
                    newRow[7] = "";
                    newRow[8] = _strContext == "dashboard" ? _iDocumentSectionID.ToString() : "";
                    newRow[9] = _strContext;
                    newRow[10] = _strContext == "tabletab" ? _iTableTabID.ToString() : "";

                    dtShowWhen.Rows.InsertAt(newRow, iPos + 1);

                    dtShowWhen.AcceptChanges();

                    ViewState["dtShowWhen"] = dtShowWhen;

                    PopulateShowWhen();
                    //PopulatePreviousColorShowWhen();

                    //grdShowWhen.Rows.
                }

            }
        }
        catch
        {
            //
        }
    }

  
        

    protected void lnkSave_Click(object sender, EventArgs e)
    {

        SetShowWhenRowData();

        if (ViewState["dtShowWhen"] != null && ((_strContext == "field" && _iColumnID != -1)
            || (_strContext == "dashboard" && _iDocumentSectionID != -1) || (_strContext == "tabletab" && _iTableTabID != -1)))
        {
            //Common.ExecuteText("UPDATE [Column] SET HideColumnID=" + ddlHideColumn.SelectedValue
            //    + ", HideColumnValue='" + hfHideColumnValue.Value.Replace("'", "''") + "',HideOperator='"+ddlOperator.SelectedValue+"' WHERE ColumnID=" + _qsColumnID);

            DataTable dtOldShowWhen = RecordManager.dbg_ShowWhen_Select(_strContext == "field" ? (int?)_iColumnID : null,
                                                                        _strContext == "dashboard" ? (int?)_iDocumentSectionID : null,
                                                                        _strContext == "tabletab" ? (int?)_iTableTabID : null);

            DataTable dtShowWhen = (DataTable)ViewState["dtShowWhen"];


            int iOldRowsCount = dtOldShowWhen.Rows.Count;
            string strActiveShowWhenIDs = "-1";
            int iDO = 1;

            bool bInsert = false;

            foreach (DataRow drSW in dtShowWhen.Rows)
            {
                if (iDO == 1)
                {
                    if (drSW["HideColumnID"].ToString() == "") //|| drSW["HideColumnValue"].ToString() == ""
                    {
                        continue;
                    }
                    ShowWhen theShowWhen1 = new ShowWhen();

                    if (iOldRowsCount > 0)
                        theShowWhen1.ShowWhenID = int.Parse(dtOldShowWhen.Rows[0]["ShowWhenID"].ToString());

                    theShowWhen1.ColumnID = _strContext == "field" ? (int?)_iColumnID : null;
                    theShowWhen1.DocumentSectionID = _strContext == "dashboard" ? (int?)_iDocumentSectionID : null;
                    theShowWhen1.TableTabID = _strContext == "tabletab" ? (int?)_iTableTabID : null;
                    theShowWhen1.Context = _strContext;

                    theShowWhen1.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                    theShowWhen1.HideColumnValue = drSW["HideColumnValue"].ToString();
                    theShowWhen1.HideOperator = drSW["HideOperator"].ToString();
                    theShowWhen1.DisplayOrder = 1;
                    theShowWhen1.JoinOperator = "";

                    if (iOldRowsCount > 0)
                    {
                        RecordManager.dbg_ShowWhen_Update(theShowWhen1);
                    }
                    else
                    {
                        theShowWhen1.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen1);
                        bInsert = true;
                    }


                    strActiveShowWhenIDs = strActiveShowWhenIDs + "," + theShowWhen1.ShowWhenID.ToString();
                    iDO = iDO + 1;
                    continue;
                }
                //|| drSW["HideColumnValue"].ToString() == ""
                if (drSW["HideColumnID"].ToString() == ""  || drSW["JoinOperator"].ToString() == "")
                {
                    continue;
                }




                if (iOldRowsCount - iDO < 0)
                {
                    bInsert = true;
                }

                if (bInsert == false)
                {
                    ShowWhen theShowWhenJoin = new ShowWhen();
                    theShowWhenJoin.ShowWhenID = int.Parse(dtOldShowWhen.Rows[iDO - 1]["ShowWhenID"].ToString());

                    theShowWhenJoin.ColumnID = _strContext == "field" ? (int?)_iColumnID : null;
                    theShowWhenJoin.DocumentSectionID = _strContext == "dashboard" ? (int?)_iDocumentSectionID : null;
                    theShowWhenJoin.TableTabID = _strContext == "tabletab" ? (int?)_iTableTabID : null;
                    theShowWhenJoin.Context = _strContext;

                    theShowWhenJoin.HideColumnID = null;
                    theShowWhenJoin.HideColumnValue = "";
                    theShowWhenJoin.HideOperator = "";
                    theShowWhenJoin.DisplayOrder = iDO;
                    theShowWhenJoin.JoinOperator = drSW["JoinOperator"].ToString();

                    RecordManager.dbg_ShowWhen_Update(theShowWhenJoin);

                    strActiveShowWhenIDs = strActiveShowWhenIDs + "," + theShowWhenJoin.ShowWhenID.ToString();
                    iDO = iDO + 1;

                    if (iOldRowsCount - iDO < 0)
                    {
                        bInsert = true;
                    }


                    ShowWhen theShowWhen = new ShowWhen();
                    if (bInsert == false)
                        theShowWhen.ShowWhenID = int.Parse(dtOldShowWhen.Rows[iDO - 1]["ShowWhenID"].ToString());

                   
                    theShowWhen.ColumnID = _strContext == "field" ? (int?)_iColumnID : null;
                    theShowWhen.DocumentSectionID = _strContext == "dashboard" ? (int?)_iDocumentSectionID : null;
                    theShowWhen.TableTabID = _strContext == "tabletab" ? (int?)_iTableTabID : null;
                    theShowWhen.Context = _strContext;


                    theShowWhen.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                    theShowWhen.HideColumnValue = drSW["HideColumnValue"].ToString();
                    theShowWhen.HideOperator = drSW["HideOperator"].ToString();
                    theShowWhen.DisplayOrder = iDO;
                    theShowWhen.JoinOperator = "";

                    if (bInsert == false)
                    {
                        RecordManager.dbg_ShowWhen_Update(theShowWhen);
                    }
                    else
                    {
                        theShowWhen.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen);
                    }


                    strActiveShowWhenIDs = strActiveShowWhenIDs + "," + theShowWhen.ShowWhenID.ToString();
                    iDO = iDO + 1;

                    if (iOldRowsCount - iDO < 0)
                    {
                        bInsert = true;
                    }
                    continue;
                }
                else
                {
                    ShowWhen theShowWhenJoin = new ShowWhen();
                    
                    theShowWhenJoin.ColumnID = _strContext == "field" ? (int?)_iColumnID : null;
                    theShowWhenJoin.DocumentSectionID = _strContext == "dashboard" ? (int?)_iDocumentSectionID : null;
                    theShowWhenJoin.TableTabID = _strContext == "tabletab" ? (int?)_iTableTabID : null;
                    theShowWhenJoin.Context = _strContext;

                    theShowWhenJoin.HideColumnID = null;
                    theShowWhenJoin.HideColumnValue = "";
                    theShowWhenJoin.HideOperator = "";
                    theShowWhenJoin.DisplayOrder = iDO;
                    theShowWhenJoin.JoinOperator = drSW["JoinOperator"].ToString();

                    theShowWhenJoin.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhenJoin);

                    strActiveShowWhenIDs = strActiveShowWhenIDs + "," + theShowWhenJoin.ShowWhenID.ToString();
                    iDO = iDO + 1;

                    ShowWhen theShowWhen = new ShowWhen();
                   
                    theShowWhen.ColumnID = _strContext == "field" ? (int?)_iColumnID : null;
                    theShowWhen.DocumentSectionID = _strContext == "dashboard" ? (int?)_iDocumentSectionID : null;
                    theShowWhen.TableTabID = _strContext == "tabletab" ? (int?)_iTableTabID : null;
                    theShowWhen.Context = _strContext;

                    theShowWhen.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                    theShowWhen.HideColumnValue = drSW["HideColumnValue"].ToString();
                    theShowWhen.HideOperator = drSW["HideOperator"].ToString();
                    theShowWhen.DisplayOrder = iDO;
                    theShowWhen.JoinOperator = "";

                    theShowWhen.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen);

                    strActiveShowWhenIDs = strActiveShowWhenIDs + "," + theShowWhen.ShowWhenID.ToString();
                    iDO = iDO + 1;
                }
            }

            if(bInsert==false)
            {
                if (_strContext == "dashboard")
                {
                    Common.ExecuteText(@"DELETE ShowWhen WHERE DocumentSectionID=" + _iDocumentSectionID.ToString() 
                        + @" AND ShowWhenID NOT IN (" + strActiveShowWhenIDs + @")");

                }
                else if (_strContext == "tabletab")
                {
                    Common.ExecuteText(@"DELETE ShowWhen WHERE TableTabID=" + _iTableTabID.ToString()
                        + @" AND ShowWhenID NOT IN (" + strActiveShowWhenIDs + @")");

                }
                else
                {
                    Common.ExecuteText(@"DELETE ShowWhen WHERE ColumnID=" + _iColumnID.ToString() 
                        + @" AND ShowWhenID NOT IN (" + strActiveShowWhenIDs + @")");

                }
            }
        }

        if ( ViewState["dtShowWhen"] != null &&
            ((_strContext == "field" && _iColumnID == -1) || (_strContext == "dashboard" && _iDocumentSectionID == -1)
            || (_strContext == "tabletab" && _iTableTabID == -1)))
        {
            Session["dtShowWhen"] = (DataTable)ViewState["dtShowWhen"];
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Save Action", "GetBackValueEdit();", true);
                

    }


  


}