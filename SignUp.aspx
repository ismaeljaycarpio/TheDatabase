<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignUp.aspx.cs" Inherits="Login"
    EnableEventValidation="false" ValidateRequest="true" MasterPageFile="~/Home/Marketing2.master"
    EnableTheming="true" %>

<%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
  
    
    <div class="ContentMain" style="text-align: left;">
       
        <table border="0" cellpadding="0" cellspacing="0" align="center">
            <tr>
                <td width="29" rowspan="22">
                </td>
                <td width="943" valign="top">
                    <div id="menu">
                    </div>
                    <div id="content1">
                        <table border="0" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td>
                                    <table width="700px">
                                        <tr>
                                            <td>
                                                <h1>
                                                    1 Step <span style="color: #14A3BA;">Sign Up </span>
                                                </h1>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="margin-top: 5px;">
                                <td width="700" style="padding-left: 0px" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="10" style="background-color: Transparent;">
                                                <img src="App_Themes/Default/Images/emd_03.png" />
                                            </td>
                                            <td background="App_Themes/Default/Images/emd_04.png">
                                            </td>
                                            <td width="14">
                                                <img src="App_Themes/Default/Images/emd_05.png" />
                                            </td>
                                        </tr>
                                        <tr style="background-color: White;">
                                            <td background="App_Themes/Default/Images/emd_09.png">
                                            </td>
                                            <td valign="top">
                                                <div>
                                                    <asp:Panel ID="pnlReg" runat="server">
                                                        <div id="divReg">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="700" style="padding-left: 10px" valign="top">
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td style="padding-right: 6px" valign="top">
                                                                                    <div runat="server" id="divStep1" visible="true" >
                                                                                        <table border="0" align="left" cellpadding="0" cellspacing="10" width="100%">
                                                                                            <tr>
                                                                                                <td height="10" colspan="2">
                                                                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                                                                                        ValidationGroup="reg2" ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct following errors:" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="right" valign="middle">
                                                                                                    <strong>Your Email Address:</strong>
                                                                                                </td>
                                                                                                <td valign="middle">
                                                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="Email" CssClass="NormalTextBox" runat="server" Width="275px" Text=""
                                                                                                                            OnTextChanged="Email_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="padding-left: 3px;">
                                                                                                                        <asp:Label runat="server" ForeColor="Red" ID="lblEmailErrorMsg" Text=""></asp:Label>
                                                                                                                        <asp:RequiredFieldValidator ID="RFVEmail" runat="server" ControlToValidate="Email"
                                                                                                                            ForeColor="Red" Display="Dynamic" ValidationGroup="reg2" ErrorMessage="Email Address is required"></asp:RequiredFieldValidator>
                                                                                                                        <asp:RegularExpressionValidator ForeColor="Red" Display="Dynamic" ID="REVEmail" runat="server"
                                                                                                                            ControlToValidate="Email" ValidationGroup="reg2" ErrorMessage="Invalid Email"
                                                                                                                            ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                                                                                                        </asp:RegularExpressionValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                        <Triggers>
                                                                                                            <asp:AsyncPostBackTrigger ControlID="Email" />
                                                                                                        </Triggers>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="right">
                                                                                                    <strong>Choose a Password:</strong>
                                                                                                </td>
                                                                                                <td id="tdPassword">
                                                                                                    <asp:TextBox ID="txtRegPassword" CssClass="NormalTextBox" runat="server" TextMode="Password"
                                                                                                        Width="275px" Text=""></asp:TextBox>
                                                                                                    <cc1:PasswordStrength ID="PS" runat="server" TargetControlID="txtRegPassword" DisplayPosition="RightSide"
                                                                                                        StrengthIndicatorType="Text" PreferredPasswordLength="12" PrefixText="" TextCssClass="TextIndicator"
                                                                                                        MinimumNumericCharacters="1" MinimumSymbolCharacters="1" RequiresUpperAndLowerCaseCharacters="false"
                                                                                                        MinimumLowerCaseCharacters="1" MinimumUpperCaseCharacters="1" TextStrengthDescriptions="Too short;Weak;Good;Strong;Strong"
                                                                                                        TextStrengthDescriptionStyles="TextIndicator_Strength1;TextIndicator_Strength2;TextIndicator_Strength3;TextIndicator_Strength4;TextIndicator_Strength5"
                                                                                                        CalculationWeightings="50;15;15;20" />
                                                                                                    <asp:RegularExpressionValidator ID="revPasswordLength" runat="server" ControlToValidate="txtRegPassword"
                                                                                                        ValidationGroup="reg2" ErrorMessage="Must be at least 6 characters" ValidationExpression=".{6,30}"
                                                                                                        ForeColor="Red" Display="Dynamic">
                                                                                                    </asp:RegularExpressionValidator>
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRegPassword"
                                                                                                        ValidationGroup="reg2" ErrorMessage="Password is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="right" valign="middle">
                                                                                                    <strong>Account Name:</strong>
                                                                                                </td>
                                                                                                <td valign="middle">
                                                                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtAccount" CssClass="NormalTextBox" runat="server" Width="275px" Text=""
                                                                                                                            OnTextChanged="Account_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="padding-left: 3px;">
                                                                                                                        <asp:Label runat="server" ForeColor="Red" ID="lblAccountErrorMsg" Text=""></asp:Label>
                                                                                                                        <asp:RequiredFieldValidator ID="rfvtxtAccount" runat="server" ControlToValidate="txtAccount"
                                                                                                                            ForeColor="Red" Display="Dynamic" ValidationGroup="reg2" ErrorMessage="Account Name is required"></asp:RequiredFieldValidator>
                                                                                                                       
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                        <Triggers>
                                                                                                            <asp:AsyncPostBackTrigger ControlID="txtAccount" />
                                                                                                            
                                                                                                        </Triggers>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="right">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td style="padding-left:120px;">
                                                                                                                <div>
                                                                                                                    <asp:HyperLink runat="server"  NavigateUrl="~/Login.aspx"  CssClass="btn" > <strong>Cancel</strong> </asp:HyperLink>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                            <td style="padding-left:1px;">
                                                                                                                <div>
                                                                                                                    <asp:LinkButton runat="server" ID="lnkNext1" ValidationGroup="reg2" CssClass="btn"
                                                                                                                        ClientIDMode="Static" CausesValidation="true" OnClick="lnkNext1_Click"> <strong>Sign Up</strong> </asp:LinkButton>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>

                                                                                             <tr>
                                                                                                <td  colspan="2" style="padding-left:40px;">
                                                                                               
                                                                                                    By clicking Sign Up you agree to our <a href="ETS-Terms-Of-Service.pdf" target="_blank">
                                                                                                        Terms of Service</a> and <a href="ETS-Privacy-Policy.pdf" target="_blank">Privacy Policy</a>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr>
                                                                                                <td colspan="2" height="5" align="center">
                                                                                                    <asp:Label ID="LblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
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
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                            <td background="App_Themes/Default/Images/emd_11.png">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td background="App_Themes/Default/Images/emd_09.png">
                                            </td>
                                            <td>
                                                <%--<img src="App_Themes/Default/Images/spacer.gif" height="5" />--%>
                                            </td>
                                            <td background="App_Themes/Default/Images/emd_11.png">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <img src="App_Themes/Default/Images/emd_12.png" />
                                            </td>
                                            <td background="App_Themes/Default/Images/emd_13.png">
                                            </td>
                                            <td>
                                                <img src="App_Themes/Default/Images/emd_14.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td width="28">
                </td>
            </tr>
        </table>
    </div>
   
</asp:Content>
