<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="StyleAdvanced.aspx.cs" Inherits="Pages_DocGen_StyleAdvanced" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">

      
        function GetBackValue() {

            
            window.parent.document.getElementById('txtStyle').value = document.getElementById('txtAdvancedStyle').value;
            window.parent.document.getElementById('txtGenStyle').value = '';
            window.parent.ShowRecord();
           
            parent.$.fancybox.close();

        }

        
      


    </script>
    <div style="padding: 20px;">
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblTopTitle" CssClass="TopTitle" Text="Advanced Style"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAdvancedStyle" ClientIDMode="Static" CssClass="MultiLineTextBox"
                        TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <table>
                        <tr>
                            
                            <td>
                                <a class="btn" id="btnClose" href="#" onclick="javascript:parent.$.fancybox.close();">
                                    <strong>Close</strong></a>
                            </td>

                            <td>
                                <a class="btn" id="btnSave" href="#" onclick="GetBackValue();"><strong>Save</strong></a>
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:HiddenField runat="server" ID="hfTxtStyle" ClientIDMode="Static" Value="" />
                    <asp:HiddenField runat="server" ID="hfTxtGenStyle" ClientIDMode="Static" Value="" />

                     
                </td>
            </tr>
        </table>
    </div>

     
</asp:Content>
