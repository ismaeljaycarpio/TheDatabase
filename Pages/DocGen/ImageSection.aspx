<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="ImageSection.aspx.cs" Inherits="DocGen.Document.ImageSection.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">

     <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>

    <script type="text/javascript">
        function SavedAndRefresh() {
            window.parent.document.getElementById('btnRefresh').click();
            parent.$.fancybox.close();

        }

        function CloseAndRefresh() {
            if (document.getElementById('hfRemoveSection').value == '0') {
                parent.$.fancybox.close();
            }
            else {
                //                window.parent.document.getElementById('btnRefresh').click();
                window.parent.RemoveNoAddedSection();
                parent.$.fancybox.close();
            }

        }
       

       

        $(document).ready(function () {

            $(function () {
                $(".showlink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 900,
                height: 500,
                titleShow: false
                });
            });


            function ShowHideControl()
            {
                var chkOpenLink = document.getElementById("chkOpenLink");
                if (chkOpenLink.checked == true) {
                    $("#txtOpenLink").fadeIn();
                }
                else {
                    $("#txtOpenLink").fadeOut();
                }

            }

            $("#chkOpenLink").click(function () {
                ShowHideControl();
            });
            $("#chkShowWhen").click(function () {
                var chkShowWhen = document.getElementById("chkShowWhen");
                if (chkShowWhen.checked == true) {
                    $("#hlShowWhen").trigger("click");
                }

            });
            ShowHideControl();
        });
    </script>
   
    <asp:HiddenField runat="server" ID="hfRemoveSection" ClientIDMode="Static" Value="0" />
    <br />
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification"
        ValidationGroup="MainValidationGroup" />
    <br />
    <div>
        <table cellpadding="3">
            <%--<tr>
                <td align="right">
                    <strong>Title </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTitle" MaxLength="500" runat="server" CssClass="NormalTextBox"
                        Width="450px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="TitleRequired" runat="server" ControlToValidate="txtTitle"
                        CssClass="failureNotification" ErrorMessage="Title is required." ToolTip="Title is required."
                        ValidationGroup="MainValidationGroup">*</asp:RequiredFieldValidator>
                </td>
            </tr>--%>
            <tr>
                <td align="left">
                     <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="Image"></asp:Label>
                </td>
                <td align="right">
                     <table>
                        <tr>
                                                      
                            <%--<td>
                                <div runat="server" id="div1">
                                   
                                    <asp:LinkButton runat="server" ID="CancelButton" CausesValidation="false"
                                        OnClientClick="CloseAndRefresh(); return false; " > 
                                        <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                        </asp:LinkButton>
                                           
                                </div>
                            </td>--%>
                             <td>
                            </td>
                             <td>
                                <div runat="server" id="div2">
                                   
                                                <asp:LinkButton runat="server" ID="SaveButton"  OnClick="SaveButton_Click"
                                                    ValidationGroup="MainValidationGroup" > 
                                                         <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                                      </asp:LinkButton>
                                         
                                </div>
                            </td>
                        </tr>
                    </table>

                    
                </td>
            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Position</strong>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlPosition" runat="server" CssClass="NormalTextBox">
                        <asp:ListItem Value="left">Left (default)</asp:ListItem>
                        <asp:ListItem Value="center">Center</asp:ListItem>
                        <asp:ListItem Value="right">Right</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Width </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtSize" MaxLength="4" runat="server" CssClass="textEntry" Width="60px"></asp:TextBox>
                    px&nbsp;&nbsp;&nbsp; (<span class="comment">The height will be scale automatically.
                        Leave this blank to show full-size image</span>)
                </td>
            </tr>
            <tr>
                <td></td>
                 <td align="left">
                     <asp:CheckBox runat="server" ID="chkShowWhen" Text="" TextAlign="Right" Font-Bold="true"
                        ClientIDMode="Static" />
                    <asp:HyperLink runat="server" NavigateUrl="~/Pages/Record/ShowHide.aspx" CssClass="showlink"
                        ID="hlShowWhen" ClientIDMode="Static">Show When...</asp:HyperLink>
                    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfShowHref" />  <br />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Open Link </strong>
                </td>
                 <td align="left">
                     <table style="border-collapse:collapse;border-spacing:0;">
                         <tr>
                              <td align="left">
                                  <asp:CheckBox runat="server" ID="chkOpenLink"  Checked="false"
                                        ClientIDMode="Static" />
                             </td>
                              <td align="left">
                                  <asp:TextBox runat="server" ID="txtOpenLink" ClientIDMode="Static" CssClass="NormalTextBox" Width="500px"></asp:TextBox>
                             </td>
                         </tr>
                     </table>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <strong>Image</strong>
                </td>
                <td align="left">
                    <asp:FileUpload ID="fuImage" runat="server" Width="400px" Font-Size="11px" onchange="this.form.submit();" />
                    <asp:CustomValidator ID="ImageValidator" ControlToValidate="fuImage" runat="server"
                        CssClass="failureNotification" ErrorMessage="Image file is invalid." ToolTip="Image file is invalid."
                        ValidationGroup="MainValidationGroup" OnServerValidate="ImageValidator_ServerValidate">*</asp:CustomValidator>
                    <p class="comment">
                        <asp:Label ID="lblAllowedExt" runat="server"><b>Allowed file types</b>: {0}</asp:Label>
                        <br />
                        <asp:Label ID="lblMaxFileSize" runat="server"><b>Max file size in</b>: {0}KB</asp:Label>
                    </p>
                </td>
            </tr>
            
            <tr>
                <td align="right">
                    <strong></strong>
                </td>
                <td align="left">
                    <asp:Image ID="imgImage" onerror="this.src = '../../images/no_img.gif';" AlternateText="Image"
                        Style="max-width: 450px" runat="server" ImageUrl="~/Images/no_img.gif" />
                </td>
            </tr>
            <tr >
                
                <td colspan="2" align="center">
                   
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong></strong>
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong></strong>
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong></strong>
                </td>
                <td align="left">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
