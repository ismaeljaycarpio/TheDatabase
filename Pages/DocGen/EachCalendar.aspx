<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EachCalendar.aspx.cs" Inherits="Pages_DocGen_EachCalendar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />

     <style type="text/css">
        .OtherMonthDayStyle
        {
            background-color: #F2F2F2;
            padding: 0px;
             vertical-align: top;
        }
        .TodayDayStyle
        {
             vertical-align: top;
        }
          .WeekendDayStyle
        {
             vertical-align: top;
        }
        .maincalender
        {
            background-color: #FFFFFF;
            border-color:#D8D8D8;
        }

         .maincalender tr:first-child
        {
            display:none;
        }
          
        .DayStyle
        {
            padding: 0px;
            vertical-align: top;
        }
       

        
        .eachschedule
        {
         background-color:#ffffff;
       
         padding:1px;
         font-weight:bold;

        }

         .TitleStyle
         {
              font-family:Verdana;
              font-size:16pt;
              background-color:#FFFFFF;             
      
         }

         .DayHeaderStyle
         {
              font-family:Verdana;
              font-size:12px;
              background-color:#0299C6;

         }

         th.DayHeaderStyle:nth-child(6)
         {
              font-family:Verdana;
              font-size:12px;
              background-color:#45A6EB;

         }
         th.DayHeaderStyle:nth-child(7)
         {
              font-family:Verdana;
              font-size:12px;
              background-color:#45A6EB;

         }

         .TodayTop
        {
         padding-left:5px;
         padding-top:5px;
           
            text-align:left;
            background-color:#ffffff;
             font-size:12px;
             font-family:Arial;
             color:Gray;
              vertical-align: top;
        }

        .OtherDayTop
        {
        padding-left:5px;
         padding-top:5px;
        
          text-align:left;
          background-color:#ffffff;
           font-size:12px;
           font-family:Arial;
           color:Gray;
            vertical-align: top;
        }
         .OtherMonthDayTop
        {
        padding-left:5px;
         padding-top:5px;
         
          text-align:left;
          background-color:#F2F2F2;
           font-size:12px;
           font-family:Arial;
           color:Gray;
           vertical-align: top;
        }
        .BottomRightLink
        {
           
          
            background-color:#0299C6;
            color:#ffffff;
            width: 30px;
            height: 30px;
            cursor: pointer;
            

        }
</style>

<script type="text/javascript">

    function SetWeekView(SourceDate) {
        document.getElementById('hfSourceDate').value = SourceDate;
        var clickButton = document.getElementById('btnRefresh');
        clickButton.click();
        //$("#btnRefresh").trigger("click");
    }
 
</script>

</head>
<body style="background-image: none; background-color: #ffffff;">
     <%--<script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-1.11.3.min.js")%>"></script>--%>
     <script type="text/javascript" src="<%=ResolveUrl("~/script/jquery.js")%>"></script>

    <form id="form1" runat="server">

           <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="36000">
        </asp:ScriptManager>

<asp:UpdateProgress class="ajax-indicator-full" ID="upMaster" runat="server">
    <ProgressTemplate>
        <table style="width: 100%; height: 100%; text-align: center;">
            <tr valign="middle">
                <td>
                    <p style="font-size:12px;">
                        Please wait...
                    </p>
                    <asp:Image runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                </td>
            </tr>
        </table>
    </ProgressTemplate>
