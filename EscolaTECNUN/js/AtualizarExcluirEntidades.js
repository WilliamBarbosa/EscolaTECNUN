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


    $(".EditarAluno").click(function () {

        var Id = this.id;

        $.ajax({
            url: '/Home/ConsultaAluno/',
            type: 'POST',
            dataType: 'json',
            data: { CodAluno: Id },
            success: function (data) {

                $("#editarAlunoModal .modal-title").text("Editar Aluno");

                $("#editarAlunoModal .Id").val(Id);

                $("#editarAlunoModal .Nome").val(data.Nome);

                var dt = new Date(parseInt(data.DataNasc.replace(/(^.*\()|([+-].*$)/g, ''))).toISOString().substring(0, 10);

                $("#editarAlunoModal .DataNasc").val(dt);

                $("#editarAlunoModal .CPF").val(data.CPF);

                $("#editarAlunoModal .Telefone").val(data.Telefone);

                $("#editarAlunoModal .Email").val(data.Email);

                $("#editarAlunoModal .InfoAdic").val(data.InfoAdic);

                $('#editarAlunoModal').modal('show');


            },
            error: function () {
                console.log('err');
            }
        });

    });

    $('#AtualizarAluno').click(function () {
        var jsonText = JSON.parse(JSON.stringify($('#editarAlunoForm').serializeFormJSON()));
        $.ajax({
            type: "POST",
            url: '/Home/AtualizarAluno/',
            data: jsonText,
            dataType: 'json',

            //if received a response from the server
            success: function (response) {
                alert(response.Mensagem);
                $('#editarAlunoModal').modal('hide');

                setInterval(function () { location.reload(); }, 1000);


            },
            error: function (response) {
                console.log(response.Mensagem);
            }

        });
    });

    $(".ExcluirAluno").click(function () {

        var Id = this.id;

        var NumTurma = $(this).attr('data-turma');

        $("#ExcluirAlunoModal .modal-title").text("Aluno " + Id);

        $("#ExcluirAlunoModal .RemoverAluno").attr("id", Id);

        $("#ExcluirAlunoModal .RemoverAluno").attr("data-turma", NumTurma);

        $("#ExcluirAlunoModal .modal-body").prepend('<strong>Deseja mesmo excluir o Aluno ' + Id + ' ?</strong>');

        $('#ExcluirAlunoModal').modal('show');

    });

    $(".RemoverAluno").click(function () {

        var Id = this.id;
        var NumTurma = $(this).attr('data-turma');

        $.ajax({
            url: '/Home/RemoverAluno/',
            type: 'POST',
            dataType: 'json',
            data: { Id: Id, NumTurma: NumTurma },
            success: function (response) {

                alert(response.Mensagem);
                $('#ExcluirAlunoModal').modal('hide');

                setInterval(function () { location.reload(); }, 1000);
            },
            error: function (response) {
                console.log(response.Mensagem);
            }
        });

    });

}); 