<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordList.ascx.cs" Inherits="Pages_UserControl_RecordList" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/ControlByColumn.ascx" TagName="ControlByColumn"
    TagPrefix="dbg" %>
<%@ Register Src="~/Pages/UserControl/ViewDetail.ascx" TagName="OneView" TagPrefix="dbg" %>
<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>
<asp:Literal ID="ltScriptHere" runat="server"></asp:Literal>
<asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>
<style type="text/css">
    .headerlink a {
        text-decoration: none;
    }
</style>


<%--<script type="text/javascript">
    $(document).ready(function () {

        function OnScrollDiv(Scrollablediv) {
            //DivPagerRow
            document.getElementById('DivPagerRow').scrollLeft = Scrollablediv.scrollLeft;
            document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
            document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
        }

        function autoResizeMe(id, w) {
            //var newheight;
            var newwidth;

            if (document.getElementById) {
                //newheight = document.getElementById(id).contentWindow.document.body.scrollHeight;
                // var newwidthS = document.getElementById(id).contentWindow.document.body.scrollWidth;
                newwidth = $(window).width() - 100;
                // document.getElementById(id).height = (newheight) + "px";
                //alert(newwidth);
                if (parseInt(w) == 2) {
                    document.getElementById(id).width = (newwidth / parseInt(w)) + "px";
                }
                else {

                    document.getElementById(id).width = (newwidth) + "px";

                }

            }

        }

    });
</script>--%>

<%--<asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server">
    <ProgressTemplate>
        <table style="width: 100%; height: 100%; text-align: center;">
            <tr valign="middle">
                <td>
                    <p style="font-weight: bold;">
                        Please wait...
                    </p>
                    <asp:Image runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                </td>
            </tr>
        </table>
    </ProgressTemplate>
</asp:UpdateProgress>--%>


