using Sandbox.UI.Construct;
using System;
using System.Linq;

namespace Sandbox.UI.PropCount
{
	class PropCount : Panel
	{
		public Label Count;
		public Label Total;
		public Panel SinceNextSpawn;

		public PropCount()
		{
			AddClass("active");
			Count = Add.Label( "00", "COUNT" );
			Total = Add.Label( "/00", "TOTAL" );
			SinceNextSpawn = Add.Panel( "SINCENEXTSPAWN" );
		}


		public override void Tick()
		{
			base.Tick();
			if(Local.Pawn is SandboxPlayer p)
			{
				int propcount = p.PropTotal == -1 ? -1 : Entity.All.OfType<Prop>().Where( e => (SandboxPlayer)e.Owner == p ).Count();

				string pc = "Props : " + (propcount == -1 ? "" : (propcount > 99 ? propcount.ToString() : (( propcount < 10 ? "00" : "0") + propcount)));
				string pt = (propcount == -1 ? "INFINI" : ("/" +  (p.PropTotal > 99 ? p.PropTotal.ToString() : (( p.PropTotal < 10 ? "00" : "0") + p.PropTotal))));

				SinceNextSpawn.SetClass( "hide", p.ClientTimeSincePropSpawn >= 5.95f );
				float radius = (p.ClientTimeSincePropSpawn * 100)/6.0f;
				float fr = radius > 100 ? 100 : radius;
				SinceNextSpawn.SetProperty( "style", $"width:{100-fr}%;" );

				if ( p.ClientTimeSincePropSpawn < 5.95f ) {
					pc = $"AntiSpam {Math.Round( 6.5 - (Double)p.ClientTimeSincePropSpawn, 0 )}sec";
					pt = "";
				}



				Count.SetText( pc );
				Total.SetText( pt );
			}
		}

	}
}
