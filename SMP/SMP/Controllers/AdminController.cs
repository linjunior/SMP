﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMP.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult PainelAdmin()
        {
            return View();
        }

        public ActionResult CadastrarProfessor()
        {
            return View();
        }

        public ActionResult Lista()
        {
            return View();
        }

        public ActionResult Log()
        {
            return View();
        }
    }
}