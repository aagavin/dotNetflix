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
		public async Task<IActionResult> Index([FromForm] string name, [FromForm] IFormFile file)
		{
			if (file.ContentType.Equals("video/mp4"))
			{
				int userId = 0;

				foreach (var claim in User.Claims)
				{
					if (claim.Type.Equals("id"))
					{
						userId = int.Parse(claim.Value);
						break;
					}
				}

				BucketAccess bucket = new BucketAccess();
				string url = await bucket.UpdateFile(file.FileName, file.ContentType, file.OpenReadStream());

				using (var context = new DbSqlContext())
				{
					Users user = context.Users.Where(u => u.Userid == userId).FirstOrDefault();
					context.Database.EnsureCreated();
					Videos video = new Videos();
					video.Name = name;
					video.bucketurl = url;
					video.User = user;
					context.Videos.Add(video);
					context.SaveChanges();
				}

				ViewData["Message"] = "File successfully uploaded upload another?";
				return View();
			}

			ViewData["Message"] = "File not correct type mp4 files only";
			return View();

		}

	}
}
