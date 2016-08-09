<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EachMap.aspx.cs" Inherits="Pages_DocGen_EachMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
            <link href="~/App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />

    <title></title>
     <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false">
     </script>
    <script type="text/javascript" src="../../script/jquery.js"></script>

 
</head>
<body style="background-image:none; background-color:#ffffff; padding-top:10px;">
    <form id="form1" runat="server">
    <div>
    <asp:HiddenField ID="hfMaptype" runat="server" Value="roadmap" ClientIDMode="Static" />
        <asp:HiddenField ID="hfLayout" runat="server" Value="1" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfCentreLat" Value="-33.87365" ClientIDMode="Static">
        </asp:HiddenField>
        <asp:HiddenField runat="server" ID="hfCentreLong" Value="151.20688960000007" ClientIDMode="Static">
        </asp:HiddenField>
        <asp:HiddenField runat="server" ID="hfForceMapCenter" Value="no" ClientIDMode="Static">
        </asp:HiddenField>
        <asp:HiddenField runat="server" ID="hfOtherZoomLevel" Value="4" ClientIDMode="Static">
        </asp:HiddenField>
        <%--<asp:HiddenField runat="server" ID="hfGunPoints" ClientIDMode="Static" />--%>

        <div runat="server" id="divMAP" >
            <div id="map_canvas" style="width: 450px; height: 450px;" runat="server" clientidmode="Static">
            </div>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTableMap" DataTextField="TableName" ClientIDMode="Static"
                            DataValueField="TableID" CssClass="NormalTextBox" onchange="ShowSSWhenClicked()">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%--<asp:CheckBox runat="server" ID="chkShowWorkSite" ClientIDMode="Static" />--%>
                        <%--<asp:Label runat="server" ID="lblShowWorkSite" Text="Show Work Sites" Style="font-size:12px;"></asp:Label>--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>

       <script type="text/javascript">
           var map;
           var marker;
           var markersArray = [];

