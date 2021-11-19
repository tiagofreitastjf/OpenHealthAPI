using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class Clinica
    {
        public Clinica()
        {
            Autorizacao = new HashSet<Autorizacao>();
            Consulta = new HashSet<Consulta>();
            Exames = new HashSet<Exame>();
            Profissional = new HashSet<Profissional>();
            Vacinas = new HashSet<Vacina>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cep { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public virtual ICollection<Autorizacao> Autorizacao { get; set; }
        public virtual ICollection<Consulta> Consulta { get; set; }
        public virtual ICollection<Exame> Exames { get; set; }
        public virtual ICollection<Profissional> Profissional { get; set; }
        public virtual ICollection<Vacina> Vacinas { get; set; }
    }
}
