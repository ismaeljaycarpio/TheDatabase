using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Pages_Record_DeleteRecord : SecurePage
{
    //string _strRecordRightID = Common.UserRoleType.None;
    //bool _bDeleteReason = false;
    string _strDynamictabPart = "";
    //bool _bGodAH = false;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if(!IsPostBack)
        {
            lnkDeleteAllOK.Attributes.Add("href", "#");
            string strParentObject = "";
            if (Request.QueryString["sc_id"] != null)
            {
                SearchCriteria theSC = SystemData.SearchCriteria_Detail(int.Parse(Request.QueryString["sc_id"].ToString()));
                if (theSC != null)
                {
                    System.Xml.XmlDocument xmlSC_Doc = new System.Xml.XmlDocument();

                    xmlSC_Doc.Load(new StringReader(theSC.SearchText));

                    strParentObject =strParentObject+ " var p_lnkDeleteAllOK = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["lnkDeleteAllOK"].InnerText + @"');";
                    strParentObject = strParentObject + " var p_chkAll = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["chkAll"].InnerText + @"');";

                    ViewState["TableName"] = xmlSC_Doc.FirstChild["TableName"].InnerText;
                     ViewState["_bDeleteReason"] = bool.Parse(xmlSC_Doc.FirstChild["DeleteReason"].InnerText);
                     ViewState["_strRecordRightID"] = xmlSC_Doc.FirstChild["RecordRightID"].InnerText;
                     _strDynamictabPart = xmlSC_Doc.FirstChild["DynamictabPart"].InnerText;
                     ViewState["_strDynamictabPart"] = _strDynamictabPart;
                     strParentObject = strParentObject + " var p_hfParmanentDelete = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["hfParmanentDelete"].InnerText + @"');";
                     strParentObject = strParentObject + " var p_hfchkDeleteParmanent = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["hfchkDeleteParmanent"].InnerText + @"');";
                     strParentObject = strParentObject + " var p_hfchkUndo = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["hfchkUndo"].InnerText + @"');";
                     strParentObject = strParentObject + " var p_hfchkDelateAllEvery = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["hfchkDelateAllEvery"].InnerText + @"');";
                     strParentObject = strParentObject + " var p_hftxtDeleteReason = window.parent.document.getElementById('" + xmlSC_Doc.FirstChild["hftxtDeleteReason"].InnerText + @"');";
                }
            }


            if (ViewState["_strRecordRightID"].ToString() == Common.UserRoleType.Administrator
                      || ViewState["_strRecordRightID"].ToString() == Common.UserRoleType.GOD)
            {
            }
            else
            {
                trDeleteParmanent.Visible = false;
                trUndo.Visible = false;
            }




            if (bool.Parse(ViewState["_bDeleteReason"].ToString()))
            {
                trDeleteRestoreMessage.Visible = false;
                if(Request.QueryString["type"].ToString()=="d")
                {
                    trDeleteReason.Visible = true;
                }              
            }

            txtDeleteReason.Text = "";




            string strSaveAndRefresh = "";



            string strConfirm = "";

            switch (Request.QueryString["type"].ToString().Trim())
            {
                case "d":
                    lblTitle.Text = ViewState["TableName"] + " - Delete Records";
                     chkDelateAllEvery.Checked = false;
                    lblDeleteRestoreMessage.Text = "Are you sure you want to delete selected item(s)?";
                    chkDelateAllEvery.Text = "I would like to delete EVERY item in this table";
                    hfParmanentDelete.Value = "no";
                    lblDeleteMessageNote.Visible = true;

                    chkDelateAllEvery.Checked = false;
                    chkDeleteParmanent.Checked = false;
                    chkUndo.Checked = false;
                   
                    if (bool.Parse(ViewState["_bDeleteReason"].ToString()))
                    {
                        strConfirm = @"
                             if(document.getElementById('" + txtDeleteReason.ClientID + @"').value=='')
                                {
                                    CommonAlertMessage('Please enter delete reason.',2000);
                                    return false;
                                }
                            ";
                    }
                    strSaveAndRefresh = @"
                            function DeleteRecordsFromATable()
                                {
                                    " + strConfirm + @"

                                    p_hfchkDeleteParmanent.value='false';  p_hfParmanentDelete.value='no';
                                    p_hftxtDeleteReason.value=document.getElementById('" + txtDeleteReason.ClientID + @"').value;
                                    p_hfchkDelateAllEvery.value=document.getElementById('" + chkDelateAllEvery.ClientID + @"').checked.toString();
                                    $(p_lnkDeleteAllOK).trigger('click');parent.$.fancybox.close();
                                }

                                ";

                    //lnkDeleteAllOK.OnClientClick = strSaveAndRefresh;
                    break;
                case "p":
                    lblTitle.Text = ViewState["TableName"] + " - Parmanent Delete Records";

                     trDeleteAllEvery.Visible = true;
                    chkDelateAllEvery.Checked = false;

                    lblDeleteRestoreMessage.Text = "Are you sure you want to PERMANENTLY delete the selected item(s)?";
                    chkDelateAllEvery.Text = "I would like to PERMANENTLY delete EVERY item in this table";

                    hfParmanentDelete.Value = "yes";

                    chkDelateAllEvery.Checked = false;
                    chkDeleteParmanent.Checked = false;
                    chkUndo.Checked = false;
                    trDeleteReason.Visible = false;

                    trDeleteParmanent.Visible = false;
                    trUndo.Visible = true;
                     strConfirm="";
                    
                        strConfirm = @"
                             if(document.getElementById('" + chkUndo.ClientID + @"').checked==false)
                                {
                                    CommonAlertMessage('I will not be able to undo this action checkbox must be ticked to delete PERMANENTLY.',4000);
                                    return false;
                                }
                            ";
                    
                    strSaveAndRefresh = @"
                            function DeleteRecordsFromATable()
                                {
                                         " + strConfirm + @"
                                    p_hfParmanentDelete.value='yes';
                                    p_hfchkDeleteParmanent.value='true';
                                    p_hfchkUndo.value=document.getElementById('" + chkUndo.ClientID + @"').checked.toString();
                                    p_hfchkDelateAllEvery.value=document.getElementById('" + chkDelateAllEvery.ClientID + @"').checked.toString();
                                    $(p_lnkDeleteAllOK).trigger('click');parent.$.fancybox.close();
                                }

                                ";


                    //rfvDeleteReason.Enabled = false;
                    break;
                case "r":
                    chkDelateAllEvery.Checked = false;
                    lblTitle.Text = ViewState["TableName"] + " - Restore Records";
                    lblDeleteRestoreMessage.Text = "Are you sure you want to restore selected item(s)?";
                    chkDelateAllEvery.Text = "I would like to restore EVERY item in this table";
                    lblDeleteMessageNote.Visible = false;

                    hfParmanentDelete.Value = "res";


                    chkDelateAllEvery.Checked = false;
                    chkDeleteParmanent.Checked = false;
                    chkUndo.Checked = false;

                    trDeleteParmanent.Visible = false;
                    trUndo.Visible = false;
                                trDeleteRestoreMessage.Visible = true;
                    trDeleteReason.Visible = false;
                    strSaveAndRefresh = @"
                            function DeleteRecordsFromATable()
                                {
                                    p_hfchkDeleteParmanent.value='false';
                                      p_hfParmanentDelete.value='no';
                                    p_hfchkDelateAllEvery.value=document.getElementById('" + chkDelateAllEvery.ClientID + @"').checked.toString();
                                    $(p_lnkDeleteAllOK).trigger('click');parent.$.fancybox.close();
                                }

                                ";



                    //rfvDeleteReason.Enabled = false;
                    break;
                default:
                    break;
            }

            
            string strXX = @"
                       

                         $(document).ready(function () {


  
                                "+strParentObject+ @"


                                if(p_chkAll!=null )
                                    {
                                        if(p_chkAll.checked)
                                            {
                                                $('#" + trDeleteAllEvery.ClientID + @"').fadeIn();
                                            }
                                        else
                                            {
                                                $('#" + trDeleteAllEvery.ClientID + @"').fadeOut();
                                            }
                                    }


                                 


                                    if( document.getElementById('" + hfParmanentDelete.ClientID + @"').value=='yes')
                                         {
                                            $('#" + trUndo.ClientID + @"').fadeIn();
                                        }
                                        else
                                        {
                                             $('#" + trUndo.ClientID + @"').fadeOut();
                                        }



                                        
                                    " + strSaveAndRefresh + @"

                                       $('#" + lnkDeleteAllOK.ClientID + @"').click(function () {

                                                    DeleteRecordsFromATable();return false;
                                    });
                                });

                ";

            ScriptManager.RegisterStartupScript(upMain, upMain.GetType(), "DelJSCode" + _strDynamictabPart, strXX, true);
        }

       


            //delete related

            //TheDatabase.SetValidationGroup(pnlDeleteAll.Controls, "DE" + _strDynamictabPart);
        
    }
    protected void Pager_UnDeleteAction(object sender, EventArgs e)
    {
        //EnsureSecurity();
        //bool bIsAllCheckeD = false;

        //bool bHeaderChecked = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;
        //string sCheck = "";
        //if (bHeaderChecked)
        //{
        //    bIsAllCheckeD = true;
        //    for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        //    {
        //        bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
        //        if (ischeck)
        //        {
        //            sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
        //        }
        //        else
        //        {
        //            bIsAllCheckeD = false;
        //        }
        //    }

        //}
        //else
        //{
        //    for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        //    {
        //        bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
        //        if (ischeck)
        //        {
        //            sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
        //        }
        //    }

        //}

        //if (bIsAllCheckeD)
        //{
        //    trDeleteAllEvery.Visible = true;
        //}
        //else
        //{
        //    trDeleteAllEvery.Visible = false;
        //}

        //if (_gvPager != null)
        //{
        //    _gvPager.HideDeleteURL = true;
        //    _gvPager.HideUnDelete = false;
        //    if (_strRecordRightID == Common.UserRoleType.Administrator
        //               || _strRecordRightID == Common.UserRoleType.GOD)
        //    {
        //        if (chkIsActive.Checked)
        //        {
        //            _gvPager.HideParmanentDelete = false;
        //        }
        //    }
        //    ShowHidePermanentDelete();
        //}

        //if (string.IsNullOrEmpty(sCheck))
        //{
        //    ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
        //    return;
        //}


        //chkDelateAllEvery.Checked = false;

        //lblDeleteRestoreMessage.Text = "Are you sure you want to restore selected item(s)?";
        //chkDelateAllEvery.Text = "I would like to restore EVERY item in this table";
        //lblDeleteMessageNote.Visible = false;

        //hfParmanentDelete.Value = "no";


        //chkDelateAllEvery.Checked = false;
        //chkDeleteParmanent.Checked = false;
        //chkUndo.Checked = false;

        //trDeleteParmanent.Visible = false;
        //trUndo.Visible = false;

        //if (_strRecordRightID == Common.UserRoleType.Administrator
        //               || _strRecordRightID == Common.UserRoleType.GOD)
        //{
        //}
        //else
        //{
        //    trDeleteParmanent.Visible = false;
        //    trUndo.Visible = false;
        //}


        //trDeleteRestoreMessage.Visible = true;
        //trDeleteReason.Visible = false;

        //mpeDeleteAll.Show();


    }




    protected void Pager_OnParmanenetDelAction(object sender, EventArgs e)
    {

        //bool bIsAllCheckeD = false;

        //bool bHeaderChecked = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;
        //string sCheck = "";
        //if (bHeaderChecked)
        //{
        //    //Ticket 1013
        //    trUndo.Style.Add("display", "table-row");
        //    //End

        //    bIsAllCheckeD = true;
        //    for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        //    {
        //        bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
        //        if (ischeck)
        //        {
        //            sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
        //        }
        //        else
        //        {
        //            bIsAllCheckeD = false;
        //        }
        //    }

        //}
        //else
        //{
        //    for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        //    {
        //        bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
        //        if (ischeck)
        //        {
        //            sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
        //        }
        //    }
        //}



        //if (_gvPager != null)
        //{
        //    _gvPager.HideDeleteURL = true;
        //    _gvPager.HideUnDelete = false;
        //    _gvPager.HideParmanentDelete = false;

        //    ShowHidePermanentDelete();
        //}

        //if (string.IsNullOrEmpty(sCheck))
        //{
        //    ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
        //    return;
        //}

        //trDeleteAllEvery.Visible = true;
        //chkDelateAllEvery.Checked = false;

        //lblDeleteRestoreMessage.Text = "Are you sure you want to PERMANENTLY delete the selected item(s)?";
        //chkDelateAllEvery.Text = "I would like to PERMANENTLY delete EVERY item in this table";

        //hfParmanentDelete.Value = "yes";

        //chkDelateAllEvery.Checked = false;
        //chkDeleteParmanent.Checked = false;
        //chkUndo.Checked = false;


        //trDeleteParmanent.Visible = false;
        //trUndo.Visible = true;
        

        //if (_strRecordRightID == Common.UserRoleType.Administrator
        //               || _strRecordRightID == Common.UserRoleType.GOD)
        //{
        //}
        //else
        //{
        //    trDeleteParmanent.Visible = false;
        //    trUndo.Visible = false;
        //}

        //mpeDeleteAll.Show();

    }

    protected void chkDeleteParmanent_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkDeleteParmanent.Checked)
        {
            trUndo.Visible = false;
        }
        else
        {
            trUndo.Visible = true;
        }

        //mpeDeleteAll.Show();
    }
    protected void Pager_DeleteAction(object sender, EventArgs e)
    {
        //Ticket 1013
        //trUndo.Style.Add("display", "none");
        //End

        //EnsureSecurity();
        //bool bIsAllCheckeD = false;

        //bool bHeaderChecked = ((CheckBox)gvTheGrid.HeaderRow.FindControl("chkAll")).Checked;
        //string sCheck = "";
        //if (bHeaderChecked)
        //{
        //    bIsAllCheckeD = true;
        //    for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        //    {
        //        bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
        //        if (ischeck)
        //        {
        //            sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
        //        }
        //        else
        //        {
        //            bIsAllCheckeD = false;
        //        }
        //    }

        //}
        //else
        //{
        //    for (int i = 0; i < gvTheGrid.Rows.Count; i++)
        //    {
        //        bool ischeck = ((CheckBox)gvTheGrid.Rows[i].FindControl("chkDelete")).Checked;
        //        if (ischeck)
        //        {
        //            sCheck = sCheck + ((Label)gvTheGrid.Rows[i].FindControl("LblID")).Text + ",";
        //        }
        //    }

        //}



        //if (string.IsNullOrEmpty(sCheck))
        //{
        //    Session["tdbmsgpb"] = "Please select a record.";
        //    if (hfUsingScrol.Value == "yes" && _gvPager != null)
        //    {
        //        BindTheGrid(_gvPager.StartIndex, gvTheGrid.PageSize);
        //    }
        //    //ScriptManager.RegisterClientScriptBlock(gvTheGrid, typeof(Page), "message_alert", "alert('Please select a record.');", true);
        //    return;
        //}


        //chkDelateAllEvery.Checked = false;
        //lblDeleteRestoreMessage.Text = "Are you sure you want to delete selected item(s)?";
        //chkDelateAllEvery.Text = "I would like to delete EVERY item in this table";
        //hfParmanentDelete.Value = "no";
        //lblDeleteMessageNote.Visible = true;

        //chkDelateAllEvery.Checked = false;
        //chkDeleteParmanent.Checked = false;
        //chkUndo.Checked = false;


        //if (bIsAllCheckeD)
        //{
        //    trDeleteAllEvery.Visible = true;
        //}
        //else
        //{
        //    trDeleteAllEvery.Visible = false;
        //}

        //trDeleteParmanent.Visible = true;
        //trUndo.Visible = true;

        //if (_strRecordRightID == Common.UserRoleType.Administrator
        //               || _strRecordRightID == Common.UserRoleType.GOD)
        //{
        //}
        //else
        //{
        //    trDeleteParmanent.Visible = false;
        //    trUndo.Visible = false;
        //}




        //if (_bDeleteReason)
        //{
        //    trDeleteRestoreMessage.Visible = false;
        //    trDeleteReason.Visible = true;
        //}

        //txtDeleteReason.Text = "";
       

    }




}







