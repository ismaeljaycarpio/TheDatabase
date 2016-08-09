<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="AccountCreated.aspx.cs"
    Inherits="Page_AccountCreated" MasterPageFile="~/Home/Popup.master" %>

<%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <!-- Google Code for Demonstration Conversion Page -->
    <script type="text/javascript">
/* <![CDATA[ */
var google_conversion_id = 1006839338;
var google_conversion_language = "en";
var google_conversion_format = "1";
var google_conversion_color = "ffffff";
var google_conversion_label = "9PQbCParuAMQqsyM4AM";
var google_conversion_value = 0;
/* ]]> */
    </script>
    <script type="text/javascript" src="http://www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
        <div style="display: inline;">
            <img height="1" width="1" style="border-style: none;" alt="" src="http://www.googleadservices.com/pagead/conversion/1006839338/?value=0&amp;label=9PQbCParuAMQqsyM4AM&amp;guid=ON&amp;script=0" />
        </div>
    </noscript>
    <table class="ContentMain" style="width: 500px;">
        <tr>
            <td width="28">
            </td>
            <td>
                <h1>
                    <asp:Label runat="server" ID="lblHeading"></asp:Label>
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
                        <table style="text-align: right; width: 100%;">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblContentCommon"></asp:Label>
                                    <%--<DBGurus:DBGContent ID="dbgContentCommon" runat="server" ConnectionName="CnString"
                                        ContentKey="LoginScreenContent" TableName="Content" ExtenderPath="Extender/"
                                        ShowInlineContentEditor="false" UseAssetManager="true" />--%>
                                        <asp:HiddenField runat="server" ID="hfEmail" ClientIDMode="Static" Value="" />
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
                   
                       
                    <asp:LinkButton runat="server" ID="lnkBack" CssClass="btn" OnClientClick="GetBackValue();return false;"
                        CausesValidation="false"> <strong>  Back</strong> </asp:LinkButton>
                        
                    
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

<script language="javascript" type="text/javascript">
    function GetBackValue() {
        if (window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtLogInEmail') != null) {

            window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtLogInEmail').value = document.getElementById('hfEmail').value;
        }

        parent.$.fancybox.close();
    }
</script>

</asp:Content>
