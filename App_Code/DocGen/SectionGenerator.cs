using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

namespace DocGen.DAL
{
    public class SectionGenerator
    {
        public static string GenerateTextSection(DAL.DocumentSection section, ref StringBuilder sbTOC)
        {
            StringBuilder sb = new StringBuilder();
            if (section.SectionName == "{TOC}")
            {
                sb.Append("<div id=\"divTOC\"><a name=\"TOC\"></a>{TOC}</div>");
            }
            using (DAL.DocGenDataContext ctx = new DocGenDataContext())
            {
                var texts = from t in ctx.DocumentSections
                            where t.DocumentSectionID == section.DocumentSectionID
                            orderby t.Position
                            select t;

                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == section.DocumentID );

                var textStyles = from ts in ctx.DocumentSectionStyles
                                 where ts.AccountID == doc.AccountID
                                 select ts;

                Dictionary<int, string> dicStyle = new Dictionary<int, string>();

                foreach (DAL.DocumentSectionStyle tStyle in textStyles)
                {
                    dicStyle.Add(tStyle.DocumentSectionStyleID, tStyle.StyleName);
                }


                foreach (DAL.DocumentSection text in texts)
                {
                    string StyleName = "";
                    if (dicStyle.ContainsKey((int)text.DocumentSectionStyleID))
                        StyleName = dicStyle[(int)text.DocumentSectionStyleID];
                    if (text.Content == "{TOC}")
                    {
                        sb.Append("<div id=\"divTOC\"><a name=\"TOC\"></a>{TOC}</div>");
                    }
                    else
                    {
                        switch (StyleName)
                        {
                            case "Heading 1":
                            case "Heading 2":
                            case "Heading 3":
                            case "Heading 4":
                            case "Heading 5":
                            case "Heading 6":
                                string n = StyleName.Split(' ')[1];
                                sb.Append(String.Format("<h{0}><a name=\"_Toc{1}\"></a><span class=\"DOCGEN_TextStyle_{2}\">{3}</span></h{0}>",
                                    n,
                                    text.DocumentSectionID,
                                    text.DocumentSectionStyleID,
                                    HttpUtility.HtmlEncode(text.Content)));
                                sbTOC.AppendLine(GetTOCItem(sbTOC.ToString().Contains("MsoHyperlink"), n, HttpUtility.HtmlEncode(text.Content), text.DocumentSectionID));
                                break;
                            default:
                                sb.Append(String.Format("<p class=\"DOCGEN_TextStyle_{0}\">", text.DocumentSectionStyleID));
                                sb.Append(HttpUtility.HtmlEncode(text.Content));
                                sb.Append("</p>");
                                break;
                        }
                    }
                }
            }
            return Common.FixCrLfAndOtherNonPrint( sb.ToString());
        }

