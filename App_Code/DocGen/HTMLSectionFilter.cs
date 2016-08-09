using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocGen.DAL
{
    public class HTMLSectionFilter : JSONField
    {
        private List<SPInputParam> _Params; 
        public string SPName { get; set; }
        public List<SPInputParam> Params
        {
            get
            {
                return _Params;
            }
            set
            {
                _Params = value;
            }
        }
    }
}