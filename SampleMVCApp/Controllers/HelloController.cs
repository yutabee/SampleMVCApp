﻿using Microsoft.AspNetCore.Mvc;

namespace SampleMVCApp.Controllers
{
    public class HelloController : Controller
    {
        public List<string> list;

        public HelloController()
        {
            list = new List<string>();
            list.Add("Japan");
            list.Add("USA");
            list.Add("India");
            list.Add("UK");
        }

        [Route("hello/{id?}/{name?}")]
        public IActionResult Index(int id, string name)
        {
            ViewData["message"] = "id = " + id + ", name = " + name;
            return View();
        }


        [HttpPost]
        public IActionResult Form()
        {
            ViewData["message"] = '"' + Request.Form["list"] + '"' + " selected.";
            ViewData["list"] = Request.Form["list"];
            ViewData["listdata"] = list;
            return View("Index");
        }
    }
}

