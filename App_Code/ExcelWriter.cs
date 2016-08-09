using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using oxc = DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExcelWriter
/// </summary>
public class ExcelWriter
{
    private int _nSeries;
    private int _nRows;
    SortedDictionary<DateTime, int> _dates;
    Dictionary<int, decimal>[] _series;
    
    public ExcelWriter()
    {
        _nSeries = 0;
        _nRows = 0;
        _dates = new SortedDictionary<DateTime, int>();
        _series = new Dictionary<int, decimal>[0];
    }

    public void AddSeries(System.Data.DataTable table, string dateColumnName, string valueColumnName)
    {
        Dictionary<int, decimal> dict = new Dictionary<int, decimal>();
        foreach(DataRow row in table.Rows)
        {
            DateTime dt;
            Decimal value = 0m;
                                   
            if (row[dateColumnName] is DateTime)
                dt = (DateTime)row[dateColumnName];
            else
                dt = DateTime.Parse(row[dateColumnName].ToString());

            if (!Decimal.TryParse(row[valueColumnName].ToString(), out value))
                value = 0m;

            int n = -1;
            if (_dates.ContainsKey(dt))
            {
                n = _dates[dt];
            }
            else
            {
                _nRows++;
                _dates.Add(dt, _nRows);
                n = _nRows;
            }

            if (!dict.ContainsKey(n)) //MR
            {
                dict.Add(n, value);
            }
            
               
           
        }
        Array.Resize<Dictionary<int, decimal>>(ref _series, _nSeries + 1);
        _series[_nSeries] = dict;
        _nSeries++;
    }

    public bool CreateWorkbook(string fileFullName)
    {
        using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Create(
            fileFullName, SpreadsheetDocumentType.Workbook))
        {
            // create the workbook
            spreadSheet.AddWorkbookPart();

            //WorkbookStylesPart wbsp = spreadSheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            //wbsp.Stylesheet = CreateStylesheet();
            //wbsp.Stylesheet.Save();

            spreadSheet.WorkbookPart.Workbook = new Workbook();
            WorksheetPart wsp = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();

            wsp.Worksheet = new Worksheet();
            wsp.Worksheet.AppendChild(new SheetData());

            foreach (DateTime dt in _dates.Keys)
            {
                // create row
                Row r = wsp.Worksheet.First().AppendChild(new Row());

                // create cell with date
                r.AppendChild(new Cell() { CellValue = new CellValue(dt.ToOADate().ToString(CultureInfo.InvariantCulture)),
                                           //StyleIndex = 164,
                                           DataType = new EnumValue<CellValues>(CellValues.Number)
                });

                for (int i = 0; i < _nSeries; i++)
                {
                    r.AppendChild(new Cell() { CellValue = new CellValue(_series[i][_dates[dt]].ToString()),
                        //StyleIndex = 0,
                        DataType = new EnumValue<CellValues>(CellValues.Number)
                    });
                }
            }

            // save worksheet
            wsp.Worksheet.Save();

            InsertChartInSpreadsheet(wsp);

            // create the worksheet to workbook relation
            spreadSheet.WorkbookPart.Workbook.AppendChild(new Sheets());
            spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet()
            {
                Id = spreadSheet.WorkbookPart.GetIdOfPart(spreadSheet.WorkbookPart.WorksheetParts.First()),
                SheetId = 1,
                Name = "test"
            });

            spreadSheet.WorkbookPart.Workbook.Save();

