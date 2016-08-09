<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="GraphMsg.aspx.cs" Inherits="GraphMsg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
 <script type="text/javascript">
     function SavedAndRefresh() {
         window.parent.document.getElementById('btnRefreshGraphOnly').click();
         parent.$.fancybox.close();

     }
    </script>

    <div >

         <h1>
            Graph Message</h1>
            <br /><br />

        <table>
           
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblMessage"></asp:Label> <br /><br />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <table>
                        <tr>
                            <td>
                                <div runat="server" id="divOk">
                                    <asp:LinkButton runat="server" ID="lnkOk" CssClass="btn" OnClick="lnkOk_Click" CausesValidation="true"> <strong>Ok</strong> </asp:LinkButton>
                                </div>
                            </td>
                            <td>
                                <div runat="server" id="divNo">
                                    <asp:LinkButton runat="server" ID="hlNo" CssClass="btn" OnClientClick="parent.$.fancybox.close();return false;"> <strong>No</strong>  </asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
