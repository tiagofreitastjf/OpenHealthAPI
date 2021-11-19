using Microsoft.Extensions.Options;
using OpenHealthAPI.Models;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OpenHealthAPI.Servicos
{
    public class EmailServico : IEmailServico
    {
        private readonly EmailSettings _mailSettings;

        public EmailServico(IOptions<EmailSettings> emailSettings)
        {
            _mailSettings = emailSettings.Value;
        }

        public async Task EnviarEmailAsync(EmailRequest mailRequest)
        {
            MailMessage mensagem = new MailMessage();
            SmtpClient smtpCliente = new SmtpClient();

            mensagem.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            mensagem.To.Add(new MailAddress(mailRequest.ToEmail));
            mensagem.Subject = mailRequest.Subject;

            if (mailRequest.Attachments != null)
            {
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                            mensagem.Attachments.Add(att);
                        }
                    }
                }
            }

            mensagem.IsBodyHtml = false;
            mensagem.Body = mailRequest.Body;
            smtpCliente.Port = _mailSettings.Port;
            smtpCliente.Host = _mailSettings.Host;
            smtpCliente.EnableSsl = true;
            smtpCliente.UseDefaultCredentials = false;
            smtpCliente.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            smtpCliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtpCliente.SendMailAsync(mensagem);
        }
    }
}
