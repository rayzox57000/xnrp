using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Category;
using System.Collections.Generic;

using ATM;

namespace Sandbox.UI.AtmPanel
{
	public class AtmPanel : PanelMenuEntity<ATMEntity>
	{

		private float Fee = 10.0f;
		
    public Panel Container;
		public Panel Wallet;
		public Label WalletTitle;
		public Label WalletValue;

		public Panel WalletInput;

		public TextEntry WalletToBank;
		public Label WalletToBankAfter;
		public Button WalletTransfert;

		public Panel Bank;
		public Label BankTitle;
		public Label BankValue;

		public Panel BankInput;

		public TextEntry BankToWallet;
		public Label BankToWalletAfter;
		public Button BankTransfert;


		public AtmPanel()
    {
		this.switchable = true;
		this.AutoEnter = true;
		this.IB = InputButton.Use;
		this.RadiusEnter = 22.0f;
		this.StyleSheet.Load( "/ui/atm/base/AtmPanel.scss" );
		Container = Add.Panel("CONTAINER");
		Container.AddClass("GLASS");

			Wallet = Container.Add.Panel( "WALLET PART GLASS " );
			WalletTitle = Wallet.Add.Label( "DEPOT D'ARGENT", "TEXT" );
			WalletValue = Wallet.Add.Label( "0.00€", "TEXT" );

			WalletInput = Wallet.Add.Panel("INPUTC GLASS");

			WalletToBank = WalletInput.Add.TextEntry( "0" );
			WalletToBankAfter = WalletInput.Add.Label("€","INPUTAFTER");

			WalletTransfert = Wallet.Add.Button($"DEPOSER ({Fee}% de FRAIS DE DOSSIER)","BUTTON GLASS");

			Bank = Container.Add.Panel( "BANK PART GLASS" );
			BankTitle = Bank.Add.Label( "RETRAIT D'ARGENT", "TEXT" );
			BankValue = Bank.Add.Label( "0.00€", "TEXT" );

			BankInput = Bank.Add.Panel("INPUTC GLASS");

			BankToWallet= BankInput.Add.TextEntry( "0" );
			BankToWalletAfter = BankInput.Add.Label( "€", "INPUTAFTER" );

			BankTransfert = Bank.Add.Button("RETIRER","BUTTON GLASS");

			WalletToBank.AddClass( "INPUT" );
			BankToWallet.AddClass( "INPUT" );

			WalletToBank.Numeric = true;
			BankToWallet.Numeric = true;

			WalletTransfert.AddEventListener( "onclick", () => {
				float v = float.Parse( WalletToBank.Text );
				float f = v >= 10.0f ? float.Parse(((Fee * v)/100).ToString("n2")) : 0;
				WalletToBank.SetProperty( "value", "" );
				MT(v, false, f );
			} );

			BankTransfert.AddEventListener( "onclick", () => {
				float v = float.Parse(BankToWallet.Text);
				BankToWallet.SetProperty( "value", "" );
				MT( v, true );
				
			} );



			this.Instance = this;
    }

		public override void Tick()
		{
			if ( Local.Pawn is SandboxPlayer p )
			{
				BankValue.Text = $"SOLDE : {p.Bank}€";
				WalletValue.Text = $"SOLDE : {p.Wallet}€";
			}
			base.Tick();
		}


		[ServerCmd]
		public static void MT( float money, bool toBank, float fee = 0.0f)
		{
			if(ConsoleSystem.Caller.Pawn is SandboxPlayer p ) if ( p.AddMoney( -money, !toBank ) ) p.AddMoney( money-fee, toBank );
		}

	}
}
