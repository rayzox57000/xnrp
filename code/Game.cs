using Sandbox;
using Sandbox.Maps;
using Sandbox.Jobs.Categories;
using System.Linq;

partial class SandboxGame : Game
{
	public static JobsCategories JobsCategoriesList = new();

	public void InitSpawn() // WIP
	{
		string map = Global.MapName;
		if(map == "facepunch.construct" ) new Construct().Init();
		if ( map == "htu.street" ) new HtuStreet().Init();

	}


	public SandboxGame()
	{
		
		if ( IsServer )
		{
			InitSpawn();
			// Create the HUD
			_ = new SandboxHud();
		}
	}

	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );
		var player = new SandboxPlayer( cl );
		player.Respawn();

		cl.Pawn = player;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	[ServerCmd( "spawn" )]
	public static void Spawn( string modelname )
	{
		var owner = ConsoleSystem.Caller?.Pawn;

		if ( ConsoleSystem.Caller == null )
			return;

		if (owner is SandboxPlayer p)
		{
			if ( p.TimeSincePropSpawn < 6.0f ) return;
			p.TimeSincePropSpawn = 0.0f;
			if (!p.AddMoney(-05.0f)) return;

			var tr = Trace.Ray( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 500 )
				.UseHitboxes()
				.Ignore( owner )
				.Run();

			if ( Entity.All.OfType<Prop>().Where( e => (SandboxPlayer)e.Owner == p ).Count() + 1 > p.PropTotal )
			{
				Log.Error( "PROP LIMIT REACH .... REMOVE BEFORE RESPAWN" );
				return;
			}

			var ent = new Prop();
			ent.Position = tr.EndPos;
			ent.Rotation = Rotation.From( new Angles( 0, owner.EyeRot.Angles().yaw, 0 ) ) * Rotation.FromAxis( Vector3.Up, 180 );
			ent.SetModel( modelname );
			ent.Position = tr.EndPos - Vector3.Up * ent.CollisionBounds.Mins.z;
			ent.Owner = p;
		}
	}

	[ServerCmd( "spawn_entity" )]
	public static void SpawnEntity( string entName )
	{
		var owner = ConsoleSystem.Caller.Pawn;

			if (owner is SandboxPlayer p)
		{

			if ( owner == null )
				return;

			var attribute = Library.GetAttribute( entName );

			if ( attribute == null || !attribute.Spawnable )
				return;

			var tr = Trace.Ray( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 200 )
				.UseHitboxes()
				.Ignore( owner )
				.Size( 2 )
				.Run();

			var ent = Library.Create<Entity>( entName );
			if ( ent is BaseCarriable && owner.Inventory != null )
			{
				if ( owner.Inventory.Add( ent, true ) )
					return;
			}

			Log.Info( tr.EndPos );

			ent.Position = tr.EndPos;
			ent.Rotation = Rotation.From( new Angles( 0, owner.EyeRot.Angles().yaw, 0 ) );
			ent.Owner = p;

			//Log.Info( $"ent: {ent}" );
		}
	}

	public override void DoPlayerNoclip( Client player )
	{
		if ( player.Pawn is Player basePlayer )
		{
			if ( basePlayer.DevController is NoclipController )
			{
				Log.Info( "Noclip Mode Off" );
				basePlayer.DevController = null;
			}
			else
			{
				Log.Info( "Noclip Mode On" );
				basePlayer.DevController = new NoclipController();
			}
		}
	}

	[ClientCmd( "debug_write" )]
	public static void Write()
	{
		ConsoleSystem.Run( "quit" );
	}
}
