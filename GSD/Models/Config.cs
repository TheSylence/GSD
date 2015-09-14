using System;

namespace GSD.Models
{
	public class Config
	{
		public virtual T Get<T>()
		{
			return (T)Convert.ChangeType( Value, typeof( T ) );
		}

		public virtual string Id { get; set; }
		public virtual string Value { get; set; }
	}
}