        public static string GetTOCItem(bool HasItem, string Level, string Text, int RefID)
        {            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("	<p class=\"MsoToc{0}\" style='tab-stops: right dotted 467.5pt'>");
            if (!HasItem)
            {
                sb.AppendLine(@"<!--[if supportFields]><span style='mso-element:field-begin'></span><span style='mso-spacerun:yes'> </span>TOC \o &quot;1-6&quot; \h \z \u <span style='mso-element:field-separator'></span><![endif]-->");
            }
            sb.AppendLine("		<span class=\"MsoHyperlink\">");
            sb.AppendLine("			<span style='mso-no-proof: yes'>");
            sb.AppendLine("				<a href=\"#_Toc{2}\">{1}");
            sb.AppendLine("					<span style='color: windowtext; display: none; mso-hide: screen; text-decoration: none; text-underline: none'>");
            sb.AppendLine("						<span style='mso-tab-count: 1 dotted'>. </span>");
            sb.AppendLine("					</span>");
            sb.AppendLine("					<!--[if supportFields]>");
            sb.AppendLine("					<span style='color:windowtext;display:none;mso-hide:screen;text-decoration:none; text-underline:none'>");
            sb.AppendLine("						<span style='mso-element:field-begin'></span>");
            sb.AppendLine("					</span>");
            sb.AppendLine("					<span style='color:windowtext;display:none;mso-hide:screen;text-decoration:none; text-underline:none'> PAGEREF _Toc{2} \\h </span>");
            sb.AppendLine("					<span style='color:windowtext; display:none;mso-hide:screen;text-decoration:none;text-underline:none'>");
            sb.AppendLine("						<span style='mso-element:field-separator'></span>");
            sb.AppendLine("					</span>");
            sb.AppendLine("					<![endif]-->");
            sb.AppendLine("					<span style='color: windowtext; display: none; mso-hide: screen; text-decoration: none; text-underline: none'>1</span>");
            sb.AppendLine("					<span style='color: windowtext; display: none; mso-hide: screen; text-decoration: none; text-underline: none'></span>");
            sb.AppendLine("					<!--[if supportFields]>");
            sb.AppendLine("					<span style='color:windowtext; display:none;mso-hide:screen;text-decoration:none;text-underline:none'>");
            sb.AppendLine("						<span style='mso-element:field-end'></span>");
            sb.AppendLine("					</span>");
            sb.AppendLine("					<![endif]-->");
            sb.AppendLine("				</a>");
            sb.AppendLine("			</span>");
            sb.AppendLine("		</span>");
            sb.AppendLine("		<span style='mso-no-proof: yes'><o:p></o:p></span>");
            sb.AppendLine("	</p>");
            return String.Format(sb.ToString(), Level, Text, RefID);
        }
        public static string GenerateImageSection(DAL.DocumentSection section)
        {
            HttpContext hContext = HttpContext.Current;
            StringBuilder sb = new StringBuilder();
            string CSSPosition = "";
            ImageSectionStyle imageStyle = JSONField.GetTypedObject<ImageSectionStyle>(section.Details);
            if (imageStyle == null)
            {
                imageStyle = new ImageSectionStyle()
                {
                    Position = "left",
                    Width = 0
                };
            }

            string strStartTagA = "";
            string strEndTagA = "";
            if(string.IsNullOrEmpty(imageStyle.OpenLink)==false)
            {
                string strOpenLink = imageStyle.OpenLink;
                if(strOpenLink.ToLower().IndexOf("[userrecordid]")>-1)
                {
                    try
                    {
                        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UserRecordID"] != null)
                        {
                            strOpenLink = strOpenLink.Replace("[UserRecordID]", Cryptography.Encrypt(System.Web.HttpContext.Current.Session["UserRecordID"].ToString()));
                        }
                        else
                        {
                            strOpenLink = strOpenLink.Replace("[UserRecordID]", Cryptography.Encrypt("-1"));

                        }
                    }
                    catch
                    {
                        strOpenLink = strOpenLink.Replace("[UserRecordID]", Cryptography.Encrypt("-1"));
                    }
                    
                }

                strStartTagA = "<a  href=\"" + strOpenLink + "\">";
                strEndTagA = "</a>";
            }

            switch (imageStyle.Position)
            {
                case "left":
                    sb.Append(String.Format("<figure>" + strStartTagA + "<img src=\"{5}/Uploaded/ImageSection/{1}.png\" alt=\"{2}\" width=\"{3}\"/>" + strEndTagA + "<p>{4}</p></figure>",
                        CSSPosition,
                        section.DocumentSectionID,
                        section.SectionName,
                        imageStyle.Width > 0 ? imageStyle.Width.ToString() : "100%",
                        section.Content,
                            "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath 
                        ));
                    break;
                case "center":
                    sb.Append(String.Format("<figure><center>" + strStartTagA + "<img src=\"{5}/Uploaded/ImageSection/{1}.png\" alt=\"{2}\" width=\"{3}\"/>" + strEndTagA + "<p>{4}</p></center></figure>",
                    CSSPosition,
                    section.DocumentSectionID,
                    section.SectionName,
                    imageStyle.Width > 0 ? imageStyle.Width.ToString() : "100%",
                    section.Content,
                        "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath 
                    ));
                    break;
                case "right":
                    sb.Append(String.Format("<div><figure style='float:right;'>" + strStartTagA + "<img src=\"{5}/Uploaded/ImageSection/{1}.png\" alt=\"{2}\" width=\"{3}\"/>" + strEndTagA + "<p>{4}</p></figure><div style=\"clear:both\"></div></div>",
                    CSSPosition,
                    section.DocumentSectionID,
                    section.SectionName,
                    imageStyle.Width > 0 ? imageStyle.Width.ToString() : "100%",
                    section.Content,
                        "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath 
                    ));
                    break;
            }
            return sb.ToString();
        }

