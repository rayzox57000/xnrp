using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.UI.Construct;


namespace Sandbox.UI
{ 
	public class Demo : Panel
	{
		public Label DemoT;
		public Demo()
		{
			this.StyleSheet.Load( "/ui/demo/Demo.scss" );
			DemoT = Add.Label( "CECI EST UNE DEMO / THIS IS A DEMO ", "DEMOT" );
			DemoT.AddClass( "GLASS" );
		}
		
	}
}
