using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Graph_GraphDefDetail : System.Web.UI.Page
{
    string _strActionMode = "view";
    int? _iGraphDefinitionID;
    string _qsMode = "";
    string _qsGraphDefinitionID = "";
    User _objUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        _objUser = (User)Session["User"];
        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1,2"))
            { Response.Redirect("~/Default.aspx", false); }

            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlBack.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/GraphDef.aspx?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString();
            }
            else
            {
                Response.Redirect(Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Graph/GraphDef.aspx", false);//i think no need
            }

            PopulateEachTableDDL();
        }
        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;

                if (Request.QueryString["GraphDefinitionID"] != null)
                {
                    _qsGraphDefinitionID = Cryptography.Decrypt(Request.QueryString["GraphDefinitionID"]);
                    _iGraphDefinitionID = int.Parse(_qsGraphDefinitionID);
                }
            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }
        }

        // checking permission

        switch (_strActionMode.ToLower())
        {
            case "add":
                lblTitle.Text = "Add Graph Definition";
                break;
            case "view":
                lblTitle.Text = "View Graph Definition";
                PopulateTheRecord();
                EnableTheRecordControls(false);
                divSave.Visible = false;
                break;
            case "edit":
                lblTitle.Text = "Edit Graph Definition";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;
            default:
                //?
                break;
        }
        Title = lblTitle.Text;
    }

    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;
            //List<GraphDefinition> listGraphDefinition = SystemData.GraphDefinition_Select(_iGraphDefinitionID, "", "", "", null, null, "GraphDefinitionID", "ASC", null, null, ref iTemp);

            GraphDefinition theGraphDefinition = GraphManager.ets_GraphDefinition_Detail((int)_iGraphDefinitionID);

            txtDefinitionName.Text = theGraphDefinition.DefinitionName;
            txtDefinitionScript.Text = theGraphDefinition.Definition.Replace("&lt;", "<").Replace("&gt;", ">");
            cbIsSytem.Checked = (theGraphDefinition.IsSystem.HasValue && theGraphDefinition.IsSystem.Value);
            cbIsHidden.Checked = (theGraphDefinition.IsHidden.HasValue && theGraphDefinition.IsHidden.Value);
            txtDefinitionKey.Text = theGraphDefinition.DefinitionKey;
            ddlEachTable.SelectedValue = theGraphDefinition.TableID.HasValue ? theGraphDefinition.TableID.Value.ToString() : "-1";
            if (theGraphDefinition.TableID.HasValue)
            {
                PopulateEachAnalyteDDL(theGraphDefinition.TableID.Value);
            }
            ddlEachAnalyte.SelectedValue = theGraphDefinition.ColumnID.HasValue ? theGraphDefinition.ColumnID.Value.ToString() : "-1";
            ddlColumn1.SelectedValue = theGraphDefinition.DataColumn1ID.HasValue ? theGraphDefinition.DataColumn1ID.Value.ToString() : "-1";
            ddlColumn2.SelectedValue = theGraphDefinition.DataColumn2ID.HasValue ? theGraphDefinition.DataColumn2ID.Value.ToString() : "-1";
            ddlColumn3.SelectedValue = theGraphDefinition.DataColumn3ID.HasValue ? theGraphDefinition.DataColumn3ID.Value.ToString() : "-1";
            ddlColumn4.SelectedValue = theGraphDefinition.DataColumn4ID.HasValue ? theGraphDefinition.DataColumn4ID.Value.ToString() : "-1";


            if (_strActionMode == "edit")
            {
                ViewState["theGraphDefinition"] = theGraphDefinition;
                if (theGraphDefinition.IsActive.HasValue && theGraphDefinition.IsActive.Value)
                {
                    divDelete.Visible = true;
                }
                else
                {
                    divUnDelete.Visible = true;
                }
            }
            else if (_strActionMode == "view")
            {
                divEdit.Visible = true;
                hlEditLink.NavigateUrl = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath + "/Graph/GraphDefDetail.aspx?mode=" + Cryptography.Encrypt("edit") + "&SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&GraphDefinitionID=" + Cryptography.Encrypt(theGraphDefinition.GraphDefinitionID.ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }


    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtDefinitionName.Enabled = p_bEnable;
    }


    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action
        return true;
    }


    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsUserInputOK())
            {
                switch (_strActionMode.ToLower())
                {
                    case "add":
                        //DataTable dtTemp = Common.DataTableFromText("SELECT GraphDefinitionID FROM GraphDefinition WHERE DefinitionName ='" + txtDefinitionName.Text + "' " +
                        //    "AND IsActive = 1");
                        //if (dtTemp.Rows.Count > 0)
                        //{

                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", @"alert('Graph Definition name \'" + txtDefinitionName.Text + @"\' already exist, please try another name.');", true);
                        //    return;
                        //}
                        //else
                        //{
                        GraphDefinition theGraphDefinition = new GraphDefinition(null, null,
                            txtDefinitionName.Text, txtDefinitionScript.Text,
                            cbIsSytem.Checked, cbIsHidden.Checked,
                            ddlEachTable.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEachTable.SelectedValue),
                            ddlEachAnalyte.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEachAnalyte.SelectedValue),
                            String.IsNullOrEmpty(txtDefinitionKey.Text) ? null : txtDefinitionKey.Text,
                            true,
                            ddlColumn1.SelectedValue == "-1" ? null : (int?)int.Parse(ddlColumn1.SelectedValue),
                            ddlColumn2.SelectedValue == "-1" ? null : (int?)int.Parse(ddlColumn2.SelectedValue),
                            ddlColumn3.SelectedValue == "-1" ? null : (int?)int.Parse(ddlColumn3.SelectedValue),
                            ddlColumn4.SelectedValue == "-1" ? null : (int?)int.Parse(ddlColumn4.SelectedValue)
                            );
                        int id = GraphManager.ets_GraphDefinition_Insert(theGraphDefinition);
                        break;
                        //}
                    case "view":
                        break;
                    case "edit":
                        GraphDefinition editGraphDefinition = (GraphDefinition)ViewState["theGraphDefinition"];
                        //DataTable dtTemp2 = Common.DataTableFromText("SELECT GraphDefinitionID FROM GraphDefinition WHERE GraphDefinitionID <> " +
                        //    editGraphDefinition.GraphDefinitionID.ToString() + " AND DefinitionName = '" + txtDefinitionName.Text + "' " +
                        //    "AND IsActive = 1");
                        //if (dtTemp2.Rows.Count > 0)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", @"alert('Graph Definition name \'" + txtDefinitionName.Text + @"\' already exist, please try another name.');", true);
                        //    return;
                        //}
                        //else
                        //{
                        editGraphDefinition.DefinitionName = txtDefinitionName.Text;
                        editGraphDefinition.Definition = txtDefinitionScript.Text;
                        editGraphDefinition.IsSystem = cbIsSytem.Checked;
                        editGraphDefinition.IsHidden = cbIsHidden.Checked;
                        editGraphDefinition.DefinitionKey = String.IsNullOrEmpty(txtDefinitionKey.Text) ? null : txtDefinitionKey.Text;
                        editGraphDefinition.TableID = ddlEachTable.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEachTable.SelectedValue);
                        editGraphDefinition.ColumnID = ddlEachAnalyte.SelectedValue == "-1" ? null : (int?)int.Parse(ddlEachAnalyte.SelectedValue);
                        editGraphDefinition.DataColumn1ID = ddlColumn1.SelectedValue == "-1" ? null : (int?)int.Parse(ddlColumn1.SelectedValue);
                        editGraphDefinition.DataColumn2ID = ddlColumn2.SelectedValue == "-1" ? null : (int?)int.Parse(ddlColumn2.SelectedValue);
                        editGraphDefinition.DataColumn3ID = ddlColumn3.SelectedValue == "-1" ? null : (int?)int.Parse(ddlColumn3.SelectedValue);
                        editGraphDefinition.DataColumn4ID = ddlColumn4.SelectedValue == "-1" ? null : (int?)int.Parse(ddlColumn4.SelectedValue);
                        GraphManager.ets_GraphDefinition_Update(editGraphDefinition);
                        //}

                        break;
                    default:
                        //?
                        break;
                }
            }
            else
            {
                //user input is not ok
            }
            Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        GraphDefinition editGraphDefinition = (GraphDefinition)ViewState["theGraphDefinition"];
        int? GraphDefinitionID = editGraphDefinition.GraphDefinitionID;
        try
        {
            if (GraphManager.ets_GraphDefinition_Delete(GraphDefinitionID.Value) == -2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg_delete", @"alert('Couldn\'t delete System Graph Definition');", true);
            }
            else
            {
                Response.Redirect(hlBack.NavigateUrl, false);
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void lnkUnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            GraphDefinition editGraphDefinition = (GraphDefinition)ViewState["theGraphDefinition"];
            int? GraphDefinitionID = editGraphDefinition.GraphDefinitionID;
            DataTable dtTemp = Common.DataTableFromText("SELECT GraphDefinitionID FROM GraphDefinition " +
                "WHERE GraphDefinitionID <> " + GraphDefinitionID.ToString() + " AND DefinitionName = '" + txtDefinitionName.Text + "' " +
                "AND IsActive = 1");
            if (dtTemp.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", @"alert('Couldn\'t restore: Graph Definition name \'" + txtDefinitionName.Text + @"\' already exist.');", true);
            }
            else
            {
                GraphManager.ets_GraphDefinition_Restore(GraphDefinitionID.Value);
                Response.Redirect(hlBack.NavigateUrl, false);
            }
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void PopulateEachTableDDL()
    {
        int iTN = 0;

        ddlEachTable.DataSource = RecordManager.ets_Table_Select(null,
            null,
            null,
            int.Parse(Session["AccountID"].ToString()),
            null, null, true,
            "st.TableName", "ASC",
            null, null, ref iTN, null);
        ddlEachTable.DataBind();
    }

    protected void PopulateEachAnalyteDDL(int TableID)
    {
        try
        {
            ddlEachAnalyte.Items.Clear();

            ddlColumn1.Items.Clear();
            ddlColumn2.Items.Clear();
            ddlColumn3.Items.Clear();
            ddlColumn4.Items.Clear();
            string strTableID = ddlEachTable.SelectedValue;

            int iTN = 0;

            List<Column> lstColumns = RecordManager.ets_Table_Columns(TableID,
                    null, null, ref iTN);

            Column dtColumn = new Column();
            foreach (Column eachColumn in lstColumns.AsQueryable().Where(eachColumn =>
                (eachColumn.IsStandard == false) && (!String.IsNullOrEmpty(eachColumn.GraphLabel) && (eachColumn.ColumnType == "number"))))
            {
                System.Web.UI.WebControls.ListItem aItem = new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString());
                ddlEachAnalyte.Items.Insert(ddlEachAnalyte.Items.Count, aItem);

                ddlColumn1.Items.Insert(ddlColumn1.Items.Count, new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString()));
                ddlColumn2.Items.Insert(ddlColumn2.Items.Count, new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString()));
                ddlColumn3.Items.Insert(ddlColumn3.Items.Count, new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString()));
                ddlColumn4.Items.Insert(ddlColumn4.Items.Count, new System.Web.UI.WebControls.ListItem(eachColumn.GraphLabel, eachColumn.ColumnID.ToString()));
            }
            ddlEachAnalyte.Items.Insert(0, new ListItem("-- All --", "-1"));

            ddlColumn1.Items.Insert(0, new ListItem("-- None --", "-1"));
            ddlColumn2.Items.Insert(0, new ListItem("-- None --", "-1"));
            ddlColumn3.Items.Insert(0, new ListItem("-- None --", "-1"));
            ddlColumn4.Items.Insert(0, new ListItem("-- None --", "-1"));
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Graph Definition Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }
    }

    protected void ddlEachTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateEachAnalyteDDL(int.Parse(ddlEachTable.SelectedValue));
    }

    protected void ddlEachAnalyte_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlColumn1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlColumn2_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlColumn3_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlColumn4_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void lnkConvert_Click(object sender, EventArgs e)
    {
        string s = txtDefinitionScript.Text;

        s = CheckRoot(s);
        s = ReplaceContainer(s);
        s = SetChartElement(s);
        s = SetTitleElement(s);
        s = SetSubtitleElement(s);
        s = ClearXAxisElement(s);
        s = SetYAxisElement(s);
        s = ReplaceDataSource(s);

        txtDefinitionScript.Text = s.Replace("&lt;", "<").Replace("&gt;", ">");
    }

    private string CheckRoot(string s)
    {
        if (String.IsNullOrEmpty(s))
        {
            s = "$(function () {" + "\n" +
                "    $('#WebChartViewer1').highcharts({" + "\n" +
                "    });" + "\n" +
                "});";
        }

        return s;
    }

    private string ReplaceContainer(string s)
    {
        return s.Replace("#container", "#WebChartViewer1");
    }

    private string SetChartElement(string s)
    {
        Regex r = new Regex(@"(\bchart\s*:\s*\{)");
        Match match = r.Match(s);
        if (match.Success)
        {
            int i = match.Index;
            int l = match.Length;
            s = s.Insert(i + l, "\n" +
                "            height: Number($('#chartHeight').val())," + "\n" +
                "            width: Number($('#chartWidth').val()),");
        }
        else
        {
            r = new Regex(@"\$\(\s*'#WebChartViewer1'\s*\)\.highcharts\s*\(\s*\{");
            match = r.Match(s);
            if (match.Success)
            {
                int i = match.Index;
                int l = match.Length;
                s = s.Insert(i + l, "\n" +
                    "        chart: {" + "\n" +
                    "            height: Number($('#chartHeight').val())," + "\n" +
                    "            width: Number($('#chartWidth').val())" + "\n" +
                    "        },");
            }
        }

        return s;
    }

    private string SetTitleElement(string s)
    {
        bool isFound = false;

        Regex r = new Regex(@"(\btitle\s*:\s*\{)");
        MatchCollection matches = r.Matches(s);
        foreach (Match m in matches)
        {
            if (CheckLevel(m, s, 2))
            {
                int i = m.Index;
                int l = m.Length;
                int index = i + l;
                int counter = 1;
                while (index < s.Length)
                {
                    if (s[index] == '{')
                        counter++;
                    else if (s[index] == '}')
                        counter--;
                    if (counter == 0)
                        break;
                    index++;
                }
                if (index != s.Length)
                {
                    s = s.Remove(i + l, index - i - l);
                    s = s.Insert(i + l, "\n" +
                        "            text: $('#chartTitle').prop('textContent')" + "\n" + new String(' ', 8));
                    isFound = true;
                    break;
                }
            }
        }
        if (!isFound)
        {
            r = new Regex(@"(\bchart\s*:\s*\{)");
            Match match = r.Match(s);
            int i = match.Index;
            int l = match.Length;
            int index = i + l;
            int counter = 1;
            while (index < s.Length)
            {
                if (s[index] == '{')
                    counter++;
                else if (s[index] == '}')
                    counter--;
                if (counter == 0)
                    break;
                index++;
            }
            index += 3;
            if (index < s.Length)
            {
                s = s.Insert(index,
                    "        title: {" + "\n" +
                    "            text: $('#chartTitle').prop('textContent')" + "\n" +
                    "        }," + "\n");
            }
        }

        return s;
    }

    private string SetSubtitleElement(string s)
    {
        bool isFound = false;
        Regex r = new Regex(@"(\bsubtitle\s*:\s*\{)");
        MatchCollection matches = r.Matches(s);
        foreach (Match m in matches)
        {
            if (CheckLevel(m, s, 2))
            {
                int i = m.Index;
                int l = m.Length;
                int index = i + l;
                int counter = 1;
                while (index < s.Length)
                {
                    if (s[index] == '{')
                        counter++;
                    else if (s[index] == '}')
                        counter--;
                    if (counter == 0)
                        break;
                    index++;
                }
                if (index != s.Length)
                {
                    s = s.Remove(i + l, index - i - l);
                    s = s.Insert(i + l, "\n" +
                        "            text: $('#chartSubtitle').prop('textContent')" + "\n" + new String(' ', 8));
                    isFound = true;
                    break;
                }
            }
        }
        if (!isFound)
        {
            r = new Regex(@"(\btitle\s*:\s*\{)");
            Match match = r.Match(s);
            int i = match.Index;
            int l = match.Length;
            int index = i + l;
            int counter = 1;
            while (index < s.Length)
            {
                if (s[index] == '{')
                    counter++;
                else if (s[index] == '}')
                    counter--;
                if (counter == 0)
                    break;
                index++;
            }
            index += 3;
            if (index < s.Length)
            {
                s = s.Insert(index,
                    "        subtitle: {" + "\n" +
                    "            text: $('#chartSubtitle').prop('textContent')" + "\n" +
                    "        }," + "\n");
            }
        }

        return s;
    }

    private string ClearXAxisElement(string s)
    {
        Regex r = new Regex(@"(\bxAxis\s*:\s*\{)");
        Match match = r.Match(s);
        if (match.Success)
        {
            int i = match.Index;
            int l = match.Length;
            int index = i + l;
            int counter = 1;
            while (index < s.Length)
            {
                if (s[index] == '{')
                    counter++;
                else if (s[index] == '}')
                    counter--;
                if (counter == 0)
                    break;
                index++;
            }
            int n = 1;
            if ((index + n != s.Length) && (s[index + n] == ','))
            {
                n = 2;
                if ((index + n != s.Length) && (s[index + n] == '\n'))
                {
                    n = 3;
                    while (true)
                    {
                        if ((index + n != s.Length) && ((s[index + n] == ' ') || (s[index + n] == '\t')))
                            n++;
                        else
                            break;
                    }
                }
            }
            if (index != s.Length)
            {
                s = s.Remove(i, index - i + n);
            }
        }

        return s;
    }

    private string SetYAxisElement(string s)
    {
        string substr = String.Empty;
        bool isFound = false;
        Regex r = new Regex(@"(\byAxis\s*:\s*\{)");
        Match match = r.Match(s);
        if (match.Success)
        {
            int i = match.Index;
            int l = match.Length;
            int index = i + l;
            int counter = 1;
            while (index < s.Length)
            {
                if (s[index] == '{')
                    counter++;
                else if (s[index] == '}')
                    counter--;
                if (counter == 0)
                    break;
                index++;
            }
            index++;
            if (index < s.Length)
            {
                substr = s.Substring(i, index - i);

                int offset = i;
                if (!String.IsNullOrEmpty(substr))
                {
                    isFound = false;
                    r = new Regex(@"(\btitle\s*:\s*\{)");
                    MatchCollection matches = r.Matches(substr);
                    foreach (Match m in matches)
                    {
                        if (CheckLevel(m, substr, 1))
                        {
                            i = m.Index;
                            l = m.Length;
                            index = i + l;
                            counter = 1;
                            while (index < substr.Length)
                            {
                                if (substr[index] == '{')
                                    counter++;
                                else if (substr[index] == '}')
                                    counter--;
                                if (counter == 0)
                                    break;
                                index++;
                            }
                            if (index != substr.Length)
                            {
                                s = s.Remove(offset + i + l, index - i - l);
                                s = s.Insert(offset + i + l, "\n" +
                                    "                text: $('#YAxisTile').prop('textContent')" + "\n" + new String(' ', 12));
                                isFound = true;
                                break;
                            }
                        }
                    }
                    if (!isFound)
                    {
                        index = offset + l + 1;
                        if (index < s.Length)
                        {
                            s = s.Insert(index,
                                "            title: {" + "\n" +
                                "                text: $('#YAxisTile').prop('textContent')" + "\n" +
                                "            }," + "\n");
                        }
                    }
                }
            }
        }
        else
        {
            r = new Regex(@"(\bsubtitle\s*:\s*\{)");
            match = r.Match(s);
            if (match.Success)
            {
                int i = match.Index;
                int l = match.Length;
                int index = i + l;
                int counter = 1;
                while (index < s.Length)
                {
                    if (s[index] == '{')
                        counter++;
                    else if (s[index] == '}')
                        counter--;
                    if (counter == 0)
                        break;
                    index++;
                }
                index += 3;
                if (index < s.Length)
                {
                    s = s.Insert(index,
                        "        yAxis: {" + "\n" +
                        "            title: {" + "\n" +
                        "                text: $('#YAxisTile').prop('textContent')" + "\n" +
                        "            }," + "\n" +
                        "        }," + "\n");
                }
            }
        }

        return s;
    }

    private string ReplaceDataSource(string s)
    {
        Regex r = new Regex(@"(\bseries\s*:\s*[\{\[])");
        MatchCollection matches = r.Matches(s);
        int insertAt = 0;
        bool isNew = false;
        bool isFound = false;
        foreach (Match m in matches)
        {
            if (CheckLevel(m, s, 2))
            {
                int i = m.Index;
                int l = m.Length;
                int index = i + l;
                int counter = 1;
                char openChar = s[index - 1];
                char closeChar = '}';
                switch (openChar)
                {
                    case '[':
                        closeChar = ']';
                        break;
                    case '{':
                        closeChar = '}';
                        break;
                }
                while (index < s.Length)
                {
                    if (s[index] == openChar)
                        counter++;
                    else if (s[index] == closeChar)
                        counter--;
                    if (counter == 0)
                        break;
                    index++;
                }
                if (index != s.Length)
                {
                    s = s.Remove(i, index - i + 1);
                    insertAt = i;
                    isFound = true;
                }
            }
        }
        
        if (!isFound)
        {
            r = new Regex(@"(\bdata\s*:\s*\{)");
            Match match = r.Match(s);
            if (!match.Success)
            {
                isNew = true;
                r = new Regex(@"(\byAxis\s*:\s*\{)");
                match = r.Match(s);
                if (match.Success)
                {
                    int i = match.Index;
                    int l = match.Length;
                    int index = i + l;
                    int counter = 1;
                    while (index < s.Length)
                    {
                        if (s[index] == '{')
                            counter++;
                        else if (s[index] == '}')
                            counter--;
                        if (counter == 0)
                            break;
                        index++;
                    }
                    int n = 1;
                    if ((index + n != s.Length) && (s[index + n] == ','))
                    {
                        n = 2;
                        if ((index + n != s.Length) && (s[index + n] == '\n'))
                            n = 3;
                    }
                    if (index != s.Length)
                    {
                        insertAt = index + n;
                    }
                }
            }
        }

        if (insertAt > 0)
        {
            s = s.Insert(insertAt,
                (isNew ? new String(' ', 8) : "") +
                "data: {" + "\n" +
                "             csv: $('#CSVData').prop('textContent')" + "\n" +
                "        }," +
                (isNew ? "\n" : ""));
        }
        
        return s;
    }

    private bool CheckLevel(Match match, string s, int level)
    {
        int index = 0;
        int counter = 0;
        bool start = false;
        while (index < match.Index)
        {
            if (s[index] == '{')
            {
                start = true;
                counter++;
            }
            else if (s[index] == '}')
                counter--;
            if (start && (counter == 0))
                break;
            index++;
        }

        return (counter == level);
    }
}