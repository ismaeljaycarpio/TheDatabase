<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="Detail.aspx.cs" Inherits="User_Detail" MaintainScrollPositionOnPostback="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
   
      <%--<link href="<%=ResolveUrl("~/Styles/jquery-ui-1.7.3.custom.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-ui-1.9.2.custom.min.js")%>"></script>--%>

    <style type="text/css">
        .headerlink a
        {
            text-decoration: none;
            color: Black;
            cursor: default;
        }
    </style>
    <script type="text/javascript">

        function SetFolder(iFolderID) {
            document.getElementById('hfParentFolderID').value = iFolderID;
            document.getElementById('btnRefreshFolder').click();
        }

        $(function () {
            $(".popuplink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 900,
                height: 350,
                titleShow: false
            });
        });

        $(function () {
            $(".popuplink2").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 900,
                height: 300,
                titleShow: false
            });
        });

        $(function () {
            $(".rolepopuplink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 550,
                height: 250,
                titleShow: false
            });
        });

        function OpenAddAccount() {
            document.getElementById('hlAddAccount').click();
        }
        function OpenAddUserConfirm() {
            $('#hlAddUserLink').trigger('click');
           
        }

        function OpenDashResetConfirm() {
            $('#hlResetDashBoard').trigger('click');

        }
        //         $(document).ready(function () {

        //             document.getElementById('hlAdvanced').href = 'UserName.aspx?UserID=' + encodeURIComponent(document.getElementById('hfUserID').value) + '&Email=' + encodeURIComponent(document.getElementById('txtEmail').value) + '&UserName=' + encodeURIComponent(document.getElementById('hfUserName').value) + '&mode=' + encodeURIComponent(document.getElementById('hfMode').value);
        //   
        //         });

        //         function UpdateHref() {
        //             document.getElementById('hlAdvanced').href = 'UserName.aspx?UserID=' + encodeURIComponent(document.getElementById('hfUserID').value) + '&Email=' + encodeURIComponent(document.getElementById('txtEmail').value) + '&UserName=' + encodeURIComponent(document.getElementById('hfUserName').value) + '&mode=' + encodeURIComponent(document.getElementById('hfMode').value);
        //         }

    </script>
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
    <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
        <div runat="server" id="divDetail" onkeypress="abc();">
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
                                    <table>
                                        <tr>
                                            <td>
                                                <div style="width: 40px; height: 40px;">
                                                    <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server">
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
                                                                    <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="javascript:return confirm('Are you sure you want to delete this User?')"
                                                                        CausesValidation="false" OnClick="lnkDelete_Click">
                                                                        <asp:Image runat="server" ID="Image3"  ImageUrl="~/App_Themes/Default/images/delete_big.png"  ToolTip="Delete" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                                <div runat="server" id="divUnDelete" visible="false">
                                                                    <asp:LinkButton runat="server" ID="lnkUnDelete" OnClientClick="javascript:return confirm('Are you sure you want to restore this User?')"
                                                                        CausesValidation="false" OnClick="lnkUnDelete_Click">
                                                                        <asp:Image runat="server" ID="Image4"  ImageUrl="~/App_Themes/Default/images/Restore_Big.png"  ToolTip="Restore" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divSave">
                                                                    <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="true"
                                                                        ValidationGroup="msg">
                                                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
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
                    <td colspan="3">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                    </td>
                    <td valign="top">
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ValidationGroup="msg" ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct following errors:" />
                        </div>
                        <table cellpadding="3">
                             <tr runat="server" id="trPrimaryAccount" visible="false">
                                <td align="right">
                                    <strong>Primary Account</strong>
                                </td>
                                <td  align="left">
                                    <asp:Label runat="server" ID="lblPrimaryAccount"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>First Name*</strong>
                                </td>
                                <td style="width: 700px;" align="left">
                                    <asp:TextBox ID="txtFirstName" runat="server" Width="200px" CssClass="NormalTextBox"
                                        Text=""></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                        ErrorMessage="First Name - Required" Display="None" ValidationGroup="msg"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Last Name*</strong>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="NormalTextBox" Text="" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                        ErrorMessage="Last Name - Required" Display="None" ValidationGroup="msg"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr runat="server" id="trPassword">
                                <td align="right">
                                    <asp:Label runat="server" ID="lblPassword" Text="Password*" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="NormalTextBox" TextMode="Password"
                                        Text="" Width="200px"></asp:TextBox>
                                    <ajaxToolkit:PasswordStrength ID="PS" runat="server" TargetControlID="txtPassword"
                                        DisplayPosition="RightSide" StrengthIndicatorType="Text" PreferredPasswordLength="20"
                                        PrefixText="Strength:" TextCssClass="TextIndicator" MinimumNumericCharacters="2"
                                        MinimumSymbolCharacters="2" RequiresUpperAndLowerCaseCharacters="true" MinimumLowerCaseCharacters="2"
                                        MinimumUpperCaseCharacters="1" TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
                                        TextStrengthDescriptionStyles="TextIndicator_Strength1;TextIndicator_Strength2;TextIndicator_Strength3;TextIndicator_Strength4;TextIndicator_Strength5"
                                        CalculationWeightings="50;15;15;20" />
                                    <asp:RegularExpressionValidator Display="None" ID="revPasswordLength" runat="server"
                                        ControlToValidate="txtPassword" ValidationGroup="msg" ErrorMessage="Password Minimum length is 6."
                                        ValidationExpression=".{6,30}"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                        ErrorMessage="Password - Required" Display="None" ValidationGroup="msg"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Email*</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="250px" CssClass="NormalTextBox"
                                        Text="" ClientIDMode="Static"></asp:TextBox>
                                    <%--<asp:HyperLink runat="server" ID="hlAdvanced" Text="Advanced..." ClientIDMode="Static" CssClass="popuplink"></asp:HyperLink>--%>
                                    <asp:HiddenField runat="server" ID="hfUserName" ClientIDMode="Static" />
                                    <asp:HiddenField runat="server" ID="hfMode" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Email - Required" Display="None" ValidationGroup="msg"></asp:RequiredFieldValidator>
                                    &nbsp;
                                    <asp:RegularExpressionValidator Display="Dynamic" ID="REVEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage=" Invalid Email" ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Phone Number</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="NormalTextBox" Text=""
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>

                             

                            <%--<tr >
                                <td align="right">
                                   
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" id="chkUserRoleEditDashBoard" TextAlign="Right" 
                                        Text="Edit Dashboard" Font-Bold="true"/>
                                </td>
                            </tr>--%>

                             <tr runat="server" id="trDataScopeTable" visible="false">
                                <td align="right">
                                    <strong>Data Scope</strong>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlScopeTable" DataTextField="TableName"
                                     DataValueField="TableID" CssClass="NormalTextBox" AutoPostBack="true" 
                                        onselectedindexchanged="ddlScopeTable_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>


                              <tr runat="server" id="trDataScopeValue" visible="false">
                                <td align="right">
                                    <strong>Field</strong>
                                </td>
                                <td>
                                    <table cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlScopeField" DataTextField="DisplayName" DataValueField="ColumnID"
                                                    CssClass="NormalTextBox"   AutoPostBack="true"
                                                    onselectedindexchanged="ddlScopeField_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlScopeValue" CssClass="NormalTextBox">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkNotifyUser" runat="server" Checked="false" />
                                            </td>
                                            <td>
                                                <strong>Notify user of account details</strong>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server" id="trAccountHolder" visible="false">
                                <td align="right">
                                    <strong>Account Holder</strong>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkAccountHolder" runat="server" Checked="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" CssClass="DBGTab">
                            <ajaxToolkit:TabPanel ID="TabRole" runat="server">
                                <HeaderTemplate>
                                    <strong>User Roles</strong>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div style="padding: 10px;">
                                        <br />
                                        <div runat="server" id="divBasicRoles" style="min-width:800px">

                                            <table style="padding:3px;" >


                                                 <tr>
                                                    <td >
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <strong>Role:</strong>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlBasicRoles" runat="server" CssClass="NormalTextBox" DataTextField="Role" DataValueField="RoleID"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlBasicRoles_SelectedIndexChanged">
                                                                    </asp:DropDownList>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                        
                                                    </td>
                                                    <td >

                                                      
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                      <asp:HyperLink runat="server" ID="hlRoleGroupNew" 
                                                             CssClass="rolepopuplink" Text="New"/>

                                                                </td>
                                                                <td style="padding-left:10px;">
                                                                      <asp:LinkButton runat="server" ID="lnkRoleEdit" 
                                                             Text="Edit" Visible="false" OnClick="lnkRoleEdit_Click"/>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </td>
                                                    <td align="right" style="width:125px;">
                                                        <asp:LinkButton runat="server" ID="lnkRoleGroupSave" CssClass="btn" 
                                                            OnClick="lnkRoleGroupSave_Click" Visible="false"> <strong>Save Role </strong> </asp:LinkButton>

                                                    </td>
                                                    <td  align="right" style="width:125px;">
                                                         <asp:LinkButton runat="server" ID="lnkRoleGroupDelete" CssClass="btn" 
                                                            OnClick="lnkRoleGroupDelete_Click" OnClientClick="return confirm('Are you sure you wish to delete the current role?');"><strong>Delete Role</strong>  </asp:LinkButton>

                                                    </td>
                                                     <td></td>
                                                </tr>
                                            </table>

                                         
                                        </div>
                                        <div runat="server" id="divUserTable">
                                            <table>

                                                <tr>
                                                    <td colspan="2" valign="top">

                                                        <asp:GridView ID="gvUserTable" runat="server" AutoGenerateColumns="False" DataKeyNames="TableID"
                                                            CssClass="gridview" GridLines="Both" OnRowDataBound="gvUserTable_RowDataBound">
                                                            <HeaderStyle CssClass="gridview_header" />
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblTableID" Text='<%# Eval("TableID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="true" HeaderText="Table">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblTableName" CssClass="NormalText" Text='<%# Eval("TableName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Role">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlRecordRightID" runat="server" CssClass="NormalTextBox">
                                                                            <asp:ListItem Selected="True" Value="6" Text="None"></asp:ListItem>
                                                                            <asp:ListItem Value="5" Text="Read Only"></asp:ListItem>
                                                                            <asp:ListItem Value="7" Text="Add Record Data Only"></asp:ListItem>
                                                                            <asp:ListItem Value="4" Text="Add and Edit Record Data"></asp:ListItem>
                                                                            <%--<asp:ListItem Value="3" Text="Edit Record and Site Data"></asp:ListItem>--%>
                                                                            <asp:ListItem Value="8" Text="Own Data Only"></asp:ListItem>
                                                                            <asp:ListItem Value="9" Text="Edit Own Data, View Others"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField Visible="true" HeaderText="Can Export" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chkCaExport" Checked="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="User View">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlViewsDefaultFromUserID" runat="server" CssClass="NormalTextBox" DataTextField="Email"
                                                                                DataValueField="UserID">                                                                            
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField Visible="true" HeaderText="Edit View" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chkAllowEditView"  />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField Visible="true" HeaderText="Show Menu" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chkShowMenu" Checked="true"  />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>

                                                    </td>


                                                </tr>
                                               

                                            </table>


                                        </div>
                                        <br />
                                        <table>
                                            <tr>
                                               <td>
                                                     <div runat="server" id="divRoleSpecialRights" visible="false">
                                            <table>
                                                <tr>
                                                    <td colspan="2" valign="top">
                                                        <div style="padding: 5px; border: solid 2px #909090; margin-left: 30px; height: 100px;">
                                                            <strong>Role</strong>
                                                            <br />
                                                            <br />
                                                            <div style="padding-left: 20px;">

                                                                <table>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <strong>Dashboard</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlDashboard" DataTextField="Email"
                                                                                DataValueField="UserID" CssClass="NormalTextBox">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chkRoleEditDashboard" Text="Edit Dashboard"
                                                                                TextAlign="Right" />
                                                                        </td>
                                                                        <td>
                                                                             <asp:LinkButton runat="server" ID="lnkResetDashBoard"
                                                            OnClick="lnkResetDashBoard_Click">Reset</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                     <tr runat="server" id="trViewAllTable">
                                                                        <td align="right">
                                                                            <strong>View(all tables)</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlRole_ViewsDefaultFromUserID" DataTextField="Email"
                                                                                DataValueField="UserID" CssClass="NormalTextBox">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chkRole_AllowEditView" Text="Edit View"
                                                                                TextAlign="Right" />
                                                                        </td>
                                                                         <td>
                                                                             <%--  <asp:LinkButton runat="server" ID="lnkResetViews"
                                                            OnClick="lnkResetViews_Click">Reset</asp:LinkButton>--%>
                                                                         </td>
                                                                    </tr>
                                                                </table>

                                                            </div>

                                                        </div>
                                                    </td>

                                                </tr>

                                            </table>
                                        </div>
                                               </td>
                                                <td>
                                                    <div runat="server" id="divUserRole" visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="chkAllowDeleteTable" TextAlign="Right" Text="Delete Tables" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="chkAllowDeleteColumn" TextAlign="Right" Text="Delete Fields" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="chkAllowDeleteRecord" TextAlign="Right" Text="Permanently Delete Records" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>

                                      
                                        <br />
                                        <%--<asp:CheckBox runat="server" ID="chkAdvancedSecurity" Text="Advanced Security" Font-Bold="true"
                                            OnCheckedChanged="chkAdvancedSecurity_CheckedChanged" AutoPostBack="true" />--%>
                                        <br />
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="tabDocuments" runat="server">
                                <HeaderTemplate>
                                    <strong>Documents</strong>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div style="padding: 10px;">
                                        <br />
                                        <div runat="server" id="divBasicDocSec">
                                            <asp:DropDownList ID="ddlBasicDocSec" runat="server" CssClass="NormalTextBox">
                                                <asp:ListItem Value="none" Text="None" ></asp:ListItem>
                                                <asp:ListItem Value="read" Text="Read Only"></asp:ListItem>
                                                <asp:ListItem Value="upload" Text="Read and Upload"></asp:ListItem>
                                                <asp:ListItem Value="full" Text="Administrator" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div runat="server" id="divDocAdvancedSec" visible="false">
                                            <p>
                                                <asp:Label Visible="false" runat="server" ID="lblCurrentFolder" Text="<a href='javascript:SetFolder(-1)'>Home</a>/"></asp:Label></p>
                                            <asp:GridView ID="gvDocAdvancedSec" runat="server" AutoGenerateColumns="False" DataKeyNames="FolderID"
                                                CssClass="gridview" GridLines="Both" OnRowDataBound="gvDocAdvancedSec_RowDataBound">
                                                <HeaderStyle CssClass="gridview_header" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="LblID" Text='<%# Eval("FolderID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%--<asp:HyperLink ID="hlFolderIcon" runat="server" ToolTip="Folder" NavigateUrl='#'
                                                                ImageUrl="~/App_Themes/Default/Images/Folder.png">
                                           
                                                            </asp:HyperLink>--%>
                                                            <asp:Image runat="server" ID="imgFolderIcon" ToolTip="Folder"
                                                            ImageUrl="~/App_Themes/Default/Images/Folder.png" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="true" HeaderText="Folder">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblFolderName" CssClass="NormalText" Text='<%# Eval("FolderName") %>'></asp:Label>
                                                            <%--<asp:LinkButton ID="lnkFolderName" runat="server" OnClick="GoToFolder" Text='<%# Eval("FolderName") %>'></asp:LinkButton>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Right">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlBasicDocSecEach" runat="server" CssClass="NormalTextBox">
                                                                <asp:ListItem Value="none" Text="None" ></asp:ListItem>
                                                                <asp:ListItem Value="read" Text="Read Only"></asp:ListItem>
                                                                <asp:ListItem Value="upload" Text="Read and Upload"></asp:ListItem>
                                                                <asp:ListItem Value="full" Text="Administrator" Selected="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <br />
                                        <asp:CheckBox runat="server" ID="chkDocAdvancedSec" Text="Advanced Security" Font-Bold="true"
                                            OnCheckedChanged="chkDocAdvancedSec_CheckedChanged" AutoPostBack="true" />
                                        <br />
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                         
                            <ajaxToolkit:TabPanel ID="TabLinkedAccounts" runat="server">
                                <HeaderTemplate>
                                    <strong>Linked Accounts</strong>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upLinkedAccounts" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="padding: 10px;">
                                                <asp:HyperLink runat="server" ID="hlAddAccount" CausesValidation="false" ClientIDMode="Static"
                                                    Text="Add Account" NavigateUrl="#" CssClass="popuplink2" style="display:none;" />
                                                <br />
                                               
                                      <div id="divGridMain" style="padding-right:10px;">
                                          <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                              HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left" AllowPaging="True"
                                              AllowSorting="false" DataKeyNames="UserRoleID" HeaderStyle-ForeColor="Black"
                                              Width="100%" AutoGenerateColumns="false" PageSize="15"
                                              OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound">
                                              <PagerSettings Position="Top" />
                                              <Columns>
                                                  <asp:TemplateField Visible="false">
                                                      <ItemStyle Width="10px" HorizontalAlign="Left" />
                                                      <ItemTemplate>
                                                          <asp:Label ID="LblID" runat="server" Text='<%# Eval("UserRoleID") %>'></asp:Label>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField>
                                                      <ItemStyle Width="10px" HorizontalAlign="Left" />
                                                      <HeaderTemplate>
                                                          <input id="chkAll" onclick="DoMasterSelect(this, 'divGridMain')" runat="server" type="checkbox" />
                                                      </HeaderTemplate>
                                                      <ItemTemplate>
                                                          <asp:CheckBox ID="chkDelete" runat="server" />
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                                 
                                                  <asp:TemplateField HeaderText="Account Name"   HeaderStyle-HorizontalAlign="Left"  >
                                                  <ItemStyle HorizontalAlign="Left" />
                                                      <ItemTemplate>
                                                          <asp:Label ID="lblAccountName" runat="server" Text='<%# Eval("AccountName") %>'></asp:Label>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Role"  HeaderStyle-HorizontalAlign="Left" >
                                                  <ItemStyle HorizontalAlign="Left" />
                                                      <ItemTemplate>
                                                          <asp:Label ID="lblRole" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                              </Columns>
                                              <HeaderStyle CssClass="gridview_header" />
                                              <RowStyle CssClass="gridview_row" />
                                              <PagerTemplate>
                                                  <asp:GridViewPager runat="server" ID="Pager" HideDelete="false" HideAdd="false" HideExport="false"
                                                      OnExportForCSV="Pager_OnExportForCSV" OnDeleteAction="Pager_DeleteAction"  
                                                      OnBindTheGridToExport="Pager_BindTheGridToExport" OnBindTheGridAgain="Pager_BindTheGridAgain"
                                                      HideFilter="true" />
                                              </PagerTemplate>
                                          </dbg:dbgGridView>
                                      </div>
                                      <br />
                                      <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                                          <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                              <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png"  />
                                              No accounts have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                                  Add new account now.</strong>
                                          </asp:HyperLink>
                                      </div>
                                      <br /><br />
                                     


                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                        <asp:HiddenField runat="server" ID="hfUserID" ClientIDMode="Static" Value="-1" />
                        <asp:HiddenField runat="server" ID="hfParentFolderID" Value="-1" ClientIDMode="Static" />
                        <asp:Button runat="server" ID="btnRefreshLinkedUser" Style="display: none;" OnClick="btnRefreshLinkedUser_Click"
                            ClientIDMode="Static" />
                        <asp:Button ID="btnRefreshFolder" runat="server" Visible="true" ClientIDMode="Static"
                            Text="" Height="1px" Width="1px" Style="display: none;" OnClick="btnFolderSaved_Click" />

                        <asp:Button ID="btnRoleSaved" runat="server" Visible="true" ClientIDMode="Static" Text=""
                                            Height="1px" Width="1px" style="display:none;" OnClick="btnRoleSaved_Click" />
                          <asp:HiddenField runat="server" ID="hfRoleGroupID" Value="" ClientIDMode="Static" />
                         <asp:Button runat="server" ID="btnAddUserLinkOK" ClientIDMode="Static" Style="display: none;" OnClick="btnAddUserLinkOK_Click" />
                        <asp:Button runat="server" ID="btnRestoreUser" ClientIDMode="Static" Style="display: none;" OnClick="btnRestoreUser_Click" />
                         <asp:HyperLink ID="hlAddUserLink" ClientIDMode="Static" runat="server" CssClass="popupaddlinkuser" style="display:none;"></asp:HyperLink>
                        <asp:HyperLink ID="hlResetDashBoard" ClientIDMode="Static" runat="server" CssClass="popupaddlinkuser" style="display:none;"></asp:HyperLink>
                                                 <asp:Button runat="server" ID="btnResetDashBoard" ClientIDMode="Static" Style="display: none;" OnClick="btnResetDashBoard_Click" />

                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
