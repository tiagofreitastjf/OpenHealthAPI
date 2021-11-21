using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class Profissional
    {
        public Profissional()
        {
            Consulta = new HashSet<Consulta>();
            Vacinas = new HashSet<Vacina>();
            Autorizacao = new HashSet<Autorizacao>();
            Agenda = new HashSet<Agenda>();
        }

        public int Id { get; set; }
        public int IdClinica { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string Cep { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public virtual Clinica IdClinicaNavigation { get; set; }
        public virtual ICollection<Consulta> Consulta { get; set; }
        public virtual ICollection<Vacina> Vacinas { get; set; }
        public virtual ICollection<Autorizacao> Autorizacao { get; set; }
        public virtual ICollection<Agenda> Agenda { get; set; }
    }
}
