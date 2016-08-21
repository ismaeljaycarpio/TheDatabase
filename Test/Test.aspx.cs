using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using System.Data;
using Microsoft.Office.Interop.Excel;
using HtmlAgilityPack;
public partial class Test_Test : System.Web.UI.Page
{
   

    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //BrowserSession browserSession = new BrowserSession();

        //browserSession.Post("https://www.google.com/accounts/Logout");


        //browserSession.Get("https://accounts.google.com/ServiceLogin?service=mail&passive=true&rm=false&continue=https://mail.google.com/mail/&ss=1&scc=1&ltmpl=default&ltmplcache=2&emr=1&osid=1#identifier");

        ////Fill Username and Password, set Remember Me checkbox to checked 
        //browserSession.FormElements["Email"] = "dbgurustest@gmail.com";

        //HtmlNode htmlElmt = (HtmlNode)browserSession.FormElements["next"];

        //browserSession.Get("https://accounts.google.com/ServiceLogin?service=mail&passive=true&rm=false&continue=https://mail.google.com/mail/&ss=1&scc=1&ltmpl=default&ltmplcache=2&emr=1&osid=1#identifier");

        

        //browserSession.FormElements["Passwd"] = "dbg1234!";
        //browserSession.FormElements["CbRememberMe"] = "on";

        Session["tdbmsg"] = "INVALID: Air Temperature at 2m (C) greater than: 0.007";
        Response.Redirect("~/Default.aspx", true);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {




        System.Data.DataTable dt = new System.Data.DataTable();

        int iNoOfColumns = 20;//450

        for (int i = 0; i < iNoOfColumns; i++)
        {
            dt.Columns.Add("TestC" + i.ToString());
        }
        dt.AcceptChanges();

        DateTime dtBase = new DateTime(2000,01,01);


        int iNoOfRows = 100000;

        for (int i = 0; i < iNoOfRows; i++)
        {
            DataRow dr;
            dr = dt.NewRow();

            for (int j = 0; j < iNoOfColumns; j++)
            {
                if(j>15 && j<20)
                {
                    dr[j] = dtBase.AddDays((double)(i + j)).ToString();
                    //dr[j] = dtBase.AddDays((double)(i + j)).ToShortDateString();
                }
                //else if (j > 10 && j < 15)
                //{
                //    dr[j] = dtBase.AddDays((double)(i + j)).ToString();
                //}
                //else if (j > 15 && j < 20)
                //{
                //    dr[j] = dtBase.AddHours((double)(i + j)).ToLongTimeString();
                //}
                //else if (j > 20 && j < 25)
                //{
                //    dr[j] = dtBase.AddHours((double)(i + j)).ToShortTimeString();
                //}
                //else if (j > 25 && j < 30)
                //{
                //    dr[j] = dtBase.AddHours((double)(i + j)).ToUniversalTime();
                //}
                //else if (j > 30 && j < 35)
                //{
                //    dr[j] = dtBase.AddDays((double)(i + j)).ToUniversalTime();
                //}
                //else if (j > 35 && j < 40)
                //{
                //    dr[j] = dtBase.AddDays((double)(i + j)).ToLocalTime();
                //}
                //else if (j > 40 && j < 45)
                //{
                //    dr[j] = dtBase.AddDays((double)(i + j)).ToJulianDate();
                //}
                //else if (j > 45 && j < 50)
                //{
                //    dr[j] = dtBase.AddDays((double)(i + j)).ToFileTime();
                //}
                //else if (j > 50 && j < 55)
                //{
                //    dr[j] = dtBase.AddDays((double)(i + j)).ToFileTimeUtc();
                //}
                //else if (j > 55 && j < 60)
                //{
                //    dr[j] = dtBase.AddDays((double)(i + j)).ToOADate();
                //}
                //else if (j > 60 && j < 65)
                //{
                //    dr[j] = dtBase.AddDays((double)(i + j)).ToOADate()*(i+j);
                //}
                else
                {
                    dr[j] = i + j;
                }
                
            }

            dt.Rows.Add(dr);
        }

        dt.AcceptChanges();




        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        Microsoft.Office.Interop.Excel.Range excelCellrange;

        // Start Excel and get Application object
        excel = new Microsoft.Office.Interop.Excel.Application();
        // for making Excel visible
        excel.Visible = false;
        excel.DisplayAlerts = false;
        // Creation a new Workbook
        excelworkBook = excel.Workbooks.Add(Type.Missing);
        // Workk sheet
        excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
        excelSheet.Name = "Test work sheet";

        try
        {
           
            excelSheet.Cells[1, 1] = "Delete this";
            excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

            // loop through each row and add values to our sheet
            int rowcount = 2;

            foreach (DataRow datarow in dt.Rows)
            {
                rowcount += 1;
                for (int i = 1; i <= dt.Columns.Count; i++)
                {
                    // on the first iteration we add the column headers
                    if (rowcount == 3)
                    {
                        excelSheet.Cells[2, i] = dt.Columns[i - 1].ColumnName;
                        //excelSheet.Cells.Font.Color = System.Drawing.Color.Black;

                    }

                    excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();

                    ////for alternate rows
                    //if (rowcount > 3)
                    //{
                    //    if (i == dt.Columns.Count)
                    //    {
                    //        if (rowcount % 2 == 0)
                    //        {
                    //            excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dt.Columns.Count]];
                    //            FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
                    //        }

                    //    }
                    //}

                }

            }

            // now we resize the columns
            excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dt.Columns.Count]];
            excelCellrange.EntireColumn.AutoFit();
            Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
            border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            border.Weight = 2d;


            excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, dt.Columns.Count]];
            FormattingExcelCells(excelCellrange, "#000099", System.Drawing.Color.White, true);


            //now save the workbook and exit Excel


            string strUniqueName = Guid.NewGuid().ToString() + "_" + "Test_Large_File_" + DateTime.Today.ToString("yyyyMMdd") + ".xlsx";

            strUniqueName = Common.GetValidFileName(strUniqueName);
            string strFolderPath = System.Web.HttpContext.Current.Server.MapPath("~\\ExportedFiles");
            string strPath = strFolderPath + "\\" + strUniqueName;


            excelworkBook.SaveAs(strPath); ;
            excelworkBook.Close();
            excel.Quit();

            releaseObject(excelCellrange);
            releaseObject(excelSheet);
            releaseObject(excelworkBook);
            releaseObject(excel);

            lblMsg.Text = "Done";
        }
        catch(Exception ex)
        {
            lblMsg.Text = "Error:" + ex.Message + "----" + ex.StackTrace;
            releaseObject(excelSheet);
            releaseObject(excelworkBook);
            releaseObject(excel);
        }
        finally
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        //DBG.Common.ExportUtil.ExportToExcel(dt, "\"" + "Test_Large_File " + DateTime.Today.ToString("yyyyMMdd") + ".xls" + "\"");
    }
    private void releaseObject(object obj)
    {
        try
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            obj = null;
        }
        catch (Exception ex)
        {
            obj = null;
            //MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
        }
        finally
        {
            GC.Collect();
        }
    }

    public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
    {
        range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorCode);
        range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontColor);
        if (IsFontbool == true)
        {
            range.Font.Bold = IsFontbool;
        }
    }
}