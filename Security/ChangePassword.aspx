<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Security_ChangePassword"
    MasterPageFile="~/Home/Home.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
  <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"   type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>

    <script type="text/javascript">
        function validatePassword(validator, arg) {
            var textBox1 = document.getElementById('<%=Password.ClientID %>');
            var textBox2 = document.getElementById('<%=ConformPassword.ClientID %>');
            if (textBox1.value == textBox2.value)
                arg.IsValid = true; //Valid Value   
            else
                arg.IsValid = false; //Invalid Value   
        }
                     
           
    </script>
    <div style="width: 970px;" class="ContentMainTra">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" align="center">
                    <tr>
                        <td width="28">
                        </td>
                        <td colspan="2">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" style="width: 50%;">
                                        <span class="TopTitle">Change Password</span>
                                    </td>
                                    <td align="right">
                                                <asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static"
                                                NavigateUrl="~/Pages/Help/Help.aspx?contentkey=ChangePasswordHelp" >
                                                <asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/Default/images/help.png"  />
                                                </asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="28">
                        </td>
                    </tr>                 
                    <tr>
                        <td width="28" rowspan="3">
                        </td>
                        <td width="945" valign="top">
                            </div>
                            <div id="content1">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 400px">
                                    <tr>
                                        <td width="615" style="padding-left: 10px" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="padding-right: 6px" valign="top">
                                                        <table border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td height="30" colspan="2">
                                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                                                        ValidationGroup="reg" ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct following errors:" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="padding-right: 5px; font-weight: bold;">
                                                                    Full Name:
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="5" colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="padding-right: 5px; font-weight: bold;">
                                                                      Old Password
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="OldPassword" CssClass="NormalTextBox" MaxLength="30" TextMode="Password"
                                                                    runat="server"  Width="300px" AutoCompleteType="Disabled"  ClientIDMode="Static" ></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RFVOldPassword" runat="server" ControlToValidate="OldPassword"
                                                                    ValidationGroup="reg" ErrorMessage="Old Password is required."  Display="None"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" height="5" align="center">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="padding-right: 5px; font-weight: bold;">
                                                                    New Password
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:TextBox ID="Password" CssClass="NormalTextBox" MaxLength="30" TextMode="Password"
                                                                        ClientIDMode="Static" runat="server" Width="300px"></asp:TextBox>
                                                                  
                                                                    <ajaxToolkit:PasswordStrength ID="PS" runat="server" TargetControlID="Password" DisplayPosition="RightSide"
                                                                        StrengthIndicatorType="Text" PreferredPasswordLength="20" PrefixText="Strength:"
                                                                        TextCssClass="TextIndicator" MinimumNumericCharacters="2" MinimumSymbolCharacters="2"
                                                                        RequiresUpperAndLowerCaseCharacters="true" MinimumLowerCaseCharacters="2" MinimumUpperCaseCharacters="1"
                                                                        TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent" TextStrengthDescriptionStyles="TextIndicator_Strength1;TextIndicator_Strength2;TextIndicator_Strength3;TextIndicator_Strength4;TextIndicator_Strength5"
                                                                        CalculationWeightings="50;15;15;20" />
                                                                    <asp:RegularExpressionValidator  ID="revPasswordLength" runat="server"
                                                                        ControlToValidate="Password" ValidationGroup="reg" ErrorMessage="Password Minimum length is 6."
                                                                        ValidationExpression=".{6,30}" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                      <asp:RequiredFieldValidator ID="RFVPassword" runat="server" ControlToValidate="Password"
                                                                        ValidationGroup="reg" ErrorMessage="New Password is required." Display="None"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" height="5" align="center">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="padding-right: 5px; font-weight: bold;">
                                                                    Confirm Password
                                                                </td>
                                                                <td align="left" height="22px">
                                                                    <asp:TextBox ID="ConformPassword" CssClass="NormalTextBox" runat="server" TextMode="Password"
                                                                        ClientIDMode="Static" Width="300px"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RfvConformPassword" runat="server" ControlToValidate="ConformPassword"
                                                                        ValidationGroup="reg" ErrorMessage="Confirm Password is required." Display="None"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td height="5" align="left">
                                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="validatePassword"
                                                                        ErrorMessage="New and Confirm Passwords need to be same!" ValidationGroup="reg"
                                                                        Display="None"></asp:CustomValidator>
                                                                    <%--<asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords do not match" ControlToCompare="ConformPassword" ControlToValidate="Password" ValidationGroup="reg" ></asp:CompareValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="lblMeg" Text="" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="padding-right: 5px">
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" height="40px">
                                                                    <%--<asp:ImageButton ID="cmdSave" runat="server" ImageUrl="~/App_Themes/Default/Images/btnSave.png"
                                                                    CausesValidation="true" OnClick="cmdSave_Click" ValidationGroup="reg" />--%>
                                                                    &nbsp;
                                                                    <%--<asp:ImageButton ID="cmdCancel" runat="server" ImageUrl="~/App_Themes/Default/Images/btnCancel.png"
                                                                    CausesValidation="false" OnClick="cmdCancel_Click" />--%>
                                                                    <div>
                                                                        <table>
                                                                            <tr>
                                                                                
                                                                                <td>
                                                                                    <div>
                                                                                        <%--<asp:LinkButton runat="server" ID="lnkCancel" OnClick="lnkCancel_Click" CssClass="btn"
                                                                                            CausesValidation="false"> <strong>Cancel</strong> </asp:LinkButton>--%>
                                                                                        <asp:HyperLink runat="server" ID="hlBack" CssClass="btn" NavigateUrl="~/Default.aspx"
                                                                                            > <strong>Cancel</strong> </asp:HyperLink>
                                                                                    </div>
                                                                                </td>

                                                                                <td>
                                                                                    <div runat="server" id="divSave">
                                                                                        <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                                                            CausesValidation="true" ValidationGroup="reg"> <strong>Save</strong> </asp:LinkButton>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td width="27" rowspan="3">
                        </td>
                        <td width="28" rowspan="3">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
