using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DocGen.Utility;
using System.Configuration;
using DocGen.DAL;
using System.Drawing;
using System.Data;
using System.Xml;
namespace DocGen.Document.Calendar
{
    public partial class Edit : SecurePage
    {
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


        protected void PopulateDatabaseField()
        {

            ddlDatabaseField.Items.Clear();

            if (ddlTable.SelectedValue == "")
                return;

            int iTableID = int.Parse(ddlTable.SelectedValue);

            //TableTableID IS NULL AND 
            DataTable dtColumns = Common.DataTableFromText("SELECT DisplayName FROM [Column] WHERE IsStandard=0 AND   TableID=" + iTableID.ToString() + "  ORDER BY DisplayName");
            
            foreach (DataRow dr in dtColumns.Rows)
            {
                ListItem aItem = new ListItem(dr["DisplayName"].ToString(), dr["DisplayName"].ToString());
                ddlDatabaseField.Items.Add(aItem);
            }


            //Work with 1 top level Parent tables.
            DataTable dtPT = Common.DataTableFromText("SELECT distinct ParentTableID FROM TableChild WHERE ChildTableID=" + iTableID.ToString()); //AND DetailPageType<>'not'

            if (dtPT.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPT.Rows)
                {
                    DataTable dtPColumns = Common.DataTableFromText(@"SELECT distinct TableName + ':' + DisplayName AS DP FROM [Column] INNER JOIN [Table]
                                        ON [Column].TableID=[Table].TableID WHERE IsStandard=0 AND TableTableID IS NULL AND  [Column].TableID=" + dr["ParentTableID"].ToString());
                    foreach (DataRow drP in dtPColumns.Rows)
                    {
                        ListItem aItem = new ListItem(drP["DP"].ToString(), drP["DP"].ToString());
                        ddlDatabaseField.Items.Add(aItem);
                    }
                }
            }


        }

