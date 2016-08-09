<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="DashBoard.aspx.cs" Inherits="Pages_Home_DashBoard" EnableEventValidation="false" %>

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
               //            if (checkbox == null) {
               //                return;
               //            }

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
                                    <span class="TopTitle">Dashboards</span>
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
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDocumentText" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <div>
                                              
                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                             HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            AllowPaging="True" AllowSorting="True" DataKeyNames="DocumentID" HeaderStyle-ForeColor="Black"
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
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("DocumentID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField>
                                    <ItemStyle Width="16px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlEdit" runat="server" ToolTip="Edit Properties" NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("DocumentID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField >
                                    <ItemStyle Width="16px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlProperty" runat="server"  Target="_blank" ToolTip="Edit Dashboard"
                                        NavigateUrl='<%# EditPropertyURL() + Cryptography.Encrypt(Eval("DocumentID").ToString())  %>'>   
                                                <asp:Image runat="server" Height="16px" Width="16px" ImageUrl="~/App_Themes/Default/Images/map_edit.png" />
                                           </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DocumentText" HeaderText="Dashboard Name">
                                <%--<ItemStyle  HorizontalAlign="Left" />--%>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentText" runat="server" Text='<%# Eval("DocumentText") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderText="User">
                                <ItemStyle  HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFullName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField SortExpression="DocumentText" HeaderText="Link">
                                  <%--<ItemStyle  HorizontalAlign="Left" />--%>
                                    <ItemTemplate>
                                       

                                        <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# ViewPropertyURL() + Cryptography.Encrypt(Eval("DocumentID").ToString()) %>'><%# ViewPropertyURL() + Cryptography.Encrypt(Eval("DocumentID").ToString())%></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager"  OnExportForCSV="Pager_OnExportForCSV" HideExport="true"
                                    OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" OnDeleteAction="Pager_DeleteAction" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                            <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No dashboards have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                    Add new dashboard now.</strong>
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
                                     Add new dashboard.</strong>
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
