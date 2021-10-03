using Sandbox;
using System;
using System.Linq;


	[Library( "ent_plate", Title = "Plate", Spawnable = false )]
	public partial class CarPlate : Prop
	{
		public override void Spawn()
		{
			base.Spawn();
			SetModel( "models/car/plate/car_plate.vmdl" );
		}

		private static Random random = new Random();
		public string randomPlate { get; private set; }


		public static string RandomString( int length )
		{
			const string chars = "azertyupqsdfghjklxcvbn0123456789";
			return new string( Enumerable.Repeat( chars, length )
			  .Select( s => s[random.Next( s.Length )] ).ToArray() );
		}

		public static string pathName = "models/car/plate/sym/{{name}}.vmdl";

		public static string path(char l)
		{
			return pathName.Replace( "{{name}}", l.ToString() );
		}


		public override void ClientSpawn()
		{
			string p = "models/car/plate/sym/{{name}}.vmdl";
			randomPlate = RandomString( 6 );
			char[] p1 = randomPlate.ToCharArray();

			base.ClientSpawn();
			{
				{
					var p10 = new ModelEntity();
					p10.SetModel( path( p1[0] ) );
					p10.Transform = Transform;
					p10.Parent = this;
					p10.LocalPosition = new Vector3( 0.024f, -0.2F, 0.01f ) * 40.0f;
				}
				{
					var p11 = new ModelEntity();
					p11.SetModel( path( p1[1] ) );
					p11.Transform = Transform;
					p11.Parent = this;
					p11.LocalPosition = new Vector3( 0.024f, -0.12F, 0.01f ) * 40.0f;
				}
				{
					var p12 = new ModelEntity();
					p12.SetModel( path( p1[2] ) );
					p12.Transform = Transform;
					p12.Parent = this;
					p12.LocalPosition = new Vector3( 0.024f, -0.04F, 0.01f ) * 40.0f;
				}
				{
					var p13 = new ModelEntity();
					p13.SetModel( path( p1[3] ) );
					p13.Transform = Transform;
					p13.Parent = this;
					p13.LocalPosition = new Vector3( 0.024f, 0.04F, 0.01f ) * 40.0f;
				}
				{
					var p14 = new ModelEntity();
					p14.SetModel( path( p1[4] ) );
					p14.Transform = Transform;
					p14.Parent = this;
					p14.LocalPosition = new Vector3( 0.024f, 0.12F, 0.01f ) * 40.0f;
				}
				{
					var p15 = new ModelEntity();
					p15.SetModel( path( p1[5] ) );
					p15.Transform = Transform;
					p15.Parent = this;
					p15.LocalPosition = new Vector3( 0.024f, 0.20F, 0.01f ) * 40.0f;
				}
			}

		}

		}