//           var gunpoints;


           function GetMapTypeID() {
               if (document.getElementById("hfMaptype").value == 'hybrid') {
                   return google.maps.MapTypeId.HYBRID;

               }

               if (document.getElementById("hfMaptype").value == 'terrain') {
                   return google.maps.MapTypeId.TERRAIN;

               }
               if (document.getElementById("hfMaptype").value == 'satellite') {
                   return google.maps.MapTypeId.SATELLITE;

               }
               if (document.getElementById("hfMaptype").value == 'roadmap') {
                   return google.maps.MapTypeId.ROADMAP;

               }

               return google.maps.MapTypeId.HYBRID;
           }

           function initialize() {

               var mapType = GetMapTypeID();
               var myOptions = {
                   zoom: parseFloat(document.getElementById('hfOtherZoomLevel').value),
                   maxZoom: 20, scrollwheel: false,
                   center: new google.maps.LatLng(document.getElementById('hfCentreLat').value, document.getElementById('hfCentreLong').value),
                   mapTypeId: mapType,
                   optimized: false
               };
               if (document.getElementById('hfLayout').value == '1') {
                   map = new google.maps.Map(document.getElementById('map_canvas'),
            myOptions);
               }
               else {
                   map = new google.maps.Map(document.getElementById('map_canvas'),
            myOptions);
               }
//               gunpoints = document.getElementById("hfGunPoints").value;

//               google.maps.event.addListener(map, 'center_changed', function () {

//                   var markerc = new google.maps.Marker({
//                       position: new google.maps.LatLng(map.getCenter().lat(), map.getCenter().lng()),
//                       map: map,
//                       icon: gunpoints
//                   });

//                   for (var i = 0; i < markersArray.length; i++) {
//                       if (markersArray[i].icon == gunpoints) {
//                           markersArray[i].setMap(null);
//                       }
//                   }
//                   markersArray.push(markerc);

//               });

           }
           var hfForceMapCenter = document.getElementById('hfForceMapCenter');

           google.maps.event.addDomListener(window, 'load', initialize);


           var bounds = new google.maps.LatLngBounds();


           function addMarker(lat, lng, no, image, Location, url, ssid, mappopup) {

               var bAdd = true;
               //try
               //{
               //    if (markersArray) {
               //        for (var i = 0; i < markersArray.length; i++) {
               //            //markersArray[i].setMap(null);
                          
               //            if (parseFloat(markersArray[i].lat).toFixed(6) == parseFloat(lat).toFixed(6) && parseFloat(markersArray[i].lng).toFixed(6) == parseFloat(lng).toFixed(6)) {
               //                bAdd = false;
               //            }
               //        }
               //    }
               //}
               //catch(err)
               //{
               //    //
               //}
               
               if (bAdd)
               {
                   marker = new google.maps.Marker({
                       position: new google.maps.LatLng(lat, lng),
                       map: map,
                       icon: image,
                       title: Location,
                       optimized: false
                   });

                   markersArray.push(marker);

                   var ll = new google.maps.LatLng(lat, lng);
                   bounds.extend(ll);
                   if (document.getElementById('hfForceMapCenter').value == 'no') {
                       map.fitBounds(bounds);
                   }

                   // setssinfo(Location, marker, url, ssid);
                   setinfo(mappopup, marker, url);
               }
               

           };

           function addMarkerB(lat, lng, no, image, Location, url) {

               marker = new google.maps.Marker({
                   position: new google.maps.LatLng(lat, lng),
                   map: map,
                   icon: image,
                   title: Location,
                   optimized: false
               });



               markersArray.push(marker);


               var ll = new google.maps.LatLng(lat, lng);
               bounds.extend(ll);
               if (document.getElementById('hfForceMapCenter').value == 'no') {
                   map.fitBounds(bounds);
               }

               setinfo(Location, marker, url);

           };


           function setinfo(Location, marker, url) {
               var infowindow = new google.maps.InfoWindow({
                   content: '<div> <strong><a target="_parent" style="text-decoration: none !important;color: inherit;" href="' + url + '">' + Location + '</a></strong></div>'
               });

               google.maps.event.addListener(marker, 'click', function () {
                   infowindow.open(map, marker);

               });

           }

           function setssinfo(Location, marker, url, ssid) {


               var vcontent = ' <strong><a target="_parent" href="' + url + '">' + Location + '</a></strong><br/> ';
               $.ajax({
                   url: '../../GetLocations.aspx?LocationID=' + ssid,
                   dataType: 'json',
                   success: function (res) {
                       var points = res;


                   },
                   error: function (xhr, err) {
                       vcontent = vcontent + xhr.responseText;
                       //alert(  vcontent);
                       var infowindow = new google.maps.InfoWindow({
                           content: vcontent
                       });

                       google.maps.event.addListener(marker, 'click', function () {
                           infowindow.open(map, marker);

                       });
                   }
               });

               //alert( vcontent);


           }

           google.maps.Map.prototype.clearOverlays = function () {
               if (markersArray) {
                   for (var i = 0; i < markersArray.length; i++) {
                       markersArray[i].setMap(null);
                   }
               }
           }

           function ShowSSWhenClicked() {
//               var chk;
//               if (document.getElementById('hfLayout').value == '1') {
//                   chk = document.getElementById("chkShowWorkSite");
//               }
//               else {
//                   chk = document.getElementById("chkShowWorkSite2");
//               }
//               chk.checked = false;
               ShowLocations();
               if (document.getElementById('hfForceMapCenter').value == 'yes') {
                   map.setCenter(new google.maps.LatLng(document.getElementById('hfCentreLat').value, document.getElementById('hfCentreLong').value));
               }

           }

           function ShowLocations() {

               bounds = new google.maps.LatLngBounds();
               if (document.getElementById('hfForceMapCenter').value == 'no') {

                   if (map != null) {
                       map.clearOverlays();
                   }
               }
               if (document.getElementById('hfForceMapCenter').value == 'yes') {

                   deleteOverlays();
               }

               var TableID;
               if (document.getElementById('hfLayout').value == '1') {
                   TableID = parseInt($("#ddlTableMap").val());
               }
               else {
                   TableID = parseInt($("#ddlTableMap2").val());
               }

//               var markerc = new google.maps.Marker({
//                   position: new google.maps.LatLng(map.getCenter().lat(), map.getCenter().lng()),
//                   map: map,
//                   icon: gunpoints
//               });

//               for (var i = 0; i < markersArray.length; i++) {
//                   if (markersArray[i].icon == gunpoints) {
//                       markersArray[i].setMap(null);
//                   }
//               }
//               markersArray.push(markerc);


               // alert(String(TableID));
               if (TableID > -2) {
                   var d = new Date();

                   $.ajax({
                       url: '../../GetLocation.ashx?Location=yes&TableID=' + TableID + '&t=' + d,
                       dataType: 'json',
                       success: function (res) {
                           var points = res;

                           if (points != null)
                           {
                               for (var i = 0; i < points.length; i++) {
                                   var lat = points[i].lat;
                                   var lon = points[i].lon;
                                   addMarker(lat, lon, i, points[i].pin, points[i].title, points[i].url, points[i].ssid, points[i].mappopup);

                               }
                           }
                           
                       },
                       error: function (xhr, err) {
                           //alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
//                           alert("responseText: " + xhr.responseText);


                       }
                   });
               }


           }

           // Shows any overlays currently in the array
           function showOverlays() {
               if (markersArray) {
                   for (i in markersArray) {
                       markersArray[i].setMap(map);
                   }
               }
           }

           // Deletes all markers in the array by removing references to them
           function deleteOverlays() {
               if (markersArray) {
                   for (i in markersArray) {
                       markersArray[i].setMap(null);
                   }
                   markersArray.length = 0;
               }
           }

           function ShowWorkSites() {


               if (document.getElementById('hfForceMapCenter').value == 'no') {
                   map.clearOverlays();
               }

               var TableID;
               if (document.getElementById('hfLayout').value == '1') {
                   TableID = parseInt($("#ddlTableMap").val());
               }
               else {
                   TableID = parseInt($("#ddlTableMap2").val());
               }

               if (TableID > -2) {
                   var d = new Date();
                   $.ajax({
                       url: '../../GetLocations.aspx?WorkSite=yes',
                       dataType: 'json',
                       success: function (res) {
                           var points = res;

                           for (var i = 0; i < points.length; i++) {
                               var lat = points[i].lat;
                               var lon = points[i].lon;
                               addMarkerB(lat, lon, i, points[i].pin, points[i].title, points[i].url);

                           }
                       },
                       error: function (xhr, err) {
                           //alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
                           //alert("responseText: " + xhr.responseText);                           
                       }
                   });
               }


               if (document.getElementById("hfCentreLat").value != '' && document.getElementById('hfForceMapCenter').value == 'yes' &&
                         document.getElementById("hfCentreLong").value != '') {

                   var locationx = new google.maps.LatLng(document.getElementById("hfCentreLat").value, document.getElementById("hfCentreLong").value);

                   addMarkerB(document.getElementById("hfCentreLat").value, document.getElementById("hfCentreLong").value, -1, null, 'Center', '');

               }




           }

          


           if (window.addEventListener)
               window.addEventListener("load", ShowLocations, false);
           else if (window.attachEvent)
               window.attachEvent("onload", ShowLocations);
           else if (document.getElementById)
               window.onload = ShowLocations;

     
       


    </script>
</body>

</html>
