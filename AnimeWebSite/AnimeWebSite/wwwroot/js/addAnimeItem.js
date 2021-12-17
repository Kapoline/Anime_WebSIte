$('#add-item-btn').click(function (){
    $.ajax({
        type: "POST",
        url: "/AnimeItem/OnPostAddItem",
        headers: {
            "XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: $('#add-item-form').serialize(),
        success: function(data) {
            uloadImage(data)
        }
    });
});

function uloadImage(itemId){
    var files = $('#file').prop("files");
    var url = "/AnimeItem/OnPostUploadPhoto";
    formData = new FormData();
    formData.append("uploadedFile", files[0]);
    formData.append('itemId', itemId.toString())

    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data){
            $('#userPhoto').attr("src", data)
            alert("Добавлено")
        }
    });
}