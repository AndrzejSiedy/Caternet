// Write your JavaScript code.


function updateAvailableSeatsDD(seats) {
    $("#Seats").html(""); // clear before appending new list
    $.each(seats, function (i, seat) {
        $("#Seats").append(
            $('<option></option>').val(seat.id).html(seat.seatNumber));
    });
}

function FillSeats() {
    var eventId = $('#Events').val();
    $.ajax({
        url: '/Events/GetSeats',
        type: "GET",
        dataType: "json",
        data: { eventId: eventId },
        success: function (data) {
            var seats = data.seats;
            var userSeats = data.userSeats;
            console.warn(data);
            $("#message").html(userSeats);
            updateAvailableSeatsDD(seats);
        }
    });
}


function SaveSeat() {
    var eventId = $('#Events').val();
    var seatId = $('#Seats').val();

    var email = $('#email').val();
    var attendeeName = $('#attendeeName').val();

    $.ajax({
        url: '/Events/SaveSeat',
        type: "POST",
        dataType: "json",
        data: { eventId: eventId, seatId: seatId, email: email, attendeeName: attendeeName },
        success: function (data) {
            console.warn(data);
            updateAvailableSeatsDD(data.seats);

            $("#message").html(data.userSeats);
            $("#seats-selected").html(data.bookedSeats);
        }
    });
}