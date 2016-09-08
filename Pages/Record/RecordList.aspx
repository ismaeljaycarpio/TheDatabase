<%@ Page Language="C#" MasterPageFile="~/Home/Home.master" CodeFile="RecordList.aspx.cs"
    Inherits="Record_Record_List" EnableEventValidation="false" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/RecordList.ascx" TagName="RecordList" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>
    <style type="text/css">
       

    </style>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">


    <%-- OnClick="btnReloadMe_Click"--%>
    <asp:Button ID="btnReloadMe" runat="server" ClientIDMode="Static" OnClientClick="ReloadMe();return false;"
        Style="display: none;" />


    <table>
        <tr>
            <td>
                <asp:RecordList runat="server" ID="rlOne" PageType="p" ShowAddButton="true" ShowEditButton="true" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblSummaryPageContent"></asp:Label>
            </td>
        </tr>
    </table>

</asp:Content>
