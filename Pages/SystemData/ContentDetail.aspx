<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ContentDetail.aspx.cs" Inherits="Pages_SystemData_ContentDetail" ValidateRequest="false" %>

<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%-- <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"      type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>

     <%--<script type="text/javascript" src="<%=ResolveUrl("~/Script/pasteimage.js")%>"></script>--%>

    <script language="JavaScript">
        var ids = new Array('ctl00_HomeContentPlaceHolder_txtContentKey',
    'ctl00_HomeContentPlaceHolder_txtHeading',
    'idContentoEdit_ctl00_HomeContentPlaceHolder_edtContent', 'ctl00_HomeContentPlaceHolder_txtStoredProcedure');
        var values = new Array('', '', '', '');

        function populateArrays() {
            // assign the default values to the items in the values array
            for (var i = 0; i < ids.length; i++) {
                var elem = document.getElementById(ids[i]);
                if (elem)
                    if (elem.type == 'checkbox' || elem.type == 'radio')
                        values[i] = elem.checked;
                    else
                        values[i] = elem.value;
            }
        }



        var needToConfirm = true;

        window.onbeforeunload = confirmExit;
        function confirmExit() {
            if (needToConfirm) {
                // check to see if any changes to the data entry fields have been made
                for (var i = 0; i < values.length; i++) {
                    var elem = document.getElementById(ids[i]);
                    if (elem)
                        if ((elem.type == 'checkbox' || elem.type == 'radio')
                  && values[i] != elem.checked)
                            return "You have attempted to leave this page.  If you have made any changes to the fields without clicking the Save button, your changes will be lost.  Are you sure you want to exit this page?";
                        else if (!(elem.type == 'checkbox' || elem.type == 'radio') &&
                  elem.value != values[i])
                            return "You have attempted to leave this page.  If you have made any changes to the fields without clicking the Save button, your changes will be lost.  Are you sure you want to exit this page?";
                }

                // no changes - return nothing      
            }
        }
    </script>


         <%--<script type="text/javascript">

             // We start by checking if the browser supports the 
             // Clipboard object. If not, we need to create a 
             // contenteditable element that catches all pasted data 
             if (!window.Clipboard) {
                 var pasteCatcher = document.createElement("div");

                 // Firefox allows images to be pasted into contenteditable elements
                 pasteCatcher.setAttribute("contenteditable", "");

                 // We can hide the element and append it to the body,
                 pasteCatcher.style.opacity = 0;
                 document.body.appendChild(pasteCatcher);

                 // as long as we make sure it is always in focus
                 pasteCatcher.focus();
                 document.addEventListener("click", function () { pasteCatcher.focus(); });
             }
             // Add the paste event listener
             window.addEventListener("paste", pasteHandler);

             /* Handle paste events */
             function pasteHandler(e) {
                 // We need to check if event.clipboardData is supported (Chrome)
                 if (e.clipboardData) {
                     // Get the items from the clipboard
                     var items = e.clipboardData.items;
                     if (items) {
                         // Loop through all items, looking for any kind of image
                         for (var i = 0; i < items.length; i++) {
                             if (items[i].type.indexOf("image") !== -1) {
                                 // We need to represent the image as a file,
                                 var blob = items[i].getAsFile();
                                 // and use a URL or webkitURL (whichever is available to the browser)
                                 // to create a temporary URL to the object
                                 var URLObj = window.URL || window.webkitURL;
                                 var source = URLObj.createObjectURL(blob);

                                 // The URL can then be used as the source of an image
                                 createImage(source);
                             }
                         }
                     }
                     // If we can't handle clipboard data directly (Firefox), 
                     // we need to read what was pasted from the contenteditable element
                 } else {
                     // This is a cheap trick to make sure we read the data
                     // AFTER it has been inserted.
                     setTimeout(checkInput, 1);
                 }
             }

             /* Parse the input in the paste catcher element */
             function checkInput() {
                 // Store the pasted content in a variable
                 var child = pasteCatcher.childNodes[0];

                 // Clear the inner html to make sure we're always
                 // getting the latest inserted content
                 pasteCatcher.innerHTML = "";

                 if (child) {
                     // If the user pastes an image, the src attribute
                     // will represent the image as a base64 encoded string.
                     if (child.tagName === "IMG") {
                         createImage(child.src);
                     }
                 }
             }

             /* Creates a new image from a given source */
             function createImage(source) {
                 var pastedImage = new Image();
                 pastedImage.onload = function () {
                     // You now have the image!
                 }
                 pastedImage.src = source;
             }

     </script>--%>   
    <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
        <div runat="server" id="divDetail">
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
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
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <asp:HyperLink runat="server" ID="hlBack">
                                                                        <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                                                    </asp:HyperLink>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divEdit" visible="false">
                                                                    <asp:HyperLink runat="server" ID="hlEditLink">
                                                                        <asp:Image runat="server" ID="Image2"  ImageUrl="~/App_Themes/Default/images/Edit_big.png"  ToolTip="Edit" />
                                                                    </asp:HyperLink>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divDelete" visible="false">
                                                                    <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="javascript:needToConfirm = false;return confirm('Are you sure you want to delete this Content?')"
                                                                        CausesValidation="false" OnClick="lnkDelete_Click">
                                                                        <asp:Image runat="server" ID="Image3"  ImageUrl="~/App_Themes/Default/images/delete_big.png"  ToolTip="Delete" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divSave">
                                                                    <asp:LinkButton runat="server" ID="lnkSave" OnClientClick="needToConfirm = false;"
                                                                        OnClick="lnkSave_Click" CausesValidation="true">
                                                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td style="padding-left:100px;">
                                                <asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static" NavigateUrl="~/Pages/Help/Help.aspx?contentkey=ContentHelp">
                                                    <asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/Default/images/help.png"  />
                                                </asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="padding-left: 20px;">
                                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
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
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <div>
                            <table cellpadding="3">
                                <tr runat="server" id="trOnlyGlobalUser">
                                    <td align="left" colspan="2">
                                        <asp:CheckBox runat="server" TextAlign="Right" ID="chkAllAccount" Text="Template"
                                            Font-Bold="true" />
                                        <%--<asp:CheckBox runat="server" ID="chkForceUpdate" Font-Bold="true" Text="Force update to all account"
                                        Visible="false" />--%>
                                        <asp:CheckBox runat="server" TextAlign="Right" ID="chkOnlyGlobal" Text="Only Global"
                                            Font-Bold="true" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr runat="server" id="trContentKey">
                                    <td align="left" style="width: 100px;" colspan="2">
                                        <strong>Content Key*</strong>
                                        <asp:TextBox ID="txtContentKey" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvContentKey" runat="server" ControlToValidate="txtContentKey"
                                            ErrorMessage="Content Key - Required"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                     
                                    <td align="left">
                                        <strong>Heading</strong>
                                    </td>
                                   
                                    <td  align="left">
                                            <strong>Type</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" >
                                        <asp:TextBox ID="txtHeading" runat="server" Width="500px" CssClass="NormalTextBox"
                                            MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td  align="left">
                                         <asp:DropDownList runat="server" ID="ddlContentType" DataTextField="ContentTypeName"
                                             DataValueField="ContentTypeID"  AutoPostBack="false" CssClass="NormalTextBox"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <strong>Content</strong>
                                    </td>
                                    <td align="center">
                                        <table>
                                            <tr runat="server" id="trDatabaseField" visible="false">
                                                <td align="right">
                                                    <strong>Database Value</strong>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlDatabaseField" CssClass="NormalTextBox" DataTextField="Text"
                                                                    DataValueField="Value" ToolTip="Select database value and then click Add to add it to your content.">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="div1">
                                                                    <asp:LinkButton runat="server" ID="lnlAddDataBaseField" CssClass="btn" OnClientClick="InsertMergeField(); return false;"
                                                                        CausesValidation="true"> <strong>Add</strong></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="lnlAddDataBaseFieldText" CssClass="btn" OnClientClick=" insertAtCaret(); return false;"
                                                                        CausesValidation="true" Visible="false"> <strong>Add</strong></asp:LinkButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <editor:WYSIWYGEditor runat="server" scriptPath="../../Editor/scripts/" ID="edtContent"
                                            btnStyles="true" btnSave="false" EditorHeight="400" Height="400" EditorWidth="610"
                                            Width="900" AssetManager="../../assetmanager/assetmanager.aspx" AssetManagerWidth="550"
                                            AssetManagerHeight="400" Visible="true" ToolbarMode="0" btnPreview="False"
                    btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False" />
                                        <asp:TextBox runat="server" ID="txtContent" Width="800" Height="200" TextMode="MultiLine"
                                            CssClass="MultiLineTextBox" Visible="false" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%-- <td valign="top">
                                        <div runat="server" id="divToken" visible="false">
                                            <table>
                                                <tr style="height:90px;"><td></td></tr>
                                                <tr>

                                                    <td>
                                                        <div>
                                                            <asp:GridView ID="grdToken" runat="server" AutoGenerateColumns="false" CssClass="gridview">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Database Values" HeaderStyle-BorderStyle="None" >
                                                                        <ItemTemplate>                                                                            
                                                                                <asp:TextBox runat="server" Width="200px" Text='<%# Container.DataItem %>' BorderStyle="None" ReadOnly="true" 
                                                                                CssClass="NormalTextBox"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>


                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">To add a database value to your content copy and paste the text (including the square brackets) into the content or drag and drop it in.</td>
                                                </tr>
                                            </table>
                                        
                                        </div>
                                
                                </td>--%>
                                </tr>
                                <tr runat="server" id="trSP" visible="false">
                                    <td align="right">
                                        <strong>Stored Procedure</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStoredProcedure" runat="server" Width="150px" CssClass="NormalTextBox"
                                            MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <span style="font-weight: bold" align="center"></span>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <script type="text/javascript">
        populateArrays();
    </script>
    <script type="text/javascript">
        var oEditor = null;
        function InsertMergeField() {
            oUtil.obj.insertHTML("[" + document.getElementById("ctl00_HomeContentPlaceHolder_ddlDatabaseField").value + "]")
        }

        function insertAtCaret() {
            var txtarea = document.getElementById('txtContent');
            var text = "[" + document.getElementById('ctl00_HomeContentPlaceHolder_ddlDatabaseField').value + "]";
            var scrollPos = txtarea.scrollTop;
            var strPos = 0;
            var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ? "ff" : (document.selection ? "ie" : false));
            if (br == "ie") {
                txtarea.focus();
                var range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length); strPos = range.text.length;
            }
            else if (br == "ff") strPos = txtarea.selectionStart;
            var front = (txtarea.value).substring(0, strPos);
            var back = (txtarea.value).substring(strPos, txtarea.value.length);
            txtarea.value = front + text + back; strPos = strPos + text.length;
            if (br == "ie") {
                txtarea.focus();
                var range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length);
                range.moveStart('character', strPos);
                range.moveEnd('character', 0);
                range.select();
            }
            else if (br == "ff") {
                txtarea.selectionStart = strPos;
                txtarea.selectionEnd = strPos; txtarea.focus();
            }
            txtarea.scrollTop = scrollPos;
        }


        //function paste(src) {
        //    oUtil.obj.insertHTML("<img src='" + src + "'>");
        //}

        //$(function () {
        //    $.pasteimage(paste);
        //});

    </script>


</asp:Content>
