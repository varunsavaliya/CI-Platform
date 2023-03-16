


///////////////////////////////////////////////////////////////////////////////////////////
//function paginate(currentPage, itemsPerPage, totalItems) {
//    // Calculate the starting and ending indices for the current page
//    var startIndex = (currentPage - 1) * itemsPerPage;
//    var endIndex = Math.min(startIndex + itemsPerPage, totalItems);

//    // Get the array of items to be displayed on the current page
//    var currentItems = allCards.slice(startIndex, endIndex);

//    // Update the HTML for the current items
//    $('.card').each(function (index) {
//        if (index >= startIndex && index < endIndex) {
//            // Show the card if it's part of the current page
//            $(this).parent().show();
//            console.log("hi")
//        } else {
//            // Hide the card if it's not part of the current page
//            $(this).parent().hide();
//        }
//    });

//    // Update the pagination links to reflect the current page
//    $('#pagination-container .pagination-link').removeClass('active');
//    $('#pagination-container .pagination-link[data-page="' + currentPage + '"]').addClass('active');
//}
//$(document).on('click', '.pagination-link', function (e) {
//    e.preventDefault();
//    var currentPage = parseInt($(this).attr('data-page'));
//    paginate(currentPage, itemsPerPage, totalCards);
//});

//let allCards = $('.card:visible');
//let itemsPerPage = 3;
//let totalCards = allCards.length;
//console.log(totalCards)
//paginate(1, itemsPerPage, totalCards);

//
////////////////////////////////////////////////////////////////////////////


