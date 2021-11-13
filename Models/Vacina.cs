using System;

namespace OpenHealthAPI.Models
{
    public class Vacina
    {
        public int id { get; set; }
        public int idCliente { get; set; }
        public int idClinica { get; set; }
        public int? idProfissional { get; set; }
        public DateTime Data { get; set; }
        public string TipoVacina { get; set; }
        public string Observacao { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Clinica Clinica { get; set; }
        public virtual Profissional Profissional { get; set; }
    }
}
