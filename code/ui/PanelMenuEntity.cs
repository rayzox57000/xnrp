
namespace Sandbox.UI
{
	public class PanelMenuEntity<T> : PanelMenu
	{
		
		public T Entity;
		public float RadiusEnter = 20.0f;
		public bool AutoEnter = false;

		private T IsEye(SandboxPlayer p )
		{
			var eyePos = p.EyePos;
			var eyeRot = p.EyeRot;
			var tr = Trace.Ray( eyePos, eyePos + eyeRot.Forward * RadiusEnter ).Radius( 2 ).Ignore( p ).EntitiesOnly().Run();
			if ( tr.Entity is T e ) return e;
			return default( T );
		}


		public override void Tick()
		{
			if ( Local.Pawn is SandboxPlayer p )
			{
					bool ForceClose = Instance.HasClass( "SHOW" ) && (IsEye( p ) == null) ? true : false;

					if ( ForceClose == true ) Close();
					else {
						if ( ((!Instance.HasClass( "SHOW" )) && AutoEnter == true) || Input.Down( IB )) 
						{
							if ( (!Instance.HasClass( "SHOW" )) && AutoEnter == true )
							{
								if ( IsEye( p ) is T e )
								{
									Entity = e;
									Open();
								}
							}
							else
							{

								if ( p.timeSinceHudBind >= 0.250f )
								{
									p.timeSinceHudBind = 0.0f;
									bool o = (switchable == true && Instance != null && Instance.HasClass( "SHOW" )) ? false : true;


									if ( o )
									{
										if ( IsEye( p ) is T e )
										{
											Entity = e;
											Open();
										}
									}
									else
									{
										Entity = default( T );
										Close();
									}
								}
							}
						}
					}
			}
		}


	}
}
