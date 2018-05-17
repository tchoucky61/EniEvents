(function ($) {
    var gmapApiUrl = 'https://maps.googleapis.com/maps/api/geocode/json?address={address}CA&key={key}';
    var gmapApiKey = 'AIzaSyBj842_CpEx66J2_YKNGpXF676QJpgo4z8';

    // On document ready.
    $(document).ready(function () {
        // Materialize.
        $('select').formSelect();

        // Event geolocation.
        $('#geolocateEventBtn').on('click', function () {
            var address = $('#Event_Address').val();
            var city = $('#Event_City').val();
            var zipcode = $('#Event_Zipcode').val();

            if (address.length && city.length && zipcode.length) {
                var apiUrl = gmapApiUrl.replace('{address}', address + '+' + city + '+' + zipcode);
                apiUrl = apiUrl.replace('{key}', gmapApiKey);

                $.ajax({
                    url: apiUrl,
                    dataType: 'json',

                })
                .done(function (data) {
                    if (data.status === "OK") {
                        var lat = data.results[0]['geometry']['location']['lat'];
                        var lng = data.results[0]['geometry']['location']['lng'];
                        $('#Event_Lat').val(lat);
                        $('#Event_Long').val(lng);
                        $('#Event_LatDisplay').text(lat);
                        $('#Event_LongDisplay').text(lng);
                    }
                    else {
                        alert('Impossible de géolocaliser cette addresse.');
                    }
                });
            }
            else {
                alert('Vous devez renseigner les champs addresse, ville et code postal.');
            }
        });

        // Home.
        if ($('#eventMap').length) {
            $('#Periods, #fooSelectedId').on('change', function () {
                var period = $('#Periods').val();
                var themaId = $('#fooSelectedId').val() != '' ? $('#fooSelectedId').val() : 0;

                console.log(period);
                console.log(themaId);
                $.ajax({
                    url: '/event/get/' + period + '/' + themaId,
                    dataType: 'json',

                })
               .done(function (data) {
                   deleteMarkers();
                   if (data.length) {
                       for (var i = 0; i < data.length; i++) {
                           var image = 'bidule.png';
                           var infoContent = '<div class="infowindow--title">' + data[i].Title + '</div>';
                           infoContent += '<div class="infowindow--description">';
                           infoContent += data[i].Description.length > 100 ? data[i].Description.substring(0, 97) + '...' : data[i].Description;
                           infoContent += '</div>';
                           infoContent += '<div class="infowindow--link"><a href="#!" data-url="event/details/' + data[i].Id + '" data-title="' + data[i].Title + '">Plus de détails</a></div>';
                           var marker = new google.maps.Marker({
                               map: eventMap,
                               title: data[i].title,
                               animation: google.maps.Animation.DROP,
                               position: { lat: parseFloat(data[i].Lat), lng: parseFloat(data[i].Long) },
                           });
                           infowindows.push(new google.maps.InfoWindow({
                               content: infoContent,
                               maxWidth: 300,
                           }));
                           marker.addListener('click', function () {
                               infowindows[mapMarkers.indexOf(this)].open(eventMap, this);
                               $('.infowindow--link a').click(showEventDetails);
                           });

                           mapMarkers.push(marker);
                       }
                   }
               });
            });
            $('#Periods').trigger('change');

            var showEventDetails = function (event) {
                event.preventDefault();
                event.stopPropagation();
                $('#popIn .modal-title').html($(this).data('title'));
                $('#popIn .modal-body').html('');
                var url = $(this).data('url');
                $.ajax({
                    url: url,
                    dataType: 'text',
                })
               .done(function (data) {
                   $('#popIn .modal-body').html(data);
                   $('#popin').show();
               });
            }
        }
    });

})(jQuery)

var mapCenter;
var eventMap;
var mapMarkers = [];
var infowindows = [];


// Google maps.
function initEventMap() {
    mapCenter = { lat: 48.117266, lng: -1.6777926 };
    eventMap = new google.maps.Map(document.getElementById('eventMap'), {
        zoom: 13,
        center: mapCenter
    });
}

function setMapOnAll(map) {
    for (var i = 0; i < mapMarkers.length; i++) {
        mapMarkers[i].setMap(map);
    }
}

function clearMarkers() {
    setMapOnAll(null);
}

function deleteMarkers() {
    clearMarkers();
    mapMarkers = [];
    infowindows = [];
}