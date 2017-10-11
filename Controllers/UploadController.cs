using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotNetflix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace dotNetflix.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] IFormFile file){

            // BucketAccess bucket = new BucketAccess();
            System.Console.WriteLine("*****************");
            System.Console.WriteLine(file.FileName);
            System.Console.WriteLine(file.ContentType);
            System.Console.WriteLine("*****************");
            return RedirectToAction("Select");
        }

        public string Select(){
            return "You are here";
        }


    }
}
