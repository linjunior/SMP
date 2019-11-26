using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMP.Models
{
    public class Notificacao
    {
        public int id_notificacao { get; set; }
        public Usuario usuarioNotificador;
        public Usuario usuarioNotificado;
        public string txt_titulo { get; set; }
        public string txt_mensagem { get; set; }
    }
}