</asp:UpdateProgress>
        <asp:UpdatePanel ID="upNotificationMessage" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="divNotificationMessage" style="position: fixed; top: 0px;">
                    <asp:Label runat="server" ID="lblNotificationMessage"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:HiddenField  runat="server" ID="hfSourceDate" ClientIDMode="Static"/>
         <asp:Button ID="btnRefresh" runat="server" ClientIDMode="Static" OnClick="btnRefresh_Click"
                        Style="display: none;" />
    <table width="100%">
        <tr runat="server" id="trHeading">
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" Text="" Font-Bold="true" Style="font-size: 20px;
                                color: Black; font-family: Verdana;"></asp:Label>
                        </td>
                        <td align="right" style="padding-right:10px;">
                            <asp:HyperLink runat="server" ID="hlAddNewRecord" ToolTip="Add New Record" Visible="false">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                            </asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
            
        </tr>
        <tr>
            <td>
                <div style="padding: 5px;">
                    <table width="100%">
                        <tr>
                            <td>
                                <table width="100%" >
                                    <tr>
                                        <td style="width:25%;">
                                            <asp:LinkButton runat="server" ID="lnkPre" OnClick="lnkPre_OnClick" ToolTip="Go to the previous month">
                                                       <img src='../../Images/CalenderLeft.png' border='0'/>
                                            </asp:LinkButton>
                                        </td>
                                        <td style="width:50%; text-align:center;">
                                            <asp:Label runat="server" CssClass="TitleStyle" ID="lblTitle"></asp:Label>
                                        </td>
                                        <td style="width:20%;" align="right">
                                            <asp:LinkButton runat="server" ID="lnkMonthWeekView" style="font-family:Verdana;font-size:11pt;" 
                                            ForeColor="Blue"  OnClick="lnkMonthWeekView_OnClick">Week View</asp:LinkButton>
                                        </td>
                                        <td align="right" style="width:5%;">
                                            <asp:LinkButton runat="server" ID="lnkNext" OnClick="lnkNext_OnClick" ToolTip="Go to the next month">
                                                       <img src='../../Images/CalenderRight.png' border='0'/>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>


                                 <asp:Calendar runat="server" ID="cldDate" Width="100%"  OnDayRender="cldDate_DayRender" 
                                    TitleStyle-CssClass="TitleStyle" DayHeaderStyle-CssClass="DayHeaderStyle" DayStyle-CssClass="DayStyle" 
                                    NextPrevStyle-CssClass="NextPrevStyle" OtherMonthDayStyle-CssClass="OtherMonthDayStyle"
                                    SelectedDayStyle-CssClass="SelectedDayStyle" SelectorStyle-CssClass="SelectorStyle"
                                    TodayDayStyle-CssClass="TodayDayStyle" WeekendDayStyle-CssClass="WeekendDayStyle"
                                    CssClass="maincalender" OnSelectionChanged="cldDate_SelectionChanged" DayNameFormat="Full"
                                    BorderStyle="Solid" BorderWidth="1px"  ForeColor="Black" SelectedDayStyle-BackColor="ActiveBorder"
                                    SelectedDayStyle-ForeColor="Black" ShowGridLines="true" OnVisibleMonthChanged="cldDate_VisibleMonthChanged">
                                    <DayHeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Height="30px" ForeColor="White" />
                                    <TitleStyle CssClass="TitleStyle" BackColor="White" Height="0px" />
                                </asp:Calendar>

                               <%-- <asp:Calendar runat="server" ID="cldDate" Width="1000px" Height="500px" OnDayRender="cldDate_DayRender"
                                    TitleStyle-CssClass="TitleStyle" DayHeaderStyle-CssClass="DayHeaderStyle" DayStyle-CssClass="DayStyle"
                                    NextPrevStyle-CssClass="NextPrevStyle" PrevMonthText="<img src='../../Images/CalenderLeft.png' border='0'/>"
                                    NextMonthText="<img src='../../Images/CalenderRight.png' border='0'/>" OtherMonthDayStyle-CssClass="OtherMonthDayStyle"
                                    SelectedDayStyle-CssClass="SelectedDayStyle" SelectorStyle-CssClass="SelectorStyle"
                                    TodayDayStyle-CssClass="TodayDayStyle" WeekendDayStyle-CssClass="WeekendDayStyle"
                                    CssClass="maincalender" OnSelectionChanged="cldDate_SelectionChanged" DayNameFormat="Full"
                                    BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" SelectedDayStyle-BackColor="ActiveBorder"
                                    SelectedDayStyle-ForeColor="Black" ShowGridLines="true" OnVisibleMonthChanged="cldDate_VisibleMonthChanged">
                                    <DayHeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Height="30px" ForeColor="White" />
                                    <TitleStyle CssClass="TitleStyle" BackColor="White" Height="0px" />
                                </asp:Calendar>--%>
                            </td>
                        </tr>
                    </table>
                                  

               </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
