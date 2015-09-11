package com.malow.carnicusserver;

import com.malow.malowlib.Vector3;

public class Projectile {
	public static final int MELEE_ATTACK = 0;
	public static final int ARROW = 10; 
	
	public Projectile(int type, long attackerGUI, float posx, float posy, float posz,
			float speed, float maxTime, float dirx, float diry, float dirz) {
		this.GUID = Projectile.ProjectileGUID++;
		
		this.type = type;
		this.attackerGUI = attackerGUI;
		this.pos = new Vector3(posx, posy, posz);
		this.speed = speed;
		this.maxTime = maxTime;
		this.dir = new Vector3(dirx, diry, dirz);
		this.dir.normalize();
		this.flownTime = 0.0f;
	}
	
	public static long ProjectileGUID = 0;

	public int type;
	public long GUID;
	public long attackerGUI;
	public Vector3 pos;
	public float speed;
	public float maxTime;
	public Vector3 dir;
	public float flownTime;
}
