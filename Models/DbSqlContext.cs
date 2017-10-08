using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotNetflix.Models
{
	public class DbSqlContext : DbContext
	{
		public DbSet<Users> Users { get; set; }
		public DbSet<Videos> Videos { get; set; }
		public DbSet<Comments> Comments { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(@"server=35.186.176.236;database=mydotdb;user=web;password=password;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Users>().HasKey(e => e.Userid);
			modelBuilder.Entity<Videos>().HasKey(e => e.Videoid);
			modelBuilder.Entity<Comments>().HasKey(e => new {e.Userid, e.Videoid });

		}
	}
}