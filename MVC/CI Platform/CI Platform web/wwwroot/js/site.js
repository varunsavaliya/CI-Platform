$(document).ready(function () {
    $('#loginModal').modal('show');
    // declared globally because we have to use it in another function
    var currentUrl = window.location.href;
    let selectedCountry = null;
    let selectedSortOption = null;
    let sortOrder = null;
    let explore = null;
    if (currentUrl.includes("LandingPage")) {
        getMission()
        $('#exploreDropdown').parent().prop('hidden', false);
    } else if (currentUrl.includes("StoriesListing")) {
        getStory()
    }
    $(".search-field input").keyup(function () {
        if (currentUrl.includes("LandingPage")) {
            explore = null;
            getMission()
        } else if (currentUrl.includes("StoriesListing")) {
            getStory()
        }
    });
    $(document).on('click', '#exploreDropdown li', function () {
        sortOrder = null;
        selectedSortOption = null;
        explore = $(this).find('a').text();
        getMission();
    })


    // cities according to country
    function getCitiesByCountry(countryId) {
        $.ajax({
            type: "GET",
            url: "/Mission/GetCitiesByCountry",
            data: { countryId: countryId },
            success: function (data) {
                if (currentUrl.includes("LandingPage") || currentUrl.includes('StoriesListing')) {
                    var dropdown = $("#cityDropdown");
                    dropdown.empty();
                    var items = "";
                    $(data).each(function (i, item) {
                        items += `<li class="form-check ps-4"><input type="checkbox" class="form-check-input me-2" name="cityCheckboxes" id=` + item.cityId + ` value=` + item.cityId + ` multiple><label class="form-check-label" for=` + item.cityId + `>` + item.name + `</label></li>`
                    })
                    dropdown.html(items);

                    var dropdown = $("#cityDropdownOffCanvas");
                    dropdown.empty();
                    var items = "";
                    $(data).each(function (i, item) {
                        items += `<li class="form-check ps-4"><input type="checkbox" class="form-check-input me-2" name="cityCheckboxes" id=` + item.cityId + ` value=` + item.cityId + ` multiple><label class="form-check-label" for=` + item.cityId + `>` + item.name + `</label></li>`
                    })
                    dropdown.html(items);
                }
                else if (currentUrl.includes("UserProfile") || currentUrl.includes('Admin')) {
                    var select = $('.user-city');
                    select.empty();
                    var items = `<option value="0">Select your city</option>`;
                    $(data).each(function (i, item) {
                        items += `<option value=` + item.cityId + `>` + item.name + `</option>`
                    })
                    select.append(items)
                }
            }
        });
    }

    // notification functionalities
    let notificationCount = $('.notification-count').text();
    let allNotifications = $('.notifications-section div').length;
    console.log($('.notifications-section div').length)
    if (notificationCount > 0 && currentUrl.includes('LandingPage'))
    {
        $('.notification-icon-container').click();
    }
    if (notificationCount < 1) {
        $('.notification-container').each(function () {
            $(this).find('i.fa-solid.fa-circle').removeClass('fa-solid fa-circle text-warning').addClass('fa-solid fa-circle-check text-secondary');
            $(this).removeClass('unread');
        });
    }
    if (allNotifications < 1) {
        $('.no-notification-section').show();
    }

    $(document).on('click', '.notification-settings-icon', function (e) {
        e.stopPropagation();
        $(this).hide();
        $('.notification-heading').text('Notification Settings');
        $('.clear-all-notification').hide();
        $('.notifications-section').hide();
        $('.notification-settings-section').show();
        $('.no-notification-section').hide();
    })

    $(document).on('click', '#notification-settings-cancel-btn', function (e) {
        e.stopPropagation();
        $('.notification-settings-icon').show();
        $('.notification-heading').text('Notifications');
        $('.clear-all-notification').show();
        $('.notifications-section').show();
        $('.notification-settings-section').hide();
        console.log($('.notification-count').text())
        if ($('.notification-count').text() < 1) {
            $('.no-notification-section').show();
        }
    })

    $(document).on('click', '.clear-all-notification', function (e) {
        e.stopPropagation();
        $('.notification-container').each(function (index) {
            $(this).delay(index * 50).queue(function () {
                $(this).css('transform', 'translateX(100%)').dequeue();
            });
        });

        $.ajax({
            url: "/Notification/ClearNotifications",
            type: "POST",
            success: function (response) {
                setTimeout(function () {
                    $('.no-notification-section').show();
                    $('.notifications-section').empty();
                    $('.notifications-section').css({
                        'height': 'min-content',
                        'overflow-y': 'auto'
                    });

                    $('.notification-count').hide();
                    $('.notification-count').text(0);
                }, 500);
            },
            error: function (xhr) {
                console.log(xhr.responseText);
            }
        });
        

    })

    $('.notification-icon-container').click(function () {
        var notificationIds = [];

        $('.notification-container.unread').each(function () {
            notificationIds.push(parseInt($(this).data('id')));
        });

        $.ajax({
            url: "/Notification/HandleNotificationStatus",
            type: "POST",
            data: { notificationIds: notificationIds },
            success: function (response) {
                setTimeout(function () {
                    $('.notification-count').hide();
                    $('.notification-container.unread').each(function () {
                        $(this).find('i.fa-solid.fa-circle').removeClass('fa-solid fa-circle text-warning').addClass('fa-solid fa-circle-check text-secondary');
                        $(this).removeClass('unread');
                    });
                }, 1000);
            },
            error: function (xhr) {
                console.log(xhr.responseText);
            }
        });
    });


    $(".save-notification-settings").click(function (e) {
        e.stopPropagation();
        var notificationSettings = [];

        $("input[type=checkbox]:checked").each(function () {
            notificationSettings.push(parseInt($(this).attr("id")));
        });

        $.ajax({
            url: "/Notification/HandleNotificationSettings",
            type: "POST",
            data: { notificationSettings: notificationSettings },
            success: function (response) {
                $('.notification-settings-icon').show();
                $('.notification-heading').text('Notifications');
                $('.clear-all-notification').show();
                $('.notifications-section').show();
                $('.notification-settings-section').hide();
                if ($('.notification-count').text() < 1) {
                    $('.no-notification-section').show();
                }
            },
            error: function (xhr) {
                console.log(xhr.responseText);
            }
        });
    });



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
                    if (currentUrl.includes("LandingPage")) {
                        explore = null;
                        getMission()
                    } else if (currentUrl.includes("StoriesListing")) {
                        getStory()
                    }
                });

                // Add "Close All" button
                if (closeAllButton.length === 0) {
                    searchedFilters.append('<div class="pill closeAll">Close All</div>');
                    searchedFilters.children('.closeAll').click(function () {
                        selectedCountry = null;
                        allDropdowns.find('input[type="checkbox"]').prop('checked', false);
                        searchedFilters.empty();
                        if (currentUrl.includes("LandingPage")) {
                            explore = null;
                            getMission()
                        } else if (currentUrl.includes("StoriesListing")) {
                            getStory()
                        }
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

    $("#countryDropdown li").click(function () {
        allDropdowns.find('input[type="checkbox"]').prop('checked', false);
        searchedFilters.empty();
        var countryId = $(this).val();
        getCitiesByCountry(countryId);

        selectedCountry = $(this).val();
        if (currentUrl.includes("LandingPage")) {
            explore = null;
            getMission()
        } else if (currentUrl.includes("StoriesListing")) {
            getStory()
        }

    });

    // for all dropdown
    allDropdowns.each(function () {
        $(this).on('change', 'input[type="checkbox"]', function (e) {
            if (currentUrl.includes("LandingPage")) {
                explore = null;
                getMission()
            } else if (currentUrl.includes("StoriesListing")) {
                getStory()
            }
        });
    });

    $('#sortByDropdown li').on('click', function () {
        selectedSortOption = $(this).find('a').text();
        if (selectedSortOption == 'Newest') {
            selectedSortOption = 'CreatedAt'
            sortOrder = 'Desc'
        } else if (selectedSortOption == 'Oldest') {
            selectedSortOption = 'CreatedAt'
            sortOrder = 'Asc'
        } else if (selectedSortOption == 'Lowest available seats') {
            selectedSortOption = 'SeatsLeft'
            sortOrder = 'Asc'
        } else if (selectedSortOption == 'Highest available seats') {
            selectedSortOption = 'SeatsLeft'
            sortOrder = 'Desc'
        } else if (selectedSortOption == 'Registration deadline') {
            selectedSortOption = 'StartDate'
            sortOrder = 'Asc'
        } else if (selectedSortOption == 'My favourites') {
            selectedSortOption = 'Favorites'
            sortOrder = 'Asc'
        }
        if (currentUrl.includes("LandingPage")) {
            explore = null;
            getMission()
        } else if (currentUrl.includes("StoriesListing")) {
            getStory()
        }
    });

    // add to favourite
    $(document).on('click', 'i.favorite-button', function () {
        var missionId = $(this).data('mission-id');
        $.ajax({
            url: '/Mission/AddToFavorites',
            type: 'POST',
            data: { missionId: missionId },
            success: function (result) {
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

            }
        });
    });

    // swiper images both arrows
    if (currentUrl.includes('MissionVolunteering') || currentUrl.includes('StoryDetail')) {

        var swiper = new Swiper(".mySwiper", {
            loop: true,
            spaceBetween: 10,
            slidesPerView: 4,
            freeMode: true,
            watchSlidesProgress: true,
        });
        var swiper2 = new Swiper(".mySwiper2", {
            loop: true,
            spaceBetween: 10,
            navigation: {
                nextEl: ".swiper-button-next",
                prevEl: ".swiper-button-prev",
            },
            thumbs: {
                swiper: swiper,
            },
        });
    }

    // rating functionality on volunteering mission page
    // Get all star elements
    var stars = $('.star-capsule').find('i');
    // Add click event listener to each star
    stars.click(function () {
        var missionId = $('.favorite-button').data('mission-id');
        // Get the index of the clicked star
        var index = $(this).data('star');
        $.ajax({
            type: 'POST',
            url: "/Mission/UpdateRating",
            data: { missionId: missionId, rating: index },
            success: function (data) {
                // Remove text-warning class from all stars
                stars.removeClass('text-warning');
                // Add text-warning class to all stars up to the clicked star
                for (var i = 1; i <= index; i++) {
                    stars.filter('[data-star=' + i + ']').addClass('text-warning');
                }
            },
            error: function () {

            }
        });
    });

    // mission application functionality
    $('#confirmation').click(function () {
        var missionId = $('.favorite-button').data('mission-id');
        // Submit application here
        $.ajax({
            url: '/Mission/Apply',
            type: 'POST',
            data: { MissionId: missionId },
            success: function (response) {
                $('.apply-btn').text('Applied').addClass('published-btn btn btn-success disabled');
                $('.apply-btn').removeClass('card-button');
                swal.fire({
                    position: 'center',
                    icon: response.icon,
                    title: response.message,
                    showConfirmButton: false,
                    timer: 3000
                })
            },
            error: function (error) {

            }
        });
        $('#confirmationModal').modal('hide');
    });

    function getMission(pageNo) {
        let searchText = $(".search-field input").val();

        let selectedCities = $('input[type="checkbox"][name="cityCheckboxes"]:checked').map(function () {
            return parseInt($(this).val());
        }).get();


        let selectedThemes = $('input[type="checkbox"][name="themeCheckboxes"]:checked').map(function () {
            return parseInt($(this).val());
        }).get();

        let selectedSkills = $('input[type="checkbox"][name="skillCheckboxes"]:checked').map(function () {
            return parseInt($(this).val());
        }).get();
        let pageSize = 9;

        let inputData = {
            CountryId: selectedCountry !== "" ? selectedCountry : null,
            CityIds: selectedCities.length !== 0 ? selectedCities : null,
            ThemeIds: selectedThemes.length !== 0 ? selectedThemes : null,
            SkillIds: selectedSkills.length !== 0 ? selectedSkills : null,
            searchText: searchText !== "" ? searchText : null,
            SortBy: selectedSortOption !== undefined ? selectedSortOption : "Newest",
            Explore: explore,
            SortOrder: sortOrder,
            pageSize: pageSize,
            pageNo: pageNo !== undefined ? pageNo : 1
        }

        $.ajax({
            url: '/Mission/LandingPage',
            type: 'POST',
            data: inputData,
            success: function (response) {
                var cardsContainer = $('.card-container-list-grid');
                cardsContainer.empty();
                cardsContainer.append(response)

                if (document.getElementById('total-records') !== null) {
                    var totalRecords = document.getElementById('total-records').innerText;
                }
                let totalPages = Math.ceil(totalRecords / pageSize);

                if (totalPages <= 1) {
                    $('#pagination-container').parent().parent().hide();
                }
                else {
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
                    $('#pagination-container').parent().parent().show();
                }

                // pagination
                let currentPage;

                $(document).on('click', '.pagination li', (function () {
                    $('.pagination li').each(function () {
                        if ($(this).hasClass('active')) {

                            currentPage = $(this).find('a').data('page');
                            $(this).removeClass('active');
                        }
                    })
                    let tempPage = currentPage;
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

                        if (currentPage > 1) {
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
                    if (pageNo != tempPage) {

                        getMission(pageNo)
                    }
                }));
                $('.totalMissions').text(totalRecords + ' Missions')
            },
            error: function (error) {
                console.log(error)
            }
        });
    }

    // function for stories with search, filter, pagination
    function getStory(pageNo) {
        let searchText = $(".search-field input").val();

        let selectedCities = $('input[type="checkbox"][name="cityCheckboxes"]:checked').map(function () {
            return parseInt($(this).val());
        }).get();


        let selectedThemes = $('input[type="checkbox"][name="themeCheckboxes"]:checked').map(function () {
            return parseInt($(this).val());
        }).get();

        let selectedSkills = $('input[type="checkbox"][name="skillCheckboxes"]:checked').map(function () {
            return parseInt($(this).val());
        }).get();
        let pageSize = 6;

        let inputData = {
            CountryId: selectedCountry !== "" ? selectedCountry : null,
            CityIds: selectedCities.length !== 0 ? selectedCities : null,
            ThemeIds: selectedThemes.length !== 0 ? selectedThemes : null,
            SkillIds: selectedSkills.length !== 0 ? selectedSkills : null,
            searchText: searchText !== "" ? searchText : null,
            pageSize: pageSize,
            pageNo: pageNo !== undefined ? pageNo : 1
        }

        $.ajax({
            url: '/Story/StoriesListing',
            type: 'POST',
            data: inputData,
            success: function (response) {
                let storiesContainer = $('#story-container');
                storiesContainer.empty();
                storiesContainer.append(response);


                if (document.getElementById('total-stories') !== null) {
                    var totalRecords = document.getElementById('total-stories').innerText;
                }
                let totalPages = Math.ceil(totalRecords / pageSize);

                if (totalPages <= 1) {
                    $('#pagination-container').parent().parent().hide();
                }
                else {
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
                    $('#pagination-container').parent().parent().show();
                }

                // pagination
                let currentPage;

                $(document).on('click', '.pagination li', (function () {
                    $('.pagination li').each(function () {
                        if ($(this).hasClass('active')) {

                            currentPage = $(this).find('a').data('page');
                            $(this).removeClass('active');
                        }
                    })
                    let tempPage = currentPage;

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

                        if (currentPage > 1) {
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
                    if (pageNo != tempPage)
                        getStory(pageNo)
                }));
            }
        })
    }

    // add comment in mission volunteering page
    $('#comment-form').submit(function (e) {
        e.preventDefault()
        if ($(this).valid()) {

            var formData = $(this).serialize();
            $.ajax({
                url: '/Mission/AddComment',
                type: 'POST',
                data: formData,
                success: function (response) {
                    $('.comment-box').empty()
                    swal.fire({
                        position: 'center',
                        icon: response.icon,
                        title: response.message,
                        showConfirmButton: false,
                        timer: 1500
                    })
                },
                error: function (error) {

                }
            })
        }
    })

    // Invite user for volunteering mission page, Landing page, and story detail page
    $(document).on('click', '.invite-button', function (e) {
        e.preventDefault();
        let ToUserID = $(this).parent().parent().find('#user-name').data('userid');
        let controllerAction;
        let Id
        if (currentUrl.includes('StoryDetail')) {
            controllerAction = '/Story/StoryInvite'
            Id = $(this).data('story-id');
        } else {
            controllerAction = '/Mission/MissionInvite'
            Id = $(this).data('mission-id');
        }
        $.ajax({
            url: controllerAction,
            type: 'POST',
            data: { ToUserId: ToUserID, Id: Id, FromUserId: userId },
            success: function (response) {
                $('.Invited-' + ToUserID).html(' <button class="btn btn-success" data-mission-Id="@card.MissionId">Invited</button>');
            }
        })
    })

    let pageSize = 3;
    let recentVolunteerPageNo = 1;
    let totalRecentVolunteers = $('#totalVolunteers').val();
    const getVolunteers = (pageNo, button) => {
        let missionId = $('.favorite-button').data('mission-id');
        $.ajax({
            type: "GET",
            url: "/Mission/GetRecentVolunteers",
            data: { missionId: missionId, pageNo: pageNo, pageSize: pageSize },
            success: function (data) {
                let volunteersContainer = $('.recent-volunteers');
                if (totalRecentVolunteers == 0) {
                    volunteersContainer.text('No recent volunteers for this mission').addClass('px-3 py-3');
                    $('#volunteer-pagination').parent().parent().hide()
                } else {

                    volunteersContainer.empty();
                    volunteersContainer.append(data);
                    if (button == 1) {
                        volunteersContainer.addClass('slide-from-left');
                        volunteersContainer.removeClass('slide-from-right');
                    }
                    else if (button == 2) {
                        volunteersContainer.addClass('slide-from-right');
                        volunteersContainer.removeClass('slide-from-left');
                    }
                    let start = (pageNo - 1) * pageSize + 1;
                    let end = Math.min(start + pageSize - 1, totalRecentVolunteers);
                    $('#volunteer-pagination').text((end != totalRecentVolunteers ? start + '-' : '') + end + ' of ' + totalRecentVolunteers + ' Recent Volunteers');
                }
            }
        });
    }


    $('.recent-volunteers-btns div a').click(function () {
        let totalPages = Math.ceil(totalRecentVolunteers / pageSize);
        let volunteersContainer = $('.recent-volunteers');
        if ($(this).data('bs-slide') == "prev") {
            if (recentVolunteerPageNo > 1) {
                recentVolunteerPageNo--;
                getVolunteers(recentVolunteerPageNo, 1);
                volunteersContainer.removeClass('slide-from-left');
            }
        } else {
            if (recentVolunteerPageNo < totalPages) {
                recentVolunteerPageNo++;
                getVolunteers(recentVolunteerPageNo, 2);
                volunteersContainer.removeClass('slide-from-right');
            }
        }
    })
    if (currentUrl.includes('MissionVolunteering')) {
        getVolunteers(recentVolunteerPageNo, 2);
    }

    $(document).on('keyup', '.note-editable', function () {
        var form = $('.StoryForm');
        var actualTextBox = form.find('#actual_text');
        var text = $(this).html();
        actualTextBox.val(text);
    })

    // drag and drop images in share your story page
    var allfiles = [];
    var defaultImage = null;

    function handleFiles(e) {
        $('.image-validation-error').text('');
        // Add dropped images or selected images to the list
        var files = e.target.files || e.originalEvent.dataTransfer.files;

        // Add selected images to the list
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            if (file.size >= 4 * 1024 * 1024) {
                $('.image-validation-error').text('File size exceeds 4 MB: ' + file.name)
                continue;
            }
            var reader = new FileReader();
            allfiles.push(file);
            //formData.append('file', file);

            // Create image preview and close icon
            reader.onload = (function (file) {
                return function (e) {
                    var image = $('<img>').attr('src', e.target.result);
                    var closeIcon = $('<span>').addClass('close-icon').text('x');

                    // Add image and close icon to the list
                    var item = $('<div>').addClass('image-item').append(image).append(closeIcon);
                    imageList.append(item);

                    // Handle close icon click event
                    closeIcon.on('click', function () {
                        item.remove();
                        allfiles.splice(allfiles.indexOf(file), 1);
                        if (file === defaultImage) {
                            defaultImage = null;
                        }
                    });

                    item.on('click', function () {
                        $('.image-item.default').removeClass('default border border-secondary');
                        $(this).addClass('default border border-secondary');
                        defaultImage = file;
                    });
                };
            })(file);

            // Read image file as data URL
            reader.readAsDataURL(file);
        }
        // Create a new DataTransfer object
        var dataTransfer = new DataTransfer();
        // Create a new FileList object from the DataTransfer object
        fileList = dataTransfer.files;
    }

    //var allfiles = new DataTransfer().files;
    let dropzone = $('#dropzone');
    let imageList = $('#image-list');
    function handleFileDrop(e) {
        e.preventDefault();
        e.stopPropagation();

        // Remove dropzone highlight
        dropzone.removeClass('dragover');
        $('.note-dropzone').remove();
        //$('.note-dropzone-message').remove();
        handleFiles(e);
    }

    // Handle file drop event
    dropzone.on('drop', '#dropzone', function (e) {
        handleFileDrop(e);

    });

    // Handle file dragover event
    dropzone.on('dragover', function (e) {
        e.preventDefault();
        e.stopPropagation();

        // Highlight dropzone
        dropzone.addClass('dragover');
    });

    // Handle file dragleave event
    dropzone.on('dragleave', function (e) {
        e.preventDefault();
        e.stopPropagation();

        // Remove dropzone highlight
        dropzone.removeClass('dragover');
    });


    // Handle file input change event
    $(document).on('change', '#actual-file-input', function (e) {
        handleFiles(e);
    });

    if (currentUrl.includes('ShareStory')) {

        $('#editor').summernote({
            height: 200, // set the height of the editor
            toolbar: [
                // add formatting options to the toolbar
                ['style', ['bold', 'italic', 'strikethrough', 'subscript', 'superscript', 'underline']]
            ]
        });
    }
    let storyTitle;
    $('#storyTitle').on('keyup', function () {
        $('#storyTitleValidation').text('')
        storyTitle = $(this).val();
    })
    let story;
    $('.note-editable').on('blur', function () {
        $('#storyValidation').text('')
        story = $(this).html();
    })
    // getting all data from form and add it to the formdata
    var formDetails = new FormData();
    let missionId;
    $('#select-mission').on('change', function () {
        $('#selectMissionValidation').text('')
        missionId = $(this).val();
        $.ajax({
            url: '/Story/GetStory',
            type: 'GET',
            data: { missionId: missionId },
            success: async function (result) {
                imageList.empty();
                $('#storyTitle').val("");
                $('.note-editable').empty();
                $('#url').val("");
                $('#storyTitle').prop('disabled', false);
                $('.note-editable').prop('contenteditable', true);
                if (result != null && result.message != null) {
                    swal.fire({
                        position: 'center',
                        icon: result.icon,
                        title: result.message,
                        showConfirmButton: false,
                        timer: 3000
                    })
                }
                if (result != null && result.message == "You have already added story for this mission") {
                    $('#storyTitle').prop('disabled', true);
                    $('.note-editable').prop('contenteditable', false);

                }
                allfiles = [];
                //$('.note-editable').html(result.description);
                if (result != null) {
                    $('#storyTitle').val(result.Title);
                    $('.note-editable').html(result.Description);
                    storyTitle = $('#storyTitle').val();
                    story = $('.note-editable').html();
                    // adding all urls in url box
                    let url = '';
                    for (var i in result.StoryMedia.$values) {
                        if (result.StoryMedia.$values.hasOwnProperty(i)) {
                            var obj = result.StoryMedia.$values[i];
                            if (obj.Type === "VIDEO") {
                                url += obj.Path + `\n`;
                            }
                            else {
                                // Create a new File object from the image path
                                const response = await fetch('/Upload/' + obj.Path);
                                const blob = await response.blob();
                                const file = new File([blob], obj.Path, { type: blob.type });
                                // Add the new file to the allfiles array
                                allfiles.push(file);

                                var image = $('<img>').attr('src', '../Upload/' + obj.Path);
                                var closeIcon = $('<span>').addClass('close-icon').text('x');

                                // Add image and close icon to the list
                                var item = $('<div>').addClass('image-item').append(image).append(closeIcon);
                                imageList.append(item);

                                // Handle close icon click event
                                item.on('click', '.close-icon', function (event) {
                                    let index = $(this).parent().index();
                                    $(this).parent().remove();
                                    allfiles.splice(index, 1);
                                });
                            }
                        }
                    }
                    $('#url').val(url);
                    $('#preview-btn').val(result.StoryId)
                }
            },
            error: function (error) {

            }
        });

    });


    function validateForm() {
        if (missionId === '' || missionId === undefined) {
            $('#selectMissionValidation').text('Select the mission to write a story')
            return false;
        }
        if (storyTitle === '' || storyTitle === undefined) {
            $('#storyTitleValidation').text('Title is required')
            return false;
        }
        if (story === '' || story === undefined) {
            $('.note-editor').addClass('mb-1');
            $('#storyValidation').text('Write story for mission')
            return false;
        }

        let youtubeUrls = $('#url').val().trim();

        // Split the input into an array of URLs
        var urlsArray = youtubeUrls.split('\n');
        // Check each URL for validity
        var youtubeRegex = /^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$/i;
        if (youtubeUrls !== '') {

            for (var i = 0; i < urlsArray.length; i++) {
                if (!youtubeRegex.test(urlsArray[i])) {
                    $('#urlValidation').text('Invalid YouTube URL: ' + urlsArray[i]);
                    return false;
                }
            }
        }
        if (urlsArray.length > 20) {
            $('#urlValidation').text('You can not add more than 20 urls');
            return false;
        }
        return true;
    }

    $('.ssbuttons div').on('click', 'button', function (e) {
        e.preventDefault();
        e.stopPropagation();
        if ($(this).val() == 1 || $(this).val() == 2) {
            if (validateForm()) {
                formDetails.append("selectMission", missionId);

                formDetails.append("storyTitle", storyTitle);
                formDetails.append("story", story);
                formDetails.append("Date", $('date').val());
                formDetails.append("button", $(this).val());
                // Loop through all the files in the allfiles array and add them to the DataTransfer object
                for (var i = 0; i < allfiles.length; i++) {
                    formDetails.append('images', allfiles[i]);
                }
                // append all urls in formDetail
                var urls = null;
                var u = $('#url').val();
                if (u != null) {
                    urls = u.split('\n');
                    for (var i = 0; i < urls.length; i++) {
                        formDetails.append("url", urls[i]);
                    }
                }
                else {
                    formDetails.append("url", null);
                }
                $.ajax({
                    url: '/Story/ShareStory',
                    type: 'POST',
                    data: formDetails,
                    processData: false,
                    contentType: false,
                    success: function (result) {
                        swal.fire({
                            position: 'center',
                            icon: result.icon,
                            title: result.message,
                            showConfirmButton: false,
                            timer: 3000
                        })
                    },
                    error: function (error) {

                    }
                });
            }
        }
    })

    $('#preview-btn').click(function () {
        if ($(this).val() == '' || $(this).val() == undefined) {
            swal.fire({
                position: 'center',
                icon: 'warning',
                title: 'Story needs to be saved first',
                showConfirmButton: false,
                timer: 3000
            })
        }
        else {
            let id = $(this).val();
            $.ajax({
                url: '/Story/StoryDetail',
                type: 'GET',
                data: { id: id },
                success: function (result) {
                    var url = '/Story/StoryDetail?id=' + id;
                    window.location.href = url;
                },
                error: function (error) {

                }
            });
        }
    })


    $('.edit-icon').click(function () {
        // Open file input dialog
        $('#profile-image-input').click();
    });

    // Add change event listener to profile image file input
    $('#profile-image-input').change(function () {
        // Read image file and display preview
        var reader = new FileReader();
        reader.onload = function (e) {
            $('.user-image').attr('src', e.target.result);
        }
        reader.readAsDataURL(this.files[0]);
    });

    $(document).on('click', '.form-check-label', function () {
        var checkbox = $(this).prev();
        if (!checkbox.prop('checked')) {
            $(this).addClass('skill-bg');
        } else {
            $(this).removeClass('skill-bg');
        }
    });
    $(document).on('keyup', '#employeeId', function () {
        var employeeId = $(this).val();
        $.ajax({
            url: '/User/ValidateEmployeeId',
            type: 'GET',
            data: { employeeId: employeeId },
            success: function (result) {
                if (result.isValid === true) {
                    // Employee Id is valid
                    $('#employeeId').removeClass('is-invalid');
                    $('span[data-valmsg-for="employeeId"]').empty();
                    $('#employeeId').attr('aria-invalid', false);
                } else {
                    // Employee Id is invalid
                    $('#employeeId').addClass('is-invalid');
                    $('#employeeId').attr('aria-invalid', true);
                    $('span[data-valmsg-for="employeeId"]').text(result.message);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    });



    var uniqueId = 0;
    // Add arrow button event listener
    $('.add-arrow').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        $('.user-skill-options').empty();
        // Get checked checkboxes from all-skill-options
        var checkedOptions = $('.all-skill-options input[type="checkbox"]:checked');

        // Add checked options to user-skill-options
        checkedOptions.each(function () {
            uniqueId++;
            var option = $(this).closest('.form-check').clone();
            $(option).find('label').removeClass('skill-bg').attr('for', 'input-' + uniqueId);
            $(option).find('input[type="checkbox"]').prop('checked', false).attr('id', 'input-' + uniqueId);
            $('.user-skill-options').append(option);
        });
    });

    // Remove arrow button event listener
    $('.remove-arrow').on('click', function () {
        // Get checked checkboxes from user-skill-options
        var checkedOptions = $('.user-skill-options input[type="checkbox"]:checked');

        // Remove checked options from user-skill-options and uncheck them in all-skill-options
        checkedOptions.each(function () {
            var option = $(this).closest('.form-check');
            var optionValue = $(option).find('label').text();
            var checkbox = $('.all-skill-options').find('input[type="checkbox"]');
            checkbox.each(function () {
                if ($(this).next('label').text() == optionValue) {
                    $(this).prop('checked', false);
                    $(this).next('label').removeClass('skill-bg');
                    option.remove();
                }
            })
        });
    });

    var selectedUserSkills = [];
    $('#save-skill-btn').click(function () {
        let skills = $('.user-skill-options').find('label');
        let skillContainer = $('.skill-display');
        skillContainer.empty();
        skills.each(function () {
            let skill = `<span class="mb-2">` + $(this).text() + `</span><br>`;
            skillContainer.append(skill)
        })

        $('.all-skill-options').find('.form-check-input:checked').each(function () {
            selectedUserSkills.push($(this).val());
        });

        $('#selected-skills').val(selectedUserSkills.join(','));
    })

    $(document).on('click', '.user-country', function () {
        let countryId = $(this).val();
        getCitiesByCountry(countryId);
    })

    // change password form in user edit profile
    function validateChangePassForm() {
        if ($('#oldPassword').val() == '') {
            $('#oldPasswordValidation').text('Enter your old password');
            return false;
        }
        if ($('#newPassword').val() == '') {
            $('#newPasswordValidation').text('Enter your new password');
            return false;
        }
        if ($('#confirmPassword').val() == '' || $('#newPassword').val() != $('#confirmPassword').val()) {
            $('#confirmPasswordValidation').text('Password does not match!!');
            return false;
        }
        return true;
    }
    $('#confirmPassword').keyup(function () {
        $('#confirmPasswordValidation').text('');
        validateChangePassForm();
    })
    $('#oldPassword').keyup(function () {
        $('#oldPasswordValidation').text('');
    })
    $('#newPassword').keyup(function () {
        $('#newPasswordValidation').text('');
    })


    $('#change-password-btn').click(function () {
        if (validateChangePassForm()) {
            let oldPass = $('#oldPassword').val();
            let newPass = $('#newPassword').val();
            $.ajax({
                url: '/Home/ChangePassword',
                type: 'POST',
                data: { oldPass: oldPass, newPass: newPass },
                success: function (result) {
                    swal.fire({
                        position: 'center',
                        icon: result.icon,
                        title: result.message,
                        showConfirmButton: false,
                        timer: 3000
                    })
                },
                error: function (error) {

                }
            });
        }
    })

    // contact us
    $('#contact-us-text').click(() => {
        // for resetting form every time its open
        let form = $('#contact-us-form')[0];
        form.reset();
        $('#SubjectValidation').text('');
        $('#MessageValidation').text('');

    })
    const validateContactUs = () => {
        if ($('#Subject').val() == '') {
            $('#SubjectValidation').text('Subject is required');
            return false;
        }
        if ($('#Message').val() == '') {
            $('#MessageValidation').text('Enter your message first');
            return false;
        }
        return true;
    }

    $('#Message, #Subject').keyup(function () {
        $(this).next('span').text('');
    })


    $('#contact-us-form').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        var formData = form.serialize();
        if (validateContactUs()) {
            $.ajax({
                url: '/Home/ContactUs',
                type: 'POST',
                data: formData,
                success: function (result) {
                    swal.fire({
                        position: 'center',
                        icon: result.icon,
                        title: result.message,
                        showConfirmButton: false,
                        timer: 3000
                    })
                },
                error: function (error) {

                }
            });
        }
    })

    // privacy policy page

    $('.policyNavigation a').click(function () {
        $('.policyNavigation').find('a').find('span').removeClass('Pactive');
        $(this).find('span').addClass('Pactive');
    })

    if (currentUrl.includes('PrivacyPolicy')) {
        let allTitles = $('.policyNavigation a');
        allTitles.each(function () {
            if (currentUrl.includes($(this).attr('href'))) {
                $(this).find('span').addClass('Pactive');
            } else {
                $(this).find('span').removeClass('Pactive');
            }
        })
    }


    // volunteering timesheet
    $('#time-mission, #goal-mission').change(function () {
        var selectElement = $(this);
        let missionId = $(this).val();
        $.ajax({
            url: '/Timesheet/GetMissionData',
            type: 'GET',
            data: { missionId: missionId },
            success: function (result) {
                var startDate = new Date(result.StartDate);
                var currentDate = new Date();
                // Set the end date to the current date if it's after the current date
                var endDate = new Date(result.EndDate) > currentDate ? currentDate : new Date(result.EndDate);
                var defaultDate = getRandomDateInRange(startDate, endDate);
                if (selectElement.attr('id') == 'time-mission') {
                    // Destroy the previous instance of Flatpickr
                    $("#time-date").flatpickr().destroy();
                    // Initialize the datepicker
                    var fp = flatpickr("#time-date", {
                        // Set the default date format
                        dateFormat: "d-m-Y",
                        // Disable dates before the start date or after the end date
                        minDate: startDate,
                        maxDate: endDate,
                        defaultDate: defaultDate
                    });
                } else if (selectElement.attr('id') == 'goal-mission') {
                    // Destroy the previous instance of Flatpickr
                    $("#goal-date").flatpickr().destroy();
                    // Initialize the datepicker
                    var fp = flatpickr("#goal-date", {
                        // Set the default date format
                        dateFormat: "d-m-Y",
                        // Disable dates before the start date or after the end date
                        minDate: startDate,
                        maxDate: endDate,
                        defaultDate: defaultDate
                    });
                }

            },
            error: function (error) {

            }
        });
    })

    // for default random date
    function getRandomDateInRange(startDate, endDate) {
        var startTimestamp = startDate.getTime();
        var endTimestamp = endDate.getTime();
        var randomTimestamp = startTimestamp + Math.random() * (endTimestamp - startTimestamp);
        return new Date(randomTimestamp);
    }

    $('table').on('click', '.generic-edit-icon', function () {
        if (currentUrl.includes('AdminUser')) {
            let userId = $(this).parent().find('input').val();
            addOrEdit('AddorEditUser', userId);
        } else if (currentUrl.includes('AdminCMS')) {
            let cmsId = $(this).parent().find('input').val();
            addOrEdit('AddorEditCMS', cmsId);
        } else if (currentUrl.includes('AdminMissionPage')) {
            let missionId = $(this).parent().find('input').val();
            addOrEdit('AddorEditMission', missionId);
        }
        else if (currentUrl.includes('AdminMissionTheme')) {
            let themeId = $(this).parent().find('input').val();
            addOrEdit('AddorEditMissionTheme', themeId);
        } else if (currentUrl.includes('AdminMissionSkill')) {
            let themeId = $(this).parent().find('input').val();
            addOrEdit('AddorEditMissionSkill', themeId);
        } else if (currentUrl.includes('BannerManagement')) {
            let bannerId = $(this).parent().find('input').val();
            addOrEdit('AddorEditBanner', bannerId);
        }
        else {

            let timesheetId = $(this).parent().find('input').val();

            $.ajax({
                url: '/Timesheet/GetTimesheetData',
                type: 'GET',
                data: { timesheetId: timesheetId },
                success: function (response) {
                    if (response.Time == null) {
                        $('#GoalBasedTimesheet_TimesheetId').val(response.TimesheetId)

                        $('#goal-mission option').each(function () {
                            if ($(this).val() == response.MissionId) {
                                $(this).prop('selected', true);
                            }
                        });
                        $('#goal-mission').prop(`disabled`, true);

                        var startDate = new Date(response.Mission.StartDate);
                        var currentDate = new Date();
                        var endDate = new Date(response.Mission.EndDate) > currentDate ? currentDate : new Date(response.Mission.EndDate);
                        var defaultDate = new Date(response.DateVolunteered);
                        $("#goal-date").flatpickr().destroy();
                        var fp = flatpickr("#goal-date", {
                            dateFormat: "d-m-Y",
                            minDate: startDate,
                            maxDate: endDate,
                            defaultDate: defaultDate
                        });

                        $('#Actions').val(response.Action)

                        $('#goal-message').val(response.Notes);
                    } else {
                        $('#TimeBasedTimesheet_TimesheetId').val(response.TimesheetId)

                        $('#time-mission option').each(function () {
                            if ($(this).val() == response.MissionId) {
                                $(this).prop('selected', true);
                            }
                        });
                        $('#time-mission').prop(`disabled`, true);

                        var startDate = new Date(response.Mission.StartDate);
                        var currentDate = new Date();
                        var endDate = new Date(response.Mission.EndDate) > currentDate ? currentDate : new Date(response.Mission.EndDate);
                        var defaultDate = new Date(response.DateVolunteered);
                        $("#time-date").flatpickr().destroy();
                        var fp = flatpickr("#time-date", {
                            dateFormat: "d-m-Y",
                            minDate: startDate,
                            maxDate: endDate,
                            defaultDate: defaultDate
                        });

                        let time = response.Time;
                        let parts = time.split(":");
                        let hours = parseInt(parts[0]);
                        let minutes = parseInt(parts[1]);
                        //let hours = time.Substring(0, 2)
                        //let minutes = time.Substring(3, 2)
                        $('#Hours').val(hours)
                        $('#Minutes').val(minutes)

                        $('#time-message').val(response.Notes);
                    }
                },
                error: function (error) {

                }
            });
        }

    })

    $('#time-form').submit(function () {
        $('#time-mission').prop(`disabled`, false);
    })
    $('#goal-form').submit(function () {
        $('#goal-mission').prop(`disabled`, false);
    })

    $('table').on('click', '.delete-timesheet-btn', function () {
        let timesheetId = $(this).parent().find('input').val();
        deleteTableData('/TimeSheet/DeleteTimesheet', timesheetId);
    })

    $('.add-timesheet-btn').on('click', function () {
        let form1 = $('#goal-form')[0];
        let form2 = $('#time-form')[0];
        $('#time-mission').prop(`disabled`, false);
        $('#goal-mission').prop(`disabled`, false);

        form1.reset();
        form2.reset();
    })

    // admin panel

    $('.add-record-btn').click(function () {
        if (currentUrl.includes('AdminUser')) {
            addOrEdit('AddorEditUser');
        } else if (currentUrl.includes('AdminCMS')) {

            addOrEdit('AddorEditCMS');
        } else if (currentUrl.includes('AdminMissionPage')) {
            addOrEdit('AddorEditMission');
        } else if (currentUrl.includes('AdminMissionTheme')) {
            addOrEdit('AddorEditMissionTheme');
        } else if (currentUrl.includes('AdminMissionSkill')) {
            addOrEdit('AddorEditMissionSkill');
        }
        else if (currentUrl.includes('BannerManagement')) {
            addOrEdit('AddorEditBanner');
        }
    })

    let dataTable;
    function initializeDataTable(id) {
        dataTable = $(id).DataTable({
            lengthChange: false,
            paging: true,
            searching: true,
            pageLength: 10,
            info: false,
            pagingType: "full_numbers", // include "First" and "Last" buttons
            language: { // customize text of pagination controls
                paginate: {
                    first: '<img src="/images/previous.png" />',
                    last: '<img src="/images/next.png" />',
                    next: '<img src="/images/right-arrow1.png" />',
                    previous: '<img src="/images/left.png" />'
                }
            },
            columnDefs: [
                { "orderable": false, "targets": '_all' }
            ],
            order: false,
            drawCallback: function () {
                $('.paginate_button').removeClass('table-pagination-active');
                $('.paginate_button.current').addClass('table-pagination-active');
            }
        });
    }
    // user page
    if (currentUrl.includes('Admin')) {
        initializeDataTable('#admin-table');
        let navMenu = $('.vertical-nav-admin a');
        navMenu.each(function () {
            if (currentUrl.includes($(this).attr('href'))) {
                $(this).addClass('admin-nav-active')
            }
            else {
                $(this).removeClass('admin-nav-active')
            }
        })
        let pageInfo = dataTable.page.info();
        if (pageInfo.pages < 2) {
            $('.dataTables_paginate').hide();
        }
        else {
            $('.dataTables_paginate').show();
        }
        // filter data based on custom search input
        $('#custom-search').on('keyup', function () {
            dataTable.search($(this).val()).draw();
            var dataTableEmpty = $('.dataTables_empty');
            if (dataTableEmpty.length > 0 && dataTableEmpty.is(':visible')) {
                $('.dataTables_paginate').hide();
            }
            else {
                $('.dataTables_paginate').show();
            }
        });
        // Set active class to first page initially
        $('.paginate_button.current').addClass('table-pagination-active');

        // Add active class to clicked page
        $('.dataTables_paginate').on('click', '.paginate_button:not(.current, .first, .last, .previous, .next)', function () {
            $('.paginate_button').removeClass('table-pagination-active');
            $(this).addClass('table-pagination-active');
        });

        $('table').on('click', '.admin-delete-btn', function () {
            let Id = $(this).parent().find('input').val();
            if (currentUrl.includes('/Admin/AdminUser'))
                deleteTableData('/Admin/DeleteUser', Id);
            else if (currentUrl.includes('/Admin/AdminCMS'))
                deleteTableData('/Admin/DeleteCMS', Id);
            else if (currentUrl.includes('/Admin/AdminMissionPage'))
                deleteTableData('/Admin/DeleteMission', Id);
            else if (currentUrl.includes('/Admin/AdminMissionTheme'))
                deleteTableData('/Admin/DeleteMissionTheme', Id);
            else if (currentUrl.includes('/Admin/AdminMissionSkill'))
                deleteTableData('/Admin/DeleteMissionSkill', Id);
            else if (currentUrl.includes('/Admin/AdminStory'))
                deleteTableData('/Admin/DeleteStory', Id);
            else if (currentUrl.includes('/Admin/BannerManagement'))
                deleteTableData('/Admin/DeleteBanner', Id);


        })
    }

    var missionDocs = [];

    $(document).on('change', '#document-input', function () {
        var document = this;
        var $list = $(document).closest('.dropzone').siblings('#docs-list');
        var $existing = $list.children('.selected-file');
        var $new = $(document).prop('files');

        $.each($new, function (_, file) {
            if (!missionDocs.some(function (existing) { return existing.name === file.name })) {
                missionDocs.push(file);
                var $filename = $('<span class="filename">').text(file.name);
                var $remove = $('<span class="remove">').text('X').on('click', function () {
                    missionDocs.splice(missionDocs.indexOf(file), 1);
                    $(this).closest('.selected-file').remove();
                });
                var $selected = $('<div class="selected-file">').append($filename, $remove);
                $list.append($selected);
            }
        });

    });


    let imageDiv;
    let docDiv;
    function addOrEdit(page, id) {
        $.ajax({
            url: '/Admin/' + page,
            type: 'GET',
            data: { id: id == undefined ? 0 : id },
            success: async function (response) {
                $('.add-form-container').html(response);
                $('.admin-tables').remove();
                // Get today's date
                var today = new Date().toISOString().split('T')[0];
                // Set the minimum date of the input element to today's date
                if (id == undefined && $('#start-date') != undefined) {
                    $('#start-date').attr('min', today);
                }
                dropzone = $('#dropzone');
                imageList = $('#image-list');
                $('#GoalValue').parent().hide();
                $('#GoalObjectiveText').parent().hide();
                $('#TotalSeats').parent().hide

                if ($('#type').val() == "Goal") {
                    $('#GoalValue').parent().show();
                    $('#GoalObjectiveText').parent().show();
                    $('#TotalSeats').parent().hide();
                } else {
                    $('#GoalValue').parent().hide();
                    $('#GoalObjectiveText').parent().hide();
                    $('#TotalSeats').parent().show();
                }

                imageDiv = $('.image-item')
                docDiv = $('.selected-file')

                let imagePaths = $('.image-paths');
                let defaultImagePath = $('.default-image-path');
                if (defaultImagePath[0].value != '') {
                    const response = await fetch('/MissionImages/' + defaultImagePath[0].value);
                    const blob = await response.blob();
                    const file = new File([blob], defaultImagePath[0].value, { type: blob.type });
                    // Add the new file to the allfiles array
                    defaultImage = file;
                    allfiles.push(file);
                    var image = $('<img>').attr('src', '../MissionImages/' + defaultImagePath[0].value);
                    var closeIcon = $('<span>').addClass('close-icon').text('x');

                    // Add image and close icon to the list
                    var item = $('<div>').addClass('image-item default border border-secondary').append(image).append(closeIcon);
                    imageList.append(item);

                    closeIcon.on('click', function () {
                        item.remove();
                        allfiles.splice(allfiles.indexOf(file), 1);
                        if (file === defaultImage) {
                            defaultImage = null;
                        }
                    });

                    item.on('click', function () {
                        $('.image-item.default').removeClass('default border border-secondary');
                        $(this).addClass('default border border-secondary');
                        defaultImage = file;
                    });
                }
                if (imagePaths != null && imagePaths != undefined) {
                    for (let i = 0; i < imagePaths.length; i++) {
                        const response = await fetch('/MissionImages/' + imagePaths[i].value);
                        const blob = await response.blob();
                        const file = new File([blob], imagePaths[i].value, { type: blob.type });
                        // Add the new file to the allfiles array
                        allfiles.push(file);
                        var image = $('<img>').attr('src', '../MissionImages/' + imagePaths[i].value);
                        var closeIcon = $('<span>').addClass('close-icon').text('x');

                        // Add image and close icon to the list
                        var item = $('<div>').addClass('image-item').append(image).append(closeIcon);
                        imageList.append(item);

                        closeIcon.on('click', function () {
                            item.remove();
                            allfiles.splice(allfiles.indexOf(file), 1);
                            if (file === defaultImage) {
                                defaultImage = null;
                            }
                        });

                        item.on('click', function () {
                            $('.image-item.default').removeClass('default border border-secondary');
                            $(this).addClass('default border border-secondary');
                            defaultImage = file;
                        });
                    }
                }

                let docPaths = $('.doc-paths');
                var $list = $('#docs-list');
                if (docPaths != null && docPaths != undefined) {
                    for (let i = 0; i < docPaths.length; i++) {
                        var filename = docPaths[i].value;
                        if (filename.length > 15) {
                            filename = filename.substring(0, 6) + '...' + filename.substring(filename.length - 5);
                        }
                        var $filename = $('<span class="filename">').text(filename);
                        var $remove = $('<span class="remove">').text('X').on('click', function () {
                            missionDocs.splice(missionDocs.indexOf(file), 1);
                            $(this).closest('.selected-file').remove();
                        });
                        var $selected = $('<div class="selected-file">').append($filename, $remove);
                        $list.append($selected);
                        const response = await fetch('/MissionDocuments/' + docPaths[i].value);
                        const blob = await response.blob();
                        const file = new File([blob], docPaths[i].value, { type: blob.type });
                        missionDocs.push(file);
                    }
                }

            },
            error: function (error) {

            }
        });
    }

    let deleteTableData = (actionMethod, id) => {
        let deleteTr = $('.tr_' + id);
        $.ajax({
            url: actionMethod,
            type: 'GET',
            data: { id: id },
            success: function (response) {
                deleteTr.remove();
                swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: response,
                    showConfirmButton: false,
                    timer: 1500
                })
            },
            error: function (error) {
                swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: error,
                    showConfirmButton: false,
                    timer: 1500
                })
            }
        });
    }

    $(document).on('click', '#admin-cancel-btn', function () {
        location.reload();
    })

    $(document).on('change', '#start-date', function () {
        var startDate = $(this).val();
        $('#end-date').attr('min', startDate);
    });

    function missionFormValidation() {
        var editorDesc = tinymce.get('editor');
        editorDesc.on('change', function () {
            var text = editorDesc.getContent().trim();
            $('#descriptionValidation').text('')

        });
        var description = editorDesc.getContent().trim();
        if (description === '') {
            $('#descriptionValidation').text('Enter description for Mission')
            return false;
        }


        if ($('#tinymce').html() == '') {
            $('#descriptionValidation').text('Description is required')
            return false;
        }
        let youtubeUrls = $('#url').val().trim();

        var urlsArray = youtubeUrls.split('\n');
        var youtubeRegex = /^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$/i;
        if (youtubeUrls !== '') {

            for (var i = 0; i < urlsArray.length; i++) {
                if (!youtubeRegex.test(urlsArray[i])) {
                    $('#urlValidation').text('Invalid YouTube URL: ' + urlsArray[i]);
                    return false;
                }
            }
        }
        if (urlsArray.length > 20) {
            $('#urlValidation').text('You can not add more than 20 urls');
            return false;
        }
        return true;
    }

    $('.add-form-container').on('submit', '#mission-form', function (e) {
        e.preventDefault();
        e.stopPropagation();
        let isValid = missionFormValidation();
        if ($(this).valid() && isValid) {
            let formDetails = new FormData($('#mission-form')[0]);
            for (var i = 0; i < allfiles.length; i++) {
                if (defaultImage != allfiles[i])
                    formDetails.append('Files', allfiles[i]);
                else
                    formDetails.append('DefaultImage', allfiles[i]);
            }

            let selectedSkills = $('input[type="checkbox"][name="skillCheckboxes"]:checked').map(function () {
                return $(this).val();
            }).get();

            for (let i = 0; i < selectedSkills.length; i++) {
                formDetails.append('MissionSkills', selectedSkills[i]);
            }

            for (let i = 0; i < missionDocs.length; i++) {
                formDetails.append('MissionDocs', missionDocs[i]);
            }
            $.ajax({
                url: '/Admin/AdminMissionPage',
                type: 'POST',
                data: formDetails,
                processData: false,
                contentType: false,
                success: function (result) {
                    swal.fire({
                        position: 'center',
                        icon: result.icon,
                        title: result.message,
                        showConfirmButton: false,
                        timer: 3000
                    })
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
                },
                error: function (error) {

                }
            });
        }

    })

    $('.add-form-container').on('change', '#type', function () {
        if ($(this).val() == "Goal") {
            $('#GoalValue').parent().show();
            $('#GoalObjectiveText').parent().show();
            $('#TotalSeats').parent().hide();
            $('#TotalSeats').val('');
        } else {
            $('#GoalValue').parent().hide();
            $('#GoalValue').val('');
            $('#GoalObjectiveText').parent().hide();
            $('#GoalObjectiveText').val('');
            $('#TotalSeats').parent().show();
        }
    })

    $('.admin-tables').on('click', '#handle-application i', function () {
        let applicationId = $(this).parent().find('input').val()
        let storyId = $(this).parent().find('input').val()

        if ($(this).hasClass('approve-application'))
            handleApprovals('HandleMissionApplication', 1, applicationId)
        else if ($(this).hasClass('decline-application'))
            handleApprovals('HandleMissionApplication', 0, applicationId)
        else if ($(this).hasClass('approve-story'))
            handleApprovals('HandleStoryApproval', 1, storyId)
        else if ($(this).hasClass('decline-story'))
            handleApprovals('HandleStoryApproval', 0, storyId)
        else if ($(this).hasClass('approve-comment'))
            handleApprovals('HandleCommentApproval', 1, storyId)
        else if ($(this).hasClass('decline-comment'))
            handleApprovals('HandleCommentApproval', 0, storyId)
        else if ($(this).hasClass('approve-timesheet'))
            handleApprovals('HandleTimesheetApproval', 1, storyId)
        else if ($(this).hasClass('decline-timesheet'))
            handleApprovals('HandleTimesheetApproval', 0, storyId)
    })

    $('.sdbtns').on('click', 'button', function () {
        let storyId = $(this).parent().find('input').val()
        if ($(this).hasClass('approve-story')) {
            handleApprovals('HandleStoryApproval', 1, storyId)
            setTimeout(function () {
                location.reload();
                window.location.href = '/Admin/AdminStory';
            }, 3000);
        }
        else if ($(this).hasClass('decline-story')) {
            handleApprovals('HandleStoryApproval', 0, storyId)
            setTimeout(function () {
                location.reload();
                window.location.href = '/Admin/AdminStory';
            }, 3000);
        }
    })

    $(document).on('click', '.story-delete-btn', function () {
        let storyId = $(this).parent().find('input').val()
        debugger
        deleteTableData('/Admin/DeleteStory', storyId);
        setTimeout(function () {
            location.reload();
            window.location.href = '/Admin/AdminStory';
        }, 1500);
    })
    function handleApprovals(action, button, Id) {
        let deleteTr = $('.tr_' + Id);
        $.ajax({
            url: '/Admin/' + action,
            type: 'POST',
            data: { button: button, Id: Id },
            success: function (result) {
                deleteTr.remove();
                swal.fire({
                    position: 'center',
                    icon: result.icon,
                    title: result.message,
                    showConfirmButton: false,
                    timer: 3000
                })
            },
            error: function (error) {

            }
        });
    }

    // Update the image preview when a file is selected
    $(document).on('change', '#Image', function () {
        $('#banner-image-preview-on-edit').hide();
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                let preview = $("#banner-image-preview");
                preview.attr("src", e.target.result);
                preview.show();
            };
            reader.readAsDataURL(file);
        } else {
            $("#banner-image-preview").hide();
        }
    });

})