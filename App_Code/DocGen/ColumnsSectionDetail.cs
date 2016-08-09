using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ColumnsSectionDetail
/// </summary>
/// 
namespace DocGen.DAL
{
    public class ColumnsSectionDetail : JSONField
    {
        public List<int> Widths { get; set; }
        public int? Spacing { get; set; }
    }
}