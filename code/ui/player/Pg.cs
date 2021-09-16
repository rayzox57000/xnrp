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
		public Label Position;
		public Label Angles;
		public Label Owner;

		public Pg()
		{
			Title = Add.Label( "PhysGun", "title" );
			Description = Add.Label("", "description");
			Position = Add.Label("Position : ---","position");
			Angles = Add.Label("Angles : ---","angles");
			Owner = Add.Label("Owner : ---","owner");
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
						AddClass("focused");
						var m = pr.GetModelName();
						var mn = m.Substring(m.LastIndexOf("/")+1);
						s += " ( Modèle : " + Regex.Replace(mn, $"{".vmdl"}$", "") + " )";
					}

					Vector3 ps = ent.Position;
					Position.SetText( $"Position : x: {ps.x.ToString( "0.00" )} y: {ps.y.ToString( "0.00" )} z: {ps.z.ToString( "0.00" )}" );
					Rotation an = ent.Rotation;
					Angles.SetText( $"Angles : x: {an.x.ToString( "0.00" )} y: {an.y.ToString( "0.00" )} z: {an.z.ToString( "0.00" )}" );
					string owner = "World";

					if(ent.Owner is SandboxPlayer sp) owner = sp.GetClientOwner().Name;

					Owner.SetText( $"Owner : {owner}" );

					return s;
				}
			}
			Position.SetText( "Position : ---" );
			Angles.SetText( "Angles : ---" );
			Owner.SetText( "Owner : ---" );
			RemoveClass("focused");
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
