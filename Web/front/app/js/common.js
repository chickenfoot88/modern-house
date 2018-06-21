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
