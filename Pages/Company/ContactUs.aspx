<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Marketing2.master" AutoEventWireup="true"
    CodeFile="ContactUs.aspx.cs" Inherits="Pages_Company_ContactUs" %>

<%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div class="ContentM">
        <table style="margin: 0px; padding: 0px; width: 100%;" cellspacing="0">
            <tr>
                <td style="width: 30px">
                </td>
                <td colspan="3" class="ContentTop">
                    <h1 style="line-height: 0px">
                        Contact Us</h1>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="vsContactUs" runat="server" EnableClientScript="true"
                                    ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct the following errors:" />
                            </td>
                        </tr>
                        <tr id="Tr1" runat="server" visible="true">
                            <td align="right" style="width: 20px;">
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" Width="335px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="Name Required." Display="None"></asp:RequiredFieldValidator>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="tbwCUName" TargetControlID="txtName" WatermarkText="Your Name"
                                    runat="server" >
                                </ajaxToolkit:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="335px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="Email Required." Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="None" ID="REVEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="Email Invalid." ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                            </asp:RegularExpressionValidator>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="tbwCUEmail" TargetControlID="txtEmail"
                                    WatermarkText="Your Email" runat="server">
                                </ajaxToolkit:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr id="Tr2" runat="server" visible="true">
                            <td align="right">
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtMessage" runat="server" Text="" TextMode="MultiLine" Width="335px"
                                    Height="400px" CssClass="MultiLineTextBox" Font-Size="13px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage"
                                    ErrorMessage="Message Required." Display="None"></asp:RequiredFieldValidator>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="tbweMessage" TargetControlID="txtMessage"
                                    WatermarkText="Your Message" runat="server">
                                </ajaxToolkit:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left" valign="middle">
                                <div>
                                    <%--<asp:ImageButton runat="server" ID="lnkSubmit" ImageUrl="~/Images/Send1.png" CausesValidation="true"
                                                    OnClick="lnkSubmit_Click"></asp:ImageButton>--%>
                                    <div runat="server" id="divSave">
                                       
                                                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSubmit_Click"
                                                        CausesValidation="true"> <strong>Send </strong> </asp:LinkButton>
                                             
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 40px;">
                </td>
                <td align="left" valign="top" style="padding-top: 25px;">
                    <div style="text-align: left">
                        <DBGurus:DBGContent ID="dbgContent" runat="server" ConnectionName="CnString" ContentKey="ContactUs"
                            TableName="Content" ExtenderPath="Extender/" ShowInlineContentEditor="false"
                            UseAssetManager="true" />
                        <br />
                        </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
