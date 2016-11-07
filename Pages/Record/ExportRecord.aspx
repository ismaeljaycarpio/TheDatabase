<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="ExportRecord.aspx.cs" Inherits="Pages_Record_ExportRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
   <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>

    
<%--<asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server">
    <ProgressTemplate>
        <table style="width: 100%; height: 100%; text-align: center;">
            <tr valign="middle">
                <td>
                    <p style="font-weight: bold;">
                        Please wait...
                    </p>
                    <asp:Image runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                </td>
            </tr>
        </table>
    </ProgressTemplate>
</asp:UpdateProgress>--%>

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
        <asp:Panel ID="pnlExportRecords" runat="server">
           
                <div style="padding-top: 50px; padding: 20px;">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblTitle" Font-Bold="true" Text="Export Records" CssClass="TopTitle"></asp:Label>
                                <br />
                                <br />



                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="left">
                                <strong>Records (rows) to Export</strong>
                                <br />
                                <asp:DropDownList runat="server" ID="rdbRecords" CssClass="NormalTextBox" Width="320px"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdbRecords_SelectedIndexChanged">
                                    <asp:ListItem Value="a" Text="Export All records in this table" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="f" Text="Export Records that match current filter"></asp:ListItem>
                                    <asp:ListItem Value="t" Text="Export Only records that have been ticked"></asp:ListItem>
                                    <asp:ListItem Value="d" Text="Export All records and child records"></asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-left: 50px;"></td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <strong>File Format</strong>
                                            <br />
                                            <asp:DropDownList ID="ddlExportFiletype" runat="server" AutoPostBack="false" CssClass="NormalTextBox" Width="125px">
                                                <asp:ListItem Value="e" Text="Excel"></asp:ListItem>
                                                <asp:ListItem Value="c" Text="CSV"></asp:ListItem>
                                                <asp:ListItem Value="w" Text="Word"></asp:ListItem>
                                                <asp:ListItem Value="p" Text="PDF"></asp:ListItem>
                                            </asp:DropDownList>

                                        </td>
                                        <td style="width: 10px;"></td>
                                        <td>
                                            <strong>Export Template</strong>
                                            <br />
                                            <asp:DropDownList runat="server" ID="ddlTemplate" CssClass="NormalTextBox" DataValueField="ExportTemplateID" Width="175px"
                                                AutoPostBack="true" DataTextField="ExportTemplateName" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </td>
                                        <td>
                                            <asp:HyperLink runat="server" ID="hlExportTemplate" NavigateUrl="~/Pages/Export/ExportTemplate.aspx">Edit</asp:HyperLink>
                                        </td>
                                        <td style="padding-left: 5px;">
                                            <asp:HyperLink runat="server" ID="hlExportTemplateNew" NavigateUrl="~/Pages/Export/ExportTemplate.aspx">New</asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td align="left">
                                <br />
                                <br />
                                <strong>Fields to export</strong><br />
                                <asp:CheckBoxList Style="display: block; overflow: auto; min-width: 350px; max-width: 500px; min-height: 150px; max-height: 300px; border: solid 1px black;"
                                    runat="server" ID="chklstFields" SelectionMode="Multiple">
                                </asp:CheckBoxList>

                             
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right">
                                <asp:LinkButton runat="server" ID="lnkUntickAllExport" OnClick="lnkUntickAllExport_Click">Untick All</asp:LinkButton>
                            </td>

                        </tr>

                        <tr style="height: 15px;">
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkExportRecordsCancel" CssClass="btn" CausesValidation="false"  OnClientClick="parent.$.fancybox.close();return false;" > <strong>Cancel</strong></asp:LinkButton>
                                           

                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkExportRecords" CssClass="btn" CausesValidation="false"
                                                OnClick="lnkExportRecords_Click"> <strong>Export</strong></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>


                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblMagExportRecords" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
           
        </asp:Panel>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

