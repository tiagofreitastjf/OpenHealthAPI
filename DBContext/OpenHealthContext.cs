using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OpenHealthAPI.Models
{
    public partial class OpenHealthContext : DbContext
    {
        public OpenHealthContext()
        {
        }

        public OpenHealthContext(DbContextOptions<OpenHealthContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Autorizacao> Autorizacao { get; set; }
        public virtual DbSet<Clinica> Clinicas { get; set; }
        public virtual DbSet<Consulta> Consulta { get; set; }
        public virtual DbSet<Exame> Exames { get; set; }
        public virtual DbSet<Profissional> Profissionals { get; set; }
        public virtual DbSet<Vacina> Vacinas { get; set; }
        public virtual DbSet<Agenda> Agenda { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=OpenHealth;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bairro)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cep)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("CEP");

                entity.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Complemento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CPF");

                entity.Property(e => e.DataNascimento).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Endereco)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                //entity.Property(e => e.Token)
                //    .IsRequired()
                //    .IsUnicode(false);
            });

            modelBuilder.Entity<Autorizacao>(entity =>
            {
                entity.ToTable("Autorizacao");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.idCliente).HasColumnName("idCliente");

                entity.Property(e => e.idClinica).HasColumnName("idClinica");
                entity.Property(e => e.idProfissional).HasColumnName("idProfissional");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Autorizacao)
                    .HasForeignKey(d => d.idCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.IdClinicaNavigation)
                    .WithMany(p => p.Autorizacao)
                    .HasForeignKey(d => d.idClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                
                entity.HasOne(d => d.IdProfissionalNavigation)
                    .WithMany(p => p.Autorizacao)
                    .HasForeignKey(d => d.idProfissional)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Clinica>(entity =>
            {
                entity.ToTable("Clinica");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bairro)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cep)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("CEP");

                entity.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Complemento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Endereco)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Consulta>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.IdClinica).HasColumnName("idClinica");

                entity.Property(e => e.IdProfissional).HasColumnName("idProfissional");

                entity.Property(e => e.TipoConsulta)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consulta_Cliente");

                entity.HasOne(d => d.IdClinicaNavigation)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.IdClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consulta_Clinica");

                entity.HasOne(d => d.IdProfissionalNavigation)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.IdProfissional)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consulta_Profissional");
            });

            modelBuilder.Entity<Exame>(entity =>
            {
                entity.ToTable("Exame");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArquivoBase64)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.IdClinica).HasColumnName("idClinica");

                entity.Property(e => e.NomeArquivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Exames)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exame_Cliente");

                entity.HasOne(d => d.IdClinicaNavigation)
                    .WithMany(p => p.Exames)
                    .HasForeignKey(d => d.IdClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exame_Clinica");
            });

            modelBuilder.Entity<Profissional>(entity =>
            {
                entity.ToTable("Profissional");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bairro)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cep)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("CEP");

                entity.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Complemento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CPF");

                entity.Property(e => e.DataNascimento).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Endereco)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.IdClinica).HasColumnName("idClinica");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClinicaNavigation)
                    .WithMany(p => p.Profissional)
                    .HasForeignKey(d => d.IdClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profissional_Clinica");
            });

            modelBuilder.Entity<Vacina>(entity =>
            {
                entity.ToTable("Vacina");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.IdClinica).HasColumnName("idClinica");

                entity.Property(e => e.IdProfissional).HasColumnName("idProfissional");

                entity.Property(e => e.Observacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TipoVacina)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Vacinas)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacina_Cliente");

                entity.HasOne(d => d.IdClinicaNavigation)
                    .WithMany(p => p.Vacinas)
                    .HasForeignKey(d => d.IdClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacina_Clinica");

                entity.HasOne(d => d.IdProfissionalNavigation)
                    .WithMany(p => p.Vacinas)
                    .HasForeignKey(d => d.IdProfissional)
                    .HasConstraintName("FK_Vacina_Profissional");
            });
            
            modelBuilder.Entity<Agenda>(entity =>
            {
                entity.ToTable("Agenda");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.idCliente).HasColumnName("idCliente");

                entity.Property(e => e.idClinica).HasColumnName("idClinica");

                entity.Property(e => e.idProfissional).HasColumnName("idProfissional");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Agenda)
                    .HasForeignKey(d => d.idCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.IdClinicaNavigation)
                    .WithMany(p => p.Agenda)
                    .HasForeignKey(d => d.idClinica)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.IdProfissionalNavigation)
                    .WithMany(p => p.Agenda)
                    .HasForeignKey(d => d.idProfissional);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
