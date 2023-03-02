////$("#countryDropdown li").click(function () {
////    var countryId = $(this).val();
////    getCitiesByCountry(countryId);
////});



////function getCitiesByCountry(countryId) {
////    $.ajax({
////        type: "GET",
////        url: "/Mission/GetCitiesByCountry",
////        data: { countryId: countryId },
////        success: function (data) {
////            var dropdown = $("#cityDropdown");
////            dropdown.empty();
////            $.each(data, function (i, item) {
////                /* var li = dropdown.createElement("li");*/
////                dropdown.append($(`<li>
////                            <div class="dropdown-item mb-3 form-check">
////                            <input type="checkbox" class="form-check-input" id="exampleCheck1">
////                            <label class="form-check-label" for="exampleCheck1">Check me out</label>
////                            </div>
////                        </li>`)).val(item.id)
////                    .text(item.name)
////                //dropdown.append($(`<input type="checkbox" class="form-check-input" id="exampleCheck1">`)).val(item.id)
////                //    .text(item.name)
////                            /*<label class="form-check-label" for="exampleCheck1">Check me out</label>`*/
                    
   
////            });
////        }
////    });
////}

$("#countryDropdown li").click(function () {
    var countryId = $(this).val();
    getCitiesByCountry(countryId);
});

function getCitiesByCountry(countryId) {
    $.ajax({
        type: "GET",
        url: "/Mission/GetCitiesByCountry",
        data: { countryId: countryId },
        success: function (data) {
            var dropdown = $("#cityDropdown");
            dropdown.empty();
            $.each(data, function (i, item) {
                dropdown.append($('<option></option>').val(item.id).text(item.name));
            });
        }
    });
}
