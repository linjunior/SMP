using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMP.Models
{
    //Classe Aluno Herdando de Usuario
    public class Aluno : Usuario
    {
        public int md_peso { get; set; }

        public int md_meta { get; set; }
    }
}