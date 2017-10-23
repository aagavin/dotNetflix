using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotNetflix.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace dotNetflix.Controllers
{
	public class AccountController : Controller
	{

		/// <summary>
		/// Show the logged in user there 
		/// comments and uploaded videos
		/// </summary>
		/// <returns>IActionResult</returns>
        [Authorize]
		public IActionResult Index()
		{
			using (var context = new DbSqlContext())
			{
				context.Database.EnsureCreated();
                // get userid of logged in user
                int userid =int.Parse(this.User.Claims.FirstOrDefault().Value);
                var comments = context.Comments.Where(c => c.User.Userid == userid).ToList();

				var videos = context.Videos
				.Where(v => v.User.Userid == userid)
				.Include(v => v.Comments)
				.ToList();
				
                ViewData["Comments"] = comments;
				ViewData["Videos"] = videos;
                return View();
			}
		}


		/// <summary>
		/// Allows a use to update
		/// the name of their uploaded video
		/// or
		/// delete a video that the user uploaded
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="delete"></param>
		/// <returns>IActionResult</returns>
		[Authorize]
		[HttpPost]
		public IActionResult UpdateVideo([FromForm] int id, [FromForm] string name, [FromForm] int? delete){

			using (var context = new DbSqlContext())
			{
				context.Database.EnsureCreated();
				var video = context.Videos
				.Include(v => v.Comments)
				.Single(v => v.Videoid==id);

				if (delete != null)
				{
					context.Remove(video);
				}
				else
				{
					video.Name = name;
				}
				context.SaveChanges();
			}

			return RedirectToAction("Index");
		}


		/// <summary>
		/// Allows a use to update
		/// the text of their comment
		/// or
		/// delete a comment that the user uploaded
		/// </summary>
		/// <param name="userComment"></param>
		/// <param name="commentId"></param>
		/// <param name="delete"></param>
		/// <returns>IActionResult</returns>
        [Authorize]
        public IActionResult UpdateComment([FromForm] string userComment, [FromForm] int commentId, [FromForm] int? delete)
        {
            using(var context = new DbSqlContext()){
                context.Database.EnsureCreated();

                var comment = context.Comments.Find(commentId);

				if (delete !=null)
				{
					context.Remove(comment);
				}
				else
				{
					comment.UserComment = userComment;	
				}
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


		/// <summary>
		/// Shows the sign in page
		/// </summary>
		/// <returns>IActionResult</returns>
		public IActionResult Signin(){return View();}


		/// <summary>
		/// Signs in a user using username and password
		/// If valid a cooke will be set 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Signin([FromForm] string username, [FromForm] string password, string returnUrl = null)
		{
			Console.WriteLine($"{username} {password}");

			if (username == null || password == null)
			{
				return View();
			}

			using (var context = new DbSqlContext())
			{
				context.Database.EnsureCreated();

				User user = context.Users.Where(u => u.Username == username).FirstOrDefault();

				PasswordHash passwordHash = new PasswordHash(user.Password);

				if (!passwordHash.Verify(password))
				{
					return Redirect("/account/signin");
				}

				var claims = new List<Claim>{
						new Claim("id", user.Userid.ToString()),
						new Claim("name", user.Username),
						new Claim("role", "user")
						};

				var ci = new ClaimsIdentity(claims, "password", "name", "role");
				var p = new ClaimsPrincipal(ci);

				await HttpContext.SignInAsync(p);


			}

			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return Redirect("/");
			}
		}


		/// <summary>
		/// logouts a user and redirects back to the home page
		/// </summary>
		/// <returns>IActionResult</returns>
        [Authorize]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}


		/// <summary>
		/// Shows the Create account page 
		/// </summary>
		/// <returns>IActionResult</returns>
		public IActionResult Create(){ return View(); }


		/// <summary>
		/// Creates an account and saves a hash of the users password
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="passwordverify"></param>
		/// <returns>IActionResult</returns>
		[HttpPost]
		public IActionResult Create([FromForm] string username, [FromForm] string password, [FromForm] string passwordverify)
		{

			if (username == null || password == null || passwordverify == null)
			{
				ViewData["Message"] = "Error all items need to be created";
				return View();
			}
			else if (!password.Equals(passwordverify))
			{
				ViewData["Message"] = $"Passwords dont match {password} {passwordverify}";
				return View();
			}

			PasswordHash passwordHash = new PasswordHash(password);

			using (var context = new DbSqlContext())
			{
				context.Database.EnsureCreated();

				User user = new User();
				user.Username = username;
				user.Password = passwordHash.ToArray();
				context.Users.Add(user);
				context.SaveChanges();
			}
			ViewData["Message"] = "Account created";
			return RedirectToAction("Index");
		}

	}
}
