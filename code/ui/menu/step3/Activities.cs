using Sandbox;
using Sandbox.Jobs;
using Sandbox.Jobs.Activity;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.JobsMenu;
using System.Collections.Generic;

public class ActivitiesStep : Panel
{
	public JobMenu JobMenuPanel;
	public Job Job;

	public Panel JOB_PREVIEW;
	public Image JOB_PREVIEW_IMG;
	public Label JOB_PREVIEW_TITLE;
	public Panel ACTIVITIES;
	public Panel ACTIVITIES_LIST;
	public Button ACTIVITIES_BTN;
	public List<Panel> ACTIVITIES_ITEMS = new();
	public JobsActivity ACTIVITY_FOCUS = null;
  public string ACTIVITIES_BTN_DVALUE = "Ne pas choisir d'activité.";

	public void UpdateJobActivity()
	{
    string name = ACTIVITIES_BTN_DVALUE;
    if(ACTIVITY_FOCUS != null) name = $"Choisir comme activité : {ACTIVITY_FOCUS.Name}";
    ACTIVITIES_BTN.Text = name;
	}

	public bool GenerateActivities()
	{
		ACTIVITIES_LIST.DeleteChildren( true );
		if ( Job.Activities.Count == 0 ) return JobMenuPanel.ToStep();
		if ( Job.Activities is List<JobsActivity> JAL )
		{
			JAL.ForEach( ( JobsActivity JobsActivity ) =>
			{
				if ( JobsActivity is JobsActivity JA )
				{
					Log.Info("AJOUT " + JA.Name);
					Panel p = ACTIVITIES_LIST.Add.Panel( "ACTIVITY" );
					p.AddClass( "ACTIVITY GLASS HOVER TEXT" );
					Image icon = p.Add.Image( JA.Icon, "ACTIVITY_ICON" );
					icon.AddClass( "ACTIVITY_ICON" );
					Panel pc = p.Add.Panel( "ACTIVITY_CONTENT" );
					pc.AddClass( "ACTIVITY_CONTENT" );
					Label ln = pc.Add.Label( JA.Name, "value" );
					ln.AddClass( "ACTIVITY_NAME" );
					Label ld = pc.Add.Label( JA.Description, "value" );
					ld.AddClass( "ACTIVITY_DESCRIPTION" );

					p.AddEventListener( "onclick", (e) =>
					{
						ACTIVITIES_ITEMS.ForEach( ( Panel panel ) => { panel.RemoveClass( "ACTIVE" ); } );
						if ( ACTIVITY_FOCUS == JA ) ACTIVITY_FOCUS = null;
						else ACTIVITY_FOCUS = JA;
						var Target = e.Target.HasClass( "ACTIVITY" ) ? e.Target : e.Target.Parent;
						UpdateJobActivity();
					} );

					ACTIVITIES_ITEMS.Add( p );
				}
			} );
		};
    return true;
	}

	public ActivitiesStep( Job Job, JobMenu JobMenuPanel )
	{
		this.StyleSheet.Load( "/ui/menu/step3/Activities.scss" );
		this.JobMenuPanel = JobMenuPanel;
		this.Job = Job;

		JOB_PREVIEW = Add.Panel( "JOB_PREVIEW" );
		JOB_PREVIEW.AddClass( "JOB_PREVIEW" );
		JOB_PREVIEW_IMG = JOB_PREVIEW.Add.Image( this.Job.Icon, "JOB_PREVIEW_IMG" );
		JOB_PREVIEW_TITLE = JOB_PREVIEW.Add.Label( this.Job.Name, "value" );
		JOB_PREVIEW_TITLE.AddClass( "JOB_PREVIEW_TITLE TEXT" );

		ACTIVITIES = Add.Panel( "ACTIVITIES" );
		ACTIVITIES.AddClass( "ACTIVITIES" );

		ACTIVITIES_LIST = ACTIVITIES.Add.Panel( "ACTIVITIES_LIST" );
		ACTIVITIES_LIST.AddClass( "ACTIVITIES_LIST GLASS" );

		ACTIVITIES_BTN = ACTIVITIES.Add.Button("");
		ACTIVITIES_BTN.AddClass( "ACTIVITIES_BTN GLASS TEXT ACTIVE" );

		UpdateJobActivity();

		ACTIVITIES_BTN.AddEventListener("onclick", () => {
        this.JobMenuPanel.ToStep();
    });

		GenerateActivities();

	}


}
