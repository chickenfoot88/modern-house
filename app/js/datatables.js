 $(function() {
    
    $('.data-table').DataTable({

      "oLanguage": {
        "sUrl": "//cdn.datatables.net/plug-ins/1.10.16/i18n/Russian.json"
      },

      aoColumnDefs: [
        { bSortable: false, aTargets: [ -1 ] },

      ]

    });

 });
