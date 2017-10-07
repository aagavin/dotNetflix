using System;
using System.Collections.Generic;

namespace dotNetflix.Models
{
	public class Users
	{

		public int Userid { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public virtual ICollection<Videos> Videos { get; set; }
		public virtual ICollection<Comments> Comments { get; set; }
	}

	public class Videos
	{
		public int Videoid { get; set; }
		public virtual Users User { get; set; }
		public int views { get; set; }
		public string bucketurl { get; set; }
		public virtual ICollection<Comments> Comments { get; set; }
	}

	public class Comments
	{
		public int Videoid { get; set; }
		public int Userid { get; set; }
		public string Comment { get; set; }
	}
}