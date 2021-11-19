using OpenHealthAPI.Models;
using System.Threading.Tasks;

namespace OpenHealthAPI.Servicos
{
    public interface IEmailServico
    {
        Task EnviarEmailAsync(EmailRequest emailRequest);
    }
}
