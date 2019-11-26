using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMP.Models
{
    public class Usuario
    {
        private int id_usuario;
        private int id_tipoUsuario;
        private string no_usuario;
        private string email;
        private string CREF;
        private string senha;

        public void setId_usuario (int id_usuario)
        {
            this.id_usuario = id_usuario;
        }

        public int  getId_usuario ()
        {
            return this.id_usuario;
        }

        public void setId_tipoUsuario(int id_tipoUsuario)
        {
            this.id_tipoUsuario = id_tipoUsuario;
        }

        public int getId_tipoUsuario()
        {
            return this.id_tipoUsuario;
        }

        public void setno_usuario(string no_usuario)
        {
            this.no_usuario = no_usuario;
        }

        public string getno_usuario()
        {
            return this.no_usuario;
        }

        public void setemail(string email)
        {
            this.email = email;
        }

        public string getemail()
        {
            return this.email;
        }

        public void setCREF(string CREF)
        {
            this.CREF = CREF;
        }

        public string getCREF()
        {
            return this.CREF;
        }
        public void setsenha(string senha)
        {
            this.senha = senha;
        }

        public String getsenha()
        {
            return this.senha;
        }
    }
}