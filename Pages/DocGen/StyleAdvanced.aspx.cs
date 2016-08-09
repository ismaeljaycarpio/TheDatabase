using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_DocGen_StyleAdvanced : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["txtStyle"] != null)
            {
                hfTxtStyle.Value = Request.QueryString["txtStyle"].ToString();
            }
            if (Request.QueryString["txtGenStyle"] != null)
            {
                hfTxtGenStyle.Value = Request.QueryString["txtGenStyle"].ToString();
            }
           txtAdvancedStyle.Text= MergeStyles();

        }
    }

 
    protected string  MergeStyles()
    {
        string strOldStyle = hfTxtStyle.Value;
        string strNewStyleAdded = hfTxtGenStyle.Value;

        strOldStyle = strOldStyle.Replace("background-color", "background-clr");

        strNewStyleAdded = strNewStyleAdded.Replace("background-color", "background-clr");

        if (strNewStyleAdded.Trim() == "")
        {
            return hfTxtStyle.Value;
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
            if (bFound == false && eachOld != "")
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

                if (bFound == false)
                {
                    foreach (string eachOld in aOldStyles)
                    {
                        if (eachOld != "")
                        {
                            if (eachOld.IndexOf(eachProperty) > -1)
                            {
                                strTempEachValue = eachOld;
                                break;
                            }
                        }
                    }

                    if (strTempEachValue != "")
                        strOtherValues = strOtherValues + strTempEachValue + ";";
                }
            }
        }



        strFinalProperties = strNewStyleAdded + strOtherValues + strOtherProperties;
        return strFinalProperties.Replace("background-clr", "background-color");


    }
}