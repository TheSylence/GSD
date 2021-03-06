﻿using System.Collections.Generic;

namespace GSD.Models
{
	internal class Tag : Entity
	{
		public Tag()
		{
			Todos = new List<Todo>();
		}

		public virtual string Color { get; set; }
		public virtual string Name { get; set; }
		public virtual Project Project { get; set; }

		public virtual ICollection<Todo> Todos { get; set; }
	}
}