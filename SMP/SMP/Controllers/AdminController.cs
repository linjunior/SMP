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
    public class AdminController : Controller
    {
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SMPContext"].ToString();
            con = new SqlConnection(constr);

        }

        // GET: Admin
        public ActionResult PainelAdmin()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");

            }

            return View();
        }

        public ActionResult CadastrarProfessor()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");

            }
            return View();
        }

        // POST: Admin/NovoProfessor    
        //Implementação do Caso de Uso Manter Professor
        //Cadastro do novo professor
        [HttpPost]
        public ActionResult CadastrarProfessor(Professor usu)
        {
            try
            {

                /*
                 * Fluxo alternativo do Caso de Uso Manter Aluno
                */
                if (string.IsNullOrEmpty(usu.no_usuario))
                {
                    ViewBag.Message = "Informe o Nome!";
                    return View();
                }

                if (string.IsNullOrEmpty(usu.email))
                {
                    ViewBag.Message = "Informe o Email!";
                    return View();
                }

                string email = usu.email;

                Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

                if (!rg.IsMatch(email))
                {
                    ViewBag.Message = "Informe um Email válido!";
                    return View();
                }


                if (string.IsNullOrEmpty(usu.senha))
                {
                    ViewBag.Message = "Informe a Senha!";
                    return View();
                }

                if (string.IsNullOrEmpty(usu.CREF.ToString()))
                {
                    ViewBag.Message = "Informe o CREF!";
                    return View();
                }

                /*
                * Fim Fluxo alternativo do Caso de Uso Manter Aluno
               */


                if (ModelState.IsValid)
                {
                    AdminController UsuRepo = new AdminController();

                    if (UsuRepo.AddProfessor(usu))
                    {
                        ViewBag.Message = "Registro inserido com Sucesso";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        



        public ActionResult Lista()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");
            }
            Usuario usuarioLogado = (Usuario)Session["usuarioLogado"];

            return View(buscarProfesor());
        }

        public ActionResult Log()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home"); ;

            }
            return View();
        }

        //Buscar Alunos
        private List<Professor> buscarProfesor()
        {

            connection();
            SqlCommand com = new SqlCommand("ListarProfessores", con);
            com.CommandType = CommandType.StoredProcedure;
            List<Professor> professors = new List<Professor>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Professor professor = new Professor();
                    professor.id_usuario = reader.GetInt32(reader.GetOrdinal("id_usuario"));
                    professor.no_usuario = reader.GetString(reader.GetOrdinal("no_usuario"));
                    professor.CREF = reader.GetString(reader.GetOrdinal("CREF"));


                    professors.Add(professor);

                }
                con.Close();
                return professors;
            }

            con.Close();
            return null;
        }


        //Adciona Novo Professor
        public bool AddProfessor(Professor obj)
        {

            connection();
            SqlCommand com = new SqlCommand("novoUsuario", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@no_usuario", obj.no_usuario);
            com.Parameters.AddWithValue("@tp_usuario", 3);
            com.Parameters.AddWithValue("@email", obj.email);
            com.Parameters.AddWithValue("@CREF", obj.CREF);
            com.Parameters.AddWithValue("@senha", obj.senha);
            com.Parameters.AddWithValue("@md_peso", null);
            com.Parameters.AddWithValue("@md_meta", null);

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