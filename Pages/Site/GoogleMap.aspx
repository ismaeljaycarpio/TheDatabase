<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoogleMap.aspx.cs" Inherits="Pages_Site_GoogleMap"
    MasterPageFile="~/Home/Popup.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.0/jquery.min.js"></script>--%>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>


    <script language="javascript" type="text/javascript">
        function abc() {
            var b = document.getElementById('<%= lnkSEarch.ClientID %>');
            if (b && typeof (b.click) == 'undefined') {
                b.click = function () {
                    var result = true;
                    if (b.onclick) result = b.onclick();
                    if (typeof (result) == 'undefined' || result) {
                        eval(b.getText('href'));
                    }
                }
            }

        }
    </script>


    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <div id="Content" class="ContentMain">
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="text-align: right">
                </td>
                <td style="padding-left: 25px;">
                    <h1>
                        <asp:Label ID="lblTitle" runat="server" Text=" Choose Location"> </asp:Label>
                    </h1>
                    <div align="left">
                        <table>
                            <tr runat="server" id="trMainSave">
                                <td>
                                    <input type="text" value="" id="searchbox" style="width: 400px; height: 30px; font-size: 15px;" >
                                    <%--<asp:TextBox runat="server" ClientIDMode="Static" ID="searchbox" CssClass="NormalTextBox" Width="400px"></asp:TextBox>--%>
                                </td>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:HiddenField runat="server" ID="hfLat" Value="-33.873651" ClientIDMode="Static" />
                                                <asp:HiddenField runat="server" ID="hfLng" Value="151.20688960000007" ClientIDMode="Static" />
                                                <asp:HiddenField runat="server" ID="hfType" Value="Location" ClientIDMode="Static" />
                                                <asp:HiddenField runat="server" ID="hfPath" Value="" ClientIDMode="Static" />
                                                <div>
                                                    
                                                                <asp:LinkButton runat="server" ID="lnkSEarch" OnClientClick="showAddress(); return false"
                                                                    CssClass="btn" CausesValidation="false"> <strong>Search</strong></asp:LinkButton>
                                                           
                                                </div>
                                            </td>
                                            <td>
                                                <div runat="server" id="divSave">
                                                    
                                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" CausesValidation="false"
                                                                    OnClientClick="javascript:return GetBackValue();"> <strong>Save</strong></asp:LinkButton>
                                                           
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                   
                                                                <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                                                    CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                                          
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div align="left" id="map" style="width: 400px; height: 400px; margin-top: 10px;">
                                    </div>
                                </td>
                                <td style="padding-left: 25px; width: 350px;">
                                    <asp:Label runat="server" ID="lblContentCommon"></asp:Label>
                                    <%--<DBGurus:DBGContent ID="dbgContentCommon" runat="server" ConnectionName="CnString"
                                        ContentKey="ChooseLocationHelp" TableName="Content" ExtenderPath="Extender/"
                                        ShowInlineContentEditor="false" UseAssetManager="true" />--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td align="left">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    <asp:HiddenField runat="server" ID="hfCentreLat" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfCentreLong" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfFlag" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfOtherZoomLevel" Value="18" 
                 ClientIDMode="Static">   </asp:HiddenField>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                </td>
                <td colspan="3">
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        var flag = document.getElementById("hfFlag").value;

        var mapOptions = {
            zoom: parseFloat(document.getElementById('hfOtherZoomLevel').value),
            mapTypeId: google.maps.MapTypeId.HYBRID,
            center: new google.maps.LatLng(document.getElementById("hfLat").value, document.getElementById("hfLng").value)
        };

        var map = new google.maps.Map(document.getElementById("map"), mapOptions);

        var geocoder = new google.maps.Geocoder();

        function SetCentreFlag() {
            if (document.getElementById("hfCentreLat").value != '' &&
                         document.getElementById("hfCentreLong").value != '') {

                var locationx = new google.maps.LatLng(document.getElementById("hfCentreLat").value, document.getElementById("hfCentreLong").value);

                var markerx = new google.maps.Marker({
                    position: locationx,
                    map: map,
                    icon: flag
                });

            }

        }

        SetCentreFlag();

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

            var address = document.getElementById("searchbox").value;

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


        function GetBackValue() {

            var hfType = document.getElementById("hfType");
            if (hfType.value == "account") {
                window.parent.document.getElementById("ctl00_HomeContentPlaceHolder_TabContainer1_tabHome_txtLatitude").value = map.getCenter().lat();
                window.parent.document.getElementById("ctl00_HomeContentPlaceHolder_TabContainer1_tabHome_txtLongitude").value = map.getCenter().lng();
                window.parent.document.getElementById('hlChoose').href = document.getElementById("hfPath").value + 'GoogleMap.aspx?type=account&lat=' + map.getCenter().lat() + "&lng=" + map.getCenter().lng() + "&zoom=" + map.getZoom();

                var iZoom = map.getZoom();

                if (iZoom > 10 && iZoom < 21) {
                    parent.$("#ctl00_HomeContentPlaceHolder_TabContainer1_tabHome_ddlScale").val(iZoom);
                }
                else {
                    parent.$("#ctl00_HomeContentPlaceHolder_TabContainer1_tabHome_ddlScale").val("-1");
                }
                window.parent.SetLocationFromPop();
                parent.$.fancybox.close();

            }
            else if (hfType.value == "mapsection") {
                window.parent.document.getElementById("ctl00_HomeContentPlaceHolder_txtLatitude").value = map.getCenter().lat();
                window.parent.document.getElementById("ctl00_HomeContentPlaceHolder_txtLongitude").value = map.getCenter().lng();
                window.parent.document.getElementById('hlChoose').href = document.getElementById("hfPath").value + 'GoogleMap.aspx?type=mapsection&lat=' + map.getCenter().lat() + "&lng=" + map.getCenter().lng() + "&zoom=" + map.getZoom();

                var iZoom = map.getZoom();

                if (iZoom > 10 && iZoom < 21) {
                    parent.$("#ddlScale").val(iZoom);
                    window.parent.document.getElementById("hfOtherZoomLevel").value = iZoom;
                }
                else {
                    parent.$("#ddlScale").val("-1");
                }

                window.parent.SetLocationFromPop();
                parent.$.fancybox.close();

            }
            else {

                window.parent.document.getElementById("ctl00_HomeContentPlaceHolder_txtLatitude").value = map.getCenter().lat();
                window.parent.document.getElementById("ctl00_HomeContentPlaceHolder_txtLongitude").value = map.getCenter().lng();
                window.parent.document.getElementById('hlChoose').href = 'GoogleMap.aspx?type=Location&lat=' + map.getCenter().lat() + "&lng=" + map.getCenter().lng() + "&zoom=" + map.getZoom();
                window.parent.SetLocationFromPop();
                parent.$.fancybox.close();
            }


        }
   


    </script>



</asp:Content>
