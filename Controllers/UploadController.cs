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
        public async Task<IActionResult> Index([FromForm] IFormFile file)
        {
            if (file.ContentType.Equals("video/mp4"))
            {
                BucketAccess bucket = new BucketAccess();
                string url = await bucket.UpdateFile(file.FileName, file.ContentType, file.OpenReadStream());
                return RedirectToAction("Select", new {url=url});                
            }

            ViewData["Message"] = "File not correct type mp4 files only";
            return View();

        }

        public IActionResult Select(string url){
            ViewData["url"] = url;
            return View();
        }


    }
}
