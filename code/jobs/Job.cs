using Sandbox.Jobs.Activity;
using System.Collections.Generic;

namespace Sandbox.Jobs
{
    public class Job : NetworkComponent
    {
      public int UniqueID {get;}
      public string Name {get;}
      public string Description {get; private set;}
      public string Icon {get; private set;}
      public int Places {get; private set;}
      public float Salary {get; private set;}
      public int PlacesTaken {get; private set;}
      public float Health {get; private set;}
      public bool ShowInJobMenu {get; private set;}
      public List<JobsActivity> Activities {get; private set;} = new();

      public Job(int UniqueID, string Name = "",string Description = "", string Icon = "", int Places = 0, float Salary = 0.0f, float Health = 100.0f,bool ShowInJobMenu = true)
      {
        this.UniqueID = UniqueID;
        this.Name = Name;
        this.Description = Description;
        this.Icon = $"materials/xnrp/job/{Icon}";
        this.Places = Places;
        this.PlacesTaken = 0;
        this.Salary = Salary;
        this.Health = Health;
        this.ShowInJobMenu = ShowInJobMenu;
      }

      public void AddActivity( JobsActivity JobsActivity ) { this.Activities.Add( JobsActivity ); }
		  public void AddBulkActivities( params JobsActivity[] list ) { foreach ( JobsActivity JobsActivity in list ) { this.AddActivity( JobsActivity ); } }


      public bool TakeJob()
      {
        var places = this.PlacesTaken;
        if(this.Places > 0 && this.Places < places++) { return false; }
        this.PlacesTaken++;
        return true;
      }

      public bool OutJob()
      {
        this.PlacesTaken--;
        this.PlacesTaken = this.PlacesTaken < 0 ? 0 : this.PlacesTaken;
        return true;
      }

    }
}