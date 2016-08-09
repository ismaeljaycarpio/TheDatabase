<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ColumnUI.ascx.cs" 
    Inherits="Pages_UserControl_ColumnUI" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<style>
    .tblBorderCSS
    {       
        border-spacing: 0px;
         border-collapse: collapse; 
         border-top-style: solid; 
         border-top-width:0px;
    }

    .tblBorderCSS td
    {
        border-width:0px;
        padding:0px;
    }
    </style>
<asp:Panel runat="server" ID="pnlUIMain">
    
    <asp:TextBox runat="server" ID="txtTextCommon" CssClass="NormalTextBox" Width="200px" Visible="false"></asp:TextBox>

    <asp:TextBox runat="server" ID="txtNumber" CssClass="NormalTextBox" Width="150px" Visible="false"></asp:TextBox>

    <div runat="server" id="divDateTime" visible="false">
        <table class="tblBorderCSS">
            <tr>
                <td >
                     <asp:TextBox runat="server" ID="txtDTDate" Width="90px" CssClass="NormalTextBox"></asp:TextBox>
                </td>
                <td style="padding-left:2px;" >
                    <asp:ImageButton runat="server" ID="ibDT" ImageUrl="~/Images/Calendar.png"
                                AlternateText="Click to show calendar" CausesValidation="false" />
                </td>
                <td  style="padding-left:2px;" >
                     <asp:TextBox runat="server" ID="txtDTTime" Width="50px" CssClass="NormalTextBox"
                                ToolTip="Time"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>

     <div runat="server" id="divDate" visible="false">
        <table class="tblBorderCSS">
            <tr>
                <td >
                     <asp:TextBox runat="server" ID="txtDate" Width="90px" CssClass="NormalTextBox"></asp:TextBox>
                </td>
                <td  style="padding-left:2px;" >
                    <asp:ImageButton runat="server" ID="ibDate" ImageUrl="~/Images/Calendar.png"
                                AlternateText="Click to show calendar" CausesValidation="false" />
                </td>                
            </tr>
        </table>
    </div>


    <asp:TextBox runat="server" ID="txtTime" Width="50px" CssClass="NormalTextBox" ToolTip="Time" Visible="false"></asp:TextBox>

    <asp:ListBox runat="server" ID="lstListbox" Visible="false" SelectionMode="Multiple"
        Style="min-width: 200px; min-height: 100px;"></asp:ListBox>
    <asp:CheckBoxList runat="server" ID="cblListbox" Visible="false"
        Style="display:block;overflow:auto;min-width:198px;min-height:100px;max-width:350px;border:solid 1px black;" />

    <asp:DropDownList runat="server" ID="ddlDropdownCommon" Visible="false" CssClass="NormalTextBox">
    </asp:DropDownList>


    <asp:CheckBox runat="server" ID="chkCheckbox" Visible="false" TextAlign="Right" />


    <asp:RadioButtonList runat="server" ID="radioRadiobutton" Visible="false"
        CssClass="NormalTextBox" RepeatDirection="Horizontal">
    </asp:RadioButtonList>
   
    <%--number--%>
    <asp:RegularExpressionValidator ID="revNumber" ControlToValidate="txtNumber" Enabled="false"
        runat="server" ErrorMessage="Numeric!" Display="Dynamic"
        ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
    </asp:RegularExpressionValidator>

    <%--Datetime--%>
    <ajaxToolkit:CalendarExtender ID="ceDTDate" runat="server" TargetControlID="txtDTDate"
        Format="dd/MM/yyyy" PopupButtonID="ibDT" FirstDayOfWeek="Monday" Enabled="false">
    </ajaxToolkit:CalendarExtender>

    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeDTTime" TargetControlID="txtDTTime"
        AutoCompleteValue="00:00" MaskType="Time"  Mask="99:99" Enabled="false" ></ajaxToolkit:MaskedEditExtender>

    <%--date--%>
     <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate"
        Format="dd/MM/yyyy" PopupButtonID="ibDate" FirstDayOfWeek="Monday" Enabled="false">
    </ajaxToolkit:CalendarExtender>

    <%--Time--%>
    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeTime" TargetControlID="txtTime"
        AutoCompleteValue="00:00" MaskType="Time"  Mask="99:99" Enabled="false" ></ajaxToolkit:MaskedEditExtender>


</asp:Panel>
