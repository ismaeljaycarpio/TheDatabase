using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using DocGen.DAL;
using System.Text;

/// <summary>
/// Summary description for WorkFlowManager
/// </summary>
public class WorkFlowManager
{
	public WorkFlowManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}





    #region WorkFlow



    //public static int ets_WorkFlow_Insert(WorkFlow p_WorkFlow)
    //{

    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //            connection.Open();
    //        }
    //    }
    //    else
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = tn.Connection;
    //        }
    //    }

    //    using (SqlCommand command = new SqlCommand("ets_WorkFlow_Insert", connection))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //        pRV.Direction = ParameterDirection.Output;

    //        command.Parameters.Add(pRV);
    //        command.Parameters.Add(new SqlParameter("@nAccountID", p_WorkFlow.AccountID));
    //        command.Parameters.Add(new SqlParameter("@sWorkFlowName", p_WorkFlow.WorkFlowName));


    //        //connection.Open();
    //        command.ExecuteNonQuery();


    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }

    //        return int.Parse(pRV.Value.ToString());
    //    }

    //}


    //public static int ets_WorkFlow_Update(WorkFlow p_WorkFlow, SqlTransaction tn)
    //{

    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //        connection.Open();
    //    }
    //    else
    //    {
    //        connection = tn.Connection;
    //    }

    //    using (SqlCommand command = new SqlCommand("ets_WorkFlow_Update", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        command.Parameters.Add(new SqlParameter("@nWorkFlowID", p_WorkFlow.WorkFlowID));
    //        command.Parameters.Add(new SqlParameter("@nAccountID", p_WorkFlow.AccountID));
    //        command.Parameters.Add(new SqlParameter("@sWorkFlowName", p_WorkFlow.WorkFlowName));


    //        command.ExecuteNonQuery();

    //        if (tn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        return 1;
    //    }

    //}



    //public static DataTable ets_WorkFlow_Select(int? nAccountID, string sWorkFlowName,
    //    string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    //{

    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_WorkFlow_Select", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;

    //            command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

            

    //            if (sWorkFlowName != "")
    //                command.Parameters.Add(new SqlParameter("@sWorkFlowName", sWorkFlowName));


    //            if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
    //            { sOrder = "WorkFlowID"; sOrderDirection = "DESC"; }

    //            command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

    //            if (nStartRow != null)
    //                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

    //            if (nMaxRows != null)
    //                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


    //            connection.Open();

    //            SqlDataAdapter da = new SqlDataAdapter();
    //            da.SelectCommand = command;
    //            DataTable dt = new DataTable();
    //            System.Data.DataSet ds = new System.Data.DataSet();
    //            da.Fill(ds);

    //            connection.Close();
    //            connection.Dispose();

    //            iTotalRowsNum = 0;
    //            if (ds.Tables.Count > 1)
    //            {
    //                iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
    //            }
    //            if (ds.Tables.Count > 0)
    //            {
    //                return ds.Tables[0];
    //            }
    //            {
    //                return null;
    //            }


    //        }
    //    }
    //}




    //public static int ets_WorkFlow_Delete(int nWorkFlowID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_WorkFlow_Delete", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nWorkFlowID ", nWorkFlowID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}




    //public static WorkFlow ets_WorkFlow_Detail(int nWorkFlowID, SqlTransaction tn, SqlConnection cn)
    //{
    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //            connection.Open();
    //        }
    //    }
    //    else
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = tn.Connection;
    //        }
    //    }
    //    using (SqlCommand command = new SqlCommand("ets_WorkFlow_Detail", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }
    //        command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));

    //        //connection.Open();

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                WorkFlow temp = new WorkFlow(
    //                    (int)reader["WorkFlowID"], (int)reader["AccountID"],
    //                  (string)reader["WorkFlowName"]
    //                    );

    //                if (tn == null && cn == null)
    //                {
    //                    connection.Close();
    //                    connection.Dispose();
    //                }

    //                return temp;
    //            }

    //        }

    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }

    //        return null;

    //    }

    //}












    #endregion






    #region WorkFlowSection



    //public static int ets_WorkFlowSection_Insert(WorkFlowSection p_WorkFlowSection, 
    //    SqlTransaction tn, SqlConnection cn)
    //{

    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //            connection.Open();
    //        }
    //    }
    //    else
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = tn.Connection;
    //        }
    //    }

    //    using (SqlCommand command = new SqlCommand("ets_WorkFlowSection_Insert", connection))
    //    {

    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
    //        pRV.Direction = ParameterDirection.Output;

    //        command.Parameters.Add(pRV);
    //        command.Parameters.Add(new SqlParameter("@nWorkFlowID", p_WorkFlowSection.WorkFlowID));
    //        //command.Parameters.Add(new SqlParameter("@nWorkFlowSectionID", p_WorkFlowSection.WorkFlowSectionID));

           
    //        command.Parameters.Add(new SqlParameter("@nWorkFlowSectionTypeID", p_WorkFlowSection.WorkFlowSectionTypeID));
    //        command.Parameters.Add(new SqlParameter("@nPosition", p_WorkFlowSection.Position));
    //        command.Parameters.Add(new SqlParameter("@nParentSectionID", p_WorkFlowSection.ParentSectionID));
    //        command.Parameters.Add(new SqlParameter("@sJSONContent", p_WorkFlowSection.JSONContent));
    //        command.Parameters.Add(new SqlParameter("@nColumnIndex", p_WorkFlowSection.ColumnIndex));

    //        command.Parameters.Add(new SqlParameter("@sSectionName", p_WorkFlowSection.SectionName));



    //        //connection.Open();
    //        command.ExecuteNonQuery();


    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }

    //        return int.Parse(pRV.Value.ToString());
    //    }

    //}


    //public static int ets_WorkFlowSection_Update(WorkFlowSection p_WorkFlowSection, SqlTransaction tn)
    //{

    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //        connection.Open();
    //    }
    //    else
    //    {
    //        connection = tn.Connection;
    //    }

    //    using (SqlCommand command = new SqlCommand("ets_WorkFlowSection_Update", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }

    //        command.Parameters.Add(new SqlParameter("@nWorkFlowSectionID", p_WorkFlowSection.WorkFlowSectionID));
    //        command.Parameters.Add(new SqlParameter("@nWorkFlowID", p_WorkFlowSection.WorkFlowID));
          
    //        command.Parameters.Add(new SqlParameter("@nWorkFlowSectionTypeID", p_WorkFlowSection.WorkFlowSectionTypeID));
    //        command.Parameters.Add(new SqlParameter("@nPosition", p_WorkFlowSection.Position));
    //        command.Parameters.Add(new SqlParameter("@nParentSectionID", p_WorkFlowSection.ParentSectionID));
    //        command.Parameters.Add(new SqlParameter("@sJSONContent", p_WorkFlowSection.JSONContent));
    //        command.Parameters.Add(new SqlParameter("@nColumnIndex", p_WorkFlowSection.ColumnIndex));

    //        command.Parameters.Add(new SqlParameter("@sSectionName", p_WorkFlowSection.SectionName));


    //        command.ExecuteNonQuery();

    //        if (tn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }


    //        return 1;
    //    }

    //}



    ////public static DataTable ets_WorkFlowSection_Select(int? nWorkFlowID1,int? nWorkFlowSectionTypeID,
    ////    string sSectionName,
    ////    string sOrder, string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    ////{

    ////    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    ////    {
    ////        using (SqlCommand command = new SqlCommand("ets_WorkFlowSection_Select", connection))
    ////        {
    ////            command.CommandType = CommandType.StoredProcedure;

    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));



    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowSectionTypeID));
    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", sSectionName));
    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));
    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));
    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));
    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));
    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));
    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));
    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));

    ////            command.Parameters.Add(new SqlParameter("@nWorkFlowID", nWorkFlowID));


    ////            if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
    ////            { sOrder = "WorkFlowSectionID"; sOrderDirection = "DESC"; }

    ////            command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

    ////            if (nStartRow != null)
    ////                command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

    ////            if (nMaxRows != null)
    ////                command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


    ////            connection.Open();

    ////            SqlDataAdapter da = new SqlDataAdapter();
    ////            da.SelectCommand = command;
    ////            DataTable dt = new DataTable();
    ////            System.Data.DataSet ds = new System.Data.DataSet();
    ////            da.Fill(ds);

    ////            connection.Close();
    ////            connection.Dispose();

    ////            iTotalRowsNum = 0;
    ////            if (ds.Tables.Count > 1)
    ////            {
    ////                iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
    ////            }
    ////            if (ds.Tables.Count > 0)
    ////            {
    ////                return ds.Tables[0];
    ////            }
    ////            {
    ////                return null;
    ////            }


    ////        }
    ////    }
    ////}




    //public static int ets_WorkFlowSection_Delete(int nWorkFlowSectionID)
    //{
    //    using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
    //    {
    //        using (SqlCommand command = new SqlCommand("ets_WorkFlowSection_Delete", connection))
    //        {

    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@nWorkFlowSectionID ", nWorkFlowSectionID));

    //            connection.Open();
    //            command.ExecuteNonQuery();

    //            connection.Close();
    //            connection.Dispose();

    //            return 1;

    //        }
    //    }
    //}




    //public static WorkFlowSection ets_WorkFlowSection_Detail(int nWorkFlowSectionID, SqlTransaction tn, SqlConnection cn)
    //{
    //    SqlConnection connection;
    //    if (tn == null)
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = new SqlConnection(DBGurus.strGlobalConnectionString);
    //            connection.Open();
    //        }
    //    }
    //    else
    //    {
    //        if (cn != null)
    //        {
    //            connection = cn;
    //        }
    //        else
    //        {
    //            connection = tn.Connection;
    //        }
    //    }
    //    using (SqlCommand command = new SqlCommand("ets_WorkFlowSection_Detail", connection))
    //    {
    //        command.CommandType = CommandType.StoredProcedure;
    //        if (tn != null)
    //        {
    //            command.Transaction = tn;
    //        }
    //        command.Parameters.Add(new SqlParameter("@nWorkFlowSectionID", nWorkFlowSectionID));

    //        //connection.Open();

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                WorkFlowSection temp = new WorkFlowSection(
    //                    (int)reader["WorkFlowSectionID"], (int)reader["WorkFlowID"], (int)reader["WorkFlowSectionTypeID"],
    //                  reader["SectionName"] == DBNull.Value ? string.Empty : (string)reader["SectionName"],
    //                  reader["Position"] == DBNull.Value ? null : (int?)reader["Position"],
    //                   reader["JSONContent"] == DBNull.Value ? string.Empty : (string)reader["JSONContent"],
    //                  reader["ParentSectionID"] == DBNull.Value ? null : (int?)reader["ParentSectionID"],
    //                  reader["ColumnIndex"] == DBNull.Value ? null : (int?)reader["ColumnIndex"]
    //                  );

    //                if (tn == null && cn == null)
    //                {
    //                    connection.Close();
    //                    connection.Dispose();
    //                }

    //                return temp;
    //            }

    //        }

    //        if (tn == null && cn == null)
    //        {
    //            connection.Close();
    //            connection.Dispose();
    //        }

    //        return null;

    //    }

    //}

    


    #endregion



    public static string GenerateEmail_Received_TriggerSection(WorkFlowSection section)
    {
        HttpContext hContext = HttpContext.Current;
        StringBuilder sb = new StringBuilder();
      
        Email_Received_Trigger theEmail_Received_Trigger = JSONField.GetTypedObject<Email_Received_Trigger>(section.JSONContent);
        //if (theEmail_Received_Trigger == null)
        //{
        //    theEmail_Received_Trigger = new ImageSectionStyle()
        //    {
        //        Position = "left",
        //        Width = 0
        //    };
        //}
        if(theEmail_Received_Trigger!=null)
            sb.Append("<div><img src='../../images/Email_Received_Trigger.png' alt='Date_Time_Trigger'></img> </div><div>" + theEmail_Received_Trigger.FromAddress + " , " + theEmail_Received_Trigger.ToAddress + "</div>");
       


        return sb.ToString();
    }


    public static string GenerateEmail_Out_ActionSection(WorkFlowSection section)
    {
        HttpContext hContext = HttpContext.Current;
        StringBuilder sb = new StringBuilder();

        Email_Out_Action theEmail_Out_Action = JSONField.GetTypedObject<Email_Out_Action>(section.JSONContent);
       
        if (theEmail_Out_Action != null)
            sb.Append(" <div><img src='../../images/Email_Out_Action.png' alt='Date_Time_Trigger'></img> </div><div>" + theEmail_Out_Action.FromAddress + " , " + theEmail_Out_Action.ToAddress + "</div>");



        return sb.ToString();
    }



    public static string GenerateDate_Time_TriggerSection(WorkFlowSection section)
    {
        HttpContext hContext = HttpContext.Current;
        StringBuilder sb = new StringBuilder();

        Date_Time_Trigger theDate_Time_Trigger = JSONField.GetTypedObject<Date_Time_Trigger>(section.JSONContent);
        //if (theEmail_Received_Trigger == null)
        //{
        //    theEmail_Received_Trigger = new ImageSectionStyle()
        //    {
        //        Position = "left",
        //        Width = 0
        //    };
        //}
        if (theDate_Time_Trigger != null)
            sb.Append(" <div><img src='../../images/Date_Time_Trigger.png' alt='Date_Time_Trigger'></img> </div> <div>" + theDate_Time_Trigger.TimePeriod.ToString() + " , " + theDate_Time_Trigger.TimePeriodUnit + "</div>");



        return sb.ToString();
    }


    //public static string GenerateColumnsSection(WorkFlowSection section, bool EditMode)
    //{
    //    Columns sectionDetail = JSONField.GetTypedObject<Columns>(section.JSONContent);
    //    int ColumnCount = Convert.ToInt32(sectionDetail.NumberOfColumns);

    //    List<int> colWidths = new List<int>();
    //    int i = 0;
    //    for (i = 0; i < ColumnCount; i++)
    //    {
    //        colWidths.Add(100 / ColumnCount);
    //    }
    //    //if (sectionDetail.Widths.Count > 0)
    //    //{
    //    //    i = 0;
    //    //    foreach (int w in sectionDetail.Widths)
    //    //    {
    //    //        if (i < ColumnCount - 1)
    //    //        {
    //    //            colWidths[i] = w;
    //    //        }
    //    //    }
    //    //}

    //    StringBuilder sb = new StringBuilder();
    //    StringBuilder sbTemp = new StringBuilder();
    //    //
    //    if (EditMode)
    //    {
    //        sb.AppendLine("<table class=\"LayoutTable\" width=\"100%\"  >");
    //    }
    //    else
    //    {
    //        //sb.AppendLine("<table class=\"LayoutTable\" width=\"100%\"  >");
    //        sb.AppendLine("<table class=\"LayoutTable\" >");
    //    }
    //    sb.AppendLine("<tr>");

    //    using (DocGenDataContext ctx = new DocGenDataContext())
    //    {
    //        for (i = 0; i < ColumnCount; i++)
    //        {
    //            int iSpacing = 0;
    //            if (i != 0 && sectionDetail.Spacing != null)
    //            {
    //                iSpacing = (int)sectionDetail.Spacing;
    //            }
    //            //\"width:" + colWidths[i] + "%;

    //            if (EditMode)
    //            {
    //                sb.AppendLine("<td class=\"Zone\" style=\"width:" + colWidths[i] + "%;padding-left:" + iSpacing.ToString() + "px;\">");
    //            }
    //            else
    //            {
    //                sb.AppendLine("<td class=\"Zone\" style=padding-left:" + iSpacing.ToString() + "px;\">");
    //            }

    //            if (EditMode) sb.AppendLine("   <div class=\"ZoneInner\" colIndex=\"" + i.ToString() + "\">");
    //            //var subSections_query = from s in ctx.DocumentSections
    //            //                        where s.ParentSectionID == section.DocumentSectionID
    //            //                        orderby s.Position
    //            //                        select s;

    //            DataTable dtSections = Common.DataTableFromText("SELECT * FROM WorkFlowSection WHERE ParentSectionID=" + section.WorkFlowSectionID.ToString() + " ORDER BY Position");

    //            foreach (DataRow dr in dtSections.Rows)
    //            {
    //                WorkFlowSection subSection=WorkFlowManager.ets_WorkFlowSection_Detail(int.Parse(dr["WorkFlowSectionID"].ToString()),null,null);
    //                if (subSection.ColumnIndex == i || (i == 0 && (!subSection.ColumnIndex.HasValue || subSection.ColumnIndex >= ColumnCount)))
    //                {
    //                    string SType = "";
    //                    switch (subSection.WorkFlowSectionTypeID)
    //                    {
    //                        case 1:
    //                            SType = "Email_Received_Trigger";
    //                            break;
    //                        case 2:
    //                            SType = "Date_Time_Trigger";
    //                            break;
    //                        case 3:
    //                            SType = "Email_Out_Action";
    //                            break;
                         
    //                        case 4:
    //                            SType = "Columns";
    //                            break;
                         
    //                    }

    //                    string STypeCaption = SType;

    //                    //if (SType == "DashChart")
    //                    //{
    //                    //    STypeCaption = "Chart";
    //                    //}

    //                    if (EditMode)
    //                    {
    //                        sb.AppendLine(String.Format("<div class=\"Section {0}Section\" id=\"SECTION_{1}\" ondblclick=\"EditSection($(this))\">", SType, subSection.WorkFlowSectionID));
    //                        sb.AppendLine("<div class=\"Toolbar\">");
    //                        sb.AppendLine("<a class=\"ui-icon ui-icon-carat-2-n-s\" title=\"Drag and drop to change order\"></a><a class=\"ui-icon ui-icon-pencil\" onclick=\"EditSection($(this).parent().parent())\" title=\"Edit section\"></a><a class=\"ui-icon ui-icon-trash\" onclick=\"RemoveSection($(this).parent().parent())\" title=\"Delete section\"></a>");
    //                        sb.AppendLine("<div class=\"section-type\">" + STypeCaption + "</div>");
    //                        sb.AppendLine("</div>");
    //                        sb.AppendLine("<div class=\"Content\">");
    //                    }
    //                    switch (subSection.WorkFlowSectionTypeID)
    //                    {
    //                        case 1:
    //                            sb.AppendLine(GenerateEmail_Received_TriggerSection(subSection));
    //                            sb.Append("<br/>");
    //                            break;
    //                        case 2:
    //                            sb.AppendLine(GenerateDate_Time_TriggerSection(subSection));
    //                            sb.Append("<br/>");
    //                            break;

    //                        case 3:
    //                            sb.AppendLine(GenerateEmail_Out_ActionSection(subSection));
    //                            sb.Append("<br/>");
    //                            break;
                           
    //                        case 4:
    //                            sb.AppendLine(GenerateColumnsSection(subSection, EditMode));
    //                            sb.Append("<br/>");
    //                            break;
                           
    //                    }
    //                    if (EditMode) sb.AppendLine("</div></div>");
    //                }
    //            }
    //            if (EditMode) sb.AppendLine("   </div>");
    //            sb.AppendLine("</td>");
    //        }
    //    }
    //    sb.AppendLine("</tr>");
    //    sb.AppendLine("</table>");
    //    return sb.ToString();
    //}     


}


public class Email_Received_Trigger : JSONField
{
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class Email_Out_Action : JSONField
{
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class Date_Time_Trigger : JSONField
{
    public int? TimePeriod { get; set; }
    public string TimePeriodUnit { get; set; }
}


public class Columns : JSONField
{
    public List<int> Widths { get; set; }
    public int? Spacing { get; set; }
    public int? NumberOfColumns { get; set; }
}
