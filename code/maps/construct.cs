using ATM;

namespace Sandbox.Maps
{
	class Construct : BaseMap
	{

		public override void Init()
		{
			base.Init();

			ATMEntity e = new();
			e.Position = new Vector3( -1192.14f, -2928.49f, 45.15f );
			e.Rotation = Rotation.From( new Angles( 0, 90, 0 ) );
			e.Spawn();
			e.SetupPhysicsFromModel( PhysicsMotionType.Static, false );

			e = new();
			e.Position = new Vector3( -2018.50f, -2915.93f, 45.15f );
			e.Rotation = Rotation.From( new Angles( 0, 0, 0 ) );
			e.Spawn();
			e.SetupPhysicsFromModel( PhysicsMotionType.Static, false );


		}

	}
}
