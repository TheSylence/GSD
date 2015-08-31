using System.Collections.Generic;

namespace GSD.Models
{
	internal class Todo : Entity
	{
		public Todo()
		{
			Tags = new List<Tag>();
		}

		public virtual string Details { get; set; }
		public virtual Project Project { get; set; }
		public virtual string Summary { get; set; }
		public virtual ICollection<Tag> Tags { get; set; }
	}
}