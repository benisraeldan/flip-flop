
// Get the modal
var modal = document.getElementById('Modal');

// Get the button that opens the modal
var btn = document.getElementById('myBtn');

// Get the <span> element that closes the modal
var span = document.getElementById('close');

Helper = function (value) {
    modal.style.display = value;
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

 
    //$(document).ready(function () {
    //        var subCat = this;
    //        $.ajax({
    //            url: "plaintickets/ReturnJsonSubCategories/?categoryId=" + 3,
    //            type: "GET",
    //            contentType: "application/json; charset=utf-8",
    //            datatype: JSON,
    //            success: function (result) {
    //                var categories = "";
    //                $(result).each(function () {
    //                    categories = categories + '<option value="' + this.subCategoryId + '">' + this.name + '</option>'
    //                });

    //                var subCateList = $(".selectSubCategroy");
    //                subCateList.empty();
    //                subCateList.append(categories);
    //            },
    //            error: function (data) {
    //                return "Error";
    //            }
    //        });
    //});