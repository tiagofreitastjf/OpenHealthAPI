using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHealthAPI.Models
{
    public class Agenda
    {
        public int id { get; set; }
        public int idCliente { get; set; }
        public int idClinica { get; set; }
        public int idProfissional { get; set; }
        public DateTime Data { get; set; }
        [NotMapped]
        public TimeSpan Hora { get; set; }
        public bool Confirmado { get; set; }
        public bool Pendente { get; set; }
        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Clinica IdClinicaNavigation { get; set; }
        public virtual Profissional IdProfissionalNavigation { get; set; }
    }
}