            return true;
        }
    }
    
    private void InsertChartInSpreadsheet(WorksheetPart worksheetPart)
    {
        // Add a new drawing to the worksheet.
        DrawingsPart drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();
        worksheetPart.Worksheet.Append(
            new DocumentFormat.OpenXml.Spreadsheet.Drawing() { Id = worksheetPart.GetIdOfPart(drawingsPart) });
        worksheetPart.Worksheet.Save();

        // Add a new chart and set the chart language to English-AU.
        ChartPart chartPart = drawingsPart.AddNewPart<ChartPart>();
        chartPart.ChartSpace = new ChartSpace();
        chartPart.ChartSpace.Append(new EditingLanguage() { Val = new StringValue("en-AU") });
        DocumentFormat.OpenXml.Drawing.Charts.Chart chart =
            chartPart.ChartSpace.AppendChild<DocumentFormat.OpenXml.Drawing.Charts.Chart>(
                new DocumentFormat.OpenXml.Drawing.Charts.Chart());

        // Title
        chart.AppendChild<Title>(new Title(
            new oxc.Layout(),
            new Overlay() { Val = new BooleanValue(true) },
            new ChartShapeProperties(
                new NoFill(),
                new DocumentFormat.OpenXml.Drawing.Outline(
                    new NoFill()),
                new EffectList()),
            new oxc.TextProperties(
                new BodyProperties()
                {
                    Rotation = 0,
                    UseParagraphSpacing = true,
                    VerticalOverflow = TextVerticalOverflowValues.Ellipsis,
                    Vertical = TextVerticalValues.Horizontal,
                    Wrap = TextWrappingValues.Square,
                    Anchor = TextAnchoringTypeValues.Center,
                    AnchorCenter = true
                },
                new ListStyle(),
                new Paragraph(
                    new ParagraphProperties(
                        new DefaultRunProperties(
                            new SolidFill(
                                new SchemeColor(
                                    new LuminanceModulation() { Val = 65000 },
                                    new LuminanceOffset() { Val = 35000 })
                                { Val = SchemeColorValues.Text1 }),
                            new LatinFont() { Typeface = "+mn-lt" },
                            new EastAsianFont() { Typeface = "+mn-ea" },
                            new ComplexScriptFont() { Typeface = "+mn-cs" })
                        {
                            FontSize = 1400,
                            Bold = false,
                            Italic = false,
                            Underline = TextUnderlineValues.None,
                            Strike = TextStrikeValues.NoStrike,
                            Kerning = 1200,
                            Spacing = 0,
                            Baseline = 0
                        }),
                    new EndParagraphRunProperties() { Language = "en-AU" }))));

        chart.AppendChild<AutoTitleDeleted>(new AutoTitleDeleted() { Val = new BooleanValue(true) });

        // Create a new Line chart.
        PlotArea plotArea = chart.AppendChild<PlotArea>(new PlotArea());
        oxc.Layout layout = plotArea.AppendChild<oxc.Layout>(new oxc.Layout());

        LineChart lineChart = plotArea.AppendChild<LineChart>(new LineChart(
            new Grouping() { Val = GroupingValues.Standard },
            new VaryColors() { Val = false }));

        for (uint i = 0; (i < _nSeries) && (i < 2); i++)
        {
            NumberingCache cacheX = new NumberingCache(
                new FormatCode() { Text = "m/d/yyyy" },
                new PointCount() { Val = (UInt32Value)Convert.ToUInt32(_nRows) }
                );
            uint k = 0;
            foreach (DateTime dt in _dates.Keys)
            {
                NumericPoint np = new NumericPoint(
                    new NumericValue() { Text = dt.ToOADate().ToString() }) { Index = (UInt32Value)k };
                cacheX.Append(np);
                k++;
            }
            NumberReference nrX = new NumberReference(
                new oxc.Formula() { Text = String.Format("test!A1:A{0}", _nRows) },
                cacheX);
            CategoryAxisData categoryAxisData = new CategoryAxisData(nrX);

            NumberingCache cache = new NumberingCache(
                new FormatCode() { Text = "General" },
                new PointCount() { Val = (UInt32Value)Convert.ToUInt32(_nRows) });
            uint j = 0;
            foreach (DateTime dt in _dates.Keys)
            {
                if (_series[i].ContainsKey(_dates[dt]))
                {
                    NumericPoint np = new NumericPoint(
                        new NumericValue() { Text = _series[i][_dates[dt]].ToString() })
                    { Index = (UInt32Value)j };
                    cache.Append(np);
                }
                j++;
            }
            string s = String.Format("test!{0}1:{0}{1}", i == 0 ? "B" : "C", _nRows);
            NumberReference nr = new NumberReference(
                new oxc.Formula() { Text = s },
                cache);
            oxc.Values values = new oxc.Values(nr);

            LineChartSeries lineChartSeries = lineChart.AppendChild<LineChartSeries>(
                new LineChartSeries(
                    new Index() { Val = new UInt32Value(i) },
                    new Order() { Val = new UInt32Value(i) },
                    new ChartShapeProperties(
                        new DocumentFormat.OpenXml.Drawing.Outline(
                            new SolidFill(
                                new SchemeColor() { Val = i == 0 ? SchemeColorValues.Accent1 : SchemeColorValues.Accent2 }),
                            new Round())
                        { Width = 28575, CapType = LineCapValues.Round },
                        new EffectList()),
                    new Marker(
                        new Symbol() { Val = MarkerStyleValues.Circle },
                        new Size() { Val = 5 },
                        new ChartShapeProperties(
                            new SolidFill(
                                new SchemeColor() { Val = i == 0 ? SchemeColorValues.Accent1 : SchemeColorValues.Accent2 }),
                            new DocumentFormat.OpenXml.Drawing.Outline(
                                new SolidFill(
                                    new SchemeColor() { Val = i == 0 ? SchemeColorValues.Accent1 : SchemeColorValues.Accent2 }))
                                { Width = 9525 },
                            new EffectList()
                            )),
                    categoryAxisData,
                    values,
                    new Smooth() { Val = false }));
        }

        lineChart.AppendChild<DataLabels>(new DataLabels(
            new ShowLegendKey(),
            new ShowValue() { Val = new BooleanValue(false) },
            new ShowCategoryName() { Val = new BooleanValue(false) },
            new ShowSeriesName() { Val = new BooleanValue(false) },
            new ShowPercent() { Val = new BooleanValue(false) },
            new ShowBubbleSize() { Val = new BooleanValue(false) }));

        lineChart.AppendChild<ShowMarker>(new ShowMarker() { Val = new BooleanValue(true) });

        lineChart.AppendChild<Smooth>(new Smooth() { Val = new BooleanValue(false) });

        lineChart.Append(new AxisId() { Val = new UInt32Value(48650112u) });
        lineChart.Append(new AxisId() { Val = new UInt32Value(48672768u) });

        // Add the Date Axis.
        DateAxis dateAxis = plotArea.AppendChild<DateAxis>(
            new DateAxis(
                new AxisId() { Val = new UInt32Value(48650112u) },
                new Scaling(
                    new Orientation() { Val = oxc.OrientationValues.MinMax }),
                new Delete() { Val = false },
                new AxisPosition() { Val = new EnumValue<AxisPositionValues>(AxisPositionValues.Bottom) },
                new oxc.NumberingFormat() { FormatCode = "m/d/yyyy", SourceLinked = true },
                new MajorTickMark() { Val = TickMarkValues.Outside },
                new MinorTickMark() { Val = TickMarkValues.None },
                new TickLabelPosition() { Val = new EnumValue<TickLabelPositionValues>(TickLabelPositionValues.NextTo) },
                new ChartShapeProperties(
                    new NoFill(),
                    new DocumentFormat.OpenXml.Drawing.Outline(
                        new SolidFill(
                            new SchemeColor(
                                new LuminanceModulation() { Val = 15000 },
                                new LuminanceOffset() { Val = 85000 }) { Val = SchemeColorValues.Text1 }),
                        new Round())
                        {
                            Width = 9525,
                            CapType = LineCapValues.Flat,
                            CompoundLineType = CompoundLineValues.Single,
                            Alignment = PenAlignmentValues.Center
                        },
                    new EffectList()),
                new DocumentFormat.OpenXml.Drawing.Charts.TextProperties(
                    new BodyProperties()
                    {
                        Rotation = -60000000,
                        UseParagraphSpacing = true,
                        VerticalOverflow = TextVerticalOverflowValues.Ellipsis,
                        Vertical = TextVerticalValues.Horizontal,
                        Wrap = TextWrappingValues.Square,
                        Anchor = TextAnchoringTypeValues.Center,
                        AnchorCenter = true
                    },
                    new ListStyle(),
                    new Paragraph(
                        new ParagraphProperties(
                            new DefaultRunProperties(
                                new SolidFill(
                                    new SchemeColor(
                                        new LuminanceModulation() { Val = 65000 },
                                        new LuminanceOffset() { Val = 35000 }) { Val = SchemeColorValues.Text1 }),
                                new LatinFont() { Typeface = "+mn-lt" },
                                new EastAsianFont() { Typeface = "+mn-ea" },
                                new ComplexScriptFont() { Typeface = "+mn-cs" })
                                {
                                    FontSize = 900,
                                    Bold = false,
                                    Italic = false,
                                    Underline = TextUnderlineValues.None,
                                    Strike = TextStrikeValues.NoStrike,
                                    Kerning = 1200,
                                    Baseline = 0
                                }),
                        new EndParagraphRunProperties() { Language = "en-AU" })),
                new CrossingAxis() { Val = new UInt32Value(48672768U) },
                new Crosses() { Val = CrossesValues.AutoZero },
                new AutoLabeled() { Val = new BooleanValue(true) },
                new LabelAlignment() { Val = LabelAlignmentValues.Center },
                new LabelOffset() { Val = new UInt16Value((ushort)100) },
                new BaseTimeUnit() { Val = TimeUnitValues.Months }));
            
        // Add the Value Axis.
        ValueAxis valAx = plotArea.AppendChild<ValueAxis>(
            new ValueAxis(new AxisId() { Val = new UInt32Value(48672768u) },
                new Scaling(
                    new Orientation() { Val = oxc.OrientationValues.MinMax }),
                new Delete() { Val = false },
                new AxisPosition() { Val = AxisPositionValues.Left },
                new MajorGridlines(
                    new ChartShapeProperties(
                        new DocumentFormat.OpenXml.Drawing.Outline(
                            new SolidFill(
                                new SchemeColor(
                                    new LuminanceModulation() { Val = 15000 },
                                    new LuminanceOffset() { Val = 85000 }) { Val = SchemeColorValues.Text1 }),
                            new Round())
                        {
                            Width = 9525,
                            CapType = LineCapValues.Flat,
                            CompoundLineType = CompoundLineValues.Single,
                            Alignment = PenAlignmentValues.Center
                        },
                        new EffectList())),
                new oxc.NumberingFormat()
                {
                    FormatCode = new StringValue("General"),
                    SourceLinked = new BooleanValue(true)
                },
                new MajorTickMark() { Val = TickMarkValues.None },
                new MinorTickMark() { Val = TickMarkValues.None },
                new TickLabelPosition() { Val = TickLabelPositionValues.NextTo },
                new ChartShapeProperties(
                    new NoFill(),
                    new DocumentFormat.OpenXml.Drawing.Outline(
                        new NoFill()),
                    new EffectList()),
                new oxc.TextProperties(
                    new BodyProperties()
                    {
                        Rotation = -60000000,
                        UseParagraphSpacing = true,
                        VerticalOverflow = TextVerticalOverflowValues.Ellipsis,
                        Vertical = TextVerticalValues.Horizontal,
                        Wrap = TextWrappingValues.Square,
                        Anchor = TextAnchoringTypeValues.Center,
                        AnchorCenter = true
                    },
                    new ListStyle(),
                    new Paragraph(
                        new ParagraphProperties(
                            new DefaultRunProperties(
                                new SolidFill(
                                    new SchemeColor(
                                        new LuminanceModulation() { Val = 65000 },
                                        new LuminanceOffset() { Val = 35000 })
                                    { Val = SchemeColorValues.Text1 }),
                                new LatinFont() { Typeface = "+mn-lt" },
                                new EastAsianFont() { Typeface = "+mn-ea" },
                                new ComplexScriptFont() { Typeface = "+mn-cs" })
                            {
                                FontSize = 900,
                                Bold = false,
                                Italic = false,
                                Underline = TextUnderlineValues.None,
                                Strike = TextStrikeValues.NoStrike,
                                Kerning = 1200,
                                Baseline = 0
                            }),
                        new EndParagraphRunProperties() { Language = "en-AU" })),
                new CrossingAxis() { Val = new UInt32Value(48650112U) },
                new Crosses() { Val = new EnumValue<CrossesValues>(CrossesValues.AutoZero) },
                new CrossBetween() { Val = new EnumValue<CrossBetweenValues>(CrossBetweenValues.Between) }));

        plotArea.AppendChild<oxc.ShapeProperties>(new oxc.ShapeProperties(
            new NoFill(),
            new DocumentFormat.OpenXml.Drawing.Outline(
                new NoFill()),
            new EffectList()));


        // Add the chart Legend
        Legend legend = chart.AppendChild<Legend>(
            new Legend(
                new LegendPosition() { Val = new EnumValue<LegendPositionValues>(LegendPositionValues.Bottom) },
                new oxc.Layout(),
                new Overlay() { Val = new BooleanValue(true) },
                new ChartShapeProperties(
                    new NoFill(),
                    new DocumentFormat.OpenXml.Drawing.Outline(
                        new NoFill()),
                    new EffectList()),
                new oxc.TextProperties(
                    new BodyProperties()
                    {
                        Rotation = 0,
                        UseParagraphSpacing = true,
                        VerticalOverflow = TextVerticalOverflowValues.Ellipsis,
                        Vertical = TextVerticalValues.Horizontal,
                        Wrap = TextWrappingValues.Square,
                        Anchor = TextAnchoringTypeValues.Center,
                        AnchorCenter = true
                    },
                    new ListStyle(),
                    new Paragraph(
                        new ParagraphProperties(
                            new DefaultRunProperties(
                                new SolidFill(
                                    new SchemeColor(
                                        new LuminanceModulation() { Val = 65000 },
                                        new LuminanceOffset() { Val = 35000 })
                                    { Val = SchemeColorValues.Text1 }),
                                new LatinFont() { Typeface = "+mn-lt" },
                                new EastAsianFont() { Typeface = "+mn-ea" },
                                new ComplexScriptFont() { Typeface = "+mn-cs" })
                            {
                                FontSize = 900,
                                Bold = false,
                                Italic = false,
                                Underline = TextUnderlineValues.None,
                                Strike = TextStrikeValues.NoStrike,
                                Kerning = 1200,
                                Baseline = 0
                            }),
                        new EndParagraphRunProperties() { Language = "en-AU" }))));


        chart.AppendChild<PlotVisibleOnly>(new PlotVisibleOnly() { Val = new BooleanValue(true) });
        chart.AppendChild<DisplayBlanksAs>(new DisplayBlanksAs() { Val = new EnumValue<DisplayBlanksAsValues>(DisplayBlanksAsValues.Zero) });
        chart.AppendChild<ShowDataLabelsOverMaximum>(new ShowDataLabelsOverMaximum() { Val = new BooleanValue(false) });

        // Save the chart part.
        chartPart.ChartSpace.Save();

        // Position the chart on the worksheet using a TwoCellAnchor object.
        drawingsPart.WorksheetDrawing = new WorksheetDrawing();
        TwoCellAnchor twoCellAnchor = drawingsPart.WorksheetDrawing.AppendChild<TwoCellAnchor>(new TwoCellAnchor());
        twoCellAnchor.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.FromMarker(
            new ColumnId("4"),
            new ColumnOffset("76200"),
            new RowId("4"),
            new RowOffset("180975")));
        twoCellAnchor.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.ToMarker(
            new ColumnId("13"),
            new ColumnOffset("314325"),
            new RowId("25"),
            new RowOffset("157162")));

        // Append a GraphicFrame to the TwoCellAnchor object.
        DocumentFormat.OpenXml.Drawing.Spreadsheet.GraphicFrame graphicFrame =
            twoCellAnchor.AppendChild<DocumentFormat.OpenXml.Drawing.Spreadsheet.GraphicFrame>(
                new DocumentFormat.OpenXml.Drawing.Spreadsheet.GraphicFrame());
        graphicFrame.Macro = "";

        graphicFrame.Append(
            new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualGraphicFrameProperties(
                new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties() { Id = new UInt32Value(2u), Name = "Chart 1" },
                new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualGraphicFrameDrawingProperties()));

        graphicFrame.Append(
            new Transform(
                new Offset() { X = 0L, Y = 0L },
                new Extents() { Cx = 0L, Cy = 0L }));

        graphicFrame.Append(
            new Graphic(
                new GraphicData(
                    new ChartReference() { Id = drawingsPart.GetIdOfPart(chartPart) })
                { Uri = "http://schemas.openxmlformats.org/drawingml/2006/chart" }));

        twoCellAnchor.Append(new ClientData());

        // Save the WorksheetDrawing object.
        drawingsPart.WorksheetDrawing.Save();
    }
    
    //private static Stylesheet CreateStylesheet()
    //{
    //    Stylesheet ss = new Stylesheet();

    //    Fonts fts = new Fonts();
    //    DocumentFormat.OpenXml.Spreadsheet.Font ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
    //    FontName ftn = new FontName();
    //    ftn.Val = "Calibri";
    //    FontSize ftsz = new FontSize();
    //    ftsz.Val = 11;
    //    ft.FontName = ftn;
    //    ft.FontSize = ftsz;
    //    fts.Append(ft);
    //    fts.Count = (uint)fts.ChildElements.Count;

    //    Fills fills = new Fills();
    //    Fill fill;
    //    PatternFill patternFill;
    //    fill = new Fill();
    //    patternFill = new PatternFill();
    //    patternFill.PatternType = PatternValues.None;
    //    fill.PatternFill = patternFill;
    //    fills.Append(fill);
    //    fill = new Fill();
    //    patternFill = new PatternFill();
    //    patternFill.PatternType = PatternValues.Gray125;
    //    fill.PatternFill = patternFill;
    //    fills.Append(fill);
    //    fills.Count = (uint)fills.ChildElements.Count;

    //    Borders borders = new Borders();
    //    Border border = new Border();
    //    border.LeftBorder = new LeftBorder();
    //    border.RightBorder = new RightBorder();
    //    border.TopBorder = new TopBorder();
    //    border.BottomBorder = new BottomBorder();
    //    border.DiagonalBorder = new DiagonalBorder();
    //    borders.Append(border);
    //    borders.Count = (uint)borders.ChildElements.Count;

    //    CellStyleFormats csfs = new CellStyleFormats();
    //    CellFormat cf = new CellFormat();
    //    cf.NumberFormatId = 0;
    //    cf.FontId = 0;
    //    cf.FillId = 0;
    //    cf.BorderId = 0;
    //    csfs.Append(cf);
    //    csfs.Count = (uint)csfs.ChildElements.Count;

    //    uint iExcelIndex = 164;
    //    NumberingFormats nfs = new NumberingFormats();
    //    CellFormats cfs = new CellFormats();

    //    cf = new CellFormat();
    //    cf.NumberFormatId = 0;
    //    cf.FontId = 0;
    //    cf.FillId = 0;
    //    cf.BorderId = 0;
    //    cf.FormatId = 0;
    //    cfs.Append(cf);

    //    NumberingFormat nf;
    //    nf = new NumberingFormat();
    //    nf.NumberFormatId = iExcelIndex++;
    //    nf.FormatCode = "dd/mm/yyyy hh:mm:ss";
    //    nfs.Append(nf);
    //    cf = new CellFormat();
    //    cf.NumberFormatId = nf.NumberFormatId;
    //    cf.FontId = 0;
    //    cf.FillId = 0;
    //    cf.BorderId = 0;
    //    cf.FormatId = 0;
    //    cf.ApplyNumberFormat = true;
    //    cfs.Append(cf);

    //    //nf = new NumberFormat();
    //    //nf.NumberFormatId = iExcelIndex++;
    //    //nf.FormatCode = "#,##0.0000";
    //    //nfs.Append(nf);
    //    //cf = new CellFormat();
    //    //cf.NumberFormatId = nf.NumberFormatId;
    //    //cf.FontId = 0;
    //    //cf.FillId = 0;
    //    //cf.BorderId = 0;
    //    //cf.FormatId = 0;
    //    //cf.ApplyNumberFormat = true;
    //    //cfs.Append(cf);

    //    //// #,##0.00 is also Excel style index 4
    //    //nf = new NumberFormat();
    //    //nf.NumberFormatId = iExcelIndex++;
    //    //nf.FormatCode = "#,##0.00";
    //    //nfs.Append(nf);
    //    //cf = new CellFormat();
    //    //cf.NumberFormatId = nf.NumberFormatId;
    //    //cf.FontId = 0;
    //    //cf.FillId = 0;
    //    //cf.BorderId = 0;
    //    //cf.FormatId = 0;
    //    //cf.ApplyNumberFormat = true;
    //    //cfs.Append(cf);

    //    //// @ is also Excel style index 49
    //    //nf = new NumberFormat();
    //    //nf.NumberFormatId = iExcelIndex++;
    //    //nf.FormatCode = "@";
    //    //nfs.Append(nf);
    //    //cf = new CellFormat();
    //    //cf.NumberFormatId = nf.NumberFormatId;
    //    //cf.FontId = 0;
    //    //cf.FillId = 0;
    //    //cf.BorderId = 0;
    //    //cf.FormatId = 0;
    //    //cf.ApplyNumberFormat = true;
    //    //cfs.Append(cf);

    //    nfs.Count = (uint)nfs.ChildElements.Count;
    //    cfs.Count = (uint)cfs.ChildElements.Count;

    //    ss.Append(nfs);
    //    ss.Append(fts);
    //    ss.Append(fills);
    //    ss.Append(borders);
    //    ss.Append(csfs);
    //    ss.Append(cfs);

    //    CellStyles css = new CellStyles();
    //    CellStyle cs = new CellStyle();
    //    cs.Name = "Normal";
    //    cs.FormatId = 0;
    //    cs.BuiltinId = 0;
    //    css.Append(cs);
    //    css.Count = (uint)css.ChildElements.Count;
    //    ss.Append(css);

    //    DifferentialFormats dfs = new DifferentialFormats();
    //    dfs.Count = 0;
    //    ss.Append(dfs);

    //    TableStyles tss = new TableStyles();
    //    tss.Count = 0;
    //    tss.DefaultTableStyle = "TableStyleMedium9";
    //    tss.DefaultPivotStyle = "PivotStyleLight16";
    //    ss.Append(tss);

    //    return ss;
    //}
}