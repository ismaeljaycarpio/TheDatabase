<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="EditableGridTest.aspx.cs" Inherits="Test_EditableGridTest" %>
<%@ Register Src="~/Pages/UserControl/ControlByColumn.ascx" TagName="ControlByColumn" TagPrefix="dbg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">

      <asp:UpdatePanel ID="upCalendarColour" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div style="padding-left: 50px; padding-top: 10px;">
                                        <asp:GridView ID="grdCalendarColor" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                            CssClass="gridview" OnRowCommand="grdCalendarColor_RowCommand" OnRowDataBound="grdCalendarColor_RowDataBound"
                                             ShowHeaderWhenEmpty="true"
                                            ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblID" Text='<%# Eval("ID") %>' ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnMinus" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png"
                                                            CommandName="minus"  CommandArgument='<%# Eval("ID") %>'   />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              
                                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                     <HeaderTemplate>
                                                         <strong>Field and Value</strong>
                                                     </HeaderTemplate>
                                                    
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                       
                                                            <dbg:ControlByColumn runat="server" ID="cbcColour" OnddlYAxis_Changed="CBCColorYAxisChanged" />
                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                     <HeaderTemplate>
                                                         <strong>Colour</strong>
                                                     </HeaderTemplate>
                                                    
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>                                                       
                                                            <asp:DropDownList runat="server" ID="ddlTextColour" CssClass="NormalTextBox">
                                                                <asp:ListItem Value="000000" Text="Black"></asp:ListItem>
                                                                <asp:ListItem Value="FFFFFF" Text="White"></asp:ListItem>
                                                                <asp:ListItem Value="C0C0C0" Text="Silver"></asp:ListItem>
                                                                <asp:ListItem Value="808080" Text="Gray"></asp:ListItem>
                                                                <asp:ListItem Value="FF0000" Text="Red"></asp:ListItem>
                                                                <asp:ListItem Value="800000" Text="Maroon"></asp:ListItem>
                                                                <asp:ListItem Value="FFFF00" Text="Yellow"></asp:ListItem>
                                                                <asp:ListItem Value="808000" Text="Olive"></asp:ListItem>
                                                                <asp:ListItem Value="00FF00" Text="Lime"></asp:ListItem>
                                                                <asp:ListItem Value="008000" Text="Green"></asp:ListItem>
                                                                <asp:ListItem Value="00FFFF" Text="Aqua"></asp:ListItem>
                                                                <asp:ListItem Value="008080" Text="Teal"></asp:ListItem>
                                                                <asp:ListItem Value="0000FF" Text="Blue"></asp:ListItem>
                                                                <asp:ListItem Value="000080" Text="Navy"></asp:ListItem>
                                                                <asp:ListItem Value="FF00FF" Text="Fuchsia"></asp:ListItem>
                                                                <asp:ListItem Value="800080" Text="Purple"></asp:ListItem>
                                                              </asp:DropDownList>                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnPlus" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png"
                                                            CommandName="plus" Visible="false"   CommandArgument='<%# Eval("ID") %>'  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                            </Columns>
                                            <HeaderStyle CssClass="gridview_header" Height="25px" />
                                            <RowStyle CssClass="gridview_row_NoPadding" />
                                        </asp:GridView>
                                       
                                        <div>
                                            <asp:Label ID="lblMsgTab" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                     <asp:LinkButton runat="server" ID="SaveButton" OnClick="SaveButton_Click" ValidationGroup="MainValidationGroup">
                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                            ToolTip="Save" />
                                    </asp:LinkButton>
                                </ContentTemplate>
                                <Triggers>
                                    <%--<asp:AsyncPostBackTrigger ControlID="grdCalendarColor" />--%>
                                </Triggers>
                            </asp:UpdatePanel>

</asp:Content>

