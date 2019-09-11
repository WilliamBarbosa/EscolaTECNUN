using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EscolaTECNUN.Models
{
    public class Turma
    {
        public int Id { get; set; }
        public string NumTurma { get; set; }

        public DateTime DataTurma { get; set; }

        public string Periodo { get; set; }

        public string Horario { get; set; }

        public int ProfessorId { get; set; }

    }
}