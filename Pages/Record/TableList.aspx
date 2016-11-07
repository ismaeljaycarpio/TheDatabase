<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TableList.aspx.cs" Inherits="Pages_Record_TableList" EnableEventValidation="false" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
   <%--<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>

     <script type="text/javascript">
         function MouseEvents(objRef, evt) {
             var checkbox = objRef.getElementsByTagName("input")[0];



             if (evt.type == "mouseover") {
                 objRef.style.backgroundColor = "#76BAF2";
                 objRef.style.cursor = 'pointer';
             }
             else {
                
                 if (checkbox != null && checkbox.checked) {
                     objRef.style.backgroundColor = "#96FFFF";
                 }
                 else if (evt.type == "mouseout") {
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
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
                                        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" >
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
                                <td align="right">

                                       <asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static"
                                                NavigateUrl="~/Pages/Help/Help.aspx?contentkey=TableHelp" >
                                                <asp:Image ID="Image2" runat="server"  ImageUrl="~/App_Themes/Default/images/help.png"  />
                                                </asp:HyperLink>
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
                            <div id="search" class="searchcorner" onkeypress="abc();">
                            <br />
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                    HeaderText="List of validation errors" />
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr id="trRecordGroup" runat="server" visible="true">
                                        <%--<td align="right">
                                            <strong>Menu</strong>
                                        </td>--%>
                                        <%--<td>
                                            <asp:DropDownList ID="ddlRecordGroupFilter" runat="server" AutoPostBack="true" DataTextField="MenuP"
                                                DataValueField="MenuID" OnSelectedIndexChanged="ddlRecordGroupFilter_SelectedIndexChanged"
                                                CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                            <asp:HyperLink runat="server" ID="hlMenuEdit" Text="Edit" NavigateUrl="~/Pages/Record/TableGroup.aspx"
                                                CssClass="NormalTextBox"></asp:HyperLink>
                                        </td>--%>
                                        <%--<td>
                                        </td>--%>
                                        <td align="right">
                                            <strong runat="server" id="stgTable">Table</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtTableSearch" CssClass="NormalTextBox" Width="250"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <div>
                                                
                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" 
                                                            OnClick="lnkSearch_Click"> <strong>Go</strong></asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td style="width: 20px">
                                        </td>
                                        <td>
                                            <%--<asp:HyperLink runat="server" ID="hlCopyTable" NavigateUrl="~/Pages/Template/TableList.aspx">Copy Tables</asp:HyperLink>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <asp:CheckBox ID="chkIsActive" Checked="false" runat="server" AutoPostBack="true"
                                                Text="Show Deleted Records" OnCheckedChanged="chkIsActive_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                            AllowSorting="True" DataKeyNames="TableID" HeaderStyle-ForeColor="Black"
                            Width="100%" AutoGenerateColumns="false" PageSize="100" OnSorting="gvTheGrid_Sorting"
                            OnPreRender="gvTheGrid_PreRender" OnRowCommand="gvTheGrid_RowCommand" 
                            OnRowDataBound="gvTheGrid_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("TableID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Configure" NavigateUrl='<%# GetEditURL() +  Cryptography.Encrypt(Eval("TableID").ToString())  + "#topline"  %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                
                                 <asp:TemplateField HeaderText="Map Pin" Visible="false">
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemTemplate>

                                        <asp:Image runat="server" ID="imgMapPin" ImageUrl='<%# GetRootURL() + (Eval("PinImage").ToString()==""?"Pages/Record/PINImages/DefaultPin.png":Eval("PinImage").ToString()) %>' />
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField SortExpression="TableName" HeaderText="Table">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("TableID").ToString())  %>'
                                            Text='<%# Eval("TableName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField  HeaderText="Menu" Visible="false">
                                  
                                    <ItemTemplate>
                                        <asp:Label ID="lblMenu" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="Menu" HeaderText="Menu" SortExpression="Menu" />--%>
                                <%--<asp:BoundField DataField="IsImportPositional" HeaderText="Positional" />--%>
                                <%--<asp:BoundField DataField="UploadEmail" HeaderText=" Upload Email" />--%>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" TableName="Table"
                                    AddURL="<%# GetAddURL() %>" OnExportForCSV="Pager_OnExportForCSV" OnApplyFilter="Pager_OnApplyFilter"
                                    OnBindTheGridToExport="Pager_BindTheGridToExport" OnBindTheGridAgain="Pager_BindTheGridAgain" />
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
                              <span runat="server" id="spnOr">or</span>
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
