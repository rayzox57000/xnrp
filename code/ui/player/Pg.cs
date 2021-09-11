using Sandbox;
using Sandbox.Tools;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Text.RegularExpressions;

namespace Sandbox.UI.Pg
{
	public class Pg : Panel
	{
		public Label Title;
		public Label Description;

		public Pg()
		{
			Title = Add.Label( "PhysGun", "title" );
			Description = Add.Label("", "description");
		}

		public override void Tick()
		{
			PhysGun pg = GetCurrentPG();
			SetClass( "active", pg != null );

			var player = Local.Pawn;
			if ( player is SandboxPlayer p ) {
				Description.SetText(GetEyeTarget(p));
				if (p.timeSinceRollInventory >= 2.0f) {
					RemoveClass("hide");
				}		
			}
		}

		string GetEyeTarget(SandboxPlayer p)
		{
			if ( p.Vehicle as Entity == null )
			{
				var eyePos = p.EyePos;
				var eyeRot = p.EyeRot;

				var tr = Trace.Ray( eyePos, eyePos + eyeRot.Forward * 5000 ).Radius( 2 ).Ignore( p ).EntitiesOnly().Run();
				if ( tr.Entity as Entity != null )
				{
					var ent = tr.Entity as Entity;
					string s = ent.ClassInfo.Name;

					

					if(ent is Sandbox.Prop pr)
					{
						var m = pr.GetModelName();
						var mn = m.Substring(m.LastIndexOf("/")+1);
						s += " ( Modèle : " + Regex.Replace(mn, $"{".vmdl"}$", "") + " )";
					}

					return s;
				}
			}
			return "Vous n'avez rien devant vous.";
		}

		PhysGun GetCurrentPG()
		{
			var player = Local.Pawn;
			if ( player == null ) return null;

			var inventory = player.Inventory;
			if ( inventory == null ) return null;
			if ( inventory.Active == null) return null;

			if(inventory.Active is PhysGun pg)
			{
				return pg;
			}
			return null;
		}

	[Event( "buildinput" )]
	public void ProcessClientInput( InputBuilder input )
	{
		if ( Local.Pawn is SandboxPlayer p ) {
			if ( input.MouseWheel != 0 ) {
				AddClass("hide");
				p.timeSinceRollInventory = 0.0f;
			}
		}
	}

	}

}