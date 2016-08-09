<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    EnableTheming="true" CodeFile="LookUpDetail.aspx.cs" Inherits="Pages_LookUp_LookUpDetail"
    ValidateRequest="false" %>

<%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>
<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
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
            <td valign="top">
            </td>
            <td valign="top">
                <div id="search" style="padding-bottom: 10px">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                        ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                </div>
                <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                   
                        <div runat="server" id="divDetail">
                            <table cellpadding="3">
                                <tr>
                                    <td align="right">
                                        <asp:Label runat="server" ID="lblDisplayText" CssClass="NormalTextBox" Font-Bold="true"
                                            Text="Display Text*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDisplayText" runat="server" Width="300px" CssClass="roundedtextbox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDisplayText" runat="server" ControlToValidate="txtDisplayText"
                                            ErrorMessage=" Required" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label runat="server" ID="lblValue" CssClass="NormalTextBox" Font-Bold="true"
                                            Text="Value*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValue" runat="server" Width="300px" CssClass="roundedtextbox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="txtValue"
                                            ErrorMessage=" Required" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White"></asp:Label>
                        <br />
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <div runat="server" id="divSave">
                                           
                                           
                                                        <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                            CausesValidation="true"> <strong>Save</strong></asp:LinkButton>
                                                  
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            
                                            
                                                        <asp:HyperLink runat="server" ID="hlBack"  CssClass="ButtonLink"> <strong>Back</strong> </asp:HyperLink>
                                                   
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    
                </asp:Panel>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
    </table>
</asp:Content>
