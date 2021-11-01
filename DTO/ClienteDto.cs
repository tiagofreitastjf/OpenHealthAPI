using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenHealthAPI.DTO
{
    public class ClienteDto
    {
        public int? Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public string Email { get; set; }
        public string Senha { get; set; }
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        public int? Numero { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        public string Endereco { get; set; }
        [Required]
        public string Complemento { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string Estado { get; set; }
    }

    public class ClienteLoginDto
    {
        [Required]
        public string Email { get; set; }
        //[Required]
        public string CPF { get; set; }
        [Required]
        public string Senha { get; set; }
    }

}
