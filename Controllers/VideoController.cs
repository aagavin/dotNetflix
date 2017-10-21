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
                ViewData["Videos"] = context.Videos.ToArray();
            }

            return View();
        }

        public IActionResult Id([FromRoute] int id){
            using (var context = new DbSqlContext())
            {
                context.Database.EnsureCreated();
                var video = context.Videos.Include(v => v.User)
                .Single(v => v.Videoid == id);

                video.views++;

                ViewData["Id"] = video.Videoid;
                ViewData["Name"] = video.Name;
                ViewData["User"] = video.User.Username;
                ViewData["Views"] = video.views;
                ViewData["bucketurl"] = video.bucketurl;
                ViewData["Comments"] = video.Comments;
                context.SaveChanges();
            }
            return View();
        }

        [HttpPost]
        public String AddComment([FromForm] string comment, [FromForm] int videoId){
            return $"{comment} {videoId}";
        }

    }
}
