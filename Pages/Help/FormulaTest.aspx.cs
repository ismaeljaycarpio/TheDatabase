using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Page_Help_FormulaTest: SecurePage
{
    string _strType = "valid";
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Request.QueryString["type"] != null)
        {
            _strType = Request.QueryString["type"].ToString();
        }
       

        if (!IsPostBack)
        {
            if(Request.QueryString["TableID"]!=null)
            {
                Table theTable = RecordManager.ets_Table_Details(int.Parse(Request.QueryString["TableID"].ToString()));
                if (theTable != null)
                    hfTableID.Value = theTable.TableID.ToString();

                if(Request.QueryString["ColumnID"]!=null)
                {
                    hfColumnID.Value = Request.QueryString["ColumnID"].ToString();
                    if(hfColumnID.Value=="-1")
                    {
                        if (theTable!=null)
                            lblSubTitle.Text = theTable.TableName + " - " + "New Column";
                    }
                    else
                    {
                        Column theColumn = RecordManager.ets_Column_Details(int.Parse(hfColumnID.Value));
                        if (theTable != null && theColumn!=null)
                            lblSubTitle.Text = theTable.TableName + " - " + theColumn.DisplayName;
                    }
                    
                }
            }


            if (Request.QueryString["min"] != null && Request.QueryString["max"] != null && Request.QueryString["formula"] != null)
            {

                if (Request.QueryString["min"].ToString().Trim() != ""
                    || Request.QueryString["max"].ToString().Trim() != "")
                {
                    //well we got not advanced part.
                    string strFormula = "";

                    //if (_strType == "valid")
                    //{

                        if (Request.QueryString["min"].ToString().Trim() != "")
                        {
                            strFormula = "value>=" + Request.QueryString["min"].ToString().Trim();
                        }

                        if (Request.QueryString["max"].ToString().Trim() != "")
                        {
                            strFormula = "value<=" + Request.QueryString["max"].ToString().Trim();
                        }

                        if (Request.QueryString["min"].ToString().Trim() != ""
                        && Request.QueryString["max"].ToString().Trim() != "")
                        {
                            strFormula = "value>=" + Request.QueryString["min"].ToString().Trim() + " AND " + "value<=" + Request.QueryString["max"].ToString().Trim();
                        }

                        txtValidation.Text = strFormula;
                    //}
                    //else
                    //{

                    //    if (Request.QueryString["min"].ToString().Trim() != "")
                    //    {
                    //        strFormula = "value>" + Request.QueryString["min"].ToString().Trim();
                    //    }

                    //    if (Request.QueryString["max"].ToString().Trim() != "")
                    //    {
                    //        strFormula = "value<" + Request.QueryString["max"].ToString().Trim();
                    //    }

                    //    if (Request.QueryString["min"].ToString().Trim() != ""
                    //    && Request.QueryString["max"].ToString().Trim() != "")
                    //    {
                    //        strFormula = "value>" + Request.QueryString["min"].ToString().Trim() + " AND  " + "value<" + Request.QueryString["max"].ToString().Trim();
                    //    }

                    //    txtValidation.Text = strFormula;


                    //}

                }
                else
                {

                    txtValidation.Text = Request.QueryString["formula"].ToString();
                }

            }


            //if (Request.QueryString["formula"] != null)
            //{
            //    txtValidation.Text = Request.QueryString["formula"].ToString();
            //}

        }



        string strContentKey = "";
        if (_strType == "valid")
        {
            lblValidationType.Text = "Data Validation Formula";
            Title="Data Validation";
            strContentKey = "ValidationHelp";
        }
        else if (_strType == "warning")
        {
            lblValidationType.Text = "Data Warning Formula";
            Title = "Data Warning";
            strContentKey = "WarningHelp";
        }
        else
        {
            lblValidationType.Text = "Data Exceedance Formula";
            Title = "Data Exceedance";
            strContentKey = "ExceedanceHelp";
        }


        Content theContent = SystemData.Content_Details_ByKey(strContentKey, null);
        if (theContent != null)
        {
            lblContentCommon.Text = theContent.ContentP;
        }




        if (!IsPostBack)
        {
            if (_strType == "valid")
            {

                this.lnkOk.Attributes.Add("onclick", "javascript:return GetBackValue('valid')");
            }
            else if (_strType == "warning")
            {
                this.lnkOk.Attributes.Add("onclick", "javascript:return GetBackValue('warning')");
            }
            else
            {
                this.lnkOk.Attributes.Add("onclick", "javascript:return GetBackValue('exceedance')");
            }
        }
        

    }

    protected void PerformTest(ref string strError)
    {
        strError = "";
        if (_strType == "valid")
        {

            lblResult.Text = "";

            if (txtData.Text.Trim() == "" || txtValidation.Text.Trim() == "")
            {
                lblResult.Text = "Please enter validation Formula and Data";
                lblResult.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (UploadManager.IsDataValid(txtData.Text, txtValidation.Text, ref strError))
            {
                lblResult.Text = "Valid - data would be imported";
                lblResult.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblResult.ForeColor = System.Drawing.Color.Red;
                if (strError == "")
                {
                    lblResult.Text = "Invalid - data would be rejected";
                }
                else
                {
                    lblResult.Text = "ERROR:" + strError;
                    lblResult.Text = lblResult.Text + "<br/> <I>Please correct the validation Formula syntax!</I>";
                }
            }

        }
        else if (_strType == "warning")
        {
            lblResult.Text = "";

            if (txtData.Text.Trim() == "" || txtValidation.Text.Trim() == "")
            {
                lblResult.Text = "Please enter warning Formula and Data";
                lblResult.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (!UploadManager.IsDataValid(txtData.Text, txtValidation.Text, ref strError))
            {
                lblResult.Text = "A warning would be issued";
                lblResult.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (strError == "")
                {
                    lblResult.Text = "No warning would be issued";
                    lblResult.ForeColor = System.Drawing.Color.Green;
                }
                else
                {

                    lblResult.Text = "ERROR:" + strError;
                    lblResult.Text = lblResult.Text + "<br/> <I>Please correct the warning Formula syntax!</I>";
                    lblResult.ForeColor = System.Drawing.Color.Red;
                }
            }


        }
        else
        {
            lblResult.Text = "";

            if (txtData.Text.Trim() == "" || txtValidation.Text.Trim() == "")
            {
                lblResult.Text = "Please enter exceedance Formula and Data";
                lblResult.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (!UploadManager.IsDataValid(txtData.Text, txtValidation.Text, ref strError))
            {
                lblResult.Text = "A exceedance would be issued";
                lblResult.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (strError == "")
                {
                    lblResult.Text = "No exceedance would be issued";
                    lblResult.ForeColor = System.Drawing.Color.Green;
                }
                else
                {

                    lblResult.Text = "ERROR:" + strError;
                    lblResult.Text = lblResult.Text + "<br/> <I>Please correct the exceedance Formula syntax!</I>";
                    lblResult.ForeColor = System.Drawing.Color.Red;
                }
            }

        }
       
    }

    protected void lnkTest_Click(object sender, EventArgs e)
    {
        string strError = "";

        if ( txtValidation.Text.Trim() != "")
        {
            if (txtData.Text.Trim() == "")
            {
                lblResult.Text = "Enter Data in the text field to validated";
                lblResult.ForeColor = System.Drawing.Color.Red;
                //txtData.Focus();
                return;
                //txtData.Text = "1";
            }
        }

        PerformTest(ref strError);

    }

    protected void lnkNo_Click(object sender, EventArgs e)
    {
        trMainSave.Visible = true;
        trConfirmation.Visible = false;
        lblMessage.Text = "";
    }
   
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        string strError = "";


        if (txtValidation.Text.Trim() != "")
        {
            if (txtData.Text.Trim() == "")
            {
                lblResult.Text = "Enter Data in the text field to validated";
                lblResult.ForeColor = System.Drawing.Color.Red;
                //txtData.Focus();
                return;
                //txtData.Text = "1";
            }
        }


        PerformTest(ref strError);

        if (strError != "")
        {
            lblMessage.Text = "The Formula caused an error with a Data value of "+txtData.Text+". Do you wish to save this Formula?";
            trMainSave.Visible = false;
            trConfirmation.Visible = true;
        }
        else
        {

            string strFormula = txtValidation.Text.Trim();
            string strMin = "";
            string strMax = "";

            int iTotalValue=0;

            if (_strType == "valid")
            {

                iTotalValue = Common.GetNumberOfValue(strFormula);


                strMin = Common.GetMinVaue(strFormula);
                strMax = Common.GetMaxVaue(strFormula);
                hfMax.Value = strMax;
                hfMin.Value = strMin;


                if (iTotalValue > 2)
                {
                    hfAdvanced.Value = "yes";
                }
                else
                {
                    if (iTotalValue > 1)
                    {
                        if (strMin != "" && strMax != "")
                        {
                            hfAdvanced.Value = "no";
                        }
                        else
                        {
                            hfAdvanced.Value = "yes";
                        }
                    }
                    else
                    {
                        if (strMin != "" || strMax != "")
                        {
                            hfAdvanced.Value = "no";
                        }
                        else
                        {
                            if (txtValidation.Text.Trim() == "")
                            {
                                hfAdvanced.Value = "no";
                            }
                            else
                            {
                                hfAdvanced.Value = "yes";
                            }
                        }

                    }
                }
            }

            else
            {


                iTotalValue = Common.GetNumberOfValue(strFormula);


                strMin = Common.GetMinFromFormula(strFormula);
                strMax = Common.GetMaxFromFormula(strFormula);
                hfMax.Value = strMax;
                hfMin.Value = strMin;


                if (iTotalValue > 2)
                {
                    hfAdvanced.Value = "yes";
                }
                else
                {
                    if (iTotalValue > 1)
                    {
                        if (strMin != "" && strMax != "")
                        {
                            hfAdvanced.Value = "no";
                        }
                        else
                        {
                            hfAdvanced.Value = "yes";
                        }
                    }
                    else
                    {
                        if (strMin != "" || strMax != "")
                        {
                            hfAdvanced.Value = "no";
                        }
                        else
                        {
                            if (txtValidation.Text.Trim() == "")
                            {
                                hfAdvanced.Value = "no";
                            }
                            else
                            {
                                hfAdvanced.Value = "yes";
                            }
                        }

                    }
                }



            }



                if (_strType == "valid")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormulaEdit", "GetBackValue('valid')", true);
                }
                else if (_strType == "warning")
                {                          

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormulaEdit", "GetBackValue('warning')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormulaEdit", "GetBackValue('exceedance')", true);
                }

            
           
        }
    }

   

}
