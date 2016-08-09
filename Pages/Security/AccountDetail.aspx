<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="AccountDetail.aspx.cs" Inherits="Pages_Security_AccountDetail" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        if (document.getElementById('hlChoose') != null) {
            document.getElementById('hlChoose').href = document.getElementById("hfRootURL").value + "/Pages/Site/" + "GoogleMap.aspx?type=account&lat=" + document.getElementById("ctl00_HomeContentPlaceHolder_txtLatitude").value + "&lng=" + document.getElementById("ctl00_HomeContentPlaceHolder_txtLongitude").value;
        };

        $(function () {
            $('#hlChoose').fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 800,
                height: 550,
                titleShow: false
            });
        });




        function Submit() {

            document.forms["aspnetForm"].submit()

        }


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

        function ShowHide() {

            var chk = document.getElementById("chkUseDefaultLogo");
            var x = document.getElementById("tblLogo");

            if (chk!=null && chk.checked == true) { x.style.display = 'none'; }

        }

        function EmailValidaotrEnableDisable()
        {

            try{

                var txtSMTPEmail = document.getElementById("txtSMTPEmail");
                if (txtSMTPEmail.value == '') {
                    ValidatorEnable(document.getElementById('rfvSMTPUserName'), false)
                    ValidatorEnable(document.getElementById('rfvSMTPPassword'), false)
                    ValidatorEnable(document.getElementById('rfvSMTPPort'), false)
                    ValidatorEnable(document.getElementById('rfvSMTPServer'), false)

                }
                else {
                    ValidatorEnable(document.getElementById('rfvSMTPUserName'), true)
                    ValidatorEnable(document.getElementById('rfvSMTPPassword'), true)
                    ValidatorEnable(document.getElementById('rfvSMTPPort'), true)
                    ValidatorEnable(document.getElementById('rfvSMTPServer'), true)
                }

                var txtPOP3Email = document.getElementById("txtPOP3Email");
                var chkSameAsSMTP = document.getElementById("chkSameAsSMTP");
                if (txtPOP3Email.value == '') {
                    ValidatorEnable(document.getElementById('rfvPOP3UserName'), false)
                    ValidatorEnable(document.getElementById('rfvPOP3Password'), false)
                    ValidatorEnable(document.getElementById('rfvPOP3Port'), false)
                    ValidatorEnable(document.getElementById('rfvPOP3Server'), false)

                }
                else {
                    ValidatorEnable(document.getElementById('rfvPOP3UserName'), true)
                    ValidatorEnable(document.getElementById('rfvPOP3Password'), true)
                    ValidatorEnable(document.getElementById('rfvPOP3Port'), true)
                    ValidatorEnable(document.getElementById('rfvPOP3Server'), true)
                }
                if(chkSameAsSMTP!=null && chkSameAsSMTP.checked==true)
                {
                    ValidatorEnable(document.getElementById('rfvPOP3UserName'), false)
                    ValidatorEnable(document.getElementById('rfvPOP3Password'), false)
                    ValidatorEnable(document.getElementById('revPOP3Email'), false)
                    
                }
            }
            catch(err)
            {
                //
            }

        }

        $(document).ready(function () {
            
            EmailValidaotrEnableDisable();
            

            function ShowHideEmailControls()
            {
                var chkSameAsSMTP = document.getElementById("chkSameAsSMTP");
                if (chkSameAsSMTP!=null && chkSameAsSMTP.checked == true) {

                    $("#txtPOP3Email").val($("#txtSMTPEmail").val());
                    $("#txtPOP3UserName").val($("#txtSMTPUserName").val());
                    $("#txtPOP3Password").val($("#txtSMTPPassword").val());
                    //$("#txtPOP3Port").val($("#txtSMTPPort").val());
                    //$("#txtPOP3Server").val($("#txtSMTPServer").val());
                    // $("#txtEmail2").val($("#txtEmail1").val());
                    $('#radioPOP3SSL input').val([$('#radioSMTPSSL input:checked').val()]);

                    
                    $("#trPOP3Email").fadeOut();
                    $("#trPOP3UserName").fadeOut();
                    $("#trPOP3Password").fadeOut();
                    $("#trPOP3SSL").fadeOut();

                }
                else {
                  
                    $("#trPOP3Email").fadeIn();
                    $("#trPOP3UserName").fadeIn();
                    $("#trPOP3Password").fadeIn();
                    $("#trPOP3SSL").fadeIn();
                }

                EmailValidaotrEnableDisable();
            }


            var chk = document.getElementById("chkUseDefaultLogo");
            ShowHide();
            $("#chkUseDefaultLogo").click(function () {
                if (chk.checked == true) {

                    $("#tblLogo").fadeOut();
                }
                else {

                    $("#tblLogo").fadeIn();
                }
            });

            $("#chkSameAsSMTP").click(function () {
                ShowHideEmailControls()
            });
            ShowHideEmailControls();
            //$("#chkSameAsAbove").click(function () {
            //    var chkSameAsAbove = document.getElementById("chkSameAsAbove");

            //    var txtFirstName = document.getElementById("txtFirstName");
            //    var txtLastName = document.getElementById("txtLastName");
            //    var txtPhoneNumber = document.getElementById("txtPhoneNumber");
            //    var txtEmail = document.getElementById("txtEmail");

            //    var txtBillingFirstName = document.getElementById("txtBillingFirstName");
            //    var txtBillingLastName = document.getElementById("txtBillingLastName");
            //    var txtBillingPhoneNumber = document.getElementById("txtBillingPhoneNumber");
            //    var txtBillingEmail = document.getElementById("txtBillingEmail");

            //    if (chkSameAsAbove.checked == true) {
            //        txtBillingFirstName.value = txtFirstName.value;
            //        txtBillingLastName.value = txtLastName.value;
            //        txtBillingPhoneNumber.value = txtPhoneNumber.value;
            //        txtBillingEmail.value = txtEmail.value;
            //    }
            //    if (chkSameAsAbove.checked == false) {
            //        //
            //    }
            //    //                alert('ok');
            //});

        });

        if (window.addEventListener)
            window.addEventListener("load", ShowHide, false);
        else if (window.attachEvent)
            window.attachEvent("onload", ShowHide);
        else if (document.getElementById)
            window.onload = ShowHide;

    </script>
    <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
        <div runat="server" id="div1" onkeypress="abc();">
            <table border="0" cellpadding="0" cellspacing="0"  align="center" onload="ShowHide()">
                <tr>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td colspan="3" height="40">
                        <table>
                            <tr>
                                <td style="width: 400px;">
                                    <span class="TopTitle">
                                        <asp:Label runat="server" ID="lblTitle"></asp:Label>
                                    </span>
                                </td>
                                <td></td>
                                <td>
                                    <div>
                                        <table>
                                            <tr>
                                                <td>
                                                    <div runat="server" id="divBack">
                                                        <asp:HyperLink runat="server" ID="hlBack">
                                                            <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"
                                                                ToolTip="Back" />
                                                        </asp:HyperLink>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divEdit" visible="false">
                                                        <asp:HyperLink runat="server" ID="hlEditLink">
                                                            <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/Edit_big.png"
                                                                ToolTip="Edit" />
                                                        </asp:HyperLink>
                                                    </div>
                                                </td>
                                                <td>
                                                    <%--<div runat="server" id="divDelete">
                                                        <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="javascript: return confirm('Are you sure you want to delete this Account?');"
                                                            CausesValidation="false" OnClick="lnkDelete_Click">
                                                            <asp:Image runat="server" ID="Image3" ImageUrl="~/App_Themes/Default/images/delete_big.png"
                                                                ToolTip="Delete" />
                                                        </asp:LinkButton>
                                                    </div>--%>
                                                    <div runat="server" id="divUnDelete" visible="false">
                                                        <asp:LinkButton runat="server" ID="lnkUnDelete" OnClientClick="javascript:return confirm('Are you sure you want to restore this Account?')"
                                                            CausesValidation="false" OnClick="lnkUnDelete_Click">
                                                            <asp:Image runat="server" ID="Image4" ImageUrl="~/App_Themes/Default/images/Restore_Big.png"
                                                                ToolTip="Restore" />
                                                        </asp:LinkButton>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divSave">
                                                        <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="true">
                                                            <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                                ToolTip="Save" />
                                                        </asp:LinkButton>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCheckPayment" visible="false">
                                                        <asp:HyperLink runat="server" ID="hlMakePayment" CssClass="btn"> <strong>Make Payment</strong> </asp:HyperLink>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divNext">
                                                        <asp:LinkButton runat="server" ID="lnkNext" CssClass="btn" OnClick="lnkNext_Click"
                                                            CausesValidation="true"> <strong>Next</strong> </asp:LinkButton>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divDemoEmail" visible="false">
                                                        <asp:HyperLink runat="server" ID="hlDemoEmail" CssClass="btn"> <strong>Demo Email</strong> </asp:HyperLink>
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
                <tr>
                    <td colspan="3" style="padding-left: 20px;">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                        <asp:HiddenField runat="server" ID="hfRootURL" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfCentreLat" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfCentreLong" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfFlag" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfMapZoomLevel" Value="18" ClientIDMode="Static"></asp:HiddenField>
                        <asp:HiddenField runat="server" ID="hfLat" Value="-33.873651" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfLng" Value="151.20688960000007" ClientIDMode="Static" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" CssClass="DBGTab">

                            <ajaxToolkit:TabPanel ID="tabAccount" runat="server">
                                <HeaderTemplate>
                                    <strong>Account</strong>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table style="padding-left: 10px; padding-bottom: 20px;">
                                        <tr>
                                            <td>
                                                <h2>Your Details</h2>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="margin-left:24px;">
                                                    <table>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>First Name</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox Width="300px" ID="txtFirstName" runat="server" CssClass="NormalTextBox"
                                                                    ClientIDMode="Static" BackColor="Azure"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName"
                                                                    ErrorMessage="First Name is required" Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Last Name</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox Width="300px" ID="txtLastName" runat="server" CssClass="NormalTextBox"
                                                                    ClientIDMode="Static" BackColor="Azure"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLastName"
                                                                    ErrorMessage="Last Name is required" Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="padding-right: 5px">
                                                                <strong>Phone Number</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtContactNumber" CssClass="NormalTextBox" runat="server" Text=""></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtContactNumber"
                                                                    ValidationGroup="reg" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Email</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmail" runat="server" Width="300px" CssClass="NormalTextBox"
                                                                    ClientIDMode="Static" BackColor="Azure"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                                    ErrorMessage="Email is required" Display="None"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="REVEmail" runat="server" ControlToValidate="txtEmail"
                                                                    ErrorMessage="Invalid Email" Display="None" ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trPassword">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblPassword" Text="Password*" Font-Bold="true" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPassword" Width="300px" runat="server" CssClass="NormalTextBox"
                                                                    TextMode="Password"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                                                    ErrorMessage="Password - Required"></asp:RequiredFieldValidator>
                                                                <ajaxToolkit:PasswordStrength ID="PS" runat="server" TargetControlID="txtPassword"
                                                                    DisplayPosition="RightSide" StrengthIndicatorType="Text" PreferredPasswordLength="20"
                                                                    PrefixText="Strength:" TextCssClass="TextIndicator" MinimumNumericCharacters="2"
                                                                    MinimumSymbolCharacters="2" RequiresUpperAndLowerCaseCharacters="true" MinimumLowerCaseCharacters="2"
                                                                    MinimumUpperCaseCharacters="1" TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
                                                                    TextStrengthDescriptionStyles="TextIndicator_Strength1;TextIndicator_Strength2;TextIndicator_Strength3;TextIndicator_Strength4;TextIndicator_Strength5"
                                                                    CalculationWeightings="50;15;15;20" />
                                                                <asp:RegularExpressionValidator Display="Dynamic" ID="revPasswordLength" runat="server"
                                                                    ControlToValidate="txtPassword" ValidationGroup="reg" ErrorMessage="Password Minimum length is 6."
                                                                    ValidationExpression=".{6,30}"> </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:HyperLink runat="server" ID="hlChangePassword" NavigateUrl="~/Security/ChangePassword.aspx">Change Password</asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table runat="server" id="tblBillingInfo" visible="false">
                                                    <tr>
                                                        <td>
                                                            <h2>Billing Details</h2>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="chkSameAsAbove" ClientIDMode="Static" Text="Same as above"
                                                                TextAlign="Right" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>First Name</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="300px" ID="txtBillingFirstName" runat="server" CssClass="NormalTextBox"
                                                                ClientIDMode="Static"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtBillingFirstName"
                                                                ErrorMessage="Billing First Name is required" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Last Name</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="300px" ID="txtBillingLastName" runat="server" CssClass="NormalTextBox"
                                                                ClientIDMode="Static"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtBillingLastName"
                                                                ErrorMessage="Billing Last Name is required" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Phone Number</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="300px" ID="txtBillingPhoneNumber" runat="server" CssClass="NormalTextBox"
                                                                ClientIDMode="Static"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtBillingPhoneNumber"
                                                                ErrorMessage="Billing Phone Number is required" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Billing Email</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="300px" ID="txtBillingEmail" runat="server" CssClass="NormalTextBox"
                                                                ClientIDMode="Static"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtBillingEmail"
                                                                ErrorMessage="Billing Email is required" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtBillingEmail"
                                                                ErrorMessage="Invalid Billing Email" Display="None" ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Billing Address</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="300px" ID="txtBillingAddress" runat="server" CssClass="NormalTextBox"
                                                                TextMode="MultiLine" Height="80px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtBillingAddress"
                                                                ErrorMessage="Billing Address is required" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="padding-right: 5px">
                                                            <strong>Country:</strong>
                                                        </td>
                                                        <td style="padding-left: 1px">
                                                            <asp:DropDownList runat="server" ID="ddlCountry" CssClass="NormalTextBox" AutoPostBack="false"
                                                                DataTextField="DisplayText" DataValueField="LookupDataID" Width="300px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divAccountDetail" runat="server" visible="false">
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <h2>Account Details</h2>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="width: 130px;">
                                                                <strong runat="server" id="strongAccountNameTest">Account Name*:</strong>
                                                            </td>
                                                            <td style="padding-left: 3PX;">
                                                                <asp:TextBox ID="txtAccountName" runat="server" Width="200px" CssClass="NormalTextBox"></asp:TextBox>
                                                                <asp:Label runat="server" Visible="false" ID="lblAccountName"></asp:Label>
                                                                <asp:RequiredFieldValidator ID="rfvAccountName" runat="server" ControlToValidate="txtAccountName"
                                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Account Type*:</strong>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlAccountType" CssClass="NormalTextBox" AutoPostBack="false"
                                                                                DataTextField="AccountTypeName" DataValueField="AccountTypeID">
                                                                            </asp:DropDownList>
                                                                            <asp:Label runat="server" Visible="false" ID="lblAccountType"></asp:Label>
                                                                        </td>
                                                                        <td>&nbsp;
                                                                            <asp:HyperLink runat="server" ID="hlChangeAccountType" NavigateUrl="#" Visible="false"
                                                                                ClientIDMode="Static"> <strong>Change </strong> </asp:HyperLink>
                                                                        </td>
                                                                        <td></td>
                                                                        <td>&nbsp;
                                                                            <asp:HyperLink runat="server" ID="hlRenew" NavigateUrl="#"
                                                                                Visible="false" ClientIDMode="Static"> <strong>Renew </strong> </asp:HyperLink>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Expiry Date:</strong>
                                                            </td>
                                                            <td style="padding-left: 3PX;">
                                                                <asp:TextBox runat="server" ID="txtExpiryDate" Width="100px" CssClass="NormalTextBox"
                                                                    ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                                                <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtExpiryDate"
                                                                    Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                                                </ajaxToolkit:CalendarExtender>
                                                                <asp:Label runat="server" Visible="false" ID="lblExpiryDate"></asp:Label>
                                                                <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtExpiryDate"
                                                                    ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                                    MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtExpiryDate"
                                                                    WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                                                </ajaxToolkit:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Emails Sent:</strong>

                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblEmailsSent" Text="[EmailCount] of [MaxEmailsPerMonth]"></asp:Label>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td align="right">
                                                                <strong>SMS Sent:</strong>

                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblSMSSent" Text=" [SMSCount] of [MaxSMSPerMonth]"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Total Records:</strong>

                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblTotalRecords" Text=" [RecordCount] of [MaxTotalRecords]"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trMasterPage">
                                            <td align="right">
                                                <strong>Master Page</strong>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlMasterPage" CssClass="NormalTextBox">
                                                    <asp:ListItem Text="--Default--" Value="" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="HIS" Value="~/Home/HIS.master"></asp:ListItem>
                                                    <asp:ListItem Text="RRP" Value="~/Home/RRP.master"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trIsActive" visible="false">
                                            <td align="right"></td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkIsActiveAccount" Text="Active"
                                                    TextAlign="Right" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkIsActiveAccount_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="tabOptions" runat="server" Visible="true">
                                <HeaderTemplate>
                                    <strong>Options</strong>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td valign="top" style="padding-left: 20px;">
                                                <table>

                                                    <tr runat="server" visible="false">
                                                        <td></td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong id="strong4" runat="server">Layout</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlLayout" runat="server" CssClass="NormalTextBox">
                                                                            <asp:ListItem Selected="True" Text="Default" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Alternative" Value="1"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" style="width: 120px;">
                                                                        <strong>Default Graph</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlDefaultGraph" runat="server" CssClass="NormalTextBox">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <h2>Logo</h2>
                                                        </td>

                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkUseDefaultLogo" runat="server" Checked="True" ClientIDMode="Static" />
                                                                        <strong>Use default logo</strong>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table id="tblLogo" runat="server" clientidmode="Static">
                                                                            <tr runat="server">
                                                                                <td runat="server">
                                                                                    <strong>Upload logo file: </strong>
                                                                                </td>
                                                                            </tr>
                                                                            <tr runat="server">
                                                                                <td runat="server">
                                                                                    <asp:FileUpload ID="fuPhoto" runat="server" Font-Size="11px" onchange="javascript:Submit()"
                                                                                        size="45" Style="width: 400px;" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr runat="server">
                                                                                <td runat="server">
                                                                                    <asp:Panel ID="pnlPhoto" runat="server" Width="400px">
                                                                                        <asp:Image ID="imgPhoto" runat="server" ImageUrl="~/Images/NoPhoto.jpg" />
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr><td colspan="2" style="height:20px;"></td></tr>
                                                    <tr >
                                                        <td valign="top">
                                                            <h2>Maps</h2>
                                                        </td>

                                                        <td>
                                                                <br />
                                                            <table>
                                                                <tr>
                                                                    <td align="right" >
                                                                        <%--<strong>Map Scale</strong>--%>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:HiddenField runat="server" ID="hfMapScale" Value="-1" ClientIDMode="Static" />
                                                                        <%--<asp:DropDownList ID="ddlScale" runat="server" CssClass="NormalTextBox">
                                                                            <asp:ListItem Selected="True" Text="Auto" Value="-1"></asp:ListItem>
                                                                            <asp:ListItem Text="11 – City" Value="11"></asp:ListItem>
                                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                            <asp:ListItem Text="13 - City Area" Value="13"></asp:ListItem>
                                                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                            <asp:ListItem Text="15 - Suburb" Value="15"></asp:ListItem>
                                                                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                            <asp:ListItem Text="17 - Street" Value="17"></asp:ListItem>
                                                                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                            <asp:ListItem Text="20 - House" Value="20"></asp:ListItem>
                                                                        </asp:DropDownList>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" style="width: 120px;">
                                                                        <strong>Other Map Scale</strong>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlOtherMapScale" runat="server" CssClass="NormalTextBox">
                                                                            <asp:ListItem Selected="True" Text="Auto" Value="-1"></asp:ListItem>
                                                                            <asp:ListItem Text="11 – City" Value="11"></asp:ListItem>
                                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                            <asp:ListItem Text="13 - City Area" Value="13"></asp:ListItem>
                                                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                            <asp:ListItem Text="15 - Suburb" Value="15"></asp:ListItem>
                                                                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                            <asp:ListItem Text="17 - Street" Value="17"></asp:ListItem>
                                                                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                            <asp:ListItem Text="20 - House" Value="20"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" >
                                                                        <strong runat="server" id="stgShowLocation">Show Location</strong>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlTableMap" runat="server" CssClass="NormalTextBox" DataTextField="TableName"
                                                                            DataValueField="TableID">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                   

                                                    <tr><td colspan="2" style="height:20px;"></td></tr>
                                                    <tr runat="server" id="trHomePageControls">
                                                        <td valign="top" style="padding-right:10px;">
                                                            <h2>Home Page</h2>
                                                        </td>
                                                        <td align="left">
                                                            <br />
                                                            <strong>Menu:</strong>
                                                            <asp:TextBox runat="server" ID="txtHomeMenuCaption" CssClass="NormalTextBox" Text=""> </asp:TextBox>
                                                            &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:CheckBox runat="server" ID="chkShowOpenMenu" TextAlign="Right"
                                                                Font-Bold="true" Text="Show Open Menu" />
                                                            <br />
                                                            <br />
                                                            <asp:RadioButton runat="server" ID="optDashboard" GroupName="ST" Text="Display Dashboard"
                                                                ClientIDMode="Static" Font-Bold="true" Checked="true" />
                                                            <br />
                                                            <br />
                                                            <asp:RadioButton runat="server" ID="optDisplayTable" GroupName="ST" Text="Display Table"
                                                                ClientIDMode="Static" Font-Bold="true" />
                                                            <asp:DropDownList runat="server" ID="ddlDisplayTable" CssClass="NormalTextBox" DataTextField="TableName"
                                                                DataValueField="TableID">
                                                            </asp:DropDownList>
                                                            <br />
                                                            <br />
                                                            <asp:RadioButton runat="server" ID="optHomePageLink" GroupName="ST" Text="Link"
                                                                ClientIDMode="Static" Font-Bold="true" />
                                                            &nbsp;
                                                             <asp:TextBox runat="server" ID="txtHomePageLink" CssClass="NormalTextBox"
                                                                 Text="" Width="500px"> </asp:TextBox>

                                                            <br />
                                                            <br />

                                                        </td>
                                                    </tr>


                                                     <tr><td colspan="2" style="height:20px;"></td></tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <h2>Others</h2>
                                                        </td>

                                                        <td>
                                                            <br />
                                                            <asp:CheckBox runat="server" ID="chkUseDataScope" Text="Use Data Scope" TextAlign="Right"
                                                                Font-Bold="true" />
                                                            <br />
                                                            <asp:CheckBox runat="server" ID="chkLabelOnTop" TextAlign="Right" Text="Label On Top" Font-Bold="true" />
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                              <ajaxToolkit:TabPanel ID="TabPanel1" runat="server">
                                <HeaderTemplate>
                                    <strong>Email</strong>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div style="padding: 10px;">

                                        <table>
                                            <%--<tr>
                                                <td></td>
                                                <td></td>
                                                <td align="left">
                                                   
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td valign="top">
                                                    <table>
                                                        <tr>
                                                            <td valign="top" colspan="2" align="left">
                                                                <h3>Send Email (SMTP)</h3>
                                                            </td>
                                                            
                                                        </tr>
                                                            <tr>
                                                            <td align="right">
                                                                <strong>Port</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSMTPPort" CssClass="NormalTextBox" Width="50px" ClientIDMode="Static"> </asp:TextBox>
                                                                 <asp:RequiredFieldValidator runat="server" ID="rfvSMTPPort" ClientIDMode="Static" ControlToValidate="txtSMTPPort" 
                                                                    ErrorMessage="SMTP Port - Required." Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                      
                                                       
                                                         <tr>
                                                            <td align="right">
                                                                <strong>Server</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSMTPServer" CssClass="NormalTextBox" Width="200px" ClientIDMode="Static"> </asp:TextBox>
                                                                 <asp:RequiredFieldValidator runat="server" ID="rfvSMTPServer" ClientIDMode="Static" ControlToValidate="txtSMTPServer" 
                                                                    ErrorMessage="SMTP Server - Required." Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Email</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSMTPEmail" CssClass="NormalTextBox" Width="300px" ClientIDMode="Static"> </asp:TextBox>
                                                                 <asp:RegularExpressionValidator Display="None" ID="revSMTPEmail" runat="server" ControlToValidate="txtSMTPEmail"
                                                            ErrorMessage="Invalid SMTP Email" ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                                        </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>User Name</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSMTPUserName" CssClass="NormalTextBox" Width="200px" ClientIDMode="Static"> </asp:TextBox>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvSMTPUserName" ClientIDMode="Static" ControlToValidate="txtSMTPUserName" 
                                                                    ErrorMessage="SMTP User Name - Required." Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Password</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSMTPPassword" CssClass="NormalTextBox" Width="200px" ClientIDMode="Static" TextMode="Password"> </asp:TextBox>
                                                                 <asp:RequiredFieldValidator runat="server" ID="rfvSMTPPassword" ClientIDMode="Static" ControlToValidate="txtSMTPPassword" 
                                                                    ErrorMessage="SMTP Password - Required." Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                     <tr>
                                                            <td align="right">
                                                                <strong>SSL</strong>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList runat="server" ID="radioSMTPSSL" ClientIDMode="Static" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Reply to</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSMTPReplyToEmail" CssClass="NormalTextBox" Width="300px" ClientIDMode="Static"> </asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </td>
                                                <td style="width:50px;"></td>
                                                <td valign="top">
                                                     <table>
                                                        <tr>
                                                            <td valign="top"   align="right" style="width:200px;">
                                                                <h3>Read Email (POP3)</h3>
                                                            </td>
                                                            <td  align="right">
                                                                 <asp:CheckBox runat="server" ID="chkSameAsSMTP" ClientIDMode="Static" Font-Bold="true"
                                                         TextAlign="Right" Text="Same as Send Email" />

                                                            </td>
                                                            
                                                        </tr>
                                                            <tr>
                                                            <td align="right">
                                                                <strong>Port</strong>
                                                            </td>
                                                            <td style="width:220px;">
                                                                <asp:TextBox runat="server" ID="txtPOP3Port" CssClass="NormalTextBox" Width="50px" ClientIDMode="Static"> </asp:TextBox>
                                                                 <asp:RequiredFieldValidator runat="server" ID="rfvPOP3Port" ClientIDMode="Static" ControlToValidate="txtPOP3Port" 
                                                                    ErrorMessage="POP3 Port - Required." Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                     
                                                       

                                                        <tr >
                                                            <td align="right">
                                                                <strong>Server</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtPOP3Server" CssClass="NormalTextBox" Width="200px" ClientIDMode="Static"> </asp:TextBox>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvPOP3Server" ClientIDMode="Static" ControlToValidate="txtPOP3Server" 
                                                                    ErrorMessage="POP3 Server - Required." Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPOP3Email">
                                                            <td align="right">
                                                                <strong>Email</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtPOP3Email" CssClass="NormalTextBox" Width="300px" ClientIDMode="Static"> </asp:TextBox>

                                                                <asp:RegularExpressionValidator Display="None" ID="revPOP3Email" runat="server" ControlToValidate="txtPOP3Email"
                                                            ErrorMessage="Invalid SMTP Email" ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                                        </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPOP3UserName">
                                                            <td align="right">
                                                                <strong>User Name</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtPOP3UserName" CssClass="NormalTextBox" Width="200px" ClientIDMode="Static"> </asp:TextBox>
                                                                 <asp:RequiredFieldValidator runat="server" ID="rfvPOP3UserName" ClientIDMode="Static" ControlToValidate="txtPOP3UserName" 
                                                                    ErrorMessage="POP3 User Name - Required." Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPOP3Password">
                                                            <td align="right">
                                                                <strong>Password</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtPOP3Password" CssClass="NormalTextBox" Width="200px" ClientIDMode="Static" TextMode="Password"> </asp:TextBox>
                                                                 <asp:RequiredFieldValidator runat="server" ID="rfvPOP3Password" ClientIDMode="Static" ControlToValidate="txtPOP3Password" 
                                                                    ErrorMessage="POP3 Password - Required." Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                     
                                                       <tr id="trPOP3SSL">
                                                            <td align="right">
                                                                <strong>SSL</strong>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList runat="server" ID="radioPOP3SSL" ClientIDMode="Static" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </td>

                                            </tr>

                                        </table>



                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>

                            <ajaxToolkit:TabPanel ID="tabReporting" runat="server">
                                <HeaderTemplate>
                                    <strong>Reporting</strong>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div style="padding: 10px;">
                                        <table>
                                            <tr>
                                                <td valign="top">
                                                            <h2>Reporting</h2>
                                                        </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <strong>Report Server</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtReportServer" CssClass="NormalTextBox" Width="200px" Text="localhost"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <strong>Report User</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtReportUser" CssClass="NormalTextBox" Width="200px"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <strong>Report Password</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtReportPW" CssClass="NormalTextBox" Width="200px"  TextMode="Password"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <strong>Report Server URL</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtReportServerUrl" CssClass="NormalTextBox" Width="400px" Text="http://localhost/reportserver"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkIsReportTopMenu" TextAlign="Right" Text="Report Top Menu" Visible="false" />
                                                </td>
                                            </tr>

                                        </table>
                                        <%--<h2>
                                        Payments</h2>--%>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div runat="server" id="div3" visible="false">
                                                    <dbg:dbgGridView ID="gvInvoice" runat="server" GridLines="Both" CssClass="gridview"
                                                        HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                                        AllowSorting="false" DataKeyNames="InvoiceID" HeaderStyle-ForeColor="Black" Width="100%"
                                                        AutoGenerateColumns="false" PageSize="15" OnPreRender="gvInvoice_PreRender" OnRowDataBound="gvInvoice_RowDataBound">
                                                        <PagerSettings Position="Top" />
                                                        <Columns>
                                                            <asp:TemplateField Visible="true" HeaderText="Our Ref" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceID" runat="server" Text='<%# Eval("InvoiceID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="true" HeaderText="Invoice Covers">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceCovers" runat="server" Text=""></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="true" HeaderText="Date ">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceDate", "{0:dd MMM yyyy}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="true" HeaderText="Amount (AUD)">
                                                                <ItemTemplate>
                                                                    $<asp:Label ID="lblAmountAUD" runat="server" Text=""></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="true" HeaderText="Details">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDetails" runat="server" Text=""></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="gridview_header" />
                                                        <RowStyle CssClass="gridview_row" />
                                                        <PagerTemplate>
                                                            <asp:GridViewPager runat="server" ID="Pager" HideFilter="true" HideAdd="true" HideDelete="true"
                                                                OnBindTheGridToExport="Pager_BindTheGridToExport" OnApplyFilter="Pager_OnApplyFilter"
                                                                OnBindTheGridAgain="Pager_BindTheGridAgain" OnExportForCSV="Pager_OnExportForCSV" />
                                                        </PagerTemplate>
                                                        <EmptyDataTemplate>
                                                            No payments have been made yet.
                                                        </EmptyDataTemplate>
                                                    </dbg:dbgGridView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvInvoice" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td valign="top"></td>
                    <td valign="top" width="902" valign="top">
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <div runat="server" id="divDetail">
                            <table>
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="3">
                                            <tr>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <div style="border-width: 1px; border-style: solid; padding: 5px; width: 450px;"
                                                        runat="server" visible="false">
                                                        <table style="width: 450px;">
                                                            <tr runat="server" visible="false">
                                                                <td align="right">
                                                                    <strong runat="server" id="strong1">Extension Packs</strong>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtExtension" runat="server" Width="200px" CssClass="NormalTextBox"></asp:TextBox>
                                                                    <asp:Label runat="server" Visible="false" ID="lblExtension"></asp:Label>
                                                                    <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtExtension"
                                                                        ErrorMessage="Integer value please." MaximumValue="1000000" MinimumValue="0"
                                                                        Type="Integer" />
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" visible="false">
                                                                <td align="right">
                                                                    <strong runat="server" id="strong2">Alerts</strong>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="chkAlerts" />
                                                                    <asp:Label runat="server" Visible="false" ID="lblAlerts"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" visible="false">
                                                                <td align="right">
                                                                    <strong runat="server" id="strong3">Report Gen</strong>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="chkReportGen" />
                                                                    <asp:Label runat="server" Visible="false" ID="lblReportGen"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 15px;"></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <strong>Phone Number</strong>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox Width="300px" ID="txtPhoneNumber" runat="server" CssClass="NormalTextBox"
                                                                        ClientIDMode="Static"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPhoneNumber"
                                                                        ErrorMessage="Phone Number is required" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td valign="top"></td>
                </tr>
                <tr>
                    <td colspan="3" height="13"></td>
                </tr>
            </table>
        </div>
    </asp:Panel>

</asp:Content>
