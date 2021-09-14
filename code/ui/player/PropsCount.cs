using Sandbox.UI.Construct;

namespace Sandbox.UI.PropCount
{
	class PropsCount : Panel
	{
		public Label Count;
		public Label Total;

		public PropsCount()
		{
			Count = Add.Label( "00", "COUNT" );
			Total = Add.Label( "/00", "TOTAL" );
		}

		public override void Tick()
		{
			base.Tick();
			if(Local.Pawn is SandboxPlayer p)
			{
				p.UpPropCount();
				string pc = (p.PropCount > 9 ? p.PropCount.ToString() : "0" + p.PropCount);
				Count.SetText( pc );
				string pt = (p.PropTotal > 9 ? p.PropTotal.ToString() : "0" + p.PropTotal);
				Total.SetText( $"/{pt}" );
			}
		}

	}
}
