using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_Record_WordExport : System.Web.UI.Page
{

    Record _theRecord = null;
    Table _theTable = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["id"] != null)
        {
            _theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(Request.QueryString["id"].ToString()));
            _theTable = RecordManager.ets_Table_Details((int)_theRecord.TableID);
        }
        if (!IsPostBack)
        {


            if (_theTable != null)
            {
                DataTable dtTemp = TheDatabase.GetDataRetrieverByTable((int)_theTable.TableID);
                if (dtTemp.Rows.Count > 0)
                {
                    ddlDataRetriever.DataSource = dtTemp;
                    ddlDataRetriever.DataBind();

                    if (dtTemp.Rows.Count == 1)
                    {
                        //lnkWordExportOK_Click(null, null);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "trigger_export", " $('#btnAutoExportWord').trigger('click');", true);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "close_export", "setTimeout(function () { parent.$.fancybox.close();}, 3000);", true);
                        //return;
                    }
                }

            }


        }
    }

    protected void lnkWordExportOK_Click(object sender, EventArgs e)
    {

        //

        try
        {
            string strError = "";
            if (ddlDataRetriever.SelectedItem != null)
            {


                DocTemplate theDocTemplate = DocumentManager.dbg_DocTemplate_Detail(int.Parse(ddlDataRetriever.SelectedValue));

                DataRetriever theDataRetriever = DocumentManager.dbg_DataRetriever_Detail((int)theDocTemplate.DataRetrieverID);

                DataTable dtRecordInfo = DocumentManager.DataRetrieverSP(null, _theRecord.RecordID, theDataRetriever.SPName);
                //string strFolder = "DocTemplates";


                //oliver <begin> Ticket 1451
                string sTableName = Common.GetValueFromSQL("SELECT TableName FROM [Table] WHERE TableID=" + theDataRetriever.TableID.ToString());
                string[] sFileSplitter = theDocTemplate.FileUniqueName.ToString().Split('.');
                string sFileExt = sFileSplitter[sFileSplitter.Length - 1].ToString();
                string sFileName = sTableName + "_" + _theRecord.RecordID.ToString() + "." + sFileExt;

                string strFileToCopy = Server.MapPath("Temp\\" + theDocTemplate.FileUniqueName);
                string strNewCopy = Server.MapPath("DocTemplates\\" + sFileName);

                if (System.IO.File.Exists(strNewCopy))
                    System.IO.File.Delete(strNewCopy);

                if (System.IO.File.Exists(Server.MapPath("Temp\\" + sFileName)))
                    System.IO.File.Delete(Server.MapPath("Temp\\" + sFileName));

                if (!System.IO.File.Exists(strFileToCopy))
                {
                    if (System.IO.File.Exists(Server.MapPath("DocTemplates\\" + theDocTemplate.FileUniqueName)))
                    {
                        System.IO.File.Copy(Server.MapPath("DocTemplates\\" + theDocTemplate.FileUniqueName), strFileToCopy);
                    }
                }

                System.IO.File.Copy(strFileToCopy, strNewCopy);
                System.IO.File.Copy(strFileToCopy, Server.MapPath("Temp\\" + sFileName));
                Common.GenerateWORDDoc2(Server.MapPath("Temp\\" + sFileName), dtRecordInfo, out strError);
                //oliver <end> Ticket 1451  
                
                //if(IsPostBack)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "close_export", "setTimeout(function () { parent.$.fancybox.close();}, 3000);", true);
                //}
                Page.Response.Redirect(Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Record/Temp/" + sFileName, false);
                //oliver <end>
            }
           

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('This template file type is not supported, please use .docx format file.');", true);
        }

    }

}