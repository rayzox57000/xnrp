using Sandbox;
using Sandbox.Tools;
using Sandbox.Jobs;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class HudInfo : Panel
{
	public Panel Panel;
  public Label JobName;
  public Label JobActivityName;
  public Label WalletValue;
  public Label BankValue;
  public Panel Health;

  public Image Avatar;

  public Label HealthV;

	public HudInfo()
	{
    this.StyleSheet.Load( "/ui/player/HudInfo.scss" );
		Panel = Add.Panel();
    Panel.AddClass("GLASS XNRP_HUD_INFO");

    JobName = Panel.Add.Label("","value");
    JobName.AddClass("TEXT XNRP_JOBNAME");

    JobActivityName = Panel.Add.Label("","value");
    JobActivityName.AddClass("TEXT XNRP_JOBACTIVITYNAME");

    WalletValue = Panel.Add.Label("","value");
    WalletValue.AddClass("TEXT XNRP_WALLETVALUE");

    BankValue = Panel.Add.Label("","value");
    BankValue.AddClass("TEXT XNRP_BANKVALUE");

    Health = Panel.Add.Panel();
    Health.AddClass("GLASS XNRP_HEALTH");

    HealthV = Panel.Add.Label("","value");
    HealthV.AddClass("TEXT XNRP_HEALTHV");

    Avatar = Panel.Add.Image( $"avatar:{Local.SteamId}" );
    Avatar.AddClass("GLASS XNRP_AVATAR");

	}

	public override void Tick()
	{      
		var player = Local.Pawn;
		if ( player is SandboxPlayer p ) {
      JobName.Text = $"Métier : {p.JobName}";

      string jname = p.JobActivityName == null || p.JobActivityName == "" ? "Aucune" : p.JobActivityName;
      jname = jname.Length <= 15 ? jname : (jname.Substring(0, 15) + " [...]"); 

      JobActivityName.Text = $"Activité : {jname}";

      string w = p.Wallet.ToString("n2");
      WalletValue.Text = $"Wallet : {w}€";

      string b = p.Bank.ToString("n2");
      BankValue.Text = $"Bank : {b}€";

      int health = player.Health.CeilToInt();
      health = health > 100 ? 100 : (health < 0 ? 0 : health);
      Health.SetProperty( "style", $"width:{health}%; border-radius: 0 0 {(health < 100 ? "0" : "10px")} 10px; background-color: rgba({255-(health*2.55f)},{health*2.55f},0,{(health == 0 ? 0 : 1)});" );
      HealthV.Text = "HEALTH : " + player.Health.CeilToInt().ToString() + "HP";

    }
	}

	public override void OnHotloaded()
	{
		base.OnHotloaded();
  }

}
