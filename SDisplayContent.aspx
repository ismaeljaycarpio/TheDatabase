<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="SDisplayContent.aspx.cs" ValidateRequest="false"
    Inherits="Page_SDisplayContent" MasterPageFile="~/Home/Home.master" %>

    <%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%--<asp:Label runat="server" ID="lblContentCommon"></asp:Label>--%>
    <DBGurus:DBGContent ID="dbgContentCommon" runat="server" ConnectionName="CnString" DialogWidth="1400" DialogWidthInMozilla="1400"
                                            ContentKey="" TableName="Content" ExtenderPath="Extender/" DialogHeight="650" DialogHeightInMozilla="650"
                                            ShowInlineContentEditor="false" UseAssetManager="true" />


</asp:Content>