//        if (PageType == "c")
//        {
//            strXX = @"
//
//
//
//                         $(document).ready(function () {
//
//
//                                  
//
//                                $('#" + chkDeleteParmanent.ClientID + @"').click(function () {
//                                        var chk = document.getElementById('" + chkDeleteParmanent.ClientID + @"');
//                                        if (chk.checked == true) {
//                                            $('#" + trUndo.ClientID + @"').fadeIn();
//
//                                        }
//                                        else {
//                                            $('#" + trUndo.ClientID + @"').fadeOut();
//                                             var chkUndo = document.getElementById('" + chkUndo.ClientID + @"');
//                                            chkUndo.checked=false;
//                                        }
//                                    });
//                         if( document.getElementById('" + hfParmanentDelete.ClientID + @"').value=='yes' && document.getElementById('" + chkDelateAllEvery.ClientID + @"')==null)
//                                         {
//                                            $('#" + trUndo.ClientID + @"').fadeIn();
//                                            }
//                        $('#" + chkDelateAllEvery.ClientID + @"').click(function () {
//                                            if( document.getElementById('" + hfParmanentDelete.ClientID + @"').value=='yes')
//                                            {
//                                                    var chk = document.getElementById('" + chkDelateAllEvery.ClientID + @"');
//                                                    if (chk.checked == true) {
//                                                        $('#" + trUndo.ClientID + @"').fadeIn();
//
//                                                    }
//                                            }
//
//                                    });
//                               
//
//                                });
//
//                        ";
//        }


