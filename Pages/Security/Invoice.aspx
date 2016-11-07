<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="Invoice.aspx.cs" Inherits="Pages_Security_Invoice" EnableEventValidation="false" %>

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
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                    <tr>
                        <td colspan="3" >
                            <table width="100%" cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left"  style="width:50%;">
                                        <span class="TopTitle">Invoices</span>
                                    </td>
                                    <td align="left">
                                        <div style="width:40px; height:40px;">
                                            <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" >
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
                            <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                                <div id="search" style="padding-bottom: 10px" onkeypress="abc();">
                                   
                                    <table style="border-collapse: collapse" cellpadding="4">
                                        <tr>
                                            <td align="right">
                                                <strong>Invoice Date</strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox runat="server" ID="txtInvoiceDateFrom"  Width="100px" 
                                                    CssClass="NormalTextBox" ValidationGroup="MKE"  BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtInvoiceDateFrom"
                                                    Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                                </ajaxToolkit:CalendarExtender>
                                                <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtInvoiceDateFrom"
                                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="NormalTextBox"
                                                    OnInvalidCssClass="NormalTextBox" MaskType="Date" AcceptNegative="None" ErrorTooltipEnabled="True"
                                                    CultureName="en-GB" />--%>
                                                <%--<ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender1"
                                                    ControlToValidate="txtInvoiceDateFrom" InvalidValueMessage="Date is invalid"
                                                    Display="Dynamic" InvalidValueBlurredMessage="*" IsValidEmpty="true" ValidationGroup="MKE" />--%>
                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtInvoiceDateFrom"
                                                    ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                    MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                  <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtInvoiceDateFrom" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                               
                                                To
                                                <asp:TextBox runat="server" ID="txtInvoiceDateTo"  Width="100px" 
                                                    CssClass="NormalTextBox" ValidationGroup="MKE"  BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtInvoiceDateTo"
                                                    Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                                </ajaxToolkit:CalendarExtender>
                                                <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtInvoiceDateTo"
                                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="NormalTextBox"
                                                    OnInvalidCssClass="NormalTextBox" MaskType="Date" AcceptNegative="None" ErrorTooltipEnabled="True"
                                                    CultureName="en-GB" />--%>
                                                <%--<ajaxToolkit:MaskedEditValidator ID="Maskededitvalidator3" runat="server" ControlExtender="MaskedEditExtender2"
                                                    ControlToValidate="txtInvoiceDateTo" InvalidValueMessage="Date is invalid" Display="Dynamic"
                                                    InvalidValueBlurredMessage="*" IsValidEmpty="true" ValidationGroup="MKE" />--%>
                                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtInvoiceDateTo"
                                                    ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                    MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                  <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtInvoiceDateTo" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                            </td>
                                            <td style="width: 10px;">
                                            </td>
                                            <td align="right">
                                                <strong>Account Name</strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox runat="server" ID="txtAccountName" CssClass="NormalTextBox"></asp:TextBox>
                                            </td>
                                            <td style="width: 10px;">
                                            </td>
                                            <td align="right">
                                                <strong>Invoice Confirmed</strong>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList runat="server" ID="ddlIsPaid" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlIsPaid_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                               
                                            </td>
                                            <td align="left">
                                               
                                            </td>
                                            <td>
                                            </td>
                                            <td align="right">
                                                <strong>Email</strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox runat="server" ID="txtEmail" CssClass="NormalTextBox"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td align="right">
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td>
                                              
                                                <div>
                                                    
                                                        <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong></asp:LinkButton>
                                                            
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                           
                            <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                 HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                AllowPaging="True" AllowSorting="True" DataKeyNames="InvoiceID" HeaderStyle-ForeColor="Black"
                                Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                                OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound">
                                <PagerSettings Position="Top" />
                                <Columns>
                                  
                                    <asp:TemplateField Visible="false">
                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="LblID" runat="server" Text='<%# Eval("InvoiceID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:HyperLink ID="viewHyperLink" runat="server" ToolTip="View" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("InvoiceID").ToString())  %>'
                                                ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 
                                    <asp:TemplateField HeaderText="Account Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountName" runat="server" Text='<%# Eval("AccountName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Billing Email">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillingEmail" runat="server" Text='<%# Eval("BillingEmail") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GrossAmountAUD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossAmountAUD" runat="server" Text='<%# Eval("GrossAmountAUD") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Paid">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsPaid" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("InvoiceDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Method">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaymentMethod" runat="server" Text='<%# Eval("PaymentMethod") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gridview_header" />
                                <RowStyle CssClass="gridview_row" />
                                <PagerTemplate>
                                    <asp:GridViewPager runat="server" ID="Pager" HideAdd="false" HideDelete="true" OnApplyFilter="Pager_OnApplyFilter"
                                        OnBindTheGridToExport="Pager_BindTheGridToExport" OnBindTheGridAgain="Pager_BindTheGridAgain" />
                                </PagerTemplate>
                            </dbg:dbgGridView>
                            <br />
                            <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                                <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                    <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                    No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                        Add new record now.</strong>
                                </asp:HyperLink>
                            </div>
                            <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                                <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                    OnClick="Pager_OnApplyFilter">
                                    <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                    No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                        Clear filter</strong>
                                </asp:LinkButton>
                                                           or
                            <asp:HyperLink runat="server" ID="hplNewDataFilter" Style="text-decoration: none;
                                color: Black;">                                
                                  <strong style="text-decoration: underline; color: Blue;">
                                     Add new record.</strong>
                            </asp:HyperLink>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
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
                 <asp:AsyncPostBackTrigger ControlID="lnkSearch" />
                  <asp:AsyncPostBackTrigger ControlID="ddlIsPaid" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
