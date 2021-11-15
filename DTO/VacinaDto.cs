using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenHealthAPI.DTO
{
    public class VacinaDto
    {
        public int? Id { get; set; }
        [Required]
        public int? IdCliente { get; set; }
        [Required]
        public int? IdProfissional { get; set; }
        [Required]
        public int? IdClinica { get; set; }
        [Required]
        public DateTime? Data { get; set; }
        public string Observacao { get; set; }
        [Required]
        public string TipoVacina { get; set; }
    }
}
