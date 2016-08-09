<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InlineContentEditor.aspx.cs" 
Inherits="Extender_InlineContentEditor" ValidateRequest="false" EnableTheming="false" %>
<%@ Register     TagPrefix="editor"     Assembly="WYSIWYGEditor"   
  Namespace="InnovaStudio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>TIAR</title><meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <%--<link rel="stylesheet" rev="stylesheet" type="text/css" href="../Styles/Style.css"/>--%>
    <%--<link rel="shortcut icon" href="../Files/emd_icon.ico" />--%>
    <link href="../Editor/assetmanager/style.css" rel="stylesheet" type="text/css" />
</head>
<body >
    <form id="form1" runat="server">
    <div>
        
        <editor:WYSIWYGEditor runat="server" ID="oEditor" 
                btnStyles="true" btnSave="false" EditorHeight="450" Height="450" EditorWidth="990"
                Width="990" AssetManagerWidth="650" AssetManagerHeight="500" />
        
        <p style="text-align: center;">
            <asp:Button ID="SaveButton" Width="100px" Height="24px" 
            runat="server" Text="Save" OnClick="SaveButton_Click" />
        
        <input type="button" id="CancelButton1" value="Cancel" 
            style="width:100px; height:24px;" onclick="parent.contenteditorwindow.hide();" />
        </p>
    </div>
    </form>
</body>
</html>
