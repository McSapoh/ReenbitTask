function saveObject(formId) {
    // gettimg data from form
    event.preventDefault();
    const form = document.getElementById(formId)
    let formData = new FormData(form)
    let files = $("#file")[0].files;
    formData.append("File", files[0]);
    
    $.ajax({
        processData: false,
        contentType: false,
        type: 'POST',
        url: $("#" + formId).attr("action"),
        data: formData,
        // displaying result of the action.
        statusCode: {
            200: function () {
                toastr.success('Successfully saved, message sended to current email');
            },
            400: function () {
                toastr.error('File was not saved');
            },
            409: function () {
                toastr.error('Email is not valid');
            },
            500: function () {
                toastr.error('File was saved, but email was not send');
            }
        }
    })
}