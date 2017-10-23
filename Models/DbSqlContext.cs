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


		/// <summary>
		/// Configures the db connection
		/// </summary>
		/// <param name="optionsBuilder"></param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(@"server=35.186.176.236;database=mydotdb;user=web;password=password;");
			// optionsBuilder.UseMySql(@"server=localhost;database=mydotdb;user=root;password=admin;");
		}


		/// <summary>
		/// Creates the db
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder){	base.OnModelCreating(modelBuilder);	}
	}
}