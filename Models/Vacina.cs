using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class Vacina
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdClinica { get; set; }
        public int? IdProfissional { get; set; }
        public DateTime Data { get; set; }
        public string TipoVacina { get; set; }
        public string Observacao { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Clinica IdClinicaNavigation { get; set; }
        public virtual Profissional IdProfissionalNavigation { get; set; }
    }
}
