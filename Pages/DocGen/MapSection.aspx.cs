using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;

namespace DocGen.Document.MapSection
{



    public partial class Edit : SecurePage
    {

        protected void PopulateTableDDL()
        {
            int iTN = 0;

            //ddlTableMapPop.DataSource = RecordManager.ets_Table_Select(null,
            //        null,
            //        null,
            //        AccountID,
            //        null, null, true,
            //        "st.TableName", "ASC",
            //        null, null, ref  iTN, "");

            ddlTableMapPop.DataSource = Common.DataTableFromText(@"SELECT   DISTINCT  [Table].TableName, [Table].TableID
FROM         [Column] INNER JOIN
                      [Table] ON [Column].TableID = [Table].TableID 
                      WHERE [Column].ColumnType='location' AND [Table].IsActive=1  AND [Table].AccountID=" + AccountID.ToString());

            ddlTableMapPop.DataBind();


            System.Web.UI.WebControls.ListItem liSelect2 = new System.Web.UI.WebControls.ListItem("All", "-1");
            ddlTableMapPop.Items.Insert(0, liSelect2);


            
        }

        public int DocumentSectionID
        {
            get
            {
                int _DocumentSectionID = 0;
                if (Request.QueryString["DocumentSectionID"] != null)
                {
                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out _DocumentSectionID);
                }
                return _DocumentSectionID;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                PopulateTableDDL();

                string strDefaultPin = "Pages/Record/PINImages/DefaultPin.png";

                hfImage.Value = "http://" + Request.Url.Authority + Request.ApplicationPath + "/" + strDefaultPin;
                hfFlag.Value = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Images/Flag.png";
                hfGunPoints.Value = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Images/gun_points.png";
                //gun_points.png


                if (DocumentSectionID <= 0)
                {
                    if (Request.QueryString["PrevID"].ToString() != "-1")
                    {
                        //hfRemoveSection.Value = "1";
                    }
                    Account theAccount = SecurityManager.Account_Details((int)AccountID);

                    if (theAccount != null)
                    {
                        if (theAccount.MapCentreLat != null)
                            hfCentreLat.Value = theAccount.MapCentreLat.ToString();

                        if (theAccount.MapCentreLong != null)
                            hfCentreLong.Value = theAccount.MapCentreLong.ToString();

                        if (theAccount.MapZoomLevel != null)
                            hfOtherZoomLevel.Value = theAccount.MapZoomLevel.ToString();

                        hfForceMapCenter.Value = "yes";
                    }

                    //add



                    //Response.Redirect("../Summary.aspx");
                }
                else
                {

                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {



                        DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                        if (section != null)
                        {
                            if (section.Details != "")
                            {
                                MapSectionDetail mapDetail = JSONField.GetTypedObject<MapSectionDetail>(section.Details);
                                if (mapDetail != null)
                                {
                                    txtAddress.Text = mapDetail.Address;

                                    if (mapDetail.MapTypeId != null && mapDetail.MapTypeId != "")
                                    {
                                        hfMaptype.Value = mapDetail.MapTypeId;
                                    }

                                    if (mapDetail.Latitude != null)
                                    {
                                        txtLatitude.Text = mapDetail.Latitude.ToString();
                                        hfCentreLat.Value = mapDetail.Latitude.ToString();
                                        hfForceMapCenter.Value = "yes";
                                    }
                                    
                                    if (mapDetail.Longitude != null)
                                    {
                                        txtLongitude.Text = mapDetail.Longitude.ToString();
                                        hfCentreLong.Value = mapDetail.Longitude.ToString();
                                    }

                                    if (mapDetail.MapScale != null)
                                    {
                                        //hfOtherZoomLevel.Value = mapDetail.MapScale.ToString();
                                        hfOtherZoomLevel.Value = mapDetail.MapScale.ToString();
                                    }

                                    if (mapDetail.ShowLocation != null)
                                        ddlTableMapPop.SelectedValue = mapDetail.ShowLocation.ToString();

                                    if (mapDetail.Height != null)
                                        txtHeight.Text = mapDetail.Height.ToString();

                                    if (mapDetail.Width != null)
                                        txtWidth.Text = mapDetail.Width.ToString();


                                    //if (mapDetail.Latitude != null && mapDetail.Longitude != null && mapDetail.MapScale != null)
                                    //{
                                    //    hlChoose.NavigateUrl = "~/Pages/Site/GoogleMap.aspx?type=mapsection&lat=" + mapDetail.Latitude.ToString() + "&lng=" + mapDetail.Longitude.ToString() + "&zoom=" + mapDetail.MapScale.ToString();

                                    //}
                                    //else if (mapDetail.Latitude != null && mapDetail.Longitude != null)
                                    //{
                                    //    hlChoose.NavigateUrl = "~/Pages/Site/GoogleMap.aspx?type=mapsection&lat=" + mapDetail.Latitude.ToString() + "&lng=" + mapDetail.Longitude.ToString();

                                    //}
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

        protected void CheckPermission(int DocumentID)
        {
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID && d.AccountID == this.AccountID);
                if (doc == null)
                {
                    Response.Redirect("~/Empty.aspx", false);
                }
            }
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


        protected void SaveButton_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            try
            {

                MapSectionDetail mapDetail = new MapSectionDetail();

                mapDetail.Address = txtAddress.Text;
                mapDetail.Latitude=txtLatitude.Text==""?null:(double?) double.Parse(txtLatitude.Text);
                mapDetail.Longitude = txtLongitude.Text == "" ? null : (double?)double.Parse(txtLongitude.Text);
                mapDetail.MapScale = int.Parse(hfOtherZoomLevel.Value);
                mapDetail.ShowLocation =int.Parse(ddlTableMapPop.SelectedValue);
                mapDetail.MapTypeId = hfMaptype.Value;
                mapDetail.Height = txtHeight.Text == "" ? null : (int?)int.Parse(txtHeight.Text);
                mapDetail.Width = txtWidth.Text == "" ? null : (int?)int.Parse(txtWidth.Text);

                Account theAccount = SecurityManager.Account_Details((int)AccountID);

                if (theAccount != null)
                {
                    theAccount.MapCentreLat = mapDetail.Latitude;
                    theAccount.MapCentreLong = mapDetail.Longitude;
                    SecurityManager.Account_Update(theAccount);
                }



                int DocumentSectionID = 0;
                if (Request.QueryString["PrevID"].ToString() != "-1")
                {

                    int DocumentID = 0;
                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);

                    int iPosition = 1;                  

                    int NewSectionID = 0;

                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {

                        if (Request.QueryString["PrevID"].ToString() != "0")
                        {

                            DAL.DocumentSection PreSection = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == int.Parse(Request.QueryString["PrevID"].ToString()));

                            iPosition = PreSection.Position + 1;
                        }
                        else
                        {
                            iPosition = 1;
                        }

                        DAL.DocumentSection newSection = new DAL.DocumentSection();

                        //if (Request.QueryString["Position"] != null)
                        //{
                        ctx.ExecuteCommand("UPDATE DocumentSection SET Position=Position + 1 WHERE DocumentID={0}  AND Position>{1}", DocumentID.ToString(), (iPosition - 1).ToString());
                        //}

                        newSection.DocumentID = DocumentID;
                        //newSection.SectionName = txtTitle.Text;
                        newSection.DocumentSectionTypeID = 7; //Map
                        //newSection.DocumentSectionStyleID = int.Parse(ddlStyle.SelectedValue);
                        newSection.Content = "";
                        newSection.Details = mapDetail.GetJSONString();
                        newSection.Position = iPosition;
                        newSection.DateAdded = DateTime.Now;
                        newSection.DateUpdated = DateTime.Now;
                        ctx.DocumentSections.InsertOnSubmit(newSection);



                        ctx.SubmitChanges();

                        NewSectionID = newSection.DocumentSectionID;
                        DocumentSectionID = NewSectionID;
                        //hfRemoveSection.Value = "0";
                    }


                }
                else
                {

                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out DocumentSectionID);
                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {
                        DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                        section.Content = "";
                        //section.DocumentSectionStyleID = int.Parse(ddlStyle.SelectedValue);
                        section.Details = mapDetail.GetJSONString();
                        ctx.SubmitChanges();
                    }
                }
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + DocumentSectionID.ToString() + ");", true);

            }
            catch
            {
                lblMsg.Text = "";

            }

        }
        


    }
}