using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenHealthAPI.DTO;
using OpenHealthAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenHealthAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ExameController : ControllerBase
    {
        private readonly OpenHealthContext _context;

        public ExameController(OpenHealthContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Buscar um exame pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Exame</returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int? id)
        {
            try
            {
                if (!id.HasValue) throw new Exception("Id inválido");
                var exame = this._context.Exames.Find(id.Value);
                if (exame == null) return NotFound();

                var dto = new ExameDto()
                {
                    Id = exame.Id,
                    IdCliente = exame.IdCliente,
                    IdClinica = exame.IdClinica,
                    Data = exame.Data,
                    Observacao = exame.Observacao,
                    ArquivoBase64 = exame.ArquivoBase64,
                    NomeArquivo = exame.NomeArquivo,
                };

                return Ok(exame);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Adiciona ou atualiza um exame
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddOrUpdate([FromBody] ExameDto dto)
        {
            try
            {
                Exame exame;

                if (dto.Id.HasValue) exame = _context.Exames.Find(dto.Id.Value);
                else
                {
                    exame = new Exame();
                    exame.IdCliente = dto.IdCliente.Value;
                    exame.IdClinica = dto.IdClinica.Value;
                }

                exame.Data = dto.Data.Value;
                exame.Observacao = dto.Observacao;
                exame.ArquivoBase64 = dto.ArquivoBase64;
                exame.NomeArquivo = dto.NomeArquivo;

                if (!dto.Id.HasValue) _context.Exames.Add(exame);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Pegar todos exames do cliente compartilhados com a clinica
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="idClinica"></param>
        /// <returns></returns>
        [HttpGet("Clinica")]
        public IActionResult GetTodos([FromQuery, Required] int? idCliente, [FromQuery, Required] int? idClinica)
        {
            try
            {
                // verificar se a clinica é autorizada.
               var clienteAutorizaClinica = _context.Autorizacao.FirstOrDefault(p => p.idCliente == idCliente && p.idClinica == idClinica);
                if (clienteAutorizaClinica == null || clienteAutorizaClinica.Autorizado == false) return Ok(new {
                    Mensagem = "Clinica não autorizada."
                });

                List<Exame> exames = _context.Exames.Where(p => p.IdCliente == idCliente).ToList();
                var examesDto = new List<ExameDto>();

                foreach (var exame in exames)
                {
                    examesDto.Add(new ExameDto
                    {
                        Id = exame.Id,
                        Data = exame.Data,
                        ArquivoBase64 = exame.ArquivoBase64,
                        IdCliente = exame.IdCliente,
                        IdClinica = exame.IdClinica,
                        Observacao = exame.Observacao,
                        NomeArquivo = exame.NomeArquivo
                    });
                }

                return Ok(examesDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Pegar todos exames do cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="idClinica"></param>
        /// <returns></returns>
        [HttpGet("Cliente")]
        public IActionResult GetTodos([FromQuery, Required] int? idCliente)
        {
            try
            {
                List<Exame> exames = _context.Exames.Where(p => p.IdCliente == idCliente).ToList();
                var examesDto = new List<ExameDto>();

                foreach (var exame in exames)
                {
                    examesDto.Add(new ExameDto
                    {
                        Id = exame.Id,
                        Data = exame.Data,
                        ArquivoBase64 = exame.ArquivoBase64,
                        IdCliente = exame.IdCliente,
                        IdClinica = exame.IdClinica,
                        Observacao = exame.Observacao,
                        NomeArquivo = exame.NomeArquivo
                    });
                }

                return Ok(examesDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
