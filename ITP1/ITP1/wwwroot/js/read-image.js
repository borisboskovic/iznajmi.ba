function readURL(input) {
    if (input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#label-img")
                .attr('style', 'display: none');
            $("#new-img")
                .attr('src', e.target.result)
                .attr('style', 'display: block')
                .width(200)
                .height(200);
        };
        reader.readAsDataURL(input.files[0]);
        document.getElementById('add-img-form').submit();
    }
}