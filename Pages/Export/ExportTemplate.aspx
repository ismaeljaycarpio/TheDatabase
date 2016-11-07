<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ExportTemplate.aspx.cs" Inherits="Pages_Export_ExportTemplate" EnableEventValidation="false" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">



   <script type="text/javascript">
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

       function checkAll(objRef) {
           var GridView = objRef.parentNode.parentNode.parentNode;
           var inputList = GridView.getElementsByTagName("input");
           for (var i = 0; i < inputList.length; i++) {
               //Get the Cell To find out ColumnIndex
               var row = inputList[i].parentNode.parentNode;
               if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                   if (objRef.checked) {
                       //If the header checkbox is checked
                       //check all checkboxes
                       //and highlight all rows
                       row.style.backgroundColor = "#96FFFF";
                       inputList[i].checked = true;
                   }
                   else {
                       //If the header checkbox is checked
                       //uncheck all checkboxes
                       //and change rowcolor back to original 
                       if (row.rowIndex % 2 == 0) {
                           //Alternating Row Color
                           row.style.backgroundColor = "white";

                       }
                       else {
                           row.style.backgroundColor = "#DCF2F0";
                       }
                       inputList[i].checked = false;
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

        function SelectAllCheckboxes(spanChk) {
            checkAll(spanChk);
            // Added as ASPX uses SPAN for checkbox

            var oItem = spanChk.children;

            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];

            xState = theBox.checked;

            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)

                if (elm[i].type == "checkbox" && elm[i].id != theBox.id && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkIsActive' && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkShowOnlyWarning' && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkShowAdvancedOptions') {

                    //elm[i].click();

                    if (elm[i].checked != xState)

                        elm[i].click();

                    //elm[i].checked=xState;

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
                                    <span class="TopTitle">Export Template</span>
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
                            <div id="search"   class="searchcorner"  onkeypress="abc();">
                                <br />
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr>
                                        <td align="right">
                                            <strong runat="server" id="stgTableCap">Table</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <strong>Template Name</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtSearch" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
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
                            AllowPaging="True" AllowSorting="True" DataKeyNames="ExportTemplateID" HeaderStyle-ForeColor="Black"
                            Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                            OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound"
                             AlternatingRowStyle-BackColor="#DCF2F0">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("ExportTemplateID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <input id="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                            type="checkbox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("ExportTemplateID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <%--<asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlview" runat="server" ToolTip="View" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("ExportTemplateID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                    </ItemTemplate>
                                
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Table" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblTable" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField SortExpression="ExportTemplateName" HeaderText="ExportTemplate Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExportTemplateName" runat="server" Text='<%# Eval("ExportTemplateName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                          
                              
                               
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" HideDelete="false" HideAdd="false" OnExportForCSV="Pager_OnExportForCSV"
                                    OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" OnDeleteAction="Pager_DeleteAction" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                            <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                    Add new.</strong>
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
                                     Add new.</strong>
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
