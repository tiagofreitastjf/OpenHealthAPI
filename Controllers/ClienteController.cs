using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenHealthAPI.DTO;
using OpenHealthAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenHealthAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly OpenHealthContext _context;

        public ClienteController(OpenHealthContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Buscar um cliente pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Cliente</returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int? id)
        {
            try
            {
                if (!id.HasValue) throw new Exception("Id inválido");
                var cliente = this._context.Clientes.Find(id.Value);
                if (cliente == null) return NotFound();

                var dto = new ClienteDto()
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    DataNascimento = cliente.DataNascimento,
                    Email = cliente.Email,
                    Cpf = cliente.Cpf,
                    Cep = cliente.Cep,
                    Numero = cliente.Numero,
                    Bairro = cliente.Bairro,
                    Endereco = cliente.Endereco,
                    Complemento = cliente.Complemento,
                    Cidade = cliente.Cidade,
                    Estado = cliente.Estado,
                };

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Adiciona ou atualiza um cliente
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddOrUpdate([FromBody] ClienteDto dto)
        {
            try
            {
                Cliente cliente;

                if (dto.Id.HasValue) cliente = _context.Clientes.Find(dto.Id.Value);
                else
                {
                    cliente = new Cliente();
                    // salvar a senha quando estiver criando um novo cliente, após isso alteração apenas por outro método
                    if (string.IsNullOrWhiteSpace(dto.Senha)) throw new Exception("Campo Senha inválido.");
                    cliente.Senha = dto.Senha;
                }

                cliente.Nome = dto.Nome;
                cliente.DataNascimento = dto.DataNascimento;
                cliente.Email = dto.Email;
                cliente.Senha = dto.Senha;
                cliente.Cpf = dto.Cpf;
                cliente.Cep = dto.Cep;
                cliente.Numero = dto.Numero.Value;
                cliente.Bairro = dto.Bairro;
                cliente.Endereco = dto.Endereco;
                cliente.Complemento = dto.Complemento;
                cliente.Cidade = dto.Cidade;
                cliente.Estado = dto.Estado;

                if (!dto.Id.HasValue) _context.Clientes.Add(cliente);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Fazer o login (temporário)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] ClienteLoginDto dto)
        {
            try
            {
                var cliente = _context.Clientes.FirstOrDefault(p => p.Email == dto.Email && p.Senha == dto.Senha);
                if (cliente == null) return Ok(new { UserNotFound = true });
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
