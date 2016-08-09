using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_DocGen_EachRecordTable : SecurePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        double iSec = 3;
        string strTopMsgNoOfMS = "3000";
        string strTopMessageDisplayNumberSeconds = SystemData.SystemOption_ValueByKey_Account("Top Message Display Number Seconds", int.Parse(Session["AccountID"].ToString()), null);
        if (strTopMessageDisplayNumberSeconds != "")
        {
            double dTemp = 0;
            if (double.TryParse(strTopMessageDisplayNumberSeconds, out dTemp))
            {
                if (dTemp > 300)
                    dTemp = 300;

                iSec = dTemp;
                dTemp = dTemp * 1000;
                strTopMsgNoOfMS = dTemp.ToString("N0").Replace(",", "");

            }

        }



        ltTextStyles.Text = @"<style  type='text/css'>
                            .cssanimations.csstransforms #divNotificationMessage {
                    -webkit-transform: translateY(-50px);
                    -webkit-animation: slideDown " + iSec.ToString() + @"s 0.2s 1 ease forwards;
                    -moz-transform:    translateY(-50px);
                    -moz-animation:    slideDown " + iSec.ToString() + @"s 0.2s 1 ease forwards;
                }
 </style>
            ";


        string strHidedivNotificationMessage = @"                                                     
                                                  $(document).ready(function () {

                                                      try
                                                        {
                                                            window.setTimeout(HidedivNotificationMessage," + strTopMsgNoOfMS + @");
                                                        }
                                                      catch(err)
                                                        {
                                                            //
                                                        }
     
                                                    });
                                                ";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "strHidedivNotificationMessage", strHidedivNotificationMessage, true);




        if(Request.QueryString["DocumentSectionID"]!=null)
        {
            string strDocumentSectionID = Cryptography.Decrypt(Request.QueryString["DocumentSectionID"].ToString());

            //if(!IsPostBack)
            //{
            //    string strDetail = Common.GetValueFromSQL("SELECT Details FROM DocumentSection WHERE DocumentSectionID=" + strDocumentSectionID);
            //    bool bUpdateDetail = false;
            //        if(strDetail!="")
            //        {
            //             DocGen.DAL.RecordTableSectionDetail rtDetail = DocGen.DAL.JSONField.GetTypedObject<DocGen.DAL.RecordTableSectionDetail>(strDetail);
            //             if (rtDetail != null)
            //            {
            //                if (rtDetail.TableID != null && rtDetail.ViewID!=null)
            //                {
                                
            //                  //test if view is ok
            //                    try
            //                    {
            //                        View theView=ViewManager.dbg_View_Detail(rtDetail.ViewID,null,null);
            //                        rtDetail.ViewID =(int) theView.ViewID;

            //                        strDetail = rtDetail.GetJSONString();
            //                    }
            //                    catch
            //                    {
            //                        //
                                   
            //                        try
            //                        {
                                       

            //                            // Create dashboard view
            //                            Table _theTable = RecordManager.ets_Table_Details((int)rtDetail.TableID);
            //                            View newView = new View(null, (int)_theTable.TableID, _theTable.TableName, (int)((User)Session["User"]).UserID, 10, "", "",
            //                                   null, null, null, null, null, null);
            //                            newView.ViewPageType = "dash";
            //                            int iNewViewID = ViewManager.dbg_View_Insert(newView, null, null);
            //                            rtDetail.ViewID = iNewViewID;

            //                            strDetail= rtDetail.GetJSONString();
            //                            ViewManager.dbg_CreateDefaultViewItem(iNewViewID);
            //                            bUpdateDetail = true;
            //                        }
            //                        catch
            //                        {

            //                            //Common.ExecuteText("DELETE FROM DocumentSection WHERE DocumentSectionID =" + strDocumentSectionID);
            //                        }
                                   
            //                    }
                                
            //                    if(bUpdateDetail)
            //                    {
            //                        Common.ExecuteText("UPDATE DocumentSection SET Details='" + strDetail.Replace("'","''")+ "'  WHERE DocumentSectionID=" + strDocumentSectionID);
            //                    }
                                
            //                }

            //            }
            //        }
               

            //}
            string strWidth = "1";
            if (Request.QueryString["width"] != null)
            {
                strWidth = "2";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "DocumentSectionID" + strDocumentSectionID, "setTimeout(function () { parent.autoResizeRecord('iframe" + strDocumentSectionID + "'," + strWidth + ");}, 1000);", true);
        }


        //if (!IsPostBack)
        //{
        //    if (Session["tdbmsg"] != null)
        //    {
        //        lblNotificationMessage.Text = Session["tdbmsg"].ToString();
        //        Session["tdbmsg"] = null;

        //        if (lblNotificationMessage.Text != "")
        //        {
        //            lblNotificationMessage.Text = lblNotificationMessage.Text + "&nbsp; <a id='aNotificationMessageClose' href='#'>Close</a>";
        //        }
        //    }
        //    else
        //    {
        //        lblNotificationMessage.Text = "";
        //    }

        //}
        //else
        //{
        //    Session["tdbmsg"] = null;
        //}

    }



    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (Session["tdbmsgpb"] != null)
            {
                lblNotificationMessage.Text = Session["tdbmsgpb"].ToString();
                Session["tdbmsgpb"] = null;

                if (lblNotificationMessage.Text != "")
                {
                    lblNotificationMessage.Text = lblNotificationMessage.Text + "&nbsp; <a id=\"aNotificationMessageClose\" onclick=\"document.getElementById('divNotificationMessage').style.display = 'none';return false;\" href=\"#\" >Close</a>";
                }
            }
            else
            {
                lblNotificationMessage.Text = "";
            }
        }



    }


    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    return;
    //}


}