        protected void Page_Load(object sender, EventArgs e)
        {


            edtContent.AssetManager = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Editor/assetmanager/assetmanager.aspx";


            if (!IsPostBack)
            {

                PopulateTableDDL();
                //PopulateColour();
                if (DocumentSectionID <= 0)
                {
                    if (Request.QueryString["PrevID"].ToString() != "-1")
                    {
                        hfRemoveSection.Value = "1";
                    }
                }
                else
                {
                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {
                        DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                        if (section != null)
                        {

                            CalendarSectionDetail calDetail = JSONField.GetTypedObject<CalendarSectionDetail>(section.Details);
                            if (calDetail != null)
                            {
                                if (calDetail.TableID != null)
                                {
                                    ddlTable.SelectedValue = calDetail.TableID.ToString();

                                    ddlTable_SelectedIndexChanged(null, null);
                                }

                                txtCalendarTitle.Text = calDetail.CalendarTitle;
                                if (calDetail.ShowAddRecordIcon != null)
                                {
                                    chkShowAddRecordIcon.Checked = (bool)calDetail.ShowAddRecordIcon;
                                }
                                else
                                {
                                    chkShowAddRecordIcon.Checked = false;
                                }

                                edtContent.Text = calDetail.FieldDisplay;

                                if (calDetail.DateFieldColumnID != null)
                                {
                                    //PopulateDateField();
                                    ddlDateField.SelectedValue = calDetail.DateFieldColumnID.ToString();
                                }

                                if (calDetail.CalendarDefaultView != null && calDetail.CalendarDefaultView!="")
                                    radioCalendarDefaultView.SelectedValue = calDetail.CalendarDefaultView;


                                if (calDetail.Width != null)
                                    txtWidth.Text = calDetail.Width.ToString();
                                if (calDetail.Height != null)
                                    txtHeight.Text = calDetail.Height.ToString();

                                if (calDetail.FilterControlInfo != null && calDetail.FilterControlInfo!="")
                                {

                                    string strXML = calDetail.FilterControlInfo;

                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.LoadXml(strXML);

                                    XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

                                    DataSet ds = new DataSet();
                                    ds.ReadXml(r);

                                    if (ds.Tables.Count>0 && ds.Tables[0] != null)
                                    {
                                        bool bCompareOperatorFound = false;
                                        foreach (DataColumn dc in ds.Tables[0].Columns)
                                        {
                                            if (dc.ColumnName.ToLower() == "CompareOperator".ToLower())
                                            {
                                                bCompareOperatorFound = true;
                                              
                                                break;
                                            }
                                        }
                                        if (bCompareOperatorFound == false)
                                        {
                                            ds.Tables[0].Columns.Add("CompareOperator");
                                            ds.Tables[0].AcceptChanges();
                                        }

                                        int i = 1;
                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {

                                            if (i == 1)
                                            {
                                                cbcFilter1.TableID = calDetail.TableID;
                                                cbcFilter1.ddlYAxisV = dr[0].ToString();
                                                cbcFilter1.TextValue = dr[1].ToString();
                                                cbcFilter1.CompareOperator = dr[2].ToString();
                                               
                                            }

                                            if (i == 2)
                                            {
                                                lnkAddFilter1.Visible = false;
                                                trFilter2.Visible = true;
                                                cbcFilter2.TableID = calDetail.TableID;
                                                cbcFilter2.ddlYAxisV = dr[0].ToString();
                                                cbcFilter2.TextValue = dr[1].ToString();
                                                cbcFilter2.CompareOperator = dr[2].ToString();
                                               
                                            }

                                            if (i == 3)
                                            {
                                                lnkAddFilter2.Visible = false;
                                                trFilter3.Visible = true;
                                                cbcFilter3.TableID = calDetail.TableID;
                                                cbcFilter3.ddlYAxisV = dr[0].ToString();
                                                cbcFilter3.TextValue = dr[1].ToString();
                                                cbcFilter3.CompareOperator = dr[2].ToString();
                                                
                                            }
                                           
                                            i = i + 1;
                                        }
                                    }

                                }



                                if (calDetail.TextColourInfo != null && calDetail.TextColourInfo != "")
                                {

                                    PopulateCalendarColor(calDetail.TextColourInfo);
                                    //string strXML = calDetail.TextColourInfo;

                                    //XmlDocument xmlDoc = new XmlDocument();
                                    //xmlDoc.LoadXml(strXML);

                                    //XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

                                    //DataSet ds = new DataSet();
                                    //ds.ReadXml(r);

                                    //if (ds.Tables[0] != null)
                                    //{
                                    //    int i = 1;
                                    //    foreach (DataRow dr in ds.Tables[0].Rows)
                                    //    {

                                    //        if (i == 1)
                                    //        {
                                    //            cbcColour1.TableID = calDetail.TableID;
                                    //            cbcColour1.ddlYAxisV = dr[0].ToString();
                                    //            cbcColour1.TextValue = dr[1].ToString();
                                    //            txtTextColour1.Text = dr[2].ToString();
                                    //        }

                                    //        if (i == 2)
                                    //        {
                                    //            lnkAddColour1.Visible = false;
                                    //            trColour2.Visible = true;
                                    //            cbcColour2.TableID = calDetail.TableID;
                                    //            cbcColour2.ddlYAxisV = dr[0].ToString();
                                    //            cbcColour2.TextValue = dr[1].ToString();
                                    //            txtTextColour2.Text = dr[2].ToString();

                                    //        }

                                    //        if (i == 3)
                                    //        {
                                    //            lnkAddColour2.Visible = false;
                                    //            trColour3.Visible = true;
                                    //            cbcColour3.TableID = calDetail.TableID;
                                    //            cbcColour3.ddlYAxisV = dr[0].ToString();
                                    //            cbcColour3.TextValue = dr[1].ToString();
                                    //            txtTextColour3.Text = dr[2].ToString();

                                    //        }

                                    //        i = i + 1;
                                    //    }
                                    //}

                                }
                                else
                                {
                                    PopulateCalendarColor("");
                                }

                            }
                            
                        }
                        else
                        {
                            //Response.Redirect("../Summary.aspx", true);
                        }
                    }
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


        
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
              
                

                ErrorMessage.Text = "";
                CalendarSectionDetail calDetail = new CalendarSectionDetail();

                calDetail.TableID = int.Parse(ddlTable.SelectedValue);
                calDetail.CalendarTitle = txtCalendarTitle.Text;
                calDetail.ShowAddRecordIcon = chkShowAddRecordIcon.Checked;
                calDetail.DateFieldColumnID = int.Parse(ddlDateField.SelectedValue);

                calDetail.FieldDisplay = edtContent.Text;
                calDetail.CalendarDefaultView = radioCalendarDefaultView.SelectedValue;
                //now need to work on 
                 //calDetail.FilterControlInfo
                //calDetail.FilterTextSearch
                //calDetail.
                calDetail.Width = txtWidth.Text == "" ? null : (int?)int.Parse(txtWidth.Text);
                calDetail.Height = txtHeight.Text == "" ? null : (int?)int.Parse(txtHeight.Text);

                string strAll_FilterTextSearch = "";

                string strFilter1TextSearch=cbcFilter1.TextSearch;
                if (strFilter1TextSearch != "")
                {
                    strAll_FilterTextSearch = "(" + cbcFilter1.TextSearch + ")";
                }
                //if (cbcFilter1.NumericSearch != "" && cbcFilter1.NumericSearch != null)
                //{
                //    strFilterTextSearch = "(" + cbcFilter1.NumericSearch + ")";
                //}

                
                string strFilter2TextSearch=cbcFilter2.TextSearch;
                if (strFilter2TextSearch != "" )
                {
                    if(strAll_FilterTextSearch!="")
                        strAll_FilterTextSearch=strAll_FilterTextSearch+ " AND ";
                    strAll_FilterTextSearch = strAll_FilterTextSearch + "(" + strFilter2TextSearch + ")";
                }
                //if (cbcFilter2.NumericSearch != "" && cbcFilter2.NumericSearch != null)
                //{
                //    if (strFilterTextSearch != "")
                //        strFilterTextSearch = strFilterTextSearch + " AND ";

                //    strFilterTextSearch = strFilterTextSearch + "(" + cbcFilter2.NumericSearch + ")";
                //}


                string strFilter3TextSearch=cbcFilter3.TextSearch;

                if (strFilter3TextSearch != "" )
                {
                    if (strAll_FilterTextSearch != "")
                        strAll_FilterTextSearch = strAll_FilterTextSearch + " AND ";
                    strAll_FilterTextSearch = strAll_FilterTextSearch + "(" + strFilter3TextSearch + ")";
                }
                //if (cbcFilter3.NumericSearch != "" && cbcFilter3.NumericSearch != null)
                //{
                //    if (strAll_FilterTextSearch != "")
                //        strAll_FilterTextSearch = strAll_FilterTextSearch + " AND ";

                //    strAll_FilterTextSearch = strAll_FilterTextSearch + "(" + cbcFilter3.NumericSearch + ")";
                //}


                calDetail.FilterTextSearch = strAll_FilterTextSearch;

                if (strAll_FilterTextSearch != "")
                {
                    string strFilterControlInfo = "<?xml version='1.0' encoding='utf-8' ?><items>";

                    if (cbcFilter1.ddlYAxisV != "" )
                    {
                        strFilterControlInfo = strFilterControlInfo + "<item><columnid>" + HttpUtility.HtmlEncode(cbcFilter1.ddlYAxisV)
                               + "</columnid><value>" + HttpUtility.HtmlEncode(cbcFilter1.TextValue) + "</value><CompareOperator>"
                             + HttpUtility.HtmlEncode(cbcFilter1.CompareOperator) + "</CompareOperator></item>";
                    }

                    if (cbcFilter2.ddlYAxisV != "" )
                    {
                        strFilterControlInfo = strFilterControlInfo + "<item><columnid>" + HttpUtility.HtmlEncode(cbcFilter2.ddlYAxisV)
                               + "</columnid><value>" + HttpUtility.HtmlEncode(cbcFilter2.TextValue) + "</value><CompareOperator>"
                             + HttpUtility.HtmlEncode(cbcFilter2.CompareOperator) + "</CompareOperator></item>";
                    }
                    if (cbcFilter3.ddlYAxisV != "" )
                    {
                        strFilterControlInfo = strFilterControlInfo + "<item><columnid>" + HttpUtility.HtmlEncode(cbcFilter3.ddlYAxisV)
                               + "</columnid><value>" + HttpUtility.HtmlEncode(cbcFilter3.TextValue) + "</value><CompareOperator>"
                             + HttpUtility.HtmlEncode(cbcFilter3.CompareOperator) + "</CompareOperator></item>";
                    }

                    strFilterControlInfo = strFilterControlInfo + "</items>";

                    calDetail.FilterControlInfo = strFilterControlInfo;
                }


                bool bFoundTextColour = false;

                string strTextColour = "<?xml version='1.0' encoding='utf-8' ?><items>";

                string strEnteredTextColour = GetTextColour();

                if(strEnteredTextColour!="")
                {
                    bFoundTextColour = true;
                    strTextColour = strTextColour + strEnteredTextColour;
                }


                //if (cbcColour1.ddlYAxisV != "" && cbcColour1.TextValue != "" && txtTextColour1.Text!="")
                //{
                //    bFoundTextColour = true;
                //    strTextColour = strTextColour + "<item><columnid>" + HttpUtility.HtmlEncode(cbcColour1.ddlYAxisV)
                //              + "</columnid><value>" + HttpUtility.HtmlEncode(cbcColour1.TextValue) + "</value><colour>"
                //              + HttpUtility.HtmlEncode(txtTextColour1.Text) + "</colour></item>";
                //}


                //if (cbcColour2.ddlYAxisV != "" && cbcColour2.TextValue != "" && txtTextColour2.Text != "")
                //{
                //    bFoundTextColour = true;
                //    strTextColour = strTextColour + "<item><columnid>" + HttpUtility.HtmlEncode(cbcColour2.ddlYAxisV)
                //              + "</columnid><value>" + HttpUtility.HtmlEncode(cbcColour2.TextValue) + "</value><colour>"
                //              + HttpUtility.HtmlEncode(txtTextColour2.Text) + "</colour></item>";
                //}

                //if (cbcColour3.ddlYAxisV != "" && cbcColour3.TextValue != "" && txtTextColour3.Text != "")
                //{
                //    bFoundTextColour = true;
                //    strTextColour = strTextColour + "<item><columnid>" + HttpUtility.HtmlEncode(cbcColour3.ddlYAxisV)
                //              + "</columnid><value>" + HttpUtility.HtmlEncode(cbcColour3.TextValue) + "</value><colour>"
                //              + HttpUtility.HtmlEncode(txtTextColour3.Text) + "</colour></item>";
                //}


                if (bFoundTextColour == true)
                {
                    strTextColour = strTextColour + "</items>";
                }
                else
                {
                    strTextColour = "";
                }

                if (strTextColour != "")
                {

                    calDetail.TextColourInfo = strTextColour;
                }

                

               
                try
                {
                    int ID = 0;

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

                            
                            ctx.ExecuteCommand("UPDATE DocumentSection SET Position=Position + 1 WHERE DocumentID={0}  AND Position>{1}", DocumentID.ToString(), (iPosition - 1).ToString());
                            

                            newSection.DocumentID = DocumentID;
                            //newSection.SectionName = txtTitle.Text;
                            newSection.DocumentSectionTypeID = 11; //caledar

                            newSection.Details = calDetail.GetJSONString();
                            newSection.Position = iPosition;
                            newSection.DateAdded = DateTime.Now;
                            newSection.DateUpdated = DateTime.Now;
                            ctx.DocumentSections.InsertOnSubmit(newSection);

                            ctx.SubmitChanges();
                            NewSectionID = newSection.DocumentSectionID;
                            hfRemoveSection.Value = "0";
                            ID = NewSectionID;
                        }



                        //if (ViewState["imagepath"] != null)
                        //{
                        //    File.Copy(Server.MapPath(ViewState["imagepath"].ToString()), Server.MapPath( String.Format("~/Uploaded/ImageSection/{0}.png", NewSectionID)));
                        //}


                    }
                    else
                    {
                        ID = DocumentSectionID;



                        //if (ViewState["imagepath"] != null)
                        //{
                        //    File.Copy( Server.MapPath( ViewState["imagepath"].ToString()), Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", ID)),true);
                        //}


                        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                        {
                            DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == ID);
                            if (section != null)
                            {
                               
                                section.Details = calDetail.GetJSONString();
                            }
                            ctx.SubmitChanges();
                            hfRemoveSection.Value = "0";
                        }


                    }

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + ID.ToString() + ");", true);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }

            }
        }


    





        protected void lnkAddFilter1_Click(object sender, EventArgs e)
        {
            trFilter2.Visible = true;
            lnkAddFilter1.Visible = false;
            //lnkAddFilter2.Visible = true;
           
        }

        protected void lnkAddFilter2_Click(object sender, EventArgs e)
        {

            trFilter3.Visible = true;
            lnkAddFilter2.Visible = false;
           
        }



        protected void lnkMinusFilter2_Click(object sender, EventArgs e)
        {
            lnkAddFilter1.Visible = true;
            trFilter2.Visible = false;
            cbcFilter2.ddlYAxisV = "";

        }

        protected void lnkMinusFilter3_Click(object sender, EventArgs e)
        {

            lnkAddFilter2.Visible = true;

            trFilter3.Visible = false;
            cbcFilter3.ddlYAxisV = "";
        }


        //protected void lnkAddColour1_Click(object sender, EventArgs e)
        //{
        //    trColour2.Visible = true;
        //    lnkAddColour1.Visible = false;
        //    //lnkAddColour2.Visible = true;
           
        //}

        //protected void lnkAddColour2_Click(object sender, EventArgs e)
        //{

        //    trColour3.Visible = true;
        //    lnkAddColour2.Visible = false;
            
           
        //}


        //protected void lnkMinusColour2_Click(object sender, EventArgs e)
        //{
        //    lnkAddColour1.Visible = true;
        //    trColour2.Visible = false;
        //    cbcColour2.ddlYAxisV = "";

        //}

        //protected void lnkMinusColour3_Click(object sender, EventArgs e)
        //{

        //    lnkAddColour2.Visible = true;

        //    trColour3.Visible = false;
        //    cbcColour3.ddlYAxisV = "";
        //}



        protected void cbcFilter1_OnddlYAxis_Changed(object sender, EventArgs e)
        {
            if(cbcFilter1.ddlYAxisV=="")
            {
                lnkAddFilter1.Visible=false;
            }
            else
            {
                lnkAddFilter1.Visible = true;
            }

        }

        protected void cbcFilter2_OnddlYAxis_Changed(object sender, EventArgs e)
        {
            if (cbcFilter2.ddlYAxisV == "")
            {
                lnkAddFilter2.Visible = false;
            }
            else
            {
                lnkAddFilter2.Visible = true;
            }
        }

        protected void cbcFilter3_OnddlYAxis_Changed(object sender, EventArgs e)
        {

        }



        //protected void cbcColour1_OnddlYAxis_Changed(object sender, EventArgs e)
        //{
        //    if (cbcColour1.ddlYAxisV == "")
        //    {
        //        lnkAddColour1.Visible = false;
        //    }
        //    else
        //    {
        //        lnkAddColour1.Visible = true;
        //    }
        //}

        //protected void cbcColour2_OnddlYAxis_Changed(object sender, EventArgs e)
        //{
        //    if (cbcColour2.ddlYAxisV == "")
        //    {
        //        lnkAddColour2.Visible = false;
        //    }
        //    else
        //    {
        //        lnkAddColour2.Visible = true;
        //    }
        //}

        //protected void cbcColour3_OnddlYAxis_Changed(object sender, EventArgs e)
        //{

        //}



        protected void PopulateTableDDL()
        {
            int iTN = 0;
            ddlTable.DataSource = RecordManager.ets_Table_Select(null,
                    null,
                    null,
                    int.Parse(Session["AccountID"].ToString()),
                    null, null, true,
                    "st.TableName", "ASC",
                    null, null, ref  iTN, Session["STs"].ToString());

            ddlTable.DataBind();
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
            ddlTable.Items.Insert(0, liSelect);
        }

        protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDateField();
            PopulateDatabaseField();

            if (ddlTable.SelectedValue != "")
            {
                tblDateField.Visible = true;
                trDisplayText.Visible = true;
                trFilter.Visible = true;
                trColourText.Visible = true;

                cbcFilter1.TableID = int.Parse(ddlTable.SelectedValue);
                cbcFilter1.ClearYAxis();
                cbcFilter1.ddlYAxisV = "";


                cbcFilter2.TableID = int.Parse(ddlTable.SelectedValue);
                cbcFilter2.ClearYAxis();
                cbcFilter2.ddlYAxisV = "";

                cbcFilter3.TableID = int.Parse(ddlTable.SelectedValue);
                cbcFilter1.ClearYAxis();
                cbcFilter1.ddlYAxisV = "";

                //cbcColour1.TableID = int.Parse(ddlTable.SelectedValue);
                //cbcColour1.ClearYAxis();
                //cbcColour1.ddlYAxisV = "";


                //cbcColour2.TableID = int.Parse(ddlTable.SelectedValue);
                //cbcColour2.ClearYAxis();
                //cbcColour2.ddlYAxisV = "";

                //cbcColour3.TableID = int.Parse(ddlTable.SelectedValue);
                //cbcColour3.ClearYAxis();
                //cbcColour3.ddlYAxisV = "";

                PopulateCalendarColor("");


            }
            else
            {
                tblDateField.Visible = false;
                trDisplayText.Visible = false;
                trFilter.Visible = false;
                trColourText.Visible = false;


                cbcFilter1.TableID = null;
                cbcFilter1.ClearYAxis();
                cbcFilter2.TableID = null;
                cbcFilter2.ClearYAxis();
                cbcFilter3.TableID = null;
                cbcFilter3.ClearYAxis();
                edtContent.Text = "";

                //cbcColour1.TableID = null;
                //cbcColour1.ClearYAxis();
                //cbcColour2.TableID = null;
                //cbcColour2.ClearYAxis();
                //cbcColour3.TableID = null;
                //cbcColour3.ClearYAxis();

               
                //txtTextColour1.Text = "";
                //txtTextColour1.Text = "";
                //txtTextColour1.Text = "";

            }

            trFilter2.Visible = false;
            trFilter3.Visible = false;
            lnkAddFilter1.Visible = false;

            //trColour2.Visible = false;
            //trColour3.Visible = false;
            //lnkAddColour1.Visible = false;

        }

        //protected void PopulateColour()
        //{
        //    Dictionary<string,string> dicColour=new Dictionary<string,string>();
           
        //    dicColour.Add("Black","000000");
        //    dicColour.Add("White","FFFFFF");
        //     dicColour.Add("Silver","C0C0C0");
        //     dicColour.Add("Gray","808080");
             
        //     dicColour.Add("Red","FF0000");
        //     dicColour.Add("Maroon","800000");
        //     dicColour.Add("Yellow","FFFF00");
        //     dicColour.Add("Olive","808000");
        //     dicColour.Add("Lime","00FF00");
        //     dicColour.Add("Green","008000");
        //    dicColour.Add("Aqua","00FFFF");
        //     dicColour.Add("Teal","008080");
        //     dicColour.Add("Blue","0000FF");
        //     dicColour.Add("Navy","000080");
        //     dicColour.Add("Fuchsia","FF00FF");
        //     dicColour.Add("Purple","800080");
            
        //    foreach(var pair in dicColour)
        //    {
        //        ListItem liTemp=new ListItem(pair.Key,pair.Value);
        //        txtTextColour1.Items.Add(liTemp);

        //    }
        //     foreach(var pair in dicColour)
        //    {
        //        ListItem liTemp=new ListItem(pair.Key,pair.Value);
        //        txtTextColour2.Items.Add(liTemp);

        //    }
        //     foreach(var pair in dicColour)
        //    {
        //        ListItem liTemp=new ListItem(pair.Key,pair.Value);
        //        txtTextColour3.Items.Add(liTemp);

        //    }


        //}

        protected void PopulateDateField()
        {

            ddlDateField.Items.Clear();

            if (ddlTable.SelectedValue != "")
            {   
                ddlDateField.DataSource = Common.DataTableFromText("SELECT ColumnID,DisplayName FROM [Column] WHERE TableID=" +
                    ddlTable.SelectedValue + " AND (ColumnType='date' OR ColumnType='datetime')");
                ddlDateField.DataBind();
            }

            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
            ddlDateField.Items.Insert(0, liSelect);
        }




        protected void CBCColorYAxisChanged(object sender, EventArgs e)
        {
            Pages_UserControl_ControlByColumn cbcColour = sender as Pages_UserControl_ControlByColumn;

            if (cbcColour != null)
            {
                GridViewRow row = cbcColour.NamingContainer as GridViewRow;
                Label lblID = row.FindControl("lblID") as Label;
                ImageButton imgbtnMinus = row.FindControl("imgbtnMinus") as ImageButton;
                ImageButton imgbtnPlus = row.FindControl("imgbtnPlus") as ImageButton;
                DropDownList ddlTextColour = row.FindControl("ddlTextColour") as DropDownList;

                if (cbcColour.ddlYAxisV == "")
                {
                    imgbtnPlus.Visible = false;
                }
                else
                {
                    imgbtnPlus.Visible = true;
                }
            }


        }

        protected void grdCalendarColor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblID = e.Row.FindControl("lblID") as Label;
                    ImageButton imgbtnMinus = e.Row.FindControl("imgbtnMinus") as ImageButton;
                    ImageButton imgbtnPlus = e.Row.FindControl("imgbtnPlus") as ImageButton;
                    DropDownList ddlTextColour = e.Row.FindControl("ddlTextColour") as DropDownList;
                    //imgbtnPlus.CommandArgument = e.Row.RowIndex.ToString();

                    Pages_UserControl_ControlByColumn cbcColour = e.Row.FindControl("cbcColour") as Pages_UserControl_ControlByColumn;
                    cbcColour.TableID = int.Parse(ddlTable.SelectedValue);
                    cbcColour.ColumnTypeOut = "'file','image','calculation'";
                    cbcColour.ddlYAxisV = DataBinder.Eval(e.Row.DataItem, "columnid").ToString();
                    cbcColour.TextValue = DataBinder.Eval(e.Row.DataItem, "value").ToString();
                    if (DataBinder.Eval(e.Row.DataItem, "CompareOperator") != null)
                        cbcColour.CompareOperator = DataBinder.Eval(e.Row.DataItem, "CompareOperator").ToString();

                    if (DataBinder.Eval(e.Row.DataItem, "colour").ToString() != "")
                        ddlTextColour.SelectedValue = DataBinder.Eval(e.Row.DataItem, "colour").ToString();

                    lblID.Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                    imgbtnMinus.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                    imgbtnPlus.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ID").ToString();


                }
            }
            catch
            {
                //
            }
        }

        protected string GetTextColour()
        {
            string strTextColour = "";
            if (grdCalendarColor.Rows.Count > 0)
            {
                foreach (GridViewRow gvRow in grdCalendarColor.Rows)
                {
                  
                    Label lblID = gvRow.FindControl("lblID") as Label;
                    DropDownList ddlTextColour = gvRow.FindControl("ddlTextColour") as DropDownList;
                    Pages_UserControl_ControlByColumn cbcColour = gvRow.FindControl("cbcColour") as Pages_UserControl_ControlByColumn;
                   
                    //dtCalenderdarColor.Rows.Add(lblID.Text.ToString(), cbcColour.ddlYAxisV, cbcColour.TextValue, ddlTextColour.SelectedValue);

                    if(cbcColour.ddlYAxisV!="" &&  ddlTextColour.SelectedValue!="")
                    {
                        strTextColour = strTextColour + "<item><columnid>" + HttpUtility.HtmlEncode(cbcColour.ddlYAxisV)
                             + "</columnid><value>" + HttpUtility.HtmlEncode(cbcColour.TextValue) + "</value><colour>"
                             + HttpUtility.HtmlEncode(ddlTextColour.SelectedValue) + "</colour><CompareOperator>"
                             + HttpUtility.HtmlEncode(cbcColour.CompareOperator) + "</CompareOperator></item>";
                    }
                    
                }

            }
            return strTextColour;

        }
        protected void SetCalendarColorRowData()
        {

            if (ViewState["dtCalenderdarColor"] != null)
            {
                DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];
                dtCalenderdarColor.Rows.Clear();

                int iID = 0;
                if (grdCalendarColor.Rows.Count > 0)
                {
                    foreach (GridViewRow gvRow in grdCalendarColor.Rows)
                    {

                        //gvRow.FindControl()
                        Label lblID = gvRow.FindControl("lblID") as Label;
                        DropDownList ddlTextColour = gvRow.FindControl("ddlTextColour") as DropDownList;
                        Pages_UserControl_ControlByColumn cbcColour = gvRow.FindControl("cbcColour") as Pages_UserControl_ControlByColumn;
                        //dtCalenderdarColor.Rows.Add(iID, cbcColour.ddlYAxisV, cbcColour.TextValue, ddlTextColour.SelectedValue);
                        dtCalenderdarColor.Rows.Add(lblID.Text.ToString(), cbcColour.ddlYAxisV, cbcColour.TextValue, ddlTextColour.SelectedValue, cbcColour.CompareOperator);
                        iID = iID + 1;
                    }

                }
                dtCalenderdarColor.AcceptChanges();
                ViewState["dtCalenderdarColor"] = dtCalenderdarColor;


            }
        }
        protected void PopulateCalendarColor(string strTextColourInfo)
        {
            if (strTextColourInfo != "")
            {
                string strXML = strTextColourInfo;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strXML);

                XmlTextReader r = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

                DataSet ds = new DataSet();
                ds.ReadXml(r);


                if (ds.Tables[0] != null)
                {
                    bool bCompareOperatorFound = false;
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        if(dc.ColumnName.ToLower()=="CompareOperator".ToLower())
                        {
                            bCompareOperatorFound = true;
                            break;
                        }
                    }

                    if (bCompareOperatorFound==false)
                    {
                        ds.Tables[0].Columns.Add("CompareOperator");
                    }
                    ds.Tables[0].Columns.Add("ID");

                    ds.Tables[0].AcceptChanges();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["ID"] = Guid.NewGuid().ToString();
                    }
                    ds.Tables[0].AcceptChanges();

                    grdCalendarColor.DataSource = ds.Tables[0];
                    grdCalendarColor.DataBind();

                }
            }
            else
            {
                if (ViewState["dtCalenderdarColor"] == null)
                {
                    DataTable dtCalenderdarColor = new DataTable();
                    dtCalenderdarColor.Columns.Add("ID");
                    //DataColumn dc = dtCalenderdarColor.Columns.Add("ID", typeof(int));
                    //dc.AutoIncrement = true;
                    //dc.AutoIncrementSeed = 1;
                    //dc.AutoIncrementStep = 1;


                    dtCalenderdarColor.Columns.Add("columnid");
                    dtCalenderdarColor.Columns.Add("value");
                    dtCalenderdarColor.Columns.Add("colour");
                    dtCalenderdarColor.Columns.Add("CompareOperator");
                    dtCalenderdarColor.AcceptChanges();

                    //dtCalenderdarColor.Rows.Add(0,"", "","");

                    dtCalenderdarColor.Rows.Add(Guid.NewGuid().ToString(), "", "", "","");

                    grdCalendarColor.DataSource = dtCalenderdarColor;
                    grdCalendarColor.DataBind();

                    ViewState["dtCalenderdarColor"] = dtCalenderdarColor;
                }
                else
                {
                    DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];
                    grdCalendarColor.DataSource = dtCalenderdarColor;
                    grdCalendarColor.DataBind();
                }


            }
        }


        protected void grdCalendarColor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                SetCalendarColorRowData();

                if (e.CommandName == "minus")
                {
                    if (ViewState["dtCalenderdarColor"] != null)
                    {
                        DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];

                        for (int i = dtCalenderdarColor.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtCalenderdarColor.Rows[i];
                            if (dr["id"].ToString() == e.CommandArgument.ToString())
                            {
                                dr.Delete();
                                break;
                            }

                        }


                        dtCalenderdarColor.AcceptChanges();

                        ViewState["dtCalenderdarColor"] = dtCalenderdarColor;

                        PopulateCalendarColor("");
                        //PopulatePreviousColor();


                    }
                }
                if (e.CommandName == "plus")
                {
                    if (ViewState["dtCalenderdarColor"] != null)
                    {
                        DataTable dtCalenderdarColor = (DataTable)ViewState["dtCalenderdarColor"];

                        //dtCalenderdarColor.Rows.Add(int.Parse(e.CommandArgument.ToString())+1,  "", "","");

                        //dtCalenderdarColor.Rows.Add(Guid.NewGuid().ToString(),"", "", "");
                        int iPos = 0;

                        for (int i = 0; i < dtCalenderdarColor.Rows.Count; i++)
                        {
                            if (dtCalenderdarColor.Rows[i][0].ToString() == e.CommandArgument.ToString())
                            {
                                iPos = i;
                                break;
                            }
                        }
                        //for (int i = 0 dtCalenderdarColor.Rows.Count - 1; i >= 0; i--)
                        //{
                        //    DataRow dr = dtCalenderdarColor.Rows[i];
                        //    if (dr["id"].ToString() == e.CommandArgument.ToString())
                        //    {
                        //        iPos = i;
                        //        break;
                        //    }

                        //}


                        DataRow newRow = dtCalenderdarColor.NewRow();
                        newRow[0] = Guid.NewGuid().ToString();
                        newRow[1] = "";
                        newRow[2] = "";
                        newRow[3] = "";
                        newRow[4] = "";


                        dtCalenderdarColor.Rows.InsertAt(newRow, iPos + 1);

                        dtCalenderdarColor.AcceptChanges();

                        ViewState["dtCalenderdarColor"] = dtCalenderdarColor;

                        PopulateCalendarColor("");
                        //PopulatePreviousColor();

                        //grdCalendarColor.Rows.
                    }

                }
            }
            catch
            {
                //
            }
        }





    }
}


