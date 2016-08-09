<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master"
 AutoEventWireup="true" CodeFile="ChildTableDetail.aspx.cs" Inherits="Pages_Record_ChildTableDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">

<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>


    <script type="text/javascript">
        $(document).ready(function () {

            $('#chkShowWhen').click(function (e) {

                var chk = document.getElementById("chkShowWhen");
                if (chk.checked == false) { $("#trShowWhenControls").fadeOut(); }
                if (chk.checked == true) { $("#trShowWhenControls").fadeIn(); }
            });


            var chk = document.getElementById("chkShowWhen");
            if (chk.checked == false) { $("#trShowWhenControls").fadeOut(); }
            if (chk.checked == true) { $("#trShowWhenControls").fadeIn(); }

            $('#ddDDDisplayColumn').change(function (e) {

                var strColumnValue = $('#ddDDDisplayColumn').val();

                if (strColumnValue != '') {
                    $('#hlDDEdit').fadeOut();
                    document.getElementById('hfDisplayColumnsFormula').value = '[' + $('#ddDDDisplayColumn option:selected').text() + ']';
                }
                else {
                    $('#hlDDEdit').fadeIn();
                }

                document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?ct=yes&formula=' + encodeURIComponent(document.getElementById('hfDisplayColumnsFormula').value) + '&Tableid=' + document.getElementById('hfParentTableID').value;


            });

        });

    </script>

<div >
    <table >
        <tr>
            <td colspan="2" style="height:50px;">
               
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblDetailTitle" Font-Size="16px" Font-Bold="true" Text="Child Table - Detail"> </asp:Label>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td align="right" >
                <strong>Child Table:</strong>
            </td>
            <td>
                  <asp:DropDownList ID="ddlChildTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                     Width="255px" DataValueField="TableID" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlChildTable_SelectedIndexChanged">
                </asp:DropDownList>
                 <asp:RequiredFieldValidator ID="rfvTable" runat="server" ControlToValidate="ddlChildTable"
                                        ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr runat="server" id="trConnection">
            <td align="right">
                <strong>How are they connected:</strong>
            </td>
            <td>
                <table >
                    <tr>
                        <td align="left">
                           <strong runat="server" id="stgParentFieldCap">Parent Field </strong>  
                        </td>
                        <td align="left" style="padding-left:20px;">
                               <strong runat="server" id="stgChildFieldCap"> Child Field</strong>  

                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                           
                             <asp:DropDownList runat="server" ID="ddlParentColumn" CssClass="NormalTextBox"
                            DataValueField="ColumnID" DataTextField="DisplayNamne"></asp:DropDownList>
                           
                        </td>
                        <td align="left" style="padding-left:20px;">
                            <asp:DropDownList runat="server" ID="ddlChildColumn"
                             CssClass="NormalTextBox"></asp:DropDownList>
                          
                        </td>
                    </tr>
                   
                </table>
              
            </td>
        </tr>

         <tr runat="server" id="trDisplayColumn">
                        <td align="right">
                            <strong runat="server" id="stgDisplayField">Display Field</strong>
                        </td>
                        <td align="left" >
                            <asp:DropDownList runat="server" ID="ddDDDisplayColumn" ClientIDMode="Static" CssClass="NormalTextBox">
                             </asp:DropDownList>
                            <asp:HyperLink runat="server" ID="hlDDEdit" Text="Edit" ClientIDMode="Static" NavigateUrl="~/Pages/Help/TableColumn.aspx"></asp:HyperLink>
                             <asp:HiddenField runat="server" ID="hfDisplayColumnsFormula" ClientIDMode="Static" />
                             <asp:HiddenField runat="server" ID="hfParentTableID" ClientIDMode="Static" />
                        </td>
                    </tr>

        <tr>
            <td align="right">
                <strong>Tab Label:</strong>
            </td>
            <td>
               

                 <asp:TextBox ID="txtDescription" runat="server"   Width="250px"  
                  CssClass="NormalTextBox"></asp:TextBox>
              
            </td>
        </tr>
         <tr>
            <td align="right" >
                <strong>Show On Detail Page:</strong>
            </td>
            <td>
                  <asp:DropDownList ID="ddlDetailPageType" runat="server" AutoPostBack="false" 
                    Width="150px" CssClass="NormalTextBox">
                                 <asp:ListItem Text="Not displayed" Value="not" Selected="True"></asp:ListItem>
                                 <asp:ListItem Text="As a list" Value="list"></asp:ListItem>
                                 <asp:ListItem Text="One at a time" Value="one"></asp:ListItem>
                                  <asp:ListItem Text="One Record Only" Value="alone"></asp:ListItem>
                </asp:DropDownList>
                
            </td>
        </tr>
       

        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkShowAddButton" Text="Add Button" TextAlign="Right" Font-Bold="true" Checked="true" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                  <asp:CheckBox runat="server" ID="chkShowEditButton" Text="Edit Button" TextAlign="Right" Font-Bold="true" Checked="true" />
            </td>
        </tr>


         <tr>
            <td>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="3" align="left">
                                 <asp:CheckBox runat="server" ID="chkShowWhen" Text="Show When" TextAlign="Right"
                                  Font-Bold="true" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr id="trShowWhenControls" style="display:none;"  >
                        <td valign="top" style="padding-left:20px;">
                            <asp:DropDownList runat="server" ID="ddlHideColumn" CssClass="NormalTextBox"
                            DataValueField="ColumnID" DataTextField="DisplayNamne" AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlHideColumn_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td valign="top" style="padding-left:3px;">
                            <asp:DropDownList runat="server" ID="ddlOperator" CssClass="NormalTextBox" >
                                <asp:ListItem Value="equals" Text="Equals" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="contains" Text="Contains"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td valign="top"  style="padding-left:3px;">
                            <asp:TextBox runat="server" ID="txtHideColumnValue" 
                            CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                            
                            <asp:ListBox runat="server" ID="lstHideColumnValue" Visible="false" 
                            SelectionMode="Multiple" style="min-width:200px; min-height:100px;"></asp:ListBox>

                            <asp:DropDownList runat="server" ID="ddlHideColumnValue" 
                            Visible="false" CssClass="NormalTextBox" ></asp:DropDownList>

                            <asp:HiddenField runat="server" ID="hfHideColumnValueControl" Value="text" />

                        </td>
                    </tr>
                </table>


            </td>
        </tr>

        <tr style="height:10px;">
            <td></td>   <td></td>
        </tr>
        <tr>
            <td></td>
            <td  align="left">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div runat="server" id="div1" style="padding-left: 10px;">
                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" CausesValidation="true"
                                    OnClick="lnkSave_Click"> <strong>Save</strong></asp:LinkButton>
                            </div>
                        </td>
                        <td>
                            <div runat="server" id="div2" style="padding-left: 10px;">
                                <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                    CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>



    </table>

</div>
    

      <script type="text/javascript">

          function CloseAndRefresh() {
              window.parent.document.getElementById('btnRefreshGrid').click();
              parent.$.fancybox.close();
              // alert('ok');
          }
    
    </script>

</asp:Content>

