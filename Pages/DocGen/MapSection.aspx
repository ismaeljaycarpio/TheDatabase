<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="MapSection.aspx.cs" Inherits="DocGen.Document.MapSection.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false">
    </script>
    <div>
        <div>
            <asp:HiddenField ID="hfMaptype" runat="server" Value="roadmap" ClientIDMode="Static" />
            <asp:HiddenField ID="hfLayout" runat="server" Value="1" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hfCentreLat" Value="-33.87365" ClientIDMode="Static">
            </asp:HiddenField>
            <asp:HiddenField runat="server" ID="hfCentreLong" Value="151.20688960000007" ClientIDMode="Static">
            </asp:HiddenField>
            <asp:HiddenField runat="server" ID="hfForceMapCenter" Value="no" ClientIDMode="Static">
            </asp:HiddenField>
            <asp:HiddenField runat="server" ID="hfOtherZoomLevel" Value="18" ClientIDMode="Static">
            </asp:HiddenField>
            <asp:HiddenField runat="server" ID="hfImage" ClientIDMode="Static" Value='../Record/PINImages/DefaultPin.png' />
            <asp:HiddenField runat="server" ID="hfFlag" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hfGunPoints" ClientIDMode="Static" />
            <table>
                <tr>
                    <td>
                        <div id="map_canvas" style="width: 450px; height: 450px;">
                        </div>
                    </td>
                    <td style="padding-left: 20px; vertical-align: top;">
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="Map"></asp:Label>
                                </td>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>
                                                <%--<div runat="server" id="div21">
                                                    <asp:LinkButton runat="server" ID="CancelButton" CausesValidation="false" OnClientClick="parent.$.fancybox.close(); return false; ">
                                                        <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                                    </asp:LinkButton>
                                                </div>--%>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="SaveButton" OnClick="SaveButton_Click">
                                                    <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                        ToolTip="Save" />
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 50px;">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <strong>Map Centre</strong>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Address</strong>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddress" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                        Width="200px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="right">
                                    <asp:LinkButton runat="server" ID="lnkSEarch" OnClientClick="showAddress(); return false"
                                        CausesValidation="false"> <strong>Search</strong></asp:LinkButton>
                                    <%--<asp:HyperLink runat="server" NavigateUrl="~/Pages/Site/GoogleMap.aspx?type=mapsection&lat=-33.873651&lng=151.20688960000007"
                                        CssClass="googlemap" ID="hlChoose" ClientIDMode="Static">Search</asp:HyperLink>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Latitude</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLatitude" runat="server" CssClass="NormalTextBox" onblur="ReCentreMap();"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Longitude</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLongitude" runat="server" CssClass="NormalTextBox" onblur="ReCentreMap();"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <strong>Dimensions</strong>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Height</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHeight" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtHeight"
                                        runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Width</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtWidth" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtWidth"
                                        runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr style="height: 25px;">
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <%--<tr>
                                <td align="right">
                                    <strong>Map Scale</strong>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlScale" runat="server" CssClass="NormalTextBox" ClientIDMode="Static"
                                        onchange="ChangeScale()">
                                        <asp:ListItem Selected="True" Text="Auto" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="11 – City" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13 - City Area" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15 - Suburb" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17 - Street" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20 - House" Value="20"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="left">
                                    <strong>Show</strong>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong runat="server" id="stgShowLocation">Locations</strong>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTableMapPop" runat="server" CssClass="NormalTextBox" DataTextField="TableName"
                                        ClientIDMode="Static" onchange="ShowSSWhenClicked()" DataValueField="TableID"
                                        Width="200px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ForeColor="Red" ID="lblMsg"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        var map;
        var marker;
        var markersArray = [];

        var image = document.getElementById("hfImage").value;
        var flag = document.getElementById("hfFlag").value;
        var gunpoints = document.getElementById("hfGunPoints").value;


        var mapType = GetMapTypeID();

        var myOptions = {
            zoom: parseFloat(document.getElementById('hfOtherZoomLevel').value),
            maxZoom: 20,
            center: new google.maps.LatLng(document.getElementById('hfCentreLat').value, document.getElementById('hfCentreLong').value),
            mapTypeId: mapType
        };
        map = new google.maps.Map(document.getElementById('map_canvas'),
            myOptions);

        //}

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

            ShowLocations();
            //                alert( 'ok');
        }

        var hfForceMapCenter = document.getElementById('hfForceMapCenter');

        //        google.maps.event.addDomListener(window, 'load', initialize);



        var bounds = new google.maps.LatLngBounds();


        function addMarker(lat, lng, no, image, Location, url, ssid) {


            marker = new google.maps.Marker({
                position: new google.maps.LatLng(lat, lng),
                map: map,
                icon: image,
                title: Location
            });

            markersArray.push(marker);

            var ll = new google.maps.LatLng(lat, lng);
            bounds.extend(ll);
            if (document.getElementById('hfForceMapCenter').value == 'no') {
                map.fitBounds(bounds);
            }

            //setssinfo(Location, marker, url, ssid);
            setinfo(Location, marker, url);
        };

        function addMarkerB(lat, lng, no, image, Location, url) {

            marker = new google.maps.Marker({
                position: new google.maps.LatLng(lat, lng),
                map: map,
                icon: image,
                title: Location
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
                content: '<div> <strong><a target="_blank" href="' + url + '">' + Location + '</a></strong></div>'
            });

            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);

            });

        }

        function setssinfo(Location, marker, url, ssid) {


            var vcontent = ' <strong><a target="_blank" href="' + url + '">' + Location + '</a></strong><br/> ';
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

            ShowLocations();
            if (document.getElementById('hfForceMapCenter').value == 'yes') {
                map.setCenter(new google.maps.LatLng(document.getElementById('hfCentreLat').value, document.getElementById('hfCentreLong').value));
            }

        }

        function ReCentreMap() {
            if (document.getElementById("ctl00_HomeContentPlaceHolder_txtLatitude").value != '' &&
                document.getElementById("ctl00_HomeContentPlaceHolder_txtLongitude").value != '') {
                document.getElementById('hfCentreLat').value = document.getElementById("ctl00_HomeContentPlaceHolder_txtLatitude").value;
                document.getElementById('hfCentreLong').value = document.getElementById("ctl00_HomeContentPlaceHolder_txtLongitude").value;

                map.setCenter(new google.maps.LatLng(document.getElementById('hfCentreLat').value, document.getElementById('hfCentreLong').value));


            }
        }

        function ShowLocations() {

            // mapType = document.getElementById("hfMaptype").value;           


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
            TableID = parseInt($("#ddlTableMapPop").val());

            // alert(String(TableID));

            if (TableID > -2) {
                var d = new Date();

                $.ajax({
                    url: '../../GetLocations.aspx?Location=yes&TableID=' + TableID + '&t=' + d,
                    dataType: 'json',
                    success: function (res) {
                        var points = res;

                        for (var i = 0; i < points.length; i++) {
                            var lat = points[i].lat;
                            var lon = points[i].lon;
                            addMarker(lat, lon, i, points[i].pin, points[i].title, points[i].url, points[i].ssid);

                        }
                    },
                    error: function (xhr, err) {
                        //alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
                        //alert("responseText: " + xhr.responseText);


                    }
                });
            }

            var markerc = new google.maps.Marker({
                position: new google.maps.LatLng(map.getCenter().lat(), map.getCenter().lng()),
                map: map,
                icon: gunpoints
            });

            for (var i = 0; i < markersArray.length; i++) {
                if (markersArray[i].icon == gunpoints) {
                    markersArray[i].setMap(null);
                }
            }
            markersArray.push(markerc);


            //            map.setMapTypeId(google.maps.MapTypeId[document.getElementById("hfMaptype").value]);
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
            TableID = parseInt($("#ddlTableMapPop").val());


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

                addMarkerB(document.getElementById("hfCentreLat").value, document.getElementById("hfCentreLong").value, -1, document.getElementById("hfFlag").value, 'Center', '');

            }




        }


        function ChangeScale() {

            //document.getElementById('hfOtherZoomLevel').value = document.getElementById("ddlScale").value;
            //SetLocationFromPop();
        }

        function SetLocationFromPop() {

            if (document.getElementById("ctl00_HomeContentPlaceHolder_txtLatitude").value != '' &&
        document.getElementById("ctl00_HomeContentPlaceHolder_txtLongitude").value != '') {

                var location3 = new google.maps.LatLng(document.getElementById("ctl00_HomeContentPlaceHolder_txtLatitude").value, document.getElementById("ctl00_HomeContentPlaceHolder_txtLongitude").value);
                map.clearOverlays();

                mapOptions2 = {
                    zoom: parseFloat(document.getElementById('hfOtherZoomLevel').value),
                    mapTypeId: google.maps.MapTypeId[document.getElementById("hfMaptype").value],
                    center: location3
                };

                map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions2);



                var marker = new google.maps.Marker({
                    position: location3,
                    map: map,
                    icon: image
                });

                markersArray.push(marker);

                if (document.getElementById("hfCentreLat").value != '' &&
                         document.getElementById("hfCentreLong").value != '') {

                    var locationx = new google.maps.LatLng(document.getElementById("hfCentreLat").value, document.getElementById("hfCentreLong").value);

                    var markerx = new google.maps.Marker({
                        position: locationx,
                        map: map,
                        icon: flag
                    });

                    markersArray.push(markerx);

                }

            }
            else {
                //                document.getElementById("ctl00_HomeContentPlaceHolder_lnkSetLocation").style.display = "none";

            }

        };






        function showAddress() {

            var address = document.getElementById("txtAddress").value;

            //var map = new google.maps.Map(document.getElementById("map"), mapOptions);

            var geocoder = new google.maps.Geocoder();

            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    results[0].geometry.location;

                    var b = new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng());

                    map.setCenter(b);

                    //alert(nlat);
                    //outputGeo(r);
                } else {
                    alert("Google Maps had some trouble finding " + address + ".");
                }
            });

        }



        $(document).ready(function () {

            google.maps.event.addListener(map, 'maptypeid_changed', function () {
                //alert( map.getMapTypeId());
                if (map.getMapTypeId() != null) {
                    document.getElementById("hfMaptype").value = map.getMapTypeId();
                }
            });

            google.maps.event.addListener(map, 'center_changed', function () {
                // alert('ok');

                var txtLatitude = document.getElementById("ctl00_HomeContentPlaceHolder_txtLatitude");
                txtLatitude.value = map.getCenter().lat();
                var txtLongitude = document.getElementById("ctl00_HomeContentPlaceHolder_txtLongitude");
                txtLongitude.value = map.getCenter().lng();
                document.getElementById("hfCentreLat").value = map.getCenter().lat();
                document.getElementById("hfCentreLong").value = map.getCenter().lng();

                var markerc = new google.maps.Marker({
                    position: new google.maps.LatLng(map.getCenter().lat(), map.getCenter().lng()),
                    map: map,
                    icon: gunpoints
                });
                //                if (markersArray.length > 0) {
                //                    markersArray[markersArray.length - 1].setMap(null);
                //                    markersArray.push(markerc);
                //                }

                for (var i = 0; i < markersArray.length; i++) {
                    if (markersArray[i].icon == gunpoints) {
                        markersArray[i].setMap(null);
                    }
                }
                markersArray.push(markerc);

            });


            google.maps.event.addListener(map, 'zoom_changed', function () {
                document.getElementById('hfOtherZoomLevel').value = map.getZoom();

            });


        });


        if (window.addEventListener)
            window.addEventListener("load", initialize, false);
        else if (window.attachEvent)
            window.attachEvent("onload", initialize);
        else if (document.getElementById)
            window.onload = initialize;

     
       


    </script>
</asp:Content>
