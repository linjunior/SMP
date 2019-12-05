using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMP.Models
{
    public class Meta
    {
        public int id_meta { get; set; }
        public Usuario usuario;
        public int md_peso { get; set; }
        public int md_meta { get; set; }
    }
}