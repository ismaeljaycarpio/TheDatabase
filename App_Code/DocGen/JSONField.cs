using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DocGen.DAL
{
    [Serializable]
    public class JSONField
    {
        public string GetJSONString()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(this);
        }

        public static T GetTypedObject<T>(string JSONString)
        {
            if (String.IsNullOrEmpty(JSONString))
                JSONString = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<T>(JSONString);
        }
    }
}