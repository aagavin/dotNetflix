using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNetflix.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dotNetflix
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			services.AddAuthentication("Cookies")
			.AddCookie("Cookies", options => {
				options.LoginPath = "/account/signin";
				options.AccessDeniedPath = "/account/signin";
			});

			// claims transformation
			services.AddTransient<IClaimsTransformation, ClaimsTransformer>();


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseAuthentication();
			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
									name: "default",
									template: "{controller=Home}/{action=Index}/{id?}");
			});

		}
	}
}
