using ATM;

namespace Sandbox.Maps
{
	class HtuStreet : BaseMap
	{

		public override void Init()
		{
			base.Init();

			ATMEntity e = new();
			e.Position = new Vector3( 505.56f, 2482.12f, 123.86f );
			e.Rotation = Rotation.From( new Angles( 0, 90, 0 ) );
			e.Spawn();
			e.SetupPhysicsFromModel( PhysicsMotionType.Static, false );

			e = new();
			e.Position = new Vector3( -1162.22f, 3012.66f, 123.86f );
			e.Rotation = Rotation.From( new Angles( 0, 0, 0 ) );
			e.Spawn();
			e.SetupPhysicsFromModel( PhysicsMotionType.Static, false );


		}

	}
}
