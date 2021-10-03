using Sandbox;
using System;


public partial class CarEntity : Prop, IUse
{

	private string locksound = "switch";
	private string unlocksound = "switch";

	public enum CarState
	{
		LOCK = 1,
		UNLOCK = 2
	};
	public CarState State { get; private set; } = CarState.UNLOCK;

	public void Switch()
	{
		if ( IsClient ) return;
		this.State = this.State == CarState.UNLOCK ? CarState.LOCK : CarState.UNLOCK;
		Sound.FromEntity(this.State == CarState.LOCK ? locksound : unlocksound, this);
	}

	[Event.Tick.Server]
	protected void TickVehicle()
	{
		if ( driver is SandboxPlayer player ) if ( this.State == CarState.LOCK ) { RemoveDriver( player ); }
	}

	

}
