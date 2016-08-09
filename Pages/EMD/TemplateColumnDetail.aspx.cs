using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Pages_EMD_TemplateColumnDetail : SecurePage
{

    int? _iTableID = null;
    //int? _iTableItemID = null;
    Table _theTable = null;
    string _strSFTID;
    UserRole _theUserRole = null;
    Role _theRole = null;
    bool _bGod = false;
      
    protected void PopulateTerminology()
    {

        //stgParentFieldCap.InnerText = stgParentFieldCap.InnerText.Replace("Field", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Field", "Field"));
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        
        _iTableID=int.Parse( Cryptography.Decrypt(Request.QueryString["TableID"].ToString()));

        _theTable = RecordManager.ets_Table_Details((int)_iTableID);

        _strSFTID = SystemData.SystemOption_ValueByKey_Account("Standardised_Field_Table", _theTable.AccountID, _theTable.TableID);

        _theUserRole = (UserRole)Session["UserRole"];
        _theUserRole = SecurityManager.UserRole_Details((int)_theUserRole.UserRoleID);
        _theRole = SecurityManager.Role_Details((int)_theUserRole.RoleID);

        if (Common.HaveAccess(Session["roletype"].ToString(), "1"))
        {
            _bGod = true;
        }

        if (!IsPostBack)
        {        
            PopulateListBoxes();
            PopulateTerminology();
            //lnkRemove.Visible = false;
            //if (_theUserRole != null && _theUserRole.IsAccountHolder != null && (bool)_theUserRole.IsAccountHolder)
            //{
            //    lnkRemove.Visible=true;
            //}
            //else if (_bGod)
            //{
            //    lnkRemove.Visible = true;
            //}
            //else
            //{
            //    lnkRemove.Visible = false;
            //    if (_theRole != null && _theRole.RoleType == "2")
            //    {
            //        if (_theUserRole != null && _theUserRole.AllowDeleteColumn != null && (bool)_theUserRole.AllowDeleteColumn)
            //        {
            //            lnkRemove.Visible = true;
            //        }
            //    }
            //}

        }
    }


 


    protected void lnkRemove_Click(object sender, EventArgs e)
    {
        if (lstUsed.SelectedItem != null)
        {
            for (int i = lstUsed.Items.Count - 1; i >= 0; --i)
            {
                if (lstUsed.Items[i].Selected)
                {
                    lstNotUsed.Items.Add(new ListItem(lstUsed.Items[i].Text, lstUsed.Items[i].Value));

                    lstUsed.Items.RemoveAt(i);
                }
            }

        }
        else
        {
            lstNotUsed.Focus();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RemoveMessage", "alert('Please select a column from right side list.');", true);
        }


    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        

        if (lstNotUsed.SelectedItem != null)
        {
            for (int i = lstNotUsed.Items.Count - 1; i >= 0; --i)
            {
                if (lstNotUsed.Items[i].Selected)
                {
                    lstUsed.Items.Add(new ListItem(lstNotUsed.Items[i].Text, lstNotUsed.Items[i].Value));

                    lstNotUsed.Items.RemoveAt(i);
                }
            }

        }
        else
        {
            lstUsed.Focus();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RemoveMessage", "alert('Please select a column from left side list.');", true);
        }



    }


    protected void lnkSaveNew_Click(object sender, EventArgs e)
    {

        //DELETE Removed item.

        //foreach (ListItem li in lstNotUsed.Items)
        //{
        //    if(_bGod)
        //    {
        //        //RecordManager.ets_Column_Delete(int.Parse(li.Value));
        //    }
                       
        //}

        //add new items

        foreach (ListItem li in lstUsed.Items)
        {

            DataTable dtTemp = Common.DataTableFromText("SELECT ColumnID FROM [Column] WHERE TableID=" + _iTableID.ToString() + " AND ColumnID=" + li.Value);

            if (dtTemp.Rows.Count == 0)
            {
                //lets add this column 
            
                string strDSystemName = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE TableID=" + _strSFTID + " AND DisplayName='" +
             EMD_Standardised_Field_Table.Decimals + "'");

                string strISSystemName = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE TableID=" + _strSFTID + " AND DisplayName='" +
             EMD_Standardised_Field_Table.Ignore_Symbols + "'");
                string strSGSystemName = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE TableID=" + _strSFTID + " AND DisplayName='" +
           EMD_Standardised_Field_Table.Show_Graph + "'");

                DataTable dtSFT_Row = Common.DataTableFromText("SELECT " + strDSystemName + "," + strISSystemName + "," 
                    + strSGSystemName + " FROM [Record] WHERE RecordID=" + li.Value);

                if (dtSFT_Row.Rows.Count > 0)
                {
                    Column newColumn = new Column();
                    newColumn.TableID = _theTable.TableID;
                    newColumn.DisplayName = li.Text;
                    newColumn.DisplayTextSummary = li.Text;
                    newColumn.DisplayTextDetail = li.Text;
                    newColumn.NameOnImport = li.Text;
                    newColumn.NameOnExport = li.Text;
                    newColumn.ColumnType = "number";
                    newColumn.NumberType = 1;

                    string strAutoSystemName = "";

                    strAutoSystemName = RecordManager.ets_Column_NextSystemName((int)_theTable.TableID);

                    int? iDisplayOrder = RecordManager.ets_Table_MaxOrder((int)_theTable.TableID);

                    if (iDisplayOrder == null)
                        iDisplayOrder = -1;

                    newColumn.SystemName = strAutoSystemName;
                    newColumn.DisplayOrder = iDisplayOrder + 1;


                    try
                    {
                        if (dtSFT_Row.Rows[0][0] != DBNull.Value)
                        {
                            newColumn.IsRound = true;
                            newColumn.RoundNumber = int.Parse(dtSFT_Row.Rows[0][0].ToString());
                        }

                        if (dtSFT_Row.Rows[0][1] != DBNull.Value)
                        {
                            if (dtSFT_Row.Rows[0][1].ToString().ToLower() == "yes")
                            {
                                newColumn.IgnoreSymbols = true;
                            }
                           
                        }

                        if (dtSFT_Row.Rows[0][2] != DBNull.Value)
                        {
                            if (dtSFT_Row.Rows[0][2].ToString().ToLower() == "yes")
                            {
                                newColumn.GraphLabel = li.Text;
                            }

                        }
                    }
                    catch
                    {
                        //
                    }

                    RecordManager.ets_Column_Insert(newColumn);

                }
            }

         
            
        }


        ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);

    }



    


    protected void PopulateListBoxes()
    {
        
       

        if (_strSFTID != "")
        {
            //Table theSFTTable = RecordManager.ets_Table_Details(int.Parse(strSFT));
            string strSTSystemName = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE TableID="+_strSFTID+" AND DisplayName='"+ 
                EMD_Standardised_Field_Table.Sample_Type +"'");

            string strANSystemName = Common.GetValueFromSQL("SELECT SystemName FROM [Column] WHERE TableID=" + _strSFTID + " AND DisplayName='" +
               EMD_Standardised_Field_Table.Analyte_Name + "'");

            DataTable dtNotUsed = Common.DataTableFromText("SELECT " + strANSystemName + ",RecordID FROM [Record] WHERE IsActive=1 AND TableID=" + _strSFTID
                + " AND " + strANSystemName + " IS  NOT NULL AND (" + strSTSystemName + " LIKE '%" + _theTable.TableName.Replace("'", "''") + "%' OR "
                + strSTSystemName + " IS NULL)"
                + " AND " + strANSystemName + " NOT IN (SELECT DisplayName FROM [Column] WHERE TableID=" + _theTable.TableID.ToString() + ")   ORDER BY " + strANSystemName + " ASC");

            lstNotUsed.Items.Clear();

            foreach (DataRow dr in dtNotUsed.Rows)
            {
                ListItem liTemp = new ListItem(dr[0].ToString(), dr[1].ToString());
                lstNotUsed.Items.Add(liTemp);
            }
            
            DataTable dtUsed = Common.DataTableFromText(@"SELECT DisplayName,ColumnID FROM [Column] WHERE 
                                        SystemName not in('IsActive','TableID','DateTimeRecorded','EnteredBy','RecordID') 
                                         AND TableID=" + _theTable.TableID.ToString()
                                        + "  ORDER BY DisplayName ASC");

            lstUsed.Items.Clear();

            foreach (DataRow dr in dtUsed.Rows)
            {
                ListItem liTemp = new ListItem(dr["DisplayName"].ToString(), dr["ColumnID"].ToString());
                lstUsed.Items.Add(liTemp);
            }


        }



    }



}