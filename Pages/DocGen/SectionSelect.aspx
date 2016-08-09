<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="SectionSelect.aspx.cs" Inherits="Pages_DocGen_SectionSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
     <script type="text/javascript">
         
         parent.document.getElementById("hfSectionType").value = '-1';
         
         function TextOpen() {

             window.parent.document.getElementById('hlText').href = document.getElementById("hfTextPath").value;
             parent.document.getElementById("hfSectionType").value = '2';
             parent.$.fancybox.close();
         }

         function HTMLOpen() {
             window.parent.document.getElementById('hlHTML').href = document.getElementById("hfHTMLPath").value;
             parent.document.getElementById("hfSectionType").value = '1';
             parent.$.fancybox.close();
         }


         function TableOpen() {
             window.parent.document.getElementById('hlTable').href = document.getElementById("hfTablePath").value;
             parent.document.getElementById("hfSectionType").value = '6';
             parent.$.fancybox.close();
         }

         function PhotoOpen() {
             window.parent.document.getElementById('hlPhoto').href = document.getElementById("hfPhotoPath").value;
             parent.document.getElementById("hfSectionType").value = '3';
             parent.$.fancybox.close();
         }

         function ChartOpen() {
             window.parent.document.getElementById('hlChart').href = document.getElementById("hfChartPath").value;
             parent.document.getElementById("hfSectionType").value = '5';
             parent.$.fancybox.close();
         }


     </script>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfTextPath" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfHTMLPath" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfTablePath" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfPhotoPath" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfChartPath" />
    <table>
            <tr>
                <td>
                    <%--<asp:HyperLink runat="server" ID="hlText" onclick="$.fancybox.close();"
                        NavigateUrl="#">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/images/text_big.png" />
                    </asp:HyperLink>--%>
                    <asp:ImageButton runat="server" ImageUrl="~/App_Themes/Default/images/text.gif"  Width="75px" OnClientClick="TextOpen();" />
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="75px" ImageUrl="~/App_Themes/Default/images/photo.png"   OnClientClick="PhotoOpen();" />
                     <br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="75px" ImageUrl="~/App_Themes/Default/images/table_big.png" OnClientClick="TableOpen();" />
                     <br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton ID="ImageButton4" runat="server" Width="75px" ImageUrl="~/App_Themes/Default/images/chart.png"  OnClientClick="ChartOpen();" />
                     <br />
                </td>
            </tr>
            <tr>
                <td>
                  
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="75px" ImageUrl="~/App_Themes/Default/images/html.png" OnClientClick="HTMLOpen();" />
                    <br />
                </td>
            </tr>
             <tr>
                 <td>
                     <div runat="server" id="div1">
                         <table cellpadding="0" cellspacing="0">
                             <tr>
                                 <td class="bL">
                                     &nbsp;
                                 </td>
                                 <td class="bC">
                                     <asp:LinkButton runat="server" ID="CancelButton" CssClass="ButtonLink" CausesValidation="false"
                                         OnClientClick=" parent.$.fancybox.close();return false;" Text="Close" />
                                 </td>
                                 <td class="bR">
                                     &nbsp;
                                 </td>
                             </tr>
                         </table>
                     </div>
                 </td>
             </tr>
        </table>

</asp:Content>

