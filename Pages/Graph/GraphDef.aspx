<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="GraphDef.aspx.cs" Inherits="Pages_Graph_GraphDef" EnableEventValidation="false" %>

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

       function Check_Click(objRef) {
           //Get the Row based on checkbox
           var row = objRef.parentNode.parentNode;
           if (objRef.checked) {
               //If checked change color to Aqua
               row.style.backgroundColor = "#96FFFF";
           }
           else {
               //If not checked change back to original color
               if (row.rowIndex % 2 == 0) {
                   //Alternating Row Color
                   row.style.backgroundColor = "white";

               }
               else {
                   row.style.backgroundColor = "#DCF2F0";
               }
           }

           //Get the reference of GridView
           var GridView = row.parentNode;

           //Get all input elements in Gridview
           var inputList = GridView.getElementsByTagName("input");

           for (var i = 0; i < inputList.length; i++) {
               //The First element is the Header Checkbox
               var headerCheckBox = inputList[0];

               //Based on all or none checkboxes
               //are checked check/uncheck Header Checkbox
               var checked = true;
               if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                   if (!inputList[i].checked) {
                       checked = false;
                       break;
                   }
               }
           }
           headerCheckBox.checked = checked;
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
                                    <span class="TopTitle">Graph Definitions</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                           <asp:Image ID="Image2" runat="server" AlternateText="Processing..."  ImageUrl="~/Images/ajax.gif"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                </td>
                                <td style="width: 50px;">
                                    <div runat="server" id="divConfig" visible="true">
                                        <asp:HyperLink runat="server" ID="hlConfig" ClientIDMode="Static" NavigateUrl="DefaultGraphDef.aspx" >
                                        <asp:Image runat="server" ID="imgConfig" 
                                            ImageUrl="~/App_Themes/Default/images/Config.png"
                                            ToolTip="Configure" />
                                        </asp:HyperLink>
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
                            <div id="search" class="searchcorner" onkeypress="abc();">
                                <br />
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr>
                                        <td align="right">
                                            <strong>Search</strong>
                                        </td>
                                        <td width="260px">
                                            <asp:TextBox runat="server" ID="txtDefinitionName" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <div>
                                                <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong> </asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right"><strong>Table</strong></td>
                                        <td colspan="2">
                                            <asp:DropDownList runat="server" ID="ddlEachTable"
                                                DataTextField="TableName" DataValueField="TableID" 
                                                OnSelectedIndexChanged="ddlEachTable_SelectedIndexChanged" AutoPostBack="true"
                                                AppendDataBoundItems="true"
                                                CssClass="NormalTextBox" Width="200px">
                                                <asp:ListItem Text="-- All --" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                            <strong>Column</strong>
                                            <asp:DropDownList runat="server" ID="ddlEachAnalyte"
                                                OnSelectedIndexChanged="ddlEachAnalyte_SelectedIndexChanged" AutoPostBack="true"
                                                AppendDataBoundItems="true"
                                                CssClass="NormalTextBox" Width="200px">
                                                <asp:ListItem Text="-- All --" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkIsActive" runat="server"
                                                OnCheckedChanged="chkIsActive_CheckedChanged" AutoPostBack="true"
                                                Checked="false" Text="Show Deleted Records" />
                                            &nbsp;
                                            <asp:CheckBox ID="chkIsHidden" runat="server"
                                                OnCheckedChanged="chkIsHidden_CheckedChanged" AutoPostBack="true"
                                                Checked="false" Text="Show Hidden Records" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            AllowPaging="True" AllowSorting="True" DataKeyNames="GraphDefinitionID" HeaderStyle-ForeColor="Black"
                            Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                            OnPreRender="gvTheGrid_PreRender"  OnRowDataBound="gvTheGrid_RowDataBound"
                            AlternatingRowStyle-BackColor="#DCF2F0">
                            <PagerSettings Position="Top" />
                            <Columns>
                                 <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="16px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("GraphDefinitionID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Width="16px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlEdit" runat="server" ToolTip="Edit Definition" NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("GraphDefinitionID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="16px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibDuplicate" runat="server" ToolTip="Duplicate Definition"
                                            ImageUrl="~/App_Themes/Default/Images/iconCopy.png"
                                            CommandArgument='<%# Eval("GraphDefinitionID") %>' OnCommand="ibDuplicate_Command" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DefinitionName" HeaderText="Definition Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDefinitionName" runat="server" Text='<%# Eval("DefinitionName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="IsSystem" HeaderText="System">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbIsSystem" runat="server" Checked='<%# Eval("IsSystem") %>' Enabled="false"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="IsHidden" HeaderText="Hidden">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbIsHidden" runat="server" Checked='<%# Eval("IsHidden") %>' Enabled="false"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
<%--                                <asp:TemplateField SortExpression="IsDefault" HeaderText="Table / Column<br />Default">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbIsDefault" runat="server" Checked='<%# Eval("IsDefault") %>' Enabled="false"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField SortExpression="TableName" HeaderText="Table Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbTableName" runat="server" Text='<%# Eval("TableName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="GraphLabel" HeaderText="Column Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbColumnName" runat="server" Text='<%# Eval("GraphLabel") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="IsActive" HeaderText="Deleted" Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbIsActive" runat="server" Enabled="false"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" OnExportForCSV="Pager_OnExportForCSV" HideExport="false"
                                    OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" OnDeleteAction="Pager_DeleteAction" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                            <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No graph definitions have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                    Add new definition now.</strong>
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
                                     Add new definition.</strong>
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
