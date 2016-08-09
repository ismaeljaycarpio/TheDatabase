using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocGen.DAL
{
    public class ChartSectionDetail : JSONField
    {
        public string Comment { get; set; }
        public int ChartDefinition { get; set; }
        public string Series { get; set; }
        public string ValueFields { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string XAxisTitle { get; set; }
        public string XAxisLabelFormat { get; set; }
        public string YAxisTitle { get; set; }
        public string YAxisLabelFormat { get; set; }
        public int PlotAreaWidth { get; set; }
        public int PlotAreaHeight { get; set; }
        public string LegendPosition { get; set; }
        public bool Display3D { get; set; }
        public double AlphaLevel { get; set; }
        public string GraphType { get; set; }
        public string GraphType2 { get; set; }
        public double? Low { get; set; }
        public double? High { get; set; }
        public double? Low2 { get; set; }
        public double? High2 { get; set; }
        public string Series2 { get; set; }
        public bool? IsUseReportDate { get; set; }
        public bool? HideDate { get; set; }

    }

    public class TableSectionOtherInfo : JSONField
    {
        public bool? IsUseReportDate { get; set; }

        public bool? ShowTimeWithDate { get; set; }
        public int? RecentDays { get; set; }
        public string TableType { get; set; }
        public string SystemTableName { get; set; }

    }
}