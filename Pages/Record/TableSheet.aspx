<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TableSheet.aspx.cs" Inherits="Pages_Record_TableSheet" EnableTheming="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%--<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"   type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            //            $('#ddlPinImages').change(function (e) {
            //                document.getElementById("imgPIN").src = document.getElementById("hfWebroot").value + '/' + document.getElementById("ddlPinImages").value;
            //            });
            $('#ctl00_HomeContentPlaceHolder_txtTable').change(function () {
                var txtTable = document.getElementById("ctl00_HomeContentPlaceHolder_txtTable");
                var txtNewMenuName = document.getElementById("ctl00_HomeContentPlaceHolder_txtNewMenuName");
                txtNewMenuName.value = txtTable.value;
            });

            $('#ctl00_HomeContentPlaceHolder_fuRecordFile').change(function () {
                var fuRecordFile = document.getElementById("ctl00_HomeContentPlaceHolder_fuRecordFile")
                var sFileName = fuRecordFile.value;

                if (sFileName.lastIndexOf("\\") > -1) {
                    sFileName = sFileName.substring(sFileName.lastIndexOf("\\") + 1);
                }
                if (sFileName.lastIndexOf(".") > -1) {
                    sFileName = sFileName.substring(0, sFileName.lastIndexOf("."));
                }

                var txtTable = document.getElementById("ctl00_HomeContentPlaceHolder_txtTable");
                txtTable.value = sFileName;
                var txtNewMenuName = document.getElementById("ctl00_HomeContentPlaceHolder_txtNewMenuName");
                txtNewMenuName.value = sFileName;
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

            if (document.getElementById("ctl00_HomeContentPlaceHolder_ddlMenu").value == 'new') {
            }
            else {
                ValidatorEnable(document.getElementById('rfvNewMenuName'), false);
            }

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
    <div class="ContentMain" style="width: 1000px; min-height: 650px; background-color: #ffffff;
        padding-left: 20px;">
        <table border="0" cellpadding="0" cellspacing="0" align="left" onload="ShowHide();">
            <tr>
                <td colspan="2" height="40">
                    <span class="TopTitle">
                        <asp:Label runat="server" ID="lblTitle" Text="Add Table"></asp:Label></span>
                         <asp:HiddenField runat="server" ID="hfFirstTable" ClientIDMode="Static" />
                </td>
                <td >
                 
                </td>
            </tr>
            <tr>
                <td colspan="3" height="13">
                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 50px;">
                </td>
                <td valign="top">
                    <div id="search" style="padding-bottom: 10px">
                    </div>
                    <asp:Panel ID="Panel2" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <div runat="server" id="divDetail">
                                        <table cellpadding="3">
                                            <tr>
                                                <td colspan="2" align="center">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top"  style="width:250px;" >
                                                    <strong>Select your spreadsheet*:</strong>
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="fuRecordFile" runat="server" Style="width: 350px; font-size:12px;" size="70"
                                                        Font-Size="12px" />
                                                    <br />
                                                    CSV,XLS or XLSX
                                                </td>
                                            </tr>
                                            <tr style="height: 15px;">
                                                <td colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" >
                                                    &nbsp;<strong runat="server" id="stgTable">Name*:</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTable" runat="server" Width="256px" CssClass="NormalTextBox"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvTable" runat="server" ControlToValidate="txtTable"
                                                        Display="None" ErrorMessage="Name - Required" CssClass="NormalTextBox"></asp:RequiredFieldValidator>
                                                    <asp:Label runat="server" ID="Label1" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="width: 100px;">
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox Checked="true" runat="server" ID="chkRecordsData" TextAlign="Right"
                                                        CssClass="NormalTextBox" Text="Upload data now" />
                                                </td>
                                            </tr>
                                            <tr style="height: 25px;">
                                                <td colspan="2">
                                                </td>
                                            </tr>

                                           
                                             <tr runat="server" visible="false">
                                                    <td align="right" style="width:250px;">
                                                        <strong>My column headers are on row</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtImportColumnHeaderRow" CssClass="NormalTextBox"
                                                         Width="75px"  Text="1"  ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr runat="server" visible="false">
                                                    <td align="right">
                                                        <strong>My data starts on row</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtImportDataStartRow" CssClass="NormalTextBox"
                                                         Width="75px" Text="2" ></asp:TextBox>
                                                    </td>
                                                </tr>


                                            <tr id="trMenu">
                                                <td align="right" style="width: 230px;">
                                                    <strong>Show under Menu*:</strong>
                                                </td>
                                                <td style="width: 200px;">
                                                    <asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="false" DataTextField="MenuP"
                                                        Width="155px" DataValueField="MenuID" CssClass="NormalTextBox">
                                                    </asp:DropDownList>
                                                    <%--<asp:HyperLink runat="server" ID="hlMenuEdit" Text="Edit" NavigateUrl="~/Pages/Record/TableGroup.aspx"
                                                CssClass="NormalTextBox"></asp:HyperLink>--%>
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
                                            <tr>
                                                <td colspan="2" style="height: 7px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                                        ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct following errors:" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                           
                                                            <td>
                                                                <div runat="server" id="divCancle">
                                                                    <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>&lt;&nbsp;Back</strong> </asp:HyperLink>
                                                                </div>
                                                            </td>
                                                             <td>
                                                                <div runat="server" id="divSave" clientidmode="Static">
                                                                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"> <strong>Next&nbsp;&gt;</strong> </asp:LinkButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td style="padding-left: 10px; vertical-align: top;">
                                    <div style="background-color: #FFE8BC; padding: 10px; width: 160px;">
                                        <asp:Label runat="server" ID="lblHelpContent"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                 <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtImportDataStartRow"
                                                            ErrorMessage="Invalid Import Data Start Row data!" MaximumValue="1000000" MinimumValue="2"
                                                            Type="Integer" Display="None" />
                                                            <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtImportColumnHeaderRow"
                                                            ErrorMessage="Invalid Import Column Header Row!" MaximumValue="1000000" MinimumValue="1"
                                                            Type="Integer" Display="None" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
