<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="AuditDetail.aspx.cs" Inherits="Pages_Record_AuditDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div style="min-width: 450px; min-height: 500px; padding-top: 10px;">
        <h2>Change History Detail</h2>
        <asp:GridView ID="grdAuditDetail" runat="server" AutoGenerateColumns="false" CssClass="gridview"
            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
            OnRowDataBound="grdAuditDetail_RowDataBound">
            <Columns>
                <asp:BoundField DataField="DateAdded" HeaderText="Update Date" />
                <asp:BoundField DataField="User" HeaderText="User" />
                <asp:BoundField DataField="ColumnName" HeaderText="Column Name" />
                <%--<asp:BoundField DataField="OldValue" HeaderText="Old Value" />--%>
                <%--<asp:BoundField DataField="NewValue" HeaderText=" New Value" />--%>
                <asp:TemplateField Visible="true" HeaderText="Old Value">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblOldValue" runat="server" Text='<%# Eval("OldValue") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="true" HeaderText="New Value">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblNewValue" runat="server" Text='<%# Eval("NewValue") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridview_header" />
            <RowStyle CssClass="gridview_row" />
        </asp:GridView>
        <br />
        <div runat="server" visible="false">

            <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="window.close();return false;"
                CssClass="btn" CausesValidation="false"> <strong>Close</strong></asp:LinkButton>

        </div>
    </div>
</asp:Content>
