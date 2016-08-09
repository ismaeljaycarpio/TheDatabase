<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="MessageDetail.aspx.cs" Inherits="Pages_Record_MessageDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
     <table border="0" cellpadding="0" cellspacing="0" width="1000" align="center">
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle"></asp:Label>
                </span>
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
                <asp:Panel ID="Panel2" runat="server">
                    <div runat="server" id="divDetail">
                        <table cellpadding="3">
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td align="right" style="width:120px;">
                                                <strong>Table:</strong>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblTable"></asp:Label>
                                            </td>
                                            <td align="right" style="padding-left:10px;">
                                                <strong>Message Type:</strong>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblMessageType"></asp:Label>
                                            </td>
                                            <td align="right" style="padding-left:10px;">
                                                <strong>Delivery Method:</strong>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblDeliveryMethod"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <strong>Date and Time:</strong>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblDateandTime"></asp:Label>
                                            </td>
                                            <td align="right" style="padding-left:10px;">
                                                <strong>Direction:</strong>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblDirection"></asp:Label>
                                            </td>
                                            <td align="right"></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width:120px;">
                                    <strong>Other Party:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOtherParty"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <strong>Subject:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblSubject"  Width="850px"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 10px">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <strong>Body:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblBody" Width="700px"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 10px">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    
                                </td>
                                <td>
                                    <asp:HyperLink runat="server" ID="hlViewEmail" Target="_blank">View Email</asp:HyperLink>
                                </td>
                            </tr>
                                <tr>
                                    <td colspan="2" style="height:20px;">

                                    </td>
                                </tr>
                            <tr>
                                <td>

                                </td>
                                <td>
                                     <asp:LinkButton runat="server" ID="hlBack"  CssClass="btn" OnClientClick="parent.$.fancybox.close();return false;"> <strong>Close</strong> </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                   
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

