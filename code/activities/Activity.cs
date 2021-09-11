using Sandbox.Jobs;
using System.Collections.Generic;

namespace Sandbox.Jobs.Activity
{
	public class JobsActivity : NetworkComponent
	{
		public int UniqueID { get; }
		public string Name { get; }
		public string Description { get; }
		public string Icon { get; }

		public JobsActivity( int UniqueID, string Name = "", string Description = "", string Icon = "" )
		{
			this.UniqueID = UniqueID;
			this.Name = Name;
			this.Description = Description;
			this.Icon = $"materials/xnrp/act/{Icon}";
		}

	}
}