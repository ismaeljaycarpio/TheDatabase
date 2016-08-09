<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="DocumentStypeEdit.aspx.cs" Inherits="DocGen.DocumentSectionStyle.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
    <script src="StyleGen.js" type="text/javascript"></script>
  <%--  <link href="<% =ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css") %>" rel="stylesheet"    type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            $(function () {
                $(".popuplink").fancybox({
                    scrolling: 'auto',
                    type: 'iframe',
                    width: 700,
                    height: 400,
                    titleShow: false
                });
            });

        });

    </script>
    <%--<h2>Edit document text style</h2>--%>
    <asp:Label runat="server" ID="lblTopTitle" CssClass="TopTitle" Text="Edit document style"></asp:Label>
    <br />
    <p>
        Edit document text style definition</p>
    <span style="color: Red;">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification"
        ValidationGroup="MainValidationGroup" />
    <div style="width: 42%">
        <table cellpadding="3">
            <tr>
                <td align="left" colspan="2" style="padding-left:60px;">
                    <strong>
                        <asp:Label ID="lblTitle" runat="server" AssociatedControlID="txtTitle">Title:</asp:Label>
                    </strong>
                
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="NormalTextBox" MaxLength="100" Width="250px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="TitleRequired" runat="server" ControlToValidate="txtTitle"
                        CssClass="failureNotification" ErrorMessage="Title is required." ToolTip="Title is required."
                        ValidationGroup="MainValidationGroup">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="display:none;">
                <td align="right">
                    <strong>
                        <asp:Label ID="lblStyle" runat="server" AssociatedControlID="txtStyle">Style:</asp:Label></strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtStyle"  runat="server" CssClass="MultiLineTextBox"
                        ClientIDMode="Static" Width="300px" TextMode="MultiLine" Rows="10"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="StyleRequired" runat="server" ControlToValidate="txtStyle"
                        CssClass="failureNotification" ErrorMessage="Style difinition is required." ToolTip="Style difinition is required."
                        ValidationGroup="MainValidationGroup">*</asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%--<div style="margin: 0; padding: 0;">
                        <span id="zone01"></span>
                        <textarea style="width: 680px; height: 85px;" id="zonetext0" onkeyup="new_fct();"
                            name="content0" cols="83" rows="4">Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi.</textarea>
                    </div>--%>
                    <div>
                        <table cellspacing="3" style="text-align: right; width: 600px;">
                            <tr>
                                <td>
                                    Font:
                                    <select runat="server" onchange="new_fct()" clientidmode="Static" style="width: 110px"
                                        id="ddlFont" class="NormalTextBox">
                                        <option value=""></option>
                                        <option style="font-family: Verdana" value="verdana">Verdana</option>
                                        <option style="font-family: arial" value="arial">Arial</option>
                                        <option style="font-family: arial black" value="arial black">Arial Black</option>
                                        <option style="font-family: trebuchet MS" value="trebuchet ms">Trebuchet MS</option>
                                        <option style="font-family: courier new" value="courier new">Courier</option>
                                        <option style="font-family: helvetica" value="helvetica">Helvetica</option>
                                        <option style="font-family: georgia" value="georgia">Georgia</option>
                                        <option style="font-family: palatino" value="palatino">Palatino</option>
                                        <option style="font-family: impact" value="impact">Impact</option>
                                        <option style="font-family: comic sans ms" value="comic sans ms">Comic Sans MS</option>
                                        <option style="font-family: tahoma" value="tahoma">Tahoma</option>
                                        <option style="font-family: times new roman" value="times new roman">Times New Roman</option>
                                        <option style="font-family: lucida sans unicode" value="lucida sans unicode">Lucida
                                            Sans</option>
                                    </select>
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td align="left">
                                    <input runat="server" clientidmode="Static" type="checkbox" id="chkBold" onchange="new_fct()" /><strong>Bold</strong>
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td>
                                    Text Colour:
                                    <select runat="server" clientidmode="Static" onchange="new_fct()" style="width: 110px"
                                        id="ddlTextColour" class="NormalTextBox">
                                        <option value=""></option>
                                        <option style="color: Aqua;" value="aqua">Aqua</option>
                                        <option style="color: Black;" value="black">Black</option>
                                        <option style="color: Blue;" value="blue">Blue</option>
                                        <option style="color: Fuchsia;" value="fuchsia">Fuchsia</option>
                                        <option style="color: Gray;" value="gray">Gray</option>
                                        <option style="color: Green;" value="green">Green</option>
                                        <option style="color: Lime;" value="lime">Lime</option>
                                        <option style="color: Maroon;" value="maroon">Maroon</option>
                                        <option style="color: Navy;" value="navy">Navy</option>
                                        <option style="color: Olive;" value="olive">Olive</option>
                                        <option style="color: Orange;" value="orange">Orange</option>
                                        <option style="color: Purple;" value="purple">Purple</option>
                                        <option style="color: Red;" value="red">Red</option>
                                        <option style="color: Silver;" value="silver">Silver</option>
                                        <option style="color: Teal;" value="teal">Teal</option>
                                        <option style="color: Black;" value="white">White</option>
                                        <option style="color: Yellow;" value="yellow">Teal</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Size:
                                    <select runat="server" clientidmode="Static" onchange="new_fct()" style="width: 110px;
                                        height: 22px;" id="ddlFontSize" class="NormalTextBox">
                                        <option value=""></option>
                                        <option style="font-size: 8px;" value="8">8</option>
                                        <option style="font-size: 9px;" value="9">9</option>
                                        <option style="font-size: 10px;" value="10">10</option>
                                        <option style="font-size: 11px;" value="11">11</option>
                                        <option style="font-size: 12px;" value="12">12</option>
                                        <option style="font-size: 14px;" value="14">14</option>
                                        <option style="font-size: 16px;" value="16">16</option>
                                        <option style="font-size: 18px;" value="18">18</option>
                                        <option style="font-size: 20px;" value="20">20</option>
                                        <option style="font-size: 22px;" value="22">22</option>
                                        <option style="font-size: 24px;" value="24">24</option>
                                        <option style="font-size: 26px;" value="26">26</option>
                                        <option style="font-size: 28px;" value="28">28</option>
                                        <option style="font-size: 36px;" value="36">36</option>
                                        <option style="font-size: 48px;" value="48">48</option>
                                        <option style="font-size: 72px;" value="72">72</option>
                                    </select>
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td align="left">
                                    <input runat="server" clientidmode="Static" type="checkbox" id="chkItalic" onchange="new_fct()" /><i>Italic</i>
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td>
                                    Background:
                                    <select runat="server" clientidmode="Static" onchange="new_fct()" style="width: 110px"
                                        id="ddlBackground" class="NormalTextBox">
                                        <option value=""></option>
                                        <option style="background-color: Aqua;" value="aqua">Aqua</option>
                                        <option style="background-color: Black; color: white;" value="Black">Black</option>
                                        <option style="background-color: Blue;" value="blue">Blue</option>
                                        <option style="background-color: Fuchsia;" value="fuchsia">Fuchsia</option>
                                        <option style="background-color: Gray;" value="gray">Gray</option>
                                        <option style="background-color: Green;" value="green">Green</option>
                                        <option style="background-color: Lime;" value="lime">Lime</option>
                                        <option style="background-color: Maroon;" value="maroon">Maroon</option>
                                        <option style="background-color: Navy; color: White;" value="navy">Navy</option>
                                        <option style="background-color: Olive;" value="olive">Olive</option>
                                        <option style="background-color: Orange;" value="orange">Orange</option>
                                        <option style="background-color: Purple;" value="purple">Purple</option>
                                        <option style="background-color: Red;" value="red">Red</option>
                                        <option style="background-color: Silver;" value="silver">Silver</option>
                                        <option style="background-color: Teal;" value="teal">Teal</option>
                                        <option style="background-color: White;" value="white">White</option>
                                        <option style="background-color: Yellow;" value="yellow">Teal</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Line Height:
                                    <select runat="server" clientidmode="Static" onchange="new_fct()" style="width: 110px;
                                        height: 22px;" id="ddlLineHeight" class="NormalTextBox">
                                        <option value=""></option>
                                        <option style="line-height: 1;" value="1">1</option>
                                        <option style="line-height: 1.25;" value="1.25">1.25</option>
                                        <option style="line-height: 1.5;" value="1.5">1.5</option>
                                        <option style="line-height: 2;" value="2">2</option>
                                        <option style="line-height: 3;" value="3">3</option>
                                    </select>
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td style="text-decoration: underline;" align="left">
                                    <input runat="server" clientidmode="Static" type="checkbox" id="chkUnderline" onchange="new_fct()" />Underline
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td>
                                    Border Colour:
                                    <select runat="server" clientidmode="Static" onchange="new_fct()" style="width: 110px;
                                        height: 22px;" id="ddlBorderColour" class="NormalTextBox">
                                        <option value=""></option>
                                        <option style="border-color: Aqua; border-style: solid;" value="aqua">Aqua</option>
                                        <option style="border-color: Black; border-style: solid;" value="black">Black</option>
                                        <option style="border-color: Blue; border-style: solid;" value="blue">Blue</option>
                                        <option style="border-color: Fuchsia; border-style: solid;" value="fuchsia">Fuchsia</option>
                                        <option style="border-color: Gray; border-style: solid;" value="gray">Gray</option>
                                        <option style="border-color: Green; border-style: solid;" value="green">Green</option>
                                        <option style="border-color: Lime; border-style: solid;" value="lime">Lime</option>
                                        <option style="border-color: Maroon; border-style: solid;" value="maroon">Maroon</option>
                                        <option style="border-color: Navy; border-style: solid;" value="navy">Navy</option>
                                        <option style="border-color: Olive; border-style: solid;" value="olive">Olive</option>
                                        <option style="border-color: Orange; border-style: solid;" value="orange">Orange</option>
                                        <option style="border-color: Purple; border-style: solid;" value="purple">Purple</option>
                                        <option style="border-color: Red; border-style: solid;" value="red">Red</option>
                                        <option style="border-color: Silver; border-style: solid;" value="silver">Silver</option>
                                        <option style="border-color: Teal; border-style: solid;" value="teal">Teal</option>
                                        <option style="border-color: White; border-style: solid;" value="white">White</option>
                                        <option style="border-color: Yellow; border-style: solid;" value="yellow">Teal</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Border:
                                    <select runat="server" clientidmode="Static" onchange="new_fct()" style="width: 110px;
                                        height: 22px;" id="ddlBorder" class="NormalTextBox">
                                        <option value=""></option>
                                        <option style="border: 0px solid;" value="0">0</option>
                                        <option style="border: 1px solid;" value="1">1</option>
                                        <option style="border: 2px solid;" value="2">2</option>
                                        <option style="border: 3px solid;" value="3">3</option>
                                        <option style="border: 4px solid;" value="4">4</option>
                                        <option style="border: 5px solid;" value="5">5</option>
                                    </select>
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td align="left" style="text-decoration: line-through;">
                                    <input runat="server" clientidmode="Static" type="checkbox" id="chkStrikethrough"
                                        onchange="new_fct()" />Strikethrough
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td>
                                    Margin Width:&nbsp;<input runat="server" clientidmode="Static" id="txtMargin" style="width: 105px;"
                                        onkeyup="new_fct();" value="" type="text" class="NormalTextBox" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <%--<asp:LinkButton runat="server" ID="hlAdvanced" OnClick="hlAdvanced_Click" >Advanced</asp:LinkButton>--%>
                                    <asp:HyperLink runat="server" ID="hlAdvanced" ClientIDMode="Static" CssClass="popuplink"
                                        NavigateUrl="StyleAdvanced.aspx"> Advanced</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none;">
                       
                        <br />
                        <div runat="server" clientidmode="Static" id="csscode1">
                            <asp:TextBox ID="txtGenStyle" runat="server" CssClass="MultiLineTextBox" Width="400px"
                                TextMode="MultiLine" Rows="10" ClientIDMode="Static" ></asp:TextBox>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" style="padding-left: 60px;">
                    <table>
                        <tr>
                            <td>
                                <strong>Record:</strong>
                            </td>
                            <td>
                                <div>
                                    <span id="zonetextnew"></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <%--<span id="Record" style="display:none;">The quick brown fox jumps over the lazy dog</span>--%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong></strong>
                </td>
                <td align="left">
                    <table>
                        <tr>
                           
                            <td>
                                <div runat="server" id="div1">
                                    <asp:HyperLink runat="server" ID="CancelButton" CssClass="btn" CausesValidation="false"
                                        ToolTip="Back"> <strong>Back</strong> </asp:HyperLink>
                                </div>
                            </td>
                            <td>
                            </td>
                               <td>
                                <div runat="server" id="div2">
                                    <asp:LinkButton runat="server" ID="SaveButton" CssClass="btn" OnClick="SaveButton_Click"
                                        ValidationGroup="MainValidationGroup"> <strong>Save</strong> </asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hfFullStyle" ClientIDMode="Static" Value="" />
                                </div>
                            </td>
                            <td>
                            </td>


                            <%--<td>
                                <div runat="server" id="div3">
                                    <asp:LinkButton runat="server" ID="lnkPerformTest" CssClass="btn" OnClick="lnkPerformTest_Click"> <strong>Test Cross</strong> </asp:LinkButton>
                                </div>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <p>
        </p>
        <%--<p class="submitButton">
        <asp:Button ID="SaveButton" runat="server" Text="Save" 
            ValidationGroup="MainValidationGroup" onclick="SaveButton_Click"/>
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" 
            CausesValidation="false" 
            OnClientClick="window.location = 'List.aspx'; return false;" />
    </p>--%>
    </div>
    <script type="text/javascript">
        function ShowRecord() {
            $("#zonetextnew").attr('style', $("textarea:first").val());
        }
        $(function () {
            document.getElementById("zonetextnew").innerHTML = 'The quick brown fox jumps over the lazy dog';
            ShowRecord();
        });

        function PopulateHref() {
            document.getElementById('hlAdvanced').href = 'StyleAdvanced.aspx?txtGenStyle=' + encodeURIComponent(document.getElementById('txtGenStyle').value) + '&txtStyle=' + encodeURIComponent(document.getElementById('txtStyle').value);

        }
    </script>
</asp:Content>
