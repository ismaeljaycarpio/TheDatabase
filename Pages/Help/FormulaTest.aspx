<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="FormulaTest.aspx.cs"
    Inherits="Page_Help_FormulaTest" MasterPageFile="~/Home/Popup.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
<script language="javascript" type="text/javascript">

    function GetBackValue(val) {

        var hfAdvanced = document.getElementById('hfAdvanced')
        if (val == 'valid') {

//            window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value = document.getElementById('txtValidation').value;
//            window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlValidEdit').href = '../Help/FormulaTest.aspx?type=valid&formula=' + encodeURIComponent(document.getElementById('txtValidation').value);
            //            parent.$.fancybox.close();

            if (hfAdvanced.value == 'yes') {
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value = document.getElementById('txtValidation').value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlValidEdit').href = '../Help/FormulaTest.aspx?type=valid&min=&max=&formula=' +
                    encodeURIComponent(document.getElementById('txtValidation').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMinValid').value = '';
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxValid').value = '';

                var x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry');
                x.style.display = 'block';

                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlValidEdit');
                x.style.display = 'block';

                x = window.parent.document.getElementById('divValidAdvanced');
                x.style.display = 'none';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced');
                x.style.display = 'none';


               // window.parent.$("#ctl00_HomeContentPlaceHolder_lblValidationEntry").text("Data Valid if");

            }

            if (hfAdvanced.value == 'no') {

                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value = '';
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced').href = '../Help/FormulaTest.aspx?type=valid&formula='
                + '&min=' + encodeURIComponent(document.getElementById('hfMin').value)
                + '&max=' + encodeURIComponent(document.getElementById('hfMax').value)
                +  "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMinValid').value = document.getElementById('hfMin').value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxValid').value = document.getElementById('hfMax').value;

                var x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry');
                x.style.display = 'none';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlValidEdit');
                x.style.display = 'none';
                x = window.parent.document.getElementById('divValidAdvanced');
                x.style.display = 'block';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced');
                x.style.display = 'block';

               // window.parent.$("#ctl00_HomeContentPlaceHolder_lblValidationEntry").text("Data Valid Between");

               
            }
            var chk = window.parent.document.getElementById('chkValidFormula');
            chk.checked = true;
            chk = window.parent.document.getElementById('chkValidConditions');
            chk.checked = false;
            parent.$.fancybox.close();


        }
        else if (val == 'warning') {
//            window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value = document.getElementById('txtValidation').value;
//            window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningEdit').href = '../Help/FormulaTest.aspx?type=warning&formula=' + encodeURIComponent(document.getElementById('txtValidation').value);
            //            parent.$.fancybox.close();
            

            if (hfAdvanced.value == 'yes') {
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value = document.getElementById('txtValidation').value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningEdit').href = '../Help/FormulaTest.aspx?type=warning&min=&max=&formula=' +
                    encodeURIComponent(document.getElementById('txtValidation').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMinWaring').value = '';

                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxWrning').value = '';


                var x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning');
                x.style.display = 'block';

                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningEdit');
                x.style.display = 'block';

                x = window.parent.document.getElementById('divWrningAdvanced');
                x.style.display = 'none';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningAdvanced');
                x.style.display = 'none';
                //window.parent.$("#ctl00_HomeContentPlaceHolder_lblWarningValidation").text("Data Warning if");

               // parent.$.fancybox.close();
                
            }

            if (hfAdvanced.value == 'no') {

                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value = '';
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningAdvanced').href = '../Help/FormulaTest.aspx?type=warning&formula='
                + '&min=' + encodeURIComponent(document.getElementById('hfMin').value)
                + '&max=' + encodeURIComponent(document.getElementById('hfMax').value)  + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMinWaring').value = document.getElementById('hfMin').value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxWrning').value = document.getElementById('hfMax').value;

                var x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning');
                x.style.display = 'none';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningEdit');
                x.style.display = 'none';
                x = window.parent.document.getElementById('divWrningAdvanced');
                x.style.display = 'block';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningAdvanced');
                x.style.display = 'block';
                
                //window.parent.$("#ctl00_HomeContentPlaceHolder_lblWarningValidation").text("Data Warning if outside the range");

               // parent.$.fancybox.close();
            }
            var chk = window.parent.document.getElementById('chkWarningFormula');
            chk.checked = true;
            chk = window.parent.document.getElementById('chkWarningConditions');
            chk.checked = false;
            parent.$.fancybox.close();
        }
        else
        {
            if (hfAdvanced.value == 'yes') {
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').value = document.getElementById('txtValidation').value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceEdit').href = '../Help/FormulaTest.aspx?type=Exceedance&min=&max=&formula=' +
                    encodeURIComponent(document.getElementById('txtValidation').value) +  "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMinExceedance').value = '';

                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxExceedance').value = '';


                var x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance');
                x.style.display = 'block';

                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceEdit');
                x.style.display = 'block';

                x = window.parent.document.getElementById('divExceedanceAdvanced');
                x.style.display = 'none';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced');
                x.style.display = 'none';
               // window.parent.$("#ctl00_HomeContentPlaceHolder_lblExceedanceValidation").text("Data Exceedance if");

                //parent.$.fancybox.close();

            }

            if (hfAdvanced.value == 'no') {

                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').value = '';
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced').href = '../Help/FormulaTest.aspx?type=exceedance&formula='
                + '&min=' + encodeURIComponent(document.getElementById('hfMin').value)
                + '&max=' + encodeURIComponent(document.getElementById('hfMax').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMinExceedance').value = document.getElementById('hfMin').value;
                window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxExceedance').value = document.getElementById('hfMax').value;

                var x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance');
                x.style.display = 'none';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceEdit');
                x.style.display = 'none';
                x = window.parent.document.getElementById('divExceedanceAdvanced');
                x.style.display = 'block';
                x = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced');
                x.style.display = 'block';

               // window.parent.$("#ctl00_HomeContentPlaceHolder_lblExceedanceValidation").text("Data Exceedance if outside the range");

                //parent.$.fancybox.close();
            }
            var chk = window.parent.document.getElementById('chkExceedanceFormula');
            chk.checked = true;
            chk = window.parent.document.getElementById('chkExceedanceConditions');
            chk.checked = false;
            parent.$.fancybox.close();

        }
//        window.close();

    }

  

    


    </script>

<div style="min-width:500px; min-height:500px; padding-top:10px;">


    <asp:Label ID="lblValidationType" runat="server" Text="Data Validation" Width="400px" CssClass="TopTitle"> </asp:Label>

    <br />
    &nbsp;<asp:Label runat="server" ID="lblSubTitle" Font-Size="Small"></asp:Label><br /><br />
                <table cellpadding="3">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="server" Text="Formula:" Font-Bold="true"></asp:Label>
                            
                            <asp:HiddenField runat="server" ID="hfTableID" />
                            <asp:HiddenField runat="server" ID="hfColumnID" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtValidation" runat="server" Height="111px" TextMode="MultiLine"  ClientIDMode="Static"
                                Width="400px" CssClass="MultiLineTextBox"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hfMin" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hfMax" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hfAdvanced" ClientIDMode="Static" Value="yes" />
                                
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label2" runat="server" Text="Data:" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="padding-left: 0px;">
                            <table style="padding-left: 0px;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtData" runat="server" Width="345px" CssClass="NormalTextBox"></asp:TextBox>
                                       
                                    </td>
                                    <td style="padding-left:10px;">
                                        <div>
                                            
                                                        <asp:LinkButton runat="server" ID="lnkTest" OnClick="lnkTest_Click" CssClass="btn"
                                                            CausesValidation="true"><strong>Test</strong></asp:LinkButton>
                                                   
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left">
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label3" runat="server" Text="Result:" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblResult" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="height: 30px;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td colspan="3">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lblMessage" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trConfirmation" visible="false">
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div runat="server" id="div1">
                                                       
                                                                    <asp:LinkButton runat="server" ID="lnkOk" CssClass="btn"
                                                                     CausesValidation="true"> <strong>Ok</strong></asp:LinkButton>
                                                               
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        
                                                                    <asp:LinkButton runat="server" ID="lnkNo" OnClick="lnkNo_Click" CssClass="btn"
                                                                        CausesValidation="false"> <strong>No</strong></asp:LinkButton>
                                                              
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="trMainSave">
                                   
                                    <td>
                                        <div>
                                          
                                                        <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                                            CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                                 
                                        </div>
                                    </td>

                                     <td>
                                        <div runat="server" id="divSave">
                                           
                                                        <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" CausesValidation="true"
                                                            OnClick="lnkSave_Click"> <strong>Save</strong></asp:LinkButton>
                                                  
                                        </div>
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="text-align: right;" valign="top">
                            <asp:Label ID="Label4" runat="server" Text="Help" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="3" style="text-align: left; width: 600px;">
                            <%--<DBGurus:DBGContent ID="dbgContentCommon" runat="server" ConnectionName="CnString"
                                ContentKey="ValidationHelp" TableName="Content" ExtenderPath="Extender/" ShowInlineContentEditor="false"
                                UseAssetManager="true" />--%>
                                <asp:Label runat="server" ID="lblContentCommon"></asp:Label>
                        </td>
                    </tr>
                </table>
            
</div>
</asp:Content>


