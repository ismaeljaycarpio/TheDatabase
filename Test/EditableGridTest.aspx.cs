using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DocGen.Utility;
using System.Configuration;
using DocGen.DAL;
using System.Drawing;
using System.Data;
using System.Xml;

public partial class Test_EditableGridTest : System.Web.UI.Page
{

    int _iTableID = 2721;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            PopulateCalendarColor();
        }

    }

    protected void CBCColorYAxisChanged(object sender, EventArgs e)
    {
        Pages_UserControl_ControlByColumn cbcColour = sender as Pages_UserControl_ControlByColumn;

        if (cbcColour != null)
        {
            GridViewRow row = cbcColour.NamingContainer as GridViewRow;
            Label lblID = row.FindControl("lblID") as Label;
            ImageButton imgbtnMinus = row.FindControl("imgbtnMinus") as ImageButton;
            ImageButton imgbtnPlus = row.FindControl("imgbtnPlus") as ImageButton;
            DropDownList ddlTextColour = row.FindControl("ddlTextColour") as DropDownList;

            if(cbcColour.ddlYAxisV=="")
            {
                imgbtnPlus.Visible = false;
            }
            else
            {
                imgbtnPlus.Visible = true;
            }           
        }


    }

    protected void grdCalendarColor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblID = e.Row.FindControl("lblID") as Label;
                ImageButton imgbtnMinus = e.Row.FindControl("imgbtnMinus") as ImageButton;
                ImageButton imgbtnPlus = e.Row.FindControl("imgbtnPlus") as ImageButton;
                DropDownList ddlTextColour = e.Row.FindControl("ddlTextColour") as DropDownList;
                //imgbtnPlus.CommandArgument = e.Row.RowIndex.ToString();

                Pages_UserControl_ControlByColumn cbcColour = e.Row.FindControl("cbcColour") as Pages_UserControl_ControlByColumn;
                cbcColour.TableID = _iTableID;
                cbcColour.ddlYAxisV = DataBinder.Eval(e.Row.DataItem, "columnid").ToString();
                cbcColour.TextValue = DataBinder.Eval(e.Row.DataItem, "value").ToString();

                if (DataBinder.Eval(e.Row.DataItem, "colour").ToString() != "")
                    ddlTextColour.SelectedValue = DataBinder.Eval(e.Row.DataItem, "colour").ToString();

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

  
    protected void SetCalendarColorRowData()
    {
        
        if(ViewState["dtCalenderdarColor"]!=null)
        {
            DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];
            dtCalenderdarColor.Rows.Clear();

            int iID = 0;
            if(grdCalendarColor.Rows.Count>0)
            {
                foreach(GridViewRow gvRow in grdCalendarColor.Rows)
                {

                    //gvRow.FindControl()
                    Label lblID = gvRow.FindControl("lblID") as Label;
                    DropDownList ddlTextColour = gvRow.FindControl("ddlTextColour") as DropDownList;
                    Pages_UserControl_ControlByColumn cbcColour = gvRow.FindControl("cbcColour") as Pages_UserControl_ControlByColumn;
                    //dtCalenderdarColor.Rows.Add(iID, cbcColour.ddlYAxisV, cbcColour.TextValue, ddlTextColour.SelectedValue);
                    dtCalenderdarColor.Rows.Add(lblID.Text.ToString(), cbcColour.ddlYAxisV, cbcColour.TextValue, ddlTextColour.SelectedValue);
                    iID = iID + 1;
                }

            }
            dtCalenderdarColor.AcceptChanges();
            ViewState["dtCalenderdarColor"] = dtCalenderdarColor;
            

        }
    }
    protected void PopulateCalendarColor()
    {
        if (ViewState["TextColourInfo"] != null)
        {
            string strXML = ViewState["TextColourInfo"].ToString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXML);

            XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

            DataSet ds = new DataSet();
            ds.ReadXml(r);


            if (ds.Tables[0] != null)
            {          
                    
                ds.Tables[0].Columns.Add("ID");

                ds.Tables[0].AcceptChanges();

                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    dr["ID"] = Guid.NewGuid().ToString();
                }
                ds.Tables[0].AcceptChanges();
                
                grdCalendarColor.DataSource = ds.Tables[0];
                grdCalendarColor.DataBind();

            }
        }
        else
        {
            if(ViewState["dtCalenderdarColor"]==null)
            {
                DataTable dtCalenderdarColor = new DataTable();
                dtCalenderdarColor.Columns.Add("ID");
                //DataColumn dc = dtCalenderdarColor.Columns.Add("ID", typeof(int));
                //dc.AutoIncrement = true;
                //dc.AutoIncrementSeed = 1;
                //dc.AutoIncrementStep = 1;


                dtCalenderdarColor.Columns.Add("columnid");
                dtCalenderdarColor.Columns.Add("value");
                dtCalenderdarColor.Columns.Add("colour");
                dtCalenderdarColor.AcceptChanges();

                //dtCalenderdarColor.Rows.Add(0,"", "","");

                dtCalenderdarColor.Rows.Add(Guid.NewGuid().ToString(), "", "", "");

                grdCalendarColor.DataSource = dtCalenderdarColor;
                grdCalendarColor.DataBind();

                ViewState["dtCalenderdarColor"] = dtCalenderdarColor;
            }
            else
            {
                DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];
                grdCalendarColor.DataSource = dtCalenderdarColor;
                grdCalendarColor.DataBind();
            }
            

        }
    }

   
    protected void grdCalendarColor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            SetCalendarColorRowData();
           
            if (e.CommandName == "minus")
            {
                 if( ViewState["dtCalenderdarColor"]!=null)
                 {
                     DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];

                     for (int i = dtCalenderdarColor.Rows.Count - 1; i >= 0; i--)
                     {
                         DataRow dr = dtCalenderdarColor.Rows[i];
                         if (dr["id"].ToString() == e.CommandArgument.ToString())
                         {
                             dr.Delete();
                             break;
                         }
                            
                     }


                     dtCalenderdarColor.AcceptChanges();

                     ViewState["dtCalenderdarColor"] = dtCalenderdarColor;

                     PopulateCalendarColor();
                     //PopulatePreviousColor();


                 }
            }
            if (e.CommandName == "plus")
            {
               if( ViewState["dtCalenderdarColor"]!=null)
               {
                   DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];

                   //dtCalenderdarColor.Rows.Add(int.Parse(e.CommandArgument.ToString())+1,  "", "","");

                   //dtCalenderdarColor.Rows.Add(Guid.NewGuid().ToString(),"", "", "");
                   int iPos = 0;

                    for (int i = 0; i < dtCalenderdarColor.Rows.Count; i++)
			            {
			                 if(dtCalenderdarColor.Rows[i][0].ToString()==e.CommandArgument.ToString())
                             {
                                 iPos = i;
                                 break;
                             }            
			            }
                   //for (int i = 0 dtCalenderdarColor.Rows.Count - 1; i >= 0; i--)
                   //{
                   //    DataRow dr = dtCalenderdarColor.Rows[i];
                   //    if (dr["id"].ToString() == e.CommandArgument.ToString())
                   //    {
                   //        iPos = i;
                   //        break;
                   //    }

                   //}


                   DataRow newRow = dtCalenderdarColor.NewRow();
                   newRow[0] = Guid.NewGuid().ToString();
                   newRow[1] = "";
                   newRow[2] = "";
                   newRow[3] = "";


                   dtCalenderdarColor.Rows.InsertAt(newRow, iPos+1);

                   dtCalenderdarColor.AcceptChanges();

                   ViewState["dtCalenderdarColor"] = dtCalenderdarColor;

                   PopulateCalendarColor();
                   //PopulatePreviousColor();

                   //grdCalendarColor.Rows.
               }

            }
        }
        catch
        {
            //
        }
    }




    protected void SaveButton_Click(object sender, EventArgs e)
    {

    }

    protected void PopulatePreviousColor()
    {
        if (ViewState["dtCalenderdarColor"] != null)
        {
            DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];

            int iID = 0;
            foreach (GridViewRow gvRow in grdCalendarColor.Rows)
            {
                DropDownList ddlTextColour = gvRow.FindControl("ddlTextColour") as DropDownList;
                Pages_UserControl_ControlByColumn cbcColour = gvRow.FindControl("cbcColour") as Pages_UserControl_ControlByColumn;
                if (dtCalenderdarColor.Rows[iID]["colour"].ToString() != "")
                    ddlTextColour.SelectedValue = dtCalenderdarColor.Rows[iID]["colour"].ToString();

                cbcColour.TableID = _iTableID;
                cbcColour.ddlYAxisV = dtCalenderdarColor.Rows[iID]["columnid"].ToString();
                cbcColour.TextValue = dtCalenderdarColor.Rows[iID]["value"].ToString();



                if ((iID + 2) > dtCalenderdarColor.Rows.Count)
                {
                    break;
                }
                iID = iID + 1;
            }
        }
    }
}