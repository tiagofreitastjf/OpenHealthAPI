using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenHealthAPI.DTO;
using OpenHealthAPI.Models;
using OpenHealthAPI.Servicos;
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
        private readonly IEmailServico emailServico;

        public ProfissionalController(OpenHealthContext context, IEmailServico emailServico)
        {
            this._context = context;
            this.emailServico = emailServico;
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
                Profissional jaExite = _context.Profissionals.FirstOrDefault(p => p.Email == dto.Email);
                if (jaExite != null)
                {
                    return BadRequest("Email já cadastrado.");
                }

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
                var profissional = _context.Profissionals.FirstOrDefault(p => p.Email == dto.Email && p.Senha == dto.Senha);
                if (profissional == null) return Ok(new { UserNotFound = true });
                return Ok(profissional);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("RedefinirSenha")]
        public async Task<IActionResult> EnviarEmailRedefinirSenha(string email, string tipo)
        {
            try
            {
                Profissional profissional = _context.Profissionals.FirstOrDefault(p => p.Email == email);

                if (profissional == null)
                {
                    return Ok(new { Erro = true, Mensagem = "Profissional não encontrado." });
                }

                EmailRequest request = new EmailRequest();
                request.ToEmail = profissional.Email;
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
                sb.Append("<a style='border-radius: 8px; background-color: #198754; color: white; width: 181px; height: 24px; border: 1px solid #146c43; padding-bottom: 6px; padding-top: 6px; padding-left: 12px;padding-right: 12px;' href='https://localhost:44362/RedefinirSenha?id='" + profissional.Id + "&tipo=" + tipo + ">Redefinir</a>");
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
                Profissional profissional = _context.Profissionals.FirstOrDefault(p => p.Id == id);

                if (profissional == null)
                {
                    return Ok(new { Erro = true, Mensagem = "Paciente não encontrado." });
                }

                profissional.Senha = novaSenha;
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
