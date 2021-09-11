
namespace Sandbox.UI
{
	public class PanelMenu : Panel
	{

		public Panel Instance;
		public InputButton IB;
		public bool switchable = false;
		

		public void Open()
		{
			if ( Instance != null && (!Instance.HasClass( "SHOW" )) ) Instance.AddClass( "SHOW" );
		}

		public virtual bool Close()
		{
			if ( Instance != null && Instance.HasClass( "SHOW" ) ) Instance.RemoveClass( "SHOW" );
			return true;
		}

		public override void Tick()
		{
			base.Tick();
			if ( Local.Pawn is SandboxPlayer p )
			{
				if ( IB is InputButton I )
				{
						if ( Input.Down( I ) )
						{
							if ( p.timeSinceHudBind >= 0.250f )
							{
								p.timeSinceHudBind = 0.0f;
								bool o = (switchable == true && Instance != null && Instance.HasClass("SHOW")) ? false : true;


								if ( o ) Open();
								else Close();
						}
					}
				}
			}
		}


	}
}
