namespace OpenHealthAPI.Models
{
    public class Autorizacao
    {
        public int id { get; set; }
        public int idCliente { get; set; }
        public int? idClinica { get; set; }
        public int idProfissional { get; set; }
        public bool Pendente { get; set; }
        public string Descricao { get; set; }
        public bool Autorizado { get; set; }
        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Clinica IdClinicaNavigation { get; set; }
        public virtual Profissional IdProfissionalNavigation { get; set; }
    }
}
