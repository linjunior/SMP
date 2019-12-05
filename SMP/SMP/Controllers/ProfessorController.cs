using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMP.Controllers
{
    public class ProfessorController : Controller
    {
        // GET: Professor
        public ActionResult PainelProfessor()
        {
            return View();
        }


        public ActionResult Listar()
        {
            return View();
        }
    }
}