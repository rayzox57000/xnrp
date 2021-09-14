using Sandbox;
using Sandbox.Jobs;
using Sandbox.Jobs.Activity;
using Sandbox.Jobs.Category;
using Sandbox.UI;
using Sandbox.UI.JobsMenu;
using System.Collections.Generic;
using System.Linq;

partial class SandboxPlayer : Player
{
	private TimeSince timeSinceDropped;
	private TimeSince timeSinceJumpReleased;

	public TimeSince timeSinceHudBind;

	private TimeSince timeSinceLastSalary;
	private DamageInfo lastDamage;
	public Job CurrentJob {get; set;} = new(-1);
	public JobsActivity CurrentActivity {get; set;} = new(-1);

	[Net] public float Wallet {get; set;} = 200.0f;
	[Net] public float Bank {get; set;} = 0.0f;

	[Net] public int PropCount { get; set; } = 0;
	[Net] public int PropTotal { get; set; } = 0;


	[Net] public string JobName {get; set;} = "Citoyen";
	[Net] public string JobActivityName {get; set;} = "";
	[Net] public int JobID {get; set;} = -1;

	[Net] public PawnController VehicleController { get; set; }
	[Net] public PawnAnimator VehicleAnimator { get; set; }
	[Net, Predicted] public ICamera VehicleCamera { get; set; }
	[Net, Predicted] public Entity Vehicle { get; set; }
	[Net, Predicted] public ICamera MainCamera { get; set; }

	public ICamera LastCamera { get; set; }

	public TimeSince timeSinceRollInventory; 

	

	/// <summary>
	/// The clothing container is what dresses the citizen
	/// </summary>
	public Clothing.Container Clothing = new();

	/// <summary>
	/// Default init
	/// </summary>
	public SandboxPlayer()
	{
		Inventory = new Inventory( this );
	}

