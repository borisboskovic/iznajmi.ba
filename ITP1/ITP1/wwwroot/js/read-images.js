function readURL(input) {
    if (input.files.length > 6) {
        alert("Ne mozete unijeti vise od 6 slika!");
    }
    for (var i = 0; i < 6; i++) {
        var id = "#img" + (i + 1) + "-cont";
        $(id)
            .attr('style', 'display: none');
    }
    var counter = 1;
    for (var i = 0; i < input.files.length; i++) {
        if (input.files && input.files[i]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var contId = "#img" + counter + "-cont";
                $(contId)
                    .attr('style', 'display: block');
                var id = "#img" + counter++;
                $(id)
                    .attr('src', e.target.result)
                    .attr('style', 'display: block')
                    .width(200)
                    .height(200);
            };

            reader.readAsDataURL(input.files[i]);
        }
    }
}