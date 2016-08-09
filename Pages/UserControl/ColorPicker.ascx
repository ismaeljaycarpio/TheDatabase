<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ColorPicker.ascx.cs" Inherits="Pages_UserControl_ColorPicker" %>
 
<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
    type="text/css" />
<script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>

<div>
    <table>
        <tr>
            <td>
                <div id="divpreviewtxtfinal" style="float: left; width: 50px; border:1px; border-style:solid; min-height:16px;min-width:65px;
                 padding-left: 5px; padding-top: 5px;" runat="server" clientidmode="Static">
                            </div>
                                <%--<asp:TextBox runat="server" ID="txtColorValue" CssClass="NormalTextBox" Width="70" ClientIDMode="Static"></asp:TextBox>--%>
                            <asp:HiddenField runat="server" ID="hfColorValue" ClientIDMode="Static"  Value=""/>
            </td>
            <td>
                <asp:HyperLink runat="server" ID="hlColorSelect" ClientIDMode="Static" NavigateUrl="~/Pages/UserControl/ColorPicker.aspx" >Select...</asp:HyperLink>
                <asp:Button runat="server" ID="btnPostback" ClientIDMode="Static" CausesValidation="false"
                    OnClick="btnPostback_Click" style="visibility:hidden; display:none;" />
            </td>
        </tr>
    </table>
</div>
