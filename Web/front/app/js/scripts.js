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

var dateRangeFilter = function() {

  $.fn.dataTableExt.afnFiltering.push(

  	function( oSettings, aData, iDataIndex ) {

  		var date = $('#dateRangePicker')[0].value;

  		var iStartDateCol = 1;
  		var iEndDateCol = 1;

  		var startDate = date.substring(6,10) + date.substring(3,5) + date.substring(0,2);
  		var endDate =  date.substring(19,23) + date.substring(16,18) + date.substring(13,15);

      var tableDate =  aData[iStartDateCol].substring(6,10) + aData[iStartDateCol].substring(3,5) + aData[iStartDateCol].substring(0,2);

    	if ( startDate === "" && endDate === "" )
  		{
  			return true;
  		}
  		else if ( startDate <= tableDate && endDate === "")
  		{
  			return true;
  		}
  		else if ( startDate >= tableDate && endDate === "")
  		{
  			return true;
  		}
  		else if (startDate <= tableDate && endDate >= tableDate)
  		{
  			return true;
  		}
  		return false;
  });

};

setMonitoringMap();

  function setMonitoringMap() {
    if ($('.map-container-big').length != 0) {
      $('.map-container-big').css('min-height', $(document).height() - 152);
      setTimeout(initMap, 1000);
    };
  }

  var table = ""

  function initDataTable() {
    $.fn.dataTable.moment( 'DD.MM.YYYY HH:mm' );

    // Инициализаяи таблиц
    $('.data-table').dataTable().fnDestroy();

    table = $('.data-table').DataTable({
      "order": [[0, 'desc']],
      paging: false,
      "pageLength": 100,
      "oLanguage": {
        "sUrl": "//cdn.datatables.net/plug-ins/1.10.16/i18n/Russian.json"
      },

      initComplete: function afterInit() {
        var elem = $('.date-filter').find(".dataTables_wrapper").find('.row').first();
        elem.length == 0 ? false : appendDatePicker(elem);
      },
      "fnPreDrawCallback": showPreloader($('.box-body')),
      "fnInitComplete": hidePreloader(),

    });

  };

  function appendDatePicker(elem) {

    var datePicker = '<div class="col-sm-6"><div class="form-group form-group_no-label"><div class="input-group date"><div class="input-group-addon"><i class="fa fa-calendar"></i></div><input class="form-control pull-right dateRangePicker" id="dateRangePicker" type="text"></div></div></div>';
    elem.append(datePicker);
    initDateRangePicker();
    dateRangeFilter();
  };

  $(window).on('shown.bs.modal', function() {
    initICheck();
    initDatePicker();
    initInputMask();
  });

  // Инициализация плагина iCheck
  function initICheck() {
    $('.icheck').iCheck({
      checkboxClass: 'icheckbox_minimal',
      radioClass: 'icheckbox_minimal',
      increaseArea: '20%'
    });
  };

  // Инициализация плагина iCheck
  initICheck();

  // Инициализация Date & Time Picker
  function initDatePicker() {
    console.log('init datepicker');
    $('.datepicker').datetimepicker({
      language: 'ru',
      format: 'dd.mm.yyyy hh:ii'
    });
  };

  // Инициализация плагина inputmask
  function initInputMask() {
    $(".form-control-tel").inputmask("+7(999) 999-99-99");
    $(".form-control-inn").inputmask("999999999999");
    $(".form-control-time").inputmask("99:99");
  };

  //функция пребразования путей (нужна для добавления /appPath)
  function prepareUrl(relativeUrl) {
	  var baseUrl = $("#BaseUrl").attr("data-baseurl");
	  if(baseUrl[baseUrl.length - 1] == "/"){
		  baseUrl = baseUrl.substring(0,baseUrl.length - 1);
	  }

	  if (typeof(relativeUrl) == "string" && relativeUrl.length > 0) {
		  if (relativeUrl[0] === "/") {
			  return baseUrl + "/" + relativeUrl.substring(1);
		  } else {
			  return baseUrl + "/" + relativeUrl;
		  }
	  }
	  return relativeUrl;
  };

  // Подгрузке прелоадера
  function showPreloader(modal) {
    var preloader = '<div class="preloader"></div>';
    modal.append(preloader);
  };

  // Скрытие прелоадера
  function hidePreloader() {
    $('.preloader').delay(1000).fadeOut('300');
  };

  //получение наименования сущности по url-у
  //для отрисовки сообщений о успешной/не успешной операции
  function returnItemName(href) {

    var itemName = "";
    switch(href.split('/')[1]) {
      case 'Containers':
      itemName = 'Контейнер';
      break;
      case 'Customers':
      itemName = 'Заказчик';
      break;
      case 'Cars':
      case 'CarContainerTypes':
      itemName = 'Автомобиль';
      break;
      case 'Polygons':
      itemName = 'Полигон';
      break;
      case 'ContainerTypes':
      itemName = 'Тип контейнера';
      break;
      case 'Drivers':
      itemName = 'Водитель';
      break;
      case 'Requests':
      itemName = 'Заявка';
      break;
      case 'WayBills':
      itemName = 'Маршрут';
      break;
      case 'Schedules':
      itemName = 'График работ';
      break;
    };

    return itemName;

  }

  // Инициализация Date Range Picker
  function initDateRangePicker() {

    $('.dateRangePicker').daterangepicker({
      "locale": {
        "format": "DD.MM.YYYY",
        "separator": " - ",
        "applyLabel": "Применить",
        "cancelLabel": "Сбросить",
        "fromLabel": "От",
        "toLabel": "До",
        "customRangeLabel": "Произвольный промежуток",
        "weekLabel": "Н",
        "daysOfWeek": [
          "Вс",
          "Пн",
          "Вт",
          "Ср",
          "Чт",
          "Пт",
          "Сб"
        ],
        "monthNames": [
          "Январь",
          "Февраль",
          "Март",
          "Апрель",
          "Май",
          "Июнь",
          "Июль",
          "Август",
          "Сентябрь",
          "Октябрь",
          "Ноябрь",
          "Декабрь"
        ],
        "firstDay": 1,
      },
      autoUpdateInput: false,
      "opens": "center"
    });
  };

