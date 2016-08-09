<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="God.aspx.cs" Inherits="God" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
    <p>
        <br />
    </p>
    <p>
    </p>
    <p>
        &nbsp;</p>
    <p>
    </p>
    <p>
         <asp:TextBox ID="txtEncrypted" runat="server" Width="501px"></asp:TextBox>
        
        
    </p>
    <p>
        <asp:Button ID="btnGetInt" runat="server" Text="Decrypt" OnClick="btnGetInt_Click" />
       
    </p>
    <p> 
          <asp:Label runat="server" ID="lblDecrypt"></asp:Label>
    </p>
          
       
    <p>
        &nbsp;</p>
   
</asp:Content>

