using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNetflix.Models
{
	public class User
	{
		[Key]
		public int Userid { get; set; }
		public string Username { get; set; }
		public byte[] Password { get; set; }

	}

	public class Video
	{
		[Key]
		public int Videoid { get; set; }
		public User User { get; set; }	
		public string Name { get; set; }
		public int views { get; set; }
		public string bucketurl { get; set; }
		public ICollection<Comment> Comments { get; set; }
	}

	public class Comment
	{
		[Key]
		public int Commentid { get; set; }
		public Video Video { get; set; }
		public User User { get; set; }
		public string UserComment { get; set; }
	}
}
