using Sandbox;
using Sandbox.Tools;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Text.RegularExpressions;

namespace Sandbox.UI.Keys
{
	public class Keys : Panel
	{
		public Label Title;
		public Label Description;

		public Keys()
		{
			Title = Add.Label( "Keys", "title" );
			Description = Add.Label( "", "description" );
		}

		public override void Tick()
		{
			Key pg = GetCurrentKeys();
			SetClass( "active", pg != null );

			var player = Local.Pawn;
			if ( player is SandboxPlayer p )
			{
				Description.SetText( GetEyeTarget( p ) );
				if ( p.timeSinceRollInventory >= 2.0f )
				{
					RemoveClass( "hide" );
				}
			}
		}

		string GetEyeTarget( SandboxPlayer p )
		{
			if ( p.Vehicle as Entity == null )
			{
				var eyePos = p.EyePos;
				var eyeRot = p.EyeRot;

				var tr = Trace.Ray( eyePos, eyePos + eyeRot.Forward * 80 ).Radius( 2 ).Ignore( p ).EntitiesOnly().Run();
				if ( tr.Entity as Entity != null )
				{
					var ent = tr.Entity as Entity;


					if ( ent is RpDoorEntity de )
					{
						var m = de.DoorOwner as SandboxPlayer;
						Log.Info( de.DoorOwner );
						if (m != null)
						{
							if ( m == p ) return "Appuyez Click Droit pour (Dé)Vérouiller la porte";
							else return "Propriétaire : " + m.GetClientOwner().Name;
						}
						return "Cette porte est libre ... 'R' pour l'acheter";
					}
				}
			}
			return "Approchez vous d'une porte pour plus d'infos.";
		}

		Key GetCurrentKeys()
		{
			var player = Local.Pawn;
			if ( player == null ) return null;

			var inventory = player.Inventory;
			if ( inventory == null ) return null;
			if ( inventory.Active == null ) return null;

			if ( inventory.Active is Key k )
			{
				return k;
			}
			return null;
		}

		[Event( "buildinput" )]
		public void ProcessClientInput( InputBuilder input )
		{
			if ( Local.Pawn is SandboxPlayer p )
			{
				if ( input.MouseWheel != 0 )
				{
					AddClass( "hide" );
					p.timeSinceRollInventory = 0.0f;
				}
			}
		}

	}

}
