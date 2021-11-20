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
                    .Include(p => p.Autorizacao)
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
                    solicitacoes = cliente.Autorizacao.Select(p => new {
                        p.id,
                        Titulo = "Solicitação de acesso",
                        p.Descricao,
                        p.Pendente,
                        p.idClinica,
                        p.idCliente,
                        p.idProfissional
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
                    .Include("Autorizacao.IdClienteNavigation")
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
                        p.IdCliente,
                        p.TipoConsulta,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Descricao
                    }).Take(3),
                    vacina = profissional.Vacinas.Select(p => new {
                        p.Id,
                        p.IdCliente,
                        p.TipoVacina,
                        data = p.Data.ToString("dd/MM/yyyy"),
                        p.Observacao
                    }).Take(3),
                    prontuarios = profissional.Autorizacao.Select(p => new 
                    {
                        p.id,
                        p.idCliente,
                        p.idClinica,
                        p.idProfissional,
                        p.Autorizado,
                        p.Descricao,
                        p.Pendente,
                        p.IdClienteNavigation.Nome
                    }).Where(p => p.Pendente == false && p.Autorizado == true)
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

                Autorizacao autorizacao = new Autorizacao();
                autorizacao.idCliente = cliente.Id;
                autorizacao.idClinica = profissional.IdClinica;
                autorizacao.idProfissional = profissional.Id;
                autorizacao.Pendente = true;
                autorizacao.Descricao = $"O médico {profissional.Nome} solicita acesso ao seu prontuário.";
                autorizacao.Autorizado = false;

                _context.Autorizacao.Add(autorizacao);
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
                Autorizacao autorizacao = _context.Autorizacao.FirstOrDefault(p => p.id == id);

                if (autorizacao == null)
                {
                    return BadRequest("Solicitação não encontrada.");
                }

                if (permitir)
                {
                    autorizacao.Pendente = false;
                    autorizacao.Autorizado = permitir;
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
