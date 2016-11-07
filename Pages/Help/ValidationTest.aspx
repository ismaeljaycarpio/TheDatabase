<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master"
 AutoEventWireup="true" CodeFile="ValidationTest.aspx.cs" 
 Inherits="Pages_Help_ValidationTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
  <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <table style="width:100%; text-align:center">
                <tr>
                    <td> <img alt="Processing..." src="../../Images/ajax.gif" /> </td>
                </tr>
                </table>
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
 
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
 <ContentTemplate>
<div>
 <br />
        <br />
    
        <br />
        <table >
       
            <tr>
                <td>
                    &nbsp;</td>
                <td style="text-align: right">
        <asp:Label ID="Label1" runat="server" Text="Validation"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtValidation" runat="server" Height="111px" TextMode="MultiLine"  
            Width="400px" CssClass="MultiLineTextBox"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td style="text-align: right">
        <asp:Label ID="Label2" runat="server" Text="Data"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtData" runat="server" Width="463px"  CssClass="NormalTextBox"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td style="text-align: right">
        <asp:Button ID="btnShowResult" runat="server"  Text="Show Result" 
                        style="text-align: right" Font-Bold="True" onclick="btnShowResult_Click" 
                        Width="109px" />
                </td>
                <td>
        <asp:Label ID="lblResult" runat="server" Font-Bold="True" Font-Size="Larger"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
         
      
        </table>
        <br />

</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

