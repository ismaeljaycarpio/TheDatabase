using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;

using System.IO;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
//using NPOI.SS;
namespace DBG.Common
{


    public class ExportUtil
    {


        public static string StripTagsCharArray(string HTMLText)
        {
            string strR = "";
            using (SqlConnection conn = new SqlConnection(DBGurus.strGlobalConnectionString))
            {
                using (SqlCommand comm = new SqlCommand("dbo.udf_StripHTML", conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    SqlParameter p1 = new SqlParameter("@HTMLText", SqlDbType.VarChar);
                    // You can call the return value parameter anything, .e.g. "@Result".
                    SqlParameter p2 = new SqlParameter("@Result", SqlDbType.VarChar);

                    p1.Direction = ParameterDirection.Input;
                    p2.Direction = ParameterDirection.ReturnValue;

                    p1.Value = HTMLText;

                    comm.Parameters.Add(p1);
                    comm.Parameters.Add(p2);

                    conn.Open();

                    try
                    {
                        comm.ExecuteNonQuery();

                        if (p2.Value != DBNull.Value)
                            strR = (string)p2.Value;
                    }
                    catch
                    {

                    }
                    conn.Close();
                    conn.Dispose();

                }
            }
            return strR;
        }
        public static void ExportToExcel(DataTable sourceTable, string fileName)
        {
            ExportToExcel(sourceTable, "Sheet 1", fileName);
        }

        public static void ExportToExcel(DataTable sourceTable, string sheetName, string fileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(String.IsNullOrEmpty(sheetName) ? "Sheet 1" : sheetName);
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);

            Dictionary<int, string> dicCellType = new Dictionary<int, string>();
            //HSSFCellStyle boldStyle =(HSSFCellStyle) workbook.CreateCellStyle();
            var boldStyle = workbook.CreateCellStyle();
            var boldFont = workbook.CreateFont();
            boldFont.Boldweight = (short)FontBoldWeight.BOLD;
            boldStyle.SetFont(boldFont);
            

            boldStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            boldStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.MEDIUM;
            boldStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            boldStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;

            boldStyle.TopBorderColor = HSSFColor.BLACK.index;
            boldStyle.RightBorderColor = HSSFColor.BLACK.index;
            boldStyle.BottomBorderColor = HSSFColor.BLACK.index;
            boldStyle.LeftBorderColor = HSSFColor.BLACK.index;

            boldStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            NPOI.HSSF.Record.PaletteRecord palette=new NPOI.HSSF.Record.PaletteRecord();
            HSSFPalette p = new HSSFPalette(palette);

            //p.AddColor(219, 229, 241);

            //boldStyle.FillForegroundColor = ((HSSFColor)p.FindSimilarColor(20, 230, 230)).GetIndex();

            boldStyle.FillForegroundColor = ((HSSFColor)setColor(workbook, 219, 229, 241)).GetIndex();
         
           
            // handling header.
            foreach (DataColumn column in sourceTable.Columns)
            {
                HSSFCell headerCell = (HSSFCell)headerRow.CreateCell(column.Ordinal);
                headerCell.CellStyle = boldStyle;
             
                headerCell.SetCellValue(column.ColumnName);                                
                dicCellType.Add(column.Ordinal, column.DataType.ToString());                
            }
            
            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in sourceTable.Rows)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    HSSFCell dataCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);
                    
                    switch (dicCellType[column.Ordinal])
                    {
                        case "System.Byte":
                        case "System.Short":
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Double":
                        case "System.Decimal":                            
                            if (row[column] != DBNull.Value)
                            {
                                dataCell.SetCellValue(Convert.ToDouble(row[column]));
                            }
                            break;
                        case "System.Bolean":
                            dataCell.SetCellType(CellType.BOOLEAN);
                            dataCell.SetCellValue(Convert.ToString(row[column]));
                            break;
                        //case "System.DateTime":
                        //    if(row[column] != DBNull.Value)
                        //        dataCell.SetCellValue(Convert.ToDateTime(row[column]).ToString("dd/MM/yyyy hh:mm:ss"));
                        //    break;
                        default:
                            try
                            {
                                string strF = Convert.ToString(row[column]);

                                if (strF.Length>32000)
                                {
                                    strF = StripTagsCharArray(strF);
                                }
                                if (strF.Length > 32000)
                                {
                                    strF = strF.Substring(0, 32000);
                                }
                                dataCell.SetCellValue(strF);
                            }
                            catch
                            {
                                //
                            }
                           
                            break;
                    }
                }
                rowIndex++;
            }
            for (int i = 0; i < sourceTable.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
                try
                {
                    sheet.SetColumnWidth(i, Convert.ToInt32(sheet.GetColumnWidth(i) * 1.1));
                }
                catch
                {
                    //sheet.SetColumnWidth(i, 255);
                }
            }
            workbook.Write(memoryStream);
            memoryStream.Flush();

            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            response.BinaryWrite(memoryStream.GetBuffer());
            response.End();
        }



        public static HSSFColor setColor(HSSFWorkbook workbook, byte r, byte g, byte b)
        {
            HSSFPalette palette = workbook.GetCustomPalette();
            HSSFColor hssfColor = null;
            try
            {
                hssfColor = palette.FindColor(r, g, b);
                if (hssfColor == null)
                {
                    palette.SetColorAtIndex(HSSFColor.LAVENDER.index, r, g, b);
                    hssfColor = palette.GetColor(HSSFColor.LAVENDER.index);
                }
            }
            catch (Exception e)
            {
                //logger.error(e);
            }

            return hssfColor;
        }



       
    }
}