<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateAndTimePicker.ascx.cs" Inherits="Test_DateAndTimePicker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<div>
    <table>
        <tr>
            <td valign="top" style="padding-top:8px;">Date: </td>
            <td valign="top">
             <asp:TextBox ID="PostDateTextBox" runat="server" Width="100px"></asp:TextBox> &nbsp;<span style="font-size:10px">Date Format: <asp:Literal ID="DateFormatLiteral" runat="server"></asp:Literal></span>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="PostDateTextBox"  FirstDayOfWeek="Monday">
                </ajaxToolkit:CalendarExtender><br />
                <asp:RequiredFieldValidator ID="DateRequiredFieldValidator1" runat="server" ControlToValidate="PostDateTextBox"
                    ErrorMessage="Date is Required"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="DateCompareValidator1" runat="server" ControlToValidate="PostDateTextBox" ErrorMessage="Date must be formated as a date"
                    Operator="DataTypeCheck" Type="Date"></asp:CompareValidator><br />
            </td>
            <td valign="top" style="padding-top:8px;">Time: </td>
            <td valign="top">
           <div id="TimePickeDiv" style="width:350px;display:inline-table;">
            <asp:TextBox ID="TimeOfDayTextBox" runat="server" Width="90px" /><asp:Image ID="DropDownImage" runat="server" ImageUrl="dropdown-button.png" ImageAlign="AbsMiddle" /> &nbsp;<span style="font-size:10px">Time format: <asp:Literal ID="TimeFormatLiteral" runat="server"></asp:Literal></span>
            <div style="position:relative;">
                <asp:ListBox ID="TimeHourHalfListBox" runat="server" Rows="8" style="width:120px;position:absolute;left:0px;display:none;"></asp:ListBox>
                <asp:RequiredFieldValidator ID="TimeRequiredFieldValidator1" runat="server" ControlToValidate="TimeOfDayTextBox"
                    ErrorMessage="Time is Required"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="TimeFormatCustomValidator" runat="server" ErrorMessage="Time format is invalid" OnServerValidate="TimeFormatCustomValidator_ServerValidate"></asp:CustomValidator>
              </div>
            </div>
            </td>
        </tr>
    </table>
</div>