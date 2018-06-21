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