<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Open.master" AutoEventWireup="true" CodeFile="UpdateData.aspx.cs" Inherits="UpdateData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container">
        <div class="row contact-wrap" style="padding-top: 80px;">
            <div class="status alert alert-success" style="display: none"></div>
            
            <div class="col-sm-4 col-sm-offset-1">
                <div class="form-group">
                    <h2>To confirm please enter your password below:</h2>
                </div>
                <div class="form-group">

                    <strong style="color: #0299C6;">Email:</strong>
                    <br />
                    <asp:TextBox ID="txtLogInEmail" runat="server" CssClass="logintextbox" Width="300px" ReadOnly="true"></asp:TextBox>
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
                        ClientIDMode="Static" CausesValidation="true" OnClick="lnkLogIn_Click" Width="300px"><strong>Confirm</strong> </asp:LinkButton>

                </div>
                <div class="form-group" style="vertical-align: middle; padding-top: 20px;">
                    <asp:CheckBox ID="chkRememberMe" runat="server" />
                    <strong>Remember me</strong>
                </div>

            </div>
        </div>

    </div>
    <div>
        <asp:HiddenField runat="server" ID="hfRecordID" />
        <asp:HiddenField runat="server" ID="hfField1" />
        <asp:HiddenField runat="server" ID="hfValue1" />

        <asp:HiddenField runat="server" ID="hfField2" />
        <asp:HiddenField runat="server" ID="hfValue2" />


        <asp:HiddenField runat="server" ID="hfField3" />
        <asp:HiddenField runat="server" ID="hfValue3" />
    </div>
</asp:Content>

