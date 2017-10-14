using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotNetflix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace dotNetflix.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {
        public IActionResult Index()
        {
            using(var context = new DbSqlContext()){
                context.Database.EnsureCreated();

                List<Videos> videos =context.Videos.ToList();
                foreach (Videos video in videos)
                {
                    System.Console.WriteLine("**********************************");
                    System.Console.WriteLine(video.User.Userid);
                    System.Console.WriteLine(video.Name);
                    System.Console.WriteLine(video.bucketurl);
                    System.Console.WriteLine("**********************************");
                }
            }
            return View();
        }

    }
}
