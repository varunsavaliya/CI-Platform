////const { type } = require("jquery");

$(document).ready(function () {
    $('#loginModal').modal('show');
    // declared globally because we have to use it in another function
    var currentUrl = window.location.href;
    let selectedCountry = null;
    let selectedSortOption = null;
    if (currentUrl.includes("LandingPage")) {
        getMission()
    } else if (currentUrl.includes("StoriesListing")) {
        getStory()
    }
    $(".search-field input").keyup(function () {
        if (currentUrl.includes("LandingPage")) {
            getMission()
        } else if (currentUrl.includes("StoriesListing")) {
            getStory()
        }
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
        });
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
                    if (currentUrl.includes("LandingPage")) {
                        getMission()
                    } else if (currentUrl.includes("StoriesListing")) {
                        getStory()
                    }
                });

                // Add "Close All" button
                if (closeAllButton.length === 0) {
                    searchedFilters.append('<div class="pill closeAll">Close All</div>');
                    searchedFilters.children('.closeAll').click(function () {
                        allDropdowns.find('input[type="checkbox"]').prop('checked', false);
                        searchedFilters.empty();
                        if (currentUrl.includes("LandingPage")) {
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
            getMission()
        } else if (currentUrl.includes("StoriesListing")) {
            getStory()
        }

    });

    // for all dropdown
    allDropdowns.each(function () {
        $(this).on('change', 'input[type="checkbox"]', function (e) {
            if (currentUrl.includes("LandingPage")) {
                getMission()
            } else if (currentUrl.includes("StoriesListing")) {
                getStory()
            }
        });
    });

    $('#sortByDropdown li').on('click', function () {
        selectedSortOption = $(this).find('a').text();
        if (currentUrl.includes("LandingPage")) {
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
            data: { missionId: missionId, userId: userId, rating: index },
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
            data: {
                MissionId: missionId, UserId: userId
            },
            success: function (response) {
                $(".apply-btn").text("Applied").addClass('published-btn').removeClass('card-btn');
            },
            error: function (error) {

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
                    getMission(pageNo)
                }));
                $('.totalMissions').text(totalRecords + ' Missions')
            },
            error: function (error) {

            }
        });
    }

    // function for stories with search, filter, pagination
    function getStory(pageNo) {
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
        let pageSize = 3;

        let inputData = {
            selectedCountry: selectedCountry !== "" ? selectedCountry : null,
            selectedCities: selectedCities !== "" ? selectedCities.join() : null,
            selectedThemes: selectedThemes !== "" ? selectedThemes.join() : null,
            selectedSkills: selectedSkills !== "" ? selectedSkills.join() : null,
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
                    getStory(pageNo)
                }));
            }
        })
    }

    // add comment in mission volunteering page
    $('#comment-form').submit(function (e) {
        e.preventDefault()
        var formData = $(this).serialize();
        $.ajax({
            url: '/Mission/AddComment',
            type: 'POST',
            data: formData,
            success: function (response) {
                $('.comment-box').empty()
            },
            error: function (error) {

            }
        })
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

    $(document).on('keyup', '.note-editable', function () {
        var form = $('.StoryForm');
        var actualTextBox = form.find('#actual_text');
        var text = $(this).html();
        actualTextBox.val(text);
    })

    // drag and drop images in share your story page
    var allfiles = [];
    var fileInput = document.getElementById('actual-file-input');
    var fileList;
    function handleFiles(e) {

        // Add dropped images or selected images to the list
        var files = e.target.files || e.originalEvent.dataTransfer.files;

        // Add selected images to the list
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var reader = new FileReader();
            allfiles.push(files[i]);
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
                        console.log(file);
                        console.log(allfiles);
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
    var dropzone = $('#dropzone');
    var imageList = $('#image-list');

    // Handle file drop event
    dropzone.on('drop', function (e) {
        e.preventDefault();
        e.stopPropagation();

        // Remove dropzone highlight
        dropzone.removeClass('dragover');
        $('.note-dropzone').remove();
        //$('.note-dropzone-message').remove();
        handleFiles(e);
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
    $('#actual-file-input').on('change', function (e) {
        handleFiles(e);
    });

    $('#editor').summernote({
        height: 200, // set the height of the editor
        toolbar: [
            // add formatting options to the toolbar
            ['style', ['bold', 'italic', 'strikethrough', 'subscript', 'superscript', 'underline']]
        ]
    });
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
            success: function (result) {
                console.log(result)
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
                    $('#storyTitle').val(result.title);
                    $('.note-editable').html(result.description);
                    
                    storyTitle = $('#storyTitle').val();
                    story = $('.note-editable').html();
                    // adding all urls in url box
                    let url = '';
                    for (var i in result.storyMedia) {
                        if (result.storyMedia.hasOwnProperty(i)) {
                            var obj = result.storyMedia[i];
                            if (obj.type === "VIDEO") {
                                url += obj.path + `\n`;
                            }
                            else {
                                // Create a new File object from the image path
                                var blob = new Blob([obj.path], { type: 'image/png' });
                                var file = new File([blob], obj.path, { type: 'image/png' });

                                // Add the new file to the allfiles array
                                allfiles.push(file);
                                var image = $('<img>').attr('src', '../Upload/' + obj.path);
                                var closeIcon = $('<span>').addClass('close-icon').text('x');

                                // Add image and close icon to the list
                                var item = $('<div>').addClass('image-item').append(image).append(closeIcon);
                                imageList.append(item);

                                // Handle close icon click event
                                closeIcon.on('click', function () {
                                    $(this).parent().remove();
                                    allfiles.splice(allfiles.indexOf(file), 1);
                                    console.log(allfiles);
                                });

                            }
                        }
                    }
                    $('#url').val(url);
                    $('#preview-btn').val(result.storyId)
                }
            },
            error: function (error) {

            }
        });

    });
    

    function validateForm(){
        if (missionId === '' || missionId === undefined) {
            $('#selectMissionValidation').text('Select the mission to write a story')
            return false;
        }
        if (storyTitle === '' || storyTitle === undefined) {
            $('#storyTitleValidation').text('Title is required')
            return false;
        }
        if (story === '' || story === undefined || story.split(' ').length < 100) {
            $('.note-editor').addClass('mb-1');
            $('#storyValidation').text('Minimum 100 words are required')
            return false;
        }

        let youtubeUrls = $('#url').val().trim();

        // Split the input into an array of URLs
        var urlsArray = youtubeUrls.split('\n');
        // Check each URL for validity
        var youtubeRegex = /^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$/i;
        for (var i = 0; i < urlsArray.length; i++) {
            if (!youtubeRegex.test(urlsArray[i])) {
                $('#urlValidation').text('Invalid YouTube URL: ' + urlsArray[i]);
                return false;
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
            console.log(formDetails)
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
                url: '/Story/SaveStory',
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
                data: {id : id},
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

        // Send new profile image to server
        //var formData = new FormData();
        //formData.append('profile_image', this.files[0]);
        //$.ajax({
        //    url: '/update_profile_image',
        //    type: 'POST',
        //    data: formData,
        //    processData: false,
        //    contentType: false,
        //    success: function (data) {
        //        // Update user's profile with new image
        //        // ...
        //    }
        //});
    });
})