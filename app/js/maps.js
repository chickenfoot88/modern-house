function initMap(id) {

  var mapOptions = {
    center: { lat: 55.800070, lng: 49.105291 },
    zoom: 12
  }

  var map = new google.maps.Map(document.getElementById(id), mapOptions);

  function getMarkerPos() {
    marker.lat = marker.getPosition().lat();
    marker.lng = marker.getPosition().lng();
    console.log(marker.lat);
    console.log(marker.lng);
  };

  var marker;

  function placeMarker(location) {

    if ( marker ) {
      marker.setPosition(location);
    } else {
      marker = new google.maps.Marker({
        position: location,
        map: map,
        icon: {
          path: fontawesome.markers.TRASH,
          scale: 0.6,
          strokeWeight: 0.2,
          strokeColor: 'black',
          strokeOpacity: 1,
          fillColor: '#ff431e',
          fillOpacity: 0.8
        },
        title: 'Контейнер №1'
      });
    };

  };

  google.maps.event.addListener(map, 'click', function(event) {
    placeMarker(event.latLng);
    getMarkerPos();
  });

};

var addContainerId = 'add-container-map';
var editContaiinerId = 'edit-container-map';
var addPolygonId = 'add-polygon-map';
var editPolygonId = 'edit-polygon-map';


$('#add-container').on('shown.bs.modal', function () {
  initMap(addContainerId);
});

$('#edit-container').on('shown.bs.modal', function () {
  initMap(editContaiinerId);
});

$('#add-polygon').on('shown.bs.modal', function () {
  initMap(addPolygonId);
});

$('#edit-polygon').on('shown.bs.modal', function () {
  initMap(editPolygonId);
});
