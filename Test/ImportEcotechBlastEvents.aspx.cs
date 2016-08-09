using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class Automation_ImportEcotechBlastEvents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            EcotechBlastDataImport ecotechBlastDataImport = new EcotechBlastDataImport();

            Status_Label.Text = ecotechBlastDataImport.ImportBlastEvents();
        }
        catch (Exception ex)
        {
            DBGurus.AddErrorLog(ex.Message);
            throw;
        }
        
    }
}

