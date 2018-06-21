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