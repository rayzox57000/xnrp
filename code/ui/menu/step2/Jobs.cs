using Sandbox;
using Sandbox.Jobs;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.JobsMenu;
using System.Collections.Generic;

public class JobsStep : Panel
{
	public JobMenu JobMenuPanel;
	public Panel Jobs;
	public Panel Job_Description;
	public Panel Job_Description_Content;
	public Label JOB_DESCRIPTION_NAME;
	public Label JOB_DESCRIPTION_NAME_V;
	public Label JOB_DESCRIPTION_DESCRIPTION;
	public Label JOB_DESCRIPTION_DESCRIPTION_V;
	public Label JOB_DESCRIPTION_SALARY;
	public Label JOB_DESCRIPTION_SALARY_V;
	public Button JOB_BECOME;
	public List<Panel> JobsList = new();
	public static List<Label> JobCountList = new();
	public Job CurrentFocus;

	public void SetJob( Job job = null )
	{
		CurrentFocus = job;
		JOB_DESCRIPTION_NAME_V.Text = job == null ? "---" : job.Name;
		JOB_DESCRIPTION_DESCRIPTION_V.Text = job == null ? "---" : job.Description;
		JOB_DESCRIPTION_SALARY_V.Text = job == null ? "---€ / Heure" : $"{job.Salary}€ / Heure";
		JOB_BECOME.Text = "CHOISIR : " + (job == null ? "---" : job.Name.ToUpper());
		JOB_BECOME.SetClass( "DISABLED", CurrentFocus == null || job.Places == job.PlacesTaken );
		JOB_BECOME.SetClass( "HOVER", !(CurrentFocus == null || job.Places == job.PlacesTaken) );
	}

	public void GenerateJobs( List<Job> list )
	{
		Jobs.DeleteChildren( true );
		list.ForEach( ( Job Job ) =>
		{
			if ( Job.ShowInJobMenu == true )
			{
				Panel panel = Jobs.Add.Panel();
				panel.AddClass( "GLASS JOB HOVER ENABLED SHOW" );

				Label title = panel.Add.Label( Job.Name, "value" );
				title.AddClass( "JOB_TITLE TEXT" );

				Image img = panel.Add.Image( Job.Icon, "" );
				img.AddClass( "JOB_ICON" );

				int pt = Job.PlacesTaken;
				int p = Job.Places;
				int fp = p - pt;
				int percent = p <= 0 ? 100 : fp * 100 / p;
				Label freePlaces = panel.Add.Label( $"{pt}/{p}", "value" );
				string FPCLASS = percent <= 25 ? "MID" : (percent <= 75 ? "MED" : "MAX");
				freePlaces.AddClass( $"FREE_PLACES TEXT {FPCLASS}" );
				freePlaces.AddClass( $"JOBID_{Job.UniqueID}" );
				JobCountList.Add( freePlaces );

				panel.AddEventListener( "onclick", ( e ) =>
				{

					JobsList.ForEach( ( Panel panel ) => { panel.RemoveClass( "ACTIVE" ); } );

					bool AlreadyFocus = CurrentFocus == Job;

					var Target = e.Target.HasClass( "JOB" ) ? e.Target : e.Target.Parent;

					Target.SetClass( "ACTIVE", !AlreadyFocus );
					this.SetJob( AlreadyFocus ? null : Job );
				} );


				JobsList.Add( panel );
			}
		} );
	}

	public void CreateDescription()
	{
		Job_Description = Add.Panel();
		Job_Description.AddClass( "JOB_DESCRPTION" );
		Job_Description_Content = Job_Description.Add.Panel();
		Job_Description_Content.AddClass( "JOB_DESCRIPTION_CONTENT" );

		JOB_DESCRIPTION_NAME = Job_Description_Content.Add.Label( "Nom :", "value" );
		JOB_DESCRIPTION_NAME.AddClass( "JOB_DESCRIPTION_NAME DESC TEXT BOLD" );

		JOB_DESCRIPTION_NAME_V = Job_Description_Content.Add.Label( "---", "value" );
		JOB_DESCRIPTION_NAME_V.AddClass( "JOB_DESCRIPTION_NAME_V DESC TEXT" );

		JOB_DESCRIPTION_DESCRIPTION = Job_Description_Content.Add.Label( "Description :", "value" );
		JOB_DESCRIPTION_DESCRIPTION.AddClass( "JOB_DESCRIPTION_DESCRIPTION DESC TEXT BOLD" );

		JOB_DESCRIPTION_DESCRIPTION_V = Job_Description_Content.Add.Label( "---", "value" );
		JOB_DESCRIPTION_DESCRIPTION_V.AddClass( "JOB_DESCRIPTION_DESCRIPTION_V DESC TEXT" );

		JOB_DESCRIPTION_SALARY = Job_Description_Content.Add.Label( "Salaire :", "value" );
		JOB_DESCRIPTION_SALARY.AddClass( "JOB_DESCRIPTION_SALARY DESC TEXT BOLD" );

		JOB_DESCRIPTION_SALARY_V = Job_Description_Content.Add.Label( "---€ / Heure", "value" );
		JOB_DESCRIPTION_SALARY_V.AddClass( "JOB_DESCRIPTION_SALARY_V DESC TEXT" );

		JOB_BECOME = Job_Description.Add.Button( "CHOISIR : ---" );
		JOB_BECOME.AddClass( "JOB_BECOME GLASS DESC TEXT DISABLED" );

		JOB_BECOME.AddEventListener( "onclick", () =>
		{
			if ( CurrentFocus == null ) return;
				if( CurrentFocus.Activities.Count == 0 ) this.JobMenuPanel.BecomeSimple();
				else this.JobMenuPanel.ToStep();
		} );

	}


	public JobsStep( List<Job> JL, JobMenu JobMenuPanel )
	{
		JobsList = new();
		JobCountList = new();
		this.JobMenuPanel = JobMenuPanel;
		this.DeleteChildren();
		this.StyleSheet.Load( "/ui/menu/step2/Jobs.scss" );
		this.AddClass( "STEP_2" );
		Jobs = Add.Panel();
		Jobs.AddClass( "JOBS" );
		CreateDescription();
		GenerateJobs( JL );
	}
	public override void OnHotloaded()
	{
		base.OnHotloaded();
	}

}
