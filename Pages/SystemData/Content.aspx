<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="Content.aspx.cs" Inherits="Pages_SystemData_Content" EnableEventValidation="false" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function abc() {
            var b = document.getElementById('<%= lnkSearch.ClientID %>');
            if (b && typeof (b.click) == 'undefined') {
                b.click = function () {
                    var result = true;
                    if (b.onclick) result = b.onclick();
                    if (typeof (result) == 'undefined' || result) {
                        eval(b.getAttribute('href'));
                    }
                }
            }

        }
    </script>

     <script type="text/javascript">
         function MouseEvents(objRef, evt) {
             if (evt.type == "mouseover") {
                 objRef.style.backgroundColor = "#76BAF2";
                 objRef.style.cursor = 'pointer';
             }
             else {

                 if (evt.type == "mouseout") {
                     if (objRef.rowIndex % 2 == 0) {
                         //Alternating Row Color
                         objRef.style.backgroundColor = "white";
                     }
                     else {
                         objRef.style.backgroundColor = "#DCF2F0";
                     }
                 }
             }
         }



    </script>




    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <asp:Label runat="server" ID="lblAdminArea" CssClass="TopTitle" Text="Admin Area:"></asp:Label>
                                    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlAdminArea" CssClass="TopTitle"
                                        OnSelectedIndexChanged="ddlAdminArea_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <img alt="Processing..." src="../../Images/ajax.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>--%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                    </td>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                            <div id="search"  class="searchcorner"  onkeypress="abc();">
                                <br />
                                <table cellpadding="4">
                                    <tr>
                                        <td align="right">
                                            <strong>Search</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtContentKey" CssClass="NormalTextBox" Width="350px"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong> </asp:LinkButton>
                                            </div>
                                        </td>
                                        <td style="width:20px;">
                                        </td>
                                        <td align="right">
                                            <strong>Content Type</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlContentType" DataTextField="ContentTypeName"
                                             DataValueField="ContentTypeID"  AutoPostBack="true" CssClass="NormalTextBox"
                                                onselectedindexchanged="ddlContentType_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlAccount" DataTextField="AccountName"
                                             DataValueField="AccountID"  AutoPostBack="true" CssClass="NormalTextBox"
                                                onselectedindexchanged="ddlAccount_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkOnlyTemplate" AutoPostBack="true" OnCheckedChanged="chkOnlyTemplate_CheckedChanged"
                                                Text="Template" TextAlign="Left"  Visible="false"/>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkOnlyGlobal" AutoPostBack="true" OnCheckedChanged="chkOnlyGlobal_CheckedChanged"
                                                Text="Only Global" TextAlign="Left"  Visible="false"/>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            AllowPaging="True" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            AllowSorting="True" DataKeyNames="ContentID" HeaderStyle-ForeColor="Black" Width="100%"
                            AutoGenerateColumns="false" PageSize="15" OnRowDataBound="gvTheGrid_RowDataBound" 
                            OnSorting="gvTheGrid_Sorting" OnPreRender="gvTheGrid_PreRender" 
                             AlternatingRowStyle-BackColor="#DCF2F0">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("ContentID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("ContentID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ContentKey" HeaderText="Content Key">
                                    <ItemTemplate>
                                        <asp:Label ID="lblView" runat="server" Text='<%# Eval("ContentKey") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Heading" HeaderText="Heading">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHeading" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="StoredProcedure" HeaderText="SP">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStoredProcedure" runat="server" Text='<%# Eval("StoredProcedure") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Template" SortExpression="ForAllAccount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblForAllAccount" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Only Global">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOnlyGlobal" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                <asp:BoundField DataField="DateAdded" SortExpression="DateAdded" HeaderText="Date Added"
                                    DataFormatString="{0:d}" ReadOnly="true" />
                                <asp:BoundField DataField="DateUpdated" SortExpression="DateUpdated" HeaderText="Date Updated"
                                    DataFormatString="{0:d}" ReadOnly="true" />

                                  <asp:TemplateField  HeaderText="Content Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContentTypeName" runat="server" Text='<%# Eval("ContentTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" OnApplyFilter="Pager_OnApplyFilter"
                                    OnExportForCSV="Pager_OnExportForCSV" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                            <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                    Add new record now.</strong>
                            </asp:HyperLink>
                        </div>
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                    Clear filter</strong>
                            </asp:LinkButton>
                            or
                            <asp:HyperLink runat="server" ID="hplNewDataFilter" Style="text-decoration: none;
                                color: Black;">                                
                                  <strong style="text-decoration: underline; color: Blue;">
                                     Add new record.</strong>
                            </asp:HyperLink>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
