using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;
using DocGen.Utility;
using System.Data;

namespace DocGen.Document.STTableSection
{
    public partial class Edit : SecurePage
    {
        string _strNumericSearch = "";
        string _strTextSearch = "";
        string _strGridViewSortColumn = "DBGSystemRecordID";
        string _strGridViewSortDirection = "DESC";

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

        protected void PopulateTerminology()
        {
            //lblLocationCap.Text = SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), lblLocationCap.Text, lblLocationCap.Text);

            stgFields.InnerText = stgFields.InnerText.Replace("Fields", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Fields", "Fields"));
            stgTableCap.InnerText = stgTableCap.InnerText.Replace("Table", SecurityManager.etsTerminology(Request.Path.Substring(Request.Path.LastIndexOf("/") + 1), "Table", "Table"));


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                   
                    int DocumentID1 = 0;
                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID1);
                    DAL.Document doc1 = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID1);
                    if (doc1 != null)
                    {
                        if (doc1.ForDashBoard != null)
                        {
                            if ((bool)doc1.ForDashBoard)
                            {
                                trDateRecorded.Visible = false;
                                trRecentDays.Visible = true;
                                stgUseReportDate.InnerText = "Use Recent Dates";

                            }

                        }
                    }

                    PopulateTableDDL();
                    string strFilterSystemName = "";
                    DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                    if (section != null)
                    {
                        CheckPermission(section.DocumentID);
                        //txtTitle.Text = section.SectionName;
                        TableSectionOtherInfo tblOtherInfo = JSONField.GetTypedObject<TableSectionOtherInfo>(section.ValueFields);


                       if (!String.IsNullOrEmpty(section.Filter))
                        {
                            TableSectionFilter filter = JSONField.GetTypedObject<TableSectionFilter>(section.Filter);
                           
                            foreach (SPInputParam item in filter.Params)
                            {
                                switch (item.Name)
                                {
                                    case "@nTableID":
                                        ddlSamleType.Text = item.Value.ToString();
                                        break;

                                    case "@dDateFrom":
                                        txtDateFrom.Text =((DateTime)item.Value).ToShortDateString();// item.Value.ToString();
                                        break;
                                    case "@dDateTo":
                                        txtDateTo.Text = ((DateTime)item.Value).ToShortDateString();//item.Value.ToString();
                                        break;

                                }
                            }

                            //PopulateLocationList();
                            PopulateFilterYAxis();
                            PopulateSortOrder();

                            if (filter.MaxRow != null)
                                txtMaxRows.Text = filter.MaxRow.ToString();

                            foreach (SPInputParam item in filter.Params)
                            {
                                switch (item.Name)
                                {

                                    //case "@sLocations":

                                    //    string strSSIDs = (string)item.Value;
                                    //    if (strSSIDs != "")
                                    //    {
                                    //        string[] strSS = strSSIDs.Split(',');
                                    //        foreach (string SS in strSS)
                                    //        {
                                    //            lstLocation.Items.FindByValue(SS).Selected = true;
                                    //        }
                                    //    }

                                    //    break;

                                    case "@sOrder":
                                        string sOrder = (string)item.Value;
                                        if (sOrder.IndexOf(" ASC") > -1)
                                        {
                                            ddlOrderDirection.Text = "ASC";
                                            sOrder = sOrder.Substring(0, sOrder.Length - 4);

                                        }
                                        else
                                        {
                                            ddlOrderDirection.Text = "DESC";
                                            sOrder = sOrder.Substring(0, sOrder.Length - 5);
                                        }

                                        sOrder = sOrder.Trim();
                                        //sOrder = sOrder.Substring(1);
                                        //sOrder = sOrder.Substring(0, sOrder.Length - 1);
                                        //sOrder is SystemName)
                                        DataTable dtSCs = RecordManager.ets_Table_Columns_Summary(int.Parse(ddlSamleType.Text), null);

                                        foreach (DataRow dr in dtSCs.Rows)
                                        {
                                            if (sOrder.IndexOf( dr["DisplayTextSummary"].ToString())>-1)
                                            {
                                                ddlOrderYAxis.Text = dr["ColumnID"].ToString();
                                            }
                                        }


                                        break;
                                    case "@sNumericSearch":
                                        string sNumericSearch = (string)item.Value;
                                        DataTable dtSCs2 = RecordManager.ets_Table_Columns_Summary(int.Parse(ddlSamleType.Text), null);

                                        foreach (DataRow dr in dtSCs2.Rows)
                                        {
                                            if (sNumericSearch.IndexOf("Record." + dr["SystemName"].ToString()) > -1)
                                            {
                                                ddlFilterYAxis.Text = dr["ColumnID"].ToString();
                                                strFilterSystemName = dr["SystemName"].ToString();
                                            }
                                        }
                                        ddlYAxis_SelectedIndexChanged(null, null);
                                        if (sNumericSearch.IndexOf("CONVERT(decimal(20,10)") > -1)
                                        {
                                            string strMin = "";
                                            string strMax = "";
                                            if (sNumericSearch.IndexOf(">= CONVERT(decimal(20,10),") > -1)
                                            {
                                                strMin = sNumericSearch.Replace("Record." + strFilterSystemName + " >= CONVERT(decimal(20,10),", "");
                                                txtLowerLimit.Text = strMin.Substring(0, strMin.IndexOf(")") );
                                            }


                                            if (sNumericSearch.IndexOf("<= CONVERT(decimal(20,10),") > -1)
                                            {
                                                if (strMin == "")
                                                {
                                                    //only max
                                                    strMax = sNumericSearch.Replace("Record." + strFilterSystemName + " <= CONVERT(decimal(20,10),", "");
                                                    txtUpperLimit.Text = strMax.Substring(0, strMax.IndexOf(")") - 1);
                                                }
                                                else
                                                {
                                                    //both min and max
                                                    strMin = strMin.Substring(strMin.IndexOf(" AND"));

                                                    strMax = strMin.Replace(" AND Record." + strFilterSystemName + " <= CONVERT(decimal(20,10),", "");
                                                    txtUpperLimit.Text = strMax.Substring(0, strMax.IndexOf(")") );
                                                }
                                            }

                                        }



                                        break;
                                    case "@sTextSearch":

                                        string sTextSearch = (string)item.Value;
                                        DataTable dtSCs3 = RecordManager.ets_Table_Columns_Summary(int.Parse(ddlSamleType.Text), null);

                                        foreach (DataRow dr in dtSCs3.Rows)
                                        {
                                            if (sTextSearch.IndexOf("Record." + dr["SystemName"].ToString()) > -1)
                                            {
                                                ddlFilterYAxis.Text = dr["ColumnID"].ToString();
                                                strFilterSystemName = dr["SystemName"].ToString();
                                            }
                                        }
                                        ddlYAxis_SelectedIndexChanged(null, null);
                                        if (sTextSearch.IndexOf(" like'%") > -1)
                                        {
                                            sTextSearch = sTextSearch.Replace("Record." + strFilterSystemName + " like'%", "");
                                            txtSearchText.Text = sTextSearch.Substring(0, sTextSearch.IndexOf("%'"));
                                        }


                                        break;
                                }

                            }

                           


                        }
                        if (!String.IsNullOrEmpty(section.Details))
                        {
                            if(tblOtherInfo.TableType!="")
                            {
                                ddlTableType.SelectedValue = tblOtherInfo.TableType;
                                if (tblOtherInfo.TableType == "system")
                                {
                                    ddlSystemTable.SelectedValue = tblOtherInfo.SystemTableName;
                                }
                            }
                            
                            if (tblOtherInfo.TableType != "system")
                            {

                                TableSectionDetails tsc = JSONField.GetTypedObject<TableSectionDetails>(section.Details);
                                gvColumns.DataSource = tsc.Columns;
                                gvColumns.DataBind();
                            }
                          
                            //ltTest.Text = SectionGenerator.GenerateTableSection(section);
                            ddlTableType_SelectedIndexChanged(null, null);
                        }

                        if (!String.IsNullOrEmpty(section.ValueFields))
                        {
                           
                            if (tblOtherInfo.ShowTimeWithDate != null)
                            {
                                chkShowTime.Checked = (bool)tblOtherInfo.ShowTimeWithDate;
                            }

                            if (tblOtherInfo.IsUseReportDate != null)
                            {
                                if ((bool)tblOtherInfo.IsUseReportDate)
                                {
                                    chkUseReportDates.Checked = true;
                                    ShowHideDates(false);
                                }
                                else
                                {
                                    chkUseReportDates.Checked = false;
                                    ShowHideDates(false);
                                }

                            }

                            if (doc1.ForDashBoard != null)
                            {
                                if (tblOtherInfo.RecentDays != null)
                                {
                                    txtRecentDays.Text = tblOtherInfo.RecentDays.ToString();
                                }
                            }


                        }


                        //CancelButton.CommandArgument = section.DocumentID.ToString();
                    }
                    else
                    {
                        if (Request.QueryString["PrevID"].ToString() != "-1")
                        {
                            hfRemoveSection.Value = "1";
                        }
                        //new
                        if (Request.QueryString["DocumentID"] != null)
                        {

                            int DocumentID = 0;
                            Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);
                            DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);
                            if (doc != null)
                            {
                                if (doc.ForDashBoard != null)
                                {
                                    if ((bool)doc.ForDashBoard)
                                    {
                                        trDateRecorded.Visible = false;
                                        trRecentDays.Visible = true;
                                        stgUseReportDate.InnerText = "Use Recent Dates";

                                    }
                                }

                                if (doc.DocumentDate != null)
                                {
                                    txtDateFrom.Text = doc.DocumentDate.Value.ToShortDateString();
                                }
                                if (doc.DocumentEndDate != null)
                                {
                                    txtDateTo.Text = doc.DocumentEndDate.Value.ToShortDateString();
                                }
                                       
                            }
                        }
                        ShowHideDates(true);

