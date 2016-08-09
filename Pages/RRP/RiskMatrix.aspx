<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="RiskMatrix.aspx.cs" Inherits="Pages_RRP_RiskMatrix" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10px 10px 10px 10px">
                <asp:Label ID="lblTitle" runat="server" Text="Risk Matrix" Visible="false"> </asp:Label>
                <br />
                <table>

                    <tr runat="server" visible="false">
                        <td colspan="3">
                            <strong>Report: </strong>
                            <asp:DropDownList runat="server" ID="ddlReport" CssClass="NormalTextBox">
                                <asp:ListItem Text="Risk Matrix" Value="risk"></asp:ListItem>
                            </asp:DropDownList> <br /><br />
                        </td>
                    </tr>

                    <tr>
                        <td valign="top">
                            <strong>Date:</strong>&nbsp;<asp:TextBox runat="server" ID="txtLeftDate" Width="100px"
                                CssClass="NormalTextBox" AutoPostBack="true" OnTextChanged="txtLeftDate_TextChanged"
                                ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                            <asp:ImageButton runat="server" ID="imgLeftDate" ImageUrl="~/Images/Calendar.png"
                                AlternateText="Click to show calendar" />
                            <ajaxToolkit:CalendarExtender ID="ce_txtLeftDate" runat="server" TargetControlID="txtLeftDate"
                                Format="dd/MM/yyyy" PopupButtonID="imgLeftDate" FirstDayOfWeek="Monday">
                            </ajaxToolkit:CalendarExtender>
                            <asp:RangeValidator ID="rngLeftDate" runat="server" ControlToValidate="txtLeftDate"
                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtLeftDate"
                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                            </ajaxToolkit:TextBoxWatermarkExtender>
                            <br />
                            <br />
                            <table border="1" cellpadding="3" cellspacing="0" runat="server" id="tblLeft">
                                <tr>
                                    <td rowspan="2" style="background-color: #DFDFDF; width:200px;">
                                        <strong>1. Assess the severity of</strong>
                                        <br />
                                        <strong>CONSEQUNENCES</strong>
                                        <br />
                                        <i>(how seriously could it hurt) </i>
                                    </td>
                                    <td colspan="5" style="background-color: #DFDFDF;">
                                        <strong>2. Assess the LIKELIHOOD or probability </strong>
                                        <br />
                                        <i>(how likely is it to be that bad?)</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        1. Almost
                                        <br />
                                        Certain
                                    </td>
                                    <td>
                                        2. Very
                                        <br />
                                        Likely
                                    </td>
                                    <td>
                                        3. Likely
                                    </td>
                                    <td>
                                        4. Unlikely
                                    </td>
                                    <td>
                                        5. Very
                                        <br />
                                        Unlikely
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        1. Kill or cause permanent disability
                                    </td>
                                    <td runat="server" id="tdL11" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdL12" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdL13" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdL14" style="background-color:#FFC000;">
                                    </td>
                                    <td runat="server" id="tdL15" style="background-color:#FFFF00;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        2. Long term illness or serious injury
                                    </td>
                                    <td runat="server" id="tdL21" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdL22" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdL23" style="background-color:#FFC000;">
                                    </td>
                                    <td runat="server" id="tdL24" style="background-color:#FFFF00;">
                                    </td>
                                    <td runat="server" id="tdL25" style="background-color:#808080;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3. Medical attention and several days off work
                                    </td>
                                    <td runat="server" id="tdL31" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdL32" style="background-color:#FFC000;">
                                    </td>
                                    <td runat="server" id="tdL33" style="background-color:#FFFF00;">
                                    </td>
                                    <td runat="server" id="tdL34" style="background-color:#808080;">
                                    </td>
                                    <td runat="server" id="tdL35"  style="background-color:#BFBFBF;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        4. First Aid Needed
                                    </td>
                                    <td runat="server" id="tdL41" style="background-color:#FFC000;">
                                    </td>
                                    <td runat="server" id="tdL42" style="background-color:#FFFF00;">
                                    </td>
                                    <td runat="server" id="tdL43" style="background-color:#808080;">
                                    </td>
                                    <td runat="server" id="tdL44" style="background-color:#BFBFBF;">
                                    </td>
                                    <td runat="server" id="tdL45"  style="background-color:#0000FF;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        5. No injuries but causes minor discomfort
                                    </td>
                                    <td runat="server" id="tdL51" style="background-color:#FFFF00;">
                                    </td>
                                    <td runat="server" id="tdL52" style="background-color:#808080;">
                                    </td>
                                    <td runat="server" id="tdL53" style="background-color:#BFBFBF;">
                                    </td>
                                    <td runat="server" id="tdL54"  style="background-color:#0000FF;">
                                    </td>
                                    <td runat="server" id="tdL55"  style="background-color:#00FF00;">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width:50px;"></td>
                        <td valign="top">
                            <asp:CheckBox runat="server" ID="chkCompare" Checked="true"  OnCheckedChanged="chkCompare_CheckedChanged"
                            TextAlign="Right" Font-Bold="true" Text="Compare to Earlier Date"  AutoPostBack="true"/>

                            &nbsp;<asp:TextBox runat="server" ID="txtRightDate" Width="100px" CssClass="NormalTextBox"
                                AutoPostBack="true" OnTextChanged="txtRightDate_TextChanged" ValidationGroup="MKE"
                                BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                            <asp:ImageButton runat="server" ID="imgDateRight" ImageUrl="~/Images/Calendar.png"
                                AlternateText="Click to show calendar"  />
                            <ajaxToolkit:CalendarExtender ID="ce_txtRightDate" runat="server" TargetControlID="txtRightDate"
                                Format="dd/MM/yyyy" PopupButtonID="imgDateRight" FirstDayOfWeek="Monday">
                            </ajaxToolkit:CalendarExtender>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtRightDate"
                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtRightDate"
                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                            </ajaxToolkit:TextBoxWatermarkExtender>
                            <br />
                            <br />
                            <table border="1" cellpadding="3" cellspacing="0" runat="server" id="tblRight">
                                <tr>
                                    <td rowspan="2" style="background-color: #DFDFDF; width:200px;">
                                        <strong>1. Assess the severity of</strong>
                                        <br />
                                        <strong>CONSEQUNENCES</strong>
                                        <br />
                                        <i>(how seriously could it hurt) </i>
                                    </td>
                                    <td colspan="5" style="background-color: #DFDFDF;">
                                        <strong>2. Assess the LIKELIHOOD or probability </strong>
                                        <br />
                                        <i>(how likely is it to be that bad?)</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        1. Almost
                                        <br />
                                        Certain
                                    </td>
                                    <td>
                                        2. Very
                                        <br />
                                        Likely
                                    </td>
                                    <td>
                                        3. Likely
                                    </td>
                                    <td>
                                        4. Unlikely
                                    </td>
                                    <td>
                                        5. Very
                                        <br />
                                        Unlikely
                                    </td>
                                </tr>
                                         <tr>
                                    <td>
                                        1. Kill or cause permanent disability
                                    </td>
                                    <td runat="server" id="tdR11" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdR12" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdR13" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdR14" style="background-color:#FFC000;">
                                    </td>
                                    <td runat="server" id="tdR15" style="background-color:#FFFF00;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        2. Long term illness or serious injury
                                    </td>
                                    <td runat="server" id="tdR21" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdR22" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdR23" style="background-color:#FFC000;">
                                    </td>
                                    <td runat="server" id="tdR24" style="background-color:#FFFF00;">
                                    </td>
                                    <td runat="server" id="tdR25" style="background-color:#808080;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3. Medical attention and several days off work
                                    </td>
                                    <td runat="server" id="tdR31" style="background-color:#FF0000;">
                                    </td>
                                    <td runat="server" id="tdR32" style="background-color:#FFC000;">
                                    </td>
                                    <td runat="server" id="tdR33" style="background-color:#FFFF00;">
                                    </td>
                                    <td runat="server" id="tdR34" style="background-color:#808080;">
                                    </td>
                                    <td runat="server" id="tdR35"  style="background-color:#BFBFBF;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        4. First Aid Needed
                                    </td>
                                    <td runat="server" id="tdR41" style="background-color:#FFC000;">
                                    </td>
                                    <td runat="server" id="tdR42" style="background-color:#FFFF00;">
                                    </td>
                                    <td runat="server" id="tdR43" style="background-color:#808080;">
                                    </td>
                                    <td runat="server" id="tdR44" style="background-color:#BFBFBF;">
                                    </td>
                                    <td runat="server" id="tdR45"  style="background-color:#0000FF;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        5. No injuries but causes minor discomfort
                                    </td>
                                    <td runat="server" id="tdR51" style="background-color:#FFFF00;">
                                    </td>
                                    <td runat="server" id="tdR52" style="background-color:#808080;">
                                    </td>
                                    <td runat="server" id="tdR53" style="background-color:#BFBFBF;">
                                    </td>
                                    <td runat="server" id="tdR54"  style="background-color:#0000FF;">
                                    </td>
                                    <td runat="server" id="tdR55"  style="background-color:#00FF00;">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />

                <asp:CheckBox runat="server"  ID="chkShowChange" Font-Bold="true" AutoPostBack="true"  Checked="true" Visible="false"
                 Text="Show Change of Risk Status" TextAlign="Right" OnCheckedChanged="chkShowChange_CheckedChanged" />
                <br />
                <br />
               
                <strong>Key</strong><br />
                <table cellpadding="5" style="font-weight: bold;">
                    <tr>
                        <td style="background-color: #FF0000; width: 120px; text-align: center;">
                            1= Extreme Risk
                        </td>
                        <td style="background-color: #FFC000; width: 130px; text-align: center;">
                            2= Very High Risk
                        </td>
                        <td style="background-color: #FFFF00; width: 120px; text-align: center;">
                            3= High Risk
                        </td>
                        <td style="background-color: #808080; width: 120px; text-align: center;">
                            4= Moderate Risk
                        </td>
                        <td style="background-color: #BFBFBF; width: 120px; text-align: center;">
                            5= Low Risk
                        </td>
                        <td style="background-color: #0000FF; width: 120px; text-align: center;">
                            6= Very Low Risk
                        </td>
                        <td style="background-color: #00FF00; width: 130px; text-align: center;">
                            7= Negligible Risk
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
