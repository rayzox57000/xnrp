using Sandbox;
using System;

[Library( "ent_car_m_amg_gt3", Title = "Car2", Spawnable = false )]
public class M_AMG_GT3 : CarEntity
{

	public override void Spawn()
	{
		this.artistName = "Paraseven";
		this.artistUrl = "https://www.cgtrader.com/paraseven";
		this.artistModelUrl = "https://www.cgtrader.com/free-3d-models/car/racing/mercedes-amg-gt3-b17b8b0e-beff-4387-a008-0a0e2c46c221";
		this.modelName = "models/car/custom/mercedes_amg_gt3_base_.vmdl";
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		this.modelNameWheelRearRight = "models/car/custom/mercedes_amg_gt3_wheel_.vmdl";
		this.modelNameWheelRearLeft = this.modelNameWheelRearRight;
		this.modelNameWheelFrontRight = this.modelNameWheelRearRight;
		this.modelNameWheelFrontLeft = this.modelNameWheelRearRight;

		/*this.FuelTankShow = false;
		this.ChassisFrontShow = false;
		this.ChassisSteeringShow = false;
		this.ChassisRearShow = false;
		this.ChassisTransmissionShow = false;*/

		this.ChassisFrontPosition = new Vector3( 1.77f, 0.0f, 0.45f );
		this.ChassisRearPosition = new Vector3( -1.6f, 0, 0.45f );
		this.PlateFrontPosition = new Vector3( 2.2f, 0.02f, 0.5f );
		this.PlateRearPosition = new Vector3( -3.55f, -0.0f, 0.7f );
		this.PlateRearRotation = Rotation.From( 90, -90, 90 );


		base.ClientSpawn();
	}
}
