<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TableProperty.aspx.cs" Inherits="Pages_Record_TableProperty" EnableEventValidation="false" %>

<%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
  <%--  <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <script type="text/javascript" language="javascript">
        function addrows() {
            // alert('test');
            $("#lnkHiddenAdd").trigger('click');
        }

    </script>
    <script language="javascript" type="text/javascript">

        function Submit() {

            document.forms["aspnetForm"].submit()
            //        alert('ok');

        }

        $(document).ready(function () {

            //            $('#ddlPinImages').change(function (e) {
            //                document.getElementById("imgPIN").src = document.getElementById("hfWebroot").value + '/' + document.getElementById("ddlPinImages").value;
            //            });

            $('#ctl00_HomeContentPlaceHolder_txtTable').change(function () {
                var txtTable = document.getElementById("ctl00_HomeContentPlaceHolder_txtTable");
                var txtNewMenuName = document.getElementById("ctl00_HomeContentPlaceHolder_txtNewMenuName");
                txtNewMenuName.value = txtTable.value;
            });

            $('#ctl00_HomeContentPlaceHolder_txtTable').keyup(function () {
                var txtTable = document.getElementById("ctl00_HomeContentPlaceHolder_txtTable");
                var txtNewMenuName = document.getElementById("ctl00_HomeContentPlaceHolder_txtNewMenuName");
                txtNewMenuName.value = txtTable.value;
            });

            ValidatorEnable(document.getElementById('rfvNewMenuName'), true);
            $('#ctl00_HomeContentPlaceHolder_ddlMenu').change(function (e) {
                if (document.getElementById("ctl00_HomeContentPlaceHolder_ddlMenu").value == 'new') {
                    $("#trNewMenuName").fadeIn();
                    var txtNewMenuName = document.getElementById("ctl00_HomeContentPlaceHolder_txtNewMenuName");
                    txtNewMenuName.value = '';
                    $('#ctl00_HomeContentPlaceHolder_lblMsg').text('');
                    ValidatorEnable(document.getElementById('rfvNewMenuName'), true);
                }
                else {
                    $("#trNewMenuName").fadeOut();
                    var txtNewMenuName = document.getElementById("ctl00_HomeContentPlaceHolder_txtNewMenuName");
                    txtNewMenuName.value = '';
                    $('#ctl00_HomeContentPlaceHolder_lblMsg').text('');
                    ValidatorEnable(document.getElementById('rfvNewMenuName'), false);
                }
            });

        });





        function ShowHide() {
            if (document.getElementById("hfFirstTable").value == '') {
                $("#trMenu").fadeIn();
                if (document.getElementById("ctl00_HomeContentPlaceHolder_ddlMenu").value == 'new') {
                    $("#trNewMenuName").fadeIn();
                    ValidatorEnable(document.getElementById('rfvNewMenuName'), true);
                }
                else {
                    $("#trNewMenuName").fadeOut();
                    ValidatorEnable(document.getElementById('rfvNewMenuName'), false);
                }
            }
            else {
                $("#trNewMenuName").fadeOut();
                $("#trMenu").fadeOut();
                ValidatorEnable(document.getElementById('rfvNewMenuName'), false);
            }
        }

        if (window.addEventListener)
            window.addEventListener("load", ShowHide, false);
        else if (window.attachEvent)
            window.attachEvent("onload", ShowHide);
        else if (document.getElementById)
            window.onload = ShowHide;



    </script>
    <asp:Panel ID="Panel2" runat="server">
        <div style="padding-left: 20px; padding-top: 10px;">
            <table border="0" cellpadding="0" cellspacing="0" align="left" width="100%" onload="ShowHide();">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 80%;">
                                    <span class="TopTitle">
                                        <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                                        <asp:HiddenField runat="server" ID="hfFirstTable" ClientIDMode="Static" />
                                </td>
                                <td align="right" style="padding-left: 80px;">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <div style="width: 40px; height: 40px;">
                                        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <asp:Image ID="Image2" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
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
                    <td valign="top" align="left" colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div runat="server" id="div1" >
                                    <div runat="server" id="divDetail">
                                        <asp:HiddenField runat="server" ID="hfMenuID" />
                                        <asp:HiddenField runat="server" ID="hfTableID" />
                                        <table>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td align="right" style="width: 230px;">
                                                                <strong runat="server" id="stgNameCaption">Name*:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTable" runat="server" Width="256px" CssClass="NormalTextBox"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trMenu" clientidmode="Static" style="display:none;">
                                                            <td align="right">
                                                                <strong>Menu:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="false" DataTextField="MenuP"
                                                                    Width="155px" DataValueField="MenuID" CssClass="NormalTextBox">
                                                                </asp:DropDownList>
                                                                <asp:HyperLink runat="server" ID="hlMenuEdit" Text="Edit" NavigateUrl="~/Pages/Record/TableGroup.aspx"
                                                                    CssClass="NormalTextBox" Visible="false"></asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                        <tr id="trNewMenuName" style="display: none;">
                                                            <td align="right">
                                                                <strong>New Menu Name*:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNewMenuName" runat="server" Width="256px" CssClass="NormalTextBox"></asp:TextBox>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvNewMenuName" ControlToValidate="txtNewMenuName"
                                                                    ClientIDMode="Static" ErrorMessage="New Menu Name - Required" CssClass="NormalTextBox"
                                                                    Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <%--<tr style="height:15px;">
                                                            <td colspan="2"></td>
                                                        </tr>--%>
                                                        <tr >
                                                            <td align="right" valign="top">
                                                          
                                                                <asp:Label runat="server" ID="lblAddFields" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="div3" style="font-size: 9pt;">
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
                                                                                    GridLines="Both" OnRowDataBound="grdFields_RowDataBound" Visible="false">
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
                                                                                <asp:Label runat="server" ID="Label1" ForeColor="Red"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                  
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                            </td>
                                                            <td align="left">
                                                                <asp:RequiredFieldValidator ID="rfvTable" runat="server" ControlToValidate="txtTable"
                                                                    ErrorMessage="Name - Required" CssClass="NormalTextBox" Display="None"></asp:RequiredFieldValidator>
                                                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                                                    ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct following errors:" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                            </td>
                                                            <td>
                                                                <div style="text-align: left">
                                                                    <table runat="server" id="tblAddButton">
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <asp:HyperLink runat="server" ID="hlBack2" CssClass="btn"><strong>&lt;&nbsp;Back</strong></asp:HyperLink>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div runat="server" id="div2">
                                                                                    <asp:LinkButton runat="server" ID="lnkNext" CssClass="btn" OnClick="lnkSave_Click"
                                                                                        CausesValidation="true"> <strong>Next&nbsp;&gt;</strong> </asp:LinkButton>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="padding-left: 100px; vertical-align: top;">
                                                    <div style="background-color: #FFE8BC; padding: 10px; width: 160px;">
                                                        <asp:Label runat="server" ID="lblHelpContent"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="50px">
                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfWebroot" />
                        <asp:DropDownList Visible="false" runat="server" ID="ddlPinImages" AutoPostBack="false"
                            ClientIDMode="Static" CssClass="NormalTextBox">
                            <asp:ListItem Selected="True" Text="--Please Select--" Value="Pages/Record/PINImages/DefaultPin.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Blue" Value="Pages/Record/PINImages/PinBlue.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Black" Value="Pages/Record/PINImages/PinBlack.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Gray" Value="Pages/Record/PINImages/PinGray.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Green" Value="Pages/Record/PINImages/PinGreen.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Light Blue" Value="Pages/Record/PINImages/PinLBlue.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Orange" Value="Pages/Record/PINImages/PinOrange.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Purple" Value="Pages/Record/PINImages/PinPurple.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Red" Value="Pages/Record/PINImages/PinRed.png"></asp:ListItem>
                            <asp:ListItem Text="Pin Yellow" Value="Pages/Record/PINImages/PinYellow.png"></asp:ListItem>
                            <asp:ListItem Text="Round Blue" Value="Pages/Record/PINImages/RoundBlue.png"></asp:ListItem>
                            <asp:ListItem Text="Round Gray" Value="Pages/Record/PINImages/RoundGray.png"></asp:ListItem>
                            <asp:ListItem Text="Round Green" Value="Pages/Record/PINImages/RoundGreen.png"></asp:ListItem>
                            <asp:ListItem Text="Round Light Blue" Value="Pages/Record/PINImages/RoundLightBlue.png"></asp:ListItem>
                            <asp:ListItem Text="Round Orange" Value="Pages/Record/PINImages/RoundOrange.png"></asp:ListItem>
                            <asp:ListItem Text="Round Purple" Value="Pages/Record/PINImages/RoundPurple.png"></asp:ListItem>
                            <asp:ListItem Text="Round Red" Value="Pages/Record/PINImages/RoundRed.png"></asp:ListItem>
                            <asp:ListItem Text="Round White" Value="Pages/Record/PINImages/RoundWhite.png"></asp:ListItem>
                            <asp:ListItem Text="Round Yellow" Value="Pages/Record/PINImages/RoundYellow.png"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
