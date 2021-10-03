using Sandbox;
using System;

[Library( "ent_car_ferrari_f1", Title = "Ferrari F1", Spawnable = true )]
public class FERRARI_F1 : CarEntity
{

	public override void Spawn()
	{
		this.artistName = "dilafroze";
		this.artistUrl = "https://sketchfab.com/dilafroze";
		this.artistModelUrl = "https://c4ddownload.com/ferrari-formula-1-car-3d-model/";
		this.modelName = "models/car/custom/ferrari_f1.vmdl";
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		this.modelNameWheelRearRight = "models/car/custom/ferrari_f1_wheel.vmdl";
		this.modelNameWheelRearLeft = this.modelNameWheelRearRight;
		this.modelNameWheelFrontRight = this.modelNameWheelRearRight;
		this.modelNameWheelFrontLeft = this.modelNameWheelFrontRight;

		this.FuelTankShow = false;
		this.ChassisFrontShow = false;
		this.ChassisSteeringShow = false;
		this.ChassisRearShow = false;
		this.ChassisTransmissionShow = false;

		this.ChassisFrontPosition = new Vector3( 1.5f, 0.0f, 0.3f );
		this.ChassisRearPosition = new Vector3( -1.65f, 0, 0.3f );
		this.PlateFrontPosition = new Vector3( 1.74f, 0.02f, 0.28f );
		this.PlateRearPosition = new Vector3( -3.01f, -0.0f, 0.28f );
		this.PlateRearRotation = Rotation.From( 90, -90, 90 );


		base.ClientSpawn();
	}
}
