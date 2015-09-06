namespace GSD.Models
{
	internal class Entity
	{
		public static bool operator !=( Entity x, Entity y )
		{
			return !( x == y );
		}

		public static bool operator ==( Entity x, Entity y )
		{
			return Equals( x, y );
		}

		public override bool Equals( object obj )
		{
			Entity other = obj as Entity;
			if( other == null )
				return false;

			// handle the case of comparing two NEW objects
			bool otherIsTransient = Equals( other.Id, 0 );
			bool thisIsTransient = Equals( Id, 0 );
			if( otherIsTransient && thisIsTransient )
				return ReferenceEquals( other, this );

			return other.Id.Equals( Id );
		}

		public override int GetHashCode()
		{
			// Once we have a hash code we'll never change it
			if( OldHashCode.HasValue )
				return OldHashCode.Value;

			bool thisIsTransient = Equals( Id, 0 );

			// When this instance is transient, we use the base GetHashCode()
			// and remember it, so an instance can NEVER change its hash code.
			if( thisIsTransient )
			{
				OldHashCode = base.GetHashCode();
				return OldHashCode.Value;
			}
			return Id.GetHashCode();
		}

		public virtual int Id { get; set; }
		private int? OldHashCode;
	}
}