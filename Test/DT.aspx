<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="DT.aspx.cs" Inherits="Test_DT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div style="padding: 20px;">
        <asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server">
            <ProgressTemplate>
                <table style="width: 100%; height: 100%; text-align: center;">
                    <tr valign="middle">
                        <td>
                            <p style="font-weight: bold;">Please wait...</p>
                            <asp:Image ID="Image5" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                        </td>
                    </tr>
                </table>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upDetailDynamic" runat="server" UpdateMode="Conditional">
            <ContentTemplate>


                <table>
                    <tr>
                        <td align="right">
                            <strong>Format</strong>
                        </td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddlFormat">
                                <asp:ListItem Value="" Text="dd/mm/yyyy"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <strong>Any datetime string</strong>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtDTString" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">
                            <asp:Button runat="server" ID="btnConvert" Text="Convert to C# DateTime object" OnClick="btnConvert_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">
                            <asp:Label runat="server" ID="lblResult" Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

