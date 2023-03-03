

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
            var items = "";
            $(data).each(function (i, item) {
                //items += "<option value=" + this.value + ">" + this.text + "</option>"
                items += `<li><input type="checkbox" class="form-check-input me-2" id="exampleCheck1" value=` + item.cityId + `><label class="form-check-label" for="exampleCheck1" >` + item.name + `</label></li>`
            })
            dropdown.html(items);


            var dropdown = $("#cityDropdownOffCanvas");
            dropdown.empty();
            var items = "";
            $(data).each(function (i, item) {
                //items += "<option value=" + this.value + ">" + this.text + "</option>"
                items += `<li><input type="checkbox" class="form-check-input me-2" id="exampleCheck1"  value=` + item.cityId + `><label class="form-check-label" for="exampleCheck1">` + item.name + `</label></li>`
            })
            dropdown.html(items);
        }
    });
}