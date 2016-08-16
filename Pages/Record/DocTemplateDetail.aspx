<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="DocTemplateDetail.aspx.cs" Inherits="Pages_Record_DocTemplateDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div>
        <table>
            <tr>
                <td colspan="2" style="height: 50px;">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblDetailTitle" Font-Size="16px" Font-Bold="true" Text="Template"> </asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 150px;" valign="top">
                    <strong>Word Document:</strong>
                </td>
                <td>
                    <asp:FileUpload ID="fuWordDocument" runat="server" Style="width: 300px;" size="70"
                        Font-Size="11px" />
                        <br />
                    
                       <asp:Label runat="server" ID="Label1" Font-Size="Smaller" Text="Please select a .docx or .dotx (Word 2007 or later)"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblWordDocument"></asp:Label>
                    <%--<asp:RequiredFieldValidator ID="rfvWordDocument" runat="server" ControlToValidate="fuWordDocument"
                        ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr align="left">
                <td colspan="2" align="left">
                    <div>
                        <div >
                            <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <table style="width: 100%; text-align: center">
                                        <tr>
                                            <td>
                                                <asp:Image ID="Image2" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                                            </td>
                                        </tr>
                                    </table>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td align="right" style="width:150px;">
                                            <strong>Data Retriever:</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlDataRetriever" DataTextField="DataRetrieverName" DataValueField="DataRetrieverID"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDataRetriever_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvDataRetriever" runat="server" ControlToValidate="ddlDataRetriever"
                                                ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-left: 10px;">
                                            Below is a list of data fields you can put into your Word Document 
                                            – note that you must include these symbols « »!
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-left: 10px;">
                                            <br />
                                            <asp:Label runat="server" ID="lblTokens"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="left">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div runat="server" id="div1" style="padding-left: 10px;">
                                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" CausesValidation="true"
                                        OnClick="lnkSave_Click"> <strong>Save</strong></asp:LinkButton>
                                </div>
                            </td>
                            <td>
                                <div runat="server" id="div2" style="padding-left: 10px;">
                                    <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                        CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        function CloseAndRefresh() {
            window.parent.document.getElementById('btnRefreshTemplates').click();
            parent.$.fancybox.close();
            // alert('ok');
        }
    
    </script>
</asp:Content>
