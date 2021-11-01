using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class Clinica
    {
        public Clinica()
        {
            Profissionals = new HashSet<Profissional>();
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

        public virtual ICollection<Profissional> Profissionals { get; set; }
    }
}
