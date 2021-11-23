$("#createKeyword").click(function () {
    var input = $("#inputKeyword");
    var value = input.val().toLowerCase();
    if (input != null && value != "") {
        var list = $("#keywordList");
        var options = $("#keywordList option");
        var exists = false;
        for (var i = 0; i < options.length; i++) {
            if (options[i].value == value) {
                alert("Keyword already exists");
                exists = true;
                break;
            }
        }
        if (!exists) {
            var temp = new Array();
            for (var i = 0; i < options.length; i++) {
                temp[i] = options[i];
            }
            temp[options.length] = new Option(value);
            temp.sort(sortByName);
            list.empty();
            for (var i = 0; i < temp.length; i++) {
                list.append(temp[i]);
            }
        }
    } else {
        alert("Please enter a keyword value");
    }
    input.val("");
});

$("#deleteKeyword").click(function () {
    var selected = $("#keywordList option:selected");
    if (selected.length > 0) {
        selected.remove();
    } else {
        alert("No new keywords selected");
    }
});

$("#inputKeyword").keypress(function (e) {
    if (!(e.key).match("^[a-z0-9-\s]+$")) e.preventDefault();
});

function sortByName(a, b) {
    if (a.text < b.text) return -1;
    if (a.text > b.text) return 1;
    return 0;
};