<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
        <div runat="server" id="divRecordListTop">
            <table style="width: 100%; border-collapse: collapse; border-spacing: 0;">
                <tr>
                    <td colspan="3" align="left">
                        <table runat="server" id="tblTopCaption" width="100%">
                            <%-- 1000px--%>
                            <tr runat="server" id="trRecordListTitle">
                                <td align="left" style="width: 50%;" runat="server" id="tdTopTitile">
                                    <span class="TopTitle">
                                        <asp:Label ID="lblTitle" runat="server" Text="Records" Visible="false"> </asp:Label>
                                        <%--<asp:Label ID="lblRecords" runat="server" Text="Records:" Visible="true"> </asp:Label>--%>
                                        <asp:HiddenField runat="server" ID="hfViewID" />
                                        <asp:HiddenField runat="server" ID="hfHidePagerGoButton" />

                                    </span>
                                    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlTableMenu" CssClass="TopTitle"
                                        DataValueField="TableID" DataTextField="TableName" OnSelectedIndexChanged="ddlTableMenu_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 50px;"></td>
                                <td runat="server" id="tdTopButtons">
                                    <table style="width: 100%; text-align: right; padding-right: 42px;" class="ListConfigControl">
                                        <tr>

                                            <td></td>
                                            <td style="width: 50px;">
                                                <div runat="server" id="divShowGraph" visible="true">
                                                    <asp:HyperLink runat="server" ID="hlShowGraph" CssClass="ButtonLink">
                                            <asp:Image runat="server" ID="imgShowGraph" ImageUrl="~/App_Themes/Default/images/Graph.png"
                                                ToolTip="Graph" />
                                                    </asp:HyperLink>
                                                </div>
                                            </td>
                                            <td style="width: 50px;">
                                                <div runat="server" id="divUpload" visible="true">
                                                    <asp:HyperLink runat="server" ID="hlUpload" CssClass="ButtonLink">
                                            <asp:Image runat="server" ID="imgUpload" ImageUrl="~/App_Themes/Default/images/Upload.png"
                                                ToolTip="Upload" />
                                                    </asp:HyperLink>
                                                </div>
                                            </td>
                                            <td style="width: 50px;" runat="server" id="divEmail">
                                                <div>
                                                    <asp:ImageButton ID="ibEmail" runat="server" ImageUrl="~/App_Themes/Default/images/email.png"
                                                        ToolTip="Email" OnClick="ibEmail_Click" />
                                                </div>
                                            </td>
                                            <td style="width: 50px;">
                                                <div runat="server" id="divConfig" visible="false">
                                                    <asp:HyperLink runat="server" ID="hlConfig" CssClass="ButtonLink">
                                            <asp:Image runat="server" ID="imgConfig" ImageUrl="~/App_Themes/Default/images/Config.png"
                                                ToolTip="Configure" />
                                                    </asp:HyperLink>
                                                </div>
                                            </td>
                                            <td runat="server" id="divBatches" visible="false">
                                                <div>
                                                    <asp:HyperLink runat="server" ID="hlBatches" CssClass="btn"> <strong>My Batches</strong> </asp:HyperLink>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px;">
            <table border="0" cellpadding="0" cellspacing="0" align="left" style="width: 100%;">
                <%-- 1000px--%>
                <tr>
                    <td colspan="3" height="13"></td>
                </tr>
                <tr>
                    <td valign="top"></td>
                    <td valign="top">
                        
                                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="lnkSearch" Style="width: 100%">
                                    <div runat="server" id="divSearch" onclick="abc();" class="searchcorner" style="width: 100%;">
                                        <div style="max-width: 1200px;">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr runat="server" id="trFiletrTop">
                                                    <td>
                                                        <table runat="server" id="tblFilterByColumn" visible="false">
                                                            <tr runat="server" id="trSummaryFilter">
                                                                <td align="right">
                                                                    <asp:Label runat="server" ID="lblFilterColumnName" Font-Bold="true"></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hfFilterColumnSystemName" />
                                                                </td>
                                                                <td>
                                                                    <%--<asp:DropDownList runat="server" ID="ddlFilterValue" AutoPostBack="true" CssClass="NormalTextBox"
                                                                OnSelectedIndexChanged="ddlFilterValue_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>
                                                                    <%--<dbg:cbcValue runat="server" ID="cbcvSumFilter" OnddlYAxis_Changed="cbcvSumFilter_OnddlYAxis_Changed" />--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <%--<td align="right" runat="server" id="tdFilterCaption">
                                                <strong>Filter:</strong>
                                            </td>--%>
                                                    <td align="left">
                                                        <%--<asp:DropDownList ID="ddlYAxis" runat="server" AutoPostBack="true"
                                                 CssClass="NormalTextBox" OnSelectedIndexChanged="ddlYAxis_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                        <div runat="server" id="tdFilterYAxis">

                                                            <table>
                                                                <tr>
                                                                    <td></td>
                                                                    <td align="right">
                                                                        <strong runat="server" id="stgFilter">Filter:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <td>
                                                                                <dbg:ControlByColumn runat="server" ID="cbcSearchMain" OnddlYAxis_Changed="cbcSearchMain_OnddlYAxis_Changed"
                                                                                    OnddlCompareOperator_Changed="cbcSearchMain_OnddlCompareOperator_Changed" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:LinkButton runat="server" ID="lnkAddSearch1" Visible="false" OnClick="lnkSearch_Click">
                                                                        <asp:Image  runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png"/>
                                                                                </asp:LinkButton>
                                                                            </td>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trSearch1" style="display: none;">
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="lnkMinusSearch1">
                                                                <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                        </asp:LinkButton>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:HiddenField runat="server" ID="hfAndOr1" Value="" />
                                                                        <asp:LinkButton runat="server" ID="lnkAndOr1" Text="and"></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <td>
                                                                                <dbg:ControlByColumn runat="server" ID="cbcSearch1"
                                                                                    OnddlYAxis_Changed="cbcSearch1_OnddlYAxis_Changed"
                                                                                    OnddlCompareOperator_Changed="cbcSearch1_OnddlCompareOperator_Changed" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:LinkButton runat="server" ID="lnkAddSearch2" Visible="false" OnClick="lnkSearch_Click">
                                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                </asp:LinkButton>
                                                                            </td>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trSearch2" style="display: none;">
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="lnkMinusSearch2">
                                                                <asp:Image ID="Image5" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                        </asp:LinkButton>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:HiddenField runat="server" ID="hfAndOr2" Value="" />
                                                                        <asp:LinkButton runat="server" ID="lnkAndOr2">and</asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <td>
                                                                                <dbg:ControlByColumn runat="server" ID="cbcSearch2" OnddlYAxis_Changed="cbcSearch2_OnddlYAxis_Changed" OnddlCompareOperator_Changed="cbcSearch2_OnddlCompareOperator_Changed" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:LinkButton runat="server" ID="lnkAddSearch3" Visible="false" OnClick="lnkSearch_Click">
                                                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                </asp:LinkButton>
                                                                            </td>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trSearch3" style="display: none;">
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="lnkMinusSearch3">
                                                                <asp:Image ID="Image6" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                        </asp:LinkButton>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:HiddenField runat="server" ID="hfAndOr3" Value="" />
                                                                        <asp:LinkButton runat="server" ID="lnkAndOr3">and</asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <dbg:ControlByColumn runat="server" ID="cbcSearch3" OnddlCompareOperator_Changed="cbcSearch3_OnddlCompareOperator_Changed" />
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField runat="server" ID="hfTextSearch" Value="" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="tdFilterDynamic">
                                                            <table runat="server" id="tblSearchControls" visible="true" cellpadding="3">
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <td style="float: right;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table width="120px">
                                                                        <tr>
                                                                            <td colspan="2"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong></asp:LinkButton>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div>
                                                                                    <asp:LinkButton runat="server" ID="lnkReset" CssClass="btn" OnClick="lnkReset_Click"
                                                                                        CausesValidation="false"> <strong>Reset</strong></asp:LinkButton>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2"></td>
                                                                        </tr>
                                                                    </table>
                                                                    <%--<asp:HyperLink class="popuplink" runat="server" 
                                                        ID="hlAdvancedSearch" Text="Advanced..." NavigateUrl="~/Pages/Record/AdvancedFilter.aspx"></asp:HyperLink>--%>
                                                                </td>
                                                                <td>
                                                                    <table id="tblAdvancedOptionChk" runat="server">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <table>

                                                                                    <%-- Advanced Search Checkbox --%>
                                                                                    <tr>
                                                                                        <td style="text-align: right;">
                                                                                            <asp:CheckBox runat="server" ID="chkShowAdvancedOptions" AutoPostBack="true" OnCheckedChanged="chkShowAdvancedOptions_OnCheckedChanged" />

                                                                                        </td>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:Label runat="server" ID="lblAdvancedCaption" Text="Advanced Search"></asp:Label>
                                                                                            <asp:HiddenField runat="server" ID="hfHideAdvancedOption" />

                                                                                        </td>

                                                                                    </tr>

                                                                                    <%-- Show Deleted Records Checkbox--%>
                                                                                    <tr>
                                                                                        <td style="text-align: right;">
                                                                                            <asp:CheckBox ID="chkShowDeletedRecords" Checked="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkIsActive_CheckedChanged" />
                                                                                        </td>
                                                                                        <td style="text-align: left;">Show Deleted Records
                                                                                        </td>
                                                                                    </tr>

                                                                                    <%-- Show Warning Only checkbox --%>
                                                                                    <tr id="trChkShowOnlyWarning" runat="server">
                                                                                        <td style="text-align: right;">
                                                                                            <asp:CheckBox ID="chkShowOnlyWarning" Checked="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowOnlyWarning_CheckedChanged" /></td>
                                                                                        <td style="text-align: left;">Show Only Warning</td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                </tr>
                                                <tr runat="server" id="trFilterBottom">
                                                    <td colspan="4">
                                                        <table id="tblAdvancedOptionChkC" runat="server" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <div id="search" style="padding-bottom: 10px;" runat="server" visible="true">
                                                                        <table runat="server" id="tblAdvancedOption" style="border-collapse: collapse; display: none;"
                                                                            cellpadding="4">
                                                                            <tr id="trRecordGroup" runat="server" visible="false">
                                                                                <td colspan="3"></td>
                                                                                <td align="right">
                                                                                    <strong>Record Group</strong>
                                                                                </td>
                                                                                <td colspan="2"></td>
                                                                            </tr>
                                                                            <tr id="Tr1" runat="server" visible="false">
                                                                                <td colspan="3"></td>
                                                                                <td align="right">
                                                                                    <strong>Table</strong>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                                                        DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                                                        CssClass="NormalTextBox">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3"></td>
                                                                                <td align="right"></td>
                                                                                <td>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table>
                                                                                                    <tr style="vertical-align: bottom;">

                                                                                                        <td>
                                                                                                            <strong>Date Added:</strong>
                                                                                                            <br />
                                                                                                            <asp:TextBox runat="server" ID="txtDateFrom" Width="100px" CssClass="NormalTextBox"
                                                                                                                BorderWidth="1" BorderStyle="Solid" BorderColor="#909090" />

                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 7px;">
                                                                                                            <asp:ImageButton runat="server" ID="imgDateForm" ImageUrl="~/Images/Calendar.png"
                                                                                                                AlternateText="Click to show calendar" CausesValidation="false" />
                                                                                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                                                                                Format="dd/MM/yyyy" PopupButtonID="imgDateForm" FirstDayOfWeek="Monday">
                                                                                                            </ajaxToolkit:CalendarExtender>
                                                                                                            <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                                                                                ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                                                                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                                                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtDateFrom"
                                                                                                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                                                                                            </ajaxToolkit:TextBoxWatermarkExtender>

                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-left: 2px; padding-right: 2px;">To

                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox runat="server" ID="txtDateTo" Width="100px" CssClass="NormalTextBox"
                                                                                                                BorderWidth="1" BorderStyle="Solid" BorderColor="#909090" />

                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 7px;">
                                                                                                            <asp:ImageButton runat="server" ID="imgDateTo" ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar"
                                                                                                                CausesValidation="false" />
                                                                                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                                                                                                Format="dd/MM/yyyy" PopupButtonID="imgDateTo" FirstDayOfWeek="Monday">
                                                                                                            </ajaxToolkit:CalendarExtender>
                                                                                                            <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo"
                                                                                                                ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1753"
                                                                                                                MaximumValue="1/1/3000"></asp:RangeValidator>
                                                                                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" TargetControlID="txtDateTo"
                                                                                                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                                                                                            </ajaxToolkit:TextBoxWatermarkExtender>

                                                                                                        </td>
                                                                                                    </tr>

                                                                                                </table>





                                                                                            </td>
                                                                                            <td style="width: 10px;"></td>
                                                                                            <td>
                                                                                                <strong>Entered By:</strong>
                                                                                                <br />
                                                                                                <asp:Panel runat="server" ID="divEnteredBy">
                                                                                                    <asp:DropDownList ID="ddlEnteredBy" runat="server" AutoPostBack="true" DataTextField="FullName" Style="max-width: 200px;"
                                                                                                        DataValueField="UserID" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlEnteredBy_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </asp:Panel>
                                                                                            </td>

                                                                                            <td style="width: 10px;"></td>
                                                                                            <td>
                                                                                                <strong>Upload Batch:</strong>
                                                                                                <br />
                                                                                                <asp:Panel runat="server" ID="divUploadedBatch">
                                                                                                    <asp:DropDownList ID="ddlUploadedBatch" runat="server" AutoPostBack="true" DataTextField="BatchDescription" Style="max-width: 200px;"
                                                                                                        DataValueField="BatchID" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlUploadedBatch_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </asp:Panel>
                                                                                            </td>


                                                                                            <td style="width: 10px;"></td>
                                                                                            <td>

                                                                                                <%-- Moved checboxes here --%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>



                                                                        </table>
                                                                    </div>
                                                                </td>
                                                                <td style="padding-left: 10px">
                                                                    <table width="120px">
                                                                        <tr>
                                                                            <%--<td>
                                                                        <asp:LinkButton runat="server" ID="lnkSearch2" CssClass="btn" OnClick="lnkSearch_Click"
                                                                            Style="display: none;" > <strong>Go</strong></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="lnkReset2" CssClass="btn" OnClick="lnkReset_Click"
                                                                            Style="display: none;" CausesValidation="false"> <strong>Reset</strong></asp:LinkButton>
                                                                    </td>--%>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </asp:Panel>
                         
                        <%-- <asp:Panel ID="gridPanel" runat="server" Style="max-height: 600px" Width="100%" ScrollBars="Auto">--%>
                        <%--<div style="overflow-y: auto; overflow-x: hidden;max-height:600px;">--%>
                        <%--   <div style="overflow: scroll;" onscroll="OnScrollDiv(this)" id="DivMainContent">--%>

                       

                                <div id="DivRoot">
                                    <div id="DivPagerRow">
                                    </div>
                                    <div style="overflow: hidden;" id="DivHeaderRow">
                                    </div>
                                    <div id="DivMainContent">
                                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                            AllowSorting="True" DataKeyNames="<%# GetDataKeyNames() %>"
                                            Width="100%" AutoGenerateColumns="true" PageSize="15" OnSorting="gvTheGrid_Sorting"
                                            OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound" ShowFooter="true"
                                            OnDataBound="gvTheGrid_DataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                                            <PagerSettings Position="Top" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="20px" />
                                                    <HeaderTemplate>
                                                        <%--<input id="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                type="checkbox" />--%>
                                                        <asp:CheckBox ID="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDelete" runat="server" onclick="Check_Click(this)" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("DBGSystemRecordID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="30px" />

                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Width="30px"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                                        <%--NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("DBGSystemRecordID").ToString()) %>'--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="DBGSystemRecordID" Visible="false">
                                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="30px" />
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Width="30px"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="viewHyperLink" runat="server" ToolTip="View" ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                                        <%--NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("DBGSystemRecordID").ToString())  %>'--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="gridview_header" />
                                            <%--gridview_header/HeaderFreez/FixedHeader--%>
                                            <RowStyle CssClass="gridview_row" />
                                            <AlternatingRowStyle CssClass="gridview_row" />
                                            <FooterStyle HorizontalAlign="Right" Font-Bold="true" CssClass="gridview_footer" />

                                            <PagerTemplate>

                                                <asp:GridViewPager runat="server" ID="Pager" TableName="Recordlist" OnExportForCSV="Pager_OnExportForCSV"
                                                    OnExportForExcel="Pager_OnExportForExcel" HidePagerGoButton="false"
                                                    HideSendEmail="true" HideFilter="true"
                                                    OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                                    OnSendEmailAction="Pager_OnSendEmailAction" OnBindTheGridAgain="Pager_BindTheGridAgain" OnCopyRecordAction="Pager_CopyRecordAction"
                                                    HideExcelExport="true" HideExport="true" HideAllExport="false" HideRefresh="true" HideGo="true"
                                                    HideEditView="false" TableID="<%#TableID%>" />
                                            </PagerTemplate>
                                        </dbg:dbgGridView>
                                    </div>
                                    <div id="DivFooterRow" style="overflow: hidden">
                                    </div>
                                </div>


                                <%--</asp:Panel>--%>

                                <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                                    <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                    Add new record now.</strong> 
                                    </asp:HyperLink>
                                    &nbsp or  &nbsp   
                            <asp:HyperLink runat="server" ID="hlEditView2" Text="Edit View" Font-Bold="true" CssClass="popuplink2"> </asp:HyperLink>

                                </div>
                                <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                                    <table>
                                        <tr>
                                            <td>No records matched your search. &nbsp<asp:LinkButton runat="server" ID="lnkNoFilter"
                                                OnClick="Pager_OnApplyFilter" Font-Bold="true" Text="Clear Search"> </asp:LinkButton>

                                            </td>
                                            <td>
                                                <div runat="server" id="divAddAndViewControls">
                                                    <table style="width: 100%; border-collapse: collapse; border-spacing: 0;">
                                                        <tr>
                                                            <td>,
                                                            </td>
                                                            <td>
                                                                <asp:HyperLink runat="server" ID="hlEditView" Text="Edit View" Font-Bold="true" CssClass="popuplink2"> </asp:HyperLink>
                                                            </td>
                                                            <td>or
                                                            </td>
                                                            <td>
                                                                <asp:HyperLink runat="server" ID="hplNewDataFilter" Text="Add Record" Font-Bold="true"> </asp:HyperLink>
                                                            </td>
                                                            <td>
                                                                <asp:HyperLink runat="server" ID="hplNewDataFilter2">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/Pages/Pager/Images/add.png" />
                                                                </asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </table>




                                                </div>

                                            </td>
                                        </tr>
                                    </table>

                                </div>
                                <br />
                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>



                                <asp:Button runat="server" ID="lnkDeleteAllOK" Style="display: none;" CausesValidation="false"
                                    OnClick="lnkDeleteAllOK_Click"></asp:Button>

                                <asp:HiddenField runat="server" ID="hfParmanentDelete" Value="no" />
                                <asp:HiddenField runat="server" ID="hfchkUndo" Value="false" />
                                <asp:HiddenField runat="server" ID="hfchkDeleteParmanent" Value="false" />
                                <asp:HiddenField runat="server" ID="hfchkDelateAllEvery" Value="false" />
                                <asp:HiddenField runat="server" ID="hftxtDeleteReason" Value="no" />
                                <br />

                                <asp:Button runat="server" ID="lnkEditManyOK" Style="display: none;" CausesValidation="false"
                                    OnClick="lnkEditManyOK_Click"></asp:Button>
                                <asp:HiddenField runat="server" ID="hfddlYAxisBulk" />
                                <asp:HiddenField runat="server" ID="hfBulkValue" />
                                <asp:HiddenField runat="server" ID="hfchkUpdateEveryItem" />

                                <asp:Button runat="server" ID="lnkExportRecords" CausesValidation="false" Style="display: none;"
                                    OnClick="lnkExportRecords_Click"></asp:Button>

                                <asp:HiddenField runat="server" ID="hfUsingScrol" />
                                <asp:Button runat="server" ID="btnRefreshViewChange" CausesValidation="false" Style="display: none;"
                                    OnClick="btnRefreshViewChange_Click"></asp:Button>
                        

                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                        <%--<asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Width="1" Style="display: none;" CausesValidation="false" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <br />


    </ContentTemplate>
    <Triggers>
        <%--<asp:AsyncPostBackTrigger ControlID="gvTheGrid" />--%>
        <asp:PostBackTrigger ControlID="lnkExportRecords" />
    </Triggers>
</asp:UpdatePanel>





