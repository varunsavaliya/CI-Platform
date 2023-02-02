let password = document.querySelector("#pwd");
let confirmPassword = document.querySelector("#re-pwd");

confirmPassword.addEventListener("blur", function () {
    if (password.value != confirmPassword.value) {
        alert("Please enter same value for both passwords");
    }
})


