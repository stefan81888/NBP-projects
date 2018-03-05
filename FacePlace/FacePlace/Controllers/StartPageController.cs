using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FacePlace.DataLayer.Model;
using FacePlace.DataProcessing;

namespace FacePlace.Controllers
{
    public class StartPageController : Controller
    {
        BusinessLogic dataSource = new BusinessLogic();

        // GET: StartPage
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            string username = user.Username;
            if (dataSource.Register(user) != null)
                return RedirectToAction("Index", "Posts", new { username = username });

            return RedirectToAction("Index", "StartPage");
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            string username = user.Username;
            if (dataSource.Login(user) != null)
                return RedirectToAction("Index", "Posts", new { username = username });

            return RedirectToAction("Index", "StartPage");
        }

    }
}