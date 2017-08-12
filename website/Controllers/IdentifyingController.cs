using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace website.Controllers
{
    public class IdentifyingController : Controller
    {
        // GET: Identifying
        public ActionResult Identifying()
        {
            return View();

        }

        public ActionResult Index()
        {
            return View("Identifying");
        }

    }
}