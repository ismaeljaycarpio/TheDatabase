<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TableFields.aspx.cs" Inherits="Pages_Record_TableFields" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%-- <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"   type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <script type="text/javascript" language="javascript">
        function addrows() {
            // alert('test');
            $("#lnkHiddenAdd").trigger('click');
        }

    </script>
    <div class="ContentMain" style="width: 877px; min-height: 650px; background-color: #ffffff;
        padding-left: 20px;">
        <table border="0" cellpadding="0" cellspacing="0" align="left" width="100%">
            <tr>
                <td colspan="2" height="40">
                    <table>
                        <tr>
                            <td align="left">
                                <span class="TopTitle">
                                    <asp:Label runat="server" ID="lblTitle" Text="Add Fields" Width="400px"></asp:Label></span>
                                <br />
                            </td>
                            <td align="right">
                                <%--<asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static" NavigateUrl="~/Pages/Help/Help.aspx?contentkey=TableOptionsHelp">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/images/help.png" />
                                </asp:HyperLink>--%>
                            </td>
                        </tr>
                    </table>
                </td>
                <td >
                   
                </td>
            </tr>
            <tr>
                <td colspan="3" height="20">
                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 50px;">
                </td>
                <td valign="top">
                    <div id="search" style="padding-bottom: 10px">
                    </div>
                    <asp:Panel ID="Panel2" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div runat="server" id="divDetail" style="font-size: 9pt;">
                                    <table cellpadding="3">
                                        <tr>
                                            <td align="right" valign="top">
                                            </td>
                                            <td>
                                                <%--<asp:LinkButton runat="server" ID="lnkAdd" CssClass="btn" 
                                                                    OnClick="lnkAdd_Click"><strong>Add New Field</strong></asp:LinkButton>--%>
                                                <asp:Button Style="display: none;" runat="server" ID="lnkHiddenAdd" ClientIDMode="Static"
                                                    OnClick="lnkAdd_Click" CausesValidation="true"></asp:Button>
                                                <asp:GridView ID="grdFields" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                                    GridLines="Both" OnRowDataBound="grdFields_RowDataBound">
                                                    <HeaderStyle CssClass="gridview_header" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="true" HeaderText="Field Name">
                                                            <ItemTemplate>
                                                                <asp:TextBox runat="server" ID="txtDisplayName" CssClass="NormalTextBox" placeholder="Enter the field name here"
                                                                    Width="200px" onblur="addrows()"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type">
                                                            <ItemTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlType" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="checkbox" Text="Checkbox"></asp:ListItem>
                                                                    <asp:ListItem Value="staticcontent" Text="Content"></asp:ListItem>
                                                                    <%--<asp:ListItem Value="content" Text="Content Editor"></asp:ListItem>--%>
                                                                    <asp:ListItem Value="date_time" Text="Date / Time"></asp:ListItem>
                                                                    <asp:ListItem Value="dropdown" Text="Dropdown"></asp:ListItem>
                                                                    <asp:ListItem Value="file" Text="File"></asp:ListItem>
                                                                    <asp:ListItem Value="image" Text="Image"></asp:ListItem>
                                                                    <asp:ListItem Value="listbox" Text="List Box (multi-select)"></asp:ListItem>
                                                                    <asp:ListItem Value="location" Text="Location"></asp:ListItem>
                                                                    <asp:ListItem Value="number" Text="Number"></asp:ListItem>
                                                                    <asp:ListItem Value="radiobutton" Text="Radio Button"></asp:ListItem>
                                                                    <asp:ListItem Value="text" Text="Text" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height: 7px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div runat="server" id="divNext" clientidmode="Static">
                                                                <asp:LinkButton runat="server" ID="lnkNext" CssClass="btn" OnClick="lnkNext_Click"><strong>Finish</strong></asp:LinkButton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <%--<div runat="server" id="divBack">
                                                       
                                                                <asp:HyperLink runat="server" ID="hlBack" 
                                                                CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                                               
                                                    </div>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <table style="width: 100%; text-align: center">
                                    <tr>
                                        <td>
                                            <asp:Image ID="Image2" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </asp:Panel>
                </td>
                <td valign="top">
                     <div style="background-color: #FFE8BC; padding: 10px; width: 200px;">
                        <asp:Label runat="server" ID="lblHelpContent"></asp:Label>
                    </div>

                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
