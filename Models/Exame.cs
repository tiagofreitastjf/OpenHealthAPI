using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class Exame
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdClinica { get; set; }
        public DateTime Data { get; set; }
        public string Observacao { get; set; }
        public string ArquivoBase64 { get; set; }
        public string NomeArquivo { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Clinica IdClinicaNavigation { get; set; }
    }
}
