function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#myImg')
                .attr('src', e.target.result)
                .width(200)
                .height(200);
        };
        reader.readAsDataURL(input.files[0]);
    }
}


var nameInput = document.getElementById('name-input');
nameInput.oninvalid = function (e) { e.target.setCustomValidity('Ovo polje je obavezno.'); };
nameInput.oninput = function (e) { e.target.setCustomValidity(''); };
