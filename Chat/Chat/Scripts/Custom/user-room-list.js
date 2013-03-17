$(function() {
    $('#load-rooms').click(function() {
        var container = $('#user-room-list');
        if (container.is(":visible")) {
            container.toggle();
            return false;
        }
    });
});

function refreshRooms(data) {
    var pattern = $('#room-pattern').html();
    var container = $('#user-room-list');
    container.html('');

    for (var i = 0; i < data.length; i++) {
        var id = data[i].Id;
        var title = data[i].Title;
        var record = pattern.replace('~room-id~', id);
        record = record.replace('~title~', title);
        container.append(record);
    }
    container.toggle();
}