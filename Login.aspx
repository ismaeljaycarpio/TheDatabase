<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Open.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="Login2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <%--<script type="text/javascript" src="<%=ResolveUrl("~/swfobject.js")%>"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     <script language="javascript" type="text/javascript">
         function abc() {
             var b = document.getElementById('<%= lnkLogIn.ClientID %>');
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

        <%-- $(document).ready(function () {

             function IsFlashSupported() {
                 var hfFlashSupport = document.getElementById('<%= hfFlashSupport.ClientID %>');
                 if (swfobject.hasFlashPlayerVersion('1')) {
                     hfFlashSupport.value = 'yes';
                 }
                 else {
                     hfFlashSupport.value = 'no';
                 }
                 alert(hfFlashSupport.value);
             }
             IsFlashSupported();
             //setTimeout(function () { IsFlashSupported(); }, 1000);
         });--%>

    </script>
    <asp:Panel ID="pnlFull" runat="server" DefaultButton="lnkLogIn">
         <%--<asp:HiddenField runat="server" ID="hfFlashSupport" Value="no" />--%>
        <div runat="server" id="divFull" onkeypress="abc();">
            <div class="container">
                <asp:HiddenField runat="server" ID="hfScreenWidth" ClientIDMode="Static" />
                <div class="row contact-wrap" style="padding-top: 80px;">
                    <div class="status alert alert-success" style="display: none"></div>

                    <div class="col-sm-4 col-sm-offset-1">
                        <div class="form-group">

                            <strong style="color: #0299C6;">Email:</strong>
                            <br />
                            <asp:TextBox ID="txtLogInEmail" runat="server" CssClass="logintextbox" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVUserName" runat="server" ErrorMessage="*" ControlToValidate="txtLogInEmail"
                                ValidationGroup="Login"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <strong style="color: #0299C6;">Password:</strong>
                            <br />
                            <asp:TextBox ID="txtLogInPassword" MaxLength="30" runat="server" TextMode="Password"
                                CssClass="logintextbox" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVPassword" runat="server" ErrorMessage="*" ControlToValidate="txtLogInPassword"
                                ValidationGroup="Login"></asp:RequiredFieldValidator>

                        </div>

                        <div class="form-group">

                            <asp:LinkButton runat="server" ID="lnkLogIn" ValidationGroup="Login" CssClass="btn btn-primary btn-lg"
                                ClientIDMode="Static" CausesValidation="true" OnClick="lnkLogIn_Click" Width="300px"><strong>Sign In</strong> </asp:LinkButton>

                        </div>
                        <div class="form-group" style="vertical-align: middle; padding-top: 20px;">
                            <asp:CheckBox ID="chkRememberMe" runat="server" />
                            <strong>Remember me</strong>
                        </div>
                        <div class="form-group">
                            <asp:HyperLink runat="server" ID="hlForgotPassword" NavigateUrl="~/Security/PasswordReminder.aspx">Forgot your password? </asp:HyperLink>
                        </div>

                    </div>
                    <div class="col-sm-4 hidden-xs hidden-sm hidden-md">
                        <div class="form-group">
                            <div runat="server" id="divContentCommon">
                                <asp:Label runat="server" ID="lblContentCommon" Style="color: #0299C6;" Text="TheDatabase is a highly configurable online database that has been designed to be very easy to use.</br> </br>  Developed by DB Gurus Australia since 2012, they have provided customisation services since 2006 for over 200 clients throughout Australia and can modify TheDatabase if required."></asp:Label>
                                <br />
                                <div style="width:100%; text-align:right;">
                                    <asp:HyperLink runat="server" ID="hlContentCommonEdit" Visible="false" >
                                        <asp:Image runat="server" ImageUrl="~/App_Themes/Default/Images/iconEdit.png" AlternateText="Edit" ToolTip="Edit Login page content (only for Admin)." />
                                    </asp:HyperLink>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="SignUpDiv" runat="server" id="divSignUp" visible="false">
                                <table width="100%">
                                    <tr style="height: 75px;" runat="server" id="trCommonContentHeight">
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td align="center" runat="server" id="tdContentCommon2">
                                            <%--<strong style="font-size: 20px;">New to the TheDatabase?</strong><br />  <br />
                                                                                            <p>Get started now. It quick, easy and free!</p>
                                                                                            <br />--%>
                                            <asp:Label runat="server" ID="lblContentCommon2"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trSignUp">
                                        <td style="padding-left: 140px;">
                                            <asp:HyperLink runat="server" ID="hlSignUp" CssClass="btn" NavigateUrl="~/SignUp.aspx"
                                                Font-Size="15px" Font-Bold="true"><strong>Register</strong> </asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">
        $(document).ready(function () {

            if (window.screen.width != null) {
                document.getElementById('hfScreenWidth').value = window.screen.width;
            }
            else {
                if (window.screen.width != null) {
                    document.getElementById('hfScreenWidth').value = $(window).width();
                }
            }
        });

        //             if (window.screen.availWidth != null) {
        //                 document.getElementById('hfScreenWidth').value = window.screen.availWidth;
        //             }  $(window).width()
        //      if (window.screen.availablewidth < 1000) {
        //          window.location.href = "http://sizeForSmallRez";
        //      }
        //      else {
        //          window.location.href = "http://siteForLargeRez";
        //      }
    </script>

</asp:Content>
