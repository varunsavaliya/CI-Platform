// search mission functionality

$(document).ready(function () {
    let cards = $(".card");
    let noMissionFounud = $(".no-mission-found");
    $(".search-field input").keyup(function () {
        let searchText = $(".search-field input").val().toLowerCase();
        let cardsVisible = false;
        cards.each(function () {

            let cardTitle = $(this).find(".card-title").text().toLowerCase();
            if (cardTitle.includes(searchText)) {
                $(this).parent().show();
                cardsVisible = true;
            } else {
                $(this).parent().hide();
            }
        });

        console.log(cardsVisible)
        if (!cardsVisible) {
            $(".no-mission-found").show();
        } else {
            $(".no-mission-found").hide();
        }
    });
});

// cities according to country

$("#countryDropdown li").click(function () {
    var countryId = $(this).val();
    getCitiesByCountry(countryId);

    var selectedCountry = $(this).find('a').text();
    $('.card').each(function () {
        var cardCountry = $(this).find('.mission-country').text();
        //var cardskills = $(this).find('.mission-theme h3').text();
        //console.log(cardTheme);

        //var isMatch = selectedThemes.some(function (selectedTheme) {
        //    //console.log(selectedTheme.text());
        //    return cardTheme === selectedTheme.text();
        //});

        if (selectedCountry.toLowerCase() == cardCountry.toLowerCase()) {
            $(this).parent().show();
        } else {
            $(this).parent().hide();
        }
    });
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
                items += `<li class="form-check ps-4"><input type="checkbox" class="form-check-input me-2" name="city" id=` + item.cityId + ` value=` + item.cityId + ` multiple><label class="form-check-label" for=` + item.cityId + `>` + item.name + `</label></li>`
            })
            dropdown.html(items);


            var dropdown = $("#cityDropdownOffCanvas");
            dropdown.empty();
            var items = "";
            $(data).each(function (i, item) {
                //items += "<option value=" + this.value + ">" + this.text + "</option>"
                items += `<li class="form-check ps-4"><input type="checkbox" class="form-check-input me-2" name="city" id=` + item.cityId + ` value=` + item.cityId + ` multiple><label class="form-check-label" for=` + item.cityId + `>` + item.name + `</label></li>`
            })
            dropdown.html(items);
        }
    });
    //$('#cityDropdown').on('click', function (e) {
    //    e.stopPropagation();
    //});
    //$('#cityDropdownOffCanvas').on('click', function (e) {
    //    e.stopPropagation();
    //});
    //$('#themeDropdown').on('click', function (e) {
    //    e.stopPropagation();
    //});
    //$('#skillDropdown').on('click', function (e) {
    //    e.stopPropagation();
    //});
}


// functionality : when user check any filter then it will add as pill after search bar

let cityDropdown = $("#cityDropdown");
let searchedFilters = $(".searched-filters");
let allDropdowns = $('.dropdown ul');
allDropdowns.each(function () {
    let dropdown = $(this);
    $(this).on('change', 'input[type="checkbox"]', function () {
        // if the check box is checked then and then  only we have to add it to pill
        if ($(this).is(':checked')) {
            let selectedOptionText = $(this).next('label').text();
            let selectedOptionValue = $(this).val();
            const closeAllButton = searchedFilters.children('.closeAll');

            // creating a new pill
            let pill = $('<div></div>').addClass('pill me-1');

            // adding the text to pill
            let pillText = $('<span></span>').text(selectedOptionText);
            pill.append(pillText);

            // add the close icon (bootstrap)
            let closeIcon = $('<span></span>').addClass('close').html('x');
            pill.append(closeIcon);


            // for closing the pill when clicking on close icon
            closeIcon.click(function () {
                const pillToRemove = $(this).closest('.pill');
                pillToRemove.remove();
                // Uncheck the corresponding checkbox
                const checkboxElement = dropdown.find(`input[type="checkbox"][value="${selectedOptionValue}"]`);
                checkboxElement.prop('checked', false);
                if (searchedFilters.children('.pill').length === 1) {
                    searchedFilters.children('.closeAll').remove();
                }
            });

            // Add "Close All" button
            if (closeAllButton.length === 0) {
                searchedFilters.append('<div class="pill closeAll">Close All</div>');
                searchedFilters.children('.closeAll').click(function () {
                    allDropdowns.find('input[type="checkbox"]').prop('checked', false);
                    searchedFilters.empty();
                });

                //add the pill before the close icon
                searchedFilters.prepend(pill);

            }
            else {
                searchedFilters.children('.closeAll').before(pill);
            }

        }
        // if the checkbox is not checked then we have to check for its value if it is exists in the pills section then we have to remove it
        else {
            let selectedOptionText = $(this).next('label').text() + 'x';
            let selectedOptionValue = $(this).val();
            $('.pill').each(function () {
                const pillText = $(this).text();
                if (pillText === selectedOptionText) {
                    $(this).remove();
                }
            });
            if ($('.pill').length === 1) {
                $('.closeAll').remove();
            }
        }
    });
})

