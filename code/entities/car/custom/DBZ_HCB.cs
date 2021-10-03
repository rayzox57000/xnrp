using Sandbox;
using System;

[Library( "ent_car_dbz_hcb", Title = "DBZ HoverCar", Spawnable = true )]
public class DBZ_HCB : CarEntity
{

	public override void Spawn()
	{
		this.artistName = "nataliedesign";
		this.artistUrl = "https://nataliedesign.com";
		this.artistModelUrl = "https://sketchfab.com/3d-models/dragon-ball-hover-car-black-8d85afecc08d43ec8c9f07003e1ccda8";
		this.modelName = "models/car/custom/dbz_hcb.vmdl";
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		this.PlateGlobalShow = false;
		this.FuelTankShow = false;
		this.ChassisFrontShow = false;
		this.WheelGlobalShow = false;
		this.WheelFrontRightShow = false;
		this.WheelFrontLeftShow = false;
		this.ChassisSteeringShow = false;
		this.ChassisRearShow = false;
		this.ChassisTransmissionShow = false;
		this.WheelRearLeftShow = false;
		this.WheelRearRightShow = false;
		base.ClientSpawn();
	}
}
