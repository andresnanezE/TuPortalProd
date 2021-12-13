//function openMod(postbck, messa) {
//    var TeamDetailPostBackURL = '/Portal/Home/Emergente/';
//    var id = messa;
//    var options = { "backdrop": "static", keyboard: true };
//    $.ajax({
//        type: "GET",
//        url: TeamDetailPostBackURL,
//        contentType: "application/json; charset=utf-8",
//        data: { "title": "Cotizaciones vigentes", "mess": id },
//        datatype: "json",
//        success: function (data) {
//            $('#myModalContent').html(data);
//            $('#myModal').modal(options);
//            $('#myModal').modal('show');
//        },
//        error: function (ex) {
//            alert("Error.", ex);
//        }
//    });
//}