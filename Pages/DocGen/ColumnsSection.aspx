<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="ColumnsSection.aspx.cs" Inherits="DocGen.Document.ColumnsSection.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .main
        {
            min-height: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification"
        ValidationGroup="MainValidationGroup" />
    <div>
        <div class="edit">
            <p>
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text=" Columns Section"></asp:Label>
                        </td>
                        <td>
                            <%--<asp:LinkButton ID="CancelButton" runat="server" CausesValidation="false" OnClick="CancelButton_Click">
                                <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                            </asp:LinkButton>--%>
                            <span style="display: inline-block; width: 20px; float: left">&nbsp;</span>
                            <asp:LinkButton ID="SaveButton" runat="server" ValidationGroup="MainValidationGroup"
                                OnClick="SaveButton_Click" OnClientClick="">
                                <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </p>
            <p>
                <asp:Label ID="lblStyle" runat="server" CssClass="NormalTextBox"><b>Number of Columns:</b></asp:Label>
                <br />
                <asp:DropDownList ID="ddlNumberOfCols" runat="server" Width="100px" CssClass="NormalTextBox">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem Selected="True">2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:DropDownList>
            </p>
            <p>
                <asp:Label ID="Label2" runat="server" CssClass="NormalTextBox"><b>Spacing between Columns:</b></asp:Label>
                <br />
                <asp:DropDownList ID="ddlSpacing" runat="server" Width="100px" CssClass="NormalTextBox">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem Selected="True">2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                </asp:DropDownList>
            
            </p>
            <p runat="server" visible="false">
                <asp:Label ID="lblWidths" runat="server"><b>Width values %</b>(optional):</asp:Label>
                <br />
                <asp:TextBox ID="txtWidths" runat="server" TextMode="MultiLine" Rows="5" Width="200px"></asp:TextBox>
            </p>
            <p class="comment" runat="server" visible="false">
                Width for columns in separately line (decimal value without unit)
            </p>
        </div>
    </div>
    <script type="text/javascript">
               
    </script>
</asp:Content>
