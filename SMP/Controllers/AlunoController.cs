using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMP.Controllers
{
    public class AlunoController : Controller
    {
        // GET: Aluno
        public ActionResult PainelAluno()
        {
            return View();
        }


        public ActionResult Grafico()
        {
            //private DataPointsDBEntities _db = new DataPointsDBEntities();
            //https://canvasjs.com/docs/charts/integration/asp-net-mvc-charts/how-to/asp-net-mvc-charts-data-entity-database/


            //return View(_db.DataPointsSet.ToList());
            return View();
        }

        public ActionResult Notificacoes()
        {

            return View();
        }





    }
}