	/// <summary>
	/// Initialize using this client
	/// </summary>
	public SandboxPlayer( Client cl ) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient( cl );
	}

	public override void Spawn()
	{
		MainCamera = new FirstPersonCamera();
		LastCamera = MainCamera;

		base.Spawn();
	}

	public override void Respawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();

		MainCamera = LastCamera;
		Camera = MainCamera;

		if ( DevController is NoclipController )
		{
			DevController = null;
		}

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		Clothing.DressEntity( this );

		Inventory.Add( new PhysGun(), true );
		//Inventory.Add( new GravGun() );
		Inventory.Add( new Tool() );
		//Inventory.Add( new Pistol() );
		Inventory.Add( new Flashlight() );

		base.Respawn();

		this.Health = this.CurrentJob.Health;

	}

	public override void OnKilled()
	{
		base.OnKilled();

		if ( lastDamage.Flags.HasFlag( DamageFlags.Vehicle ) )
		{
			Particles.Create( "particles/impact.flesh.bloodpuff-big.vpcf", lastDamage.Position );
			Particles.Create( "particles/impact.flesh-big.vpcf", lastDamage.Position );
			PlaySound( "kersplat" );
		}

		VehicleController = null;
		VehicleAnimator = null;
		VehicleCamera = null;
		Vehicle = null;

		BecomeRagdollOnClient( Velocity, lastDamage.Flags, lastDamage.Position, lastDamage.Force, GetHitboxBone( lastDamage.HitboxIndex ) );
		LastCamera = MainCamera;
		MainCamera = new SpectateRagdollCamera();
		Camera = MainCamera;
		Controller = null;

		EnableAllCollisions = false;
		EnableDrawing = false;

		Inventory.DropActive();
		Inventory.DeleteContents();
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( GetHitboxGroup( info.HitboxIndex ) == 1 )
		{
			info.Damage *= 10.0f;
		}

		lastDamage = info;

		TookDamage( lastDamage.Flags, lastDamage.Position, lastDamage.Force );

		base.TakeDamage( info );
	}

	[ClientRpc]
	public void TookDamage( DamageFlags damageFlags, Vector3 forcePos, Vector3 force )
	{
	}

	public override PawnController GetActiveController()
	{
		if ( VehicleController != null ) return VehicleController;
		if ( DevController != null ) return DevController;

		return base.GetActiveController();
	}

	public override PawnAnimator GetActiveAnimator()
	{
		if ( VehicleAnimator != null ) return VehicleAnimator;

		return base.GetActiveAnimator();
	}

	public ICamera GetActiveCamera()
	{
		if ( VehicleCamera != null ) return VehicleCamera;

		return MainCamera;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl ); 


		if( timeSinceLastSalary >= 120.0f )
		{
			float s = (CurrentJob.Salary * 2.0f) / 60.0f;
			Bank += s;
			timeSinceLastSalary = 0.0f;
		}


		if ( Input.ActiveChild != null )
		{
			ActiveChild = Input.ActiveChild;
		}

		if ( LifeState != LifeState.Alive )
			return;

		if ( VehicleController != null && DevController is NoclipController )
		{
			DevController = null;
		}

		var controller = GetActiveController();
		if ( controller != null )
			EnableSolidCollisions = !controller.HasTag( "noclip" );

		TickPlayerUse();
		SimulateActiveChild( cl, ActiveChild );

		if ( Input.Pressed( InputButton.View ) )
		{
			if ( MainCamera is not FirstPersonCamera )
			{
				MainCamera = new FirstPersonCamera();
			}
			else
			{
				MainCamera = new ThirdPersonCamera();
			}
		}

		Camera = GetActiveCamera();

		if ( Input.Pressed( InputButton.Drop ) )
		{
			var dropped = Inventory.DropActive();
			if ( dropped != null )
			{
				dropped.PhysicsGroup.ApplyImpulse( Velocity + EyeRot.Forward * 500.0f + Vector3.Up * 100.0f, true );
				dropped.PhysicsGroup.ApplyAngularImpulse( Vector3.Random * 100.0f, true );

				timeSinceDropped = 0;
			}
		}

		if ( Input.Released( InputButton.Jump ) )
		{
			if ( timeSinceJumpReleased < 0.3f )
			{
				Game.Current?.DoPlayerNoclip( cl );
			}

			timeSinceJumpReleased = 0;
		}

		if ( Input.Left != 0 || Input.Forward != 0 )
		{
			timeSinceJumpReleased = 1;
		}
	}

	public override void StartTouch( Entity other )
	{
		if ( timeSinceDropped < 1 ) return;

		base.StartTouch( other );
	}

	public bool AddMoney(float take, bool fromWallet = true)
	{
		if ( IsClient ) return false;
		if(take < 0.0f && (!HasMoney(-take,fromWallet))) {
			Log.Error("NOT ENOUGH MONEY");
			return false;
		}
		if(fromWallet == true) this.Wallet += take;
		else this.Bank += take;
		return true;
	}

	public bool HasMoney(float take, bool fromWallet = true)
	{
		return ((fromWallet == true ? this.Wallet : this.Bank) - take) >= 0.0f;
	}

	public void UpPropCount()
	{
		//PropCount = Entity.All.OfType<PropExtended>().Where( e => e.Owner == this ).Count();			
	}


	[ClientRpc]
	public static void UpdateJob(int CLIENTID, int PLACES,int NEWPLACESTAKEN)
	{
		JobMenu JobMenu = SandboxHud.JobMenu;
		if(JobMenu == null || JobMenu.STEP_2 == null) return;
		JobsStep JobsStep = JobMenu.STEP_2;
		if(JobsStep == null || JobsStep.JobCountList == null) return;
		JobsStep.JobCountList.ForEach((Label JobCount) => {
				if(JobCount.HasClass($"JOBID_{CLIENTID}")) {
					JobCount.Text = $"{NEWPLACESTAKEN}/{PLACES}";
				}
		});
	}

	[ServerCmd( "inventory_current" )]
	public static void SetInventoryCurrent( string entName )
	{
		var target = ConsoleSystem.Caller.Pawn;
		if ( target == null ) return;

		var inventory = target.Inventory;
		if ( inventory == null )
			return;

		for ( int i = 0; i < inventory.Count(); ++i )
		{
			var slot = inventory.GetSlot( i );
			if ( !slot.IsValid() )
				continue;

			if ( !slot.ClassInfo.IsNamed( entName ) )
				continue;

			inventory.SetActiveSlot( i, false );

			break;
		}
	}

	[ServerCmd( "BecomeJob")]

	public static void BecomeJob(int ID = -1, int IDACT = -1)
	{
		if(ConsoleSystem.Caller.Pawn is SandboxPlayer player)
		{
			player.JobName = "";
			player.JobActivityName = "";
			
			List<JobsCategory> CategList = SandboxGame.JobsCategoriesList.CategList;
			Job JobSelected = null;
			JobsActivity ActivitySelected = null;

			CategList.ForEach((JobsCategory Category) => {
				Category.JobsList.ForEach((Job Job) => {
					if(JobSelected == null && Job.UniqueID == ID) JobSelected = Job;
				});
			});

			if(JobSelected == null) return;

			if(IDACT != -1) JobSelected.Activities.ForEach((JobsActivity JobsActivity) => { if(ActivitySelected == null && JobsActivity.UniqueID == IDACT) ActivitySelected = JobsActivity;});


			if(JobSelected.TakeJob() == false) return;
			if(player.CurrentJob.OutJob() == false) return;
			UpdateJob(player.CurrentJob.UniqueID,player.CurrentJob.Places,player.CurrentJob.PlacesTaken);
			UpdateJob(JobSelected.UniqueID,JobSelected.Places,JobSelected.PlacesTaken);

			player.CurrentJob = JobSelected;
			if(ActivitySelected != null) {
				player.CurrentActivity = ActivitySelected;
				player.JobActivityName = ActivitySelected.Name;
			}
			player.JobName = JobSelected.Name;
			player.Respawn();
			player.timeSinceLastSalary = 0.0f;
		
		}
		
	}



		// TODO

		//public override bool HasPermission( string mode )
		//{
		//	if ( mode == "noclip" ) return true;
		//	if ( mode == "devcam" ) return true;
		//	if ( mode == "suicide" ) return true;
		//
		//	return base.HasPermission( mode );
		//	}
	}
