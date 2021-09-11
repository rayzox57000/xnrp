using Sandbox;
using Sandbox.Jobs;
using Sandbox.Jobs.Category;
using Sandbox.Jobs.Categories;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.JobsMenu;
using System.Collections.Generic;

namespace Sandbox.UI.Category
{
    public class CategoryStep : Panel
{

	public List<Panel> Categories = new();
	public JobsCategory FOCUS_JOBCATEGORY = null;
	public List<Job> ListJobs = new();
	public JobMenu JobMenuPanel;

	public void GenerateCategory()
	{
		this.DeleteChildren( true );
		JobsCategories JC = SandboxGame.JobsCategoriesList;
		JC.CategList.ForEach( ( Categ ) =>
		{
			int c = Categ.JobsCount;
			Panel Panel = new();
			Panel.AddClass( $"CATEGORY GLASS_HOVER_CATEGORY {(c == 0 ? "DISABLED" : "" )}" );
			Label Desc = Panel.Add.Label( $"JE SUIS LA POUR ...\n{Categ.Description}.", "value" );
			Image Icon = Panel.Add.Image( Categ.Icon, "" );
			Label Count = Panel.Add.Label( $"{c} Métier{(c > 1 ? "s" : "")} Disponible{(c > 1 ? "s" : "")}");
			Icon.AddClass( "JOB_ICON" );
			Desc.AddClass( "JOB_TITLE TEXT" );
			Count.AddClass("JOB_COUNT_VALUE TEXT");

			Panel.AddEventListener( "onclick", () => { this.FOCUS_JOBCATEGORY = Categ; this.ListJobs = Categ.JobsList; this.JobMenuPanel.ToStep(false,this.ListJobs); } );

			Categories.Add( Panel );
			this.AddChild( Panel );
		} );
	}

	public CategoryStep( JobMenu JobMenu )
	{
		JobMenuPanel = JobMenu;
		this.StyleSheet.Load( "/ui/menu/step1/Category.scss" );
		this.AddClass( "STEP_1" );
		GenerateCategory();
	}
	public override void OnHotloaded()
	{
		base.OnHotloaded();
	}

}
}