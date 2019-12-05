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
    public class AlunoController : Controller
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SMPContext"].ToString();
            con = new SqlConnection(constr);

        }


        // GET: Aluno
        public ActionResult PainelAluno()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");
            }
            Usuario usuarioLogado = (Usuario)Session["usuarioLogado"];


            ViewBag.Title = "Bem vindo "+usuarioLogado.no_usuario+ "!" ;
            Meta metaAtual = buscarMeta(usuarioLogado.id_usuario);

            ViewBag.Meta = metaAtual.md_meta;
            ViewBag.PesoAtual = metaAtual.md_peso;

            return View();
        }


        // POST: Aluno/PainelAluno
        //Implementação do Caso de Uso Manter Medidas 
        [HttpPost]
        public ActionResult PainelAluno(Medida novaMedida)
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");
            }
            Usuario usuarioLogado = (Usuario)Session["usuarioLogado"];

            if(novaMedida.md_medida < 1)
            {
                ViewBag.Message = "Insira uma medida válida!";
                return View();
            }



            ViewBag.Title = "Bem vindo " + usuarioLogado.no_usuario + "!";
            Meta metaAtual = buscarMeta(usuarioLogado.id_usuario);
           
            if (AddMedida(novaMedida, metaAtual.id_meta))
            {
                
                ViewBag.PesoAtual = novaMedida.md_medida;
                ViewBag.Meta = metaAtual.md_meta;
                ViewBag.Message = "Medida Inserida com Sucesso!";
                return View();
            }else
            {
                
                ViewBag.Message = "Erro ao inserir medida!";
                return View();
            }

        }


        public ActionResult Grafico()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");
            }
            Usuario usuarioLogado = (Usuario)Session["usuarioLogado"];


            Meta metaAtual = buscarMeta(usuarioLogado.id_usuario);

            ViewBag.Meta = metaAtual.md_meta;
            ViewBag.PesoAtual = metaAtual.md_peso;
            


            ViewBag.DataPoints = JsonConvert.SerializeObject(buscarMedidas(metaAtual.id_meta));
            return View();
        }

        public ActionResult Notificacoes()
        {
            if (Session["usuarioLogado"] == null)
            {
                return RedirectToAction("Login", "home");
            }
            Usuario usuarioLogado = (Usuario)Session["usuarioLogado"];

            return View(buscarNotificacoes(usuarioLogado.id_usuario));
        }



        public ActionResult CadastrarAluno()
        {

            return View();
        }

        // POST: Aluno/NovoAluno    
        //Implementação do Caso de Uso Manter Aluno
        //Cadastro do novo aluno
        [HttpPost]
        public ActionResult CadastrarAluno(Aluno usu)
        {
            try
            {

                /*
                 * Fluxo alternativo do Caso de Uso Manter Aluno
                */
                if(string.IsNullOrEmpty(usu.no_usuario))
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

                if (string.IsNullOrEmpty(usu.md_meta.ToString()))
                {
                    ViewBag.Message = "Informe a Meta!";
                    return View();
                }

                if (string.IsNullOrEmpty(usu.md_peso.ToString()))
                {
                    ViewBag.Message = "Informe o Alvo!";
                    return View();
                }

                /*
                * Fim Fluxo alternativo do Caso de Uso Manter Aluno
               */


                if (ModelState.IsValid)
                {
                    AlunoController UsuRepo = new AlunoController();

                    if (UsuRepo.AddAluno(usu))
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

        //Adciona Novo Aluno
        public bool AddAluno(Aluno obj)
        {

            connection();
            SqlCommand com = new SqlCommand("novoUsuario", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@no_usuario", obj.no_usuario);
            com.Parameters.AddWithValue("@tp_usuario", 2);
            com.Parameters.AddWithValue("@email", obj.email);
            com.Parameters.AddWithValue("@CREF", null);
            com.Parameters.AddWithValue("@senha", obj.senha);
            com.Parameters.AddWithValue("@md_peso", obj.md_peso);
            com.Parameters.AddWithValue("@md_meta", obj.md_meta);

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

        //Adciona Buscar Metas e Valores
        private Meta buscarMeta(int id)
        {

            connection();
            SqlCommand com = new SqlCommand("buscarValoresAtuais", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idUsuario", id);
            Meta metaAtual = new Meta();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    metaAtual.id_meta = reader.GetInt32(reader.GetOrdinal("id_meta"));
                    metaAtual.md_meta = reader.GetInt32(reader.GetOrdinal("md_meta"));
                    if(reader.GetInt32(reader.GetOrdinal("md_medida")) > 0)
                    {
                        metaAtual.md_peso = reader.GetInt32(reader.GetOrdinal("md_medida"));
                    }else
                    {
                        metaAtual.md_peso = reader.GetInt32(reader.GetOrdinal("md_peso"));
                    }

                }
                con.Close();
                return metaAtual;
            }

            con.Close();
            return null;
        }


        //Adciona Nova Medida
        public bool AddMedida(Medida obj, int idMeta)
        {

            connection();
            SqlCommand com = new SqlCommand("novaMedida", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id_meta", idMeta);
            com.Parameters.AddWithValue("@md_medida", obj.md_medida);
            com.Parameters.AddWithValue("@dt_medida", obj.dt_medida);

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

        //Buscar Medidas
        private List<Medida> buscarMedidas(int id)
        {

            connection();
            SqlCommand com = new SqlCommand("buscarMedidas", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idMeta", id);
           List<Medida> medidasAtuais = new List<Medida>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Medida medida = new Medida();
                    medida.dt_medida = reader.GetDateTime(reader.GetOrdinal("dt_medida"));
                    medida.md_medida = reader.GetInt32(reader.GetOrdinal("md_medida"));

                    medidasAtuais.Add(medida);

                }
                con.Close();
                return medidasAtuais;
            }

            con.Close();
            return null;
        }

        //Buscar Notificações
        private List<Notificacao> buscarNotificacoes(int id)
        {

            connection();
            SqlCommand com = new SqlCommand("buscarNotificacoes", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@idUsuarioNotificado", id);
            List<Notificacao> notificacoes = new List<Notificacao>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Notificacao notificacao = new Notificacao();
                    notificacao.usuarioNotificador = reader.GetString(reader.GetOrdinal("no_usuario"));
                    notificacao.txt_titulo = reader.GetString(reader.GetOrdinal("txt_titulo"));
                    notificacao.txt_mensagem = reader.GetString(reader.GetOrdinal("txt_mensagem"));

                    notificacoes.Add(notificacao);

                }
                con.Close();
                return notificacoes;
            }

            con.Close();
            return null;
        }


    }

}
