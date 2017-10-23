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


		/// <summary>
		/// Shows the upload page 
		/// </summary>
		/// <returns>IActionResult</returns>
		public IActionResult Index()
		{
			return View();
		}


		/// <summary>
		/// Uploads the a move 
		/// added video the db and google storage
		/// </summary>
		/// <param name="name"></param>
		/// <param name="file"></param>
		/// <returns>IActionResult</returns>
		[HttpPost]
		public async Task<IActionResult> Index([FromForm] string name, [FromForm] IFormFile file)
		{
			int userId = 0;

			if (!file.ContentType.Equals("video/mp4"))
			{
				ViewData["Message"] = "File not correct type mp4 files only";
				return View();
			}

			userId = int.Parse(this.User.Claims.FirstOrDefault().Value);

			BucketAccess bucket = new BucketAccess();
			string url = await bucket.UpdateFile(file.FileName, file.ContentType, file.OpenReadStream());

			using (var context = new DbSqlContext())
			{
				context.Database.EnsureCreated();
				User user = context.Users.Find(userId); //
				Video video = new Video
				{
					Name = name,
					bucketurl = url,
					User = user,

				};
				context.Videos.Add(video);
				context.SaveChanges();
			}

			ViewData["Message"] = "File successfully uploaded upload another?";
			return View();
		}

	}
}
