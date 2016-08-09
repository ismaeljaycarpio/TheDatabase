using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocGen.DAL
{
    public class ChartSectionValueFields : JSONField
    {
        private List<ChartSectionValueField> _Fields;
        public List<ChartSectionValueField> Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
            }
        }

        public ChartSectionValueFields()
        {
            _Fields = new List<ChartSectionValueField>();
        }
    }

    public class ChartSectionValueField
    {
        public int Tableid { get; set; }
        public string field { get; set; }
        public string text { get; set; }
        public string chartType { get; set; }
    }
}