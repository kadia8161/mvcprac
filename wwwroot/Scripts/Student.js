function InitStudent() {

    $('#txtDateOfBirth').datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-50:+0",
        dateFormat: 'dd-M-yy'
    });
    $('#txtDateOfBirth').datepicker("setDate", new Date(Date.parse(birthDateStr)));
}

function save() {
    if (!$('#frmStudentDetails').valid())
        return;
    var bdate = $('#txtDateOfBirth').datepicker("getDate");
    var curDate = new Date();
    var diffTime = curDate - bdate;
    var diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    if ((diffDays / 365) < 8) {
        BootstrapDialog.show({
            title: 'Alert',
            message: 'Age Must be greater than 8 years'
        });
        return;
    }
    var model = [];
    var StudentId = 0;
    if ($('#StudentId').val() != '')
        StudentId = parseInt($('#StudentId').val());
    var selectedCourses = [];
    $('input[name="CourseSelected"]:checkbox').each(function (i, obj) {
        if ($(obj).is(':checked'))
            selectedCourses.push({
                CourseId: $(obj).val(),
                CourseName: $(obj).parents().eq(1).find('label').html()
            });
    });

    model = {
        StudentId: StudentId,
        Name: $('#txtName').val(),
        DateOfBirth: bdate.getFullYear() + '-' + (bdate.getMonth() + 1) + '-' + bdate.getDate(), //$('#txtDateOfBirth').val(),
        Gender: $('#Gender:checked').val(),
        SelectedCourses: selectedCourses
    }

    $.ajax({

        type: "POST",
        url: saveUrl,
        data: model,
        success: function (retval) {
            if (retval.valid) {
                BootstrapDialog.show({
                    title: 'Success',
                    message: 'Record Saved...!',
                    buttons: [{
                        label: 'Ok',
                        action: function (dialog) {
                            window.location = cancelUrl;
                        }
                    }]
                });

            } else {
                BootstrapDialog.show({
                    title: 'Error',
                    message: retval.msg
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            BootstrapDialog.show({
                title: 'Error',
                message: errorThrown
            });
        }
    });
}

function getURLParam(paraname) {
    var paravalue = '';
    strurl = window.location.href;
    if (strurl.split('?').length > 1) {
        var para = strurl.split('?')[1].split('&');
        for (let index = 0; index < para.length; index++) {
            if (para[index].split('=')[0] == paraname) {
                paravalue = para[index].split('=')[1];
                break;
            }
        }
    }
    return paravalue;
}

function pagechange() {
    var sortByProp = getURLParam('sortByProp');
    var sortDir = getURLParam('sortDir');
    window.location = refreshUrl + '?' + 'PageNo=' + $('#ddlPageNo').val() + '&sortByProp=' + sortByProp + '&sortDir=' + sortDir;
}

function cancel() {
    window.location = cancelUrl;
}

function deletedata(StudentId) {
    BootstrapDialog.confirm('Are you sure you want delete record ?', function (result) {
        if (result) {
            $.ajax({
                type: "POST",
                url: deleteUrl,
                data: { Id: StudentId },
                success: function (retval) {
                    if (retval.valid) {
                        BootstrapDialog.show({
                            title: 'Success',
                            message: 'Data Deleted',
                            buttons: [{
                                label: 'Ok',
                                action: function (dialog) {
                                    window.location = refreshUrl;
                                }
                            }]
                        });
                    } else {
                        BootstrapDialog.show({
                            title: 'Error',
                            message: retval.msg
                        });
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    BootstrapDialog.show({
                        title: 'Error',
                        message: errorThrown
                    });
                }
            });
        }
    });


}

