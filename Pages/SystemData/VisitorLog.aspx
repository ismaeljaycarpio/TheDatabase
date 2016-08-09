<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="VisitorLog.aspx.cs" Inherits="Pages_SystemData_VisitorLog" EnableEventValidation="false" %>

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <span class="TopTitle">Visitor Logs</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                            <div id="search" style="padding-bottom: 10px" onkeypress="abc();">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                    HeaderText="List of validation errors" />
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr>
                                        <td align="right">
                                            <strong>Visited date</strong>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtDateFrom"  Width="100px" CssClass="NormalTextBox" 
                                                ValidationGroup="MKE"  BorderStyle="Solid" BorderColor="#909090" BorderWidth="1"  />
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            <%--<ajaxToolkit:MaskedEditExtender ID="meeDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="NormalTextBox"
                                                OnInvalidCssClass="NormalTextBox" MaskType="Date" AcceptNegative="None" ErrorTooltipEnabled="True"
                                                CultureName="en-GB" />--%>
                                            <%--<ajaxToolkit:MaskedEditValidator ID="mevDateFrom" runat="server" ControlExtender="meeDateFrom"
                                                ControlToValidate="txtDateFrom" InvalidValueMessage="Date is invalid" Display="Dynamic"
                                                InvalidValueBlurredMessage="*" IsValidEmpty="true" ValidationGroup="MKE" />--%>
                                            <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtDateFrom" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                            To
                                            <asp:TextBox runat="server" ID="txtDateTo"  Width="100px" CssClass="NormalTextBox"  
                                                ValidationGroup="MKE"  BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            <%--<ajaxToolkit:MaskedEditExtender ID="meeDateTo" runat="server" TargetControlID="txtDateTo"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="NormalTextBox"
                                                OnInvalidCssClass="NormalTextBox" MaskType="Date" AcceptNegative="None" ErrorTooltipEnabled="True"
                                                CultureName="en-GB" />--%>
                                            <%--<ajaxToolkit:MaskedEditValidator ID="Maskededitvalidator1" runat="server" ControlExtender="meeDateTo"
                                                ControlToValidate="txtDateTo" InvalidValueMessage="Date is invalid" Display="Dynamic"
                                                InvalidValueBlurredMessage="*" IsValidEmpty="true" ValidationGroup="MKE" />--%>
                                            <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                                                ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1911"
                                                MaximumValue="1/1/3000"></asp:RangeValidator>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtDateTo" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <strong>Page URL</strong>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox runat="server" ID="txtPageURL" CssClass="NormalTextBox" Width="328px"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Email</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEmail" CssClass="NormalTextBox" Width="230px"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <strong>Browser</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBrowser" CssClass="NormalTextBox"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <strong>IP Address </strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtIPAddress" CssClass="NormalTextBox"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <div>
                                               
                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong> </asp:LinkButton>
                                                        
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                              
                                                            <asp:HyperLink runat="server" ID="hlBack"  CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                                    
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" Width="900px"
                            AllowPaging="True" AllowSorting="True" DataKeyNames="VisitorLogID" HeaderStyle-ForeColor="Black"
                            AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("VisitorLogID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Email" HeaderText="Email">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Browser" HeaderText="Browser">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBrowser" runat="server" Text='<%# Eval("Browser") %>' Width="220px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DateAdded" HeaderText="Date" ReadOnly="true" />
                                <asp:TemplateField SortExpression="IPAddress" HeaderText="IP Address">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIPAddress" runat="server" Text='<%# Eval("IPAddress") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="PageURL" HeaderText="Page URL">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPageURL" runat="server" Text='<%# Eval("PageURL") %>' Width="300px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField SortExpression="RefSite" HeaderText="Ref">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRefSite" runat="server" Text='<%# Eval("RefSite") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" IsInLine="true" HideAdd="true" HideDelete="true"
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
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
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
