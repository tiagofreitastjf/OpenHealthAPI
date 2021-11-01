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
    public class ClinicaController : ControllerBase
    {
        private readonly OpenHealthContext _context;

        public ClinicaController(OpenHealthContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Buscar uma clinica pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Clinica</returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int? id)
        {
            try
            {
                if (!id.HasValue) throw new Exception("Id inválido");
                var clinica = this._context.Clinicas.Find(id.Value);
                if (clinica == null) return NotFound();

                var dto = new ClinicaDto()
                {
                    Id = clinica.Id,
                    Nome = clinica.Nome,
                    Cep = clinica.Cep,
                    Numero = clinica.Numero,
                    Bairro = clinica.Bairro,
                    Endereco = clinica.Endereco,
                    Complemento = clinica.Complemento,
                    Cidade = clinica.Cidade,
                    Estado = clinica.Estado,
                };

                return Ok(clinica);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Adiciona ou atualiza uma clinica
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddOrUpdate([FromBody] ClinicaDto dto)
        {
            try
            {
                Clinica clinica;

                if (dto.Id.HasValue) clinica = _context.Clinicas.Find(dto.Id.Value);
                else
                {
                    clinica = new Clinica();
                }

                clinica.Nome = dto.Nome;
                clinica.Cep = dto.Cep;
                clinica.Numero = dto.Numero.Value;
                clinica.Bairro = dto.Bairro;
                clinica.Endereco = dto.Endereco;
                clinica.Complemento = dto.Complemento;
                clinica.Cidade = dto.Cidade;
                clinica.Estado = dto.Estado;

                if (!dto.Id.HasValue) _context.Clinicas.Add(clinica);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Solicitar autororização ao paciente
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("SolicitarAutorizacao")]
        public IActionResult PostSolicitarAutorizacao([FromBody] ClinicaSolicaAutorizacaoDto dto)
        {
            try
            {
                var autorizacao = _context.ClienteAutorizaClinicas.FirstOrDefault(p => p.IdCliente == dto.IdCliente && p.IdClinica == dto.IdClinica);
                // caso já tiver sido autorizado em algum momento
                if (autorizacao != null && autorizacao.Autorizado == true) return Ok();

                var solicitacao = _context.ClinicaSolicitaAutorizacaos.FirstOrDefault(p => p.IdCliente == dto.IdCliente && p.IdClinica == dto.IdClinica && p.Pendente == true);
                if (solicitacao != null) throw new Exception("Já existe um pedido de autorização pendente.");

                solicitacao = new ClinicaSolicitaAutorizacao();
                solicitacao.IdCliente = dto.IdCliente.Value;
                solicitacao.IdClinica = dto.IdClinica.Value;
                solicitacao.Descricao = dto.Descricao;
                solicitacao.Pendente = true;

                _context.ClinicaSolicitaAutorizacaos.Add(solicitacao);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
