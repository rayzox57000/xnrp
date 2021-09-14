using Sandbox;

[Library( "weapon_keys", Title = "Clés", Spawnable = true )]
partial class Key : Weapon
{
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";
	public override float PrimaryRate => 1;
	public override float SecondaryRate => 1;
	public override float ReloadTime => 0.0f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/v_rust_pistol.vmdl" );
	}

	public Entity EyeFocusDoor(SandboxPlayer p)
	{
		var eyePos = p.EyePos;
		var eyeRot = p.EyeRot;

		var tr = Trace.Ray( eyePos, eyePos + eyeRot.Forward * 80 ).Radius( 2 ).Ignore( p ).EntitiesOnly().Run();
		if ( tr.Entity as RpDoorEntity != null ) return tr.Entity;
		return null;
	}

	public override void AttackPrimary()
	{}

	public override void AttackSecondary()
	{
		if ( Owner is SandboxPlayer p )
		{
			if ( EyeFocusDoor( p ) is RpDoorEntity e )
			{
				if ( e.DoorOwner is SandboxPlayer d )
				{
					if ( e.Locked == true && p == d ) { e.Unlock(); Log.Error( "DEBUG : UNLOCK OK !!!" );  return; }
					if ( ( e.Locked == false ) && p == d ) { e.Lock(); Log.Error( "DEBUG : LOCK OK  !!!" ); return; }
				}
			}
		}
	}

	public override void Reload()
	{
		if ( Owner is SandboxPlayer p )
		{
			if ( EyeFocusDoor( p ) is RpDoorEntity e )
			{
				if ( e.DoorOwner == null && p.AddMoney(-50,true) )
				{
					Log.Error( "DEBUG : ACHAT OK !!!" );
					e.SetDoorOwner(p as Player);
				}
			}
		}
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		
	}

	[ClientRpc]
	protected virtual void DoubleShootEffects()
	{
		
	}

	public override void OnReloadFinish()
	{
		IsReloading = false;

		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		FinishReload();
	}

	[ClientRpc]
	protected virtual void FinishReload()
	{
		ViewModelEntity?.SetAnimBool( "reload_finished", true );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 3 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1.0f );
	}
}
