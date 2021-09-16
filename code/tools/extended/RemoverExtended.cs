
namespace Sandbox.Tools
{
	public partial class RemoverTool : BaseTool
	{
	private void AddMoney(SandboxPlayer p, Entity e)
		{
			if ( e is Prop pe && pe != null && (pe.Owner == p) ) p.AddMoney( 02.5f );
		}
	}
}
