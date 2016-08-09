using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Pages_Record_OrderSC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( Request.QueryString["newSC"] != null)
        {

            //SqlTransaction tn;
            //SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString);
            //connection.Open();
            //tn = connection.BeginTransaction();

            try
            {
                string strNewSC = Request.QueryString["newSC"].ToString().Substring(0, Request.QueryString["newSC"].ToString().Length - 1);
                string[] newSC = strNewSC.Split(',');


                DataTable dtDO = Common.DataTableFromText("SELECT DisplayOrder FROM [Column] WHERE   ColumnID IN (" + strNewSC + ") ORDER BY DisplayOrder");
                if (newSC.Length == dtDO.Rows.Count)
                {
                    for (int i = 0; i < newSC.Length; i++)
                    {

                        Column newColumn = RecordManager.ets_Column_Details(int.Parse(newSC[i]));

                        if (newColumn != null)
                        {
                            newColumn.DisplayOrder = int.Parse(dtDO.Rows[i][0].ToString());
                            RecordManager.ets_Column_Update(newColumn);

                            //oldColumn.DisplayOrder = iNew;
                            //RecordManager.ets_Column_Update(oldColumn);
                        }
                    }
                }


                
            }
            catch (Exception ex)
            {

               //

            }
            

        }
    }
}