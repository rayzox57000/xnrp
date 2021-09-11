
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class InventoryIcon : Panel
{
	public Panel panel;
	public Entity TargetEnt;
	public Label Label;
	public Label Number;

	public InventoryIcon( int i, Panel parent )
	{
		Parent = parent;
		panel = Parent;
		Label = Add.Label( "empty", "item-name" );
		Number = Add.Label( $"{i}", "slot-number" );
		AddClass("glass");
	}

	public void Clear()
	{
		Label.Text = "";
		SetClass( "active", false );
	}

	[Event( "buildinput" )]
	public void ProcessClientInput( InputBuilder input )
	{
		if ( Local.Pawn is SandboxPlayer p ) {
			if ( input.MouseWheel != 0 ) {
				AddClass("enlarge");
				Log.Info("SCROLL");
				p.timeSinceRollInventory = 0.0f;
			}
		}
	}

	public override void Tick()
	{
		base.Tick();
		var player = Local.Pawn;
		if ( player is SandboxPlayer p ) {
				if (p.timeSinceRollInventory >= 2.0f) {
					RemoveClass("enlarge");
				}			
		}
	}

}
