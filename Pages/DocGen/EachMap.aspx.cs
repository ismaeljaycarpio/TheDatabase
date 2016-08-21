using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Pages_DocGen_EachMap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //hfGunPoints.Value = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Images/gun_points.png";

                PopulateAllTableDDL();

                int _DocumentSectionID = 0;
                if (Request.QueryString["DocumentSectionID"] != null)
                {
                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out _DocumentSectionID);
                }
                else
                {
                    if (Request.QueryString["mobile"] != null)
                    {
                        map_canvas.Style.Add("width", "250px");
                        map_canvas.Style.Add("height", "250px");
                        return;
                    }
                }


                if (_DocumentSectionID <= 0)
                {
                    if (Request.QueryString["PrevID"].ToString() != "-1")
                    {
                        //hfRemoveSection.Value = "1";
                    }

                    //Response.Redirect("../Summary.aspx");
                }
                else
                {

                    using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
                    {



                        DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == _DocumentSectionID);
                        if (section != null)
                        {
                            if (section.Details != "")
                            {
                                DocGen.DAL.MapSectionDetail mapDetail = DocGen.DAL.JSONField.GetTypedObject<DocGen.DAL.MapSectionDetail>(section.Details);
                                if (mapDetail != null)
                                {
                                    if (mapDetail.MapTypeId != null && mapDetail.MapTypeId != "")
                                    {
                                        hfMaptype.Value = mapDetail.MapTypeId;
                                    }

                                    if (mapDetail.Latitude != null)
                                    {
                                        hfCentreLat.Value = mapDetail.Latitude.ToString();
                                        hfForceMapCenter.Value = "yes";
                                    }

                                    if (mapDetail.Longitude != null)
                                    {
                                        hfCentreLong.Value = mapDetail.Longitude.ToString();
                                    }

                                    if (mapDetail.MapScale != null)
                                    {
                                        hfOtherZoomLevel.Value = mapDetail.MapScale.ToString();
                                    }

                                    if (mapDetail.ShowLocation != null)
                                        ddlTableMap.SelectedValue = mapDetail.ShowLocation.ToString();

                                    if (mapDetail.Width != null)
                                        map_canvas.Style.Add("width", mapDetail.Width.ToString() + "px");
                                    if (mapDetail.Height != null)
                                        map_canvas.Style.Add("height", mapDetail.Height.ToString() + "px");


                                    if (mapDetail.Latitude != null && mapDetail.Longitude != null)
                                    {
                                        hfForceMapCenter.Value = "yes";
                                    }
                                    else
                                    {
                                        hfForceMapCenter.Value = "no";
                                    }

                                }

                            }

                        }
                        else
                        {
                            //Response.Redirect("../Summary.aspx", true);
                        }
                    }



                    //ShowList();
                }




            }
        }
        catch (Exception ex)
        {
            //
        }
        //cat

    }


    protected void PopulateAllTableDDL()
    {
        int iTN = 0;

        

        //ddlTableMap.DataSource = RecordManager.ets_Table_Select(null,
        //        null,
        //        null,
        //        int.Parse(Session["AccountID"].ToString()),
        //        null, null, true,
        //        "st.TableName", "ASC",
        //        null, null, ref  iTN, Session["STs"].ToString()); ;

      DataTable dtTableMap   = Common.DataTableFromText(@"SELECT   DISTINCT  [Table].TableName, [Table].TableID
FROM         [Column] INNER JOIN
                      [Table] ON [Column].TableID = [Table].TableID 
                      WHERE [Column].ColumnType='location' AND [Table].IsActive=1  AND [Table].AccountID=" + Session["AccountID"].ToString());


        ddlTableMap.DataSource=dtTableMap;

        ddlTableMap.DataBind();

        if (dtTableMap.Rows.Count >= 1)
        {
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("All", "-1");
            ddlTableMap.Items.Insert(0, liSelect);
        }

        if (dtTableMap.Rows.Count == 1 || dtTableMap.Rows.Count==0)
        {
            //ddlTableMap.Visible = false;
            ddlTableMap.Style.Add("display", "none");
        }
        
    }

    


}