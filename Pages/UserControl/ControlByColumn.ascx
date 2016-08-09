<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlByColumn.ascx.cs"
    Inherits="Pages_UserControl_ControlByColumn" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <table cellspacing="0" cellpadding="0">
        <tr>
            <td align="right" runat="server" id="tdFilterYAxis" style="padding-left: 5px;">
                <asp:Panel runat="server" ID="divYAxis">
                    <asp:DropDownList ID="ddlYAxis" runat="server" AutoPostBack="true" CssClass="NormalTextBox"
                        OnSelectedIndexChanged="ddlYAxis_SelectedIndexChanged">
                    </asp:DropDownList>
                </asp:Panel>
                <asp:HiddenField runat="server" ID="hfTextSearch" Value="" />
                <asp:HiddenField runat="server" ID="hfColumnTypeOut" Value="" />
            </td>
            <td runat="server" id="tdCompareOperator" visible="false">
                <asp:DropDownList runat="server" ID="ddlCompareOperator" CssClass="NormalTextBox" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlCompareOperator_SelectedIndexChanged"  Width="90px">
                         <%--   <asp:ListItem Text="=" Value="=" Selected="True"></asp:ListItem>
                            <asp:ListItem Text=">" Value=">"></asp:ListItem>
                            <asp:ListItem Text="<" Value="<"></asp:ListItem>
                            <asp:ListItem Text="Between" Value="between"></asp:ListItem> 
                            <asp:ListItem Text="Is Empty" Value="empty"></asp:ListItem>  
                            <asp:ListItem Text="Is Not Empty" Value="notempty"></asp:ListItem> 
                            <asp:ListItem Text="Not Equal" Value="<>"></asp:ListItem>       --%>
                 </asp:DropDownList>
            </td>
            <td style="padding-left:5px;">
                <table>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="txtLowerLimit" Width="90px" CssClass="NormalTextBox"
                                Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtLowerDate" Width="90px" CssClass="NormalTextBox"
                                Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtSearchText" Width="100px" CssClass="NormalTextBox"
                                Visible="false"></asp:TextBox>
                            <asp:DropDownList runat="server" ID="ddlDropdownColumnSearch" CssClass="NormalTextBox"
                                Visible="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                             

                            <asp:ImageButton Visible="false" runat="server" ID="ibLowerDate" ImageUrl="~/Images/Calendar.png"
                                AlternateText="Click to show calendar" CausesValidation="false" />

                              <asp:TextBox runat="server" ID="txtLowerTime" Width="50px" CssClass="NormalTextBox"
                                Visible="false" ToolTip="Time"></asp:TextBox>

                            <%--<ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtLowerLimit"
                                WatermarkText="Lower" runat="server">
                            </ajaxToolkit:TextBoxWatermarkExtender>
                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" TargetControlID="txtLowerDate"
                                WatermarkText="Lower" runat="server">
                            </ajaxToolkit:TextBoxWatermarkExtender>--%>
                            <ajaxToolkit:CalendarExtender ID="ce_txtLowerDate" runat="server" TargetControlID="txtLowerDate"
                                Format="dd/MM/yyyy" PopupButtonID="ibLowerDate" FirstDayOfWeek="Monday">
                            </ajaxToolkit:CalendarExtender>
                            <%--<asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtLowerDate"
                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                MinimumValue="1/1/1753" MaximumValue="01/01/3000"></asp:RangeValidator>--%>
                            <asp:RegularExpressionValidator ID="revLowerLimit" ControlToValidate="txtLowerLimit"
                                runat="server" ErrorMessage="Numeric value please!" Display="Dynamic"
                                ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                            </asp:RegularExpressionValidator>
                            <ajaxToolkit:MaskedEditExtender runat="server" ID="meeLowerTime" TargetControlID="txtLowerTime"
                               AutoCompleteValue="00:00" MaskType="Time"  Mask="99:99"  ></ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding-left:5px; padding-right:5px;">
                   <asp:Label runat="server" ID="lblTo" Text="To" CssClass="NormalTextBox" Visible="false"></asp:Label>
            </td>
            <td >
                <table>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="txtUpperLimit" Width="90px" CssClass="NormalTextBox"
                                Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtUpperDate" Width="90px" CssClass="NormalTextBox"
                                Visible="false"></asp:TextBox>
                        </td>
                        <td>
                            

                            <asp:ImageButton Visible="false" runat="server" ID="ibUpperDate" ImageUrl="~/Images/Calendar.png"
                                AlternateText="Click to show calendar" CausesValidation="false" />

                             <asp:TextBox runat="server" ID="txtUpperTime" Width="50px" CssClass="NormalTextBox"
                                Visible="false" ToolTip="Time"></asp:TextBox>

                            <%--<ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" TargetControlID="txtUpperDate"
                                WatermarkText="Upper" runat="server">
                            </ajaxToolkit:TextBoxWatermarkExtender>--%>
                            <ajaxToolkit:CalendarExtender ID="ce_txtUpperDate" runat="server" TargetControlID="txtUpperDate"
                                Format="dd/MM/yyyy" PopupButtonID="ibUpperDate" FirstDayOfWeek="Monday">
                            </ajaxToolkit:CalendarExtender>
                            <%--<asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtUpperDate"
                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>--%>
                            <asp:RegularExpressionValidator ID="revUpperLimit" ControlToValidate="txtUpperLimit"
                                 runat="server" ErrorMessage="Numeric value please!" Display="Dynamic"
                                ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                            </asp:RegularExpressionValidator>
                            <%--<ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtUpperLimit"
                                WatermarkText="Upper" runat="server">
                            </ajaxToolkit:TextBoxWatermarkExtender>--%>
                             <ajaxToolkit:MaskedEditExtender runat="server" ID="meeUpperTime" TargetControlID="txtUpperTime"
                               AutoCompleteValue="00:00" MaskType="Time"  Mask="99:99"  ></ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                </table>
               


            </td>
            <%--<td>
                <div style="width: 50px;">
                    <asp:UpdateProgress  ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <table style="width: 50px; text-align: center">
                                <tr>
                                    <td>
                                        <img alt="Processing..." src="../../Images/ajax.gif" />
                                    </td>
                                </tr>
                            </table>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </td>--%>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
