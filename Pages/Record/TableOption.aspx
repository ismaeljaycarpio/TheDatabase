<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TableOption.aspx.cs" Inherits="Pages_Record_TableOption"
    EnableTheming="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

  <%-- <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"   type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>

     <div runat="server" id="trFirstTable" visible="false" style="background-color:#FFD800; text-align:center; margin-bottom:10px;">
          <asp:Label runat="server" ID="lblFirstTableInfo" Font-Size="Large"></asp:Label> <br />
     </div>
    <div class="ContentMain" style="width: 877px; min-height: 650px; background-color: #ffffff;
        padding-left: 20px;">
        <table border="0" cellpadding="0" cellspacing="0" align="left" width="100%">
           
            <tr>
                <td colspan="2" height="40">
                    <table>
                        <tr>
                            <td align="left">
                                <span class="TopTitle">
                                    <asp:Label runat="server" ID="lblTitle" Text="Add Table Options"></asp:Label></span>
<%--                                    <br /><br />  <asp:Label runat="server" ID="lblSuggest" Text="We strongly suggest you create a new Table." Visible="false" Width="600px"></asp:Label>
--%>                            </td>
                            <td align="right">
                               
                            </td>
                        </tr>
                    </table>
                </td>
                <td rowspan="3"  style="padding-right:150px;">
                              <div  style="background-color:#FFE8BC; padding:10px; width:200px;">
                                     <asp:Label runat="server" ID="lblHelpContent"></asp:Label>                                
                              </div>
                </td>
                 
            </tr>
            <tr>
                <td colspan="2" height="20">
                </td>
                <td></td>
            </tr>
            <tr>
                <td valign="top" style="width:50px;">
                </td>
                <td valign="top">
                    <div id="search" style="padding-bottom: 10px">
                    </div>
                    <asp:Panel ID="Panel2" runat="server">
                        <div runat="server" id="divDetail"  style="font-size:9pt;">
                            <table cellpadding="3">
                               
                                <tr>
                                    <td align="right" valign="top">
                                        <%--<strong>Options</strong>--%>
                                    </td>
                                    <td>
                                     <asp:RadioButton runat="server" ID="optCreateFromSpreadSheet" GroupName="ST" Text="Upload Spreadsheet"
                                            ClientIDMode="Static" Font-Size="11pt" Checked="true" />
                                         <br />    <br />
                                        <asp:RadioButton runat="server" ID="optCopyFromTemplate" GroupName="ST" Text="Use Template"
                                            ClientIDMode="Static" Font-Size="11pt"  />
                                         <br />   <br />
                                        <asp:RadioButton runat="server" ID="optBrandNew" GroupName="ST" Text="Brand New Table"
                                             ClientIDMode="Static" Font-Size="11pt" />
                                        
                                        
                                       
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
                                                       
                                                                <asp:HyperLink runat="server" ID="hlBack" 
                                                                CssClass="btn"> <strong>Cancel</strong> </asp:HyperLink>
                                                               
                                                    </div>
                                                </td>

                                                 <td>
                                                    <div runat="server" id="divContinue" clientidmode="Static">
                                                       
                                                                    <asp:LinkButton runat="server" ID="lnkContinue" CssClass="btn" 
                                                                    OnClick="lnkContinue_Click"><strong>Next&nbsp;&gt;</strong></asp:LinkButton>
                                                               
                                                    </div>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
