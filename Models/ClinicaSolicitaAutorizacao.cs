using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class ClinicaSolicitaAutorizacao
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdClinica { get; set; }
        public string Descricao { get; set; }
        public string Token { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Clinica IdClinicaNavigation { get; set; }
    }
}