        public static string GenerateHTMLSection(DAL.DocumentSection section)
        {
            string content = section.Content;
            if (content == null)
                content = "";

            Regex rResourceFile = new Regex("\"/Uploaded/");
            content = rResourceFile.Replace(content, "\"" + "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/Uploaded/");
            if (!String.IsNullOrEmpty(section.Filter))
            {
                //THIS PART IS NOT NEEDED
                HTMLSectionFilter datasource = JSONField.GetTypedObject<HTMLSectionFilter>(section.Filter);
                if (datasource != null)
                {
                    string Message = "";
                    DataTable dt = DBUtil.ExecuteSP(datasource.SPName, datasource.Params, out Message);
                    if (Message != "")
                    {
                        content = "ERROR: " + Message;
                    }
                    else
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Regex rMergeField = new Regex(@"\[[^\]]*\]");
                            MatchCollection mc = rMergeField.Matches(content);
                            string FieldName = "";
                            string FieldValue = "";
                            foreach (Match m in mc)
                            {
                                FieldName = m.Value.Substring(1, m.Value.Length - 2);
                                if (dt.Columns.Contains(FieldName))
                                {
                                    switch (dt.Columns[FieldName].DataType.ToString())
                                    {
                                        case "System.DateTime":
                                            FieldValue = (dt.Rows[0][FieldName] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0][FieldName]).ToString("MM/dd/yyyy") : "__/__/____");
                                            break;
                                        default:
                                            FieldValue = Convert.ToString(dt.Rows[0][FieldName]);
                                            break;
                                    }
                                }
                                content = content.Replace(m.Value, FieldValue);
                            }
                        }
                    }
                }
            }
            return content;
        }

        public static string GenerateTableSection(DAL.DocumentSection section)
        {
            StringBuilder sb = new StringBuilder();
            TableSectionDetails sectionDetail = JSONField.GetTypedObject<TableSectionDetails>(section.Details);
            TableSectionFilter sectionFilter = JSONField.GetTypedObject<TableSectionFilter>(section.Filter);
            TableSectionOtherInfo tblOtherInfo = JSONField.GetTypedObject<TableSectionOtherInfo>(section.ValueFields);

           

            if (sectionDetail != null)
            {
                sb.Append("<table border=\"1\" class=\"TableSection\" width=\"100%\">");
                sb.Append("<tr>");
                foreach (TableSectionColumn col in sectionDetail.Columns)
                {
                    if (col.Visible && col.SystemName!="")
                    {

                        //if (tblOtherInfo.TableType == "system" && tblOtherInfo.SystemTableName == "uploads")
                        //{
                        //    if (col.SystemName == "ValidCount")
                        //    {
                                
                        //        sb.Append("<th style='color:Green;'>");
                        //    }
                        //    else if (col.SystemName == "WarningCount")
                        //    {

                        //        sb.Append("<th style='color:Blue;'>");
                        //    }
                        //    else if (col.SystemName == "NotValidCount")
                        //    {

                        //        sb.Append("<th style='color:Red;'>");
                        //    }

                        //    else
                        //    {
                        //        sb.Append("<th>");
                        //    }
                        //}
                        //else
                        //{
                            sb.Append("<th>");
                        //}
                        
                        if (!String.IsNullOrEmpty(col.DisplayName))
                            sb.Append(col.DisplayName);
                        else
                            sb.Append(col.SystemName);
                        sb.Append("</th>");
                    }
                }
                sb.Append("</tr>");
                string Message = "";
                string style = "";
                string FieldValue;

                int iTableID = -1;

                if (tblOtherInfo.TableType != "system")
                {
                    for (int i = 0; i < sectionFilter.Params.Count; i++)
                    {
                        if (sectionFilter.Params[i].Name == "@nTableID")
                        {
                            iTableID = int.Parse(sectionFilter.Params[i].Value.ToString());
                        }
                    }
                }


                if (!String.IsNullOrEmpty(section.ValueFields) && tblOtherInfo.TableType != "system")
                {
                    //TableSectionOtherInfo tblOtherInfo = JSONField.GetTypedObject<TableSectionOtherInfo>(section.ValueFields);
                    if (tblOtherInfo.IsUseReportDate != null)
                    {
                        if ((bool)tblOtherInfo.IsUseReportDate)
                        {
                            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                            {
                                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == section.DocumentID);

                                if (doc != null)
                                {
                                    bool bForDashBoard = false;
                                    if (doc.ForDashBoard != null)
                                    {
                                        if ((bool)doc.ForDashBoard)
                                        {
                                            bForDashBoard = true;
                                            //
                                            if (tblOtherInfo.RecentDays != null)
                                            {
                                                DateTime dtTo = DateTime.Now;
                                                DateTime dtFrom = DateTime.Now;
                                                DataTable dtTemp = Common.DataTableFromText("SELECT MAX(DateTimeRecorded) FROM Record WHERE IsActive=1 AND TableID=" + iTableID.ToString());

                                                if (dtTemp.Rows.Count > 0)
                                                {
                                                    if (dtTemp.Rows[0][0] != DBNull.Value)
                                                    {
                                                        dtTo = (DateTime)dtTemp.Rows[0][0];
                                                        dtFrom = ((DateTime)dtTemp.Rows[0][0]).AddDays(-(int)tblOtherInfo.RecentDays);
                                                    }

                                                }


                                                bool bFound = false;
                                                for (int i = 0; i < sectionFilter.Params.Count; i++)
                                                {
                                                    if (sectionFilter.Params[i].Name == "@dDateFrom")
                                                    {
                                                        sectionFilter.Params[i].Value = dtFrom;
                                                        bFound = true;
                                                    }
                                                }
                                                if (bFound == false)
                                                {
                                                    //add this param
                                                    SPInputParam ptxtDateFrom = new SPInputParam();
                                                    ptxtDateFrom.Name = "@dDateFrom";
                                                    ptxtDateFrom.DataType = "datetime";
                                                    ptxtDateFrom.Value = dtFrom.ToLocalTime();
                                                    sectionFilter.Params.Add(ptxtDateFrom);
                                                }

                                                bFound = false;

                                                for (int i = 0; i < sectionFilter.Params.Count; i++)
                                                {
                                                    if (sectionFilter.Params[i].Name == "@dDateTo")
                                                    {
                                                        sectionFilter.Params[i].Value = dtTo;
                                                        bFound = true;
                                                    }
                                                }
                                                if (bFound == false)
                                                {
                                                    SPInputParam ptxtDateTo = new SPInputParam();
                                                    ptxtDateTo.Name = "@dDateTo";
                                                    ptxtDateTo.DataType = "datetime";
                                                    ptxtDateTo.Value = dtTo.ToLocalTime();
                                                    sectionFilter.Params.Add(ptxtDateTo);

                                                }



                                            }
                                            else
                                            {

                                            }

                                        }
                                    }



                                    //
                                    if (doc.DocumentDate != null && bForDashBoard == false)
                                    {
                                        bool bFound = false;
                                        for (int i = 0; i < sectionFilter.Params.Count; i++)
                                        {
                                            if (sectionFilter.Params[i].Name == "@dDateFrom")
                                            {
                                                sectionFilter.Params[i].Value = doc.DocumentDate;
                                                bFound = true;
                                            }
                                        }
                                        if (bFound == false)
                                        {
                                            //add this param
                                            SPInputParam ptxtDateFrom = new SPInputParam();
                                            ptxtDateFrom.Name = "@dDateFrom";
                                            ptxtDateFrom.DataType = "datetime";
                                            ptxtDateFrom.Value = doc.DocumentDate.Value.ToLocalTime();
                                            sectionFilter.Params.Add(ptxtDateFrom);
                                        }

                                    }

                                    if (doc.DocumentEndDate != null && bForDashBoard == false)
                                    {
                                        bool bFound = false;
                                        for (int i = 0; i < sectionFilter.Params.Count; i++)
                                        {
                                            if (sectionFilter.Params[i].Name == "@dDateTo")
                                            {
                                                sectionFilter.Params[i].Value = doc.DocumentEndDate;

                                                bFound = true;
                                            }
                                        }
                                        if (bFound == false)
                                        {
                                            SPInputParam ptxtDateTo = new SPInputParam();
                                            ptxtDateTo.Name = "@dDateTo";
                                            ptxtDateTo.DataType = "datetime";
                                            ptxtDateTo.Value = doc.DocumentDate.Value.ToLocalTime();
                                            sectionFilter.Params.Add(ptxtDateTo);

                                        }

                                    }

                                    //



                                }
                            }
                        }
                        else
                        {
                            //not report date
                            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                            {
                                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == section.DocumentID);
                                if (doc != null)
                                {
                                    bool bForDashBoard = false;
                                    if (doc.ForDashBoard != null)
                                    {
                                        if ((bool)doc.ForDashBoard)
                                        {
                                            bool bFound = false;
                                            for (int i = 0; i < sectionFilter.Params.Count; i++)
                                            {
                                                if (sectionFilter.Params[i].Name == "@dDateFrom")
                                                {
                                                    sectionFilter.Params[i].Value = DateTime.Now;
                                                    bFound = true;
                                                }
                                            }
                                            if (bFound == false)
                                            {
                                                //add this param
                                                SPInputParam ptxtDateFrom = new SPInputParam();
                                                ptxtDateFrom.Name = "@dDateFrom";
                                                ptxtDateFrom.DataType = "datetime";
                                                ptxtDateFrom.Value = DateTime.Now.ToLocalTime();
                                                sectionFilter.Params.Add(ptxtDateFrom);
                                            }

                                            bFound = false;

                                            for (int i = 0; i < sectionFilter.Params.Count; i++)
                                            {
                                                if (sectionFilter.Params[i].Name == "@dDateTo")
                                                {
                                                    sectionFilter.Params[i].Value = DateTime.Now;
                                                    bFound = true;
                                                }
                                            }
                                            if (bFound == false)
                                            {
                                                SPInputParam ptxtDateTo = new SPInputParam();
                                                ptxtDateTo.Name = "@dDateTo";
                                                ptxtDateTo.DataType = "datetime";
                                                ptxtDateTo.Value = DateTime.Now.ToLocalTime();
                                                sectionFilter.Params.Add(ptxtDateTo);

                                            }



                                        }

                                    }

                                }
                            }

                        }

                    }
                }

                if (tblOtherInfo.TableType == "system")
                {

                }


                //so this is a test
                    //do we have MaxRow
                    if (sectionFilter.MaxRow != null)
                    {
                        SPInputParam ptxtnMaxRows = new SPInputParam();
                        ptxtnMaxRows.Name = "@nMaxRows";
                        ptxtnMaxRows.DataType = "System.Int32";
                        ptxtnMaxRows.Value = sectionFilter.MaxRow;
                        sectionFilter.Params.Add(ptxtnMaxRows);
                    }

                
                

                DataTable dtSource = DBUtil.ExecuteSP(sectionFilter.SPName, sectionFilter.Params, out Message);
                if (Message == "")
                {
                    DataTable _dtRecordColums = RecordManager.ets_Table_Columns_Summary(iTableID, null);
                    foreach (DataRow drSource in dtSource.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (TableSectionColumn c in sectionDetail.Columns)
                        {
                            if (c.Visible && c.SystemName!="" )
                            {
                                style = "text-align:" + c.Alignment + ";";
                                if (c.Bold) style += "font-weight:bold;";
                                if (c.Italic) style += "font-style:italic;";
                                if (c.Underline) style += "text-decoration:underline;";

                                if (tblOtherInfo.TableType == "system" && tblOtherInfo.SystemTableName == "uploads")
                                {
                                    if (c.SystemName == "ValidCount")
                                    {
                                        style += "color:Green;border-color:Black;";
                                    }
                                    if (c.SystemName == "WarningCount")
                                    {
                                        style += "color:Blue;border-color:Black;";
                                    }
                                    if (c.SystemName == "NotValidCount")
                                    {
                                        style += "color:Red;border-color:Black;";
                                    }
                                }

                                switch (dtSource.Columns[c.SystemName].DataType.ToString())
                                {
                                    case "System.DateTime":


                                        FieldValue = (drSource[c.SystemName] != DBNull.Value ? Convert.ToDateTime(drSource[c.SystemName]).ToString("dd/MM/yyyy") : "__/__/____");

                                        if (!String.IsNullOrEmpty(section.ValueFields))
                                        {
                                            //TableSectionOtherInfo tblOtherInfo = JSONField.GetTypedObject<TableSectionOtherInfo>(section.ValueFields);

                                            if (tblOtherInfo.ShowTimeWithDate != null)
                                            {
                                                if ((bool)tblOtherInfo.ShowTimeWithDate)
                                                {
                                                    FieldValue = (drSource[c.SystemName] != DBNull.Value ? Convert.ToDateTime(drSource[c.SystemName]).ToString("dd/MM/yyyy HH:mm:ss") : "__/__/____");
                                                }

                                            }

                                        }

                                        
                                        sb.Append(String.Format("<td style=\"{1}\">{0}</td>", FieldValue, style));
                                        break;
                                    case "System.Int16":
                                    case "System.Int32":
                                    case "System.Int64":
                                    case "System.Decimal":
                                    case "System.Double":
                                    case "System.Single":
                                    case "System.Byte":
                                        FieldValue = Convert.ToString(drSource[c.SystemName]);

                                        if (tblOtherInfo.TableType == "system" && tblOtherInfo.SystemTableName == "uploads")
                                        {
                                            if (c.SystemName == "BatchID")
                                            {
                                                string strBase = "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;

                                                string strLink = "<a title='View' href='" + strBase +
                                                    "/Pages/Record/UploadValidation.aspx?TableID=" + Cryptography.Encrypt(drSource["TableID"].ToString())
                                                    + "&BatchID=" + Cryptography.Encrypt(drSource["BatchID"].ToString())
                                                    + "'> <img title='View' src='" + strBase + "/App_Themes/Default/Images/iconShow.png' alt='' style='border-width:0px;'></a>";
                                                FieldValue = strLink;

                                            }
                                        }


                                        sb.Append(String.Format("<td style=\"{1}\">{0}</td>", FieldValue, style));
                                        break;
                                    default:
                                        FieldValue = Convert.ToString(drSource[c.SystemName]);

                                        if (tblOtherInfo.TableType != "system")
                                        {
                                            for (int i = 0; i < _dtRecordColums.Rows.Count; i++)
                                            {


                                                if (_dtRecordColums.Rows[i]["ColumnType"].ToString() == "file")
                                                {
                                                    if (_dtRecordColums.Rows[i]["DisplayTextSummary"].ToString() == c.SystemName)
                                                    {
                                                        if (FieldValue != "")
                                                        {
                                                            try
                                                            {
                                                                if (FieldValue.Length > 37)
                                                                {
                                                                    FieldValue = FieldValue.Substring(37);

                                                                }
                                                            }
                                                            catch
                                                            {
                                                                //
                                                            }
                                                        }
                                                    }
                                                }


                                                if (_dtRecordColums.Rows[i]["TableTableID"] != DBNull.Value
                                                && (_dtRecordColums.Rows[i]["DropDownType"].ToString() == "table"
                                                || _dtRecordColums.Rows[i]["DropDownType"].ToString() == "linked")
                                                 && _dtRecordColums.Rows[i]["ColumnType"].ToString() == "dropdown"
                                                && _dtRecordColums.Rows[i]["DisplayColumn"].ToString() != "")
                                                {
                                                    if (_dtRecordColums.Rows[i]["DisplayTextSummary"].ToString() == c.SystemName)
                                                    {

                                                        if (FieldValue != "")
                                                        {
                                                            try
                                                            {
                                                                int iTableRecordID = int.Parse(FieldValue);
                                                                DataTable dtTableTableSC = Common.DataTableFromText(@"SELECT SystemName,DisplayName 
                                                                FROM [Column] WHERE   TableID ="
                                                                 + _dtRecordColums.Rows[i]["TableTableID"].ToString());

                                                                string strDisplayColumn = _dtRecordColums.Rows[i]["DisplayColumn"].ToString();

                                                                foreach (DataRow dr in dtTableTableSC.Rows)
                                                                {
                                                                    strDisplayColumn = strDisplayColumn.Replace("[" + dr["DisplayName"].ToString() + "]", "[" + dr["SystemName"].ToString() + "]");

                                                                }

                                                                DataTable dtTheRecord = Common.DataTableFromText("SELECT * FROM Record WHERE RecordID=" + iTableRecordID.ToString());
                                                                if (dtTheRecord.Rows.Count > 0)
                                                                {
                                                                    foreach (DataColumn dc in dtTheRecord.Columns)
                                                                    {
                                                                        strDisplayColumn = strDisplayColumn.Replace("[" + dc.ColumnName + "]", dtTheRecord.Rows[0][dc.ColumnName].ToString());
                                                                    }
                                                                }

                                                                FieldValue = strDisplayColumn;
                                                            }
                                                            catch
                                                            {
                                                                //
                                                            }


                                                        }
                                                    }

                                                }



                                                if (_dtRecordColums.Rows[i]["IsRound"] != DBNull.Value)
                                                {
                                                    if (_dtRecordColums.Rows[i]["IsRound"].ToString().ToLower() == "true")
                                                    {
                                                        if (_dtRecordColums.Rows[i]["DisplayTextSummary"].ToString() == c.SystemName)
                                                        {

                                                            if (FieldValue != "")
                                                            {
                                                                try
                                                                {
                                                                    FieldValue = Math.Round(double.Parse(Common.IgnoreSymbols(FieldValue)), int.Parse(_dtRecordColums.Rows[i]["RoundNumber"].ToString())).ToString();
                                                                }
                                                                catch
                                                                {
                                                                    //
                                                                }
                                                            }
                                                        }
                                                    }

                                                }

                                            }
                                        }

                                        //drSource[c.SystemName]
                                        if (tblOtherInfo.TableType == "system" && tblOtherInfo.SystemTableName == "alerts")
                                        {
                                            if (c.SystemName == "WarningCount")
                                            {
                                                string strLink = "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/Pages/Record/RecordList.aspx?warning=yes&TableID=" + Cryptography.Encrypt(drSource["TableID"].ToString());
                                                FieldValue = "<a href='"+strLink+"'>" + FieldValue + "</a>";

                                            }
                                        }

                                       

                                        sb.Append(String.Format("<td style=\"{1}\">{0}</td>", FieldValue, style));
                                        break;
                                }
                            }
                        }
                        sb.Append("</tr>");
                    }
                    if (tblOtherInfo.TableType == "system" && tblOtherInfo.SystemTableName == "alerts")
                    {
                        string strLink = "<a  href='" + "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/Pages/Record/Notification.aspx'>...More</a>";
                        sb.Append("<tr><td colspan='3' align='right'>" + strLink + "</td></tr>");
                    }
                    if (tblOtherInfo.TableType == "system" && tblOtherInfo.SystemTableName == "uploads")
                    {
                        string strLink = "<a  href='" + "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/Pages/Record/Batches.aspx?menu=" + Cryptography.Encrypt("yes") + "&TableID=" + Cryptography.Encrypt("-1") + "'>...More</a>";
                        sb.Append("<tr><td colspan='6' align='right'>" + strLink + "</td></tr>");
                    }
                }
                else
                {
                    switch (Message)
                    {
                        case "ExecuteReader: CommandText property has not been initialized":
                            Message = "Please select data source for table section";
                            break;
                    }
                    sb.Append(String.Format("<tr><td colspan=\"{0}\">{1}</td></tr>", sectionDetail.Columns.Count, Message));
                }

                sb.Append("</table>");
            }
           
            return sb.ToString();
        }

        public static string GenerateChartSection(DAL.DocumentSection section, int iSearchCriteria)
        {
            //ChartSectionDetail sectionDetail = JSONField.GetTypedObject<ChartSectionDetail>(section.Details);



            string retVal = String.Format("<p style=\"text-align:center\"><img alt=\"" + section.SectionName + "\" src=\"{0}/Pages/DocGen/ChartSectionImage.aspx?DocumentSectionID={1}&SearchCriteria={2}\"/></p>",
                "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath,
                section.DocumentSectionID, iSearchCriteria
                );

            //if (!String.IsNullOrEmpty(sectionDetail.Comment))
            //{
            //    retVal += String.Format("<p style=\"text-align:center\">{0}</p>", HttpUtility.HtmlEncode(sectionDetail.Comment));
            //}
            return retVal;
        }

        public static string GenerateRecordTableSection(DAL.DocumentSection section)
        {
            //string  strWidth = "810";
            string strHeight = "460px";

            //string strHeight = "100%";

            if (section.Details != "")
            {
                DocGen.DAL.RecordTableSectionDetail rtDetail = DocGen.DAL.JSONField.GetTypedObject<DocGen.DAL.RecordTableSectionDetail>(section.Details);
                if (rtDetail != null)
                {
                    if (rtDetail.TableID != null)
                    {
                       

                        string strExtraParam = "";
                        if (section.ValueFields != null)
                        {
                            if (section.ValueFields.ToString() == "half")
                            {
                                strExtraParam = "&width=half";
                            }
                        }

                        //string retVal = String.Format("<table width='100%' style='min-width:490px;'><tr><td align='center'><iframe frameBorder='0' width='100%' height='" + strHeight + "' scrolling='no'  src=\"{0}/Pages/DocGen/EachRecordTable.aspx?RecordTable=Yes&Dashboard=Yes" + strExtraParam + "&TableID={1}&SearchCriteriaID={2}\"></iframe></td></tr></table>",
                        //    "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath,
                        //    Cryptography.Encrypt(rtDetail.TableID.ToString()),Cryptography.Encrypt(rtDetail.SearchCriteriaID.ToString())
                        //    );

                        //handle exception
                        if (rtDetail.ViewID != 0)
                        {
                            View theView = ViewManager.dbg_View_Detail(rtDetail.ViewID);
                            if (theView == null)
                                rtDetail.ViewID = 0;
                        }

                        if(rtDetail.ViewID==0)
                        {

                            //int? iNewViewID=  ViewManager.CreateDashView(rtDetail.TableID);
                            //User objUser = (User)System.Web.HttpContext.Current.Session["User"];

                            int? iNewViewID = ViewManager.dbg_View_CreateDash(section.Document.UserID, (int)rtDetail.TableID);

                            rtDetail.ViewID = (int)iNewViewID;
                            section.Details = rtDetail.GetJSONString();

                             using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                             {
                                 DAL.DocumentSection section2 = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == section.DocumentSectionID);
                                 if (section2 != null)
                                 {
                                     RecordTableSectionDetail rtSec = new RecordTableSectionDetail();
                                     rtSec.TableID = rtDetail.TableID;                                   
                                     rtSec.ViewID = rtDetail.ViewID;
                                     section2.Details = rtSec.GetJSONString();
                                     ctx.SubmitChanges();
                                 }
                             }

                        }


                        //string retVal = String.Format("<table width='100%' style='min-width:400px;'><tr><td align='left'><iframe id='iframe" + section.DocumentSectionID.ToString() + "' onLoad='autoResize(\"iframe" + section.DocumentSectionID.ToString()
                        //    + "\")'; frameBorder='0'  scrolling='no'  src=\"{0}/Pages/DocGen/EachRecordTable.aspx?RecordTable=Yes&Dashboard=Yes" + strExtraParam + "&TableID={1}&ViewID={2}\"></iframe></td></tr></table>",
                        //   "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath,
                        //   Cryptography.Encrypt(rtDetail.TableID.ToString()), Cryptography.Encrypt(rtDetail.ViewID.ToString())
                        //   );


                        string retVal = String.Format("<table width='100%' style='min-width:400px;'><tr><td align='left'><iframe id='iframe" + section.DocumentSectionID.ToString() 
                            + "' frameBorder='0'  scrolling='no'  src=\"{0}/Pages/DocGen/EachRecordTable.aspx?RecordTable=Yes&Dashboard=Yes" + strExtraParam 
                            + "&TableID={1}&ViewID={2}&DocumentSectionID={3}\"></iframe></td></tr></table>",
                           "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath,
                           Cryptography.Encrypt(rtDetail.TableID.ToString()), Cryptography.Encrypt(rtDetail.ViewID.ToString()),
                           Cryptography.Encrypt(section.DocumentSectionID.ToString())
                           );

                        return retVal;

                    }                    

                   

                }

               
            }

            return "";
        }


        public static string GenerateCalendarSection(DAL.DocumentSection section)
        {
           

            if (section.Details != "")
            {
                DocGen.DAL.CalendarSectionDetail rtDetail = DocGen.DAL.JSONField.GetTypedObject<DocGen.DAL.CalendarSectionDetail>(section.Details);
                if (rtDetail != null)
                {
                    if (rtDetail.TableID != null)
                    {


                        string strWidthStuff = "";
                        if (rtDetail.Width != null && rtDetail.Height!=null)
                        {
                            if (rtDetail.Height<900)
                            {
                                rtDetail.Height = 900;
                            }
                            strWidthStuff = " width='" + rtDetail.Width.ToString() + "px' height='" + rtDetail.Height.ToString() + "px'";
                        }
                        else if(rtDetail.Width!=null)
                        {
                            strWidthStuff = " width='" + rtDetail.Width.ToString() + "px' height='900px'";
                        }
                        else
                        {
                            strWidthStuff = " onLoad='autoResize(\"iframe" + section.DocumentSectionID.ToString() + "\")';";
                        }

                        string retVal = String.Format("<table width='100%' style='min-width:400px;'><tr><td align='left'><iframe id='iframe" + section.DocumentSectionID.ToString() + "'" + strWidthStuff
                            + " frameBorder='0'  scrolling='no'  src=\"{0}/Pages/DocGen/EachCalendar.aspx?DocumentSectionID={1}\"></iframe></td></tr></table>",
                           "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath, section.DocumentSectionID.ToString()
                           );


                        return retVal;

                    }



                }


            }

            return "";
        }

        public static string GenerateMapSection(DAL.DocumentSection section)
        {
            int iWidth = 450;
            int iHeight = 450;

            if (section.Details != "")
            {
                DocGen.DAL.MapSectionDetail mapDetail = DocGen.DAL.JSONField.GetTypedObject<DocGen.DAL.MapSectionDetail>(section.Details);
                if (mapDetail != null)
                {
                    if (mapDetail.Height != null)
                        iHeight = (int)mapDetail.Height;

                    if (mapDetail.Width != null)
                        iWidth = (int)mapDetail.Width;

                }

            }

            iWidth = iWidth + 10;
            iHeight = iHeight + 60;
            string retVal = String.Format("<table width='100%'><tr><td align='left'><iframe frameBorder='0' width='" + iWidth.ToString() + "px' height='" + iHeight.ToString() + "px' scrolling='no'  src=\"{0}/Pages/DocGen/EachMap.aspx?DocumentSectionID={1}\"></iframe></td></tr></table>",
                "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath,
                section.DocumentSectionID
                );
           
            return retVal;
        }


        public static string GenerateDialSection(DAL.DocumentSection section)
        {

            int iWidth = 200;
            int iHeight = 180;

            if (section.Details != "")
            {
                DialSectionDetail dialSecDetail = JSONField.GetTypedObject<DialSectionDetail>(section.Details);

                if (dialSecDetail.Height != null)
                    iHeight = (int)dialSecDetail.Height;
                if (dialSecDetail.Width != null)
                    iWidth = (int)dialSecDetail.Width;
                if (dialSecDetail.Heading == "")
                {

                }
                else
                {
                    iHeight = iHeight + 50;
                }

            }
            iWidth = iWidth + 10;
            iHeight = iHeight + 10;

            string retVal = String.Format("<iframe frameBorder='0' width='"+iWidth.ToString()+"px' height='"+iHeight.ToString()+"px' scrolling='no'  src=\"{0}/Pages/DocGen/EachDial.aspx?DocumentSectionID={1}\"></iframe>",
                "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath,
                section.DocumentSectionID
                );

            return retVal;
        }

        public static string GenerateDashChartSection(DAL.DocumentSection section)
        {


            int iWidth = 700;
            int  iHeight = 500;

            GraphOption theGraphOption = GraphManager.ets_GraphOption_Detail(int.Parse(section.Details));

            if (theGraphOption.Width != null && theGraphOption.Height != null)
            {
                iWidth = (int)theGraphOption.Width;
                iHeight = (int)theGraphOption.Height;

            }
            iHeight = iHeight + 100;
            iWidth = iWidth + 25;

            string retVal = String.Format("<iframe frameBorder='0' width='" + iWidth.ToString() + "px' height='" + iHeight.ToString() + "px' scrolling='no'  src=\"{0}/Pages/DocGen/EachDashChart.aspx?DocumentSectionID={1}\"></iframe>",
                "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath,
                section.DocumentSectionID
                );

            return retVal;
        }



        public static string GenerateColumnsSection(DAL.DocumentSection section, bool EditMode)
        {
            ColumnsSectionDetail sectionDetail = JSONField.GetTypedObject<ColumnsSectionDetail>(section.Details);
            int ColumnCount = Convert.ToInt32(section.Content);

            List<int> colWidths = new List<int>();
            int i = 0;
            for (i = 0; i < ColumnCount; i++)
            {
                colWidths.Add(100 / ColumnCount);
            }
            if (sectionDetail.Widths.Count > 0)
            {
                i = 0;
                foreach (int w in sectionDetail.Widths)
                {
                    if (i < ColumnCount - 1)
                    {
                        colWidths[i] = w;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder sbTemp = new StringBuilder();
            //
            if (EditMode)
            {
                sb.AppendLine("<table class=\"LayoutTable\" width=\"100%\"  >");
            }
            else
            {
                //sb.AppendLine("<table class=\"LayoutTable\" width=\"100%\"  >");
                sb.AppendLine("<table class=\"LayoutTable\" >");
            }
            sb.AppendLine("<tr>");

            using (DocGenDataContext ctx = new DocGenDataContext())
            {
                for (i = 0; i < ColumnCount; i++)
                {
                    int iSpacing=0;
                    if (i != 0 && sectionDetail.Spacing != null)
                    {
                        iSpacing = (int)sectionDetail.Spacing;
                    }
                    //\"width:" + colWidths[i] + "%;

                    if (EditMode)
                    {
                        sb.AppendLine("<td class=\"Zone\" style=\"width:" + colWidths[i] + "%;padding-left:" + iSpacing.ToString() + "px;\">");
                    }
                    else
                    {
                        sb.AppendLine("<td class=\"Zone\" style=padding-left:" + iSpacing.ToString() + "px;\">");
                    }
                    
                    if (EditMode) sb.AppendLine("   <div class=\"ZoneInner\" colIndex=\"" + i.ToString() + "\">");
                    var subSections_query = from s in ctx.DocumentSections
                                            where s.ParentSectionID == section.DocumentSectionID
                                            orderby s.Position
                                            select s;

                    foreach (DocumentSection subSection in subSections_query)
                    {
                        if (subSection.ColumnIndex == i || (i == 0 && (!subSection.ColumnIndex.HasValue || subSection.ColumnIndex >= ColumnCount)))
                        {
                            string SType = "";
                            switch (subSection.DocumentSectionTypeID)
                            {
                                case 1:
                                    SType = "HTML";
                                    break;
                                case 2:
                                    SType = "Text";
                                    break;
                                case 3:
                                    SType = "Image";
                                    break;
                                case 6:
                                    SType = "Table";
                                    break;
                                case 5:
                                    SType = "Chart";
                                    break;
                                case 4:
                                    SType = "Columns";
                                    break;
                                case 7:
                                    SType = "Map";
                                    break;
                                case 9:
                                    SType = "DashChart";
                                    break;
                                case 8:
                                    SType = "Dial";
                                    break;
                                case 10:
                                    SType = "RecordTable";
                                    break;
                                case 11:
                                    SType = "Calendar";
                                    break;

                            }

                            string STypeCaption = SType;

                            if (SType == "DashChart")
                            {
                                STypeCaption = "Chart";
                            }

                            if (EditMode)
                            {
                                sb.AppendLine(String.Format("<div class=\"Section {0}Section\" id=\"SECTION_{1}\" ondblclick=\"EditSection($(this))\">", SType, subSection.DocumentSectionID));
                                sb.AppendLine("<div class=\"Toolbar\">");
                                sb.AppendLine("<a class=\"ui-icon ui-icon-carat-2-n-s\" title=\"Drag and drop to change order\"></a><a class=\"ui-icon ui-icon-pencil\" onclick=\"EditSection($(this).parent().parent())\" title=\"Edit section\"></a><a class=\"ui-icon ui-icon-trash\" onclick=\"RemoveSection($(this).parent().parent())\" title=\"Delete section\"></a>");
                                sb.AppendLine("<div class=\"section-type\">" + STypeCaption + "</div>");
                                sb.AppendLine("</div>");
                                sb.AppendLine("<div class=\"Content\">");
                            }
                            switch (subSection.DocumentSectionTypeID)
                            {
                                case 1:
                                    sb.AppendLine(GenerateHTMLSection(subSection));
                                    sb.Append("<br/>");
                                    break;
                                case 2:
                                    sb.AppendLine(GenerateTextSection(subSection, ref sbTemp));
                                    sb.Append("<br/>");
                                    break;
                                case 3:
                                    sb.AppendLine(GenerateImageSection(subSection));
                                    sb.Append("<br/>");
                                    break;
                                case 6:
                                    sb.AppendLine(GenerateTableSection(subSection));
                                    sb.Append("<br/>");
                                    break;
                                case 5:
                                    sb.AppendLine(GenerateChartSection(subSection, -1));
                                    sb.Append("<br/>");
                                    break;
                                case 4:
                                    sb.AppendLine(GenerateColumnsSection(subSection, EditMode));
                                    sb.Append("<br/>");
                                    break;
                                case 7:
                                    sb.AppendLine(GenerateMapSection(subSection));
                                    sb.Append("<br/>");
                                    break;
                                case 9:
                                    sb.AppendLine(GenerateDashChartSection(subSection));
                                    sb.Append("<br/>");
                                    break;
                                case 8:
                                    sb.AppendLine(GenerateDialSection(subSection));
                                    sb.Append("<br/>");
                                    break;
                                case 10:
                                    subSection.ValueFields = "half";
                                    sb.AppendLine(GenerateRecordTableSection(subSection));
                                    sb.Append("<br/>");
                                    break;

                                case 11:
                                  
                                    sb.AppendLine(GenerateCalendarSection(subSection));
                                    sb.Append("<br/>");
                                    break;

                            }
                            if (EditMode) sb.AppendLine("</div></div>");
                        }
                    }
                    if (EditMode) sb.AppendLine("   </div>");
                    sb.AppendLine("</td>");
                }
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
            return sb.ToString();
        }     
    }
}