// search mission functionality
$(document).ready(function () {
    // pages for pagination

    let selectedCountry = null;
    let selectedSortOption = null;
    getMission()
    //let noMissionFounud = $(".no-mission-found");
    $(".search-field input").keyup(function () {
        let searchText = $(this).val().toLowerCase();
        getMission()
        $(".card").each(function () {

            let cardTitle = $(this).find(".card-title").text().toLowerCase();
            if (cardTitle.includes(searchText)) {
                $(this).parent().show();
            } else {
                $(this).parent().hide();
            }
            if ($('.card:visible').length == 0) {
                $(".no-mission-found").show();
            } else {
                $(".no-mission-found").hide();
            }
        });

    });


    // cities according to country

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
                    items += `<li class="form-check ps-4"><input type="checkbox" class="form-check-input me-2" name="cityCheckboxes" id=` + item.cityId + ` value=` + item.cityId + ` multiple><label class="form-check-label" for=` + item.cityId + `>` + item.name + `</label></li>`
                })
                dropdown.html(items);


                var dropdown = $("#cityDropdownOffCanvas");
                dropdown.empty();
                var items = "";
                $(data).each(function (i, item) {
                    //items += "<option value=" + this.value + ">" + this.text + "</option>"
                    items += `<li class="form-check ps-4"><input type="checkbox" class="form-check-input me-2" name="cityCheckboxes" id=` + item.cityId + ` value=` + item.cityId + ` multiple><label class="form-check-label" for=` + item.cityId + `>` + item.name + `</label></li>`
                })
                dropdown.html(items);
            }
        });
    }
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
                    getMission();
                    //filter()
                });

                // Add "Close All" button
                if (closeAllButton.length === 0) {
                    searchedFilters.append('<div class="pill closeAll">Close All</div>');
                    searchedFilters.children('.closeAll').click(function () {
                        allDropdowns.find('input[type="checkbox"]').prop('checked', false);
                        searchedFilters.empty();
                        getMission();
                        //filter()
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

    // declared globally because we have to use it in another function

    $("#countryDropdown li").click(function () {
        var countryId = $(this).val();
        getCitiesByCountry(countryId);

        selectedCountry = $(this).val();
        getMission()
        //$('.card').each(function () {
        //    var cardCountry = $(this).find('.mission-country').text();
        //    if (selectedCountry.toLowerCase() == cardCountry.toLowerCase()) {
        //        $(this).parent().show();
        //    } else {
        //        $(this).parent().hide();
        //    }
        //});
        //filter()

    });

    // three functions for comparing city theme and skills
    //function compareCity(selectedCities, cardCity) {
    //    let isMatch = selectedCities.some(function (selectedCity) {
    //        return cardCity.trim() == selectedCity.text().trim();
    //    })
    //    return isMatch;
    //}

    //function compareTheme(selectedThemes, cardTheme) {
    //    let isMatch = selectedThemes.some(function (selectedTheme) {
    //        return cardTheme === selectedTheme.text();
    //    })
    //    return isMatch;
    //}

    //function compareSkill(selectedSkills, cardSkills) {
    //    let isMatch = selectedSkills.some(function (selectedSkill) {
    //        return cardSkills.some(function (cardSkill) {
    //            return cardSkill === selectedSkill.text();
    //        })
    //    })
    //    return isMatch;
    //}

    // function for filter all cards when clicked
    //function filter() {

    //    let selectedCities = $('input[type="checkbox"][name="cityCheckboxes"]:checked').map(function () {
    //        return $(this).next('label');
    //    }).get();

    //    let selectedThemes = $('input[type="checkbox"][name="themeCheckboxes"]:checked').map(function () {
    //        return $(this).next('label');
    //    }).get();

    //    let selectedSkills = $('input[type="checkbox"][name="skillCheckboxes"]:checked').map(function () {
    //        return $(this).next('label');
    //    }).get();
    //    let atleastOneCity = (selectedCities.length !== 0);
    //    let noCity = (selectedCities.length === 0);

    //    let atleastOneTheme = (selectedThemes.length !== 0);
    //    let noTheme = (selectedThemes.length === 0);

    //    let atleastOneSkill = (selectedSkills.length !== 0);
    //    let noSkill = (selectedSkills.length === 0);

    //    if (noCity && noTheme && noSkill && selectedCountry == null) {
    //        // If no themes are selected, show all cards
    //        console.log("no themes and skills selected")
    //        $('.card').parent().show();
    //    } else {
    //        // Otherwise, loop over all cards and compare themes and cities and skills
    //        $('.card').each(function () {
    //            var cardCountry = $(this).find('.mission-country').text();
    //            let cardCity = $(this).find('.location').text();
    //            let cardTheme = $(this).find('.mission-theme h3').text();
    //            let cardSkills = $(this).find('.mission-skills').map(function () {
    //                return $(this).text();
    //            }).get();
    //            if (selectedCountry != null) {
    //                if (atleastOneCity) {
    //                    if (atleastOneTheme) {
    //                        if (atleastOneSkill) {
    //                            let isMatch = (selectedCountry == cardCountry) && compareCity(selectedCities, cardCity) && compareTheme(selectedThemes, cardTheme) && compareSkill(selectedSkills, cardSkills);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                        else {
    //                            let isMatch = (selectedCountry == cardCountry) && compareCity(selectedCities, cardCity) && compareTheme(selectedThemes, cardTheme);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                    }
    //                    else {
    //                        if (atleastOneSkill) {
    //                            let isMatch = (selectedCountry == cardCountry) && compareCity(selectedCities, cardCity) && compareSkill(selectedSkills, cardSkills);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                        else {
    //                            let isMatch = (selectedCountry == cardCountry) && compareCity(selectedCities, cardCity);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                    }
    //                }
    //                else {
    //                    if (atleastOneTheme) {
    //                        if (atleastOneSkill) {
    //                            let isMatch = (selectedCountry == cardCountry) && compareTheme(selectedThemes, cardTheme) && compareSkill(selectedSkills, cardSkills);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                        else {
    //                            let isMatch = (selectedCountry == cardCountry) && compareTheme(selectedThemes, cardTheme);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                    }
    //                    else {
    //                        if (atleastOneSkill) {
    //                            let isMatch = (selectedCountry == cardCountry) && compareSkill(selectedSkills, cardSkills);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                        else {
    //                            let isMatch = (selectedCountry == cardCountry);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();

    //                        }
    //                    }
    //                }
    //            }
    //            else {
    //                if (atleastOneCity) {
    //                    if (atleastOneTheme) {
    //                        if (atleastOneSkill) {
    //                            let isMatch = compareCity(selectedCities, cardCity) && compareTheme(selectedThemes, cardTheme) && compareSkill(selectedSkills, cardSkills);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                        else {
    //                            let isMatch = compareCity(selectedCities, cardCity) && compareTheme(selectedThemes, cardTheme);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                    }
    //                    else {
    //                        if (atleastOneSkill) {
    //                            let isMatch = compareCity(selectedCities, cardCity) && compareSkill(selectedSkills, cardSkills);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                        else {
    //                            let isMatch = compareCity(selectedCities, cardCity);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                    }
    //                }
    //                else {
    //                    if (atleastOneTheme) {
    //                        if (atleastOneSkill) {
    //                            let isMatch = compareTheme(selectedThemes, cardTheme) && compareSkill(selectedSkills, cardSkills);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                        else {
    //                            let isMatch = compareTheme(selectedThemes, cardTheme);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                    }
    //                    else {
    //                        if (atleastOneSkill) {
    //                            let isMatch = compareSkill(selectedSkills, cardSkills);
    //                            (isMatch) ? $(this).parent().show() : $(this).parent().hide();
    //                        }
    //                        else {
    //                            $(this).parent().show();
    //                        }
    //                    }
    //                }
    //            }

    //        });
    //    }
    //}

    // for all dropdown
    allDropdowns.each(function () {
        let dropdown = $(this);

        $(this).on('change', 'input[type="checkbox"]', function (e) {
            getMission();
            //filter();

        });
    });

    $('#sortByDropdown li').on('click', function () {
        selectedSortOption = $(this).find('a').text();
        getMission()
    });


    // sortBy functionality
    //$('#sortByDropdown li').on('click', function () {
    //    selectedSortOption = $(this).find('a').text();
    //    getMission()
    //    let gridCardsContainer = $('.grid-card').parent().parent();
    //    let listCardsContainer = $('.list-card').parent().parent();
    //    var dateArray = [];
    //    switch (selectedSortOption) {
    //        case 'Newest':
    //            dateArray = [];

    //            //console.log(dateArray)
    //            let cardsDateForNewest = $('.card').find('.created-date');
    //            cardsDateForNewest.each(function (j) {
    //                dateArray.push($(this).text());
    //            });

    //            dateArray.sort(function (a, b) {
    //                var dateA = new Date(
    //                    parseInt(a.substring(6)),
    //                    parseInt(a.substring(3, 5)) - 1,
    //                    parseInt(a.substring(0, 2))
    //                );
    //                var dateB = new Date(
    //                    parseInt(b.substring(6)),
    //                    parseInt(b.substring(3, 5)) - 1,
    //                    parseInt(b.substring(0, 2))
    //                );
    //                return dateA - dateB;
    //            });
    //            dateArray = $.unique(dateArray)

    //            // Arrange Array Element In Descending order
    //            dateArray.reverse();
    //            //console.log(newestDateArray)
    //            for (let i = 0; i < dateArray.length; i++) {
    //                $('.grid-card').each(function () {
    //                    if ($(this).find('.created-date').text() == dateArray[i]) {
    //                        $(this).parent().appendTo($(gridCardsContainer));
    //                    }
    //                });
    //            }
    //            for (let i = 0; i < dateArray.length; i++) {
    //                $('.list-card').each(function () {
    //                    if ($(this).find('.created-date').text() == dateArray[i]) {
    //                        $(this).parent().appendTo($(listCardsContainer));
    //                    }
    //                });
    //            }
    //            filter()
    //            break;
    //        case 'Oldest':
    //            dateArray = [];
    //            let cardsDateForOldest = $('.card').find('.created-date')
    //            //let dateArray = [];
    //            cardsDateForOldest.each(function (j) {
    //                dateArray.push($(this).text());
    //            });
    //            // Arrange  array Elemeny in Ascending order
    //            dateArray = $.unique(dateArray)
    //            //dateArray.sort(function (a, b) {
    //            //    var dateA = new Date(a);
    //            //    var dateB = new Date(b);
    //            //    return dateA - dateB;
    //            //});

    //            dateArray.sort(function (a, b) {
    //                var dateA = new Date(
    //                    parseInt(a.substring(6)),
    //                    parseInt(a.substring(3, 5)) - 1,
    //                    parseInt(a.substring(0, 2))
    //                );
    //                var dateB = new Date(
    //                    parseInt(b.substring(6)),
    //                    parseInt(b.substring(3, 5)) - 1,
    //                    parseInt(b.substring(0, 2))
    //                );
    //                return dateA - dateB;
    //            });

    //            for (let i = 0; i < dateArray.length; i++) {
    //                $('.grid-card').each(function () {
    //                    if ($(this).find('.created-date').text() == dateArray[i]) {
    //                        console.log(true)

    //                        $(this).parent().appendTo($(gridCardsContainer));
    //                    }
    //                });
    //            }
    //            for (var i = 0; i < dateArray.length; i++) {
    //                $('.list-card').each(function () {
    //                    if ($(this).find('.created-date').text() == dateArray[i]) {
    //                        console.log(true)

    //                        $(this).parent().appendTo($(listCardsContainer));
    //                    }
    //                });
    //            }
    //            filter()
    //            break;
    //        case 'Lowest available seats':
    //            let seatsLeftForLowest = $('.card').find('.seats-left')
    //            var seatsArray = [];
    //            seatsLeftForLowest.each(function (j) {
    //                seatsArray.push($(this).text());
    //            });
    //            // Arrange  array Elemeny in Ascending order
    //            seatsArray = $.unique(seatsArray)
    //            seatsArray.sort()

    //            for (var i = 0; i < seatsArray.length; i++) {
    //                $('.grid-card').each(function () {
    //                    if ($(this).find('.seats-left').text() == seatsArray[i]) {
    //                        console.log(true)

    //                        $(this).parent().appendTo($(gridCardsContainer));
    //                    }
    //                });
    //            }
    //            for (var i = 0; i < seatsArray.length; i++) {
    //                $('.list-card').each(function () {
    //                    if ($(this).find('.seats-left').text() == seatsArray[i]) {
    //                        console.log(true)

    //                        $(this).parent().appendTo($(listCardsContainer));
    //                    }
    //                });
    //            }
    //            filter()
    //            break;
    //        case 'Highest available seats':
    //            let seatsLeft = $('.card').find('.seats-left')
    //            var seatsArray = [];
    //            seatsLeft.each(function (j) {
    //                seatsArray.push($(this).text());
    //            });
    //            // Arrange  array Elemeny in Ascending order
    //            seatsArray = $.unique(seatsArray)
    //            seatsArray.sort()
    //            seatsArray.reverse()
    //            for (var i = 0; i < seatsArray.length; i++) {
    //                $('.grid-card').each(function () {
    //                    if ($(this).find('.seats-left').text() == seatsArray[i]) {
    //                        $(this).parent().appendTo($(gridCardsContainer));
    //                    }
    //                });
    //            }
    //            for (var i = 0; i < seatsArray.length; i++) {
    //                $('.list-card').each(function () {
    //                    if ($(this).find('.seats-left').text() == seatsArray[i]) {
    //                        $(this).parent().appendTo($(listCardsContainer));
    //                    }
    //                });
    //            }
    //            filter()
    //            break;
    //        case 'My favourites':
    //            //let favButton = $('.bi-heart-fill');

    //            $('.grid-card').each(function () {
    //                let favButton = $(this).find('.favorite-button');
    //                if (favButton.hasClass('bi-heart-fill')) {
    //                    $(this).parent().show();
    //                }
    //                else {
    //                    $(this).parent().hide();
    //                }
    //            });
    //            $('.list-card').each(function () {
    //                let favButton = $(this).find('.favorite-button');
    //                if (favButton.hasClass('bi-heart-fill')) {
    //                    $(this).parent().show();
    //                }
    //                else {
    //                    $(this).parent().hide();
    //                }
    //            });
    //            break;
    //        case 'Registration deadline':
    //            let deadlines = $('.card').find('.deadline')
    //            var dateArray = [];
    //            deadlines.each(function (j) {
    //                dateArray.push($(this).text());
    //            });
    //            dateArray.sort(function (a, b) {
    //                var dateA = new Date(
    //                    parseInt(a.substring(6)),
    //                    parseInt(a.substring(3, 5)) - 1,
    //                    parseInt(a.substring(0, 2))
    //                );
    //                var dateB = new Date(
    //                    parseInt(b.substring(6)),
    //                    parseInt(b.substring(3, 5)) - 1,
    //                    parseInt(b.substring(0, 2))
    //                );
    //                return dateA - dateB;
    //            });
    //            // Arrange  array Elemeny in Ascending order
    //            console.log(dateArray)
    //            for (var i = 0; i < dateArray.length; i++) {
    //                $('.grid-card').each(function () {
    //                    if ($(this).find('.deadline').text() == dateArray[i]) {
    //                        $(this).parent().appendTo($(gridCardsContainer));
    //                    }
    //                });
    //            }
    //            for (var i = 0; i < dateArray.length; i++) {
    //                $('.list-card').each(function () {
    //                    if ($(this).find('.deadline').text() == dateArray[i]) {
    //                        $(this).parent().appendTo($(listCardsContainer));
    //                    }
    //                });
    //            }
    //            filter()
    //            break;
    //    }
    //})
    // add to favourite

    $(document).on('click', 'i.favorite-button', function () {
        var missionId = $(this).data('mission-id');
        $.ajax({
            url: '/Mission/AddToFavorites',
            type: 'POST',
            data: { missionId: missionId },
            success: function (result) {
                // Show a success message or update the UI
                console.log(missionId)
                var allMissionId = $('.favorite-button')
                allMissionId.each(function () {
                    if ($(this).data('mission-id') === missionId) {
                        if ($(this).hasClass('bi-heart')) {
                            $(this).addClass('bi-heart-fill text-danger')
                            $(this).removeClass('bi-heart text-light')
                            $(this).next('span').text('Remove From Favorites')
                        }
                        else {
                            $(this).addClass('bi-heart text-light')
                            $(this).removeClass('bi-heart-fill text-danger')
                            $(this).next('span').text('Add to Favorites')
                        }
                    }
                })
            },
            error: function (error) {
                // Show an error message or handle the error
                console.log("error")

            }
        });
    });


    // rating functionality on volunteering mission page

    // Get all star elements
    var stars = $('.star-capsule').find('i');

    // Add click event listener to each star
    stars.click(function () {
        var missionId = $('.favorite-button').data('mission-id');

        // Get the index of the clicked star
        var index = $(this).data('star');

        // Remove text-warning class from all stars
        stars.removeClass('text-warning');

        // Add text-warning class to all stars up to the clicked star
        for (var i = 1; i <= index; i++) {
            stars.filter('[data-star=' + i + ']').addClass('text-warning');
        }

        // Make AJAX call to update rating in database
        $.ajax({
            type: 'POST',
            url: "/Mission/UpdateRating",
            data: { missionId: missionId, userId: userId, rating: index }, // Replace "yourMissionId" and "yourUserId" with the actual values
            success: function (data) {
                // Handle the response from the server if needed
                console.log("success")
            },
            error: function () {
                // Handle the error if needed
                console.log("error")

            }
        });
    });

    // mission application functionality (pending)

    $('#confirmApply').click(function () {
        var missionId = $('.favorite-button').data('mission-id');

        // Submit application here
        var application = {
            MissionId: missionId,
            UserId: userId,
            AppliedAt: new Date(),
            ApprovalStatus: 'pending'
        };
        $.ajax({
            url: '/Mission/Apply',
            type: 'POST',
            data: application,
            success: function (response) {
                // Handle success here
                $("#applyButton").text("Applied").prop("disabled", true);
            },
            error: function (error) {
                // Handle error here
            }
        });
        $('#confirmationModal').modal('hide');
    });



    


    function getMission(pageNo) {
        let searchText = $(".search-field input").val();

        let selectedCities = $('input[type="checkbox"][name="cityCheckboxes"]:checked').map(function () {
            return $(this).val();
        }).get();

        let selectedThemes = $('input[type="checkbox"][name="themeCheckboxes"]:checked').map(function () {
            return $(this).val();
        }).get();

        let selectedSkills = $('input[type="checkbox"][name="skillCheckboxes"]:checked').map(function () {
            return $(this).val();
        }).get();
        let pageSize = 6;

        let inputData = {
            selectedCountry: selectedCountry !== "" ? selectedCountry : null,
            selectedCities: selectedCities !== "" ? selectedCities.join() : null,
            selectedThemes: selectedThemes !== "" ? selectedThemes.join() : null,
            selectedSkills: selectedSkills !== "" ? selectedSkills.join() : null,
            searchText: searchText !== "" ? searchText : null,
            selectedSortOption: selectedSortOption !== undefined ? selectedSortOption : null,
            userId: userId,
            pageSize: pageSize,
            pageNo: pageNo !== undefined ? pageNo : 1
        }

        $.ajax({
            url: '/Mission/LandingPage',
            type: 'POST',
            data: inputData,
            success: function (response) {
                // Handle success here

                var cardsContainer = $('.card-container-list-grid');
                cardsContainer.empty();
                cardsContainer.append(response)

                var totalRecords = document.getElementById('total-records').innerText;
                let totalPages = Math.ceil(totalRecords / pageSize);
                console.log(totalRecords)
                console.log(totalPages)

                if (totalPages <= 1) {
                    $('#pagination-container').parent().parent().hide();
                }
                let paginationHTML = `
  <li class="page-item">
    <a class="pagination-link first-page" aria-label="Previous">
      <span aria-hidden="true"><img src="/images/previous.png" /></span>
    </a>
  </li>
  <li class="page-item">
    <a class="pagination-link previous-page" aria-label="Previous">
      <span aria-hidden="true"><img src="/images/left.png" /></span>
    </a>
  </li>`;

                for (let i = 1; i <= totalPages; i++) {
                    let activeClass = '';
                    if (i === (pageNo === undefined ? 1 : pageNo)) {
                        activeClass = ' active';
                    }
                    paginationHTML += `
    <li class="page-item ${activeClass}">
        <a class="pagination-link" data-page="${i}">${i}</a>
    </li>`;
                }

                paginationHTML += `
  <li class="page-item">
    <a class="pagination-link next-page" aria-label="Next">
      <span aria-hidden="true"><img src="/images/right-arrow1.png" /></span>
    </a>
  </li>
  <li class="page-item">
    <a class="pagination-link last-page" aria-label="Next">
      <span aria-hidden="true"><img src="/images/next.png" /></span>
    </a>
  </li>`;

                $('#pagination-container').empty()
                $('#pagination-container').append(paginationHTML)


                // pagination
                let currentPage;

                $(document).on('click', '.pagination li', (function () {
                    $('.pagination li').each(function () {
                        if ($(this).hasClass('active')) {

                            currentPage = $(this).find('a').data('page');
                            $(this).removeClass('active');
                        }
                    })
                    pageNo = currentPage;
                    if ($(this).find('a').hasClass('first-page')) {
                        pageNo = 1;
                        currentPage = pageNo;
                        $('.pagination li').find('a').each(function () {
                            if ($(this).data('page') == 1) {
                                $(this).parent().addClass('active')
                            }
                        })
                    }
                    else if ($(this).find('a').hasClass('last-page')) {
                        pageNo = totalPages;
                        currentPage = pageNo;
                        $('.pagination li').find('a').each(function () {
                            if ($(this).data('page') == totalPages) {
                                $(this).parent().addClass('active')
                            }
                        })
                    }
                    else if ($(this).find('a').hasClass('previous-page')) {
                        if (currentPage > totalPages) {
                        pageNo = currentPage - 1;
                        }
                        $('.pagination li').find('a').each(function () {
                            if ($(this).data('page') == pageNo) {
                                $(this).parent().addClass('active')
                            }
                        })
                        currentPage = pageNo;

                    } else if ($(this).find('a').hasClass('next-page')) {
                        if (currentPage < totalPages) {
                            pageNo = currentPage + 1;
                        }

                        $('.pagination li').find('a').each(function () {
                            if ($(this).data('page') == pageNo) {
                                $(this).parent().addClass('active')
                            }
                        })
                        currentPage = pageNo;

                    } else {
                        $(this).addClass('active')

                        pageNo = $(this).find('a').data('page');
                        currentPage = pageNo;

                    }
                    getMission(pageNo);
                }));
            },
            error: function (error) {
                console.log(error);

                // Handle error here
            }
        });
    }



    //    if (selectedSkills.length === 0) {
    //        // If no themes are selected, show all cards
    //        console.log("no themes and skills selected")
    //        $('.card').parent().show();
    //    } else {
    //        // Otherwise, loop over all cards and compare themes and cities
    //        $('.card').each(function () { // skills dropdown

    //$('.dropdown #skillDropdown').on('change', 'input[type="checkbox"]', function () {
    //    //console.log("on chnging the dropdown")
    //    var selectedSkills = $('input[type="checkbox"]:checked').map(function () {
    //        return $(this).next('label');
    //    }).get();
    //    console.log(selectedSkills)

    //    //var selectedSkills = $('input[name="skillCheckboxes[]"]:checked').map(function () {
    //    //    return $(this).next('label');
    //    //}).get();
    //            //var cardSkills = $(this).find('mission-skills').text();
    //            var cardSkills = $(this).find('.mission-skills').map(function () {
    //                return $(this).text();
    //            }).get();
    //            console.log(cardSkills)

    //            var isMatch = selectedSkills.some(function (selectedSkill) {
    //                return cardSkills.some(function (cardSkill) {
    //                    return cardSkill === selectedSkill.text();
    //                })
    //            })

    //            if (isMatch) {
    //                $(this).parent().show();
    //            } else {
    //                $(this).parent().hide();
    //            }
    //        });
    //    }
    //})



    // theme dropdown

    //$('.dropdown #themeDropdown').on('change', 'input[type="checkbox"]', function () {
    //    //console.log("on chnging the dropdown")
    //    var selectedThemes = $('input[type="checkbox"]:checked').map(function () {
    //        return $(this).next('label');
    //    }).get();
    //    //var selectedSkills = $('input[name="skillCheckboxes[]"]:checked').map(function () {
    //    //    return $(this).next('label');
    //    //}).get();


    //    if (selectedThemes.length === 0) {
    //        // If no themes are selected, show all cards
    //        console.log("no themes and skills selected")
    //        $('.card').parent().show();
    //    } else {
    //        // Otherwise, loop over all cards and compare themes and cities
    //        $('.card').each(function () {
    //            var cardTheme = $(this).find('.mission-theme h3').text();


    //            //var cardskills = $(this).find('.mission-theme h3').text();
    //            //console.log(cardTheme);

    //            var isMatch = selectedThemes.some(function (selectedTheme) {
    //                //console.log(selectedTheme.text());
    //                //console.log(cardTheme === selectedTheme.text())
    //                return cardTheme === selectedTheme.text();
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

    // city dropdown
    //$('.dropdown #cityDropdown').on('change', 'input[type="checkbox"]', function () {
    //    //console.log("on chnging the dropdown")

    //    var selectedCities = $('input[type="checkbox"]:checked').map(function () {
    //        return $(this).next('label');
    //    }).get();

    //    if (selectedCities.length === 0) {
    //        // If no themes are selected, show all cards
    //        $('.card').parent().show();
    //    } else {
    //        //console.log(selectedCities);
    //        $('.card').each(function () {
    //            var cardCity = $(this).find('.location').text();

    //            var isMatch = selectedCities.some(function (selectedCity) {
    //                return cardCity.trim().toLowerCase() == selectedCity.text().trim().toLowerCase();
    //            })

    //            if (isMatch) {
    //                $(this).parent().show();
    //            } else {
    //                $(this).parent().hide();
    //            }
    //        });
    //    }
    //})

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

})