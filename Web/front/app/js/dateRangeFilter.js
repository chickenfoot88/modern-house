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
