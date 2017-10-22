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

namespace dotNetflix.Controllers
{
	public class AccountController : Controller
	{
        [Authorize]
		public IActionResult Index()
		{
			using (var context = new DbSqlContext())
			{
				context.Database.EnsureCreated();
                // get userid of logged in user
                // var user = context.Users.Find(int.Parse(this.User.Claims.FirstOrDefault().Value));
                int userid =int.Parse(this.User.Claims.FirstOrDefault().Value);
                var comments = context.Comments.Where(c => c.User.Userid == userid).ToList();

                ViewData["Comments"] = comments;
                return View();
			}
			// return RedirectToAction("Index", "Home");
		}

        [Authorize]
        public IActionResult UpdateComment([FromForm] string userComment, [FromForm] int commentId)
        {
            using(var context = new DbSqlContext()){
                context.Database.EnsureCreated();

                var comment = context.Comments.Find(commentId);
                comment.UserComment = userComment;
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult DeleteComment([FromBody] int commentId){
            using(var context = new DbSqlContext()){
                context.Database.EnsureCreated();
                var comment = context.Comments.Find(commentId);
                context.Remove(comment);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

		public IActionResult Signin()
		{
			return View();
		}

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

        [Authorize]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create([FromForm] string username, [FromForm] string password, [FromForm] string passwordverify)
		{
			Console.WriteLine($"{username} {password} {passwordverify} {password.Equals(passwordverify)}");

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
