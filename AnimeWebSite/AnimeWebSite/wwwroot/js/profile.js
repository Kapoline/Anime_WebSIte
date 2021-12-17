    $.ajax({
        type: "GET",
        url: "/Account/TestHandler",
        headers: {
            "XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function() {
        }
    });

$('#edit-btn').click(function (){
    uploadImage()
    $.ajax({
        type: "POST",
        url: "/Account/OnPostEditProfile",
        headers: {
            "XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: $('#profile-edit-form').serialize(),
        //data: {birthday: "${birthday}", sex: "fem", city: "${city}", description: "${description}"},
        success: function() {
            alert("Изменения сохранены")
            window.location.replace("/account/userprofile");
        }
    });
});

function uploadImage(){
    var files = $('#file').prop("files");
    var url = "/Account/OnPostUploadPhoto";
    formData = new FormData();
    formData.append("uploadedFile", files[0]);

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
        }
    });
};