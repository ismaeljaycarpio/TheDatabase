using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;



public partial class Pages_Record_FormSetPrint : System.Web.UI.Page
{
 
    private Pages_UserControl_DetailEdit[] _ctDetailEdit;
    public int ParentTableID { get; set; }
    public int ParentRecordID { get; set; }
    User _objUser;
    Table _theTable;
    DataTable _dtFormSet;
    string _strMode = "view";

    protected void Page_Init(object sender, EventArgs e)
    {
        _objUser = (User)Session["User"];
        ParentTableID = int.Parse(Cryptography.Decrypt(Request.QueryString["ParentTableID"].ToString()));
        ParentRecordID = int.Parse(Cryptography.Decrypt(Request.QueryString["ParentRecordID"].ToString()));
        _theTable = RecordManager.ets_Table_Details(ParentTableID);

        //_dtFormSet = Common.DataTableFromText("SELECT * FROM FormSet WHERE ParentTableID=" + ParentTableID.ToString() + " ORDER BY ProgressColumnID,DisplayOrder");

       

        Pages_UserControl_DetailEdit _ctDetailEditP = (Pages_UserControl_DetailEdit)LoadControl("~/Pages/UserControl/DetailEdit.ascx");
        _ctDetailEditP.TableID = ParentTableID;
        _ctDetailEditP.ID = "_ctDetailEditP";
        _ctDetailEditP.Mode = _strMode;
        _ctDetailEditP.RecordID = ParentRecordID;
        Panel cTabPanelP = new Panel();

        cTabPanelP.BorderStyle = BorderStyle.Solid;
        cTabPanelP.BorderWidth = 1;

        divDynamic.Controls.Add(new LiteralControl("<strong style='font-size:15pt;'>"+_theTable.TableName + "</strong>"));
        divDynamic.Controls.Add(new LiteralControl("<br/>"));
        cTabPanelP.Controls.Add(_ctDetailEditP);
        divDynamic.Controls.Add(cTabPanelP);


        if (Request.QueryString["NoFormSet"] == null)
        {

            _dtFormSet = Common.DataTableFromText(@"SELECT FormSetForm.FormSetFormID,FormSetForm.DisplayOrder,UpdateColumnID,UpdateColumnValue,FormSetForm.TableID AS ChildTableID,
                        (SELECT TOP 1 HideColumnID FROM TableChild WHERE 
                        TableChild.ParentTableID=" + ParentTableID.ToString() + @" AND TableChild.ChildTableID=FormSetForm.TableID )  AS HideColumnID,
                        (SELECT TOP 1 HideColumnValue FROM TableChild WHERE 
                        TableChild.ParentTableID=" + ParentTableID.ToString() + @" AND TableChild.ChildTableID=FormSetForm.TableID )  AS HideColumnValue,
                        (SELECT TOP 1 HideOperator FROM TableChild WHERE 
                        TableChild.ParentTableID=" + ParentTableID.ToString() + @" AND TableChild.ChildTableID=FormSetForm.TableID )  AS HideOperator
                        FROM FormSetForm INNER JOIN [Table]
                        ON FormSetForm.TableID=[Table].TableID
                        INNER JOIN FormSet ON FormSetForm.FormSetID=FormSet.FormSetID
                        INNER JOIN FormSetGroup ON FormSetGroup.FormSetGroupID=FormSet.FormSetGroupID                       
                        WHERE FormSetGroup.ParentTableID=" + ParentTableID.ToString()
                           + @" ORDER BY FormSetGroup.ColumnPosition,FormSet.RowPosition, FormSetForm.DisplayOrder");

            if (_dtFormSet.Rows.Count > 0)
            {
                MakeChildTables(_dtFormSet);
            }
        }


    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Print-" + _theTable.TableName;
    }

