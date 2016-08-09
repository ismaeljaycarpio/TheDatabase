<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
 CodeFile="Role.aspx.cs" Inherits="Pages_Security_Role" EnableEventValidation="false"%>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
    <%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl"  TagPrefix="dbg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3" height="40">
                       
                <span  class="TopTitle">
                    Roles</span>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
               
            </td>
        </tr>
        <tr>
            <td valign="top" >
               
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                HeaderText="List of validation errors" />
                            
                            <table style="border-collapse: collapse" cellpadding="4">
                                <tr>
                                    <td align="right">
                                        <strong>Role Name</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtRoleSearch" CssClass="NormalTextBox"></asp:TextBox>
                                        <br />
                                    </td>                               
                                    <td>
                                    </td>
                                    <td>
                                        <%--<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="../../Images/iconGo.png" 
                                            onclick="btnSearch_Click" />--%>
                                            <div>
                                                 <table cellpadding="0" cellspacing="0"  >
                                                    <tr>
                                                        <td class="bL">&nbsp;</td>
                                                        <td class="bC">
                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="ButtonLink" 
                                                                onclick="lnkSearch_Click"> Go</asp:LinkButton>
                                                            
                                                            </td>
                                                        <td class="bR">&nbsp;</td>
                                                    </tr>
                                                </table>               
                                            </div>
                                    </td>
                                </tr>
                            </table>
                          
                        </div>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <table style="width:100%; text-align:center">
                                <tr>
                                    <td> <img alt="Processing..." src="../../Images/ajax.gif" /> </td>
                                </tr>
                                </table>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                       
                        
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" 
                            GridLines="Both" CssClass="gridview" 
                            AllowPaging="True" 
                            AllowSorting="True" DataKeyNames="RoleID" 
                            HeaderStyle-ForeColor="Black" Width="100%"
                            AutoGenerateColumns="false" PageSize="15" onsorting="gvTheGrid_Sorting" 
                            onprerender="gvTheGrid_PreRender" >
                            <PagerSettings Position="Top"   />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("RoleID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                                             
                                 <asp:TemplateField >
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit"  
                                        NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("RoleID").ToString())  %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>   
                                                
                                <asp:TemplateField SortExpression="Role" HeaderText="Role"  >
                                  
                                  <ItemTemplate>
                                      <asp:HyperLink ID="hlView" runat="server" 
                                     NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("RoleID").ToString())  %>' Text='<%# Eval("RoleName")%>' />

                                       
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                
                                 <asp:TemplateField SortExpression="RoleTye" HeaderText="Role Type"  >
                                   <ItemTemplate>
                                        <asp:Label ID="lblRoleTye" runat="server" Text='<%# Eval("RoleType") %>'></asp:Label>
                                    </ItemTemplate>
                                  
                                    
                                </asp:TemplateField>
                                
                                <asp:TemplateField  HeaderText="Role Notes"  >
                                   <ItemTemplate>
                                        <asp:Label ID="lblRoleNotes" runat="server" Text='<%# Eval("RoleNotes") %>'  ></asp:Label>
                                    </ItemTemplate>
                                   
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="DateAdded" SortExpression="DateAdded" HeaderText="DateAdded" DataFormatString="{0:d}" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DateUpdated" SortExpression="DateUpdated" HeaderText="DateUpdated" DataFormatString="{0:d}" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                                
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate >
                                   <asp:GridViewPager runat="server" ID="Pager" 
                                    OnExportForCSV="Pager_OnExportForCSV" 
                                OnDeleteAction="Pager_DeleteAction"  OnApplyFilter="Pager_OnApplyFilter" 
                                   OnBindTheGridToExport="Pager_BindTheGridToExport" OnBindTheGridAgain="Pager_BindTheGridAgain" 
                                 />
                            </PagerTemplate>
                            
                        </dbg:dbgGridView>
                       <br />
                        <div runat="server" id="divEmptyData" visible="false">
                        There is currently no Role. <br />
                        To add a new Role, please click  <asp:HyperLink runat="server" ID="hplNewData" >here</asp:HyperLink> 
                        
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red" ></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                    </Triggers>
                </asp:UpdatePanel>
                
                <span style="font-weight: bold" align="center"></span>
            </td>
            <td >
              
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
               
            </td>
        </tr>
    </table>
  
</asp:Content>
