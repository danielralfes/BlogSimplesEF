if (comLoading == undefined)
{ var comLoading = true; }

$(document).ready(function ()
{
    $(document).ajaxSend(function (e, jqXHR)
    {
        if (comLoading)
        { $("body").addClass("loadingAjax"); }
    });

    $(document).ajaxComplete(function (e, jqXHR)
    {
        $("body").removeClass("loadingAjax");
    });

    //Outro eventos ajax
    //$(document).ajaxStart(function () {
    //    console.log('ajaxStart');
    //}).ajaxStop(function () {
    //    console.log('ajaxStop');
    //}).ajaxError(function () {
    //    console.log('ajaxerror');
    //});
});