$(function() {
	
	if($('.data-table').length > 0)
	{
		var table = ""

		function initDataTable() {
			$.fn.dataTable.moment( 'DD.MM.YYYY HH:mm' );

			// Инициализаяи таблиц
			$('.data-table').dataTable().fnDestroy();

			table = $('.data-table').DataTable({
			  "order": [[0, 'desc']],
			  paging: false,
			  "pageLength": 100,
			  "oLanguage": {
				"sUrl": "//cdn.datatables.net/plug-ins/1.10.16/i18n/Russian.json"
			  },

			  initComplete: function afterInit() {
				var elem = $('.date-filter').find(".dataTables_wrapper").find('.row').first();
				elem.length == 0 ? false : appendDatePicker(elem);
			  },
			  "fnPreDrawCallback": showPreloader($('.box-body')),
			  "fnInitComplete": hidePreloader(),

			});

		};
	  
		function initDateRangePickerEvents(){
			$('#dateRangePicker').on('apply.daterangepicker', function(ev, picker) {
			  $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));
			  table.draw();
			});

			$('#dateRangePicker').on('cancel.daterangepicker', function(ev, picker) {
			  $(this).val('');
			  table.draw();
			});
		}

		function appendDatePicker(elem) {

			var datePicker = '<div class="col-sm-6"><div class="form-group form-group_no-label"><div class="input-group date"><div class="input-group-addon"><i class="fa fa-calendar"></i></div><input class="form-control pull-right dateRangePicker" id="dateRangePicker" type="text"></div></div></div>';
			elem.append(datePicker);

			//создаем сам элемент
			initDateRangePicker();

			//привязываем на события выбора и сброса - перезагрузку грида
			initDateRangePickerEvents();

			//привязываем плагин фильтрации грида (local store)
			dateRangeFilter();
		};

		runModal();

		// Вызов модальных окон для создания и редактирования
		function runModal() {
			var modalWindow = $('.modal-window');

			btnCreateItemTable = $('.create-button');
			btnEditItemTable = $('.edit-button');
			btnDeleteItemTable = $('.delete-button');
			btnCopyItemTable = $('.copy-button');

			$.ajaxSetup({ cache: false });

			var openWindow = function(e, href, id) {

			  e.preventDefault();

			  if (href.split('/')[1] == 'CarContainerTypes') {
				var carId = $('#carId').val();
				href = href + '?carId=' + carId;
			  }

			  $.get(href, function (data) {

				var modalWindowContent = $('.modal-dialog');

				modalWindowContent.html(data);

				showPreloader(modalWindowContent);

				modalWindow.on("hidden.bs.modal", function(){
				  modalWindowContent.empty();
				  modalWindow.modal('hide');
				});

				modalWindow.find(' form ').submit(function (e) {

				  e.preventDefault();

				  var form = modalWindow.find(' form ');

				  var type = !id
				  ? 'POST'
				  : 'PUT';

				  var url = !id
				  ? prepareUrl('/api/' + href.split('/')[1])
				  : prepareUrl('/api/' + href.split('/')[1] + '/' + id);

				  if (href.split('/')[1] == 'CarContainerTypes') {
					url = prepareUrl('/api/cars/' + carId + '/container-type');
				  } else if (href.split('/')[1] == 'WayBills') {
					url = prepareUrl('/api/WayBills/requests');
				  }

				  itemName = returnItemName(href);

				  $.ajax({
					type: type,
					url: url,
					data: form.serialize(),
					success: function () {
					  modalWindow.modal('show');
					  $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-green"><i class="fa fa-check" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">' + itemName + ' успешно сохранен</span></div></div>');
					  setTimeout(function(){location.reload()}, 1000);
					  table.draw();
					},
					error: function (resp) {
					  modalWindow.modal('show');
					  if (resp.responseJSON && resp.responseJSON.Error) {
						$('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-yellow"><i class="fa fa-exclamation" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">' + resp.responseJSON.Error + '</span></div></div>');
					  } else {
						$('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-red"><i class="fa fa-times" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">В результате сохранения возникла ошибка. Обратитесь к раработчику</span></div></div>');
					  }
					},
					failure: function() {
					  modalWindow.modal('show');
					  $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-yellow"><i class="fa fa-times" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">Не удалось сохранить' + itemName.toLowerCase() + '</span></div></div>');
					  setTimeout(function(){location.reload()}, 1000);
					}
				  });

				});

				modalWindow.modal('show');

			  });

			};

			modalWindow.on('shown.bs.modal', function () {

			  if ($('.map-container')[0]) {
				initMap();
			  };

			  hidePreloader();

			});

			btnCreateItemTable.click(function (e) {
			  openWindow(e, this.getAttribute('href'));
			});

			btnEditItemTable.click(function (e) {
			  openWindow(e, this.getAttribute('href'), this.getAttribute('elementId'));
			});

			btnCopyItemTable.click(function (e) {
			  openWindow(e, this.getAttribute('href'));
			});

			btnDeleteItemTable.click(function (e) {

			  var btn = this;

			  var href = btn.getAttribute('href');

			  url = 'api/' + href.split('/')[1] + '/' + href.split('/')[3];

			  if (href.split('/')[1] == 'CarContainerTypes') {
				url = prepareUrl('/api/cars/container-type/' + href.split('/')[3]);
			  } else if (href.split('/')[1] == 'WayBills') {
				elementId = btn.getAttribute('elementid');
				url = prepareUrl('/api/WayBills/requests/' + elementId);
			  };

			  itemName = returnItemName(href);

			  $.ajax({
				type: 'DELETE',
				url: url,
				success: function () {
				  var row = $($(btn).parents('tr')).closest('tr');
				  var nRow = row[0];
				  $('.data-table').dataTable().fnDeleteRow(row);
				  modalWindow.modal('show');
				  $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-green"><i class="fa fa-trash" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">' + itemName + ' успешно удален</span></div></div>');
				  setTimeout(function() { modalWindow.modal('hide'); }, 1000);
				},
				error: function() {
				  modalWindow.modal('show');
				  $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-red"><i class="fa fa-trash" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">При попытке удаления возникла ошибка. Обратитесь к раработчику</span></div></div>');
				  setTimeout(function() { modalWindow.modal('hide'); }, 1000);
				},
				failure: function() {
				  modalWindow.modal('show');
				  $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-yellow"><i class="fa fa-trash" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">Не удалось удалить ' + itemName.toLowerCase() + '.</span></div></div>');
				  setTimeout(function() { modalWindow.modal('hide'); }, 1000);
				}
			  });
			});
		}

		initDataTable();
	}
});
$(function () {

    if ($('.requests-table').length > 0) {
        var requestsDataTable = "";

        function initRequestsDataTable() {
            requestsDataTable = $('.requests-table')
                .DataTable({
                    "processing": true,
                    "serverSide": true,
                    "ordering": false,
                    "searching": false,
                    "lengthChange": false,                    
                    "pageLength": 50,
                    "ajax": {
                        "url": prepareUrl('/api/requests'),
                        "dataSrc": "Data",
                        "data": function (d) {
                            //remove unused on server side information
                            delete d.columns;
                            applyRequestsDataTableFilters(d);
                        }
                    },
                    "columnDefs": [
                        {
                            className: 'text-center',
                            targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                        },
                        {
                            className: 'edit-cell edit-cell-4',
                            targets: [10]
                        }
                    ],
                    "drawCallback": function ( settings ) {
						var api = this.api();
						var rows = api.rows( {page:'current'} ).nodes();
						var lastDate = null;
						
						var parseDate = function(dateStr){
							var parts = dateStr.split(' ');
							if(parts.length < 1){
								return null;
							}
							
							var parts = parts[0].split('.');
							if(parts.length != 3){
								return null;
							}
							
							var day = parseInt(parts[0]);
							var month = parseInt(parts[1]);
							var year = parseInt(parts[2]);
							
							var date = new Date(year, month - 1, day);
							
							if ( (date.getFullYear() != year) || (date.getMonth() != month - 1) || (date.getDate() != day) ){
								return null;
							}
							return date;	
						}
						
						api.column(0, {page:'current'} ).data().each( function ( group, i ) {
							var groupDate = parseDate(group);
							if ( !lastDate ||
								 (groupDate.getFullYear() != lastDate.getFullYear()) || 
								 (groupDate.getMonth() != lastDate.getMonth()) || 
								 (groupDate.getDate() != lastDate.getDate()) 
							) {
								var suffix = '';
								if(!!groupDate){
									var days = [
										'Воскресенье',
										'Понедельник',
										'Вторник',
										'Среда',
										'Четверг',
										'Пятница',
										'Суббота'
									];
									suffix = ' - ' + days[groupDate.getDay()];
								}
								$(rows).eq( i ).before(
									'<tr class="group" style="background-color: #ddd !important;"><td colspan="11">'+
										groupDate.getFullYear()+'.'+
										(groupDate.getMonth()+1).toString().padStart(2,0)+'.'+
										(groupDate.getDate()).toString().padStart(2,0)+
										suffix+'</td></tr>'
								);
								lastDate = groupDate;
							}
						} );
					},
                    "oLanguage": {
                        "sUrl": "//cdn.datatables.net/plug-ins/1.10.16/i18n/Russian.json"
                    },
                    "fnPreDrawCallback": showPreloader($('.box-body')),
                    "fnInitComplete": hidePreloader(),
                    "columns": [
                        { "data": "PlannedDate" },
                        { "data": "Address" },
                        { "data": "ContactPersonPhone" },
                        {
                            "data": "IsPaid",
                            "render": function (data, type, row, meta) {
                                var className = data
                                    ? 'fa-check'
                                    : 'fa-times';

                                return '<i class="fa ' + className + '" aria-hidden="true"></i>';
                            }
                        },
                        { "data": "StatusName",
                            "render": function (data, type, row, meta) {
                                var color='white';
                                switch(data)
                                {
                                    case 'Новая':
                                    color = '#80ff00';
                                    break;
                                    case 'В работе':
                                    color = '#ffff00';
                                    break;
                                }

                                if(new Date(row.PlannedDate.substring(6,10) + '.' + row.PlannedDate.substring(3,5) + '.' + row.PlannedDate.substring(0,2) + row.PlannedDate.substring(10,16)).getTime() < new Date() && data =='Новая')
                                color = '#fe0229';

                                return '<div style="background-color: '+color+' !important;">'+data+'</div>'
                            }
                        },
                        { "data": "TypeName" },
                        {
                            "data": "Container",
                            "render": function (data, type, row, meta) {
                                if (!data) {
                                    return '<i class="fa fa-exclamation-triangle" aria-hidden="true" style="color:red;"></i>'
                                }
                                else {
                                    return data.Type;
                                }
                            }
                        },
                        {
                            "data": "Customer",
                            "render": function (data) {
                                if (!!data) {
                                    return data.Name;
                                }
                                else {
                                    return "Не заполнен";
                                }
                            }
                        },
                        {
                            "data": "Car",
                            "orderable": false,
                            "render": function (data) {
                                if (!data) {
                                    return '<i class="fa fa-exclamation-triangle" aria-hidden="true" style="color:red;"></i>'
                                }
                                else {
                                    return data.Number;
                                }
                            }
                        },
                        {
                            "data": "Driver",
                            "orderable": false,
                            "render": function (data) {
                                if (!data) {
                                    return '<i class="fa fa-exclamation-triangle" aria-hidden="true" style="color:red;"></i>'
                                }
                                else {
                                    return data.Name;
                                }
                            }
                        },
                        {
                            "data": "Id",
                            "orderable": false,
                            "render": function (data, type, row, meta) {
                                var btns =
                                    '<button href="' + prepareUrl("/Requests/Edit?id=" + data) + '" elementId="' + data + '" class="btn btn-warning edit-button">' +
                                    '<span class="glyphicon glyphicon-pencil" aria-hidden="true"></span>' +
                                    '</button>' +
                                    '<!--<button href="' + prepareUrl("/Requests/Delete/" + data) + '" elementId="' + data + '" class="btn btn-danger delete-button">' +
                                    '<span class="glyphicon glyphicon-trash" aria-hidden="true"></span>' +
                                    '</button>-->' +
                                    '<button href="' + prepareUrl("/Requests/Copy?id=" + data) + '" class="btn btn-primary copy-button" >' +
                                    '<span class="glyphicon glyphicon-copy" aria-hidden="true"></span>' +
                                    '</button >';
                                return btns;
                            }
                        }
                    ]
                });

			requestsDataTable.on('draw', function(){
				initRequestsDataTableButtons()
			});
            initRequestsDataTableFilters();
        }

        function initRequestsDataTableFilters() {
            initDateRangePicker();
			initDateRangePickerEvents();
			
			var applyFilterButton = $('.applyFilterButton');
			applyFilterButton.click(function (e) {
				e.preventDefault();
                requestsDataTable.draw();
            });
        }
		
		function initDateRangePickerEvents(){
			$('#dateRangePicker').on('apply.daterangepicker', function(ev, picker) {
			  $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));
			});

			$('#dateRangePicker').on('cancel.daterangepicker', function(ev, picker) {
			  $(this).val('');
			});
		}

        function initRequestsDataTableButtons() {
            var modalWindow = $('.modal-window');

            btnCreateItemTable = $('.create-button');
            btnEditItemTable = $('.edit-button');
            btnDeleteItemTable = $('.delete-button');
            btnCopyItemTable = $('.copy-button');

            $.ajaxSetup({ cache: false });

            var openWindow = function (e, href, id) {

                e.preventDefault();

                $.get(href, function (data) {

                    var modalWindowContent = $('.modal-dialog');

                    modalWindowContent.html(data);

                    showPreloader(modalWindowContent);

                    modalWindow.on("hidden.bs.modal", function () {
                        modalWindowContent.empty();
                        modalWindow.modal('hide');
                    });

                    modalWindow.find(' form ').submit(function (e) {

                        e.preventDefault();

                        var form = modalWindow.find(' form ');

                        var type = !id
                            ? 'POST'
                            : 'PUT';

                        var url = !id
                            ? prepareUrl('/api/' + href.split('/')[1])
                            : prepareUrl('/api/' + href.split('/')[1] + '/' + id);

                        itemName = returnItemName(href);

                        $.ajax({
                            type: type,
                            url: url,
                            data: form.serialize(),
                            success: function () {
                                modalWindow.modal('show');
                                $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-green"><i class="fa fa-check" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">' + itemName + ' успешно сохранен</span></div></div>');
                                setTimeout(function () { location.reload() }, 1000);
                                requestsDataTable.draw();
                            },
                            error: function (resp) {
                                modalWindow.modal('show');
                                if (resp.responseJSON && resp.responseJSON.Error) {
                                    $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-yellow"><i class="fa fa-exclamation" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">' + resp.responseJSON.Error + '</span></div></div>');
                                } else {
                                    $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-red"><i class="fa fa-times" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">В результате сохранения возникла ошибка. Обратитесь к раработчику</span></div></div>');
                                }
                            },
                            failure: function () {
                                modalWindow.modal('show');
                                $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-yellow"><i class="fa fa-times" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">Не удалось сохранить' + itemName.toLowerCase() + '</span></div></div>');
                                setTimeout(function () { location.reload() }, 1000);
                            }
                        });

                    });

                    modalWindow.modal('show');

                });

            };

            modalWindow.on('shown.bs.modal', function () {

                if ($('.map-container')[0]) {
                    initMap();
                };

                hidePreloader();

            });

            btnCreateItemTable.click(function (e) {
                openWindow(e, this.getAttribute('href'));
            });

            btnEditItemTable.click(function (e) {
                openWindow(e, this.getAttribute('href'), this.getAttribute('elementId'));
            });

            btnCopyItemTable.click(function (e) {
                openWindow(e, this.getAttribute('href'));
            });

            btnDeleteItemTable.click(function (e) {

                var btn = this;

                var href = btn.getAttribute('href');

                url = 'api/' + href.split('/')[1] + '/' + href.split('/')[3];

                itemName = returnItemName(href);

                $.ajax({
                    type: 'DELETE',
                    url: url,
                    success: function () {
                        requestsDataTable.draw();
                        modalWindow.modal('show');
                        $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-green"><i class="fa fa-trash" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">' + itemName + ' успешно удален</span></div></div>');
                        setTimeout(function () { modalWindow.modal('hide'); }, 1000);
                    },
                    error: function () {
                        modalWindow.modal('show');
                        $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-red"><i class="fa fa-trash" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">При попытке удаления возникла ошибка. Обратитесь к раработчику</span></div></div>');
                        setTimeout(function () { modalWindow.modal('hide'); }, 1000);
                    },
                    failure: function () {
                        modalWindow.modal('show');
                        $('#dialogContent').html('<div class="info-box"><span class="info-box-icon bg-yellow"><i class="fa fa-trash" aria-hidden="true"></i></span><div class="info-box-content"><span class="info-box-text"></span><span class="info-box-number">Не удалось удалить ' + itemName.toLowerCase() + '.</span></div></div>');
                        setTimeout(function () { modalWindow.modal('hide'); }, 1000);
                    }
                });
            });
        }

        function applyRequestsDataTableFilters(params) {
			
			params.Filter = {};
			var datesStr = $('#dateRangePicker')[0].value;
			if(!!datesStr && datesStr.length >= 23){
				params.Filter.DateStart = datesStr.substring(6,10) + '.' + datesStr.substring(3,5) + '.' + datesStr.substring(0,2);
				params.Filter.DateEnd =  datesStr.substring(19,23) + '.' + datesStr.substring(16,18) + '.' + datesStr.substring(13,15);
			}
			
			var containerTypeId = $('#containerTypeFilter')[0].value;
			if(containerTypeId != '0'){
				params.Filter.ContainerTypeId = containerTypeId;
			}
			
			var carId = $('#carFilter')[0].value;
			if(carId != '0'){
				params.Filter.CarId = carId;
			}
			
			var requestType = $('#requestTypeFilter')[0].value;
			if(requestType != '0'){
				params.Filter.RequestType = requestType;
			}
			
			var driverId = $('#driverFilter')[0].value;
			if(driverId != '0'){
				params.Filter.DriverId = driverId;
			}
			
			var customerFilter = $('#customerFilter')[0].value;
			if(!!customerFilter){
				params.Filter.CustomerFilter = customerFilter;
			}
			
			var addressFilter = $('#addressFilter')[0].value;
			if(!!addressFilter){
				params.Filter.AddressFilter = addressFilter;
            }

			var contactPersonPhoneFilter = $('#contactPersonPhoneFilter')[0].value;
			if(!!contactPersonPhoneFilter){
				params.Filter.ContactPersonPhoneFilter = contactPersonPhoneFilter;
            }

			var requestStatus = $('#requestStatusFilter')[0].value;
			if(requestStatus != '0'){
				params.Filter.RequestStatus = requestStatus;
            }
            
			var isPaids = $('#isPaidFilter')[0].value;
			if(isPaids != '0'){
				params.Filter.IsPaids = isPaids;
            }
            
			var requestId = $('#requestIdFilter')[0].value;
			if(requestId != '0'){
				params.Filter.RequestId = requestId;
            }
        }
    
		initRequestsDataTable();
	}
});