// Инициализация карты

function initMap() {

  var mapWrapper = $('.map-container')[0];
  var marker = null;

  // Начальные координаты для отрисовки карты
  var defaultMapOptions = {
    'center': {
      lat: 55.808307,
      lng: 49.101819
    },
    'zoom': 12,
    'mapTypeId': google.maps.MapTypeId.ROADMAP
  };

  var markerLocation = {};

  if ($('.markerLatitude').val() && $('.markerLongitude').val()) {

    markerLocation.lat = Number($('.markerLatitude').val().slice(0,2) + '.' + $('.markerLatitude').val().slice(3,10));
    markerLocation.lng = Number($('.markerLongitude').val().slice(0,2) + '.' + $('.markerLongitude').val().slice(3,10));

  } else {
    markerLocation.lat = 0;
    markerLocation.lng = 0;
  }

  var mapOptions = {
    'center': {
      lat: markerLocation.lat,
      lng: markerLocation.lng
    },
    'zoom': 15,
    'mapTypeId': google.maps.MapTypeId.ROADMAP
  };

  if (markerLocation.lat == 0 || markerLocation.lng == 0) {
    map = new google.maps.Map(mapWrapper, defaultMapOptions);
  } else {
    map = new google.maps.Map(mapWrapper, mapOptions);
    placeMarker(markerLocation);
    getMarkerAddress();
  };


  // Установка маркера
  function placeMarker(location, markersLength, icon, color) {
    var markers = [];

    if ( marker ) {
      marker.setPosition(location);
    } else {
      var mapContainerId = map.getDiv().id;
      var iconPath;
      var iconAnchor;
      var iconAnchorX;
      var iconAnchorY = -25;

      if (mapContainerId== "customer-map") {
        iconPath = fontawesome.markers.BELL;
        iconColor = '#3C8DBC';
        iconAnchorX = 32.5;
      } else if (mapContainerId == "polygon-map") {
        iconPath = fontawesome.markers.RECYCLE;
        iconColor = '#00A65A';
        iconAnchorX = 31.5;
      } else if (mapContainerId == "container-map" || mapContainerId == "request-map"){
        var containerTypeCapacity = $('#containerTypeCapacity').val();

        switch(containerTypeCapacity) {
          case '8,00':
            iconColor = '#00A65A';
            break;
          case '10,00':
            iconColor = '#39CCCC';
            break;
          case '13,00':
            iconColor = '#367FA9';
            break;
          case '15,00':
            iconColor = '#D73925';
            break;
          case '20,00':
            iconColor = '#e08e0b';
            break;
          case '':
            iconColor = '#FF7701';
            break;
        };

        iconPath = fontawesome.markers.TRASH;
        iconColor = '#FF7701';
        iconAnchorX = 24.5;
      } else {
        iconPath = fontawesome.markers.TRUCK;
        iconColor = '#3C8DBC';
        iconAnchorX = 24.5;
      };

      iconAnchor = new google.maps.Point(iconAnchorX, iconAnchorY );
      markersLength = markersLength || 1;

      for (var i = 0; i < markersLength; i++) {
        var loc = location[i] || location;

        if (mapContainerId == "container-map-monitoring") {
          var containerValue = loc.ContainerTypeName.substr(3);
          iconPath = fontawesome.markers.TRASH;

          switch(containerValue) {
            case '8':
              iconColor = '#00A65A';
              break;
            case '10':
              iconColor = '#39CCCC';
              break;
            case '13':
              iconColor = '#367FA9';
              break;
            case '15':
              iconColor = '#D73925';
              break;
            case '20':
              iconColor = '#e08e0b';
              break;
            case '':
              iconColor = '#FF7701';
              break;
          };

          var description = location[i].Number + ' ' + location[i].ContainerTypeName
        };

        // Выключаем перетаскивание маркеров для карт мониторинга
        if (mapContainerId == "container-map-monitoring" || mapContainerId == "cars-map-monitoring") {
          var draggable = false;
        } else {
          draggable = true;
        };

        marker = new google.maps.Marker({
          position: loc,
          map: map,
          icon: {
            path: iconPath,
            scale: 0.5,
            strokeWeight: 0.2,
            strokeColor: 'black',
            strokeOpacity: 1,
            fillColor: iconColor,
            fillOpacity: 0.8,
            anchor: iconAnchor
          },
          title: '',
          draggable: draggable
          });

        if (mapContainerId == 'cars-map-monitoring') {
          var description = location[i].Number;
          bindInfoWindow(marker, map, description);
        };

        if (mapContainerId == "container-map-monitoring") {
          bindInfoWindow(marker, map, description);
        };

        markers.push(marker);
      };

      console.log('clusters');
        var markerCluster = new MarkerClusterer(map, markers, {
          imagePath: '../Content/img/icons/map/m',
          maxZoom: 15,
          minimumClusterSize: 4,
          gridSize: 50
      });
    };

    // Удаление всех маркеров с карты и из массива
    marker.addListener("dblclick", function() {
      if (mapContainerId !== "container-map-monitoring" || mapContainerId !== "cars-map-monitoring") clearMarkers();
    });

    // Обрабатывает перетаскивание маркера
    marker.addListener('dragend', function functionName() {
      getMarkerPos();
      getMarkerAddress();
      setMarkerPosToInput();
    });

  };

  // Удаление всех маркеров c карты и из массива
  function clearMarkers() {
    for (var i = 0; i < markers.length; i++) {
      markers[i].setMap(null);
    }
    marker = "";
  };

  // Получение координаты маркера
  function getMarkerPos() {
    markerLocation.lat = marker.getPosition().lat();
    markerLocation.lng = marker.getPosition().lng();
  };

  function setMarkerPosToInput() {
    $('.markerLatitude').val(markerLocation.lat);
    $('.markerLongitude').val(markerLocation.lng);
  }

  // Установка маркера при клике на карту
  map.addListener('click', function(event){
    var mapContainerId = map.getDiv().id;

    if(mapContainerId !== 'container-map-monitoring' && mapContainerId !== 'cars-map-monitoring') {
      placeMarker(event.latLng);
      getMarkerPos();
      getMarkerAddress();
      setMarkerPosToInput();
    };
  });

  // Запрос на получение адреса по координатам маркера
  function getMarkerAddress() {

    var markerAddress = "";

    var lat = markerLocation.lat.toString().slice(0,9);
    var lng = markerLocation.lng.toString().slice(0,9);

    $.ajax({
      type: 'GET',
      url: 'https://maps.googleapis.com/maps/api/geocode/json?latlng='+lat+','+
      lng+'&location_type=ROOFTOP&result_type=street_address&key=AIzaSyB4NT5_bn3ZPTrYW2Il5YLaBrMHGjzjcjM',
      beforeSend: getMarkerPos(),
      success: function (data) {
        if (data.status == "OK") {
          markerAddress = '<div class="marker-title">Адрес: '+data.results[0].address_components[1].long_name +
          ', ' + data.results[0].address_components[0].long_name+'</div>';
          $('.marker-address').val(data.results[0].address_components[1].long_name + ', ' + data.results[0].address_components[0].long_name);
          hideAddressErrorMsg();
        } else {
          markerAddress = '<div class="marker-title">Не удалось установить адрес. Попробуйте ввести вручную.</div>';
          $('.marker-address').val("Адрес не найден");
          hideAddressErrorMsg();
          showAddressErrorMsg();
        };

        bindInfoWindow(marker, map, markerAddress);
      }
    });
  };

  // Выводит сообщение в случае неустановленного адреса
  function showAddressErrorMsg() {
    $('.marker-address').val("Адрес не найден");
    addressInput.parents('.form-group').addClass('has-warning');
    addressInput.after('<span class="help-block">По указанному адресу не удалось ничего найти</span>');
    addressInput.parents('.form-group').find('label').prepend('<i class="fa fa-exclamation-triangle" aria-hidden="true"></i>  ');
  };

  function hideAddressErrorMsg() {
    addressInput.parents('.form-group').removeClass('has-warning');
    addressInput.parents('.form-group').find('.fa-exclamation-triangle').remove();
    addressInput.parents('.form-group').find('.help-block').remove();
  };

  // Установка маркера и центрирование при введение адреса в input
  var geocoder = new google.maps.Geocoder();

  var addressInput = $('.marker-address');

  addressInput.on('change', function(event) {
    $('.help-block').fadeOut('50');
    addressInput.parents('.form-group').removeClass('has-warning');
    addressInput.parents('.form-group').find('.fa').fadeOut('50');
    geocodeAddress(geocoder, map, placeMarker);
  });

  function geocodeAddress(geocoder, resultsMap) {
    var address = addressInput.val();
    geocoder.geocode({'address': 'Казань,' + address}, function(results, status) {
     if (status === 'OK') {
      placeMarker(results[0].geometry.location);
      getMarkerPos();
      getMarkerAddress();
      setMarkerPosToInput();
      resultsMap.setCenter(results[0].geometry.location);
     } else {
      showAddressErrorMsg();
     }
    });
  };

  var infowindow = null;

  // Инициализация инфоблока для маркера
  function bindInfoWindow(marker, map, description) {

    if (infowindow) {
      infowindow.close();
      infowindow.setContent(description);
      infowindow.open(map, marker);
    } else {
      infowindow =  new google.maps.InfoWindow();
      infowindow.setContent(description);
      infowindow.open(map, marker);
    }

    marker.addListener('click', function() {
      infowindow.setContent(description);
      infowindow.open(map, marker);
    });

  };

  // Маркеры на странице мониторинга
  if ($('.map-container-big').length != 0) {

    var page = location.href.split('/')[3];

    var apiPath;

    switch(page) {
      case 'ContainersMonitoring':
        apiPath = '/api/monitorings/containers';
        break;
      case 'CarsMonitoring':
        apiPath = '/api/monitorings/cars';
        break;
    };

    var url = apiPath;

    $.ajax({
      type: 'GET',
      url: url,
      success: function (data) {
        placeMarker(data.Data, data.Data.length);
      }
    });
  };
};

// Подключение Google Map API
function loadGoogleMapApi() {
  var script = document.createElement("script");
  script.type = "text/javascript";
  script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyB4NT5_bn3ZPTrYW2Il5YLaBrMHGjzjcjM";
  document.body.appendChild(script);
};

window.onload = loadGoogleMapApi();
