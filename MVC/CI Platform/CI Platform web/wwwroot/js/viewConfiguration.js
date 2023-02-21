let GridView = document.getElementById("GridView");
let ListView = document.getElementById("ListView");
let listBtn = document.getElementsByClassName("list");

window.addEventListener("resize", function () {
    if (window.outerWidth < 992) {
    if (ListView.classList.contains("active")) {
        ListView.classList.remove("active");
        GridView.classList.add("active");
    }
    }
    if (window.outerWidth > 991) {
        if (listBtn[0].classList.contains("active")) {
            GridView.classList.remove("active");
            ListView.classList.add("active");
        }
    }
})