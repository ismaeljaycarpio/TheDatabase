<%@ Page Language="C#" MasterPageFile="~/Home/Home.master" CodeFile="List.aspx.cs"
    Inherits="User_List" EnableEventValidation="false" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
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
                                        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <img alt="Processing..." src="../../Images/ajax.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
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
                            <div id="search"  onkeypress="abc();" class="searchcorner">
                                <br />
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr>
                                        <td align="right">
                                            <strong>First Name</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td style="width: 10px"/>
                                            <td align="right">
                                                <strong>Last Name</strong>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtLastName" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                                <br />
                                            </td>
                                        
                                        <td align="right">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server" visible="true">
                                        <td align="right">
                                            <strong>Email</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEmailSearch" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td style="width: 10px">
                                        </td>
                                        <td align="right">
                                            <strong> <asp:Label runat="server" ID="lblAccount" Text="Account" Visible="false" ></asp:Label>  </strong>
                                        </td>
                                       
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlAccount" DataValueField="AccountID" DataTextField="AccountName"  Visible="false"  CssClass="NormalTextBox"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlAccount_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                         <td>
                                            <div>
                                                
                                                <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> 
                                                <strong>Go</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <asp:CheckBox ID="chkIsActive" Checked="false" runat="server" AutoPostBack="true"
                                                Text="Show Deleted Records" OnCheckedChanged="chkIsActive_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                            AllowSorting="True" DataKeyNames="UserID" HeaderStyle-ForeColor="Black" Width="100%"
                            AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                             OnPreRender="gvTheGrid_PreRender"  AlternatingRowStyle-BackColor="#DCF2F0"
                            OnRowDataBound="gvTheGrid_RowDataBound">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="UserID">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("UserID").ToString())  %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="First Name" SortExpression="FirstName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("UserID").ToString())  %>'
                                            Text='<%# Eval("FirstName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Name" SortExpression="LastName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlView2" runat="server" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("UserID").ToString())  %>'
                                            Text='<%# Eval("LastName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                   <asp:TemplateField HeaderText="First Name"  Visible="false">
                                    <ItemTemplate>
                                       <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("FirstName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Last Name" Visible="false">
                                    <ItemTemplate>
                                       <asp:Label runat="server" ID="lblLastName" Text='<%# Eval("LastName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                
                                <%--<asp:BoundField DataField="PhoneNumber" SortExpression="u.PhoneNumber" HeaderText="Phone Number" />--%>
                                <asp:BoundField DataField="Email" SortExpression="u.Email" HeaderText="Email" />
                                <asp:BoundField DataField="IsAccountHolder" SortExpression="IsAccountHolder" HeaderText="Account Holder"
                                    ItemStyle-HorizontalAlign="Center" />

                                <asp:TemplateField HeaderText="User Role" SortExpression="Role" >
                                    <ItemTemplate>
                                       <asp:Label runat="server" ID="lblUserRole"  Text='<%# Eval("Role")%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LastLoginTime" SortExpression="LastLoginTime"
                                 HeaderText="Last Logged In"
                                    DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="LoginCount" SortExpression="LoginCount" 
                                HeaderText="Login Count" />      
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" OnExportForCSV="Pager_OnExportForCSV"
                                    OnDeleteAction="Pager_DeleteAction" OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain"  />
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
