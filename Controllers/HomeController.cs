using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotNetflix.Models;

namespace dotNetflix.Controllers
{
    public class HomeController : Controller
    {


        /// <summary>
        /// Home page of the application
        /// Shows the top viewed videos and
        /// the top commented video
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            using (var context = new DbSqlContext()){
                await context.Database.EnsureCreatedAsync();
                
                ViewData["TopViewVideos"] = await context.Videos
                    .Where(v => v.views>50)
                    .OrderBy(v => v.views)
                    .Take(5)
                    .ToListAsync();

                ViewData["topCommentVideo"] = await context.Videos
                    .Where(v => v.Comments.Count>1)
                    .OrderBy(v => v.Comments.Count)
                    .Take(5)
                    .ToListAsync();
            }
            return View();
            
        }


        /// <summary>
        /// Error page
        /// </summary>
        /// <returns></returns>
        public IActionResult Error() { return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); }
    }
}
