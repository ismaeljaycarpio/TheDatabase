<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="RecordHistory.aspx.cs" Inherits="Pages_Record_RecordHistory" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
        <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>
    
    <%-- <asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upCommon">
        <ProgressTemplate>
            <table style="width: 100%; height: 100%; text-align: center;">
                <tr valign="middle">
                    <td>
                        <p style="font-weight: bold;">Please wait...</p>
                        <asp:Image ID="ImageProcessing" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                    </td>
                </tr>
            </table>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

    <div style="width: 100%; min-height:500px;padding:10px;">
        <asp:UpdatePanel ID="upCommon" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
               
                    <div style="float: left; width: 70%; ">
                        <br />
                      
                        <strong class="TopTitle">Change History</strong>
                        <br />  <br />
                        <dbg:dbggridview id="gvChangedLog" runat="server" gridlines="Both" cssclass="gridview"
                            headerstyle-horizontalalign="Center" rowstyle-horizontalalign="Center" allowpaging="True"
                            allowsorting="false" datakeynames="DateAdded" headerstyle-forecolor="Black" width="100%"
                            autogeneratecolumns="false" pagesize="15" onprerender="gvChangedLog_PreRender"
                            onrowdatabound="gvChangedLog_RowDataBound">
                                            <PagerSettings Position="Top" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" ID="hlView" CssClass="popuplink">
                                                        <asp:Image runat="server" ID="imgView" ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Updated Date" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="UpdateDate" runat="server" Text='<%# Eval("DateAdded") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="User">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUser" runat="server" Text='<%# Eval("User") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Changed Column List">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblColumnList" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Reason for change">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResonForChange" runat="server" Text='<%# Eval("ResonForChange") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="gridview_header" />
                                            <RowStyle CssClass="gridview_row" />
                                            <PagerTemplate>
                                                <asp:GridViewPager runat="server" ID="CL_Pager" HideFilter="true" HideAdd="true" 
                                                    HideDelete="true" OnBindTheGridToExport="CL_Pager_BindTheGridToExport" OnApplyFilter="CL_Pager_OnApplyFilter"
                                                    OnBindTheGridAgain="CL_Pager_BindTheGridAgain" OnExportForCSV="CL_Pager_OnExportForCSV" />
                                            </PagerTemplate>
                                            <EmptyDataTemplate>
                                                <div style="padding-left: 100px;">
                                                    No changes have been made yet.
                                                </div>
                                            </EmptyDataTemplate>
                                        </dbg:dbggridview>
                    </div>
                    <div runat="server" id="divLastUpdatedInfo" visible="true" style="float: right; width: 30%; margin-top: 50px; position: relative;">
                        <!--oliver: additional fields here...-->
                        <div style="width: 100%; margin-left: 60px;">
                            <div style="float: left; width: 100px; padding: 10px 0 0;"><b>Created By:</b></div>
                            <div style="float: left; width: 180px; padding: 10px 0 0;">
                                <asp:Label ID="lblCreatedBy" runat="server" Text=""></asp:Label>
                            </div>
                            <div style="clear: both;"></div>
                        </div>
                        <div style="width: 100%; margin-left: 60px;">
                            <div style="float: left; width: 100px; padding: 10px 0 0;"><b>Date Created:</b></div>
                            <div style="float: left; width: 180px; padding: 10px 0 0;">
                                <asp:Label ID="lblDateCreated" runat="server" Text=""></asp:Label>
                            </div>
                            <div style="clear: both;"></div>
                        </div>
                        <div style="width: 100%; margin-left: 60px;">
                            <div style="float: left; width: 100px; padding: 10px 0 0;">
                                <asp:Label ID="lblUpdatedByText" runat="server" Text="<b>Updated By:</b>"></asp:Label>
                            </div>
                            <div style="float: left; width: 180px; padding: 10px 0 0;">
                                <asp:Label ID="lblUpdatedBy" runat="server" Text=""></asp:Label>
                            </div>
                            <div style="clear: both;"></div>
                        </div>
                        <div style="width: 100%; margin-left: 60px;">
                            <div style="float: left; width: 100px; padding: 10px 0 0;">
                                <asp:Label ID="lblDateUpdatedText" runat="server" Text="<b>Date Updated:</b>"></asp:Label>
                            </div>
                            <div style="float: left; width: 180px; padding: 10px 0 0;">
                                <asp:Label ID="lblDateUpdated" runat="server" Text=""></asp:Label>
                            </div>
                            <div style="clear: both;"></div>
                        </div>
                    </div>
                    <div style="clear: both;"></div>
              

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

