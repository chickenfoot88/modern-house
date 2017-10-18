function initMap(id) {

  var myLatlng = {lat: 55.832842, lng: 49.075224};

  var mapOptions = {
    center: myLatlng,
    zoom: 18
  }

  var map = new google.maps.Map(document.getElementById(id), mapOptions);

  var marker = new google.maps.Marker({
    position: myLatlng,
    map: map,
    title: 'Контейнер №1'
  });

  marker.setMap(map);

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
