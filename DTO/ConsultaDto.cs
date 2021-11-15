using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenHealthAPI.DTO
{
    public class ConsultaDto
    {
        public int? Id { get; set; }
        [Required]
        public int? IdCliente { get; set; }
        [Required]
        public int? IdClinica { get; set; }
        [Required]
        public int? IdProfissional { get; set; }
        [Required]
        public DateTime? Data { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public string TipoConsulta { get; set; }
    }
}
