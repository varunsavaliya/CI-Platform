﻿

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
                console.log("hi")
                //items += "<option value=" + this.value + ">" + this.text + "</option>"
                items += `<li class="form-check ps-4"><input type="checkbox" class="form-check-input me-2" name="checkboxes[]" id=` + item.cityId + ` value=` + item.cityId + ` multiple><label class="form-check-label" for=` + item.cityId + `>` + item.name + `</label></li>`
            })
            dropdown.html(items);


            var dropdown = $("#cityDropdownOffCanvas");
            dropdown.empty();
            var items = "";
            $(data).each(function (i, item) {
                //items += "<option value=" + this.value + ">" + this.text + "</option>"
                items += `<li class="form-check ps-4"><input type="checkbox" class="form-check-input me-2" name="checkboxes[]" id=` + item.cityId + ` value=` + item.cityId + ` multiple><label class="form-check-label" for=` + item.cityId + `>` + item.name + `</label></li>`
            })
            dropdown.html(items);
        }
    });
    $('#cityDropdown').on('click', function (e) {
        e.stopPropagation();
    });
    $('#cityDropdownOffCanvas').on('click', function (e) {
        e.stopPropagation();
    });
    $('#themeDropdown').on('click', function (e) {
        e.stopPropagation();
    });
    $('#skillDropdown').on('click', function (e) {
        e.stopPropagation();
    });
}