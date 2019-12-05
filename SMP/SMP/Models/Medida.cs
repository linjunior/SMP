using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMP.Models
{
    public class Medida
    {
       public int id_medida { get; set; }
       public Meta meta;
       public int md_medida { get; set; }
       public DateTime dt_medida { get; set; } 
    }
}