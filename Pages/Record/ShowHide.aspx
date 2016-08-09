<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="ShowHide.aspx.cs" Inherits="Pages_Record_ShowHide" %>
<%@ Register Src="~/Pages/UserControl/ShowWhenCondition.ascx" TagName="SWCon" TagPrefix="dbg" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

<script language="javascript" type="text/javascript">

    function GetBackValueEdit() {
        window.parent.document.getElementById('chkShowWhen').checked =true;
        //window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = '../Help/CalculationTest.aspx?type=calculation&formula=' + encodeURIComponent(document.getElementById('txtFormula').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
        parent.$.fancybox.close();

    }

    function GetBackValueAdd() {
        window.parent.document.getElementById('chkShowWhen').checked = true;
       
        parent.$.fancybox.close();
            
    }

    //function PutAddValue() {
       

    //    if (document.getElementById('ddlHideColumn').value != '') {
    //        $("#btnPopulateAdd").trigger("click");
    //    }
    //}

    </script>
    <div style="padding-top: 10px;">
        <asp:UpdatePanel ID="upShowWhen" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <table>
                    <tr>
                        <td align="left" colspan="4">
                            <table>
                                <td align="left">
                                    <h3>Show When</h3>
                                </td>
                                <td align="left" style="padding-left: 400px;">
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
                                <td align="left"></td>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">

                            <div style="padding-left: 50px; padding-top: 10px;">
                                <asp:GridView ID="grdShowWhen" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                    CssClass="gridview" OnRowCommand="grdShowWhen_RowCommand" OnRowDataBound="grdShowWhen_RowDataBound"
                                    ShowHeaderWhenEmpty="true"
                                    ShowFooter="true">
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblID" Text='<%# Eval("ID") %>'></asp:Label>
                                                <asp:Label runat="server" ID="lblShowWhenID" Text='<%# Eval("ShowWhenID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnMinus" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png"
                                                    CommandName="minus" CommandArgument='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <strong></strong>
                                            </HeaderTemplate>

                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>

                                                <dbg:SWCon runat="server" ID="swcShowWhen" OnddlHideColumn_Changed="SWCHideColumnChanged" />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField>
                                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnPlus" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png"
                                                    CommandName="plus" Visible="false" CommandArgument='<%# Eval("ID") %>' />
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
                   
                </table>

            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="grdShowWhen" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
