using Sandbox;
using System;

[Library( "ent_car_gendarmerie", Title = "Voiture Gendarmerie", Spawnable = true )]
public class GENDARMERIE : CarEntity
{

	public override void Spawn()
	{
		this.artistName = "Paraseven";
		this.artistUrl = "https://www.cgtrader.com/paraseven";
		this.artistModelUrl = "https://www.cgtrader.com/free-3d-models/car/sport/gendarmerie-police-seat-leon";
		this.modelName = "models/car/custom/gendarmerie.vmdl";
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		this.modelNameWheelRearRight = "models/car/custom/gendarmerie_wheel_rear.vmdl";
		this.modelNameWheelRearLeft = this.modelNameWheelRearRight;
		this.modelNameWheelFrontRight = "models/car/custom/gendarmerie_wheel_front.vmdl";
		this.modelNameWheelFrontLeft = this.modelNameWheelFrontRight;

		this.FuelTankShow = false;
		this.ChassisFrontShow = false;
		this.ChassisSteeringShow = false;
		this.ChassisRearShow = false;
		this.ChassisTransmissionShow = false;

		this.ChassisFrontPosition = new Vector3( 2.15f, 0.0f, 0.48f );
		this.ChassisRearPosition = new Vector3( -1.35f, 0, 0.48f );
		this.PlateFrontPosition = new Vector3( 2.75f, 0.02f, 0.48f );
		this.PlateRearPosition = new Vector3( -3.145f, -0.0f, 0.78f );
		this.PlateRearRotation = Rotation.From( 90, -90, 90 );


		base.ClientSpawn();
	}
}
