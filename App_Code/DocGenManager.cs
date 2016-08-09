using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocGen.DAL;
/// <summary>
/// Summary description for DocGenManager
/// </summary>
public class DocGenManager
{
	public DocGenManager()
	{
		//
		// TODO: Add constructor logic here
		//
        
	}


    public static string TextSectionValues(DocumentSection section, string strText)
    {
        Document theDocument = DocumentManager.ets_Document_Detail(section.DocumentID);


        if (theDocument != null)
        {
            if (theDocument.DocumentDate != null)
            {
                strText = strText.Replace("[ReportStartDate]", theDocument.DocumentDate.Value.ToShortDateString());
            }

            if (theDocument.DocumentEndDate != null)
            {
                strText = strText.Replace("[ReportEndDate]", theDocument.DocumentEndDate.Value.ToShortDateString());
            }


        }

        return strText;

    }


}