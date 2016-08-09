﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="CSSLite.aspx.cs" Inherits="Pages_CSSGen_CSSLite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
      <script src="StyleGen.js" type="text/javascript"></script>

       <script language="javascript" type="text/javascript">

           function GetBackValue() {

               if (document.getElementById('text_b') != null) {
                   window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtStyle').value = document.getElementById('text_b').value;
                   window.parent.ShowRecord();
               }
               parent.$.fancybox.close();

           }

    </script>


   <div style="padding: 20px;">
        <div style="margin: 0; padding: 0;">
            <h1>Style Generator</h1>
            <span id="zone01"></span>
            <textarea style="width: 680px; height: 85px;" id="zonetext0" onkeyup="new_fct();"
                name="content0" cols="83" rows="4">Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi.</textarea>
        </div>
        <div>
            <table cellspacing="3" style="text-align:right;">
                <tr>
                    <td>
                      Font:  <select onchange="new_fct()" style="width: 110px" id="ddlFont" class="NormalTextBox" >
                             <option  value=""></option>
                            <option style="font-family: Tahoma,sans-serif,Verdana,Arial,Helvetica,Helvetica-Narrow"
                             value="Tahoma,sans-serif,Verdana,Arial,Helvetica,Helvetica-Narrow">Verdana</option>

                            <option style="font-family: arial, helvetica, sans-serif" value="arial, helvetica, sans-serif"
                                >Arial</option>
                            <option style="font-family: arial black, sans-serif" value="arial black, sans-serif">
                                Arial Black</option>
                            <option style="font-family: trebuchet MS, sans-serif" value="trebuchet MS, sans-serif">
                                Trebuchet MS</option>
                            <option style="font-family: courier new, courier, monospace" value="courier new, courier, monospace">
                                Courier</option>
                            <option style="font-family: helvetica, sans-serif" value="helvetica, sans-serif">Helvetica</option>
                            
                            <option style="font-family: georgia, serif" value="georgia, serif">Georgia</option>
                            <option style="font-family: palatino linotype, palatino, serif" value="palatino linotype, palatino, serif">
                                Palatino</option>
                            <option style="font-family: impact, sans-serif" value="impact, sans-serif">Impact</option>
                            <option style="font-family: comic sans, comic sans ms, cursive, verdana, arial, sans-serif"
                                value="comic sans, comic sans ms, cursive, verdana, arial, sans-serif">Comic Sans
                                MS</option>
                            <option style="font-family: tahoma, verdana, arial, sans-serif" value="tahoma, verdana, arial, sans-serif">
                                Tahoma</option>
                            <option style="font-family: times new roman, times, serif" value="times new roman, times, serif">
                                Times New Roman</option>
                            <option style="font-family: lucida sans unicode, lucida grande, sans-serif" value="lucida sans unicode, lucida grande, sans-serif">
                                Lucida Sans</option>
                        </select>
                    </td>
                    <td style="width:20px;"></td>
                    <td align="left">
                        <input type="checkbox" id="chkBold" onchange="new_fct()" /><strong>Bold</strong>
                    </td>
                     <td style="width:20px;"></td>
                    <td>
                        Text Colour:
                        <select onchange="new_fct()" style="width: 110px" id="ddlTextColour" class="NormalTextBox" >
                            <option  value=""></option>
                              <option style="color:Aqua;"  value="Aqua">Aqua</option>
                              <option style="color:Black;"  value="Black">Black</option>
                               <option style="color:Blue;"  value="Blue">Blue</option>
                                <option style="color:Fuchsia;"  value="Fuchsia">Fuchsia</option>
                                 <option style="color:Gray;"  value="Gray">Gray</option>
                                  <option style="color:Green;"  value="Green">Green</option>
                                   <option style="color:Lime;"  value="Lime">Lime</option>
                                    <option style="color:Maroon;"  value="Maroon">Maroon</option>
                                     <option style="color:Navy;"  value="Navy">Navy</option>
                                      <option style="color:Olive;"  value="Olive">Olive</option>
                                       <option style="color:Orange;"  value="Orange">Orange</option>
                                        <option style="color:Purple;"  value="Purple">Purple</option>
                                         <option style="color:Red;"  value="Red">Red</option>
                                          <option style="color:Silver;"  value="Silver">Silver</option>
                                           <option style="color:Teal;"  value="Teal">Teal</option>
                                             <option style="color:Black;"  value="White">White</option>
                                               <option style="color:Yellow;"  value="Yellow">Teal</option>
                                               
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        Size:
                          <select onchange="new_fct()" style="width: 110px;height:22px;" id="ddlFontSize" class="NormalTextBox" >
                                 <option  value=""></option>
                                 <option style="font-size:8px;"  value="8">8</option>
                                 <option style="font-size:9px;"  value="9">9</option>
                                 <option style="font-size:10px;"  value="10">10</option>
                                 <option style="font-size:11px;"  value="11">11</option>
                                  <option style="font-size:12px;"  value="12">12</option>
                                  <option style="font-size:14px;"  value="14">14</option>
                                 <option style="font-size:16px;"  value="16">16</option>
                                  <option style="font-size:18px;"  value="18">18</option>
                                  <option style="font-size:20px;"  value="20">20</option>
                                  <option style="font-size:22px;"  value="22">22</option>
                                  <option style="font-size:24px;"  value="24">24</option>
                                  <option style="font-size:26px;"  value="26">26</option>
                                  <option style="font-size:28px;"  value="28">28</option>
                                  <option style="font-size:36px;"  value="36">36</option>
                                  <option style="font-size:48px;"  value="48">48</option>
                                  <option style="font-size:72px;"  value="72">72</option>                                  
                          </select>
                        
                    </td>
                     <td style="width:20px;"></td>
                    <td align="left">
                         <input type="checkbox" id="chkItalic" onchange="new_fct()" /><i>Italic</i>

                    </td>
                     <td style="width:20px;"></td>
                    <td>
                          Background:
                        <select onchange="new_fct()" style="width: 110px" id="ddlBackground" class="NormalTextBox"  >
                            <option  value=""></option>
                              <option style="background-color:Aqua;"  value="Aqua">Aqua</option>
                              <option style="background-color:Black;color:White;"  value="Black">Black</option>
                               <option style="background-color:Blue;"  value="Blue">Blue</option>
                                <option style="background-color:Fuchsia;"  value="Fuchsia">Fuchsia</option>
                                 <option style="background-color:Gray;"  value="Gray">Gray</option>
                                  <option style="background-color:Green;"  value="Green">Green</option>
                                   <option style="background-color:Lime;"  value="Lime">Lime</option>
                                    <option style="background-color:Maroon;"  value="Maroon">Maroon</option>
                                     <option style="background-color:Navy;color:White;"  value="Navy">Navy</option>
                                      <option style="background-color:Olive;"  value="Olive">Olive</option>
                                       <option style="background-color:Orange;"  value="Orange">Orange</option>
                                        <option style="background-color:Purple;"  value="Purple">Purple</option>
                                         <option style="background-color:Red;"  value="Red">Red</option>
                                          <option style="background-color:Silver;"  value="Silver">Silver</option>
                                           <option style="background-color:Teal;"  value="Teal">Teal</option>
                                             <option style="background-color:White;"  value="White">White</option>
                                               <option style="background-color:Yellow;"  value="Yellow">Teal</option>
                                               
                        </select>

                    </td>
                </tr>
                <tr>
                    <td>

                           Line Height:
                          <select onchange="new_fct()" style="width: 110px;height:22px;" id="ddlLineHeight" class="NormalTextBox" >
                                 <option  value=""></option>
                                 <option style="line-height:1;"  value="1">1</option>
                                <option style="line-height:1.25;"  value="1.25">1.25</option>
                                <option style="line-height:1.5;"  value="1.5">1.5</option>
                                <option style="line-height:2;"  value="2">2</option>
                                <option style="line-height:3;"  value="3">3</option>
                                                              
                          </select>



                    </td>
                     <td style="width:20px;"></td>
                    <td style="text-decoration:underline;" align="left">

                         <input type="checkbox" id="chkUnderline" onchange="new_fct()" />Underline

                    </td>
                     <td style="width:20px;"></td>
                    <td>

                               Border Colour:
                        <select onchange="new_fct()" style="width: 110px;height:22px;" id="ddlBorderColour" class="NormalTextBox" >
                            <option  value=""></option>
                              <option style="border-color:Aqua;border-style:solid;"  value="Aqua">Aqua</option>
                              <option style="border-color:Black;border-style:solid;"  value="Black">Black</option>
                               <option style="border-color:Blue;border-style:solid;"  value="Blue">Blue</option>
                                <option style="border-color:Fuchsia;border-style:solid;"  value="Fuchsia">Fuchsia</option>
                                 <option style="border-color:Gray;border-style:solid;"  value="Gray">Gray</option>
                                  <option style="border-color:Green;border-style:solid;"  value="Green">Green</option>
                                   <option style="border-color:Lime;border-style:solid;"  value="Lime">Lime</option>
                                    <option style="border-color:Maroon;border-style:solid;"  value="Maroon">Maroon</option>
                                     <option style="border-color:Navy;border-style:solid;"  value="Navy">Navy</option>
                                      <option style="border-color:Olive;border-style:solid;"  value="Olive">Olive</option>
                                       <option style="border-color:Orange;border-style:solid;"  value="Orange">Orange</option>
                                        <option style="border-color:Purple;border-style:solid;"  value="Purple">Purple</option>
                                         <option style="border-color:Red;border-style:solid;"  value="Red">Red</option>
                                          <option style="border-color:Silver;border-style:solid;"  value="Silver">Silver</option>
                                           <option style="border-color:Teal;border-style:solid;"  value="Teal">Teal</option>
                                             <option style="border-color:White;border-style:solid;"  value="White">White</option>
                                               <option style="border-color:Yellow;border-style:solid;"  value="Yellow">Teal</option>
                                               
                        </select>


                    </td>
                </tr>
                <tr>
                    <td>

                          Border:
                          <select onchange="new_fct()" style="width: 110px;height:22px;" id="ddlBorder" class="NormalTextBox" >
                                 <option  value=""></option>
                                 <option style="border: 0px solid;"  value="0">0</option>
                                   <option style="border: 1px solid;"  value="1">1</option>
                                     <option style="border: 2px solid;"  value="2">2</option>
                                       <option style="border: 3px solid;"  value="3">3</option>
                                         <option style="border: 4px solid;"  value="4">4</option>
                                           <option style="border: 5px solid;"  value="5">5</option>
                                
                                                              
                          </select>

                    </td>
                     <td style="width:20px;"></td>
                    <td align="left" style="text-decoration:line-through;">
                         <input type="checkbox" id="chkStrikethrough" onchange="new_fct()" />Strikethrough
                    </td>
                     <td style="width:20px;"></td>
                    <td>

                         Margin Width:&nbsp;<input id="txtMargin" style="width:105px;"  onkeyup="new_fct();"
                                        value="" type="text" class="NormalTextBox" />                    
                                
                                                              
                         


                    </td>
                </tr>

            </table>
        </div>
        <div>
            <div>
                <span id="zonetextnew"></span>
            </div>
                     <br />
            <div id="csscode1">
            </div>
        
        </div>

        <div style="padding-top:10px;">
            <table>
                <tr>
                    <td>
                         <a class="btn" id="btnSave" href="#" onclick="GetBackValue();" > <strong>Save</strong></a> 
                    </td>
                    <td style="width:10px;">
                    </td>
                    <td>
                         <a class="btn" id="btnClose" href="#" onclick="javascript:parent.$.fancybox.close();" > <strong>Close</strong></a>
                    </td>
                </tr>
            </table>

           
         

            
        </div>
    </div>

</asp:Content>

