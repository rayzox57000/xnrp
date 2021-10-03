using Sandbox;
using System;

[Library( "ent_car_m_amg_gt3_2", Title = "Mercedes AMG GT3", Spawnable = true )]
public class M_AMG_GT3_2 : M_AMG_GT3
{
	public override void Spawn()
	{
		this.skin = 2;
		base.Spawn();
	}
}
