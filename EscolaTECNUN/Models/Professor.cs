using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EscolaTECNUN.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public DateTime DataNasc { get; set; }

        public string CPF { get; set; }

        public string Telefone { get; set; }

    }
}