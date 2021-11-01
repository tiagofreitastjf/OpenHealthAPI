using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            ClienteAutorizaClinicas = new HashSet<ClienteAutorizaClinica>();
            ClinicaSolicitaAutorizacaos = new HashSet<ClinicaSolicitaAutorizacao>();
            Exames = new HashSet<Exame>();
        }

        public int Id { get; set; }
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
        public string Token { get; set; }

        public virtual ICollection<ClienteAutorizaClinica> ClienteAutorizaClinicas { get; set; }
        public virtual ICollection<ClinicaSolicitaAutorizacao> ClinicaSolicitaAutorizacaos { get; set; }
        public virtual ICollection<Exame> Exames { get; set; }
    }
}
