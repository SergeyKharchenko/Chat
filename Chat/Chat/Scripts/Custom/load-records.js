$(function () {
    window.setTimeout(loadRecords, 200);
    $('#enter-button').on('click', addNewMessage);
})

function loadRecords() {
    var hidden = $("#chat-id");
    var chatId = hidden.val();
    var action = hidden.data("action");
    var lastMessage = $("#messages-container div:last");
    var creationDate = lastMessage.data("creation-date");

    $.ajax({
        type: 'POST',
        data: { chatId: chatId, lastRecordsCreationDate: creationDate },
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
    window.setTimeout(loadRecords, 1000);
}

function addNewMessage() {
    var textArea = $('#new_message');
    var text = textArea.val();
    var action = textArea.data('action');

    $.ajax({
        type: 'POST',
        data: { chatId: $("#chat-id").val(), text: text },
        url: action,
        timeout: 2000,
        async: false,
        success: function() {
            textArea.val('');
        },
        error: null
    });
}