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

namespace dotNetflix.Controllers
{
    public class AccountController : Controller
    {
      public IActionResult Index()
      {
        return RedirectToAction("Index", "Home");
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

          Users user = context.Users.Where(u => u.Username == username).FirstOrDefault();
          var claims = new List<Claim>{
            new Claim("id", user.Userid.ToString()),
            new Claim("name", user.Username),
            new Claim("role", "user")
          };

          var ci = new ClaimsIdentity(claims, "password", "name", "role");
          var p = new ClaimsPrincipal(ci);
          
          await HttpContext.SignInAsync(p);


        }

        if(Url.IsLocalUrl(returnUrl)){
          return Redirect(returnUrl);
        }
        else{
          return Redirect("/");
        }
        return RedirectToAction("Index", "Home");
      }

      public String Forbidden()
      {
        return "You are not allowed to access this pages";
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

          Users user = new Users();
          user.Username = username;
          user.Password = System.Text.Encoding.UTF8.GetString(passwordHash.ToArray());
          context.Users.Add(user);
          context.SaveChanges();
        }
        ViewData["Message"] = "Account created";
        return RedirectToAction("Index");
      }

    }
}
