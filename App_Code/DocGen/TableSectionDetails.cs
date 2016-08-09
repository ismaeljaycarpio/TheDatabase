using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocGen.DAL
{
    public class TableSectionDetails : JSONField
    {
        private List<TableSectionColumn> _Columns;
        public List<TableSectionColumn> Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }
        }

        public TableSectionDetails()
        {
            _Columns = new List<TableSectionColumn>();
        }
    }

    public class TableSectionColumn
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public bool Visible { get; set; }
        public int Position { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
        public string Alignment { get; set; }
    }
}