using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMP.Models
{
    public class Log
    {
        public int id_log { get; set; }
        public string no_operacao { get; set; }
        public Usuario usuario_envolvido { get; set; }

        public DateTime dt_log { get; set; }


    }
}