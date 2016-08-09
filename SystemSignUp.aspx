<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SystemSignUp.aspx.cs" Inherits="SystemSignUp"
    EnableEventValidation="false" ValidateRequest="true" MasterPageFile="~/Home/Home.master"
    EnableTheming="true" %>

<%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
      

        function regenter() {
            var b = document.getElementById('<%= lnkNext.ClientID %>');
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
    <div class="ContentMain" style="text-align: left; width:970px;" >
        <table border="0" cellpadding="0" cellspacing="0" align="center">
            <tr>
                <td width="29" rowspan="22">
                </td>
                <td width="943" valign="top">
                    <div id="menu">
                    </div>
                    <div id="content1">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="3" height="66" valign="middle" class="LogInTopTitle">
                                    The Database
                                </td>
                            </tr>
                            <tr style="margin-top: 5px;">
                                <td width="585" style="padding-left: 0px" valign="top">
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
                                                <div style="text-align: right; width: 100%;">
                                                  
                                                </div>
                                                <div>
                                                    <asp:Panel ID="pnlReg" runat="server" DefaultButton="lnkNext">
                                                        <div id="divReg" onkeypress="regenter();">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="700" style="padding-left: 10px" valign="top">
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td style="padding-right: 6px" valign="top">
                                                                                    <table border="0" align="left" cellpadding="0" cellspacing="10" width="100%">
                                                                                        <tr>
                                                                                            <td class="SingUpTitle" colspan="2">
                                                                                                Account Sign Up Wizard
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td height="10" colspan="2">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <strong>Account Type:</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlAccountType" CssClass="NormalTextBox" AutoPostBack="false"
                                                                                                    DataTextField="AccountTypeName" DataValueField="AccountTypeID">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="right" style="padding-right: 5px; width: 125px;">
                                                                                                <strong>Account Name:</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtAccount"  CssClass="NormalTextBox" runat="server" Width="275px"
                                                                                                    Text=""></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAccount"
                                                                                                    ValidationGroup="reg" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="right" style="padding-right: 5px">
                                                                                                <strong>First Name:</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="FirstName"  CssClass="NormalTextBox" runat="server" Width="275px"
                                                                                                    Text=""></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RFVFirstName" runat="server" ControlToValidate="FirstName"
                                                                                                    ValidationGroup="reg" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="right" style="padding-right: 5px">
                                                                                                <strong>Last Name:</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="LastName"  CssClass="NormalTextBox" runat="server" Width="275px"
                                                                                                    Text=""></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RFVLastName" runat="server" ControlToValidate="LastName"
                                                                                                    ValidationGroup="reg" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>

                                                                                         <tr >
                                                                                            <td align="right" style="padding-right: 5px">
                                                                                                <strong>Country:</strong>
                                                                                            </td>
                                                                                            <td style="padding-left: 1px">
                                                                                                <asp:DropDownList runat="server" ID="ddlCountry" CssClass="NormalTextBox" AutoPostBack="false"
                                                                                                   DataTextField="DisplayText" DataValueField="LookupDataID">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>

                                                                                         <tr>
                                                                                            <td align="right" style="padding-right: 5px">
                                                                                                <strong>Contact Number:</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtContactNumber"  CssClass="NormalTextBox" runat="server" Width="275px"
                                                                                                    Text=""></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtContactNumber"
                                                                                                    ValidationGroup="reg" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>


                                                                                        <tr>
                                                                                            <td align="right" valign="middle" style="padding-right: 5px;">
                                                                                                <strong>Email Address:</strong>
                                                                                            </td>
                                                                                            <td valign="middle">
                                                                                                <asp:TextBox ID="Email"  CssClass="NormalTextBox" runat="server" Width="275px"
                                                                                                    Text=""></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RFVEmail" runat="server" ControlToValidate="Email"
                                                                                                    ValidationGroup="reg" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator Display="Dynamic" ID="REVEmail" runat="server" ControlToValidate="Email"
                                                                                                    ValidationGroup="reg" ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                                                                                </asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="right" style="padding-right: 5px">
                                                                                                <strong>Password:</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtRegPassword"  CssClass="NormalTextBox" runat="server" Width="275px" TextMode="Password"
                                                                                                     Text="" ></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRegPassword"
                                                                                                    ValidationGroup="reg" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                                                <cc1:PasswordStrength ID="PS" runat="server" TargetControlID="txtRegPassword" DisplayPosition="RightSide"
                                                                                                    StrengthIndicatorType="Text" PreferredPasswordLength="12" PrefixText=""
                                                                                                     TextCssClass="TextIndicator" MinimumNumericCharacters="1" MinimumSymbolCharacters="1" 
                                                                                                    RequiresUpperAndLowerCaseCharacters="false" MinimumLowerCaseCharacters="1" MinimumUpperCaseCharacters="1"
                                                                                                    TextStrengthDescriptions="Too short;Weak;Good;Strong;Strong" 
                                                                                                    TextStrengthDescriptionStyles="TextIndicator_Strength1;TextIndicator_Strength2;TextIndicator_Strength3;TextIndicator_Strength4;TextIndicator_Strength5"
                                                                                                    CalculationWeightings="50;15;15;20" />

                                                                                                     <asp:RegularExpressionValidator Display="Dynamic" ID="revPasswordLength" runat="server" ControlToValidate="txtRegPassword"
                                                                                                    ValidationGroup="reg" ErrorMessage="Password Minimum length is 6." 
                                                                                                    ValidationExpression=".{6,30}">
                                                                                                </asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                       
                                                                                       
                                                                                        <tr>
                                                                                            <td colspan="2" height="5">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="right" style="padding-right: 5px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td align="left" height="40px">
                                                                                               
                                                                                                <div>
                                                                                                    
                                                                                                                <asp:LinkButton runat="server" ID="lnkNext" ValidationGroup="reg" CssClass="btn" 
                                                                                                                 ClientIDMode="Static" CausesValidation="true" OnClick="lnkNext_Click"> <strong>Next</strong> </asp:LinkButton>
                                                                                                         
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2" height="5">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2" height="5" align="center">
                                                                                                <asp:Label ID="LblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
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
                                                <img src="App_Themes/Default/Images/spacer.gif" height="67" />
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
                                <td style="padding-right: 6px" valign="top">
                                     <asp:Label runat="server" ID="lblError" ForeColor="Red" Visible="false"></asp:Label>
                                    
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
