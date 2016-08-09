<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="DocumentStyleList.aspx.cs" Inherits="DocGen.DocumentTextStyle.List" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
    <%--<h2>Document section types</h2>--%>
    <div>
        <table>
            <tr>
                <td align="left" style="width:750px;">
                    <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="Document styles"></asp:Label>
                </td>
                <td>
                    <div style="padding-top:10px;">
                       
                      
                        <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="DocumentStypeEdit.aspx" ToolTip="Back">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/images/back32.png"  />
                        </asp:HyperLink>
                          &nbsp;

                           <asp:HyperLink ID="lnkNew" runat="server" NavigateUrl="DocumentStypeEdit.aspx" ToolTip="Add new style">
                            <asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/Default/images/add32.png"  />
                        </asp:HyperLink>

                    </div>
                </td>
               
            </tr>
        </table>
    </div>
    <p>
        Create new, update or delete document section types...</p>
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <p>
        <asp:GridView ID="gvDocumentTextStyle" runat="server" AutoGenerateColumns="False"
            GridLines="None" Width="100%" OnRowCommand="gvDocumentTextStyle_RowCommand" OnRowDataBound="gvDocumentTextStyle_RowDataBound"
            OnRowDeleting="gvDocumentTextStyle_RowDeleting" DataKeyNames="DocumentSectionStyleID"
            CssClass="gridview">
            <HeaderStyle CssClass="gridview_header" />
            <Columns>
                <asp:TemplateField HeaderText="Index" Visible="false">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="left" />
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlEditRecord" NavigateUrl='DocumentStypeEdit.aspx?DocumentSectionStyleID=<%# Eval("DocumentSectionStyleID")%>'
                            ToolTip="Edit">
                            <asp:Image runat="server" ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                        </asp:HyperLink>
                        &nbsp;
                        <asp:LinkButton ID="lbDelete" CommandArgument='<%# Eval("DocumentSectionStyleID")%>'
                            ToolTip="Delete" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?')"
                            runat="server">
                                 <asp:Image runat="server" ImageUrl="~/App_Themes/Default/Images/icon_delete.gif" />
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:HyperLinkField DataNavigateUrlFields="DocumentSectionStyleID" DataNavigateUrlFormatString="DocumentStypeEdit.aspx?DocumentSectionStyleID={0}"
                    DataTextField="StyleName" HeaderText="Style name" />--%>
                <asp:TemplateField HeaderText="Style name">
                    <ItemStyle HorizontalAlign="left" />
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlEditRecord2" NavigateUrl='DocumentStypeEdit.aspx?DocumentSectionStyleID=<%# Eval("DocumentSectionStyleID")%>'
                            ToolTip="Edit">
                            <%# Eval("StyleName")%>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Record">
                    <ItemTemplate>
                        <div <%# String.Format("style=\"{0}\"", Eval("StyleDefinition"))%>>
                            The quick brown fox jumps over the lazy dog</div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </p>
</asp:Content>
