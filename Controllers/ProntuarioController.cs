using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenHealthAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
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
                    .Include(p => p.Vacinas)
                    .Include(p => p.ClinicaSolicitaAutorizacaos)
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
                        p.Id,
                        p.TipoConsulta,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Descricao
                    }),
                    vacina = cliente.Vacinas.Select(p => new {
                        p.Id,
                        p.TipoVacina,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Observacao
                    }),
                    solicitacoes = cliente.ClinicaSolicitaAutorizacaos.Select(p => new {
                        p.Id,
                        Titulo = "Solicitação de acesso",
                        p.Descricao,
                        p.Pendente,
                        p.IdClinica
                    }).Where(p => p.Pendente == true)
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
                Profissional profissional = _context.Profissionals
                    .Include(p => p.Consulta)
                    .Include(p => p.Vacinas)
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
                        p.Id,
                        p.TipoConsulta,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Descricao
                    }),
                    vacina = profissional.Vacinas.Select(p => new {
                        p.Id,
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
        
        [HttpGet("SolicitarAcesso")]
        public IActionResult SolicitarAcesso([Required] string CPF, [Required] int idProfissional)
        {
            try
            {
                Cliente cliente = _context.Clientes.FirstOrDefault(p => p.Cpf == CPF);
                Profissional profissional = _context.Profissionals.FirstOrDefault(p => p.Id == idProfissional);

                if (cliente == null || profissional == null)
                {
                    return BadRequest("Solicitação recusada por falta de informação.");
                }

                ClinicaSolicitaAutorizacao autorizacao = new ClinicaSolicitaAutorizacao();
                autorizacao.IdCliente = cliente.Id;
                autorizacao.IdClinica = profissional.IdClinica;
                autorizacao.Pendente = true;
                autorizacao.Descricao = $"O médico {profissional.Nome} solicita acesso ao seu prontuário.";

                _context.ClinicaSolicitaAutorizacaos.Add(autorizacao);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        [HttpGet("AcessoProntuario")]
        public IActionResult AcessoProntuario([Required] int id, [Required] bool permitir, [Required] int idCliente, [Required] int idClinica)
        {
            try
            {
                ClinicaSolicitaAutorizacao autorizacao = _context.ClinicaSolicitaAutorizacaos.FirstOrDefault(p => p.Id == id);

                if (autorizacao == null)
                {
                    return BadRequest("Solicitação não encontrada.");
                }

                autorizacao.Pendente = false;

                if (permitir)
                {
                    ClienteAutorizaClinica autoriza = new ClienteAutorizaClinica();
                    autoriza.IdCliente = idCliente;
                    autoriza.IdClinica = idClinica;
                    autoriza.Autorizado = permitir;

                    _context.ClienteAutorizaClinicas.Add(autoriza);
                    _context.SaveChanges();
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