    protected void MakeChildTables(DataTable dtCT)
    {


        _ctDetailEdit = new Pages_UserControl_DetailEdit[dtCT.Rows.Count];

        Panel[] cTabPanel = new Panel[dtCT.Rows.Count];


        int i = 0;
        foreach (DataRow dr in dtCT.Rows)
        {
            

            Table theChildTable = RecordManager.ets_Table_Details(int.Parse(dr["ChildTableID"].ToString()));
                        
            string strCaption = theChildTable.TableName;

            strCaption = "<strong style='font-size:13pt;'>" + strCaption + "</strong>";
            
            string strTextSearch = "";
            string strRecordID = "";
            if (ParentRecordID.ToString() != "")
            {

                DataTable dtTemp = Common.DataTableFromText("SELECT SystemName,ColumnID FROM [Column] WHERE ColumnType='dropdown' AND  TableID=" + dr["ChildTableID"].ToString() + " AND TableTableID=" + ParentTableID.ToString());
                foreach (DataRow drCT in dtTemp.Rows)
                {
                    Column theChildColumn = RecordManager.ets_Column_Details(int.Parse(drCT["ColumnID"].ToString()));
                    Column theLinkedColumn = RecordManager.ets_Column_Details((int)theChildColumn.LinkedParentColumnID);
                    Record theLinkedRecord = RecordManager.ets_Record_Detail_Full(int.Parse(ParentRecordID.ToString()));
                    string strLinkedColumnValue = RecordManager.GetRecordValue(ref theLinkedRecord, theLinkedColumn.SystemName);
                    strLinkedColumnValue = strLinkedColumnValue.Replace("'", "''");

                    if (strTextSearch == "")
                    {
                        strTextSearch = " Record." + drCT["SystemName"].ToString() + "='" + strLinkedColumnValue + "' ";
                    }
                    else
                    {
                        strTextSearch = strTextSearch + " OR " + " Record." + drCT["SystemName"].ToString() + "='" + strLinkedColumnValue + "' ";
                    }

                    strRecordID = Common.GetValueFromSQL("SELECT TOP 1 RecordID FROM Record WHERE TableID=" +
                dr["ChildTableID"].ToString() + " AND IsActive= 1 AND (" + strTextSearch + ") ORDER BY RecordID");

                    if (strRecordID == "")
                    {
                        if (SecurityManager.IsRecordsExceeded((int)_theTable.AccountID))
                        {
                            Session["DoNotAllow"] = "true";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "DoNotAllow", "alert('" + Common.RecordExceededMessage.Replace("'", "''") + "');", true);

                            continue;
                        }

                        //create a balnk record
                        Record theRecord = new Record();
                        theRecord.TableID = int.Parse(dr["ChildTableID"].ToString());
                        theRecord.IsActive = true;
                        theRecord.EnteredBy = _objUser.UserID;
                        RecordManager.MakeTheRecord(ref theRecord, drCT["SystemName"].ToString(), strLinkedColumnValue);
                        int iNewRecordID = RecordManager.ets_Record_Insert(theRecord);
                        strRecordID = iNewRecordID.ToString();
                    }

                }
            }

            //_TabIndex = _TabIndex + 1;
            
            _ctDetailEdit[i] = (Pages_UserControl_DetailEdit)LoadControl("~/Pages/UserControl/DetailEdit.ascx");
            _ctDetailEdit[i].TableID = int.Parse(dr["ChildTableID"].ToString());
            _ctDetailEdit[i].ID = "ctDetailEdit" + i.ToString();
            _ctDetailEdit[i].Mode = _strMode;

            _ctDetailEdit[i].RecordID = int.Parse(strRecordID);

            cTabPanel[i] = new Panel();

            cTabPanel[i].BorderStyle=BorderStyle.Dotted;
            cTabPanel[i].BorderWidth=1;

            divDynamic.Controls.Add(new LiteralControl("<br/>"));
            divDynamic.Controls.Add(new LiteralControl(strCaption));
            divDynamic.Controls.Add(new LiteralControl("<br/>"));

            cTabPanel[i].Controls.Add(_ctDetailEdit[i]);
            divDynamic.Controls.Add(cTabPanel[i]);

            //}

            i = i + 1;
        }




    }
}