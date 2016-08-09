<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="MakePayment.aspx.cs" Inherits="Pages_Security_MakePayment" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script type="text/javascript">
        // alert(lblInvoiceAmount.value);

        function CalculateValue() {
            var optMonthly = document.getElementById("optMonthly");
            var optYearly = document.getElementById("optYearly");
            var optOnce = document.getElementById("optOnce");
            var hfAustralian = document.getElementById("hfAustralian");

            if (hfAustralian.value == 'y') {
                $("#lblMonthly").text("Monthly ($495+GST / month))");
                $("#lblYearly").text("Yearly ($4,950+GST / year) - get 2 months free!");
                $("#lblOnce").text("Once ($9,900+GST / year) pay for full 5 years!");
                if (optMonthly.checked == true) {
                    $("#lblInvoiceAmount").text("$544.4");
                    $("#optCheque").fadeOut();
                    $("#lblChequeOpt").fadeOut();
                    $("#optEFT").fadeOut();
                    $("#lblEFT").fadeOut();
                }
                if (optYearly.checked == true) {
                    $("#lblInvoiceAmount").text("$5,445");
                    $("#optCheque").fadeIn();
                    $("#lblChequeOpt").fadeIn();
                    $("#lblEFT").fadeIn();
                    $("#optEFT").fadeIn();
                }

                if (optOnce.checked == true) {
                    $("#lblInvoiceAmount").text("$10,890");
                    $("#optCheque").fadeIn();
                    $("#lblChequeOpt").fadeIn();
                    $("#optEFT").fadeIn();
                    $("#lblEFT").fadeIn();
                }
            }
            else {

                $("#lblMonthly").text("Monthly ($495 / month))");
                $("#lblYearly").text("Yearly ($4,950 / year) - get 2 months free!");
                $("#lblOnce").text("Once ($9,900 / year) pay for full 5 years!");


                if (optMonthly.checked == true) {
                    $("#lblInvoiceAmount").text("$495");
                    $("#optCheque").fadeOut();
                    $("#lblChequeOpt").fadeOut();
                    $("#optEFT").fadeOut();
                    $("#lblEFT").fadeOut();
                }
                if (optYearly.checked == true) {
                    $("#lblInvoiceAmount").text("$4,950");
                    $("#optCheque").fadeIn();
                    $("#lblChequeOpt").fadeIn();
                    $("#optEFT").fadeIn();
                    $("#lblEFT").fadeIn();
                }

                if (optOnce.checked == true) {
                    $("#lblInvoiceAmount").text("$9,900");
                    $("#optCheque").fadeIn();
                    $("#lblChequeOpt").fadeIn();
                    $("#optEFT").fadeIn();
                    $("#lblEFT").fadeIn();
                }
            }




            InvoiceDetail();

        }

        function InvoiceDetail() {
            var optPayPal = document.getElementById("optPayPal");
            var optEFT = document.getElementById("optEFT");
            var optCheque = document.getElementById("optCheque");
            var hfAustralian = document.getElementById("hfAustralian");
            
            if (optPayPal.checked == true) {
                $("#divPymentMade").hide();
                $("#divPayNow").show();
                $("#trInvoiceDetail").hide();
            }
            else {
                $("#divPymentMade").show();
                $("#divPayNow").hide();
                $("#trInvoiceDetail").show();
                if (optEFT.checked == true) {
                    $("#lblCheque").hide();
                    $("#lblEFTDetail").show();
                  
                    if (hfAustralian.value == 'y') {
                        $("#lblSwiftCode").hide();
                    }
                    else {
                        $("#lblSwiftCode").show();
                    }

                }

                if (optCheque.checked == true) {
                    $("#lblCheque").show();
                    $("#lblEFTDetail").hide();
                    $("#lblSwiftCode").hide();
                }

                }


            }

        


        $(document).ready(function () {

            $("#optMonthly").click(function () {
                CalculateValue();
            });

            $("#optYearly").click(function () {
                CalculateValue();
            });

            $("#optOnce").click(function () {
                CalculateValue();
            });


            $("#optPayPal").click(function () {
                InvoiceDetail();
            });

            $("#optEFT").click(function () {
                InvoiceDetail();
            });

            $("#optCheque").click(function () {
                InvoiceDetail();
            });

        }); 
    </script>
    <div class="ContentMain" style="width: 950px; min-height: 700px; background-color: #ffffff;
        padding-left: 50px;">
        <table border="0" cellpadding="0" cellspacing="0" align="left">
            <tr>
                <td colspan="3" height="40">
                    <span class="TopTitle">
                        <asp:Label runat="server" ID="lblTitle" Text="Make Payment"></asp:Label></span>
                </td>
            </tr>
            <tr>
                <td colspan="3" height="13">
                    <asp:HiddenField runat="server" ID="hfAustralian" Value="y" ClientIDMode="Static" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="3">
                    <asp:Panel ID="Panel2" runat="server">
                        <div runat="server" id="divDetail">
                            <table>
                                <tr>
                                    <td align="left" colspan="3" style="padding: 10px;">
                                        <div style="border-width: 1px; border-style: solid; padding: 5px; width: 100%;">
                                            <asp:Label runat="server" ID="lblContent" CssClass="NormalTextBox" ForeColor="Red"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="3">
                                            <tr>
                                                <td colspan="2" style="height: 25px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <strong>Account Type </strong>
                                                </td>
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label runat="server" ID="lblAccountInfo"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 7px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label runat="server" ID="lblFrequency" Text="Frequency" Font-Bold="true" ToolTip="Billing frequency – you can pay for either a month or a year in advance. If you may yearly then you get a free month each year."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton runat="server" ID="optMonthly" GroupName="Frequency" Checked="true"
                                                        TabIndex="2" ClientIDMode="Static" />
                                                    <asp:Label runat="server" ID="lblMonthly" Text="Monthly ($495+GST / month)" ClientIDMode="Static"></asp:Label>
                                                    <br />
                                                    <asp:RadioButton runat="server" ID="optYearly" GroupName="Frequency" ClientIDMode="Static"
                                                        TabIndex="2" />
                                                    <asp:Label runat="server" ID="lblYearly" Text="Yearly ($4,950+GST / year) - get 2 months free!"
                                                        ClientIDMode="Static"></asp:Label>
                                                    <br />
                                                    <asp:RadioButton runat="server" ID="optOnce" GroupName="Frequency" ClientIDMode="Static"
                                                        TabIndex="3" />
                                                    <asp:Label runat="server" ID="lblOnce" Text="Once ($9,900+GST / year) pay for full 5 years!"
                                                        ClientIDMode="Static"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 7px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label runat="server" ID="lblMethod" Font-Bold="true" Text="Method" ToolTip="Pay by PayPal or Electronic Funds Transfer."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton runat="server" ID="optPayPal" GroupName="Method" Checked="true"
                                                        TabIndex="3" Text="PayPal" ClientIDMode="Static" />
                                                    <br />
                                                    <asp:RadioButton runat="server" ID="optEFT" GroupName="Method" 
                                                        ClientIDMode="Static" TabIndex="3" />
                                                        <asp:Label runat="server" ID="lblEFT" Text="EFT – available to Australians on Yearly plans only"
                                                        ClientIDMode="Static"></asp:Label>
                                                    <br />
                                                    <asp:RadioButton runat="server" ID="optCheque" GroupName="Method" ClientIDMode="Static"
                                                        TabIndex="3" />
                                                    <asp:Label runat="server" ID="lblChequeOpt" Text="Cheque – available for Yearly plans only"
                                                        ClientIDMode="Static"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 7px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <strong>Invoice Amount</strong>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblInvoiceAmount" Text="$544.50" ClientIDMode="Static"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 7px">
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trInvoiceDetail" clientidmode="Static" style="display: none;">
                                                <td align="right" valign="top">
                                                    <strong>Invoice Detail</strong>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCheque" ClientIDMode="Static" Text="Text Detail" />
                                                    <asp:Label runat="server" ID="lblEFTDetail" Text="Text Detail" ClientIDMode="Static" />
                                                    <br />
                                                    <asp:Label runat="server" ID="lblSwiftCode" ClientIDMode="Static" Text="Code" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <strong>Transaction Ref*</strong>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtTransactionRef"></asp:TextBox>
                                                            </td>
                                                            <td class="normalinfocolor">
                                                                Please enter the bank transaction reference
                                                                <br />
                                                                in case there is any problem locating the Invoice.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div runat="server" id="divPayNow" clientidmode="Static">
                                                                    <asp:LinkButton runat="server" ID="lnkPayNow" CssClass="btn" TabIndex="7" OnClick="lnkPayNow_Click"> <strong> Pay Now</strong> </asp:LinkButton>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divPymentMade" style="display: none;" clientidmode="Static">
                                                                    <asp:LinkButton runat="server" ID="lnkInvoiceMade" CssClass="btn" OnClick="lnkInvoiceMade_Click"
                                                                        TabIndex="8" OnClientClick="alert('If Invoice is not received within the next 7 days your account may be disabled.');"><strong>Invoice Made</strong></asp:LinkButton>
                                                                </div>
                                                            </td>
                                                            <td runat="server" id="tdPayLater">
                                                                <div>
                                                                    <asp:HyperLink runat="server" ID="hlPayLater" CssClass="btn" TabIndex="9" NavigateUrl="~/Default.aspx"> <strong>Pay Later</strong> </asp:HyperLink>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divCancle">
                                                                    <asp:HyperLink runat="server" ID="hlBack" CssClass="btn" TabIndex="10" NavigateUrl="~/Default.aspx"> <strong>Cancel</strong> </asp:HyperLink>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 20px;">
                                    </td>
                                    <td valign="top">
                                        <%--<table>
                                            <tr>
                                                <td colspan="3" style="height: 25px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label runat="server" ID="lblPrimaryPhone" Text="Primary Phone Number*" Font-Bold="true"
                                                        ToolTip="In order to place an order we need a phone number to contact you on in case there is a problem."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhoneNumber" runat="server" Width="200px" CssClass="NormalTextBox"
                                                        TabIndex="4"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp;<asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber"
                                                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label runat="server" ID="lblBackupEmail" Font-Bold="true" Text="Backup email address*"
                                                        ToolTip="Please enter a secondary email address in case of a problem with your primary email address."></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBackUpEmail" runat="server" Width="200px" CssClass="NormalTextBox"
                                                        TabIndex="5"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBackUpEmail"
                                                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <strong></strong>
                                                    <asp:Label runat="server" ID="lblBillingAddress" Text="Billing Address*" Font-Bold="true"
                                                        ToolTip="For legal reasons we require a billing address.  Please include the state, postcode and country (if outside of Australia)."></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtBillingAddress" runat="server" Width="200px" TextMode="MultiLine"
                                                        TabIndex="6" CssClass="MultiLineTextBox" Height="100px" Font-Size="11px"></asp:TextBox>
                                                </td>
                                                <td valign="top">
                                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBillingAddress"
                                                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>--%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        CalculateValue();
    </script>
</asp:Content>
