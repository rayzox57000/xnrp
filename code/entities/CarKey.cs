using Sandbox;
using System.Collections.Generic;

[Library( "weapon_carkey", Title = "Clé Voiture", Spawnable = true )]
partial class CarKey : Weapon
{ 
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;

	public TimeSince TimeSinceDischarge { get; set; }
	public new TimeSince TimeSincePrimaryAttack = 2.0f;
	public new TimeSince TimeSinceSecondaryAttack = 2.0f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed( InputButton.Attack1 );
	}
	public IEnumerable<Entity> FocusVehicle( SandboxPlayer p )
	{
		return Physics.GetEntitiesInSphere( p.Position, 500.0f );
	}

	public void Switch()
	{
		if ( IsClient ) return;
		if ( TimeSincePrimaryAttack < 1.0f || TimeSinceSecondaryAttack < 1.0f ) return;
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( Owner is SandboxPlayer p )
		{
			foreach ( Entity e in FocusVehicle( p ) )
			{
				if ( e is CarEntity v  && v.Owner is SandboxPlayer d && p == d ) v.Switch();
			}
		}
	}

	public override void AttackPrimary()
	{
		Switch();
	}

	public override void AttackSecondary()
	{
		Switch();
	}

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 1 );
		anim.SetParam( "aimat_weight", 1.0f );
		anim.SetParam( "holdtype_handedness", 0 );
	}
}
