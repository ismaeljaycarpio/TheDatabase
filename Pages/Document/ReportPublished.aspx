<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ReportPublished.aspx.cs" Inherits="Pages_Document_ReportPublished" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
   <%-- <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"     type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <table class="ContentMain" style="width: 970px;">
        <tr>
            <td width="28">
            </td>
            <td>
                <h1>
                    <asp:Label runat="server" ID="lblHeading" Text="Report Published"></asp:Label>
                </h1>
            </td>
            <td width="28">
            </td>
        </tr>
        <tr>
            <td width="28">
            </td>
            <td>
                <div>
                    <div style="text-align: right; width: 100%;">
                        <table style="text-align: right; width: 100%; font-size:18px;">
                            <tr>
                                <td align="left">
                                    Congratulations you have published your report and it is now available to the public
                                    at the following address:
                                     <br />
                                    <br />
                                    <asp:HyperLink runat="server" ID="hlReportOne" Target="_blank"></asp:HyperLink>
                                    <%--<asp:HyperLink runat="server" ID="hlReportAccount"></asp:HyperLink>--%>
                                    <br />
                                     <br />
                                    This link can be copied and pasted into email or used on other websites.
                                    <br />
                                     <br />
                                    If you have published by mistake go to 
                                    <asp:HyperLink runat="server" ID="hlProperties" ClientIDMode="Static">Document Properties</asp:HyperLink>
                                    and untick the Published checkbox.
                                    <br />
                                     <br />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
            <td width="28">
            </td>
        </tr>
        <tr>
            <td width="28">
            </td>
            <td align="center" height="10px">
            </td>
            <td width="28">
            </td>
        </tr>
        <tr>
            <td width="28">
            </td>
            <td align="left">
                <div>
                    
                                <asp:HyperLink runat="server" ID="hlBack" CssClass="btn" 
                                CausesValidation="false"> <strong>Back</strong></asp:HyperLink>
                           
                </div>
            </td>
            <td width="28">
            </td>
        </tr>
        <tr>
            <td width="28">
            </td>
            <td align="center" height="40px">
            </td>
            <td width="28">
            </td>
        </tr>
    </table>
    <script type="text/javascript">
    
     jQuery(document).ready(function() {                   

                         $("#hlProperties").fancybox({                                   
                        'transitionIn'  : 'elastic',
                        'transitionOut' : 'none',
                                scrolling: 'auto',
                                type: 'iframe',
                                width: 750,
                                height: 450,
                                titleShow: false                               
                            });  

                     });         
    
    </script>
</asp:Content>
