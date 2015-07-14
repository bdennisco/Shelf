using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shelf.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /Public/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
	}
}