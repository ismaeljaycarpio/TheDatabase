using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class Test_DocumentFolderChange : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnChangeFolder_Click(object sender, EventArgs e)
    {
        return;

        DataTable dtDocuments = Common.DataTableFromText(@"SELECT * FROM [Document] WHERE UniqueName <> '' 
            AND UniqueName IS NOT NULL AND UniqueName<>'DashBoard' AND DateUpdated <DATEADD(D,-1,GETDATE())");


        foreach (DataRow dr in dtDocuments.Rows)
        {
            Document theDocument = DocumentManager.ets_Document_Detail(int.Parse(dr["DocumentID"].ToString()));

            if (theDocument != null)
            {
                if(File.Exists(Server.MapPath("..\\Pages\\Document\\Uploads\\" + dr["UniqueName"].ToString())))
                {
                    try
                    {
                        theDocument.FileUniqename = dr["AccountID"].ToString() + "_" + theDocument.FileUniqename;
                        File.Copy(Server.MapPath("..\\Pages\\Document\\Uploads\\" + dr["UniqueName"].ToString())
                            , Server.MapPath("..\\UserFiles\\Documents\\" + theDocument.FileUniqename), true);

                        DocumentManager.ets_Document_Update(theDocument);


                    }
                    catch
                    {
                        //do nothing

                    }
                }

            }

        }
        

    }



}