$(function () {
    $('#enter-button').on('click', addNewMessage);
    window.setTimeout(loadRecords, 200);
});

function loadRecords() {
    
    var hidden = $("#room-id");
    var roomId = hidden.val();
    var action = hidden.data("action");
    var lastMessage = $("#messages-container div:last");
    var creationDate = lastMessage.data("creation-date");

    $.ajax({
        type: 'POST',
        data: { roomId: roomId, lastRecordsCreationDate: creationDate },
        url: action,
        timeout: 2000,
        async: true,
        success: refreshRecords,
        error: null
    });
}

function refreshRecords(data) {
    
    var pattern = $('#record-pattern').html();
    
    for (var i = 0; i < data.length; i++) {
        var text = data[i].Text;
        var date = data[i].CreationDate;
        var record = pattern.replace('{creation-date}', date);
        record = record.replace('{text}', text);
        $("#messages-container").append(record);
    }
    window.setTimeout(loadRecords, 200);
}

function addNewMessage() {    
    var textArea = $('#new-message');
    var text = textArea.val();
    var action = textArea.data('action');

    $.ajax({
        type: 'POST',
        data: { roomId: $("#room-id").val(), text: text },
        url: action,
        timeout: 2000,
        async: false,
        success: function() {
            textArea.val('');
        },
        error: null
    });
}
