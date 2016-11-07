<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DBGGraphControl.ascx.cs"
    Inherits="DBGGraphControl" %>
<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%--<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>--%>
<%--<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/ColorPicker.ascx" TagName="CP" TagPrefix="asp" %>

<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>

<script src="<%=ResolveUrl("~/highcharts/highcharts.js")%>"></script>
<script src="<%=ResolveUrl("~/highcharts/highcharts-more.js")%>"></script>
<script src="<%=ResolveUrl("~/highcharts/modules/data.js")%>"></script>
<script src="<%=ResolveUrl("~/highcharts/modules/exporting.js")%>"></script>
<script src="<%=ResolveUrl("~/highcharts/modules/no-data-to-display.js")%>"></script>

<style type="text/css">
    .highchartData {
        display: none;
        visibility: hidden;
    }

    .disabledLink {
        cursor: default;
        color: gray;
    }
</style>

<script type="text/javascript">
    function ChangeTimePeriod(dtvalue) {

        document.getElementById("hfDateTimeValue").value = dtvalue;

        if (dtvalue != '') {
            document.getElementById('btnRefreshChart').click();
        }
    }

    function CheckFromTime(sender, args) {
        var compare = RegExp("^([0-1]?[0-9]|2[0-3]):([0-5][0-9])(:[0-5][0-9])?$");
        args.IsValid = compare.test(args.Value);
        return;
    }

    function CheckToTime(sender, args) {
        var compare = RegExp("^([0-1]?[0-9]|2[0-3]):([0-5][0-9])(:[0-5][0-9])?$");
        args.IsValid = compare.test(args.Value);
        return;
    }

    function AddNewGraphOptionDetail() {

        //document.getElementById('btnAddNewDetail').click();
        $("#hlAddNewDetail").trigger('click');
    }

    function FolderOpenClick() {

        $('#hlFolderOpen').trigger('click');
    }

    function OnWordWrap_CheckedChanged() {
        if ($('#chkWordWrap').is(':checked')) {
            $('textarea').css('white-space', '').css('word-wrap', '').css('overflow-x', '');
        }
        else {
            $('textarea').css('white-space', 'pre').css('word-wrap', 'normal').css('overflow-x', 'scroll');
        }
    }

    function OnlbHCSEdit_OK_click() {
        if ($('[id*=editableHCS]').val().length === 0) {
            alert("Script is required");
            return false;
        }
        $('[id*=editableHCS]').val(
            $('[id*=editableHCS]').val().
            replace(new RegExp('<', 'g'), '&lt;').
            replace(new RegExp('>', 'g'), '&gt;'));
        $.fancybox.close();
        return true;
    }

    var oldScript;

    function OnlnkHcscript_click() {
        oldScript = $('[id*=editableHCS]').val();
        $('[id*=editableHCS]').val(
            $('[id*=editableHCS]').val().
            replace(new RegExp('&lt;', 'g'), '<').
            replace(new RegExp('&gt;', 'g'), '>'));
    }

    function OnlbHCSEdit_Cancel_click() {
        $('[id*=editableHCS]').val(oldScript);
        $.fancybox.close();
    }

    function SendGraphEmail() {
        document.getElementById("hfGraphImageURL").value = '';
        var chart = $('#WebChartViewer1').highcharts();
        var imageURL = '';
        var svg = chart.getSVG();
        var txtGraphTitle = document.getElementById("ctl00_HomeContentPlaceHolder_gcTest_txtGraphTitle_Simple");
        var dataString = 'type=image/png&filename=' + encodeURIComponent(txtGraphTitle.value) + '&width=500&svg=' + svg;
        $.ajax({
            type: 'POST',
            data: dataString,
            url: 'ExportGraph.ashx',
            async: false,
            success: function (data) {
                document.getElementById("hfGraphImageURL").value = data;
                document.getElementById('btnSendEmail').click();
            }
        });
    }
