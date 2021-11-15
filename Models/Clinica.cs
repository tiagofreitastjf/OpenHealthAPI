using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class Clinica
    {
        public Clinica()
        {
            ClienteAutorizaClinicas = new HashSet<ClienteAutorizaClinica>();
            Clientes = new HashSet<Cliente>();
            ClinicaSolicitaAutorizacaos = new HashSet<ClinicaSolicitaAutorizacao>();
            Consulta = new HashSet<Consulta>();
            Exames = new HashSet<Exame>();
            Profissionals = new HashSet<Profissional>();
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

        public virtual ICollection<ClienteAutorizaClinica> ClienteAutorizaClinicas { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<ClinicaSolicitaAutorizacao> ClinicaSolicitaAutorizacaos { get; set; }
        public virtual ICollection<Consulta> Consulta { get; set; }
        public virtual ICollection<Exame> Exames { get; set; }
        public virtual ICollection<Profissional> Profissionals { get; set; }
        public virtual ICollection<Vacina> Vacinas { get; set; }
    }
}
