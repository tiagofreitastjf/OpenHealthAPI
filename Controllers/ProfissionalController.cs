﻿using Microsoft.AspNetCore.Http;
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
    public class ProfissionalController : ControllerBase
    {
        private readonly OpenHealthContext _context;

        public ProfissionalController(OpenHealthContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Buscar um profissional pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Profissional</returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int? id)
        {
            try
            {
                if (!id.HasValue) throw new Exception("Id inválido");
                var profissional = this._context.Profissionals.Find(id.Value);
                if (profissional == null) return NotFound();

                var dto = new ProfissionalDto()
                {
                    Id = profissional.Id,
                    Nome = profissional.Nome,
                    DataNascimento = profissional.DataNascimento,
                    Email = profissional.Email,
                    Cpf = profissional.Cpf,
                    Cep = profissional.Cep,
                    Numero = profissional.Numero,
                    Bairro = profissional.Bairro,
                    Endereco = profissional.Endereco,
                    Complemento = profissional.Complemento,
                    Cidade = profissional.Cidade,
                    Estado = profissional.Estado,
                };

                return Ok(profissional);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Adiciona ou atualiza um profissional
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddOrUpdate([FromBody] ProfissionalDto dto)
        {
            try
            {
                Profissional profissional;

                if (dto.Id.HasValue) profissional = _context.Profissionals.Find(dto.Id.Value);
                else
                {
                    profissional = new Profissional();
                    // salvar a senha quando estiver criando um novo cliente, após isso alteração apenas por outro método
                    if (string.IsNullOrWhiteSpace(dto.Senha)) throw new Exception("Campo Senha inválido.");
                    profissional.Senha = dto.Senha;
                    
                    var clinica = _context.Clinicas.Find(dto.IdClinica.Value);
                    if (clinica == null) throw new Exception("Clinica inválida.");
                }

                profissional.Nome = dto.Nome;
                profissional.DataNascimento = dto.DataNascimento;
                profissional.Email = dto.Email;
                profissional.Senha = dto.Senha;
                profissional.Cpf = dto.Cpf;
                profissional.Cep = dto.Cep;
                profissional.Numero = dto.Numero.Value;
                profissional.Bairro = dto.Bairro;
                profissional.Endereco = dto.Endereco;
                profissional.Complemento = dto.Complemento;
                profissional.Cidade = dto.Cidade;
                profissional.Estado = dto.Estado;

                if (!dto.Id.HasValue) _context.Profissionals.Add(profissional);

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
                var cliente = _context.Profissionals.FirstOrDefault(p => p.Cpf == dto.CPF && p.Senha == dto.Senha);
                if (cliente == null) return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
