using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_Record_TableTabList : SecurePage
{

    string _strActionMode = "view";
    string _qsMode = "";
    int? _iTableID = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        int iTemp = 0;

        _iTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));

        string strChildTables = @"
                    $(function () {
                            $('.popuplink').fancybox({
                                scrolling: 'auto',
                                type: 'iframe',
                                'transitionIn': 'elastic',
                                'transitionOut': 'none',
                                width: 1100,
                                height: 750,
                                titleShow: false
                            });
                        });

                ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "TableTab", strChildTables, true);


        if (!IsPostBack)
        {
            hlAddTableTab.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableTabDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(_iTableID.ToString());
            PopulateTabs();
        }
        



    }

     
   

    protected void grdTableTabt_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetype")
        {
            try
            {

                string strTabName = Common.GetValueFromSQL("SELECT TabName FROM [TableTab] WHERE TableTabID=" + e.CommandArgument.ToString());


                string strNoOfTabs = Common.GetValueFromSQL("SELECT Count(*) FROM TableTab WHERE TableID=" + _iTableID.ToString());

                int iNoOfTabs = int.Parse(strNoOfTabs);

                if (iNoOfTabs > 1)
                {
                    string strHasFields = Common.GetValueFromSQL("SELECT TOP 1 ColumnID FROM [Column] WHERE TableTabID=" + e.CommandArgument.ToString());

                    if (strHasFields!="")
                    {

                        Session["tdbmsgpb"] = "Page " + strTabName + " has fields, please move those fields to another page and try again.";
                       
                    }
                    else
                    {
                        RecordManager.dbg_TableTab_Delete(int.Parse(e.CommandArgument.ToString()));
                        Session["tdbmsgpb"] = "Page " + strTabName + " has been deleted.";
                    }

                   
                    
                }
                else
                {
                    Session["tdbmsgpb"] = "You can not delete the primary tab.";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "TableTabdEL", "alert('You can not delete the primary tab.');", true);

                }

                PopulateTabs();
            }
            catch (Exception ex)
            {
                if(ex.Message.IndexOf("DELETE")>-1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "TableTabdEL", "alert('This tab has fields, please edit fields and try again.');", true);
                }
            }
        }
    }

    protected void grdTableTab_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            HyperLink hlAddDetail = e.Row.FindControl("hlAddDetail") as HyperLink;
            if (hlAddDetail != null)
            {
                hlAddDetail.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableTabDetail.aspx?mode=" + Cryptography.Encrypt("add") + "&TableID=" + Cryptography.Encrypt(_iTableID.ToString());
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");



            HyperLink hlEditDetail = e.Row.FindControl("hlEditDetail") as HyperLink;

            if (hlEditDetail != null)
            {
                hlEditDetail.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/TableTabDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&TableID=" + Cryptography.Encrypt(_iTableID.ToString()) + "&TableTabID=" + Cryptography.Encrypt(DataBinder.Eval(e.Row.DataItem, "TableTabID").ToString());
            }


            TableTab theTableTab = RecordManager.ets_TableTab_Detail(int.Parse(DataBinder.Eval(e.Row.DataItem, "TableTabID").ToString()));
            ImageButton imgbtnDelete = e.Row.FindControl("imgbtnDelete") as ImageButton;
            if (imgbtnDelete != null)
            {
                imgbtnDelete.OnClientClick = "Javascript:return confirm('Are you sure you want to delete " + theTableTab.TabName + " page?');";
            }
            if (theTableTab != null && theTableTab.DisplayOrder == 0)
            {

               
                if (imgbtnDelete != null)
                {
                    imgbtnDelete.Visible = false;
                }
                    
            }

        }



    }

    protected void PopulateTabs()
    {
        DataTable dtPages = Common.DataTableFromText(@"SELECT TableTabID,TabName
            FROM TableTab TT WHERE  TT.TableID=" + _iTableID.ToString() + " ORDER BY TT.DisplayOrder");

        grdTableTab.DataSource = dtPages;
        grdTableTab.DataBind();

        if (dtPages.Rows.Count == 0)
        {
            divTableTab.Visible = true;
        }
        else
        {
            divTableTab.Visible = false;
        }

    }


    protected void btnOrderFF_Click(object sender, EventArgs e)
    {
        //
        if (hfOrderFF.Value != "")
        {
            
            try
            {
                string strNewFF = hfOrderFF.Value.Substring(0, hfOrderFF.Value.Length - 1);
                string[] newFF = strNewFF.Split(',');

                //string strFilter = "";

                //if (chkShowSystemFields.Checked == false)
                //    strFilter = " IsStandard=0 AND ";

                DataTable dtDO = Common.DataTableFromText("SELECT DisplayOrder,TableTabID FROM [TableTab] WHERE TableTabID IN (" + strNewFF + ") AND DisplayOrder<>0 ORDER BY DisplayOrder");
                if (newFF.Length == dtDO.Rows.Count+1)
                {
                    for (int i = 1; i < newFF.Length; i++)
                    {

                        TableTab theTableTab = RecordManager.ets_TableTab_Detail(int.Parse(newFF[i]));

                        if (theTableTab.DisplayOrder != 0)
                        {
                            //Common.ExecuteText("UPDATE TableTab SET DisplayOrder =" + dtDO.Rows[i-1][0].ToString() + " WHERE TableTabID=" + newFF[i], tn);

                            Common.ExecuteText("UPDATE TableTab SET DisplayOrder =" + i.ToString() + " WHERE TableTabID=" + newFF[i]);
                        }

                    }
                }


            }
            catch (Exception ex)
            {

                //

            }
            PopulateTabs();
        }
    }

    protected void btnRefreshPages_Click(object sender, EventArgs e)
    {
        PopulateTabs();
    }
    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
 
   
}
