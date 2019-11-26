using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SMP.Models;

namespace SMP.DAL
{
    public class SMPInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SMPContext>
    {
        protected override void Seed(SMPContext context)
        {
            var tipoUsuarios = new List<TipoUsuario>
            {
            new TipoUsuario{id_tipoUsuario=1,no_tipoUsuario="Admin"},
            new TipoUsuario{id_tipoUsuario=2,no_tipoUsuario="Professor"},
            new TipoUsuario{id_tipoUsuario=3,no_tipoUsuario="Aluno"}
            };

            tipoUsuarios.ForEach(s => context.tipoUsuarios.Add(s));
            context.SaveChanges();

            var usuarios = new List<Usuario>
            {
            new Usuario{id_usuario=1,no_usuario="Admin",email="Admin@admin.com",senha="admin",CREF="", id_tipoUsuario=1}
            };
            usuarios.ForEach(s => context.usuarios.Add(s));
            context.SaveChanges();
           
        }

    }
}