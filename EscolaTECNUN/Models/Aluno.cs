using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EscolaTECNUN.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public DateTime DataNasc { get; set; }

        public string Telefone { get; set; }

        public string CPF { get; set; }

        public string Email { get; set; }
        public string InfoAdic { get; set; }
    }
}