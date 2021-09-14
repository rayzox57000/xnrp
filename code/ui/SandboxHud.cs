using Sandbox;
using Sandbox.UI;
using Sandbox.UI.AtmPanel;
using Sandbox.UI.JobsMenu;
using Sandbox.UI.CurrentTool;
using Sandbox.UI.FlashLight;
using Sandbox.UI.Pg;
using Sandbox.UI.Keys;

[Library]
public partial class SandboxHud : HudEntity<RootPanel>
{

	public static JobMenu JobMenu {get; private set;}

	public SandboxHud()
	{
		if ( !IsClient )
			return;

		RootPanel.StyleSheet.Load( "/ui/SandboxHud.scss" );

		JobMenu = new();

		RootPanel.AddChild<NameTags>();
		RootPanel.AddChild<CrosshairCanvas>();
		RootPanel.AddChild<HudInfo>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<KillFeed>();
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
		RootPanel.AddChild<InventoryBar>();
		RootPanel.AddChild<CurrentTool>();
		RootPanel.AddChild<FlashLight>();
		RootPanel.AddChild<Pg>();
		RootPanel.AddChild<Keys>();
		RootPanel.AddChild<SpawnMenu>();
		RootPanel.AddChild(JobMenu);
		RootPanel.AddChild<AtmPanel>();
		RootPanel.AddChild<Demo>();
	}
}
