using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Public : System.Web.UI.Page
{
    string _qsTableID = "";
    Table _theTable;
    Table _theParentTable;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            _qsTableID = Request.QueryString["TableID"].ToString();
            int iTableID = int.Parse(_qsTableID);

            _theTable = RecordManager.ets_Table_Details(iTableID);
            if (_theTable.AddWithoutLogin != null)
            {
                if ((bool)_theTable.AddWithoutLogin)
                {

                    if (_theTable.ParentTableID == null)
                    {
                        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                            + "/Pages/Record/RecordDetail.aspx?public=yes&mode=" + Cryptography.Encrypt("add")
                            + "&TableID=" + Cryptography.Encrypt(_qsTableID) + "&SearchCriteriaID="
                            + Cryptography.Encrypt("-1"), true);
                    }
                    else
                    {
                        //now find the colum

                        _theParentTable = RecordManager.ets_Table_Details((int)_theTable.ParentTableID);
                        if (!IsPostBack)
                        {
                            divParentTable.Visible = true;
                            if (_theParentTable != null)
                            {
                                //lblNotVaid.Visible = true;
                                //txtParentRecordID.Text = Cryptography.Decrypt(Request.QueryString["ParentRecordID"].ToString());
                                if (_theTable.ValidateColumnID1 != null)
                                {
                                    Column theValidateColumn1=RecordManager.ets_Column_Details((int)_theTable.ValidateColumnID1);
                                    if(theValidateColumn1!=null)
                                    {
                                        lblParentRecord.Text = theValidateColumn1.DisplayName;
                                    }
                                }
                                if (_theTable.ValidateColumnID2 != null)
                                {
                                    Column theValidateColumn2 = RecordManager.ets_Column_Details((int)_theTable.ValidateColumnID2);
                                    if (theValidateColumn2 != null)
                                    {
                                        lblParentRecord2.Text = theValidateColumn2.DisplayName;
                                    }
                                }
                            }
                        }
                      

                    }
                }
                else
                {
                    lblBadRequest.Visible = true;
                }
            }
            else
            {
                
            }
        }
        catch
        {
            lblBadRequest.Visible = true;
        }
      
    }


    protected void lnkContinue_Click(object sender, EventArgs e)
    {
        lblBadRequest.Visible = false;
        lblNotVaid.Visible = false;

        string strSQL = "SELECT RecordID FROM Record WHERE IsActive=1 AND TableID=" + _theParentTable.TableID.ToString();
        bool bRunSQL = false;
        if (_theTable.ValidateColumnID1 != null)
        {
            Column theValidateColumn1 = RecordManager.ets_Column_Details((int)_theTable.ValidateColumnID1);
            if (theValidateColumn1 != null)
            {
                strSQL = strSQL + "  AND " + theValidateColumn1.SystemName + "='"+txtParentRecord.Text.Replace("'","''")+"'";
                bRunSQL = true;
            }
        }

        if (_theTable.ValidateColumnID2 != null)
        {
            Column theValidateColumn2 = RecordManager.ets_Column_Details((int)_theTable.ValidateColumnID2);
            if (theValidateColumn2 != null)
            {
                strSQL = strSQL + "  AND " + theValidateColumn2.SystemName + "='" + txtParentRecord2.Text.Replace("'", "''") + "'";
                bRunSQL = true;
            }
        }

        if (bRunSQL == false)
        {
            lblBadRequest.Visible = true;
            return;
        }

        string strParentRecordID = Common.GetValueFromSQL(strSQL);

        if (strParentRecordID == "")
        {
            lblBadRequest.Visible = false ;
            lblNotVaid.Visible = true;
            return;
        }

        Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                            + "/Pages/Record/RecordDetail.aspx?public=yes&mode=" + Cryptography.Encrypt("add")
                            + "&TableID=" + Cryptography.Encrypt(_qsTableID) + "&SearchCriteriaID="
                            + Cryptography.Encrypt("-1") + "&ParentID=" + _theTable.ParentTableID.ToString()
                            + "&ParentRecordID=" + Cryptography.Encrypt(strParentRecordID), true);

    }

}