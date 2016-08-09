using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DocGen
{
    public class DocGenPageBase : System.Web.UI.Page
    {
        public int AccountID
        {
            get
            {                
                int retVal = 0;
                if (Session["AccountID"] != null)
                    retVal = Convert.ToInt32(Session["AccountID"]);                
                if(retVal == 0)
                    FormsAuthentication.SignOut();
                return retVal;
            }
        }
    }
}