<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoogleMapTest.aspx.cs" Inherits="Test_GoogleMapTest" %>

<!DOCTYPE PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head runat="server">
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.0/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
</head>
<body style="background-image: none; background-color: White;">
    <h1>
        Google Maps Location Search with Autocomplete</h1>
    <div align="left" >
        <input type="text" value="" id="searchbox" style="width: 500px; height: 30px; font-size: 15px;">
        <input type="button" id="btnSearch" value="Search" onclick="showAddress(); return false" />
    </div>
    <div align="left" id="map" style="width: 400px; height: 400px; margin-top: 10px;">
    </div>
</body>
</html>
<script type="text/javascript">

    var mapOptions = {
        zoom: 10,
        mapTypeId: google.maps.MapTypeId.HYBRID,
        center: new google.maps.LatLng(41.06000, 28.98700)
    };

    var map = new google.maps.Map(document.getElementById("map"), mapOptions);

    var geocoder = new google.maps.Geocoder();


    $(document).ready(function () {

      

        $(function () {
            $("#searchbox").autocomplete({

                source: function (request, response) {

                    if (geocoder == null) {
                        geocoder = new google.maps.Geocoder();
                    }
                    geocoder.geocode({ 'address': request.term }, function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {

                            var searchLoc = results[0].geometry.location;
                            var lat = results[0].geometry.location.lat();
                            var lng = results[0].geometry.location.lng();
                            var latlng = new google.maps.LatLng(lat, lng);
                            var bounds = results[0].geometry.bounds;

                            geocoder.geocode({ 'latLng': latlng }, function (results1, status1) {
                                if (status1 == google.maps.GeocoderStatus.OK) {
                                    if (results1[1]) {
                                        response($.map(results1, function (loc) {
                                            return {
                                                label: loc.formatted_address,
                                                value: loc.formatted_address,
                                                bounds: loc.geometry.bounds
                                            }
                                        }));
                                    }
                                }
                            });
                        }
                    });
                },
                select: function (event, ui) {
                    var pos = ui.item.position;
                    var lct = ui.item.locType;
                    var bounds = ui.item.bounds;

                    if (bounds) {
                        map.fitBounds(bounds);
                    }
                }
            });
        });



      

    });




    function showAddress() {

        var address =document.getElementById("searchbox").value;        
       
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
                alert("Google Maps had some trouble finding" + address + status);
            }
        });




    }
   


</script>
