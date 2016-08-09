<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" ValidateRequest="false"
    CodeFile="DocumentDetail.aspx.cs" Inherits="Pages_Document_DocumentDetail" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
   <script src="Uploadify/jquery.uploadify.v2.1.4.js" type="text/javascript"></script>
    <script src="Uploadify/swfobject.js" type="text/javascript"></script>
    <link href="Uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
                        
     

       // var theFolder = "..\\..\\UserFiles\\Documents";



        $(document).ready(function () {

            var strscript = document.getElementById("hfRootURL").value + '/Pages/Document/Handler.ashx';

            $('#file_upload').uploadify({
                'uploader': 'uploadify/uploadify.swf',
                'script': strscript,
                'cancelImg': 'uploadify/cancel.png',
                'auto': true,
                'multi': false,
                'fileDesc': 'Files',
                'fileExt': '*.*',
                'queueSizeLimit': 90,
                'sizeLimit': 1000000000,
                'buttonText': 'Browse...',
                'scriptAccess': 'always',
                'folder': 'UserFiles\\Documents',
                'onComplete': function (event, queueID, fileObj, response, data) {
                    document.getElementById("hfFileName").value = response;
                    $('#ctl00_HomeContentPlaceHolder_lblFileName').html(fileObj.name);
                    //alert(fileObj.name + ' File has been uploaded.');
                    //window.location = document.getElementById("hfDocURL").value;
                },
                'onSelect': function (event, ID, fileObj) {
                    // Update selected so we know they have selected a file 
                    $('#ctl00_HomeContentPlaceHolder_lblFileName').html('');
                    $("#hfIsFileUploaded").val('yes');
                    $('#file_upload').uploadifySettings(
                                    'scriptData', { 'foo': 'UserFiles\\Documents', 'AccountID': document.getElementById("hfAccountID").value }
                             );
                    $('#file_upload').uploadifyUpload();
                    //SaveDocument();

                },
                'onError': function (event, ID, fileObj, errorObj) {
                    // error info 
                    alert("ERROR: " + errorObj.info);
                },
                'onCancel': function (event, ID, fileObj, data) {
                    // Update selected so we know they have no file selected
                    $("#hfIsFileUploaded").val('');

                }

            });


        });

      

        function Submit() {

            document.forms["aspnetForm"].submit()
            document.getElementById("hfFileName").value=this.value;
        };

        function GoBack()
        {
            alert( document.getElementById("hfFileName").value + ' File has been uploaded.');
             window.location = document.getElementById("hfDocURL").value;

        };

        function ConfirmDelete() {
            if (confirm('Are you sure that you want to delete this record?')) {
                return true;
            }
            return false;
        };
        function HideUplodify() {
            var x = document.getElementById("trUploadControl");
            x.style.display = 'none';
        };

        function SaveDocument() {
          

            var selected = $("#hfIsFileUploaded").val();

            if (selected == 'yes') {

                                $('#file_upload').uploadifySettings( 
                                    'scriptData', { 'foo': 'UserFiles\\Documents', 'ActionMode': document.getElementById("hfActionMode").value, 'DocumentID': document.getElementById("hfDocumentID").value, 'DocumentDate': document.getElementById("txtDocumentDate").value, 'DocumentTypeID': document.getElementById("ddlDocumentType").value, 'DocumentText': document.getElementById("txtDocumentText").value, 'AccountID': document.getElementById("hfAccountID").value, 'UserID': document.getElementById("hfUserID").value, 'TableID': '-1', 'FolderID': document.getElementById("hfFolderID").value }
                             );

                                $('#file_upload').uploadifyUpload();

                               // alert('save document method');


                return false;
            }
            else {

                if (document.getElementById("hfActionMode").value == 'add') {
                    alert('Please select a document.');
                    return false;
                }
                else {
                    return true;
                }

            }

        };

        function UserBasic() {
            var y = document.getElementById('divNoFlash');
            y.style.display = 'block';

            y = document.getElementById('ctl00_HomeContentPlaceHolder_divUploadButton');
            y.style.display = 'none';

            y = document.getElementById('lnkUseBasic');
            y.style.display = 'none';

        }
        
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
                <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                    <div runat="server" id="divDetail">
                        <table cellpadding="3">
                            <tr>
                                <td align="right">
                                    <strong>Document Date*</strong>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtDocumentDate" 
                                       Width="100px" CssClass="NormalTextBox" ValidationGroup="MKE" 
                                         BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                         <asp:ImageButton runat="server" ID="ibDocumentDate"  ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false"/>
                                    <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDocumentDate"
                                        Format="dd/MM/yyyy" PopupButtonID="ibDocumentDate"  FirstDayOfWeek="Monday">
                                    </ajaxToolkit:CalendarExtender>
                                    
                                    <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDocumentDate"
                                        ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                        MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDocumentDate"
                                        ErrorMessage="Document Date - Required"></asp:RequiredFieldValidator>
                                     <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtDocumentDate" WatermarkText="dd/mm/yyyy"
                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Category</strong>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlDocumentType" AutoPostBack="false" ClientIDMode="Static"
                                        DataValueField="DocumentTypeID" DataTextField="DocumentTypeName" CssClass="NormalTextBox">
                                    </asp:DropDownList>
                                    <asp:HyperLink runat="server" ID="hlDocumentTypeEdit" Text="Edit" NavigateUrl="~/Pages/Document/DocumentType.aspx"></asp:HyperLink>
                                </td>
                            </tr>
                            <%--<tr runat="server" id="trSingleTable">
                                <td align="right">
                                    <strong>Table</strong>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAllTable" runat="server" DataTextField="TableName"
                                        ClientIDMode="Static" DataValueField="TableID" CssClass="NormalTextBox">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="right">
                                    <strong>Document Name</strong>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDocumentText" CssClass="NormalTextBox" ClientIDMode="Static"
                                        Width="650px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr runat="server" clientidmode="Static" id="trUploadControl">
                                <td align="right" valign="top">
                                    <strong>File Name*</strong>
                                </td>
                                <td>
                                    <div runat="server" id="divUploadButton" >
                                        <%--<asp:FileUpload runat="server" ID="file_upload" ClientIDMode="Static"  onchange="javascript:Submit()"  />--%>
                                        <input id="file_upload" name="file_upload" type="file"   />
                                       
                                    </div>
                                    <div id="divNoFlash" >
                                                <asp:FileUpload runat="server" ID="fuReadOnly" Visible="true"  />  <%-- onchange="javascript:Submit()"  --%>                  
                                    </div>
                                   
                                     <asp:FileUpload runat="server" ID="fuReadOnly5" Visible="false" 
                                      />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="left">
                                
                                    <asp:LinkButton runat="server" ID="lnkUseBasic" Text="Use basic version" ClientIDMode="Static"  
                                       OnClientClick="UserBasic();return false;" ></asp:LinkButton>
                                
                                </td>
                                
                            </tr>


                            <tr>
                                <td align="right" valign="top">
                                    <asp:Label runat="server" ID="lblFileNameCaption" Text="File Name*" Visible="true"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td valign="middle">
                                    <asp:Label runat="server" ID="lblFileName"  Width="500px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red" CssClass="NormalTextBox"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <div>
                                        <table>
                                            <tr>
                                                 <td>
                                                    <div>
                                                        <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>Back</strong></asp:HyperLink>
                                                    </div>
                                                </td>

                                               
                                                <td>
                                                    <div runat="server" id="divEdit" visible="false">
                                                        <asp:HyperLink runat="server" ID="hlEditLink" CssClass="btn"><strong>Edit</strong> </asp:HyperLink>
                                                    </div>
                                                </td>

                                                 <td>
                                                    <div runat="server" id="divSaveAll">
                                                        <asp:LinkButton runat="server" ID="lnkUploadTest" CssClass="btn" CausesValidation="true"
                                                             OnClick="lnkSave_Click"> <strong>Save </strong> </asp:LinkButton> <%--OnClientClick="javascript:return SaveDocument();"--%>
                                                    </div>
                                                </td>
                                               

                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <asp:HiddenField id="hfFileName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfIsFileUploaded" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfAccountID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfUserID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfActionMode" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfDocumentID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfDocURL" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField runat="server" ID="hfFolderID" Value="-1" ClientIDMode="Static" />
                     <asp:HiddenField ID="hfRootURL" runat="server" ClientIDMode="Static" />
                    <br />
                   <asp:Button runat="server" ID="lnkSave" name="lnkSave" CssClass="ButtonLink" Height="1" 
                                Width="1" BackColor="White" BorderColor="White" BorderStyle="None" 
                                                                           CausesValidation="true" 
                                ClientIDMode="Static" style="display:none;" > </asp:Button>
                </asp:Panel>
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
        if (swfobject.hasFlashPlayerVersion("1")) {
            //alert("You have flash!");
            var x = document.getElementById('divNoFlash');
            x.style.display = 'none';

            x = document.getElementById('ctl00_HomeContentPlaceHolder_divUploadButton');
            x.style.display = 'block';
        }
        else {
            var y = document.getElementById('divNoFlash');
            y.style.display = 'block';

            y = document.getElementById('ctl00_HomeContentPlaceHolder_divUploadButton');
            y.style.display = 'none';
        }
    </script>
</asp:Content>
