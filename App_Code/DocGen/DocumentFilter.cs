using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocGen.DAL
{
    public class DocumentFilter : JSONField
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}