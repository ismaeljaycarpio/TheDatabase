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
function HidedivNotificationCommonMessage() {
    $('#divNotificationMessage').fadeOut();
    var divNotificationMessage = document.getElementById('divNotificationMessage');
    var lblNotificationMessage = document.getElementById('ctl00_lblNotificationMessage');
    if (lblNotificationMessage != null)
    {
        $('#ctl00_lblNotificationMessage').html('');
    }
    else
    {
        lblNotificationMessage = document.getElementById('lblNotificationMessage');
        if (lblNotificationMessage != null)
            $('#ctl00_lblNotificationMessage').html('');
    }
      
    if (divNotificationMessage != null)
        divNotificationMessage.style.display = 'none';
}

function CommonAlertMessage(msg,td)
{
    try
    {
        var divNotificationMessage = document.getElementById('divNotificationMessage');        var lblNotificationMessage = document.getElementById('ctl00_lblNotificationMessage');        if (lblNotificationMessage == null)
            lblNotificationMessage = document.getElementById('lblNotificationMessage');

        if (lblNotificationMessage != null && divNotificationMessage!=null)
        {
            $('#divNotificationMessage').fadeIn();
            $(lblNotificationMessage).html('');
            $(lblNotificationMessage).text(msg);
            $(lblNotificationMessage).append('&nbsp; <a id="aNotificationMessageCloseC" href="#" style="text-decoration:none;" >Close</a>');
            $("#aNotificationMessageCloseC").bind("click", function () {
                HidedivNotificationCommonMessage()
            });

            window.setTimeout(HidedivNotificationCommonMessage, td);
        }
        else
        {
            alert(msg);
        }
    }
    catch(err)
    {
        //
    }
   

}
//function DoMasterSelectContainer(checked, containerID) {
    
//    $('#' + containerID + ' input[type="checkbox"]').attr('checked', checked ? 'checked' : '');
//}