// for all dropdown

$('.dropdown #skillDropdown').on('change', 'input[type="checkbox"]', function () {
    //console.log("on chnging the dropdown")
    var selectedSkills = $('input[type="checkbox"]:checked').map(function () {
        return $(this).next('label');
    }).get();
    //console.log(selectedSkills)

    //var selectedSkills = $('input[name="skillCheckboxes[]"]:checked').map(function () {
    //    return $(this).next('label');
    //}).get();


    if (selectedSkills.length === 0) {
        // If no themes are selected, show all cards
        console.log("no themes and skills selected")
        $('.card').parent().show();
    } else {
        // Otherwise, loop over all cards and compare themes and cities
        $('.card').each(function () {
            //var cardSkills = $(this).find('mission-skills').text();
            var cardSkills = $(this).find('.mission-skills').map(function () {
                return $(this).text();
            }).get();

            var isMatch = selectedSkills.some(function (selectedSkill) {
                return cardSkills.some(function (cardSkill) {
                    return cardSkill === selectedSkill.text();
                })
            })

            if (isMatch) {
                $(this).parent().show();
            } else {
                $(this).parent().hide();
            }
        });
    }
})



// pending from here

$('.dropdown #themeDropdown').on('change', 'input[type="checkbox"]', function () {
    //console.log("on chnging the dropdown")
    var selectedThemes = $('input[type="checkbox"]:checked').map(function () {
        return $(this).next('label');
    }).get();
    //var selectedSkills = $('input[name="skillCheckboxes[]"]:checked').map(function () {
    //    return $(this).next('label');
    //}).get();


    if (selectedThemes.length === 0) {
        // If no themes are selected, show all cards
        console.log("no themes and skills selected")
        $('.card').parent().show();
    } else {
        // Otherwise, loop over all cards and compare themes and cities
        $('.card').each(function () {
            var cardTheme = $(this).find('.mission-theme h3').text();


            //var cardskills = $(this).find('.mission-theme h3').text();
            //console.log(cardTheme);

            var isMatch = selectedThemes.some(function (selectedTheme) {
                //console.log(selectedTheme.text());
                //console.log(cardTheme === selectedTheme.text())
                return cardTheme === selectedTheme.text();
            })
            //    || (selectedCities.some(function (selectedCity) {
            //    console.log(cardCity)
            //    console.log(selectedCity)
            //    console.log(cardCity === selectedCity)
            //    return cardCity === selectedCity.text();
            //}))

            //console.log(isMatch)

            if (isMatch) {
                $(this).parent().show();
            } else {
                $(this).parent().hide();
            }
        });
    }
})


