<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="TableSection.aspx.cs" Inherits="DocGen.Document.STTableSection.Edit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
    <script type="text/javascript">
        function SavedAndRefresh() {
            window.parent.document.getElementById('btnRefresh').click();
            parent.$.fancybox.close();

        }

        function CloseAndRefresh() {
            if (document.getElementById('hfRemoveSection').value == '0') {
                parent.$.fancybox.close();
            }
            else {
                //                window.parent.document.getElementById('btnRefresh').click();
                window.parent.RemoveNoAddedSection();
                parent.$.fancybox.close();
            }

        }
    
    </script>
    <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="Table Section"></asp:Label>
    <asp:HiddenField runat="server" ID="hfRemoveSection" ClientIDMode="Static" Value="0" />
    <br />
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification"
        ValidationGroup="MainValidationGroup" ShowSummary="false" ShowMessageBox="true"
        HeaderText="Please correct the following errors:" />
    <br />
    <div style="text-align: center;">
        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
            <ProgressTemplate>
                <div id="AJAXIndicator">
                    <asp:Image ID="Image2" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td align="right" style="width: 120px;">
                        <strong>Table Type</strong>
                    </td>
                    <td align="left">
                        <asp:DropDownList runat="server" ID="ddlTableType" AutoPostBack="true" CssClass="NormalTextBox"
                            OnSelectedIndexChanged="ddlTableType_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Table" Text="Table"></asp:ListItem>
                            <asp:ListItem Value="system" Text="System Table"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td align="right" style="width: 100px;">
                                    <strong></strong>
                                </td>
                                <td align="left" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <div runat="server" id="div1">
                                                    <asp:LinkButton runat="server" ID="lbTest" CausesValidation="false" OnClick="lbTest_Click">
                                                        <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/Refresh2.png"  ToolTip="Refresh Preview" />
                                                    </asp:LinkButton>
                                                </div>
                                            </td>
                                            <td>
                                                <div runat="server" id="div2">
                                                    <asp:LinkButton runat="server" ID="SaveButton" OnClick="SaveButton_Click" ValidationGroup="MainValidationGroup">
                                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                                    </asp:LinkButton>
                                                </div>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trSystemTable" visible="false">
                    <td align="right" style="width: 120px;">
                        <strong>System Table</strong>
                    </td>
                    <td align="left">
                        <asp:DropDownList runat="server" ID="ddlSystemTable" CssClass="NormalTextBox" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSystemTable_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="uploads" Text="Recent Uploads"></asp:ListItem>
                            <asp:ListItem Value="alerts" Text="Recent Alerts"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr runat="server" id="trTableTalbe">
                    <td align="right" style="width: 120px;">
                        <strong runat="server" id="stgTableCap">Table</strong>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSamleType" runat="server" AutoPostBack="True" CssClass="NormalTextBox"
                            DataTextField="TableName" DataValueField="TableID" OnSelectedIndexChanged="ddlSamleType_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSamleType"
                            CssClass="failureNotification" ErrorMessage="Table is required." ToolTip="Table is required."
                            ValidationGroup="MainValidationGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr runat="server" id="trUseReportDate">
                    <td>
                    </td>
                    <td align="left" colspan="2">
                        <asp:CheckBox runat="server" ID="chkUseReportDates" CssClass="NormalTextBox" AutoPostBack="true"
                            Checked="true" ClientIDMode="Static" OnCheckedChanged="chkUseReportDates_CheckedChanged" />
                        &nbsp; <strong runat="server" id="stgUseReportDate">Use report dates</strong>
                    </td>
                </tr>
                <tr runat="server" id="trRecentDays" visible="false">
                    <td align="right">
                        <strong>Days</strong>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="NormalTextBox" Width="50px" ID="txtRecentDays"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtRecentDays"
                            ErrorMessage="Numeric only!" MaximumValue="1000000" MinimumValue="0" Type="Double"
                            Display="Dynamic" />
                    </td>
                </tr>

                 <tr runat="server" id="tr1" >
                    <td align="right">
                        <strong>Max Rows:</strong>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="NormalTextBox" Width="50px" ID="txtMaxRows" Text="10"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtRecentDays"
                            ErrorMessage="Numeric only!" MaximumValue="1000000" MinimumValue="0" Type="Double"
                            Display="Dynamic" />
                    </td>
                </tr>
                <tr runat="server" id="trDateRecorded">
                    <td align="right">
                        <strong>Date Recorded:</strong>
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtDateFrom" Width="100px" CssClass="NormalTextBox"
                            ValidationGroup="MainValidationGroup" BorderStyle="Solid" BorderColor="#909090"
                            BorderWidth="1" />
                        <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                            Format="dd/MM/yyyy"  FirstDayOfWeek="Monday">
                        </ajaxToolkit:CalendarExtender>
                        <%--<ajaxToolkit:MaskedEditExtender ID="meeDateFrom" runat="server" TargetControlID="txtDateFrom"
                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="NormalTextBox"
                            OnInvalidCssClass="NormalTextBox" MaskType="Date" AcceptNegative="None" ErrorTooltipEnabled="True"
                            CultureName="en-GB" />--%>
                        <%--<ajaxToolkit:MaskedEditValidator ID="mevDateFrom" runat="server" ControlExtender="meeDateFrom"
                            ControlToValidate="txtDateFrom" InvalidValueMessage="Date is invalid" Display="Dynamic"
                            InvalidValueBlurredMessage="*" IsValidEmpty="true" ValidationGroup="MainValidationGroup" />--%>
                        <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                            ValidationGroup="MainValidationGroup" ErrorMessage="Invalid start date." Font-Bold="true"
                            Display="Dynamic" Type="Date" MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="tbwStartDate" TargetControlID="txtDateFrom"
                            WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                        To
                        <asp:TextBox runat="server" ID="txtDateTo" Width="100px" CssClass="NormalTextBox"
                            ValidationGroup="MainValidationGroup" BorderStyle="Solid" BorderColor="#909090"
                            BorderWidth="1" />
                        <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                            Format="dd/MM/yyyy"  FirstDayOfWeek="Monday">
                        </ajaxToolkit:CalendarExtender>
                        <%--<ajaxToolkit:MaskedEditExtender ID="meeDateTo" runat="server" TargetControlID="txtDateTo"
                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="NormalTextBox"
                            OnInvalidCssClass="NormalTextBox" MaskType="Date" AcceptNegative="None" ErrorTooltipEnabled="True"
                            CultureName="en-GB" />--%>
                        <%--<ajaxToolkit:MaskedEditValidator ID="Maskededitvalidator1" runat="server" ControlExtender="meeDateTo"
                            ControlToValidate="txtDateTo" InvalidValueMessage="Date is invalid" Display="Dynamic"
                            InvalidValueBlurredMessage="*" IsValidEmpty="true" ValidationGroup="MainValidationGroup" />--%>
                        <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MainValidationGroup"
                            ErrorMessage="Invalid end date" Font-Bold="true" Display="Dynamic" Type="Date"
                            MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtDateTo"
                            WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                        <asp:CompareValidator ID="EndDateCompare" runat="server" Type="Date" ControlToValidate="txtDateTo"
                            ControlToCompare="txtDateFrom" Operator="GreaterThanEqual" ToolTip="End date must be after the start date"
                            ErrorMessage="End date must be after the start date" CssClass="failureNotification"
                            ValidationGroup="MainValidationGroup">*</asp:CompareValidator>
                    </td>
                </tr>
                <%--<tr runat="server" id="trLocations">
                    <td align="right" valign="top">
                        <strong>Location(s)</strong>
                    </td>
                    <td colspan="2">
                        <asp:ListBox runat="server" ID="lstLocation" DataTextField="LocationName" DataValueField="LocationID"
                            Width="200px" Height="100px" SelectionMode="Multiple" CssClass="NormalTextBox">
                        </asp:ListBox>
                    </td>
                </tr>--%>
                <tr runat="server" id="trFilter">
                    <td align="right">
                        <strong>Filter:</strong>
                    </td>
                    <td align="left" colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlFilterYAxis" runat="server" AutoPostBack="true" CssClass="NormalTextBox"
                                        OnSelectedIndexChanged="ddlYAxis_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtLowerLimit" Width="50px" CssClass="NormalTextBox"
                                                    Visible="false"></asp:TextBox>
                                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtLowerLimit"
                                                    WatermarkText="Lower" runat="server">
                                                </ajaxToolkit:TextBoxWatermarkExtender>
                                                <asp:TextBox runat="server" ID="txtSearchText" Width="100px" CssClass="NormalTextBox"
                                                    Visible="false"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revLatitude" ControlToValidate="txtLowerLimit"
                                                    runat="server" ErrorMessage="Numeric value please!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblTo" Text="To" CssClass="NormalTextBox" Visible="false"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtUpperLimit" Width="50px" CssClass="NormalTextBox"
                                                    Visible="false"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtUpperLimit"
                                                    runat="server" ErrorMessage="Numeric value please!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                                </asp:RegularExpressionValidator>
                                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtUpperLimit"
                                                    WatermarkText="Upper" runat="server">
                                                </ajaxToolkit:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trSortOrder">
                    <td align="right">
                        <strong>Sort Order:</strong>
                    </td>
                    <td align="left" colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlOrderYAxis" runat="server" AutoPostBack="false" CssClass="NormalTextBox">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrderDirection" runat="server" AutoPostBack="false" CssClass="NormalTextBox">
                                        <asp:ListItem Selected="True" Value="ASC" Text="Ascending"></asp:ListItem>
                                        <asp:ListItem Value="DESC" Text="Descending"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trShowTime">
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:CheckBox runat="server" ID="chkShowTime" Text="Show time with date" TextAlign="Right" />
                    </td>
                </tr>
                <tr id="pColumns" runat="server" clientidmode="Static">
                    <td align="right">
                        <strong runat="server" id="stgFields">Fields</strong>
                    </td>
                    <td align="left" colspan="2">
                        <asp:GridView Style="margin-left: 10px; margin-top: 0px" ID="gvColumns" AutoGenerateColumns="false"
                            GridLines="None" runat="server" CssClass="gridview" OnRowCommand="gvColumns_RowCommand"
                            OnRowDataBound="gvColumns_RowDataBound">
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <Columns>
                                <asp:TemplateField HeaderText="Visible">
                                    <ItemStyle Width="40px" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkVisible" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbUp" CommandName="MoveUp" Visible="false" runat="server"><span class="ui-icon ui-icon-arrowthick-1-n"></span></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDown" CommandName="MoveDown" Visible="false" runat="server"><span class="ui-icon ui-icon-arrowthick-1-s"></span></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="SystemName" HeaderText="System Name" ItemStyle-Width="150px"  />--%>
                                <asp:TemplateField HeaderText="System Name" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSystemName" Text='<%# Eval("SystemName")%>' CssClass="textEntry"
                                            runat="server" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Display Name">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDisplayName" Text='<%# Eval("DisplayName")%>' CssClass="textEntry"
                                            runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bold" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBold" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Italic" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkItalic" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Underline" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkUnderline" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Alignment" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlAlignment" runat="server">
                                            <asp:ListItem Value="left">Left</asp:ListItem>
                                            <asp:ListItem Value="center">Center</asp:ListItem>
                                            <asp:ListItem Value="right">Right</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                    </td>
                    <td align="left" colspan="2" style="padding-top:10px;">
                        <asp:Literal ID="ltTest" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="chkUseReportDates" />
            <asp:AsyncPostBackTrigger ControlID="ddlSamleType" />
            <asp:AsyncPostBackTrigger ControlID="lbTest" />
            <asp:AsyncPostBackTrigger ControlID="ddlFilterYAxis" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
