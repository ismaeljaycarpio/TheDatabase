using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Security : System.Web.UI.Page
{
    User _objUser;
    UserRole _theUserRole;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (Session["User"] != null)
            {
                _objUser = (User)Session["User"];
                _theUserRole = (UserRole)Session["UserRole"];
                //if (Session["TableIDTryCount"] == null)
                //{
                //    Session["TableIDTryCount"] = 1;
                //}
                //else
                //{
                //    Session["TableIDTryCount"] = int.Parse(Session["TableIDTryCount"].ToString()) + 1;
                //}

                if ((bool)_theUserRole.IsAdvancedSecurity)
                {
                    if (Session["STs"] != null)
                    {
                        string[] strSTs = Session["STs"].ToString().Split(',');
                        int i = 1;
                        foreach (string strST in strSTs)
                        {
                            if (strST != "-1")
                            {

                                //if (i.ToString() == Session["TableIDTryCount"].ToString())
                                //{
                                    //Response.Redirect("~/Pages/Record/RecordList.aspx?TableID=" + Cryptography.Encrypt(strST), true);
                                Response.Redirect("~/Empty.aspx", false);
                                return;
                                //}
                                //i = i + 1;
                            }
                        }
                    }

                }
            }
        }
        catch
        {
            //
        }


    }
}