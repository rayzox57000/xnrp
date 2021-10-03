using Sandbox;
using System;

[Library( "ent_car_m_amg_gt3_1", Title = "Mercedes AMG GT3", Spawnable = true )]
public class M_AMG_GT3_1 : M_AMG_GT3
{
	public override void Spawn()
	{
		this.skin = 1;
		base.Spawn();
	}
}
