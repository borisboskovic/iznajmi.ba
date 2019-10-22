window.addEventListener('load', function () {
    resetColor();
}, false);

function setRating(item) {
    var id = $(item).attr("id");
    if (id == 'rating1') {
        $('#ocjena').val(1);
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("fas").addClass("far");;
        $('#rating2').css('color', 'black');
        $('#rating3').removeClass("fas").addClass("far");;
        $('#rating3').css('color', 'black');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if (id == 'rating2') {
        $('#ocjena').val(2);
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("fas").addClass("far");;
        $('#rating3').css('color', 'black');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if (id == 'rating3') {
        $('#ocjena').val(3);
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if (id == 'rating4') {
        $('#ocjena').val(4);
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("far").addClass("fas");;
        $('#rating4').css('color', '#FFDF00');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if (id == 'rating5') {
        $('#ocjena').val(5);
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("far").addClass("fas");;
        $('#rating4').css('color', '#FFDF00');
        $('#rating5').removeClass("far").addClass("fas");;
        $('#rating5').css('color', '#FFDF00');
    }
}

function changeColor(item) {
    var id = $(item).attr("id");
    if (id == 'rating1') {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("fas").addClass("far");;
        $('#rating2').css('color', 'black');
        $('#rating3').removeClass("fas").addClass("far");;
        $('#rating3').css('color', 'black');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if (id == 'rating2') {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("fas").addClass("far");;
        $('#rating3').css('color', 'black');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if (id == 'rating3') {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if (id == 'rating4') {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("far").addClass("fas");;
        $('#rating4').css('color', '#FFDF00');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if (id == 'rating5') {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("far").addClass("fas");;
        $('#rating4').css('color', '#FFDF00');
        $('#rating5').removeClass("far").addClass("fas");;
        $('#rating5').css('color', '#FFDF00');
    }
}

function resetColor() {
    if ($('#ocjena').val() == 1) {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("fas").addClass("far");;
        $('#rating2').css('color', 'black');
        $('#rating3').removeClass("fas").addClass("far");;
        $('#rating3').css('color', 'black');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if ($('#ocjena').val() == 2) {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("fas").addClass("far");;
        $('#rating3').css('color', 'black');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if ($('#ocjena').val() == 3) {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if ($('#ocjena').val() == 4) {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("far").addClass("fas");;
        $('#rating4').css('color', '#FFDF00');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
    else if ($('#ocjena').val() == 5) {
        $('#rating1').removeClass("far").addClass("fas");;
        $('#rating1').css('color', '#FFDF00');
        $('#rating2').removeClass("far").addClass("fas");;
        $('#rating2').css('color', '#FFDF00');
        $('#rating3').removeClass("far").addClass("fas");;
        $('#rating3').css('color', '#FFDF00');
        $('#rating4').removeClass("far").addClass("fas");;
        $('#rating4').css('color', '#FFDF00');
        $('#rating5').removeClass("far").addClass("fas");;
        $('#rating5').css('color', '#FFDF00');
    }
    else {
        $('#rating1').removeClass("fas").addClass("far");;
        $('#rating1').css('color', 'black');
        $('#rating2').removeClass("fas").addClass("far");;
        $('#rating2').css('color', 'black');
        $('#rating3').removeClass("fas").addClass("far");;
        $('#rating3').css('color', 'black');
        $('#rating4').removeClass("fas").addClass("far");;
        $('#rating4').css('color', 'black');
        $('#rating5').removeClass("fas").addClass("far");;
        $('#rating5').css('color', 'black');
    }
}