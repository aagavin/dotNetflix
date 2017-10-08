using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotNetflix.Models;
using Microsoft.AspNetCore.Authorization;

namespace dotNetflix.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
