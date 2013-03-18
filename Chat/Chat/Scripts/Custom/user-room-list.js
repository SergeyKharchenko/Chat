$(function() {
    $('#load-rooms').click(function() {
        var container = $('#rooms-table');
        if (container.is(":visible")) {
            container.toggle();
            return false;
        }
    });
});

function refreshRooms(data) {
    var container = $('#user-room-list');
    container.html('');
    
    var pattern = $('#room-pattern');
    for (var i = 0; i < data.length; i++) {
        var id = data[i].Id;
        var title = data[i].Title;
        var isCreator = data[i].IsCreator;
        
        if (isCreator) {
            pattern.find('img').show();
        } else {
            pattern.find('img').hide();
        }

        var patternHtml = pattern.html();
        var record = patternHtml.replace('~room-id~', id);
        record = record.replace('~title~', title);
        container.append(record);
    }
    $('#rooms-table').toggle();
}