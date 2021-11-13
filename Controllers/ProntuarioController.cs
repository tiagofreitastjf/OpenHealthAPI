using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenHealthAPI.Models;
using System;
using System.Linq;

namespace OpenHealthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProntuarioController : ControllerBase
    {
        private readonly OpenHealthContext _context;

        public ProntuarioController(OpenHealthContext context)
        {
            this._context = context;
        }

        [HttpGet("GetProntuarioCliente")]
        public IActionResult GetProntuarioCliente(int idCliente)
        {
            try
            {
                Cliente cliente = _context.Clientes
                    .Include(p => p.Consulta)
                    .Include(p => p.Vacina)
                    .FirstOrDefault(p => p.Id == idCliente);

                if (cliente == null)
                {
                    return BadRequest("Cliente não encontrado.");
                }

                return Ok(new 
                {
                    id = cliente.Id,
                    nome = cliente.Nome,
                    dataNascimento = cliente.DataNascimento.ToString("dd/MM/yyyy"),
                    consulta = cliente.Consulta.Select(p => new {
                        p.TipoConsulta,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Descricao
                    }),
                    vacina = cliente.Vacina.Select(p => new {
                        p.TipoVacina,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Observacao
                    }),
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetProntuarioProfissional")]
        public IActionResult GetProntuarioProfissional(int idProfissional)
        {
            try
            {
                Profissional profissional = _context.Profissional
                    .Include(p => p.Consulta)
                    .Include(p => p.Vacina)
                    .FirstOrDefault(p => p.Id == idProfissional);

                if (profissional == null)
                {
                    return BadRequest("Profissional não encontrado.");
                }

                return Ok(new
                {
                    id = profissional.Id,
                    nome = profissional.Nome,
                    dataNascimento = profissional.DataNascimento.ToString("dd/MM/yyyy"),
                    consulta = profissional.Consulta.Select(p => new {
                        p.TipoConsulta,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Descricao
                    }),
                    vacina = profissional.Vacina.Select(p => new {
                        p.TipoVacina,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Observacao
                    }),
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
