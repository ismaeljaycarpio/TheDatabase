//$(function () {
//    $("input.textDate").datepicker({
//        showOn: "button",
//        buttonImage: "/images/calendar_icon.gif",
//        buttonImageOnly: true,
//        changeMonth: true,
//        changeYear: true,
//        dateFormat: 'dd/mm/yy'
//    });
//    
//});

function DoMasterSelect(masterCheckbox, containerID) {
    var checked = masterCheckbox.checked;
    $('#' + containerID + ' input[type="checkbox"]').attr('checked', checked ? 'checked' : '');
}

//function DoMasterSelectContainer(checked, containerID) {
    
//    $('#' + containerID + ' input[type="checkbox"]').attr('checked', checked ? 'checked' : '');
//}