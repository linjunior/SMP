using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using SMP.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SMP.Controllers
{
    public class ProfessorController : Controller
    {
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SMPContext"].ToString();
            con = new SqlConnection(constr);

        }

        

        // GET: Professor
        public ActionResult PainelProfessor()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");
            }
            Usuario usuarioLogado = (Usuario)Session["usuarioLogado"];


            ViewBag.Title = "Bem vindo " + usuarioLogado.no_usuario + "!";
            
           
            return View(buscarAlunos());
        }


        // GET: Professor
        [HttpPost]
        public ActionResult PainelProfessor(string notificado, string txt_titulo, string txt_mensagem)
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");
            }
            Usuario usuarioLogado = (Usuario)Session["usuarioLogado"];
            if(string.IsNullOrEmpty(txt_titulo))
            {
                ViewBag.Message = "Informe o Título!";
            }
            if (string.IsNullOrEmpty(txt_mensagem))
            {
                ViewBag.Message = "Informe a Mensagem!";
            }

            ProfessorController proRepo = new ProfessorController();
            List<Aluno> alunos = buscarAlunos();
            int idNotificado = 1;
            foreach (var item in alunos)
            {
                if (item.no_usuario.Trim() == notificado.Trim())
                    idNotificado = item.id_usuario;
            }


            if (AddNotificacao(usuarioLogado.id_usuario, idNotificado, txt_titulo, txt_mensagem))
            {
                ViewBag.Message = "Notificação inserida com Sucesso!";
            }
            


            return View(buscarAlunos());
        }




        public ActionResult Listar()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");
            }
            Usuario usuarioLogado = (Usuario)Session["usuarioLogado"];

            return View(buscarAlunos());
        }


        //Buscar Alunos
        private List<Aluno> buscarAlunos()
        {

            connection();
            SqlCommand com = new SqlCommand("ListarUsuarios", con);
            com.CommandType = CommandType.StoredProcedure;
            List<Aluno> alunos = new List<Aluno>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Aluno aluno = new Aluno();
                    aluno.id_usuario = reader.GetInt32(reader.GetOrdinal("id_usuario"));
                    aluno.no_usuario = reader.GetString(reader.GetOrdinal("no_usuario"));
                    aluno.md_meta = reader.GetInt32(reader.GetOrdinal("md_meta"));
                    aluno.md_peso = reader.GetInt32(reader.GetOrdinal("md_medida"));

                    alunos.Add(aluno);

                }
                con.Close();
                return alunos;
            }

            con.Close();
            return null;
        }

        //Adciona Nova Medida
        public bool AddNotificacao(int notificador, int idNotificado, string tituto, string mensagem)
        {

            connection();
            SqlCommand com = new SqlCommand("novaNotificacao", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id_notificador", notificador);
            com.Parameters.AddWithValue("@notificado", idNotificado);
            com.Parameters.AddWithValue("@titulo", tituto);
            com.Parameters.AddWithValue("@mensagem", mensagem);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


}