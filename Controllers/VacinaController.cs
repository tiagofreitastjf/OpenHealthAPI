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
    public class VacinaController : ControllerBase
    {
        private readonly OpenHealthContext _context;

        public VacinaController(OpenHealthContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Buscar uma vacina pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Consulta</returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int? id)
        {
            try
            {
                if (!id.HasValue) throw new Exception("Id inválido");
                var exame = this._context.Vacinas.Find(id.Value);
                if (exame == null) return NotFound();

                var dto = new VacinaDto()
                {
                    Id = exame.Id,
                    IdCliente = exame.IdCliente,
                    IdClinica = exame.IdClinica,
                    IdProfissional = exame.IdProfissional,
                    Data = exame.Data,
                    Observacao = exame.Observacao,
                    TipoVacina = exame.TipoVacina
                };

                return Ok(exame);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Adiciona ou atualiza uma vacina
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddOrUpdate([FromBody] VacinaDto dto)
        {
            try
            {
                Vacina vacina;

                if (dto.Id.HasValue) vacina = _context.Vacinas.Find(dto.Id.Value);
                else
                {
                    vacina = new Vacina();
                    vacina.IdCliente = dto.IdCliente.Value;
                    vacina.IdClinica = dto.IdClinica.Value;
                    vacina.IdProfissional = dto.IdProfissional.Value;
                }

                vacina.Data = dto.Data.Value;
                vacina.Observacao = dto.Observacao;
                vacina.TipoVacina = dto.TipoVacina;

                if (!dto.Id.HasValue) _context.Vacinas.Add(vacina);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Pegar todas as vacinas do compartilhadas com a clinica
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="idClinica"></param>
        /// <returns></returns>
        [HttpGet("Clinica")]
        public IActionResult GetTodos([FromQuery, Required] int? idCliente, [FromQuery, Required] int? idProfissional)
        {
            try
            {
                // verificar se a clinica é autorizada.
               var clienteAutorizaClinica = _context.Autorizacao.FirstOrDefault(p => p.idCliente == idCliente && p.idProfissional == idProfissional);
                if (clienteAutorizaClinica == null || clienteAutorizaClinica.Autorizado == false) return Ok(new {
                    Erro = true,
                    Mensagem = "Clinica não autorizada."
                });

                List<Vacina> vacinas = _context.Vacinas.Where(p => p.IdCliente == idCliente).ToList();
                var vacinasDto = new List<dynamic>();

                foreach (var vacina in vacinas)
                {
                    vacinasDto.Add(new
                    {
                        Id = vacina.Id,
                        Data = vacina.Data.ToString("dd/MM/yyyy"),
                        IdCliente = vacina.IdCliente,
                        IdClinica = vacina.IdClinica,
                        IdProfissional = vacina.IdProfissional,
                        Observacao = vacina.Observacao,
                        TipoVacina = vacina.TipoVacina
                    });
                }

                return Ok(vacinasDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Pegar todas as vacinas do cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="idClinica"></param>
        /// <returns></returns>
        [HttpGet("Cliente")]
        public IActionResult GetTodos([FromQuery, Required] int? idCliente)
        {
            try
            {
                List<Vacina> vacinas = _context.Vacinas.Where(p => p.IdCliente == idCliente).ToList();
                var vacinasDto = new List<VacinaDto>();

                foreach (var vacina in vacinasDto)
                {
                    vacinasDto.Add(new VacinaDto
                    {
                        Id = vacina.Id,
                        Data = vacina.Data,
                        IdCliente = vacina.IdCliente,
                        IdClinica = vacina.IdClinica,
                        IdProfissional = vacina.IdProfissional,
                        Observacao = vacina.Observacao,
                        TipoVacina = vacina.TipoVacina
                    });
                }

                return Ok(vacinasDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