                        //Response.Redirect("../Summary.aspx", true);
                    }
                }

                PopulateTerminology();
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

        protected void ddlSystemTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbTest_Click(null, null);

        }
        protected void ddlTableType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlTableType.SelectedValue == "Table")
            {
                //trLocations.Visible = true;
                trRecentDays.Visible = true;
                trTableTalbe.Visible = true;
                trShowTime.Visible = true;
                trSortOrder.Visible = true;
                trSystemTable.Visible = false;
                trSystemTable.Visible = false;
                pColumns.Visible = true;
                trFilter.Visible = true;
                trDateRecorded.Visible = true;
                trUseReportDate.Visible = true;
                global::Document doc1 = DocumentManager.ets_Document_Detail(int.Parse(Request.QueryString["DocumentID"]));
                if (doc1 != null)
                {
                    if (doc1.ForDashBoard != null)
                    {
                        if ((bool)doc1.ForDashBoard)
                        {
                            trDateRecorded.Visible = false;
                            trRecentDays.Visible = true;

                        }
                        else
                        {
                            trDateRecorded.Visible = true;
                            trRecentDays.Visible = false;
                        }

                    }
                    else
                    {

                        trDateRecorded.Visible = true;
                        trRecentDays.Visible = false;
                    }
                }
            }
            else
            {
            
                trSystemTable.Visible=true;
                //trLocations.Visible = false;
                trRecentDays.Visible = false;
                trTableTalbe.Visible = false;
                trShowTime.Visible = false;
                trSortOrder.Visible = false;                
                pColumns.Visible = false;
                trFilter.Visible = false;
                trDateRecorded.Visible = false;
                trUseReportDate.Visible = false;

            }

            lbTest_Click(null, null);
        }
        protected void ddlSamleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PopulateLocationList();
            PopulateFilterYAxis();
            PopulateSortOrder();

            List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();
            int Counter = 1;

            DataTable dtSCs = RecordManager.ets_Table_Columns_Summary(int.Parse(ddlSamleType.Text), null);

            foreach (DataRow dr in dtSCs.Rows)
            {
                if (dr["DisplayTextSummary"].ToString() != "")
                {
                    lstColumns.Add(new TableSectionColumn()
                    {

                        SystemName = dr["DisplayTextSummary"].ToString(),
                        DisplayName = dr["DisplayTextSummary"].ToString(),
                        Visible = true,
                        Position = Counter++,
                        Bold = false,
                        Italic = false,
                        Underline = false,
                        Alignment = "left"
                    });
                }
            }

            gvColumns.DataSource = lstColumns;
            gvColumns.DataBind();


        }
        //protected void ddlStoredProcedure_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string SPName = ddlStoredProcedure.SelectedValue;
        //    List<SPInputParam> lstParams = null;
        //    if (SPName != "")
        //    {
        //        lstParams = DBUtil.GetSPInputParams(SPName);
        //        using (DAL.DocGenDataContext ctx = new DocGenDataContext())
        //        {
        //            DAL.DataMap dmap = ctx.DataMaps.SingleOrDefault<DAL.DataMap>(d => d.StoredProcedureName == SPName);
        //            List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();
        //            int Counter = 1;
        //            foreach (string rField in dmap.ReturnFields.Split(','))
        //            {
        //                lstColumns.Add(new TableSectionColumn()
        //                {
        //                    SystemName = rField.Trim(),
        //                    DisplayName = rField.Trim(),
        //                    Visible = true,
        //                    Position = Counter++,
        //                    Bold = false,
        //                    Italic = false,
        //                    Underline = false,
        //                    Alignment = "left"
        //                });
        //            }
        //            gvColumns.DataSource = lstColumns;
        //            gvColumns.DataBind();
        //        }
        //        gvParams.DataSource = lstParams;
        //        gvParams.DataBind();
        //    }
        //    else
        //    {
        //        gvParams.DataSource = null;
        //        gvParams.DataBind();

        //        gvColumns.DataSource = null;
        //        gvColumns.DataBind();
        //    }
        //    ltTest.Text = "";
        //}

        //protected void gvParams_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItemIndex >= 0)
        //    {
        //        SPInputParam p = (SPInputParam)e.Row.DataItem;
        //        TextBox txtValue = (TextBox)e.Row.FindControl("txtValue");
        //        txtValue.CssClass = "Param_" + p.DataType;
        //        if (p.MaxCharLength > 0)
        //        {
        //            txtValue.MaxLength = p.MaxCharLength;
        //        }
        //    }
        //}

        public List<TableSectionColumn> GetUploadsColumns()
        {
            List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "BatchID",
                DisplayName = " ",
                Visible = true,
                Position = 1,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "TableID",
                DisplayName = "TableID",
                Visible = false,
                Position = 1,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "TableName",
                DisplayName = SecurityManager.etsTerminology("", "Table", "Table"),
                Visible = true,
                Position = 1,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "DateAdded",
                DisplayName = "Date Uploaded",
                Visible = true,
                Position = 2,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "ValidCount",
                DisplayName = "Valid",
                Visible = true,
                Position = 3,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "WarningCount",
                DisplayName = "Warning",
                Visible = true,
                Position = 4,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "NotValidCount",
                DisplayName = "Invalid",
                Visible = true,
                Position = 5,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            return lstColumns;
        }


        protected List<TableSectionColumn> GetAlertsColumns()
        {
            List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "LastWarningTime",
                DisplayName = "Last Time",
                Visible = true,
                Position = 1,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "TableName",
                DisplayName = SecurityManager.etsTerminology("", "Table", "Table"),
                Visible = true,
                Position = 2,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "WarningCount",
                DisplayName = "Warning",
                Visible = true,
                Position = 3,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            lstColumns.Add(new TableSectionColumn()
            {
                SystemName = "TableID",
                DisplayName = "TableID",
                Visible = false,
                Position = 4,
                Bold = false,
                Italic = false,
                Underline = false,
                Alignment = "left"
            });

            

            return lstColumns;
        }
        protected void lbTest_Click(object sender, EventArgs e)
        {
            ErrorMessage.Text = "";
            //if (ddlStoredProcedure.SelectedIndex > 0)
            if (ddlSamleType.SelectedIndex > 0 && ddlTableType.SelectedValue=="Table")
            {
                TableSectionDetails tsc = new TableSectionDetails();
                List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();
                DAL.DocumentSection section = new DocumentSection();
                foreach (GridViewRow r in gvColumns.Rows)
                {
                    CheckBox chkSelect = (CheckBox)r.FindControl("chkVisible");
                    TextBox txtSystemName = (TextBox)r.FindControl("txtSystemName");
                    TextBox txtDisplayName = (TextBox)r.FindControl("txtDisplayName");
                    CheckBox chkBold = (CheckBox)r.FindControl("chkBold");
                    CheckBox chkItalic = (CheckBox)r.FindControl("chkItalic");
                    CheckBox chkUnderline = (CheckBox)r.FindControl("chkUnderline");
                    DropDownList ddlAlignment = (DropDownList)r.FindControl("ddlAlignment");
                    lstColumns.Add(new TableSectionColumn()
                    {
                        SystemName = txtSystemName.Text,
                        DisplayName = txtDisplayName.Text,
                        Visible = chkSelect.Checked,
                        Position = r.RowIndex + 1,
                        Bold = chkBold.Checked,
                        Italic = chkItalic.Checked,
                        Underline = chkUnderline.Checked,
                        Alignment = ddlAlignment.SelectedValue
                    });
                }
                tsc.Columns = lstColumns;
                section.Details = tsc.GetJSONString();


                TableSectionOtherInfo tblOtherInfo = new TableSectionOtherInfo();
                tblOtherInfo.IsUseReportDate = chkUseReportDates.Checked;

                tblOtherInfo.ShowTimeWithDate = chkShowTime.Checked;

                if (txtRecentDays.Text.Trim() != "")
                {
                    tblOtherInfo.RecentDays = int.Parse(txtRecentDays.Text);
                }
                else
                {
                    tblOtherInfo.RecentDays = null;
                }

                

                section.ValueFields = tblOtherInfo.GetJSONString();
                int DocumentID = 0;
                Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);
                section.DocumentID = DocumentID;
                TableSectionFilter filter = new TableSectionFilter();

                if (txtMaxRows.Text.Trim() != "")
                {
                    filter.MaxRow = int.Parse(txtMaxRows.Text.Trim());
                    //filter.ForActualReport = false;
                }
                else
                {
                    filter.MaxRow = 10;
                    //filter.ForActualReport = null;
                }


                bool ValidParams = true;
                //filter.SPName = ddlStoredProcedure.SelectedValue;
                filter.SPName = "ets_Record_List";
                filter.Params = GetParamValues(out ValidParams);
                section.Filter = filter.GetJSONString();
                if (ValidParams)
                    ltTest.Text = SectionGenerator.GenerateTableSection(section);
                else
                    ltTest.Text = "<table border=\"1\" class=\"TableSection\" width=\"100%\"><tr><td>Invalid parameter</td></tr></table>";
            }
            else if (ddlSamleType.SelectedIndex == 0 && ddlTableType.SelectedValue == "Table")
            {
                ltTest.Text = "<table border=\"1\" class=\"TableSection\" width=\"100%\"><tr><td>Please select a Table </td></tr></table>";
                ddlSamleType.Focus();
            }
            else if (ddlTableType.SelectedValue == "system")
            {
                TableSectionDetails tsc = new TableSectionDetails();
                List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();
                DAL.DocumentSection section = new DocumentSection();
                TableSectionFilter filter = new TableSectionFilter();
                if (ddlSystemTable.SelectedValue == "uploads")
                {
                    lstColumns = GetUploadsColumns();
                    filter.SPName = "ets_Recent_Uploads";  
                }

                if (ddlSystemTable.SelectedValue == "alerts")
                {
                    lstColumns = GetAlertsColumns();
                    filter.SPName = "ets_Recent_Alerts";  
                }

                tsc.Columns = lstColumns;
                section.Details = tsc.GetJSONString();

                TableSectionOtherInfo tblOtherInfo = new TableSectionOtherInfo();

                tblOtherInfo.TableType = ddlTableType.SelectedValue;
                tblOtherInfo.SystemTableName = ddlSystemTable.SelectedValue;
                tblOtherInfo.ShowTimeWithDate = true;
                section.ValueFields = tblOtherInfo.GetJSONString();
                int DocumentID = 0;
                Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);
                section.DocumentID = DocumentID;                

                List<SPInputParam> lstParams = new List<SPInputParam>();
                SPInputParam pST = new SPInputParam();
                pST.Name = "@nAccountID";
                pST.DataType = "int";
                pST.Value = AccountID;
                lstParams.Add(pST);
                filter.Params = lstParams;
                section.Filter = filter.GetJSONString();

                ltTest.Text = SectionGenerator.GenerateTableSection(section);
            }




        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {                
                ErrorMessage.Text = "";
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

                            bool ValidParams = true;
                            DAL.DocumentSection newSection = new DAL.DocumentSection();

                            ctx.ExecuteCommand("UPDATE DocumentSection SET Position=Position + 1 WHERE DocumentID={0}  AND Position>{1}", DocumentID.ToString(), (iPosition - 1).ToString());
                            
                            newSection.DocumentID = DocumentID;
                            newSection.DocumentSectionTypeID = 6; //Table Table
                            
                            newSection.Position = iPosition;
                            newSection.DateAdded = DateTime.Now;
                            newSection.DateUpdated = DateTime.Now;

                            TableSectionDetails tsc = new TableSectionDetails();
                            List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();
                            TableSectionFilter filter = new TableSectionFilter();
                            TableSectionOtherInfo tblOtherInfo = new TableSectionOtherInfo();
                            tblOtherInfo.TableType = ddlTableType.SelectedValue;
                            if (ddlTableType.SelectedValue == "system")
                            {                                
                                tblOtherInfo.SystemTableName = ddlSystemTable.SelectedValue;
                                tblOtherInfo.ShowTimeWithDate = true;

                                if (ddlSystemTable.SelectedValue == "uploads")
                                {
                                    newSection.Content = "ets_Recent_Uploads";
                                    lstColumns = GetUploadsColumns();
                                    filter.SPName = "ets_Recent_Uploads";                                    
                                }

                                if (ddlSystemTable.SelectedValue == "alerts")
                                {
                                    newSection.Content = "ets_Recent_Alerts";
                                    lstColumns = GetAlertsColumns();
                                    filter.SPName = "ets_Recent_Alerts";
                                }

                                List<SPInputParam> lstParams = new List<SPInputParam>();
                                SPInputParam pST = new SPInputParam();
                                pST.Name = "@nAccountID";
                                pST.DataType = "int";
                                pST.Value = AccountID;
                                lstParams.Add(pST);
                                filter.Params = lstParams;
                            }

                            if (ddlTableType.SelectedValue != "system")
                            {
                                newSection.Content = "ets_Record_List";
                                foreach (GridViewRow r in gvColumns.Rows)
                                {
                                    CheckBox chkSelect = (CheckBox)r.FindControl("chkVisible");
                                    TextBox txtSystemName = (TextBox)r.FindControl("txtSystemName");
                                    TextBox txtDisplayName = (TextBox)r.FindControl("txtDisplayName");
                                    CheckBox chkBold = (CheckBox)r.FindControl("chkBold");
                                    CheckBox chkItalic = (CheckBox)r.FindControl("chkItalic");
                                    CheckBox chkUnderline = (CheckBox)r.FindControl("chkUnderline");
                                    DropDownList ddlAlignment = (DropDownList)r.FindControl("ddlAlignment");
                                    lstColumns.Add(new TableSectionColumn()
                                    {
                                        SystemName = txtSystemName.Text,
                                        DisplayName = txtDisplayName.Text,
                                        Visible = chkSelect.Checked,
                                        Position = r.RowIndex + 1,
                                        Bold = chkBold.Checked,
                                        Italic = chkItalic.Checked,
                                        Underline = chkUnderline.Checked,
                                        Alignment = ddlAlignment.SelectedValue
                                    });
                                }

                                filter.SPName = "ets_Record_List";
                                filter.Params = GetParamValues(out ValidParams);


                                tblOtherInfo.IsUseReportDate = chkUseReportDates.Checked;
                                tblOtherInfo.ShowTimeWithDate = chkShowTime.Checked;

                                if (txtRecentDays.Text.Trim() != "")
                                {
                                    tblOtherInfo.RecentDays = int.Parse(txtRecentDays.Text);
                                }
                                else
                                {
                                    tblOtherInfo.RecentDays = null;
                                }
                                if (txtMaxRows.Text.Trim() != "")
                                {
                                    filter.MaxRow = int.Parse(txtMaxRows.Text.Trim());
                                }
                                else
                                {
                                    filter.MaxRow = null;
                                }

                            }
                            tsc.Columns = lstColumns;
                            newSection.Details = tsc.GetJSONString(); 
                            newSection.Filter = filter.GetJSONString(); 
                            newSection.ValueFields = tblOtherInfo.GetJSONString();
                            ctx.DocumentSections.InsertOnSubmit(newSection);
                            if (ValidParams)
                            {
                                ctx.SubmitChanges();
                                NewSectionID = newSection.DocumentSectionID;
                                ID = NewSectionID;
                                hfRemoveSection.Value = "0";
                            }
                            else
                            {
                                ErrorMessage.Text = "Invalid parameter";
                                return;
                            }

                        }

                    }
                    else
                    {
                        

                        ID = DocumentSectionID;
                        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                        {
                            DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == ID);
                            bool ValidParams = true;
                            if (section != null)
                            {
                                
                               
                                TableSectionDetails tsc = new TableSectionDetails();
                                List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();
                                TableSectionFilter filter = new TableSectionFilter();
                                TableSectionOtherInfo tblOtherInfo = new TableSectionOtherInfo();

                                tblOtherInfo.TableType = ddlTableType.SelectedValue;
                                if (ddlTableType.SelectedValue == "system")
                                {
                                    tblOtherInfo.SystemTableName = ddlSystemTable.SelectedValue;
                                    tblOtherInfo.ShowTimeWithDate = true;

                                    if (ddlSystemTable.SelectedValue == "uploads")
                                    {
                                        section.Content = "ets_Recent_Uploads";
                                        lstColumns = GetUploadsColumns();
                                        filter.SPName = "ets_Recent_Uploads";
                                    }

                                    if (ddlSystemTable.SelectedValue == "alerts")
                                    {
                                        section.Content = "ets_Recent_Alerts";
                                        lstColumns = GetAlertsColumns();
                                        filter.SPName = "ets_Recent_Alerts";
                                    }

                                    List<SPInputParam> lstParams = new List<SPInputParam>();
                                    SPInputParam pST = new SPInputParam();
                                    pST.Name = "@nAccountID";
                                    pST.DataType = "int";
                                    pST.Value = AccountID;
                                    lstParams.Add(pST);
                                    filter.Params = lstParams;
                                }

                                if (ddlTableType.SelectedValue != "system")
                                {
                                    section.Content = "ets_Record_List";
                                    foreach (GridViewRow r in gvColumns.Rows)
                                    {
                                        CheckBox chkSelect = (CheckBox)r.FindControl("chkVisible");
                                        TextBox txtSystemName = (TextBox)r.FindControl("txtSystemName");
                                        TextBox txtDisplayName = (TextBox)r.FindControl("txtDisplayName");
                                        CheckBox chkBold = (CheckBox)r.FindControl("chkBold");
                                        CheckBox chkItalic = (CheckBox)r.FindControl("chkItalic");
                                        CheckBox chkUnderline = (CheckBox)r.FindControl("chkUnderline");
                                        DropDownList ddlAlignment = (DropDownList)r.FindControl("ddlAlignment");
                                        lstColumns.Add(new TableSectionColumn()
                                        {
                                            SystemName = txtSystemName.Text,
                                            DisplayName = txtDisplayName.Text,
                                            Visible = chkSelect.Checked,
                                            Position = r.RowIndex + 1,
                                            Bold = chkBold.Checked,
                                            Italic = chkItalic.Checked,
                                            Underline = chkUnderline.Checked,
                                            Alignment = ddlAlignment.SelectedValue
                                        });
                                    }

                                    filter.SPName = "ets_Record_List";
                                    filter.Params = GetParamValues(out ValidParams);


                                    tblOtherInfo.IsUseReportDate = chkUseReportDates.Checked;
                                    tblOtherInfo.ShowTimeWithDate = chkShowTime.Checked;

                                    if (txtRecentDays.Text.Trim() != "")
                                    {
                                        tblOtherInfo.RecentDays = int.Parse(txtRecentDays.Text);
                                    }
                                    else
                                    {
                                        tblOtherInfo.RecentDays = null;
                                    }

                                    if (txtMaxRows.Text.Trim() != "")
                                    {
                                        filter.MaxRow = int.Parse(txtMaxRows.Text.Trim());
                                    }
                                    else
                                    {
                                        filter.MaxRow = null;
                                    }

                                }
                                tsc.Columns = lstColumns;
                                section.Details = tsc.GetJSONString();
                                section.Filter = filter.GetJSONString();
                                section.ValueFields = tblOtherInfo.GetJSONString();


                            }


                            if (ValidParams)
                            {
                                ctx.SubmitChanges();
                                hfRemoveSection.Value = "0";
                            }
                            else
                            {
                                ErrorMessage.Text = "Invalid parameter";
                            }
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

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("../Edit.aspx?DocumentID=" + CancelButton.CommandArgument);
        //}

        protected void gvColumns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex >= 0)
            {
                TableSectionColumn colSetting = (TableSectionColumn)e.Row.DataItem;
                CheckBox chkVisible = (CheckBox)e.Row.FindControl("chkVisible");
                CheckBox chkBold = (CheckBox)e.Row.FindControl("chkBold");
                CheckBox chkItalic = (CheckBox)e.Row.FindControl("chkItalic");
                CheckBox chkUnderline = (CheckBox)e.Row.FindControl("chkUnderline");
                LinkButton lbDown = (LinkButton)e.Row.FindControl("lbDown");
                DropDownList ddlAlignment = (DropDownList)e.Row.FindControl("ddlAlignment");
                chkVisible.Checked = colSetting.Visible;
                chkBold.Checked = colSetting.Bold;
                chkItalic.Checked = colSetting.Italic;
                chkUnderline.Checked = colSetting.Italic;
                if (ddlAlignment.Items.FindByValue(colSetting.Alignment) != null)
                    ddlAlignment.SelectedValue = colSetting.Alignment;

                if (e.Row.DataItemIndex > 0)
                {
                    LinkButton lbUp = (LinkButton)e.Row.FindControl("lbUp");
                    LinkButton lbDown_Prev = (LinkButton)gvColumns.Rows[e.Row.RowIndex - 1].FindControl("lbDown");
                    lbUp.Visible = true;
                    lbDown_Prev.Visible = true;
                    lbDown_Prev.CommandArgument = lbUp.CommandArgument = e.Row.DataItemIndex.ToString();
                }
            }
        }

      
        protected List<SPInputParam> GetParamValues(out bool Success)
        {
            Success = true;
            List<SPInputParam> lstParams = new List<SPInputParam>();
            SPInputParam pST = new SPInputParam();
            pST.Name = "@nTableID";
            pST.DataType = "int";
            pST.Value = Convert.ToInt32(ddlSamleType.Text);
            lstParams.Add(pST);


            SPInputParam pbIsActive = new SPInputParam();
            pbIsActive.Name = "@bIsActive";
            pbIsActive.DataType = "bit";
            pbIsActive.Value = 1;
            lstParams.Add(pbIsActive);

            //SPInputParam pSS = new SPInputParam();
            //pSS.Name = "@sLocations";
            //pSS.DataType = "varchar";
            //pSS.Value = GetLocationIDs();
            //lstParams.Add(pSS);

            if (txtDateFrom.Text.Trim() != "")
            {
                SPInputParam ptxtDateFrom = new SPInputParam();
                ptxtDateFrom.Name = "@dDateFrom";
                ptxtDateFrom.DataType = "datetime";
                ptxtDateFrom.Value = ConvertUtil.GetDate(txtDateFrom.Text).ToLocalTime(); 
                lstParams.Add(ptxtDateFrom);
            }

            if (txtDateTo.Text.Trim() != "")
            {
                SPInputParam ptxtDateTo = new SPInputParam();
                ptxtDateTo.Name = "@dDateTo";
                ptxtDateTo.DataType = "datetime";
                ptxtDateTo.Value = ConvertUtil.GetDate(txtDateTo.Text).ToLocalTime();
                lstParams.Add(ptxtDateTo);
            }

            PopulateSearchParams();

            if (ddlFilterYAxis.Text != "")
            {
                PopulateSearchParams();
                if (_strNumericSearch != "")
                {
                    SPInputParam p_strNumericSearch = new SPInputParam();
                    p_strNumericSearch.Name = "@sNumericSearch";
                    p_strNumericSearch.DataType = "varchar";

                    if (_strNumericSearch.Trim().Substring(0, 1) == "O")
                    {
                        _strNumericSearch = _strNumericSearch.Substring(3);
                    }
                    else
                    {
                        _strNumericSearch = _strNumericSearch.Substring(4);
                    }

                    p_strNumericSearch.Value = _strNumericSearch;
                    lstParams.Add(p_strNumericSearch);

                }
                if (_strTextSearch != "")
                {
                    SPInputParam p_strTextSearch = new SPInputParam();
                    p_strTextSearch.Name = "@sTextSearch";
                    p_strTextSearch.DataType = "varchar";

                    if (_strTextSearch.Trim().Substring(0, 1) == "O")
                    {
                        _strTextSearch = _strTextSearch.Substring(3);
                    }
                    else
                    {
                        _strTextSearch = _strTextSearch.Substring(4);
                    }

                    p_strTextSearch.Value = _strTextSearch;
                    lstParams.Add(p_strTextSearch);

                }

            }

            MakeSortColumn();

            SPInputParam psOrder = new SPInputParam();
            psOrder.Name = "@sOrder";
            psOrder.DataType = "varchar";
            psOrder.Value = GetsOrder(_strGridViewSortColumn, ddlOrderDirection.Text);
            lstParams.Add(psOrder);


            //foreach (GridViewRow r in gvParams.Rows)
            //{
            //    Label lblError = (Label)r.FindControl("lblError");
            //    lblError.Visible = false;
            //    try
            //    {
            //        TextBox txtValue = (TextBox)r.FindControl("txtValue");                    
            //        SPInputParam p = new SPInputParam();
            //        p.Name = r.Cells[0].Text;
            //        p.DataType = txtValue.CssClass.Split('_')[1];
            //        switch (p.DataType)
            //        {
            //            //Exact Numerics
            //            case "bigint":
            //                p.Value = Convert.ToInt64(txtValue.Text);
            //                break;
            //            case "bit":
            //                switch (txtValue.Text.ToLower())
            //                {
            //                    case "yes":
            //                    case "true":
            //                    case "male":
            //                    case "1":
            //                    case "y":
            //                        p.Value = 1;
            //                        break;
            //                    default:
            //                        p.Value = 0;
            //                        break;
            //                }
            //                break;
            //            case "decimal":
            //            case "money":
            //            case "numeric":
            //            case "smallmoney":
            //                p.Value = Convert.ToDecimal(txtValue.Text);
            //                break;
            //            case "int":
            //                p.Value = Convert.ToInt32(txtValue.Text);
            //                break;
            //            case "smallint":
            //                p.Value = Convert.ToInt16(txtValue.Text);
            //                break;
            //            case "tinyint":
            //                p.Value = Convert.ToByte(txtValue.Text);
            //                break;

            //            //Approximate Numerics
            //            case "float":
            //                p.Value = Convert.ToDouble(txtValue.Text);
            //                break;
            //            case "real":
            //                p.Value = Convert.ToSingle(txtValue.Text);
            //                break;

            //            //Date and Time
            //            case "date":
            //            case "datetime":
            //                p.Value = ConvertUtil.GetDate(txtValue.Text);
            //                break;

            //            //Character Strings
            //            case "char":
            //            case "varchar":
            //            case "nchar":
            //            case "nvarchar":
            //            case "text":
            //            case "ntext":
            //                p.Value = txtValue.Text;
            //                break;

            //        }
            //        lstParams.Add(p);
            //    }
            //    catch
            //    {
            //        lblError.Visible = true;
            //        Success = false;
            //    }
            //}
            return lstParams;
        }

        protected string GetsOrder(string sOrder, string sOrderDirection)
        {
            if (sOrder.IndexOf("VERT(decimal", 0) > 0)
            {
                return sOrder + sOrderDirection;
            }
            else
            {
                return "[" + sOrder + "] " + sOrderDirection;
            }
        }


        protected void MakeSortColumn()
        {
            if (ddlOrderYAxis.Text != "")
                _strGridViewSortColumn = ddlOrderYAxis.SelectedItem.Text;

            DataTable _dtRecordColums = RecordManager.ets_Table_Columns_Summary(
                 int.Parse(ddlSamleType.Text), null);

            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
            {
                if (_dtRecordColums.Rows[i]["ColumnID"].ToString() == ddlOrderYAxis.Text)
                {
                    if (_dtRecordColums.Rows[i]["ColumnType"].ToString()=="number")
                    {
                        if (!bool.Parse(_dtRecordColums.Rows[i]["IgnoreSymbols"].ToString()))
                        {
                            _strGridViewSortColumn = "CONVERT(decimal(20,10), [" + _dtRecordColums.Rows[i]["DisplayTextSummary"].ToString() + "]) ";

                        }
                        // return;
                    }
                }
            }


        }


        protected void PopulateSearchParams()
        {
            if (ddlFilterYAxis.SelectedValue == "")
            {
                _strNumericSearch = "";
                _strTextSearch = "";
            }
            else
            {
                Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlFilterYAxis.SelectedValue));

                if (theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false)
                {
                    _strTextSearch = "";
                    _strNumericSearch = "";
                    if (txtLowerLimit.Text != "")
                    {
                        _strNumericSearch = " AND Record." + theColumn.SystemName + " >= CONVERT(decimal(20,10)," + txtLowerLimit.Text.Trim() + ")";
                    }

                    if (txtUpperLimit.Text != "")
                    {
                        _strNumericSearch = _strNumericSearch + " AND Record." + theColumn.SystemName + " <= CONVERT(decimal(20,10)," + txtUpperLimit.Text.Trim() + ")";
                    }

                }
                else
                {
                    _strNumericSearch = "";
                    _strTextSearch = "";

                    if (txtSearchText.Text != "")
                    {
                        _strTextSearch = " AND Record." + theColumn.SystemName + " like'%" + txtSearchText.Text.Trim().Replace("'", "''") + "%'";
                    }

                }
            }
        }

        //protected string GetLocationIDs()
        //{
        //    string strLocationIDs = "";

        //    foreach (ListItem item in lstLocation.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            strLocationIDs = strLocationIDs + item.Value + ",";
        //        }
        //    }

        //    if (strLocationIDs == "")
        //    {
        //        foreach (ListItem item in lstLocation.Items)
        //        {

        //            strLocationIDs = strLocationIDs + item.Value + ",";

        //        }
        //    }

        //    if (strLocationIDs.Length > 0)
        //    {
        //        strLocationIDs = strLocationIDs.Substring(0, strLocationIDs.Length - 1);
        //    }
        //    return strLocationIDs;

        //}

        protected void gvColumns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int i = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "MoveDown":
                case "MoveUp":
                    List<TableSectionColumn> lstColumns = new List<TableSectionColumn>();
                    TableSectionColumn temp = new TableSectionColumn();

                    foreach (GridViewRow r in gvColumns.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)r.FindControl("chkVisible");
                        TextBox txtSystemName = (TextBox)r.FindControl("txtSystemName");
                        TextBox txtDisplayName = (TextBox)r.FindControl("txtDisplayName");
                        CheckBox chkBold = (CheckBox)r.FindControl("chkBold");
                        CheckBox chkItalic = (CheckBox)r.FindControl("chkItalic");
                        CheckBox chkUnderline = (CheckBox)r.FindControl("chkUnderline");
                        DropDownList ddlAlignment = (DropDownList)r.FindControl("ddlAlignment");
                        lstColumns.Add(new TableSectionColumn()
                        {
                            SystemName = txtSystemName.Text,
                            DisplayName = txtDisplayName.Text,
                            Visible = chkSelect.Checked,
                            Position = r.RowIndex + 1,
                            Bold = chkBold.Checked,
                            Italic = chkItalic.Checked,
                            Underline = chkUnderline.Checked,
                            Alignment = ddlAlignment.SelectedValue
                        });
                    }
                    temp.SystemName = lstColumns[i].SystemName;
                    temp.DisplayName = lstColumns[i].DisplayName;
                    temp.Visible = lstColumns[i].Visible;
                    temp.Position = lstColumns[i].Position;
                    temp.Bold = lstColumns[i].Bold;
                    temp.Italic = lstColumns[i].Italic;
                    temp.Underline = lstColumns[i].Underline;
                    temp.Alignment = lstColumns[i].Alignment;

                    lstColumns[i].SystemName = lstColumns[i - 1].SystemName;
                    lstColumns[i].DisplayName = lstColumns[i - 1].DisplayName;
                    lstColumns[i].Visible = lstColumns[i - 1].Visible;
                    lstColumns[i].Position = lstColumns[i - 1].Position;
                    lstColumns[i].Bold = lstColumns[i - 1].Bold;
                    lstColumns[i].Italic = lstColumns[i - 1].Italic;
                    lstColumns[i].Underline = lstColumns[i - 1].Underline;
                    lstColumns[i].Alignment = lstColumns[i - 1].Alignment;

                    lstColumns[i - 1].SystemName = temp.SystemName;
                    lstColumns[i - 1].DisplayName = temp.DisplayName;
                    lstColumns[i - 1].Visible = temp.Visible;
                    lstColumns[i - 1].Position = temp.Position;
                    lstColumns[i - 1].Bold = temp.Bold;
                    lstColumns[i - 1].Italic = temp.Italic;
                    lstColumns[i - 1].Underline = temp.Underline;
                    lstColumns[i - 1].Alignment = temp.Alignment;
                    gvColumns.DataSource = lstColumns;
                    gvColumns.DataBind();
                    break;
            }
        }

        protected void ParamsValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool ValidParams = true;
            GetParamValues(out ValidParams);
            args.IsValid = ValidParams;
        }


        protected void PopulateTableDDL()
        {
            int iTN = 0;
            ddlSamleType.DataSource = RecordManager.ets_Table_Select(null,
                    null,
                    null,
                    int.Parse(Session["AccountID"].ToString()),
                    null, null, true,
                    "st.TableName", "ASC",
                    null, null, ref  iTN, Session["STs"].ToString());

            ddlSamleType.DataBind();
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Please Select--", "");
            ddlSamleType.Items.Insert(0, liSelect);
        }
        //protected void PopulateLocationList()
        //{
        //    if (ddlSamleType.Text != "")
        //    {
        //        int iTN = 0;
        //        lstLocation.DataSource = SiteManager.ets_Location_Select(null,
        //            int.Parse(ddlSamleType.Text), null, "",
        //            "",  true, null, null, null, null,
        //            int.Parse(Session["AccountID"].ToString()),
        //            "LocationName", "ASC", null, null, ref iTN, "");
        //        lstLocation.DataBind();
        //    }

        //}

        protected void PopulateFilterYAxis()
        {
            ddlFilterYAxis.Items.Clear();
            if (ddlSamleType.Text != "")
            {

                DataTable dtSCs = RecordManager.ets_Table_Columns_Summary(int.Parse(ddlSamleType.Text), null);

                foreach (DataRow dr in dtSCs.Rows)
                {
                    if (bool.Parse(dr["IsStandard"].ToString()) == false)
                    {
                        System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(dr["DisplayTextSummary"].ToString(), dr["ColumnID"].ToString());

                        ddlFilterYAxis.Items.Insert(ddlFilterYAxis.Items.Count, aItem);
                    }

                }

                System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("--None--", "");

                ddlFilterYAxis.Items.Insert(0, fItem);
            }

        }

        protected void PopulateSortOrder()
        {
            ddlOrderYAxis.Items.Clear();

            if (ddlSamleType.Text != "")
            {

                DataTable dtSCs = RecordManager.ets_Table_Columns_Summary(int.Parse(ddlSamleType.Text), null);

                foreach (DataRow dr in dtSCs.Rows)
                {
                    //if (bool.Parse(dr["IsStandard"].ToString()) == false)
                    //{
                    if (dr["DisplayTextSummary"].ToString() != "")
                    {
                        System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(dr["DisplayTextSummary"].ToString(), dr["ColumnID"].ToString());

                        ddlOrderYAxis.Items.Insert(ddlOrderYAxis.Items.Count, aItem);
                    }
                    //}

                }

                System.Web.UI.WebControls.ListItem fItem = new System.Web.UI.WebControls.ListItem("--None--", "");

                ddlOrderYAxis.Items.Insert(0, fItem);
            }

        }


        protected void chkUseReportDates_CheckedChanged(Object sender, EventArgs args)
        {

            ShowHideDates(true);

            if (chkUseReportDates.Checked)
            {
                txtRecentDays.Enabled = true;
            }
            else
            {
                txtRecentDays.Text = "";
                txtRecentDays.Enabled = false;
            }

            lbTest_Click(null, null);

        }


        protected void ShowHideDates(bool bDefaultDate)
        {
            int DocumentID = 0;
            Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);

            if (chkUseReportDates.Checked)
            {
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                    DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);
                    if (doc != null)
                    {
                        txtDateFrom.Enabled = false;
                        txtDateTo.Enabled = false;
                       
                        if (doc.DocumentDate != null)
                        {
                            txtDateFrom.Text = doc.DocumentDate.Value.ToShortDateString();
                        }
                        if (doc.DocumentEndDate != null)
                        {
                            txtDateTo.Text = doc.DocumentEndDate.Value.ToShortDateString();
                        }
                    }
                }


            }
            else
            {
                using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                {
                    DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID);
                    if (doc != null)
                    {
                        txtDateFrom.Enabled = true;
                        txtDateTo.Enabled = true;
                       
                        if (bDefaultDate)                        
                        {
                           
                            if (doc.DocumentDate != null)
                            {
                                txtDateFrom.Text = doc.DocumentDate.Value.ToShortDateString();
                            }
                            if (doc.DocumentEndDate != null)
                            {
                                txtDateTo.Text = doc.DocumentEndDate.Value.ToShortDateString();
                            }
                        }
                    }
                }


            }


        }




        protected void ddlYAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //do the show hide

            if (ddlFilterYAxis.SelectedValue == "")
            {
                txtLowerLimit.Visible = false;
                txtUpperLimit.Visible = false;
                lblTo.Visible = false;
                txtSearchText.Visible = false;
            }
            else
            {
                Column theColumn = RecordManager.ets_Column_Details(int.Parse(ddlFilterYAxis.SelectedValue));

                if (theColumn.ColumnType == "number" && theColumn.IgnoreSymbols == false)
                {
                    txtLowerLimit.Visible = true;
                    txtUpperLimit.Visible = true;
                    lblTo.Visible = true;
                    txtSearchText.Visible = false;
                }
                else
                {
                    txtLowerLimit.Visible = false;
                    txtUpperLimit.Visible = false;
                    lblTo.Visible = false;
                    txtSearchText.Visible = true;
                }
                txtLowerLimit.Text = "";
                txtUpperLimit.Text = "";
                txtSearchText.Text = "";

            }

        }



    }
}