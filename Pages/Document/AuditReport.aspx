<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="AuditReport.aspx.cs" Inherits="Pages_Document_AuditReport" EnableEventValidation="false" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
   <%-- <link href="<% =ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css") %>" rel="stylesheet"      type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <script language="javascript" type="text/javascript">
        function abc() {
            var b = document.getElementById('<%= lnkSearch.ClientID %>');
            if (b && typeof (b.click) == 'undefined') {
                b.click = function () {
                    var result = true;
                    if (b.onclick) result = b.onclick();
                    if (typeof (result) == 'undefined' || result) {
                        eval(b.getAttribute('href'));
                    }
                }
            }

        }

        function confirmation() {
            var answer = confirm("Do you want to save this PDF as Document?")
            if (answer) {
                var a = document.getElementById('hfSaveToDocument');
                a.value = 'true';
            }
            else {
                var b = document.getElementById('hfSaveToDocument');
                b.value = 'false';
            }
        }


    </script>

     <script type="text/javascript">
         function MouseEvents(objRef, evt) {
             if (evt.type == "mouseover") {
                 objRef.style.backgroundColor = "#76BAF2";
                 objRef.style.cursor = 'pointer';
             }
             else {

                 if (evt.type == "mouseout") {
                     if (objRef.rowIndex % 2 == 0) {
                         //Alternating Row Color
                         objRef.style.backgroundColor = "white";
                     }
                     else {
                         objRef.style.backgroundColor = "#DCF2F0";
                     }
                 }
             }
         }



    </script>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <asp:Label runat="server" ID="lblAdminArea" CssClass="TopTitle" Text="Admin Area:"></asp:Label>
                                    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlAdminArea" CssClass="TopTitle"
                                        OnSelectedIndexChanged="ddlAdminArea_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hfSaveToDocument" ClientIDMode="Static" Value="false" />
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <img alt="Processing..." src="../../Images/ajax.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                    </td>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                            <div id="search"  class="searchcorner" onkeypress="abc();">
                                <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                HeaderText="List of validation errors" />--%>
                                    <br />
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr>
                                        <td align="right">
                                            <strong>Table</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlTable" AutoPostBack="true" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                CssClass="NormalTextBox">
                                                <asp:ListItem Value="" Text="All" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="Record" Text="Record"></asp:ListItem>
                                                <asp:ListItem Value="Table" Text="Table"></asp:ListItem>
                                                <asp:ListItem Value="Column" Text="Record Column"></asp:ListItem>
                                                <asp:ListItem Value="Location" Text="Location"></asp:ListItem>                                               
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <strong>User</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="true" DataTextField="FullName"
                                                DataValueField="UserID" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged"
                                                CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <strong runat="server" id="lblTable">Table</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTable1" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable1_SelectedIndexChanged"
                                                CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Changes From</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDateFrom"  Width="100px" CssClass="NormalTextBox"  
                                                ValidationGroup="MKE"  BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                                 <asp:ImageButton runat="server" ID="imgDateForm"  ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false"/>  
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                Format="dd/MM/yyyy" PopupButtonID="imgDateForm"  FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                          
                                            <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                             <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtDateFrom" WatermarkText="dd/mm/yyyy"
                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>

                                            To
                                            <asp:TextBox runat="server" ID="txtDateTo" Width="100px" CssClass="NormalTextBox" 
                                                ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                                <asp:ImageButton runat="server" ID="imgDateTo"  ImageUrl="~/Images/Calendar.png"  AlternateText="Click to show calendar" CausesValidation="false"/>
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                                Format="dd/MM/yyyy" PopupButtonID="imgDateTo"  FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                           
                                            <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                                                ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1753"
                                                MaximumValue="1/1/3000"></asp:RangeValidator>
                                             <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtDateTo" WatermarkText="dd/mm/yyyy"
                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                        </td>
                                        <td style="width: 10px;">
                                        </td>
                                        <td align="right">
                                            <strong>Search</strong>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox runat="server" ID="txtSearch" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <%--<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="../../Images/iconGo.png" 
                                            onclick="btnSearch_Click" />--%>
                                            <div>
                                                
                                                <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" 
                                                OnClick="lnkSearch_Click"> <strong> Go</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvChangedLog" runat="server" GridLines="Both" CssClass="gridview"
                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                            AllowSorting="false" DataKeyNames="DateAdded" HeaderStyle-ForeColor="Black" Width="100%"
                            AutoGenerateColumns="false" PageSize="15" OnPreRender="gvChangedLog_PreRender"
                            OnRowDataBound="gvChangedLog_RowDataBound"  AlternatingRowStyle-BackColor="#DCF2F0">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hlView" CssClass="popuplink">
                                            <asp:Image runat="server" ID="imgView" ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="true" HeaderText="Table">
                                   
                                    <ItemTemplate>
                                        <asp:Label ID="lblTableName1" runat="server" Text='<%# Eval("TableName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="true" HeaderText="Updated Date" HeaderStyle-HorizontalAlign="Center">
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="UpdateDate" runat="server" Text='<%# Eval("DateAdded","{0:g}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="true" HeaderText="User">
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="lblUser" runat="server" Text='<%# Eval("User") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="true" HeaderText="Changed Column List">
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="lblColumnList" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="true" HeaderText="Table">
                                   
                                    <ItemTemplate>
                                        <asp:Label ID="lblTableName" runat="server" Text='<%# Eval("TableName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="PK">
                                   
                                    <ItemTemplate>
                                        <asp:Label ID="lblPK" runat="server" Text='<%# Eval("PrimaryKeyValue") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField Visible="true" HeaderText="Reason for change">                                                                           
                                    <ItemTemplate>
                                        <asp:Label ID="lblResonForChange" runat="server" Text='<%# Eval("ResonForChange") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="CL_Pager" HideAdd="true" HideDelete="true"
                                    OnBindTheGridToExport="CL_Pager_BindTheGridToExport" OnApplyFilter="CL_Pager_OnApplyFilter"
                                    OnBindTheGridAgain="CL_Pager_BindTheGridAgain" OnExportForCSV="CL_Pager_OnExportForCSV"
                                    OnCustomExportPDF="CL_Pager_OnCustomExportPDF" DOCustomPDF="true" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="CL_Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                    Clear filter.</strong>
                            </asp:LinkButton>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                        <br />
                        <span style="font-weight: bold" align="center"></span>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvChangedLog" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
