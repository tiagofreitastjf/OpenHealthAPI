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
    public class ConsultaController : ControllerBase
    {
        private readonly OpenHealthContext _context;

        public ConsultaController(OpenHealthContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Buscar uma consulta pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Consulta</returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int? id)
        {
            try
            {
                if (!id.HasValue) throw new Exception("Id inválido");
                var consulta = this._context.Consulta.Find(id.Value);
                if (consulta == null) return NotFound();

                var dto = new ConsultaDto()
                {
                    Id = consulta.Id,
                    IdCliente = consulta.IdCliente,
                    IdClinica = consulta.IdClinica,
                    IdProfissional = consulta.IdProfissional,
                    Data = consulta.Data,
                    Descricao = consulta.Descricao,
                    TipoConsulta = consulta.TipoConsulta
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Adiciona ou atualiza uma consulta
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddOrUpdate([FromBody] ConsultaDto dto)
        {
            try
            {
                Consulta consulta;

                if (dto.Id.HasValue) consulta = _context.Consulta.Find(dto.Id.Value);
                else
                {
                    consulta = new Consulta();
                    consulta.IdCliente = dto.IdCliente.Value;
                    consulta.IdClinica = dto.IdClinica.Value;
                    consulta.IdProfissional = dto.IdProfissional.Value;
                }

                consulta.Data = dto.Data.Value;
                consulta.Descricao = dto.Descricao;
                consulta.TipoConsulta = dto.TipoConsulta;

                if (!dto.Id.HasValue) _context.Consulta.Add(consulta);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Pegar todas as consultas do compartilhadas com a clinica
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
                    Mensagem = "Profissional não autorizado."
                });


                List<Consulta> consultas = _context.Consulta.Where(p => p.IdCliente == idCliente).ToList();
                var consultasDto = new List<dynamic>();

                foreach (var consulta in consultas)
                {
                    consultasDto.Add(new
                    {
                        Id = consulta.Id,
                        Data = consulta.Data.ToString("dd/MM/yyyy"),
                        IdCliente = consulta.IdCliente,
                        IdClinica = consulta.IdClinica,
                        IdProfissional = consulta.IdProfissional,
                        Descricao = consulta.Descricao,
                        TipoConsulta = consulta.TipoConsulta
                    });
                }

                return Ok(consultasDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Pegar todas consultas do cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="idClinica"></param>
        /// <returns></returns>
        [HttpGet("Cliente")]
        public IActionResult GetTodos([FromQuery, Required] int? idCliente)
        {
            try
            {
                List<Consulta> consultas = _context.Consulta.Where(p => p.IdCliente == idCliente).ToList();
                var consultasDto = new List<ConsultaDto>();

                foreach (var consulta in consultas)
                {
                    consultasDto.Add(new ConsultaDto
                    {
                        Id = consulta.Id,
                        Data = consulta.Data,
                        IdCliente = consulta.IdCliente,
                        IdClinica = consulta.IdClinica,
                        IdProfissional = consulta.IdProfissional,
                        Descricao = consulta.Descricao,
                        TipoConsulta = consulta.TipoConsulta
                    });
                }

                return Ok(consultasDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
