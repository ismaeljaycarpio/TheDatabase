<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="DashBoardDetail.aspx.cs" Inherits="Pages_Home_DashBoardDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
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
    </script>

    <script type="text/javascript">
        function MouseEvents(objRef, evt) {
            if (evt.type == "mouseover") {
                objRef.style.backgroundColor = "#76BAF2";
                objRef.style.cursor = 'pointer';
            }
            else {

                if (evt.type == "mouseout") {
                    if (objRef.rowIndex % 2 == 0) {
                        //Alternating Row Color
                        objRef.style.backgroundColor = "white";
                    }
                    else {
                        objRef.style.backgroundColor = "#DCF2F0";
                    }
                }
            }
        }



    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
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
                                <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                    <ProgressTemplate>
                                        <table style="width: 100%; text-align: center">
                                            <tr>
                                                <td>
                                                   <asp:Image ID="Image1" runat="server" AlternateText="Processing..."  ImageUrl="~/Images/ajax.gif"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>--%>
                            </div>
                        </td>
                        <td>
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
                                        <div runat="server" id="divSave">
                                            <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="true"
                                                >
                                                <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                            </asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">
                                    <tr runat="server" id="trDashboard">
                                        <td>

                                        </td>
                                        <td>
                                             <asp:DropDownList ID="ddlDashboard" CausesValidation="false" Font-Bold="true" runat="server" AutoPostBack="true" DataTextField="DocumentText" Style="max-width: 400px;"
                                                    DataValueField="DocumentID" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlDashboard_SelectedIndexChanged">
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Dashboard Name:</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDashboardName" runat="server" Width="400px" CssClass="NormalTextBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDashboardName" runat="server" ControlToValidate="txtDashboardName"
                                                ErrorMessage="*"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ForeColor="Red" Display="Dynamic" ID="revDashboardName" runat="server"
                                            ControlToValidate="txtDashboardName" ErrorMessage="No spaces or non-ascii characters only"
                                            ValidationExpression="^[A-Za-z0-9_]+$">
                                        </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trLink" visible="false">
                                        <td align="right">
                                            <strong>Link:</strong>
                                        </td>
                                        <td>
                                           <asp:Label runat="server" ID="lblLink"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr >
                                          <td align="left" colspan="2" style="padding-top:20px;">
                                            <asp:Label runat="server" ID="lblLisOfUsers" Font-Bold="true" Text="Users"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trUsers">
                                        <td align="left" colspan="2" style="padding-left:10px;">
                                              
                                            <asp:GridView ID="grdUser"  GridLines="Both" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
                                                 RowStyle-CssClass="gridview_row" CellSpacing="5"  AlternatingRowStyle-BackColor="#DCF2F0"
                                                HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left" CssClass="gridview" OnRowDataBound="grdUser_RowDataBound"  >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="First Name">
                                                        <ItemTemplate>
                                                            <%--<asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("FirstName")%>'></asp:Label>--%>
                                                            <asp:HyperLink runat="server" ID="hlFirstName" Text='<%# Eval("FirstName")%>' ></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Name" >
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblLastName" Text='<%# Eval("LastName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Role"   HeaderText="Role" />
                                                   <%-- <asp:BoundField DataField="PhoneNumber"   HeaderText="Phone Number" />
                                                    <asp:BoundField DataField="Email"   HeaderText="Email" />--%>
                                                </Columns>
                                                <HeaderStyle CssClass="gridview_header" />
                                                 <RowStyle CssClass="gridview_row" />
                                                    <AlternatingRowStyle CssClass="gridview_row" />
                                                <EmptyDataTemplate>No users have been assigned this dashboard yet </EmptyDataTemplate>
                                            </asp:GridView>
                                        
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                          
                          <%--  <div>
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
                                               
                                                            <asp:HyperLink runat="server" ID="hlBack"  CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false">
                                                
                                                            <asp:HyperLink runat="server" ID="hlEditLink" CssClass="btn"> <strong>Edit</strong> </asp:HyperLink>
                                                       
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
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
</asp:Content>