</script>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
    <ProgressTemplate>
        <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 100%; height: 100%; background: #999; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
            <asp:Image Style="top: 48%; left: 42%; position: relative;" runat="server" ID="imgAjax"
                ImageUrl="~/Images/ajax.gif" AlternateText="Processing..." />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server">
            <table>
                <tr>
                    <td>
                        <div runat="server" if="divGraph">
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <%--<chart:WebChartViewer ID="WebChartViewer1" runat="server" />--%>
                                                    <div id="WebChartViewer1" clientidmode="Static" runat="server" style="display: block;"></div>

                                                    <asp:HiddenField ID="chartHeight" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="chartWidth" ClientIDMode="Static" runat="server" />

                                                    <asp:Label ID="CSVData" ClientIDMode="Static" runat="server" CssClass="highchartData" />

                                                    <asp:Label ID="chartTitle" ClientIDMode="Static" runat="server" CssClass="highchartData" />
                                                    <asp:Label ID="chartSubtitle" ClientIDMode="Static" runat="server" CssClass="highchartData" />
                                                    <asp:Label ID="YAxisTile" ClientIDMode="Static" runat="server" CssClass="highchartData" />

                                                    <asp:HiddenField ID="YAxisMin" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="YAxisMax" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="YAxisInterval" ClientIDMode="Static" runat="server" />

                                                    <asp:HiddenField ID="showWarnings" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="warningHigh" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="warningHighColor" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="warningHighCaption" ClientIDMode="Static" runat="server" />


                                                    <%--  <asp:HiddenField ID="showExceedances" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="ExceedanceHigh" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="ExceedanceHighColor" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="ExceedanceHighCaption" ClientIDMode="Static" runat="server" />--%>


                                                    <asp:HiddenField ID="showErrors" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="errorHigh" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="errorHighColor" ClientIDMode="Static" runat="server" />
                                                    <asp:HiddenField ID="errorHighCaption" ClientIDMode="Static" runat="server" />
                                                </td>
                                                <td valign="top">
                                                    <asp:LinkButton runat="server" ID="lnkZoom" OnClick="lnkZoom_Click" CausesValidation="false">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/zoom.png" />
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="lnkCloseZoom" OnClick="lnkCloseZoom_Click" Visible="false"
                                                        CausesValidation="false">
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/zoomclose.png" />
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                </tr>

                                <tr runat="server" id="tdClickHelp" visible="false">
                                    <td colspan="4" align="center">Click on a data point to drill-down into that date
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                            <asp:DropDownList runat="server" ID="ddlGraphOption" AutoPostBack="true" DataTextField="TableName" Visible="false"
                                Width="180px" DataValueField="TableID" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlGrpahOption_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:HiddenField runat="server" ID="hfDateTimeValue" ClientIDMode="Static" Value="" />
                            <asp:Button runat="server" ID="btnRefreshChart" ClientIDMode="Static" Style="display: none;"
                                OnClick="btnRefreshChart_Click" CausesValidation="false" />
                            <asp:Button runat="server" ID="btnRefreshChartPop" ClientIDMode="Static" Style="display: none;"
                                OnClick="btnRefreshChartPop_Click" />
                            <asp:HiddenField runat="server" ID="hfDetailSearchID" ClientIDMode="Static" Value="" />
                            <asp:HyperLink ID="hlAddNewDetail" ClientIDMode="Static" runat="server" NavigateUrl="~/Pages/UserControl/GraphOptionDetail.aspx"
                                CssClass="popuplink" Style="display: none;"></asp:HyperLink>
                        </div>
                    </td>
                    <td valign="top" style="vertical-align: top;">
                        <div runat="server" id="divOptionControls">
                            <table cellpadding="3" cellspacing="3" border="0">
                                <tr runat="server" id="trMainTitle">
                                    <td colspan="5"></td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="left">
                                        <table style="margin-left: 180px;">
                                            <tr>
                                                <td>
                                                    <div runat="server" id="divBack">
                                                        <asp:HyperLink runat="server" ID="hlBack" CssClass="ButtonLink" CausesValidation="false">
                                                            <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"
                                                                ToolTip="Back" />
                                                        </asp:HyperLink>
                                                    </div>
                                                </td>

                                                <td>
                                                    <div runat="server" id="div1">
                                                        <asp:ImageButton runat="server" ID="lnkExcel" ImageUrl="~/App_Themes/Default/images/chart2xl.png"
                                                            OnClick="ibExcel_Click" ToolTip="Export to Excel" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divRefresh">

                                                        <asp:ImageButton runat="server" ID="lnkRefresh" ImageUrl="~/App_Themes/Default/images/Refresh2.png"
                                                            OnClick="lnkRefresh_Click" ToolTip="Refresh" CausesValidation="false" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divEmail">


                                                        <asp:ImageButton runat="server" ID="ibEmail" ImageUrl="~/App_Themes/Default/images/email.png"
                                                            OnClientClick="SendGraphEmail();return false;" ToolTip="Email" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divSave">

                                                        <asp:ImageButton runat="server" ID="lnkSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                            OnClick="lnkSave_Click" ToolTip="Save" CausesValidation="true" ValidationGroup="main" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divFolderOpen">
                                                        <asp:HyperLink runat="server" ID="hlFolderOpen" class="popupgraph" ClientIDMode="Static">
                                                            <asp:Image runat="server" ImageUrl="~/App_Themes/Default/images/FolderOpen.png" ToolTip="Open Graph" />
                                                        </asp:HyperLink>
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:MultiView runat="server" ID="divMain" ActiveViewIndex="0">
                                            <asp:View runat="server" ID="viewBasic">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Graph&nbsp;Title</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtGraphTitle_Simple" CssClass="NormalTextBox" Width="300px"
                                                                ValidationGroup="main"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtGraphTitle_Simple"
                                                                ErrorMessage="Graph Title is required" Display="Dynamic" ValidationGroup="main"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Graph&nbsp;Subtitle</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtGraphSubtitle_Simple" CssClass="NormalTextBox" Width="300px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Field</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlEachAnalyte_Simple" CssClass="NormalTextBox" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlEachAnalyte_Simple_SelectedIndexChanged" Width="160px">
                                                                <asp:ListItem Text="--None--" Value="-1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr runat="server" id="trSeries">
                                                        <td style="text-align: right; vertical-align: top;">
                                                            <strong>Series</strong>
                                                        </td>
                                                        <td>

                                                            <asp:CheckBoxList runat="server" ID="SampleSitesList" AutoPostBack="false"
                                                                Style="display: block; overflow: auto; min-width: 300px; max-width: 400px; height: 140px; border: solid 1px #909090;">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Graph&nbsp;Definition</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlGraphType_Simple" CssClass="NormalTextBox" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlGraphType_Simple_SelectedIndexChanged"
                                                                Width="160px">
                                                            </asp:DropDownList>
                                                            <a id="lnkHcscript" href="#hcscript" onclick="return OnlnkHcscript_click();">
                                                                <asp:ImageButton runat="server" ID="ibEdit"
                                                                    ImageUrl="~/App_Themes/Default/images/iconEdit.png"
                                                                    ToolTip="Edit" Height="15px" Style="position: relative; top: 3px" />
                                                            </a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Limits</strong>
                                                        </td>
                                                        <td valign="top" style="vertical-align: top;">
                                                            <asp:CheckBox runat="server" ID="chkShowLimits_Simple" Text="" TextAlign="Right" Font-Bold="true"
                                                                AutoPostBack="true" OnCheckedChanged="chkShowLimits_Simple_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trShowLimitsOnGraphOne_Simple" visible="true">
                                                        <td align="left" colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 35px;">&nbsp;</td>
                                                                    <td align="left">
                                                                        <table>
                                                                            <tr runat="server" id="trWarning_Simple" visible="false">
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtWarningCaption_Simple" Width="150px" Text="Warning"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtWarningValue_Simple" Width="55px"></asp:TextBox>
                                                                                    <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtWarningValue_Simple"
                                                                                        ErrorMessage="*" MaximumValue="1000000" MinimumValue="-1000000" Text="*" Type="Double"></asp:RangeValidator>
                                                                                </td>
                                                                                <td>
                                                                                    <select style="width: 110px" id="ddlWarningColor_Simple" class="NormalTextBox" runat="server">
                                                                                        <option style="background-color: Aqua;" value="Aqua">Aqua</option>
                                                                                        <option style="background-color: Black; color: White;" value="Black">Black</option>
                                                                                        <option style="background-color: Blue;" value="Blue" selected="selected">Blue</option>
                                                                                        <option style="background-color: Fuchsia;" value="Fuchsia">Fuchsia</option>
                                                                                        <option style="background-color: Gray;" value="Gray">Gray</option>
                                                                                        <option style="background-color: Green;" value="Green">Green</option>
                                                                                        <option style="background-color: Lime;" value="Lime">Lime</option>
                                                                                        <option style="background-color: Maroon;" value="Maroon">Maroon</option>
                                                                                        <option style="background-color: Navy; color: White;" value="Navy">Navy</option>
                                                                                        <option style="background-color: Olive;" value="Olive">Olive</option>
                                                                                        <option style="background-color: Orange;" value="Orange">Orange</option>
                                                                                        <option style="background-color: Purple;" value="Purple">Purple</option>
                                                                                        <option style="background-color: Red;" value="Red">Red</option>
                                                                                        <option style="background-color: Silver;" value="Silver">Silver</option>
                                                                                        <option style="background-color: Teal;" value="Teal">Teal</option>
                                                                                        <option style="background-color: Yellow;" value="Yellow">Yellow</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr runat="server" id="trExceedance_Simple" visible="false">
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtExceedanceCaption_Simple" Width="150px" Text="Exceedance"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtExceedanceValue_Simple" Width="55px"></asp:TextBox>
                                                                                    <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtExceedanceValue_Simple"
                                                                                        ErrorMessage="*" MaximumValue="1000000" MinimumValue="-1000000" Text="*" Type="Double"></asp:RangeValidator>
                                                                                </td>
                                                                                <td>
                                                                                    <select style="width: 110px" id="ddlExceedanceColor_Simple" class="NormalTextBox" runat="server">
                                                                                        <option style="background-color: Aqua;" value="Aqua">Aqua</option>
                                                                                        <option style="background-color: Black; color: White;" value="Black">Black</option>
                                                                                        <option style="background-color: Blue;" value="Blue">Blue</option>
                                                                                        <option style="background-color: Fuchsia;" value="Fuchsia">Fuchsia</option>
                                                                                        <option style="background-color: Gray;" value="Gray">Gray</option>
                                                                                        <option style="background-color: Green;" value="Green">Green</option>
                                                                                        <option style="background-color: Lime;" value="Lime">Lime</option>
                                                                                        <option style="background-color: Maroon;" value="Maroon">Maroon</option>
                                                                                        <option style="background-color: Navy; color: White;" value="Navy">Navy</option>
                                                                                        <option style="background-color: Olive;" value="Olive">Olive</option>
                                                                                        <option style="background-color: Orange;" value="Orange">Orange</option>
                                                                                        <option style="background-color: Purple;" value="Purple">Purple</option>
                                                                                        <option style="background-color: Red;" value="Red" selected="selected">Red</option>
                                                                                        <option style="background-color: Silver;" value="Silver">Silver</option>
                                                                                        <option style="background-color: Teal;" value="Teal">Teal</option>
                                                                                        <option style="background-color: Yellow;" value="Yellow">Yellow</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr valign="top">
                                                        <td align="right">
                                                            <asp:Label runat="server" ID="lblTimePeriodDisplay_Simple" Font-Bold="true" Text="X-Axis"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlTimePeriodDisplay_Simple" AutoPostBack="true" CssClass="NormalTextBox"
                                                                            OnSelectedIndexChanged="ddlTimePeriodDisplay_Simple_SelectedIndexChanged">
                                                                            <asp:ListItem Text="Date Range" Value="C" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Text="One Year" Value="Y"></asp:ListItem>
                                                                            <asp:ListItem Text="One Month" Value="M"></asp:ListItem>
                                                                            <asp:ListItem Text="One Week" Value="W"></asp:ListItem>
                                                                            <asp:ListItem Text="One Day" Value="D"></asp:ListItem>
                                                                            <asp:ListItem Text="One Hour" Value="H"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="padding-left: 10px;">
                                                                        <asp:LinkButton Visible="false" runat="server" ID="lnkTPPrev2_Simple" Text="Prev" OnClick="lnkTPPrev2_Click"
                                                                            CausesValidation="false"></asp:LinkButton>
                                                                        <asp:Label Visible="false" runat="server" ID="lblTPPrev2_Simple" CssClass="disabledLink" Text="Prev" />
                                                                    </td>
                                                                    <td style="padding-left: 10px;">
                                                                        <asp:LinkButton Visible="false" runat="server" ID="lnkTPNext2_Simple" Text="Next" OnClick="lnkTPNext2_Click"
                                                                            CausesValidation="false"></asp:LinkButton>
                                                                        <asp:Label Visible="false" runat="server" ID="lblTPNext2_Simple" CssClass="disabledLink" Text="Next" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trStartDate_Simple">
                                                        <td align="right">
                                                            <strong>From</strong>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtStartDate_Simple" runat="server" CssClass="NormalTextBox" ClientIDMode="Static"
                                                                Width="100px" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1">
                                                            </asp:TextBox>
                                                            <asp:ImageButton runat="server" ID="ibStartDate_Simple" ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false" />
                                                            <asp:TextBox runat="server" ID="txtFromTime_Simple" ClientIDMode="Static" Style="width: 80px"
                                                                CssClass="NormalTextBox"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trEndDate_Simple">
                                                        <td align="right" valign="top">
                                                            <strong>To</strong>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtEndDate_Simple" runat="server" CssClass="NormalTextBox" ClientIDMode="Static"
                                                                Width="100px" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1">
                                                            </asp:TextBox>
                                                            <asp:ImageButton runat="server" ID="ibEndDate_Simple" ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false" />
                                                            <asp:TextBox runat="server" ID="txtToTime_Simple" Style="width: 80px" ClientIDMode="Static"
                                                                CssClass="NormalTextBox"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <%-- Date From --%>
                                                            <%--                                                            <asp:RequiredFieldValidator ID="StartDateRequired_Simple" runat="server" ControlToValidate="txtStartDate_Simple"
                                                                CssClass="failureNotification" ErrorMessage="Start date is required." ToolTip="Start date is required.">*</asp:RequiredFieldValidator>--%>
                                                            <asp:RangeValidator ID="rngDateFrom_Simple" runat="server" ControlToValidate="txtStartDate_Simple"
                                                                CssClass="failureNotification" ErrorMessage="*" ToolTip="From date is not ok." Display="Dynamic"
                                                                Type="Date" MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                            <ajaxToolkit:TextBoxWatermarkExtender ID="tbwStartDate_Simple" TargetControlID="txtStartDate_Simple"
                                                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                                            </ajaxToolkit:TextBoxWatermarkExtender>
                                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom_Simple" runat="server" TargetControlID="txtStartDate_Simple"
                                                                Format="dd/MM/yyyy" PopupButtonID="ibStartDate_Simple" FirstDayOfWeek="Monday">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <%-- Time From --%>
                                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtFromTime_Simple"
                                                                AutoCompleteValue="00:00" Mask="99:99" AcceptAMPM="false" runat="server" MaskType="Time">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                            <asp:CustomValidator runat="server" ID="cvTest_Simple" ControlToValidate="txtFromTime_Simple"
                                                                ClientValidationFunction="CheckFromTime"
                                                                CssClass="failureNotification" ErrorMessage="hh:mm format (24 hrs) please!"></asp:CustomValidator>
                                                            <%-- Date To --%>
                                                            <%--                                                            <asp:RequiredFieldValidator ID="EndDateRequired_Simple" runat="server" ControlToValidate="txtEndDate_Simple"
                                                                CssClass="failureNotification" ErrorMessage="End date is required." ToolTip="End date is required.">*</asp:RequiredFieldValidator>--%>
                                                            <asp:RangeValidator ID="rngDateTo_Simple" runat="server" ControlToValidate="txtEndDate_Simple"
                                                                CssClass="failureNotification" ErrorMessage="*" ToolTip="To date is not ok." Display="Dynamic"
                                                                Type="Date" MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtEndDate_Simple"
                                                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                                            </ajaxToolkit:TextBoxWatermarkExtender>
                                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateTo_Simple" runat="server" TargetControlID="txtEndDate_Simple"
                                                                Format="dd/MM/yyyy" PopupButtonID="ibEndDate_Simple" FirstDayOfWeek="Monday">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <%-- Time To --%>
                                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtToTime_Simple"
                                                                AutoCompleteValue="00:00" Mask="99:99" AcceptAMPM="false" runat="server" MaskType="Time">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                            <asp:CustomValidator runat="server" ID="CustomValidator3" ControlToValidate="txtToTime_Simple"
                                                                ClientValidationFunction="CheckToTime"
                                                                CssClass="failureNotification" ErrorMessage="hh:mm format (24 hrs) please!"></asp:CustomValidator>
                                                            <%-- Compare Dates --%>
                                                            <asp:CompareValidator ID="EndDateCompare_Simple" runat="server" Type="Date" ControlToValidate="txtEndDate_Simple"
                                                                ControlToCompare="txtStartDate_Simple" Operator="GreaterThanEqual" ToolTip="End date must be after the start date"
                                                                ErrorMessage="End date must be after the start date" CssClass="failureNotification">*</asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="Label1" Font-Bold="true" Text="Y-Axis"></asp:Label>

                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right"></td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label runat="server" ID="Label2" Font-Bold="true" Text="Highest Value"></asp:Label></td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtYHighestValue" runat="server" CssClass="NormalTextBox"
                                                                            Width="70px" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label runat="server" ID="Label3" Font-Bold="true" Text="Lowest Value"></asp:Label></td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtYLowestValue" runat="server" CssClass="NormalTextBox"
                                                                            Width="70px" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label runat="server" ID="Label4" Font-Bold="true" Text="Interval"></asp:Label></td>
                                                                    <td align="right">
                                                                        <asp:TextBox ID="txtYInterval" runat="server" CssClass="NormalTextBox"
                                                                            Width="70px" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </asp:View>
                                            <asp:View runat="server" ID="viewAdvanced">
                                            </asp:View>
                                            <asp:View runat="server" ID="viewDashboard">
                                                <div style="text-align: center;">
                                                    <br />
                                                    <br />
                                                    <br />


                                                    <table>
                                                        <tr id="trSavedGraph" runat="server" visible="false">
                                                            <td>
                                                                <strong>Saved Graph:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblSavedGraphName"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 100px;">
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkDateRange" Font-Bold="true" Text="Date Range" TextAlign="Right" />
                                                            </td>
                                                            <td>
                                                                <br />
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox runat="server" CssClass="NormalTextBox" Width="50px" ID="txtRecentNumber"></asp:TextBox>
                                                                            <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtRecentNumber"
                                                                                ErrorMessage="Numeric only!" MaximumValue="1000000" MinimumValue="0" Type="Double"
                                                                                Display="Dynamic" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlRecentPeriod" CssClass="NormalTextBox">

                                                                                <asp:ListItem Text="Years" Value="Y" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="Months" Value="M"></asp:ListItem>
                                                                                <asp:ListItem Text="Weeks" Value="W"></asp:ListItem>
                                                                                <asp:ListItem Text="Days" Value="D"></asp:ListItem>
                                                                                <asp:ListItem Text="Hours" Value="H"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </div>
                                            </asp:View>
                                        </asp:MultiView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:HiddenField runat="server" ID="hfSelectedGraphs" ClientIDMode="Static" />
                        <asp:Button runat="server" ID="btnUpdateGraphs" ClientIDMode="Static" OnClick="btnUpdateGraphs_Click"
                            Style="display: none;" />
                        <asp:Button runat="server" ID="HiddenButtonRefresh" ClientIDMode="Static"
                            OnClick="HiddenButtonRefresh_Click"
                            Style="display: none;" />
                        <asp:HiddenField runat="server" ID="hfGraphImageURL" ClientIDMode="Static" />
                        <asp:Button runat="server" ID="btnSendEmail" ClientIDMode="Static" OnClick="btnSendEmail_Click"
                            Style="display: none;" />
                    </td>
                </tr>
            </table>

            <div style="display: none">
                <div id="hcscript"
                    style="width: 500px; height: 550px; text-align: center;">
                    <div style="position: absolute; top: 15px; left: 25px; font-size: 0.8em;" class="ContentMain">
                        <label for="chkWordWrap">Word Wrap</label>
                        <input id="chkWordWrap" type="checkbox" checked="checked" onchange="OnWordWrap_CheckedChanged();" />
                    </div>
                    <textarea runat="server" id="editableHCS"
                        style="position: absolute; top: 40px; left: 25px; height: 450px; width: 450px;">
                    </textarea>
                    <br />
                    <div style="position: absolute; bottom: 10px; right: 25px;">
                        <asp:LinkButton runat="server" ID="lbHCSEdit_OK" CssClass="btn"
                            OnClientClick="return OnlbHCSEdit_OK_click();"
                            OnClick="lbHCSEdit_OK_Click">
                            <strong>OK</strong>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lbHCSEdit_Cancel" CssClass="btn"
                            Style="margin-left: 0.5em;"
                            OnClientClick="OnlbHCSEdit_Cancel_click(); return false;">
                            <strong>Cancel</strong>
                        </asp:LinkButton>
                    </div>
                </div>
        </asp:Panel>

        <asp:HiddenField runat="server" ID="hfInputErrors" />

        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" ClientIDMode="Static" runat="server"
            BehaviorID="popup" TargetControlID="hfInputErrors" PopupControlID="pnlPopup"
            BackgroundCssClass="modalBackground" CancelControlID="btnModalNo" />

        <asp:Panel ID="pnlPopup" runat="server" Style="display: none">
            <div style="border-width: 5px; background-color: #ffffff; border-color: #000000; height: 100px; border-style: outset;">
                <div style="padding-top: 50px; padding: 20px;">
                    <asp:Label ID="lblModalMessage" runat="server" Text="There is a graph with same title. Do you want to overwrite it?." />
                </div>
                <div style="text-align: center; padding-left: 160px;">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" ID="btnModalOK" CssClass="btn" CausesValidation="false"
                                    OnClick="btnModalOK_Click"> <strong>Yes</strong></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="btnModalNo" CssClass="btn" CausesValidation="false"> <strong>No</strong></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lnkExcel" />
    </Triggers>
</asp:UpdatePanel>
