function changeActiveCheckBox(radioBtn) {
    var icons = document.getElementsByName(radioBtn.classList[0])[0];
    if (radioBtn.checked == true) {
        icons.classList.remove('fa-square');
        icons.classList.add('fa-check-square');
    }
    else {
        icons.classList.remove('fa-check-square');
        icons.classList.add('fa-square');
    }
}

