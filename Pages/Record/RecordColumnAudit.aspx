<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="RecordColumnAudit.aspx.cs" Inherits="Pages_Record_ColumnAudit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div style="min-width: 450px; min-height: 500px; padding-top: 10px;">
        <h2>
            Audit Detail</h2>
        <asp:GridView ID="grdAuditDetail" runat="server" AutoGenerateColumns="false" CssClass="gridview"
            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
            <Columns>
                <asp:BoundField DataField="DateAdded" HeaderText="Update Date" />
                <asp:BoundField DataField="User" HeaderText="User" />
                <asp:BoundField DataField="FieldName" HeaderText="Column Name" />
                <asp:BoundField DataField="OldValue" HeaderText="Old Value" />
                <asp:BoundField DataField="NewValue" HeaderText=" New Value" />
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
