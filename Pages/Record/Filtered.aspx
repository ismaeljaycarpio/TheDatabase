<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="Filtered.aspx.cs" Inherits="Pages_Record_Filtered" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
   <script language="javascript" type="text/javascript">
       function GetBackValue() {
           window.parent.document.getElementById('hfFilterParentColumnID').value = '';
           window.parent.document.getElementById('hfFilterOtherColumnID').value = '';
           window.parent.document.getElementById('hfFilterValue').value = '';
           window.parent.document.getElementById('hfFilterOperator').value = 'equals';

           if (document.getElementById('ddlFilterParentColumnID').value != '') {
               window.parent.document.getElementById('hfFilterParentColumnID').value = document.getElementById('ddlFilterParentColumnID').value;
               window.parent.document.getElementById('hfFilterOperator').value = document.getElementById('ddlFilterOperator').value;

               if (document.getElementById('ddlFilterOtherColumnID') != null && document.getElementById('ddlFilterOtherColumnID').value != '') {
                   window.parent.document.getElementById('hfFilterOtherColumnID').value = document.getElementById('ddlFilterOtherColumnID').value;
               }

               if (document.getElementById('txtFilterValue') != null && document.getElementById('txtFilterValue').value != '') {
                   window.parent.document.getElementById('hfFilterValue').value = document.getElementById('txtFilterValue').value;
               }
           }

           window.parent.document.getElementById('hlFiltered').href = 'Filtered.aspx?hfFilterOperator=' + encodeURIComponent(window.parent.document.getElementById('hfFilterOperator').value) + '&hfFilterParentColumnID=' + encodeURIComponent(window.parent.document.getElementById('hfFilterParentColumnID').value) + "&hfFilterOtherColumnID=" + encodeURIComponent(window.parent.document.getElementById("hfFilterOtherColumnID").value) + "&hfFilterValue=" + encodeURIComponent(window.parent.document.getElementById("hfFilterValue").value) + "&ParentTableID=" + encodeURIComponent(document.getElementById("hfParentTableID").value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
           window.parent.document.getElementById('chkFiltered').checked = true;           

           parent.$.fancybox.close();

       }

   </script>
   <asp:HiddenField  runat="server" ID="hfParentTableID" ClientIDMode="Static"/>
   <asp:HiddenField  runat="server" ID="hfTableID"/>
    <asp:HiddenField  runat="server" ID="hfColumnID"/>
   
 <div style="padding-top: 10px;">
       <h3>Filter Dropdown Items</h3>
     <table>
         <tr>
             <td align="right">
                 <strong></strong>
             </td>
             <td align="right">
                 <div runat="server" id="divSave">
                     <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="false">
                         <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                             ToolTip="Save" />
                     </asp:LinkButton>
                 </div>

             </td>
         </tr>
         
         <tr>
             <td colspan="2" align="left">
                <strong>  Only show items where:</strong>
             </td>
         </tr>
          <tr>
             <td align="right" style="width:90px;">
                 <strong runat="server" id="stgField">Field</strong>
             </td>
             <td align="left">
                 <asp:DropDownList runat="server" ID="ddlFilterParentColumnID" ClientIDMode="Static"
                                                                                CssClass="NormalTextBox">
                                                                            </asp:DropDownList>
             </td>
         </tr>
         <tr>
             <td></td>
             <td align="left">
                 <asp:DropDownList runat="server" ID="ddlFilterOperator" CssClass="NormalTextBox" ClientIDMode="Static">
                            <asp:ListItem Value="equals" Text="Equals" Selected="True"></asp:ListItem>
                             <asp:ListItem Value="contains" Text="Contains"></asp:ListItem>
                            <asp:ListItem Value="greaterthan" Text="Greater Than"></asp:ListItem>
                            <asp:ListItem Value="lessthan" Text="Less Than"></asp:ListItem>
                            <asp:ListItem Value="empty" Text="Is Empty"></asp:ListItem>
                            <asp:ListItem Value="notempty" Text="Is Not Empty"></asp:ListItem>
                        </asp:DropDownList>
             </td>
         </tr>
          <tr>
             <td align="right" valign="top">
                 <%--<strong>Filter On</strong>--%>
             </td>
             <td align="left">
                <asp:RadioButtonList runat="server" AutoPostBack="true" ID="optFilterType" 
                    OnSelectedIndexChanged="optFilterType_SelectedIndexChanged" RepeatDirection="Horizontal">
                    <asp:ListItem Value="f" Text="Value" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="o" Text="Another Field"></asp:ListItem>
                </asp:RadioButtonList>
             </td>
         </tr>
          <tr runat="server" id="trOtherField" visible="false">
             <td align="right">
                 <strong runat="server" id="stgOtherField">Field</strong>
             </td>
              <td align="left">
                  <asp:DropDownList runat="server" ID="ddlFilterOtherColumnID" ClientIDMode="Static"
                      CssClass="NormalTextBox">
                  </asp:DropDownList>
              </td>
         </tr>
         <tr runat="server" id="trFilterValue">
             <td align="right">
                 <strong>Value</strong>
             </td>
             <td align="left">
              <asp:TextBox runat="server" ID="txtFilterValue" ClientIDMode="Static" CssClass="NormalTextBox" Width="400px"></asp:TextBox>
             </td>
         </tr>
     </table>

 </div>
</asp:Content>

