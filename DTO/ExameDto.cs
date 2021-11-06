using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenHealthAPI.DTO
{
    public class ExameDto
    {
        public int? Id { get; set; }
        [Required]
        public int? IdCliente { get; set; }
        [Required]
        public int? IdClinica { get; set; }
        [Required]
        public DateTime? Data { get; set; }
        public string Observacao { get; set; }
        [Required]
        public string ArquivoBase64 { get; set; }
        [Required]
        public string NomeArquivo { get; set; }
    }
}
