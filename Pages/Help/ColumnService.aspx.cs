using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;
using System.Data;
public partial class Pages_Help_ColumnService : System.Web.UI.Page
{
    string _strDynamictabPart = "ctl00_HomeContentPlaceHolder_";
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {

            MakeTitle();

            PopulateControlDDL();
            PopulateRecord();
        }
    }

    protected void MakeTitle()
    {
        try
        {
            if (Request.QueryString["ID"] != null)
            {
                Column theColumn = RecordManager.ets_Column_Details(int.Parse(Request.QueryString["ID"].ToString()));
                if(theColumn!=null)
                {
                    lblTitle.Text = theColumn.DisplayName + " Action";
                }
            }
        }
        catch
        {
            //
        }
       
    }

    protected void PopulateControlDDL()
    {
        if(Request.QueryString["TableID"]!=null)
        {
            int iTableID = int.Parse(Request.QueryString["TableID"].ToString());
            Table theTable = RecordManager.ets_Table_Details(iTableID);
            DataTable _dtColumnsDetail = RecordManager.ets_Table_Columns_Detail(iTableID);

            ddlConrolIDnText.Items.Add(new ListItem("", ""));

            ddlConrolIDnText.Items.Add(new ListItem("Save button", "btnSaveRecord"));
            ddlConrolIDnText.Items.Add(new ListItem("User Role Name", "hfUserRoleName"));
            ddlConrolIDnText.Items.Add(new ListItem("Add/Edit/View", "hfRecordAddEditView"));
            ddlConrolIDnText.Items.Add(new ListItem("TableID", "hfRecordTableID"));
            ddlConrolIDnText.Items.Add(new ListItem("---------------------------------------", ""));

            for (int i = 0; i < _dtColumnsDetail.Rows.Count; i++)
            {

                string strPrefix = "";
                string strPrefix2 = "";
                string strPrefix3 = "";
                string strHTMLControl = "";
                string strHTMLControl2 = "";
                string strHTMLControl3 = "";
                if (Common.IsIn(_dtColumnsDetail.Rows[i]["ColumnType"].ToString(), "text,number,date,time,calculation")
                           || (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown" &&
                               _dtColumnsDetail.Rows[i]["DropDownType"].ToString() == "table"))
                {
                    strPrefix = "txt";
                    strHTMLControl = "Textbox";
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "dropdown" &&
                               _dtColumnsDetail.Rows[i]["DropDownType"].ToString() != "table")
                {
                    strPrefix = "ddl";
                    strHTMLControl = "Dropdown";
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "datetime")
                {
                    strPrefix = "txt";
                    strHTMLControl = "Textbox";
                    ddlConrolIDnText.Items.Add(new ListItem(_dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "(Textbox)",
                        _strDynamictabPart + "txtTime" + _dtColumnsDetail.Rows[i]["SystemName"].ToString()));
                }
                else if (_dtColumnsDetail.Rows[i]["SystemName"].ToString().ToLower() == "enteredby")
                {

                    ddlConrolIDnText.Items.Add(new ListItem(_dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "(Dropdown)",
                        _strDynamictabPart + "ddlEnteredBy"));
                    
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "button")
                {
                    strPrefix = "lnk";
                    strHTMLControl = "LinkButton";
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "file"
                       || _dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "image")
                {
                    strPrefix = "fu";
                    strHTMLControl = "Fileupload";
                    strPrefix2 = "hf";
                    strHTMLControl2 = "HiddenField";
                    strPrefix3 = "pnl";
                    strHTMLControl3 = "Div";
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "radiobutton")
                {
                    strPrefix = "radio";
                    strHTMLControl = "Radiobutton List";
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "location")
                {
                    strPrefix = "hf";
                    strHTMLControl = "HiddenField";
                    strPrefix2 = "hf2";
                    strHTMLControl2 = "HiddenField";
                    strPrefix3 = "hf3";
                    strHTMLControl3 = "HiddenField";
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "checkbox")
                {
                    strPrefix = "chk";
                    strHTMLControl = "Checkbox";
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                        && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "")
                {
                    strPrefix = "lst";
                    strHTMLControl = "Listbox";
                }
                else if (_dtColumnsDetail.Rows[i]["ColumnType"].ToString() == "listbox"
                      && _dtColumnsDetail.Rows[i]["DateCalculationType"].ToString() == "checkbox")
                {
                    strPrefix = "cbl";
                    strHTMLControl = "Listbox with Checkbox";
                }

                if(strPrefix!="")
                {
                    ddlConrolIDnText.Items.Add(new ListItem(_dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "(" + strHTMLControl + ")",
                       _strDynamictabPart + strPrefix + _dtColumnsDetail.Rows[i]["SystemName"].ToString()));
                }

                if (strPrefix2 != "")
                {
                    ddlConrolIDnText.Items.Add(new ListItem(_dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "(" + strHTMLControl2 + ")",
                       _strDynamictabPart + strPrefix2 + _dtColumnsDetail.Rows[i]["SystemName"].ToString()));
                }

                if (strPrefix3 != "")
                {
                    ddlConrolIDnText.Items.Add(new ListItem(_dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "(" + strHTMLControl3 + ")",
                       _strDynamictabPart + strPrefix3 + _dtColumnsDetail.Rows[i]["SystemName"].ToString()));
                }

                ddlConrolIDnText.Items.Add(new ListItem(_dtColumnsDetail.Rows[i]["DisplayTextDetail"].ToString() + "(row)",
                      _strDynamictabPart + "trX" + _dtColumnsDetail.Rows[i]["SystemName"].ToString()));
            }


            DataTable dtCT = Common.DataTableFromText("SELECT * FROM TableChild WHERE ParentTableID=" + iTableID.ToString() + " AND DetailPageType<>'not' ORDER BY DisplayOrder");

            if (dtCT.Rows.Count > 0)
            {
                
                ddlConrolIDnText.Items.Add(new ListItem("------------------------------", ""));
                ddlConrolIDnText.Items.Add(new ListItem( theTable.TableName+ " Header (div)", "div" + _strDynamictabPart + "lnkHeading"));
                ddlConrolIDnText.Items.Add(new ListItem(theTable.TableName + " Header (Linkbutton)",  _strDynamictabPart + "lnkHeading"));
                ddlConrolIDnText.Items.Add(new ListItem(theTable.TableName + " Record detail (div)", "pnlDetail"));

                foreach (DataRow dr in dtCT.Rows)
                {
                    Table ctTable = RecordManager.ets_Table_Details(int.Parse(dr["ChildTableID"].ToString()));
                    string strCaption = dr["Description"].ToString();

                    if (strCaption == "")
                    {
                        strCaption = ctTable.TableName;
                    }
                    string strDetail = "detail";
                    if (dr["DetailPageType"].ToString() == "list")
                    {
                        strDetail = "list";
                    }
                    ddlConrolIDnText.Items.Add(new ListItem("----------", ""));
                    ddlConrolIDnText.Items.Add(new ListItem(strCaption + " Child Header (div)", "div" + _strDynamictabPart + "lnkHeading" + dr["ChildTableID"].ToString()));
                    ddlConrolIDnText.Items.Add(new ListItem(strCaption + " Child Header (Linkbutton)", _strDynamictabPart + "lnkHeading" + dr["ChildTableID"].ToString()));
                    ddlConrolIDnText.Items.Add(new ListItem(strCaption + " Child " + strDetail + " (div)",  _strDynamictabPart + "pnlDetail" + dr["ChildTableID"].ToString()));

                }
            }

            if ((theTable.ShowSentEmails != null && (bool)theTable.ShowSentEmails)
               || (theTable.ShowReceivedEmails != null && (bool)theTable.ShowReceivedEmails))
            {
                ddlConrolIDnText.Items.Add(new ListItem("----------", ""));

                ddlConrolIDnText.Items.Add(new ListItem("Message Header (div)", "div" + _strDynamictabPart + "lnkHeadingmlList"));
                ddlConrolIDnText.Items.Add(new ListItem("Message Header (Linkbutton)", _strDynamictabPart + "lnkHeadingmlList"));
                ddlConrolIDnText.Items.Add(new ListItem("Message list (div)", _strDynamictabPart + "pnlDetailmlList"));

            }
           DataTable  dtDBTableTab = Common.DataTableFromText("SELECT * FROM TableTab WHERE TableID=" +
                    theTable.TableID.ToString() + " ORDER BY DisplayOrder");
           if (dtDBTableTab != null)
           {
               if (dtDBTableTab.Rows.Count > 1)
               {
                   ddlConrolIDnText.Items.Add(new ListItem("-------"+theTable.TableName+" Pages------", ""));

                   for (int t = 0; t < dtDBTableTab.Rows.Count; t++)
                   {
                       if (t == 0)
                       {
                           ddlConrolIDnText.Items.Add(new ListItem("       " + dtDBTableTab.Rows[t]["TabName"].ToString() + " -- Header (Linkbutton)", _strDynamictabPart + "lnkDetialTab"));
                           ddlConrolIDnText.Items.Add(new ListItem("       " + dtDBTableTab.Rows[t]["TabName"].ToString() + " -- Detail (Div)",  "pnlDetailTab"));
                       }
                       else
                       {
                           ddlConrolIDnText.Items.Add(new ListItem("       " + dtDBTableTab.Rows[t]["TabName"].ToString() + " -- Header (Linkbutton)", _strDynamictabPart + "lnkDetialTabD" + dtDBTableTab.Rows[t]["TableTabID"].ToString()));
                           ddlConrolIDnText.Items.Add(new ListItem("       " + dtDBTableTab.Rows[t]["TabName"].ToString() + " -- Detail (Div)", _strDynamictabPart + "pnlDetailTabD" + dtDBTableTab.Rows[t]["TableTabID"].ToString()));
                       }
                   }
               }
           }



        }
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        ColumnChangeService theColumnChangeService = new ColumnChangeService();

        if (ddlControlEvent.SelectedValue != "")
            theColumnChangeService.ControlEvent = ddlControlEvent.SelectedValue;

        if (txtSPName.Text.Trim() != "")
            theColumnChangeService.SPName = txtSPName.Text.Trim();

        if (txtDotNetMethod.Text.Trim() != "")
            theColumnChangeService.DotNetMethod = txtDotNetMethod.Text.Trim();

        if (txtJavaScriptFunction.Text.Trim() != "")
            theColumnChangeService.JavaScriptFunction = txtJavaScriptFunction.Text.Trim();

        if (ddlMessageType.SelectedValue != "")
            theColumnChangeService.MessageType = ddlMessageType.SelectedValue;
        if (ddlAfterValueChange.SelectedValue != "")
            theColumnChangeService.AfterValueChange = ddlAfterValueChange.SelectedValue;
        if (txtSPName.Text.Trim() == "" && ddlMessageType.SelectedValue == ""
            && ddlAfterValueChange.SelectedValue == "" && txtDotNetMethod.Text.Trim() == ""
            && txtJavaScriptFunction.Text.Trim() == "")
        {
            Session["ControlValueChangeService"] = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ControlValueChangeService", "GetBackValue('n')", true);
        }
        else
        {
            Session["ControlValueChangeService"] = theColumnChangeService.GetJSONString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ControlValueChangeService", "GetBackValue('y')", true);
        }
    }
    protected void PopulateRecord()
    {
        if (Session["ControlValueChangeService"] != null && Session["ControlValueChangeService"].ToString()!="")
        {
            ColumnChangeService theColumnChangeService = JSONField.GetTypedObject<ColumnChangeService>(Session["ControlValueChangeService"].ToString());
            if(theColumnChangeService!=null)
            {
                if (!string.IsNullOrEmpty(theColumnChangeService.ControlEvent))
                {
                    if (ddlControlEvent.Items.FindByValue(theColumnChangeService.ControlEvent) != null)
                        ddlControlEvent.SelectedValue = theColumnChangeService.ControlEvent;
                }

                if (!string.IsNullOrEmpty(theColumnChangeService.SPName))
                    txtSPName.Text = theColumnChangeService.SPName;

                if (!string.IsNullOrEmpty(theColumnChangeService.DotNetMethod))
                    txtDotNetMethod.Text = theColumnChangeService.DotNetMethod;

                if (!string.IsNullOrEmpty(theColumnChangeService.JavaScriptFunction))
                    txtJavaScriptFunction.Text = theColumnChangeService.JavaScriptFunction;



                if (!string.IsNullOrEmpty(theColumnChangeService.MessageType))
                {
                    if (ddlMessageType.Items.FindByValue(theColumnChangeService.MessageType) != null)
                        ddlMessageType.SelectedValue = theColumnChangeService.MessageType;
                }
                if (!string.IsNullOrEmpty(theColumnChangeService.AfterValueChange))
                {
                    if (ddlAfterValueChange.Items.FindByValue(theColumnChangeService.AfterValueChange) != null)
                        ddlAfterValueChange.SelectedValue = theColumnChangeService.AfterValueChange;
                }
            }
        }
    }
}