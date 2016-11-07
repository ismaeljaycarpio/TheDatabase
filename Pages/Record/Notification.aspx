<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="Notification.aspx.cs" Inherits="Pages_Record_Notification" EnableEventValidation="false" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
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

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <img alt="Processing..." src="../../Images/ajax.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>--%>
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
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                                    <div id="search" class="searchcorner" onkeypress="abc();">
                                        <br />
                                        <table style="border-collapse: collapse" cellpadding="4">
                                            <tr>
                                                <td align="right">
                                                    <strong>Record Date</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDateFrom" Width="100px" CssClass="NormalTextBox" 
                                                        ValidationGroup="MKE"  BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                                      <asp:ImageButton runat="server" ID="imgDateForm"  ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false"/>  
                                                    <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                        Format="dd/MM/yyyy" PopupButtonID="imgDateForm" FirstDayOfWeek="Monday">
                                                    </ajaxToolkit:CalendarExtender>
                                                  
                                                    <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                        ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                        MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                      <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtDateFrom" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>

                                                    To
                                                    <asp:TextBox runat="server" ID="txtDateTo"  Width="100px" CssClass="NormalTextBox"  
                                                        ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                                        <asp:ImageButton runat="server" ID="imgDateTo"  ImageUrl="~/Images/Calendar.png"  AlternateText="Click to show calendar" CausesValidation="false"/>
                                                    <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                                        Format="dd/MM/yyyy" PopupButtonID="imgDateTo" FirstDayOfWeek="Monday">
                                                    </ajaxToolkit:CalendarExtender>
                                                   
                                                    <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                                                        ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1753"
                                                        MaximumValue="1/1/3000"></asp:RangeValidator>
                                                      <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtDateTo" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <div>
                                                        
                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" 
                                                            OnClick="lnkSearch_Click"> <strong>Go</strong></asp:LinkButton>
                                                             
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </div>
                                </asp:Panel>
                                <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                    AllowPaging="True" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                    AllowSorting="True" DataKeyNames="TableID" HeaderStyle-ForeColor="Black"
                                    Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                                    OnPreRender="gvTheGrid_PreRender" AlternatingRowStyle-BackColor="#DCF2F0"
                                     OnRowDataBound="gvTheGrid_RowDataBound">
                                    <PagerSettings Position="Top" />
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("TableID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastWarningTime" runat="server" Text='<%# Eval("LastWarningTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Table">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTableName" runat="server" Text='<%# Eval("TableName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Warning">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("TableID").ToString())  %>'
                                                    Text='<%# Eval("WarningCount")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridview_header" />
                                    <RowStyle CssClass="gridview_row" />
                                    <PagerTemplate>
                                        <asp:GridViewPager runat="server" ID="Pager" HideAdd="true" HideDelete="true" OnExportForCSV="Pager_OnExportForCSV"
                                            OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                            OnBindTheGridAgain="Pager_BindTheGridAgain" />
                                    </PagerTemplate>
                                </dbg:dbgGridView>
                                <br />
                                <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                                    <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                        OnClick="Pager_OnApplyFilter">
                                        <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                        No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                            Clear filter.</strong>
                                    </asp:LinkButton>
                                </div>
                                </div>
                                <br />
                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                            </Triggers>
                        </asp:UpdatePanel>
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
            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
