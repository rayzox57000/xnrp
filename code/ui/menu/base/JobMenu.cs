using Sandbox.Jobs;
using Sandbox.Jobs.Activity;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Category;
using System.Collections.Generic;

namespace Sandbox.UI.JobsMenu
{
	public class JobMenu : PanelMenu
	{
		public string S1_TITLE = "Bienvenue à la Mairie {{name}} !";
		public string S1_STITLE = "Step 1/3 : Sélectionnez une Catégorie.";
		public string S2_TITLE = "Vous avez choisi : {{name}} !";
		public string S2_STITLE = "Step 2/3 : Sélectionnez un Métier.";
		public string S3_TITLE = "Vous avez choisi : {{name}} !";
		public string S3_STITLE = "Step 3/3 : Sélectionnez votre Activité.";

		public Panel CONTENT;

		public Panel BASE;
		public Panel BASE_HEADER;

		public Label BASE_HEADER_TITLE;
		public Label BASE_HEADER_SUBTITLE;
		public Panel BASE_HEADER_CLOSE;
		public Panel BASE_HEADER_BACK;

		public Panel BASE_CONTAINER;

		public CategoryStep STEP_1;
		public JobsStep STEP_2;
		public ActivitiesStep STEP_3;

		public bool ClearStepContainer()
		{
			BASE_CONTAINER.DeleteChildren( true );
			return true;
		}

		public bool ShowHideBtn()
		{
			BASE_HEADER_BACK.AddClass( "SHOW" );
			return true;
		}

		public bool HideBackBtn()
		{
			BASE_HEADER_BACK.RemoveClass( "SHOW" );
			return true;
		}

		public void SetHeaderTitle( string t = "", string st = "" )
		{
			BASE_HEADER_TITLE.Text = t;
			BASE_HEADER_SUBTITLE.Text = st;
		}

		public bool PanelUpdate( Panel p, string t, string st, bool showBack = false )
		{
			ClearStepContainer();
			BASE_CONTAINER.AddChild( p );
			SetHeaderTitle( t, st );
			return showBack ? ShowHideBtn() : HideBackBtn();
		}

		public override bool Close()
		{
			base.Close();
			return STEP_PROCESS_2( true, null );
		}

		public bool BecomeWithActivity()
		{
			int ACTIVITY_ID = STEP_3.ACTIVITY_FOCUS == null ? -1 : STEP_3.ACTIVITY_FOCUS.UniqueID;
			ConsoleSystem.Run( "BecomeJob", STEP_3.Job.UniqueID, ACTIVITY_ID );
			return Close();
		}

		public bool BecomeSimple()
		{
			ConsoleSystem.Run( "BecomeJob", STEP_2.CurrentFocus.UniqueID );
			return Close();
		}


		public bool STEP_PROCESS_1( List<Job> ListJobs = null )
		{
			if ( ListJobs == null ) return STEP_PROCESS_2( true );
			STEP_2 = new( ListJobs, this );
			return PanelUpdate( STEP_2, S2_TITLE.Replace("{{name}}",STEP_1.FOCUS_JOBCATEGORY.Name), S2_STITLE, true );
		}

		public bool STEP_PROCESS_2( bool GoBack = false, List<Job> ListJobs = null )
		{
			Panel panel;
			if ( GoBack == false && STEP_2 != null && (STEP_2.CurrentFocus != null) )
			{
				STEP_3 = new( STEP_2.CurrentFocus, this );
				panel = STEP_3;

			}
			else if ( GoBack == false && (STEP_2 == null || STEP_2.CurrentFocus == null) ) return STEP_PROCESS_1( ListJobs );
			else
			{
				if ( STEP_1 == null ) STEP_1 = new( this );
				panel = STEP_1;
			}

			string title = S1_TITLE; string subtitle = S1_STITLE; bool showback = false;
			if ( STEP_3 != null && panel == STEP_3 ) { title = S3_TITLE.Replace("{{name}}",STEP_2.CurrentFocus.Name); subtitle = S3_STITLE; showback = true; }

			return PanelUpdate( panel, title, subtitle, showback );
		}

		public bool STEP_PROCESS_3( bool GoBack = false, List<Job> ListJobs = null )
		{
			if ( GoBack == false && STEP_3 != null && STEP_3.Job != null ) return BecomeWithActivity();
			if ( GoBack == true && STEP_1 != null && STEP_1.ListJobs != null ) return STEP_PROCESS_1( STEP_1.ListJobs );
			return STEP_PROCESS_2( false, ListJobs );
		}

		public bool ToStep( bool GoBack = false, List<Job> ListJobs = null )
		{

			bool IS_STEP_2 = BASE_CONTAINER.GetChildIndex( STEP_2 ) != -1;
			bool IS_STEP_3 = BASE_CONTAINER.GetChildIndex( STEP_3 ) != -1;

			if ( IS_STEP_3 ) return STEP_PROCESS_3( GoBack, ListJobs );
			if ( IS_STEP_2 ) return STEP_PROCESS_2( GoBack, ListJobs );
			return STEP_PROCESS_1( ListJobs );
		}

		public JobMenu()
		{

			this.IB = InputButton.Slot0;

			this.StyleSheet.Load( "/ui/menu/base/JobMenu.scss" );
			this.AddClass("SHOW");
			CONTENT = Add.Panel( "CONTENT" );
			CONTENT.AddClass( "CONTENT SHOW" );
			// BASE
			BASE = CONTENT.Add.Panel( "BASE" );
			BASE.AddClass( "GLASS BASE" );
			// BASE_HEADER
			BASE_HEADER = BASE.Add.Panel( "BASE_HEADER" );
			BASE_HEADER.AddClass( "BASE_HEADER" );
			BASE_HEADER_TITLE = BASE_HEADER.Add.Label( "", "value" );
			BASE_HEADER_TITLE.AddClass( "BASE_HEADER_TITLE TEXT" );
			BASE_HEADER_SUBTITLE = BASE_HEADER.Add.Label( "", "value" );
			BASE_HEADER_SUBTITLE.AddClass( "BASE_HEADER_SUBTITLE TEXT" );
			BASE_HEADER_CLOSE = BASE_HEADER.Add.Button( "X" );
			BASE_HEADER_CLOSE.AddClass( "BASE_HEADER_CLOSE GLASS GLASS_HOVER TEXT" );
			BASE_HEADER_BACK = BASE_HEADER.Add.Button( "🢀" );
			BASE_HEADER_BACK.AddClass( "BASE_HEADER_BACK GLASS GLASS_HOVER TEXT" );

			BASE_HEADER_CLOSE.AddEventListener( "onclick", () => { Close(); } );
			BASE_HEADER_BACK.AddEventListener( "onclick", () => { ToStep( true ); } );

			// BASE CONTAINER
			BASE_CONTAINER = BASE.Add.Panel( "BASE_CONTAINER" );

			this.Instance = this;

			// INIT STEPS
			ToStep();
		}
	}
}
