using Sandbox.Jobs;
using System.Collections.Generic;

namespace Sandbox.Jobs.Category
{
	public class JobsCategory : NetworkComponent
	{
		public string Name { get; }
		public string Description { get; }
		public string Icon { get; }
		public List<Job> JobsList { get; }
    public int JobsCount {get; private set;}

		public JobsCategory( string Name, string Description = "", string Icon = "" )
		{
			this.Name = Name;
			this.Description = Description;
			this.Icon = $"materials/xnrp/categ/{Icon}";
			this.JobsList = new();
		}

		public void AddJob( Job job ) { this.JobsList.Add( job ); if(job.ShowInJobMenu == true) this.JobsCount++; }
		public void AddBulkJobs( params Job[] list ) { foreach ( Job job in list ) { this.AddJob( job ); } }

	}
}