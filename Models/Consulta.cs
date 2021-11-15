using System;
using System.Collections.Generic;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class Consulta
    {
        public DateTime Data { get; set; }
        public string TipoConsulta { get; set; }
        public string Descricao { get; set; }
    }
}