$('.dropdown #cityDropdown').on('change', 'input[type="checkbox"]', function () {
    //console.log("on chnging the dropdown")

    var selectedCities = $('input[type="checkbox"]:checked').map(function () {
        return $(this).next('label');
    }).get();

    if (selectedCities.length === 0) {
        // If no themes are selected, show all cards
        $('.card').parent().show();
    } else {
        console.log(selectedCities);
        $('.card').each(function () {
            var cardCity = $(this).find('.location').text();

            var isMatch = selectedCities.some(function (selectedCity) {
                return cardCity.trim().toLowerCase() == selectedCity.text().trim().toLowerCase();
            })

            if (isMatch) {
                $(this).parent().show();
            } else {
                $(this).parent().hide();
            }
        });
    }
})

//$('.dropdown #skillDropdown').on('change', 'input[type="checkbox"]', function () {
//    //console.log("on chnging the dropdown")
//    var selectedSkills = $('input[name="skillCheckboxes[]"]:checked').map(function () {
//        return $(this).next('label');
//    }).get();
//    console.log(selectedSkills)


//    if (selectedSkills.length === 0) {
//        // If no themes are selected, show all cards
//        console.log("no themes and skills selected")
//        $('.card').parent().show();
//    } else {
//        // Otherwise, loop over all cards and compare themes and cities
//        $('.card').each(function () {
//            var cardSkills = $(this).find('.mission-theme h3').text();


//            //var cardskills = $(this).find('.mission-theme h3').text();
//            //console.log(cardTheme);

//            var isMatch = selectedSkills.some(function (selectedTheme) {
//                //console.log(selectedTheme.text());
//                //console.log(cardTheme === selectedTheme.text())
//                return cardTheme === selectedTselectedSkillsheme.text();
//            })
//            //    || (selectedCities.some(function (selectedCity) {
//            //    console.log(cardCity)
//            //    console.log(selectedCity)
//            //    console.log(cardCity === selectedCity)
//            //    return cardCity === selectedCity.text();
//            //}))

//            //console.log(isMatch)

//            if (isMatch) {
//                $(this).parent().show();
//            } else {
//                $(this).parent().hide();
//            }
//        });
//    }
//})


//if ($(this).is(':checked')) {
//    let selectedOptionText = $(this).next('label').text().trim().toLowerCase();
//    let missionTheme = $('.card .mission-theme h3');
//    missionTheme.each(function () {
//        //console.log($(this).text().trim().toLowerCase())
//        //const cardTheme = $(this).find('.mission-theme h3').text().trim();
//        //console.log($(this).text().trim().toLowerCase() != selectedOptionText)
//        if ($(this).text().trim().toLowerCase() != selectedOptionText) {
//            $(this).parent().parent().parent().hide();
//            console.log($(this).parent().parent().parent().parent())
//        }
//        else {
//            //$('.card').parent().show();
//            $(this).parent().parent().parent().parent().show();
//            //console.log(selectedOptionText)

//        }
//    })
//    //$('.card').each(function () {
//    //    const cardTheme = $(this).find('.mission-theme h3').text().trim();
//    //    if (cardTheme.toLowerCase() != selectedOptionText) {
//    //        $(this).parent().hide();
//    //    }
//    //})
//}
//else {
//    let selectedOptionText = $(this).next('label').text().trim().toLowerCase();
//    let missionTheme = $('.card .mission-theme h3');
//    $('.card').each(function () {
//        const cardTheme = $(this).find('.mission-theme h3').text().trim();
//        if (cardTheme.toLowerCase() != selectedOptionText) {
//            console.log("hi")
//            $(this).parent().show();
//        }
//    })
//}

//// Close button functionality for every pills
//let closeButtons = $('.pill .close');
//let closeAllButton = $('.closeAll');
//let length = closeButtons.length;
//closeButtons.each(function () {
//    $(this).click(function () {
//        $(this).parent().remove();
//        length--;
//        console.log(closeButtons.length);
//        if (length == 0) {
//            closeAllButton.remove();
//        }
//    });
//});
//// Close button functionality for close all text
//closeAllButton.click(function () {
//    closeButtons.each(function () {
//        $(this).parent().remove();
//    });
//    $(this).remove();
//})