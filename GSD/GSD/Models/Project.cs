using System.Collections.Generic;

namespace GSD.Models
{
	internal class Project : Entity
	{
		public Project()
		{
			Tags = new List<Tag>();
			Todos = new List<Todo>();
		}

		public virtual string Name { get; set; }

		public virtual ICollection<Tag> Tags { get; set; }
		public virtual ICollection<Todo> Todos { get; set; }
	}
}