// Close button functionality for every pills
var closeButtons = document.querySelectorAll('.pill .close');
var closeAllButton = document.getElementsByClassName('closeAll');
for (var i = 0; i < closeButtons.length; i++) {
    closeButtons[i].addEventListener('click', function () {
        console.log("hii")
        //e.preventDefault();
        //console.log(this.parentNode);

        //this.parentNode.style.display = 'none';
        this.parentNode.remove();
        console.log(closeButtons.length);
    });
}
// Close button functionality for close all text
closeAllButton[0].addEventListener('click', function () {
    for (var i = 0; i < closeButtons.length; i++) {
        closeButtons[i].parentNode.style.display = 'none';
    }
    this.style.display = "none";
})

