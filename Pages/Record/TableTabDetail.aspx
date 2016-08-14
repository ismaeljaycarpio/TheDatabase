<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="TableTabDetail.aspx.cs" Inherits="Pages_Record_TableTabDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
     <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>



    <script language="javascript" type="text/javascript">

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
            $("#chkShowWhen").click(function () {
                var chkShowWhen = document.getElementById("chkShowWhen");
                if (chkShowWhen.checked == true) {
                    $("#hlShowWhen").trigger("click");
                }

            });


            function abc() {
                var b = document.getElementById('<%= lnkSave.ClientID %>');
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
        });

      

 
    </script>
    <table border="0" cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                        </td>
                        <td align="left">
                            <div style="width: 40px; height: 40px;">
                                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress2" runat="server">
                                    <ProgressTemplate>
                                        <table style="width: 100%; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" >
                                <table cellpadding="3">
                                  
                                    <tr >
                                        <td align="right">
                                            <strong>Page:</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTabName" runat="server" Width="150px" CssClass="NormalTextBox"></asp:TextBox>
                                              <asp:RequiredFieldValidator ID="rfvTabtName" runat="server" ControlToValidate="txtTabName"
                                                ErrorMessage="Page - Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr id="rrShowWhen" runat="server">
                                        <td></td>
                                        <td align="left">
                                            <asp:CheckBox runat="server" ID="chkShowWhen" Text="" TextAlign="Right" Font-Bold="true"
                                                ClientIDMode="Static" />
                                            <asp:HyperLink runat="server" NavigateUrl="~/Pages/Record/ShowHide.aspx" CssClass="showlink"
                                                ID="hlShowWhen" ClientIDMode="Static">Show When...</asp:HyperLink>
                                            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfShowHref" />
                                            <br />
                                        </td>
                                    </tr>
                                    
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                          
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div runat="server" id="divSave">
                                                
                                                            <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                                CausesValidation="true"> <strong>Save</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                               
                                                            <%--<asp:HyperLink runat="server" ID="hlBack"  CssClass="btn"> <strong>Back</strong> </asp:HyperLink>--%>
                                                              <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                                    CssClass="btn" CausesValidation="false"> <strong>Close</strong></asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false">
                                                
                                                            <asp:HyperLink runat="server" ID="hlEditLink" CssClass="btn"> <strong>Edit</strong> </asp:HyperLink>
                                                       
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
    </table>

     <script type="text/javascript">

         function CloseAndRefresh() {
             window.parent.document.getElementById('btnRefreshPages').click();
             parent.$.fancybox.close();
             // alert('ok');
         }
    
    </script>
</asp:Content>
