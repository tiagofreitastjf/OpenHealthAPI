using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenHealthAPI.DTO;
using OpenHealthAPI.Models;
using OpenHealthAPI.Servicos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OpenHealthAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly OpenHealthContext _context;
        private readonly IEmailServico emailServico;

        public ClienteController(OpenHealthContext context, IEmailServico emailServico)
        {
            this._context = context;
            this.emailServico = emailServico;
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
                Cliente jaExite = _context.Clientes.FirstOrDefault(p => p.Email == dto.Email);
                if (jaExite != null)
                {
                    return BadRequest("Email já cadastrado.");
                }

                Cliente cliente;

                if (dto.Id.HasValue) cliente = _context.Clientes.Find(dto.Id.Value);
                else
                {
                    cliente = new Cliente();
                    // salvar a senha quando estiver criando um novo cliente, após isso alteração apenas por outro método
                    if (string.IsNullOrWhiteSpace(dto.Senha)) throw new Exception("Campo Senha inválido.");
                    cliente.Senha = dto.Senha;
                    // gerar token de 8 caracteres
                    //var caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    //cliente.Token = new string(Enumerable.Repeat(caracteres, 8).Select(s => s[(new Random()).Next(s.Length)]).ToArray());
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

        /// <summary>
        /// Responder uma solicitação de acesso
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        //[HttpPost("ResponderSolicitacao")]
        //public IActionResult PostClienteResponderSolicitacao([FromBody] ClienteResponderSolicitacaoDto dto)
        //{
        //    try
        //    {
        //        var solicitacao = _context.Autorizacao.Find(dto.IdSolicitacao.Value);
        //        if (solicitacao == null) throw new Exception("Solicitação inválida.");
        //        if (solicitacao.Pendente == false) throw new Exception("Está solicitação já foi respondida.");

        //        var jaTemAutorizacao = _context.Autorizacao.FirstOrDefault(p => p.idCliente == solicitacao.idCliente && p.idClinica == solicitacao.idClinica && p.Autorizado == true);
        //        if (jaTemAutorizacao != null) throw new Exception("Clinica já autorizada.");

        //        var cliente = _context.Clientes.FirstOrDefault(p => p.Id == solicitacao.idCliente);
        //        if (cliente == null) throw new Exception("Cliente inválido.");

        //        //if (cliente.Token != dto.Token) throw new Exception("Token inválido.");

        //        solicitacao.Pendente = false;
        //        _context.Autorizacao.Add(new Autorizacao
        //        {
        //            idCliente = cliente.Id,
        //            idClinica = solicitacao.idClinica,
        //            Autorizado = dto.Autorizado.Value
        //        });

        //        _context.SaveChanges();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        [HttpGet("RedefinirSenha")]
        public async Task<IActionResult> EnviarEmailRedefinirSenha(string email, string tipo)
        {
            try
            {
                Cliente cliente = _context.Clientes.FirstOrDefault(p => p.Email == email);

                if (cliente == null)
                {
                    return Ok(new { Erro = true, Mensagem = "Paciente não encontrado." });
                }

                EmailRequest request = new EmailRequest();
                request.ToEmail = cliente.Email;
                request.Subject = "Contato Open Health";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<table>");
                    sb.Append("<tr>");
                        sb.Append("<td>");
                            sb.Append("<div style='text-alingment'>");
                                sb.Append("<p>Redefinição de senha da sua conta Open Health.</p>");
                            sb.Append("</div>");
                        sb.Append("</td>");
                        sb.Append("<td>");
                            sb.Append("<div style='text-alingment'>");
                                sb.Append("<a style='border-radius: 8px; background-color: #198754; color: white; width: 181px; height: 24px; border: 1px solid #146c43; padding-bottom: 6px; padding-top: 6px; padding-left: 12px;padding-right: 12px;' href='https://localhost:44362/RedefinirSenha?id=" + cliente.Id + "&tipo=" + tipo + "'>Redefinir</a>");
                            sb.Append("</div>");
                        sb.Append("</td>");
                    sb.Append("</tr>");
                sb.Append("</table>");
                request.Body = sb.ToString();
                await emailServico.EnviarEmailAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet("AtualizarSenha")]
        public async Task<IActionResult> AtualizarSenha(int id, string novaSenha)
        {
            try
            {
                Cliente cliente = _context.Clientes.FirstOrDefault(p => p.Id == id);

                if (cliente == null)
                {
                    return Ok(new { Erro = true, Mensagem = "Paciente não encontrado." });
                }

                cliente.Senha = novaSenha;
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAgendamentos")]
        public IActionResult GetAgendamentos([FromQuery, Required] int idCliente)
        {
            try
            {
                var agendamentos = _context.Agenda
                    .Include(p => p.IdProfissionalNavigation)
                    .Include(p => p.IdClinicaNavigation)
                    .Where(p => p.idCliente == idCliente);
                
                return Ok(agendamentos.Select(p => new 
                {
                    p.id,
                    p.idCliente,
                    p.idClinica,
                    p.idProfissional,
                    Status = p.Confirmado == true ? "Confirmado" : "Não confirmado",
                    p.Confirmado,
                    Data = p.Data.ToString("dd/MM/yyyy HH:mm"),
                    Medico = p.IdProfissionalNavigation.Nome,
                    Local = $"Endereço: {p.IdClinicaNavigation.Endereco}, Número: {p.IdClinicaNavigation.Numero}, Bairro: {p.IdClinicaNavigation.Bairro}, Complemento: {p.IdClinicaNavigation.Complemento}",
                    p.Pendente
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpPost("Agendar")]
        public IActionResult Agendar(Agenda agenda)
        {
            try
            {
                Agenda novo = new Agenda();
                novo.idCliente = agenda.idCliente;
                novo.idClinica = agenda.idClinica;
                novo.idProfissional = agenda.idProfissional;
                novo.Data = DateTime.Parse(agenda.Data.ToString("dd/MM/yyyy") + " " + agenda.Hora.ToString());
                novo.Confirmado = false;
                novo.Pendente = true;

                _context.Agenda.Add(novo);
                _context.SaveChanges();
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Desmarcar")]
        public IActionResult Desmarcar(int id)
        {
            try
            {
                Agenda agenda = _context.Agenda.FirstOrDefault(p => p.id == id);
                _context.Agenda.Remove(agenda);
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
