<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="InvoiceDetail.aspx.cs" Inherits="Pages_Security_InvoiceDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%-- <link href="<% =ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css") %>" rel="stylesheet"     type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <script type="text/javascript">
        function abc() {
            var b = document.getElementById('<%= lnkSave.ClientID %>');
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
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                        </td>
                        <td align="left">
                            <div style="width: 40px; height: 40px;">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">
                                    <tr>
                                        <td align="right">
                                            <strong>InvoiceID</strong>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblInvoiceID"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Account ID</strong>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblAccountID"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Account Name</strong>
                                        </td>
                                        <td>
                                              <asp:Label runat="server" ID="lblAccountName"></asp:Label>
                                               <asp:DropDownList runat="server" ID="ddlAccount" DataTextField="AccountName"
                                                         DataValueField="AccountID" CssClass="NormalTextBox"  AutoPostBack="true"
                                                  onselectedindexchanged="ddlAccount_SelectedIndexChanged"></asp:DropDownList>
                                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlAccount"
                                            ErrorMessage="required" Display="Dynamic"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Account Type</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAccountType" runat="server" DataTextField="AccountTypeName"
                                                DataValueField="AccountTypeID" CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>NetAmountAUD*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNetAmountAUD" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNetAmountAUD"
                                            ErrorMessage="required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>GSTAmountAUD</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGSTAmountAUD" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>GrossAmountAUD</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGrossAmountAUD" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>InvoiceDate</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtInvoiceDate" Width="100px" CssClass="NormalTextBox"
                                                ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtInvoiceDate"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtInvoiceDate"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtInvoiceDate"
                                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                            </ajaxToolkit:TextBoxWatermarkExtender>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtInvoiceDate"
                                            ErrorMessage="required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>StartDate</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtStartDate" Width="100px" CssClass="NormalTextBox"
                                                ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtStartDate"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtStartDate"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtStartDate"
                                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                            </ajaxToolkit:TextBoxWatermarkExtender>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>EndDate</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEndDate" Width="100px" CssClass="NormalTextBox"
                                                ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtEndDate"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtEndDate"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" TargetControlID="txtEndDate"
                                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                            </ajaxToolkit:TextBoxWatermarkExtender>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Payment Method</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="NormalTextBox">
                                                <asp:ListItem Value="P" Text="Paypal"></asp:ListItem>
                                                <asp:ListItem Value="E" Text="EFT" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="C" Text="Cheque"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>PaypalID</strong>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblPaypalID"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>PaidDate</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPaidDate" Width="100px" CssClass="NormalTextBox"
                                                ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtPaidDate"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtPaidDate"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" TargetControlID="txtPaidDate"
                                                WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                            </ajaxToolkit:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Notes</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNotes" TextMode="MultiLine" Height="100px" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>OrganisationName</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrganisationName" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>BillingEmail</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillingEmail" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>BillingAddress</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillingAddress" TextMode="MultiLine" Height="100px" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Country</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCountry" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>ClientRef</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtClientRef" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trPayPalDetail" visible="false">
                                        <td align="right" valign="top">
                                            <strong>PayPal Detail</strong>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblPayPalDetail" CssClass="NormalTextBox"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div runat="server" id="divSave">
                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                    CausesValidation="true"> <strong>Save</strong> </asp:LinkButton>
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false">
                                                <asp:HyperLink runat="server" ID="hlEditLink" CssClass="btn"><strong>Edit</strong></asp:HyperLink>
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divDelete" visible="false">
                                                <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="javascript:return confirm('Are you sure you want to delete this Invoice?')"
                                                    CssClass="btn" CausesValidation="false" OnClick="lnkDelete_Click"> <strong>Delete</strong> </asp:LinkButton>
                                            </div>
                                        </td>
                                          <td>
                                            <div runat="server" id="divSendInvoice">
                                                <asp:LinkButton runat="server" ID="lnkSendInvoice" CssClass="btn" OnClick="lnkSendInvoice_Click"
                                                    CausesValidation="true"> <strong>Send Invoice</strong> </asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <span style="font-weight: bold" align="center"></span>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
        </tr>
        <tr>
            <td colspan="3" height="50Px">
            </td>
        </tr>
    </table>
</asp:Content>