//$('#" + chkDeleteParmanent.ClientID + @"').click(function () {
//                                        var chk = document.getElementById('" + chkDeleteParmanent.ClientID + @"');
//                                        if (chk.checked == true) {
//                                            $('#" + trUndo.ClientID + @"').fadeIn();
//                                        }
//                                        else {
//                                            $('#" + trUndo.ClientID + @"').fadeOut();
//                                             var chkUndo = document.getElementById('" + chkUndo.ClientID + @"');
//                                            chkUndo.checked=false;
//                                        }
//                                    });
//                                     $('#" + chkDelateAllEvery.ClientID + @"').click(function () {
//                                            if( document.getElementById('" + hfParmanentDelete.ClientID + @"').value=='yes')
//                                            {
//                                                    var chkDelateAllEvery = document.getElementById('" + chkDelateAllEvery.ClientID + @"');
//                                                    if (chkDelateAllEvery.checked == true) {
//                                                        $('#" + trUndo.ClientID + @"').fadeIn();
//                                                    }
//                                                   else
//                                                    {
//                                                            $('#" + trUndo.ClientID + @"').fadeOut();
//                                                            var chkUndo2 = document.getElementById('" + chkUndo.ClientID + @"');       
//                                                            if (chkUndo2!=null)
//                                                                {
//                                                                    chkUndo2.checked=false;
//                                                            }                                                                                                                 
//                                                    }
//                                            }
//                                    });

