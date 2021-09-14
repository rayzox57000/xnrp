using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Sandbox
{
	[Library( "ent_door" )]
	[Hammer.Model]
	[Hammer.SupportsSolid]
	[Hammer.DoorHelper( "movedir", "movedir_islocal", "movedir_type", "distance" )]
	[Hammer.RenderFields]
	public partial class RpDoorEntity : DoorEntity
	{

		public enum Flags
		{
			UseOpens = 1,
			StartLocked = 2,
		};

		[Net] public Player DoorOwner { get; private set; }
		[Net] public List<Player> Flatmates { get; private set; }


		public override bool OnUse( Entity user )
		{
			bool v = base.OnUse( user );
			if(State == DoorState.Open) Log.Error( "DEBUG : OPEN OK !!!" );
			if ( State == DoorState.Closed ) Log.Error( "DEBUG : CLOSE OK !!!" );
			return v;
		}

		public override void Spawn()
		{
			base.Spawn();
			LockedSound = "sounds/door/lock.mp3";
			CloseSound = "sounds/door/close.mp3";
			OpenSound = "sounds/door/open.mp3";
			TimeBeforeReset = -1;
		}

		public void SetDoorOwner(Player p)
		{
			this.DoorOwner = p;
		}

		public void PlayLock()
		{
			PlaySound( LockedSound );
			SetAnimBool( "locked", true );
		}

	}
}
