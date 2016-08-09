<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TestM.aspx.cs" Inherits="Test_TestM" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

 <style>
        .rrpddl
        {
            height: 30px;
            border: solid 1px #909090;
            -webkit-appearance: none;
            background: url("ddlarrow.png");
            background-position: right;
            background-repeat: no-repeat;
             padding-left:10px;
        }
        
        .rrptxt
        {
            height: 25px;
            border: solid 1px #909090;
            -webkit-appearance: none;
            background: url("RRP_Calender.png");
            background-position: right;
            background-repeat: no-repeat;
           
            outline: none;
           
        }

        INPUT[type="text"] 
        {
              padding-left:10px;
        }
    </style>

    <div style=" overflow:hidden; width:200px; border-right:solid 1px #909090;">
    <asp:DropDownList runat="server" ID="ddlTest" CssClass="rrpddl" Width="220px">
        <asp:ListItem Text="Aaaaaaaaaa" Value="a"></asp:ListItem>
        <asp:ListItem Text="Bbbbbb" Value="b"></asp:ListItem>
    </asp:DropDownList>
    </div>


    <br />
    <br />
    <asp:TextBox runat="server" ID="txtDateFrom" Width="150px" CssClass="rrptxt" ValidationGroup="MKE"
        BorderWidth="1" BorderStyle="Solid" BorderColor="#909090" />
    <%--<asp:ImageButton runat="server" ID="imgDateForm" ImageUrl="~/Images/Calendar.png"
        AlternateText="Click to show calendar" CausesValidation="false" />--%>
    <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
        Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
    </ajaxToolkit:CalendarExtender>
    <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
        ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
        MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
    <br />
    <br />
    <%--<asp:CheckBox ID="chkIsActive" Checked="false" runat="server" AutoPostBack="true"/>--%>


    <%--<ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtDateFrom"
        WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
    </ajaxToolkit:TextBoxWatermarkExtender>--%>
    <%-- <asp:TextBox ID="xx1" runat="server" CssClass="textbox"></asp:TextBox>
   
    <ajaxToolkit:DropDownExtender ID="COAElement1DDE" runat="server" DropArrowImageUrl="ddlarrow.png"
        DropArrowBackColor="#dddddd" HighlightBackColor="#e9e9e9" HighlightBorderColor="#dadada"
        DynamicServicePath="" Enabled="True" TargetControlID="xx1" DropDownControlID="xx1Options">
    </ajaxToolkit:DropDownExtender>
    <asp:Panel ID="xx1Options" runat="server">
        <asp:DropDownList runat="server" ID="ddlTest" CssClass="NormalTextBox" Width="200px">
            <asp:ListItem Text="Aaaaaaaaaa" Value="a"></asp:ListItem>
            <asp:ListItem Text="Bbbbbb" Value="b"></asp:ListItem>
        </asp:DropDownList>

    </asp:Panel>--%>
    <%--         <asp:Label runat="server" ID="lblTest"></asp:Label>
               <br />

                <ajaxToolkit:DropDownExtender runat="server" TargetControlID="lblTest" Enabled="True" DropArrowImageUrl="ddlarrow.png"
        DropDownControlID="xx1Options" ID="ddlETest">
    </ajaxToolkit:DropDownExtender>
               <br />
    <asp:Panel ID="xx1Options" runat="server">
        <asp:DropDownList runat="server" ID="ddlTest" CssClass="NormalTextBox" Width="200px">
            <asp:ListItem Text="Aaaaaaaaaa" Value="a"></asp:ListItem>
            <asp:ListItem Text="Bbbbbb" Value="b"></asp:ListItem>
        </asp:DropDownList>
    </asp:Panel>--%>
    <br />

   Testing
    <hr />
    testing 2
</asp:Content>
