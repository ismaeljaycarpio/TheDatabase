<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DetailEdit.ascx.cs" Inherits="Pages_UserControl_DetailEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>

<asp:UpdatePanel ID="upChildDetail" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <div>
            <script type="text/javascript">






                function CheckMyText(sender, args) {
                    var compare = RegExp("^([0-1]?[0-9]|2[0-3]):([0-5][0-9])(:[0-5][0-9])?$");
                    args.IsValid = compare.test(args.Value);
                    return;
                }

                function GetOptValue(optName) {
                    var rates = document.getElementsByName(optName);
                    var rate_value;
                    for (var i = 0; i < rates.length; i++) {
                        if (rates[i].checked) {
                            rate_value = rates[i].value;
                        }
                    }
                    return rate_value;

                }



            </script>
            <table>
                <tr>
                    <td style="width: 10px;"></td>
                    <td>

                        <div runat="server" id="divDynamic" class="NormalTextBox">
                            <asp:Panel runat="server" DefaultButton="lnkSaveClose" ID="pnlDetailWhole">
                                <div runat="server" id="div1">
                                    <table>
                                        <tr>
                                            <td style="width: 10px;"></td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
                                                    <tr>
                                                        <td colspan="3">
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" style="width: 500px;">
                                                                        <%--<span class="TopTitle">
                                                                <asp:Label runat="server" ID="lblTitle"></asp:Label></span>--%>
                                                                        <br />
                                                                        <br />
                                                                        <%--<asp:Label runat="server" ID="lblHeaderName" Style="padding-left: 5px;"> </asp:Label>--%>
                                                                        <asp:HiddenField runat="server" ID="hfRecordID" Value="-1" ClientIDMode="Static" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <table>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td>
                                                                                    <div>

                                                                                        <div runat="server" id="trMainSave">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <div>
                                                                                                            <asp:HyperLink runat="server" ID="hlBack" Visible="false">
                                                                                                                <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"
                                                                                                                    ToolTip="Back" />
                                                                                                            </asp:HyperLink>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton runat="server" ID="lnkPrevious" OnClick="lnkPrevious_Click" CausesValidation="false" Visible="false">
                                                                                                            <asp:Image runat="server" ID="Image3" ImageUrl="~/App_Themes/Default/images/control_rewind_blue.png"
                                                                                                                ToolTip="Previous" />
                                                                                                        </asp:LinkButton>
                                                                                                        &nbsp;
                                                                                            <asp:HyperLink runat="server" ID="hlAdd" NavigateUrl="" Visible="false">
                                                                                                <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/add32.png"
                                                                                                    ToolTip="Add" />
                                                                                            </asp:HyperLink>
                                                                                                        &nbsp;
                                                                                           <%-- <div runat="server" id="divEdit" visible="false">--%>
                                                                                                        <asp:HyperLink runat="server" ID="hlEdit" Visible="false">
                                                                                                            <asp:Image runat="server" ID="ImageEdit" ImageUrl="~/App_Themes/Default/images/Edit_big.png"
                                                                                                                ToolTip="Edit" />
                                                                                                        </asp:HyperLink>
                                                                                                        <asp:HiddenField runat="server" ID="hfEdit" />
                                                                                                        <%-- </div>--%>
                                                                                            &nbsp;
                                                                                            <asp:LinkButton runat="server" ID="lnkNext" OnClick="lnkNext_Click" CausesValidation="false" Visible="false">
                                                                                                <asp:Image runat="server" ID="Image4" ImageUrl="~/App_Themes/Default/images/control_fastforward_blue.png"
                                                                                                    ToolTip="Next" />
                                                                                            </asp:LinkButton>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <div runat="server" id="divSaveClose" visible="false">
                                                                                                            <asp:LinkButton runat="server" ID="lnkSaveClose" CausesValidation="false">
                                                                                                                <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                                                                                    ToolTip="Save" />
                                                                                                            </asp:LinkButton>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="padding-left: 20px;">
                                                                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top"></td>
                                                        <td valign="top">
                                                            <asp:Panel runat="server" ID="pnlDetail">

                                                                <table>
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:Panel runat="server" ID="pnlMain">
                                                                                <asp:Panel runat="server" ID="pnlTabHeading">
                                                                                </asp:Panel>
                                                                                <asp:Panel runat="server" ID="pnlDetailTab">
                                                                                    <table runat="server" id="tblMain">
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <table id="tblLeft" runat="server" visible="true" cellpadding="3">
                                                                                                </table>
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <table id="tblRight" runat="server" visible="true" cellpadding="3" style="margin-left: 10px;">
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>


                                                            </asp:Panel>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="50px"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </div>
                        <div runat="server" id="divNorecordAdd" visible="false" style="padding: 20px;">
                            <asp:HyperLink runat="server" ID="hlNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No records have been added yet. <strong style="text-decoration: underline; color: Blue;">Add new record now.</strong>
                            </asp:HyperLink>
                        </div>
                        <div runat="server" id="divNorecord" visible="false" style="padding: 20px;">
                            No records have been added yet.
                        </div>

                    </td>
                </tr>
            </table>
            <br />
              <asp:Literal ID="ltTextJS" runat="server"></asp:Literal>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
