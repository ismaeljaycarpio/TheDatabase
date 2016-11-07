<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ControlByColumnValue.ascx.cs"
    Inherits="Pages_UserControl_ControlByColumnValue" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<div>
    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <table cellspacing="0" cellpadding="0">
        <tr>
            <td align="right" runat="server" id="tdFilterYAxis" style="padding-left:5px;">
             <asp:Panel runat="server" id="divYAxis">
                <asp:DropDownList ID="ddlYAxis" runat="server" AutoPostBack="true" CssClass="NormalTextBox"
                    OnSelectedIndexChanged="ddlYAxis_SelectedIndexChanged">
                </asp:DropDownList>
                </asp:Panel>
                
            </td>            
            <td style="padding-left:5px;">
                <asp:TextBox runat="server" ID="txtLowerLimit" Width="90px" CssClass="NormalTextBox"
                    Visible="false"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtLowerDate" Width="90px" CssClass="NormalTextBox"
                    Visible="false"></asp:TextBox>
                <asp:ImageButton Visible="false" runat="server" ID="ibLowerDate" ImageUrl="~/Images/Calendar.png"
                    AlternateText="Click to show calendar" CausesValidation="false" />
                <%--<ajaxtoolkit:textboxwatermarkextender id="TextBoxWatermarkExtender1" targetcontrolid="txtLowerLimit"
                    watermarktext="Lower" runat="server">
                                                            </ajaxtoolkit:textboxwatermarkextender>--%>
                <%--<ajaxtoolkit:textboxwatermarkextender id="TextBoxWatermarkExtender5" targetcontrolid="txtLowerDate"
                    watermarktext="Lower" runat="server">
                                                            </ajaxtoolkit:textboxwatermarkextender>--%>
                <ajaxtoolkit:calendarextender id="ce_txtLowerDate" runat="server" targetcontrolid="txtLowerDate"
                    format="dd/MM/yyyy" popupbuttonid="ibLowerDate" FirstDayOfWeek="Monday">
                                                            </ajaxtoolkit:calendarextender>
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtLowerDate"
                    ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                    MinimumValue="1/1/1753" MaximumValue="01/01/3000"></asp:RangeValidator>
                <asp:TextBox runat="server" ID="txtSearchText" Width="100px" CssClass="NormalTextBox"
                    Visible="false"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revLowerLimit" ControlToValidate="txtLowerLimit"  ValidationGroup="MKE"
                    runat="server" ErrorMessage="Numeric value please!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                </asp:RegularExpressionValidator>
                <asp:DropDownList runat="server" ID="ddlDropdownColumnSearch" CssClass="NormalTextBox"
                    Visible="false">
                </asp:DropDownList>
            </td>
         
       
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
