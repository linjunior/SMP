using SMP.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SMP.DAL
{
    public class SMPContext : DbContext
    {

        public SMPContext() : base("SMPContext")
        {
        }

        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<TipoUsuario> tipoUsuarios { get; set; }
        public DbSet<Meta> metas { get; set; }

        public DbSet<Medida> medidas { get; set; }
        public DbSet<Notificacao> notificacaos { get; set; }
        public DbSet<Log> logs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}