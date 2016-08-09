<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileColumnLink.ascx.cs" 
    Inherits="Pages_UserControl_FileColumnLink" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div>

            <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                AllowPaging="false" AllowSorting="false" DataKeyNames="ID" HeaderStyle-ForeColor="Black"
                Width="100%" AutoGenerateColumns="false" PageSize="500" 
                OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound"
                AlternatingRowStyle-BackColor="#DCF2F0">
                <PagerSettings Position="Top" />
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="LblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="File Column">
                        <ItemTemplate>
                            <div style="padding-left: 10px;">
                                <asp:Label runat="server" ID="lblFileColumn" ></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Table Column">
                        <ItemTemplate>
                            <div style="padding-left: 10px;">
                                <asp:DropDownList runat="server" ID="ddlColumn" DataValueField="ColumnID"
                                     DataTextField="DisplayName" CssClass="NormalTextBox"></asp:DropDownList>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <HeaderStyle CssClass="gridview_header" />
                <RowStyle CssClass="gridview_row" />
                <PagerTemplate>
                    <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" HideAdd="true"
                        HideAllExport="true" HideFilter="true" HidePageSizeButton="true"
                        HideGo="true" HideNavigation="true" HideRefresh="true" HideExport="true" />
                </PagerTemplate>
            </dbg:dbgGridView>

        </div>
    </ContentTemplate>
    <%--<Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
    </Triggers>--%>
</asp:UpdatePanel>


