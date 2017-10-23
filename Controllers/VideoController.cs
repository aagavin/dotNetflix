using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotNetflix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotNetflix.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {

        /// <summary>
        /// Shows all the videos
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult Index()
        {
            using(var context = new DbSqlContext()){
                context.Database.EnsureCreated();
                ViewData["Videos"] = context.Videos.ToArray();
            }

            return View();
        }


        /// <summary>
        /// Gets a video by id and
        /// updates the view count
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        public IActionResult Id([FromRoute] int id){
            using (var context = new DbSqlContext())
            {
                context.Database.EnsureCreated();
                var video = context.Videos
                    .Include(v => v.User)
                    .Include(v => v.Comments)
                        .ThenInclude(c => c.User)
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


        /// <summary>
        /// Adds a new comment to the db
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="videoId"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult AddComment([FromForm] string comment, [FromForm] int videoId){

            using(var context = new DbSqlContext())
            {
                context.Database.EnsureCreated();

                var user = context.Users.Find(int.Parse(this.User.Claims.FirstOrDefault().Value));
                var video = context.Videos.Find(videoId);

                Comment newComment = new Comment{
                    User = user,
                    UserComment = comment,
                    Video = video,
                };

                context.Add(newComment);
                context.SaveChanges();

            }

            return RedirectToAction("Id", new {id = videoId});
        }

    }
}
