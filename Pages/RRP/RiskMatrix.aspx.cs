using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Pages_RRP_RiskMatrix : SecurePage
{

    Column _cDateIdentified = null;
    Column _cLikelihood = null;
    Column _cConsequence = null;
    Table _theTable = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "RRP - Risk Matrix";

        //variables must be here

        string riskTableID = "1695";
        string riskDateIdentifiedColumn = "28329";
        string riskLikelihoodColumn = "28332";
        string riskConsequenceColumn = "28333";

        if (Request.QueryString["TableID"] != null)
        {
            string qsTableID=Cryptography.Decrypt(Request.QueryString["TableID"].ToString());
            Table theRiskTable = RecordManager.ets_Table_Details(int.Parse(qsTableID));
            if (theRiskTable != null)
            {
                if (theRiskTable.TableName.ToLower() == "hazard")
                {
                    riskTableID = qsTableID;

                    string strDateIdentifiedColumn = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE TableID=" + qsTableID + " AND SystemName='V006'");

                    if (strDateIdentifiedColumn != "")
                    {
                        riskDateIdentifiedColumn = strDateIdentifiedColumn;
                    }

                    string strLikelihoodColumn = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE TableID=" + qsTableID + " AND SystemName='V009'");

                    if (strLikelihoodColumn != "")
                    {
                        riskLikelihoodColumn = strLikelihoodColumn;
                    }

                    string strConsequenceColumn = Common.GetValueFromSQL("SELECT ColumnID FROM [Column] WHERE TableID=" + qsTableID + " AND SystemName='V010'");

                    if (strConsequenceColumn != "")
                    {
                        riskConsequenceColumn = strConsequenceColumn;
                    }



                }
            }


        }

        _theTable = RecordManager.ets_Table_Details(int.Parse(riskTableID));
        _cDateIdentified = RecordManager.ets_Column_Details(int.Parse(riskDateIdentifiedColumn));
        _cLikelihood = RecordManager.ets_Column_Details(int.Parse(riskLikelihoodColumn));
        _cConsequence = RecordManager.ets_Column_Details(int.Parse(riskConsequenceColumn));
        
        //
        
        //fix the date format
        
        
        if (txtLeftDate.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtLeftDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"),
                DateTimeStyles.None, out dtTemp))
            {
                txtLeftDate.Text = dtTemp.ToShortDateString();
            }
            else
            {
                txtLeftDate.Text = "";
            }
        }




        if (txtRightDate.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtRightDate.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"),
                DateTimeStyles.None, out dtTemp))
            {
                txtRightDate.Text = dtTemp.ToShortDateString();
            }
            else
            {
                txtRightDate.Text = "";
            }
        }


        if (!IsPostBack)
        {
            txtLeftDate.Text = DateTime.Today.ToShortDateString();
            txtRightDate.Text = DateTime.Today.AddMonths(-1).ToShortDateString();

            //PopulateLeftPanel();
            //PopulateRightPanel();
            chkShowChange_CheckedChanged(null, null);

        }


    }

    protected void txtLeftDate_TextChanged(object sender, EventArgs e)
    {
        PopulateLeftPanel();
        PopulateRightPanel();
    }


    protected void chkShowChange_CheckedChanged(object sender, EventArgs e)
    {
        if (chkShowChange.Checked)
        {
            txtLeftDate.Enabled = true;
            ce_txtLeftDate.Enabled = true;

            txtRightDate.Enabled = true;
            ce_txtRightDate.Enabled = true;

            imgLeftDate.Enabled = true;
            imgDateRight.Enabled = true;
           
        }
        else
        {
            txtLeftDate.Enabled = false;
            ce_txtLeftDate.Enabled = false;
            txtRightDate.Enabled = false;
            ce_txtRightDate.Enabled = false;

            imgLeftDate.Enabled = false;
            imgDateRight.Enabled = false;
        }

        PopulateLeftPanel();
        PopulateRightPanel();

    }

    protected void chkCompare_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCompare.Checked)
        {
            tblRight.Visible = true;
        }
        else
        {
            tblRight.Visible = false;
        }
        PopulateRightPanel();
        PopulateLeftPanel();
    }

    protected void txtRightDate_TextChanged(object sender, EventArgs e)
    {
        PopulateRightPanel();
        PopulateLeftPanel();
    }

    protected void PopulateLeftPanel()
    {
        //for (int i = 1; i <= 5; i++)
        //{
        //    for (int j = 1; j <= 5; j++)
        //    {
        //        string tdID = "tdL" + i.ToString() + j.ToString();
        //        System.Web.UI.HtmlControls.HtmlTableCell tdCell = (System.Web.UI.HtmlControls.HtmlTableCell)tblLeft.FindControl(tdID);
        //        if (tdCell != null)
        //        {
                   
        //            tdCell.Style.Add("background-color", "#ffffff" );

        //        }

        //    }
        //}

        if (txtLeftDate.Text != "")
        {
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    string tdID = "tdL" + i.ToString() + j.ToString();
                    System.Web.UI.HtmlControls.HtmlTableCell tdCell = (System.Web.UI.HtmlControls.HtmlTableCell)tblLeft.FindControl(tdID);
                    if (tdCell != null)
                    {
                        string sCount = Common.GetValueFromSQL("SELECT COUNT(*) FROM Record WHERE v016='Open' and TableID=" + _theTable.TableID.ToString()
                            + " AND IsActive=1 AND convert(datetime," + _cDateIdentified.SystemName + ",103)<=convert(datetime,'" + txtLeftDate.Text + "',103)  AND " + 
                            _cConsequence.SystemName +"='"+i.ToString()+"' AND "+_cLikelihood.SystemName+"='"+j.ToString()+"'");
                        //tdCell.Style.Add("background-color", "#" + GetColorCode(sCount));

                        if (sCount != "0" )
                        {
                            tdCell.Style.Add("text-align", "center");
                            string strTextSearch = " AND v016='Open' and   convert(datetime,Record." + _cDateIdentified.SystemName + ",103)<=convert(datetime,'" + txtLeftDate.Text + "',103) AND Record." +
                            _cConsequence.SystemName + "='" + i.ToString() + "' AND Record." + _cLikelihood.SystemName + "='" + j.ToString() + "'";

                            string strListURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/RecordList.aspx?TableID="+Cryptography.Encrypt(_theTable.TableID.ToString())+"&TextSearch=" + Cryptography.Encrypt(strTextSearch);

                            tdCell.Controls.Add(new LiteralControl("<a target='_blank' style='font-weight:bold;font-size:15px;' href='" + strListURL + "'>" + sCount + "</a>"));
                        }
                    }
                    
                }
            }

        }

    }


    protected void PopulateRightPanel()
    {

        //for (int i = 1; i <= 5; i++)
        //{
        //    for (int j = 1; j <= 5; j++)
        //    {
        //        string tdID = "tdR" + i.ToString() + j.ToString();
        //        System.Web.UI.HtmlControls.HtmlTableCell tdCell = (System.Web.UI.HtmlControls.HtmlTableCell)tblRight.FindControl(tdID);
        //        if (tdCell != null)
        //        {

        //            tdCell.Style.Add("background-color", "#ffffff");

        //        }

        //    }
        //}

        if (txtRightDate.Text != "" && chkCompare.Checked)
        {
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    string tdID = "tdR" + i.ToString() + j.ToString();
                    System.Web.UI.HtmlControls.HtmlTableCell tdCell = (System.Web.UI.HtmlControls.HtmlTableCell)tblRight.FindControl(tdID);
                    if (tdCell != null)
                    {
                        string sCount = Common.GetValueFromSQL("SELECT COUNT(*) FROM Record WHERE v016='Open' and TableID=" + _theTable.TableID.ToString()
                            + " AND IsActive=1 AND convert(datetime," + _cDateIdentified.SystemName + ",103)<=convert(datetime,'" + txtRightDate.Text + "',103) AND " +
                            _cConsequence.SystemName + "='" + i.ToString() + "' AND " + _cLikelihood.SystemName + "='" + j.ToString() + "'");
                        //tdCell.Style.Add("background-color", "#" + GetColorCode(sCount));

                        if (sCount != "0")
                        {
                            tdCell.Style.Add("text-align", "center");
                            string strTextSearch = " AND v016='Open' and convert(datetime,Record." + _cDateIdentified.SystemName + ",103)<=convert(datetime,'" + txtRightDate.Text + "',103) AND Record." +
                            _cConsequence.SystemName + "='" + i.ToString() + "' AND Record." + _cLikelihood.SystemName + "='" + j.ToString() + "'";

                            string strListURL = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath
                                + "/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(_theTable.TableID.ToString() )+ "&TextSearch=" + Cryptography.Encrypt(strTextSearch);

                            tdCell.Controls.Add(new LiteralControl("<a target='_blank' style='font-weight:bold;font-size:15px;' href='" + strListURL + "'>" + sCount + "</a>"));
                        }
                    }

                }
            }

        }

    }

    protected string GetColorCode(string sNumber)
    {
        string strColor = "ffffff";
        switch (sNumber)
        {
            case "":
                strColor = "ffffff";
                break;
            case "0":
                strColor = "ffffff";
                break;
            case "1":
                strColor = "FF0000";
                break;
            case "2":
                strColor = "FFC000";
                break;
            case "3":
                strColor = "FFFF00";
                break;
            case "4":
                strColor = "808080";
                break;
            case "5":
                strColor = "BFBFBF";
                break;
            case "6":
                strColor = "0000FF";
                break;
            case "7":
                strColor = "00FF00";
                break;
            default:
                strColor = "00FF00";
                break;
        }

        return strColor;
    }
}