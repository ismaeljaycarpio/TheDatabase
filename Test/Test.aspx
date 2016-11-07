<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test_Test" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <form id="form1" runat="server">



        <br />

        <asp:Label runat="server" ID="lblMsg"></asp:Label>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Create Excel large file" />

         <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Go to gmail account." />

        <br />
        <a target="_blank"></a>


    </form>
   
</body>
</html>
