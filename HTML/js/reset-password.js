let password = document.querySelector(".new-pass");
let confirmPassword = document.querySelector(".confirm-new-pass");

confirmPassword.addEventListener("blur", function () {
    if (password.value != confirmPassword.value) {
        alert("Please enter same value for both passwords");
    }
})