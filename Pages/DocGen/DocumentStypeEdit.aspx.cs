using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DocGen.DocumentSectionStyle
{
    public partial class Edit : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int ID = 0;
                if (Request.QueryString["DocumentSectionStyleID"] != null)
                {
                    lblTopTitle.Text = "Edit document text style";

                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionStyleID"]), out ID);
                    if (ID > 0)
                    {
                        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                        {
                            DAL.DocumentSectionStyle textStyle = ctx.DocumentSectionStyles.SingleOrDefault<DAL.DocumentSectionStyle>(dt => dt.DocumentSectionStyleID == ID);
                            if (textStyle != null)
                            {
                                if (textStyle.AccountID == this.AccountID)
                                {
                                    txtTitle.Text = textStyle.StyleName;
                                    txtStyle.Text = textStyle.StyleDefinition;
                                    PopulateStyleControls(textStyle.StyleDefinition);

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "PopulateHref();", true);
                                }
                                else
                                {
                                    Response.Redirect("DocumentStyleList.aspx", true);
                                }
                            }
                            else
                            {
                                Response.Redirect("DocumentStyleList.aspx", true);
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("DocumentStyleList.aspx", true);
                    }
                }
                else
                {
                    //new
                    lblTopTitle.Text = "Add document text style";
                }
                CancelButton.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/DocGen/DocumentStyleList.aspx?rmode=" + Request.QueryString["rmode"].ToString() + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&TableID=" + Request.QueryString["TableID"].ToString() + "&SSearchCriteriaID=" + Request.QueryString["SSearchCriteriaID"].ToString() + "&DocumentID=" + Request.QueryString["DocumentID"].ToString();

            }
        }

        protected string  MergeStyles()
        {
            string strOldStyle = txtStyle.Text;
            string strNewStyleAdded = txtGenStyle.Text;

            strOldStyle = strOldStyle.Replace("background-color", "background-clr");

            strNewStyleAdded = strNewStyleAdded.Replace("background-color", "background-clr");
         
            if (strNewStyleAdded.Trim() == "")
            {
                return txtStyle.Text;
            }                       

            string[] aOldStyles = strOldStyle.Split(';');
            string[] aNewStyles = strNewStyleAdded.Split(';');
           
            string strListOfProperties = "font-family,font-size,font-weight,text-decoration,font-style,color,background-clr,border,line-height,margin";
            string[] strProterties = strListOfProperties.Split(',');

            string strFinalProperties = "";
            string strOtherProperties = "";
            foreach (string eachOld in aOldStyles)
            {
                bool bFound = false;
                foreach (string eachProperty in strProterties)
                {
                    if (eachOld.IndexOf(eachProperty) > -1)
                    {
                        if (eachProperty != "" && eachOld != "")
                        {
                            bFound = true;
                        }
                    }
                }
                if (bFound == false && eachOld!="")
                {
                    strOtherProperties = strOtherProperties + eachOld + ";";
                }
            }

            string strOtherValues = "";
           
            string strTempEachValue = "";
            foreach (string eachProperty in strProterties)
            {
                if (eachProperty != "")
                {
                    bool bFound = false;
                    strTempEachValue = "";
                    foreach (string eachNew in aNewStyles)
                    {                        
                        if (eachNew != "")
                        {
                            foreach (string eachOld in aOldStyles)
                            {
                                if (eachOld != "")
                                {                                   
                                    if (eachOld.IndexOf(eachProperty) > -1 && eachNew.IndexOf(eachProperty) > -1)
                                    {                                       
                                        bFound = true;
                                        break;
                                    }                                   
                                }
                            }
                           
                        }
                     
                    }

                    if (bFound==false)
                    {
                        foreach (string eachOld in aOldStyles)
                        {
                            if (eachOld != "")
                            {
                                if (eachOld.IndexOf(eachProperty) > -1 )
                                {
                                    strTempEachValue=eachOld;
                                    break;
                                }
                            }
                        }

                        if (strTempEachValue!="")
                            strOtherValues = strOtherValues + strTempEachValue + ";";                       
                    }
                }
            }
          

            
            strFinalProperties = strNewStyleAdded + strOtherValues +  strOtherProperties;

            if (chkBold.Checked==false)
            {
                strFinalProperties = strFinalProperties.Replace("font-weight:bold;", "");
            }

            if (chkItalic.Checked == false)
            {
                strFinalProperties = strFinalProperties.Replace("font-style:italic;", "");
            }

            if (chkUnderline.Checked == false && chkStrikethrough.Checked == false)
            {
                strFinalProperties = strFinalProperties.Replace("text-decoration:underline line-through;", "");
                strFinalProperties = strFinalProperties.Replace("text-decoration:underline;", "");
                strFinalProperties = strFinalProperties.Replace("text-decoration:line-through;", "");
            }
            else if (chkUnderline.Checked && chkStrikethrough.Checked == false) 
            {
                strFinalProperties = strFinalProperties.Replace("line-through", "");
            }
            else if (chkUnderline.Checked==false && chkStrikethrough.Checked)
            {
                strFinalProperties = strFinalProperties.Replace("underline", "");
            }



            return strFinalProperties.Replace("background-clr", "background-color");


        }


        protected void PopulateStyleControls(string strStyle)
        {
            //font
            strStyle = strStyle.ToLower();

            string strProperty = "font-family:";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                string strStartToEnd = strStyle.Substring(strStyle.IndexOf(strProperty));
                if (strStartToEnd.IndexOf(";")>-1)
                {
                string strValue = strStartToEnd.Substring(strProperty.Length, strStartToEnd.IndexOf(";") - strProperty.Length);

                ListItem theValue = ddlFont.Items.FindByValue(strValue);

                if (theValue != null)
                    ddlFont.Value = strValue;
                }
               
            }


            //font size           

            strProperty = "font-size:";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                string strStartToEnd = strStyle.Substring(strStyle.IndexOf(strProperty));
                if (strStartToEnd.IndexOf(";") > -1)
                {
                    string strValue = strStartToEnd.Substring(strProperty.Length, strStartToEnd.IndexOf(";") - strProperty.Length);
                    strValue = strValue.Replace("px", "");
                    ListItem theValue = ddlFontSize.Items.FindByValue(strValue);

                    if (theValue != null)
                        ddlFontSize.Value = strValue;
                }
            }


            //chkBold          

            strProperty = "font-weight:";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                string strStartToEnd = strStyle.Substring(strStyle.IndexOf(strProperty));
                if (strStartToEnd.IndexOf(";") > -1)
                {
                    string strValue = strStartToEnd.Substring(strProperty.Length, strStartToEnd.IndexOf(";") - strProperty.Length);

                    if (strValue.Trim() == "bold")
                    {
                        chkBold.Checked = true;
                    }
                    else
                    {
                        chkBold.Checked = false;
                    }
                  
                }
            }


            //italic

            strProperty = "italic";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                chkItalic.Checked = true;
            }
            else
            {
                chkItalic.Checked = false;
            }

            //underline

            strProperty = "underline";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                chkUnderline.Checked = true;
            }
            else
            {
                chkUnderline.Checked = false;
            }

            //line-through

            strProperty = "line-through";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                chkStrikethrough.Checked = true;
            }
            else
            {
                chkStrikethrough.Checked = false;
            }

            //line-height           

            strProperty = "line-height:";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                string strStartToEnd = strStyle.Substring(strStyle.IndexOf(strProperty));
                if (strStartToEnd.IndexOf(";") > -1)
                {
                    string strValue = strStartToEnd.Substring(strProperty.Length, strStartToEnd.IndexOf(";") - strProperty.Length);                   
                    ListItem theValue = ddlLineHeight.Items.FindByValue(strValue);

                    if (theValue != null)
                        ddlLineHeight.Value = strValue;
                }
            }

            //background-color          

            strProperty = "background-color:";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                string strStartToEnd = strStyle.Substring(strStyle.IndexOf(strProperty));
                if (strStartToEnd.IndexOf(";") > -1)
                {
                    string strValue = strStartToEnd.Substring(strProperty.Length, strStartToEnd.IndexOf(";") - strProperty.Length);
                    ListItem theValue = ddlBackground.Items.FindByValue(strValue);

                    if (theValue != null)
                        ddlBackground.Value = strValue;
                }
            }

            //color     

            string strStyleTemp = strStyle.Replace("background-color", "");
            strProperty = "color:";
            if (strStyleTemp.IndexOf(strProperty) > -1)
            {
                string strStartToEnd = strStyleTemp.Substring(strStyleTemp.IndexOf(strProperty));
                if (strStartToEnd.IndexOf(";") > -1)
                {
                    string strValue = strStartToEnd.Substring(strProperty.Length, strStartToEnd.IndexOf(";") - strProperty.Length);
                    ListItem theValue = ddlTextColour.Items.FindByValue(strValue);

                    if (theValue != null)
                        ddlTextColour.Value = strValue;
                }
            }

            //margin          

            strProperty = "margin:";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                string strStartToEnd = strStyle.Substring(strStyle.IndexOf(strProperty));
                if (strStartToEnd.IndexOf(";") > -1)
                {
                    string strValue = strStartToEnd.Substring(strProperty.Length, strStartToEnd.IndexOf(";") - strProperty.Length);
                    strValue = strValue.Replace("px", "");
                    txtMargin.Value = strValue;
                }
            }


            //border:          

            strProperty = "border:";
            if (strStyle.IndexOf(strProperty) > -1)
            {
                string strStartToEnd = strStyle.Substring(strStyle.IndexOf(strProperty));
                if (strStartToEnd.IndexOf(";") > -1)
                {
                    string strValue = strStartToEnd.Substring(strProperty.Length, strStartToEnd.IndexOf(";") - strProperty.Length);

                    strValue = strValue.Trim();
                    string[] words = strValue.Split(' ');
                    foreach (string word in words)
                    {
                        if (word.IndexOf("px") > -1)
                        {
                            ListItem theValue = ddlBorder.Items.FindByValue(word.Replace("px",""));

                            if (theValue != null)
                                ddlBorder.Value = word.Replace("px", "");

                        }
                        else if (word == "solid")
                        {
                            //do nothing
                        }
                        else
                        {
                            ListItem theValue = ddlBorderColour.Items.FindByValue(word);

                            if (theValue != null)
                                ddlBorderColour.Value = word;
                        }

                    }

                    //string strFirstValue = strValue.Trim().Substring(0, strValue.Trim().IndexOf(" ")).Trim();
                    //strValue = strValue.Replace("px", "");
                    //txtMargin.Value = strValue;
                }
            }





        }


        protected void hlAdvanced_Click(object sender, EventArgs e)
        {
             hfFullStyle.Value=  MergeStyles();

             ScriptManager.RegisterStartupScript(this, this.GetType(), "AjaxAlert", " setTimeout(function () { ClickPopulateButton(); }, 100);", true);

        }


        public int AccountID
        {
            get
            {
                int retVal = 0;
                if (Session["AccountID"] != null)
                    retVal = Convert.ToInt32(Session["AccountID"]);
                return retVal;
            }
        }

        protected void lnkPerformTest_Click(object sender, EventArgs e)
        {

            txtStyle.Text = txtGenStyle.Text;


        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            ErrorMessage.Text = "";
            txtStyle.Text= MergeStyles();
            if (Request.QueryString["DocumentSectionStyleID"] != null)
            {

                int ID = 0;
                Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionStyleID"]), out ID);
                try
                {
                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {
                        DAL.DocumentSectionStyle textStyle = ctx.DocumentSectionStyles.SingleOrDefault<DAL.DocumentSectionStyle>(dt => dt.DocumentSectionStyleID == ID);

                        if (textStyle != null)
                        {
                            if (textStyle.IsSystem == false)
                            {
                                textStyle.StyleName = txtTitle.Text.Trim();
                            }
                            textStyle.StyleDefinition = txtStyle.Text;
                            ctx.SubmitChanges();
                        }
                    }
                    Response.Redirect(CancelButton.NavigateUrl, false);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = "This style name is used, please use another style name.";
                }
            }
            else
            {
                //

                try
                {
                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {
                        DAL.DocumentSectionStyle textStyle = new DAL.DocumentSectionStyle()
                        {
                            StyleName = txtTitle.Text.Trim(),
                            StyleDefinition = txtStyle.Text,
                            DateAdded = DateTime.Now,
                            DateUpdated = DateTime.Now,
                            AccountID = this.AccountID
                        };
                        ctx.DocumentSectionStyles.InsertOnSubmit(textStyle);
                        ctx.SubmitChanges();
                    }
                    Response.Redirect(CancelButton.NavigateUrl, false);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = "This style name is used, please use another style name.";
                }


            }
        }
    }
}