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

namespace SMP.Controllers
{
    public class HomeController : Controller
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SMPContext"].ToString();
            con = new SqlConnection(constr);

        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Login()
        {
            Session["usuarioLogado"] = null;
            ViewBag.Message = "Your Login page.";
            return View();
        }

        // POST: Home/Login
        //Implementação do Caso de Uso Login
        [HttpPost]
        public ActionResult Login(Usuario usu)
        {
            try
            {
                
                //Fluxo Alternativo do Login
                if (string.IsNullOrEmpty(usu.email))
                {
                    ViewBag.Message = "Informe o Email!";
                    return View();
                }
                usu.email = usu.email.Trim();
                string email = usu.email;

                Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

                if (!rg.IsMatch(email) && usu.email != "admin" )
                {
                    ViewBag.Message = "Informe um Email válido!";
                    return View();
                }

                if (string.IsNullOrEmpty(usu.senha))
                {
                    ViewBag.Message = "Informe a Senha!";
                    return View();
                }
                //Fluxo Alternativo do Login



                if (ModelState.IsValid)
                {
                    HomeController UsuRepo = new HomeController();
                    Usuario usuariologado = UsuRepo.logar(usu);


                    if (usuariologado != null)
                    {
                        Session["usuarioLogado"] = usuariologado;
                        if(usuariologado.id_tipoUsuario == 1)
                        {
                            //Fluxo Alternativo do Login, Logar ADMIN
                            return RedirectToAction("PainelAdmin", "admin");
                        }else  if (usuariologado.id_tipoUsuario == 2)
                        {
                            //Fluxo Alternativo do Login, Logar Aluno
                            return RedirectToAction("PainelAluno", "aluno");
                        }else
                        {
                            //Fluxo Alternativo do Login, Logar Professor
                            return RedirectToAction("PainelProfessor", "Professor");
                        }

                    }
                    else
                    {
                        ViewBag.Message = "Usuário não encontrado!";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //Caso de uso Logar
        public Usuario logar(Usuario obj)
        {

            connection();
            SqlCommand com = new SqlCommand("logar", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@email", obj.email);
            com.Parameters.AddWithValue("@senha", obj.senha);
            Usuario usuarioLogado = new Usuario();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    usuarioLogado.id_usuario = reader.GetInt32(reader.GetOrdinal("id_usuario"));
                    usuarioLogado.id_tipoUsuario = reader.GetInt32(reader.GetOrdinal("id_tipoUsuario"));
                    usuarioLogado.no_usuario = reader.GetString(reader.GetOrdinal("no_usuario"));
                    usuarioLogado.email = reader.GetString(reader.GetOrdinal("email"));
                }
                con.Close();
                return usuarioLogado;
            }

            con.Close();
            return null;

        }


    }
}