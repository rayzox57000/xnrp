using Sandbox;
using Sandbox.Tools;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Sandbox.UI.FlashLight
{
	public class FlashLight : Panel
	{
		public Label Title;

		public FlashLight()
		{
			Title = Add.Label( "FlashLight", "title" );
		}

		public override void Tick()
		{
			Flashlight flashlight = GetCurrentFL();
			SetClass( "active", flashlight != null );

			if ( flashlight != null ) {
				bool isOn = flashlight.IsOn();
				SetClass("DarkMode", !isOn);
			}

			var player = Local.Pawn;
			if ( player is SandboxPlayer p ) {
				if (p.timeSinceRollInventory >= 2.0f) {
					RemoveClass("hide");
				}		
			}
			
		}

		Flashlight GetCurrentFL()
		{
			var player = Local.Pawn;
			if ( player == null ) return null;

			var inventory = player.Inventory;
			if ( inventory == null ) return null;
			if ( inventory.Active == null) return null;

			if(inventory.Active is Flashlight flashlight)
			{
				return flashlight;
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