namespace Sandbox
{
	partial class PropExtended : Prop
	{

		public SandboxPlayer Owner { get; private set; }

		public void setOwner(SandboxPlayer o)
		{
			Owner = o;
		}


		public override void Spawn()
		{
			base.Spawn();

		}

	}
}
