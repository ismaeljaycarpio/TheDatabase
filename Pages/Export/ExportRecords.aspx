<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="ExportRecords.aspx.cs" Inherits="Pages_Export_ExportRecords" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
<div style="padding:10px;">
  <%--<asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upCommon">
        <ProgressTemplate>
            <table style="width:100%;  height:100%; text-align: center;" >
                <tr valign="middle">
                    <td>
                        <p style="font-weight:bold;"> Please wait...</p>
                        <asp:Image ID="ImageProcessing" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                    </td>
                </tr>
            </table>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

      <asp:UpdatePanel ID="upCommon" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblDTitle" Font-Size="16px"
                                 Font-Bold="true" Text="Export Records"> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height:10px;"></td>
                    </tr>
                    <tr>
                        <td style="width:20px;"></td>
                        <td align="left">
                            <strong>Records</strong> <br />
                            <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="rdbRecords">
                                             <asp:ListItem Value="a" Text="All" Selected="True"></asp:ListItem>
                                             <asp:ListItem Value="f" Text="Matching Filter"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <strong>Format</strong> <br />
                            <asp:DropDownList runat="server" ID="ddlExportFiletype" CssClass="NormalTextBox">
                                    <asp:ListItem Value="e" Text="Excel" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="c" Text="CSV"></asp:ListItem>
                                    <asp:ListItem Value="w" Text="Word"></asp:ListItem>
                                    <asp:ListItem Value="p" Text="PDF"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <strong>Template</strong>
                            <br />
                            <asp:DropDownList runat="server" ID="ddlTemplate" CssClass="NormalTextBox" DataValueField="ExportTemplateID"
                                DataTextField="ExportTemplateName" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <strong>Fields</strong><br />
                            <asp:CheckBoxList Style="display: block; overflow: auto; min-width: 250px; min-height: 300px;
                                border: solid 1px black;" runat="server" ID="chklstFields" SelectionMode="Multiple"
                                DataValueField="FieldID" DataTextField="Heading">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:LinkButton runat="server" ID="lnkExport" CssClass="btn" OnClick="lnkExport_Click"
                                ValidationGroup="MKE"> <strong>Export</strong></asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkCancel" CssClass="btn" OnClientClick="parent.$.fancybox.close();return false;"
                                ValidationGroup="MKE"> <strong>Cancel</strong></asp:LinkButton>
                        </td>
                    </tr>
                     

                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
</div>

</asp:Content>

