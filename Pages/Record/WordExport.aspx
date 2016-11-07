<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="WordExport.aspx.cs" Inherits="Pages_Record_WordExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

    <div style="padding-top: 50px; padding: 20px;">
        <table>
            <tr>
                <td colspan="2">
                    <h3>Document Generation</h3>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Template:</strong>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDataRetriever" DataTextField="FileName" DataValueField="DocTemplateID"
                        AutoPostBack="false" CssClass="NormalTextBox" Width="350px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <table style="padding: 50px 0 0 85px;">
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkWordExportOK" CssClass="btn" CausesValidation="false" OnClientClick="setTimeout(function () { parent.$.fancybox.close();}, 3000);"
                                    OnClick="lnkWordExportOK_Click"> <strong>OK</strong></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkWordExportCancel" CssClass="btn" CausesValidation="false" OnClientClick="parent.$.fancybox.close();return false;"> <strong>Cancel</strong></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

    </div>
<asp:Button ID="btnAutoExportWord" runat="server" ClientIDMode="Static" OnClick="lnkWordExportOK_Click"
                Style="display: none;" />
</asp:Content>

