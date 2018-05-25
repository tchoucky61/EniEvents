var mapCenter;
var eventMap;
var mapMarkers = [];
var infowindows = [];


// Google maps.
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
    bounds = new google.maps.LatLngBounds();
}
function initEventMap() {
    mapCenter = { lat: 48.087266, lng: -1.6777926 };
    eventMap = new google.maps.Map(document.getElementById('eventMap'), {
        zoom: 12,
        center: mapCenter
    });
    deleteMarkers();
}

var iconBase = '/Content/img/markers/';

(function ($) {
    var gmapApiUrl = 'https://maps.googleapis.com/maps/api/geocode/json?address={address}CA&key={key}';
    var gmapApiKey = 'AIzaSyB_7BsCOJKiwRPrRWoPMCm8C7QqLO_4Y-4';

    // On document ready.
    $(document).ready(function () {
        // Materialize.
        $('select').formSelect();
        $('.sidenav').sidenav();
        M.updateTextFields();

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
                        $('#Event_LatDisplay').val(lat);
                        $('#Event_LongDisplay').val(lng);
                        M.updateTextFields();
                    }
                    else {
                        alert('Impossible de géolocaliser cette addresse.');
                    }
                });
            }
            else {
                alert('Vous devez renseigner les champs adresse, ville et code postal.');
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
                    url: '/Public/Home/EventJson/' + period + '/' + themaId,
                    dataType: 'json',
                })
               .done(function (data) {
                   deleteMarkers();
                       $('.search--result span').text(data.length);
                   if (data.length) {
                       for (var i = 0; i < data.length; i++) {
                           var image = 'bidule.png';
                           var infoContent = '<div class="infowindow--title bernadette">' + data[i].Title + '</div>';
                           infoContent += '<div class="infowindow--thema thema-' + data[i].Thema.Id + '"';
                           if (data[i].Pictures.length > 0) {
                               infoContent += 'style="background-image:url(/Content/media/img/' + data[i].Pictures[0].FileName + ');background-opacity:.7;"';
                           }
                           infoContent += '><div class="thema-' + data[i].Thema.Id + '">' + data[i].Thema.Title + '</div></div>';
                           infoContent += '<div class="infowindow--content">';
                           infoContent += data[i].Description.length > 200 ? data[i].Description.substring(0, 197) + '...' : data[i].Description;
                           infoContent += '</div>';
                           infoContent += '<div class="infowindow--link"><a href="#!" data-url="/Public/Home/EventDetails/' + data[i].Id + '" data-title="' + data[i].Title + '">Plus de détails</a></div>';
                           var marker = new google.maps.Marker({
                               map: eventMap,
                               title: data[i].Title,
                               animation: google.maps.Animation.DROP,
                               position: { lat: parseFloat(data[i].Lat), lng: parseFloat(data[i].Long) },
                               icon: iconBase + data[i].Thema.Id + '.png',
                           });
                           loc = new google.maps.LatLng(marker.position.lat(), marker.position.lng());
                           bounds.extend(loc);
                           infowindows.push(new google.maps.InfoWindow({
                               content: infoContent,
                               maxWidth: 350,
                               minWidth: 300,
                           }));
                           marker.addListener('click', function () {
                               for (var i = 0; i < infowindows.length; i++) {
                                   infowindows[i].close();
                               }
                               infowindows[mapMarkers.indexOf(this)].open(eventMap, this);
                               $('.infowindow--link a').click(showEventDetails);
                           });

                           mapMarkers.push(marker);
                       }
                       eventMap.fitBounds(bounds);
                       eventMap.panToBounds(bounds);
                   }
               });
            });
            $('#Periods').trigger('change');

            var $eventModal = $('#modalEventDetail').modal();
            console.log($eventModal);
            var showEventDetails = function (event) {
                event.preventDefault();
                event.stopPropagation();
                $('#modalEventDetail .modal-title').html($(this).data('title'));
                $('#modalEventDetail .modal-content').html('');
                var url = $(this).data('url');
                $.ajax({
                    url: url,
                    dataType: 'text',
                })
               .done(function (data) {
                   $('#modalEventDetail .modal-content').html(data);
                   $('#modalEventDetail').modal('open');


                   $('.event--picture').first().addClass('selected');
                   $('.event--picture').on('click', function () {
                       $('.event--picture').removeClass('selected');
                       var pictureUrl = $(this).data('picture-url');
                       $(this).addClass('selected');
                       $('.event--header').css('background-image', 'url(' + pictureUrl + ')');
                   });

                   $('#getParkListBtn').click(function () {
                       $('#eventParkList').html('<div class="col spinner"><i class="material-icons" style="vertical-align:middle">hourglass_empty</i> Recherche en cours ... Veuillez patienter.</div>');
                       var startAddress = $('#userAddress').val();
                       var eventId = $('#eventId').text();
                       $.ajax({
                           url: '/Public/Home/ParkList/' + eventId + '/' + startAddress,
                           dataType: 'text',
                       })
                      .done(function (data) {
                          $('#eventParkList').html(data);
                      });
                   });
               });
            }            
        }

        // Event edit.
        $('.form-event--picture-delete').click(function (e) {
            var eventPictureId = $(this).data('event-picture-id');
            $('#eventPicture_' + eventPictureId).remove();
        });
    });

})(jQuery)