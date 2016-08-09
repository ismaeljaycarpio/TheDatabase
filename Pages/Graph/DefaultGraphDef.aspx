<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="DefaultGraphDef.aspx.cs" Inherits="Pages_Graph_DefaultGraphDef" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">

        function OnClose() {
             parent.$.fancybox.close();
         }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="350px" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left">
                        <span class="TopTitle">Default Graph Definition</span>
                    </td>
                </tr>
                <tr>
                    <td><br /><br /></td>
                </tr>
                <tr>
                    <td>Default Graph Definition<br /><br /></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlGraphDefinition" runat="server"
                            DataValueField="GraphDefinitionID" DataTextField="DefinitionName" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td><br /><br /><br /><br /></td>
                </tr>
            </table>

            <div style="position: absolute; bottom: 10px; right: 25px;">
                <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn"
                    OnClick="LinkButton1_Click" >
                    <strong>OK</strong>
                </asp:LinkButton>
                <asp:LinkButton runat="server" ID="hlBack" CssClass="btn"
                    style="margin-left:0.5em;"
                    OnClientClick="OnClose(); return false;">
                    <strong>Cancel</strong>
                </asp:LinkButton>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
