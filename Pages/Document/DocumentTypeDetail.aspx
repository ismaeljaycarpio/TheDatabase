<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="DocumentTypeDetail.aspx.cs" Inherits="Pages_Document_DocumentTypeDetail" %>

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
                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <strong>Document Type*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDocumentType" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDocumentType" runat="server" ControlToValidate="txtDocumentType"
                                                ErrorMessage="Document Type - Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                            <%--<asp:ImageButton ID="cmdSave" runat="server" ImageUrl="~/App_Themes/Default/Images/btnSave.png"
                                CausesValidation="true" onclick="cmdSave_Click"  />--%>
                            &nbsp;
                            <%--<asp:ImageButton ID="cmdBack" runat="server" ImageUrl="~/App_Themes/Default/Images/btnBack.png"
                                CausesValidation="false" onclick="cmdBack_Click"  />--%>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div runat="server" id="divSave">
                                               
                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                    CausesValidation="true"> <strong>Save </strong> </asp:LinkButton>
                                                      
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                               
                                                    <asp:HyperLink runat="server" ID="hlBack"  
                                                    CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                                      
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false" style="width:75px;">
                                                
                                                    <asp:HyperLink runat="server" ID="hlEditLink"
                                                     CssClass="btn"><strong>Edit</strong> </asp:HyperLink>
                                                      
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divDelete" visible="false" style="width:75px;">
                                                
                                                <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="javascript:return confirm('Are you sure you want to delete this Document Type?')"
                                                    CssClass="btn" CausesValidation="false" OnClick="lnkDelete_Click"  ><strong>Delete</strong> </asp:LinkButton>
                                                     
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
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
