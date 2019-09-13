
function ConsultaProfessores() {

    $.ajax({
        url: '/Home/ConsultaProfessores',
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            $.each(data, function (i, item) {
                $('<option value="' + item.Id + '">' + item.Nome + '</option>').appendTo('#ProfessorId');
            });
        },
        error: function () {
            console.log('err');
        }
    });
}