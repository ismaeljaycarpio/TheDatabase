<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="ColumnColourList.aspx.cs" Inherits="Pages_Record_ColumnColour" %>
<%@ Register Src="~/Pages/UserControl/ShowWhenCondition.ascx" TagName="SWCon" TagPrefix="dbg" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

<script language="javascript" type="text/javascript">

    function GetBackValueEdit() {
        window.parent.document.getElementById('chkColumnColour').checked = true;
        //window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = '../Help/CalculationTest.aspx?type=calculation&formula=' + encodeURIComponent(document.getElementById('txtFormula').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
        parent.$.fancybox.close();

    }

    function GetBackValueAdd() {
        window.parent.document.getElementById('chkColumnColour').checked = true;
        parent.$.fancybox.close();

    }

    function PutAddValue() {
        //document.getElementById('ddlControllingColumn').value = window.parent.document.getElementById('hfControllingColumnID').value;
        //document.getElementById('ddlOperator').value = window.parent.document.getElementById('hfControllingColumnOperator').value;
        //document.getElementById('hfValue').value = window.parent.document.getElementById('hfValue').value;


        //document.getElementById('ddlJoinOperator').value = window.parent.document.getElementById('hfJoinOperator').value;

        //document.getElementById('ddlControllingColumn2').value = window.parent.document.getElementById('hfControllingColumnID2').value;
        //document.getElementById('ddlOperator2').value = window.parent.document.getElementById('hfControllingColumnOperator2').value;
        //document.getElementById('hfValue2').value = window.parent.document.getElementById('hfValue    2').value;


        if (document.getElementById('ddlControllingColumn').value != '') {
            $("#btnPopulateAdd").trigger("click");
        }
    }

    </script>
    <div style="padding-top: 10px;">
           <asp:UpdatePanel ID="upColumnColour" runat="server" UpdateMode="Always">
                                <ContentTemplate>
        <table>
            <tr>
                <td align="left" colspan="4">
                 
                        <asp:Label runat="server" ID="lblTopTitle" CssClass="TopTitle" Text="Text Colour"></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td colspan="4">
                 
                                    <div style="padding-left: 50px; padding-top: 10px;">
                                        <asp:GridView ID="grdColumnColour" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                            CssClass="gridview" OnRowCommand="grdColumnColour_RowCommand" OnRowDataBound="grdColumnColour_RowDataBound"
                                             ShowHeaderWhenEmpty="true"
                                            ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblID" Text='<%# Eval("ID") %>' ></asp:Label>
                                                         <asp:Label runat="server" ID="lblColumnColourID" Text='<%# Eval("ColumnColourID") %>' ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnMinus" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png"
                                                            CommandName="minus"  CommandArgument='<%# Eval("ID") %>'   />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              
                                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Right"  ItemStyle-HorizontalAlign="Left">
                                                     <HeaderTemplate>
                                                         <strong style="padding-right:50px;">Value</strong>
                                                     </HeaderTemplate>
                                                    
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                       
                                                            <dbg:SWCon runat="server" ID="swcColumnColour" OnddlHideColumn_Changed="SWCControllingColumnChanged" />
                                                       
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
                                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
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
                                   
                              

                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <table>
                        <tr>
                           
                            <td>
                                <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:parent.$.fancybox.close(); return false;"
                                    CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                    <%--<asp:Button runat="server" ID="btnPopulateAdd" ClientIDMode="Static"  style="display:none;"/>--%>
                            </td>
                             <td>
                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                    CausesValidation="true"> <strong>Save</strong></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>

        </ContentTemplate>
    <Triggers>
        <%--<asp:AsyncPostBackTrigger ControlID="grdColumnColour" />--%>
    </Triggers>
</asp:UpdatePanel>
    </div>
</asp:Content>
