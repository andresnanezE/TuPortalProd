﻿using System.Web.Mvc;

namespace Presentacion.Mvc.App.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index(string msg)
        {
            ViewBag.Mensaje = msg;
            return View("GeneralError");
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  // 200
            return View("Error");
        }

        public ViewResult GeneralError()
        {
            Response.StatusCode = 404;  // 200
            return View("GeneralError");
        }
    }
}