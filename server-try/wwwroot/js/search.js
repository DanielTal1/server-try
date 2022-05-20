$(function () {
    $('form').submit(e => {
        e.preventDefault();

        const q = $('#search').val().toLowerCase();
        //$('#search-div').load('/Review/Index?query=' + q);
        $("#search-div ").each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(q) > -1)
        });
    })
});
