using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Record_AuditDetail : SecurePage
{
    int? _iRecordID = null;
    int? _iTableTableID = null;
    int? _iLinkedParentColumnID = null;
    string _strDisplayColumn = "";
    string _strSystemName = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["UpdatedDate"] != null && Request.QueryString["RecordID"] != null)
        {

            DateTime dtUpdatedDate = DateTime.Parse(Request.QueryString["UpdatedDate"].ToString());
            _iRecordID = int.Parse(Request.QueryString["RecordID"].ToString());
            grdAuditDetail.DataSource = RecordManager.Record_Audit_Detail((int)_iRecordID, dtUpdatedDate);
            grdAuditDetail.DataBind();
        }

        

    }

    protected void grdAuditDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (_iRecordID != null)
            {

                DataTable dtRecordColums = Common.DataTableFromText(@"SELECT [Column].TableTableID,DisplayColumn,
                [Column].SystemName,LinkedParentColumnID
                    FROM         Record INNER JOIN
                  [Column] ON Record.TableID = [Column].TableID
                  WHERE  [Column].TableTableID IS NOT NULL
                  AND ([Column].DropDownType='table' OR [Column].DropDownType='tabledd') AND [Column].ColumnType='dropdown'
                  AND [Column].DisplayColumn IS NOT NULL AND Record.RecordID=" + _iRecordID.ToString()
                    + @" AND [Column].SystemName='" + DataBinder.Eval(e.Row.DataItem, "SystemName").ToString() + "'");

                if (dtRecordColums.Rows.Count > 0)
                {
                    _iTableTableID = int.Parse(dtRecordColums.Rows[0]["TableTableID"].ToString());
                    _iLinkedParentColumnID = int.Parse(dtRecordColums.Rows[0]["LinkedParentColumnID"].ToString());
                    _strDisplayColumn = dtRecordColums.Rows[0]["DisplayColumn"].ToString();
                    _strSystemName = dtRecordColums.Rows[0]["SystemName"].ToString();
                }

            }

            if (_iTableTableID != null && _iLinkedParentColumnID!=null)
            {
                if (_strSystemName.ToLower() == DataBinder.Eval(e.Row.DataItem, "SystemName").ToString().ToLower())
                {

                    if (DataBinder.Eval(e.Row.DataItem, "OldValue") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "OldValue").ToString() != "")
                    {


                        Column theLinkedColumn = RecordManager.ets_Column_Details((int)_iLinkedParentColumnID);
                        
                      
                        string strLinkedColumnValue = DataBinder.Eval(e.Row.DataItem, "OldValue").ToString();
                        DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
                         + _iTableTableID.ToString());

                        string strDisplayColumn = _strDisplayColumn;
                        string sstrDisplayColumnOrg = strDisplayColumn;

                        foreach (DataRow dr in dtTableTableSC.Rows)
                        {
                            strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                        }

                        sstrDisplayColumnOrg = strDisplayColumn;
                        string strFilterSQL = "";
                        if (theLinkedColumn.SystemName.ToLower() == "recordid")
                        {

                            strFilterSQL = strLinkedColumnValue;
                        }
                        else
                        {
                            strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                        }

                        DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);
                        if (dtTheRecord.Rows.Count > 0)
                        {
                            foreach (DataColumn dc in dtTheRecord.Columns)
                            {
                                Column theColumn = RecordManager.ets_Column_Details_By_Sys((int)theLinkedColumn.TableID, dc.ColumnName);
                                if (theColumn != null)
                                {
                                    if (theColumn.ColumnType == "date")
                                    {
                                        string strDatePartOnly = dtTheRecord.Rows[0][dc.ColumnName].ToString();

                                        if (strDatePartOnly.Length > 9)
                                        {
                                            strDatePartOnly = strDatePartOnly.Substring(0, 10);
                                        }

                                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", strDatePartOnly);
                                    }
                                    else
                                    {
                                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                    }
                                }

                            }
                        }

                        Label lblOldValue = (Label)e.Row.FindControl("lblOldValue");

                        if (lblOldValue != null)
                        {
                            if (sstrDisplayColumnOrg != strDisplayColumn)
                                lblOldValue.Text = strDisplayColumn;
                        }
                        //DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE  TableID ="
                        //                         + _iTableTableID.ToString());

                        //string strDisplayColumn = _strDisplayColumn;

                        //foreach (DataRow dr in dtTableTableSC.Rows)
                        //{
                        //    strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                        //}

                        //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + DataBinder.Eval(e.Row.DataItem, "OldValue").ToString());
                        //if (dtTheRecord.Rows.Count > 0)
                        //{
                        //    foreach (DataColumn dc in dtTheRecord.Columns)
                        //    {
                        //        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                        //    }
                        //}

                       

                    }

                    //new value

                    if (DataBinder.Eval(e.Row.DataItem, "NewValue") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "NewValue").ToString() != "")
                    {

                        Column theLinkedColumn = RecordManager.ets_Column_Details((int)_iLinkedParentColumnID);


                        string strLinkedColumnValue = DataBinder.Eval(e.Row.DataItem, "NewValue").ToString();
                        DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE   TableID ="
                         + _iTableTableID.ToString());

                        string strDisplayColumn = _strDisplayColumn;
                        string sstrDisplayColumnOrg = strDisplayColumn;

                        foreach (DataRow dr in dtTableTableSC.Rows)
                        {
                            strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                        }

                        sstrDisplayColumnOrg = strDisplayColumn;
                        string strFilterSQL = "";
                        if (theLinkedColumn.SystemName.ToLower() == "recordid")
                        {

                            strFilterSQL = strLinkedColumnValue;
                        }
                        else
                        {
                            strFilterSQL = "'" + strLinkedColumnValue.Replace("'", "''") + "'";
                        }

                        DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE TableID=" + theLinkedColumn.TableID.ToString() + " AND " + theLinkedColumn.SystemName + "=" + strFilterSQL);
                        if (dtTheRecord.Rows.Count > 0)
                        {
                            foreach (DataColumn dc in dtTheRecord.Columns)
                            {
                                Column theColumn = RecordManager.ets_Column_Details_By_Sys((int)theLinkedColumn.TableID, dc.ColumnName);
                                if (theColumn != null)
                                {
                                    if (theColumn.ColumnType == "date")
                                    {
                                        string strDatePartOnly = dtTheRecord.Rows[0][dc.ColumnName].ToString();

                                        if (strDatePartOnly.Length > 9)
                                        {
                                            strDatePartOnly = strDatePartOnly.Substring(0, 10);
                                        }

                                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", strDatePartOnly);
                                    }
                                    else
                                    {
                                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                    }
                                }

                            }
                        }

                        Label lblNewValue = (Label)e.Row.FindControl("lblNewValue");

                        if (lblNewValue != null)
                        {
                            if (sstrDisplayColumnOrg != strDisplayColumn)
                                lblNewValue.Text = strDisplayColumn;
                        }

                        //DataTable dtTableTableSC = Common.DataTableFromText("SELECT SystemName,DisplayName FROM [Column] WHERE  TableID ="
                        //                         + _iTableTableID.ToString());

                        //string strDisplayColumn = _strDisplayColumn;

                        //foreach (DataRow dr in dtTableTableSC.Rows)
                        //{
                        //    strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                        //}

                        //DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + DataBinder.Eval(e.Row.DataItem, "NewValue").ToString());
                        
                        //if (dtTheRecord.Rows.Count > 0)
                        //{
                        //    foreach (DataColumn dc in dtTheRecord.Columns)
                        //    {
                        //        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                        //    }
                        //}

                       

                    }


                }

            }
           
                //FILE NAME
                if (_iRecordID != null)
                {
                    DataTable dtRecordColums = Common.DataTableFromText(@"SELECT     Record.RecordID, [Column].SystemName
                        FROM         Record INNER JOIN
                      [Column] ON Record.TableID = [Column].TableID
                      WHERE ([Column].ColumnType='file' OR [Column].ColumnType='image') AND  Record.RecordID=" + _iRecordID.ToString() + " AND [Column].SystemName='" + DataBinder.Eval(e.Row.DataItem, "SystemName").ToString() + "'");
                    if (dtRecordColums.Rows.Count > 0)
                    {
                        _strSystemName = dtRecordColums.Rows[0]["SystemName"].ToString();
                        if (_strSystemName.ToLower() == DataBinder.Eval(e.Row.DataItem, "SystemName").ToString().ToLower())
                        {
                            if (DataBinder.Eval(e.Row.DataItem, "OldValue") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "OldValue").ToString() != "")
                            {
                                Label lblOldValue = (Label)e.Row.FindControl("lblOldValue");
                                if (lblOldValue != null)
                                {
                                    if (lblOldValue.Text.Length > 37)
                                    {
                                        lblOldValue.Text = lblOldValue.Text.Substring(37);
                                    }
                                }
                            }

                            if (DataBinder.Eval(e.Row.DataItem, "NewValue") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "NewValue").ToString() != "")
                            {

                                Label lblNewValue = (Label)e.Row.FindControl("lblNewValue");

                                if (lblNewValue != null)
                                {
                                    if (lblNewValue.Text.Length > 37)
                                    {
                                        lblNewValue.Text = lblNewValue.Text.Substring(37);
                                    }
                                }
                            }

                        }

                    }


                }//


            



        }

    }
}