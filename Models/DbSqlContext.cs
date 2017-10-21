using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotNetflix.Models
{
	public class DbSqlContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Video> Videos { get; set; }
		public DbSet<Comment> Comments { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(@"server=35.186.176.236;database=mydotdb;user=web;password=password;");
			// optionsBuilder.UseMySql(@"server=localhost;database=mydotdb;user=root;password=admin;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// modelBuilder.Entity<User>().HasKey(e => e.Userid);
			// modelBuilder.Entity<Video>().HasKey(e => e.Videoid);
			// modelBuilder.Entity<Comment>().HasKey(e => new {e.Userid, e.Videoid });

		}
	}
}