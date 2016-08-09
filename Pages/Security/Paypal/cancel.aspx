<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cancel.aspx.cs" Inherits="paypal_cancel"
    MasterPageFile="~/Home/Marketing2.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <table class="ContentMain" style="width: 970px;">
        <tr>
            <td width="28">
            </td>
            <td>
                <h1>
                    <asp:Label runat="server" ID="lblHeading"></asp:Label>
                </h1>
            </td>
            <td width="28">
            </td>
        </tr>
        <tr>
            <td width="28">
            </td>
            <td>
                <div>
                    <div style="text-align: right; width: 100%;">
                        <table style="text-align: right; width: 100%;">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblContentCommon"></asp:Label>
                                    <%--<DBGurus:DBGContent ID="dbgContentCommon" runat="server" ConnectionName="CnString"
                                        ContentKey="Payment_cancel" TableName="Content" ExtenderPath="Extender/" ShowInlineContentEditor="false"
                                        UseAssetManager="true" />--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
            <td width="28">
            </td>
        </tr>
        <tr>
            <td width="28">
            </td>
            <td align="center" height="10px">
            </td>
            <td width="28">
            </td>
        </tr>
        <tr>
            <td width="28">
            </td>
            <td align="left">
               <div>
                                                <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                            </div>
            </td>
            <td width="28">
            </td>
        </tr>
        <tr>
            <td width="28">
            </td>
            <td align="center" height="40px">
            </td>
            <td width="28">
            </td>
        </tr>
    </table>
</asp:Content>
