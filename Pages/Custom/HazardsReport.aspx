<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="HazardsReport.aspx.cs" Inherits="Pages_Custom_HazardsReport" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

    <span class="TopTitle">
       Open/Closed Hazards
    </span>
    <br />       <br />    <br />
    <div class="searchcorner">
        <table>
            <tr>
            <td style="width:50px;"></td>
                <td>
                    <strong>From Date</strong>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDateFrom" Width="100px" CssClass="NormalTextBox"
                        ValidationGroup="MKE" BorderWidth="1" BorderStyle="Solid" BorderColor="#909090" />
                    <asp:ImageButton runat="server" ID="imgDateForm" ImageUrl="~/Images/Calendar.png"
                        AlternateText="Click to show calendar" CausesValidation="false" />
                    <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                        Format="dd/MM/yyyy" PopupButtonID="imgDateForm"  FirstDayOfWeek="Monday">
                    </ajaxToolkit:CalendarExtender>
                    <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                        ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                        MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtDateFrom"
                        WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                    </ajaxToolkit:TextBoxWatermarkExtender>
                </td>
                <td>
                </td>
                <td style="width:50px;"></td>
                <td>
                    <strong>To Date</strong>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDateTo" Width="100px" CssClass="NormalTextBox"
                        ValidationGroup="MKE" BorderWidth="1" BorderStyle="Solid" BorderColor="#909090" />
                    <asp:ImageButton runat="server" ID="imgDateTo" ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar"
                        CausesValidation="false" />
                    <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                        Format="dd/MM/yyyy" PopupButtonID="imgDateTo"  FirstDayOfWeek="Monday">
                    </ajaxToolkit:CalendarExtender>
                    <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                        ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1753"
                        MaximumValue="1/1/3000"></asp:RangeValidator>
                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" TargetControlID="txtDateTo"
                        WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                    </ajaxToolkit:TextBoxWatermarkExtender>
                </td>
                <td style="width:50px;"></td>
                <td>
                    <div>
                        <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"
                            ValidationGroup="MKE"> <strong>Go</strong></asp:LinkButton>
                    </div>
                </td>
                <td style="width:50px;"></td>
            </tr>
        </table>
    </div>
    <br />
    <div>
        <chart:WebChartViewer ID="WebChartViewer1" runat="server" Visible="true" />
    </div>
</asp:Content>
