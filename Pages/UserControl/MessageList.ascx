<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageList.ascx.cs" 
    Inherits="Pages_UserControl_MessageList" %>


<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>



   <script type="text/javascript">

       function abc() {
           var b = document.getElementById('<%= lnkSearch.ClientID %>');
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

       //function MouseEvents(objRef, evt) {
       //    var checkbox = objRef.getElementsByTagName("input")[0];



       //    if (evt.type == "mouseover") {
       //        objRef.style.backgroundColor = "#76BAF2";
       //        objRef.style.cursor = 'pointer';
       //    }
       //    else {
       //        //            if (checkbox == null) {
       //        //                return;
       //        //            }

       //        if (checkbox != null && checkbox.checked) {
       //            objRef.style.backgroundColor = "#96FFFF";
       //        }
       //        else if (evt.type == "mouseout") {
       //            if (objRef.rowIndex % 2 == 0) {
       //                //Alternating Row Color
       //                objRef.style.backgroundColor = "white";
       //            }
       //            else {
       //                objRef.style.backgroundColor = "#DCF2F0";
       //            }
       //        }
       //    }
       //}


       //function Check_Click(objRef) {
       //    //Get the Row based on checkbox
       //    var row = objRef.parentNode.parentNode;
       //    if (objRef.checked) {
       //        //If checked change color to Aqua
       //        row.style.backgroundColor = "#96FFFF";
       //    }
       //    else {
       //        //If not checked change back to original color
       //        if (row.rowIndex % 2 == 0) {
       //            //Alternating Row Color
       //            row.style.backgroundColor = "white";

       //        }
       //        else {
       //            row.style.backgroundColor = "#DCF2F0";
       //        }
       //    }

       //    //Get the reference of GridView
       //    var GridView = row.parentNode;

       //    //Get all input elements in Gridview
       //    var inputList = GridView.getElementsByTagName("input");

       //    for (var i = 0; i < inputList.length; i++) {
       //        //The First element is the Header Checkbox
       //        var headerCheckBox = inputList[0];

       //        //Based on all or none checkboxes
       //        //are checked check/uncheck Header Checkbox
       //        var checked = true;
       //        if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
       //            if (!inputList[i].checked) {
       //                checked = false;
       //                break;
       //            }
       //        }
       //    }
       //    headerCheckBox.checked = checked;

       //}

       //function checkAll(objRef) {
       //    var GridView = objRef.parentNode.parentNode.parentNode;
       //    var inputList = GridView.getElementsByTagName("input");
       //    for (var i = 0; i < inputList.length; i++) {
       //        //Get the Cell To find out ColumnIndex
       //        var row = inputList[i].parentNode.parentNode;
       //        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
       //            if (objRef.checked) {
       //                //If the header checkbox is checked
       //                //check all checkboxes
       //                //and highlight all rows
       //                row.style.backgroundColor = "#96FFFF";
       //                inputList[i].checked = true;
       //            }
       //            else {
       //                //If the header checkbox is checked
       //                //uncheck all checkboxes
       //                //and change rowcolor back to original 
       //                if (row.rowIndex % 2 == 0) {
       //                    //Alternating Row Color
       //                    row.style.backgroundColor = "white";

       //                }
       //                else {
       //                    row.style.backgroundColor = "#DCF2F0";
       //                }
       //                inputList[i].checked = false;
       //            }
       //        }
       //    }
       //}

    </script>

    <script language="javascript" type="text/javascript">
       <%-- function abc() {
            var b = document.getElementById('<%= lnkSearch.ClientID %>');
            if (b && typeof (b.click) == 'undefined') {
                b.click = function () {
                    var result = true;
                    if (b.onclick) result = b.onclick();
                    if (typeof (result) == 'undefined' || result) {
                        eval(b.getAttribute('href'));
                    }
                }
            }

        }--%>

        function SelectAllCheckboxes(spanChk) {
            checkAll(spanChk);
            // Added as ASPX uses SPAN for checkbox

            var oItem = spanChk.children;

            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];

            xState = theBox.checked;

            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)

                if (elm[i].type == "checkbox" && elm[i].id != theBox.id && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkIsActive' && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkShowOnlyWarning' && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkShowAdvancedOptions') {

                    //elm[i].click();

                    if (elm[i].checked != xState)

                        elm[i].click();

                    //elm[i].checked=xState;

                }

        }


    </script>

   

        <asp:UpdatePanel ID="upMsg" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0"  align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr runat="server" visible="false">
                                <td align="left" style="width: 50%;">
                                    <span class="TopTitle">Messages</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" >
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <img alt="Processing..." src="../../Images/ajax.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>--%>
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
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch" >
                            <div id="search"   class="searchcorner" onkeypress="abc();" > <%----%>
                                <br />
                                <table style="border-collapse: collapse" cellpadding="3">
                                    <tr>
                                        <td >
                                                <br />
                                           <strong>Search</strong>
                                            
                                        </td>
                                        <td runat="server" id="tdTableFilter" visible="false">
                                             Table <br />
                                            <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                            <asp:HiddenField runat="server" ID="hfRecordID" />
                                        </td>


                                        <td style="vertical-align:bottom;">Date and Time
                                            <br />
                                          
                                            <table cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtLowerDate" Width="90px" CssClass="NormalTextBox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="ibLowerDate" ImageUrl="~/Images/Calendar.png"
                                                            AlternateText="Click to show calendar" CausesValidation="false" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtLowerTime" Width="50px" CssClass="NormalTextBox"
                                                            ToolTip="Time"></asp:TextBox>
                                                        <ajaxtoolkit:calendarextender id="ce_txtLowerDate" runat="server" targetcontrolid="txtLowerDate"
                                                            format="dd/MM/yyyy" popupbuttonid="ibLowerDate" firstdayofweek="Monday">
                            </ajaxtoolkit:calendarextender>
                                                        <ajaxtoolkit:maskededitextender runat="server" id="meeLowerTime" targetcontrolid="txtLowerTime"
                                                            autocompletevalue="00:00" masktype="Time" mask="99:99"></ajaxtoolkit:maskededitextender>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trUpperMessageDate">
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtUpperDate" Width="90px" CssClass="NormalTextBox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="ibUpperDate" ImageUrl="~/Images/Calendar.png"
                                                            AlternateText="Click to show calendar" CausesValidation="false" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtUpperTime" Width="50px" CssClass="NormalTextBox"
                                                            ToolTip="Time"></asp:TextBox>

                                                        <ajaxtoolkit:calendarextender id="ce_txtUpperDate" runat="server" targetcontrolid="txtUpperDate"
                                                            format="dd/MM/yyyy" popupbuttonid="ibUpperDate" firstdayofweek="Monday">
                            </ajaxtoolkit:calendarextender>
                                                        <ajaxtoolkit:textboxwatermarkextender id="TextBoxWatermarkExtender2" targetcontrolid="txtUpperDate"
                                                            watermarktext="Upper" runat="server">
                                                        </ajaxtoolkit:textboxwatermarkextender>
                                                        <ajaxtoolkit:maskededitextender runat="server" id="meeUpperTime" targetcontrolid="txtUpperTime"
                                                            autocompletevalue="00:00" masktype="Time" mask="99:99"></ajaxtoolkit:maskededitextender>
                                                    </td>
                                                </tr>

                                            </table>

                                        </td>
                                        <td style="vertical-align:bottom;">
                                           Message type <br />
                                            <asp:DropDownList runat="server" ID="ddlMessageType" CssClass="NormalTextBox"  AutoPostBack="true" OnSelectedIndexChanged="lnkSearch_Click">
                                                <asp:ListItem Text="Message" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Warning" Value="W"></asp:ListItem>
                                                <asp:ListItem Text="General Email" Value="E"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="vertical-align:bottom;">
                                             Direction <br />
                                            <asp:DropDownList runat="server" ID="ddlIsIncoming" CssClass="NormalTextBox" AutoPostBack="true" OnSelectedIndexChanged="lnkSearch_Click">
                                                <asp:ListItem Text="--Both--" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Incoming" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="Outgoing" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="vertical-align:bottom;">
                                            Other Party <br />
                                            <asp:TextBox runat="server" ID="txtOtherparty" CssClass="NormalTextBox" ></asp:TextBox>
                                        </td>
                                        <td style="vertical-align:bottom;">
                                            Subject <br />
                                            <asp:TextBox runat="server" ID="txtSubject" CssClass="NormalTextBox" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <div>
                                              
                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                              
                                                            <asp:LinkButton runat="server" ID="lnkReset" CssClass="btn" OnClick="lnkReset_Click"> <strong>Reset</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview" EmptyDataText="No messages."
                             HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            AllowPaging="True" AllowSorting="True" DataKeyNames="MessageID" HeaderStyle-ForeColor="Black"
                            Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                            OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound"
                             AlternatingRowStyle-BackColor="#DCF2F0">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("MessageID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <%--<asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                            <input id="chkAll" onclick="javascript: SelectAllCheckboxes(this);" runat="server"
                                            type="checkbox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlViewMessage" runat="server" ToolTip="View Message"  CssClass="popupmessage" 
                                            ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                

                                <asp:TemplateField HeaderText="Table"  Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTable" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField SortExpression="DateTime" HeaderText="Date and Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateTime" runat="server" Text='<%# Eval("DateTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField SortExpression="MessageType" HeaderText="Message Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMessageType" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField SortExpression="DeliveryMethod" HeaderText="Delivery Method">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliveryMethod" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField SortExpression="IsIncoming" HeaderText="Direction">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsIncoming" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField SortExpression="OtherParty" HeaderText="Other Party">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOtherParty" runat="server" Text='<%# Eval("OtherParty") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField SortExpression="Subject" HeaderText="Subject">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField  HeaderText="Body">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBody" runat="server" Text='<%# Eval("Body") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                               
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" HideAdd="true" HideRefresh="true" 
                                    OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport" HideFilter="true" 
                                    HideAllExport="true" HideExport="true" HideExcelExport="false"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" OnExportForExcel="Pager_OnExportForExcel"  />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                    
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
        </Triggers>
    </asp:UpdatePanel>


