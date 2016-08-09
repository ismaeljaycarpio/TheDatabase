<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="FancyConfirm.aspx.cs" Inherits="Pages_Help_FancyConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">

    <div>
        <asp:Label runat="server" ID="lblTopTitle" CssClass="TopTitle">Please Confirm</asp:Label>
        <br />
        <br />
        <br />
        <div style="text-align:center;">
            <table>
                <tr>
                    <td colspan="2">
                          <asp:Label runat="server" ID="lblMessage" ForeColor="Red"
            Text="Do you want to replace existing views for all user with this one?"></asp:Label>

                    </td>
                </tr>
                <tr><td colspan="2" style="height:30px;"></td></tr>
                <tr >
                    <td style="width:40%;">


                    </td>
                    <td>
                        <table>

                            <tr>
                                <td>
                                    <asp:LinkButton runat="server" ID="lnkOK" CssClass="btn" OnClientClick="CloseAndRefresh();return false;">
                                           <strong>Yes</strong>
                                    </asp:LinkButton>
                                </td>
                                <td style="padding-left:5px;">
                                    <asp:LinkButton runat="server" ID="lnkNo" CssClass="btn">
                                           <strong>No</strong>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
           
        </div>


    </div>

</asp:Content>

