using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Pages_Record_ColumnColour : System.Web.UI.Page
{
   
    int _iTableID = -1;
    int _iID = -1;
    string _sContext="columnid";
    protected void Page_Load(object sender, EventArgs e)
    {
        _iTableID =int.Parse( Request.QueryString["TableID"].ToString());
        _iID = int.Parse( Request.QueryString["ID"].ToString());
        _sContext = Request.QueryString["Context"].ToString();
        

        if (!IsPostBack)
        {
            if(_sContext=="tabletabid")
            {
                lblTopTitle.Text = "Page Link Colour";
            }
            PopulateColumnColour();
           
        }
    }


    protected void SWCControllingColumnChanged(object sender, EventArgs e)
    {
        Pages_UserControl_ShowWhenCondition swcColumnColour = sender as Pages_UserControl_ShowWhenCondition;

        if (swcColumnColour != null)
        {
            GridViewRow row = swcColumnColour.NamingContainer as GridViewRow;
            Label lblID = row.FindControl("lblID") as Label;
            Label lblColumnColourID = row.FindControl("lblColumnColourID") as Label;
            ImageButton imgbtnMinus = row.FindControl("imgbtnMinus") as ImageButton;
            ImageButton imgbtnPlus = row.FindControl("imgbtnPlus") as ImageButton;

            if (swcColumnColour.ddlHideColumnV == "")
            {
                imgbtnPlus.Visible = false;
            }
            else
            {
                imgbtnPlus.Visible = true;
            }
        }


    }

    protected void grdColumnColour_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblID = e.Row.FindControl("lblID") as Label;
                Label lblColumnColourID = e.Row.FindControl("lblColumnColourID") as Label;
                ImageButton imgbtnMinus = e.Row.FindControl("imgbtnMinus") as ImageButton;
                ImageButton imgbtnPlus = e.Row.FindControl("imgbtnPlus") as ImageButton;
                DropDownList ddlTextColour = e.Row.FindControl("ddlTextColour") as DropDownList;

                Pages_UserControl_ShowWhenCondition swcColumnColour = e.Row.FindControl("swcColumnColour") as Pages_UserControl_ShowWhenCondition;
                swcColumnColour.TableID = _iTableID;

                if(_sContext=="columnid")
                    swcColumnColour.ColumnID = _iID;
                
                swcColumnColour.ShowJoinOperator = false;
                swcColumnColour.ddlJoinOperatorV = "";
               

                swcColumnColour.ddlOperatorV = DataBinder.Eval(e.Row.DataItem, "Operator").ToString();
                swcColumnColour.ddlHideColumnV = DataBinder.Eval(e.Row.DataItem, "ControllingColumnID").ToString();
                swcColumnColour.hfHideColumnValueV = DataBinder.Eval(e.Row.DataItem, "Value").ToString();

                ddlTextColour.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Colour").ToString(); 

                lblColumnColourID.Text = DataBinder.Eval(e.Row.DataItem, "ColumnColourID").ToString();
                lblID.Text = DataBinder.Eval(e.Row.DataItem, "pID").ToString();
                imgbtnMinus.CommandArgument = DataBinder.Eval(e.Row.DataItem, "pID").ToString();
                imgbtnPlus.CommandArgument = DataBinder.Eval(e.Row.DataItem, "pID").ToString();


            }
        }
        catch
        {
            //
        }
    }


    protected void SetColumnColourRowData()
    {

        if (ViewState["dtColumnColour"] != null)
        {
            DataTable dtColumnColour = (DataTable)ViewState["dtColumnColour"];
            dtColumnColour.Rows.Clear();

            //int iDisplayOrder = 0;
            if (grdColumnColour.Rows.Count > 0)
            {
                foreach (GridViewRow gvRow in grdColumnColour.Rows)
                {

                    Label lblID = gvRow.FindControl("lblID") as Label;
                    Label lblColumnColourID = gvRow.FindControl("lblColumnColourID") as Label;
                    DropDownList ddlTextColour = gvRow.FindControl("ddlTextColour") as DropDownList;
                    Pages_UserControl_ShowWhenCondition swcColumnColour = gvRow.FindControl("swcColumnColour") as Pages_UserControl_ShowWhenCondition;


                    dtColumnColour.Rows.Add(lblID.Text, lblColumnColourID.Text,_sContext,  _iID.ToString(), swcColumnColour.ddlHideColumnV,
                         swcColumnColour.ddlOperatorV, swcColumnColour.hfHideColumnValueV, ddlTextColour.SelectedValue);
                    //iDisplayOrder = iDisplayOrder + 1;
                }

            }
            dtColumnColour.AcceptChanges();
            ViewState["dtColumnColour"] = dtColumnColour;


        }
    }
    protected void PopulateColumnColour()
    {
        DataTable dtColumnColour = new DataTable();
        dtColumnColour.Columns.Add("pID");
        dtColumnColour.Columns.Add("ColumnColourID");
        dtColumnColour.Columns.Add("Context");
        dtColumnColour.Columns.Add("ID");
        dtColumnColour.Columns.Add("ControllingColumnID");
        dtColumnColour.Columns.Add("Operator");
        dtColumnColour.Columns.Add("Value");
        dtColumnColour.Columns.Add("Colour");
        dtColumnColour.AcceptChanges();

        if (_iID != -1 && ViewState["dtColumnColour"] == null)
        {


            DataTable dtColumnColourDB = Cosmetic.dbg_ColumnColour_Select(_sContext, _iID);

            if (dtColumnColour != null && dtColumnColourDB != null)
            {

                //dtColumnColour.Columns.Add("pID");

                //dtColumnColour.AcceptChanges();

                foreach (DataRow dr in dtColumnColourDB.Rows)
                {
                    DataRow newRow = dtColumnColour.NewRow();
                    newRow[0] = Guid.NewGuid().ToString();
                    newRow[1] = dr[0].ToString();
                    newRow[2] = dr[1].ToString();
                    newRow[3] = dr[2].ToString();
                    newRow[4] = dr[3].ToString();
                    newRow[5] = dr[4].ToString();
                    newRow[6] = dr[5].ToString();
                    newRow[7] = dr[6].ToString(); 

                    dtColumnColour.Rows.Add(newRow);
                }

                //foreach(DataRow dr in dtColumnColour.Rows)
                //{
                //    dr["pID"] = Guid.NewGuid().ToString();
                //}
                dtColumnColour.AcceptChanges();

                if(dtColumnColour.Rows.Count==0)
                {
                    dtColumnColour.Rows.Add(Guid.NewGuid().ToString(), "-1",_sContext, _iID.ToString(), "-1", "equals", "", "000000");
                    dtColumnColour.AcceptChanges();
                }

                ViewState["dtColumnColour"] = dtColumnColour;

                grdColumnColour.DataSource = dtColumnColour;
                grdColumnColour.DataBind();

            }
        }
        else
        {
            if (ViewState["dtColumnColour"] == null)
            {
                if (Session["dtColumnColour"] == null)
               {
                   dtColumnColour.Rows.Add(Guid.NewGuid().ToString(), "-1",_sContext, _iID.ToString(), "-1", "equals", "", "000000");

                   grdColumnColour.DataSource = dtColumnColour;
                   grdColumnColour.DataBind();

                   ViewState["dtColumnColour"] = dtColumnColour;
               }
               else
               {
                   dtColumnColour = (DataTable)Session["dtColumnColour"];
                   grdColumnColour.DataSource = dtColumnColour;
                   grdColumnColour.DataBind();

                   ViewState["dtColumnColour"] = dtColumnColour;

               }
              
            }
            else
            {
                dtColumnColour = (DataTable)ViewState["dtColumnColour"];
                grdColumnColour.DataSource = dtColumnColour;
                grdColumnColour.DataBind();
            }


        }
    }


    protected void grdColumnColour_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            SetColumnColourRowData();

            if (e.CommandName == "minus")
            {
                if (ViewState["dtColumnColour"] != null)
                {
                    DataTable dtColumnColour = (DataTable)ViewState["dtColumnColour"];

                    if (dtColumnColour.Rows.Count == 1)
                    {
                        return;
                    }

                    for (int i = dtColumnColour.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dtColumnColour.Rows[i];
                        if (dr["pID"].ToString() == e.CommandArgument.ToString())
                        {
                            dr.Delete();
                            break;
                        }

                    }


                    dtColumnColour.AcceptChanges();

                    ViewState["dtColumnColour"] = dtColumnColour;

                    PopulateColumnColour();
                    //PopulatePreviousColorColumnColour();


                }
            }
            if (e.CommandName == "plus")
            {
                if (ViewState["dtColumnColour"] != null)
                {
                    DataTable dtColumnColour = (DataTable)ViewState["dtColumnColour"];


                    int iPos = 0;

                    for (int i = 0; i < dtColumnColour.Rows.Count; i++)
                    {
                        if (dtColumnColour.Rows[i][0].ToString() == e.CommandArgument.ToString())
                        {
                            iPos = i;
                            break;
                        }
                    }


                    DataRow newRow = dtColumnColour.NewRow();
                    newRow[0] = Guid.NewGuid().ToString();
                    newRow[1] = "-1";
                    newRow[2] = _sContext;
                    newRow[3] = _iID.ToString();
                    newRow[4] = "-1";
                    newRow[5] = "equals";
                    newRow[6] = "";
                    newRow[7] = "000000";
                   


                    dtColumnColour.Rows.InsertAt(newRow, iPos + 1);

                    dtColumnColour.AcceptChanges();

                    ViewState["dtColumnColour"] = dtColumnColour;

                    PopulateColumnColour();
                    //PopulatePreviousColorColumnColour();

                    //grdColumnColour.Rows.
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

        SetColumnColourRowData();

        if (_iID != -1 && ViewState["dtColumnColour"] != null)
        {
            DataTable dtOldColumnColour = Cosmetic.dbg_ColumnColour_Select(_sContext, _iID);

            DataTable dtColumnColour = (DataTable)ViewState["dtColumnColour"];

            int iOldRowsCount = dtOldColumnColour.Rows.Count;
            int iDO = 0;
          
            string strActiveColumnColourIDs = "-1";


            bool bInsert = false;

            foreach (DataRow drCC in dtColumnColour.Rows)
            {

                if (drCC["ControllingColumnID"].ToString() == "" || drCC["Operator"].ToString() == "" || drCC["Value"].ToString() == "" || drCC["Colour"].ToString() == "")
                {
                    continue;
                }

                ColumnColour theColumnColour = new ColumnColour();

                if (iOldRowsCount>iDO)
                    theColumnColour.ColumnColourID = int.Parse(dtOldColumnColour.Rows[iDO]["ColumnColourID"].ToString());

                if (theColumnColour.ColumnColourID!=null)
                    theColumnColour = Cosmetic.dbg_ColumnColour_Detail((int)theColumnColour.ColumnColourID);

                if (theColumnColour==null || iOldRowsCount<iDO)
                {
                    bInsert = true;
                }
                theColumnColour.Context = _sContext;
                theColumnColour.ID = _iID;
                theColumnColour.ControllingColumnID = int.Parse(drCC["ControllingColumnID"].ToString());
                theColumnColour.Value = drCC["Value"].ToString();
                theColumnColour.Operator = drCC["Operator"].ToString();
                theColumnColour.Colour = drCC["Colour"].ToString();

                if (bInsert == false && theColumnColour.ColumnColourID!=null)
                {
                    Cosmetic.dbg_ColumnColour_Update(theColumnColour);
                }
                else
                {
                    theColumnColour.ColumnColourID = Cosmetic.dbg_ColumnColour_Insert(theColumnColour);                   
                }


                strActiveColumnColourIDs = strActiveColumnColourIDs + "," + theColumnColour.ColumnColourID.ToString();
                iDO = iDO + 1;
                
            }

            
            Common.ExecuteText(@"DELETE ColumnColour WHERE Context='"+_sContext+"' AND ID=" + _iID.ToString() + @" AND ColumnColourID NOT IN (" + strActiveColumnColourIDs + @")");
            
        }

        if (_iID == -1 && ViewState["dtColumnColour"] != null)
        {
            Session["dtColumnColour"] = (DataTable)ViewState["dtColumnColour"];
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Save Action", "GetBackValueEdit();", true);
                

    }


  


}