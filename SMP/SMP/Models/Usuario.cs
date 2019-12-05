using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMP.Models
{
    //Classe Usuario
    //Essa é a classe Pai das classes Professor e Aluno
    public class Usuario
    {
        public int id_usuario { get; set; }
        public int id_tipoUsuario { get; set; }
        public string no_usuario { get; set; }
        public string email { get; set; }

        public string senha { get; set; }

    }
}