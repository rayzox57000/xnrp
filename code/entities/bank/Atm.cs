using Sandbox;

namespace ATM
{

	[Library( "ent_atm", Title = "ATM", Spawnable = true )]
	public partial class ATMEntity : Prop
	{
		public override void Spawn()
		{
			base.Spawn();
			SetModel( "models/atm/atm_machine.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		}
	}
}
