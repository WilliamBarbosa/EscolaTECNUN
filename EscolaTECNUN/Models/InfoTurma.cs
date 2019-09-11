using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EscolaTECNUN.Models
{
    public class InfoTurma
    {
        public string NumTurma { get; set; }
        public string Periodo { get; set; }

        public string Professor { get; set; }

        public List<Aluno> Aluno { get; set; }
    }
}