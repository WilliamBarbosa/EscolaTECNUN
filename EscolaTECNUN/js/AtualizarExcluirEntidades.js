$(document).ready(function () { 

    (function ($) {
        $.fn.serializeFormJSON = function () {

            var o = {};
            var a = this.serializeArray();
            $.each(a, function () {
                if (o[this.name]) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        };
    })(jQuery);

    $(".EditarTurma").click(function () {
    
        var NumTurma = this.id;

        $.ajax({
            url: '/Home/ConsultaTurma/' ,
            type: 'POST',
            dataType: 'json',
            data: { NumTurma: NumTurma },
            success: function (data) {

                $("#editarTurmaModal .modal-title").text("Turma " + NumTurma);

                $("#editarTurmaModal .NumTurma").val(NumTurma);

                var dt = new Date(parseInt(data.DataTurma.replace(/(^.*\()|([+-].*$)/g, ''))).toISOString().substring(0, 10);

                $("#editarTurmaModal .DataTurma").val(dt);

                $("#editarTurmaModal .Periodo").val(data.Periodo);

                $("#editarTurmaModal .Horario").val(data.Horario);

                $("#editarTurmaModal .ProfessorId").val(data.ProfessorId);

                $('#editarTurmaModal').modal('show');

            
            },
            error: function () {
                console.log('err');
            }
        });
    
    });




    $('#AtualizarTurma').click(function () {
        var jsonText = JSON.parse(JSON.stringify($('#editarTurmaForm').serializeFormJSON()));
        $.ajax({
            type: "POST",
            url: '/Home/AtualizarTurma/',
            data: jsonText,
            dataType: 'json',

            //if received a response from the server
            success: function (response) {
                alert(response.Mensagem);
                $('#editarTurmaModal').modal('hide');

                setInterval(function () { location.reload(); }, 1000);


            },
            error: function (response) {
                console.log(response.Mensagem);
            }

        });
    });

    $(".ExcluirTurma").click(function () {

        var NumTurma = this.id;

        $("#ExcluirTurmaModal .modal-title").text("Turma " + NumTurma);

        $("#ExcluirTurmaModal .RemoverTurma").attr("id", NumTurma);

        $("#ExcluirTurmaModal .modal-body").prepend('<strong>Deseja mesmo excluir a Turma ' + NumTurma + ' ?</strong>');

        $('#ExcluirTurmaModal').modal('show');

    });


    $(".RemoverTurma").click(function () {

        var NumTurma = this.id;

        $.ajax({
            url: '/Home/RemoverTurma/',
            type: 'POST',
            dataType: 'json',
            data: { NumTurma: NumTurma },
            success: function (response) {

                alert(response.Mensagem);
                $('#ExcluirTurmaModal').modal('hide');

                setInterval(function () { location.reload(); }, 1000);
            },
            error: function (response) {
                console.log(response.Mensagem);
            }
        });

    });

}); 