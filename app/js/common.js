$(function() {
  $('.datepicker').datepicker({
    language: 'ru'
  });

  $('.icheck').iCheck({
    radioClass: 'icheckbox_minimal',
    checkboxClass: 'icheckbox_minimal',
    increaseArea: '20%'
  });

  $(".form-control-tel").inputmask("+7(999) 999-99-99");
  $(".form-control-inn").inputmask("999999999999");
  $(".form-control-time").inputmask("99:99");

});
